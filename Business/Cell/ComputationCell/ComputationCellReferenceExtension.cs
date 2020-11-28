using System;
using System.Collections;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The ComputationCellReferenceExtension class is an abstract class that describes functionality that is an extension to the ComputationCellReference class.  This class
	/// allows for different implementations of the contained functionality through inheritance.
	/// </summary>

	abstract public class ComputationCellReferenceExtension
	{
		//=======
		// FIELDS
		//=======

		protected ComputationCellReference _computationCellRef;

		//=============
		// CONSTRUCTORS
		//=============

		public ComputationCellReferenceExtension(ComputationCellReference aComputationCellRef)
		{
			_computationCellRef = aComputationCellRef;
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

		abstract public bool isShadowCell { get; }

		//========
		// METHODS
		//========

		abstract internal void InitCellDisplayOnlyFlag();

		virtual internal void InitCellIneligibleFlag()
		{
			throw new Exception("Invalid Call");
		}

		virtual internal void InitCellClosedFlag()
		{
			throw new Exception("Invalid Call");
		}

		virtual internal void InitCellProtectedFlag()
		{
			throw new Exception("Invalid Call");
		}

		virtual internal void InitCellBlockedFlag()
		{
			throw new Exception("Invalid Call");
		}

		virtual internal void InitCellFixedFlag()
		{
			throw new Exception("Invalid Call");
		}

		abstract internal ExtensionCell GetExtensionCell(bool aAllocate);

		/// <summary>
		/// Creates a copy of the ComputationCellReference.  The ComputationCube is a shallow copy, while the ComputationCell is a deep copy.
		/// </summary>
		/// <returns>
		/// A new instance of ComputationCellReference with a copy of this objects information.
		/// </returns>

		abstract public ComputationCellReferenceExtension Copy();

		/// <summary>
		/// Method that copies values from a ComputationCellReferenceExtension object to the current object.
		/// </summary>
		/// <param name="aComputationCellRefExt">
		/// The ComputationCellReferenceExtension object to copy from.
		/// </param>

		abstract public void CopyFrom(ComputationCellReferenceExtension aComputationCellRefExt);

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
		/// Sets the value of the ComputationCell's lock flag.
		/// </summary>
        /// <param name="aBlock">
		/// A boolean containing the new block flag.
		/// </param>

		virtual public void SetCellBlock(bool aBlock)
		{
			throw new Exception("Invalid Call");
		}
	}
}
