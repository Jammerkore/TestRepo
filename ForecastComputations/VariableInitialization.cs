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

	abstract public class BasePlanVariableInitialization
	{
		//=======
		// FIELDS
		//=======

		protected BasePlanComputations _basePlanComputations;
		private InitConditionOperand[] _totalNeedConditions;
		private InitConditionOperand[] _regularNeedConditions;
		private InitConditionOperand[] _totalActualConditions;
		private InitConditionOperand[] _regularActualConditions;
		private InitConditionOperand[] _markdownActualConditions;
		private InitConditionOperand[] _actualCurrentConditions;
		private InitConditionOperand[] _totalActualCurrentConditions;
		private InitConditionOperand[] _regularActualCurrentConditions;
		private InitConditionOperand[] _markdownActualCurrentConditions;

		//=============
		// CONSTRUCTORS
		//=============

		public BasePlanVariableInitialization(BasePlanComputations aBasePlanComputations)
		{
			_basePlanComputations = aBasePlanComputations;
			_totalNeedConditions = null;
			_regularNeedConditions = null;
			_totalActualConditions = null;
			_regularActualConditions = null;
			_markdownActualConditions = null;
			_actualCurrentConditions = null;
			_totalActualCurrentConditions = null;
			_regularActualCurrentConditions = null;
			_markdownActualCurrentConditions = null;
		}

		//===========
		// PROPERTIES
		//===========

		protected BasePlanVariables BasePlanVariables
		{
			get
			{
				return _basePlanComputations.BasePlanVariables;
			}
		}

		protected BasePlanQuantityVariables BasePlanQuantityVariables
		{
			get
			{
				return _basePlanComputations.BasePlanQuantityVariables;
			}
		}

		protected BasePlanTimeTotalVariables BasePlanTimeTotalVariables
		{
			get
			{
				return _basePlanComputations.BasePlanTimeTotalVariables;
			}
		}

		protected BasePlanFormulasAndSpreads BasePlanFormulasAndSpreads
		{
			get
			{
				return _basePlanComputations.BasePlanFormulasAndSpreads;
			}
		}

		protected BasePlanChangeMethods BasePlanChangeMethods
		{
			get
			{
				return _basePlanComputations.BasePlanChangeMethods;
			}
		}

		protected BasePlanToolBox BasePlanToolBox
		{
			get
			{
				return _basePlanComputations.BasePlanToolBox;
			}
		}

		protected InitConditionOperand[] TotalNeedConditions
		{
			get
			{
				if (_totalNeedConditions == null)
				{
					_totalNeedConditions = new InitConditionOperand[] {	  new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isForecast), eConjunctionType.And),
																		  new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isTotalPlanType), eConjunctionType.And),
																		  new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isPlan), eConjunctionType.Or),
																		  new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isBasis)) };
				}

				return _totalNeedConditions;
			}
		}

		protected InitConditionOperand[] RegularNeedConditions
		{
			get
			{
				if (_regularNeedConditions == null)
				{
					_regularNeedConditions = new InitConditionOperand[] {	new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isForecast), eConjunctionType.And),
																			new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isRegularPlanType), eConjunctionType.And),
																			new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isPlan), eConjunctionType.Or),
																			new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isBasis)) };
				}

				return _regularNeedConditions;
			}
		}

		private InitConditionOperand[] ActualCurrentConditions
		{
			get
			{
				if (_actualCurrentConditions == null)
				{
					_actualCurrentConditions = new InitConditionOperand[] { new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isActual), eConjunctionType.Or), new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isCurrentPostingWeekInPlan)) };
				}
			
				return _actualCurrentConditions;
			}
		}

		protected InitConditionOperand[] TotalActualConditions
		{
			get
			{
				if (_totalActualConditions == null)
				{
					_totalActualConditions = new InitConditionOperand[] { new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isActual), eConjunctionType.Or), new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isTotalPlanType)) };
				}

				return _totalActualConditions;
			}
		}

		protected InitConditionOperand[] RegularActualConditions
		{
			get
			{
				if (_regularActualConditions == null)
				{
					_regularActualConditions = new InitConditionOperand[] { new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isActual), eConjunctionType.Or), new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isRegularPlanType)) };
				}

				return _regularActualConditions;
			}
		}

		protected InitConditionOperand[] MarkdownActualConditions
		{
			get
			{
				if (_markdownActualConditions == null)
				{
					_markdownActualConditions = new InitConditionOperand[] { new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isActual)) };
				}

				return _markdownActualConditions;
			}
		}

		protected InitConditionOperand[] TotalActualCurrentConditions
		{
			get
			{
				if (_totalActualCurrentConditions == null)
				{
					_totalActualCurrentConditions = new InitConditionOperand[] { new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isActual), eConjunctionType.Or), new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isTotalPlanType), eConjunctionType.And), new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isCurrentPostingWeekInPlan)) };
				}
			
				return _totalActualCurrentConditions;
			}
		}

		protected InitConditionOperand[] RegularActualCurrentConditions
		{
			get
			{
				if (_regularActualCurrentConditions == null)
				{
					_regularActualCurrentConditions = new InitConditionOperand[] { new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isActual), eConjunctionType.Or), new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isRegularPlanType), eConjunctionType.And), new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isCurrentPostingWeekInPlan)) };
				}
			
				return _regularActualCurrentConditions;
			}
		}

		protected InitConditionOperand[] MarkdownActualCurrentConditions
		{
			get
			{
				if (_markdownActualCurrentConditions == null)
				{
					_markdownActualCurrentConditions = new InitConditionOperand[] { new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isActual), eConjunctionType.And), new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isCurrentPostingWeekInPlan)) };
				}
			
				return _markdownActualCurrentConditions;
			}
		}

		//========
		// METHODS
		//========

		virtual public void ChainBasisDetail(PlanCube aPlanCube)
		{
			// Value Rules

			WeekDetail_Detail_ValueRules(aPlanCube);
		}

		virtual public void ChainBasisWeekDetail(PlanCube aPlanCube)
		{
			// Value Rules

			WeekDetail_Basis_ValueRules(aPlanCube);

			// PctChange Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToTimePeriod Rules

			TimeDetail_PctToTimeTotalRules(aPlanCube);

			// PctToLowLevelTotal Rules

			TimeDetail_PctToLowLevelRules(aPlanCube);

			// Difference Rules

			TimeDetail_ChainStoreDifferenceRules(aPlanCube);
		}

		virtual public void ChainBasisLowLevelTotalWeekDetail(PlanCube aPlanCube)
		{
			ChainPlanLowLevelTotalWeekDetail(aPlanCube);
		}

		virtual public void ChainBasisPeriodDetail(PlanCube aPlanCube)
		{
			ChainPlanPeriodDetail(aPlanCube);
		}

		virtual public void ChainBasisLowLevelTotalPeriodDetail(PlanCube aPlanCube)
		{
			ChainPlanLowLevelTotalPeriodDetail(aPlanCube);
		}

		virtual public void ChainBasisDateTotal(PlanCube aPlanCube)
		{
			ChainPlanDateTotal(aPlanCube);
		}

		virtual public void ChainBasisLowLevelTotalDateTotal(PlanCube aPlanCube)
		{
			ChainPlanLowLevelTotalDateTotal(aPlanCube);
		}

		virtual public void ChainPlanWeekDetail(PlanCube aPlanCube)
		{
			// Value Rules

			WeekDetail_Detail_ValueRules(aPlanCube);

			// PctChange Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToTimePeriod Rules

			TimeDetail_PctToTimeTotalRules(aPlanCube);

			// PctToLowLevelTotal Rules

			TimeDetail_PctToLowLevelRules(aPlanCube);

			// Difference Rules

			TimeDetail_ChainStoreDifferenceRules(aPlanCube);
		}

		virtual public void ChainPlanLowLevelTotalWeekDetail(PlanCube aPlanCube)
		{
			// Value Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_SumLowLevelTotalDetail, BasePlanFormulasAndSpreads.Init_SumLowLevelRegularDetail, BasePlanChangeMethods.Change_LowLevelTotal);

			// PctChange Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToTimePeriod Rules

			TimeDetail_PctToTimeTotalRules(aPlanCube);

			// LowLevelAverage Rules

			TimeDetail_LowLevelAverageRules(aPlanCube);

			// Balance Rules

			TimeDetail_ChainLowLevelBalanceRules(aPlanCube);
		}

		virtual public void ChainPlanPeriodDetail(PlanCube aPlanCube)
		{
			// Value Rules

			PeriodDetail_Detail_ValueRules(aPlanCube);

			// PctChange Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToTimePeriod Rules

			TimeDetail_PctToTimeTotalRules(aPlanCube);

			// PctToLowLevelTotal Rules

			TimeDetail_PctToLowLevelRules(aPlanCube);

			// Difference Rules

			TimeDetail_ChainStoreDifferenceRules(aPlanCube);
		}

		virtual public void ChainPlanLowLevelTotalPeriodDetail(PlanCube aPlanCube)
		{
			// Value Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_SumLowLevelTotalDetail, BasePlanFormulasAndSpreads.Init_SumLowLevelRegularDetail, BasePlanChangeMethods.Change_LowLevelTotal);

			// PctChange Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToTimePeriod Rules

			TimeDetail_PctToTimeTotalRules(aPlanCube);

			// LowLevelAverage Rules

			TimeDetail_LowLevelAverageRules(aPlanCube);

			// Balance Rules

			TimeDetail_ChainLowLevelBalanceRules(aPlanCube);
		}

		virtual public void ChainPlanDateTotal(PlanCube aPlanCube)
		{
			// Value Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.Value);

			// PctChange Rules

			TimeTotal_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeTotal_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToLowLevelTotal Rules

			TimeTotal_PctToLowLevelRules(aPlanCube);

			// Difference Rules

			TimeTotal_ChainStoreDifferenceRules(aPlanCube);
		}

		virtual public void ChainPlanLowLevelTotalDateTotal(PlanCube aPlanCube)
		{
			// Value Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.Value);

			// PctChange Rules

			TimeTotal_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeTotal_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// LowLevelAverage Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.LowLevelAverage);

			// Balance Rules

			TimeTotal_ChainLowLevelBalanceRules(aPlanCube);
		}

		virtual public void StoreBasisDetail(PlanCube aPlanCube)
		{
			// Value Rules

			WeekDetail_Detail_ValueRules(aPlanCube);
		}

		virtual public void StoreBasisWeekDetail(PlanCube aPlanCube)
		{
			// Value Rules

			WeekDetail_Basis_ValueRules(aPlanCube);

			// PctChange Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToSet Rules

			TimeDetail_PctToSetRules(aPlanCube);

			// PctToAllStore Rules

			TimeDetail_PctToAllRules(aPlanCube);

			// PctToTimePeriod Rules

			TimeDetail_PctToTimeTotalRules(aPlanCube);

			// PctToLowLevelTotal Rules

			TimeDetail_PctToLowLevelRules(aPlanCube);
		}

		virtual public void StoreBasisLowLevelTotalWeekDetail(PlanCube aPlanCube)
		{
			StorePlanLowLevelTotalWeekDetail(aPlanCube);
		}

		virtual public void StoreBasisGroupTotalWeekDetail(PlanCube aPlanCube)
		{
			StorePlanGroupTotalWeekDetail(aPlanCube);
		}

		virtual public void StoreBasisLowLevelTotalGroupTotalWeekDetail(PlanCube aPlanCube)
		{
			StorePlanLowLevelTotalGroupTotalWeekDetail(aPlanCube);
		}

		virtual public void StoreBasisStoreTotalWeekDetail(PlanCube aPlanCube)
		{
			StorePlanStoreTotalWeekDetail(aPlanCube);
		}

		virtual public void StoreBasisLowLevelTotalStoreTotalWeekDetail(PlanCube aPlanCube)
		{
			StorePlanLowLevelTotalStoreTotalWeekDetail(aPlanCube);
		}

		virtual public void StoreBasisPeriodDetail(PlanCube aPlanCube)
		{
			StorePlanPeriodDetail(aPlanCube);
		}

		virtual public void StoreBasisLowLevelTotalPeriodDetail(PlanCube aPlanCube)
		{
			StorePlanLowLevelTotalPeriodDetail(aPlanCube);
		}

		virtual public void StoreBasisGroupTotalPeriodDetail(PlanCube aPlanCube)
		{
			StorePlanGroupTotalPeriodDetail(aPlanCube);
		}

		virtual public void StoreBasisLowLevelTotalGroupTotalPeriodDetail(PlanCube aPlanCube)
		{
			StorePlanLowLevelTotalGroupTotalPeriodDetail(aPlanCube);
		}

		virtual public void StoreBasisStoreTotalPeriodDetail(PlanCube aPlanCube)
		{
			StorePlanStoreTotalPeriodDetail(aPlanCube);
		}

		virtual public void StoreBasisLowLevelTotalStoreTotalPeriodDetail(PlanCube aPlanCube)
		{
			StorePlanLowLevelTotalStoreTotalPeriodDetail(aPlanCube);
		}

		virtual public void StoreBasisDateTotal(PlanCube aPlanCube)
		{
			StorePlanDateTotal(aPlanCube);
		}

		virtual public void StoreBasisLowLevelTotalDateTotal(PlanCube aPlanCube)
		{
			StorePlanLowLevelTotalDateTotal(aPlanCube);
		}

		virtual public void StoreBasisGroupTotalDateTotal(PlanCube aPlanCube)
		{
			StorePlanGroupTotalDateTotal(aPlanCube);
		}

		virtual public void StoreBasisLowLevelTotalGroupTotalDateTotal(PlanCube aPlanCube)
		{
			StorePlanLowLevelTotalGroupTotalDateTotal(aPlanCube);
		}

		virtual public void StoreBasisStoreTotalDateTotal(PlanCube aPlanCube)
		{
			StorePlanStoreTotalDateTotal(aPlanCube);
		}

		virtual public void StoreBasisLowLevelTotalStoreTotalDateTotal(PlanCube aPlanCube)
		{
			StorePlanLowLevelTotalStoreTotalDateTotal(aPlanCube);
		}

		virtual public void StorePlanWeekDetail(PlanCube aPlanCube)
		{
			// Value Rules

			WeekDetail_Detail_ValueRules(aPlanCube);

			// PctChange Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToSet Rules

			TimeDetail_PctToSetRules(aPlanCube);

			// PctToAllStore Rules

			TimeDetail_PctToAllRules(aPlanCube);

			// PctToTimePeriod Rules

			TimeDetail_PctToTimeTotalRules(aPlanCube);

			// PctToLowLevelTotal Rules

			TimeDetail_PctToLowLevelRules(aPlanCube);
		}

		virtual public void StorePlanLowLevelTotalWeekDetail(PlanCube aPlanCube)
		{
			// Value Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_SumLowLevelTotalDetail, BasePlanFormulasAndSpreads.Init_SumLowLevelRegularDetail, BasePlanChangeMethods.Change_LowLevelTotal);

			// PctChange Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToSet Rules

			TimeDetail_PctToSetRules(aPlanCube);

			// PctToAllStore Rules

			TimeDetail_PctToAllRules(aPlanCube);

			// PctToTimePeriod Rules

			TimeDetail_PctToTimeTotalRules(aPlanCube);

			// LowLevelAverage Rules

			TimeDetail_LowLevelAverageRules(aPlanCube);

			// Balance Rules

			TimeDetail_StoreLowLevelBalanceRules(aPlanCube);
		}

		virtual public void StorePlanPeriodDetail(PlanCube aPlanCube)
		{
			// Value Rules

			PeriodDetail_Detail_ValueRules(aPlanCube);

			// PctChange Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToSet Rules

			TimeDetail_PctToSetRules(aPlanCube);

			// PctToAllStore Rules

			TimeDetail_PctToAllRules(aPlanCube);

			// PctToTimePeriod Rules

			TimeDetail_PctToTimeTotalRules(aPlanCube);

			// PctToLowLevelTotal Rules

			TimeDetail_PctToLowLevelRules(aPlanCube);
		}

		virtual public void StorePlanLowLevelTotalPeriodDetail(PlanCube aPlanCube)
		{
			// Value Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_SumLowLevelTotalDetail, BasePlanFormulasAndSpreads.Init_SumLowLevelRegularDetail, BasePlanChangeMethods.Change_LowLevelTotal);

			// PctChange Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToSet Rules

			TimeDetail_PctToSetRules(aPlanCube);

			// PctToAllStore Rules

			TimeDetail_PctToAllRules(aPlanCube);

			// PctToTimePeriod Rules

			TimeDetail_PctToTimeTotalRules(aPlanCube);

			// LowLevelAverage Rules

			TimeDetail_LowLevelAverageRules(aPlanCube);

			// Balance Rules

			TimeDetail_StoreLowLevelBalanceRules(aPlanCube);
		}

		virtual public void StorePlanDateTotal(PlanCube aPlanCube)
		{
			// Value Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.Value);

			// PctChange Rules

			TimeTotal_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeTotal_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToSet Rules

			TimeTotal_PctToSetRules(aPlanCube);

			// PctToAllStore Rules

			TimeTotal_PctToAllRules(aPlanCube);

			// PctToLowLevelTotal Rules

			TimeTotal_PctToLowLevelRules(aPlanCube);
		}

		virtual public void StorePlanLowLevelTotalDateTotal(PlanCube aPlanCube)
		{
			// Value Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.Value);

			// PctChange Rules

			TimeTotal_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeTotal_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToSet Rules

			TimeTotal_PctToSetRules(aPlanCube);

			// PctToAllStore Rules

			TimeTotal_PctToAllRules(aPlanCube);

			// LowLevelAverage Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.LowLevelAverage);

			// Balance Rules

			TimeTotal_StoreLowLevelBalanceRules(aPlanCube);
		}

		virtual public void StorePlanGroupTotalWeekDetail(PlanCube aPlanCube)
		{
			// Value Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_SetTotal);

			// StoreAverage Rules

			TimeDetail_StoreAverageRules(aPlanCube, BasePlanChangeMethods.Change_SetAverage);

			// PctChange Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToAllStore Rules

			TimeDetail_PctToAllRules(aPlanCube);

			// PctToTimePeriod Rules

			TimeDetail_PctToTimeTotalRules(aPlanCube);

			// PctToLowLevelTotal Rules

			TimeDetail_PctToLowLevelRules(aPlanCube);

			// Comp Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.Comp, BasePlanFormulasAndSpreads.Init_SumCompStore, BasePlanChangeMethods.Change_SetCompTotal);

			// NonComp Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.NonComp, BasePlanFormulasAndSpreads.Init_SumNonCompStore, BasePlanChangeMethods.Change_SetNonCompTotal);

			// New Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.New, BasePlanFormulasAndSpreads.Init_SumNewStore, BasePlanChangeMethods.Change_SetNewTotal);
		}

		virtual public void StorePlanLowLevelTotalGroupTotalWeekDetail(PlanCube aPlanCube)
		{
			// Value Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_SetTotal);

			// StoreAverage Rules

			TimeDetail_StoreAverageRules(aPlanCube, BasePlanChangeMethods.Change_SetAverage);

			// PctChange Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToAllStore Rules

			TimeDetail_PctToAllRules(aPlanCube);

			// PctToTimePeriod Rules

			TimeDetail_PctToTimeTotalRules(aPlanCube);

			// Comp Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.Comp, BasePlanFormulasAndSpreads.Init_SumCompStore, BasePlanChangeMethods.Change_SetCompTotal);

			// NonComp Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.NonComp, BasePlanFormulasAndSpreads.Init_SumNonCompStore, BasePlanChangeMethods.Change_SetNonCompTotal);

			// New Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.New, BasePlanFormulasAndSpreads.Init_SumNewStore, BasePlanChangeMethods.Change_SetNewTotal);

			// LowLevelAverage Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.LowLevelAverage, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_SetTotal);

			// Balance Rules

			TimeDetail_StoreLowLevelBalanceRules(aPlanCube);
		}

		virtual public void StorePlanGroupTotalPeriodDetail(PlanCube aPlanCube)
		{
			// Value Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_SetTotal);

			// StoreAverage Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.StoreAverage, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_SetTotal);

			// PctChange Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToAllStore Rules

			TimeDetail_PctToAllRules(aPlanCube);

			// PctToTimePeriod Rules

			TimeDetail_PctToTimeTotalRules(aPlanCube);

			// PctToLowLevelTotal Rules

			TimeDetail_PctToLowLevelRules(aPlanCube);

			// Comp Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.Comp, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_TotalPeriodToWeeks);

			// NonComp Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.NonComp, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_TotalPeriodToWeeks);

			// New Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.New, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_TotalPeriodToWeeks);
		}

		virtual public void StorePlanLowLevelTotalGroupTotalPeriodDetail(PlanCube aPlanCube)
		{
			// Value Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_SetTotal);

			// StoreAverage Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.StoreAverage, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_SetTotal);

			// PctChange Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToAllStore Rules

			TimeDetail_PctToAllRules(aPlanCube);

			// PctToTimePeriod Rules

			TimeDetail_PctToTimeTotalRules(aPlanCube);

			// Comp Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.Comp, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_TotalPeriodToWeeks);

			// NonComp Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.NonComp, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_TotalPeriodToWeeks);

			// New Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.New, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_TotalPeriodToWeeks);

			// LowLevelAverage Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.LowLevelAverage, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_SetTotal);

			// Balance Rules

			TimeDetail_StoreLowLevelBalanceRules(aPlanCube);
		}

		virtual public void StorePlanGroupTotalDateTotal(PlanCube aPlanCube)
		{
			// Value Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.Value);

			// StoreAverage Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.StoreAverage);

			// PctChange Rules

			TimeTotal_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeTotal_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToAllStore Rules

			TimeTotal_PctToAllRules(aPlanCube);

			// PctToLowLevelTotal Rules

			TimeTotal_PctToLowLevelRules(aPlanCube);

			// Comp Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.Comp);

			// NonComp Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.NonComp);

			// New Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.New);
		}

		virtual public void StorePlanLowLevelTotalGroupTotalDateTotal(PlanCube aPlanCube)
		{
			// Value Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.Value);

			// StoreAverage Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.StoreAverage);

			// PctChange Rules

			TimeTotal_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeTotal_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToAllStore Rules

			TimeTotal_PctToAllRules(aPlanCube);

			// Comp Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.Comp);

			// NonComp Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.NonComp);

			// New Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.New);

			// LowLevelAverage Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.LowLevelAverage);

			// Balance Rules

			TimeTotal_StoreLowLevelBalanceRules(aPlanCube);
		}

		virtual public void StorePlanStoreTotalWeekDetail(PlanCube aPlanCube)
		{
			// Value Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_AllTotal);

			// StoreAverage Rules

			TimeDetail_StoreAverageRules(aPlanCube, BasePlanChangeMethods.Change_AllAverage);

			// PctChange Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToTimePeriod Rules

			TimeDetail_PctToTimeTotalRules(aPlanCube);

			// PctToLowLevelTotal Rules

			TimeDetail_PctToLowLevelRules(aPlanCube);

			// Comp Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.Comp, BasePlanFormulasAndSpreads.Init_SumCompStore, BasePlanChangeMethods.Change_AllCompTotal);

			// NonComp Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.NonComp, BasePlanFormulasAndSpreads.Init_SumNonCompStore, BasePlanChangeMethods.Change_AllNonCompTotal);

			// New Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.New, BasePlanFormulasAndSpreads.Init_SumNewStore, BasePlanChangeMethods.Change_AllNewTotal);
		}

		virtual public void StorePlanLowLevelTotalStoreTotalWeekDetail(PlanCube aPlanCube)
		{
			// Value Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_AllTotal);

			// StoreAverage Rules

			TimeDetail_StoreAverageRules(aPlanCube, BasePlanChangeMethods.Change_AllAverage);

			// PctChange Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToTimePeriod Rules

			TimeDetail_PctToTimeTotalRules(aPlanCube);

			// Comp Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.Comp, BasePlanFormulasAndSpreads.Init_SumCompStore, BasePlanChangeMethods.Change_AllCompTotal);

			// NonComp Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.NonComp, BasePlanFormulasAndSpreads.Init_SumNonCompStore, BasePlanChangeMethods.Change_AllNonCompTotal);

			// New Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.New, BasePlanFormulasAndSpreads.Init_SumNewStore, BasePlanChangeMethods.Change_AllNewTotal);

			// LowLevelAverage Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.LowLevelAverage, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_AllTotal);

			// Balance Rules

			TimeDetail_StoreLowLevelBalanceRules(aPlanCube);
		}

		virtual public void StorePlanStoreTotalPeriodDetail(PlanCube aPlanCube)
		{
			// Value Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_AllTotal);

			// StoreAverage Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.StoreAverage, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_AllTotal);

			// PctChange Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToTimePeriod Rules

			TimeDetail_PctToTimeTotalRules(aPlanCube);

			// PctToLowLevelTotal Rules

			TimeDetail_PctToLowLevelRules(aPlanCube);

			// Comp Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.Comp, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_TotalPeriodToWeeks);

			// NonComp Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.NonComp, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_TotalPeriodToWeeks);

			// New Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.New, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_TotalPeriodToWeeks);
		}

		virtual public void StorePlanLowLevelTotalStoreTotalPeriodDetail(PlanCube aPlanCube)
		{
			// Value Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_AllTotal);

			// StoreAverage Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.StoreAverage, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_AllTotal);

			// PctChange Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeDetail_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToTimePeriod Rules

			TimeDetail_PctToTimeTotalRules(aPlanCube);

			// Comp Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.Comp, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_TotalPeriodToWeeks);

			// NonComp Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.NonComp, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_TotalPeriodToWeeks);

			// New Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.New, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_TotalPeriodToWeeks);

			// LowLevelAverage Rules

			TimeDetail_Total_ValueRules(aPlanCube, BasePlanQuantityVariables.LowLevelAverage, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_AllTotal);

			// Balance Rules

			TimeDetail_StoreLowLevelBalanceRules(aPlanCube);
		}

		virtual public void StorePlanStoreTotalDateTotal(PlanCube aPlanCube)
		{
			// Value Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.Value);

			// StoreAverage Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.StoreAverage);

			// PctChange Rules

			TimeTotal_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeTotal_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// PctToLowLevelTotal Rules

			TimeTotal_PctToLowLevelRules(aPlanCube);

			// Comp Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.Comp);

			// NonComp Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.NonComp);

			// New Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.New);
		}

		virtual public void StorePlanLowLevelTotalStoreTotalDateTotal(PlanCube aPlanCube)
		{
			// Value Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.Value);

			// StoreAverage Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.StoreAverage);

			// PctChange Rules

			TimeTotal_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange);

			// PctChangeToPlan Rules

			TimeTotal_PctChangeRules(aPlanCube, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, null);

			// Comp Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.Comp);

			// NonComp Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.NonComp);

			// New Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.New);

			// LowLevelAverage Rules

			TimeTotal_ValueRules(aPlanCube, BasePlanQuantityVariables.LowLevelAverage);

			// Balance Rules

			TimeTotal_StoreLowLevelBalanceRules(aPlanCube);
		}

		#region Generic Routines

		private void WeekDetail_Detail_ValueRules(PlanCube aPlanCube)
		{
			aPlanCube.AddRule(BasePlanVariables.SalesTotalUnits, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesTotalUnits, BasePlanChangeMethods.Change_SalesTotalUnits, null);
			aPlanCube.AddRule(BasePlanVariables.SalesRegularUnits, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesRegularUnits, BasePlanChangeMethods.Change_SalesRegularUnits, null);
			aPlanCube.AddRule(BasePlanVariables.SalesPromoUnits, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesPromoUnits, BasePlanChangeMethods.Change_SalesPromoUnits, null);
			aPlanCube.AddRule(BasePlanVariables.SalesRegPromoUnits, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesRegPromoUnits, BasePlanChangeMethods.Change_SalesRegPromoUnits, null);
			aPlanCube.AddRule(BasePlanVariables.SalesMarkdownUnits, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesMarkdownUnits, null, null, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryTotalUnits, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_InventoryTotalUnits, BasePlanChangeMethods.Change_InventoryTotalUnits, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryRegularUnits, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_InventoryRegularUnits, BasePlanChangeMethods.Change_InventoryRegularUnits, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownUnits, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_InventoryMarkdownUnits, null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptTotalUnits, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ReceiptTotalUnits, null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptRegularUnits, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ReceiptRegularUnits, null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptMarkdownUnits, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ReceiptMarkdownUnits, null, null);
			aPlanCube.AddRule(BasePlanVariables.SalesStockRatioTotal, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesStockRatioTotal, null, null);
			aPlanCube.AddRule(BasePlanVariables.SalesStockRatioRegPromo, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesStockRatioRegPromo, null, null);
			aPlanCube.AddRule(BasePlanVariables.SalesStockRatioMarkdown, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesStockRatioMarkdown, null, null);
			aPlanCube.AddRule(BasePlanVariables.ForwardWOSTotal, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSTotal, null, null);
			aPlanCube.AddRule(BasePlanVariables.ForwardWOSRegPromo, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSRegPromo, null, null);
			aPlanCube.AddRule(BasePlanVariables.ForwardWOSMarkdown, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSMarkdown, null, null);
			aPlanCube.AddRule(BasePlanVariables.SellThruPctTotal, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctTotal, null, null);
			aPlanCube.AddRule(BasePlanVariables.SellThruPctRegPromo, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctRegPromo, null, null);
			aPlanCube.AddRule(BasePlanVariables.SellThruPctMarkdown, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctMarkdown, null, null);
          
			if (aPlanCube.isStoreCube)
			{
				aPlanCube.AddRule(BasePlanVariables.SalesTotalSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesTotalSetIndex, BasePlanChangeMethods.Change_SalesTotalSetIndex, null);
				aPlanCube.AddRule(BasePlanVariables.SalesRegularSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesRegularSetIndex, BasePlanChangeMethods.Change_SalesRegularSetIndex, null);
				aPlanCube.AddRule(BasePlanVariables.SalesPromoSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesPromoSetIndex, BasePlanChangeMethods.Change_SalesPromoSetIndex, null);
				aPlanCube.AddRule(BasePlanVariables.SalesRegPromoSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesRegPromoSetIndex, BasePlanChangeMethods.Change_SalesRegPromoSetIndex, null);
				aPlanCube.AddRule(BasePlanVariables.SalesMarkdownSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesMarkdownSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SalesTotalAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesTotalAllIndex, BasePlanChangeMethods.Change_SalesTotalAllIndex, null);
				aPlanCube.AddRule(BasePlanVariables.SalesRegularAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesRegularAllIndex, BasePlanChangeMethods.Change_SalesRegularAllIndex, null);
				aPlanCube.AddRule(BasePlanVariables.SalesPromoAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesPromoAllIndex, BasePlanChangeMethods.Change_SalesPromoAllIndex, null);
				aPlanCube.AddRule(BasePlanVariables.SalesRegPromoAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesRegPromoAllIndex, BasePlanChangeMethods.Change_SalesRegPromoAllIndex, null);
				aPlanCube.AddRule(BasePlanVariables.SalesMarkdownAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesMarkdownAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.InventoryTotalSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_InventoryTotalSetIndex, BasePlanChangeMethods.Change_InventoryTotalSetIndex, null);
				aPlanCube.AddRule(BasePlanVariables.InventoryRegularSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_InventoryRegularSetIndex, BasePlanChangeMethods.Change_InventoryRegularSetIndex, null);
				aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_InventoryMarkdownSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.InventoryTotalAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_InventoryTotalAllIndex, BasePlanChangeMethods.Change_InventoryTotalAllIndex, null);
				aPlanCube.AddRule(BasePlanVariables.InventoryRegularAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_InventoryRegularAllIndex, BasePlanChangeMethods.Change_InventoryRegularAllIndex, null);
				aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_InventoryMarkdownAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.ForwardWOSTotalSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSTotalSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.ForwardWOSRegPromoSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSRegPromoSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.ForwardWOSMarkdownSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSMarkdownSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.ForwardWOSTotalAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSTotalAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.ForwardWOSRegPromoAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSRegPromoAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.ForwardWOSMarkdownAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSMarkdownAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SellThruPctTotalSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctTotalSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SellThruPctRegPromoSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctRegPromoSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SellThruPctMarkdownSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctMarkdownSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SellThruPctTotalAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctTotalAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SellThruPctRegPromoAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctRegPromoAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SellThruPctMarkdownAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctMarkdownAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.Intransit, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_Intransit, null, null);
				aPlanCube.AddRule(BasePlanVariables.GradeTotal, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_GradeTotal, null, null);
				aPlanCube.AddRule(BasePlanVariables.GradeRegPromo, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_GradeRegPromo, null, null);
				aPlanCube.AddRule(BasePlanVariables.StoreStatus, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_StoreStatus, null, null);
			}
		}

		private void PeriodDetail_Detail_ValueRules(PlanCube aPlanCube)
		{
			aPlanCube.AddRule(BasePlanVariables.SalesTotalUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_PeriodSumDetail, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_PeriodTotal, null, BasePlanChangeMethods.Change_AutototalSpreadLock);
			aPlanCube.AddRule(BasePlanVariables.SalesRegularUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_PeriodSumDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PeriodTotal, null, BasePlanChangeMethods.Change_AutototalSpreadLock);
			aPlanCube.AddRule(BasePlanVariables.SalesPromoUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_PeriodSumDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PeriodTotal, null, BasePlanChangeMethods.Change_AutototalSpreadLock);
            // Begin TT#1722 - RMatelic - Open Sales R/P to be editable
            //aPlanCube.AddRule(BasePlanVariables.SalesRegPromoUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_PeriodSumDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
            aPlanCube.AddRule(BasePlanVariables.SalesRegPromoUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_PeriodSumDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PeriodTotal, null, BasePlanChangeMethods.Change_AutototalInitOnly);
            // End TT#1722 
            aPlanCube.AddRule(BasePlanVariables.SalesMarkdownUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_PeriodSumDetail, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanVariables.InventoryTotalUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_PeriodSumDetail, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_PeriodTotal, null, BasePlanChangeMethods.Change_AutototalSpreadLock);
			aPlanCube.AddRule(BasePlanVariables.InventoryRegularUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_PeriodSumDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PeriodTotal, null, BasePlanChangeMethods.Change_AutototalSpreadLock);
			aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_PeriodSumDetail, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanVariables.ReceiptTotalUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_PeriodSumDetail, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanVariables.ReceiptRegularUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_PeriodSumDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanVariables.ReceiptMarkdownUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_PeriodSumDetail, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanVariables.SalesStockRatioTotal, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesStockRatioTotal, null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanVariables.SalesStockRatioRegPromo, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesStockRatioRegPromo, null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanVariables.SalesStockRatioMarkdown, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesStockRatioMarkdown, null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanVariables.ForwardWOSTotal, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_PeriodSumDetail, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanVariables.ForwardWOSRegPromo, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_PeriodSumDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanVariables.ForwardWOSMarkdown, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_PeriodSumDetail, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanVariables.SellThruPctTotal, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctTotal, null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanVariables.SellThruPctRegPromo, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctRegPromo, null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanVariables.SellThruPctMarkdown, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctMarkdown, null, null, BasePlanChangeMethods.Change_AutototalInitOnly);

			if (aPlanCube.isStoreCube)
			{
				aPlanCube.AddRule(BasePlanVariables.SalesTotalSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesTotalSetIndex, BasePlanChangeMethods.Change_SalesTotalSetIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SalesRegularSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesRegularSetIndex, BasePlanChangeMethods.Change_SalesRegularSetIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SalesPromoSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesPromoSetIndex, BasePlanChangeMethods.Change_SalesPromoSetIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SalesRegPromoSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesRegPromoSetIndex, BasePlanChangeMethods.Change_SalesRegPromoSetIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SalesMarkdownSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesMarkdownSetIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SalesTotalAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesTotalAllIndex, BasePlanChangeMethods.Change_SalesTotalAllIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SalesRegularAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesRegularAllIndex, BasePlanChangeMethods.Change_SalesRegularAllIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SalesPromoAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesPromoAllIndex, BasePlanChangeMethods.Change_SalesPromoAllIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SalesRegPromoAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesRegPromoAllIndex, BasePlanChangeMethods.Change_SalesRegPromoAllIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SalesMarkdownAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesMarkdownAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.InventoryTotalSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_InventoryTotalSetIndex, BasePlanChangeMethods.Change_InventoryTotalSetIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.InventoryRegularSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_InventoryRegularSetIndex, BasePlanChangeMethods.Change_InventoryRegularSetIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_InventoryMarkdownSetIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.InventoryTotalAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_InventoryTotalAllIndex, BasePlanChangeMethods.Change_InventoryTotalAllIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.InventoryRegularAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_InventoryRegularAllIndex, BasePlanChangeMethods.Change_InventoryRegularAllIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_InventoryMarkdownAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.ForwardWOSTotalSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSTotalSetIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.ForwardWOSRegPromoSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSRegPromoSetIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.ForwardWOSMarkdownSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSMarkdownSetIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.ForwardWOSTotalAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSTotalAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.ForwardWOSRegPromoAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSRegPromoAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.ForwardWOSMarkdownAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSMarkdownAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SellThruPctTotalSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctTotalSetIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SellThruPctRegPromoSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctRegPromoSetIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SellThruPctMarkdownSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctMarkdownSetIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SellThruPctTotalAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctTotalAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SellThruPctRegPromoAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctRegPromoAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SellThruPctMarkdownAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctMarkdownAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.Intransit, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_PeriodSumDetail, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.GradeTotal, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_GradeTotal, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.GradeRegPromo, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_GradeRegPromo, null, BasePlanChangeMethods.Change_AutototalInitOnly);
        	}
			//Begin TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
			else
			{
				aPlanCube.AddRule(BasePlanVariables.WOSTotal, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_PeriodAvgDetail, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_PeriodTotal, null, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.WOSRegPromo, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_PeriodAvgDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PeriodTotal, null, BasePlanChangeMethods.Change_AutototalSpreadLock);
			}
			//End TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
		}

		private void WeekDetail_Basis_ValueRules(PlanCube aPlanCube)
		{
			aPlanCube.AddRule(BasePlanVariables.SalesTotalUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanVariables.SalesRegularUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanVariables.SalesPromoUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanVariables.SalesRegPromoUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanVariables.SalesMarkdownUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryTotalUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryRegularUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptTotalUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptRegularUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptMarkdownUnits, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanVariables.SalesStockRatioTotal, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesStockRatioTotal, null, null, null);
			aPlanCube.AddRule(BasePlanVariables.SalesStockRatioRegPromo, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesStockRatioRegPromo, null, null, null);
			aPlanCube.AddRule(BasePlanVariables.SalesStockRatioMarkdown, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesStockRatioMarkdown, null, null, null);
			aPlanCube.AddRule(BasePlanVariables.ForwardWOSTotal, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSTotal, null, null, null);
			aPlanCube.AddRule(BasePlanVariables.ForwardWOSRegPromo, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSRegPromo, null, null, null);
			aPlanCube.AddRule(BasePlanVariables.ForwardWOSMarkdown, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSMarkdown, null, null, null);
			aPlanCube.AddRule(BasePlanVariables.SellThruPctTotal, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctTotal, null, null, null);
			aPlanCube.AddRule(BasePlanVariables.SellThruPctRegPromo, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctRegPromo, null, null, null);
			aPlanCube.AddRule(BasePlanVariables.SellThruPctMarkdown, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctMarkdown, null, null, null);

			if (aPlanCube.isStoreCube)
			{
				aPlanCube.AddRule(BasePlanVariables.SalesTotalSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesTotalSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SalesRegularSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesRegularSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SalesPromoSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesPromoSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SalesRegPromoSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesRegPromoSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SalesMarkdownSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesMarkdownSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SalesTotalAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesTotalAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SalesRegularAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesRegularAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SalesPromoAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesPromoAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SalesRegPromoAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesRegPromoAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SalesMarkdownAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SalesMarkdownAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.InventoryTotalSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_InventoryTotalSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.InventoryRegularSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_InventoryRegularSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_InventoryMarkdownSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.InventoryTotalAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_InventoryTotalAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.InventoryRegularAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_InventoryRegularAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_InventoryMarkdownAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.ForwardWOSTotalSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSTotalSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.ForwardWOSRegPromoSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSRegPromoSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.ForwardWOSMarkdownSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSMarkdownSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.ForwardWOSTotalAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSTotalAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.ForwardWOSRegPromoAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSRegPromoAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.ForwardWOSMarkdownAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_ForwardWOSMarkdownAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SellThruPctTotalSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctTotalSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SellThruPctRegPromoSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctRegPromoSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SellThruPctMarkdownSetIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctMarkdownSetIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SellThruPctTotalAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctTotalAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SellThruPctRegPromoAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctRegPromoAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.SellThruPctMarkdownAllStoreIndex, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SellThruPctMarkdownAllIndex, null, null);
				aPlanCube.AddRule(BasePlanVariables.Intransit, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, null, null);
				aPlanCube.AddRule(BasePlanVariables.GradeTotal, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_GradeTotal, null, null);
				aPlanCube.AddRule(BasePlanVariables.GradeRegPromo, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_GradeRegPromo, null, null);
				aPlanCube.AddRule(BasePlanVariables.StoreStatus, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_StoreStatus, null, null);
			}
			//Begin TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
			else
			{
				aPlanCube.AddRule(BasePlanVariables.WOSTotal, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
				aPlanCube.AddRule(BasePlanVariables.WOSRegPromo, BasePlanQuantityVariables.Value, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
			}
			//End TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
		}

		private void TimeDetail_Total_ValueRules(PlanCube aPlanCube, QuantityVariableProfile aQuantityVarProf, FormulaProfile aInitSumFormula, ChangeMethodProfile aPrimaryChangeMethod)
		{
			TimeDetail_Total_ValueRules(aPlanCube, aQuantityVarProf, true, aInitSumFormula, null, null, aPrimaryChangeMethod);
		}

		private void TimeDetail_Total_ValueRules(PlanCube aPlanCube, QuantityVariableProfile aQuantityVarProf, FormulaProfile aInitSumFormula, FormulaProfile aInitSumTotalFormula, FormulaProfile aInitSumRegularFormula, ChangeMethodProfile aPrimaryChangeMethod)
		{
			TimeDetail_Total_ValueRules(aPlanCube, aQuantityVarProf, false, aInitSumFormula, aInitSumTotalFormula, aInitSumRegularFormula, aPrimaryChangeMethod);
		}

		private void TimeDetail_Total_ValueRules(PlanCube aPlanCube, QuantityVariableProfile aQuantityVarProf, bool aUseDefaultSum, FormulaProfile aInitSumFormula, FormulaProfile aInitSumTotalFormula, FormulaProfile aInitSumRegularFormula, ChangeMethodProfile aPrimaryChangeMethod)
		{
			if (aUseDefaultSum)
			{
				aPlanCube.AddRule(BasePlanVariables.SalesTotalUnits, aQuantityVarProf, new InitCondition(aInitSumFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.SalesRegularUnits, aQuantityVarProf, new InitCondition(aInitSumFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.SalesPromoUnits, aQuantityVarProf, new InitCondition(aInitSumFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.SalesRegPromoUnits, aQuantityVarProf, new InitCondition(aInitSumFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.SalesMarkdownUnits, aQuantityVarProf, new InitCondition(aInitSumFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.InventoryTotalUnits, aQuantityVarProf, new InitCondition(aInitSumFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.InventoryRegularUnits, aQuantityVarProf, new InitCondition(aInitSumFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownUnits, aQuantityVarProf, new InitCondition(aInitSumFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.ReceiptTotalUnits, aQuantityVarProf, new InitCondition(aInitSumFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.ReceiptRegularUnits, aQuantityVarProf, new InitCondition(aInitSumFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.ReceiptMarkdownUnits, aQuantityVarProf, new InitCondition(aInitSumFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalSpreadLock);
			}
			else
			{
				aPlanCube.AddRule(BasePlanVariables.SalesTotalUnits, aQuantityVarProf, new InitCondition(aInitSumTotalFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.SalesRegularUnits, aQuantityVarProf, new InitCondition(aInitSumRegularFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.SalesPromoUnits, aQuantityVarProf, new InitCondition(aInitSumRegularFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.SalesRegPromoUnits, aQuantityVarProf, new InitCondition(aInitSumRegularFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.SalesMarkdownUnits, aQuantityVarProf, new InitCondition(aInitSumRegularFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.InventoryTotalUnits, aQuantityVarProf, new InitCondition(aInitSumTotalFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.InventoryRegularUnits, aQuantityVarProf, new InitCondition(aInitSumRegularFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownUnits, aQuantityVarProf, new InitCondition(aInitSumRegularFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.ReceiptTotalUnits, aQuantityVarProf, new InitCondition(aInitSumTotalFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.ReceiptRegularUnits, aQuantityVarProf, new InitCondition(aInitSumRegularFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.ReceiptMarkdownUnits, aQuantityVarProf, new InitCondition(aInitSumRegularFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalSpreadLock);
			}
        
			if (aPlanCube.isStoreCube)
			{
				aPlanCube.AddRule(BasePlanVariables.Intransit, aQuantityVarProf, aInitSumFormula, null, null, null);
                //Begin TT#3482-MD -jsobek -In Stock Sales - negative oh hands are not treated as 0.  In a store multi level view the all store, set and High and Low Levels are not populating for the In stock variables.
                aPlanCube.AddRule(BasePlanVariables.InStockSalesTotalUnits, aQuantityVarProf, aInitSumFormula, null, null, null);
                aPlanCube.AddRule(BasePlanVariables.InStockSalesRegularUnits, aQuantityVarProf, aInitSumFormula, null, null, null);
                aPlanCube.AddRule(BasePlanVariables.InStockSalesPromoUnits, aQuantityVarProf, aInitSumFormula, null, null, null);
                aPlanCube.AddRule(BasePlanVariables.InStockSalesMarkdownUnits, aQuantityVarProf, aInitSumFormula, null, null, null);

                aPlanCube.AddRule(BasePlanVariables.AccumSellThruSalesUnits, aQuantityVarProf, aInitSumFormula, null, null, null);
                aPlanCube.AddRule(BasePlanVariables.AccumSellThruStockUnits, aQuantityVarProf, aInitSumFormula, null, null, null);
                //aPlanCube.AddRule(BasePlanVariables.DaysInStock, aQuantityVarProf, aInitSumFormula, null, null, null);
                //aPlanCube.AddRule(BasePlanVariables.ReceivedStockDuringWeek, aQuantityVarProf, aInitSumFormula, null, null, null);
                //End TT#3482-MD -jsobek -In Stock Sales - negative oh hands are not treated as 0.  In a store multi level view the all store, set and High and Low Levels are not populating for the In stock variables.    

			}
			//Begin TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
			else
			{
				aPlanCube.AddRule(BasePlanVariables.WOSTotal, aQuantityVarProf, new InitCondition(aInitSumTotalFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.WOSRegPromo, aQuantityVarProf, new InitCondition(aInitSumRegularFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
			}
			//End TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)

			if (aQuantityVarProf.Key != BasePlanQuantityVariables.StoreAverage.Key && aQuantityVarProf.Key != BasePlanQuantityVariables.LowLevelAverage.Key)
			{
				aPlanCube.AddRule(BasePlanVariables.SalesStockRatioTotal, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesStockRatioTotal, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SalesStockRatioRegPromo, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesStockRatioRegPromo, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SalesStockRatioMarkdown, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesStockRatioMarkdown, null, BasePlanChangeMethods.Change_AutototalInitOnly);

				if (aPlanCube.isWeekDetailCube)
				{
					aPlanCube.AddRule(BasePlanVariables.ForwardWOSTotal, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_ForwardWOSTotal, null, BasePlanChangeMethods.Change_AutototalInitOnly);
					aPlanCube.AddRule(BasePlanVariables.ForwardWOSRegPromo, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_ForwardWOSRegPromo, null, BasePlanChangeMethods.Change_AutototalInitOnly);
					aPlanCube.AddRule(BasePlanVariables.ForwardWOSMarkdown, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_ForwardWOSMarkdown, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				}
				else
				{
					aPlanCube.AddRule(BasePlanVariables.ForwardWOSTotal, aQuantityVarProf, new InitCondition(aInitSumFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
					aPlanCube.AddRule(BasePlanVariables.ForwardWOSRegPromo, aQuantityVarProf, new InitCondition(aInitSumFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
					aPlanCube.AddRule(BasePlanVariables.ForwardWOSMarkdown, aQuantityVarProf, new InitCondition(aInitSumFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
				}

				aPlanCube.AddRule(BasePlanVariables.SellThruPctTotal, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SellThruPctTotal, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SellThruPctRegPromo, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SellThruPctRegPromo, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SellThruPctMarkdown, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SellThruPctMarkdown, null, BasePlanChangeMethods.Change_AutototalInitOnly);

				if (aPlanCube.isStoreCube)
				{
					if (!aPlanCube.isGroupTotalCube && !aPlanCube.isStoreTotalCube)
					{
						aPlanCube.AddRule(BasePlanVariables.SalesTotalSetIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesTotalSetIndex, BasePlanChangeMethods.Change_SalesTotalSetIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SalesRegularSetIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesRegularSetIndex, BasePlanChangeMethods.Change_SalesRegularSetIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SalesPromoSetIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesPromoSetIndex, BasePlanChangeMethods.Change_SalesPromoSetIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SalesRegPromoSetIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesRegPromoSetIndex, BasePlanChangeMethods.Change_SalesRegPromoSetIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SalesMarkdownSetIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesMarkdownSetIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.InventoryTotalSetIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryTotalSetIndex, BasePlanChangeMethods.Change_InventoryTotalSetIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.InventoryRegularSetIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryRegularSetIndex, BasePlanChangeMethods.Change_InventoryRegularSetIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownSetIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryMarkdownSetIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.ForwardWOSTotalSetIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_ForwardWOSTotalSetIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.ForwardWOSRegPromoSetIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_ForwardWOSRegPromoSetIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.ForwardWOSMarkdownSetIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_ForwardWOSMarkdownSetIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SellThruPctTotalSetIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SellThruPctTotalSetIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SellThruPctRegPromoSetIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SellThruPctRegPromoSetIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SellThruPctMarkdownSetIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SellThruPctMarkdownSetIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SalesTotalAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesTotalAllIndex, BasePlanChangeMethods.Change_SalesTotalAllIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SalesRegularAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesRegularAllIndex, BasePlanChangeMethods.Change_SalesRegularAllIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SalesPromoAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesPromoAllIndex, BasePlanChangeMethods.Change_SalesPromoAllIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SalesRegPromoAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesRegPromoAllIndex, BasePlanChangeMethods.Change_SalesRegPromoAllIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SalesMarkdownAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesMarkdownAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.InventoryTotalAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryTotalAllIndex, BasePlanChangeMethods.Change_InventoryTotalAllIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.InventoryRegularAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryRegularAllIndex, BasePlanChangeMethods.Change_InventoryRegularAllIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryMarkdownAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.ForwardWOSTotalAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_ForwardWOSTotalAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.ForwardWOSRegPromoAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_ForwardWOSRegPromoAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.ForwardWOSMarkdownAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_ForwardWOSMarkdownAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SellThruPctTotalAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SellThruPctTotalAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SellThruPctRegPromoAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SellThruPctRegPromoAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SellThruPctMarkdownAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SellThruPctMarkdownAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
					}

					if (aPlanCube.isGroupTotalCube)
					{
						aPlanCube.AddRule(BasePlanVariables.SalesTotalAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetSalesTotalAllIndex, BasePlanChangeMethods.Change_SalesTotalAllIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SalesRegularAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetSalesRegularAllIndex, BasePlanChangeMethods.Change_SalesRegularAllIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SalesPromoAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetSalesPromoAllIndex, BasePlanChangeMethods.Change_SalesPromoAllIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SalesRegPromoAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetSalesRegPromoAllIndex, BasePlanChangeMethods.Change_SalesRegPromoAllIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SalesMarkdownAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetSalesMarkdownAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.InventoryTotalAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetInventoryTotalAllIndex, BasePlanChangeMethods.Change_InventoryTotalAllIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.InventoryRegularAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetInventoryRegularAllIndex, BasePlanChangeMethods.Change_InventoryRegularAllIndex, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetInventoryMarkdownAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.ForwardWOSTotalAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_ForwardWOSTotalAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.ForwardWOSRegPromoAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_ForwardWOSRegPromoAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.ForwardWOSMarkdownAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_ForwardWOSMarkdownAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SellThruPctTotalAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SellThruPctTotalAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SellThruPctRegPromoAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SellThruPctRegPromoAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanVariables.SellThruPctMarkdownAllStoreIndex, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SellThruPctMarkdownAllIndex, null, BasePlanChangeMethods.Change_AutototalInitOnly);
					}
				}
			}
		}

		private void TimeDetail_StoreAverageRules(PlanCube aPlanCube, ChangeMethodProfile aPrimaryChangeMethod)
		{
			if (aPlanCube.isLowLevelTotalCube)
			{
				aPlanCube.AddRule(BasePlanVariables.SalesTotalUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreSalesTotalUnits_LowLevelTotal, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_SalesTotalUnitsAverage_LowLevelTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.SalesRegularUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreSalesRegularUnits_LowLevelTotal, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_SalesRegularUnitsAverage_LowLevelTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.SalesPromoUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreSalesRegularUnits_LowLevelTotal, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_SalesPromoUnitsAverage_LowLevelTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.SalesRegPromoUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreSalesRegularUnits_LowLevelTotal, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_SalesRegPromoUnitsAverage_LowLevelTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.SalesMarkdownUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreSalesMarkdownUnits_LowLevelTotal, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.InventoryTotalUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreInventoryTotalUnits_LowLevelTotal, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_InventoryTotalUnitsAverage_LowLevelTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.InventoryRegularUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreInventoryRegularUnits_LowLevelTotal, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_InventoryRegularUnitsAverage_LowLevelTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreInventoryMarkdownUnits_LowLevelTotal, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SalesStockRatioTotal, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_SalesStockRatioTotal, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SalesStockRatioRegPromo, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_SalesStockRatioRegPromo, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SalesStockRatioMarkdown, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_SalesStockRatioMarkdown, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.ReceiptTotalUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgDetail_LowLevelTotal, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.ReceiptRegularUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgDetail_LowLevelTotal, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.ReceiptMarkdownUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgDetail_LowLevelTotal, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.Intransit, BasePlanQuantityVariables.StoreAverage, BasePlanFormulasAndSpreads.Init_AvgDetail_LowLevelTotal, null, BasePlanChangeMethods.Change_AutototalInitOnly);
			}
			else
			{
				aPlanCube.AddRule(BasePlanVariables.SalesTotalUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreSalesTotalUnits, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_SalesTotalUnitsAverage, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.SalesRegularUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreSalesRegularUnits, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_SalesRegularUnitsAverage, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.SalesPromoUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreSalesRegularUnits, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_SalesPromoUnitsAverage, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.SalesRegPromoUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreSalesRegularUnits, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_SalesRegPromoUnitsAverage, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.SalesMarkdownUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreSalesMarkdownUnits, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.InventoryTotalUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreInventoryTotalUnits, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_InventoryTotalUnitsAverage, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.InventoryRegularUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreInventoryRegularUnits, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_InventoryRegularUnitsAverage, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreInventoryMarkdownUnits, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SalesStockRatioTotal, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_SalesStockRatioTotal, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SalesStockRatioRegPromo, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_SalesStockRatioRegPromo, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.SalesStockRatioMarkdown, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_SalesStockRatioMarkdown, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.ReceiptTotalUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.ReceiptRegularUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.ReceiptMarkdownUnits, BasePlanQuantityVariables.StoreAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
				aPlanCube.AddRule(BasePlanVariables.Intransit, BasePlanQuantityVariables.StoreAverage, BasePlanFormulasAndSpreads.Init_AvgDetail, null, BasePlanChangeMethods.Change_AutototalInitOnly);
			}
		}

		private void TimeDetail_ChainLowLevelBalanceRules(PlanCube aPlanCube)
		{
			aPlanCube.AddRule(BasePlanVariables.SalesTotalUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_ChainLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanVariables.SalesRegularUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_ChainLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanVariables.SalesPromoUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_ChainLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanVariables.SalesRegPromoUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_ChainLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanVariables.SalesMarkdownUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryTotalUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_ChainLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryRegularUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_ChainLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptTotalUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptRegularUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptMarkdownUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
		}

		private void TimeDetail_StoreLowLevelBalanceRules(PlanCube aPlanCube)
		{
			aPlanCube.AddRule(BasePlanVariables.SalesTotalUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_StoreLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanVariables.SalesRegularUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_StoreLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanVariables.SalesPromoUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_StoreLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanVariables.SalesRegPromoUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_StoreLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanVariables.SalesMarkdownUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryTotalUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_StoreLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryRegularUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_StoreLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptTotalUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptRegularUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptMarkdownUnits, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.Intransit, BasePlanQuantityVariables.Balance, BasePlanFormulasAndSpreads.Init_StoreBalance, null, null);
		}

		private void TimeDetail_ChainStoreDifferenceRules(PlanCube aPlanCube)
		{
			aPlanCube.AddRule(BasePlanVariables.SalesTotalUnits, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_ChainStoreDifference, null);
			aPlanCube.AddRule(BasePlanVariables.SalesRegularUnits, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_ChainStoreDifference, null);
			aPlanCube.AddRule(BasePlanVariables.SalesPromoUnits, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_ChainStoreDifference, null);
			aPlanCube.AddRule(BasePlanVariables.SalesRegPromoUnits, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_ChainStoreDifference, null);
			aPlanCube.AddRule(BasePlanVariables.SalesMarkdownUnits, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryTotalUnits, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_ChainStoreDifference, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryRegularUnits, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_ChainStoreDifference, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownUnits, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptTotalUnits, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptRegularUnits, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptMarkdownUnits, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
		}

		private void TimeDetail_LowLevelAverageRules(PlanCube aPlanCube)
		{
			aPlanCube.AddRule(BasePlanVariables.SalesTotalUnits, BasePlanQuantityVariables.LowLevelAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_LowLevelAverage, BasePlanChangeMethods.Change_AutototalSpreadLock);
			aPlanCube.AddRule(BasePlanVariables.SalesRegularUnits, BasePlanQuantityVariables.LowLevelAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_LowLevelAverage, BasePlanChangeMethods.Change_AutototalSpreadLock);
			aPlanCube.AddRule(BasePlanVariables.SalesPromoUnits, BasePlanQuantityVariables.LowLevelAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_LowLevelAverage, BasePlanChangeMethods.Change_AutototalSpreadLock);
			aPlanCube.AddRule(BasePlanVariables.SalesRegPromoUnits, BasePlanQuantityVariables.LowLevelAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_LowLevelAverage, BasePlanChangeMethods.Change_AutototalSpreadLock);
			aPlanCube.AddRule(BasePlanVariables.SalesMarkdownUnits, BasePlanQuantityVariables.LowLevelAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanVariables.InventoryTotalUnits, BasePlanQuantityVariables.LowLevelAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_LowLevelAverage, BasePlanChangeMethods.Change_AutototalSpreadLock);
			aPlanCube.AddRule(BasePlanVariables.InventoryRegularUnits, BasePlanQuantityVariables.LowLevelAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_LowLevelAverage, BasePlanChangeMethods.Change_AutototalSpreadLock);
			aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownUnits, BasePlanQuantityVariables.LowLevelAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanVariables.ReceiptTotalUnits, BasePlanQuantityVariables.LowLevelAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanVariables.ReceiptRegularUnits, BasePlanQuantityVariables.LowLevelAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanVariables.ReceiptMarkdownUnits, BasePlanQuantityVariables.LowLevelAverage, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);

			if (aPlanCube.isStoreCube)
			{
				aPlanCube.AddRule(BasePlanVariables.Intransit, BasePlanQuantityVariables.LowLevelAverage, BasePlanFormulasAndSpreads.Init_AvgDetail, null, BasePlanChangeMethods.Change_AutototalInitOnly);
			}
		}

		private void TimeDetail_PctChangeRules(PlanCube aPlanCube, QuantityVariableProfile aQuantityVar, FormulaProfile aInitFormula, BasePlanChangeMethods.PlanChangeMethodProfile aChangeMethod)
		{
			aPlanCube.AddRule(BasePlanVariables.SalesTotalUnits, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), aChangeMethod, null);
			aPlanCube.AddRule(BasePlanVariables.SalesRegularUnits, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aChangeMethod, null);
			aPlanCube.AddRule(BasePlanVariables.SalesPromoUnits, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aChangeMethod, null);
			aPlanCube.AddRule(BasePlanVariables.SalesRegPromoUnits, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aChangeMethod, null);
			aPlanCube.AddRule(BasePlanVariables.SalesMarkdownUnits, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryTotalUnits, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), aChangeMethod, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryRegularUnits, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aChangeMethod, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownUnits, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptTotalUnits, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptRegularUnits, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptMarkdownUnits, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.SalesStockRatioTotal, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.SalesStockRatioRegPromo, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.SalesStockRatioMarkdown, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ForwardWOSTotal, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ForwardWOSRegPromo, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ForwardWOSMarkdown, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.SellThruPctTotal, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.SellThruPctRegPromo, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.SellThruPctMarkdown, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);

			if (aPlanCube.isStoreCube)
			{
				if (!aPlanCube.isGroupTotalCube && !aPlanCube.isStoreTotalCube)
				{
					aPlanCube.AddRule(BasePlanVariables.SalesTotalSetIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), aChangeMethod, null);
					aPlanCube.AddRule(BasePlanVariables.SalesRegularSetIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aChangeMethod, null);
					aPlanCube.AddRule(BasePlanVariables.SalesPromoSetIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aChangeMethod, null);
					aPlanCube.AddRule(BasePlanVariables.SalesRegPromoSetIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aChangeMethod, null);
					aPlanCube.AddRule(BasePlanVariables.SalesMarkdownSetIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
					aPlanCube.AddRule(BasePlanVariables.InventoryTotalSetIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), aChangeMethod, null);
					aPlanCube.AddRule(BasePlanVariables.InventoryRegularSetIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aChangeMethod, null);
					aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownSetIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
					aPlanCube.AddRule(BasePlanVariables.SellThruPctTotalSetIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
					aPlanCube.AddRule(BasePlanVariables.SellThruPctRegPromoSetIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
					aPlanCube.AddRule(BasePlanVariables.SellThruPctMarkdownSetIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
				}

				if (!aPlanCube.isStoreTotalCube)
				{
					aPlanCube.AddRule(BasePlanVariables.SalesTotalAllStoreIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), aChangeMethod, null);
					aPlanCube.AddRule(BasePlanVariables.SalesRegularAllStoreIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aChangeMethod, null);
					aPlanCube.AddRule(BasePlanVariables.SalesPromoAllStoreIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aChangeMethod, null);
					aPlanCube.AddRule(BasePlanVariables.SalesRegPromoAllStoreIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aChangeMethod, null);
					aPlanCube.AddRule(BasePlanVariables.SalesMarkdownAllStoreIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
					aPlanCube.AddRule(BasePlanVariables.InventoryTotalAllStoreIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), aChangeMethod, null);
					aPlanCube.AddRule(BasePlanVariables.InventoryRegularAllStoreIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aChangeMethod, null);
					aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownAllStoreIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
					aPlanCube.AddRule(BasePlanVariables.SellThruPctTotalAllStoreIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
					aPlanCube.AddRule(BasePlanVariables.SellThruPctRegPromoAllStoreIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
					aPlanCube.AddRule(BasePlanVariables.SellThruPctMarkdownAllStoreIndex, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
				}
			}
		}

		private void TimeDetail_PctToLowLevelRules(PlanCube aPlanCube)
		{
			aPlanCube.AddRule(BasePlanVariables.SalesTotalUnits, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_PctToLowLevel, null);
			aPlanCube.AddRule(BasePlanVariables.SalesRegularUnits, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToLowLevel, null);
			aPlanCube.AddRule(BasePlanVariables.SalesPromoUnits, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToLowLevel, null);
			aPlanCube.AddRule(BasePlanVariables.SalesRegPromoUnits, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToLowLevel, null);
			aPlanCube.AddRule(BasePlanVariables.SalesMarkdownUnits, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryTotalUnits, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_PctToLowLevel, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryRegularUnits, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToLowLevel, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownUnits, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptTotalUnits, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptRegularUnits, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptMarkdownUnits, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);

			if (aPlanCube.isStoreCube)
			{
				aPlanCube.AddRule(BasePlanVariables.Intransit, BasePlanQuantityVariables.PctToLowLevelTotal, BasePlanFormulasAndSpreads.Init_PctToLowLevel, null, null);
			}
		}

		private void TimeDetail_PctToTimeTotalRules(PlanCube aPlanCube)
		{
			aPlanCube.AddRule(BasePlanVariables.SalesTotalUnits, BasePlanQuantityVariables.PctToTimePeriod, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToTimePeriod, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_PctToTimeTotal, null);
			aPlanCube.AddRule(BasePlanVariables.SalesRegularUnits, BasePlanQuantityVariables.PctToTimePeriod, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToTimePeriod, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToTimeTotal, null);
			aPlanCube.AddRule(BasePlanVariables.SalesPromoUnits, BasePlanQuantityVariables.PctToTimePeriod, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToTimePeriod, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToTimeTotal, null);
			aPlanCube.AddRule(BasePlanVariables.SalesRegPromoUnits, BasePlanQuantityVariables.PctToTimePeriod, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToTimePeriod, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToTimeTotal, null);
			aPlanCube.AddRule(BasePlanVariables.SalesMarkdownUnits, BasePlanQuantityVariables.PctToTimePeriod, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToTimePeriod, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryTotalUnits, BasePlanQuantityVariables.PctToTimePeriod, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToTimePeriod, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_PctToTimeTotal, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryRegularUnits, BasePlanQuantityVariables.PctToTimePeriod, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToTimePeriod, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToTimeTotal, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownUnits, BasePlanQuantityVariables.PctToTimePeriod, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToTimePeriod, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptTotalUnits, BasePlanQuantityVariables.PctToTimePeriod, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToTimePeriod, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptRegularUnits, BasePlanQuantityVariables.PctToTimePeriod, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToTimePeriod, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptMarkdownUnits, BasePlanQuantityVariables.PctToTimePeriod, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToTimePeriod, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
		}

		private void TimeDetail_PctToSetRules(PlanCube aPlanCube)
		{
			aPlanCube.AddRule(BasePlanVariables.SalesTotalUnits, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_PctToSet, null);
			aPlanCube.AddRule(BasePlanVariables.SalesRegularUnits, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToSet, null);
			aPlanCube.AddRule(BasePlanVariables.SalesPromoUnits, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToSet, null);
			aPlanCube.AddRule(BasePlanVariables.SalesRegPromoUnits, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToSet, null);
			aPlanCube.AddRule(BasePlanVariables.SalesMarkdownUnits, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryTotalUnits, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_PctToSet, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryRegularUnits, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToSet, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownUnits, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptTotalUnits, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptRegularUnits, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptMarkdownUnits, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
		}

		private void TimeDetail_PctToAllRules(PlanCube aPlanCube)
		{
			aPlanCube.AddRule(BasePlanVariables.SalesTotalUnits, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_PctToAll, null);
			aPlanCube.AddRule(BasePlanVariables.SalesRegularUnits, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToAll, null);
			aPlanCube.AddRule(BasePlanVariables.SalesPromoUnits, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToAll, null);
			aPlanCube.AddRule(BasePlanVariables.SalesRegPromoUnits, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToAll, null);
			aPlanCube.AddRule(BasePlanVariables.SalesMarkdownUnits, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryTotalUnits, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_PctToAll, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryRegularUnits, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToAll, null);
			aPlanCube.AddRule(BasePlanVariables.InventoryMarkdownUnits, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptTotalUnits, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptRegularUnits, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanVariables.ReceiptMarkdownUnits, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
		}

		private void TimeTotal_ValueRules(PlanCube aPlanCube, QuantityVariableProfile aQuantityVarProf)
		{
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT1, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_DateTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT2, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SalesUnitsTimeAvg, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_DateAverage, BasePlanChangeMethods.Change_AutototalSpreadLock);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularUnitsT1, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_DateTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoUnitsT1, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_DateTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT1, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT2, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SalesUnitsTimeAvg, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT1, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT2, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SalesUnitsTimeAvg, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT1, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_DateTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT2, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_InventoryUnitsTimeAvg, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_InventoryUnitsEnding, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT1, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_DateTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT2, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_InventoryUnitsTimeAvg, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_InventoryUnitsEnding, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownUnitsT1, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownUnitsT2, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_InventoryUnitsTimeAvg, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_InventoryUnitsEnding, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptTotalUnitsT1, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptRegularUnitsT1, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptMarkdownUnitsT1, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesStockRatioTotalT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesStockRatioTotalT1, null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesStockRatioRegPromoT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesStockRatioRegPromoT1, null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesStockRatioMarkdownT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesStockRatioMarkdownT1, null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ForwardWOSTotalT1, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ForwardWOSRegPromoT1, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ForwardWOSMarkdownT1, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SellThruPctTotalT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SellThruPctTotalT1, null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SellThruPctRegPromoT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SellThruPctRegPromoT1, null, BasePlanChangeMethods.Change_AutototalInitOnly);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SellThruPctMarkdownT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SellThruPctMarkdownT1, null, BasePlanChangeMethods.Change_AutototalInitOnly);


			if (aPlanCube.isStoreCube)
			{
				aPlanCube.AddRule(BasePlanTimeTotalVariables.IntransitT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SumDetail, null, BasePlanChangeMethods.Change_AutototalInitOnly);

				if (!aPlanCube.isGroupTotalCube && !aPlanCube.isStoreTotalCube && aQuantityVarProf.Key != BasePlanQuantityVariables.StoreAverage.Key)
				{
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalSetIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesTotalSetIndexT1, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularSetIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesRegularSetIndexT1, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoSetIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesPromoSetIndexT1, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoSetIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesRegPromoSetIndexT1, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownSetIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesMarkdownSetIndexT1, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalSetIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryTotalSetIndexT1, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalSetIndexT2, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryTotalSetIndexT2, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalSetIndexT3, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryTotalSetIndexT3, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularSetIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryRegularSetIndexT1, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularSetIndexT2, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryRegularSetIndexT2, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularSetIndexT3, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryRegularSetIndexT3, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownSetIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryMarkdownSetIndexT1, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownSetIndexT2, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryMarkdownSetIndexT2, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownSetIndexT3, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryMarkdownSetIndexT3, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.ForwardWOSTotalSetIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_ForwardWOSTotalSetIndexT1, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.ForwardWOSRegPromoSetIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_ForwardWOSRegPromoSetIndexT1, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.ForwardWOSMarkdownSetIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_ForwardWOSMarkdownSetIndexT1, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SellThruPctTotalSetIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SellThruPctTotalSetIndexT1, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SellThruPctRegPromoSetIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SellThruPctRegPromoSetIndexT1, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SellThruPctMarkdownSetIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SellThruPctMarkdownSetIndexT1, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.GradeTotalT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_GradeTotalT1, null, BasePlanChangeMethods.Change_AutototalInitOnly);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.GradeRegPromoT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_GradeRegPromoT1, null, BasePlanChangeMethods.Change_AutototalInitOnly);
				}

				if (!aPlanCube.isStoreTotalCube && aQuantityVarProf.Key != BasePlanQuantityVariables.StoreAverage.Key)
				{
					aPlanCube.AddRule(BasePlanTimeTotalVariables.ForwardWOSTotalAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_ForwardWOSTotalAllIndexT1, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.ForwardWOSRegPromoAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_ForwardWOSRegPromoAllIndexT1, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.ForwardWOSMarkdownAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_ForwardWOSMarkdownAllIndexT1, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SellThruPctTotalAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SellThruPctTotalAllIndexT1, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SellThruPctRegPromoAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SellThruPctRegPromoAllIndexT1, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SellThruPctMarkdownAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SellThruPctMarkdownAllIndexT1, null, null);

					if (!aPlanCube.isGroupTotalCube)
					{
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesTotalAllIndexT1, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesRegularAllIndexT1, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesPromoAllIndexT1, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesRegPromoAllIndexT1, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SalesMarkdownAllIndexT1, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryTotalAllIndexT1, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalAllStoreIndexT2, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryTotalAllIndexT2, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalAllStoreIndexT3, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryTotalAllIndexT3, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryRegularAllIndexT1, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularAllStoreIndexT2, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryRegularAllIndexT2, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularAllStoreIndexT3, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryRegularAllIndexT3, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryMarkdownAllIndexT1, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownAllStoreIndexT2, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryMarkdownAllIndexT2, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownAllStoreIndexT3, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_InventoryMarkdownAllIndexT3, null, null);
					}
					else
					{
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetSalesTotalAllIndexT1, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetSalesRegularAllIndexT1, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetSalesPromoAllIndexT1, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetSalesRegPromoAllIndexT1, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetSalesMarkdownAllIndexT1, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetInventoryTotalAllIndexT1, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalAllStoreIndexT2, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetInventoryTotalAllIndexT2, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalAllStoreIndexT3, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetInventoryTotalAllIndexT3, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetInventoryRegularAllIndexT1, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularAllStoreIndexT2, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetInventoryRegularAllIndexT2, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularAllStoreIndexT3, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetInventoryRegularAllIndexT3, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownAllStoreIndexT1, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetInventoryMarkdownAllIndexT1, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownAllStoreIndexT2, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetInventoryMarkdownAllIndexT2, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownAllStoreIndexT3, aQuantityVarProf, BasePlanFormulasAndSpreads.Init_SetInventoryMarkdownAllIndexT3, null, null);
					}
				}

				if (aPlanCube.isLowLevelTotalCube)
				{
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumLowLevelTotalDetail, BasePlanFormulasAndSpreads.Init_Null, TotalActualCurrentConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularUnitsT2, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumLowLevelRegularDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoUnitsT2, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumLowLevelRegularDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumLowLevelRegularDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumLowLevelRegularDetail, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualCurrentConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT4, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumLowLevelTotalDetail, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
                   	aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT5, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumLowLevelTotalDetail, BasePlanFormulasAndSpreads.Init_Null, TotalActualCurrentConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT6, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumLowLevelTotalDetail, BasePlanFormulasAndSpreads.Init_Null, TotalNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
                   	// Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
				    aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT8, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumLowLevelTotalDetail, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
                    // End TT#2054
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT4, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumLowLevelRegularDetail, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT5, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumLowLevelRegularDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT6, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumLowLevelRegularDetail, BasePlanFormulasAndSpreads.Init_Null, RegularNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
                  	// Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
				    aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT8, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumLowLevelRegularDetail, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
					// End TT#2054
					if (aPlanCube.isGroupTotalCube || aPlanCube.isStoreTotalCube)
					{
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT7, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumLowLevelStoreInventoryTotalUnitsT7, BasePlanFormulasAndSpreads.Init_Null, TotalNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT7, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumLowLevelStoreInventoryRegularUnitsT7, BasePlanFormulasAndSpreads.Init_Null, RegularNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
					}
					else
					{
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT7, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumLowLevelInventoryTotalUnitsT7, BasePlanFormulasAndSpreads.Init_Null, TotalNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT7, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumLowLevelInventoryRegularUnitsT7, BasePlanFormulasAndSpreads.Init_Null, RegularNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
					}
				}
				else if (aPlanCube.isGroupTotalCube || aPlanCube.isStoreTotalCube)
				{
					if (aQuantityVarProf.Key == BasePlanQuantityVariables.StoreAverage.Key)
					{
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreSalesTotalUnitsT3, BasePlanFormulasAndSpreads.Init_Null, TotalActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularUnitsT2, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreSalesRegularUnitsT2, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoUnitsT2, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreSalesPromoUnitsT2, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreSalesRegPromoUnitsT3, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreSalesMarkdownUnitsT3, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT4, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreInventoryTotalUnitsT4, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
                		aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT5, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreInventoryTotalUnitsT5, BasePlanFormulasAndSpreads.Init_Null, TotalActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT6, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreInventoryTotalUnitsT6, BasePlanFormulasAndSpreads.Init_Null, TotalNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
                        // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT8, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreInventoryTotalUnitsT8, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
                        // End TT#2054
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT4, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreInventoryRegularUnitsT4, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
                       	aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT5, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreInventoryRegularUnitsT5, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT6, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreInventoryRegularUnitsT6, BasePlanFormulasAndSpreads.Init_Null, RegularNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
                       	// Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals 
					    aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT8, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgStoreInventoryRegularUnitsT8, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
                  		// End TT#2054
				    }
					else if (aQuantityVarProf.Key == BasePlanQuantityVariables.Comp.Key)
					{
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumCompStoreSalesTotalUnitsT3, BasePlanFormulasAndSpreads.Init_Null, TotalActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularUnitsT2, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumCompStoreSalesRegularUnitsT2, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoUnitsT2, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumCompStoreSalesPromoUnitsT2, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumCompStoreSalesRegPromoUnitsT3, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumCompStoreSalesMarkdownUnitsT3, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT4, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumCompStoreInventoryTotalUnitsT4, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
                        aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT5, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumCompStoreInventoryTotalUnitsT5, BasePlanFormulasAndSpreads.Init_Null, TotalActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT6, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumCompStoreInventoryTotalUnitsT6, BasePlanFormulasAndSpreads.Init_Null, TotalNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT7, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumCompStoreInventoryTotalUnitsT7, BasePlanFormulasAndSpreads.Init_Null, TotalNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
                        // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
					    aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT8, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumCompStoreInventoryTotalUnitsT8, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
                        // End TT#2054
					    aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT4, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumCompStoreInventoryRegularUnitsT4, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
                       	aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT5, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumCompStoreInventoryRegularUnitsT5, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT6, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumCompStoreInventoryRegularUnitsT6, BasePlanFormulasAndSpreads.Init_Null, RegularNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT7, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumCompStoreInventoryRegularUnitsT7, BasePlanFormulasAndSpreads.Init_Null, RegularNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
                        // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT8, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumCompStoreInventoryRegularUnitsT8, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
                   		// End TT#2054
				    }
					else if (aQuantityVarProf.Key == BasePlanQuantityVariables.NonComp.Key)
					{
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNonCompStoreSalesTotalUnitsT3, BasePlanFormulasAndSpreads.Init_Null, TotalActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularUnitsT2, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNonCompStoreSalesRegularUnitsT2, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoUnitsT2, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNonCompStoreSalesPromoUnitsT2, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNonCompStoreSalesRegPromoUnitsT3, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNonCompStoreSalesMarkdownUnitsT3, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT4, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNonCompStoreInventoryTotalUnitsT4, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT5, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNonCompStoreInventoryTotalUnitsT5, BasePlanFormulasAndSpreads.Init_Null, TotalActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT6, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNonCompStoreInventoryTotalUnitsT6, BasePlanFormulasAndSpreads.Init_Null, TotalNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT7, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNonCompStoreInventoryTotalUnitsT7, BasePlanFormulasAndSpreads.Init_Null, TotalNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
                        // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
					    aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT8, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNonCompStoreInventoryTotalUnitsT8, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
                        // End TT#2054
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT4, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNonCompStoreInventoryRegularUnitsT4, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT5, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNonCompStoreInventoryRegularUnitsT5, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT6, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNonCompStoreInventoryRegularUnitsT6, BasePlanFormulasAndSpreads.Init_Null, RegularNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT7, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNonCompStoreInventoryRegularUnitsT7, BasePlanFormulasAndSpreads.Init_Null, RegularNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
                        // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT8, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNonCompStoreInventoryRegularUnitsT8, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
               			// End TT#2054
			   	    }
					else if (aQuantityVarProf.Key == BasePlanQuantityVariables.New.Key)
					{
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNewStoreSalesTotalUnitsT3, BasePlanFormulasAndSpreads.Init_Null, TotalActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularUnitsT2, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNewStoreSalesRegularUnitsT2, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoUnitsT2, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNewStoreSalesPromoUnitsT2, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNewStoreSalesRegPromoUnitsT3, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNewStoreSalesMarkdownUnitsT3, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT4, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNewStoreInventoryTotalUnitsT4, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT5, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNewStoreInventoryTotalUnitsT5, BasePlanFormulasAndSpreads.Init_Null, TotalActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT6, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNewStoreInventoryTotalUnitsT6, BasePlanFormulasAndSpreads.Init_Null, TotalNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT7, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNewStoreInventoryTotalUnitsT7, BasePlanFormulasAndSpreads.Init_Null, TotalNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
                        // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT8, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNewStoreInventoryTotalUnitsT8, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
                        // End TT#2054
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT4, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNewStoreInventoryRegularUnitsT4, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT5, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNewStoreInventoryRegularUnitsT5, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT6, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNewStoreInventoryRegularUnitsT6, BasePlanFormulasAndSpreads.Init_Null, RegularNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT7, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNewStoreInventoryRegularUnitsT7, BasePlanFormulasAndSpreads.Init_Null, RegularNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
                        // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
					    aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT8, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumNewStoreInventoryRegularUnitsT8, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
                  		// End TT#2054
				    }
					else
					{
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumStoreSalesTotalUnitsT3, BasePlanFormulasAndSpreads.Init_Null, TotalActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularUnitsT2, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumStoreSalesRegularUnitsT2, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoUnitsT2, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumStoreSalesPromoUnitsT2, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumStoreSalesRegPromoUnitsT3, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumStoreSalesMarkdownUnitsT3, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT4, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumStoreInventoryTotalUnitsT4, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT5, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumStoreInventoryTotalUnitsT5, BasePlanFormulasAndSpreads.Init_Null, TotalActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT6, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumStoreInventoryTotalUnitsT6, BasePlanFormulasAndSpreads.Init_Null, TotalNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT7, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumStoreInventoryTotalUnitsT7, BasePlanFormulasAndSpreads.Init_Null, TotalNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
                        // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
					    aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT8, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumStoreInventoryTotalUnitsT8, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
                        // End TT#2054
					    aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT4, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumStoreInventoryRegularUnitsT4, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT5, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumStoreInventoryRegularUnitsT5, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null, null);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT6, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumStoreInventoryRegularUnitsT6, BasePlanFormulasAndSpreads.Init_Null, RegularNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT7, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumStoreInventoryRegularUnitsT7, BasePlanFormulasAndSpreads.Init_Null, RegularNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
                        // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
						aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT8, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SumStoreInventoryRegularUnitsT8, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
                  		// End TT#2054
				    }
				}
				else
				{
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SalesTotalUnitsT3, BasePlanFormulasAndSpreads.Init_Null, new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isCurrentPostingWeekInPlan))), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularUnitsT2, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SalesRegularUnitsT2, BasePlanFormulasAndSpreads.Init_Null, new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isCurrentPostingWeekInPlan))), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoUnitsT2, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SalesPromoUnitsT2, BasePlanFormulasAndSpreads.Init_Null, new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isCurrentPostingWeekInPlan))), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SalesRegPromoUnitsT3, BasePlanFormulasAndSpreads.Init_Null, new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isCurrentPostingWeekInPlan))), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT3, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_SalesMarkdownUnitsT3, BasePlanFormulasAndSpreads.Init_Null, new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isCurrentPostingWeekInPlan))), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT4, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_InventoryTotalUnitsT4, BasePlanFormulasAndSpreads.Init_Null, new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isCurrentPostingWeekInPlan))), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT5, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_InventoryTotalUnitsT5, BasePlanFormulasAndSpreads.Init_Null, new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isCurrentPostingWeekInPlan))), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT6, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_InventoryTotalUnitsT6, BasePlanFormulasAndSpreads.Init_Null, TotalNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT7, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_InventoryTotalUnitsT7, BasePlanFormulasAndSpreads.Init_Null, TotalNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
                   	// Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
				    aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT8, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_InventoryTotalUnitsT8, BasePlanFormulasAndSpreads.Init_Null, new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isCurrentPostingWeekInPlan))), null, null, null);
                    // End TT#2054
				    aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT4, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_InventoryRegularUnitsT4, BasePlanFormulasAndSpreads.Init_Null, new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isCurrentPostingWeekInPlan))), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT5, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_InventoryRegularUnitsT5, BasePlanFormulasAndSpreads.Init_Null, new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isCurrentPostingWeekInPlan))), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT6, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_InventoryRegularUnitsT6, BasePlanFormulasAndSpreads.Init_Null, RegularNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT7, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_InventoryRegularUnitsT7, BasePlanFormulasAndSpreads.Init_Null, RegularNeedConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
                    // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
				    aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT8, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_InventoryRegularUnitsT8, BasePlanFormulasAndSpreads.Init_Null, new InitConditionOperand(new InitConditionDelegate(BasePlanToolBox.isCurrentPostingWeekInPlan))), null, null, null);
            		// End TT#2054
			    }
			}
			//Begin TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
			if (aQuantityVarProf.Key != BasePlanQuantityVariables.LowLevelAverage.Key)
			{
				aPlanCube.AddRule(BasePlanTimeTotalVariables.WOSTotalT1, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_DateTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
				aPlanCube.AddRule(BasePlanTimeTotalVariables.WOSRegPromoT1, aQuantityVarProf, new InitCondition(BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_DateTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
			}
			//End TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
		}

		private void TimeTotal_ChainLowLevelBalanceRules(PlanCube aPlanCube)
		{
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_ChainLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_ChainLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_ChainLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_ChainLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_ChainLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_ChainLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptTotalUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptRegularUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptMarkdownUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
		}

		private void TimeTotal_StoreLowLevelBalanceRules(PlanCube aPlanCube)
		{
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_ChainLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_ChainLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_ChainLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_ChainLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_ChainLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT4, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT5, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, TotalActualCurrentConditions), null, null);
            // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT8, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null);
            // End TT#2054
		    aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_ChainLowLevelBalance, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT4, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT5, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, RegularActualCurrentConditions), null, null);
            // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
		    aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT8, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null);
            // End TTQ@054
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptTotalUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptRegularUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptMarkdownUnitsT1, BasePlanQuantityVariables.Balance, new InitCondition(BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.IntransitT1, BasePlanQuantityVariables.Balance, BasePlanFormulasAndSpreads.Init_StoreBalance, null, null);
		}

		private void TimeTotal_ChainStoreDifferenceRules(PlanCube aPlanCube)
		{
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT1, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_ChainStoreDifference, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularUnitsT1, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_ChainStoreDifference, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoUnitsT1, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_ChainStoreDifference, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT1, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_ChainStoreDifference, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT1, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT1, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_ChainStoreDifference, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT1, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_ChainStoreDifference, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownUnitsT1, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptTotalUnitsT1, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptRegularUnitsT1, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptMarkdownUnitsT1, BasePlanQuantityVariables.Difference, new InitCondition(BasePlanFormulasAndSpreads.Init_Difference, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
		}

		private void TimeTotal_PctChangeRules(PlanCube aPlanCube, QuantityVariableProfile aQuantityVar, FormulaProfile aInitFormula, BasePlanChangeMethods.PlanChangeMethodProfile aChangeMethod)
		{
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), aChangeMethod, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT2, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), aChangeMethod, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularUnitsT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aChangeMethod, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoUnitsT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aChangeMethod, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT2, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT2, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), aChangeMethod, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT2, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT3, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), aChangeMethod, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT2, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT3, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownUnitsT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownUnitsT2, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownUnitsT3, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptTotalUnitsT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptRegularUnitsT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptMarkdownUnitsT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesStockRatioTotalT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesStockRatioRegPromoT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesStockRatioMarkdownT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ForwardWOSTotalT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ForwardWOSRegPromoT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ForwardWOSMarkdownT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SellThruPctTotalT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SellThruPctRegPromoT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SellThruPctMarkdownT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);

			if (aPlanCube.isStoreCube)
			{
				aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT3, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
				aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularUnitsT2, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
				aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoUnitsT2, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
				aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT3, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
				aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT3, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
				aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT4, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
				aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT5, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
				aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT6, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
                // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
			    aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT8, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
                // End TT#2054 
				aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT4, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
				aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT5, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
				aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT6, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, BasePlanChangeMethods.Change_AutototalInitOnly);
                // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
			    aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT8, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null, null);
				// End TT#2054
				if (!aPlanCube.isGroupTotalCube && !aPlanCube.isStoreTotalCube)
				{
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalSetIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularSetIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoSetIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoSetIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownSetIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalSetIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalSetIndexT2, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalSetIndexT3, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularSetIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularSetIndexT2, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularSetIndexT3, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownSetIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownSetIndexT2, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownSetIndexT3, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SellThruPctTotalSetIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SellThruPctRegPromoSetIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SellThruPctMarkdownSetIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
				}

				if (!aPlanCube.isStoreTotalCube)
				{
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalAllStoreIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularAllStoreIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoAllStoreIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoAllStoreIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownAllStoreIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalAllStoreIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalAllStoreIndexT2, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalAllStoreIndexT3, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularAllStoreIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularAllStoreIndexT2, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularAllStoreIndexT3, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownAllStoreIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownAllStoreIndexT2, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownAllStoreIndexT3, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SellThruPctTotalAllStoreIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SellThruPctRegPromoAllStoreIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null, null);
					aPlanCube.AddRule(BasePlanTimeTotalVariables.SellThruPctMarkdownAllStoreIndexT1, aQuantityVar, new InitCondition(aInitFormula, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null, null);
				}
			}
		}

		private void TimeTotal_PctToLowLevelRules(PlanCube aPlanCube)
		{
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT1, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_PctToLowLevel, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT2, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_PctToLowLevel, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT3, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularUnitsT1, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToLowLevel, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularUnitsT2, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoUnitsT1, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToLowLevel, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoUnitsT2, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT1, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT2, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT3, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT1, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT2, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT3, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT1, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_PctToLowLevel, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT2, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT3, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT4, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT5, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT6, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
            // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
		    aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT8, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null);
            // End TT#2054
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT1, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToLowLevel, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT2, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT3, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT4, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT5, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT6, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
            // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT8, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null);
            // End TT#2054
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownUnitsT1, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownUnitsT2, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownUnitsT3, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptTotalUnitsT1, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptRegularUnitsT1, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptMarkdownUnitsT1, BasePlanQuantityVariables.PctToLowLevelTotal, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);

			if (aPlanCube.isStoreCube)
			{
				aPlanCube.AddRule(BasePlanTimeTotalVariables.IntransitT1, BasePlanQuantityVariables.PctToLowLevelTotal, BasePlanFormulasAndSpreads.Init_PctToLowLevel, null, null);
			}
		}

		private void TimeTotal_PctToSetRules(PlanCube aPlanCube)
		{
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT1, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_PctToSet, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT2, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_PctToSet, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT3, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularUnitsT1, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToSet, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularUnitsT2, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoUnitsT1, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToSet, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoUnitsT2, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT1, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT2, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT3, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT1, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT2, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT3, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT1, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_PctToSet, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT2, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT3, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT4, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT5, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT6, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
            // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
		    aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT8, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null);
            // End TT#2054
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT1, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToSet, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT2, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT3, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT4, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT5, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT6, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
            // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
		    aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT8, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null);
            // End TT#2054  
		    aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownUnitsT1, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownUnitsT2, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownUnitsT3, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptTotalUnitsT1, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptRegularUnitsT1, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptMarkdownUnitsT1, BasePlanQuantityVariables.PctToSet, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
		}

		private void TimeTotal_PctToAllRules(PlanCube aPlanCube)
		{
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT1, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_PctToAll, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT2, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_PctToAll, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesTotalUnitsT3, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularUnitsT1, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToAll, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegularUnitsT2, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoUnitsT1, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToAll, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesPromoUnitsT2, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT1, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT2, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesRegPromoUnitsT3, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT1, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT2, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.SalesMarkdownUnitsT3, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT1, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), BasePlanChangeMethods.Change_PctToAll, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT2, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT3, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT4, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT5, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT6, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
            // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals			
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryTotalUnitsT8, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null);
            // End TT#2054
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT1, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), BasePlanChangeMethods.Change_PctToAll, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT2, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT3, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT4, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT5, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT6, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
            // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
		    aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryRegularUnitsT8, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, ActualCurrentConditions), null, null);
            // End TT#2054
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownUnitsT1, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownUnitsT2, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.InventoryMarkdownUnitsT3, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptTotalUnitsT1, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, TotalActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptRegularUnitsT1, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, RegularActualConditions), null, null);
			aPlanCube.AddRule(BasePlanTimeTotalVariables.ReceiptMarkdownUnitsT1, BasePlanQuantityVariables.PctToAllStore, new InitCondition(BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanFormulasAndSpreads.Init_Null, MarkdownActualConditions), null, null);
		}

		#endregion
	}
}
