using System;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;

namespace MIDRetail.Business.Allocation
{
	/// <summary>
	/// Summary description for AllocationWareHouseSizeMethod.
	/// </summary>
	[Serializable]
	public class AllocationWareHouseSizeMethod:AllocationBaseMethod
	{
		//=======
		// FIELDS
		//=======
		private object _methodObj;  // address of data layer method object
		// To DO:  define fields and appropriate properties and methods.

		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates an instance of the Business Allocation Warehouse Size Method.
		/// </summary>
		/// <param name="aSAB" >Session Address Block.</param>
		/// <param name="aMethodRID">RID that identifies the method.</param>
		//Begin TT#523 - JScott - Duplicate folder when new folder added
		//public AllocationWareHouseSizeMethod(SessionAddressBlock aSAB, int aMethodRID):base(aSAB, aMethodRID, eMethodType.WarehouseSizeAllocation)
		public AllocationWareHouseSizeMethod(SessionAddressBlock aSAB, int aMethodRID):base(aSAB, aMethodRID, eMethodType.WarehouseSizeAllocation, eProfileType.MethodWarehouseSizeAllocation)
		//End TT#523 - JScott - Duplicate folder when new folder added
		{
			_methodObj = null;
			if (base.Filled)
			{
				// ToDo:  populate datalayer object and copy info to this object.
                // _methodObj = new Object(base.Key);
			}
			else
			{
	        	// To do:  fill in default values .
			}
		}

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets Profile Type for this profile
		/// </summary>
		public override eProfileType ProfileType
		{
			get
			{
				return eProfileType.MethodWarehouseSizeAllocation;
			}
		}


		//  To Do.

		//========
		// METHODS
		//========
		public override void ProcessMethod(
			ApplicationSessionTransaction aApplicationTransaction,
			int aStoreFilter, Profile methodProfile)
		{
			foreach (AllocationProfile ap in (AllocationProfileList)aApplicationTransaction.GetMasterProfileList(eProfileType.Allocation))
			{
				AllocationWorkFlowStep awfs = 
					new AllocationWorkFlowStep(
					this,
					new GeneralComponent(eGeneralComponentType.Total),
					false,
					true,
//					aApplicationTransaction.SAB.ApplicationServerSession.GlobalOptions.BalanceTolerancePercent,
					aApplicationTransaction.GlobalOptions.BalanceTolerancePercent,
					aStoreFilter,
					-1);
				this.ProcessAction(
					aApplicationTransaction.SAB,
					aApplicationTransaction,
					awfs,
					ap,
					true,
					Include.NoRID);
			}
		}

		public override void ProcessAction(
			SessionAddressBlock aSAB, 
			ApplicationSessionTransaction aApplicationTransaction, 
			ApplicationWorkFlowStep aAllocationWorkFlowStep, 
			Profile aAllocationProfile,
			bool WriteToDB,
			int aStoreFilterRID)
		{
			if (!Enum.IsDefined(typeof(eAllocationMethodType),aAllocationWorkFlowStep._method.MethodType))
			{
				// throw exception invalid work flow bin
			}
			if (eProfileType.Allocation != aAllocationProfile.ProfileType)
			{
				//throw exception invalid profile
			}
			AllocationProfile ap = (AllocationProfile) aAllocationProfile;
			if (!ap.AllocationStarted)
			{
//				ap.ShipToCdrRID = _shipToDayCdrRID;
//				ap.BeginCdrRID = _beginDayCdrRID;
//				ap.PlanHnRID = _OTSPlan.MdseHnRID;
//				if (_OTSPlan.ProductHnLvlRID >0)
//				{
//					// identify the hierarchy plan node and replace ap.PlanHnRID with it.
//				}
//				// process reserve amount.
//				// remember overrides affect the decision here. must wait to see if we have one.
			}
            // Begin TT#1966-MD - JSmith- DC Fulfillment
            else if (ap.IsMasterHeader && ap.DCFulfillmentProcessed)
            {
                string errorMessage = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed), ap.HeaderID);
                aSAB.ApplicationServerSession.Audit.Add_Msg(
                    eMIDMessageLevel.Warning,
                    eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed,
                    this.Name + " " + errorMessage,
                    this.GetType().Name);
                aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
            }
            else if (ap.IsSubordinateHeader && !ap.DCFulfillmentProcessed)
            {
                string errorMessage = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed), ap.HeaderID);
                aSAB.ApplicationServerSession.Audit.Add_Msg(
                    eMIDMessageLevel.Warning,
                    eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed,
                    this.Name + " " + errorMessage,
                    this.GetType().Name);
                aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
            }
            // End TT#1966-MD - JSmith- DC Fulfillment
			else
			{
				// begin MID Track 5778 - Scheduler "Run Now" feature gets Error in Audit
				//aSAB.ApplicationServerSession.Audit.Add_Msg(
				//	eMIDMessageLevel.Warning,
				//	eMIDTextCode.msg_GeneralHeaderIgnored,"AllocationGeneralMethod");
				aSAB.ApplicationServerSession.Audit.Add_Msg(
					eMIDMessageLevel.Warning,
					eMIDTextCode.msg_MethodIgnored,
					string.Format(
					MIDText.GetText(eMIDTextCode.msg_MethodIgnored),
					ap.HeaderID, 
					MIDText.GetTextOnly(eMIDTextCode.frm_SizeWarehouseMethod), 
					this.Name),
					this.GetType().Name);
				// end MID Track 5778 - Scheduler "Run Now" feature gets Error in Audit
			}

		}

		/// <summary>
		/// Creates or updates method on the database.
		/// </summary>
		new public void Update(TransactionData td)
		{
			bool create = false;
			if (_methodObj == null || base.Key < 0 || base.Method_Change_Type == eChangeType.add)
			{
				_methodObj = new Object(); // new copy of data layer object
				create = true;
			}
            //  To do copy fields from this.object to _methodObj
			try
			{
				base.Update(td);
				if (create)
				{
					// TO DO
					// _methodObj.Create();  create new instance of this method on database 
				}
				else
				{
					// TO DO
					// _methodObj.UpdateMethod(); update existing instance on database
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				//TO DO:  whatever has to be done after an update or exception.
			}
		}

		public override bool WithinTolerance(double aTolerancePercent)
		{
			return true;
		}

		/// <summary>
		/// Returns a copy of this object.
		/// </summary>
		/// <param name="aSession">
		/// The current session
		/// </param>
		/// <param name="aCloneDateRanges">
		/// A flag identifying if date ranges are to be cloned or use the original</param>
        /// <param name="aCloneCustomOverrideModels">
        /// A flag identifying if custom override models are to be cloned or use the original
        /// </param>
		/// <returns>
		/// A copy of the object.
		/// </returns>
        // Begin Track #5912 - JSmith - Save As needs to clone custom override models
        //override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges)
        override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges, bool aCloneCustomOverrideModels)
        // End Track #5912
		{
			AllocationWareHouseSizeMethod newAllocationWareHouseSizeMethod = null;

			try
			{
				newAllocationWareHouseSizeMethod = (AllocationWareHouseSizeMethod)this.MemberwiseClone();
				return newAllocationWareHouseSizeMethod;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// Begin MID Track 4858 - JSmith - Security changes
		/// <summary>
		/// Returns a flag identifying if the user can update the data on the method.
		/// </summary>
		/// <param name="aSession">
		/// The current session
		/// </param>
		/// <param name="aUserRID">
		/// The internal key of the user</param>
		/// <returns>
		/// A flag.
		/// </returns>
		override public bool AuthorizedToUpdate(Session aSession, int aUserRID)
		{
			return true;
		}
		// End MID Track 4858
	}
}
