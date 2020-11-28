using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Text;
//using System.Runtime.Remoting;
//using System.Runtime.Remoting.Lifetime;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using System.Collections;
using System.Collections.Generic;
using MIDRetail.Business.Allocation;
using System.Diagnostics;

namespace MIDRetail.Business
{
	/// <summary>
	/// The Transaction class defines the "local" workspace for a series of functions in the client application.
	/// </summary>
	/// <remarks>
	/// This class gives the user the ability to store information that is "local", or unique, to a series of screens or functions.  This allows a Client
	/// application to open multiple functions of the same type, yet each has its own copy of information contained in this class.
	/// </remarks>

	public class ApplicationSessionTransaction : Transaction
	{
		//=======
		// FIELDS
		//=======

        // Begin TT#1031 - JSmith - Serializaion error in audit
        //private Audit _audit; // MID Track 4372 Generic Size Constraints
        private Audit _audit = null; // MID Track 4372 Generic Size Constraints
        // End TT#1031
		private System.Collections.Hashtable _profileHash;
		private ProfileList _profileHashLastProfileList;

		private System.Collections.Hashtable _profileListGroupHash;
		private System.Collections.Hashtable _profileXRefHash;

		private System.Collections.Hashtable _versionProtectedHash;

		private System.Collections.Hashtable _subtotalKeyHash;
		private string _subtotalKeyHashLastKey;
		private int _subtotalKeyHashLastValue;

		private System.Collections.Hashtable _subtotalPacks;
		private PackHdr _subtotalPacksLastPack;
		private ArrayList _subtotalPacksLastPackArray;

		//private System.Collections.Hashtable _storeRID_IndexXref; // MID Track 4341 Performance Issues
		private Index_RID[] _storeRID_IndexXref; // NOTE:  index is storeRID //MID Track 4341 Performance Issues
		private Index_RID[] _storeIdxRIDArray;   // NOTE: index is store position within the StoreProfile List
		//private int _storeRIDLastRIDKey;       // MID Track 4341 Performance Issues
		//private Index_RID _storeRIDLastRIDValue; // MID Track 4341 Performance Issues
		private int[] _allStoreRIDList;
		private ArrayList _storeRID_CharArray_List; // MID Track 4341 Performance Issues

		private System.Collections.Hashtable _inStoreReceiptDays;
		private System.Collections.Hashtable _onHandHash;
		// BEGIN MID Track #2230   Display Current Week-to-Day Sales on Sku Review
        //private System.Collections.Hashtable _weekToDaySalesHash;
		// END MID Track #2230   Display Current Week-to-Day Sales on Sku Review
		private System.Collections.Hashtable _storeStockPlanHash;
		private System.Collections.Hashtable _storeSalesPlanHash;
		private System.Collections.Hashtable _ruleLayerHash;
        //private System.Collections.Hashtable _allocationActionStatusHash;  // TT#241 - MD JEllis - Header Enqueue Process
        private Dictionary<int, eAllocationActionStatus> _allocationActionStatusDict;                    // TT#241 - MD JEllis - Header Enqueue Process
		private System.Collections.Hashtable _headersChangedHash; // MID Track 5067 Release Action fails yet header is released
		private System.Collections.Hashtable _primarySizeCodeByRID;
		private System.Collections.Hashtable _primarySizeCodeByPrimary;
		private System.Collections.Hashtable _secondarySizeCodeByRID;
		private System.Collections.Hashtable _secondarySizeCodeBySecondary;
		private System.Collections.Hashtable _sizeCodeByPrimarySecondary;
        // begin TT#41 - MD - JEllis - Size Inventory Min Max
        //private System.Collections.Hashtable _sizeCurve; // MID Track #2937 Size Onhand is Incorrect
        private Dictionary<HeaderSizeNeedMethodKey, SizeNeedMethod> _sizeNeedMethodCache;
        private HeaderSizeNeedMethodKey? _lastHeaderSizeNeedMethodKey;
        private SizeNeedMethod _lastSizeNeedMethod;
        // end TT#41 - MD - JEllis - Size Inventory Min Max
		private int _tempSizeNeedMethodRID; // MID Track #2937 Size Onhand is Incorrect
        //private System.Collections.Hashtable _sizeOnhandHash; // MID Track #2937 Size Onhand Incorrect // TT#3060 - Loehman - Jellis - System Argument Error
        private Dictionary<StoreColorCurveOnHandKey, Dictionary<int, int>> _storeColorOnhandDict;     // TT#3060 - Loehman - Jellis - System Argument Error
        private StoreColorCurveOnHandKey _lastStoreColorOnHandKey;                                   // TT#3060 - Loehman - Jellis - System Argument Error
        private Dictionary<int, int> _lastStoreColorSizeOnHandDict;                             // TT#3060 - Loehman - Jellis - System Argument Error

		private System.Collections.Hashtable _nodeDataHash;
		private int _nodeDataHashLastKey;
		private HierarchyNodeProfile _nodeDataHashLastValue;

		private System.Collections.Hashtable _planLevelDataHash;
		private int _planLevelDataHashLastKey;
		private HierarchyNodeProfile _planLevelDataHashLastValue;

		private Hashtable _OTS_NeedHorizonHash; // MID Track 4309 OTS basis onhand not summed
		private OTS_NeedHorizon _OTS_NeedHorizon; // MID Track 4309 OTS basis onhand not summed
		private Hashtable _parentHnHash; // MID Track 4312 Size Intransit not relieved at style total
		private HierarchyNodeProfile _parentHnHashLastParent; // MID Track 4312 Size Intransit not relieved at style total
		private Hashtable _IKT_Hash; // MID Track 4312 Size Intransit not relieved at style total
		private Hashtable _IKT_HashLast_iktHash; // MID Track 4312 Size Intransit not relieved at style total
		private int _IKT_HashLastKey; // MID Track 4312 Size Intransit not relieved at style total
		//private long _OTS_HorizonKey;           // MID Change j.ellis add need methods for OTS  // MID Track 4309 OTS basis onhand not summed
		//private bool _OTS_BeginWeekIsCurrent;   // MID Change j.ellis add need methods for OTS  // MID Track 4309 OTS basis onhand not summed
		private DayProfile _OTS_CurrentSalesDay; // MID Change j.ellis add need methods for OTS
		private WeekProfile _OTS_CurrentSalesWeek; // MID Track 4309 OTS basis onhand not summed
		//private WeekProfile _OTS_BeginWeek;     // MID Change j.ellis add need methods for OTS  // MID Track 4309 OTS basis onhand not summed
		//private WeekProfile _OTS_EndWeek;       // MID Change j.ellis add need methods for OTS  // MID Track 4309 OTS basis onhand not summed
		private Hashtable _OTS_NeedHash;        // MID Change j.ellis add need methods for OTS
		private int _OTS_NeedHashLastKey;       // MID Change j.ellis add need methods for OTS
		private Hashtable _OTS_VerHash;         // MID Change j.ellis add need methods for OTS
		private int _OTS_VerHashLastKey;        // MID Change j.ellis add need methods for OTS
		private Hashtable _OTS_HorizonHash;     // MID Track 4309 OTS basis onhand not summed
		private long _OTS_HorizonHashLastKey;   // MID Track 4309 OTS basis onhand not summed
		private Hashtable _OTS_StoreHash;       // MID Change j.ellis add need methods for OTS
		private int _OTS_StoreHashLastKey;      // MID Change j.ellis add need methods for OTS
		private double[] _OTS_LastStoreValues;  // MID Change j.ellis add need methods for OTS

		private HierarchyProfile _mainHierarchyData; // MID Track 4020 Plan level not reset on cancel
		private int _colorHierarchySequence;         // MID Track 4020 Plan level not reset on cancel
 
		private StoreGroupProfile _currStoreGroupProfile;
		private Dictionary<long,ProfileList> _storeGroupDict; // MID Track 5820 AnF Defect 1819 - Unhandled Exception After Store Activation 
		private string _currComputationMode;
		private IPlanComputations _planComputations;
		private StorePlanMaintCubeGroup _allocationCubeGroup;
		private StorePlanMaintCubeGroup _allocationBasisCubeGroup;
		private ForecastCubeGroup _forecastCubeGroup;
		private string _grandTotalName;
		private AllocationSubtotalProfile _grandTotalProfile;
        private AssortmentProfile _assortmentProfile; // TT#1154 - MD - Jellis - Group Allocation Infinite  Loop When Going to SIze Analysis
        private MasterHeaderProfile _masterHeader = null;  // TT#1966-MD - JSmith - DC Fulfillment
		private Index_RID _reserveStoreIndexRID;
		//		private DateTime _inTransitFlashBack;
		private IntransitReader _intransitRdr;
		private OnHandReader _onHandRdr;
		// BEGIN TT#1401 - stodd - add resevation stores (IMO)
		private IMO_Reader _IMORdr;
		// END TT#1401 - stodd - add resevation stores (IMO)
        //Begin TT#739-MD -jsobek -Delete Stores -VSW
        //private StoreVswReverseOnhand _storeVswReverseOnhand; // TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4
        private VswReverseOnhandVariableManager _storeVswReverseOnhand; 
        //End TT#739-MD -jsobek -Delete Stores -VSW
        private BuildItemIndicators _lastBuildItemIndicators;               // TT#488 - MD - Jellis - Group Allocation
        private Dictionary<int, BuildItemIndicators> _buildItemIndicators;  // TT#488 - MD - Jellis - Group Allocation
        private Dictionary<int, Dictionary<int, AllocationProfile[]>> _buildItemProcessOrder; // TT#488 - MD - Jellis - Group Allocation
        // BEGIN MID Track #2230   Display Current Week-to-Day Sales on Sku Review
		private DailySalesReader _dailySalesRdr;
		// END MID Track #2230   Display Current Week-to-Day Sales on Sku Review
		private AllocationViewSelectionCriteria _allocationCriteria;
		private AssortmentViewSelectionCriteria _assortmentCriteria;	// TT#2 - stodd - assortment
        private eAssortmentBasisLoadedFrom _assortmentViewLoadedFrom;
//		private ArrayList _buildWaferColumns = new ArrayList();  // MID Track 4426 Need Section goes to Zero
		private ArrayList _buildStyleWaferColumns;               // MID Track 4426 Need Section goes to Zero
		private ArrayList _buildSizeWaferColumns;                // MID Track 4426 Need Section goes to Zero
		private VelocityMethod _velocityCriteria;
		private bool _determineShowNeedAnalysis;
		private bool _showNeedAnalysisCriteria;
		private bool _needAnalysisFrom1stHeader;
        private bool _allocationWafersNeedRebuilt = false;  // TT#2067-MD - JSmith/AGallagher - PPK & Bulk Header-Process Minimum rule-select Vel Str Detail- receive mssg Pack is not defined on subtotal
        private BuildPacksMethod _buildPacksCriteria;            // TT#370 Build Packs Enhancement
		private HierarchySessionTransaction _hierarchySessionTransaction = null;
		private eOTSPlanActionStatus _OTSPlanActionStatus;
		//		private ArrayList _selectedComponentList;
        //private HeaderEnqueue _headerEnq;
		private string _headerEnqMessage;
		private int _allocationStoreFilterRID;
		private bool _quickFilterChanged;
		private ProfileList _selectedAllocationStores;
		private long _pctToTotalDecimalPrecision;
		private double[] _velocityStorePctSellThruIdx_Chain;
		private double [] _velocityStorePctSellThruIdx_Set;
		private bool _rebuildAllocationStores;
		private bool _rebuildAllocationStoresActive;
		private bool _needHeaders = true;
		// Added for assortment
        private int _userRID;  // TT#1185 - JEllis - Verify ENQ before Update (part 2) -- duplicate field!
		private string _userId;
        private int _clientThreadID;   // TT#1185 - JEllis - Verify ENQ before Update (part 2)

		// To cache eligibility information, two Hashtable are required for each sales and stock.
		// The outermost Hashtable is keyed by nodeRID and contains a Hashtable of dates as the value.
		// The innermost Hashtable is keyed by year/week and contains a BitArray indexed by storeRID
		// which contains the eligibility settings.
		private System.Collections.Hashtable _salesEligibilityHashByNodeRID;
		private System.Collections.Hashtable _stockEligibilityHashByNodeRID;
		private System.Collections.Hashtable _priorityShippingHashByNodeRID;
		private System.Collections.Hashtable _salesEligibilityHashByYearWeek;
		private System.Collections.Hashtable _stockEligibilityHashByYearWeek;
		private System.Collections.Hashtable _priorityShippingHashByYearWeek;
		private System.Collections.Hashtable _salesWkRangeEligibilityHashByNodeRID; // MID Track #2539 Grades not same in OTS and Allocation
		private System.Collections.Hashtable _salesEligibilityHashByWeekRange;      // MID Track #2539 Grades not same in OTS and Allocation
		private System.Collections.BitArray _salesEligibilityBitArray;
		private System.Collections.BitArray _salesWkRangeEligibilityBitArray;  // MID Track #2539 Grades not same in OTS and Allocation

		private System.Collections.BitArray _stockEligibilityBitArray;
		private System.Collections.BitArray _priorityShippingBitArray;
		private int _currentSalesEligibilityNodeRID = -1;
		private int _currentSalesEligibilityYearWeek = -1;
		private int _currentStockEligibilityNodeRID = -1;
		private int _currentStockEligibilityYearWeek = -1;
		private int _currentPriorityShippingNodeRID = -1;
		private int _currentPriorityShippingYearWeek = -1;
		private int _currentSalesWkRangeEligibilityNodeRID = -1;  // MID Track # 2539 Grades not same in OTS and Allocation
		private long _currentSalesEligibilityWeekRange = -1;      // MID Track #2539 Grades not same in OTS and Allocation

		// To cache daily percentages, two Hashtable are required.
		// The outermost Hashtable is keyed by nodeRID and contains a Hashtable of dates as the value.
		// The innermost Hashtable is keyed by year/week and contains a Hashtable keyed by storeRID
		// which contains the daily percentages values.
		private System.Collections.Hashtable _dailyPercentagesHashByNodeRID;
		private System.Collections.Hashtable _dailyPercentagesHashByYearWeek;
		private int _currentDailyPercentagesNodeRID = Include.NoRID;
		private int _currentDailyPercentagesYearWeek = Include.NoRID;
		private StoreWeekDailyPercentagesList _storeWeekDailyPercentagesList;
		
		private System.Collections.Hashtable _storeGradesHash;
		private System.Collections.Hashtable _velocityGradesHash;
		private System.Collections.Hashtable _sellThruPctsHash;
		private System.Collections.Hashtable _storeStatusHash;
		private System.Collections.Hashtable _storeCapacityHash;
		private System.Collections.Hashtable _similarStoreHash;

		// To cache sales and stock modifier information, two Hashtable are required for each sales and stock.
		// The outermost Hashtable is keyed by nodeRID and contains a Hashtable of dates as the value.
		// The innermost Hashtable is keyed by year/week and contains a Hashtable indexed by storeRID
		// which contains the modifier settings.
		private System.Collections.Hashtable _salesModifierHashByNodeRID;
		private System.Collections.Hashtable _stockModifierHashByNodeRID;
		private System.Collections.Hashtable _salesModifierHashByYearWeek;
		private System.Collections.Hashtable _stockModifierHashByYearWeek;
		private System.Collections.Hashtable _salesModifierHash;
		private System.Collections.Hashtable _stockModifierHash;
		private int _currentSalesModifierNodeRID = Include.NoRID;
		private int _currentSalesModifierYearWeek = Include.NoRID;
		private int _currentStockModifierNodeRID = Include.NoRID;
		private int _currentStockModifierYearWeek = Include.NoRID;
		// BEGIN MID Track #4370 - John Smith - FWOS Models
		private System.Collections.Hashtable _FWOSModifierHashByNodeRID;
		private System.Collections.Hashtable _FWOSModifierHashByYearWeek;
		private System.Collections.Hashtable _FWOSModifierHash;
		private int _currentFWOSModifierNodeRID = Include.NoRID;
		private int _currentFWOSModifierYearWeek = Include.NoRID;
		// END MID Track #4370
		// BEGIN MID Track #4827 - John Smith - Presentation plus sales
		private System.Collections.Hashtable _PMPlusSalesHashByNodeRID;
		private System.Collections.BitArray _PMPlusSalesBitArray;
		private int _currentPMPlusSalesNodeRID = Include.NoRID;
		// END MID Track #4827
		private System.Windows.Forms.UserControl _allocationWorkspaceExplorer;
		private System.Windows.Forms.Form _velocityWindow;
		private ArrayList _forecastingOverrideList;
		private AllocationProfileList _linkedHeaderList;
		private ArrayList _forecastingBalanceOverrideList;
		// begin MID Track 4341 Performance Issues
		private Hashtable _storeRID_ToCharArrayHash;
		private Hashtable _OTS_IT_HnRIDHash;   // holds intransit hash table _OTS_IT_HorizonHash for each HnRID
		private Hashtable _OTS_IT_HorizonHash; // holds intransit _OTS_IT_StoreValues for given horizon within last HnRID
		private int[] _OTS_IT_StoreValues;     // store intransit values for given HnRID and Horizon
        // end MID Track 4341 Performance Issues
		private ArrayList _rebuildIntransitUpdateRequest;  // TT#1137 (Issue 4351) Rebuild Intransit Utility
		// begin MID Track 4362 Alternate Intransit Performance
		private Hashtable _intransitReadNodeHash;
		private Hashtable _lastIntransitReadPllvHash;
		private ArrayList _lastIntransitReadNodeArray;
		private int _lastIntransitReadNodeHashKey;
		private eHierarchyLevelMasterType _lastIntransitReadNodeHashLevel;
		// end MID Track 4362 Alternate Intransit Performance
		// end MID Track 4341 Performance Issues
        // begin TT#488 - MD - Jellis - Group Allocation (fields not used)
        //// BEGIN TT#1401 - stodd - add resevation stores (IMO)
        //private Hashtable _IMOReadNodeHash;
        //private Hashtable _lastIMOReadPllvHash;
        //private ArrayList _lastIMOReadNodeArray;
        //private int _lastIMOReadNodeHashKey;
        //private eHierarchyLevelMasterType _lastIMOReadNodeHashLevel;
        //// END TT#1401 - stodd - add resevation stores (IMO)
        // end TT#488 - MD - Jellis - Group Allocation (fields not used)
		// begin MID Track 4372 Generic Size Constraints
        private Hashtable _headerGenericNameCharacteristics;
		private DataTable _lastHeaderGenericNameCharacteristicsDT;
		private int _lastGenericNameCharacteristicHeaderKey;
		private Hashtable _genericHeaderCharacteristicHash;
		private long _lastGenericHeaderCharacteristicKey;
		private Hashtable _lastGenericMerchandiseHash;
		private HierarchyNodeProfile _lastGenericMerchandiseHierarchyNodeProfile;
		private Hashtable _lastGenericColorHash;
		private int _lastGenericColorCodeRID;
		private string _lastGenericSizeName;
		private char _productLevelDelimiter;
		private HeaderCharGroupProfileList _headerCharacteristicProfileList;
		private HeaderCharGroupProfile _lastHeaderCharacteristicProfile;
		private int _lastHeaderCharacteristicGroupRID;
		// end MID Track 4372 Generic Size Constraints
		// begin 4448 AnF Audit Enhancement
		private AuditData _auditData;
		private int _processRID;
        //private int _userRID;      // TT#1185 - JEllis - Verify ENQ before Update (part 2) -- duplicate field
		// end 4448 AnF Audit Enhancement
        private ArrayList _sizeViewSizesAL; // TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
        // Begin TT#1031 - JSmith - Serializaion error in audit
        private Session _session = null;
        // End TT#1031
        private ArrayList _allocationWaferChangeRequest;   // TT#59 Implement Temp Locks

        private bool _assmntViewSelBypass; //TT#1962 - DOConnell - When selecting a single assortment from the assrt workspace, review screen is using user selection basis.
		// Begin TT#1705 - JSmith - Reset Header with Piggybacking
        private bool _processingCustomReset = false;
        // End TT#1705

        private RuleBasedAllocation _ruleBasedAllocation; // TT#586 - MD - Jellis - Assortment Add 2 dimensional spread
		// BEGIN TT#2225 - stodd - VSW ANF Enhancement (IMO)
		private CustomBusinessRoutines _buisnessRoutines = null;
		// END TT#2225 - stodd - VSW ANF Enhancement (IMO)

        // Begin TT#219 - MD - DOConnell - Spread Average not getting expected results
		// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
        //private List<int> _allocHdrKeyList;
		private SelectedHeaderList _assortmentSelectedHdrList;
		// END TT#371-MD - stodd -  Velocity Interactive on Assortment
        // End TT#219 - MD - DOConnell - Spread Average not getting expected results
		private bool _useAssortment = false;		// TT#698-MD - Stodd - add ability for workflows to be run against assortments.
        //private MasterProfileChangeEvent _masterProfileChangeEvent; // TT#488 - MD - JEllis - Group Allocation  // TT#935 - MD - Jellis - Group Allocation Infrastructur build wrong
		private eActionOrigin _ActionOrigin = eActionOrigin.Unknown;	// TT#488-MD - Stodd - Group Allocation

        private Dictionary<int, AllocationProfileList> _masterSubordinates = new Dictionary<int, AllocationProfileList>();  // TT#1966-MD - JSmith - DC Fulfillment
        private Dictionary<int,long> _instanceTracker = new Dictionary<int, long>(); 

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of Transaction under the given SessionAddressBlock.
		/// </summary>
		/// <param name="aSAB">
		/// The SessionAddressBlock that owns this Transaction.
		/// </param>

		public ApplicationSessionTransaction(SessionAddressBlock aSAB)
			: base(aSAB)
		{
			Initialize();
		}

        // Begin TT#1031 - JSmith - Serializaion error in audit
        public ApplicationSessionTransaction(SessionAddressBlock aSAB, Session aSession)
            : base(aSAB)
        {
            _session = aSession;
            _audit = aSession.Audit;
            Initialize();
        }
        // End TT#1031

		public ApplicationSessionTransaction(SessionAddressBlock aSAB, int aLeaseTimeInSeconds)
			: base(aSAB, aLeaseTimeInSeconds)
		{
			Initialize();
		}

		public void Initialize()
		{
			_profileHash = new System.Collections.Hashtable();
			_profileHashLastProfileList = null;

			_profileListGroupHash = new System.Collections.Hashtable();
			_profileXRefHash = new System.Collections.Hashtable();

			_versionProtectedHash = new System.Collections.Hashtable();

			_subtotalKeyHash = new System.Collections.Hashtable();
			_subtotalKeyHashLastKey = null;
			_subtotalKeyHashLastValue = 0;

			_subtotalPacks = null;
			_subtotalPacksLastPack = new PackHdr(null);
			_subtotalPacksLastPackArray = null;

			_storeRID_IndexXref = null;
			//_storeRIDLastRIDKey = Include.NoRID; // MID Track 4341 Performance Issues
			//_storeRIDLastRIDValue = new Index_RID(0, Include.UndefinedStoreRID); // MID Track 4341 Performance Issues
			_storeRID_ToCharArrayHash = new Hashtable(); // MID Track 4341 Performance Issues
			_OTS_IT_HnRIDHash = new Hashtable(); // MID Track 4341 Performance Issues

			_reserveStoreIndexRID = new Index_RID(0, Include.UndefinedStoreRID);
			_grandTotalName = null;
			_grandTotalProfile = null;
			_inStoreReceiptDays = null;
			//			_inTransitFlashBack = Include.UndefinedDate;
			_intransitRdr = null;
			_onHandRdr = null;
			// BEGIN MID Track #2230   Display Current Week-to-Day Sales on Sku Review
			_dailySalesRdr = null;
			// END MID Track #2230   Display Current Week-to-Day Sales on Sku Review
			_allocationCubeGroup = null;
			_allocationBasisCubeGroup = null;
			_allocationCriteria = null;
			_velocityCriteria = null;
            _buildPacksCriteria = null;  // TT#370 Build Packs Enhancement
            _lastBuildItemIndicators = new BuildItemIndicators(Include.NoRID); // TT#488 - MD - Jellis - Group Allocation
            _buildItemIndicators = new Dictionary<int, BuildItemIndicators>(); // TT#488 - MD - Jellis - Group Allocation
            _buildItemProcessOrder = new Dictionary<int, Dictionary<int, AllocationProfile[]>>(); // TT#488 - MD - Jellis - Group Allocation
			_nodeDataHash = null;
			_nodeDataHashLastKey = 0;
			_nodeDataHashLastValue = null;

			_planLevelDataHash = null;
			_planLevelDataHashLastKey = 0;
			_planLevelDataHashLastValue = null;

			_salesEligibilityHashByNodeRID = new System.Collections.Hashtable();
			_stockEligibilityHashByNodeRID = new System.Collections.Hashtable();
			_priorityShippingHashByNodeRID = new System.Collections.Hashtable();
			_salesWkRangeEligibilityHashByNodeRID = new System.Collections.Hashtable();  // MID Track #2539 Grades not same in OTS and ALlocation
			_storeGradesHash = new System.Collections.Hashtable();
			_velocityGradesHash = new System.Collections.Hashtable();
			_sellThruPctsHash = new System.Collections.Hashtable();
			_storeStatusHash = new System.Collections.Hashtable();
			_storeCapacityHash = new System.Collections.Hashtable();
			_similarStoreHash = new System.Collections.Hashtable();
			_dailyPercentagesHashByNodeRID = new System.Collections.Hashtable();
//			SalesModifierHashByNodeRID = new System.Collections.Hashtable();
//			StockModifierHashByNodeRID = new System.Collections.Hashtable();
            // begin TT#41 - MD - JEllis - Size Inventory Min Max
            //_sizeCurve = new System.Collections.Hashtable(); // MID Track #2937 Size Onhand Incorrect
            _lastHeaderSizeNeedMethodKey = null;
            _sizeNeedMethodCache = new Dictionary<HeaderSizeNeedMethodKey, SizeNeedMethod>();
            // end TT#41 - MD - JEllis - Size Inventory Min Max
			_tempSizeNeedMethodRID = 0; // MID Track #2937 Size OnHand Incorrect

			//_OTS_HorizonKey = 0;                                       // MID Change j.ellis add need methods for OTS // MID Track 4309 OTS basis onhand not summed
			_OTS_HorizonHash = new Hashtable();                        // MID Track 4309 OTS Basis onhand not summed
			_OTS_HorizonHashLastKey = 0;                               // MID Track 4309 OTS Basis onhand not summed
			//_OTS_NeedHash = new System.Collections.Hashtable();        // MID Change j.ellis add need methods for OTS // MID Track 4309 OTS basis onhand not summed
			_OTS_NeedHash = null;                                      // MID Track 4309 OTS Basis onhand not summed
			_OTS_NeedHashLastKey = Include.NoRID;                      // MID Change j.ellis add need methods for OTS
			_OTS_VerHash = null;                                       // MID Change j.ellis add need methods for OTS
			_OTS_VerHashLastKey = Include.NoRID;                       // MID Change j.ellis add need methods for OTS
			_OTS_StoreHash = null;                                     // MID Change j.ellis add need methods for OTS
			_OTS_StoreHashLastKey = Include.NoRID;                     // MID Change j.ellis add need methods for OTS
			_OTS_LastStoreValues = null;                               // MID Change j.ellis add need methods for OTS
             
			_mainHierarchyData = null;                                 // MID Track 4020 plan level not reset on cancel
			_colorHierarchySequence = -1;                              // MID Track 4020 plan level not reset on cancel

			//			HierarchySessionTransaction = SAB.HierarchyServerSession.CreateTransaction();
			_showNeedAnalysisCriteria = false;
			_determineShowNeedAnalysis = true;
			_needAnalysisFrom1stHeader = false;
			_showNeedAnalysisCriteria = false;
			_determineShowNeedAnalysis = true;
			_currStoreGroupProfile = null;
			_currComputationMode = string.Empty;
			_storeStockPlanHash = new Hashtable();
			_storeSalesPlanHash = new Hashtable();
			_headerEnqMessage = null;
            //_headerEnq = null;
			_allocationStoreFilterRID = int.MinValue;
			_quickFilterChanged = true;
			_selectedAllocationStores = null;
			_grandTotalName = MIDText.GetTextOnly((int) eHeaderNode.GrandTotal);
            _assortmentProfile = null; //  TT#1154- MD - Jellis - Group Allocation Infinite  Loop When Going to SIze Analysis
			this._pctToTotalDecimalPrecision = (long)Math.Pow(10,Include.DecimalPositionsInStoreSizePctToColor);
			_velocityStorePctSellThruIdx_Chain = null;
			_velocityStorePctSellThruIdx_Set = null;
			_rebuildAllocationStores = false;       // MID Track 4341 Performance Issues
			_rebuildAllocationStoresActive = false;
            _storeRID_CharArray_List = new ArrayList(); // MID Track 4341 Performance Issues
			_parentHnHash = new Hashtable();         // MID track 4312 Size Intransit not relieved at style total level
            _parentHnHashLastParent = null;          // MID Track 4312 Size Intransit not relieved at style total level
		    _IKT_Hash = new Hashtable();             // MID Track 4312 Size Intransit not relieved at style total
		    _IKT_HashLast_iktHash = new Hashtable(); // MID Track 4312 Size Intransit not relieved at style total
		    _IKT_HashLastKey = Include.NoRID;        // MID Track 4312 Size Intransit not relieved at style total
			// begin MID Track 4362 Alternate Intransit Performance
			_intransitReadNodeHash = new Hashtable();
		    _lastIntransitReadPllvHash = null;
		    _lastIntransitReadNodeArray = null;
		    _lastIntransitReadNodeHashKey = Include.NoRID;
		    _lastIntransitReadNodeHashLevel = eHierarchyLevelMasterType.Undefined;
			// end MID Track 4362 Alternate Intransit Performance
            // begin TT#488 - MD - Jellis - Group Allocation (fields not used)
            //// BEGIN TT#1401 - stodd - add resevation stores (IMO)
            //_IMOReadNodeHash = new Hashtable();
            //_lastIMOReadPllvHash = null;
            //_lastIMOReadNodeArray = null;
            //_lastIMOReadNodeHashKey = Include.NoRID; ;
            //_lastIMOReadNodeHashLevel = eHierarchyLevelMasterType.Undefined; ;
            //// END TT#1401 - stodd - add resevation stores (IMO)
            // end TT#488 - MD - Jellis - Group Allocation (fields not used)
			_storeRID_CharArray_List = new ArrayList(); // MID Track 4341 Performance Issues
			_parentHnHash = new Hashtable();         // MID track 4312 Size Intransit not relieved at style total level
			_parentHnHashLastParent = null;          // MID Track 4312 Size Intransit not relieved at style total level
			_IKT_Hash = new Hashtable();             // MID Track 4312 Size Intransit not relieved at style total
			_IKT_HashLast_iktHash = new Hashtable(); // MID Track 4312 Size Intransit not relieved at style total
			_IKT_HashLastKey = Include.NoRID;        // MID Track 4312 Size Intransit not relieved at style total
			// begin MID Track 4372 Generic Size Constraints
            _headerGenericNameCharacteristics = new Hashtable();
            _lastGenericNameCharacteristicHeaderKey = int.MinValue;
			_genericHeaderCharacteristicHash = new Hashtable();
			_lastGenericHeaderCharacteristicKey = long.MinValue;
			_lastGenericMerchandiseHierarchyNodeProfile = new HierarchyNodeProfile(Include.NoRID);
			_lastGenericColorCodeRID = int.MinValue;
			_lastGenericSizeName = string.Empty;
			_productLevelDelimiter = SAB.HierarchyServerSession.GlobalOptions.ProductLevelDelimiter;
			_headerCharacteristicProfileList = null;
			_lastHeaderCharacteristicGroupRID = Include.NoRID;
			_lastHeaderCharacteristicProfile = null;
            // Begin TT#1031 - JSmith - Serializaion error in audit
            //_audit = this.SAB.ApplicationServerSession.Audit;
            if (_audit == null)
            {
                _audit = this.SAB.ApplicationServerSession.Audit;
            }
            // Begin TT#1031 - JSmith - Serializaion error in audit
			// end MID Track 4372 Generic Size Constraints
			// begin MID Track 4448 AnF Audit Enhancement
			_auditData = null;
			_processRID = Include.NoRID;
			_userRID = Include.NoRID;
            _userId = string.Empty; // TT#1185 - Verify ENQ before Update (part 2) - not related
            _clientThreadID = SAB.ClientServerSession.ThreadID; // TT#1185 - Verify ENQ before Update (part 2) - App Server Session houses ENQ
			// end MID Track 4448 AnF Audit Enhancement
			// begin MID Track 4426 Need Section goes to zero
			_buildStyleWaferColumns = new ArrayList();
			_buildSizeWaferColumns = new ArrayList();
			// end MID Track 4426 Need Section goes to zero
			_storeGroupDict = new Dictionary<long,ProfileList>(); // MID Track 5820 AnF Defect 1819 - Unhandled Execption After Store Activated

            _sizeViewSizesAL = new ArrayList(); ; // TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
		    _allocationWaferChangeRequest = new ArrayList();  // TT#59 Implement Temp Locks

            _assmntViewSelBypass = false; //TT#1962 - DOConnell - When selecting a single assortment from the assrt workspace, review screen is using user selection basis.

			// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
            //_allocHdrKeyList = new List<int>(); //TT#219 - MD - DOConnell - Spread Average not getting expected results
			_assortmentSelectedHdrList = new SelectedHeaderList(eProfileType.SelectedHeader);
			// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
            _ruleBasedAllocation = null; // TT#586 - MD - Jellis - Assortment add 2 dimensional spread
            //_masterProfileChangeEvent = new MasterProfileChangeEvent(); // TT#488 - MD - Jellis - Group Allocation    // TT#935 - MD - Jellis - Group Allocation Infrastructur build wrong

		}

        // Begin TT#1759-MD - JSmith - GA- Velocity-> Velocity Store Detail-> Allocate and Velocity Matrix buttons gray out when selecting attribute in Matrix
        public void ReInitializeForAssortGroup()
        {

            _allocationCubeGroup = null;
            _allocationBasisCubeGroup = null;
            _velocityCriteria = null;
            _buildPacksCriteria = null; 
            
            _buildStyleWaferColumns = new ArrayList();
            _buildSizeWaferColumns = new ArrayList();
            _storeGroupDict = new Dictionary<long, ProfileList>(); 
        }
        // End TT#1759-MD - JSmith - GA- Velocity-> Velocity Store Detail-> Allocate and Velocity Matrix buttons gray out when selecting attribute in Matrix

        // Begin TT#1731 - JSmith - Forecast Job Errors in MID 4.0 - Out of Memory
        public void ClearSalesModifierCache()
        {
            if (_salesModifierHashByNodeRID != null &&
                _salesModifierHashByNodeRID.Count > 0)
            {
                _salesModifierHashByNodeRID.Clear();
                _salesModifierHashByNodeRID = null;
                _salesModifierHashByNodeRID = new Hashtable();
            }
        }

        public void ClearStockModifierCache()
        {
            if (_stockModifierHashByNodeRID != null &&
                _stockModifierHashByNodeRID.Count > 0)
            {
                _stockModifierHashByNodeRID.Clear();
                _stockModifierHashByNodeRID = null;
                _stockModifierHashByNodeRID = new Hashtable();
            }
        }

        public void ClearFWOSModifierCache()
        {
            if (_FWOSModifierHashByNodeRID != null &&
                _FWOSModifierHashByNodeRID.Count > 0)
            {
                _FWOSModifierHashByNodeRID.Clear();
                _FWOSModifierHashByNodeRID = null;
                _FWOSModifierHashByNodeRID = new Hashtable();
            }
        }

        public void ClearPMPlusSalesCache()
        {
            if (_PMPlusSalesHashByNodeRID != null &&
                _PMPlusSalesHashByNodeRID.Count > 0)
            {
                _PMPlusSalesHashByNodeRID.Clear();
                _PMPlusSalesHashByNodeRID = null;
                _PMPlusSalesHashByNodeRID = new Hashtable();
            }
        }

        public void ClearSalesEligibilityCache()
        {
            if (_salesEligibilityHashByNodeRID != null &&
                _salesEligibilityHashByNodeRID.Count > 0)
            {
                _salesEligibilityHashByNodeRID.Clear();
                _salesEligibilityHashByNodeRID = null;
                _salesEligibilityHashByNodeRID = new Hashtable();
            }
        }

        public void ClearStockEligibilityCache()
        {
            if (_stockEligibilityHashByNodeRID != null &&
                _stockEligibilityHashByNodeRID.Count > 0)
            {
                _stockEligibilityHashByNodeRID.Clear();
                _stockEligibilityHashByNodeRID = null;
                _stockEligibilityHashByNodeRID = new Hashtable();
            }
        }

        public void ClearPriorityShippingCache()
        {
            if (_priorityShippingHashByNodeRID != null &&
                _priorityShippingHashByNodeRID.Count > 0)
            {
                _priorityShippingHashByNodeRID.Clear();
                _priorityShippingHashByNodeRID = null;
                _priorityShippingHashByNodeRID = new Hashtable();
            }
        }
        
        public void ClearSalesWkRangeEligibilityCache()
        {
            if (_salesWkRangeEligibilityHashByNodeRID != null &&
                _salesWkRangeEligibilityHashByNodeRID.Count > 0)
            {
                _salesWkRangeEligibilityHashByNodeRID.Clear();
                _salesWkRangeEligibilityHashByNodeRID = null;
                _salesWkRangeEligibilityHashByNodeRID = new Hashtable();
            }
        }

        public void ClearDailyPercentagesCache()
        {
            if (_dailyPercentagesHashByNodeRID != null &&
                _dailyPercentagesHashByNodeRID.Count > 0)
            {
                _dailyPercentagesHashByNodeRID.Clear();
                _dailyPercentagesHashByNodeRID = null;
                _dailyPercentagesHashByNodeRID = new Hashtable();
            }
        }

        public void ClearStoreGradesCache()
        {
            if (_storeGradesHash != null &&
                _storeGradesHash.Count > 0)
            {
                _storeGradesHash.Clear();
                _storeGradesHash = null;
                _storeGradesHash = new Hashtable();
            }
        }

        public void ClearVelocityGradesCache()
        {
            if (_velocityGradesHash != null &&
                _velocityGradesHash.Count > 0)
            {
                _velocityGradesHash.Clear();
                _velocityGradesHash = null;
                _velocityGradesHash = new Hashtable();
            }
        }

        public void ClearSellThruPctsCache()
        {
            if (_sellThruPctsHash != null &&
                _sellThruPctsHash.Count > 0)
            {
                _sellThruPctsHash.Clear();
                _sellThruPctsHash = null;
                _sellThruPctsHash = new Hashtable();
            }
        }

        public void ClearSimilarStoreCache()
        {
            if (_similarStoreHash != null &&
                _similarStoreHash.Count > 0)
            {
                _similarStoreHash.Clear();
                _similarStoreHash = null;
                _similarStoreHash = new Hashtable();
            }
        }

        public void ClearStoreCapacityCache()
        {
            if (_storeCapacityHash != null &&
                _storeCapacityHash.Count > 0)
            {
                _storeCapacityHash.Clear();
                _storeCapacityHash = null;
                _storeCapacityHash = new Hashtable();
            }
        }

        public void ClearStoreStatusCache()
        {
            if (_storeStatusHash != null &&
                _storeStatusHash.Count > 0)
            {
                _storeStatusHash.Clear();
                _storeStatusHash = null;
                _storeStatusHash = new Hashtable();
            }
        }
        // End TT#1731

        // begin TT#1029 - MD - Jellis - Group Allocation Velocity - All units in VSW
        public void ClearVSWCache()
        {
            _buildItemProcessOrder.Clear();
            _buildItemIndicators.Clear();
            _storeVswReverseOnhand = null;
            _lastBuildItemIndicators = new BuildItemIndicators(Include.NoRID);
        }
        // end TT#1029 - MD - Jellis - Group Allocaiton Velocity - All units in VSW

		override protected void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
                    //DequeueHeaders(); // TT#1185 - JEllis - Verify ENQ before Update                    // MID TT#29 - JEllis - Header Locks not being cleared
                    SAB.ApplicationServerSession.DequeueHeaders(_clientThreadID, TransactionID, UserRID); // MID TT#29 - JEllis - Header Locks not being cleared
                    _headerEnqueue = null;                                                                // MID TT#29 - JEllis - Header Locks not being cleared
					foreach (ProfileListGroup plg in _profileListGroupHash.Values)
					{
						plg.Dispose();
					}

					if (_forecastCubeGroup != null)
					{
						_forecastCubeGroup.Dispose();
					}
				
					if (_allocationCubeGroup != null)
					{
						_allocationCubeGroup.Dispose();
					}
				
					if (_allocationBasisCubeGroup != null)
					{
						_allocationBasisCubeGroup.Dispose();
					}
				
					if (_hierarchySessionTransaction != null)
					{
						_hierarchySessionTransaction.Dispose();
					}
                    // begin TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4
					// Begin TT#1757-MD - GA Velocity STock selected to grade stores-> select specific attribute set and apply minimum rule-> get System Null Ref error message- ANN initial process 
                    //_storeVswReverseOnhand = null;
                    //_lastBuildItemIndicators = new BuildItemIndicators(Include.NoRID); // TT#488- MD - Jellis - Group Allocation
                    //_buildItemIndicators = null; // TT#488 - MD - Jellis - Group Allocation
                    //_buildItemProcessOrder = null; // TT#488 - MD - Jellis - Group Allocation
                    // end TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4
                    ClearVSWCache();
					// End TT#1757-MD - GA Velocity STock selected to grade stores-> select specific attribute set and apply minimum rule-> get System Null Ref error message- ANN initial process 
                    _ruleBasedAllocation = null; // TT#586 - MD - Jellis - Assortment add 2 dimensional spread
				}

				base.Dispose(disposing);
			}
			catch (Exception)
			{
			}
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the HierarchySessionTransaction object.
		/// </summary>

		public HierarchySessionTransaction HierarchySessionTransaction
		{
			get
			{
				if (_hierarchySessionTransaction == null)
				{
					_hierarchySessionTransaction = SAB.HierarchyServerSession.CreateTransaction();
				}
				return _hierarchySessionTransaction;
			}
		}

		/// <summary>
		/// Gets the Computations object from the ApplicationServerSession.
		/// </summary>

		public IPlanComputations PlanComputations
		{
			get
			{
				if (_planComputations == null)
				{
					if (_currComputationMode == string.Empty)
					{
						_planComputations = SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations();
					}
					else
					{
						_planComputations = SAB.ApplicationServerSession.ComputationsCollection.GetComputations(_currComputationMode);
					}

					if (_planComputations == null)
					{
						string msg = MIDText.GetText(eMIDTextCode.msg_pl_InvalidComputationsRequested);
						msg = msg.Replace("{0}",_currComputationMode);
						throw new MIDException(eErrorLevel.severe,
							(int)eMIDTextCode.msg_pl_InvalidComputationsRequested, msg);
					}
				}

				return _planComputations;
			}
		}

		/// <summary>
		/// Gets or sets the current Computations Mode.
		/// </summary>

		public string CurrentComputationMode
		{
			get
			{
				return _currComputationMode;
			}
			set
			{
				_currComputationMode = value;
				_planComputations = null;
			}
		}

		/// <summary>
		/// Gets or sets the StoreGroupProfile that describes the current Store Group.
		/// </summary>

		public StoreGroupProfile CurrentStoreGroupProfile
		{
			get
			{
				return _currStoreGroupProfile;
			}
			set
			{
				_profileListGroupHash.Remove(eProfileType.StoreGroupLevel);
				_profileXRefHash.Remove(new ProfileXRef(eProfileType.StoreGroupLevel, eProfileType.Store));
				_currStoreGroupProfile = value;
                //_masterProfileChangeEvent.ChangeMasterProfile(this, eProfileType.StoreGroupLevel, eChangeType.delete); // TT#488 - MD - Jellis - Group Allocation  // TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
			}
		}

		public Index_RID ReserveStore
		{
			get 
			{
				if (_storeRID_IndexXref == null)
				{
					BuildStoreRIDIndexXref();
				}
				return this._reserveStoreIndexRID;
			}
		}

        // begin TT#586 Velocity variables not calculated correctly
        ////BEGIN #153 - Velocity Matrix Variables - apicchetti
        ////array list of needed datatables for matrix totals
        //ArrayList _storesellthru = new ArrayList();
        //public ArrayList StoresSellThru
        //{
        //    set
        //    {
        //        _storesellthru = value;
        //    }
        //    get
        //    {
        //        return _storesellthru;
        //    }
        //}
        ////END #153 - Velocity Matrix Variables - apicchetti
        // end TT#586 Velocity Variables not calculated correctly

		public bool RebuildAllocationStores
		{
			get
			{
				return _rebuildAllocationStores;
			}
			set
			{
				_rebuildAllocationStores = value;
			}
		}
		/// <summary>
		/// Gets the grand total name for allocation headers.
		/// </summary>
		public string GrandTotalName
		{
			get
			{
				return _grandTotalName;
			}
		}

        // begin TT#1154 - MD - Jellis - Group Allocation Infinite  Loop When Going to SIze Analysis
        public AssortmentProfile AssortmentProfile
        {
            get
            {
                return _assortmentProfile;
            }
            set
            {
                _assortmentProfile = value;
            }

        }
        // end  TT#1154 - MD - Jellis - Group Allocation Infinite  Loop When Going to SIze Analysis

        // begin TT#586 - MD - Jellis - Assortment add 2 dimensional spread
        public RuleBasedAllocation RuleBasedAllocation
        {
            get
            {
                if (_ruleBasedAllocation == null)
                {
                    _ruleBasedAllocation = new RuleBasedAllocation(this);
                }
                return _ruleBasedAllocation;
            }
        }
        // end TT#586 - MD - Jellis - Assortment add 2 dimensional spread

		/// <summary>
		/// Gets the summary level allocation action status for headers associated with this transaction. 
		/// </summary>
		public eAllocationActionStatus AllocationActionAllHeaderStatus
		{
            // begin TT#241 - MD - JEllis - Header Enqueue Process
            get
            {
                if (this._allocationActionStatusDict == null ||
                    this._allocationActionStatusDict.Count == 0)
                {
                    return eAllocationActionStatus.NoActionPerformed;
                }
                // Begin TT#494-MD - JSmith - The size methods it would give an Action Failed Message.  It says completed successfully.
                //eAllocationActionStatus actionStatus = eAllocationActionStatus.ActionCompletedSuccessfully;
                eAllocationActionStatus actionStatus = eAllocationActionStatus.NoActionPerformed;
				// BEGIN TT#488-MD - STodd - Group Allocation 
				//if (this.AssortmentViewLoadedFrom == null)
				//{
				//    if (_needHeaders &&
				//        this._allocationActionStatusDict.Count != this.GetAllocationProfileList().Count)
				//    {
				//        actionStatus = eAllocationActionStatus.ActionFailed;
				//    }
				////End TT#362 - MD - DOConnell - Running Balance to Proportional against a header attached to a placeholder does not reduce the Placeholder by the new allocated units
				//}
				// END TT#488-MD - STodd - Group Allocation 
                foreach (eAllocationActionStatus status in _allocationActionStatusDict.Values)
                {
                    switch (actionStatus)
                    {
                        case (eAllocationActionStatus.HeaderEnqueueFailed):
                            {
                                break;
                            }
                        case (eAllocationActionStatus.NoHeaderResourceLocks):
                            {
                                if (status == eAllocationActionStatus.HeaderEnqueueFailed)
                                {
                                    actionStatus = status;
                                }
                                break;
                            }
                        case (eAllocationActionStatus.ActionFailed):
                            {
                                if (status == eAllocationActionStatus.HeaderEnqueueFailed
                                    || status == eAllocationActionStatus.NoHeaderResourceLocks)
                                {
                                    actionStatus = status;
                                }
                                break;
                            }
                        case (eAllocationActionStatus.VelocityBasisError):
                            {
                                if (status == eAllocationActionStatus.HeaderEnqueueFailed
                                    || status == eAllocationActionStatus.NoHeaderResourceLocks
                                    || status == eAllocationActionStatus.ActionFailed)
                                {
                                    actionStatus = status;
                                }
                                break;
                            }
                        // begin TT#2671 - Jellis - Release Action Fails yet headers released
                        case (eAllocationActionStatus.NotAllLinkedHeadersRlseApproved):
                            {
                                    if (status == eAllocationActionStatus.HeaderEnqueueFailed
                                    || status == eAllocationActionStatus.NoHeaderResourceLocks
                                    || status == eAllocationActionStatus.VelocityBasisError
                                    || status == eAllocationActionStatus.ActionFailed)
                                {
                                    actionStatus = status;
                                }
                                break;
                            }
                        // end TT#2671 - Jellis - Release Action Fails yet headers released
                        // Begin TT#494-MD - JSmith - The size methods it would give an Action Failed Message.  It says completed successfully.
                        //case (eAllocationActionStatus.ActionCompletedSuccessfully):
                        //case (eAllocationActionStatus.NoActionPerformed):
                        //    {
                        //        if (status == eAllocationActionStatus.HeaderEnqueueFailed
                        //            || status == eAllocationActionStatus.NoHeaderResourceLocks
                        //            || status == eAllocationActionStatus.ActionFailed
                        //            || status == eAllocationActionStatus.VelocityBasisError              // TT#2671 - Jellis - Release Action Fails yet headers released
                        //            || status == eAllocationActionStatus.NotAllLinkedHeadersRlseApproved)  // TT#2671 - Jellis - Release Action Fails yet headers released
                        //        {
                        //            actionStatus = status;
                        //        }
                        //        break;
                        //    }
                        case (eAllocationActionStatus.NoActionPerformed):
                            {
                                if (status == eAllocationActionStatus.HeaderEnqueueFailed
                                    || status == eAllocationActionStatus.NoHeaderResourceLocks
                                    || status == eAllocationActionStatus.ActionFailed
                                    || status == eAllocationActionStatus.ActionCompletedSuccessfully   
                                    || status == eAllocationActionStatus.VelocityBasisError              // TT#2671 - Jellis - Release Action Fails yet headers released
                                    || status == eAllocationActionStatus.NotAllLinkedHeadersRlseApproved)  // TT#2671 - Jellis - Release Action Fails yet headers released
                                {
                                    actionStatus = status;
                                }
                                break;
                            }
                        case (eAllocationActionStatus.ActionCompletedSuccessfully):
                            {
                                if (status == eAllocationActionStatus.HeaderEnqueueFailed
                                    || status == eAllocationActionStatus.NoHeaderResourceLocks
                                    || status == eAllocationActionStatus.ActionFailed
                                    || status == eAllocationActionStatus.VelocityBasisError              // TT#2671 - Jellis - Release Action Fails yet headers released
                                    || status == eAllocationActionStatus.NotAllLinkedHeadersRlseApproved)  // TT#2671 - Jellis - Release Action Fails yet headers released
                                {
                                    actionStatus = status;
                                }
                                break;
                            }
                        // End TT#494-MD - JSmith - The size methods it would give an Action Failed Message.  It says completed successfully.
                        default:
                            {
                                throw new Exception(GetType().Name + "__AllocationActionAllHeaderStatus: Unknown eAllocationActionStatus: " + actionStatus.ToString());
                            }
                    }
                    if (actionStatus == eAllocationActionStatus.HeaderEnqueueFailed)
                    {
                        break;
                    }
                }
                return actionStatus;
            }
            //get
            //{
            //    if (this._allocationActionStatusHash == null ||
            //        this._allocationActionStatusHash.Count == 0)
            //    {
            //        return eAllocationActionStatus.NoActionPerformed;
            //    }
            //    // Begin TT#155 - JSmith - Size Curve Method
            //    //if (this._allocationActionStatusHash.Count != this.GetAllocationProfileList().Count)
            //    if (_needHeaders &&
            //        this._allocationActionStatusHash.Count != this.GetAllocationProfileList().Count)
            //    // End TT#155
            //    {
            //        return eAllocationActionStatus.ActionFailed;
            //    }
            //    foreach (eAllocationActionStatus status in _allocationActionStatusHash.Values)
            //    {
            //        if (status != eAllocationActionStatus.ActionCompletedSuccessfully)
            //        {
            //            return status;
            //        }
            //    }
            //    return eAllocationActionStatus.ActionCompletedSuccessfully;
            //}
            // end TT#241 - MD - JEllis - Header Enqueue Process
		}

		public eOTSPlanActionStatus OTSPlanActionStatus 
		{
			get { return _OTSPlanActionStatus ; }
			set { _OTSPlanActionStatus = value; }
		}

        // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
        // moved this event to and renamed it in ProfileList
        //// begin TT#488 - MD - Jellis - Group Allocation
        ///// <summary>
        ///// Gets MasterProfileChangeEvent
        ///// </summary>
        //public MasterProfileChangeEvent MasterProfileChangeEvent
        //{
        //    get
        //    {
        //        return _masterProfileChangeEvent;
        //    }
        //}
        //// end TT#488 - MD - Jellis - Group Allocation
        // end TT#935 - MD - Jellis - Group Allocation Infrastructure Built wrong

		/// <summary>
		/// Gets the header enqueue message from the last enqueue.
		/// </summary>
		public string HeaderEnqueueMessage
		{
			get
			{
				return _headerEnqMessage;
			}
		}
		/// <summary>
		/// Gets or sets the Allocation ViewIsSequential flag value
		/// </summary>
		/// <remarks>True:  Diplay is in sequential format; False: Display is in matrix format (for Size View: the columns = primary size and the rows = secondary size)</remarks>
		public bool AllocationViewIsSequential
		{
			get
			{
				return this._allocationCriteria.ViewIsSequential;
			}
			set
			{
				this._allocationCriteria.ViewIsSequential = value;
			}
		}

		public bool SizeViewSequentialSizeDisplay
		{
			get
			{
				return AllocationViewIsSequential;
			}
			set
			{
				AllocationViewIsSequential = value;
			}
		}
		/// <summary>
		/// Gets or sets the size view group by for variable or size.
		/// </summary>
		public int AllocationSecondaryGroupBy
		{
			get
			{
				return this._allocationCriteria.AllocationSecondaryGroupBy;
			}
			set
			{
				this._allocationCriteria.AllocationSecondaryGroupBy = value;
			}
		}

		/// <summary>
		/// Gets or sets the size view group by for variable or size.
		/// </summary>
		public eAllocationSizeView2ndGroupBy SizeViewVariableOrSizeGroupBy
		{
			get
			{
				return (eAllocationSizeView2ndGroupBy)AllocationSecondaryGroupBy;
			}
			set
			{
				AllocationSecondaryGroupBy = (int)value;
			}
		}

		/// <summary>
		/// Gets the size view row variable count
		/// </summary>
		/// <remarks>NOTE:  this value is only valid AFTER the size view wafers have been built</remarks>
		public int SizeViewRowVariableCount
		{
			get
			{
				return this._allocationCriteria.SizeViewRowVariableCount;
			}
		}
		
		// begin MID Track 3611 Quick Filter not working in Size Review
		public DataTable QuickFilterSizesDropDown
		{
			get
			{
				return this._allocationCriteria.QuickFilterSizeDropDown;
			}
		}
		// end MID Track 3611 Quick Filter not working in Size Review

		/// <summary>
		/// Gets the forecastingOverrideList which is ForecastingOverride array
		/// </summary>
		public ArrayList ForecastingOverrideList
		{
			get
			{
				return _forecastingOverrideList;
			}
		}

		/// <summary>
		/// Gets the ForecastingBalanceOverrideList which is ForecastingBalanceOverride array
		/// </summary>
		public ArrayList ForecastingBalanceOverrideList
		{
			get
			{
				return _forecastingBalanceOverrideList;
			}
		}

		public bool SizeViewIsValid
		{
			get
			{
				
				if (this._sizeCodeByPrimarySecondary.Count > 0)
				{
					return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Gets a divisor that is used to convert a "long" integer value into a double with a default number of decimal positions.
		/// </summary>
		/// <remarks>Number of actual decimal positions is given by Include.Include.DecimalPositionsInStoreSizePctToColor</remarks>
		public long StoreSizePctToTotalDecimalPrecision
		{
			get
			{
				return this._pctToTotalDecimalPrecision;
			}
		}

		/// <summary>
		/// Gets or sets LoadSelectedHeaders which tells if the transaction needs the headers loaded or not.
		/// </summary>
		public bool NeedHeaders
		{
			get
			{
				return _needHeaders;
			}
			set
			{
				_needHeaders = value;
			}
		}

        // begin TT#1185 - JEllis - Verify ENQ before Update - (part 2)  -  remove duplicate processes
        ///// <summary>
        ///// Key of user running transaction.
        ///// </summary>
        //public int UserRid
        //{
        //    get { return _userRid; }
        //    set
        //    {
        //        _userRid = value;
        //        SecurityAdmin secAdmin = new SecurityAdmin();
        //        _userId = secAdmin.GetUserName(_userRid);
        //    }
        //}
        ///// <summary>
        ///// Id of user running transaction. Set by setting UserRid.
        ///// </summary>
        //public string UserID
        //{
        //    get { return _userId; }
        //}

        /// <summary>
        /// Gets key of user running transaction
        /// </summary>
        public int UserRID
        {
            get 
            {
                if (_userRID == Include.NoRID)
                {
                    _userRID = SAB.ClientServerSession.UserRID;
                }
                return _userRID;
            }
        }
		/// <summary>
		/// Id of user running transaction. 
		/// </summary>
		public string UserID
		{
            get 
            {
                if (_userId == string.Empty)
                {
                    //Begin TT#827-MD -jsobek -Allocation Reviews Performance
                    //SecurityAdmin secAdmin = new SecurityAdmin();
                    //_userId = secAdmin.GetUserName(UserRID);
                    _userId = UserNameStorage.GetUserName(UserRID);
                    //End TT#827-MD -jsobek -Allocation Reviews Performance
                }
                return _userId;
            } 
		}
        // end TT#1185 - JEllis - Verify ENQ before Update - (part 2)

		/// <summary>
		/// Gets or sets the columns to build in the wafer.
		/// </summary>
		public ArrayList BuildWaferColumns
		{
			get
			{
				// begin MID Track 4426 Need Section goes to zero
				//return _buildWaferColumns;
				if (this.AllocationViewType == eAllocationSelectionViewType.Size)
				{
					return _buildSizeWaferColumns;
				}
				else
				{
					return _buildStyleWaferColumns; // assume this is the default
				}
				// end MID Track 4426 Need Section goes to zero 
			}
//			set
//			{
//				_buildWaferColumns = value;
//			}
		}

		/// <summary>
		/// Gets the list of AllocationProfiles for the linked headers.
		/// </summary>
		public AllocationProfileList LinkedHeaderList
		{
			get
			{
				if (_linkedHeaderList == null)
				{
					_linkedHeaderList = new AllocationProfileList(eProfileType.Allocation);
				}
				return _linkedHeaderList;
			}
		}

		// BEGIN MID Track #4370 - John Smith - FWOS Models
		private Hashtable SalesModifierHashByNodeRID 
		{
			get 
			{ 
				if (_salesModifierHashByNodeRID == null)
				{
					_salesModifierHashByNodeRID = new System.Collections.Hashtable();
				}
				return _salesModifierHashByNodeRID ; 
			}
//			set 
//			{ 
//				_salesModifierHashByNodeRID = value; 
//			}
		}

		private Hashtable StockModifierHashByNodeRID 
		{
			get 
			{ 
				if (_stockModifierHashByNodeRID == null)
				{
					_stockModifierHashByNodeRID = new System.Collections.Hashtable();
				}
				return _stockModifierHashByNodeRID ; 
			}
//			set 
//			{ 
//				_stockModifierHashByNodeRID = value; 
//			}
		}

		private Hashtable FWOSModifierHashByNodeRID 
		{
			get 
			{ 
				if (_FWOSModifierHashByNodeRID == null)
				{
					_FWOSModifierHashByNodeRID = new System.Collections.Hashtable();
				}
				return _FWOSModifierHashByNodeRID ; 
			}
			set 
			{ 
				_FWOSModifierHashByNodeRID = value; 
			}
		}
		// END MID Track #4370

		// BEGIN MID Track #4827 - John Smith - Presentation plus sales
		private Hashtable PMPlusSalesHashByNodeRID 
		{
			get 
			{ 
				if (_PMPlusSalesHashByNodeRID == null)
				{
					_PMPlusSalesHashByNodeRID = new System.Collections.Hashtable();
				}
				return _PMPlusSalesHashByNodeRID ; 
			}
			set 
			{ 
				_PMPlusSalesHashByNodeRID = value; 
			}
		}
		// END MID Track #4827

        // Begin TT#1705 - JSmith - Reset Header with Piggybacking
        /// <summary>
        /// Gets or sets ProcessingCustomReset which tells if a custom reset is already in progress.
        /// </summary>
        public bool ProcessingCustomReset
        {
            get
            {
                return _processingCustomReset;
            }
            set
            {
                _processingCustomReset = value;
            }
        }
        // End TT#1705

		// BEGIN TT#2225 - stodd - VSW ANF Enhancement (IMO)
		/// <summary>
		/// gets a pointer to the custom business routines
		/// </summary>
		public CustomBusinessRoutines BuisnessRoutines
		{
			get
			{
				if (_buisnessRoutines == null)
				{
					_buisnessRoutines = new CustomBusinessRoutines(SAB, this);
				}
				return _buisnessRoutines;
			}
		}
		// END TT#2225 - stodd - VSW ANF Enhancement (IMO)

        // Begin TT#1495-MD - RMatelic - ASST- Created a post asst- highlight 1st header open style review all the detail columns appear.  Close style review and reopen and all the detail columns are gone
        public bool DetermineShowNeedAnalysis
        {
            get
            {
                return _determineShowNeedAnalysis;
            }
            set
            {
                _determineShowNeedAnalysis = value;
            }
        }
        // End TT#1495-MD

		//========
		// METHODS
		//========

		//========================
		// ProfileList functions
		//========================

		/// <summary>
		/// This method will retrieve the current ProfileList stored in this session.  If the ProfileList has not yet been created, the
		/// values will be retrieved from other Sessions, if necessary.  If the information is not available in other sessions, an error will
		/// be thrown.
		/// </summary>
		/// <param name="aProfileType">
		/// The eProfileType of the ProfileList to retieve.
		/// </param>
		/// <returns>
		/// The ProfileList object for the given eProfileType.
		/// </returns>

		public ProfileList GetProfileList(eProfileType aProfileType)
		{
			ProfileList profileList;

			if (_profileHashLastProfileList == null
				|| _profileHashLastProfileList.ProfileType != aProfileType)
			{
				profileList = (ProfileList)_profileHash[aProfileType];

				if (profileList == null)
				{
					switch (aProfileType)
					{
						case eProfileType.Store:

							profileList = StoreMgmt.StoreProfiles_GetActiveStoresList(); //SAB.ApplicationServerSession.GetProfileList(aProfileType); //TT#1517-MD -jsobek -Store Service Optimization
							break;

						case eProfileType.StoreGroup:

							profileList = StoreMgmt.StoreGroup_GetList(); //SAB.ApplicationServerSession.GetProfileList(aProfileType); //TT#1517-MD -jsobek -Store Service Optimization
        				   	break;

                        case eProfileType.StoreGroupListView: //TT#1517-MD -jsobek -Store Service Optimization

                            profileList = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.All, false); //SAB.ApplicationServerSession.GetProfileList(aProfileType); //TT#1517-MD -jsobek -Store Service Optimization
                            break;

                        case eProfileType.Version: //TT#1517-MD -jsobek -Store Service Optimization

                            profileList = SAB.ApplicationServerSession.GetProfileListVersion(); //SAB.ClientServerSession.GetUserForecastVersions(); //SAB.ApplicationServerSession.GetProfileList(aProfileType); //TT#1517-MD -jsobek -Store Service Optimization
                            break;                   

						case eProfileType.TimeTotalVariable:

							profileList = PlanComputations.PlanTimeTotalVariables.TimeTotalVariableProfileList;
							break;

						case eProfileType.Variable:

							profileList = PlanComputations.PlanVariables.VariableProfileList;
							break;

						case eProfileType.QuantityVariable:
							profileList = PlanComputations.PlanQuantityVariables.QuantityVariableProfileList;
							break;

						case eProfileType.SelectedHeader:

							profileList = SAB.ClientServerSession.GetSelectedHeaderList();
							break;

						default:  //DateRange, Period,  Week, Basis, HierarchyNode are some that fall through here...they just return null
							profileList = SAB.ApplicationServerSession.GetProfileList(aProfileType);
                            //throw new Exception("Unknown Profile List Type:" + aProfileType.ToString());
                            break;

					}
					if (profileList != null)
					{
						_profileHash.Add(profileList.ProfileType, profileList);
					}
				}
				_profileHashLastProfileList = profileList;
			}

			return _profileHashLastProfileList;
		}


		/// <summary>
		/// Gets the list of select components from the Client Server Session
		/// </summary>
		/// <returns></returns>
		public ArrayList GetSelectedComponentList()
		{
			ArrayList componentList = SAB.ClientServerSession.GetSelectedComponentList();
			return componentList;
		}

		/// <summary>
		/// Retrieves the master ProfileList as requested by the given eProfileType.
		/// </summary>
		/// <param name="aProfileType">
		/// The eProfileType that identifies which ProfileList to retrieve.
		/// </param>
		/// <returns>
		/// The ProfileList requested.
		/// </returns>

		public ProfileListGroup GetProfileListGroup(eProfileType aProfileType)
		{
			ProfileList profileList;
			ProfileListGroup profileListGroup;

			profileListGroup = (ProfileListGroup)_profileListGroupHash[aProfileType];

			if (profileListGroup == null)
			{
				switch (aProfileType)
				{
						// begin MID Track 43441 Performance Issues
					case (eProfileType.Allocation):
					{
						profileList = null;
						break;
					}
						// end MID Track 4341 Performance Issues
					case eProfileType.StoreGroupLevel:

						if (_currStoreGroupProfile == null)
						{
							//TODO: Error Logging
							throw new Exception("Current Store Group not specified for Store Group Level retrieval");
						}

						//_currStoreGroupProfile = (StoreGroupProfile)(SAB.ApplicationServerSession.GetProfileList(eProfileType.StoreGroup)).FindKey(_currStoreGroupProfile.Key);
						profileList = StoreMgmt.StoreGroup_GetLevelListFilled(_currStoreGroupProfile.Key); //SAB.ApplicationServerSession.GetStoreGroupLevelProfileList(_currStoreGroupProfile.Key);
						break;

					case eProfileType.StoreGroupLevelListView:

						if (_currStoreGroupProfile == null)
						{
							//TODO: Error Logging
							throw new Exception("Current Store Group not specified for Store Group Level retrieval");
						}

						//profileList = SAB.ApplicationServerSession.GetStoreGroupLevelListViewProfileList(_currStoreGroupProfile.Key, false);
                        profileList = StoreMgmt.StoreGroup_GetLevelListViewList(_currStoreGroupProfile.Key, false);
						break;

					default:

						profileList = GetProfileList(aProfileType);
						break;
				}

				profileListGroup = new ProfileListGroup();
				profileListGroup.MasterProfileList = profileList;
				_profileListGroupHash[aProfileType] = profileListGroup;
                //_masterProfileChangeEvent.ChangeMasterProfile(this, aProfileType, eChangeType.add); // TT#488 - MD - Jellis - Group Allocation  // TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
			}

			return profileListGroup;
		}

		/// <summary>
		/// Removes the master ProfileList as requested by the given eProfileType.
		/// </summary>
		/// <param name="aProfileType">
		/// The eProfileType that identifies which ProfileList to remove.
		/// </param>
        public void RemoveProfileListGroup(eProfileType aProfileType)  //TT#1316-MD - stodd - Severe Relieve in Transit errors for Group Allocation
		{
			if (_profileListGroupHash.Contains(aProfileType))
			{
				_profileListGroupHash.Remove(aProfileType);
                //_masterProfileChangeEvent.ChangeMasterProfile(this, aProfileType, eChangeType.delete); // TT#488 - MD - Jellis - Group Allocation // TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
			}
		}

		public void ApplyFilter(Filter aFilter, eFilterType aFilterType)
		{
			GetProfileListGroup(aFilter.ProfileType).ApplyFilter(aFilter, aFilterType);
		}

		//Begin TT#668 - JScott - Workflow does not match individual
		public void ResetFilter(eProfileType aProfileType)
		{
			GetProfileListGroup(aProfileType).ResetFilteredList();
		}

		//End TT#668 - JScott - Workflow does not match individual
		public ProfileList GetMasterProfileList(eProfileType aProfileType)
		{
			return GetProfileListGroup(aProfileType).MasterProfileList;
		}

		public void SetMasterProfileList(ProfileList aProfileList)
		{
			GetProfileListGroup(aProfileList.ProfileType).MasterProfileList = aProfileList;
            //_masterProfileChangeEvent.ChangeMasterProfile(this, aProfileList.ProfileType, eChangeType.update); // TT#488 - MD - Jellis - Group Allocation  // TT#935 - MD - Jellis - Group Allocation Inftrastructure built wrong
		}

		public void RemoveMasterProfileList(ProfileList aProfileList)
		{
			RemoveProfileListGroup(aProfileList.ProfileType);
		}

		public ProfileList GetFilteredProfileList(eProfileType aProfileType)
		{
			return GetProfileListGroup(aProfileType).FilteredProfileList;
		}

		public void SetFilteredProfileList(ProfileList aProfileList)
		{
			GetProfileListGroup(aProfileList.ProfileType).FilteredProfileList = aProfileList;
		}

		public void RemoveFilteredProfileList(ProfileList aProfileList)
		{
			ProfileListGroup plg = (ProfileListGroup)GetProfileListGroup(aProfileList.ProfileType);
			plg.FilteredProfileList = null;
			if (plg.MasterProfileList == null &&
				plg.FilteredProfileList == null)
			{
				RemoveProfileListGroup(aProfileList.ProfileType);
			}
		}

		/// <summary>
		/// This method removes the ProfileList identified by the given eProfileType from the Transaction's stored list.  This will cause the 
		/// list to be retrieved from the SAB at the next reference.
		/// </summary>
		/// <param name="aProfileType">
		/// The eProfileType that identifies the ProfileList to refresh.
		/// </param>

		public void RefreshProfileLists(eProfileType aProfileType)
		{
			_profileListGroupHash.Remove(aProfileType);
            //_masterProfileChangeEvent.ChangeMasterProfile(this, aProfileType, eChangeType.delete); // TT#488 - MD - Jellis - Group Allocation  // TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
		}

		//========================
		// ProfileXRef functions
		//========================

		/// <summary>
		/// Retrieves the ProfileXRef as requested by the given total and detail eProfileTypes.
		/// </summary>
		/// <returns>
		/// The requested ProfileXRef.
		/// </returns>

		public BaseProfileXRef GetProfileXRef(BaseProfileXRef aProfXRef)
		{
			BaseProfileXRef profileXRef;

			profileXRef = (BaseProfileXRef)_profileXRefHash[aProfXRef];

			//Begin Track #5695 - JScott - Filter with "Month" selection - errors when used
			//if (profileXRef == null && aProfXRef.GetType() == typeof(ProfileXRef))
			//{
			if (profileXRef == null)
			{
				if (aProfXRef.GetType() == typeof(ProfileXRef))
				{
			//End Track #5695 - JScott - Filter with "Month" selection - errors when used
					switch (((ProfileXRef)aProfXRef).TotalType)
					{
						case eProfileType.StoreGroupLevel:

							switch (((ProfileXRef)aProfXRef).DetailType)
							{
								case eProfileType.Store:

									if (_currStoreGroupProfile == null)
									{
										//TODO: Error Logging
										throw new Exception("Current Store Group not specified for Store Group XRef retrieval");
									}

									profileXRef = StoreMgmt.GetStoreGroupLevelXRef(_currStoreGroupProfile.Key); //SAB.StoreServerSession.GetStoreGroupLevelXRef(_currStoreGroupProfile.Key);
									break;

								default:
									profileXRef = SAB.ApplicationServerSession.GetProfileXRef(aProfXRef);
									break;
							}

							break;

						case eProfileType.TimeTotalVariable:

							switch (((ProfileXRef)aProfXRef).DetailType)
							{
								case eProfileType.Variable:
									profileXRef = PlanComputations.PlanVariables.TimeTotalXRef;
									break;

								default:
									profileXRef = SAB.ApplicationServerSession.GetProfileXRef(aProfXRef);
									break;
							}

							break;

						default:
							profileXRef = SAB.ApplicationServerSession.GetProfileXRef(aProfXRef);
							break;
					}
				}
				else
				{
					profileXRef = SAB.ApplicationServerSession.GetProfileXRef(aProfXRef);
				}

				if (profileXRef != null)
				{
					_profileXRefHash.Add(profileXRef, profileXRef);
				}
			//Begin Track #5695 - JScott - Filter with "Month" selection - errors when used
			}
			//End Track #5695 - JScott - Filter with "Month" selection - errors when used

			return profileXRef;
		}

		//========================
		// PlanCubeGroup functions
		//========================

		/// <summary>
		/// Creates and returns a new instance of the StoreMainCubeGroup class.
		/// </summary>
		/// <remarks>
		/// The StoreMaintCubeGroup class represents all the cubes required for Store Plan Maintenance.
		/// </remarks>
		/// <returns></returns>

		public StorePlanMaintCubeGroup CreateStorePlanMaintCubeGroup()
		{
			return new StorePlanMaintCubeGroup(SAB, this);
		}

		public StoreMultiLevelPlanMaintCubeGroup CreateStoreMultiLevelPlanMaintCubeGroup()
		{
			return new StoreMultiLevelPlanMaintCubeGroup(SAB, this);
		}

		public ChainPlanMaintCubeGroup CreateChainPlanMaintCubeGroup()
		{
			return new ChainPlanMaintCubeGroup(SAB, this);
		}

		public ChainMultiLevelPlanMaintCubeGroup CreateChainMultiLevelPlanMaintCubeGroup()
		{
			return new ChainMultiLevelPlanMaintCubeGroup(SAB, this);
		}

		// BEGIN issue 4364 - stodd 4/12/07
		// Begin MID Track #5210 - JSmith - Out of memory
//		public ForecastCubeGroup GetForecastCubeGroup(bool forceNewCubeGroup)
//		{
//			if (_forecastCubeGroup == null)
//			{
//				_forecastCubeGroup = new ForecastCubeGroup(SAB, this);
//			}
//			else
//			{
//				if (forceNewCubeGroup)
//				{
//					_forecastCubeGroup = new ForecastCubeGroup(SAB, this);
//				}
//			}
//			return _forecastCubeGroup;
//		}
		public ForecastCubeGroup GetForecastCubeGroup()
		{
			// BEGIN Issue 5557 stodd
			_forecastCubeGroup = new ForecastCubeGroup(SAB, this);
			return _forecastCubeGroup;
			// END Issue 5557 stodd
		}
		// End MID Track #5210
		// END issue 4364 - stodd 4/12/07

		#region AllocationCubeGroup
		//===============================
		// AllocationCubeGroup functions
		//===============================
		/// <summary>
		/// Gets an instance of the AllocationCubeGroup class associated with this transaction.
		/// </summary>
		/// <remarks>
		/// The AllocationCubeGroup class represents all the cubes required for an Allocation.
		/// </remarks>
		/// <returns>Allocation Cube Group for plan/actual purposes</returns>

		public StorePlanMaintCubeGroup GetAllocationCubeGroup()
		{
			if (_allocationCubeGroup == null)
			{
				_allocationCubeGroup = new StorePlanMaintCubeGroup(SAB, this);
			}
			return _allocationCubeGroup;
		}

        // Begin TT#378 - JSmith - Workflow Run individual methods and get expected results run, Run the same methods in a Workflow and do not get the same results.
        public void ClearAllocationCubeGroup()
        {
            _allocationCubeGroup = null;
			//Begin TT#668 - JScott - Workflow does not match individual
			_allocationBasisCubeGroup = null;
			//End TT#668 - JScott - Workflow does not match individual
		}
        // End TT#378


		/// <summary>
		/// Gets an instance of the AllocationBasisCubeGroup class associated with this transaction.
		/// </summary>
		/// <remarks>
		/// The AllocationBasisCubeGroup class represents all the cubes required for an Allocation.
		/// </remarks>
		/// <returns>Instance of the Allocation Cube Group (for basis purposes in Velocity)</returns>

		public StorePlanMaintCubeGroup GetAllocationBasisCubeGroup()
		{
			if (_allocationBasisCubeGroup == null)
			{
				_allocationBasisCubeGroup = new StorePlanMaintCubeGroup(SAB, this);
				this.OpenAllocationBasisCubeGroup(this.GetAllocationGrandTotalProfile().PlanHnRID, Include.UndefinedDate, Include.UndefinedDate);  // MID Track 2621 Period Basis not processed correctly.
			}
			return _allocationBasisCubeGroup;
		}



		//				AllocationSubtotalProfile grandTotal = this.GetAllocationGrandTotalProfile();
		//				DateTime beginDay;
		//				if (grandTotal.BeginDay == Include.UndefinedDate)
		//				{
		//					beginDay = this.SAB.ApplicationServerSession.Calendar.CurrentWeek.Date;
		//				}
		//				else
		//				{
		//					beginDay = this.SAB.ApplicationServerSession.Calendar.GetWeek(grandTotal.BeginDay).Date;
		//				}
		//				DateTime endDay;
		//				if (grandTotal.EndDay == Include.UndefinedDate)
		//				{
		//					endDay = beginDay.AddDays(7);
		//					Index_RID storeIdxRID;
		//					ProfileList allStores = this.GetMasterProfileList(eProfileType.Store);
		//					DateTime storeShipDay;
		//					foreach (AllocationProfile ap in grandTotal.SubtotalMembers)
		//					{
		//						foreach (StoreProfile sp in allStores)
		//						{
		//							storeIdxRID = this.StoreIndexRID(sp.Key);
		//							storeShipDay = ap.GetStoreShipDay(storeIdxRID);
		//							if (storeShipDay > endDay)   
		//							{
		//								endDay = storeShipDay;
		//							}
		//						}
		//					}
		//				}
		//				else
		//				{
		//					endDay = grandTotal.EndDay;
		//				}
		//
		//				this.OpenAllocationCubeGroup(grandTotal.PlanHnRID, beginDay, endDay);
		//			}
		//			return _allocationCubeGroup;
		//		}
		#endregion AllocationCubeGroup

		//==================
		// Generic Functions
		//==================
		#region isVersionProtected
		/// <summary>
		/// Gets a boolean indicating if the given version is protected for the given week.
		/// 		/// </summary>
		/// <param name="versionRID">
		/// The versionRID of the version to check.
		/// </param>
		/// <param name="yearWeek">
		/// The year/week of the version to check.
		/// </param>
		/// <returns>
		/// A boolean indicating the protected status.
		/// </returns>
		public bool isVersionProtected(int versionRID, int yearWeek)
		{
			try
			{
				object hashValue;
				bool isProtected;
				int hashCode;

				hashCode = ((versionRID & 0xFFFF) << 16) | (yearWeek & 0xFFFF);
				hashValue = _versionProtectedHash[hashCode];
				if (hashValue == null)
				{
					isProtected = false;
					if (((VersionProfile)((ProfileList)this.GetMasterProfileList(eProfileType.Version)).FindKey(versionRID)).ProtectHistory)
					{
						if (SAB.ApplicationServerSession.Calendar.CurrentWeek > SAB.ApplicationServerSession.Calendar.GetWeek(yearWeek))
						{
							isProtected = true;
						}
					}
					_versionProtectedHash.Add(hashCode, isProtected);
				}
				else
				{
					isProtected = (bool)hashValue;
				}
				return isProtected;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		#endregion isVersionProtected

		#region StoreIndexRID
		// begin Track 5953 Null Reference When Executing General Method
		// /// <summary>
		// /// Gets store index value for store RID.
		// /// </summary>
		// /// <param name="aStoreRID">RID identifier for the store.</param>
		// /// <returns>Store Index_RID that describes the index associated with the storeRID; if storeRID is not found, the returned RID in Index_RID is the UndefinedStoreRID.</returns>
		// public Index_RID StoreIndexRID(int aStoreRID)
		//{
		// 	// begin MID Track 4341 Performance Issues
		//	//if (_storeRIDLastRIDKey != aStoreRID)
		//	//{
		//	//	if (_storeRID_IndexXref == null)
		//	//	{
		//	//		BuildStoreRIDIndexXref();
		//	//  removed unnecessary comments
		//	//	}
		//	//	_storeRIDLastRIDKey = aStoreRID;
		//	//if (_storeRID_IndexXref.Contains(aStoreRID))
		//	//{
		//	//	//					_storeRIDLastRIDValue = new Index_RID((int)_storeRID_IndexXref[aStoreRID], aStoreRID);
		//	//	_storeRIDLastRIDValue = (Index_RID)_storeRID_IndexXref[aStoreRID];
		//	//}
		//	//else
		//	//{
		//	//	_storeRIDLastRIDValue = new Index_RID(0, Include.UndefinedStoreRID);
		//	//}
		//	//}
		//	//if (this._rebuildAllocationStores == true 
		//	//	&& this._rebuildAllocationStoresActive == false)
		//	//{
		//	//	Index_RID holdStoreIdxRID = _storeRIDLastRIDValue;
		//	//	BuildAllocationStores();
		//	//	_storeRIDLastRIDValue = holdStoreIdxRID;
		//	//	//_storeRIDLastRIDKey = _storeRIDLastRIDValue.RID; // MID Track 4341 Performance Issues
		//	//}
		//	//return _storeRIDLastRIDValue;
		//	if (this._rebuildAllocationStores == true 
		//		&& this._rebuildAllocationStoresActive == false)
		//	{
		//		BuildAllocationStores();
		//	}
		//	try
		//	{
		//		try
		//		{
		//			return _storeRID_IndexXref[aStoreRID];
		//		}
		//		catch (NullReferenceException)
		//		{
		//			BuildStoreRIDIndexXref();
		//			return _storeRID_IndexXref[aStoreRID];
		//		}
		//	}
		//	catch (IndexOutOfRangeException)
		//	{
		//	}
		//	return new Index_RID(0, Include.UndefinedStoreRID);
		//	// end MID Track 4341 Performance Issues
		//}
		
    	/// <summary>
		/// Gets store index value for store RID (also builds AllocationProfile Store lists).
		/// </summary>
		/// <param name="aStoreRID">RID identifier for the store.</param>
		/// <returns>Store Index_RID that describes the index associated with the storeRID; if storeRID is not found, the returned RID in Index_RID is the UndefinedStoreRID.</returns>
		public Index_RID StoreIndexRID(int aStoreRID)
		{
			Index_RID storeIndexRID = GetStoreIndexRID(aStoreRID);
			if (this._rebuildAllocationStores == true 
				&& this._rebuildAllocationStoresActive == false)
			{
				BuildAllocationStores();
			}
			return storeIndexRID;
		}
		/// <summary>
		/// Gets store index value for store RID (BUT does not build AllocationProfile Store lists)
		/// </summary>
		/// <param name="aStoreRID">RID identifier for the store.</param>
		/// <returns>Store Index_RID that describes the index associated with the storeRID; if storeRID is not found, the returned RID in Index_RID is the UndefinedStoreRID.</returns>
		internal Index_RID GetStoreIndexRID(int aStoreRID)
		{
			try
			{
				try
				{
					return _storeRID_IndexXref[aStoreRID];
				}
				catch (NullReferenceException)
				{
					BuildStoreRIDIndexXref();
					return _storeRID_IndexXref[aStoreRID];
				}
			}
			catch (IndexOutOfRangeException)
			{
			}
			return new Index_RID(0, Include.UndefinedStoreRID);
		}
		// End MID Track 5953 Null Reference when executing General Method
		
		/// <summary>
		/// Builds Store RID/Index cross reference Hashtable.
		/// </summary>
		internal void BuildStoreRIDIndexXref()
		{
			ProfileList _allStoreList = this.GetMasterProfileList(eProfileType.Store);
			// begin MID Track 4341 Performance Issues
			//_storeRID_IndexXref = new System.Collections.Hashtable();
			_storeRID_IndexXref = new Index_RID[_allStoreList.MaxValue + 1];
			for (int i=0; i<_storeRID_IndexXref.Length; i++)
			{
				_storeRID_IndexXref[i] = new Index_RID(0, Include.NoRID);
			}
			// end MID Track 4341 Performance Issues
			int storeRID;
			int storeCount = _allStoreList.Count;
			_allStoreRIDList = new int[storeCount];
			_storeIdxRIDArray = new Index_RID[storeCount];           // MID Track 3567 BOD Calculation Incorrect
			_storeRID_CharArray_List = new ArrayList(storeCount); // MID Track 4341 Performance Issues
			for (int i = 0; i < storeCount; i++)
			{ 
				storeRID = ((Profile)(_allStoreList.ArrayList[i])).Key;
				//_storeRID_IndexXref.Add(storeRID, new Index_RID(i, storeRID)); // MID Track 4341 Performance Issues
				_storeRID_IndexXref[storeRID] = new Index_RID(i, storeRID);; // MID Track 4341 Performance Issues
				_allStoreRIDList[i] = storeRID;
				// begin MID Track 4341 Performance Issues
				//_storeIdxRIDArray[i] = (Index_RID)_storeRID_IndexXref[storeRID]; // MID Track 3567 BOD Calculation Incorrect
				_storeIdxRIDArray[i] = _storeRID_IndexXref[storeRID]; // MID Track 4341 Performance Issues
				_storeRID_CharArray_List.Add(this.GetStoreRID_ToCharArray(storeRID)); // MID Track 4341 Performance Issues
			}
			if (this.GlobalOptions.ReserveStoreRID != Include.UndefinedStoreRID)
			{
				//				_reserveStoreIndexRID = StoreIndexRID(this.GlobalOptions.ReserveStoreRID);
				//_reserveStoreIndexRID = (Index_RID)_storeRID_IndexXref[this.GlobalOptions.ReserveStoreRID]; // MID Trck 4341 Performance Issues
				_reserveStoreIndexRID = _storeRID_IndexXref[this.GlobalOptions.ReserveStoreRID]; // MID Track 4341 Performance issues
			}
			else
			{
				_reserveStoreIndexRID.RID = Include.UndefinedStoreRID;
				_reserveStoreIndexRID.Index = 0;
			}

			//_storeIdxRIDArray = new Index_RID[storeCount];           // MID Track 3567 BOD Calculation Incorrect
			//_storeRID_IndexXref.Values.CopyTo(_storeIdxRIDArray, 0); // MID Track 3567 BOD Calculation Incorrect
			// begin MID Track 4341 Performance Issues
			//RebuildAllocationStores = true; 
            // begin MID Track 5953 Null Reference when executing General Method   
			//if (this._rebuildAllocationStoresActive == false)
			//{
			//	BuildAllocationStores();
			//}
			// end MID Track 5953 Null Reference when executing General Method
			// end MID Track 4341 Performance Issues
		}
		/// <summary>
		/// Loads stores to the AllocationProfiles associated with this transaction.
		/// </summary>
		internal void BuildAllocationStores()
		{
			_rebuildAllocationStoresActive = true;
			RebuildAllocationStores = false; // MID Track 4341 Performance Issues
			AllocationProfileList apl = this.GetAllocationProfileList();
			if (apl != null)
			{
				ArrayList apArray = apl.ArrayList;
				AllocationProfile ap;
				int apArrayCount = apArray.Count;
				for (int i=0; i < apArrayCount; i++)
				{
					ap = (AllocationProfile)apArray[i];
					if (!ap.StoresLoaded)
					{
						ap.LoadStores();
					}
				}
				//RebuildAllocationStores = false; // MID Track 4341 Performance Issues
			}
			_rebuildAllocationStoresActive = false;
		}
		// begin MID Track 5953 Null Reference when executing General Method
		//public Index_RID[] StoreIndexRIDArray()
		//{
		//	if (_storeRID_IndexXref == null)
		//	{
		//		this.BuildStoreRIDIndexXref();
		//	}
		//		// begin MID Track 4341 Performance Issues
		//	else
		//	{
		//		// end MID Track 4341 Performance Issues
		//		if (this._rebuildAllocationStores == true 
		//			&& this._rebuildAllocationStoresActive == false)
		//		{
		//			BuildAllocationStores();
		//		}
		//	}   // MID Track 4341 Performance Issues
		//	return _storeIdxRIDArray;
		//}
		//public int[] AllStoreRIDList()
		//{
		//	if (_allStoreRIDList == null)
		//	{
		//		this.BuildStoreRIDIndexXref();
		//	}
		//		// begin MID Track 4341 Performance Issues
		//	else
		//	{
		//		// end MID Track 4341 Performance Issues
		//		if (this._rebuildAllocationStores == true 
		//			&& this._rebuildAllocationStoresActive == false)
		//		{
		//			BuildAllocationStores();
		//		}
		//	}   // MID Track 4341 Performance Issues
		//	return _allStoreRIDList;
		//}

		/// <summary>
		/// Gets Array of StoreIndexRIDs.  Also builds AllocationProfile stores when necessary.
		/// </summary>
		/// <returns>Index_RID[] array</returns>
		public Index_RID[] StoreIndexRIDArray()
		{
            Index_RID[] storeIndexRIDArray = GetStoreIndexRIDArray();
			if (this._rebuildAllocationStores == true 
				&& this._rebuildAllocationStoresActive == false)
			{
				BuildAllocationStores();
			}
			return storeIndexRIDArray;
		}
		/// <summary>
		/// Gets Array of StoreIndexRIDs.  Does not build AllocationProfile stores.
		/// </summary>
		/// <returns>Index_RID[] array</returns>
		internal Index_RID[] GetStoreIndexRIDArray()
		{
			if (_storeRID_IndexXref == null)
			{
				this.BuildStoreRIDIndexXref();
			}
			return _storeIdxRIDArray;
		}
		/// <summary>
		/// Gets integer array of Store RIDs for the active stores.  Also builds AllocationProfile Stores when necessary.
		/// </summary>
		/// <returns>int[] array of Store RIDs for the active stores.</returns>
		public int[] AllStoreRIDList()
		{
			int[] allStoreRIDList = GetAllStoreRIDList();
			if (this._rebuildAllocationStores == true 
				&& this._rebuildAllocationStoresActive == false)
			{
				BuildAllocationStores();
			}
			return allStoreRIDList;
		}
		/// <summary>
		/// Gets integer array of Store RIDs for the active stores.  Does not build Allocation Profile Stores.
		/// </summary>
		/// <returns>int[] array of Store RIDs for the active stores</returns>
		internal int[] GetAllStoreRIDList()
		{
			if (_allStoreRIDList == null)
			{
				this.BuildStoreRIDIndexXref();
			}
			return _allStoreRIDList;
		}
		// end MID Track 5953 Null REference when executing General Method
		#endregion StoreIndexRID 
		// begin MID Track 4341 Performance Issues
		#region StoreRID_ToCharArray
		/// <summary>
		/// Gets an array list of Store RIDs that are formated as Character arrays.
		/// </summary>
		/// <returns></returns>
		public ArrayList GetStoreRID_CharArray_List()
		{
			if (this._storeRID_CharArray_List == null)
			{
				this.BuildStoreRIDIndexXref();
			}
			return this._storeRID_CharArray_List;
		}
		/// <summary>
		/// Gets a trimmed character array of the store RID
		/// </summary>
		/// <param name="aStoreRID">Store RID</param>
		/// <returns>Trimmed (ie. no leading zeros) character array of the store RID</returns>
		public char[] GetStoreRID_ToCharArray(int aStoreRID)
		{
			char[] storeRID_CharArray = (char[])_storeRID_ToCharArrayHash[aStoreRID];
			if (storeRID_CharArray == null)
			{
				string storeRIDString = aStoreRID.ToString();
				int storeRIDCharIdx = storeRIDString.IndexOfAny(Include.NonZeroDigitCharArray);
				int storeRIDCharLength = storeRIDString.Length - storeRIDCharIdx;
				storeRID_CharArray = new char[storeRIDCharLength];
				storeRIDString.CopyTo(storeRIDCharIdx, storeRID_CharArray,0,storeRIDCharLength);
				_storeRID_ToCharArrayHash.Add(aStoreRID,storeRID_CharArray);
			}
			return storeRID_CharArray;
		}
		#endregion StoreRID_ToCharArray
		// end MID Track 4341 Performance Issues

		#region InStoreReceiptDays
		/// <summary>
		/// Gets the InStoreReceiptDays Hashtable
		/// </summary>
		/// <returns>Hashtable: key=StoreRID; object=Array of 7 bools that indicate store's receipt days during the merchandise week with the first bool corresponding to the first day of the merchandise week.</returns>
		public System.Collections.Hashtable GetInStoreReceiptDays()
		{
			if (_inStoreReceiptDays == null)
			{
                _inStoreReceiptDays = StoreMgmt.GetInStoreReceiptDates(SAB.ApplicationServerSession); //SAB.StoreServerSession.GetInStoreReceiptDates();
			}
			return _inStoreReceiptDays;
		}
		#endregion InStoreReceiptDays

		#region GetStoreOTS_Need
		/// <summary>
		/// Sets and gets a store's need 
		/// </summary>
		/// <param name="aHnRID">Hierarchy node RID for which need is to be calculated</param>
		/// <param name="aVerRID">Plan version RID for which need is to be calculated</param>
		/// <param name="aStoreRID">Store RID for which need is to be calculated</param>
		/// <param name="aBeginWeek">First week of the planning horizon.  </param>
		/// <param name="aEndWeek">Last week of the planning horizon (if the horizon is a "period" horizon, then this week should be the BOW of the last period).</param>
		/// <param name="aEndingStock">BOW stock plan for aEndWeek</param>
		/// <param name="aPartTotalSalesPlan"></param>IF current selling week is in planning horizon, this is total weekly sales plan from current selling week plus 1 through aEndWeek -1; otherwise, this is the total sales plan from aBeginWeek + 1 through aEndWeek - 1  
		/// <param name="aCurrWeekBOW_Plan">BOW stock plan for the current selling week (ignored when current selling week is not in planning horizon)</param>
		/// <returns>Store Need</returns>
		public double GetStoreOTS_Need(
			int aHnRID,                        
			int aVerRID,
			int aStoreRID,    
			WeekProfile aBeginWeek,    
			WeekProfile aEndWeek,
			double aEndingStock,               
			double aPartTotalSalesPlan,            
			double aCurrWeekBOW_Plan,          
			double aCurrWeekSales_Plan)         
		{
			// begin MID Track 4309 OTS basis onhand is not sum of basis onhand
			OTS_NeedRequest ots_NeedRequest = 
				new OTS_NeedRequest(
				aHnRID,
				aVerRID,
				aBeginWeek,
				aEndWeek,
				aEndingStock,
				aPartTotalSalesPlan,
				aCurrWeekBOW_Plan,
				aCurrWeekSales_Plan);
			return GetStoreOTS_Need(
				ots_NeedRequest,
				aStoreRID);
		}
		/// <summary>
		/// Gets a store's accumulated need across multiple basis Hierarchy nodes and time frames.
		/// </summary>
		/// <param name="aOTS_NeedRequest">OTS_NeedRequest Structure array that describes the necessary information to calculate need</param>
		/// <param name="aStoreRID">Store RID for which Need is to be calculated</param>
		/// <returns>Store Need</returns>
		public double GetStoreOTS_Need(
			OTS_NeedRequest[] aOTS_NeedRequest,
			int aStoreRID)
		{
			double storeNeed = 0;
			foreach (OTS_NeedRequest ots_NeedRequest in aOTS_NeedRequest)
			{
				storeNeed += 
					GetStoreOTS_Need(ots_NeedRequest, aStoreRID);
			}
			return storeNeed;
		}
		/// <summary>
		/// Gets a store's need
		/// </summary>
		/// <param name="aOTS_NeedRequest">OTS_NeedRequest Structure that describes the necessary information to calculate need</param>
		/// <param name="aStoreRID">Store RID for which Need is to be calculated</param>
		/// <returns>Store Need</returns>
		public double GetStoreOTS_Need(
			OTS_NeedRequest aOTS_NeedRequest,
			int aStoreRID)
		{
			double[] needParms = this.GetStoreOTS_NeedParameters(aOTS_NeedRequest.OTS_NeedRequestKey, aStoreRID);
			//ResetOTS_NeedHorizon(aOTS_NeedRequestKey);  // NOTE:  This is implied by the previous statement

			needParms[(int)eStoreOTS_NeedParmPosition.Plan] =      // initialize plan
				aOTS_NeedRequest.PartTotalSalesPlan
				+ aOTS_NeedRequest.EndingStock;
			if (_OTS_NeedHorizon.OTS_BeginWeekIsCurrent)
			{
                // Begin TT#987-MD - JSmith - Need Units and $ for VSW stores does not match in Style and OTS Forecast Review Screens.
                //needParms[(int)eStoreOTS_NeedParmPosition.OnHand] = this.GetStoreOnHand(this._OTS_NeedHashLastKey,Include.UndefinedDate, new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize), aStoreRID);
                needParms[(int)eStoreOTS_NeedParmPosition.OnHand] = this.GetStoreOnHand(this._OTS_NeedHashLastKey, Include.UndefinedDate, new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize), aStoreRID)
                        + this.GetStoreImoHistory(this._OTS_NeedHashLastKey, Include.UndefinedDate, new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize), aStoreRID);
                // End TT#987-MD - JSmith - Need Units and $ for VSW stores does not match in Style and OTS Forecast Review Screens.
				double[] dailySales =
					this.GetDailySales(
					this._OTS_NeedHashLastKey,
					//this.StoreIndexRID(aStoreRID),  // MID Track 5953 Null Reference when executing General Method
					this.GetStoreIndexRID(aStoreRID), // MID Track 5953 Null Reference when executing General Method
					_OTS_NeedHorizon.OTS_BeginWeek,
					aOTS_NeedRequest.CurrentWeekSales_Plan);
				if (_OTS_NeedHorizon.OTS_BeginWeek.YearWeek < _OTS_NeedHorizon.OTS_EndWeek.YearWeek)
				{
					for (int d=(_OTS_CurrentSalesDay.DayInWeek - 1); d < _OTS_NeedHorizon.OTS_BeginWeek.DaysInWeek ;d++)
					{
						_OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.Plan] += dailySales[d];  // get remaining sales for the week
					}
				}
				else if (_OTS_NeedHorizon.OTS_BeginWeek.YearWeek == _OTS_NeedHorizon.OTS_EndWeek.YearWeek)
				{
					_OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.Plan] = 0;  // no plan because end date is before or equal begin date
				}
			}
			else
			{
				if (_OTS_NeedHorizon.OTS_BeginWeek.YearWeek == _OTS_NeedHorizon.OTS_EndWeek.YearWeek)
				{
					_OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.Plan] = 0;  // no plan because end date is before or equal begin date
				}
				else
				{
					_OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.Plan] += aOTS_NeedRequest.CurrentWeekSales_Plan;      //  adjust "plan" by planned weekly sales
			
				}
				_OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.OnHand] = aOTS_NeedRequest.CurrentWeekBOW_Plan;       //  use planned BOW as substitute for onhand
			}
			needParms[(int)eStoreOTS_NeedParmPosition.Need] = Need.UnitNeed(_OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.Plan], _OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.OnHand], (int)_OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.InTransit], 0);
			needParms[(int)eStoreOTS_NeedParmPosition.PctNeed] = Need.PctUnitNeed(_OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.Need], _OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.Plan]);
			_OTS_StoreHash.Remove(_OTS_StoreHashLastKey);
			_OTS_LastStoreValues = needParms;
			_OTS_StoreHash.Add(_OTS_StoreHashLastKey, _OTS_LastStoreValues);
			return _OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.Need];
		}
		//		{
		//			long horizonKey = ((long)aBeginWeek.YearWeek << 32) + aEndWeek.YearWeek;
		//			if (horizonKey != _OTS_HorizonKey)
		//			{
		//				_OTS_HorizonKey = horizonKey;
		//				_OTS_CurrentSalesDay = this.SAB.ApplicationServerSession.Calendar.CurrentDate;
		//				WeekProfile currentSalesWeek = this.SAB.ApplicationServerSession.Calendar.GetWeek(_OTS_CurrentSalesDay.Date);
		//				if (currentSalesWeek.YearWeek >= aBeginWeek.YearWeek
		//					&& currentSalesWeek.YearWeek < aEndWeek.YearWeek)
		//				{
		//					this._OTS_BeginWeek = currentSalesWeek;
		//					_OTS_BeginWeekIsCurrent = true;
		//				}
		//				else
		//				{
		//					this._OTS_BeginWeek = aBeginWeek;
		//					_OTS_BeginWeekIsCurrent = false;
		//				}
		//				this._OTS_EndWeek = aEndWeek;
		//			}
		//			double[] needParms = this.GetStoreOTS_NeedParameters(aHnRID, aVerRID, aStoreRID);
		//
		//
		//			needParms[(int)eStoreOTS_NeedParmPosition.Plan] =            // initialize plan
		//				aPartTotalSalesPlan 
		//				+ aEndingStock;
		//			if (_OTS_BeginWeekIsCurrent)
		//			{
		//				needParms[(int)eStoreOTS_NeedParmPosition.OnHand] = this.GetStoreOnHand(this._OTS_NeedHashLastKey,Include.UndefinedDate, new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize), aStoreRID); 
		//				double[] dailySales =
		//					this.GetDailySales(
		//					this._OTS_NeedHashLastKey,
		//					this.StoreIndexRID(aStoreRID),
		//					this._OTS_BeginWeek,
		//					aCurrWeekSalesPlan);
		//				if (_OTS_BeginWeek.YearWeek < _OTS_EndWeek.YearWeek)
		//				{
		//					for (int d=(_OTS_CurrentSalesDay.DayInWeek - 1); d < this._OTS_BeginWeek.DaysInWeek ;d++)
		//					{
		//						_OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.Plan] += dailySales[d];  // get remaining sales for the week
		//					}
		//				}
		//				else if (_OTS_BeginWeek.YearWeek == _OTS_EndWeek.YearWeek)
		//				{
		//					_OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.Plan] = 0;  // no plan because end date is before or equal begin date
		//				}
		//			}
		//			else
		//			{
		//				if (_OTS_BeginWeek.YearWeek == _OTS_EndWeek.YearWeek)
		//				{
		//					_OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.Plan] = 0;  // no plan because end date is before or equal begin date
		//				}
		//				else
		//				{
		//					_OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.Plan] += aCurrWeekSalesPlan;      //  adjust "plan" by planned weekly sales
		//
		//				}
		//				_OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.OnHand] = aCurrWeekBOW_Plan;       //  use planned BOW as substitute for onhand
		//			}
		//			needParms[(int)eStoreOTS_NeedParmPosition.Need] = Need.UnitNeed(_OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.Plan], _OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.OnHand], (int)_OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.InTransit], 0);
		//			needParms[(int)eStoreOTS_NeedParmPosition.PctNeed] = Need.PctUnitNeed(_OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.Need], _OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.Plan]);
		//			_OTS_StoreHash.Remove(_OTS_StoreHashLastKey);
		//			_OTS_LastStoreValues = needParms;
		//			_OTS_StoreHash.Add(_OTS_StoreHashLastKey, _OTS_LastStoreValues);
		//			return _OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.Need];
		//		}
		/// <summary>
		/// Gets a store's need when the need has been previously calculated (and the plan has not changed since the calculation)
		/// </summary>
		/// <param name="aOTS_NeedRequestKey">OTS_NeedRequestKey Structure that describes the key to the previously calculated need</param>
		/// <param name="aStoreRID">Store RID for which need is to be retrieved</param>
		/// <returns></returns>
		public double GetStoreOTS_Need(
			OTS_NeedRequestKey[] aOTS_NeedRequestKey,
			int aStoreRID)
		{
			double storeNeed = 0;
			foreach (OTS_NeedRequestKey ots_NeedRequestKey in aOTS_NeedRequestKey)
			{
				storeNeed += GetStoreOTS_NeedParameters(ots_NeedRequestKey, aStoreRID)[(int)eStoreOTS_NeedParmPosition.Need];
			}
			return storeNeed;
		}

		//		/// <summary>
		//		/// Gets a store's need 
		//		/// </summary>
		//		/// <param name="aHnRID">Hierarchy node RID for which need is to be calculated</param>
		//		/// <param name="aVerRID">Plan version RID for which need is to be calculated</param>
		//		/// <param name="aStoreRID">Store RID for which need is to be calculated</param>
		//		/// <returns>Store Need</returns>
		//		public double GetStoreOTS_Need(
		//			int aHnRID,                        
		//			int aVerRID,
		//			int aStoreRID)    
		//		{
		//            return GetStoreOTS_NeedParameters(aHnRID, aVerRID, aStoreRID)[(int)eStoreOTS_NeedParmPosition.Need];
		//		}
		// end MID Track 4309 OTS basis onhand is not sum of basis onhand
		#endregion GetOTS_Need

		#region GetStoreOTS_NeedParameters
		// begin MID Track 4309 OTS basis onhand not summed
		/// <summary>
		/// Resets _OTS_NeedHorizonHash and the current _OTS_NeedHorizon.
		/// </summary>
		/// <param name="aOTS_NeedRequestKey">Key of the _OTS_NeedHorizon</param>
		private void ResetOTS_NeedHorizon(OTS_NeedRequestKey aOTS_NeedRequestKey)
		{
			if (this._OTS_NeedHorizonHash == null)
			{
				_OTS_NeedHorizonHash = new Hashtable();
				_OTS_CurrentSalesDay = this.SAB.ApplicationServerSession.Calendar.CurrentDate;
				_OTS_CurrentSalesWeek = this.SAB.ApplicationServerSession.Calendar.GetWeek(_OTS_CurrentSalesDay.Date);
				_OTS_NeedHorizon = new OTS_NeedHorizon(0, aOTS_NeedRequestKey.BeginWeek, aOTS_NeedRequestKey.EndWeek, false);
			}

			if (_OTS_NeedHorizon.OTS_NeedHorizonKey != aOTS_NeedRequestKey.NeedHorizonKey)
			{
				if (_OTS_NeedHorizonHash.Contains(aOTS_NeedRequestKey.NeedHorizonKey))
				{
					_OTS_NeedHorizon = (OTS_NeedHorizon)_OTS_NeedHorizonHash[aOTS_NeedRequestKey.NeedHorizonKey];
				}
				else
				{
					if (_OTS_CurrentSalesWeek.YearWeek >= aOTS_NeedRequestKey.BeginWeek.YearWeek
						&& _OTS_CurrentSalesWeek.YearWeek < aOTS_NeedRequestKey.EndWeek.YearWeek)
					{
						_OTS_NeedHorizon = new OTS_NeedHorizon(aOTS_NeedRequestKey.NeedHorizonKey, _OTS_CurrentSalesWeek, aOTS_NeedRequestKey.EndWeek, true);
					}
					else
					{
						_OTS_NeedHorizon = new OTS_NeedHorizon(aOTS_NeedRequestKey.NeedHorizonKey, aOTS_NeedRequestKey.BeginWeek, aOTS_NeedRequestKey.EndWeek, false);
					}
					_OTS_NeedHorizonHash.Add(_OTS_NeedHorizon.OTS_NeedHorizonKey, _OTS_NeedHorizon);
				}
			}
		}
		/// <summary>
		/// Gets the store OTS_NeedParameters (plan, intransit, onhand need and percent need)
		/// </summary>
		/// <param name="aOTS_NeedRequestKey">OTS_NeedRequestKey structure that describes the desired Need Parameters</param>
		/// <param name="aStoreRID">Store RID</param>
		/// <returns>Need Parameters:  Plan, Intransit, Onhand, Need and Percent Need for the requested key</returns>
		private double[] GetStoreOTS_NeedParameters(
			OTS_NeedRequestKey aOTS_NeedRequestKey,
			int aStoreRID)
		{
			ResetOTS_NeedHorizon(aOTS_NeedRequestKey);

			if (_OTS_HorizonHashLastKey != _OTS_NeedHorizon.OTS_NeedHorizonKey)
			{

				_OTS_HorizonHashLastKey = _OTS_NeedHorizon.OTS_NeedHorizonKey;
				_OTS_NeedHashLastKey = Include.NoRID;
				_OTS_VerHashLastKey = Include.NoRID;
				_OTS_StoreHashLastKey = Include.NoRID;


				_OTS_NeedHash = (Hashtable)_OTS_HorizonHash[_OTS_NeedHorizon.OTS_NeedHorizonKey];
				if (_OTS_NeedHash == null)
				{
					_OTS_NeedHash = new Hashtable();
					_OTS_HorizonHash.Add(_OTS_NeedHorizon.OTS_NeedHorizonKey, _OTS_NeedHash);
				}
			}                                                      // MID Track Intransit Reader Error when reading multiple basis
			if (_OTS_NeedHashLastKey != aOTS_NeedRequestKey.HnRID) // MID Track Intransit Reader Error when reading multiple basis
			{                                                      // MID Track Intransit Reader Error when reading multiple basis
				// begin MID Track 4341 Performance
				//if (_OTS_NeedHorizon.OTS_BeginWeekIsCurrent)
				//{
				//	this.GetIntransitReader().SetStoreIT_DayRange(
				//		aOTS_NeedRequestKey.HnRID,
				//		(_OTS_CurrentSalesDay.Date.Year * 1000) + _OTS_CurrentSalesDay.Date.DayOfYear,
				//		(_OTS_NeedHorizon.OTS_EndWeek.Date.Year * 1000) + _OTS_NeedHorizon.OTS_EndWeek.Date.DayOfYear);
				//}
				//else
				//{
				//	this.GetIntransitReader().SetStoreIT_DayRange(
				//		aOTS_NeedRequestKey.HnRID,
				//		(_OTS_NeedHorizon.OTS_BeginWeek.Date.Year * 1000) + _OTS_NeedHorizon.OTS_BeginWeek.Date.DayOfYear,
				//		(_OTS_NeedHorizon.OTS_EndWeek.Date.Year * 1000) + _OTS_NeedHorizon.OTS_EndWeek.Date.DayOfYear);
				//}

                int beginHorizon;
                int endHorizon = (_OTS_NeedHorizon.OTS_EndWeek.Date.Year * 1000) + _OTS_NeedHorizon.OTS_EndWeek.Date.DayOfYear;
                if (_OTS_NeedHorizon.OTS_BeginWeekIsCurrent)
                {
                    beginHorizon = (_OTS_CurrentSalesDay.Date.Year * 1000) + _OTS_CurrentSalesDay.Date.DayOfYear;
                }
                else
                {
                    beginHorizon = (_OTS_NeedHorizon.OTS_BeginWeek.Date.Year * 1000) + _OTS_NeedHorizon.OTS_BeginWeek.Date.DayOfYear;
                }

                // begin TT#4345 - MD - Jellis - GA VSW calculated incorrectly
                //this.GetIntransitReader().SetStoreIT_DayRange(
                //    aOTS_NeedRequestKey.HnRID,
                //    beginHorizon,
                //    endHorizon);
                StoreSalesITHorizon ssIH;
                if (_OTS_NeedHorizon.OTS_BeginWeekIsCurrent)
                {
                    ssIH = new StoreSalesITHorizon(this, _OTS_CurrentSalesDay.Date, _OTS_NeedHorizon.OTS_EndWeek.Date.AddDays(1));
                }
                else
                {
                    ssIH = new StoreSalesITHorizon(this, _OTS_NeedHorizon.OTS_BeginWeek.Date, _OTS_NeedHorizon.OTS_EndWeek.Date.AddDays(1));
                }
                // end TT#4345 - MD - Jellis - GA VSW calculated incorrectly

				//}                                                      // MID Track Intransit Reader Error when reading multiple basis
				//if (_OTS_NeedHashLastKey != aOTS_NeedRequestKey.HnRID) // MID Track Intransit Reader Error when reading multiple basis
				//{                                                      // MID Track Intransit Reader Error when reading multiple basis
				_OTS_NeedHashLastKey = aOTS_NeedRequestKey.HnRID;
				_OTS_IT_HorizonHash = (Hashtable)_OTS_IT_HnRIDHash[_OTS_NeedHashLastKey]; 
				if (_OTS_IT_HorizonHash == null)
				{
					_OTS_IT_HorizonHash = new Hashtable();
					_OTS_IT_HnRIDHash.Add(_OTS_NeedHashLastKey, _OTS_IT_HorizonHash);
				}
				long beginEndHorizonKey = ((long)((long)beginHorizon << 32) + (long)endHorizon);
				_OTS_IT_StoreValues = (int[])_OTS_IT_HorizonHash[beginEndHorizonKey];
				if (_OTS_IT_StoreValues == null)
				{
					IntransitKeyType ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
					_OTS_IT_StoreValues = this.GetIntransitReader().GetStoreIntransitArray(_OTS_NeedHashLastKey, ssIH, ikt); // TT#4345 - MD - JEllis - GA VSW calculated incorrectly
					_OTS_IT_HorizonHash.Add(beginEndHorizonKey, _OTS_IT_StoreValues);
				}
				// end MID Track 4341 Performance issues

				_OTS_VerHashLastKey = Include.NoRID;
				_OTS_StoreHashLastKey = Include.NoRID;
				_OTS_VerHash = (Hashtable)_OTS_NeedHash[aOTS_NeedRequestKey.HnRID];
				if (_OTS_VerHash == null)
				{
					_OTS_VerHash = new Hashtable();
					_OTS_NeedHash.Add(aOTS_NeedRequestKey.HnRID, _OTS_VerHash);
				}
			}
			if (_OTS_VerHashLastKey != aOTS_NeedRequestKey.VersionRID)
			{
				_OTS_VerHashLastKey = aOTS_NeedRequestKey.VersionRID;
				_OTS_StoreHashLastKey = Include.NoRID;
				_OTS_StoreHash = (Hashtable)_OTS_VerHash[aOTS_NeedRequestKey.VersionRID];
				if (_OTS_StoreHash == null)
				{
					_OTS_StoreHash = new Hashtable();
					_OTS_VerHash.Add(aOTS_NeedRequestKey.VersionRID, _OTS_StoreHash);
				}
			}

			if (_OTS_StoreHashLastKey != aStoreRID)
			{
				_OTS_StoreHashLastKey = aStoreRID;
				_OTS_LastStoreValues = (double[])_OTS_StoreHash[aStoreRID];
				if (_OTS_LastStoreValues == null)
				{
					_OTS_LastStoreValues = new double[5];
					_OTS_StoreHash.Add(aStoreRID, _OTS_LastStoreValues);
					// begin MID Track 4341 Performance Issues
					//IntransitKeyType[] ikt = {new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize)};
					//_OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.InTransit] = this.GetStoreInTransit(_OTS_NeedHashLastKey, ikt, aStoreRID);
					// begin MID Track 5953 Null Reference when executing General Method
					//_OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.InTransit] = _OTS_IT_StoreValues[StoreIndexRID(aStoreRID).Index];
					_OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.InTransit] = _OTS_IT_StoreValues[GetStoreIndexRID(aStoreRID).Index];
                    // end MID Track 5953 Null Reference when executing General Method
					
					// end MID Track 4341 Performance issues
				}
			}
			return _OTS_LastStoreValues;
		}
		
		//		/// <summary>
		//		/// Gets a store's plan 
		//		/// </summary>
		//		/// <param name="aHnRID">Hierarchy node RID for which plan is to be calculated</param>
		//		/// <param name="aVerRID">Plan version RID for which plan is to be calculated</param>
		//		/// <param name="aStoreRID">Store RID for which plan is to be calculated</param>
		//		/// <returns>Position 0 = Intransit; 1 = Onhand; 2=Plan; 3= Need; 4=%Need</returns>
		//		public double[] GetStoreOTS_NeedParameters(
		//			int aHnRID,                        
		//			int aVerRID,
		//			int aStoreRID)    
		//		{
		//			if (_OTS_HorizonKey == 0)
		//			{
		//				throw new MIDException (eErrorLevel.fatal,
		//					(int)eMIDTextCode.msg_al_OTS_NeedFailed_StoreNeedNotCalc,
		//					MIDText.GetText(eMIDTextCode.msg_al_OTS_NeedFailed_StoreNeedNotCalc));
		//			}
		//			if (_OTS_NeedHashLastKey != aHnRID)
		//			{
		//				_OTS_NeedHashLastKey = aHnRID;
		//				_OTS_VerHashLastKey = Include.NoRID;
		//				_OTS_StoreHashLastKey = Include.NoRID;
		//				if (_OTS_NeedHash.Contains(aHnRID))
		//				{
		//					_OTS_VerHash = (Hashtable)_OTS_NeedHash[aHnRID];
		//				}
		//				else
		//				{
		//					_OTS_VerHash = new Hashtable();
		//					_OTS_NeedHash.Add(aHnRID, _OTS_VerHash);
		//					if (this._OTS_BeginWeekIsCurrent)
		//					{
		//						this.GetIntransitReader().SetStoreIT_DayRange(
		//							aHnRID,
		//							(_OTS_CurrentSalesDay.Date.Year * 1000) + _OTS_CurrentSalesDay.Date.DayOfYear,
		//							(_OTS_EndWeek.Date.Year * 1000) + _OTS_EndWeek.Date.DayOfYear);
		//					}
		//					else
		//					{
		//						this.GetIntransitReader().SetStoreIT_DayRange(
		//							aHnRID,
		//							(this._OTS_BeginWeek.Date.Year * 1000) + _OTS_BeginWeek.Date.DayOfYear,
		//							(_OTS_EndWeek.Date.Year * 1000) + _OTS_EndWeek.Date.DayOfYear);
		//					}
		//				}
		//			}
		//			if (_OTS_VerHashLastKey != aVerRID)
		//			{
		//				_OTS_VerHashLastKey = aVerRID;
		//				_OTS_StoreHashLastKey = Include.NoRID;
		//				if (_OTS_VerHash.Contains(aVerRID))
		//				{
		//					_OTS_StoreHash = (Hashtable)_OTS_VerHash[aVerRID];
		//				}
		//				else
		//				{
		//					_OTS_StoreHash = new Hashtable();
		//					_OTS_VerHash.Add(aVerRID, _OTS_StoreHash);
		//				}
		//			}
		//			if (_OTS_StoreHashLastKey != aStoreRID)
		//			{
		//				_OTS_StoreHashLastKey = aStoreRID;
		//				if (_OTS_StoreHash.Contains(aStoreRID))
		//				{
		//					_OTS_LastStoreValues = (double[])_OTS_StoreHash[aStoreRID];
		//				}
		//				else
		//				{
		//					_OTS_LastStoreValues = new double[5];
		//					_OTS_StoreHash.Add(aStoreRID, _OTS_LastStoreValues);
		//				}
		//				IntransitKeyType[] ikt = {new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize)};
		//				_OTS_LastStoreValues[(int)eStoreOTS_NeedParmPosition.InTransit] = this.GetStoreInTransit(_OTS_NeedHashLastKey, ikt, aStoreRID);
		//			}
		//			return _OTS_LastStoreValues;
		//		}
		// end MID Track 4309 OTS basis onhand not summed
		#endregion GetStoreOTS_NeedParameters

		#region GetTotalStoreOTS_Need
		// begin MID Track 4309 OTS basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets a Total Store need 
		/// </summary>
		/// <param name="aOTS_NeedRequestKey">OTS_NeedRequestKey structure that provides the necessary information to calculate the requested need</param>
		/// <param name="aStoreRID">List of store RIDs for which an aggregate need is required</param>
		/// <returns>Total Need</returns>
		public double GetTotalStoreOTS_Need(
			OTS_NeedRequestKey[] aOTS_NeedRequestKey,
			int[] aStoreRID)         
		{
			double need = 0;
			for (int s=0; s<aStoreRID.Length; s++)
			{
				need += GetStoreOTS_Need(aOTS_NeedRequestKey,aStoreRID[s]);
			}
			return need;
		}
		//		/// <summary>
		//		/// Gets a Total Store need 
		//		/// </summary>
		//		/// <param name="aHnRID">Hierarchy node RIDs for which need is to be calculated (in 1-to-1 sync with aVerRID)</param>
		//		/// <param name="aVerRID">Plan version RIDs for which need is to be calculated (in 1-to-1 sync with aHnRID)</param>
		//		/// <param name="aStoreRID">Store RIDs for which total need is to be calculated</param>
		//		/// <returns>Store Total Need</returns>
		//		public double GetTotalStoreOTS_Need(
		//			int[] aHnRID,                        
		//			int[] aVerRID,
		//			int[] aStoreRID)         
		//		{
		//            double need = 0;
		//			if (aHnRID.Length != aVerRID.Length)
		//			{
		//				throw new MIDException (eErrorLevel.fatal,
		//					(int)eMIDTextCode.msg_al_OTS_NeedFailed_ArraysNotInSync,
		//					MIDText.GetText(eMIDTextCode.msg_al_OTS_NeedFailed_ArraysNotInSync));
		//			}
		//			for (int i=0; i<aHnRID.Length; i++)
		//			{
		//				for (int s=0; s<aStoreRID.Length; s++)
		//				{
		//					need += this.GetStoreOTS_Need(aHnRID[i], aVerRID[i], aStoreRID[s]);
		//				}
		//			}
		//			return need;
		//		}

		// end MID Track 4309 OTS basis onhand is not sum of basis onhand
		#endregion GetTotalStoreOTS_Need

		#region GetTotalStoreOTS_PctNeed
		// begin MID Track 4309 OTS basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets a Total Store Percent Need
		/// </summary>
		/// <param name="aOTS_NeedRequestKey">Array of OTS_NeedRequestKey structures that desribe the various need calculation requirements</param>
		/// <param name="aStoreRID">Array of store RIDs for which a total need calculation is required</param>
		/// <param name="aNeed">The aggregate need of the stores that was calculated using the OTS_NeedRequest structure parameter</param>
		/// <returns>Total Need Percent</returns>
		public double GetTotalStoreOTS_PctNeed(
			OTS_NeedRequestKey[] aOTS_NeedRequestKey,
			int[] aStoreRID,
			double aNeed)
		{
			double plan = 0;
			double need = 0;
			double[] needParms;
			foreach (OTS_NeedRequestKey ots_NeedRequestKey in aOTS_NeedRequestKey)
			{
				for (int s=0; s<aStoreRID.Length; s++)
				{
					needParms = this.GetStoreOTS_NeedParameters(ots_NeedRequestKey, aStoreRID[s]);
					plan += needParms[(int)eStoreOTS_NeedParmPosition.Plan];
					need += needParms[(int)eStoreOTS_NeedParmPosition.Need];
				}
			}
			return Need.PctUnitNeed(need, plan);
		}
		//		/// <summary>
		//		/// Gets a Total Store Percent Need
		//		/// </summary>
		//		/// <param name="aHnRID">Hierarchy node RIDs for which total store percent need is to be calculated (in 1-to-1 sync with aVerRID)</param>
		//		/// <param name="aVerRID">Plan version RIDs for which total store percent need  is to be calculated (in 1-to-1 sync with aHnRID)</param>
		//		/// <param name="aStoreRID">Store RIDs for which total store percent need  is to be calculated</param>
		//		/// <param name="aNeed">Total Store Need</param>
		//		/// <returns>Store Total Percent Need</returns>
		//		public double GetTotalStoreOTS_PctNeed(
		//			int[] aHnRID,                        
		//			int[] aVerRID,
		//			int[] aStoreRID,
		//			double aNeed)
		//		{
		//			if (aHnRID.Length != aVerRID.Length)
		//			{
		//				throw new MIDException (eErrorLevel.fatal,
		//					(int)eMIDTextCode.msg_al_OTS_NeedFailed_ArraysNotInSync,
		//					MIDText.GetText(eMIDTextCode.msg_al_OTS_NeedFailed_ArraysNotInSync));
		//			}
		//			double plan = 0;
		//			double need = 0;
		//			double[] needParms;
		//			for (int i=0; i<aHnRID.Length; i++)
		//			{
		//				for (int s=0; s<aStoreRID.Length; s++)
		//				{
		//					needParms = this.GetStoreOTS_NeedParameters(aHnRID[i], aVerRID[i], aStoreRID[s]);
		//                    plan += needParms[(int)eStoreOTS_NeedParmPosition.Plan];
		//					need += needParms[(int)eStoreOTS_NeedParmPosition.Need];
		//				}
		//			}
		//			return Need.PctUnitNeed(need, plan);
		//		}

		// end MID Track 4309 OTS basis onhand is not sum of basis onhand
		#endregion GetTotalStoreOTS_Plan

		#region GetStoreOTS_PctNeed
		// begin MID Track 4309 OTS basis onhand is not sum of basis onhand
		//		/// <summary>
		//		/// Gets a store's percent need (GetStoreOTS_Need must be called before calling this method)
		//		/// </summary>
		//		/// <param name="aHnRID">Hierarchy node RID for which percent need is desired</param>
		//		/// <param name="aVerRID">Plan version RID for which need is to be calculated</param>
		//		/// <param name="aStoreRID">Store RID for which need is to be calculated</param>
		//		/// <param name="aNeed">Store Need</param>
		//    	/// <returns>Store Need</returns>
		//		public double GetStoreOTS_PctNeed(
		//			int aHnRID,
		//			int aVerRID,
		//			int aStoreRID,
		//			double aNeed)
		//		{
		//            return this.GetStoreOTS_NeedParameters(aHnRID, aVerRID, aStoreRID)[(int)eStoreOTS_NeedParmPosition.PctNeed];
		//		}
		/// <summary>
		/// Gets a Store Percent Need
		/// </summary>
		/// <param name="aOTS_NeedRequestKey">Array of OTS_NeedRequestKey structures that desribe the various need calculation requirements</param>
		/// <param name="aStoreRID">Store RID for which a need calculation is required</param>
		/// <param name="aNeed">The aggregate need of the store that was calculated using the OTS_NeedRequest structure parameter</param>
		/// <returns>Total Need Percent</returns>
		public double GetStoreOTS_PctNeed(
			OTS_NeedRequestKey[] aOTS_NeedRequestKey,
			int aStoreRID,
			double aNeed)
		{
			int[] storeRIDs = new int[1];
			storeRIDs[0] = aStoreRID;
			return GetTotalStoreOTS_PctNeed(aOTS_NeedRequestKey, storeRIDs, aNeed);
		}
		// end MID Track 4309 OTS basis onhand is not sum of basis onhand
		#endregion GetOTS_PctNeed

		#region GetStoresInGroup
		// begin MID Track 5820 - Unhandled Exception After Store Activation
		/// <summary>
		/// Gets the stores in a group "filtered" by the stores that were active when the 1st allocation profile was accessed
		/// </summary>
		/// <param name="aStoreGrpRID">Store Group RID</param>
		/// <param name="aStoreGrpLvlRID">Store Group Level RID</param>
		/// <returns>Profile List of store profiles in the specified store group level</returns>
		public ProfileList GetActiveStoresInGroup (int aStoreGrpRID, int aStoreGrpLvlRID)
		{
			long storeGrpLvlKey = ((long)aStoreGrpRID << 32) + (long)aStoreGrpLvlRID;
            ProfileList storesInGroup;
            try
            {
                storesInGroup = _storeGroupDict[storeGrpLvlKey];
            }
            catch (KeyNotFoundException)
            {
 				storesInGroup = GetStoresInGroup(aStoreGrpRID, aStoreGrpLvlRID);
				ArrayList removeStore = new ArrayList();
				foreach (StoreProfile sp in storesInGroup)
				{
					if (this.StoreIndexRID(sp.Key).RID == Include.UndefinedStoreRID)
						// NOTE:  track 5953 does not apply here!!  We want the stores loaded to the allocation profile!
					{
						removeStore.Add(sp);
					}
				}
				for (int i=0; i<removeStore.Count; i++)
				{
					storesInGroup.Remove((Profile)removeStore[i]);
				}
				_storeGroupDict.Add(storeGrpLvlKey, storesInGroup);
			}
			return storesInGroup;
		}
		// end MID Track 5820 - Unhandled Exception After Store Activation
		public ProfileList GetStoresInGroup (int aStoreGrpRID, int aStoreGrpLvlRID)
		{
			if (aStoreGrpLvlRID == Include.AllStoreTotal)
			{
				return this.GetMasterProfileList(eProfileType.Store);
			}
			if (this.CurrentStoreGroupProfile == null 
				|| aStoreGrpRID != this.CurrentStoreGroupProfile.Key
				|| this.CurrentStoreGroupProfile.GroupLevels.Count == 0)
			{
				//this.CurrentStoreGroupProfile = (StoreGroupProfile)(SAB.ApplicationServerSession.GetProfileList(eProfileType.StoreGroup)).FindKey(aStoreGrpRID);
                this.CurrentStoreGroupProfile = StoreMgmt.StoreGroup_Get(aStoreGrpRID); //(StoreGroupProfile)(StoreMgmt.GetStoreGroupList()).FindKey(aStoreGrpRID); //TT#1517-MD -jsobek -Store Service Optimization
			}

            ProfileList storeGroupLevelsList = StoreMgmt.StoreGroup_GetLevelListFilled(CurrentStoreGroupProfile.Key); //TT#1517-MD -jsobek -Store Service Optimization

            return ((StoreGroupLevelProfile)storeGroupLevelsList.FindKey(aStoreGrpLvlRID)).Stores; //return ((StoreGroupLevelProfile)SAB.ApplicationServerSession.GetStoreGroupLevelProfileList(CurrentStoreGroupProfile.Key).FindKey(aStoreGrpLvlRID)).Stores; //TT#1517-MD -jsobek -Store Service Optimization
			//End Track #3767 - JScott - Force client to use cached store group lists in application session
		}
		#endregion GetStoresInGroup
		 
		//======================
		// Allocation functions
		//======================
		#region ResetVelocity
		/// <summary>
		/// Resets velocity so that all fields are recalculated
		/// </summary>
		public void ResetVelocity(VelocityMethod aVelocityMethod)
		{
			bool rereadBasis = true;
			this._velocityStorePctSellThruIdx_Chain = null;
			this._velocityStorePctSellThruIdx_Set = null;
			// BEGIN Issue 4778 stodd 10.8.2007 
            if (aVelocityMethod.IsInteractive && !aVelocityMethod.BasisChangesMade) // TT#4522 - stodd - velocity matrix wrong
			{
				// If interactive, we do not need to reset the basis.
				rereadBasis = false;
			}
			else if (aVelocityMethod.IsBasisDynamic()) // 10.17.2007
			{
				// If there is a forecast level in the basis, we need to rebuild the basis;
				_allocationBasisCubeGroup = null;
			}
			// END Issue 4778

            // Begin TT#4522 - stodd - Velocity Matrix incorrect 
            //==============================================================================================
            // Why are we calling GetAllocationFilteredStoreList() and not using anything it returns?
            // Calling this clears and resets the filtered store list, and also rebuilds
            // the _selectedAllocationStores list in the applicationSessionTransaction.
            // This list is used in Velocity (Style) review.
            // Calling SetAllocationFilteredStoreList() does NOT rebuild this list for some reason.
            //==============================================================================================
            //this.GetProfileListGroup(eProfileType.Store).ResetFilteredList();   // TT#4522 - stodd - Velocity Matrix incorrect 
            bool outdatedFilter = false;
            _quickFilterChanged = true;
			AllocationSubtotalProfile grandTotal = this.GetAllocationGrandTotalProfile();
            this.GetAllocationFilteredStoreList(grandTotal.PlanHnRID, Include.UndefinedStoreFilter, ref outdatedFilter);
            // End TT#4522 - stodd - Velocity Matrix incorrect 
			grandTotal.ResetVelocity(rereadBasis); // Issue 4778
			this._velocityCriteria = aVelocityMethod;
		}
		#endregion ResetVelocity

        // begin TT#370 Build Packs Enhancement
        #region ResetBuildPacks
        /// <summary>
        /// Resets Build Packs so that all fields are recalculated
        /// </summary>
        public void ResetBuildPacks(BuildPacksMethod aBuildPacksMethod)
        {
            _buildPacksCriteria = aBuildPacksMethod;
        }
        #endregion ResetBuildPacks
        // end TT#370 Build Packs Enhancement

		#region AllocationQuickFilter
		#region GetAllocationQuickFilter
		/// <summary>
		/// Gets the current allocation quick filtered list of stores.
		/// </summary>
		/// <returns>Profile List of selected stores</returns>
		public ProfileList GetAllocationQuickFilterStoreList()
		{
			ProfileList spl = this.GetMasterProfileList(eProfileType.AllocationQuickFilter);
			if (spl == null)
			{
				spl = this.GetMasterProfileList(eProfileType.Store);
			}
			return spl;
		}
		#endregion GetAllocationQuickFilter

		#region ClearAllocationQuickFilter
		/// <summary>
		/// Clears the effects of quick filters
		/// </summary>
		public void ClearAllocationQuickFilter()
		{
			this.RefreshProfileLists(eProfileType.AllocationQuickFilter);
			this._quickFilterChanged = true;
		}
		#endregion ClearAllocationQuickFilter

		#region BuildAllocationQuickFilter
		/// <summary>
		/// Builds a quick filtered list of stores.
		/// </summary>
		/// <param name="aSubtotalName">Subtotal to use for store selection purposes</param>
		/// <param name="aComponent">Component to use for store selection purposes</param>
		/// <param name="aVariable">The variable to use for store selection purposes</param>
		/// <param name="aCompareQty">The quantity used to which to compare the store variable value.</param>
		/// <param name="aComparison">The comparison to be made between the variable value and compare quantity.</param>
		/// <returns>True: stores were selected; False; there were no stores selected.</returns>
		public bool ApplyAllocationQuickFilter
			//			(bool aClearQuickFilter,
			(string aSubtotalName,
			GeneralComponent aComponent,
			eAllocationWaferVariable aVariable,
			double aCompareQty,
			eFilterComparisonType aComparison)
		{
			AllocationProfile ap = null;
			if (aSubtotalName == string.Empty)
			{
				return ApplyAllocationQuickFilter(
					//					aClearQuickFilter,
					false,
					ap,
					aComponent,
					aVariable,
					aCompareQty,
					aComparison);
			}
			throw new Exception("Subtotal quick filter not implemented");
		}

		/// <summary>
		/// Builds a quick filtered list of stores.
		/// </summary>
		/// <param name="aHeaderRID">Header RID that identifies the header for store selection purposes</param>
		/// <param name="aComponent">Component to use for store selection purposes</param>
		/// <param name="aVariable">The variable to use for store selection purposes</param>
		/// <param name="aCompareQty">The quantity used to which to compare the store variable value.</param>
		/// <param name="aComparison">The comparison to be made between the variable value and compare quantity.</param>
		/// <returns>True: stores were selected; False; there were no stores selected.</returns>
		public bool ApplyAllocationQuickFilter
			//			(bool aClearQuickFilter,
			(int aHeaderRID,
			GeneralComponent aComponent,
			eAllocationWaferVariable aVariable,
			double aCompareQty,
			eFilterComparisonType aComparison)
		{
			AllocationProfile ap = this.GetAllocationProfile(aHeaderRID);
			return ApplyAllocationQuickFilter(
				//				aClearQuickFilter,
				false,
				ap,
				aComponent,
				aVariable,
				aCompareQty,
				aComparison);
		}

		/// <summary>
		/// Builds a quick filtered list of stores.
		/// </summary>
		/// <param name="aClearQuickFilter">True: All stores are candidates for inclusion in the quick filter (the quick filter is cleared and started from scratch); False: Only the selected stores from the previously applied quick filter are candidates for inclusion (in other words, the quick filters will be stacked and te resulting list of stores will be those that satisfied the conditions of all previously applied quick filters.</param>
		/// <param name="aAllocationProfile">Profile to use for store selection purposes (null uses Grand Total)</param>
		/// <param name="aComponent">Component to use for store selection purposes</param>
		/// <param name="aVariable">The variable to use for store selection purposes</param>
		/// <param name="aCompareQty">The quantity used to which to compare the store variable value.</param>
		/// <param name="aComparison">The comparison to be made between the variable value and compare quantity.</param>
		/// <returns>True: stores were selected; False; there were no stores selected.</returns>
		private bool ApplyAllocationQuickFilter
			(bool aClearQuickFilter,
			AllocationProfile aAllocationProfile,
			GeneralComponent aComponent,
			eAllocationWaferVariable aVariable,
			double aCompareQty,
			eFilterComparisonType aComparison)
		{
			AllocationSubtotalProfile GrandTotal = this.GetAllocationGrandTotalProfile();
			ProfileList quickFilterStores;
			if (aClearQuickFilter) 
			{
				quickFilterStores = this.GetMasterProfileList(eProfileType.Store);
			}
			else
			{
				quickFilterStores = this.GetAllocationQuickFilterStoreList();
			}
			ProfileList selectedStores = new ProfileList(eProfileType.AllocationQuickFilter);
			int count = quickFilterStores.Count;
			StoreProfile sp;
			bool[] selected = new bool[count];
			switch (aVariable)
			{
				case (eAllocationWaferVariable.AvgWeeklySales):
				{
					double[] testValue = new double[count];
					for (int i=0; i<count; i++)
					{
						sp = (StoreProfile)quickFilterStores.ArrayList[i];
						testValue[i] = GrandTotal.GetStoreAvgWeeklySales(sp.Key);
					}
					selected = this.QuickFilterSelect(testValue, aCompareQty, aComparison);
					for (int i=0; i<count; i++)
					{
						if (selected[i])
						{
							selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
						}
					}
					break;
				}
				case (eAllocationWaferVariable.AvgWeeklyStock):
				{
					double[] testValue = new double[count];
					for (int i=0; i<count; i++)
					{
						sp = (StoreProfile)quickFilterStores.ArrayList[i];
						testValue[i] = GrandTotal.GetStoreAvgWeeklyStock(sp.Key);
					}
					selected = this.QuickFilterSelect(testValue, aCompareQty, aComparison);
					for (int i=0; i<count; i++)
					{
						if (selected[i])
						{
							selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
						}
					}
					break;				}
				case (eAllocationWaferVariable.BasisInTransit):
				{
					int[] testValue = new int[count];
					for (int i=0; i<count; i++)
					{
						sp = (StoreProfile)quickFilterStores.ArrayList[i];
						testValue[i] = (int)GrandTotal.GetStoreBasisInTransit(sp.Key);
					}
					selected = this.QuickFilterSelect(testValue, (int)aCompareQty, aComparison);
					for (int i=0; i<count; i++)
					{
						if (selected[i])
						{
							selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
						}
					}
					break;
				}
				case (eAllocationWaferVariable.BasisOnHand):
				{
					int[] testValue = new int[count];
					for (int i=0; i<count; i++)
					{
						sp = (StoreProfile)quickFilterStores.ArrayList[i];
						testValue[i] = (int)GrandTotal.GetStoreBasisOnHand(sp.Key);
					}
					selected = this.QuickFilterSelect(testValue, (int)aCompareQty, aComparison);
					for (int i=0; i<count; i++)
					{
						if (selected[i])
						{
							selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
						}
					}
					break;
				}
				case (eAllocationWaferVariable.BasisSales):
				{
					int[] testValue = new int[count];
					for (int i=0; i<count; i++)
					{
						sp = (StoreProfile)quickFilterStores.ArrayList[i];
						testValue[i] = (int)GrandTotal.GetStoreBasisSales(sp.Key);
					}
					selected = this.QuickFilterSelect(testValue, (int)aCompareQty, aComparison);
					for (int i=0; i<count; i++)
					{
						if (selected[i])
						{
							selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
						}
					}
					break;
				}
				case (eAllocationWaferVariable.InTransit):
				{
					int colorRID = Include.IntransitKeyTypeNoColor;
					int sizeRID = Include.IntransitKeyTypeNoSize;
					switch (aComponent.ComponentType)
					{
						case (eComponentType.ColorAndSize):
						{
							AllocationColorSizeComponent acsc = (AllocationColorSizeComponent)aComponent;
							if (acsc.ColorComponent.ComponentType == eComponentType.SpecificColor)
							{
								colorRID = ((AllocationColorOrSizeComponent)acsc.ColorComponent).ColorRID;
							}
							if (acsc.SizeComponent.ComponentType == eComponentType.SpecificSize)
							{
								sizeRID = ((AllocationColorOrSizeComponent)acsc.SizeComponent).SizeRID;
							}
							break;
						}
						case (eComponentType.SpecificColor):
						{
							colorRID = ((AllocationColorOrSizeComponent)aComponent).ColorRID;
							break;
						}
						case (eComponentType.SpecificSize):
						{
							sizeRID = ((AllocationColorOrSizeComponent)aComponent).SizeRID;
							break;
						}
					}
					IntransitKeyType ikt = new IntransitKeyType(colorRID, sizeRID);
					int[] testValue = new int[count];
					for (int i=0; i<count; i++)
					{
						sp = (StoreProfile)quickFilterStores.ArrayList[i];
						testValue[i] = (int)GrandTotal.GetStoreInTransit(ikt, sp.Key);
					}
					selected = this.QuickFilterSelect(testValue, (int)aCompareQty, aComparison);
					for (int i=0; i<count; i++)
					{
						if (selected[i])
						{
							selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
						}
					}
					break;
				}
				case (eAllocationWaferVariable.Need):
				case (eAllocationWaferVariable.OpenToShip):
				{
					int[] testValue = new int[count];
					// begin MID Track 3611 Quick Filter not working in Size Review
					if (aComponent.ComponentType == eComponentType.ColorAndSize)
					{
						for (int i=0; i<count; i++)
						{
							sp = (StoreProfile)quickFilterStores.ArrayList[i];
							testValue[i] = (int)GrandTotal.GetStoreSizeNeed(aAllocationProfile, sp.Key, aComponent, false, false);  // MID Track 4291 add fill variables to size review  // MID track 4921 AnF#666 Fill to Size Plan Enhancement
						}
					}
					else
					{
						for (int i=0; i<count; i++)
						{
							sp = (StoreProfile)quickFilterStores.ArrayList[i];
							testValue[i] = (int)GrandTotal.GetStoreUnitNeed(sp.Key);
						}
					}
					// end MID Track 3611 Quick Filter not working in Size Review
					selected = this.QuickFilterSelect(testValue, (int)aCompareQty, aComparison);
					for (int i=0; i<count; i++)
					{
						if (selected[i])
						{
							selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
						}
					}
					break;
				}
				case (eAllocationWaferVariable.OnHand):
				{
					int colorRID = Include.IntransitKeyTypeNoColor;
					int sizeRID = Include.IntransitKeyTypeNoSize;
					switch (aComponent.ComponentType)
					{
						case (eComponentType.ColorAndSize):
						{
							AllocationColorSizeComponent acsc = (AllocationColorSizeComponent)aComponent;
							if (acsc.ColorComponent.ComponentType == eComponentType.SpecificColor)
							{
								colorRID = ((AllocationColorOrSizeComponent)acsc.ColorComponent).ColorRID;
							}
							if (acsc.SizeComponent.ComponentType == eComponentType.SpecificSize)
							{
								sizeRID = ((AllocationColorOrSizeComponent)acsc.SizeComponent).SizeRID;
							}
							break;
						}
						case (eComponentType.SpecificColor):
						{
							colorRID = ((AllocationColorOrSizeComponent)aComponent).ColorRID;
							break;
						}
						case (eComponentType.SpecificSize):
						{
							sizeRID = ((AllocationColorOrSizeComponent)aComponent).SizeRID;
							break;
						}
					}
					IntransitKeyType ikt = new IntransitKeyType(colorRID, sizeRID);
					int[] testValue = new int[count];
					for (int i=0; i<count; i++)
					{
						sp = (StoreProfile)quickFilterStores.ArrayList[i];
						testValue[i] = (int)GrandTotal.GetStoreOnHand(ikt, sp.Key);
					}
					selected = this.QuickFilterSelect(testValue, (int)aCompareQty, aComparison);
					for (int i=0; i<count; i++)
					{
						if (selected[i])
						{
							selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
						}
					}
					break;
				}
				case (eAllocationWaferVariable.OriginalQuantityAllocated):
				{
					int[] testValue = new int[count];
					for (int i=0; i<count; i++)
					{
						sp = (StoreProfile)quickFilterStores.ArrayList[i];
						testValue[i] = (int)GrandTotal.GetStoreOrigQtyAllocated(aComponent, sp.Key);
					}
					selected = this.QuickFilterSelect(testValue, (int)aCompareQty, aComparison);
					for (int i=0; i<count; i++)
					{
						if (selected[i])
						{
							selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
						}
					}
					break;
				}
				case (eAllocationWaferVariable.PctSellThru):
				{
					double[] testValue = new double[count];
					for (int i=0; i<count; i++)
					{
						sp = (StoreProfile)quickFilterStores.ArrayList[i];
						testValue[i] = this.GetStorePctSellThru(sp.Key);
					}
					selected = this.QuickFilterSelect(testValue, (int)aCompareQty, aComparison);
					for (int i=0; i<count; i++)
					{
						if (selected[i])
						{
							selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
						}
					}
					break;
				}
				case (eAllocationWaferVariable.PctSellThruIdx):
				{
					double[] testValue = new double[count];
					for (int i=0; i<count; i++)
					{
						sp = (StoreProfile)quickFilterStores.ArrayList[i];
						testValue[i] = this.GetStorePctSellThruIdx(sp.Key);
					}
					selected = this.QuickFilterSelect(testValue, (int)aCompareQty, aComparison);
					for (int i=0; i<count; i++)
					{
						if (selected[i])
						{
							selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
						}
					}
					break;
				}
				case (eAllocationWaferVariable.PercentNeed):
				{
					double[] testValue = new double[count];
					// begin MID Track 3611 Quick Filter not working in Size Review
					if (aComponent.ComponentType == eComponentType.ColorAndSize)
					{
						for (int i=0; i<count; i++)
						{
							sp = (StoreProfile)quickFilterStores.ArrayList[i];
							testValue[i] = GrandTotal.GetStoreSizePctNeed(aAllocationProfile, sp.Key, aComponent, false, false); // MID Track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
						}
					}
					else
					{
						for (int i=0; i<count; i++)
						{
							sp = (StoreProfile)quickFilterStores.ArrayList[i];
							testValue[i] = GrandTotal.GetStorePercentNeed(sp.Key);
						}
					}
					// end MID track 3611 Quick Filter not wokring in Size Review
					selected = this.QuickFilterSelect(testValue, (int)aCompareQty, aComparison);
					for (int i=0; i<count; i++)
					{
						if (selected[i])
						{
							selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
						}
					}
					break;
				}
				case (eAllocationWaferVariable.QuantityAllocated):
				{
					int[] testValue = new int[count];
					for (int i=0; i<count; i++)
					{
						sp = (StoreProfile)quickFilterStores.ArrayList[i];
						testValue[i] = GrandTotal.GetStoreQtyAllocated(aComponent, sp.Key);
					}
					selected = this.QuickFilterSelect(testValue, (int)aCompareQty, aComparison);
					for (int i=0; i<count; i++)
					{
						if (selected[i])
						{
							selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
						}
					}
					break;
				}
				case (eAllocationWaferVariable.RuleResults):
				{
					int[] testValue = new int[count];
					for (int i=0; i<count; i++)
					{
						sp = (StoreProfile)quickFilterStores.ArrayList[i];
						testValue[i] = GrandTotal.GetStoreQtyAllocatedByRule(aComponent, sp.Key);
					}
					selected = this.QuickFilterSelect(testValue, (int)aCompareQty, aComparison);
					for (int i=0; i<count; i++)
					{
						if (selected[i])
						{
							selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
						}
					}
					break;
				}
				case (eAllocationWaferVariable.Sales):
				{
					int[] testValue = new int[count];
					for (int i=0; i<count; i++)
					{
						sp = (StoreProfile)quickFilterStores.ArrayList[i];
						testValue[i] = GrandTotal.GetStoreSalesPlan(sp.Key);
					}
					selected = this.QuickFilterSelect(testValue, (int)aCompareQty, aComparison);
					for (int i=0; i<count; i++)
					{
						if (selected[i])
						{
							selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
						}
					}
					break;
				}
				case (eAllocationWaferVariable.SizeCurvePct):
				{
					break;
				}
				case (eAllocationWaferVariable.SizeInTransit):
				{
					break;
				}
				case (eAllocationWaferVariable.SizeNeed):
				{
					break;
				}
				case (eAllocationWaferVariable.SizeOnHand):
				{
					break;
				}
				case (eAllocationWaferVariable.SizeOnHandPlusIT):
				{
					break;
				}
				case (eAllocationWaferVariable.SizePctNeed):
				{
					break;
				}
				case (eAllocationWaferVariable.SizePlan):
				{
					break;
				}
				case (eAllocationWaferVariable.SizePositiveNeed):
				{
					break;
				}
				case (eAllocationWaferVariable.SizePositivePctNeed):
				{
					break;
				}
				case (eAllocationWaferVariable.SizeTotalAllocated):
				{
					break;
				}
				case (eAllocationWaferVariable.Stock):
				{
					int[] testValue = new int[count];
					for (int i=0; i<count; i++)
					{
						sp = (StoreProfile)quickFilterStores.ArrayList[i];
						testValue[i] = GrandTotal.GetStoreStockPlan(sp.Key);
					}
					selected = this.QuickFilterSelect(testValue, (int)aCompareQty, aComparison);
					for (int i=0; i<count; i++)
					{
						if (selected[i])
						{
							selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
						}
					}
					break;
				}
				case (eAllocationWaferVariable.StyleInTransit):
				{
					int colorRID = Include.IntransitKeyTypeNoColor;
					int sizeRID = Include.IntransitKeyTypeNoSize;
					switch (aComponent.ComponentType)
					{
						case (eComponentType.ColorAndSize):
						{
							AllocationColorSizeComponent acsc = (AllocationColorSizeComponent)aComponent;
							if (acsc.ColorComponent.ComponentType == eComponentType.SpecificColor)
							{
								colorRID = ((AllocationColorOrSizeComponent)acsc.ColorComponent).ColorRID;
							}
							if (acsc.SizeComponent.ComponentType == eComponentType.SpecificSize)
							{
								sizeRID = ((AllocationColorOrSizeComponent)acsc.SizeComponent).SizeRID;
							}
							break;
						}
						case (eComponentType.SpecificColor):
						{
							colorRID = ((AllocationColorOrSizeComponent)aComponent).ColorRID;
							break;
						}
						case (eComponentType.SpecificSize):
						{
							sizeRID = ((AllocationColorOrSizeComponent)aComponent).SizeRID;
							break;
						}
					}
					IntransitKeyType ikt = new IntransitKeyType(colorRID, sizeRID);
					int[] testValue = new int[count];
					for (int i=0; i<count; i++)
					{
						sp = (StoreProfile)quickFilterStores.ArrayList[i];
						testValue[i] = GrandTotal.GetStoreStyleInTransit(ikt, sp.Key);
					}
					selected = this.QuickFilterSelect(testValue, (int)aCompareQty, aComparison);
					for (int i=0; i<count; i++)
					{
						if (selected[i])
						{
							selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
						}
					}
					break;
				}
				case (eAllocationWaferVariable.StyleOnHand):
				{
					int colorRID = Include.IntransitKeyTypeNoColor;
					int sizeRID = Include.IntransitKeyTypeNoSize;
					switch (aComponent.ComponentType)
					{
						case (eComponentType.ColorAndSize):
						{
							AllocationColorSizeComponent acsc = (AllocationColorSizeComponent)aComponent;
							if (acsc.ColorComponent.ComponentType == eComponentType.SpecificColor)
							{
								colorRID = ((AllocationColorOrSizeComponent)acsc.ColorComponent).ColorRID;
							}
							if (acsc.SizeComponent.ComponentType == eComponentType.SpecificSize)
							{
								sizeRID = ((AllocationColorOrSizeComponent)acsc.SizeComponent).SizeRID;
							}
							break;
						}
						case (eComponentType.SpecificColor):
						{
							colorRID = ((AllocationColorOrSizeComponent)aComponent).ColorRID;
							break;
						}
						case (eComponentType.SpecificSize):
						{
							sizeRID = ((AllocationColorOrSizeComponent)aComponent).SizeRID;
							break;
						}
					}
					IntransitKeyType ikt = new IntransitKeyType(colorRID, sizeRID);
					int[] testValue = new int[count];
					for (int i=0; i<count; i++)
					{
						sp = (StoreProfile)quickFilterStores.ArrayList[i];
						testValue[i] = GrandTotal.GetStoreStyleOnHand(ikt, sp.Key);
					}
					selected = this.QuickFilterSelect(testValue, (int)aCompareQty, aComparison);
					for (int i=0; i<count; i++)
					{
						if (selected[i])
						{
							selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
						}
					}
					break;
				}
				case (eAllocationWaferVariable.Transfer):
				{
					if (this.VelocityCriteriaExists)
					{
						int[] testValue = new int[count];
						for (int i=0; i<count; i++)
						{
							sp = (StoreProfile)quickFilterStores.ArrayList[i];
							// (CSMITH) - BEG MID Track #2410: Allow interactive processing with multiple headers
							//							testValue[i] = (int)this.Velocity.GetStoreVelocityTransferQty(sp.Key);
							testValue[i] = (int)this.Velocity.GetStoreVelocityTransferQty(aAllocationProfile.HeaderRID, sp.Key);
							// (CSMITH) - END MID Track #2410
						}
						selected = this.QuickFilterSelect(testValue, (int)aCompareQty, aComparison);
						for (int i=0; i<count; i++)
						{
							if (selected[i])
							{
								selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
							}
						}
					}
					break;
				}
				case (eAllocationWaferVariable.VelocityRuleQty):
				{
					if (this.VelocityCriteriaExists)
					{
						double[] testValue = new double[count];
						for (int i=0; i<count; i++)
						{
							sp = (StoreProfile)quickFilterStores.ArrayList[i];
							// (CSMITH) - BEG MID Track #2410: Allow interactive processing with multiple headers
							//							testValue[i] = this.Velocity.GetStoreVelocityRuleQty(sp.Key);
							testValue[i] = this.Velocity.GetStoreVelocityRuleQty(aAllocationProfile.HeaderRID, sp.Key);
							// (CSMITH) - END MID Track #2410
						}
						selected = this.QuickFilterSelect(testValue, (int)aCompareQty, aComparison);
						for (int i=0; i<count; i++)
						{
							if (selected[i])
							{
								selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
							}
						}
					}
					break;
				}
				case (eAllocationWaferVariable.VelocityRuleResult):
				{
					if (this.VelocityCriteriaExists)
					{
						int[] testValue = new int[count];
						for (int i=0; i<count; i++)
						{
							sp = (StoreProfile)quickFilterStores.ArrayList[i];
							// (CSMITH) - BEG MID Track #2410: Allow interactive processing with multiple headers
							//							testValue[i] = (int)this.Velocity.GetStoreVelocityRuleResult(sp.Key);
							testValue[i] = (int)this.Velocity.GetStoreVelocityRuleResult(aAllocationProfile.HeaderRID, sp.Key);
							// (CSMITH) - END MID Track #2410
						}
						selected = this.QuickFilterSelect(testValue, (int)aCompareQty, aComparison);
						for (int i=0; i<count; i++)
						{
							if (selected[i])
							{
								selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
							}
						}
					}
					break;
				}
				case (eAllocationWaferVariable.VelocityRuleType):
				{
					if (this.VelocityCriteriaExists)
					{
						int[] testValue = new int[count];
						for (int i=0; i<count; i++)
						{
							sp = (StoreProfile)quickFilterStores.ArrayList[i];
							// (CSMITH) - BEG MID Track #2410: Allow interactive processing with multiple headers
							//							testValue[i] = (int)this.Velocity.GetStoreVelocityRuleType(sp.Key);
							testValue[i] = (int)this.Velocity.GetStoreVelocityRuleType(aAllocationProfile.HeaderRID, sp.Key);
							// (CSMITH) - END MID Track #2410
						}
						selected = this.QuickFilterSelect(testValue, (int)aCompareQty, aComparison);
						for (int i=0; i<count; i++)
						{
							if (selected[i])
							{
								selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
							}
						}
					}
					break;
				}
			}
			//			if (selectedStores.Count > 0)
			//			{
			this.RefreshProfileLists(eProfileType.AllocationQuickFilter);
			this.SetMasterProfileList(selectedStores);
			this._quickFilterChanged = true;
			return true;
			//			}
			//			return false;

		}

		/// <summary>
		/// Builds a quick filtered list of stores.
		/// </summary>
		/// <param name="aClearQuickFilter">True: All stores are candidates for inclusion in the quick filter (the quick filter is cleared and started from scratch); False: Only the selected stores from the previously applied quick filter are candidates for inclusion (in other words, the quick filters will be stacked and te resulting list of stores will be those that satisfied the conditions of all previously applied quick filters.</param>
		/// <param name="aSubtotalName">Subtotal to use for store selection purposes</param>
		/// <param name="aComponent">Component to use for store selection purposes</param>
		/// <param name="aVariable">The variable to use for store selection purposes</param>
		/// <param name="aCompareQty">The quantity used to which to compare the store variable value.</param>
		/// <param name="aComparison">The comparison to be made between the variable value and compare quantity.</param>
		/// <returns>True: stores were selected; False; there were no stores selected.</returns>
		public bool ApplyAllocationQuickFilter
			(bool aClearQuickFilter,
			string aSubtotalName,
			GeneralComponent aComponent,
			eAllocationWaferVariable aVariable,
			string aCompareQty,
			eFilterComparisonType aComparison)
		{
			AllocationProfile ap = null;
			return ApplyAllocationQuickFilter(
				aClearQuickFilter,
				ap,
				aComponent,
				aVariable,
				aCompareQty,
				aComparison);
		}

		/// <summary>
		/// Builds a quick filtered list of stores.
		/// </summary>
		/// <param name="aClearQuickFilter">True: All stores are candidates for inclusion in the quick filter (the quick filter is cleared and started from scratch); False: Only the selected stores from the previously applied quick filter are candidates for inclusion (in other words, the quick filters will be stacked and te resulting list of stores will be those that satisfied the conditions of all previously applied quick filters.</param>
		/// <param name="aHeaderRID">Header RID that identifies the header for store selection purposes</param>
		/// <param name="aComponent">Component to use for store selection purposes</param>
		/// <param name="aVariable">The variable to use for store selection purposes</param>
		/// <param name="aCompareQty">The quantity used to which to compare the store variable value.</param>
		/// <param name="aComparison">The comparison to be made between the variable value and compare quantity.</param>
		/// <returns>True: stores were selected; False; there were no stores selected.</returns>
		public bool ApplyAllocationQuickFilter
			(bool aClearQuickFilter,
			int aHeaderRID,
			GeneralComponent aComponent,
			eAllocationWaferVariable aVariable,
			string aCompareQty,
			eFilterComparisonType aComparison)
		{
			AllocationProfile ap = this.GetAllocationProfile(aHeaderRID);
			return ApplyAllocationQuickFilter(
				aClearQuickFilter,
				ap,
				aComponent,
				aVariable,
				aCompareQty,
				aComparison);
		}

		/// <summary>
		/// Builds a quick filtered list of stores.
		/// </summary>
		/// <param name="aClearQuickFilter">True: All stores are candidates for inclusion in the quick filter (the quick filter is cleared and started from scratch); False: Only the selected stores from the previously applied quick filter are candidates for inclusion (in other words, the quick filters will be stacked and te resulting list of stores will be those that satisfied the conditions of all previously applied quick filters.</param>
		/// <param name="aAllocationProfile">Profile to use for store selection purposes (null uses Grand Total)</param>
		/// <param name="aComponent">Component to use for store selection purposes</param>
		/// <param name="aVariable">The variable to use for store selection purposes</param>
		/// <param name="aCompareQty">The quantity used to which to compare the store variable value.</param>
		/// <param name="aComparison">The comparison to be made between the variable value and compare quantity.</param>
		/// <returns>True: stores were selected; False; there were no stores selected.</returns>
		private bool ApplyAllocationQuickFilter
			(bool aClearQuickFilter,
			AllocationProfile aAllocationProfile,
			GeneralComponent aComponent,
			eAllocationWaferVariable aVariable,
			string aCompareQty,
			eFilterComparisonType aComparison)
		{
			AllocationSubtotalProfile GrandTotal = this.GetAllocationGrandTotalProfile();
			ProfileList quickFilterStores;
			if (aClearQuickFilter) 
			{
				quickFilterStores = this.GetMasterProfileList(eProfileType.Store);
			}
			else
			{
				quickFilterStores = this.GetAllocationQuickFilterStoreList();
			}
			ProfileList selectedStores = new ProfileList(eProfileType.AllocationQuickFilter);
			int count = quickFilterStores.Count;
			StoreProfile sp;
			bool[] selected = new bool[count];
			switch (aVariable)
			{
				case (eAllocationWaferVariable.StoreGrade):
				{
					string[] testValue = new string[count];
					for (int i=0; i<count; i++)
					{
						sp = (StoreProfile)quickFilterStores.ArrayList[i];
						testValue[i] = GrandTotal.GetStoreGrade(sp.Key);
					}
					selected = this.QuickFilterSelect(testValue, aCompareQty, aComparison);
					for (int i=0; i<count; i++)
					{
						if (selected[i])
						{
							selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
						}
					}
					break;
				}
				case (eAllocationWaferVariable.VelocityGrade):
				{
					if (this.VelocityCriteriaExists)
					{
						string[] testValue = new string[count];
						for (int i=0; i<count; i++)
						{
							sp = (StoreProfile)quickFilterStores.ArrayList[i];
							testValue[i] = GrandTotal.GetStoreVelocityGrade(sp.Key);
						}
						selected = this.QuickFilterSelect(testValue, aCompareQty, aComparison);
						for (int i=0; i<count; i++)
						{
							if (selected[i])
							{
								selectedStores.Add((StoreProfile)quickFilterStores.ArrayList[i]);
							}
						}
					}
					break;
				}
			}
			if (selectedStores.Count > 0)
			{
				this.RefreshProfileLists(eProfileType.AllocationQuickFilter);
				this.SetMasterProfileList(selectedStores);
				return true;
			}
			return false;
		}
		#endregion BuildAllocationQuickFilter

		#region QuickFilterSelect
		/// <summary>
		/// Identifies selected items for a quick filter
		/// </summary>
		/// <param name="aTestThisValue">The test values for each item</param>
		/// <param name="aCompareToValue">A compare value for each item</param>
		/// <param name="aComparison">The comparison to use to select items</param>
		/// <returns>Array of bools.  True:  item is selected; False: item is not selected</returns>
		private bool[] QuickFilterSelect (
			int[] aTestThisValue,
			int aCompareToValue,
			eFilterComparisonType aComparison) 
		{
			bool[] selected = new bool[aTestThisValue.Length];
			switch (aComparison)
			{
				case (eFilterComparisonType.Equal):
				{
					for(int i=0; i<aTestThisValue.Length; i++)
					{
						if (aTestThisValue[i] == aCompareToValue)
						{
							selected[i] = true;
						}
						else
						{
							selected[i] = false;
						}
					}
					break;
				}
				case (eFilterComparisonType.NotEqual):
				{
					for(int i=0; i<aTestThisValue.Length; i++)
					{
						if (aTestThisValue[i] != aCompareToValue)
						{
							selected[i] = true;
						}
						else
						{
							selected[i] = false;
						}
					}
					break;
				}
				case (eFilterComparisonType.Greater):
				case (eFilterComparisonType.NotLessEqual):
				{
					for(int i=0; i<aTestThisValue.Length; i++)
					{
						if (aTestThisValue[i] > aCompareToValue)
						{
							selected[i] = true;
						}
						else
						{
							selected[i] = false;
						}
					}
					break;
				}
				case (eFilterComparisonType.Less):
				case (eFilterComparisonType.NotGreaterEqual):
				{
					for(int i=0; i<aTestThisValue.Length; i++)
					{
						if (aTestThisValue[i] < aCompareToValue)
						{
							selected[i] = true;
						}
						else
						{
							selected[i] = false;
						}
					}
					break;
				}
				case (eFilterComparisonType.LessEqual):
				case (eFilterComparisonType.NotGreater):
				{
					for(int i=0; i<aTestThisValue.Length; i++)
					{
						if (aTestThisValue[i] <= aCompareToValue)
						{
							selected[i] = true;
						}
						else
						{
							selected[i] = false;
						}
					}
					break;
				}
				case (eFilterComparisonType.GreaterEqual):
				case (eFilterComparisonType.NotLess):
				{
					for(int i=0; i<aTestThisValue.Length; i++)
					{
						if (aTestThisValue[i] >= aCompareToValue)
						{
							selected[i] = true;
						}
						else
						{
							selected[i] = false;
						}
					}
					break;
				}
			}
			return selected;;
		}

		/// <summary>
		/// Identifies selected items for a quick filter
		/// </summary>
		/// <param name="aTestThisValue">The test values for each item</param>
		/// <param name="aCompareToValue">A compare value for each item</param>
		/// <param name="aComparison">The comparison to use to select items</param>
		/// <returns>Array of bools.  True:  item is selected; False: item is not selected</returns>
		private bool[] QuickFilterSelect (
			double[] aTestThisValue,
			double aCompareToValue,
			eFilterComparisonType aComparison) 
		{
			bool[] selected = new bool[aTestThisValue.Length];
			switch (aComparison)
			{
				case (eFilterComparisonType.Equal):
				{
					for(int i=0; i<aTestThisValue.Length; i++)
					{
						if (aTestThisValue[i] == aCompareToValue)
						{
							selected[i] = true;
						}
						else
						{
							selected[i] = false;
						}
					}
					break;
				}
				case (eFilterComparisonType.NotEqual):
				{
					for(int i=0; i<aTestThisValue.Length; i++)
					{
						if (aTestThisValue[i] != aCompareToValue)
						{
							selected[i] = true;
						}
						else
						{
							selected[i] = false;
						}
					}
					break;
				}
				case (eFilterComparisonType.Greater):
				case (eFilterComparisonType.NotLessEqual):
				{
					for(int i=0; i<aTestThisValue.Length; i++)
					{
						if (aTestThisValue[i] > aCompareToValue)
						{
							selected[i] = true;
						}
						else
						{
							selected[i] = false;
						}
					}
					break;
				}
				case (eFilterComparisonType.Less):
				case (eFilterComparisonType.NotGreaterEqual):
				{
					for(int i=0; i<aTestThisValue.Length; i++)
					{
						if (aTestThisValue[i] < aCompareToValue)
						{
							selected[i] = true;
						}
						else
						{
							selected[i] = false;
						}
					}
					break;
				}
				case (eFilterComparisonType.LessEqual):
				case (eFilterComparisonType.NotGreater):
				{
					for(int i=0; i<aTestThisValue.Length; i++)
					{
						if (aTestThisValue[i] <= aCompareToValue)
						{
							selected[i] = true;
						}
						else
						{
							selected[i] = false;
						}
					}
					break;
				}
				case (eFilterComparisonType.GreaterEqual):
				case (eFilterComparisonType.NotLess):
				{
					for(int i=0; i<aTestThisValue.Length; i++)
					{
						if (aTestThisValue[i] >= aCompareToValue)
						{
							selected[i] = true;
						}
						else
						{
							selected[i] = false;
						}
					}
				}
					break;
			}
			return selected;;
		}

		/// <summary>
		/// Identifies selected items for a quick filter
		/// </summary>
		/// <param name="aTestThisValue">The test values for each item</param>
		/// <param name="aCompareToValue">A compare value for each item</param>
		/// <param name="aComparison">The comparison to use to select items</param>
		/// <returns>Array of bools.  True:  item is selected; False: item is not selected</returns>
		private bool[] QuickFilterSelect (
			string[] aTestThisValue,
			string aCompareToValue,
			eFilterComparisonType aComparison) 
		{
			bool[] selected = new bool[aTestThisValue.Length];
			switch (aComparison)
			{
				case (eFilterComparisonType.Equal):
				{
					for(int i=0; i<aTestThisValue.Length; i++)
					{
						if (aTestThisValue[i] == aCompareToValue)
						{
							selected[i] = true;
						}
						else
						{
							selected[i] = false;
						}
					}
					break;
				}
				case (eFilterComparisonType.NotEqual):
				{
					for(int i=0; i<aTestThisValue.Length; i++)
					{
						if (aTestThisValue[i] != aCompareToValue)
						{
							selected[i] = true;
						}
						else
						{
							selected[i] = false;
						}
					}
					break;
				}
				case (eFilterComparisonType.Greater):
				case (eFilterComparisonType.NotLessEqual):
				{
					for(int i=0; i<aTestThisValue.Length; i++)
					{
						if ((aTestThisValue[i]).CompareTo(aCompareToValue) > 0)
						{
							selected[i] = true;
						}
						else
						{
							selected[i] = false;
						}
					}
					break;
				}
				case (eFilterComparisonType.Less):
				case (eFilterComparisonType.NotGreaterEqual):
				{
					for(int i=0; i<aTestThisValue.Length; i++)
					{
						if ((aTestThisValue[i]).CompareTo(aCompareToValue) < 0)
						{
							selected[i] = true;
						}
						else
						{
							selected[i] = false;
						}
					}
					break;
				}
				case (eFilterComparisonType.LessEqual):
				case (eFilterComparisonType.NotGreater):
				{
					for(int i=0; i<aTestThisValue.Length; i++)
					{
						if ((aTestThisValue[i]).CompareTo(aCompareToValue) < 1)

						{
							selected[i] = true;
						}
						else
						{
							selected[i] = false;
						}
					}
					break;
				}
				case (eFilterComparisonType.GreaterEqual):
				case (eFilterComparisonType.NotLess):
				{
					for(int i=0; i<aTestThisValue.Length; i++)
					{
						if ((aTestThisValue[i]).CompareTo(aCompareToValue) > -1)

						{
							selected[i] = true;
						}
						else
						{
							selected[i] = false;
						}
					}
					break;
				}
			}
			return selected;;
		}
		#endregion QuickFilterSelect
		#endregion AllocationQuickFilter

		#region AllocationHeaderStatus
		/// <summary>
		/// Gets the current "adjusted" status of a header.
		/// </summary>
		/// <param name="aHeaderRID">RID of the header</param>
		/// <returns>eHeaderAllocationStatus</returns>
		public eHeaderAllocationStatus GetHeaderAllocationStatus(int aHeaderRID)
		{
			return this.GetAllocationProfile(aHeaderRID).HeaderAllocationStatus;
		}
		#endregion AllocationHeaderStatus

		#region DoAllocationAction
		/// <summary>
		/// Executes an Allocation Action.
		/// </summary>
		/// <param name="aAllocationWorkFlowStep">Allocaction Work Flow Step that describes the action and its parameters</param>
		/// <returns>True: if the action executes successfully for all Allocation Headers in the profile list; False: otherwise.</returns>
        // Begin TT#1137 - JEllis - Rebuild Intransit Enhancement
        //public void DoAllocationAction(AllocationWorkFlowStep aAllocationWorkFlowStep)
        //{
        public void DoAllocationAction(AllocationWorkFlowStep aAllocationWorkFlowStep)
        {
            DoAllocationAction(aAllocationWorkFlowStep, false);
        }
        public void DoAllocationAction(AllocationWorkFlowStep aAllocationWorkFlowStep, bool aAllowMultiMemberProcess)
		{
        // End TT#1137
			AllocationProfileList apl = (AllocationProfileList)this.GetMasterProfileList(eProfileType.Allocation);
			this.ResetAllocationActionStatus();
			if (apl != null)
			{
				// Begin Assortment changes
				foreach (object apObj in apl)
				{	
					// BEGIN TT#488-MD - Stodd - Group Allocation
					//if (apObj.GetType() == typeof(AllocationProfile))
					//{
						AllocationProfile ap = (AllocationProfile)apObj;
						// End assortment changes
                    // Begin TT#1137 - JEllis - Rebuild Intransit Enhancement
                    //if (ap.InUseByMulti)
						//Begin TT#362 - MD - DOConnell - Running Balance to Proportional against a header attached to a placeholder does not reduce the Placeholder by the new allocated units
						// This 'IF' is deciding whether to use the assortment/groupAllocation selected header list or 
						// original transaction's selected header list.
                        //if (this.AssortmentViewLoadedFrom == eAssortmentBasisLoadedFrom.AssortmentProperties)
						// Begin TT#925 - MD - stodd - Style/Size Review "No Action Performed" -
                        //if (ActionOrigin != eActionOrigin.AllocationWorkspace && this.UseAssortmentSelectedHeaders)
                        //{
                        //    // Only for group allocation should an "assortment" header be sent through.
                        //    if (ap.HeaderType == eHeaderType.Assortment && ap.AsrtType != (int)eAssortmentType.GroupAllocation)
                        //    {
                        //        break;
                        //    }
                        //    // END TT#488-MD - Stodd - Group Allocation
                        //    // BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
                        //    if (_assortmentSelectedHdrList.Contains(ap.Key))
                        //    // END TT#371-MD - stodd -  Velocity Interactive on Assortment
                        //    {
                        //        if (ap.InUseByMulti && !aAllowMultiMemberProcess)
                        //        // End TT#1137
                        //        {
                        //            SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);
                        //            string msgText = MIDText.GetText(eMIDTextCode.msg_al_HeaderInUseCannotProcess);
                        //            msgText = msgText.Replace("{0}", ap.HeaderID);
                        //            SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msgText, this.GetType().Name);
                        //        }
                        //        else
                        //        {
                        //            aAllocationWorkFlowStep.Method.ProcessAction(
                        //                this.SAB,
                        //                this,
                        //                aAllocationWorkFlowStep,
                        //                ap,
                        //                true,
                        //                Include.NoRID);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                        //    }
                        ////End TT#362 - MD - DOConnell - Running Balance to Proportional against a header attached to a placeholder does not reduce the Placeholder by the new allocated units
                        //}
                        //else
						// End TT#925 - MD - stodd - Style/Size Review "No Action Performed" -

                        {
                            // Only for group allocation should an "assortment" header be sent through.
                            if (ap.HeaderType == eHeaderType.Assortment && ap.AsrtType != (int)eAssortmentType.GroupAllocation
                                && ((eAllocationMethodType)aAllocationWorkFlowStep.Method.MethodType) != eAllocationMethodType.BackoutAllocation)  // TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
                            {
                                // Begin TT#2003-MD - JSmith - Select Cancel Assortment and Process.  After processing the Asstorment and Style review are not cancelled.  Expected them to be 0'd out.
                                //break;
                                continue;
                                // End TT#2003-MD - JSmith - Select Cancel Assortment and Process.  After processing the Asstorment and Style review are not cancelled.  Expected them to be 0'd out.
                            }

                            if (ap.InUseByMulti && !aAllowMultiMemberProcess)
                            // End TT#1137
                            {
                                SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);
                                string msgText = MIDText.GetText(eMIDTextCode.msg_al_HeaderInUseCannotProcess);
                                msgText = msgText.Replace("{0}", ap.HeaderID);
                                SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msgText, this.GetType().Name);
                            }
                            else
                            {
                                aAllocationWorkFlowStep.Method.ProcessAction(
                                    this.SAB,
                                    this,
                                    aAllocationWorkFlowStep,
                                    ap,
                                    true,
                                    Include.NoRID);
                            }
                        }
				    //}		// TT#488-MD - Stodd - Group Allocation
			    }
		    }
		}
		#endregion DoAllocationAction
		#region DoAssortmentAction
		public void DoAssortmentAction(AssortmentWorkFlowStep aAssortmentWorkFlowStep)
		{
            // Begin TT#2005-MD - JSmith - Select Balance Assortment - recieve mssg No Action Performed although the Asst does Balance
            //AllocationProfileList apl = (AllocationProfileList)this.GetMasterProfileList(eProfileType.Allocation);
            AllocationProfileList apl = (AllocationProfileList)this.GetMasterProfileList(eProfileType.AssortmentMember);
            // End TT#2005-MD - JSmith - Select Balance Assortment - recieve mssg No Action Performed although the Asst does Balance
			this.ResetAllocationActionStatus();
         
			if (apl != null)
			{
				foreach (object asrtObj in apl)
				{
                    if (asrtObj.GetType() == typeof(AssortmentProfile))
                    {
                        AssortmentProfile asp = (AssortmentProfile)asrtObj;
                        if (asp.InUseByMulti)
                        {
                            SetAllocationActionStatus(asp.Key, eAllocationActionStatus.ActionFailed);
                            string msgText = MIDText.GetText(eMIDTextCode.msg_al_HeaderInUseCannotProcess);
                            msgText = msgText.Replace("{0}", asp.HeaderID);
                            SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msgText, this.GetType().Name);
                        }
                        else
                        {
                            aAssortmentWorkFlowStep.Method.ProcessAction(
                                this.SAB,
                                this,
                                aAssortmentWorkFlowStep,
                                asp,
                                true,
                                Include.NoRID);

                            //======================================================================
                            // Sets the completion status on the subbordinant headers
                            //======================================================================
							// Begin TT#1228 - stodd
							//==================================================================================================
							// The list above is filled with both Assortment headers and other headers, tied to the assortment.
							// We only process the Assortment headers, but in the end we want the other headers to have the
							// same action status as the assortment header.
							// That's what this code does.
							//==================================================================================================
							eAllocationActionStatus status = this.GetAllocationActionStatus(asp.Key);
							foreach (object hdrObj in apl)
							{
								if (hdrObj.GetType() == typeof(AllocationProfile))
								{
									AllocationProfile ap = (AllocationProfile)hdrObj;
									SetAllocationActionStatus(ap.Key, status);
								}

							}
							//List<int> hdrList = asp.GetHeaderRidList();
							//foreach (int hdrRid in hdrList)
							//{
							//    SetAllocationActionStatus(hdrRid, status);
							//}
							// End TT#1228 - stodd
                        }
                    }
				}
			}
		}
		#endregion DoAssortmentAction

		// Begin TT#980 - MD - stodd - null ref running size need - 
        #region LoadHeadersInTransaction

		// Begin TT#1040 - MD - stodd - header load API for Group Allocation 
        public void LoadHeadersInTransaction(int hdrRid)
        {
            ArrayList selectedHeaderKeyList = new ArrayList();
            ArrayList selectedAssortmentKeyList = new ArrayList();
            selectedHeaderKeyList.Add(hdrRid);

            LoadHeadersInTransaction(selectedHeaderKeyList, selectedAssortmentKeyList, false, true);	// TT#1154-MD - stodd - augument out of range - 
        }
		// End TT#1040 - MD - stodd - header load API for Group Allocation 
		
        public void LoadHeadersInTransaction(SelectedHeaderList selectedHeaderList)
        {
            ArrayList selectedHeaderKeyList = new ArrayList();
            ArrayList selectedAssortmentKeyList = new ArrayList();
            foreach (SelectedHeaderProfile shp in selectedHeaderList.ArrayList)
            {
                selectedHeaderKeyList.Add(shp.Key);
            }

            LoadHeadersInTransaction(selectedHeaderKeyList, selectedAssortmentKeyList, false, true);	// TT#1154-MD - stodd - augument out of range - 
        }

        //Begin TT#1313-MD -jsobek -Header Filters -unused function
        //public void LoadHeadersInTransaction(ArrayList selectedHeaderKeyList, ArrayList selectedAssortmentKeyList)
        //{
        //    LoadHeadersInTransaction(selectedHeaderKeyList, selectedAssortmentKeyList, false, true);	// TT#1154-MD - stodd - augument out of range - 
        //}
        //End TT#1313-MD -jsobek -Header Filters -unused function

		// Begin TT#1154-MD - stodd - augument out of range - 
        public void LoadHeadersInTransaction(ArrayList selectedHeaderKeyList, ArrayList selectedAssortmentKeyList, bool loadHeadersOnly)
        {
            LoadHeadersInTransaction(selectedHeaderKeyList, selectedAssortmentKeyList, loadHeadersOnly, true);
        }
		// End TT#1154-MD - stodd - augument out of range - 
		
        public void LoadHeadersInTransaction(ArrayList selectedHeaderKeyList, ArrayList selectedAssortmentKeyList, bool loadHeadersOnly, bool buildSelectedAssortmentList)	// TT#1154-MD - stodd - augument out of range - 
        {
            try
            {
                // Begin TT#964 - MD - stodd - VSW null ref exception - 
                int[] selectedAssortmentArray = null;
                int[] selectedHeaderArray = null;
                SelectedHeaderList shpl = new SelectedHeaderList(eProfileType.SelectedHeader);

                //=======================================================================================================
                // If an assortment or group allocation is in play, we want to build the assortment member header list
                //=======================================================================================================
                if (selectedAssortmentKeyList.Count > 0)
                {
                    // Converts the selected header key lists into a single selected header profile list
                    shpl = BuildSelectedHeaderList(selectedHeaderKeyList, selectedAssortmentKeyList);	// TT#974 - MD - stodd - cancel GA header and get action failed
                    // Builds the AssortmentMember master profile list within the transaction
                    LoadAssortmentMemberHeaders(shpl);

                    //===============================================================================================================
                    // From the assortment member list, use those allocation profiles to build the master allocation profile list
                    //===============================================================================================================
                    ProfileList ampl = GetMasterProfileList(eProfileType.AssortmentMember);
                    AllocationProfileList apl = new AllocationProfileList(eProfileType.Allocation);
                    foreach (AllocationProfile ap in ampl.ArrayList)
                    {
                        if (loadHeadersOnly)
                        {
                            if (ap.HeaderType != eHeaderType.Assortment)
                            {
                                if (ap.HeaderType == eHeaderType.Placeholder)
                                {
                                    AssortmentProfile asp = (AssortmentProfile)ampl.FindKey(ap.AsrtRID);
                                    if (asp != null)
                                    {
                                        if (asp.AsrtType == (int)eAssortmentType.PreReceipt)
                                        {
                                            apl.Add(ap);    // add the placeholder
                                        }
                                    }
                                    else
                                    {
                                        apl.Add(ap);    // add the placeholder
                                    }
                                }
                                else
                                {
                                    apl.Add(ap);        // Add regular header 
                                }
                            }
                        }
                        else
                        {
                            // Add ALL selected headers (inlcuding Assortment)
                            apl.Add(ap);
                        }
                    }

                    //================================================================
                    // set the master allocation profile list within the transaction
                    //================================================================
                    SetMasterProfileList(apl);
                }
                else    // No Assortment headers in assortment key list
                {
                    // Begin TT#964 - MD - stodd - selected header list - 
                    //====================================================================================================
                    // Look through the selected header list to see if any BELONG to an assortment or Group Allocation
                    //====================================================================================================
                    AllocationHeaderProfileList AsrtHdrList = new AllocationHeaderProfileList(eProfileType.AllocationHeader);
                    foreach (int hdrRid in selectedHeaderKeyList)
                    {
                        AllocationHeaderProfile ahp = SAB.HeaderServerSession.GetHeaderData(hdrRid, false, false, true);
                        if (ahp.AsrtRID != Include.NoRID)
                        {
                            AllocationHeaderProfile ashp = SAB.HeaderServerSession.GetHeaderData(ahp.AsrtRID, false, false, true);
							// Begin TT#1154-MD - stodd - augument out of range - 
							// Removed - stodd
                            //if (buildSelectedAssortmentList)
                            //{
                            //    if (!AsrtHdrList.Contains(ashp.Key))
                            //    {
                            //        AsrtHdrList.Add(ashp);
                            //    }
                            //}
                            //else
                            //{
                                // Only add assortment header, not the rest of the members
                                if (ashp.HeaderType == eHeaderType.Assortment)
                                {
                                    if (!AsrtHdrList.Contains(ashp.Key))
                                    {
                                        AsrtHdrList.Add(ashp);
                                    }
                                }
                            //}
							// End TT#1154-MD - stodd - augument out of range - 
                        }
                    }

                    //======================================================================
                    // No headers belong to an assortment or GA, so build the normal list
                    // Also, goes hear if buildSelectedAssortmentList = false
                    //======================================================================
                    if (AsrtHdrList.Count == 0)
                    {
                        selectedAssortmentArray = new int[selectedAssortmentKeyList.Count];
                        selectedAssortmentKeyList.CopyTo(selectedAssortmentArray);

                        selectedHeaderArray = new int[selectedHeaderKeyList.Count];
                        selectedHeaderKeyList.CopyTo(selectedHeaderArray);

                        LoadHeaders(selectedAssortmentArray, selectedHeaderArray); // TT#488 - MD - Jellis - Group Allocation
                    }
                    else
                    //=========================================================================
                    // Headers were found that belong to an assortment or Group Allocation.
                    // Because of this we want to build the Assortment Member List
                    //=========================================================================
                    {
                        NewAllocationMasterProfileList();
                        // Begin TT#974 - MD - stodd - cancel GA header and get action failed
                        ArrayList origSelectedHeaderKeyList = BackupHeaderKeyList(selectedHeaderKeyList);
						// Begin TT#1154-MD - stodd - null reference when opening selection - 
                        // Build assortment member list normally
                        if (buildSelectedAssortmentList)
                        {
                            selectedHeaderKeyList.Clear();

                            foreach (AllocationHeaderProfile asrtHdr in AsrtHdrList.ArrayList)
                            {
                                GetAllHeadersInAssortment(asrtHdr.Key, selectedHeaderKeyList, selectedAssortmentKeyList);
                            }
                        }
                        else
                        {
                            // Only add the assortments to the member list
                            foreach (AllocationHeaderProfile asrtHdr in AsrtHdrList.ArrayList)
                            {
                                selectedAssortmentKeyList.Add(asrtHdr.Key);
                            }
                        }
						// end TT#1154-MD - stodd - null reference when opening selection - 

                        //========================================================================================
                        // Converts the selected header key lists into a single selected header profile list
                        //========================================================================================
                        shpl = BuildSelectedHeaderList(selectedHeaderKeyList, selectedAssortmentKeyList);
                        //===================================================
                        // Builds the AssortmentMember master profile list
                        //===================================================
                        LoadAssortmentMemberHeaders(shpl);

                        //===============================================================================================================
                        // From the assortment member list, use those allocation profiles to build the master allocation profile list
                        //===============================================================================================================
                        ProfileList ampl = GetMasterProfileList(eProfileType.AssortmentMember);
                        AllocationProfileList apl = new AllocationProfileList(eProfileType.Allocation);
                        foreach (int hdrRid in origSelectedHeaderKeyList)
                        {
                            AllocationProfile ap = (AllocationProfile)ampl.FindKey(hdrRid);
                            if (ap != null)
                            {
                                apl.Add(ap);
                            }
                        }

                        //===============================================================
                        // Set Master allocation header Profile List within transaction
                        //===============================================================
                        SetMasterProfileList(apl);

                        //=========================================
                        // Restores Selected header key lists
                        //=========================================
                        selectedAssortmentKeyList.Clear();
                        selectedHeaderKeyList.Clear();
                        foreach (int hdrRid in origSelectedHeaderKeyList)
                        {
                            selectedHeaderKeyList.Add(hdrRid);
                        }
                        // End TT#974 - MD - stodd - cancel GA header and get action failed
                    }
                    // End TT#964 - MD - stodd - selected header list - 
                }
                // End TT#964 - MD - stodd - VSW null ref exception - 
            }
            catch
            {
                throw;
            }
        }
        // End TT#964 - MD - stodd - style review includes placeholder - 
        // End TT#2 

        // Begin TT#974 - MD - stodd - cancel GA header and get action failed
        /// <summary>
        /// Converts the two selected key lists into a single selected header profile list.
        /// </summary>
        private SelectedHeaderList BuildSelectedHeaderList(ArrayList selectedHeaderKeyList, ArrayList selectedAssortmentKeyList)
        {
            SelectedHeaderList shpl = new SelectedHeaderList(eProfileType.SelectedHeader);
            foreach (int hdrRid in selectedAssortmentKeyList)
            {
                AllocationHeaderProfile ahp = SAB.HeaderServerSession.GetHeaderData(hdrRid, false, false, true);
                SelectedHeaderProfile shp = new SelectedHeaderProfile(ahp.Key);
                shp.HeaderType = ahp.HeaderType;
                shpl.Add(shp);
            }
            foreach (int hdrRid in selectedHeaderKeyList)
            {
                AllocationHeaderProfile ahp = SAB.HeaderServerSession.GetHeaderData(hdrRid, false, false, true);
                SelectedHeaderProfile shp = new SelectedHeaderProfile(ahp.Key);
                shp.HeaderType = ahp.HeaderType;
                shpl.Add(shp);
            }
            return shpl;
        }

        private void GetAllHeadersInAssortment(int aAsrtRID, ArrayList selectedHeaderKeyList, ArrayList selectedAssortmentKeyList)
        {
            try
            {
                ArrayList al = SAB.HeaderServerSession.GetHeadersInAssortment(aAsrtRID);
                for (int i = 0; i < al.Count; i++)
                {
                    int hdrRID = (int)al[i];
                    // begin TT#488 - MD - Jellis - Group Allocation
                    if (hdrRID == aAsrtRID)
                    {
                        if (!selectedAssortmentKeyList.Contains(hdrRID))
                        {
                            selectedAssortmentKeyList.Add(hdrRID);
                        }
                    }
                    else
                    {
                        // end TT#488 - MD - Jellis - Group Allocation
                        if (!selectedHeaderKeyList.Contains(hdrRID))
                        {
                            selectedHeaderKeyList.Add(hdrRID);
                        }
                    }  // TT#488 - MD - Jellis - Group Allocation
                }
            }
            catch
            {
                throw;
            }
        }

        private ArrayList BackupHeaderKeyList(ArrayList aList)
        {
            ArrayList origHeaderKeyList = new ArrayList();
            foreach (int hdrRid in aList)
            {
                origHeaderKeyList.Add(hdrRid);
            }
            return origHeaderKeyList;
        }

        #endregion LoadHeadersInTransaction
		// End TT#980 - MD - stodd - null ref running size need -
		
        #region NewAllocationMasterProfileList
        // begin TT#488 -  MD - Jellis - Group Allocation
        /// <summary>
        /// Creates AllocationProfileList Master from the SelectedHeaderList Master
        /// </summary>
        /// <returns>AllocationProfileList Master from the SelectedHeaderList</returns>
        public AllocationProfileList CreateMasterAllocationProfileListFromSelectedHeaders()
        {
            SelectedHeaderList selectedHeaderList = (SelectedHeaderList)GetProfileList(eProfileType.SelectedHeader);
            return CreateMasterAllocationProfileListFromSelectedHeaders(selectedHeaderList);
        }
        /// <summary>
        /// Creates AllocationProfileList Master from the SelectedHeaderProfiles
        /// </summary>
        /// <param name="aSelectedHeaderList">SelectedHeaderList identifying the desired SelectedHeaderProfiles</param>
        /// <returns>AllocationProfileList continaing the selected headers</returns>
        public AllocationProfileList CreateMasterAllocationProfileListFromSelectedHeaders(SelectedHeaderList aSelectedHeaderList)
        {
            AllocationProfileList apl = NewAllocationMasterProfileList();
            if (aSelectedHeaderList.Count > 0)
            {
                apl.LoadHeaders(this, aSelectedHeaderList, SAB.ApplicationServerSession);
            }
            // Begin TT#1966-MD - JSmith - DC Fulfillment
            BuildMasterSubordinateReference(apl);
            // End TT#1966-MD - JSmith - DC Fulfillment
            return apl;
        }
        /// <summary>
        /// Creates AllocationProfileList Master from the selected Header RIDs (reads Database)
        /// </summary>
        /// <param name="aSelectedHeaderRIDs">RIDs of the headers to include in the list</param>
        /// <returns>AllocationProfileList continaing the selected headers</returns>
        public AllocationProfileList CreateMasterAllocationProfileListFromSelectedHeaders(int[] aAssortmentRIDList, int[] aSelectedHeaderRIDs)
        {
            AllocationProfileList apl = NewAllocationMasterProfileList();
            if (aSelectedHeaderRIDs.Length > 0)
            {
                apl.LoadHeaders(this, aAssortmentRIDList, aSelectedHeaderRIDs, SAB.ApplicationServerSession);
            }
            // Begin TT#1966-MD - JSmith - DC Fulfillment
            BuildMasterSubordinateReference(apl);
            // End TT#1966-MD - JSmith - DC Fulfillment
            return apl;
        }
        public AllocationProfileList CreateMasterAllocationProfileListFromSelectedHeaders(AllocationHeaderProfileList aAllocationHeaderProfileList)
        {
            AllocationProfileList apl = NewAllocationMasterProfileList();
            if (aAllocationHeaderProfileList.Count > 0)
            {
                apl.LoadHeaders(this, aAllocationHeaderProfileList, SAB.ApplicationServerSession);
            }
            // Begin TT#1966-MD - JSmith - DC Fulfillment
            BuildMasterSubordinateReference(apl);
            // End TT#1966-MD - JSmith - DC Fulfillment
            return apl;
        }

        // Begin TT#1966-MD - JSmith - DC Fulfillment
        private void BuildMasterSubordinateReference(AllocationProfileList apl)
        {
            _instanceTracker.Clear();
            AllocationProfileList allocationList = (AllocationProfileList)GetMasterProfileList(eProfileType.Allocation);
            foreach (AllocationProfile ap in allocationList)
            {
                if (ap.IsSubordinateHeader)
                {
                    GetSubordinates(ap, true);
#if (DEBUG)
                    CheckInstance(ap.Key, ap.InstanceID);
#endif
                }
            }
        }
        // End TT#1966-MD - JSmith - DC Fulfillment

		// Begin TT#867 - MD (with #TT486-MD manual merge) - stodd - post-reciept list s/n contain placeholders
		/// <summary>
		/// Creates the AllocationProfileList Master from the Assortment Master List
		/// </summary>
		/// <param name="aIncludeAssortmentProfile">True: Include AssortmentProfile in the list</param>
		/// <returns>AllocationProfileList Master containing the AllocationProfiles from the Assortment Master</returns>
		public AllocationProfileList CreateAllocationProfileListFromAssortmentMaster(bool aIncludeAssortmentProfile)
		{
			return CreateAllocationProfileListFromAssortmentMaster(aIncludeAssortmentProfile, true);
		}

		// BEGIN TT#1194-MD - stodd - view ga header
        public AllocationProfileList CreateAllocationProfileListFromAssortmentMaster(bool aIncludeAssortmentProfile, bool aIncludePlaceholders)
        {
            return CreateAllocationProfileListFromAssortmentMaster(aIncludeAssortmentProfile, aIncludePlaceholders, true);
        }

        /// <summary>
        /// Creates the AllocationProfileList Master from the Assortment Master List
        /// </summary>
        /// <param name="aIncludeAssortmentProfile">True: Include AssortmentProfile in the list</param>
		/// <param name="aIncludePlaceholders">True: Include placeholder headers in the list</param>
        /// <returns>AllocationProfileList Master containing the AllocationProfiles from the Assortment Master</returns>
		public AllocationProfileList CreateAllocationProfileListFromAssortmentMaster(bool aIncludeAssortmentProfile, bool aIncludePlaceholders, bool aIncludeHeaders)
        {
            AllocationProfileList apl = NewAllocationMasterProfileList();
            AllocationProfileList asrtList = (AllocationProfileList)GetMasterProfileList(eProfileType.AssortmentMember);

            foreach (AllocationProfile ap in asrtList)
            {
                if (ap.HeaderType == eHeaderType.Assortment)
                {
                    if (aIncludeAssortmentProfile)
                    {
                        apl.Add(ap);
                    }
                } 
                else if (ap.HeaderType == eHeaderType.Placeholder)
                {
                    if (aIncludePlaceholders)
                    {
                        apl.Add(ap);
                    }
                }
                else if (aIncludeHeaders)
                {
                    apl.Add(ap);
                }
            }

            //if (aIncludeAssortmentProfile)
            //{
            //    foreach (AllocationProfile ap in asrtList)
            //    {
            //        if (ap.HeaderType == eHeaderType.Placeholder)
            //        {
            //            if (aIncludePlaceholders)
            //            {
            //                apl.Add(ap);
            //            }
            //        }
            //        else
            //        {
            //            apl.Add(ap);
            //        }
            //    }
            //}
            //else
            //{
            //    foreach (AllocationProfile ap in asrtList)
            //    {
            //        if (!(ap is AssortmentProfile))
            //        {
            //            if (ap.HeaderType == eHeaderType.Placeholder)
            //            {
            //                if (aIncludePlaceholders)
            //                {
            //                    apl.Add(ap);
            //                }
            //            }
            //            else
            //            {
            //                apl.Add(ap);
            //            }
            //        }
            //    }
            //}
			// END TT#1194-MD - stodd - view ga header
			// End TT#867 - MD (with #TT486-MD manual merge) - stodd - post-reciept list s/n contain placeholders
            AddAllocationProfileToGrandTotal(apl);
            return apl;
        }
        /// <summary>
        /// Creates the AllocationProfileList Master from the Assortment Master list
        /// </summary>
        /// <param name="aIncludeAssortmentProfile">True: Includes AssortmentProfile in the AllocationProfileList Master</param>
        /// <param name="aSelectedHeaderList">SelectedHeaderRIDs identifies the RID of the AllocationProfiles to include IF it is already in the Assortment Master List</param>
        /// <param name="addHeaders"> should headers for selected assortment be included</param>
        /// <returns>AllocationProfileList containing the selected AllocationProfiles from the Assortment Master List</returns>
		// Begin TT#889 - MD - stodd - need not working - 
        public AllocationProfileList CreateAllocationProfileListFromAssortmentMaster(bool aIncludeAssortmentProfile, int[] aSelectedHeaderRIDs, bool addHeaders)
		// End TT#889 - MD - stodd - need not working
        {
            AllocationProfileList apl = NewAllocationMasterProfileList();
            AllocationProfileList asrtList = (AllocationProfileList)GetMasterProfileList(eProfileType.AssortmentMember);
            AllocationProfile ap;
            if (aIncludeAssortmentProfile)
            {
                foreach (int apKey in aSelectedHeaderRIDs)
                {
                    ap = (AllocationProfile)asrtList.FindKey(apKey);
                    if (ap != null)
                    {
                        apl.Add(ap);
						// Begin TT#889 - MD - stodd - need not working
                        if (ap.HeaderType == eHeaderType.Assortment && addHeaders)
                        {
                            foreach (AllocationProfile asap in asrtList.ArrayList)
                            {
                                if (asap.AsrtRID == ap.Key)
                                {
                                    apl.Add(asap);
                                }
                            }
                        }
						// End TT#889 - MD - stodd - need not working
                    }
                }
            }
            else
            {
                foreach (int apKey in aSelectedHeaderRIDs)
                {
                    ap = (AllocationProfile)asrtList.FindKey(apKey);
                    if (ap != null)
                    {
                        // Begin TT#4988 - BVaughan - Performance
                        #if DEBUG
                        if ((ap is AssortmentProfile && !ap.isAssortmentProfile) || (!(ap is AssortmentProfile) && ap.isAssortmentProfile))
                        {
                            throw new Exception("Object does not match AssortmentProfile in CreateAllocationProfileListFromAssortmentMaster()");
                        }
                        #endif
                        //if (!(ap is AssortmentProfile))
                        if (!(ap.isAssortmentProfile))
                        // End TT#4988 - BVaughan - Performance
                        {
                            apl.Add(ap);
							// Begin TT#889 - MD - stodd - need not working
                            if (ap.HeaderType == eHeaderType.Assortment && addHeaders)
                            {
                                foreach (AllocationProfile asap in asrtList.ArrayList)
                                {
                                    if (asap.AsrtRID == ap.Key)
                                    {
                                        apl.Add(asap);
                                    }
                                }
                            }
							// End TT#889 - MD - stodd - need not working
                        }
                    }
                }
            }
            AddAllocationProfileToGrandTotal(apl);
            return apl;
        }
        /// <summary>
        /// Creates the AllocationProfileList Master from the Assortment Master list
        /// </summary>
        /// <param name="aIncludeAssortmentProfile">True: Includes AssortmentProfile in the AllocationProfileList Master</param>
        /// <param name="aSelectedHeaderList">SelectedHeaderProfile List to include in the AllocationProfileList IF it is already in the Assortment Master List</param>
        /// <returns>AllocationProfileList containing the selected AllocationProfiles from the Assortment Master List</returns>
		// Begin TT#889 - MD - stodd - need not working
        public AllocationProfileList CreateAllocationProfileListFromAssortmentMaster(bool aIncludeAssortmentProfile, SelectedHeaderList aSelectedHeaderList, bool addHeaders)
		// End TT#889 - MD - stodd - need not working
        {
            AllocationProfileList apl = NewAllocationMasterProfileList();
            AllocationProfileList asrtList = (AllocationProfileList)GetMasterProfileList(eProfileType.AssortmentMember);
            AllocationProfile ap;
            if (aIncludeAssortmentProfile)
            {
                foreach (SelectedHeaderProfile shp in aSelectedHeaderList)
                {
                    ap = (AllocationProfile)asrtList.FindKey(shp.Key);
                    if (ap != null)
                    {
                        apl.Add(ap);
						// Begin TT#889 - MD - stodd - need not working
                        if (ap.HeaderType == eHeaderType.Assortment && addHeaders)
                        {
                            foreach (AllocationProfile asap in asrtList.ArrayList)
                            {
                                if (asap.AsrtRID == ap.Key)
                                {
                                    apl.Add(asap);
                                }
                            }
                        }
						// End TT#889 - MD - stodd - need not working
                    }
                }
            }
            else
            {
                foreach (SelectedHeaderProfile shp in aSelectedHeaderList)
                {
                    ap = (AllocationProfile)asrtList.FindKey(shp.Key);
                    if (ap != null)
                    {
                        // Begin TT#4988 - BVaughan - Performance
                        #if DEBUG
                        if ((ap is AssortmentProfile && !ap.isAssortmentProfile) || (!(ap is AssortmentProfile) && ap.isAssortmentProfile))
                        {
                            throw new Exception("Object does not match AssortmentProfile in CreateAllocationProfileListFromAssortmentMaster()");
                        }
                        #endif
                        //if (!(ap is AssortmentProfile))
                        if (!(ap.isAssortmentProfile))
                        // End TT#4988 - BVaughan - Performance
                        {
                            apl.Add(ap);
							// Begin TT#889 - MD - stodd - need not working
                            if (ap.HeaderType == eHeaderType.Assortment && addHeaders)
                            {
                                foreach (AllocationProfile asap in asrtList.ArrayList)
                                {
                                    if (asap.AsrtRID == ap.Key)
                                    {
                                        apl.Add(asap);
                                    }
                                }
                            }
							// End TT#889 - MD - stodd - need not working
                        }
                    }
                }
            }
            AddAllocationProfileToGrandTotal(apl);
            return apl;
        }
        // end TT#488 - MD - Jellis - Group Allocaiton
		/// <summary>
		/// Create a new allocation profile list on the application server
		/// and set it to the allocation master profile list
		/// </summary>
		public AllocationProfileList NewAllocationMasterProfileList() // TT#488 - MD - Jellis - Group Allocation
		{
            AllocationProfileList allocationList = (AllocationProfileList)GetMasterProfileList(eProfileType.Allocation);
            // begin TT#1042 - MD - Jellis - Qty Allocated Cannot Be Negative part 2
            if (_grandTotalProfile != null)
            {
                _grandTotalProfile.RemoveAllSubtotalMembers();
            }
            // end TT#1042 - MD - Jellis - Qty Allocated Cannot Be Negative part 2
            if (allocationList == null)
            {
                allocationList = new AllocationProfileList(eProfileType.Allocation);
                SetMasterProfileList(allocationList);
            }
            else
            {
                //RemoveAllocationProfileFromGrandTotal(allocationList); // TT#1042 - MD - Jellis - Qty Allocated Cannot Be Negative part 2
				// Begin TT#1561-MD - stodd - drag/drop a PPK header to a place holder and receive a null reference exception
				// Replaced the Clear() because of timing issues with the AllocationProfile.Dispose() logic that clears the
				// Allocation Subtotal Profile. 
                //allocationList.Clear();
                allocationList = new AllocationProfileList(eProfileType.Allocation);
                SetMasterProfileList(allocationList);
				// End TT#1561-MD - stodd - drag/drop a PPK header to a place holder and receive a null reference exception
            }
            return allocationList;
		}
		#endregion

		// BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
		#region NewAssortmentMemberMasterProfileList
        // begin TT#488 -  MD - Jellis - Group Allocation
        /// <summary>
        /// Creates Assortment Member Profile Master from the SelectedHeaderList Master
        /// </summary>
        /// <returns>Assortment AllocationProfileList Master from the SelectedHeaderList</returns>
        public AllocationProfileList CreateMasterAssortmentMemberListFromSelectedHeaders()
        {
            SelectedHeaderList selectedHeaderList = (SelectedHeaderList)GetProfileList(eProfileType.SelectedHeader);
            return CreateMasterAssortmentMemberListFromSelectedHeaders(selectedHeaderList);
        }
        /// <summary>
        /// Creates Assortment Member Profile Master from the SelectedHeaderProfiles
        /// </summary>
        /// <param name="aSelectedHeaderList">SelectedHeaderList identifying the desired SelectedHeaderProfiles</param>
        /// <returns>Assortment AllocationProfileList continaing the selected headers</returns>
        public AllocationProfileList CreateMasterAssortmentMemberListFromSelectedHeaders(SelectedHeaderList aSelectedHeaderList)
        {
            AllocationProfileList apl = NewAssortmentMemberMasterProfileList();
            if (aSelectedHeaderList.Count > 0)
            {
                apl.LoadHeaders(this, aSelectedHeaderList, SAB.ApplicationServerSession,false);  // TT#1042 - MD - Jellis - Qty Allocated Cannot Be Negative part 2 (do NOT allow any member of the assortment master list to update subtotals until put to Allocation List!!!!)
            }
            return apl;
        }
        /// <summary>
        /// Creates Assortment Member Profile Master from the selected Header RIDs (reads Database)
        /// </summary>
        /// <param name="aAssortmentRIDList">RIDS of the Assortments (excluding Header RIDs)</param>
        /// <param name="aSelectedHeaderRIDs">RIDs of the headers to include in the list (excluding Assortment RIDs)</param>
        /// <returns>Assortment AllocationProfileList continaing the selected headers</returns>
        public AllocationProfileList CreateMasterAssortmentMemberListFromSelectedHeaders(int[] aAssortmentRIDList, int[] aSelectedHeaderRIDs)
        {
            AllocationProfileList apl = NewAssortmentMemberMasterProfileList();
            if (aSelectedHeaderRIDs.Length > 0
                || aAssortmentRIDList.Length > 0)
            {
                apl.LoadHeaders(this, aAssortmentRIDList, aSelectedHeaderRIDs, SAB.ApplicationServerSession, false);  // TT#1042 - MD - Jellis - Qty Allocated Cannot Be Negative part 2 (do NOT allow any member of the assortment master list to update subtotals Until put in Allocation List!!!!)
            }
            return apl;
        }
        // end TT#488 - MD - Jellis - Group Allocation

		/// <summary>
		/// Create a new allocation profile list on the application server
		/// and set it to the allocation master profile list
		/// </summary>
		public void NewAssortmentMasterMasterProfileList()
		{
			AllocationProfileList allocationList = new AllocationProfileList(eProfileType.AssortmentMember);
			SetMasterProfileList(allocationList);
		}
		#endregion
		// END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member

		// begin MID Track 5067 Release Action Fails yet header Released
		#region GetChangedHeaderKeys
		public void SetChangedHeader(int aHeaderRID)
		{
			if (_headersChangedHash == null)
			{
				_headersChangedHash = new Hashtable();
			}
			try
			{
				if (!_headersChangedHash.ContainsKey(aHeaderRID))
				{
					_headersChangedHash.Add(aHeaderRID, aHeaderRID);
				}
			}
			catch (System.ArgumentException)
			{
				// aHeaderRID is already in the changed list
			}
		}
		/// <summary>
		/// Gets the keys of any header potentially changed by last action or method
		/// </summary>
		public int[] GetChangedHeaderKeys()
		{
			if (_headersChangedHash == null
				|| _headersChangedHash.Count == 0)
			{
				return GetAllocationProfileKeys();
			}
			AllocationProfileList apl = GetAllocationProfileList();
			ArrayList al = new ArrayList();
			foreach (int key in _headersChangedHash.Keys)
			{
                al.Add(key);
			}
			foreach (Profile ap in apl)
			{
				if (!_headersChangedHash.ContainsKey(ap.Key))
				{
					al.Add(ap.Key);
				}
			}
			int[] intKeys = new int[al.Count];
			al.CopyTo(intKeys);
			return intKeys;
		}
		#endregion GetChangedHeaderKeys
		// end MID Track 5067 Release Action Fails yet header Released
		
		#region GetAllocationProfile
		/// <summary>
		/// Gets the AllocationProfile for the specified header RID.
		/// </summary>
		/// <param name="aHeaderRID">RID of the header to retrieve.</param>
		/// <returns>AllocationProfile</returns>
		public AllocationProfile GetAllocationProfile(int aHeaderRID)
		{
			AllocationProfileList apl = GetAllocationProfileList();
			return (AllocationProfile)apl.FindKey(aHeaderRID);
		}
		/// <summary>
		/// Gets the keys of the AllocationProfiles associated with this transaction.
		/// </summary>
		/// <returns>Array of allocation profile keys</returns>
		public int[] GetAllocationProfileKeys()
		{
			AllocationProfileList apl = GetAllocationProfileList();
			if (apl == null)
			{
				return null;
			}
			int [] key = new int[apl.Count];
			for (int i=0; i<apl.Count; i++)
			{
				key [i] = ((AllocationProfile)apl[i]).Key;
			}
			return key;
		}
		/// <summary>
		/// Gets the AllocationProfileList associated with this transaction.
		/// </summary>
		/// <returns>AllocationProfileList associated with this transaction.</returns>
		public AllocationProfileList GetAllocationProfileList()
		{
			return (AllocationProfileList)this.GetMasterProfileList(eProfileType.Allocation);
		}
		#endregion GetAllocationProfile

		#region AddAllocationProfiles
		/// <summary>
		/// Adds an allocation profile to the transaction tracked allocations
		/// </summary>
		/// <param name="aAllocationProfile">Allocation Profile to add</param>
		public void AddAllocationProfile(AllocationProfile aAllocationProfile)
		{
			AllocationProfileList allocationList 
				= (AllocationProfileList)GetMasterProfileList(eProfileType.Allocation);
			if (allocationList == null)
			{
                // begin TT#488 - MD - Jellis - Group Allocation
                allocationList = NewAllocationMasterProfileList();
                //allocationList = new AllocationProfileList(eProfileType.Allocation);
                //SetMasterProfileList(allocationList);
                // TT#488 - MD - Jellis - Group Allocation
			}
			allocationList.Add(aAllocationProfile);
			if (_storeRID_IndexXref != null)
			{
				if (!aAllocationProfile.StoresLoaded)
				{
					aAllocationProfile.LoadStores();
				}
			}
		}

		/// <summary>
		/// Adds Allocation Profiles to the transcation tracked allocations
		/// </summary>
		/// <param name="aAllocationList">AllocationProfileList of AllocationProfiles to add.</param>
		public void AddAllocationProfile(AllocationProfileList aAllocationList)
		{
			AddAllocationProfile(aAllocationList.ArrayList);
		}

		/// <summary>
		/// Adds Allocation Profiles to the transaction tracked allocations
		/// </summary>
		/// <param name="aAllocationList">ICollection of Allocation Profiles to add</param>
		public void AddAllocationProfile(System.Collections.ICollection aAllocationList)
		{
			foreach (AllocationProfile ap in aAllocationList)
			{
				AddAllocationProfile(ap);
			}
		}
		/// <summary>
		/// Adds Allocation Profiles to the transaction tracked allocations
		/// </summary>
		/// <param name="aAllocationHeaderList">Profile List of AllocationHeaderProfiles to add</param>
        // Begin TT#2013-MD - JSmith - Process Allocate Task List - the Header attached to an Asst receives a Null Reference Exception
        //public void AddAllocationProfile(ProfileList aAllocationHeaderList)
        //{
        //    AllocationProfile ap;
        //    foreach (AllocationHeaderProfile ahp in aAllocationHeaderList)
        //    {
        //        ap = new AllocationProfile(this, ahp.HeaderID, ahp.Key, this.SAB.ApplicationServerSession);
        //        AddAllocationProfile(ap);
        //    }
        //}
        public bool AddAllocationProfile(ProfileList aAllocationHeaderList, bool bIncludeAssortmentAndGroupAllocation, bool bIncludeMultiHeaders, bool bIncludeReleaseHeaders)
        {
            AllocationProfile ap;
            bool headerAdded = false;
            foreach (AllocationHeaderProfile ahp in aAllocationHeaderList)
            {
                ap = new AllocationProfile(this, ahp.HeaderID, ahp.Key, this.SAB.ApplicationServerSession);
                if (!bIncludeAssortmentAndGroupAllocation
                    && ap.AsrtRID != Include.NoRID
                    )
                {
                    continue;
                }
                else if (!bIncludeMultiHeaders
                    && ap.InUseByMulti
                    )
                {
                    continue;
                }
                else if (!bIncludeReleaseHeaders
                    && ap.Released
                    )
                {
                    continue;
                }
                AddAllocationProfile(ap);
                headerAdded = true;
            }

            return headerAdded;
        }
        // End TT#2013-MD - JSmith - Process Allocate Task List - the Header attached to an Asst receives a Null Reference Exception
		#endregion AddAllocationProfiles
		
		#region RemoveAllocationProfiles
		/// <summary>
		/// Removes an allocation profile from the transaction tracked allocations
		/// </summary>
		/// <param name="aAllocationProfile">Allocation Profile to Remove</param>
		public void RemoveAllocationProfile(AllocationProfile aAllocationProfile)
		{
			AllocationProfileList allocationList 
				= (AllocationProfileList)GetMasterProfileList(eProfileType.Allocation);
			if (allocationList != null)
			{
				allocationList.Remove(aAllocationProfile);
			}
		}

		/// <summary>
		/// Removes Allocation Profiles from the transcation tracked allocations
		/// </summary>
		/// <param name="aAllocationList">AllocationProfileList of AllocationProfiles to Remove.</param>
		public void RemoveAllocationProfile(AllocationProfileList aAllocationList)
		{
			RemoveAllocationProfile(aAllocationList.ArrayList);
		}

		/// <summary>
		/// Removes Allocation Profiles from the transaction tracked allocations
		/// </summary>
		/// <param name="aAllocationList">ICollection of Allocation Profiles to Remove</param>
		public void RemoveAllocationProfile(System.Collections.ICollection aAllocationList)
		{
			foreach (AllocationProfile ap in aAllocationList)
			{
				RemoveAllocationProfile(ap);
			}
		}
		#endregion RemoveAllocationProfiles

		// BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
		#region NewAssortmentMemberMasterProfileList

		/// <summary>
		/// Create a new assortment member profile list on the application server
		/// and set it to the allocation master profile list
		/// </summary>
		public AllocationProfileList NewAssortmentMemberMasterProfileList() // TT#488 - MD - Jellis - Group Allocation
		{
			AllocationProfileList allocationList = new AllocationProfileList(eProfileType.AssortmentMember);
			SetMasterProfileList(allocationList);
            return allocationList;  // TT#488 - MD - Jellis - Group Allocation
		}
		#endregion

		#region GetAssortmentMemberProfile
        // Begin TT#2063-MD - JSmith - In an Asst close style review with the X in the top right corner.  Receieve mssg about Group allocation and values changing.  The only thing changed was the View.  Would not expect the mssg.
		/// <summary>
		/// Gets the AllocationProfile for the AssortmentProfile.
		/// </summary>
		/// <returns>AllocationProfile</returns>
		public AllocationProfile GetAssortmentProfile()
		{
            AllocationProfile ap = null;
			AllocationProfileList apl = GetAssortmentMemberProfileList();
			if (apl != null)
            {
                foreach (Profile p in apl)
                {
                    if (p is AssortmentProfile)
                    {
                        ap = (AllocationProfile)p;
                        break;
                    }
                }
            }
            return ap;
		}
        // End TT#2063-MD - JSmith - In an Asst close style review with the X in the top right corner.  Receieve mssg about Group allocation and values changing.  The only thing changed was the View.  Would not expect the mssg.

		/// <summary>
		/// Gets the AllocationProfile for the specified header RID.
		/// </summary>
		/// <param name="aHeaderRID">RID of the header to retrieve.</param>
		/// <returns>AllocationProfile</returns>
		public AllocationProfile GetAssortmentMemberProfile(int aHeaderRID)
		{
            AllocationProfile ap = null;	// TT#1752-MD - stodd - GA - Open up application select Allocation -> Review -> Select-> Null Ref error message (GA only) 
			AllocationProfileList apl = GetAssortmentMemberProfileList();
			// Begin TT#1752-MD - stodd - GA - Open up application select Allocation -> Review -> Select-> Null Ref error message (GA only) 
            if (apl != null)
            {
                //if (aHeaderRID == Include.NoRID)
                //{
                //    ap = (AllocationProfile)apl[0];
                //    return ap;
                //}
                ap = (AllocationProfile)apl.FindKey(aHeaderRID);
            }
            return ap;
			// End TT#1752-MD - stodd - GA - Open up application select Allocation -> Review -> Select-> Null Ref error message (GA only) 
		}
		/// <summary>
		/// Gets the keys of the Assortment AllocationProfiles associated with this transaction.
		/// </summary>
		/// <returns>Array of Assortment allocation profile keys</returns>
		public int[] GetAssortmentMemberProfileKeys()
		{
			AllocationProfileList apl = GetAssortmentMemberProfileList();
			if (apl == null)
			{
				return null;
			}
			int[] key = new int[apl.Count];
			for (int i = 0; i < apl.Count; i++)
			{
				key[i] = ((AllocationProfile)apl[i]).Key;
			}
			return key;
		}

        // Begin TT#2101-MD - JSmith - Matrix values change when beg and Del dates changed without REDO being selected.
		public AssortmentProfile GetAssortmentProfileFromList()
        {
            AllocationProfileList apl = (AllocationProfileList)this.GetMasterProfileList(eProfileType.AssortmentMember);
            if (apl != null)
            {
                foreach (object asrtObj in apl)
                {
                    if (asrtObj is AssortmentProfile)
                    {
                        return (AssortmentProfile)asrtObj;
                    }
                }
            }

            return null;
        }
		// End TT#2101-MD - JSmith - Matrix values change when beg and Del dates changed without REDO being selected.

		/// <summary>
		/// Gets the Assortment AllocationProfileList associated with this transaction.
		/// </summary>
		/// <returns>Assortment AllocationProfileList associated with this transaction.</returns>
		public AllocationProfileList GetAssortmentMemberProfileList()
		{
			// Begin TT#964 - MD - stodd - VSW null ref exception - 
            AllocationProfileList apl = (AllocationProfileList)this.GetMasterProfileList(eProfileType.AssortmentMember);
            // begin TT#918 - MD - Jellis - Group Allocation errors prevent actions
            if (apl == null)
            {
                return (AllocationProfileList)this.GetMasterProfileList(eProfileType.Allocation);
            }
            else
            {
                return apl;
            }
            // end TT#918 - MD - Jellis - Group Allocation errors prevent actions
			//return (AllocationProfileList)this.GetMasterProfileList(eProfileType.AssortmentMember);
			// End TT#964 - MD - stodd - VSW null ref exception - 
		}
		#endregion GetAssortmentMemberProfile

		#region AddAssortmentMemberProfiles
		/// <summary>
		/// Adds an allocation profile to the transaction tracked allocations
		/// </summary>
		/// <param name="aAllocationProfile">Allocation Profile to add</param>
		public void AddAssortmentMemberProfile(AllocationProfile aAllocationProfile)
		{
			AllocationProfileList allocationList
				= (AllocationProfileList)GetMasterProfileList(eProfileType.AssortmentMember);
			if (allocationList == null)
			{
                // begin TT#488 - MD - Jellis - Group Allocation
                allocationList = NewAssortmentMemberMasterProfileList();
                //allocationList = new AllocationProfileList(eProfileType.AssortmentMember);
                //SetMasterProfileList(allocationList);
                // end TT#488 - MD - Jellis - Group Allocation
			}
			allocationList.Add(aAllocationProfile);
			if (_storeRID_IndexXref != null)
			{
				if (!aAllocationProfile.StoresLoaded)
				{
					aAllocationProfile.LoadStores(false);
				}
			}
		}

		/// <summary>
		/// Adds Allocation Profiles to the transcation tracked allocations
		/// </summary>
		/// <param name="aAllocationList">AllocationProfileList of AllocationProfiles to add.</param>
		public void AddAssortmentMemberProfile(AllocationProfileList aAllocationList)
		{
			AddAssortmentMemberProfile(aAllocationList.ArrayList);
		}

		/// <summary>
		/// Adds Allocation Profiles to the transaction tracked allocations
		/// </summary>
		/// <param name="aAllocationList">ICollection of Allocation Profiles to add</param>
		public void AddAssortmentMemberProfile(System.Collections.ICollection aAllocationList)
		{
			foreach (AllocationProfile ap in aAllocationList)
			{
				AddAssortmentMemberProfile(ap);
			}
		}
		/// <summary>
		/// Adds Allocation Profiles to the transaction tracked allocations
		/// </summary>
		/// <param name="aAllocationHeaderList">Profile List of AllocationHeaderProfiles to add</param>
		public void AddAssortmentMemberProfile(ProfileList aAllocationHeaderList)
		{
			AllocationProfile ap;
			foreach (AllocationHeaderProfile ahp in aAllocationHeaderList)
			{
				ap = new AllocationProfile(this, ahp.HeaderID, ahp.Key, this.SAB.ApplicationServerSession);
				AddAssortmentMemberProfile(ap);
			}
		}
		#endregion AddAssortmentMemberProfiles

		#region RemoveAssortmentMemberProfiles
		/// <summary>
		/// Removes an allocation profile from the transaction tracked allocations
		/// </summary>
		/// <param name="aAllocationProfile">Allocation Profile to Remove</param>
		public void RemoveAssortmentMemberProfile(AllocationProfile aAllocationProfile)
		{
			AllocationProfileList allocationList
				= (AllocationProfileList)GetMasterProfileList(eProfileType.AssortmentMember);
			if (allocationList != null)
			{
				allocationList.Remove(aAllocationProfile);
			}
		}

		/// <summary>
		/// Removes Allocation Profiles from the transcation tracked allocations
		/// </summary>
		/// <param name="aAllocationList">AllocationProfileList of AllocationProfiles to Remove.</param>
		public void RemoveAssortmentMemberProfile(AllocationProfileList aAllocationList)
		{
			RemoveAssortmentMemberProfile(aAllocationList.ArrayList);
		}

		/// <summary>
		/// Removes Allocation Profiles from the transaction tracked allocations
		/// </summary>
		/// <param name="aAllocationList">ICollection of Allocation Profiles to Remove</param>
		public void RemoveAssortmentMemberProfile(System.Collections.ICollection aAllocationList)
		{
			foreach (AllocationProfile ap in aAllocationList)
			{
				RemoveAssortmentMemberProfile(ap);
			}
		}
		#endregion RemoveAssortmentMemberProfiles
		// END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member

        // Begin TT#1966-MD - JSmith - DC Fulfillment
        #region MasterHeader
        public MasterHeaderProfile GetMasterHeaderProfile(int iKey)
        {
            if (_masterHeader == null
                || _masterHeader.Key != iKey)
            {
                _masterHeader = new MasterHeaderProfile(this, null, iKey, this.SAB.ApplicationServerSession);
            }
            return _masterHeader;  
        }

        /// <summary>
        /// Create a new master header subordinate profile list on the application server
        /// and set it to the allocation master profile list
        /// </summary>
        public AllocationProfileList NewMasterSubordinateProfileList(MasterHeaderProfile masterHeader) 
        {
            AllocationProfileList subordinateList = new AllocationProfileList(eProfileType.MasterHeaderSubordinates);
            _masterSubordinates.Add(masterHeader.Key, subordinateList);
            return subordinateList;  
        }

        public AllocationProfileList GetSubordinates(AllocationProfile currentHeader, bool bReplaceSubordinate = false)
        {
            AllocationProfileList subordinates = null;
            MasterHeaderProfile masterHeader = null;

            if (currentHeader.IsMasterHeader)
            {
                masterHeader = currentHeader as MasterHeaderProfile;
            }
            else if (currentHeader.IsSubordinateHeader)
            {
                masterHeader = currentHeader.MasterHeader;
            }

            if (masterHeader == null)
            {
                return null;
            }

            if (!_masterSubordinates.TryGetValue(masterHeader.Key, out subordinates))
            {
                subordinates = NewMasterSubordinateProfileList(masterHeader);
                if (currentHeader != null)
                {
                    bReplaceSubordinate = true;
                }

                if (currentHeader.IsMasterHeader)
                {
                    subordinates.LoadHeaders(this, null, currentHeader.SubordinateRIDs.ToArray(), SAB.ApplicationServerSession);
                }
                else if (currentHeader.IsSubordinateHeader)
                {
                    subordinates.LoadHeaders(this, null, currentHeader.MasterHeader.SubordinateRIDs.ToArray(), SAB.ApplicationServerSession);
                }
                // Build Assortment members if master is in a group or assortment
                if (masterHeader.AsrtRID != Include.NoRID)
                {
                    AllocationProfileList apl = (AllocationProfileList)this.GetMasterProfileList(eProfileType.AssortmentMember);
                    if (apl == null)
                    {
                        BuildAssortmentMemberList(masterHeader.Key, masterHeader.AsrtRID);
                    }
                }
            }

            if (bReplaceSubordinate)
            {
                // put the current header in the list so will have the correct reference
                AllocationProfileList subordinatesWork = new AllocationProfileList(eProfileType.MasterHeaderSubordinates);
                
                foreach (AllocationProfile subordinate in subordinates)
                {
                    if (currentHeader != null
                        && currentHeader.Key == subordinate.Key)
                    {
                        subordinatesWork.Add(currentHeader);
                    }
                    else
                    {
                        subordinatesWork.Add(subordinate);
                    }
                }
                subordinates = subordinatesWork;
                _masterSubordinates[masterHeader.Key] = subordinates;
            }

            return subordinates;
        }

        public bool BuildAssortmentMemberList(int aHeaderRID, int aAsrtRID)
        {
            ArrayList selectedAssortmentKeyList = new ArrayList();
            ArrayList selectedHeaderKeyList = new ArrayList();
            ArrayList selectedHeaderRIDs = new ArrayList();
            ArrayList selectedAssortmentRIDs = new ArrayList();
                        
            selectedHeaderKeyList.Add(aHeaderRID);

            GetAllHeadersInAssortment(aAsrtRID, ref selectedAssortmentKeyList, ref selectedHeaderKeyList);

            foreach (int rid in selectedHeaderKeyList)
            {
                if (!selectedHeaderRIDs.Contains(rid))
                {
                    selectedHeaderRIDs.Add(rid);
                }
            }

            foreach (int rid in selectedAssortmentKeyList)
            {
                if (!selectedAssortmentRIDs.Contains(rid))
                {
                    selectedAssortmentRIDs.Add(rid);
                }
            }

            int[] selectedHeaderArray = new int[selectedHeaderRIDs.Count];
            int[] selectedAssortmentArray = new int[selectedAssortmentRIDs.Count];
            selectedHeaderRIDs.CopyTo(selectedHeaderArray);
            selectedAssortmentRIDs.CopyTo(selectedAssortmentArray);

            CreateMasterAssortmentMemberListFromSelectedHeaders(selectedAssortmentArray, selectedHeaderArray);
            
            return true;
        }

        private void GetAllHeadersInAssortment(int aAsrtRID, ref ArrayList selectedAssortmentKeyList, ref ArrayList selectedHeaderKeyList)
        {
            try
            {
                ArrayList al = SAB.HeaderServerSession.GetHeadersInAssortment(aAsrtRID);
                for (int i = 0; i < al.Count; i++)
                {
                    int hdrRID = (int)al[i];
                    if (hdrRID == aAsrtRID)
                    {
                        if (!selectedAssortmentKeyList.Contains(hdrRID))
                        {
                            selectedAssortmentKeyList.Add(hdrRID);
                        }
                    }
                    else
                    {
                        if (!selectedHeaderKeyList.Contains(hdrRID))
                        {
                            selectedHeaderKeyList.Add(hdrRID);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        
        public bool CheckInstance (int Key, long instanceID)
        {
            long currentInstanceID = -1;
            if (_instanceTracker.TryGetValue(Key, out currentInstanceID))
            {
                if (instanceID !=currentInstanceID )
                {
                    throw new MIDException(eErrorLevel.fatal,
                        999999,
                        "Instance ID does not match");
                }
            }
            else
            {
                _instanceTracker.Add(Key, instanceID);
            }
            return true;
        }

        public void ClearInstanceTracker()
        {
            _instanceTracker.Clear();
        }

        #endregion MasterHeader
        // End TT#1966-MD - JSmith - DC Fulfillment

		#region RuleLayerID
		public int GetRuleLayerID (int aHeaderRID, GeneralComponent aComponent)
		{
			int ruleLayerID;
			if (this._ruleLayerHash == null)
			{
				this._ruleLayerHash = new Hashtable();
			}
			Hashtable componentLayerHash;
			if (this._ruleLayerHash.Contains(aHeaderRID))
			{
				componentLayerHash = (Hashtable)this._ruleLayerHash[aHeaderRID];
			}
			else
			{
				componentLayerHash = new Hashtable();
				_ruleLayerHash.Add(aHeaderRID, componentLayerHash);
			}
			Hashtable layerIDHash;
			switch (aComponent.ComponentType)
			{
				case (eComponentType.Total):
				case (eComponentType.Bulk):
				case (eComponentType.DetailType):
				{
					if (componentLayerHash.Contains((int)aComponent.ComponentType))
					{
						//ruleLayerID = (int)componentLayerHash[(int)aComponent.ComponentType] + 1;  // MID Change j.ellis Allow soft rule in interactive Velocity
						//componentLayerHash.Remove((int)aComponent.ComponentType);                  // MID Change j.ellis Allow soft rule in interactive Velocity
						ruleLayerID = (int)componentLayerHash[(int)aComponent.ComponentType];        // MID Change j.ellis Allow soft rule in interactive Velocity 
					}
					else
					{
						ruleLayerID = GetNextLayerID(aHeaderRID, aComponent.ComponentType, Include.NoRID);
						componentLayerHash.Add((int)aComponent.ComponentType, ruleLayerID);	// MID Change j.ellis Allow soft rule in interactive Velocity			
					}
					//					componentLayerHash.Add((int)aComponent.ComponentType, ruleLayerID); // MID Change j.ellis Allow soft rule in interactive Velocity
					break;
				}
				case (eComponentType.SpecificColor):
				{
					AllocationColorOrSizeComponent color = (AllocationColorOrSizeComponent)aComponent;
					if (componentLayerHash.Contains((int)aComponent.ComponentType))
					{
						layerIDHash = (Hashtable)componentLayerHash[(int)aComponent.ComponentType];
					}
					else
					{
						layerIDHash = new Hashtable();
						componentLayerHash.Add((int)aComponent.ComponentType, layerIDHash);
					}
					if (layerIDHash.Contains(color.ColorRID))
					{
						ruleLayerID = (int)layerIDHash[color.ColorRID];
						//						layerIDHash.Remove(color.ColorRID);   // MID Change j.ellis Allow soft rule in interactive Velocity
					}
					else
					{
						ruleLayerID = GetNextLayerID(aHeaderRID, aComponent.ComponentType, color.ColorRID);
						// BEGIN MID Track 3180 duplicate key in hashtable
						//componentLayerHash.Add((int)aComponent.ComponentType, ruleLayerID);   // MID Change j.ellis Allow soft rule in interactive Velocity
						layerIDHash.Add(color.ColorRID, ruleLayerID);
						// END MID Track 3180
					}
					//					layerIDHash.Add(color.ColorRID, ruleLayerID); // MID Change j.ellis Allow soft rule in interactive Velocity
					break;
				}
				case (eComponentType.SpecificPack):
				{
					AllocationPackComponent pack = (AllocationPackComponent)aComponent;
					if (componentLayerHash.Contains((int)aComponent.ComponentType))
					{
						layerIDHash = (Hashtable)componentLayerHash[(int)aComponent.ComponentType];
					}
					else
					{
						layerIDHash = new Hashtable();
						componentLayerHash.Add((int)aComponent.ComponentType, layerIDHash);
					}
					if (layerIDHash.Contains(pack.PackName))
					{
						ruleLayerID = (int)layerIDHash[pack.PackName];
						//layerIDHash.Remove(pack.PackName);  // MID Change j.ellis Allow soft rule in interactive Velocity
					}
					else
					{
						ruleLayerID = GetNextLayerID(aHeaderRID, pack.PackName);
						layerIDHash.Add(pack.PackName, ruleLayerID);   // MID Change j.ellis Allow soft rule in interactive Velocity
					}
					//					layerIDHash.Add(pack.PackName, ruleLayerID);    // MID Change j.ellis Allow soft rule in interactive Velocity
					break;				
				}
				case (eComponentType.AllColors):
				case (eComponentType.AllGenericPacks):
				case (eComponentType.AllNonGenericPacks):
				case (eComponentType.AllPacks):
				case (eComponentType.AllSizes):
				case (eComponentType.ColorAndSize):
				case (eComponentType.GenericType):
				case (eComponentType.SpecificSize):
				{
					throw new MIDException (eErrorLevel.fatal,
						(int)eMIDTextCode.msg_al_ComponentMustBeSpecific,
						MIDText.GetText(eMIDTextCode.msg_al_ComponentMustBeSpecific));
				}
				default:
				{
					throw new MIDException(eErrorLevel.fatal,
						(int)eMIDTextCode.msg_al_UnknownComponentType,
						MIDText.GetText(eMIDTextCode.msg_al_UnknownComponentType));
				}
			}
			return ruleLayerID;
		}

		private int GetNextLayerID(int aHeaderRID, string aPackName)
		{
			RuleAllocation ra = new RuleAllocation();
			return ra.GetLayerID(aHeaderRID, aPackName);
		}
		private int GetNextLayerID(int aHeaderRID, eComponentType aComponentType, int aComponentRID)
		{
			RuleAllocation ra = new RuleAllocation();
			return ra.GetLayerID(aHeaderRID, aComponentType, aComponentRID);
		}

		// BEGIN MID Change j.ellis Allow "Soft" Rule in interactive Velocity
		/// <summary>
		/// Increments the layer ID on a save
		/// </summary>
		/// <param name="aHeaderRID">Header RID</param>
		/// <param name="aComponent">Component</param>
		/// <param name="aLayerID">Layer being saved</param>
		internal void UpdateLayerID(int aHeaderRID, GeneralComponent aComponent, int aLayerID)
		{
			if (this._ruleLayerHash == null)
			{
				return;
			}
			Hashtable componentLayerHash;
			if (this._ruleLayerHash.Contains(aHeaderRID))
			{
				componentLayerHash = (Hashtable)this._ruleLayerHash[aHeaderRID];
			}
			else
			{
				return;
			}
			Hashtable layerIDHash;
			switch (aComponent.ComponentType)
			{
				case (eComponentType.Total):
				case (eComponentType.Bulk):
				case (eComponentType.DetailType):
				{
					if (componentLayerHash.Contains((int)aComponent.ComponentType))
					{
						componentLayerHash.Remove((int)aComponent.ComponentType);
						componentLayerHash.Add((int)aComponent.ComponentType, aLayerID + 1);
					}
					break;
				}
				case (eComponentType.SpecificColor):
				{
					AllocationColorOrSizeComponent color = (AllocationColorOrSizeComponent)aComponent;
					if (componentLayerHash.Contains((int)aComponent.ComponentType))
					{
						layerIDHash = (Hashtable)componentLayerHash[(int)aComponent.ComponentType];
						if (layerIDHash.Contains(color.ColorRID))
						{
							layerIDHash.Remove(color.ColorRID);
							layerIDHash.Add(color.ColorRID, aLayerID + 1);
						}
					}
					break;
				}
				case (eComponentType.SpecificPack):
				{
					AllocationPackComponent pack = (AllocationPackComponent)aComponent;
					if (componentLayerHash.Contains((int)aComponent.ComponentType))
					{
						layerIDHash = (Hashtable)componentLayerHash[(int)aComponent.ComponentType];
						if (layerIDHash.Contains(pack.PackName))
						{
							layerIDHash.Remove(pack.PackName);
							layerIDHash.Add(pack.PackName, aLayerID + 1);
						}
					}
					break;				
				}
				case (eComponentType.AllColors):
				case (eComponentType.AllGenericPacks):
				case (eComponentType.AllNonGenericPacks):
				case (eComponentType.AllPacks):
				case (eComponentType.AllSizes):
				case (eComponentType.ColorAndSize):
				case (eComponentType.GenericType):
				case (eComponentType.SpecificSize):
				{
					throw new MIDException (eErrorLevel.fatal,
						(int)eMIDTextCode.msg_al_ComponentMustBeSpecific,
						MIDText.GetText(eMIDTextCode.msg_al_ComponentMustBeSpecific));
				}
				default:
				{
					throw new MIDException(eErrorLevel.fatal,
						(int)eMIDTextCode.msg_al_UnknownComponentType,
						MIDText.GetText(eMIDTextCode.msg_al_UnknownComponentType));
				}
			}
			return;
		}
		// BEGIN MID Change j.ellis Allow "Soft" Rule in interactive Velocity
		#endregion RuleLayerID

		#region Allocation Audits
		/// <summary>
		/// Gets an instance of the Application Server Session Audit.
		/// </summary>
		/// <returns>An instance of the Application Server Session Audit</returns>
		private Audit GetApplicationServerSessionAudit()
		{
			if (_audit == null)
			{
				_audit = this.SAB.ApplicationServerSession.Audit;
			}
			return _audit;						   
		}
		/// <summary>
		/// Gets an instance of AuditData
		/// </summary>
		/// <returns>An instance of AuditData</returns>
		private AuditData GetAuditData()
		{
			if (_auditData == null)
			{
				_auditData = new AuditData();
			}
			return _auditData;
		}
		/// <summary>
		/// Gets the Process RID associated with this transaction
		/// </summary>
		/// <returns>Process RID</returns>
		private int GetProcessRID()
		{
			if (_processRID == Include.NoRID)
			{
				_processRID = this.GetApplicationServerSessionAudit().ProcessRID;
			}
			return _processRID;
		}

        // begin TT#1185 - JEllis - Verify ENQ before Update (part 2) - remove duplicated processes
        ///// <summary>
        ///// Gets the User RID associated with this transaction
        ///// </summary>
        ///// <returns>User RID</returns>
        //private int GetUserRID()
        //{
        //    if (_userRID == Include.NoRID)
        //    {
        //        _userRID = SAB.ClientServerSession.UserRID;
        //    }
        //    return _userRID;
        //}
        // end TT#1185 - JEllis - Verify ENQ before Update (part 2) 

		/// <summary>
		/// Writes an audit row to the Audit_Header table
		/// </summary>
		/// <param name="aHdrRID">Header RID</param>
		/// <param name="aMethodRID">Method RID</param>
		/// <param name="aMethodName">Method name</param>
		/// <param name="aAllocationActionType">Action Type</param>
		/// <param name="aMethodType">Method Type</param>
		/// <param name="aComponentType">Component Type</param>
		/// <param name="aPackOrColorComponentName">Pack Name if component is a pack; Color Name if component is color or size</param>
		/// <param name="aSizeComponentName">Size Name if component is a size</param>
		/// <param name="aUnitsAllocatedByProcess">Total units allocated by the action or method</param>
		/// <param name="aStoreAffectedByProcessCount">Number of stores affected by the action or method</param>
		public void WriteAllocationAuditInfo(
			int aHdrRID,
			eAllocationActionType aAllocationActionType,
			eMethodType aMethodType,
			int aMethodRID,
			string aMethodName,
			eComponentType aComponentType,
			string aPackOrColorComponentName,
			string aSizeComponentName,      // MID Track 4448 AnF Audit Enhancement 
			int aUnitsAllocatedByProcess,   // MID Track 4448 AnF Audit Enhancement
			int aStoreAffectedByProcessCount) // MID Track 4448 AnF Audit Enhancement
		{
			AuditData auditData = this.GetAuditData();
			try
			{
				auditData.OpenUpdateConnection();
                //Begin TT#571-MD -jsobek -Allocation Audit needs to keep iterations of allocation
				switch (aAllocationActionType)
				{
					case eAllocationActionType.BackoutAllocation:
					{
                            auditData.AllocationAuditHeader_ClearLastEntryByHdrComponent
							(aHdrRID,
							aComponentType,
							aPackOrColorComponentName,
							aSizeComponentName);
                            int auditRID = auditData.AllocationAuditHeader_Add
                            (DateTime.Now,
                            this.GetProcessRID(),
                                //this.GetUserRID(),   // TT#1185 - JEllis - Verify ENQ before Update (part 2) - remove duplicate processes
                            UserRID,               // TT#1185 - JEllis - Verify ENQ before Update (part 2) - remove duplicate processes
                            aHdrRID,
                            aAllocationActionType,
                            aMethodType,
                            aMethodRID,
                            aMethodName,
                            aComponentType,
                            aPackOrColorComponentName,
                            aSizeComponentName,              // MID Track 4448 AnF Audit Enhancement 
                            aUnitsAllocatedByProcess,        // MID Track 4448 AnF Audit Enhancement
                            aStoreAffectedByProcessCount);   // MID Track 4448 AnF Audit Enhancement
                        break;
					}
					case eAllocationActionType.BackoutSizeAllocation:
                        {
                            auditData.AllocationAuditHeader_ClearLastEntryByHdrSizeActions
                                (aHdrRID,
                                aComponentType,
                                aPackOrColorComponentName,
                                aSizeComponentName);
                            int auditRID = auditData.AllocationAuditHeader_Add
                            (DateTime.Now,
                            this.GetProcessRID(),
                                //this.GetUserRID(),   // TT#1185 - JEllis - Verify ENQ before Update (part 2) - remove duplicate processes
                            UserRID,               // TT#1185 - JEllis - Verify ENQ before Update (part 2) - remove duplicate processes
                            aHdrRID,
                            aAllocationActionType,
                            aMethodType,
                            aMethodRID,
                            aMethodName,
                            aComponentType,
                            aPackOrColorComponentName,
                            aSizeComponentName,              // MID Track 4448 AnF Audit Enhancement 
                            aUnitsAllocatedByProcess,        // MID Track 4448 AnF Audit Enhancement
                            aStoreAffectedByProcessCount);   // MID Track 4448 AnF Audit Enhancement
                            break;
                        }
					case eAllocationActionType.BackoutDetailPackAllocation:
					{
                            auditData.AllocationAuditHeader_ClearLastEntryByHdrSizeActions
							(aHdrRID,
						    aComponentType,
						   	aPackOrColorComponentName,
							aSizeComponentName);
						break;
					}
					default:
					{
						int auditRID = auditData.AllocationAuditHeader_Add
							(DateTime.Now,
							this.GetProcessRID(),
							//this.GetUserRID(),   // TT#1185 - JEllis - Verify ENQ before Update (part 2) - remove duplicate processes
                            UserRID,               // TT#1185 - JEllis - Verify ENQ before Update (part 2) - remove duplicate processes
							aHdrRID,
							aAllocationActionType,
							aMethodType,
							aMethodRID,
							aMethodName,
							aComponentType,
							aPackOrColorComponentName,
							aSizeComponentName,              // MID Track 4448 AnF Audit Enhancement 
							aUnitsAllocatedByProcess,        // MID Track 4448 AnF Audit Enhancement
							aStoreAffectedByProcessCount);   // MID Track 4448 AnF Audit Enhancement
						break;
					}
				}
                //End TT#571-MD -jsobek -Allocation Audit needs to keep iterations of allocation
				auditData.CommitData();
			}
			catch
			{
				throw;
			}
			finally
			{
				auditData.CloseUpdateConnection();
			}
		}
		#endregion Allocation Audits
		// end MID Track 4448 Abercrombie and Fitch Audit Enhancement
		
		#region AllocationActionSuccess
		/// <summary>
		/// Gets the Allocation Action Status for a given Header RID.
		/// </summary>
		/// <param name="aHeaderRID">RID of the Header</param>
		/// <returns>eAllocationActionStatus for the most recent action in this transcation</returns>
		// begin TT#241 - MD - JEllis - Header Enqueue Process
        public eAllocationActionStatus GetAllocationActionStatus (int aHeaderRID)
		{
            eAllocationActionStatus actionStatus;
			if (_allocationActionStatusDict == null)
			{
				_allocationActionStatusDict = new Dictionary<int, eAllocationActionStatus>();
			}
            else if (_allocationActionStatusDict.TryGetValue(aHeaderRID, out actionStatus))
            {
                return actionStatus;
            }
            return eAllocationActionStatus.NoActionPerformed;
        }
        //public eAllocationActionStatus GetAllocationActionStatus(int aHeaderRID)
        //{
        //    if (this._allocationActionStatusHash == null)
        //    {
        //        this._allocationActionStatusHash = new Hashtable();
        //    }
        //    if (this._allocationActionStatusHash.Contains(aHeaderRID))
        //    {
        //        return (eAllocationActionStatus)this._allocationActionStatusHash[aHeaderRID];
        //    }
        //    return eAllocationActionStatus.NoActionPerformed;
        //}
        // end TT#241 - MD JEllis - Header Enqueue Process

		/// <summary>
		/// Sets the Allocation Action Status for a given Header RID.
		/// </summary>
		/// <param name="aHeaderRID">RID of the Header</param>
		/// <param name="aAllocationActionStatus">eAllocationActionStatus of the action.</param>
		public void SetAllocationActionStatus (int aHeaderRID, eAllocationActionStatus aAllocationActionStatus)		// TT#3818 - stodd - Unnecessary popup message - 
		{
            // begin TT#241 - MD JEllis - Header Enqueue Process
            //if (this._allocationActionStatusHash == null)
            //{
            //    this._allocationActionStatusHash = new Hashtable();
            //}
            //if (this._allocationActionStatusHash.Contains(aHeaderRID))
            //{
            //    this._allocationActionStatusHash.Remove(aHeaderRID);
            //}
            //this._allocationActionStatusHash.Add(aHeaderRID, aAllocationActionStatus);
            if (_allocationActionStatusDict == null)
            {
                _allocationActionStatusDict = new Dictionary<int, eAllocationActionStatus>();
            }
            else
            {
                switch (aAllocationActionStatus)
                {
                    case (eAllocationActionStatus.HeaderEnqueueFailed):
                        {
                            break;
                        }
                    case (eAllocationActionStatus.NoHeaderResourceLocks):
                        {
                            if (GetAllocationActionStatus(aHeaderRID) == eAllocationActionStatus.HeaderEnqueueFailed)
                            {
                                return;
                            }
                            break;
                        }
                    case (eAllocationActionStatus.ActionFailed):
                        {
                            eAllocationActionStatus actionStatus = GetAllocationActionStatus(aHeaderRID);
                            if (actionStatus == eAllocationActionStatus.HeaderEnqueueFailed
                                || actionStatus == eAllocationActionStatus.NoHeaderResourceLocks             // TT#2671 - Jellis - Release Action Fails yet headers released  
                                || actionStatus == eAllocationActionStatus.NotAllLinkedHeadersRlseApproved) // TT#2671 - Jellis - Release Action Fails yet headers released
                            {
                                return;
                            }
                            break;
                        }
                    // begin TT#2671 - Jellis - Release Action Fails yet headers released
                    case (eAllocationActionStatus.NotAllLinkedHeadersRlseApproved):
                        {
                            eAllocationActionStatus actionStatus = GetAllocationActionStatus(aHeaderRID);
                            if (actionStatus == eAllocationActionStatus.HeaderEnqueueFailed
                                || actionStatus == eAllocationActionStatus.NoHeaderResourceLocks)                                
                            {
                                return;
                            }
                            break;
                        }
                    case (eAllocationActionStatus.AllLinkedHeadersReleased):
                        {
                            eAllocationActionStatus actionStatus = GetAllocationActionStatus(aHeaderRID);
                            if (actionStatus == eAllocationActionStatus.HeaderEnqueueFailed
                                || actionStatus == eAllocationActionStatus.NoHeaderResourceLocks
                                || actionStatus == eAllocationActionStatus.VelocityBasisError
                                || actionStatus == eAllocationActionStatus.ActionFailed)
                            {
                                return;
                            }
                            if (actionStatus == eAllocationActionStatus.NotAllLinkedHeadersRlseApproved)
                            {
                                aAllocationActionStatus = eAllocationActionStatus.ActionCompletedSuccessfully;
                            }
                            break; 
                        }
                    // end TT#2671 - Jellis - Release Action Fails yet headers released
                    case (eAllocationActionStatus.VelocityBasisError):
                        {
                            eAllocationActionStatus actionStatus = GetAllocationActionStatus(aHeaderRID);
                            if (actionStatus == eAllocationActionStatus.HeaderEnqueueFailed
                                || actionStatus == eAllocationActionStatus.NoHeaderResourceLocks
                                || actionStatus == eAllocationActionStatus.NotAllLinkedHeadersRlseApproved // TT#2671 - Jellis - Release Action Fails yet headers released
                                || actionStatus == eAllocationActionStatus.ActionFailed)
                            {
                                return;
                            }
                            break;
                        }
                    case (eAllocationActionStatus.ActionCompletedSuccessfully):
                    case (eAllocationActionStatus.NoActionPerformed):
                        {
                            eAllocationActionStatus actionStatus = GetAllocationActionStatus(aHeaderRID);
                            if (actionStatus == eAllocationActionStatus.HeaderEnqueueFailed
                                || actionStatus == eAllocationActionStatus.NoHeaderResourceLocks
                                || actionStatus == eAllocationActionStatus.NotAllLinkedHeadersRlseApproved // TT#2671 - Jellis - Release Action Fails yet headers released
                                || actionStatus == eAllocationActionStatus.ActionFailed
                                || actionStatus == eAllocationActionStatus.VelocityBasisError)
                            {
                                return;
                            }
                            break;
                        }
                    default:
                        {
                            throw new Exception("Unknown eAllocationActionStatus: " + aAllocationActionStatus.ToString());
                        }
                }
                _allocationActionStatusDict.Remove(aHeaderRID);
            }
			this._allocationActionStatusDict.Add(aHeaderRID, aAllocationActionStatus);
            // end TT#241 - MD JEllis - Header Enqueue Process
        }

		/// <summary>
		/// Resets the AllocationActionStatus hashtable.
		/// </summary>
		internal void ResetAllocationActionStatus()
		{
            // begin TT#241 - MD - Jellis - Header Enqueue Process
            //if (this._allocationActionStatusHash != null)
            //{
            //    this._allocationActionStatusHash.Clear();
            //}
            if (_allocationActionStatusDict != null)
            {
                _allocationActionStatusDict.Clear();
            }
            // end TT#241 - MD - JEllis - Header Enqueue Process
		}
		#endregion AllocationActionSuccess
		
		#region GetSubtotalPackID
		public string GetSubtotalPackID(PackHdr aPack)
		{
			if (_subtotalPacksLastPack.PackName != aPack.PackName)
			{
				if (_subtotalPacks == null)
				{
					_subtotalPacks = new System.Collections.Hashtable();
				}
				_subtotalPacksLastPack = GetSubtotalPack(aPack);
			}
			else
			{
				if (_subtotalPacksLastPack.PackMultiple != aPack.PackMultiple 
					|| _subtotalPacksLastPack.GenericPack != aPack.GenericPack)
				{
					_subtotalPacksLastPack = GetSubtotalPack(aPack, _subtotalPacksLastPackArray);
				}
			}
			return _subtotalPacksLastPack.SubtotalPackName;
		}
		private PackHdr GetSubtotalPack(PackHdr aPack)
		{
			if (_subtotalPacks.Contains(aPack.PackName))
			{
				_subtotalPacksLastPackArray = (System.Collections.ArrayList)_subtotalPacks[aPack.PackName];
			}
			else
			{
				_subtotalPacksLastPackArray = new System.Collections.ArrayList();
				_subtotalPacks.Add(aPack.PackName, _subtotalPacksLastPackArray);
			}
			return GetSubtotalPack(aPack, _subtotalPacksLastPackArray);
		}
		private PackHdr GetSubtotalPack(PackHdr aPack, ArrayList aSubtotalPacksArray)
		{
			PackHdr subtotalPack = null;
			if (aSubtotalPacksArray.Count > 0)
			{
				foreach (PackHdr ph in aSubtotalPacksArray)
				{
					if (ph.PackMultiple == aPack.PackMultiple &&
						ph.GenericPack == aPack.GenericPack)
					{
						subtotalPack = ph;
						break;
					}
				}
			}
			if (subtotalPack == null)
			{
				subtotalPack = new PackHdr(aPack.PackName);
				aPack.CopyPackContentTo(subtotalPack);
				subtotalPack.SubtotalPackName = subtotalPack.PackName;
				subtotalPack.SubtotalPackName = subtotalPack.SubtotalPackName + " (" + subtotalPack.PackMultiple.ToString(CultureInfo.CurrentUICulture) + ")";
				if (!subtotalPack.GenericPack)
				{
					subtotalPack.SubtotalPackName = subtotalPack.SubtotalPackName + "+";
				}
				aSubtotalPacksArray.Add(subtotalPack);
			}
			return subtotalPack;
		}
		#endregion GetSubtotalPackID	

		#region GetSubtotal
		/// <summary>
		/// Gets the Grand Total Profile for all allocations attached to this transaction.
		/// </summary>
		/// <returns></returns>
		public AllocationSubtotalProfile GetAllocationGrandTotalProfile()
		{
			if (_grandTotalProfile == null)
			{
				_grandTotalProfile = GetAllocationSubtotalProfile(GrandTotalName);
				if (_grandTotalProfile.SubtotalMembers.Count == 0)
				{
                    // begin TT#1154 - MD - Jellis - Group Allocation Style Review No Stores
                    //if (_grandTotalProfile.AssortmentProfile == null)   //  TT#1154- MD - Jellis - Group Allocation Infinite  Loop When Going to SIze Analysis
                    //{                                                   // TT#1154 - MD- Jellis - Index out of range
                    //    this.AddAllocationProfileToGrandTotal(this.GetAllocationProfileList());
                    //}                    // TT#1154 - MD - Jellis - Group Allocation Infinite  Loop When Going to SIze Analysis
                    AddAllocationProfileToGrandTotal(this.GetAllocationProfileList());
                    // end TT#1154 - MD - Jellis - Group Allocation Style Review No Stores
				}
			}
			return _grandTotalProfile;
		}

		// Begin TT#831-MD - stodd - Need, Need%, & VSW Onhand incorrect 
        public AllocationSubtotalProfile GetAssortmentGrandTotalProfile()
        {
            if (_grandTotalProfile == null)
            {
                _grandTotalProfile = GetAllocationSubtotalProfile(GrandTotalName);
                if (_grandTotalProfile.SubtotalMembers.Count == 0)
                {
                    AddAllocationProfileToGrandTotal(this.GetAssortmentMemberProfileList());
                }
            }
            return _grandTotalProfile;
        }
		// End TT#831-MD - stodd - Need, Need%, & VSW Onhand incorrect 

		/// <summary>
		/// Gets named AllocationSubtotalProfile
		/// </summary>
		/// <param name="aSubtotalName">SubtotalName to retrieve.</param>
		/// <returns></returns>
		public AllocationSubtotalProfile GetAllocationSubtotalProfile (string aSubtotalName)
		{
			AddAllocationSubtotal(aSubtotalName);
			AllocationSubtotalProfileList subtotalList 
				= (AllocationSubtotalProfileList)GetMasterProfileList(eProfileType.AllocationSubtotal);
             // begin TT#2727 - Jellis - Rebuild Intransit Giving wrong message and gets Null Reference
            if (subtotalList != null)
            {
                // end TT#2727 - Jellis - Rebuild Intransit Giving wrong message and gets Null Reference
                return (AllocationSubtotalProfile)subtotalList.FindKey(GetAllocationSubtotalKey(aSubtotalName));
            }  // TT#2727 - Jellis - Rebuild Intransit Giving wrong message and gets Null Reference
            return null; //TT#2727 - Jellis - Rebuild Intransit Giving wrong message and gets Null Reference
		}

		/// <summary>
		/// Removes the AllocationSubtotalProfileList
		/// </summary>
		public void RemoveAllocationSubtotalProfileList()
		{
			RemoveProfileListGroup(eProfileType.AllocationSubtotal);
			this._grandTotalProfile = null; // MID Track 4210 Multi Intransit displayed/maintained incorrectly
			this._subtotalKeyHash.Clear(); // MID Track 4210 Multi Intransit displayed/maintained incorrectly
		}

		public string[] GetSubtotalNames()
		{
			string[] aArray = new string[_subtotalKeyHash.Count];
			_subtotalKeyHash.Values.CopyTo(aArray,0);
			return aArray;
		}

		/// <summary>
		/// Gets the integer key for a subtotal.
		/// </summary>
		/// <param name="aSubtotalName"></param>
		/// <returns>Integer key for the subtotal.</returns>
		public int GetAllocationSubtotalKey(string aSubtotalName)
		{
			if (_subtotalKeyHashLastKey != aSubtotalName)
			{
				if (_subtotalKeyHash.Contains(aSubtotalName))
				{
					_subtotalKeyHashLastKey = aSubtotalName;
					_subtotalKeyHashLastValue = (int)_subtotalKeyHash[aSubtotalName];
				}
				else
				{
					return 0;
				}
			}
			return _subtotalKeyHashLastValue;
		}

		/// <summary>
		/// Generates an integer key for a subtotal name.
		/// </summary>
		/// <param name="aSubtotalName">Subtotal Name</param>
		/// <returns>Key (if subtotal already exists, returns existing key)</returns>
		internal int GenerateSubtotalKey(string aSubtotalName)
		{
			int key = GetAllocationSubtotalKey(aSubtotalName);
			if (key == 0)
			{
				return _subtotalKeyHash.Count + 1;
			}
			return key;
		}
		#endregion GetSubtotal

		#region AddAllocationSubtotal
		/// <summary>
		/// Adds a subtotal name to the transaction.
		/// </summary>
		/// <param name="aSubtotalName">Subtotal name.</param>
		/// <returns>True: indicates subtotal added; False: indicates subtotal already exists</returns>
		public bool AddAllocationSubtotal(string aSubtotalName)
		{
			if (_subtotalKeyHash.Contains(aSubtotalName))
			{
				// subtotal already exists
				return false;
			}
			AllocationSubtotalProfile subtotalProfile 
				= new AllocationSubtotalProfile
				(this, aSubtotalName, GenerateSubtotalKey(aSubtotalName));
			AllocationSubtotalProfileList subtotalList 
				= (AllocationSubtotalProfileList)GetMasterProfileList(eProfileType.AllocationSubtotal);
			if (subtotalList == null)
			{
				subtotalList = new AllocationSubtotalProfileList(eProfileType.AllocationSubtotal);
				SetMasterProfileList(subtotalList);
			}
			_subtotalKeyHash.Add(aSubtotalName,subtotalProfile.Key);
			subtotalList.Add(subtotalProfile);
			return true;
		}

		/// <summary>
		/// Add AllocationProfile to GrandTotal
		/// </summary>
		/// <param name="aAllocationProfile">AllocationProfile to add.</param>
		public void AddAllocationProfileToGrandTotal(AllocationProfile aAllocationProfile)
		{
            // begin TT#981 - MD - Jellis - GA - Size Need on Group gets Argument Out of Range
            //// Begin TT#1282 - RMatelic - Assortment - Added colors to a style, the style originally had quantities in it.  When I added the colors the style went to 0
            //if (aAllocationProfile.HeaderType == eHeaderType.Assortment)
            //{
            //    return;
            //}
            //// End TT#1282
            // end TT#981 - MD - Jellis - GA - Size Need on Group gets Argument Out of Range
			if (!aAllocationProfile.StoresLoaded)
			{
				aAllocationProfile.LoadStores(false);
			}
			AddAllocationSubtotal(GrandTotalName);
			// begin MID Track 4297 Style Review gets error when select mult hdr with at least one released
			//AddAllocationProfileToSubtotal(aAllocationProfile, GrandTotalName);
			//aAllocationProfile.GrandTotalProfile = this.GetAllocationGrandTotalProfile();
			aAllocationProfile.GrandTotalProfile = this.GetAllocationGrandTotalProfile();
			AddAllocationProfileToSubtotal(aAllocationProfile, GrandTotalName);
			// end MID Track 4297 Style Review gets error when select mult hdr with at least one released
		}

		/// <summary>
		/// Adds AllocationProfiles to GrandTotal
		/// </summary>
		/// <param name="aAllocationList">AllocationProfileList of AllocationProfiles to add.</param>
		public void AddAllocationProfileToGrandTotal(AllocationProfileList aAllocationList)
		{
			AddAllocationProfileToGrandTotal(aAllocationList.ArrayList);
		}

		/// <summary>
		/// Adds AllocationProfiles to GrandTotal
		/// </summary>
		/// <param name="aAllocationProfiles">ICollection of AllocationProfiles to add</param>
		public void AddAllocationProfileToGrandTotal(System.Collections.ICollection aAllocationProfiles)
		{
			foreach (AllocationProfile ap in aAllocationProfiles)
			{
				AddAllocationProfileToGrandTotal(ap);
			}
		}

		/// <summary>
		/// Adds AllocationProfile to a subtotal
		/// </summary>
		/// <param name="aAllocationProfile">Allocation Profile to add</param>
		/// <param name="aSubtotalName">Subtotal Name</param>
		public void AddAllocationProfileToSubtotal(AllocationProfile aAllocationProfile, string aSubtotalName)
		{
			((AllocationSubtotalProfile)GetAllocationSubtotalProfile(aSubtotalName)).AddAllocationToSubtotal(aAllocationProfile);
		}

		/// <summary>
		/// Adds AllocationProfiles to a subtotal
		/// </summary>
		/// <param name="aAllocationList">AllocationProfileList of AllocationProfiles to add.</param>
		/// <param name="aSubtotalName">Subtotal Name</param>
		public void AddAllocationProfileToSubtotal(AllocationProfileList aAllocationList, string aSubtotalName)
		{
			AddAllocationProfileToSubtotal(aAllocationList.ArrayList, aSubtotalName);
		}

		/// <summary>
		/// Adds AllocationProfiles to a subtotal
		/// </summary>
		/// <param name="aAllocationProfiles">ICollection of Allocation Profiles to add.</param>
		/// <param name="aSubtotalName">Subtotal Name.</param>
		public void AddAllocationProfileToSubtotal(System.Collections.ICollection aAllocationProfiles, string aSubtotalName)
		{
			foreach (AllocationProfile ap in aAllocationProfiles)
			{
				AddAllocationProfileToSubtotal(ap, aSubtotalName);
			}
		}
		#endregion AddAllocationSubtotal

		#region RemoveAllocationSubtotal
		/// <summary>
		/// Removes a subtotal name from the transaction.
		/// </summary>
		/// <param name="aSubtotalName">Subtotal name.</param>
		/// <returns>True: indicates subtotal Removed; False: indicates subtotal did not exist</returns>
		public bool RemoveAllocationSubtotal(string aSubtotalName)
		{
			if (!(_subtotalKeyHash.Contains(aSubtotalName)))
			{
				return false;
			}
			AllocationSubtotalProfileList subtotalList 
				= (AllocationSubtotalProfileList)GetMasterProfileList(eProfileType.AllocationSubtotal);
			if (subtotalList == null)
			{
				return false;
			}
			AllocationSubtotalProfile sp = (AllocationSubtotalProfile)subtotalList[(int)_subtotalKeyHash[aSubtotalName]];
			foreach (AllocationProfile ap in sp.SubtotalMembers)
			{
				sp.RemoveAllocationFromSubtotal(ap);
			}
			subtotalList.Remove(sp);
			_subtotalKeyHash.Remove(aSubtotalName);
			return true;
		}

		/// <summary>
		/// Remove AllocationProfile from GrandTotal
		/// </summary>
		/// <param name="aAllocationProfile">AllocationProfile to Remove.</param>
		public void RemoveAllocationProfileFromGrandTotal(AllocationProfile aAllocationProfile)
		{
			RemoveAllocationProfileFromSubtotal(aAllocationProfile, GrandTotalName);
		}

		/// <summary>
		/// Removes AllocationProfiles from GrandTotal
		/// </summary>
		/// <param name="aAllocationList">AllocationProfileList of AllocationProfiles to Remove.</param>
		public void RemoveAllocationProfileFromGrandTotal(AllocationProfileList aAllocationList)
		{
			RemoveAllocationProfileFromGrandTotal(aAllocationList.ArrayList);
		}

		/// <summary>
		/// Removes AllocationProfiles from GrandTotal
		/// </summary>
		/// <param name="aAllocationProfiles">ICollection of AllocationProfiles to Remove</param>
		public void RemoveAllocationProfileFromGrandTotal(System.Collections.ICollection aAllocationProfiles)
		{
			foreach (AllocationProfile ap in aAllocationProfiles)
			{
				RemoveAllocationProfileFromGrandTotal(ap);
			}
		}

		/// <summary>
		/// Removes AllocationProfile from a subtotal
		/// </summary>
		/// <param name="aAllocationProfile">Allocation Profile to Remove</param>
		/// <param name="aSubtotalName">Subtotal Name</param>
		public void RemoveAllocationProfileFromSubtotal(AllocationProfile aAllocationProfile, string aSubtotalName)
		{
			((AllocationSubtotalProfile)GetAllocationSubtotalProfile(aSubtotalName)).RemoveAllocationFromSubtotal(aAllocationProfile);
		}

		/// <summary>
		/// Removes AllocationProfiles from a subtotal
		/// </summary>
		/// <param name="aAllocationList">AllocationProfileList of AllocationProfiles to Remove.</param>
		/// <param name="aSubtotalName">Subtotal Name</param>
		public void RemoveAllocationProfileFromSubtotal(AllocationProfileList aAllocationList, string aSubtotalName)
		{
			RemoveAllocationProfileFromSubtotal(aAllocationList.ArrayList, aSubtotalName);
		}

		/// <summary>
		/// Removes AllocationProfiles from a subtotal
		/// </summary>
		/// <param name="aAllocationProfiles">ICollection of Allocation Profiles to Remove.</param>
		/// <param name="aSubtotalName">Subtotal Name.</param>
		public void RemoveAllocationProfileFromSubtotal(System.Collections.ICollection aAllocationProfiles, string aSubtotalName)
		{
			foreach (AllocationProfile ap in aAllocationProfiles)
			{
				RemoveAllocationProfileFromSubtotal(ap, aSubtotalName);
			}
            ResetAnalysisSettings();  // TT#1154 - MD - Jellis - Group Allocation Style Review No Stores
		}
		#endregion RemoveAllocationSubtotal

		#region OnHand
		//======================
		// OnHand and Intransit
		//======================
		/// <summary>
		/// Gets instance of OnHand reader.
		/// </summary>
		/// <returns>OnHandReader</returns>
		public OnHandReader GetOnHandReader()
		{
			if (_onHandRdr == null)
			{
				_onHandRdr = new OnHandReader(this);
			}
			return _onHandRdr;
		}
		#endregion OnHand
		
		// BEGIN MID Track #2230   Display Current Week-to-Day Sales on Sku Review
		#region Sales
		//=======
		// Sales
		//=======
		/// <summary>
		/// Gets instance of Sales reader.
		/// </summary>
		/// <returns>OnHandReader</returns>
		public DailySalesReader GetDailySalesReader()
		{
			if (_dailySalesRdr == null)
			{
				_dailySalesRdr = new DailySalesReader(this);
			}
			return _dailySalesRdr;
		}
		#endregion Sales
		// END MID Track #2230   Display Current Week-to-Day Sales on Sku Review

		#region Intransit
		/// <summary>
		/// Gets instance of intransit reader.
		/// </summary>
		/// <returns></returns>
		public IntransitReader GetIntransitReader()
		{
			if (_intransitRdr == null)
			{
				_intransitRdr = new IntransitReader(this);
			}
			return _intransitRdr;
		}

		/// <summary>
		/// Resets the Intransit Reader so that subsequent reads retrieve the most recently updated intransit.
		/// </summary>
		public void ResetIntransitReader()
		{
			if (_intransitRdr != null)
			{
				_intransitRdr.ResetIntransitReader();
				AllocationSubtotalProfileList aspl = (AllocationSubtotalProfileList)GetMasterProfileList(eProfileType.AllocationSubtotal);
			    // begin TT#2727 - Jellis - Rebuild Intransit Giving wrong message and gets Null Reference
                if (aspl != null)
                {
                    // end TT#2727 - Jellis - Rebuild Intransit Giving wrong message and gets Null Reference
                    foreach (AllocationSubtotalProfile asp in aspl)
                    {
                        asp.RedoIntransitPctToTotal();
                    }
                } // TT#2727 - Jellis - Rebuild Intransit Giving wrong message and gets Null Reference
                // begin TT#3079 - AnF - Jellis - VSW Calculation Resets incorrectly on Cancel Intransit
                AllocationProfileList apl = (AllocationProfileList)GetMasterProfileList(eProfileType.Allocation);
                FlushSizeNeedMethod(null);
                foreach (AllocationProfile ap in apl)
                {
                    ap.LoadIntransit = true;
                }
                // end TT#3079 - AnF - Jellis - VSW Calculation Resets incorrectly on Cancel Intransit
			}
		}
		#endregion Intransit

		// BEGIN TT#1401 - stodd - add resevation stores (IMO)
		#region IMO
		/// <summary>
		/// Gets instance of IMO reader.
		/// </summary>
		/// <returns></returns>
		public IMO_Reader GetIMOReader()
		{
			if (_IMORdr == null)
			{
				_IMORdr = new IMO_Reader(this);
			}
			return _IMORdr;
		}

		/// <summary>
		/// Resets the IMO Reader so that subsequent reads retrieve the most recently updated IMO.
		/// </summary>
		public void ResetIMOReader()
		{
			if (_IMORdr != null)
			{
				_IMORdr.ResetIMO_Reader();
				//AllocationSubtotalProfileList aspl = (AllocationSubtotalProfileList)GetMasterProfileList(eProfileType.AllocationSubtotal);
				//foreach (AllocationSubtotalProfile asp in aspl)
				//{
				//    asp.RedoIntransitPctToTotal();
				//}
			}
		}
        // begin TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4
        //Begin TT#739-MD -jsobek -Delete Stores -VSW
        /// <summary>
        /// Gets an instance of the StoreVswReverseOnhand Container
        /// </summary>
        /// <returns>An instance of the StoreVswReverseOnhand container.</returns>
        //public StoreVswReverseOnhand GetVswReverseOnhandContainer()
        //{
        //    if (_storeVswReverseOnhand == null)
        //    {
        //        _storeVswReverseOnhand = new StoreVswReverseOnhand();
        //    }
        //    return _storeVswReverseOnhand;
        //}
        public VswReverseOnhandVariableManager GetVswReverseOnhandContainer()
        {
            if (_storeVswReverseOnhand == null)
            {
                _storeVswReverseOnhand = new VswReverseOnhandVariableManager();
            }
            return _storeVswReverseOnhand;
        }
        //End TT#739-MD -jsobek -Delete Stores -VSW
        // end TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4

        // begin TT#488 - MD - JEllis - Group ALlocation
        /// <summary>
        /// Gets Build Item Indicator
        /// </summary>
        /// <param name="aBuildItemKey">Key for which Build Item Indicator is desired</param>
        /// <returns>True: Build Item should occur for the given key</returns>
        /// <remarks>Build Item Key is either Assortment Header RID or Allocation Header RID.  When a header belongs to an assortment, the key must be the Assortment RID otherwise it is the Header's RID</remarks>
        public bool GetBuildItem(int aBuildItemKey)
        {            
            return GetBuildItemIndicators(aBuildItemKey).BuildItem;
        }
        /// <summary>
        /// Gets Assortment Calculate Item/VSW indicator
        /// </summary>
        /// <param name="aBuildItemKey"Key for which indicator is desired></param>
        /// <returns>True:  Assortment needs to rebuild its item/VSW values</returns>
        /// <remarks>Build Item Key is either Assortment Header RID or Allocation Header RID.  When a header belongs to an assortment, the key must be the Assortment RID otherwise it is the Header's RID</remarks>
        public bool GetCalcAssortmentItemVsw(int aBuildItemKey)
        {
            return GetBuildItemIndicators(aBuildItemKey).CalculateAssortmentItemVsw;
        }
        /// <summary>
        /// Gets Building Item VSW Values Indictor
        /// </summary>
        /// <param name="aBuildItemKey">Key for which Building Item VSW Values Indicator is desired</param>
        /// <returns>True: Building Item VSW Values is currently in progress; False: Building Item VSW Values is currently not in progress.</returns>
        public bool GetBuildingItemVSWValues(int aBuildItemKey)
        {
            return GetBuildItemIndicators(aBuildItemKey).BuildingItemVswValues;
        }
        /// <summary>
        /// Gets Build Item Suspended Indicator
        /// </summary>
        /// <param name="aBuildItemKey">Key for which Build Item Suspended Indicator is desired</param>
        /// <returns>True: Build Item for the given key is currently suspended (ie. Build Item is in progress for the given key)</returns>
        /// <remarks>Build Item Key is either Assortment Header RID or Allocation Header RID.  When a header belongs to an assortment, the key must be the Assortment RID otherwise it is the Header's RID</remarks>
        public bool GetBuildItemSuspended(int aBuildItemKey)
        {
            return GetBuildItemIndicators(aBuildItemKey).BuildItemSuspended;
        }
        /// <summary>
        /// Gets Calculate Imo Allocation Maximum Indicator
        /// </summary>
        /// <param name="aBuildItemKey">Key for which Calculate Imo Allocation Maximum Indicator is desired</param>
        /// <returns>True: Calculate Imo Allocation Maximum for the given Build Item Key</returns>
        /// <remarks>Build Item Key is either Assortment Header RID or Allocation Header RID.  When a header belongs to an assortment, the key must be the Assortment RID otherwise it is the Header's RID</remarks>
        public bool GetCalculateImoAloctnMax(int aBuildItemKey)
        {
            return GetBuildItemIndicators(aBuildItemKey).CalculateImoAloctnMax;
        }
        /// <summary>
        /// Gets Determine Header VSW Process Order Indicator
        /// </summary>
        /// <param name="aBuildItemKey">Key for which Determine Header Process Order Indicator is desired</param>
        /// <returns>True: The order in which the headers are to be processed for VSW must be determined</returns>
        /// <remarks>Build Item Key is either Assortment Header RID or Allocation Header RID.  When a header belongs to an assortment, the key must be the Assortment RID otherwise it is the Header's RID</remarks>
        public bool GetDetermineHeaderVswProcessOrder(int aBuildItemKey)
        {
            return GetBuildItemIndicators(aBuildItemKey).DetermineHeaderVswProcessOrder;
        }
        /// <summary>
        /// Sets Build Item indicator
        /// </summary>
        /// <param name="aBuildItemKey">Key for which the build item indicator is to be set</param>
        /// <param name="aBuildItemValue">True: Build Item is required; False: Build Item is current and correct</param>
        public void SetBuildItem(int aBuildItemKey, bool aBuildItemValue)
        {
            BuildItemIndicators bii = GetBuildItemIndicators(aBuildItemKey);
            bii.BuildItem = aBuildItemValue;
        }
        /// <summary>
        /// Sets Building Item VSW Values Indicator
        /// </summary>
        /// <param name="aBuildItemKey">Key for which the Building Item VSW Values Indicator is to be set</param>
        /// <param name="aBuildingItemVswValue">True: Item VSW Values are being calculated; False: Item VSW values are not being calculated</param>
        public void SetBuildingItemVswValues(int aBuildItemKey, bool aBuildingItemVswValue)
        {
            BuildItemIndicators bii = GetBuildItemIndicators(aBuildItemKey);
            bii.BuildingItemVswValues = aBuildingItemVswValue;
        }
        /// <summary>
        /// Sets Assortment Calculation indicator for Item/VSW 
        /// </summary>
        /// <param name="aBuildItemKey">Key for which the indicator is to be set</param>
        /// <param name="aCalcAssortmentItemVswValue">True: Item VSW values need to be re-calc'd for assortment</param>
        public void SetCalcAssortmentItemVsw(int aBuildItemKey, bool aCalcAssortmentItemVswValue)
        {
            BuildItemIndicators bii = GetBuildItemIndicators(aBuildItemKey);
            bii.CalculateAssortmentItemVsw = aCalcAssortmentItemVswValue;
        }
        /// <summary>
        /// Sets the Build Item Suspended Indicator
        /// </summary>
        /// <param name="aBuildItemKey">Key for which the Build Item Suspended indicator is to be set</param>
        /// <param name="aBuildItemSuspendValue">True: Build Items are being constructed and all other build item requests are to be ignored; False, Build Item Indicator is current and may be used to trigger a new Build Item for the given key</param>
        /// <remarks>Build Item Key is either Assortment Header RID or Allocation Header RID.  When a header belongs to an assortment, the key must be the Assortment RID otherwise it is the Header's RID</remarks>
        public void SetBuildItemSuspended(int aBuildItemKey, bool aBuildItemSuspendValue)
        {
            BuildItemIndicators bii = GetBuildItemIndicators(aBuildItemKey);
            bii.BuildItemSuspended = aBuildItemSuspendValue;
        }
        /// <summary>
        /// Sets the Calculate Imo Allocation Maximum Indicator
        /// </summary>
        /// <param name="aBuildItemKey">Key for which the Calculate Imo Allocation Maximum Indicator is to be set</param>
        /// <param name="aCalculateImoAloctnMaxValue">True: indicates the calculation is required; False: indicates the calculation is done and current</param>
        /// <remarks>Build Item Key is either Assortment Header RID or Allocation Header RID.  When a header belongs to an assortment, the key must be the Assortment RID otherwise it is the Header's RID</remarks>
        public void SetCalculateImoAloctnMax(int aBuildItemKey, bool aCalculateImoAloctnMaxValue)
        {
            BuildItemIndicators bii = GetBuildItemIndicators(aBuildItemKey);
            bii.CalculateImoAloctnMax = aCalculateImoAloctnMaxValue;
        }
        /// <summary>
        /// Sets Determine Header VSW Process Order Indicator
        /// </summary>
        /// <param name="aBuildItemKey">Key for which the Determine Header VSW Process Order Indicator is to be set</param>
        /// <param name="aDetermineHeaderVswProcessOrderValue">True: indicates Determine Header VSW Process Order must be resolved</param>
        /// <remarks>Build Item Key is either Assortment Header RID or Allocation Header RID.  When a header belongs to an assortment, the key must be the Assortment RID otherwise it is the Header's RID</remarks>
        public void SetDetermineHeaderVswProcessOrder(int aBuildItemKey, bool aDetermineHeaderVswProcessOrderValue)
        {
            BuildItemIndicators bii = GetBuildItemIndicators(aBuildItemKey);
            bii.DetermineHeaderVswProcessOrder = aDetermineHeaderVswProcessOrderValue;
        }
        /// <summary>
        /// Gets the Build Item Indicators for the given build Item Key
        /// </summary>
        /// <param name="aBuildItemKey">Key for which the Build Item Indicators is desired</param>
        /// <returns>BuildItemIndicators for the given key</returns>
        /// <remarks>Build Item Key is either Assortment Header RID or Allocation Header RID.  When a header belongs to an assortment, the key must be the Assortment RID otherwise it is the Header's RID</remarks>
        private BuildItemIndicators GetBuildItemIndicators(int aBuildItemKey)
        {
            if (aBuildItemKey != _lastBuildItemIndicators.BuildItemKey)
            {
                if (!_buildItemIndicators.TryGetValue(aBuildItemKey, out _lastBuildItemIndicators))
                {
                    _lastBuildItemIndicators = new BuildItemIndicators(aBuildItemKey);
                    _buildItemIndicators.Add(_lastBuildItemIndicators.BuildItemKey, _lastBuildItemIndicators);
                }
            }
            return _lastBuildItemIndicators;
        }
        /// <summary>
        /// Determines process order for VSW calculation on multiple headers (by store)
        /// </summary>
        /// <param name="aAllocationProfile"></param>
        public void DetermineHeaderVswProcessOrder(AllocationProfile aAllocationProfile)
        {
            AssortmentProfile asrtP = aAllocationProfile as AssortmentProfile;
            AllocationProfile[] apList;
            Dictionary<int, AllocationProfile[]> processOrder = new Dictionary<int,AllocationProfile[]>();
            if (asrtP != null)
            {
                AllocationProfile[] members = asrtP.AssortmentMembers;  // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                foreach (AllocationProfile ap in members)
                {
                    _buildItemProcessOrder.Remove(ap.HeaderRID);
                }
                foreach (Index_RID strIdxRID in this.StoreIndexRIDArray())
                {
                    apList = new AllocationProfile[asrtP.AssortmentMembers.Length]; // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                    AllocationProfile ap;
                    MIDGenericSortItem[] mgsi = new MIDGenericSortItem[asrtP.AssortmentMembers.Length]; // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                    bool storeHasImoMaxValue = false;
                    for (int i = 0; i < members.Length; i++) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                    {
                        ap = members[i];
                        mgsi[i] = new MIDGenericSortItem();
                        mgsi[i].Item = i;
                        mgsi[i].SortKey = new double[4];
                        mgsi[i].SortKey[1] = ap.BulkColors.Count;
                        mgsi[i].SortKey[2] = - ap.GetStoreImoMaxValue(strIdxRID);
                        if (-mgsi[i].SortKey[2] < int.MaxValue
                            || ap.BulkSizeIntransitUpdated
                            || ap.GetStoreItemIsManuallyAllocated(eAllocationSummaryNode.Total, strIdxRID))
                        {
                            mgsi[i].SortKey[0] = 0;
                        }
                        else
                        {
                            storeHasImoMaxValue = true;
                            mgsi[i].SortKey[0] = -1;
                        }
                        mgsi[i].SortKey[3] = GetRandomDouble();
                    }
                    if (storeHasImoMaxValue)
                    {
                        Array.Sort(mgsi, new MIDGenericSortDescendingComparer());
                    }
                    for (int i = 0; i < members.Length; i++) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                    {
                        apList[i] = members[mgsi[i].Item];
                    }
                    processOrder.Add(strIdxRID.RID, apList);
                }
            }
            // Begin TT#1966-MD - JSmith - DC Fulfillment
            else if (aAllocationProfile.IsSubordinateHeader)
            {
                MasterHeaderProfile masterHeader = aAllocationProfile.MasterHeader;
                AllocationProfileList subordinateList = masterHeader.GetSubordinates(this, aAllocationProfile);
                apList = subordinateList.ArrayList.ToArray(typeof(AllocationProfile)) as AllocationProfile[];
                foreach (Index_RID strIdxRID in this.StoreIndexRIDArray())
                {
                    processOrder.Add(strIdxRID.RID, apList);
                }
            }
            // End TT#1966-MD - JSmith - DC Fulfillment
            else
            {
                apList = new AllocationProfile[1];
                apList[0] = aAllocationProfile;
                foreach (Index_RID strIdxRID in this.StoreIndexRIDArray())
                {
                    processOrder.Add(strIdxRID.RID, apList);
                }
            }
            _buildItemProcessOrder.Remove(aAllocationProfile.BuildItemKey);
            _buildItemProcessOrder.Add(aAllocationProfile.BuildItemKey, processOrder);
            SetDetermineHeaderVswProcessOrder(aAllocationProfile.BuildItemKey, false);
        }
        public AllocationProfile[] GetStoreHeaderVswProcessOrder(int aBuildItemKey, int aStoreRID)
        {
            AllocationProfile[] storeHeaderVswProcessOrder;
            Dictionary<int, AllocationProfile[]> processOrder;
            if (!_buildItemProcessOrder.TryGetValue(aBuildItemKey, out processOrder))
            {
                return null;
            }
            if (!processOrder.TryGetValue(aStoreRID, out storeHeaderVswProcessOrder))
            {
                return null;
            }
            return storeHeaderVswProcessOrder;
        }
        // end TT#488 - MD - Jellis - Group Allocation
		#endregion IMO
		// BEGIN TT#1401 - stodd - add resevation stores (IMO)

		#region GetStoreIntransit
		//		/// <summary>
		//		/// Gets store intransit for the specified Mdse RID and day range(onhand day upto but not including the target inventory day)
		//		/// </summary>
		//		/// <param name="aOnHandHnRID">RID for the OnHand Hierarchy node</param>
		//		/// <param name="aIKTArray">Array containing IntransitKeyType structures that identify the intransit to retrieve: Total, color or color-size.</param>
		//		/// <param name="aStoreRID">RID of the store for which intransit is to be retrieved</param>
		//		/// <returns>Requested Store Intransit</returns>
		//		public int GetStoreInTransit(
		//			int aOnHandHnRID, 
		//			IntransitKeyType[] aIKTArray, 
		//			int aStoreRID)
		//		{
		//			return GetStoreInTransit(
		//				aOnHandHnRID,
		//				aIKTArray,
		//				aStoreRID);
		//		}
		//		/// <summary>
		//		/// Gets store intransit for the specified Mdse RID and day range(onhand day upto but not including the target inventory day)
		//		/// </summary>
		//		/// <param name="aOnHandHnRID">RID for the OnHand Hierarchy node</param>
		//		/// <param name="aOnHandDay">Day Profile of the onhand day (first day of the shipping horizon)</param>
		//		/// <param name="aTargetInventoryDay">Day Profile of the targeted inventory day (last day of the shipping horizon)</param>
		//		/// <param name="aIKTArray">ArrayList containing IntransitKeyType structures that identify the intransit to retrieve: Total, color or color-size.</param>
		//		/// <param name="aStoreRID">RID of the store for which intransit is to be retrieved</param>
		//		/// <returns>Requested Store Intransit</returns>
		//		public int GetStoreInTransit(
		//			int aOnHandHnRID, 
		//			DayProfile aOnHandDay, 
		//			DayProfile aTargetInventoryDay, 
		//			ArrayList aIKTArray, 
		//			int aStoreRID)
		//		{
		//			ProfileList needHorizon = 
		//				this.SAB.ApplicationServerSession.Calendar.DayRange
		//				(aOnHandDay.YearDay, aTargetInventoryDay.YearDay);
		//			needHorizon.Remove(aTargetInventoryDay);
		//			return GetStoreInTransit(
		//				aOnHandHnRID, 
		//				needHorizon,
		//				aIKTArray,
		//				aStoreRID);
		//		}

		public int GetStoreInTransit(
			int aOnHandRID,
            StoreSalesITHorizon aStoreSalesITHorizon, // TT#4345 - MD - JEllis - GA VSW calculated incorrectly
			IntransitKeyType[] aIKT,
			int aStoreRID)
		{
            return this.GetIntransitReader().GetStoreIntransit(aStoreRID, aOnHandRID, aStoreSalesITHorizon, aIKT); // TT#4345 - MD - JEllis - GA VSW calculated incorrectly
		}
		//		public int GetStoreInTransit(
		//			int aOnHandRID,
		//			ProfileList aDayProflieList,
		//			ArrayList aIKTArray,
		//			int aStoreRID)
		//		{
		//			ArrayList sal = new ArrayList();
		//			sal.Add(aStoreRID);
		//			int intransit = 0;
		//			foreach (IntransitKeyType keyType in aIKTArray)
		//			{
		//				intransit += 
		//					(this.GetIntransitReader().GetDayRangeIntransit(
		//					aDayProflieList,
		//					aOnHandRID,
		//					keyType.ColorRID,
		//					keyType.SizeRID,
		//					sal))[0];
		//			}
		//			return intransit;
		//		}
		#endregion GetStoreIntransit

        // begin TT#1401 - JEllis - Urban Reservation Stores - pt 7
        // Replaced this code with "GetStoreImoHistory"
        // BEGIN TT#1401 - stodd - add resevation stores (IMO)
        //#region GetStoreIMO
        //public int GetStoreIMO(
        //    int aNodeRID,
        //    IntransitKeyType[] aIKT,
        //    int aStoreRID)
        //{
        //    return this.GetIMOReader().GetStoreIMO(aStoreRID, aNodeRID, aIKT);
        //}
        //#endregion GetStoreIMO
		// END TT#1401 - stodd - add resevation stores (IMO)
        #region StoreImoHistory
        /// <summary>
        /// Gets Store IMO History. 
        /// </summary>
        /// <param name="aAllocationProfile">AllocationProfile containing the date and Hierarchy Node</param>
        /// <param name="aIKT">Key type of the IMO to retrieve: Total, Color or Color-Size</param>
        /// <param name="aStoreRID">Store RID for which IMO is desired.</param>
        /// <returns>Actual IMO when date is undefined; otherwise, Zero for specified date.</returns>
        public int GetStoreImoHistory(
            AllocationProfile aAllocationProfile,
            IntransitKeyType aIKT,
            int aStoreRID)
        {
            return GetStoreImoHistory
               (aAllocationProfile.StyleHnRID,
                aAllocationProfile.BeginDay,
                aIKT,
                aStoreRID);
        }
        /// <summary>
        /// Gets Store IMO History. 
        /// </summary>
        /// <param name="aHnRID">Hierarchy Node RID for which IMO History is desired.</param>
        /// <param name="aOnHandDate">Date for which IMO is desired: Use Include.UndefinedDate to get current onhand; No "IMO" is returned for "defined" dates (ie. IMO is not a planned variable; so zero is returend when date is not Undefined date).</param>
        /// <param name="aIKT">Key type of the IMO to retrieve: Total, Color or Color-Size</param>
        /// <param name="aStoreRID">Store RID for which IMO is desired.</param>
        /// <returns>Actual IMO when date is undefined; otherwise, Zero for specified date.</returns>
        public int GetStoreImoHistory(
            int aHnRID,
            DateTime aOnHandDate,
            IntransitKeyType aIKT,
            int aStoreRID)
        {
            if (aOnHandDate == Include.UndefinedDate)
            {
                return this.GetIMOReader().GetStoreIMO(aStoreRID, aHnRID, aIKT);
            }
            return 0;
        }
        #endregion StoreImoHistory
        // end TT#1401 - JEllis - Urban Reservation Stores - pt 7

		#region GetStoreOTSSalesPlan
		public int GetStoreOTSSalesPlan(
			int aStoreRID, 
			int aPlanHnRID, 
			DayProfile aBeginDay, 
			DayProfile aTargetInventoryDay,
			double aPlanFactor)
		{
			ArrayList targetInventoryDayList = new ArrayList();
			targetInventoryDayList.Add(aTargetInventoryDay);
			return GetStoreOTSSalesPlan(aStoreRID, aPlanHnRID, this.GetAllocationPlanCube
				(aPlanHnRID, aBeginDay.Date, targetInventoryDayList),
				aBeginDay,
				aTargetInventoryDay,
				aPlanFactor);
		}
		public int GetStoreOTSSalesPlan(
			int aStoreRID,
			int aPlanHnRID,
			PlanCube aStorePlanCube,
			DayProfile aBeginDay,
			DayProfile aTargetInventoryDay,
			double aPlanFactor)
		{
			Hashtable salesPlanHash;
			//			salesPlanHash = (Hashtable)_storeSalesPlanHash[aStoreRID];
			salesPlanHash = (Hashtable)_storeSalesPlanHash[aPlanHnRID];
			if (salesPlanHash == null)
			{
				salesPlanHash = new Hashtable ();
				//				_storeSalesPlanHash.Add(aStoreRID, salesPlanHash);
				_storeSalesPlanHash.Add(aPlanHnRID, salesPlanHash);
			}
			Hashtable salesBeginDayHash;
			//			salesDayHash = (Hashtable)salesPlanHash[aPlanHnRID];
			salesBeginDayHash = (Hashtable)salesPlanHash[aBeginDay];
			if (salesBeginDayHash == null)
			{
				salesBeginDayHash = new Hashtable();
				//				salesPlanHash.Add(aPlanHnRID, salesDayHash);
				salesPlanHash.Add(aBeginDay, salesBeginDayHash);
			}
			//			Hashtable salesDayAccumHash;
			//			salesDayAccumHash = (Hashtable)salesDayHash[aBeginDay];
			//			if (salesDayAccumHash == null)
			//			{
			//				salesDayAccumHash = new Hashtable();
			//				salesDayHash.Add(aBeginDayDay, salesDayAccumHash);
			//			}
			int[] storeSalesPlan;
			if (salesBeginDayHash.Contains(aTargetInventoryDay))
			{
				storeSalesPlan = (int[])salesBeginDayHash[aTargetInventoryDay];
			}
			else
			{
				PlanCellReference planCellRef = new PlanCellReference(aStorePlanCube);
				planCellRef[eProfileType.Version] = Include.FV_ActionRID;
				planCellRef[eProfileType.HierarchyNode] = aPlanHnRID;
				planCellRef[eProfileType.QuantityVariable] = PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
				if (eOTSPlanLevelType.Regular == this.GetNodeData(aPlanHnRID).OTSPlanLevelType)
				{
					planCellRef[eProfileType.Variable] = PlanComputations.PlanVariables.SalesRegPromoUnitsVariable.Key;
				}
				else
				{
					planCellRef[eProfileType.Variable] = PlanComputations.PlanVariables.SalesTotalUnitsVariable.Key;
				}
				//				ArrayList weeklyPlanValues;
            
				//				storeSalesPlan = 0;
				// begin MID Track 5953 Null Reference when executing General Method
				//storeSalesPlan = new int[this.StoreIndexRIDArray().Length];
				Index_RID[] storeIndexRIDArray = this.GetStoreIndexRIDArray();
				storeSalesPlan = new int[storeIndexRIDArray.Length];
				// end MID Track 5953 Null Reference when executing General Method
				storeSalesPlan.Initialize();
				ProfileList weekList = 
					this.SAB.ApplicationServerSession.Calendar.GetWeekRange(aBeginDay.Date, aTargetInventoryDay.Date);
				//				Index_RID storeIndexRID;
				double[] dailySales;
				//				planCellRef[eProfileType.Store] = aStoreRID;
				//				storeIndexRID = StoreIndexRID(aStoreRID);
				//				weeklyPlanValues = planCellRef.GetCellRefArray(weekList);
				WeekProfile wk;
				ProfileList allStoreList = this.GetMasterProfileList(eProfileType.Store);
				for (int w=0; w < weekList.Count; w++)
				{
					planCellRef[eProfileType.Week] = weekList[w].Key;
					ArrayList storeWeekSalesPlans = planCellRef.GetCellRefArray(allStoreList);
					wk = (WeekProfile)weekList.ArrayList[w];
					
					if (wk.Key == ((DayProfile)aBeginDay).Week.Key)
					{
						if (aBeginDay.Week.Key == aTargetInventoryDay.Week.Key)
						{
							// begin MID Track 5953 Null Reference when executing General Method
							//foreach (Index_RID storeIdxRID in this.StoreIndexRIDArray())
							foreach (Index_RID storeIdxRID in storeIndexRIDArray)
								// end MID Track 5953 Null Reference when executing General Method
							{
								dailySales = this.GetDailySales(
									aPlanHnRID,
									storeIdxRID,
									wk,
									(int)((PlanCellReference)storeWeekSalesPlans[storeIdxRID.Index]).CurrentCellValue);
								for(int d=(aBeginDay.DayInWeek - 1);
									d < (aTargetInventoryDay.DayInWeek - 1);
									d++)
								{
									storeSalesPlan[storeIdxRID.Index] += (int)dailySales[d];
								}
							}
						}
						else if (aBeginDay.DayInWeek == 1)
						{
							// begin MID Track 5953 Null REference when executing General Method
							//foreach (Index_RID storeIdxRID in this.StoreIndexRIDArray())
							foreach (Index_RID storeIdxRID in storeIndexRIDArray)
								// end mID Track 5953 Null Reference when executing General MEthod
							{
								storeSalesPlan[storeIdxRID.Index] += (int)((PlanCellReference)storeWeekSalesPlans[storeIdxRID.Index]).CurrentCellValue;	
							}
						}
						else
						{
							// begin MID Track 5953 Null REference when executing General Method
							//foreach (Index_RID storeIdxRID in this.StoreIndexRIDArray())
                            foreach (Index_RID storeIdxRID in storeIndexRIDArray)
								// end mID Track 5953 Null Reference when executing General MEthod
							{
								dailySales = this.GetDailySales(
									aPlanHnRID,
									storeIdxRID,
									wk,
									(int)((PlanCellReference)storeWeekSalesPlans[storeIdxRID.Index]).CurrentCellValue);
								for (int d=(aBeginDay.DayInWeek - 1);
									d < 7;
									d++)
								{
									storeSalesPlan[storeIdxRID.Index] += (int)dailySales[d];
								}
							}
						}
					}
					else if (wk.Key == aTargetInventoryDay.Week.Key)
					{
						// begin MID Track 5953 Null REference when executing General Method
						//foreach (Index_RID storeIdxRID in this.StoreIndexRIDArray())
						foreach (Index_RID storeIdxRID in storeIndexRIDArray)
							// end mID Track 5953 Null Reference when executing General MEthod
						{
							dailySales = this.GetDailySales(
								aPlanHnRID,
								//StoreIndexRID(aStoreRID),  // MID Track 5525 AnF Defect 1618: Rounding Error
								storeIdxRID,                 // MID Track 5525 AnF Defect 1618: Rounding Error
								wk,
								(int)((PlanCellReference)storeWeekSalesPlans[storeIdxRID.Index]).CurrentCellValue);
							for(int d=0;
								d < (aTargetInventoryDay.DayInWeek - 1);
								d++)
							{
								storeSalesPlan[storeIdxRID.Index] += (int)dailySales[d];
							}
						}
					}
					else if (wk.Key > aTargetInventoryDay.Week.Key)
					{
						break;
					}
					else
					{
						// begin MID Track 5953 Null REference when executing General Method
						//foreach (Index_RID storeIdxRID in this.StoreIndexRIDArray())
						foreach (Index_RID storeIdxRID in storeIndexRIDArray)
							// end mID Track 5953 Null Reference when executing General MEthod

						{
							storeSalesPlan[storeIdxRID.Index] += (int)((PlanCellReference)storeWeekSalesPlans[storeIdxRID.Index]).CurrentCellValue;	
						}
					}
				}
				//				storeSalesPlan =
				//					(int)((((double)storeSalesPlan * aPlanFactor) / 100.0d) + 0.5d);
				salesBeginDayHash.Add(aTargetInventoryDay, storeSalesPlan);
			}
			//			return storeSalesPlan;
			// begin MID Track 5953 Null Reference when executing General Method
			//double intermediateValue = ((double)storeSalesPlan[this.StoreIndexRID(aStoreRID).Index] * aPlanFactor) / 100.0d;
			double intermediateValue = ((double)storeSalesPlan[this.GetStoreIndexRID(aStoreRID).Index] * aPlanFactor) / 100.0d;
			// end MID Track 5953 Null Reference when executing Getneral Method
			if (intermediateValue < 0)
			{
				return (int)(intermediateValue - .5d);
			}
			return (int)(intermediateValue + 0.5d);

		}
		#endregion GetStoreSalesPlan

		#region GetStoreOTSStockPlan
		/// <summary>
		/// Gets a store's Open To Ship Stock Plan
		/// </summary>
		/// <param name="aStoreRID">RID of the store</param>
		/// <param name="aPlanHnRID">RID of the Plan Hierarchy Node RID</param>
		/// <param name="aTargetInventoryDay">Day for which the stock plan is requested</param>
		/// <param name="aPlanFactor">Percentage of the plan to return</param>
		/// <returns>Store's stock plan on the requested day.</returns>
		public int GetStoreOTSStockPlan(
			int aStoreRID, 
			int aPlanHnRID, 
			DayProfile aTargetInventoryDay,
			double aPlanFactor)
		{
			return GetStoreOTSStockPlan(aStoreRID, aPlanHnRID, this.GetAllocationPlanCube
				(aPlanHnRID, aTargetInventoryDay.Date, aTargetInventoryDay.Date),
				aTargetInventoryDay,
				aPlanFactor);
			//			ArrayList targetInventoryDayList = new ArrayList();
			//			targetInventoryDayList.Add(aTargetInventoryDay);
			//			return GetStoreOTSStockPlan(aStoreRID, aPlanHnRID, this.GetAllocationPlanCube
			//				(aPlanHnRID, aTargetInventoryDay.Date, targetInventoryDayList),
			//				aTargetInventoryDay,
			//				aPlanFactor);
		}

		/// <summary>
		/// Gets a store's Open To Ship Stock Plan
		/// </summary>
		/// <param name="aStoreRID">RID of the store</param>
		/// <param name="aPlanHnRID">RID of the Plan Hierarchy Node RID</param>
		/// <param name="aStorePlanCube">Plan Cube where the stock plan resides</param>
		/// <param name="aTargetInventoryDay">Day for which the stock plan is requested</param>
		/// <param name="aPlanFactor">Percentage of the plan to return</param>
		/// <returns>Store's stock plan on the requested day.</returns>
		public int GetStoreOTSStockPlan(
			int aStoreRID,
			int aPlanHnRID,
			PlanCube aStorePlanCube,
			DayProfile aTargetInventoryDay,
			double aPlanFactor)
		{
			Hashtable stockPlanHash;
			stockPlanHash = (Hashtable)_storeStockPlanHash[aStoreRID];
			if (stockPlanHash == null)
			{
				stockPlanHash = new Hashtable ();
				_storeStockPlanHash.Add(aStoreRID, stockPlanHash);
			}
			Hashtable stockDayHash;
			stockDayHash = (Hashtable)stockPlanHash[aPlanHnRID];
			if (stockDayHash == null)
			{
				stockDayHash = new Hashtable();
				stockPlanHash.Add(aPlanHnRID, stockDayHash);
			}
			int stockPlan;
			if (stockDayHash.Contains(aTargetInventoryDay))
			{
				stockPlan = (int)stockDayHash[aTargetInventoryDay];
			}
			else
			{
				WeekProfile wk = this.SAB.ApplicationServerSession.Calendar.GetWeek(aTargetInventoryDay.Date);
				// begin MID Track 5953 Null Reference when executing General Method
				//Index_RID storeIdxRID = StoreIndexRID(aStoreRID);
				Index_RID storeIdxRID = GetStoreIndexRID(aStoreRID);
				// end MID Track 5953 Null Reference when executing General Method
				stockPlan = GetStoreDailyInventory(
					aPlanHnRID,
					(StoreProfile)((ProfileList)this.GetMasterProfileList(eProfileType.Store)).FindKey(storeIdxRID.RID),
					//				this.SAB.StoreServerSession.GetStoreProfile(aStoreRID),
					wk,
					this.GetDailySales(aStorePlanCube, aPlanHnRID, storeIdxRID, wk))
					//					aPlanFactor)
					[aTargetInventoryDay.DayInWeek - 1];
				stockDayHash.Add(aTargetInventoryDay, stockPlan);
			}
			//			return stockPlan;
			double intermediateValue = ((double)stockPlan
				* aPlanFactor
				/ 100);
			if (intermediateValue < 0)
			{
				return (int)(intermediateValue - .5d);
			}
			return 
				(int)(intermediateValue + 0.5d);
		}
		#endregion GetStoreStockPlan

		#region GetStoreBasisElementValue 
		public double GetStoreBasisElementValue (int aPlanHnRID, int aStoreRID, VariableProfile aVariableProfile, TimeTotalVariableProfile aTimeVariableProfile)
		{
			int[] aStoreRIDList = {aStoreRID};
			return GetStoreBasisElementValue(aPlanHnRID, aStoreRIDList, aVariableProfile, aTimeVariableProfile)[0];
		}

		public double[] GetStoreBasisElementValue(int aPlanHnRID, int[] aStoreRIDList, VariableProfile aVariableProfile, TimeTotalVariableProfile aTimeVariableProfile)
		{
			double[] storeElementValues = new double[aStoreRIDList.Length];
			PlanCellReference planCellRef = GetBasisDateTotalRef(aPlanHnRID, aVariableProfile, aTimeVariableProfile);
			ArrayList storePlanValues;
			ProfileList storeList = this.GetProfileList(eProfileType.Store);
            
			storePlanValues = planCellRef.GetCellRefArray(storeList);
			int count = aStoreRIDList.Length;
			PlanCellReference aStorePlanValue;
			Index_RID storeIdxRID;
			for(int j = 0;  j < count; j++)
			{
				// begin MID Track 5953 Null Reference when executing General Method
				//storeIdxRID = this.StoreIndexRID(aStoreRIDList[j]);
				storeIdxRID = this.GetStoreIndexRID(aStoreRIDList[j]);
				// end MID Track 5953 Null Reference when executing General Method
				aStorePlanValue = (PlanCellReference)storePlanValues[storeIdxRID.Index];
				storeElementValues[j] = aStorePlanValue.CurrentCellValue;
			}
			return storeElementValues;
		}

        // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
        public string[] GetStoreBasisElementValueString(int aPlanHnRID, int[] aStoreRIDList, VariableProfile aVariableProfile, TimeTotalVariableProfile aTimeVariableProfile)
        {
            string[] storeElementValues = new string[aStoreRIDList.Length];
            PlanCellReference planCellRef = GetBasisDateTotalRef(aPlanHnRID, aVariableProfile, aTimeVariableProfile);
            ArrayList storePlanValues;
            ProfileList storeList = this.GetProfileList(eProfileType.Store);

            storePlanValues = planCellRef.GetCellRefArray(storeList);
            int count = aStoreRIDList.Length;
            PlanCellReference aStorePlanValue;
            Index_RID storeIdxRID;
            for (int j = 0; j < count; j++)
            {
                storeIdxRID = this.GetStoreIndexRID(aStoreRIDList[j]);
                aStorePlanValue = (PlanCellReference)storePlanValues[storeIdxRID.Index];
                
                PlanWaferCell pwc = new PlanWaferCell(aStorePlanValue, aStorePlanValue.CurrentCellValue, "1", "1");
                storeElementValues[j] = pwc.ValueAsString;
            }
            return storeElementValues;
        }
        // End TT#638

		private PlanCellReference GetBasisDateTotalRef(int aPlanHnRID, VariableProfile aVariableProfile, TimeTotalVariableProfile aTimeVariableProfile)
		{
			PlanCube planCube = (PlanCube)this.GetAllocationBasisCubeGroup().GetCube(eCubeType.StoreBasisDateTotal);
			// BEGIN MID Track #2621 Period Basis not processed correctly
			//			if (planCube == null)
			//			{ 
			//				this.OpenAllocationBasisCubeGroup(this.GetAllocationGrandTotalProfile().PlanHnRID, Include.UndefinedDate, Include.UndefinedDate);
			//				planCube = (PlanCube)this.GetAllocationBasisCubeGroup().GetCube(eCubeType.StoreBasisDateTotal);
			//			}
			// END MID Track #2621 Period Basis not processed correctly
			PlanCellReference planCellRef = new PlanCellReference(planCube);
			planCellRef[eProfileType.Version] = Include.FV_ActionRID;
			planCellRef[eProfileType.HierarchyNode] = aPlanHnRID;
			planCellRef[eProfileType.Basis] = 0;
			planCellRef[eProfileType.QuantityVariable] = PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
			planCellRef[eProfileType.TimeTotalVariable] = aTimeVariableProfile.Key;
			
			planCellRef[eProfileType.Variable] = aVariableProfile.Key;
			return planCellRef;
		}
		#endregion GetStoreBasisElementValue

		#region GetStoreGrpBasisElementValue 
		public double GetStoreGrpBasisElementValue(int aPlanHnRID, int aStoreGrpRID, int aStoreGrpLvlRID, VariableProfile aVariableProfile, TimeTotalVariableProfile aTimeVariableProfile)
		{
			PlanCellReference planCellRef;
			if (aStoreGrpRID != Include.AllStoreFilterRID
				&& aStoreGrpLvlRID != Include.AllStoreTotal)
			{
				planCellRef = GetBasisGroupDateDetailRef(aPlanHnRID, aStoreGrpRID, aVariableProfile, aTimeVariableProfile);
				planCellRef[eProfileType.StoreGroupLevel] = aStoreGrpLvlRID;
			}
			else
			{
				planCellRef = GetBasisGroupDateDetailRef(aPlanHnRID, Include.AllStoreFilterRID, aVariableProfile, aTimeVariableProfile);

			}
			return  planCellRef.GetCellValue(eGetCellMode.Current, true);
		}
		private PlanCellReference GetBasisGroupDateDetailRef(int aPlanHnRID, int aStoreGrpRID, VariableProfile aVariableProfile, TimeTotalVariableProfile aTimeVariableProfile)
		{
			PlanCube planCube;
			if (aStoreGrpRID == Include.AllStoreFilterRID)
			{
				planCube = (PlanCube)this.GetAllocationBasisCubeGroup().GetCube(eCubeType.StoreBasisStoreTotalDateTotal);
			}
			else
			{
				planCube = (PlanCube)this.GetAllocationBasisCubeGroup().GetCube(eCubeType.StoreBasisGroupTotalDateTotal);
			}
			if (planCube == null)
			{ 
				this.OpenAllocationBasisCubeGroup(this.GetAllocationGrandTotalProfile().PlanHnRID, Include.UndefinedDate, Include.UndefinedDate);
				if (aStoreGrpRID == Include.AllStoreFilterRID)
				{
					planCube = (PlanCube)this.GetAllocationBasisCubeGroup().GetCube(eCubeType.StoreBasisStoreTotalDateTotal);
				}
				else
				{
					planCube = (PlanCube)this.GetAllocationBasisCubeGroup().GetCube(eCubeType.StoreBasisGroupTotalDateTotal);
				}
			}
			PlanCellReference planCellRef = new PlanCellReference(planCube);
			planCellRef[eProfileType.Version] = Include.FV_ActionRID;
			planCellRef[eProfileType.HierarchyNode] = aPlanHnRID;
			planCellRef[eProfileType.Basis] = 0;
			planCellRef[eProfileType.QuantityVariable] = PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
			planCellRef[eProfileType.TimeTotalVariable] = aTimeVariableProfile.Key;
			
			planCellRef[eProfileType.Variable] = aVariableProfile.Key;
			return planCellRef;
		}
		#endregion GetStoreGrpBasisElementValue

		#region ReleasePath
		/// <summary>
		/// Gets the release path from the configuration file.
		/// </summary>
		/// <returns>Release Path</returns>
		// (CSMITH) - BEG MID Track #????: Include header type as node in release path
		public string ReleasePath(AllocationProfile aAllocationProfile)
			// (CSMITH) - END MID Track #????
		{
			string releasePath = MIDConfigurationManager.AppSettings["HeaderReleaseFilePath"].TrimEnd('\\');

			if (releasePath == null)
			{
				throw new MIDException(eErrorLevel.fatal,
					(int)eMIDTextCode.msg_al_HeaderReleaseFilePathNotFound,
					MIDText.GetText(eMIDTextCode.msg_al_HeaderReleaseFilePathNotFound));
			}
			else
			{
				// (CSMITH) - BEG MID Track #????: Include header type as node in release path
				if (aAllocationProfile.Receipt)
				{
					releasePath = releasePath + @"\Receipt";
				}
				else if (aAllocationProfile.IsPurchaseOrder)
				{
					releasePath = releasePath + @"\PO";
				}
				else if (aAllocationProfile.ASN)
				{
					releasePath = releasePath + @"\ASN";
				}
				else if (aAllocationProfile.IsDummy)
				{
					releasePath = releasePath + @"\Dummy";
				}
				else if (aAllocationProfile.DropShip)
				{
					releasePath = releasePath + @"\DropShip";
				}
				else if (aAllocationProfile.Reserve)
				{
					releasePath = releasePath + @"\Reserve";
				}
				else if (aAllocationProfile.WorkUpBulkSizeBuy)
				{
					releasePath = releasePath + @"\WorkUpSizeBuy";
				}
				else if (aAllocationProfile.WorkUpTotalBuy)
				{
					releasePath = releasePath + @"\WorkupTotalBuy";
				}
				// BEGIN TT#1401 - stodd - set release path for VSW
				else if (aAllocationProfile.IMO)
				{
					releasePath = releasePath + @"\VSW";
				}
				// END TT#1401 - stodd - set release path for VSW
				else
				{
					releasePath = releasePath + @"\Receipt";
				}
				// (CSMITH) - END MID Track #????

				if (!Directory.Exists(releasePath))
				{
					Directory.CreateDirectory(releasePath);
				}
			}
			return releasePath;
		}

		/// <summary>
		/// Gets the Release File Location
		/// </summary>
		/// <param name="aAllocationProfile">Allocation Profile for the released header</param>
		/// <returns>Release File Location, Path and name</returns>
		public string ReleaseFileLocation(AllocationProfile aAllocationProfile)
		{
			// (CSMITH) - BEG MID Track #????: Include header type as node in release path
			return ReleasePath(aAllocationProfile) + @"\" + aAllocationProfile.HeaderID + ".xml";
			// (CSMITH) - END MID Track #????
		}

		/// <summary>
		/// Deletes the release file for a given allocation profile
		/// </summary>
		/// <param name="aAllocationProfile">Allocation Profile</param>
		public bool DeleteReleaseFile(AllocationProfile aAllocationProfile)
		{
			try
			{
				File.Delete(ReleaseFileLocation(aAllocationProfile));
                // Begin TT#2065 - JSmith - Release XML Trigger Files are not removed on a Reset Action
                string releaseFileTriggerExtension = SAB.ApplicationServerSession.ReleaseFileTriggerExtension;
                if (releaseFileTriggerExtension != null && releaseFileTriggerExtension != string.Empty)
                {
                    File.Delete(ReleaseFileLocation(aAllocationProfile) + "." + releaseFileTriggerExtension);
                }
                // End TT#2065
			}
			catch (Exception e)
			{
				this.SAB.ApplicationServerSession.Audit.Log_Exception(e,this.GetType().Name,eExceptionLogging.logAllInnerExceptions);
				return false;
			}
			return true;
		}
		#endregion ReleasePath

		#region GetStorePctSellThru
		public double GetStorePctSellThru (int aStoreRID)
		{
			int[] storeRIDList = {aStoreRID};
			return GetStorePctSellThru(storeRIDList)[0];
		}
		public double[] GetStorePctSellThru(int[] aStoreRIDList)
		{
			int planHnRID = this.GetAllocationGrandTotalProfile().PlanHnRID;

			if (eOTSPlanLevelType.Regular == this.GetNodeData(planHnRID).OTSPlanLevelType)
			{
				return GetStoreBasisElementValue(planHnRID, aStoreRIDList, PlanComputations.PlanVariables.SellThruPctRegPromoVariable, PlanComputations.PlanTimeTotalVariables.TotalSellThruPctRegPromoVariable);
			}
			else
			{
				return GetStoreBasisElementValue(planHnRID, aStoreRIDList, PlanComputations.PlanVariables.SellThruPctTotalVariable, PlanComputations.PlanTimeTotalVariables.TotalSellThruPctTotalVariable);
			}
		}

		public double GetStorePctSellThruIdx (int aStoreRID)
		{
			int[] storeRIDList = {aStoreRID};
			return GetStorePctSellThruIdx(storeRIDList)[0];
		}
		public double[] GetStorePctSellThruIdx(int[] aStoreRIDList)
		{
			int planHnRID = this.GetAllocationGrandTotalProfile().PlanHnRID;

			if (_velocityStorePctSellThruIdx_Chain == null)
			{
				if (eOTSPlanLevelType.Regular == this.GetNodeData(planHnRID).OTSPlanLevelType)
				{
					_velocityStorePctSellThruIdx_Chain =
						GetStoreBasisElementValue(planHnRID, this.AllStoreRIDList(), PlanComputations.PlanVariables.SellThruPctRegPromoAllStoreIndexVariable, PlanComputations.PlanTimeTotalVariables.TotalSellThruPctRegPromoAllStoreIndexVariable);
				}
				else
				{
					_velocityStorePctSellThruIdx_Chain =
						GetStoreBasisElementValue(planHnRID, this.AllStoreRIDList(), PlanComputations.PlanVariables.SellThruPctTotalAllStoreIndexVariable, PlanComputations.PlanTimeTotalVariables.TotalSellThruPctTotalAllStoreIndexVariable);
				}
			}
			if (_velocityStorePctSellThruIdx_Set == null)
			{
				if (eOTSPlanLevelType.Regular == this.GetNodeData(this.GetAllocationGrandTotalProfile().PlanHnRID).OTSPlanLevelType)
				{
					_velocityStorePctSellThruIdx_Set =
						GetStoreBasisElementValue(planHnRID, this.AllStoreRIDList(), PlanComputations.PlanVariables.SellThruPctRegPromoSetIndexVariable, PlanComputations.PlanTimeTotalVariables.TotalSellThruPctRegPromoSetIndexVariable);
				}
				else
				{
					_velocityStorePctSellThruIdx_Set =
						GetStoreBasisElementValue(planHnRID, this.AllStoreRIDList(), PlanComputations.PlanVariables.SellThruPctTotalSetIndexVariable, PlanComputations.PlanTimeTotalVariables.TotalSellThruPctTotalSetIndexVariable);
				}
			}
			double[] storePctSellThruIdx = new double[aStoreRIDList.Length];
			if (this.Velocity.CalculateAverageUsingChain)
			{
				for (int i=0; i < aStoreRIDList.Length; i++)
				{
					// begin MID Track 5953 Null Reference when executing General Method
					//storePctSellThruIdx[i] = _velocityStorePctSellThruIdx_Chain[StoreIndexRID(aStoreRIDList[i]).Index];
					storePctSellThruIdx[i] = _velocityStorePctSellThruIdx_Chain[GetStoreIndexRID(aStoreRIDList[i]).Index];
					// end MID Track 5953 Null Reference when executing General Method
				}			
			}
			else
			{
				for (int i=0; i < aStoreRIDList.Length; i++)
				{
					// begin MID Track 5953 Null Reference when executing General Method
					//storePctSellThruIdx[i] = _velocityStorePctSellThruIdx_Set[StoreIndexRID(aStoreRIDList[i]).Index];
					storePctSellThruIdx[i] = _velocityStorePctSellThruIdx_Set[GetStoreIndexRID(aStoreRIDList[i]).Index];
					// end MID Track 5953 Null Reference when executing General Method
				}	
			}
			return storePctSellThruIdx;
		}

		public double GetStoreGrpPctSellThru (int aStoreGrpRID, int aStoreGrpLvlRID)
		{
			int planHnRID = this.GetAllocationGrandTotalProfile().PlanHnRID;

			if (eOTSPlanLevelType.Regular == this.GetNodeData(planHnRID).OTSPlanLevelType)
			{
				return GetStoreGrpBasisElementValue(planHnRID, aStoreGrpRID, aStoreGrpLvlRID, PlanComputations.PlanVariables.SellThruPctRegPromoVariable, PlanComputations.PlanTimeTotalVariables.TotalSellThruPctRegPromoVariable);
			}
			else
			{
				return GetStoreGrpBasisElementValue(planHnRID, aStoreGrpRID, aStoreGrpLvlRID, PlanComputations.PlanVariables.SellThruPctTotalVariable, PlanComputations.PlanTimeTotalVariables.TotalSellThruPctTotalVariable);
			}
		}
		#endregion GetStorePctSellThru

		#region Store Eligibility
		/// <summary>
		/// Removes the cached eligibility values 
		/// </summary>
		/// <param name="nodeRID">The record ID of the node</param>
		public void RemoveStoreEligibility(int nodeRID)
		{
			try
			{
				if (_salesEligibilityHashByNodeRID.Contains(nodeRID))
				{
					_salesEligibilityHashByNodeRID.Remove(nodeRID);
				}
				// BEGIN MID Track # 2539 Grades not same in OTS and Allocation
				if (_salesWkRangeEligibilityHashByNodeRID.Contains(nodeRID))
				{
					_salesWkRangeEligibilityHashByNodeRID.Remove(nodeRID);
				}
				// END MID Track # 2539
				if (_stockEligibilityHashByNodeRID.Contains(nodeRID))
				{
					_stockEligibilityHashByNodeRID.Remove(nodeRID);
				}
				if (_priorityShippingHashByNodeRID.Contains(nodeRID))
				{
					_priorityShippingHashByNodeRID.Remove(nodeRID);
				}
				_currentSalesEligibilityNodeRID = -1;
				_currentSalesEligibilityYearWeek = -1;
				_currentStockEligibilityNodeRID = -1;
				_currentStockEligibilityYearWeek = -1;
				_currentPriorityShippingNodeRID = -1;
				_currentPriorityShippingYearWeek = -1;
				_currentSalesWkRangeEligibilityNodeRID = -1;  // MID Track # 2539 Grades not same in OTS and Allocation
				_currentSalesEligibilityWeekRange = -1;       // MID Track # 2539 Grades not same in OTS and Allocation 
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the sales eligibility of a single store for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="storeRID">
		/// The record ID of the store
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which eligibility is to be determined
		/// </param>
		/// <returns>
		/// A boolean identifying if the store is eligible
		/// </returns>
		public bool GetStoreEligibilityForSales(int nodeRID, int storeRID, int yearWeek)
		{
			try
			{
				return DetermineSalesEligibility(nodeRID, storeRID, yearWeek);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the priority shipping status of a single store for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="storeRID">
		/// The record ID of the store
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which priority shipping is to be determined
		/// </param>
		/// <returns>
		/// A boolean identifying if the store is a priority shipper
		/// </returns>
		public bool GetStorePriorityShipping(int nodeRID, int storeRID, int yearWeek)
		{
			try
			{
				return DeterminePriorityShipping(nodeRID, storeRID, yearWeek);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		// BEGIN MID Track # 2539 Grades not same in OTS and ALlocation
		/// <summary>
		/// Returns the sales eligibility for a store in a range of year/weeks
		/// </summary>
		/// <param name="aNodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="aBeginYearWeek">First week in range</param>
		/// <param name="aEndYearWeek">Ending week in range</param>
		/// <returns>
		/// True: Store is eligible for Sales in the specified range of weeks
		/// False: Store is not eligible for Sales in the specified range of weeks
		/// </returns>
		public bool GetStoreEligibilityForSalesInWeekRange(int aStoreRID, int aNodeRID, int aBeginYearWeek, int aEndYearWeek)
		{
			try
			{
				if (aBeginYearWeek < 1000000)  // YYYYWW format
				{
					// convert to YYYYDDD
					aBeginYearWeek = this.SAB.ApplicationServerSession.Calendar.GetWeekKey(aBeginYearWeek); 
				}
				if (aEndYearWeek < 1000000)  //  YYYYWW format
				{
					// convert to YYYYDDD
					aEndYearWeek = this.SAB.ApplicationServerSession.Calendar.GetWeekKey(aEndYearWeek);
				}
				long weekRange = (aBeginYearWeek << 32) + aEndYearWeek;
				if (aNodeRID != _currentSalesWkRangeEligibilityNodeRID)	// if not same node, get year/week Hashtable		
				{
					_salesEligibilityHashByWeekRange = (System.Collections.Hashtable)_salesWkRangeEligibilityHashByNodeRID[aNodeRID];
					if (_salesEligibilityHashByWeekRange == null)
					{
						_salesEligibilityHashByWeekRange = new System.Collections.Hashtable();
						_salesWkRangeEligibilityHashByNodeRID.Add(aNodeRID, _salesEligibilityHashByWeekRange);
					}
					_currentSalesWkRangeEligibilityNodeRID = aNodeRID;
					_currentSalesEligibilityWeekRange = -1;	// reset current yearWeek range since new node
				}
	
				if (weekRange != _currentSalesEligibilityWeekRange)	// if not same week range, get BitArray for year/week
				{
					_salesWkRangeEligibilityBitArray = (System.Collections.BitArray)_salesEligibilityHashByWeekRange[weekRange];
					if (_salesWkRangeEligibilityBitArray == null)
					{
						_salesWkRangeEligibilityBitArray = HierarchySessionTransaction.GetStoreSalesEligibilityFlags(aNodeRID, aBeginYearWeek, aEndYearWeek);
						_salesEligibilityHashByWeekRange.Add(weekRange, _salesWkRangeEligibilityBitArray);
					}
					_currentSalesEligibilityWeekRange = weekRange;
				}

				return _salesWkRangeEligibilityBitArray[aStoreRID];
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		// END MID Track # 2539 Grades not same in OTS and ALlocation

		/// <summary>
		/// Returns the sales eligibility of a list of store and year/week combinations
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="swel">
		/// A ProfileList containing instances of the StoreWeekEligibilityProfile class
		/// </param>
		/// <returns>
		/// The StoreWeekEligibilityList object containing the eligibility settings.
		/// </returns>
		/// <remarks>
		/// This method will not set the store's priority shipping status.
		/// </remarks>
		public StoreWeekEligibilityList GetStoreEligibilityForSales(int nodeRID, StoreWeekEligibilityList swel)
		{
			try
			{
				return GetStoreEligibilityForSales(nodeRID, swel, false);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the sales eligibility of a list of store and year/week combinations
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="swel">
		/// A ProfileList containing instances of the StoreWeekEligibilityProfile class
		/// </param>
		/// <param name="setPriorityShipping">
		/// A flag identifying that the priority shipping is to be determined
		/// </param>
		/// <returns>
		/// The StoreWeekEligibilityList object containing the eligibility settings.
		/// </returns>
		public StoreWeekEligibilityList GetStoreEligibilityForSales(int nodeRID, StoreWeekEligibilityList swel, bool setPriorityShipping)
		{
			try
			{
				foreach(StoreWeekEligibilityProfile swep in swel)
				{
					swep.StoreIsEligible = DetermineSalesEligibility(nodeRID, swep.Key, swep.YearWeek);
					if (setPriorityShipping)
					{
						swep.StoreIsPriorityShipper = DeterminePriorityShipping(nodeRID, swep.Key, swep.YearWeek);
					}
				}
				return swel;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the sales eligibility of all stores for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which eligibility is to be determined
		/// </param>
		/// <returns>
		/// The StoreWeekEligibilityList object containing the eligibility settings.
		/// </returns>
		/// <remarks>
		/// This method will not set the store's priority shipping status.
		/// </remarks>
		public StoreWeekEligibilityList GetStoreEligibilityForSales(int nodeRID, int yearWeek)
		{
			try
			{
				return GetStoreEligibilityForSales(nodeRID, yearWeek, false);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the sales eligibility of all stores for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which eligibility is to be determined
		/// </param>
		/// <param name="setPriorityShipping">
		/// A flag identifying that the priority shipping is to be determined
		/// </param>
		/// <returns>
		/// The StoreWeekEligibilityList object containing the eligibility settings.
		/// </returns>
		public StoreWeekEligibilityList GetStoreEligibilityForSales(int nodeRID, int yearWeek, bool setPriorityShipping)
		{
			try
			{
				StoreWeekEligibilityList swel = new StoreWeekEligibilityList(eProfileType.StoreEligibility);
				StoreWeekEligibilityProfile swep = null;
				ProfileList allStoreList = GetProfileList(eProfileType.Store);
				foreach(StoreProfile sp in allStoreList.ArrayList)
				{
					swep = new StoreWeekEligibilityProfile(sp.Key);
					swep.YearWeek = yearWeek;
					swep.StoreIsEligible = DetermineSalesEligibility(nodeRID, swep.Key, swep.YearWeek);
					if (setPriorityShipping)
					{
						swep.StoreIsPriorityShipper = DeterminePriorityShipping(nodeRID, swep.Key, swep.YearWeek);
					}
					swel.Add(swep);
				}
				return swel;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		
		/// <summary>
		/// Returns the sales eligibility for a provided list of stores for a given node and year/week
		/// </summary>
		/// <param name="storeList">
		/// A ProfileList containing StoreProfiles for the store to determine eligibility
		/// </param>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which eligibility is to be determined
		/// </param>
		/// <returns>
		/// The StoreWeekEligibilityList object containing the eligibility settings.
		/// </returns>
		/// <remarks>
		/// This method will not set the store's priority shipping status.
		/// </remarks>
		public StoreWeekEligibilityList GetStoreEligibilityForSales(ProfileList storeList, int nodeRID, int yearWeek)
		{
			try
			{
				return GetStoreEligibilityForSales(storeList, nodeRID, yearWeek, false);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the sales eligibility for a provided list of stores for a given node and year/week
		/// </summary>
		/// <param name="storeList">
		/// A ProfileList containing StoreProfiles for the store to determine eligibility
		/// </param>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which eligibility is to be determined
		/// </param>
		/// <param name="setPriorityShipping">
		/// A flag identifying that the priority shipping is to be determined
		/// </param>
		/// <returns>
		/// The StoreWeekEligibilityList object containing the eligibility settings.
		/// </returns>
		public StoreWeekEligibilityList GetStoreEligibilityForSales(ProfileList storeList, int nodeRID, int yearWeek, bool setPriorityShipping)
		{
			try
			{
				StoreWeekEligibilityList swel = new StoreWeekEligibilityList(eProfileType.StoreEligibility);
				StoreWeekEligibilityProfile swep = null;
				foreach(StoreProfile sp in storeList.ArrayList)
				{
					swep = new StoreWeekEligibilityProfile(sp.Key);
					swep.YearWeek = yearWeek;
					swep.StoreIsEligible = DetermineSalesEligibility(nodeRID, swep.Key, swep.YearWeek);
					if (setPriorityShipping)
					{
						swep.StoreIsPriorityShipper = DeterminePriorityShipping(nodeRID, swep.Key, swep.YearWeek);
					}
					swel.Add(swep);
				}
				return swel;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Determines the sales eligibility of a store for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="storeRID">
		/// The record ID of the store
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which eligibility is to be determined
		/// </param>
		/// <returns>
		/// A boolean identifying if the store is eligible
		/// </returns>
		/// <remarks>This routine will accept a year/week as either YYYYWW or YYYYDDD</remarks>
		private bool DetermineSalesEligibility(int nodeRID, int storeRID, int yearWeek)
		{
			try
			{
				if (yearWeek < 1000000)  // YYYYWW format
				{
					// convert to YYYYDDD
					yearWeek = this.SAB.ApplicationServerSession.Calendar.GetWeekKey(yearWeek); 
				}

				if (nodeRID != _currentSalesEligibilityNodeRID) // if not same node, get year/week Hashtable		
				{
					_salesEligibilityHashByYearWeek = (System.Collections.Hashtable)_salesEligibilityHashByNodeRID[nodeRID];
					if (_salesEligibilityHashByYearWeek == null)
					{
						_salesEligibilityHashByYearWeek = new System.Collections.Hashtable();
						_salesEligibilityHashByNodeRID.Add(nodeRID, _salesEligibilityHashByYearWeek);
					}
					_currentSalesEligibilityNodeRID = nodeRID;
					_currentSalesEligibilityYearWeek = -1;	// reset current yearWeek since new node
				}
	
				if (yearWeek != _currentSalesEligibilityYearWeek)	// if not same week, get BitArray for year/week
				{
					_salesEligibilityBitArray = (System.Collections.BitArray)_salesEligibilityHashByYearWeek[yearWeek];
					if (_salesEligibilityBitArray == null)
					{
						_salesEligibilityBitArray = HierarchySessionTransaction.GetStoreSalesEligibilityFlags(nodeRID, yearWeek);
						_salesEligibilityHashByYearWeek.Add(yearWeek, _salesEligibilityBitArray);
					}
					_currentSalesEligibilityYearWeek = yearWeek;
				}

                
				return _salesEligibilityBitArray[storeRID];
                
			}
            // Begin TT#1522 - JSmith - WUB Headers failing to load into MID
            catch (System.NullReferenceException)
            {
                return false;
            }
            // End TT#1522
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the stock eligibility of a single store for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="storeRID">
		/// The record ID of the store
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which eligibility is to be determined
		/// </param>
		/// <returns>
		/// A boolean identifying if the store is eligible
		/// </returns>
		public bool GetStoreEligibilityForStock(int nodeRID, int storeRID, int yearWeek)
		{
			try
			{
				return DetermineStockEligibility(nodeRID, storeRID, yearWeek);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the stock eligibility of a list of store and year/week combinations
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="swel">
		/// A ProfileList containing instances of the StoreWeekEligibilityProfile class
		/// </param>
		/// <returns>
		/// The StoreWeekEligibilityList object containing the eligibility settings.
		/// </returns>
		/// <remarks>
		/// This method will not set the store's priority shipping status.
		/// </remarks>
		public StoreWeekEligibilityList GetStoreEligibilityForStock(int nodeRID, StoreWeekEligibilityList swel)
		{
			try
			{
				return GetStoreEligibilityForStock(nodeRID, swel, false);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the stock eligibility of a list of store and year/week combinations
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="swel">
		/// A ProfileList containing instances of the StoreWeekEligibilityProfile class
		/// </param>
		/// <param name="setPriorityShipping">
		/// A flag identifying that the priority shipping is to be determined
		/// </param>
		/// <returns>
		/// The StoreWeekEligibilityList object containing the eligibility settings.
		/// </returns>
		public StoreWeekEligibilityList GetStoreEligibilityForStock(int nodeRID, StoreWeekEligibilityList swel, bool setPriorityShipping)
		{
			try
			{
				foreach(StoreWeekEligibilityProfile swep in swel)
				{
					swep.StoreIsEligible = DetermineStockEligibility(nodeRID, swep.Key, swep.YearWeek);
					if (setPriorityShipping)
					{
						swep.StoreIsPriorityShipper = DeterminePriorityShipping(nodeRID, swep.Key, swep.YearWeek);
					}
				}
				return swel;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the stock eligibility of all stores for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which eligibility is to be determined
		/// </param>
		/// <returns>
		/// The StoreWeekEligibilityList object containing the eligibility settings.
		/// </returns>
		/// <remarks>
		/// This method will not set the store's priority shipping status.
		/// </remarks>
		public StoreWeekEligibilityList GetStoreEligibilityForStock(int nodeRID, int yearWeek)
		{
			try
			{
				return GetStoreEligibilityForStock(nodeRID, yearWeek, false);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the stock eligibility of all stores for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which eligibility is to be determined
		/// </param>
		/// <param name="setPriorityShipping">
		/// A flag identifying that the priority shipping is to be determined
		/// </param>
		/// <returns>
		/// The StoreWeekEligibilityList object containing the eligibility settings.
		/// </returns>
		public StoreWeekEligibilityList GetStoreEligibilityForStock(int nodeRID, int yearWeek, bool setPriorityShipping)
		{
			try
			{
				StoreWeekEligibilityList swel = new StoreWeekEligibilityList(eProfileType.StoreEligibility);
				StoreWeekEligibilityProfile swep = null;
				ProfileList allStoreList = GetProfileList(eProfileType.Store);
				foreach(StoreProfile sp in allStoreList.ArrayList)
				{
					swep = new StoreWeekEligibilityProfile(sp.Key);
					swep.YearWeek = yearWeek;
					swep.StoreIsEligible = DetermineStockEligibility(nodeRID, swep.Key, swep.YearWeek);
					if (setPriorityShipping)
					{
						swep.StoreIsPriorityShipper = DeterminePriorityShipping(nodeRID, swep.Key, swep.YearWeek);
					}
					swel.Add(swep);
				}
				return swel;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the stock eligibility a provided list of stores for a given node and year/week
		/// </summary>
		/// <param name="storeList">
		/// A ProfileList containing StoreProfiles for the store to determine eligibility
		/// </param>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which eligibility is to be determined
		/// </param>
		/// <returns>
		/// The StoreWeekEligibilityList object containing the eligibility settings.
		/// </returns>
		/// <remarks>
		/// This method will not set the store's priority shipping status.
		/// </remarks>
		public StoreWeekEligibilityList GetStoreEligibilityForStock(ProfileList storeList, int nodeRID, int yearWeek)
		{
			try
			{
				return GetStoreEligibilityForStock(storeList, nodeRID, yearWeek, false);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the stock eligibility a provided list of stores for a given node and year/week
		/// </summary>
		/// <param name="storeList">
		/// A ProfileList containing StoreProfiles for the store to determine eligibility
		/// </param>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which eligibility is to be determined
		/// </param>
		/// <param name="setPriorityShipping">
		/// A flag identifying that the priority shipping is to be determined
		/// </param>
		/// <returns>
		/// The StoreWeekEligibilityList object containing the eligibility settings.
		/// </returns>
		public StoreWeekEligibilityList GetStoreEligibilityForStock(ProfileList storeList, int nodeRID, int yearWeek, bool setPriorityShipping)
		{
			try
			{
				StoreWeekEligibilityList swel = new StoreWeekEligibilityList(eProfileType.StoreEligibility);
				StoreWeekEligibilityProfile swep = null;
				foreach(StoreProfile sp in storeList.ArrayList)
				{
					swep = new StoreWeekEligibilityProfile(sp.Key);
					swep.YearWeek = yearWeek;
					swep.StoreIsEligible = DetermineStockEligibility(nodeRID, swep.Key, swep.YearWeek);
					if (setPriorityShipping)
					{
						swep.StoreIsPriorityShipper = DeterminePriorityShipping(nodeRID, swep.Key, swep.YearWeek);
					}
					swel.Add(swep);
				}
				return swel;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Determines the stock eligibility of a store for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="storeRID">
		/// The record ID of the store
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which eligibility is to be determined
		/// </param>
		/// <returns>
		/// A boolean identifying if the store is eligible
		/// </returns>
		/// <remarks>This routine will accept a year/week as either YYYYWW or YYYYDDD</remarks>
		private bool DetermineStockEligibility(int nodeRID, int storeRID, int yearWeek)
		{
			try
			{
				if (yearWeek < 1000000)  // YYYYWW format
				{
					// convert to YYYYDDD
					yearWeek = this.SAB.ApplicationServerSession.Calendar.GetWeekKey(yearWeek); 
				}

				if (nodeRID != _currentStockEligibilityNodeRID)	// if not same node, get year/week Hashtable		
				{
					_stockEligibilityHashByYearWeek = (System.Collections.Hashtable)_stockEligibilityHashByNodeRID[nodeRID];
					if (_stockEligibilityHashByYearWeek == null)
					{
						_stockEligibilityHashByYearWeek = new System.Collections.Hashtable();
						_stockEligibilityHashByNodeRID.Add(nodeRID, _stockEligibilityHashByYearWeek);
					}
					_currentStockEligibilityNodeRID = nodeRID;
					_currentStockEligibilityYearWeek = -1;	// reset current yearWeek since new node
				}
	
				if (yearWeek != _currentStockEligibilityYearWeek)	// if not same week, get BitArray for year/week
				{
					_stockEligibilityBitArray = (System.Collections.BitArray)_stockEligibilityHashByYearWeek[yearWeek];
					if (_stockEligibilityBitArray == null)
					{
						_stockEligibilityBitArray = HierarchySessionTransaction.GetStoreStockEligibilityFlags(nodeRID, yearWeek);
						_stockEligibilityHashByYearWeek.Add(yearWeek, _stockEligibilityBitArray);
					}
					_currentStockEligibilityYearWeek = yearWeek;
				}

				return _stockEligibilityBitArray[storeRID];
			}
            // Begin TT#1522 - JSmith - WUB Headers failing to load into MID
            catch (System.NullReferenceException)
            {
                return false;
            }
            // End TT#1522
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Determines the priority shipping status of a store for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="storeRID">
		/// The record ID of the store
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which priority shipping is to be determined
		/// </param>
		/// <returns>
		/// A boolean identifying if the store is a priority shipper
		/// </returns>
		/// <remarks>This routine will accept a year/week as either YYYYWW or YYYYDDD</remarks>
		private bool DeterminePriorityShipping(int nodeRID, int storeRID, int yearWeek)
		{
			try
			{
				if (yearWeek < 1000000)  // YYYYWW format
				{
					// convert to YYYYDDD
					yearWeek = this.SAB.ApplicationServerSession.Calendar.GetWeekKey(yearWeek); 
				}

				if (nodeRID != _currentPriorityShippingNodeRID)	// if not same node, get year/week Hashtable		
				{
					_priorityShippingHashByYearWeek = (System.Collections.Hashtable)_priorityShippingHashByNodeRID[nodeRID];
					if (_priorityShippingHashByYearWeek == null)
					{
						_priorityShippingHashByYearWeek = new System.Collections.Hashtable();
						_priorityShippingHashByNodeRID.Add(nodeRID, _priorityShippingHashByYearWeek);
					}
					_currentPriorityShippingNodeRID = nodeRID;
					_currentPriorityShippingYearWeek = -1;	// reset current yearWeek since new node
				}
	
				if (yearWeek != _currentPriorityShippingYearWeek)	// if not same week, get BitArray for year/week
				{
					_priorityShippingBitArray = (System.Collections.BitArray)_priorityShippingHashByYearWeek[yearWeek];
					if (_priorityShippingBitArray == null)
					{
						_priorityShippingBitArray = HierarchySessionTransaction.GetStorePriorityShippingFlags(nodeRID, yearWeek);
						_priorityShippingHashByYearWeek.Add(yearWeek, _priorityShippingBitArray);
					}
					_currentPriorityShippingYearWeek = yearWeek;
				}

				return _priorityShippingBitArray[storeRID];
			}
            // Begin TT#1522 - JSmith - WUB Headers failing to load into MID
            catch (System.NullReferenceException)
            {
                return false;
            }
            // End TT#1522
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		#endregion Store Eligibility

		#region Store Grade
		/// <summary>
		/// Returns the store grade list for the node
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <returns>
		/// The StoreGradeList object containing the store grade settings.
		/// </returns>
		public StoreGradeList GetStoreGradeList(int nodeRID)
		{
			try
			{
				//				if (_storeGradesHash.Contains(nodeRID))
				//				{
				//					return (StoreGradeList)_storeGradesHash[nodeRID];
				//				}
				//				else
				//				{
				//					StoreGradeList sgl = SAB.HierarchyServerSession.GetStoreGradeList(nodeRID, false);
				//					if (sgl.Count > 0)
				//					{
				//						_storeGradesHash.Add(nodeRID, sgl);
				//					}
				//					return sgl;
				//				}
				StoreGradeList sgl = (StoreGradeList)_storeGradesHash[nodeRID];

				if (sgl == null)
				{
					sgl = SAB.HierarchyServerSession.GetStoreGradeList(nodeRID, false);
					_storeGradesHash.Add(nodeRID, sgl);
				}

				return sgl;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		#endregion Store Grade

		#region Velocity Grade
		/// <summary>
		/// Returns the velocity grade list for the node
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <returns>
		/// The VelocityGradeList object containing the velocity grade settings.
		/// </returns>
		public VelocityGradeList GetVelocityGradeList(int nodeRID)
		{
			try
			{
				if (_velocityGradesHash.Contains(nodeRID))
				{
					return (VelocityGradeList)_velocityGradesHash[nodeRID];
				}
				else
				{
					VelocityGradeList vgl = SAB.HierarchyServerSession.GetVelocityGradeList(nodeRID, false);
					_velocityGradesHash.Add(nodeRID, vgl);
					return vgl;
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		#endregion Velocity Grade

		#region Sell Thru Percentages
		/// <summary>
		/// Returns the sell thru percentages list for the node
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <returns>
		/// The SellThruPctList object containing instances of the SellThruPctProfile class.
		/// </returns>
		public SellThruPctList GetSellThruPctList(int nodeRID)
		{
			try
			{
				if (_sellThruPctsHash.Contains(nodeRID))
				{
					return (SellThruPctList)_sellThruPctsHash[nodeRID];
				}
				else
				{
					SellThruPctList stpl = SAB.HierarchyServerSession.GetSellThruPctList(nodeRID, false);
					_sellThruPctsHash.Add(nodeRID, stpl);
					return stpl;
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		#endregion Sell Thru Percentages

		#region Store Capacity
		/// <summary>
		/// Returns the store capacity list for the node
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="stopOnFind">
		/// This switch tells the routine to stop when the first node with capacities is found.  If set to
		/// true, stores not found will be set to the maximum value for an integer.
		/// </param>
		/// <returns>
		/// The StoreCapacityList object containing instances of the StoreCapacityProfile class.
		/// </returns>
		public StoreCapacityList GetStoreCapacityList(int nodeRID, bool stopOnFind)
		{
			try
			{
				if (_storeCapacityHash.Contains(nodeRID))
				{
					return (StoreCapacityList)_storeCapacityHash[nodeRID];
				}
				else
				{
					StoreCapacityList scl = SAB.HierarchyServerSession.GetStoreCapacityList(GetProfileList(eProfileType.Store), nodeRID, stopOnFind, false);
					_storeCapacityHash.Add(nodeRID, scl);
					return scl;
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		#endregion Store Capacity

		#region Daily Percentages
		/// <summary>
		/// Returns the daily percentages of a single store for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="storeRID">
		/// The record ID of the store
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which daily percentages is to be determined
		/// </param>
		/// <returns>
		/// An instance of the StoreWeekDailyPercentagesProfile class containing the percentages for the week
		/// </returns>
		public StoreWeekDailyPercentagesProfile GetStoreDailyPercentages(int nodeRID, int storeRID, int yearWeek)
		{
			try
			{
				return DetermineDailyPercentages(nodeRID, storeRID, yearWeek);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the daily percentages of a list of store and year/week combinations
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="swdpl">
		/// A ProfileList containing instances of the StoreWeekDailyPercentagesProfile class
		/// </param>
		/// <returns>
		/// The StoreWeekDailyPercentagesList object containing instances of the StoreWeekDailyPercentagesProfile class.
		/// </returns>
		public StoreWeekDailyPercentagesList GetStoreDailyPercentages(int nodeRID, StoreWeekDailyPercentagesList swdpl)
		{
			try
			{
				foreach(StoreWeekDailyPercentagesProfile swdpp in swdpl)
				{
					StoreWeekDailyPercentagesProfile updated_swdpp = DetermineDailyPercentages(nodeRID, swdpp.Key, swdpp.YearWeek);
					swdpp.DailyPercentages = updated_swdpp.DailyPercentages;
				}
				return swdpl;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the daily percentages of all stores for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which daily percentages are to be determined
		/// </param>
		/// <returns>
		/// The StoreWeekDailyPercentagesList object containing instances of the StoreWeekDailyPercentagesProfile class.
		/// </returns>
		public StoreWeekDailyPercentagesList GetStoreDailyPercentages(int nodeRID, int yearWeek)
		{
			try
			{
				StoreWeekDailyPercentagesList swdpl = new StoreWeekDailyPercentagesList(eProfileType.DailyPercentages);
				StoreWeekDailyPercentagesProfile swdpp = null;
				ProfileList allStoreList = GetProfileList(eProfileType.Store);
				foreach(StoreProfile sp in allStoreList.ArrayList)
				{
					swdpp = new StoreWeekDailyPercentagesProfile(sp.Key);
					swdpp.YearWeek = yearWeek;
					swdpp = DetermineDailyPercentages(nodeRID, sp.Key, swdpp.YearWeek);
					swdpl.Add(swdpp);
				}
				return swdpl;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the daily percentages for a provided list of stores for a given node and year/week
		/// </summary>
		/// <param name="storeList">
		/// A ProfileList containing StoreProfiles for the store to determine daily percentages
		/// </param>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which daily percentages is to be determined
		/// </param>
		/// <returns>
		/// The StoreWeekDailyPercentagesList object containing instances of the StoreWeekDailyPercentagesProfile class.
		/// </returns>
		public StoreWeekDailyPercentagesList GetStoreDailyPercentages(ProfileList storeList, int nodeRID, int yearWeek)
		{
			try
			{
				StoreWeekDailyPercentagesList swdpl = new StoreWeekDailyPercentagesList(eProfileType.DailyPercentages);
				StoreWeekDailyPercentagesProfile swdpp = null;
				foreach(StoreProfile sp in storeList.ArrayList)
				{
					swdpp = new StoreWeekDailyPercentagesProfile(sp.Key);
					swdpp.YearWeek = yearWeek;
					swdpp = DetermineDailyPercentages(nodeRID, sp.Key, swdpp.YearWeek);
					swdpl.Add(swdpp);
				}
				return swdpl;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Determines the daily percentages of a store for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="storeRID">
		/// The record ID of the store
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which daily percentages are to be determined
		/// </param>
		/// <returns>
		/// An instance of the StoreWeekDailyPercentagesProfile class containing the daily percentages
		/// </returns>
		/// <remarks>This routine will accept a year/week as either YYYYWW or YYYYDDD</remarks>
		private StoreWeekDailyPercentagesProfile DetermineDailyPercentages(int nodeRID, int storeRID, int yearWeek)
		{
			try
			{
				// begin MID Track 4461 Index out of Range Error / Null Reference when using Daily Percentages
				//if (yearWeek < 1000000)  // YYYYWW format
				//{
				//	// convert to YYYYDDD
				//	yearWeek = this.SAB.ApplicationServerSession.Calendar.GetWeekKey(yearWeek); 
				//}
				if (yearWeek >= 1000000) // format is YYYYDDD
				{
					// convert to YYYYWW
					yearWeek = this.SAB.ApplicationServerSession.Calendar.GetWeek(yearWeek).YearWeek;
				}
				// end MID Track 4461 Index out of Range Error / Null Reference when using Daily Percentages

				if (nodeRID != _currentDailyPercentagesNodeRID)	// if not same node, get year/week Hashtable		
				{
					_dailyPercentagesHashByYearWeek = (System.Collections.Hashtable)_dailyPercentagesHashByNodeRID[nodeRID];
					if (_dailyPercentagesHashByYearWeek == null)
					{
						_dailyPercentagesHashByYearWeek = new System.Collections.Hashtable();
						_dailyPercentagesHashByNodeRID.Add(nodeRID, _dailyPercentagesHashByYearWeek);
					}
					_currentDailyPercentagesNodeRID = nodeRID;
					_currentDailyPercentagesYearWeek = -1;	// reset current yearWeek since new node
				}
	
				if (yearWeek != _currentDailyPercentagesYearWeek)	// if not same week, get BitArray for year/week
				{
					_storeWeekDailyPercentagesList = (StoreWeekDailyPercentagesList)_dailyPercentagesHashByYearWeek[yearWeek];
					if (_storeWeekDailyPercentagesList == null)
					{
						_storeWeekDailyPercentagesList = SAB.HierarchyServerSession.GetStoreDailyPercentages(nodeRID, yearWeek);
						_dailyPercentagesHashByYearWeek.Add(yearWeek, _storeWeekDailyPercentagesList);
					}
					_currentDailyPercentagesYearWeek = yearWeek;
				}

                if (_audit.LoggingLevel == eMIDMessageLevel.Debug)
                {
                    ProfileList allStoreList = GetProfileList(eProfileType.Store);
                    StoreProfile sp = (StoreProfile)allStoreList.FindKey(storeRID);
                    StoreWeekDailyPercentagesProfile swdpp = (StoreWeekDailyPercentagesProfile)_storeWeekDailyPercentagesList.FindKey(storeRID);
                    string message = "Daily Percentages for Node=" + SAB.HierarchyServerSession.GetNodeID(nodeRID) + ";Store=" + sp.StoreId + ";YearWeek=" + yearWeek
                        + ";Day1=" + swdpp.DailyPercentages[0].ToString()
                        + ";Day2=" + swdpp.DailyPercentages[1].ToString()
                        + ";Day3=" + swdpp.DailyPercentages[2].ToString()
                        + ";Day4=" + swdpp.DailyPercentages[3].ToString()
                        + ";Day5=" + swdpp.DailyPercentages[4].ToString()
                        + ";Day6=" + swdpp.DailyPercentages[5].ToString()
                        + ";Day7=" + swdpp.DailyPercentages[6].ToString();
                    this._audit.Add_Msg(eMIDMessageLevel.Debug,
                                        message,
                                        this.GetType().Name);
                }

				return (StoreWeekDailyPercentagesProfile)_storeWeekDailyPercentagesList.FindKey(storeRID);
			}
            // Begin TT#1522 - JSmith - WUB Headers failing to load into MID
            catch (System.NullReferenceException)
            {
                return new StoreWeekDailyPercentagesProfile(storeRID);
            }
            // End TT#1522
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		#endregion Daily Percentages

		#region Similar Stores
		/// <summary>
		/// Returns the similar stores list for the node
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <returns>
		/// The SimilarStoresList object containing instances of the SimilarStoresProfile class.
		/// </returns>
		public SimilarStoreList GetSimilarStoreList(int nodeRID)
		{
			try
			{
				if (_similarStoreHash.Contains(nodeRID))
				{
					return (SimilarStoreList)_similarStoreHash[nodeRID];
				}
				else
				{
					SimilarStoreList ssl = SAB.HierarchyServerSession.GetSimilarStoreList(GetProfileList(eProfileType.Store), nodeRID);
					_similarStoreHash.Add(nodeRID, ssl);
					return ssl;
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		#endregion Similar Stores

		#region Store Sales,Stock and FWOS Modifier
		/// <summary>
		/// Removes the cached modifier values 
		/// </summary>
		/// <param name="nodeRID">The record ID of the node</param>
		public void RemoveStoreModifier(int nodeRID)
		{
			try
			{
				if (SalesModifierHashByNodeRID.Contains(nodeRID))
				{
					SalesModifierHashByNodeRID.Remove(nodeRID);
				}

				if (StockModifierHashByNodeRID.Contains(nodeRID))
				{
					StockModifierHashByNodeRID.Remove(nodeRID);
				}
				_currentSalesModifierNodeRID = -1;
				_currentSalesModifierYearWeek = -1;
				_currentStockModifierNodeRID = -1;
				_currentStockModifierYearWeek = -1;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the sales modifier of a single store for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="storeRID">
		/// The record ID of the store
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which modifier is to be determined
		/// </param>
		/// <returns>
		/// A double identifying the sales modifier percent for the store
		/// </returns>
		public double GetStoreModifierForSales(int nodeRID, int storeRID, int yearWeek)
		{
			try
			{
				return DetermineSalesModifier(nodeRID, storeRID, yearWeek);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the sales modifier of a list of store and year/week combinations
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="swml">
		/// A ProfileList containing instances of the StoreWeekModifierProfile class
		/// </param>
		/// <returns>
		/// The StoreWeekModifierList object containing the modifier settings.
		/// </returns>
		public StoreWeekModifierList GetStoreModifierForSales(int nodeRID, StoreWeekModifierList swml)
		{
			try
			{
				foreach(StoreWeekModifierProfile swmp in swml)
				{
					swmp.StoreModifier = DetermineSalesModifier(nodeRID, swmp.Key, swmp.YearWeek);
				}
				return swml;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the sales modifier of all stores for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which modifier is to be determined
		/// </param>
		/// <returns>
		/// The StoreWeekModifierList object containing the modifier settings.
		/// </returns>
		public StoreWeekModifierList GetStoreModifierForSales(int nodeRID, int yearWeek)
		{
			try
			{
				StoreWeekModifierList swml = new StoreWeekModifierList(eProfileType.StoreModifier);
				StoreWeekModifierProfile swmp = null;
				ProfileList allStoreList = GetProfileList(eProfileType.Store);
				foreach(StoreProfile sp in allStoreList.ArrayList)
				{
					swmp = new StoreWeekModifierProfile(sp.Key);
					swmp.YearWeek = yearWeek;
					swmp.StoreModifier = DetermineSalesModifier(nodeRID, swmp.Key, swmp.YearWeek);
					swml.Add(swmp);
				}
				return swml;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the sales modifier for a provided list of stores for a given node and year/week
		/// </summary>
		/// <param name="storeList">
		/// A ProfileList containing StoreProfiles for the store to determine Modifier
		/// </param>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which modifier is to be determined
		/// </param>
		/// <returns>
		/// The StoreWeekModifierList object containing the modifier settings.
		/// </returns>
		public StoreWeekModifierList GetStoreModifierForSales(ProfileList storeList, int nodeRID, int yearWeek)
		{
			try
			{
				StoreWeekModifierList swml = new StoreWeekModifierList(eProfileType.StoreModifier);
				StoreWeekModifierProfile swmp = null;
				foreach(StoreProfile sp in storeList.ArrayList)
				{
					swmp = new StoreWeekModifierProfile(sp.Key);
					swmp.YearWeek = yearWeek;
					swmp.StoreModifier = DetermineSalesModifier(nodeRID, swmp.Key, swmp.YearWeek);
					swml.Add(swmp);
				}
				return swml;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Determines the sales modifier of a store for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="storeRID">
		/// The record ID of the store
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which modifier is to be determined
		/// </param>
		/// <returns>
		/// A double identifying the sales modifier percent for the store
		/// </returns>
		/// <remarks>This routine will accept a year/week as either YYYYWW or YYYYDDD</remarks>
		private double DetermineSalesModifier(int nodeRID, int storeRID, int yearWeek)
		{
			try
			{
				if (yearWeek < 1000000)  // YYYYWW format
				{
					// convert to YYYYDDD
					yearWeek = this.SAB.ApplicationServerSession.Calendar.GetWeekKey(yearWeek); 
				}

				if (nodeRID != _currentSalesModifierNodeRID)	// if not same node, get year/week Hashtable		
				{
					_salesModifierHashByYearWeek = (System.Collections.Hashtable)SalesModifierHashByNodeRID[nodeRID];
					if (_salesModifierHashByYearWeek == null)
					{
						_salesModifierHashByYearWeek = new System.Collections.Hashtable();
						SalesModifierHashByNodeRID.Add(nodeRID, _salesModifierHashByYearWeek);
					}
					_currentSalesModifierNodeRID = nodeRID;
					_currentSalesModifierYearWeek = -1;	// reset current yearWeek since new node
				}
	
				if (yearWeek != _currentSalesModifierYearWeek)	// if not same week, get Hashtable for year/week
				{
					_salesModifierHash = (System.Collections.Hashtable)_salesModifierHashByYearWeek[yearWeek];
					if (_salesModifierHash == null)
					{
						_salesModifierHash = HierarchySessionTransaction.GetStoreSalesModifierPercents(nodeRID, yearWeek);
						_salesModifierHashByYearWeek.Add(yearWeek, _salesModifierHash);
					}
					_currentSalesModifierYearWeek = yearWeek;
				}

				if (_salesModifierHash.Contains(storeRID))
				{
					return Convert.ToDouble(_salesModifierHash[storeRID], CultureInfo.CurrentUICulture);
				}
				else
				{
					return Include.DefaultModifier;
				}

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the stock modifier of a single store for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="storeRID">
		/// The record ID of the store
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which modifier is to be determined
		/// </param>
		/// <returns>
		/// A double identifying the stock modifier percent for the store
		/// </returns>
		public double GetStoreModifierForStock(int nodeRID, int storeRID, int yearWeek)
		{
			try
			{
				return DetermineStockModifier(nodeRID, storeRID, yearWeek);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the stock modifier of a list of store and year/week combinations
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="swml">
		/// A ProfileList containing instances of the StoreWeekModifierProfile class
		/// </param>
		/// <returns>
		/// The StoreWeekModifierList object containing the modifier settings.
		/// </returns>
		public StoreWeekModifierList GetStoreModifierForStock(int nodeRID, StoreWeekModifierList swml)
		{
			try
			{
				foreach(StoreWeekModifierProfile swmp in swml)
				{
					swmp.StoreModifier = DetermineStockModifier(nodeRID, swmp.Key, swmp.YearWeek);
				}
				return swml;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the stock modifier of all stores for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which modifier is to be determined
		/// </param>
		/// <returns>
		/// The StoreWeekModifierList object containing the modifier settings.
		/// </returns>
		public StoreWeekModifierList GetStoreModifierForStock(int nodeRID, int yearWeek)
		{
			try
			{
				StoreWeekModifierList swml = new StoreWeekModifierList(eProfileType.StoreModifier);
				StoreWeekModifierProfile swmp = null;
				ProfileList allStoreList = GetProfileList(eProfileType.Store);
				foreach(StoreProfile sp in allStoreList.ArrayList)
				{
					swmp = new StoreWeekModifierProfile(sp.Key);
					swmp.YearWeek = yearWeek;
					swmp.StoreModifier = DetermineStockModifier(nodeRID, swmp.Key, swmp.YearWeek);
					swml.Add(swmp);
				}
				return swml;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the stock modifier a provided list of stores for a given node and year/week
		/// </summary>
		/// <param name="storeList">
		/// A ProfileList containing StoreProfiles for the store to determine Modifier
		/// </param>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which modifier is to be determined
		/// </param>
		/// <returns>
		/// The StoreWeekModifierList object containing the modifier settings.
		/// </returns>
		public StoreWeekModifierList GetStoreModifierForStock(ProfileList storeList, int nodeRID, int yearWeek)
		{
			try
			{
				StoreWeekModifierList swml = new StoreWeekModifierList(eProfileType.StoreModifier);
				StoreWeekModifierProfile swmp = null;
				foreach(StoreProfile sp in storeList.ArrayList)
				{
					swmp = new StoreWeekModifierProfile(sp.Key);
					swmp.YearWeek = yearWeek;
					swmp.StoreModifier = DetermineStockModifier(nodeRID, swmp.Key, swmp.YearWeek);
					swml.Add(swmp);
				}
				return swml;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Determines the stock modifier of a store for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="storeRID">
		/// The record ID of the store
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which modifier is to be determined
		/// </param>
		/// <returns>
		/// A double identifying the stock modifier percent for the store
		/// </returns>
		/// <remarks>This routine will accept a year/week as either YYYYWW or YYYYDDD</remarks>
		private double DetermineStockModifier(int nodeRID, int storeRID, int yearWeek)
		{
			try
			{
				if (yearWeek < 1000000)  // YYYYWW format
				{
					// convert to YYYYDDD
					yearWeek = this.SAB.ApplicationServerSession.Calendar.GetWeekKey(yearWeek); 
				}

				if (nodeRID != _currentStockModifierNodeRID)	// if not same node, get year/week Hashtable		
				{
					_stockModifierHashByYearWeek = (System.Collections.Hashtable)StockModifierHashByNodeRID[nodeRID];
					if (_stockModifierHashByYearWeek == null)
					{
						_stockModifierHashByYearWeek = new System.Collections.Hashtable();
						StockModifierHashByNodeRID.Add(nodeRID, _stockModifierHashByYearWeek);
					}
					_currentStockModifierNodeRID = nodeRID;
					_currentStockModifierYearWeek = -1;	// reset current yearWeek since new node
				}
	
				if (yearWeek != _currentStockModifierYearWeek)	// if not same week, get Hashtable for year/week
				{
					_stockModifierHash = (System.Collections.Hashtable)_stockModifierHashByYearWeek[yearWeek];
					if (_stockModifierHash == null)
					{
						_stockModifierHash = HierarchySessionTransaction.GetStoreStockModifierPercents(nodeRID, yearWeek);
						_stockModifierHashByYearWeek.Add(yearWeek, _stockModifierHash);
					}
					_currentStockModifierYearWeek = yearWeek;
				}

				if (_stockModifierHash.Contains(storeRID))
				{
					return Convert.ToDouble(_stockModifierHash[storeRID], CultureInfo.CurrentUICulture);
				}
				else
				{
					return Include.DefaultModifier;
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		// BEGIN MID Track #4370 - John Smith - FWOS Models
		/// <summary>
		/// Returns the FWOS modifier of a single store for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="storeRID">
		/// The record ID of the store
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which modifier is to be determined
		/// </param>
		/// <returns>
		/// A double identifying the FWOS modifier percent for the store
		/// </returns>
		public double GetStoreModifierForFWOS(int nodeRID, int storeRID, int yearWeek)
		{
			try
			{
				return DetermineFWOSModifier(nodeRID, storeRID, yearWeek);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the FWOS modifier of a list of store and year/week combinations
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="swml">
		/// A ProfileList containing instances of the StoreWeekModifierProfile class
		/// </param>
		/// <returns>
		/// The StoreWeekModifierList object containing the modifier settings.
		/// </returns>
		public StoreWeekModifierList GetStoreModifierForFWOS(int nodeRID, StoreWeekModifierList swml)
		{
			try
			{
				foreach(StoreWeekModifierProfile swmp in swml)
				{
					swmp.StoreModifier = DetermineFWOSModifier(nodeRID, swmp.Key, swmp.YearWeek);
				}
				return swml;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the FWOS modifier of all stores for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which modifier is to be determined
		/// </param>
		/// <returns>
		/// The StoreWeekModifierList object containing the modifier settings.
		/// </returns>
		public StoreWeekModifierList GetStoreModifierForFWOS(int nodeRID, int yearWeek)
		{
			try
			{
				StoreWeekModifierList swml = new StoreWeekModifierList(eProfileType.StoreModifier);
				StoreWeekModifierProfile swmp = null;
				ProfileList allStoreList = GetProfileList(eProfileType.Store);
				foreach(StoreProfile sp in allStoreList.ArrayList)
				{
					swmp = new StoreWeekModifierProfile(sp.Key);
					swmp.YearWeek = yearWeek;
					swmp.StoreModifier = DetermineFWOSModifier(nodeRID, swmp.Key, swmp.YearWeek);
					swml.Add(swmp);
				}
				return swml;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the FWOS modifier a provided list of stores for a given node and year/week
		/// </summary>
		/// <param name="storeList">
		/// A ProfileList containing StoreProfiles for the store to determine Modifier
		/// </param>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which modifier is to be determined
		/// </param>
		/// <returns>
		/// The StoreWeekModifierList object containing the modifier settings.
		/// </returns>
		public StoreWeekModifierList GetStoreModifierForFWOS(ProfileList storeList, int nodeRID, int yearWeek)
		{
			try
			{
				StoreWeekModifierList swml = new StoreWeekModifierList(eProfileType.StoreModifier);
				StoreWeekModifierProfile swmp = null;
				foreach(StoreProfile sp in storeList.ArrayList)
				{
					swmp = new StoreWeekModifierProfile(sp.Key);
					swmp.YearWeek = yearWeek;
					swmp.StoreModifier = DetermineFWOSModifier(nodeRID, swmp.Key, swmp.YearWeek);
					swml.Add(swmp);
				}
				return swml;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Determines the FWOS modifier of a store for a given node and year/week
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="storeRID">
		/// The record ID of the store
		/// </param>
		/// <param name="yearWeek">
		/// The year/week for which modifier is to be determined
		/// </param>
		/// <returns>
		/// A double identifying the FWOS modifier percent for the store
		/// </returns>
		/// <remarks>This routine will accept a year/week as either YYYYWW or YYYYDDD</remarks>
		private double DetermineFWOSModifier(int nodeRID, int storeRID, int yearWeek)
		{
			try
			{
				if (yearWeek < 1000000)  // YYYYWW format
				{
					// convert to YYYYDDD
					yearWeek = this.SAB.ApplicationServerSession.Calendar.GetWeekKey(yearWeek); 
				}

				if (nodeRID != _currentFWOSModifierNodeRID)	// if not same node, get year/week Hashtable		
				{
					_FWOSModifierHashByYearWeek = (System.Collections.Hashtable)FWOSModifierHashByNodeRID[nodeRID];
					if (_FWOSModifierHashByYearWeek == null)
					{
						_FWOSModifierHashByYearWeek = new System.Collections.Hashtable();
						FWOSModifierHashByNodeRID.Add(nodeRID, _FWOSModifierHashByYearWeek);
					}
					_currentFWOSModifierNodeRID = nodeRID;
					_currentFWOSModifierYearWeek = -1;	// reset current yearWeek since new node
				}
	
				if (yearWeek != _currentFWOSModifierYearWeek)	// if not same week, get Hashtable for year/week
				{
					_FWOSModifierHash = (System.Collections.Hashtable)_FWOSModifierHashByYearWeek[yearWeek];
					if (_FWOSModifierHash == null)
					{
						_FWOSModifierHash = HierarchySessionTransaction.GetStoreFWOSModifierPercents(nodeRID, yearWeek);
						_FWOSModifierHashByYearWeek.Add(yearWeek, _FWOSModifierHash);
					}
					_currentFWOSModifierYearWeek = yearWeek;
				}

				if (_FWOSModifierHash.Contains(storeRID))
				{
					return Convert.ToDouble(_FWOSModifierHash[storeRID], CultureInfo.CurrentUICulture);
				}
				else
				{
					return Double.MinValue;
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		// END MID Track #4370

		#endregion Store Sales,Stock and FWOS Modifier

		// BEGIN MID Track #4827 - John Smith - Presentation plus sales
		#region Presentation Minimum Plus Sales
		/// <summary>
		/// Removes the cached presentation minimum plus sales values 
		/// </summary>
		/// <param name="nodeRID">The record ID of the node</param>
		public void RemoveStorePMPlusSales(int nodeRID)
		{
			try
			{
				if (PMPlusSalesHashByNodeRID.Contains(nodeRID))
				{
					PMPlusSalesHashByNodeRID.Remove(nodeRID);
				}

				_currentPMPlusSalesNodeRID = -1;
				
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the presentation minimum plus sales indicator of a single store for a given node
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="storeRID">
		/// The record ID of the store
		/// </param>
		/// <returns>
		/// A flag identifying if the presentation minimum plus sales option is to be used for the node/store
		/// </returns>
		public bool GetStorePMPlusSales(int nodeRID, int storeRID)
		{
			try
			{
				return DeterminePMPlusSales(nodeRID, storeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the presentation minimum plus sales indicator of a list of store for a given node
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
        /// <param name="spmpsl">
		/// A ProfileList containing instances of the StorePMPlusSalesProfile class
		/// </param>
		/// <returns>
		/// The StorePMPlusSalesList object containing the presentation minimum plus sales settings.
		/// </returns>
		public StorePMPlusSalesList GetStorePMPlusSales(int nodeRID, StorePMPlusSalesList spmpsl)
		{
			try
			{
				foreach(StorePMPlusSalesProfile spmpsp in spmpsl)
				{
					spmpsp.ApplyPMPlusSales = DeterminePMPlusSales(nodeRID, spmpsp.Key);
				}
				return spmpsl;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the presentation minimum plus sales indicator of all stores for a given node
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <returns>
		/// The StorePMPlusSalesList object containing the presentation minimum plus sales settings.
		/// </returns>
		public StorePMPlusSalesList GetStorePMPlusSales(int nodeRID)
		{
			try
			{
				StorePMPlusSalesList spmpsl = new StorePMPlusSalesList(eProfileType.PMPlusSales);
				StorePMPlusSalesProfile spmpsp = null;
				ProfileList allStoreList = GetProfileList(eProfileType.Store);
				foreach(StoreProfile sp in allStoreList.ArrayList)
				{
					spmpsp = new StorePMPlusSalesProfile(sp.Key);
					spmpsp.ApplyPMPlusSales = DeterminePMPlusSales(nodeRID, spmpsp.Key);
					spmpsl.Add(spmpsp);
				}
				return spmpsl;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the presentation minimum plus sales indicator for a provided list of stores for a given node
		/// </summary>
		/// <param name="storeList">
		/// A ProfileList containing StoreProfiles for the store to determine presentation minimum plus sales settings
		/// </param>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <returns>
		/// The StoreWeekModifierList object containing the modifier settings.
		/// </returns>
		public StorePMPlusSalesList GetStorePMPlusSales(ProfileList storeList, int nodeRID)
		{
			try
			{
				StorePMPlusSalesList spmpsl = new StorePMPlusSalesList(eProfileType.PMPlusSales);
				StorePMPlusSalesProfile spmpsp = null;
				foreach(StoreProfile sp in storeList.ArrayList)
				{
					spmpsp = new StorePMPlusSalesProfile(sp.Key);
					spmpsp.ApplyPMPlusSales = DeterminePMPlusSales(nodeRID, spmpsp.Key);
					spmpsl.Add(spmpsp);
				}
				return spmpsl;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Determines the presentation minimum plus sales indicator of a store for a given node
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="storeRID">
		/// The record ID of the store
		/// </param>
		/// <returns>
		/// A flag identifying if the presentation minimum plus sales option is to be used for the node/store
		/// </returns>
		private bool DeterminePMPlusSales(int nodeRID, int storeRID)
		{
			try
			{
				if (nodeRID != _currentPMPlusSalesNodeRID) // if not same node, get bit array		
				{
					_PMPlusSalesBitArray = (System.Collections.BitArray)PMPlusSalesHashByNodeRID[nodeRID];
					if (_PMPlusSalesBitArray == null)
					{
						_PMPlusSalesBitArray = HierarchySessionTransaction.GetStorePMPlusSales(nodeRID);
						PMPlusSalesHashByNodeRID.Add(nodeRID, _PMPlusSalesBitArray);
					}
					_currentPMPlusSalesNodeRID = nodeRID;
				}

				return _PMPlusSalesBitArray[storeRID];
			}
            // Begin TT#1522 - JSmith - WUB Headers failing to load into MID
            catch (System.NullReferenceException)
            {
                return false;
            }
            // End TT#1522
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		#endregion Presentation Minimum Plus Sales
		// END MID Track #4827

		// BEGIN TT#2225 - stodd - VSW ANF Enhancement (IMO)
		#region ReadAllocationPlanCubeVariable
		public PlanCellReference ReadAllocationPlanCubeVariable(int storeRid, int hnRid, WeekProfile planWeek, VariableProfile variable)
		{

			Cube storePlanCube = this.GetAllocationPlanCube
					(hnRid, Include.UndefinedDate, ((DayProfile)planWeek.Days[6]).Date);

			PlanCellReference planCellRef = new PlanCellReference((PlanCube)storePlanCube);
			planCellRef[eProfileType.Version] = Include.FV_ActionRID;
			planCellRef[eProfileType.HierarchyNode] = hnRid;
			planCellRef[eProfileType.Week] = planWeek.Key;
			planCellRef[eProfileType.QuantityVariable] = storePlanCube.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
			planCellRef[eProfileType.Variable] = variable.Key;
			planCellRef[eProfileType.Store] = storeRid;

			return planCellRef;

		}
		#endregion ReadAllocationPlanCubeVariable
		// END TT#2225 - stodd - VSW ANF Enhancement (IMO)

		#region GetAllocationPlanCube
		/// <summary>
		/// Gets an instance of the Plan Cube for allocation purposes.
		/// </summary>
		/// <param name="aPlanHnRID">The plan hierarchy node RID</param>
		/// <param name="aBeginDate">The begin date for the plan (Undefined Date defaults to "current" date).</param>
		/// <param name="aTargetDateArrayList">Arraylist of inventory target day profiles</param>
		/// <returns>Plan Cube</returns>
		internal PlanCube GetAllocationPlanCube(
			int aPlanHnRID,
			DateTime aBeginDate,
			ArrayList aTargetDateArrayList)
		{
			DateTime endDate = Include.UndefinedDate;
			foreach (DayProfile dt in aTargetDateArrayList)
			{
				if (endDate < dt.Date)
				{
					endDate = dt.Date;
				}
			}
			return GetAllocationPlanCube(aPlanHnRID, aBeginDate, endDate);
		}

		/// <summary>
		/// Gets an instance of the Plan Cube for allocation purposes.
		/// </summary>
		/// <param name="aPlanHnRID">The plan hierarchy node RID</param>
		/// <param name="aBeginDate">The begin date for the plan (Undefined Date defaults to "current" date).</param>
		/// <param name="aEndDate">The ending date for the plan (Undefined Date defaults to "ship" dates for stores).</param>
		/// <returns>Plan Cube</returns>
		internal PlanCube GetAllocationPlanCube(
			int aPlanHnRID, 
			DateTime aBeginDate, 
			DateTime aEndDate)
		{
			PlanCubeGroup acg = (PlanCubeGroup)this.GetAllocationCubeGroup();
			PlanCube storePlanCube = (PlanCube)acg.GetCube(eCubeType.StorePlanWeekDetail);
            // Begin TT#4988 - JSmith - Performance
            if (acg.OpenParms != null &&
                aPlanHnRID != acg.OpenParms.StoreHLPlanProfile.NodeProfile.Key)
            {
                ClearAllocationCubeGroup();
                ResetFilter(eProfileType.Store);
            }
            // End TT#4988 - JSmith - Performance
			if (storePlanCube == null)
			{
				OpenAllocationCubeGroup(aPlanHnRID, aBeginDate, aEndDate);
			}
			storePlanCube = (PlanCube)acg.GetCube(eCubeType.StorePlanWeekDetail);
			return storePlanCube;
		}
		#endregion GetAllocationPlanCube

		// Begin Track 6074 stodd velocity change
		#region GetAllocationBasisCube
		/// <summary>
		/// Gets an instance of the Basis Cube for allocation purposes.
		/// </summary>
		/// <param name="aPlanHnRID">The plan hierarchy node RID</param>
		/// <param name="aBeginDate">The begin date for the plan (Undefined Date defaults to "current" date).</param>
		/// <param name="aTargetDateArrayList">Arraylist of inventory target day profiles</param>
		/// <returns>Plan Cube</returns>
		internal PlanCube GetAllocationBasisCube(
			int aPlanHnRID,
			DateTime aBeginDate,
			ArrayList aTargetDateArrayList)
		{
			DateTime endDate = Include.UndefinedDate;
			foreach (DayProfile dt in aTargetDateArrayList)
			{
				if (endDate < dt.Date)
				{
					endDate = dt.Date;
				}
			}
			return GetAllocationBasisCube(aPlanHnRID, aBeginDate, endDate);
		}

		/// <summary>
		/// Gets an instance of the Basis Cube for allocation purposes.
		/// </summary>
		/// <param name="aPlanHnRID">The plan hierarchy node RID</param>
		/// <param name="aBeginDate">The begin date for the plan (Undefined Date defaults to "current" date).</param>
		/// <param name="aEndDate">The ending date for the plan (Undefined Date defaults to "ship" dates for stores).</param>
		/// <returns>Plan Cube</returns>
		internal PlanCube GetAllocationBasisCube(
			int aPlanHnRID,
			DateTime aBeginDate,
			DateTime aEndDate)
		{
			PlanCubeGroup acg = (PlanCubeGroup)this.GetAllocationCubeGroup();
			PlanCube storeBasisCube = (PlanCube)acg.GetCube(eCubeType.StoreBasisWeekDetail);
			if (storeBasisCube == null)
			{
				OpenAllocationCubeGroup(aPlanHnRID, aBeginDate, aEndDate);
			}
			storeBasisCube = (PlanCube)acg.GetCube(eCubeType.StoreBasisWeekDetail);
			return storeBasisCube;
		}
		#endregion GetAllocationBasisCube
		// End Track 6074

		#region GetAllocationFilteredStoreList
		private bool SetAllocationFilteredStoreList(int aHnRID, int aStoreFilterRID)	// Issue 5727
		{
			// This makes sure the cube group is filled in
			GetAllocationPlanCube(aHnRID, Include.UndefinedDate, Include.UndefinedDate);
			// This gets the cube group
			PlanCubeGroup acg = (PlanCubeGroup)this.GetAllocationCubeGroup();

			//CustomStoreFilter aStoreFilter = new CustomStoreFilter(SAB, this, SAB.ApplicationServerSession, acg, aStoreFilterRID);

            filter aStoreFilter = filterDataHelper.LoadExistingFilter(aStoreFilterRID);
            aStoreFilter.SetExtraInfoForCubes(SAB, this, acg);
      

			// reset the filter list
			this.GetProfileListGroup(eProfileType.Store).ResetFilteredList();

            //if (!aStoreFilter.FilterOutdatedInformation)
            //{
                this.ApplyFilter(aStoreFilter, eFilterType.Temporary);
            //}
                
            this._allocationStoreFilterRID = aStoreFilterRID;
            bool isFilterOutdated = false; //aStoreFilter.FilterOutdatedInformation;

            aStoreFilter.Dispose();
			// BEGIN Issue 5727 stodd 
            return isFilterOutdated;
			// END Issue 5727 stodd 
		}
		// BEGIN Issue 5727 stodd 
		internal ProfileList GetAllocationFilteredStoreList(AllocationProfile allocProfile, int storeFilterRid, ref bool outdatedFilter)
		{
			return GetAllocationFilteredStoreList(allocProfile.PlanHnRID, storeFilterRid, ref outdatedFilter);
		}
		// END Issue 5727 stodd

		internal ProfileList GetAllocationFilteredStoreList(int aHnRID, int aStoreFilterRID,ref bool outdatedFilter)	// Track 5727
		{
			if (this._quickFilterChanged 
				|| this._allocationStoreFilterRID != aStoreFilterRID)
			{
				// BEGIN Issue 5727 stodd 
				outdatedFilter = false;
				// END Issue 5727 stodd 
				this._quickFilterChanged = false;
				if (this._allocationStoreFilterRID != aStoreFilterRID)
				{
					if (aStoreFilterRID == Include.NoRID)
					{
						// BEGIN Issue 5727 stodd 
						outdatedFilter = SetAllocationFilteredStoreList(aHnRID, Include.AllStoreFilterRID);
						// END Issue 5727 stodd 
					}
					else
					{
						// BEGIN Issue 5727 stodd 
						outdatedFilter = SetAllocationFilteredStoreList(aHnRID, aStoreFilterRID);
						// END Issue 5727 stodd 
					}
				}

				ProfileList storeFilter = this.GetFilteredProfileList(eProfileType.Store);
				ProfileList quickFilter = this.GetAllocationQuickFilterStoreList();
				_selectedAllocationStores = new ProfileList(eProfileType.Store);
				StoreProfile sp;
				ArrayList storeArray = storeFilter.ArrayList;
				for(int i=0; i< storeArray.Count; i++)
				{
					sp = (StoreProfile)storeArray[i];
					if (quickFilter.Contains(sp.Key))
					{
						_selectedAllocationStores.Add(sp);
					}
				}
			}
			return _selectedAllocationStores;
			//			return this.GetFilteredProfileList(eProfileType.Store);
		}
		#endregion

		#region OpenAllocationCubeGroup
		internal struct vSalesPeriod
		{
			internal DateRangeProfile _vSalesPeriodProfile;
			internal double _vSalesPeriod_Weight;
		}
		private void OpenAllocationCubeGroup(int aPlanHnRID, 
			DateTime aBeginDate, 
			DateTime aEndDate)
		{
			//Begin Track #4457 - JSmith - Add forecast versions
			ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
			//End Track #4457

			PlanOpenParms openParms = new PlanOpenParms(ePlanSessionType.StoreSingleLevel, 
				SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().Name);
			openParms.FunctionSecurityProfile = new FunctionSecurityProfile((int)eSecurityFunctions.NotSpecified);
			openParms.FunctionSecurityProfile.SetReadOnly();
			DateRangeProfile drp = new DateRangeProfile(Include.UndefinedCalendarDateRange);
			WeekProfile wp;
			if (aBeginDate == Include.UndefinedDate)
			{
				wp = this.SAB.ApplicationServerSession.Calendar.CurrentWeek;
			}
			else
			{
				wp = this.SAB.ApplicationServerSession.Calendar.GetWeek(aBeginDate);
			}
			drp.StartDateKey = wp.Key;
			int gradeWeekCount = this.GlobalOptions.StoreGradePeriod;
			// BEGIN MID Track 2340 Volume Grade different between OTS and Allocation
			//			DateTime targetDate;
			//			if (aEndDate < wp.Date.AddDays(7))
			//			{
			//				targetDate = wp.Date.AddDays(7);
			//			}
			//			else
			//			{
			//				targetDate = aEndDate;
			//			}
			//			AllocationProfileList apl = (AllocationProfileList)this.GetMasterProfileList(eProfileType.Allocation);
			//			foreach (AllocationProfile ap in apl)
			//			{
			//				if (gradeWeekCount < ap.GradeWeekCount)
			//				{
			//					gradeWeekCount = ap.GradeWeekCount;
			//				}
			//				foreach (Index_RID storeIdxRID in this.StoreIndexRIDArray())
			//				{
			//					if (ap.GetStoreShipDay(storeIdxRID) > targetDate)
			//					{
			//						targetDate = ap.GetStoreShipDay(storeIdxRID);
			//					}
			//				}
			//			}
			//			drp.EndDateKey = this.SAB.ApplicationServerSession.Calendar.AddWeeks(wp.Key,gradeWeekCount);
			//			WeekProfile targetWeekProfile = this.SAB.ApplicationServerSession.Calendar.GetWeek(targetDate);
			//			if (targetWeekProfile.Key > drp.EndDateKey)
			//			{
			//				drp.EndDateKey = targetWeekProfile.Key;
			//			}
			if (gradeWeekCount > 0)
			{
				drp.EndDateKey = this.SAB.ApplicationServerSession.Calendar.AddWeeks(wp.Key,gradeWeekCount - 1);
			}
			else
			{
				drp.EndDateKey = wp.Key;
			}
			//  END MID Track #2340

			drp.RelativeTo = eDateRangeRelativeTo.Current;
			drp.DateRangeType = eCalendarRangeType.Static;
			drp.SelectedDateType = eCalendarDateType.Week;
			drp.DisplayDate = "Allocation Plan Horizon";
			openParms.DateRangeProfile = drp;
//Begin Track #4457 - JSmith - Add forecast versions
//			openParms.ChainHLPlanProfile.VersionProfile = new VersionProfile(Include.FV_ActionRID);
			openParms.ChainHLPlanProfile.VersionProfile = fvpb.Build(Include.FV_ActionRID);
			//End Track #4457
			//Begin Track #5233 - JSmith - Exception during store forecast copy
			openParms.ChainHLPlanProfile.VersionProfile.ChainSecurity = new VersionSecurityProfile(Include.FV_ActionRID);
			openParms.ChainHLPlanProfile.VersionProfile.ChainSecurity.SetReadOnly();
			//Begin Track #5233
			openParms.ChainHLPlanProfile.NodeProfile = this.GetNodeData(aPlanHnRID);
			openParms.ChainHLPlanProfile.NodeProfile.ChainSecurityProfile.SetReadOnly();
			//Begin Track #4457 - JSmith - Add forecast versions
//			openParms.StoreHLPlanProfile.VersionProfile = new VersionProfile(Include.FV_ActionRID);
			openParms.StoreHLPlanProfile.VersionProfile = fvpb.Build(Include.FV_ActionRID);
			//End Track #4457
			//Begin Track #5233 - JSmith - Exception during store forecast copy
			openParms.StoreHLPlanProfile.VersionProfile.StoreSecurity = new VersionSecurityProfile(Include.FV_ActionRID);
			openParms.StoreHLPlanProfile.VersionProfile.StoreSecurity.SetReadOnly();
			//Begin Track #5233

			openParms.StoreHLPlanProfile.NodeProfile = openParms.ChainHLPlanProfile.NodeProfile;
			openParms.StoreHLPlanProfile.NodeProfile.StoreSecurityProfile.SetReadOnly();
			// BEGIN MID Change j.ellis Set Totals for Velocity Basis not shown
			if (this.CurrentStoreGroupProfile == null)
			{
				openParms.StoreGroupRID = Include.AllStoreFilterRID;
			}
			else
			{
				openParms.StoreGroupRID = this.CurrentStoreGroupProfile.Key;
			}
			// BEGIN MID Change j.ellis Set Totals for Velocity Basis not shown

			openParms.OpenPeriodAsWeeks = true;  // MID Track # 2621 Period Basis not processed correctly
            openParms.IncludeLocks = false; // Begin TT#TT#739-MD - JSmith - delete stores


			if (this.VelocityCriteriaExists)
			{
				int basisHnRID;
				int basisPHRID;
				int basisPHLSeq;
				int basisFVRID;
				// BEGIN Issue 4818
				int basisCdrRid;
				float basisWeight;
				// END Issue 4818
				HierarchyNodeProfile basisHierarchyNodeProfile;
				DataTable dtBasis = this._velocityCriteria.DSVelocity.Tables["Basis"];
				//DataTable dtSalesPeriod = this._velocityCriteria.DSVelocity.Tables["SalesPeriod"];
				//vSalesPeriod[] basisSalesPeriod = new vSalesPeriod[dtSalesPeriod.Rows.Count];
				AllocationProfile ap = (AllocationProfile)this.GetAllocationProfileList().ArrayList[0];
				//int salesPeriodCount = 0;
//				foreach (DataRow dr in dtSalesPeriod.Rows)
//				{
//					basisSalesPeriod[salesPeriodCount]._vSalesPeriodProfile = 
//						this.SAB.ApplicationServerSession.Calendar.GetDateRange(Convert.ToInt32(dr["cdrRID"],CultureInfo.CurrentUICulture));
//					basisSalesPeriod[salesPeriodCount]._vSalesPeriod_Weight = Convert.ToDouble(dr["Weight"],CultureInfo.CurrentUICulture);
//					salesPeriodCount++;
//				}

				BasisProfile basisProfile = new BasisProfile(0, "Basis", openParms);
				BasisDetailProfile basisDetailProfile;
				int basisDetailCount = 0;
				ForecastVersion fv = new ForecastVersion();
				VersionProfile basisVP;
				foreach (DataRow dr in dtBasis.Rows)
				{
					basisHnRID = Convert.ToInt32(dr["BasisHNRID"],CultureInfo.CurrentUICulture);
					basisPHRID = Convert.ToInt32(dr["BasisPHRID"],CultureInfo.CurrentUICulture);
					basisPHLSeq = Convert.ToInt32(dr["BasisPHLSequence"],CultureInfo.CurrentUICulture);
					basisFVRID = Convert.ToInt32(dr["BasisFVRID"],CultureInfo.CurrentCulture);
					//Begin Track #4457 - JSmith - Add forecast versions
//					basisVP = new VersionProfile(basisFVRID);
					basisVP = fvpb.Build(basisFVRID);
					//End Track #4457
					// BEGIN Issue 4818
					basisCdrRid = Convert.ToInt32(dr["cdrRID"],CultureInfo.CurrentUICulture);
					basisWeight = (float)Convert.ToDouble(dr["Weight"],CultureInfo.CurrentUICulture);
					// END Issue 4818
					if (basisHnRID == Include.NoRID)
					{
						if (basisPHRID == Include.NoRID)
						{
							if (basisPHLSeq == 0)
							{
								// BEGIN MID Track #3872 - use color or style node for plan level lookup
								//basisHierarchyNodeProfile = this.GetPlanLevelData(ap.StyleHnRID);
								basisHierarchyNodeProfile = this.GetPlanLevelData(ap.PlanLevelStartHnRID);
								if (basisHierarchyNodeProfile == null)
								{
									throw new MIDException(eErrorLevel.severe,
										(int)(eMIDTextCode.msg_al_PlanLevelUndetermined),
										MIDText.GetText(eMIDTextCode.msg_al_PlanLevelUndetermined));
								}
								// END MID Track #3872
							}
							else
							{
								//basisHierarchyNodeProfile = this.SAB.HierarchyServerSession.GetAncestorDataByLevel(ap.StyleHnRID, ap.StyleHnRID, basisPHLSeq); // MID Change j.ellis Performance--cache results of ancestor data lookup
								basisHierarchyNodeProfile = GetAncestorDataByLevel(ap.StyleHnRID, ap.StyleHnRID, basisPHLSeq); // MID Change j.ellis Performance--cache results of ancestor data lookup
							}
						}
						else
						{
							//basisHierarchyNodeProfile = this.SAB.HierarchyServerSession.GetAncestorDataByLevel(basisPHRID, ap.StyleHnRID, basisPHLSeq); // MID Change j.ellis Performance--cache results of ancestor data lookup
							basisHierarchyNodeProfile = GetAncestorDataByLevel(basisPHRID, ap.StyleHnRID, basisPHLSeq); // MID Change j.ellis Performance--cache results of ancestor data lookup
						}
					}
					else
					{
						//						basisHierarchyNodeProfile = this.GetPlanLevelData(basisHnRID);
						basisHierarchyNodeProfile = this.GetNodeData(basisHnRID);
					}
					//for (salesPeriodCount = 0; salesPeriodCount < basisSalesPeriod.Length; salesPeriodCount++)
					//{
						basisDetailCount++;
						basisDetailProfile = new BasisDetailProfile(basisDetailCount,openParms);
						basisDetailProfile.DateRangeProfile = this.SAB.ApplicationServerSession.Calendar.GetDateRange(basisCdrRid);  // Issue 4818
						basisDetailProfile.HierarchyNodeProfile = basisHierarchyNodeProfile;
						basisDetailProfile.IncludeExclude = eBasisIncludeExclude.Include;
						basisDetailProfile.VersionProfile = basisVP;
						basisDetailProfile.Weight = basisWeight;	// Issue 4818
						basisProfile.BasisDetailProfileList.Add(basisDetailProfile);
					//}
				}
				openParms.BasisProfileList.Add(basisProfile);
			}

			((PlanCubeGroup)this.GetAllocationCubeGroup()).OpenCubeGroup(openParms);
		}

		private void OpenAllocationBasisCubeGroup(int aPlanHnRID, 
			DateTime aBeginDate, 
			DateTime aEndDate)
		{
//Begin Track #4457 - JSmith - Add forecast versions
			ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
//End Track #4457

            //Begin Track #6342 - JSmith - Velocity assigning wrong grades
            ProfileList weeks;
            //End Track #6342
            if (this.VelocityCriteriaExists)
			{
				PlanOpenParms openParms = new PlanOpenParms(ePlanSessionType.StoreSingleLevel, 
					SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().Name);
				openParms.FunctionSecurityProfile = new FunctionSecurityProfile((int)eSecurityFunctions.NotSpecified);
				openParms.FunctionSecurityProfile.SetReadOnly();

//Begin Track #4457 - JSmith - Add forecast versions
//				openParms.ChainHLPlanProfile.VersionProfile = new VersionProfile(Include.FV_ActionRID);
				openParms.ChainHLPlanProfile.VersionProfile = fvpb.Build(Include.FV_ActionRID);
//End Track #4457
				openParms.ChainHLPlanProfile.NodeProfile = this.GetNodeData(aPlanHnRID);
				openParms.ChainHLPlanProfile.NodeProfile.ChainSecurityProfile.SetReadOnly();
//Begin Track #4457 - JSmith - Add forecast versions
//				openParms.StoreHLPlanProfile.VersionProfile = new VersionProfile(Include.FV_ActionRID);
				openParms.StoreHLPlanProfile.VersionProfile = fvpb.Build(Include.FV_ActionRID);
//End Track #4457
				openParms.StoreHLPlanProfile.NodeProfile = openParms.ChainHLPlanProfile.NodeProfile;
				openParms.StoreHLPlanProfile.NodeProfile.StoreSecurityProfile.SetReadOnly();
				openParms.StoreGroupRID = this.CurrentStoreGroupProfile.Key;
				openParms.OpenPeriodAsWeeks = true;  // MID Track # 2621 Period Basis not processed correctly
				// BEGIN MID Change j.ellis Similar Store History not being used 
                if (this.VelocityUseSimilarStoreHistory)
                {
					openParms.SimilarStores = true;
				}
				else
				{
					openParms.SimilarStores = false;
				}
				// END MID Change j.ellis Similar store history not being used.

				int basisHnRID;
				int basisPHRID;
				int basisPHLSeq;
				int basisFVRID;
				// BEGIN Issue 4818
				int basisCdrRid;
				float basisWeight;
				// END Issue 4818
				HierarchyNodeProfile basisHierarchyNodeProfile;
				DataTable dtBasis = this._velocityCriteria.DSVelocity.Tables["Basis"];
				//DataTable dtSalesPeriod = this._velocityCriteria.DSVelocity.Tables["SalesPeriod"];
				vSalesPeriod[] basisSalesPeriod = new vSalesPeriod[dtBasis.Rows.Count];

				// BEGIN Issue 4778 stodd 10.8.2007
				// If processed interactively, we use the basis from the first header(Allocation Profile).
				// If it's run normally using multiple headers we need to process the basis taking into account each
				// Header's Node Rid. This is particularly needed when only a level is define, but not a true node.
				AllocationProfile ap = null;
				if (_velocityCriteria.IsInteractive)
				{
					ap = (AllocationProfile)this.GetAllocationProfileList().ArrayList[0];
				}
				else
				{
					ap = _velocityCriteria.AlocProfile;
				}
				// END Issue 4778

				int basisCount = 0;
				int[] basisSalesWeekCount = new int[dtBasis.Rows.Count]; //MID Track 2923 Velocity Basis wrong if weeks not specified in descending order
				foreach (DataRow dr in dtBasis.Rows)
				{
					basisSalesPeriod[basisCount]._vSalesPeriodProfile = 
						this.SAB.ApplicationServerSession.Calendar.GetDateRange(Convert.ToInt32(dr["cdrRID"],CultureInfo.CurrentUICulture));
                    //Begin Track #6342 - JSmith - Velocity assigning wrong grades
                    weeks = this.SAB.ApplicationServerSession.Calendar.GetDateRangeWeeks(basisSalesPeriod[basisCount]._vSalesPeriodProfile, basisSalesPeriod[basisCount]._vSalesPeriodProfile.InternalAnchorDate);
                    if (aBeginDate == Include.UndefinedDate)
                    {
                        if (weeks.Count > 0)
                        {
                            aBeginDate = ((WeekProfile)weeks[0]).Date;
                        }
                    }
                    //End Track #6342
					basisSalesPeriod[basisCount]._vSalesPeriod_Weight = Convert.ToDouble(dr["Weight"],CultureInfo.CurrentUICulture);
					// BEGIN MID Track 2923 Velocity Basis wrong if basis weeks not specified in descending order by number of weeks
                    //Begin Track #6342 - JSmith - Velocity assigning wrong grades
                    //basisSalesWeekCount[basisCount] = 
                    //    - (this.SAB.ApplicationServerSession.Calendar.GetDateRangeWeeks(basisSalesPeriod[basisCount]._vSalesPeriodProfile,basisSalesPeriod[basisCount]._vSalesPeriodProfile.InternalAnchorDate)).Count;
                    basisSalesWeekCount[basisCount] = - weeks.Count;
                    //End Track #6342
					// force descending sort.
					// END MID Track 2923
					basisCount++;
				}
				Array.Sort(basisSalesWeekCount,basisSalesPeriod); // MID Track 2923 Velocity basis wrong if basis weeks not specified in descending order by number of weeks.
				// First basis time (one with largest number of weeks) determines plan horizon for basis cube
				// BEGIN Issues 4392/4459 stodd 10.11.2007
				// If the basis date range with the longest number of weeks is way in the past, some stores may end up being ineligible
				// and then not have any basis values returned. So instead we use header Begin Date as a starting point, then get
				// the longest basis, and add that number of weeks to the Begin Date. From that we get a date range profile to use as the 
				// plan date range profile.
				int beginYYYYWW = 0;
				WeekProfile beginWp = null;
				int endYYYYWW = 0;
				WeekProfile endWp = null;
                //Begin Track #6342 - JSmith - Velocity assigning wrong grades
                //if (ap.BeginDay != Include.UndefinedDate)
                //{
                //    beginWp = this.SAB.ApplicationServerSession.Calendar.GetWeek(ap.BeginDay);
                //    beginYYYYWW = beginWp.YearWeek;
                //}
                //else
                //{
                //    beginWp = this.SAB.ApplicationServerSession.Calendar.CurrentWeek;
                //    beginYYYYWW = beginWp.YearWeek;
                //}
                if (aBeginDate != Include.UndefinedDate)
                {
                    beginWp = this.SAB.ApplicationServerSession.Calendar.GetWeek(aBeginDate);
                    beginYYYYWW = beginWp.YearWeek;
                }
                else
                {
                    beginWp = this.SAB.ApplicationServerSession.Calendar.CurrentWeek;
                    beginYYYYWW = beginWp.YearWeek;
                }
                //End Track #6342
				int weekCount = Math.Abs((int)basisSalesWeekCount[0]);
				endWp = this.SAB.ApplicationServerSession.Calendar.Add(beginWp, weekCount);
				endYYYYWW = endWp.YearWeek;
				DateRangeProfile drp = this.SAB.ApplicationServerSession.Calendar.AddDateRangeFromWeeks(beginYYYYWW, endYYYYWW);
				openParms.DateRangeProfile = drp;
				// END Issues 4392/4459

				BasisProfile basisProfile = new BasisProfile(0, "Basis", openParms);
				BasisDetailProfile basisDetailProfile;
				int basisDetailCount = 0;
				ForecastVersion fv = new ForecastVersion();
				VersionProfile basisVP;
				foreach (DataRow dr in dtBasis.Rows)
				{
					basisHnRID = Convert.ToInt32(dr["BasisHNRID"],CultureInfo.CurrentUICulture);
					basisPHRID = Convert.ToInt32(dr["BasisPHRID"],CultureInfo.CurrentUICulture);
					basisPHLSeq = Convert.ToInt32(dr["BasisPHLSequence"],CultureInfo.CurrentUICulture);
					basisFVRID = Convert.ToInt32(dr["BasisFVRID"],CultureInfo.CurrentCulture);
					//Begin Track #4457 - JSmith - Add forecast versions
//					basisVP = new VersionProfile(basisFVRID);
					basisVP = fvpb.Build(basisFVRID);
					//End Track #4457
					// BEGIN Issue 4818
					basisCdrRid = Convert.ToInt32(dr["cdrRID"],CultureInfo.CurrentUICulture);
					basisWeight = (float)Convert.ToDouble(dr["Weight"],CultureInfo.CurrentUICulture);
					// END Issue 4818

					if (basisHnRID == Include.NoRID)
					{
						if (basisPHRID == Include.NoRID)
						{
							if (basisPHLSeq == 0)
							{
								// BEGIN MID Track #3872 - use color or style node for plan level lookup
								//basisHierarchyNodeProfile = this.GetPlanLevelData(ap.StyleHnRID);
                                // BEGIN TT#1158 - AGallagher - Velocity-> not honoring the general method OTS Forecast level change
                                //basisHierarchyNodeProfile = this.GetPlanLevelData(ap.PlanLevelStartHnRID);
                                basisHierarchyNodeProfile = this.GetNodeData(ap.PlanHnRID);
                                // END TT#1158 - AGallagher - Velocity-> not honoring the general method OTS Forecast level change
								if (basisHierarchyNodeProfile == null)
								{
									throw new MIDException(eErrorLevel.severe,
										(int)(eMIDTextCode.msg_al_PlanLevelUndetermined),
										MIDText.GetText(eMIDTextCode.msg_al_PlanLevelUndetermined));
								}
								// END MID Track #3872
							}
							else
							{
								//basisHierarchyNodeProfile = this.SAB.HierarchyServerSession.GetAncestorDataByLevel(ap.StyleHnRID, ap.StyleHnRID, basisPHLSeq); // MID Change j.ellis Performance--cache results of ancestor data lookup
								basisHierarchyNodeProfile = GetAncestorDataByLevel(ap.StyleHnRID, ap.StyleHnRID, basisPHLSeq); // MID Change j.ellis Performance--cache results of ancestor data lookup
							}
						}
						else
						{
							//basisHierarchyNodeProfile = this.SAB.HierarchyServerSession.GetAncestorDataByLevel(basisPHRID, ap.StyleHnRID, basisPHLSeq); // MID Change j.ellis Performance--cache results of ancestor data lookup
							// begin MID Track 4094 'color' not recognized by velocity
							int startHnRID = ap.StyleHnRID;
							int colorHnRID = Include.NoRID;
							HierarchyNodeProfile styleHierarchyNodeProfile = this.GetNodeData(ap.StyleHnRID);
							switch (this.Velocity.Component.ComponentType)
							{
								case (eComponentType.SpecificColor):
								{
									AllocationColorOrSizeComponent colorComponent = (AllocationColorOrSizeComponent)this.Velocity.Component;
									ColorCodeProfile ccp = this.GetColorCodeProfile(colorComponent.ColorRID);
									if(this.SAB.HierarchyServerSession.ColorExistsForStyle(styleHierarchyNodeProfile.HomeHierarchyRID, styleHierarchyNodeProfile.Key, ccp.ColorCodeID, ref colorHnRID))
									{
										startHnRID = colorHnRID;
									}
									break;
								}
								case (eComponentType.SpecificPack):
								{
									AllocationPackComponent apc = (AllocationPackComponent)this.Velocity.Component;
									PackHdr ph = ap.GetPackHdr(apc.PackName);
									if (ph.PackColors.Count == 1)
									{
										foreach (PackColorSize pcs in ph.PackColors.Values)
										{
											ColorCodeProfile ccp = this.GetColorCodeProfile(pcs.ColorCodeRID);
											if(this.SAB.HierarchyServerSession.ColorExistsForStyle(styleHierarchyNodeProfile.HomeHierarchyRID, styleHierarchyNodeProfile.Key, ccp.ColorCodeID, ref colorHnRID))
											{
												startHnRID = colorHnRID;
											}
											break;
										}
									}
									break;
								}
								case (eComponentType.Total):
								{
									startHnRID = ap.PlanLevelStartHnRID;
									break;
								}
							}
							basisHierarchyNodeProfile = GetAncestorDataByLevel(basisPHRID, startHnRID, basisPHLSeq);  // when only one color start looking at the color otherwise at the style
							//basisHierarchyNodeProfile = GetAncestorDataByLevel(basisPHRID, ap.StyleHnRID, basisPHLSeq); // MID Change j.ellis Performance--cache results of ancestor data lookup
							// end MID Track 4094 'color not recognized by velocity
						}
					}
					else
					{
						//						basisHierarchyNodeProfile = this.GetPlanLevelData(basisHnRID);
						basisHierarchyNodeProfile = this.GetNodeData(basisHnRID);
					}
					//for (salesPeriodCount = 0; salesPeriodCount < basisSalesPeriod.Length; salesPeriodCount++)
					//{
						basisDetailCount++;
						basisDetailProfile = new BasisDetailProfile(basisDetailCount,openParms);
						basisDetailProfile.DateRangeProfile = this.SAB.ApplicationServerSession.Calendar.GetDateRange(basisCdrRid);
						basisDetailProfile.HierarchyNodeProfile = basisHierarchyNodeProfile;
						basisDetailProfile.IncludeExclude = eBasisIncludeExclude.Include;
						basisDetailProfile.VersionProfile = basisVP;
						basisDetailProfile.Weight = basisWeight;
						basisProfile.BasisDetailProfileList.Add(basisDetailProfile);
					//}
				}
				openParms.BasisProfileList.Add(basisProfile);

				((PlanCubeGroup)this.GetAllocationBasisCubeGroup()).OpenCubeGroup(openParms);
			}
            // Begin TT#638 - RMatelic - Style Review - Add Basis Vatiables
            else if (BasisCriteriaExists)
            {
                OpenNonVelocityBasisCubeGroup(aPlanHnRID, aBeginDate, aEndDate);
            }
            // End TT#638
		}

        // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
        private void OpenNonVelocityBasisCubeGroup(int aPlanHnRID,
            DateTime aBeginDate,
            DateTime aEndDate)
        {
            ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
            ProfileList weeks;
            
            PlanOpenParms openParms = new PlanOpenParms(ePlanSessionType.StoreSingleLevel,
                    SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().Name);
            openParms.FunctionSecurityProfile = new FunctionSecurityProfile((int)eSecurityFunctions.NotSpecified);
            openParms.FunctionSecurityProfile.SetReadOnly();

            openParms.ChainHLPlanProfile.VersionProfile = fvpb.Build(Include.FV_ActionRID);
            openParms.ChainHLPlanProfile.NodeProfile = this.GetNodeData(aPlanHnRID);
            openParms.ChainHLPlanProfile.NodeProfile.ChainSecurityProfile.SetReadOnly();
           
            openParms.StoreHLPlanProfile.VersionProfile = fvpb.Build(Include.FV_ActionRID);
            openParms.StoreHLPlanProfile.NodeProfile = openParms.ChainHLPlanProfile.NodeProfile;
            openParms.StoreHLPlanProfile.NodeProfile.StoreSecurityProfile.SetReadOnly();
            openParms.StoreGroupRID = this.CurrentStoreGroupProfile.Key;
            openParms.OpenPeriodAsWeeks = true;   
            openParms.SimilarStores = false;

            int basisHnRID;
            int basisPHRID;
            int basisPHLSeq;
            int basisFVRID;
            int basisCdrRid;
            float basisWeight;
   
            HierarchyNodeProfile basisHierarchyNodeProfile;
            DataTable dtBasis = this._allocationCriteria.DTUserAllocBasis;
    
            vSalesPeriod[] basisSalesPeriod = new vSalesPeriod[dtBasis.Rows.Count];

            AllocationProfile ap = null;
            ap = (AllocationProfile)this.GetAllocationProfileList().ArrayList[0];
            
            int basisCount = 0;
            int[] basisSalesWeekCount = new int[dtBasis.Rows.Count]; //MID Track 2923 Velocity Basis wrong if weeks not specified in descending order
            foreach (DataRow dr in dtBasis.Rows)
            {
                // Begin TT#2093 - JSmith - Need Analysis Add basis and after window opens add columns and receive an error.
                // skip all non specific merchandise entries
                if (AnalysisOnly)
                {
                    if (Convert.ToInt32(dr["BasisHNRID"]) == Include.NoRID)
                    {
                        continue;
                    }
                }
                // End TT#2093 - JSmith - Need Analysis Add basis and after window opens add columns and receive an error.

                basisSalesPeriod[basisCount]._vSalesPeriodProfile =
                    this.SAB.ApplicationServerSession.Calendar.GetDateRange(Convert.ToInt32(dr["CdrRID"], CultureInfo.CurrentUICulture));
                weeks = this.SAB.ApplicationServerSession.Calendar.GetDateRangeWeeks(basisSalesPeriod[basisCount]._vSalesPeriodProfile, basisSalesPeriod[basisCount]._vSalesPeriodProfile.InternalAnchorDate);
                if (aBeginDate == Include.UndefinedDate)
                {
                    if (weeks.Count > 0)
                    {
                        aBeginDate = ((WeekProfile)weeks[0]).Date;
                    }
                }
 
                basisSalesPeriod[basisCount]._vSalesPeriod_Weight = Convert.ToDouble(dr["Weight"], CultureInfo.CurrentUICulture);
                basisSalesWeekCount[basisCount] = -weeks.Count;
                
                basisCount++;
            }
            Array.Sort(basisSalesWeekCount, basisSalesPeriod); // MID Track 2923 Velocity basis wrong if basis weeks not specified in descending order by number of weeks.
            // First basis time (one with largest number of weeks) determines plan horizon for basis cube
            // BEGIN Issues 4392/4459 stodd 10.11.2007
            // If the basis date range with the longest number of weeks is way in the past, some stores may end up being ineligible
            // and then not have any basis values returned. So instead we use header Begin Date as a starting point, then get
            // the longest basis, and add that number of weeks to the Begin Date. From that we get a date range profile to use as the 
            // plan date range profile.
            int beginYYYYWW = 0;
            WeekProfile beginWp = null;
            int endYYYYWW = 0;
            WeekProfile endWp = null;
            
            if (aBeginDate != Include.UndefinedDate)
            {
                beginWp = this.SAB.ApplicationServerSession.Calendar.GetWeek(aBeginDate);
                beginYYYYWW = beginWp.YearWeek;
            }
            else
            {
                beginWp = this.SAB.ApplicationServerSession.Calendar.CurrentWeek;
                beginYYYYWW = beginWp.YearWeek;
            }
     
            int weekCount = Math.Abs((int)basisSalesWeekCount[0]);
            endWp = this.SAB.ApplicationServerSession.Calendar.Add(beginWp, weekCount);
            endYYYYWW = endWp.YearWeek;
            DateRangeProfile drp = this.SAB.ApplicationServerSession.Calendar.AddDateRangeFromWeeks(beginYYYYWW, endYYYYWW);
            openParms.DateRangeProfile = drp;
   
            BasisProfile basisProfile = new BasisProfile(0, "Basis", openParms);
            BasisDetailProfile basisDetailProfile;
            int basisDetailCount = 0;
            ForecastVersion fv = new ForecastVersion();
            VersionProfile basisVP;
            foreach (DataRow dr in dtBasis.Rows)
            {
                // Begin TT#2093 - JSmith - Need Analysis Add basis and after window opens add columns and receive an error.
                // skip all non specific merchandise entries
                if (AnalysisOnly)
                {
                    if (Convert.ToInt32(dr["BasisHNRID"]) == Include.NoRID)
                    {
                        continue;
                    }
                }
                // End TT#2093 - JSmith - Need Analysis Add basis and after window opens add columns and receive an error.

                basisHnRID = Convert.ToInt32(dr["BasisHNRID"], CultureInfo.CurrentUICulture);
                basisPHRID = Convert.ToInt32(dr["BasisPHRID"], CultureInfo.CurrentUICulture);
                basisPHLSeq = Convert.ToInt32(dr["BasisPHLSequence"], CultureInfo.CurrentUICulture);
                basisFVRID = Convert.ToInt32(dr["BasisFVRID"], CultureInfo.CurrentCulture);
                basisVP = fvpb.Build(basisFVRID);
               
                basisCdrRid = Convert.ToInt32(dr["CdrRID"], CultureInfo.CurrentUICulture);
                basisWeight = (float)Convert.ToDouble(dr["Weight"], CultureInfo.CurrentUICulture);
   
                if (basisHnRID == Include.NoRID)
                {
                    if (basisPHRID == Include.NoRID)
                    {
                        if (basisPHLSeq == 0)
                        {
                            basisHierarchyNodeProfile = this.GetPlanLevelData(ap.PlanLevelStartHnRID);
                            if (basisHierarchyNodeProfile == null)
                            {
                                throw new MIDException(eErrorLevel.severe,
                                    (int)(eMIDTextCode.msg_al_PlanLevelUndetermined),
                                    MIDText.GetText(eMIDTextCode.msg_al_PlanLevelUndetermined));
                            }
                        }
                        else
                        {
                            basisHierarchyNodeProfile = GetAncestorDataByLevel(ap.StyleHnRID, ap.StyleHnRID, basisPHLSeq); 
                        }
                    }
                    else
                    {
                        int startHnRID = ap.StyleHnRID;
                        //int colorHnRID = Include.NoRID;  // Not used
                        startHnRID = ap.PlanLevelStartHnRID;
                      
                        basisHierarchyNodeProfile = GetAncestorDataByLevel(basisPHRID, startHnRID, basisPHLSeq); 
                    }
                }
                else
                {
                    basisHierarchyNodeProfile = this.GetNodeData(basisHnRID);
                }
               
                basisDetailCount++;
                basisDetailProfile = new BasisDetailProfile(basisDetailCount, openParms);
                basisDetailProfile.DateRangeProfile = this.SAB.ApplicationServerSession.Calendar.GetDateRange(basisCdrRid);
                basisDetailProfile.HierarchyNodeProfile = basisHierarchyNodeProfile;
                basisDetailProfile.IncludeExclude = eBasisIncludeExclude.Include;
                basisDetailProfile.VersionProfile = basisVP;
                basisDetailProfile.Weight = basisWeight;
                basisProfile.BasisDetailProfileList.Add(basisDetailProfile);
            }
            openParms.BasisProfileList.Add(basisProfile);

            ((PlanCubeGroup)this.GetAllocationBasisCubeGroup()).OpenCubeGroup(openParms);
        }
        // End TT#638
		#endregion OpenAllocationCubeGroup

		#region DailySalesPlan
		/// <summary>
		/// Gets the daily sales plans for a specified plan hierarchy node in a specified week
		/// </summary>
		/// <param name="aHnRID">The Hierarchy node RID for which daily sales are desired.</param>
		/// <param name="aStore">Index_RID for the store whose daily sales are desired.</param>
		/// <param name="aWeekProfile">Week profile for the week in which daily sales are desired.</param>
		/// <returns>Daily sales plan for the store in the specified week.</returns>
		internal double[] GetDailySales(
			PlanCube aStorePlanCube,
			int aHnRID,
			Index_RID aStore,
			WeekProfile aWeekProfile)
		{
			PlanCellReference planCellRef = new PlanCellReference(aStorePlanCube);
			planCellRef[eProfileType.Version] = Include.FV_ActionRID;
			planCellRef[eProfileType.HierarchyNode] = aHnRID;
			planCellRef[eProfileType.QuantityVariable] = PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
			if (eOTSPlanLevelType.Regular == this.GetNodeData(aHnRID).OTSPlanLevelType)
			{
				planCellRef[eProfileType.Variable] = PlanComputations.PlanVariables.SalesRegPromoUnitsVariable.Key;
			}
			else
			{
				planCellRef[eProfileType.Variable] = PlanComputations.PlanVariables.SalesTotalUnitsVariable.Key;
			}
			planCellRef[eProfileType.Store] = aStore.RID;
			ProfileList weekList = new ProfileList(eProfileType.Week);
			weekList.Add(aWeekProfile);
			ArrayList weeklyPlanValues = planCellRef.GetCellRefArray(weekList);

			return GetDailySales(aHnRID, aStore, aWeekProfile,
				(int)((PlanCellReference)weeklyPlanValues[0]).CurrentCellValue);
		}
		/// <summary>
		/// Gets the daily sales plans for a specified plan hierarchy node in a specified week
		/// </summary>
		/// <param name="aHnRID">The Hierarchy node RID for which daily sales are desired.</param>
		/// <param name="aStore">Index_RID for the store whose daily sales are desired.</param>
		/// <param name="aWeekProfile">Week profile for the week in which daily sales are desired.</param>
		/// <param name="aWeekSalesPlan">Sales plan for the week in which daily sales are desired.</param>
		/// <returns>Daily sales plan for the store in the specified week.</returns>
		internal double[] GetDailySales(
			int aHnRID, 
			Index_RID aStore, 
			WeekProfile aWeekProfile, 
			double aWeekSalesPlan)
		{
			return GetDailySales(
				aWeekSalesPlan,
				this.GetStoreDailyPercentages(aHnRID, aStore.RID, aWeekProfile.YearWeek));
		}
		/// <summary>
		/// Gets the daily sales plans for a specified plan hierarchy node in a specified week
		/// </summary>
		/// <param name="aWeekSalesPlan">Week profile for the week in which daily sales are desired.</param>
		/// <param name="aStoreDailyPercentsProfile">Daily sales percentages for the week in which daily sales are desired. </param>
		/// <returns>Daily sales plan for the specified week.</returns>
		private double[] GetDailySales(
			double aWeekSalesPlan, 
			StoreWeekDailyPercentagesProfile aStoreDailyPercentsProfile)
		{
			int oldTotalSales = 0;
			int newTotalSales = (int)aWeekSalesPlan;
			double[] dailyPercents = aStoreDailyPercentsProfile.DailyPercentages;
			double[] dailySales = new double[dailyPercents.Length];
			// Convert Percentages to 7-digit Integers to avoid double round-off errors
			double intermediateValue;
			for (int j=0; j<dailyPercents.Length; j++)
			{
				intermediateValue = (double)Include.PercentToInteger * dailyPercents[j] / 100d;
				if (intermediateValue < 0)
				{
					dailySales[j] = (double)(int)(intermediateValue - .5d);
				}
				else
				{
					dailySales[j] =  (double)(int)(intermediateValue + .5d); 
				}
				oldTotalSales += (int)dailySales[j];
			}
			// Use converted percentages to spread Store weekly sales to days
			int tempSales;
			for (int j=0; j<dailyPercents.Length; j++)
			{
				if (oldTotalSales > +0)
				{
					intermediateValue = ((double)dailySales[j] * (double)newTotalSales )/ (double)oldTotalSales;
					if (intermediateValue < 0)
					{
						tempSales = (int)(intermediateValue - .5d);
					}
					else
					{
						tempSales = (int)(intermediateValue + .5d);
					}
					oldTotalSales -= (int)dailySales[j];
					dailySales[j] = (double)tempSales;
					newTotalSales -= (int)dailySales[j];
				}
			}
			return dailySales;
		}
		#endregion DailySalesPlan

		// BEGIN MID Track #2230   Display Current Week-to-Day Sales on Sku Review
		#region StoreWeekToDaySales
		// BEGIN MID Change j.ellis Method to get Current week-to-day sales
		/// <summary>
		/// Gets Store Week To Day Sales for the current sales week. 
		/// </summary>
		/// <param name="aHnRID">Hierarchy Node RID for which sales is desired.</param>
		/// <param name="aStoreRID">Store RID for which sales is desired.</param>
		/// <returns>Actual week to day sales for current sales week when date is undefined; otherwise, Planned week to day sales upto the specified date.</returns>
		public int GetStoreCurrentWeekToDaySales(
			int aHnRID, 
			int aStoreRID) 
		{
			return GetStoreWeekToDaySales(
				aHnRID,
				Include.UndefinedDate,
				new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize),
				aStoreRID,
				null);
		}
		public int GetStoreCurrentWeekToDaySales(
			int aHnRID,
			ProfileList aVariableList,
			int aStoreRID) 
		{
			return GetStoreWeekToDaySales(
				aHnRID,
				Include.UndefinedDate,
				new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize),
				aStoreRID,
				aVariableList);
		}
		// END MMID Change j.ellis Method to get Current week-to-day sales
		/// <summary>
		/// Gets Store Week To Day Sales for the current sales week. 
		/// </summary>
		/// <param name="aHnRID">Hierarchy Node RID for which sales is desired.</param>
		/// <param name="aOnHandDate">Date for which sales is desired: Use Include.UndefinedDate to get week to day sales for current sales week; the planned sales is returned for "defined" dates.</param>
		/// <param name="aIKT">Key type of the sales to retrieve: Total, Color or Color-Size</param>
		/// <param name="aStoreRID">Store RID for which sales is desired.</param>
		/// <returns>Actual week to day sales for current sales week when date is undefined; otherwise, Planned week to day sales upto the specified date.</returns>
		public int GetStoreWeekToDaySales(
			int aHnRID, 
			DateTime aOnHandDate, 
			IntransitKeyType aIKT, 
			int aStoreRID,
			ProfileList aVariableList) 
		{
			//			return 0;
			if (aOnHandDate == Include.UndefinedDate)
			{
				if (aVariableList == null)
				{
					return this.GetDailySalesReader().GetCurrentWeekToDaySales(aHnRID, aIKT.ColorRID, aIKT.SizeRID, aStoreRID);
				}
				return this.GetDailySalesReader().GetCurrentWeekToDaySales(aHnRID, aIKT, aStoreRID, aVariableList);
			}
			throw new MIDException(eErrorLevel.fatal,
				(int)eMIDTextCode.msg_al_ActionNotImplemented,
				MIDText.GetText(eMIDTextCode.msg_al_ActionNotImplemented));

            //if (_weekToDaySalesHash == null)
            //{
            //    _weekToDaySalesHash = new Hashtable();
            //}
            //Hashtable storeWeekToDaySalesHash;
            //if (!_weekToDaySalesHash.Contains(aHnRID))
            //{
            //    storeWeekToDaySalesHash = new Hashtable();
            //    _weekToDaySalesHash.Add(aHnRID, storeWeekToDaySalesHash);
            //}
            //storeWeekToDaySalesHash = (Hashtable)_onHandHash[aHnRID];
            //Hashtable weekToDaySalesHash;
            //// here i am
	
            //DayProfile startDay = this.SAB.ApplicationServerSession.Calendar.GetDay(aOnHandDate);
            //int onHandDayInWeek = startDay.DayInWeek - 1;
            //if (!storeWeekToDaySalesHash.Contains(aOnHandDate))
            //{
            //    weekToDaySalesHash = 
            //        this.DetermineDailyInventory(aHnRID, (ProfileList)this.GetMasterProfileList(eProfileType.Store), startDay.Week);
            //    storeWeekToDaySalesHash.Add(aOnHandDate, weekToDaySalesHash);
            //}
            //weekToDaySalesHash = (Hashtable)storeWeekToDaySalesHash[aOnHandDate];
            //if (weekToDaySalesHash.Contains(aStoreRID))
            //{
            //    return ((int[])weekToDaySalesHash[aStoreRID])[onHandDayInWeek];
            //}
            //return 0;
		}
		#endregion StoreWeekToDaySales
		// END MID Track #2230   Display Current Week-to-Day Sales on Sku Review

		#region StoreOnHand
		/// <summary>
		/// Gets Store OnHand. 
		/// </summary>
		/// <param name="aHnRID">Hierarchy Node RID for which onhand is desired.</param>
		/// <param name="aOnHandDate">Date for which onhand is desired: Use Include.UndefinedDate to get current onhand; the planned onhand is returned for "defined" dates.</param>
		/// <param name="aIKT">Key type of the onhand to retrieve: Total, Color or Color-Size</param>
		/// <param name="aStoreRID">Store RID for which onhand is desired.</param>
		/// <returns>Actual OnHand when date is undefined; otherwise, Planned Onhand for specified date.</returns>
		public int GetStoreOnHand(
			int aHnRID, 
			DateTime aOnHandDate, 
			IntransitKeyType aIKT, 
			int aStoreRID) 
		{
			return GetStoreOnHand
				(null,
				aHnRID,
				aOnHandDate,
				aIKT,
				aStoreRID);
		}
		/// <summary>
		/// Gets Store OnHand. 
		/// </summary>
		/// <param name="aHnRID">Hierarchy Node RID for which onhand is desired.</param>
		/// <param name="aOnHandDate">Date for which onhand is desired: Use Include.UndefinedDate to get current onhand; the planned onhand is returned for "defined" dates.</param>
		/// <param name="aIKT">Key type of the onhand to retrieve: Total, Color or Color-Size</param>
		/// <param name="aStoreRID">Store RID for which onhand is desired.</param>
		/// <returns>Actual OnHand when date is undefined; otherwise, Planned Onhand for specified date.</returns>
		public int GetStoreOnHand(
			AllocationProfile aAllocationProfile,
			int aHnRID, 
			DateTime aOnHandDate, 
			IntransitKeyType aIKT, 
			int aStoreRID) 
		{
			if (aOnHandDate == Include.UndefinedDate)
			{
				return this.GetOnHandReader().GetCurrentOnHand(aHnRID, aIKT.ColorRID, aIKT.SizeRID, aStoreRID);
			}
			if (_onHandHash == null)
			{
				_onHandHash = new Hashtable();
			}
			Hashtable storeOnHandHash;
			if (!_onHandHash.Contains(aHnRID))
			{
				storeOnHandHash = new Hashtable();
				_onHandHash.Add(aHnRID, storeOnHandHash);
			}
			storeOnHandHash = (Hashtable)_onHandHash[aHnRID];
			Hashtable ohHash;
	
			DayProfile startDay = this.SAB.ApplicationServerSession.Calendar.GetDay(aOnHandDate);
			int onHandDayInWeek = startDay.DayInWeek - 1;
			if (!storeOnHandHash.Contains(aOnHandDate))
			{
				ohHash = 
					this.DetermineDailyInventory(aHnRID, (ProfileList)this.GetMasterProfileList(eProfileType.Store), startDay.Week);
				storeOnHandHash.Add(aOnHandDate, ohHash);
			}
			ohHash = (Hashtable)storeOnHandHash[aOnHandDate];
			if (ohHash.Contains(aStoreRID))
			{
				// BEGIN MID Track # 2937 Size OnHand Incorrect 
				int totalPlanOnHand = ((int[])ohHash[aStoreRID])[onHandDayInWeek];
				if (aIKT.SizeRID == Include.IntransitKeyTypeNoSize)
				{
					return totalPlanOnHand;
				}
				if (aAllocationProfile == null)
				{
					return 0; // no planned size onhand, unable to calculate it
				}
				return GetStoreColorSizeOnHand
					(aAllocationProfile,
					aHnRID,
					aIKT,
					aOnHandDate,
					totalPlanOnHand,
					aStoreRID);
				// END MID Track # 2937 Size OnHandIncorrect
			}
			return 0;
		}
		// BEGIN MID Track # 2937 Size Onhand Incorrect
		/// <summary>
		/// Gets size onhand within the given color
		/// </summary>
		/// <param name="aAllocationProfile">Allocation Profile used to determine the size curves associated with the color</param>
		/// <param name="aHnRID">Hierarchy node where the onhand resides</param>
		/// <param name="aIKT">Intransit Key Type identifies the color and size</param>
		/// <param name="aOnHandDate">Future date where the planned onhand resides</param>
		/// <param name="aPlanOnHandTotal">Planned onhand (BOD) on the future date</param>
		/// <param name="aStoreRID">Store RID identifies the store</param>
		/// <returns>Planned size onhand for the given store.</returns>
		private int GetStoreColorSizeOnHand
			(AllocationProfile aAllocationProfile,
			int aHnRID,
			IntransitKeyType aIKT,
			DateTime aOnHandDate,
			int aPlanOnHandTotal,
			int aStoreRID)
		{
			SizeNeedMethod sizeNeedMethod = null;
			if (aIKT.ColorRID == Include.DummyColorRID)
			{
				sizeNeedMethod =
					aAllocationProfile.GetSizeNeedMethod(new GeneralComponent(eGeneralComponentType.DetailType));
			}
			else
			{
				if (aAllocationProfile.BulkColorIsOnHeader(aIKT.ColorRID))
				{
					sizeNeedMethod =
						aAllocationProfile.GetSizeNeedMethod(aIKT.ColorRID);
				}
			}
			if (sizeNeedMethod == null)
			{
				return 0; // color or detail size need metohd does not exist; cannot calculate size onhand
			}
            // begin TT#3060 - Loehmans - Jellis - System Argument Exception
			if (_storeColorOnhandDict == null)
			{
				_storeColorOnhandDict = new Dictionary<StoreColorCurveOnHandKey,Dictionary<int,int>>();
                _lastStoreColorOnHandKey = new StoreColorCurveOnHandKey(Include.UndefinedDate, Include.NoRID, Include.NoRID, Include.NoRID, Include.NoRID, Include.NoRID, Include.NoRID);
			}
            StoreColorCurveOnHandKey sccok =
                new StoreColorCurveOnHandKey(aOnHandDate, aStoreRID, aIKT.ColorRID, aHnRID, sizeNeedMethod.SizeCurveGroupRid, sizeNeedMethod.SizeConstraintRid, sizeNeedMethod.SizeAlternateRid);
            if (sccok != _lastStoreColorOnHandKey)
            {
                _lastStoreColorOnHandKey = sccok;
                if (!_storeColorOnhandDict.TryGetValue(_lastStoreColorOnHandKey, out _lastStoreColorSizeOnHandDict))
                {
                    _lastStoreColorSizeOnHandDict = CalculateStoreSizeOnHand(aPlanOnHandTotal, sizeNeedMethod.GetSizeCurve(aIKT.ColorRID, aStoreRID));
                    _storeColorOnhandDict.Add(_lastStoreColorOnHandKey, _lastStoreColorSizeOnHandDict);
                }
            }
            int sizeOnhand = 0;
            _lastStoreColorSizeOnHandDict.TryGetValue(aIKT.SizeRID, out sizeOnhand);
            return sizeOnhand;
 
            //if (sizeNeedMethod == null)
            //{
            //    return 0; // color or detail size need metohd does not exist; cannot calculate size onhand
            //}
            //if (_sizeOnhandHash == null)
            //{
            //    _sizeOnhandHash = new Hashtable();
            //}
            //Hashtable sizeHnCurveHash;
            //if (_sizeOnhandHash.Contains(aOnHandDate))
            //{
            //    sizeHnCurveHash = (Hashtable)_sizeOnhandHash[aOnHandDate];
            //}
            //else
            //{
            //    sizeHnCurveHash = new Hashtable();
            //    _sizeOnhandHash.Add(aOnHandDate, sizeHnCurveHash);
            //}
            //Hashtable sizeConstraintHash;
            //long sizeHnCurveKey = (((long)aHnRID) << 32) + sizeNeedMethod.SizeCurveGroupRid;
            //if (sizeHnCurveHash.Contains(sizeHnCurveKey))
            //{
            //    sizeConstraintHash = (Hashtable)sizeHnCurveHash[sizeHnCurveKey];
            //}
            //else
            //{
            //    sizeConstraintHash = new Hashtable();
            //    sizeHnCurveHash.Add(sizeHnCurveKey, sizeConstraintHash);
            //}
            //Hashtable sizeStoreHash;
            //long sizeConstraintKey = (((long)sizeNeedMethod.SizeConstraintRid) << 32) + sizeNeedMethod.SizeAlternateRid;
            //if (sizeConstraintHash.Contains(sizeConstraintKey))
            //{
            //    sizeStoreHash = (Hashtable)sizeConstraintHash[sizeConstraintKey];
            //}
            //else
            //{
            //    sizeStoreHash = new Hashtable();
            //    sizeConstraintHash.Add(sizeConstraintKey, sizeStoreHash);
            //}
            //Hashtable sizeOnhandHash;
            //if (sizeStoreHash.Contains(aStoreRID))
            //{
            //    sizeOnhandHash = (Hashtable)sizeStoreHash[aStoreRID];
            //}
            //else
            //{
            //    sizeOnhandHash = CalculateStoreSizeOnHand(aPlanOnHandTotal, sizeNeedMethod.GetSizeCurve(aIKT.ColorRID, aStoreRID)); 
            //    sizeStoreHash.Add(aStoreRID, sizeOnhandHash);
            //}
            //if (sizeOnhandHash.Contains(aIKT.SizeRID))
            //{
            //    return ((int[])sizeOnhandHash[aIKT.SizeRID])[1];
            //}
            //return 0;
            // end TT#3060 - Loehmans - Jellis - System Argument Exception
		}
        //private Dictionary<int, int> CalculateStoreSizeOnHand (int aPlanOnHandTotal, SizeCurveProfile aSizeCurveProfile)  // TT#3060 - Loehmans - Jellis - System Argument Exception
		private Dictionary<int, int> CalculateStoreSizeOnHand (int aPlanOnHandTotal, SizeCurveProfile aSizeCurveProfile)    // TT#3050 - Loehmans - Jellis - System Argument Exception
        {
            // begin TT#3060 - Loehmans - Jellis - System Argument Exception
            Dictionary<int, int> sizeOnhandDict = new Dictionary<int, int>();
            // Begin TT#4193 - JSmith - error message in allocation size review
            if (aSizeCurveProfile == null ||
                aSizeCurveProfile.SizeCodeList == null)
            {
                return sizeOnhandDict;
            }
            // End  TT#4193 - JSmith - error message in allocation size review
            int[] sizeRID = new int[aSizeCurveProfile.SizeCodeList.Count];
            long[] sizePercent = new long[aSizeCurveProfile.SizeCodeList.Count];
            ArrayList sizeCodeProfiles = aSizeCurveProfile.SizeCodeList.ArrayList;
            int i = 0;
            long totalPercent = 0;
            long decimalPrecision =
                (long)Math.Pow(10, Include.DecimalPositionsInStoreSizePctToColor);
            foreach (SizeCodeProfile scp in sizeCodeProfiles)
            {
                sizeRID[i] = scp.Key;
                sizePercent[i] =
                    (long)(((double)aSizeCurveProfile.GetSizePercent(scp.Key)
                    * (double)decimalPrecision
                    * 100.0d)
                    + 0.5d);
                totalPercent += sizePercent[i];
                i++;
            }
            Array.Sort(sizePercent, sizeRID);
            int newTotal = aPlanOnHandTotal;
            int sizeOnhand;
            for (int sizeEntry = 0; sizeEntry < aSizeCurveProfile.SizeCodeList.Count; sizeEntry++)
            {
                if (totalPercent > 0)
                {
                    sizeOnhand = (int)(((double)newTotal * (double)sizePercent[sizeEntry] / (double)totalPercent) + .5d);
                }
                else
                {
                    sizeOnhand = 0;
                }
                newTotal -= sizeOnhand;
                totalPercent -= sizePercent[sizeEntry];
                sizeOnhandDict.Add(sizeRID[sizeEntry], sizeOnhand);
            }
            return sizeOnhandDict;

            //Hashtable sizeOnhandHash = new Hashtable();
            //int[] sizeRID = new int[aSizeCurveProfile.SizeCodeList.Count];
            //long[] sizePercent = new long[aSizeCurveProfile.SizeCodeList.Count];
            //ArrayList sizeCodeProfiles = aSizeCurveProfile.SizeCodeList.ArrayList;
            //int i = 0;
            //long totalPercent = 0;
            //long decimalPrecision =
            //    (long)Math.Pow(10, Include.DecimalPositionsInStoreSizePctToColor);
            //foreach (SizeCodeProfile scp in sizeCodeProfiles)
            //{
            //    sizeRID[i] = scp.Key;
            //    sizePercent[i] = 
            //        (long)(((double)aSizeCurveProfile.GetSizePercent(scp.Key)
            //        * (double)decimalPrecision
            //        * 100.0d)
            //        + 0.5d);
            //    totalPercent += sizePercent[i];
            //    i++;
            //}
            //Array.Sort(sizePercent, sizeRID);
            //int newTotal = aPlanOnHandTotal;
            //int sizeOnhand;
            //for(int sizeEntry=0; sizeEntry < aSizeCurveProfile.SizeCodeList.Count; sizeEntry++)
            //{
            //    if (totalPercent > 0)
            //    {
            //        sizeOnhand = (int)(((double)newTotal * (double)sizePercent[sizeEntry] / (double)totalPercent) + .5d);
            //    }
            //    else
            //    {
            //        sizeOnhand = 0;
            //    }
            //    newTotal -= sizeOnhand;
            //    totalPercent -= sizePercent[sizeEntry];
            //    int[] sizeRIDonhand = {sizeRID[sizeEntry], sizeOnhand};
            //    sizeOnhandHash.Add(sizeRID[sizeEntry], sizeRIDonhand);
            //}
            //return sizeOnhandHash;
            // end TT#3060 - Loehmans - Jellis - System Argument Exception
		}
		// END MID Track # 2937 Size OnHand Incorrect
		#endregion StoreOnHand

		#region DailyInventoryPlan
		/// <summary>
		/// Determines the daily inventory for a given hierarchy node, store list and week profile
		/// </summary>
		/// <param name="aHnRID">Merchandise Hierarchy Node RID</param>
		/// <param name="aStoreList">A profile list of store profiles</param>
		/// <param name="aWeekProfile">Week profile for which daily inventory is required</param>
		/// <returns>Hashtable of keyed pair (store RID and Integer Array of store Inventory for the week.</returns>
		internal Hashtable DetermineDailyInventory(
			int aHnRID,  
			ProfileList aStoreList, 
			WeekProfile aWeekProfile)
		{
			Hashtable storeInventoryHash = new Hashtable();
			PlanCube storePlanCube = this.GetAllocationPlanCube(aHnRID, aWeekProfile.Date, aWeekProfile.Date.AddDays(7));
  
			PlanCellReference planCellRef = new PlanCellReference((PlanCube)storePlanCube);
			planCellRef[eProfileType.Version] = Include.FV_ActionRID;
			planCellRef[eProfileType.HierarchyNode] = aHnRID;
			//			planCellRef[eProfileType.Week] = aWeekProfile.YearWeek;
			planCellRef[eProfileType.Week] = aWeekProfile.Key;
			planCellRef[eProfileType.QuantityVariable] = PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
			ArrayList storeSalesPlan;
			if (eOTSPlanLevelType.Regular == this.GetNodeData(aHnRID).OTSPlanLevelType)
			{
				planCellRef[eProfileType.Variable] = PlanComputations.PlanVariables.SalesRegPromoUnitsVariable.Key;
			}
			else
			{
				planCellRef[eProfileType.Variable] = PlanComputations.PlanVariables.SalesTotalUnitsVariable.Key;
			}
			storeSalesPlan = planCellRef.GetCellRefArray(aStoreList);
			
			StoreProfile storeProfile;
			double[] dailySales;
			for (int i=0; i < aStoreList.Count; i++)
			{
				storeProfile = (StoreProfile)aStoreList.ArrayList[i];
				dailySales = this.GetDailySales(
					storePlanCube,
					aHnRID,
					// begin MID Track 5953 Null Reference when executing General Method
					//StoreIndexRID(storeProfile.Key),
					GetStoreIndexRID(storeProfile.Key),
					// end MID Track 5953 Null Reference when executing General Method
					aWeekProfile);
				storeInventoryHash.Add(
					storeProfile.Key,
					this.GetStoreDailyInventory(aHnRID, storeProfile, aWeekProfile, dailySales));   //, 100.0d));
			}
			return storeInventoryHash;
		}

		/// <summary>
		/// Gets store daily inventory array for a given week.
		/// </summary>
		/// <param name="aPlanHnRID">Plan Hierarchy Node RID</param>
		/// <param name="aStoreProfile">Store Profile</param>
		/// <param name="aWeekProfile">Week Profile</param>
		/// <param name="aDailySales">Daily Sales</param>
		/// <returns></returns>
		internal int[] GetStoreDailyInventory(
			int aPlanHnRID, 
			StoreProfile aStoreProfile, 
			WeekProfile aWeekProfile, 
			double[] aDailySales)
			//			double aPlanFactor)
		{
			ArrayList dayProfileList = new ArrayList();
			//			dayProfileList.Add(_transaction.SAB.ApplicationServerSession.Calendar.GetDay(aWeekProfile.Date));
			PlanCube storePlanCube = GetAllocationPlanCube(aPlanHnRID, aWeekProfile.Date, aWeekProfile.Date.AddDays(7));

			PlanCellReference planCellRef = new PlanCellReference((PlanCube)storePlanCube);
			planCellRef[eProfileType.Version] = Include.FV_ActionRID;
			planCellRef[eProfileType.HierarchyNode] = aPlanHnRID;
			//			planCellRef[eProfileType.Week] = aWeekProfile.YearWeek;
			planCellRef[eProfileType.Week] = aWeekProfile.Key;
			planCellRef[eProfileType.QuantityVariable] = PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
			if (eOTSPlanLevelType.Regular == this.GetNodeData(aPlanHnRID).OTSPlanLevelType)
			{
				planCellRef[eProfileType.Variable] = PlanComputations.PlanVariables.InventoryRegularUnitsVariable.Key;
			}
			else
			{
				planCellRef[eProfileType.Variable] = PlanComputations.PlanVariables.InventoryTotalUnitsVariable.Key;
			}

			ProfileList pl = new ProfileList(eProfileType.Store);
			pl.Add(aStoreProfile);
			double storeBOWValue = 
				((PlanCellReference)(planCellRef.GetCellRefArray(pl)[0])).CurrentCellValue;
			//			planCellRef[eProfileType.Week] = this.SAB.ApplicationServerSession.Calendar.GetWeek(aWeekProfile.Date.AddDays(7)).YearWeek;
			planCellRef[eProfileType.Week] = this.SAB.ApplicationServerSession.Calendar.GetWeek(aWeekProfile.Date.AddDays(7)).Key;

			double storeEOWValue = 
				((PlanCellReference)(planCellRef.GetCellRefArray(pl)[0])).CurrentCellValue;


			int totalReceipts;
			int salesToGo = 0;
			for (int i=0; i<aDailySales.Length ; i++)
			{
				salesToGo += (int)aDailySales[i];
			}
			int salesDayCount = 7;
			int receiptsToGo;
			int receiptDayCount;
			totalReceipts = 
				(int)storeEOWValue
				- (int)storeBOWValue
				+ salesToGo;
			receiptsToGo = totalReceipts;
			bool[] receiptDay = (bool[]) GetInStoreReceiptDays(aStoreProfile.Key);
			receiptDayCount = 0;
			foreach (bool b in receiptDay)
			{
				if (b)
				{
					receiptDayCount++;
				}
			}
			int[] dailyInventory = new int[aDailySales.Length];
			dailyInventory[0] = (int)storeBOWValue;
			int yesterday = 0;
			int yesterdayReceipts;
			double intermediateValue;
			int avgReceipts;
			int avgSalesToGo;
			for (int today=1; today<aDailySales.Length; today++)
			{
				salesToGo -= (int)aDailySales[yesterday];
				salesDayCount--;
				yesterdayReceipts = 0;
				if (receiptDay[yesterday])
				{
					if (receiptDayCount <= 1)
					{
						yesterdayReceipts = receiptsToGo;
					}
					else
					{
						intermediateValue = ((double)receiptsToGo / (double) receiptDayCount);
						if (intermediateValue < 0)
						{
							avgReceipts = (int)(intermediateValue - .5d);
						}
						else
						{
							avgReceipts = (int)(intermediateValue + .5d);
						}
						intermediateValue = ((double)salesToGo / (double) salesDayCount);
						if (intermediateValue < 0)
						{
							avgSalesToGo = (int)(intermediateValue - .5d);
						}
						else
						{
							avgSalesToGo = (int)(intermediateValue + .5d);
						}
						int nextReceiptDay;
						for (nextReceiptDay=today; nextReceiptDay<aDailySales.Length && !receiptDay[nextReceiptDay]; nextReceiptDay++);
						int daysToNextReceipt = nextReceiptDay - yesterday;
						int salesToNextReceipt = 0;
						for (int r=today; r<=nextReceiptDay; r++)
						{
							salesToNextReceipt += (int)aDailySales[r];
						}
						int salesAdj = 
							salesToNextReceipt
							- (daysToNextReceipt * avgSalesToGo);
						yesterdayReceipts = 
							avgReceipts
							+ salesAdj;
						if (yesterdayReceipts > receiptsToGo)
						{
							yesterdayReceipts = receiptsToGo;
						}
						if (yesterdayReceipts < 0)
						{
							yesterdayReceipts = 0;
						}
					
						dailyInventory[today] =
							dailyInventory[yesterday]
							- (int)aDailySales[yesterday]
							+ yesterdayReceipts;
						if (dailyInventory[today] < salesToNextReceipt)
						{
							yesterdayReceipts =
								yesterdayReceipts
								+ salesToNextReceipt
								- dailyInventory[yesterday];
						}
						if (receiptsToGo > 0 &&
							yesterdayReceipts > receiptsToGo)
						{
							yesterdayReceipts = receiptsToGo;
						}
					}
					receiptDayCount--;
					receiptsToGo -= yesterdayReceipts;
				}
				dailyInventory[today] =
					dailyInventory[yesterday]
					- (int)aDailySales[yesterday]
					+ yesterdayReceipts;
				yesterday = today;
			}
			//			for(int d=0; d<7 ; d++)
			//			{
			//				dailyInventory[d] =
			//					(int)(((double)dailyInventory[d]
			//					* aPlanFactor
			//					/ 100) + 0.5d);
			//			}
			return dailyInventory;
		}

		/// <summary>
		/// Gets the in-store receipt days for a given store.
		/// </summary>
		/// <param name="aStoreRID">RID of the store</param>
		/// <returns>Array of bool corresponding to days of the week.  True indicates a pick/ship day; False indicates store is not picked/shipped on that day.</returns>
		internal bool[] GetInStoreReceiptDays (int aStoreRID)
		{
			return (bool[])((this.GetInStoreReceiptDays())[aStoreRID]);
		}
		#endregion DailyInventoryPlan

		#region Methods

		/// <summary>
		/// Retrieves method information for an unknown method type
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <returns>An instance of the ApplicationBaseAction class with method information</returns>
		public ApplicationBaseAction GetUnknownMethod(int aMethodRID)
		{
			try
			{
				return SAB.ApplicationServerSession.GetMethods.GetUnknownMethod(aMethodRID, false);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieves a list of the selected headers
		/// </summary>
		public ProfileList GetSelectedHeaders()
		{
			try
			{
				SelectedHeaderList selectedHeaderList = new SelectedHeaderList(eProfileType.SelectedHeader);

				if (NeedHeaders)
				{
					selectedHeaderList = (SelectedHeaderList)GetProfileList(eProfileType.SelectedHeader);
				}

				return selectedHeaderList;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		// Begin TT#1194-MD - stodd - view ga header
        public bool IsGroupAllocationHeaderOnly()
        {
            bool isGAOnly = true;
            ProfileList ahpl = GetMasterProfileList(eProfileType.Allocation);
            if (ahpl != null)
            {
                foreach (AllocationProfile ap in ahpl.ArrayList)
                {
                    if (ap.HeaderType == eHeaderType.Assortment && ap.AsrtType == (int)eAssortmentType.GroupAllocation)
                    {
                        // group allocation
                    }
                    else
                    {
                        isGAOnly = false;
                    }
                }
            }

            return isGAOnly;
        }
		// End TT#1194-MD - stodd - view ga header

		// Begin TT#1019 - MD - stodd - prohibit allocation actions against GA - 
        /// <summary>
        /// returns "true" if any selected headers belong to a Group Allocation.
        /// </summary>
        /// <returns></returns>
        public bool ContainsGroupAllocationHeaders()
        {
            ArrayList headerInGAList = new ArrayList();
            return ContainsGroupAllocationHeaders(ref headerInGAList);
        }

        /// <summary>
        /// returns "true" if any selected headers belong to a Group Allocation.
        /// </summary>
        /// <returns></returns>
        public bool ContainsGroupAllocationHeaders(ref ArrayList headerInGAList)
        {
            if (headerInGAList == null)
            {
                headerInGAList = new ArrayList(); 
            }
            bool containsGroupAllocationHeaders = false;
            ProfileList ahpl = GetMasterProfileList(eProfileType.Allocation);
            ProfileList aspl = GetMasterProfileList(eProfileType.AssortmentMember);
            if (aspl != null)
            {
                foreach (AllocationProfile ap in ahpl.ArrayList)
                {
                    if (ap.AsrtRID != Include.NoRID)
                    {
                        AssortmentProfile asp = (AssortmentProfile)aspl.FindKey(ap.AsrtRID);
                        if (asp != null && asp.AsrtType == (int)eAssortmentType.GroupAllocation)
                        {
                            containsGroupAllocationHeaders = true;
                            headerInGAList.Add(ap.HeaderID);
                        }
                    }
                }
            }

            return containsGroupAllocationHeaders;
        }

		// Begin TT#1154-MD - stodd - null reference when opening selection - 
        public bool ContainsGroupAllocationHeaders(ref ArrayList headerInGAList, SelectedHeaderList selectedHeaderList)
        {
            if (headerInGAList == null)
            {
                headerInGAList = new ArrayList();
            }
            bool containsGroupAllocationHeaders = false;
            foreach (SelectedHeaderProfile shp in selectedHeaderList)
            {
                if (shp.AsrtRID != Include.NoRID)
                {
                    containsGroupAllocationHeaders = true;
                    headerInGAList.Add(shp.HeaderID);
                }
            }

            return containsGroupAllocationHeaders;
        }
		// End TT#1154-MD - stodd - null reference when opening selection - 
		// End TT#1019 - MD - stodd - prohibit allocation actions against GA - 
		
		/// <summary>
		/// Processes the method requests
		/// </summary>
		/// <param name="aMethodType">The type of method to process</param>
		/// <param name="aMethodRID">The record ID of the method</param>
		public void ProcessMethod(eMethodType aMethodType, int aMethodRID)
		{
			try
			{
				int storeFilterRID = Include.NoRID;
				ApplicationBaseAction applicationAction = null;
				Profile methodProfile = null;
                //SelectedHeaderList selectedHeaderList = new SelectedHeaderList(eProfileType.SelectedHeader);  // TT#488 - MD - Jellis - Group Allocation
                bool headersSelected = false;                                                                   // TT#488 - MD - Jellis - Group Allocation

				//==================================================================
				// NeedHeaders is set in ProcessAction of WorkflowMethodFormBase.cs
				// Only Allocation processes needs the headers loaded.
				//==================================================================
				if (NeedHeaders)
				{
                    // begin TT#488 - MD - Jellis - Group Allocation
					// Begin TT#980 - MD - stodd - null ref running size need -
                    LoadHeadersInTransaction((SelectedHeaderList)GetProfileList(eProfileType.SelectedHeader));
                    //AllocationProfileList apl = CreateMasterAllocationProfileListFromSelectedHeaders();
                    AllocationProfileList apl = (AllocationProfileList)GetMasterProfileList(eProfileType.Allocation);
					// End TT#980 - MD - stodd - null ref running size need -
                    headersSelected = (apl.Count > 0);
                    //==================================================================
                    // Checks to be sure none of the selected headers are 'in use'
                    // by a multi-header.
                    //==================================================================
                    ArrayList headerInUseList = new ArrayList();
                    foreach (AllocationProfile ap in apl.ArrayList)
                    {
                        if (ap.InUseByMulti)
                        {
                            headerInUseList.Add(ap.HeaderID);
                        }
                    }
                    if (headerInUseList.Count > 0)
                    {
                        throw new HeaderInUseException(MIDText.GetText(eMIDTextCode.msg_al_HeaderInUse), headerInUseList);
                    }

                    // Begin TT#4120 - stodd - GA header highligthed and run an OTS Forecast method.  Receive a mssg.  If it's a non GA header the OTS forecast processes as expected.  Do not expect to receive a message.
                    if (Enum.IsDefined(typeof(eAllocationMethodType), (eAllocationMethodType)aMethodType))
                    {
                        // Begin TT#1019 - MD - stodd - prohibit allocation actions against GA - 
                        ArrayList headerInGAList = new ArrayList();
                        if (ContainsGroupAllocationHeaders(ref headerInGAList))
                        {
                            throw new HeaderInGroupAllocationException(MIDText.GetText(eMIDTextCode.msg_al_HeaderBelongsToGroupAllocation), headerInGAList);
                        }
                        // End TT#1019 - MD - stodd - prohibit allocation actions against GA - 
                    }
                    // End TT#4120 - stodd - GA header highligthed and run an OTS Forecast method.  Receive a mssg.  If it's a non GA header the OTS forecast processes as expected.  Do not expect to receive a message.

                }
                    //AllocationProfileList apl = new AllocationProfileList(eProfileType.Allocation);
                    //selectedHeaderList = (SelectedHeaderList)GetProfileList(eProfileType.SelectedHeader);
                    //if (selectedHeaderList.Count > 0)
                    //{
                    //     // Begin J.Ellis: Is this correct?????
                    //    // Changed for assortment
                    //    //int headerIndex = 0;
                    //    //int[] selectedHeaders = new int[selectedHeaderList.Count];
                    //    //foreach (SelectedHeaderProfile shp in selectedHeaderList)
                    //    //{
                    //    //	selectedHeaders[headerIndex] = shp.Key;
                    //    //	++headerIndex;
                    //    //}
                    //    //apl.LoadHeaders(this, selectedHeaders, SAB.ApplicationServerSession);
                    //    apl.LoadHeaders(this, selectedHeaderList, SAB.ApplicationServerSession);
                    //    // End changed for Assortment
                    //    // End J.Ellis:  Is this correct?????
                    //    SetMasterProfileList(apl);

                    //    //==================================================================
                    //    // Checks to be sure none of the selected headers are 'in use'
                    //    // by a multi-header.
                    //    //==================================================================
                    //    ArrayList headerInUseList = new ArrayList();
                    //    foreach (AllocationProfile ap in apl.ArrayList)
                    //    {
                    //        if (ap.InUseByMulti)
                    //        {
                    //            headerInUseList.Add(ap.HeaderID);
                    //        }
                    //    }
                    //    if (headerInUseList.Count > 0)
                    //    {
                    //        throw new HeaderInUseException(MIDText.GetText(eMIDTextCode.msg_al_HeaderInUse), headerInUseList);
                    //    }
                    //}
                //}
                // end TT#488 - MD - Jellis - Group Allocation
				switch (aMethodType)
				{
					case eMethodType.ForecastBalance:
						applicationAction = new OTSForecastBalanceMethod(SAB, aMethodRID);
						break;
					case eMethodType.OTSPlan:
						applicationAction = new OTSPlanMethod(SAB, aMethodRID);
						methodProfile = new OTSPlanProfile(SAB, Include.UndefinedMethodRID);
						break;
					//Begin TT#523 - JScott - Duplicate folder when new folder added
					//case eMethodType.CopyChainForecast:
					//case eMethodType.CopyStoreForecast:
					//    applicationAction = new OTSForecastCopyMethod(SAB, aMethodRID, aMethodType);
					//    methodProfile = new OTSPlanProfile(SAB, Include.UndefinedMethodRID);
					//    break;
					case eMethodType.CopyChainForecast:
						applicationAction = new OTSForecastCopyMethod(SAB, aMethodRID, aMethodType, eProfileType.MethodCopyChainForecast);
						methodProfile = new OTSPlanProfile(SAB, Include.UndefinedMethodRID);
						break;
					case eMethodType.CopyStoreForecast:
						applicationAction = new OTSForecastCopyMethod(SAB, aMethodRID, aMethodType, eProfileType.MethodCopyStoreForecast);
						methodProfile = new OTSPlanProfile(SAB, Include.UndefinedMethodRID);
						break;
					//End TT#523 - JScott - Duplicate folder when new folder added
//Begin Enhancement - JScott - Export Method - Part 2
					case eMethodType.Export:
						applicationAction = new OTSForecastExportMethod(SAB, aMethodRID);
						methodProfile = new OTSPlanProfile(SAB, Include.UndefinedMethodRID);
						break;
//End Enhancement - JScott - Export Method - Part 2
//Begin Enhancement #5004 - KJohnson - Global Unlock
					case eMethodType.Rollup:
                        applicationAction = new OTSRollupMethod(SAB, aMethodRID);
                        methodProfile = new OTSPlanProfile(SAB, Include.UndefinedMethodRID);
                        break;
                    case eMethodType.GlobalUnlock:
                        applicationAction = new OTSGlobalUnlockMethod(SAB, aMethodRID);
                        methodProfile = new OTSPlanProfile(SAB, Include.UndefinedMethodRID);
                        break;
//End Enhancement #5004 - KJohnson - Global Unlock 
                    //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                    case eMethodType.GlobalLock:
                        applicationAction = new OTSGlobalLockMethod(SAB, aMethodRID);
                        methodProfile = new OTSPlanProfile(SAB, Include.UndefinedMethodRID);
                        break;
                    //End TT#43 - MD - DOConnell - Projected Sales Enhancement
                    case eMethodType.ForecastSpread:
						applicationAction = new OTSForecastSpreadMethod(SAB, aMethodRID);
						methodProfile = new OTSPlanProfile(SAB, Include.UndefinedMethodRID);
						break;
					case eMethodType.ForecastModifySales:
						applicationAction = new OTSForecastModifySales(SAB, aMethodRID);
						methodProfile = new OTSPlanProfile(SAB, Include.UndefinedMethodRID);
						break;
					case eMethodType.GeneralAllocation:
						applicationAction = new AllocationGeneralMethod(SAB, aMethodRID);
						break;
					case eMethodType.AllocationOverride:
						applicationAction = new AllocationOverrideMethod(SAB, aMethodRID);
						break;
					case eMethodType.BasisSizeAllocation:
						applicationAction = new BasisSizeAllocationMethod(SAB, aMethodRID);
						break;
					case eMethodType.FillSizeHolesAllocation:
						applicationAction = new FillSizeHolesMethod(SAB, aMethodRID);
						break;
					case eMethodType.SizeNeedAllocation:
						applicationAction = new SizeNeedMethod(SAB, aMethodRID);
						break;
					case eMethodType.Rule:
						applicationAction = new RuleMethod(SAB, aMethodRID);
						break;
					case eMethodType.Velocity:
						applicationAction = new VelocityMethod(SAB, aMethodRID);
						this.Velocity = (VelocityMethod)applicationAction;
						break;
					case eMethodType.WarehouseSizeAllocation:
						//						applicationAction = new WarehouseSizeAllocationMethod(SAB, aMethodRID);
						break;
					// Begin TT#2 - stodd -assortment
					//case eMethodType.GeneralAssortment:
					//    applicationAction = new GeneralAssortmentMethod(SAB, aMethodRID);
					//    break;
                    // Begin TT#155 - JSmith - Size Curve Method
                    case eMethodType.SizeCurve:
                        applicationAction = new SizeCurveMethod(SAB, aMethodRID);
                        break;
                    // End TT#155
                    case eMethodType.BuildPacks:
                        applicationAction = new BuildPacksMethod(SAB, aMethodRID);
                        break;

					// Begin TT#1652-MD - stodd - DC Carton Rounding
                    case eMethodType.DCCartonRounding:
                        applicationAction = new DCCartonRoundingMethod(SAB, aMethodRID);
                        break;
					// End TT#1652-MD - stodd - DC Carton Rounding
                    // Begin TT#1966-MD - JSmith- DC Fulfillment
                    case eMethodType.CreateMasterHeaders:
                        applicationAction = new CreateMasterHeadersMethod(SAB, aMethodRID);
                        break;
                    case eMethodType.DCFulfillment:
                        applicationAction = new DCFulfillmentMethod(SAB, aMethodRID);
                        break;
                    // End TT#1966-MD - JSmith- DC Fulfillment
					
					default:
						break;
				}
				
				if (applicationAction != null)
				{
                    // Begin TT#155 - JSmith - Size Curve Method
                    //if (Enum.IsDefined(typeof(eForecastMethodType), (eForecastMethodType)aMethodType)
                    if (Enum.IsDefined(typeof(eNoHeaderMethodType), (eNoHeaderMethodType)aMethodType)
                    // End TT#155
                    || (!headersSelected && Enum.IsDefined(typeof(eOptionalHeaderMethodType), (eOptionalHeaderMethodType)aMethodType))  // TT#1966-MD - JSmith - DC Fulfillment
                    || headersSelected)                    // TT#488 - MD - Jellis - Group Allocation
                        //|| selectedHeaderList.Count > 0) // TT#488 - MD - Jellis - Group Allocation
					{
						applicationAction.ProcessMethod(this, storeFilterRID, methodProfile);
					}
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        /// <summary>
        /// Processes the method requests
        /// </summary>
        /// <param name="aMethodType">The type of method to process</param>
        /// <param name="aMethodRID">The record ID of the method</param>
        public void ProcessMethod(eMethodType aMethodType, int aMethodRID, ApplicationBaseAction aApplicationAction)
        {
            try
            {
                int storeFilterRID = Include.NoRID;
                //ApplicationBaseAction applicationAction = null;
                Profile methodProfile = null;
                //SelectedHeaderList selectedHeaderList = new SelectedHeaderList(eProfileType.SelectedHeader); // TT#488 - MD - Jellis - Group Allocation
                bool headersSelected = false;                                                                  // TT#488 - MD - Jellis - Group Allocation

                //==================================================================
                // NeedHeaders is set in ProcessAction of WorkflowMethodFormBase.cs
                // Only Allocation processes needs the headers loaded.
                //==================================================================
                if (NeedHeaders)
                {
                    // begin TT#488 - MD - Jellis - Group Allocation
                    AllocationProfileList apl = CreateMasterAllocationProfileListFromSelectedHeaders();
                    headersSelected = (apl.Count > 0);
                }
                    //AllocationProfileList apl = new AllocationProfileList(eProfileType.Allocation);
                    //selectedHeaderList = (SelectedHeaderList)GetProfileList(eProfileType.SelectedHeader);
                    //if (selectedHeaderList.Count > 0)
                    //{
                    //    int headerIndex = 0;
                    //    int[] selectedHeaders = new int[selectedHeaderList.Count];
                    //    foreach (SelectedHeaderProfile shp in selectedHeaderList)
                    //    {
                    //        selectedHeaders[headerIndex] = shp.Key;
                    //        ++headerIndex;
                    //    }
                    //    apl.LoadHeaders(this, selectedHeaders, SAB.ApplicationServerSession);
                    //    SetMasterProfileList(apl);

                    //    //==================================================================
                    //    // Checks to be sure none of the selected headers are 'in use'
                    //    // by a multi-header.
                    //    //==================================================================
                    //    ArrayList headerInUseList = new ArrayList();
                    //    foreach (AllocationProfile ap in apl.ArrayList)
                    //    {
                    //        if (ap.InUseByMulti)
                    //        {
                    //            headerInUseList.Add(ap.HeaderID);
                    //        }
                    //    }
                    //    if (headerInUseList.Count > 0)
                    //    {
                    //        throw new HeaderInUseException(MIDText.GetText(eMIDTextCode.msg_al_HeaderInUse), headerInUseList);
                    //    }
                    //}
                //}
                // end TT#488 - MD - Jellis - Group Allocation

                if (aApplicationAction != null)
                {
                    if (Enum.IsDefined(typeof(eForecastMethodType), (eForecastMethodType)aMethodType)
                        || headersSelected)              // TT#488 - MD - Jellis - Group Allocation
                        //|| selectedHeaderList.Count > 0) // TT#488 - MD - Jellis - Group Allocaton
                    {
                        aApplicationAction.ProcessMethod(this, storeFilterRID, methodProfile);
                    }
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

		/// <summary>
		/// Processes the workflow requests
		/// </summary>
		/// <param name="aAllocationProfile">The allocation profile to which the workflow applies</param>
		/// <param name="aWorkflowRID">The record ID of the workflow</param>
		/// <param name="aIgnoreReview">True indicates to ignore all review requests and continue processing; false indicates to stop processing when a review request is encountered.</param>
		/// <param name="aWriteToDB">True indicates to write the allocation to the database on successful completion of each request, false incidates that no writes to the database are to occur.</param>
		/// <param name="aRestartAtLine">Workflow line where processing is to begin. Zero or 1 starts at the beginning.</param>
		/// <returns>True if the process action is successful; false if the process action is unsuccessful.</returns> 
		public void ProcessAllocationWorkflow(AllocationProfile aAllocationProfile, int aWorkflowRID, bool aIgnoreReview,
			bool aWriteToDB, int aRestartAtLine)
		{
			AllocationWorkFlow allocationWorkflow;

			if (aAllocationProfile != null)
			{
				//allocationWorkflow = new AllocationWorkFlow(SAB, aWorkflowRID, SAB.ClientServerSession.UserRID, false); // TT#1185 - JEllis - Verify ENQ before Update - (part 2) - remove duplicate processes
                allocationWorkflow = new AllocationWorkFlow(SAB, aWorkflowRID, UserRID, false);                            // TT#1185 - JEllis - Verify ENQ before Update - (part 2) - remove duplicate processes
                if (allocationWorkflow != null)
				{
					AllocationProfileList apl = new AllocationProfileList(eProfileType.Allocation);
					apl.Add(aAllocationProfile);
					allocationWorkflow.Process(this, aIgnoreReview, aWriteToDB, aRestartAtLine, eWorkflowProcessOrder.AllStepsForHeader, apl);				
				}
			}
			else
			{
				throw new MIDException(eErrorLevel.warning,
					(int)eMIDTextCode.msg_al_NoHeadersForWorkflowProcess,
					MIDText.GetText(eMIDTextCode.msg_al_NoHeadersForWorkflowProcess));
			}
		}
		//			}		}
		/// <summary>
		/// Processes the workflow requests
		/// </summary>
		/// <param name="aWorkflowRID">The record ID of the workflow</param>
		/// <param name="aIgnoreReview">True indicates to ignore all review requests and continue processing; false indicates to stop processing when a review request is encountered.</param>
		/// <param name="aWriteToDB">True indicates to write the allocation to the database on successful completion of each request, false incidates that no writes to the database are to occur.</param>
		/// <param name="aRestartAtLine">Workflow line where processing is to begin. Zero or 1 starts at the beginning.</param>
		/// <param name="aWorkflowProcessOrder">Indicates the orders which the headers and steps are to be executed</param>
		/// <returns>True if the process action is successful; false if the process action is unsuccessful.</returns> 
		public void ProcessAllocationWorkflow(int aWorkflowRID, bool aIgnoreReview,
			bool aWriteToDB, int aRestartAtLine, eWorkflowProcessOrder aWorkflowProcessOrder)
		{
			ProcessAllocationWorkflow(aWorkflowRID, aIgnoreReview, aWriteToDB, aRestartAtLine, aWorkflowProcessOrder, true);
		}
		public void ProcessAllocationWorkflow(int aWorkflowRID, bool aIgnoreReview,
			bool aWriteToDB, int aRestartAtLine, eWorkflowProcessOrder aWorkflowProcessOrder, bool aUseSelectedHeaders)
		{
			//			try
			//			{
			AllocationWorkFlow allocationWorkflow;

			if (aUseSelectedHeaders)
			{
				// BEGIN TT#698-MD - Stodd - add ability for workflows to be run against assortments.
				if (!UseAssortmentSelectedHeaders)
				{
				// END TT#698-MD - Stodd - add ability for workflows to be run against assortments.
                    // begin TT#488 - MD - Jellis - Group Allocation
                    AllocationProfileList apl = CreateMasterAllocationProfileListFromSelectedHeaders();
                    //AllocationProfileList apl = new AllocationProfileList(eProfileType.Allocation);
                    //SelectedHeaderList selectedHeaderList = (SelectedHeaderList)GetProfileList(eProfileType.SelectedHeader);
                    //if (selectedHeaderList.Count > 0)
                    //{
                    //    // Begin J.Ellis:  Is this correct??????
                    //    // Changed for Assortment
                    //    //int headerIndex = 0;
                    //    //int[] selectedHeaders = new int[selectedHeaderList.Count];
                    //    //foreach (SelectedHeaderProfile shp in selectedHeaderList)
                    //    //{
                    //    //	selectedHeaders[headerIndex] = shp.Key;
                    //    //	++headerIndex;
                    //    //}
                    //    //apl.LoadHeaders(this, selectedHeaders, SAB.ApplicationServerSession);
                    //    apl.LoadHeaders(this, selectedHeaderList, SAB.ApplicationServerSession);
                    //    // Changed for Assortment
                    //    // End J.Ellis:  Is this correct?????
                    //    SetMasterProfileList(apl);
                    //}	// TT#698-MD - Stodd - add ability for workflows to be run against assortments.
                    // end TT#488 - MD - Jellis - Group Allocation
				}
			}
			//				if (selectedHeaderList.Count > 0)
			if (this.GetAllocationProfileList() != null && this.GetAllocationProfileList().Count > 0)
			{
                //allocationWorkflow = new AllocationWorkFlow(SAB, aWorkflowRID, SAB.ClientServerSession.UserRID, false);  // TT#1185 - JEllis - Verify ENQ before Update - (part 2) - remove duplicate processes
                allocationWorkflow = new AllocationWorkFlow(SAB, aWorkflowRID, UserRID, false);                            // TT#1185 - JEllis - Verify ENQ before Update - (part 2) - remove duplicate processes
				if (allocationWorkflow != null)
				{
					allocationWorkflow.Process(this, aIgnoreReview, aWriteToDB, aRestartAtLine, aWorkflowProcessOrder);
				}
			}
			else
			{
				throw new MIDException(eErrorLevel.warning,
					(int)eMIDTextCode.msg_al_NoHeadersForWorkflowProcess,
					MIDText.GetText(eMIDTextCode.msg_al_NoHeadersForWorkflowProcess));
			}
			//			}
			//			catch ( Exception err )
			//			{
			//				throw;
			//			}
		}

		public void ProcessOTSPlanWorkflow(int aWorkflowRID, bool aIgnoreReview,
			bool aWriteToDB, int aRestartAtLine)
		{
			try
			{
                //OTSPlanWorkFlow OTSPlanWorkFlow = new OTSPlanWorkFlow(SAB, aWorkflowRID, SAB.ClientServerSession.UserRID, false); // TT#1185 - JEllis - Verify ENQ before Update - (part 2) - remove duplicate processes
                OTSPlanWorkFlow OTSPlanWorkFlow = new OTSPlanWorkFlow(SAB, aWorkflowRID, UserRID, false);                           // TT#1185 - JEllis - Verify ENQ before Update - (part 2) - remove duplicate processes 
				if (OTSPlanWorkFlow != null)
				{
					OTSPlanWorkFlow.Process(this, aIgnoreReview, aWriteToDB, aRestartAtLine, eWorkflowProcessOrder.AllStepsForHeader);
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieves method information for an unknown method type
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <param name="returnFullMethod">A flag to identify if the routine is to return only base level method 
		/// information or return the full method based on the method type</param>
		/// <returns>An instance of the ApplicationBaseAction class with method information</returns>
		public ApplicationBaseAction GetUnknownMethod(int aMethodRID, bool returnFullMethod)
		{
			try
			{
				return SAB.ApplicationServerSession.GetMethods.GetUnknownMethod(aMethodRID, returnFullMethod);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieve an OTS Plan method definition
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <returns>An instance of the OTSPlanMethod class with method information</returns>
		public OTSPlanMethod GetOTSPlanMethod(int aMethodRID)
		{
			try
			{
				return SAB.ApplicationServerSession.GetMethods.GetOTSPlanMethod(aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieve an allocation general method definition
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <returns>An instance of the AllocationGeneralMethod class with method information</returns>
		public AllocationGeneralMethod GetAllocationGeneralMethod(int aMethodRID)
		{
			try
			{
				return SAB.ApplicationServerSession.GetMethods.GetAllocationGeneralMethod(aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieve an allocation override method definition
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <returns>An instance of the AllocationOverrideMethod class with method information</returns>
		public AllocationOverrideMethod GetAllocationOverrideMethod(int aMethodRID)
		{
			try
			{
				return SAB.ApplicationServerSession.GetMethods.GetAllocationOverrideMethod(aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieve a fill size holes method definition
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <returns>An instance of the FillSizeHolesMethod class with method information</returns>
		public FillSizeHolesMethod GetFillSizeHolesMethod(int aMethodRID)
		{
			try
			{
				return SAB.ApplicationServerSession.GetMethods.GetFillSizeHolesMethod(aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieve a velocity method definition
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <returns>An instance of the VelocityMethod class with method information</returns>
		public VelocityMethod GetVelocityMethod(int aMethodRID)
		{
			try
			{
				return SAB.ApplicationServerSession.GetMethods.GetVelocityMethod(aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieve a rule method definition
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <returns>An instance of the RuleMethod class with method information</returns>
		public RuleMethod GetRuleMethod(int aMethodRID)
		{
			try
			{
				return SAB.ApplicationServerSession.GetMethods.GetRuleMethod(aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieve a basis size allocation method definition
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <returns>An instance of the BasisSizeAllocationMethod class with method information</returns>
		public BasisSizeAllocationMethod GetBasisSizeAllocationMethod(int aMethodRID)
		{
			try
			{
				return SAB.ApplicationServerSession.GetMethods.GetBasisSizeAllocationMethod(aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieve an allocation warehouse size method definition
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <returns>An instance of the AllocationWareHouseSizeMethod class with method information</returns>
		public AllocationWareHouseSizeMethod GetAllocationWareHouseSizeMethod(int aMethodRID)
		{
			try
			{
				return SAB.ApplicationServerSession.GetMethods.GetAllocationWareHouseSizeMethod(aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a new instance of the object associated with the provided method type
		/// </summary>
		/// <param name="aMethodType">The type of method or action for which the new instance is to be created</param>
		/// <returns>A new instance of the ApplicationBaseMethod class with initialized method or action information</returns>
		public ApplicationBaseAction CreateNewMethodAction(eMethodType aMethodType)
		{
			try
			{
				ApplicationBaseAction abm = null;

				//				abm = new AllocationAction(aMethodType);


				if (Enum.IsDefined(typeof(eAllocationActionType),(eAllocationActionType)aMethodType))
				{
					// its an action
					abm = new AllocationAction(aMethodType);
				}
				else if (Enum.IsDefined(typeof(eAssortmentActionType), (eAssortmentActionType)aMethodType))
				{
					// its an action
					abm = new AssortmentAction(aMethodType);
				}
				else
				{
					// its a method
					switch (aMethodType)
					{
						case eMethodType.OTSPlan:
							abm = new OTSPlanMethod(SAB, Include.NoRID);
							break;
						case eMethodType.GeneralAllocation:
							abm = new AllocationGeneralMethod(SAB, Include.NoRID);
							break;
						case eMethodType.AllocationOverride:
							abm = new AllocationOverrideMethod(SAB, Include.NoRID);
							break;
						case eMethodType.WarehouseSizeAllocation:
							abm = new AllocationWareHouseSizeMethod(SAB, Include.NoRID);
							break;
						case eMethodType.BasisSizeAllocation:
							abm = new BasisSizeAllocationMethod(SAB, Include.NoRID);
							break;
						case eMethodType.FillSizeHolesAllocation:
							abm = new FillSizeHolesMethod(SAB, Include.NoRID);
							break;
						case eMethodType.Rule:
							abm = new RuleMethod(SAB, Include.NoRID);
							break;
						case eMethodType.Velocity:
							abm = new VelocityMethod(SAB, Include.NoRID);
							break;
						// BEGIN TT#217-MD - stodd - unable to run workflow methods against assortment
						case eMethodType.SizeNeedAllocation:
							abm = new SizeNeedMethod(SAB, Include.NoRID);
							break;
						// END TT#217-MD - stodd - unable to run workflow methods against assortment
						default:
							throw new Exception(aMethodType.ToString() + " not implemented");
					}
				}

				return abm;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		#endregion Methods

		// BEGIN MID Track #2937 Size Onhand Not correct
		#region SizeNeedMethod Cache
        // begin TT#41 - Jellis -  MD - Size Inventory Min Max pt 1
        /// <summary>
        /// Gets the SizeNeedMethod for the given Allocation Profile and Size Need Method Key
        /// </summary>
        /// <param name="aAllocationProfile">AllocationProfile associated with the desired SizeNeedMethod (may be null)</param>
        /// <param name="aSizeNeedMethodKey">SizeNeedMethodKey that identifies the size need method.</param>
        /// <returns></returns>
        public SizeNeedMethod GetSizeNeedMethod(AllocationProfile aAllocationProfile, SizeNeedMethodKey aSizeNeedMethodKey)
        {
            // begin TT#3079 - AnF - Jellis - VSW calculation wrong after cancel intransit
            //if (_processingSizeNeedMethod != null)
            //{
            //    return _processingSizeNeedMethod;
            //}
            // end TT#3079 - AnF - Jellis - VSW calculation wrong after cancel intransit
            int headerRID = Include.NoRID;
            if (aAllocationProfile != null)
            {
                headerRID = aAllocationProfile.HeaderRID;
            }
            if (_lastHeaderSizeNeedMethodKey == null
                || _lastHeaderSizeNeedMethodKey.Value.HeaderRID != headerRID
                || _lastHeaderSizeNeedMethodKey.Value.SizeNeedMethodKey != aSizeNeedMethodKey)
            {
                _lastHeaderSizeNeedMethodKey = new HeaderSizeNeedMethodKey(headerRID, aSizeNeedMethodKey);
                _sizeNeedMethodCache.TryGetValue(_lastHeaderSizeNeedMethodKey.Value, out _lastSizeNeedMethod);
                if (_lastSizeNeedMethod == null)
                {
                    _tempSizeNeedMethodRID--;
                    _lastSizeNeedMethod = new SizeNeedMethod(
                        this.SAB,
                        _tempSizeNeedMethodRID,
                        aAllocationProfile,
                        this);
                    _lastSizeNeedMethod.MerchHnRid = aSizeNeedMethodKey.MerchandiseBasisHnRID;
                    _lastSizeNeedMethod.MerchType = aSizeNeedMethodKey.MerchandiseType;
                    _lastSizeNeedMethod.MerchPhRid = _lastHeaderSizeNeedMethodKey.Value.SizeNeedMethodKey.MerchandiseBasisPhRID;
                    _lastSizeNeedMethod.MerchPhlSequence = _lastHeaderSizeNeedMethodKey.Value.SizeNeedMethodKey.MerchandiseBasisPhlSequence;
                    _lastSizeNeedMethod.SizeAlternateRid = _lastHeaderSizeNeedMethodKey.Value.SizeNeedMethodKey.AlternateRID;
                    _lastSizeNeedMethod.SizeConstraintRid = _lastHeaderSizeNeedMethodKey.Value.SizeNeedMethodKey.ConstraintRID;
                    _lastSizeNeedMethod.SizeGroupRid = _lastHeaderSizeNeedMethodKey.Value.SizeNeedMethodKey.SizeGroupRID;
                    _lastSizeNeedMethod.SizeCurveGroupRid = _lastHeaderSizeNeedMethodKey.Value.SizeNeedMethodKey.SizeCurveGroupRID;
                    _lastSizeNeedMethod.Name = "Temp" + Convert.ToString(Math.Abs(_tempSizeNeedMethodRID));
                    _lastSizeNeedMethod.SG_RID = Include.AllStoreFilterRID;
                    _lastSizeNeedMethod.NormalizeSizeCurves = _lastHeaderSizeNeedMethodKey.Value.SizeNeedMethodKey.NormalizeSizeCurves;
                    _lastSizeNeedMethod.NormalizeSizeCurvesDefaultIsOverridden = true;
                    _lastSizeNeedMethod.IB_MERCH_HN_RID = _lastHeaderSizeNeedMethodKey.Value.SizeNeedMethodKey.InventoryBasisMerchandiseHnRID;
                    _lastSizeNeedMethod.IB_MERCH_PH_RID = _lastHeaderSizeNeedMethodKey.Value.SizeNeedMethodKey.InventoryBasisMerchandisePhRID;
                    _lastSizeNeedMethod.IB_MERCH_PHL_SEQ = _lastHeaderSizeNeedMethodKey.Value.SizeNeedMethodKey.InventoryBasisMerchandisePhlSeq;
                    _lastSizeNeedMethod.IB_MerchandiseType = _lastHeaderSizeNeedMethodKey.Value.SizeNeedMethodKey.InventoryBasisMerchandiseType;
                    _lastSizeNeedMethod.VSWSizeConstraints = _lastHeaderSizeNeedMethodKey.Value.SizeNeedMethodKey.VSWSizeConstraints; // TT#246 - MD - JEllis - VSW Size In Store Minimums pt 3
                    _lastSizeNeedMethod.FillSizesToType = _lastHeaderSizeNeedMethodKey.Value.SizeNeedMethodKey.FillSizesToType; //TT#848-MD -jsobek -Fill to Size Plan Presentation
                    _sizeNeedMethodCache.Add(_lastHeaderSizeNeedMethodKey.Value, _lastSizeNeedMethod);
                }
            }
            return _lastSizeNeedMethod;
        }
        /// <summary>
        /// Flushes the Header Size Need Method Cache
        /// </summary>
        /// <param name="aAllocationProfile">Allocation Profile to flush from the cache (null flushes ALL)</param>
        public void FlushSizeNeedMethod(AllocationProfile aAllocationProfile)
        {
            if (aAllocationProfile == null)
            {
                _sizeNeedMethodCache = new Dictionary<HeaderSizeNeedMethodKey, SizeNeedMethod>();
                // begin TT#1022 - MD - Jellis - Size Review for Group Member Missing variables in Col Chooser
                //AllocationProfileList apl = GetAllocationProfileList();
                AllocationProfileList apl = (AllocationProfileList)this.GetMasterProfileList(eProfileType.AssortmentMember);
                if (apl == null)
                {
                    apl = GetAllocationProfileList();
                }
                // end TT#1022 - MD - Jellis - Size Review for Group Member Missing variables in Col Chooser
                foreach (AllocationProfile ap in apl)
                {
                    ap.FlushSizeNeedMethod();
                }
            }
            else
            {
                List<HeaderSizeNeedMethodKey> hsnmkList = new List<HeaderSizeNeedMethodKey>();
                foreach (HeaderSizeNeedMethodKey hsnmk in _sizeNeedMethodCache.Keys)
                {
                    if (aAllocationProfile.Key == hsnmk.HeaderRID)
                    {

                        hsnmkList.Add(hsnmk);
                    }
                }

                foreach (HeaderSizeNeedMethodKey hsnmk in hsnmkList)
                {
                    _sizeNeedMethodCache.Remove(hsnmk);
                }
                aAllocationProfile.FlushSizeNeedMethod();
            }
            _lastSizeNeedMethod = null; // TT#3079 - AnF - Jellis - VSW not calculating correctly after cancel intransit
            _lastHeaderSizeNeedMethodKey = null; // TT#3088 AnF - Jellis - VSW not correct on Workflows with multiple steps
        }
        ///// <summary>
        ///// Gets the Size Need Method for the specified size need method key
        ///// </summary>
        ///// <param name="aAllocationProfile">Allocation Profile to which this Size Need Method applies</param>
        ///// <param name="aSizeCurveGroupRID">Size Curve Group RID</param>
        ///// <param name="aMerchandiseType">Merchandise Type associated with the Size Need Method</param>
        ///// <param name="aHnRID">Hierarchy Node RID associated with the Size Need Method</param>
        ///// <param name="aPhRID">Merchandise PH RID</param>
        ///// <param name="aPHLSeq">Merchandise PHL Sequence</param>
        ///// <param name="aSizeConstraintRID">Size Constraint RID</param>
        ///// <param name="aSizeAlternateRID">Size Alternate RID</param>
        ///// <param name="aNormalizeSizeCurves">True:  Size Curves will be normalized; False: Size Curves will not be normalized</param>
        ///// <returns>SizeNeedMethod for the specified size need method key</returns>
        //// begin MID Track 4861 Size Curve Normalization
        ////public SizeNeedMethod GetSizeNeedMethod (AllocationProfile aAllocationProfile, int aSizeCurveGroupRID, eMerchandiseType aMerchandiseType, int aHnRID, int aPhRID, int aPHLSeq, int aSizeConstraintRID, int aSizeAlternateRID)
        //public SizeNeedMethod GetSizeNeedMethod
        //    (AllocationProfile aAllocationProfile,
        //    int aSizeCurveGroupRID,
        //    eMerchandiseType aMerchandiseType,
        //    int aHnRID, 
        //    int aPhRID, 
        //    int aPHLSeq, 
        //    int aSizeConstraintRID, 
        //    int aSizeAlternateRID,
        //    bool aNormalizeSizeCurves)
        //    // end MID Track 4861 Size Curve Normalization
        //{
        //    // begin TT#702 Size Need Method "hangs" when Processing hdrs with begin date
        //    if (_processingSizeNeedMethod != null)
        //    {
        //        return _processingSizeNeedMethod;
        //    }
        //    // end TT#702 Size Need Method "hangs" when processing hdrs with begin date
        //    long sizeCurveKey;
        //    if (aAllocationProfile == null)
        //    {
        //        sizeCurveKey = (((long)Include.NoRID << 32) + aSizeCurveGroupRID);
        //    }
        //    else
        //    {
        //        sizeCurveKey = (((long)aAllocationProfile.Key <<32) + aSizeCurveGroupRID);
        //    }
        //    long merchTypeKey;
        //    if (aHnRID == Include.NoRID)
        //    {
        //        long tempHnRID = ((long)1 << 32) - (long)1;
        //        merchTypeKey = (((long)aMerchandiseType) << 32) + tempHnRID;
        //    }
        //    else
        //    {
        //        merchTypeKey = (((long)aMerchandiseType) << 32) + aHnRID;
        //    }
        //    long merchPhKey = (((long)aPhRID) << 32) + aPHLSeq;
        //    long sizeCurveModifyKey;
        //    if (aSizeAlternateRID == Include.NoRID)
        //    { 
        //        long tempAlternateRID = ((long)1 << 32) - (long)1;
        //        sizeCurveModifyKey = (((long)aSizeConstraintRID)<<32) + tempAlternateRID;
        //    }
        //    else
        //    {
        //        sizeCurveModifyKey = (((long)aSizeConstraintRID) << 32) + aSizeAlternateRID;
        //    }
        //    int normalizationKey = 0;
        //    if (aNormalizeSizeCurves)
        //    {
        //        normalizationKey = 1;
        //    }
        //    Hashtable sizeMerchType;
        //    SizeNeedMethod sizeNeedMethod;
        //    // begin MID Track 4861 Size Curve Normalization
        //    //if (_sizeCurve.Contains (sizeCurveKey))
        //    //{
        //    //	sizeMerchType = (Hashtable)_sizeCurve[sizeCurveKey];
        //    //	Hashtable sizeMerchPH;
        //    //	if (sizeMerchType.Contains(merchTypeKey))
        //    //	{
        //    //		sizeMerchPH = (Hashtable)sizeMerchType[merchTypeKey];
        //    //		Hashtable sizeCurveHash;
        //    //		if (sizeMerchPH.Contains(merchPhKey))
        //    //		{
        //    //			sizeCurveHash = (Hashtable)sizeMerchPH[merchPhKey];
        //    //			if (sizeCurveHash.Contains(sizeCurveModifyKey))
        //    //			{
        //    //				return (SizeNeedMethod)sizeCurveHash[sizeCurveModifyKey];
        //    //			}
        //    //		}
        //    //	}
        //    //}
        //    //_tempSizeNeedMethodRID--;
        //    //sizeNeedMethod = new SizeNeedMethod(
        //    //	this.SAB,
        //    //	_tempSizeNeedMethodRID,
        //    //	aAllocationProfile, 
        //    //	this);
        //    //sizeNeedMethod.MerchHnRid = aHnRID;
        //    //sizeNeedMethod.MerchType = aMerchandiseType;
        //    //sizeNeedMethod.MerchPhRid = aPhRID;
        //    //sizeNeedMethod.MerchPhlSequence = aPHLSeq;
        //    //sizeNeedMethod.SizeAlternateRid = aSizeAlternateRID;
        //    //sizeNeedMethod.SizeConstraintRid = aSizeConstraintRID;
        //    //sizeNeedMethod.SizeCurveGroupRid = aSizeCurveGroupRID;
        //    //sizeNeedMethod.Name = "Temp" + Convert.ToString(Math.Abs(_tempSizeNeedMethodRID));
        //    //sizeNeedMethod.SG_RID = Include.AllStoreFilterRID;
        //    //this.AddSizeNeedMethod(sizeNeedMethod);
        //    //return sizeNeedMethod;
        //    sizeMerchType = (Hashtable)_sizeCurve[sizeCurveKey];
        //    if (sizeMerchType == null)
        //    {
        //        sizeMerchType = new Hashtable();
        //        _sizeCurve.Add(sizeCurveKey, sizeMerchType);
        //    }
        //    Hashtable sizeMerchPH = (Hashtable)sizeMerchType[merchTypeKey];
        //    if (sizeMerchPH == null)
        //    {
        //        sizeMerchPH = new Hashtable();
        //        sizeMerchType.Add(merchTypeKey, sizeMerchPH);
        //    }
        //    Hashtable sizeCurveHash = (Hashtable)sizeMerchPH[merchPhKey];
        //    if (sizeCurveHash == null)
        //    {
        //        sizeCurveHash = new Hashtable();
        //        sizeMerchPH.Add(merchPhKey, sizeCurveHash);
        //    }
        //    Hashtable normalizationHash = (Hashtable)sizeCurveHash[sizeCurveModifyKey];
        //    if (normalizationHash == null)
        //    {
        //        normalizationHash = new Hashtable();
        //        sizeCurveHash.Add(sizeCurveModifyKey, normalizationHash);
        //    }
        //    sizeNeedMethod = (SizeNeedMethod)normalizationHash[normalizationKey];
        //    if (sizeNeedMethod == null)
        //    {
        //        _tempSizeNeedMethodRID--;
        //        sizeNeedMethod = new SizeNeedMethod(
        //            this.SAB,
        //            _tempSizeNeedMethodRID,
        //            aAllocationProfile, 
        //            this);
        //        sizeNeedMethod.MerchHnRid = aHnRID;
        //        sizeNeedMethod.MerchType = aMerchandiseType;
        //        sizeNeedMethod.MerchPhRid = aPhRID;
        //        sizeNeedMethod.MerchPhlSequence = aPHLSeq;
        //        sizeNeedMethod.SizeAlternateRid = aSizeAlternateRID;
        //        sizeNeedMethod.SizeConstraintRid = aSizeConstraintRID;
        //        sizeNeedMethod.SizeCurveGroupRid = aSizeCurveGroupRID;
        //        sizeNeedMethod.Name = "Temp" + Convert.ToString(Math.Abs(_tempSizeNeedMethodRID));
        //        sizeNeedMethod.SG_RID = Include.AllStoreFilterRID;
        //        sizeNeedMethod.NormalizeSizeCurves = aNormalizeSizeCurves;
        //        sizeNeedMethod.NormalizeSizeCurvesDefaultIsOverridden = true;
        //        normalizationHash.Add(normalizationKey, sizeNeedMethod);
        //    }
        //    return sizeNeedMethod;
        //    // end MID Track 4861 Size Curve Normalization
        //}
        // end TT#41 - MD - JEllis - Size Inventory Min Max
		// begin MID Track 4861 Size Curve Normalization
		// /// <summary>
		// /// Adds SizeNeedMethod to cache when it is not already there
		// /// </summary>
		// /// <param name="aSizeNeedMethod">SizeNeedMethod instance</param>
		//private void AddSizeNeedMethod(SizeNeedMethod aSizeNeedMethod)  // MID Track 4861 Size Curve Normalization
		//{
		//	long sizeCurveKey;
		//	if (aSizeNeedMethod.AllocationProfile == null)
		//	{
		//		sizeCurveKey = (((long)Include.NoRID << 32) + aSizeNeedMethod.SizeCurveGroupRid);
		//	}
		//	else
		//	{
		//		sizeCurveKey = (((long)aSizeNeedMethod.AllocationProfile.Key << 32) + aSizeNeedMethod.SizeCurveGroupRid);
		//	}
		//	long merchTypeKey;
		//	if (aSizeNeedMethod.MerchHnRid == Include.NoRID)
		//	{
		//		long tempHnRID = ((long)1 << 32) - (long)1;
		//		merchTypeKey = (((long)aSizeNeedMethod.MerchType) << 32) + tempHnRID;
		//	}
		//	else
		//	{
		//		merchTypeKey = (((long)aSizeNeedMethod.MerchType) << 32) + aSizeNeedMethod.MerchHnRid;
		//	}
		//	long merchPhKey = (((long)aSizeNeedMethod.MerchPhRid) << 32) + aSizeNeedMethod.MerchPhlSequence;
		//	long sizeCurveModifyKey;
		//	if (aSizeNeedMethod.SizeAlternateRid == Include.NoRID)
		//	{
		//		long tempAlternate = ((long)1 << 32) - (long)1;
		//		sizeCurveModifyKey = (((long)aSizeNeedMethod.SizeConstraintRid << 32) + tempAlternate);
		//	}
		//	else
		//	{
		//		sizeCurveModifyKey = (((long)aSizeNeedMethod.SizeConstraintRid) << 32) + aSizeNeedMethod.SizeAlternateRid;
		//	}
		//	Hashtable sizeMerchType;
		//	if (_sizeCurve.Contains (sizeCurveKey))
		//	{
		//		sizeMerchType = (Hashtable)_sizeCurve[sizeCurveKey];
		//	}
		//	else
		//	{
		//		sizeMerchType = new Hashtable();
		//		_sizeCurve.Add(sizeCurveKey, sizeMerchType);
		//	}
		//	Hashtable sizeMerchPH;
		//	if (sizeMerchType.Contains(merchTypeKey))
		//	{
		//		sizeMerchPH = (Hashtable)sizeMerchType[merchTypeKey];
		//	}
		//	else
		//	{
		//		sizeMerchPH = new Hashtable();
		//		sizeMerchType.Add(merchTypeKey, sizeMerchPH);
		//	}
		//	Hashtable sizeCurveHash;
		//	if (sizeMerchPH.Contains(merchPhKey))
		//	{
		//		sizeCurveHash = (Hashtable)sizeMerchPH[merchPhKey];
		//	}
		//	else
		//	{
		//		sizeCurveHash = new Hashtable();
		//		sizeMerchPH.Add(merchPhKey, sizeCurveHash);
		//	}
		//	if (sizeCurveHash.Contains(sizeCurveModifyKey))
		//	{
		//	}
		//	else
		//	{
		//		sizeCurveHash.Add(sizeCurveModifyKey, aSizeNeedMethod);
		//	}
		//}
        //// end MID Track 4861 Size Curve Normalization
        //// begin TT#2305 - JEllis - FL Size Multiple not observed by balance
        //public void FlushSizeNeedMethod(AllocationProfile aAllocationProfile)
        //{
        //    if (aAllocationProfile == null)
        //    {
        //        _sizeCurve = new Hashtable();
        //        AllocationProfileList apl = GetAllocationProfileList();
        //        foreach (AllocationProfile ap in apl)
        //        {
        //            ap.FlushSizeNeedMethod();
        //        }
        //    }
        //    else
        //    {
        //        List<long> sizeCurveKeys = new List<long>(); // TT#2305 - JEllis - Balance not observing mult pt 2
        //        foreach (long sizeCurveKey in _sizeCurve.Keys)
        //        {
        //            if (aAllocationProfile.Key == (int)(sizeCurveKey >> 32))
        //            {
        //                //_sizeCurve.Remove(sizeCurveKey); // TT#2305 - JEllis - Balance not observing mult pt 2
        //                sizeCurveKeys.Add(sizeCurveKey);   // TT#2305 - JEllis - Balance not observing mult pt 2
        //            }
        //        }
        //        // begin TT#2305 - JEllis - Balance not observing mult pt 2
        //        foreach (long sizeCurveKey in sizeCurveKeys)
        //        {
        //            _sizeCurve.Remove(sizeCurveKey);
        //        }
        //        // end TT#2305 - JEllis - Balance not observing mult pt 2
        //        aAllocationProfile.FlushSizeNeedMethod();
        //    }
        //}
        //// end TT#2305 - JEllis - FL Size Multiple not observed by balance
        // end TT#41 - MD - JEllis - Size Inventory Min Max
		#endregion SizeNeedMethod Cache
		// END MID Track #2937 Size Onhand Not correct

        // begin TT#2155 - JEllis - Fill Size Hole Null Reference
        #region SizeNeedResults Cache
        long _lastSizeNeedResultsKey;
        SizeNeedResults _lastSizeNeedResults;
        Dictionary<long, SizeNeedResults> _sizeNeedResultsDict;
        /// <summary>
        /// Gets the Size Need Results for a given header and color (use "dummy" color to retrieve the results for the detail packs)
        /// </summary>
        /// <param name="aHeaderRID">RID that identifies the header</param>
        /// <param name="aColorRID">Color Code RID that identifies the color or "dummy" color</param>
        /// <returns></returns>
        public SizeNeedResults GetSizeNeedResults(int aHeaderRID, int aColorRID)
        {
            if (_sizeNeedResultsDict == null)
            {
                _sizeNeedResultsDict = new Dictionary<long, SizeNeedResults>();
                _lastSizeNeedResultsKey = 0;
            }
            //if ((int)(_lastSizeNeedResultsKey >> 32) == aHeaderRID     // TT#491 -- MD - Jellis - Argument Exception  
                //&& _lastSizeNeedResults.HeaderColorRid == aColorRID) // TT#491 -- MD - Jellis - Argument Exception
            long sizeNeedResultsKey = (long)((long)aHeaderRID << 32) + (long)aColorRID; // TT#491 -- MD - Jellis - Argument Exception
            if (_lastSizeNeedResultsKey == sizeNeedResultsKey)    // TT#491 -- MD - Jellis - Argument Exception
            {
                return _lastSizeNeedResults;
            }
            //_lastSizeNeedResultsKey = (long)(aHeaderRID << 32) + (long)aColorRID; // TT#491 -- MD - Jellis - Argument Exception 
            _lastSizeNeedResultsKey = sizeNeedResultsKey; // TT#491 -- MD - Jellis - Argument Exception 
            if (!_sizeNeedResultsDict.TryGetValue(_lastSizeNeedResultsKey, out _lastSizeNeedResults)) 
            {
                _lastSizeNeedResults = null;
            }
            return _lastSizeNeedResults;

        }
        /// <summary>
        /// Sets (saves) the SizeNeedResults associated with a given header (each bulk color on the header may have an associated size need result as well as one for the detail packs)
        /// </summary>
        /// <param name="aHeaderRID">RID that identifies the header</param>
        /// <param name="aSizeNeedResults">Size Need Results associated with a color (or the detail packs)</param>
        public void SetSizeNeedResults(int aHeaderRID, SizeNeedResults aSizeNeedResults)
        {
            try
            {
                if (_sizeNeedResultsDict == null)
                {
                    _sizeNeedResultsDict = new Dictionary<long, SizeNeedResults>();
                    _lastSizeNeedResultsKey = 0;
                }
                //_lastSizeNeedResultsKey = (long)(aHeaderRID << 32) + (long)(aSizeNeedResults.HeaderColorRid);  // TT#491 -- MD - Jellis - Argument Exception
                _lastSizeNeedResultsKey = (long)((long)aHeaderRID << 32) + (long)aSizeNeedResults.HeaderColorRid; // TT#491 -- MD - Jellis - Argument Exception
                // begin  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                //_lastSizeNeedResults = aSizeNeedResults;  // TT#491 -- MD - Jellis - Argument Exception
                //_sizeNeedResultsDict.Add(_lastSizeNeedResultsKey, aSizeNeedResults);

                if (_sizeNeedResultsDict.TryGetValue(_lastSizeNeedResultsKey, out _lastSizeNeedResults))
                {
                    if (_lastSizeNeedResults.InstanceID != aSizeNeedResults.InstanceID)
                    {
                        _lastSizeNeedResults = aSizeNeedResults;  // TT#491 -- MD - Jellis - Argument Exception
                        _sizeNeedResultsDict.Remove(_lastSizeNeedResultsKey);
                        _sizeNeedResultsDict.Add(_lastSizeNeedResultsKey, aSizeNeedResults);
                    }
                } 
                else
                {
                    _lastSizeNeedResults = aSizeNeedResults;  // TT#491 -- MD - Jellis - Argument Exception
                    _sizeNeedResultsDict.Add(_lastSizeNeedResultsKey, aSizeNeedResults);
                }
                // end  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
            }
            catch (System.ArgumentException)
            {
                _sizeNeedResultsDict.Remove(_lastSizeNeedResultsKey);
                _sizeNeedResultsDict.Add(_lastSizeNeedResultsKey, aSizeNeedResults);
            }
        }
        // begin TT#3085 - AnF - Jellis - VSW incorrect after cancel intransit in Size/Style reviews
        public void ClearSizeNeedResults(int aHeaderRID, int aColorRID)
        {
            if (_sizeNeedResultsDict != null)
            {
                _lastSizeNeedResultsKey = 0;
                _sizeNeedResultsDict.Remove((long)((long)aHeaderRID << 32) + (long)(aColorRID));
                // begin  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                if (_sizeNeedResultsDict.Count == 0)
                {
                    _sizeNeedResultsDict = null;
                }
                // end  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
            }
        }
        // end TT#3085 - AnF - Jellis - VSW incorrect after cancel intransit in Size/Style reviews
        /// <summary>
        /// Clears out the SizeNeedResults for the color leaving only the Size Curves. If no SizeNeedResults is found,
        /// nothing is done.
        /// </summary>
        public void ClearSizeNeedResults_Plan(int aHeaderRID, int aColorRID, bool aClearSizeCurves)
        {
            SizeNeedResults snr = GetSizeNeedResults(aHeaderRID, aColorRID);
            if (snr != null)
            {
                snr.Clear(aClearSizeCurves);
                snr.ProcessControl = eSizeProcessControl.SizeCurvesOnly;
            }
        }
        #endregion SizeNeedAlgorithm Cache
        // end TT#2155 - JEllis - Fill Size Hole Null Reference
		
		#region LoadHeaders
        //Begin TT#739-MD -jsobek -Delete Stores -Headers Loaded Multiple Times - Unused function
		/// <summary>
		/// Loads all Allocation Profiles to the Profile List.
		/// </summary>
        //public void LoadAll()
        //{
        //    ((AllocationProfileList)GetMasterProfileList(eProfileType.Allocation)).LoadAll(this, SAB.ApplicationServerSession);
        //}
        //End TT#739-MD -jsobek -Delete Stores -Headers Loaded Multiple Times - Unused function
		/// <summary>
		/// Creates Allocation Profiles and adds them to this Profile List.
		/// </summary>
        /// <param name="aAssortmentRIDList">List of Assortment RIDs (excluding Header RIDs)</param>
		/// <param name="aHeaderRIDList">List of Header RIDs (excluding AssortmentRIDs</param>
		public void LoadHeaders(int[] aAssortmentRIDList, int[] aHeaderRIDList) // TT#488 - MD - Jellis - Group Allocation
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            CreateMasterAllocationProfileListFromSelectedHeaders(aAssortmentRIDList, aHeaderRIDList);
            //SelectedHeaderList aHeaderList = new SelectedHeaderList(eProfileType.SelectedHeader);
            //foreach (int headerRid in aHeaderRIDList)
            //{
            //    // Begin Assortment -- added header lookup to get type
            //    // Begin TT32 - stodd - froce get of header
            //    SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(headerRid);
            //    AllocationHeaderProfile allocHeaderProfile = SAB.HeaderServerSession.GetHeaderData(headerRid, false, false, true);
            //    // End TT32 - stodd - froce get of header

            //    selectedHeader.HeaderType = allocHeaderProfile.HeaderType;
            //    // End Assortment
            //    aHeaderList.Add(selectedHeader);
            //}
            //LoadHeaders(aHeaderList);
            // ENd TT#488 - MD - Jellis - Group Allocation
		}
		/// <summary>
		/// Creates Allocation Profiles and adds them to this Profile List.
		/// </summary>
        /// <param name="aHeaderList">List of Header RIDs</param>
        public void LoadHeaders(SelectedHeaderList aHeaderList)
        {
            // begin TT#488 - MD - Jellis - Group Allocation 
            CreateMasterAllocationProfileListFromSelectedHeaders(aHeaderList);
        }
        // end TT#488 - MD - Jellis - Group Allocation
        //    AllocationProfileList apl = (AllocationProfileList)this.GetMasterProfileList(eProfileType.Allocation);
        //    if (apl != null)
        //    {
        //        apl.Clear();
        //    }
        //    else
        //    {
        //        NewAllocationMasterProfileList();
        //        apl = (AllocationProfileList)this.GetMasterProfileList(eProfileType.Allocation);
        //    }
        //    apl.LoadHeaders(this, aHeaderList, SAB.ApplicationServerSession);
        //}
		#endregion LoadHeaders

		// BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
		#region LoadAssortmentMemberHeader
		/// <summary>
		/// Loads all Allocation Profiles to the Profile List.
		/// </summary>
		public void LoadAllAssortmentMember()
		{
			((AllocationProfileList)GetMasterProfileList(eProfileType.AssortmentMember)).LoadAll(this, SAB.ApplicationServerSession);
		}

        // begin TT#488 - MD - Jellis - Group Allocation
        ///// <summary>
        ///// Creates Allocation Profiles and adds them to this Profile List.
        ///// </summary>
        ///// <param name="aHeaderRIDList">List of Header RIDs</param>
        //public void LoadAssortmentMemberHeaders(int[] aHeaderRIDList)
        //{
        //    SelectedHeaderList aHeaderList = new SelectedHeaderList(eProfileType.SelectedHeader);
        //    foreach (int headerRid in aHeaderRIDList)
        //    {
        //        // Begin Assortment -- added header lookup to get type
        //        // Begin TT32 - stodd - froce get of header
        //        SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(headerRid);
        //        AllocationHeaderProfile allocHeaderProfile = SAB.HeaderServerSession.GetHeaderData(headerRid, false, false, true);
        //        // End TT32 - stodd - froce get of header

        //        selectedHeader.HeaderType = allocHeaderProfile.HeaderType;
        //        // End Assortment
        //        aHeaderList.Add(selectedHeader);
        //    }
        //    LoadAssortmentMemberHeaders(aHeaderList);
        //}
        // end TT#488 - MD - Jellis - Group Allocation
		/// <summary>
		/// Creates Allocation Profiles and adds them to this Profile List.
		/// </summary>
		/// <param name="aHeaderList">List of Header RIDs</param>
		public void LoadAssortmentMemberHeaders(SelectedHeaderList aHeaderList)
		{
			AllocationProfileList apl = (AllocationProfileList)this.GetMasterProfileList(eProfileType.AssortmentMember);
			if (apl != null)
			{
				apl.Clear();
			}
			else
			{
				NewAssortmentMemberMasterProfileList();
				apl = (AllocationProfileList)this.GetMasterProfileList(eProfileType.AssortmentMember);
			}
			apl.LoadHeaders(this, aHeaderList, SAB.ApplicationServerSession);
            AssortmentProfile assortProf = null; // TT#1154 - MD - Jells -  Group Allocation Infinite  Loop When Going to SIze Analysis
			foreach (AllocationProfile ap in apl.ArrayList)
			{
                // Begin TT#4755 - JSmith - Cannot Remove header from Group
                // trigger AllocationProfile to load stores so packs can be resolved later
                bool allocationStarted = ap.AllocationStarted;
                // End TT#4755 - JSmith - Cannot Remove header from Group
				// Begin TT#1154-MD - stodd - error opening allocation selection screen - 
                this.AddAllocationProfile(ap);
                // was changed to false, but need this to know isStoreVisible
                // begin TT#1154 - MD - Jellis - Group Allocation Infinite  Loop When Going to SIze Analysis
                //ap.LoadStores(false); 
                // Begin TT#4988 - BVaughan - Performance
                #if DEBUG
                if ((ap is AssortmentProfile && !ap.isAssortmentProfile) || (!(ap is AssortmentProfile) && ap.isAssortmentProfile))
                {
                    throw new Exception("Object does not match AssortmentProfile in LoadAssortmentMemberHeaders()");
                }
                #endif
                //if (ap is AssortmentProfile)
                if (ap.isAssortmentProfile)
                // End TT#4988 - BVaughan - Performance
                {
                    assortProf = ap as AssortmentProfile;
                }
                // end  TT#1154 - MD - Jellis - Group Allocation Infinite  Loop When Going to SIze Analysis
				//this.AddAllocationProfile(ap);
				// End TT#1154-MD - stodd - error opening allocation selection screen - 
			}
            // begin TT#1154 - MD- Jellis-  Group Allocation Infinite  Loop When Going to SIze Analysis
            if (assortProf != null)
            {
                assortProf.LoadStores(false);
            }
            // end  TT#1154 - MD - Jellis - Group Allocation Infinite  Loop When Going to SIze Analysis
		}
		#endregion LoadHeaders
		// END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
		
		#region Linked Headers
		/// <summary>
		/// Loads all Allocation Profiles to the Profile List.
		/// </summary>
		public void AddLinkedHeader(AllocationProfile aAllocationProfile)
		{
			try
			{
				if (!IsLinkedHeader(aAllocationProfile))
				{
					LinkedHeaderList.Add(aAllocationProfile);
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a flag identifying if the header was processed as a linked header.
		/// </summary>
		public bool IsLinkedHeader(AllocationProfile aAllocationProfile)
		{
			try
			{
				if (LinkedHeaderList.Contains(aAllocationProfile.Key))
				{
					return true;
				}
				return false;
			}
			catch
			{
				throw;
			}
		}
		
		#endregion Linked Headers


		#region DeleteHeader
		public bool DeleteAllocationHeader(int aHeaderRID)
		{
			try
			{
				return this.GetAllocationProfile(aHeaderRID).DeleteHeader();
			}
			catch
			{
				return false;
			}
		}
		#endregion DeleteHeader

		// BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
		#region DeleteAssortmentMemberHeader
		public bool DeleteAssortmentMemberHeader(int aHeaderRID)
		{
			try
			{
				return this.GetAssortmentMemberProfile(aHeaderRID).DeleteHeader();
			}
			catch
			{
				return false;
			}
		}
		#endregion DeleteAssortmentMemberHeader
		// END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member

		#region PurgeHeader
		public bool PurgeAllocationHeader(int aHeaderRID, bool aForceDelete)
		{
			try
			{
				return this.GetAllocationProfile(aHeaderRID).Purge(aForceDelete);
			}
			catch
			{
				return false;
			}
		}
		#endregion PurgeHeader

		#region AssortmentViewSelectionCriteria

		// Begin TT#952 - MD - stodd - add matrix to Group Allocation Review
        public void CreateAssortmentViewSelectionCriteria()
        {
            CreateAssortmentViewSelectionCriteria(false);
        }
		

		/// <summary>
		/// Create a new AssortmentViewSelectionCriteria profile on the application server
		/// </summary>
		public void CreateAssortmentViewSelectionCriteria(bool forNewGroupAllocationOnly)
		{
			AllocationProfileList allocationList;
			// if no criteria, create one
			if (_assortmentCriteria == null)
			{
				_assortmentCriteria = new AssortmentViewSelectionCriteria(this);
			}

            if (forNewGroupAllocationOnly)
            {
                _assortmentCriteria.StoreGroupRID = Include.AllStoreGroupRID;
                _assortmentCriteria.GroupBy = (int)eAllocationAssortmentViewGroupBy.Attribute;
                _assortmentCriteria.ViewRid = Include.DefaultAssortmentViewRID;
                return;
            }
		// End TT#952 - MD - stodd - add matrix to Group Allocation Review
			// check for selected header list
			// BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
			//allocationList = (AllocationProfileList)GetMasterProfileList(eProfileType.Allocation);
			allocationList = (AllocationProfileList)GetMasterProfileList(eProfileType.AssortmentMember);
			// END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member

			// set header list in criteria or create new header list
			if (allocationList != null)
			{
				_assortmentCriteria.HeaderList = allocationList;
			}
			else if (_assortmentCriteria.HeaderList.Count == 0)
			{
                // begin TT#488 - MD - Jellis - Group Allocation
                _assortmentCriteria.HeaderList = NewAssortmentMemberMasterProfileList();
                //_assortmentCriteria.HeaderList = new AllocationProfileList(eProfileType.AssortmentMember);	// TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
                //SetMasterProfileList(_assortmentCriteria.HeaderList); 
                // end TT#488 - MD - Jellis - Group Allocation
			}
		}

        

		/// <summary>
		/// Builds the list of allocation headers from the selection criteria
		/// </summary>
		public void NewAssortmentCriteriaHeaderList()
		{
			AllocationHeaderProfileList ahpl = new AllocationHeaderProfileList(eProfileType.AllocationHeader);
			ahpl.LoadAll(_assortmentCriteria.HeaderList);
			SetMasterProfileList(ahpl);
		}

		public void SaveAssortmentDefaults()
		{
			_assortmentCriteria.SaveDefaults();
		}

		public int AssortmentUserRid
		{
			get { return _assortmentCriteria.UserRid; }
		}

		public int AssortmentStoreAttributeRid
		{
			get { return _assortmentCriteria.StoreAttributeRid; }
			set { _assortmentCriteria.StoreAttributeRid = value; }
		}

		public int AssortmentGroupBy
		{
			get { return _assortmentCriteria.GroupBy; }
			set { _assortmentCriteria.GroupBy = value; }
		}

		public int AssortmentViewRid
		{
			get { return _assortmentCriteria.ViewRid; }
			set { _assortmentCriteria.ViewRid = value; }
		}

		public eAssortmentVariableType AssortmentVariableType
		{
			get { return _assortmentCriteria.VariableType; }
			set { _assortmentCriteria.VariableType = value; }
		}

		public int AssortmentVariableNumber
		{
			get { return _assortmentCriteria.VariableNumber; }
			set { _assortmentCriteria.VariableNumber = value; }
		}

		public bool AssortmentIncludeCommitted
		{
			get { return _assortmentCriteria.IncludeCommitted; }
			set { _assortmentCriteria.IncludeCommitted = value; }
		}

		public bool AssortmentIncludeIntransit
		{
			get { return _assortmentCriteria.IncludeIntransit; }
			set { _assortmentCriteria.IncludeIntransit = value; }
		}

		public bool AssortmentIncludeOnhand
		{
			get { return _assortmentCriteria.IncludeOnhand; }
			set { _assortmentCriteria.IncludeOnhand = value; }
		}

		public bool AssortmentIncludeSimStore
		{
			get { return _assortmentCriteria.IncludeSimStore; }
			set { _assortmentCriteria.IncludeSimStore = value; }
		}

		public eStoreAverageBy AssortmentAverageBy
		{
			get { return _assortmentCriteria.AverageBy; }
			set { _assortmentCriteria.AverageBy = value; }
		}

		public eGradeBoundary AssortmentGradeBoundary
		{
			get { return _assortmentCriteria.GradeBoundary; }
			set { _assortmentCriteria.GradeBoundary = value; }
		}

		public DataTable AssortmentBasisDataTable
		{
			get { return _assortmentCriteria.BasisDataTable; }
			set { _assortmentCriteria.BasisDataTable = value; }
		}

		public DataTable AssortmentStoreGradeDataTable
		{
			get { return _assortmentCriteria.StoreGradeDataTable; }
			set { _assortmentCriteria.StoreGradeDataTable = value; }
		}

        // Begin TT#2 - RMatelic Assortment Planning
        public eAssortmentBasisLoadedFrom AssortmentViewLoadedFrom
        {
            get { return _assortmentViewLoadedFrom; }
            set { _assortmentViewLoadedFrom = value; }
        }    
        // End TT#2

		// Begin TT#1876 - STodd - summary not loading from selection screen
		public AssortmentViewSelectionCriteria AssortmentViewSelectionCriteria
		{
			get { return _assortmentCriteria; }
			set { _assortmentCriteria = value; }
		}
		// End TT#1876

		//Begin TT#1962 - DOConnell - When selecting a single assortment from the assrt workspace, review screen is using user selection basis.
        public bool AssortmentViewSelectionBypass
        {
            get { return _assmntViewSelBypass; }
            set { _assmntViewSelBypass = value; }
        }
		//Begin TT#1962 - DOConnell - When selecting a single assortment from the assrt workspace, review screen is using user selection basis.
		
		#endregion AssortmentViewSelectionCriteria


		#region AllocationViewSelectionCriteria
		/// <summary>
		/// Create a new AllocationViewSelectionCriteria profile on the application server
		/// </summary>
        public void CreateAllocationViewSelectionCriteria(bool useExistingAllocationProfileList = false) //TT#739-MD -jsobek -Delete Stores -Headers Loaded Multiple Times
		{
			AllocationProfileList allocationList;
            // Begin TT#1199-MD - stodd - GA-Velocity GA Allocate units in velocity go to Matrix Tab and the values are all 0.
            // Begin TT#1210-MD - stodd - When selecting all the member headers of a group in Workspace and going to Style Review, causes exception that the Assortment and member headers are out of sync
            if (VelocityCriteriaExists && VelocityIsInteractive)
            {
                allocationList = (AllocationProfileList)GetMasterProfileList(eProfileType.AssortmentMember);
            }
            else
            {
                allocationList = (AllocationProfileList)GetMasterProfileList(eProfileType.Allocation);  // TT#927 - MD - Jellis - Allocations disappear from Style Review when coming from Group Allocation
            }
            // Begin TT#1210-MD - stodd - When selecting all the member headers of a group in Workspace and going to Style Review, causes exception that the Assortment and member headers are out of sync
            // End TT#1199-MD - stodd - GA-Velocity GA Allocate units in velocity go to Matrix Tab and the values are all 0.

            // if no criteria, create one
			if (_allocationCriteria == null)
			{
                //_allocationCriteria = new AllocationViewSelectionCriteria(this, useExistingAllocationProfileList); //TT#739-MD -jsobek -Delete Stores -Headers Loaded Multiple Times  // TT#927 - MD - Jellis - Allocations disappear from Style Review when coming from Group Allocation
                _allocationCriteria = new AllocationViewSelectionCriteria(this, allocationList != null); //TT#739-MD -jsobek -Delete Stores -Headers Loaded Multiple Times  // TT#927 - MD - Jellis - Allocations disappear from Style Review when coming from Group Allocation
			}
			
			// check for selected header list 
            // Begin TT#927 - MD - Jellis - Allocations disappear from Style Review when coming from Group Allocation
            //allocationList = (AllocationProfileList)GetMasterProfileList(eProfileType.Allocation);
            // re-check list
            if (allocationList == null)
            {
                allocationList = (AllocationProfileList)GetMasterProfileList(eProfileType.Allocation); 
            }
            // end TT#927 - MD - Jellis - Allocations disappear from Style Review when coming from Group Allocation

			// set header list in criteria or create new header list
			if (allocationList != null)
			{
				_allocationCriteria.HeaderList = allocationList;
			}
			else if (_allocationCriteria.HeaderList.Count == 0)
			{
                // begin TT#488 - MD - Jellis - Group Allocation
                _allocationCriteria.HeaderList = NewAllocationMasterProfileList();
                //_allocationCriteria.HeaderList = new AllocationProfileList(eProfileType.Allocation);
                //SetMasterProfileList(_allocationCriteria.HeaderList); // MID Track 4210 Multi Intransit displayed incorrectly
                // end TT#488 - MD - Jellis - Group Allocation
			}
            // begin TT#1029 - MD - JEllis - Group Allocation Velocity - All units in VSW
            ClearVSWCache();
            // end TT#1029 - MD - Jellis - Group Allocation Velocity - All units in VSW
		}

		/// <summary>
		/// Updates the AllocationViewSelectionCriteria with the new header list
		/// </summary>
		public void UpdateAllocationViewSelectionHeaders()
		{
			// check for  header list
			AllocationProfileList allocationList = (AllocationProfileList)GetMasterProfileList(eProfileType.Allocation);

			// set header list in criteria or create new header list
			if (allocationList != null)
			{
				_allocationCriteria.HeaderList = allocationList;
			}
			else
			{
				_allocationCriteria.HeaderList = new AllocationProfileList(eProfileType.Allocation);
			}
		}

        // Begin TT#2045-MD - JSmith - In an Asst with a Detail PPK select Size Review receive a system argument out of range exception.  Expected mssg 80355 Size Review is not valid for the selected headers.
        public AllocationProfileList GetAllocationViewSelectionHeaders()
        {
            return _allocationCriteria.HeaderList;
        }
        // End TT#2045-MD - JSmith - In an Asst with a Detail PPK select Size Review receive a system argument out of range exception.  Expected mssg 80355 Size Review is not valid for the selected headers.

		/// <summary>
		/// Rebuilds the allocation profiles for the selected headers     
		/// </summary>
        // Begin TT#4811 - JSmith - Choose to not save changes in style review but changes save anyway
        //public void RefreshSelectedHeaders()
        public void RefreshSelectedHeaders(bool aRebuildWafers = true)
        // End TT#4811 - JSmith - Choose to not save changes in style review but changes save anyway
		{
			AllocationSubtotalProfileList subtotalList = (AllocationSubtotalProfileList)GetMasterProfileList(eProfileType.AllocationSubtotal);
            // Begin TT#1282 - RMatelic - Assortment - Added colors to a style, the style originally had quantities in it.  When I added the colors the style went to 0
            //foreach (AllocationSubtotalProfile asp in subtotalList)
            //{
            //    asp.RemoveAllSubtotalMembers();
            //}
            if (subtotalList != null)
            {
                foreach (AllocationSubtotalProfile asp in subtotalList)
                {
                    asp.RemoveAllSubtotalMembers();
                }
            }
            // End TT#1282
			//			subtotalList = null;

            RemoveAllocationSubtotalProfileList(); // TT#924-MD - JSmith - Tools Refresh trashes Style Review and other open Windows

            if (_assortmentProfile != null)
            {
                _assortmentProfile.ReReadHeader();
            }
            else
            {
                foreach (AllocationProfile ap in _allocationCriteria.HeaderList)
                {
                    ap.ReReadHeader();
                    ap.LoadStores();
                }
            }
			// (CSMITH) - BEG MID Track #2410: Allow interactive processing with multiple headers
			if (this.AllocationViewType == eAllocationSelectionViewType.Velocity)
			{
				this.Velocity.RefreshStoreData();
			}
			// (CSMITH) - END MID Track #2410
            // Begin TT#4811 - JSmith - Choose to not save changes in style review but changes save anyway
            //RebuildWafers();
            if (aRebuildWafers)
            {
                RebuildWafers();
            }
            // End TT#4811 - JSmith - Choose to not save changes in style review but changes save anyway
		}

        // Begin TT#2 Ron Matelic Assotemnt Planning
        public AllocationViewSelectionCriteria AllocationCriteria
        {
            get { return _allocationCriteria;}
        }
        // End TT#2

		/// <summary>
		/// Gets a flag identifying if the allocation criteria exists
		/// </summary>
		public bool AllocationCriteriaExists
		{
			get{ return (_allocationCriteria == null) ? false : true; }
		}

		
		/// <summary>
		/// Gets or sets the view type for the allocation criteria
		/// </summary>
		public eAllocationSelectionViewType AllocationViewType
		{
			get{ return _allocationCriteria.ViewType;}
			set
			{
				_allocationCriteria.ViewType = value;
			}
		}

		/// <summary>
		/// Gets NeedAnalysisFrom1stHeader flag (True:  Need analysis from 1st header; False: need analysis from select criteria).
		/// </summary>
		public bool NeedAnalysisFrom1stHeader
		{
			get
			{
				if (_determineShowNeedAnalysis)
				{
					bool temp = AllocationStyleViewIncludesNeedAnalysis;
				}
				return _needAnalysisFrom1stHeader;
			}
		}

		/// <summary>
		/// Gets a bool that indicates whether to include the Need Analysis grid on the Style View
		/// </summary>
		/// <remarks>
		/// True:  include Need Analysis Grid on Style View; False: do not include Need Analysis Grid
		/// </remarks>
		public bool AllocationStyleViewIncludesNeedAnalysis
		{
			get
			{
				if (_determineShowNeedAnalysis)
				{
                    _determineShowNeedAnalysis = false;
					_showNeedAnalysisCriteria = false;
					_needAnalysisFrom1stHeader = false;
					if (AllocationCriteriaExists)
					{
						if (this.AllocationNeedAnalysisPeriodBeginRID != Include.NoRID)
						{
							_showNeedAnalysisCriteria = true;
						}
						if (this.AllocationNeedAnalysisPeriodEndRID != Include.NoRID)
						{
							_showNeedAnalysisCriteria = true;
						}
						if (this.AllocationNeedAnalysisHNID != Include.NoRID)
						{
							_showNeedAnalysisCriteria = true;
						}
						if (_showNeedAnalysisCriteria)
						{
							return _showNeedAnalysisCriteria;
						}
					}
					_needAnalysisFrom1stHeader = true;
					// Set default values; in order to display need analysis all headers must have these same values
					AllocationProfileList apl = this.GetAllocationProfileList();
                    // begin TT#1154 - MD - Jellis - Group Allocation Style Review No Stores
                    if (apl == null
                        || apl.ArrayList.Count == 0)
                    {
                        _needAnalysisFrom1stHeader = false;
                        _showNeedAnalysisCriteria = false;
                        return _showNeedAnalysisCriteria;
                    }
                    // end TT#1154 - MD - Jellis - Group Allocation Style Review No Stores
                    DateTime needAnalysisPeriodBeginDate = ((AllocationProfile)apl.ArrayList[0]).BeginDay;
                    DateTime needAnalysisPeriodEndDate = ((AllocationProfile)apl.ArrayList[0]).ShipToDay;
                    double planFactor = ((AllocationProfile)apl.ArrayList[0]).PlanFactor;
                    int planHnRID = ((AllocationProfile)apl.ArrayList[0]).PlanHnRID;
                    int onHandHnRID = ((AllocationProfile)apl.ArrayList[0]).OnHandHnRID;
                    _showNeedAnalysisCriteria = true;
                    foreach (AllocationProfile ap in this.GetAllocationProfileList())
                    {
                        // Begin TT#1495-MD - RMatelic - ASST- Created a post asst- highlight 1st header open style review all the detail columns appear.  Close style review and reopen and all the detail columns are gone
                        if (ap.HeaderType == eHeaderType.Assortment && (ap.AsrtType == (int)eAssortmentType.PreReceipt || ap.AsrtType == (int)eAssortmentType.PostReceipt))
                        {
                            continue;
                        }
                        else
                        {
                        // End TT#1495-MD 
                            if (needAnalysisPeriodBeginDate != ap.BeginDay
                                || needAnalysisPeriodEndDate != ap.ShipToDay
                                || planFactor != ap.PlanFactor
                                || planHnRID != ap.PlanHnRID
                                || onHandHnRID != ap.OnHandHnRID)
                            {
                                _showNeedAnalysisCriteria = false;
                            }
                        }  // TT#1495-MD 
                    }
				}
				return _showNeedAnalysisCriteria;
			}
		}
		/// <summary>
		/// Gets or sets the store attribute ID for the allocation criteria
		/// </summary>					  
		public int AllocationStoreAttributeID
		{
			get{ return _allocationCriteria.StoreAttributeID;}
			set
			{
				_allocationCriteria.StoreAttributeID = value;		
			}
		}

		/// <summary>
		/// Gets or sets the store group level for the allocation criteria
		/// </summary>
		public int AllocationStoreGroupLevel
		{
			get{ return _allocationCriteria.StoreGroupLevel;}
			set
			{
				_allocationCriteria.StoreGroupLevel = value;		
			}
		}

		/// <summary>
		/// Gets or sets the filter table for the allocation criteria
		/// </summary>
		public DataTable AllocationFilterTable
		{
			get{ return _allocationCriteria.FilterTable; }
			set
			{
				_allocationCriteria.FilterTable = value; 		
			}
		}

		/// <summary>
		/// Gets or sets the filter ID for the allocation criteria
		/// </summary>
		public int AllocationFilterID
		{
			get
			{ 
				if (_allocationCriteria == null)
				{
					return Include.AllStoreFilterRID;
				}
				return _allocationCriteria.FilterID; 
			}
			set
			{
				_allocationCriteria.FilterID = value; 		
			}
		}

		//Begin TT#219 - MD - DOConnell - Spread Average not getting expected results
        /// <summary>
        /// Gets or sets the filter ID for the allocation criteria
        /// </summary>
		/// 
		// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
        //public List<int> AllocationSelHdrList
		public SelectedHeaderList AssortmentSelectedHdrList
        {
            get
            {
                return _assortmentSelectedHdrList;
            }
            set
            {
				_assortmentSelectedHdrList = value;
            }
        }
		// END TT#371-MD - stodd -  Velocity Interactive on Assortment
		//End TT#219 - MD - DOConnell - Spread Average not getting expected results

		// BEGIN TT#698-MD - Stodd - add ability for workflows to be run against assortments.
		public bool UseAssortmentSelectedHeaders
		{
			get
			{
				if (AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID == Include.NoRID)
				{
					_useAssortment = false;
				}
				else
				{
					_useAssortment = true;
				}
				
				return _useAssortment;
			}
			set
			{
				_useAssortment = value;
			}
		}
		// END TT#698-MD - Stodd - add ability for workflows to be run against assortments.

		// BEGIN TT#488-MD - Stodd - Group Allocation
		/// <summary>
		/// denotes where the action/method/workflow originated from.
		/// Used to help determine if Assortment or group allocation selected headers should used.
		/// </summary>
		public eActionOrigin ActionOrigin
		{
			get
			{
				return _ActionOrigin;
			}
			set
			{
				_ActionOrigin = value;
			}
		}
		// END TT#488-MD - Stodd - Group Allocation
		
		/// <summary>
		/// Gets or sets the group by for the allocation criteria
		/// </summary>
		public int AllocationGroupBy
		{
			get{ return _allocationCriteria.GroupBy; }
			set
			{
				_allocationCriteria.GroupBy = value; 		
			}
		}

		/// <summary>
		/// Gets or sets the view ID for the allocation criteria
		/// </summary>
        public int AllocationViewRID                         // Begin TT#456 - Add Views to Size Review : change ViewID to ViewRID 
		{
			get{ return _allocationCriteria.ViewRID;}
			set
			{
                _allocationCriteria.ViewRID = value;		 // End TT#456  
			}
		}
        
		/// <summary>
		/// Gets or sets the need analysis period begin record ID for the allocation criteria
		/// </summary>
		public int AllocationNeedAnalysisPeriodBeginRID
		{
			get
			{
				if (AllocationCriteriaExists)
				{
					return _allocationCriteria.NeedAnalysisPeriodBeginRID;
				}
				return Include.NoRID;
			}
			set
			{
				_allocationCriteria.NeedAnalysisPeriodBeginRID = value;
			}
		}


		/// <summary>
		/// Gets or sets the need analysis period end record ID for the allocation criteria
		/// </summary>
		public int AllocationNeedAnalysisPeriodEndRID
		{
			get
			{
				if (AllocationCriteriaExists)
				{
					return _allocationCriteria.NeedAnalysisPeriodEndRID; 
				}
				return Include.NoRID;
			}
			set
			{
				_allocationCriteria.NeedAnalysisPeriodEndRID = value;
			}
		}


		/// <summary>
		/// Gets or sets the need analysis hierarchy node record ID for the allocation criteria
		/// </summary>
		public int AllocationNeedAnalysisHNID
		{
			get
			{
				if (AllocationCriteriaExists)
				{
					return _allocationCriteria.NeedAnalysisHNID; 
				}
				return Include.NoRID;
			}
			set
			{
				_allocationCriteria.NeedAnalysisHNID = value;
                ResetAnalysisSettings();  // TT#1154 - MD - Jellis - Group ALlocation Infinite Loop
			}
		}
		
		/// <summary>
		/// Gets or sets the flag whether ineligible store are to be included in the allocation criteria
		/// </summary>
		public bool AllocationIncludeIneligibleStores
		{
			get
			{ 
				// BEGIN MID Track #2539 Grades not same
				//				if (AllocationCriteriaExists)
				if (AllocationCriteriaExists
					&& !VelocityCriteriaExists)
					// END MID Track #2539

				{
					return _allocationCriteria.IncludeIneligibleStores; 
				}
				return false;
			}
			set
			{
				_allocationCriteria.IncludeIneligibleStores = value;		
			}
		}

		/// <summary>
		/// Gets the count of the number of headers in the allocation criteria
		/// </summary>
		public int AllocationCriteriaHeaderCount
		{
			get
			{
				if (AllocationCriteriaExists)
				{
					return _allocationCriteria.HeaderList.Count; 
				}
				return 0;
			}
	
		}
		/// <summary>
		/// Gets the count of primary sizes
		/// </summary>
		public int AllocationCriteriaPrimarySizeCount
		{
			get
			{
				if (AllocationCriteriaExists)
				{
					return _allocationCriteria.PrimarySizesCount;
				}
				return 0;
			}
		}
		/// <summary>
		/// Gets the count of secondary sizes
		/// </summary>
		public int AllocationCriteriaSecondarySizeCount
		{
			get
			{
				if (AllocationCriteriaExists)
				{
					return _allocationCriteria.SecondarySizesCount;
				}
				return 0;
			}
		}
		/// <summary>
		/// Gets or sets the reference to the StyleView Window of
		///  allocation criteria
		/// </summary>
		public System.Windows.Forms.Form StyleView 
		{
            // begin TT#342 Velocity Null ref  on deactivate header
			//get{ return _allocationCriteria.StyleView;}
            get
            {
                if (AllocationCriteriaExists)
                {
                    return _allocationCriteria.StyleView;
                }
                return null;
            }
            // end TT#342 Velocity Null ref on deactivate header
			set
			{
				_allocationCriteria.StyleView = value;
			}
		}
		/// <summary>
		/// Gets or sets the reference to the SummaryView Window of
		///  allocation criteria
		/// </summary>
		public System.Windows.Forms.Form SummaryView 
		{
            // begin TT#342 Velocity Null ref on deactivate header
			//get{ return _allocationCriteria.SummaryView;}
            get
            {
                if (AllocationCriteriaExists)
                {
                    return _allocationCriteria.SummaryView;
                }
                return null;
            }
            // end TT#342 Velocity Null ref on deactivate header
			set
			{
				_allocationCriteria.SummaryView = value;
			}
		}
        // begin TT#702 Size Need Method "hangs" when process hdr with Begin Date
        SizeNeedMethod _processingSizeNeedMethod;
        /// <summary>
        /// Sets Processing Size Need Method
        /// </summary>
        public SizeNeedMethod ProcessingSizeNeedMethod
        {
            set { _processingSizeNeedMethod = value; }
        }
        // end TT#702 Size Need Method "hangs" when process hdr with Begin Date
		/// <summary>
		/// Gets or sets the reference to the SizeView Window of
		///  allocation criteria
		/// </summary>
		public System.Windows.Forms.Form SizeView 
		{
            // begin TT#342 Velocity Null ref on deactivate header
			//get{ return _allocationCriteria.SizeView;}
            get
            {
                if (AllocationCriteriaExists)
                {
                    return _allocationCriteria.SizeView;
                }
                return null;
            }
            // end TT#342 Velocity Null ref on deactivate header
			set
			{
				_allocationCriteria.SizeView = value;
			}
		}
		public System.Windows.Forms.Form AssortmentView
		{
            // begin TT#342 Velocity Null ref on deactivate header
			//get { return _allocationCriteria.AssortmentView; }
            get
            {
                if (AllocationCriteriaExists)
                {
                    return _allocationCriteria.AssortmentView;
                }
                return null;
            }
            // end TT#342 Velocity Null ref on deactivate header
			set
			{
				_allocationCriteria.AssortmentView = value;
			}
		}
        
        // begin TT#59 Implement Store Temp Locks
        public System.Windows.Forms.Form HeaderInformation
        {
            get { return _allocationCriteria.HeaderInformation; }
            set
            {
                _allocationCriteria.HeaderInformation = value;
            }
        }
        // end TT#59 Implement Store Temp Locks

		/// <summary>
		/// Gets or sets the reference to the VelocityMethod Window
		/// </summary>
		public System.Windows.Forms.Form VelocityWindow
		{
			get{ return _velocityWindow;}
			set
			{
				_velocityWindow= value;
			}
		}


		/// <summary>
		/// Gets or sets the reference to the AllocationWorkspaceExplorer 
		/// </summary>
		public System.Windows.Forms.UserControl AllocationWorkspaceExplorer 
		{
			get{ return _allocationWorkspaceExplorer;}
			set
			{
				_allocationWorkspaceExplorer = value;
			}
		}

        // Begin TT#1194-md - stodd - view ga header
        public bool IsGAMode
        {
            get
            {
                if (AllocationCriteriaExists)
                {
                    return _allocationCriteria.IsGAMode;
                }
                return false;
            }
        }
        // End TT#1194-md - stodd - view ga header

        // begin TT#1185 - Verify ENQ before Update
        ///// <summary>
        ///// Gets or sets the enque switch for the allocation criteria
        ///// </summary>
        //public bool HeadersEnqueued
        //{
            //get { return _allocationCriteria.HeadersEnqueued; }
            //set
            //{
            //    _allocationCriteria.HeadersEnqueued = value;
            //}
		//}
        // end TT#1185 - Verify ENQ before Update


		/// <summary>
		/// Gets or sets the data state for the allocation criteria
		/// </summary>
		public eDataState DataState
		{
			get
			{
				if (AllocationCriteriaExists)
				{
					return _allocationCriteria.DataState;
				}
				return eDataState.ReadOnly;
			}
			set
			{
				_allocationCriteria.DataState = value;
			}
		}

		/// <summary>
		/// Gets or sets the AnalysisOnly flag
		/// </summary>
		public bool AnalysisOnly
		{
			get
			{ 
				if (AllocationCriteriaExists)
				{
					return _allocationCriteria.AnalysisOnly; 
				}
				return false;
			}
			set
			{
				_allocationCriteria.AnalysisOnly = value;		
			}
		}
		// BEGIN MID Track #2959 - add Size Need Analysis
		/// <summary>
		/// Gets or sets the SizeCurveRID for the allocation criteria
		/// </summary>
		public int SizeCurveRID
		{
			get
			{
				// BEGIN MID Track 3179 error when allocating by size need to a future time frame.
				if (AllocationCriteriaExists)
				{
					return _allocationCriteria.SizeCurveRID;
				}
				return Include.NoRID;
				// END MID Track 3179 error when allocating by size need to a future time frame.
			}
			set
			{
				_allocationCriteria.SizeCurveRID = value;		
			}
		}
		
		/// <summary>
		/// Gets or sets the SizeConstraintRID for the allocation criteria
		/// </summary>
		public int SizeConstraintRID
		{
			get
			{ 
				// BEGIN MID Track 3179 error when allocating by size need to a future time frame.
				if (AllocationCriteriaExists)
				{
					return _allocationCriteria.SizeConstraintRID;
				}
				return Include.NoRID;
				// END MID Track 3179 error when allocating by size need to a future time frame.
			}
			set
			{
				_allocationCriteria.SizeConstraintRID = value;		
			}
		}
		
		/// <summary>
		/// Gets or sets the SizeAlternateRID for the allocation criteria
		/// </summary>
		public int SizeAlternateRID
		{
			get
			{
				// BEGIN MID Track 3179 error when allocating by size need to a future time frame.
				if (AllocationCriteriaExists)
				{
					return _allocationCriteria.SizeAlternateRID;
				}
				return Include.NoRID;
				// END MID Track 3179 error when allocating by size need to a future time frame.
			}
			set
			{
				_allocationCriteria.SizeAlternateRID = value;		
			}
		}
		// END MID Track #2959 
      
        // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
        public DataTable DTUserAllocBasis
        {
            get { return _allocationCriteria.DTUserAllocBasis; }
            set { _allocationCriteria.DTUserAllocBasis = value; }
        }

        public bool BasisCriteriaExists  
        {
            // Begin TT#4012 - JSmith - Null Reference Exception when selecting All Columns in Style Analysis
            //get { return (_allocationCriteria.DTUserAllocBasis.Rows.Count > 0) ? true: false; }
            get { return (_allocationCriteria.DTUserAllocBasis.Rows.Count > 0 && !AnalysisOnly) ? true : false; }
            // End TT#4012 - JSmith - Null Reference Exception when selecting All Columns in Style Analysis
        }
        // End TT#638 

        //		/// <summary>
		//		/// Gets the wafers in the allocation criteria
		//		/// </summary>
		//		public AllocationWaferGroup AllocationWafers
		//		{
		//			get{ return _allocationCriteria.Wafers; }
		//		}

        // Begin TT#2067-MD - JSmith/AGallagher - PPK & Bulk Header-Process Minimum rule-select Vel Str Detail- receive mssg Pack is not defined on subtotal
        public bool AllocationWafersNeedRebuilt
        {
            get { return _allocationWafersNeedRebuilt; }
            set { _allocationWafersNeedRebuilt = value; }
        }
        // End TT#2067-MD - JSmith/AGallagher - PPK & Bulk Header-Process Minimum rule-select Vel Str Detail- receive mssg Pack is not defined on subtotal

		/// <summary>
		/// Gets the wafers in the allocation criteria
		/// </summary>
		public AllocationWaferGroup AllocationWafers
		{
			get
			{
                // Begin TT#2067-MD - JSmith/AGallagher - PPK & Bulk Header-Process Minimum rule-select Vel Str Detail- receive mssg Pack is not defined on subtotal
                if (AllocationWafersNeedRebuilt)
                {
                    ResetFirstBuild(true);
                    RebuildWafers();
                    AllocationWafersNeedRebuilt = false;
                }
                // End TT#2067-MD - JSmith/AGallagher - PPK & Bulk Header-Process Minimum rule-select Vel Str Detail- receive mssg Pack is not defined on subtotal
				AllocationWaferGroup awg = null;
				AllocationWafer aw = null;
				if (_allocationCriteria.Wafers != null)
				{
					awg = new AllocationWaferGroup(_allocationCriteria.Wafers.RowCount, _allocationCriteria.Wafers.ColumnCount);
					for (int row = 0; row < _allocationCriteria.Wafers.RowCount; row++)
					{
						for (int column = 0; column < _allocationCriteria.Wafers.ColumnCount; column++)
						{
							AllocationWaferBuilder awb = _allocationCriteria.Wafers[row, column];
							awb.AllocationWaferColumn = column;
							aw = new AllocationWafer();
							aw.Cells = awb.Cells;
							aw.ColumnLabels = awb.ColumnLabels;
							aw.RowLabels = awb.RowLabels;
							aw.Columns = awb.Columns;
							aw.Rows = awb.Rows;
							awg.Wafers[row, column] = aw;
						}
					}
				}
				return awg;
			}
		}

		/// <summary>
		/// Rebuilds the wafers in the allocation criteria
		/// </summary>
		public void RebuildWafers()
		{
			_allocationCriteria.RebuildWafers();
            AllocationWafersNeedRebuilt = false;  // TT#2067-MD - JSmith/AGallagher - PPK & Bulk Header-Process Minimum rule-select Vel Str Detail- receive mssg Pack is not defined on subtotal
		}

        // Begin TT#1282 - RMatelic - Assortment - Added colors to a style, the style originally had quantities in it.  When I added the colors the style went to 0
        public void ResetFirstBuild(bool bFirstBuild)
        {
            _allocationCriteria.FirstBuild = bFirstBuild;
        }
        // End TT#1272

		// BEGIN TT#543-MD - stodd - Style View/Size View assortment display
		public void ResetFirstBuildSize(bool bFirstBuild)
		{
			_allocationCriteria.FirstBuildSize = bFirstBuild;
		}
		// END TT#543-MD - stodd - Style View/Size View assortment display

        // Begin TT#607-MD - RMatelic - Size Review not displaying new sizes added when header is dropped on an assortment placeholder
        public void ResetSizeViewGroups()
        {
            _allocationCriteria.ResetSizeViewGroups();
        }
        // End TT#607-MD  
        // begin TT#1154 - MD - Jellis - Group ALlocation Infinite Loop
        /// <summary>
        /// Resets the Need Analysis Indicators
        /// </summary>
        internal void ResetAnalysisSettings()
        {
            _determineShowNeedAnalysis = true;
            _needAnalysisFrom1stHeader = false;
            _showNeedAnalysisCriteria = false;
        }
        // end TT#1154 - MD - Jellis - Group Allocatio Infinite Loop
        /// <summary>
		/// Updates the Allocation profile with the adjusted value
		/// </summary>
		/// <param name="aWaferRow">The row index of the wafer that was changed</param>
		/// <param name="aWaferCol">The column index of the wafer that was changed</param>
		/// <param name="aGridRow">The row index of the grid that was changed</param>
		/// <param name="aGridCol">The column index of the grid that was changed</param>
		/// <param name="aValue">The changed value</param>
		public void SetAllocationCellValue(int aWaferRow, int aWaferCol, int aGridRow, int aGridCol, double aValue)
		{
            // begin TT#59 Implement Temp Locks
            _allocationWaferChangeRequest.Add (new AllocationWaferCellChange (aWaferRow, aWaferCol, aGridRow, aGridCol, aValue, _allocationWaferChangeRequest.Count));
        }
        public void SetAllocationCellValue(AllocationWaferCellChangeList aAllocationWaferCellChangeList)
        {
			//try
			//{
			//	AllocationWaferBuilder awb = _allocationCriteria.Wafers[aWaferRow, aWaferCol];
			//	awb.SetCellValue(aGridRow, aGridCol, aValue);
			//}
            // begin TT#225 Balance Size Proportional Gets Error
            AllocationProfileList apl = this.GetAllocationProfileList();
            foreach (AllocationProfile ap in apl)
            {
                ap.ResetTempLocks(true);
            }
            // end TT#225 Balance Size Proportional Gets Error
            try
            {
                int waferRow = 0;
                int waferCol = 0;
                ClearHeldAllocations(); // TT#59 - Temp Locks - lock fails when error
                AllocationWaferBuilder awb = _allocationCriteria.Wafers[waferRow, waferCol];
                List<AllocationWaferCellChange> gridCellChangeList = new List<AllocationWaferCellChange>();
                foreach (AllocationWaferCellChange awcc in aAllocationWaferCellChangeList)
                {
                    if (waferRow != awcc.WaferRow
                        || waferCol != awcc.WaferCol)
                    {
                        if (gridCellChangeList.Count > 0)
                        {
                            awb.SetCellValue(gridCellChangeList);
                            gridCellChangeList.Clear();
                        }
                        waferRow = awcc.WaferRow;
                        waferCol = awcc.WaferCol;
                        awb = _allocationCriteria.Wafers[waferRow, waferCol];
                    }
                    gridCellChangeList.Add(awcc);
                }
                if (gridCellChangeList.Count > 0)
                {
                    awb.SetCellValue(gridCellChangeList);
                    gridCellChangeList.Clear();
                }
                // begin TT#225 Balance Size Proportional Gets Error
                //AllocationProfileList apl = this.GetAllocationProfileList();
                //foreach (AllocationProfile ap in apl)
                //{
                //    ap.ResetTempLocks();
                //}
                // end TT#225 Balance Size Proportional Gets Error
            }
            // end TT#59 Implement Temp Locks

            catch (Exception)   // TT#59 - Temp Lock
            {
                // begin TT#59 - Temp Lock - lock fails when error
                //string message = e.ToString();
                RecoverHeaderAllocation();
                // end TT#59 - Temp Lock - lock fails when error

                throw;
            }
            finally
            {
                // begin TT#225 Balance Size Proportional Gets Error
                // begin TT#59 - Temp Lock - lock fails when error
                ClearHeldAllocations();
                // end TT#59 - Temp Lock - lock fails when error
                foreach (AllocationProfile ap in apl)
                {
                    ap.ResetTempLocks(true);
                }
                // end TT#225 Balance Size Proportional Gets Error
            }
		}

		/// <summary>
		/// Save the allocation defaults to the database
		/// </summary>
		public void SaveAllocationDefaults()
		{
			_allocationCriteria.SaveDefaults();
			// force need criteria settings
			//			bool forceNeedCriteria = this.AllocationStyleViewIncludesNeedAnalysis;
		}

		/// <summary>
		/// Save the allocation header information
		/// </summary>
		public bool SaveHeaders()  // TT#1185 - Verify ENQ before Update
		{
			try
			{
				AllocationProfileList allocationList = (AllocationProfileList)GetMasterProfileList(eProfileType.Allocation);
                // begin TT#1051 - MD - JEllis - GA pack Allocations zeroed out
                Dictionary<int, AssortmentProfile> assortList = new Dictionary<int, AssortmentProfile>();
                List<AllocationProfile> apList = new List<AllocationProfile>();
                AssortmentProfile assortmentProfile;
                foreach (AllocationProfile ap in allocationList)
                {
                    // Begin TT#4988 - BVaughan - Performance
                    #if DEBUG
                    if ((ap is AssortmentProfile && !ap.isAssortmentProfile) || (!(ap is AssortmentProfile) && ap.isAssortmentProfile))
                    {
                        throw new Exception("Object does not match AssortmentProfile in SaveHeaders()");
                    }
                    #endif
                    //if (ap is AssortmentProfile)
                    if (ap.isAssortmentProfile)
                    // End TT#4988 - BVaughan - Performance
                    {
                        if (!assortList.TryGetValue(ap.Key, out assortmentProfile))
                        {
                            assortList.Add(ap.Key, ap as AssortmentProfile);
                        }
                    }
                    else if (ap.AsrtRID != Include.NoRID)
                    {
                        if (!assortList.TryGetValue(ap.AsrtRID, out assortmentProfile))
                        {
                            assortList.Add(ap.AsrtRID, ap.AssortmentProfile);
                        }
                    }
                    else
                    {
                        apList.Add(ap);
                    }
                }
                foreach (AssortmentProfile asrt in assortList.Values)
                {
                    if (!asrt.WriteHeader())
                    {
                        return false;
                    }
                }
                //foreach(AllocationProfile ap in allocationList) 		
                foreach (AllocationProfile ap in apList) 
                    // end TT#1051 - MD - JEllis - GA pack Allocations zeroed out
				{				
					if (!ap.WriteHeader())  // TT#1185 - Verify ENQ before Update
                    {                      // TT#1185 - Verify ENQ before Update
                        return false;             // TT#1185 - Verify ENQ before Update
                    }                       // TT#1185 - Verify ENQ before Update
				}
                return true;               // TT#1185 - Verify ENQ before Update
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Builds the list of allocation headers from the selection criteria
		/// </summary>
		public void NewCriteriaHeaderList()
		{
			AllocationHeaderProfileList ahpl = new AllocationHeaderProfileList(eProfileType.AllocationHeader);
			ahpl.LoadAll(_allocationCriteria.HeaderList);
			SetMasterProfileList(ahpl);
		}

		//		/// <summary>
		//		/// Gets the list of allocation headers from the selection criteria
		//		/// </summary>
		//		public AllocationHeaderProfileList GetCriteriaHeaderList()
		//		{
		//			AllocationHeaderProfileList ahpl = new AllocationHeaderProfileList(eProfileType.AllocationHeader);
		//			ahpl.LoadAll(_allocationCriteria.HeaderList);
		//			SetMasterProfileList(ahpl);
		//			return ahpl;
		//		}

		/// <summary>
		/// Sets the list of allocation headers for the selection criteria
		/// </summary>
		public void SetCriteriaHeaderList(AllocationHeaderProfileList ahpl)
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            //int[] selectedHeaderArray = new int[ahpl.Count]; 
            //int selectedIndex = 0;
            //foreach (AllocationHeaderProfile ahp in ahpl)
            //{
            //    selectedHeaderArray[selectedIndex] = ahp.Key;
            //    ++selectedIndex;
            //}
            //LoadHeaders(selectedHeaderArray);
            CreateMasterAllocationProfileListFromSelectedHeaders(ahpl);
            // end TT#488 - MD - Jellis - Group Allocation
			AllocationProfileList allocationList = (AllocationProfileList)GetMasterProfileList(eProfileType.Allocation);
			_allocationCriteria.HeaderList = allocationList;
		}

        // begin TT#1185 - Verify ENQ before Update
        private HeaderEnqueue _headerEnqueue;
        /// <summary>
        /// Gets a list of all header RIDs that must be enqueued simultaneously with the given header list
        /// </summary>
        /// <param name="aHdrRidList">Headers that are to be enqueued</param>
        /// <returns>All header RIDs that must be enqueued simultaneously with the given headers.</returns>
        public List<int> GetHeadersToEnqueue(List<int> aHdrRidList)
        {
            if (_headerEnqueue == null)
            {
                _headerEnqueue = SAB.ApplicationServerSession.GetHeaderEnqueueObject(_clientThreadID, TransactionID, UserRID); // TT#1185 - JEllis - Verify ENQ before Update (part 2)
                //_headerEnqueue = new HeaderEnqueue(_clientThreadID, TransactionID, UserRID);  // TT#1185 - JEllis - Verify ENQ before Update (part 2)
            }
            return _headerEnqueue.GetHdrsToEnq(aHdrRidList);
        }
        /// <summary>
        /// Enqueue all selected headers and their related headers (such as child headers for multi's)
        /// </summary>
        /// <returns></returns>
        public bool EnqueueSelectedHeaders(out string aHdrConflictMsg)
        {
            List<int> selectedHdrs = new List<int>();
            foreach (SelectedHeaderProfile shp in GetProfileList(eProfileType.SelectedHeader))
            {
                selectedHdrs.Add(shp.Key);
            }
            return EnqueueHeaders(GetHeadersToEnqueue(selectedHdrs), out aHdrConflictMsg);
        }

        /// <summary>
        /// Enqueues the specified headers for update
        /// </summary>
        /// <param name="aAllocationHeaderProfileList"></param>
        /// <param name="aHdrConflictMsg"></param>
        /// <returns></returns>
        public bool EnqueueHeaders(AllocationHeaderProfileList aAllocationHeaderProfileList, out string aHdrConflictMsg)
        {
            List<int> hdrRidList = new List<int>();
            foreach (AllocationHeaderProfile ahp in aAllocationHeaderProfileList)
            {
                hdrRidList.Add(ahp.Key);
            }
            return EnqueueHeaders(hdrRidList, out aHdrConflictMsg);
        }
        private string newLine = System.Environment.NewLine;
        public bool EnqueueHeaders(List<int> aHdrRidList, out string aHdrConflictMsg)
        {
            if (_headerEnqueue == null)
            {
                _headerEnqueue = SAB.ApplicationServerSession.GetHeaderEnqueueObject(_clientThreadID, TransactionID, UserRID);
            }
            if (aHdrRidList.Count == 0)
            {
                aHdrConflictMsg = "There are no headers to enqueue.";
                return false;
            }
            if (_headerEnqueue.EnqueueHeaders(aHdrRidList))
            {
                aHdrConflictMsg = "Header Enqueue Successful";
                return true;
            }
            aHdrConflictMsg = _headerEnqueue.FormatHeaderConflictMsg();
            return false;
        }
        public bool AreHeadersInConflict
        {
           get
           {
               if (_headerEnqueue == null)
               {
                   return false;
               }
               return _headerEnqueue.isAnyHeaderInConflict;
           }
        }
        /// <summary>
        /// Determines if specified header is enqueued by this transaction
        /// </summary>
        /// <param name="aHdrRID">Header RID to check</param>
        /// <returns>True: Header is enqueued by this transaction; False: header is not enqueued by this transaction</returns>
        public bool IsHeaderEnqueued(int aHdrRID)
        {
            List<int> hdrRidList = new List<int>();
            hdrRidList.Add(aHdrRID);   // TT#1966-MD - JSmith - DC Fulfillment
            return AreHeadersEnqueued(hdrRidList);
        }
        /// <summary>
        /// Determines if specified headers are enqueued by this transation
        /// </summary>
        /// <param name="aHeaderProfileList">Allocation Header Profile List</param>
        /// <returns>True: Specified headers were enqueued by this transaction; False: At Least 1 of the specified headers was not enqueued by this transaction</returns>
        public bool AreHeadersEnqueued(AllocationHeaderProfileList aHeaderProfileList)
        {
            List<int> hdrRidList = new List<int>();
            foreach (AllocationHeaderProfile ahp in aHeaderProfileList)
            {
                hdrRidList.Add(ahp.Key);
            }
            return (AreHeadersEnqueued(hdrRidList));
        }
        /// <summary>
        /// Determines if specified headers are enqueued by this transaction
        /// </summary>
        /// <param name="aHdrRIDList">List of header RIDs</param>
        /// <returns>True: Specified headers were enqueued by this transaction; False: At Least 1 of the specified headers was not enqueued by this transaction</returns>
        public bool AreHeadersEnqueued(List<int> aHdrRIDList)
        {
            if (_headerEnqueue == null)
            {
                return false;
            }
            foreach (int hdrRID in aHdrRIDList)
            {
                if (!_headerEnqueue.IsHeaderEnqueued(hdrRID))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Gets the list of headers that were in conflict on the last enqueue attempt
        /// </summary>
        /// <returns>HeaderConflict array</returns>
        public HeaderConflict[] HeaderConflicts()
        {
            if (_headerEnqueue != null)
            {
                return _headerEnqueue.HeaderConflictList;
            }
            return null;
        }
        /// <summary>
        /// Dequeues all the headers that were enqueued by this transaction
        /// </summary>
        public void DequeueHeaders()
        {
            if (_headerEnqueue != null)
            {
                _headerEnqueue.DequeueHeaders();
            }
        }
        /// <summary>
        /// Dequeues the specified headers (provided they were enqueued by this transaction)
        /// </summary>
        /// <param name="aHdrRidList">Header RID list</param>
        public void DequeueHeaders(List<int> aHdrRidList)
        {
            if (_headerEnqueue != null)
            {
                _headerEnqueue.DequeueHeaders(aHdrRidList);
            }
        }
        //public void EnqueueHeaders()
        //{
        //    try
        //    {
        //        _allocationCriteria.EnqueueHeaders(); 
        //    }
        //    catch (CancelProcessException)
        //    {
        //        throw new CancelProcessException();
        //    }
        //    catch ( Exception err )
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}
        ///// <summary>
        ///// Enqueues headers
        ///// </summary>
        ///// <param name="aAllocationHeaderList">ProfileList of AllocationHeaderProfiles to enqueue</param>
        ///// <returns>True if all headers enqueued; false otherwise.</returns>
        //public bool EnqueueHeaders(AllocationHeaderProfileList aAllocationHeaderList) 
        //{
        //    StringBuilder errMsg;
        //    SecurityAdmin secAdmin;
        //    try 
        //    {
        //        _headerEnq = new HeaderEnqueue(this, aAllocationHeaderList);
        //        _headerEnq.EnqueueHeaders();
        //        _headerEnqMessage = "Selected headers enqueued successfully";
        //        //				_headerEnqMsg = MIDText.GetText(eMIDTextCode.msg_al_HeaderEnqueueSuccess);
        //        return true;
        //    }
        //    catch (HeaderConflictException) 
        //    {
        //        AllocationHeaderProfile ahp;
        //        secAdmin = new SecurityAdmin();
        //        // Begin TT#1163 - JSmith - Headers are not locked when processing Methods or Workflows
        //        //errMsg = new StringBuilder("Enqueue failed for following headers: ");
        //        errMsg = new StringBuilder(MIDText.GetText(eMIDTextCode.msg_al_HeadersInUse) + ":");
        //        // End TT#1163
        //        //				errMsg = new StringBuilder(MIDText.GetText(eMIDTextCode.msg_al_HeaderEnqueueFailed));
        //        errMsg.Append(System.Environment.NewLine);
        //        foreach (HeaderConflict hdrCon in _headerEnq.HeaderConflictList) 
        //        {
        //            ahp = (AllocationHeaderProfile)aAllocationHeaderList.FindKey(System.Convert.ToInt32(hdrCon.HeaderRID, CultureInfo.CurrentUICulture));
        //            errMsg.Append(System.Environment.NewLine);
        //            errMsg.Append(ahp.HeaderID); 
        //            errMsg.Append(", User: ");
        //            errMsg.Append(secAdmin.GetUserName(hdrCon.UserRID));
        //        }
        //        errMsg.Append(System.Environment.NewLine + System.Environment.NewLine);
        //        _headerEnqMessage = errMsg.ToString();
        //        this.SAB.ApplicationServerSession.Audit.Add_Msg(
        //            eMIDMessageLevel.Severe,
        //            //					eMIDTextCode.msg_al_SelectedHeadersInUse,
        //            _headerEnqMessage,
        //            this.GetType().Name);
        //        return false;
        //    }
        //}

        //// Begin TT#1163 - JSmith - Headers are not locked when processing Methods or Workflows
        ///// <summary>
        ///// Enqueues headers
        ///// </summary>
        ///// <returns>True if all headers enqueued; false otherwise.</returns>
        //public bool EnqueueSelectedHeaders()
        //{
        //    AllocationHeaderProfile ahp;
        //    AllocationHeaderProfileList headerList = new AllocationHeaderProfileList(eProfileType.AllocationHeader);
        //    foreach (SelectedHeaderProfile shp in GetProfileList(eProfileType.SelectedHeader))
        //    {
        //        ahp = new AllocationHeaderProfile(shp.HeaderID, shp.Key);
        //        headerList.Add(ahp);
        //    }
        //    return EnqueueHeaders(headerList);
        //}
        //// End TT#1163

        //public void DequeueHeaders() 
        //{
        //    try 
        //    {
        //        if (_headerEnq != null)
        //        {
        //            _headerEnq.DequeueHeaders();
        //            _headerEnq = null;
        //            _headerEnqMessage = "Dequeue Successful";
        //            //					_headerEnqMessage = MIDText.GetText(eMIDTextCode.msg_al_DequeueSuccessful);
        //        }
        //    }
        //    catch (Exception exc) 
        //    {
        //        string message = exc.ToString();
        //        throw;
        //    }
        //}
        // end TT#1185 - Verify ENQ before Update 

		public AllocationSelection GetAllocationSelection()
		{
			return _allocationCriteria.AllocSelection;
		}
		public void CheckForHeaderDequeue()
		{
            // begin TT#1185 - Verify ENQ before Update
            if (_allocationCriteria != null)
            {
                _allocationCriteria.CheckForHeaderDequeue();
            }
            //if (this.HeadersEnqueued)
            //    _allocationCriteria.CheckForHeaderDequeue();
            // end TT#1185 - Verify ENQ before Update
		}
		#endregion AllocationViewSelectionCriteria

		#region InteractiveVelocity
		/// <summary>
		/// Special method for creating velocity method
		/// </summary>
		public void CreateVelocityMethod(SessionAddressBlock aSAB, int aMethodRID)
		{
			try
			{
				_velocityCriteria = new VelocityMethod(aSAB,aMethodRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
        // Begin TT#983 - RMatelic - Velocity Detail Screen when selecting All Stores or Set user is prompted that the matrix will be reprocessed, but the grid is not cleared entirely and when Apply Changes is selected it seems that nothing is reprocessing.
        public void ResetVelocityMethod()
        {
            try
            {
                _velocityCriteria = null;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#983

		/// <summary>
		/// Special method for loading velocity data arrays
		/// </summary>
		public void VelocityLoadDataArrays()
		{
			try
			{
				_velocityCriteria.LoadDataArrays();
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Special method for processing interactive velocity matrix 
		/// </summary>
		//public void ProcessInteractiveVelocity(bool balance) // TT#406 Unhandled exception when checking / unchecking balance
        public void ProcessInteractiveVelocity()               // TT#406 Unhandled exception when checking / unchecking balance
		{
			try
			{
				int storeFilterRID = Include.NoRID;
				Profile methodProfile = null;

				// BEGIN TT#698-MD - Stodd - add ability for workflows to be run against assortments.
				//bool isProcessingInAssortment = false;
                //int assrtRid = Include.NoRID;  // TT#488 - MD - Jellis - Group Allocation (field not used
				//BEGIN TT#808-MD-DOConnell-Placeholder with existing style-color not showing header on hand and intransit in the velocity detail screen.
                //if (UseAssortmentSelectedHeaders)
                //{
                //    // BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
                //    //SAB.ProcessMethodOnAssortmentEvent.ProcessMethod(this, assrtRid, eMethodType.Velocity, this.Velocity.Key);
                    //AllocationProfileList headerList = (AllocationProfileList)this.GetMasterProfileList(eProfileType.Allocation);
                    //AllocationProfileList headerList2 = (AllocationProfileList)this.GetMasterProfileList(eProfileType.AssortmentMember);
                    //AllocationSubtotalProfile grandTotal = this.GetAllocationGrandTotalProfile();
                //    this.Velocity.ProcessMethod(this, storeFilterRID, methodProfile);
                //    // END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
                //}
                //else
                //{
				// END TT#698-MD - Stodd - add ability for workflows to be run against assortments.
                    // begin TT#488 - MD - Jellis - Group Allocation
					
					// Begin TT#1019 - md - For headers within a Group Allocation, Prohibit allocation-type actions in the review screens and allocation workspace - 
					// moved up sooner in the processing.
                    //AllocationProfileList apl = CreateMasterAllocationProfileListFromSelectedHeaders();
                    this.Velocity.ProcessMethod(this, storeFilterRID, methodProfile);
					// End TT#1019 - md - For headers within a Group Allocation, Prohibit allocation-type actions in the review screens and allocation workspace - 
                
                    //AllocationProfileList apl = new AllocationProfileList(eProfileType.Allocation);
                    //SelectedHeaderList selectedHeaderList = (SelectedHeaderList)GetProfileList(eProfileType.SelectedHeader);
                    //if (selectedHeaderList.Count > 0)
                    //{
                    //    apl.LoadHeaders(this, selectedHeaderList, SAB.ApplicationServerSession);
                    //    SetMasterProfileList(apl);
                    //}

					// TT#406 Unhandled exception when checking / unchecking balance
					////tt#152 - velocity balance
					//this.Velocity.Balance = balance;
					////tt#152 - velocity balance - apicchetti
					// TT#406 Unhandled exception when checking /unchecking balance

                //    this.Velocity.ProcessMethod(this, storeFilterRID, methodProfile);
                //}		// TT#698-MD - Stodd - add ability for workflows to be run against assortments.
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		
		/// <summary>
		/// Gets or sets the velocity method
		/// </summary>
		public VelocityMethod Velocity
		{
			get{ return _velocityCriteria; }
			set
			{
                _velocityCriteria = value;	
                // begin TT#3770 - MD - Jellis - Group Allocation: Navigate from Velocity to Style Review results in null reference
                // begin TT#3796 - MD - Jellis - Velocity Null Reference
                //_allocationCriteria.ViewType = eAllocationSelectionViewType.Style;
                if (_allocationCriteria != null)
                {
                    _allocationCriteria.ViewType = eAllocationSelectionViewType.Style;
                }
                // end TT#3796 - MD - Jellis - Velocity Null Reference
                // end TT#3770 - MD - Jellis - Group Allocation: Navigate from Velocity to Style Review results in null reference
			}
		}

		/// <summary>
		/// Gets a flag identifying if the velocity criteria exists
		/// </summary>
		public bool VelocityCriteriaExists
		{
			get{ return (_velocityCriteria == null) ? false : true; }
		}
	
		/// <summary>
		/// Gets a matrix grade total basis sales
		/// </summary>
		public int VelocityGetMatrixGradeTotBasisSales(int aGrpLvlRID, string aGrade)
		{
			return _velocityCriteria.GetMatrixGradeTotBasisSales (aGrpLvlRID,aGrade);
		}

		/// <summary>
		/// Gets a matrix grade average basis sales
		/// </summary>
		public double VelocityGetMatrixGradeAvgBasisSales(int aGrpLvlRID, string aGrade)
		{
			return _velocityCriteria.GetMatrixGradeAvgBasisSales (aGrpLvlRID,aGrade);
		}

		/// <summary>
		/// Gets a matrix grade average basis sales percent to total
		/// </summary>
		public double VelocityGetMatrixGradeAvgBasisSalesPctTot(int aGrpLvlRID, string aGrade)
		{
			return _velocityCriteria.GetMatrixGradeAvgBasisSalesPctTot(aGrpLvlRID,aGrade);
		}

		/// <summary>
		/// Gets a matrix grade average basis sales index
		/// </summary>
		public double VelocityGetMatrixGradeAvgBasisSalesIdx(int aGrpLvlRID, string aGrade)
		{
			return _velocityCriteria.GetMatrixGradeAvgBasisSalesIdx(aGrpLvlRID,aGrade);
		}

        //BEGIN TT#153 – add variables to velocity matrix - apicchetti
        /// <summary>
        /// Gets a matrix grade total number of stores
        /// </summary>
        public double VelocityGetMatrixGradeTotalNumberOfStores(int aGrpLvlRID, string aGrade)
        {
            return _velocityCriteria.GetMatrixGradeTotalNumberOfStores(aGrpLvlRID, aGrade);
        }
        
        // begin TT#586 Velocity Variables not calculated correctly
        /// <summary>
        /// Gets the total number of stores within the percent sell through for the given Group Level RID
        /// </summary>
        /// <param name="aGrpLvlRID">Group Level RID</param>
        /// <param name="aPctSellThruIdx">Percent Sell Thru Index</param>
        /// <returns></returns>
        public int VelocityGetSellThruTotalStores(int aGrpLvlRID, int aPctSellThruIdx)
        {
            return _velocityCriteria.GetSellThruTotalStores(aGrpLvlRID, aPctSellThruIdx);
        }
        /// <summary>
        /// Gets the average weeks of supply for a percent sell thru across all grades within a given Group Level (attribute set)
        /// </summary>
        /// <param name="aGrpLvlRID">Group Level RID</param>
        /// <param name="aPctSellThruIdx">Perchen Sell Thru Index</param>
        /// <returns></returns>
        public double VelocityGetSellThruAvgWOS(int aGrpLvlRID, int aPctSellThruIdx)
        {
            return _velocityCriteria.GetSellThruAvgWOS(aGrpLvlRID, aPctSellThruIdx);
        }
        // end TT#586 Velocity Variables not calculated correctly

        /// <summary>
        /// Gets a matrix grade average stock
        /// </summary>
        public double VelocityGetMatrixGradeAvgStock(int aGrpLvlRID, string aGrade)
        {
            return _velocityCriteria.GetMatrixGradeAvgStock(aGrpLvlRID, aGrade);
        }

        /// <summary>
        /// Gets a matrix stock percentage of total
        /// </summary>
        public double VelocityGetMatrixGradeStockPercentOfTotal(int aGrpLvlRID, string aGrade)
        {
            return _velocityCriteria.GetMatrixGradeStockPercentageOfTotal(aGrpLvlRID, aGrade);
        }

        /// <summary>
        /// Gets a matrix stock percentage of total
        /// </summary>
        public double VelocityGetMatrixGradeAllocationPercentOfTotal(int aGrpLvlRID, string aGrade)
        {
            return _velocityCriteria.GetMatrixGradeAllocationPercentageOfTotal(aGrpLvlRID, aGrade);
        }
        //END TT#153 – add variables to velocity matrix - apicchetti


		/// <summary>
		/// Gets the velocity cell stores
		/// </summary>
        public int VelocityGetMatrixCellStores(int aSetValue, int aBoundary, int aPST) 
		{
            return _velocityCriteria.GetMatrixCellStores(aSetValue, aBoundary, aPST);  
        }

		/// <summary>
		/// Gets the velocity cell Avg wos
		/// </summary>
        // begin TT#586 Stores count between matrix and detail differ
		//public double VelocityGetMatrixCellAvgWOS(int aSetValue,int  aBoundary, int aPST, bool aDisplay) // TT#586 Velocity variables not calculated correctly 
        public double VelocityGetMatrixCellAvgWOS(int aSetValue, int aBoundary, int aPST) 
            // end TT#586 Stores count between matrix and detail differ
		{
			return _velocityCriteria.GetMatrixCellAvgWOS(aSetValue,aBoundary,aPST);  // TT#586 Velocity variables not calculated correctly // TT#586 Store count between detail and matrix differ
        }

		/// <summary>
		/// Gets the velocity No On Hand number of storees
		/// </summary>
		public int VelocityGetMatrixNoOnHandStores(int aSetValue) 
		{
			return _velocityCriteria.GetMatrixNoOnHandStores(aSetValue);
		}
		
		/// <summary>
		/// Gets the velocity matrix chain Avg wos
		/// </summary>
		public double VelocityGetMatrixChainAvgWOS(int aGrpLvlRID) 
		{
			return _velocityCriteria.GetMatrixChainAvgWOS(aGrpLvlRID);
		}

		/// <summary>
		/// Gets the velocity matrix chain pct sell thru
		/// </summary>
		public double VelocityGetMatrixChainPctSellThru(int aGrpLvlRID) 
		{
			return _velocityCriteria.GetMatrixChainPctSellThru(aGrpLvlRID);
		}

		/// <summary>
		/// Gets the velocity matrix group level Avg wos
		/// </summary>
		public double VelocityGetMatrixGroupAvgWOS(int aGrpLvlRID) 
		{
			return _velocityCriteria.GetMatrixGroupAvgWOS(aGrpLvlRID);
		} 

		/// <summary>
		/// Gets the velocity matrix group level pct sell thru
		/// </summary>
		public double VelocityGetMatrixGroupPctSellThru(int aGrpLvlRID) 
		{
			return _velocityCriteria.GetMatrixGroupPctSellThru(aGrpLvlRID);
		}

		/// <summary>
		/// Sets the velocity No On Hand rule
		/// </summary>
		public void VelocitySetMatrixNoOnHandRuleType(int aSetValue, eVelocityRuleType aRuleType) 
		{
			_velocityCriteria.SetMatrixNoOnHandRuleType(aSetValue, aRuleType);
		}

		/// <summary>
		/// Sets the velocity No On Hand qty
		/// </summary>
		public void VelocitySetMatrixNoOnHandRuleQty(int aSetValue, double aRuleQty) 
		{
			_velocityCriteria.SetMatrixNoOnHandRuleQty(aSetValue, aRuleQty);
		}

		/// <summary>
		/// Sets the velocity cell rule type
		/// </summary>
		public void VelocitySetMatrixCellType(int aSetValue,int  aBoundary, int aPST, eVelocityRuleType aRuleType) 
		{
            _velocityCriteria.SetMatrixCellType(aSetValue, aBoundary, aPST, aRuleType);  
		}
		 
		/// <summary>
		/// Sets the velocity cell rule qty
		/// </summary>
		public void VelocitySetMatrixCellQty(int aSetValue,int  aBoundary, int aPST, double aRuleQty)   
		{
			_velocityCriteria.SetMatrixCellQty(aSetValue,aBoundary,aPST, aRuleQty);
		}

		/// <summary>
		/// Applies matrix changes
		/// </summary>
		public void VelocityApplyMatrixChanges() 
		{
			_velocityCriteria.ApplyMatrixChanges();
		}

		/// <summary>
		/// Applies matrix rules to stores
		/// </summary>
		public void VelocityApplyRulesToStores() 
		{
			_velocityCriteria.ApplyRulesToStores(this);
		}

		// BEGIN MID Track #2532 - Balance proportional does not save
		/// <summary>
		/// Applies line detail changes to stores
		/// </summary>
		public void VelocityApplyDetailChanges() 
		{
			_velocityCriteria.ApplyDetailChanges();
		}
		// END MID Track #2532

		/// <summary>
		/// Gets or sets the Velocity Store Group RID
		/// </summary>
		public int VelocityStoreGroupRID
		{
			get
			{
				return _velocityCriteria.StoreGroupRID;
			}

			set
			{
				_velocityCriteria.StoreGroupRID = value;
			}
		}

		/// <summary>
		/// Gets or sets OTS Plan RID
		/// </summary>
		public int VelocityOTSPlanHNRID
		{
			get
			{
				return _velocityCriteria.OTSPlanHNRID;
			}

			set
			{
				_velocityCriteria.OTSPlanHNRID = value;
			}
		}

		/// <summary>
		/// Gets or sets Velocity OTS Plan product hierarchy level reference used to dynamically find the allocation plan
		/// </summary>
		public int VelocityOTSPlanPHRID
		{
			get
			{
				return _velocityCriteria.OTSPlanPHRID;
			}

			set
			{
				_velocityCriteria.OTSPlanPHRID = value;
			}
		}

		public int VelocityOTSPlanPHLSeq
		{
			get
			{
				return _velocityCriteria.OTSPlanPHLSeq;
			}

			set
			{
				_velocityCriteria.OTSPlanPHLSeq = value;
			}
		}

		/// <summary>
		/// Gets or sets the dynamic or static Velocity OTS Begin Plan date
		/// </summary>
		public int VelocityOTS_Begin_CDR_RID
		{
			get
			{
				return _velocityCriteria.OTS_Begin_CDR_RID;
			}

			set
			{
				_velocityCriteria.OTS_Begin_CDR_RID = value;
			}
		}

		/// <summary>
		/// Gets or sets the dynamic or static Velocity OTS Ship date
		/// </summary>
		public int VelocityOTS_Ship_To_CDR_RID
		{
			get
			{
				return _velocityCriteria.OTS_Ship_To_CDR_RID;
			}

			set
			{
				_velocityCriteria.OTS_Ship_To_CDR_RID = value;
			}
		}
		/// <summary>
		/// Gets or sets the Velocity Use Similar Store History flag value
		/// </summary>
		public bool VelocityUseSimilarStoreHistory
		{
			get
			{
				return _velocityCriteria.UseSimilarStoreHistory;
			}

			set
			{
				_velocityCriteria.UseSimilarStoreHistory = value;
			}
		}
		/// <summary>
		/// Gets or sets the Velocity Calculate Average Using Chain flag value
		/// </summary>
		public bool VelocityCalculateAverageUsingChain
		{
			get
			{
				return _velocityCriteria.CalculateAverageUsingChain;
			}

			set
			{
				_velocityCriteria.CalculateAverageUsingChain = value;
			}
		}
        //Begin TT#855-MD -jsobek -Velocity Enhancements 
        public eVelocityMethodGradeVariableType VelocityCalculateGradeVariableType
        {
            get
            {
                return _velocityCriteria.GradeVariableType;
            }

            set
            {
                _velocityCriteria.GradeVariableType = value;
            }
        }
        public char VelocityBalanceToHeaderInd
        {
            get
            {
                return _velocityCriteria.BalanceToHeaderInd;
            }

            set
            {
                _velocityCriteria.BalanceToHeaderInd = value;
            }
        }
        //End TT#855-MD -jsobek -Velocity Enhancements 

		/// <summary>
		/// Gets or sets the Velocity Determine Ship Qty Using Basis flag value
		/// </summary>
		public bool VelocityDetermineShipQtyUsingBasis
		{
			get
			{
				return _velocityCriteria.DetermineShipQtyUsingBasis;
			}

			set
			{
				_velocityCriteria.DetermineShipQtyUsingBasis = value;
			}
		}

		/// <summary>
		/// Gets or sets the Velocity Trend Pct Contribution flag value
		/// </summary>
		public bool VelocityTrendPctContribution
		{
			get
			{
				return _velocityCriteria.TrendPctContribution;
			}

			set
			{
				_velocityCriteria.TrendPctContribution = value;
			}
		}

        // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
        public char VelocityApplyMinMaxInd
        {
            get
            {
                return _velocityCriteria.ApplyMinMaxInd;
            }

            set
            {
                _velocityCriteria.ApplyMinMaxInd = value;
            }
        }
        // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)

        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        public char VelocityInventoryInd
        {
            get
            {
                return _velocityCriteria.InventoryInd;
            }

            set
            {
                _velocityCriteria.InventoryInd = value;
            }
        }
        // END TT#1287 - AGallagher - Inventory Min/Max

		/// <summary>
		/// Gets or sets the Velocity Child Occurs data
		/// </summary>
		public DataSet VelocityDSVelocity
		{
			get
			{
				return _velocityCriteria.DSVelocity;
			}

			set
			{
				_velocityCriteria.DSVelocity = value;
			}
		}
		/// <summary>
		/// Gets or sets the Velocity IsInteractive switch
		/// </summary>
		public bool VelocityIsInteractive
		{
			get
			{
				return _velocityCriteria.IsInteractive;
			}

			set
			{
				_velocityCriteria.IsInteractive= value;
			}
		}

		#endregion

        // begin TT#370 Build Packs Enhancement
        #region Interactive Build Packs
        /// <summary>
        /// Special method for processing interactive build packs interactively 
        /// </summary>
        public void ProcessInteractiveBuildPacks()
        {
            // begin TT#488 - MD - Jellis _ Group Allocation
            //CreateMasterAllocationProfileListFromSelectedHeaders();	// TT#696-MD - Build Packs Method runs against wrong selected header list - MOVED TO CALLING ROUTINE
            _buildPacksCriteria.ProcessMethod(this, Include.NoRID, null);
            //AllocationProfileList apl = new AllocationProfileList(eProfileType.Allocation);
            //SelectedHeaderList selectedHeaderList = (SelectedHeaderList)GetProfileList(eProfileType.SelectedHeader);
            //if (selectedHeaderList.Count > 0)
            //{
            //    apl.LoadHeaders(this, selectedHeaderList, SAB.ApplicationServerSession);
            //    SetMasterProfileList(apl);
            //}
            //_buildPacksCriteria.ProcessMethod(this, Include.NoRID, null);
            // end TT#488 - MD - Jellis _ Group Allocation
        }

        /// <summary>
        /// Gets or sets the build packs method
        /// </summary>
        public  BuildPacksMethod BuildPacks
        {
            get { return _buildPacksCriteria; }
            set { _buildPacksCriteria = value; }
        }

        /// <summary>
        /// Gets a flag identifying if the build packs criteria exists
        /// </summary>
        public bool BuildPacksExists
        {
            get { return (_buildPacksCriteria == null) ? false : true; }
        }

 
        /// <summary>
        /// Applies selected option pack pattern to the work up buy
        /// </summary>
        public bool BuildPacksApplyApplySelectedOption(int aOptionPackProfileID, out MIDException aStatusReason)
        {
            return _buildPacksCriteria.ApplySelectedOptionPackProfile(aOptionPackProfileID, out aStatusReason);
        }


        /// <summary>
        /// Gets or sets the Build Packs IsInteractive switch
        /// </summary>
        public bool BuildPacksIsInteractive
        {
            get
            {
                return _buildPacksCriteria.IsInteractive;
            }

            set
            {
                _buildPacksCriteria.IsInteractive = value;
            }
        }

        #endregion  Interactive Build Packs
        // end TT#370 Build Packs Enhancement

		// begin MID Track 4020 Plan level not reset on cancel
		#region MainHierarchyData
		public HierarchyProfile GetMainHierarchyData()
		{
			if (_mainHierarchyData == null)
			{
				_mainHierarchyData = this.SAB.HierarchyServerSession.GetMainHierarchyData();
			}
			return _mainHierarchyData;
		}
		public int GetColorLevelSequence()
		{
			if (_colorHierarchySequence < 0)
			{
				foreach (HierarchyLevelProfile hlp in GetMainHierarchyData().HierarchyLevels.Values)
				{
					if (hlp.LevelType == eHierarchyLevelType.Color)
					{
						_colorHierarchySequence = hlp.Level;
					}
				}
			}
			return _colorHierarchySequence;
		}
		#endregion MainHierarchyData
		// end MID Track 4020 Plan level not reset on cancel

		#region NodeData
		/// <summary>
		/// Gets the hierarchy node data associated with the given hierarchy node
		/// </summary>
		/// <param name="aHnRID">hierarchy node RID</param>
		/// <returns>Hierarchy Node Profile for the given Hierarchy node</returns>
		public HierarchyNodeProfile GetNodeData(int aHnRID)
		{
			if (_nodeDataHashLastKey != aHnRID)
			{
				_nodeDataHashLastKey = aHnRID;
				if (_nodeDataHash == null)
				{
					_nodeDataHash = new Hashtable();
				}
				if (_nodeDataHash.Contains(aHnRID))
				{
					_nodeDataHashLastValue = (HierarchyNodeProfile)_nodeDataHash[aHnRID];
				}
				else
				{
					_nodeDataHashLastValue = SAB.HierarchyServerSession.GetNodeData(aHnRID);
					_nodeDataHash.Add(aHnRID, _nodeDataHashLastValue);
				}
			}
			return _nodeDataHashLastValue;
		}
		#endregion NodeData

		// Begin MID Track 4312 Size Intransit Not relieved at style total
		#region ParentNodeData
		public HierarchyNodeProfile GetParentNodeData(int aHnRID)
		{
			if (_parentHnHashLastParent == null
				|| aHnRID != _parentHnHashLastParent.Key)
			{
				if (_parentHnHash.Contains(aHnRID))
				{
					_parentHnHashLastParent = (HierarchyNodeProfile)_parentHnHash[aHnRID];
				}
				else
				{
					_parentHnHashLastParent = this.SAB.HierarchyServerSession.GetAncestorData(aHnRID, Include.ParentLevelOffset);
					_parentHnHash.Add(aHnRID, _parentHnHashLastParent);
				}
			}
			return _parentHnHashLastParent;
		}
		#endregion ParentNodeData

		#region iktHash
		/// <summary>
		/// Gets Hashtable of intransit key types for a style RID
		/// </summary>
		/// <remarks>
		/// This hashtable contains instances of iktHashContent.  There is an entry for the style, each valid color in that style and each valid color size in that style.
		/// </remarks>
		/// <returns>Hashtable of intransit key types (total style, color and color-size) for this header.</returns>
		public Hashtable Get_iktHash(int aStyleHnRID)
		{
			if (aStyleHnRID != _IKT_HashLastKey)
			{
				if (_IKT_Hash.Contains(aStyleHnRID))
				{
					_IKT_HashLast_iktHash = (Hashtable)_IKT_Hash[aStyleHnRID];
				}
				else
				{
					Hashtable iktHash = new Hashtable();
					IntransitKeyType ikt;
					iktHashContent ihc;
					ikt = new IntransitKeyType(0,0);
					ihc = new iktHashContent(ikt, iktHash.Count, aStyleHnRID);
					iktHash.Add(ikt.IntransitTypeKey, ihc); 
					HierarchyNodeList colorHnRID_pl = 
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
//						GetDescendantData(aStyleHnRID, eHierarchyLevelType.Color); 
						GetDescendantData(aStyleHnRID, eHierarchyLevelType.Color, eNodeSelectType.All); 
//End Track #4037
					HierarchyNodeList sizeHnRID_pl;
					foreach (HierarchyNodeProfile hncp in colorHnRID_pl)
					{
						if (hncp.ColorOrSizeCodeRID != Include.DummyColorRID)
						{
							ikt = new IntransitKeyType(hncp.ColorOrSizeCodeRID, 0);
							ihc = new iktHashContent(ikt, iktHash.Count, hncp.Key); 
							iktHash.Add(ikt.IntransitTypeKey, ihc);
						} 
						sizeHnRID_pl = 
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
//							(GetDescendantData(hncp.Key, eHierarchyLevelType.Size));
							(GetDescendantData(hncp.Key, eHierarchyLevelType.Size, eNodeSelectType.All));
 //End Track #4037
						foreach (HierarchyNodeProfile hnsp in sizeHnRID_pl)
						{
							ikt = new IntransitKeyType(hncp.ColorOrSizeCodeRID, hnsp.ColorOrSizeCodeRID);
							ihc = new iktHashContent(ikt, iktHash.Count, hnsp.Key);
							iktHash.Add(ikt.IntransitTypeKey, ihc);
						}
					}
					_IKT_HashLast_iktHash = iktHash;
				}
			}
			_IKT_HashLastKey = aStyleHnRID;
			return _IKT_HashLast_iktHash;
		}
		#endregion iktHash
		// end MID Track 4312 Size Intransit Not relieved at style total

        // begin TT#1055 - MD - JEllis - Size Need on Size Review does not Include effect of VSW Onhand
        #region GetSizeIntransitKeyTypes
        /// <summary>
        /// Gets an arraylist of size intransit key types associated with a Component
        /// </summary>
        /// <param name="aComponent">Wafer cell coordinate information</param>
        /// <returns>Arraylist of "size" IntransitKeyTypes associated with the cell</returns>
        public ArrayList GetSizeIntransitKeyTypes(GeneralComponent aComponent)
        {
            IntransitKeyType ikt;
            ArrayList IKT = new ArrayList();
            switch (aComponent.ComponentType)
            {
                case (eComponentType.SpecificColor):
                    {
                        ikt = new IntransitKeyType(
                            ((AllocationColorOrSizeComponent)aComponent).ColorRID,
                            Include.IntransitKeyTypeNoSize);
                        IKT.Add(ikt);
                        break;
                    }
                case (eComponentType.ColorAndSize):
                    {
                        AllocationColorSizeComponent acsc = (AllocationColorSizeComponent)aComponent;
                        AllocationColorOrSizeComponent sizeComponent;
                        switch (acsc.ColorComponent.ComponentType)
                        {
                            case (eComponentType.SpecificColor):
                                {
                                    HdrColorBin hcb = this.GetAllocationGrandTotalProfile().GetSubtotalHdrColorBin(((AllocationColorOrSizeComponent)acsc.ColorComponent).ColorRID);
                                    switch (acsc.SizeComponent.ComponentType)
                                    {
                                        case (eComponentType.SpecificSize):
                                            {
                                                ikt = new IntransitKeyType(hcb.ColorCodeRID, ((AllocationColorOrSizeComponent)acsc.SizeComponent).SizeRID);
                                                IKT.Add(ikt);
                                                break;
                                            }
                                        case (eComponentType.SpecificSizePrimaryDim):
                                            {
                                                sizeComponent = (AllocationColorOrSizeComponent)acsc.SizeComponent;
                                                SizeCodeList scl = this.GetSizeCodeByPrimaryDim(sizeComponent.PrimarySizeDimRID);
                                                foreach (SizeCodeProfile scp in scl)
                                                {
                                                    ikt = new IntransitKeyType(hcb.ColorCodeRID, scp.Key);
                                                    IKT.Add(ikt);
                                                }
                                                break;
                                            }
                                        case (eComponentType.SpecificSizeSecondaryDim):
                                            {
                                                sizeComponent = (AllocationColorOrSizeComponent)acsc.SizeComponent;
                                                SizeCodeList scl = this.GetSizeCodeBySecondaryDim(sizeComponent.SecondarySizeDimRID);
                                                foreach (SizeCodeProfile scp in scl)
                                                {
                                                    ikt = new IntransitKeyType(hcb.ColorCodeRID, scp.Key);
                                                    IKT.Add(ikt);
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                // assume all sizes
                                                foreach (HdrSizeBin hsb in hcb.ColorSizes.Values)
                                                {
                                                    ikt = new IntransitKeyType(hcb.ColorCodeRID, hsb.SizeCodeRID); // Assortment: color/size changes
                                                    IKT.Add(ikt);
                                                }
                                                break;
                                            }
                                    }
                                    break;
                                }
                            default:
                                {
                                    // assume All Colors
                                    foreach (HdrColorBin hcb in GetAllocationGrandTotalProfile().BulkColors.Values)
                                    {
                                        switch (acsc.SizeComponent.ComponentType)
                                        {
                                            case (eComponentType.SpecificSize):
                                                {
                                                    int sizeRID = ((AllocationColorOrSizeComponent)acsc.SizeComponent).SizeRID;
                                                    ikt = new IntransitKeyType(hcb.ColorCodeRID, sizeRID);
                                                    IKT.Add(ikt);
                                                    break;
                                                }
                                            case (eComponentType.SpecificSizePrimaryDim):
                                                {
                                                    sizeComponent = (AllocationColorOrSizeComponent)acsc.SizeComponent;
                                                    SizeCodeList scl = this.GetSizeCodeByPrimaryDim(sizeComponent.PrimarySizeDimRID);
                                                    foreach (SizeCodeProfile scp in scl)
                                                    {
                                                        ikt = new IntransitKeyType(hcb.ColorCodeRID, scp.Key);
                                                        IKT.Add(ikt);
                                                    }
                                                    break;
                                                }
                                            case (eComponentType.SpecificSizeSecondaryDim):
                                                {
                                                    sizeComponent = (AllocationColorOrSizeComponent)acsc.SizeComponent;
                                                    SizeCodeList scl = this.GetSizeCodeBySecondaryDim(sizeComponent.SecondarySizeDimRID);
                                                    foreach (SizeCodeProfile scp in scl)
                                                    {
                                                        ikt = new IntransitKeyType(hcb.ColorCodeRID, scp.Key);
                                                        IKT.Add(ikt);
                                                    }
                                                    break;
                                                }
                                            default:
                                                {
                                                    // assume all sizes
                                                    foreach (HdrSizeBin hsb in hcb.ColorSizes.Values)
                                                    {
                                                        ikt = new IntransitKeyType(hcb.ColorCodeRID, hsb.SizeCodeRID); // Assortment: color/size changes
                                                        IKT.Add(ikt);
                                                    }
                                                    break;
                                                }
                                        }
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        ikt = new IntransitKeyType(
                            Include.IntransitKeyTypeNoColor,
                            Include.IntransitKeyTypeNoSize);
                        IKT.Add(ikt);
                        break;
                    }
            }
            return IKT;
        }
        public ArrayList GetTotalSizeIntransitKeyTypes(GeneralComponent aComponent)
        {
            IntransitKeyType ikt;
            ArrayList IKTcolor = new ArrayList();
            switch (aComponent.ComponentType)
            {
                case (eComponentType.SpecificColor):
                    {
                        ikt = new IntransitKeyType(
                            ((AllocationColorOrSizeComponent)aComponent).ColorRID,
                            Include.IntransitKeyTypeNoSize);
                        IKTcolor.Add(ikt);
                        break;
                    }
                case (eComponentType.ColorAndSize):
                    {
                        AllocationColorSizeComponent acsc = (AllocationColorSizeComponent)aComponent;
                        switch (acsc.ColorComponent.ComponentType)
                        {
                            case (eComponentType.SpecificColor):
                                {
                                    HdrColorBin hcb = GetAllocationGrandTotalProfile().GetSubtotalHdrColorBin(((AllocationColorOrSizeComponent)acsc.ColorComponent).ColorRID);
                                    ikt = new IntransitKeyType(hcb.ColorCodeRID, Include.IntransitKeyTypeNoSize);
                                    IKTcolor.Add(ikt);
                                    break;
                                }
                            default:
                                {
                                    // assume All Colors
                                    foreach (HdrColorBin hcb in GetAllocationGrandTotalProfile().BulkColors.Values)
                                    {
                                        ikt = new IntransitKeyType(hcb.ColorCodeRID, Include.IntransitKeyTypeNoSize);
                                        IKTcolor.Add(ikt);
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        IKTcolor.Add(new IntransitKeyType(
                            Include.IntransitKeyTypeNoColor,
                            Include.IntransitKeyTypeNoSize));
                        break;
                    }
            }
            return IKTcolor;
        }
        #endregion GetSizeIntransitKeyTypes

        // end TT#1055 - MD - JEllis - Size Need on Size Review does not Include effect of VSW Onhand

		// Begin TT#1137 (MID Track 4351) Rebuild Intransit Utility
		#region Rebuild Intransit
        /// <summary>
        /// Gets an array list of IntransitUpdateRequest
        /// </summary>
        /// <returns>ArrayList</returns>
		public ArrayList GetRebuildIntransitUpdateRequest()
		{
			return _rebuildIntransitUpdateRequest;
		}
		/// <summary>
		/// Adds an entry to the RebuildIntransitUpdateRequest ArrayList
		/// </summary>
		/// <param name="aIntransitUpdateRequest">The IntransitUpdateRequest to add</param>
		public void SetRebuildIntransitUpdateRequest(IntransitUpdateRequest aIntransitUpdateRequest)
		{
            _rebuildIntransitUpdateRequest.Add(aIntransitUpdateRequest);
		}
		/// <summary>
		/// Clears the RebuildIntransitArray
		/// </summary>
		public void ClearRebuildIntransitUpdateRequest()
		{
			if (_rebuildIntransitUpdateRequest == null)
			{
				_rebuildIntransitUpdateRequest = new ArrayList();
			}
			else
			{
				_rebuildIntransitUpdateRequest.Clear();
			}
		}
		#endregion Rebuild Intransit
		// end TT#1137 (MID Track 4351) Rebuild Intransit Utility

		#region PlanLevelData
		// BEGIN MID Track #3872 - use color or style node for plan level lookup
		//						 - changed parm name from aStyleHnRID to aNodeRID
		/// <summary>
		/// Gets the plan level associated with the hierarchy node RID
		/// </summary>
		/// <param name="aNodeRID">Hierarchy Node RID</param>
		/// <returns>Plan Level associated with the given hierarchy node RID</returns>
		public HierarchyNodeProfile GetPlanLevelData(int aNodeRID)
		{
			if (_planLevelDataHashLastKey != aNodeRID)
			{
				_planLevelDataHashLastKey = aNodeRID;
				if (_planLevelDataHash == null)
				{
					_planLevelDataHash = new Hashtable();
				}
				if (_planLevelDataHash.Contains(aNodeRID))
				{
					_planLevelDataHashLastValue = (HierarchyNodeProfile)_planLevelDataHash[aNodeRID];
				}
				else
				{
					_planLevelDataHashLastValue =	SAB.HierarchyServerSession.GetPlanLevelData(aNodeRID);
					_planLevelDataHash.Add(aNodeRID, _planLevelDataHashLastValue);
				}
			}	// END MID Track #3872
			return _planLevelDataHashLastValue;
		}
		#endregion PlanLevelData

		// BEGIN MID Change j.ellis Performance--cache results of ancestor and descendant lookup
		#region GetAncestorData
		Hashtable _ancestorDataHash = null;
		/// <summary>
		/// Retrieves the ancestor node profile based on the product level sequence of the hierarchy
		/// </summary>
		///<param name="aProductHierarchyRID">Product Hierarchy RID of the ancestor to be retrieved</param>
		///<param name="aNodeRID">Node RID whose ancestor is to be retrieved</param>
		///<param name="aProductLevelSequence">Product Level Sequence of the ancestor to be retrieved</param>
		/// <returns>HierarchyNodeProfile containing the ancestor</returns>
		public HierarchyNodeProfile GetAncestorDataByLevel(int aProductHierarchyRID, int aNodeRID, int aProductLevelSequence)
		{
			if (_ancestorDataHash == null)
			{
				_ancestorDataHash = new Hashtable();
			}
			Hashtable nodeAncestors;
			if (_ancestorDataHash.Contains(aNodeRID))
			{
				nodeAncestors = (Hashtable)_ancestorDataHash[aNodeRID];
			}
			else
			{
				nodeAncestors = new Hashtable();
				_ancestorDataHash.Add(aNodeRID, nodeAncestors);
			}
			long productAncestorKey = (long)((long)aProductHierarchyRID << 32) + (long)aProductLevelSequence;
			HierarchyNodeProfile hnp;
			if (nodeAncestors.Contains(productAncestorKey))
			{
				hnp = (HierarchyNodeProfile)nodeAncestors[productAncestorKey];
			}
			else
			{
				hnp = this.SAB.HierarchyServerSession.GetAncestorDataByLevel(aProductHierarchyRID, aNodeRID, aProductLevelSequence);
				nodeAncestors.Add(productAncestorKey, hnp);
			}
			return hnp;
		}
		#endregion GetAncestorData

		#region GetDescendantData
		Hashtable _descendantDataHash = null;
		/// <summary>
		/// Retrieves descendant node information
		/// </summary>
		///<param name="aNodeRID">Node RID whose descendants are to be retrieved</param>
		///<param name="aHierarchyLevelType">Hierarchy level type of the descendant</param>
		///<returns>HierarchyNodeList containing a list of the descendants</returns>
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
		public HierarchyNodeList GetDescendantData(int aNodeRID, eHierarchyLevelType aHierarchyLevelType,
			eNodeSelectType aNodeSelectType)
//		public HierarchyNodeList GetDescendantData(int aNodeRID, eHierarchyLevelType aHierarchyLevelType)
//End Track #4037
		{
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
			return GetDescendantData(aNodeRID, (eHierarchyLevelMasterType)aHierarchyLevelType, aNodeSelectType);
//			return GetDescendantData(aNodeRID, (eHierarchyLevelMasterType)aHierarchyLevelType);
//End Track #4037
		}
		/// <summary>
		/// Retrieves descendant node information
		/// </summary>
		///<param name="aNodeRID">Node RID whose descendants are to be retrieved</param>
		///<param name="aHierarchyLevelType">Hierarchy level type of the descendant</param>
		///<param name="aNodeSelectType">The type of nodes to select</param>
		///<returns>HierarchyNodeList containing a list of the descendants</returns>
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
		public HierarchyNodeList GetDescendantData(int aNodeRID, eHierarchyLevelMasterType aHierarchyLevelType,
			eNodeSelectType aNodeSelectType)
//		public HierarchyNodeList GetDescendantData(int aNodeRID, eHierarchyLevelMasterType aHierarchyLevelType)
//End Track #4037
		{
			if (_descendantDataHash == null)
			{
				_descendantDataHash = new Hashtable();
			}
			Hashtable nodeDescendants;
			if (_descendantDataHash.Contains(aNodeRID))
			{
				nodeDescendants = (Hashtable)_descendantDataHash[aNodeRID];
				//				nodeDescendants = new Hashtable();
			}
			else
			{
				nodeDescendants = new Hashtable();
				_descendantDataHash.Add(aNodeRID, nodeDescendants);
			}
			HierarchyNodeList hnl;
			if (nodeDescendants.Contains(aHierarchyLevelType))
			{
				hnl = (HierarchyNodeList)nodeDescendants[aHierarchyLevelType];
			}
			else
			{
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
				hnl = this.SAB.HierarchyServerSession.GetDescendantData(aNodeRID, aHierarchyLevelType, false, aNodeSelectType);
//				hnl = this.SAB.HierarchyServerSession.GetDescendantData(aNodeRID, aHierarchyLevelType);
//End Track #4037
				nodeDescendants.Add(aHierarchyLevelType, hnl);
			}
			return hnl;
		}
		#endregion GetDescendantData

		// END MID Change j.ellis Performance--cache results of ancestor and descendant lookup
		// begin MID Track 4362 Alternate Intransit Performance
		public ArrayList GetIntransitReadNodes (int aPlanLevelRID, eHierarchyLevelMasterType aHierarchyLevelMasterType)
		{
			if (aPlanLevelRID != _lastIntransitReadNodeHashKey)
			{
				_lastIntransitReadNodeHashLevel = eHierarchyLevelMasterType.Undefined;
				_lastIntransitReadNodeHashKey = aPlanLevelRID;
				_lastIntransitReadPllvHash = (Hashtable)_intransitReadNodeHash[_lastIntransitReadNodeHashKey];
				if (_lastIntransitReadPllvHash == null)
				{
					_lastIntransitReadPllvHash = new Hashtable();
					_intransitReadNodeHash.Add(aPlanLevelRID, _lastIntransitReadPllvHash);
				}
			}
			if (aHierarchyLevelMasterType != _lastIntransitReadNodeHashLevel)
			{
				_lastIntransitReadNodeHashLevel = aHierarchyLevelMasterType;
				_lastIntransitReadNodeArray = (ArrayList)_lastIntransitReadPllvHash[_lastIntransitReadNodeHashLevel];
				if (_lastIntransitReadNodeArray == null)
				{
					_lastIntransitReadNodeArray = this.SAB.HierarchyServerSession.GetIntransitReadNodes(aPlanLevelRID, aHierarchyLevelMasterType);
					_lastIntransitReadPllvHash.Add(_lastIntransitReadNodeHashLevel, _lastIntransitReadNodeArray);
				}
			}
			return _lastIntransitReadNodeArray;
		}
		// end MID Track 4362 Alternate Intransit Performance

		// BEGIN TT#1401 - stodd - add resevation stores (IMO) * REMOVED *
		//// BEGIN TT#1401 - stodd - add resevation stores (IMO)
		//public ArrayList GetIMOReadNodes(int aPlanLevelRID, eHierarchyLevelMasterType aHierarchyLevelMasterType)
		//{
		//    if (aPlanLevelRID != _lastIMOReadNodeHashKey)
		//    {
		//        _lastIMOReadNodeHashLevel = eHierarchyLevelMasterType.Undefined;
		//        _lastIMOReadNodeHashKey = aPlanLevelRID;
		//        _lastIMOReadPllvHash = (Hashtable)_IMOReadNodeHash[_lastIMOReadNodeHashKey];
		//        if (_lastIMOReadPllvHash == null)
		//        {
		//            _lastIMOReadPllvHash = new Hashtable();
		//            _IMOReadNodeHash.Add(aPlanLevelRID, _lastIMOReadPllvHash);
		//        }
		//    }
		//    if (aHierarchyLevelMasterType != _lastIMOReadNodeHashLevel)
		//    {
		//        _lastIMOReadNodeHashLevel = aHierarchyLevelMasterType;
		//        _lastIMOReadNodeArray = (ArrayList)_lastIMOReadPllvHash[_lastIMOReadNodeHashLevel];
		//        if (_lastIMOReadNodeArray == null)
		//        {
		//            _lastIMOReadNodeArray = this.SAB.HierarchyServerSession.GetIMOReadNodes(aPlanLevelRID, aHierarchyLevelMasterType);
		//            _lastIMOReadPllvHash.Add(_lastIMOReadNodeHashLevel, _lastIMOReadNodeArray);
		//        }
		//    }
		//    return _lastIMOReadNodeArray;
		//}
		//// END TT#1401 - stodd - add resevation stores (IMO)
		// END TT#1401 - stodd - add resevation stores (IMO)


		#region ColorCodeProfile
		public ColorCodeProfile GetColorCodeProfile(int aColorRID)
		{
			ProfileList cpl = (ProfileList)this.GetMasterProfileList(eProfileType.ColorCode); 
			if (cpl == null)
			{
				cpl = new ProfileList(eProfileType.ColorCode);
				this.SetMasterProfileList(cpl);
			}
			if (!cpl.Contains(aColorRID))
			{
				cpl.Add(this.SAB.HierarchyServerSession.GetColorCodeProfile(aColorRID));
			}
			return (ColorCodeProfile) cpl.FindKey(aColorRID);
		}
		#endregion ColorCodeProfile
		#region SizeCodeProfile
		/// <summary>
		/// Load the specified Size Code Profiles into the Master Proflie List
		/// </summary>
		/// <param name="aSizeCodeRID">Array of size code RIDs</param>
		public void LoadSizeCodeProfiles(int[] aSizeCodeRID)
		{
			foreach (int sizeCodeRID in aSizeCodeRID)
			{
				GetSizeCodeProfile(sizeCodeRID);
			}
		}
        SizeCodeProfile _lastSizeCodeProfile;
		public SizeCodeProfile GetSizeCodeProfile(int aSizeRID)
		{
            // begin TT#1391 - TMW new Action
            if (_lastSizeCodeProfile != null)
            {
                if (_lastSizeCodeProfile.Key == aSizeRID)
                {
                    return _lastSizeCodeProfile;
                }
            }
            // end TT#1391 - TMW new Action

			ProfileList spl = (ProfileList)this.GetMasterProfileList(eProfileType.SizeCode); 
			if (spl == null)
			{
				spl = new ProfileList(eProfileType.SizeCode);
				this.SetMasterProfileList(spl);
				this._primarySizeCodeByRID= new Hashtable();
				this._primarySizeCodeByPrimary = new Hashtable();
				this._secondarySizeCodeByRID = new Hashtable();
				this._secondarySizeCodeBySecondary = new Hashtable();
				this._sizeCodeByPrimarySecondary = new Hashtable();
			}
			if (!spl.Contains(aSizeRID))
			{
				//				spl.Add(this.SAB.HierarchyServerSession.GetColorCodeProfile(aSizeRID));
				SizeCodeProfile scp = this.SAB.HierarchyServerSession.GetSizeCodeProfile(aSizeRID);
				spl.Add(scp);
				// add this primary size code to the list of primary size code's 
				if (!_primarySizeCodeByRID.Contains(scp.SizeCodePrimaryRID))
				{
					_primarySizeCodeByRID.Add(scp.SizeCodePrimaryRID,scp.SizeCodePrimary);
					_primarySizeCodeByPrimary.Add(scp.SizeCodePrimary, scp.SizeCodePrimaryRID);
				}

				// add this primary size code to the list of primary size code's 
				if (!_secondarySizeCodeByRID.Contains(scp.SizeCodeSecondaryRID))
				{
					_secondarySizeCodeByRID.Add(scp.SizeCodeSecondaryRID, scp.SizeCodeSecondary);
					_secondarySizeCodeBySecondary.Add(scp.SizeCodeSecondary, scp.SizeCodeSecondaryRID);
				}
				long primarySecondaryKey = (((long)scp.SizeCodePrimaryRID)<<32) + (long)scp.SizeCodeSecondaryRID;
				ProfileList sizeCodeProfileList;
				if (_sizeCodeByPrimarySecondary.Contains(primarySecondaryKey))
				{
					sizeCodeProfileList = (ProfileList)_sizeCodeByPrimarySecondary[primarySecondaryKey];
				}
				else
				{
					sizeCodeProfileList = new ProfileList(eProfileType.SizeCode);
					_sizeCodeByPrimarySecondary.Add(primarySecondaryKey, sizeCodeProfileList);
				}
				if (!sizeCodeProfileList.Contains(scp.Key))
				{
					sizeCodeProfileList.Add(scp);
				}				
			}
            // begin TT#1391 - TMW New Action
            _lastSizeCodeProfile = (SizeCodeProfile)spl.FindKey(aSizeRID);
            return _lastSizeCodeProfile;
            //return (SizeCodeProfile) spl.FindKey(aSizeRID);
            // end TT#1391 - TMW New Action
		}
		/// <summary>
		/// Gets a profile list of size code profiles whose primary and secondary dimension RID's match the supplied RIDs
		/// </summary>
		/// <param name="aPrimaryRID">RID of the primary size dimension</param>
		/// <param name="aSecondaryRID">RID of the secondary size dimension</param>
		/// <returns>ProfileList of SizeCodes with the same primary and secondary dimensions</returns>
		public ProfileList GetSizeCodeByPrimarySecondary(int aPrimaryRID, int aSecondaryRID)
		{
			long key = (((long)aPrimaryRID)<<32) + (long)aSecondaryRID;
			// begin MID Track xxxx Performance
			//if (_sizeCodeByPrimarySecondary.Contains(key))
			//{
			//	return (ProfileList)_sizeCodeByPrimarySecondary[key];
			//}
			ProfileList sizeCode = (ProfileList)_sizeCodeByPrimarySecondary[key];
			if (sizeCode != null)
			{
				return sizeCode;
			}
			// end MID Track xxxx Performance
			return new ProfileList(eProfileType.SizeCode);
		}
		/// <summary>
		/// Gets a profile list of size code profiles whose primary and secondary dimension RID's match the supplied RIDs
		/// </summary>
		/// <param name="aPrimaryCode">The primary size code</param>
		/// <param name="aSecondaryCode">The secondary size code</param>
		/// <returns>ProfileList of SizeCodes with the same primary and secondary codes</returns>
		public ProfileList GetSizeCodeByPrimarySecondary(string aPrimaryCode, int aSecondaryCode)
		{
			return GetSizeCodeByPrimarySecondary((int)this._primarySizeCodeByPrimary[aPrimaryCode], (int)this._secondarySizeCodeBySecondary[aSecondaryCode]);
		}

		/// <summary>
		/// Gets a profile list of size code profiles whose primary RID's match the supplied RID
		/// </summary>
		/// <param name="aPrimaryRID">RID of the primary size dimension</param>
		/// <returns>ProfileList of SizeCodes with the same primary dimensions</returns>
		public SizeCodeList GetSizeCodeByPrimaryDim(int aPrimaryRID)
		{
			SizeCodeList sizeCodeList = new SizeCodeList(eProfileType.SizeCode);
			int[] secondaryDimRID = this.GetSizeDimensionsRID(false);
			
			foreach (int sdr in secondaryDimRID)
			{
				ProfileList pl = this.GetSizeCodeByPrimarySecondary(aPrimaryRID, sdr);
				if (pl.Count > 0)
				{
					sizeCodeList.AddRange(pl.ArrayList);
				}
			}
			return sizeCodeList;
		}
		/// <summary>
		/// Gets a profile list of size code profiles whose secondary RID's match the supplied RID
		/// </summary>
		/// <param name="aSecondaryRID">RID of the secondary size dimension</param>
		/// <returns>ProfileList of SizeCodes with the same secondary dimensions</returns>
		public SizeCodeList GetSizeCodeBySecondaryDim(int aSecondaryRID)
		{
			SizeCodeList sizeCodeList = new SizeCodeList(eProfileType.SizeCode);
			int[] primaryDimRID = this.GetSizeDimensionsRID(true);
			
			foreach (int pdr in primaryDimRID)
			{
				ProfileList pl = this.GetSizeCodeByPrimarySecondary(pdr, aSecondaryRID);
				if (pl.Count > 0)
				{
					sizeCodeList.AddRange(pl.ArrayList);
				}
			}
			return sizeCodeList;
		}
		/// <summary>
		/// Gets a profile list of all size code profiles 
		/// </summary>
		/// <returns>ProfileList of all SizeCodes</returns>
		public SizeCodeList GetAllSizeCodes()
		{
			SizeCodeList sizeCodeList = new SizeCodeList(eProfileType.SizeCode);
			int[] primaryDimRID = this.GetSizeDimensionsRID(true);
			
			foreach (int pdr in primaryDimRID)
			{
				SizeCodeList scl = GetSizeCodeByPrimaryDim(pdr);
				if (scl.Count > 0)
				{
					sizeCodeList.AddRange(scl.ArrayList);
				}
			}
			return sizeCodeList;
		}

		/// <summary>
		/// Gets a list of the valid dimension Names
		/// </summary>
		/// <param name="aGetPrimaryDimensions">True:  Get Primary Dimensions; False: Get Secondary Dimensions</param>
		/// <returns>List of the requested dimensions</returns>
		public string[] GetSizeDimensionsNames (bool aGetPrimaryDimensions)
		{
			string[] dimNames;
			if (aGetPrimaryDimensions)
			{
				dimNames = new string[this._primarySizeCodeByPrimary.Count];
				this._primarySizeCodeByPrimary.Keys.CopyTo(dimNames,0);
				return dimNames;
			}
			dimNames = new string[this._secondarySizeCodeBySecondary.Count];
			this._secondarySizeCodeBySecondary.Keys.CopyTo(dimNames,0);
			return dimNames;
		}
		/// <summary>
		/// Gets a list of the valid dimension RIDs
		/// </summary>
		/// <param name="aGetPrimaryDimensions">True:  Get Primary Dimensions; False: Get Secondary Dimensions</param>
		/// <returns>List of the requested dimension RIDs</returns>
		public int[] GetSizeDimensionsRID (bool aGetPrimaryDimensions)
		{
			int[] RIDs;
			if (aGetPrimaryDimensions)
			{
				RIDs = new int[this._primarySizeCodeByRID.Count];
				this._primarySizeCodeByRID.Keys.CopyTo(RIDs,0);
				return RIDs;
			}
			RIDs = new int[this._secondarySizeCodeByRID.Count];
			this._secondarySizeCodeByRID.Keys.CopyTo(RIDs,0);
			return RIDs;
		}
		public int GetSizeDimensionRID (bool aGetPrimaryDimension, string aDimensionName)
		{
			if (aGetPrimaryDimension)
			{
				if (this._primarySizeCodeByPrimary.Contains(aDimensionName))
				{
					return (int)this._primarySizeCodeByPrimary[aDimensionName];
				}
			}
			else if (this._secondarySizeCodeBySecondary.Contains(aDimensionName))
			{
				return (int)this._secondarySizeCodeBySecondary[aDimensionName];
			}
			return Include.NoRID;
		}
		public string GetSizeDimensionName(bool aGetPrimaryDimension, int aDimensionRID)
		{
			if (aGetPrimaryDimension)
			{
				if (this._primarySizeCodeByRID.Contains(aDimensionRID))
				{
					return (string)this._primarySizeCodeByRID[aDimensionRID];
				}
			}
			else if (this._secondarySizeCodeByRID.Contains(aDimensionRID))
			{
				return (string)this._secondarySizeCodeByRID[aDimensionRID];
			}
			return null;
		}

        // Begin TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
        public bool SizeCodeWaferBuilt(int aSizeCode)
        {
            if (_sizeViewSizesAL.Contains(aSizeCode))
            {
                return true;
            }
            else
            {
                _sizeViewSizesAL.Add(aSizeCode);
                return false;
            }
        }
        // End TT#234  
		#endregion SizeCodeProfile

		// begin MID Track 4372 Generic Size Constraints
		#region GenericSizeNames
		/// <summary>
		/// Gets a Header's Characteristic Group ID associated with the specified RID
		/// </summary>
		/// <param name="aHeaderCharacteristicGroupRID">RID of the Header Characteristic Group</param>
		/// <returns>Header Characteristic ID</returns>
		public string GetCharacteristicGroupID(int aHeaderCharacteristicGroupRID)
		{
			if (aHeaderCharacteristicGroupRID != _lastHeaderCharacteristicGroupRID)
			{
				_lastHeaderCharacteristicGroupRID = aHeaderCharacteristicGroupRID;
				if (_headerCharacteristicProfileList == null)
				{
					_headerCharacteristicProfileList = SAB.HeaderServerSession.GetHeaderCharGroups();
				}
				_lastHeaderCharacteristicProfile = (HeaderCharGroupProfile)_headerCharacteristicProfileList.FindKey(aHeaderCharacteristicGroupRID);
				if (_lastHeaderCharacteristicProfile == null)
				{
					_lastHeaderCharacteristicProfile = new HeaderCharGroupProfile(_lastHeaderCharacteristicGroupRID);
				}
			}
			return _lastHeaderCharacteristicProfile.ID;
		}
		/// <summary>
		/// Gets a data table containing the valid characteristics to use in construction of a generic name
		/// </summary>
		/// <param name="aHeaderRID">RID of the header used to retrieve the characteristics</param>
		/// <returns>DataTable of Header Characteristics that may be used to construct a generic name</returns>
		private DataTable GetHeaderGenericNameCharacteristics(int aHeaderRID)
		{
			if (aHeaderRID != _lastGenericNameCharacteristicHeaderKey)
			{
				_lastHeaderGenericNameCharacteristicsDT = (DataTable)_headerGenericNameCharacteristics[aHeaderRID];
				if (_lastHeaderGenericNameCharacteristicsDT == null)
				{
					Header header = new Header();
					try
					{
						_lastHeaderGenericNameCharacteristicsDT = header.GetCharacteristics(aHeaderRID, false);
					}
					catch
					{
						_lastGenericNameCharacteristicHeaderKey = int.MinValue;
						throw;
					}
					_lastGenericNameCharacteristicHeaderKey = aHeaderRID;
					_headerGenericNameCharacteristics.Add(aHeaderRID, _lastHeaderGenericNameCharacteristicsDT);
				}
			}
            return _lastHeaderGenericNameCharacteristicsDT;
		}
		/// <summary>
		/// Gets a Generic Size Name based on Allocation Profile, header Characteristic, Merchandise criteria and Color
		/// </summary>
		/// <param name="ap">Allocation Profile for which a generic size name is desired</param>
		/// <param name="aGenericCharacteristicGroupRID">Characteristic Group RID which is to be used to construct the generic name; if "-1" (ie. Include.NoRID) then no characteristic is used in the construction of the name</param>
		/// <param name="aMerchandiseType">Merchandise Type to identify the key type of merchandise described in Allocation Merchandise Bin (allows selection of "none"</param>
		/// <param name="aAllocationMerchBin">Allocation Merchandise Bin that describes the merchandise to be used to construct the generic name</param>
		/// <param name="aColorCodeRID">Color Code RID to be used in the construction of the generic name; if "-1" (ie. Include.NoRID) then no color code is used in the name construction.</param>
		/// <returns>Empty string if the generic size name could not be built or a non-empty string if the string can be constructed from the information provided</returns>
		public string GetGenericSizeName(
			AllocationProfile ap, 
			int aGenericCharacteristicGroupRID,
			eMerchandiseType aMerchandiseType,
			AllocationMerchBin aAllocationMerchBin,
			int aColorCodeRID,
            string aNodeCurveName, bool aUseDefaultCurve, bool aThisIsSizeCurve)  // TT#413 - RMatelic add aNodeCurveName, TT#438 - add aUseDefaultCurve, aThisIsSizeCurve)
		{
			try
			{
				long headerCharacteristicKey = (long)ap.Key << 32;
				if (aGenericCharacteristicGroupRID == Include.NoRID)
				{
					headerCharacteristicKey += (long)aGenericCharacteristicGroupRID;
				}
				else
				{
					headerCharacteristicKey += Include.LongNoRID;
				}
				int colorKey = aColorCodeRID;
				HierarchyNodeProfile merchandiseHierarchyNodeProfile = null;
				switch (aMerchandiseType)
				{
					case (eMerchandiseType.OTSPlanLevel):
					{
						merchandiseHierarchyNodeProfile = GetNodeData(ap.PlanHnRID);
						break;
					}
					case (eMerchandiseType.Node):
					{
						merchandiseHierarchyNodeProfile = 
							GetNodeData(aAllocationMerchBin.MdseHnRID);
						break;
					}
					case (eMerchandiseType.HierarchyLevel):
					{
						merchandiseHierarchyNodeProfile = 
							GetAncestorDataByLevel(aAllocationMerchBin.ProductHnLvlRID, ap.StyleHnRID, aAllocationMerchBin.ProductHnLvlSeq);
						break;
					}
					default:
					{
						merchandiseHierarchyNodeProfile = 
							new HierarchyNodeProfile(Include.NoRID);
						break;
					}
				}
				if (headerCharacteristicKey != _lastGenericHeaderCharacteristicKey)
				{
					_lastGenericHeaderCharacteristicKey = headerCharacteristicKey;
					_lastGenericMerchandiseHierarchyNodeProfile = new HierarchyNodeProfile(int.MinValue);  // must have a value different from -1 to trigger retrieval of children hash tables
					_lastGenericColorCodeRID = int.MinValue;
					_lastGenericMerchandiseHash = (Hashtable)_genericHeaderCharacteristicHash[headerCharacteristicKey];
					if (_lastGenericMerchandiseHash == null)
					{
						_lastGenericMerchandiseHash = new Hashtable();
						_genericHeaderCharacteristicHash.Add(headerCharacteristicKey, _lastGenericMerchandiseHash);
					}
				}
				if (merchandiseHierarchyNodeProfile.Key != _lastGenericMerchandiseHierarchyNodeProfile.Key)
				{
					_lastGenericColorCodeRID = int.MinValue;
					_lastGenericMerchandiseHierarchyNodeProfile = merchandiseHierarchyNodeProfile;
					_lastGenericColorHash = (Hashtable)_lastGenericMerchandiseHash[merchandiseHierarchyNodeProfile.Key];
					if (_lastGenericColorHash == null)
					{
						_lastGenericColorHash = new Hashtable();
						_lastGenericMerchandiseHash.Add(merchandiseHierarchyNodeProfile.Key, _lastGenericColorHash);
					}
				}
				if (colorKey != _lastGenericColorCodeRID)
				{
					_lastGenericColorCodeRID = colorKey;
					_lastGenericSizeName = (string)_lastGenericColorHash[colorKey];
					if (_lastGenericSizeName == null 
						|| _lastGenericSizeName == string.Empty)
					{
						_lastGenericSizeName = string.Empty;
						StringBuilder sbGenericName = new StringBuilder();
                        if (aGenericCharacteristicGroupRID != Include.NoRID)
						{
							string genCharacteristicGroupID = GetCharacteristicGroupID(aGenericCharacteristicGroupRID);
							DataTable dtCharData = this.GetHeaderGenericNameCharacteristics(ap.Key);
							if (dtCharData.Rows.Count > 0)
							{
								foreach (DataRow charRow in dtCharData.Rows)
								{
									if (charRow["hcg_id"].ToString() == genCharacteristicGroupID)  
									{
										switch ((eHeaderCharType)(int)charRow["hcg_type"])
										{
											case eHeaderCharType.text:
											{
												sbGenericName = AddToGenericName(sbGenericName, charRow["text_value"].ToString());
												break;
											}
											case eHeaderCharType.number:
											{
												sbGenericName = AddToGenericName(sbGenericName, charRow["number_value"].ToString());
												break;
											}
											case eHeaderCharType.date:
											{
												sbGenericName = AddToGenericName(sbGenericName, charRow["date_value"].ToString());
												break;
											}
											case eHeaderCharType.dollar:
											{
												sbGenericName = AddToGenericName(sbGenericName, charRow["dollar_value"].ToString());
												break;
											}
										}
										break;
									}
								}
							}
							if (sbGenericName.Length == 0)
							{
								this._audit.Add_Msg(
									eMIDMessageLevel.Severe,
									eMIDTextCode.msg_al_NoGenericSizeCharacteristicForHeader,
									string.Format(MIDText.GetText(eMIDTextCode.msg_al_NoGenericSizeCharacteristicForHeader),ap.HeaderID,genCharacteristicGroupID),
									this.GetType().Name);
								return _lastGenericSizeName;
							}
						}

                        // Begin TT#438 - RMatelic - Size Curve generated in node properties- used default in Size Need method but requires a size curve (incorrect)
                        if (aUseDefaultCurve && aThisIsSizeCurve)
                        {
                            string defaultCurveName = GetDefaultSizeCurveName(ap);
                            sbGenericName.Append(defaultCurveName);
                        }
                        else
                        // End TT#438
                        {
                            if (merchandiseHierarchyNodeProfile != null
                                && merchandiseHierarchyNodeProfile.Key != Include.NoRID)
                            {
                                sbGenericName = AddToGenericName(sbGenericName, merchandiseHierarchyNodeProfile.NodeID);
                            }
                            // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                            if (aNodeCurveName != null && aNodeCurveName != string.Empty)
                            {
                                // Begin TT#438 - RMatelic - Size Curve generated in node properties- used default in Size Need method but requires a size curve (incorrect)
                                // Node Curve Name uses a preceding dash (-) instead of the product delimiter
                                //sbGenericName   = AddToGenericName(sbGenericName, aNodeCurveName);
                                if (sbGenericName.Length > 0)
                                {
                                    sbGenericName.Append("-");
                                }
                                sbGenericName.Append(aNodeCurveName);
                                // End TT#438
                            }
                            // End TT#413 
                            if (aColorCodeRID != Include.NoRID)
                            {
                                ColorCodeProfile ccp = GetColorCodeProfile(aColorCodeRID);
                                sbGenericName = AddToGenericName(sbGenericName, ccp.ColorCodeID);
                            }
                        }

                        if (sbGenericName.Length > 0)
						{
							_lastGenericSizeName = sbGenericName.ToString();
							_lastGenericColorHash.Add(colorKey, _lastGenericSizeName);
						}
					}					
				}
			}
			catch (Exception ex)
			{
				_lastGenericHeaderCharacteristicKey = long.MinValue;
				_lastGenericMerchandiseHierarchyNodeProfile = new HierarchyNodeProfile(int.MinValue);  // must have a value different from -1 to trigger retrieval of children hash tables
				_lastGenericColorCodeRID = int.MinValue;
				_lastGenericSizeName = string.Empty;
				this._audit.Log_Exception(
					ex,
                    this.GetType().Name,
					eExceptionLogging.logAllInnerExceptions);
			}
			return _lastGenericSizeName;
		}
		/// <summary>
		/// Appends a given string to a specified string builder.
		/// </summary>
		/// <param name="aGenericName">String Builder containing the existing string</param>
		/// <param name="aString">String to append</param>
		/// <returns>Resulting string builder</returns>
		private StringBuilder AddToGenericName(StringBuilder aGenericName, string aString)
		{
			if (aGenericName.Length > 0)
			{
				aGenericName.Append(_productLevelDelimiter);
			}
			aGenericName.Append(aString);
			return aGenericName;
		}
		#endregion Generic Size Names
        // begin TT#59 Implement Store Temp Locks
        #region GetHeaderInformation
        public HeaderInformationStruct[] GetHeaderInformation()
        {
            AllocationProfileList apl = GetAllocationProfileList();
            if (apl.Count > 0)
            {
                HeaderInformationStruct[] hisArray = new HeaderInformationStruct[apl.Count];
                int i = 0;
                ColorCodeProfile ccp;
                HeaderColorInformationStruct[] hcis;
                foreach (AllocationProfile ap in apl)
                {
                    hcis = new HeaderColorInformationStruct[ap.BulkColors.Count];
                    int j = 0;
                    HierarchyNodeProfile styleNode = GetNodeData(ap.StyleHnRID);
                    HierarchyNodeProfile otsForecastNode = GetNodeData(ap.PlanHnRID);
                    HierarchyNodeProfile onhandNode = GetNodeData(ap.OnHandHnRID);
                    HierarchyNodeProfile capacityNode = GetNodeData(ap.CapacityNodeRID);
                    HierarchyNodeList hnl = this.GetDescendantData(ap.StyleHnRID, eHierarchyLevelType.Color,eNodeSelectType.All);
                    foreach (HierarchyNodeProfile hnp in hnl)
                    {
                        HdrColorBin hcb = (HdrColorBin)ap.BulkColors[hnp.ColorOrSizeCodeRID];
                        if (hcb != null)
                        {
                            ccp = this.GetColorCodeProfile(hcb.ColorCodeRID);
                            hcis[j] = new HeaderColorInformationStruct(
                                ccp.ColorCodeID,
                                ccp.ColorCodeName,
                                ccp.Key,
                                hnp.LevelText,
                                hnp.Key,
                                hcb.ColorMinimum,
                                hcb.ColorMaximum);
                            j++;
                        }
                    }
                    hisArray[i] = 
                        new HeaderInformationStruct(
                            ap.HeaderRID,
                            ap.HeaderID,
                            styleNode.LevelText,
                            otsForecastNode.LevelText,
                            ap.PlanFactor,
                            onhandNode.LevelText,
                            ap.GradeWeekCount,
                            ap.PercentNeedLimit,
                            ap.BeginDay,
                            capacityNode.LevelText,
                            ap.GradeList,
                            hcis);
                    i++;
                }
                return hisArray;
            }
            return null;
        }
        #endregion GetHeaderInformation
        // begin TT#59 Implement Temp Locks - Lock fails when errors
        #region    RecoverHeader
        Dictionary<int, HoldAllocation> _holdHeaderAllocations;
        /// <summary>
        /// Temporarily holds the allocation of the header with the given key
        /// </summary>
        /// <param name="aHdrRID">RID of the header whose allocation is to be held</param>
        /// <remarks>Only ONE allocation will be kept for a given header RID and that will be the first one held</remarks>
        private void HoldHeaderAllocation(int aHdrRID)
        {
            AllocationProfile ap = this.GetAllocationProfileList().FindKey(aHdrRID) as AllocationProfile;
            if (ap != null)
            {
                HoldHeaderAllocation(ap); 
            }
        }
        /// <summary>
        /// Temporarily holds a header allocation
        /// </summary>
        /// <param name="aAp">Allocation Profile containing the header allocation</param>
        /// <remarks>Only ONE allocation will be kept for a given header RID and that will be the first one held</remarks>
        internal void HoldHeaderAllocation(AllocationProfile aAp)
        // begin TT#4210 - MD - Jellis -  Qty Allocated Cannot be negative
        {
            HoldHeaderAllocation(aAp, false);
        }
        internal void HoldHeaderAllocation(AllocationProfile aAp, bool aKeepManualAlocntAfterCapture)
        // end TT#4210 - MD - Jellis -  Qty Allocated Cannot be negative
        {
            if (_holdHeaderAllocations == null)
            {
                _holdHeaderAllocations = new Dictionary<int, HoldAllocation>();
            }
            else if (_holdHeaderAllocations.ContainsKey(aAp.Key))
            {
                return;
            }
            _holdHeaderAllocations.Add(aAp.Key, new HoldAllocation(aAp, false, false)); 	// TT#4735 - stodd - Cancel Allocation throws Null Reference Error after Need action on a Group Allocation
        }
        /// <summary>
        /// Recovers all held header allocations
        /// </summary>
        internal void RecoverHeaderAllocation()
        {
            if (_holdHeaderAllocations == null)
            {
                return;
            }
            foreach (int hdrKey in _holdHeaderAllocations.Keys)
            {
                RecoverHeaderAllocation(hdrKey);
            }
        }
        /// <summary>
        /// Recovers the header allocation for the given header RID
        /// </summary>
        /// <param name="aHdrRID">RID of the header</param>
        internal void RecoverHeaderAllocation(int aHdrRID) // TT#1064 - MD - Jellis - Cannot Release Group Allocation
        {
            if (_holdHeaderAllocations != null)
            {
                HoldAllocation ha;
                _holdHeaderAllocations.TryGetValue(aHdrRID,out ha);
                if (ha != null)
                {
                    ha.RestoreAllocation();
                }
            }
        }
        /// <summary>
        /// Removes the held allocation for the given header RID (this allocation will no longer be recoverable)
        /// </summary>
        /// <param name="aHdrRID">Header RID</param>
        internal void RemoveHeldAllocation(int aHdrRID) // TT#1064 - MD - Jellis - Cannot Release Group Allocation
        {
            if (_holdHeaderAllocations != null)
            {
                _holdHeaderAllocations.Remove(aHdrRID);
            }
        }
        /// <summary>
        /// Clears all held allocations.
        /// </summary>
        internal void ClearHeldAllocations() // TT#1008 - MD - Jellis - Get Non Negative Message for various variables
        {
            if (_holdHeaderAllocations == null)
            {
                return;
            }
            _holdHeaderAllocations.Clear();
        }
        #endregion RecoverHeader
        // end TT#59 Implement Temp Locks - Lock fails when errors
        // end TT#59 Implement Store Temp Locks
        // end MID Track 4372 Generic Size Constraints
        // Begin TT#438 - RMatelic - Size Curve generated in node properties- used default in Size Need method but requires a size curve (incorrect)
        #region DefaultSizeCurve 
        public string GetDefaultSizeCurveName(AllocationProfile ap)
        {
            string sizeCurveName = string.Empty;
            try
            {
                SizeCurveCriteriaProfile sccp = null;                
                HierarchyNodeProfile hnpStyle  = SAB.HierarchyServerSession.GetNodeData(ap.StyleHnRID);
                
                // if header has exactly 1 color, get default curve for the color node, otherwise get default curve for style node
                // Begin TT#487 - RMatelic - Size Curve Node Properties Default setting.  When using the Size Curve method with the default set not getting expected results.
                // Rewrote most of this code
                int hdrColorCount = 0;
                int colorCodeRID = GetColorCodeRID(ap, ref hdrColorCount);
                if (colorCodeRID > 0)
                {
                    int colorHnRID = -1;
                    ColorCodeProfile ccp =  SAB.HierarchyServerSession.GetColorCodeProfile(colorCodeRID);
                    if (SAB.HierarchyServerSession.ColorExistsForStyle(hnpStyle.HomeHierarchyRID, hnpStyle.Key, ccp.ColorCodeID, ref colorHnRID))
                    {
                        HierarchyNodeProfile hnpColor  = SAB.HierarchyServerSession.GetNodeData(colorHnRID);
                        sccp = SAB.HierarchyServerSession.GetDefaultSizeCurveCriteriaProfile(colorHnRID);
                        if (sccp == null)
                        {
                            string msgText = MIDText.GetText(eMIDTextCode.msg_al_NoDefaultSizeCurveDefined);
					        msgText = msgText.Replace("{0}",ap.HeaderID);
					        SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msgText, this.GetType().Name);
                        }
                        else
                        {
                            HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetDefaultSizeCurveCriteriaNode(hnpColor, sccp, hdrColorCount);
                            if (hnp == null)
                            {
                                string msgText = MIDText.GetText(eMIDTextCode.msg_al_NoDefaultSizeCurveNode);
                                msgText = msgText.Replace("{0}", ap.HeaderID);
                                SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msgText, this.GetType().Name);
                            }
                            else
                            {
                                sizeCurveName = SAB.HierarchyServerSession.GetSizeCurveCriteriaProfileCurveName(hnp, sccp);
                            }
                        }
                    }   // End TT#487
                }
                else
                {
                    sccp = SAB.HierarchyServerSession.GetDefaultSizeCurveCriteriaProfile(ap.StyleHnRID);
                    if (sccp == null)
                    {
                        string msgText = MIDText.GetText(eMIDTextCode.msg_al_NoDefaultSizeCurveDefined);
                        msgText = msgText.Replace("{0}", ap.HeaderID);
                        SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msgText, this.GetType().Name);
                    }
                    else
                    {   // Begin TT#488 -RMatelic - Size Curve Node Properties default set at color header has 2 colors; issue error if header has  more than 1 color
                        //sizeCurveName = SAB.HierarchyServerSession.GetSizeCurveCriteriaProfileCurveName(hnpStyle, sccp);
                        if (OkayToProcessDefault(sccp, hdrColorCount, hnpStyle))
                        {
                            HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetDefaultSizeCurveCriteriaNode(hnpStyle, sccp, hdrColorCount);
                            if (hnp == null)
                            {
                                if (hdrColorCount == 0)
                                {
                                    string msgText = MIDText.GetText(eMIDTextCode.msg_al_NoDefaultSizeCurveNode);
                                    msgText = msgText.Replace("{0}", ap.HeaderID);
                                    SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msgText, this.GetType().Name);
                                }
                                else
                                {
                                    string msgText = MIDText.GetText(eMIDTextCode.msg_al_CannotProcessMultipleColors);
                                    msgText = msgText.Replace("{0}", ap.HeaderID);
                                    SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msgText, this.GetType().Name);
                                }
                            }
                            else
                            {
                                sizeCurveName = SAB.HierarchyServerSession.GetSizeCurveCriteriaProfileCurveName(hnp, sccp);
                            }
                        }
                        else
                        {
                            string msgText = MIDText.GetText(eMIDTextCode.msg_al_CannotProcessMultipleColors);
                            msgText = msgText.Replace("{0}", ap.HeaderID);
                            SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msgText, this.GetType().Name);
                        }
                    }   // End TT#488
                }
            }
            catch (Exception e)
			{
				this.SAB.ApplicationServerSession.Audit.Log_Exception(e,this.GetType().Name,eExceptionLogging.logAllInnerExceptions);
			}
            return sizeCurveName;
        }

        // Begin TT#488 - RMatelic - Size Curve Node Properties default set at color header has 2 colors; issue error 
        private bool OkayToProcessDefault(SizeCurveCriteriaProfile aSccp, int aHdrColorCount, HierarchyNodeProfile aNodeProf) 
        {
            bool okToProcessDefault = true;
            try
            {
                if (aHdrColorCount > 1)
                {
                    switch (aSccp.CriteriaLevelType)
                    {
                        case eLowLevelsType.HierarchyLevel:
                            HierarchyProfile hp = SAB.HierarchyServerSession.GetHierarchyData(aSccp.CriteriaLevelRID);
                            foreach (HierarchyLevelProfile hlp in hp.HierarchyLevels.Values)
                            {
                                if (hlp.Level == aSccp.CriteriaLevelSequence)
                                {
                                    if (hlp.LevelType == eHierarchyLevelType.Color)
                                    {
                                        okToProcessDefault = false;
                                    }
                                    break;
                                }
                            }
                            break;

                        case eLowLevelsType.LevelOffset:
                            NodeAncestorList nal = SAB.HierarchyServerSession.GetNodeAncestorList(aNodeProf.Key, aSccp.CriteriaLevelRID);
                            int offSet = (nal.ArrayList.Count - 1) - aSccp.CriteriaLevelOffset;
                            if (offSet >= 0)
                            {
                                NodeAncestorProfile nap = (NodeAncestorProfile)nal.ArrayList[offSet];
                                HierarchyNodeProfile hnp = GetNodeData(nap.Key);
                            }
                            else
                            {
                                okToProcessDefault = false;
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            catch
            {
                throw;
            }
            return okToProcessDefault;
        }
        // End TT#488
        private int GetColorCodeRID(AllocationProfile ap, ref int aHdrColorCount) 
        {
            int colorCodeRID = Include.NoRID;
            try
            {
                // Header has to contain exactly 1 color RID on bulk and packs to process color node; check both bulk colors and pack colors
                aHdrColorCount = ap.BulkColors.Count;
                if (ap.BulkColors.Count < 2) 
                {
                    ArrayList colorList = new ArrayList();
                    if (ap.BulkColors.Count == 1)
                    {
                        foreach (HdrColorBin colorBin in ap.BulkColors.Values)
                        {
                            colorList.Add(colorBin.ColorCodeRID);
                        }
                    }
                    
                    foreach (PackHdr ph in ap.Packs.Values)
                    {
                        foreach (PackColorSize color in ph.PackColors.Values)
                        {
                            if (color.ColorCodeRID > Include.DummyColorRID)
                            {
                                if (!colorList.Contains(color.ColorCodeRID))
                                {
                                    colorList.Add(color.ColorCodeRID);
                                    if (colorList.Count > 1)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        if (colorList.Count > 1)
                        {
                            aHdrColorCount = colorList.Count;
                            break;
                        }
                    }
                    if (colorList.Count == 1)
                    {
                        colorCodeRID = (int)colorList[0];
                    }
                }
            }
            catch
            {
                throw;
            }
            return colorCodeRID;
        }
        #endregion DefaultSizeCurve
        // End TT#438  

        //======================
		// Forecasting functions
		//======================

		public void ForecastingOverride_Add(int nodeRid, int versionRid)
		{
			if (_forecastingOverrideList == null)
				_forecastingOverrideList = new ArrayList();

			ForecastingOverride fo = new ForecastingOverride(nodeRid, versionRid);
			_forecastingOverrideList.Add(fo);
		}

		// Begin issue 4010 - stodd
		public void ForecastingOverride_Add(int aNodeRID, int aVersionRID, string aComputationMode, int aVariableNumber, 
			bool overrideBalance, bool balance)
		{
			if (_forecastingOverrideList == null)
				_forecastingOverrideList = new ArrayList();

			ForecastingOverride fo = new ForecastingOverride(aNodeRID, aVersionRID, aComputationMode, aVariableNumber, overrideBalance, balance);
			_forecastingOverrideList.Add(fo);
		}

		public void ForecastingOverride_Add(int aNodeRID, int aVersionRID, string aComputationMode, int aVariableNumber)
		{
			if (_forecastingOverrideList == null)
				_forecastingOverrideList = new ArrayList();

			ForecastingOverride fo = new ForecastingOverride(aNodeRID, aVersionRID, aComputationMode, aVariableNumber);
			_forecastingOverrideList.Add(fo);
		}
		// End Issue 4010
		public void ForecastingOverride_ClearAll()
		{
			if (_forecastingOverrideList == null)
				_forecastingOverrideList = new ArrayList();
			else
				_forecastingOverrideList.Clear();
		}

		public void BuildWaferColumnsAdd(int aWaferColumn, eAllocationWaferVariable aAllocationWaferVariable)
		{
			try
			{
				bool addEntry = true;
				// begin MID Track 4426 Need section goes to zero
				//foreach(BuildWaferColumn bwc in _buildWaferColumns)
				//{
				//	if (aWaferColumn == bwc.waferColumn &&
				//		aAllocationWaferVariable == bwc.AllocationWaferVariable)
				//	{
				//		addEntry = false;
				//		break;
				//	}
				//}
				//if (addEntry)
				//{
				//	_buildWaferColumns.Add (new BuildWaferColumn(aWaferColumn, aAllocationWaferVariable));
				//}
				if (this.AllocationViewType == eAllocationSelectionViewType.Size)
				{
					foreach (BuildWaferColumn bwc in _buildSizeWaferColumns)
					{
						if (aWaferColumn == bwc.waferColumn
							&& aAllocationWaferVariable == bwc.AllocationWaferVariable)
						{
							addEntry = false;
							break;
						}
					}
					if (addEntry)
					{
						_buildSizeWaferColumns.Add(new BuildWaferColumn(aWaferColumn, aAllocationWaferVariable));
					}
				}
				else
				{
					foreach (BuildWaferColumn bwc in _buildStyleWaferColumns)
					{
						if (aWaferColumn == bwc.waferColumn
							&& aAllocationWaferVariable == bwc.AllocationWaferVariable)
						{
							addEntry = false;
							break;
						}
					}
					if (addEntry)
					{
						_buildStyleWaferColumns.Add(new BuildWaferColumn(aWaferColumn, aAllocationWaferVariable));
					}
				}
				// end MID Track 4426 Need Section goes to zero
			}
			catch
			{
				throw;
			}
		}

		//BEGIN TT#572-MD - stodd - Assortment matrix changes are not being reflected on style review
		private void DebugHeaders()
		{
			AllocationProfileList apl = (AllocationProfileList)this.GetMasterProfileList(eProfileType.Allocation);
			Debug.WriteLine("Debug Assortment Headers");
			foreach (AllocationProfile ap in apl.ArrayList)
			{
				Debug.WriteLine(ap.HeaderID + " HDR TYPE " + ap.HeaderType + " QTY " + ap.TotalUnitsToAllocate + " ALLOC QTY " + ap.TotalUnitsAllocated);
				if (ap.Packs != null && ap.Packs.Count > 0)
				{
                    //int packType;  // TT#488 - MD - Jellis - Group Allocation (Field not used)
					foreach (PackHdr aPack in ap.Packs.Values)
					{
						Debug.WriteLine("  PACK " + aPack.PackName + " PACK MULT " + aPack.PackMultiple + " PACK QTY " + aPack.PacksToAllocate + " PACK ALLOC QTY " + aPack.PacksAllocated);

						if (aPack.PackColors != null && aPack.PackColors.Count > 0)
						{
							foreach (PackColorSize packColor in aPack.PackColors.Values)
							{
								Debug.WriteLine("    PACKCOLOR " + packColor.ColorName + " TY " + packColor.ColorUnitsInPack);
								if (SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
								{
									if (packColor.ColorSizes != null && packColor.ColorSizes.Count > 0)
									{


									}
								}
							}
						}
					}
				}

				if (ap.BulkColors != null && ap.BulkColors.Count > 0)
				{
					foreach (HdrColorBin aColor in ap.BulkColors.Values)
					{
						ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(aColor.ColorCodeRID);
						Debug.WriteLine("  COLOR " + aColor.ColorDescription + "QTY " + aColor.ColorUnitsToAllocate + " PACK ALLOC QTY " + aColor.ColorUnitsAllocated);

						if (SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
						{
							if (aColor.ColorSizes != null && aColor.ColorSizes.Count > 0)
							{

							}
						}
					}
				}
			}
		}
		//END TT#572-MD - stodd - Assortment matrix changes are not being reflected on style review
	}

    // begin TT#935 - MD - Jellis - Group Allocation Infrastructure Built Wrong
    // Removed "MasterProfileListChangeEvent"; put it in the profile List itself
    // end TT#935 - MD - Jellis - Group Allocation Infrastructure Built Wrong

	public class BuildWaferColumn
	{
		//=======
		// FIELDS
		//=======

		private int							_waferColumn;
		private eAllocationWaferVariable	_allocationWaferVariable;

        //=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of the BuildWaferColumn class.
		/// </summary>
		/// <param name="aWaferColumn">
		/// The column of the wafer.
		/// </param>
		/// <param name="aAllocationWaferVariable">
		/// The eAllocationWaferVariable of the variable in the wafer
		/// </param>

		public BuildWaferColumn(int aWaferColumn, eAllocationWaferVariable aAllocationWaferVariable)
		{
			_waferColumn = aWaferColumn;
			_allocationWaferVariable = aAllocationWaferVariable;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the column of the wafer.
		/// </summary>

		public int waferColumn
		{
			get
			{
				return _waferColumn;
			}
		}

		/// <summary>
		/// Gets the eAllocationWaferVariable of the variable.
		/// </summary>

		public eAllocationWaferVariable AllocationWaferVariable
		{
			get
			{
				return _allocationWaferVariable;
			}
		}

	}
}
