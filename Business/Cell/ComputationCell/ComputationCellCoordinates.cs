using System;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// ComputationCellCoordinates defines the coordinates to a Cell within a Cube.
	/// </summary>
	/// <remarks>
	/// The ComputationCellCoordinates class is used to define the n-number of coordinates to locate a Cell within a Cube.
	/// </remarks>

	public class ComputationCellCoordinates : CellCoordinates
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates an instance of ComputationCellCoordinates, with a specified number of indices. 
		/// </summary>
		/// <param name="aNumCoordinates">
		/// The number of indices in the ComputationCellCoordinates object.
		/// </param>

		public ComputationCellCoordinates(int aNumCoordinates)
			: base(aNumCoordinates)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}
}
