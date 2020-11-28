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
	/// This class defines the AllocationCubeGroup.
	/// </summary>
	/// <remarks>
	/// The AllocationCubeGroup predefines all Cubes that are required for Store Allocation.  Those Cubes include StoreDetailWeekCube, StoreTotalWeekCube,
	/// StoreGroupTotalCube, StoreDetailWeekTimeTotalCube, and StoreTotalWeekTimeTotalCube.
	/// </remarks>

	public class AllocationCubeGroup : StorePlanCubeGroup
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

		public AllocationCubeGroup(
			SessionAddressBlock aSAB,
			Transaction aTransaction)
			: base(aSAB, aTransaction)
		{
			try
			{
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Opens a AllocationCubeGroup and creats master Profile Lists, XRef Lists, and Cubes that are required for support.
		/// </summary>
		/// <param name="aOpenParms">
		/// The PlanOpenParms object that contains information about the plan.
		/// </param>

		override public void OpenCubeGroup(PlanOpenParms aOpenParms)
		{
			// Begin Debug Code
			DateTime startTime;
			// End Debug Code
			EligibilityFilter eligFilter;
			ProfileList planWeekList;
			ProfileList planPeriodList;
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
				planPeriodList = _openParms.GetPeriodProfileList(SAB.ApplicationServerSession);
				storeProfileList = GetFilteredProfileList(eProfileType.Store);
				versionProfileList = GetMasterProfileList(eProfileType.Version);

				//========================================
				// Create HierarchyNode Master ProfileList
				//========================================
			
				hierarchyNodeProfileList = new ProfileList(eProfileType.HierarchyNode);

				if (!hierarchyNodeProfileList.Contains(_openParms.ChainHLPlanProfile.NodeProfile.Key))
				{
					hierarchyNodeProfileList.Add(_openParms.ChainHLPlanProfile.NodeProfile);
				}
				if (!hierarchyNodeProfileList.Contains(_openParms.StoreHLPlanProfile.NodeProfile.Key))
				{
					hierarchyNodeProfileList.Add(_openParms.StoreHLPlanProfile.NodeProfile);
				}

				//====================================
				// Set the Current Store Group Profile
				//====================================

				CurrentStoreGroupProfile = _openParms.GetStoreGroupProfile(SAB);

				//========================================
				// Set Master Profile Lists and XRef Lists
				//========================================

				SetMasterProfileList(planWeekList);
				SetMasterProfileList(_openParms.GetDateProfileList(SAB.ApplicationServerSession));
				SetMasterProfileList(_openParms.GetPeriodProfileList(SAB.ApplicationServerSession));
				SetMasterProfileList(_openParms.BasisProfileList);
				SetMasterProfileList(hierarchyNodeProfileList);

				SetProfileXRef(_openParms.GetDateToWeekXRef(SAB.ApplicationServerSession));
				SetProfileXRef(_openParms.GetDateToPeriodXRef(SAB.ApplicationServerSession));

				//=====================================================
				// Create and apply the eligiblity filter, if requested
				//=====================================================

				if (!_openParms.IneligibleStores)
				{
					eligFilter = new EligibilityFilter(this);
					ApplyFilter(eligFilter, eFilterType.Permanent);
				}

				weekCubeSize = planWeekList.Count + Include.PlanReadPreWeeks + Include.PlanReadPostWeeks;

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
					Transaction.PlanComputations.PlanCubeInitialization.ChainPlanWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.ChainPlanWeekDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

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
					Transaction.PlanComputations.PlanCubeInitialization.ChainPlanPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.ChainPlanPeriodDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

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
					planCube = new ChainPlanDateTotal(SAB, Transaction, this, cubeDef, 3, true, false);
					Transaction.PlanComputations.PlanCubeInitialization.ChainPlanDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.ChainPlanDateTotal, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//=========================================
				// Create ChainBasisWeekDetail in CubeGroup
				//=========================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
					new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.Week, weekCubeSize),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.HighLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.ChainBasisWeekDetail);

				if (planCube == null)
				{
					planCube = new ChainBasisWeekDetail(SAB, Transaction, this, cubeDef, 1);
					Transaction.PlanComputations.PlanCubeInitialization.ChainBasisWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.ChainBasisWeekDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//============================================
				// Create ChainBasisPeriodDetail in CubeGroup
				//============================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
					new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.Period, GetMasterProfileList(eProfileType.Period).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.HighLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.ChainBasisPeriodDetail);

				if (planCube == null)
				{
					planCube = new ChainBasisPeriodDetail(SAB, Transaction, this, cubeDef, 2);
					Transaction.PlanComputations.PlanCubeInitialization.ChainBasisPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.ChainBasisPeriodDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//=============================================
				// Create ChainBasisDateTotal in CubeGroup
				//=============================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
					new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.TimeTotalVariable, GetMasterProfileList(eProfileType.TimeTotalVariable).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.HighLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.ChainBasisDateTotal);

				if (planCube == null)
				{
					planCube = new ChainBasisDateTotal(SAB, Transaction, this, cubeDef, 3);
					Transaction.PlanComputations.PlanCubeInitialization.ChainBasisDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.ChainBasisDateTotal, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

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
					Transaction.PlanComputations.PlanCubeInitialization.StorePlanWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StorePlanWeekDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

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
					Transaction.PlanComputations.PlanCubeInitialization.StorePlanPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StorePlanPeriodDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

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
					planCube = new StorePlanDateTotal(SAB, Transaction, this, cubeDef, 3, true, false);
					Transaction.PlanComputations.PlanCubeInitialization.StorePlanDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StorePlanDateTotal, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

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
					planCube = new StorePlanStoreTotalWeekDetail(SAB, Transaction, this, cubeDef, 7, true, false);
					Transaction.PlanComputations.PlanCubeInitialization.StorePlanStoreTotalWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StorePlanStoreTotalWeekDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

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
					planCube = new StorePlanStoreTotalPeriodDetail(SAB, Transaction, this, cubeDef, 8, true, false);
					Transaction.PlanComputations.PlanCubeInitialization.StorePlanStoreTotalPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StorePlanStoreTotalPeriodDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

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
					planCube = new StorePlanStoreTotalDateTotal(SAB, Transaction, this, cubeDef, 9, true, false);
					Transaction.PlanComputations.PlanCubeInitialization.StorePlanStoreTotalDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StorePlanStoreTotalDateTotal, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

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
					Transaction.PlanComputations.PlanCubeInitialization.StorePlanGroupTotalWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StorePlanGroupTotalWeekDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

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
					Transaction.PlanComputations.PlanCubeInitialization.StorePlanGroupTotalPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StorePlanGroupTotalPeriodDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

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
					Transaction.PlanComputations.PlanCubeInitialization.StorePlanGroupTotalDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StorePlanGroupTotalDateTotal, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//=========================================
				// Create StoreBasisWeekDetail in CubeGroup
				//=========================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
					new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.Week, weekCubeSize),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.HighLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.Store, storeProfileList.Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisWeekDetail);

				if (planCube == null)
				{
					planCube = new StoreBasisWeekDetail(SAB, Transaction, this, cubeDef, 1);
					Transaction.PlanComputations.PlanCubeInitialization.StoreBasisWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StoreBasisWeekDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//============================================
				// Create StoreBasisPeriodDetail in CubeGroup
				//============================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
					new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.Period, GetMasterProfileList(eProfileType.Period).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.HighLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.Store, storeProfileList.Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisPeriodDetail);

				if (planCube == null)
				{
					planCube = new StoreBasisPeriodDetail(SAB, Transaction, this, cubeDef, 2);
					Transaction.PlanComputations.PlanCubeInitialization.StoreBasisPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StoreBasisPeriodDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//=============================================
				// Create StoreBasisDateTotal in CubeGroup
				//=============================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
					new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.TimeTotalVariable, GetMasterProfileList(eProfileType.TimeTotalVariable).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.HighLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.Store, storeProfileList.Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisDateTotal);

				if (planCube == null)
				{
					planCube = new StoreBasisDateTotal(SAB, Transaction, this, cubeDef, 3);
					Transaction.PlanComputations.PlanCubeInitialization.StoreBasisDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StoreBasisDateTotal, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//===================================================
				// Create StoreBasisStoreTotalWeekDetail in CubeGroup
				//===================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
					new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.Week, weekCubeSize),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.HighLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisStoreTotalWeekDetail);

				if (planCube == null)
				{
					planCube = new StoreBasisStoreTotalWeekDetail(SAB, Transaction, this, cubeDef, 7);
					Transaction.PlanComputations.PlanCubeInitialization.StoreBasisStoreTotalWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StoreBasisStoreTotalWeekDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//=====================================================
				// Create StoreBasisStoreTotalPeriodDetail in CubeGroup
				//=====================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
					new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.Period, GetMasterProfileList(eProfileType.Period).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.HighLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisStoreTotalPeriodDetail);

				if (planCube == null)
				{
					planCube = new StoreBasisStoreTotalPeriodDetail(SAB, Transaction, this, cubeDef, 8);
					Transaction.PlanComputations.PlanCubeInitialization.StoreBasisStoreTotalPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StoreBasisStoreTotalPeriodDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//=======================================================
				// Create StoreBasisStoreTotalDateTotal in CubeGroup
				//=======================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
					new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.TimeTotalVariable, GetMasterProfileList(eProfileType.TimeTotalVariable).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.HighLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisStoreTotalDateTotal);

				if (planCube == null)
				{
					planCube = new StoreBasisStoreTotalDateTotal(SAB, Transaction, this, cubeDef, 9);
					Transaction.PlanComputations.PlanCubeInitialization.StoreBasisStoreTotalDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StoreBasisStoreTotalDateTotal, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//===================================================
				// Create StoreBasisGroupTotalWeekDetail in CubeGroup
				//===================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
					new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.Week, weekCubeSize),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.HighLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.StoreGroupLevel, GetFilteredProfileList(eProfileType.StoreGroupLevel).Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisGroupTotalWeekDetail);

				if (planCube == null)
				{
					planCube = new StoreBasisGroupTotalWeekDetail(SAB, Transaction, this, cubeDef, 4);
					Transaction.PlanComputations.PlanCubeInitialization.StoreBasisGroupTotalWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StoreBasisGroupTotalWeekDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//=====================================================
				// Create StoreBasisGroupTotalPeriodDetail in CubeGroup
				//=====================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
					new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.Period, GetMasterProfileList(eProfileType.Period).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.HighLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.StoreGroupLevel, GetFilteredProfileList(eProfileType.StoreGroupLevel).Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisGroupTotalPeriodDetail);

				if (planCube == null)
				{
					planCube = new StoreBasisGroupTotalPeriodDetail(SAB, Transaction, this, cubeDef, 5);
					Transaction.PlanComputations.PlanCubeInitialization.StoreBasisGroupTotalPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StoreBasisGroupTotalPeriodDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//=======================================================
				// Create StoreBasisGroupTotalDateTotal in CubeGroup
				//=======================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
					new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.TimeTotalVariable, GetMasterProfileList(eProfileType.TimeTotalVariable).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.HighLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.StoreGroupLevel, GetFilteredProfileList(eProfileType.StoreGroupLevel).Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisGroupTotalDateTotal);

				if (planCube == null)
				{
					planCube = new StoreBasisGroupTotalDateTotal(SAB, Transaction, this, cubeDef, 6);
					Transaction.PlanComputations.PlanCubeInitialization.StoreBasisGroupTotalDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StoreBasisGroupTotalDateTotal, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//==============================
				// Load Cube with initial values
				//==============================

				// Begin Debug Code
				startTime = DateTime.Now;
				// End Debug Code
				((ChainPlanWeekDetail)GetCube(eCubeType.ChainPlanWeekDetail)).ReadAndLoadCube(_openParms.ChainHLPlanProfile, planWeekList);
				((ChainBasisWeekDetail)GetCube(eCubeType.ChainBasisWeekDetail)).ReadAndLoadCube(_openParms.ChainHLPlanProfile);
				((StorePlanWeekDetail)GetCube(eCubeType.StorePlanWeekDetail)).ReadAndLoadCube(_openParms.StoreHLPlanProfile, planWeekList);
				((StoreBasisWeekDetail)GetCube(eCubeType.StoreBasisWeekDetail)).ReadAndLoadCube(_openParms.StoreHLPlanProfile, _openParms.SimilarStores);
				// Begin Debug Code
#if (DEBUG)
				TotalDBReadAndLoadTime = TotalPageBuildTime.Add(DateTime.Now.Subtract(startTime));
#endif
				// End Debug Code
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

        public void GetReadOnlyFlags(out bool aStoreReadOnly, out bool aChainReadOnly)
		{
			aStoreReadOnly = true;
			aChainReadOnly = true;
		}

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
