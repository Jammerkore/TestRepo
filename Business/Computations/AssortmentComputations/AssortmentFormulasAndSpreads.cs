using System;
using System.Collections;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;

namespace MIDRetail.Business
{
	/// <summary>
	/// The FormulasAndSpreads class is where the Formula and Spread routines are defined.
	/// </summary>
	/// <remarks>
	/// This class is where the Formula and Spread routines are defined.  A formula or spread is defined in a FormulaSpreadProfile, which contains
	/// an Id, a name, and a FormulaSpreadDelegate that points to a method within this class that executes the formula or spread.  This method will contain
	/// all the logic to calculate or spread values as required.
	/// </remarks>

	abstract public class AssortmentFormulasAndSpreads
	{
		//=======
		// FIELDS
		//=======

		AssortmentComputations _computations;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Static.  Creates a new instance of FormulasAndSpreads.
		/// </summary>

		public AssortmentFormulasAndSpreads(AssortmentComputations aComputations)
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
