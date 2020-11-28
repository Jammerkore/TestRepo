using System;
using System.Collections;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The PlanCellReference class defines the interface to the PlanCell/PlanCube relationship.
	/// </summary>
	/// <remarks>
	/// The PlanCellReference defines interface properties and methods that allow the owner to access fields and functionality in the PlanCell
	/// and PlanCube classes.
	/// </remarks>

	public class PlanCellReference : ComputationCellReference
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of the PlanCellReference class using the given PlanCube.
		/// </summary>
		/// <param name="aPlanCube">
		/// A reference to the PlanCube where the PlanCell exists.
		/// </param>

		public PlanCellReference(PlanCube aPlanCube)
			: base(aPlanCube)
		{
		}

		/// <summary>
		/// Creates a new instance of the PlanCellReference class using the given PlanCube and CellCoordinates.
		/// </summary>
		/// <param name="aPlanCube">
		/// A reference to the PlanCube where the PlanCell exists.
		/// </param>
		/// <param name="aCellCoordinates">
		/// The CellCoordinates that defines the PlanCell's position in the PlanCube.
		/// </param>

		public PlanCellReference(PlanCube aPlanCube, CellCoordinates aCellCoordinates)
			: base(aPlanCube, aCellCoordinates)
		{
		}

		/// <summary>
		/// Creates a new instance of the PlanCellReference class using the given PlanCube and CellReference.  A coordinate-mapping is performed to translate
		/// the coordinates of the given CellReference (that may be pointing to a different PlanCube) to the given PlanCube.
		/// </summary>
		/// <param name="aPlanCube">
		/// A reference to the PlanCube where the PlanCell exists.
		/// </param>
		/// <param name="aCellReference">
		/// The CellReference that is to be mapped to the given PlanCube.
		/// </param>

		public PlanCellReference(PlanCube aPlanCube, CellReference aCellReference)
			: base (aPlanCube, aCellReference)
		{
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the Cube reference.
		/// </summary>

		public PlanCube PlanCube
		{
			get
			{
				try
				{
					return (PlanCube)Cube;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the Cube reference.
		/// </summary>

		public PlanCell PlanCell
		{
			get
			{
				try
				{
					return (PlanCell)Cell;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the Ineligible flag of the PlanCell.
		/// </summary>

		override public bool isCellIneligible
		{
			get
			{
				try
				{
					InitCellIneligibleFlag();
					return PlanCell.isIneligible;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				try
				{
					PlanCell.isIneligible = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the Protected flag of the PlanCell.
		/// </summary>

		override public bool isCellProtected
		{
			get
			{
				try
				{
					InitCellProtectedFlag();
					return PlanCell.isProtected;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				try
				{
					PlanCell.isProtected = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the Closed flag of the PlanCell.
		/// </summary>

		override public bool isCellClosed
		{
			get
			{
				try
				{
					InitCellClosedFlag();
					return PlanCell.isClosed;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				try
				{
					PlanCell.isClosed = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the ExtensionCellReference of the ComputationCell.
		/// </summary>

		override protected ComputationCellReferenceExtension ComputationCellRefExt
		{
			get
			{
				try
				{
					if (_computationCellRefExt == null)
					{
						if (GetVariableScopeVariableProfile().VariableScope == eVariableScope.Dynamic)
						{
							_computationCellRefExt = new PlanCellReferenceComparativeExtension(this);
						}
						else
						{
							_computationCellRefExt = new PlanCellReferenceValueExtension(this);
						}
					}

					return _computationCellRefExt;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell referenced by the given PlanCellReference is an Actual version.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the cell is an Actual version.
		/// </returns>

		public bool isCellActual
		{
			get
			{
				try
				{
					return PlanCube.isCellActual(this);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Abstract method that creates a new ComputationScheduleFormulaEntry object from the given parameters.
		/// </summary>
		/// <param name="aFormulaProfile">
		/// A FormulaProfile object for the formula to be executed.
		/// </param>
		/// <param name="aScheduleEntryType">
		/// An eComputationScheduleEntryType that defines the type of entry.
		/// </param>
		/// <param name="aSchedulePriority">
		/// An int indicating the priority of this schedule entry.
		/// </param>
		/// <param name="aCubePriority">
		/// An int indicating the priority of the cube.
		/// </param>
		/// <returns>
		/// A new ComputationScheduleFormulaEntry object.
		/// </returns>

		override public ComputationScheduleFormulaEntry CreateScheduleFormulaEntry(
			ComputationCellReference aChangedPlanCellRef,
			FormulaProfile aFormulaProfile,
			eComputationScheduleEntryType aScheduleEntryType,
			int aSchedulePriority,
			int aCubePriority)
		{
			try
			{
				return new PlanScheduleFormulaEntry(aChangedPlanCellRef, this, aFormulaProfile, aScheduleEntryType, aSchedulePriority, aCubePriority);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Abstract method that creates a new ComputationScheduleSpreadEntry object from the given parameters.
		/// </summary>
        /// <param name="aSpreadProfile">
		/// A SpreadProfile object for the spread to be executed.
		/// </param>
		/// <param name="aSchedulePriority">
		/// An int indicating the priority of this schedule entry.
		/// </param>
		/// <param name="aCubePriority">
		/// An int indicating the priority of the cube.
		/// </param>
		/// <returns>
		/// A new ComputationScheduleFormulaEntry object.
		/// </returns>

		override public ComputationScheduleSpreadEntry CreateScheduleSpreadEntry(
			SpreadProfile aSpreadProfile,
			int aSchedulePriority,
			int aCubePriority)
		{
			try
			{
				return new PlanScheduleSpreadEntry(this, aSpreadProfile, aSchedulePriority, aCubePriority);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override protected void InitCellDisplayOnlyFlag()
		{
			try
			{
				ComputationCellRefExt.InitCellDisplayOnlyFlag();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override protected void InitCellIneligibleFlag()
		{
			try
			{
				ComputationCellRefExt.InitCellIneligibleFlag();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override protected void InitCellClosedFlag()
		{
			try
			{
				ComputationCellRefExt.InitCellClosedFlag();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override protected void InitCellProtectedFlag()
		{
			try
			{
				ComputationCellRefExt.InitCellProtectedFlag();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override protected ExtensionCell GetExtensionCell(bool aAllocate)
		{
			try
			{
				return ComputationCellRefExt.GetExtensionCell(aAllocate);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a copy of the PlanCellReference.  The PlanCube is a shallow copy, while the PlanCell is a deep copy.
		/// </summary>
		/// <returns>
		/// A new instance of PlanCellReference with a copy of this objects information.
		/// </returns>

		override public CellReference Copy()
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = new PlanCellReference((PlanCube)Cube);
				planCellRef.CopyFrom(this);

				return planCellRef;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an ArrayList of PlanCellReferences.  This ArrayList contains all of the PlanCells that are details of this PlanCubeCell.
		/// </summary>
		/// <returns>
		/// The ArrayList of PlanCellReferences.
		/// </returns>

		public ArrayList GetDetailCellRefArray(eCubeType aCubeType, eStoreStatus aStoreStatus, bool aUseHiddenValues)
		{
			PlanCellStatusSelector cellSelector;

			try
			{
				cellSelector = new PlanCellStatusSelector(aStoreStatus, aUseHiddenValues);
				PlanCube.ProcessDetailCellSelector(this, aCubeType, cellSelector);
				return cellSelector.CellRefList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns an ArrayList of CellReference objects of all the detail cells that are affected by a change to the given Cell using
		/// the CubeRelationship object that identify the detail cells.
		/// </summary>
		/// <remarks>
		/// For the CubeRelationship associated with the detail, the ProfileXRef is retrieved from the Transaction object.  A CellReference object
		/// is added for each detail Id identified on the ProfileXRef.
		/// </remarks>
		/// <param name="aStoreStatus">
		/// The store status to filter the list to.
		/// </param>
		/// <returns>
		/// An ArrayList of CellReference objects pointing to detail Cells.
		/// </returns>

		public ArrayList GetComponentDetailCellRefArray(eStoreStatus aStoreStatus, bool aUseHiddenValues)
		{
			PlanCellStatusSelector cellSelector;

			try
			{
				cellSelector = new PlanCellStatusSelector(aStoreStatus, aUseHiddenValues);
				PlanCube.ProcessComponentDetailCellSelector(this, cellSelector);
				return cellSelector.CellRefList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		
		/// <summary>
		/// Gets an ArrayList of PlanCellReferences.  This ArrayList contains all of the PlanCells that are details of this PlanCubeCell.
		/// </summary>
		/// <returns>
		/// The ArrayList of PlanCellReferences.
		/// </returns>

		public double GetComponentDetailSum(ComputationScheduleEntry aScheduleEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, eStoreStatus aStoreStatus, bool aUseHiddenValues)
		{
			PlanCellSum cellSelector;

			try
			{
				cellSelector = new PlanCellSum(aScheduleEntry, aGetCellMode, aSetCellMode, aStoreStatus, aUseHiddenValues);
				PlanCube.ProcessComponentDetailCellSelector(this, cellSelector);
				return cellSelector.Sum;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
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
		/// <param name="aCubeType">
		/// The eCubeType of the detail cube.
		/// </param>
		/// <param name="aUseHiddenValues">
		/// A boolean indicating if hidden values should be included in the average.
		/// </param>
		/// <returns>
		/// A double containing the average.
		/// </returns>

		public double GetDetailSum(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			eCubeType aCubeType,
			eStoreStatus aStoreStatus,
			bool aUseHiddenValues)
		{
			PlanCellSum cellSelector;

			try
			{
				cellSelector = new PlanCellSum(aScheduleEntry, aGetCellMode, aSetCellMode, aStoreStatus, aUseHiddenValues);
				PlanCube.ProcessDetailCellSelector(this, aCubeType, cellSelector);
				return cellSelector.Sum;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
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
		/// <param name="aCubeType">
		/// The eCubeType of the detail cube.
		/// </param>
		/// <param name="aUseHiddenValues">
		/// A boolean indicating if hidden values should be included in the average.
		/// </param>
		/// <returns>
		/// A double containing the average.
		/// </returns>

		public double GetDetailAverage(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			eCubeType aCubeType,
			eStoreStatus aStoreStatus,
			bool aUseHiddenValues)
		{
			PlanCellAverageSum cellSelector;

			try
			{
				cellSelector = new PlanCellAverageSum(aScheduleEntry, aGetCellMode, aSetCellMode, aStoreStatus, null, aUseHiddenValues);
				PlanCube.ProcessDetailCellSelector(this, aCubeType, cellSelector);
				return cellSelector.Sum / cellSelector.Count;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
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
		/// <param name="aCubeType">
		/// The eCubeType of the detail cube.
		/// </param>
		/// <param name="aVarProf">
		/// A VariableProfile that defines the inventory variable.
		/// </param>
		/// <param name="aUseHiddenValues">
		/// A boolean indicating if hidden values should be included in the average.
		/// </param>
		/// <returns>
		/// A double containing the average.
		/// </returns>

		public double GetDetailNonZeroAverage(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			eCubeType aCubeType,
			eStoreStatus aStoreStatus,
			VariableProfile aVarProf,
			bool aUseHiddenValues)
		{
			PlanCellAverageSum cellSelector;

			try
			{
				cellSelector = new PlanCellAverageSum(aScheduleEntry, aGetCellMode, aSetCellMode, aStoreStatus, aVarProf, aUseHiddenValues);
				PlanCube.ProcessDetailCellSelector(this, aCubeType, cellSelector);
				return cellSelector.Sum / cellSelector.NonZeroCount;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns an ArrayList of CellReference objects of all the detail cells that are affected by a change to the given Cell using
		/// the CubeRelationship object that identify the detail cells.
		/// </summary>
		/// <remarks>
		/// For the CubeRelationship associated with the detail, the ProfileXRef is retrieved from the Transaction object.  A CellReference object
		/// is added for each detail Id identified on the ProfileXRef.
		/// </remarks>
		/// <param name="aStoreStatus">
		/// The store status to filter the list to.
		/// </param>
		/// <returns>
		/// An ArrayList of CellReference objects pointing to detail Cells.
		/// </returns>

		public ArrayList GetSpreadDetailCellRefArray(eStoreStatus aStoreStatus, bool aUseHiddenValues)
		{
			PlanCellStatusSelector cellSelector;

			try
			{
				cellSelector = new PlanCellStatusSelector(aStoreStatus, aUseHiddenValues);
				PlanCube.ProcessSpreadDetailCellSelector(this, cellSelector);
				return cellSelector.CellRefList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Initializes the PlanCell.
		/// </summary>

		override public void InitCellValue()
		{
			try
			{
				ComputationCellRefExt.InitCellValue();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #5712 - JScott - Issue with calcs with copy method - Part 2
		/// <summary>
		/// Reads the ComputationCell.
		/// </summary>

		override public void ReadCellValue()
		{
			try
			{
				ComputationCellRefExt.ReadCellValue();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End Track #5712 - JScott - Issue with calcs with copy method - Part 2
		/// <summary>
		/// Sets the Value of a PlanCell from user entry.
		/// </summary>
		/// <param name="aValue">
		/// The value that is to be assigned to the Cell.
		/// </param>

		override public void SetEntryCellValue(double aValue)
		{
			try
			{
				ComputationCellRefExt.SetEntryCellValue(aValue);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the Value of a PlanCell from a database read.
		/// </summary>
		/// <param name="aValue">
		/// The value that is to be assigned to the Cell.
		/// </param>

		override public void SetLoadCellValue(double aValue, bool aLock)
		{
			try
			{
				ComputationCellRefExt.SetLoadCellValue(aValue, aLock);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin TT#TT#739-MD - JSmith - delete stores
        /// <summary>
        /// Sets the Lock of a PlanCell from a database read.
        /// </summary>
        /// <param name="aLock">
        /// The lock that is to be assigned to the Cell.
        /// </param>

        override public void SetLoadCellLock(bool aLock)
        {
            try
            {
                ComputationCellRefExt.SetLoadCellLock(aLock);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#TT#739-MD - JSmith - delete stores

		// BEGIN TT#2200 - stodd - Open existing Assortment-> select REDO-> process-> MID Client stops working
		/// <summary>
		/// Sets the Value of a PlanCell from a computation.
		/// </summary>
		/// <param name="aSetCellMode">
		/// The eSetCellMode indicating the type of change that is occurring, include Initialize or Entry.
		/// </param>
		/// <param name="aValue">
		/// The value that is to be assigned to the Cell.
		/// </param>

		override public void SetCompCellValue(eSetCellMode aSetCellMode, double aValue)
		{
			try
			{
				SetCompCellValue(aSetCellMode, aValue, false);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	
		/// <summary>
		/// Sets the Value of a PlanCell from a computation.
		/// </summary>
		/// <param name="aSetCellMode">
		/// The eSetCellMode indicating the type of change that is occurring, include Initialize or Entry.
		/// </param>
		/// <param name="aValue">
		/// The value that is to be assigned to the Cell.
		/// <param name="isAsrtSimStore">
		/// Indicates if this is from an assortment similar store call.
		/// </param>

		override public void SetCompCellValue(eSetCellMode aSetCellMode, double aValue, bool isAsrtSimStore)
		{
			try
			{
				ComputationCellRefExt.SetCompCellValue(aSetCellMode, aValue, isAsrtSimStore);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		// END TT#2200 - stodd - Open existing Assortment-> select REDO-> process-> MID Client stops working
		
		/// <summary>
		/// Sets the Value of a PlanCell for a copy.
		/// </summary>
		/// <param name="aValue">
		/// The value that is to be assigned to the Cell.
		/// </param>

		override public void SetCopyCellValue(double aValue, bool aLock)
		{
			try
			{
				ComputationCellRefExt.SetCopyCellValue(aValue, aLock);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the lock value of a PlanCell.
		/// </summary>
		/// <param name="aLock">
		/// A boolean indicating if the cell is locked or unlocked.
		/// </param>

		override public void SetCellLock(bool aLock)
		{
			try
			{
				ComputationCellRefExt.SetCellLock(aLock);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns the VersionProfile for the Cell.
		/// </summary>
		/// <returns>
		/// The VersionProfile for the Cell.
		/// </returns>

		public VersionProfile GetVersionProfile()
		{
			try
			{
				return PlanCube.GetVersionProfile(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #5741 - JScott - OTS Forecast Review Combined Version In Store Inventory not appearing for Actuals.
		/// <summary>
		/// Method that returns the VersionProfile for the Cell's data.
		/// </summary>
		/// <returns>
		/// The VersionProfile for the Cell's data, which could differ from the Cell in Combined Versions.
		/// </returns>

		public VersionProfile GetVersionProfileOfData()
		{
			try
			{
				return PlanCube.GetVersionProfileOfData(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End Track #5741 - JScott - OTS Forecast Review Combined Version In Store Inventory not appearing for Actuals.
		/// <summary>
		/// Method that returns the HierarchyNodeProfile for the Cell.
		/// </summary>
		/// <returns>
		/// The HierarchyNodeProfile for the Cell.
		/// </returns>

		public HierarchyNodeProfile GetHierarchyNodeProfile()
		{
			try
			{
				return PlanCube.GetHierarchyNodeProfile(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns the week ProfileList for the Cell.
		/// </summary>
		/// <returns>
		/// The week ProfileList for the Cell.
		/// </returns>

		public ProfileList GetTimeDetailProfileList()
		{
			try
			{
				return PlanCube.GetTimeDetailProfileList(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns the AverageDivisor for the Cell.
		/// </summary>
		/// <returns>
		/// The AverageDivisor for the Cell.
		/// </returns>

		public double GetAverageDivisor()
		{
			try
			{
				return PlanCube.GetAverageDivisor(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns a boolean indicating if the Cell contains the current week.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the Cell contains the current week.
		/// </returns>

		public bool ContainsCurrentWeek()
		{
			try
			{
				return PlanCube.ContainsCurrentWeek(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns the week ProfileList for the Cell and given eProfileType.
		/// </summary>
		/// <param name="aDateProfileType">
		/// The eProfileType of the date to retrieve.
		/// </param>
		/// <returns>
		/// The week ProfileList for the Cell.
		/// </returns>

		public ProfileList GetTimeDetailProfileList(eProfileType aDateProfileType)
		{
			try
			{
				return PlanCube.GetTimeDetailProfileList(this, aDateProfileType);
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
		/// <returns>
		/// The eStoreStatus value that describes the store status of the Cell.
		/// </returns>

		public eStoreStatus GetStoreStatus()
		{
			try
			{
				return PlanCube.GetStoreStatus(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}

