using System;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for GetMethod.
	/// </summary>
	public class GetMethods
	{
		SessionAddressBlock _SAB;

		public GetMethods(SessionAddressBlock aSessionAddressBlock)
		{
			 _SAB = aSessionAddressBlock;
		}

		#region Methods
		/// <summary>
		/// Retrieves method information for an unknown method type
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <param name="returnFullMethod">A flag to identify if the routine is to return only base level method 
		/// information or return the full method based on the method type</param>
		/// <returns>An instance of the ApplicationBaseMethod class with method information</returns>
		public ApplicationBaseAction GetUnknownMethod(int aMethodRID, bool returnFullMethod)
		{
			try
			{
				ApplicationBaseAction abm;
				abm = new UnknownMethod(_SAB, aMethodRID);
				if (returnFullMethod)
				{
					return GetMethod(aMethodRID, abm.MethodType);
				}

				return abm;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieves method information for a method name where the record ID is not known
		/// </summary>
		/// <param name="aMethodName">The name of the method</param>
		/// <returns>An instance of the ApplicationBaseAction class with method information</returns>
		public ApplicationBaseAction GetMethod(string aMethodName)
		{
			try
			{
				MethodBaseData mbd = new MethodBaseData(); 
				ApplicationBaseAction abm = new UnknownMethod(_SAB, Include.NoRID);
				mbd.PopulateMethod(aMethodName);
				if (mbd.Method_RID != Include.NoRID)
				{
					abm = GetMethod(mbd.Method_RID, mbd.Method_Type_ID);
				}

				return abm;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieves method information for a method name where the record ID is not known
		/// </summary>
		/// <param name="aMethodName">The name of the method</param>
		/// <param name="aMethodType">The type of the method</param>
		/// <returns>An instance of the ApplicationBaseAction class with method information</returns>
		public ApplicationBaseAction GetMethod(string aMethodName, eMethodType aMethodType)
		{
			try
			{
				MethodBaseData mbd = new MethodBaseData(); 
				ApplicationBaseAction abm = new UnknownMethod(_SAB, Include.NoRID);
				mbd.PopulateMethod(aMethodName, aMethodType);
				if (mbd.Method_RID != Include.NoRID)
				{
					abm = GetMethod(mbd.Method_RID, mbd.Method_Type_ID);
				}

				return abm;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieves method information for an unknown method type
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <param name="aMethodType">The type of the method</param>
		/// <returns>An instance of the ApplicationBaseMethod class with method information</returns>
		public ApplicationBaseAction GetMethod(int aMethodRID, eMethodType aMethodType)
		{
			try
			{
				ApplicationBaseAction abm = null;
				if (Enum.IsDefined(typeof(eAllocationActionType),(eAllocationActionType)aMethodType))
				{
					// its an action
					abm = new AllocationAction(aMethodType);
				}
				else
				{
					switch (aMethodType)
					{
						case eMethodType.OTSPlan:
							abm = GetOTSPlanMethod(aMethodRID);
							break;
						case eMethodType.ForecastBalance:
							abm = GetForecastBalanceMethod(aMethodRID);
							break;
						case eMethodType.ForecastSpread:
							abm = GetForecastSpreadMethod(aMethodRID);
							break;
// Begin Issue # 5595 kjohnson
                        case eMethodType.GlobalUnlock:
                            abm = GetGlobalUnlockMethod(aMethodRID);
                            break;
                        //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                        case eMethodType.GlobalLock:
                            abm = GetGlobalLockMethod(aMethodRID); //TT#43 - MD - DOConnell - Projected Sales Enhancement
                            break;
                        //End TT#43 - MD - DOConnell - Projected Sales Enhancement
                        case eMethodType.Rollup:
                            abm = GetRollupMethod(aMethodRID);
                            break;
// End Issue # 5595 kjohnson
                        case eMethodType.CopyStoreForecast:
							//Begin TT#523 - JScott - Duplicate folder when new folder added
							//abm = GetForecastCopyMethod(aMethodRID, eMethodType.CopyStoreForecast);
							abm = GetForecastCopyMethod(aMethodRID, eMethodType.CopyStoreForecast, eProfileType.MethodCopyStoreForecast);
							//End TT#523 - JScott - Duplicate folder when new folder added
							break;
						case eMethodType.CopyChainForecast:
							//Begin TT#523 - JScott - Duplicate folder when new folder added
							//abm = GetForecastCopyMethod(aMethodRID, eMethodType.CopyChainForecast);
							abm = GetForecastCopyMethod(aMethodRID, eMethodType.CopyChainForecast, eProfileType.MethodCopyChainForecast);
							//End TT#523 - JScott - Duplicate folder when new folder added
							break;
						case eMethodType.ForecastModifySales:
							abm = GetForecastModifySalesMethod(aMethodRID);
							break;
//Begin Modification - JScott - Export Method - Part 10
						case eMethodType.Export:
							abm = GetForecastExportMethod(aMethodRID);
							break;
//End Modification - JScott - Export Method - Part 10
                        // Begin TT#2131-MD - JSmith - Halo Integration
                        case eMethodType.PlanningExtract:
                            abm = GetForecastPlanningExtractMethod(aMethodRID);
                            break;
                        // End TT#2131-MD - JSmith - Halo Integration
						case eMethodType.GeneralAllocation:
							abm = GetAllocationGeneralMethod(aMethodRID);
							break;
						case eMethodType.AllocationOverride:
							abm = GetAllocationOverrideMethod(aMethodRID);
							break;
						case eMethodType.FillSizeHolesAllocation:
							abm = GetFillSizeHolesMethod(aMethodRID);
							break;
						case eMethodType.Velocity:
							abm = GetVelocityMethod(aMethodRID);
							break;
						case eMethodType.Rule:
							abm = GetRuleMethod(aMethodRID);
							break;
						case eMethodType.BasisSizeAllocation:
							abm = GetBasisSizeAllocationMethod(aMethodRID);
							break;
						case eMethodType.WarehouseSizeAllocation:
							abm = GetAllocationWareHouseSizeMethod(aMethodRID);
							break;
						case eMethodType.SizeNeedAllocation:
							abm = this.GetSizeNeedMethod(aMethodRID);
							break;
                        // Begin TT#155 - JSmith - Size Curve Method
                        case eMethodType.SizeCurve:
                            abm = GetSizeCurveMethod(aMethodRID);
                            break;
                        // End TT#155
                        // Begin TT#370 - APicchetti - Build Packs Method
                        case eMethodType.BuildPacks:
                            abm = GetBuildPacksMethod(aMethodRID);
                            break;
                        // End TT#155
						// BEGIN TT#708-MD - Stodd - Group Allocation Prototype.
						case eMethodType.GroupAllocation:
							abm = GetGroupAllocationMethod(aMethodRID);
							break;
						// END TT#708-MD - Stodd - Group Allocation Prototype.
                        // Begin TT#1652-MD - RMatelic - DC Carton Rounding
                        case eMethodType.DCCartonRounding:
                            abm = GetDCCartonRoundingMethod(aMethodRID);
                            break;
                        // End TT#1652-MD 
                        // Begin TT#1966-MD - JSmith- DC Fulfillment
                        case eMethodType.CreateMasterHeaders:
                            abm = GetCreateMasterHeadersMethod(aMethodRID);
                            break;
                        case eMethodType.DCFulfillment:
                            abm = GetDCFulfillmentMethod(aMethodRID);
                            break;
                        // End TT#1966-MD - JSmith- DC Fulfillment
					}
				}

				return abm;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieve an OTS Plan method definition
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <returns>An instance of the OTSPlanMethod class with method information</returns>
		public OTSPlanMethod GetOTSPlanMethod(int aMethodRID)
		{
			try
			{
				return new OTSPlanMethod(_SAB,aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieve an OTS Plan method definition
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <returns>An instance of the OTSPlanMethod class with method information</returns>
		public OTSForecastBalanceMethod GetForecastBalanceMethod(int aMethodRID)
		{
			try
			{
				return new OTSForecastBalanceMethod(_SAB,aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		// Begin Issue # 3643 stodd 
		/// <summary>
		/// Retrieve an OTS spread method definition
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <returns>An instance of the OTSForecastSpreadMethod class with method information</returns>
		public OTSForecastSpreadMethod GetForecastSpreadMethod(int aMethodRID)
		{
			try
			{
				return new OTSForecastSpreadMethod(_SAB,aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieve an OTS Copy method definition
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <param name="methodType">Determines whether this is store or chain copy</param>
		/// <returns>An instance of the OTSForecastCopyMethod class with method information</returns>
		//Begin TT#523 - JScott - Duplicate folder when new folder added
		//public OTSForecastCopyMethod GetForecastCopyMethod(int aMethodRID, eMethodType methodType)
		public OTSForecastCopyMethod GetForecastCopyMethod(int aMethodRID, eMethodType methodType, eProfileType profileType)
		//End TT#523 - JScott - Duplicate folder when new folder added
		{
			try
			{
				//Begin TT#523 - JScott - Duplicate folder when new folder added
				//return new OTSForecastCopyMethod(_SAB, aMethodRID, methodType);
				return new OTSForecastCopyMethod(_SAB, aMethodRID, methodType, profileType);
				//End TT#523 - JScott - Duplicate folder when new folder added
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		// End Issue # 3643

        // Begin Issue # 5595 kjohnson 
        public OTSGlobalUnlockMethod GetGlobalUnlockMethod(int aMethodRID)
        {
            try
            {
                return new OTSGlobalUnlockMethod(_SAB, aMethodRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End Issue #5595 
        
        //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
        public OTSGlobalLockMethod GetGlobalLockMethod(int aMethodRID)
        {
            try
            {
                return new OTSGlobalLockMethod(_SAB, aMethodRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //End TT#43 - MD - DOConnell - Projected Sales Enhancement

        // Begin Issue # 5595 kjohnson 
        public OTSRollupMethod GetRollupMethod(int aMethodRID)
        {
            try
            {
                return new OTSRollupMethod(_SAB, aMethodRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End Issue #5595 

		public OTSForecastModifySales GetForecastModifySalesMethod(int aMethodRID)
		{
			try
			{
				return new OTSForecastModifySales(_SAB,aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

//Begin Modification - JScott - Export Method - Part 10
		public OTSForecastExportMethod GetForecastExportMethod(int aMethodRID)
		{
			try
			{
				return new OTSForecastExportMethod(_SAB,aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

//End Modification - JScott - Export Method - Part 10

        // Begin TT#2131-MD - JSmith - Halo Integration
        public OTSForecastPlanningExtractMethod GetForecastPlanningExtractMethod(int aMethodRID)
        {
            try
            {
                return new OTSForecastPlanningExtractMethod(_SAB, aMethodRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#2131-MD - JSmith - Halo Integration

		/// <summary>
		/// Retrieve an allocation general method definition
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <returns>An instance of the AllocationGeneralMethod class with method information</returns>
		public AllocationGeneralMethod GetAllocationGeneralMethod(int aMethodRID)
		{
			try
			{
				return new AllocationGeneralMethod(_SAB,aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieve an allocation override method definition
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <returns>An instance of the AllocationOverrideMethod class with method information</returns>
		public AllocationOverrideMethod GetAllocationOverrideMethod(int aMethodRID)
		{
			try
			{
				return new AllocationOverrideMethod(_SAB,aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieve a fill size holes method definition
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <returns>An instance of the FillSizeHolesMethod class with method information</returns>
		public FillSizeHolesMethod GetFillSizeHolesMethod(int aMethodRID)
		{
			try
			{
				return new FillSizeHolesMethod(_SAB,aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieve a Size Need method definition
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <returns>An instance of the SizeNeedMethod class with method information</returns>
		public SizeNeedMethod GetSizeNeedMethod(int aMethodRID)
		{
			try
			{
				return new SizeNeedMethod(_SAB,aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#155 - JSmith - Size Curve Method
        /// <summary>
		/// Retrieve a Size Curve method definition
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
        /// <returns>An instance of the SizeCurveMethod class with method information</returns>
        public SizeCurveMethod GetSizeCurveMethod(int aMethodRID)
		{
			try
			{
				return new SizeCurveMethod(_SAB,aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
        // End TT#155

        // Begin TT#370 - APicchetti - Build Packs Method
        /// <summary>
        /// Retrieve a Build Packs method definition
        /// </summary>
        /// <param name="aMethodRID">The record ID of the method</param>
        /// <returns>An instance of the BuildPacksMethod class with method information</returns>
        public BuildPacksMethod GetBuildPacksMethod(int aMethodRID)
        {
            try
            {
                return new BuildPacksMethod(_SAB, aMethodRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#370

		// BEGIN TT#708-MD - Stodd - Group Allocation Prototype.
		public GroupAllocationMethod GetGroupAllocationMethod(int aMethodRID)
		{
			try
			{
				return new GroupAllocationMethod(_SAB, aMethodRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
		// END TT#708-MD - Stodd - Group Allocation Prototype.

		/// <summary>
		/// Retrieve a velocity method definition
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <returns>An instance of the VelocityMethod class with method information</returns>
		public VelocityMethod GetVelocityMethod(int aMethodRID)
		{
			try
			{
				return new VelocityMethod(_SAB,aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieve a rule method definition
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <returns>An instance of the RuleMethod class with method information</returns>
		public RuleMethod GetRuleMethod(int aMethodRID)
		{
			try
			{
				return new RuleMethod(_SAB,aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieve a basis size allocation method definition
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <returns>An instance of the BasisSizeAllocationMethod class with method information</returns>
		public BasisSizeAllocationMethod GetBasisSizeAllocationMethod(int aMethodRID)
		{
			try
			{
				return new BasisSizeAllocationMethod(_SAB,aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieve an allocation warehouse size method definition
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <returns>An instance of the AllocationWareHouseSizeMethod class with method information</returns>
		public AllocationWareHouseSizeMethod GetAllocationWareHouseSizeMethod(int aMethodRID)
		{
			try
			{
				return new AllocationWareHouseSizeMethod(_SAB,aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
        // Begin TT#1652-MD - RMatelic - DC Carton Rounding
        public DCCartonRoundingMethod GetDCCartonRoundingMethod(int aMethodRID)
        {
            try
            {
                return new DCCartonRoundingMethod(_SAB, aMethodRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#1652-MD

        // Begin TT#1966-MD - JSmith- DC Fulfillment
        public CreateMasterHeadersMethod GetCreateMasterHeadersMethod(int aMethodRID)
        {
            try
            {
                return new CreateMasterHeadersMethod(_SAB, aMethodRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DCFulfillmentMethod GetDCFulfillmentMethod(int aMethodRID)
        {
            try
            {
                return new DCFulfillmentMethod(_SAB, aMethodRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#1966-MD - JSmith- DC Fulfillment

		#endregion Methods
	}
}
