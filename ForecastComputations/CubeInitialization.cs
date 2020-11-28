using System;
using System.Collections;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;

namespace MIDRetail.ForecastComputations
{
	/// <summary>
	/// The CubeInitialization class is where cube initialization routines are defined.
	/// </summary>
	/// <remarks>
	/// This class is where cube initialization routines are defined.  A cube initialization routine is used to identify which init and change rules
	/// are valid for each cube and if the cube has a total cube.
	/// </remarks>

	abstract public class BasePlanCubeInitialization : IPlanComputationCubeInitialization
	{
		//=======
		// FIELDS
		//=======

		private BasePlanComputations _basePlanComputations;

		//=============
		// CONSTRUCTORS
		//=============

		public BasePlanCubeInitialization(BasePlanComputations aBasePlanComputations)
		{
			_basePlanComputations = aBasePlanComputations;
		}

		//===========
		// PROPERTIES
		//===========

		protected BasePlanVariableInitialization BasePlanVariableInitialization
		{
			get
			{
				return _basePlanComputations.BasePlanVariableInitialization;
			}
		}

		//========
		// METHODS
		//========

		// Chain Cubes

		virtual public void ChainBasisDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.ChainBasisDetail(aPlanCube);
		}
		
		virtual public void ChainBasisWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.ChainBasisWeekDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainBasisDetail,
				new BasisDetailPlanCubeRelationshipItem()));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainBasisDateTotal,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainBasisPeriodDetail,
				new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainBasisLowLevelTotalWeekDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.ChainPlan)));

			aPlanCube.SetComponentDetailCube(eCubeType.ChainBasisDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.ChainBasisDetail);
			aPlanCube.AddTotalCube(eCubeType.ChainBasisPeriodDetail);
			aPlanCube.AddTotalCube(eCubeType.ChainBasisLowLevelTotalWeekDetail);

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddTotalCube(eCubeType.ChainBasisDateTotal);
			}
		}
		
		virtual public void ChainBasisDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.ChainBasisDateTotal(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainBasisWeekDetail,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainBasisLowLevelTotalDateTotal,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.ChainPlan)));

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.SetComponentDetailCube(eCubeType.ChainBasisWeekDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.ChainBasisWeekDetail);
			}
			else
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainBasisPeriodDetail,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));

				aPlanCube.SetComponentDetailCube(eCubeType.ChainBasisPeriodDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.ChainBasisPeriodDetail);
			}
		}

		virtual public void ChainBasisPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.ChainBasisPeriodDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainBasisWeekDetail,
				new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainBasisLowLevelTotalPeriodDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.ChainPlan)));

			aPlanCube.AddTotalCube(eCubeType.ChainBasisLowLevelTotalPeriodDetail);
			aPlanCube.SetComponentDetailCube(eCubeType.ChainBasisWeekDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.ChainBasisWeekDetail);

			if (aPlanDisplayType == ePlanDisplayType.Period)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainBasisDateTotal,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));

				aPlanCube.AddTotalCube(eCubeType.ChainBasisDateTotal);
			}
		}
		
		virtual public void ChainBasisLowLevelTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.ChainBasisLowLevelTotalWeekDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainBasisLowLevelTotalDateTotal,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainBasisWeekDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.ChainPlan)));

			aPlanCube.SetComponentDetailCube(eCubeType.ChainBasisWeekDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.ChainBasisWeekDetail);

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddTotalCube(eCubeType.ChainBasisLowLevelTotalDateTotal);
			}
		}
		
		virtual public void ChainBasisLowLevelTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.ChainBasisLowLevelTotalDateTotal(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainBasisLowLevelTotalWeekDetail,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainBasisDateTotal,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.ChainPlan)));

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.SetComponentDetailCube(eCubeType.ChainBasisLowLevelTotalWeekDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.ChainBasisLowLevelTotalWeekDetail);
			}
			else
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainBasisLowLevelTotalPeriodDetail,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));

				aPlanCube.SetComponentDetailCube(eCubeType.ChainBasisLowLevelTotalPeriodDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.ChainBasisLowLevelTotalPeriodDetail);
			}
		}
		
		virtual public void ChainBasisLowLevelTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.ChainBasisLowLevelTotalPeriodDetail(aPlanCube);

            aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainBasisLowLevelTotalWeekDetail,
                new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
            aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainBasisPeriodDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.ChainPlan)));

			aPlanCube.SetComponentDetailCube(eCubeType.ChainBasisPeriodDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.ChainBasisPeriodDetail);
		
			if (aPlanDisplayType == ePlanDisplayType.Period)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainBasisLowLevelTotalDateTotal,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));

				aPlanCube.AddTotalCube(eCubeType.ChainBasisLowLevelTotalDateTotal);
			}
		}

		virtual public void ChainPlanWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.ChainPlanWeekDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainPlanDateTotal,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainPlanPeriodDetail,
				new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainPlanLowLevelTotalWeekDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.ChainPlan)));

			aPlanCube.AddTotalCube(eCubeType.ChainPlanPeriodDetail);
			aPlanCube.AddTotalCube(eCubeType.ChainPlanLowLevelTotalWeekDetail);

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddTotalCube(eCubeType.ChainPlanDateTotal);
			}
		}
		
		virtual public void ChainPlanDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.ChainPlanDateTotal(aPlanCube);
		
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainPlanWeekDetail,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainPlanLowLevelTotalDateTotal,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.ChainPlan)));

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.SetComponentDetailCube(eCubeType.ChainPlanWeekDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.ChainPlanWeekDetail);
			}
			else
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainPlanPeriodDetail,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));

				aPlanCube.SetComponentDetailCube(eCubeType.ChainPlanPeriodDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.ChainPlanPeriodDetail);
			}
		}

		virtual public void ChainPlanPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.ChainPlanPeriodDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainPlanWeekDetail,
				new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainPlanLowLevelTotalPeriodDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.ChainPlan)));

			aPlanCube.AddTotalCube(eCubeType.ChainPlanLowLevelTotalPeriodDetail);
			aPlanCube.SetComponentDetailCube(eCubeType.ChainPlanWeekDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.ChainPlanWeekDetail);

			if (aPlanDisplayType == ePlanDisplayType.Period)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainPlanDateTotal,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));

				aPlanCube.AddTotalCube(eCubeType.ChainPlanDateTotal);
			}
		}
		
		virtual public void ChainPlanLowLevelTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.ChainPlanLowLevelTotalWeekDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainPlanLowLevelTotalDateTotal,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainPlanWeekDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.ChainPlan)));

			aPlanCube.SetComponentDetailCube(eCubeType.ChainPlanWeekDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.ChainPlanWeekDetail);

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddTotalCube(eCubeType.ChainPlanLowLevelTotalDateTotal);
			}
		}
		
		virtual public void ChainPlanLowLevelTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.ChainPlanLowLevelTotalDateTotal(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainPlanLowLevelTotalWeekDetail,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainPlanDateTotal,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.ChainPlan)));

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.SetComponentDetailCube(eCubeType.ChainPlanLowLevelTotalWeekDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.ChainPlanLowLevelTotalWeekDetail);
			}
			else
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainPlanLowLevelTotalPeriodDetail,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));

				aPlanCube.SetComponentDetailCube(eCubeType.ChainPlanLowLevelTotalPeriodDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.ChainPlanLowLevelTotalPeriodDetail);
			}
		}
		
		virtual public void ChainPlanLowLevelTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.ChainPlanLowLevelTotalPeriodDetail(aPlanCube);

            aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainPlanLowLevelTotalWeekDetail,
                new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
            aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainPlanPeriodDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.ChainPlan)));

			aPlanCube.SetComponentDetailCube(eCubeType.ChainPlanPeriodDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.ChainPlanPeriodDetail);

			if (aPlanDisplayType == ePlanDisplayType.Period)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.ChainPlanLowLevelTotalDateTotal,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));

				aPlanCube.AddTotalCube(eCubeType.ChainPlanLowLevelTotalDateTotal);
			}
		}
		
		// Store Basis Cubes

		virtual public void StoreBasisDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StoreBasisDetail(aPlanCube);
		}
		
		virtual public void StoreBasisWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StoreBasisWeekDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisDetail,
				new BasisDetailPlanCubeRelationshipItem()));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisStoreTotalWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisGroupTotalWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisDateTotal,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisPeriodDetail,
				new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalWeekDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisDetail);
			aPlanCube.AddTotalCube(eCubeType.StoreBasisStoreTotalWeekDetail);
			aPlanCube.AddTotalCube(eCubeType.StoreBasisGroupTotalWeekDetail);
			aPlanCube.AddTotalCube(eCubeType.StoreBasisPeriodDetail);
			aPlanCube.AddTotalCube(eCubeType.StoreBasisLowLevelTotalWeekDetail);

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddTotalCube(eCubeType.StoreBasisDateTotal);
			}
		}
		
		virtual public void StoreBasisDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StoreBasisDateTotal(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisWeekDetail,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalDateTotal,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisStoreTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisGroupTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));

				aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisWeekDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisWeekDetail);
			}
			else
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisPeriodDetail,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisStoreTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisGroupTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));

				aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisPeriodDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisPeriodDetail);
			}
		}

		virtual public void StoreBasisPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StoreBasisPeriodDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisWeekDetail,
				new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisStoreTotalPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisGroupTotalPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalPeriodDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.AddTotalCube(eCubeType.StoreBasisLowLevelTotalPeriodDetail);
			aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisWeekDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisWeekDetail);

			if (aPlanDisplayType == ePlanDisplayType.Period)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisDateTotal,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));

				aPlanCube.AddTotalCube(eCubeType.StoreBasisDateTotal);
			}
		}

		virtual public void StoreBasisGroupTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StoreBasisGroupTotalWeekDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisStoreTotalWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisGroupTotalDateTotal,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisGroupTotalPeriodDetail,
				new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalGroupTotalWeekDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.AddTotalCube(eCubeType.StoreBasisGroupTotalPeriodDetail);
			aPlanCube.AddTotalCube(eCubeType.StoreBasisLowLevelTotalGroupTotalWeekDetail);
			aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisWeekDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisWeekDetail);
			
			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddTotalCube(eCubeType.StoreBasisGroupTotalDateTotal);
			}
		}
		
		virtual public void StoreBasisGroupTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StoreBasisGroupTotalDateTotal(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisGroupTotalWeekDetail,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalGroupTotalDateTotal,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisDateTotal,
					new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisStoreTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));

				aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisGroupTotalWeekDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisGroupTotalWeekDetail);
			}
			else
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisGroupTotalPeriodDetail,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisDateTotal,
					new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisStoreTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));

				aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisGroupTotalPeriodDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisGroupTotalPeriodDetail);
			}
		}

		virtual public void StoreBasisGroupTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StoreBasisGroupTotalPeriodDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisGroupTotalWeekDetail,
				new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisStoreTotalPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalGroupTotalPeriodDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.AddTotalCube(eCubeType.StoreBasisLowLevelTotalGroupTotalPeriodDetail);
			aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisGroupTotalWeekDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisGroupTotalWeekDetail);

			if (aPlanDisplayType == ePlanDisplayType.Period)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisGroupTotalDateTotal,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));

				aPlanCube.AddTotalCube(eCubeType.StoreBasisGroupTotalDateTotal);
			}
		}
		
		virtual public void StoreBasisStoreTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StoreBasisStoreTotalWeekDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisGroupTotalWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisStoreTotalDateTotal,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisStoreTotalPeriodDetail,
				new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalStoreTotalWeekDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.AddTotalCube(eCubeType.StoreBasisStoreTotalPeriodDetail);
			aPlanCube.AddTotalCube(eCubeType.StoreBasisLowLevelTotalStoreTotalWeekDetail);
			aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisWeekDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisWeekDetail);

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddTotalCube(eCubeType.StoreBasisStoreTotalDateTotal);
			}
		}
		
		virtual public void StoreBasisStoreTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StoreBasisStoreTotalDateTotal(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisStoreTotalWeekDetail,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalStoreTotalDateTotal,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisGroupTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));

				aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisStoreTotalWeekDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisStoreTotalWeekDetail);
			}
			else
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisStoreTotalPeriodDetail,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisGroupTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));

				aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisStoreTotalPeriodDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisStoreTotalPeriodDetail);
			}
		}
		
		virtual public void StoreBasisStoreTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StoreBasisStoreTotalPeriodDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisStoreTotalWeekDetail,
				new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisStoreTotalPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalStoreTotalPeriodDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.AddTotalCube(eCubeType.StoreBasisLowLevelTotalStoreTotalPeriodDetail);
			aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisStoreTotalWeekDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisStoreTotalWeekDetail);

			if (aPlanDisplayType == ePlanDisplayType.Period)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisStoreTotalDateTotal,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));

				aPlanCube.AddTotalCube(eCubeType.StoreBasisStoreTotalDateTotal);
			}
		}

		virtual public void StoreBasisLowLevelTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StoreBasisLowLevelTotalWeekDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalStoreTotalWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalGroupTotalWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalDateTotal,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisWeekDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisWeekDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisWeekDetail);

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddTotalCube(eCubeType.StoreBasisLowLevelTotalDateTotal);
			}
		}
		
		virtual public void StoreBasisLowLevelTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StoreBasisLowLevelTotalDateTotal(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalWeekDetail,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisDateTotal,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalStoreTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalGroupTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));

				aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisLowLevelTotalWeekDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisLowLevelTotalWeekDetail);
			}
			else
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalPeriodDetail,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalStoreTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalGroupTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));

				aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisLowLevelTotalPeriodDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisLowLevelTotalPeriodDetail);
			}
		}

		virtual public void StoreBasisLowLevelTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StoreBasisLowLevelTotalPeriodDetail(aPlanCube);

            aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalWeekDetail,
                new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
            aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalStoreTotalPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalGroupTotalPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisPeriodDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisPeriodDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisPeriodDetail);

			if (aPlanDisplayType == ePlanDisplayType.Period)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalDateTotal,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));

				aPlanCube.AddTotalCube(eCubeType.StoreBasisLowLevelTotalDateTotal);
			}
		}

		virtual public void StoreBasisLowLevelTotalGroupTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StoreBasisLowLevelTotalGroupTotalWeekDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalStoreTotalWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalGroupTotalDateTotal,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisGroupTotalWeekDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisGroupTotalWeekDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisGroupTotalWeekDetail);

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddTotalCube(eCubeType.StoreBasisLowLevelTotalGroupTotalDateTotal);
			}
		}
		
		virtual public void StoreBasisLowLevelTotalGroupTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StoreBasisLowLevelTotalGroupTotalDateTotal(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalGroupTotalWeekDetail,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisGroupTotalDateTotal,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalStoreTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));

				aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisLowLevelTotalGroupTotalWeekDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisLowLevelTotalGroupTotalWeekDetail);
			}
			else
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalGroupTotalPeriodDetail,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalStoreTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));

				aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisLowLevelTotalGroupTotalPeriodDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisLowLevelTotalGroupTotalPeriodDetail);
			}
		}

		virtual public void StoreBasisLowLevelTotalGroupTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StoreBasisLowLevelTotalGroupTotalPeriodDetail(aPlanCube);

            aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalGroupTotalWeekDetail,
                new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
            aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalStoreTotalPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisGroupTotalPeriodDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisGroupTotalPeriodDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisGroupTotalPeriodDetail);

			if (aPlanDisplayType == ePlanDisplayType.Period)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalGroupTotalDateTotal,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));

				aPlanCube.AddTotalCube(eCubeType.StoreBasisLowLevelTotalGroupTotalDateTotal);
			}
		}
		
		virtual public void StoreBasisLowLevelTotalStoreTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StoreBasisLowLevelTotalStoreTotalWeekDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalGroupTotalWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalStoreTotalDateTotal,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisStoreTotalWeekDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisStoreTotalWeekDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisStoreTotalWeekDetail);

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddTotalCube(eCubeType.StoreBasisLowLevelTotalStoreTotalDateTotal);
			}
		}
		
		virtual public void StoreBasisLowLevelTotalStoreTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StoreBasisLowLevelTotalStoreTotalDateTotal(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalStoreTotalWeekDetail,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisStoreTotalDateTotal,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalGroupTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));

				aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisLowLevelTotalStoreTotalWeekDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisLowLevelTotalStoreTotalWeekDetail);
			}
			else
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalStoreTotalPeriodDetail,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalGroupTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));

				aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisLowLevelTotalStoreTotalPeriodDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisLowLevelTotalStoreTotalPeriodDetail);
			}
		}
		
		virtual public void StoreBasisLowLevelTotalStoreTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StoreBasisLowLevelTotalStoreTotalPeriodDetail(aPlanCube);

            aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalStoreTotalWeekDetail,
                new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
            aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalStoreTotalPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisStoreTotalPeriodDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisStoreTotalPeriodDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisStoreTotalPeriodDetail);

			if (aPlanDisplayType == ePlanDisplayType.Period)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisLowLevelTotalStoreTotalDateTotal,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));

				aPlanCube.AddTotalCube(eCubeType.StoreBasisLowLevelTotalStoreTotalDateTotal);
			}
		}
		
		// Store Plan Cubes

		virtual public void StorePlanWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StorePlanWeekDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanStoreTotalWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanGroupTotalWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanDateTotal,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanPeriodDetail,
				new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalWeekDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.AddTotalCube(eCubeType.StorePlanStoreTotalWeekDetail);
			aPlanCube.AddTotalCube(eCubeType.StorePlanGroupTotalWeekDetail);
			aPlanCube.AddTotalCube(eCubeType.StorePlanPeriodDetail);
			aPlanCube.AddTotalCube(eCubeType.StorePlanLowLevelTotalWeekDetail);

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddTotalCube(eCubeType.StorePlanDateTotal);
			}
		}
		
		virtual public void StorePlanDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StorePlanDateTotal(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanWeekDetail,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalDateTotal,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.AddTotalCube(eCubeType.StorePlanGroupTotalDateTotal);
			aPlanCube.AddTotalCube(eCubeType.StorePlanStoreTotalDateTotal);

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanStoreTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanGroupTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));

				aPlanCube.SetComponentDetailCube(eCubeType.StorePlanWeekDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanWeekDetail);
			}
			else
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanPeriodDetail,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanStoreTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanGroupTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));

				aPlanCube.SetComponentDetailCube(eCubeType.StorePlanPeriodDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanPeriodDetail);
			}
		}

		virtual public void StorePlanPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StorePlanPeriodDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanWeekDetail,
				new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanStoreTotalPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanGroupTotalPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalPeriodDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.AddTotalCube(eCubeType.StorePlanLowLevelTotalPeriodDetail);
			aPlanCube.SetComponentDetailCube(eCubeType.StorePlanWeekDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanWeekDetail);

			if (aPlanDisplayType == ePlanDisplayType.Period)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanDateTotal,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));

				aPlanCube.AddTotalCube(eCubeType.StorePlanDateTotal);
			}
		}

		virtual public void StorePlanGroupTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StorePlanGroupTotalWeekDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanStoreTotalWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanGroupTotalDateTotal,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanGroupTotalPeriodDetail,
				new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalGroupTotalWeekDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.AddTotalCube(eCubeType.StorePlanGroupTotalPeriodDetail);
			aPlanCube.AddTotalCube(eCubeType.StorePlanLowLevelTotalGroupTotalWeekDetail);
			aPlanCube.SetComponentDetailCube(eCubeType.StorePlanWeekDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanWeekDetail);
		
			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddTotalCube(eCubeType.StorePlanGroupTotalDateTotal);
			}
		}
		
		virtual public void StorePlanGroupTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StorePlanGroupTotalDateTotal(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanGroupTotalWeekDetail,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalGroupTotalDateTotal,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanDateTotal,
					new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanStoreTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));

				aPlanCube.SetComponentDetailCube(eCubeType.StorePlanGroupTotalWeekDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanGroupTotalWeekDetail);
			}
			else
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanGroupTotalPeriodDetail,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanDateTotal,
					new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanStoreTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));

				aPlanCube.SetComponentDetailCube(eCubeType.StorePlanGroupTotalPeriodDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanGroupTotalPeriodDetail);
			}
		}

		virtual public void StorePlanGroupTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StorePlanGroupTotalPeriodDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanGroupTotalWeekDetail,
				new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanStoreTotalPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalGroupTotalPeriodDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.AddTotalCube(eCubeType.StorePlanLowLevelTotalGroupTotalPeriodDetail);
			aPlanCube.SetComponentDetailCube(eCubeType.StorePlanGroupTotalWeekDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanGroupTotalWeekDetail);

			if (aPlanDisplayType == ePlanDisplayType.Period)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanGroupTotalDateTotal,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));

				aPlanCube.AddTotalCube(eCubeType.StorePlanGroupTotalDateTotal);
			}
		}
		
		virtual public void StorePlanStoreTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StorePlanStoreTotalWeekDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanGroupTotalWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanStoreTotalDateTotal,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanStoreTotalPeriodDetail,
				new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalStoreTotalWeekDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.AddTotalCube(eCubeType.StorePlanStoreTotalPeriodDetail);
			aPlanCube.AddTotalCube(eCubeType.StorePlanLowLevelTotalStoreTotalWeekDetail);
			aPlanCube.SetComponentDetailCube(eCubeType.StorePlanWeekDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanWeekDetail);
			
			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddTotalCube(eCubeType.StorePlanStoreTotalDateTotal);
			}
		}
		
		virtual public void StorePlanStoreTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StorePlanStoreTotalDateTotal(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanStoreTotalWeekDetail,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalStoreTotalDateTotal,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanGroupTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));

				aPlanCube.SetComponentDetailCube(eCubeType.StorePlanStoreTotalWeekDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanStoreTotalWeekDetail);
			}
			else
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanStoreTotalPeriodDetail,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanGroupTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));

				aPlanCube.SetComponentDetailCube(eCubeType.StorePlanStoreTotalPeriodDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanStoreTotalPeriodDetail);
			}
		}
		
		virtual public void StorePlanStoreTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StorePlanStoreTotalPeriodDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanStoreTotalWeekDetail,
				new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanStoreTotalPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalStoreTotalPeriodDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.AddTotalCube(eCubeType.StorePlanLowLevelTotalStoreTotalPeriodDetail);
			aPlanCube.SetComponentDetailCube(eCubeType.StorePlanStoreTotalWeekDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanStoreTotalWeekDetail);

			if (aPlanDisplayType == ePlanDisplayType.Period)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanStoreTotalDateTotal,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));

				aPlanCube.AddTotalCube(eCubeType.StorePlanStoreTotalDateTotal);
			}
		}

		virtual public void StorePlanLowLevelTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StorePlanLowLevelTotalWeekDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalStoreTotalWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalGroupTotalWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalDateTotal,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanWeekDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.SetComponentDetailCube(eCubeType.StorePlanWeekDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanWeekDetail);

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddTotalCube(eCubeType.StorePlanLowLevelTotalDateTotal);
			}
		}
		
		virtual public void StorePlanLowLevelTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StorePlanLowLevelTotalDateTotal(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalWeekDetail,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanDateTotal,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.AddTotalCube(eCubeType.StorePlanLowLevelTotalGroupTotalDateTotal);
			aPlanCube.AddTotalCube(eCubeType.StorePlanLowLevelTotalStoreTotalDateTotal);

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalStoreTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalGroupTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));

				aPlanCube.SetComponentDetailCube(eCubeType.StorePlanLowLevelTotalWeekDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanLowLevelTotalWeekDetail);
			}
			else
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalPeriodDetail,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalStoreTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalGroupTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));

				aPlanCube.SetComponentDetailCube(eCubeType.StorePlanLowLevelTotalPeriodDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanLowLevelTotalPeriodDetail);
			}
		}

		virtual public void StorePlanLowLevelTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StorePlanLowLevelTotalPeriodDetail(aPlanCube);

            aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalWeekDetail,
                new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
            aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalStoreTotalPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalGroupTotalPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanPeriodDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.SetComponentDetailCube(eCubeType.StorePlanPeriodDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanPeriodDetail);

			if (aPlanDisplayType == ePlanDisplayType.Period)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalDateTotal,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));

				aPlanCube.AddTotalCube(eCubeType.StorePlanLowLevelTotalDateTotal);
			}
		}

		virtual public void StorePlanLowLevelTotalGroupTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StorePlanLowLevelTotalGroupTotalWeekDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalStoreTotalWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalGroupTotalDateTotal,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanGroupTotalWeekDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.SetComponentDetailCube(eCubeType.StorePlanGroupTotalWeekDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanGroupTotalWeekDetail);

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddTotalCube(eCubeType.StorePlanLowLevelTotalGroupTotalDateTotal);
			}
		}
		
		virtual public void StorePlanLowLevelTotalGroupTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StorePlanLowLevelTotalGroupTotalDateTotal(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalGroupTotalWeekDetail,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanGroupTotalDateTotal,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalStoreTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));

				aPlanCube.SetComponentDetailCube(eCubeType.StorePlanLowLevelTotalGroupTotalWeekDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanLowLevelTotalGroupTotalWeekDetail);
			}
			else
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalGroupTotalPeriodDetail,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalStoreTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));

				aPlanCube.SetComponentDetailCube(eCubeType.StorePlanLowLevelTotalGroupTotalPeriodDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanLowLevelTotalGroupTotalPeriodDetail);
			}
		}

		virtual public void StorePlanLowLevelTotalGroupTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StorePlanLowLevelTotalGroupTotalPeriodDetail(aPlanCube);

            aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalGroupTotalWeekDetail,
                new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
            aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.StoreGroupLevel, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalStoreTotalPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanGroupTotalPeriodDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.SetComponentDetailCube(eCubeType.StorePlanGroupTotalPeriodDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanGroupTotalPeriodDetail);

			if (aPlanDisplayType == ePlanDisplayType.Period)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalGroupTotalDateTotal,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));

				aPlanCube.AddTotalCube(eCubeType.StorePlanLowLevelTotalGroupTotalDateTotal);
			}
		}
		
		virtual public void StorePlanLowLevelTotalStoreTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StorePlanLowLevelTotalStoreTotalWeekDetail(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalGroupTotalWeekDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalStoreTotalDateTotal,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanStoreTotalWeekDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.SetComponentDetailCube(eCubeType.StorePlanStoreTotalWeekDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanStoreTotalWeekDetail);

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddTotalCube(eCubeType.StorePlanLowLevelTotalStoreTotalDateTotal);
			}
		}
		
		virtual public void StorePlanLowLevelTotalStoreTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StorePlanLowLevelTotalStoreTotalDateTotal(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalStoreTotalWeekDetail,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanStoreTotalDateTotal,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			if (aPlanDisplayType == ePlanDisplayType.Week)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalGroupTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));

				aPlanCube.SetComponentDetailCube(eCubeType.StorePlanLowLevelTotalStoreTotalWeekDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanLowLevelTotalStoreTotalWeekDetail);
			}
			else
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalStoreTotalPeriodDetail,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalGroupTotalDateTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));

				aPlanCube.SetComponentDetailCube(eCubeType.StorePlanLowLevelTotalStoreTotalPeriodDetail);
				aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanLowLevelTotalStoreTotalPeriodDetail);
			}
		}
		
		virtual public void StorePlanLowLevelTotalStoreTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StorePlanLowLevelTotalStoreTotalPeriodDetail(aPlanCube);

            aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalStoreTotalWeekDetail,
                new PeriodWeekPlanCubeRelationshipItem(eProfileType.Period, eProfileType.Week, eProfileListType.None)));
            aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.Store, eProfileListType.Filtered)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalStoreTotalPeriodDetail,
				new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));
			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanStoreTotalPeriodDetail,
				new LowLevelTotalPlanCubeRelationshipItem(eProfileType.StorePlan)));

			aPlanCube.SetComponentDetailCube(eCubeType.StorePlanStoreTotalPeriodDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StorePlanStoreTotalPeriodDetail);

			if (aPlanDisplayType == ePlanDisplayType.Period)
			{
				aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StorePlanLowLevelTotalStoreTotalDateTotal,
					new TimeTotalPlanCubeRelationshipItem(eProfileType.Period, eProfileListType.None)));

				aPlanCube.AddTotalCube(eCubeType.StorePlanLowLevelTotalStoreTotalDateTotal);
			}
		}
		//Begin TT#2 - JScott - Assortment Planning - Phase 2

		virtual public void StoreBasisGradeTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType)
		{
			BasePlanVariableInitialization.StoreBasisDateTotal(aPlanCube);

			aPlanCube.AddRelationship(new CubeRelationship(eCubeType.StoreBasisWeekDetail,
				new TimeTotalPlanCubeRelationshipItem(eProfileType.Week, eProfileListType.None),
				new SetGradeStorePlanCubeRelationshipItem()));

			aPlanCube.SetComponentDetailCube(eCubeType.StoreBasisWeekDetail);
			aPlanCube.SetSpreadDetailCube(eCubeType.StoreBasisWeekDetail);
		}
		//End TT#2 - JScott - Assortment Planning - Phase 2
	}
}
