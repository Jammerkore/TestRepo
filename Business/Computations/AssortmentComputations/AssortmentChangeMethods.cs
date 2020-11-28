using System;
using System.Collections;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;

namespace MIDRetail.Business
{
	/// <summary>
	/// The ChangeMethods class is where the change routines are defined.
	/// </summary>
	/// <remarks>
	/// This class is where the change routines are defined.  A change routine is defined as a ChangeMethodDelegate that points to a
	/// method within this class that executes the change rules.  This method will contain all the logic to update appropriate values when a Cell is changed.
	/// </remarks>

	abstract public class AssortmentChangeMethods
	{
		//=======
		// FIELDS
		//=======

		AssortmentComputations _computations;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Static.  Creates a new instance of ChangeMethods.
		/// </summary>

		public AssortmentChangeMethods(AssortmentComputations aComputations)
		{
			_computations = aComputations;
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		//========
		// CLASSES
		//========
	}
}
