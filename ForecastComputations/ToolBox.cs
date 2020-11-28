using System;
using System.Collections;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;

namespace MIDRetail.ForecastComputations
{
	/// <summary>
	/// The Tools class contains routines that are used by the formulas, spreads, initializations, and change rules.
	/// </summary>

	abstract public class BasePlanToolBox : BaseToolBox
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public BasePlanToolBox(BaseComputations aBaseComputations)
			: base(aBaseComputations)
		{
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// This routine allows the Client computations to override the Regular/Total condition, and display variables regardless
		/// of the Node Type.
		/// </summary>
		/// <returns>
		/// Return false.  Can be overridden to true to 
		/// </returns>

		virtual protected bool IgnoreRegularTotalCondition
		{
			get
			{
				return false;
			}
		}

		protected BasePlanQuantityVariables BasePlanQuantityVariables
		{
			get
			{
				try
				{
					return ((BasePlanComputations)_computations).BasePlanQuantityVariables;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

        /// <summary>
        /// Gets the BasePlanVariables
        /// </summary>

        protected BasePlanVariables BasePlanVariables
        {
            get
            {
               return ((BasePlanComputations)_computations).BasePlanVariables;
            }
        }

        /// <summary>
        /// Gets the BasePlanTimeTotalVariables
        /// </summary>

        protected BasePlanTimeTotalVariables BasePlanTimeTotalVariables
        {
            get
            {
                return ((BasePlanComputations)_computations).BasePlanTimeTotalVariables;
            }
        }

		//========
		// METHODS
		//========

		/// <summary>
		/// Returns a boolean indicating if the given cell is contained in a Store cube.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the cell is in a Store cube.
		/// </returns>

		public bool isStore(ComputationCellReference aCompCellRef)
		{
			try
			{
				return (((PlanCellReference)aCompCellRef).PlanCube.isStoreCube);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the given cell is contained in a Chain cube.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the cell is in a Chain cube.
		/// </returns>

		public bool isChain(ComputationCellReference aCompCellRef)
		{
			try
			{
				return (((PlanCellReference)aCompCellRef).PlanCube.isChainCube);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the given cell is contained in a Total cube.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the cell is in a Total cube.
		/// </returns>
		
		public bool isTotalCube(ComputationCellReference aCompCellRef)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aCompCellRef;

				return (planCellRef.PlanCube.isDateTotalCube || planCellRef.PlanCube.isGroupTotalCube ||
					planCellRef.PlanCube.isLowLevelTotalCube || planCellRef.PlanCube.isPeriodDetailCube ||
					planCellRef.PlanCube.isStoreTotalCube);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell belongs to a forecast version.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the cell belongs to a forecast version.
		/// </returns>

		public bool isForecast(ComputationCellReference aCompCellRef)
		{
			int key;

			try
			{
				key = ((PlanCellReference)aCompCellRef).GetVersionProfileOfData().Key;
				return (key != Include.FV_ActualRID && key != Include.FV_ModifiedRID);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell belongs to a Action version.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the cell belongs to a Action version.
		/// </returns>

		public bool isAction(ComputationCellReference aCompCellRef)
		{
			try
			{
				return (((PlanCellReference)aCompCellRef).GetVersionProfileOfData().Key == Include.FV_ActionRID);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell belongs to an actual version.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the cell belongs to an actual version.
		/// </returns>

		public bool isActual(ComputationCellReference aCompCellRef)
		{
			int key;

			try
			{
				key = ((PlanCellReference)aCompCellRef).GetVersionProfileOfData().Key;
				return (key == Include.FV_ActualRID || key == Include.FV_ModifiedRID);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell belongs to an actual version.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the cell belongs to an actual version.
		/// </returns>

		public bool isModified(ComputationCellReference aCompCellRef)
		{
			int key;

			try
			{
				key = ((PlanCellReference)aCompCellRef).GetVersionProfileOfData().Key;
				return (key == Include.FV_ModifiedRID);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell belongs to a plan type.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the cell belongs to a plan type.
		/// </returns>

		public bool isPlan(ComputationCellReference aCompCellRef)
		{
			try
			{
				return (((PlanCellReference)aCompCellRef).PlanCube.isPlanCube);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell belongs to a basis type.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the cell belongs to an basis type.
		/// </returns>

		public bool isBasis(ComputationCellReference aCompCellRef)
		{
			try
			{
				return (((PlanCellReference)aCompCellRef).PlanCube.isBasisCube);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell belongs to a basis type.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the cell belongs to an basis type.
		/// </returns>

		public bool isSimpleBasis(ComputationCellReference aCompCellRef)
		{
			try
			{
				if (isBasis(aCompCellRef))
				{
					return ((PlanBasisCube)((PlanCellReference)aCompCellRef).PlanCube).isSimpleBasis((PlanCellReference)aCompCellRef);
				}
				else
				{
					return false;
				}
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell belongs to a regular plan level.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the cell belongs to a regular plan leve.
		/// </returns>

		public bool isRegularPlanType(ComputationCellReference aCompCellRef)
		{
			try
			{
				return IgnoreRegularTotalCondition || (((PlanCellReference)aCompCellRef).GetHierarchyNodeProfile().OTSPlanLevelType == eOTSPlanLevelType.Regular);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell belongs to a total plan level.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the cell belongs to a total plan leve.
		/// </returns>

		public bool isTotalPlanType(ComputationCellReference aCompCellRef)
		{
			try
			{
				return IgnoreRegularTotalCondition || (((PlanCellReference)aCompCellRef).GetHierarchyNodeProfile().OTSPlanLevelType == eOTSPlanLevelType.Total);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell is a Value quantity variable.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The ComputationCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating if the cell is a Value quantity variable.
		/// </returns>

		public bool isValueVariable(PlanCellReference aPlanCellRef)
		{
			try
			{
				return aPlanCellRef[eProfileType.QuantityVariable] == BasePlanQuantityVariables.ValueQuantity.Key;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell store's status is Comp.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the cell store's status is Comp.
		/// </returns>

		public bool isComp(PlanCellReference aPlanCellRef)
		{
			try
			{
				return (aPlanCellRef.GetStoreStatus() == eStoreStatus.Comp);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell store's status is NonComp.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the cell store's status is NonComp.
		/// </returns>

		public bool isNonComp(PlanCellReference aPlanCellRef)
		{
			try
			{
				return (aPlanCellRef.GetStoreStatus() == eStoreStatus.NonComp);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell store's status is New.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the cell store's status is New.
		/// </returns>

		public bool isNew(PlanCellReference aPlanCellRef)
		{
			try
			{
				return (aPlanCellRef.GetStoreStatus() == eStoreStatus.New);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell time detail is the first time detail.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the cell's time detail is the first time detail.
		/// </returns>

		public bool isFirstTimeDetail(PlanCellReference aPlanCellRef)
		{
			try
			{
				return isFirstTimeDetail(aPlanCellRef, 0);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell time detail is the first time detail.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the cell to test.
		/// </param>
		/// <param name="aOffset">
		/// The number of months to offset the first time by.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the cell's time detail is the first time detail.
		/// </returns>

		public bool isFirstTimeDetail(PlanCellReference aPlanCellRef, int aOffset)
		{
			try
			{
				return (aPlanCellRef[aPlanCellRef.PlanCube.GetTimeType()] == GetBeginPlanTimeDetail(aPlanCellRef, aOffset));
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell time detail is the last time detail.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the cell's time detail is the last time detail.
		/// </returns>

		public bool isLastTimeDetail(PlanCellReference aPlanCellRef)
		{
			try
			{
				return isLastTimeDetail(aPlanCellRef, 0);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell time detail is the last time detail.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the cell to test.
		/// </param>
		/// <param name="aOffset">
		/// The number of months to offset the last time by.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the cell's time detail is the last time detail.
		/// </returns>

		public bool isLastTimeDetail(PlanCellReference aPlanCellRef, int aOffset)
		{
			try
			{
				return (aPlanCellRef[aPlanCellRef.PlanCube.GetTimeType()] == GetEndPlanTimeDetail(aPlanCellRef, aOffset));
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell time detail is the first time detail.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the cell's time detail is the first time detail.
		/// </returns>

		public bool isFirstWeekInPeriod(PlanCellReference aPlanCellRef)
		{
			WeekProfile weekProf;
			ProfileList timeProfList;

			try
			{
				if (aPlanCellRef.PlanCube.GetTimeType() == eProfileType.Week)
				{
					timeProfList = aPlanCellRef.GetTimeDetailProfileList();
					weekProf = (WeekProfile)timeProfList.FindKey(aPlanCellRef[eProfileType.Week]);

					if (weekProf != null)
					{
						return (weekProf.WeekInPeriod == 1);
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
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell time detail is the first time detail.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the cell's time detail is the first time detail.
		/// </returns>

		public bool isLastWeekInPeriod(PlanCellReference aPlanCellRef)
		{
			WeekProfile weekProf;
			ProfileList timeProfList;

			try
			{
				if (aPlanCellRef.PlanCube.GetTimeType() == eProfileType.Week)
				{
					timeProfList = aPlanCellRef.GetTimeDetailProfileList();
					weekProf = (WeekProfile)timeProfList.FindKey(aPlanCellRef[eProfileType.Week]);

					if (weekProf != null)
					{
						return (weekProf.WeekInPeriod == weekProf.Period.NoOfWeeks);	// Issue 5121
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
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the current posting week is contained in the time line.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating if the current posting week is contained in the time line.
		/// </returns>

		public bool isCurrentPostingWeekInTimeLine(PlanCellReference aPlanCellRef)
		{
			try
			{
				return aPlanCellRef.ContainsCurrentWeek();
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the current posting week is contained in the plan.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference of the cell to test.
		/// </param>
		/// <returns>
		/// A boolean indicating if the current posting week is contained in the plan.
		/// </returns>

		public bool isCurrentPostingWeekInPlan(ComputationCellReference aCompCellRef)
		{
			WeekProfile currWeek;
			ProfileList weekProfList;

			try
			{
				currWeek = ((PlanCellReference)aCompCellRef).PlanCube.SAB.ApplicationServerSession.Calendar.CurrentWeek;
				weekProfList = ((PlanCellReference)aCompCellRef).PlanCube.PlanCubeGroup.OpenParms.WeekProfileList;

				if (currWeek.Key >= weekProfList[0].Key && currWeek.Key <= weekProfList[weekProfList.Count - 1].Key)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns the current time of the given PlanCellReference.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to get the current time from.
		/// </param>
		/// <returns>
		/// The current time.
		/// </returns>

		public int GetCurrentPlanTimeDetail(PlanCellReference aPlanCellRef)
		{
			try
			{
				return GetCurrentPlanTimeDetail(aPlanCellRef, 0);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns the current time of the given PlanCellReference, offsetting it by the number of months specified by the given integer value.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to get the current time from.
		/// </param>
		/// <param name="aOffset">
		/// The number of months to offset the current time by.
		/// </param>
		/// <returns>
		/// The current time.
		/// </returns>

		public int GetCurrentPlanTimeDetail(PlanCellReference aPlanCellRef, int aOffset)
		{
			try
			{
				return aPlanCellRef.PlanCube.IncrementTimeKey(aPlanCellRef, aOffset);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns the beginning time of the given PlanCellReference.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to get the beginning time from.
		/// </param>
		/// <returns>
		/// The beginning time.
		/// </returns>

		public int GetBeginPlanTimeDetail(PlanCellReference aPlanCellRef)
		{
			try
			{
				return GetBeginPlanTimeDetail(aPlanCellRef, 0);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns the beginning time of the given PlanCellReference, offsetting it by the number of months specified by the given integer value.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to get the beginning time from.
		/// </param>
		/// <param name="aOffset">
		/// The number of months to offset the beginning time by.
		/// </param>
		/// <returns>
		/// The beginning time.
		/// </returns>

		public int GetBeginPlanTimeDetail(PlanCellReference aPlanCellRef, int aOffset)
		{
			ProfileList timeProfList;

			try
			{
				timeProfList = aPlanCellRef.GetTimeDetailProfileList();
				return aPlanCellRef.PlanCube.IncrementTimeKey(timeProfList[0].Key, aOffset);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns the end time of the given PlanCellReference.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to get the end time from.
		/// </param>
		/// <returns>
		/// The end time.
		/// </returns>

		public int GetEndPlanTimeDetail(PlanCellReference aPlanCellRef)
		{
			try
			{
				return GetEndPlanTimeDetail(aPlanCellRef, 0);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns the end time of the given PlanCellReference, offsetting it by the number of months specified by the given integer value.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to get the end time from.
		/// </param>
		/// <param name="aOffset">
		/// The number of months to offset the end time by.
		/// </param>
		/// <returns>
		/// The end time.
		/// </returns>

		public int GetEndPlanTimeDetail(PlanCellReference aPlanCellRef, int aOffset)
		{
			ProfileList timeProfList;

			try
			{
				timeProfList = aPlanCellRef.GetTimeDetailProfileList();
				return aPlanCellRef.PlanCube.IncrementTimeKey(timeProfList[timeProfList.Count - 1].Key, aOffset);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Inserts a Forumla into the schedule queue for the given ComputationCellReference, FormulaSpreadProfile, 
		/// VariableProfile, and time id.
		/// </summary>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Formula in.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the cell being computed.
		/// </param>
		/// <param name="aFormula">
		/// The FormulaSpreadProfile of the forumla to execute.
		/// </param>
		/// <param name="aVariableProfile">
		/// The Id of this VariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>
		/// <param name="aTimeId">
		/// Indicates the time Id to schedule.
		/// </param>

		public void InsertFormula(
			ComputationSchedule aCompSchd,
			PlanCellReference aPlanCellRef,
			FormulaProfile aFormula,
			VariableProfile aVariableProfile,
			int aTimeId)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Copy();
				planCellRef[eProfileType.Variable] = aVariableProfile.Key;
				planCellRef[planCellRef.PlanCube.GetTimeType()] = aTimeId;

				aCompSchd.InsertFormula(aPlanCellRef, planCellRef, aFormula);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Inserts a Forumla into the schedule queue for the given ComputationCellReference, FormulaSpreadProfile, 
		/// VariableProfile, and time range.
		/// </summary>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Formula in.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the cell being computed.
		/// </param>
		/// <param name="aFormula">
		/// The FormulaSpreadProfile of the forumla to execute.
		/// </param>
		/// <param name="aVariableProfile">
		/// The Id of this VariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>
		/// <param name="aBeginTimeId">
		/// Indicates the beginning time Id of the time range.
		/// </param>
		/// <param name="aEndTimeId">
		/// Indicates the ending time Id of the time range.
		/// </param>

		public void InsertFormula(
			ComputationSchedule aCompSchd,
			PlanCellReference aPlanCellRef,
			FormulaProfile aFormula,
			VariableProfile aVariableProfile,
			int aBeginTimeId,
			int aEndTimeId)
		{
			int timeId;
			PlanCellReference planCellRef;

			try
			{
				for (timeId = aBeginTimeId; timeId <= aEndTimeId; timeId = aPlanCellRef.PlanCube.IncrementTimeKey(timeId, 1))
				{
					planCellRef = (PlanCellReference)aPlanCellRef.Copy();
					planCellRef[eProfileType.Variable] = aVariableProfile.Key;
					planCellRef[planCellRef.PlanCube.GetTimeType()] = timeId;

					aCompSchd.InsertFormula(aPlanCellRef, planCellRef, aFormula);
				}
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Inserts a Forumla into the schedule queue for the given PlanCellReference, FormulaSpreadProfile, and
		/// VariableProfile.
		/// </summary>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Formula in.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the cell being computed.
		/// </param>
		/// <param name="aFormula">
		/// The FormulaSpreadProfile of the forumla to execute.
		/// </param>
		/// <param name="aTimeTotalVariableProfile">
		/// The Id of this TimeTotalVariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>

		public void InsertFormula(
			ComputationSchedule aCompSchd,
			PlanCellReference aPlanCellRef,
			FormulaProfile aFormula,
			TimeTotalVariableProfile aTimeTotalVariableProfile)
		{
			Cube totalCube;
			PlanCellReference planCellRef;

			try
			{
				planCellRef = null;

				if (!aPlanCellRef.PlanCube.isDateTotalCube)
				{
					totalCube = aPlanCellRef.PlanCube.PlanCubeGroup.GetCube(intGetCubeType(aPlanCellRef, eCubeType.DateTotal));

					if (totalCube != null)
					{
						planCellRef = (PlanCellReference)totalCube.CreateCellReference(aPlanCellRef);
					}
				}
				else
				{
					planCellRef = (PlanCellReference)aPlanCellRef.Copy();
				}

				if (planCellRef != null)
				{
					planCellRef[eProfileType.TimeTotalVariable] = aTimeTotalVariableProfile.Key;
					planCellRef[eProfileType.Variable] = aTimeTotalVariableProfile.ParentVariableProfile.Key;

					aCompSchd.InsertFormula(aPlanCellRef, planCellRef, aFormula);
				}
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Inserts an Init into the schedule queue for the given ComputationSchedule, PlanCellReference, and
		/// VariableProfile.
		/// </summary>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Init in.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the cell being computed.
		/// </param>
		/// <param name="aVariableProfile">
		/// The Id of this VariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>

		public void InsertInitFormula(
			ComputationSchedule aCompSchd,
			PlanCellReference aPlanCellRef,
			TimeTotalVariableProfile aTimeTotalVariableProfile)
		{
			Cube totalCube;
			PlanCellReference planCellRef;

			try
			{
				planCellRef = null;

				if (!aPlanCellRef.PlanCube.isDateTotalCube)
				{
					totalCube = aPlanCellRef.PlanCube.PlanCubeGroup.GetCube(intGetCubeType(aPlanCellRef, eCubeType.DateTotal));

					if (totalCube != null)
					{
						planCellRef = (PlanCellReference)totalCube.CreateCellReference(aPlanCellRef);
					}
				}
				else
				{
					planCellRef = (PlanCellReference)aPlanCellRef.Copy();
				}

				if (planCellRef != null)
				{
					planCellRef[eProfileType.TimeTotalVariable] = aTimeTotalVariableProfile.Key;
					planCellRef[eProfileType.Variable] = aTimeTotalVariableProfile.ParentVariableProfile.Key;

					aCompSchd.InsertInitFormula(aPlanCellRef, planCellRef);
				}
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Inserts an Init into the schedule queue for the given ComputationSchedule, PlanCellReference,
		/// VariableProfile, and time id.
		/// </summary>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Init in.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the cell being computed.
		/// </param>
		/// <param name="aVariableProfile">
		/// The Id of this VariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>
		/// <param name="aTimeId">
		/// Indicates the time Id to schedule.
		/// </param>

		public void InsertInitFormula(
			ComputationSchedule aCompSchd,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			int aTimeId)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Copy();
				planCellRef[eProfileType.Variable] = aVariableProfile.Key;
				planCellRef[planCellRef.PlanCube.GetTimeType()] = aTimeId;

				aCompSchd.InsertInitFormula(aPlanCellRef, planCellRef);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Inserts an Init into the schedule queue for the given ComputationSchedule, PlanCellReference,
		/// VariableProfile, and time range.
		/// </summary>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Init in.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the cell being computed.
		/// </param>
		/// <param name="aVariableProfile">
		/// The Id of this VariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>
		/// <param name="aBeginTimeId">
		/// Indicates the beginning time Id of the time range.
		/// </param>
		/// <param name="aEndTimeId">
		/// Indicates the ending time Id of the time range.
		/// </param>

		public void InsertInitFormula(
			ComputationSchedule aCompSchd,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			int aBeginTimeId,
			int aEndTimeId)
		{
			int timeId;
			PlanCellReference planCellRef;

			try
			{
				for (timeId = aBeginTimeId; timeId <= aEndTimeId; timeId = aPlanCellRef.PlanCube.IncrementTimeKey(timeId, 1))
				{
					planCellRef = (PlanCellReference)aPlanCellRef.Copy();
					planCellRef[eProfileType.Variable] = aVariableProfile.Key;
					planCellRef[planCellRef.PlanCube.GetTimeType()] = timeId;

					aCompSchd.InsertInitFormula(aPlanCellRef, planCellRef);
				}
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Inserts a Spread into the schedule queue from the given PlanCellReference to the ArrayList of PlanCellReferences
		/// for the given FormulaSpreadProfile.
		/// </summary>
		/// <remarks>
		/// For each CellReference that is passed in through aSpreadToCellRefs, the CellReference is added to the
		/// SpreadCellReference ArrayList in the ComputationScheduleSpreadEntry.  If the user has changed one of
		/// these cells, it will be rejected.  If one of these cells is already scheduled by a differe spread or
		/// calculation, a formula conflict exception will be thrown.
		/// </remarks>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Formula in.
		/// </param>
		/// <param name="aSpreadFromCellRef">
		/// The PlanCellReference that identifies the cell being spread from.
		/// </param>
		/// <param name="aVariableProfile">
		/// The Id of this VariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>
		/// <param name="aBeginTimeId">
		/// Indicates the beginning time Id of the time range.
		/// </param>
		/// <param name="aEndTimeId">
		/// Indicates the ending time Id of the time range.
		/// </param>
		/// <param name="aSpread">
		/// The FormulaSpreadProfile of the spread to execute.
		/// </param>

		public void InsertSpread(
			ComputationSchedule aCompSchd,
			PlanCellReference aSpreadFromCellRef,
			VariableProfile aVariableProfile,
			int aTimeId,
			SpreadProfile aSpread)
		{
			try
			{
				InsertSpread(aCompSchd, aSpreadFromCellRef, aVariableProfile, aTimeId, aSpread, null);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Inserts a Spread into the schedule queue from the given PlanCellReference to the ArrayList of PlanCellReferences
		/// for the given FormulaSpreadProfile.
		/// </summary>
		/// <remarks>
		/// For each CellReference that is passed in through aSpreadToCellRefs, the CellReference is added to the
		/// SpreadCellReference ArrayList in the ComputationScheduleSpreadEntry.  If the user has changed one of
		/// these cells, it will be rejected.  If one of these cells is already scheduled by a differe spread or
		/// calculation, a formula conflict exception will be thrown.
		/// </remarks>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Formula in.
		/// </param>
		/// <param name="aSpreadFromCellRef">
		/// The PlanCellReference that identifies the cell being spread from.
		/// </param>
		/// <param name="aVariableProfile">
		/// The Id of this VariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>
		/// <param name="aBeginTimeId">
		/// Indicates the beginning time Id of the time range.
		/// </param>
		/// <param name="aEndTimeId">
		/// Indicates the ending time Id of the time range.
		/// </param>
		/// <param name="aSpread">
		/// The FormulaSpreadProfile of the spread to execute.
		/// </param>

		public void InsertSpread(
			ComputationSchedule aCompSchd,
			PlanCellReference aSpreadFromCellRef,
			VariableProfile aVariableProfile,
			int aBeginTimeId,
			int aEndTimeId,
			SpreadProfile aSpread)
		{
			try
			{
				InsertSpread(aCompSchd, aSpreadFromCellRef, aVariableProfile, aBeginTimeId, aEndTimeId, aSpread, null);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Inserts a Spread into the schedule queue from the given PlanCellReference to the ArrayList of PlanCellReferences
		/// for the given FormulaSpreadProfile.
		/// </summary>
		/// <remarks>
		/// For each CellReference that is passed in through aSpreadToCellRefs, the CellReference is added to the
		/// SpreadCellReference ArrayList in the ComputationScheduleSpreadEntry.  If the user has changed one of
		/// these cells, it will be rejected.  If one of these cells is already scheduled by a differe spread or
		/// calculation, a formula conflict exception will be thrown.
		/// </remarks>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Formula in.
		/// </param>
		/// <param name="aSpreadFromCellRef">
		/// The PlanCellReference that identifies the cell being spread from.
		/// </param>
		/// <param name="aVariableProfile">
		/// The Id of this VariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>
		/// <param name="aBeginTimeId">
		/// Indicates the beginning time Id of the time range.
		/// </param>
		/// <param name="aEndTimeId">
		/// Indicates the ending time Id of the time range.
		/// </param>
		/// <param name="aSpread">
		/// The FormulaSpreadProfile of the spread to execute.
		/// </param>
		/// <param name="aCascadeChangeMethodProf">
		/// The ChangeMethodProfile that will be executed for each spread-to cell in cascade spreads.
		/// </param>

		public void InsertSpread(
			ComputationSchedule aCompSchd,
			PlanCellReference aSpreadFromCellRef,
			VariableProfile aVariableProfile,
			int aTimeId,
			SpreadProfile aSpread,
			ChangeMethodProfile aCascadeChangeMethodProf)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aSpreadFromCellRef.Copy();
				planCellRef[eProfileType.Variable] = aVariableProfile.Key;
				planCellRef[planCellRef.PlanCube.GetTimeType()] = aTimeId;

				aCompSchd.InsertSpread(planCellRef, aSpread.GetSpreadToCellReferenceList(planCellRef), aSpread, aCascadeChangeMethodProf);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Inserts a Spread into the schedule queue from the given PlanCellReference to the ArrayList of PlanCellReferences
		/// for the given FormulaSpreadProfile.
		/// </summary>
		/// <remarks>
		/// For each CellReference that is passed in through aSpreadToCellRefs, the CellReference is added to the
		/// SpreadCellReference ArrayList in the ComputationScheduleSpreadEntry.  If the user has changed one of
		/// these cells, it will be rejected.  If one of these cells is already scheduled by a differe spread or
		/// calculation, a formula conflict exception will be thrown.
		/// </remarks>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Formula in.
		/// </param>
		/// <param name="aSpreadFromCellRef">
		/// The PlanCellReference that identifies the cell being spread from.
		/// </param>
		/// <param name="aVariableProfile">
		/// The Id of this VariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>
		/// <param name="aBeginTimeId">
		/// Indicates the beginning time Id of the time range.
		/// </param>
		/// <param name="aEndTimeId">
		/// Indicates the ending time Id of the time range.
		/// </param>
		/// <param name="aSpread">
		/// The FormulaSpreadProfile of the spread to execute.
		/// </param>
		/// <param name="aCascadeChangeMethodProf">
		/// The ChangeMethodProfile that will be executed for each spread-to cell in cascade spreads.
		/// </param>

		public void InsertSpread(
			ComputationSchedule aCompSchd,
			PlanCellReference aSpreadFromCellRef,
			VariableProfile aVariableProfile,
			int aBeginTimeId,
			int aEndTimeId,
			SpreadProfile aSpread,
			ChangeMethodProfile aCascadeChangeMethodProf)
		{
			int timeId;
			PlanCellReference planCellRef;

			try
			{
				for (timeId = aBeginTimeId; timeId <= aEndTimeId; timeId = aSpreadFromCellRef.PlanCube.IncrementTimeKey(timeId, 1))
				{
					planCellRef = (PlanCellReference)aSpreadFromCellRef.Copy();
					planCellRef[eProfileType.Variable] = aVariableProfile.Key;
					planCellRef[planCellRef.PlanCube.GetTimeType()] = timeId;

					aCompSchd.InsertSpread(planCellRef, aSpread.GetSpreadToCellReferenceList(planCellRef), aSpread, aCascadeChangeMethodProf);
				}
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Inserts a Spread into the schedule queue from the given PlanCellReference to the ArrayList of PlanCellReferences
		/// for the given FormulaSpreadProfile.
		/// </summary>
		/// <remarks>
		/// For each CellReference that is passed in through aSpreadToCellRefs, the CellReference is added to the
		/// SpreadCellReference ArrayList in the ComputationScheduleSpreadEntry.  If the user has changed one of
		/// these cells, it will be rejected.  If one of these cells is already scheduled by a differe spread or
		/// calculation, a formula conflict exception will be thrown.
		/// </remarks>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Formula in.
		/// </param>
		/// <param name="aSpreadFromCellRef">
		/// The PlanCellReference that identifies the cell being spread from.
		/// </param>
		/// <param name="aTimeTotalVariableProfile">
		/// The Id of this TimeTotalVariableProfile will be substituted into the CellReference before being added to the
		/// schedule.
		/// </param>
		/// <param name="aSpreadToCellRefs">
		/// An ArrayList of PlanCellReferences that identify the cells being spread to.
		/// </param>
		/// <param name="aFormula">
		/// The FormulaSpreadProfile of the spread to execute.
		/// </param>

		public void InsertSpread(
			ComputationSchedule aCompSchd,
			PlanCellReference aSpreadFromCellRef,
			TimeTotalVariableProfile aTimeTotalVariableProfile,
			SpreadProfile aSpread)
		{
			Cube totalCube;
			PlanCellReference planCellRef;

			try
			{
				planCellRef = null;

				if (!aSpreadFromCellRef.PlanCube.isDateTotalCube)
				{
					totalCube = aSpreadFromCellRef.PlanCube.PlanCubeGroup.GetCube(intGetCubeType(aSpreadFromCellRef, eCubeType.DateTotal));

					if (totalCube != null)
					{
						planCellRef = (PlanCellReference)totalCube.CreateCellReference(aSpreadFromCellRef);
					}
				}
				else
				{
					planCellRef = (PlanCellReference)aSpreadFromCellRef.Copy();
				}

				if (planCellRef != null)
				{
					planCellRef[eProfileType.TimeTotalVariable] = aTimeTotalVariableProfile.Key;
					planCellRef[eProfileType.Variable] = aTimeTotalVariableProfile.ParentVariableProfile.Key;
					aCompSchd.InsertSpread(planCellRef, aSpread.GetSpreadToCellReferenceList(planCellRef), aSpread);
				}
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aTimeId">
		/// The time Id to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aCompCellRef,
			VariableProfile aVariableProfile,
			int aTimeId)
		{
			try
			{
				return GetOperandCell(aScheduleEntry, aSetCellMode, aCompCellRef, aVariableProfile, aTimeId, true);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType)
		{
			try
			{
				return GetTotalOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aCubeType, true);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			QuantityVariableProfile aQuantityVariableProfile)
		{
			try
			{
				return GetTotalOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aCubeType, aQuantityVariableProfile, true);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell from the given cube, substituting the VariableProfile with the given variable ID and the
		/// QuantityVariableProfile with the given quantity variable Id before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			VariableProfile aVariableProfile,
			QuantityVariableProfile aQuantityVariableProfile)
		{
			try
			{
				return GetTotalOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aCubeType, aVariableProfile, aQuantityVariableProfile, true);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aTimeId">
		/// The time Id to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			int aTimeId,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Copy();
				planCellRef[eProfileType.Variable] = aVariableProfile.Key;
				planCellRef[planCellRef.PlanCube.GetTimeType()] = aTimeId;

				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = intGetPlanCellReferenceFromTotalCube(aPlanCellRef, aCubeType);
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell from the given cube, substituting the VariableProfile with the given variable ID and the
		/// QuantityVariableProfile with the given quantity variable Id before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			VariableProfile aVariableProfile,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = intGetPlanCellReferenceFromTotalCube(aPlanCellRef, aCubeType);
				planCellRef[eProfileType.Variable] = aVariableProfile.Key;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = intGetPlanCellReferenceFromTotalCube(aPlanCellRef, aCubeType);
				planCellRef[eProfileType.QuantityVariable] = aQuantityVariableProfile.Key;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell from the given cube, substituting the VariableProfile with the given variable ID and the
		/// QuantityVariableProfile with the given quantity variable Id before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			VariableProfile aVariableProfile,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = intGetPlanCellReferenceFromTotalCube(aPlanCellRef, aCubeType);
				planCellRef[eProfileType.Variable] = aVariableProfile.Key;
				planCellRef[eProfileType.QuantityVariable] = aQuantityVariableProfile.Key;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell from the given cube, substituting the time Id with the given time Id before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aTimeId">
		/// The time Id to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public PlanCellReference GetWeekOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			int aTimeId,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Cube.CubeGroup.GetCube(intGetCubeType(aPlanCellRef, eCubeType.WeekDetail)).CreateCellReference(aPlanCellRef);
				planCellRef[planCellRef.PlanCube.GetTimeType()] = aTimeId;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell from the given cube, substituting the time Id with the given time Id before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aTimeId">
		/// The time Id to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public PlanCellReference GetWeekOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			int aTimeId,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Cube.CubeGroup.GetCube(intGetCubeType(aPlanCellRef, eCubeType.WeekDetail)).CreateCellReference(aPlanCellRef);
				planCellRef[eProfileType.Variable] = aVariableProfile.Key;
				planCellRef[planCellRef.PlanCube.GetTimeType()] = aTimeId;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell for the Actual version.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetActualOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Copy();
				planCellRef[planCellRef.PlanCube.GetVersionType()] = Include.FV_ActualRID;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell for the Actual version.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aTimeId">
		/// The time Id to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetActionOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Copy();
				planCellRef[planCellRef.PlanCube.GetVersionType()] = Include.FV_ActionRID;
				planCellRef[eProfileType.Variable] = aVariableProfile.Key;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell for the Actual version.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aTimeId">
		/// The time Id to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetActionOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			int aTimeId,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Copy();
				planCellRef[planCellRef.PlanCube.GetVersionType()] = Include.FV_ActionRID;
				planCellRef[eProfileType.Variable] = aVariableProfile.Key;
				planCellRef[planCellRef.PlanCube.GetTimeType()] = aTimeId;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		//Begin TT#824 - JScott - Add Toolbox methods to support external node references
		/// <summary>
		/// Gets an operand Cell for the Action version.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aNodeId">
		/// The Node Id to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetActionOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			string aNodeId,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;
			int nodeRID;
			string message;

			try
			{
				nodeRID = aPlanCellRef.PlanCube.SAB.HierarchyServerSession.GetNodeRID(aNodeId);

				if (nodeRID != Include.NoRID)
				{
					planCellRef = (PlanCellReference)aPlanCellRef.Copy();
					planCellRef[planCellRef.PlanCube.GetVersionType()] = Include.FV_ActionRID;
					planCellRef[eProfileType.Variable] = aVariableProfile.Key;
					planCellRef[planCellRef.PlanCube.GetHierarchyNodeType()] = nodeRID;
					if (aCheckPending)
					{
						intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
					}
					return planCellRef;
				}
				else
				{
					message = String.Format(MIDText.GetTextOnly(eMIDTextCode.msg_pl_InvalidNodeIdSpecified), aNodeId);

					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_InvalidNodeIdSpecified,
						message);
				}
			}
			catch (Exception exc)
			{
				//string message = exc.ToString();
				throw;
			}
		}

		//End TT#824 - JScott - Add Toolbox methods to support external node references
		/// <summary>
		/// Gets an operand Cell for the Chain cube.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetChainOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Cube.CubeGroup.GetCube(intGetCubeType(aPlanCellRef, eCubeType.ChainDetail)).CreateCellReference(aPlanCellRef);
				planCellRef[eProfileType.Variable] = aVariableProfile.Key;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell for the Chain cube.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aTimeId">
		/// The time Id to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetChainOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			int aTimeId,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Cube.CubeGroup.GetCube(intGetCubeType(aPlanCellRef, eCubeType.ChainDetail)).CreateCellReference(aPlanCellRef);
				planCellRef[eProfileType.Variable] = aVariableProfile.Key;
				planCellRef[planCellRef.PlanCube.GetTimeType()] = aTimeId;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell for the Chain cube.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetChainActionOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Cube.CubeGroup.GetCube(intGetCubeType(aPlanCellRef, eCubeType.ChainDetail)).CreateCellReference(aPlanCellRef);
				planCellRef[planCellRef.PlanCube.GetVersionType()] = Include.FV_ActionRID;
				planCellRef[eProfileType.Variable] = aVariableProfile.Key;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell value.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aTimeId">
		/// The time Id to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			int aTimeId,
			bool aUseHiddenValues)
		{
			try
			{
				return GetOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aVariableProfile, aTimeId, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell value.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aTimeId">
		/// The time Id to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			int aTimeId,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aVariableProfile, aTimeId, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell value.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aBeginTimeId">
		/// The beginning time Id to be substituted before retrieving.
		/// </param>
		/// <param name="aEndTimeId">
		/// The ending time Id to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			int aBeginTimeId,
			int aEndTimeId,
			bool aUseHiddenValues)
		{
			try
			{
				return GetOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aVariableProfile, aBeginTimeId, aEndTimeId, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell value.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aBeginTimeId">
		/// The beginning time Id to be substituted before retrieving.
		/// </param>
		/// <param name="aEndTimeId">
		/// The ending time Id to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			int aBeginTimeId,
			int aEndTimeId,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			double cumValue;
			int time;

			try
			{
				cumValue = 0;

				for (time = aBeginTimeId; time <= aEndTimeId; time = aPlanCellRef.PlanCube.IncrementTimeKey(time, 1))
				{
					cumValue += GetOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aVariableProfile, time, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
				}

				return cumValue;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			bool aUseHiddenValues)
		{
			try
			{
				return GetTotalOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aCubeType, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetTotalOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aCubeType, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value from the given cube, substituting the VariableProfile with the given variable ID and the
		/// QuantityVariableProfile with the given quantity variable Id before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			VariableProfile aVariableProfile,
			bool aUseHiddenValues)
		{
			try
			{
				return GetTotalOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aCubeType, aVariableProfile, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value from the given cube, substituting the VariableProfile with the given variable ID and the
		/// QuantityVariableProfile with the given quantity variable Id before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			VariableProfile aVariableProfile,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetTotalOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aCubeType, aVariableProfile, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aUseHiddenValues)
		{
			try
			{
				return GetTotalOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aCubeType, aQuantityVariableProfile, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetTotalOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aCubeType, aQuantityVariableProfile, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value from the given cube, substituting the VariableProfile with the given variable ID and the
		/// QuantityVariableProfile with the given quantity variable Id before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			VariableProfile aVariableProfile,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aUseHiddenValues)
		{
			try
			{
				return GetTotalOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aCubeType, aVariableProfile, aQuantityVariableProfile, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value from the given cube, substituting the VariableProfile with the given variable ID and the
		/// QuantityVariableProfile with the given quantity variable Id before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			VariableProfile aVariableProfile,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetTotalOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aCubeType, aVariableProfile, aQuantityVariableProfile, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value from the given cube, substituting the VariableProfile with the given variable ID and the
		/// QuantityVariableProfile with the given quantity variable Id before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aWeekId">
		/// The week Id to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetWeekOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			int aWeekId,
			bool aUseHiddenValues)
		{
			try
			{
				return GetWeekOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aWeekId, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value from the given cube, substituting the VariableProfile with the given variable ID and the
		/// QuantityVariableProfile with the given quantity variable Id before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aWeekId">
		/// The week Id to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetWeekOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			int aWeekId,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetWeekOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aWeekId, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value from the given cube, substituting the VariableProfile with the given variable ID and the
		/// QuantityVariableProfile with the given quantity variable Id before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aWeekId">
		/// The week Id to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetWeekOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			int aWeekId,
			bool aUseHiddenValues)
		{
			try
			{
				return GetWeekOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aVariableProfile, aWeekId, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value from the given cube, substituting the VariableProfile with the given variable ID and the
		/// QuantityVariableProfile with the given quantity variable Id before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aWeekId">
		/// The week Id to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetWeekOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			int aWeekId,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetWeekOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aVariableProfile, aWeekId, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value from the given cube, substituting the VariableProfile with the given variable ID and the
		/// QuantityVariableProfile with the given quantity variable Id before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aBeginWeekId">
		/// The beginning week Id to be substituted before retrieving.
		/// </param>
		/// <param name="aEndWeekId">
		/// The ending week Id to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetWeekOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			int aBeginWeekId,
			int aEndWeekId,
			bool aUseHiddenValues)
		{
			try
			{
				return GetWeekOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aVariableProfile, aBeginWeekId, aEndWeekId, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value from the given cube, substituting the VariableProfile with the given variable ID and the
		/// QuantityVariableProfile with the given quantity variable Id before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aBeginWeekId">
		/// The beginning week Id to be substituted before retrieving.
		/// </param>
		/// <param name="aEndWeekId">
		/// The ending week Id to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetWeekOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			int aBeginWeekId,
			int aEndWeekId,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			double cumValue;
			int week;

			try
			{
				cumValue = 0;

				for (week = aBeginWeekId; week <= aEndWeekId; week = aPlanCellRef.PlanCube.SAB.ApplicationServerSession.Calendar.AddWeeks(week, 1))
				{
					cumValue += GetWeekOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aVariableProfile, week, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
				}

				return cumValue;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value for the Action version.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetActualOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			bool aUseHiddenValues)
		{
			try
			{
				return GetActualOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value for the Action version.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetActualOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetActualOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value for the Action version.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetActionOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			bool aUseHiddenValues)
		{
			try
			{
				return GetActionOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aVariableProfile, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value for the Action version.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetActionOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetActionOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aVariableProfile, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value for the Action version.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aTimeId">
		/// The time Id to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetActionOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			int aTimeId,
			bool aUseHiddenValues)
		{
			try
			{
				return GetActionOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aVariableProfile, aTimeId, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value for the Action version.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aTimeId">
		/// The time Id to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetActionOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			int aTimeId,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetActionOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aVariableProfile, aTimeId, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		//Begin TT#824 - JScott - Add Toolbox methods to support external node references
		/// <summary>
		/// Gets an operand Cell Quantity value for the Action version.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aNodeId">
		/// The Node Id to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetActionOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			string aNodeId,
			bool aUseHiddenValues)
		{
			try
			{
				return GetActionOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aVariableProfile, aNodeId, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				//string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value for the Action version.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aNodeId">
		/// The Node Id to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetActionOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			string aNodeId,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetActionOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aVariableProfile, aNodeId, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				//string message = exc.ToString();
				throw;
			}
		}

		//End TT#824 - JScott - Add Toolbox methods to support external node references
		/// <summary>
		/// Gets an operand Cell Quantity value for the Chain cube.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReferencePlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetChainOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			bool aUseHiddenValues)
		{
			try
			{
				return GetChainOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aVariableProfile, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value for the Chain cube.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetChainOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetChainOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aVariableProfile, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value for the Chain cube.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aTimeId">
		/// The time Id to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetChainOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			int aTimeId,
			bool aUseHiddenValues)
		{
			try
			{
				return GetChainOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aVariableProfile, aTimeId, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value for the Chain cube.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aTimeId">
		/// The time Id to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetChainOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			int aTimeId,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetChainOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aVariableProfile, aTimeId, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value for the Chain cube.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetChainActionOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			bool aUseHiddenValues)
		{
			try
			{
				return GetChainActionOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aVariableProfile, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value for the Chain cube.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetChainActionOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetChainActionOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aVariableProfile, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell, substituting the time total index before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aTimeTotalIdx">
		/// The time total index to substitue before retrieving.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetTimeTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			TimeTotalVariableProfile aTimeTotalVariableProfile)
		{
			try
			{
				return GetTimeTotalOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aTimeTotalVariableProfile, true);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell, substituting the time total index before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aTimeTotalIdx">
		/// The time total index to substitue before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetTimeTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			TimeTotalVariableProfile aTimeTotalVariableProfile,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;

			try
			{
				if (aPlanCellRef.PlanCube.isDateTotalCube)
				{
					planCellRef = (PlanCellReference)aPlanCellRef.Copy();
				}
				else
				{
					planCellRef = (PlanCellReference)aPlanCellRef.Cube.CubeGroup.GetCube(intGetCubeType(aPlanCellRef, eCubeType.DateTotal)).CreateCellReference(aPlanCellRef);
				}

				planCellRef[eProfileType.TimeTotalVariable] = aTimeTotalVariableProfile.Key;
				planCellRef[eProfileType.Variable] = aTimeTotalVariableProfile.ParentVariableProfile.Key;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets a time total operand Cell Quantity value from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aTimeTotalIdx">
		/// The time total index of the cell being retrieved.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the requested cell.
		/// </returns>

		public PlanCellReference GetTimeTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			QuantityVariableProfile aQuantityVariableProfile,
			TimeTotalVariableProfile aTimeTotalVariableProfile)
		{
			try
			{
				return GetTimeTotalOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aQuantityVariableProfile, aTimeTotalVariableProfile, true);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets a time total operand Cell Quantity value from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aTimeTotalIdx">
		/// The time total index of the cell being retrieved.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the requested cell.
		/// </returns>

		public PlanCellReference GetTimeTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			QuantityVariableProfile aQuantityVariableProfile,
			TimeTotalVariableProfile aTimeTotalVariableProfile,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;

			try
			{
				if (aPlanCellRef.PlanCube.isDateTotalCube)
				{
					planCellRef = (PlanCellReference)aPlanCellRef.Copy();
				}
				else
				{
					planCellRef = (PlanCellReference)aPlanCellRef.Cube.CubeGroup.GetCube(intGetCubeType(aPlanCellRef, eCubeType.DateTotal)).CreateCellReference(aPlanCellRef);
				}

				planCellRef[eProfileType.TimeTotalVariable] = aTimeTotalVariableProfile.Key;
				planCellRef[eProfileType.QuantityVariable] = aQuantityVariableProfile.Key;
				planCellRef[eProfileType.Variable] = aTimeTotalVariableProfile.ParentVariableProfile.Key;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell, substituting the time total index before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aTimeTotalIdx">
		/// The time total index to substitue before retrieving.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetTimeTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			TimeTotalVariableProfile aTimeTotalVariableProfile)
		{
			try
			{
				return GetTimeTotalOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aCubeType, aTimeTotalVariableProfile, true);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell, substituting the time total index before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aTimeTotalIdx">
		/// The time total index to substitue before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetTimeTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			TimeTotalVariableProfile aTimeTotalVariableProfile,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = intGetPlanCellReferenceFromTotalCube(aPlanCellRef, aCubeType);
				planCellRef[eProfileType.TimeTotalVariable] = aTimeTotalVariableProfile.Key;
				planCellRef[eProfileType.Variable] = aTimeTotalVariableProfile.ParentVariableProfile.Key;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets a time total operand Cell Quantity value from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aTimeTotalIdx">
		/// The time total index of the cell being retrieved.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the requested cell.
		/// </returns>

		public PlanCellReference GetTimeTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			QuantityVariableProfile aQuantityVariableProfile,
			TimeTotalVariableProfile aTimeTotalVariableProfile)
		{
			try
			{
				return GetTimeTotalOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aCubeType, aQuantityVariableProfile, aTimeTotalVariableProfile, true);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets a time total operand Cell Quantity value from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aTimeTotalIdx">
		/// The time total index of the cell being retrieved.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the requested cell.
		/// </returns>

		public PlanCellReference GetTimeTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			QuantityVariableProfile aQuantityVariableProfile,
			TimeTotalVariableProfile aTimeTotalVariableProfile,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = intGetPlanCellReferenceFromTotalCube(aPlanCellRef, aCubeType);
				planCellRef[eProfileType.TimeTotalVariable] = aTimeTotalVariableProfile.Key;
				planCellRef[eProfileType.QuantityVariable] = aQuantityVariableProfile.Key;
				planCellRef[eProfileType.Variable] = aTimeTotalVariableProfile.ParentVariableProfile.Key;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets a low-level total operand Cell Quantity value from the low-level total cube.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the requested cell.
		/// </returns>

		public PlanCellReference GetLowLevelTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef)
		{
			try
			{
				return GetLowLevelTotalOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, true);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets a low-level total operand Cell Quantity value from the low-level total cube.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the requested cell.
		/// </returns>

		public PlanCellReference GetLowLevelTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.PlanCube.PlanCubeGroup.GetCube(intGetCubeType(aPlanCellRef, eCubeType.LowLevelTotal)).CreateCellReference(aPlanCellRef);
				planCellRef[eProfileType.LowLevelTotalVersion] = Include.FV_PlanLowLevelTotalRID;
				planCellRef[eProfileType.QuantityVariable] = BasePlanQuantityVariables.ValueQuantity.Key;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell value, substituting the time total index before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aTimeTotalIdx">
		/// The time total index to substitue before retrieving.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetTimeTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			TimeTotalVariableProfile aTimeTotalVariableProfile,
			bool aUseHiddenValues)
		{
			try
			{
				return GetTimeTotalOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aTimeTotalVariableProfile, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell value, substituting the time total index before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aTimeTotalIdx">
		/// The time total index to substitue before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetTimeTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			TimeTotalVariableProfile aTimeTotalVariableProfile,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetTimeTotalOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aTimeTotalVariableProfile, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets a time total operand Cell Quantity value from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aCompCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aTimeTotalIdx">
		/// The time total index of the cell being retrieved.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetTimeTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			QuantityVariableProfile aQuantityVariableProfile,
			TimeTotalVariableProfile aTimeTotalVariableProfile,
			bool aUseHiddenValues)
		{
			try
			{
				return GetTimeTotalOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aQuantityVariableProfile, aTimeTotalVariableProfile, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets a time total operand Cell Quantity value from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aTimeTotalIdx">
		/// The time total index of the cell being retrieved.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetTimeTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			QuantityVariableProfile aQuantityVariableProfile,
			TimeTotalVariableProfile aTimeTotalVariableProfile,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetTimeTotalOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aQuantityVariableProfile, aTimeTotalVariableProfile, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell value, substituting the time total index before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aTimeTotalIdx">
		/// The time total index to substitue before retrieving.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetTimeTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			TimeTotalVariableProfile aTimeTotalVariableProfile,
			bool aUseHiddenValues)
		{
			try
			{
				return GetTimeTotalOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aCubeType, aTimeTotalVariableProfile, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell value, substituting the time total index before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aTimeTotalIdx">
		/// The time total index to substitue before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetTimeTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			TimeTotalVariableProfile aTimeTotalVariableProfile,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetTimeTotalOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aCubeType, aTimeTotalVariableProfile, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets a time total operand Cell Quantity value from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aTimeTotalIdx">
		/// The time total index of the cell being retrieved.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetTimeTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			QuantityVariableProfile aQuantityVariableProfile,
			TimeTotalVariableProfile aTimeTotalVariableProfile,
			bool aUseHiddenValues)
		{
			try
			{
				return GetTimeTotalOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aCubeType, aQuantityVariableProfile, aTimeTotalVariableProfile, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets a time total operand Cell Quantity value from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aTimeTotalIdx">
		/// The time total index of the cell being retrieved.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetTimeTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			QuantityVariableProfile aQuantityVariableProfile,
			TimeTotalVariableProfile aTimeTotalVariableProfile,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetTimeTotalOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aCubeType, aQuantityVariableProfile, aTimeTotalVariableProfile, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		public double GetBasisOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			bool aUseHiddenValues)
		{
			try
			{
				return GetBasisOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aVariableProfile, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		public double GetBasisOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = GetBasisOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aVariableProfile, aCheckPending);

				if (planCellRef != null)
				{
					return planCellRef.GetCellValue(aGetCellMode, aUseHiddenValues);
				}
				else
				{
					return 0;
				}
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		public PlanCellReference GetBasisOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;
			ProfileList basisProfList;
			int i;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Cube.CubeGroup.GetCube(intGetCubeType(aPlanCellRef, eCubeType.Basis)).CreateCellReference(aPlanCellRef);
				basisProfList = planCellRef.PlanCube.PlanCubeGroup.OpenParms.BasisProfileList;

				if (isPlan(aPlanCellRef))
				{
					if (basisProfList.Count == 0)
					{
						return null;
					}

					planCellRef[eProfileType.Basis] = basisProfList[0].Key;
				}
				else
				{
					for (i = 0; i < basisProfList.Count; i++)
					{
						if (basisProfList[i].Key == basisProfList[aPlanCellRef[eProfileType.Basis]].Key)
						{
							break;
						}
					}

					if (i == basisProfList.Count - 1)
					{
						return null;
					}

					planCellRef[eProfileType.Basis] = basisProfList[i + 1].Key;
				}

				planCellRef[eProfileType.Variable] = aVariableProfile.Key;

				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}

				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets a low-level total operand Cell Quantity value from the low-level total cube.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetLowLevelTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			bool aUseHiddenValues)
		{
			try
			{
				return GetLowLevelTotalOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets a low-level total operand Cell Quantity value from the low-level total cube.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetLowLevelTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetLowLevelTotalOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the high-level Chain operand Cell.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetHighLevelChainOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			QuantityVariableProfile aQuantityVariableProfile)
		{
			try
			{
				return GetHighLevelChainOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aQuantityVariableProfile, true);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the high-level Chain operand Cell.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetHighLevelChainOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Cube.CubeGroup.GetCube(intGetCubeType(aPlanCellRef, eCubeType.LowLevelDetail)).CreateCellReference(aPlanCellRef);
				planCellRef[planCellRef.PlanCube.GetHierarchyNodeType()] = aPlanCellRef.PlanCube.PlanCubeGroup.OpenParms.ChainHLPlanProfile.NodeProfile.Key;
				planCellRef[planCellRef.PlanCube.GetVersionType()] = aPlanCellRef.PlanCube.PlanCubeGroup.OpenParms.ChainHLPlanProfile.VersionProfile.Key;
				planCellRef[eProfileType.QuantityVariable] = aQuantityVariableProfile.Key;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the high-level Chain operand Cell.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aUseHiddenValues">
		/// A boolean indicating if hidden values should be used.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public double GetHighLevelChainOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aUseHiddenValues)
		{
			try
			{
				return GetHighLevelChainOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aQuantityVariableProfile).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the high-level Store operand Cell.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetHighLevelStoreOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			QuantityVariableProfile aQuantityVariableProfile)
		{
			try
			{
				return GetHighLevelStoreOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aQuantityVariableProfile, true);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the high-level Store operand Cell.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetHighLevelStoreOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Cube.CubeGroup.GetCube(intGetCubeType(aPlanCellRef, eCubeType.LowLevelDetail)).CreateCellReference(aPlanCellRef);
				planCellRef[planCellRef.PlanCube.GetHierarchyNodeType()] = aPlanCellRef.PlanCube.PlanCubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.Key;
				planCellRef[planCellRef.PlanCube.GetVersionType()] = aPlanCellRef.PlanCube.PlanCubeGroup.OpenParms.StoreHLPlanProfile.VersionProfile.Key;
				planCellRef[eProfileType.QuantityVariable] = aQuantityVariableProfile.Key;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the high-level Store operand Cell.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aUseHiddenValues">
		/// A boolean indicating if hidden values should be used.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public double GetHighLevelStoreOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aUseHiddenValues)
		{
			try
			{
				return GetHighLevelStoreOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aQuantityVariableProfile).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the Store Total operand Cell.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetStoreTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			QuantityVariableProfile aQuantityVariableProfile)
		{
			try
			{
				return GetStoreTotalOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aQuantityVariableProfile, true);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the Store Total Cell.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public PlanCellReference GetStoreTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aCheckPending)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Cube.CubeGroup.GetCube(intGetCubeType(aPlanCellRef, eCubeType.StoreTotal)).CreateCellReference(aPlanCellRef);
				planCellRef[eProfileType.QuantityVariable] = aQuantityVariableProfile.Key;
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				}
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the Store Total operand Cell.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aUseHiddenValues">
		/// A boolean indicating if hidden values should be used.
		/// </param>
		/// <returns>
		/// The PlanCellReference of the PlanCubeCell.
		/// </returns>

		public double GetStoreTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aUseHiddenValues)
		{
			try
			{
				return GetStoreTotalOperandCell(aScheduleEntry, aSetCellMode, aPlanCellRef, aQuantityVariableProfile).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the cooresponding plan operand Cell value for a % Change calculation.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public PlanCellReference GetPlanOperandCellForPctChange(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Cube.CubeGroup.GetCube(intGetCubeType(aPlanCellRef, eCubeType.Plan)).CreateCellReference(aPlanCellRef);
				planCellRef[eProfileType.QuantityVariable] = BasePlanQuantityVariables.ValueQuantity.Key;
				intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the cooresponding basis operand Cell value for a % Change calculation.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public PlanCellReference GetBasisOperandCellForPctChange(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef)
		{
			PlanCellReference planCellRef;
			ProfileList basisProfList;
			int i;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Cube.CubeGroup.GetCube(intGetCubeType(aPlanCellRef, eCubeType.Basis)).CreateCellReference(aPlanCellRef);
				basisProfList = planCellRef.PlanCube.PlanCubeGroup.OpenParms.BasisProfileList;

				if (isPlan(aPlanCellRef))
				{
					planCellRef[eProfileType.Basis] = basisProfList[0].Key;
				}
				else
				{
					for (i = 0; i < basisProfList.Count; i++)
					{
						if (basisProfList[i].Key == basisProfList[aPlanCellRef[eProfileType.Basis]].Key)
						{
							break;
						}
					}

					planCellRef[eProfileType.Basis] = basisProfList[i + 1].Key;
				}

				planCellRef[eProfileType.QuantityVariable] = BasePlanQuantityVariables.ValueQuantity.Key;
				intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the cooresponding Plan operand Cell value for a % Change calculation.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetPlanOperandCellValueForPctChange(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			bool aUseHiddenValues)
		{
			try
			{
				return GetPlanOperandCellForPctChange(aScheduleEntry, aSetCellMode, aPlanCellRef).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the cooresponding basis operand Cell value for a % Change calculation.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to the PlanCubeCell being requested.
		/// </param>
		/// <returns>
		/// A double containing the PlanCubeCell's value
		/// </returns>

		public double GetBasisOperandCellValueForPctChange(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			bool aUseHiddenValues)
		{
			try
			{
				return GetBasisOperandCellForPctChange(aScheduleEntry, aSetCellMode, aPlanCellRef).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves an ArrayList of PlanCellReference objects that point to all the detail ComputationCells for a total cell.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the PlanCubeCell to find details for.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve values from.
		/// </param>
		/// <returns>
		/// An ArrayList of PlanCellReference objects of the detail ComputationCells.
		/// </returns>

		public ArrayList GetDetailCellRefArray(PlanCellReference aPlanCellRef, eCubeType aCubeType, bool aUseHiddenValues)
		{
			try
			{
				return aPlanCellRef.GetDetailCellRefArray(intGetCubeType(aPlanCellRef, aCubeType), aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves an ArrayList of PlanCellReference objects that point to all the detail ComputationCells for a total cell.
		/// </summary>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve values from.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the PlanCubeCell to find details for.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// An ArrayList of PlanCellReference objects of the detail ComputationCells.
		/// </returns>

		public ArrayList GetDetailCellRefArray(PlanCellReference aPlanCellRef, eCubeType aCubeType, QuantityVariableProfile aQuantityVariableProfile, bool aUseHiddenValues)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Copy();
				planCellRef[eProfileType.QuantityVariable] = aQuantityVariableProfile.Key;
				return planCellRef.GetDetailCellRefArray(intGetCubeType(planCellRef, aCubeType), aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves an ArrayList of PlanCellReference objects that point to all the detail ComputationCells for a total cell.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the PlanCubeCell to find details for.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve values from.
		/// </param>
		/// <param name="aStoreStatus">
		/// The eStoreStatus value that indicates which type of store to return.
		/// </param>
		/// <returns>
		/// An ArrayList of PlanCellReference objects of the detail ComputationCells.
		/// </returns>

		public ArrayList GetDetailCellRefArray(PlanCellReference aPlanCellRef, eCubeType aCubeType, eStoreStatus aStoreStatus, bool aUseHiddenValues)
		{
			try
			{
				return aPlanCellRef.GetDetailCellRefArray(intGetCubeType(aPlanCellRef, aCubeType), aStoreStatus, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves an ArrayList of PlanCellReference objects that point to all the detail ComputationCells for a total cell.
		/// </summary>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve values from.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the PlanCubeCell to find details for.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aStoreStatus">
		/// The eStoreStatus value that indicates which type of store to return.
		/// </param>
		/// <returns>
		/// An ArrayList of PlanCellReference objects of the detail ComputationCells.
		/// </returns>

		public ArrayList GetDetailCellRefArray(PlanCellReference aPlanCellRef, eCubeType aCubeType, QuantityVariableProfile aQuantityVariableProfile, eStoreStatus aStoreStatus, bool aUseHiddenValues)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Copy();
				planCellRef[eProfileType.QuantityVariable] = aQuantityVariableProfile.Key;
				return planCellRef.GetDetailCellRefArray(intGetCubeType(planCellRef, aCubeType), aStoreStatus, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves an ArrayList of PlanCellReference objects that point to all the detail ComputationCells for a total cell of the given eStoreStatus.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the PlanCubeCell to find details for.
		/// </param>
		/// <param name="aStoreStatus">
		/// The eStoreStatus value that indicates which type of store to return.
		/// </param>
		/// <returns>
		/// An ArrayList of PlanCellReference objects of the detail ComputationCells.
		/// </returns>

		public ArrayList GetComponentDetailCellRefArray(PlanCellReference aPlanCellRef, eStoreStatus aStoreStatus, bool aUseHiddenValues)
		{
			try
			{
				return aPlanCellRef.GetComponentDetailCellRefArray(aStoreStatus, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves an ArrayList of PlanCellReference objects that point to all the detail ComputationCells for a total cell of the given eStoreStatus and QuantityVariableProfile type.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the PlanCubeCell to find details for.
		/// </param>
		/// <param name="aStoreStatus">
		/// The eStoreStatus value that indicates which type of store to return.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// An ArrayList of PlanCellReference objects of the detail ComputationCells.
		/// </returns>

		public ArrayList GetComponentDetailCellRefArray(PlanCellReference aPlanCellRef, eStoreStatus aStoreStatus, QuantityVariableProfile aQuantityVariableProfile, bool aUseHiddenValues)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Copy();
				planCellRef[eProfileType.QuantityVariable] = aQuantityVariableProfile.Key;
				return planCellRef.GetComponentDetailCellRefArray(aStoreStatus, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves an ArrayList of PlanCellReference objects that point to all the detail ComputationCells for a total cell of the given eStoreStatus.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the PlanCubeCell to find details for.
		/// </param>
		/// <param name="aStoreStatus">
		/// The eStoreStatus value that indicates which type of store to return.
		/// </param>
		/// <returns>
		/// An ArrayList of PlanCellReference objects of the detail ComputationCells.
		/// </returns>

		public ArrayList GetSpreadDetailCellRefArray(PlanCellReference aPlanCellRef, eStoreStatus aStoreStatus, bool aUseHiddenValues)
		{
			try
			{
				return aPlanCellRef.GetSpreadDetailCellRefArray(aStoreStatus, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves an ArrayList of PlanCellReference objects that point to all the detail ComputationCells for a total cell of the given eStoreStatus and QuantityVariableProfile type.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the PlanCubeCell to find details for.
		/// </param>
		/// <param name="aStoreStatus">
		/// The eStoreStatus value that indicates which type of store to return.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// An ArrayList of PlanCellReference objects of the detail ComputationCells.
		/// </returns>

		public ArrayList GetSpreadDetailCellRefArray(PlanCellReference aPlanCellRef, eStoreStatus aStoreStatus, QuantityVariableProfile aQuantityVariableProfile, bool aUseHiddenValues)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Copy();
				planCellRef[eProfileType.QuantityVariable] = aQuantityVariableProfile.Key;
				return planCellRef.GetSpreadDetailCellRefArray(aStoreStatus, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the On Hand value.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the PlanCubeCell being used.
		/// </param>
		/// <returns>
		/// The On Hand value.
		/// </returns>
		
		public double GetOnHandCellValue(PlanCellReference aPlanCellRef)
		{
			BasisProfile basisProf;
			int i;
			int[] nodeRIDArray;

			try
			{
				if (isBasis(aPlanCellRef))
				{
					basisProf = ((BasisProfile)aPlanCellRef.PlanCube.PlanCubeGroup.OpenParms.GetBasisProfileList(aPlanCellRef.PlanCube.PlanCubeGroup, aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.Version]).FindKey(aPlanCellRef[eProfileType.Basis]));
					nodeRIDArray = new int[basisProf.BasisDetailProfileList.Count];
					i = 0;

					foreach (BasisDetailProfile basisDetProf in basisProf.BasisDetailProfileList)
					{
						nodeRIDArray[i] = basisDetProf.HierarchyNodeProfile.Key;
						i++;
					}

					//Begin TT#824 - JScott - Add Toolbox methods to support external node references
					//return aPlanCellRef.PlanCube.Transaction.GetOnHandReader().GetCurrentOnHand(nodeRIDArray, aPlanCellRef[eProfileType.Store]);
					return GetOnHandCellValue(aPlanCellRef, nodeRIDArray);
					//End TT#824 - JScott - Add Toolbox methods to support external node references
				}
				else
				{
					//Begin TT#824 - JScott - Add Toolbox methods to support external node references
					//return aPlanCellRef.PlanCube.Transaction.GetOnHandReader().GetCurrentOnHand(aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.Store]);
					nodeRIDArray = new int[1];
					nodeRIDArray[0] = aPlanCellRef[eProfileType.HierarchyNode];
					return GetOnHandCellValue(aPlanCellRef, nodeRIDArray);
					//End TT#824 - JScott - Add Toolbox methods to support external node references
				}
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		//Begin TT#824 - JScott - Add Toolbox methods to support external node references
		/// <summary>
		/// Gets the On Hand value for a given Node.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the PlanCubeCell being used.
		/// </param>
		/// <returns>
		/// The On Hand value.
		/// </returns>

		public double GetOnHandCellValue(PlanCellReference aPlanCellRef, string aNodeId)
		{
			int nodeRID;
			int[] nodeRIDArray;
			string message;

			try
			{
				nodeRID = aPlanCellRef.PlanCube.SAB.HierarchyServerSession.GetNodeRID(aNodeId);

				if (nodeRID != Include.NoRID)
				{
					nodeRIDArray = new int[1];
					nodeRIDArray[0] = nodeRID;
					return GetOnHandCellValue(aPlanCellRef, nodeRIDArray);
				}
				else
				{
					message = String.Format(MIDText.GetTextOnly(eMIDTextCode.msg_pl_InvalidNodeIdSpecified), aNodeId);

					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_InvalidNodeIdSpecified,
						message);
				}
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the On Hand value for a given Node.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the PlanCubeCell being used.
		/// </param>
		/// <returns>
		/// The On Hand value.
		/// </returns>

		public double GetOnHandCellValue(PlanCellReference aPlanCellRef, int[] aNodeRIDArray)
		{
			ArrayList storeRIDList;
			ArrayList cellRefArray;

			try
			{
				storeRIDList = new ArrayList();

				if (aPlanCellRef.PlanCube.isGroupTotalCube || aPlanCellRef.PlanCube.isStoreTotalCube)
				{
					cellRefArray = aPlanCellRef.GetDetailCellRefArray(aPlanCellRef.PlanCube.GetStoreDetailCubeType(), false);

					foreach (PlanCellReference planCellRef in cellRefArray)
					{
						storeRIDList.Add(planCellRef[eProfileType.Store]);
					}
				}
				else
				{
					storeRIDList.Add(aPlanCellRef[eProfileType.Store]);
				}

				if (storeRIDList.Count == 1)
				{
					if (aNodeRIDArray.Length == 1)
					{
						return aPlanCellRef.PlanCube.Transaction.GetOnHandReader().GetCurrentOnHand(aNodeRIDArray[0], (int)storeRIDList[0]);
					}
					else
					{
						return aPlanCellRef.PlanCube.Transaction.GetOnHandReader().GetCurrentOnHand(aNodeRIDArray, (int)storeRIDList[0]);
					}
				}
				else
				{
					if (storeRIDList.Count == 1)
					{
						return SumOnhands(aPlanCellRef.PlanCube.Transaction.GetOnHandReader().GetCurrentOnHand(aNodeRIDArray[0], storeRIDList));
					}
					else
					{
						return SumOnhands(aPlanCellRef.PlanCube.Transaction.GetOnHandReader().GetCurrentOnHand(aNodeRIDArray, storeRIDList));
					}
				}
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		private int SumOnhands(int[] aOnHands)
		{
			int totOnHand = 0;

			try
			{
				foreach (int onHand in aOnHands)
				{
					totOnHand += onHand;
				}

				return totOnHand;
			}
			catch (Exception exc)
			{
				throw;
			}
		}
		//End TT#824 - JScott - Add Toolbox methods to support external node references

        // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
        public double GetVSWOnHandCellValue(PlanCellReference aPlanCellRef)
        {
            BasisProfile basisProf;
            int i;
            int[] nodeRIDArray;

            try
            {
                if (isBasis(aPlanCellRef))
                {
                    basisProf = ((BasisProfile)aPlanCellRef.PlanCube.PlanCubeGroup.OpenParms.GetBasisProfileList(aPlanCellRef.PlanCube.PlanCubeGroup, aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.Version]).FindKey(aPlanCellRef[eProfileType.Basis]));
                    nodeRIDArray = new int[basisProf.BasisDetailProfileList.Count];
                    i = 0;

                    foreach (BasisDetailProfile basisDetProf in basisProf.BasisDetailProfileList)
                    {
                        nodeRIDArray[i] = basisDetProf.HierarchyNodeProfile.Key;
                        i++;
                    }

                    return GetVSWOnHandCellValue(aPlanCellRef, nodeRIDArray);
                }
                else
                {
                    nodeRIDArray = new int[1];
                    nodeRIDArray[0] = aPlanCellRef[eProfileType.HierarchyNode];
                    return GetVSWOnHandCellValue(aPlanCellRef, nodeRIDArray);
                }
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public double GetVSWOnHandCellValue(PlanCellReference aPlanCellRef, string aNodeId)
        {
            int nodeRID;
            int[] nodeRIDArray;
            string message;

            try
            {
                nodeRID = aPlanCellRef.PlanCube.SAB.HierarchyServerSession.GetNodeRID(aNodeId);

                if (nodeRID != Include.NoRID)
                {
                    nodeRIDArray = new int[1];
                    nodeRIDArray[0] = nodeRID;
                    return GetVSWOnHandCellValue(aPlanCellRef, nodeRIDArray);
                }
                else
                {
                    message = String.Format(MIDText.GetTextOnly(eMIDTextCode.msg_pl_InvalidNodeIdSpecified), aNodeId);

                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_pl_InvalidNodeIdSpecified,
                        message);
                }
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public double GetVSWOnHandCellValue(PlanCellReference aPlanCellRef, int[] aNodeRIDArray)
        {
            ArrayList storeRIDList;
            ArrayList cellRefArray;
            double vswValue = 0;
            try
            {
                storeRIDList = new ArrayList();

                if (aPlanCellRef.PlanCube.isGroupTotalCube || aPlanCellRef.PlanCube.isStoreTotalCube)
                {
                    cellRefArray = aPlanCellRef.GetDetailCellRefArray(aPlanCellRef.PlanCube.GetStoreDetailCubeType(), false);

                    foreach (PlanCellReference planCellRef in cellRefArray)
                    {
                        storeRIDList.Add(planCellRef[eProfileType.Store]);
                    }
                }
                else
                {
                    storeRIDList.Add(aPlanCellRef[eProfileType.Store]);
                }

                if (storeRIDList.Count == 1)
                {
                    if (aNodeRIDArray.Length == 1)
                    {
                        vswValue = aPlanCellRef.PlanCube.Transaction.GetIMOReader().GetStoreTotalIMO(aNodeRIDArray[0], (int)storeRIDList[0]);
                    }
                    else   // unsure if condition occurs
                    { 
                        foreach (int nodeRID in aNodeRIDArray)
                        {
                            vswValue += aPlanCellRef.PlanCube.Transaction.GetIMOReader().GetStoreTotalIMO(nodeRID, (int)storeRIDList[0]);
                        }
                    }
                }
                else      // unsure if condition occurs
                {
                    foreach (int storeRID in storeRIDList)
                    {
                        vswValue += aPlanCellRef.PlanCube.Transaction.GetIMOReader().GetStoreTotalIMO(aNodeRIDArray[0], storeRID);
                    }
                }
                return vswValue;
            }
            catch (Exception exc)
            {
                throw;
            }
        }
        // End TT#2054 
         

		/// <summary>
		/// Gets the Need value.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the PlanCubeCell being used.
		/// </param>
		/// <returns>
		/// The Need value.
		/// </returns>
		
		public double GetNeedCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aSalesVarProf,
			bool aUseHiddenValues)
		{
			ArrayList needRequestList;

			try
			{
				needRequestList = new ArrayList();

				CreateNeedRequestList(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aSalesVarProf, aUseHiddenValues, needRequestList);

				return aPlanCellRef.PlanCube.Transaction.GetStoreOTS_Need(
					(OTS_NeedRequest[])needRequestList.ToArray(typeof(OTS_NeedRequest)),
					aPlanCellRef[eProfileType.Store]);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the PctNeed value.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the PlanCubeCell being used.
		/// </param>
		/// <returns>
		/// The PctNeed value.
		/// </returns>
		
		public double GetPctNeedCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			TimeTotalVariableProfile aNeedVarProf,
			bool aUseHiddenValues)
		{
			ArrayList needRequestKeyList;

			try
			{
				needRequestKeyList = new ArrayList();

				CreateNeedRequestKeyList(aGetCellMode, aSetCellMode, aPlanCellRef, aUseHiddenValues, needRequestKeyList);

				return aPlanCellRef.PlanCube.Transaction.GetStoreOTS_PctNeed(
					(OTS_NeedRequestKey[])needRequestKeyList.ToArray(typeof(OTS_NeedRequestKey)),
					aPlanCellRef[eProfileType.Store],
					GetTimeTotalOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aNeedVarProf, aUseHiddenValues));
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the PctNeed value.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the PlanCubeCell being used.
		/// </param>
		/// <returns>
		/// The PctNeed value.
		/// </returns>
		
		public double GetPctNeedCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			ArrayList aPlanCellRefList,
			int[] aStoreRIDArr,
			TimeTotalVariableProfile aNeedVarProf,
			bool aUseHiddenValues)
		{
			ArrayList needRequestKeyList;

			try
			{
				needRequestKeyList = new ArrayList();

				foreach (PlanCellReference planCellRef in aPlanCellRefList)
				{
					CreateNeedRequestKeyList(aGetCellMode, aSetCellMode, planCellRef, aUseHiddenValues, needRequestKeyList);
				}

				return aPlanCellRef.PlanCube.Transaction.GetTotalStoreOTS_PctNeed(
					(OTS_NeedRequestKey[])needRequestKeyList.ToArray(typeof(OTS_NeedRequestKey)),
					aStoreRIDArr,
					GetTimeTotalOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aNeedVarProf, aUseHiddenValues));
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the PctNeed value.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the PlanCubeCell being used.
		/// </param>
		/// <returns>
		/// The PctNeed value.
		/// </returns>
		
		public double GetPctNeedCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			int[] aStoreRIDArr,
			TimeTotalVariableProfile aNeedVarProf,
			bool aUseHiddenValues)
		{
			ArrayList needRequestKeyList;

			try
			{
				needRequestKeyList = new ArrayList();

				CreateNeedRequestKeyList(aGetCellMode, aSetCellMode, aPlanCellRef, aUseHiddenValues, needRequestKeyList);

				return aPlanCellRef.PlanCube.Transaction.GetTotalStoreOTS_PctNeed(
					(OTS_NeedRequestKey[])needRequestKeyList.ToArray(typeof(OTS_NeedRequestKey)),
					aStoreRIDArr,
					GetTimeTotalOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aNeedVarProf, aUseHiddenValues));
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		public void CreateNeedRequestList(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aSalesVarProf,
			bool aUseHiddenValues,
			ArrayList aNeedRequestList)
		{
			MRSCalendar calendar;
			BasisProfile basisProf;
			StoreBasisDetail storeBasisDetail;
			PlanCellReference detPlanCellRef;

			ProfileList dateProfList;
			WeekProfile beginWeek;
			WeekProfile currWeek;
			WeekProfile endWeek;
			double endInv;
			double currInv;
			double currSales;
			decimal partSales;
			int i;

			try
			{
				calendar = aPlanCellRef.PlanCube.SAB.ApplicationServerSession.Calendar;

				if (isBasis(aPlanCellRef))
				{
					basisProf = ((BasisProfile)aPlanCellRef.PlanCube.PlanCubeGroup.OpenParms.GetBasisProfileList(aPlanCellRef.PlanCube.PlanCubeGroup, aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.Version]).FindKey(aPlanCellRef[eProfileType.Basis]));
					storeBasisDetail = (StoreBasisDetail)aPlanCellRef.PlanCube.PlanCubeGroup.GetCube(eCubeType.StoreBasisDetail);

					foreach (BasisDetailProfile basisDetProf in basisProf.BasisDetailProfileList)
					{
						detPlanCellRef = (PlanCellReference)storeBasisDetail.CreateCellReference(aPlanCellRef);
						detPlanCellRef[eProfileType.BasisDetail] = basisDetProf.Key;
						detPlanCellRef[eProfileType.BasisHierarchyNode] = basisDetProf.HierarchyNodeProfile.Key;
						detPlanCellRef[eProfileType.BasisVersion] = basisDetProf.VersionProfile.Key;
						dateProfList = basisDetProf.GetDetailDateProfileList(aPlanCellRef.PlanCube.SAB.ApplicationServerSession);

						if (dateProfList.ProfileType == eProfileType.Week)
						{
							beginWeek = (WeekProfile)dateProfList[0];
							endWeek = (WeekProfile)dateProfList[dateProfList.Count - 1];
						}
						else
						{
							beginWeek = (WeekProfile)((PeriodProfile)dateProfList[0]).Weeks[0];
							endWeek = (WeekProfile)((PeriodProfile)dateProfList[dateProfList.Count - 1]).Weeks[0];
						}

						if (isCurrentPostingWeekInPlan(aPlanCellRef))
						{
							currWeek = calendar.CurrentWeek;
						}
						else
						{
							currWeek = beginWeek;
						}

						detPlanCellRef[eProfileType.Week] = endWeek.Key;
						endInv = detPlanCellRef.CurrentCellValue;

						detPlanCellRef[eProfileType.Week] = currWeek.Key;
						currInv = detPlanCellRef.CurrentCellValue;

						detPlanCellRef[eProfileType.Week] = currWeek.Key;
						detPlanCellRef[eProfileType.Variable] = aSalesVarProf.Key;
						currSales = detPlanCellRef.CurrentCellValue;

						detPlanCellRef[eProfileType.Variable] = aSalesVarProf.Key;

						partSales = 0;
						for (i = calendar.AddWeeks(currWeek.Key, 1); i <= calendar.AddWeeks(endWeek.Key, -1); i = detPlanCellRef.PlanCube.SAB.ApplicationServerSession.Calendar.AddWeeks(i, 1))
						{
							detPlanCellRef[eProfileType.Week] = i;
							partSales += (decimal)detPlanCellRef.CurrentCellValue;
						}

						aNeedRequestList.Add(new OTS_NeedRequest(
							basisDetProf.HierarchyNodeProfile.Key,
							basisDetProf.VersionProfile.Key,
							beginWeek,
							endWeek,
							endInv,
							(double)partSales,
							currInv,
							currSales));
					}
				}
				else
				{
					dateProfList = aPlanCellRef.PlanCube.PlanCubeGroup.OpenParms.GetDetailDateProfileList(aPlanCellRef.PlanCube.SAB.ApplicationServerSession);

					if (dateProfList.ProfileType == eProfileType.Week)
					{
						beginWeek = (WeekProfile)dateProfList[0];
						endWeek = (WeekProfile)dateProfList[dateProfList.Count - 1];
					}
					else
					{
						beginWeek = (WeekProfile)((PeriodProfile)dateProfList[0]).Weeks[0];
						endWeek = (WeekProfile)((PeriodProfile)dateProfList[dateProfList.Count - 1]).Weeks[0];
					}

					if (isCurrentPostingWeekInPlan(aPlanCellRef))
					{
						currWeek = calendar.CurrentWeek;
					}
					else
					{
						currWeek = beginWeek;
					}

					aNeedRequestList.Add(new OTS_NeedRequest(
						aPlanCellRef[eProfileType.HierarchyNode],
						aPlanCellRef[eProfileType.Version],
						beginWeek,
						endWeek,
						GetWeekOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, endWeek.Key, aUseHiddenValues),
						GetWeekOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aSalesVarProf, calendar.AddWeeks(currWeek.Key, 1), calendar.AddWeeks(endWeek.Key, -1), true),
						GetWeekOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, currWeek.Key, aUseHiddenValues),
						GetWeekOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aSalesVarProf, currWeek.Key, aUseHiddenValues)));
				}
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		public void CreateNeedRequestKeyList(eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, PlanCellReference aPlanCellRef, bool aUseHiddenValues, ArrayList aNeedRequestList)
		{
			MRSCalendar calendar;
			BasisProfile basisProf;

			ProfileList dateProfList;
			WeekProfile beginWeek;
			WeekProfile currWeek;
			WeekProfile endWeek;

			try
			{
				calendar = aPlanCellRef.PlanCube.SAB.ApplicationServerSession.Calendar;

				if (isBasis(aPlanCellRef))
				{
					basisProf = ((BasisProfile)aPlanCellRef.PlanCube.PlanCubeGroup.OpenParms.GetBasisProfileList(aPlanCellRef.PlanCube.PlanCubeGroup, aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.Version]).FindKey(aPlanCellRef[eProfileType.Basis]));

					foreach (BasisDetailProfile basisDetProf in basisProf.BasisDetailProfileList)
					{
						dateProfList = basisDetProf.GetDetailDateProfileList(aPlanCellRef.PlanCube.SAB.ApplicationServerSession);

						if (dateProfList.ProfileType == eProfileType.Week)
						{
							beginWeek = (WeekProfile)dateProfList[0];
							endWeek = (WeekProfile)dateProfList[dateProfList.Count - 1];
						}
						else
						{
							beginWeek = (WeekProfile)((PeriodProfile)dateProfList[0]).Weeks[0];
							endWeek = (WeekProfile)((PeriodProfile)dateProfList[dateProfList.Count - 1]).Weeks[0];
						}

						if (isCurrentPostingWeekInPlan(aPlanCellRef))
						{
							currWeek = calendar.CurrentWeek;
						}
						else
						{
							currWeek = beginWeek;
						}

						aNeedRequestList.Add(new OTS_NeedRequestKey(
							basisDetProf.HierarchyNodeProfile.Key,
							basisDetProf.VersionProfile.Key,
							beginWeek,
							endWeek));
					}
				}
				else
				{
					dateProfList = aPlanCellRef.PlanCube.PlanCubeGroup.OpenParms.GetDetailDateProfileList(aPlanCellRef.PlanCube.SAB.ApplicationServerSession);

					if (dateProfList.ProfileType == eProfileType.Week)
					{
						beginWeek = (WeekProfile)dateProfList[0];
						endWeek = (WeekProfile)dateProfList[dateProfList.Count - 1];
					}
					else
					{
						beginWeek = (WeekProfile)((PeriodProfile)dateProfList[0]).Weeks[0];
						endWeek = (WeekProfile)((PeriodProfile)dateProfList[dateProfList.Count - 1]).Weeks[0];
					}

					if (isCurrentPostingWeekInPlan(aPlanCellRef))
					{
						currWeek = calendar.CurrentWeek;
					}
					else
					{
						currWeek = beginWeek;
					}

					aNeedRequestList.Add(new OTS_NeedRequestKey(
						aPlanCellRef[eProfileType.HierarchyNode],
						aPlanCellRef[eProfileType.Version],
						beginWeek,
						endWeek));
				}
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the WTD Regular Sales value.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the PlanCubeCell being used.
		/// </param>
		/// <returns>
		/// The WTD Regular Sales value.
		/// </returns>
		
		public double GetWTDSalesCellValue(PlanCellReference aPlanCellRef, ProfileList aVariableList)
		{
			BasisProfile basisProf;
			int totWTDSales;

			try
			{
				if (isBasis(aPlanCellRef))
				{
					basisProf = ((BasisProfile)aPlanCellRef.PlanCube.PlanCubeGroup.OpenParms.GetBasisProfileList(aPlanCellRef.PlanCube.PlanCubeGroup, aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.Version]).FindKey(aPlanCellRef[eProfileType.Basis]));
					totWTDSales = 0;

					foreach (BasisDetailProfile basisDetProf in basisProf.BasisDetailProfileList)
					{
						totWTDSales += aPlanCellRef.PlanCube.Transaction.GetStoreCurrentWeekToDaySales(basisDetProf.HierarchyNodeProfile.Key, aVariableList, aPlanCellRef[eProfileType.Store]);
					}
					
					return totWTDSales;
				}
				else
				{
					return aPlanCellRef.PlanCube.Transaction.GetStoreCurrentWeekToDaySales(aPlanCellRef[eProfileType.HierarchyNode], aVariableList, aPlanCellRef[eProfileType.Store]);
				}
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Marks the given ComputationCellReference as a null cell.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference of the cell to mark as null.
		/// </param>

		public void SetCellDisplayOnly(ComputationCellReference aCompCellRef, bool aValue)
		{
			PlanCube planCube;
			PlanCellReference planCellRef;
			VariableProfile varProf;
			bool protect;

			try
			{
				planCube = (PlanCube)aCompCellRef.ComputationCube;
				planCellRef = (PlanCellReference)aCompCellRef;
				protect = planCube.isVersionProtected(planCellRef);
				varProf = (VariableProfile)planCube.MasterVariableProfileList.FindKey(planCellRef[planCellRef.VariableProfileType]);

				if (planCellRef.PlanCube.isDatabaseVariable(varProf, planCellRef))
				{
					planCellRef.isCellDisplayOnly = aValue;
				}
				else
				{
					planCellRef.isCellDisplayOnly = aValue | protect;
				}
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Sets a result PlanCubeCell with the grade value.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that identifies the PlanCubeCell being changed.
		/// </param>
		/// <param name="aStoreSales">
		/// The double containing the store sales.
		/// </param>
		/// <param name="aAverageStoreSales">
		/// The double containing the average store sales.
		/// </param>
		
		public void SetGradeCellValue
			(eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			double aStoreSales,
			double aAverageStoreSales)
		{
			StoreGradeList sgl;

			try
			{
				sgl = aPlanCellRef.PlanCube.Transaction.GetStoreGradeList(aPlanCellRef.GetHierarchyNodeProfile().Key);

				if (sgl.Count > 0)
				{
					SetCellValue(aSetCellMode, aPlanCellRef, System.Convert.ToDouble(StoreGrade.GetGradeProfileKey(sgl, aStoreSales, aAverageStoreSales)));
				}
				else
				{
					SetCellNull(aPlanCellRef);
				}
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Adds up the values of all the detail cells for the given PlanCellReference.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the cell to sum the detail for.
		/// </param>
		/// <returns>
		/// A double containing the sum of the detail cells.
		/// </returns>

		public double SumDetailComponents(ComputationScheduleEntry aScheduleEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, PlanCellReference aPlanCellRef)
		{
			try
			{
				return aPlanCellRef.GetComponentDetailSum(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef.isCellHidden);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Adds up the values of all the detail cells for the given PlanCellReference.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the cell to sum the detail for.
		/// </param>
		/// <returns>
		/// A double containing the sum of the detail cells.
		/// </returns>

		public double SumDetailComponents(ComputationScheduleEntry aScheduleEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, PlanCellReference aPlanCellRef, QuantityVariableProfile aQuanVarProf, eStoreStatus aStoreStatus)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Copy();
				planCellRef[eProfileType.QuantityVariable] = aQuanVarProf.Key;
				return planCellRef.GetComponentDetailSum(aScheduleEntry, aGetCellMode, aSetCellMode, aStoreStatus, planCellRef.isCellHidden);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Computes the average of an ArrayList of cells.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to calculate the average for.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the detail cube.
		/// </param>
		/// <param name="aQuanVarProf">
		/// The QuantityVariableProfile for the Quantity variable of the detail cells.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if the detail cells should be checked for pending.
		/// </param>
		/// <returns>
		/// A double containing the average.
		/// </returns>

		public double SumDetailComponents(ComputationScheduleEntry aScheduleEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, PlanCellReference aPlanCellRef, eCubeType aCubeType, eStoreStatus aStoreStatus, QuantityVariableProfile aQuanVarProf)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Copy();
				planCellRef[eProfileType.QuantityVariable] = aQuanVarProf.Key;
				return planCellRef.GetDetailSum(aScheduleEntry, aGetCellMode, aSetCellMode, intGetCubeType(planCellRef, aCubeType), aStoreStatus, planCellRef.isCellHidden);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Calculation for a FWOS formula.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The Cell to calculate the FWOS for.
		/// </param>
		/// <param name="aInventoryVar">
		/// The VariableProfile that contains the Inventory variable.
		/// </param>
		/// <param name="aSalesVar">
		/// The VariableProfile that contains the Sales variable.
		/// </param>
		/// <returns>
		/// The return value for the FWOS calculation.
		/// </returns>

		public double CalculateFWOS(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aInventoryVar,
			VariableProfile aSalesVar)
		{
			try
			{
				return CalculateFWOS(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aSalesVar, GetOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aInventoryVar, true));
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Calculation for a FWOS formula.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The Cell to calculate the FWOS for.
		/// </param>
		/// <param name="aInventoryVar">
		/// The VariableProfile that contains the Inventory variable.
		/// </param>
		/// <param name="aSalesVar">
		/// The VariableProfile that contains the Sales variable.
		/// </param>
		/// <returns>
		/// The return value for the FWOS calculation.
		/// </returns>

		public double CalculateFWOS(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aSalesVar,
			double aInventoryValue)
		{
			double FWOS;
			double sales;
			int time;
			int maxZeroSalesTimeDetails;
			int zeroSalesTimeDetails;

			try
			{
				if (aInventoryValue > 0)
				{
					FWOS = 0;
					time = GetCurrentPlanTimeDetail(aPlanCellRef);

					if (aPlanCellRef.GetStoreStatus() == eStoreStatus.Preopen)
					{
						maxZeroSalesTimeDetails = 52;
					}
					else
					{
						maxZeroSalesTimeDetails = 5;
					}

					zeroSalesTimeDetails = 0;

					while (aInventoryValue > 0 && FWOS < 52)
					{
						sales = GetOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aSalesVar, time, true);
						// Begin TT#1954-MD - JSmith - Assortment Performance
                        if (aScheduleEntry.ComputationFormulaReturnType == eComputationFormulaReturnType.Pending)
                        {
                            return 0;
                        }
						// End TT#1954-MD - JSmith - Assortment Performance

						if (sales == 0)
						{
							zeroSalesTimeDetails++;
						}
						else
						{
							zeroSalesTimeDetails = 0;
						}

						if (zeroSalesTimeDetails == maxZeroSalesTimeDetails)
						{
							break;
						}

						if (aInventoryValue >= sales)
						{
							aInventoryValue -= sales;
							FWOS++;
						}
						else
						{
							FWOS = (double)(decimal)(FWOS + (aInventoryValue / sales));
							aInventoryValue = 0;
						}

						time = aPlanCellRef.PlanCube.IncrementTimeKey(time, 1);
					}

					if (aInventoryValue > 0)
					{
						FWOS = 999;
					}

					return FWOS;
				}
				else
				{
					return 0;
				}
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Calculation for a Stock value from a FWOS formula.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The Cell to calculate the Stock for.
		/// </param>
		/// <param name="aFWOSVar">
		/// The VariableProfile that contains the FWOS variable.
		/// </param>
		/// <param name="aSalesVar">
		/// The VariableProfile that contains the Sales variable.
		/// </param>
		/// <returns>
		/// The return value for a Stock value from the FWOS formula.
		/// </returns>

		public double CalculateStockFromFWOS(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aFWOSVar,
			VariableProfile aSalesVar)
		{
			double FWOS;
			double inventory;
			double sales;
			int time;

			try
			{
				FWOS = GetOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aFWOSVar, true);

				if (FWOS > 0)
				{
					inventory = 0;
					time = GetCurrentPlanTimeDetail(aPlanCellRef);

					while (FWOS > 0)
					{
						sales = GetOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aSalesVar, time, true);

						if (FWOS >= 1)
						{
							inventory += sales;
							FWOS--;
						}
						else
						{
							inventory += (double)(decimal)Math.Round((double)(sales * FWOS), 0);
							FWOS = 0;
						}

						time = aPlanCellRef.PlanCube.IncrementTimeKey(time, 1);
					}

					return inventory;
				}
				else
				{
					return 0;
				}
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Computes the average of an ArrayList of cells.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to calculate the average for.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the detail cube.
		/// </param>
		/// <param name="aQuanVarProf">
		/// The QuantityVariableProfile for the Quantity variable of the detail cells.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if the detail cells should be checked for pending.
		/// </param>
		/// <returns>
		/// A double containing the average.
		/// </returns>

		public double CalculateAverage(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			eStoreStatus aStoreStatus,
			QuantityVariableProfile aQuanVarProf)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Copy();
				planCellRef[eProfileType.QuantityVariable] = aQuanVarProf.Key;
				return planCellRef.GetDetailAverage(aScheduleEntry, aGetCellMode, aSetCellMode, intGetCubeType(planCellRef, aCubeType), aStoreStatus, planCellRef.isCellHidden);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Computes the non-zero average of an ArrayList of cells.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to calculate the average for.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the detail cube.
		/// </param>
		/// <param name="aQuanVarProf">
		/// The QuantityVariableProfile for the Quantity variable of the detail cells.
		/// </param>
		/// <param name="aInventoryVar">
		/// A VariableProfile that defines the inventory variable.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if the detail cells should be checked for pending.
		/// </param>
		/// <returns>
		/// A double containing the average.
		/// </returns>

		public double CalculateNonZeroAverage(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			eCubeType aCubeType,
			eStoreStatus aStoreStatus,
			QuantityVariableProfile aQuanVarProf,
			VariableProfile aInventoryVar)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aPlanCellRef.Copy();
				planCellRef[eProfileType.QuantityVariable] = aQuanVarProf.Key;
				return planCellRef.GetDetailNonZeroAverage(aScheduleEntry, aGetCellMode, aSetCellMode, intGetCubeType(planCellRef, aCubeType), aStoreStatus, aInventoryVar, aPlanCellRef.isCellHidden);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns the average for a actual value
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// The eSetCellMode that describes the type of change being made.
		/// </param>
		/// <param name="aCellRefArray">
		/// The ArrayList of cells.
		/// </param>
		/// <param name="aVariableProfile">
		/// A VariableProfile that defines the variable for the PlanCellReference.
		/// </param>
		/// The average for a actual value
		/// <returns></returns>

		public double CalculateActualCurrentWeekAverage(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			PlanCellReference aPlanCellRef,
			VariableProfile aVariableProfile)
		{
			double divisor;
			ArrayList cellRefList;
			int i;
			double newValue;

			try
			{
				if (aVariableProfile.VariableWeekType == eVariableWeekType.EOW)
				{
					divisor = aPlanCellRef.GetAverageDivisor();
				}
				else
				{
					divisor = System.Math.Ceiling(aPlanCellRef.GetAverageDivisor());
				}

				if (divisor > 0)
				{
					cellRefList = aPlanCellRef.GetComponentDetailCellRefArray(false);
					newValue = 0;

					for (i = 0; i < System.Math.Ceiling(divisor); i++)
					{
						newValue += this.GetOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, (PlanCellReference)cellRefList[i], false);
					}

					return newValue / divisor;
				}
				else
				{
					return 0;
				}
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves the intransit value for the given PlanCellReference.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the cell to retrieve the intransit value for.
		/// </param>
		/// <returns>
		/// The intrasit value of the cell.
		/// </returns>

		public double GetWeeklyIntransit(PlanCellReference aPlanCellRef)
		{
			try
			{
				return GetWeeklyIntransit(aPlanCellRef, aPlanCellRef[eProfileType.Store]);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves the intransit value for the given PlanCellReference.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the cell to retrieve the intransit value for.
		/// </param>
		/// <returns>
		/// The intrasit value of the cell.
		/// </returns>

		public double GetWeeklyIntransit(PlanCellReference aPlanCellRef, int aStoreRID)
		{
			try
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.Transaction.GetIntransitReader().GetStoreTotalWeekIntransit(
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.GetWeek(aPlanCellRef[eProfileType.Week]),
					aPlanCellRef.GetHierarchyNodeProfile().Key,
					aStoreRID);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Determines the eStoreStatus for the given PlanCellReference.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference for the cell to determine the store status for.
		/// </param>
		/// <returns>
		/// A double indicating the eStoreStatus value.
		/// </returns>

		public double DetermineStoreStatus(PlanCellReference aPlanCellRef)
		{
			try
			{
				return (double)aPlanCellRef.GetStoreStatus();
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// The PlanCellReference for the cell to look up the total for.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to find the total for.
		/// </param>
		/// <returns>
		/// The PlanCellReference to the total cell in the requested cube.
		/// </returns>

		protected PlanCellReference intGetPlanCellReferenceFromTotalCube(PlanCellReference aPlanCellRef, eCubeType aCubeType)
		{
			eCubeType cubeType;
			ArrayList arrList;
			PlanCellReference planCellRef;

			try
			{
				cubeType = intGetCubeType(aPlanCellRef, aCubeType);

				arrList = aPlanCellRef.GetTotalCellRefArray(cubeType);

				if (arrList.Count == 1)
				{
					planCellRef = (PlanCellReference)arrList[0];
				}
				else
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_InvalidTotalRelationship,
						MIDText.GetText(eMIDTextCode.msg_pl_InvalidTotalRelationship));
				}

				return planCellRef;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns an eCubeType that indicates the cube type for the given eCubeType for the given PlanCellReference.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the cell to find the cube type for.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType to translate.
		/// </param>
		/// <returns>
		/// The eCubeType of the translated cube.
		/// </returns>

		protected eCubeType intGetCubeType(PlanCellReference aPlanCellRef, eCubeType aCubeType)
		{
			eCubeType cubeType;

			try
			{
				switch (aCubeType.Id)
				{
					case eCubeType.cChainDetail:
						cubeType = aPlanCellRef.PlanCube.GetChainDetailCubeType();
						break;
					case eCubeType.cGroupTotal:
						cubeType = aPlanCellRef.PlanCube.GetGroupTotalCubeType();
						break;
					case eCubeType.cStoreTotal:
						cubeType = aPlanCellRef.PlanCube.GetStoreTotalCubeType();
						break;
					case eCubeType.cDateTotal:
						cubeType = aPlanCellRef.PlanCube.GetDateTotalCubeType();
						break;
					case eCubeType.cLowLevelTotal:
						cubeType = aPlanCellRef.PlanCube.GetLowLevelTotalCubeType();
						break;
					case eCubeType.cStoreDetail:
						cubeType = aPlanCellRef.PlanCube.GetStoreDetailCubeType();
						break;
					case eCubeType.cLowLevelDetail:
						cubeType = aPlanCellRef.PlanCube.GetLowLevelDetailCubeType();
						break;
					case eCubeType.cWeekDetail:
						cubeType = aPlanCellRef.PlanCube.GetWeekDetailCubeType();
						break;
					case eCubeType.cBasis:
						cubeType = aPlanCellRef.PlanCube.GetBasisCubeType();
						break;
					case eCubeType.cPlan:
						cubeType = aPlanCellRef.PlanCube.GetPlanCubeType();
						break;
					default:
						cubeType = aCubeType;
						break;
				}

				if (cubeType != eCubeType.None)
				{
					return cubeType;
				}
				else
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_CubeTypeNotDetermined,
						MIDText.GetText(eMIDTextCode.msg_pl_CubeTypeNotDetermined));
				}
			}
			catch (Exception exc)
			{
				throw;
			}
        }
    
        // Begin TT# 843-MD - RMatelic - Combine all PCF database into one common database for all clients
        #region Common Methods moved from various clients' DefaultPlanToolBox
        public double GetActualOperandCellValue(
           ComputationScheduleEntry aScheduleEntry,
           eGetCellMode aGetCellMode,
           eSetCellMode aSetCellMode,
           PlanCellReference aPlanCellRef,
           VariableProfile aVariableProfile,
           bool aCheckPending)
        {
            PlanCellReference planCellRef;

            try
            {
                planCellRef = (PlanCellReference)aPlanCellRef.Copy();
                planCellRef[eProfileType.Variable] = aVariableProfile.Key;
                planCellRef[planCellRef.PlanCube.GetVersionType()] = Include.FV_ActualRID;
                if (aCheckPending)
                {
                    intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
                }
                return planCellRef.GetCellValue(aGetCellMode, planCellRef.isCellHidden);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        // TT#4277-JAB - RMatelic - PCF Custom Calcs - new base method for actual value that includes timeId
        public double GetActualOperandCellValue(
           ComputationScheduleEntry aScheduleEntry,
           eGetCellMode aGetCellMode,
           eSetCellMode aSetCellMode,
           PlanCellReference aPlanCellRef,
           VariableProfile aVariableProfile,
           int aTimeId, 
           bool aCheckPending)
        {
            PlanCellReference planCellRef;

            try
            {
                planCellRef = (PlanCellReference)aPlanCellRef.Copy();
                planCellRef[eProfileType.Variable] = aVariableProfile.Key;
                planCellRef[planCellRef.PlanCube.GetVersionType()] = Include.FV_ActualRID;
                planCellRef[planCellRef.PlanCube.GetTimeType()] = aTimeId;
                if (aCheckPending)
                {
                    intCheckOperandCell(aScheduleEntry, aSetCellMode, planCellRef);
                }
                return planCellRef.GetCellValue(aGetCellMode, planCellRef.isCellHidden);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#4277

        public double AverageTotalDetailComponentsMinusLast(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, PlanCellReference aPlanCellRef, double aTotalVarValue)
        {
            ComputationCellSum cellSelector;

            try
            {
                cellSelector = new ComputationCellSum(aSchdEntry, aGetCellMode, aSetCellMode, true);
                aPlanCellRef.PlanCube.ProcessComponentDetailCellSelector(aPlanCellRef, cellSelector);
                return (aTotalVarValue + cellSelector.Sum - cellSelector.LastComputationCellReference.GetCellValue(aGetCellMode, true)) / cellSelector.Count;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public double AverageTotalDetailComponents(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, PlanCellReference aPlanCellRef)
        {
            ComputationCellSum cellSelector;

            try
            {
                cellSelector = new ComputationCellSum(aSchdEntry, aGetCellMode, aSetCellMode, true);
                aPlanCellRef.PlanCube.ProcessComponentDetailCellSelector(aPlanCellRef, cellSelector);
                return cellSelector.Sum / cellSelector.Count;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public double AverageSumTotalDetailComponents(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, PlanCellReference aPlanCellRef1, PlanCellReference aPlanCellRef2)
        {
            ComputationCellSum cellSelector1;
            ComputationCellSum cellSelector2;

            try
            {
                cellSelector1 = new ComputationCellSum(aSchdEntry, aGetCellMode, aSetCellMode, true);
                aPlanCellRef1.PlanCube.ProcessComponentDetailCellSelector(aPlanCellRef1, cellSelector1);
                cellSelector2 = new ComputationCellSum(aSchdEntry, aGetCellMode, aSetCellMode, true);
                aPlanCellRef2.PlanCube.ProcessComponentDetailCellSelector(aPlanCellRef2, cellSelector2);

                return (cellSelector1.Sum + cellSelector2.Sum) / cellSelector1.Count;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public double AverageTotalDetailComponentsWithCellCopy(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, PlanCellReference aPlanCellRef, int aProfileKey)
        {
            try
            {
                PlanCellReference planCellRef = CopyPlanCellRefWithNewKey(aSchdEntry.PlanCellRef, aProfileKey);
                return AverageTotalDetailComponents(aSchdEntry, aGetCellMode, aSetCellMode, planCellRef);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public PlanCellReference CopyPlanCellRefWithNewKey(PlanCellReference aPlanCellRef, int aProfileKey)
        {
            PlanCellReference planCellRef = null;

            try
            {
                planCellRef = (PlanCellReference)aPlanCellRef.Copy();
                planCellRef[planCellRef.VariableProfileType] = aProfileKey;

                return planCellRef;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        
        public double CalculateFWOSInv(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, VariableProfile aInventoryVar, VariableProfile aSalesVar, int aBeginTimeId)
        {
            double FWOS;
            double inventory;
            double sales;
            int time;
            PlanCellReference planCellRef = aSchdEntry.PlanCellRef;
            try
            {
                // Begin Change for Retro
                //FWOS = aFWOS;
                FWOS = GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, planCellRef, aInventoryVar, aSchdEntry.PlanCellRef.isCellHidden);
                // End Change for Retro
                time = aBeginTimeId;
                if ((FWOS > 0))
                {
                    inventory = 0;

                    while (FWOS > 0)
                    {
                        sales = GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, planCellRef, aSalesVar, time, true);

                        if ((FWOS >= 1))
                        {
                            inventory += sales;
                            FWOS--;
                        }
                        else
                        {
                            inventory += (double)(decimal)Math.Round((double)(sales * FWOS), 0);
                            FWOS = 0;
                        }

                        time = planCellRef.PlanCube.IncrementTimeKey(time, 1);
                    }

                    return inventory;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public double CalcFWOS(ComputationScheduleEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, PlanCellReference aPlanCellRef, VariableProfile aInventoryVar, VariableProfile aSalesVar)
        {
            double FWOS;
            double inventory;
            double sales;

            int time;

            try
            {
                inventory = GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aInventoryVar, aPlanCellRef.isCellHidden);

                if ((inventory > 0))
                {
                    FWOS = 0;
                    time = GetCurrentPlanTimeDetail(aPlanCellRef);
                    while (inventory > 0 && FWOS < 80)
                    {
                        sales = GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aSalesVar, time, true);


                        if ((sales == 0))
                        {
                            int tempTimeId = GetCurrentPlanTimeDetail(aPlanCellRef);
                            int salesCounter = 0;
                            double salesVar = 0;
                            while (tempTimeId < time)
                            {
                                salesVar = GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aSalesVar, tempTimeId, true);
                                if ((salesVar > 0))
                                {
                                    sales += salesVar;
                                    salesCounter++;
                                }
                                tempTimeId = aPlanCellRef.PlanCube.IncrementTimeKey(tempTimeId, 1);
                            }
                            if ((salesCounter > 0))
                            {
                                sales = sales / salesCounter;
                            }
                        }

                        if ((sales == 0))
                        {
                            break;
                        }

                        if ((inventory >= sales))
                        {
                            inventory -= sales;
                            // Begin TT#4895 - RMatelic - Error Opening a Plan
                            //FWOS++;
                            if (aPlanCellRef.PlanCube.isPeriodDetailCube)
                            {
                                PeriodProfile periodProf = aPlanCellRef.PlanCube.SAB.ApplicationServerSession.Calendar.GetPeriod(aPlanCellRef[eProfileType.Period]);
                                FWOS = FWOS + periodProf.NoOfWeeks;
                            }
                            else
                            {
                                FWOS++;
                            }
                            // End TT#4895
                        }
                        else
                        {
                            FWOS = (double)(decimal)(FWOS + (inventory / sales));
                            inventory = 0;
                        }

                        time = aPlanCellRef.PlanCube.IncrementTimeKey(time, 1);
                    }

                    if ((inventory > 0))
                    {
                        FWOS = 999.9;
                    }

                    return FWOS;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public double CalculateFWOS2(ComputationScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, PlanCellReference aPlanCellRef, VariableProfile aInventoryVar, VariableProfile aSalesVar1, VariableProfile aSalesVar2)
        {
            double FWOS;
            double sales;
            int time;
            int maxZeroSalesTimeDetails;
            int zeroSalesTimeDetails;
            double aInventoryValue;
            ComputationCellReference compCellRef = aSchdEntry.ComputationCellRef;
            try
            {
                aInventoryValue = GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, compCellRef, aInventoryVar, aSchdEntry.ComputationCellRef.isCellHidden);
                if (aInventoryValue > 0)
                {
                    FWOS = 0;
                    time = GetCurrentPlanTimeDetail(aPlanCellRef);

                    if (aPlanCellRef.GetStoreStatus() == eStoreStatus.Preopen)
                    {
                        maxZeroSalesTimeDetails = 80;
                    }
                    else
                    {
                        maxZeroSalesTimeDetails = 5;
                    }

                    zeroSalesTimeDetails = 0;

                    while (aInventoryValue > 0 && FWOS < 80)
                    {
                        sales = GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aSalesVar1, time, true) +
                                GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aPlanCellRef, aSalesVar2, time, true);

                        if (sales == 0)
                        {
                            zeroSalesTimeDetails++;
                        }
                        else
                        {
                            zeroSalesTimeDetails = 0;
                        }

                        if (zeroSalesTimeDetails == maxZeroSalesTimeDetails)
                        {
                            break;
                        }

                        if (aInventoryValue >= sales)
                        {
                            aInventoryValue -= sales;
                            FWOS++;
                        }
                        else
                        {
                            FWOS = (double)(decimal)(FWOS + (aInventoryValue / sales));
                            aInventoryValue = 0;
                        }

                        time = aPlanCellRef.PlanCube.IncrementTimeKey(time, 1);
                    }

                    if (aInventoryValue > 0)
                    {
                        FWOS = 999;
                    }
                    return FWOS;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        #endregion
        // End TT#843-MD

        // Begin TT#4958 - RMatelic - Editing Numbers Issue
        public bool VersionHasProtectHistory(ComputationCellReference aCompCellRef)
        {
            try
            {
                return ((PlanCellReference)aCompCellRef).GetVersionProfileOfData().ProtectHistory;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#TT4958
    }
}
