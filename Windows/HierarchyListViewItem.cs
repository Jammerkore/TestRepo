using System;
using System.Windows.Forms;

using MIDRetail.Common;
using MIDRetail.DataCommon;


namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for HierarchyListViewItem.
	/// </summary>
	public class HierarchyListViewItem : ListViewItem
	{
		private eChangeType					_levelChangeType;
		private string						_levelColorFile;
		private string						_levelColor;
		private string						_levelID;
		private int							_level;
		private eHierarchyLevelType			_levelType;
		private eLevelLengthType			_levelLengthType;
		private int							_levelRequiredSize;
		private int							_levelSizeRangeFrom;
		private int							_levelSizeRangeTo;
		//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
		//private int							_levelNodeCount;
		private bool						_levelNodesExist;
		//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
		private bool						_levelColorChanged;
		private bool						_levelDisplayOptionChanged;
		private eOTSPlanLevelType			_levelOTSPlanLevelType;
		private eHierarchyDisplayOptions	_levelDisplayOption;
		private eHierarchyIDFormat			_levelIDFormat;
		private ePurgeTimeframe				_purgeDailyHistoryTimeframe;
		private int							_purgeDailyHistory;
		private ePurgeTimeframe				_purgeWeeklyHistoryTimeframe;
		private int							_purgeWeeklyHistory;
		private ePurgeTimeframe				_purgePlansTimeframe;
		private int							_purgePlans;
		private ePurgeTimeframe				_purgeHeadersTimeframe;
		private int							_purgeHeaders;
		private bool						_newLevel;
        //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
        private ePurgeTimeframe _htASNTimeframe;
        private int _htASN;
        private ePurgeTimeframe _htAssortmentTimeframe;
        private int _htAssortment;
        private ePurgeTimeframe _htDropShipTimeframe;
        private int _htDropShip;
        private ePurgeTimeframe _htDummyTimeframe;
        private int _htDummy;
        private ePurgeTimeframe _htMultiHeaderTimeframe;
        private int _htMultiHeader;
        private ePurgeTimeframe _htPlaceholderTimeframe;
        private int _htPlaceholder;
        private ePurgeTimeframe _htReceiptTimeframe;
        private int _htReceipt;
        private ePurgeTimeframe _htPurchaseTimeframe;
        private int _htPurchase;
        private ePurgeTimeframe _htReserveTimeframe;
        private int _htReserve;
        private ePurgeTimeframe _htVSWTimeframe;
        private int _htVSW;
        private ePurgeTimeframe _htWorkupTotByTimeframe;
        private int _htWorkupTotBy;
        //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type			

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HierarchyListViewItem() : base()
		{
			LevelChangeType = eChangeType.none;
			LevelColorChanged = false;
			PurgeDailyHistoryTimeframe = ePurgeTimeframe.None;
//			PurgeDailyHistory = 0;
			PurgeDailyHistory = Include.Undefined;
			PurgeWeeklyHistoryTimeframe = ePurgeTimeframe.None;
//			PurgeWeeklyHistory = 0;
			PurgeWeeklyHistory = Include.Undefined;
			PurgePlansTimeframe = ePurgeTimeframe.None;
//			PurgePlans = 0;
			PurgePlans = Include.Undefined;
			PurgeHeadersTimeframe = ePurgeTimeframe.None;
			PurgeHeaders = Include.Undefined;
            NewLevel = false;

            //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
            HtASNTimeframe = ePurgeTimeframe.None;
            HtASN = Include.Undefined;
            HtAssortmentTimeframe = ePurgeTimeframe.None;
            HtAssortment = Include.Undefined;
            HtDropShipTimeframe = ePurgeTimeframe.None;
            HtDropShip = Include.Undefined;
            HtDummyTimeframe = ePurgeTimeframe.None;
            HtDummy = Include.Undefined;
            HtMultiHeaderTimeframe = ePurgeTimeframe.None;
            HtMultiHeader = Include.Undefined;
            HtPlaceholderTimeframe = ePurgeTimeframe.None;
            HtPlaceholder = Include.Undefined;
            HtReceiptTimeframe = ePurgeTimeframe.None;
            HtReceipt = Include.Undefined;
            HtPurchaseTimeframe = ePurgeTimeframe.None;
            HtPurchase = Include.Undefined;
            HtReserveTimeframe = ePurgeTimeframe.None;
            HtReserve = Include.Undefined;
            HtVSWTimeframe = ePurgeTimeframe.None;
            HtVSW = Include.Undefined;
            HtWorkupTotByTimeframe = ePurgeTimeframe.None;
            HtWorkupTotBy = Include.Undefined;
            //END TT#400-MD-VStuart-Add Header Purge Criteria by Header TypeHtASNTimeframe = ePurgeTimeframe.None;
		}

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HierarchyListViewItem(string Text, int ImageIndex) : base( Text, ImageIndex )
		{
			LevelChangeType = eChangeType.none;
			LevelColorChanged = false;
			PurgeDailyHistoryTimeframe = ePurgeTimeframe.None;
			PurgeDailyHistory = Include.Undefined;
			PurgeWeeklyHistoryTimeframe = ePurgeTimeframe.None;
			PurgeWeeklyHistory = Include.Undefined;
			PurgePlansTimeframe = ePurgeTimeframe.None;
			PurgePlans = Include.Undefined;
			PurgeHeadersTimeframe = ePurgeTimeframe.None;
			PurgeHeaders = Include.Undefined;
            //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
            HtASNTimeframe = ePurgeTimeframe.None;
            HtASN = Include.Undefined;
            HtAssortmentTimeframe = ePurgeTimeframe.None;
            HtAssortment = Include.Undefined;
            HtDropShipTimeframe = ePurgeTimeframe.None;
            HtDropShip = Include.Undefined;
            HtDummyTimeframe = ePurgeTimeframe.None;
            HtDummy = Include.Undefined;
            HtMultiHeaderTimeframe = ePurgeTimeframe.None;
            HtMultiHeader = Include.Undefined;
            HtPlaceholderTimeframe = ePurgeTimeframe.None;
            HtPlaceholder = Include.Undefined;
            HtReceiptTimeframe = ePurgeTimeframe.None;
            HtReceipt = Include.Undefined;
            HtPurchaseTimeframe = ePurgeTimeframe.None;
            HtPurchase = Include.Undefined;
            HtReserveTimeframe = ePurgeTimeframe.None;
            HtReserve = Include.Undefined;
            HtVSWTimeframe = ePurgeTimeframe.None;
            HtVSW = Include.Undefined;
            HtWorkupTotByTimeframe = ePurgeTimeframe.None;
            HtWorkupTotBy = Include.Undefined;
            //END TT#400-MD-VStuart-Add Header Purge Criteria by Header TypeHtASNTimeframe = ePurgeTimeframe.None;
            NewLevel = false;
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
		/// Gets or sets the file name for the folder of the level in the hierarchy.
		/// </summary>
		public string LevelColorFile 
		{
			get { return _levelColorFile ; }
			set { _levelColorFile = value; }
		}
		/// <summary>
		/// Gets or sets the color to use for the folder of the level in the hierarchy.
		/// </summary>
		public string LevelColor 
		{
			get { return _levelColor ; }
			set { _levelColor = value; }
		}
		/// <summary>
		/// Gets or sets the flag identifying if the color associated with the level has been changed.
		/// </summary>
		public bool LevelColorChanged 
		{
			get { return _levelColorChanged ; }
			set { _levelColorChanged = value; }
		}
		/// <summary>
		/// Gets or sets the ID (name) of the level in the hierarchy.
		/// </summary>
		public string LevelID 
		{
			get { return _levelID ; }
			set { _levelID = value; }
		}
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
		/// Gets or sets the OTS level type of this level in the hierarchy.
		/// </summary>
		public eHierarchyLevelType LevelType
		{
			get { return _levelType ; }
			set { _levelType = value; }
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
		public eOTSPlanLevelType LevelOTSPlanLevelType
		{
			get { return _levelOTSPlanLevelType ; }
			set { _levelOTSPlanLevelType = value; }
		}
		/// <summary>
		/// Gets or sets the count of the nodes at this level in the hierarchy.
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
		/// Gets or sets the display option of this level in the hierarchy.
		/// </summary>
		public eHierarchyDisplayOptions LevelDisplayOption
		{
			get { return _levelDisplayOption ; }
			set { _levelDisplayOption = value; }
		}
		/// <summary>
		/// Gets or sets the flag identifying if the format associated with the level has been changed.
		/// </summary>
		public bool LevelDisplayOptionChanged 
		{
			get { return _levelDisplayOptionChanged ; }
			set { _levelDisplayOptionChanged = value; }
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
		/// <summary>
		/// Gets or sets the timeframe of the header purge information of this level in the hierarchy.
		/// </summary>
		public ePurgeTimeframe PurgeHeadersTimeframe
		{
			get { return _purgeHeadersTimeframe ; }
			set { _purgeHeadersTimeframe = value; }
		}
		/// <summary>
		/// Gets or sets the time of the header purge information of this level in the hierarchy.
		/// </summary>
		public int PurgeHeaders
		{
			get { return _purgeHeaders ; }
			set { _purgeHeaders = value; }
		}
		/// <summary>
		/// Gets or sets the flag identifying if this is a new level level in the hierarchy.
		/// </summary>
		public bool NewLevel
		{
			get { return _newLevel ; }
			set { _newLevel = value; }
		}

        //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
        /// <summary>
        /// Gets or sets the timeframe of the ASN header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe HtASNTimeframe
        {
            get { return _htASNTimeframe; }
            set { _htASNTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the ASN header type purge information of this level in the hierarchy.
        /// </summary>
        public int HtASN
        {
            get { return _htASN; }
            set { _htASN = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the Assortment header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe HtAssortmentTimeframe
        {
            get { return _htAssortmentTimeframe; }
            set { _htAssortmentTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the Assortment header type purge information of this level in the hierarchy.
        /// </summary>
        public int HtAssortment
        {
            get { return _htAssortment; }
            set { _htAssortment = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the DropShip header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe HtDropShipTimeframe
        {
            get { return _htDropShipTimeframe; }
            set { _htDropShipTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the DropShip header type purge information of this level in the hierarchy.
        /// </summary>
        public int HtDropShip
        {
            get { return _htDropShip; }
            set { _htDropShip = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the Dummy header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe HtDummyTimeframe
        {
            get { return _htDummyTimeframe; }
            set { _htDummyTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the Dummy header type purge information of this level in the hierarchy.
        /// </summary>
        public int HtDummy
        {
            get { return _htDummy; }
            set { _htDummy = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the MultiHeader header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe HtMultiHeaderTimeframe
        {
            get { return _htMultiHeaderTimeframe; }
            set { _htMultiHeaderTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the MultiHeader header type purge information of this level in the hierarchy.
        /// </summary>
        public int HtMultiHeader
        {
            get { return _htMultiHeader; }
            set { _htMultiHeader = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the header purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe HtPlaceholderTimeframe
        {
            get { return _htPlaceholderTimeframe; }
            set { _htPlaceholderTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the header purge information of this level in the hierarchy.
        /// </summary>
        public int HtPlaceholder
        {
            get { return _htPlaceholder; }
            set { _htPlaceholder = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the Receipt header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe HtReceiptTimeframe
        {
            get { return _htReceiptTimeframe; }
            set { _htReceiptTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the Receipt header type purge information of this level in the hierarchy.
        /// </summary>
        public int HtReceipt
        {
            get { return _htReceipt; }
            set { _htReceipt = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the Purchase header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe HtPurchaseTimeframe
        {
            get { return _htPurchaseTimeframe; }
            set { _htPurchaseTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the Purchase header type purge information of this level in the hierarchy.
        /// </summary>
        public int HtPurchase
        {
            get { return _htPurchase; }
            set { _htPurchase = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the header purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe HtReserveTimeframe
        {
            get { return _htReserveTimeframe; }
            set { _htReserveTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the header purge information of this level in the hierarchy.
        /// </summary>
        public int HtReserve
        {
            get { return _htReserve; }
            set { _htReserve = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the header purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe HtVSWTimeframe
        {
            get { return _htVSWTimeframe; }
            set { _htVSWTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the header purge information of this level in the hierarchy.
        /// </summary>
        public int HtVSW
        {
            get { return _htVSW; }
            set { _htVSW = value; }
        }
        /// <summary>
        /// Gets or sets the timeframe of the WorkupTotBy header type purge information of this level in the hierarchy.
        /// </summary>
        public ePurgeTimeframe HtWorkupTotByTimeframe
        {
            get { return _htWorkupTotByTimeframe; }
            set { _htWorkupTotByTimeframe = value; }
        }
        /// <summary>
        /// Gets or sets the time of the WorkupTotBy header type purge information of this level in the hierarchy.
        /// </summary>
        public int HtWorkupTotBy
        {
            get { return _htWorkupTotBy; }
            set { _htWorkupTotBy = value; }
        }
        //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
	}
}
