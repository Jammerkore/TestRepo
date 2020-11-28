using System;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// CellCoordinates defines the coordinates to a Cell within a Cube.
	/// </summary>
	/// <remarks>
	/// The CellCoordinates class is used to define the n-number of coordinates to locate a Cell within a Cube.
	/// </remarks>

	abstract public class CellCoordinates
	{
		//=======
		// FIELDS
		//=======

		private int[] _cellCoordinates;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates an instance of CellCoordinates, with a specified number of indices. 
		/// </summary>
		/// <param name="aNumCoordinates">
		/// The number of indices in the CellCoordinates object.
		/// </param>

		public CellCoordinates(int aNumCoordinates)
		{
			int i;

			try
			{
				_cellCoordinates = new int[aNumCoordinates];

				for (i = 0; i < aNumCoordinates; i++)
				{
					this.SetRawCoordinate(i, -1);
				}
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
		/// Gets the number of indices that exist in this CellCoordinates object.
		/// </summary>

		public int NumIndices
		{
			get
			{
				try
				{
					return _cellCoordinates.Length;
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
		/// Gets the indices of the CellCoordinates.
		/// </summary>

		virtual public int GetCoordinate(eProfileType aProfType, int aIndex)
		{
			try
			{
				if (aIndex != -1)
				{
					return GetRawCoordinate(aIndex);
				}
				else
				{
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_DimensionNotDefinedOnCube,
						MIDText.GetText(eMIDTextCode.msg_pl_DimensionNotDefinedOnCube));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the indices of the CellCoordinates.
		/// </summary>

		virtual public void SetCoordinate(eProfileType aProfType, int aIndex, int aValue)
		{
			try
			{
				if (aIndex != -1)
				{
					SetRawCoordinate(aIndex, aValue);
				}
				else
				{
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_DimensionNotDefinedOnCube,
						MIDText.GetText(eMIDTextCode.msg_pl_DimensionNotDefinedOnCube));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the raw indices of the CellCoordinates.
		/// </summary>

		public int GetRawCoordinate(int aIndex)
		{
			try
			{
				return _cellCoordinates[aIndex];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the raw indices of the CellCoordinates.
		/// </summary>

		public void SetRawCoordinate(int aIndex, int aValue)
		{
			try
			{
				_cellCoordinates[aIndex] = aValue;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that checks to see if this object have valid values in each indice.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the indices are valid.
		/// </returns>

		public bool areCoordinatesValid()
		{
			int i;

			try
			{
				for (i = 0; i < _cellCoordinates.Length; i++)
				{
					if (this.GetRawCoordinate(i) == -1)
					{
						return false;
					}
				}

				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method defining the Clone functionality.  Clone creates a shallow copy.
		/// </summary>
		/// <returns>
		/// Object reference to the cloned object.
		/// </returns>

		public void CopyFrom(CellCoordinates aCellCoordinates)
		{
			int i;

			try
			{
				if (this.NumIndices == aCellCoordinates.NumIndices)
				{
					for (i = 0; i < aCellCoordinates.NumIndices; i++)
					{
						this.SetRawCoordinate(i, aCellCoordinates.GetRawCoordinate(i));
					}
				}
				else
				{
					throw new Exception("Attempting to copy a BaseCellCoordiate from a different size BaseCellCoordinate");
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Override.  Returns the hashcode for this object.
		/// </summary>
		/// <remarks>
		/// Calulation of this hashcode is accomplished by bit shifting the result after adding in each indice.  If the cube uses more than
		/// 4 indices, this hashing algorithm will be less than optimal.
		/// </remarks>
		/// <returns>
		/// The hashcode for this object.
		/// </returns>

		override public int GetHashCode()
		{
			int i;
			int hashCode;

			try
			{
				hashCode = 0;

				if (_cellCoordinates.Length <= 4)
				{
					for (i = 0; i < _cellCoordinates.Length; i++)
					{
						hashCode = hashCode << 8;
						hashCode += this.GetRawCoordinate(i) & 0x00FF;
					}
				}
				else
				{
					for (i = 0; i < Math.Min(_cellCoordinates.Length, 8); i++)
					{
						hashCode = hashCode << 4;
						hashCode += this.GetRawCoordinate(i) & 0x000F;
					}
				}

				return hashCode;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Override.  Determines whether two CellCoordinates instances are equivalent.
		/// </summary>
		/// <param name="aObject">
		/// The CellCoordinates object that is to be compared to the current object.
		/// </param>
		/// <returns>
		/// A boolean indicating if the two instances are equal.
		/// </returns>

		override public bool Equals(object aObject)
		{
			int i;

			try
			{
				if (_cellCoordinates.Length != ((CellCoordinates)aObject)._cellCoordinates.Length)
				{
					return false;
				}

				for (i = 0; i < _cellCoordinates.Length; i++)
				{
					if (this.GetRawCoordinate(i) != ((CellCoordinates)aObject).GetRawCoordinate(i))
					{
						return false;
					}
				}

				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
