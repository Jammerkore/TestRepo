using System;
using System.Collections;

namespace MIDRetail.Business.Allocation
{

	#region OrderBy -- generic sort class
	//
	// generic sort 
	//
	public class OrderBy 
	{
		int _item;
		double _firstOrderBy;
		double _secondOrderBy;
		double _thirdOrderBy;

		public OrderBy(int item, double firstOrderBy)
		{
			_item = item;
			_firstOrderBy = firstOrderBy;
			_secondOrderBy = 0.0;
			_thirdOrderBy = 0.0;
		}
		public OrderBy(int item, double firstOrderBy, double secondOrderBy)
		{
			_item = item;
			_firstOrderBy = firstOrderBy;
			_secondOrderBy = secondOrderBy;
			_thirdOrderBy = 0.0;
		}
		public OrderBy(int item, double firstOrderBy, double secondOrderBy, double thirdOrderBy)
		{
			_item = item;
			_firstOrderBy = firstOrderBy;
			_secondOrderBy = secondOrderBy;
			_thirdOrderBy = thirdOrderBy;
		}

		    
		public int CompareTo(OrderBy x)    
		{      
			int result = this._firstOrderBy.CompareTo(x._firstOrderBy);
			if (result != 0)
			{
				return result;
			}
			result = this._secondOrderBy.CompareTo(x._secondOrderBy);
			if (result != 0)
			{
				return result;
			}
			return (this._thirdOrderBy.CompareTo(x._thirdOrderBy));
		}
	}

	//    
	// Class compares for ascending sorts and binary searches    
	//    
	public class OrderByAscendingComparer:IComparer    
	{      
		public int Compare(object x, object y)      
		{        
			if (!(x is OrderBy) && !(y is OrderBy))        
			{          
				throw new ArgumentException("only allows OrderBy objects");        
			}        
			return ((OrderBy)x).CompareTo((OrderBy)y);      
		}    
	}
	public class OrderByDescendingComparer:IComparer    
	{      
		public int Compare(object x, object y)      
		{        
			if (!(x is OrderBy) && !(y is OrderBy))        
			{          
				throw new ArgumentException("only allows OrderBy objects");        
			}        
			return (-((OrderBy)x).CompareTo((OrderBy)y));      
		}    
	}

	#endregion
}
