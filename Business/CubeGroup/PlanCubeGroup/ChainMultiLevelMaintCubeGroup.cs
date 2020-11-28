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
	/// This class defines the ChainPlanMaintCubeGroup.
	/// </summary>
	/// <remarks>
	/// The ChainPlanMaintCubeGroup predefines all Cubes that are required for Chain Plan Maintenance.  Those Cubes include ChainDetailWeekCube, LowLevelTotalWeekCube,
	/// ChainGroupTotalCube, ChainDetailWeekTimeTotalCube, and LowLevelTotalWeekTimeTotalCube.
	/// </remarks>

	public class ChainMultiLevelPlanMaintCubeGroup : PlanCubeGroup
	{
		//=======
		// FIELDS
		//=======

		private PlanEnqueueGroup _planEnqGrp;
		private Hashtable _chainPlanEnqueue;
		private bool _anyLowLevelEnqueued;

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

		public ChainMultiLevelPlanMaintCubeGroup(
			SessionAddressBlock aSAB,
			Transaction aTransaction)
			: base(aSAB, aTransaction)
		{
			try
			{
				_planEnqGrp = new PlanEnqueueGroup();
				_chainPlanEnqueue = new Hashtable();
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
		/// Opens a ChainPlanMaintCubeGroup and creats master Profile Lists, XRef Lists, and Cubes that are required for support.
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
			ChainVariableFilter varFilter;
			IDictionaryEnumerator dictEnum;
			ProfileList planWeekList;
			ProfileList planPeriodList;
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
				versionProfileList = GetMasterProfileList(eProfileType.Version);

				//============================================
				// Enqueue the chain and low-level chain plans
				//============================================

				_chainPlanEnqueue[_openParms.ChainHLPlanProfile.Key] = new PlanEnqueueInfo(_SAB, _openParms.ChainHLPlanProfile, ePlanType.Chain, planWeekList[0].Key, planWeekList[planWeekList.Count - 1].Key);

				foreach (PlanProfile planProf in _openParms.LowLevelPlanProfileList)
				{
					_chainPlanEnqueue[planProf.Key] = new PlanEnqueueInfo(_SAB, planProf, ePlanType.Chain, planWeekList[0].Key, planWeekList[planWeekList.Count - 1].Key);
				}

				if (_openParms.FunctionSecurityProfile.AllowUpdate)
				{
					planEnqueueList = new ArrayList();

					dictEnum = _chainPlanEnqueue.GetEnumerator();

					while (dictEnum.MoveNext())
					{
						planEnqueueList.Add(dictEnum.Value);
					}

					_planEnqGrp.EnqueuePlans(_SAB, planEnqueueList, _openParms.AllowReadOnlyOnConflict, _openParms.FormatErrorsForMessageBox, _openParms.UpdateAuditHeaderOnError);
				}

				//============================================
				// Check to see if any low-levels are enqueued
				//============================================

				_anyLowLevelEnqueued = false;

				foreach (PlanProfile planProf in _openParms.LowLevelPlanProfileList)
				{
					if (((PlanEnqueueInfo)_chainPlanEnqueue[planProf.Key]).PlanEnqueue.isEnqueued)
					{
						_anyLowLevelEnqueued = true;
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

				foreach (PlanProfile planProf in _openParms.LowLevelPlanProfileList)
				{
					if (!hierarchyNodeProfileList.Contains(planProf.Key))
					{
						hierarchyNodeProfileList.Add(planProf.NodeProfile);
					}
				}

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

				//=========================
				// Create and apply filters
				//=========================

				varFilter = new ChainVariableFilter(this);
				ApplyFilter(varFilter, eFilterType.Permanent);

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
					planCube = new ChainPlanWeekDetail(SAB, Transaction, this, cubeDef, 1, !((PlanEnqueueInfo)_chainPlanEnqueue[_openParms.ChainHLPlanProfile.NodeProfile.Key]).PlanEnqueue.isEnqueued && !_anyLowLevelEnqueued, true);
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
					planCube = new ChainPlanPeriodDetail(SAB, Transaction, this, cubeDef, 2, !((PlanEnqueueInfo)_chainPlanEnqueue[_openParms.ChainHLPlanProfile.NodeProfile.Key]).PlanEnqueue.isEnqueued && !_anyLowLevelEnqueued, true);
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
					planCube = new ChainPlanDateTotal(SAB, Transaction, this, cubeDef, 3, !((PlanEnqueueInfo)_chainPlanEnqueue[_openParms.ChainHLPlanProfile.NodeProfile.Key]).PlanEnqueue.isEnqueued && !_anyLowLevelEnqueued, true);
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
					planCube = new ChainPlanLowLevelTotalWeekDetail(SAB, Transaction, this, cubeDef, 4, !_anyLowLevelEnqueued);
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
					planCube = new ChainPlanLowLevelTotalPeriodDetail(SAB, Transaction, this, cubeDef, 5, !_anyLowLevelEnqueued);
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
					planCube = new ChainPlanLowLevelTotalDateTotal(SAB, Transaction, this, cubeDef, 6, !_anyLowLevelEnqueued);
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
			ProfileList chainHighLevelWeekProfList = null;
			ProfileList chainLowLevelWeekProfList = null;
			PlanEnqueueGroup planEnqGrp = null;
			ChainPlanWeekDetail chainCube;
			bool overrideChainHigh;
			Hashtable overrideChainLow;

			try
			{
				planEnqList = new ArrayList();
				chainCube = (ChainPlanWeekDetail)GetCube(eCubeType.ChainPlanWeekDetail);
				overrideChainHigh = false;
				overrideChainLow = new Hashtable();

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
					planEnqGrp.EnqueuePlans(_SAB, planEnqList, false, true, true);

					if (chainCube != null)
					{
						if (aSaveParms.SaveChainHighLevel)
						{
							if (overrideChainHigh)
							{
								chainCube.SaveCube(
									_openParms.ChainHLPlanProfile.NodeProfile.Key,
									_openParms.ChainHLPlanProfile.VersionProfile.Key,
									aSaveParms.ChainHighLevelNodeRID,
									aSaveParms.ChainHighLevelVersionRID,
									//Begin Enhancement - JScott - Add Balance Low Levels functionality
									aSaveParms.SaveLowLevelChainAsChain,
									//End Enhancement - JScott - Add Balance Low Levels functionality
									chainHighLevelWeekProfList,
									false,
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
									//Begin Enhancement - JScott - Add Balance Low Levels functionality
									aSaveParms.SaveLowLevelChainAsChain,
									//End Enhancement - JScott - Add Balance Low Levels functionality
									_openParms.GetWeekProfileList(SAB.ApplicationServerSession),
									true,
									//Begin Track #5690 - JScott - Can not save low to high
									//true,
									//End Track #5690 - JScott - Can not save low to high
									aSaveParms.SaveLocks);
							}
							//Begin Track #5950 - JScott - Save Low Level to High may get warning message

							if (aSaveParms.SaveLowLevelChainAsChain)
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
										aSaveParms.ChainLowLevelVersionRID,
										//Begin Enhancement - JScott - Add Balance Low Levels functionality
										false,
										//End Enhancement - JScott - Add Balance Low Levels functionality
										chainLowLevelWeekProfList,
										false,
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
										//Begin Enhancement - JScott - Add Balance Low Levels functionality
										false,
										//End Enhancement - JScott - Add Balance Low Levels functionality
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

        // Begin TT#2131-MD - JSmith - Halo Integration
        override public void ExtractCubeGroup(ExtractOptions aExtractOptions)
        {
        }
        // End TT#2131-MD - JSmith - Halo Integration

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

		public void GetReadOnlyFlags(out bool aChainReadOnly, out bool aLowLevelChainReadOnly)
		{
			try
			{
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
		/// Determines if changes have occurred to either the chain cube.
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
			return aVarProf.isDatabaseVariable(eVariableCategory.Chain, aPlanCellRef.GetVersionProfile().Key, eCalendarDateType.Week);
		}
		public void SpreadHighToLowLevelChain(bool useBasis, bool ignoreLocks)
		{
			PlanCellReference chainPlanCellRef = null;
			PlanCellReference chainLowPlanCellRef = null;
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
			int i;

			try
			{
				foreach (VariableProfile varProf in Transaction.PlanComputations.PlanVariables.VariableProfileList)
				{
					if (varProf.ChainDatabaseVariableType != eVariableDatabaseType.None)
					{
						chainPlanCellRef = (PlanCellReference)GetCube(eCubeType.ChainPlanWeekDetail).CreateCellReference();
						chainPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
						chainPlanCellRef[eProfileType.Variable] = varProf.Key;
						chainPlanCellRef[eProfileType.HierarchyNode] = _openParms.ChainHLPlanProfile.NodeProfile.Key;
						chainPlanCellRef[eProfileType.Version] = _openParms.ChainHLPlanProfile.VersionProfile.Key;

						planProfileList = _openParms.GetWeekProfileList(SAB.ApplicationServerSession);
						WeekProfile firstWeekOfPlan = (WeekProfile)planProfileList[0];

						// BEGIN Issue 5551 stodd 6.2.2008
						//basisCellRefs = null;
						//basisValueArray = null;
						// END Issue 5551

						foreach (WeekProfile weekProf in planProfileList.ArrayList)
						{
							basisValueArray = null;
							chainPlanCellRef[eProfileType.Week] = weekProf.Key;

							//======================
							// Low Level Plan list
							//======================

							planCellRefs = new ArrayList();
							chainLowPlanCellRef = (PlanCellReference)GetCube(eCubeType.ChainPlanLowLevelTotalWeekDetail).CreateCellReference();
							chainLowPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
							chainLowPlanCellRef[eProfileType.Variable] = varProf.Key;
							chainLowPlanCellRef[eProfileType.LowLevelTotalVersion] = Include.FV_PlanLowLevelTotalRID;
							chainLowPlanCellRef[eProfileType.Week] = weekProf.Key;
							planCellRefs = chainLowPlanCellRef.GetSpreadDetailCellRefArray(true);

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
											double [] dynBasisValueArray = GetDynBasisForSpread(weekList, varProf, bp.Key);
											AddBasisArrays(ref basisValueArray, dynBasisValueArray);
										}
										else
										{
											if (staticBasisValueArray == null)
												staticBasisValueArray = GetStaticBasisForSpread(planProfileList, varProf, bp.Key);

											AddBasisArrays(ref basisValueArray, staticBasisValueArray);
										}
									}

									if (varProf.VariableSpreadType == eVariableSpreadType.PctContribution)
									{
										planSpread.ExecuteBasisSpread(
											chainPlanCellRef.CurrentCellValue,
											basisValueArray,
											planCellRefs,
											null,
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
										totalLowbasisCellRef = (PlanCellReference)GetCube(eCubeType.ChainBasisLowLevelTotalWeekDetail).CreateCellReference();
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
											chainPlanCellRef.CurrentCellValue,
											basisValueArray,
											planCellRefs,
											null,
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
											chainPlanCellRef.CurrentCellValue,
											planCellRefs,
											null,
											Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.NumDecimals,
											out planCellValueList,
											out planCellRefList,
											ignoreLocks);
									}
									else if (varProf.VariableSpreadType == eVariableSpreadType.PctChange)
									{
										planSpread.ExecutePctChangeSpread(
											chainLowPlanCellRef.CurrentCellValue,
											chainPlanCellRef.CurrentCellValue,
											planCellRefs,
											planCellRefs,
											null,
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
									((PlanCellReference)planCellRefList[i]).SetLoadCellValue((double)planCellValueList[i], ((PlanCellReference)planCellRefList[i]).isCellLocked);
									((PlanCellReference)planCellRefList[i]).isCellChanged = true;

								}
							}
							catch (NothingToSpreadException)
							{
								string message = MIDText.GetText(eMIDTextCode.msg_ChainWeekLowLvlSpreadFailed);
								message = message.Replace("{0}", _openParms.ChainHLPlanProfile.NodeProfile.Text);
								message = message.Replace("{1}", weekProf.YearWeek.ToString(CultureInfo.CurrentCulture));
								_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_ChainWeekLowLvlSpreadFailed, message, this.ToString());
							}
							catch
							{
								throw;
							}

						}
					}
				}

				RecomputeCubes(true);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		// BEGIN Issue 5578/5681 stodd 7.18.2008
		private double[] GetStaticBasisForSpread( ProfileList planProfileList, VariableProfile varProf, int basis)
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

			// BEGIN TT#2668 - stodd - Forecast Spread abend
			// If there are more weeks in the basis than in the plan,
			// we temporarily have to add additional weeks to the plan line so we can read the basis
			// Info.
			ProfileList planWeeks = new ProfileList(eProfileType.Week);
			foreach (WeekProfile wp1 in planProfileList.ArrayList)
			{
				planWeeks.Add(wp1);
			}
			if (planProfileList.Count < maxBasisWeeks)
			{
				for (int w = planProfileList.Count; w < maxBasisWeeks; w++)
				{
					WeekProfile awp = _SAB.ApplicationServerSession.Calendar.Add((WeekProfile)planWeeks[planWeeks.Count - 1], 1);
					planWeeks.Add(awp);
				}
			}
			// END TT#2668 - stodd
			//=======================
			// Low Level Basis List
			//=======================

			for (int w = 0; w < maxBasisWeeks; w++)
			{
				// BEGIN TT#2668 - stodd - Forecast Spread abend
				wp = (WeekProfile)planWeeks[w];
				//wp = (WeekProfile)planProfileList[w];
				// END TT#2668 - stodd - Forecast Spread abend
				basisCellRef = (PlanCellReference)GetCube(eCubeType.ChainBasisLowLevelTotalWeekDetail).CreateCellReference();
				basisCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
				basisCellRef[eProfileType.Variable] = varProf.Key;
				basisCellRef[eProfileType.LowLevelTotalVersion] = Include.FV_PlanLowLevelTotalRID;
				basisCellRef[eProfileType.Week] = wp.Key;
				basisCellRef[eProfileType.Basis] = basis;
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
		private double[] GetDynBasisForSpread(ProfileList planProfileList, VariableProfile varProf, int basis)
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
				basisCellRef = (PlanCellReference)GetCube(eCubeType.ChainBasisLowLevelTotalWeekDetail).CreateCellReference();
				basisCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
				basisCellRef[eProfileType.Variable] = varProf.Key;
				basisCellRef[eProfileType.LowLevelTotalVersion] = Include.FV_PlanLowLevelTotalRID;
				basisCellRef[eProfileType.Week] = wp.Key;
				basisCellRef[eProfileType.Basis] = basis;
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
		//Begin Enhancement - JScott - Add Balance Low Levels functionality

		/// <summary>
		/// Perform matrix balance.
		/// </summary>

		override public void CopyLowToHigh()
		{
			//Begin Track #5768 - JScott - Copy Low to High problem with PreInit calculation references
			//PlanCube chainCube;
			//PlanCube chainLowCube;
			//ComputationSchedule compSchd;
			//PlanCellReference chainPlanCellRef;
			//PlanCellReference chainLowPlanCellRef;
			//ProfileList weekList;

			//try
			//{
			//    chainCube = (PlanCube)GetCube(eCubeType.ChainPlanWeekDetail);
			//    chainLowCube = (PlanCube)GetCube(eCubeType.ChainPlanLowLevelTotalWeekDetail);

			//    compSchd = new ComputationSchedule(this);
			//    CreateUndoRestorePoint();

			//    foreach (VariableProfile varProf in Transaction.PlanComputations.PlanVariables.VariableProfileList)
			//    {
			//        chainPlanCellRef = (PlanCellReference)chainCube.CreateCellReference();

			//        chainPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
			//        chainPlanCellRef[eProfileType.Variable] = varProf.Key;
			//        chainPlanCellRef[eProfileType.HierarchyNode] = _openParms.ChainHLPlanProfile.NodeProfile.Key;
			//        chainPlanCellRef[eProfileType.Version] = _openParms.ChainHLPlanProfile.VersionProfile.Key;

			//        if (chainPlanCellRef.PlanCube.isDatabaseVariable(varProf, chainPlanCellRef))
			//        {
			//            weekList = _openParms.GetWeekProfileList(SAB.ApplicationServerSession);

			//            foreach (WeekProfile weekProf in weekList.ArrayList)
			//            {
			//                chainPlanCellRef[eProfileType.Week] = weekProf.Key;

			//                //Begin Track #6008 - JScott - B OTB Multi Level Errors Copy Low to High and Balance Low Levels
			//                //if (chainLowPlanCellRef.PlanCell.isCellAvailableForComputation)
			//                if (chainPlanCellRef.isCellAvailableForComputation)
			//                {
			//                //End Track #6008 - JScott - B OTB Multi Level Errors Copy Low to High and Balance Low Levels
			//                    chainLowPlanCellRef = (PlanCellReference)chainLowCube.CreateCellReference();

			//                    chainLowPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
			//                    chainLowPlanCellRef[eProfileType.Variable] = varProf.Key;
			//                    chainLowPlanCellRef[eProfileType.LowLevelTotalVersion] = Include.FV_PlanLowLevelTotalRID;
			//                    chainLowPlanCellRef[eProfileType.Week] = weekProf.Key;

			//                    if (chainPlanCellRef.PlanCell.isCellAvailableForEntry)
			//                    {
			//                        chainPlanCellRef.SetCompCellValue(eSetCellMode.Computation, chainLowPlanCellRef.GetCellValue(eGetCellMode.Current, true));
			//                        compSchd.ScheduleAutoTotals(chainPlanCellRef);
			//                    }
			//                //Begin Track #6008 - JScott - B OTB Multi Level Errors Copy Low to High and Balance Low Levels
			//                }
			//                //End Track #6008 - JScott - B OTB Multi Level Errors Copy Low to High and Balance Low Levels
			//            }
			//        }
			//    }

			//    intExecuteSchedule(compSchd);
			//    ReinitHighLevelChainCubes();

			//    CreateNewUndoList = true;
			//    _userChanged = true;
			//    ForceCurrentInit = true;
			//}
			//catch (Exception exc)
			//{
			//    string message = exc.ToString();
			//    throw;
			//}
			PlanCellReference lowLevelTotalCellRef;
			PlanCellReference highLevelCellRef;
			ProfileList weekProfList;
			int i;

			try
			{
				lowLevelTotalCellRef = (PlanCellReference)GetCube(eCubeType.ChainPlanLowLevelTotalWeekDetail).CreateCellReference();
				highLevelCellRef = (PlanCellReference)GetCube(eCubeType.ChainPlanWeekDetail).CreateCellReference();

				lowLevelTotalCellRef[eProfileType.LowLevelTotalVersion] = Include.FV_PlanLowLevelTotalRID;
				lowLevelTotalCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

				highLevelCellRef[eProfileType.Version] = _openParms.ChainHLPlanProfile.VersionProfile.Key;
				highLevelCellRef[eProfileType.HierarchyNode] = _openParms.ChainHLPlanProfile.NodeProfile.Key;
				highLevelCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

				//Begin Track #6427 - JSCott - Urgent - In Prod BOTB and Vendor OTB, Copy Low to High is not working correctly
				//ClearCube(eCubeType.ChainPlanWeekDetail, highLevelCellRef, 2);
				//ClearCube(eCubeType.ChainPlanPeriodDetail, highLevelCellRef, 2);
				//ClearCube(eCubeType.ChainPlanDateTotal, highLevelCellRef, 2);
				ClearCubeForHierarchyVersion(eCubeType.ChainPlanWeekDetail, highLevelCellRef);
				ClearCubeForHierarchyVersion(eCubeType.ChainPlanPeriodDetail, highLevelCellRef);
				ClearCubeForHierarchyVersion(eCubeType.ChainPlanDateTotal, highLevelCellRef);
				//End Track #6427 - JSCott - Urgent - In Prod BOTB and Vendor OTB, Copy Low to High is not working correctly

				weekProfList = _openParms.GetWeekProfileList(this._SAB.ApplicationServerSession);

				for (i = 0; i < weekProfList.Count; i++)
				{
					lowLevelTotalCellRef[eProfileType.Week] = weekProfList[i].Key;
					highLevelCellRef[eProfileType.Week] = weekProfList[i].Key;

					foreach (VariableProfile varProf in Transaction.PlanComputations.PlanVariables.VariableProfileList)
					{
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
					}
				}

				ClearUndoStack();
				RecomputeCubes(true);
				//Begin TT#1134 - JScott - MID 4.0 does not automatically save locked cells in the OTS forecast

				_userChanged = true;
				//End TT#1134 - JScott - MID 4.0 does not automatically save locked cells in the OTS forecast
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
			//End Track #5768 - JScott - Copy Low to High problem with PreInit calculation references
		}

		//Begin Track #6427 - JSCott - Urgent - In Prod BOTB and Vendor OTB, Copy Low to High is not working correctly
		////Begin Track #5768 - JScott - Copy Low to High problem with PreInit calculation references
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

		////End Track #5768 - JScott - Copy Low to High problem with PreInit calculation references
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
		/// Method that allows PlanCubeGroups to copy High-level values to Low-level Total values.
		/// </summary>
		override public void BalanceLowLevels()
		{
			PlanCube chainCube;
			PlanCube chainLowCube;
			ComputationSchedule compSchd;
			PlanCellReference chainPlanCellRef;
			PlanCellReference chainLowPlanCellRef;
			ProfileList weekList;
			ArrayList planCellRefs = null;
			PlanSpread planSpread = new PlanSpread();
			ArrayList planCellValueList = null;
			ArrayList planCellRefList = null;
			int i;
			string msg;

			try
			{
				chainCube = (PlanCube)GetCube(eCubeType.ChainPlanWeekDetail);
				chainLowCube = (PlanCube)GetCube(eCubeType.ChainPlanLowLevelTotalWeekDetail);

				try
				{
					compSchd = new ComputationSchedule(this);
					CreateUndoRestorePoint();

					foreach (VariableProfile varProf in Transaction.PlanComputations.PlanVariables.VariableProfileList)
					{
						chainPlanCellRef = (PlanCellReference)chainCube.CreateCellReference();

						chainPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
						chainPlanCellRef[eProfileType.Variable] = varProf.Key;
						chainPlanCellRef[eProfileType.HierarchyNode] = _openParms.ChainHLPlanProfile.NodeProfile.Key;
						chainPlanCellRef[eProfileType.Version] = _openParms.ChainHLPlanProfile.VersionProfile.Key;

						if (chainPlanCellRef.PlanCube.isDatabaseVariable(varProf, chainPlanCellRef))
						{
							weekList = _openParms.GetWeekProfileList(SAB.ApplicationServerSession);

							foreach (WeekProfile weekProf in weekList.ArrayList)
							{
								chainPlanCellRef[eProfileType.Week] = weekProf.Key;

								chainLowPlanCellRef = (PlanCellReference)chainLowCube.CreateCellReference();

								chainLowPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
								chainLowPlanCellRef[eProfileType.Variable] = varProf.Key;
								chainLowPlanCellRef[eProfileType.LowLevelTotalVersion] = Include.FV_PlanLowLevelTotalRID;
								chainLowPlanCellRef[eProfileType.Week] = weekProf.Key;

								//Begin Track #6008 - JScott - B OTB Multi Level Errors Copy Low to High and Balance Low Levels
								//if (chainLowPlanCellRef.PlanCell.isCellAvailableForComputation)
								if (chainLowPlanCellRef.isCellAvailableForComputation)
								//End Track #6008 - JScott - B OTB Multi Level Errors Copy Low to High and Balance Low Levels
								{
									planCellRefs = chainLowPlanCellRef.GetSpreadDetailCellRefArray(true);

									try
									{
										if (varProf.VariableSpreadType == eVariableSpreadType.PctContribution)
										{
											planSpread.ExecuteSimpleSpread(
												chainPlanCellRef.CurrentCellValue,
												planCellRefs,
												null,
												Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.NumDecimals,
												out planCellValueList,
												out planCellRefList,
												false);
										}
										else if (varProf.VariableSpreadType == eVariableSpreadType.PctChange)
										{
											planSpread.ExecutePctChangeSpread(
												chainLowPlanCellRef.CurrentCellValue,
												chainPlanCellRef.CurrentCellValue,
												planCellRefs,
												planCellRefs,
												null,
												Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.NumDecimals,
												out planCellValueList,
												out planCellRefList,
												false);
										}

										for (i = 0; i < planCellRefList.Count; i++)
										{
											((PlanCellReference)planCellRefList[i]).SetCompCellValue(eSetCellMode.Computation, (double)planCellValueList[i]);
											compSchd.ScheduleAutoTotals((PlanCellReference)planCellRefList[i]);
										}
									}
									catch (NothingToSpreadException)
									{
										msg = MIDText.GetText(eMIDTextCode.msg_ChainWeekLowLvlSpreadFailed);
										msg = msg.Replace("{0}", _openParms.ChainHLPlanProfile.NodeProfile.Text);
										msg = msg.Replace("{1}", weekProf.YearWeek.ToString(CultureInfo.CurrentCulture));

										_SAB.MessageCallback.HandleMessage(
											msg,
											"Balance Low Levels Error",
											System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
									}
								}
							}
						}
					}

					intExecuteSchedule(compSchd);
					ReinitLowLevelChainCubes();

					CreateNewUndoList = true;
					_userChanged = true;
					ForceCurrentInit = true;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #xxxx -- Error during Copy Low to High
		//INCOMPLETE -- THE FOLLOWING COMMENTS ARE ONE PROPOSED SOLUTION
		/// <summary>
		/// This method reinitializes the High-level Chain Cube.
		/// </summary>

		private void ReinitHighLevelChainCubes()
		{
			ComputationSchedule compSchd;
			PlanCube chainCube;
			PlanCellReference chainPlanCellRef;
			ProfileList weekList;

			try
			{
				compSchd = new ComputationSchedule(this);

				foreach (VariableProfile varProf in Transaction.PlanComputations.PlanVariables.VariableProfileList)
				{
					weekList = _openParms.GetWeekProfileList(SAB.ApplicationServerSession);

					foreach (WeekProfile weekProf in weekList.ArrayList)
					{
						chainCube = (PlanCube)GetCube(eCubeType.ChainPlanWeekDetail);
						chainPlanCellRef = (PlanCellReference)chainCube.CreateCellReference();

						chainPlanCellRef[eProfileType.HierarchyNode] = _openParms.ChainHLPlanProfile.NodeProfile.Key;
						chainPlanCellRef[eProfileType.Version] = _openParms.ChainHLPlanProfile.VersionProfile.Key;
						chainPlanCellRef[eProfileType.Week] = weekProf.Key;
						chainPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
						chainPlanCellRef[eProfileType.Variable] = varProf.Key;

						if (chainPlanCellRef.PlanCube.GetInitFormulaProfile(chainPlanCellRef) != null &&
							(chainPlanCellRef.PlanCube.isDatabaseVariable(varProf, chainPlanCellRef) || chainPlanCellRef.isCellInitialized) &&
							chainPlanCellRef.PlanCell.isCellAvailableForComputation)
						{
							compSchd.InsertInitFormula(chainPlanCellRef, (ComputationCellReference)chainPlanCellRef.Copy());
						}
					}
				}

				intExecuteSchedule(compSchd);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//private void intClearCube(eCubeType aCubeType, CellReference aCellRef, int aNumDimensions)
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
		//End Track #xxxx -- Error during Copy Low to High

		/// <summary>
		/// This method reinitializes the Low-level Chain Cubes.
		/// </summary>

		private void ReinitLowLevelChainCubes()
		{
			ComputationSchedule compSchd;
			PlanCube chainCube;
			PlanCellReference chainPlanCellRef;
			ProfileList weekList;

			try
			{
				compSchd = new ComputationSchedule(this);

				foreach (PlanProfile planProf in _openParms.LowLevelPlanProfileList)
				{
					foreach (VariableProfile varProf in Transaction.PlanComputations.PlanVariables.VariableProfileList)
					{
						weekList = _openParms.GetWeekProfileList(SAB.ApplicationServerSession);

						foreach (WeekProfile weekProf in weekList.ArrayList)
						{
							chainCube = (PlanCube)GetCube(eCubeType.ChainPlanWeekDetail);
							chainPlanCellRef = (PlanCellReference)chainCube.CreateCellReference();

							chainPlanCellRef[eProfileType.HierarchyNode] = planProf.NodeProfile.Key;
							chainPlanCellRef[eProfileType.Version] = planProf.VersionProfile.Key;
							chainPlanCellRef[eProfileType.Week] = weekProf.Key;
							chainPlanCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
							chainPlanCellRef[eProfileType.Variable] = varProf.Key;

							if (chainPlanCellRef.PlanCube.GetInitFormulaProfile(chainPlanCellRef) != null &&
								(chainPlanCellRef.PlanCube.isDatabaseVariable(varProf, chainPlanCellRef) || chainPlanCellRef.isCellInitialized) &&
								chainPlanCellRef.PlanCell.isCellAvailableForComputation)
							{
								compSchd.InsertInitFormula(chainPlanCellRef, (ComputationCellReference)chainPlanCellRef.Copy());
							}
						}
					}
				}

				intExecuteSchedule(compSchd);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void intExecuteSchedule(ComputationSchedule aCompSchd)
		{
			string errorMessage;

			try
			{
				try
				{
					aCompSchd.Execute();
				}
				catch (FormulaConflictException)
				{
					UndoLastRecompute();

					SAB.MessageCallback.HandleMessage(
						MIDText.GetText(eMIDTextCode.msg_pl_FormulaConflict),
						MIDText.GetText(eMIDTextCode.msg_pl_GenericMessageBoxTitle),
						System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
				}
				catch (CircularReferenceException)
				{
					UndoLastRecompute();

					SAB.MessageCallback.HandleMessage(
						MIDText.GetText(eMIDTextCode.msg_pl_CircularReference),
						MIDText.GetText(eMIDTextCode.msg_pl_GenericMessageBoxTitle),
						System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
				}
				catch (CellCompChangedException)
				{
					UndoLastRecompute();

					SAB.MessageCallback.HandleMessage(
						MIDText.GetText(eMIDTextCode.msg_pl_CompChanged),
						MIDText.GetText(eMIDTextCode.msg_pl_GenericMessageBoxTitle),
						System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
				}
				catch (NoCellsToSpreadTo)
				{
					UndoLastRecompute();

					SAB.MessageCallback.HandleMessage(
						MIDText.GetText(eMIDTextCode.msg_pl_NoCellToSpreadTo),
						MIDText.GetText(eMIDTextCode.msg_pl_GenericMessageBoxTitle),
						System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
				}
				catch (CellNotAvailableException exc)
				{
					UndoLastRecompute();

					errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_pl_CellNotAvailable), exc.Message);

					SAB.MessageCallback.HandleMessage(
						errorMessage,
						MIDText.GetText(eMIDTextCode.msg_pl_GenericMessageBoxTitle),
						System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
				}
				catch (CustomUserErrorException exc)
				{
					UndoLastRecompute();

					errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_CustomUserException), exc.Message);

					SAB.MessageCallback.HandleMessage(
						errorMessage,
						MIDText.GetText(eMIDTextCode.msg_pl_GenericMessageBoxTitle),
						System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
				}
				finally
				{
					ClearComputations();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End Enhancement - JScott - Add Balance Low Levels functionality

        // Begin TT#5276 - JSmith - Read Only Saves
        override public bool IsNodeEnqueued(int aNodeKey, bool aIsChain)
        {
            // Begin TT#5385 - JSmith - Receiving an error when opening Multi-Level OTS Forecasts - Chain
            if (!_chainPlanEnqueue.ContainsKey(aNodeKey))
            {
                return false;
            }
            // End TT#5385 - JSmith - Receiving an error when opening Multi-Level OTS Forecasts - Chain
            return ((PlanEnqueueInfo)_chainPlanEnqueue[aNodeKey]).PlanEnqueue.isEnqueued;
        }
        // End TT#5276 - JSmith - Read Only Saves
	}
}
