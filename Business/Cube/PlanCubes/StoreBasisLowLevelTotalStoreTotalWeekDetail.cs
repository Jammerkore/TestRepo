using System;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// This class defines the StoreBasisLowLevelTotalStoreTotalWeekDetail.
	/// </summary>
	/// <remarks>
	/// The StoreBasisLowLevelTotalStoreTotalWeekDetail defines the values for the sum of the time periods for a variable.
	/// </remarks>

	public class StoreBasisLowLevelTotalStoreTotalWeekDetail : PlanBasisLowLevelTotalWeekDetailCube
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of StoreBasisLowLevelTotalStoreTotalWeekDetail, using the given SessionAddressBlock, Transaction, and PlanCubeGroup.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this StoreBasisLowLevelTotalStoreTotalWeekDetail is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this StoreBasisLowLevelTotalStoreTotalWeekDetail is a part of.
		/// </param>
		/// <param name="aPlanCubeGroup">
		/// A reference to a PlanCubeGroup that this StoreBasisLowLevelTotalStoreTotalWeekDetail is a part of.
		/// </param>

		public StoreBasisLowLevelTotalStoreTotalWeekDetail(
			SessionAddressBlock aSAB,
			ApplicationSessionTransaction aTransaction,
			PlanCubeGroup aPlanCubeGroup,
			CubeDefinition aCubeDef,
			int aCubePriority)

			: base(aSAB, aTransaction, aPlanCubeGroup, aCubeDef, PlanCubeAttributesFlagValues.Store | PlanCubeAttributesFlagValues.LowLevelTotal | CubeAttributesFlagValues.StoreTotal, aCubePriority)
		{
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the eCubeType of this cube.
		/// </summary>

		public override eCubeType CubeType
		{
			get
			{
				return eCubeType.StoreBasisLowLevelTotalStoreTotalWeekDetail;
			}
		}

		//========
		// METHODS
		//========

		public override CellCoordinates CreateCellCoordinates(int aNumIndices)
		{
			try
			{
				return new StoreBasisLowLevelCellCoordinates(aNumIndices, this, _basisDimensionIndex);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the Chain Detail cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Chain Detail cube.
		/// </returns>

		override public eCubeType GetChainDetailCubeType()
		{
			return eCubeType.ChainBasisLowLevelTotalWeekDetail;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the Low-Level Detail cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Low-Level Detail cube.
		/// </returns>

		override public eCubeType GetLowLevelDetailCubeType()
		{
			return eCubeType.StoreBasisStoreTotalWeekDetail;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the Store Detail cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Store Detail cube.
		/// </returns>

		override public eCubeType GetStoreDetailCubeType()
		{
			return eCubeType.StoreBasisLowLevelTotalWeekDetail;
		}

		/// <summary>
		/// Returns the eCubeType of the Group Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Group Total cube.
		/// </returns>

		override public eCubeType GetGroupTotalCubeType()
		{
			return eCubeType.StoreBasisLowLevelTotalGroupTotalWeekDetail;
		}

		/// <summary>
		/// Returns the eCubeType of the Store Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Store Total cube.
		/// </returns>

		override public eCubeType GetStoreTotalCubeType()
		{
			return eCubeType.None;
		}

		/// <summary>
		/// Returns the eCubeType of the Date Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Date Total cube.
		/// </returns>

		override public eCubeType GetDateTotalCubeType()
		{
			return eCubeType.StoreBasisLowLevelTotalStoreTotalDateTotal;
		}

		//Begin Track #6010 - JScott - Bad % Change on Basis2
		/// <summary>
		/// Abstract method that returns the eCubeType of the Plan cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Plan cube.
		/// </returns>

		override public eCubeType GetPlanCubeType()
		{
			return eCubeType.StorePlanLowLevelTotalStoreTotalWeekDetail;
		}

		//End Track #6010 - JScott - Bad % Change on Basis2
		/// <summary>
		/// Returns the eCubeType of the Basis cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Basis cube.
		/// </returns>

		override public eCubeType GetBasisCubeType()
		{
			return eCubeType.StoreBasisLowLevelTotalStoreTotalWeekDetail;
		}

		/// <summary>
		/// Returns a boolean value indicating if the store of the Cell pointed to by the given PlanCellReference is eligible.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that point to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A boolean indicating if the store for the given PlanCellReference is eligible.
		/// </returns>

		override public bool isStoreIneligible(PlanCellReference aPlanCellRef)
		{
			try
			{
				//Begin Track #6226 - JScott - Modified version not honoring similiar store in OTS forecast review
				//if (aPlanCellRef.isCellActual)
				//{
				//    return false;
				//}
				//else
				//{
				//End Track #6226 - JScott - Modified version not honoring similiar store in OTS forecast review
				return aPlanCellRef.GetComponentDetailStoreIneligible();
				//Begin Track #6226 - JScott - Modified version not honoring similiar store in OTS forecast review
				//}
				//End Track #6226 - JScott - Modified version not honoring similiar store in OTS forecast review
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
		/// <param name="aCompCellRef">
		/// The PlanCellReference that points to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A boolean indicating if the variable is stored on the database.
		/// </returns>

		override public bool isDatabaseVariable(ComputationVariableProfile aVarProf, ComputationCellReference aCompCellRef)
		{
			try
			{
				return aVarProf.isDatabaseVariable(eVariableCategory.Store, ((PlanCellReference)aCompCellRef).GetVersionProfile().Key, eCalendarDateType.Week);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a string describing the given PlanCellReference
		/// </summary>
		/// <param name="aCompCellRef">
		/// The PlanCellReference that points to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A string describing the PlanCellReference.
		/// </returns>

		override public string GetCellDescription(ComputationCellReference aCompCellRef)
		{
			WeekProfile weekProf;
			QuantityVariableProfile qtyVarProf;
			VariableProfile varProf;

			try
			{
				weekProf = _SAB.ApplicationServerSession.Calendar.GetWeek(aCompCellRef[eProfileType.Week]);
				qtyVarProf = (QuantityVariableProfile)Transaction.PlanComputations.PlanQuantityVariables.QuantityVariableProfileList.FindKey(aCompCellRef[eProfileType.QuantityVariable]);
				varProf = (VariableProfile)Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aCompCellRef[eProfileType.Variable]);

				return "Store Basis Low-Level Total Store Total Week Detail" +
					", Basis " + aCompCellRef[eProfileType.Basis] +
					", Week \"" + weekProf.Text() + "\"" +
					", Quantity \"" + qtyVarProf.VariableName + "\"" +
					", Variable \"" + varProf.VariableName + "\"";
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}