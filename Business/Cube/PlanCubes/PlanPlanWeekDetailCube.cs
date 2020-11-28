using System;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// This class defines the PlanPlanWeekDetailCube.
	/// </summary>
	/// <remarks>
	/// The PlanPlanWeekDetailCube defines a cube that contains the plan week detail values.
	/// </remarks>

	abstract public class PlanPlanWeekDetailCube : PlanPlanCube
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of PlanPlanWeekDetailCube, using the given SessionAddressBlock, Transaction, and PlanCubeGroup.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this PlanPlanWeekDetailCube is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this PlanPlanWeekDetailCube is a part of.
		/// </param>
		/// <param name="aPlanCubeGroup">
		/// A reference to a PlanCubeGroup that this PlanPlanWeekDetailCube is a part of.
		/// </param>

		public PlanPlanWeekDetailCube(
			SessionAddressBlock aSAB,
			ApplicationSessionTransaction aTransaction,
			PlanCubeGroup aPlanCubeGroup,
			CubeDefinition aCubeDef,
			ushort aCubeAttributes,
			int aCubePriority,
			bool aReadOnly,
			bool aCheckNodeSecurity)

			: base(aSAB, aTransaction, aPlanCubeGroup, aCubeDef, (ushort)(aCubeAttributes | PlanCubeAttributesFlagValues.WeekDetail), aCubePriority, aReadOnly, aCheckNodeSecurity)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		//Begin Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
		/// <summary>
		/// Returns a boolean indicating if the cell referenced can be scheduled for calculation.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if the cell is displayable.
		/// </returns>

		override public bool canCellBeScheduled(ComputationCellReference aCompCellRef)
		{
			ProfileList dateList;
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)aCompCellRef;
				//Begin Track #6007 - JScott - All Cum Variables when Sales are changed at Month or Quarter are not cascading.
				//dateList = GetTimeDetailProfileList(planCellRef);
				dateList = planCellRef.GetTimeDetailProfileList(eProfileType.Week);
				//End Track #6007 - JScott - All Cum Variables when Sales are changed at Month or Quarter are not cascading.

				//Begin TT#149 - JScott - BonTon Calcs Ph3 - All Store Total variable changes (Grs Rec, R/M Grs Rec, Add MKU, Add MKU %, Avg Rtl Rec)
				//if (dateList.Contains(planCellRef[GetTimeType()]))
				//{
				//    return true;
				//}
				//else
				//{
				//    return false;
				//}
				if (!dateList.Contains(planCellRef[GetTimeType()]))
				{
					return false;
				}

				if (planCellRef.isCellIneligible)
				{
					return false;
				}

				return true;
				//End TT#149 - JScott - BonTon Calcs Ph3 - All Store Total variable changes (Grs Rec, R/M Grs Rec, Add MKU, Add MKU %, Avg Rtl Rec)
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
		/// <summary>
		/// Returns the eCubeType of the week Detail cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the week Detail cube.
		/// </returns>

		override public eCubeType GetWeekDetailCubeType()
		{
			return eCubeType.None;
		}

		/// <summary>
		/// Returns an eProfileType of the time dimension for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// An eProfileType of the time dimension.
		/// </returns>

		override public eProfileType GetTimeType()
		{
			return eProfileType.Week;
		}

		//Begin Track #4581 - JScott - Custom Variables not calculating on Basis line
		///// <summary>
		///// Returns an eProfileType of the version dimension for the given PlanCellReference.
		///// </summary>
		///// <returns>
		///// An eProfileType of the version dimension.
		///// </returns>

		//override public eProfileType GetVersionType()
		//{
		//    return eProfileType.Version;
		//}

		//End Track #4581 - JScott - Custom Variables not calculating on Basis line

		//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
		/// <summary>
		/// Abstract method that returns a boolean indicating if the given PlanCellReference PlanCell should be marked as read-only.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the PlanCell to get the read-only status for.
		/// </param>
		/// <returns>
		/// A boolean indicating if the given PlanCellReference PlanCell should be marked as read-only.
		/// </returns>

		override public bool isPlanCellReadOnly(PlanCellReference aPlanCellRef)
		{
			ProfileList dateList;

			try
			{
                // Begin TT#5276 - JSmith - Read Only Saves
                if (!((PlanCubeGroup)aPlanCellRef.Cube.CubeGroup).IsNodeEnqueued(aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef.PlanCube.isChainCube))
                {
                    return true;
                }
                // End TT#5276 - JSmith - Read Only Saves

				dateList = GetTimeDetailProfileList(aPlanCellRef);

				if (dateList.Contains(aPlanCellRef[GetTimeType()]))
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End Track #5121 - JScott - Add Year/Season/Quarter totals

		/// <summary>
		/// Returns an incremented Time key.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that points to the Cell to inspect.
		/// </param>
		/// <param name="aIncrement">
		/// The amount to increment the time key by.
		/// </param>
		/// <returns>
		/// The incremented time key.
		/// </returns>

		override public int IncrementTimeKey(PlanCellReference aPlanCellRef, int aIncrement)
		{
			return Calendar.AddWeeks(aPlanCellRef[eProfileType.Week], aIncrement);
		}

		/// <summary>
		/// Returns an incremented Time key.
		/// </summary>
		/// <param name="aTimeKey">
		/// The key of the time index.
		/// </param>
		/// <param name="aIncrement">
		/// The amount to increment the time key by.
		/// </param>
		/// <returns>
		/// The incremented time key.
		/// </returns>

		override public int IncrementTimeKey(int aTimeKey, int aIncrement)
		{
			return Calendar.AddWeeks(aTimeKey, aIncrement);
		}

		/// <summary>
		/// Returns the VariableProfile object for the Cell specified by the PlanCellReference.
		/// </summary>
		/// <remarks>
		/// A PlanCellReference object that identifies the PlanCubeCell to retrieve.
		/// </remarks>

		override public ComputationVariableProfile GetVariableProfile(ComputationCellReference aPlanCellRef)
		{
			try
			{
				return (ComputationVariableProfile)MasterVariableProfileList.FindKey(aPlanCellRef[eProfileType.Variable]);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean value indicating if the version of the Cell pointed to by  the given PlanCellReference is protected.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that points to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A boolean indicating if the version for the given PlanCellReference is protected.
		/// </returns>

		override public bool isVersionProtected(PlanCellReference aPlanCellRef)
		{
			int week;

			try
			{
				if (aPlanCellRef.GetCalcVariableProfile().VariableWeekType == eVariableWeekType.BOW)
				{
					week = SAB.ApplicationServerSession.Calendar.AddWeeks(aPlanCellRef[eProfileType.Week], -1);
				}
				else
				{
					week = aPlanCellRef[eProfileType.Week];
				}

				return aPlanCellRef.Cube.CubeGroup.Transaction.isVersionProtected(aPlanCellRef[eProfileType.Version], week);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//Begin Track #5669 - JScott - BMU %

		/// <summary>
		/// Returns a boolean value indicating if the version of the Cell pointed to by  the given PlanCellReference is display-only.
		/// </summary>
		/// <param name="aPlanCellRef"></param>
		/// <returns></returns>

		override public bool isDisplayOnly(PlanCellReference aPlanCellRef)
		{
			ComputationVariableProfile baseVarProf;

			try
			{
				baseVarProf = aPlanCellRef.GetVariableAccessVariableProfile();

				if (baseVarProf != null)
				{
					return baseVarProf.VariableAccess == eVariableAccess.DisplayOnly;
				}
				else
				{
					return true;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End Track #5669 - JScott - BMU %
		//Begin Track #6088 - JScott - Bad 2008 Dsc Rsv % on basis line for B OTB if 2009 is on Plan line

		/// <summary>
		/// Returns the Time key of the data for the given PlanCellReference.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to get the Time key of.
		/// </param>
		/// <returns>
		/// The Time key of the data for the given PlanCellReference.
		/// </returns>

		override public int GetWeekKeyOfData(PlanCellReference aPlanCellRef)
		{
			try
			{
				return aPlanCellRef[eProfileType.Week];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End Track #6088 - JScott - Bad 2008 Dsc Rsv % on basis line for B OTB if 2009 is on Plan line
		//Begin Track #6427 - JSCott - Urgent - In Prod BOTB and Vendor OTB, Copy Low to High is not working correctly

		override public void ClearCubeForHierarchyVersion(PlanCellReference aCellRef)
		{
			PlanReadLogKey readLogKey;

			try
			{
				Clear(aCellRef, 2);
				readLogKey = new PlanReadLogKey(aCellRef[eProfileType.Version], aCellRef[eProfileType.HierarchyNode]);
				_planReadLog.ClearReadLogForKey(readLogKey);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End Track #6427 - JSCott - Urgent - In Prod BOTB and Vendor OTB, Copy Low to High is not working correctly
	}
}
