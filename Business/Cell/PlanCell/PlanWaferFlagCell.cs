using System;
using System.Collections.Generic;
using System.Text;

namespace MIDRetail.Business
{
	public class PlanWaferFlagCell : PlanWaferCell
	{
		//=======
		// FIELDS
		//=======

		private ComputationCellFlags _cellFlags;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of PlanWaferCell.
		/// </summary>

		public PlanWaferFlagCell()
			: base()
		{
		}

		/// <summary>
		/// Creates a new instance of PlanWaferCell using the given PlanCellReference and value.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// A PlanCellReference reference that points to the PlanCell to create a PlanWaferCell for.
		/// </param>
		public PlanWaferFlagCell(PlanCellReference aPlanCellRef, double aValue, string aUnitScaling, string aDollarScaling)
			: base(aPlanCellRef, aValue, aUnitScaling, aDollarScaling)
		{
			
		}

		public PlanWaferFlagCell(PlanCellReference aPlanCellRef, double aValue, string aUnitScaling, string aDollarScaling, bool aUseCommas)
			: base(aPlanCellRef, aValue, aUnitScaling, aDollarScaling, aUseCommas)
		{
			
		}

		/// <summary>
		/// Creates a new instance of PlanWaferCell using the given PlanCellReference.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// A PlanCellReference reference that points to the PlanCell to create a PlanWaferCell for.
		/// </param>

		public PlanWaferFlagCell(PlanCellReference aPlanCellRef)
			: base(aPlanCellRef)
		{
			_cellFlags = aPlanCellRef.CellFlags;
		}

		/// <summary>
		/// Creates a new instance of PlanWaferCell and initializes it with a copy of the given PlanWaferCell.
		/// </summary>
		/// <param name="aPlanWaferCell">
		/// The PlanWaferCell to copy from.
		/// </param>

		protected PlanWaferFlagCell(PlanWaferCell aPlanWaferCell)
			: base(aPlanWaferCell)
		{
			
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the PlanCellFlags structure containing the flags for the cell.
		/// </summary>

		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		//public ComputationCellFlags Flags
		override public ComputationCellFlags Flags
		//End TT#2 - JScott - Assortment Planning - Phase 2
		{
			get
			{
				return _cellFlags;
			}
		}

		//=============
		// METHODS
		//=============

		override protected void Initialize(PlanCellReference aPlanCellRef, double aValue, string aUnitScaling, string aDollarScaling, bool aUseCommas)
		{
			try
			{
				base.Initialize(aPlanCellRef, aValue, aUnitScaling, aDollarScaling, aUseCommas);
				_cellFlags = aPlanCellRef.CellFlags;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		public override WaferCell Copy()
		{
			PlanWaferFlagCell planWaferFlagCell;

			try
			{
				planWaferFlagCell = new PlanWaferFlagCell();
				planWaferFlagCell.CopyFrom(this);

				return planWaferFlagCell;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End TT#2 - JScott - Assortment Planning - Phase 2
		/// <summary>
		/// Override.  Method that copies values from a PlanWaferCell object to the current object.
		/// </summary>
		/// <param name="aWaferCell">
		/// The WaferCell object to copy from.
		/// </param>

		override public void CopyFrom(WaferCell aWaferCell)
		{
			try
			{
				base.CopyFrom(aWaferCell);
				_cellFlags = ((PlanWaferFlagCell)aWaferCell)._cellFlags;
				//WaferCellDetail = ((PlanWaferCell)aWaferCell).WaferCellDetail.Copy();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
