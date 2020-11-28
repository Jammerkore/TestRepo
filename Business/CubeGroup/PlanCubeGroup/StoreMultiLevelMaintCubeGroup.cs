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
	/// This class defines the StoreMultiLevelPlanMaintCubeGroup.
	/// </summary>
	/// <remarks>
	/// The StoreMultiLevelPlanMaintCubeGroup predefines all Cubes that are required for Chain Plan Maintenance.  Those Cubes include ChainDetailWeekCube, LowLevelTotalWeekCube,
	/// ChainGroupTotalCube, ChainDetailWeekTimeTotalCube, and LowLevelTotalWeekTimeTotalCube.
	/// </remarks>

	public class StoreMultiLevelPlanMaintCubeGroup : StorePlanCubeGroup
	{
		//=======
		// FIELDS
		//=======

		private PlanEnqueueGroup _planEnqGrp;
		private Hashtable _chainPlanEnqueue;
		private Hashtable _storePlanEnqueue;
		private bool _anyLowLevelChainEnqueued;
		private bool _anyLowLevelStoreEnqueued;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ChainPlanMainCubeGroup, using the given SessionAddressBlock, Transaction, and PlanCubeGroup.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this ChainPlanMainCubeGroup is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this ChainPlanMainCubeGroup is a part of.
		/// </param>

		public StoreMultiLevelPlanMaintCubeGroup(
			SessionAddressBlock aSAB,
			Transaction aTransaction)
			: base(aSAB, aTransaction)
		{
			try
			{
				_planEnqGrp = new PlanEnqueueGroup();
				_chainPlanEnqueue = new Hashtable();
				_storePlanEnqueue = new Hashtable();
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
		/// Opens a StoreMultiLevelPlanMaintCubeGroup and creats master Profile Lists, XRef Lists, and Cubes that are required for support.
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
			IDictionaryEnumerator dictEnum;
			StoreVariableFilter varFilter;
			EligibilityFilter eligFilter;
			ProfileList planWeekList;
			ProfileList planPeriodList;
			ProfileList storeProfileList;
			ProfileList versionProfileList;
			ProfileList hierarchyNodeProfileList;
			ComplexProfileXRef lowLevelXRef;
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

				_chainPlanEnqueue[_openParms.ChainHLPlanProfile.Key] = new PlanEnqueueInfo(_SAB, _openParms.ChainHLPlanProfile, ePlanType.Chain, planWeekList[0].Key, planWeekList[planWeekList.Count - 1].Key);
				_storePlanEnqueue[_openParms.StoreHLPlanProfile.Key] = new PlanEnqueueInfo(_SAB, _openParms.StoreHLPlanProfile, ePlanType.Store, planWeekList[0].Key, planWeekList[planWeekList.Count - 1].Key);

				foreach (PlanProfile planProf in _openParms.LowLevelPlanProfileList)
				{
					_chainPlanEnqueue[planProf.Key] = new PlanEnqueueInfo(_SAB, planProf, ePlanType.Chain, planWeekList[0].Key, planWeekList[planWeekList.Count - 1].Key);
					_storePlanEnqueue[planProf.Key] = new PlanEnqueueInfo(_SAB, planProf, ePlanType.Store, planWeekList[0].Key, planWeekList[planWeekList.Count - 1].Key);
				}

				if (_openParms.FunctionSecurityProfile.AllowUpdate)
				{
					planEnqueueList = new ArrayList();

					dictEnum = _chainPlanEnqueue.GetEnumerator();

					while (dictEnum.MoveNext())
					{
						planEnqueueList.Add(dictEnum.Value);
					}

					dictEnum = _storePlanEnqueue.GetEnumerator();

					while (dictEnum.MoveNext())
					{
						planEnqueueList.Add(dictEnum.Value);
					}

					_planEnqGrp.EnqueuePlans(_SAB, planEnqueueList, _openParms.AllowReadOnlyOnConflict, _openParms.FormatErrorsForMessageBox, _openParms.UpdateAuditHeaderOnError);
				}

				//============================================
				// Check to see if any low-levels are enqueued
				//============================================

				_anyLowLevelChainEnqueued = false;
				_anyLowLevelStoreEnqueued = false;

				foreach (PlanProfile planProf in _openParms.LowLevelPlanProfileList)
				{
					if (((PlanEnqueueInfo)_chainPlanEnqueue[planProf.Key]).PlanEnqueue.isEnqueued)
					{
						_anyLowLevelChainEnqueued = true;
					}

					if (((PlanEnqueueInfo)_storePlanEnqueue[planProf.Key]).PlanEnqueue.isEnqueued)
					{
						_anyLowLevelStoreEnqueued = true;
					}
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

				foreach (PlanProfile planProf in _openParms.LowLevelPlanProfileList)
				{
					if (!hierarchyNodeProfileList.Contains(planProf.Key))
					{
						hierarchyNodeProfileList.Add(planProf.NodeProfile);
					}
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

				//===============================
				// Build Hierarchy low-level XRef
				//===============================

				lowLevelXRef = new ComplexProfileXRef(eProfileType.ChainPlan, eProfileType.HierarchyNode, eProfileType.Version);

				foreach (PlanProfile lowLevelPlanProf in _openParms.LowLevelPlanProfileList)
				{
					if (!lowLevelPlanProf.NodeProfile.ChainSecurityProfile.AccessDenied)
					{
						lowLevelXRef.AddXRefIdEntry(Include.FV_PlanLowLevelTotalRID, lowLevelPlanProf.NodeProfile.Key, lowLevelPlanProf.VersionProfile.Key);
						lowLevelXRef.AddXRefIdEntry(Include.FV_ActionRID, lowLevelPlanProf.NodeProfile.Key, Include.FV_ActionRID);
						lowLevelXRef.AddXRefIdEntry(Include.FV_ActualRID, lowLevelPlanProf.NodeProfile.Key, Include.FV_ActualRID);
						lowLevelXRef.AddXRefIdEntry(Include.FV_ModifiedRID, lowLevelPlanProf.NodeProfile.Key, Include.FV_ModifiedRID);
					}
				}

				SetProfileXRef(lowLevelXRef);

				lowLevelXRef = new ComplexProfileXRef(eProfileType.StorePlan, eProfileType.HierarchyNode, eProfileType.Version);

				foreach (PlanProfile lowLevelPlanProf in _openParms.LowLevelPlanProfileList)
				{
					if (!lowLevelPlanProf.NodeProfile.StoreSecurityProfile.AccessDenied)
					{
						lowLevelXRef.AddXRefIdEntry(Include.FV_PlanLowLevelTotalRID, lowLevelPlanProf.NodeProfile.Key, lowLevelPlanProf.VersionProfile.Key);
						lowLevelXRef.AddXRefIdEntry(Include.FV_ActionRID, lowLevelPlanProf.NodeProfile.Key, Include.FV_ActionRID);
						lowLevelXRef.AddXRefIdEntry(Include.FV_ActualRID, lowLevelPlanProf.NodeProfile.Key, Include.FV_ActualRID);
						lowLevelXRef.AddXRefIdEntry(Include.FV_ModifiedRID, lowLevelPlanProf.NodeProfile.Key, Include.FV_ModifiedRID);
					}
				}

				SetProfileXRef(lowLevelXRef);

				//========================================
				// Create ChainPlanWeekDetail in CubeGroup
				//========================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
					new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
					new DimensionDefinition(eProfileType.Week, weekCubeSize),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.LowLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.ChainPlanWeekDetail);

				if (planCube == null)
				{
					planCube = new ChainPlanWeekDetail(SAB, Transaction, this, cubeDef, 1, !((PlanEnqueueInfo)_chainPlanEnqueue[_openParms.ChainHLPlanProfile.NodeProfile.Key]).PlanEnqueue.isEnqueued && !_anyLowLevelChainEnqueued, true);
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.LowLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.ChainPlanPeriodDetail);

				if (planCube == null)
				{
					planCube = new ChainPlanPeriodDetail(SAB, Transaction, this, cubeDef, 2, !((PlanEnqueueInfo)_chainPlanEnqueue[_openParms.ChainHLPlanProfile.NodeProfile.Key]).PlanEnqueue.isEnqueued && !_anyLowLevelChainEnqueued, true);
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.LowLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.ChainPlanDateTotal);

				if (planCube == null)
				{
					planCube = new ChainPlanDateTotal(SAB, Transaction, this, cubeDef, 3, !((PlanEnqueueInfo)_chainPlanEnqueue[_openParms.ChainHLPlanProfile.NodeProfile.Key]).PlanEnqueue.isEnqueued && !_anyLowLevelChainEnqueued, true);
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.LowLevel))),
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.LowLevel))),
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.LowLevel))),
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

				//==================================================
				// Create ChainPlanLowLevelTotalWeekDetail in CubeGroup
				//==================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.Week, weekCubeSize),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));
			
				planCube = (PlanCube)GetCube(eCubeType.ChainPlanLowLevelTotalWeekDetail);

				if (planCube == null)
				{
					planCube = new ChainPlanLowLevelTotalWeekDetail(SAB, Transaction, this, cubeDef, 4, !_anyLowLevelChainEnqueued);
					Transaction.PlanComputations.PlanCubeInitialization.ChainPlanLowLevelTotalWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.ChainPlanLowLevelTotalWeekDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//====================================================
				// Create ChainPlanLowLevelTotalPeriodDetail in CubeGroup
				//====================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.Period, GetMasterProfileList(eProfileType.Period).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.ChainPlanLowLevelTotalPeriodDetail);

				if (planCube == null)
				{
					planCube = new ChainPlanLowLevelTotalPeriodDetail(SAB, Transaction, this, cubeDef, 5, !_anyLowLevelChainEnqueued);
					Transaction.PlanComputations.PlanCubeInitialization.ChainPlanLowLevelTotalPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.ChainPlanLowLevelTotalPeriodDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//======================================================
				// Create ChainPlanLowLevelTotalDateTotal in CubeGroup
				//======================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.TimeTotalVariable, GetMasterProfileList(eProfileType.TimeTotalVariable).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.ChainPlanLowLevelTotalDateTotal);

				if (planCube == null)
				{
					planCube = new ChainPlanLowLevelTotalDateTotal(SAB, Transaction, this, cubeDef, 6, !_anyLowLevelChainEnqueued);
					Transaction.PlanComputations.PlanCubeInitialization.ChainPlanLowLevelTotalDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.ChainPlanLowLevelTotalDateTotal, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//==================================================
				// Create ChainBasisLowLevelTotalWeekDetail in CubeGroup
				//==================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.Week, weekCubeSize),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));
			
				planCube = (PlanCube)GetCube(eCubeType.ChainBasisLowLevelTotalWeekDetail);

				if (planCube == null)
				{
					planCube = new ChainBasisLowLevelTotalWeekDetail(SAB, Transaction, this, cubeDef, 4);
					Transaction.PlanComputations.PlanCubeInitialization.ChainBasisLowLevelTotalWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.ChainBasisLowLevelTotalWeekDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//====================================================
				// Create ChainBasisLowLevelTotalPeriodDetail in CubeGroup
				//====================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.Period, GetMasterProfileList(eProfileType.Period).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.ChainBasisLowLevelTotalPeriodDetail);

				if (planCube == null)
				{
					planCube = new ChainBasisLowLevelTotalPeriodDetail(SAB, Transaction, this, cubeDef, 5);
					Transaction.PlanComputations.PlanCubeInitialization.ChainBasisLowLevelTotalPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.ChainBasisLowLevelTotalPeriodDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//======================================================
				// Create ChainBasisLowLevelTotalDateTotal in CubeGroup
				//======================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.TimeTotalVariable, GetMasterProfileList(eProfileType.TimeTotalVariable).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.ChainBasisLowLevelTotalDateTotal);

				if (planCube == null)
				{
					planCube = new ChainBasisLowLevelTotalDateTotal(SAB, Transaction, this, cubeDef, 6);
					Transaction.PlanComputations.PlanCubeInitialization.ChainBasisLowLevelTotalDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.ChainBasisLowLevelTotalDateTotal, planCube);
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.LowLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.Store, storeProfileList.Count));

				planCube = (PlanCube)GetCube(eCubeType.StorePlanWeekDetail);

				if (planCube == null)
				{
					planCube = new StorePlanWeekDetail(SAB, Transaction, this, cubeDef, 1, !((PlanEnqueueInfo)_storePlanEnqueue[_openParms.StoreHLPlanProfile.NodeProfile.Key]).PlanEnqueue.isEnqueued && !_anyLowLevelStoreEnqueued, true);
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.LowLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.Store, storeProfileList.Count));

				planCube = (PlanCube)GetCube(eCubeType.StorePlanPeriodDetail);

				if (planCube == null)
				{
					planCube = new StorePlanPeriodDetail(SAB, Transaction, this, cubeDef, 2, !((PlanEnqueueInfo)_storePlanEnqueue[_openParms.StoreHLPlanProfile.NodeProfile.Key]).PlanEnqueue.isEnqueued && !_anyLowLevelStoreEnqueued, true);
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.LowLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.Store, storeProfileList.Count));

				planCube = (PlanCube)GetCube(eCubeType.StorePlanDateTotal);

				if (planCube == null)
				{
					planCube = new StorePlanDateTotal(SAB, Transaction, this, cubeDef, 3, !((PlanEnqueueInfo)_storePlanEnqueue[_openParms.StoreHLPlanProfile.NodeProfile.Key]).PlanEnqueue.isEnqueued && !_anyLowLevelStoreEnqueued, true);
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.LowLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));
			
				planCube = (PlanCube)GetCube(eCubeType.StorePlanStoreTotalWeekDetail);

				if (planCube == null)
				{
					planCube = new StorePlanStoreTotalWeekDetail(SAB, Transaction, this, cubeDef, 13, !((PlanEnqueueInfo)_storePlanEnqueue[_openParms.StoreHLPlanProfile.NodeProfile.Key]).PlanEnqueue.isEnqueued && !_anyLowLevelStoreEnqueued, true);
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.LowLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.StorePlanStoreTotalPeriodDetail);

				if (planCube == null)
				{
					planCube = new StorePlanStoreTotalPeriodDetail(SAB, Transaction, this, cubeDef, 14, !((PlanEnqueueInfo)_storePlanEnqueue[_openParms.StoreHLPlanProfile.NodeProfile.Key]).PlanEnqueue.isEnqueued && !_anyLowLevelStoreEnqueued, true);
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.LowLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.StorePlanStoreTotalDateTotal);

				if (planCube == null)
				{
					planCube = new StorePlanStoreTotalDateTotal(SAB, Transaction, this, cubeDef, 15, !((PlanEnqueueInfo)_storePlanEnqueue[_openParms.StoreHLPlanProfile.NodeProfile.Key]).PlanEnqueue.isEnqueued && !_anyLowLevelStoreEnqueued, true);
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.LowLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.StoreGroupLevel, GetFilteredProfileList(eProfileType.StoreGroupLevel).Count));

				planCube = (PlanCube)GetCube(eCubeType.StorePlanGroupTotalWeekDetail);

				if (planCube == null)
				{
					planCube = new StorePlanGroupTotalWeekDetail(SAB, Transaction, this, cubeDef, 7, !((PlanEnqueueInfo)_storePlanEnqueue[_openParms.StoreHLPlanProfile.NodeProfile.Key]).PlanEnqueue.isEnqueued && !_anyLowLevelStoreEnqueued, true);
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.LowLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.StoreGroupLevel, GetFilteredProfileList(eProfileType.StoreGroupLevel).Count));

				planCube = (PlanCube)GetCube(eCubeType.StorePlanGroupTotalPeriodDetail);

				if (planCube == null)
				{
					planCube = new StorePlanGroupTotalPeriodDetail(SAB, Transaction, this, cubeDef, 8, !((PlanEnqueueInfo)_storePlanEnqueue[_openParms.StoreHLPlanProfile.NodeProfile.Key]).PlanEnqueue.isEnqueued && !_anyLowLevelStoreEnqueued, true);
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.LowLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.StoreGroupLevel, GetFilteredProfileList(eProfileType.StoreGroupLevel).Count));

				planCube = (PlanCube)GetCube(eCubeType.StorePlanGroupTotalDateTotal);

				if (planCube == null)
				{
					planCube = new StorePlanGroupTotalDateTotal(SAB, Transaction, this, cubeDef, 9, !((PlanEnqueueInfo)_storePlanEnqueue[_openParms.StoreHLPlanProfile.NodeProfile.Key]).PlanEnqueue.isEnqueued && !_anyLowLevelStoreEnqueued, true);
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.LowLevel))),
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.LowLevel))),
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.LowLevel))),
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.LowLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisStoreTotalWeekDetail);

				if (planCube == null)
				{
					planCube = new StoreBasisStoreTotalWeekDetail(SAB, Transaction, this, cubeDef, 13);
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.LowLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisStoreTotalPeriodDetail);

				if (planCube == null)
				{
					planCube = new StoreBasisStoreTotalPeriodDetail(SAB, Transaction, this, cubeDef, 14);
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.LowLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisStoreTotalDateTotal);

				if (planCube == null)
				{
					planCube = new StoreBasisStoreTotalDateTotal(SAB, Transaction, this, cubeDef, 15);
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.LowLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.StoreGroupLevel, GetFilteredProfileList(eProfileType.StoreGroupLevel).Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisGroupTotalWeekDetail);

				if (planCube == null)
				{
					planCube = new StoreBasisGroupTotalWeekDetail(SAB, Transaction, this, cubeDef, 7);
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.LowLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.StoreGroupLevel, GetFilteredProfileList(eProfileType.StoreGroupLevel).Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisGroupTotalPeriodDetail);

				if (planCube == null)
				{
					planCube = new StoreBasisGroupTotalPeriodDetail(SAB, Transaction, this, cubeDef, 8);
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
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.LowLevel))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.StoreGroupLevel, GetFilteredProfileList(eProfileType.StoreGroupLevel).Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisGroupTotalDateTotal);

				if (planCube == null)
				{
					planCube = new StoreBasisGroupTotalDateTotal(SAB, Transaction, this, cubeDef, 9);
					Transaction.PlanComputations.PlanCubeInitialization.StoreBasisGroupTotalDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StoreBasisGroupTotalDateTotal, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//========================================
				// Create StorePlanLowLevelTotalWeekDetail in CubeGroup
				//========================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.Week, weekCubeSize),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.Store, storeProfileList.Count));

				planCube = (PlanCube)GetCube(eCubeType.StorePlanLowLevelTotalWeekDetail);

				if (planCube == null)
				{
					planCube = new StorePlanLowLevelTotalWeekDetail(SAB, Transaction, this, cubeDef, 4, !_anyLowLevelStoreEnqueued);
					Transaction.PlanComputations.PlanCubeInitialization.StorePlanLowLevelTotalWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StorePlanLowLevelTotalWeekDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//============================================
				// Create StorePlanLowLevelTotalPeriodDetail in CubeGroup
				//============================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.Period, GetMasterProfileList(eProfileType.Period).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.Store, storeProfileList.Count));

				planCube = (PlanCube)GetCube(eCubeType.StorePlanLowLevelTotalPeriodDetail);

				if (planCube == null)
				{
					planCube = new StorePlanLowLevelTotalPeriodDetail(SAB, Transaction, this, cubeDef, 5, !_anyLowLevelStoreEnqueued);
					Transaction.PlanComputations.PlanCubeInitialization.StorePlanLowLevelTotalPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StorePlanLowLevelTotalPeriodDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//============================================
				// Create StorePlanLowLevelTotalDateTotal in CubeGroup
				//============================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.TimeTotalVariable, GetMasterProfileList(eProfileType.TimeTotalVariable).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.Store, storeProfileList.Count));

				planCube = (PlanCube)GetCube(eCubeType.StorePlanLowLevelTotalDateTotal);

				if (planCube == null)
				{
					planCube = new StorePlanLowLevelTotalDateTotal(SAB, Transaction, this, cubeDef, 6, !_anyLowLevelStoreEnqueued);
					Transaction.PlanComputations.PlanCubeInitialization.StorePlanLowLevelTotalDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StorePlanLowLevelTotalDateTotal, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//==================================================
				// Create StorePlanLowLevelTotalStoreTotalWeekDetail in CubeGroup
				//==================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.Week, weekCubeSize),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));
			
				planCube = (PlanCube)GetCube(eCubeType.StorePlanLowLevelTotalStoreTotalWeekDetail);

				if (planCube == null)
				{
					planCube = new StorePlanLowLevelTotalStoreTotalWeekDetail(SAB, Transaction, this, cubeDef, 16, !_anyLowLevelStoreEnqueued);
					Transaction.PlanComputations.PlanCubeInitialization.StorePlanLowLevelTotalStoreTotalWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StorePlanLowLevelTotalStoreTotalWeekDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//====================================================
				// Create StorePlanLowLevelTotalStoreTotalPeriodDetail in CubeGroup
				//====================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.Period, GetMasterProfileList(eProfileType.Period).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.StorePlanLowLevelTotalStoreTotalPeriodDetail);

				if (planCube == null)
				{
					planCube = new StorePlanLowLevelTotalStoreTotalPeriodDetail(SAB, Transaction, this, cubeDef, 17, !_anyLowLevelStoreEnqueued);
					Transaction.PlanComputations.PlanCubeInitialization.StorePlanLowLevelTotalStoreTotalPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StorePlanLowLevelTotalStoreTotalPeriodDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//======================================================
				// Create StorePlanLowLevelTotalStoreTotalDateTotal in CubeGroup
				//======================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.TimeTotalVariable, GetMasterProfileList(eProfileType.TimeTotalVariable).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.StorePlanLowLevelTotalStoreTotalDateTotal);

				if (planCube == null)
				{
					planCube = new StorePlanLowLevelTotalStoreTotalDateTotal(SAB, Transaction, this, cubeDef, 18, !_anyLowLevelStoreEnqueued);
					Transaction.PlanComputations.PlanCubeInitialization.StorePlanLowLevelTotalStoreTotalDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StorePlanLowLevelTotalStoreTotalDateTotal, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//==================================================
				// Create StorePlanLowLevelTotalGroupTotalWeekDetail in CubeGroup
				//==================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.Week, weekCubeSize),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.StoreGroupLevel, GetFilteredProfileList(eProfileType.StoreGroupLevel).Count));

				planCube = (PlanCube)GetCube(eCubeType.StorePlanLowLevelTotalGroupTotalWeekDetail);

				if (planCube == null)
				{
					planCube = new StorePlanLowLevelTotalGroupTotalWeekDetail(SAB, Transaction, this, cubeDef, 10, !_anyLowLevelStoreEnqueued);
					Transaction.PlanComputations.PlanCubeInitialization.StorePlanLowLevelTotalGroupTotalWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StorePlanLowLevelTotalGroupTotalWeekDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//====================================================
				// Create StorePlanLowLevelTotalGroupTotalPeriodDetail in CubeGroup
				//====================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.Period, GetMasterProfileList(eProfileType.Period).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.StoreGroupLevel, GetFilteredProfileList(eProfileType.StoreGroupLevel).Count));

				planCube = (PlanCube)GetCube(eCubeType.StorePlanLowLevelTotalGroupTotalPeriodDetail);

				if (planCube == null)
				{
					planCube = new StorePlanLowLevelTotalGroupTotalPeriodDetail(SAB, Transaction, this, cubeDef, 11, !_anyLowLevelStoreEnqueued);
					Transaction.PlanComputations.PlanCubeInitialization.StorePlanLowLevelTotalGroupTotalPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StorePlanLowLevelTotalGroupTotalPeriodDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//======================================================
				// Create StorePlanLowLevelTotalGroupTotalDateTotal in CubeGroup
				//======================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.TimeTotalVariable, GetMasterProfileList(eProfileType.TimeTotalVariable).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.StoreGroupLevel, GetFilteredProfileList(eProfileType.StoreGroupLevel).Count));

				planCube = (PlanCube)GetCube(eCubeType.StorePlanLowLevelTotalGroupTotalDateTotal);

				if (planCube == null)
				{
					planCube = new StorePlanLowLevelTotalGroupTotalDateTotal(SAB, Transaction, this, cubeDef, 12, !_anyLowLevelStoreEnqueued);
					Transaction.PlanComputations.PlanCubeInitialization.StorePlanLowLevelTotalGroupTotalDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StorePlanLowLevelTotalGroupTotalDateTotal, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//=========================================
				// Create StoreBasisLowLevelTotalWeekDetail in CubeGroup
				//=========================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.Week, weekCubeSize),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.Store, storeProfileList.Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisLowLevelTotalWeekDetail);

				if (planCube == null)
				{
					planCube = new StoreBasisLowLevelTotalWeekDetail(SAB, Transaction, this, cubeDef, 4);
					Transaction.PlanComputations.PlanCubeInitialization.StoreBasisLowLevelTotalWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StoreBasisLowLevelTotalWeekDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//============================================
				// Create StoreBasisLowLevelTotalPeriodDetail in CubeGroup
				//============================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.Period, GetMasterProfileList(eProfileType.Period).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.Store, storeProfileList.Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisLowLevelTotalPeriodDetail);

				if (planCube == null)
				{
					planCube = new StoreBasisLowLevelTotalPeriodDetail(SAB, Transaction, this, cubeDef, 5);
					Transaction.PlanComputations.PlanCubeInitialization.StoreBasisLowLevelTotalPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StoreBasisLowLevelTotalPeriodDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//=============================================
				// Create StoreBasisLowLevelTotalDateTotal in CubeGroup
				//=============================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.TimeTotalVariable, GetMasterProfileList(eProfileType.TimeTotalVariable).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.Store, storeProfileList.Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisLowLevelTotalDateTotal);

				if (planCube == null)
				{
					planCube = new StoreBasisLowLevelTotalDateTotal(SAB, Transaction, this, cubeDef, 6);
					Transaction.PlanComputations.PlanCubeInitialization.StoreBasisLowLevelTotalDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StoreBasisLowLevelTotalDateTotal, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//===================================================
				// Create StoreBasisLowLevelTotalStoreTotalWeekDetail in CubeGroup
				//===================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.Week, weekCubeSize),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisLowLevelTotalStoreTotalWeekDetail);

				if (planCube == null)
				{
					planCube = new StoreBasisLowLevelTotalStoreTotalWeekDetail(SAB, Transaction, this, cubeDef, 16);
					Transaction.PlanComputations.PlanCubeInitialization.StoreBasisLowLevelTotalStoreTotalWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StoreBasisLowLevelTotalStoreTotalWeekDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//=====================================================
				// Create StoreBasisLowLevelTotalStoreTotalPeriodDetail in CubeGroup
				//=====================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.Period, GetMasterProfileList(eProfileType.Period).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisLowLevelTotalStoreTotalPeriodDetail);

				if (planCube == null)
				{
					planCube = new StoreBasisLowLevelTotalStoreTotalPeriodDetail(SAB, Transaction, this, cubeDef, 17);
					Transaction.PlanComputations.PlanCubeInitialization.StoreBasisLowLevelTotalStoreTotalPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StoreBasisLowLevelTotalStoreTotalPeriodDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//=======================================================
				// Create StoreBasisLowLevelTotalStoreTotalDateTotal in CubeGroup
				//=======================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.TimeTotalVariable, GetMasterProfileList(eProfileType.TimeTotalVariable).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisLowLevelTotalStoreTotalDateTotal);

				if (planCube == null)
				{
					planCube = new StoreBasisLowLevelTotalStoreTotalDateTotal(SAB, Transaction, this, cubeDef, 18);
					Transaction.PlanComputations.PlanCubeInitialization.StoreBasisLowLevelTotalStoreTotalDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StoreBasisLowLevelTotalStoreTotalDateTotal, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//===================================================
				// Create StoreBasisLowLevelTotalGroupTotalWeekDetail in CubeGroup
				//===================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.Week, weekCubeSize),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.StoreGroupLevel, GetFilteredProfileList(eProfileType.StoreGroupLevel).Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisLowLevelTotalGroupTotalWeekDetail);

				if (planCube == null)
				{
					planCube = new StoreBasisLowLevelTotalGroupTotalWeekDetail(SAB, Transaction, this, cubeDef, 10);
					Transaction.PlanComputations.PlanCubeInitialization.StoreBasisLowLevelTotalGroupTotalWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StoreBasisLowLevelTotalGroupTotalWeekDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//=====================================================
				// Create StoreBasisLowLevelTotalGroupTotalPeriodDetail in CubeGroup
				//=====================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.Period, GetMasterProfileList(eProfileType.Period).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.StoreGroupLevel, GetFilteredProfileList(eProfileType.StoreGroupLevel).Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisLowLevelTotalGroupTotalPeriodDetail);

				if (planCube == null)
				{
					planCube = new StoreBasisLowLevelTotalGroupTotalPeriodDetail(SAB, Transaction, this, cubeDef, 11);
					Transaction.PlanComputations.PlanCubeInitialization.StoreBasisLowLevelTotalGroupTotalPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StoreBasisLowLevelTotalGroupTotalPeriodDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//=======================================================
				// Create StoreBasisLowLevelTotalGroupTotalDateTotal in CubeGroup
				//=======================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.LowLevelTotalVersion, 3),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.TimeTotalVariable, GetMasterProfileList(eProfileType.TimeTotalVariable).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.LowLevelTotal))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
					new DimensionDefinition(eProfileType.StoreGroupLevel, GetFilteredProfileList(eProfileType.StoreGroupLevel).Count));

				planCube = (PlanCube)GetCube(eCubeType.StoreBasisLowLevelTotalGroupTotalDateTotal);

				if (planCube == null)
				{
					planCube = new StoreBasisLowLevelTotalGroupTotalDateTotal(SAB, Transaction, this, cubeDef, 12);
					Transaction.PlanComputations.PlanCubeInitialization.StoreBasisLowLevelTotalGroupTotalDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.StoreBasisLowLevelTotalGroupTotalDateTotal, planCube);
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
				((ChainPlanWeekDetail)GetCube(eCubeType.ChainPlanWeekDetail)).ReadAndLoadCube(_openParms.LowLevelPlanProfileList, planWeekList);
				((ChainBasisWeekDetail)GetCube(eCubeType.ChainBasisWeekDetail)).ReadAndLoadCube(_openParms.LowLevelPlanProfileList);
				((StorePlanWeekDetail)GetCube(eCubeType.StorePlanWeekDetail)).ReadAndLoadCube(_openParms.StoreHLPlanProfile, planWeekList);
				((StoreBasisWeekDetail)GetCube(eCubeType.StoreBasisWeekDetail)).ReadAndLoadCube(_openParms.StoreHLPlanProfile, _openParms.SimilarStores);
				((StorePlanWeekDetail)GetCube(eCubeType.StorePlanWeekDetail)).ReadAndLoadCube(_openParms.LowLevelPlanProfileList, planWeekList);
				((StoreBasisWeekDetail)GetCube(eCubeType.StoreBasisWeekDetail)).ReadAndLoadCube(_openParms.LowLevelPlanProfileList, _openParms.SimilarStores);
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
			ProfileList storeLowLevelWeekProfList = null;
			ProfileList chainHighLevelWeekProfList = null;
			ProfileList chainLowLevelWeekProfList = null;
			PlanEnqueueGroup planEnqGrp = null;
			StorePlanWeekDetail storeCube;
			ChainPlanWeekDetail chainCube;
			bool overrideStoreHigh;
			bool overrideChainHigh;
			Hashtable overrideStoreLow;
			Hashtable overrideChainLow;

			try
			{
				planEnqList = new ArrayList();
				storeCube = (StorePlanWeekDetail)GetCube(eCubeType.StorePlanWeekDetail);
				chainCube = (ChainPlanWeekDetail)GetCube(eCubeType.ChainPlanWeekDetail);
				overrideStoreHigh = false;
				overrideChainHigh = false;
				overrideStoreLow = new Hashtable();
				overrideChainLow = new Hashtable();

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

				if (aSaveParms.SaveStoreLowLevel && aSaveParms.OverrideStoreLowLevel)
				{
					dateRangeProf = _SAB.ClientServerSession.Calendar.GetDateRange(aSaveParms.StoreLowLevelDateRangeRID);
					storeLowLevelWeekProfList = SAB.ApplicationServerSession.Calendar.GetWeekRange(dateRangeProf, dateRangeProf);
			
					foreach (PlanProfile lowLevelPlanProf in _openParms.LowLevelPlanProfileList)
					{
						if (aSaveParms.StoreLowLevelVersionRID != lowLevelPlanProf.VersionProfile.Key ||
							aSaveParms.StoreLowLevelDateRangeRID != _openParms.DateRangeProfile.Key)
						{
							planProf = new PlanProfile(lowLevelPlanProf.NodeProfile.Key);
							planProf.NodeProfile = _SAB.HierarchyServerSession.GetNodeData(lowLevelPlanProf.NodeProfile.Key);
							planProf.NodeProfile.ChainSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(lowLevelPlanProf.NodeProfile.Key, (int)eSecurityTypes.Chain);
							planProf.NodeProfile.StoreSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(lowLevelPlanProf.NodeProfile.Key, (int)eSecurityTypes.Store);
							planProf.VersionProfile = (VersionProfile)storeCube.MasterVersionProfileList.FindKey(aSaveParms.StoreLowLevelVersionRID);

							planEnqList.Add(new PlanEnqueueInfo(_SAB, planProf, ePlanType.Store, ((WeekProfile)storeLowLevelWeekProfList[0]).Key, ((WeekProfile)storeLowLevelWeekProfList[storeLowLevelWeekProfList.Count - 1]).Key));

							overrideStoreLow.Add(lowLevelPlanProf, null);
						}
					}
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

				if (aSaveParms.SaveChainLowLevel && aSaveParms.OverrideChainLowLevel)
				{
					dateRangeProf = _SAB.ClientServerSession.Calendar.GetDateRange(aSaveParms.ChainLowLevelDateRangeRID);
					chainLowLevelWeekProfList = SAB.ApplicationServerSession.Calendar.GetWeekRange(dateRangeProf, dateRangeProf);
			
					foreach (PlanProfile lowLevelPlanProf in _openParms.LowLevelPlanProfileList)
					{
						if (aSaveParms.ChainLowLevelVersionRID != lowLevelPlanProf.VersionProfile.Key ||
							aSaveParms.ChainLowLevelDateRangeRID != _openParms.DateRangeProfile.Key)
						{
							planProf = new PlanProfile(lowLevelPlanProf.NodeProfile.Key);
							planProf.NodeProfile = _SAB.HierarchyServerSession.GetNodeData(lowLevelPlanProf.NodeProfile.Key);
							planProf.NodeProfile.ChainSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(lowLevelPlanProf.NodeProfile.Key, (int)eSecurityTypes.Chain);
							planProf.NodeProfile.ChainSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(lowLevelPlanProf.NodeProfile.Key, (int)eSecurityTypes.Chain);
							planProf.VersionProfile = (VersionProfile)chainCube.MasterVersionProfileList.FindKey(aSaveParms.ChainLowLevelVersionRID);

							planEnqList.Add(new PlanEnqueueInfo(_SAB, planProf, ePlanType.Chain, ((WeekProfile)chainLowLevelWeekProfList[0]).Key, ((WeekProfile)chainLowLevelWeekProfList[chainLowLevelWeekProfList.Count - 1]).Key));

							overrideChainLow.Add(lowLevelPlanProf, null);
						}
					}
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

						if (aSaveParms.SaveStoreLowLevel)
						{
							foreach (PlanProfile lowLevelPlanProf in _openParms.LowLevelPlanProfileList)
							{
								if (overrideStoreLow.ContainsKey(lowLevelPlanProf))
								{
									storeCube.SaveCube(
										lowLevelPlanProf.NodeProfile.Key,
										lowLevelPlanProf.VersionProfile.Key,
										lowLevelPlanProf.NodeProfile.Key,
										aSaveParms.StoreLowLevelVersionRID,
										storeLowLevelWeekProfList,
										false,
										//Begin Track #5690 - JScott - Can not save low to high
										//false,
										//End Track #5690 - JScott - Can not save low to high
										aSaveParms.SaveLocks);
								}
								else
								{
									storeCube.SaveCube(
										lowLevelPlanProf.NodeProfile.Key,
										lowLevelPlanProf.VersionProfile.Key,
										_openParms.GetWeekProfileList(SAB.ApplicationServerSession),
										true,
										//Begin Track #5690 - JScott - Can not save low to high
										//true,
										//End Track #5690 - JScott - Can not save low to high
										aSaveParms.SaveLocks);
								}
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
									//!aSaveParms.SaveHighLevelAllStoreAsChain,
									//End Track #5690 - JScott - Can not save low to high
									aSaveParms.SaveLocks);
							}
							//Begin Track #5950 - JScott - Save Low Level to High may get warning message

							if (aSaveParms.SaveHighLevelAllStoreAsChain)
							{
								chainCube.ClearChanges(_openParms.ChainHLPlanProfile);
							}
							//End Track #5950 - JScott - Save Low Level to High may get warning message
						}

						if (aSaveParms.SaveChainLowLevel)
						{
							foreach (PlanProfile lowLevelPlanProf in _openParms.LowLevelPlanProfileList)
							{
								if (overrideChainLow.ContainsKey(lowLevelPlanProf))
								{
									chainCube.SaveCube(
										lowLevelPlanProf.NodeProfile.Key,
										lowLevelPlanProf.VersionProfile.Key,
										lowLevelPlanProf.NodeProfile.Key,
										lowLevelPlanProf.VersionProfile.Key,
										lowLevelPlanProf.NodeProfile.Key,
										aSaveParms.ChainLowLevelVersionRID,
										chainLowLevelWeekProfList,
										false,
										aSaveParms.SaveLowLevelAllStoreAsChain,
										//Begin Track #5690 - JScott - Can not save low to high
										//false,
										//End Track #5690 - JScott - Can not save low to high
										aSaveParms.SaveLocks);
								}
								else
								{
									chainCube.SaveCube(
										lowLevelPlanProf.NodeProfile.Key,
										lowLevelPlanProf.VersionProfile.Key,
										lowLevelPlanProf.NodeProfile.Key,
										lowLevelPlanProf.VersionProfile.Key,
										_openParms.GetWeekProfileList(SAB.ApplicationServerSession),
										true,
										aSaveParms.SaveLowLevelAllStoreAsChain,
										//Begin Track #5690 - JScott - Can not save low to high
										//!aSaveParms.SaveLowLevelAllStoreAsChain,
										//End Track #5690 - JScott - Can not save low to high
										aSaveParms.SaveLocks);
								}
							}
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

		public void GetReadOnlyFlags(out bool aStoreReadOnly, out bool aChainReadOnly, out bool aLowLevelStoreReadOnly, out bool aLowLevelChainReadOnly)
		{
			try
			{
				aStoreReadOnly = !((PlanEnqueueInfo)_storePlanEnqueue[_openParms.StoreHLPlanProfile.NodeProfile.Key]).PlanEnqueue.isEnqueued;

				aLowLevelStoreReadOnly = true;

				foreach (PlanProfile planProf in _openParms.LowLevelPlanProfileList)
				{
					if (((PlanEnqueueInfo)_storePlanEnqueue[planProf.NodeProfile.Key]).PlanEnqueue.isEnqueued)
					{
						aLowLevelStoreReadOnly = false;
						break;
					}
				}

				aChainReadOnly = !((PlanEnqueueInfo)_chainPlanEnqueue[_openParms.ChainHLPlanProfile.NodeProfile.Key]).PlanEnqueue.isEnqueued;

				aLowLevelChainReadOnly = true;

				foreach (PlanProfile planProf in _openParms.LowLevelPlanProfileList)
				{
					if (((PlanEnqueueInfo)_chainPlanEnqueue[planProf.NodeProfile.Key]).PlanEnqueue.isEnqueued)
					{
						aLowLevelChainReadOnly = false;
						break;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Determines if changes have occurred to either the store high level cubes.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the store cube has changed.
		/// </returns>

		public bool hasStoreHighLevelCubeChanged()
		{
			StorePlanWeekDetail cube;

			try
			{
				if (((PlanEnqueueInfo)_storePlanEnqueue[_openParms.StoreHLPlanProfile.NodeProfile.Key]).PlanEnqueue.isEnqueued)
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
		/// Determines if changes have occurred to either the store low level cubes.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the store cube has changed.
		/// </returns>

		public bool hasStoreLowLevelCubeChanged()
		{
			StorePlanWeekDetail cube;

			try
			{
				foreach (PlanProfile lowLevelPlanProf in _openParms.LowLevelPlanProfileList)
				{
					if (((PlanEnqueueInfo)_storePlanEnqueue[lowLevelPlanProf.NodeProfile.Key]).PlanEnqueue.isEnqueued)
					{
						cube = (StorePlanWeekDetail)GetCube(eCubeType.StorePlanWeekDetail);

						if (cube != null)
						{
							if (cube.hasPlanChanged(lowLevelPlanProf))
							{
								return true;
							}
						}
					}
				}

				return false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Determines if changes have occurred to either the chain high level cubes.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the chain cube has changed.
		/// </returns>

		public bool hasChainHighLevelCubeChanged()
		{
			ChainPlanWeekDetail cube;

			try
			{
				if (((PlanEnqueueInfo)_chainPlanEnqueue[_openParms.ChainHLPlanProfile.NodeProfile.Key]).PlanEnqueue.isEnqueued)
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
		/// Determines if changes have occurred to either the chain low level cubes.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the chain cube has changed.
		/// </returns>

		public bool hasChainLowLevelCubeChanged()
		{
			ChainPlanWeekDetail cube;

			try
			{
				foreach (PlanProfile lowLevelPlanProf in _openParms.LowLevelPlanProfileList)
				{
					if (((PlanEnqueueInfo)_chainPlanEnqueue[lowLevelPlanProf.NodeProfile.Key]).PlanEnqueue.isEnqueued)
					{
						cube = (ChainPlanWeekDetail)GetCube(eCubeType.ChainPlanWeekDetail);

						if (cube != null)
						{
							if (cube.hasPlanChanged(lowLevelPlanProf))
							{
								return true;
							}
						}
					}
				}

				return false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Perform matrix balance.
		/// </summary>

		public void MatrixBalance(int aVariableNumber, eBalanceMode aBalanceMode, int aIterationsCount, 
			ePlanBasisType aPlanBasisType, int aBasisIndex)
		{
			try
			{
				ClearUndoStack();
				System.GC.Collect();

				int recomputesProcessed = 0;
				for (int i=0; i<aIterationsCount; i++)
				{
					if (aBalanceMode == eBalanceMode.Chain)
					{
						// Only spread in one direction on first iteration
						if (i > 0)
						{
							MatrixBalanceToStore(aVariableNumber, false, -1, ref recomputesProcessed);
						}
						MatrixBalanceToChain(aVariableNumber, 
							(_openParms.BasisProfileList.Count > 0) && (i==0) && (aPlanBasisType == ePlanBasisType.Basis), 
							aBasisIndex,
							ref recomputesProcessed);
					}
					else
					{
						// Only spread in one direction on first iteration
						if (i > 0)
						{
							MatrixBalanceToChain(aVariableNumber, false, -1, ref recomputesProcessed);
						}
						MatrixBalanceToStore(aVariableNumber, (
							_openParms.BasisProfileList.Count > 0) && (i==0) && (aPlanBasisType == ePlanBasisType.Basis), 
							aBasisIndex,
							ref recomputesProcessed);
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Perform matrix balance.
		/// </summary>

		//Begin Enhancement - JScott - Add Balance Low Levels functionality
		//public void CopyLowToHigh()
		override public void CopyLowToHigh()
		//End Enhancement - JScott - Add Balance Low Levels functionality
		{
			PlanCellReference lowLevelTotalCellRef;
			PlanCellReference highLevelCellRef;
			ProfileList weekProfList;
			int i;

			try
			{
				lowLevelTotalCellRef = (PlanCellReference)GetCube(eCubeType.StorePlanLowLevelTotalWeekDetail).CreateCellReference();
				highLevelCellRef = (PlanCellReference)GetCube(eCubeType.StorePlanWeekDetail).CreateCellReference();
				
				lowLevelTotalCellRef[eProfileType.LowLevelTotalVersion] = Include.FV_PlanLowLevelTotalRID;
				lowLevelTotalCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

				highLevelCellRef[eProfileType.Version] = _openParms.StoreHLPlanProfile.VersionProfile.Key;
				highLevelCellRef[eProfileType.HierarchyNode] = _openParms.StoreHLPlanProfile.NodeProfile.Key;
				highLevelCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

				//Scenario #1
				//Begin Track #6427 - JSCott - Urgent - In Prod BOTB and Vendor OTB, Copy Low to High is not working correctly
				//GetCube(eCubeType.StorePlanWeekDetail).Clear(highLevelCellRef, 2);
				//ClearCube(eCubeType.StorePlanPeriodDetail, highLevelCellRef, 2);
				//ClearCube(eCubeType.StorePlanDateTotal, highLevelCellRef, 2);
				//ClearCube(eCubeType.StorePlanStoreTotalWeekDetail, highLevelCellRef, 2);
				//ClearCube(eCubeType.StorePlanStoreTotalPeriodDetail, highLevelCellRef, 2);
				//ClearCube(eCubeType.StorePlanStoreTotalDateTotal, highLevelCellRef, 2);
				//ClearCube(eCubeType.StorePlanGroupTotalWeekDetail, highLevelCellRef, 2);
				//ClearCube(eCubeType.StorePlanGroupTotalPeriodDetail, highLevelCellRef, 2);
				//ClearCube(eCubeType.StorePlanGroupTotalDateTotal, highLevelCellRef, 2);
				ClearCubeForHierarchyVersion(eCubeType.StorePlanWeekDetail, highLevelCellRef);
				ClearCubeForHierarchyVersion(eCubeType.StorePlanPeriodDetail, highLevelCellRef);
				ClearCubeForHierarchyVersion(eCubeType.StorePlanDateTotal, highLevelCellRef);
				ClearCubeForHierarchyVersion(eCubeType.StorePlanStoreTotalWeekDetail, highLevelCellRef);
				ClearCubeForHierarchyVersion(eCubeType.StorePlanStoreTotalPeriodDetail, highLevelCellRef);
				ClearCubeForHierarchyVersion(eCubeType.StorePlanStoreTotalDateTotal, highLevelCellRef);
				ClearCubeForHierarchyVersion(eCubeType.StorePlanGroupTotalWeekDetail, highLevelCellRef);
				ClearCubeForHierarchyVersion(eCubeType.StorePlanGroupTotalPeriodDetail, highLevelCellRef);
				ClearCubeForHierarchyVersion(eCubeType.StorePlanGroupTotalDateTotal, highLevelCellRef);
				//End Track #6427 - JSCott - Urgent - In Prod BOTB and Vendor OTB, Copy Low to High is not working correctly
				//End Scenario #1

				weekProfList = _openParms.GetWeekProfileList(this._SAB.ApplicationServerSession);

				for (i = 0; i < weekProfList.Count; i++)
				{
					lowLevelTotalCellRef[eProfileType.Week] = weekProfList[i].Key;
					highLevelCellRef[eProfileType.Week] = weekProfList[i].Key;

					foreach (StoreProfile storeProf in GetMasterProfileList(eProfileType.Store))
					{
						lowLevelTotalCellRef[eProfileType.Store] = storeProf.Key;
						highLevelCellRef[eProfileType.Store] = storeProf.Key;

						foreach (VariableProfile varProf in Transaction.PlanComputations.PlanVariables.VariableProfileList)
						{
							//Scenario #1
							lowLevelTotalCellRef[eProfileType.Variable] = varProf.Key;
							highLevelCellRef[eProfileType.Variable] = varProf.Key;

							if (highLevelCellRef.PlanCube.isDatabaseVariable(varProf, highLevelCellRef))
							{
								highLevelCellRef.SetLoadCellValue(lowLevelTotalCellRef.CurrentCellValue, false);
								highLevelCellRef.isCellChanged = true;
								//Begin Track #6086 - JScott - Urgent - Copy Low to High completely broken for B OTB
								highLevelCellRef.isCellLoadedFromDB = true;
								//End Track #6086 - JScott - Urgent - Copy Low to High completely broken for B OTB
							}
							//End Scenario #1
							//Scenario #2
//							if (varProf.DatabaseColumnName != null)
//							{
//								planCellRef[eProfileType.Variable] = varProf.Key;
//
//								if (planCellRef.PlanCube.doesCellExist(planCellRef))
//								{
//									if (aOnlyChanged)
//									{
//										if (planCellRef.isCellChanged)
//										{
//											valueColHash.Add(varProf, planCellRef.CurrentCellValue);
//											lockColHash.Add(varProf, planCellRef.isCellLocked);
//										}
//									}
//									else
//									{
//										if (planCellRef.CurrentCellValue != 0)
//										{
//											valueColHash.Add(varProf, planCellRef.CurrentCellValue);
//										}
//										if (planCellRef.isCellLocked)
//										{
//											lockColHash.Add(varProf, planCellRef.isCellLocked);
//										}
//									}
//
//									if (aResetChangeFlags)
//									{
//										planCellRef.ClearCellChanges();
//									}
//								}
//							}
							//End Scenario #2
						}
					}
				}

				//Scenario #1
				ClearUndoStack();
				//End Scenario #1
				//Begin Enhancement - JScott - Add Balance Low Levels functionality
				RecomputeCubes(true);
				//End Enhancement - JScott - Add Balance Low Levels functionality
				//Begin TT#1134 - JScott - MID 4.0 does not automatically save locked cells in the OTS forecast

				_userChanged = true;
				//End TT#1134 - JScott - MID 4.0 does not automatically save locked cells in the OTS forecast
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #6427 - JSCott - Urgent - In Prod BOTB and Vendor OTB, Copy Low to High is not working correctly
		//private void ClearCube(eCubeType aCubeType, CellReference aCellRef, int aNumDimensions)
		//{
		//    Cube cube;
		//    CellReference cellRef;

		//    try
		//    {
		//        cube = GetCube(aCubeType);
		//        cellRef = cube.CreateCellReference(aCellRef);
		//        cube.Clear(cellRef, aNumDimensions);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}
		private void ClearCubeForHierarchyVersion(eCubeType aCubeType, PlanCellReference aCellRef)
		{
			PlanCube cube;
			PlanCellReference cellRef;

			try
			{
				cube = (PlanCube)GetCube(aCubeType);
				cellRef = (PlanCellReference)cube.CreateCellReference(aCellRef);
				cube.ClearCubeForHierarchyVersion(cellRef);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End Track #6427 - JSCott - Urgent - In Prod BOTB and Vendor OTB, Copy Low to High is not working correctly

		/// <summary>
		/// Perform matrix balance to chain.  Balances the low level chain value to the low level all stores total.
		/// </summary>

		private void MatrixBalanceToChain(int aVariableNumber, bool aUseBasis, int aBasisIndex, ref int aRecomputesProcessed)
		{
			PlanCellReference chainPlanCellRef = null;
			PlanCellReference storePlanCellRef = null;
			PlanCellReference basisPlanCellRef = null;
			PlanSpread planSpread = new PlanSpread();
			ArrayList planCellValueList = null;
			ArrayList planCellRefList = null;
			int i;
			
			try
			{
				VariableProfile varProf = (VariableProfile)Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aVariableNumber);
				//Begin Track #6157 - JScott - Formula Conflict error when running matrix balance for BOP Inv
				//if (_openParms.GetDisplayType(SAB.ApplicationServerSession) == ePlanDisplayType.Week)
				//{
				//End Track #6157 - JScott - Formula Conflict error when running matrix balance for BOP Inv
				chainPlanCellRef = (PlanCellReference)GetCube(eCubeType.ChainPlanWeekDetail).CreateCellReference();
				chainPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
				chainPlanCellRef[eProfileType.Variable] = varProf.Key;

				storePlanCellRef = (PlanCellReference)GetCube(eCubeType.StorePlanStoreTotalWeekDetail).CreateCellReference();
				storePlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
				storePlanCellRef[eProfileType.Variable] = varProf.Key;

				if (aUseBasis)
				{
					basisPlanCellRef = (PlanCellReference)GetCube(eCubeType.StoreBasisStoreTotalWeekDetail).CreateCellReference();
					basisPlanCellRef[eProfileType.Basis] = _openParms.BasisProfileList[aBasisIndex].Key;
					basisPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
					basisPlanCellRef[eProfileType.Variable] = varProf.Key;
				}

				foreach (PlanProfile lowLevelPlanProf in _openParms.LowLevelPlanProfileList)
				{
					//Begin Track #6157 - JScott - Formula Conflict error when running matrix balance for BOP Inv
					//foreach (WeekProfile weekProf in _openParms.GetDetailDateProfileList(SAB.ApplicationServerSession))
					foreach (WeekProfile weekProf in _openParms.GetWeekProfileList(SAB.ApplicationServerSession))
					//End Track #6157 - JScott - Formula Conflict error when running matrix balance for BOP Inv
					{
						chainPlanCellRef[eProfileType.HierarchyNode] = lowLevelPlanProf.NodeProfile.Key;
						chainPlanCellRef[eProfileType.Version] = lowLevelPlanProf.VersionProfile.Key;
						chainPlanCellRef[eProfileType.Week] = weekProf.Key;

						storePlanCellRef[eProfileType.HierarchyNode] = lowLevelPlanProf.NodeProfile.Key;
						storePlanCellRef[eProfileType.Version] = lowLevelPlanProf.VersionProfile.Key;
						storePlanCellRef[eProfileType.Week] = weekProf.Key;

						try
						{
							if (aUseBasis)
							{
								basisPlanCellRef[eProfileType.HierarchyNode] = lowLevelPlanProf.NodeProfile.Key;
								basisPlanCellRef[eProfileType.Version] = lowLevelPlanProf.VersionProfile.Key;
								basisPlanCellRef[eProfileType.Week] = weekProf.Key;
								ArrayList planCellRefs = storePlanCellRef.GetSpreadDetailCellRefArray(false);
								ArrayList basisCellRefs = basisPlanCellRef.GetSpreadDetailCellRefArray(false);
								if (ProcessSpread(storePlanCellRef))
								{
									planSpread.ExecuteBasisSpread(
										chainPlanCellRef.CurrentCellValue,
										basisCellRefs,
										planCellRefs,
										null,
										Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.NumDecimals,
										out planCellValueList,
										out planCellRefList);
								}
								else
								{
									continue;
								}
							}
							else
							{
								ArrayList planCellRefs = storePlanCellRef.GetSpreadDetailCellRefArray(false);
								if (ProcessSpread(storePlanCellRef))
								{
									planSpread.ExecuteSimpleSpread(
										chainPlanCellRef.CurrentCellValue,
										planCellRefs,
										null,
										Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.NumDecimals,
										out planCellValueList,
										out planCellRefList);
								}
								else
								{
									continue;
								}
							}

							for (i = 0; i < planCellRefList.Count; i++)
							{
								((PlanCellReference)planCellRefList[i]).SetEntryCellValue((double)planCellValueList[i]);
							}
						}
						catch (NothingToSpreadException)
						{
							string message = MIDText.GetText(eMIDTextCode.msg_ChainWeekSpreadFailed);
							message = message.Replace("{0}", lowLevelPlanProf.NodeProfile.Text);
							message = message.Replace("{1}", weekProf.YearWeek.ToString(CultureInfo.CurrentCulture));
							throw new SpreadFailed(message, weekProf, lowLevelPlanProf, aRecomputesProcessed);
						}
						catch
						{
							throw;
						}
					}
				}
				//Begin Track #6157 - JScott - Formula Conflict error when running matrix balance for BOP Inv
				//}
				//else
				//{
				//    chainPlanCellRef = (PlanCellReference)GetCube(eCubeType.ChainPlanPeriodDetail).CreateCellReference();
				//    chainPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
				//    chainPlanCellRef[eProfileType.Variable] = varProf.Key;

				//    storePlanCellRef = (PlanCellReference)GetCube(eCubeType.StorePlanStoreTotalPeriodDetail).CreateCellReference();
				//    storePlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
				//    storePlanCellRef[eProfileType.Variable] = varProf.Key;

				//    if (aUseBasis)
				//    {
				//        basisPlanCellRef = (PlanCellReference)GetCube(eCubeType.StoreBasisStoreTotalPeriodDetail).CreateCellReference();
				//        basisPlanCellRef[eProfileType.Basis] = _openParms.BasisProfileList[aBasisIndex].Key;
				//        basisPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
				//        basisPlanCellRef[eProfileType.Variable] = varProf.Key;
				//    }

				//    foreach (PlanProfile lowLevelPlanProf in _openParms.LowLevelPlanProfileList)
				//    {
				//        foreach (DateProfile periodProf in _openParms.GetDetailDateProfileList(SAB.ApplicationServerSession))
				//        {
				//            chainPlanCellRef[eProfileType.HierarchyNode] = lowLevelPlanProf.NodeProfile.Key;
				//            chainPlanCellRef[eProfileType.Version] = lowLevelPlanProf.VersionProfile.Key;
				//            chainPlanCellRef[eProfileType.Period] = periodProf.Key;

				//            storePlanCellRef[eProfileType.HierarchyNode] = lowLevelPlanProf.NodeProfile.Key;
				//            storePlanCellRef[eProfileType.Version] = lowLevelPlanProf.VersionProfile.Key;
				//            storePlanCellRef[eProfileType.Period] = periodProf.Key;

				//            try
				//            {
				//                if (aUseBasis)
				//                {
				//                    basisPlanCellRef[eProfileType.HierarchyNode] = lowLevelPlanProf.NodeProfile.Key;
				//                    basisPlanCellRef[eProfileType.Version] = lowLevelPlanProf.VersionProfile.Key;
				//                    basisPlanCellRef[eProfileType.Period] = periodProf.Key;
				//                    ArrayList planCellRefs = storePlanCellRef.GetSpreadDetailCellRefArray(false);
				//                    ArrayList basisCellRefs = basisPlanCellRef.GetSpreadDetailCellRefArray(false);
				//                    if (ProcessSpread(storePlanCellRef))
				//                    {
				//                        planSpread.ExecuteBasisSpread(
				//                            chainPlanCellRef.CurrentCellValue,
				//                            basisCellRefs,
				//                            planCellRefs,
				//                            null,
				//                            Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.NumDecimals,
				//                            out planCellValueList,
				//                            out planCellRefList);
				//                    }
				//                    else
				//                    {
				//                        continue;
				//                    }
				//                }
				//                else
				//                {
				//                    ArrayList planCellRefs = storePlanCellRef.GetSpreadDetailCellRefArray(false);
				//                    if (ProcessSpread(storePlanCellRef))
				//                    {
				//                        planSpread.ExecuteSimpleSpread(
				//                            chainPlanCellRef.CurrentCellValue,
				//                            planCellRefs,
				//                            null,
				//                            Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.NumDecimals,
				//                            out planCellValueList,
				//                            out planCellRefList);
				//                    }
				//                    else
				//                    {
				//                        continue;
				//                    }
				//                }

				//                for (i = 0; i < planCellRefList.Count; i++)
				//                {
				//                    ((PlanCellReference)planCellRefList[i]).SetEntryCellValue((double)planCellValueList[i]);
				//                }
				//            }
				//            catch(NothingToSpreadException)
				//            {
				//                string message = MIDText.GetText(eMIDTextCode.msg_ChainPeriodSpreadFailed);
				//                message = message.Replace("{0}", lowLevelPlanProf.NodeProfile.Text);
				//                message = message.Replace("{1}", periodProf.Key.ToString(CultureInfo.CurrentCulture));
				//                throw new SpreadFailed(message, periodProf, lowLevelPlanProf, aRecomputesProcessed);
				//            }
				//            catch
				//            {
				//                throw;	
				//            }
				//        }
				//    }
				//}
				//End Track #6157 - JScott - Formula Conflict error when running matrix balance for BOP Inv

				_userChanged = true;
				RecomputeCubes(true);
				++aRecomputesProcessed;

				ClearUndoStack();
				System.GC.Collect();
				//Begin TT#1134 - JScott - MID 4.0 does not automatically save locked cells in the OTS forecast

				_userChanged = true;
				//End TT#1134 - JScott - MID 4.0 does not automatically save locked cells in the OTS forecast
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Perform matrix balance to store.  Balances the store high level value to the store low level total.
		/// </summary>

		private void MatrixBalanceToStore(int aVariableNumber, bool aUseBasis, int aBasisIndex, ref int aRecomputesProcessed)
		{
			PlanCellReference storeHighLevelPlanCellRef = null;
			PlanCellReference basisLowLevelTotalPlanCellRef = null;
			PlanCellReference storeLowLevelTotalPlanCellRef = null;
			PlanSpread planSpread = new PlanSpread();
			ArrayList planCellValueList = null;
			ArrayList planCellRefList = null;
			int i;

			try
			{
				//Begin Track #6157 - JScott - Formula Conflict error when running matrix balance for BOP Inv
				//if (_openParms.GetDisplayType(SAB.ApplicationServerSession) == ePlanDisplayType.Week)
				//{
				//End Track #6157 - JScott - Formula Conflict error when running matrix balance for BOP Inv
				storeHighLevelPlanCellRef = (PlanCellReference)GetCube(eCubeType.StorePlanWeekDetail).CreateCellReference();
				storeHighLevelPlanCellRef[eProfileType.HierarchyNode] = _openParms.StoreHLPlanProfile.NodeProfile.Key;
				storeHighLevelPlanCellRef[eProfileType.Version] = _openParms.StoreHLPlanProfile.VersionProfile.Key;
				storeHighLevelPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
				storeHighLevelPlanCellRef[eProfileType.Variable] = Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aVariableNumber).Key;

				storeLowLevelTotalPlanCellRef = (PlanCellReference)GetCube(eCubeType.StorePlanLowLevelTotalWeekDetail).CreateCellReference();
				storeLowLevelTotalPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
				storeLowLevelTotalPlanCellRef[eProfileType.Variable] = Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aVariableNumber).Key;
				storeLowLevelTotalPlanCellRef[eProfileType.LowLevelTotalVersion] = Include. FV_PlanLowLevelTotalRID;

				if (aUseBasis)
				{
					basisLowLevelTotalPlanCellRef = (PlanCellReference)GetCube(eCubeType.StoreBasisLowLevelTotalWeekDetail).CreateCellReference();
					basisLowLevelTotalPlanCellRef[eProfileType.Basis] = _openParms.BasisProfileList[aBasisIndex].Key;
					basisLowLevelTotalPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
					basisLowLevelTotalPlanCellRef[eProfileType.Variable] = Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aVariableNumber).Key;
					basisLowLevelTotalPlanCellRef[eProfileType.LowLevelTotalVersion] = Include. FV_PlanLowLevelTotalRID;
				}

				foreach (StoreProfile storeProfile in GetFilteredProfileList(eProfileType.Store))
				{
					//Begin Track #6157 - JScott - Formula Conflict error when running matrix balance for BOP Inv
					//foreach (WeekProfile weekProf in _openParms.GetDetailDateProfileList(SAB.ApplicationServerSession))
					foreach (WeekProfile weekProf in _openParms.GetWeekProfileList(SAB.ApplicationServerSession))
					//End Track #6157 - JScott - Formula Conflict error when running matrix balance for BOP Inv
					{
						storeHighLevelPlanCellRef[eProfileType.Week] = weekProf.Key;
						storeHighLevelPlanCellRef[eProfileType.Store] = storeProfile.Key;

						storeLowLevelTotalPlanCellRef[eProfileType.Week] = weekProf.Key;
						storeLowLevelTotalPlanCellRef[eProfileType.Store] = storeProfile.Key;

						try
						{
							if (aUseBasis)
							{
								basisLowLevelTotalPlanCellRef[eProfileType.Week] = weekProf.Key;
								basisLowLevelTotalPlanCellRef[eProfileType.Store] = storeProfile.Key;
								ArrayList planCellRefs = storeLowLevelTotalPlanCellRef.GetSpreadDetailCellRefArray(false);
								ArrayList basisCellRefs = basisLowLevelTotalPlanCellRef.GetSpreadDetailCellRefArray(false);
								if (ProcessSpread(storeLowLevelTotalPlanCellRef))
								{
									planSpread.ExecuteBasisSpread(
										storeHighLevelPlanCellRef.CurrentCellValue,
										basisCellRefs,
										planCellRefs,
										null,
										Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.NumDecimals,
										out planCellValueList,
										out planCellRefList);
								}
								else
								{
									continue;
								}
							}
							else
							{
								ArrayList planCellRefs = storeLowLevelTotalPlanCellRef.GetSpreadDetailCellRefArray(false);
								if (ProcessSpread(storeLowLevelTotalPlanCellRef))
								{
									planSpread.ExecuteSimpleSpread(
										storeHighLevelPlanCellRef.CurrentCellValue,
										planCellRefs,
										null,
										Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.NumDecimals,
										out planCellValueList,
										out planCellRefList);
								}
								else
								{
									continue;
								}
							}

							for (i = 0; i < planCellRefList.Count; i++)
							{
								((PlanCellReference)planCellRefList[i]).SetEntryCellValue((double)planCellValueList[i]);
							}

						}
						catch(NothingToSpreadException)
						{
							string message = MIDText.GetText(eMIDTextCode.msg_HighLevelWeekSpreadFailed);
							message = message.Replace("{0}", _openParms.StoreHLPlanProfile.NodeProfile.Text);
							message = message.Replace("{1}", storeProfile.Text);
							message = message.Replace("{2}", weekProf.YearWeek.ToString(CultureInfo.CurrentCulture));
							throw new SpreadFailed(message, weekProf, storeProfile, aRecomputesProcessed);
						}
						catch
						{
							throw;
						}
					}
				}
				//Begin Track #6157 - JScott - Formula Conflict error when running matrix balance for BOP Inv
				//}
				//else
				//{
				//    storeHighLevelPlanCellRef = (PlanCellReference)GetCube(eCubeType.StorePlanPeriodDetail).CreateCellReference();
				//    storeHighLevelPlanCellRef[eProfileType.HierarchyNode] = _openParms.StoreHLPlanProfile.NodeProfile.Key;
				//    storeHighLevelPlanCellRef[eProfileType.Version] = _openParms.StoreHLPlanProfile.VersionProfile.Key;
				//    storeHighLevelPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
				//    storeHighLevelPlanCellRef[eProfileType.Variable] = Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aVariableNumber).Key;

				//    storeLowLevelTotalPlanCellRef = (PlanCellReference)GetCube(eCubeType.StorePlanLowLevelTotalPeriodDetail).CreateCellReference();
				//    storeLowLevelTotalPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
				//    storeLowLevelTotalPlanCellRef[eProfileType.Variable] = Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aVariableNumber).Key;
				//    storeLowLevelTotalPlanCellRef[eProfileType.LowLevelTotalVersion] = Include. FV_PlanLowLevelTotalRID;

				//    if (aUseBasis)
				//    {
				//        basisLowLevelTotalPlanCellRef = (PlanCellReference)GetCube(eCubeType.StoreBasisLowLevelTotalPeriodDetail).CreateCellReference();
				//        basisLowLevelTotalPlanCellRef[eProfileType.Basis] = _openParms.BasisProfileList[aBasisIndex].Key;
				//        basisLowLevelTotalPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
				//        basisLowLevelTotalPlanCellRef[eProfileType.Variable] = Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aVariableNumber).Key;
				//        basisLowLevelTotalPlanCellRef[eProfileType.LowLevelTotalVersion] = Include. FV_PlanLowLevelTotalRID;
				//    }

				//    foreach (StoreProfile storeProfile in GetFilteredProfileList(eProfileType.Store))
				//    {
				//        foreach (DateProfile periodProf in _openParms.GetDetailDateProfileList(SAB.ApplicationServerSession))
				//        {
				//            storeHighLevelPlanCellRef[eProfileType.Period] = periodProf.Key;
				//            storeHighLevelPlanCellRef[eProfileType.Store] = storeProfile.Key;

				//            storeLowLevelTotalPlanCellRef[eProfileType.Period] = periodProf.Key;
				//            storeLowLevelTotalPlanCellRef[eProfileType.Store] = storeProfile.Key;

				//            try
				//            {
				//                if (aUseBasis)
				//                {
				//                    basisLowLevelTotalPlanCellRef[eProfileType.Period] = periodProf.Key;
				//                    basisLowLevelTotalPlanCellRef[eProfileType.Store] = storeProfile.Key;
				//                    ArrayList planCellRefs = storeLowLevelTotalPlanCellRef.GetSpreadDetailCellRefArray(false);
				//                    ArrayList basisCellRefs = basisLowLevelTotalPlanCellRef.GetSpreadDetailCellRefArray(false);
				//                    if (ProcessSpread(storeLowLevelTotalPlanCellRef))
				//                    {
				//                        planSpread.ExecuteBasisSpread(
				//                            storeHighLevelPlanCellRef.CurrentCellValue,
				//                            basisCellRefs,
				//                            planCellRefs,
				//                            null,
				//                            Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.NumDecimals,
				//                            out planCellValueList,
				//                            out planCellRefList);
				//                    }
				//                    else
				//                    {
				//                        continue;
				//                    }
				//                }
				//                else
				//                {
				//                    ArrayList planCellRefs = storeLowLevelTotalPlanCellRef.GetSpreadDetailCellRefArray(false);
				//                    // check to determine is spread should be attempted
				//                    if (ProcessSpread(storeLowLevelTotalPlanCellRef))
				//                    {
				//                        planSpread.ExecuteSimpleSpread(
				//                            storeHighLevelPlanCellRef.CurrentCellValue,
				//                            planCellRefs,
				//                            null,
				//                            Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.NumDecimals,
				//                            out planCellValueList,
				//                            out planCellRefList);
				//                    }
				//                    else
				//                    {
				//                        continue;
				//                    }
				//                }

				//                for (i = 0; i < planCellRefList.Count; i++)
				//                {
				//                    ((PlanCellReference)planCellRefList[i]).SetEntryCellValue((double)planCellValueList[i]);
				//                }
				//            }
				//            catch(NothingToSpreadException)
				//            {
				//                string message = MIDText.GetText(eMIDTextCode.msg_HighLevelPeriodSpreadFailed);
				//                message = message.Replace("{0}", _openParms.StoreHLPlanProfile.NodeProfile.Text);
				//                message = message.Replace("{1}", storeProfile.Text);
				//                message = message.Replace("{2}", periodProf.Key.ToString(CultureInfo.CurrentCulture));
				//                throw new SpreadFailed(message, periodProf, storeProfile, aRecomputesProcessed);
				//            }
				//            catch
				//            {
				//                throw;
				//            }
				//        }
				//    }
				//}
				//End Track #6157 - JScott - Formula Conflict error when running matrix balance for BOP Inv

				_userChanged = true;
				RecomputeCubes(true);
				++aRecomputesProcessed;

				ClearUndoStack();
				System.GC.Collect();
				//Begin TT#1134 - JScott - MID 4.0 does not automatically save locked cells in the OTS forecast

				_userChanged = true;
				//End TT#1134 - JScott - MID 4.0 does not automatically save locked cells in the OTS forecast
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private bool ProcessSpread(PlanCellReference aPlanCellRef)
		{
			try
			{
				if (aPlanCellRef.isCellProtected ||
					aPlanCellRef.isCellIneligible ||
					aPlanCellRef.isCellClosed ||
					aPlanCellRef.isCellReadOnly)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Get the matrix tolerance.
		/// </summary>

		public double GetTolerance(int aVariableNumber, eBalanceMode aBalanceMode)
		{
			try
			{
				if (aBalanceMode == eBalanceMode.Chain)
				{
					return	GetStoreTolerance(aVariableNumber);
				}
				else
				{
					return	GetChainTolerance(aVariableNumber);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private double GetChainTolerance(int aVariableNumber)
		{
			PlanCellReference chainPlanCellRef = null;
			PlanCellReference storeTotalPlanCellRef = null;
			double tolerance = 0;

			try
			{
				if (_openParms.GetDisplayType(SAB.ApplicationServerSession) == ePlanDisplayType.Week)
				{
					chainPlanCellRef = (PlanCellReference)GetCube(eCubeType.ChainPlanWeekDetail).CreateCellReference();
					chainPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
					chainPlanCellRef[eProfileType.Variable] = Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aVariableNumber).Key;

					storeTotalPlanCellRef = (PlanCellReference)GetCube(eCubeType.StorePlanStoreTotalWeekDetail).CreateCellReference();
					storeTotalPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
					storeTotalPlanCellRef[eProfileType.Variable] = Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aVariableNumber).Key;

					foreach (PlanProfile lowLevelPlanProf in _openParms.LowLevelPlanProfileList)
					{
						foreach (WeekProfile weekProf in _openParms.GetDetailDateProfileList(SAB.ApplicationServerSession))
						{
							chainPlanCellRef[eProfileType.HierarchyNode] = lowLevelPlanProf.NodeProfile.Key;
							chainPlanCellRef[eProfileType.Version] = lowLevelPlanProf.VersionProfile.Key;
							chainPlanCellRef[eProfileType.Week] = weekProf.Key;

							storeTotalPlanCellRef[eProfileType.HierarchyNode] = lowLevelPlanProf.NodeProfile.Key;
							storeTotalPlanCellRef[eProfileType.Version] = lowLevelPlanProf.VersionProfile.Key;
							storeTotalPlanCellRef[eProfileType.Week] = weekProf.Key;

							try
							{
								double storeTotalValue = storeTotalPlanCellRef.GetCellValue(eGetCellMode.Current, false);
								double chainValue = chainPlanCellRef.GetCellValue(eGetCellMode.Current, false);
								double tmpTolerance = ComputeTolerance(chainValue, storeTotalValue);
								if (tmpTolerance > tolerance)
								{
									tolerance = tmpTolerance;
								}
							}
							catch
							{
								throw;
							}
						}
					}
				}
				else
				{
					chainPlanCellRef = (PlanCellReference)GetCube(eCubeType.ChainPlanPeriodDetail).CreateCellReference();
					chainPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
					chainPlanCellRef[eProfileType.Variable] = Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aVariableNumber).Key;

					storeTotalPlanCellRef = (PlanCellReference)GetCube(eCubeType.StorePlanStoreTotalPeriodDetail).CreateCellReference();
					storeTotalPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
					storeTotalPlanCellRef[eProfileType.Variable] = Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aVariableNumber).Key;

					foreach (PlanProfile lowLevelPlanProf in _openParms.LowLevelPlanProfileList)
					{
						foreach (DateProfile periodProf in _openParms.GetDetailDateProfileList(SAB.ApplicationServerSession))
						{
							chainPlanCellRef[eProfileType.HierarchyNode] = lowLevelPlanProf.NodeProfile.Key;
							chainPlanCellRef[eProfileType.Version] = lowLevelPlanProf.VersionProfile.Key;
							chainPlanCellRef[eProfileType.Period] = periodProf.Key;

							storeTotalPlanCellRef[eProfileType.HierarchyNode] = lowLevelPlanProf.NodeProfile.Key;
							storeTotalPlanCellRef[eProfileType.Version] = lowLevelPlanProf.VersionProfile.Key;
							storeTotalPlanCellRef[eProfileType.Period] = periodProf.Key;

							try
							{
								double storeTotalValue = storeTotalPlanCellRef.GetCellValue(eGetCellMode.Current, false);
								double chainValue = chainPlanCellRef.GetCellValue(eGetCellMode.Current, false);
								double tmpTolerance = ComputeTolerance(chainValue, storeTotalValue);
								if (tmpTolerance > tolerance)
								{
									tolerance = tmpTolerance;
								}
							}
							catch
							{
								throw;
							}
						}
					}
				}
				return tolerance;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private double GetStoreTolerance(int aVariableNumber)
		{
			PlanCellReference storeHighLevelPlanCellRef = null;
			PlanCellReference storeLowLevelTotalPlanCellRef = null;
			double tolerance = 0;

			try
			{
				if (_openParms.GetDisplayType(SAB.ApplicationServerSession) == ePlanDisplayType.Week)
				{
					storeHighLevelPlanCellRef = (PlanCellReference)GetCube(eCubeType.StorePlanWeekDetail).CreateCellReference();
					storeHighLevelPlanCellRef[eProfileType.HierarchyNode] = _openParms.StoreHLPlanProfile.NodeProfile.Key;
					storeHighLevelPlanCellRef[eProfileType.Version] = _openParms.StoreHLPlanProfile.VersionProfile.Key;
					storeHighLevelPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
					storeHighLevelPlanCellRef[eProfileType.Variable] = Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aVariableNumber).Key;

					storeLowLevelTotalPlanCellRef = (PlanCellReference)GetCube(eCubeType.StorePlanLowLevelTotalWeekDetail).CreateCellReference();
					storeLowLevelTotalPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
					storeLowLevelTotalPlanCellRef[eProfileType.Variable] = Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aVariableNumber).Key;

					foreach (StoreProfile storeProfile in GetFilteredProfileList(eProfileType.Store))
					{
						foreach (WeekProfile weekProf in _openParms.GetDetailDateProfileList(SAB.ApplicationServerSession))
						{
							storeHighLevelPlanCellRef[eProfileType.Week] = weekProf.Key;
							storeHighLevelPlanCellRef[eProfileType.Store] = storeProfile.Key;

							storeLowLevelTotalPlanCellRef[eProfileType.Week] = weekProf.Key;
							storeLowLevelTotalPlanCellRef[eProfileType.Store] = storeProfile.Key;
							
							try
							{
								double lowLevelValue = storeLowLevelTotalPlanCellRef.GetCellValue(eGetCellMode.Current, false);
								double highLevelValue = storeHighLevelPlanCellRef.GetCellValue(eGetCellMode.Current, false);
								double tmpTolerance = ComputeTolerance(highLevelValue, lowLevelValue);
								if (tmpTolerance > tolerance)
								{
									tolerance = tmpTolerance;
								}
							}
							catch
							{
								throw;
							}
						}
					}
				}
				else
				{
					storeHighLevelPlanCellRef = (PlanCellReference)GetCube(eCubeType.StorePlanPeriodDetail).CreateCellReference();
					storeHighLevelPlanCellRef[eProfileType.HierarchyNode] = _openParms.StoreHLPlanProfile.NodeProfile.Key;
					storeHighLevelPlanCellRef[eProfileType.Version] = _openParms.StoreHLPlanProfile.VersionProfile.Key;
					storeHighLevelPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
					storeHighLevelPlanCellRef[eProfileType.Variable] = Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aVariableNumber).Key;

					storeLowLevelTotalPlanCellRef = (PlanCellReference)GetCube(eCubeType.StorePlanLowLevelTotalPeriodDetail).CreateCellReference();
					storeLowLevelTotalPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
					storeLowLevelTotalPlanCellRef[eProfileType.Variable] = Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aVariableNumber).Key;

					foreach (StoreProfile storeProfile in GetFilteredProfileList(eProfileType.Store))
					{
						foreach (DateProfile periodProf in _openParms.GetDetailDateProfileList(SAB.ApplicationServerSession))
						{
							storeHighLevelPlanCellRef[eProfileType.Period] = periodProf.Key;
							storeHighLevelPlanCellRef[eProfileType.Store] = storeProfile.Key;

							storeLowLevelTotalPlanCellRef[eProfileType.Period] = periodProf.Key;
							storeLowLevelTotalPlanCellRef[eProfileType.Store] = storeProfile.Key;

							try
							{
								double lowLevelValue = storeLowLevelTotalPlanCellRef.GetCellValue(eGetCellMode.Current, false);
								double highLevelValue = storeHighLevelPlanCellRef.GetCellValue(eGetCellMode.Current, false);
								double tmpTolerance = ComputeTolerance(highLevelValue, lowLevelValue);
								if (tmpTolerance > tolerance)
								{
									tolerance = tmpTolerance;
								}
							}
							catch
							{
								throw;
							}
						}
					}
				}

				return tolerance;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private double ComputeTolerance(double aValue1, double aValue2)
		{
			try
			{
				double dividend, divisor;

				if (aValue2 > aValue1)
				{
					dividend = aValue1;
					divisor = aValue2;
				}
				else
				{
					divisor = aValue1;
					dividend = aValue2;
				}

				if (divisor == 0)
				{
					if (dividend == 0)
					{
						return 0;
					}
					else
					{
						return double.MaxValue;
					}
				}
				return (dividend/divisor) * 100;
			}
			catch
			{
				throw;
			}
		}

        // BEGIN MID Track #5647 - KJohnson - Matrix Forecast
        public void SpreadHighToLowLevelStore(int aVariableNumber, bool useBasis, bool ignoreLocks)
        {
            PlanCellReference storePlanCellRef = null;
            PlanCellReference storeLowPlanCellRef = null;
            //PlanCellReference basisCellRef = null;
            PlanCellReference totalLowbasisCellRef = null;
            ArrayList planCellRefs = null;
            //ArrayList basisCellRefs = null;
            double[] basisValueArray = null;
            double[] staticBasisValueArray = null;
            //int maxBasisWeeks;
            WeekProfile wp;
            ProfileList planProfileList;
            PlanSpread planSpread = new PlanSpread();
            ArrayList planCellValueList = null;
            ArrayList planCellRefList = null;
            //VariableProfile varProf = (VariableProfile)this.Variables.VariableProfileList.FindKey(aVariableNumber);
            VariableProfile varProf = (VariableProfile)Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aVariableNumber);
            int i;

            try
            {
                foreach (StoreProfile storeProfile in GetFilteredProfileList(eProfileType.Store))
                {
                    storePlanCellRef = (PlanCellReference)GetCube(eCubeType.StorePlanWeekDetail).CreateCellReference();
                    //storePlanCellRef[eProfileType.QuantityVariable] = QuantityVariables.ValueQuantity.Key;
                    storePlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
                    storePlanCellRef[eProfileType.Variable] = varProf.Key;
                    storePlanCellRef[eProfileType.HierarchyNode] = _openParms.StoreHLPlanProfile.NodeProfile.Key;
                    storePlanCellRef[eProfileType.Version] = _openParms.StoreHLPlanProfile.VersionProfile.Key;
                    storePlanCellRef[eProfileType.Store] = storeProfile.Key;

                    planProfileList = _openParms.GetWeekProfileList(SAB.ApplicationServerSession);
                    WeekProfile firstWeekOfPlan = (WeekProfile)planProfileList[0];

                    // BEGIN Issue 5551 stodd 6.2.2008
                    //basisCellRefs = null;
                    //basisValueArray = null;
                    // END Issue 5551

                    foreach (WeekProfile weekProf in planProfileList.ArrayList)
                    {
                        basisValueArray = null;
                        storePlanCellRef[eProfileType.Week] = weekProf.Key;

                        //======================
                        // Low Level Plan list
                        //======================

                        planCellRefs = new ArrayList();
                        storeLowPlanCellRef = (PlanCellReference)GetCube(eCubeType.StorePlanLowLevelTotalWeekDetail).CreateCellReference();
                        //storeLowPlanCellRef[eProfileType.QuantityVariable] = QuantityVariables.ValueQuantity.Key;
                        storeLowPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
                        storeLowPlanCellRef[eProfileType.Variable] = varProf.Key;
                        storeLowPlanCellRef[eProfileType.LowLevelTotalVersion] = Include.FV_PlanLowLevelTotalRID;
                        storeLowPlanCellRef[eProfileType.Week] = weekProf.Key;
                        storeLowPlanCellRef[eProfileType.Store] = storeProfile.Key;
                        planCellRefs = storeLowPlanCellRef.GetSpreadDetailCellRefArray(true);

                        try
                        {
                            if (useBasis)
                            {
                                //========================================================================================
                                // A note about what's expected when reading the bais information.
                                // When the basis is dynamic to plan:
                                //   Only 1 Basis detail profile per basis profile is expected. Each must be accumed
                                //   seperately due to the nature of basis to plan dates.
                                // Any other basis type (static or dyn to current):
                                //   The basis profile must contain any and all basis infomation as basis detail. 
                                //   ie, 1 basis profile and 1 or more basis detail profiles
                                //========================================================================================
                                staticBasisValueArray = null;
                                foreach (BasisProfile bp in _openParms.BasisProfileList.ArrayList)
                                {
                                    bool dynToPlan = false;
                                    foreach (BasisDetailProfile bdp in bp.BasisDetailProfileList.ArrayList)
                                    {
                                        if (bdp.DateRangeProfile.DateRangeType == eCalendarRangeType.Dynamic
                                            && bdp.DateRangeProfile.RelativeTo == eDateRangeRelativeTo.Plan)
                                        {
                                            dynToPlan = true;
                                        }
                                    }

                                    if (dynToPlan)
                                    {
                                        WeekProfile fromWeek = weekProf;
                                        BasisDetailProfile bdp = (BasisDetailProfile)bp.BasisDetailProfileList[0];
                                        if (bdp.DateRangeProfile.SelectedDateType == eCalendarDateType.Period)
                                        {
                                            WeekProfile PriorWeek = _SAB.ApplicationServerSession.Calendar.Add(fromWeek, -1);

                                            if (fromWeek.Period != PriorWeek.Period && fromWeek != firstWeekOfPlan)
                                            {
                                                fromWeek = bdp.ForecastingInfo.ShiftDateRange(_SAB.ApplicationServerSession.Calendar);
                                            }
                                            else
                                            {
                                                fromWeek = bdp.ForecastingInfo.PlanWeek;
                                            }
                                        }

                                        WeekProfile toWeek = _SAB.ApplicationServerSession.Calendar.Add(fromWeek, bdp.ForecastingInfo.OrigWeekListCount - 1);
                                        ProfileList weekList = _SAB.ApplicationServerSession.Calendar.GetWeekRange(fromWeek, toWeek);
                                        double[] dynBasisValueArray = GetDynBasisForSpreadStore(weekList, varProf, bp.Key, storeProfile.Key);
                                        AddBasisArrays(ref basisValueArray, dynBasisValueArray);
                                    }
                                    else
                                    {
                                        if (staticBasisValueArray == null)
                                            staticBasisValueArray = GetStaticBasisForSpreadStore(planProfileList, varProf, bp.Key, storeProfile.Key);

                                        AddBasisArrays(ref basisValueArray, staticBasisValueArray);
                                    }
                                }

                                if (varProf.VariableSpreadType == eVariableSpreadType.PctContribution)
                                {
                                    planSpread.ExecuteBasisSpread(
                                        storePlanCellRef.CurrentCellValue,
                                        basisValueArray,
                                        planCellRefs,
                                        null,
                                        //QuantityVariables.ValueQuantity.NumDecimals,
                                        Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.NumDecimals,
                                        out planCellValueList,
                                        out planCellRefList,
                                        ignoreLocks);
                                }
                                else if (varProf.VariableSpreadType == eVariableSpreadType.PctChange)
                                {
                                    //======================
                                    // BASIS low level total
                                    //======================
                                    totalLowbasisCellRef = (PlanCellReference)GetCube(eCubeType.StoreBasisLowLevelTotalWeekDetail).CreateCellReference();
                                    //totalLowbasisCellRef[eProfileType.QuantityVariable] = QuantityVariables.ValueQuantity.Key;
                                    totalLowbasisCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
                                    totalLowbasisCellRef[eProfileType.Variable] = varProf.Key;
                                    totalLowbasisCellRef[eProfileType.LowLevelTotalVersion] = Include.FV_PlanLowLevelTotalRID;
                                    totalLowbasisCellRef[eProfileType.Week] = weekProf.Key;
                                    totalLowbasisCellRef[eProfileType.Basis] = 1;

                                    //=====================================================================================
                                    // When a basis is present for a pct change spread, we send the low level basis total
                                    // and the low level value list to the method instead of the plan equivalents.
                                    //=====================================================================================
                                    planSpread.ExecutePctChangeSpread(
                                        totalLowbasisCellRef.CurrentCellValue,
                                        storePlanCellRef.CurrentCellValue,
                                        basisValueArray,
                                        planCellRefs,
                                        null,
                                        //QuantityVariables.ValueQuantity.NumDecimals,
                                        Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.NumDecimals,
                                        out planCellValueList,
                                        out planCellRefList,
                                        ignoreLocks);
                                }
                            }
                            else   // No Basis 
                            {
                                if (varProf.VariableSpreadType == eVariableSpreadType.PctContribution)
                                {
                                    planSpread.ExecuteSimpleSpread(
                                        storePlanCellRef.CurrentCellValue,
                                        planCellRefs,
                                        null,
                                        //QuantityVariables.ValueQuantity.NumDecimals,
                                        Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.NumDecimals,
                                        out planCellValueList,
                                        out planCellRefList,
                                        ignoreLocks);
                                }
                                else if (varProf.VariableSpreadType == eVariableSpreadType.PctChange)
                                {
                                    planSpread.ExecutePctChangeSpread(
                                        storeLowPlanCellRef.CurrentCellValue,
                                        storePlanCellRef.CurrentCellValue,
                                        planCellRefs,
                                        planCellRefs,
                                        null,
                                        //QuantityVariables.ValueQuantity.NumDecimals,
                                        Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.NumDecimals,
                                        out planCellValueList,
                                        out planCellRefList,
                                        ignoreLocks);
                                }
                            }


                            //==================================================================================
                            // Update Low levels
                            //==================================================================================
                            for (i = 0; i < planCellRefList.Count; i++)
                            {
                                if (ignoreLocks && ((PlanCellReference)planCellRefList[i]).isCellLocked)
                                    ((PlanCellReference)planCellRefList[i]).SetCellLock(false);
                                // Begin track # 5647 - set ENTRY cell value so change rules are kicked off
                                ((PlanCellReference)planCellRefList[i]).SetEntryCellValue((double)planCellValueList[i]);
                                // End track # 5647 
                                ((PlanCellReference)planCellRefList[i]).isCellChanged = true;

                            }
                        }
                        catch (NothingToSpreadException)
                        {
                            string message = MIDText.GetText(eMIDTextCode.msg_StoreWeekLowLvlSpreadFailed);
                            message = message.Replace("{0}", _openParms.StoreHLPlanProfile.NodeProfile.Text);
                            message = message.Replace("{1}", weekProf.YearWeek.ToString(CultureInfo.CurrentCulture));
                            _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_ChainWeekLowLvlSpreadFailed, message, this.ToString());
                        }
                        catch
                        {
                            throw;
                        }

                    }
                }

                MIDTimer recomputeTimer = new MIDTimer();
                recomputeTimer.Start();
                //RecomputePlanCubes();
                RecomputeCubes(true);
                recomputeTimer.Stop("Store Spread RecomputePlanCubes");
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private double[] GetStaticBasisForSpreadStore(ProfileList planProfileList, VariableProfile varProf, int basis, int aStoreProfileKey)
        {
            PlanCellReference basisCellRef = null;
            ArrayList basisCellRefs = null;
            int maxBasisWeeks;
            WeekProfile wp;
            int i;
            double[] basisValueArray = null;

            //============================
            // Get max basis week count
            //============================
            maxBasisWeeks = 0;
            foreach (BasisDetailProfile bdp in ((BasisProfile)_openParms.BasisProfileList[0]).BasisDetailProfileList.ArrayList)
            {
                if (bdp.GetWeekProfileList(SAB.ApplicationServerSession).Count > maxBasisWeeks)
                {
                    maxBasisWeeks = bdp.GetWeekProfileList(SAB.ApplicationServerSession).Count;
                }
            }

            //=======================
            // Low Level Basis List
            //=======================

            for (int w = 0; w < maxBasisWeeks; w++)
            {
                wp = (WeekProfile)planProfileList[w];
                basisCellRef = (PlanCellReference)GetCube(eCubeType.StoreBasisLowLevelTotalWeekDetail).CreateCellReference();
                //basisCellRef[eProfileType.QuantityVariable] = QuantityVariables.ValueQuantity.Key;
                basisCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
                basisCellRef[eProfileType.Variable] = varProf.Key;
                basisCellRef[eProfileType.LowLevelTotalVersion] = Include.FV_PlanLowLevelTotalRID;
                basisCellRef[eProfileType.Week] = wp.Key;
                basisCellRef[eProfileType.Basis] = basis;
                basisCellRef[eProfileType.Store] = aStoreProfileKey;
                basisCellRefs = basisCellRef.GetSpreadDetailCellRefArray(true);

                //====================================================================
                // Used to accum the basis values for each week defined in the basis.
                //====================================================================
                if (basisValueArray == null)
                {
                    basisValueArray = new double[basisCellRefs.Count];
                }
                for (i = 0; i < basisCellRefs.Count; i++)
                {
                    basisValueArray[i] += ((PlanCellReference)basisCellRefs[i]).CurrentCellValue;
                }
            }
            return basisValueArray;
        }

        /// <summary>
        /// Adds up Dynamic to Plan basis weeks.
        /// </summary>
        /// <param name="planProfileList"></param>
        /// <param name="varProf"></param>
        /// <param name="basis"></param>
        /// <returns></returns>
        private double[] GetDynBasisForSpreadStore(ProfileList planProfileList, VariableProfile varProf, int basis, int aStoreProfileKey)
        {
            PlanCellReference basisCellRef = null;
            ArrayList basisCellRefs = null;
            int maxBasisWeeks;
            WeekProfile wp;
            int i;
            double[] basisValueArray = null;

            //============================
            // Get max basis week count
            //============================
            maxBasisWeeks = planProfileList.Count;


            //=======================
            // Low Level Basis List
            //=======================

            for (int w = 0; w < maxBasisWeeks; w++)
            {
                wp = (WeekProfile)planProfileList[w];
                basisCellRef = (PlanCellReference)GetCube(eCubeType.StoreBasisLowLevelTotalWeekDetail).CreateCellReference();
                //basisCellRef[eProfileType.QuantityVariable] = QuantityVariables.ValueQuantity.Key;
                basisCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
                basisCellRef[eProfileType.Variable] = varProf.Key;
                basisCellRef[eProfileType.LowLevelTotalVersion] = Include.FV_PlanLowLevelTotalRID;
                basisCellRef[eProfileType.Week] = wp.Key;
                basisCellRef[eProfileType.Basis] = basis;
                basisCellRef[eProfileType.Store] = aStoreProfileKey;
                basisCellRefs = basisCellRef.GetSpreadDetailCellRefArray(true);

                //====================================================================
                // Used to accum the basis values for each week defined in the basis.
                //====================================================================
                if (basisValueArray == null)
                {
                    basisValueArray = new double[basisCellRefs.Count];
                }
                for (i = 0; i < basisCellRefs.Count; i++)
                {
                    basisValueArray[i] += ((PlanCellReference)basisCellRefs[i]).HiddenCurrentCellValue;
                }
            }
            return basisValueArray;
        }

        private void AddBasisArrays(ref double[] totalArray, double[] valueArray)
        {
            if (totalArray == null)
            {
                totalArray = new double[valueArray.Length];
            }

            for (int i = 0; i < valueArray.Length; i++)
            {
                totalArray[i] = totalArray[i] + valueArray[i];
            }

        }
        // END Issue 5578/5681
        // END MID Track #5647

        // Begin TT#5276 - JSmith - Read Only Saves
        override public bool IsNodeEnqueued(int aNodeKey, bool aIsChain)
        {
            if (aIsChain)
            {
                // Begin TT#5385 - JSmith - Receiving an error when opening Multi-Level OTS Forecasts - Chain
                if (!_chainPlanEnqueue.ContainsKey(aNodeKey))
                {
                    return false;
                }
                // End TT#5385 - JSmith - Receiving an error when opening Multi-Level OTS Forecasts - Chain
                return ((PlanEnqueueInfo)_chainPlanEnqueue[aNodeKey]).PlanEnqueue.isEnqueued;
            }
            else
            {
                // Begin TT#5385 - JSmith - Receiving an error when opening Multi-Level OTS Forecasts - Chain
                if (!_storePlanEnqueue.ContainsKey(aNodeKey))
                {
                    return false;
                }
                // End TT#5385 - JSmith - Receiving an error when opening Multi-Level OTS Forecasts - Chain
                return ((PlanEnqueueInfo)_storePlanEnqueue[aNodeKey]).PlanEnqueue.isEnqueued;
            }
        }
        // End TT#5276 - JSmith - Read Only Saves
	}
}
