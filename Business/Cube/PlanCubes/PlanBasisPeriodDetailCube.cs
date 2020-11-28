using System;
//Begin Track #4309 - JScott - Onhand incorrect for multiple detail basis
using System.Collections;
//End Track #4309 - JScott - Onhand incorrect for multiple detail basis
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// This class defines the PlanBasisPeriodDetailCube.
	/// </summary>
	/// <remarks>
	/// The PlanBasisPeriodDetailCube defines a cube that contains the basis Period detail values.
	/// </remarks>

	abstract public class PlanBasisPeriodDetailCube : PlanBasisCube
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of PlanBasisPeriodDetailCube, using the given SessionAddressBlock, Transaction, and PlanCubeGroup.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this PlanBasisPeriodDetailCube is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this PlanBasisPeriodDetailCube is a part of.
		/// </param>
		/// <param name="aPlanCubeGroup">
		/// A reference to a PlanCubeGroup that this PlanBasisPeriodDetailCube is a part of.
		/// </param>

		public PlanBasisPeriodDetailCube(
			SessionAddressBlock aSAB,
			ApplicationSessionTransaction aTransaction,
			PlanCubeGroup aPlanCubeGroup,
			CubeDefinition aCubeDef,
			ushort aCubeAttributes,
			int aCubePriority)

			: base(aSAB, aTransaction, aPlanCubeGroup, aCubeDef, (ushort)(aCubeAttributes | PlanCubeAttributesFlagValues.PeriodDetail), aCubePriority)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

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
		/// Returns a boolean indicating if the cell referenced by the given PlanCellReference is displayable.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The PlanCellReference to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if the cell is displayable.
		/// </returns>

		override public bool isCellDisplayable(ComputationCellReference aCompCellRef)
		{
			PlanCellReference planCellRef;
			BasisProfile basisProfile;

			try
			{
				planCellRef = (PlanCellReference)aCompCellRef;
				basisProfile = (BasisProfile)PlanCubeGroup.OpenParms.GetBasisProfileList(planCellRef.PlanCube.PlanCubeGroup, planCellRef[eProfileType.HierarchyNode], planCellRef[eProfileType.Version]).FindKey(planCellRef[eProfileType.Basis]);
				return basisProfile.isPlanPeriodDisplayable(SAB.ApplicationServerSession, planCellRef[eProfileType.Period]);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns an eProfileType of the time dimension for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// An eProfileType of the time dimension.
		/// </returns>

		override public eProfileType GetTimeType()
		{
			return eProfileType.Period;
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
			return Calendar.AddPeriods(aPlanCellRef[eProfileType.Period], aIncrement);
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
			return Calendar.AddPeriods(aTimeKey, aIncrement);
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
			return false;
		}
		//Begin Track #5669 - JScott - BMU %

		/// <summary>
		/// Returns a boolean value indicating if the version of the Cell pointed to by  the given PlanCellReference is display-only.
		/// </summary>
		/// <param name="aPlanCellRef"></param>
		/// <returns></returns>

		override public bool isDisplayOnly(PlanCellReference aPlanCellRef)
		{
			return true;
		}
		//End Track #5669 - JScott - BMU %
//Begin Track #4309 - JScott - Onhand incorrect for multiple detail basis

		/// <summary>
		/// Allows a cube to specify custom initializations for a Cell.  Occurs after the standard Cell initialization.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to initialize.
		/// </param>

		public override void InitCellValue(ComputationCellReference aCompCellRef)
		{
			try
			{
				aCompCellRef.SumDetailCellLocks();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
//End Track #4309 - JScott - Onhand incorrect for multiple detail basis
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
			return -1;
		}
		//End Track #6088 - JScott - Bad 2008 Dsc Rsv % on basis line for B OTB if 2009 is on Plan line
	}
}
