using System;
using System.Collections.Generic;
using System.Text;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// Abstract.  The AssortmentViewComputations class is a base class for Computations classes.
	/// </summary>

	public class AssortmentViewComputations : AssortmentComputations
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public AssortmentViewComputations()
		{
			try
			{
				_formulasAndSpreads = new AssortmentViewFormulasAndSpreads(this);
				_changeMethods = new AssortmentViewChangeMethods(this);
				_variableInits = new AssortmentViewVariableInitialization();

				_assortmentComponentVariables = new AssortmentViewComponentVariables();
				_assortmentTotalVariables = new AssortmentViewTotalVariables();
				_assortmentDetailVariables = new AssortmentViewDetailVariables();
				_assortmentSummaryVariables = new AssortmentViewSummaryVariables();
				_assortmentQuantityVariables = new AssortmentViewQuantityVariables();

				_assortmentComponentVariables.Initialize();
				_assortmentTotalVariables.Initialize();
				_assortmentDetailVariables.Initialize();
				_assortmentSummaryVariables.Initialize();
				_assortmentQuantityVariables.Initialize();
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

		//========
		// METHODS
		//========
	}
}
