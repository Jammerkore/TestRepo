using System;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// This class defines the StoreBasisLowLevelTotalPeriodDetail.
	/// </summary>
	/// <remarks>
	/// The StoreBasisLowLevelTotalPeriodDetail defines the values for the sum of the time periods for a variable.
	/// </remarks>

	public class StoreBasisLowLevelTotalPeriodDetail : PlanBasisLowLevelTotalPeriodDetailCube
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of StoreBasisLowLevelTotalPeriodDetail, using the given SessionAddressBlock, Transaction, and PlanCubeGroup.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this StoreBasisLowLevelTotalPeriodDetail is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this StoreBasisLowLevelTotalPeriodDetail is a part of.
		/// </param>
		/// <param name="aPlanCubeGroup">
		/// A reference to a PlanCubeGroup that this StoreBasisLowLevelTotalPeriodDetail is a part of.
		/// </param>

		public StoreBasisLowLevelTotalPeriodDetail(
			SessionAddressBlock aSAB,
			ApplicationSessionTransaction aTransaction,
			PlanCubeGroup aPlanCubeGroup,
			CubeDefinition aCubeDef,
			int aCubePriority)

			: base(aSAB, aTransaction, aPlanCubeGroup, aCubeDef, PlanCubeAttributesFlagValues.Store | PlanCubeAttributesFlagValues.LowLevelTotal, aCubePriority)
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
				return eCubeType.StoreBasisLowLevelTotalPeriodDetail;
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
			return eCubeType.ChainBasisLowLevelTotalPeriodDetail;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the Low-Level Detail cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Low-Level Detail cube.
		/// </returns>

		override public eCubeType GetLowLevelDetailCubeType()
		{
			return eCubeType.StoreBasisPeriodDetail;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the Store Detail cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Store Detail cube.
		/// </returns>

		override public eCubeType GetStoreDetailCubeType()
		{
			return eCubeType.None;
		}

//Begin Track #4123 - JScott - Period Comp, NonComp, and New values wrong
		/// <summary>
		/// Returns the eCubeType of the week Detail cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the week Detail cube.
		/// </returns>

		override public eCubeType GetWeekDetailCubeType()
		{
			return eCubeType.StoreBasisLowLevelTotalWeekDetail;
		}

//End Track #4123 - JScott - Period Comp, NonComp, and New values wrong
		/// <summary>
		/// Returns the eCubeType of the Group Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Group Total cube.
		/// </returns>

		override public eCubeType GetGroupTotalCubeType()
		{
			return eCubeType.StoreBasisLowLevelTotalGroupTotalPeriodDetail;
		}

		/// <summary>
		/// Returns the eCubeType of the Store Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Store Total cube.
		/// </returns>

		override public eCubeType GetStoreTotalCubeType()
		{
			return eCubeType.StoreBasisLowLevelTotalStoreTotalPeriodDetail;
		}

		/// <summary>
		/// Returns the eCubeType of the Date Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Date Total cube.
		/// </returns>

		override public eCubeType GetDateTotalCubeType()
		{
			return eCubeType.StoreBasisLowLevelTotalDateTotal;
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
			return eCubeType.StorePlanLowLevelTotalPeriodDetail;
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
			return eCubeType.StoreBasisLowLevelTotalPeriodDetail;
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
			PeriodProfile perProf;
			QuantityVariableProfile qtyVarProf;
			VariableProfile varProf;
			StoreProfile storeProf;

			try
			{
				perProf = _SAB.ApplicationServerSession.Calendar.GetPeriod(aCompCellRef[eProfileType.Period]);
				qtyVarProf = (QuantityVariableProfile)Transaction.PlanComputations.PlanQuantityVariables.QuantityVariableProfileList.FindKey(aCompCellRef[eProfileType.QuantityVariable]);
				varProf = (VariableProfile)Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aCompCellRef[eProfileType.Variable]);
				storeProf = (StoreProfile)PlanCubeGroup.GetMasterProfileList(eProfileType.Store).FindKey(aCompCellRef[eProfileType.Store]);

				return "Store Basis Low-Level Total Period Detail" +
					", Basis " + aCompCellRef[eProfileType.Basis] +
					", Period \"" + perProf.Name + "\"" +
					", Quantity \"" + qtyVarProf.VariableName + "\"" +
					", Variable \"" + varProf.VariableName + "\"" +
					", Store \"" + storeProf.Text + "\"";
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
