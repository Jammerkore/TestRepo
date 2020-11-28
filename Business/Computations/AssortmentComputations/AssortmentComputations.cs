using System;
using System.Collections.Generic;
using System.Text;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// Abstract.  The AssortmentComputations class is a base class for Computations classes.
	/// </summary>

	abstract public class AssortmentComputations : BaseComputations
	{
		//=======
		// FIELDS
		//=======

		protected AssortmentToolBox _toolBox;
		protected AssortmentFormulasAndSpreads _formulasAndSpreads;
		protected AssortmentChangeMethods _changeMethods;
		protected AllocationVariableInitialization _variableInits;
		protected AssortmentComponentVariables _assortmentComponentVariables;
		protected AssortmentVariables _assortmentTotalVariables;
		protected AssortmentVariables _assortmentDetailVariables;
		protected AssortmentVariables _assortmentSummaryVariables;
		protected AssortmentQuantityVariables _assortmentQuantityVariables;

		//=============
		// CONSTRUCTORS
		//=============

		public AssortmentComputations()
		{
			try
			{
				_toolBox = new AssortmentToolBox(this);
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
		/// Gets the AssortmentFormulasAndSpreads object.
		/// </summary>

		public AssortmentFormulasAndSpreads FormulasAndSpreads
		{
			get
			{
				return _formulasAndSpreads;
			}
		}

		/// <summary>
		/// Gets the AssortmentChangeMethods object.
		/// </summary>

		public AssortmentChangeMethods ChangeMethods
		{
			get
			{
				return _changeMethods;
			}
		}

		/// <summary>
		/// Gets the AllocationVariableInitialization object.
		/// </summary>

		public AllocationVariableInitialization VariableInitializations
		{
			get
			{
				return _variableInits;
			}
		}

		/// <summary>
		/// Gets the ComputationsToolBox object.
		/// </summary>

		override public BaseToolBox ToolBox
		{
			get
			{
				return _toolBox;
			}
		}

		/// <summary>
		/// Gets the AssortmentComponentVariables object.
		/// </summary>

		public AssortmentComponentVariables AssortmentComponentVariables
		{
			get
			{
				return _assortmentComponentVariables;
			}
		}

		/// <summary>
		/// Gets the AssortmentTotalVariables object.
		/// </summary>

		public AssortmentVariables AssortmentTotalVariables
		{
			get
			{
				return _assortmentTotalVariables;
			}
		}

		/// <summary>
		/// Gets the AssortmentDetailVariables object.
		/// </summary>

		public AssortmentVariables AssortmentDetailVariables
		{
			get
			{
				return _assortmentDetailVariables;
			}
		}

		/// <summary>
		/// Gets the AssortmentSummaryVariables object.
		/// </summary>

		public AssortmentVariables AssortmentSummaryVariables
		{
			get
			{
				return _assortmentSummaryVariables;
			}
		}

		/// <summary>
		/// Gets the AssortmentQuantityVariables object.
		/// </summary>

		public AssortmentQuantityVariables AssortmentQuantityVariables
		{
			get
			{
				return _assortmentQuantityVariables;
			}
		}

		//========
		// METHODS
		//========

		public void Initialize(ApplicationSessionTransaction aTransaction)
		{
			try
			{
				aTransaction.SetMasterProfileList(_assortmentComponentVariables.VariableProfileList);
				aTransaction.SetMasterProfileList(_assortmentTotalVariables.VariableProfileList);
				aTransaction.SetMasterProfileList(_assortmentDetailVariables.VariableProfileList);
				aTransaction.SetMasterProfileList(_assortmentSummaryVariables.VariableProfileList);
				aTransaction.SetMasterProfileList(_assortmentQuantityVariables.VariableProfileList);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
