using System;
using System.Collections;
using System.Globalization;

using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	
	/// <summary>
	/// This class houses the observation data for the 
	/// <see cref="AverageQuantity"/>,
	/// <see cref="Index"/>,
	/// <see cref="PercentToTotal"/> and
	/// <see cref="ProportionalSpread"/>
	/// calculations
	/// <seealso cref="Store"/>
	/// </summary>
	public class Summand
	{
		int _item;
		int _itemIdx;
		int _set;
		int _grade;
		double _quantity;
		double _min;
		double _max;
		double _random;
		double _result;
		bool _locked;
		bool _eligible;
		int _numberOfStores;
		//inventory
		double _weeksOfSupply;
		double _WOSIndex;
		double _totalSales;
		double _avgWeeklySales;
		double _weeksUsed;
		bool _salesPlanDepleted;
		double _inventory;
//		double _avgStoreQuantity;
		double _trend;
		double _applyToValue;
		// BEGIN MID Track #4370 - John Smith - FWOS Models
		bool _weeksOfSupplyWasOverridden;
		bool _maxStockWasApplied;
		// End MID Track #4370

		// constructor -- set up defaults
		public Summand()
		{
			_locked = false;
			_eligible = true;
			// BEGIN MID Track #4370 - John Smith - FWOS Models
			_weeksOfSupplyWasOverridden = false;
			_maxStockWasApplied = false;
			// End MID Track #4370
		}

		#region Properties
		public int Item
		{
			get { return _item; }
			set { _item = value; }
		}
		public int ItemIdx
		{
			get { return _itemIdx; }
			set { _itemIdx = value; }
		}
		public int Set
		{
			get { return _set; }
			set { _set = value; }
		}
		public int Grade
		{
			get { return _grade; }
			set { _grade = value; }
		}
		public double Quantity
		{
			get { return _quantity; }
			set { _quantity = value; }
		}
		public double Min
		{
			get { return _min; }
			set { _min = value; }
		}
		public double Max
		{
			get { return _max; }
			set { _max = value; }
		}
		public double Random
		{
			get { return _random; }
			set { _random = value; }
		}
		public double Result
		{
			get { return _result; }
			set { _result = value; }
		}
		public bool Locked
		{
			get { return _locked; }
			set { _locked = value; }
		}
		public bool Eligible
		{
			get { return _eligible; }
			set { _eligible = value; }
		}
		public int NumberOfStores
		{
			get { return _numberOfStores; }
			set { _numberOfStores = value; }
		}
		public double WeeksOfSupply
		{
			get { return _weeksOfSupply; }
			set { _weeksOfSupply = value; }
		}
		public double TotalSales
		{
			get { return _totalSales; }
			set { _totalSales = value; }
		}
		public double AvgWeeklySales
		{
			get { return _avgWeeklySales; }
			set { _avgWeeklySales = value; }
		}
		public double WeeksUsed
		{
			get { return _weeksUsed; }
			set { _weeksUsed = value; }
		}
		public double WOSIndex
		{
			get { return _WOSIndex; }
			set { _WOSIndex = value; }
		}
		public bool SalesPlanDepleted
		{
			get { return _salesPlanDepleted; }
			set { _salesPlanDepleted = value; }
		}
		public double Inventory
		{
			get { return _inventory; }
			set { _inventory = value; }
		}
		public double Trend
		{
			get { return _trend; }
			set { _trend = value; }
		}
		public double ApplyToValue
		{
			get { return _applyToValue; }
			set { _applyToValue = value; }
		}
//		public double AvgStoreQuantity
//		{
//			get { return _avgStoreQuantity; }
//			set { _avgStoreQuantity = value; }
//		}
		// BEGIN MID Track #4370 - John Smith - FWOS Models
		public bool WeeksOfSupplyWasOverridden
		{
			get { return _weeksOfSupplyWasOverridden; }
			set { _weeksOfSupplyWasOverridden = value; }
		}
		public bool MaxStockWasApplied
		{
			get { return _maxStockWasApplied; }
			set { _maxStockWasApplied = value; }
		}
		// End MID Track #4370
		#endregion

		/// <summary>
		/// CompareTo method for sorting
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public int CompareTo(Summand x)    
		{      
			int result = this._quantity.CompareTo(x._quantity);
			if (result != 0)
			{
				return result;
			}
			return (this._random.CompareTo(x._random));
		}

		/// <summary>
		/// Makes deep copy of Summand
		/// </summary>
		/// <returns></returns>
		public Summand Clone()
		{
			Summand S = new Summand();

			S._item = _item;
			S._itemIdx = _itemIdx;
			S._set = _set;
			S._grade = _grade;
			S._quantity = _quantity;
			S._min = _min;
			S._max = _max;
			S._random = _random;
			S._result = _result;
			S._locked = _locked;
			S._eligible = _eligible;
//			S._avgStoreQuantity = _avgStoreQuantity;

			return S;
		}

		public int GetSetGradeHash()
		{
			string setString = this.Set.ToString(CultureInfo.CurrentUICulture).PadLeft(5,'0');
			if (setString.Length > 5)
			{
				int startIndex = setString.Length - 5;
				setString = setString.Substring(startIndex, 5);
			}
			string gradeString = this.Grade.ToString(CultureInfo.CurrentUICulture).PadLeft(4,'0'); 
			if (gradeString.Length > 4)
			{
				int startIndex = gradeString.Length - 4;
				gradeString = gradeString.Substring(startIndex, 4);
			}
			string hashString = setString + gradeString;
			return Convert.ToInt32(hashString, CultureInfo.CurrentUICulture);
		}

	}


	/// <summary>
	/// Compare Classes for ascending and descending
	/// sorts and binary searches 
	/// </summary>
	public class SummandAscendingComparer:IComparer    
	{      
		public int Compare(object x, object y)      
		{        
			if (!(x is Summand) && !(y is Summand))        
			{          
				throw new ArgumentException("only allows Summand objects");        
			}        
			return ((Summand)x).CompareTo((Summand)y);      
		}    
	}
	public class SummandDescendingComparer:IComparer    
	{      
		public int Compare(object x, object y)      
		{        
			if (!(x is Summand) && !(y is Summand))        
			{          
				throw new ArgumentException("only allows Summand objects");        
			}        
			return (-((Summand)x).CompareTo((Summand)y));      
		}    
	}

	class SummandSort:IComparer
	{
		private eGroupLevelSmoothBy _compareType;
		public SummandSort(eGroupLevelSmoothBy sortOrder)
		{
			_compareType = sortOrder;
		}
		public int Compare(object x,object y)
		{
			int result = 0;
			switch(_compareType)
			{
				case eGroupLevelSmoothBy.StoreSet:
					return ((Summand)x).Set.CompareTo(((Summand)y).Set);
				case eGroupLevelSmoothBy.StoreGrade:
					return ((Summand)x).Grade.CompareTo(((Summand)y).Grade);
				case eGroupLevelSmoothBy.Both: 
					result = ((Summand)x).Set.CompareTo(((Summand)y).Set);
					if (result != 0)
					{
						return result;
					} 
					result = ((Summand)x).Grade.CompareTo(((Summand)y).Grade);
					return result;
				default:
					return ((Summand)x).Item.CompareTo(((Summand)y).Item);
			}
		}
	}
	
}




