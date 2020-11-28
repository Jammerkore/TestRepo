using System;
using System.Collections.Generic;
using System.Text;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// Abstract.  The AllocationSummaryComputations class is a base class for Computations classes.
	/// </summary>

	public class AllocationSummaryComputations : AssortmentComputations
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public AllocationSummaryComputations()
		{
			try
			{
				_formulasAndSpreads = new AllocationSummaryFormulasAndSpreads(this);
				_changeMethods = new AllocationSummaryChangeMethods(this);
				_variableInits = new AllocationSummaryVariableInitialization();

				_assortmentComponentVariables = new AllocationSummaryComponentVariables();
				_assortmentTotalVariables = new AllocationSummaryTotalVariables();
				_assortmentDetailVariables = new AllocationSummaryDetailVariables();
				_assortmentSummaryVariables = new AllocationSummarySummaryVariables();
				_assortmentQuantityVariables = new AllocationSummaryQuantityVariables();

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
