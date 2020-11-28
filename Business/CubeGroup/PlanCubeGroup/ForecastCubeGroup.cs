using System;
using System.Globalization;
using System.Collections;
using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;


namespace MIDRetail.Business
{
	/// <summary>
	/// This class defines the ForecastCubeGroup.
	/// </summary>
	/// <remarks>
	/// The ForecastCubeGroup predefines all Cubes that are required for Store Plan Maintenance.  Those Cubes include StoreDetailWeekCube, StoreTotalWeekCube,
	/// StoreGroupTotalCube, StoreDetailWeekTimeTotalCube, and StoreTotalWeekTimeTotalCube.
	/// </remarks>

	public class ForecastCubeGroup : StorePlanCubeGroup
	{

		//=======
		// FIELDS
		//=======

		PlanEnqueueGroup _planEnqGrp;
		PlanEnqueueInfo _storePlanEnqueue;

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

		public ForecastCubeGroup(
			SessionAddressBlock aSAB,
			Transaction aTransaction)
			: base(aSAB, aTransaction)
		{
			_planEnqGrp = new PlanEnqueueGroup();
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		// Begin Track #5940 stodd
		override public void OpenCubeGroup(PlanOpenParms aOpenParms)
		{
			OpenCubeGroup(aOpenParms, false);
		}

		public void OpenCubeGroup(PlanOpenParms aOpenParms, bool readOnly)
		{
		// End Track #5940
			ProfileList HierarchyNodeProfileList;
			CubeDefinition cubeDef;
			PlanCube planCube;
			EligibilityFilter eligFilter;

			base.OpenCubeGroup(aOpenParms);

			//========================================
			// Create HierarchyNode Master ProfileList
			//========================================
			
			HierarchyNodeProfileList = new ProfileList(eProfileType.HierarchyNode);

			if (!HierarchyNodeProfileList.Contains(_openParms.ChainHLPlanProfile.NodeProfile.Key))
			{
				HierarchyNodeProfileList.Add(_openParms.ChainHLPlanProfile.NodeProfile);
			}
			if (!HierarchyNodeProfileList.Contains(_openParms.StoreHLPlanProfile.NodeProfile.Key))
			{
				HierarchyNodeProfileList.Add(_openParms.StoreHLPlanProfile.NodeProfile);
			}

			//====================================
			// Set the Current Store Group Profile
			//====================================

			CurrentStoreGroupProfile = _openParms.GetStoreGroupProfile(_SAB);

			//========================================
			// Set Master Profile Lists and XRef Lists
			//========================================

			SetMasterProfileList(_openParms.GetWeekProfileList(SAB.ApplicationServerSession));
			SetMasterProfileList(_openParms.GetDateProfileList(SAB.ApplicationServerSession));
            // BEGIN MID Track #6147 - Forecast Method null refreence exception; cube needs ChainPlanPeriod Detail Cube 
            SetMasterProfileList(_openParms.GetPeriodProfileList(SAB.ApplicationServerSession));
            // END MID Track #6147
            SetMasterProfileList(_openParms.BasisProfileList);
			SetMasterProfileList(HierarchyNodeProfileList);

			SetProfileXRef(_openParms.GetDateToWeekXRef(SAB.ApplicationServerSession));

			//=======================
			// Enqueue the store plan
			//=======================

			ProfileList planWeekList = _openParms.GetWeekProfileList(SAB.ApplicationServerSession);
			// Begin Track #5940 stodd
			_storePlanEnqueue = new PlanEnqueueInfo(_SAB, _openParms.StoreHLPlanProfile, ePlanType.Store, planWeekList[0].Key, planWeekList[planWeekList.Count - 1].Key);
			if (!readOnly)
			{
				_planEnqGrp.EnqueuePlan(_SAB, _storePlanEnqueue, false, _openParms.FormatErrorsForMessageBox, _openParms.UpdateAuditHeaderOnError);
			}
			// End Track #5940

			//=========================================
			// Create and apply ineligibility filters
			//=========================================

			if (!_openParms.IneligibleStores)
			{
				eligFilter = new EligibilityFilter(this);
				ApplyFilter(eligFilter, eFilterType.Permanent);
			}

			//=========================================
			// Create ChainPlanWeekDetail in CubeGroup
			//=========================================

			cubeDef = new CubeDefinition(
				new DimensionDefinition(eProfileType.Version, GetMasterProfileList(eProfileType.Version).Count),
				new DimensionDefinition(eProfileType.HierarchyNode, GetMasterProfileList(eProfileType.HierarchyNode).Count),
				new DimensionDefinition(eProfileType.Week, GetMasterProfileList(eProfileType.Week).Count),
				new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.HighLevel))),
				new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

			planCube = (PlanCube)GetCube(eCubeType.ChainPlanWeekDetail);

			if (planCube == null)
			{
				planCube = new ChainPlanWeekDetail(SAB, Transaction, this, cubeDef, 1, true, false);
				Transaction.PlanComputations.PlanCubeInitialization.ChainPlanWeekDetail(planCube, ePlanDisplayType.Week);
				SetCube(eCubeType.ChainPlanWeekDetail, planCube);
			}
			else
			{
//				planCube.Expand(cubeDef);
			}

            // BEGIN MID Track #6147 - Forecast Method null refreence exception; cube needs ChainPlanPeriod Detail Cube 
            //============================================
            // Create ChainPlanPeriodDetail in CubeGroup
            //============================================

            cubeDef = new CubeDefinition(
                new DimensionDefinition(eProfileType.Version, GetMasterProfileList(eProfileType.Version).Count),
                new DimensionDefinition(eProfileType.HierarchyNode, GetMasterProfileList(eProfileType.HierarchyNode).Count),
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
                //planCube.ExpandDimensionSize(cubeDef);
            }

            //============================================
            // Create ChainPlanDateTotal in CubeGroup
            //============================================

            cubeDef = new CubeDefinition(
                new DimensionDefinition(eProfileType.Version, GetMasterProfileList(eProfileType.Version).Count),
                new DimensionDefinition(eProfileType.HierarchyNode, GetMasterProfileList(eProfileType.HierarchyNode).Count),
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
                //planCube.ExpandDimensionSize(cubeDef);
            }
            // END MID Track #6147

			//=========================================
			// Create StorePlanWeekDetail in CubeGroup
			//=========================================

			cubeDef = new CubeDefinition(
				new DimensionDefinition(eProfileType.Version, GetMasterProfileList(eProfileType.Version).Count),
				new DimensionDefinition(eProfileType.HierarchyNode, GetMasterProfileList(eProfileType.HierarchyNode).Count),
				new DimensionDefinition(eProfileType.Week, GetMasterProfileList(eProfileType.Week).Count),
				new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.HighLevel))),
				new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
				new DimensionDefinition(eProfileType.Store, GetFilteredProfileList(eProfileType.Store).Count));

			planCube = (PlanCube)GetCube(eCubeType.StorePlanWeekDetail);

			if (planCube == null)
			{
				planCube = new StorePlanWeekDetail(SAB, Transaction, this, cubeDef, 1, !_storePlanEnqueue.PlanEnqueue.isEnqueued, false);
				Transaction.PlanComputations.PlanCubeInitialization.StorePlanWeekDetail(planCube, ePlanDisplayType.Week);
				SetCube(eCubeType.StorePlanWeekDetail, planCube);
			}
			else
			{
//				planCube.Expand(cubeDef);
			}

			// Begin Track #6187 stodd
			//============================================
			// Create StorePlanPeriodDetail in CubeGroup
			//============================================

			cubeDef = new CubeDefinition(
				new DimensionDefinition(eProfileType.Version, GetMasterProfileList(eProfileType.Version).Count),
				new DimensionDefinition(eProfileType.HierarchyNode, GetMasterProfileList(eProfileType.HierarchyNode).Count),
				new DimensionDefinition(eProfileType.Period, GetMasterProfileList(eProfileType.Period).Count),
				new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.HighLevel))),
				new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
				new DimensionDefinition(eProfileType.Store, GetFilteredProfileList(eProfileType.Store).Count));

			planCube = (PlanCube)GetCube(eCubeType.StorePlanPeriodDetail);

			if (planCube == null)
			{
				planCube = new StorePlanPeriodDetail(SAB, Transaction, this, cubeDef, 2, !_storePlanEnqueue.PlanEnqueue.isEnqueued, false);
				Transaction.PlanComputations.PlanCubeInitialization.StorePlanPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
				SetCube(eCubeType.StorePlanPeriodDetail, planCube);
			}
			else
			{
				//planCube.ExpandDimensionSize(cubeDef);
			}
			// End track #6187

			//=========================================
			// Create StoreBasisWeekDetail in CubeGroup
			//=========================================

			cubeDef = new CubeDefinition(
				new DimensionDefinition(eProfileType.Version, GetMasterProfileList(eProfileType.Version).Count),
				new DimensionDefinition(eProfileType.HierarchyNode, GetMasterProfileList(eProfileType.HierarchyNode).Count),
				new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
				new DimensionDefinition(eProfileType.Week, GetMasterProfileList(eProfileType.Week).Count),
				new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.HighLevel))),
				new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
				new DimensionDefinition(eProfileType.Store, GetFilteredProfileList(eProfileType.Store).Count));

			planCube = (PlanCube)GetCube(eCubeType.StoreBasisWeekDetail);

			if (planCube == null)
			{
				planCube = new StoreBasisWeekDetail(SAB, Transaction, this, cubeDef, 1);
				Transaction.PlanComputations.PlanCubeInitialization.StoreBasisWeekDetail(planCube, ePlanDisplayType.Week);
				SetCube(eCubeType.StoreBasisWeekDetail, planCube);
			}
			else
			{
//				planCube.Expand(cubeDef);
			}


			//==================================================
			// Create StorePlanStoreTotalWeekDetail in CubeGroup
			//==================================================

			cubeDef = new CubeDefinition(
				new DimensionDefinition(eProfileType.Version, GetMasterProfileList(eProfileType.Version).Count),
				new DimensionDefinition(eProfileType.HierarchyNode, GetMasterProfileList(eProfileType.HierarchyNode).Count),
				new DimensionDefinition(eProfileType.Week, GetMasterProfileList(eProfileType.Week).Count),
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
//				planCube.ExpandDimensionSize(cubeDef);
			}

			//==================================================
			// Create StorePlanGroupTotalWeekDetail in CubeGroup
			//==================================================

			cubeDef = new CubeDefinition(
				new DimensionDefinition(eProfileType.Version, GetMasterProfileList(eProfileType.Version).Count),
				new DimensionDefinition(eProfileType.HierarchyNode, GetMasterProfileList(eProfileType.HierarchyNode).Count),
				new DimensionDefinition(eProfileType.Week, GetMasterProfileList(eProfileType.Week).Count),
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
//				planCube.ExpandDimensionSize(cubeDef);
			}

			//======================================================
			// Create StorePlanStoreTotalDateTotal in CubeGroup
			//======================================================

			cubeDef = new CubeDefinition(
				new DimensionDefinition(eProfileType.Version, GetMasterProfileList(eProfileType.Version).Count),
				new DimensionDefinition(eProfileType.HierarchyNode, GetMasterProfileList(eProfileType.HierarchyNode).Count),
				new DimensionDefinition(eProfileType.TimeTotalVariable, GetMasterProfileList(eProfileType.TimeTotalVariable).Count),
				new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.LowLevel))),
				new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

			planCube = (PlanCube)GetCube(eCubeType.StorePlanStoreTotalDateTotal);

			if (planCube == null)
			{
				planCube = new StorePlanStoreTotalDateTotal(SAB, Transaction, this, cubeDef, 15, !_storePlanEnqueue.PlanEnqueue.isEnqueued, false);
				Transaction.PlanComputations.PlanCubeInitialization.StorePlanStoreTotalDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
				SetCube(eCubeType.StorePlanStoreTotalDateTotal, planCube);
			}
			else
			{
//				planCube.ExpandDimensionSize(cubeDef);
			}

			//============================================
			// Create StorePlanDateTotal in CubeGroup
			//============================================

			cubeDef = new CubeDefinition(
				new DimensionDefinition(eProfileType.Version, GetMasterProfileList(eProfileType.Version).Count),
				new DimensionDefinition(eProfileType.HierarchyNode, GetMasterProfileList(eProfileType.HierarchyNode).Count),
				new DimensionDefinition(eProfileType.TimeTotalVariable, GetMasterProfileList(eProfileType.TimeTotalVariable).Count),
				new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.LowLevel))),
				new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
				new DimensionDefinition(eProfileType.Store, GetFilteredProfileList(eProfileType.Store).Count));

			planCube = (PlanCube)GetCube(eCubeType.StorePlanDateTotal);

			if (planCube == null)
			{
				planCube = new StorePlanDateTotal(SAB, Transaction, this, cubeDef, 3, !_storePlanEnqueue.PlanEnqueue.isEnqueued, false);
				Transaction.PlanComputations.PlanCubeInitialization.StorePlanDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
				SetCube(eCubeType.StorePlanDateTotal, planCube);
			}
			else
			{
//				planCube.ExpandDimensionSize(cubeDef);
			}

			//===================================================
			// Create StoreBasisStoreTotalWeekDetail in CubeGroup
			//===================================================

			cubeDef = new CubeDefinition(
				new DimensionDefinition(eProfileType.Version, GetMasterProfileList(eProfileType.Version).Count),
				new DimensionDefinition(eProfileType.HierarchyNode, GetMasterProfileList(eProfileType.HierarchyNode).Count),
				new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
				new DimensionDefinition(eProfileType.Week, GetMasterProfileList(eProfileType.Week).Count),
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
//				planCube.ExpandDimensionSize(cubeDef);
			}

			//=======================================================
			// Create StoreBasisStoreTotalDateTotal in CubeGroup
			//=======================================================

			cubeDef = new CubeDefinition(
				new DimensionDefinition(eProfileType.Version, GetMasterProfileList(eProfileType.Version).Count),
				new DimensionDefinition(eProfileType.HierarchyNode, GetMasterProfileList(eProfileType.HierarchyNode).Count),
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
//				planCube.ExpandDimensionSize(cubeDef);
			}

			//===================================================
			// Create StoreBasisGroupTotalWeekDetail in CubeGroup
			//===================================================

			cubeDef = new CubeDefinition(
				new DimensionDefinition(eProfileType.Version, GetMasterProfileList(eProfileType.Version).Count),
				new DimensionDefinition(eProfileType.HierarchyNode, GetMasterProfileList(eProfileType.HierarchyNode).Count),
				new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
				new DimensionDefinition(eProfileType.Week, GetMasterProfileList(eProfileType.Week).Count),
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
//				planCube.ExpandDimensionSize(cubeDef);
			}

			//=======================================================
			// Create StoreBasisGroupTotalDateTotal in CubeGroup
			//=======================================================

			cubeDef = new CubeDefinition(
				new DimensionDefinition(eProfileType.Version, GetMasterProfileList(eProfileType.Version).Count),
				new DimensionDefinition(eProfileType.HierarchyNode, GetMasterProfileList(eProfileType.HierarchyNode).Count),
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
//				planCube.ExpandDimensionSize(cubeDef);
			}

			// Load Cube with initial values

			((ChainPlanWeekDetail)GetCube(eCubeType.ChainPlanWeekDetail)).ReadAndLoadCube(_openParms.ChainHLPlanProfile, _openParms.GetWeekProfileList(SAB.ApplicationServerSession));
			((StorePlanWeekDetail)GetCube(eCubeType.StorePlanWeekDetail)).ReadAndLoadCube(_openParms.StoreHLPlanProfile, _openParms.GetWeekProfileList(SAB.ApplicationServerSession));
			((StoreBasisWeekDetail)GetCube(eCubeType.StoreBasisWeekDetail)).ReadAndLoadCube(_openParms.StoreHLPlanProfile, _openParms.SimilarStores);
		}

		/// <summary>
		/// Saves this PlanCubeGroup
		/// </summary>
		/// <param name="aSaveParms">
		/// The PlanSaveParms that contains information about the save.
		/// </param>

		override public void SaveCubeGroup(PlanSaveParms aSaveParms)
		{
			try
			{
				((StorePlanWeekDetail)GetCube(eCubeType.StorePlanWeekDetail)).SaveCube(
					_openParms.StoreHLPlanProfile.NodeProfile.Key,
					_openParms.StoreHLPlanProfile.VersionProfile.Key,
					_openParms.GetWeekProfileList(SAB.ApplicationServerSession),
					true,
					//Begin Track #5690 - JScott - Can not save low to high
					//true,
					//End Track #5690 - JScott - Can not save low to high
					aSaveParms.SaveLocks);
			}
			catch (Exception)
			{
				throw;
			}
		}

        // Begin TT#2131-MD - JSmith - Halo Integration
        override public void ExtractCubeGroup(ExtractOptions aExtractOptions)
        {
        }
        // End TT#2131-MD - JSmith - Halo Integration

        // Begin Track #5940 stodd
        override public void CloseCubeGroup()
		{
			CloseCubeGroup(false);
		}

		public void CloseCubeGroup(bool readOnly)
		{
			try
			{
				base.CloseCubeGroup();

				if (!readOnly)
				{
					_planEnqGrp.DequeuePlans();
					//Begin Enhancement - JScott - Add Balance Low Levels functionality
					//_cubeTable.Clear();
					//End Enhancement - JScott - Add Balance Low Levels functionality
				}

		// End Track #5940 stodd
			}
			catch (Exception)
			{
				throw;
			}
		}

		public ArrayList GetCubeData(PlanCellReference planCellRef)
		{
			ArrayList ar = new ArrayList();
			return ar;
		
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
