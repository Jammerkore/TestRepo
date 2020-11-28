using System;
using MIDRetail.DataCommon;
using MIDRetail.Common;

namespace MIDRetail.Business
{
	/// <summary>
	/// This class defines the PlanBasisCube.
	/// </summary>
	/// <remarks>
	/// The PlanBasisCube defines a cube that contains basis values.
	/// </remarks>

	abstract public class PlanBasisCube : PlanCube
	{
		//=======
		// FIELDS
		//=======

		protected int _hierarchyDimensionIndex;
		protected int _versionDimensionIndex;
		protected int _basisDimensionIndex;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of PlanBasisCube, using the given SessionAddressBlock, Transaction, and PlanCubeGroup.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this PlanBasisCube is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this PlanBasisCube is a part of.
		/// </param>
		/// <param name="aPlanCubeGroup">
		/// A reference to a PlanCubeGroup that this PlanBasisCube is a part of.
		/// </param>

		public PlanBasisCube(
			SessionAddressBlock aSAB,
			ApplicationSessionTransaction aTransaction,
			PlanCubeGroup aPlanCubeGroup,
			CubeDefinition aCubeDef,
			ushort aCubeAttributes,
			int aCubePriority)

			: base(aSAB, aTransaction, aPlanCubeGroup, aCubeDef, (ushort)(aCubeAttributes | PlanCubeAttributesFlagValues.Basis), aCubePriority, true, false)
		{
			_hierarchyDimensionIndex = this.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.HierarchyNode);
			_versionDimensionIndex = this.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.HierarchyNode);
			_basisDimensionIndex = this.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.Basis);
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		public override CellCoordinates CreateCellCoordinates(int aNumIndices)
		{
			try
			{
				return new PlanBasisCellCoordinates(aNumIndices, this, _hierarchyDimensionIndex, _versionDimensionIndex, _basisDimensionIndex);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

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
			return false;
		}

		//End Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
		//Begin Track #4581 - JScott - Custom Variables not calculating on Basis line
		/// <summary>
		/// Returns an eProfileType of the version dimension for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// An eProfileType of the version dimension.
		/// </returns>

		override public eProfileType GetVersionType()
		{
			return eProfileType.Version;
		}

		//End Track #4581 - JScott - Custom Variables not calculating on Basis line
		/// <summary>
		/// Returns an eProfileType of the hierarchy node dimension for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// An eProfileType of the Hierarchy Node dimension.
		/// </returns>

		override public eProfileType GetHierarchyNodeType()
		{
			//Begin Track #4581 - JScott - Custom Variables not calculating on Basis line
			//return eProfileType.None;
			return eProfileType.HierarchyNode;
			//End Track #4581 - JScott - Custom Variables not calculating on Basis line
		}

		/// <summary>
		/// Method that returns a boolean indicating the the basis of the given PlanCellReference is a simple (1 detail) basis.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the cell to check.
		/// </param>
		/// <returns>
		/// A boolean indicating the the basis of the given PlanCellReference is a simple (1 detail) basis.
		/// </returns>

		public bool isSimpleBasis(PlanCellReference aPlanCellRef)
		{
			try
			{
				return ((BasisProfile)PlanCubeGroup.OpenParms.GetBasisProfileList(aPlanCellRef.PlanCube.PlanCubeGroup, aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.Version]).FindKey(aPlanCellRef[eProfileType.Basis])).BasisDetailProfileList.Count == 1;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns the VersionProfile for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// The VersionProfile for the given PlanCellReference.
		/// </returns>

		override public VersionProfile GetVersionProfile(PlanCellReference aPlanCellRef) 
		{
			try
			{
				return ((BasisDetailProfile)((BasisProfile)PlanCubeGroup.OpenParms.GetBasisProfileList(aPlanCellRef.PlanCube.PlanCubeGroup, aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.Version]).FindKey(aPlanCellRef[eProfileType.Basis])).BasisDetailProfileList[0]).VersionProfile;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns the HierarchyNodeProfile for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// The HierarchyNodeProfile for the given PlanCellReference.
		/// </returns>

		override public HierarchyNodeProfile GetHierarchyNodeProfile(PlanCellReference aPlanCellRef)
		{
			try
			{
				return ((BasisDetailProfile)((BasisProfile)PlanCubeGroup.OpenParms.GetBasisProfileList(aPlanCellRef.PlanCube.PlanCubeGroup, aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.Version]).FindKey(aPlanCellRef[eProfileType.Basis])).BasisDetailProfileList[0]).HierarchyNodeProfile;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns the store status for the Cell.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// A PlanCellReference object that identifies the PlanCubeCell to process.
		/// </param>
		/// <returns>
		/// The eStoreStatus value that describes the store status of the Cell.
		/// </returns>

		override public eStoreStatus GetStoreStatus(PlanCellReference aPlanCellRef)
		{
			try
			{
				if (isStoreCube)
				{
					return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(aPlanCellRef[eProfileType.Store],
						((BasisDetailProfile)((BasisProfile)PlanCubeGroup.OpenParms.GetBasisProfileList(aPlanCellRef.PlanCube.PlanCubeGroup, aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.Version]).FindKey(aPlanCellRef[eProfileType.Basis])).BasisDetailProfileList[0]).GetBasisWeekIdFromPlanWeekId(_SAB.ApplicationServerSession, aPlanCellRef[eProfileType.Week]));
				}
				else
				{
					return eStoreStatus.None;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

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
			return true;
		}

		//End Track #5121 - JScott - Add Year/Season/Quarter totals
		/// <summary>
		/// Override.  Method that returns a ProfileList of the weeks for the given PlanCellReference.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the PlanCell to find the week ProfileList for.
		/// </param>
		/// <returns>
		/// A ProfileList of weeks.
		/// </returns>

		override public ProfileList GetTimeDetailProfileList(PlanCellReference aPlanCellRef)
		{
			try
			{
				return GetTimeDetailProfileList(aPlanCellRef, GetTimeType());
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns a ProfileList of the weeks for the given PlanCellReference and eProfileType.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the PlanCell to find the week ProfileList for.
		/// </param>
        /// <param name="aDateProfileType">
		/// The eProfileType of the date type to look up.
		/// </param>
		/// <returns>
		/// A ProfileList of time.
		/// </returns>

		override public ProfileList GetTimeDetailProfileList(PlanCellReference aPlanCellRef, eProfileType aDateProfileType)
		{
			try
			{
				if (aDateProfileType == eProfileType.Week)
				{
					return ((BasisProfile)PlanCubeGroup.OpenParms.GetBasisProfileList(aPlanCellRef.PlanCube.PlanCubeGroup, aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.Version]).FindKey(aPlanCellRef[eProfileType.Basis])).DisplayablePlanWeekProfileList(aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession);
				}
				else
				{
					return ((BasisProfile)PlanCubeGroup.OpenParms.GetBasisProfileList(aPlanCellRef.PlanCube.PlanCubeGroup, aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.Version]).FindKey(aPlanCellRef[eProfileType.Basis])).DisplayablePlanPeriodProfileList(aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
//Begin Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity

		/// <summary>
		/// Method that returns the AverageDivisor for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// The AverageDivisor for the given PlanCellReference.
		/// </returns>

		override public double GetAverageDivisor(PlanCellReference aPlanCellRef)
		{
			try
			{
				return ((BasisProfile)PlanCubeGroup.OpenParms.GetBasisProfileList(aPlanCellRef.PlanCube.PlanCubeGroup, aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.Version]).FindKey(aPlanCellRef[eProfileType.Basis])).GetAverageDivisor(aPlanCellRef.PlanCube.SAB.ApplicationServerSession);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns a boolean indicating if the given PlanCellReference contains the current week.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the given PlanCellReference contains the current week.
		/// </returns>

		override public bool ContainsCurrentWeek(PlanCellReference aPlanCellRef)
		{
			try
			{
				return ((BasisProfile)PlanCubeGroup.OpenParms.GetBasisProfileList(aPlanCellRef.PlanCube.PlanCubeGroup, aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.Version]).FindKey(aPlanCellRef[eProfileType.Basis])).ContainsCurrentWeek(aPlanCellRef.PlanCube.SAB.ApplicationServerSession);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
//End Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity
	}
}
