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
	/// This class defines the StorePlanMaintCubeGroup.
	/// </summary>
	/// <remarks>
	/// The StorePlanMaintCubeGroup predefines all Cubes that are required for Store Plan Maintenance.  Those Cubes include StoreDetailWeekCube, StoreTotalWeekCube,
	/// StoreGroupTotalCube, StoreDetailWeekTimeTotalCube, and StoreTotalWeekTimeTotalCube.
	/// </remarks>

	public class StorePlanMaintCubeGroup : StorePlanCubeGroup
	{
		//=======
		// FIELDS
		//=======

		private PlanEnqueueGroup _planEnqGrp;
		private PlanEnqueueInfo _storePlanEnqueue;
		private PlanEnqueueInfo _chainPlanEnqueue;

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

		public StorePlanMaintCubeGroup(
			SessionAddressBlock aSAB,
			Transaction aTransaction)
			: base(aSAB, aTransaction)
		{
			try
			{
				_planEnqGrp = new PlanEnqueueGroup();
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
		/// Opens a StorePlanMaintCubeGroup and creats master Profile Lists, XRef Lists, and Cubes that are required for support.
		/// </summary>
		/// <param name="aOpenParms">
		/// The PlanOpenParms object that contains information about the plan.
		/// </param>

		override public void OpenCubeGroup(PlanOpenParms aOpenParms)
		{
#if (DEBUG)
			DateTime startTime;
#endif
			ArrayList planEnqueueList;
			StoreVariableFilter varFilter;
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
				planPeriodList = _openParms.GetPeriodProfileList(_SAB.ApplicationServerSession);
				storeProfileList = GetFilteredProfileList(eProfileType.Store);
				versionProfileList = GetMasterProfileList(eProfileType.Version);

				//==================
				// Enqueue the plans
				//==================

				_chainPlanEnqueue = new PlanEnqueueInfo(_SAB, _openParms.ChainHLPlanProfile, ePlanType.Chain, planWeekList[0].Key, planWeekList[planWeekList.Count - 1].Key);
				_storePlanEnqueue = new PlanEnqueueInfo(_SAB, _openParms.StoreHLPlanProfile, ePlanType.Store, planWeekList[0].Key, planWeekList[planWeekList.Count - 1].Key);

				if (_openParms.FunctionSecurityProfile.AllowUpdate)
				{
					planEnqueueList = new ArrayList();
					planEnqueueList.Add(_chainPlanEnqueue);
					planEnqueueList.Add(_storePlanEnqueue);
					_planEnqGrp.EnqueuePlans(_SAB, planEnqueueList, _openParms.AllowReadOnlyOnConflict, _openParms.FormatErrorsForMessageBox, _openParms.UpdateAuditHeaderOnError);
				}

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

				CurrentStoreGroupProfile = _openParms.GetStoreGroupProfile(_SAB);
                Audit _audit;
                _audit = _SAB.ApplicationServerSession.Audit;

                string message = "Current Store Group Profile: " + CurrentStoreGroupProfile.GroupId.ToString() + CurrentStoreGroupProfile.Name;
                _audit.Add_Msg(eMIDMessageLevel.Debug, message, this.GetType().Name);
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

				//=========================
				// Create and apply filters
				//=========================

				varFilter = new StoreVariableFilter(this);
				ApplyFilter(varFilter, eFilterType.Permanent);

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
					planCube = new ChainPlanWeekDetail(SAB, Transaction, this, cubeDef, 1, !_chainPlanEnqueue.PlanEnqueue.isEnqueued, false);
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
					planCube = new ChainPlanPeriodDetail(SAB, Transaction, this, cubeDef, 2, !_chainPlanEnqueue.PlanEnqueue.isEnqueued, false);
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
					planCube = new ChainPlanDateTotal(SAB, Transaction, this, cubeDef, 3, !_chainPlanEnqueue.PlanEnqueue.isEnqueued, false);
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
					planCube = new StorePlanWeekDetail(SAB, Transaction, this, cubeDef, 1, !_storePlanEnqueue.PlanEnqueue.isEnqueued, false);
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
					planCube = new StorePlanPeriodDetail(SAB, Transaction, this, cubeDef, 2, !_storePlanEnqueue.PlanEnqueue.isEnqueued, false);
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
					planCube = new StorePlanDateTotal(SAB, Transaction, this, cubeDef, 3, !_storePlanEnqueue.PlanEnqueue.isEnqueued, false);
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
					planCube = new StorePlanStoreTotalWeekDetail(SAB, Transaction, this, cubeDef, 7, !_storePlanEnqueue.PlanEnqueue.isEnqueued, false);
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
					planCube = new StorePlanStoreTotalPeriodDetail(SAB, Transaction, this, cubeDef, 8, !_storePlanEnqueue.PlanEnqueue.isEnqueued, false);
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
					planCube = new StorePlanStoreTotalDateTotal(SAB, Transaction, this, cubeDef, 9, !_storePlanEnqueue.PlanEnqueue.isEnqueued, false);
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
					planCube = new StorePlanGroupTotalWeekDetail(SAB, Transaction, this, cubeDef, 4, !_storePlanEnqueue.PlanEnqueue.isEnqueued, false);
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
					planCube = new StorePlanGroupTotalPeriodDetail(SAB, Transaction, this, cubeDef, 5, !_storePlanEnqueue.PlanEnqueue.isEnqueued, false);
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
					planCube = new StorePlanGroupTotalDateTotal(SAB, Transaction, this, cubeDef, 6, !_storePlanEnqueue.PlanEnqueue.isEnqueued, false);
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

#if (DEBUG)
				startTime = DateTime.Now;
#endif
				((ChainPlanWeekDetail)GetCube(eCubeType.ChainPlanWeekDetail)).ReadAndLoadCube(_openParms.ChainHLPlanProfile, planWeekList);
				((ChainBasisWeekDetail)GetCube(eCubeType.ChainBasisWeekDetail)).ReadAndLoadCube(_openParms.ChainHLPlanProfile);
				((StorePlanWeekDetail)GetCube(eCubeType.StorePlanWeekDetail)).ReadAndLoadCube(_openParms.StoreHLPlanProfile, planWeekList);
				((StoreBasisWeekDetail)GetCube(eCubeType.StoreBasisWeekDetail)).ReadAndLoadCube(_openParms.StoreHLPlanProfile, _openParms.SimilarStores);
#if (DEBUG)
				TotalDBReadAndLoadTime = TotalPageBuildTime.Add(DateTime.Now.Subtract(startTime));
#endif
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
			ArrayList planEnqList;
			PlanProfile planProf;
			DateRangeProfile dateRangeProf;
			ProfileList storeHighLevelWeekProfList = null;
			ProfileList chainHighLevelWeekProfList = null;
			PlanEnqueueGroup planEnqGrp = null;
			StorePlanWeekDetail storeCube;
			ChainPlanWeekDetail chainCube;
			bool overrideStoreHigh;
			bool overrideChainHigh;

			try
			{
				planEnqList = new ArrayList();
				storeCube = (StorePlanWeekDetail)GetCube(eCubeType.StorePlanWeekDetail);
				chainCube = (ChainPlanWeekDetail)GetCube(eCubeType.ChainPlanWeekDetail);
				overrideStoreHigh = false;
				overrideChainHigh = false;

				if (aSaveParms.SaveStoreHighLevel &&
					(aSaveParms.StoreHighLevelNodeRID != _openParms.StoreHLPlanProfile.NodeProfile.Key ||
					aSaveParms.StoreHighLevelVersionRID != _openParms.StoreHLPlanProfile.VersionProfile.Key ||
					aSaveParms.StoreHighLevelDateRangeRID != _openParms.DateRangeProfile.Key))
				{
					dateRangeProf = _SAB.ClientServerSession.Calendar.GetDateRange(aSaveParms.StoreHighLevelDateRangeRID);
					storeHighLevelWeekProfList = SAB.ApplicationServerSession.Calendar.GetWeekRange(dateRangeProf, dateRangeProf);

					planProf = new PlanProfile(aSaveParms.StoreHighLevelNodeRID);
					planProf.NodeProfile = _SAB.HierarchyServerSession.GetNodeData(aSaveParms.StoreHighLevelNodeRID);
					planProf.NodeProfile.ChainSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(aSaveParms.StoreHighLevelNodeRID, (int)eSecurityTypes.Chain);
					planProf.NodeProfile.StoreSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(aSaveParms.StoreHighLevelNodeRID, (int)eSecurityTypes.Store);
					planProf.VersionProfile = (VersionProfile)storeCube.MasterVersionProfileList.FindKey(aSaveParms.StoreHighLevelVersionRID);

					planEnqList.Add(new PlanEnqueueInfo(_SAB, planProf, ePlanType.Store, ((WeekProfile)storeHighLevelWeekProfList[0]).Key, ((WeekProfile)storeHighLevelWeekProfList[storeHighLevelWeekProfList.Count - 1]).Key));

					overrideStoreHigh = true;
				}

				if (aSaveParms.SaveChainHighLevel &&
					(aSaveParms.ChainHighLevelNodeRID != _openParms.ChainHLPlanProfile.NodeProfile.Key ||
					aSaveParms.ChainHighLevelVersionRID != _openParms.ChainHLPlanProfile.VersionProfile.Key ||
					aSaveParms.ChainHighLevelDateRangeRID != _openParms.DateRangeProfile.Key))
				{
					dateRangeProf = _SAB.ClientServerSession.Calendar.GetDateRange(aSaveParms.ChainHighLevelDateRangeRID);
					chainHighLevelWeekProfList = SAB.ApplicationServerSession.Calendar.GetWeekRange(dateRangeProf, dateRangeProf);

					planProf = new PlanProfile(aSaveParms.ChainHighLevelNodeRID);
					planProf.NodeProfile = _SAB.HierarchyServerSession.GetNodeData(aSaveParms.ChainHighLevelNodeRID);
					planProf.NodeProfile.ChainSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(aSaveParms.ChainHighLevelNodeRID, (int)eSecurityTypes.Chain);
					planProf.NodeProfile.StoreSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(aSaveParms.ChainHighLevelNodeRID, (int)eSecurityTypes.Store);
					planProf.VersionProfile = (VersionProfile)chainCube.MasterVersionProfileList.FindKey(aSaveParms.ChainHighLevelVersionRID);

					planEnqList.Add(new PlanEnqueueInfo(_SAB, planProf, ePlanType.Chain, ((WeekProfile)chainHighLevelWeekProfList[0]).Key, ((WeekProfile)chainHighLevelWeekProfList[chainHighLevelWeekProfList.Count - 1]).Key));

					overrideChainHigh = true;
				}

				planEnqGrp = new PlanEnqueueGroup();

				try
				{
					planEnqGrp.EnqueuePlans(_SAB, planEnqList, false, _openParms.FormatErrorsForMessageBox, _openParms.UpdateAuditHeaderOnError);

					if (storeCube != null)
					{
						if (aSaveParms.SaveStoreHighLevel)
						{
							if (overrideStoreHigh)
							{
								storeCube.SaveCube(
									_openParms.StoreHLPlanProfile.NodeProfile.Key,
									_openParms.StoreHLPlanProfile.VersionProfile.Key,
									aSaveParms.StoreHighLevelNodeRID,
									aSaveParms.StoreHighLevelVersionRID,
									storeHighLevelWeekProfList,
									false,
									//Begin Track #5690 - JScott - Can not save low to high
									//false,
									//End Track #5690 - JScott - Can not save low to high
									aSaveParms.SaveLocks);
							}
							else
							{
								storeCube.SaveCube(
									_openParms.StoreHLPlanProfile.NodeProfile.Key,
									_openParms.StoreHLPlanProfile.VersionProfile.Key,
									_openParms.GetWeekProfileList(SAB.ApplicationServerSession),
									true,
									//Begin Track #5690 - JScott - Can not save low to high
									//true,
									//End Track #5690 - JScott - Can not save low to high
									aSaveParms.SaveLocks);
							}
						}
					}
					else
					{
						throw new MIDException (eErrorLevel.severe,
							(int)eMIDTextCode.msg_pl_CubeNotDefined,
							MIDText.GetText(eMIDTextCode.msg_pl_CubeNotDefined));
					}

					if (chainCube != null)
					{
						if (aSaveParms.SaveChainHighLevel)
						{
							if (overrideChainHigh)
							{
								chainCube.SaveCube(
									_openParms.ChainHLPlanProfile.NodeProfile.Key,
									_openParms.ChainHLPlanProfile.VersionProfile.Key,
									_openParms.StoreHLPlanProfile.NodeProfile.Key,
									_openParms.StoreHLPlanProfile.VersionProfile.Key,
									aSaveParms.ChainHighLevelNodeRID,
									aSaveParms.ChainHighLevelVersionRID,
									chainHighLevelWeekProfList,
									false,
									aSaveParms.SaveHighLevelAllStoreAsChain,
									//Begin Track #5690 - JScott - Can not save low to high
									//false,
									//End Track #5690 - JScott - Can not save low to high
									aSaveParms.SaveLocks);
							}
							else
							{
								chainCube.SaveCube(
									_openParms.ChainHLPlanProfile.NodeProfile.Key,
									_openParms.ChainHLPlanProfile.VersionProfile.Key,
									_openParms.StoreHLPlanProfile.NodeProfile.Key,
									_openParms.StoreHLPlanProfile.VersionProfile.Key,
									_openParms.GetWeekProfileList(SAB.ApplicationServerSession),
									true,
									aSaveParms.SaveHighLevelAllStoreAsChain,
									//Begin Track #5690 - JScott - Can not save low to high
									////Begin Enhancement - JScott - Add Balance Low Levels functionality
									////!aSaveParms.SaveHighLevelAllStoreAsChain);
									//!aSaveParms.SaveHighLevelAllStoreAsChain,
									//aSaveParms.SaveLocks);
									////End Enhancement - JScott - Add Balance Low Levels functionality
									aSaveParms.SaveLocks);
									//End Track #5690 - JScott - Can not save low to high
							}
							//Begin Track #5950 - JScott - Save Low Level to High may get warning message

							if (aSaveParms.SaveHighLevelAllStoreAsChain)
							{
								chainCube.ClearChanges(_openParms.ChainHLPlanProfile);
							}
							//End Track #5950 - JScott - Save Low Level to High may get warning message
						}
					}
					else
					{
						throw new MIDException (eErrorLevel.severe,
							(int)eMIDTextCode.msg_pl_CubeNotDefined,
							MIDText.GetText(eMIDTextCode.msg_pl_CubeNotDefined));
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					planEnqGrp.DequeuePlans();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Closes this PlanCubeGroup.
		/// </summary>

		override public void CloseCubeGroup()
		{
			try
			{
				base.CloseCubeGroup();

				_planEnqGrp.DequeuePlans();
				//Begin Enhancement - JScott - Add Balance Low Levels functionality
				//_cubeTable.Clear();
				//End Enhancement - JScott - Add Balance Low Levels functionality
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void GetReadOnlyFlags(out bool aStoreReadOnly, out bool aChainReadOnly)
		{
			aStoreReadOnly = !_storePlanEnqueue.PlanEnqueue.isEnqueued;
			aChainReadOnly = !_chainPlanEnqueue.PlanEnqueue.isEnqueued;
		}

		/// <summary>
		/// Determines if changes have occurred to either the store cube.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the store cube has changed.
		/// </returns>

		public bool hasStoreCubeChanged()
		{
			StorePlanWeekDetail cube;

			try
			{
				if (_storePlanEnqueue.PlanEnqueue.isEnqueued)
				{
					cube = (StorePlanWeekDetail)GetCube(eCubeType.StorePlanWeekDetail);

					if (cube != null)
					{
						return cube.hasPlanChanged(_openParms.StoreHLPlanProfile);
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Determines if changes have occurred to either the chain cube.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the chain cube has changed.
		/// </returns>

		public bool hasChainCubeChanged()
		{
			ChainPlanWeekDetail cube;

			try
			{
				if (_chainPlanEnqueue.PlanEnqueue.isEnqueued)
				{
					cube = (ChainPlanWeekDetail)GetCube(eCubeType.ChainPlanWeekDetail);

					if (cube != null)
					{
						return cube.hasPlanChanged(_openParms.ChainHLPlanProfile);
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
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
		/// <summary>
		/// copies store values in the basis to the stoore values in the plan.
		/// This copies from the first Basis Profile defined.
		/// </summary>
	
		public int CopyBasisToStores(ProfileList storeList)
		{
			PlanCellReference planCellRef;
			PlanCellReference basisCellRef;
			ProfileList weekProfList;
			int i;
			int cellsCopied = 0;

			try
			{
				planCellRef = (PlanCellReference)GetCube(eCubeType.StorePlanWeekDetail).CreateCellReference();
				basisCellRef = (PlanCellReference)GetCube(eCubeType.StoreBasisWeekDetail).CreateCellReference();

				planCellRef[eProfileType.Version] = _openParms.StoreHLPlanProfile.VersionProfile.Key;
				planCellRef[eProfileType.HierarchyNode] = _openParms.StoreHLPlanProfile.NodeProfile.Key;
				planCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

				basisCellRef[eProfileType.Version] = _openParms.StoreHLPlanProfile.VersionProfile.Key;
				basisCellRef[eProfileType.HierarchyNode] = _openParms.StoreHLPlanProfile.NodeProfile.Key;
				basisCellRef[eProfileType.Basis] = _openParms.BasisProfileList[0].Key;
				basisCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

				weekProfList = _openParms.GetWeekProfileList(this._SAB.ApplicationServerSession);

				for (i = 0; i < weekProfList.Count; i++)
				{
					planCellRef[eProfileType.Week] = weekProfList[i].Key;
					basisCellRef[eProfileType.Week] = weekProfList[i].Key;

					foreach (StoreProfile storeProf in storeList.ArrayList)
					{
						planCellRef[eProfileType.Store] = storeProf.Key;
						basisCellRef[eProfileType.Store] = storeProf.Key;

						foreach (VariableProfile varProf in Transaction.PlanComputations.PlanVariables.VariableProfileList)
						{
							planCellRef[eProfileType.Variable] = varProf.Key;
							basisCellRef[eProfileType.Variable] = varProf.Key;

							if (planCellRef.PlanCube.isDatabaseVariable(varProf, planCellRef))
							{
								planCellRef.SetLoadCellValue(basisCellRef.CurrentCellValue, false);
								planCellRef.isCellChanged = true;
                                // Begin Track #5971, #5981 - JSmith - Cannot copy value in blended version
                                planCellRef.isCellLoadedFromDB = true;
                                // End Track #5971, #5981
                                // Begin Track #6004 - JSmith - Destination values not equal source values
                                // The performance change removing the pre-initialization of the database values now
                                // causes the initializations for the destination value to execute after the value
                                // is copied causing the value to potentially change.  But, the value will be 
                                // initialized using the destination criteria when the data is retrieved causing
                                // it to still potentially change.
                                planCellRef.isCellInitialized = true;
                                // End Track #6004
								cellsCopied++;
							}
						}
					}
				}

				ClearUndoStack();
				return cellsCopied;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin TT#5276 - JSmith - Read Only Saves
        override public bool IsNodeEnqueued(int aNodeKey, bool aIsChain)
        {
            if (aIsChain)
            {
                return _chainPlanEnqueue.PlanEnqueue.isEnqueued;
            }
            else
            {
                return _storePlanEnqueue.PlanEnqueue.isEnqueued;
            }
        }
        // End TT#5276 - JSmith - Read Only Saves
    }
}
