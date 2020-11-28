using System;
using System.Collections;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using System.Diagnostics;

namespace MIDRetail.Business
{
	/// <summary>
	/// The ComputationCellReference class defines the interface to the ComputationCell/ComputationCube relationship.
	/// </summary>
	/// <remarks>
	/// The ComputationCellReference defines interface properties and methods that allow the owner to access fields and functionality in the ComputationCell
	/// and ComputationCube classes.
	/// </remarks>

	abstract public class ComputationCellReference : CellReference
	{
		//=======
		// FIELDS
		//=======

		protected ComputationCellReferenceExtension _computationCellRefExt;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of the ComputationCellReference class using the given ComputationCube.
		/// </summary>
        /// <param name="aCompCube">
		/// A reference to the ComputationCube where the ComputationCell exists.
		/// </param>

		public ComputationCellReference(ComputationCube aCompCube)
			: base(aCompCube)
		{
		}

		/// <summary>
		/// Creates a new instance of the ComputationCellReference class using the given ComputationCube and CellCoordinates.
		/// </summary>
        /// <param name="aCompCube">
		/// A reference to the ComputationCube where the ComputationCell exists.
		/// </param>
		/// <param name="aCellCoordinates">
		/// The CellCoordinates that defines the ComputationCell's position in the ComputationCube.
		/// </param>

		public ComputationCellReference(ComputationCube aCompCube, CellCoordinates aCellCoordinates)
			: base(aCompCube, aCellCoordinates)
		{
		}

		/// <summary>
		/// Creates a new instance of the ComputationCellReference class using the given ComputationCube and CellReference.  A coordinate-mapping is performed to translate
		/// the coordinates of the given CellReference (that may be pointing to a different ComputationCube) to the given ComputationCube.
		/// </summary>
        /// <param name="aCompCube">
		/// A reference to the ComputationCube where the ComputationCell exists.
		/// </param>
		/// <param name="aCellReference">
		/// The CellReference that is to be mapped to the given ComputationCube.
		/// </param>

		public ComputationCellReference(ComputationCube aCompCube, CellReference aCellReference)
			: base(aCompCube, aCellReference)
		{
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the ExtensionCellReference of the ComputationCell.
		/// </summary>

		abstract protected ComputationCellReferenceExtension ComputationCellRefExt { get; }

		/// <summary>
		/// Gets the Cell reference.
		/// </summary>

		override public Cell Cell
		{
			get
			{
				try
				{
					if (_cell == null)
					{
						if (ComputationCellRefExt.isShadowCell)
						{
							_cell = Cube.CreateCell(this);
						}
						else
						{
							_cell = Cube.GetCell(this);
						}
					}
					return _cell;
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

		public ComputationCube ComputationCube
		{
			get
			{
				try
				{
					return (ComputationCube)Cube;
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

		public ComputationCell ComputationCell
		{
			get
			{
				try
				{
					return (ComputationCell)Cell;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the Value of the ComputationCell.
		/// </summary>

		public double UninitializedCellValue
		{
			get
			{
				try
				{
					// Begin TT#2 - stodd- assortment
					//return ComputationCell.Value;
					return CellValue;
					// End TT#2 - stodd- assortment
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the Flags of the ComputationCell.
		/// </summary>

		public ComputationCellFlags CellFlags
		{
			get
			{
				try
				{
					InitCellValue();
					return ComputationCell.Flags;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the Initialized flag of the ComputationCell.
		/// </summary>

		public bool isCellInitialized
		{
			get
			{
				try
				{
					return ComputationCell.isInitialized;
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
					ComputationCell.isInitialized = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the Locked flag of the ComputationCell.
		/// </summary>

		virtual public bool isCellLocked
		{
			get
			{
				try
				{
					return ComputationCell.isLocked;
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
                    ComputationCell.isLocked = value;
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
		}

		/// <summary>
		/// Gets or sets the LoadedFromDB flag of the ComputationCell.
		/// </summary>

		public bool isCellLoadedFromDB
		{
			get
			{
				try
				{
					return ComputationCell.isLoadedFromDB;
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
					ComputationCell.isLoadedFromDB = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the Null flag of the ComputationCell.
		/// </summary>

		public bool isCellNull
		{
			get
			{
				try
				{
					InitCellValue();
					return ComputationCell.isNull;
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
					ComputationCell.isNull = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the ReadOnly flag of the ComputationCell.
		/// </summary>

		public bool isCellReadOnly
		{
			get
			{
				try
				{
					InitCellValue();
					return ComputationCell.isReadOnly;
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
					ComputationCell.isReadOnly = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the DisplayOnly flag of the ComputationCell.
		/// </summary>

		public bool isCellDisplayOnly
		{
			get
			{
				try
				{
					InitCellDisplayOnlyFlag();
					return ComputationCell.isDisplayOnly;
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
					ComputationCell.isDisplayOnly = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the Changed flag of the ComputationCell.
		/// </summary>

		public bool isCellChanged
		{
			get
			{
				try
				{
					InitCellValue();
					return ComputationCell.isChanged;
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
					ComputationCell.isChanged = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the Adjusted flag of the ComputationCell.
		/// </summary>

		public bool isCellAdjusted
		{
			get
			{
				try
				{
					return CurrentCellValue != PostInitCellValue;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the Hidden flag of the ComputationCell.
		/// </summary>

		public bool isCellHidden
		{
			get
			{
				try
				{
					if (!ComputationCell.isHiddenInitialized)
					{
						ComputationCell.isHiddenInitialized = true;
						ComputationCell.isHidden = !isCellDisplayable;
					}

					return ComputationCell.isHidden;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the ExtensionCreated flag of the ComputationCell.
		/// </summary>

		public bool isCellExtensionCreated
		{
			get
			{
				try
				{
					return ComputationCell.isExtensionCreated;
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
					ComputationCell.isExtensionCreated = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets or sets the Ineligible flag of the ComputationCell.
		/// </summary>

		virtual public bool isCellIneligible
		{
			get
			{
				return false;
			}
			set
			{
				throw new Exception("Invalid Call");
			}
		}

		/// <summary>
		/// Gets or sets the Protected flag of the ComputationCell.
		/// </summary>

		virtual public bool isCellProtected
		{
			get
			{
				return false;
			}
			set
			{
				throw new Exception("Invalid Call");
			}
		}

		/// <summary>
		/// Gets or sets the Closed flag of the ComputationCell.
		/// </summary>

		virtual public bool isCellClosed
		{
			get
			{
				return false;
			}
			set
			{
				throw new Exception("Invalid Call");
			}
		}

		/// <summary>
		/// Gets the Blocked flag of the ComputationCell.
		/// </summary>

		virtual public bool isCellBlocked
		{
			get
			{
				return false;
			}
			set
			{
				throw new Exception("Invalid Call");
			}
		}

		/// <summary>
		/// Gets the Fixed flag of the ComputationCell.
		/// </summary>

		virtual public bool isCellFixed
		{
			get
			{
				return false;
			}
			set
			{
				throw new Exception("Invalid Call");
			}
		}

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        /// <summary>
        /// Gets the Reinit flag of the ComputationCell.
        /// </summary>

        virtual public bool isCellReinit
        {
            get
            {
                return false;
            }
            set
            {
                throw new Exception("Invalid Call");
            }
        }

        //End TT#2 - JScott - Assortment Planning - Phase 2
        /// <summary>
		/// Method that checks to see if the coordinates have valid values in each indice, the variable is available to this cube, and computations have
		/// not marked this cell as null.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the cell is valid.
		/// </returns>

		public bool isCellValid
		{
			get
			{
				try
				{
					return CellCoordinates.areCoordinatesValid() && ComputationCube.isCellValid(this);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//Begin Track #6008 - JScott - B OTB Multi Level Errors Copy Low to High and Balance Low Levels
		/// <summary>
		/// Returns a boolean indicating if the Cell can be entered by the User.
		/// </summary>

		public bool isCellAvailableForEntry
		{
			get
			{
				try
				{
					InitCellValue();
					return ComputationCell.isCellAvailableForEntry;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the Cell can be initialize for the current value.
		/// </summary>

		public bool isCellAvailableForCurrentInitialization
		{
			get
			{
				try
				{
					InitCellValue();
					return ComputationCell.isCellAvailableForCurrentInitialization;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the Cell can be updated by a computation.
		/// </summary>

		public bool isCellAvailableForComputation
		{
			get
			{
				try
				{
					InitCellValue();
					return ComputationCell.isCellAvailableForComputation;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        ///// <summary>
        ///// Returns a boolean indicating if the Cell can be updated by a forced ReInit.
        ///// </summary>

        //public bool isCellAvailableForForcedReInit
        //{
        //    get
        //    {
        //        try
        //        {
        //            InitCellValue();
        //            return ComputationCell.isCellAvailableForForcedReInit;
        //        }
        //        catch (Exception exc)
        //        {
        //            string message = exc.ToString();
        //            throw;
        //        }
        //    }
        //}

        //End TT#2 - JScott - Assortment Planning - Phase 2
        /// <summary>
		/// Returns a boolean indicating if the Cell can be updated by a copy.
		/// </summary>

		public bool isCellAvailableForCopy
		{
			get
			{
				try
				{
					InitCellValue();
					return ComputationCell.isCellAvailableForCopy;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the Cell can be locked by the User.
		/// </summary>

		public bool isCellAvailableForLocking
		{
			get
			{
				try
				{
					InitCellValue();
					return ComputationCell.isCellAvailableForLocking;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//End Track #6008 - JScott - B OTB Multi Level Errors Copy Low to High and Balance Low Levels
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

		public bool canCellBeScheduled
		{
			get
			{
				try
				{
					return ComputationCube.canCellBeScheduled(this);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//End Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
		//Begin Track #4126 - JScott - Invalid "Temp Locked" message when first week of spread is locked
		/// <summary>
		/// Gets or sets the ExcludedFromSpread flag of the ComputationCell.
		/// </summary>

		public bool isCellExcludedFromSpread
		{
			get
			{
				try
				{
					return GetExtensionCell(true).GetCompInfo(this).isExcludedFromSpread;
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
					GetExtensionCell(true).GetCompInfo(this).isExcludedFromSpread = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//End Track #4126 - JScott - Invalid "Temp Locked" message when first week of spread is locked
		/// <summary>
		/// Gets the current Value of the ComputationCell, zeroing if hidden.
		/// </summary>

		public double CurrentCellValue
		{
			get
			{
				try
				{
					return GetCurrentCellValue(false);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
				throw;
				}
			}
		}

		/// <summary>
		/// Gets the pre-init Value of the ComputationCell before any changes were made, zeroing if hidden.
		/// </summary>

		public double PreInitCellValue
		{
			get
			{
				try
				{
					return GetPreInitCellValue(false);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the post-init Value of the ComputationCell before any changes were made, zeroing if hidden.
		/// </summary>

		public double PostInitCellValue
		{
			get
			{
				try
				{
					return GetPostInitCellValue(false);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the previous Value of the ComputationCell before any changes were made in the current recalc, zeroing if hidden.
		/// </summary>

		public double PreviousCellValue
		{
			get
			{
				try
				{
					return GetPreviousCellValue(false);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the current Value of the ComputationCell.
		/// </summary>

		public double HiddenCurrentCellValue
		{
			get
			{
				try
				{
					return GetCurrentCellValue(true);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the pre-init Value of the ComputationCell before any changes were made.
		/// </summary>

		public double HiddenPreInitCellValue
		{
			get
			{
				try
				{
					return GetPreInitCellValue(true);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the post-init Value of the ComputationCell before any changes were made.
		/// </summary>

		public double HiddenPostInitCellValue
		{
			get
			{
				try
				{
					return GetPostInitCellValue(true);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the previous Value of the ComputationCell before any changes were made.
		/// </summary>

		public double HiddenPreviousCellValue
		{
			get
			{
				try
				{
					return GetPreviousCellValue(true);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the CompChanged flag of the ComputationInfo object of the ComputationCell.
		/// </summary>

		public bool isCellCompChanged
		{
			get
			{
				ExtensionCell extCell;

				try
				{
					extCell = GetExtensionCell(false);
					if (extCell != null)
					{
						return extCell.GetCompInfo(this).isCompChanged;
					}
					else
					{
						return false;
					}
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
					GetExtensionCell(true).GetCompInfo(this).isCompChanged = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the CompLocked flag of the ComputationInfo object of the ComputationCell.
		/// </summary>

		public bool isCellCompLocked
		{
			get
			{
				ExtensionCell extCell;

				try
				{
					extCell = GetExtensionCell(false);
					if (extCell != null)
					{
						return extCell.GetCompInfo(this).isCompLocked;
					}
					else
					{
						return false;
					}
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
					GetExtensionCell(true).GetCompInfo(this).isCompLocked = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//Begin Track #5752 - JScott - Calculation Time
		/// <summary>
		/// Gets the AutoTotalsProcessed flag of the ComputationInfo object of the ComputationCell.
		/// </summary>

		public bool isCellAutoTotalsProcessed
		{
			get
			{
				ExtensionCell extCell;

				try
				{
					extCell = GetExtensionCell(false);
					if (extCell != null)
					{
						return extCell.GetCompInfo(this).isAutoTotalsProcessed;
					}
					else
					{
						return false;
					}
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
					GetExtensionCell(true).GetCompInfo(this).isAutoTotalsProcessed = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//End Track #5752 - JScott - Calculation Time
		/// <summary>
		/// Gets the UserChanged flag of the ComputationInfo object of the ComputationCell.
		/// </summary>

		public bool isCellUserChanged
		{
			get
			{
				ExtensionCell extCell;

				try
				{
					extCell = GetExtensionCell(false);
					if (extCell != null)
					{
						return extCell.GetCompInfo(this).isUserChanged;
					}
					else
					{
						return false;
					}
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
					GetExtensionCell(true).GetCompInfo(this).isUserChanged = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the ComputationScheduleEntry of the ComputationInfo object of the PlanCell.
		/// </summary>

		public ComputationScheduleFormulaEntry CellScheduledFormula
		{
			get
			{
				ExtensionCell extCell;

				try
				{
					extCell = GetExtensionCell(false);
					if (extCell != null)
					{
						return extCell.GetCompInfo(this).ScheduledFormula;
					}
					else
					{
						return null;
					}
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
					GetExtensionCell(true).GetCompInfo(this).ScheduledFormula = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the ComputationScheduleEntry of the ComputationInfo object of the PlanCell.
		/// </summary>

		public ComputationScheduleSpreadEntry CellScheduledSpread
		{
			get
			{
				ExtensionCell extCell;

				try
				{
					extCell = GetExtensionCell(false);
					if (extCell != null)
					{
						return extCell.GetCompInfo(this).ScheduledSpread;
					}
					else
					{
						return null;
					}
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
					GetExtensionCell(true).GetCompInfo(this).ScheduledSpread = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        ///// <summary>
        ///// Gets the ComputationScheduleEntry of the ComputationInfo object of the ComputationCell.
        ///// </summary>

        //public ComputationScheduleEntry CellPostExecuteReInitScheduleEntry
        //{
        //    get
        //    {
        //        ExtensionCell extCell;

        //        try
        //        {
        //            extCell = GetExtensionCell(false);
        //            if (extCell != null)
        //            {
        //                return extCell.GetCompInfo(this).PostExecuteReInitScheduleEntry;
        //            }
        //            else
        //            {
        //                return null;
        //            }
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
        //            GetExtensionCell(true).GetCompInfo(this).PostExecuteReInitScheduleEntry = value;
        //        }
        //        catch (Exception exc)
        //        {
        //            string message = exc.ToString();
        //            throw;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Gets the ComputationScheduleEntry of the ComputationInfo object of the ComputationCell.
        ///// </summary>

        //public ComputationScheduleEntry CellPostExecuteReInitTriggerScheduleEntry
        //{
        //    set
        //    {
        //        try
        //        {
        //            GetExtensionCell(true).GetCompInfo(this).PostExecuteReInitTriggerScheduleEntry = value;
        //        }
        //        catch (Exception exc)
        //        {
        //            string message = exc.ToString();
        //            throw;
        //        }
        //    }
        //}

        //End TT#2 - JScott - Assortment Planning - Phase 2
        /// <summary>
		/// Returns a boolean indicating if this ComputationCell is displayable.
		/// </summary>

		public bool isCellDisplayable
		{
			get
			{
				try
				{
					return ComputationCube.isCellDisplayable(this);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Returns the eProfileType for the Variable for this ComputationCell.
		/// </summary>

		public eProfileType VariableProfileType
		{
			get
			{
				try
				{
					return ComputationCube.VariableProfileType;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Returns the eProfileType for the Quantity Variable for this ComputationCell.
		/// </summary>

		public eProfileType QuantityVariableProfileType
		{
			get
			{
				try
				{
					return ComputationCube.QuantityVariableProfileType;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		// Begin TT#2 - stodd - assortment: linking to allocationProfile
		virtual protected double CellValue
		{
			get
			{
				try
				{
					return ComputationCell.Value;
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
					ComputationCell.Value = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
		// End TT#2 - stodd - assortment: linking to allocationProfile


		//========
		// METHODS
		//========

		abstract protected void InitCellDisplayOnlyFlag();

		virtual protected void InitCellIneligibleFlag()
		{
			throw new Exception("Invalid Call");
		}

		virtual protected void InitCellClosedFlag()
		{
			throw new Exception("Invalid Call");
		}

		virtual protected void InitCellProtectedFlag()
		{
			throw new Exception("Invalid Call");
		}

		virtual protected void InitCellBlockedFlag()
		{
			throw new Exception("Invalid Call");
		}

		virtual protected void InitCellFixedFlag()
		{
			throw new Exception("Invalid Call");
		}

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        virtual protected void InitCellReinitFlag()
        {
            throw new Exception("Invalid Call");
        }

        //End TT#2 - JScott - Assortment Planning - Phase 2
        abstract protected ExtensionCell GetExtensionCell(bool aAllocate);

		/// <summary>
		/// Initializes the ComputationCell.
		/// </summary>

		abstract public void InitCellValue();

		//Begin Track #5712 - JScott - Issue with calcs with copy method - Part 2
		/// <summary>
		/// Reads the ComputationCell.
		/// </summary>

		abstract public void ReadCellValue();

		//End Track #5712 - JScott - Issue with calcs with copy method - Part 2
		/// <summary>
		/// Sets the Value of a ComputationCell from user entry.
		/// </summary>
		/// <param name="aValue">
		/// The value that is to be assigned to the Cell.
		/// </param>

		abstract public void SetEntryCellValue(double aValue);

		/// <summary>
		/// Sets the Value of a ComputationCell from a database read.
		/// </summary>
		/// <param name="aValue">
		/// The value that is to be assigned to the Cell.
		/// </param>

		abstract public void SetLoadCellValue(double aValue, bool aLock);

        // Begin TT#TT#739-MD - JSmith - delete stores
        /// <summary>
        /// Sets the Lock of a ComputationCell from a database read.
        /// </summary>
        /// <param name="aLock">
        /// The lock that is to be assigned to the Cell.
        /// </param>

        abstract public void SetLoadCellLock(bool aLock);
        // End TT#TT#739-MD - JSmith - delete stores

		/// <summary>
		/// Sets the Value of a ComputationCell from a computation.
		/// </summary>
		/// <param name="aSetCellMode">
		/// The eSetCellMode indicating the type of change that is occurring, include Initialize or Entry.
		/// </param>
		/// <param name="aValue">
		/// The value that is to be assigned to the Cell.
		/// </param>

		abstract public void SetCompCellValue(eSetCellMode aSetCellMode, double aValue);

		// BEGIN TT#2200 - stodd - Open existing Assortment-> select REDO-> process-> MID Client stops working
		abstract public void SetCompCellValue(eSetCellMode aSetCellMode, double aValue, bool isAsrtSimStore);
		// END TT#2200 - stodd - Open existing Assortment-> select REDO-> process-> MID Client stops working

		/// <summary>
		/// Sets the Value of a ComputationCell for a copy.
		/// </summary>
		/// <param name="aValue">
		/// The value that is to be assigned to the Cell.
		/// </param>

		abstract public void SetCopyCellValue(double aValue, bool aLock);

		/// <summary>
		/// Sets the value of the ComputationCell's lock flag.
		/// </summary>
		/// <param name="aValue">
		/// A boolean containing the new lock flag.
		/// </param>

		abstract public void SetCellLock(bool aValue);

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

		abstract public ComputationScheduleFormulaEntry CreateScheduleFormulaEntry(
			ComputationCellReference aChangedCellRef,
			FormulaProfile aFormulaProfile,
			eComputationScheduleEntryType aScheduleEntryType,
			int aSchedulePriority,
			int aCubePriority);

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

		abstract public ComputationScheduleSpreadEntry CreateScheduleSpreadEntry(
			SpreadProfile aSpreadProfile,
			int aSchedulePriority,
			int aCubePriority);

		/// <summary>
		/// This method is called when the CellReference is to be reset.
		/// </summary>

		override public void Reset()
		{
			try
			{
				base.Reset();
				_computationCellRefExt = null;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the Pending indicator of the ComputationInfo object of the ComputationCell.
		/// </summary>

		public bool isCellFormulaPending(ComputationScheduleEntry aScheduleEntry)
		//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
		{
			try
			{
				return isCellFormulaPending(aScheduleEntry, false);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the Pending indicator of the ComputationInfo object of the ComputationCell.
		/// </summary>

		public bool isCellFormulaPending(ComputationScheduleEntry aScheduleEntry, bool aComputationScheduleUseOnly)
		//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
		{
			ExtensionCell extCell;

			try
			{
				//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
				if (!aComputationScheduleUseOnly)
				{
					InitCellValue();
				}

				//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
				extCell = GetExtensionCell(false);
				if (extCell != null)
				{
					return extCell.GetCompInfo(this).isFormulaPending(aScheduleEntry);
				}
				else
				{
					return false;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the Pending indicator of the ComputationInfo object of the ComputationCell.
		/// </summary>

		public bool isCellSpreadPending(ComputationScheduleEntry aScheduleEntry)
		//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
		{
			try
			{
				return isCellSpreadPending(aScheduleEntry, false);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the Pending indicator of the ComputationInfo object of the ComputationCell.
		/// </summary>

		public bool isCellSpreadPending(ComputationScheduleEntry aScheduleEntry, bool aComputationScheduleUseOnly)
		//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
		{
			ExtensionCell extCell;

			try
			{
				//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
				if (!aComputationScheduleUseOnly)
				{
					InitCellValue();
				}

				//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
				extCell = GetExtensionCell(false);
				if (extCell != null)
				{
					return extCell.GetCompInfo(this).isSpreadPending(aScheduleEntry);
				}
				else
				{
					return false;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the current Value of the ComputationCell, zeroing if hidden.
		/// </summary>

		public double GetCurrentCellValue(bool aShowHidden)
		{
			try
			{
				InitCellValue();
				if (isCellNull || isCellBlocked || (!aShowHidden && ComputationCell.isHidden))
				{
					return 0;
				}
				else
				{
					// Begin TT#2 - stodd- assortment
					//return ComputationCell.Value;
					return CellValue;
					// End TT#2 - stodd- assortment
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the pre-init Value of the ComputationCell before any changes were made, zeroing if hidden.
		/// </summary>

		public double GetPreInitCellValue(bool aShowHidden)
		{
			ExtensionCell extCell;

			try
			{
				//Begin Track #5712 - JScott - Issue with calcs with copy method - Part 2
				ReadCellValue();
				//End Track #5712 - JScott - Issue with calcs with copy method - Part 2
				//Begin Track #5712 - JScott - Issue with calcs with copy method
				//if (isCellNull || isCellBlocked || (!aShowHidden && ComputationCell.isHidden))
				if (ComputationCell.isNull || (!aShowHidden && ComputationCell.isHidden))
				//End Track #5712 - JScott - Issue with calcs with copy method
				{
					return 0;
				}
				else
				{
					extCell = GetExtensionCell(false);
					if (extCell != null && extCell.isPreInitValueSet)
					{
						return extCell.PreInitValue;
					}
					else
					{

						// Begin TT#2 - stodd- assortment
						//return ComputationCell.Value;
						return CellValue;
						// End TT#2 - stodd- assortment
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
		/// Gets the post-init Value of the ComputationCell before any changes were made, zeroing if hidden.
		/// </summary>

		public double GetPostInitCellValue(bool aShowHidden)
		{
			ExtensionCell extCell;

			try
			{
				InitCellValue();
				if (isCellNull || isCellBlocked || (!aShowHidden && ComputationCell.isHidden))
				{
					return 0;
				}
				else
				{
					extCell = GetExtensionCell(false);
					if (extCell != null && extCell.isPostInitValueSet)
					{
						return extCell.PostInitValue;
					}
					else
					{
						// Begin TT#2 - stodd- assortment
						//return ComputationCell.Value;
						return CellValue;
						// End TT#2 - stodd- assortment
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
		/// Gets the previous Value of the ComputationCell before any changes were made in the current recalc, zeroing if hidden.
		/// </summary>

		public double GetPreviousCellValue(bool aShowHidden)
		{
			ExtensionCell extCell;

			try
			{
				InitCellValue();
				if (isCellNull || isCellBlocked || (!aShowHidden && ComputationCell.isHidden))
				{
					return 0;
				}
				else
				{
					extCell = GetExtensionCell(false);
					if (extCell != null && extCell.GetCompInfo(this).PreviousValue != null)
					{
						return extCell.GetCompInfo(this).PreviousValue.Value;
					}
					else
					{
						// Begin TT#2 - stodd- assortment
						//return ComputationCell.Value;
						return CellValue;
						// End TT#2 - stodd- assortment
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
		/// Gets a count of details.  This count includes all of the ComputationCells that are details of this ComputationCubeCell.
		/// </summary>
		/// <returns>
		/// An integer that contains the count of the detail ComputationCells.
		/// </returns>

		public int GetDetailCount(eCubeType aCubeType, bool aUseHiddenValues)
		{
			ComputationCellCounter cellCounter;

			try
			{
				cellCounter = new ComputationCellCounter(aUseHiddenValues);
				ComputationCube.ProcessDetailCellSelector(this, aCubeType, cellCounter);
				return cellCounter.ItemCount;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an ArrayList of ComputationCellReferences.  This ArrayList contains all of the ComputationCells that are details of this ComputationCubeCell.
		/// </summary>
		/// <returns>
		/// The ArrayList of ComputationCellReferences.
		/// </returns>

		public ArrayList GetDetailCellRefArray(eCubeType aCubeType, bool aUseHiddenValues)
		{
			ComputationCellSelector cellSelector;

			try
			{
				cellSelector = new ComputationCellSelector(aUseHiddenValues);
				ComputationCube.ProcessDetailCellSelector(this, aCubeType, cellSelector);
				return cellSelector.CellRefList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets a count of details.  This count includes all of the ComputationCells that are details of this ComputationCubeCell.
		/// </summary>
		/// <returns>
		/// An integer that contains the count of the detail ComputationCells.
		/// </returns>

		public int GetComponentDetailCount(bool aUseHiddenValues)
		{
			ComputationCellCounter cellCounter;

			try
			{
				cellCounter = new ComputationCellCounter(aUseHiddenValues);
				ComputationCube.ProcessComponentDetailCellSelector(this, cellCounter);
				return cellCounter.ItemCount;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an ArrayList of ComputationCellReferences.  This ArrayList contains all of the ComputationCells that are details of this ComputationCubeCell.
		/// </summary>
		/// <returns>
		/// The ArrayList of ComputationCellReferences.
		/// </returns>

		public ArrayList GetComponentDetailCellRefArray(bool aUseHiddenValues)
		{
			ComputationCellSelector cellSelector;

			try
			{
				cellSelector = new ComputationCellSelector(aUseHiddenValues);
				ComputationCube.ProcessComponentDetailCellSelector(this, cellSelector);
				return cellSelector.CellRefList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an ArrayList of ComputationCellReferences.  This ArrayList contains all of the ComputationCells that are details of this ComputationCubeCell.
		/// </summary>
		/// <returns>
		/// The ArrayList of ComputationCellReferences.
		/// </returns>

		public bool GetComponentDetailStoreClosed()
		{
			ComputationCellStoreClosed cellSelector;

			try
			{
				cellSelector = new ComputationCellStoreClosed();
				ComputationCube.ProcessComponentDetailCellSelector(this, cellSelector);
				return cellSelector.isStoreClosed;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an ArrayList of ComputationCellReferences.  This ArrayList contains all of the ComputationCells that are details of this ComputationCubeCell.
		/// </summary>
		/// <returns>
		/// The ArrayList of ComputationCellReferences.
		/// </returns>

		public bool GetComponentDetailVersionProtected()
		{
			ComputationCellVersionProtected cellSelector;

			try
			{
				cellSelector = new ComputationCellVersionProtected();
				ComputationCube.ProcessComponentDetailCellSelector(this, cellSelector);
				return cellSelector.isVersionProtected;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #5669 - JScott - BMU %
		/// <summary>
		/// Gets an ArrayList of ComputationCellReferences.  This ArrayList contains all of the ComputationCells that are details of this ComputationCubeCell.
		/// </summary>
		/// <returns>
		/// The ArrayList of ComputationCellReferences.
		/// </returns>

		public bool GetComponentDetailDisplayOnly()
		{
			ComputationCellDisplayOnly cellSelector;

			try
			{
				cellSelector = new ComputationCellDisplayOnly();
				ComputationCube.ProcessComponentDetailCellSelector(this, cellSelector);
				return cellSelector.isDisplayOnly;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End Track #5669 - JScott - BMU %
		/// <summary>
		/// Gets an ArrayList of ComputationCellReferences.  This ArrayList contains all of the ComputationCells that are details of this ComputationCubeCell.
		/// </summary>
		/// <returns>
		/// The ArrayList of ComputationCellReferences.
		/// </returns>

		public bool GetComponentDetailStoreIneligible()
		{
			ComputationCellStoreIneligible cellSelector;

			try
			{
				cellSelector = new ComputationCellStoreIneligible();
				ComputationCube.ProcessComponentDetailCellSelector(this, cellSelector);
				return cellSelector.isStoreIneligible;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an ArrayList of ComputationCellReferences.  This ArrayList contains all of the ComputationCells that are details of this ComputationCubeCell.
		/// </summary>
		/// <returns>
		/// The ArrayList of ComputationCellReferences.
		/// </returns>

		public double GetComponentDetailSum(eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, bool aUseHiddenValues)
		{
			try
			{
				return GetComponentDetailSum(null, aGetCellMode, aSetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an ArrayList of ComputationCellReferences.  This ArrayList contains all of the ComputationCells that are details of this ComputationCubeCell.
		/// </summary>
		/// <returns>
		/// The ArrayList of ComputationCellReferences.
		/// </returns>

		public double GetComponentDetailSum(ComputationScheduleEntry aScheduleEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, bool aUseHiddenValues)
		{
			ComputationCellSum cellSelector;

			try
			{
				cellSelector = new ComputationCellSum(aScheduleEntry, aGetCellMode, aSetCellMode, aUseHiddenValues);
				ComputationCube.ProcessComponentDetailCellSelector(this, cellSelector);
                //if (aScheduleEntry is AssortmentScheduleFormulaEntry)
                //{
                //    Debug.WriteLine(((AssortmentScheduleFormulaEntry)aScheduleEntry).AssortmentCellRef.CellKeys + cellSelector.Sum);
                //}
				return cellSelector.Sum;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an ArrayList of ComputationCellReferences.  This ArrayList contains all of the ComputationCells that are details of this ComputationCubeCell.
		/// </summary>
		/// <returns>
		/// The ArrayList of ComputationCellReferences.
		/// </returns>

		public double GetComponentDetailAverage(
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			bool aUseHiddenValues)
		{
			try
			{
				return GetComponentDetailAverage(null, aGetCellMode, aSetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an ArrayList of ComputationCellReferences.  This ArrayList contains all of the ComputationCells that are details of this ComputationCubeCell.
		/// </summary>
		/// <returns>
		/// The ArrayList of ComputationCellReferences.
		/// </returns>

		public double GetComponentDetailAverage(
			ComputationScheduleEntry aScheduleEntry, 
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			bool aUseHiddenValues)
		{
			ComputationCellAverageSum cellSelector;

			try
			{
				cellSelector = new ComputationCellAverageSum(aScheduleEntry, aGetCellMode, aSetCellMode, null, aUseHiddenValues);
				ComputationCube.ProcessComponentDetailCellSelector(this, cellSelector);
				return cellSelector.Sum / cellSelector.Count;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an ArrayList of ComputationCellReferences.  This ArrayList contains all of the ComputationCells that are details of this ComputationCubeCell.
		/// </summary>
		/// <returns>
		/// The ArrayList of ComputationCellReferences.
		/// </returns>

		public double GetComponentDetailAverage(
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			ArrayList aCellRefArray,
			out int aStoreCount,
			bool aUseHiddenValues)
		{
			try
			{
				return GetComponentDetailAverage(null, aGetCellMode, aSetCellMode, aCellRefArray, out aStoreCount, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an ArrayList of ComputationCellReferences.  This ArrayList contains all of the ComputationCells that are details of this ComputationCubeCell.
		/// </summary>
		/// <returns>
		/// The ArrayList of ComputationCellReferences.
		/// </returns>

		public double GetComponentDetailAverage(
			ComputationScheduleEntry aScheduleEntry, 
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			ArrayList aCellRefArray,
			out int aStoreCount,
			bool aUseHiddenValues)
		{
			ComputationCellAverageSum cellSelector;
			bool cancel;

			try
			{
				cellSelector = new ComputationCellAverageSum(aScheduleEntry, aGetCellMode, aSetCellMode, null, aUseHiddenValues);

				foreach (ComputationCellReference compCellRef in aCellRefArray)
				{
					cellSelector.CheckItem(compCellRef, out cancel);
					if (cancel)
					{
						break;
					}
				}

				aStoreCount = cellSelector.Count;
				return cellSelector.Sum / cellSelector.Count;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an ArrayList of ComputationCellReferences.  This ArrayList contains all of the ComputationCells that are details of this ComputationCubeCell.
		/// </summary>
		/// <returns>
		/// The ArrayList of ComputationCellReferences.
		/// </returns>

		public double GetComponentDetailNonZeroAverage(
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			ComputationVariableProfile aVarProf,
			bool aUseHiddenValues)
		{
			try
			{
				return GetComponentDetailNonZeroAverage(null, aGetCellMode, aSetCellMode, aVarProf, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an ArrayList of ComputationCellReferences.  This ArrayList contains all of the ComputationCells that are details of this ComputationCubeCell.
		/// </summary>
		/// <returns>
		/// The ArrayList of ComputationCellReferences.
		/// </returns>

		public double GetComponentDetailNonZeroAverage(
			ComputationScheduleEntry aScheduleEntry, 
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			ComputationVariableProfile aVarProf,
			bool aUseHiddenValues)
		{
			ComputationCellAverageSum cellSelector;

			try
			{
				cellSelector = new ComputationCellAverageSum(aScheduleEntry, aGetCellMode, aSetCellMode, aVarProf, aUseHiddenValues);
				ComputationCube.ProcessComponentDetailCellSelector(this, cellSelector);
				return cellSelector.Sum / cellSelector.NonZeroCount;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an ArrayList of ComputationCellReferences.  This ArrayList contains all of the ComputationCells that are details of this ComputationCubeCell.
		/// </summary>
		/// <returns>
		/// The ArrayList of ComputationCellReferences.
		/// </returns>

		public double GetComponentDetailNonZeroAverage(
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			ArrayList aCellRefArray,
			ComputationVariableProfile aVarProf,
			out int aStoreCount,
			bool aUseHiddenValues)
		{
			try
			{
				return GetComponentDetailNonZeroAverage(null, aGetCellMode, aSetCellMode, aCellRefArray, aVarProf, out aStoreCount, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an ArrayList of ComputationCellReferences.  This ArrayList contains all of the ComputationCells that are details of this ComputationCubeCell.
		/// </summary>
		/// <returns>
		/// The ArrayList of ComputationCellReferences.
		/// </returns>

		public double GetComponentDetailNonZeroAverage(
			ComputationScheduleEntry aScheduleEntry, 
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			ArrayList aCellRefArray,
			ComputationVariableProfile aVarProf,
			out int aStoreCount,
			bool aUseHiddenValues)
		{
			ComputationCellAverageSum cellSelector;
			bool cancel;

			try
			{
				cellSelector = new ComputationCellAverageSum(aScheduleEntry, aGetCellMode, aSetCellMode, aVarProf, aUseHiddenValues);

				foreach (ComputationCellReference compCellRef in aCellRefArray)
				{
					cellSelector.CheckItem(compCellRef, out cancel);
					if (cancel)
					{
						break;
					}
				}

				aStoreCount = cellSelector.NonZeroCount;
				return cellSelector.Sum / cellSelector.NonZeroCount;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an ArrayList of ComputationCellReferences.  This ArrayList contains all of the ComputationCells that are details of this ComputationCubeCell.
		/// </summary>
		/// <returns>
		/// The ArrayList of ComputationCellReferences.
		/// </returns>

		public ArrayList GetSpreadDetailCellRefArray(bool aUseHiddenValues)
		{
			ComputationCellSelector cellSelector;

			try
			{
				cellSelector = new ComputationCellSelector(aUseHiddenValues);
				ComputationCube.ProcessSpreadDetailCellSelector(this, cellSelector);
				return cellSelector.CellRefList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Adds the given integer formula Id to the ScheduledSpreadsArray ArrayList of the ComputationInfo object of the ComputationCell.
		/// </summary>
		/// <param name="aFormulaId">
		/// The Id of the formula that is to be added to the ScheduledSpreadsArray.
		/// </param>

		public void AddCellScheduledSpread(int aFormulaId)
		{
			try
			{
				GetExtensionCell(true).GetCompInfo(this).ScheduledSpreadsArray.Add(aFormulaId);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #4126 - JScott - Invalid "Temp Locked" message when first week of spread is locked
		/// <summary>
		/// Removes the given integer formula Id from the ScheduledSpreadsArray ArrayList of the ComputationInfo object of the ComputationCell.
		/// </summary>
		/// <param name="aFormulaId">
		/// The Id of the formula that is to be removed from the ScheduledSpreadsArray.
		/// </param>

		public void RemoveCellScheduledSpread(int aFormulaId)
		{
			ExtensionCell extCell;

			try
			{
				extCell = GetExtensionCell(false);
				if (extCell != null)
				{
					extCell.GetCompInfo(this).ScheduledSpreadsArray.Remove(aFormulaId);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End Track #4126 - JScott - Invalid "Temp Locked" message when first week of spread is locked
		/// <summary>
		/// Checks to see if the ScheduledSpreadsArray ArrayList of the ComputationInfo object of the ComputationCell contains the given integer formula Id.
		/// </summary>
		/// <param name="aFormulaId">
		/// The Id of the formula that is to be checked.
		/// </param>
		/// <returns>
		/// A boolean indicating if the ComputationCell contains the given integer formula Id.
		/// </returns>

		public bool isCellSpreadScheduled(int aFormulaId)
		{
			ExtensionCell extCell;

			try
			{
				extCell = GetExtensionCell(false);
				if (extCell != null)
				{
					return extCell.GetCompInfo(this).ScheduledSpreadsArray.Contains(aFormulaId);
				}
				else
				{
					return false;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Clears the scheduled formula in the ComputationInfo object pointed to by the ComputationCell.
		/// </summary>

		public void ClearCellScheduledFormula()
		{
			ExtensionCell extCell;

			try
			{
				extCell = GetExtensionCell(false);
				if (extCell != null)
				{
					extCell.GetCompInfo(this).ClearScheduledFormula();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Clears the scheduled spread in the ComputationInfo object pointed to by the PlanCell.
		/// </summary>

		public void ClearCellScheduledSpread()
		{
			ExtensionCell extCell;

			try
			{
				extCell = GetExtensionCell(false);
				if (extCell != null)
				{
					extCell.GetCompInfo(this).ClearScheduledSpread();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        ///// <summary>
        ///// Clears the scheduled post spread reinit formula in the ComputationInfo object pointed to by the ComputationCell.
        ///// </summary>

        //public void ClearCellPostExecuteReInitScheduledFormula()
        //{
        //    ExtensionCell extCell;

        //    try
        //    {
        //        extCell = GetExtensionCell(false);
        //        if (extCell != null)
        //        {
        //            extCell.GetCompInfo(this).ClearPostExecuteReInitScheduledFormula();
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        string message = exc.ToString();
        //        throw;
        //    }
        //}


        //End TT#2 - JScott - Assortment Planning - Phase 2
        /// <summary>
		/// Public method that returns an operand Cell value.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return.
		/// </param>
		/// <returns>
		/// A double containing the ComputationCubeCell's value
		/// </returns>

		public double GetCellValue(eGetCellMode aGetCellMode, bool aUseHiddenValues)
		{
			try
			{
				switch (aGetCellMode)
				{
					case eGetCellMode.Current:

						if (aUseHiddenValues)
						{
							return HiddenCurrentCellValue;
						}
						else
						{
							return CurrentCellValue;
						}

					case eGetCellMode.Previous:

						if (aUseHiddenValues)
						{
							return HiddenPreviousCellValue;
						}
						else
						{
							return PreviousCellValue;
						}

					case eGetCellMode.PreInit:

						if (aUseHiddenValues)
						{
							return HiddenPreInitCellValue;
						}
						else
						{
							return PreInitCellValue;
						}

					case eGetCellMode.PostInit:

						if (aUseHiddenValues)
						{
							return HiddenPostInitCellValue;
						}
						else
						{
							return PostInitCellValue;
						}

					default:

						throw new MIDException(eErrorLevel.severe,
							(int)eMIDTextCode.msg_pl_InvalidCall,
							MIDText.GetText(eMIDTextCode.msg_pl_InvalidCall));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Clears the changes in the Cells after a save.
		/// </summary>

		public void ClearCellChanges()
		{
			try
			{
				isCellChanged = false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns the VariableProfile object used for calculation for the Cell.
		/// </summary>

		public ComputationVariableProfile GetCalcVariableProfile()
		{
			try
			{
				return ComputationCube.GetVariableProfile(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns the VariableProfile object used for format type for the Cell.
		/// </summary>

		public ComputationVariableProfile GetFormatTypeVariableProfile()
		{
			try
			{
				return ComputationCube.GetFormatTypeVariableProfile(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns the VariableProfile object used for variable scope for the Cell.
		/// </summary>

		public ComputationVariableProfile GetVariableScopeVariableProfile()
		{
			try
			{
				return ComputationCube.GetVariableScopeVariableProfile(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns the VariableProfile object used for variable style for the Cell.
		/// </summary>

		public ComputationVariableProfile GetVariableStyleVariableProfile()
		{
			try
			{
				return ComputationCube.GetVariableStyleVariableProfile(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns the VariableProfile object used for variable access for the Cell.
		/// </summary>

		public ComputationVariableProfile GetVariableAccessVariableProfile()
		{
			try
			{
				return ComputationCube.GetVariableAccessVariableProfile(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the ChangeMethodProfile for the primary change rule for this ComputationCell.
		/// </summary>
		/// <returns>
		/// The ChangeMethodProfile for the primary change rule for this ComputationCell.
		/// </returns>

		public ChangeMethodProfile GetPrimaryChangeMethodProfile()
		{
			try
			{
				return ComputationCube.GetPrimaryChangeMethodProfile(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the ChangeMethodProfile for the secondary change rule for this ComputationCell.
		/// </summary>
		/// <returns>
		/// The ChangeMethodProfile for the secondary change rule for this ComputationCell.
		/// </returns>

		public ChangeMethodProfile GetSecondaryChangeMethodProfile()
		{
			try
			{
				return ComputationCube.GetSecondaryChangeMethodProfile(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the ChangeMethodProfile for the autototal change rule for this ComputationCell.
		/// </summary>
		/// <returns>
		/// The ChangeMethodProfile for the autototal change rule for this ComputationCell.
		/// </returns>

		public ChangeMethodProfile GetAutototalChangeMethodProfile()
		{
			try
			{
				return ComputationCube.GetAutototalChangeMethodProfile(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the FormulaSpreadProfile of the init for this ComputationCell.
		/// </summary>
		/// <returns>
		/// The FormulaSpreadProfile for the init of this ComputationCell.
		/// </returns>

		public FormulaProfile GetInitFormulaProfile()
		{
			try
			{
				return ComputationCube.GetInitFormulaProfile(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a string describing the given cell
		/// </summary>
		/// <returns>
		/// A string describing the ComputationCellReference.
		/// </returns>

		public string GetCellDescription()
		{
			try
			{
				return ComputationCube.GetCellDescription(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Initializes a lock value.
		/// </summary>

		public void SumDetailCellLocks()
		{
			ArrayList cellRefList;
			bool locked;

			try
			{
				cellRefList = GetComponentDetailCellRefArray(false);

				if (cellRefList.Count > 0)
				{
					locked = true;

                    foreach (ComputationCellReference compCellRef in cellRefList)
                    {
                        // Begin TT#3809 - stodd - Locked Cell doesn't save when processing Need
                        // Adding this statement (somehow) forces the "if" below to resolve correctly. I don't know why.
                        bool isLocked = ComputationCellFlagValues.isDisplayOnly(compCellRef.CellFlags);
                        // End TT#3809 - stodd - Locked Cell doesn't save when processing Need
                        if (!compCellRef.isCellLocked)
                        {
                            locked = false;
                            break;
                        }
                    }

					SetCellLock(locked);
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

