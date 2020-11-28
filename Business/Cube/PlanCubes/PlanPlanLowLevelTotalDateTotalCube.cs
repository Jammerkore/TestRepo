using System;
using System.Collections;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// This class defines the PlanPlanLowLevelTotalDateTotal.
	/// </summary>
	/// <remarks>
	/// The PlanPlanLowLevelTotalDateTotal defines the values for the sum of the time periods for a variable.
	/// </remarks>

	abstract public class PlanPlanLowLevelTotalDateTotalCube : PlanPlanDateTotalCube
	{
		//=======
		// FIELDS
		//=======

		private Hashtable _versionProtectedHash;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of PlanPlanLowLevelTotalDateTotal, using the given SessionAddressBlock, Transaction, and PlanCubeGroup.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this PlanPlanLowLevelTotalDateTotal is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this PlanPlanLowLevelTotalDateTotal is a part of.
		/// </param>
		/// <param name="aPlanCubeGroup">
		/// A reference to a PlanCubeGroup that this PlanPlanLowLevelTotalDateTotal is a part of.
		/// </param>

		public PlanPlanLowLevelTotalDateTotalCube(
			SessionAddressBlock aSAB,
			ApplicationSessionTransaction aTransaction,
			PlanCubeGroup aPlanCubeGroup,
			CubeDefinition aCubeDef,
			ushort aCubeAttributes,
			int aCubePriority,
			bool aReadOnly,
			bool aCheckNodeSecurity)

			: base(aSAB, aTransaction, aPlanCubeGroup, aCubeDef, aCubeAttributes, aCubePriority, aReadOnly, aCheckNodeSecurity)
		{
			_versionProtectedHash = new Hashtable();
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Returns the eCubeType of the Low-Level Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Low-Level Total cube.
		/// </returns>

		override public eCubeType GetLowLevelTotalCubeType()
		{
			return eCubeType.None;
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
			//Begin Track #6008 - JScott - B OTB Multi Level Errors Copy Low to High and Balance Low Levels
			HashKeyObject hashKey;
			//End Track #6008 - JScott - B OTB Multi Level Errors Copy Low to High and Balance Low Levels
			object hashValue;
			bool isProtected;

			try
			{
				//Begin Track #6008 - JScott - B OTB Multi Level Errors Copy Low to High and Balance Low Levels
				//hashValue = _versionProtectedHash[aPlanCellRef[eProfileType.TimeTotalVariable]];
				hashKey = new HashKeyObject(aPlanCellRef[eProfileType.LowLevelTotalVersion], aPlanCellRef[eProfileType.TimeTotalVariable]);
				hashValue = _versionProtectedHash[hashKey];
				//End Track #6008 - JScott - B OTB Multi Level Errors Copy Low to High and Balance Low Levels

				if (hashValue == null)
				{
					isProtected = aPlanCellRef.GetComponentDetailVersionProtected();
					//Begin Track #6008 - JScott - B OTB Multi Level Errors Copy Low to High and Balance Low Levels
					//_versionProtectedHash[aPlanCellRef[eProfileType.TimeTotalVariable]] = isProtected;
					_versionProtectedHash[hashKey] = isProtected;
					//End Track #6008 - JScott - B OTB Multi Level Errors Copy Low to High and Balance Low Levels
				}
				else
				{
					isProtected = (bool)hashValue;
				}
				return isProtected;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Reads the PlanCubeCell from the database.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// A PlanCellReference object that identifies the PlanCubeCell to read.
		/// </param>

		override public void ReadCell(PlanCellReference aPlanCellRef)
		{
		}

		/// <summary>
		/// Returns true if any cell for the given ProfileList of PlanProfiles has changed.
		/// </summary>
		/// <param name="aPlanProfileList">
		/// A ProfileList of PlanProfiles to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if any cell has changed.
		/// </returns>

		override public bool hasAnyPlanChanged(ProfileList aPlanProfileList)
		{
			return false;
		}

		/// <summary>
		/// Returns true if any cell for the given PlanProfile has changed.
		/// </summary>
		/// <param name="aPlanProfile">
		/// The PlanProfile to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if any cell has changed.
		/// </returns>

		override public bool hasPlanChanged(PlanProfile aPlanProfile)
		{
			return false;
		}

		/// <summary>
		/// Returns an eProfileType of the version dimension for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// An eProfileType of the version dimension.
		/// </returns>

		override public eProfileType GetVersionType()
		{
			return eProfileType.LowLevelTotalVersion;
		}
		//Begin Track #4581 - JScott - Custom Variables not calculating on Basis line

		/// <summary>
		/// Returns an eProfileType of the hierarchy node dimension for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// An eProfileType of the Hierarchy Node dimension.
		/// </returns>

		override public eProfileType GetHierarchyNodeType()
		{
			return eProfileType.None;
		}
		//End Track #4581 - JScott - Custom Variables not calculating on Basis line
	}
}
