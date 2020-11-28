using System;
using System.Globalization;
using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// The WaferCellDetail class defines a detail for a WaferCell.
	/// </summary>

	[Serializable]
	abstract public class WaferCellDetail
	{
		//=======
		// FIELDS
		//=======

		protected WaferCell _waferCell;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of WaferCellDetail
		/// </summary>

		public WaferCellDetail()
		{
			_waferCell = null;
		}

		/// <summary>
		/// Creates a new instance of WaferCellDetail with the given WaferCell
		/// </summary>
		/// <param name="aWaferCell">
		/// </param>

		public WaferCellDetail(WaferCell aWaferCell)
		{
			_waferCell = aWaferCell;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the string representation of the numeric cell value.
		/// </summary>

		abstract public string ValueAsString { get; }

		/// <summary>
		/// Gets a boolean indicating if the numeric value is negative.
		/// </summary>

		abstract public bool isValueNegative { get; }

		/// <summary>
		/// Gets a boolean indicating if the value is numeric.
		/// </summary>

		abstract public bool isValueNumeric { get; }

		/// <summary>
		/// Gets an integer indicating the number of decimal digits.
		/// </summary>

		abstract public int NumberOfDecimals { get; }

		//========
		// METHODS
		//========

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
		/// Override.  Method defining the Clone functionality.  Clone creates a shallow copy of the object.
		/// </summary>
		/// <returns>
		/// Object reference to cloned object.
		/// </returns>

		public WaferCellDetail Clone()
		{
			try
			{
				return Copy();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Override.  Method defining the Copy functionality.  Copy creates a deep copy. 
		/// </summary>
		/// <returns>
		/// Object reference to the copied object.
		/// </returns>

		abstract public WaferCellDetail Copy();

		/// <summary>
		/// Override.  Method that copies values from a WaferCell object to the current object.
		/// </summary>
        /// <param name="aWaferCellDetail">
		/// The WaferCell object to copy from.
		/// </param>

		virtual public void CopyFrom(WaferCellDetail aWaferCellDetail)
		{
			try
			{
				_waferCell = aWaferCellDetail._waferCell;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Override.  Defines the method to clear the contents of the WaferCell.
		/// </summary>

		abstract public void Clear();
	}

	/// <summary>
	/// The NullWaferCellDetail class defines a detail for a WaferCell.
	/// </summary>

	[Serializable]
	public class NullWaferCellDetail : WaferCellDetail
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of NullWaferCellDetail
		/// </summary>

		public NullWaferCellDetail()
			: base()
		{
		}

		/// <summary>
		/// Creates a new instance of NullWaferCellDetail.
		/// </summary>

		public NullWaferCellDetail(WaferCell aWaferCell)
			: base(aWaferCell)
		{
		}

		/// <summary>
		/// Creates a new instance of NullWaferCellDetail and initializes it with a copy of the given WaferCell.
		/// </summary>
        /// <param name="aNullWaferCellDetail">
		/// The WaferCell to copy from.
		/// </param>

		protected NullWaferCellDetail(NullWaferCellDetail aNullWaferCellDetail)
			: base()
		{
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the string representation of the numeric cell value.
		/// </summary>

		override public string ValueAsString
		{
			get
			{
				return "";
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the numeric value is negative.
		/// </summary>

		override public bool isValueNegative
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the value is numeric.
		/// </summary>

		override public bool isValueNumeric
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets an integer indicating the number of decimal digits.
		/// </summary>

		override public int NumberOfDecimals
		{
			get
			{
				return 0;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Override.  Method defining the Copy functionality.  Copy creates a deep copy. 
		/// </summary>
		/// <returns>
		/// Object reference to the copied object.
		/// </returns>

		override public WaferCellDetail Copy()
		{
			WaferCellDetail waferCellDetail;

			try
			{
				waferCellDetail = new NullWaferCellDetail(this);
				waferCellDetail.CopyFrom(this);

				return waferCellDetail;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Override.  Method that copies values from a WaferCell object to the current object.
		/// </summary>
        /// <param name="aWaferCellDetail">
		/// The WaferCell object to copy from.
		/// </param>

		override public void CopyFrom(WaferCellDetail aWaferCellDetail)
		{
			try
			{
				base.CopyFrom(aWaferCellDetail);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Override.  Defines the method to clear the contents of the WaferCell.
		/// </summary>

		override public void Clear()
		{
		}
	}

	/// <summary>
	/// The GenericNumericWaferCellDetail class defines a detail for a WaferCell.
	/// </summary>

	[Serializable]
	public class GenericNumericWaferCellDetail : WaferCellDetail
	{
		//=======
		// FIELDS
		//=======

		private string _formatString;
		private eVariableStyle _varStyle;
		private int _numDecimals;
		//Begin Modification - JScott - Add Scaling Decimals
		//private int _unitScaling;
		//private int _dollarScaling;
		private int _scaling;
		//End Modification - JScott - Add Scaling Decimals

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of GenericNumericWaferCellDetail
		/// </summary>

		public GenericNumericWaferCellDetail()
			: base()
		{
		}

		/// <summary>
		/// Creates a new instance of GenericNumericWaferCellDetail using the given paramters and value.
		/// </summary>

		//Begin BonTon Calcs - JScott - Add Display Precision
		//public GenericNumericWaferCellDetail(WaferCell aWaferCell, eVariableStyle aVarStyle, int aNumDecimals, int aUnitScaling, int aDollarScaling)
		//    : base(aWaferCell)
		//{
		//    try
		//    {
		//        _unitScaling = aUnitScaling;
		//        _dollarScaling = aDollarScaling;

		//        _varStyle = aVarStyle;
		//        _numDecimals = aNumDecimals;

		//        if (_numDecimals < Include.DecimalFormats.Length)
		//        {
		//            _formatString = Include.DecimalFormats[_numDecimals];
		//        }
		//        else
		//        {
		//            _formatString = "###,###,##0.";
		//            _formatString = _formatString.PadRight(_formatString.Length + _numDecimals, '0');
		//        }

		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}
		//Begin Modification - JScott - Add Scaling Decimals
		//public GenericNumericWaferCellDetail(WaferCell aWaferCell, eVariableStyle aVarStyle, int aNumDecimals, int aUnitScaling, int aDollarScaling)
		public GenericNumericWaferCellDetail(WaferCell aWaferCell, eVariableStyle aVarStyle, int aNumDecimals, string aUnitScaling, string aDollarScaling)
		//End Modification - JScott - Add Scaling Decimals
			: base(aWaferCell)
		{
			try
			{
				Initialize(aWaferCell, aVarStyle, aNumDecimals, aUnitScaling, aDollarScaling, true);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Modification - JScott - Add Scaling Decimals
		//public GenericNumericWaferCellDetail(WaferCell aWaferCell, eVariableStyle aVarStyle, int aNumDecimals, int aUnitScaling, int aDollarScaling, bool aUseCommas)
		public GenericNumericWaferCellDetail(WaferCell aWaferCell, eVariableStyle aVarStyle, int aNumDecimals, string aUnitScaling, string aDollarScaling, bool aUseCommas)
		//End Modification - JScott - Add Scaling Decimals
			: base(aWaferCell)
		{
			try
			{
				Initialize(aWaferCell, aVarStyle, aNumDecimals, aUnitScaling, aDollarScaling, aUseCommas);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End BonTon Calcs - JScott - Add Display Precision

		/// <summary>
		/// Creates a new instance of WaferCell and initializes it with a copy of the given WaferCell.
		/// </summary>
        /// <param name="aGenericNumericWaferCellDetail">
		/// The WaferCell to copy from.
		/// </param>

		protected GenericNumericWaferCellDetail(GenericNumericWaferCellDetail aGenericNumericWaferCellDetail)
			: base()
		{
			try
			{
				CopyFrom(aGenericNumericWaferCellDetail);
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
		/// Gets the string representation of the numeric cell value.
		/// </summary>

		override public string ValueAsString
		{
			get
			{
				double cellValue;

				try
				{
					if (_formatString != null)
					{
						switch (_varStyle)
						{
							//Begin Modification - JScott - Add Scaling Decimals
							//case eVariableStyle.Dollar:

							//    cellValue = (double)(decimal)System.Math.Round(_waferCell.Value / _dollarScaling, _numDecimals);
							//    return cellValue.ToString(_formatString, CultureInfo.CurrentUICulture);

							//case eVariableStyle.Units:

							//    cellValue = (double)(decimal)System.Math.Round(_waferCell.Value / _unitScaling, _numDecimals);
							//    return cellValue.ToString(_formatString, CultureInfo.CurrentUICulture);
							case eVariableStyle.Dollar:
							case eVariableStyle.Units:

								cellValue = (double)(decimal)System.Math.Round(_waferCell.Value / _scaling, _numDecimals);
								return cellValue.ToString(_formatString, CultureInfo.CurrentUICulture);
							//End Modification - JScott - Add Scaling Decimals

							default:

								return _waferCell.Value.ToString(_formatString, CultureInfo.CurrentUICulture);
						}
					}
					else
					{
						return "";
					}
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

		override public bool isValueNegative
		{
			get
			{
				return _waferCell.Value < 0;
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the value is numeric.
		/// </summary>

		override public bool isValueNumeric
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Gets an integer indicating the number of decimal digits.
		/// </summary>

		override public int NumberOfDecimals
		{
			get
			{
				return _numDecimals;
			}
		}

		//========
		// METHODS
		//========

		//Begin BonTon Calcs - JScott - Add Display Precision
		//Begin Modification - JScott - Add Scaling Decimals
		//private void Initialize(WaferCell aWaferCell, eVariableStyle aVarStyle, int aNumDecimals, int aUnitScaling, int aDollarScaling, bool aUseCommas)
		private void Initialize(WaferCell aWaferCell, eVariableStyle aVarStyle, int aNumDecimals, string aUnitScaling, string aDollarScaling, bool aUseCommas)
		//End Modification - JScott - Add Scaling Decimals
		{
			try
			{
				//Begin Modification - JScott - Add Scaling Decimals
				//_unitScaling = aUnitScaling;
				//_dollarScaling = aDollarScaling;

				//_varStyle = aVarStyle;
				//_numDecimals = aNumDecimals;
				_varStyle = aVarStyle;

				switch (_varStyle)
				{
					case eVariableStyle.Units:

						intParseScalingString(aUnitScaling, aNumDecimals, out _scaling, out _numDecimals);
						break;

					case eVariableStyle.Dollar:

						intParseScalingString(aDollarScaling, aNumDecimals, out _scaling, out _numDecimals);
						break;

					default:

						_scaling = 1;
						_numDecimals = aNumDecimals;
						break;
				}

				//End Modification - JScott - Add Scaling Decimals
				if (_numDecimals < Include.DecimalFormats.Length)
				{
					if (aUseCommas)
					{
						_formatString = Include.DecimalFormats[_numDecimals];
					}
					else
					{
						_formatString = Include.NoCommaDecimalFormats[_numDecimals];
					}
				}
				else
				{
					if (aUseCommas)
					{
						_formatString = "###,###,##0.";
					}
					else
					{
						_formatString = "########0.";
					}

					_formatString = _formatString.PadRight(_formatString.Length + _numDecimals, '0');
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

		override public WaferCellDetail Copy()
		{
			WaferCellDetail waferCellDetail;

			try
			{
				waferCellDetail = new GenericNumericWaferCellDetail();
				waferCellDetail.CopyFrom(this);

				return waferCellDetail;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Override.  Method that copies values from a WaferCell object to the current object.
		/// </summary>
        /// <param name="aWaferCellDetail">
		/// The WaferCell object to copy from.
		/// </param>

		override public void CopyFrom(WaferCellDetail aWaferCellDetail)
		{
			try
			{
				base.CopyFrom(aWaferCellDetail);
				_formatString = ((GenericNumericWaferCellDetail)aWaferCellDetail)._formatString;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Override.  Defines the method to clear the contents of the WaferCell.
		/// </summary>

		override public void Clear()
		{
			try
			{
				_formatString = null;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//Begin Modification - JScott - Add Scaling Decimals

		private void intParseScalingString(string aScalingString, int aVarDecimals, out int aScaling, out int aDecimals)
		{
			string[] scalingArray;

			try
			{
				if (aScalingString.Contains("."))
				{
					scalingArray = aScalingString.Split('.');
					aScaling = Convert.ToInt32(scalingArray[0]);
					aDecimals = Math.Max(aVarDecimals, scalingArray[1].Length);
				}
				else
				{
					aScaling = Convert.ToInt32(aScalingString);
					aDecimals = aVarDecimals;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End Modification - JScott - Add Scaling Decimals
	}

	/// <summary>
	/// The StoreGradeWaferCellDetail class defines a detail for a WaferCell.
	/// </summary>

	[Serializable]
	public class StoreGradeWaferCellDetail : WaferCellDetail
	{
		//=======
		// FIELDS
		//=======

		private string _valueAsString;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of StoreGradeWaferCellDetail
		/// </summary>

		public StoreGradeWaferCellDetail()
			: base()
		{
		}

		/// <summary>
		/// Creates a new instance of StoreGradeWaferCellDetail.
		/// </summary>

		public StoreGradeWaferCellDetail(WaferCell aWaferCell)
			: base(aWaferCell)
		{
			_valueAsString = "";
		}

		/// <summary>
		/// Creates a new instance of StoreGradeWaferCellDetail using the given parameters and value.
		/// </summary>

		public StoreGradeWaferCellDetail(WaferCell aWaferCell, ApplicationSessionTransaction aTransaction, int aNodeKey)
			: base(aWaferCell)
		{
			int hierarchyNodeKey;
			int storeGradeKey;
			StoreGradeList storeGradeList;
			StoreGradeProfile storeGradeProf;

			try
			{
				hierarchyNodeKey = aNodeKey;
				storeGradeList = aTransaction.GetStoreGradeList(hierarchyNodeKey);
				storeGradeKey = System.Convert.ToInt32(aWaferCell.Value, CultureInfo.CurrentUICulture);
				storeGradeProf = (StoreGradeProfile)storeGradeList.FindKey(storeGradeKey);

				if (storeGradeProf != null)
				{
					_valueAsString = storeGradeProf.StoreGrade;
				}
				else
				{
					_valueAsString = "N/A";
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a new instance of WaferCell and initializes it with a copy of the given WaferCell.
		/// </summary>
        /// <param name="aStoreGradeWaferCellDetail">
		/// The WaferCell to copy from.
		/// </param>

		protected StoreGradeWaferCellDetail(StoreGradeWaferCellDetail aStoreGradeWaferCellDetail)
			: base()
		{
			try
			{
				CopyFrom(aStoreGradeWaferCellDetail);
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
		/// Gets the string representation of the numeric cell value.
		/// </summary>

		override public string ValueAsString
		{
			get
			{
				return _valueAsString;
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the numeric value is negative.
		/// </summary>

		override public bool isValueNegative
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the value is numeric.
		/// </summary>

		override public bool isValueNumeric
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets an integer indicating the number of decimal digits.
		/// </summary>

		override public int NumberOfDecimals
		{
			get
			{
				return 0;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Override.  Method defining the Copy functionality.  Copy creates a deep copy. 
		/// </summary>
		/// <returns>
		/// Object reference to the copied object.
		/// </returns>

		override public WaferCellDetail Copy()
		{
			WaferCellDetail waferCellDetail;

			try
			{
				waferCellDetail = new StoreGradeWaferCellDetail();
				waferCellDetail.CopyFrom(this);

				return waferCellDetail;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Override.  Method that copies values from a WaferCell object to the current object.
		/// </summary>
        /// <param name="aWaferCellDetail">
		/// The WaferCell object to copy from.
		/// </param>

		override public void CopyFrom(WaferCellDetail aWaferCellDetail)
		{
			try
			{
				base.CopyFrom(aWaferCellDetail);
				_valueAsString = ((StoreGradeWaferCellDetail)aWaferCellDetail)._valueAsString;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Override.  Defines the method to clear the contents of the WaferCell.
		/// </summary>

		override public void Clear()
		{
			try
			{
				_valueAsString = "";
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// The StoreStatusWaferCellDetail class defines a detail for a WaferCell.
	/// </summary>

	[Serializable]
	public class StoreStatusWaferCellDetail : WaferCellDetail
	{
		//=======
		// FIELDS
		//=======

		private string _valueAsString;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of StoreStatusWaferCellDetail
		/// </summary>

		public StoreStatusWaferCellDetail()
			: base()
		{
		}

		///// <summary>
		///// Creates a new instance of StoreStatusWaferCellDetail.
		///// </summary>

		//public StoreStatusWaferCellDetail(WaferCell aWaferCell)
		//    : base(aWaferCell)
		//{
		//    _valueAsString = "";
		//}

		/// <summary>
		/// Creates a new instance of StoreStatusWaferCellDetail using the given parameters and value.
		/// </summary>

		public StoreStatusWaferCellDetail(WaferCell aWaferCell, int x)
			: base(aWaferCell)
		{
			eStoreStatus storeStatus;

			try
			{
				storeStatus = (eStoreStatus)aWaferCell.Value;

				switch (storeStatus)
				{
					case eStoreStatus.None:
						_valueAsString = "";
						break;

					case eStoreStatus.Comp:
						_valueAsString = MIDText.GetTextOnly((int)eStoreStatus.Comp);
						break;

					case eStoreStatus.NonComp:
						_valueAsString = MIDText.GetTextOnly((int)eStoreStatus.NonComp);
						break;

					case eStoreStatus.New:
						_valueAsString = MIDText.GetTextOnly((int)eStoreStatus.New);
						break;

					case eStoreStatus.Preopen:
						_valueAsString = MIDText.GetTextOnly((int)eStoreStatus.Preopen);
						break;

					case eStoreStatus.Closed:
						_valueAsString = MIDText.GetTextOnly((int)eStoreStatus.Closed);
						break;

					default:
						_valueAsString = "N/A";
						break;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a new instance of WaferCell and initializes it with a copy of the given WaferCell.
		/// </summary>
        /// <param name="aStoreStatusWaferCellDetail">
		/// The WaferCell to copy from.
		/// </param>

		protected StoreStatusWaferCellDetail(StoreStatusWaferCellDetail aStoreStatusWaferCellDetail)
			: base()
		{
			try
			{
				CopyFrom(aStoreStatusWaferCellDetail);
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
		/// Gets the string representation of the numeric cell value.
		/// </summary>

		override public string ValueAsString
		{
			get
			{
				return _valueAsString;
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the numeric value is negative.
		/// </summary>

		override public bool isValueNegative
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the value is numeric.
		/// </summary>

		override public bool isValueNumeric
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets an integer indicating the number of decimal digits.
		/// </summary>

		override public int NumberOfDecimals
		{
			get
			{
				return 0;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Override.  Method defining the Copy functionality.  Copy creates a deep copy. 
		/// </summary>
		/// <returns>
		/// Object reference to the copied object.
		/// </returns>

		override public WaferCellDetail Copy()
		{
			WaferCellDetail waferCellDetail;

			try
			{
				waferCellDetail = new StoreStatusWaferCellDetail();
				waferCellDetail.CopyFrom(this);

				return waferCellDetail;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Override.  Method that copies values from a WaferCell object to the current object.
		/// </summary>
        /// <param name="aWaferCellDetail">
		/// The WaferCell object to copy from.
		/// </param>

		override public void CopyFrom(WaferCellDetail aWaferCellDetail)
		{
			try
			{
				base.CopyFrom(aWaferCellDetail);
				_valueAsString = ((StoreStatusWaferCellDetail)aWaferCellDetail)._valueAsString;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Override.  Defines the method to clear the contents of the WaferCell.
		/// </summary>

		override public void Clear()
		{
			try
			{
				_valueAsString = "";
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// The WaferCell class defines the wafer cell that is used to transport data from the Application Session to the window.
	/// </summary>

	[Serializable]
	//Begin TT#2 - JScott - Assortment Planning - Phase 2
	//public class WaferCell
	abstract public class WaferCell
	//End TT#2 - JScott - Assortment Planning - Phase 2
	{
		//=======
		// FIELDS
		//=======

		private double _value;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of WaferCell.
		/// </summary>

		public WaferCell()
		{
			_value = 0;
		}

		/// <summary>
		/// Creates a new instance of WaferCell with the given value.
		/// </summary>

		public WaferCell(double aValue)
		{
			_value = aValue;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the numeric cell value.
		/// </summary>

		public double Value
		{
			get
			{
				return _value;
			}
		}

		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		/// <summary>
		/// Gets the PlanCellFlags structure containing the flags for the cell.
		/// </summary>

		abstract public ComputationCellFlags Flags { get; }

		/// <summary>
		/// Gets the string representation of the numeric cell value.
		/// </summary>

		abstract public string ValueAsString { get; }

		/// <summary>
		/// Gets a boolean indicating if the numeric value is negative.
		/// </summary>

		abstract public bool isValueNegative { get; }

		/// <summary>
		/// Gets a boolean indicating if the value is numeric.
		/// </summary>

		abstract public bool isValueNumeric { get; }

		/// <summary>
		/// Gets an integer indicating the number of decimal digits.
		/// </summary>

		abstract public int NumberOfDecimals { get; }

		//End TT#2 - JScott - Assortment Planning - Phase 2
		////========
		//// METHODS
		////========

		/// <summary>
		/// Override.  Method defining the Copy functionality.  Copy creates a deep copy. 
		/// </summary>
		/// <returns>
		/// Object reference to the copied object.
		/// </returns>

		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		//virtual public WaferCell Copy()
		//{
		//    WaferCell waferCell;

		//    try
		//    {
		//        waferCell = new WaferCell();
		//        waferCell.CopyFrom(this);

		//        return waferCell;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}
		abstract public WaferCell Copy();

		//End TT#2 - JScott - Assortment Planning - Phase 2
		/// <summary>
		/// Override.  Method that copies values from a PlanWaferCell object to the current object.
		/// </summary>
		/// <param name="aWaferCell">
		/// The WaferCell object to copy from.
		/// </param>

		virtual public void CopyFrom(WaferCell aWaferCell)
		{
			try
			{
				_value = aWaferCell._value;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Clears the value.
		/// </summary>
		
		virtual public void Clear()
		{
			try
			{
				_value = 0;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
