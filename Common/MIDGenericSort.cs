using System;
using System.Collections;

namespace MIDRetail.Common
{

	#region MIDGeneric sort structure
	/// <summary>
	/// Describes the structure of a MIDGeneric sort item.
	/// </summary>
	/// <remarks>
	/// This structure is used to sort an array using any number of sort keys.  The structure fields are:
	/// <list type="bullet">
	/// <item>Item:  This integer is a reference to the array item to be sorted.</item>
	/// <item>SortKey:  This is an array of sort keys in order of importance (major key to minor key).</item>
	/// The following code illustrates how to use this structure to sort an array, X, whose entries contain Plan, Need, PercentNeed and UnitsAllocated:
	/// <example>
	/// <code>
	/// MIDGenericSortItem[] _nse = new MIDGenericSortItem[X.Rank];
	///	for (int i=0; i &lt; X.Rank; i++)
	///	{
	///		_nse[i].Item = i;
	///		_nse[i].SortKey = new double[2] ;
	///		_nse[i].SortKey[0] = X[i].PercentNeed;
	///		_nse[i].SortKey[1] = X[i].Need;
	///	}
	///	Array.Sort(_nse,new SortDescendingComparer());
	/// </code>
	/// </example>
	/// </list>
	/// </remarks>
	public struct MIDGenericSortItem 
	{
		int _item;
		double [] _sortKey;

		/// <summary>
		/// Reference to original item in the array to be sorted.
		/// </summary>
		public int Item
		{
			get
			{
				return _item;
			}
			set
			{
				_item = value;
			}
		}

		/// <summary>
		/// Array of sort keys in major to minor order.  
		/// </summary>
		/// <remarks>
		/// The first entry of this array must be the primary sort key. Each subsequent entry must be the next most important key of the remaining keys.
		/// </remarks>
		public double[] SortKey 
		{
			get
			{
				return _sortKey;
			}
			set
			{
				_sortKey = value;
			}

		}

		/// <summary>
		/// Compares two MIDGenericSortItems.
		/// </summary>
		/// <param name="x">MIDGenericSortItem to be compared to this MIDGenericSortItem.</param>
		/// <returns>
		/// <item>-1 when this MIDGenericSortItem is less than the compared to.</item>
		/// <item>0 when both objects are equal.</item>
		/// <item>1 when the compared to MIDGenericSortItem is greater than this MIDGenericSortItem</item>
		/// </returns>
		public int CompareTo(MIDGenericSortItem x)    
		{   
			int result = 0;
			if (this._sortKey.Length != x._sortKey.Length)
			{
				throw new ArgumentException("sort keys must have same rank");
			}
			for (int sk = 0; sk < this._sortKey.Length; sk++)
			{
				result = this._sortKey[sk].CompareTo(x._sortKey[sk]);
				if (result != 0)
				{
					break;
				}
			}
			return result;
		}
	}

	/// <summary>
	/// MIDGenericSortItem IComparer for ascending sorts and binary searches. 
	/// </summary>
    public class MIDGenericSortAscendingComparer : IComparer    // TT#1143 - MD - Jellis - Group Allocation Mins not working   
	{      
		public int Compare(object x, object y)      
		{        
			if (!(x is MIDGenericSortItem) || !(y is MIDGenericSortItem))        
			{          
				throw new ArgumentException("only allows MIDGenericSortItem objects");        
			}        
			return ((MIDGenericSortItem)x).CompareTo((MIDGenericSortItem)y);      
		}    
	}

	/// <summary>
	/// MIDGenericSortItem IComparer for descending sorts.
	/// </summary>
	public class MIDGenericSortDescendingComparer:IComparer    
	{      
		public int Compare(object x, object y)      
		{        
			if (!(x is MIDGenericSortItem) || !(y is MIDGenericSortItem))        
			{          
				throw new ArgumentException("only allows MIDGenericSortItem objects");        
			}        
			return (-((MIDGenericSortItem)x).CompareTo((MIDGenericSortItem)y));      
		}    
	}

	#endregion
}
