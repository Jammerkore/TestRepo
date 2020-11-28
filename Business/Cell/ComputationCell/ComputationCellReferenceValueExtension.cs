using System;
using System.Collections;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// The ComputationCellReference class defines the interface to the ComputationCell/ComputationCube relationship for value-type variables.
	/// </summary>
	/// <remarks>
	/// The ComputationCellReference defines interface properties and methods that allow the owner to access fields and functionality in the ComputationCell
	/// and ComputationCube classes of value-type variables.
	/// </remarks>

	abstract public class ComputationCellReferenceValueExtension : ComputationCellReferenceExtension
	{
		//=======
		// FIELDS
		//=======

		protected ExtensionCell _extCell;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of the ComputationCellReference class using the given ComputationCube.
		/// </summary>

		public ComputationCellReferenceValueExtension(ComputationCellReference aComputationCellRef)
			: base(aComputationCellRef)
		{
			_extCell = null;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Method that determines if the Cell is a shadow Cell.  A shadow Cell is one that does not exist on the cube and is created everytime a
		/// CellReference is created.  This allows for a shadow cell to be used as a cube cell, but not consume permanent storage in the cube.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the Cell is a shadow Cell.
		/// </returns>

		override public bool isShadowCell
		{
			get
			{
				return false;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Initializes this Cell's display-only flag.
		/// </summary>

		override internal void InitCellDisplayOnlyFlag()
		{
			try
			{
				InitCellValue();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override internal ExtensionCell GetExtensionCell(bool aAllocate)
		{
			try
			{
				if (_extCell == null)
				{
					if (_computationCellRef.isCellExtensionCreated)
					{
						_extCell = (ExtensionCell)_computationCellRef.ComputationCube.GetExtensionCell(_computationCellRef);
					}
					else if (aAllocate)
					{
						_extCell = (ExtensionCell)_computationCellRef.ComputationCube.GetExtensionCell(_computationCellRef);
						_computationCellRef.isCellExtensionCreated = true;
					}
				}

				return _extCell;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that copies values from a ComputationCellReferenceExtension object to the current object.
		/// </summary>

		override public void CopyFrom(ComputationCellReferenceExtension aComputationCellRefExt)
		{
			try
			{
				_extCell = ((ComputationCellReferenceValueExtension)aComputationCellRefExt)._extCell;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the Value of a ComputationCell from user entry.
		/// </summary>
		/// <param name="aValue">
		/// The value that is to be assigned to the Cell.
		/// </param>

		override public void SetEntryCellValue(double aValue)
		{
			double newValue;

			try
			{
			    // Begin TT#1954-MD - JSmith - Assortment Performance
				// Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
				//_computationCellRef.ComputationCube.ComputationCubeGroup.BackupCellForPendingUndo(_computationCellRef.ComputationCell, (ComputationCell)_computationCellRef.CellCopy(), GetExtensionCell(false), _extCell == null ? null : (ExtensionCell)GetExtensionCell(false).Copy());
                _computationCellRef.ComputationCube.ComputationCubeGroup.BackupCellForPendingUndo(_computationCellRef.ComputationCell, (ComputationCell)_computationCellRef.CellCopy(), GetExtensionCell(false), _extCell == null ? null : (ExtensionCell)GetExtensionCell(false).Copy(), eSetCellMode.Entry);
				// End RO-4741 - JSmith - Need to scroll to variables prior to making change
				// End TT#1954-MD - JSmith - Assortment Performance

				InitCellValue();

				if (!_computationCellRef.ComputationCell.isCellHasNoValue)
				{
					newValue = (double)(decimal)System.Math.Round(aValue, _computationCellRef.GetFormatTypeVariableProfile().NumDecimals);

					if (_computationCellRef.ComputationCell.Value != newValue)
					{
						if (!_computationCellRef.ComputationCell.isCellAvailableForEntry)
						{
							throw new CellNotAvailableException(_computationCellRef.GetCellDescription());
						}

						GetExtensionCell(true);

						_extCell.GetCompInfo(_computationCellRef).PreviousValue = (ComputationCell)_computationCellRef.CellCopy();
						// Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
						//_computationCellRef.ComputationCube.ComputationCubeGroup.BackupCellForUndo(_computationCellRef.ComputationCell, _extCell.GetCompInfo(_computationCellRef).PreviousValue);
						_computationCellRef.ComputationCube.ComputationCubeGroup.BackupCellForUndo(_computationCellRef.ComputationCell, _extCell.GetCompInfo(_computationCellRef).PreviousValue, eSetCellMode.Entry);
						// End RO-4741 - JSmith - Need to scroll to variables prior to making change

						if (!_computationCellRef.isCellUserChanged)
						{
							_computationCellRef.ComputationCube.ComputationCubeGroup.AddCellToChangedList(_computationCellRef);
						}

						if (!_extCell.isPostInitValueSet)
						{
							_extCell.PostInitValue = _computationCellRef.ComputationCell.Value;
						}

						_computationCellRef.ComputationCell.Value = newValue;
						_computationCellRef.ComputationCell.isChanged = true;
						_computationCellRef.isCellCompChanged = true;
						_computationCellRef.isCellUserChanged = true;
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
		/// Sets the Value of a ComputationCell from a database read.
		/// </summary>
		/// <param name="aValue">
		/// The value that is to be assigned to the Cell.
		/// </param>

		override public void SetLoadCellValue(double aValue, bool aLock)
		{
			try
			{
				if (!_computationCellRef.ComputationCell.isCellHasNoValue)
				{
					_computationCellRef.ComputationCell.Value = (double)(decimal)System.Math.Round(aValue, _computationCellRef.GetFormatTypeVariableProfile().NumDecimals);
					_computationCellRef.ComputationCell.isLocked = aLock;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin TT#TT#739-MD - JSmith - delete stores
        /// <summary>
        /// Sets the Lock of a ComputationCell from a database read.
        /// </summary>
        /// <param name="aLock">
        /// The lock that is to be assigned to the Cell.
        /// </param>

        override public void SetLoadCellLock(bool aLock)
        {
            try
            {
                _computationCellRef.ComputationCell.isLocked = aLock;
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
		/// Sets the Value of a ComputationCell from a computation.
		/// </summary>
		/// <param name="aSetCellMode">
		/// The eSetCellMode indicating the type of change that is occurring, include Initialize or Entry.
		/// </param>
		/// <param name="aValue">
		/// The value that is to be assigned to the Cell.
		/// </param>
		/// 
		override public void SetCompCellValue(eSetCellMode aSetCellMode, double aValue)
		{
			SetCompCellValue(aSetCellMode, aValue, false);
		}

		/// <summary>
		/// Sets the Value of a ComputationCell from a computation.
		/// </summary>
		/// <param name="aSetCellMode">
		/// The eSetCellMode indicating the type of change that is occurring, include Initialize or Entry.
		/// </param>
		/// <param name="aValue">
		/// The value that is to be assigned to the Cell.
        /// </param>
		/// <param name="isAsrtSimStore">
		/// Indicates whether this was called for a assortment similar store cell.
		/// </param>

		override public void SetCompCellValue(eSetCellMode aSetCellMode, double aValue, bool isAsrtSimStore)
		// END TT#2200 - stodd - Open existing Assortment-> select REDO-> process-> MID Client stops working
		{
			double newValue;
			ComputationCell copyCell;

			try
			{
			    // Begin TT#1954-MD - JSmith - Assortment Performance
				// Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
				//_computationCellRef.ComputationCube.ComputationCubeGroup.BackupCellForPendingUndo(_computationCellRef.ComputationCell, (ComputationCell)_computationCellRef.CellCopy(), GetExtensionCell(false), _extCell == null ? null : (ExtensionCell)GetExtensionCell(false).Copy());
                _computationCellRef.ComputationCube.ComputationCubeGroup.BackupCellForPendingUndo(_computationCellRef.ComputationCell, (ComputationCell)_computationCellRef.CellCopy(), GetExtensionCell(false), _extCell == null ? null : (ExtensionCell)GetExtensionCell(false).Copy(), aSetCellMode);
				// End RO-4741 - JSmith - Need to scroll to variables prior to making change
				// End TT#1954-MD - JSmith - Assortment Performance

				InitCellValue();
				if (!isAsrtSimStore)
				{
					//Begin TT#2024 - DOConnell - Asst Tab changed the asst after creating the placeholders from 100% of rcpts to 45%
					_computationCellRef.ComputationCube.InitCellValue(_computationCellRef);
					//End TT#2024 - DOConnell - Asst Tab changed the asst after creating the placeholders from 100% of rcpts to 45%
				}
				if (!_computationCellRef.ComputationCell.isCellHasNoValue)
				{
					switch (aSetCellMode)
					{
						case eSetCellMode.Initialize:

							newValue = (double)(decimal)System.Math.Round(aValue, _computationCellRef.GetFormatTypeVariableProfile().NumDecimals);

							if (_computationCellRef.ComputationCell.Value != newValue)
							{
								GetExtensionCell(true);

								if (!_extCell.isPreInitValueSet)
								{
									_extCell.PreInitValue = _computationCellRef.ComputationCell.Value;
								}

								_computationCellRef.ComputationCell.Value = newValue;
								_computationCellRef.ComputationCell.isChanged = true;
							}

							break;

						case eSetCellMode.InitializeCurrent:

							newValue = (double)(decimal)System.Math.Round(aValue, _computationCellRef.GetFormatTypeVariableProfile().NumDecimals);

							if (_computationCellRef.ComputationCell.Value != newValue)
							{
								if (!_computationCellRef.ComputationCell.isCellAvailableForCurrentInitialization)
								{
									throw new CellNotAvailableException(_computationCellRef.GetCellDescription());
								}

								GetExtensionCell(true);

								copyCell = (ComputationCell)_computationCellRef.CellCopy();
								copyCell.isCurrentInitialized = false;
								//Begin Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
								//_extCell.GetCompInfo(_computationCellRef).PreviousValue = copyCell;
								//End Track #5829 - JScott - Month did not change for COGS/GM/RM BOP Inv/CF BOP Inv
								// Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
								//_computationCellRef.ComputationCube.ComputationCubeGroup.BackupCellForUndo(_computationCellRef.ComputationCell, copyCell);
								_computationCellRef.ComputationCube.ComputationCubeGroup.BackupCellForUndo(_computationCellRef.ComputationCell, copyCell, aSetCellMode);
								// End RO-4741 - JSmith - Need to scroll to variables prior to making change

								if (!_extCell.isPostInitValueSet)
								{
									_extCell.PostInitValue = _computationCellRef.ComputationCell.Value;
								}

								_computationCellRef.ComputationCell.Value = newValue;
								_computationCellRef.ComputationCell.isChanged = true;
							}

							break;

						case eSetCellMode.Computation:
						case eSetCellMode.AutoTotal:

							newValue = (double)(decimal)System.Math.Round(aValue, _computationCellRef.GetFormatTypeVariableProfile().NumDecimals);

							if (_computationCellRef.ComputationCell.Value != newValue)
							{
								if (!_computationCellRef.ComputationCell.isCellAvailableForComputation)
								{
									throw new CellNotAvailableException(_computationCellRef.GetCellDescription());
								}

								GetExtensionCell(true);

								_extCell.GetCompInfo(_computationCellRef).PreviousValue = (ComputationCell)_computationCellRef.CellCopy();
								// Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
								//								_computationCellRef.ComputationCube.ComputationCubeGroup.BackupCellForUndo(_computationCellRef.ComputationCell, _extCell.GetCompInfo(_computationCellRef).PreviousValue);
								_computationCellRef.ComputationCube.ComputationCubeGroup.BackupCellForUndo(_computationCellRef.ComputationCell, _extCell.GetCompInfo(_computationCellRef).PreviousValue, aSetCellMode);
								// End RO-4741 - JSmith - Need to scroll to variables prior to making change

								if (!_extCell.isPostInitValueSet)
								{
									_extCell.PostInitValue = _computationCellRef.ComputationCell.Value;
								}

								_computationCellRef.ComputationCell.Value = newValue;
								_computationCellRef.ComputationCell.isChanged = true;
							}
                            // BEGIN MID Track #5658 - changing Period total gets TempCellLocked error
							//_computationCellRef.isCellCompChanged = true;
                            if (aSetCellMode == eSetCellMode.Computation)
                            {
                                _computationCellRef.isCellCompChanged = true;
                            }
                            // END MID Track #5658
							break;

                        //Begin TT#2 - JScott - Assortment Planning - Phase 2
                        //case eSetCellMode.ForcedReInit:

                        //    newValue = (double)(decimal)System.Math.Round(aValue, _computationCellRef.GetFormatTypeVariableProfile().NumDecimals);

                        //    if (_computationCellRef.ComputationCell.Value != newValue)
                        //    {
                        //        if (!_computationCellRef.ComputationCell.isCellAvailableForForcedReInit)
                        //        {
                        //            throw new CellNotAvailableException(_computationCellRef.GetCellDescription());
                        //        }

                        //        GetExtensionCell(true);

                        //        _extCell.GetCompInfo(_computationCellRef).PreviousValue = (ComputationCell)_computationCellRef.CellCopy();
                        //        _computationCellRef.ComputationCube.ComputationCubeGroup.BackupCellForUndo(_computationCellRef.ComputationCell, _extCell.GetCompInfo(_computationCellRef).PreviousValue);

                        //        if (!_extCell.isPostInitValueSet)
                        //        {
                        //            _extCell.PostInitValue = _computationCellRef.ComputationCell.Value;
                        //        }

                        //        _computationCellRef.ComputationCell.Value = newValue;
                        //        _computationCellRef.ComputationCell.isChanged = true;
                        //    }

                        //    _computationCellRef.isCellCompChanged = true;
                        //    break;

                        //End TT#2 - JScott - Assortment Planning - Phase 2
                        default:
							throw new MIDException(eErrorLevel.severe,
								(int)eMIDTextCode.msg_pl_InvalidSetCellMode,
								MIDText.GetText(eMIDTextCode.msg_pl_InvalidSetCellMode));
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
		/// Sets the Value of a ComputationCell for a copy.
		/// </summary>
		/// <param name="aValue">
		/// The value that is to be assigned to the Cell.
		/// </param>

		override public void SetCopyCellValue(double aValue, bool aLock)
		{
			try
			{
			    // Begin TT#1954-MD - JSmith - Assortment Performance
				// Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
				//_computationCellRef.ComputationCube.ComputationCubeGroup.BackupCellForPendingUndo(_computationCellRef.ComputationCell, (ComputationCell)_computationCellRef.CellCopy(), GetExtensionCell(false), _extCell == null ? null : (ExtensionCell)GetExtensionCell(false).Copy());
                _computationCellRef.ComputationCube.ComputationCubeGroup.BackupCellForPendingUndo(_computationCellRef.ComputationCell, (ComputationCell)_computationCellRef.CellCopy(), GetExtensionCell(false), _extCell == null ? null : (ExtensionCell)GetExtensionCell(false).Copy(), eSetCellMode.Entry);
				// End RO-4741 - JSmith - Need to scroll to variables prior to making change
				// End TT#1954-MD - JSmith - Assortment Performance

				if (_computationCellRef.ComputationCell.Value != aValue || _computationCellRef.ComputationCell.isLocked != aLock)
				{
					if (!_computationCellRef.ComputationCell.isCellAvailableForCopy)
					{
						throw new CellNotAvailableException(_computationCellRef.GetCellDescription());
					}

					// Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
					//_computationCellRef.ComputationCube.ComputationCubeGroup.BackupCellForUndo(_computationCellRef.ComputationCell, (ComputationCell)_computationCellRef.CellCopy());
					_computationCellRef.ComputationCube.ComputationCubeGroup.BackupCellForUndo(_computationCellRef.ComputationCell, (ComputationCell)_computationCellRef.CellCopy(), eSetCellMode.Entry);
					// End RO-4741 - JSmith - Need to scroll to variables prior to making change

					if (_computationCellRef.ComputationCell.Value != aValue)
					{
						GetExtensionCell(true);

						if (!_extCell.isPostInitValueSet)
						{
							_extCell.PostInitValue = _computationCellRef.ComputationCell.Value;
						}

						_computationCellRef.ComputationCell.Value = aValue;
					}

					if (_computationCellRef.ComputationCell.isLocked != aLock)
					{
						_computationCellRef.ComputationCell.isLocked = aLock;
					}

					_computationCellRef.ComputationCell.isChanged = true;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the value of the ComputationCell's lock flag.
		/// </summary>
		/// <param name="aValue">
		/// A boolean containing the new lock flag.
		/// </param>

		override public void SetCellLock(bool aValue)
		{
			try
			{
			    // Begin TT#1954-MD - JSmith - Assortment Performance
				// Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
				//_computationCellRef.ComputationCube.ComputationCubeGroup.BackupCellForPendingUndo(_computationCellRef.ComputationCell, (ComputationCell)_computationCellRef.CellCopy(), null, null);
                _computationCellRef.ComputationCube.ComputationCubeGroup.BackupCellForPendingUndo(_computationCellRef.ComputationCell, (ComputationCell)_computationCellRef.CellCopy(), null, null, eSetCellMode.Entry);
				// End RO-4741 - JSmith - Need to scroll to variables prior to making change
				// End TT#1954-MD - JSmith - Assortment Performance

				InitCellValue();

				if (_computationCellRef.ComputationCell.isCellAvailableForLocking)
				{
					if (aValue != _computationCellRef.ComputationCell.isLocked)
					{
						// Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
						//_computationCellRef.ComputationCube.ComputationCubeGroup.BackupCellForUndo(_computationCellRef.ComputationCell, (ComputationCell)_computationCellRef.CellCopy());
						_computationCellRef.ComputationCube.ComputationCubeGroup.BackupCellForUndo(_computationCellRef.ComputationCell, (ComputationCell)_computationCellRef.CellCopy(), eSetCellMode.Entry);
						// End RO-4741 - JSmith - Need to scroll to variables prior to making change
						_computationCellRef.ComputationCell.isLocked = aValue;
						_computationCellRef.ComputationCell.isChanged = true;
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
