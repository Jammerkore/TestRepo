
using System;
using MIDRetail.DataCommon;
using System.Collections.Generic;
using System.Runtime.Serialization;

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
                if (Enum.IsDefined(typeof(eHierarchyType), value))
                {
                    _hierarchyType = value;
                }
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
                if (Enum.IsDefined(typeof(eHierarchyRollupOption), value))
                {
                    _hierarchyRollupOption = value;
                }
            }
        }

        public eOTSPlanLevelType PlanLevelType
        {
            get { return _planLevelType; }
            set
            {
                if (Enum.IsDefined(typeof(eOTSPlanLevelType), value))
                {
                    _planLevelType = value;
                }
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
                if (Enum.IsDefined(typeof(eHierarchyLevelType), value))
                {
                    _levelType = value;
                }
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
                if (Enum.IsDefined(typeof(eLevelLengthType), value))
                {
                    _levelLengthType = value;
                }
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
                if (Enum.IsDefined(typeof(eOTSPlanLevelType), value))
                {
                    _levelOTSPlanLevelType = value;
                }
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
                if (Enum.IsDefined(typeof(eHierarchyDisplayOptions), value))
                {
                    _levelDisplayOption = value;
                }
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
                if (Enum.IsDefined(typeof(eHierarchyIDFormat), value))
                {
                    _levelIDFormat = value;
                }
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
    public abstract class RONodeProperties : ROBaseProperties
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
                if (Enum.IsDefined(typeof(eProductType), value))
                {
                    _productType = value;
                    if (_originalProductType == eProductType.Undefined)
                    {
                        _originalProductType = _productType;
                    }
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
                if (Enum.IsDefined(typeof(eHierarchyLevelType), value))
                {
                    _levelType = value;
                }
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
                if (Enum.IsDefined(typeof(ePlanLevelSelectType), value))
                {
                    _OTSForecastSelectType = value;
                }
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
                if (Enum.IsDefined(typeof(eMaskField), value))
                {
                    _OTSForecastMaskType = value;
                }
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
                if (Enum.IsDefined(typeof(eOTSPlanLevelType), value))
                {
                    _OTSForecastType = value;
                    if (_originalOTSForecastType == eOTSPlanLevelType.Undefined)
                    {
                        _originalOTSForecastType = _OTSForecastType;
                    }
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
        List<RONodePropertiesEligibilityAttributeSet> _eligibilityAttributeSet;

        #region Public Properties

        public KeyValuePair<int, string> Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }
        public List<RONodePropertiesEligibilityAttributeSet> EligibilityAttributeSet
        {
            get { return _eligibilityAttributeSet; }
        }

        #endregion
        public RONodePropertiesEligibility(KeyValuePair<int, string> node,
             KeyValuePair<int, string> attribute) :
            base(eProfileType.StoreEligibility, node)

        {
            _attribute = attribute;
            _eligibilityAttributeSet = new List<RONodePropertiesEligibilityAttributeSet>();
        }
    }

    [DataContract(Name = "RONodePropertiesCharacteristics", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesCharacteristics : RONodeProperties
    {


        #region Public Properties



        #endregion
        public RONodePropertiesCharacteristics(KeyValuePair<int, string> node) :
            base(eProfileType.StoreCharacteristics, node)

        {

        }
    }

    [DataContract(Name = "RONodePropertiesVelocityGrades", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesVelocityGrades : RONodeProperties
    {


        #region Public Properties



        #endregion
        public RONodePropertiesVelocityGrades(KeyValuePair<int, string> node) :
            base(eProfileType.VelocityGrade, node)

        {

        }
    }

    [DataContract(Name = "RONodePropertiesStoreGrades", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesStoreGrades : RONodeProperties
    {


        #region Public Properties



        #endregion
        public RONodePropertiesStoreGrades(KeyValuePair<int, string> node) :
            base(eProfileType.StoreGrade, node)

        {

        }
    }

    [DataContract(Name = "RONodePropertiesStockMinMax", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesStockMinMax : RONodeProperties
    {


        #region Public Properties



        #endregion
        public RONodePropertiesStockMinMax(KeyValuePair<int, string> node) :
            base(eProfileType.StockMinMax, node)

        {

        }
    }

    [DataContract(Name = "RONodePropertiesStoreCapacity", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesStoreCapacity : RONodeProperties
    {


        #region Public Properties



        #endregion
        public RONodePropertiesStoreCapacity(KeyValuePair<int, string> node) :
            base(eProfileType.StoreCapacity, node)

        {

        }
    }

    [DataContract(Name = "RONodePropertiesDailyPercentages", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesDailyPercentages : RONodeProperties
    {


        #region Public Properties



        #endregion
        public RONodePropertiesDailyPercentages(KeyValuePair<int, string> node) :
            base(eProfileType.DailyPercentages, node)

        {

        }
    }

    [DataContract(Name = "RONodePropertiesPurgeCriteria", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesPurgeCriteria : RONodeProperties
    {


        #region Public Properties



        #endregion
        public RONodePropertiesPurgeCriteria(KeyValuePair<int, string> node) :
            base(eProfileType.PurgeCriteria, node)

        {

        }
    }

    [DataContract(Name = "RONodePropertiesSizeCurves", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesSizeCurves : RONodeProperties
    {


        #region Public Properties



        #endregion
        public RONodePropertiesSizeCurves(KeyValuePair<int, string> node) :
            base(eProfileType.SizeCurve, node)

        {

        }
    }

    [DataContract(Name = "RONodePropertiesChainSetPercent", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesChainSetPercent : RONodeProperties
    {


        #region Public Properties



        #endregion
        public RONodePropertiesChainSetPercent(KeyValuePair<int, string> node) :
            base(eProfileType.ChainSetPercent, node)

        {

        }
    }

    [DataContract(Name = "RONodePropertiesVSW", Namespace = "http://Logility.ROWeb/")]
    public class RONodePropertiesVSW : RONodeProperties
    {


        #region Public Properties



        #endregion
        public RONodePropertiesVSW(KeyValuePair<int, string> node) :
            base(eProfileType.IMO, node)

        {

        }
    }
}