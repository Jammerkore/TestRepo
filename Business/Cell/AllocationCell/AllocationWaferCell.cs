using System;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;
using System.Globalization;


namespace MIDRetail.Business.Allocation
{

	/// <summary>
	/// The AllocationCubeCell class defines the cell for a Allocation.
	/// </summary>
	/// <remarks>
	/// AllocationCubeCell contains additional information that is required by a Allocation.  This information includes a double value.
	/// </remarks>

	[Serializable]
	public class AllocationWaferCell : Cell
	{
		//=======
		// FIELDS
		//=======

//		private static readonly string[] decimalFormats = 
//					{
//						"###,###,##0;-###,###,##0;0",
//						"###,###,##0.0;-###,###,##0.0;0.0",
//						"###,###,##0.00;-###,###,##0.00;0.00",
//						"###,###,##0.000;-###,###,##0.000;0.000",
//						"###,###,##0.0000;-###,###,##0.0000;0.0000",
//						"###,###,##0.00000;-###,###,##0.00000;0.00000"   
//					};
		private double _value;
		private string _valueAsString;
//		private byte _flags;
		private uint _flags;
		eAllocationWaferVariable _variableID;
		private double _gradeMax;
		private double _primaryMax;
		private double _min;
		private int _multiple;
//		private bool _cellIsValid;
//		private bool _cellCanBeChanged;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AllocationWaferCell.
		/// </summary>

		public AllocationWaferCell()
			: base()
		{
			_value = 0.0;
			_valueAsString = "";
			_variableID = eAllocationWaferVariable.None;
			_flags = 0;
			this.CellIsValid = true;
//			_cellIsValid = true;
			_gradeMax = double.MaxValue;
			_primaryMax = double.MaxValue;
			_min = double.MinValue;
			_multiple = 1;
		}

		/// <summary>
		/// Creates a new instance of AllocationWaferCell with the specified value.
		/// </summary>

		private AllocationWaferCell(double aValue,eAllocationWaferVariable aVariableKey)
			: base()
		{
			_variableID = aVariableKey;
			Value = aValue;
			_flags = 0;
			this.CellIsValid = true;
//			_cellIsValid = true;
			_gradeMax = double.MaxValue;
			_primaryMax = double.MaxValue;
			_min = double.MinValue;
			_multiple = 1;
		}

		public AllocationWaferCell(string aStringValue, eAllocationWaferVariable aVariableKey)
			: base()
		{
			_value = 0;
			_variableID = aVariableKey;
			_valueAsString = aStringValue;
			_flags = 0;
			this.CellIsValid = true;
			_gradeMax = double.MaxValue;
			_primaryMax = double.MaxValue;
			_min = double.MinValue;
			_multiple = 1;
		}
			//			_cellIsValid = true;		}

		/// <summary>
		/// Creates a new instance of AllocationWaferCell and initializes it with a copy of the given AllocationWaferCell.
		/// </summary>
		/// <param name="aAllocationWaferCell">
		/// The AllocationWaferCell to copy from.
		/// </param>

		protected AllocationWaferCell(AllocationWaferCell aAllocationWaferCell)
			: base()
		{
			CopyFrom(aAllocationWaferCell);
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the value of the numeric cell value.
		/// </summary>
		
		public double Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				_valueAsString = this.GetFormattedValue();
			}
		}

		/// <summary>
		/// Gets the formated numeric cell value.
		/// </summary>

		public string ValueAsString
		{
			get { return _valueAsString; }
			set { _valueAsString = value; }
		}

		/// <summary>
		/// Gets or sets the grade maximum value for the cell.
		/// </summary>
		public double GradeMaximumValue
		{
			get { return _gradeMax; }
			set { _gradeMax = value; }
		}

		/// <summary>
		/// Gets or sets the primary maximum value for the cell.
		/// </summary>
		public double PrimaryMaximumValue
		{
			get { return _primaryMax; }
			set { _primaryMax = value; }
		}

		/// <summary>
		/// Gets or sets the minimum value for the cell.
		/// </summary>
		public double MinimumValue
		{
			get { return _min; }
			set { _min = value; }
		}

        /// <summary>
        /// Gets or sets the multiple by which the cell integer values must be evenly divisible.
        /// </summary>
		public int Multiple
		{
			get { return _multiple; }
			set 
			{
				if (value < 1)
				{
                    throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_MultipleCannotBeLessThan1,
						MIDText.GetText(eMIDTextCode.msg_MultipleCannotBeLessThan1));					
				}
				_multiple = value; 					
			}
		}

		/// <summary>
		/// Gets or sets the flag identifying if the cell is valid to display.
		/// </summary>
		public bool CellIsValid
		{
//			get { return _cellIsValid; }
//			set { _cellIsValid = value; }
			get
			{
                 return MIDFlag.GetFlagValue(_flags,(int)eAllocationWaferCellFlag.CellIsValid);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationWaferCellFlag.CellIsValid,value);
			}
		}

		/// <summary>
		/// Gets or sets the flag identifying if the cell can be adjusted.
		/// </summary>

		public bool CellCanBeChanged
		{
//			get { return _cellCanBeChanged; }
//			set { _cellCanBeChanged = value; }
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationWaferCellFlag.CellCanBeChanged);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationWaferCellFlag.CellCanBeChanged,value);
			}
		}

		/// <summary>
		/// Gets or sets the flag to allow exceed grade maximum.
		/// </summary>
		public bool MayExceedGradeMaximum
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationWaferCellFlag.MayExceedGradeMaximum);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationWaferCellFlag.MayExceedGradeMaximum,value);
			}
		}


		/// <summary>
		/// Gets or sets the flag to allow exceed primary maximum.
		/// </summary>
		public bool MayExceedPrimaryMaximum
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationWaferCellFlag.MayExceedPrimaryMaximum);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationWaferCellFlag.MayExceedPrimaryMaximum,value);
			}
		}

		/// <summary>
		/// Gets or sets the flag to allow exceed capacity maximum.
		/// </summary>
		public bool MayExceedCapacityMaximum
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationWaferCellFlag.MayExceedCapacityMaximum);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationWaferCellFlag.MayExceedCapacityMaximum,value);
			}
		}


		/// <summary>
		/// Gets or sets the flag that indicates when a store exceeds capacity.
		/// </summary>
		public bool StoreExceedsCapacity
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags,(int)eAllocationWaferCellFlag.StoreExceedsCapacity);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationWaferCellFlag.StoreExceedsCapacity,value);
			}
		}

		// BEGIN MID Track # 1511 Highlight Stores that are out of balance
		/// <summary>
		/// Gets or sets the flag that indicates whether a store's allocation is out of balance with its components
		/// </summary>
		public bool StoreAllocationOutOfBalance
		{
			get
			{
				return MIDFlag.GetFlagValue(_flags, (int)eAllocationWaferCellFlag.StoreAllocationOutOfBalance);
			}
			set
			{
				_flags = MIDFlag.SetFlagValue(_flags,(int)eAllocationWaferCellFlag.StoreAllocationOutOfBalance, value);
			}
		}
		// END MID Track # 1511 

		//========
		// METHODS
		//========

		/// <summary>
		/// Override.  Method defining the Clone functionality.  Clone creates a shallow copy of the object.
		/// </summary>
		/// <returns>
		/// Object reference to cloned object.
		/// </returns>

		override public Cell Clone()
		{
			return Copy();
		}

		/// <summary>
		/// Override.  Method defining the Copy functionality.  Copy creates a deep copy. 
		/// </summary>
		/// <returns>
		/// Object reference to the copied object.
		/// </returns>

		override public Cell Copy()
		{
			AllocationWaferCell allocationWaferCell;
			allocationWaferCell = new AllocationWaferCell();
			allocationWaferCell.CopyFrom(this);
			return allocationWaferCell;
		}

		/// <summary>
		/// Override.  Method that copies values from a AllocationWaferCell object to the current object.
		/// </summary>
		/// <param name="aCell">
		/// The AllocationWaferCell object to copy from.
		/// </param>

		override public void CopyFrom(Cell aCell)
		{
			_value = ((AllocationWaferCell)aCell)._value;
			_valueAsString = ((AllocationWaferCell)aCell)._valueAsString;
		}

		/// <summary>
		/// Override.  Defines the method to clear the contents of the AllocationWaferCell.
		/// </summary>

		override public void Clear()
		{
			_value = 0.0;
			_valueAsString = "";
		}

		/// <summary>
		/// Returns the value of the cell as a formatted string.
		/// </summary>
		/// <returns>
		/// The formatted string value.
		/// </returns>

		private string GetFormattedValue()
		{
			AllocationWaferVariable varProf;
			string fmtString;

			if (_variableID < 0)
			{
				return this.Value.ToString("###,###,##0.00", CultureInfo.CurrentUICulture);
			}
			else
			{
				varProf = AllocationWaferVariables.GetVariableProfile(_variableID);
				switch (varProf.Format)
				{
					case eAllocationWaferVariableFormat.Number:
						if (varProf.NumDecimals < Include.DecimalFormats.Length)
						{
							fmtString = Include.DecimalFormats[varProf.NumDecimals];
						}
						else
						{
							fmtString = "###,###,##0.";
							fmtString.PadRight(fmtString.Length + varProf.NumDecimals, '0');
						}
						return this.Value.ToString(fmtString, CultureInfo.CurrentUICulture);
//						break;

					case eAllocationWaferVariableFormat.String:
						return this.ValueAsString;
//						break;

					default:
						// TODO: Error logging
						throw new Exception("Invalid value format");
//						break;
				}
			}
		}
	}
}