using System;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// AssortmentHeaderCellCoordinates defines the coordinates to a Cell within a Cube.
	/// </summary>
	/// <remarks>
	/// The AssortmentHeaderCellCoordinates class is used to define the n-number of coordinates to locate a Cell within a Cube.
	/// </remarks>

	public class AssortmentHeaderColorDetailCellCoordinates : CellCoordinates
	{
		//=======
		// FIELDS
		//=======

		private AssortmentHeaderColorDetail _headerCube;
		private int _headerIndex;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates an instance of AssortmentHeaderCellCoordinates, with a specified number of indices. 
		/// </summary>
		/// <param name="aNumCoordinates">
		/// The number of indices in the AssortmentHeaderCellCoordinates object.
		/// </param>

		public AssortmentHeaderColorDetailCellCoordinates(int aNumCoordinates, AssortmentHeaderColorDetail aHeaderCube)
			: base(aNumCoordinates)
		{
			_headerCube = aHeaderCube;
			_headerIndex = aHeaderCube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader);
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Gets the indices of the CellCoordinates.
		/// </summary>

		override public int GetCoordinate(eProfileType aProfType, int aIndex)
		{
			int coordVal;

			try
			{
				coordVal = GetRawCoordinate(aIndex);

				if (coordVal == Include.NoRID)
				{
					return coordVal;
				}
				else
				{
					if (aIndex == _headerIndex)
					{
						return coordVal / 2;
					}
					else
					{
						return coordVal;
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
		/// Sets the indices of the CellCoordinates.
		/// </summary>

		override public void SetCoordinate(eProfileType aProfType, int aIndex, int aValue)
		{
			int coordVal;

			try
			{
				if (aValue == Include.NoRID)
				{
					SetRawCoordinate(aIndex, aValue);
				}
				else
				{
					if (aIndex == _headerIndex)
					{
						if (!_headerCube.LoadingAssortmentData && _headerCube.AssortmentCubeGroup.useHeaderAllocation(aValue))
						{
							coordVal = aValue * 2;
						}
						else
						{
							coordVal = (aValue * 2) + 1;
						}

						SetRawCoordinate(aIndex, coordVal);
					}
					else
					{
						SetRawCoordinate(aIndex, aValue);
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
