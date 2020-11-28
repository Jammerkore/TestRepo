using System;
using System.Collections.Generic;
using System.Text;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	public class PlanScheduleSpreadEntry : ComputationScheduleSpreadEntry
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
		/// <param name="aPlanCellRef">
		/// A reference to a ComputationCellReference that indicates which cell is to be calculated.
		/// </param>
        /// <param name="aSpreadProfile">
		/// A reference to a SpreadProfile that indicates which formula or spread to execute.
		/// </param>

		public PlanScheduleSpreadEntry(
			PlanCellReference aPlanCellRef,
			SpreadProfile aSpreadProfile,
			int aSchedulePriority,
			int aCubePriority)
			: base(aPlanCellRef, aSpreadProfile, aSchedulePriority, aCubePriority)
		{
		}

		//===========
		// PROPERTIES
		//===========

		public PlanCellReference PlanCellRef
		{
			get
			{
				return (PlanCellReference)ComputationCellRef;
			}
		}

		//========
		// METHODS
		//========
	}

	public class PlanScheduleFormulaEntry : ComputationScheduleFormulaEntry
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
		/// <param name="aPlanCellRef">
		/// A reference to a ComputationCellReference that indicates which cell is to be calculated.
		/// </param>
        /// <param name="aFormulaProfile">
		/// A reference to a FormulaSpreadProfile that indicates which formula or spread to execute.
		/// </param>

		public PlanScheduleFormulaEntry(
			ComputationCellReference aChangedPlanCellRef,
			PlanCellReference aPlanCellRef,
			FormulaProfile aFormulaProfile,
			eComputationScheduleEntryType aScheduleEntryType,
			int aSchedulePriority,
			int aCubePriority)
			: base(aChangedPlanCellRef, aPlanCellRef, aFormulaProfile, aScheduleEntryType, aSchedulePriority, aCubePriority)
		{
		}

		//===========
		// PROPERTIES
		//===========

		public PlanCellReference PlanCellRef
		{
			get
			{
				return (PlanCellReference)ComputationCellRef;
			}
		}

		//========
		// METHODS
		//========
	}
}
