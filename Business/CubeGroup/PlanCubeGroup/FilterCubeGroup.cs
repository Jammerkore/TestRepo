using System;
using System.Collections;
using System.Globalization;
using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for FilterCubeGroup.
	/// </summary>

	public class FilterCubeGroup : PlanCubeGroup
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of StorePlanMainCubeGroup, using the given SessionAddressBlock, Transaction, and PlanCubeGroup.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this StorePlanMainCubeGroup is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this StorePlanMainCubeGroup is a part of.
		/// </param>

		public FilterCubeGroup(
			SessionAddressBlock aSAB,
			Transaction aTransaction)
			: base(aSAB, aTransaction)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Opens a StorePlanMaintCubeGroup and creats master Profile Lists, XRef Lists, and Cubes that are required for support.
		/// </summary>
		/// <param name="aOpenParms">
		/// The PlanOpenParms object that contains information about the plan.
		/// </param>

		override public void OpenCubeGroup(PlanOpenParms aOpenParms)
		{
			ProfileList planWeekList;
			ProfileList storeProfileList;
			ProfileList versionProfileList;
			ProfileList hierarchyNodeProfileList;
			CubeDefinition cubeDef;
			PlanCube planCube;
			int weekCubeSize;

			try
			{
				base.OpenCubeGroup(aOpenParms);

				//==================
				// Initialize fields
				//==================

				planWeekList = _openParms.GetWeekProfileList(SAB.ApplicationServerSession);
				storeProfileList = GetFilteredProfileList(eProfileType.Store);
				versionProfileList = GetMasterProfileList(eProfileType.Version);

				//========================================
				// Create HierarchyNode Master ProfileList
				//========================================
			
				hierarchyNodeProfileList = new ProfileList(eProfileType.HierarchyNode);

				if (_openParms.ChainHLPlanProfile.NodeProfile != null)
				{
					hierarchyNodeProfileList.Add(_openParms.ChainHLPlanProfile.NodeProfile);
				}
				// Begin Track #6097 stodd - A&F HOL Null Ref resolving filter
				if (_openParms.StoreHLPlanProfile.NodeProfile != null &&
					!hierarchyNodeProfileList.Contains(_openParms.StoreHLPlanProfile.NodeProfile.Key))
				// End Track #6097
				{
					hierarchyNodeProfileList.Add(_openParms.StoreHLPlanProfile.NodeProfile);
				}

				//====================================
				// Set the Current Store Group Profile
				//====================================

				CurrentStoreGroupProfile = _openParms.GetStoreGroupProfile(_SAB);

				//========================================
				// Set Master Profile Lists and XRef Lists
				//========================================

				SetMasterProfileList(planWeekList);
				SetMasterProfileList(_openParms.GetDateProfileList(SAB.ApplicationServerSession));
				SetMasterProfileList(_openParms.GetPeriodProfileList(SAB.ApplicationServerSession));
				SetMasterProfileList(hierarchyNodeProfileList);

				SetProfileXRef(_openParms.GetDateToWeekXRef(SAB.ApplicationServerSession));
				SetProfileXRef(_openParms.GetDateToPeriodXRef(SAB.ApplicationServerSession));

				weekCubeSize = planWeekList.Count + Include.PlanReadPreWeeks + Include.PlanReadPostWeeks;

				foreach (eCubeType cubeType in ((FilterOpenParms)_openParms).ChainCubeList)
				{
					switch (cubeType.Id)
					{
						case eCubeType.cChainPlanWeekDetail:

							//========================================
							// Create ChainPlanWeekDetail in CubeGroup
							//========================================

							cubeDef = new CubeDefinition(
								new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
								new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
								new DimensionDefinition(eProfileType.Week, weekCubeSize),
								new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.HighLevel))),
								new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

							planCube = (PlanCube)GetCube(eCubeType.ChainPlanWeekDetail);

							if (planCube == null)
							{
								planCube = new ChainPlanWeekDetail(SAB, Transaction, this, cubeDef, 1, true, false);
								//Begin Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								//Transaction.PlanComputations.PlanCubeInitialization.ChainPlanWeekDetail(planCube, ePlanDisplayType.Week);
								Transaction.PlanComputations.PlanCubeInitialization.ChainPlanWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
								//End Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								SetCube(eCubeType.ChainPlanWeekDetail, planCube);
							}
							else
							{
								planCube.ExpandDimensionSize(cubeDef);
							}

							break;

						case eCubeType.cChainPlanPeriodDetail:

							//============================================
							// Create ChainPlanPeriodDetail in CubeGroup
							//============================================

							cubeDef = new CubeDefinition(
								new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
								new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
								new DimensionDefinition(eProfileType.Period, GetMasterProfileList(eProfileType.Period).Count),
								new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.HighLevel))),
								new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

							planCube = (PlanCube)GetCube(eCubeType.ChainPlanPeriodDetail);

							if (planCube == null)
							{
								planCube = new ChainPlanPeriodDetail(SAB, Transaction, this, cubeDef, 2, true, false);
								//Begin Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								//Transaction.PlanComputations.PlanCubeInitialization.ChainPlanPeriodDetail(planCube, ePlanDisplayType.Week);
								Transaction.PlanComputations.PlanCubeInitialization.ChainPlanPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
								//End Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								SetCube(eCubeType.ChainPlanPeriodDetail, planCube);
							}
							else
							{
								planCube.ExpandDimensionSize(cubeDef);
							}

							break;

						case eCubeType.cChainPlanDateTotal:

							//============================================
							// Create ChainPlanDateTotal in CubeGroup
							//============================================

							cubeDef = new CubeDefinition(
								new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
								new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
								new DimensionDefinition(eProfileType.TimeTotalVariable, GetMasterProfileList(eProfileType.TimeTotalVariable).Count),
								new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.HighLevel))),
								new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

							planCube = (PlanCube)GetCube(eCubeType.ChainPlanDateTotal);

							if (planCube == null)
							{
								planCube = new ChainPlanDateTotal(SAB, Transaction, this, cubeDef, 2, true, false);
								//Begin Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								//Transaction.PlanComputations.PlanCubeInitialization.ChainPlanDateTotal(planCube, ePlanDisplayType.Week);
								Transaction.PlanComputations.PlanCubeInitialization.ChainPlanDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
								//End Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								SetCube(eCubeType.ChainPlanDateTotal, planCube);
							}
							else
							{
								planCube.ExpandDimensionSize(cubeDef);
							}
							
							break;
					}
				}

				foreach (eCubeType cubeType in ((FilterOpenParms)_openParms).StoreCubeList)
				{
					switch (cubeType.Id)
					{
						case eCubeType.cStorePlanWeekDetail:
							
							//========================================
							// Create StorePlanWeekDetail in CubeGroup
							//========================================

							cubeDef = new CubeDefinition(
								new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
								new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
								new DimensionDefinition(eProfileType.Week, weekCubeSize),
								new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.HighLevel))),
								new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
								new DimensionDefinition(eProfileType.Store, storeProfileList.Count));

							planCube = (PlanCube)GetCube(eCubeType.StorePlanWeekDetail);

							if (planCube == null)
							{
								planCube = new StorePlanWeekDetail(SAB, Transaction, this, cubeDef, 1, true, false);
								//Begin Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								//Transaction.PlanComputations.PlanCubeInitialization.StorePlanWeekDetail(planCube, ePlanDisplayType.Week);
								Transaction.PlanComputations.PlanCubeInitialization.StorePlanWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
								//End Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								SetCube(eCubeType.StorePlanWeekDetail, planCube);
							}
							else
							{
								planCube.ExpandDimensionSize(cubeDef);
							}

							break;

						case eCubeType.cStorePlanPeriodDetail:

							//============================================
							// Create StorePlanPeriodDetail in CubeGroup
							//============================================

							cubeDef = new CubeDefinition(
								new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
								new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
								new DimensionDefinition(eProfileType.Period, GetMasterProfileList(eProfileType.Period).Count),
								new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.HighLevel))),
								new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
								new DimensionDefinition(eProfileType.Store, storeProfileList.Count));

							planCube = (PlanCube)GetCube(eCubeType.StorePlanPeriodDetail);

							if (planCube == null)
							{
								planCube = new StorePlanPeriodDetail(SAB, Transaction, this, cubeDef, 2, true, false);
								//Begin Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								//Transaction.PlanComputations.PlanCubeInitialization.StorePlanPeriodDetail(planCube, ePlanDisplayType.Week);
								Transaction.PlanComputations.PlanCubeInitialization.StorePlanPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
								//End Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								SetCube(eCubeType.StorePlanPeriodDetail, planCube);
							}
							else
							{
								planCube.ExpandDimensionSize(cubeDef);
							}
							
							break;

						case eCubeType.cStorePlanDateTotal:

							//============================================
							// Create StorePlanDateTotal in CubeGroup
							//============================================

							cubeDef = new CubeDefinition(
								new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
								new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
								new DimensionDefinition(eProfileType.TimeTotalVariable, GetMasterProfileList(eProfileType.TimeTotalVariable).Count),
								new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.HighLevel))),
								new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
								new DimensionDefinition(eProfileType.Store, storeProfileList.Count));

							planCube = (PlanCube)GetCube(eCubeType.StorePlanDateTotal);

							if (planCube == null)
							{
								planCube = new StorePlanDateTotal(SAB, Transaction, this, cubeDef, 2, true, false);
								//Begin Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								//Transaction.PlanComputations.PlanCubeInitialization.StorePlanDateTotal(planCube, ePlanDisplayType.Week);
								Transaction.PlanComputations.PlanCubeInitialization.StorePlanDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
								//End Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								SetCube(eCubeType.StorePlanDateTotal, planCube);
							}
							else
							{
								planCube.ExpandDimensionSize(cubeDef);
							}

							break;

						case eCubeType.cStorePlanGroupTotalWeekDetail:

							//==================================================
							// Create StorePlanGroupTotalWeekDetail in CubeGroup
							//==================================================

							cubeDef = new CubeDefinition(
								new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
								new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
								new DimensionDefinition(eProfileType.Week, weekCubeSize),
								new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.HighLevel))),
								new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
								new DimensionDefinition(eProfileType.StoreGroupLevel, GetFilteredProfileList(eProfileType.StoreGroupLevel).Count));

							planCube = (PlanCube)GetCube(eCubeType.StorePlanGroupTotalWeekDetail);

							if (planCube == null)
							{
								planCube = new StorePlanGroupTotalWeekDetail(SAB, Transaction, this, cubeDef, 4, true, false);
								//Begin Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								//Transaction.PlanComputations.PlanCubeInitialization.StorePlanGroupTotalWeekDetail(planCube, ePlanDisplayType.Week);
								Transaction.PlanComputations.PlanCubeInitialization.StorePlanGroupTotalWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
								//End Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								SetCube(eCubeType.StorePlanGroupTotalWeekDetail, planCube);
							}
							else
							{
								planCube.ExpandDimensionSize(cubeDef);
							}

							break;

						case eCubeType.cStorePlanGroupTotalPeriodDetail:

							//====================================================
							// Create StorePlanGroupTotalPeriodDetail in CubeGroup
							//====================================================

							cubeDef = new CubeDefinition(
								new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
								new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
								new DimensionDefinition(eProfileType.Period, GetMasterProfileList(eProfileType.Period).Count),
								new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.HighLevel))),
								new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
								new DimensionDefinition(eProfileType.StoreGroupLevel, GetFilteredProfileList(eProfileType.StoreGroupLevel).Count));

							planCube = (PlanCube)GetCube(eCubeType.StorePlanGroupTotalPeriodDetail);

							if (planCube == null)
							{
								planCube = new StorePlanGroupTotalPeriodDetail(SAB, Transaction, this, cubeDef, 5, true, false);
								//Begin Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								//Transaction.PlanComputations.PlanCubeInitialization.StorePlanGroupTotalPeriodDetail(planCube, ePlanDisplayType.Week);
								Transaction.PlanComputations.PlanCubeInitialization.StorePlanGroupTotalPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
								//End Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								SetCube(eCubeType.StorePlanGroupTotalPeriodDetail, planCube);
							}
							else
							{
								planCube.ExpandDimensionSize(cubeDef);
							}

							break;

						case eCubeType.cStorePlanGroupTotalDateTotal:

							//======================================================
							// Create StorePlanGroupTotalDateTotal in CubeGroup
							//======================================================

							cubeDef = new CubeDefinition(
								new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
								new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
								new DimensionDefinition(eProfileType.TimeTotalVariable, GetMasterProfileList(eProfileType.TimeTotalVariable).Count),
								new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.HighLevel))),
								new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
								new DimensionDefinition(eProfileType.StoreGroupLevel, GetFilteredProfileList(eProfileType.StoreGroupLevel).Count));

							planCube = (PlanCube)GetCube(eCubeType.StorePlanGroupTotalDateTotal);

							if (planCube == null)
							{
								planCube = new StorePlanGroupTotalDateTotal(SAB, Transaction, this, cubeDef, 6, true, false);
								//Begin Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								//Transaction.PlanComputations.PlanCubeInitialization.StorePlanGroupTotalDateTotal(planCube, ePlanDisplayType.Week);
								Transaction.PlanComputations.PlanCubeInitialization.StorePlanGroupTotalDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
								//End Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								SetCube(eCubeType.StorePlanGroupTotalDateTotal, planCube);
							}
							else
							{
								planCube.ExpandDimensionSize(cubeDef);
							}

							break;

						case eCubeType.cStorePlanStoreTotalWeekDetail:

							//==================================================
							// Create StorePlanStoreTotalWeekDetail in CubeGroup
							//==================================================

							cubeDef = new CubeDefinition(
								new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
								new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
								new DimensionDefinition(eProfileType.Week, weekCubeSize),
								new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.HighLevel))),
								new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));
			
							planCube = (PlanCube)GetCube(eCubeType.StorePlanStoreTotalWeekDetail);

							if (planCube == null)
							{
								planCube = new StorePlanStoreTotalWeekDetail(SAB, Transaction, this, cubeDef, 5, true, false);
								//Begin Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								//Transaction.PlanComputations.PlanCubeInitialization.StorePlanStoreTotalWeekDetail(planCube, ePlanDisplayType.Week);
								Transaction.PlanComputations.PlanCubeInitialization.StorePlanStoreTotalWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
								//End Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								SetCube(eCubeType.StorePlanStoreTotalWeekDetail, planCube);
							}
							else
							{
								planCube.ExpandDimensionSize(cubeDef);
							}

							break;

						case eCubeType.cStorePlanStoreTotalPeriodDetail:

							//====================================================
							// Create StorePlanStoreTotalPeriodDetail in CubeGroup
							//====================================================

							cubeDef = new CubeDefinition(
								new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
								new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
								new DimensionDefinition(eProfileType.Period, GetMasterProfileList(eProfileType.Period).Count),
								new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.HighLevel))),
								new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

							planCube = (PlanCube)GetCube(eCubeType.StorePlanStoreTotalPeriodDetail);

							if (planCube == null)
							{
								planCube = new StorePlanStoreTotalPeriodDetail(SAB, Transaction, this, cubeDef, 2, true, false);
								//Begin Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								//Transaction.PlanComputations.PlanCubeInitialization.StorePlanStoreTotalPeriodDetail(planCube, ePlanDisplayType.Week);
								Transaction.PlanComputations.PlanCubeInitialization.StorePlanStoreTotalPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
								//End Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								SetCube(eCubeType.StorePlanStoreTotalPeriodDetail, planCube);
							}
							else
							{
								planCube.ExpandDimensionSize(cubeDef);
							}

							break;

						case eCubeType.cStorePlanStoreTotalDateTotal:

							//======================================================
							// Create StorePlanStoreTotalDateTotal in CubeGroup
							//======================================================

							cubeDef = new CubeDefinition(
								new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
								new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
								new DimensionDefinition(eProfileType.TimeTotalVariable, GetMasterProfileList(eProfileType.TimeTotalVariable).Count),
								new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.HighLevel))),
								new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

							planCube = (PlanCube)GetCube(eCubeType.StorePlanStoreTotalDateTotal);

							if (planCube == null)
							{
								planCube = new StorePlanStoreTotalDateTotal(SAB, Transaction, this, cubeDef, 6, true, false);
								//Begin Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								//Transaction.PlanComputations.PlanCubeInitialization.StorePlanStoreTotalDateTotal(planCube, ePlanDisplayType.Week);
								Transaction.PlanComputations.PlanCubeInitialization.StorePlanStoreTotalDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
								//End Track #5898 - JScott - Turn $ not showing correctly when using a filter.
								SetCube(eCubeType.StorePlanStoreTotalDateTotal, planCube);
							}
							else
							{
								planCube.ExpandDimensionSize(cubeDef);
							}

							break;
					}
				}

				//==============================
				// Load Cube with initial values
				//==============================

				if (_openParms.StoreHLPlanProfile.VersionProfile != null && _openParms.StoreHLPlanProfile.NodeProfile != null)
				{
					((StorePlanWeekDetail)GetCube(eCubeType.StorePlanWeekDetail)).ReadAndLoadCube(_openParms.StoreHLPlanProfile, _openParms.GetWeekProfileList(SAB.ApplicationServerSession));
				}

				if (_openParms.ChainHLPlanProfile.VersionProfile != null && _openParms.ChainHLPlanProfile.NodeProfile != null)
				{
					((ChainPlanWeekDetail)GetCube(eCubeType.ChainPlanWeekDetail)).ReadAndLoadCube(_openParms.ChainHLPlanProfile, _openParms.GetWeekProfileList(SAB.ApplicationServerSession));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Saves this PlanCubeGroup
		/// </summary>
		/// <param name="aSaveParms">
		/// The PlanSaveParms that contains information about the save.
		/// </param>

		override public void SaveCubeGroup(PlanSaveParms aSaveParms)
		{
		}

        // Begin TT#2131-MD - JSmith - Halo Integration
        override public void ExtractCubeGroup(ExtractOptions aExtractOptions)
        {
        }
        // End TT#2131-MD - JSmith - Halo Integration

        /// <summary>
        /// Returns a boolean value indicating if the variable is stored on the database.
        /// </summary>
        /// <param name="aVarProf">
        /// The VariableProfile containing the variable to inspect
        /// </param>
        /// <param name="aPlanCellRef">
        /// The PlanCellReference that points to the Cell to inspect.
        /// </param>
        /// <returns>
        /// A boolean indicating if the variable is stored on the database.
        /// </returns>

        override public bool isDatabaseVariable(VariableProfile aVarProf, PlanCellReference aPlanCellRef)
		{
			return aVarProf.isDatabaseVariable(eVariableCategory.Store, aPlanCellRef.GetVersionProfile().Key, eCalendarDateType.Week);
		}

        // Begin TT#5276 - JSmith - Read Only Saves
        override public bool IsNodeEnqueued(int aNodeKey, bool aIsChain)
        {
            return true;
        }
        // End TT#5276 - JSmith - Read Only Saves
	}
}
