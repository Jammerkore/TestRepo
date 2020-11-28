using System;
using System.Collections;
using System.Collections.Generic;

using MIDRetail.DataCommon;
using MIDRetail.Business;

namespace MIDRetail.Common
{
	/// <summary>
	/// Contains the information about the store grades for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class StoreGradeProfile  : Profile
	{
		// Fields

		private eChangeType			_storeGradeChangeType;
		private bool				_storeGradeFound;
//		private string				_storeGradesInheritedFrom;
		private bool				_storeGradesIsInherited;
		private int					_storeGradesInheritedFromNodeRID;
		private string				_storeGrade;
		private int					_boundary;	// Index  TT#2 - stodd - assortment
        private int                 _originalBoundary;	// TT#2404 - JSmith - Questions on Store Grades in OTS Forecast Review
		private int					_wosIndex;
		private int					_boundaryUnits;	// TT#2 - stodd - assortment


		private eChangeType			_minMaxChangeType;
		private bool				_minMaxFound;
//		private string				_minMaxesInheritedFrom;
		private bool				_minMaxesIsInherited;
		private int					_minMaxesInheritedFromNodeRID;
		private int					_minStock;
		private int					_maxStock;
		private int					_minAd;
//		private int					_maxAd;
		private int					_minColor;
		private int					_maxColor;
        private int                 _shipUpTo;  // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StoreGradeProfile(int aKey)
			: base(aKey)
		{
			_storeGradeChangeType				= eChangeType.none;
			_storeGradesIsInherited				= false;
			_storeGradesInheritedFromNodeRID	= Include.NoRID;
			_minMaxChangeType					= eChangeType.none;
			_minMaxesIsInherited				= false;
			_minMaxesInheritedFromNodeRID		= Include.NoRID;
			_minStock							= -1;
			_maxStock							= -1;
			_minAd								= -1;
			_minColor							= -1;
			_maxColor							= -1;
            _shipUpTo                           = -1;  // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.StoreGrade;
			}
		}

		/// <summary>
		/// Gets or sets the change type of a store grade.
		/// </summary>
		public eChangeType StoreGradeChangeType 
		{
			get { return _storeGradeChangeType ; }
			set { _storeGradeChangeType = value; }
		}
		/// <summary>
		/// Gets or sets the code for the store grade.
		/// </summary>
		public string StoreGrade 
		{
			get { return _storeGrade ; }
			set { _storeGrade = value; }
		}
		/// <summary>
		/// Gets or sets the flag that the store grade was found.
		/// </summary>
		public bool StoreGradeFound 
		{
			get { return _storeGradeFound ; }
			set { _storeGradeFound = value; }
		}
//		/// <summary>
//		/// Gets or sets the node ID from where the store grades were inherited.
//		/// </summary>
//		public string StoreGradesInheritedFrom 
//		{
//			get { return _storeGradesInheritedFrom ; }
//			set { _storeGradesInheritedFrom = value; }
//		}
		/// <summary>
		/// Gets or sets a flag identifying if store grades information is inherited.
		/// </summary>
		public bool StoreGradesIsInherited 
		{
			get { return _storeGradesIsInherited ; }
			set { _storeGradesIsInherited = value; }
		}
		/// <summary>
		/// Gets or sets the record ID of the node where the store grades information is inherited from.
		/// </summary>
		public int StoreGradesInheritedFromNodeRID 
		{
			get { return _storeGradesInheritedFromNodeRID ; }
			set { _storeGradesInheritedFromNodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the boundary for the store grade.
		/// </summary>
		public int Boundary 
		{
			get { return _boundary ; }
			set { _boundary = value; }
		}
        // Begin TT#2404 - JSmith - Questions on Store Grades in OTS Forecast Review
        /// <summary>
        /// Gets or sets the original boundary for the store grade.
        /// </summary>
        public int OriginalBoundary
        {
            get { return _originalBoundary; }
            set { _originalBoundary = value; }
        }
        // End TT#2404 - JSmith - Questions on Store Grades in OTS Forecast Review
		// Begin TT#2 - stodd - assortment
		/// <summary>
		/// Gets or sets the boundary Units for the store grade.
		/// </summary>
		public int BoundaryUnits
		{
			get { return _boundaryUnits; }
			set { _boundaryUnits = value; }
		}
		// End TT#2 - stodd - assortment
		/// <summary>
		/// Gets or sets the WOS Index for the store grade.
		/// </summary>
		public int WosIndex 
		{
			get { return _wosIndex ; }
			set { _wosIndex = value; }
		}
		/// <summary>
		/// Gets or sets the change type of the min/maxes.
		/// </summary>
		public eChangeType MinMaxChangeType 
		{
			get { return _minMaxChangeType ; }
			set { _minMaxChangeType = value; }
		}
		/// <summary>
		/// Gets or sets the flag that min/maxes were found.
		/// </summary>
		public bool MinMaxFound 
		{
			get { return _minMaxFound ; }
			set { _minMaxFound = value; }
		}
//		/// <summary>
//		/// Gets or sets the node ID from where the min/maxes were inherited.
//		/// </summary>
//		public string MinMaxesInheritedFrom 
//		{
//			get { return _minMaxesInheritedFrom ; }
//			set { _minMaxesInheritedFrom = value; }
//		}
		/// <summary>
		/// Gets or sets a flag identifying if min/max information is inherited.
		/// </summary>
		public bool MinMaxesIsInherited 
		{
			get { return _minMaxesIsInherited ; }
			set { _minMaxesIsInherited = value; }
		}
		/// <summary>
		/// Gets or sets the record ID of the node where the min/max information is inherited from.
		/// </summary>
		public int MinMaxesInheritedFromNodeRID 
		{
			get { return _minMaxesInheritedFromNodeRID ; }
			set { _minMaxesInheritedFromNodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the Min Stock for the store grade.
		/// </summary>
		public int MinStock 
		{
			get { return _minStock ; }
			set { _minStock = value; }
		}
		/// <summary>
		/// Gets or sets the Max Stock for the store grade.
		/// </summary>
		public int MaxStock 
		{
			get { return _maxStock ; }
			set { _maxStock = value; }
		}
		/// <summary>
		/// Gets or sets the Min Ad for the store grade.
		/// </summary>
		public int MinAd 
		{
			get { return _minAd ; }
			set { _minAd = value; }
		}
//		/// <summary>
//		/// Gets or sets the Max Ad for the store grade.
//		/// </summary>
//		public int MaxAd 
//		{
//			get { return _maxAd ; }
//			set { _maxAd = value; }
//		}
		/// <summary>
		/// Gets or sets the Color Min for the store grade.
		/// </summary>
		public int MinColor 
		{
			get { return _minColor ; }
			set { _minColor = value; }
		}
		/// <summary>
		/// Gets or sets the Color Max for the store grade.
		/// </summary>
		public int MaxColor 
		{
			get { return _maxColor ; }
			set { _maxColor = value; }
		}
        // BEGIN TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
        public int ShipUpTo
        {
            get { return _shipUpTo; }
            set { _shipUpTo = value; }
        }
        // END TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
	}

	/// <summary>
	/// Used to retrieve a list of store grades
	/// </summary>
	[Serializable()]
	public class StoreGradeList : ProfileList
	{
		//private Hashtable _storeGradeByCodeHash = null; //TT#1343-MD -jsobek -Store Filter - "equals exactly" returns incorrect results 
        private List<StoreGradeProfile> _storeGradeByCodeList = null; //TT#1343-MD -jsobek -Store Filter - "equals exactly" returns incorrect results 
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StoreGradeList(eProfileType aProfileType)
			: base(aProfileType)
		{
			//
			// TODO: Add constructor logic here
			//
		}

        //Begin TT#1343-MD -jsobek -Store Filter - "equals exactly" returns incorrect results
		/// <summary>
		/// Returns the key of the StoreGradeProfile in the StoreGradeList for the given grade code
		/// </summary>
		/// <param name="aGradeCode">The grade code</param>
		/// <returns>The key of the grade code if found else returns -1</returns>
        //public List<StoreGradeProfile> GetStoreGradeList()     
        //{
        //    //Begin TT#1343-MD -jsobek -Store Filter - "equals exactly" returns incorrect results 
        //    //int storeGradeKey = Include.NoRID;

        //    //// build Hashtable on first access
        //    //if (_storeGradeByCodeHash == null)
        //    //{
        //    //    _storeGradeByCodeHash = new Hashtable();
        //    //    foreach (StoreGradeProfile sgp in this)
        //    //    {
        //    //        _storeGradeByCodeHash.Add(sgp.StoreGrade, sgp.Key);
        //    //    }
        //    //}

        //    //if (_storeGradeByCodeHash.Contains(aGradeCode))
        //    //{
        //    //    storeGradeKey = Convert.ToInt32(_storeGradeByCodeHash[aGradeCode]);
        //    //}

        //    //return _storeGradeByCodeHash;



        //    //return storeGradeKey;
        //    //End TT#1343-MD -jsobek -Store Filter - "equals exactly" returns incorrect results
        //    if (_storeGradeByCodeList == null)
        //    {
        //        _storeGradeByCodeList = new List<StoreGradeProfile>();
        //        foreach (StoreGradeProfile sgp in this)
        //        {
        //            _storeGradeByCodeList.Add(sgp);
        //        }
        //    }
        //    return _storeGradeByCodeList;
        //}

        //End TT#1343-MD -jsobek -Store Filter - "equals exactly" returns incorrect results

        //Begin TT#1343-MD -jsobek -Store Filter - "equals exactly" returns incorrect results - unused function
        //public void Reset()
        //{
        //    _storeGradeByCodeHash = null;
        //}
        //End TT#1343-MD -jsobek -Store Filter - "equals exactly" returns incorrect results - unused function
	}

	/// <summary>
	/// Contains the information about the stock min/maxes for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class NodeStockMinMaxesProfile  : Profile
	{
		// Fields

		private eChangeType				_nodeStockMinMaxChangeType;
		private bool					_nodeStockMinMaxFound;
		private bool					_nodeStockMinMaxsIsInherited;
		private int						_nodeStockMinMaxsInheritedFromNodeRID;
		private int						_nodeStockStoreGroupRID;
		private NodeStockMinMaxSetList	_nodeSetList;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeStockMinMaxesProfile(int aKey)
			: base(aKey)
		{
			_nodeStockMinMaxChangeType				= eChangeType.none;
			_nodeStockMinMaxFound					= false;
			_nodeStockMinMaxsIsInherited			= false;
			_nodeStockMinMaxsInheritedFromNodeRID	= Include.NoRID;
			_nodeStockStoreGroupRID					= Include.NoRID;
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.NodeStockMinMax;
			}
		}

		/// <summary>
		/// Gets or sets the change type of a store grade.
		/// </summary>
		public eChangeType NodeStockMinMaxChangeType 
		{
			get { return _nodeStockMinMaxChangeType ; }
			set { _nodeStockMinMaxChangeType = value; }
		}
		/// <summary>
		/// Gets or sets the flag that the store grade was found.
		/// </summary>
		public bool NodeStockMinMaxFound 
		{
			get { return _nodeStockMinMaxFound ; }
			set { _nodeStockMinMaxFound = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if store grades information is inherited.
		/// </summary>
		public bool NodeStockMinMaxsIsInherited 
		{
			get { return _nodeStockMinMaxsIsInherited ; }
			set { _nodeStockMinMaxsIsInherited = value; }
		}
		/// <summary>
		/// Gets or sets the record ID of the node where the store grades information is inherited from.
		/// </summary>
		public int NodeStockMinMaxsInheritedFromNodeRID 
		{
			get { return _nodeStockMinMaxsInheritedFromNodeRID ; }
			set { _nodeStockMinMaxsInheritedFromNodeRID = value; }
		}

		/// <summary>
		/// Gets or sets the record ID of the store group used to define the stock min/maxes.
		/// </summary>
		public int NodeStockStoreGroupRID 
		{
			get { return _nodeStockStoreGroupRID ; }
			set { _nodeStockStoreGroupRID = value; }
		}

		/// <summary>
		/// Gets or sets the NodeSetList.
		/// </summary>
		public NodeStockMinMaxSetList NodeSetList 
		{
			get 
			{ 
				if (_nodeSetList == null)
				{
					_nodeSetList = new NodeStockMinMaxSetList(eProfileType.NodeStockMinMaxSet);
				}
				return _nodeSetList ; 
			}
			set { _nodeSetList = value; }
		}
	}

	/// <summary>
	/// Contains the information about the stock min/maxes for a set
	/// </summary>
	[Serializable()]
	public class NodeStockMinMaxSetProfile  : Profile
	{
		// Fields

		private NodeStockMinMaxBoundaryProfile	_defaults;
		private NodeStockMinMaxBoundaryList		_boundaryList;
		

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeStockMinMaxSetProfile(int aKey)
			: base(aKey)
		{
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.NodeStockMinMaxSet;
			}
		}

		/// <summary>
		/// Gets or sets the default values for the set.
		/// </summary>
		public NodeStockMinMaxBoundaryProfile Defaults 
		{
			get 
			{ 
				if (_defaults == null)
				{
					_defaults = new NodeStockMinMaxBoundaryProfile(Include.Undefined);
				}
				return _defaults ; 
			}
			set { _defaults = value; }
		}
		/// <summary>
		/// Gets or sets the boundary list for the set.
		/// </summary>
		public NodeStockMinMaxBoundaryList BoundaryList 
		{
			get 
			{ 
				if (_boundaryList == null)
				{
					_boundaryList = new NodeStockMinMaxBoundaryList(eProfileType.NodeStockMinMaxBoundary);
				}
				return _boundaryList ; 
			}
			set { _boundaryList = value; }
		}		
	}

	/// <summary>
	/// Used to retrieve a list of store grades
	/// </summary>
	[Serializable()]
	public class NodeStockMinMaxSetList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeStockMinMaxSetList(eProfileType aProfileType)
			: base(aProfileType)
		{
			
		}
	}

	/// <summary>
	/// Contains the information about the stock min/maxes for a store grade
	/// </summary>
	/// <remarks>
	/// The Key is the boundary
	/// </remarks>
	[Serializable()]
	public class NodeStockMinMaxBoundaryProfile  : Profile
	{
		// Fields

		private string				_storeGrade;
		private NodeStockMinMaxList _minMaxList;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeStockMinMaxBoundaryProfile(int aKey)
			: base(aKey)
		{

		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.NodeStockMinMaxBoundary;
			}
		}
		/// <summary>
		/// Gets or sets the code for the store grade.
		/// </summary>
		public string StoreGrade 
		{
			get { return _storeGrade ; }
			set { _storeGrade = value; }
		}
		/// <summary>
		/// Gets or sets the list of min/maxes for the store grade.
		/// </summary>
		public NodeStockMinMaxList MinMaxList 
		{
			get 
			{ 
				if (_minMaxList == null)
				{
					_minMaxList = new NodeStockMinMaxList(eProfileType.NodeStockMinMaxBoundary);
				}
				return _minMaxList ; 
			}
			set { _minMaxList = value; }
		}
	}

	/// <summary>
	/// Used to retrieve a list of store grades
	/// </summary>
	[Serializable()]
	public class NodeStockMinMaxBoundaryList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeStockMinMaxBoundaryList(eProfileType aProfileType)
			: base(aProfileType)
		{
			
		}
	}

	/// <summary>
	/// Contains the information about the stock min/maxes for a node in a hierarchy
	/// </summary>
	/// <remarks>
	/// The Key is the date range key
	/// </remarks>
	[Serializable()]
	public class NodeStockMinMaxProfile  : Profile
	{
		// Fields

		private int					_minimum;
		private int					_maximum;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeStockMinMaxProfile(int aKey)
			: base(aKey)
		{
			_minimum = int.MinValue;
			_maximum = int.MaxValue;
		}

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeStockMinMaxProfile(int aKey, int aMinimum, int aMaximum)
			: base(aKey)
		{
			_minimum = aMinimum;
			_maximum = aMaximum;
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.NodeStockMinMax;
			}
		}

		/// <summary>
		/// Gets or sets the minimum value.
		/// </summary>
		public int Minimum 
		{
			get { return _minimum ; }
			set { _minimum = value; }
		}
		/// <summary>
		/// Gets or sets the maximum value.
		/// </summary>
		public int Maximum 
		{
			get { return _maximum ; }
			set { _maximum = value; }
		}
	}

	/// <summary>
	/// Used to retrieve a list of stock min/maxes
	/// </summary>
	[Serializable()]
	public class NodeStockMinMaxList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeStockMinMaxList(eProfileType aProfileType)
			: base(aProfileType)
		{
			
		}
	}
}
