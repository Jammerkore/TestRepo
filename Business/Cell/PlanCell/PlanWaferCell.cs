using System;
using System.Globalization;
using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// The WaferCell class defines the wafer cell that is used to transport data from the Application Session to the window.
	/// </summary>

	[Serializable]
	public class PlanWaferCell : WaferCell
	{
		//=======
		// FIELDS
		//=======

		// Begin Track #6415 - stodd
		// private ComputationCellFlags _cellFlags;
		// End Track # 6415
		private WaferCellDetail _waferCellDetail;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of PlanWaferCell.
		/// </summary>

		public PlanWaferCell()
			: base()
		{
		}

		/// <summary>
		/// Creates a new instance of PlanWaferCell using the given PlanCellReference and value.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// A PlanCellReference reference that points to the PlanCell to create a PlanWaferCell for.
		/// </param>

		//Begin BonTon Calcs - JScott - Add Display Precision
		//public PlanWaferCell(PlanCellReference aPlanCellRef, double aValue, int aUnitScaling, int aDollarScaling)
		//    : base(aValue)
		//{
		//    ComputationVariableProfile varProf;

		//    try
		//    {
		//        _cellFlags = aPlanCellRef.CellFlags;

		//        varProf = aPlanCellRef.GetFormatTypeVariableProfile();

		//        switch (varProf.FormatType)
		//        {
		//            case eValueFormatType.GenericNumeric:

		//                _waferCellDetail = new GenericNumericWaferCellDetail(
		//                        this,
		//                        aPlanCellRef.GetVariableStyleVariableProfile().VariableStyle,
		//                        //Begin BonTon Calcs - JScott - Add Display Precision
		//                        //aPlanCellRef.GetFormatTypeVariableProfile().NumDecimals,
		//                        varProf.NumDisplayDecimals,
		//                        //End BonTon Calcs - JScott - Add Display Precision
		//                        aUnitScaling,
		//                        aDollarScaling);

		//                break;

		//            case eValueFormatType.StoreGrade:

		//                _waferCellDetail = new StoreGradeWaferCellDetail(this, aPlanCellRef.Cube.Transaction, aPlanCellRef[eProfileType.HierarchyNode]);

		//                break;

		//            case eValueFormatType.StoreStatus:

		//                _waferCellDetail = new StoreStatusWaferCellDetail(this, 0);

		//                break;

		//            default:

		//                throw new MIDException(eErrorLevel.severe,
		//                    (int)eMIDTextCode.msg_pl_InvalidValueFormat,
		//                    MIDText.GetText(eMIDTextCode.msg_pl_InvalidValueFormat));
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}
		//Begin Modification - JScott - Add Scaling Decimals
		//public PlanWaferCell(PlanCellReference aPlanCellRef, double aValue, int aUnitScaling, int aDollarScaling)
		public PlanWaferCell(PlanCellReference aPlanCellRef, double aValue, string aUnitScaling, string aDollarScaling)
		//End Modification - JScott - Add Scaling Decimals
			: base(aValue)
		{
			try
			{
				Initialize(aPlanCellRef, aValue, aUnitScaling, aDollarScaling, true);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Modification - JScott - Add Scaling Decimals
		//public PlanWaferCell(PlanCellReference aPlanCellRef, double aValue, int aUnitScaling, int aDollarScaling, bool aUseCommas)
		public PlanWaferCell(PlanCellReference aPlanCellRef, double aValue, string aUnitScaling, string aDollarScaling, bool aUseCommas)
		//End Modification - JScott - Add Scaling Decimals
			: base(aValue)
		{
			try
			{
				Initialize(aPlanCellRef, aValue, aUnitScaling, aDollarScaling, aUseCommas);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//Begin BonTon Calcs - JScott - Add Display Precision

		/// <summary>
		/// Creates a new instance of PlanWaferCell using the given PlanCellReference.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// A PlanCellReference reference that points to the PlanCell to create a PlanWaferCell for.
		/// </param>

		public PlanWaferCell(PlanCellReference aPlanCellRef)
			: base(0)
		{
			// Begin Track #6415 - stodd
			//_cellFlags = aPlanCellRef.CellFlags;
			// End Track #6415
			_waferCellDetail = new NullWaferCellDetail(this);
		}

		/// <summary>
		/// Creates a new instance of PlanWaferCell and initializes it with a copy of the given PlanWaferCell.
		/// </summary>
		/// <param name="aPlanWaferCell">
		/// The PlanWaferCell to copy from.
		/// </param>

		protected PlanWaferCell(PlanWaferCell aPlanWaferCell)
			: base()
		{
			try
			{
				CopyFrom(aPlanWaferCell);
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

		// Begin Track #6415 - stodd
		///// <summary>
		///// Gets the PlanCellFlags structure containing the flags for the cell.
		///// </summary>

		//public ComputationCellFlags Flags
		//{
		//    get
		//    {
		//        return _cellFlags;
		//    }
		//}
		// End Track #6415

		/// <summary>
		/// Gets the string representation of the numeric cell value.
		/// </summary>

		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		//public string ValueAsString
		override public ComputationCellFlags Flags
		{
			get
			{
				throw new Exception("Invalid Call -- Flags not defined for PlanWaferCell");
			}
		}

		override public string ValueAsString
		//End TT#2 - JScott - Assortment Planning - Phase 2
		{
			get
			{
				try
				{
					return _waferCellDetail.ValueAsString;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the numeric value is negative.
		/// </summary>

		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		//public bool isValueNegative
		override public bool isValueNegative
		//End TT#2 - JScott - Assortment Planning - Phase 2
		{
			get
			{
				try
				{
					return _waferCellDetail.isValueNegative;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the value is numeric.
		/// </summary>

		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		//public bool isValueNumeric
		override public bool isValueNumeric
		//End TT#2 - JScott - Assortment Planning - Phase 2
		{
			get
			{
				try
				{
					return _waferCellDetail.isValueNumeric;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets an integer indicating the number of decimal digits.
		/// </summary>

		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		//public int NumberOfDecimals
		override public int NumberOfDecimals
		//End TT#2 - JScott - Assortment Planning - Phase 2
		{
			get
			{
				try
				{
					return _waferCellDetail.NumberOfDecimals;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//========
		// METHODS
		//========

		//Begin BonTon Calcs - JScott - Add Display Precision
		//Begin Modification - JScott - Add Scaling Decimals
		//private void Initialize(PlanCellReference aPlanCellRef, double aValue, int aUnitScaling, int aDollarScaling, bool aUseCommas)
		virtual protected void Initialize(PlanCellReference aPlanCellRef, double aValue, string aUnitScaling, string aDollarScaling, bool aUseCommas)
		//End Modification - JScott - Add Scaling Decimals
		{
			ComputationVariableProfile varProf;

			try
			{
				// Begin Track #6415 - stodd
				//_cellFlags = aPlanCellRef.CellFlags;
				// End Track #6415

				varProf = aPlanCellRef.GetFormatTypeVariableProfile();

				switch (varProf.FormatType)
				{
					case eValueFormatType.GenericNumeric:

						_waferCellDetail = new GenericNumericWaferCellDetail(
								this,
								aPlanCellRef.GetVariableStyleVariableProfile().VariableStyle,
								varProf.NumDisplayDecimals,
								aUnitScaling,
								aDollarScaling,
								aUseCommas);

						break;

					case eValueFormatType.StoreGrade:

						//Begin Track #4581 - JScott - Custom Variables not calculating on Basis line
						//_waferCellDetail = new StoreGradeWaferCellDetail(this, aPlanCellRef.Cube.Transaction, aPlanCellRef[eProfileType.HierarchyNode]);
						_waferCellDetail = new StoreGradeWaferCellDetail(this, aPlanCellRef.Cube.Transaction, aPlanCellRef.GetHierarchyNodeProfile().Key);
						//End Track #4581 - JScott - Custom Variables not calculating on Basis line

						break;

					case eValueFormatType.StoreStatus:

						_waferCellDetail = new StoreStatusWaferCellDetail(this, 0);

						break;

					default:

						throw new MIDException(eErrorLevel.severe,
							(int)eMIDTextCode.msg_pl_InvalidValueFormat,
							MIDText.GetText(eMIDTextCode.msg_pl_InvalidValueFormat));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End BonTon Calcs - JScott - Add Display Precision
		/// <summary>
		/// Override.  Method defining the Copy functionality.  Copy creates a deep copy. 
		/// </summary>
		/// <returns>
		/// Object reference to the copied object.
		/// </returns>

		override public WaferCell Copy()
		{
			PlanWaferCell planWaferCell;

			try
			{
				planWaferCell = new PlanWaferCell();
				planWaferCell.CopyFrom(this);

				return planWaferCell;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

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
				// Begin Track #6415 - stodd
				//_cellFlags = ((PlanWaferCell)aWaferCell)._cellFlags;
				// End Track #6415 - stodd
				_waferCellDetail = ((PlanWaferCell)aWaferCell)._waferCellDetail.Copy();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the string representation of the numeric cell value.
		/// </summary>
		/// <returns>
		/// The string representation of the numeric cell value.
		/// </returns>

		public override string ToString()
		{
			try
			{
				return ValueAsString;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Override.  Defines the method to clear the contents of the PlanWaferCell.
		/// </summary>

		override public void Clear()
		{
			try
			{
				base.Clear();
				_waferCellDetail.Clear();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
