using System;
using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	/// <summary>
	/// IntransitKeyType encapsulates the description of keys for each of the four different types of intransit.
	/// </summary>
	/// <remarks>
	/// There are four different types of intransit:
	/// <list type="bullet">
	/// <item>Store Total Intransit:  this is the store total units intransit for a given merchandise level in a given time.</item>
	/// <item>Store Color Intransit:  this is the store color total units intransit for a given merchandise level in a given time.</item>
	/// <item>Store Size Intransit:  this is the store size total units intransit for a given merchandise level in a given time.</item>
	/// <item>Store Color-Size Intransit:  this is the store size within color units intransit for a given merchandise level in a given time.</item>
	/// </list></remarks>
	public struct IntransitKeyType
	{
		//========
		// FIELDS
		//========
		private int _colorRID;
		private int _sizeRID;
		private eIntransitBy _intransitBy;
 
		//==============
		// CONSTRUCTORS
		//==============
		public IntransitKeyType(int aColorRID, int aSizeRID)
		{
			_colorRID = aColorRID;
			_sizeRID = aSizeRID;
			if (_sizeRID == Include.IntransitKeyTypeNoSize)
			{
				if (_colorRID == Include.IntransitKeyTypeNoColor)
				{
					_intransitBy = eIntransitBy.Total;
				}
				else
				{
					_intransitBy = eIntransitBy.Color;
				}
			}
			else
			{
				if (_colorRID == Include.IntransitKeyTypeNoColor)
				{
					_intransitBy = eIntransitBy.Size;
				}
				else
				{
					_intransitBy = eIntransitBy.SizeWithinColors;
				}
			}
		}

		//============
		// PROPERTIES 
		//============
		public eIntransitBy IntransitType
		{
			get
			{
				return _intransitBy;
			}
		}
		public int ColorRID
		{
			get
			{
				return _colorRID;
			}
		}
		public int SizeRID
		{
			get
			{
				return _sizeRID;
			}
		}
		public long IntransitTypeKey
		{
			get
			{
				return ((((long)_colorRID)<<32) + (long)_sizeRID);
			}
		}
	}
	// MID track 4312 Size Intransit not relieved at style total
	// NOTE:  This structure was moved from AllocationProfile
	/// <summary>
	/// Structure used during update of intransit.
	/// </summary>
	public struct iktHashContent
	{
		private IntransitKeyType _ikt;
		private int _idx;
		private int _hierarchyRID;
		/// <summary>
		/// Creates an instance of this structure.
		/// </summary>
		/// <param name="aikt">IntransitKeyType containing the key and related information about this structure.</param>
		/// <param name="aIndex">Associated array index for this Intransit Key Type (position in the intransit array where this key is kept).</param>
		/// <param name="aHierarchyRID">HierarchyRID associated with this Intransit Key Type (where intransit is to be posted).</param>
		public iktHashContent(IntransitKeyType aikt, int aIndex, int aHierarchyRID)
		{
			_ikt = aikt;
			_idx  = aIndex;
			_hierarchyRID = aHierarchyRID;
		}
		/// <summary>
		/// Get Intransit Key Type (Style, color, color-size or size)
		/// </summary>
		public IntransitKeyType IKT
		{
			get { return _ikt; }
		}
		/// <summary>
		/// Get Array Position where Intransit Value for this Key Type resides
		/// </summary>
		public int IntransitArrayIndex
		{
			get { return _idx; }
		}
		/// <summary>
		/// Get Hierarchy RID where Intransit is to be posted
		/// </summary>
		public int PostIntransitToHierarchyRID
		{
			get { return _hierarchyRID; }
		}
	}
	// MID track 4312 Size Intransit not relieved at style total
}
