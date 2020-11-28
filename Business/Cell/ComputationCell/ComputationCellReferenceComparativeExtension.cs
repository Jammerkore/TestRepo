using System;
using System.Collections;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// The ComputationCellReference class defines the interface to the ComputationCell/ComputationCube relationship for comparative-type variables.
	/// </summary>
	/// <remarks>
	/// The ComputationCellReference defines interface properties and methods that allow the owner to access fields and functionality in the ComputationCell
	/// and ComputationCube classes of comparative-type variables.
	/// </remarks>

	abstract public class ComputationCellReferenceComparativeExtension : ComputationCellReferenceExtension
	{
		//=======
		// FIELDS
		//=======

		protected ExtensionCell _extCell;
		protected byte _initFlags;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of the ComputationCellReference class using the given ComputationCube.
		/// </summary>

		public ComputationCellReferenceComparativeExtension(ComputationCellReference aComputationCellRef)
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
				return true;
			}
		}

		/// <summary>
		/// Gets or sets the value of the DisplayOnlyInited flag.
		/// </summary>

		private bool isDisplayOnlyInited
		{
			get
			{
				try
				{
					return ((_initFlags & ComparativeExtensionFlagValues.DisplayOnlyInited) > 0);
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
					if (value)
					{
						_initFlags = (byte)(_initFlags | ComparativeExtensionFlagValues.DisplayOnlyInited);
					}
					else
					{
						_initFlags = (byte)(_initFlags & ~ComparativeExtensionFlagValues.DisplayOnlyInited);
					}
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
		/// Initializes this Cell's display-only flag.
		/// </summary>

		override internal void InitCellDisplayOnlyFlag()
		{
			ComputationVariableProfile varProf;

			try
			{
				if (!isDisplayOnlyInited)
				{
					isDisplayOnlyInited = true;

					varProf = _computationCellRef.GetVariableAccessVariableProfile();
					if (varProf != null)
					{
						if (varProf.VariableAccess == eVariableAccess.DisplayOnly)
						{
							_computationCellRef.ComputationCell.isDisplayOnly = true;
						}
					}
				}
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
				if (_extCell == null && aAllocate)
				{
					_extCell = new ExtensionCell();
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
				_extCell = ((ComputationCellReferenceComparativeExtension)aComputationCellRefExt)._extCell;
				_initFlags = ((ComputationCellReferenceComparativeExtension)aComputationCellRefExt)._initFlags;
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
			ChangeMethodProfile changeMethodProfile;

			try
			{
				try
				{
					if (!_computationCellRef.ComputationCell.isCellHasNoValue)
					{
						if (!_computationCellRef.ComputationCell.isCellAvailableForEntry)
						{
							throw new CellNotAvailableException(_computationCellRef.GetCellDescription());
						}

						InitCellValue();
						_computationCellRef.ComputationCell.Value = (double)(decimal)System.Math.Round(aValue, _computationCellRef.GetFormatTypeVariableProfile().NumDecimals);

						changeMethodProfile = _computationCellRef.GetPrimaryChangeMethodProfile();

						if (changeMethodProfile != null)
						{
							changeMethodProfile.ExecuteChangeMethod(null, _computationCellRef, "ComputationCellReferenceComparativeExtension::SetEntryCellValue");
						}
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
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
			throw new MIDException(eErrorLevel.severe,
				(int)eMIDTextCode.msg_pl_InvalidCall,
				MIDText.GetText(eMIDTextCode.msg_pl_InvalidCall));
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
            throw new MIDException(eErrorLevel.severe,
                (int)eMIDTextCode.msg_pl_InvalidCall,
                MIDText.GetText(eMIDTextCode.msg_pl_InvalidCall));
        }
        // End TT#TT#739-MD - JSmith - delete stores

		// BEGIN TT#2200 - stodd - Open existing Assortment-> select REDO-> process-> MID Client stops working
		override public void SetCompCellValue(eSetCellMode aSetCellMode, double aValue, bool isAsrtSimStore)
		{
			SetCompCellValue(aSetCellMode, aValue);
		}
		// END TT#2200 - stodd - Open existing Assortment-> select REDO-> process-> MID Client stops working


		/// <summary>
		/// Sets the Value of a ComputationCell from a computation.
		/// </summary>
		/// <param name="aSetCellMode">
		/// The eSetCellMode indicating the type of change that is occurring, include Initialize or Entry.
		/// </param>
		/// <param name="aValue">
		/// The value that is to be assigned to the Cell.
		/// </param>

		override public void SetCompCellValue(eSetCellMode aSetCellMode, double aValue)
		{
			double newValue;

			try
			{
				InitCellValue();

				if (!_computationCellRef.ComputationCell.isCellHasNoValue)
				{
					switch (aSetCellMode)
					{
						case eSetCellMode.Initialize:

							GetExtensionCell(true);
							_extCell.PostInitValue = (double)(decimal)System.Math.Round(aValue, _computationCellRef.GetFormatTypeVariableProfile().NumDecimals);
							_computationCellRef.ComputationCell.Value = _extCell.PostInitValue;
							break;

						case eSetCellMode.InitializeCurrent:
						case eSetCellMode.Computation:
						case eSetCellMode.AutoTotal:
                        //Begin TT#2 - JScott - Assortment Planning - Phase 2
                        //case eSetCellMode.ForcedReInit:
                        //End TT#2 - JScott - Assortment Planning - Phase 2

							newValue = (double)(decimal)System.Math.Round(aValue, _computationCellRef.GetFormatTypeVariableProfile().NumDecimals);
							_computationCellRef.ComputationCell.Value = newValue;

							break;

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
			throw new MIDException(eErrorLevel.severe,
				(int)eMIDTextCode.msg_pl_InvalidCall,
				MIDText.GetText(eMIDTextCode.msg_pl_InvalidCall));
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
				InitCellValue();

				if (_computationCellRef.ComputationCell.isCellAvailableForLocking)
				{
					if (aValue != _computationCellRef.ComputationCell.isLocked)
					{
						_computationCellRef.ComputationCell.isLocked = aValue;
                        _computationCellRef.ComputationCell.isChanged = true;	// TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
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
