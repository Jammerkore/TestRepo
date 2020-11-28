using System;
using System.Collections.Generic;
using System.Text;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	public class AssortmentScheduleSpreadEntry : ComputationScheduleSpreadEntry
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ComputationScheduleEntry for a given FormulaSpreadProfile and ComputationCellReference.
		/// </summary>
		/// <param name="aAssrtCellRef">
		/// A reference to a ComputationCellReference that indicates which cell is to be calculated.
		/// </param>
        /// <param name="aSpreadProfile">
		/// A reference to a SpreadProfile that indicates which formula or spread to execute.
		/// </param>

		public AssortmentScheduleSpreadEntry(
			AssortmentCellReference aAssrtCellRef,
			SpreadProfile aSpreadProfile,
			int aSchedulePriority,
			int aCubePriority)
			: base(aAssrtCellRef, aSpreadProfile, aSchedulePriority, aCubePriority)
		{
		}

		//===========
		// PROPERTIES
		//===========

		public AssortmentCellReference AssortmentCellRef
		{
			get
			{
				return (AssortmentCellReference)ComputationCellRef;
			}
		}

		//========
		// METHODS
		//========
	}

	public class AssortmentScheduleFormulaEntry : ComputationScheduleFormulaEntry
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ComputationScheduleEntry for a given FormulaSpreadProfile and ComputationCellReference.
		/// </summary>
		/// <param name="aAssrtCellRef">
		/// A reference to a ComputationCellReference that indicates which cell is to be calculated.
		/// </param>
        /// <param name="aFormulaProfile">
		/// A reference to a FormulaProfile that indicates which formula or spread to execute.
		/// </param>

		public AssortmentScheduleFormulaEntry(
			ComputationCellReference aChangedAssrtCellRef,
			AssortmentCellReference aAssrtCellRef,
			FormulaProfile aFormulaProfile,
			eComputationScheduleEntryType aScheduleEntryType,
			int aSchedulePriority,
			int aCubePriority)
			: base(aChangedAssrtCellRef, aAssrtCellRef, aFormulaProfile, aScheduleEntryType, aSchedulePriority, aCubePriority)
		{
		}

		//===========
		// PROPERTIES
		//===========

		public AssortmentCellReference AssortmentCellRef
		{
			get
			{
				return (AssortmentCellReference)ComputationCellRef;
			}
		}

		//========
		// METHODS
		//========
	}
}
