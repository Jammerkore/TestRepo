using System;
using System.Collections;

namespace MIDRetail.Business
{
	/// <summary>
	/// Class that defines a CellSelector, which is used by the recursive routine that traverses the Cube to decide if the Cell should be selected or not.
	/// </summary>

	abstract public class CellSelector
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		abstract public void CheckItem(CellReference aCellRef, out bool aCancel);
	}

	/// <summary>
	/// A CellSelector that selects all Cells.
	/// </summary>

	public class AllSelector : CellSelector
	{
		//=======
		// FIELDS
		//=======

		ArrayList _cellRefList;

		//=============
		// CONSTRUCTORS
		//=============

		public AllSelector()
		{
			_cellRefList = new ArrayList();
		}

		//===========
		// PROPERTIES
		//===========

		public ArrayList CellRefList
		{
			get
			{
				return _cellRefList;
			}
		}

		//========
		// METHODS
		//========

		override public void CheckItem(CellReference aCellRef, out bool aCancel)
		{
			_cellRefList.Add(aCellRef.Copy());
			aCancel = false;
		}
	}
}
