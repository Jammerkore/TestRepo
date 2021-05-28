
using System;
using MIDRetail.DataCommon;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Globalization;

namespace Logility.ROWebSharedTypes
{
    // Hierarchy Property classes
    [DataContract(Name = "ROHierarchyPropertiesProfile", Namespace = "http://Logility.ROWeb/")]
    public class ROHierarchyPropertiesProfile : ROBaseProperties
    {
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _hierarchy;

        [DataMember(IsRequired = true)]
        private eProfileType _profileType;

        [DataMember(IsRequired = true)]
        private int _hierarchyRootNodeRID;

        [DataMember(IsRequired = true)]
        private string _hierarchyColor;

        [DataMember(IsRequired = true)]
        private eHierarchyType _hierarchyType;

        [DataMember(IsRequired = true)]
        private int _ownerKey;

        [DataMember(IsRequired = true)]
        private eHierarchyRollupOption _hierarchyRollupOption;

        [DataMember(IsRequired = true)]
        private eOTSPlanLevelType _planLevelType;

        [DataMember(IsRequired = true)]
        private string _postingDate;

        [DataMember(IsRequired = true)]
        List<ROHierarchyLevel> _levelList;

        public ROHierarchyPropertiesProfile(KeyValuePair<int, string> hierarchy)
        {
            _hierarchy = hierarchy;
            _profileType = eProfileType.Hierarchy;
            _hierarchyRootNodeRID = Include.NoRID;
            _hierarchyType = eHierarchyType.None;
            _ownerKey = Include.NoRID;
            _hierarchyRollupOption = eHierarchyRollupOption.Undefined;
            _planLevelType = eOTSPlanLevelType.Undefined;
            _levelList = new List<ROHierarchyLevel>();
        }

        public KeyValuePair<int, string> Hierarchy
        {
            get { return _hierarchy; }
            set { _hierarchy = value; }
        }

        public eProfileType ProfileType
        {
            get { return _profileType; }
            set { _profileType = value; }
        }

        public int HierarchyRootNodeRID
        {
            get { return _hierarchyRootNodeRID; }
            set { _hierarchyRootNodeRID = value; }
        }

        public string HierarchyColor
        {
            get { return _hierarchyColor; }
            set { _hierarchyColor = value; }
        }

        public eHierarchyType HierarchyType
        {
            get { return _hierarchyType; }
            set
            {
                _hierarchyType = value;
            }
        }

        public int OwnerKey
        {
            get { return _ownerKey; }
            set { _ownerKey = value; }
        }

        public eHierarchyRollupOption HierarchyRollupOption
        {
            get { return _hierarchyRollupOption; }
            set
            {
                _hierarchyRollupOption = value;
            }
        }

        public eOTSPlanLevelType PlanLevelType
        {
            get { return _planLevelType; }
            set
            {
                _planLevelType = value;
            }
        }

        public string PostingDate
        {
            get { return _postingDate; }
            set { _postingDate = value; }
        }

        public List<ROHierarchyLevel> LevelList
        {
            get { return _levelList; }
            set { _levelList = value; }
        }

        
    }

    [DataContract(Name = "ROHierarchyLevel", Namespace = "http://Logility.ROWeb/")]
    public class ROHierarchyLevel
    {
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _level;

        [DataMember(IsRequired = true)]
        private string _levelColor;

        [DataMember(IsRequired = true)]
        private eHierarchyLevelType _levelType;

        [DataMember(IsRequired = true)]
        private eLevelLengthType _levelLengthType;

        [DataMember(IsRequired = true)]
        private int _levelRequiredSize;

        [DataMember(IsRequired = true)]
        private int _levelSizeRangeFrom;

        [DataMember(IsRequired = true)]
        private int _levelSizeRangeTo;

        [DataMember(IsRequired = true)]
        private bool _levelNodesExist;

        [DataMember(IsRequired = true)]
        private eOTSPlanLevelType _levelOTSPlanLevelType;

        [DataMember(IsRequired = true)]
        private eHierarchyDisplayOptions _levelDisplayOption;

        [DataMember(IsRequired = true)]
        private eHierarchyIDFormat _levelIDFormat;

        [DataMember(IsRequired = true)]
        private int? _purgeDailyHistory;

        [DataMember(IsRequired = true)]
        private int? _purgeWeeklyHistory;

        [DataMember(IsRequired = true)]
        private int? _purgePlans;

        [DataMember(IsRequired = true)]
        private int? _purgeHeaderASN;

        [DataMember(IsRequired = true)]
        private int? _purgeHeaderAssortment;

        [DataMember(IsRequired = true)]
        private int? _purgeHeaderDropShip;

        [DataMember(IsRequired = true)]
        private int? _purgeHeaderDummy;

        [DataMember(IsRequired = true)]
        private int? _purgeHeaderMultiHeader;

        [DataMember(IsRequired = true)]
        private int? _purgeHeaderPlaceholder;

        [DataMember(IsRequired = true)]
        private int? _purgeHeaderReceipt;

        [DataMember(IsRequired = true)]
        private int? _purgeHeaderPurchaseOrder;

        [DataMember(IsRequired = true)]
        private int? _purgeHeaderReserve;

        [DataMember(IsRequired = true)]
        private int? _purgeHeaderVSW;

        [DataMember(IsRequired = true)]
        private int? _purgeHeaderWorkupTotalBuy;

        public ROHierarchyLevel(KeyValuePair<int, string> level)
        {
            _level = level;

            PurgeDailyHistory = null;
            PurgeWeeklyHistory = null;
            PurgePlans = null;
            PurgeHeaderASN = null;
            PurgeHeaderAssortment = null;
            PurgeHeaderDropShip = null;
            PurgeHeaderDummy = null;
            PurgeHeaderMultiHeader = null;
            PurgeHeaderPlaceholder = null;
            PurgeHeaderReceipt = null;
            PurgeHeaderPurchaseOrder = null;
            PurgeHeaderReserve = null;
            PurgeHeaderVSW = null;
            PurgeHeaderWorkupTotalBuy = null;

            _levelType = eHierarchyLevelType.Undefined;
            _levelLengthType = eLevelLengthType.unrestricted;
            _levelNodesExist = false;
            _levelOTSPlanLevelType = eOTSPlanLevelType.Undefined;
            _levelDisplayOption = eHierarchyDisplayOptions.IdAndName;
            _levelIDFormat = eHierarchyIDFormat.Unique;
        }

        /// <summary>
        /// Gets or sets the KeyValuePair containing the level index and ID.
        /// </summary>
        public KeyValuePair<int, string> Level
        {
            get { return _level; }
            set { _level = value; }
        }

        /// <summary>
		/// Gets or sets the color to use for the folder of the level in the hierarchy.
		/// </summary>
		public string LevelColor
        {
            get { return _levelColor; }
            set { _levelColor = value; }
        }

        /// <summary>
        /// Gets or sets the OTS level type of this level in the hierarchy.
        /// </summary>
        public eHierarchyLevelType LevelType
        {
            get { return _levelType; }
            set
            {
                _levelType = value;
            }
        }
        /// <summary>
        /// Gets or sets the type of length (unrestricted, required, or range) for the level in the hierarchy.
        /// </summary>
        public eLevelLengthType LevelLengthType
        {
            get { return _levelLengthType; }
            set
            {
                _levelLengthType = value;
            }
        }
        /// <summary>
        /// Gets or sets the required size of this level in the hierarchy.
        /// </summary>
        /// <remarks>
        /// This field is only used if the LevelLengthType is set to required.
        /// </remarks>
        public int LevelRequiredSize
        {
            get { return _levelRequiredSize; }
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
            get { return _levelSizeRangeFrom; }
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
            get { return _levelSizeRangeTo; }
            set { _levelSizeRangeTo = value; }
        }
        /// <summary>
        /// Gets or sets the OTS level type of this level in the hierarchy.
        /// </summary>
        public eOTSPlanLevelType LevelOTSPlanLevelType
        {
            get { return _levelOTSPlanLevelType; }
            set
            {
                _levelOTSPlanLevelType = value;
            }
        }
        public bool LevelNodesExist
        {
            get { return _levelNodesExist; }
            set { _levelNodesExist = value; }
        }
        /// <summary>
        /// Gets or sets the display option of this level in the hierarchy.
        /// </summary>
        public eHierarchyDisplayOptions LevelDisplayOption
        {
            get { return _levelDisplayOption; }
            set
            {
                _levelDisplayOption = value;
            }
        }
        /// <summary>
        /// Gets or sets the ID format of this level in the hierarchy.
        /// </summary>
        public eHierarchyIDFormat LevelIDFormat
        {
            get { return _levelIDFormat; }
            set
            {
                _levelIDFormat = value;
            }
        }
        /// <summary>
        /// Gets or sets the time of the history purge information of this level in the hierarchy.
        /// </summary>
        public int? PurgeDailyHistory
        {
            get { return _purgeDailyHistory; }
            set { _purgeDailyHistory = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the purge daily history is set.
        /// </summary>
        public bool PurgeDailyHistoryIsSet
        {
            get { return _purgeDailyHistory != null; }
        }
        /// <summary>
        /// Gets or sets the time of the forecast purge information of this level in the hierarchy.
        /// </summary>
        public int? PurgeWeeklyHistory
        {
            get { return _purgeWeeklyHistory; }
            set { _purgeWeeklyHistory = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the purge wekly history is set.
        /// </summary>
        public bool PurgeWeeklyHistoryIsSet
        {
            get { return _purgeWeeklyHistory != null; }
        }
        /// <summary>
        /// Gets or sets the time of the distro purge information of this level in the hierarchy.
        /// </summary>
        public int? PurgePlans
        {
            get { return _purgePlans; }
            set { _purgePlans = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the purge plans is set.
        /// </summary>
        public bool PurgePlansIsSet
        {
            get { return _purgePlans != null; }
        }
        /// <summary>
        /// Gets or sets the time of the ASN header type purge information of this level in the hierarchy.
        /// </summary>
        public int? PurgeHeaderASN
        {
            get { return _purgeHeaderASN; }
            set { _purgeHeaderASN = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the purge ASN headers is set.
        /// </summary>
        public bool PurgeHeaderASNIsSet
        {
            get { return _purgeHeaderASN != null; }
        }
        /// <summary>
        /// Gets or sets the time of the Assortment header type purge information of this level in the hierarchy.
        /// </summary>
        public int? PurgeHeaderAssortment
        {
            get { return _purgeHeaderAssortment; }
            set { _purgeHeaderAssortment = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the purge Assortment headers is set.
        /// </summary>
        public bool PurgeHeaderAssortmentIsSet
        {
            get { return _purgeHeaderAssortment != null; }
        }
        /// <summary>
        /// Gets or sets the time of the DropShip header type purge information of this level in the hierarchy.
        /// </summary>
        public int? PurgeHeaderDropShip
        {
            get { return _purgeHeaderDropShip; }
            set { _purgeHeaderDropShip = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the purge DropShip headers is set.
        /// </summary>
        public bool PurgeHeaderDropShipIsSet
        {
            get { return _purgeHeaderDropShip != null; }
        }
        /// <summary>
        /// Gets or sets the time of the Dummy header type purge information of this level in the hierarchy.
        /// </summary>
        public int? PurgeHeaderDummy
        {
            get { return _purgeHeaderDummy; }
            set { _purgeHeaderDummy = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the purge dummy headers is set.
        /// </summary>
        public bool PurgeHeaderDummyIsSet
        {
            get { return _purgeHeaderDummy != null; }
        }
        /// <summary>
        /// Gets or sets the time of the MultiHeader header type purge information of this level in the hierarchy.
        /// </summary>
        public int? PurgeHeaderMultiHeader
        {
            get { return _purgeHeaderMultiHeader; }
            set { _purgeHeaderMultiHeader = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the purge MultiHeader header is set.
        /// </summary>
        public bool PurgeHeaderMultiHeaderIsSet
        {
            get { return _purgeHeaderMultiHeader != null; }
        }
        /// <summary>
        /// Gets or sets the time of the placeholder header purge information of this level in the hierarchy.
        /// </summary>
        public int? PurgeHeaderPlaceholder
        {
            get { return _purgeHeaderPlaceholder; }
            set { _purgeHeaderPlaceholder = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the purge placeholder header is set.
        /// </summary>
        public bool PurgeHeaderPlaceholderIsSet
        {
            get { return _purgeHeaderPlaceholder != null; }
        }
        /// <summary>
        /// Gets or sets the time of the Receipt header type purge information of this level in the hierarchy.
        /// </summary>
        public int? PurgeHeaderReceipt
        {
            get { return _purgeHeaderReceipt; }
            set { _purgeHeaderReceipt = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the purge Receipt header is set.
        /// </summary>
        public bool PurgeHeaderReceiptIsSet
        {
            get { return _purgeHeaderReceipt != null; }
        }
        /// <summary>
        /// Gets or sets the time of the Purchase Order header type purge information of this level in the hierarchy.
        /// </summary>
        public int? PurgeHeaderPurchaseOrder
        {
            get { return _purgeHeaderPurchaseOrder; }
            set { _purgeHeaderPurchaseOrder = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the purge Purchase Order header is set.
        /// </summary>
        public bool PurgeHeaderPurchaseOrderIsSet
        {
            get { return _purgeHeaderPurchaseOrder != null; }
        }
        /// <summary>
        /// Gets or sets the time of the Reserve header purge information of this level in the hierarchy.
        /// </summary>
        public int? PurgeHeaderReserve
        {
            get { return _purgeHeaderReserve; }
            set { _purgeHeaderReserve = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the purge Reserve header is set.
        /// </summary>
        public bool PurgeHeaderReserveIsSet
        {
            get { return _purgeHeaderReserve != null; }
        }
        /// <summary>
        /// Gets or sets the time of the VSW header purge information of this level in the hierarchy.
        /// </summary>
        public int? PurgeHeaderVSW
        {
            get { return _purgeHeaderVSW; }
            set { _purgeHeaderVSW = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the purge VSW header is set.
        /// </summary>
        public bool PurgeHeaderVSWIsSet
        {
            get { return _purgeHeaderVSW != null; }
        }
        /// <summary>
        /// Gets or sets the time of the Workup Total Buy header type purge information of this level in the hierarchy.
        /// </summary>
        public int? PurgeHeaderWorkupTotalBuy
        {
            get { return _purgeHeaderWorkupTotalBuy; }
            set { _purgeHeaderWorkupTotalBuy = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the purge Workup Total Buy header is set.
        /// </summary>
        public bool PurgeHeaderWorkupTotalBuyIsSet
        {
            get { return _purgeHeaderWorkupTotalBuy != null; }
        }
    }


    // Node Property classes
    [DataContract(Name = "RONodeProperties", Namespace = "http://Logility.ROWeb/")]
    public class RONodeProperties : ROBaseProperties
    {
        [DataMember(IsRequired = true)]
        protected KeyValuePair<int, string> _node;


        [DataMember(IsRequired = true)]
        protected eProfileType _profileType;

        public RONodeProperties(eProfileType profileType, KeyValuePair<int, string> node)
        {
            _node = node;
            _profileType = profileType;
        }

        public KeyValuePair<int, string> Node
        {
            get { return _node; }
            set { _node = value; }
        }

        public eProfileType ProfileType
        {
            get { return _profileType; }
            set { _profileType = value; }
        }

    }

    [DataContract(Name = "RONodePropertiesProfile", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesProfile : RONodeProperties
    {
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _parent;

        [DataMember(IsRequired = true)]
        bool _isHierarchyNode;

        [DataMember(IsRequired = true)]
        string _nodeDescription;

        [DataMember(IsRequired = true)]
        string _nodeID;

        [DataMember(IsRequired = true)]
        string _nodeName;

        [DataMember(IsRequired = true)]
        eProductType _productType;

        [DataMember(IsRequired = true)]
        eProductType _originalProductType;

        [DataMember(IsRequired = true)]
        eHierarchyLevelType _levelType;

        [DataMember(IsRequired = true)]
        string _colorGroup;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _productTypeInheritedFromNode;

        [DataMember(IsRequired = true)]
        bool _isActive;

        [DataMember(IsRequired = true)]
        bool _isAllowApply;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _applyNodePropertiesFromNode;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _applyNodePropertiesFromNodeInheritedFromNode;

        [DataMember(IsRequired = true)]
        ePlanLevelSelectType _OTSForecastSelectType;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _OTSForecastAnchorNode;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _OTSForecastLevel;

        [DataMember(IsRequired = true)]
        List<KeyValuePair<int, string>> _OTSForecastLevelList;

        [DataMember(IsRequired = true)]
        eMaskField _OTSForecastMaskType;

        [DataMember(IsRequired = true)]
        string _OTSForecastMask;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _OTSForecastInheritedFromNode;

        [DataMember(IsRequired = true)]
        eOTSPlanLevelType _OTSForecastType;

        [DataMember(IsRequired = true)]
        eOTSPlanLevelType _originalOTSForecastType;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _OTSForecastTypeInheritedFromNode;

        #region Public Properties

        /// <summary>
        /// Gets or sets the home hierarchy parent
        /// </summary>
        public KeyValuePair<int, string> Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the parent is set.
        /// </summary>
        public bool ParentIsSet
        {
            get { return !_parent.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets or sets a flag identifying if the node is the main hierarchy node.
        /// </summary>
        public bool IsHierarchyNode
        {
            get { return _isHierarchyNode; }
            set { _isHierarchyNode = value; }
        }
        /// <summary>
        /// Gets or sets the description of the node.
        /// </summary>
        public string NodeDescription
        {
            get { return _nodeDescription; }
            set { _nodeDescription = value; }
        }
        /// <summary>
        /// Gets or sets the ID of the node.
        /// </summary>
        public string NodeID
        {
            get { return _nodeID; }
            set { _nodeID = value; }
        }
        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        public string NodeName
        {
            get { return _nodeName; }
            set { _nodeName = (value == null) ? value : value.Trim(); }
        }
        /// <summary>
        /// Gets or sets the product type of the node.
        /// </summary>
        public eProductType ProductType
        {
            get { return _productType; }
            set
            {
                _productType = value;
                if (_originalProductType == eProductType.Undefined)
                {
                    _originalProductType = _productType;
                }
            }
        }
        /// <summary>
        /// Gets the original product type of the node.
        /// </summary>
        public eProductType OriginalProductType
        {
            get { return _originalProductType; }
        }
        /// <summary>
        /// Gets or sets the level type of the node.
        /// </summary>
        public eHierarchyLevelType LevelType
        {
            get { return _levelType; }
            set
            {
                _levelType = value;
            }
        }
        /// <summary>
        /// Gets or sets the color group of the node if the LevelType is color.
        /// </summary>
        public string ColorGroup
        {
            get { return _colorGroup; }
            set { _colorGroup = value; }
        }
        /// <summary>
		/// Gets a flag identifying if the product type is inherited.
		/// </summary>
		public bool ProductTypeIsInherited
        {
            get { return !ProductTypeInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the product type is inherited from.
        /// </summary>
        public KeyValuePair<int, string> ProductTypeInheritedFromNode
        {
            get { return _productTypeInheritedFromNode; }
            set { _productTypeInheritedFromNode = value; }
        }
        /// <summary>
		/// Gets or sets a flag identifying if the node is active.
		/// </summary>
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }
        /// <summary>
		/// Gets or sets a flag identifying if the apply node properties information can be set on the node.
		/// </summary>
        public bool IsAllowApply
        {
            get { return _isAllowApply; }
            set { _isAllowApply = value; }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the node properties are to be applied from.
        /// </summary>
        public KeyValuePair<int, string> ApplyNodePropertiesFromNode
        {
            get { return _applyNodePropertiesFromNode; }
            set { _applyNodePropertiesFromNode = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the apply node properties from node is set.
        /// </summary>
        public bool ApplyNodePropertiesFromNodeIsSet
        {
            get { return !ApplyNodePropertiesFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
		/// Gets a flag identifying if the apply node properties from is inherited.
		/// </summary>
		public bool ApplyNodePropertiesFromNodeIsInherited
        {
            get { return !ApplyNodePropertiesFromNodeInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the apply node properties from node is inherited from.
        /// </summary>
        public KeyValuePair<int, string> ApplyNodePropertiesFromNodeInheritedFromNode
        {
            get { return _applyNodePropertiesFromNodeInheritedFromNode; }
            set { _applyNodePropertiesFromNodeInheritedFromNode = value; }
        }
        /// <summary>
        /// Gets or sets the select type to be used to locate the plan level node.
        /// </summary>
        public ePlanLevelSelectType OTSForecastSelectType
        {
            get { return _OTSForecastSelectType; }
            set
            {
                _OTSForecastSelectType = value;
            }
        }
        /// <summary>
        /// Gets or sets the anchor node to be used to locate the plan level node.
        /// </summary>
        public KeyValuePair<int, string> OTSForecastAnchorNode
        {
            get { return _OTSForecastAnchorNode; }
            set { _OTSForecastAnchorNode = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the plan level anchor node is set.
        /// </summary>
        public bool OTSForecastAnchorNodeIsSet
        {
            get { return !_OTSForecastAnchorNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the level to be used to locate the plan level node.
        /// </summary>
        public KeyValuePair<int, string> OTSForecastLevel
        {
            get { return _OTSForecastLevel; }
            set { _OTSForecastLevel = value; }
        }
        /// <summary>
        /// Gets the list of plan level level values.
        /// </summary>
        public List<KeyValuePair<int, string>> OTSForecastLevelList
        {
            get { return _OTSForecastLevelList; }
        }
        /// <summary>
        /// Gets the flag identifying if the plan level level is set.
        /// </summary>
        public bool OTSForecastLevelIsSet
        {
            get { return !OTSForecastLevel.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the plan level mask type to be used to search for the plan level.
        /// </summary>
        public eMaskField OTSForecastMaskType
        {
            get { return _OTSForecastMaskType; }
            set
            {
                _OTSForecastMaskType = value;
            }
        }
        /// <summary>
        /// Gets or sets the plan level mask to be used to search for the plan level.
        /// </summary>
        public string OTSForecastMask
        {
            get { return _OTSForecastMask; }
            set { _OTSForecastMask = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the plan level mask is set.
        /// </summary>
        public bool OTSForecastMaskIsSet
        {
            get { return OTSForecastMask != null; }
        }
        /// <summary>
        /// Gets the flag identifying if the plan level type is inherited.
        /// </summary>
        public bool OTSForecastIsInherited
        {
            get { return !OTSForecastInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the plan level is inherited from.
        /// </summary>
        public KeyValuePair<int, string> OTSForecastInheritedFromNode
        {
            get { return _OTSForecastInheritedFromNode; }
            set { _OTSForecastInheritedFromNode = value; }
        }
        /// <summary>
        /// Gets or sets the OTSForecastType of the node
        /// </summary>
        public eOTSPlanLevelType OTSForecastType
        {
            get { return _OTSForecastType; }
            set
            {
                _OTSForecastType = value;
                if (_originalOTSForecastType == eOTSPlanLevelType.Undefined)
                {
                    _originalOTSForecastType = _OTSForecastType;
                }
            }
        }
        /// <summary>
        /// Gets the original OTSForecastType of the node
        /// </summary>
        public eOTSPlanLevelType OriginalOTSForecastType
        {
            get { return _originalOTSForecastType; }
        }
        /// <summary>
        /// Gets the flag identifying if the plan level type is inherited.
        /// </summary>
        public bool OTSForecastTypeIsInherited
        {
            get { return !OTSForecastTypeInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the plan level type is inherited from.
        /// </summary>
        public KeyValuePair<int, string> OTSForecastTypeInheritedFromNode
        {
            get { return _OTSForecastTypeInheritedFromNode; }
            set { _OTSForecastTypeInheritedFromNode = value; }
        }

        #endregion
        public RONodePropertiesProfile(KeyValuePair<int, string> node) :
            base(eProfileType.HierarchyNode, node)

        {
            _parent = default(KeyValuePair<int, string>);
            _productTypeInheritedFromNode = default(KeyValuePair<int, string>);
            _applyNodePropertiesFromNode = default(KeyValuePair<int, string>);
            _applyNodePropertiesFromNodeInheritedFromNode = default(KeyValuePair<int, string>);
            _OTSForecastInheritedFromNode = default(KeyValuePair<int, string>);
            _OTSForecastTypeInheritedFromNode = default(KeyValuePair<int, string>);
            _OTSForecastAnchorNode = default(KeyValuePair<int, string>);
            _OTSForecastLevel = default(KeyValuePair<int, string>);
            _OTSForecastLevelList = new List<KeyValuePair<int, string>>();
            _OTSForecastMask = null;
            _originalOTSForecastType = eOTSPlanLevelType.Undefined;
            _originalProductType = eProductType.Undefined;
        }
    }

    [DataContract(Name = "RONodePropertiesEligibilityValues", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesEligibilityValues
    {
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _eligibilityInheritedFromNode;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _eligibilityModel;
        [DataMember(IsRequired = true)]
        private bool? _storeIneligible;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _stockModifierInheritedFromNode;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _stockModifierModel;
        [DataMember(IsRequired = true)]
        private double? _stockModifierPct;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _StockLeadWeeksInheritedFromNode;
        [DataMember(IsRequired = true)]
        private int? _StockLeadWeeks;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _salesModifierInheritedFromNode;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _salesModifierModel;
        [DataMember(IsRequired = true)]
        private double? _salesModifierPct;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _FWOSModifierInheritedFromNode;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _FWOSModifierModel;
        [DataMember(IsRequired = true)]
        private double? _FWOSModifierPct;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _similarStoreInheritedFromNode;
        [DataMember(IsRequired = true)]
        private List<KeyValuePair<int,string>> _similarStores;
        [DataMember(IsRequired = true)]
        private double? _similarStoreRatio;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _similarStoreUntilDateRange;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _presentationPlusSalesInheritedFromNode;
        [DataMember(IsRequired = true)]
        private bool? _presentationPlusSalesInd;


        #region Public Properties

        /// <summary>
		/// Gets a flag identifying if a store's eligibility information is inherited.
		/// </summary>
		public bool EligibilityIsInherited
        {
            get { return !EligibilityInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the store's eligibility information is inherited from.
        /// </summary>
        public KeyValuePair<int, string> EligibilityInheritedFromNode
        {
            get { return _eligibilityInheritedFromNode; }
            set { _eligibilityInheritedFromNode = value; }
        }

        /// <summary>
        /// Gets if the type of eligibility is set.
        /// </summary>
        public bool EligibilityTypeIsSet
        {
            get
            {
                if (StoreIneligibleIsSet)
                {
                    return true;
                }
                else if (EligibilityModelIsSet)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Gets or sets the type of eligibility.
        /// </summary>
        public eEligibilitySettingType EligibilityType
        {
            get
            {
                if (StoreIneligibleIsSet)
                {
                    return eEligibilitySettingType.SetEligible;
                }
                else if (EligibilityModelIsSet)
                {
                    return eEligibilitySettingType.Model;
                }
                else
                {
                    return eEligibilitySettingType.None;
                }
            }
        }

        /// <summary>
        /// Gets or sets the record id of the elig model for the store.
        /// </summary>
        public KeyValuePair<int, string> EligibilityModel
        {
            get { return _eligibilityModel; }
            set { _eligibilityModel = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the eligibility model has been set.
        /// </summary>
        public bool EligibilityModelIsSet
        {
            get { return !_eligibilityModel.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the flag identifying that a store is ineligible.
        /// </summary>
        public bool? StoreIneligible
        {
            get { return _storeIneligible; }
            set { _storeIneligible = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the store ineligibility option has been set.
        /// </summary>
        public bool StoreIneligibleIsSet
        {
            get { return _storeIneligible != null; }
        }
        /// <summary>
        /// Gets a flag identifying if a store's stock modifier information is inherited.
        /// </summary>
        public bool StockModifierIsInherited
        {
            get { return !StockModifierInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the store's stock modifier information is inherited from.
        /// </summary>
        public KeyValuePair<int, string> StockModifierInheritedFromNode
        {
            get { return _stockModifierInheritedFromNode; }
            set { _stockModifierInheritedFromNode = value; }
        }
        /// <summary>
        /// Gets if the type of stock modifier is set.
        /// </summary>
        public bool StockModifierTypeIsSet
        {
            get
            {
                if (StockModifierPctIsSet)
                {
                    return true;
                }
                else if (StockModifierModelIsSet)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Gets or sets the type of stock modifier.
        /// </summary>
        public eModifierType StockModifierType
        {
            get
            {
                if (StockModifierPctIsSet)
                {
                    return eModifierType.Percent;
                }
                else if (StockModifierModelIsSet)
                {
                    return eModifierType.Model;
                }
                else
                {
                    return eModifierType.None;
                }
            }
        }
        /// <summary>
        /// Gets or sets the record id for the stock modifier model.
        /// </summary>
        public KeyValuePair<int, string> StockModifierModel
        {
            get { return _stockModifierModel; }
            set { _stockModifierModel = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the stock modifier model has been set.
        /// </summary>
        public bool StockModifierModelIsSet
        {
            get { return !_stockModifierModel.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the stock modifier percent for the store if a model is not used.
        /// </summary>
        public double? StockModifierPct
        {
            get { return _stockModifierPct; }
            set { _stockModifierPct = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the stock modifier percent has been set.
        /// </summary>
        public bool StockModifierPctIsSet
        {
            get { return _stockModifierPct != null; }
        }
        /// <summary>
        /// Gets or sets the stock modifier text.
        /// </summary>
        public string StockModifierText
        {
            get
            {
                if (StockModifierModelIsSet)
                {
                    return _stockModifierModel.Value;
                }
                else if (StockModifierPctIsSet)
                {
                    return _stockModifierPct.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// Gets a flag identifying if a store's Stock Lead Weeks information is inherited.
        /// </summary>
        public bool StockLeadWeeksInherited
        {
            get { return !StockLeadWeeksInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the store's Stock Lead Weeks information is inherited from.
        /// </summary>
        public KeyValuePair<int, string> StockLeadWeeksInheritedFromNode
        {
            get { return _StockLeadWeeksInheritedFromNode; }
            set { _StockLeadWeeksInheritedFromNode = value; }
        }

        /// <summary>
        /// Gets or sets the stock lead weeks a new store will recieve in a new store inventory plan.
        /// </summary>
        public int? StockLeadWeeks
        {
            get { return _StockLeadWeeks; }
            set { _StockLeadWeeks = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the stock lead weeks has been set.
        /// </summary>
        public bool StockLeadWeeksIsSet
        {
            get { return _StockLeadWeeks != null; }
        }
        /// <summary>
        /// Gets a flag identifying if a store's sales modifier information is inherited.
        /// </summary>
        public bool SalesModifierIsInherited
        {
            get { return !SalesModifierInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the store's sales modifier information is inherited from.
        /// </summary>
        public KeyValuePair<int, string> SalesModifierInheritedFromNode
        {
            get { return _salesModifierInheritedFromNode; }
            set { _salesModifierInheritedFromNode = value; }
        }
        /// <summary>
        /// Gets if the sales modifier type is set.
        /// </summary>
        public bool SalesModifierTypeIsSet
        {
            get
            {
                if (SalesModifierPctIsSet)
                {
                    return true;
                }
                else if (SalesModifierModelIsSet)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Gets or sets the sales modifier.
        /// </summary>
        public eModifierType SalesModifierType
        {
            get
            {
                if (SalesModifierPctIsSet)
                {
                    return eModifierType.Percent;
                }
                else if (SalesModifierModelIsSet)
                {
                    return eModifierType.Model;
                }
                else
                {
                    return eModifierType.None;
                }
            }
        }
        /// <summary>
        /// Gets or sets the record id for the sales modifier model.
        /// </summary>
        public KeyValuePair<int, string> SalesModifierModel
        {
            get { return _salesModifierModel; }
            set { _salesModifierModel = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the sales modifier model has been set.
        /// </summary>
        public bool SalesModifierModelIsSet
        {
            get { return !_salesModifierModel.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the sales modifier percent for the store if a model is not used.
        /// </summary>
        public double? SalesModifierPct
        {
            get { return _salesModifierPct; }
            set { _salesModifierPct = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the sales modifier percent has been set.
        /// </summary>
        public bool SalesModifierPctIsSet
        {
            get { return _salesModifierPct != null; }
        }
        /// <summary>
        /// Gets or sets the sales modifier text.
        /// </summary>
        public string SalesModifierText
        {
            get
            {
                if (SalesModifierModelIsSet)
                {
                    return _salesModifierModel.Value;
                }
                else if (SalesModifierPctIsSet)
                {
                    return _salesModifierPct.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets a flag identifying if a store's FWOS modifier information is inherited.
        /// </summary>
        public bool FWOSModifierIsInherited
        {
            get { return !FWOSModifierInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }

        }
        /// <summary>
        /// Gets or sets the record ID of the node where the store's FWOS modifier information is inherited from.
        /// </summary>
        public KeyValuePair<int, string> FWOSModifierInheritedFromNode
        {
            get { return _FWOSModifierInheritedFromNode; }
            set { _FWOSModifierInheritedFromNode = value; }
        }
        /// <summary>
        /// Gets if the FWOS modifier is set.
        /// </summary>
        public bool FWOSModifierTypeIsSet
        {
            get
            {
                if (FWOSModifierPctIsSet)
                {
                    return true;
                }
                else if (FWOSModifierModelIsSet)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets or sets the FWOS modifier.
        /// </summary>
        public eModifierType FWOSModifierType
        {
            get
            {
                if (FWOSModifierPctIsSet)
                {
                    return eModifierType.Percent;
                }
                else if (FWOSModifierModelIsSet)
                {
                    return eModifierType.Model;
                }
                else
                {
                    return eModifierType.None;
                }
            }
        }
        /// <summary>
        /// Gets or sets the record id for the FWOS modifier model.
        /// </summary>
        public KeyValuePair<int, string> FWOSModifierModel
        {
            get { return _FWOSModifierModel; }
            set { _FWOSModifierModel = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the FWOS modifier model has been set.
        /// </summary>
        public bool FWOSModifierModelIsSet
        {
            get { return !_FWOSModifierModel.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the FWOS modifier percent for the store if a model is not used.
        /// </summary>
        public double? FWOSModifierPct
        {
            get { return _FWOSModifierPct; }
            set { _FWOSModifierPct = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the FWOS modifier percent has been set.
        /// </summary>
        public bool FWOSModifierPctIsSet
        {
            get { return _FWOSModifierPct != null; }
        }
        /// <summary>
        /// Gets or sets the FWOS modifier text.
        /// </summary>
        public string FWOSModifierText
        {
            get
            {
                if (FWOSModifierModelIsSet)
                {
                    return _FWOSModifierModel.Value;
                }
                else if (FWOSModifierPctIsSet)
                {
                    return _FWOSModifierPct.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// Gets a flag identifying if a store's similar store information is inherited.
        /// </summary>
        public bool SimilarStoreIsInherited
        {
            get { return !SimilarStoreInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the store's similar store information is inherited from.
        /// </summary>
        public KeyValuePair<int, string> SimilarStoreInheritedFromNode
        {
            get { return _similarStoreInheritedFromNode; }
            set { _similarStoreInheritedFromNode = value; }
        }
        /// <summary>
        /// Gets or sets the type of simlar store identified for the store.
        /// </summary>
        public eSimilarStoreType SimilarStoreType
        {
            get
            {
                if (_similarStores.Count > 0)
                {
                    return eSimilarStoreType.Stores;
                }
                else
                {
                    return eSimilarStoreType.None;
                }
            }
        }
        /// <summary>
        /// Gets or sets the list of simlar stores identified for the store.
        /// </summary>
        public List<KeyValuePair<int, string>> SimilarStores
        {
            get { return _similarStores; }
            set { _similarStores = value; }
        }
        /// <summary>
        /// Gets or sets the ratio of simlar store for the store.
        /// </summary>
        public double? SimilarStoreRatio
        {
            get { return _similarStoreRatio; }
            set { _similarStoreRatio = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the similar store ratio has been set.
        /// </summary>
        public bool SimilarStoreRatioIsSet
        {
            get { return _similarStoreRatio != null; }
        }
        /// <summary>
        /// Gets or sets the record id of the date range to use for the similar store.
        /// </summary>
        public KeyValuePair<int, string> SimilarStoreUntilDateRange
        {
            get { return _similarStoreUntilDateRange; }
            set { _similarStoreUntilDateRange = value; }
        }
        /// <summary>
        /// Gets a flag identifying if a store's similar store until date is set.
        /// </summary>
        public bool SimilarStoreUntilDateRangeIsSet
        {
            get { return !_similarStoreUntilDateRange.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the similar store text.
        /// </summary>
        public string SimilarStoreText
        {
            get
            {
                if (_similarStores.Count > 1)
                {
                    return "(Average of stores)";
                }
                else if (_similarStores.Count > 0)
                {
                    return _similarStores[0].Value;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// Gets a flag identifying if the presentation plus sales indicator was inherited.
        /// </summary>
        public bool PresentationPlusSalesIsInherited
        {
            get { return !PresentationPlusSalesInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the flag identifying if the presentation plus sales indicator was inherited.
        /// </summary>
        public KeyValuePair<int, string> PresentationPlusSalesInheritedFromNode
        {
            get { return _presentationPlusSalesInheritedFromNode; }
            set { _presentationPlusSalesInheritedFromNode = value; }
        }
        /// <summary>
        /// Gets or sets the flag identifying if the presentation plus sales option is to be used.
        /// </summary>
        public bool? PresentationPlusSalesInd
        {
            get { return _presentationPlusSalesInd; }
            set { _presentationPlusSalesInd = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the presentation plus sales option has been set.
        /// </summary>
        public bool PresentationPlusSalesIndIsSet
        {
            get { return _presentationPlusSalesInd != null; }
        }

        #endregion

        public RONodePropertiesEligibilityValues()
        {
            Clear();
        }

        public void Clear()
        {
            _eligibilityInheritedFromNode = default(KeyValuePair<int, string>);
            _eligibilityModel = default(KeyValuePair<int, string>);
            _storeIneligible = null;

            _stockModifierInheritedFromNode = default(KeyValuePair<int, string>);
            _stockModifierModel = default(KeyValuePair<int, string>);
            _stockModifierPct = null;
            _StockLeadWeeksInheritedFromNode = default(KeyValuePair<int, string>);
            _StockLeadWeeks = null;
            _salesModifierInheritedFromNode = default(KeyValuePair<int, string>);
            _salesModifierModel = default(KeyValuePair<int, string>);
            _salesModifierPct = null;

            _FWOSModifierInheritedFromNode = default(KeyValuePair<int, string>);
            _FWOSModifierModel = default(KeyValuePair<int, string>);
            _FWOSModifierPct = null;

            _similarStoreInheritedFromNode = default(KeyValuePair<int, string>);
            _similarStores = new List<KeyValuePair<int, string>>();
            _similarStoreRatio = null;
            _similarStoreUntilDateRange = default(KeyValuePair<int, string>);
            _eligibilityInheritedFromNode = default(KeyValuePair<int, string>);

            _presentationPlusSalesInd = null;
            _presentationPlusSalesInheritedFromNode = default(KeyValuePair<int, string>);
        }
    }

    [DataContract(Name = "RONodePropertiesEligibilityStore", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesEligibilityStore
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _store;
        [DataMember(IsRequired = true)]
        RONodePropertiesEligibilityValues _eligibilityValues;


        #region Public Properties

        public KeyValuePair<int, string> Store
        {
            get { return _store; }
        }

        public RONodePropertiesEligibilityValues EligibilityValues
        {
            get { return _eligibilityValues; }
        }


        #endregion

        public RONodePropertiesEligibilityStore(KeyValuePair<int, string> store)
        {
            _store = store;
            _eligibilityValues = new RONodePropertiesEligibilityValues();
        }
    }

    [DataContract(Name = "RONodePropertiesEligibilityAttributeSet", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesEligibilityAttributeSet
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attributeSet;

        [DataMember(IsRequired = true)]
        RONodePropertiesEligibilityValues _eligibilityValues;

        [DataMember(IsRequired = true)]
        List<RONodePropertiesEligibilityStore> _eligibilityStore;

        #region Public Properties

        public KeyValuePair<int, string> AttributeSet
        {
            get { return _attributeSet; }
        }

        public RONodePropertiesEligibilityValues EligibilityValues
        {
            get { return _eligibilityValues; }
        }

        public List<RONodePropertiesEligibilityStore> EligibilityStore
        {
            get { return _eligibilityStore; }
        }

        #endregion

        public RONodePropertiesEligibilityAttributeSet(KeyValuePair<int, string> attributeSet)
        {
            _attributeSet = attributeSet;
            _eligibilityValues = new RONodePropertiesEligibilityValues();
            _eligibilityStore = new List<RONodePropertiesEligibilityStore>();
        }
    }

    [DataContract(Name = "RONodePropertiesEligibility", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesEligibility : RONodeProperties
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attribute;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attributeSet;

        [DataMember(IsRequired = true)]
        RONodePropertiesEligibilityAttributeSet _eligibilityAttributeSet = null;

        #region Public Properties

        /// <summary>
        /// Gets the flag identifying if the attribute has been set.
        /// </summary>
        public bool AttributeIsSet
        {
            get { return !Attribute.Equals(default(KeyValuePair<int, string>)); }
        }

        public KeyValuePair<int, string> Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

        /// <summary>
        /// Gets the flag identifying if the attribute has been set.
        /// </summary>
        public bool AttributeSetIsSet
        {
            get { return !AttributeSet.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets or sets the attribut for the criteria.
        /// </summary>
        public KeyValuePair<int, string> AttributeSet
        {
            get { return _attributeSet; }
            set { _attributeSet = value; }
        }

        public RONodePropertiesEligibilityAttributeSet EligibilityAttributeSet
        {
            get { return _eligibilityAttributeSet; }
            set { _eligibilityAttributeSet = value; }
        }

        #endregion
        public RONodePropertiesEligibility(KeyValuePair<int, string> node,
             KeyValuePair<int, string> attribute = default(KeyValuePair<int, string>),
             KeyValuePair<int, string> attributeSet = default(KeyValuePair<int, string>)) :
            base(eProfileType.StoreEligibility, node)

        {
            _attribute = attribute;
            _attributeSet = attributeSet;
        }
    }


    [DataContract(Name = "RONodePropertiesCharacteristicsValue", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesCharacteristicsValue
    {
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _characteristic;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _characteristicInheritedFromNode;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _characteristicValue;
        [DataMember(IsRequired = true)]
        private List<KeyValuePair<int, string>> _characteristicValues;

        #region Public Properties

        /// <summary>
        /// Gets the characteristic.
        /// </summary>
        public KeyValuePair<int, string> Characteristic
        {
            get { return _characteristic; }
        }
        /// <summary>
		/// Gets a flag identifying if characteristic is inherited.
		/// </summary>
		public bool CharacteristicIsInherited
        {
            get { return !CharacteristicInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the characteristic is inherited from.
        /// </summary>
        public KeyValuePair<int, string> CharacteristicInheritedFromNode
        {
            get { return _characteristicInheritedFromNode; }
            set { _characteristicInheritedFromNode = value; }
        }

        /// <summary>
        /// Gets or sets the characteristic value.
        /// </summary>
        public KeyValuePair<int, string> CharacteristicValue
        {
            get { return _characteristicValue; }
            set { _characteristicValue = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the characteristic value has been set.
        /// </summary>
        public bool CharacteristicValueIsSet
        {
            get { return !_characteristicValue.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets the characteristic values.
        /// </summary>
        public List<KeyValuePair<int, string>> CharacteristicValues
        {
            get { return _characteristicValues; }
        }

        #endregion

        public RONodePropertiesCharacteristicsValue(KeyValuePair<int, string> characteristic)
        {
            _characteristic = characteristic;
            _characteristicValue = default(KeyValuePair<int, string>);
            _characteristicInheritedFromNode = default(KeyValuePair<int, string>);
            _characteristicValues = new List<KeyValuePair<int, string>>();
        }    
    }

    [DataContract(Name = "RONodePropertiesCharacteristics", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesCharacteristics : RONodeProperties
    {
        [DataMember(IsRequired = true)]
        List<RONodePropertiesCharacteristicsValue> _characteristics;

        #region Public Properties

        /// <summary>
        /// Gets the list of characteristics of the node
        /// </summary>
        public List<RONodePropertiesCharacteristicsValue> Characteristics
        {
            get { return _characteristics; }
        }

        #endregion
        public RONodePropertiesCharacteristics(KeyValuePair<int, string> node) :
            base(eProfileType.ProductCharacteristic, node)

        {
            _characteristics = new List<RONodePropertiesCharacteristicsValue>();
        }
    }

    [DataContract(Name = "RONodePropertiesVelocityGrades", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesVelocityGrades : RONodeProperties
    {


        [DataMember(IsRequired = true)]
        List<RONodePropertiesVelocityGrade> _velocityGrades;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _velocityGradesInheritedFromNode;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _minimumMaximumsInheritedFromNode;

        [DataMember(IsRequired = true)]
        List<ROSellThruList> _sellThruPercents;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _sellThruPercentsInheritedFromNode;

        #region Public Properties
        /// <summary>
        /// Gets the list of velocity grades of the node
        /// </summary>
        public List<RONodePropertiesVelocityGrade> VelocityGrades
        {
            get { return _velocityGrades; }
        }

        /// <summary>
		/// Gets a flag identifying if the velocity grades are inherited.
		/// </summary>
		public bool VelocityGradesIsInherited
        {
            get { return !VelocityGradesInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the velocity grades are inherited from.
        /// </summary>
        public KeyValuePair<int, string> VelocityGradesInheritedFromNode
        {
            get { return _velocityGradesInheritedFromNode; }
            set { _velocityGradesInheritedFromNode = value; }
        }
        /// <summary>
		/// Gets a flag identifying if the velocity grades are inherited.
		/// </summary>
		public bool MinimumMaximumsIsInherited
        {
            get { return !MinimumMaximumsInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the velocity grades are inherited from.
        /// </summary>
        public KeyValuePair<int, string> MinimumMaximumsInheritedFromNode
        {
            get { return _minimumMaximumsInheritedFromNode; }
            set { _minimumMaximumsInheritedFromNode = value; }
        }

        /// <summary>
        /// Gets the list of sell thru percents of the node
        /// </summary>
        public List<ROSellThruList> SellThruPercents
        {
            get { return _sellThruPercents; }
        }
        /// <summary>
		/// Gets a flag identifying if the sell thru percents are inherited.
		/// </summary>
		public bool SellThruPercentsIsInherited
        {
            get { return !SellThruPercentsInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the sell thru percents are inherited from.
        /// </summary>
        public KeyValuePair<int, string> SellThruPercentsInheritedFromNode
        {
            get { return _sellThruPercentsInheritedFromNode; }
            set { _sellThruPercentsInheritedFromNode = value; }
        }

        #endregion
        public RONodePropertiesVelocityGrades(KeyValuePair<int, string> node) :
            base(eProfileType.VelocityGrade, node)

        {
            _velocityGrades = new List<RONodePropertiesVelocityGrade>();
            _velocityGradesInheritedFromNode = default(KeyValuePair<int, string>);
            _minimumMaximumsInheritedFromNode = default(KeyValuePair<int, string>);

            _sellThruPercents = new List<ROSellThruList>();
            _sellThruPercentsInheritedFromNode = default(KeyValuePair<int, string>);
        }
    }

    [DataContract(Name = "RONodePropertiesVelocityGrade", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesVelocityGrade
    {
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _velocityGrade;
        [DataMember(IsRequired = true)]
        private int? _minimum;
        [DataMember(IsRequired = true)]
        private int? _maximum;
        [DataMember(IsRequired = true)]
        private int? _adMinimum;


        #region Public Properties
        /// <summary>
        /// Gets the velocity grade.
        /// </summary>
        public KeyValuePair<int, string> VelocityGrade
        {
            get { return _velocityGrade; }
        }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        public int? Minimum
        {
            get { return _minimum; }
            set { _minimum = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the minimum is set.
        /// </summary>
        public bool MinimumIsSet { get { return _minimum != null && _minimum != int.MinValue && _minimum != Include.Undefined; } }
        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        public int? Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the maximum is set.
        /// </summary>
        public bool MaximumIsSet { get { return _maximum != null && _maximum != int.MaxValue && _maximum != Include.Undefined; } }
        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        public int? AdMinimum
        {
            get { return _adMinimum; }
            set { _adMinimum = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the ad minimum is set.
        /// </summary>
        public bool AdMinimumIsSet { get { return _adMinimum != null && _adMinimum != int.MinValue && _adMinimum != Include.Undefined; } }
        

        #endregion

        public RONodePropertiesVelocityGrade(KeyValuePair<int, string> velocityGrade)
        {
            _velocityGrade = velocityGrade;
            _minimum = null;
            _maximum = null;
            _adMinimum = null;
        }
    }

    [DataContract(Name = "RONodePropertiesStoreGrades", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesStoreGrades : RONodeProperties
    {
        [DataMember(IsRequired = true)]
        List<RONodePropertiesStoreGrade> _storeGrades;


        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _storeGradesInheritedFromNode;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _minimumMaximumsInheritedFromNode;

        #region Public Properties
        /// <summary>
        /// Gets the list of store grades of the node
        /// </summary>
        public List<RONodePropertiesStoreGrade> StoreGrades
        {
            get { return _storeGrades; }
        }

        /// <summary>
		/// Gets a flag identifying if the store grades are inherited.
		/// </summary>
		public bool StoreGradesIsInherited
        {
            get { return !StoreGradesInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the store grades are inherited from.
        /// </summary>
        public KeyValuePair<int, string> StoreGradesInheritedFromNode
        {
            get { return _storeGradesInheritedFromNode; }
            set { _storeGradesInheritedFromNode = value; }
        }
        /// <summary>
		/// Gets a flag identifying if the store grades are inherited.
		/// </summary>
		public bool MinimumMaximumsIsInherited
        {
            get { return !MinimumMaximumsInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the store grades are inherited from.
        /// </summary>
        public KeyValuePair<int, string> MinimumMaximumsInheritedFromNode
        {
            get { return _minimumMaximumsInheritedFromNode; }
            set { _minimumMaximumsInheritedFromNode = value; }
        }

        #endregion
        public RONodePropertiesStoreGrades(KeyValuePair<int, string> node) :
            base(eProfileType.StoreGrade, node)

        {
            _storeGrades = new List<RONodePropertiesStoreGrade>();
            _storeGradesInheritedFromNode = default(KeyValuePair<int, string>);
            _minimumMaximumsInheritedFromNode = default(KeyValuePair<int, string>);
        }

        public void ClearStoreGradesInheritance()
        {
            _storeGradesInheritedFromNode = default(KeyValuePair<int, string>);
        }
        public void ClearMinimumMaximumsInheritance()
        {
            _minimumMaximumsInheritedFromNode = default(KeyValuePair<int, string>);
        }
    }

    [DataContract(Name = "RONodePropertiesStoreGrade", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesStoreGrade
    {
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _storeGrade;
        [DataMember(IsRequired = true)]
        private int _weeksOfSupplyIndex;
        [DataMember(IsRequired = true)]
        private int? _minimum;
        [DataMember(IsRequired = true)]
        private int? _maximum;
        [DataMember(IsRequired = true)]
        private int? _adMinimum;
        [DataMember(IsRequired = true)]
        private int? _colorMinimum;
        [DataMember(IsRequired = true)]
        private int? _colorMaximum;
        [DataMember(IsRequired = true)]
        private int? _shipUpTo;

        #region Public Properties
        /// <summary>
        /// Gets the store grade.
        /// </summary>
        public KeyValuePair<int, string> StoreGrade
        {
            get { return _storeGrade; }
        }
        /// <summary>
        /// Gets the weeks of supply index.
        /// </summary>
        public int WeeksOfSupplyIndex
        {
            get { return _weeksOfSupplyIndex; }
        }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        public int? Minimum
        {
            get { return _minimum; }
            set { _minimum = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the minimum is set.
        /// </summary>
        public bool MinimumIsSet { get { return _minimum != null && _minimum != int.MinValue && _minimum != Include.Undefined; } }
        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        public int? Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the maximum is set.
        /// </summary>
        public bool MaximumIsSet { get { return _maximum != null && _maximum != int.MaxValue && _maximum != Include.Undefined; } }
        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        public int? AdMinimum
        {
            get { return _adMinimum; }
            set { _adMinimum = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the ad minimum is set.
        /// </summary>
        public bool AdMinimumIsSet { get { return _adMinimum != null && _adMinimum != int.MinValue && _adMinimum != Include.Undefined; } }
        /// <summary>
        /// Gets or sets the color minimum.
        /// </summary>
        public int? ColorMinimum
        {
            get { return _colorMinimum; }
            set { _colorMinimum = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the maximum is set.
        /// </summary>
        public bool ColorMinimumIsSet { get { return _colorMinimum != null && _colorMinimum != int.MinValue && _colorMinimum != Include.Undefined; } }
        /// <summary>
        /// Gets or sets the color maximum.
        /// </summary>
        public int? ColorMaximum
        {
            get { return _colorMaximum; }
            set { _colorMaximum = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the maximum is set.
        /// </summary>
        public bool ColorMaximumIsSet { get { return _colorMaximum != null && _colorMaximum != int.MaxValue && _colorMaximum != Include.Undefined; } }
        /// <summary>
        /// Gets or sets the ship up to.
        /// </summary>
        public int? ShipUpTo
        {
            get { return _shipUpTo; }
            set { _shipUpTo = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the ship up to is set.
        /// </summary>
        public bool ShipUpToIsSet { get { return _shipUpTo != null && _shipUpTo != int.MinValue && _shipUpTo != Include.Undefined; } }


        #endregion

        public RONodePropertiesStoreGrade(KeyValuePair<int, string> storeGrade,
            int weeksOfSupplyIndex) 
        {
            _storeGrade = storeGrade;
            _weeksOfSupplyIndex = weeksOfSupplyIndex;
            _minimum = null;
            _maximum = null;
            _adMinimum = null;
            _colorMinimum = null;
            _colorMaximum = null;
            _shipUpTo = null;
        }
    }

    [DataContract(Name = "RONodePropertiesStockMinMaxStoreGradeEntry", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesStockMinMaxStoreGradeEntry
    {
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _dateRange;
        [DataMember(IsRequired = true)]
        private int? _minimum;
        [DataMember(IsRequired = true)]
        private int? _maximum;

        #region Public Properties
        /// <summary>
        /// Gets the date range.
        /// </summary>
        public KeyValuePair<int, string> DateRange
        {
            get { return _dateRange; }
        }
        /// <summary>
        /// Gets the flag identifying if the date range is the default.
        /// </summary>
        public bool DateRangeIsDefault
        {
            get { return DateRange.Key == Include.UndefinedCalendarDateRange; }
        }
        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        public int? Minimum
        {
            get { return _minimum; }
            set { _minimum = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the minimum is set.
        /// </summary>
        public bool MinimumIsSet { get { return _minimum != null && _minimum != int.MinValue && _minimum != Include.Undefined; } }
        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        public int? Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the maximum is set.
        /// </summary>
        public bool MaximumIsSet { get { return _maximum != null && _maximum != int.MaxValue && _maximum != Include.Undefined; } }
        
        #endregion

        public RONodePropertiesStockMinMaxStoreGradeEntry(KeyValuePair<int, string> dateRange)
        {
            _dateRange = dateRange;
            _minimum = null;
            _maximum = null;
        }
    }

    [DataContract(Name = "RONodePropertiesStockMinMaxStoreGrade", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesStockMinMaxStoreGrade
    {
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _storeGrade;
        [DataMember(IsRequired = true)]
        private List<RONodePropertiesStockMinMaxStoreGradeEntry> _storeGradeEntries;


        #region Public Properties
        /// <summary>
        /// Gets the store grade.
        /// </summary>
        public KeyValuePair<int, string> StoreGrade
        {
            get { return _storeGrade; }
        }

        /// <summary>
        /// Gets the flag identifying if the store grade is the default.
        /// </summary>
        public bool StoreGradeIsDefault
        {
            get { return _storeGrade.Key == Include.Undefined; }
        }

        /// <summary>
        /// Gets the list of store grade entries.
        /// </summary>
        public List<RONodePropertiesStockMinMaxStoreGradeEntry> StoreGradeEntries
        {
            get { return _storeGradeEntries; }
        }


        #endregion

        public RONodePropertiesStockMinMaxStoreGrade(KeyValuePair<int, string> storeGrade)
        {
            _storeGrade = storeGrade;
            _storeGradeEntries = new List<RONodePropertiesStockMinMaxStoreGradeEntry>();
        }
    }

    [DataContract(Name = "RONodePropertiesStockMinMaxAttributeSet", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesStockMinMaxAttributeSet
    {
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _attributeSet;
        [DataMember(IsRequired = true)]
        private List<RONodePropertiesStockMinMaxStoreGrade> _storeGrades;


        #region Public Properties
        /// <summary>
        /// Gets the attribute set.
        /// </summary>
        public KeyValuePair<int, string> AttributeSet
        {
            get { return _attributeSet; }
        }


        /// <summary>
        /// Gets the list of store grades.
        /// </summary>
        public List<RONodePropertiesStockMinMaxStoreGrade> StoreGrades
        {
            get { return _storeGrades; }
        }


        #endregion

        public RONodePropertiesStockMinMaxAttributeSet(KeyValuePair<int, string> attributeSet)
        {
            _attributeSet = attributeSet;
            _storeGrades = new List<RONodePropertiesStockMinMaxStoreGrade>();
        }
    }

    [DataContract(Name = "RONodePropertiesStockMinMax", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesStockMinMax : RONodeProperties
    {
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _attribute;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attributeSet;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _minimumMaximumsInheritedFromNode;

        [DataMember(IsRequired = true)]
        private RONodePropertiesStockMinMaxAttributeSet _stockMinMaxAttributeSet = null;

        #region Public Properties

        /// <summary>
        /// Gets the flag identifying if the attribute has been set.
        /// </summary>
        public bool AttributeIsSet
        {
            get { return !Attribute.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets or sets the attribute for the criteria.
        /// </summary>
        public KeyValuePair<int, string> Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

        /// <summary>
        /// Gets the flag identifying if the attribute has been set.
        /// </summary>
        public bool AttributeSetIsSet
        {
            get { return !AttributeSet.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets or sets the attribut for the criteria.
        /// </summary>
        public KeyValuePair<int, string> AttributeSet
        {
            get { return _attributeSet; }
            set { _attributeSet = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the stock minimums and maximums are inherited.
		/// </summary>
		public bool MinimumMaximumsIsInherited
        {
            get { return !MinimumMaximumsInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the stock minimums and maximums are inherited from.
        /// </summary>
        public KeyValuePair<int, string> MinimumMaximumsInheritedFromNode
        {
            get { return _minimumMaximumsInheritedFromNode; }
            set { _minimumMaximumsInheritedFromNode = value; }
        }

        public RONodePropertiesStockMinMaxAttributeSet StockMinMaxAttributeSet
        {
            get { return _stockMinMaxAttributeSet; }
            set { _stockMinMaxAttributeSet = value; }
        }

        #endregion
        public RONodePropertiesStockMinMax(KeyValuePair<int, string> node,
            KeyValuePair<int, string> attribute = default(KeyValuePair<int, string>),
            KeyValuePair<int, string> attributeSet = default(KeyValuePair<int, string>)) :
            base(eProfileType.StockMinMax, node)

        {
            _attribute = attribute;
            _attributeSet = attributeSet;
            _minimumMaximumsInheritedFromNode = default(KeyValuePair<int, string>);
        }
    }

    [DataContract(Name = "RONodePropertiesStoreCapacityValues", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesStoreCapacityValues
    {
        [DataMember(IsRequired = true)]
        private int? _capacity;

        #region Public Properties


        /// <summary>
        /// Gets or sets the Store Capacity.
        /// </summary>
        public int? Capacity
        {
            get { return _capacity; }
            set { _capacity = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the Store Capacity has been set.
        /// </summary>
        public bool CapacityIsSet
        {
            get { return _capacity != null; }
        }

        

        #endregion

        public RONodePropertiesStoreCapacityValues()
        {
            Clear();
        }

        public void Clear()
        {
            _capacity = null;
        }
    }

    [DataContract(Name = "RONodePropertiesStoreCapacityStore", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesStoreCapacityStore
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _store;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _StoreCapacityInheritedFromNode;
        [DataMember(IsRequired = true)]
        RONodePropertiesStoreCapacityValues _StoreCapacityValues;


        #region Public Properties

        public KeyValuePair<int, string> Store
        {
            get { return _store; }
        }

        /// <summary>
		/// Gets a flag identifying if a store's Capacity information is inherited.
		/// </summary>
		public bool StoreCapacityIsInherited
        {
            get { return !StoreCapacityInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the store's Capacity information is inherited from.
        /// </summary>
        public KeyValuePair<int, string> StoreCapacityInheritedFromNode
        {
            get { return _StoreCapacityInheritedFromNode; }
            set { _StoreCapacityInheritedFromNode = value; }
        }

        public RONodePropertiesStoreCapacityValues StoreCapacityValues
        {
            get { return _StoreCapacityValues; }
        }

        #endregion

        public RONodePropertiesStoreCapacityStore(KeyValuePair<int, string> store)
        {
            _store = store;
            _StoreCapacityInheritedFromNode = default(KeyValuePair<int, string>);
            _StoreCapacityValues = new RONodePropertiesStoreCapacityValues();
        }
    }

    [DataContract(Name = "RONodePropertiesStoreCapacityAttributeSet", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesStoreCapacityAttributeSet
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attributeSet;

        [DataMember(IsRequired = true)]
        RONodePropertiesStoreCapacityValues _StoreCapacityValues;

        [DataMember(IsRequired = true)]
        List<RONodePropertiesStoreCapacityStore> _store;

        #region Public Properties

        public KeyValuePair<int, string> AttributeSet
        {
            get { return _attributeSet; }
        }

        public RONodePropertiesStoreCapacityValues StoreCapacityValues
        {
            get { return _StoreCapacityValues; }
        }

        public List<RONodePropertiesStoreCapacityStore> Store
        {
            get { return _store; }
        }

        #endregion

        public RONodePropertiesStoreCapacityAttributeSet(KeyValuePair<int, string> attributeSet)
        {
            _attributeSet = attributeSet;
            _StoreCapacityValues = new RONodePropertiesStoreCapacityValues();
            _store = new List<RONodePropertiesStoreCapacityStore>();
        }
    }

    [DataContract(Name = "RONodePropertiesStoreCapacity", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesStoreCapacity : RONodeProperties
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attribute;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attributeSet;

        [DataMember(IsRequired = true)]
        RONodePropertiesStoreCapacityAttributeSet _storeCapacityAttributeSet = null;

        #region Public Properties

        /// <summary>
        /// Gets the flag identifying if the attribute has been set.
        /// </summary>
        public bool AttributeIsSet
        {
            get { return !Attribute.Equals(default(KeyValuePair<int, string>)); }
        }
        public KeyValuePair<int, string> Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

        /// <summary>
        /// Gets the flag identifying if the attribute has been set.
        /// </summary>
        public bool AttributeSetIsSet
        {
            get { return !AttributeSet.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets or sets the attribut for the criteria.
        /// </summary>
        public KeyValuePair<int, string> AttributeSet
        {
            get { return _attributeSet; }
            set { _attributeSet = value; }
        }

        public RONodePropertiesStoreCapacityAttributeSet StoreCapacityAttributeSet
        {
            get { return _storeCapacityAttributeSet; }
            set { _storeCapacityAttributeSet = value; }
        }

        #endregion
        public RONodePropertiesStoreCapacity(KeyValuePair<int, string> node,
             KeyValuePair<int, string> attribute = default(KeyValuePair<int, string>),
             KeyValuePair<int, string> attributeSet = default(KeyValuePair<int, string>)) :
            base(eProfileType.StoreCapacity, node)

        {
            _attribute = attribute;
            _attributeSet = attributeSet;
        }
    }

    [DataContract(Name = "RONodePropertiesDailyPercentagesValues", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesDailyPercentagesValues
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _dateRange;

        [DataMember(IsRequired = true)]
        private double? _total;

        [DataMember(IsRequired = true)]
        private double?[] _day;

        #region Public Properties

        /// <summary>
		/// Gets the date range for the daily percentages.
		/// </summary>
        public KeyValuePair<int, string> DateRange
        {
            get { return _dateRange; }
        }

        /// <summary>
		/// Gets a flag identifying if the date range is the default.
		/// </summary>
		public bool DateRangeIsDefault
        {
            get { return _dateRange.Key == Include.NoRID; }
        }

        /// <summary>
        /// Gets or sets the daily total.
        /// </summary>
        public double? Total
        {
            get { return _total; }
            set { _total = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the Store Capacity has been set.
        /// </summary>
        public bool TotalIsSet
        {
            get { return _total != null; }
        }

        /// <summary>
		/// Gets the array of the daily percentages.
		/// </summary>
        public double?[] Day
        {
            get { return _day; }
        }

        /// <summary>
		/// Gets a flag identifying if any daily percentage has been provied.
		/// </summary>
        public bool DailyPercentagesEntered
        {
            get { return _day[0] != null || _day[1] != null || _day[2] != null || _day[3] != null || _day[4] != null || _day[5] != null || _day[6] != null; }
        }

        #endregion

        public RONodePropertiesDailyPercentagesValues(KeyValuePair<int, string> dateRange)
        {
            _dateRange = dateRange;
            Clear();
        }

        public void Clear()
        {
            _total = null;
            _day = new double?[7];
        }
    }

    [DataContract(Name = "RONodePropertiesDailyPercentagesStore", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesDailyPercentagesStore
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _store;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _dailyPercentagesInheritedFromNode;

        [DataMember(IsRequired = true)]
        RONodePropertiesDailyPercentagesValues _defaultDailyPercentagesValues;

        [DataMember(IsRequired = true)]
        List<RONodePropertiesDailyPercentagesValues> _dailyPercentagesValues;


        #region Public Properties

        public KeyValuePair<int, string> Store
        {
            get { return _store; }
        }

        /// <summary>
		/// Gets a flag identifying if a store's Capacity information is inherited.
		/// </summary>
		public bool DailyPercentagesIsInherited
        {
            get { return !DailyPercentagesInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the store's Capacity information is inherited from.
        /// </summary>
        public KeyValuePair<int, string> DailyPercentagesInheritedFromNode
        {
            get { return _dailyPercentagesInheritedFromNode; }
            set { _dailyPercentagesInheritedFromNode = value; }
        }

        /// <summary>
        /// Gets the store's default daily percentage values.
        /// </summary>
        public RONodePropertiesDailyPercentagesValues DefaultDailyPercentagesValues
        {
            get { return _defaultDailyPercentagesValues; }
        }

        /// <summary>
        /// Gets the list of the store's daily percentage values.
        /// </summary>
        public List<RONodePropertiesDailyPercentagesValues> DailyPercentagesValues
        {
            get { return _dailyPercentagesValues; }
        }

        #endregion

        public RONodePropertiesDailyPercentagesStore(KeyValuePair<int, string> store)
        {
            _store = store;
            _dailyPercentagesInheritedFromNode = default(KeyValuePair<int, string>);
            _defaultDailyPercentagesValues = new RONodePropertiesDailyPercentagesValues(dateRange: new KeyValuePair<int, string>(Include.NoRID, "(Defaults)"));
            _dailyPercentagesValues = new List<RONodePropertiesDailyPercentagesValues>();
        }
    }

    [DataContract(Name = "RONodePropertiesDailyPercentagesAttributeSet", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesDailyPercentagesAttributeSet
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attributeSet;

        [DataMember(IsRequired = true)]
        RONodePropertiesDailyPercentagesValues _defaultDailyPercentagesValues;

        [DataMember(IsRequired = true)]
        List<RONodePropertiesDailyPercentagesValues> _dailyPercentagesValues;

        [DataMember(IsRequired = true)]
        List<RONodePropertiesDailyPercentagesStore> _store;

        #region Public Properties

        /// <summary>
        /// Gets the attribute set.
        /// </summary>
        public KeyValuePair<int, string> AttributeSet
        {
            get { return _attributeSet; }
        }

        /// <summary>
        /// Gets the default daily percentage values for the attribute set.
        /// </summary>
        public RONodePropertiesDailyPercentagesValues DefaultDailyPercentagesValues
        {
            get { return _defaultDailyPercentagesValues; }
        }

        /// <summary>
        /// Gets the list of the store's daily percentage values.
        /// </summary>
        public List<RONodePropertiesDailyPercentagesValues> DailyPercentagesValues
        {
            get { return _dailyPercentagesValues; }
        }

        /// <summary>
        /// Gets the list of stores in the attribute set.
        /// </summary>
        public List<RONodePropertiesDailyPercentagesStore> Store
        {
            get { return _store; }
        }

        #endregion

        public RONodePropertiesDailyPercentagesAttributeSet(KeyValuePair<int, string> attributeSet)
        {
            _attributeSet = attributeSet;
            _defaultDailyPercentagesValues = new RONodePropertiesDailyPercentagesValues(dateRange: new KeyValuePair<int, string>(Include.NoRID, "(Defaults)"));
            _dailyPercentagesValues = new List<RONodePropertiesDailyPercentagesValues>();
            _store = new List<RONodePropertiesDailyPercentagesStore>();
        }
    }

    [DataContract(Name = "RONodePropertiesDailyPercentages", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesDailyPercentages : RONodeProperties
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attribute;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attributeSet;

        [DataMember(IsRequired = true)]
        RONodePropertiesDailyPercentagesAttributeSet _dailyPercentagesAttributeSet = null;

        #region Public Properties

        /// <summary>
        /// Gets the flag identifying if the attribute has been set.
        /// </summary>
        public bool AttributeIsSet
        {
            get { return !Attribute.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets or sets the attribute for the criteria.
        /// </summary>
        public KeyValuePair<int, string> Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

        /// <summary>
        /// Gets the flag identifying if the attribute has been set.
        /// </summary>
        public bool AttributeSetIsSet
        {
            get { return !AttributeSet.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets or sets the attribut for the criteria.
        /// </summary>
        public KeyValuePair<int, string> AttributeSet
        {
            get { return _attributeSet; }
            set { _attributeSet = value; }
        }

        public RONodePropertiesDailyPercentagesAttributeSet DailyPercentagesAttributeSet
        {
            get { return _dailyPercentagesAttributeSet; }
            set { _dailyPercentagesAttributeSet = value; }
        }

        #endregion
        public RONodePropertiesDailyPercentages(KeyValuePair<int, string> node,
             KeyValuePair<int, string> attribute = default(KeyValuePair<int, string>),
             KeyValuePair<int, string> attributeSet = default(KeyValuePair<int, string>)) :
            base(eProfileType.DailyPercentages, node)

        {
            _attribute = attribute;
            _attributeSet = attributeSet;
        }
    }

    [DataContract(Name = "RONodePropertiesPurgeCriteriaSettings", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesPurgeCriteriaSettings
    {
        [DataMember(IsRequired = true)]
        ePurgeDataType _purgeDataType;

        [DataMember(IsRequired = true)]
        string _purgeLabel;

        [DataMember(IsRequired = true)]
        int _purgeValue;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _purgeInheritedFromNode;

        #region Public Properties

        /// <summary>
        /// Gets the purge data type.
        /// </summary>
        public ePurgeDataType PurgeDataType
        {
            get { return _purgeDataType; }
        }

        /// <summary>
        /// Gets the purge label.
        /// </summary>
        public string PurgeLabel
        {
            get { return _purgeLabel; }
        }

        /// <summary>
        /// Gets or sets the purge value.
        /// </summary>
        public int PurgeValue
        {
            get { return _purgeValue; }
            set { _purgeValue = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the purge setting is set.
		/// </summary>
        public bool PurgeValueIsSet
        {
            get { return _purgeValue != Include.Undefined; }
        }

        /// <summary>
		/// Gets a flag identifying if the purge setting is inherited.
		/// </summary>
		public bool PurgeIsInherited
        {
            get { return !PurgeInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the purge values are inherited from.
        /// </summary>
        public KeyValuePair<int, string> PurgeInheritedFromNode
        {
            get { return _purgeInheritedFromNode; }
            set { _purgeInheritedFromNode = value; }
        }

        #endregion

        public RONodePropertiesPurgeCriteriaSettings(ePurgeDataType purgeDataType, 
            string purgeLabel = null)
        {
            _purgeDataType = purgeDataType;
            _purgeLabel = purgeLabel;
            _purgeValue = Include.Undefined;
            _purgeInheritedFromNode = default(KeyValuePair<int, string>);
        }
    }

    [DataContract(Name = "RONodePropertiesPurgeCriteria", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesPurgeCriteria : RONodeProperties
    {
        [DataMember(IsRequired = true)]
        List<RONodePropertiesPurgeCriteriaSettings> _purgeCriteriaSettings;

        #region Public Properties

        /// <summary>
		/// Gets the list purge criteria settings.
		/// </summary>
        public List<RONodePropertiesPurgeCriteriaSettings> PurgeCriteriaSettings
        {
            get { return _purgeCriteriaSettings; }
        }

        #endregion
        public RONodePropertiesPurgeCriteria(KeyValuePair<int, string> node) :
            base(eProfileType.PurgeCriteria, node)

        {
            _purgeCriteriaSettings = new List<RONodePropertiesPurgeCriteriaSettings>();
        }
    }

    [DataContract(Name = "RONodePropertiesSizeCurveCriteria", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesSizeCurveCriteria
    {
        [DataMember(IsRequired = true)]
        private int _key;
        [DataMember(IsRequired = true)]
        private bool? _criteriaIsDefault;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _defaultInheritedFromNode;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _criteriaInheritedFromNode;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _merchandise;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _criteriaDate;
        [DataMember(IsRequired = true)]
        private bool? _applyLostSales;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _overrideLowLevelsModel;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _customLowLevelsModel;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _sizeGroup;
        [DataMember(IsRequired = true)]
        private string _sizeCurve;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _attribute;

        #region Public Properties

        /// <summary>
		/// Gets the key of the criteria.
		/// </summary>
		public int Key
        {
            get { return _key; }
        }

        /// <summary>
		/// Gets a flag identifying if the criteria is default flag is set.
		/// </summary>
		public bool CriteriaIsDefaultIsSet
        {
            get { return _criteriaIsDefault != null; }
        }

        /// <summary>
        /// Gets or sets the flag identifying if the criteria is the default criteria.
        /// </summary>
        public bool? CriteriaIsDefault
        {
            get { return _criteriaIsDefault; }
            set { _criteriaIsDefault = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the default setting is inherited.
		/// </summary>
		public bool DefaultIsInherited
        {
            get { return !DefaultInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the default information is inherited from.
        /// </summary>
        public KeyValuePair<int, string> DefaultInheritedFromNode
        {
            get { return _defaultInheritedFromNode; }
            set { _defaultInheritedFromNode = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the criteria information is inherited.
		/// </summary>
		public bool CriteriaIsInherited
        {
            get { return !CriteriaInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the criteria information is inherited from.
        /// </summary>
        public KeyValuePair<int, string> CriteriaInheritedFromNode
        {
            get { return _criteriaInheritedFromNode; }
            set { _criteriaInheritedFromNode = value; }
        }

        /// <summary>
        /// Gets or sets the criteria merchandise.
        /// </summary>
        public KeyValuePair<int, string> Merchandise
        {
            get { return _merchandise; }
            set { _merchandise = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the criteria merchandise has been set.
        /// </summary>
        public bool MerchandiseIsSet
        {
            get { return !_criteriaDate.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets or sets the criteria date.
        /// </summary>
        public KeyValuePair<int, string> CriteriaDate
        {
            get { return _criteriaDate; }
            set { _criteriaDate = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the criteria date has been set.
        /// </summary>
        public bool CriteriaDateIsSet
        {
            get { return !_criteriaDate.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the flag identifying lost sales is to be applied.
        /// </summary>
        public bool? ApplyLostSales
        {
            get { return _applyLostSales; }
            set { _applyLostSales = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the apply lost sales option has been set.
        /// </summary>
        public bool ApplyLostSalesIsSet
        {
            get { return _applyLostSales != null; }
        }
        /// <summary>
		/// Gets a flag identifying if the override low level model.
		/// </summary>
		public bool OverrideLowLevelsModelIsSet
        {
            get { return !OverrideLowLevelsModel.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the override low level model.
        /// </summary>
        public KeyValuePair<int, string> OverrideLowLevelsModel
        {
            get { return _overrideLowLevelsModel; }
            set { _overrideLowLevelsModel = value; }
        }
        
        /// <summary>
        /// Gets or sets the custom low level model.
        /// </summary>
        public KeyValuePair<int, string> CustomLowLevelsModel
        {
            get { return _customLowLevelsModel; }
            set { _customLowLevelsModel = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the custom low level model has been set.
        /// </summary>
        public bool CustomLowLevelsModelIsSet
        {
            get { return !CustomLowLevelsModel.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets the flag identifying if the size group has been set.
        /// </summary>
        public bool SizeGroupIsSet
        {
            get { return !SizeGroup.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets or sets the size group for the criteria.
        /// </summary>
        public KeyValuePair<int, string> SizeGroup
        {
            get { return _sizeGroup; }
            set { _sizeGroup = value; }
        }

        /// <summary>
        /// Gets the flag identifying if the size curve has been set.
        /// </summary>
        public bool SizeCurveIsSet
        {
            get { return SizeCurve != null; }
        }

        /// <summary>
        /// Gets or sets the size curve for the criteria.
        /// </summary>
        public string SizeCurve
        {
            get { return _sizeCurve; }
            set { _sizeCurve = value; }
        }

        /// <summary>
        /// Gets the flag identifying if the attribute has been set.
        /// </summary>
        public bool AttributeIsSet
        {
            get { return !Attribute.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets or sets the attribut for the criteria.
        /// </summary>
        public KeyValuePair<int, string> Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

        #endregion

        public RONodePropertiesSizeCurveCriteria(int key)
        {
            _key = key;
            Clear();
        }

        public void Clear()
        {
            _criteriaIsDefault = null;
            _criteriaInheritedFromNode = default(KeyValuePair<int, string>);
            _defaultInheritedFromNode = default(KeyValuePair<int, string>);
            _merchandise = default(KeyValuePair<int, string>);
            _criteriaDate = default(KeyValuePair<int, string>);
            _applyLostSales = null;
            _overrideLowLevelsModel = default(KeyValuePair<int, string>);
            _customLowLevelsModel = default(KeyValuePair<int, string>);
            _sizeGroup = default(KeyValuePair<int, string>);
            _sizeCurve = null;
            _attribute = default(KeyValuePair<int, string>);
        }
    }
    [DataContract(Name = "RONodePropertiesSizeCurvesSimilarStoreValues", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesSizeCurvesSimilarStoreValues
    {
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _similarStoreInheritedFromNode;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _similarStore;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _similarStoreTimePeriod;

        #region Public Properties

        /// <summary>
        /// Gets a flag identifying if a store's similar store information is inherited.
        /// </summary>
        public bool SimilarStoreIsInherited
        {
            get { return !SimilarStoreInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the store's similar store information is inherited from.
        /// </summary>
        public KeyValuePair<int, string> SimilarStoreInheritedFromNode
        {
            get { return _similarStoreInheritedFromNode; }
            set { _similarStoreInheritedFromNode = value; }
        }
        /// <summary>
        /// Gets a flag identifying if a similar store is set.
        /// </summary>
        public bool SimilarStoreIsSet
        {
            get { return !_similarStore.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the simlar stores identified for the store.
        /// </summary>
        public KeyValuePair<int, string> SimilarStore
        {
            get { return _similarStore; }
            set { _similarStore = value; }
        }
        /// <summary>
        /// Gets or sets the record id of the date range to use for the similar store.
        /// </summary>
        public KeyValuePair<int, string> SimilarStoreTimePeriod
        {
            get { return _similarStoreTimePeriod; }
            set { _similarStoreTimePeriod = value; }
        }
        /// <summary>
        /// Gets a flag identifying if a store's similar store until date is set.
        /// </summary>
        public bool SimilarStoreTimePeriodIsSet
        {
            get { return !_similarStoreTimePeriod.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the similar store text.
        /// </summary>
        public string SimilarStoreText
        {
            get
            {
                if (SimilarStoreIsSet)
                {
                    return _similarStore.Value;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        #endregion

        public RONodePropertiesSizeCurvesSimilarStoreValues()
        {
            Clear();
        }

        public void Clear()
        {
            _similarStoreInheritedFromNode = default(KeyValuePair<int, string>);
            _similarStore = default(KeyValuePair<int, string>);
            _similarStoreTimePeriod = default(KeyValuePair<int, string>);
        }
    }

    [DataContract(Name = "RONodePropertiesSizeCurvesSimilarStore", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesSizeCurvesSimilarStore
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _store;
        [DataMember(IsRequired = true)]
        RONodePropertiesSizeCurvesSimilarStoreValues _similarStoresValues;


        #region Public Properties

        public KeyValuePair<int, string> Store
        {
            get { return _store; }
        }

        public RONodePropertiesSizeCurvesSimilarStoreValues SimilarStoresValues
        {
            get { return _similarStoresValues; }
        }


        #endregion

        public RONodePropertiesSizeCurvesSimilarStore(KeyValuePair<int, string> store)
        {
            _store = store;
            _similarStoresValues = new RONodePropertiesSizeCurvesSimilarStoreValues();
        }
    }

    [DataContract(Name = "RONodePropertiesSizeCurvesSimilarStoresAttributeSet", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesSizeCurvesSimilarStoresAttributeSet
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attributeSet;

        [DataMember(IsRequired = true)]
        RONodePropertiesSizeCurvesSimilarStoreValues _similarStoresValues;

        [DataMember(IsRequired = true)]
        List<RONodePropertiesSizeCurvesSimilarStore> _similarStores;

        #region Public Properties

        public KeyValuePair<int, string> AttributeSet
        {
            get { return _attributeSet; }
        }

        public RONodePropertiesSizeCurvesSimilarStoreValues SimilarStoresValues
        {
            get { return _similarStoresValues; }
        }

        public List<RONodePropertiesSizeCurvesSimilarStore> SimilarStores
        {
            get { return _similarStores; }
        }

        #endregion

        public RONodePropertiesSizeCurvesSimilarStoresAttributeSet(KeyValuePair<int, string> attributeSet)
        {
            _attributeSet = attributeSet;
            _similarStoresValues = new RONodePropertiesSizeCurvesSimilarStoreValues();
            _similarStores = new List<RONodePropertiesSizeCurvesSimilarStore>();
        }
    }

    [DataContract(Name = "RONodePropertiesSizeCurves", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesSizeCurves : RONodeProperties
    {
        // Criteria
        [DataMember(IsRequired = true)]
        List<KeyValuePair<int, string>> _merchandiseList;
        
        [DataMember(IsRequired = true)]
        List<KeyValuePair<int, string>> _sizeGroupList;

        [DataMember(IsRequired = true)]
        List<KeyValuePair<int, string>> _attributeList;        

        [DataMember(IsRequired = true)]
        private List<RONodePropertiesSizeCurveCriteria> _sizeCurveInheritedCriteria;

        [DataMember(IsRequired = true)]
        private List<RONodePropertiesSizeCurveCriteria> _sizeCurveCriteria;

        // Tolerance

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _toleranceMinimumAverageInheritedFromNode;

        [DataMember(IsRequired = true)]
        private double? _toleranceMinimumAverage;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _toleranceLevelInheritedFromNode;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _toleranceLevel;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _toleranceSalesToleranceInheritedFromNode;

        [DataMember(IsRequired = true)]
        private double? _toleranceSalesTolerance;

        [DataMember(IsRequired = true)]
        private eNodeChainSalesType _toleranceSalesToleranceType;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _toleranceMinimumPercentInheritedFromNode;

        [DataMember(IsRequired = true)]
        private double? _toleranceMinimumPercent;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _toleranceMaximumPercentInheritedFromNode;

        [DataMember(IsRequired = true)]
        private double? _toleranceMaximumPercent;

        [DataMember(IsRequired = true)]
        private bool? _applyMinimumToZeroTolerance;

        [DataMember(IsRequired = true)]
        List<KeyValuePair<int, string>> _toleranceLevelList;

        // Similar Stores

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _similarStoresAttribute;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _similarStoresAttributeSet;

        [DataMember(IsRequired = true)]
        RONodePropertiesSizeCurvesSimilarStoresAttributeSet _similarStoresAttributeSetValues;

        #region Public Properties

        // Criteria

        /// <summary>
        /// Gets the list of merchandise level values.
        /// </summary>
        public List<KeyValuePair<int, string>> MerchandiseList
        {
            get { return _merchandiseList; }
        }

        /// <summary>
        /// Gets the list of merchandise level values.
        /// </summary>
        public List<KeyValuePair<int, string>> SizeGroupList
        {
            get { return _sizeGroupList; }
        }

        public List<KeyValuePair<int, string>> AttributeList
        {
            get { return _attributeList; }
        }        

        /// <summary>
        /// Gets the list of RONodePropertiesSizeCurveCriteria objects containing the inherited size curve criteria.
        /// </summary>
        public List<RONodePropertiesSizeCurveCriteria> SizeCurveInheritedCriteria
        {
            get { return _sizeCurveInheritedCriteria; }
        }

        /// <summary>
        /// Gets the list of RONodePropertiesSizeCurveCriteria objects containing the size curve criteria.
        /// </summary>
        public List<RONodePropertiesSizeCurveCriteria> SizeCurveCriteria
        {
            get { return _sizeCurveCriteria; }
        }

        // Tolerance

        /// <summary>
        /// Gets the list of tolerance level values.
        /// </summary>
        public List<KeyValuePair<int, string>> ToleranceLevelList
        {
            get { return _toleranceLevelList; }
        }

        /// <summary>
        /// Gets a flag identifying if the tolerance minimum average is inherited.
        /// </summary>
        public bool ToleranceMinimumAverageIsInherited
        {
            get { return !ToleranceMinimumAverageInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the tolerance minimum average is inherited from.
        /// </summary>
        public KeyValuePair<int, string> ToleranceMinimumAverageInheritedFromNode
        {
            get { return _toleranceMinimumAverageInheritedFromNode; }
            set { _toleranceMinimumAverageInheritedFromNode = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the tolerance minimum average is set.
		/// </summary>
		public bool ToleranceMinimumAverageIsSet
        {
            get { return _toleranceMinimumAverage != null; }
        }
        /// <summary>
        /// Gets or sets the tolerance minimum average.
        /// </summary>
        public double? ToleranceMinimumAverage
        {
            get { return _toleranceMinimumAverage; }
            set { _toleranceMinimumAverage = value; }
        }

        /// <summary>
        /// Gets a flag identifying if the tolerance level is inherited.
        /// </summary>
        public bool ToleranceLevelIsInherited
        {
            get { return !ToleranceLevelInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the tolerance level is inherited from.
        /// </summary>
        public KeyValuePair<int, string> ToleranceLevelInheritedFromNode
        {
            get { return _toleranceLevelInheritedFromNode; }
            set { _toleranceLevelInheritedFromNode = value; }
        }

        /// <summary>
        /// Gets a flag identifying if the tolerance level is set.
        /// </summary>
        public bool ToleranceLevelIsSet
        {
            get { return !ToleranceLevel.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the tolerance level.
        /// </summary>
        public KeyValuePair<int, string> ToleranceLevel
        {
            get { return _toleranceLevel; }
            set { _toleranceLevel = value; }
        }

        /// <summary>
        /// Gets a flag identifying if the tolerance sales tolerance is inherited.
        /// </summary>
        public bool ToleranceSalesToleranceIsInherited
        {
            get { return !ToleranceSalesToleranceInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the tolerance sales is inherited from.
        /// </summary>
        public KeyValuePair<int, string> ToleranceSalesToleranceInheritedFromNode
        {
            get { return _toleranceSalesToleranceInheritedFromNode; }
            set { _toleranceSalesToleranceInheritedFromNode = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the tolerance sales tolerance is set.
		/// </summary>
		public bool ToleranceSalesToleranceIsSet
        {
            get { return _toleranceSalesTolerance != null; }
        }
        /// <summary>
        /// Gets or sets the tolerance sales tolerance.
        /// </summary>
        public double? ToleranceSalesTolerance
        {
            get { return _toleranceSalesTolerance; }
            set { _toleranceSalesTolerance = value; }
        }

        /// <summary>
        /// Gets a flag identifying if the tolerance index units is set.
        /// </summary>
        public bool ToleranceSalesToleranceTypeIsSet
        {
            get { return _toleranceSalesToleranceType != eNodeChainSalesType.None; }
        }
        /// <summary>
        /// Gets or sets the tolerance sales type.
        /// </summary>
        public eNodeChainSalesType ToleranceSalesToleranceType
        {
            get { return _toleranceSalesToleranceType; }
            set { _toleranceSalesToleranceType = value; }
        }

        /// <summary>
        /// Gets a flag identifying if the minimum tolerance is inherited.
        /// </summary>
        public bool ToleranceMinimumPercentIsInherited
        {
            get { return !ToleranceMinimumPercentInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the tolerance minimum percent is inherited from.
        /// </summary>
        public KeyValuePair<int, string> ToleranceMinimumPercentInheritedFromNode
        {
            get { return _toleranceMinimumPercentInheritedFromNode; }
            set { _toleranceMinimumPercentInheritedFromNode = value; }
        }

        /// <summary>
        /// Gets a flag identifying if the tolerance minimum percent is set.
        /// </summary>
        public bool ToleranceMinimumPercentIsSet
        {
            get { return _toleranceMinimumPercent != null; }
        }
        /// <summary>
        /// Gets or sets the tolerance minimum percent.
        /// </summary>
        public double? ToleranceMinimumPercent
        {
            get { return _toleranceMinimumPercent; }
            set { _toleranceMinimumPercent = value; }
        }

        /// <summary>
        /// Gets a flag identifying if the tolerance level is inherited.
        /// </summary>
        public bool ToleranceMaximumPercentIsInherited
        {
            get { return !ToleranceMaximumPercentInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the tolerance maximum is inherited from.
        /// </summary>
        public KeyValuePair<int, string> ToleranceMaximumPercentInheritedFromNode
        {
            get { return _toleranceMaximumPercentInheritedFromNode; }
            set { _toleranceMaximumPercentInheritedFromNode = value; }
        }

        /// <summary>
        /// Gets a flag identifying if the tolerance maximum percent is set.
        /// </summary>
        public bool ToleranceMaximumPercentIsSet
        {
            get { return _toleranceMaximumPercent != null; }
        }
        /// <summary>
        /// Gets or sets the tolerance maximum percent.
        /// </summary>
        public double? ToleranceMaximumPercent
        {
            get { return _toleranceMaximumPercent; }
            set { _toleranceMaximumPercent = value; }
        }

        /// <summary>
        /// Gets a flag identifying if the apply minimum to zero tolerance flag is set.
        /// </summary>
        public bool ApplyMinimumToZeroToleranceIsSet
        {
            get { return _applyMinimumToZeroTolerance != null; }
        }
        /// <summary>
        /// Gets or sets the apply minimum to zero tolerance flag.
        /// </summary>
        public bool? ApplyMinimumToZeroTolerance
        {
            get { return _applyMinimumToZeroTolerance; }
            set { _applyMinimumToZeroTolerance = value; }
        }

        // Similar Stores

        /// <summary>
        /// Gets the flag identifying if the similar store attribute is set.
        /// </summary>
        public bool SimilarStoresAttributeIsSet
        {
            get { return !_similarStoresAttribute.Equals(default(KeyValuePair<int, string>)); }
        }
        public KeyValuePair<int, string> SimilarStoresAttribute
        {
            get { return _similarStoresAttribute; }
            set { _similarStoresAttribute = value; }
        }
        public bool SimilarStoresAttributeSetIsSet
        {
            get { return !_similarStoresAttributeSet.Equals(default(KeyValuePair<int, string>)); }
        }
        public KeyValuePair<int, string> SimilarStoresAttributeSet
        {
            get { return _similarStoresAttributeSet; }
            set { _similarStoresAttributeSet = value; }
        }

        public RONodePropertiesSizeCurvesSimilarStoresAttributeSet SimilarStoresAttributeSetValues
        {
            get { return _similarStoresAttributeSetValues; }
            set { _similarStoresAttributeSetValues = value; }
        }

        #endregion

        public RONodePropertiesSizeCurves(KeyValuePair<int, string> node) :
            base(eProfileType.SizeCurve, node)

        {
            // Criteria
            _sizeCurveInheritedCriteria = new List<RONodePropertiesSizeCurveCriteria>();
            _sizeCurveCriteria = new List<RONodePropertiesSizeCurveCriteria>();
            _merchandiseList = new List<KeyValuePair<int, string>>();
            _sizeGroupList = new List<KeyValuePair<int, string>>();
            _attributeList = new List<KeyValuePair<int, string>>();

            // Tolerance

            _toleranceMinimumAverageInheritedFromNode = default(KeyValuePair<int, string>);
            _toleranceMinimumAverage = null;
            _toleranceLevelInheritedFromNode = default(KeyValuePair<int, string>); ;
            _toleranceLevel = default(KeyValuePair<int, string>);
            _toleranceSalesToleranceInheritedFromNode = default(KeyValuePair<int, string>);
            _toleranceSalesTolerance = null;
            _toleranceSalesToleranceType = eNodeChainSalesType.None;
            _toleranceMinimumPercentInheritedFromNode = default(KeyValuePair<int, string>);
            _toleranceMinimumPercent = null;
            _toleranceMaximumPercentInheritedFromNode = default(KeyValuePair<int, string>);
            _toleranceMaximumPercent = null;
            _applyMinimumToZeroTolerance = null;
            _toleranceLevelList = new List<KeyValuePair<int, string>>();

            // Similar Stores

            // _similarStoresAttributeSets = new List<RONodePropertiesSizeCurvesSimilarStoresAttributeSet>();
            _similarStoresAttribute = default(KeyValuePair<int, string>);
            _similarStoresAttributeSet = default(KeyValuePair<int, string>);
        }
    }

    [DataContract(Name = "RONodePropertiesChainSetPercentWeek", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesChainSetPercentWeek
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _week;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _chainSetPercentWeekInheritedFromNode;

        #region Public Properties

        public KeyValuePair<int, string> Week
        {
            get { return _week; }
        }

        /// <summary>
		/// Gets a flag identifying if a chain percent week is inherited.
		/// </summary>
		public bool ChainSetPercentWeekIsInherited
        {
            get { return !ChainSetPercentWeekInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the chain percent week is inherited from.
        /// </summary>
        public KeyValuePair<int, string> ChainSetPercentWeekInheritedFromNode
        {
            get { return _chainSetPercentWeekInheritedFromNode; }
            set { _chainSetPercentWeekInheritedFromNode = value; }
        }

        #endregion

        public RONodePropertiesChainSetPercentWeek(KeyValuePair<int, string> week)
        {
            _week = week;
            _chainSetPercentWeekInheritedFromNode = default(KeyValuePair<int, string>);
        }
    }

    [DataContract(Name = "RONodePropertiesChainSetPercentAttributeSet", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesChainSetPercentAttributeSet
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attributeSet;
       

        #region Public Properties

        public KeyValuePair<int, string> AttributeSet
        {
            get { return _attributeSet; }
        }

        #endregion

        public RONodePropertiesChainSetPercentAttributeSet(KeyValuePair<int, string> attributeSet)
        {
            _attributeSet = attributeSet;
        }
    }

    [DataContract(Name = "RONodePropertiesChainSetPercent", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesChainSetPercent : RONodeProperties
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attribute;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _timePeriod;

        [DataMember(IsRequired = true)]
        List<RONodePropertiesChainSetPercentAttributeSet> _attributeSets;

        [DataMember(IsRequired = true)]
        List<RONodePropertiesChainSetPercentWeek> _weeks;

        [DataMember(IsRequired = true)]
        decimal?[][] _percentages;

        [DataMember(IsRequired = true)]
        decimal?[] _totals;

        #region Public Properties

        public KeyValuePair<int, string> Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

        public KeyValuePair<int, string> TimePeriod
        {
            get { return _timePeriod; }
            set { _timePeriod = value; }
        }

        /// <summary>
        /// Gets the flag identifying if the time period has been set.
        /// </summary>
        public bool TimePeriodIsSet
        {
            get { return !_timePeriod.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets the list of RONodePropertiesChainSetPercentAttributeSet objects containing attribute set information.
        /// </summary>
        public List<RONodePropertiesChainSetPercentAttributeSet> AttributeSets
        {
            get { return _attributeSets; }
        }

        /// <summary>
        /// Gets the list of RONodePropertiesChainSetPercentWeek objects containing week information.
        /// </summary>
        public List<RONodePropertiesChainSetPercentWeek> Weeks
        {
            get { return _weeks; }
        }

        /// <summary>
        /// Gets the matrix of percentages.
        /// </summary>
        public decimal?[][] Percentages
        {
            get { return _percentages; }
        }

        /// <summary>
        /// Gets the array of totals.
        /// </summary>
        public decimal?[] Totals
        {
            get { return _totals; }
        }

        #endregion
        public RONodePropertiesChainSetPercent(KeyValuePair<int, string> node,
             KeyValuePair<int, string> attribute,
             KeyValuePair<int, string> timePeriod) :
            base(eProfileType.ChainSetPercent, node)

        {
            _attribute = attribute;
            _timePeriod = timePeriod;
            _attributeSets = new List<RONodePropertiesChainSetPercentAttributeSet>();
            _weeks = new List<RONodePropertiesChainSetPercentWeek>();
        }

        public void DefinePercentages(int numberOfRows, int numberOfColumns)
        {
            if (_attributeSets.Count > numberOfRows)
            {
                numberOfRows = _attributeSets.Count;
            }

            if (_weeks.Count > numberOfColumns)
            {
                numberOfColumns = _weeks.Count;
            }

            _percentages = new decimal?[numberOfRows][];
            for (int row = 0; row < numberOfRows; row++)
            {
                _percentages[row] = new decimal?[numberOfColumns];
            }

            _totals = new decimal?[numberOfColumns];
        }
    }

    [DataContract(Name = "RONodePropertiesVSWValues", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesVSWValues
    {
        
        [DataMember(IsRequired = true)]
        private string _VSWID;
        [DataMember(IsRequired = true)]
        private int? _MinShipQty;
        [DataMember(IsRequired = true)]
        private double? _PackQty;
        [DataMember(IsRequired = true)]
        private int? _MaxValue;
        [DataMember(IsRequired = true)]
        private bool? _ApplyVSW;
        [DataMember(IsRequired = true)]
        private double? _FWOSMax;
        [DataMember(IsRequired = true)]
        private int? _PushToBackStock;
        [DataMember(IsRequired = true)]
        private eModifierType _FWOSMaxType;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _FWOSMaxModel;

        #region Public Properties

        /// <summary>
        /// Gets or sets the VSW ID.
        /// </summary>
        public string VSWID
        {
            get { return _VSWID; }
            set { _VSWID = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the VSW ID has been set.
        /// </summary>
        public bool VSWIDIsSet
        {
            get { return _VSWID != null; }
        }

        /// <summary>
        /// Gets or sets the VSW Minimum Ship Quantity.
        /// </summary>
        public int? MinShipQty
        {
            get { return _MinShipQty; }
            set { _MinShipQty = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the VSW Minimum Ship Quantity has been set.
        /// </summary>
        public bool MinShipQtyIsSet
        {
            get { return _MinShipQty != null; }
        }

        /// <summary>
        /// Gets or sets the VSW Pack Quantity.
        /// </summary>
        public double? PackQty
        {
            get { return _PackQty; }
            set { _PackQty = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the VSW Pack Quantity has been set.
        /// </summary>
        public bool PackQtyIsSet
        {
            get { return _PackQty != null; }
        }

        /// <summary>
        /// Gets or sets the VSW Maximum Value.
        /// </summary>
        public int? MaxValue
        {
            get { return _MaxValue; }
            set { _MaxValue = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the VSW Maximum Value has been set.
        /// </summary>
        public bool MaxValueIsSet
        {
            get { return _MaxValue != null; }
        }

        /// <summary>
        /// Gets or sets the VSW Apply VSW.
        /// </summary>
        public bool? ApplyVSW
        {
            get { return _ApplyVSW; }
            set { _ApplyVSW = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the VSW Apply VSW has been set.
        /// </summary>
        public bool ApplyVSWIsSet
        {
            get { return _ApplyVSW != null; }
        }

        /// <summary>
        /// Gets or sets the VSW FWOS Max.
        /// </summary>
        public double? FWOSMax
        {
            get { return _FWOSMax; }
            set { _FWOSMax = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the VSW FWOS Max has been set.
        /// </summary>
        public bool FWOSMaxIsSet
        {
            get { return _FWOSMax != null; }
        }

        /// <summary>
        /// Gets or sets the VSW Push To Backstock.
        /// </summary>
        public int? PushToBackStock
        {
            get { return _PushToBackStock; }
            set { _PushToBackStock = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the VSW Push To Backstock has been set.
        /// </summary>
        public bool PushToBackStockIsSet
        {
            get { return _PushToBackStock != null; }
        }

        /// <summary>
        /// Gets or sets the VSW FWOS Max Type.
        /// </summary>
        public eModifierType FWOSMaxType
        {
            get { return _FWOSMaxType; }
            set { _FWOSMaxType = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the VSW FWOS Max Type has been set.
        /// </summary>
        public bool FWOSMaxTypeIsSet
        {
            get { return _FWOSMaxType != eModifierType.None; }
        }

        /// <summary>
        /// Gets or sets the record id of the VSW FWOS Max model.
        /// </summary>
        public KeyValuePair<int, string> FWOSMaxModel
        {
            get { return _FWOSMaxModel; }
            set { _FWOSMaxModel = value; }
        }
        /// <summary>
        /// Gets the flag identifying if the VSW FWOS Max model has been set.
        /// </summary>
        public bool FWOSMaxModelIsSet
        {
            get { return !_FWOSMaxModel.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets or sets the VSW FWOS Max.
        /// </summary>
        public string FWOSMaxText
        {
            get
            {
                switch (_FWOSMaxType)
                {
                    case eModifierType.Model:
                        return _FWOSMaxModel.Value;
                    case eModifierType.Percent:
                        return ((double)_FWOSMax).ToString(CultureInfo.CurrentUICulture);
                    default:
                        return string.Empty;
                }
            }
        }

        #endregion

        public RONodePropertiesVSWValues()
        {
            Clear();
        }

        public void Clear()
        {
            _VSWID = null;
            _MinShipQty = null;
            _PackQty = null;
            _MaxValue = null;

            _ApplyVSW = null;
            _FWOSMax = null;
            _FWOSMaxType = eModifierType.None;
            _FWOSMaxModel = default(KeyValuePair<int, string>);
        }
    }

    [DataContract(Name = "RONodePropertiesVSWStore", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesVSWStore
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _store;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _VSWInheritedFromNode;
        [DataMember(IsRequired = true)]
        RONodePropertiesVSWValues _VSWValues;


        #region Public Properties

        public KeyValuePair<int, string> Store
        {
            get { return _store; }
        }

        /// <summary>
		/// Gets a flag identifying if a store's VSW information is inherited.
		/// </summary>
		public bool VSWIsInherited
        {
            get { return !VSWInheritedFromNode.Equals(default(KeyValuePair<int, string>)); }
        }
        /// <summary>
        /// Gets or sets the record ID of the node where the store's VSW information is inherited from.
        /// </summary>
        public KeyValuePair<int, string> VSWInheritedFromNode
        {
            get { return _VSWInheritedFromNode; }
            set { _VSWInheritedFromNode = value; }
        }

        public RONodePropertiesVSWValues VSWValues
        {
            get { return _VSWValues; }
        }

        #endregion

        public RONodePropertiesVSWStore(KeyValuePair<int, string> store)
        {
            _store = store;
            _VSWInheritedFromNode = default(KeyValuePair<int, string>);
            _VSWValues = new RONodePropertiesVSWValues();
        }
    }

    [DataContract(Name = "RONodePropertiesVSWAttributeSet", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesVSWAttributeSet
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attributeSet;

        [DataMember(IsRequired = true)]
        RONodePropertiesVSWValues _VSWValues;

        [DataMember(IsRequired = true)]
        List<RONodePropertiesVSWStore> _store;

        #region Public Properties

        public KeyValuePair<int, string> AttributeSet
        {
            get { return _attributeSet; }
        }

        public RONodePropertiesVSWValues VSWValues
        {
            get { return _VSWValues; }
        }

        public List<RONodePropertiesVSWStore> Store
        {
            get { return _store; }
        }

        #endregion

        public RONodePropertiesVSWAttributeSet(KeyValuePair<int, string> attributeSet)
        {
            _attributeSet = attributeSet;
            _VSWValues = new RONodePropertiesVSWValues();
            _store = new List<RONodePropertiesVSWStore>();
        }
    }

    [DataContract(Name = "RONodePropertiesVSW", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesVSW : RONodeProperties
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attribute;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attributeSet;

        [DataMember(IsRequired = true)]
        RONodePropertiesVSWAttributeSet _VSWAttributeSet = null;

        #region Public Properties

        /// <summary>
        /// Gets the flag identifying if the attribute has been set.
        /// </summary>
        public bool AttributeIsSet
        {
            get { return !Attribute.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets or sets the attribute for the criteria.
        /// </summary>
        public KeyValuePair<int, string> Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

        /// <summary>
        /// Gets the flag identifying if the attribute has been set.
        /// </summary>
        public bool AttributeSetIsSet
        {
            get { return !AttributeSet.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets or sets the attribut for the criteria.
        /// </summary>
        public KeyValuePair<int, string> AttributeSet
        {
            get { return _attributeSet; }
            set { _attributeSet = value; }
        }
        public RONodePropertiesVSWAttributeSet VSWAttributeSet
        {
            get { return _VSWAttributeSet; }
            set { _VSWAttributeSet = value; }
        }

        #endregion
        public RONodePropertiesVSW(KeyValuePair<int, string> node,
             KeyValuePair<int, string> attribute = default(KeyValuePair<int, string>),
             KeyValuePair<int, string> attributeSet = default(KeyValuePair<int, string>)) :
            base(eProfileType.IMO, node)

        {
            _attribute = attribute;
            _attributeSet = attributeSet;
        }
    }
}