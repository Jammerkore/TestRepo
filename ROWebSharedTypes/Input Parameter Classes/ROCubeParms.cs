using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.DataCommon;

namespace Logility.ROWebSharedTypes
{

    [DataContract(Name = "ROGridChangesParms", Namespace = "http://Logility.ROWeb/")]
    public class ROGridChangesParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private List<ROGridCellChange> _cellChanges;

        public ROGridChangesParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, List<ROGridCellChange> listCellChanges) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _cellChanges = listCellChanges;
        }

        public ROGridChangesParms(string sROUserID, string sROSessionID, eScreenID eScreenIDParam, long ROInstanceID, List<ROGridCellChange> listCellChanges)
        {
            _sROUserID = sROUserID;
            _sROSessionID = sROSessionID;
            _ROInstanceID = ROInstanceID;
            _ROClass = ROParms.GetClassForScreen(eScreenIDParam);
            _RORequest = eRORequest.CellChanged;

            _cellChanges = listCellChanges;
        }

        public List<ROGridCellChange> CellChanges { get { return _cellChanges; } }
    };

    [DataContract(Name = "ROCubeOpenParms", Namespace = "http://Logility.ROWeb/")]
    public class ROCubeOpenParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private eScreenID _eScreenID;               // PlanChainLadder, PlanView, StyleView, ...  (see solution's Windows folder)
        [DataMember(IsRequired = true)]
        private string _sView;                      // the view to organize the data (see PLAN_VIEW database table)
        [DataMember(IsRequired = true)]
        private MIDRetail.DataCommon.ePlanSessionType _ePlanSessionType; // what type of sesssion (store/chain, single/multi - see Enums.cs/ePlanSessionType)
        [DataMember(IsRequired = true)]
        private int _nodeRID;                       // the node key
        [DataMember(IsRequired = true)]
        private string _sVersion;                   // the version of the data
        [DataMember(IsRequired = true)]
        private int _cdrRID;                         // calendar date range ID
        [DataMember(IsRequired = true)]
        private bool _showBasis;                     // show Quick/Dirty Basis (using same filter values as data)
        [DataMember(IsRequired = true)]
        private List<ROBasisProfile> _basisProfiles;
        [DataMember(IsRequired = true)]
        private int _viewKey;                      // the view to organize the data (see PLAN_VIEW database table)
        [DataMember(IsRequired = true)]
        private int _versionKey;                   // the version of the data
        [DataMember(IsRequired = true)]
        private MIDRetail.DataCommon.eStorePlanSelectedGroupBy _eStorePlanSelectedGroupBy;  // what Group By Option "ByTimePeriod" or "ByVariable"
        [DataMember(IsRequired = true)]
        private string _sLowLevelVersion;            // the low level version of the data
        [DataMember(IsRequired = true)]
        private int _lowLevelVersionKey;            // the low level version of the data
        [DataMember(IsRequired = true)]
        private int _filterRID;                      // filter ID
        [DataMember(IsRequired = true)]
        private MIDRetail.DataCommon.eLowLevelsType _lowLevelsType;  // what type of low level plan is used for this cube None = 0, HierarchyLevel = 1, Characteristic = 2, LevelOffset = 3
        [DataMember(IsRequired = true)]
        private int _lowLevelsOffset;               // int offset for low levels
        [DataMember(IsRequired = true)]
        private int _lowLevelsSequence;              // int sequence for low levels
        [DataMember(IsRequired = true)]
        private int _ollRID;                         // int Override Low Level ID
        [DataMember(IsRequired = true)]
        private int _customOLLRID;                 // int Customer Override Low Level ID
        [DataMember(IsRequired = true)]
        private int _storeGroupRID;
        [DataMember(IsRequired = true)]
        private int _storeNodeRID;
        [DataMember(IsRequired = true)]
        private int _storeVersionKey;
        [DataMember(IsRequired = true)]
        private bool _ineligibleStores;
        [DataMember(IsRequired = true)]
        private bool _similarStore;
        [DataMember(IsRequired = true)]
        private string _storeRID;
        [DataMember(IsRequired = true)]
        private int _storeKey;

        public ROCubeOpenParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, eScreenID eScreenIDParam,

            string sViewParam, MIDRetail.DataCommon.ePlanSessionType ePlanSessionTypeParam, int nodeRID, string sVersionParam, int cdrRID,
            bool showBasis, List<ROBasisProfile> basisProfiles = null, eStorePlanSelectedGroupBy eStorePlanSelectedGroupByParam = eStorePlanSelectedGroupBy.ByTimePeriod,
            string sLowLevelVersionParam = null, int filterRID = Include.Undefined, MIDRetail.DataCommon.eLowLevelsType eLowLevelsTypeParam = eLowLevelsType.None,
            int lowLevelsOffset = 0, int lowLevelsSequence = 0, int ollRID = Include.Undefined, int customOLLRID = Include.Undefined,
            int storeGroupRID = Include.Undefined, int storeNodeRID = Include.Undefined, int storeFVRID = Include.Undefined, bool ineligibleStores = false,
            bool similarStore = false, string storeRID = null,
            int viewKey = Include.Undefined, int versionKey = Include.Undefined, int lowLevelVersionKey = Include.Undefined, int storeVersionKey = Include.Undefined, int storeKey = Include.Undefined) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _eScreenID = eScreenIDParam;
            _sView = sViewParam;
            _ePlanSessionType = ePlanSessionTypeParam;
            _nodeRID = nodeRID;
            _sVersion = sVersionParam;
            _cdrRID = cdrRID;
            _showBasis = showBasis;
            _basisProfiles = basisProfiles;
            _viewKey = viewKey;
            _versionKey = versionKey;
            _eStorePlanSelectedGroupBy = eStorePlanSelectedGroupByParam;
            _sLowLevelVersion = sLowLevelVersionParam;
            _lowLevelVersionKey = lowLevelVersionKey;
            _filterRID = filterRID;
            _lowLevelsType = eLowLevelsTypeParam;
            _lowLevelsOffset = lowLevelsOffset;
            _lowLevelsSequence = lowLevelsSequence;
            _ollRID = ollRID;
            _customOLLRID = customOLLRID;
            _storeGroupRID = storeGroupRID;
            _storeNodeRID = storeNodeRID;
            _ineligibleStores = ineligibleStores;
            _similarStore = similarStore;
            _storeRID = storeRID;
            _storeVersionKey = storeFVRID;
            if (storeVersionKey != Include.Undefined)
            {
                _storeVersionKey = storeVersionKey;
            }
            _storeKey = storeKey;
        }

        public ROCubeOpenParms(string sROUserID, string sROSessionID, long ROInstanceID, eScreenID eScreenIDParam, string sViewParam,
            MIDRetail.DataCommon.ePlanSessionType ePlanSessionTypeParam, int nodeRID, string sVersionParam, int cdrRID, bool showBasis,
            List<ROBasisProfile> basisProfiles = null, eStorePlanSelectedGroupBy eStorePlanSelectedGroupByParam = eStorePlanSelectedGroupBy.ByTimePeriod,
            string sLowLevelVersionParam = null, int filterRID = Include.Undefined, MIDRetail.DataCommon.eLowLevelsType eLowLevelsTypeParam = eLowLevelsType.None,
            int lowLevelsOffset = 0, int lowLevelsSequence = 0, int ollRID = Include.Undefined, int customOLLRID = Include.Undefined,
            int viewKey = Include.Undefined, int versionKey = Include.Undefined, int lowLevelVersionKey = Include.Undefined)
        {
            _sROUserID = sROUserID;
            _sROSessionID = sROSessionID;
            _ROInstanceID = ROInstanceID;
            _ROClass = ROParms.GetClassForScreen(eScreenIDParam);
            _RORequest = eRORequest.OpenCube;

            _eScreenID = eScreenIDParam;
            _sView = sViewParam;
            _ePlanSessionType = ePlanSessionTypeParam;
            _nodeRID = nodeRID;
            _sVersion = sVersionParam;
            _cdrRID = cdrRID;
            _showBasis = showBasis;
            _basisProfiles = basisProfiles;
            _viewKey = viewKey;
            _versionKey = versionKey;
            _eStorePlanSelectedGroupBy = eStorePlanSelectedGroupByParam;
            _sLowLevelVersion = sLowLevelVersionParam;
            _lowLevelVersionKey = lowLevelVersionKey;
            _filterRID = filterRID;
            _lowLevelsType = eLowLevelsTypeParam;
            _lowLevelsOffset = lowLevelsOffset;
            _lowLevelsSequence = lowLevelsSequence;
            _ollRID = ollRID;
            _customOLLRID = customOLLRID;
        }
        //  String Variables for Key Parameters are removed in this call
        public ROCubeOpenParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
            eScreenID eScreenIDParam, MIDRetail.DataCommon.ePlanSessionType ePlanSessionTypeParam, int nodeRID, int cdrRID, bool showBasis, int viewKey, int versionKey,
            //begin optional parameters
            List<ROBasisProfile> basisProfiles = null, eStorePlanSelectedGroupBy eStorePlanSelectedGroupByParam = eStorePlanSelectedGroupBy.ByTimePeriod, int filterRID = Include.Undefined,
             //begin optional Multi Level parameters
             MIDRetail.DataCommon.eLowLevelsType eLowLevelsTypeParam = eLowLevelsType.None, int lowLevelVersionKey = Include.Undefined, int lowLevelsOffset = 0, int lowLevelsSequence = 0,
             int ollRID = Include.Undefined, int customOLLRID = Include.Undefined,
            //begin optional Store Level parameters
            int storeGroupRID = Include.Undefined, int storeNodeRID = Include.Undefined, int storeVersionKey = Include.Undefined, bool ineligibleStores = false,
            bool similarStore = false, int storeKey = Include.Undefined) :
            //base parameters
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _eScreenID = eScreenIDParam;
            _ePlanSessionType = ePlanSessionTypeParam;
            _nodeRID = nodeRID;
            _cdrRID = cdrRID;
            _showBasis = showBasis;
            _viewKey = viewKey;
            _versionKey = versionKey;
            _basisProfiles = basisProfiles;
            _eStorePlanSelectedGroupBy = eStorePlanSelectedGroupByParam;
            _filterRID = filterRID;
            _lowLevelsType = eLowLevelsTypeParam;
            _lowLevelVersionKey = lowLevelVersionKey;
            _lowLevelsOffset = lowLevelsOffset;
            _lowLevelsSequence = lowLevelsSequence;
            _ollRID = ollRID;
            _customOLLRID = customOLLRID;
            _storeGroupRID = storeGroupRID;
            _storeNodeRID = storeNodeRID;
            _storeVersionKey = storeVersionKey;
            _ineligibleStores = ineligibleStores;
            _similarStore = similarStore;
            _storeKey = storeKey;
        }

        public eScreenID eScreenID { get { return _eScreenID; } }
        public string sView { get { return _sView; } }
        public int ViewKey { get { return _viewKey; } }
        public MIDRetail.DataCommon.ePlanSessionType ePlanSessionType { get { return _ePlanSessionType; } }
        public int NodeRID { get { return _nodeRID; } }
        public string sVersion { get { return _sVersion; } }
        public int VersionKey { get { return _versionKey; } }
        public int cdrRID { get { return _cdrRID; } }
        public bool showBasis { get { return _showBasis; } }

        public List<ROBasisProfile> BasisProfiles
        {
            get { return _basisProfiles; }
            set { _basisProfiles = value; }
        }

        public MIDRetail.DataCommon.eStorePlanSelectedGroupBy eStorePlanSelectedGroupBy { get { return _eStorePlanSelectedGroupBy; } }
        public string sLowLevelVersion { get { return _sLowLevelVersion; } }
        public int LowLevelVersionKey { get { return _lowLevelVersionKey; } }
        public int filterRID { get { return _filterRID; } }
        public MIDRetail.DataCommon.eLowLevelsType eLowLevelsType { get { return _lowLevelsType; } }
        public int lowLevelsOffset { get { return _lowLevelsOffset; } }
        public int lowLevelsSequence { get { return _lowLevelsSequence; } }
        public int ollRID { get { return _ollRID; } }
        public int customOLLRID { get { return _customOLLRID; } }


        public int StoreGroupRID { get { return _storeGroupRID; } }
        public int StoreNodeRID { get { return _storeNodeRID; } }
        public int StoreFVRID { get { return _storeVersionKey; } }
        public int StoreVersionKey { get { return _storeVersionKey; } }
        public bool IneligibleStores { get { return _ineligibleStores; } }
        public bool SimilarStore { get { return _similarStore; } }
        public string storeRID { get { return _storeRID; } }
        public int storeKey { get { return _storeKey; } }

    };


    [DataContract(Name = "ROBasisProfile", Namespace = "http://Logility.ROWeb/")]
    public class ROBasisProfile
    {
        [DataMember(IsRequired = true)]
        private string _basisName;
        [DataMember(IsRequired = false)]
        //private eTyLyType _basisType;
        //[DataMember(IsRequired = false)]
        private bool _isDynToPlan;
        [DataMember(IsRequired = true)]
        private int _basisId;
        [DataMember(IsRequired = true)]
        private List<ROBasisDetailProfile> _basisDetailProfiles;

        public int BasisId
        {
            get { return _basisId; }
            set { _basisId = value; }
        }


        /// <summary>
        /// Creates a new instance of BasisProfile using the given Id.
        /// </summary>
        /// <param name="aKey">
        /// The Id of this profile.
        /// </param>

        public ROBasisProfile(int iBasisId, string sName)

        {
            _basisId = iBasisId;
            _basisName = sName;
            _isDynToPlan = false;   // Issue 
        }

        public List<ROBasisDetailProfile> BasisDetailProfiles
        {
            get { return _basisDetailProfiles; }
            set { _basisDetailProfiles = value; }
        }

        /// <summary>
        /// Returns the eProfileType of this profile.
        /// </summary>
        public eProfileType ProfileType
        {
            get
            {
                return eProfileType.Basis;
            }
        }

        /// <summary>
        /// Gets the name of this profile.
        /// </summary>
        public string BasisName
        {
            get
            {
                return _basisName;
            }
            set
            {
                _basisName = value;
            }
        }

        ///// <summary>
        ///// Describes what type of basis this is for TY/LY Trend processing. 
        ///// </summary>
        //public eTyLyType BasisType
        //{
        //    get
        //    {
        //        return _basisType;
        //    }
        //    set
        //    {
        //        _basisType = value;
        //    }
        //}

        /// <summary>
        /// Used by the forecast spread 
        /// </summary>
        /// 
        public bool IsDynToPlan
        {
            get
            {
                return _isDynToPlan;
            }
            set
            {
                _isDynToPlan = value;
            }
        }

    }

    [DataContract(Name = "ROBasisDetailProfile", Namespace = "http://Logility.ROWeb/")]
    public class ROBasisDetailProfile
    {
        [DataMember(IsRequired = true)]
        private bool _isIncluded;
        [DataMember(IsRequired = true)]
        private string _includeButton;
        [DataMember(IsRequired = true)]
        private string _picture;
        [DataMember(IsRequired = true)]
        private int _daterangeId;
        [DataMember(IsRequired = true)]
        private string _dateRange;
        [DataMember(IsRequired = true)]
        private int _versionId;
        [DataMember(IsRequired = true)]
        private string _version;
        [DataMember(IsRequired = true)]
        private int _merchandiseId;
        [DataMember(IsRequired = true)]
        private string _merchandise;
        [DataMember(IsRequired = true)]
        private int _basisId;
        [DataMember(IsRequired = true)]
        private float _weight;


        public bool IsIncluded
        {
            get { return _isIncluded; }
            set { _isIncluded = value; }
        }

        public string IncludeButton
        {
            get { return _includeButton; }
            set { _includeButton = value; }
        }

        public string Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        public int DateRangeId
        {
            get { return _daterangeId; }
            set { _daterangeId = value; }
        }

        public string DateRange
        {
            get { return _dateRange; }
            set { _dateRange = value; }
        }

        public int VersionId
        {
            get { return _versionId; }
            set { _versionId = value; }
        }

        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public int MerchandiseId
        {
            get { return _merchandiseId; }
            set { _merchandiseId = value; }
        }

        public string Merchandise
        {
            get { return _merchandise; }
            set { _merchandise = value; }
        }

        public int BasisId
        {
            get { return _basisId; }
            set { _basisId = value; }
        }

        public float Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        /// <summary>
        /// Creates a new instance of BasisDetailProfile.
        /// </summary>
        public ROBasisDetailProfile(int iBasisId, int iMerchandiseId, string sMerchandise, int iVersionId, string sVersion, int iDaterangeId,
            string sDateRange, string sPicture, float fWeight, bool bIsIncluded, string sIncludeButton)

        {
            _basisId = iBasisId;
            _merchandiseId = iMerchandiseId;
            _merchandise = sMerchandise;
            _versionId = iVersionId;
            _version = sVersion;
            _dateRange = sDateRange;
            _daterangeId = iDaterangeId;
            _picture = sPicture;
            _weight = fWeight;
            _isIncluded = bIsIncluded;
            _includeButton = sIncludeButton;
        }
    }

    [DataContract(Name = "ROForecastingBasisDetailsProfile", Namespace = "http://Logility.ROWeb/")]
    public class ROForecastingBasisDetailsProfile : ROBasisDetailProfile
    {
        [DataMember(IsRequired = true)]
        private eMerchandiseType _merchandiseType;

        public eMerchandiseType MerchandiseType
        {
            get { return _merchandiseType; }
            set { _merchandiseType = value; }
        }
        [DataMember(IsRequired = true)]
        private int _merchPhRid;

        public int MerchPhRId
        {
            get { return _merchPhRid; }
            set { _merchPhRid = value; }
        }
        [DataMember(IsRequired = true)]
        private int _merchPhlSequence;

        public int MerchPhlSequence
        {
            get { return _merchPhlSequence; }
            set { _merchPhlSequence = value; }
        }
        [DataMember(IsRequired = true)]
        private int _merchOffset;

        public int MerchOffset
        {
            get { return _merchOffset; }
            set { _merchOffset = value; }
        }
        [DataMember(IsRequired = true)]
        private eTyLyType _tyLyType;

        public eTyLyType TyLyType
        {
            get { return _tyLyType; }
            set { _tyLyType = value; }
        }



        public ROForecastingBasisDetailsProfile(int iBasisId, int iMerchandiseId, string sMerchandise, int iVersionId, string sVersion, int iDaterangeId,
            string sDateRange, string sPicture, float fWeight, bool bIsIncluded, string sIncludeButton,eTyLyType tyLyType,  eMerchandiseType merchandiseType, int iMerchPhRId,
            int iMerchPhlSequence, int iMerchOffset

            ) : base(iBasisId, iMerchandiseId, sMerchandise, iVersionId, sVersion, iDaterangeId, sDateRange, sPicture, fWeight, bIsIncluded, sIncludeButton)
        {
            _tyLyType = tyLyType;
            _merchandiseType = merchandiseType;
            _merchPhRid = MerchPhRId;
            _merchPhlSequence = iMerchPhlSequence;
            _merchOffset = iMerchOffset;
        }
    }


    [DataContract(Name = "ROCubeGetMetadataParams", Namespace = "http://Logility.ROWeb/")]
    public class ROCubeGetMetadataParams : ROParms
    {
        [DataMember(IsRequired = true)]
        private string _sView;
        [DataMember(IsRequired = true)]
        private int _viewKey;


        public ROCubeGetMetadataParams(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, string view, int viewKey) :
            base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _sView = view;
            _viewKey = viewKey;
        }

        public ROCubeGetMetadataParams(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID, int viewKey) :
             base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _viewKey = viewKey;
        }
        public ROCubeGetMetadataParams(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID) :
             base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
        }

        public ROCubeGetMetadataParams(string sROUserID, string sROSessionID, eScreenID eScreenIDParam, long ROInstanceID, string view)
        {
            _sROUserID = sROUserID;
            _sROSessionID = sROSessionID;
            _ROInstanceID = ROInstanceID;
            _ROClass = ROParms.GetClassForScreen(eScreenIDParam);
            _RORequest = eRORequest.GetROCubeMetadata;
            _sView = view;
        }

        public string view { get { return _sView; } }
        public int viewKey { get { return _viewKey; } }
    }

    [DataContract(Name = "ROCubeTableParams", Namespace = "http://Logility.ROWeb/")]
    public class ROCubeTableParams
    {
        public const int RETURN_ALL_ROWS = -1;
        public const string INVALID_ROW_ID = null;

        [DataMember(IsRequired = true)]
        private string _tableName;
        [DataMember(IsRequired = true)]
        private string _nextTableRowID;
        [DataMember(IsRequired = true)]
        private int _maxRowCount;
        [DataMember(IsRequired = true)]
        private List<string> _columnNames;

        public ROCubeTableParams(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("must supply a non null, non empty table name");
            }

            _tableName = tableName;
            _nextTableRowID = INVALID_ROW_ID;
            _maxRowCount = 0;
            _columnNames = new List<string>();
        }

        public string tableName { get { return _tableName; } }

        public string nextTableRowID
        {
            get { return _nextTableRowID; }
            set
            {
                _nextTableRowID = value;
            }
        }

        public int maxRowCount
        {
            get { return _maxRowCount; }
            set
            {
                if (value < RETURN_ALL_ROWS)
                {
                    value = RETURN_ALL_ROWS;
                }
                _maxRowCount = value;
            }
        }

        public bool ReturnAllRows()
        {
            return _maxRowCount == RETURN_ALL_ROWS;
        }

        public bool GetAllColumns()
        {
            return _columnNames.Count < 1;
        }

        public IEnumerable<string> columnNames
        {
            get
            {
                return _columnNames as IEnumerable<string>;
            }

            set
            {
                if (value != null)
                {
                    _columnNames = new List<string>(value);
                }
                else
                {
                    _columnNames = new List<string>();
                }
            }
        }
    }

    [DataContract(Name = "ROCubeGetDataParams", Namespace = "http://Logility.ROWeb/")]
    public class ROCubeGetDataParams : ROCubeGetMetadataParams
    {
        [DataMember(IsRequired = true)]
        private bool _bTransposeTables; // if true, swap rows for columns and vice-versa
        [DataMember(IsRequired = true)]
        private Dictionary<string, ROCubeTableParams> _ROCubeTableParamsByName;
        [DataMember(IsRequired = true)]
        private string _sUnitScaling;
        [DataMember(IsRequired = true)]
        private string _sDollarScaling;
        [DataMember(IsRequired = true)]
        private int _iStartingRowIndex;
        [DataMember(IsRequired = true)]
        private int _iNumberOfRows;
        [DataMember(IsRequired = true)]
        private int _iStartingColIndex;
        [DataMember(IsRequired = true)]
        private int _iNumberOfColumns;
        [DataMember(IsRequired = true)]
        private eGridOrientation _gridOrientation;
        [DataMember(IsRequired = true)]
        private int _storeAttributeSetKey;
        [DataMember(IsRequired = true)]
        private int _storeAttributeKey;
        [DataMember(IsRequired = true)]
        private int _viewKey;
        [DataMember(IsRequired = true)]
        private int _filterKey;

        /// <summary>
        /// Get the next set of data using the provided class and request IDs
        /// </summary>
        /// <param name="bTransposeTablesParam">if true, swap rows for columns and vice-versa</param>
        public ROCubeGetDataParams(string sROUserID, string sROSessionID, eROClass ROClass, long ROInstanceID, IEnumerable<ROCubeTableParams> tableParams,
                                   string unitScaling, string dollarScaling, bool bRecomputeFirst, bool bTransposeTablesParam, string sView, int iStartingRowIndex, int iNumberOfRows, int iStartingColIndex, int iNumberOfColumns, eGridOrientation gridOrientation,
                                   int viewKey = Include.Undefined, int filterKey = Include.Undefined, int storeAttributeKey = Include.Undefined, int storeAttributeSetKey = Include.Undefined)
            : base(sROUserID, sROSessionID, ROClass, eRORequest.GetCubeData, ROInstanceID, sView, viewKey)
        {
            Init(tableParams, unitScaling, dollarScaling, bRecomputeFirst, bTransposeTablesParam, iStartingRowIndex, iNumberOfRows, iStartingColIndex, iNumberOfColumns, gridOrientation,
                viewKey, filterKey, storeAttributeKey, storeAttributeSetKey);
        }

        public ROCubeGetDataParams(string sROUserID, string sROSessionID, eROClass ROClass, long ROInstanceID, bool bRecomputeFirst, bool bTransposeTablesParam, string view, int iStartingRowIndex, int iNumberOfRows, int iStartingColIndex, int iNumberOfColumns, eGridOrientation gridOrientation,
                                    int viewKey = Include.Undefined, int filterKey = Include.Undefined, int storeAttributeKey = Include.Undefined, int storeAttributeSetKey = Include.Undefined)
            : this(sROUserID, sROSessionID, ROClass, ROInstanceID, null, "1", "1", bRecomputeFirst, bTransposeTablesParam, view, iStartingRowIndex, iNumberOfRows, iStartingColIndex, iNumberOfColumns, gridOrientation,
                    viewKey, filterKey, storeAttributeKey, storeAttributeSetKey)
        {
        }

        /// <summary>
        /// Get the next set of data using the provided row IDs as a starting point where a max number of rows per DataTable needs to be specified.
        /// </summary>
        /// <param name="bTransposeTablesParam">if true, swap rows for columns and vice-versa</param>
        public ROCubeGetDataParams(string sROUserID, string sROSessionID, eScreenID eScreenID, long ROInstanceID, IEnumerable<ROCubeTableParams> tableParams,
                                   string unitScaling, string dollarScaling, bool bRecomputeFirst, bool bTransposeTablesParam, string sView, int iStartingRowIndex, int iNumberOfRows, int iStartingColIndex, int iNumberOfColumns, eGridOrientation gridOrientation,
                                   int viewKey = Include.Undefined, int filterKey = Include.Undefined, int storeAttributeKey = Include.Undefined, int storeAttributeSetKey = Include.Undefined)
            : base(sROUserID, sROSessionID, eScreenID, ROInstanceID, sView)
        {
            Init(tableParams, unitScaling, dollarScaling, bRecomputeFirst, bTransposeTablesParam, iStartingRowIndex, iNumberOfRows, iStartingColIndex, iNumberOfColumns, gridOrientation,
                viewKey, filterKey, storeAttributeKey, storeAttributeSetKey);
        }

        public ROCubeGetDataParams(string sROUserID, string sROSessionID, eScreenID eScreenID, long ROInstanceID, bool bRecomputeFirst, bool bTransposeTablesParam, string sView, int iStartingRowIndex, int iNumberOfRows, int iStartingColIndex, int iNumberOfColumns, eGridOrientation gridOrientation,
                                    int viewKey = Include.Undefined, int filterKey = Include.Undefined, int storeAttributeKey = Include.Undefined, int storeAttributeSetKey = Include.Undefined)
            : this(sROUserID, sROSessionID, eScreenID, ROInstanceID, null, "1", "1", bRecomputeFirst, bTransposeTablesParam, sView, iStartingRowIndex, iNumberOfRows, iStartingColIndex, iNumberOfColumns, gridOrientation,
                    viewKey, filterKey, storeAttributeKey, storeAttributeSetKey)
        {
        }

        /// <summary>
        /// Get the data using int parameters and unit/dollar scaling
        /// /// </summary>
        /// <param name="bTransposeTablesParam">if true, swap rows for columns and vice-versa</param>
        public ROCubeGetDataParams(string sROUserID, string sROSessionID, eROClass ROClass, long ROInstanceID,
                                    IEnumerable<ROCubeTableParams> tableParams, string unitScaling, string dollarScaling,
                                    bool bRecomputeFirst, bool bTransposeTablesParam,
                                    int iStartingRowIndex, int iNumberOfRows, int iStartingColIndex, int iNumberOfColumns, eGridOrientation gridOrientation,
                                    int viewKey, int filterKey = Include.Undefined, int storeAttributeKey = Include.Undefined, int storeAttributeSetKey = Include.Undefined)
            : base(sROUserID, sROSessionID, ROClass, eRORequest.GetCubeData, ROInstanceID, viewKey)
        {
            Init(tableParams, unitScaling, dollarScaling, bRecomputeFirst, bTransposeTablesParam,
                iStartingRowIndex, iNumberOfRows, iStartingColIndex, iNumberOfColumns, gridOrientation,
                viewKey, filterKey, storeAttributeKey, storeAttributeSetKey);
        }


        private void Init(IEnumerable<ROCubeTableParams> tableParamsList, string unitScaling, string dollarScaling, bool bRecomputeFirst, bool bTransposeTablesParam,
                        int iStartingRowIndex, int iNumberOfRows, int iStartingColIndex, int iNumberOfColumns, eGridOrientation gridOrientation,
                        int viewKey, int filterKey = Include.Undefined, int storeAttributeKey = Include.Undefined, int storeAttributeSetKey = Include.Undefined)
        {
            if (tableParamsList == null)
            {
                tableParamsList = new List<ROCubeTableParams>();
            }

            _ROCubeTableParamsByName = new Dictionary<string, ROCubeTableParams>();
            foreach (ROCubeTableParams tableParams in tableParamsList)
            {
                _ROCubeTableParamsByName.Add(tableParams.tableName, tableParams);
            }
            _RORequest = bRecomputeFirst ? eRORequest.RecomputeCubes : eRORequest.GetCubeData;
            _sUnitScaling = unitScaling;
            _sDollarScaling = dollarScaling;
            _bTransposeTables = bTransposeTablesParam;
            _iStartingRowIndex = iStartingRowIndex;
            _iNumberOfRows = iNumberOfRows;
            _iStartingColIndex = iStartingColIndex;
            _iNumberOfColumns = iNumberOfColumns;
            _gridOrientation = gridOrientation;
            _viewKey = viewKey;
            _filterKey = filterKey;
            _storeAttributeKey = storeAttributeKey;
            _storeAttributeSetKey = storeAttributeSetKey;
        }

        public bool bTransposeTables { get { return _bTransposeTables; } }

        public bool GetAllTables()
        {
            return _ROCubeTableParamsByName.Count < 1;
        }

        /// <summary>
        /// Retrieve the table specific data parameters
        /// </summary>
        /// <param name="sTableName">The associated table's name</param>
        /// <returns>the corresponding table params or null if not found</returns>
        public ROCubeTableParams GetTableParams(string sTableName)
        {
            ROCubeTableParams returnVal = null;

            if (_ROCubeTableParamsByName.ContainsKey(sTableName))
            {
                returnVal = _ROCubeTableParamsByName[sTableName];
            }

            return returnVal;
        }

        public string sUnitScaling { get { return _sUnitScaling; } }

        public string sDollarScaling { get { return _sDollarScaling; } }

        public int iStartingRowIndex { get { return _iStartingRowIndex; } }

        public int iNumberOfRows { get { return _iNumberOfRows; } }

        public int iStartingColIndex { get { return _iStartingColIndex; } }

        public int iNumberOfColumns { get { return _iNumberOfColumns; } }

        public eGridOrientation GridOrientation { get { return _gridOrientation; } }

        public int StoreAttributeSetKey { get { return _storeAttributeSetKey; } }
        public int StoreAttributeKey { get { return _storeAttributeKey; } }
        public int ViewKey { get { return _viewKey; } }
        public int FilterKey { get { return _filterKey; } }
    };

    [DataContract(Name = "ROCubePeriodChangeParams", Namespace = "http://Logility.ROWeb/")]
    public class ROCubePeriodChangeParams : ROCubeGetMetadataParams
    {
        [DataMember(IsRequired = true)]
        private bool _bShowYears;    // if true, include a table in the dataset of yearly values
        [DataMember(IsRequired = true)]
        private bool _bShowSeasons;  // if true, include a table in the dataset of seasonal values
        [DataMember(IsRequired = true)]
        private bool _bShowQuarters; // if true, include a table in the dataset of quarterly values
        [DataMember(IsRequired = true)]
        private bool _bShowMonths;   // if true, include a table in the dataset of month values
        [DataMember(IsRequired = true)]
        private bool _bShowWeeks;    // if true, include a table in the dataset of weekly values

        public ROCubePeriodChangeParams(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
            bool bShowYearsParam, bool bShowSeasonsParam, bool bShowQuartersParam, bool bShowMonthsParam,
                                         bool bShowWeeksParam, bool bTransposeTablesParam, string sView, int viewKey = Include.Undefined)
            : base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID, sView, viewKey)
        {
            _RORequest = eRORequest.HandlePeriodChange;
            _bShowYears = bShowYearsParam;
            _bShowSeasons = bShowSeasonsParam;
            _bShowQuarters = bShowQuartersParam;
            _bShowMonths = bShowMonthsParam;
            _bShowWeeks = bShowWeeksParam;
        }

        public ROCubePeriodChangeParams(string sROUserID, string sROSessionID, eScreenID eScreenID, long ROInstanceID,
            bool bShowYearsParam, bool bShowSeasonsParam, bool bShowQuartersParam, bool bShowMonthsParam,
                                         bool bShowWeeksParam, bool bTransposeTablesParam, string sView)
            : base(sROUserID, sROSessionID, eScreenID, ROInstanceID, sView)
        {
            _RORequest = eRORequest.HandlePeriodChange;
            _bShowYears = bShowYearsParam;
            _bShowSeasons = bShowSeasonsParam;
            _bShowQuarters = bShowQuartersParam;
            _bShowMonths = bShowMonthsParam;
            _bShowWeeks = bShowWeeksParam;
        }

        public bool bShowYears { get { return _bShowYears; } }
        public bool bShowSeasons { get { return _bShowSeasons; } }
        public bool bShowQuarters { get { return _bShowQuarters; } }
        public bool bShowMonths { get { return _bShowMonths; } }
        public bool bShowWeeks { get { return _bShowWeeks; } }
    };

    [DataContract(Name = "ROGridCellChange", Namespace = "http://Logility.ROWeb/")]
    public class ROGridCellChange
    {
        [DataMember(IsRequired = true)]
        private eDataType _dataType;  // the table the change is from
        [DataMember(IsRequired = true)]
        private int _rowIndex;       // the row the change is from
        [DataMember(IsRequired = true)]
        private int _columnIndex; // the column the change is from
        [DataMember(IsRequired = true)]
        private double _dNewValue;   // the new value
        [DataMember(IsRequired = true)]
        private eROCellAction _eROCellAction;   // the new value

        //To Implement Cell Unlock and Lock functionality
        public enum eROCellAction
        {
            Lock = 1,
            Unlock = 2,
            CellChanged = 3,
            Close = 4,
            Open = 5,
            CascadeLock = 6,
            CascadeUnlock = 7,
        };


        public ROGridCellChange(eDataType dataType, int rowIndexParam, int columnIndexParm, double dNewValueParam, eROCellAction eROCellActionParam)
        {
            _dataType = dataType;
            _rowIndex = rowIndexParam;
            _columnIndex = columnIndexParm;
            _dNewValue = dNewValueParam;
            _eROCellAction = eROCellActionParam;
        }

        public eDataType DataType { get { return _dataType; } }
        public int RowIndex { get { return _rowIndex; } set { _rowIndex = value; } }
        public int ColumnIndex { get { return _columnIndex; } }
        public double dNewValue { get { return _dNewValue; } }
        public eROCellAction CellAction { get { return _eROCellAction; } }
    };

}
