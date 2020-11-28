using System;
using System.Collections;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	/// <summary>
	/// The AssortmentCellReference class defines the interface to the AssortmentCell/AssortmentCube relationship.
	/// </summary>
	/// <remarks>
	/// The AssortmentCellReference defines interface properties and methods that allow the owner to access fields and functionality in the AssortmentCell
	/// and AssortmentCube classes.
	/// </remarks>

	public class AssortmentCellReference : ComputationCellReference
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of the AssortmentCellReference class using the given AssortmentCube.
		/// </summary>
		/// <param name="aAssrtCube">
		/// A reference to the AssortmentCube where the AssortmentCell exists.
		/// </param>

		public AssortmentCellReference(AssortmentCube aAssrtCube)
			: base(aAssrtCube)
		{
		}

		/// <summary>
		/// Creates a new instance of the AssortmentCellReference class using the given AssortmentCube and CellCoordinates.
		/// </summary>
		/// <param name="aAssrtCube">
		/// A reference to the AssortmentCube where the AssortmentCell exists.
		/// </param>
		/// <param name="aCellCoordinates">
		/// The CellCoordinates that defines the AssortmentCell's position in the AssortmentCube.
		/// </param>

		public AssortmentCellReference(AssortmentCube aAssrtCube, CellCoordinates aCellCoordinates)
			: base(aAssrtCube, aCellCoordinates)
		{
		}

		/// <summary>
		/// Creates a new instance of the AssortmentCellReference class using the given AssortmentCube and CellReference.  A coordinate-mapping is performed to translate
		/// the coordinates of the given CellReference (that may be pointing to a different AssortmentCube) to the given AssortmentCube.
		/// </summary>
		/// <param name="aAssrtCube">
		/// A reference to the AssortmentCube where the AssortmentCell exists.
		/// </param>
		/// <param name="aCellReference">
		/// The CellReference that is to be mapped to the given AssortmentCube.
		/// </param>

		public AssortmentCellReference(AssortmentCube aAssrtCube, CellReference aCellReference)
			: base (aAssrtCube, aCellReference)
		{
		}

		//===========
		// PROPERTIES
		//===========

		// BEGIN TT#831-MD - Stodd - Need / Intransit not displayed
		// This code was moved to CellReference class.
		// BEGIN TT#2150 - stodd - totals not showing in main matrix grid
		/// <summary>
		/// Displays the cell's coordinates and values to help with debugging.
		/// </summary>
		//public string CellKeys
		//{
		//    get
		//    {
		//        string cellKeys = string.Empty;
		//        for (int i = 0; i < CellCoordinates.NumIndices; i++)
		//        {
		//            int key = CellCoordinates.GetRawCoordinate(i);
		//            DimensionDefinition prof = Cube.CubeDefinition[i];

		//            cellKeys += prof.ProfileType + " " + key + " ";
		//        }

		//        return cellKeys;
		//    }
		//}
		// END TT#2150 - stodd - totals not showing in main matrix grid
		// END TT#831-MD - Stodd - Need / Intransit not displayed

		/// <summary>
		/// Gets the Cube reference.
		/// </summary>

		public AssortmentCube AssortmentCube
		{
			get
			{
				try
				{
					return (AssortmentCube)Cube;
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

		public AssortmentCell AssortmentCell
		{
			get
			{
				try
				{
					return (AssortmentCell)Cell;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the Blocked flag of the ComputationCell.
		/// </summary>

		override public bool isCellBlocked
		{
			get
			{
				try
				{
					InitCellBlockedFlag();
					return AssortmentCell.isBlocked;
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
					AssortmentCell.isBlocked = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

        override public bool isCellLocked
        {
            get
            {
                try
                {
                    return AssortmentCell.isLocked;
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
                    AssortmentCell.isLocked = value;
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }

		/// <summary>
		/// Gets the Fixed flag of the ComputationCell.
		/// </summary>

		override public bool isCellFixed
		{
			get
			{
				try
				{
					InitCellFixedFlag();
					return AssortmentCell.isFixed;
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
					AssortmentCell.isFixed = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        /// <summary>
        /// Gets the Reinit flag of the ComputationCell.
        /// </summary>

        override public bool isCellReinit
        {
            get
            {
                try
                {
                    InitCellReinitFlag();
                    return AssortmentCell.isReinit;
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
                    AssortmentCell.isReinit = value;
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }

        //End TT#2 - JScott - Assortment Planning - Phase 2
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
							_computationCellRefExt = new AssortmentCellReferenceComparativeExtension(this);
						}
						else
						{
							_computationCellRefExt = new AssortmentCellReferenceValueExtension(this);
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

		// Begin TT#2 - stodd - assortment: linking to allocationProfile
		//override protected double CellValue
		//{
		//    get
		//    {
		//        try
		//        {
		//            return ComputationCell.Value;
		//        }
		//        catch (Exception exc)
		//        {
		//            string message = exc.ToString();
		//            throw;
		//        }
		//    }
		//    set
		//    {
		//        try
		//        {
		//            if (((AssortmentCube)this.Cube).LoadValuesFromHeader)
		//            {
		//                // Add extra code to set in allocation profile
		//                int hdrRid = this.CellCoordinates.GetRawCoordinate(0);
		//                int packRid = this.CellCoordinates.GetRawCoordinate(1);
		//                int packColorRid = this.CellCoordinates.GetRawCoordinate(2);
		//                int storeGroupLevelRid = this.CellCoordinates.GetRawCoordinate(3);
		//                int storeGrade = this.CellCoordinates.GetRawCoordinate(4);
		//                eAssortmentDetailVariables detailVar = (eAssortmentDetailVariables)this.CellCoordinates.GetRawCoordinate(5);
		//                eAssortmentQuantityVariables quantityVar = (eAssortmentQuantityVariables)this.CellCoordinates.GetRawCoordinate(6);

		//                ProfileList list = this.Cube.CubeGroup.GetMasterProfileList(eProfileType.StoreGroup);
		//                int sgRid = ((AssortmentCubeGroup)this.Cube.CubeGroup).AssortmentStoreGroupRID;

		//                AllocationProfile ap = this.Cube.Transaction.GetAllocationProfile(hdrRid);
		//                if (detailVar == eAssortmentDetailVariables.TotalUnits)
		//                {
		//                    if (this[eProfileType.HeaderPack] != int.MaxValue)
		//                    {
		//                        if (this[eProfileType.HeaderPackColor] != int.MaxValue)
		//                        {
		//                            // pack and color
		//                            ap.SetAllocatedUnits(this[eProfileType.HeaderPack], this[eProfileType.HeaderPackColor], sgRid, this[eProfileType.StoreGroupLevel], this[eProfileType.StoreGrade], (int)CellValue);
		//                        }
		//                        else
		//                        {
		//                            // Pack
		//                            ap.SetAllocatedPackUnits(this[eProfileType.HeaderPack], sgRid, this[eProfileType.StoreGroupLevel], this[eProfileType.StoreGrade], (int)CellValue);
		//                        }
		//                    }
		//                    else
		//                    {
		//                        if (this[eProfileType.HeaderPackColor] != int.MaxValue)
		//                        {
		//                            // color
		//                            ap.SetAllocatedColorUnits(this[eProfileType.HeaderPackColor], sgRid, this[eProfileType.StoreGroupLevel], this[eProfileType.StoreGrade], (int)CellValue);
		//                        }
		//                        else
		//                        {
		//                            ap.SetAllocatedUnits(sgRid, this[eProfileType.StoreGroupLevel], this[eProfileType.StoreGrade], (int)CellValue);
		//                        }
		//                    }
		//                }
		//            }
		//            ComputationCell.Value = value;
		//        }
		//        catch (Exception exc)
		//        {
		//            string message = exc.ToString();
		//            throw;
		//        }
		//    }
		//}
		// End TT#2 - stodd - assortment: linking to allocationProfile


		//========
		// METHODS
		//========

		/// <summary>
		/// Overridden method that creates a new ComputationScheduleFormulaEntry object from the given parameters.
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
			ComputationCellReference aChangeCompCellRef,
			FormulaProfile aFormulaProfile,
			eComputationScheduleEntryType aScheduleEntryType,
			int aSchedulePriority,
			int aCubePriority)
		{
			try
			{
				return new AssortmentScheduleFormulaEntry(aChangeCompCellRef, this, aFormulaProfile, aScheduleEntryType, aSchedulePriority, aCubePriority);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Overridden method that creates a new ComputationScheduleSpreadEntry object from the given parameters.
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
				return new AssortmentScheduleSpreadEntry(this, aSpreadProfile, aSchedulePriority, aCubePriority);
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

		override protected void InitCellBlockedFlag()
		{
			try
			{
				ComputationCellRefExt.InitCellBlockedFlag();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override protected void InitCellFixedFlag()
		{
			try
			{
				ComputationCellRefExt.InitCellFixedFlag();
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
		/// Creates a copy of the AssortmentCellReference.  The AssortmentCube is a shallow copy, while the AssortmentCell is a deep copy.
		/// </summary>
		/// <returns>
		/// A new instance of AssortmentCellReference with a copy of this objects information.
		/// </returns>

		override public CellReference Copy()
		{
			AssortmentCellReference assrtCellRef;

			try
			{
				assrtCellRef = new AssortmentCellReference((AssortmentCube)Cube);
				assrtCellRef.CopyFrom(this);

				return assrtCellRef;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Initializes the AssortmentCell.
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
		/// Sets the Value of a AssortmentCell from user entry.
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
		/// Sets the Value of a AssortmentCell from a database read.
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
        /// Sets the Lock of a AssortmentCell from a database read.
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
		override public void SetCompCellValue(eSetCellMode aSetCellMode, double aValue, bool isAsrtSimStore)
		{
			try
			{
				SetCompCellValue(aSetCellMode, aValue, isAsrtSimStore);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		// END TT#2200 - stodd - Open existing Assortment-> select REDO-> process-> MID Client stops working

		/// <summary>
		/// Sets the Value of a AssortmentCell from a computation.
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
				ComputationCellRefExt.SetCompCellValue(aSetCellMode, aValue);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the Value of a AssortmentCell for a copy.
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
		/// Sets the lock value of a AssortmentCell.
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
		/// Sets the Value of a AssortmentCell from a database read.
		/// </summary>
        /// <param name="aBlock">
		/// The value that is to be assigned to the Cell.
		/// </param>

		public void SetCellBlock(bool aBlock)
		{
			try
			{
				ComputationCellRefExt.SetCellBlock(aBlock);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin TT#1954-MD - JSmith - Assortment
        /// <summary>
        /// Initializes cell flag values.
        /// </summary>

        public void SumDetailCellFlags()
        {
            ArrayList cellRefList;
            bool isBlocked;
            bool isReadOnly;
            bool isDisplayOnly;
            bool isFixed;

            try
            {
                cellRefList = GetComponentDetailCellRefArray(false);

                if (cellRefList.Count > 0)
                {
                    isBlocked = true;
                    isReadOnly = true;
                    isDisplayOnly = true;
                    isFixed = true;

                    foreach (AssortmentCellReference asrtCellRef in cellRefList)
                    {
                        if (!asrtCellRef.isCellBlocked)
                        {
                            isBlocked = false;
                        }

                        if (!asrtCellRef.isCellReadOnly)
                        {
                            isReadOnly = false;
                        }

                        if (!asrtCellRef.isCellDisplayOnly)
                        {
                            isDisplayOnly = false;
                        }

                        if (!asrtCellRef.isCellFixed)
                        {
                            isFixed = false;
                        }

                        if (!isBlocked && !isReadOnly && !isDisplayOnly && !isFixed)
                        {
                            break;
                        }
                    }

                    SetCellBlock(isBlocked);

                    if (!AssortmentCell.isNull)
                    {
                        AssortmentCell.isReadOnly = isReadOnly;
                        AssortmentCell.isDisplayOnly = isDisplayOnly;
                        AssortmentCell.isFixed = isFixed;
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#1954-MD - JSmith - Assortment

		/// <summary>
		/// Initializes a block value.
		/// </summary>

		public void SumDetailCellBlocked()
		{
			ArrayList cellRefList;
			bool isBlocked;

			try
			{
				cellRefList = GetComponentDetailCellRefArray(false);

				if (cellRefList.Count > 0)
				{
					isBlocked = true;

					foreach (AssortmentCellReference asrtCellRef in cellRefList)
					{
						if (!asrtCellRef.isCellBlocked)
						{
							isBlocked = false;
							break;
						}
					}

					SetCellBlock(isBlocked);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Initializes a readonly value.
		/// </summary>

		public void SumDetailCellReadOnly()
		{
			ArrayList cellRefList;
			bool isReadOnly;

			try
			{
				cellRefList = GetComponentDetailCellRefArray(false);

				if (cellRefList.Count > 0)
				{
					isReadOnly = true;

					foreach (AssortmentCellReference asrtCellRef in cellRefList)
					{
						if (!asrtCellRef.isCellReadOnly)
						{
							isReadOnly = false;
							break;
						}
					}

					if (!AssortmentCell.isNull)
					{
						AssortmentCell.isReadOnly = isReadOnly;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Initializes a displayonly value.
		/// </summary>

		public void SumDetailCellDisplayOnly()
		{
			ArrayList cellRefList;
			bool isDisplayOnly;

			try
			{
				cellRefList = GetComponentDetailCellRefArray(false);

				if (cellRefList.Count > 0)
				{
					isDisplayOnly = true;

					foreach (AssortmentCellReference asrtCellRef in cellRefList)
					{
						if (!asrtCellRef.isCellDisplayOnly)
						{
							isDisplayOnly = false;
							break;
						}
					}

					if (!AssortmentCell.isNull)
					{
						AssortmentCell.isDisplayOnly = isDisplayOnly;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Initializes a fixed value.
		/// </summary>

		public void SumDetailCellFixed()
		{
			ArrayList cellRefList;
			bool isFixed;

			try
			{
				cellRefList = GetComponentDetailCellRefArray(false);

				if (cellRefList.Count > 0)
				{
					isFixed = true;

					foreach (AssortmentCellReference asrtCellRef in cellRefList)
					{
						if (!asrtCellRef.isCellFixed)
						{
							isFixed = false;
							break;
						}
					}

					if (!AssortmentCell.isNull)
					{
						AssortmentCell.isFixed = isFixed;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}

