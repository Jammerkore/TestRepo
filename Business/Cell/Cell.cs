using System;

namespace MIDRetail.Business
{
	/// <summary>
	/// Cell is an abstract class that defines the structure of a cell, or a single piece of information stored in the cube.
	/// </summary>

	[Serializable]
	abstract public class Cell
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of the Cell class
		/// </summary>

		public Cell()
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Abstract method forcing definition of Clone functionality.  Clone creates a shallow copy.
		/// </summary>
		/// <returns>
		/// Object reference to cloned object.
		/// </returns>

		abstract public Cell Clone();

		/// <summary>
		/// Abstract method forcing definition of Copy functionality.  Copy creates a deep copy.
		/// </summary>

		abstract public Cell Copy();

		/// <summary>
		/// Abstract method forcing definition of CopyFrom functionality.  SetFrom copies the contents of another cell to itself.
		/// </summary>
		/// <param name="aCell">
		/// The cell to copy.
		/// </param>

		abstract public void CopyFrom(Cell aCell);

		/// <summary>
		/// Virtual method to clear the contents of the Cell.
		/// </summary>

		abstract public void Clear();
	}
}
