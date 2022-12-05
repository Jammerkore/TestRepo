using System;
using System.Collections;
using System.Collections.Generic;

//using MIDRetail.DataCommon;

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// Used to retrieve and update information about a hierarchy.
	/// </summary>
	[Serializable()]
	public class HierarchyProfile : Profile
	{
		private eChangeType		_hierarchyChangeType;
		private eLockStatus		_hierarchyLockStatus;
		private bool			_hierarchyChangeSuccessful;
		private int				_hierarchyRootNodeRID;
		private string			_hierarchyID;
		private string			_hierarchyColor;
		private eHierarchyType	_hierarchyType;
//Begin - JScott - Add Rollup Type to Hierarchy Properties - Part 2
		private eHierarchyRollupOption _hierarchyRollupOption;
//End - JScott - Add Rollup Type to Hierarchy Properties - Part 2
//Begin Track #3863 - JScott - OTS Forecast Level Defaults
		private eOTSPlanLevelType _planLevelType;
//End Track #3863 - JScott - OTS Forecast Level Defaults
//Begin Track #3948 - JSmith - add OTS Forecast Level interface
		private int				_OTSPlanLevelHierarchyLevelSequence;
//End Track #3948
		private Hashtable		_hierarchyLevels = new Hashtable();
		private int				_hierarchyDBLevelsCount = 0;
		private int				_owner = Include.GlobalUserRID;
		private DateTime		_postingDate;
		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		private string						_inUseUserID;
		//End Track #4815

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HierarchyProfile(int aKey)
			: base(aKey)
		{
			_hierarchyChangeType = eChangeType.none;
			_hierarchyLockStatus = eLockStatus.Undefined;
			_hierarchyChangeSuccessful = true;
//Begin Track #3948 - JSmith - add OTS Forecast Level interface
			_OTSPlanLevelHierarchyLevelSequence = Include.Undefined;
//End Track #3948
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Hierarchy;
			}
		}

		/// <summary>
		/// Gets or sets the change type for the hierarchy.
		/// </summary>
		public eChangeType HierarchyChangeType 
		{
			get { return _hierarchyChangeType ; }
			set { _hierarchyChangeType = value; }
		}

		/// <summary>
		/// Gets or sets the lock status for the hierarchy.
		/// </summary>
		/// <remarks>The status should be locked before updates are allowed</remarks>
		public eLockStatus	HierarchyLockStatus 
		{
			get { return _hierarchyLockStatus ; }
			set { _hierarchyLockStatus = value; }
		}

		/// <summary>
		/// Gets or sets the status of the hierarchy change.
		/// </summary>
		public bool HierarchyChangeSuccessful 
		{
			get { return _hierarchyChangeSuccessful ; }
			set { _hierarchyChangeSuccessful = value; }
		}

		/// <summary>
		/// Gets or sets the record id for the root node for the hierarchy.
		/// </summary>
		public int HierarchyRootNodeRID 
		{
			get { return _hierarchyRootNodeRID ; }
			set { _hierarchyRootNodeRID = value; }
		}

		/// <summary>
		/// Gets or sets the id of the hierarchy.
		/// </summary>
		public string HierarchyID 
		{
			get { return _hierarchyID ; }
			set { _hierarchyID = (value == null) ? value : value.Trim(); }
		}

		/// <summary>
		/// Gets or sets the color of the hierarchy.
		/// </summary>
		public string HierarchyColor 
		{
			get { return _hierarchyColor ; }
			set 
			{ 
				_hierarchyColor = (value == null) ? value : value.Trim(); 
				if (_hierarchyColor == null ||
					_hierarchyColor.Trim().Length == 0)
				{
					_hierarchyColor = Include.MIDDefaultColor;
				}
			}
		}

		/// <summary>
		/// Gets or sets the type of the hierarchy.
		/// </summary>
		public eHierarchyType HierarchyType 
		{
			get { return _hierarchyType ; }
			set { _hierarchyType = value; }
		}

//Begin - JScott - Add Rollup Type to Hierarchy Properties - Part 2
		/// <summary>
		/// Gets or sets the type of the hierarchy.
		/// </summary>
		public eHierarchyRollupOption HierarchyRollupOption 
		{
			get { return _hierarchyRollupOption ; }
			set { _hierarchyRollupOption = value; }
		}

//End - JScott - Add Rollup Type to Hierarchy Properties - Part 2
//Begin Track #3863 - JScott - OTS Forecast Level Defaults
		/// <summary>
		/// Gets or sets the type of the hierarchy.
		/// </summary>
		public eOTSPlanLevelType OTSPlanLevelType 
		{
			get { return _planLevelType ; }
			set { _planLevelType = value; }
		}

//End Track #3863 - JScott - OTS Forecast Level Defaults

//Begin Track #3948 - JSmith - add OTS Forecast Level interface
		/// <summary>
		/// Gets or sets whether to OTS Plan Level is overridden.
		/// </summary>
		public int OTSPlanLevelHierarchyLevelSequence
		{
			get { return _OTSPlanLevelHierarchyLevelSequence ; }
			set { _OTSPlanLevelHierarchyLevelSequence = value; }
		}
//End Track #3948

		/// <summary>
		/// Gets or sets the level of the hierarchy.
		/// </summary>
		public Hashtable HierarchyLevels
		{
			get { return _hierarchyLevels ; }
			set { _hierarchyLevels = value; }
		}

		/// <summary>
		/// Gets or sets the number of levels of the hierarchy found on the database.
		/// </summary>
		public int HierarchyDBLevelsCount
		{
			get { return _hierarchyDBLevelsCount ; }
			set { _hierarchyDBLevelsCount = value; }
		}

		/// <summary>
		/// Gets or sets the number of owner of the hierarchy.
		/// </summary>
		public int Owner
		{
			get { return _owner ; }
			set { _owner = value; }
		}

		/// <summary>
		/// Gets or sets the posting date of the hierarchy.
		/// </summary>
		public DateTime PostingDate
		{
			get { return _postingDate ; }
			set { _postingDate = value; }
		}

		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		/// <summary>
		/// Gets or sets the user the hierarchy is in use by.
		/// </summary>
		public string InUseUserID
		{
			get
			{
				return _inUseUserID ;
			}

			set { _inUseUserID = value; }
		}
		//End Track #4815
	}

	/// <summary>
	/// Used to retrieve a list of hierarchy profiles
	/// </summary>
	[Serializable()]
	public class HierarchyProfileList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HierarchyProfileList(eProfileType aProfileType)
			: base(aProfileType)
		{
			
		}
	}

	/// <summary>
	/// Contains the information about the levels of a hierarchy
	/// </summary>
	[Serializable()]
	public class HierarchyLevelProfile : Profile
	{
		// Fields
		private eChangeType					_levelChangeType;
		private bool						_levelChangeSuccessful;
		//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
		//private int							_levelNodeCount;
		private bool						_levelNodesExist;
		//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
		private int							_level;
		private string						_levelID;			
		private string						_levelColor;
		private eLevelLengthType			_levelLengthType;
		private int							_levelRequiredSize;
		private int							_levelSizeRangeFrom;
		private int							_levelSizeRangeTo;
		private eHierarchyLevelType			_levelType;
		private eOTSPlanLevelType			_levelOTSPlanLevelType;
		private eHierarchyDisplayOptions	_levelDisplayOption;
		private eHierarchyIDFormat			_levelIDFormat;
		private ePurgeTimeframe				_purgeDailyHistoryTimeframe;
		private int							_purgeDailyHistory;
		private ePurgeTimeframe				_purgeWeeklyHistoryTimeframe;
		private int							_purgeWeeklyHistory;
		private ePurgeTimeframe				_purgePlansTimeframe;
		private int							_purgePlans;
        // Begin TT#400-MD - JSmith - Add Header Purge criteria by Header Type
        //private ePurgeTimeframe				_purgeHeadersTimeframe;
        //private int							_purgeHeaders;
        // End TT#400-MD - JSmith - Add Header Purge criteria by Header Type
		private ePlanLevelLevelType			_OTSPlanLevelLevelType;
        //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
        private ePurgeTimeframe _purgeHtASNTimeframe;
        private ePurgeTimeframe _purgeHtDropShipTimeframe;
        private ePurgeTimeframe _purgeHtDummyTimeframe;
        private ePurgeTimeframe _purgeHtPurchaseOrderTimeframe;
        private ePurgeTimeframe _purgeHtReceiptTimeframe;
        private ePurgeTimeframe _purgeHtReserveTimeframe;
        private ePurgeTimeframe _purgeHtVSWTimeframe;
        private ePurgeTimeframe _purgeHtWorkUpTotTimeframe;
        private int _purgeHtASN;
        private int _purgeHtDropShip;
        private int _purgeHtDummy;
        private int _purgeHtPurchaseOrder;
        private int _purgeHtReceipt;
        private int _purgeHtReserve;
        private int _purgeHtVSW;
        private int _purgeHtWorkUpTot;
        //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HierarchyLevelProfile(int aKey)
			: base(aKey)
		{
			_levelChangeType = eChangeType.none;
			_levelChangeSuccessful = true;
			_levelColor = Include.MIDDefaultColor;
			_levelLengthType = 0;
			_levelRequiredSize = 0;
			_levelSizeRangeFrom = 0;
			_levelSizeRangeTo = 0;
			//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
			//_levelNodeCount = 0;
			_levelNodesExist = false;
			//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
			_levelType = eHierarchyLevelType.Undefined;
			_levelOTSPlanLevelType = eOTSPlanLevelType.Undefined;
			_levelDisplayOption = eHierarchyDisplayOptions.NameOnly;
			_levelIDFormat = eHierarchyIDFormat.Unique;
			_purgeDailyHistoryTimeframe = ePurgeTimeframe.None;
			_purgeDailyHistory = Include.Undefined;
			_purgeWeeklyHistoryTimeframe = ePurgeTimeframe.None;
			_purgeWeeklyHistory = Include.Undefined;
			_purgePlansTimeframe = ePurgeTimeframe.None;
			_purgePlans = Include.Undefined;
            // Begin TT#400-MD - JSmith - Add Header Purge criteria by Header Type
            //_purgeHeadersTimeframe = ePurgeTimeframe.None;
            //_purgeHeaders = Include.Undefined;
            // End TT#400-MD - JSmith - Add Header Purge criteria by Header Type
			_OTSPlanLevelLevelType = ePlanLevelLevelType.HierarchyLevel;
            //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
            _purgeHtASN = Include.Undefined;
            _purgeHtASNTimeframe = ePurgeTimeframe.None;
            _purgeHtDropShip = Include.Undefined;
            _purgeHtDropShipTimeframe = ePurgeTimeframe.None;
            _purgeHtDummy = Include.Undefined;
            _purgeHtDummyTimeframe = ePurgeTimeframe.None;
            _purgeHtPurchaseOrder = Include.Undefined;
            _purgeHtPurchaseOrderTimeframe = ePurgeTimeframe.None;
            _purgeHtReceipt = Include.Undefined;
            _purgeHtReceiptTimeframe = ePurgeTimeframe.None;
            _purgeHtReserve = Include.Undefined;
            _purgeHtReserveTimeframe = ePurgeTimeframe.None;
            _purgeHtVSW = Include.Undefined;
            _purgeHtVSWTimeframe = ePurgeTimeframe.None;
            _purgeHtWorkUpTot = Include.Undefined;
            _purgeHtWorkUpTotTimeframe = ePurgeTimeframe.None;
            //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
        }

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.HierarchyLevel;
			}
		}

		/// <summary>
		/// Gets or sets the change type for a level.
		/// </summary>
		public eChangeType LevelChangeType 
		{
			get { return _levelChangeType ; }
			set { _levelChangeType = value; }
		}
		/// <summary>
		/// Gets or sets the status of the change to the level.
		/// </summary>
		public bool LevelChangeSuccessful 
		{
			get { return _levelChangeSuccessful  ; }
			set { _levelChangeSuccessful  = value; }
		}
		/// <summary>
		/// Gets or sets number of nodes defined to this level in the hierarchy.
		/// </summary>
		/// <remarks>
		/// This property is used to determine if this level can be expanded on the hierarchy explorer.
		/// </remarks>
		//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
		//public int LevelNodeCount 
		//{
		//    get { return _levelNodeCount ; }
		//    set { _levelNodeCount = value; }
		//}
		public bool LevelNodesExist
		{
			get { return _levelNodesExist; }
			set { _levelNodesExist = value; }
		}
		//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
		/// <summary>
		/// Gets or sets the relative position of this level in the hierarchy.
		/// </summary>
		/// <remarks>
		/// Levels begin with 1.
		/// </remarks>
		public int Level 
		{
			get { return _level ; }
			set { _level = value; }
		}
		/// <summary>
		/// Gets or sets the ID (name) of the level in the hierarchy.
		/// </summary>
		public string LevelID 
		{
			get { return _levelID ; }
			set { _levelID = (value == null) ? value : value.Trim(); }
		}
		/// <summary>
		/// Gets or sets the color to use for the folder of the level in the hierarchy.
		/// </summary>
		public string LevelColor 
		{
			get { return _levelColor ; }
			set 
			{ 
				_levelColor = (value == null) ? value : value.Trim(); 
				if (_levelColor == null ||
					_levelColor.Trim().Length == 0)
				{
					_levelColor = Include.MIDDefaultColor;
				}
			}
		}
		/// <summary>
		/// Gets or sets the type of length (unrestricted, required, or range) for the level in the hierarchy.
		/// </summary>
		public eLevelLengthType LevelLengthType 
		{
			get { return _levelLengthType ; }
			set { _levelLengthType = value; }
		}
		/// <summary>
		/// Gets or sets the required size of this level in the hierarchy.
		/// </summary>
		/// <remarks>
		/// This field is only used if the LevelLengthType is set to required.
		/// </remarks>
		public int LevelRequiredSize 
		{
			get { return _levelRequiredSize ; }
			set { _levelRequiredSize = value; }
		}
		/// <summary>
		/// Gets or sets the from size of this level in the hierarchy.
		/// </summary>
		/// <remarks>
		/// This field is only used if the LevelLengthType is set to range.
		/// </remarks>
		public int LevelSizeRangeFrom 
		{
			get { return _levelSizeRangeFrom ; }
			set { _levelSizeRangeFrom = value; }
		}
		/// <summary>
		/// Gets or sets the to size of this level in the hierarchy.
		/// </summary>
		/// <remarks>
		/// This field is only used if the LevelLengthType is set to range.
		/// </remarks>
		public int LevelSizeRangeTo 
		{
			get { return _levelSizeRangeTo ; }
			set { _levelSizeRangeTo = value; }
		}
		/// <summary>
		/// Gets or sets the OTS level type of this level in the hierarchy.
		/// </summary>
		public eHierarchyLevelType LevelType 
		{
			get { return _levelType ; }
			set { _levelType = value; }
		}
		/// <summary>
		/// Gets or sets the OTS plan level type of this level in the hierarchy.
		/// </summary>
		/// <remarks>
		/// This field is only used if the LevelType is "Plan Level".</remarks>
		public eOTSPlanLevelType LevelOTSPlanLevelType
		{
			get { return _levelOTSPlanLevelType ; }
			set { _levelOTSPlanLevelType = value; }
		}
		/// <summary>
		/// Gets or sets the display option of this level in the hierarchy.
		/// </summary>
		public eHierarchyDisplayOptions LevelDisplayOption
		{
			get { return _levelDisplayOption ; }
			set { _levelDisplayOption = value; }
		}
		/// <summary>
		/// Gets or sets the ID format of this level in the hierarchy.
		/// </summary>
		public eHierarchyIDFormat LevelIDFormat
		{
			get { return _levelIDFormat ; }
			set { _levelIDFormat = value; }
		}
		/// <summary>
		/// Gets or sets the timeframe of the history purge information of this level in the hierarchy.
		/// </summary>
		public ePurgeTimeframe PurgeDailyHistoryTimeframe
		{
			get { return _purgeDailyHistoryTimeframe ; }
			set { _purgeDailyHistoryTimeframe = value; }
		}
		/// <summary>
		/// Gets or sets the time of the history purge information of this level in the hierarchy.
		/// </summary>
		public int PurgeDailyHistory
		{
			get { return _purgeDailyHistory ; }
			set { _purgeDailyHistory = value; }
		}
		/// <summary>
		/// Gets or sets the timeframe of the forecast purge information of this level in the hierarchy.
		/// </summary>
		public ePurgeTimeframe PurgeWeeklyHistoryTimeframe
		{
			get { return _purgeWeeklyHistoryTimeframe ; }
			set { _purgeWeeklyHistoryTimeframe = value; }
		}
		/// <summary>
		/// Gets or sets the time of the forecast purge information of this level in the hierarchy.
		/// </summary>
		public int PurgeWeeklyHistory
		{
			get { return _purgeWeeklyHistory ; }
			set { _purgeWeeklyHistory = value; }
		}
		/// <summary>
		/// Gets or sets the timeframe of the distro purge information of this level in the hierarchy.
		/// </summary>
		public ePurgeTimeframe PurgePlansTimeframe
		{
			get { return _purgePlansTimeframe ; }
			set { _purgePlansTimeframe = value; }
		}
		/// <summary>
		/// Gets or sets the time of the distro purge information of this level in the hierarchy.
		/// </summary>
		public int PurgePlans
		{
			get { return _purgePlans ; }
			set { _purgePlans = value; }
		}
        // Begin TT#400-MD - JSmith - Add Header Purge criteria by Header Type
        ///// <summary>
        ///// Gets or sets the timeframe of the header purge information of this level in the hierarchy.
        ///// </summary>
        //public ePurgeTimeframe PurgeHeadersTimeframe
        //{
        //    get { return _purgeHeadersTimeframe ; }
        //    set { _purgeHeadersTimeframe = value; }
        //}
        ///// <summary>
        ///// Gets or sets the time of the header purge information of this level in the hierarchy.
        ///// </summary>
        //public int PurgeHeaders
        //{
        //    get { return _purgeHeaders ; }
        //    set { _purgeHeaders = value; }
        //}
        // End TT#400-MD - JSmith - Add Header Purge criteria by Header Type
		/// <summary>
		/// Gets or sets the plan level level type of this level in the hierarchy.
		/// </summary>
		public ePlanLevelLevelType OTSPlanLevelLevelType
		{
			get { return _OTSPlanLevelLevelType ; }
			set { _OTSPlanLevelLevelType = value; }
		}
        //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
        /// <summary>
        /// Gets or sets the timeframe of the ASN header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe PurgeHtASNTimeframe
        {
            get { return _purgeHtASNTimeframe; }
            set { _purgeHtASNTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the ASN header type purge information of this level in the hierarchy.
        /// </summary>
        public int PurgeHtASN
        {
            get { return _purgeHtASN; }
            set { _purgeHtASN = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the DropShip header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe PurgeHtDropShipTimeframe
        {
            get { return _purgeHtDropShipTimeframe; }
            set { _purgeHtDropShipTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the DropShip header type purge information of this level in the hierarchy.
        /// </summary>
        public int PurgeHtDropShip
        {
            get { return _purgeHtDropShip; }
            set { _purgeHtDropShip = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the Dummy header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe PurgeHtDummyTimeframe
        {
            get { return _purgeHtDummyTimeframe; }
            set { _purgeHtDummyTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the Dummy header type purge information of this level in the hierarchy.
        /// </summary>
        public int PurgeHtDummy
        {
            get { return _purgeHtDummy; }
            set { _purgeHtDummy = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the PurchaseOrder header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe PurgeHtPurchaseOrderTimeframe
        {
            get { return _purgeHtPurchaseOrderTimeframe; }
            set { _purgeHtPurchaseOrderTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the PurchaseOrder header type purge information of this level in the hierarchy.
        /// </summary>
        public int PurgeHtPurchaseOrder
        {
            get { return _purgeHtPurchaseOrder; }
            set { _purgeHtPurchaseOrder = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the Receipt header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe PurgeHtReceiptTimeframe
        {
            get { return _purgeHtReceiptTimeframe; }
            set { _purgeHtReceiptTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the Receipt header type purge information of this level in the hierarchy.
        /// </summary>
        public int PurgeHtReceipt
        {
            get { return _purgeHtReceipt; }
            set { _purgeHtReceipt = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the Reserve header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe PurgeHtReserveTimeframe
        {
            get { return _purgeHtReserveTimeframe; }
            set { _purgeHtReserveTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the Reserve header type purge information of this level in the hierarchy.
        /// </summary>
        public int PurgeHtReserve
        {
            get { return _purgeHtReserve; }
            set { _purgeHtReserve = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the VSW header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe PurgeHtVSWTimeframe
        {
            get { return _purgeHtVSWTimeframe; }
            set { _purgeHtVSWTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the VSW header type purge information of this level in the hierarchy.
        /// </summary>
        public int PurgeHtVSW
        {
            get { return _purgeHtVSW; }
            set { _purgeHtVSW = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the WorkUpTot header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe PurgeHtWorkUpTotTimeframe
        {
            get { return _purgeHtWorkUpTotTimeframe; }
            set { _purgeHtWorkUpTotTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the WorkUpTot header type purge information of this level in the hierarchy.
        /// </summary>
        public int PurgeHtWorkUpTot
        {
            get { return _purgeHtWorkUpTot; }
            set { _purgeHtWorkUpTot = value; }
        }
        //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
    }


	/// <summary>
	/// Used to retrieve and update information from the hierarchy session about a node.
	/// </summary>
	[Serializable()]
	public class HierarchyNodeProfile : Profile, ICloneable
	{
		// Fields

		private eChangeType					_nodeChangeType;
		private eLockStatus					_nodeLockStatus;
		private bool						_nodeChangeSuccessful;
		private string						_message;
		private int							_hierarchyRID;
//		private int							_parentRID;
		private int							_homeHierarchyParentRID;
		private ArrayList					_parents;
		private string						_nodeID;
		private string						_qualifiedNodeID;
		private string						_nodeName;
		private string						_nodeDescription;
		private eHierarchyDisplayOptions	_displayOption;
		private string						_text;
		private string						_levelText;
		private int							_homeHierarchyRID;
		private int							_homeHierarchyLevel;
		private eHierarchyType				_homeHierarchyType;
        private int _homeHierarchyOwner;
//Begin - JScott - Add Rollup Type to Hierarchy Properties - Part 2
		private eHierarchyRollupOption		_rollupOption;
//End - JScott - Add Rollup Type to Hierarchy Properties - Part 2
//Begin Track #3863 - JScott - OTS Forecast Level Defaults
		private bool						_productTypeIsOverridden;
//End Track #3863 - JScott - OTS Forecast Level Defaults
		private eProductType				_productType;
		private eInheritedFrom				_productTypeInherited;
		private int							_productTypeInheritedFrom;
		private bool						_OTSPlanLevelIsOverridden;
//Begin Track #3863 - JScott - OTS Forecast Level Defaults
		private ePlanLevelSelectType		_OTSPlanLevelSelectType;
		private ePlanLevelLevelType			_OTSPlanLevelLevelType;
		private int							_OTSPlanLevelHierarchyRID;
		private int							_OTSPlanLevelHierarchyLevelSequence;
		private int							_OTSPlanLevelAnchorNode;
		private eMaskField					_OTSPlanLevelMaskField;
		private string						_OTSPlanLevelMask;
		private eInheritedFrom				_OTSPlanLevelInherited;
		private int							_OTSPlanLevelInheritedFrom;
		private bool						_OTSPlanLevelTypeIsOverridden;
//		private int							_OTSPlanLevelRID;
//End Track #3863 - JScott - OTS Forecast Level Defaults
		private eOTSPlanLevelType			_OTSPlanLevelType;
		private eInheritedFrom				_OTSPlanLevelTypeInherited;
		private int							_OTSPlanLevelTypeInheritedFrom;
//		private int							_dailyPercentagesModel;
		private eHierarchyLevelType			_levelType;
		private bool						_isParentOfStyle;
		private bool						_useBasicReplenishment;
		private int							_colorOrSizeCodeRID;
		private string						_nodeColor;
		private int							_nodeLevel;
		private bool						_hasChildren;
		private bool						_displayChildren;
		private eChangeType					_purgeCriteriaChangeType;
		private eInheritedFrom				_purgeDailyCriteriaInherited;
		private int							_purgeDailyCriteriaInheritedFrom;
		private int							_purgeDailyHistoryAfter;
		private eInheritedFrom				_purgeWeeklyCriteriaInherited;
		private int							_purgeWeeklyCriteriaInheritedFrom;
		private int							_purgeWeeklyHistoryAfter;
		private eInheritedFrom				_purgeOTSCriteriaInherited;
		private int							_purgeOTSCriteriaInheritedFrom;
		private int							_purgeOTSPlansAfter;
        /* BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type */
        //private eInheritedFrom				_purgeHeadersCriteriaInherited;
        //private int							_purgeHeadersCriteriaInheritedFrom;
        //private int							_purgeHeadersAfter;
        private eInheritedFrom _purgeHtASNCriteriaInherited;
        private eInheritedFrom _purgeHtDropShipCriteriaInherited;
        private eInheritedFrom _purgeHtDummyCriteriaInherited;
        private eInheritedFrom _purgeHtPurchaseOrderCriteriaInherited;
        private eInheritedFrom _purgeHtReceiptCriteriaInherited;
        private eInheritedFrom _purgeHtReserveCriteriaInherited;
        private eInheritedFrom _purgeHtVSWCriteriaInherited;
        private eInheritedFrom _purgeHtWorkUpTotCriteriaInherited;
        private int _purgeHtASNAfter;
        private int _purgeHtASNCriteriaInheritedFrom;
        private int _purgeHtDropShipAfter;
        private int _purgeHtDropShipCriteriaInheritedFrom;
        private int _purgeHtDummyAfter;
        private int _purgeHtDummyCriteriaInheritedFrom;
        private int _purgeHtPurchaseOrderAfter;
        private int _purgeHtPurchaseOrderCriteriaInheritedFrom;
        private int _purgeHtReceiptAfter;
        private int _purgeHtReceiptCriteriaInheritedFrom;
        private int _purgeHtReserveAfter;
        private int _purgeHtReserveCriteriaInheritedFrom;
        private int _purgeHtVSWAfter;
        private int _purgeHtVSWCriteriaInheritedFrom;
        private int _purgeHtWorkUpTotAfter;
        private int _purgeHtWorkUpTotCriteriaInheritedFrom;
        /* END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type */

		private int							_weightingFactor;
		private HierarchyNodeSecurityProfile	_chainSecurityProfile;
		private HierarchyNodeSecurityProfile	_storeSecurityProfile;
		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		private string						_inUseUserID;
		//End Track #4815
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
		private bool						_virtualInd;
//End Track #4037
        private ePurpose                    _purpose;
        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
        private bool _activeInd;
        // End TT#988
        //Begin TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
        private int                         _Begin_CDR_RID;
        //End TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
// BEGIN TT#1399
        private eInheritedFrom              _applyFromInherited = eInheritedFrom.Node;
        private int                         _applyFromInheritedFrom;
        private int                         _ApplyHNRIDFrom;
// END TT#1399
        private bool _commitOnSuccessfulUpdate;  // TT#3173 - JSmith - Severe Error during History Load
        private bool _deleteNode;	// TT#3630 - JSmith - Delete My Hierarchy
        private int _digitalAssetKey;

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public HierarchyNodeProfile(int aKey)
			: base(aKey)
		{
			_nodeChangeType = eChangeType.none;
			_nodeLockStatus = eLockStatus.Undefined;
			_purgeCriteriaChangeType = eChangeType.none;
			_productTypeInherited = eInheritedFrom.None;
			_OTSPlanLevelTypeInherited = eInheritedFrom.None;
			_purgeDailyCriteriaInherited = eInheritedFrom.None;
			_purgeWeeklyCriteriaInherited = eInheritedFrom.None;
			_purgeOTSCriteriaInherited = eInheritedFrom.None;
            //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
            //_purgeHeadersCriteriaInherited = eInheritedFrom.None;
            _purgeHtASNCriteriaInherited = eInheritedFrom.None;
            _purgeHtDropShipCriteriaInherited = eInheritedFrom.None;
            _purgeHtDummyCriteriaInherited = eInheritedFrom.None;
            _purgeHtPurchaseOrderCriteriaInherited = eInheritedFrom.None;
            _purgeHtReceiptCriteriaInherited = eInheritedFrom.None;
            _purgeHtReserveCriteriaInherited = eInheritedFrom.None;
            _purgeHtVSWCriteriaInherited = eInheritedFrom.None;
            _purgeHtWorkUpTotCriteriaInherited = eInheritedFrom.None;
            //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
			_hierarchyRID = Include.NoRID;
			_homeHierarchyRID = Include.NoRID; // MID Track #4389 - Justiin Bolles - Node Copy Error
			_homeHierarchyParentRID = Include.NoRID;
			_parents = new ArrayList();
			_levelType = eHierarchyLevelType.Undefined;
//Begin Track #3863 - JScott - OTS Forecast Level Defaults
			_productTypeIsOverridden = false;
//			_OTSPlanLevelRID = Include.NoRID;
//End Track #3863 - JScott - OTS Forecast Level Defaults
			_productType = eProductType.Undefined;
			_isParentOfStyle = false;
			_useBasicReplenishment = false;
			_OTSPlanLevelIsOverridden = false;
//Begin Track #3863 - JScott - OTS Forecast Level Defaults
//			_OTSPlanLevelType = eOTSPlanLevelType.Total;
			_OTSPlanLevelSelectType = ePlanLevelSelectType.Undefined;
			_OTSPlanLevelLevelType  = ePlanLevelLevelType.Undefined;
			_OTSPlanLevelHierarchyRID = Include.NoRID;
			_OTSPlanLevelHierarchyLevelSequence = Include.Undefined;
			_OTSPlanLevelAnchorNode = Include.Undefined;
			_OTSPlanLevelMaskField = eMaskField.Undefined;
			_OTSPlanLevelMask = null;
			_OTSPlanLevelTypeIsOverridden = false;
			_OTSPlanLevelType = eOTSPlanLevelType.Undefined;
//End Track #3863 - JScott - OTS Forecast Level Defaults
//			_dailyPercentagesModel = -1;
			_displayOption = eHierarchyDisplayOptions.NameOnly;
			_nodeChangeSuccessful = true;
			_weightingFactor = 1;
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
			_virtualInd = false;
//End Track #4037
//Begin TT #1399
//  set it to -1 to signify that it is not set
            _applyFromInheritedFrom = Include.NoRID;
            _ApplyHNRIDFrom = Include.NoRID;
//End TT#1399
            _purpose = ePurpose.Default;
            // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
            _activeInd = true;
            // End TT#988
			//Begin TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
            _Begin_CDR_RID = Include.NoRID;
           //End TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
           // Begin TT#1577 - JSmith - The Purge Criteria is set to zero when a new Alternate Vendor is created manually and by an API transaction.
           _purgeDailyHistoryAfter = Include.Undefined;
           _purgeWeeklyHistoryAfter = Include.Undefined;
           _purgeOTSPlansAfter = Include.Undefined;
           //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
           //_purgeHeadersAfter = Include.Undefined;
            // End TT#1577
           _purgeHtASNAfter = Include.Undefined;
           _purgeHtDropShipAfter = Include.Undefined;
           _purgeHtDummyAfter = Include.Undefined;
           _purgeHtReceiptAfter = Include.Undefined;
           _purgeHtPurchaseOrderAfter = Include.Undefined;
           _purgeHtReserveAfter = Include.Undefined;
           _purgeHtVSWAfter = Include.Undefined;
           _purgeHtWorkUpTotAfter = Include.Undefined;
            //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
			 _commitOnSuccessfulUpdate = false;  // TT#3173 - JSmith - Severe Error during History Load
             _deleteNode = false;  // TT#3630 - JSmith - Delete My Hierarchy
            _digitalAssetKey = Include.Undefined;

        }

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.HierarchyNode;
			}
		}

        // Begin TT#1399
        /// <summary>
        /// Tests whether this is a shortcut
        /// </summary>
        /// <remarks>
        /// if homehierarchyrid != hierarchyrid then it is a shortcut
        /// </remarks>
        public bool isShortcut
        {
            get { return (_homeHierarchyRID != _hierarchyRID); }
        }
        public int ApplyFromInheritedFrom
        {
            get { return _applyFromInheritedFrom; }
            set { _applyFromInheritedFrom = value; }
        }
        public eInheritedFrom ApplyFromInherited
        {
            get { return _applyFromInherited; }
            set { _applyFromInherited = value; }
        }
        public int ApplyHNRIDFrom
        {
            get { return _ApplyHNRIDFrom; }
            set { _ApplyHNRIDFrom = value; }
        }

        /// <summary>
        /// Copies this Hierarchy to another provided
        /// </summary>
        public void CopyHierarchy(HierarchyNodeProfile _destinationhnp)
        {
            this.ApplyHNRIDFrom = _destinationhnp.ApplyHNRIDFrom;
            if (this.OTSPlanLevelInherited != _destinationhnp.OTSPlanLevelInherited)
            {
                this.OTSPlanLevelInheritedFrom = _destinationhnp.HierarchyRID;
                this.OTSPlanLevelInherited = _destinationhnp.OTSPlanLevelInherited;
                this.OTSPlanLevelHierarchyLevelSequence = _destinationhnp.OTSPlanLevelHierarchyLevelSequence;
                this.OTSPlanLevelLevelType = _destinationhnp.OTSPlanLevelLevelType;
                this.OTSPlanLevelMask = _destinationhnp.OTSPlanLevelMask;
                this.OTSPlanLevelMaskField = _destinationhnp.OTSPlanLevelMaskField;
                this.OTSPlanLevelSelectType = _destinationhnp.OTSPlanLevelSelectType;
                this.OTSPlanLevelType = _destinationhnp.OTSPlanLevelType;
                this.OTSPlanLevelTypeIsOverridden = _destinationhnp.OTSPlanLevelTypeIsOverridden;
            }
            if (this.ProductTypeInherited != _destinationhnp.ProductTypeInherited)
            {
                this.ProductTypeInheritedFrom = _destinationhnp.HierarchyRID;
                this.ProductTypeInherited = _destinationhnp.ProductTypeInherited;
                this.ProductType = _destinationhnp.ProductType;
                this.ProductTypeIsOverridden = _destinationhnp.ProductTypeIsOverridden;
            }
            if (this.PurgeDailyCriteriaInherited != _destinationhnp.PurgeDailyCriteriaInherited)
            {
                this.PurgeDailyCriteriaInheritedFrom = _destinationhnp.HierarchyRID;
                this.PurgeCriteriaChangeType = _destinationhnp.PurgeCriteriaChangeType;
                this.PurgeDailyCriteriaInherited = _destinationhnp.PurgeDailyCriteriaInherited;
            }
            if (this.PurgeOTSCriteriaInherited != _destinationhnp.PurgeOTSCriteriaInherited)
            {
                this.PurgeOTSCriteriaInheritedFrom = _destinationhnp.HierarchyRID;
                this.PurgeOTSCriteriaInherited = _destinationhnp.PurgeOTSCriteriaInherited;
            }
            if (this.PurgeWeeklyCriteriaInherited != _destinationhnp.PurgeWeeklyCriteriaInherited)
            {
                this.PurgeWeeklyCriteriaInheritedFrom = _destinationhnp.PurgeWeeklyCriteriaInheritedFrom;
                this.PurgeWeeklyCriteriaInherited = _destinationhnp.PurgeWeeklyCriteriaInherited;
            }
        }
        // End TT#1399

		/// <summary>
		/// Gets or sets the change type for the node.
		/// </summary>
		public eChangeType NodeChangeType 
		{
			get { return _nodeChangeType ; }
			set { _nodeChangeType = value; }
		}

		/// <summary>
		/// Gets or sets the lock status for the node.
		/// </summary>
		/// <remarks>The status should be locked before updates are allowed</remarks>
		public eLockStatus	NodeLockStatus 
		{
			get { return _nodeLockStatus ; }
			set { _nodeLockStatus = value; }
		}

		/// <summary>
		/// Gets or sets the status of the change to the node.
		/// </summary>
		public bool NodeChangeSuccessful 
		{
			get { return _nodeChangeSuccessful ; }
			set { _nodeChangeSuccessful = value; }
		}

		/// <summary>
		/// Gets or sets the message of the change to the node.
		/// </summary>
		public string Message 
		{
			get { return _message ; }
			set { _message = (value == null) ? value : value.Trim(); }
		}

		/// <summary>
		/// Gets or sets the record id of the hierarchy where the node is located.
		/// </summary>
		public int HierarchyRID 
		{
			get { return _hierarchyRID ; }
			set { _hierarchyRID = value; }
		}

		/// <summary>
		/// Gets or sets the record id of the parent node in the home hierarchy of the node.
		/// </summary>
		public int HomeHierarchyParentRID 
		{
			get { return _homeHierarchyParentRID ; }
			set { _homeHierarchyParentRID = value; }
		}
		/// <summary>
		/// Gets or sets the record id(s) of the parent node(s) in the hierarchy.
		/// </summary>
		public ArrayList Parents 
		{
			get { return _parents ; }
			set { _parents = value; }
		}

		/// <summary>
		/// Gets or sets the id of the node.
		/// </summary>
		public string NodeID 
		{
			get { return _nodeID ; }
			set { _nodeID = (value == null) ? value : value.Trim(); }
		}

		/// <summary>
		/// Gets or sets the fully qualified id of the node.
		/// </summary>
		/// <remarks>
		/// This field is only pertinent for color and size nodes.
		/// </remarks>
		public string QualifiedNodeID 
		{
			get { return _qualifiedNodeID ; }
			set { _qualifiedNodeID = (value == null) ? value : value.Trim(); }
		}

		/// <summary>
		/// Gets or sets the name of the node.
		/// </summary>
		public string NodeName 
		{
			get { return _nodeName ; }
			set { _nodeName = (value == null) ? value : value.Trim(); }
		}

		/// <summary>
		/// Gets or sets the description of the node.
		/// </summary>
		public string NodeDescription 
		{
			get { return _nodeDescription ; }
			set { _nodeDescription = (value == null) ? value : value.Trim(); }
		}

		/// <summary>
		/// Gets or sets the display option of the node.
		/// </summary>
		public eHierarchyDisplayOptions DisplayOption 
		{
			get { return _displayOption ; }
			set { _displayOption = value; }
		}

		/// <summary>
		/// Gets or sets the display text for the node.
		/// </summary>
		/// <remarks>
		/// This is formatted based on the display option for the level and includes 
		/// ancestor levels when necessary to fully identify the node.
		/// </remarks>
		public string Text 
		{
			get { return _text ; }
			set { _text = (value == null) ? value : value.Trim(); }
		}

		/// <summary>
		/// Gets or sets the display text for the node of the requested level only.
		/// </summary>
		/// <remarks>
		/// This is formatted based on the display option for the level.
		/// </remarks>
		public string LevelText 
		{
			get { return _levelText ; }
			set { _levelText = (value == null) ? value : value.Trim(); }
		}

		/// <summary>
		/// Gets or sets the record id of the home hierarchy for the node.
		/// </summary>
		public int HomeHierarchyRID
		{
			get { return _homeHierarchyRID ; }
			set { _homeHierarchyRID = value; }
		}

		/// <summary>
		/// Gets or sets the level in the home hierarchy what the node is located.
		/// </summary>
		public int HomeHierarchyLevel
		{
			get { return _homeHierarchyLevel ; }
			set { _homeHierarchyLevel = value; }
		}

		/// <summary>
		/// Gets or sets the type of hierarchy where the node is located.
		/// </summary>
		public eHierarchyType HomeHierarchyType 
		{
			get { return _homeHierarchyType ; }
			set { _homeHierarchyType = value; }
		}

        /// <summary>
		/// Gets or sets the owner of hierarchy where the node is located.
		/// </summary>
        public int HomeHierarchyOwner 
		{
            get { return _homeHierarchyOwner; }
            set { _homeHierarchyOwner = value; }
		}

//Begin - JScott - Add Rollup Type to Hierarchy Properties - Part 2
		/// <summary>
		/// Gets or sets the type of hierarchy where the node is located.
		/// </summary>
		public eHierarchyRollupOption RollupOption 
		{
			get { return _rollupOption ; }
			set { _rollupOption = value; }
		}

//End - JScott - Add Rollup Type to Hierarchy Properties - Part 2
//Begin Track #3863 - JScott - OTS Forecast Level Defaults
		/// <summary>
		/// Gets or sets whether to OTS Plan Level is overridden.
		/// </summary>
		public bool ProductTypeIsOverridden
		{
			get { return _productTypeIsOverridden ; }
			set { _productTypeIsOverridden = value; }
		}

//End Track #3863 - JScott - OTS Forecast Level Defaults
		/// <summary>
		/// Gets or sets the product type of the node.
		/// </summary>
		public eProductType ProductType
		{
			get { return _productType ; }
			set { _productType = value; }
		}

		/// <summary>
		/// Gets or sets the whether the product type was inherited.
		/// </summary>
		public eInheritedFrom ProductTypeInherited 
		{
			get { return _productTypeInherited ; }
			set { _productTypeInherited = value; }
		}

		/// <summary>
		/// Gets or sets the node RID or level index where the product type was inherited from.
		/// </summary>
		public int ProductTypeInheritedFrom 
		{
			get { return _productTypeInheritedFrom ; }
			set { _productTypeInheritedFrom = value; }
		}

		/// <summary>
		/// Gets or sets whether to OTS Plan Level is overridden.
		/// </summary>
		public bool OTSPlanLevelIsOverridden
		{
			get { return _OTSPlanLevelIsOverridden ; }
			set { _OTSPlanLevelIsOverridden = value; }
		}

		/// <summary>
		/// Gets or sets OTS Plan Level level select type.
		/// </summary>
		public ePlanLevelSelectType OTSPlanLevelSelectType
		{
			get { return _OTSPlanLevelSelectType ; }
			set { _OTSPlanLevelSelectType = value; }
		}

		/// <summary>
		/// Gets or sets OTS Plan Level level type.
		/// </summary>
		public ePlanLevelLevelType OTSPlanLevelLevelType
		{
			get { return _OTSPlanLevelLevelType ; }
			set { _OTSPlanLevelLevelType = value; }
		}

//Begin Track #3863 - JScott - OTS Forecast Level Defaults
		/// <summary>
		/// Gets or sets OTS Plan Level hierarchy.
		/// </summary>
		public int OTSPlanLevelHierarchyRID
		{
			get { return _OTSPlanLevelHierarchyRID ; }
			set { _OTSPlanLevelHierarchyRID = value; }
		}

		/// <summary>
		/// Gets or sets the OTS Plan Level level sequence.
		/// </summary>
		public int OTSPlanLevelHierarchyLevelSequence
		{
			get { return _OTSPlanLevelHierarchyLevelSequence ; }
			set { _OTSPlanLevelHierarchyLevelSequence = value; }
		}

		/// <summary>
		/// Gets or sets the OTS Plan Level anchor node.
		/// </summary>
		public int OTSPlanLevelAnchorNode
		{
			get { return _OTSPlanLevelAnchorNode ; }
			set { _OTSPlanLevelAnchorNode = value; }
		}

		/// <summary>
		/// Gets or sets the OTS Plan Level mask field.
		/// </summary>
		public eMaskField OTSPlanLevelMaskField
		{
			get { return _OTSPlanLevelMaskField ; }
			set { _OTSPlanLevelMaskField = value; }
		}

		/// <summary>
		/// Gets or sets the OTS Plan Level mask.
		/// </summary>
		public string OTSPlanLevelMask
		{
			get { return _OTSPlanLevelMask ; }
			set { _OTSPlanLevelMask = value; }
		}

//		/// <summary>
//		/// Gets or sets the OTS Plan Level RID.
//		/// </summary>
//		public int OTSPlanLevelRID
//		{
//			get { return _OTSPlanLevelRID ; }
//			set { _OTSPlanLevelRID = value; }
//		}

		/// <summary>
		/// Gets or sets the whether the OTS plan level was inherited.
		/// </summary>
		public eInheritedFrom OTSPlanLevelInherited 
		{
			get { return _OTSPlanLevelInherited ; }
			set { _OTSPlanLevelInherited = value; }
		}

		/// <summary>
		/// Gets or sets the node RID or level index where the OTS plan level  was inherited from.
		/// </summary>
		public int OTSPlanLevelInheritedFrom 
		{
			get { return _OTSPlanLevelInheritedFrom ; }
			set { _OTSPlanLevelInheritedFrom = value; }
		}

		/// <summary>
		/// Gets or sets whether to OTS Plan Level is overridden.
		/// </summary>
		public bool OTSPlanLevelTypeIsOverridden
		{
			get { return _OTSPlanLevelTypeIsOverridden ; }
			set { _OTSPlanLevelTypeIsOverridden = value; }
		}

//End Track #3863 - JScott - OTS Forecast Level Defaults
		/// <summary>
		/// Gets or sets the type of override for the OTS Plan Level.
		/// </summary>
		public eOTSPlanLevelType OTSPlanLevelType
		{
			get { return _OTSPlanLevelType ; }
			set { _OTSPlanLevelType = value; }
		}

		/// <summary>
		/// Gets or sets the whether the OTS plan level was inherited.
		/// </summary>
		public eInheritedFrom OTSPlanLevelTypeInherited 
		{
			get { return _OTSPlanLevelTypeInherited ; }
			set { _OTSPlanLevelTypeInherited = value; }
		}

		/// <summary>
		/// Gets or sets the node RID or level index where the OTS plan level type was inherited from.
		/// </summary>
		public int OTSPlanLevelTypeInheritedFrom 
		{
			get { return _OTSPlanLevelTypeInheritedFrom ; }
			set { _OTSPlanLevelTypeInheritedFrom = value; }
		}

//		/// <summary>
//		/// Gets or sets the model used for daily percentages.
//		/// </summary>
//		public int DailyPercentagesModel
//		{
//			get { return _dailyPercentagesModel ; }
//			set { _dailyPercentagesModel = value; }
//		}

		/// <summary>
		/// Gets or sets the level type for this product.
		/// </summary>
		public eHierarchyLevelType LevelType
		{
			get { return _levelType ; }
			set { _levelType = value; }
		}

		/// <summary>
		/// Gets or sets the flag identifying if this node is a parent to the style level.
		/// </summary>
		public bool IsParentOfStyle 
		{
			get { return _isParentOfStyle ; }
			set { _isParentOfStyle = value; }
		}

		/// <summary>
		/// Gets or sets the flag identifying if this node is to use basic replenishment.
		/// </summary>
		public bool UseBasicReplenishment 
		{
			get { return _useBasicReplenishment ; }
			set { _useBasicReplenishment = value; }
		}

		/// <summary>
		/// Gets or sets the record id of the color or size code.
		/// </summary>
		public int ColorOrSizeCodeRID 
		{
			get { return _colorOrSizeCodeRID ; }
			set { _colorOrSizeCodeRID = value; }
		}

		/// <summary>
		/// Gets or sets the color of the child as it relates to the level in its home hierarchy.
		/// </summary>
		public string NodeColor 
		{
			get { return _nodeColor ; }
			set 
			{ 
				_nodeColor = (value == null) ? value : value.Trim(); 
				if (_nodeColor == null ||
					_nodeColor.Trim().Length == 0)
				{
					_nodeColor = Include.MIDDefaultColor;
				}
			}
		}

		/// <summary>
		/// Gets or sets the relative level of the child in the hierarchy.
		/// </summary>
		public int NodeLevel 
		{
			get { return _nodeLevel ; }
			set { _nodeLevel = value; }
		}

		/// <summary>
		/// Gets or sets the flag to identify if the node has children.
		/// </summary>
		public bool HasChildren
		{
			get { return _hasChildren ; }
			set { _hasChildren = value; }
		}

		/// <summary>
		/// Gets or sets the flag to identify if the children of the node are to be displayed.
		/// </summary>
		public bool DisplayChildren
		{
			get { return _displayChildren ; }
			set { _displayChildren = value; }
		}

		/// <summary>
		/// Gets or sets the change type for the purge criteria.
		/// </summary>
		public eChangeType PurgeCriteriaChangeType 
		{
			get { return _purgeCriteriaChangeType ; }
			set { _purgeCriteriaChangeType = value; }
		}

		/// <summary>
		/// Gets or sets the whether the daily purge criteria was inherited.
		/// </summary>
		public eInheritedFrom PurgeDailyCriteriaInherited 
		{
			get { return _purgeDailyCriteriaInherited ; }
			set { _purgeDailyCriteriaInherited = value; }
		}

		/// <summary>
		/// Gets or sets the node RID or level index where the daily history purge criteria was inherited from.
		/// </summary>
		public int PurgeDailyCriteriaInheritedFrom 
		{
			get { return _purgeDailyCriteriaInheritedFrom ; }
			set { _purgeDailyCriteriaInheritedFrom = value; }
		}

		/// <summary>
		/// Gets or sets purge criteria for daily history.
		/// </summary>
		public int PurgeDailyHistoryAfter
		{
			get { return _purgeDailyHistoryAfter ; }
			set { _purgeDailyHistoryAfter = value; }
		}

		/// <summary>
		/// Gets or sets the whether the weekly purge criteria was inherited.
		/// </summary>
		public eInheritedFrom PurgeWeeklyCriteriaInherited 
		{
			get { return _purgeWeeklyCriteriaInherited ; }
			set { _purgeWeeklyCriteriaInherited = value; }
		}

		/// <summary>
		/// Gets or sets the node RID or level index where the weekly history purge criteria was inherited from.
		/// </summary>
		public int PurgeWeeklyCriteriaInheritedFrom 
		{
			get { return _purgeWeeklyCriteriaInheritedFrom ; }
			set { _purgeWeeklyCriteriaInheritedFrom = value; }
		}

		/// <summary>
		/// Gets or sets purge criteria for weekly history.
		/// </summary>
		public int PurgeWeeklyHistoryAfter
		{
			get { return _purgeWeeklyHistoryAfter ; }
			set { _purgeWeeklyHistoryAfter = value; }
		}

		/// <summary>
		/// Gets or sets the whether the OTS plan purge criteria was inherited.
		/// </summary>
		public eInheritedFrom PurgeOTSCriteriaInherited 
		{
			get { return _purgeOTSCriteriaInherited ; }
			set { _purgeOTSCriteriaInherited = value; }
		}

		/// <summary>
		/// Gets or sets the node RID or level index where the OTS plan purge criteria was inherited from.
		/// </summary>
		public int PurgeOTSCriteriaInheritedFrom 
		{
			get { return _purgeOTSCriteriaInheritedFrom ; }
			set { _purgeOTSCriteriaInheritedFrom = value; }
		}

		/// <summary>
		/// Gets or sets purge criteria for OTS plans.
		/// </summary>
		public int PurgeOTSPlansAfter
		{
			get { return _purgeOTSPlansAfter ; }
			set { _purgeOTSPlansAfter = value; }
		}

        /* BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type */
        ///// <summary>
        ///// Gets or sets the whether the header purge criteria was inherited.
        ///// </summary>
        //public eInheritedFrom PurgeHeadersCriteriaInherited 
        //{
        //    get { return _purgeHeadersCriteriaInherited ; }
        //    set { _purgeHeadersCriteriaInherited = value; }
        //}

        ///// <summary>
        ///// Gets or sets the node RID or level index where the header purge criteria was inherited from.
        ///// </summary>
        //public int PurgeHeadersCriteriaInheritedFrom 
        //{
        //    get { return _purgeHeadersCriteriaInheritedFrom ; }
        //    set { _purgeHeadersCriteriaInheritedFrom = value; }
        //}

        ///// <summary>
        ///// Gets or sets purge criteria for headers.
        ///// </summary>
        //public int PurgeHeadersAfter
        //{
        //    get { return _purgeHeadersAfter ; }
        //    set { _purgeHeadersAfter = value; }
        //}

        /// Gets or sets the whether the header type ASN purge criteria was inherited.
        /// </summary>
        public eInheritedFrom PurgeHtASNCriteriaInherited
        {
            get { return _purgeHtASNCriteriaInherited; }
            set { _purgeHtASNCriteriaInherited = value; }
        }

        /// <summary>
        /// Gets or sets the node RID or level index where the header type ASN purge criteria was inherited from.
        /// </summary>
        public int PurgeHtASNCriteriaInheritedFrom
        {
            get { return _purgeHtASNCriteriaInheritedFrom; }
            set { _purgeHtASNCriteriaInheritedFrom = value; }
        }

        /// <summary>
        /// Gets or sets purge criteria for ASN header type.
        /// </summary>
        public int PurgeHtASNAfter
        {
            get { return _purgeHtASNAfter; }
            set { _purgeHtASNAfter = value; }
        }

        /// Gets or sets the whether the header type DropShip purge criteria was inherited.
        /// </summary>
        public eInheritedFrom PurgeHtDropShipCriteriaInherited
        {
            get { return _purgeHtDropShipCriteriaInherited; }
            set { _purgeHtDropShipCriteriaInherited = value; }
        }

        /// <summary>
        /// Gets or sets the node RID or level index where the header type DropShip purge criteria was inherited from.
        /// </summary>
        public int PurgeHtDropShipCriteriaInheritedFrom
        {
            get { return _purgeHtDropShipCriteriaInheritedFrom; }
            set { _purgeHtDropShipCriteriaInheritedFrom = value; }
        }

        /// <summary>
        /// Gets or sets purge criteria for DropShip header type.
        /// </summary>
        public int PurgeHtDropShipAfter
        {
            get { return _purgeHtDropShipAfter; }
            set { _purgeHtDropShipAfter = value; }
        }

        /// Gets or sets the whether the header type Dummy purge criteria was inherited.
        /// </summary>
        public eInheritedFrom PurgeHtDummyCriteriaInherited
        {
            get { return _purgeHtDummyCriteriaInherited; }
            set { _purgeHtDummyCriteriaInherited = value; }
        }

        /// <summary>
        /// Gets or sets the node RID or level index where the header type Dummy purge criteria was inherited from.
        /// </summary>
        public int PurgeHtDummyCriteriaInheritedFrom
        {
            get { return _purgeHtDummyCriteriaInheritedFrom; }
            set { _purgeHtDummyCriteriaInheritedFrom = value; }
        }

        /// <summary>
        /// Gets or sets purge criteria for Dummy header type.
        /// </summary>
        public int PurgeHtDummyAfter
        {
            get { return _purgeHtDummyAfter; }
            set { _purgeHtDummyAfter = value; }
        }

        /// Gets or sets the whether the header type Receipt purge criteria was inherited.
        /// </summary>
        public eInheritedFrom PurgeHtReceiptCriteriaInherited
        {
            get { return _purgeHtReceiptCriteriaInherited; }
            set { _purgeHtReceiptCriteriaInherited = value; }
        }

        /// <summary>
        /// Gets or sets the node RID or level index where the header type Receipt purge criteria was inherited from.
        /// </summary>
        public int PurgeHtReceiptCriteriaInheritedFrom
        {
            get { return _purgeHtReceiptCriteriaInheritedFrom; }
            set { _purgeHtReceiptCriteriaInheritedFrom = value; }
        }

        /// <summary>
        /// Gets or sets purge criteria for Receipt header type.
        /// </summary>
        public int PurgeHtReceiptAfter
        {
            get { return _purgeHtReceiptAfter; }
            set { _purgeHtReceiptAfter = value; }
        }

        /// Gets or sets the whether the header type PurchaseOrder purge criteria was inherited.
        /// </summary>
        public eInheritedFrom PurgeHtPurchaseOrderCriteriaInherited
        {
            get { return _purgeHtPurchaseOrderCriteriaInherited; }
            set { _purgeHtPurchaseOrderCriteriaInherited = value; }
        }

        /// <summary>
        /// Gets or sets the node RID or level index where the header type PurchaseOrder purge criteria was inherited from.
        /// </summary>
        public int PurgeHtPurchaseOrderCriteriaInheritedFrom
        {
            get { return _purgeHtPurchaseOrderCriteriaInheritedFrom; }
            set { _purgeHtPurchaseOrderCriteriaInheritedFrom = value; }
        }

        /// <summary>
        /// Gets or sets purge criteria for PurchaseOrder header type.
        /// </summary>
        public int PurgeHtPurchaseOrderAfter
        {
            get { return _purgeHtPurchaseOrderAfter; }
            set { _purgeHtPurchaseOrderAfter = value; }
        }

        /// Gets or sets the whether the header type Reserve purge criteria was inherited.
        /// </summary>
        public eInheritedFrom PurgeHtReserveCriteriaInherited
        {
            get { return _purgeHtReserveCriteriaInherited; }
            set { _purgeHtReserveCriteriaInherited = value; }
        }

        /// <summary>
        /// Gets or sets the node RID or level index where the header type Reserve purge criteria was inherited from.
        /// </summary>
        public int PurgeHtReserveCriteriaInheritedFrom
        {
            get { return _purgeHtReserveCriteriaInheritedFrom; }
            set { _purgeHtReserveCriteriaInheritedFrom = value; }
        }

        /// <summary>
        /// Gets or sets purge criteria for Reserve header type.
        /// </summary>
        public int PurgeHtReserveAfter
        {
            get { return _purgeHtReserveAfter; }
            set { _purgeHtReserveAfter = value; }
        }

        /// Gets or sets the whether the header type VSW purge criteria was inherited.
        /// </summary>
        public eInheritedFrom PurgeHtVSWCriteriaInherited
        {
            get { return _purgeHtVSWCriteriaInherited; }
            set { _purgeHtVSWCriteriaInherited = value; }
        }

        /// <summary>
        /// Gets or sets the node RID or level index where the header type VSW purge criteria was inherited from.
        /// </summary>
        public int PurgeHtVSWCriteriaInheritedFrom
        {
            get { return _purgeHtVSWCriteriaInheritedFrom; }
            set { _purgeHtVSWCriteriaInheritedFrom = value; }
        }

        /// <summary>
        /// Gets or sets purge criteria for VSW header type.
        /// </summary>
        public int PurgeHtVSWAfter
        {
            get { return _purgeHtVSWAfter; }
            set { _purgeHtVSWAfter = value; }
        }

        /// <summary>
        /// Gets or sets the whether the header purge criteria was inherited.
        /// </summary>
        public eInheritedFrom PurgeHtWorkUpTotCriteriaInherited
        {
            get { return _purgeHtWorkUpTotCriteriaInherited; }
            set { _purgeHtWorkUpTotCriteriaInherited = value; }
        }

        /// <summary>
        /// Gets or sets the node RID or level index where the header purge criteria was inherited from.
        /// </summary>
        public int PurgeHtWorkUpTotCriteriaInheritedFrom
        {
            get { return _purgeHtWorkUpTotCriteriaInheritedFrom; }
            set { _purgeHtWorkUpTotCriteriaInheritedFrom = value; }
        }

        /// <summary>
        /// Gets or sets purge criteria for WorkUpTotal header type.
        /// </summary>
        public int PurgeHtWorkUpTotAfter
        {
            get { return _purgeHtWorkUpTotAfter; }
            set { _purgeHtWorkUpTotAfter = value; }
        }
        /* END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type */		

		/// <summary>
		/// Gets or sets the nunber of times the node is found in a descendant list.
		/// </summary>
		public int WeightingFactor 
		{
			get { return _weightingFactor ; }
			set { _weightingFactor = value; }
		}

		/// <summary>
		/// Gets or sets the HierarchyNodeSecurityProfile for chain.
		/// </summary>
		public HierarchyNodeSecurityProfile ChainSecurityProfile
		{
			get
			{
				if (_chainSecurityProfile == null)
				{
					_chainSecurityProfile = new HierarchyNodeSecurityProfile(Key);
				}

				return _chainSecurityProfile ;
			}

			set { _chainSecurityProfile = value; }
		}

		/// <summary>
		/// Gets or sets the HierarchyNodeSecurityProfile for store.
		/// </summary>
		public HierarchyNodeSecurityProfile StoreSecurityProfile
		{
			get
			{
				if (_storeSecurityProfile == null)
				{
					_storeSecurityProfile = new HierarchyNodeSecurityProfile(Key);
				}

				return _storeSecurityProfile ;
			}

			set { _storeSecurityProfile = value; }
		}

		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		/// <summary>
		/// Gets or sets the user the node is in use by.
		/// </summary>
		public string InUseUserID
		{
			get
			{
				return _inUseUserID ;
			}

			set { _inUseUserID = value; }
		}
		//End Track #4815
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
		/// <summary>
		/// Gets or sets the flag identifying if the node is a virtual node.
		/// </summary>
		public bool IsVirtual
		{
			get { return _virtualInd ; }
			set { _virtualInd = value; }
		}
//End Track #4037
        /// <summary>
        /// Gets or sets the field identifying the purpose of the node.
        /// </summary>
        public ePurpose Purpose
        {
            get { return _purpose; }
            set { _purpose = value; }
        }

        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
        public bool Active
        {
            get { return _activeInd; }
            set { _activeInd = value; }
        }
        // End TT#988

        //Begin TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
        /// <summary>
        /// Gets or sets the record id of the calendar date range.
        /// </summary>
        public int Begin_CDR_RID
        {
            get { return _Begin_CDR_RID; }
            set { _Begin_CDR_RID = value; }
        }
        //End TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3

        // Begin TT#3173 - JSmith - Severe Error during History Load
        /// <summary>
        /// Gets or sets the flag identifying if the changes should be immediately committed on successful update.
        /// </summary>
        public bool CommitOnSuccessfulUpdate
        {
            get { return _commitOnSuccessfulUpdate; }
            set { _commitOnSuccessfulUpdate = value; }
        }
        // Begin TT#3173 - JSmith - Severe Error during History Load

        // Begin TT#3630 - JSmith - Delete My Hierarchy
        public bool DeleteNode
        {
            get { return _deleteNode; }
            set { _deleteNode = value; }
        }
        // End // TT#3630 - JSmith - Delete My Hierarchy

        public int DigitalAssetKey
        {
            get { return _digitalAssetKey; }
            set { _digitalAssetKey = value; }
        }

        public object Clone()
        {
            HierarchyNodeProfile hierarchyNodeProfile;

            hierarchyNodeProfile = new HierarchyNodeProfile(_key);
            hierarchyNodeProfile._nodeChangeType = _nodeChangeType;
            hierarchyNodeProfile._nodeLockStatus = _nodeLockStatus;
            hierarchyNodeProfile._nodeChangeSuccessful = _nodeChangeSuccessful;
            hierarchyNodeProfile._message = _message;
            hierarchyNodeProfile._hierarchyRID = _hierarchyRID;
            hierarchyNodeProfile._homeHierarchyParentRID = _homeHierarchyParentRID;
            hierarchyNodeProfile._parents = _parents;
            hierarchyNodeProfile._nodeID = _nodeID;
            hierarchyNodeProfile._qualifiedNodeID = _qualifiedNodeID;
            hierarchyNodeProfile._nodeName = _nodeName;
            hierarchyNodeProfile._nodeDescription = _nodeDescription;
            hierarchyNodeProfile._displayOption = _displayOption;
            hierarchyNodeProfile._text = _text;
            hierarchyNodeProfile._levelText = _levelText;
            hierarchyNodeProfile._homeHierarchyRID = _homeHierarchyRID;
            hierarchyNodeProfile._homeHierarchyLevel = _homeHierarchyLevel;
            hierarchyNodeProfile._homeHierarchyType = _homeHierarchyType;
            hierarchyNodeProfile._productTypeIsOverridden = _productTypeIsOverridden;
            hierarchyNodeProfile._productType = _productType;
            hierarchyNodeProfile._productTypeInherited = _productTypeInherited;
            hierarchyNodeProfile._productTypeInheritedFrom = _productTypeInheritedFrom;
            hierarchyNodeProfile._OTSPlanLevelIsOverridden = _OTSPlanLevelIsOverridden;
            hierarchyNodeProfile._OTSPlanLevelHierarchyRID = _OTSPlanLevelHierarchyRID;
            //Begin TT#106 MD - JSmith - Number of Nodes Added needs to be included in the Audit > Summary of the Hierarchy Load API
            hierarchyNodeProfile._OTSPlanLevelAnchorNode = _OTSPlanLevelAnchorNode;
            hierarchyNodeProfile._OTSPlanLevelSelectType = _OTSPlanLevelSelectType;
            hierarchyNodeProfile._OTSPlanLevelLevelType = _OTSPlanLevelLevelType;
            //End TT#106 MD
            hierarchyNodeProfile._OTSPlanLevelHierarchyLevelSequence = _OTSPlanLevelHierarchyLevelSequence;
            hierarchyNodeProfile._OTSPlanLevelInherited = _OTSPlanLevelInherited;
            hierarchyNodeProfile._OTSPlanLevelInheritedFrom = _OTSPlanLevelInheritedFrom;
            hierarchyNodeProfile._OTSPlanLevelTypeIsOverridden = _OTSPlanLevelTypeIsOverridden;
            hierarchyNodeProfile._OTSPlanLevelType = _OTSPlanLevelType;
            hierarchyNodeProfile._OTSPlanLevelTypeInherited = _OTSPlanLevelTypeInherited;
            hierarchyNodeProfile._OTSPlanLevelTypeInheritedFrom = _OTSPlanLevelTypeInheritedFrom;
            hierarchyNodeProfile._levelType = _levelType;
            hierarchyNodeProfile._isParentOfStyle = _isParentOfStyle;
            hierarchyNodeProfile._useBasicReplenishment = _useBasicReplenishment;
            hierarchyNodeProfile._colorOrSizeCodeRID = _colorOrSizeCodeRID;
            hierarchyNodeProfile._nodeColor = _nodeColor;
            hierarchyNodeProfile._nodeLevel = _nodeLevel;
            hierarchyNodeProfile._hasChildren = _hasChildren;
            hierarchyNodeProfile._displayChildren = _displayChildren;
            hierarchyNodeProfile._purgeCriteriaChangeType = _purgeCriteriaChangeType;
            hierarchyNodeProfile._purgeDailyCriteriaInherited = _purgeDailyCriteriaInherited;
            hierarchyNodeProfile._purgeDailyCriteriaInheritedFrom = _purgeDailyCriteriaInheritedFrom;
            hierarchyNodeProfile._purgeDailyHistoryAfter = _purgeDailyHistoryAfter;
            hierarchyNodeProfile._purgeWeeklyCriteriaInherited = _purgeWeeklyCriteriaInherited;
            hierarchyNodeProfile._purgeWeeklyCriteriaInheritedFrom = _purgeWeeklyCriteriaInheritedFrom;
            hierarchyNodeProfile._purgeWeeklyHistoryAfter = _purgeWeeklyHistoryAfter;
            hierarchyNodeProfile._purgeOTSCriteriaInherited = _purgeOTSCriteriaInherited;
            hierarchyNodeProfile._purgeOTSCriteriaInheritedFrom = _purgeOTSCriteriaInheritedFrom;
            hierarchyNodeProfile._purgeOTSPlansAfter = _purgeOTSPlansAfter;
            //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
            //hierarchyNodeProfile._purgeHeadersCriteriaInherited = _purgeHeadersCriteriaInherited;
            //hierarchyNodeProfile._purgeHeadersCriteriaInheritedFrom = _purgeHeadersCriteriaInheritedFrom;
            //hierarchyNodeProfile._purgeHeadersAfter = _purgeHeadersAfter;

            hierarchyNodeProfile._purgeHtASNCriteriaInherited = _purgeHtASNCriteriaInherited;
            hierarchyNodeProfile._purgeHtASNCriteriaInheritedFrom = _purgeHtASNCriteriaInheritedFrom;
            hierarchyNodeProfile._purgeHtASNAfter = _purgeHtASNAfter;

            hierarchyNodeProfile._purgeHtDropShipCriteriaInherited = _purgeHtDropShipCriteriaInherited;
            hierarchyNodeProfile._purgeHtDropShipCriteriaInheritedFrom = _purgeHtDropShipCriteriaInheritedFrom;
            hierarchyNodeProfile._purgeHtDropShipAfter = _purgeHtDropShipAfter;

            hierarchyNodeProfile._purgeHtDummyCriteriaInherited = _purgeHtDummyCriteriaInherited;
            hierarchyNodeProfile._purgeHtDummyCriteriaInheritedFrom = _purgeHtDummyCriteriaInheritedFrom;
            hierarchyNodeProfile._purgeHtDummyAfter = _purgeHtDummyAfter;

            hierarchyNodeProfile._purgeHtReceiptCriteriaInherited = _purgeHtReceiptCriteriaInherited;
            hierarchyNodeProfile._purgeHtReceiptCriteriaInheritedFrom = _purgeHtReceiptCriteriaInheritedFrom;
            hierarchyNodeProfile._purgeHtReceiptAfter = _purgeHtReceiptAfter;

            hierarchyNodeProfile._purgeHtPurchaseOrderCriteriaInherited = _purgeHtPurchaseOrderCriteriaInherited;
            hierarchyNodeProfile._purgeHtPurchaseOrderCriteriaInheritedFrom = _purgeHtPurchaseOrderCriteriaInheritedFrom;
            hierarchyNodeProfile._purgeHtPurchaseOrderAfter = _purgeHtPurchaseOrderAfter;

            hierarchyNodeProfile._purgeHtReserveCriteriaInherited = _purgeHtReserveCriteriaInherited;
            hierarchyNodeProfile._purgeHtReserveCriteriaInheritedFrom = _purgeHtReserveCriteriaInheritedFrom;
            hierarchyNodeProfile._purgeHtReserveAfter = _purgeHtReserveAfter;

            hierarchyNodeProfile._purgeHtVSWCriteriaInherited = _purgeHtVSWCriteriaInherited;
            hierarchyNodeProfile._purgeHtVSWCriteriaInheritedFrom = _purgeHtVSWCriteriaInheritedFrom;
            hierarchyNodeProfile._purgeHtVSWAfter = _purgeHtVSWAfter;

            hierarchyNodeProfile._purgeHtWorkUpTotCriteriaInherited = _purgeHtWorkUpTotCriteriaInherited;
            hierarchyNodeProfile._purgeHtWorkUpTotCriteriaInheritedFrom = _purgeHtWorkUpTotCriteriaInheritedFrom;
            hierarchyNodeProfile._purgeHtWorkUpTotAfter = _purgeHtWorkUpTotAfter;
            //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type

            hierarchyNodeProfile._weightingFactor = _weightingFactor;
            hierarchyNodeProfile._chainSecurityProfile = _chainSecurityProfile;
            hierarchyNodeProfile._storeSecurityProfile = _storeSecurityProfile;
            hierarchyNodeProfile._virtualInd = _virtualInd;
            hierarchyNodeProfile._purpose = _purpose;
            // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
            hierarchyNodeProfile._activeInd = _activeInd;
            // End TT#988
            //Begin TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
            hierarchyNodeProfile._Begin_CDR_RID = _Begin_CDR_RID;
            //End TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
			// BEGIN TT#1399 - GRT - Alternate Hierarchy Inherit Node Properties
            hierarchyNodeProfile._ApplyHNRIDFrom = _ApplyHNRIDFrom;
            // END TT#1399
            hierarchyNodeProfile._commitOnSuccessfulUpdate = _commitOnSuccessfulUpdate; // TT#3173 - JSmith - Severe Error during History Load
            hierarchyNodeProfile.DigitalAssetKey = _digitalAssetKey;

            return hierarchyNodeProfile;
        }

        //Begin TT#106 MD - JSmith - Number of Nodes Added needs to be included in the Audit > Summary of the Hierarchy Load API
        /// <summary>
        /// overrided Equals
        /// </summary>
        /// <param name="obj">HierarchyNodeProfile</param>
        /// <returns>Bool</returns>
        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            HierarchyNodeProfile compareObj = (HierarchyNodeProfile)obj;
            if (this.Key != compareObj.Key ||
                this.NodeDescription != compareObj.NodeDescription ||
                this.NodeName != compareObj.NodeName ||
                this.ProductType != compareObj.ProductType ||
                this.OTSPlanLevelAnchorNode != compareObj.OTSPlanLevelAnchorNode ||
                this.OTSPlanLevelHierarchyLevelSequence != compareObj.OTSPlanLevelHierarchyLevelSequence ||
                this.OTSPlanLevelHierarchyRID != compareObj.OTSPlanLevelHierarchyRID ||
                this.OTSPlanLevelLevelType != compareObj.OTSPlanLevelLevelType ||
                this.OTSPlanLevelMask != compareObj.OTSPlanLevelMask ||
                this.OTSPlanLevelMaskField != compareObj.OTSPlanLevelMaskField ||
                this.OTSPlanLevelSelectType != compareObj.OTSPlanLevelSelectType ||
                this.OTSPlanLevelType != compareObj.OTSPlanLevelType ||
                this.ApplyHNRIDFrom != compareObj.ApplyHNRIDFrom ||
                this.NodeColor != compareObj.NodeColor
                )
            {
                return false;
            }
            return true;
        }
        //End TT#106 MD
	}

	/// <summary>
	/// Used to retrieve and update a list of nodes for a parent in the hierarchy
	/// </summary>
	[Serializable()]
	public class HierarchyNodeList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HierarchyNodeList(eProfileType aProfileType)
			: base(aProfileType)
		{
			
		}
	}

	/// <summary>
	/// Used to retrieve and update information about a node selected from the Merchandise Explorer.
	/// </summary>
	[Serializable()]
	public class SelectedHierarchyNode
	{
		// Fields

        // Begin Track #5005 - JSmith - Explorer Organization
        //private eHierarchyNodeType _nodeType;
        private eHierarchySelectType _nodeType;
        // End Track #5005
		private HierarchyNodeProfile _nodeProfile;
		

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
        // Begin Track #5005 - JSmith - Explorer Organization
        //public SelectedHierarchyNode(eHierarchyNodeType aNodeType, HierarchyNodeProfile aNodeProfile)
        public SelectedHierarchyNode(eHierarchySelectType aNodeType, HierarchyNodeProfile aNodeProfile)
        // End Track #5005
		{
			_nodeType = aNodeType;
			_nodeProfile = aNodeProfile;
		}

        /// <summary>
		/// Gets or sets the type of node.
		/// </summary>
        // Begin Track #5005 - JSmith - Explorer Organization
        //public eHierarchyNodeType NodeType
        public eHierarchySelectType NodeType
		{
			get { return _nodeType; }
			set { _nodeType = value; }
		}
        // End Track #5005

		/// <summary>
		/// Gets or sets the HierarchyNodeProfile.
		/// </summary>
		public HierarchyNodeProfile NodeProfile
		{
			get { return _nodeProfile; }
			set { _nodeProfile = value; }
		}
	}

	/// <summary>
	/// Used to retrieve the IDs of a node and its parent.
	/// </summary>
	[Serializable()]
	public class HierarchyNodeAndParentIdsProfile : Profile
	{
		// Fields

		private string						_nodeID;
//		private int							_parentRID;
		private ArrayList					_parents;
//		private string						_parentNodeID;
		private ArrayList					_parentNodeIDs;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HierarchyNodeAndParentIdsProfile(int aKey)
			: base(aKey)
		{
			_parents = new ArrayList();
			_parentNodeIDs = new ArrayList();
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.HierarchyNodeParentIds;
			}
		}

		/// <summary>
		/// Gets or sets the id of the node.
		/// </summary>
		public string NodeID 
		{
			get { return _nodeID ; }
			set { _nodeID = (value == null) ? value : value.Trim(); }
		}
//		/// <summary>
//		/// Gets or sets the record id of the parent node in the hierarchy.
//		/// </summary>
//		public int ParentRID 
//		{
//			get { return _parentRID ; }
//			set { _parentRID = value; }
//		}
		/// <summary>
		/// Gets or sets the list of record id(s) of the parent node(s) in the hierarchy.
		/// </summary>
		public ArrayList Parents 
		{
			get { return _parents ; }
			set { _parents = value; }
		}
//		/// <summary>
//		/// Gets or sets the id of the parent of the node.
//		/// </summary>
//		public string ParentNodeID 
//		{
//			get { return _parentNodeID ; }
//			set { _parentNodeID = (value == null) ? value : value.Trim(); }
//		}
		/// <summary>
		/// Gets or sets the id(s) of the parent(s) of the node.
		/// </summary>
		public ArrayList ParentNodeIDs 
		{
			get { return _parentNodeIDs ; }
			set { _parentNodeIDs = value; }
		}
	}

	/// <summary>
	/// Used to retrieve and update information from the hierarchy session about the relationships between nodes
	/// in a hierarchy.
	/// </summary>
	/// <remarks>
	/// This class is used when nodes are moved or reorganized in the hierarchies.
	/// </remarks>
	[Serializable()]
	public class HierarchyJoinProfile : Profile
	{
		// Fields

		private eChangeType					_joinChangeType;
		private bool						_joinChangeSuccessful;
		private int							_oldHierarchyRID;
		private int							_oldParentRID;
		private int							_newHierarchyRID;
		private int							_newParentRID;
		private eHierarchyLevelType			_levelType;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HierarchyJoinProfile(int aKey)
			: base(aKey)
		{
			_joinChangeType = eChangeType.none;
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.HierarchyJoin;
			}
		}

		/// <summary>
		/// Gets or sets the type of change for the relationship in the hierarchy.
		/// </summary>
		public eChangeType JoinChangeType 
		{
			get { return _joinChangeType ; }
			set { _joinChangeType = value; }
		}

		/// <summary>
		/// Gets or sets the type of change for the relationship in the hierarchy.
		/// </summary>
		public bool JoinChangeSuccessful 
		{
			get { return _joinChangeSuccessful ; }
			set { _joinChangeSuccessful = value; }
		}

		/// <summary>
		/// Gets or sets the record id of the original hierarchy.
		/// </summary>
		public int OldHierarchyRID 
		{
			get { return _oldHierarchyRID ; }
			set { _oldHierarchyRID = value; }
		}

		/// <summary>
		/// Gets or sets the record id of the original parent.
		/// </summary>
		public int OldParentRID 
		{
			get { return _oldParentRID ; }
			set { _oldParentRID = value; }
		}

		/// <summary>
		/// Gets or sets the record id of the new hierarchy.
		/// </summary>
		public int NewHierarchyRID 
		{
			get { return _newHierarchyRID ; }
			set { _newHierarchyRID = value; }
		}

		/// <summary>
		/// Gets or sets the record id of the new parent.
		/// </summary>
		public int NewParentRID 
		{
			get { return _newParentRID ; }
			set { _newParentRID = value; }
		}

		/// <summary>
		/// Gets or sets the OTS level type of this level in the hierarchy.
		/// </summary>
		public eHierarchyLevelType LevelType 
		{
			get { return _levelType ; }
			set { _levelType = value; }
		}
	}

	/// <summary>
	/// Used to retrieve information about the ancestors of a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class NodeAncestorProfile : Profile
	{
		// Fields
		private int							_homeHierarchyRID;
		private int							_homeHierarchyLevel;
		//Begin TT#427 - JScott - Size Curve Tab,>Tolerance Tab- Node-Properties -In the Highest Level drop down do not include Personal Hierarchies.
		private int							_homeHierarchyOwner;
		//End TT#427 - JScott - Size Curve Tab,>Tolerance Tab- Node-Properties -In the Highest Level drop down do not include Personal Hierarchies.

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeAncestorProfile(int aKey)
			: base(aKey)
		{

		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.HierarchyNodeAncestor;
			}
		}

		/// <summary>
		/// Gets or sets the record id of its home hierarchy.
		/// </summary>
		public int HomeHierarchyRID 
		{
			get { return _homeHierarchyRID ; }
			set { _homeHierarchyRID = value; }
		}

		/// <summary>
		/// Gets or sets the level of the node in its home hierarchy.
		/// </summary>
		public int HomeHierarchyLevel
		{
			get { return _homeHierarchyLevel; }
			set { _homeHierarchyLevel = value; }
		}
		//Begin TT#427 - JScott - Size Curve Tab,>Tolerance Tab- Node-Properties -In the Highest Level drop down do not include Personal Hierarchies.

		/// <summary>
		/// Gets or sets the owner of the node in its home hierarchy.
		/// </summary>
		public int HomeHierarchyOwner
		{
			get { return _homeHierarchyOwner; }
			set { _homeHierarchyOwner = value; }
		}
		//End TT#427 - JScott - Size Curve Tab,>Tolerance Tab- Node-Properties -In the Highest Level drop down do not include Personal Hierarchies.
	}

	/// <summary>
	/// Used to retrieve information about the ancestors of a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class NodeAncestorList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeAncestorList(eProfileType aProfileType)
			: base(aProfileType)
		{
			
		}
	}

	/// <summary>
	/// Used to retrieve information about the descendants of a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class NodeDescendantProfile : Profile
	{
		// Fields
        // Begin TT#2231 - JSmith - Size curve build failing
        private int _colorOrSizeCodeRID;
        // End TT#2231

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeDescendantProfile(int aKey)
			: base(aKey)
		{
            // Begin TT#2231 - JSmith - Size curve build failing
            _colorOrSizeCodeRID = Include.NoRID;
            // End TT#2231
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.HierarchyNodeDescendant;
			}
		}

        // Begin TT#2231 - JSmith - Size curve build failing
        /// <summary>
        /// Gets or sets the owner of the node in its home hierarchy.
        /// </summary>
        public int ColorOrSizeCodeRID
        {
            get { return _colorOrSizeCodeRID; }
            set { _colorOrSizeCodeRID = value; }
        }
        // Begin TT#2231
	}

	/// <summary>
	/// Used to retrieve a list of descendants for a node in the hierarchy
	/// </summary>
	[Serializable()]
	public class NodeDescendantList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeDescendantList(eProfileType aProfileType)
			: base(aProfileType)
		{
			
		}
	}

	/// <summary>
	/// Used to send or retrieve information about a lock request for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class NodeLockRequestProfile : Profile
	{
		// Fields
		private int _hierarchyRID;
        private eProfileType _nodeType;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeLockRequestProfile(int aKey)
			: base(aKey)
		{
            _nodeType = eProfileType.HierarchyNode;
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.NodeLockRequest;
			}
		}

		/// <summary>
		/// Gets or sets the record id of hierarchy for the node.
		/// </summary>
		public int HierarchyRID
		{
			get { return _hierarchyRID; }
			set { _hierarchyRID = value; }
		}

        /// <summary>
        /// Gets or sets the profile type the node.
        /// </summary>
        public eProfileType NodeType
        {
            get { return _nodeType; }
            set { _nodeType = value; }
        }
	}

	/// <summary>
	/// Used to send or retrieve information about a lock request for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class NodeLockRequestList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeLockRequestList(eProfileType aProfileType)
			: base(aProfileType)
		{

		}
	}

	/// <summary>
	/// Used to send or retrieve information about a lock conflict for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class NodeLockConflictProfile : Profile
	{
		// Fields
		private int _branchHierarchyRID;
		private int _branchNodeRID;
		private string _inUseNodeName;
		private int _inUseByUserRID;
		private string _inUseByUserName;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeLockConflictProfile(int aKey)
			: base(aKey)
		{

		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.NodeLockConflict;
			}
		}

		/// <summary>
		/// Gets or sets the record id of hierarchy for the node.
		/// </summary>
		public int BranchHierarchyRID
		{
			get { return _branchHierarchyRID; }
			set { _branchHierarchyRID = value; }
		}

		/// <summary>
		/// Gets or sets the record id of the node for which the branch is being locked.
		/// </summary>
		public int BranchNodeRID
		{
			get { return _branchNodeRID; }
			set { _branchNodeRID = value; }
		}

		/// <summary>
		/// Gets or sets the name of the node that is locked.
		/// </summary>
		public string InUseNodeName
		{
			get { return _inUseNodeName; }
			set { _inUseNodeName = value; }
		}

		/// <summary>
		/// Gets or sets the record id of user who has the node locked.
		/// </summary>
		public int InUseByUserRID
		{
			get { return _inUseByUserRID; }
			set { _inUseByUserRID = value; }
		}

		/// <summary>
		/// Gets or sets the name of user who has the node locked.
		/// </summary>
		public string InUseByUserName
		{
			get { return _inUseByUserName; }
			set { _inUseByUserName = value; }
		}
	}

	/// <summary>
	/// Used to send or retrieve information about a lock conflict for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class NodeLockConflictList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeLockConflictList(eProfileType aProfileType)
			: base(aProfileType)
		{

		}

		public bool ContainsBranchKey(int aKey)
		{
			foreach (NodeLockConflictProfile conflictProfile in this)
			{
				if (conflictProfile.BranchNodeRID == aKey)
				{
					return true;
				}
			}
			return false;
		}
	}

	/// <summary>
	/// Used to retrieve and update information about a hierarchy node.
	/// </summary>
	[Serializable()]
	public class NodeLookup
	{
		private bool			_lookupSuccessful;
		private int				_nodeRID;
		private string			_nodeID;
		private string			_nodeName;
		private string			_nodeDescription;
		private string			_parentID;
		private string			_sizeProductCategory;
		private string			_sizePrimary;
		private string			_sizeSecondary;
		private int				_nodesAdded;
		// Begin MID Track #4906 - JSmith - database errors during autoadd
		private EditMsgs		_editMsgs;
		// End MID Track #4906

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeLookup()
		{
			_lookupSuccessful = false;
			_nodeRID = Include.NoRID;
			// Begin MID Track #4906 - JSmith - database errors during autoadd
			_editMsgs = null;
			// End MID Track #4906
		}

		/// <summary>
		/// Gets or sets the flag identifying if the lookup was successful.
		/// </summary>
		public bool LookupSuccessful 
		{
			get { return _lookupSuccessful ; }
			set { _lookupSuccessful = value; }
		}

		/// <summary>
		/// Gets or sets the record ID of the node.
		/// </summary>
		public int NodeRID 
		{
			get { return _nodeRID ; }
			set { _nodeRID = value; }
		}

		/// <summary>
		/// Gets or sets the ID of the node.
		/// </summary>
		public string NodeID 
		{
			get { return _nodeID ; }
			set { _nodeID = (value == null) ? value : value.Trim(); }
		}

		/// <summary>
		/// Gets or sets the name of the node.
		/// </summary>
		public string NodeName 
		{
			get { return _nodeName ; }
			set { _nodeName = (value == null) ? value : value.Trim(); }
		}

		/// <summary>
		/// Gets or sets the description of the node.
		/// </summary>
		public string NodeDescription 
		{
			get { return _nodeDescription ; }
			set { _nodeDescription = (value == null) ? value : value.Trim(); }
		}

		/// <summary>
		/// Gets or sets the parent ID of the node.
		/// </summary>
		public string ParentID 
		{
			get { return _parentID ; }
			set { _parentID = (value == null) ? value : value.Trim(); }
		}

		/// <summary>
		/// Gets or sets the product category of the size code.
		/// </summary>
		public string SizeProductCategory 
		{
			get { return _sizeProductCategory ; }
			set { _sizeProductCategory = (value == null) ? value : value.Trim(); }
		}

		/// <summary>
		/// Gets or sets the primary of the size code.
		/// </summary>
		public string SizePrimary 
		{
			get { return _sizePrimary ; }
			set { _sizePrimary = (value == null) ? value : value.Trim(); }
		}

		/// <summary>
		/// Gets or sets the secondary of the size code.
		/// </summary>
		public string SizeSecondary 
		{
			get { return _sizeSecondary ; }
			set { _sizeSecondary = (value == null) ? value : value.Trim(); }
		}

		/// <summary>
		/// Gets or sets the count of the nodes auto added.
		/// </summary>
		public int NodesAdded 
		{
			get { return _nodesAdded ; }
			set { _nodesAdded = value; }
		}

		// Begin MID Track #4906 - JSmith - database errors during autoadd
		public EditMsgs EditMsgs 
		{
			get { return _editMsgs ; }
			set { _editMsgs = value; }
		}
		// End MID Track #4906
	}

	/// <summary>
	/// Used to retrieve the information about the colors in the system
	/// </summary>
	[Serializable()]
	public class ColorCodeProfile : Profile 
	{
		private eChangeType		_colorCodeChangeType;
		private string			_colorCodeID;
		private string			_colorCodeName;
		private string			_colorCodeGroup;
		private string			_text;
        private bool            _virtualInd;
        private ePurpose _purpose;
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public ColorCodeProfile(int aKey)
			: base(aKey)
		{
            _purpose = ePurpose.Default;
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.ColorCode;
			}
		}

		/// <summary>
		/// Gets or sets the type of change for the color.
		/// </summary>
		public eChangeType ColorCodeChangeType 
		{
			get { return _colorCodeChangeType ; }
			set { _colorCodeChangeType = value; }
		}

		/// <summary>
		/// Gets or sets the ID of the color.
		/// </summary>
		public string ColorCodeID 
		{
			get { return _colorCodeID ; }
			set { _colorCodeID = (value == null) ? value : value.Trim(); }
		}
		/// <summary>
		/// Gets or sets the name of the color.
		/// </summary>
		public string ColorCodeName 
		{
			get { return _colorCodeName ; }
			set { _colorCodeName = (value == null) ? value : value.Trim(); }
		}
		/// <summary>
		/// Gets or sets the group of the color.
		/// </summary>
		public string ColorCodeGroup 
		{
			get { return _colorCodeGroup ; }
			set { _colorCodeGroup = (value == null) ? value : value.Trim(); }
		}
		/// <summary>
		/// Gets or sets the text of the color.
		/// </summary>
		public string Text
		{
			get { return _text; }
			set { _text = value; }
		}
        /// <summary>
        /// Gets or sets the virtualInd of the color.
        /// </summary>
        public bool VirtualInd
        {
            get { return _virtualInd; }
            set { _virtualInd = value; }
        }
        /// <summary>
        /// Gets or sets the purpose of the color.
        /// </summary>
        public ePurpose Purpose
        {
            get { return _purpose; }
            set { _purpose = value; }
        }
	}

	/// <summary>
	/// Used to retrieve a list of colors in the system
	/// </summary>
	[Serializable()]
	public class ColorCodeList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public ColorCodeList(eProfileType aProfileType)
			: base(aProfileType)
		{
			
		}
	}

    /// <summary>
    /// Used to retrieve the information about the color groups in the system
    /// </summary>
    [Serializable()]
    public class ColorGroupProfile : Profile
    {
        private string _colorGroupName;
        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public ColorGroupProfile(int aKey)
            : base(aKey)
        {

        }

        /// <summary>
        /// Returns the eProfileType of this profile.
        /// </summary>

        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.ColorGroup;
            }
        }

        /// <summary>
        /// Gets or sets the name of the color group.
        /// </summary>
        public string ColorGroupName
        {
            get { return _colorGroupName; }
            set { _colorGroupName = (value == null) ? value : value.Trim(); }
        }
    }


	/// <summary>
	/// Used to retrieve and update information about a node selected from the Merchandise Explorer.
	/// </summary>
	[Serializable()]
	public class SelectedColorNode
	{
		// Fields

		private ColorCodeProfile _profile;


		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SelectedColorNode(ColorCodeProfile aProfile)
		{
			_profile = aProfile;
		}

		/// <summary>
		/// Gets or sets the ColorCodeProfile.
		/// </summary>
		public ColorCodeProfile SelectedProfile
		{
			get { return _profile; }
			set { _profile = value; }
		}
	}

	/// <summary>
	/// Contains the information about the sizes in the system
	/// </summary>
	[Serializable()]
	public class SizeCodeProfile : Profile, ICloneable
	{
		private eChangeType		_sizeCodeChangeType;
		private string			_sizeCodeID;
		private string			_sizeCodeName;
		private string			_sizeCodePrimary;
		private string			_sizeCodeSecondary;
		private string			_sizeCodeProductCategory;
//		private string			_sizeCodeTableName;
//		private string			_sizeCodeHeading1;
//		private string			_sizeCodeHeading2;
//		private string			_sizeCodeHeading3;
//		private string			_sizeCodeHeading4;
//		private int				_sizeCodeMultipleSizeCode;
		private float			_percent;  // only used as part of a Size Curve
		private int             _sizeCodePrimaryRID;
		private int             _sizeCodeSecondaryRID;
		private int             _primarySequence;
		private int             _secondarySequence;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SizeCodeProfile(int aKey)
			: base(aKey)
		{
			_sizeCodeID = " ";
			_sizeCodeName = " ";
			_sizeCodePrimary = " ";
			_sizeCodeSecondary = " ";
			_sizeCodeProductCategory = " ";
//			_sizeCodeTableName = " ";
//			_sizeCodeHeading1 = " ";
//			_sizeCodeHeading2 = " ";
//			_sizeCodeHeading3 = " ";
//			_sizeCodeHeading4 = " ";
//			_sizeCodeMultipleSizeCode = 0;
			_percent = 0;
			_sizeCodePrimaryRID = Include.NoRID;
			_sizeCodeSecondaryRID = Include.NoRID;
			_primarySequence = int.MaxValue;
			_secondarySequence = int.MaxValue;
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SizeCode;

			}
		}

		/// <summary>
		/// Gets or sets the type of change for the size.
		/// </summary>
		public eChangeType SizeCodeChangeType 
		{
			get { return _sizeCodeChangeType ; }
			set { _sizeCodeChangeType = value; }
		}

		/// <summary>
		/// Gets or sets the ID of the size.
		/// </summary>
		public string SizeCodeID 
		{
			get { return _sizeCodeID ; }
			set { _sizeCodeID = (value == null) ? value : value.Trim(); }
		}

		/// <summary>
		/// Used for the Key for Hashtables and is associated with the table column name
		/// </summary>
		public string SizeCodePrimaryKey
		{
			get { return SizeCodePrimary + " [" + SizeCodeID + "]"; }
		}
		
		/// <summary>
		/// Gets or sets the name of the size.
		/// </summary>
		public string SizeCodeName 
		{
			get { return _sizeCodeName ; }
			set { _sizeCodeName = (value == null) ? value : value.Trim(); }
		}
		/// <summary>
		/// Gets or sets the primary name of the size.
		/// </summary>
		public string SizeCodePrimary 
		{
			get { return _sizeCodePrimary ; }
			set { _sizeCodePrimary = (value == null) ? value : value.Trim(); }
		}
		/// <summary>
		/// Gets or sets the secondary name of the size.
		/// </summary>
		public string SizeCodeSecondary 
		{
			get { return _sizeCodeSecondary ; }
			set { _sizeCodeSecondary = (value == null) ? value : value.Trim(); }
		}
		/// <summary>
		/// Gets or sets the product category of the size.
		/// </summary>
		public string SizeCodeProductCategory 
		{
			get { return _sizeCodeProductCategory ; }
			set { _sizeCodeProductCategory = (value == null) ? value : value.Trim(); }
		}
//		/// <summary>
//		/// Gets or sets the table name of the size.
//		/// </summary>
//		public string SizeCodeTableName 
//		{
//			get { return _sizeCodeTableName ; }
//			set { _sizeCodeTableName = (value == null) ? value : value.Trim(); }
//		}
//		/// <summary>
//		/// Gets or sets the heading1 of the size.
//		/// </summary>
//		public string SizeCodeHeading1 
//		{
//			get { return _sizeCodeHeading1 ; }
//			set { _sizeCodeHeading1 = (value == null) ? value : value.Trim(); }
//		}
//		/// <summary>
//		/// Gets or sets the heading2 of the size.
//		/// </summary>
//		public string SizeCodeHeading2 
//		{
//			get { return _sizeCodeHeading2 ; }
//			set { _sizeCodeHeading2 = (value == null) ? value : value.Trim(); }
//		}
//		/// <summary>
//		/// Gets or sets the heading3 of the size.
//		/// </summary>
//		public string SizeCodeHeading3 
//		{
//			get { return _sizeCodeHeading3 ; }
//			set { _sizeCodeHeading3 = (value == null) ? value : value.Trim(); }
//		}
//		/// <summary>
//		/// Gets or sets the heading4 of the size.
//		/// </summary>
//		public string SizeCodeHeading4 
//		{
//			get { return _sizeCodeHeading4 ; }
//			set { _sizeCodeHeading4 = (value == null) ? value : value.Trim(); }
//		}
//		/// <summary>
//		/// Gets or sets the multiple size code of the size.
//		/// </summary>
//		public int SizeCodeMultipleSizeCode 
//		{
//			get { return _sizeCodeMultipleSizeCode ; }
//			set { _sizeCodeMultipleSizeCode = (value == null) ? value : value.Trim(); }
//		}
		/// <summary>
		/// Gets or sets the Size Curve percent for this size code.
		/// Only used when part of a Size Curve.
		/// </summary>
		public float SizeCodePercent 
		{
			get { return _percent ; }
			set { _percent = value; }
		}

		/// <summary>
		/// Gets or sets the primary size RID (MID column Dimension aka size) 
		/// </summary>
		public int SizeCodePrimaryRID
		{
			get { return _sizeCodePrimaryRID ; }
			set { _sizeCodePrimaryRID = value; }
		}
		/// <summary>
		/// Gets or sets the secondary size RID (MID row Dimension aka width)
		/// </summary>
		public int SizeCodeSecondaryRID
		{
			get { return _sizeCodeSecondaryRID ; }
			set { _sizeCodeSecondaryRID = value; }
		}

		/// <summary>
		/// Gets or sets the primary sequence number used in sorting sizes
		/// </summary>
		public int PrimarySequence
		{
			get { return _primarySequence ; }
			set { _primarySequence = value; }
		}
		/// <summary>
		/// Gets or sets the secondary sequence number used in sorting sizes
		/// </summary>
		public int SecondarySequence
		{
			get { return _secondarySequence ; }
			set { _secondarySequence = value; }
		}


		public object Clone()
		{
			SizeCodeProfile sizeCodeProf;

			sizeCodeProf = new SizeCodeProfile(_key);
			sizeCodeProf._sizeCodeChangeType = _sizeCodeChangeType;
			sizeCodeProf._sizeCodeID = _sizeCodeID;
			sizeCodeProf._sizeCodeName = _sizeCodeName;
			sizeCodeProf._sizeCodePrimary = _sizeCodePrimary;
			sizeCodeProf._sizeCodeSecondary = _sizeCodeSecondary;
			sizeCodeProf._sizeCodeProductCategory = _sizeCodeProductCategory;
//			sizeCodeProf._sizeCodeTableName = _sizeCodeTableName;
//			sizeCodeProf._sizeCodeHeading1 = _sizeCodeHeading1;
//			sizeCodeProf._sizeCodeHeading2 = _sizeCodeHeading2;
//			sizeCodeProf._sizeCodeHeading3 = _sizeCodeHeading3;
//			sizeCodeProf._sizeCodeHeading4 = _sizeCodeHeading4;
//			sizeCodeProf._sizeCodeMultipleSizeCode = _sizeCodeMultipleSizeCode;
			sizeCodeProf._percent = _percent;
			sizeCodeProf._sizeCodePrimaryRID = _sizeCodePrimaryRID;
			sizeCodeProf._sizeCodeSecondaryRID = _sizeCodeSecondaryRID;
			sizeCodeProf._primarySequence = _primarySequence;
			sizeCodeProf._secondarySequence = _secondarySequence;


			return sizeCodeProf;
		}
	}

	/// <summary>
	/// Used to retrieve a list of sizes in the system
	/// </summary>
	[Serializable()]
	public class SizeCodeList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SizeCodeList(eProfileType aProfileType)
			: base(aProfileType)
		{
			
		}
	}

	public class SizeCodePercentComparer:IComparer    
	{      
		public int Compare(object x, object y)      
		{        
			if (!(x is SizeCodeProfile) && !(y is SizeCodeProfile))        
			{          
				throw new ArgumentException("only allows SizeCodeProfile objects");        
			}        
			//return (-((SizeCodeProfile)x).SizeCodePercent.CompareTo(((SizeCodeProfile)y).SizeCodePercent));  // MID Track 4289 Sort Sizes in Ascending Seq by Curve % for Size Plan calculation
			return ((SizeCodeProfile)x).SizeCodePercent.CompareTo(((SizeCodeProfile)y).SizeCodePercent); // MID Track 4289 Sort Sizes in Ascending Seq by Curve % for Size Plan calculation
		}    
	}

	public class SizeCodeSequence:IComparer    
	{      
		public int Compare(object x, object y)      
		{        
			int result = 0;
			if (!(x is SizeCodeProfile) && !(y is SizeCodeProfile))        
			{          
				throw new ArgumentException("only allows SizeCodeProfile objects");        
			}   
     
			result = ((SizeCodeProfile)x).PrimarySequence.CompareTo(((SizeCodeProfile)y).PrimarySequence);   
			if (result != 0)
			{
				return result;
			} 
			result = ((SizeCodeProfile)x).SecondarySequence.CompareTo(((SizeCodeProfile)y).SecondarySequence);
			return result;
		}    
	}

	//Begin Track #4362 - JSmith - Intransit read performance
	/// <summary>
	/// Used to retrieve information for a node used to read intransit.
	/// </summary>
	[Serializable()]
	public class IntransitReadNodeProfile : Profile
	{
		// Fields

		private eHierarchyLevelMasterType			_levelType;
		private int                                 _colorCodeRID;
		private int                                 _sizeCodeRID;
		private bool                                _styleDefinedInHierarchy;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public IntransitReadNodeProfile(int aKey)
			: base(aKey)
		{
			InitializeProfile(eHierarchyLevelMasterType.Undefined, Include.NoRID, Include.NoRID, false);
		}
		/// <summary>
		/// Creates a Size Level instance of the IntransitReadNodeProfile
		/// </summary>
		/// <param name="aKey">Profile Hierarchy Node Key</param>
		/// <param name="aColorCodeRID">Color Code RID </param>
		/// <param name="aSizeCodeRID">Size Code RID </param>
		public IntransitReadNodeProfile(int aKey, int aColorCodeRID, int aSizeCodeRID)
			: base(aKey)
		{
			if (aColorCodeRID < 0 || aSizeCodeRID < 0)
			{
				throw new Exception("ColorCodeRID and SizeCodeRID must be non-negative integers for a size level instance of the IntransitReadProfile");
			}
            InitializeProfile(eHierarchyLevelMasterType.Size, aColorCodeRID, aSizeCodeRID, false); 
		}
		/// <summary>
		/// Creates a Color Level instance of the IntransitReadNodeProfile
		/// </summary>
		/// <param name="aKey">Profile Hierarchy Node Key</param>
		/// <param name="aColorCodeRID">Color Code RID</param>
		public IntransitReadNodeProfile(int aKey, int aColorCodeRID)
			: base(aKey)
		{
			if (aColorCodeRID < 0)
			{
				throw new Exception("ColorCodeRID must be a non-negative integer for a color level instance of IntransitReadNodeProfile");
			}
			InitializeProfile(eHierarchyLevelMasterType.Color, aColorCodeRID, Include.NoRID, false);
		}

		/// <summary>
		/// Creates a ParentOfStyle or Style level instance of the IntransitReadNodeProfile
		/// </summary>
		/// <param name="aKey">Profile Hierarchy Node Key</param>
		/// <param name="aHierarchyLevelMasterType">ParentOfStyle or Style Hierarchy Level Master Type</param>
		public IntransitReadNodeProfile(int aKey, eHierarchyLevelMasterType aHierarchyLevelMasterType)
			: base(aKey)
		{
			if (aHierarchyLevelMasterType != eHierarchyLevelMasterType.ParentOfStyle
				&& aHierarchyLevelMasterType != eHierarchyLevelMasterType.Style)
			{
				throw new Exception("HierarchyLevelMasterType must be either ParentOfStyle or Style to create a ParentOfStyle or Style instance of IntransitReadNodeProfile");
			}
			InitializeProfile(aHierarchyLevelMasterType, Include.NoRID, Include.NoRID, true);
		}
		private void InitializeProfile(eHierarchyLevelMasterType aHierarchyLevelMasterType, int aColorCodeRID, int aSizeCodeRID, bool aStyleDefinedInHierarchy)
		{
			_levelType = aHierarchyLevelMasterType;
			_colorCodeRID = aColorCodeRID;
			_sizeCodeRID = aSizeCodeRID;
			_styleDefinedInHierarchy = aStyleDefinedInHierarchy;
		}
		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.IntransitReadNode;
			}
		}

		/// <summary>
		/// Gets or sets the level type for this product.
		/// </summary>
		public eHierarchyLevelMasterType LevelType
		{
			get { return _levelType ; }
			set { _levelType = value; }
		}
		/// <summary>
		/// Gets or sets whether associated Styles are defined in the Hierarchy
		/// </summary>
		public bool StyleDefinedInHierarchy
		{
			get { return _styleDefinedInHierarchy; }
			set { _styleDefinedInHierarchy = value; }
		}
		/// <summary>
		/// Gets the Color Code RID when this is a color or size level IntransitReadNodeProfile
		/// </summary>
		/// <remarks>Returns Include.NoRID = -1 when this is not a color or size level </remarks>
		public int ColorCodeRID
		{
			get { return _colorCodeRID; }
		}
		/// <summary>
		/// Gets the Size Code RID when this is a size level IntransitReadNodeProfile
		/// </summary>
		public int SizeCodeRID
		{
			get { return _sizeCodeRID; }
		}
	}

	/// <summary>
	/// Used to retrieve and update a list of nodes for a parent in the hierarchy
	/// </summary>
	[Serializable()]
	public class IntransitReadNodeList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public IntransitReadNodeList(eProfileType aProfileType)
			: base(aProfileType)
		{
			
		}
	}

	// BEGIN TT#1401 - stodd - add resevation stores (IMO)
	/// <summary>
	/// Used to retrieve information for a node used to read IMO.
	/// </summary>
	[Serializable()]
	public class IMOReadNodeProfile : Profile
	{
		// Fields

		private eHierarchyLevelMasterType _levelType;
		private int _colorCodeRID;
		private int _sizeCodeRID;
		private bool _styleDefinedInHierarchy;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public IMOReadNodeProfile(int aKey)
			: base(aKey)
		{
			InitializeProfile(eHierarchyLevelMasterType.Undefined, Include.NoRID, Include.NoRID, false);
		}
		/// <summary>
		/// Creates a Size Level instance of the IMOReadNodeProfile
		/// </summary>
		/// <param name="aKey">Profile Hierarchy Node Key</param>
		/// <param name="aColorCodeRID">Color Code RID </param>
		/// <param name="aSizeCodeRID">Size Code RID </param>
		public IMOReadNodeProfile(int aKey, int aColorCodeRID, int aSizeCodeRID)
			: base(aKey)
		{
			if (aColorCodeRID < 0 || aSizeCodeRID < 0)
			{
				throw new Exception("ColorCodeRID and SizeCodeRID must be non-negative integers for a size level instance of the IMOReadNodeProfile");
			}
			InitializeProfile(eHierarchyLevelMasterType.Size, aColorCodeRID, aSizeCodeRID, false);
		}
		/// <summary>
		/// Creates a Color Level instance of the IMOReadNodeProfile
		/// </summary>
		/// <param name="aKey">Profile Hierarchy Node Key</param>
		/// <param name="aColorCodeRID">Color Code RID</param>
		public IMOReadNodeProfile(int aKey, int aColorCodeRID)
			: base(aKey)
		{
			if (aColorCodeRID < 0)
			{
				throw new Exception("ColorCodeRID must be a non-negative integer for a color level instance of IMOReadNodeProfile");
			}
			InitializeProfile(eHierarchyLevelMasterType.Color, aColorCodeRID, Include.NoRID, false);
		}

		/// <summary>
		/// Creates a ParentOfStyle or Style level instance of the IMOReadNodeProfile
		/// </summary>
		/// <param name="aKey">Profile Hierarchy Node Key</param>
		/// <param name="aHierarchyLevelMasterType">ParentOfStyle or Style Hierarchy Level Master Type</param>
		public IMOReadNodeProfile(int aKey, eHierarchyLevelMasterType aHierarchyLevelMasterType)
			: base(aKey)
		{
			if (aHierarchyLevelMasterType != eHierarchyLevelMasterType.ParentOfStyle
				&& aHierarchyLevelMasterType != eHierarchyLevelMasterType.Style)
			{
				throw new Exception("HierarchyLevelMasterType must be either ParentOfStyle or Style to create a ParentOfStyle or Style instance of IMOReadNodeProfile");
			}
			InitializeProfile(aHierarchyLevelMasterType, Include.NoRID, Include.NoRID, true);
		}
		private void InitializeProfile(eHierarchyLevelMasterType aHierarchyLevelMasterType, int aColorCodeRID, int aSizeCodeRID, bool aStyleDefinedInHierarchy)
		{
			_levelType = aHierarchyLevelMasterType;
			_colorCodeRID = aColorCodeRID;
			_sizeCodeRID = aSizeCodeRID;
			_styleDefinedInHierarchy = aStyleDefinedInHierarchy;
		}
		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.IMOReadNode;
			}
		}

		/// <summary>
		/// Gets or sets the level type for this product.
		/// </summary>
		public eHierarchyLevelMasterType LevelType
		{
			get { return _levelType; }
			set { _levelType = value; }
		}
		/// <summary>
		/// Gets or sets whether associated Styles are defined in the Hierarchy
		/// </summary>
		public bool StyleDefinedInHierarchy
		{
			get { return _styleDefinedInHierarchy; }
			set { _styleDefinedInHierarchy = value; }
		}
		/// <summary>
		/// Gets the Color Code RID when this is a color or size level IntransitReadNodeProfile
		/// </summary>
		/// <remarks>Returns Include.NoRID = -1 when this is not a color or size level </remarks>
		public int ColorCodeRID
		{
			get { return _colorCodeRID; }
		}
		/// <summary>
		/// Gets the Size Code RID when this is a size level IntransitReadNodeProfile
		/// </summary>
		public int SizeCodeRID
		{
			get { return _sizeCodeRID; }
		}
	}

	/// <summary>
	/// Used to retrieve and update a list of nodes for a parent in the hierarchy
	/// </summary>
	[Serializable()]
	public class IMOReadNodeList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public IMOReadNodeList(eProfileType aProfileType)
			: base(aProfileType)
		{

		}
	}
    // END TT#1401 - stodd - add resevation stores (IMO)

    // BEGIN TT#1401 - gtaylor - reservation stores
    [Serializable()]
    public class IMOBaseProfile : Profile
    {
        private int _imoMinShipQty;
        private double _imoPackQty;
        private int _imoMaxValue;
        private int _imoStoreRID;
        private bool _imoApplyVSW;
        private double _imoFWOSMax;    //  TT#2225 = gtaylor - VSW ANF

        //BEGIN TT#108 - MD - DOConnell - FWOS Max Model
        private eModifierType _imoFWOSMaxType;
        private int _imoFWOSMaxModelRID;
        private string _imoFWOSMaxModelName;
        //END TT#108 - MD - DOConnell - FWOS Max Model

        public int IMOMinShipQty
        {
            get { return _imoMinShipQty; }
            set { _imoMinShipQty = value; }
        }
        public bool IMOMinShipQtyIsDefault
        {
            get { return (_imoMinShipQty == 0); }
        }

        public double IMOPackQty
        {
            get { return _imoPackQty; }
            set { _imoPackQty = value; }
        }
        public bool IMOPackQtyIsDefault
        {
            get { return (_imoPackQty == .5); }
        }

        // BEGIN TT#2225 - gtaylor - VSW ANF
        public double IMOFWOS_Max
        {
            get { return _imoFWOSMax; }
            set { _imoFWOSMax = value; }
        }
        public bool IMOFWOSMaxIsDefault
        {
            // Begin TT#270-MD - JSmith -  FWOS Max Model not holding in Node Properties
            //get { return (_imoFWOSMax == int.MaxValue); }
            get { return (_imoFWOSMax == int.MaxValue && _imoFWOSMaxModelRID == Include.NoRID); }
            // End TT#270-MD - JSmith -  FWOS Max Model not holding in Node Properties
        }
        // END TT#2225 - gtaylor - VSW ANF

        //BEGIN TT#108 - MD - DOConnell - FWOS Max Model
        public eModifierType IMOFWOS_MaxType
        {
            get { return _imoFWOSMaxType; }
            set { _imoFWOSMaxType = value; }
        }
        public int IMOFWOS_MaxModelRID
        {
            get { return _imoFWOSMaxModelRID; }
            set { _imoFWOSMaxModelRID = value; }
        }

        public string IMOFWOS_MaxModelName
        {
            get { return _imoFWOSMaxModelName; }
            set { _imoFWOSMaxModelName = value; }
        }

        //END TT#108 - MD - DOConnell - FWOS Max Model


        public int IMOMaxValue
        {
            get { return _imoMaxValue; }
            set { _imoMaxValue = value; }
        }
        public bool IMOMaxValueIsDefault
        {
            get { return (_imoMaxValue == int.MaxValue); }
        }

        public int IMOStoreRID
        {
            get { return _imoStoreRID; }
            set { _imoStoreRID = value; }
        }
        public bool IMOStoreRIDIsDefault
        {
            get { return (_imoStoreRID == Include.NoRID); }
        }

        public bool IMO_Apply_VSW
        {
            get { return _imoApplyVSW; }
            set { _imoApplyVSW = value; }
        }

        public IMOBaseProfile(int aKey)
            : base(aKey)
        {
            _imoMinShipQty = 0;
            _imoPackQty = .5;
            _imoMaxValue = int.MaxValue;
            _imoFWOSMax = int.MaxValue; // TT#2225 - gtaylor - ANF VSW
            _imoStoreRID = Include.NoRID;
            //BEGIN TT#108 - MD - DOConnell - FWOS Max Model
            _imoFWOSMaxType = eModifierType.None;
            _imoFWOSMaxModelRID = Include.NoRID;
            _imoFWOSMaxModelName = "";
            //END TT#108 - MD - DOConnell - FWOS Max Model
        }
        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.IMO;
            }
        }

        public bool IsIMODefaultValues
        {
            get
            {   
                return (IMOMinShipQtyIsDefault && IMOPackQtyIsDefault && IMOMaxValueIsDefault && IMOFWOSMaxIsDefault); // TT#2225 - gtaylor - ANF VSW
            }  
        }

        public bool IsIMOBaseDefault
        {
            get { return (IMOMinShipQtyIsDefault && IMOPackQtyIsDefault &&
                IMOMaxValueIsDefault && IMOStoreRIDIsDefault && IMOFWOSMaxIsDefault);  // TT#2225 - gtaylor - ANF VSW
            }            
        }
    }

    [Serializable()]
    public class IMOProfile : IMOBaseProfile
    {
        // Fields
        private eChangeType     _IMOChangeType;
        private bool            _recordExists;

        private bool            _imoIsInherited;
        private int             _imoInheritedFromNodeRID;

        private bool            _imoIsDefault;

        private int             _imoPshToBackStock;
        private int             _imoNodeRID;

        // Properties
        public bool RecordExists
        {
            get { return _recordExists; }
            set { _recordExists = value; }
        }

        public eChangeType IMOChangeType
        {
            get { return _IMOChangeType; }
            set { _IMOChangeType = value; }
        }

        public bool IMOIsInherited
        {
            get { return _imoIsInherited; }
            set { _imoIsInherited = value; }
        }

        public int IMOInheritedFromNodeRID
        {
            get { return _imoInheritedFromNodeRID; }
            set { _imoInheritedFromNodeRID = value; }
        }

        public bool IMOIsDefault
        {
            get { return _imoIsDefault; }
            set { _imoIsDefault = value; }
        }

        public int IMOPshToBackStock
        {
            get { return _imoPshToBackStock; }
            set { _imoPshToBackStock = value; }
        }

        public bool IMOPshToBackStockIsDefault
        {
            get { return (_imoPshToBackStock == Include.NoRID);}
        }

        public int IMONodeRID
        {
            get { return _imoNodeRID; }
            set { _imoNodeRID = value; }
        }

        public bool IMONodeRIDIsDefault
        {
            get { return (_imoNodeRID == Include.NoRID); }
        }

        public IMOProfile(int aKey)
            : base(aKey)
        {
            _IMOChangeType = eChangeType.none;
            _recordExists = false;           
            _imoPshToBackStock = Include.NoRID;
            _imoNodeRID = Include.NoRID;            
            _imoInheritedFromNodeRID = Include.NoRID;
            _imoIsInherited = false;
            _imoIsDefault = true;
        }

        public bool IsDefault
        {
            get { return (IMOPshToBackStockIsDefault && IMONodeRIDIsDefault && IsIMOBaseDefault); }
        }

        public bool IsDefaultValues
        {
            get { return (IMOPshToBackStockIsDefault && IsIMODefaultValues); }
        }
    }


    [Serializable()]
    public class IMOMethodOverrideProfile : IMOBaseProfile
    {        
        // Fields
        private eChangeType     _IMOChangeType;
        private bool            _recordExists;

        private int             _imoMethodRID;

        private bool            _isDefault;

        // Properties
        public eChangeType IMOChangeType
        {
            get { return _IMOChangeType; }
            set { _IMOChangeType = value; }
        }
        public bool RecordExists
        {
            get { return _recordExists; }
            set { _recordExists = value; }
        }
        public bool IMOIsDefault
        {
            get { return _isDefault; }
            set { _isDefault = value; }
        }
        public int IMOMethodRID
        {
            get { return _imoMethodRID; }
            set { _imoMethodRID = value; }
        }
        public bool IMOMethodRIDIsDefault
        {
            get { return (_imoMethodRID == Include.NoRID); }
        }

        public IMOMethodOverrideProfile(int aKey)
            : base(aKey)
        {
            _IMOChangeType = eChangeType.none;
            _recordExists = false;
            _imoMethodRID = Include.NoRID;
            _isDefault = true;
        }

        public bool IsDefault
        {
            get { return (IMOMethodRIDIsDefault && IsIMOBaseDefault); }
        }

    }

    /// <summary>
    /// Used to retrieve a list of IMO information
    /// </summary>
    [Serializable()]
    public class IMOProfileList : ProfileList
    {
        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public IMOProfileList(eProfileType aProfileType)
            : base(aProfileType)
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }

    /// <summary>
    /// Used to retrieve a list of IMO information
    /// </summary>
    [Serializable()]
    public class IMOMethodOverrideProfileList : ProfileList
    {
        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public IMOMethodOverrideProfileList(eProfileType aProfileType)
            : base(aProfileType)
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }

    // END TT#1401 - gtaylor - reservation stores

	//Begin Track #4362 

	/// <summary>
	/// Contains the information about the product characteristics in the system
	/// </summary>
	[Serializable()]
	public class ProductCharProfile : Profile
	{
		private eChangeType _productCharChangeType;
		private string _productCharID;
		private ArrayList _productCharValues;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public ProductCharProfile(int aKey)
			: base(aKey)
		{
			_productCharID = string.Empty;
			_productCharValues = new ArrayList();

		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.ProductCharacteristic;

			}
		}

		/// <summary>
		/// Gets or sets the type of change for the product characteristic.
		/// </summary>
		public eChangeType ProductCharChangeType
		{
			get { return _productCharChangeType; }
			set { _productCharChangeType = value; }
		}
		/// <summary>
		/// Gets or sets the name of the product characteristic.
		/// </summary>
		public string ProductCharID
		{
			get { return _productCharID; }
			set { _productCharID = value; }
		}
		/// <summary>
		/// Gets or sets the list of product characteristic values.
		/// </summary>
		/// <remarks>Contains instances of ProductCharValueProfile class</remarks>
		public ArrayList ProductCharValues
		{
			get { return _productCharValues; }
			set { _productCharValues = value; }
		}
		/// <summary>
		/// Gets the text to display for the characteristic.
		/// </summary>
		public string Text
		{
			get { return _productCharID; }
		}

		public object Clone()
		{
			ProductCharProfile productCharProf;

			productCharProf = new ProductCharProfile(_key);
			productCharProf._productCharChangeType = _productCharChangeType;
			productCharProf._productCharID = _productCharID;
			productCharProf._productCharValues = new ArrayList();
			foreach (ProductCharValueProfile pcvp in _productCharValues)
			{
				productCharProf._productCharValues.Add(pcvp.Clone());
			}

			return productCharProf;
		}
	}

	/// <summary>
	/// Used to retrieve a list of product characteristic profiles
	/// </summary>
	[Serializable()]
	public class ProductCharProfileList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public ProductCharProfileList(eProfileType aProfileType)
			: base(aProfileType)
		{

		}
	}

	/// <summary>
	/// Contains the information about the product characteristics in the system
	/// </summary>
	[Serializable()]
	public class ProductCharValueProfile : Profile
	{
		private eChangeType _productCharValueChangeType;
		private string _productCharValue;
		private int _productCharRID;
        private bool _hasBeenMoved; //TT#3962-VStuart-Dragged Values never allowed to drop-MID

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public ProductCharValueProfile(int aKey)
			: base(aKey)
		{
			_productCharValue = string.Empty;
			_productCharRID = Include.NoRID;
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.ProductCharacteristicValue;

			}
		}

		/// <summary>
		/// Gets or sets the type of change for the product characteristic value.
		/// </summary>
		public eChangeType ProductCharValueChangeType
		{
			get { return _productCharValueChangeType; }
			set { _productCharValueChangeType = value; }
		}

		/// <summary>
		/// Gets or sets the value of the product characteristic value.
		/// </summary>
		public string ProductCharValue
		{
			get { return _productCharValue; }
			set { _productCharValue = value; }
		}
		/// <summary>
		/// Gets or sets the record ID of the product characteristic.
		/// </summary>
		public int ProductCharRID
		{
			get { return _productCharRID; }
			set { _productCharRID = value; }
		}

        //BEGIN TT#3962-VStuart-Dragged Values never allowed to drop-MID
        /// <summary>
        /// Gets or sets the flag to identify if the node has been moved.
        /// </summary>
        public bool HasBeenMoved
        {
            get { return _hasBeenMoved; }
            set { _hasBeenMoved = value; }
        }
        //END TT#3962-VStuart-Dragged Values never allowed to drop-MID
        
        /// <summary>
		/// Gets the text of the product characteristic.
		/// </summary>
		public string Text
		{
			get { return _productCharValue; }
		}


		public object Clone()
		{
			ProductCharValueProfile productCharValueProf;

			productCharValueProf = new ProductCharValueProfile(_key);
			productCharValueProf._productCharValueChangeType = _productCharValueChangeType;
			productCharValueProf._productCharValue = _productCharValue;
			productCharValueProf._productCharRID = _productCharRID;

			return productCharValueProf;
		}
	}

	/// <summary>
	/// Used to retrieve a list of product characteristic profiles
	/// </summary>
	[Serializable()]
	public class ProductCharValueProfileList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public ProductCharValueProfileList(eProfileType aProfileType)
			: base(aProfileType)
		{

		}
	}

	/// <summary>
	/// Contains the information about the product characteristics in the system
	/// </summary>
	[Serializable()]
	public class NodeCharProfile : Profile
	{
		private eChangeType _productCharChangeType;
		private string _productCharID;
		private int  _productCharValueRID;
		private string _productCharValue;
		private eInheritedFrom _typeInherited;
		private int _inheritedFrom;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public NodeCharProfile(int aKey)
			: base(aKey)
		{
			_productCharID = string.Empty;
			_productCharChangeType = eChangeType.none;
			_typeInherited = eInheritedFrom.None;
			_inheritedFrom = Include.NoRID;
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.ProductCharacteristic;
			}
		}

		/// <summary>
		/// Gets or sets the type of change for the product characteristic.
		/// </summary>
		public eChangeType ProductCharChangeType
		{
			get { return _productCharChangeType; }
			set { _productCharChangeType = value; }
		}
		/// <summary>
		/// Gets or sets the name of the product characteristic.
		/// </summary>
		public string ProductCharID
		{
			get { return _productCharID; }
			set { _productCharID = value; }
		}
		/// <summary>
		/// Gets or sets the record ID of the product characteristic value.
		/// </summary>
		/// <remarks>Key is </remarks>
		public int ProductCharValueRID
		{
			get { return _productCharValueRID; }
			set { _productCharValueRID = value; }
		}
		/// <summary>
		/// Gets or sets the the product characteristic value.
		/// </summary>
		/// <remarks>Key is </remarks>
		public string ProductCharValue
		{
			get { return _productCharValue; }
			set { _productCharValue = value; }
		}


		/// <summary>
		/// Gets or sets the whether the product type was inherited.
		/// </summary>
		public eInheritedFrom TypeInherited
		{
			get { return _typeInherited; }
			set { _typeInherited = value; }
		}

		/// <summary>
		/// Gets or sets the node RID or level index where the product type was inherited from.
		/// </summary>
		public int InheritedFrom
		{
			get { return _inheritedFrom; }
			set { _inheritedFrom = value; }
		}

		public object Clone()
		{
			NodeCharProfile nodeCharProf;

			nodeCharProf = new NodeCharProfile(_key);
			nodeCharProf._productCharChangeType = _productCharChangeType;
			nodeCharProf._typeInherited = _typeInherited;
			nodeCharProf._productCharID = _productCharID;
			nodeCharProf._inheritedFrom = _inheritedFrom;
			nodeCharProf._productCharValueRID = _productCharValueRID;

			return nodeCharProf;
		}
	}

    /// <summary>
    /// Used to retrieve a list of product characteristic profiles
    /// </summary>
    [Serializable()]
    public class NodeCharProfileList : ProfileList
    {
        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public NodeCharProfileList(eProfileType aProfileType)
            : base(aProfileType)
        {

        }
    }

	//  BEGIN TT#2015 - gtaylor - Apply Changes to Lower Levels
    [Serializable()]
    public class NodeChangeProfile
    {
        /// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
        public NodeChangeProfile()
        {
        }

        public NodeChangeProfile(int nodeRID, int userID, DateTime changeDate, Dictionary<int, Dictionary<long, object>> nodeChanges)
		{
            ChangeDate = changeDate;
            NodeRID = nodeRID;
            UserID = userID;
            NodeChanges = nodeChanges;
        }

        public NodeChangeProfile(int nodeRID, int userID, DateTime changeDate)
        {
            ChangeDate = changeDate;
            NodeRID = nodeRID;
            UserID = userID;
            NodeChanges = new Dictionary<int,Dictionary<long,object>>();
        }

        private DateTime _changeDate;
        public DateTime ChangeDate
        {
            get { return _changeDate; }
            set { _changeDate = value; }
        }

        private int _nodeRID = Include.NoRID;
        public int NodeRID
        {
            get { return _nodeRID; }
            set { _nodeRID = value; }
        }

        private int _userID;
        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        private int _totalNumberOfDescendants;
        public int TotalNumberOfDescendants
        {
            get { return _totalNumberOfDescendants; }
            set { _totalNumberOfDescendants = value; }
        }

        /// <summary>
        /// dictionary of changes made by this user to this node
        ///     eProfileType (existing enum)
        ///     Change Dictionary
        ///         column or field id
        ///         ChangeProfile
        /// </summary>
        private Dictionary<int, Dictionary<long, object>> _nodeChanges;
        public Dictionary<int, Dictionary<long, object>> NodeChanges
        {
            get { return _nodeChanges; }
            set { _nodeChanges = value; }
        }

        /// <summary>
        /// add a dictionary entry to the underlying dictionary for the specified profiletype
        /// </summary>
        /// <param name="profileType"></param>
        /// <param name="changeEntry"></param>
        public void Add(eProfileType profileType, KeyValuePair<long, object> changeEntry)
        {
            Dictionary<long, object> _parentdictionary = new Dictionary<long, object>();            
            try
            {
                //  if the change's old and new values are not the same then add it
                if (((ChangeProfileBase)changeEntry.Value).OldValue != ((ChangeProfileBase)changeEntry.Value).NewValue)
                {
                    // does this profiletype already exist in the dictionary?
                    if (NodeChanges.TryGetValue((int)profileType, out _parentdictionary))
                    {
                        // the dictionary has items of this profiletype
                        if (_parentdictionary != null)
                        {
                            if (!_parentdictionary.ContainsKey(changeEntry.Key))
                            {
                                _parentdictionary.Add(changeEntry.Key, changeEntry.Value);
                            }
                            else
                            {
                                //  this changeEntry exists
                                //  grab the old value
                                //  replace the old value in the change entry
                                ((ChangeProfileBase)changeEntry.Value).OldValue = ((ChangeProfileBase)_parentdictionary[changeEntry.Key]).OldValue;
                                //  are the old and new values the same after the oldvalue copy?
                                if (((ChangeProfileBase)changeEntry.Value).OldValue != ((ChangeProfileBase)changeEntry.Value).NewValue)
                                    _parentdictionary[changeEntry.Key] = changeEntry.Value;
                                else
                                    _parentdictionary.Remove(changeEntry.Key);
                            }
                        }
                        //  no items in the dictionary of this profiletype
                        else
                        {
                            _parentdictionary = new Dictionary<long, object>();
                            _parentdictionary.Add(changeEntry.Key, changeEntry.Value);
                            NodeChanges.Add((int)profileType, _parentdictionary);
                        }
                    }
                    else
                    {
                        //  add the profiletype entry AND the change profile
                        _parentdictionary = new Dictionary<long, object>();
                        _parentdictionary.Add(changeEntry.Key, changeEntry.Value);
                        NodeChanges.Add((int)profileType, _parentdictionary);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void Delete(eProfileType profileType, KeyValuePair<long, object> changeEntry)
        {
            Dictionary<long, object> _parentdictionary;
            try
            {
                // does this profiletype already exist in the dictionary?
                if (NodeChanges.TryGetValue((int)profileType, out _parentdictionary))
                {
                    // exists
                    if (_parentdictionary.ContainsKey(changeEntry.Key))
                    {
                        _parentdictionary.Remove(changeEntry.Key);
                    }
                }
            }
            catch
            {
                throw;
            }        
        }

        public Dictionary<long, object> Find(eProfileType profileType, KeyValuePair<long, object> changeEntry)
        {
            Dictionary<long, object> _parentdictionary;
            try
            {
                // does this profiletype already exist in the dictionary?
                NodeChanges.TryGetValue((int)profileType, out _parentdictionary);
                return _parentdictionary;
            }
            catch
            {
                throw;
            }
        }

        public int ChangesTotal()
        {
            int changecount = 0;

            foreach (Dictionary<long, object> dict in this.NodeChanges.Values)
            {
                changecount += dict.Count;
            }

            return changecount;
        }
    }

    /// <summary>
    ///     this base stores the old and new values 
    ///     for the object that has been altered
    /// </summary>
    [Serializable()]
    public class ChangeProfileBase
    {
        public ChangeProfileBase()
        { }

        private string _oldvalue;
        public string OldValue
        {
            get { return _oldvalue; }
            set { _oldvalue = value; }
        }

        private string _newvalue;
        public string NewValue
        {
            get { return _newvalue; }
            set { _newvalue = value; }
        }

        private string _changedObjectName;
        public string ChangedObjectName
        {
            get { return _changedObjectName; }
            set { _changedObjectName = value; }
        }
    }

    [Serializable()]
    public class ChangeProfileVSW : ChangeProfileBase   //  VSW - Virtual Store Warehouse
    {
        public ChangeProfileVSW()
        { }

        public ChangeProfileVSW(int storeRID, string storeID, string oldValue, string newValue, string changedObjectName, int columnIndex)
        {
            StoreRID = storeRID;
            StoreID = storeID;
            OldValue = oldValue;
            NewValue = newValue;
            ChangedObjectName = changedObjectName;
            ColumnIndex = columnIndex;
        }

        /// <summary>
        /// Store RID
        /// </summary>
        private int _storeRID;
        public int StoreRID
        {
            get { return _storeRID; }
            set { _storeRID = value; }
        }

        /// <summary>
        /// VSW Store ID
        /// </summary>
        private string _storeID;
        public string StoreID
        {
            get { return _storeID; }
            set { _storeID = value; }
        }

        /// <summary>
        /// Column Index of Changed Column
        /// </summary>
        private int _columnIndex;
        public int ColumnIndex
        {
            get { return _columnIndex; }
            set { _columnIndex = value; }
        }

        public long ChangeProfileKey
        {
            get
            {
                return ((((long)StoreRID) << 32) + (long)ColumnIndex);
            }
        }
    }

    [Serializable()]
    public class ChangeProfileSC : ChangeProfileBase        //  Store Capacity
	{
        public ChangeProfileSC()
        { }

        public ChangeProfileSC(int storeRID, string storeID, string oldValue, string newValue, string changedObjectName, int columnIndex)
        {
            StoreRID = storeRID;
            StoreID = storeID;
            OldValue = oldValue;
            NewValue = newValue;
            ChangedObjectName = changedObjectName;
            ColumnIndex = columnIndex;
        }

        /// <summary>
        /// Store RID
        /// </summary>
        private int _storeRID;
        public int StoreRID
        {
            get { return _storeRID; }
            set { _storeRID = value; }
        }

        /// <summary>
        /// VSW Store ID
        /// </summary>
        private string _storeID;
        public string StoreID
        {
            get { return _storeID; }
            set { _storeID = value; }
        }

        /// <summary>
        /// Column Index of Changed Column
        /// </summary>
        private int columnIndex;
        public int ColumnIndex
        {
            get { return columnIndex; }
            set { columnIndex = value; }
        }

        public long ChangeProfileKey
        {
            get
            {
                return ((((long)StoreRID) << 32) + (long)ColumnIndex);
            }
        }
    }

    [Serializable()]
    public class ChangeProfileElig : ChangeProfileBase      //  Store Eligibility
    {
        public ChangeProfileElig()
        { }

        public ChangeProfileElig(int storeRID, string oldValue, string newValue, string changedObjectName, int columnIndex)
        {
            StoreRID = storeRID;
            //StoreID = StoreID;
            OldValue = oldValue;
            NewValue = newValue;
            ChangedObjectName = changedObjectName;
            ColumnIndex = columnIndex;
        }
        /// <summary>
        /// Store RID
        /// </summary>
        private int _storeRID;
        public int StoreRID
        {
            get { return _storeRID; }
            set { _storeRID = value; }
        }

        /// <summary>
        /// VSW Store ID
        /// </summary>
        private string _storeID;
        public string StoreID
        {
            get { return _storeID; }
            set { _storeID = value; }
        }

        /// <summary>
        /// Column Index of Changed Column
        /// </summary>
        private int columnIndex;
        public int ColumnIndex
        {
            get { return columnIndex; }
            set { columnIndex = value; }
        }

        public long ChangeProfileKey
        {
            get
            {
                return ((((long)StoreRID) << 32) + (long)ColumnIndex);
            }
        }
    }

    [Serializable()]
    public class ChangeProfileDailyPct : ChangeProfileBase  //  Daily Percentages
    {
        public ChangeProfileDailyPct()
        { }

        public ChangeProfileDailyPct(int storeRID, string storeID, string oldValue, string newValue, string changedObjectName, int columnIndex)
        {
            StoreRID = storeRID;
            StoreID = storeID;
            OldValue = oldValue;
            NewValue = newValue;
            ChangedObjectName = changedObjectName;
            ColumnIndex = columnIndex;
        }

        /// <summary>
        /// Store RID
        /// </summary>
        private int _storeRID;
        public int StoreRID
        {
            get { return _storeRID; }
            set { _storeRID = value; }
        }

        /// <summary>
        /// VSW Store ID
        /// </summary>
        private string _storeID;
        public string StoreID
        {
            get { return _storeID; }
            set { _storeID = value; }
        }

        /// <summary>
        /// Column Index of Changed Column
        /// </summary>
        private int columnIndex;
        public int ColumnIndex
        {
            get { return columnIndex; }
            set { columnIndex = value; }
        }

        public long ChangeProfileKey
        {
            get
            {
                return ((((long)StoreRID) << 32) + (long)ColumnIndex);
            }
        }
    }

    [Serializable()]
    public class ChangeProfileChar : ChangeProfileBase      //  Characteristics
    {
        public ChangeProfileChar()
        { }

        public ChangeProfileChar(int scRID, int scgRID, string oldValue, string newValue, string changedObjectName, int columnIndex)
        {
            SC_RID = scRID;
            SCG_RID = scgRID;
            OldValue = oldValue;
            NewValue = newValue;
            ChangedObjectName = changedObjectName;
            ColumnIndex = columnIndex;
        }

        /// <summary>
        /// Store Characteristic RID
        /// </summary>
        private int _sc_RID;
        public int SC_RID
        {
            get { return _sc_RID; }
            set { _sc_RID = value; }
        }

        /// <summary>
        /// Store Characteristic Group RID
        /// </summary>
        private int _scg_RID;
        public int SCG_RID
        {
            get { return _scg_RID; }
            set { _scg_RID = value; }
        }

        /// <summary>
        /// Column Index of Changed Column
        /// </summary>
        private int columnIndex;
        public int ColumnIndex
        {
            get { return columnIndex; }
            set { columnIndex = value; }
        }

        public long ChangeProfileKey
        {
            get
            {
                return ((((long)SC_RID) << 32) + (long)ColumnIndex);
            }
        }
    }

    [Serializable()]
    public class ChangeProfileChainSet : ChangeProfileBase  //  Chain Set Percent
    {
        public ChangeProfileChainSet()
        { }

        public ChangeProfileChainSet(string oldValue, string newValue, string changedObjectName, int storeGroupListRID, int columnIndex, int timeID)
        {
            OldValue = oldValue;
            NewValue = newValue;
            ChangedObjectName = changedObjectName;
            ColumnIndex = columnIndex;
            SGL_RID = storeGroupListRID;
            TimeID = timeID;
        }

        /// <summary>
        /// Store Group List RID
        /// </summary>
        private int _timeID;
        public int TimeID
        {
            get { return _timeID; }
            set { _timeID = value; }
        }

        /// <summary>
        /// Store Group List RID
        /// </summary>
        private int _sglRID;
        public int SGL_RID
        {
            get { return _sglRID; }
            set { _sglRID = value; }
        }

        /// <summary>
        /// Column Index of Changed Column
        /// </summary>
        private int columnIndex;
        public int ColumnIndex
        {
            get { return columnIndex; }
            set { columnIndex = value; }
        }

        public long ChangeProfileKey
        {
            get
            {
                return ((((long)SGL_RID) << 32) + (long)ColumnIndex);
            }
        }
    }

    [Serializable()]
    public class ChangeProfilePurge : ChangeProfileBase     //  Purge Criteria
    {
        public ChangeProfilePurge()
        { }

        //  when a text field is first entered
        //  the old value is empty
        //  the new value is the textbox
        //  the change object name is the field for the database table
        public ChangeProfilePurge(string oldValue, string newValue, string changedObjectName)
        {
            OldValue = oldValue;
            NewValue = newValue;
            ChangedObjectName = changedObjectName;
        }
    }

    // END TT#2015 - gtaylor - Apply Changes to Lower Levels
}
