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
	public class AssortmentWaferCell : WaferCell
	{
		//=======
		// FIELDS
		//=======

		private ComputationCellFlags _cellFlags;
		private WaferCellDetail _waferCellDetail;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AssortmentWaferCell.
		/// </summary>

		public AssortmentWaferCell()
			: base()
		{
		}

		//BEGIN TODO: Temporary test code
		/// <summary>
		/// Creates a new instance of AssortmentWaferCell using the given parameters and value.
		/// </summary>

		public AssortmentWaferCell(
			ApplicationSessionTransaction aTransaction,
			int aNodeKey,
			ComputationCellFlags aCellFlags,
			eValueFormatType aValFormatType,
			eVariableStyle aVarStyle,
			int aNumDecimals,
			double aValue)
			: base(aValue)
		{
			try
			{
				_cellFlags = aCellFlags;

				switch (aValFormatType)
				{
					case eValueFormatType.GenericNumeric:

						_waferCellDetail = new GenericNumericWaferCellDetail(
								this,
								aVarStyle,
								aNumDecimals,
								//Begin Modification - JScott - Add Scaling Decimals
								//1,
								//1);
								"1",
								"1");
								//End Modification - JScott - Add Scaling Decimals

						break;

					case eValueFormatType.StoreGrade:

						_waferCellDetail = new StoreGradeWaferCellDetail(this, aTransaction, aNodeKey);

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

		//END TODO: Temporary test code
		/// <summary>
		/// Creates a new instance of AssortmentWaferCell using the given parameters and value.
		/// </summary>

		public AssortmentWaferCell(AssortmentCellReference aAssrtCellRef, double aValue)
			: base(aValue)
		{
			ComputationVariableProfile varProf;

			try
			{
				_cellFlags = aAssrtCellRef.CellFlags;

				varProf = aAssrtCellRef.GetFormatTypeVariableProfile();

				switch (varProf.FormatType)
				{
					case eValueFormatType.GenericNumeric:

						_waferCellDetail = new GenericNumericWaferCellDetail(
								this,
								aAssrtCellRef.GetVariableStyleVariableProfile().VariableStyle,
								//Begin BonTon Calcs - JScott - Add Display Precision
								//aAssrtCellRef.GetFormatTypeVariableProfile().NumDecimals,
								aAssrtCellRef.GetFormatTypeVariableProfile().NumDisplayDecimals,
								//End BonTon Calcs - JScott - Add Display Precision
								//Begin Modification - JScott - Add Scaling Decimals
								//1,
								//1);
								"1",
								"1");
								//End Modification - JScott - Add Scaling Decimals

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

		/// <summary>
		/// Creates a new instance of AssortmentWaferCell using the given parameters.
		/// </summary>

		public AssortmentWaferCell(ComputationCellFlags aCellFlags)
			: base(0)
		{
			_cellFlags = aCellFlags;
			_waferCellDetail = new NullWaferCellDetail(this);
		}

		/// <summary>
		/// Creates a new instance of AssortmentWaferCell and initializes it with a copy of the given AssortmentWaferCell.
		/// </summary>
		/// <param name="aAssortmentWaferCell">
		/// The AssortmentWaferCell to copy from.
		/// </param>

		protected AssortmentWaferCell(AssortmentWaferCell aAssortmentWaferCell)
			: base()
		{
			try
			{
				CopyFrom(aAssortmentWaferCell);
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
		/// Gets the AssortmentCellFlags structure containing the flags for the cell.
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

		/// <summary>
		/// Gets the string representation of the numeric cell value.
		/// </summary>

		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		//public string ValueAsString
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

		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		override public bool isValueNumeric
		{
			get
			{
				return true;
			}
		}

		override public int NumberOfDecimals
		{
			get
			{
				return 0;
			}
		}

        override public eVariableStyle VariableStyle
        {
            get
            {
                return _waferCellDetail.VariableStyle;
            }
        }
		//End TT#2 - JScott - Assortment Planning - Phase 2
		//========
		// METHODS
		//========

		/// <summary>
		/// Override.  Method defining the Copy functionality.  Copy creates a deep copy. 
		/// </summary>
		/// <returns>
		/// Object reference to the copied object.
		/// </returns>

		override public WaferCell Copy()
		{
			AssortmentWaferCell assortmentWaferCell;

			try
			{
				assortmentWaferCell = new AssortmentWaferCell();
				assortmentWaferCell.CopyFrom(this);

				return assortmentWaferCell;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Override.  Method that copies values from a AssortmentWaferCell object to the current object.
		/// </summary>
		/// <param name="aWaferCell">
		/// The WaferCell object to copy from.
		/// </param>

		override public void CopyFrom(WaferCell aWaferCell)
		{
			try
			{
				base.CopyFrom(aWaferCell);
				_cellFlags = ((AssortmentWaferCell)aWaferCell)._cellFlags;
				_waferCellDetail = ((AssortmentWaferCell)aWaferCell)._waferCellDetail.Copy();
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
		/// Override.  Defines the method to clear the contents of the AssortmentWaferCell.
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
