using System;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// This class defines the Extension Cube, which contains backup Cells for undo functionality.
	/// </summary>
	/// <remarks>
	/// The Extension Cube contains a stack for each Cell that stores backup copies of the Cell at each undo sequence.  The Extension Cube presents functionality
	/// for backing up and undo the changes to the Cells.
	/// </remarks>

	public class ComputationExtensionCube : Cube
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ComputationExtensionCube.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this ComputationExtensionCube is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this ComputationExtensionCube is a part of.
		/// </param>
        /// <param name="aCompCubeGroup">
		/// A reference to a ComputationCubeGroup that this ComputationExtensionCube is a part of.
		/// </param>
		/// <param name="aCubeDefinition">
		/// The CubeDefinition that describes the structure of this ComputationExtensionCube.
		/// </param>

		public ComputationExtensionCube(SessionAddressBlock aSAB, ApplicationSessionTransaction aTransaction, ComputationCubeGroup aCompCubeGroup, CubeDefinition aCubeDefinition)
			: base(aSAB, aTransaction, aCompCubeGroup, aCubeDefinition)
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
				return eCubeType.None;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Abstract method that allows the inheritor the ability to dictate the way a CellCoordinates are created.
		/// </summary>
		/// <returns>
		/// A reference to a new CellCoordinates object.
		/// </returns>

		override public CellCoordinates CreateCellCoordinates(int aNumIndices)
		{
			try
			{
				return new ComputationCellCoordinates(aNumIndices);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a new instance of a ExtensionCell.
		/// </summary>
		/// <returns>
		/// A reference to a new ExtensionCell.
		/// </returns>

		override public Cell CreateCell(CellReference aCellReference)
		{
			try
			{
				return new ExtensionCell();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a new instance of a ExtensionCellReference.
		/// </summary>
		/// <returns>
		/// A reference to a new ExtensionCellReference.
		/// </returns>

		override public CellReference CreateCellReference()
		{
			try
			{
				return null;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a new instance of a ExtensionCellReference using the given CellCoordinates.
		/// </summary>
		/// <param name="aCoordinates">
		/// The CellCoordinates object that describes the ExtensionCell's position in this ComputationExtensionCube.
		/// </param>
		/// <returns>
		/// A reference to a new ExtensionCellReference.
		/// </returns>

		override public CellReference CreateCellReference(CellCoordinates aCoordinates)
		{
			try
			{
				return null;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a new instance of a ExtensionCellReference using the given CellReference.
		/// </summary>
		/// <param name="aCellRef">
		/// The CellReference object that describes the ExtensionCell's position in this ComputationExtensionCube.  This indices in this object will be translated to 
		/// cooresponding coordinates in this ComputationExtensionCube.
		/// </param>
		/// <returns>
		/// A reference to a new ExtensionCellReference.
		/// </returns>

		override public CellReference CreateCellReference(CellReference aCellRef)
		{
			try
			{
				return null;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}