using System;
using System.Collections;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// CellReference is a class that contains references to a Cube, a CellCoordinates and a Cell object and provides functionality for the
	/// relationship between the objects.
	/// </summary>
	/// <remarks>
	/// The CellReference class is the interface into the relationship between a Cube and the Cells that are contained in it.  All actions 
	/// performed on a Cell are done through the methods and properties in this class.
	/// </remarks>

	abstract public class CellReference
	{
		//=======
		// FIELDS
		//=======

		private Cube _cube;
		private CellCoordinates _coordinates;
		protected Cell _cell;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of CellReference that exists in cube aCube. 
		/// </summary>
		/// <param name="aCube">
		/// The cube where the cell exists.
		/// </param>

		public CellReference(Cube aCube)
		{
			try
			{
				_cube = aCube;
				_coordinates = _cube.CreateCellCoordinates(aCube.CubeDefinition.NumDimensions);
				_cell = null;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a new instance of CellReference that points to the cell with coordinates aCellCoordinates in cube aCube. 
		/// </summary>
		/// <param name="aCube">
		/// The cube where the cell exists.
		/// </param>
		/// <param name="aCoordinates">
		/// The coordinates that identify the cell's position in the cube.
		/// </param>

		public CellReference(Cube aCube, CellCoordinates aCoordinates)
		{
			try
			{
				_cube = aCube;
				_coordinates = _cube.CreateCellCoordinates(aCoordinates.NumIndices);
				_coordinates.CopyFrom(aCoordinates);
				_cell = null;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a new instance of CellReference and converts the coordinates in aCellReference to matching ones in cube aCube.
		/// </summary>
		/// <param name="aCube">
		/// The cube where the cell exists.
		/// </param>
		/// <param name="aCellReference">
		/// The CellReference object whose coordinates are to be converted.  Profile types of the dimensions from aCube will be
		/// matched to this cube where possible.  Non-matching indices will be set to -1.
		/// </param>

		public CellReference(Cube aCube, CellReference aCellReference)
		{
			try
			{
				_cube = aCube;
				_coordinates = _cube.CreateCellCoordinates(aCube.CubeDefinition.NumDimensions);
				ConvertFrom(aCellReference);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Indexer.  Gets or sets the indice located by the profile type of aProfileType.
		/// </summary>

		public int this[eProfileType aProfileType]
		{
			get
			{
				try
				{
					return _cube.GetCoordinate(this, aProfileType, _cube.GetDimensionProfileTypeIndex(aProfileType));
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
					_cube.SetCoordinate(this, aProfileType, _cube.GetDimensionProfileTypeIndex(aProfileType), value);
					Reset();
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Indexer.  Gets or sets the indice at the specified index.
		/// </summary>

		internal int this[int aIndex]
		{
			get
			{
				try
				{
					return _cube.GetCoordinate(this, _cube.CubeDefinition[aIndex].ProfileType, aIndex);
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
					_cube.SetCoordinate(this, _cube.CubeDefinition[aIndex].ProfileType, aIndex, value);
					Reset();
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the Cube reference.  Protected.
		/// </summary>

		public Cube Cube
		{
			get
			{
				return _cube;
			}
		}

		/// <summary>
		/// Gets the CellCoordinates reference.  Protected.
		/// </summary>

		public CellCoordinates CellCoordinates
		{
			get
			{
				return _coordinates;
			}
		}

		/// <summary>
		/// Gets the Cell reference, pulling the reference from the Cube on the initial call.  Protected.
		/// </summary>

		virtual public Cell Cell
		{
			get
			{
				try
				{
					if (_cell == null)
					{
						_cell = _cube.GetCell(this);
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

		// BEGIN TT#831-MD - Stodd - Need / Intransit not displayed
		/// <summary>
		/// Displays Coordinates and keys values in a readable fashion.
		/// </summary>
		public string CellKeys
		{
			get
			{
				string cellKeys = string.Empty;
				#if (DEBUG)
				for (int i = 0; i < CellCoordinates.NumIndices; i++)
				{
					int key = CellCoordinates.GetRawCoordinate(i);
					DimensionDefinition prof = Cube.CubeDefinition[i];

					cellKeys += prof.ProfileType + " " + key + " ";
				}
				#endif

				return cellKeys;
			}
		}
		// END TT#831-MD - Stodd - Need / Intransit not displayed

		//========
		// METHODS
		//========

		/// <summary>
		/// Abstract method forcing definition of Copy functionality.  Copy creates a deep copy.
		/// </summary>

		abstract public CellReference Copy();

		override public bool Equals(object obj)
		{
			CellReference cellRef;

			try
			{
				if (this.GetType() == obj.GetType() || obj.GetType().IsSubclassOf(this.GetType()))
				{
					cellRef = (CellReference)obj;

					if (_cube.GetType() == cellRef._cube.GetType() && _coordinates.Equals(cellRef._coordinates))
					{
						return true;
					}
					else
					{
						return false;
					}
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

		public override int GetHashCode()
		{
			try
			{
				return _coordinates.GetHashCode();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Override.  Method that copies values from a CellReference object to the current object.
		/// </summary>
		/// <param name="aCellReference">
		/// The CellReference object to copy from.
		/// </param>

		virtual public void CopyFrom(CellReference aCellReference)
		{
			try
			{
				_cube = aCellReference.Cube;
				_coordinates = _cube.CreateCellCoordinates(aCellReference.Cube.CubeDefinition.NumDimensions);
				ConvertFrom(aCellReference);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// This method is called when the CellReference is to be reset.
		/// </summary>

		virtual public void Reset()
		{
			_cell = null;
		}

		/// <summary>
		/// Pass-through call to the Clone method of the Cell.
		/// </summary>
		/// <returns>
		/// A reference to the cloned Cell.
		/// </returns>

		public Cell CellClone()
		{
			try
			{
				return Cell.Clone();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Pass-through call to the Copy method of the Cell.
		/// </summary>
		/// <returns>
		/// A reference to the copied Cell.
		/// </returns>

		public Cell CellCopy()
		{
			try
			{
				return Cell.Copy();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Pass-through call to the CopyFrom method of the Cell.
		/// </summary>
		/// <param name="aCell">
		/// The reference to the Cell to copy.
		/// </param>

		public void CellCopyFrom(Cell aCell)
		{
			try
			{
				Cell.CopyFrom(aCell);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a bool indicating if the Cell exists in the Cube.
		/// </summary>

		public bool doesCellExist
		{
			get
			{
				try
				{
					return _cube.doesCellExist(this);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Method that returns the Dimension index for a given ProfileType.
		/// </summary>
		/// <param name="aProfileType">
		/// The ProfileType of the dimension.
		/// </param>
		/// <returns>
		/// The index of the Dimension on the Cube.
		/// </returns>

		public int GetDimensionProfileTypeIndex(eProfileType aProfileType)
		{
			try
			{
				return _cube.GetDimensionProfileTypeIndex(aProfileType);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method to convert the coordinates of a CellReference to the current one.
		/// </summary>
		/// <param name="aCellReference">
		/// The CellReference object to convert from.
		/// </param>
		/// <remarks>
		/// The Convert method attempts to match profile types in the dimensions from one CellReference to another.  Initially, Convert will
		/// check to see if the CubeDefinitions of each Cube are equal.  If they are, the coordinates are simply copied.  If not, Convert will
		/// attempt to match profile types of the Cube's dimensions.  If a match is found, the indice is moved from the original CellReference
		/// to the new position in this object.  If a matching profile type is not found, or is not specified on the original CellReference,
		/// the indice is set to -1.
		/// </remarks>

		public void ConvertFrom(CellReference aCellReference)
		{
			int i;
			eProfileType profileType;
			int dimensionIdx;

			try
			{
				if (_cube.CubeDefinition != aCellReference._cube.CubeDefinition)
				{
					for (i = 0; i < _coordinates.NumIndices; i++)
					{
						profileType = _cube.CubeDefinition[i].ProfileType;
						dimensionIdx = aCellReference._cube.GetDimensionProfileTypeIndex(profileType);

						if (dimensionIdx != -1)
						{
							this[i] = aCellReference[dimensionIdx];
						}
						else
						{
							_coordinates.SetCoordinate(profileType, i, -1);
						}
					}
				}
				else
				{
					_coordinates = _cube.CreateCellCoordinates(aCellReference._coordinates.NumIndices);
					_coordinates.CopyFrom(aCellReference._coordinates);
				}

				Reset();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an ArrayList of PlanCellReferences.  This ArrayList contains all of the PlanCells that are totals of this PlanCubeCell.
		/// </summary>
		/// <returns>
		/// The ArrayList of PlanCellReferences.
		/// </returns>

		public ArrayList GetTotalCellRefArray()
		{
			try
			{
				return _cube.GetTotalCellRefArray(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an ArrayList of PlanCellReferences.  This ArrayList contains all of the PlanCells that are totals of this PlanCubeCell for the given
		/// eCubeType.
		/// </summary>
		/// <param name="aTotalCubeType">
		/// The eCubeType to find totals for.
		/// </param>
		/// <returns>
		/// The ArrayList of PlanCellReferences.
		/// </returns>

		public ArrayList GetTotalCellRefArray(eCubeType aTotalCubeType)
		{
			try
			{
				return _cube.GetTotalCellRefArray(this, aTotalCubeType);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an ArrayList of CellReferences.  This ArrayList represents all of the cells on a dimension, as requested by the given ProfileList.
		/// </summary>
		/// <param name="aProfileList">
		/// The ProfileList of the Dimension that is to be retrieved.
		/// </param>
		/// <returns>
		/// The ArrayList of CellReferences.
		/// </returns>

		public ArrayList GetCellRefArray(ProfileList aProfileList)
		{
			try
			{
				return _cube.GetCellRefArray(this, aProfileList);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
