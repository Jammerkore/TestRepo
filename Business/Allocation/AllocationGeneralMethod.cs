using System;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business.Allocation
{
	/// <summary>
	/// Defines the general criteria that drives the allocation process.
	/// </summary>
	/// <remarks>
	/// </remarks>
	public class AllocationGeneralMethod:AllocationBaseMethod
	{
		//=======
		// FIELDS
		//=======
		private GeneralAllocationMethodData _methodData;
		private int _Begin_CDR_RID;
		private int _Ship_To_CDR_RID;
		private int _Merch_HN_RID;
		private int _Merch_PH_RID;
		private int _Merch_PHL_Sequence;
		private int _Gen_Alloc_HDR_RID;
		private double _Reserve;
		private bool _Percent_Ind;
		private eMerchandiseType _merchandiseType;
		// BEGIN TT#667 - Stodd - Pre-allocate Reserve
		private double _reserveAsBulk;
		private double _reserveAsPacks;
		// END TT#667 - Stodd - Pre-allocate Reserve

		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates an instance of the Business Allocation General Method.
		/// </summary>
		/// <param name="SAB">Session Address Block.</param>
		/// <param name="aMethodRID">RID that identifies this method.</param>
		//Begin TT#523 - JScott - Duplicate folder when new folder added
		//public AllocationGeneralMethod(SessionAddressBlock SAB, int aMethodRID): base (SAB, aMethodRID, eMethodType.GeneralAllocation)
		public AllocationGeneralMethod(SessionAddressBlock SAB, int aMethodRID): base (SAB, aMethodRID, eMethodType.GeneralAllocation, eProfileType.MethodGeneralAllocation)
		//End TT#523 - JScott - Duplicate folder when new folder added
		{
			if (base.Filled)
			{
				_methodData = new GeneralAllocationMethodData(base.Key, eChangeType.populate);
				_Ship_To_CDR_RID = _methodData.Ship_To_CDR_RID;
				_Begin_CDR_RID = _methodData.Begin_CDR_RID;
				_Percent_Ind = Include.ConvertCharToBool(_methodData.Percent_Ind);
				_Reserve = _methodData.Reserve;
				_Merch_HN_RID = _methodData.Merch_HN_RID;
				_Merch_PH_RID = _methodData.Merch_PH_RID;
				_Merch_PHL_Sequence = _methodData.Merch_PHL_SEQ;
				_Gen_Alloc_HDR_RID = _methodData.Gen_Alloc_HDR_RID;
				_merchandiseType = _methodData.MerchandiseType;
				// BEGIN TT#667 - Stodd - Pre-allocate Reserve
				_reserveAsBulk = _methodData.ReserveAsBulk;
				_reserveAsPacks = _methodData.ReserveAsPacks;
				// END TT#667 - Stodd - Pre-allocate Reserve

			}
			else
			{
				_Ship_To_CDR_RID = Include.NoRID;
				_Begin_CDR_RID = Include.NoRID;
				_Percent_Ind = false;
				_Reserve = Include.UndefinedReserve;
				_Merch_HN_RID = Include.DefaultPlanHnRID;
				_Merch_PH_RID = Include.NoRID;
				_Merch_PHL_Sequence = 0;
				_Gen_Alloc_HDR_RID = Include.NoRID;
				_merchandiseType = eMerchandiseType.OTSPlanLevel;
				// BEGIN TT#667 - Stodd - Pre-allocate Reserve
				_reserveAsBulk = 0;
				_reserveAsPacks = 0;
				// END TT#667 - Stodd - Pre-allocate Reserve

			}
		}

	    //============
		// PROPERTIES
		//============
		/// <summary>
		/// Gets the ProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.MethodGeneralAllocation;
			}
		}

		/// <summary>
		/// Gets or sets the dynamic or static Allocation Begin Plan date.
		/// </summary>
		public int Begin_CDR_RID
		{
			get	{return _Begin_CDR_RID;}
			set	{_Begin_CDR_RID = value;}
		}

		/// <summary>
		/// Gets or sets the dynamic or static Allocation Ship date.
		/// </summary>
		public int Ship_To_CDR_RID
		{
			get	{return _Ship_To_CDR_RID;}
			set	{_Ship_To_CDR_RID = value;}
		}

		/// <summary>
		/// Gets or sets OTS Plan RID
		/// </summary>
		public int Merch_HN_RID 
		{
			get	{return _Merch_HN_RID;}
			set	{_Merch_HN_RID = value;}
		}

		/// <summary>
		/// Gets or sets OTS Plan product hierarchy level reference RID used to dynamically find the allocation plan.
		/// </summary>
		public int Merch_PH_RID
		{
			get	{return _Merch_PH_RID;}
			set	{_Merch_PH_RID = value;	}
		}

		/// <summary>
		/// Gets or sets OTS Plan product hierarchy level sequence used to dynamically find the allocation plan.
		/// </summary>
		public int Merch_PHL_Sequence
		{
			get	{return _Merch_PHL_Sequence;}
			set	{_Merch_PHL_Sequence = value;}
		}

		/// <summary>
		/// Gets or sets the Primary Allocation Header RID.
		/// </summary>
		public int Gen_Alloc_HDR_RID
		{
			get	{return _Gen_Alloc_HDR_RID;}
			set	{_Gen_Alloc_HDR_RID = value;}
		}

		/// <summary>
		/// Gets or sets Reserve units. 
		/// </summary>
		/// <remarks>
		/// Percent_Ind determines whether this is a percent or unit value.
		/// </remarks>
		public double Reserve
		{
			get	{return _Reserve;}
			set	{_Reserve = value;	}
		}

		/// <summary>
		/// Gets or sets Merchandise Type. 
		/// </summary>
		public eMerchandiseType MerchandiseType
		{
			get	{return _merchandiseType;}
			set	{_merchandiseType = value;	}
		}

		/// <summary>
		/// Gets or sets Reserve Percent flag value.
		/// </summary>
		public bool Percent_Ind
		{
			get	{return _Percent_Ind;}
			set	{_Percent_Ind = value;	}
		}

		//		public eChangeType GenAllocMethod_Change_Type 
		//		{
		//			get { return _GenAllocMethod_Change_Type ; }
		//			set { _GenAllocMethod_Change_Type = value; }
		//		}

		//		public bool Filled 
		//		{
		//			get { return _filled ; }
		//			set { _filled = value; }
		//		}

		// BEGIN TT#667 - Stodd - Pre-allocate Reserve
		public double ReserveAsBulk
		{
			get { return _reserveAsBulk; }
			set { _reserveAsBulk = value; }
		}

		public double ReserveAsPacks
		{
			get { return _reserveAsPacks; }
			set { _reserveAsPacks = value; }
		}
		// END TT#667 - Stodd - Pre-allocate Reserve

		//========
		// METHODS
		//========
		public override void ProcessMethod(
			ApplicationSessionTransaction aApplicationTransaction,
			int aStoreFilter, Profile methodProfile)
		{
			aApplicationTransaction.ResetAllocationActionStatus();

			foreach (AllocationProfile ap in (AllocationProfileList)aApplicationTransaction.GetMasterProfileList(eProfileType.Allocation))
			{
				AllocationWorkFlowStep awfs = 
					new AllocationWorkFlowStep(
					this,
					new GeneralComponent(eGeneralComponentType.Total),
					false,
					true,
					aApplicationTransaction.SAB.ApplicationServerSession.GlobalOptions.BalanceTolerancePercent,
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

		/// <summary>
		/// Processes the action associated with this method.
		/// </summary>
		/// <param name="aSAB">Session Address Block</param>
		/// <param name="aApplicationTransaction">An instance of the Application Transaction object</param>
		/// <param name="aAllocationWorkFlowStep">Workflow Step that describes parameters associated with this action.</param>
		/// <param name="aAllocationProfile">Allocation Profile to which to apply this action</param>
		/// <param name="WriteToDB">True: write results of action to database; False: Do not write results of action to database.</param>
		public override void ProcessAction(
			SessionAddressBlock aSAB, 
			ApplicationSessionTransaction aApplicationTransaction, 
			ApplicationWorkFlowStep aAllocationWorkFlowStep,  
			Profile aAllocationProfile,
			bool WriteToDB,
			int aStoreFilterRID)
		{
            bool actionSuccess = true; // TT#1185 - Verify ENQ before Update
            // begin TT#421 Detail packs/bulk not allocated by Size Need Method.
			//AllocationProfile ap = (AllocationProfile) aAllocationProfile;
            AllocationProfile ap = aAllocationProfile as AllocationProfile;
            Audit audit = aSAB.ApplicationServerSession.Audit;
            if (ap == null)
            {
                string auditMsg = MIDText.GetText(eMIDTextCode.msg_NotAllocationProfile);
                audit.Add_Msg(
                    eMIDMessageLevel.Severe,
                    eMIDTextCode.msg_NotAllocationProfile,
                    auditMsg,
                    this.GetType().Name);
                throw new MIDException(eErrorLevel.severe,
                    (int)(eMIDTextCode.msg_NotAllocationProfile),
                    auditMsg);
            }
            // end TT#421 Detail packs/bulk not allocated by Size Need Method.
			try
			{
                // begin TT#421 Detail packs/bulk not allocated by Size Need Method.
                ap.ResetTempLocks(false); // turn temp locks off.
                //Audit audit = aSAB.ApplicationServerSession.Audit;
                // end TT#421 Detail packs/bulk not allocated by Size Need Method.
				if (!Enum.IsDefined(typeof(eAllocationMethodType),(int)aAllocationWorkFlowStep._method.MethodType))
				{
					throw new MIDException(eErrorLevel.severe,
						(int)(eMIDTextCode.msg_WorkflowTypeInvalid),
						MIDText.GetText(eMIDTextCode.msg_WorkflowTypeInvalid));
				}
                // begin TT#421 Detail packs/bulk not allocated by Size Need Method
				//if (eProfileType.Allocation != aAllocationProfile.ProfileType)
				//{
				//	throw new MIDException(eErrorLevel.severe,
				//		(int)(eMIDTextCode.msg_NotAllocationProfile),
				//		MIDText.GetText(eMIDTextCode.msg_NotAllocationProfile));
				//}
                // end TT#421 Detail packs/bulk not allocated by Size need Method

                // begin TT#1601 - MD - Jellis - General Method cannot be processed against a Group Allocation or any of its members
                AssortmentProfile asrmntP = ap as AssortmentProfile;
                if (asrmntP == null)
                {
                    asrmntP = ap.AssortmentProfile;
                }
                if (asrmntP != null
                    && asrmntP.AsrtType == (int)eAssortmentType.GroupAllocation)
                {
                    string msgText = MIDText.GetTextOnly((int)eMIDTextCode.msg_as_GeneralAllocationMethodNotAllowed);
                    audit.Add_Msg(
                        eMIDMessageLevel.Error, 
                        eMIDTextCode.msg_as_GeneralAllocationMethodNotAllowed, 
                        (this.Name + " " + ap.HeaderID + " " + msgText),
                        GetType().Name);
                    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                }
                else
                // end TT#1601 - MD - Jellis - General Method cannot be processed against a Group Allocation or any of its members
				// BEGIN MID Track 3011 Headers with status Rec'd OUT of Bal are being processed
				if (ap.HeaderAllocationStatus == eHeaderAllocationStatus.ReceivedOutOfBalance)
				{
					string msg = string.Format(
						audit.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction,false), MIDText.GetTextOnly((int)ap.HeaderAllocationStatus));
					audit.Add_Msg(
						eMIDMessageLevel.Warning,eMIDTextCode.msg_HeaderStatusDisallowsAction,
						(this.Name + " " + ap.HeaderID + " " + msg),    // MID Track 3011 Headers with status Rec'd OUT of Bal are being processed
						this.GetType().Name);
					aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
				}
					// begin MID Track 4020 Plan level not reset on cancel
				else if (this.MerchandiseType == eMerchandiseType.HierarchyLevel
                    && _Merch_PH_RID != Include.NoRID
					&& aApplicationTransaction.GetColorLevelSequence() == this.Merch_PHL_Sequence
					&& ap.PlanLevelStartHnRID == ap.StyleHnRID) // Color level specified and more than one color
				{
					audit.Add_Msg(
						eMIDMessageLevel.Warning,
						eMIDTextCode.msg_al_CannotDetermineOTSColorWhenMultipleColors,
						this.Name + " " + MIDText.GetTextOnly(eMIDTextCode.msg_al_CannotDetermineOTSColorWhenMultipleColors) + ": " + ap.HeaderID,
						this.GetType().Name);
					aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
				}
					// end MID Track 4020 Plan level not reset on cancel
                // Begin TT#1966-MD - JSmith- DC Fulfillment
                else if (ap.IsMasterHeader && ap.DCFulfillmentProcessed)
                {
                    string errorMessage = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed), ap.HeaderID);
                    audit.Add_Msg(
                        eMIDMessageLevel.Warning,
                        eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed,
                        this.Name + " " + errorMessage,
                        this.GetType().Name);
                    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                }
                else if (ap.IsSubordinateHeader && !ap.DCFulfillmentProcessed)
                {
                    string errorMessage = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed), ap.HeaderID);
                    audit.Add_Msg(
                        eMIDMessageLevel.Warning,
                        eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed,
                        this.Name + " " + errorMessage,
                        this.GetType().Name);
                    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                }
                // End TT#1966-MD - JSmith- DC Fulfillment
				else
				{
					// END MID Track 3011 Headers with status Rec'd OUT of Bal are being processed
					if (!ap.AllocationStarted ||
						(ap.AllocationStarted && ap.AllUnitsInReserve))
					{
						int workFlowRID = ap.API_WorkflowRID;
						bool workFlowTrigger = ap.API_WorkflowTrigger;
						ap.Action(eAllocationMethodType.BackoutAllocation, new GeneralComponent(eGeneralComponentType.Total), double.MaxValue, Include.AllStoreFilterRID, false);
						if (_Ship_To_CDR_RID != Include.UndefinedCalendarDateRange)
						{
							ap.ShipToDay = ((DayProfile)(aSAB.ApplicationServerSession.Calendar.GetFirstDayOfRange(_Ship_To_CDR_RID))).Date;
						}
						if (_Begin_CDR_RID != Include.UndefinedCalendarDateRange)
						{
							ap.BeginDay = ((DayProfile)(aSAB.ApplicationServerSession.Calendar.GetFirstDayOfRange(_Begin_CDR_RID))).Date;
						}
						switch (this.MerchandiseType)
						{
							case (eMerchandiseType.HierarchyLevel):
							{
								if (_Merch_PH_RID != Include.NoRID)
								{
									//			                HierarchyNodeProfile  hnp = aSAB.HierarchyServerSession.GetAncestorData(ap.StyleHnRID, 0);
									//							int offset = hnp.NodeLevel - this._Merch_PHL_Sequence;
									//						    ap.PlanHnRID = aSAB.HierarchyServerSession.GetAncestorData(ap.StyleHnRID, offset).Key;
	
									//ap.PlanHnRID = aSAB.HierarchyServerSession.GetAncestorDataByLevel(this._Merch_PH_RID, ap.StyleHnRID,  this.Merch_PHL_Sequence).Key; // MID Change j.ellis Performance--cache Ancestor data
									//ap.PlanHnRID = aApplicationTransaction.GetAncestorDataByLevel(this._Merch_PH_RID, ap.StyleHnRID,  this.Merch_PHL_Sequence).Key; // MID Change j.ellis Performance--cache Ancestor data // MID Track 4020 plan level not reset on cancel
									ap.PlanHnRID = aApplicationTransaction.GetAncestorDataByLevel(this._Merch_PH_RID, ap.PlanLevelStartHnRID,  this.Merch_PHL_Sequence).Key; // MID Change j.ellis Performance--cache Ancestor data // MID Track 4020 plan level not reset on cancel
								}
								break;
							}
							case (eMerchandiseType.Node):
							{
								if (_Merch_HN_RID != Include.NoRID)
								{
									ap.PlanHnRID = _Merch_HN_RID;
								}
								break;
							}
							case (eMerchandiseType.OTSPlanLevel):
							{
								//							ap.PlanHnRID = (aSAB.HierarchyServerSession.GetAncestorData(ap.StyleHnRID, eHierarchyLevelType.Planlevel)).Key;
								// BEGIN MID Track #3872 - use color or style node for plan level lookup
								//ap.PlanHnRID = (aApplicationTransaction.GetPlanLevelData(ap.StyleHnRID)).Key;
								
								HierarchyNodeProfile hnp = aApplicationTransaction.GetPlanLevelData(ap.PlanLevelStartHnRID);
								if (hnp == null)
								{
									throw new MIDException(eErrorLevel.severe,
										(int)(eMIDTextCode.msg_al_PlanLevelUndetermined),
										MIDText.GetText(eMIDTextCode.msg_al_PlanLevelUndetermined));
								}
								else
									ap.PlanHnRID = hnp.Key;
								// END MID Track #3872  
								break;
							}
							default:
							{
								break;
							}
						}
						if (Gen_Alloc_HDR_RID != Include.NoRID)
						{
							ap.AssignPrimaryOrSecondaryHdr(Gen_Alloc_HDR_RID,eAssignAllocationHdr.Primary);
						}
						if (this.Reserve != Include.UndefinedReserve)
						{
                            // begin TT667 Allow pack and bulk reserve
                            //if (Percent_Ind == true)
                            //{
                            //    ap.ReserveUnits = (int)(((double)ap.TotalUnitsToAllocate * ((double)Reserve / 100.0d)) + .5d);
                            //}
                            //else
                            //{
                            //    ap.ReserveUnits = (int)Reserve;
                            //}
                            //foreach (PackHdr p in ap.Packs.Values)
                            //{
                            //    p.SetReservePacks(0);
                            //}
                            //foreach (HdrColorBin c in ap.BulkColors.Values)
                            //{
                            //    c.SetReserveUnits(0);
                            //    foreach (HdrSizeBin s in c.ColorSizes.Values)
                            //    {
                            //        s.SetReserveUnits(0);
                            //    }
                            //}
                            //if (ap.ReserveUnits > 0)
                            //{
                            //    ap.AllocateReserve();
                            //}
                            AllocateReserveSpecification ars =
                                new AllocateReserveSpecification(Percent_Ind, Reserve, this.ReserveAsPacks, this.ReserveAsBulk);
                            ap.AllocateReserve(ars);
                            // end TT#667 Allow pack and bulk reserve
						}
						ap.WorkflowRID = workFlowRID;
						ap.WorkflowTrigger = workFlowTrigger;
						ap.API_WorkflowRID = ap.WorkflowRID;
						ap.API_WorkflowTrigger = ap.WorkflowTrigger;

						audit.Add_Msg(
							eMIDMessageLevel.Information,
							eMIDTextCode.msg_GeneralHeaderApplied,
							this.Name + " " + ap.HeaderID,
							this.GetType().Name);

						// begin MID Track 4981 API Workflow must be applied in General Method
						if (ap.API_WorkflowRID != Include.UndefinedWorkflowRID)
						{
							ap.WorkflowTrigger = true;
						}
						// end MID Track 4981 API Workflow must be applied in General Method
                        if (actionSuccess)  // TT#1185 - Verify ENQ before Update
                        {                   // TT#1185 - Verify ENQ before Update
                            if (WriteToDB
                                || ap.WorkflowTrigger == true)
                            {
                                actionSuccess = ap.WriteHeader(); // TT#1185 - Verify ENQ before Update
                            }
                            if (actionSuccess) // TT#1185 - Verify ENQ before Update
                            {                  // TT#1185 - Verify ENQ before Update
                                aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
                                // begin MID Track 4448 AnF Audit Enhancement
                                aApplicationTransaction.WriteAllocationAuditInfo
                                    (ap.Key,
                                    0,
                                    this.MethodType,
                                    this.Key,
                                    this.Name,
                                    eComponentType.Total,
                                    null,
                                    null, // MID Track 4448 AnF Audit Enhancement
                                    0,    // MID Track 4448 AnF Audit Enhancement
                                    0     // MID Track 4448 AnF Audit Enhancement
                                    );    // MID Track 4448 AnF Audit Enhancement
                                // end MID Track 4448 AnF Audit Enhancement	
                            }  // TT#1185 - Verify ENQ before Update
                            else // TT#1185 - Verify ENQ before Update
                            {    // TT#1185 - Verify ENQ before Update 
                                aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed); // TT#1185 - Verify ENQ before Update
                            }                                                                                                    // TT31185 - Verify ENQ before Update
                        }   // TT#1185 - Verify ENQ before Update
                        else // TT#1185 - Verify ENQ before Update
                        {    // TT#1185 - Verify ENQ before Update
                            aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed); // TT#1185 - Verify ENQ before Update
                        }    // TT#1185 - Verify ENQ Before Update 
					}
					else
					{
						// begin MID Track 5778 - Scheduler "Run Now" feature gets Error in Audit
						//audit.Add_Msg(
						//	eMIDMessageLevel.Warning,
						//	eMIDTextCode.msg_GeneralHeaderIgnored,
						//	(this.Name + " " + ap.HeaderID),
						//	this.GetType().Name);
						audit.Add_Msg(
							eMIDMessageLevel.Warning,
							eMIDTextCode.msg_MethodIgnored,
							string.Format(
							MIDText.GetText(eMIDTextCode.msg_MethodIgnored),
							ap.HeaderID, 
							MIDText.GetTextOnly(eMIDTextCode.frm_GeneralAllocationMethod), 
							this.Name),
							this.GetType().Name);
						// end MID Track 5778 - Scheduler "Run Now" feature gets Error in Audit
						aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
					}
				} // MID Track 3011 Headers with status Rec'd OUT of Bal are being processed

			}
			catch (Exception error)
			{
				aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);
				string message = error.ToString();
				throw;
			}
			finally
			{
                ap.ResetTempLocks(true); //TT#421 Detail packs/bulk not allocated by Size Need Method. 
			}
		}

		/// <summary>
		/// Unloads MethodOTSPlanProfile in to field by field object array.
		/// </summary>
		/// <returns>Object array</returns>
		public object [] ItemArray()
		{
			object [] ar = new object[9];
			ar[0] = this.Key;
			ar[1] = this.Begin_CDR_RID;
			ar[2] = this.Ship_To_CDR_RID;
			ar[3] = this.Merch_HN_RID;
			ar[4] = this.Merch_PH_RID;
			ar[5] = this.Merch_PHL_Sequence;
			ar[6] = this.Gen_Alloc_HDR_RID;
			ar[7] = this.Reserve;
			ar[8] = Include.ConvertBoolToChar(this.Percent_Ind);	
		
			return ar;
		}

		override public void Update(TransactionData td)
		{
//			bool create = false;
			if (_methodData == null || base.Key < 0 || base.Method_Change_Type == eChangeType.add)
			{
				_methodData = new GeneralAllocationMethodData(td);
//				create = true;
			}
			_methodData.Ship_To_CDR_RID = _Ship_To_CDR_RID;
			_methodData.Begin_CDR_RID = _Begin_CDR_RID;
			_methodData.Percent_Ind = Include.ConvertBoolToChar(_Percent_Ind);
	        _methodData.Reserve = _Reserve;
            _methodData.Merch_HN_RID = _Merch_HN_RID;
			_methodData.Merch_PH_RID = _Merch_PH_RID;
			_methodData.Merch_PHL_SEQ = _Merch_PHL_Sequence;
			_methodData.MerchandiseType = _merchandiseType;
			_methodData.Gen_Alloc_HDR_RID = this._Gen_Alloc_HDR_RID;
			// BEGIN TT#667 - Stodd - Pre-allocate Reserve
			_methodData.ReserveAsBulk = _reserveAsBulk;
			_methodData.ReserveAsPacks = _reserveAsPacks;
			// END TT#667 - Stodd - Pre-allocate Reserve

			try
			{
				
				switch (base.Method_Change_Type)
				{
					case eChangeType.add:
						base.Update(td);
						_methodData.InsertMethod(base.Key, td);
						break;
					case eChangeType.update:
						base.Update(td);
						_methodData.UpdateMethod(base.Key, td);
						break;
					case eChangeType.delete:
						_methodData.DeleteMethod(base.Key, td);
						base.Update(td);
						break;
				}
			}
			catch (Exception e)
			{
				string message = e.ToString();
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
			AllocationGeneralMethod newAllocationGeneralMethod = null;

			try
			{
				newAllocationGeneralMethod = (AllocationGeneralMethod)this.MemberwiseClone();
				if (aCloneDateRanges &&
					Begin_CDR_RID != Include.UndefinedCalendarDateRange)
				{
					newAllocationGeneralMethod.Begin_CDR_RID = aSession.Calendar.GetDateRangeClone(Begin_CDR_RID).Key;
				}
				else
				{
					newAllocationGeneralMethod.Begin_CDR_RID = Begin_CDR_RID;
				}
				newAllocationGeneralMethod.Gen_Alloc_HDR_RID = Gen_Alloc_HDR_RID;
				newAllocationGeneralMethod.Merch_HN_RID = Merch_HN_RID;
				newAllocationGeneralMethod.Merch_PH_RID = Merch_PH_RID;
				newAllocationGeneralMethod.Merch_PHL_Sequence = Merch_PHL_Sequence;
				newAllocationGeneralMethod.MerchandiseType = MerchandiseType;
				newAllocationGeneralMethod.Method_Change_Type = eChangeType.none;
				newAllocationGeneralMethod.Method_Description = Method_Description;
				newAllocationGeneralMethod.MethodStatus = MethodStatus;
				newAllocationGeneralMethod.Name = Name;
				newAllocationGeneralMethod.Percent_Ind = Percent_Ind;
				newAllocationGeneralMethod.Reserve = Reserve;
				newAllocationGeneralMethod.SG_RID = SG_RID;
				if (aCloneDateRanges &&
					Ship_To_CDR_RID != Include.UndefinedCalendarDateRange)
				{
					newAllocationGeneralMethod.Ship_To_CDR_RID = aSession.Calendar.GetDateRangeClone(Ship_To_CDR_RID).Key;
				}
				else
				{
					newAllocationGeneralMethod.Ship_To_CDR_RID = Ship_To_CDR_RID;
				}
				newAllocationGeneralMethod.User_RID = User_RID;
				newAllocationGeneralMethod.Virtual_IND = Virtual_IND;

				return newAllocationGeneralMethod;
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

        // Begin TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.
        /// <summary>
        /// Returns a flag identifying if the user can view the data on the method.
        /// </summary>
        /// <param name="aSession">
        /// The current session
        /// </param>
        /// <param name="aUserRID">
        /// The internal key of the user</param>
        /// <returns>
        /// A flag.
        /// </returns>
        override public bool AuthorizedToView(Session aSession, int aUserRID)
        {
            HierarchyNodeSecurityProfile hierNodeSecurity;

            if (_merchandiseType == eMerchandiseType.Node)
            {
                hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_Merch_HN_RID, (int)eSecurityTypes.Store);
                if (!hierNodeSecurity.AllowView)
                {
                    return false;
                }

            }

            return true;
        }
        // End TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.

	}
}

