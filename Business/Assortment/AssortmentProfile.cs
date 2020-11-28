using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Data;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	public class AssortmentProfile : AllocationProfile, ICloneable
    {
        #region Fields
        //=======
		// FIELDS
		//=======

		//private int _storeFilterRid;	// TT#2 - stodd - assortment
		private double _assortReserveAmount;
		private eReserveType _assortReserveType;
		private int _assortStoreGroupRid;
		private eAssortmentVariableType _assortVariableType;
		private int _assortVariableNumber;
		private bool _assortInclOnhand;
		private bool _assortInclIntransit;
		private bool _assortInclSimStores;
		private bool _assortInclCommitted;	// TT#2 - stodd - assortment
		private eStoreAverageBy _assortAverageBy;
		//private int _genAssortMethodRid;	// TT#2 - stodd - assortment
		private eGradeBoundary _assortGradeBoundary;
		private int _assortCdrRid;		// TT#2 - stodd - assortment
		// Begin TT#1224 - stodd - committed
		private DayProfile _assortApplyToDay;
		// End TT#1224 - stodd - committed
		private int _assortAnchorNodeRid;		// TT#2 - stodd - assortment
		private int _assortUserRid;
		private DateTime _assortLastProcessDt;
        private int _assortBeginDayCdrRid;		// TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
        private DayProfile _assortBeginDay;     // TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working

		private List<AssortmentBasis> _basisList;
        //private ProfileList _storeGradeList;  // TT#488 - MD - Jellis - Group Allocation
		private ProfileList _assortmentStoreGradeList;	// TT#488-MD - STodd - Group Allocation 
		//private DataTable _dtTotalAssortment;
		private int _lastSglRidUsedInSummary = Include.NoRID;

		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		private AssortmentBasisReader _basisReader;
		//End TT#2 - JScott - Assortment Planning - Phase 2
		private AssortmentSummaryProfile _assortmentSummaryProfile;

        // Begin TT#1489 - RMatelic - Add Hierarchy levels above style as default displayable items
        private Hashtable _nodeAncestorList; 
        // End TT#1489
		
        //private ApplicationSessionTransaction _transaction; //TT#2505 - DOConnell - Placeholder values changing when combining assortments // TT#488 - Jellis - Group Allocation
        private bool _assortmentLoaded;                      // TT#488 - MD - Jellis - Group Allocation // TT#888 - MD - Jellis - Assortment/Group members not populated
        private bool _assortmentPacksLoaded;                // TT#1008 - MD - Jellis - Get Non Negative Message for various variables
        private bool _processingRules;                      // TT#1117 - MD- Jellis - Units Allocated less than min
        private bool _buildAssortmentProperties;          // TT#488 - MD - Jellis - Group Allocation
        private bool _buildingAssortmentProperties;       // TT#897 - MD - Jellis - Null Reference
        private bool _suspendAssortmentUpdates;            // TT#488 - MD - Jellis - Group Allocation
        private int _memberBulkSizeIntransitUpdated;       // TT#488 - MD - JEllis -  Group Allocation
        private int _memberBulkColorIntransitUpdated;      // TT#488 - MD - Jellis - Group Allocation
        private int _memberStyleIntransitUpdated;          // TT#488 - MD - Jellis - Group Allocation
        private int _memberColorReceiptsBalanceToBulk;     // TT#488 - MD - Jellis - Group Allocation
        private int _memberSizeReceiptsBalanceToColor;     // TT#488 - MD - Jellis - Group Allocation
        private int _memberBottomUpSizePerformed;          // TT#488 - MD - Jellis - Group ALlocation
        private int _memberRulesDefinedAndProcessed;       // TT#488 - MD - Jellis - Group Allocation
        private int _memberNeedAllocationPerformed;        // TT#488 - MD - Jellis - Group Allocation
        private int _memberPackBreakoutByContent;          // TT#488 - MD - Jellis - Group Allocation
        private int _memberBulkSizeBreakoutPerformed;      // TT#488 - MD - Jellis - Group Allocation
        private bool _processingGroupAllocation;            // TT#488 - MD - Jellis - Group Allocation
        private bool _processingActionOnHeaderInGroup;      // TT#1064 - MD - Jellis - Cannot Release Group Allocation
        private int _memberAllocationMultiple;              // TT#488 - MD - Jellis - Group Allocation   
        private int _memberGenericMultiple;                 // TT#488 - MD - Jellis - Group Allocation
        private int _memberDetailTypeMultiple;              // TT#488 - MD - Jellis - Group Allocation
        private int _memberBulkMultiple;                    // TT#488 - MD - Jellis - Group Allocation
        private int _memberReleaseApproved;                 // TT#1064 - MD - Jellis - Cannot release Group Allocation
        private int _memberReleased;                       // TT#1064 - MD - Jellis - Cannot release Group Allocation
        private int _memberShippingStarted;                 // TT#1064 - MD - Jellis - Cannot release Group Allocation
        private int _memberShippingComplete;                // TT#1064 - MD - Jellis - Cannot release Group Allocation
        private int _memberShippingOnHold;                  // TT#1064 - MD - Jellis - Cannot release Group Allocation
        private bool _allMembersHaveSameCapacityNode;       // TT#1148 - MD - Jellis - Group Allocation Enforces Capacity on wrong headers
        private bool _memberHeadersProcessingCompleted;			// TT#1184-MD - stodd - db timeout on cancel allocation
        private bool _memberPlaceholdersProcessingCompleted;	// TT#1184-MD - stodd - db timeout on cancel allocation
        private StoreVector _storeTotalMemberMaximum;           // TT#4208 - MD - Jellis - GA Velocity allocates less than Minimum

        private AllocationProfile[] _assortmentMembers; // TT#488 - MD - Jellis - Group Allocation // TT#891 - MD - Jellis - Group Allocation Need Action gets erro
        private AllocationProfile[] _placeHolders;      // TT#891 - MD - Jellis - Group Allocation Need Action gets error

        private AllocationProfile[] _sortedAssortmentMembers; // TT#488 - MD - Jellis - Group Allocation // TT#891 - MD - Jellis - Group Allocation Need Action gets erro

        private Dictionary<int, AllocationProfile> _assortmentPackHome; // TT#1008 - MD - Jellis - Get Non Negative Message for various variables
        // begin TT#1074 - MD - Jellis - Group ALlocation - Inventory Min Max Broken
        private bool _buildInventoryBasisAllocation;           
        private Dictionary<long, List<int>> _styleColorUpdatesInventoryXref;  // long = 2 int's: one Hdr Style; other 000 for total or ColorCodeRID for specific color; 
        private Dictionary<long, List<int>> _sizeUpdatesInventoryXref;        // long = 2 int's: one Hdr Style; other 000 for Total or ColorCodeRID for specific color
        private Dictionary<long, StoreVector> _inventoryBasisAllocation;      // long = 2 int's: one Inventory RID; other 000 for total or SizeCodeRID for specific size
        // end TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
        // begin TT#3941 - Urban - Jellis - Group Allocation Item/VSW split incorrect
        private bool _buildPriorItemUnitsManuallyAllocated;
        private Dictionary<long, StoreVector> _priorStyleColorItemUnitsManuallyAllocated;
        private Dictionary<long, Dictionary<int, StoreVector>> _priorSizeItemUnitsManuallyAllocated;
        // end TT#3941 - Urban - Jellis - Group Allocation Item/VSW split incorrect
        private Dictionary<int, string> _assortmentGradesByStore = null;		// TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working

		// Begin TT#1006 - md - stodd - GA Screen and Allocation WS after run Size Need status' are out of sync- 
        /// <summary>
        /// for Group Allocation indicates that action is available for placeholders
        /// </summary>
        private bool _placeholderAction;
        /// <summary>
        /// for Group Allocation indicates that a methed is processing the Group Allocation
        /// </summary>
        private bool _processingMethod;

        private eGroupAllocationProcessAs _groupAllocationProcessAs = eGroupAllocationProcessAs.Unknown; 	// TT#4364 - stodd - processing a "Cancel Allocation" action against an individual header within a GA replaces the plan level and grades. 


        // End TT#1006 - md - stodd - GA Screen and Allocation WS after run Size Need status' are out of sync- 

        private eAllocationMethodType _currentAction;  // TT#1723-MD - JSmith - Records written during cancel allocation
        private bool _setAllocationZero = false;  // TT#1772-MD - JSmith - GA-> ppk and bulk same style/color -> Velocity WOS , cannot balance remaining 14 units and headers have positive on pack and negative on bulk
        private bool _saveAssortmentMembers = false;  // TT#2109-MD - JSmith - Change the Beginning and Delivery Date in an Asst and the Delivery Date is incorrect.

        #endregion Fields

        #region Constructors
        //=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of the Assortment Profile
		/// </summary>
		/// <param name="aTransaction">Transaction associated with this profile.</param>
		/// <param name="aHeaderID">The user assigned name of the allocation header.</param>
		/// <param name="aKey">A unique integer identifier for the allocation header.</param>
		/// <param name="aSession">The session where the profile is being instantiated</param>
		/// <param name="aIncludeInSubtotals">True:  Header is included in grand and sub totals; False: Header is not included in grand and sub totals</param>
		/// <remarks>
		/// An Assortment profile describes the assortment information of an allocation header.
		/// </remarks>
		public AssortmentProfile(Transaction aTransaction, string aHeaderID, int aKey, Session aSession, bool aIncludeInSubtotals)
			: base(aTransaction, aHeaderID, aKey, aSession, aIncludeInSubtotals)
		{
			// Begin TT#952 - MD - stodd - add matrix to Group Allocation Review
			// Begin TT#1151-MD stodd - error removing headers with in a group allocation - 
            //if (base.PlanLevelStartHnRID == Include.NoRID)
            //{
                Fill(false);
            //}
            //else
            //{
            //    Fill(true);
            //}
			// End TT#1151-MD stodd - error removing headers with in a group allocation - 
			// End TT#952 - MD - stodd - add matrix to Group Allocation Review
		}
		/// <summary>
		/// Creates a new instance of the Assortment Profile
		/// </summary>
		/// <param name="aTransaction">Transaction associated with this profile.</param>
		/// <param name="aHeaderID">The user assigned name of the allocation header.</param>
		/// <param name="aKey">A unique integer identifier for the allocation header.</param>
		/// <param name="aSession">The session where the profile is being instantiated</param>
		/// <remarks>
		/// An Assortment profile describes the assortment information of an allocation header.
		/// </remarks>
		public AssortmentProfile(Transaction aTransaction, string aHeaderID, int aKey, Session aSession)
			: base(aTransaction, aHeaderID, aKey, aSession)
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            ////BEGIN TT#2505 - DOConnell - Placeholder values changing when combining assortments
            //if (aTransaction != null)
            //{
            //    _transaction = (ApplicationSessionTransaction)aTransaction;
            //}
            ////END TT#2505 - DOConnell - Placeholder values changing when combining assortments
            // end TT#488 - MD - Jellis - Group Allocation
			Fill(false);	// TT#1151-MD stodd - error removing headers with in a group allocation - 
		}
		/// <summary>
		/// Creates a new instance of the Assortment Profile
		/// </summary>
		/// <param name="aSAB">SessionAddressBlock associated with this profile.</param>
		/// <param name="aHeaderID">The user assigned name of the allocation header.</param>
		/// <param name="aKey">A unique integer identifier for the allocation header.</param>
		/// <param name="aSession">The session where the profile is being instantiated</param>
		/// <remarks>
		/// An Assortment profile describes the assortment information of an allocation header.
		/// </remarks>
		public AssortmentProfile(SessionAddressBlock aSAB, string aHeaderID, int aKey, Session aSession)
			: base(aSAB, aHeaderID, aKey, aSession)
		{
			Fill(false);	// TT#1151-MD stodd - error removing headers with in a group allocation - 
		}
		/// <summary>
		/// Creates a new instance of the Assortment Profile
		/// </summary>
		/// <param name="aSAB">SessionAddressBlock associated with this profile.</param>
		/// <param name="aHeaderDataRow">A data row from header table of database</param>
		/// <param name="aSession">The session where the profile is being instantiated</param>
		/// <remarks>
		/// An Assortment profile describes the assortment information of an allocation header.
		/// </remarks>
		public AssortmentProfile(SessionAddressBlock aSAB, System.Data.DataRow aHeaderDataRow, Session aSession)
			: base(aSAB, aHeaderDataRow, aSession)
		{
			Fill(false);	// TT#1151-MD stodd - error removing headers with in a group allocation - 
		}
		/// <summary>
		/// Creates a new instance of the Assortment Profile
		/// </summary>
		/// <param name="aHeaderDataRow">A data row from header table of database</param>
		/// <param name="aSession">The session where the profile is being instantiated</param>
		/// <remarks>
		/// An Assortment profile describes the assortment information of an allocation header.
		/// </remarks>
		public AssortmentProfile(Transaction aTransaction, System.Data.DataRow aHeaderDataRow, Session aSession, bool buildSummary, bool forUpdate)	// TT#1183 - stodd - assortment
			: base(aTransaction, aHeaderDataRow, aSession, forUpdate)
		{
			Fill(buildSummary);	// TT#1183 - stodd - assortment
		}
		/// <summary>
		/// Creates a new instance of the Assortment Profile
		/// </summary>
		/// <param name="aSAB">SessionAddressBlock associated with this profile.</param>
		/// <param name="aAllocationHeaderProfile">An allocation header profile that describes the header.</param>
		/// <param name="aSession">The session where the profile is being instantiated</param>
		/// <remarks>
		/// An Assortment profile describes the assortment information of an allocation header.
		/// </remarks>
		public AssortmentProfile(SessionAddressBlock aSAB, AllocationHeaderProfile aAllocationHeaderProfile, Session aSession)
			: base(aSAB, aAllocationHeaderProfile, aSession)
		{
			Fill(false);	// TT#1151-MD stodd - error removing headers with in a group allocation - 
		}

		// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
		public AssortmentProfile(Transaction aTransaction, string aHeaderID, int aKey, Session aSession, bool buildSummary, bool notUsed)
			: base(aTransaction, aHeaderID, aKey, aSession)
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            ////BEGIN TT#2505 - DOConnell - Placeholder values changing when combining assortments
            //if (aTransaction != null)
            //{
            //    _transaction = (ApplicationSessionTransaction)aTransaction;
            //}
            ////END TT#2505 - DOConnell - Placeholder values changing when combining assortments
            // end TT#488 - MD - Jellis - Group Allocation
			Fill(buildSummary);
		}
        // END TT#773-MD - Stodd - replace hashtable with dictionary
        #endregion Constructors

        #region Properties
        //===========
		// PROPERTIES
        //===========

         /// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Assortment;
			}
		}

        #region Assortment Properties
        // begin TT#488 - MD - Jellis - Group Allocation
        //public ProfileList StoreGradeList
        //{
        //    get
        //    {
        //        if (_storeGradeList == null)
        //            _storeGradeList = new ProfileList(eProfileType.StoreGrade);
        //        return _storeGradeList;
        //    }
        //    set { _storeGradeList = value; }
        //}
        // end TT#488 - MD - Jellis - Group Allocation

		// BEGIN TT#488-MD - STodd - Group Allocation  
		public ProfileList AssortmentStoreGradeList
		{
			get
			{
				if (_assortmentStoreGradeList == null)
					_assortmentStoreGradeList = new ProfileList(eProfileType.StoreGrade);
				return _assortmentStoreGradeList;
			}
			set { _assortmentStoreGradeList = value; }
		}
		// END TT#488-MD - STodd - Group Allocation 

        // Begin TT#2109-MD - JSmith - Change the Beginning and Delivery Date in an Asst and the Delivery Date is incorrect.
        public bool SaveAssortmentMembers
        {
            get { return _saveAssortmentMembers; }
            set { _saveAssortmentMembers = value; }
        }
        // End TT#2109-MD - JSmith - Change the Beginning and Delivery Date in an Asst and the Delivery Date is incorrect.

        // Begin TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO
        public int LastSglRidUsedInSummary
        {
            get { return _lastSglRidUsedInSummary; }
        }
        // End TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO
        
		public eAssortmentVariableType AssortmentVariableType
		{
			get
			{
				return _assortVariableType;
			}
			set { _assortVariableType = value; }
		}

		public int AssortmentStoreGroupRID
		{
			get
			{
				return _assortStoreGroupRid;
			}
			set { _assortStoreGroupRid = value; }
		}

		/// <summary>
		/// Gets or set the Assortment Variable Number which is set by the general assortment method.
		/// </summary>
		public int AssortmentVariableNumber
		{
			get { return _assortVariableNumber; }
			set { _assortVariableNumber = value; }
		}
		// Begin  TT#2 - stodd - assortment
		//public int StoreFilterRid
		//{
		//    get { return _storeFilterRid; }
		//    //set { _storeFilterRid = value; }
		//}
		// End TT#2 - stodd - assortment
		public double AssortmentReserveAmount
		{
			get { return _assortReserveAmount; }
			set { _assortReserveAmount = value; }
		}

		public eReserveType AssortmentReserveType
		{
			get { return _assortReserveType; }
			set { _assortReserveType = value; }
		}

		public bool AssortmentIncludeOnhand
		{
            // Begin TT#2076-MD - JSmith - GA_process GA Method_ Pull up Style Reveiw_Need is 100% for all Stores.  Need not cacling correctly
			//get { return _assortInclOnhand; }
			get 
            {
                // Begin TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used. 
                //if (AsrtType == (int)eAssortmentType.GroupAllocation)
                //{
                //    return true;
                //}
                
                //return _assortInclOnhand;
                return true;
                // End TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
            }
            // End TT#2076-MD - JSmith - GA_process GA Method_ Pull up Style Reveiw_Need is 100% for all Stores.  Need not cacling correctly
            set { _assortInclOnhand = value; }
		}

		public bool AssortmentIncludeIntransit
		{
            // Begin TT#2076-MD - JSmith - GA_process GA Method_ Pull up Style Reveiw_Need is 100% for all Stores.  Need not cacling correctly
			//get { return _assortInclIntransit; }
			get 
            {
                // Begin TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
                //if (AsrtType == (int)eAssortmentType.GroupAllocation)
                //{
                //    return true;
                //}
                //return _assortInclIntransit;
                return true;
                // End TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
            }
            // End TT#2076-MD - JSmith - GA_process GA Method_ Pull up Style Reveiw_Need is 100% for all Stores.  Need not cacling correctly
			set { _assortInclIntransit = value; }
		}

		public bool AssortmentIncludeSimilarStores
		{
			get { return _assortInclSimStores; }
			set { _assortInclSimStores = value; }
		}
		// Begin  TT#2 - stodd - assortment
		public bool AssortmentIncludeCommitted
		{
			get { return _assortInclCommitted; }
			set { _assortInclCommitted = value; }
		}
		// End  TT#2 - stodd - assortment
		public eStoreAverageBy AssortmentAverageBy
		{
			get { return _assortAverageBy; }
			set { _assortAverageBy = value; }
		}
		// Begin  TT#2 - stodd - assortment
		//public int AssortmentMethodRid
		//{
		//    get { return _genAssortMethodRid; }
		//    //set { _genAssortMethodRid = value; }
		//}
		// End  TT#2 - stodd - assortment
		public eGradeBoundary AssortmentGradeBoundary
		{
			get { return _assortGradeBoundary; }
			set { _assortGradeBoundary = value; }
		}
		// Begin  TT#2 - stodd - assortment
		public int AssortmentCalendarDateRangeRid
		{
			get { return _assortCdrRid; }
			set 
			{ 
				_assortCdrRid = value;
				_assortApplyToDay = null;	// TT#1224 - stodd - committed
			}
		}
		// Begin TT#1224 - stodd - committed
		public DayProfile AssortmentApplyToDate
		{
			get 
			{
				if (_assortApplyToDay == null)
				{
					_assortApplyToDay = ((DayProfile)(this.AppSessionTransaction.SAB.ApplicationServerSession.Calendar.GetFirstDayOfRange(_assortCdrRid)));

				}
				return _assortApplyToDay; 
			}
			set { _assortApplyToDay = value; }	// TT#488-MD - Stodd - Group Allocation
		}
		// End TT#1224 - stodd - committed

        // Begin TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
        public int AssortmentBeginDayCalendarDateRangeRid
        {
            get { return _assortBeginDayCdrRid; }
            set
            {
                _assortBeginDayCdrRid = value;
                _assortBeginDay = null;
            }
        }
        
        public DayProfile AssortmentBeginDay
        {
            get
            {
                if (_assortBeginDay == null)
                {
                    if (_assortBeginDayCdrRid > Include.UndefinedCalendarDateRange)
                    {
                        _assortBeginDay = ((DayProfile)(this.AppSessionTransaction.SAB.ApplicationServerSession.Calendar.GetFirstDayOfRange(_assortBeginDayCdrRid)));
                    }
                    else
                    {
                        _assortBeginDay = new DayProfile(0);
                    }
                }
                return _assortBeginDay;
            }
            set { _assortBeginDay = value; }
        }
        // End TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working

		public int AssortmentAnchorNodeRid
		{
			get { return _assortAnchorNodeRid; }
			set { _assortAnchorNodeRid = value; }
		}
		// End  TT#2 - stodd - assortment
		public List<AssortmentBasis> AssortmentBasisList
		{
			get { return _basisList; }
			set { _basisList = value; }
		}
		// Begin  TT#2 - stodd - assortment
		public int AssortmentUserRid
		{
			get { return _assortUserRid; }
			set { _assortUserRid = value; }
		}

		public DateTime AssortmentLastProcessDt
		{
			get { return _assortLastProcessDt; }
			set { _assortLastProcessDt = value; }
		}

		public string Name
		{
			get { return HeaderID; }
			set { HeaderID = value; }
		}

		public AssortmentSummaryProfile AssortmentSummaryProfile
		{
			get { return _assortmentSummaryProfile; }
			set { _assortmentSummaryProfile = value; }
		}

		
		// End  TT#2 - stodd - assortment
		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		public AssortmentBasisReader BasisReader
		{
			get
			{
				if (_basisReader == null)
				{
					// Begin TT#1188 - stodd - reader not using anchor date
					// Begin TT#3815 - Set totals in group allocation summary area of matrix grid are missing after running Group Allocation Method - 
                    // Begin TT#1239-MD - stodd - Get Null Reference when opening Assortment from Assortment Properties window
                    if (AsrtType == (int)eAssortmentType.GroupAllocation)
                    {
                        _basisReader = new AssortmentBasisReader(
                        SAB, AppSessionTransaction, _assortAnchorNodeRid, _assortCdrRid,
                        _basisList, AppSessionTransaction.CurrentStoreGroupProfile.Key, _assortInclSimStores,
                        _assortInclIntransit, _assortInclOnhand, _assortInclCommitted,
                        StoreMgmt.StoreProfiles_GetActiveStoresList()); //SAB.StoreServerSession.GetActiveStoresList());
                    }
                    else
                    {
                        _basisReader = new AssortmentBasisReader(
                            SAB, AppSessionTransaction, _assortAnchorNodeRid, _assortCdrRid,
                            _basisList, _assortStoreGroupRid, _assortInclSimStores,
                            _assortInclIntransit, _assortInclOnhand, _assortInclCommitted,
                            StoreMgmt.StoreProfiles_GetActiveStoresList()); //SAB.StoreServerSession.GetActiveStoresList());
                    }
                    // End TT#1239-MD - stodd - Get Null Reference when opening Assortment from Assortment Properties window
					// End TT#3815 - Set totals in group allocation summary area of matrix grid are missing after running Group Allocation Method - 
					// End TT#1188 - stodd - reader not using anchor date
				}

				return _basisReader;
			}
			// Begin TT#2 - stodd - so it can be set to null and refreshed.
			set { _basisReader = value; }
			// End TT#2 - stodd
		}
		//End TT#2 - JScott - Assortment Planning - Phase 2

        // begin TT#488 - MD - Jellis - Group Assortment
        public AllocationProfile[] AssortmentMembers  // TT#891 - MD - Jellis - Group Allocation Need Error
        {
            get
            {
			    // begin TT#946 - MD - Jellis - Group Allocation Not Working 
                //// begin TT#927 - MD - Jellis - Allocations disappear from Style Review when coming from Group Allocation
                //if (_buildingAssortmentProperties)
                //{
                //    return new AllocationProfile[0];
                //}
                // end TT#927 - MD - Jellis - Allocations disappear from Style Review when coming from Group Allocation
                // end TT#946 - MD - Jellis - Group Allocation Not Working
				if (_assortmentMembers == null)
                {
                    _assortmentPackHome = null;             // TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                    _assortmentPacksLoaded = false;         // TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                    if (!_assortmentLoaded)  // TT#888 - MD - Jellis - Assortment/Group members not populated
                    {
                        // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong (following 2 arrays must be null to trigger building member lists correctly)
                        //// Begin TT#890 - MD - stodd - null ref opening assortment - 
                        //_sortedAssortmentMembers = new AllocationProfile[0];
                        //_placeHolders = new AllocationProfile[0];
                        //// End TT#890 - MD - stodd - null ref opening assortment 
                        // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong 
                        return new AllocationProfile[0]; // TT#891 - MD - Jellis - Group Allocation Need Error
                    }
                    // Begin TT#1516-MD - stodd - Decreasing the Quantity on a header attached to an Assortment causes a "Quantity to allocate cannot be less than zero" error
                    // begin TT#910 - MD - Jellis - GROUP ALLOCATION STOPPED WORKING
                    //else if (AsrtType != (int)eAssortmentType.GroupAllocation)
                    //{
                    //    // Begin TT#890 - MD - stodd - null ref opening assortment - 
                    //    _sortedAssortmentMembers = new AllocationProfile[0]; 
                    //    _assortmentMembers = new AllocationProfile[0];
                    //    _placeHolders = new AllocationProfile[0];
                    //    // End TT#890 - MD - stodd - null ref opening assortment - 
                    //}
                    // End TT#1516-MD - stodd - Decreasing the Quantity on a header attached to an Assortment causes a "Quantity to allocate cannot be less than zero" error
                    else	// TT#890 - MD - stodd - null ref opening assortment - add "else"
                    {
                        // end TT#910 - MD - Jellis - GROUP ALLOCATION STOPPED WORKING

                        _sortedAssortmentMembers = null; // TT#488 - MD - JEllis - Group Allocation // TT#891 - MD - Jellis - Group Allocation Need Action Gets error
                        List<AllocationProfile> assortmentMembers = new List<AllocationProfile>();  // TT#891 - MD - Jellis - Group Allocation Need Action Gets error
                        List<AllocationProfile> placeHolders = new List<AllocationProfile>();       // TT#891 - MD - Jellis - Group Allocation Need Action Gets error
                        if (base.AppSessionTransaction != null)
                        {
                            //AllocationProfileList apl = (AllocationProfileList)base.AppSessionTransaction.GetMasterProfileList(eProfileType.Allocation); // TT#488 - MD - Jellis - Group Allocation
                            AllocationProfileList apl = (AllocationProfileList)base.AppSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember); // TT#488 - MD - Jellis - Group Allocation
                            if (apl != null)
                            {
                                apl.ProfileListChangeEvent.OnProfileListChangeHandler += new ProfileListChangeEvent.ProfileListChangeEventHandler(OnMasterProfileChange);
                                //foreach (AllocationProfile ap in base.AppSessionTransaction.GetMasterProfileList(eProfileType.Allocation)) // TT#488 - MD - JEllis - Group Allocation
                                foreach (AllocationProfile ap in apl)       // TT#488 - MD - Jellis - Group Allocation
                                {
                                    if (this.Key == ap.AsrtRID)
                                    {
                                        // begin TT#910 - MD - Jellis - Group Allocaton STOPPED WORKING
                                        //if (ap.HeaderType != eHeaderType.Assortment                   // TT#488 - Group Allocation - Remove Placeholder from Group
                                        //    && ap.AsrtType == (int)eAssortmentType.GroupAllocation) // TT#890 - DOConnell - Group Allocation - Null Reference Exception when opening an Assortment
                                        if (ap.HeaderType != eHeaderType.Assortment)
                                        // end TT#910 - MD - Jellis - Group Allocation STOPPED WORKING
                                        {
                                            if (InstanceID != ap.AssortmentInstanceID)
                                            {
                                                throw new Exception(
                                                    "Assortment Instances out of sync: Assortment Instance ID = ["
                                                    + InstanceID.ToString()
                                                    + "];  Member Assortment Instance ID = ["
                                                    + ap.AssortmentInstanceID.ToString()
                                                    + "]");
                                            }

                                            // TT#488 - Group Allocation - Remove Placeholder from Group
                                            //if (this.AsrtType == (int)eAssortmentType.PostReceipt) // TT#488 - Group Allocation - Remove Placeholder from Group
                                            if (AsrtType == (int)eAssortmentType.GroupAllocation)    // TT#488 - Group Allocation - Remove Placeholder from Group
                                            {
                                                if (ap.HeaderType != eHeaderType.Placeholder)
                                                {
                                                    assortmentMembers.Add(ap); // TT#891 - MD - Jellis - Group Allocation Need Action Gets error
                                                }
                                                // begin TT#891 - MD - Jellis - Group ALlocation Need Action Gets Error
                                                else
                                                {
                                                    placeHolders.Add(ap);
                                                }
                                                // end TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                                            }
                                            else
                                            {
                                                assortmentMembers.Add(ap); // TT#891 - MD - Jellis - Group Allocation Need Action Gets error
                                                // begin TT#891 - MD - Jellis - Group Allocaton Need Action Gets Error
                                                if (ap.HeaderType == eHeaderType.Placeholder)
                                                {
                                                    placeHolders.Add(ap);
                                                }
                                                // end TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                                            }
                                        } // TT#488 - Group Allocation - Remove Placeholder from Group
                                    }
                                }
                            }
                        }
                        // begin TT#891 - Group Allocation Need Gets Error
                        _assortmentMembers = new AllocationProfile[assortmentMembers.Count];
                        _placeHolders = new AllocationProfile[placeHolders.Count];
                        assortmentMembers.CopyTo(_assortmentMembers);
                        placeHolders.CopyTo(_placeHolders);
                        // end TT#891 - Group Allocation Need Gets Error
                    }		// TT#890 - MD - stodd - null ref opening assortment - for add "else"
               }
                return _assortmentMembers;
            }
        }
        public AllocationProfile[] AssortmentMembersSorted
        {
            get
            {
      		    // begin TT#946 - MD - Jellis - Group Allocation Not Working 
                //// begin TT#927 - MD - Jellis - Allocations disappear from Style Review when coming from Group Allocation
                //if (_buildingAssortmentProperties)
                //{
                //    return new AllocationProfile[0];
                //}
                // end TT#927 - MD - Jellis - Allocations disappear from Style Review when coming from Group Allocation
                // end TT#946 - MD - Jellis - Group Allocation Not Working

                if (_sortedAssortmentMembers == null)
                {
                    AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets error
                    if (apList.Length == 0) // TT#891 - MD - Jellis - Group Allocation Need Action Gets error
                    {
                        return apList;
                    }
                    _sortedAssortmentMembers = new AllocationProfile[_assortmentMembers.Length]; // TT#891 - MD - Jellis - Group Allocation Need Action Gets error
                    MIDGenericSortItem[] mgsi = new MIDGenericSortItem[apList.Length]; // TT#891 - MD - Jellis - Group Allocation Need Action Gets error
                    int i = 0;

                    foreach (AllocationProfile ap in apList)
                    {
                        mgsi[i] = new MIDGenericSortItem();
                        mgsi[i].Item = i;
                        mgsi[i].SortKey = new double[2]; 
                        mgsi[i].SortKey[0] = ap.TotalUnitsToAllocate;
                        mgsi[i].SortKey[1] = AppSessionTransaction.GetRandomDouble();
                        i++;
                    }
                    //Array.Sort(mgsi, new MIDGenericSortDescendingComparer()); // TT#1143 - MD - Jellis - Group Allocation Min Broken
                    Array.Sort(mgsi, new MIDGenericSortAscendingComparer());    // TT#1143 - MD - Jellis - Group Allocation Min Broken
                    i = 0; // TT#891 - MD - Jellis - Group Allocation Need Action Gets error
                    foreach (MIDGenericSortItem sortItem in mgsi)
                    {
                        _sortedAssortmentMembers[i] = apList[sortItem.Item]; // TT#891 - MD - Jellis - Group Allocation Need Action Gets error
                        i++; // TT#891 - MD - Jellis - Group Allocation Need Action Gets error
                    }                    
                }
                return _sortedAssortmentMembers;
            }
        }
        // end TT#488 - MD - Jellis - Group Assortment

        // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
        public AllocationProfile[] AssortmentPlaceHolders
        {
            get
            {
                AllocationProfile[] apList = AssortmentMembers;
                if (_placeHolders == null)
                {
                    return new AllocationProfile[0];
                }
                return _placeHolders;
            }
        }
        // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong

        // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
        /// <summary>
        /// Gets or sets BuildInventoryBasisAllocation flag
        /// </summary>
        public bool BuildInventoryBasisAllocation
        {
            get { return _buildInventoryBasisAllocation; }
            set { _buildInventoryBasisAllocation = value; }
        }
        // end TT#1074 - MD - Jellis - Group Alllocation Inventory Min Max Broken
        // begin TT#3941 - Urban - Jellis - Group Allocation Item/VSW split incorrect
        public bool BuildItemUnitsManuallyAllocated
        {
            get { return _buildPriorItemUnitsManuallyAllocated; }
            set { _buildPriorItemUnitsManuallyAllocated = value; }
        }
        // end TT#3941 - Urban - Jellis - Group Allocation Item/VSW split incorrect

        // begin TT#1063 - MD - Jellis - Group Allocation non-size Results change after Size Need
        /// <summary>
        /// Gets or sets BuildAssortmentProperties flag
        /// </summary>
        public bool BuildAssortmentProperties
        {
            get { return _buildAssortmentProperties; }
            set
            {
                if (!SuspendAssortmentUpdates)
                {
                    _buildAssortmentProperties = value;
                }
            }
        }

        // begin TT#897 - MD - Jellis - Null Reference
        public bool BuildingAssortmentProperties
        {
            get { return _buildingAssortmentProperties; }
            set { _buildingAssortmentProperties = value; }
        }
        // end TT#897 - MD - Jellis - Null Reference

        /// <summary>
        /// Gets or sets SuspendAssortmentProperites flag (when true changes to Rebuild flag not honored) 
        /// </summary>
        public bool SuspendAssortmentUpdates
        {
            get { return _suspendAssortmentUpdates; }
            set { _suspendAssortmentUpdates = value; }
        }
        // end TT#1063 - MD - Jellis - Group Allocation non-size Results change after Size Need

        // begin TT#1117 - MD - Jellis - Units Allocated Less than Min
        public bool ProcessingRules  // TT#1566 - MD - Jellis - GA Min Not honored when Header min less than GA MIN
        {
            get { return _processingRules; }
            set { _processingRules = value; }
        }
        // end TT#1117 - MD - Jellis - Units Allocated Less than Min

		// Begin TT#4364 - stodd - processing a "Cancel Allocation" action against an individual header within a GA replaces the plan level and grades.
        public eGroupAllocationProcessAs GroupAllocationProcessAs
        {
            get { return _groupAllocationProcessAs; }
            set { _groupAllocationProcessAs = value; }
        }
		// End TT#4364 - stodd - processing a "Cancel Allocation" action against an individual header within a GA replaces the plan level and grades.
		
        #endregion Assortment Properties

        // begin TT#488 - MD - Jellis - Group Allocation
        #region Allocation Profile Property Overrides
        /// <summary>
		/// Gets StyleHnRID
		/// </summary>
		/// <remarks>
		/// Required style RID from the product heirarchy.
		/// </remarks>
		override public int StyleHnRID
		{
			get
			{
                if (base.StyleHnRID == Include.NoRID)
                {
                    if (_assortmentLoaded // TT#888 - MD - Jellis - Assortment/Group members not populated
                        && AssortmentMembers.Length > 0)
                    {
                        base.StyleHnRID = AssortmentMembers[0].StyleHnRID;
                    }
                }
                return base.StyleHnRID;
			}
            set
            {
                base.StyleHnRID = value;
            }
		}


        // Moved non-allocation profile properties to assortment properties region // TT#1063 - MD - Jellis - Group Allocation non-size Results change after Size Need

        // begin TT#1114 - MD - Jellis - Need Analysis Not showing
        /// <summary>
		/// Gets or sets GradeWeekCount
		/// </summary>
        virtual public int GradeWeekCount 
        {
            get
            {
                return base.GradeWeekCount;
            }
            set
            {
                AllocationProfile[] apList = AssortmentMembers;
                foreach (AllocationProfile ap in apList)
                {
                    ap.GradeWeekCount = value;
                }
                base.GradeWeekCount = value;
            }
        }
        // end TT#1114 - MD - Jellis - Need Analysis Not showing

        /// <summary>
        /// Gets or sets ReserveUnits
        /// </summary>
        override public int ReserveUnits
        {
            get
            {
                // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong 
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
                return base.ReserveUnits;
            }
            set
            {
                if (value <0)
                {
                    throw new MIDException(eErrorLevel.warning,
						(int)eMIDTextCode.msg_ReserveQtyCannotBeNeg,
						Session.Audit.GetText(eMIDTextCode.msg_ReserveQtyCannotBeNeg, false));
                }
                if (value != ReserveUnits)
                {
                    try
                    {

                        AllocationProfile[] sapList = AssortmentMembersSorted;
                        SuspendAssortmentUpdates = true;
                        int totalReserve = value;
                        int totalBasis = 0;
                        foreach (AllocationProfile ap in sapList)
                        {
                            totalBasis = +ap.TotalUnitsToAllocate;
                        }
                        foreach (AllocationProfile ap in sapList)
                        {
                            ap.ReserveUnits =
                                (int)((double)(((double)totalReserve
                                       * (double)ap.TotalUnitsToAllocate)
                                       / (double)totalBasis) + .5d);
                            totalBasis = -ap.TotalUnitsToAllocate;
                            totalReserve = -ap.ReserveUnits;
                        }
                        base.ReserveUnits = totalReserve; // sets base to total reserve units on the database
                    }
                    finally
                    {
                        SuspendAssortmentUpdates = false;
                    }
                }

            }
        }

        // begin TT#935 - MD - Jellis - Group Allocation infrastructure built wrong
        /// <summary>
        /// Gets or sets AllocatedUnits.
        /// </summary>
        override public int AllocatedUnits 
        {
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                return base.AllocatedUnits;
            }

            set
            {
                base.AllocatedUnits = value;
            }
        }
        // end TT#935 - MD- Jellis - Group Allocation infrastructure built wrong

        /// <summary>
        /// Gets Item Allocated Units
        /// </summary>
        override public int ItemAllocatedUnits
        {
            get
            {
                if (AppSessionTransaction.GetCalcAssortmentItemVsw(BuildItemKey))
                {
                    AssortmentPropertiesRebuildItemVsw();
                }
                return base.ItemAllocatedUnits;
            }
        }
        /// <summary>
        /// Gets IMO Allocated Units
        /// </summary>
        override public int ImoAllocatedUnits
        {
            get
            {
                int imoAllocatedUnits =
                    AllocatedUnits
                    - ItemAllocatedUnits;
                if (imoAllocatedUnits < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)
                        + ": ImoAllocatedUnits in " + GetType().Name);
                }
                return imoAllocatedUnits;
            }
        }
        // begin  TT#935 - MD - Jellis - Group Allocation infrastructure built wrong 		
        /// <summary>
        /// Gets or sets OrigAllocatedUnits.
        /// </summary>
        override public int OrigAllocatedUnits  // TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
        {
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                return base.OrigAllocatedUnits;
            }

            set
            {
                base.OrigAllocatedUnits = value;
            }
        }
        // end TT#935 - MD - Jellis - Group Allocation infrastructure built wrong

        /// <summary>
        /// Gets  RsvAllocatedUnits.
        /// </summary>
        override public int RsvAllocatedUnits
        {
            get
            {
                // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong 
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
                return base.RsvAllocatedUnits;
            }
            set
            {
                if (value <0)
                {
                    throw new MIDException(eErrorLevel.warning,
						(int)eMIDTextCode.msg_al_RsvAloctdQtyCannotBeNeg,
						Session.Audit.GetText(eMIDTextCode.msg_al_RsvAloctdQtyCannotBeNeg, false)); 
                }
                if (value != RsvAllocatedUnits)
                {
                    try
                    {
                        SuspendAssortmentUpdates = true;
                        AllocationProfile[] sapList = AssortmentMembersSorted; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                        int totalReserve = value;
                        int totalBasis = 0;
                        foreach (AllocationProfile ap in sapList)
                        {
                            totalBasis = +ap.TotalUnitsToAllocate;
                        }
                        foreach (AllocationProfile ap in sapList)
                        {
                            ap.RsvAllocatedUnits =
                                (int)((double)(((double)totalReserve
                                       * (double)ap.TotalUnitsToAllocate)
                                       / (double)totalBasis) + .5d);
                            totalBasis = -ap.TotalUnitsToAllocate;
                            totalReserve = -ap.RsvAllocatedUnits;
                        }
                        totalReserve = base.RsvAllocatedUnits; // set value on database
                    }
                    finally
                    {
                        SuspendAssortmentUpdates = false;
                    }
                }
            }
        }
        /// <summary>
        /// Gets or sets percent need limit.
        /// </summary>
        override public double PercentNeedLimit
        {
            get
            {
                return base.PercentNeedLimit;
            }
            set
            {
                base.PercentNeedLimit = value;
                AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                foreach (AllocationProfile ap in apList)
                {
                    ap.PercentNeedLimit = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the Begin Day of the Shipping Horizon.
        /// </summary>
        /// <remarks>
        /// <para>Optional, user-specified start date of the Shipping Horizon.</para>
        /// <para>When specified, the allocation header is assumed to be a 
        /// future shipment whose allocation is based on having already achieved
        /// the planned inventory level at this date (a substitute for onhand).
        /// </para><para>
        /// When not specified, the allocation header is assumed to be a "current"
        /// shipment whose allocation is based on the "current" onhands in the stores.</para>
        /// </remarks>
        override public DateTime BeginDay
        {
            get
            {
                return base.BeginDay;
            }
            set
            {
                base.BeginDay = value;
                AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                foreach (AllocationProfile ap in apList)
                {
                    ap.BeginDay = value;
                }
            }
        }

        override public bool BeginDayIsSet
        {
            get
            {
                return base.BeginDayIsSet;
            }
            set
            {
                base.BeginDayIsSet = value;
                AllocationProfile[] apList = AssortmentMembers;
                foreach (AllocationProfile ap in apList)
                {
                    ap.BeginDayIsSet = value;
                }
            }
        }

        // Begin TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO
		public void ResetAssortmentStoreShipDates()
        {
            base.ResetStoreShipDates();
        }
		// End TT#2116-MD - JSmith - Change the Basis Weeks and the Matrix Basis values do not recalc.  Should do an implicit REDO

        /// <summary>
        /// Gets or sets the Ship Day for the Shipping Horizon.
        /// </summary>
        /// <remarks>
        /// <para>Optional, user-specified ending or target date for the shipping horizon.</para>
        /// <para>When specified, all stores use the same ship day.</para>
        /// <para>When not specified, every store's ship day is dependent on its lead time and picking-shipping schedule.</para>
        /// </remarks>
        override public DateTime ShipToDay
        {
            get
            {
                return base.ShipToDay;
            }
            set
            {
                base.ShipToDay = value;
                AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                foreach (AllocationProfile ap in apList)
                {
                    ap.ShipToDay = value;
                }
            }
        }

        override public bool ShipToDayIsSet
        {
            get
            {
                return base.ShipToDayIsSet;
            }
            set
            {
                base.ShipToDayIsSet = value;
                AllocationProfile[] apList = AssortmentMembers;
                foreach (AllocationProfile ap in apList)
                {
                    ap.ShipToDayIsSet = value;
                }
            }
        }

        /// <summary>
		/// Gets the Earliest Ship Day.
		/// </summary>
		override public DateTime EarliestShipToDay
		{
			get
			{
                AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                if (apList.Length > 0) // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                {
                    DateTime earliestShipDay = apList[0].EarliestShipToDay;
                    foreach (AllocationProfile ap in apList)
                    {
                        if (ap.EarliestShipToDay != Include.UndefinedDate)
                        {
                            if (ap.EarliestShipToDay < earliestShipDay
                                || earliestShipDay == Include.UndefinedDate)
                            {
                                earliestShipDay = ap.EarliestShipToDay;
                            }
                        }
                    }
                    return earliestShipDay;
                }
                else
                {
                    return base.EarliestShipToDay;
                }
			}
		}

        /// <summary>
		/// Gets or sets BottomUpSizePerformed flag.
		/// </summary>
		override public bool BottomUpSizePerformed
		{
			get
			{
                return (base.BottomUpSizePerformed || MemberBottomUpSizePerformed > 0);  // TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
			}
            set
            {
                // begin TT#1021 - MD - Jellis - Header Status Wrong
                AllocationProfile[] apList = AssortmentMembers;
                foreach (AllocationProfile ap in apList)
                {
                    ap.BottomUpSizePerformed = value;
                }
                // end TT#1021 - MD - Jellis - Header Status Wrong
                base.BottomUpSizePerformed = value;
            }
		}

        /// <summary>
		/// Gets or sets RulesDefinedAndProcessed flag
		/// </summary>
        override public bool RulesDefinedAndProcessed
        {
            get
            {
                return (base.RulesDefinedAndProcessed || MemberRulesDefinedAndProcessed > 0);
            }
            set
            {
                base.RulesDefinedAndProcessed = value;
            }
        }

		/// <summary>
		/// Gets or sets NeedAllocationPerformed flag
		/// </summary>
		override public bool NeedAllocationPerformed
		{
			get
			{
                return (base.NeedAllocationPerformed || MemberNeedAllocationPerformed > 0);
			}
			set
			{
                // begin TT#1021 - MD - Jellis - Header Status Wrong
                AllocationProfile[] apList = AssortmentMembers;
                foreach (AllocationProfile ap in apList)
                {
                    ap.NeedAllocationPerformed = value;
                }
                // end TT#1021 - MD - Jellis - Header Status Wrong
				base.NeedAllocationPerformed = value;
			}
		}

        /// <summary>
		/// Gets or sets PackBreakoutByContent flag
		/// </summary>
		override public bool PackBreakoutByContent
		{
			get
			{
                return (base.PackBreakoutByContent || MemberPackBreakoutByContent > 0);
			}
			set
			{
                // begin TT#1021 - MD - Jellis - Header Status Wrong
                AllocationProfile[] apList = AssortmentMembers;
                foreach (AllocationProfile ap in apList)
                {
                    ap.PackBreakoutByContent = value;
                }
                // end TT#1021 - MD - Jellis - Header Status Wrong
				base.PackBreakoutByContent = value;
			}
		}

		/// <summary>
		/// Gets or sets BulkSizeBreakoutPerformed
		/// </summary>
		override public bool BulkSizeBreakoutPerformed
		{
			get
			{
                return (base.BulkSizeBreakoutPerformed || MemberBulkSizeBreakoutPerformed > 0);
			}
			set
			{
                // begin TT#1021 - MD - Jellis - Header Status Wrong
                AllocationProfile[] apList = AssortmentMembers;
                foreach (AllocationProfile ap in apList)
                {
                    ap.BulkSizeBreakoutPerformed = value;
                }
                // end TT#1021 - MD - Jellis - Header Status Wrong
				base.BulkSizeBreakoutPerformed = value;
			}
		}

        // begin TT#1064 - MD - Jellis - Cannot Release Group Allocation
        /// <summary>
        /// Gets or sets ReleaseApproved flag
        /// </summary>
        override public bool ReleaseApproved  // TT#1064 - MD - Jellis - Cannot Release Group Allocation
        {
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                // Begin TT#1211-MD - stodd - New assortments get created with a status of "Released"
                if (_memberReleaseApproved > 0)
                {
                    base.SetReleaseApproved(_memberReleaseApproved == AssortmentMembers.Length);
                }
                else
                {
                    base.SetReleaseApproved(false);
                }
                // End TT#1211-MD - stodd - New assortments get created with a status of "Released"
                return base.ReleaseApproved;
            }
        }

        /// <summary>
        /// Gets Released flag
        /// </summary>
        override public bool Released  // TT#1064 - MD - Jellis - Cannot Release Group Allocation
        {
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }

                // Begin TT#1211-MD - stodd - New assortments get created with a status of "Released"
                if (_memberReleased > 0)
                {
                    base.SetReleased(_memberReleased == AssortmentMembers.Length);
                }
                else
                {
                    base.SetReleased(false);
                }
                // End TT#1211-MD - stodd - New assortments get created with a status of "Released"
                return base.Released;
            }
        }

        /// <summary>
        /// Gets or sets ShippingStarted flag
        /// </summary>
        override public bool ShippingStarted
        {
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                base.SetShippingStarted(_memberShippingStarted > 0);
                return base.ShippingStarted;
            }
        }

        /// <summary>
        /// Gets or sets ShippingComplete flag
        /// </summary>
        override public bool ShippingComplete
        {
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                // Begin TT#1211-MD - stodd - New assortments get created with a status of "Released"
                if (_memberShippingComplete > 0)
                {
                    base.SetShippingComplete(_memberShippingComplete == AssortmentMembers.Length);
                }
                else
                {
                    base.SetShippingComplete(false);
                }
                // End TT#1211-MD - stodd - New assortments get created with a status of "Released"
                return base.ShippingComplete;
            }
         }

        /// <summary>
        /// Gets or sets ShippingOnHold audit flag.
        /// </summary>
        override public bool ShippingOnHold
        {
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }

                // Begin TT#1211-MD - stodd - New assortments get created with a status of "Released"
                if (_memberShippingOnHold > 0)
                {
                    base.ShippingOnHold = (_memberShippingOnHold == AssortmentMembers.Length);
                }
                else
                {
                    base.ShippingOnHold = false;
                }
                // End TT#1211-MD - stodd - New assortments get created with a status of "Released"

                return base.ShippingOnHold;
            }
            set
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                AllocationProfile[] apList = AssortmentMembers;
                foreach (AllocationProfile ap in apList)
                {
                    ap.ShippingOnHold = value;
                }
                base.ShippingOnHold = value;
            }
        }
        // end TT#1064 - MD - Jellis - Cannot Release Group Allocation

        /// <summary>
		/// Gets or sets SizeReceiptsBalanceToColor
		/// </summary>
		override public bool SizeReceiptsBalanceToColor
		{
			get
			{
                // Begin TT#1211-MD - stodd - New assortments get created with a status of "Released"
                if (_memberSizeReceiptsBalanceToColor > 0)
                {
                    base.SizeReceiptsBalanceToColor = (_memberSizeReceiptsBalanceToColor == AssortmentMembers.Length); // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                }
                else
                {
                    base.SizeReceiptsBalanceToColor = false;
                }
                // End TT#1211-MD - stodd - New assortments get created with a status of "Released"
                return base.SizeReceiptsBalanceToColor;
			}
		}

		/// <summary>
		/// Gets or sets ColorReceiptsBalanceToBulk
		/// </summary>
		override public bool ColorReceiptsBalanceToBulk
		{
			get
			{
                // Begin TT#1211-MD - stodd - New assortments get created with a status of "Released"
                if (_memberColorReceiptsBalanceToBulk > 0)
                {
                    base.ColorReceiptsBalanceToBulk = (_memberColorReceiptsBalanceToBulk == AssortmentMembers.Length); // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                }
                else
                {
                    base.ColorReceiptsBalanceToBulk = false;
                }
                // End TT#1211-MD - stodd - New assortments get created with a status of "Released"

                return base.ColorReceiptsBalanceToBulk;
			}
		}

        /// <summary>
		/// Gets or sets StyleIntransitUpdated
		/// </summary>
		override public bool StyleIntransitUpdated
		{
			get
			{
                // Begin TT#1211-MD - stodd - New assortments get created with a status of "Released"
                if (_memberStyleIntransitUpdated > 0)
                {
                    base.StyleIntransitUpdated = (_memberStyleIntransitUpdated == AssortmentMembers.Length); // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                }
                else
                {
                    base.StyleIntransitUpdated = false;
                }
                // End TT#1211-MD - stodd - New assortments get created with a status of "Released"

                return base.StyleIntransitUpdated;
			}
		}
        	/// <summary>
		/// Gets or sets BulkColorIntransitUpdated
		/// </summary>
		override public bool BulkColorIntransitUpdated
		{
			get
			{
                // Begin TT#1211-MD - stodd - New assortments get created with a status of "Released"
                if (_memberBulkColorIntransitUpdated > 0)
                {
                    base.BulkColorIntransitUpdated = (_memberBulkColorIntransitUpdated == AssortmentMembers.Length); // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                }
                else
                {
                    base.BulkColorIntransitUpdated = false;
                }
                // End TT#1211-MD - stodd - New assortments get created with a status of "Released"

                return base.BulkColorIntransitUpdated;
			}
		}
		/// <summary>
		/// Gets or sets BulkSizeIntransitUpdated
		/// </summary>
		override public bool BulkSizeIntransitUpdated
		{
			get
			{
                // Begin TT#1211-MD - stodd - New assortments get created with a status of "Released"
                if (_memberBulkSizeIntransitUpdated > 0)
                {
                    base.BulkSizeIntransitUpdated = (_memberBulkSizeIntransitUpdated == AssortmentMembers.Length); // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                }
                else
                {
                    base.BulkSizeIntransitUpdated = false;
                }
                // End TT#1211-MD - stodd - New assortments get created with a status of "Released"

                return base.BulkSizeIntransitUpdated;
			}
		}

        new public bool ProcessingGroupAllocation
        {
            get { return _processingGroupAllocation; }
			// Begin TT#1184-MD - stodd - db timeout on cancel allocation
            set 
            { 
                _processingGroupAllocation = value;
                // If we are beggining to process a groupAllocation,
                // mark processing completed to FALSE for member headers and placeholders.
                // When ProcessingGroupAllocation = false, we want to be sure to set 
                // the two switches to true to avoid any confusion.
                // These switches are also managed in the ProcessNonGroupAction() method.
                if (_processingGroupAllocation)
                {
                    _memberHeadersProcessingCompleted = false;
                    _memberPlaceholdersProcessingCompleted = false;
                }
                else
                {
                    _memberHeadersProcessingCompleted = true;
                    _memberPlaceholdersProcessingCompleted = true;
                }
            }
			// End TT#1184-MD - stodd - db timeout on cancel allocation
        }

        // begin TT#1064 - MD - Jellis - Cannot Release Group Allocation
        new public bool ProcessingActionOnHeaderInGroup
        {
            get { return _processingActionOnHeaderInGroup; }
            set { _processingActionOnHeaderInGroup = value; }
        }
        // end TT#1064 - MD - Jellis - Cannot Release Group Allocation

        /// <summary>
		/// Gets or sets PlanHnRID
		/// </summary>
		/// <remarks>
		/// Identifies the merchandise plan source.
		/// </remarks>
		override public int PlanHnRID
		{
			get
			{
				if (base.PlanHnRID == Include.DefaultPlanHnRID && StyleHnRID != Include.NoRID)
				{
                    HierarchyNodeProfile hnp = null;
                    if (AsrtType == (int)eAssortmentType.GroupAllocation)
                    {
                        hnp = AppSessionTransaction.GetPlanLevelData(PlanLevelStartHnRID);
                    }
                    else
                    {
                        hnp = AppSessionTransaction.GetNodeData(AsrtAnchorNodeRid);
                    }

					if (hnp == null)
					{
						if (AsrtType == (int)eAssortmentType.GroupAllocation)
						{
							PlanHnRID = StyleHnRID;
						}
						else
						{
							throw new MIDException(eErrorLevel.severe,
								(int)eMIDTextCode.msg_al_PlanLevelUndetermined,
								Session.Audit.GetText(eMIDTextCode.msg_al_PlanLevelUndetermined, false)); 
						}
					}
					else
					{
                        PlanHnRID = hnp.Key; 
					}
				}
				return base.PlanHnRID;
			}
			set
			{
				base.PlanHnRID = value;
                AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                foreach (AllocationProfile ap in apList)
                {
                    ap.PlanHnRID = value;
                }
			}
		}
        		/// <summary>
		/// Gets or sets OnHandHnRID
		/// </summary>
		/// <remarks>
		/// Identifies the merchandise OnHand source. 
		/// </remarks>
		override public int OnHandHnRID
		{
			get
			{
				return base.OnHandHnRID;
			}
			set
			{
				base.OnHandHnRID = value;
                AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                foreach (AllocationProfile ap in apList)
                {
                    ap.OnHandHnRID = value;
                }
			}
		}
		/// <summary>
		/// Gets or sets PlanFactor
		/// </summary>
		override public double PlanFactor
		{
			get
			{
				return base.PlanFactor;
			}
			set
			{
                base.PlanFactor = value;
                AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                foreach (AllocationProfile ap in apList)
                {
                    ap.PlanFactor = value;
                }
			}
		}
        		/// <summary>
		/// Gets or sets the GradeList definition.
		/// </summary>
		override public ArrayList GradeList
		{
			get
			{
                return base.GradeList;
			}
            set
            {
                base.GradeList = value;
                AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                foreach (AllocationProfile ap in apList)
                {
                    ap.GradeList = value;
                }
            }
        }

        // Begin TT#2101-MD - JSmith - Matrix values change when beg and Del dates changed without REDO being selected.
        //
        public bool AssortmentAllocationStarted
        {
            get
            {
                if (AllocationStarted)
                {
                    return true;
                }

                AllocationProfile[] apList = AssortmentMembers; 
                foreach (AllocationProfile ap in apList)
                {
                    if (ap.AllocationStarted)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
        // End TT#2101-MD - JSmith - Matrix values change when beg and Del dates changed without REDO being selected.

        // begin TT#935 - MD - Jellis - Group Allocation - Infrastructure built wrong
        /// <summary>
        /// Gets or sets Total Units To Allocate
        /// </summary>
        override public int TotalUnitsToAllocate 
        {
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                return base.TotalUnitsToAllocate;
            }
            set
            {
                base.TotalUnitsToAllocate = value;
            }
        }

        /// <summary>
        /// Gets or sets TotalUnitsAllocated
        /// </summary>
        override public int TotalUnitsAllocated  
        {
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
 
                return base.TotalUnitsAllocated;
            }
            internal set
            {
                base.TotalUnitsAllocated = value;
            }
        }
        // end TT#935 - MD - Jellis - Group Allocation - Infrastructure built wrong
        
        /// <summary>
        /// Gets or sets Total ItemUnitsAllocated
        /// </summary>
        override public int TotalItemUnitsAllocated
        {
            get
            {
                if (AppSessionTransaction.GetCalcAssortmentItemVsw(BuildItemKey))
                {
                    AssortmentPropertiesRebuildItemVsw();
                }
                return base.TotalItemUnitsAllocated;
            }
        }
        // End TT#1401
        /// <summary>
        /// Gets Total ImoUnitsAllocated
        /// </summary>
        override public int TotalImoUnitsAllocated
        {
            get
            {
                int imoAllocatedUnits = 
                    TotalUnitsAllocated 
                    - TotalItemUnitsAllocated;
                if (imoAllocatedUnits < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)
                        + ": TotalImoUnitsAllocated in " + GetType().Name);
                }
                return imoAllocatedUnits;
            }
        }

        /// <summary>
        /// Gets or sets Total ItemOrigUnitsAllocated
        /// </summary>
        override public int TotalItemOrigUnitsAllocated
        {
            get
            {
                if (AppSessionTransaction.GetCalcAssortmentItemVsw(BuildItemKey))
                {
                    AssortmentPropertiesRebuildItemVsw();
                }
                return base.TotalItemOrigUnitsAllocated; 
            }
        }

        // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
        /// <summary>
        /// Gets total UnitsShipped
        /// </summary>
        override public int UnitsShipped  
        {
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                return base.UnitsShipped;
            }
            set
            {
                base.UnitsShipped = value;
            }
        }
        // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong

        override public int  AllocationMultiple
        {
	        get 
	        {
                // begin TT#488 - MD - JEllis - Group Allocation 
                if (base.AllocationMultiple > 1)
                {
                    return base.AllocationMultiple;
                }
                // end TT#488 - MD - JEllis - Group Allocation 

                if (MemberAllocationMultiple > 0)
                {
                    return MemberAllocationMultiple;
                }
                return base.AllocationMultiple; // TT#488 - MD - Jellis - Group Allocation
	        }
	        set 
	        { 
		        base.AllocationMultiple = value;
	        }
        }
        
        private int MemberAllocationMultiple
        {
            get
            {
                if (_memberAllocationMultiple < 0)
                {
                    _memberAllocationMultiple = 0;
                    AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error

                    if (apList.Length == 0) // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                    {
                        return _memberAllocationMultiple;
                    }
                    _memberAllocationMultiple = apList[0].AllocationMultiple;
                    foreach (AllocationProfile ap in apList)
                    {
                        _memberAllocationMultiple = MIDMath.GreatestCommonDivisor(_memberAllocationMultiple, ap.AllocationMultiple);
                    }
                }
                return _memberAllocationMultiple;
            }
        }

        // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
        /// <summary>
        /// Gets Generic Units To Allocate
        /// </summary>
        override public int GenericUnitsToAllocate  
        {
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                return base.GenericUnitsToAllocate;
            }
            set
            {
                base.GenericUnitsToAllocate = value;
            }
        }
        // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong

		/// <summary>
		/// Gets or sets GenericUnitsAllocated
		/// </summary>
		override public int GenericUnitsAllocated
		{
			get
			{
                // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong 
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong

                return base.GenericUnitsAllocated;
			}
			set
			{
				if (GenericUnitsAllocated != value)
				{
                    try
                    {
                        int spreadBasis = 0;
                        int genericUnitsAllocated = value;
                        AllocationProfile[] sapList = AssortmentMembersSorted; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                        SuspendAssortmentUpdates = true;
                        foreach (AllocationProfile ap in sapList)
                        {
                            spreadBasis = ap.TotalUnitsToAllocate;
                        }
                        foreach (AllocationProfile ap in sapList)
                        {
                            if (spreadBasis > 0)
                            {
                                ap.GenericUnitsAllocated =
                                    (int)(((double)ap.TotalUnitsToAllocate
                                           * (double)genericUnitsAllocated
                                           / (double)spreadBasis) + .5d);
                            }
                            else
                            {
                                ap.GenericUnitsAllocated = 0;
                            }
                            spreadBasis -= ap.TotalUnitsToAllocate;
                            genericUnitsAllocated -= ap.GenericUnitsAllocated;
                        }
                    }
                    finally
                    {
                        SuspendAssortmentUpdates = false;
                    }

				}
			}
		}

        /// <summary>
        /// Gets or sets GenericItemUnitsAllocated
        /// </summary>
        override public int GenericItemUnitsAllocated
        {
            get
            {
                if (AppSessionTransaction.GetCalcAssortmentItemVsw(BuildItemKey))
                {
                    AssortmentPropertiesRebuildItemVsw();
                }
                return base.GenericItemUnitsAllocated;

            }
            set
            {
                if (GenericItemUnitsAllocated != value)
                {
                    try
                    {
                        int spreadBasis = 0;
                        int genericItemUnitsAllocated = value;
                        AllocationProfile[] sapList = AssortmentMembersSorted; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                        SuspendAssortmentUpdates = true;
                        foreach (AllocationProfile ap in sapList)
                        {
                            spreadBasis = ap.TotalUnitsToAllocate;
                        }
                        foreach (AllocationProfile ap in sapList)
                        {
                            if (spreadBasis > 0)
                            {
                                ap.GenericItemUnitsAllocated =
                                    (int)(((double)ap.TotalUnitsToAllocate
                                       * (double)genericItemUnitsAllocated
                                       / (double)spreadBasis) + .5d);
                            }
                            else
                            {
                                ap.GenericItemUnitsAllocated = 0;
                            }
                            spreadBasis -= ap.TotalUnitsToAllocate;
                            genericItemUnitsAllocated -= ap.GenericUnitsAllocated;
                        }
                    }
                    finally
                    {
                        SuspendAssortmentUpdates = false;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets GenericImoUnitsAllocated
        /// </summary>
        override public int GenericImoUnitsAllocated
        {
            get
            {
                int imoAllocatedUnits =
                    GenericUnitsAllocated 
                    - GenericItemUnitsAllocated;
                if (imoAllocatedUnits < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)
                        + ": GenericImoUnitsAllocated in " + GetType().Name);
                }
                return imoAllocatedUnits;
            }
        }

        /// <summary>
        /// Gets or sets Generic Multiple
        /// </summary>
        override public int GenericMultiple
        {
            get
            {
                // begin TT#488 - MD - JEllis - Group Allocation 
                if (base.GenericMultiple > 1)
                {
                    return base.GenericMultiple;
                }
                // end TT#488 - MD - JEllis - Group Allocation 

                if (MemberGenericMultiple > 0)
                {
                    return MemberGenericMultiple;
                }
                return base.GenericMultiple; // TT#488 - MD - Jellis - Group Allocation
            }
            set
            {
                base.GenericMultiple = value;
            }
        }

        private int MemberGenericMultiple
        {
            get
            {
                if (_memberGenericMultiple < 0)
                {
                    _memberGenericMultiple = 0;
                    AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error

                    if (apList.Length == 0) // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                    {
                        return _memberGenericMultiple;
                    }
                    _memberGenericMultiple = apList[0].GenericMultiple;
                    foreach (AllocationProfile ap in apList)
                    {
                        _memberGenericMultiple = MIDMath.GreatestCommonDivisor(_memberGenericMultiple, ap.GenericMultiple);
                    }
                }
                return _memberGenericMultiple;
            }
        }

        /// <summary>
        /// Gets or sets Detail Type Units To Allocate
        /// </summary>
        override public int DetailTypeUnitsToAllocate  
        {
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                return base.DetailTypeUnitsToAllocate;
            }
            set
            {
                base.DetailTypeUnitsToAllocate = value;
            }
        }

		/// <summary>
		/// Gets or sets DetailTypeUnitsAllocated
		/// </summary>
		override public int DetailTypeUnitsAllocated
		{
			get
			{
                // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong 
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong

                return base.DetailTypeUnitsAllocated;
			}
			set
			{
				if (DetailTypeUnitsAllocated != value)
				{
                    try
                    {
                        int spreadBasis = 0;
                        int detailUnitsAllocated = value;
                        AllocationProfile[] sapList = AssortmentMembersSorted; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                        SuspendAssortmentUpdates = true;
                        foreach (AllocationProfile ap in sapList)
                        {
                            spreadBasis = ap.TotalUnitsToAllocate;
                        }
                        foreach (AllocationProfile ap in sapList)
                        {
                            if (spreadBasis > 0)
                            {
                                ap.DetailTypeUnitsAllocated =
                                    (int)(((double)ap.TotalUnitsToAllocate
                                       * (double)detailUnitsAllocated
                                       / (double)spreadBasis) + .5d);
                            }
                            else
                            {
                                ap.DetailTypeUnitsAllocated = 0;
                            }
                            spreadBasis -= ap.TotalUnitsToAllocate;
                            detailUnitsAllocated -= ap.DetailTypeUnitsAllocated;
                        }
                    }
                    finally
                    {
                        SuspendAssortmentUpdates = false;
                    }
				}
			}
		}

        /// <summary>
        ///  Gets or sets DetailItemUnitsAllocated
        /// </summary>
        override public int DetailTypeItemUnitsAllocated
        {
            get
            {
                if (AppSessionTransaction.GetCalcAssortmentItemVsw(BuildItemKey))
                {
                    AssortmentPropertiesRebuildItemVsw();
                }
                return base.DetailTypeItemUnitsAllocated;
            }
            set
            {
                if (DetailTypeItemUnitsAllocated != value)
                {
                    int spreadBasis = 0;
                    int detailItemUnitsAllocated = value;
                    AllocationProfile[] sapList = AssortmentMembersSorted; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                    foreach (AllocationProfile ap in sapList)
                    {
                        spreadBasis = ap.TotalUnitsToAllocate;
                    }
                    foreach (AllocationProfile ap in sapList)
                    {
                        if (spreadBasis > 0)
                        {
                        ap.DetailTypeItemUnitsAllocated =
                            (int)(((double)ap.TotalUnitsToAllocate
                                   * (double)detailItemUnitsAllocated
                                   / (double)spreadBasis) + .5d);
                        }
                        else
                        {
                            ap.DetailTypeItemUnitsAllocated = 0;
                        }
                        spreadBasis -= ap.TotalUnitsToAllocate;
                        detailItemUnitsAllocated -= ap.DetailTypeItemUnitsAllocated;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets DetailItemUnitsAllocated
        /// </summary>
        override public int DetailTypeImoUnitsAllocated
        {
            get
            {
                int imoAllocatedUnits =
                    DetailTypeUnitsAllocated 
                    - DetailTypeItemUnitsAllocated;
                if (imoAllocatedUnits < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)
                        + ": DetailTypeImoUnitsAllocated in " + GetType().Name);
                }
                return imoAllocatedUnits;
            }
        }
  
        /// <summary>
        /// Gets or sets DetailTypeMultiple
        /// </summary>
        override public int DetailTypeMultiple
        {
            get
            {
                // begin TT#488 - MD - JEllis - Group Allocation 
                if (base.DetailTypeMultiple > 1)
                {
                    return base.DetailTypeMultiple;
                }
                // end TT#488 - MD - JEllis - Group Allocation 

                if (MemberDetailTypeMultiple > 0)
                {
                    return MemberDetailTypeMultiple;
                }
                return base.DetailTypeMultiple; // TT#488 - MD - Jellis - Group Allocation
            }
            set
            {
                base.DetailTypeMultiple = value;
            }
        }
        private int MemberDetailTypeMultiple
        {
            get
            {
                if (_memberDetailTypeMultiple < 0)
                {
                    _memberDetailTypeMultiple = 0;
                    AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error

                    if (apList.Length == 0) // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                    {
                        return _memberDetailTypeMultiple;
                    }
                    _memberDetailTypeMultiple = apList[0].DetailTypeMultiple;
                    foreach (AllocationProfile ap in apList)
                    {
                        _memberDetailTypeMultiple = MIDMath.GreatestCommonDivisor(_memberDetailTypeMultiple, ap.DetailTypeMultiple);
                    }
                }
                return _memberDetailTypeMultiple;
            }
        }

        // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
        /// <summary>
        /// Gets Bulk Units To Allocate
        /// </summary>
        override public int BulkUnitsToAllocate  // TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong  // TT#1148 - MD - Capacity Enforced on wrong header (unrelated issue)
        {
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                return base.BulkUnitsToAllocate;
            }
            set
            {
                base.BulkUnitsToAllocate = value;
            }
        }
        // end TT#935 - MD - JEllis - Group Allocation Infrastructure built wrong

		/// <summary>
		/// Gets or sets BulkUnitsAllocated
		/// </summary>
		override public int BulkUnitsAllocated
		{
			get
			{
                // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong 
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong

                return base.BulkUnitsAllocated;
			}
			set
			{
				if (BulkUnitsAllocated != value)
				{
                    int spreadBasis = 0;
                    int bulkUnitsAllocated = value;
                    AllocationProfile[] sapList = AssortmentMembersSorted; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                    foreach (AllocationProfile ap in sapList)
                    {
                        spreadBasis = ap.TotalUnitsToAllocate;
                    }
                    foreach (AllocationProfile ap in sapList)
                    {
                        if (spreadBasis > 0)
                        {
                        ap.BulkUnitsAllocated =
                            (int)(((double)ap.TotalUnitsToAllocate
                                   * (double)bulkUnitsAllocated
                                   / (double)spreadBasis) + .5d);
                        }
                        else
                        {
                            ap.BulkUnitsAllocated = 0;
                        }
                        spreadBasis -= ap.TotalUnitsToAllocate;
                        bulkUnitsAllocated -= ap.BulkUnitsAllocated;
                    }
				}
			}
		}

        /// <summary>
        ///  Gets or sets BulkItemUnitsAllocated
        /// </summary>
        override public int BulkItemUnitsAllocated
        {
            get
            {
                if (AppSessionTransaction.GetCalcAssortmentItemVsw(BuildItemKey))
                {
                    AssortmentPropertiesRebuildItemVsw();
                }
                return base.BulkItemUnitsAllocated;
            }
            set
            {
                if (BulkItemUnitsAllocated != value)
                {
                    try
                    {
                        int spreadBasis = 0;
                        int bulkItemUnitsAllocated = value;
                        AllocationProfile[] sapList = AssortmentMembersSorted; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                        SuspendAssortmentUpdates = true;
                        foreach (AllocationProfile ap in sapList)
                        {
                            spreadBasis = ap.TotalUnitsToAllocate;
                        }
                        foreach (AllocationProfile ap in sapList)
                        {
                            if (spreadBasis > 0)
                            {
                                ap.BulkItemUnitsAllocated =
                                    (int)(((double)ap.TotalUnitsToAllocate
                                           * (double)bulkItemUnitsAllocated
                                           / (double)spreadBasis) + .5d);
                            }
                            else
                            {
                                ap.DetailTypeItemUnitsAllocated = 0;
                            }
                            spreadBasis -= ap.TotalUnitsToAllocate;
                            bulkItemUnitsAllocated -= ap.BulkItemUnitsAllocated;
                        }
                    }
                    finally
                    {
                        SuspendAssortmentUpdates = false;
                    }
                }
            }
        }

        /// <summary>
        /// Gets  BulkImoUnitsAllocated
        /// </summary>
        override public int BulkImoUnitsAllocated
        {
            get
            {
                int imoAllocatedUnits =
                    BulkUnitsAllocated 
                    - BulkItemUnitsAllocated;
                if (imoAllocatedUnits < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
                        MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg)
                        + ": BulkImoUnitsAllocated in " + GetType().Name);
                }
                return imoAllocatedUnits;
            }
        }

        /// <summary>
        /// Gets or sets Bulk Multiple
        /// </summary>
        override public int BulkMultiple
        {
            get
            {
                // begin TT#488 - MD - JEllis - Group Allocation 
                if (base.BulkMultiple > 1)
                {
                    return base.BulkMultiple;
                }
                // end TT#488 - MD - JEllis - Group Allocation 

                if (MemberBulkMultiple > 0)
                {
                    return MemberBulkMultiple;
                }
                return base.BulkMultiple; // TT#488 - MD - Jellis - Group Allocation
            }
            set
            {
                base.BulkMultiple = value;
            }
        }
        private int MemberBulkMultiple
        {
            get
            {
                if (_memberBulkMultiple < 0)
                {
                    _memberBulkMultiple = 0;
                    AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error

                    if (apList.Length == 0) // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                    {
                        return _memberBulkMultiple;
                    }
                    _memberBulkMultiple = apList[0].BulkMultiple;
                    foreach (AllocationProfile ap in apList)
                    {
                        _memberBulkMultiple = MIDMath.GreatestCommonDivisor(_memberBulkMultiple, ap.BulkMultiple);
                    }
                }
                return _memberBulkMultiple;
            }
        }


        ///// <summary>
        ///// Gets number of packs in assortment/group
        ///// </summary>
        //override public int PackCount
        //{
        //    get
        //    {
        //        List<AllocationProfile> apList = AssortmentMembers;
        //        int packCount = 0;
        //        foreach (AllocationProfile ap in apList)
        //        {
        //            packCount += ap.PackCount;
        //        }
        //        return packCount;
        //    }
        //}
        override public int ColorHnRID
        {
            get
            {				
                if (DoColorCheck)
                {
                    AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                    if (apList.Length == 0) // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                    {
                        base.ColorHnRID = Include.NoRID;
                        base.HeaderColorCodeRID = Include.NoRID;
                    }
                    else
                    {
                        base.ColorHnRID = apList[0].ColorHnRID;
                        base.HeaderColorCodeRID = apList[0].HeaderColorCodeRID;
                        foreach (AllocationProfile ap in apList)
                        {
                            if (base.ColorHnRID != ap.ColorHnRID
                                || base.HeaderColorCodeRID != ap.HeaderColorCodeRID)
                            {
                                base.ColorHnRID = Include.NoRID;
                                base.HeaderColorCodeRID = Include.NoRID;
                            }
                        }
                    }
                    DoColorCheck = false;
                }
                return base.ColorHnRID;
            }
            set
            {
                base.ColorHnRID = value;
            }
        }
        /// <summary>
		/// Gets a hierarchy node RID used to look up OTS Plan Level
		/// </summary>
		override public int PlanLevelStartHnRID
		{
			get
			{
				if (ColorHnRID != Include.NoRID)
					return ColorHnRID;
				else
					return StyleHnRID;
			}
		} 
        /// <summary>
        /// Gets the header color code RID of the header (when multiple colors, returned RID is "Dummy Color RID" which is "0"
        /// </summary>
        override public int HeaderColorCodeRID
        {
            get
            {
                if (DoColorCheck)
                {
                    int colorHnRID = ColorHnRID;  // this will set the color code of the assortment
                }
                return base.HeaderColorCodeRID;
            }
            set
            {
                base.HeaderColorCodeRID = value;
            }
        }

        /// <summary>
		/// Gets the total store style allocation of the manually changed stores.
		/// </summary>
		override public int StoreStyleManualAllocationTotal
		{
			get
			{
                base.StoreStyleManualAllocationTotal = 0;
                AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                foreach (AllocationProfile ap in apList)
                {
                    base.StoreStyleManualAllocationTotal += ap.StoreStyleManualAllocationTotal;
                }
                return base.StoreStyleManualAllocationTotal;
			}
		}
		/// <summary>
		/// Gets the total store size allocation of the manually changed store sizes
		/// </summary>
		override public int StoreSizeManualAllocationTotal
		{
			get
			{
                base.StoreSizeManualAllocationTotal = 0;
                AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                foreach (AllocationProfile ap in apList)
                {
                    base.StoreSizeManualAllocationTotal += ap.StoreSizeManualAllocationTotal;
                }
                return base.StoreSizeManualAllocationTotal;
			}
 		}
        /// <summary>
		/// Gets the number of stores whose style/color allocation has been manually changed
		/// </summary>
		override public int StoreStyleAllocationManuallyChangedCount
		{
			get
			{
                AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                base.StoreStyleAllocationManuallyChangedCount = 0;
                if (!base.StoresLoaded)
                {
                    LoadStores();
                    foreach (AllocationProfile ap in apList)
                    {
                        if (!ap.StoresLoaded)
                        {
                            ap.LoadStores();
                        }
                    }
                }
                Index_RID[] storeIndexRIDs = AppSessionTransaction.StoreIndexRIDArray();
                for (int i = 0; i < storeIndexRIDs.Length; i++)
                {
                    foreach (AllocationProfile ap in apList)
                    {
                        if (ap.GetStoreStyleAllocationIsManuallyAllocated(storeIndexRIDs[i]))
                        {
                            base.StoreStyleAllocationManuallyChangedCount++;
                            break;
                        }
                    }
                }
                return base.StoreStyleAllocationManuallyChangedCount;
			}
		}
		/// <summary>
		/// Gets the number of stores whose color-size allocation has been manually changed
		/// </summary>
		override public int StoreSizeAllocationManuallyChangedCount
		{
			get
			{
                AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                base.StoreSizeAllocationManuallyChangedCount = 0;
                if (!base.StoresLoaded)
                {
                    LoadStores();
                    foreach (AllocationProfile ap in apList)
                    {
                        if (!ap.StoresLoaded)
                        {
                            ap.LoadStores();
                        }
                    }
                }
                Index_RID[] storeIndexRIDs = AppSessionTransaction.StoreIndexRIDArray();
                for (int i = 0; i < storeIndexRIDs.Length; i++)
                {
                    foreach (AllocationProfile ap in apList)
                    {
                        if (ap.GetStoreSizeAllocationIsManuallyAllocated(storeIndexRIDs[i]))
                        {
                            base.StoreSizeAllocationManuallyChangedCount++;
                            break;
                        }
                    }
                }
                return base.StoreSizeAllocationManuallyChangedCount;
            }
		}
        #endregion Allocation Profile Property Overrides
        
        #region Additional Allocation Properties
    
        /// <summary>
        /// Gets AtLeastOneMemberChargedToStyleIntransit (ie. True if at least one member is charged to "style" intransit)
        /// </summary>
        public bool AtLeastOneMemberChargedToStyleIntransit
        {
            get
			{
                return (_memberStyleIntransitUpdated > 0);
			}
        }
        /// <summary>
        /// Gets AtLeastOneMemberChargedToBulkColorIntransit (ie. True if at least one member is charged to "bulk color" intransit
        /// </summary>
        public bool AtLeastOneMemberChargedToBulkColorIntransit
        {
            get
			{
                return (_memberBulkColorIntransitUpdated > 0);
			}
        }
                /// <summary>
        /// Gets AtLeastOneMemberChargedToBulkSizeIntransit (ie. True if at least one member is charged to "bulk size" intransit
        /// </summary>
        public bool AtLeastOneMemberChargedToBulkSizeIntransit
        {
            get
			{
                return (_memberBulkSizeIntransitUpdated > 0);
			}
        }
        /// <summary>
        /// Gets or sets number of member headers with Color Receipts Balanced to Bulk
        /// </summary>
        public int MemberColorReceiptsBalanceToBulk
        {
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong 
            get 
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                return _memberColorReceiptsBalanceToBulk; 
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "MemberColorReceiptsBalanceToBulk")) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                        + " in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                }
                _memberColorReceiptsBalanceToBulk = value;
            }
        }
        /// <summary>
        /// Gets or sets number of headers with Bulk Size Receipts Balanced to Bulk Color
        /// </summary>
        public int MemberSizeReceiptsBalanceToColor
        {
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong 
            get 
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                return _memberSizeReceiptsBalanceToColor; 
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "MemberSizeReceiptsBalanceToColor")) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                        + " in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                }
                _memberSizeReceiptsBalanceToColor = value;
            }
        }
        /// <summary>
        /// Gets or sets number of member headers charged to Bulk Size Intransit
        /// </summary>
        public int MemberBulkSizeIntransitUpdated
        {
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong 
            get 
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                 return _memberBulkSizeIntransitUpdated;
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "MemberBulkSizeIntransitUpdated")) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                        + " in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                }
                _memberBulkSizeIntransitUpdated = value;
            }
        }
        /// <summary>
        /// Gets or sets number of member headers charged to Bulk Color Intransit
        /// </summary>
        public int MemberBulkColorIntransitUpdated
        {
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong 
            get 
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                 return _memberBulkColorIntransitUpdated;
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "MemberBulkColorIntransitUpdated")) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                        + " in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                }
                _memberBulkColorIntransitUpdated = value;
            }
        }
        /// <summary>
        /// Gets or sets number of member headers charged to Style Intransit
        /// </summary>
        public int MemberStyleIntransitUpdated
        {
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong 
            get 
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                return _memberStyleIntransitUpdated;
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "MemberStyleIntransitUpdated")) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                        + " in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                }
                _memberStyleIntransitUpdated = value;
            }
        }
        /// <summary>
        /// Gets or sets number of member headers where Bottom Up Size Performed
        /// </summary>
        public int MemberBottomUpSizePerformed
        {
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong 
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                return _memberBottomUpSizePerformed;
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "MemberBottomUpSizePerformed")) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                        + " in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                }
                _memberBottomUpSizePerformed = value;
            }
        }
        /// <summary>
        /// Gets or sets number of member headers where Rules Defined and Processed
        /// </summary>
        public int MemberRulesDefinedAndProcessed      
        {
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong 
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                return _memberRulesDefinedAndProcessed;
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "MemberRulesDefinedAndProcessed")) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                        + " in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                }
                _memberRulesDefinedAndProcessed = value;
            }
        }
        /// <summary>
        /// Gets or sets number of member headers where Need Allocation Performed 
        /// </summary>
        public int MemberNeedAllocationPerformed
        {
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong 
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                return _memberNeedAllocationPerformed;
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "MemberNeedAllocationPerformed")) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                        + " in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                }
                _memberNeedAllocationPerformed = value;
            }
        }
        /// <summary>
        /// Gets or sets number of member headers where Pack Breakout by Content occurred
        /// </summary>
        public int MemberPackBreakoutByContent
        {
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong 
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                return _memberPackBreakoutByContent;
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "MemberPackBreakoutByContent")) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                        + " in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                }
                _memberPackBreakoutByContent = value;
            }
        }
        /// <summary>
        /// Gets or sets number of member headers where bulk size breakouts performed
        /// </summary>
        public int MemberBulkSizeBreakoutPerformed
        {
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong 
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                return _memberBulkSizeBreakoutPerformed;
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong

            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "MemberBulkSizeBreakoutPerformed")) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                        + " in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error 

                }
                _memberBulkSizeBreakoutPerformed = value;
            }
        }
        // begin TT#1064 - MD - Jellis - Cannot Release Group Allocation
        /// <summary>
        /// Gets or sets number of member headers where header is Release Approved
        /// </summary>
        public int MemberReleaseApproved
        {
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong 
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                return _memberReleaseApproved;
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "MemberReleaseApproved")) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                        + " in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                }
                _memberReleaseApproved = value;
            }
        }

        /// <summary>
        /// Gets or sets number of member headers where header is Released
        /// </summary>
        public int MemberReleased
        {
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong 
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                return _memberReleased;
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "MemberReleased")) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                        + " in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                }
                _memberReleased = value;
            }
        }
        /// <summary>
        /// Gets or sets number of member headers where Shipping Started
        /// </summary>
        public int MemberShippingStarted
        {
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong 
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                return _memberShippingStarted;
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "MemberShippingStarted")) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                        + " in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                }
                _memberShippingStarted = value;
            }
        }

        /// <summary>
        /// Gets or sets number of member headers where Shipping Complete
        /// </summary>
        public int MemberShippingComplete
        {
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong 
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                return _memberRulesDefinedAndProcessed;
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "MemberShippingComplete")) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                        + " in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                }
                _memberShippingComplete = value;
            }
        }

        /// <summary>
        /// Gets or sets number of member headers where Shipping On Hold
        /// </summary>
        public int MemberShippingOnHold
        {
            // begin TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong 
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                return _memberRulesDefinedAndProcessed;
            }
            // end TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            set
            {
                if (value < 0)
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_ValueCannotBeNeg,
                        String.Format(MIDText.GetText(eMIDTextCode.msg_al_ValueCannotBeNeg, "MemberShippingOnHold")) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                        + " in " + GetType().Name); // TT#891 - MD - Jellis - Group Allocation Need Gets Error
                }
                _memberShippingOnHold = value;
            }
        }
        // end TT#1064 - MD - Jellis - Cannot Release Group Allocation
        // begin TT#1148 - MD - Jellis -Group Allocation Enforces Capacity on wrong header
        /// <summary>
        /// True: when all members of group have same capacity node; False otherwise
        /// </summary>
        public bool AllMemberHeadersHaveSameCapacityNode
        {
            get
            {
                if (BuildAssortmentProperties)
                {
                    AssortmentPropertiesRebuild();
                }
                return _allMembersHaveSameCapacityNode;
            }
        }
        // end TT#1148 - MD - Jellis - Group Allocation Enforces Capacity on wrong header

        // begin TT#1568 - MD - Jellis - Size Need on GA allocation not observing Header Min/Max
        public Dictionary<long, StoreVector> InventoryBasisAllocation
        {
            get
            {
                if (_buildInventoryBasisAllocation)
                {
                    CreateInventoryBasisAllocationXref();
                }
                Dictionary<long, StoreVector> inventoryBasisAllocation = new Dictionary<long,StoreVector>();
                StoreVector sv;
                foreach (KeyValuePair<long,StoreVector> keyValuePair in _inventoryBasisAllocation)
                {
                    sv = (StoreVector)(keyValuePair.Value).Clone();
                    inventoryBasisAllocation.Add(keyValuePair.Key, sv);
                }
                return inventoryBasisAllocation;
            }
        }
        // end TT#1568 - MD - Jellis - Size Need on GA allocation not observing Header Min/Max
        #endregion Additional Allocation Properties
        // end TT#488  - MD - Jellis - Group Allocation

        #endregion Properties

        #region Methods
        //==============
		// Methods
		//==============

        #region Assortment Specific Methods
		static public DataTable CreateAssortmentComponentTable(AssortmentComponentVariables aComponentVars)
		{
			DataTable dtComponents;

			try
			{
				dtComponents = MIDEnvironment.CreateDataTable("Components");

				dtComponents.Columns.Add(new DataColumn("READONLY", typeof(bool)));
				dtComponents.Columns.Add(new DataColumn("ASSORTMENT_IND", typeof(bool)));

				foreach (AssortmentComponentVariableProfile varProf in aComponentVars.VariableProfileList)
				{
					dtComponents.Columns.Add(new DataColumn(varProf.RIDColumnName, typeof(int)));
					dtComponents.Columns.Add(new DataColumn(varProf.TextColumnName, typeof(string)));
					dtComponents.Columns.Add(new DataColumn(varProf.AlternateTextColumnName, typeof(string)));
				}

				dtComponents.Columns.Add(new DataColumn("PLACEHOLDERSEQ_RID", typeof(int)));
				dtComponents.Columns.Add(new DataColumn("PLACEHOLDERSEQ", typeof(string)));
				dtComponents.Columns.Add(new DataColumn("PLACEHOLDERSEQ_ALTERNATE", typeof(string)));

				dtComponents.Columns.Add(new DataColumn("HEADERSEQ_RID", typeof(int)));
				dtComponents.Columns.Add(new DataColumn("HEADERSEQ", typeof(string)));
				dtComponents.Columns.Add(new DataColumn("HEADERSEQ_ALTERNATE", typeof(string)));
                dtComponents.Columns.Add(new DataColumn("COLORSEQ", typeof(string))); //TT#664 - MD - DOConnell - Placeholder colors display in reverse order on the Content Tab

				return dtComponents;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// BEgin TT#1876 - STODD - Basis info wrong when coming from selection screen
		/// <summary>
		/// Replaces assortment information with that from Assortment Selection screen
		/// </summary>
		/// <param name="trans"></param>
		public void SetupSummaryfromSelection(ApplicationSessionTransaction trans)
		{
			if (trans != null)
			{
				if (trans.AssortmentViewSelectionCriteria != null)
				{
					FillAssortHeaderFromSelection(trans);
					FillAssortBasisFromSelection(trans);
					FillAssortGradesFromSelection(trans);
				}
			}
		}
		// END TT#1876 - STODD - Basis info wrong when coming from selection screen

		// Begin TT#952 - MD - stodd - add matrix to Group Allocation Review
        public void SetupSummaryfromGroupAllocation(ApplicationSessionTransaction trans)
        {
            FillAssortHeaderFromGroupAllocation(trans);
            FillAssortBasisFromGroupAllocation();
            FillAssortGradesFromGroupAllocation(trans);
        }

        public void SetupSummaryfromAssortment(int asrtRid)
        {
            FillAssortHeader(asrtRid);
            FillAssortBasis(asrtRid);
            FillAssortGrades(asrtRid);
        }
		// End TT#952 - MD - stodd - add matrix to Group Allocation Review

		public void GetAssortmentComponents(AssortmentCubeGroup aAsrtCubeGroup, DataTable aComponentTable, PackColorProfileXRef aPackColorXRef)
		{
			Hashtable packHash;
			Hashtable packColorHash;
			Header dlAssortment;
			DataTable dtAssrtComponents;
			AssortmentViewComponentVariables vars;
			AllocationSummaryComponentVariables summaryComponentVars;
			DataTable dtHdrComponents;
			AllocationProfile hdrProf;
			AllocationProfile plchldrProf;
			object[] rowParms;
			int i;
			NodeCharProfileList charProfList;
			NodeCharProfile charProf;
			string hashKey;
			PackColorHashEntry packHashEntry;
			PackColorHashEntry packColorHashEntry;
            //int packRID;
            //string packID;

			try
			{
				packHash = new Hashtable();
				packColorHash = new Hashtable();

				dlAssortment = new Header();
				dtAssrtComponents = dlAssortment.GetAssormentComponents(Key);
				vars = (AssortmentViewComponentVariables)aAsrtCubeGroup.AssortmentComponentVariables;

				summaryComponentVars = new AllocationSummaryComponentVariables();
				summaryComponentVars.Initialize();
				dtHdrComponents = AllocationProfile.CreateAllocationComponentTable(summaryComponentVars);
				// Begin TT#1429 - stodd - do not show difference line on post receipt
				string balanceText = MIDText.GetTextOnly(eMIDTextCode.msg_as_PlaceholderBalance);
				// End TT#1439
				// BEGIN TT#2402 - stodd - totaling incorrect when certain views are selected
				string styleText = string.Empty;
				// END TT#2402 - stodd - totaling incorrect when certain views are selected

                eAssortmentType assortmentType = eAssortmentType.Undefined;	// TT#3932 - stodd - Issues with Matrix View for headers with packs
				foreach (DataRow assrtRow in dtAssrtComponents.Rows)
				{
					// Begin TT#1439 - stodd - post-receipt
					bool isDifferenceRow = false;
					// End TT#1439


					if ((eHeaderType)Convert.ToInt32(assrtRow["DISPLAY_TYPE"]) == eHeaderType.Placeholder)
					{
						dtHdrComponents.Clear();
						// BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
						//hdrProf = AppSessionTransaction.GetAllocationProfile(Convert.ToInt32(assrtRow["HDR_RID"]));
						hdrProf = AppSessionTransaction.GetAssortmentMemberProfile(Convert.ToInt32(assrtRow["HDR_RID"]));
						// END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member

                        if (hdrProf == null)
                        {
                            hdrProf = new AllocationProfile(AppSessionTransaction, string.Empty, Convert.ToInt32(assrtRow["HDR_RID"]), Session);
                            // BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
                            //AppSessionTransaction.AddAllocationProfile(hdrProf);
                            AppSessionTransaction.AddAssortmentMemberProfile(hdrProf);
                            // END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
                        }
						// Begin TT#3932 - stodd - Issues with Matrix View for headers with packs
                        else
                        {
                            if (assortmentType == eAssortmentType.Undefined)
                            {
                                AssortmentProfile asrtProfile = hdrProf.AssortmentProfile;
                                assortmentType = (eAssortmentType)asrtProfile.AsrtType;
                            }
                        }
						// End TT#3932 - stodd - Issues with Matrix View for headers with packs
                        

						hdrProf.GetAllocationComponents(aAsrtCubeGroup, dtHdrComponents);
						// BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
						//AllocationProfileList apl = (AllocationProfileList)AppSessionTransaction.GetMasterProfileList(eProfileType.Allocation);
						AllocationProfileList apl = (AllocationProfileList)AppSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember);
						// END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member

                        // Begin TT#1489 - RMatelic - Add Hierarchy levels above style as default displayable items
                        HierarchyNodeProfile styleHnp = SAB.HierarchyServerSession.GetNodeData(hdrProf.StyleHnRID);  
                        // End TT#1489
						
                        foreach (DataRow hdrRow in dtHdrComponents.Rows)
						{
							i = 0;
							rowParms = new object[aComponentTable.Columns.Count];
							rowParms[i++] = false;
							rowParms[i++] = true;

							foreach (AssortmentComponentVariableProfile varProf in vars.VariableProfileList)
							{
								if (varProf.Key == vars.Assortment.Key)
								{
									rowParms[i++] = this.Key;
									rowParms[i++] = this.HeaderID;
									rowParms[i++] = DBNull.Value;
								}
								else if (varProf.Key == vars.Placeholder.Key)
								{
									rowParms[i++] = hdrRow[vars.HeaderID.RIDColumnName];
                                    // if style is virtual use HeaderID; if real, use style text -  RMatelic
                                    //rowParms[i++] = hdrRow[vars.HeaderID.TextColumnName];
                                    HierarchyNodeProfile hnp_style = SAB.HierarchyServerSession.GetNodeData(hdrProf.StyleHnRID, false);
                                    if (hnp_style.IsVirtual && hnp_style.Purpose == ePurpose.Placeholder)
                                    {
                                        rowParms[i++] = hdrRow[vars.HeaderID.TextColumnName];
										// BEGIN TT#2402 - stodd - totaling incorrect when certain views are selected
										styleText = hdrRow[vars.HeaderID.TextColumnName].ToString();
										// END TT#2402 - stodd - totaling incorrect when certain views are selected
                                    }
                                    else
                                    {
                                        rowParms[i++] = hnp_style.LevelText;
										// BEGIN TT#2402 - stodd - totaling incorrect when certain views are selected
										styleText = hnp_style.LevelText;
										// END TT#2402 - stodd - totaling incorrect when certain views are selected
                                    }    
									rowParms[i++] = DBNull.Value;
								}
								else if (varProf.Key == vars.HeaderID.Key)
								{
									// Begin TT#1439 - stodd - post-receipt
									isDifferenceRow = true;
									// End TT#1439
									rowParms[i++] = int.MaxValue;
									rowParms[i++] = MIDText.GetTextOnly(eMIDTextCode.msg_as_PlaceholderBalance);
									rowParms[i++] = DBNull.Value;
								}
                                // Begin TT#1489 - RMatelic - Add Hierarchy levels above style as default displayable items
                                else if (varProf.Key < (int)eAssortmentComponentVariables.Assortment)
                                {
                                    NodeAncestorList nal = GetNodeAncestorList(hdrProf.StyleHnRID);
                                    // undo manipulated profile keys to get level 
                                    int hierLevel =  (int)eAssortmentComponentVariables.HierarchyLevel - 5 - varProf.Key;   
                                    foreach (NodeAncestorProfile nap in nal)
                                    {
                                        HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(nap.Key);
										// BEGIN TT#2150 - stodd - totals not showing in main matrix grid
										//if (hnp.Key == hdrProf.StyleHnRID)
										//{
										//    continue;
										//}
										//else 
										// END TT#2150 - stodd - totals not showing in main matrix grid
										if (hnp.HomeHierarchyLevel == hierLevel && hnp.HomeHierarchyRID == styleHnp.HomeHierarchyRID)
                                        {
											// BEGIN TT#2402 - stodd - totaling incorrect when certain views are selected
												rowParms[i++] = hnp.Key;
												rowParms[i++] = hnp.LevelText;
											// END TT#2402 - stodd - totaling incorrect when certain views are selected
                                            rowParms[i++] = DBNull.Value;
                                            break;
                                        }
                                    }
                                }
                                // End TT#1489
								else if (varProf.Key >= (int)eAssortmentComponentVariables.Characteristic)
								{
									charProfList = SAB.HierarchyServerSession.GetProductCharacteristics(hdrProf.StyleHnRID);
									charProf = (NodeCharProfile)charProfList.FindKey(varProf.Key - (int)eAssortmentComponentVariables.Characteristic);

									if (charProf != null)
									{
										rowParms[i++] = charProf.ProductCharValueRID;
										rowParms[i++] = charProf.ProductCharValue;
										rowParms[i++] = DBNull.Value;
									}
									else
									{
										rowParms[i++] = int.MaxValue;
										rowParms[i++] = DBNull.Value;
										rowParms[i++] = DBNull.Value;
									}
								}
								else if (varProf.Key == vars.Pack.Key)
								{
									rowParms[i++] = hdrRow[varProf.RIDColumnName];
									rowParms[i++] = hdrRow[varProf.TextColumnName];
									// Begin TT#866 - MD - stodd - quantities do not display unless header ID is selected (post-rec)
									// Adding the header to the pack name caused the Bulk total to be separated by header on the matrix even the amount was the 
									// total for the bulk of all headers
									string packId = Convert.ToString(hdrRow[varProf.TextColumnName]);
									if (packId.Contains("Bulk"))
									{
										rowParms[i++] = Convert.ToString(hdrRow[varProf.TextColumnName]);
									}
									else
									{
										rowParms[i++] = Convert.ToString(hdrRow[varProf.TextColumnName]) + " (" + Convert.ToString(hdrRow[vars.HeaderID.TextColumnName]) + ")";
									}
									// End TT#866 - MD - stodd - quantities do not display unless header ID is selected (post-rec)
								}
								// Begin TT#1227 - stodd *REMOVED for TT#1322*
								//else if (varProf.Key == vars.PlaceholderSeq.Key || varProf.Key == vars.HeaderSeq.Key)
								//{
								//    // Begin TT#1335 - stodd
								//    try
								//    {
								//        int parm = int.Parse(hdrRow[varProf.RIDColumnName].ToString());
								//        rowParms[i++] = parm.ToString("0000");
								//        rowParms[i++] = parm.ToString("0000");
								//    }
								//    catch
								//    {
								//        rowParms[i++] = hdrRow[varProf.RIDColumnName];
								//        rowParms[i++] = hdrRow[varProf.RIDColumnName];
								//    }
								//    rowParms[i++] = DBNull.Value;
								//    // End TT#1335 - stodd
								//}
								// End TT#1227 - stodd
								else
								{
									rowParms[i++] = hdrRow[varProf.RIDColumnName];
									rowParms[i++] = hdrRow[varProf.TextColumnName];
									rowParms[i++] = DBNull.Value;
								}
							}

							// Begin TT#1322 -s todd sorting
							// Always add these
							string placeholderSeq = string.Empty;	// TT#1438 - sorting augument exception
							try
							{
								int parm = int.Parse(hdrRow["PLACEHOLDERSEQ_RID"].ToString());
								placeholderSeq = parm.ToString("0000");	// TT#1438 - sorting augument exception
								rowParms[i++] = parm.ToString("0000");
								rowParms[i++] = parm.ToString("0000");
							}
							catch
							{
								rowParms[i++] = hdrRow["PLACEHOLDERSEQ_RID"];
								rowParms[i++] = hdrRow["PLACEHOLDERSEQ_RID"];
							}
							rowParms[i++] = DBNull.Value;

							try
							{
								int parm = int.Parse(hdrRow["HEADERSEQ_RID"].ToString());
								rowParms[i++] = placeholderSeq + parm.ToString("0000");	// TT#1438 - sorting augument exception
								// Begin TT#1461 - stodd - multing selecting assortments
								rowParms[i++] = this.Key.ToString("0000000000") + placeholderSeq + parm.ToString("0000");	// TT#1438 - sorting augument exception
								// End TT#1461 - stodd - multing selecting assortments
							}
							catch
							{
								rowParms[i++] = hdrRow["HEADERSEQ_RID"];
								rowParms[i++] = hdrRow["HEADERSEQ_RID"];
							}
							rowParms[i++] = DBNull.Value;
							// End TT#1322 -s todd sorting

                            rowParms[i++] = hdrRow["COLORSEQ"]; //TT#664 - MD - DOConnell - Placeholder colors display in reverse order on the Content Tab

							// Begin TT#1439 - stodd - dont show diff row on post reciept
							//if ((eAssortmentType)AsrtType == eAssortmentType.PostReceipt
							//    && isDifferenceRow)
							//{
							//    // Don't write difference row
							//}
							//else
							{
								aComponentTable.Rows.Add(rowParms);
							}
							// End TT#1439 - stodd 

							hashKey = Convert.ToString(hdrRow[vars.HeaderID.TextColumnName]) + Convert.ToString(hdrRow[vars.Pack.TextColumnName]);
							packHash[hashKey] = new PackColorHashEntry(
								Convert.ToInt32(hdrRow[vars.Pack.RIDColumnName]),
								Convert.ToString(hdrRow[vars.Pack.TextColumnName]),
								Convert.ToString(hdrRow[vars.Pack.TextColumnName]) + " (" + Convert.ToString(hdrRow[vars.HeaderID.TextColumnName]) + ")");

							hashKey = Convert.ToString(hdrRow[vars.HeaderID.TextColumnName]) + Convert.ToString(hdrRow[vars.Pack.TextColumnName]) + Convert.ToString(hdrRow[vars.Color.TextColumnName]);
							packColorHash[hashKey] = new PackColorHashEntry(
								Convert.ToInt32(hdrRow[vars.Color.RIDColumnName]),
								Convert.ToString(hdrRow[vars.Color.TextColumnName]),
								String.Empty);
						}
					}
				}

				foreach (DataRow assrtRow in dtAssrtComponents.Rows)
				{
					if ((eHeaderType)Convert.ToInt32(assrtRow["DISPLAY_TYPE"]) != eHeaderType.Placeholder
                        && (eHeaderType)Convert.ToInt32(assrtRow["DISPLAY_TYPE"]) != eHeaderType.Assortment) //TT#873-MD-DOConnell - System Exception: Header does not have Placeholder ID - when trying to add placeholders to an Assortment.
					{
						dtHdrComponents.Clear();
						// BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
						//hdrProf = AppSessionTransaction.GetAllocationProfile(Convert.ToInt32(assrtRow["HDR_RID"]));
						hdrProf = AppSessionTransaction.GetAssortmentMemberProfile(Convert.ToInt32(assrtRow["HDR_RID"]));
						// END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member

						if (hdrProf == null)
						{
                            hdrProf = new AllocationProfile(AppSessionTransaction, string.Empty, Convert.ToInt32(assrtRow["HDR_RID"]), Session);
							// BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
							//AppSessionTransaction.AddAllocationProfile(hdrProf);
							AppSessionTransaction.AddAssortmentMemberProfile(hdrProf);
							// END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
						}
						// Begin TT#3932 - stodd - Issues with Matrix View for headers with packs
                        else
                        {
                            if (assortmentType == eAssortmentType.Undefined)
                            {
                                AssortmentProfile asrtProfile = hdrProf.AssortmentProfile;
                                assortmentType = (eAssortmentType)asrtProfile.AsrtType;
                            }
                        }
						// End TT#3932 - stodd - Issues with Matrix View for headers with packs

						hdrProf.GetAllocationComponents(aAsrtCubeGroup, dtHdrComponents);
						// BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
						//AllocationProfileList apl = (AllocationProfileList)AppSessionTransaction.GetMasterProfileList(eProfileType.Allocation);
						AllocationProfileList apl = (AllocationProfileList)AppSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember);
						// END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member

                        // Begin TT#1489 - RMatelic - Add Hierarchy levels above style as default displayable items
                        HierarchyNodeProfile styleHnp = SAB.HierarchyServerSession.GetNodeData(hdrProf.StyleHnRID);
                        // End TT#1489

						foreach (DataRow hdrRow in dtHdrComponents.Rows)
						{
							if (assrtRow["PLACEHOLDER_RID"] != DBNull.Value)
							{
								// BEGIN TT#771-MD - Stodd - null reference exception
								//plchldrProf = AppSessionTransaction.GetAllocationProfile(Convert.ToInt32(assrtRow["PLACEHOLDER_RID"]));
								plchldrProf = AppSessionTransaction.GetAssortmentMemberProfile(Convert.ToInt32(assrtRow["PLACEHOLDER_RID"]));
								// END TT#771-MD - Stodd - null reference exception
                                HierarchyNodeProfile ph_style = SAB.HierarchyServerSession.GetNodeData(plchldrProf.StyleHnRID, false);
								i = 0;
								rowParms = new object[aComponentTable.Columns.Count];
								rowParms[i++] = false;
								rowParms[i++] = false;

								foreach (AssortmentComponentVariableProfile varProf in vars.VariableProfileList)
								{
									if (varProf.Key == vars.Assortment.Key)
									{
										rowParms[i++] = this.Key;
										rowParms[i++] = this.HeaderID;
										rowParms[i++] = DBNull.Value;
									}
									else if (varProf.Key == vars.Placeholder.Key)
									{
										rowParms[i++] = plchldrProf.Key;
                                        // if style is virtual use HeaderID; if real, use style text -  RMatelic
                                        //rowParms[i++] = plchldrProf.HeaderID;
                                        if (ph_style.IsVirtual && ph_style.Purpose == ePurpose.Placeholder)
                                        {
                                            rowParms[i++] = plchldrProf.HeaderID;
											// BEGIN TT#2402 - stodd - totaling incorrect when certain views are selected
											styleText = plchldrProf.HeaderID;
											// END TT#2402 - stodd - totaling incorrect when certain views are selected
                                        }
                                        else
                                        {
                                            rowParms[i++] = ph_style.LevelText;
											// BEGIN TT#2402 - stodd - totaling incorrect when certain views are selected
											styleText = ph_style.LevelText;
											// END TT#2402 - stodd - totaling incorrect when certain views are selected
                                        }    
										rowParms[i++] = DBNull.Value;
									}
                                    // Begin TT#1489 - RMatelic - Add Hierarchy levels above style as default displayable items
                                    else if (varProf.Key < (int)eAssortmentComponentVariables.Assortment)
                                    {
                                        NodeAncestorList nal = GetNodeAncestorList(hdrProf.StyleHnRID);
                                        // undo manipulated profile keys to get level
                                        int hierLevel = (int)eAssortmentComponentVariables.HierarchyLevel - 5 - varProf.Key;
                                        foreach (NodeAncestorProfile nap in nal)
                                        {
                                            HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(nap.Key);
											// BEGIN TT#2150 - stodd - totals not showing in main matrix grid
											//if (hnp.Key == hdrProf.StyleHnRID)
											//{
											//    continue;
											//}
											//else 
											// END TT#2150 - stodd - totals not showing in main matrix grid
											if (hnp.HomeHierarchyLevel == hierLevel && hnp.HomeHierarchyRID == styleHnp.HomeHierarchyRID)
                                            {
												// BEGIN TT#2402 - stodd - totaling incorrect when certain views are selected
												rowParms[i++] = hnp.Key;
												rowParms[i++] = hnp.LevelText;
												// END TT#2402 - stodd - totaling incorrect when certain views are selected						
                                                rowParms[i++] = DBNull.Value;
                                                break;
                                            }
                                        }
                                    }
                                    // End TT#1489
                                    else if (varProf.Key >= (int)eAssortmentComponentVariables.Characteristic)
                                    {
                                        charProfList = SAB.HierarchyServerSession.GetProductCharacteristics(hdrProf.StyleHnRID);
                                        charProf = (NodeCharProfile)charProfList.FindKey(varProf.Key - (int)eAssortmentComponentVariables.Characteristic);

										if (charProf != null)
										{
											rowParms[i++] = charProf.ProductCharValueRID;
											rowParms[i++] = charProf.ProductCharValue;
											rowParms[i++] = DBNull.Value;
										}
										else
										{
											rowParms[i++] = int.MaxValue;
											rowParms[i++] = DBNull.Value;
											rowParms[i++] = DBNull.Value;
										}
									}
									else if (varProf.Key == vars.Pack.Key)
									{
										rowParms[i++] = hdrRow[varProf.RIDColumnName];
										rowParms[i++] = hdrRow[varProf.TextColumnName];

										// Begin TT#3932 - stodd - Issues with Matrix View for headers with packs
                                        string packId = Convert.ToString(hdrRow[varProf.TextColumnName]);
                                        if (packId.Contains("Bulk"))
                                        {
                                            rowParms[i++] = Convert.ToString(hdrRow[varProf.TextColumnName]);
                                        }
                                        else
                                        {
                                            if (assortmentType == eAssortmentType.GroupAllocation || assortmentType == eAssortmentType.PostReceipt)
                                            {
                                                rowParms[i++] = Convert.ToString(hdrRow[varProf.TextColumnName]) + " (" + hdrProf.HeaderID + ")";
                                            }
                                            else
                                            {
                                                rowParms[i++] = Convert.ToString(hdrRow[varProf.TextColumnName]) + " (" + plchldrProf.HeaderID + ")";
                                            }
                                        }
										// End TT#3932 - stodd - Issues with Matrix View for headers with packs
									}
									// Begin TT#1227 - stodd *REMOVED for TT#1322*
									//else if (varProf.Key == vars.PlaceholderSeq.Key || varProf.Key == vars.HeaderSeq.Key)
									//{
									//    rowParms[i++] = hdrRow[varProf.RIDColumnName];
									//    rowParms[i++] = hdrRow[varProf.RIDColumnName];
									//    rowParms[i++] = DBNull.Value;
									//}
									// End TT#1227 - stodd
									else
									{
										rowParms[i++] = hdrRow[varProf.RIDColumnName];
										rowParms[i++] = hdrRow[varProf.TextColumnName];
										rowParms[i++] = DBNull.Value;
									}
								}

								// Begin TT#1322 -s todd sorting
								// Always add these
								string placeholderSeq = string.Empty;	// TT#1438 - sorting augument exception
								try
								{
									int parm = int.Parse(hdrRow["PLACEHOLDERSEQ_RID"].ToString());
									placeholderSeq = parm.ToString("0000");	// TT#1438 - sorting augument exception
									rowParms[i++] = parm.ToString("0000");
									rowParms[i++] = parm.ToString("0000");
								}
								catch
								{
									rowParms[i++] = hdrRow["PLACEHOLDERSEQ_RID"];
									rowParms[i++] = hdrRow["PLACEHOLDERSEQ_RID"];
								}
								rowParms[i++] = DBNull.Value;

								try
								{
									int parm = int.Parse(hdrRow["HEADERSEQ_RID"].ToString());
									rowParms[i++] = placeholderSeq + parm.ToString("0000");	// TT#1438 - sorting augument exception
									// Begin TT#1461 - stodd - multing selecting assortments
									rowParms[i++] = this.Key.ToString("0000000000") + placeholderSeq + parm.ToString("0000");	// TT#1438 - sorting augument exception
									// End TT#1461 - stodd - multing selecting assortments
								}
								catch
								{
									rowParms[i++] = hdrRow["HEADERSEQ_RID"];
									rowParms[i++] = hdrRow["HEADERSEQ_RID"];
								}
								rowParms[i++] = DBNull.Value;
								// End TT#1322 -s todd sorting

                                rowParms[i++] = hdrRow["COLORSEQ"]; //TT#664 - MD - DOConnell - Placeholder colors display in reverse order on the Content Tab

								aComponentTable.Rows.Add(rowParms);

								hashKey = plchldrProf.HeaderID + Convert.ToString(hdrRow[vars.Pack.TextColumnName]);
								packHashEntry = (PackColorHashEntry)packHash[hashKey];

								if (packHashEntry == null)
								{
									packHashEntry = new PackColorHashEntry(
										Convert.ToInt32(hdrRow[vars.Pack.RIDColumnName]),
										Convert.ToString(hdrRow[vars.Pack.TextColumnName]),
										Convert.ToString(hdrRow[vars.Pack.TextColumnName]) + " (" + plchldrProf.HeaderID + ")");
									packHash[hashKey] = packHashEntry;
								}

								hashKey = plchldrProf.HeaderID + packHashEntry.ID + Convert.ToString(hdrRow[vars.Color.TextColumnName]);
								packColorHashEntry = (PackColorHashEntry)packColorHash[hashKey];

								if (packColorHashEntry == null)
								{
									packColorHashEntry = new PackColorHashEntry(
										Convert.ToInt32(hdrRow[vars.Color.RIDColumnName]),
										Convert.ToString(hdrRow[vars.Color.TextColumnName]),
										string.Empty);
									packHash[hashKey] = packColorHashEntry;

									i = 0;
									rowParms = new object[aComponentTable.Columns.Count];
									rowParms[i++] = true;
									rowParms[i++] = true;

									foreach (AssortmentComponentVariableProfile varProf in vars.VariableProfileList)
									{
										if (varProf.Key == vars.Assortment.Key)
										{
											rowParms[i++] = this.Key;
											rowParms[i++] = this.HeaderID;
											rowParms[i++] = DBNull.Value;
										}
										else if (varProf.Key == vars.Placeholder.Key)
										{
											rowParms[i++] = plchldrProf.Key;
                                            // if style is virtual use HeaderID; if real, use style text -  RMatelic
                                            //rowParms[i++] = plchldrProf.HeaderID;
                                            if (ph_style.IsVirtual && ph_style.Purpose == ePurpose.Placeholder)
                                            {
                                                rowParms[i++] = plchldrProf.HeaderID;
												// BEGIN TT#2402 - stodd - totaling incorrect when certain views are selected
												styleText = plchldrProf.HeaderID;
												// END TT#2402 - stodd - totaling incorrect when certain views are selected
                                            }
                                            else
                                            {
                                                rowParms[i++] = ph_style.LevelText;
												// BEGIN TT#2402 - stodd - totaling incorrect when certain views are selected
												styleText = ph_style.LevelText;
												// END TT#2402 - stodd - totaling incorrect when certain views are selected
                                            }    
											rowParms[i++] = DBNull.Value;
										}
										else if (varProf.Key == vars.HeaderID.Key)
										{
											rowParms[i++] = int.MaxValue;
											rowParms[i++] = MIDText.GetTextOnly(eMIDTextCode.msg_as_PlaceholderBalance);
											rowParms[i++] = DBNull.Value;
										}
                                        // Begin TT#1489 - RMatelic - Add Hierarchy levels above style as default displayable items
                                        else if (varProf.Key < (int)eAssortmentComponentVariables.Assortment)
                                        {
                                            NodeAncestorList nal = GetNodeAncestorList(hdrProf.StyleHnRID);
                                            // undo manipulated profile keys to get level
                                            int hierLevel = (int)eAssortmentComponentVariables.HierarchyLevel - 5 - varProf.Key;
                                            foreach (NodeAncestorProfile nap in nal)
                                            {
                                                HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(nap.Key);
												// BEGIN TT#2150 - stodd - totals not showing in main matrix grid
												//if (hnp.Key == hdrProf.StyleHnRID)
												//{
												//    continue;
												//}
												//else 
												// END TT#2150 - stodd - totals not showing in main matrix grid
												if (hnp.HomeHierarchyLevel == hierLevel && hnp.HomeHierarchyRID == styleHnp.HomeHierarchyRID)
                                                {
                                                    // BEGIN TT#2402 - stodd - totaling incorrect when certain views are selected
													rowParms[i++] = hnp.Key;
													rowParms[i++] = hnp.LevelText;
													// END TT#2402 - stodd - totaling incorrect when certain views are selected
                                                    rowParms[i++] = DBNull.Value;
                                                    break;
                                                }
                                            }
                                        }
                                        // End TT#1489
										else if (varProf.Key >= (int)eAssortmentComponentVariables.Characteristic)
										{
											charProfList = SAB.HierarchyServerSession.GetProductCharacteristics(hdrProf.StyleHnRID);
											charProf = (NodeCharProfile)charProfList.FindKey(varProf.Key - (int)eAssortmentComponentVariables.Characteristic);

											if (charProf != null)
											{
												rowParms[i++] = charProf.ProductCharValueRID;
												rowParms[i++] = charProf.ProductCharValue;
												rowParms[i++] = DBNull.Value;
											}
											else
											{
												rowParms[i++] = int.MaxValue;
												rowParms[i++] = DBNull.Value;
												rowParms[i++] = DBNull.Value;
											}
										}
										else if (varProf.Key == vars.Pack.Key)
										{
											rowParms[i++] = packHashEntry.RID;
											rowParms[i++] = packHashEntry.ID;
											rowParms[i++] = packHashEntry.AlternateID;
										}
										// Begin TT#1227 - stodd *REMOVED for TT#1322*
										//else if (varProf.Key == vars.PlaceholderSeq.Key || varProf.Key == vars.HeaderSeq.Key)
										//{
										//    rowParms[i++] = hdrRow[varProf.RIDColumnName];
										//    rowParms[i++] = hdrRow[varProf.RIDColumnName];
										//    rowParms[i++] = DBNull.Value;
										//}
										// End TT#1227 - stodd
										else
										{
											rowParms[i++] = hdrRow[varProf.RIDColumnName];
											rowParms[i++] = hdrRow[varProf.TextColumnName];
											rowParms[i++] = DBNull.Value;
										}
									}
                                    // Begin TT#1322 - RMatelic sorting
                                    // Always add these
									placeholderSeq = string.Empty;	// TT#1438 - sorting augument exception
                                    try
                                    {
                                        int parm = int.Parse(hdrRow["PLACEHOLDERSEQ_RID"].ToString());
										placeholderSeq = parm.ToString("0000");	// TT#1438 - sorting augument exception
                                        rowParms[i++] = parm.ToString("0000");
                                        rowParms[i++] = parm.ToString("0000");
                                    }
                                    catch
                                    {
                                        rowParms[i++] = hdrRow["PLACEHOLDERSEQ_RID"];
                                        rowParms[i++] = hdrRow["PLACEHOLDERSEQ_RID"];
                                    }
                                    rowParms[i++] = DBNull.Value;

                                    try
                                    {
                                        int parm = int.Parse(hdrRow["HEADERSEQ_RID"].ToString());
										rowParms[i++] = placeholderSeq + parm.ToString("0000");		// TT#1438 - sorting augument exception
										// Begin TT#1461 - stodd - multing selecting assortments
										rowParms[i++] = this.Key.ToString("0000000000") + placeholderSeq + parm.ToString("0000");		// TT#1438 - sorting augument exception
										// End TT#1461 - stodd - multing selecting assortments
                                    }
                                    catch
                                    {
                                        rowParms[i++] = hdrRow["HEADERSEQ_RID"];
                                        rowParms[i++] = hdrRow["HEADERSEQ_RID"];
                                    }
                                    rowParms[i++] = DBNull.Value;
                                    // End TT#1322 - RMatelic sorting

                                    rowParms[i++] = hdrRow["COLORSEQ"]; //TT#664 - MD - DOConnell - Placeholder colors display in reverse order on the Content Tab

                                    // Begin TT#2 - RMatelic - Assortment Planning-allocating header packs not totaling correctly
                                    //              commented next line which was adding what appears to be an unnecesssary row to the matrix tab 
                                    //              unsure as to why it may be needed
                                    //aComponentTable.Rows.Add(rowParms);
                                    // End TT#2
								}

								aPackColorXRef.AddXRefIdEntry(
									plchldrProf.Key,
									packHashEntry.RID,
									packColorHashEntry.RID,
									Convert.ToInt32(hdrRow[vars.HeaderID.RIDColumnName]),
									Convert.ToInt32(hdrRow[vars.Pack.RIDColumnName]),
									Convert.ToInt32(hdrRow[vars.Color.RIDColumnName]));
							}
							else
							{
								throw new Exception("Header does not have Placeholder ID");
							}
						}
					}
				}

				aComponentTable.AcceptChanges();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // begin TT#488 - MD - Jellis - Group Allocation
        new public void GetAllocationComponents(AssortmentCubeGroup aAsrtCubeGroup, DataTable aComponentTable)
        {
            throw new Exception("Method 'GetAllocationComponents' is not valid when profile is an Assortment");
        }
        // end TT#488 - MD - Jellis - Group Allocation

		// Begin TT#936 - MD - Prevent the saving of empty Group Allocations
        public int GetAssortmentHeaderCount()
        {
            int headerCount = 0;
            Header assortmentData = new Header();
            headerCount = assortmentData.GetAssortmentHeaderCount(Key);
            return headerCount;
        }
		// End TT#936 - MD - Prevent the saving of empty Group Allocations

        // Begin TT#1489 - RMatelic - Add Hierarchy levels above style as default displayable items
        private NodeAncestorList GetNodeAncestorList(int aStyleHnRID)
        {
            NodeAncestorList nal = null;
            try
            {
                if (_nodeAncestorList.ContainsKey(aStyleHnRID))
                {
                    nal = (NodeAncestorList)_nodeAncestorList[aStyleHnRID];
                }
                else
                {
                    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(aStyleHnRID);
                    nal = SAB.HierarchyServerSession.GetNodeAncestorList(aStyleHnRID, hnp.HomeHierarchyRID, eHierarchySearchType.HomeHierarchyOnly);
                    _nodeAncestorList.Add(aStyleHnRID, nal);
                }
                return nal;
            }
            catch
            {
                throw;
            }
        }
        // End TT#1489

		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		public void ClearBasisReader()
		{
			_basisReader = null;
		}

		//End TT#2 - JScott - Assortment Planning - Phase 2
		public List<int> GetHeaderRidList()
        {
            List<int> headerRidList = new List<int>();
			Header dlAssortment = new Header();
            DataTable dtAssrtComponents = dlAssortment.GetAssormentComponents(Key);

            foreach (DataRow assrtRow in dtAssrtComponents.Rows)
            {
                int hdrRid = Convert.ToInt32(assrtRow["HDR_RID"]);
                headerRidList.Add(hdrRid);
            }
            return headerRidList;
        }

        // begin TT#488 - MD - Jellis - Group Allocation (This property was overridden)
        ////BEGIN TT#774 - MD - DOConnell - Change how TotalUnitsToAllocate are calculated for an assortment profile so it is calculated as needed instead of maintained by the assortment review.
        //public int TotalUnitsToAllocate
        //{
        //    get
        //    {
        //        List<int> aHeaderRIDList = GetHeaderRidList();
        //        int unitsToAllocateTotal = 0;
        //        foreach (int headerRid in aHeaderRIDList)
        //        {
        //            AllocationHeaderProfile allocHeaderProfile = SAB.HeaderServerSession.GetHeaderData(headerRid, false, false, true);
        //            unitsToAllocateTotal = unitsToAllocateTotal + allocHeaderProfile.BulkUnitsToAllocate;
        //        }
        //        return unitsToAllocateTotal;
        //    }
        //}
        ////END TT#774 - MD - DOConnell - Change how TotalUnitsToAllocate are calculated for an assortment profile so it is calculated as needed instead of maintained by the assortment review.
        // end TT#488 - MD - Jellis - Group Allocation (this property was overridden)

		// BEGIN TT#488-MD - STodd - Group Allocation 
		public ProfileList GetAssortmentStoreGrades()
		{
			ProfileList outList;
			try
			{
				if (AssortmentStoreGradeList != null && AssortmentStoreGradeList.Count > 0)
					outList = AssortmentStoreGradeList;
				else
					outList = base.GetStoreGrades();
				return outList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		// END TT#488-MD - STodd - Group Allocation 

        // begin TT#488 - MD - Jellis - Group Allocation
        //override public ProfileList GetStoreGrades()
        //{
        //    ProfileList outList;
        //    //StoreGradeProfile strGrdProf;

        //    try
        //    {
        //        if (StoreGradeList != null && StoreGradeList.Count > 0)
        //            outList = StoreGradeList;
        //        else
        //            outList = base.GetStoreGrades();

        //        //outList = new ProfileList(eProfileType.StoreGrade);

        //        //strGrdProf = new StoreGradeProfile(0);
        //        //strGrdProf.StoreGrade = "A";
        //        //outList.Add(strGrdProf);

        //        //strGrdProf = new StoreGradeProfile(1);
        //        //strGrdProf.StoreGrade = "B";
        //        //outList.Add(strGrdProf);

        //        //strGrdProf = new StoreGradeProfile(2);
        //        //strGrdProf.StoreGrade = "C";
        //        //outList.Add(strGrdProf);

        //        //strGrdProf = new StoreGradeProfile(3);
        //        //strGrdProf.StoreGrade = "D";
        //        //outList.Add(strGrdProf);

        //        //strGrdProf = new StoreGradeProfile(4);
        //        //strGrdProf.StoreGrade = "E";
        //        //outList.Add(strGrdProf);

        //        return outList;
        //    }
        //    catch (Exception exc)
        //    {
        //        string message = exc.ToString();
        //        throw;
        //    }
        //}
        // end - TT#488 - MD - JEllis - Group Allocation

		override public DataTable GetSummaryInformation(AssortmentCubeGroup aAsrtCubeGroup, int aStoreGroupRID, bool reload)
		{
			DataTable dt;
			AssortmentViewSummaryVariables varList;
            //StoreGroupProfile strGrpProf;


			if (aStoreGroupRID != _lastSglRidUsedInSummary || reload)
			{
				_lastSglRidUsedInSummary = aStoreGroupRID;
				_assortmentSummaryProfile.BuildSummary(aStoreGroupRID);
			}

			try
			{
				dt = MIDEnvironment.CreateDataTable("Summary Table");
				varList = (AssortmentViewSummaryVariables)aAsrtCubeGroup.AssortmentComputations.AssortmentSummaryVariables;

				dt.Columns.Add(new DataColumn(varList.Units.DatabaseColumnName, typeof(int)));
				dt.Columns.Add(new DataColumn(varList.NumStores.DatabaseColumnName, typeof(int)));
				dt.Columns.Add(new DataColumn(varList.AvgStore.DatabaseColumnName, typeof(decimal)));
				dt.Columns.Add(new DataColumn(varList.AvgUnits.DatabaseColumnName, typeof(decimal)));
				dt.Columns.Add(new DataColumn(varList.Index.DatabaseColumnName, typeof(decimal)));
				dt.Columns.Add(new DataColumn(varList.Basis.DatabaseColumnName, typeof(int)));
				dt.Columns.Add(new DataColumn(varList.Need.DatabaseColumnName, typeof(int)));
				dt.Columns.Add(new DataColumn(varList.PctNeed.DatabaseColumnName, typeof(decimal)));
				dt.Columns.Add(new DataColumn(varList.Intransit.DatabaseColumnName, typeof(int)));
				dt.Columns.Add(new DataColumn(varList.OnHand.DatabaseColumnName, typeof(int)));	// TT#845-MD - Stodd - add OnHand to Summary
                dt.Columns.Add(new DataColumn(varList.VSWOnHand.DatabaseColumnName, typeof(int)));
                // Begin TT#1725 - RMatelic - "Hide" Committed
                //dt.Columns.Add(new DataColumn(varList.Committed.DatabaseColumnName, typeof(int)));		// TT#1224 - stodd - add comitted
                // End TT#1725
				dt.Columns.Add(new DataColumn("GROUP_LEVEL_RID", typeof(int)));
				dt.Columns.Add(new DataColumn("GRADE_RID", typeof(int)));

				AssortmentSummaryItemProfile asip = null;
				foreach (StoreGroupLevelListViewProfile strGrpLvlProf in StoreMgmt.StoreGroup_GetLevelListViewList(aStoreGroupRID)) //SAB.StoreServerSession.GetStoreGroupLevelListViewList(aStoreGroupRID))
				{
					foreach (Profile gradeProf in GetAssortmentStoreGrades())		// TT#488-MD - STodd - Group Allocation
					{
						asip = GetAssortmentSummary(_assortVariableNumber, strGrpLvlProf.Key, gradeProf.Key);
						dt.Rows.Add(new object[] {asip.TotalUnits, asip.NumberOfStores, asip.AverageStore, DBNull.Value, asip.Index, 
                            // Begin TT#1725 - RMatelic - "Hide" Committed
                            //asip.Basis, asip.Need, asip.PctNeed, asip.Intransit, asip.Committed, strGrpLvlProf.Key, gradeProf.Key });	// TT#1224 - stodd
							// BEGIN TT#845-MD - Stodd - add OnHand to Summary
                            asip.Basis, asip.Need, asip.PctNeed, asip.Intransit, asip.OnHand, asip.VSWOnHand, strGrpLvlProf.Key, gradeProf.Key });
							// END TT#845-MD - Stodd - add OnHand to Summary
                            // End TT#1725
					}
					asip = GetAssortmentSummary(_assortVariableNumber, strGrpLvlProf.Key, Include.Undefined);
					dt.Rows.Add(new object[] {asip.TotalUnits, asip.NumberOfStores, asip.AverageStore, DBNull.Value, asip.Index,
                            // Begin TT#1725 - RMatelic - "Hide" Committed
                            //asip.Basis, asip.Need, asip.PctNeed, asip.Intransit, asip.Committed, strGrpLvlProf.Key, Include.Undefined });		// TT#1224 - stodd
							// BEGIN TT#845-MD - Stodd - add OnHand to Summary
                            asip.Basis, asip.Need, asip.PctNeed, asip.Intransit,  asip.OnHand, asip.VSWOnHand, strGrpLvlProf.Key, Include.Undefined });
							// END TT#845-MD - Stodd - add OnHand to Summary
                            // End TT#1725
				}

				asip = GetAssortmentSummary(_assortVariableNumber, Include.Undefined, Include.Undefined);
				dt.Rows.Add(new object[] {asip.TotalUnits, asip.NumberOfStores, asip.AverageStore, DBNull.Value, asip.Index,
                            // Begin TT#1725 - RMatelic - "Hide" Committed
                            //asip.Basis, asip.Need, asip.PctNeed, asip.Intransit, asip.Committed, Include.Undefined, Include.Undefined });		// TT#1224 - stodd
							// BEGIN TT#845-MD - Stodd - add OnHand to Summary
                            asip.Basis, asip.Need, asip.PctNeed, asip.Intransit,  asip.OnHand, asip.VSWOnHand , Include.Undefined, Include.Undefined });
							// END TT#845-MD - Stodd - add OnHand to Summary
                            // End TT#1725

				dt.AcceptChanges();

				return dt;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//public int GetAssortmentSummaryUnits(int variableNumber, int setRid, int storeGrade)
		//{
		//    try
		//    {
		//        return _assortmentSummary.GetUnits(variableNumber, setRid, storeGrade);
		//    }
		//    catch
		//    {
		//        throw;
		//    }
		//}

		//public int GetAssortmentSummaryNumberOfStores(int variableNumber, int setRid, int storeGrade)
		//{
		//    try
		//    {
		//        return _assortmentSummary.GetNumberOfStores(variableNumber, setRid, storeGrade);
		//    }
		//    catch
		//    {
		//        throw;
		//    }
		//}

		//public int GetAssortmentSummaryAverageStore(int variableNumber, int setRid, int storeGrade)
		//{
		//    try
		//    {
		//        return _assortmentSummary.GetAverageStore(variableNumber, setRid, storeGrade);
		//    }
		//    catch
		//    {
		//        throw;
		//    }
		//}

		public AssortmentSummaryItemProfile GetAssortmentSummary(int variableNumber, int setRid, int storeGrade)
		{
			try
			{
				return _assortmentSummaryProfile.GetAssortmentSummary(variableNumber, setRid, storeGrade);
			}
			catch
			{
				throw;
			}
		}

		// Begin TT#1189-md - stodd - adding locking to group allocation
        public AssortmentSummaryItemProfile GetAssortmentSummary(int variableNumber, int setRid)
        {
            try
            {
                return _assortmentSummaryProfile.GetAssortmentSummary(variableNumber, setRid);
            }
            catch
            {
                throw;
            }
        }

		// BEGIN TT#2 - stodd - assortment
		public ProfileList GetStoresInSetGrade(int setRid, int storeGrade)
		{
			AssortmentSummaryItemProfile asip = GetAssortmentSummary(_assortVariableNumber, setRid, storeGrade);
			return asip.StoreList;
		}


        public ProfileList GetStoresInSet(int setRid)
        {
            AssortmentSummaryItemProfile asip = GetAssortmentSummary(_assortVariableNumber, setRid);
            return asip.StoreList;
        }

        public ProfileList GetAllStores()
        {
            return _assortmentSummaryProfile.StoreList;
        }
		// End TT#1189-md - stodd - adding locking to group allocation

		// END TT#2 - stodd - assortment

        // Begin TT#2109-MD - JSmith - Change the Beginning and Delivery Date in an Asst and the Delivery Date is incorrect.
        /// <summary>
        /// Reset the ship date for the header and for each store.
        /// </summary>
        override public void ResetShipDates()
        {
            base.ResetShipDates();
            AllocationProfile[] apList = AssortmentMembers;
            foreach (AllocationProfile ap in apList)
            {
                ap.ResetShipDates();
            }
        }
        // End TT#2109-MD - JSmith - Change the Beginning and Delivery Date in an Asst and the Delivery Date is incorrect.
        #endregion Assortment Specific Methods

        // begin TT#488 - MD - Jellis - Group Allocation
        #region Allocation Profile Method Overrides
        // begin TT#1022 - MD - Jellis - Size Review Group Members missing variables in Col Chooser
        #region Size Need
 
        /// <summary>
        /// Sets the size need method used to allocate sizes in the specified component
        /// </summary>
        /// <param name="aComponent">A Component</param>
        /// <param name="aSizeNeedMethod">SizeNeedMethod used to allocate the sizes on this component</param>
        override public void SetSizeNeedMethod(GeneralComponent aComponent, SizeNeedMethod aSizeNeedMethod) // TT#1022 - MD - Jellis - Size Review Col Chooser missing variables for Group Members
        {           
            if (aComponent.ComponentType == eComponentType.SpecificColor)
            {
                AllocationColorOrSizeComponent colorComponent = (AllocationColorOrSizeComponent)aComponent;
                HdrColorBin aColor = (HdrColorBin)BulkColors[colorComponent.ColorRID];
                SetSizeNeedMethod(aColor, aSizeNeedMethod);
            }
            else
            {
                AllocationProfile[] apList = AssortmentMembers;
                foreach (AllocationProfile ap in apList)
                {
                    ap.SetSizeNeedMethod(aComponent, aSizeNeedMethod);
                }
                base.SetSizeNeedMethod(aComponent, aSizeNeedMethod);
            }            
        }
 
        /// <summary>
        /// Sets the size need method used to allocate sizes in the specified component
        /// </summary>
        /// <param name="aColor">HdrColorBin that describes the color</param>
        /// <param name="aSizeNeedMethod">SizeNeedMethod used to allocate the sizes on this component</param>
        override public void SetSizeNeedMethod(HdrColorBin aColor, SizeNeedMethod aSizeNeedMethod)  // TT#1022 - MD - Jellis - Size Review Col Chooser missing variables for Group Members
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                ap.SetSizeNeedMethod(aColor.ColorCodeRID, aSizeNeedMethod);
            }
            base.SetSizeNeedMethod(aColor, aSizeNeedMethod);
        }

        /// <summary>
        /// Sets the size fill method used to allocate sizes in the specified component
        /// </summary>
        /// <param name="aColor">HdrColorBin that describes the color</param>
        /// <param name="aSizeFillMethod">SizeFillMethod used to allocate the sizes on this component</param>
        override public void SetSizeFillMethod(HdrColorBin aColor, FillSizeHolesMethod aSizeFillMethod) // TT#1022 - MD - Jellis - Size Review Col Chooser missing variables for Group Members
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                ap.SetSizeFillMethod(aColor.ColorCodeRID, aSizeFillMethod);
            }
            base.SetSizeFillMethod(aColor, aSizeFillMethod);
 
        }
        #endregion Size Need
        // end TT#1022 - MD - Jellis - Size Review Group Members missing variables in Col Chooser

        // begin TT#1063 - MD - Jellis - Group Allocation non-size Results change after Size Need
        #region BulkColor
        #region BulkColorUnitsAllocated
        /// <summary>
        /// Gets bulk color total units allocated to stores.
        /// </summary>
        /// <param name="aColor">HdrColorBin that describes the color</param>
        /// <returns>Total units allocated to stores for the specified bulk color</returns>
        override internal int GetColorUnitsAllocated(HdrColorBin aColor)  // TEMP NOW
        {
            int allocated = 0;
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                allocated += ap.GetColorUnitsAllocated(aColor.ColorCodeRID);
            }
            return allocated;
        }
        #endregion BulkColorUnitsAllocated

        #region BulkSize
        #region BulkSizeUnitsAllocated
        override internal int GetSizeUnitsAllocated(HdrSizeBin aSize)
        {
            int allocated = 0;
            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                HdrSizeBin sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aSize.Color.ColorCodeRID).ColorSizes[aSize.SizeCodeRID];
                if (sizeBin != null)
                {
                    allocated += ap.GetSizeUnitsAllocated(sizeBin);
                }
            }
            return allocated;
        }
        #endregion BulkSizeUnitsAllocated
        #endregion BulkSize
        #endregion BulkColor
        // end TT#1063 - MD - Jellis - Group Allocation non-size Results change after Size Need


        #region GetQtyToAllocate
        override public int GetQtyToAllocate(GeneralComponent aGeneralComponent)
        {
            int unitsToAllocate = 0;
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            // begin TT#993 - MD - Jellis - GA - Size Need gets Color Not Defined
            GeneralComponent colorComponent;
            GeneralComponent sizeComponent;
            int colorCodeRID;
            int sizeCodeRID;
            HdrColorBin colorBin;
            HdrSizeBin sizeBin;
            switch (aGeneralComponent.ComponentType)
            {
                case (eComponentType.ColorAndSize):
                    {
                        colorComponent = ((AllocationColorSizeComponent)aGeneralComponent).ColorComponent;
                        sizeComponent = ((AllocationColorSizeComponent)aGeneralComponent).SizeComponent;
                        switch (colorComponent.ComponentType)
                        {
                            case (eComponentType.AllColors):
                                {
                                    switch (sizeComponent.ComponentType)
                                    {
                                        case (eComponentType.AllSizes):
                                        {
                                            foreach (AllocationProfile ap in apList)
                                            {
                                                unitsToAllocate += ap.GetQtyToAllocate(aGeneralComponent);
                                            }
                                            break;
                                        }
                                        case (eComponentType.SpecificSize):
                                        {
                                            //sizeCodeRID = ((AllocationColorOrSizeComponent)aGeneralComponent).SizeRID;  // TT#1039 - MD - Jellis - Missing Size need
                                            sizeCodeRID = ((AllocationColorOrSizeComponent)sizeComponent).SizeRID;        // TT#1039 - MD - Jellis - Missing Size need
                                            foreach (AllocationProfile ap in apList)
                                            {
                                                foreach (HdrColorBin hcb in ap.BulkColors.Values)
                                                {
                                                    sizeBin = (HdrSizeBin)hcb.ColorSizes[sizeCodeRID];
                                                    if (sizeBin != null)
                                                    {
                                                        unitsToAllocate += sizeBin.SizeUnitsToAllocate;
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                        case (eComponentType.SpecificSizePrimaryDim):
                                        {
                                            //sizeCodeRID = ((AllocationColorOrSizeComponent)aGeneralComponent).PrimarySizeDimRID;  // TT#1039 - MD - Jellis - Missing Size need
                                            sizeCodeRID = ((AllocationColorOrSizeComponent)sizeComponent).PrimarySizeDimRID;        // TT#1039 - MD - Jellis - Missing Size need
                                            SizeCodeList sizeCodeList = AppSessionTransaction.GetSizeCodeByPrimaryDim(sizeCodeRID);
                                            foreach (AllocationProfile ap in apList)
                                            {
                                                foreach (HdrColorBin hcb in ap.BulkColors.Values)
                                                {
                                                    foreach (SizeCodeProfile scp in sizeCodeList)
                                                    {
                                                        sizeBin = (HdrSizeBin)hcb.ColorSizes[scp.Key];
                                                        if (sizeBin != null)
                                                        {
                                                            unitsToAllocate += sizeBin.SizeUnitsToAllocate;
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                        case (eComponentType.SpecificSizeSecondaryDim):
                                        {
                                            //sizeCodeRID = ((AllocationColorOrSizeComponent)aGeneralComponent).SecondarySizeDimRID;  // TT#1039 - MD - Jellis - Missing Size need
                                            sizeCodeRID = ((AllocationColorOrSizeComponent)sizeComponent).SecondarySizeDimRID;        // TT#1039 - MD - Jellis - Missing Size need
                                            SizeCodeList sizeCodeList = AppSessionTransaction.GetSizeCodeByPrimaryDim(sizeCodeRID);
                                            foreach (AllocationProfile ap in apList)
                                            {
                                                foreach (HdrColorBin hcb in ap.BulkColors.Values)
                                                {
                                                    foreach (SizeCodeProfile scp in sizeCodeList)
                                                    {
                                                        sizeBin = (HdrSizeBin)hcb.ColorSizes[scp.Key];
                                                        if (sizeBin != null)
                                                        {
                                                            unitsToAllocate += sizeBin.SizeUnitsToAllocate;
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    }
                                    break;
                                }
                            case (eComponentType.SpecificColor):
                                {
                                    colorCodeRID = ((AllocationColorOrSizeComponent)colorComponent).ColorRID;
                                    switch (sizeComponent.ComponentType)
                                    {
                                        case (eComponentType.AllSizes):
                                            {
                                                foreach (AllocationProfile ap in apList)
                                                {
                                                    colorBin = (HdrColorBin)ap.BulkColors[colorCodeRID];
                                                    if (colorBin != null)
                                                    {
                                                        foreach (HdrSizeBin hsb in colorBin.ColorSizes.Values)
                                                        {
                                                            unitsToAllocate += hsb.SizeUnitsToAllocate;
                                                        }
                                                    }
                                                }
                                                break;
                                            }
                                        case (eComponentType.SpecificSize):
                                            {
                                                //sizeCodeRID = ((AllocationColorOrSizeComponent)aGeneralComponent).SizeRID;  // TT#1039 - MD - Jellis - Missing Size Need
                                                sizeCodeRID = ((AllocationColorOrSizeComponent)sizeComponent).SizeRID; // TT#1039 - MD - Jellis - Missing Size need
                                                foreach (AllocationProfile ap in apList)
                                                {
                                                    colorBin = (HdrColorBin)ap.BulkColors[colorCodeRID];
                                                    if (colorBin != null)
                                                    {
                                                        sizeBin = (HdrSizeBin)colorBin.ColorSizes[sizeCodeRID];
                                                        if (sizeBin != null)
                                                        {
                                                            unitsToAllocate += sizeBin.SizeUnitsToAllocate;
                                                        }
                                                    }
                                                }
                                                break;
                                            }
                                        case (eComponentType.SpecificSizePrimaryDim):
                                            {
                                                //sizeCodeRID = ((AllocationColorOrSizeComponent)aGeneralComponent).PrimarySizeDimRID;  // TT#1039 - MD - Jellis - Missing Size need
                                                sizeCodeRID = ((AllocationColorOrSizeComponent)sizeComponent).PrimarySizeDimRID;        // TT#1039 - MD - Jellis - Missing Size Need
                                                SizeCodeList sizeCodeList = AppSessionTransaction.GetSizeCodeByPrimaryDim(sizeCodeRID);
                                                foreach (AllocationProfile ap in apList)
                                                {
                                                    colorBin = (HdrColorBin)ap.BulkColors[colorCodeRID];
                                                    if (colorBin != null)
                                                    {
                                                        foreach (SizeCodeProfile scp in sizeCodeList)
                                                        {
                                                            sizeBin = (HdrSizeBin)colorBin.ColorSizes[scp.Key];
                                                            if (sizeBin != null)
                                                            {
                                                                unitsToAllocate += sizeBin.SizeUnitsToAllocate;
                                                            }
                                                        }
                                                    }
                                                }
                                                break;
                                            }
                                        case (eComponentType.SpecificSizeSecondaryDim):
                                            {
                                                //sizeCodeRID = ((AllocationColorOrSizeComponent)aGeneralComponent).SecondarySizeDimRID;  // TT#1039 - MD - Jellis - Missing Size Need
                                                sizeCodeRID = ((AllocationColorOrSizeComponent)sizeComponent).SecondarySizeDimRID;        // TT#1039 - MD- Jellis - Missing Size Need
                                                SizeCodeList sizeCodeList = AppSessionTransaction.GetSizeCodeByPrimaryDim(sizeCodeRID);
                                                foreach (AllocationProfile ap in apList)
                                                {
                                                    colorBin = (HdrColorBin)ap.BulkColors[colorCodeRID];
                                                    if (colorBin != null)
                                                    {
                                                        foreach (SizeCodeProfile scp in sizeCodeList)
                                                        {
                                                            sizeBin = (HdrSizeBin)colorBin.ColorSizes[scp.Key];
                                                            if (sizeBin != null)
                                                            {
                                                                unitsToAllocate += sizeBin.SizeUnitsToAllocate;
                                                            }
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
                                    throw new MIDException(eErrorLevel.severe,
                                        (int)(eMIDTextCode.msg_al_UnknownComponentType),
                                        Session.Audit.GetText(eMIDTextCode.msg_al_UnknownComponentType, false));
                                }
                        }
                        break;
                    }
                case (eComponentType.SpecificColor):
                    {
                        colorCodeRID = ((AllocationColorOrSizeComponent)aGeneralComponent).ColorRID;
                        foreach (AllocationProfile ap in apList)
                        {
                            colorBin = (HdrColorBin)ap.BulkColors[colorCodeRID];
                            if (colorBin != null)
                            {
                                unitsToAllocate += colorBin.ColorUnitsToAllocate;
                            }
                        }
                        break;
                    }
                case (eComponentType.SpecificSize):
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)(eMIDTextCode.msg_al_SizeRequestRequiresColorAndSize),
                            Session.Audit.GetText(eMIDTextCode.msg_al_SizeRequestRequiresColorAndSize, false));
                    }
                case (eComponentType.SpecificPack):
                    {
                        string packName = ((AllocationPackComponent)aGeneralComponent).PackName;
                        PackHdr ph = (PackHdr)Packs[packName];
                        if (ph != null)
                        {
                            unitsToAllocate = ph.UnitsToAllocate;
                        }
                        break;
                    }
                case (eComponentType.SpecificSizePrimaryDim):
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)(eMIDTextCode.msg_al_SizeRequestRequiresColorAndSize),
                            Session.Audit.GetText(eMIDTextCode.msg_al_SizeRequestRequiresColorAndSize, false));
                    }
                case (eComponentType.SpecificSizeSecondaryDim):
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)(eMIDTextCode.msg_al_SizeRequestRequiresColorAndSize),
                            Session.Audit.GetText(eMIDTextCode.msg_al_SizeRequestRequiresColorAndSize, false));
                    }
                default:
                    {
                        foreach (AllocationProfile ap in apList)
                        {
                            unitsToAllocate += ap.GetQtyToAllocate(aGeneralComponent);
                        }
                        break;
                    }
            }
            //foreach (AllocationProfile ap in apList)
            //{
            //    unitsToAllocate += ap.GetQtyToAllocate(aGeneralComponent);
            //}
            // end TT#993 - MD - Jellis - GA - Size Need gets Color Not Defined
            return unitsToAllocate;
        }
        #endregion GetQtyToAllocate

        #region GetQtyAllocated
        override public int  GetQtyAllocated(GeneralComponent aGeneralComponent)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            int allocated = 0;
            // begin TT#993 - MD - Jellis - GA - Size Need gets Color Not Defined
            GeneralComponent colorComponent;
            GeneralComponent sizeComponent;
            int colorCodeRID;
            int sizeCodeRID;
            HdrColorBin colorBin;
            HdrSizeBin sizeBin;
            switch (aGeneralComponent.ComponentType)
            {
                case (eComponentType.ColorAndSize):
                    {
                        colorComponent = ((AllocationColorSizeComponent)aGeneralComponent).ColorComponent;
                        sizeComponent = ((AllocationColorSizeComponent)aGeneralComponent).SizeComponent;
                        switch (colorComponent.ComponentType)
                        {
                            case (eComponentType.AllColors):
                                {
                                    switch (sizeComponent.ComponentType)
                                    {
                                        case (eComponentType.AllSizes):
                                            {
                                                foreach (AllocationProfile ap in apList)
                                                {
                                                    allocated += ap.GetQtyAllocated(aGeneralComponent);
                                                }
                                                break;
                                            }
                                        case (eComponentType.SpecificSize):
                                            {
                                                //sizeCodeRID = ((AllocationColorOrSizeComponent)aGeneralComponent).SizeRID; // TT#1039 - MD - Jellis - Missing Size need
                                                sizeCodeRID = ((AllocationColorOrSizeComponent)sizeComponent).SizeRID;       // TT#1039 - MD- Jellis - Missing Size Need
                                                foreach (AllocationProfile ap in apList)
                                                {
                                                    foreach (HdrColorBin hcb in ap.BulkColors.Values)
                                                    {
                                                        sizeBin = (HdrSizeBin)hcb.ColorSizes[sizeCodeRID];
                                                        if (sizeBin != null)
                                                        {
                                                            allocated += sizeBin.SizeUnitsAllocated;
                                                        }
                                                    }
                                                }
                                                break;
                                            }
                                        case (eComponentType.SpecificSizePrimaryDim):
                                            {
                                                //sizeCodeRID = ((AllocationColorOrSizeComponent)aGeneralComponent).PrimarySizeDimRID;  // TT#1039 - MD - Jellis - Missing Size need
                                                sizeCodeRID = ((AllocationColorOrSizeComponent)sizeComponent).PrimarySizeDimRID;        // TT#1039 - MD - Jellis - Missing Size Need
                                                SizeCodeList sizeCodeList = AppSessionTransaction.GetSizeCodeByPrimaryDim(sizeCodeRID);
                                                foreach (AllocationProfile ap in apList)
                                                {
                                                    foreach (HdrColorBin hcb in ap.BulkColors.Values)
                                                    {
                                                        foreach (SizeCodeProfile scp in sizeCodeList)
                                                        {
                                                            sizeBin = (HdrSizeBin)hcb.ColorSizes[scp.Key];
                                                            if (sizeBin != null)
                                                            {
                                                                allocated += sizeBin.SizeUnitsAllocated;
                                                            }
                                                        }
                                                    }
                                                }
                                                break;
                                            }
                                        case (eComponentType.SpecificSizeSecondaryDim):
                                            {
                                                //sizeCodeRID = ((AllocationColorOrSizeComponent)aGeneralComponent).SecondarySizeDimRID;   // TT#1039 - MD - Jellis - Missing Size need
                                                sizeCodeRID = ((AllocationColorOrSizeComponent)sizeComponent).SecondarySizeDimRID;         // TT#1039 - MD _ Jellis - Missing Size Need
                                                SizeCodeList sizeCodeList = AppSessionTransaction.GetSizeCodeByPrimaryDim(sizeCodeRID);
                                                foreach (AllocationProfile ap in apList)
                                                {
                                                    foreach (HdrColorBin hcb in ap.BulkColors.Values)
                                                    {
                                                        foreach (SizeCodeProfile scp in sizeCodeList)
                                                        {
                                                            sizeBin = (HdrSizeBin)hcb.ColorSizes[scp.Key];
                                                            if (sizeBin != null)
                                                            {
                                                                allocated += sizeBin.SizeUnitsAllocated;
                                                            }
                                                        }
                                                    }
                                                }
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case (eComponentType.SpecificColor):
                                {
                                    colorCodeRID = ((AllocationColorOrSizeComponent)colorComponent).ColorRID;
                                    switch (sizeComponent.ComponentType)
                                    {
                                        case (eComponentType.AllSizes):
                                            {
                                                foreach (AllocationProfile ap in apList)
                                                {
                                                    colorBin = (HdrColorBin)ap.BulkColors[colorCodeRID];
                                                    if (colorBin != null)
                                                    {
                                                        foreach (HdrSizeBin hsb in colorBin.ColorSizes.Values)
                                                        {
                                                            allocated += hsb.SizeUnitsAllocated;
                                                        }
                                                    }
                                                }
                                                break;
                                            }
                                        case (eComponentType.SpecificSize):
                                            {
                                                //sizeCodeRID = ((AllocationColorOrSizeComponent)aGeneralComponent).SizeRID;  // TT#1039 - MD - Jellis - Missing SIze Need
                                                sizeCodeRID = ((AllocationColorOrSizeComponent)sizeComponent).SizeRID;        // TT#1039 - MD - Jellis - Missing Size Need
                                                foreach (AllocationProfile ap in apList)
                                                {
                                                    colorBin = (HdrColorBin)ap.BulkColors[colorCodeRID];
                                                    if (colorBin != null)
                                                    {
                                                        sizeBin = (HdrSizeBin)colorBin.ColorSizes[sizeCodeRID];
                                                        if (sizeBin != null)
                                                        {
                                                            allocated += sizeBin.SizeUnitsAllocated;
                                                        }
                                                    }
                                                }
                                                break;
                                            }
                                        case (eComponentType.SpecificSizePrimaryDim):
                                            {
                                                //sizeCodeRID = ((AllocationColorOrSizeComponent)aGeneralComponent).PrimarySizeDimRID; // TT#1039 - MD- Jellis - Missing Size Need
                                                sizeCodeRID = ((AllocationColorOrSizeComponent)sizeComponent).PrimarySizeDimRID;        // TT#1039 - MD - Jellis - Missing Size need
                                                SizeCodeList sizeCodeList = AppSessionTransaction.GetSizeCodeByPrimaryDim(sizeCodeRID);
                                                foreach (AllocationProfile ap in apList)
                                                {
                                                    colorBin = (HdrColorBin)ap.BulkColors[colorCodeRID];
                                                    if (colorBin != null)
                                                    {
                                                        foreach (SizeCodeProfile scp in sizeCodeList)
                                                        {
                                                            sizeBin = (HdrSizeBin)colorBin.ColorSizes[scp.Key];
                                                            if (sizeBin != null)
                                                            {
                                                                allocated += sizeBin.SizeUnitsAllocated;
                                                            }
                                                        }
                                                    }
                                                }
                                                break;
                                            }
                                        case (eComponentType.SpecificSizeSecondaryDim):
                                            {
                                                //sizeCodeRID = ((AllocationColorOrSizeComponent)aGeneralComponent).SecondarySizeDimRID; // TT#1039 - MD - Jellis - Missing Size Need
                                                sizeCodeRID = ((AllocationColorOrSizeComponent)sizeComponent).SecondarySizeDimRID;       // TT#1039 - MD - Jellis - Missing Size Need
                                                SizeCodeList sizeCodeList = AppSessionTransaction.GetSizeCodeByPrimaryDim(sizeCodeRID);
                                                foreach (AllocationProfile ap in apList)
                                                {
                                                    colorBin = (HdrColorBin)ap.BulkColors[colorCodeRID];
                                                    if (colorBin != null)
                                                    {
                                                        foreach (SizeCodeProfile scp in sizeCodeList)
                                                        {
                                                            sizeBin = (HdrSizeBin)colorBin.ColorSizes[scp.Key];
                                                            if (sizeBin != null)
                                                            {
                                                                allocated += sizeBin.SizeUnitsAllocated;
                                                            }
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
                                    throw new MIDException(eErrorLevel.severe,
                                        (int)(eMIDTextCode.msg_al_UnknownComponentType),
                                        Session.Audit.GetText(eMIDTextCode.msg_al_UnknownComponentType, false));
                                }
                        }
                        break;
                    }
                case (eComponentType.SpecificColor):
                    {
                        colorCodeRID = ((AllocationColorOrSizeComponent)aGeneralComponent).ColorRID;
                        foreach (AllocationProfile ap in apList)
                        {
                            colorBin = (HdrColorBin)ap.BulkColors[colorCodeRID];
                            if (colorBin != null)
                            {
                                allocated += colorBin.ColorUnitsAllocated;
                            }
                        }
                        break;
                    }
                case (eComponentType.SpecificSize):
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)(eMIDTextCode.msg_al_SizeRequestRequiresColorAndSize),
                            Session.Audit.GetText(eMIDTextCode.msg_al_SizeRequestRequiresColorAndSize, false));
                    }
                case (eComponentType.SpecificPack):
                    {
                        string packName = ((AllocationPackComponent)aGeneralComponent).PackName;
                        PackHdr ph = (PackHdr)Packs[packName];
                        if (ph != null)
                        {
                            allocated = ph.UnitsAllocated;
                        }
                        break;
                    }
                case (eComponentType.SpecificSizePrimaryDim):
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)(eMIDTextCode.msg_al_SizeRequestRequiresColorAndSize),
                            Session.Audit.GetText(eMIDTextCode.msg_al_SizeRequestRequiresColorAndSize, false));
                    }
                case (eComponentType.SpecificSizeSecondaryDim):
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)(eMIDTextCode.msg_al_SizeRequestRequiresColorAndSize),
                            Session.Audit.GetText(eMIDTextCode.msg_al_SizeRequestRequiresColorAndSize, false));
                    }
                default:
                    {
                        foreach (AllocationProfile ap in apList)
                        {
                            allocated += ap.GetQtyAllocated(aGeneralComponent);
                        }
                        break;
                    }
            }
            //foreach (AllocationProfile ap in apList)
            //{
            //    allocated += ap.GetQtyAllocated(aGeneralComponent);
            //}
            // end TT#993 - MD - Jellis - GA - Size Need gets Color Not Defined
            return allocated;
        }
        #endregion GetQtyAllocated

        // begin TT#488 - MD - Jellis - Group Allocation
        int _assortmentActionCount;
        public override bool Action(eAllocationMethodType aAllocationAction, GeneralComponent aComponent, double aTolerancePercent, int aStoreFilterRID, bool aWriteToDB)
        {

            bool actionSuccess = true;
            try
            {
                AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error // TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                if (!ProcessingGroupAllocation)
                {
                    _assortmentActionCount = 1;
                    AssortmentBeginTransaction(true);
                    foreach (AllocationProfile ap in apList)
                    {
                        AppSessionTransaction.HoldHeaderAllocation(ap);
                    }
                    ResetTempLocks(false);  
                }
                else
                {
                    _assortmentActionCount++;
                }
                if (aAllocationAction == eAllocationMethodType.Release)
                {
                    actionSuccess = ProcessRelease();                   
                }
                else
                {
                    bool groupAction = Enum.IsDefined(typeof(eGroupAllocationActionType), (int)aAllocationAction);
                    if (groupAction)
                    {
                        actionSuccess = base.Action(aAllocationAction, aComponent, aTolerancePercent, aStoreFilterRID, false);
                    }
                    else
                    {
                        actionSuccess = ProcessNonGroupAction(aAllocationAction, aComponent, aTolerancePercent, aStoreFilterRID, aWriteToDB);
                    }
                }
                _assortmentActionCount--;
                if (_assortmentActionCount == 0)
                {
                    if (actionSuccess
                        && aWriteToDB)
                    {
                        if (Enum.IsDefined(typeof(eGroupAllocationPlaceholderActionType), (int)aAllocationAction))
                        {
                            _placeholderAction = true;
                        }
                        _currentAction = aAllocationAction;  // TT#1723-MD - JSmith - Records written during cancel allocation
                        WriteHeaderData(HeaderDataRecord);
                    }
                } 
            }  
            catch
            {
                actionSuccess = false;
                _assortmentActionCount = 0;
                throw;
            }
            finally
            {
                _currentAction = 0;  // TT#1723-MD - JSmith - Records written during cancel allocation
                if (_assortmentActionCount == 0)
                {
                    if (HeaderDataRecord.ConnectionIsOpen)
                    {
                        HeaderDataRecord.CloseUpdateConnection();
                    }
					// Begin TT#4659 - JSmith - Group Allocation Release Error
                    if (HeaderLockRecord != null && HeaderLockRecord.ConnectionIsOpen)
                    {
                        HeaderLockRecord.RemoveLocks();
                        HeaderLockRecord.CloseUpdateConnection();
                    }
					// End TT#4659 - JSmith - Group Allocation Release Error
                    ProcessingGroupAllocation = false;
                    _placeholderAction = false;		
                    if (!actionSuccess)
                    {
                        AppSessionTransaction.RecoverHeaderAllocation();

                    }
                    AppSessionTransaction.ClearHeldAllocations();
                    ResetTempLocks(true);  
                }
            }
            return actionSuccess;
        }

        #region Process Release
        private bool ProcessRelease()
        {
            int releaseApprovedCount = 0;
            bool actionSuccess = true;
            bool updateSuccess = true;
            try
            {
                _processingActionOnHeaderInGroup = true;
                foreach (AllocationProfile ap in AssortmentMembers)
                {
                    try
                    {
                        updateSuccess = true;
                        if (ap.ReleaseApproved)
                        {
                            releaseApprovedCount++;
                        }
                        else if (ap.ReleaseApprovedAction(false))
                        {
                            releaseApprovedCount++;
                        }
                        else
                        {
                            updateSuccess = false;
                        }
                    }
                    catch
                    {
                        updateSuccess = false;
                        throw;
                    }
                    finally
                    {
                        if (!updateSuccess)
                        {
                            actionSuccess = updateSuccess;
                        }
                    }
                }
                if (actionSuccess
                    && releaseApprovedCount == AssortmentMembers.Length)
                {
                    foreach (AllocationProfile ap in AssortmentMembers)
                    {
                        if (!ap.ReleaseAction())
                        {
                            ap.SetReleased(false);
                            actionSuccess = false;
                        }
                    }
                }
            }
            catch
            {
                actionSuccess = false;
                throw;
            }
            finally
            {
                _processingActionOnHeaderInGroup = false;
            }
            return actionSuccess;
        }
        #endregion Process Release
 
        #region Non-Group Action
        private bool ProcessNonGroupAction(eAllocationMethodType aAllocationAction, GeneralComponent aComponent, double aTolerancePercent, int aStoreFilterRID, bool aWriteToDB)
        {
            bool actionSuccess = true;
            try
            {
                _processingActionOnHeaderInGroup = true;
                AllocationProfile[] apList = AssortmentMembers;
				// Begin TT#1184-MD - stodd - db timeout on cancel allocation
                if (!_memberHeadersProcessingCompleted)
                {
                    foreach (AllocationProfile ap in apList)
                    {
                        if (!ProcessActionOnOneHeader(ap, aAllocationAction, aComponent, aTolerancePercent, aStoreFilterRID, aWriteToDB))
                        {
                            actionSuccess = false;
                        }
                    }
                    _memberHeadersProcessingCompleted = true;
                }
                if (Enum.IsDefined(typeof(eGroupAllocationPlaceholderActionType), (int)aAllocationAction))
                {
                    if (!_memberPlaceholdersProcessingCompleted)
                    {
                        foreach (AllocationProfile ap in AssortmentPlaceHolders)
                        {
                            if (!ProcessActionOnOneHeader(ap, aAllocationAction, aComponent, aTolerancePercent, aStoreFilterRID, aWriteToDB))
                            {
                                actionSuccess = false;
                            }
                        }
                    }
                    _memberPlaceholdersProcessingCompleted = true;
                }
				// End TT#1184-MD - stodd - db timeout on cancel allocation
                if (Enum.IsDefined(typeof(eGroupAllocationActionRequiredType), (int)aAllocationAction))
                {
                    if (!base.Action(aAllocationAction, aComponent, aTolerancePercent, aStoreFilterRID, aWriteToDB))
                    {
                        actionSuccess = false;
                    }
                }
            }
            catch
            {
                actionSuccess = false;
                throw;
            }
            finally
            {
                _processingActionOnHeaderInGroup = false;
            }

            return actionSuccess;
        }
        private bool ProcessActionOnOneHeader(AllocationProfile aAllocationProfile, eAllocationMethodType aAllocationAction, GeneralComponent aComponent, double aTolerancePercent, int aStoreFilterRID, bool aWriteToDB)
        {
            bool updateSuccess = true;
            try
            {
                bool process = false;
                switch (aComponent.ComponentType)
                {
                    case (eComponentType.AllGenericPacks):
                        {
                            process = (aAllocationProfile.GenericPacks.Count > 0);
                            break;
                        }
                    case (eComponentType.DetailType):
                    case (eComponentType.DetailSubType):
                    case (eComponentType.AllNonGenericPacks):
                        {
                            process = (aAllocationProfile.NonGenericPacks.Count > 0);
                            break;
                        }
                    case (eComponentType.AllPacks):
                        {
                            process = (aAllocationProfile.Packs.Count > 0);
                            break;
                        }
                    case (eComponentType.AllSizes):
                        {
                            process =
                                (aAllocationProfile.WorkUpBulkSizeBuy
                                 || aAllocationProfile.BulkColorSizeCount > 0);
                            break;
                        }
                    case (eComponentType.AllColors):
                    case (eComponentType.Bulk):
                        {
                            process = (aAllocationProfile.BulkColors.Count > 0);
                            break;
                        }
                    case (eComponentType.ColorAndSize):
                        {
                            AllocationColorSizeComponent acsc = (AllocationColorSizeComponent)aComponent;
                            GeneralComponent colorComponent = acsc.ColorComponent;
                            GeneralComponent sizeComponent = acsc.SizeComponent;
                            switch (colorComponent.ComponentType)
                            {
                                case(eComponentType.AllColors):
                                    {
                                        process = true;
                                        break;
                                    }
                                case (eComponentType.SpecificColor):
                                    {
                                        HdrColorBin hcb = (HdrColorBin)aAllocationProfile.BulkColors[((AllocationColorOrSizeComponent)colorComponent).ColorRID];
                                        if (hcb != null)
                                        {
                                            switch (sizeComponent.ComponentType)
                                            {
                                                case(eComponentType.AllSizes):
                                                {
                                                    process = true;
                                                    break;
                                                }
                                                case(eComponentType.SpecificSize):
                                                {
                                                    process = ((HdrSizeBin)hcb.ColorSizes[((AllocationColorOrSizeComponent)sizeComponent).SizeRID] != null);
                                                    break;
                                                }
                                                case (eComponentType.SpecificSizePrimaryDim):
                                                {
                                                    AllocationColorOrSizeComponent primeSizeComponent = (AllocationColorOrSizeComponent)sizeComponent;

                                                    SizeCodeList sdl = AppSessionTransaction.GetSizeCodeByPrimaryDim(primeSizeComponent.PrimarySizeDimRID);
                                                    foreach (SizeCodeProfile scp in sdl.ArrayList)
                                                    {
                                                        if ((HdrSizeBin)hcb.ColorSizes[scp.Key] != null)
                                                        {
                                                            process = true;
                                                            break;
                                                        }
                                                    }
                                                    break;
                                                }
                                                case (eComponentType.SpecificSizeSecondaryDim):
                                                {
                                                    AllocationColorOrSizeComponent secondSizeComponent = (AllocationColorOrSizeComponent)sizeComponent;

                                                    SizeCodeList sdl = AppSessionTransaction.GetSizeCodeBySecondaryDim(secondSizeComponent.SecondarySizeDimRID);
                                                    foreach (SizeCodeProfile scp in sdl.ArrayList)
                                                    {
                                                        if ((HdrSizeBin)hcb.ColorSizes[scp.Key] != null)
                                                        {
                                                            process = true;
                                                            break;
                                                        }
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
                    case (eComponentType.SpecificColor):
                        {
                            AllocationColorOrSizeComponent colorComponent = (AllocationColorOrSizeComponent)aComponent;
                            HdrColorBin colorBin = (HdrColorBin)aAllocationProfile.BulkColors[colorComponent.ColorRID];
                            process = (colorBin != null);
                            break;
                        }
                    case (eComponentType.SpecificPack):
                        {
                            AllocationPackComponent apc = (AllocationPackComponent)aComponent;
                            PackHdr ph = (PackHdr)Packs[apc.PackName];
                            if (ph != null)
                            {
                                if (aAllocationProfile.GetPackHdr(ph.AssociatedPackRID) != null)
                                {
                                    process = true;
                                }
                            }
                            break;
                        }
                    case (eComponentType.SpecificSize):
                    case (eComponentType.SpecificSizePrimaryDim):
                    case (eComponentType.SpecificSizeSecondaryDim):
                    default:
                        {
                            process = true;
                            break;
                        }
                }

                if (process)
                {
                    if (aAllocationProfile.Action(aAllocationAction, aComponent, aTolerancePercent, aStoreFilterRID, aWriteToDB))
                    {
                    }
                    else
                    {
                        updateSuccess = false;
                    }
                }
            }
            catch
            {
                updateSuccess = false;
                throw;
            }
            finally
            {

            }
            return updateSuccess;
        }
        #endregion Non-Group Action

        #region Obsolete code

        //bool updateSuccess = true;
        //try
        //{
        //    // begin TT#1064 - MD - Jellis - Cannot Release Group Allocation
        //    if (aAllocationAction == eAllocationMethodType.Release)
        //    {
        //        int releaseApprovedCount = 0;
        //        foreach (AllocationProfile ap in AssortmentMembers)
        //        {
        //            if (ap.ReleaseApproved)
        //            {
        //                releaseApprovedCount++;
        //            }
        //            else if (!ap.ReleaseApprovedAction(false))
        //            {
        //                updateSuccess = false;
        //            }
        //            else
        //            {
        //                releaseApprovedCount++;
        //            }
        //        }
        //        if (releaseApprovedCount == AssortmentMembers.Length)
        //        {
        //            foreach (AllocationProfile ap in AssortmentMembers)
        //            {
        //                ap.ReleaseAction();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // end TT#1064 - MD - Jellis - Cannot Release Group Allocation
        //        AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error // TT#1008 - MD - Jellis - Get Non Negative Message for various variables
        //        if (!ProcessingGroupAllocation)
        //        {
        //            _assortmentActionCount = 1;
        //            AssortmentBeginTransaction(true);		// TT#1006 - md - stodd - GA Screen and Allocation WS after run Size Need status' are out of sync- 
        //            // begin TT#1008 - MD - Jellis - Get Non Negative Message for various variables
        //            foreach (AllocationProfile ap in apList)
        //            {
        //                AppSessionTransaction.HoldHeaderAllocation(ap);
        //            }
        //            // emd TT#1008 - MD - Jellis - Get Non Negative Message for various variables
        //            ResetTempLocks(false);  // TT#1016 - MD - Jellis - Action Failed
        //        }
        //        else
        //        {
        //            _assortmentActionCount++;
        //        }
        //        bool groupAction = Enum.IsDefined(typeof(eGroupAllocationActionType), (int)aAllocationAction);

        //        //AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error // TT#1008 - MD - Jellis - Get Non Negative Message for various variables
        //        if (!groupAction)
        //        {
        //            // begin TT#1008 - MD - Jellis - Get Non Negative Message for various variables
        //            if (_assortmentActionCount == 1)
        //            {
        //                // end TT#1008 - MD - Jellis - Get Non Negative Message for various variables
        //                foreach (AllocationProfile ap in apList)
        //                {
        //                    if (!ap.Action(aAllocationAction, aComponent, aTolerancePercent, aStoreFilterRID, false))
        //                    {
        //                        updateSuccess = false;
        //                        break;
        //                    }
        //                }
        //                if (updateSuccess)  // TT#891 - MD - Jellis - Group Allocation Need Gets Error
        //                {                   // TT#891 - MD - Jellis - Group Allocation Need Gets Error
        //                    if (Enum.IsDefined(typeof(eGroupAllocationPlaceholderActionType), (int)aAllocationAction))
        //                    {
        //                        foreach (AllocationProfile ap in AssortmentPlaceHolders) // TT#995 - MD - Jellis - Group Allocation Infrastructure built wrong
        //                        {
        //                            if (!ap.Action(aAllocationAction, aComponent, aTolerancePercent, aStoreFilterRID, false))
        //                            {
        //                                updateSuccess = false;
        //                                break;
        //                            }
        //                        }

        //                    }
        //                }  // TT#891 - MD - Jellis - Group Allocation Need Gets Error
        //                // begin TT#925 - MD - Jellis - Group Balance To Reserve ignores store allocations
        //                if (updateSuccess
        //                    && Enum.IsDefined(typeof(eGroupAllocationActionRequiredType), (int)aAllocationAction))
        //                {
        //                    if (!base.Action(aAllocationAction, aComponent, aTolerancePercent, aStoreFilterRID, false))
        //                    {
        //                        updateSuccess = false;
        //                    }
        //                }
        //                // end TT#925 - MD - Jellis - Group Balance To Reserve ignores store allocations
        //            } // TT#1008 - MD - Jellis - Get Non Negative Message for various variables
        //        }
        //        else  // TT#925 - MD - Jellis - Group Balance To Reserve ignores store allocations
        //        {     // TT#925 - MD - Jellis - Group Balance To Reserve ignores store allocations 
        //            if (updateSuccess)
        //            {
        //                if (!base.Action(aAllocationAction, aComponent, aTolerancePercent, aStoreFilterRID, false))
        //                {
        //                    updateSuccess = false;
        //                }
        //            }
        //        }     // TT#925 - MD - Jellis - Group Balance To Reserve ignores store allocations
        //        _assortmentActionCount--;
        //        if (_assortmentActionCount == 0)
        //        {
        //            // Begin TT#1006 - MD - stodd - status out of sync - 
        //            if (updateSuccess
        //                && aWriteToDB)
        //            {
        //                if (Enum.IsDefined(typeof(eGroupAllocationPlaceholderActionType), (int)aAllocationAction))
        //                {
        //                    _placeholderAction = true;
        //                }
        //                WriteHeaderData(HeaderDataRecord);
        //            }
        //            //    foreach (AllocationProfile ap in apList)
        //            //    {
        //            //        if (!ap.WriteHeaderData(ap.HeaderDataRecord))
        //            //        {
        //            //            updateSuccess = false;
        //            //            break;
        //            //        }
        //            //    }
        //            //    if (updateSuccess)
        //            //    {
        //            //        // begin TT#925 - MD - Jellis - Group Balance To Reserve ignores store allocations
        //            //        if (Enum.IsDefined(typeof(eGroupAllocationPlaceholderActionType), (int)aAllocationAction))
        //            //        {
        //            //            foreach (AllocationProfile ap in AssortmentPlaceHolders) // TT#995 - MD - Jellis - Group Allocation Infrastructure built wrong
        //            //            {
        //            //                if (!ap.WriteHeaderData(ap.HeaderDataRecord))
        //            //                {
        //            //                    updateSuccess = false;
        //            //                    break;
        //            //                }
        //            //            }
        //            //        }
        //            //        if (updateSuccess)
        //            //        {
        //            //            // end TT#925 - MD - Jellis - Group Balance To Reserve ignores store allocations
        //            //            updateSuccess = base.WriteHeaderData(base.HeaderDataRecord);
        //            //            if (updateSuccess)
        //            //            {
        //            //                HeaderDataRecord.CommitData();
        //            //            }
        //            //        } // TT#925 - MD - Jellis - Group Balance To Reserve ignores store allocations
        //            //    }
        //            //}
        //            // End TT#1006 - MD - stodd - status out of sync - 
        //        }
        //    }   // TT#1064 - MD - Jellis - Cannot RElease Group Allocation
        //}
        //catch
        //{
        //    updateSuccess = false;
        //    _assortmentActionCount = 0;
        //    throw;
        //}
        //finally
        //{
        //    if (_assortmentActionCount == 0)
        //    {
        //        if (HeaderDataRecord.ConnectionIsOpen)
        //        {
        //            HeaderDataRecord.CloseUpdateConnection();
        //        }
        //        ProcessingGroupAllocation = false;
        //        _placeholderAction = false;		// TT#1006 - md - stodd - GA Screen and Allocation WS after run Size Need status' are out of sync- 
        //        // begin TT#1008 - MD - Jellis - Get Non Negative Message for various variables
        //        if (!updateSuccess)
        //        {
        //            AppSessionTransaction.RecoverHeaderAllocation();

        //        }
        //        AppSessionTransaction.ClearHeldAllocations();
        //        // end TT#1008 - MD - Jellis - Get Non Negative Message for various variables

        //        ResetTempLocks(true);  // TT#1016 - MD - Jellis - Action Failed
        //    }
        //}
        //return updateSuccess;
        //}
        #endregion Obsolete code

        #region AssortmentBeginTransaction
        internal void AssortmentBeginTransaction(bool isAction)		// Begin TT#1006 - md - stodd - GA Screen and Allocation WS after run Size Need status' are out of sync- 

        {
            try
            {
                AllocationProfile[] apList = new AllocationProfile[AssortmentMembers.Length + AssortmentPlaceHolders.Length]; // TT#995 - MD - Jellis - Group Allocation Infrastructure built wrong
                AssortmentMembers.CopyTo(apList, 0); // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                AssortmentPlaceHolders.CopyTo(apList, AssortmentMembers.Length); // TT#891 - MD - JEllis - Group Allocation Need gets error // TT#995 - MD - Jellis - Group Allocation Infrastructure built wrong
                //======================================================================================
                // build style list for locking
                //======================================================================================
                // Add Assortment style to style list first
                //======================================================================================
                List<Resource> resource = new List<Resource>();
                resource.Add(new Resource(eLockType.Header, HeaderRID, string.Empty));
                ProcessingGroupAllocation = true;
                ArrayList styleRidList = new ArrayList();
                HierarchyNodeProfile hnParent = AppSessionTransaction.GetParentNodeData(StyleHnRID);
                styleRidList.Add(hnParent.Key);
                foreach (AllocationProfile memberAp in apList)
                {
                    if (!AppSessionTransaction.GetVswReverseOnhandContainer().IsHeaderCached(memberAp.HeaderRID))
                    {
                        AppSessionTransaction.GetVswReverseOnhandContainer().ReadStoreVswReverseOnhand(memberAp.HeaderRID);
                    }
                    memberAp.HeaderDataRecord = HeaderDataRecord;
                    hnParent = AppSessionTransaction.GetParentNodeData(memberAp.StyleHnRID);
                    resource.Add(new Resource(eLockType.Header, memberAp.HeaderRID, string.Empty));
                    resource.Add(new Resource(eLockType.Intransit, hnParent.Key, string.Empty));
                }
				// Begin TT#4659 - JSmith - Group Allocation Release Error
				//HeaderDataRecord.OpenUpdateConnection(resource);
                HeaderDataRecord.OpenUpdateConnection();
                HeaderLockRecord.OpenUpdateConnection(resource);
				// End TT#4659 - JSmith - Group Allocation Release Error
                _processingMethod = !isAction;		// TT#1006 - md - stodd - GA Screen and Allocation WS after run Size Need status' are out of sync- 
                // begin TT#1064 - MD - Jellis - Cannot Release Group Allocation
                GeneralComponent gc = new GeneralComponent(eComponentType.Total);  // TT#1074 - MD - Jellis - Group ALlocation - Inventory Min Max Wrong (Force capacity to be built);
                Index_RID storeIndexRID = AppSessionTransaction.StoreIndexRIDArray()[0];  // TT#1074 - MD - Jellis - Group ALlocation - Inventory Min Max Wrong (Force capacity to be built);
                foreach (AllocationProfile memberAp in apList)
                {
                    memberAp.GetStoreCapacityMaximum(gc, storeIndexRID, false);  // TT#1074 - MD - Jellis - Group ALlocation - Inventory Min Max Wrong (Force capacity to be built);
                    if (AppSessionTransaction.GetBuildItem(memberAp.BuildItemKey))
                    {
                        memberAp.Build_Item_VSW_Values();
                    }
                    foreach (HdrColorBin hcb in memberAp.BulkColors.Values)
                    {
                        if (hcb.CalcVswSizeConstraints)
                        {
                            memberAp.CalculateSizeItemMinimums(hcb);
                        }
                    }
                }
                // end TT#1064 - MD - Jellis - Cannot Release Group Allocation
            }
            catch
            {
                throw;
            }
        }
        #endregion AssortmentBeginTransaction

        // end TT#488 - MD - Jellis - Group Allocation


        #region AllocateReserveUnits
        /// <summary>
        /// Allocates reserve units to the reserve store.
        /// </summary>
        override public void AllocateReserve(AllocateReserveSpecification aAllocateReserveSpecification)
        {
            if (aAllocateReserveSpecification.IsReserveSpecPercent)
            {
                foreach (AllocationProfile ap in AssortmentMembers)
                {
                    ap.AllocateReserve(aAllocateReserveSpecification);
                }
            }
            else
            {
                int unitsToAllocate = this.TotalUnitsToAllocate;
                AllocateReserveSpecification ars;
                int reserveTotal;
                int reservePack;
                int reserveBulk;
                foreach (AllocationProfile ap in AssortmentMembersSorted)
                {
                    if (unitsToAllocate > 0)
                    {
                        reserveTotal =
                            (int)((aAllocateReserveSpecification.ReserveTotal
                                  * (double)ap.TotalUnitsToAllocate
                                  / (double)unitsToAllocate) + .5d);
                        unitsToAllocate -= reserveTotal;
                        reservePack =
                            (int)((aAllocateReserveSpecification.ReservePack * reserveTotal
                                   / aAllocateReserveSpecification.ReserveTotal) + .5d);
                        reserveBulk = reserveTotal - reservePack;
                    }
                    else
                    {
                        reserveTotal = 0;
                        reservePack = 0;
                        reserveBulk = 0;
                    }
                    ars = new AllocateReserveSpecification(
                        false,
                        (double)reserveTotal,
                        (double)reservePack,
                        (double)reserveBulk);

                }
            }
        }
        #endregion AllocateReserveUnits

        #region ReserveQty
        #region GetReserveQty
        /// <summary>
        /// Gets Reserve Total Units
        /// </summary>
        /// <param name="aAllocationSummaryNode">eAllocationSummaryNode.Total</param>
        /// <returns>Total Reserve Units</returns>
        override internal int GetReserveQty(eAllocationSummaryNode aAllocationSummaryNode)
        {
            int reserveUnits = 0;
            foreach (AllocationProfile ap in AssortmentMembers)
            {
                reserveUnits += ap.GetReserveQty(aAllocationSummaryNode);
            }
            return reserveUnits;
        }

        /// <summary>
        /// Gets Reserve Packs
        /// </summary>
        /// <param name="aPackName">Pack Name</param>
        /// <returns>Reserve Packs in the pack.</returns>
        override internal int GetReserveQty(string aPackName)  // TT#1008 - MD - Jellis - Get Non Negative Message for various variables
        {
            throw new MIDException(eErrorLevel.severe,
                (int)(eMIDTextCode.msg_al_ReserveNotImplemented),
                Session.Audit.GetText(eMIDTextCode.msg_al_ReserveNotImplemented, false)); 
            // Assortment/Group Allocation cannot resolve PackName; only an AllocationProfile can resolve pack names
        }

        /// <summary>
        /// Gets Reserve Units for a given color.
        /// </summary>
        /// <param name="aColorRID">RID of the color</param>
        /// <returns>Reserve Units in the color</returns>
        override internal int GetReserveQty(int aColorRID)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColorRID);
            int reserveUnits = 0;
            foreach (AllocationProfile ap in apList)
            {
                reserveUnits += ap.GetReserveQty(aColorRID);
            }
            return reserveUnits;
        }

        /// <summary>
        /// Gets Reserve Units fof a given color-size.
        /// </summary>
        /// <param name="aColorRID">RID of the color</param>
        /// <param name="aSizeCodeRID">RID of the size</param>
        /// <returns>Reserve Units in the size</returns>
        override internal int GetReserveQty(int aColorRID, int aSizeCodeRID)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColorRID);
            int reserveUnits = 0;
            foreach (AllocationProfile ap in apList)
            {
                HdrSizeBin sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aColorRID).ColorSizes[aSizeCodeRID];
                if (sizeBin != null)
                {
                    reserveUnits += sizeBin.ReserveUnits;
                }
            }
            return reserveUnits;
        }
        /// <summary>
		/// Gets Reserve Units for a given size
		/// </summary>
		/// <param name="aColor">HdrColorBin that describes the color where the size reside</param>
		/// <param name="aSizeCodeRID">RID of the size</param>
		/// <returns>Reserve Units in the size</returns>
		override internal int GetReserveQty(HdrColorBin aColor, int aSizeCodeRID)
		{
			return GetReserveQty(aColor.ColorCodeRID, aSizeCodeRID);
		}
		/// <summary>
		/// Gets Reserve Units for a given size
		/// </summary>
		/// <param name="aSize">HdrSizeBin that describes the size</param>
		/// <returns>Reserve Units in the size</returns>
		override internal int GetReserveQty(HdrSizeBin aSize)
		{
			return GetReserveQty(aSize.Color.ColorCodeRID, aSize.SizeCodeRID);
		}
		#endregion GetReserveQty
        #region SetReserveQty
        /// <summary>
        /// Sets Reserve Quantity for Total
        /// </summary>
        /// <param name="aAllocationSummaryNode">eAllocationSummaryNode.Total</param>
        /// <param name="aReserveQty">Reserve Units</param>
        override internal void SetReserveQty(eAllocationSummaryNode aAllocationSummaryNode, int aReserveQty)
        {
            AllocationProfile[] apListSorted = AssortmentMembersSorted; // TT#891 - MD - Jellis - Group Allocation Need Gets error
            int reserveQty = aReserveQty;
            int totalBasis = TotalUnitsToAllocate;
            int apReserve;
            foreach (AllocationProfile ap in apListSorted)
            {
                if (totalBasis > 0)
                {
                    apReserve =
                        (int)(((double)reserveQty
                                * (double)ap.TotalUnitsToAllocate
                                / (double)totalBasis) + .5d);
                    if (apReserve > ap.TotalUnitsToAllocate)
                    {
                        apReserve = ap.TotalUnitsToAllocate;
                    }
                    reserveQty -= apReserve;
                    totalBasis -= ap.TotalUnitsToAllocate;
                }
                else
                {
                    apReserve = 0;
                }
                ap.SetReserveQty(aAllocationSummaryNode, apReserve);
            }
        }

        /// <summary>
        /// Sets Reserve Packs for a given pack.
        /// </summary>
        /// <param name="aPackName">Pack name</param>
        /// <param name="aReserveQty">Reserve Packs.</param>
        override internal void SetReserveQty(string aPackName, int aReserveQty)  // TT#1008 - MD - Jellis - Get Non Negative Message for various variables
        {
            throw new MIDException(eErrorLevel.severe,
                (int)(eMIDTextCode.msg_al_ReserveNotImplemented),
                Session.Audit.GetText(eMIDTextCode.msg_al_ReserveNotImplemented, false));  
            // Assortment/Group Allocation has no way to resolve PackName
        }

        /// <summary>
        /// Sets Reserve Quantity for a given color.
        /// </summary>
        /// <param name="aColorRID">RID for the color</param>
        /// <param name="aReserveQty">Reserve Quantity</param>
        override internal void SetReserveQty(int aColorRID, int aReserveQty)
        {
            List<AllocationProfile> apWithColorList = GetHeadersWithColor(aColorRID);
            List<AllocationProfile> apWithColorListSorted = new List<AllocationProfile>();
            int totalBasis = 0;
            int colorUnitsToAllocate;
            MIDGenericSortItem[] mgsi = new MIDGenericSortItem[apWithColorList.Count];
            int i = 0;
            foreach (AllocationProfile ap in apWithColorList)
            {
                colorUnitsToAllocate = ap.GetColorUnitsToAllocate(aColorRID);
                totalBasis += colorUnitsToAllocate;
                mgsi[i] = new MIDGenericSortItem();
                mgsi[i].Item = i;
                mgsi[i].SortKey = new double[2];
                mgsi[i].SortKey[0] = colorUnitsToAllocate;
                mgsi[i].SortKey[1] = AppSessionTransaction.GetRandomDouble();
                i++;
            }
            //Array.Sort(mgsi, new MIDGenericSortDescendingComparer()); // TT#1143 - MD - Jellis - Group Allocation Min Broken
            Array.Sort(mgsi, new MIDGenericSortAscendingComparer());    // TT#1143 - MD - Jellis - Group Allocation Min Broken
            foreach (MIDGenericSortItem sortItem in mgsi)
            {
                apWithColorListSorted.Add(apWithColorList[sortItem.Item]);
            }
            int reserveTotal = aReserveQty;
            int reserveQty;
            foreach (AllocationProfile ap in apWithColorListSorted)
            {
                if (totalBasis > 0)
                {
                    colorUnitsToAllocate = ap.GetColorUnitsToAllocate(aColorRID);
                    reserveQty = 
                        (int) (((double)reserveTotal 
                               * (double)colorUnitsToAllocate
                               / (double) totalBasis) + .5d);
                    if (reserveQty > colorUnitsToAllocate)
                    {
                        reserveQty = colorUnitsToAllocate;
                    }
                    totalBasis -= colorUnitsToAllocate;
                    reserveTotal -= reserveQty;
                }
                else
                {
                    reserveQty = 0;
                }
                ap.SetReserveQty(aColorRID, reserveQty);
            }
        }


        /// <summary>
        /// Sets Reserve Quantity for a given color-size.
        /// </summary>
        /// <param name="aColorRID">RID for the color</param>
        /// <param name="aSizeCodeRID">RID of the size</param>
        /// <param name="aReserveQty">Reserve Quantity</param>
        override internal void SetReserveQty(int aColorRID, int aSizeCodeRID, int aReserveQty)
        {
            List<AllocationProfile> apWithColorList = GetHeadersWithColor(aColorRID);
            List<HdrSizeBin> sortedSizeBins = new List<HdrSizeBin>();
            int totalBasis = 0;
            int sizeUnitsToAllocate;
            MIDGenericSortItem[] mgsi = new MIDGenericSortItem[apWithColorList.Count];
            int i = 0;
            foreach (AllocationProfile ap in apWithColorList)
            {
                HdrSizeBin sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aColorRID).ColorSizes[aSizeCodeRID];
                sizeUnitsToAllocate = sizeBin.SizeUnitsToAllocate;
                totalBasis += sizeUnitsToAllocate;
                mgsi[i] = new MIDGenericSortItem();
                mgsi[i].Item = i;
                mgsi[i].SortKey = new double[2];
                mgsi[i].SortKey[0] = sizeUnitsToAllocate;
                mgsi[i].SortKey[1] = AppSessionTransaction.GetRandomDouble();
                i++;
            }
            int reserveTotal = aReserveQty;
            int reserveQty;
            foreach (HdrSizeBin hsb in sortedSizeBins)
            {
                if (totalBasis > 0)
                {
                    reserveQty =
                        (int)(((double)reserveTotal
                               * (double)hsb.SizeUnitsToAllocate
                               / (double)totalBasis) + .5d);
                    if (reserveQty > hsb.SizeUnitsToAllocate)
                    {
                        reserveQty = hsb.SizeUnitsToAllocate;
                    }
                    totalBasis -= hsb.SizeUnitsToAllocate;
                    reserveTotal -= reserveQty;
                }
                else
                {
                    reserveQty = 0;
                }
                hsb.SetReserveUnits(reserveQty);
            }
        }
        #endregion SetReserveQty
        #endregion ReserveQty

        #region Stores
 
        #region StoreIsManuallyAllocated

        #region GetStoreStyleAllocationIsManuallyAllocated
        /// <summary>
        /// Gets a bool indicating if a store's allocation by style/color was manually allocated
        /// </summary>
        /// <param name="aStore">Index_RID identifying the store</param>
        /// <returns>True: store's total, at least one pack or at least one bulk color allocation was manually entered; False: store's total, all packs and all bulk colors allocations were not manually entered </returns>
        override public bool GetStoreStyleAllocationIsManuallyAllocated(Index_RID aStore)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            foreach (AllocationProfile ap in apList)
            {
                if (ap.GetStoreStyleAllocationIsManuallyAllocated(aStore))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion GetStoreStyleIsManuallyAllocated

        #region GetStoreSizeAllocationIsManuallyAllocated
        /// <summary>
        /// Gets a bool indicating if a store's size allocation was manually allocated
        /// </summary>
        /// <param name="aStore">Index_RID identifying the store</param>
        /// <returns>True: store's size allocation was manully entered for at least one size; False: store's size allocation was not manually entered for any size</returns>
        override public bool GetStoreSizeAllocationIsManuallyAllocated(Index_RID aStore)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            foreach (AllocationProfile ap in apList)
            {
                if (ap.GetStoreSizeAllocationIsManuallyAllocated(aStore))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion GetStoreSizeAllocationIsManuallyAllocat

        #region GetStoreIsManuallyAllocated
        /// <summary>
        /// Gets Store Is Manually Allocated audit flag for specified store on specified node.
        /// </summary>
        /// <param name="aAllocationSummaryNode">Allocation summary node.</param>
        /// <param name="aStore">Index_RID for store</param>
        /// <returns>True when store is manually allocated; false otherwise.</returns>
        override internal bool GetStoreIsManuallyAllocated(eAllocationSummaryNode aAllocationSummaryNode, Index_RID aStore)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            foreach (AllocationProfile ap in apList)
            {
                if (!ap.GetStoreIsManuallyAllocated(aAllocationSummaryNode, aStore))
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Gets Store Is Manually Allocated audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>True when store is manually allocated; false otherwise.</returns>
        override internal bool GetStoreIsManuallyAllocated(HdrColorBin aColor, Index_RID aStore)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                if (!ap.GetStoreIsManuallyAllocated(aColor.ColorCodeRID, aStore))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets Store Is Manually Allocated audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aSize">HdrSizeBin for the size where the store resides.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>True when store is manually allocated; false otherwise.</returns>
        override internal bool GetStoreIsManuallyAllocated(HdrSizeBin aSize, Index_RID aStore)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                // begin TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                HdrSizeBin sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aSize.Color.ColorCodeRID).ColorSizes[aSize.SizeCodeRID];
                if (sizeBin != null)
                {
                    if (!ap.GetStoreIsManuallyAllocated(sizeBin, aStore))
                    //if (!ap.GetStoreIsManuallyAllocated(aSize.Color.ColorCodeRID, aSize.SizeCodeRID, aStore))
                    // end TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion GetStoreIsManuallyAllocated

        #region SetStoreIsManuallyAllocated

        /// <summary>
        /// Sets Store Is Manually Allocated audit flag for specified store on specified node.
        /// </summary>
        /// <param name="aAllocationSummaryNode">Allocation summary node.</param>
        /// <param name="aStore">Index_RID for store</param>
        /// <param name="aFlagValue">True when store is manually allocated; false otherwise.</param>
        override internal void SetStoreIsManuallyAllocated(eAllocationSummaryNode aAllocationSummaryNode, Index_RID aStore, bool aFlagValue)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            foreach (AllocationProfile ap in apList)
            {
                // NOTE:  when Assortment level is changed the "flag" value is pushed to each member header
                ap.SetStoreIsManuallyAllocated(aAllocationSummaryNode, aStore, aFlagValue);
            }
        }
        
        /// <summary>
        /// Sets Store Is Manually Allocated audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <param name="aFlagValue">True when store is manually allocated; false otherwise.</param>
        override internal void SetStoreIsManuallyAllocated(HdrColorBin aColor, Index_RID aStore, bool aFlagValue)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                ap.SetStoreIsManuallyAllocated(aColor.ColorCodeRID, aStore, aFlagValue);
            }
        }

        /// <summary>
        /// Sets Store Is Manually Allocated audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aSize">HdrSizeBin for the size where the store resides.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <param name="aFlagValue">True when store is manually allocated; false otherwise.</param>
        override internal void SetStoreIsManuallyAllocated(HdrSizeBin aSize, Index_RID aStore, bool aFlagValue)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                // begin TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                HdrSizeBin sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aSize.Color.ColorCodeRID).ColorSizes[aSize.SizeCodeRID];
                if (sizeBin != null)
                {
                    ap.SetStoreIsManuallyAllocated(sizeBin, aStore, aFlagValue);
                }
                //ap.SetStoreIsManuallyAllocated(aSize.Color.ColorCodeRID, aSize.SizeCodeRID, aStore, aFlagValue);
                // end TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
            }
        }
        #endregion SetStoreIsManuallyAllocated

        #region GetStoreItemIsManuallyAllocated

        /// <summary>
        /// Gets Store Item Is Manually Allocated audit flag for specified store on specified node.
        /// </summary>
        /// <param name="aAllocationSummaryNode">Allocation summary node.</param>
        /// <param name="aStore">Index_RID for store</param>
        /// <returns>True when Store Item Is manually allocated; false otherwise.</returns>
        override internal bool GetStoreItemIsManuallyAllocated(eAllocationSummaryNode aAllocationSummaryNode, Index_RID aStore)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            foreach (AllocationProfile ap in apList)
            {
                if (!ap.GetStoreItemIsManuallyAllocated(aAllocationSummaryNode, aStore))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets Store Item Is Manually Allocated audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>True when Store Item Is manually allocated; false otherwise.</returns>
        override internal bool GetStoreItemIsManuallyAllocated(HdrColorBin aColor, Index_RID aStore) // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                if (!ap.GetStoreItemIsManuallyAllocated(aColor.ColorCodeRID, aStore))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets Store Item Is Manually Allocated audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aSize">HdrSizeBin for the size where the store resides.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>True when Store Item Is manually allocated; false otherwise.</returns>
        override internal bool GetStoreItemIsManuallyAllocated(HdrSizeBin aSize, Index_RID aStore)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                // begin TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                HdrSizeBin sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aSize.Color.ColorCodeRID).ColorSizes[aSize.SizeCodeRID];
                if (sizeBin != null)
                {
                    if (!ap.GetStoreItemIsManuallyAllocated(sizeBin, aStore))
                    //if (!ap.GetStoreItemIsManuallyAllocated(aSize.Color.ColorCodeRID, aSize.SizeCodeRID, aStore))
                        // end TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                    {
                        return false;
                    }
                }  // TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
            }
            return true;
        }
        #endregion GetStoreItemIsManuallyAllocated

        #region SetStoreItemIsManuallyAllocated
        /// <summary>
        /// Sets Store Item Is Manually Allocated audit flag for specified store on specified node.
        /// </summary>
        /// <param name="aAllocationSummaryNode">Allocation summary node.</param>
        /// <param name="aStore">Index_RID for store</param>
        /// <param name="aFlagValue">True when Store Item Is manually allocated; false otherwise.</param>
        override internal void SetStoreItemIsManuallyAllocated(eAllocationSummaryNode aAllocationSummaryNode, Index_RID aStore, bool aFlagValue)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            foreach (AllocationProfile ap in apList)
            {
                // "push" value to all member headers
                ap.SetStoreItemIsManuallyAllocated(aAllocationSummaryNode, aStore, aFlagValue);
            }
        }

        /// <summary>
        /// Sets Store Item Is Manually Allocated audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <param name="aFlagValue">True when Store Item Is manually allocated; false otherwise.</param>
        override internal void SetStoreItemIsManuallyAllocated(HdrColorBin aColor, Index_RID aStore, bool aFlagValue) // TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                ap.SetStoreItemIsManuallyAllocated(aColor.ColorCodeRID, aStore, aFlagValue);
            }
        }

        /// <summary>
        /// Sets Store Item Is Manually Allocated audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aSize">HdrSizeBin for the size where the store resides.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <param name="aFlagValue">True when Store Item Is manually allocated; false otherwise.</param>
        override internal void SetStoreItemIsManuallyAllocated(HdrSizeBin aSize, Index_RID aStore, bool aFlagValue)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                // begin TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                HdrSizeBin sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aSize.Color.ColorCodeRID).ColorSizes[aSize.SizeCodeRID];
                if (sizeBin != null)
                {
                    ap.SetStoreItemIsManuallyAllocated(sizeBin, aStore, aFlagValue);
                }
                //ap.SetStoreItemIsManuallyAllocated(aSize.Color.ColorCodeRID, aSize.SizeCodeRID, aStore, aFlagValue); // TT#915 - MD - Jellis - Group ALlocatoin Stack Overflow on Cancel Allocation
                // end TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
            }
        }
        #endregion SetStoreItemIsManuallyAllocated
        // end TT#1401 - JEllis - Urban Reservation Stores pt 2
        #endregion StoreIsManuallyAllocated

        #region StoreLocked
        #region GetStoreLocked

        /// <summary>
        /// Gets Store Locked audit flag for specified store on specified node.
        /// </summary>
        /// <param name="aAllocationSummaryNode">Allocation summary node.</param>
        /// <param name="aStore">Index_RID for store</param>
        /// <returns>True when store Locked; false otherwise.</returns>
        override public bool GetStoreLocked(eAllocationSummaryNode aAllocationSummaryNode, Index_RID aStore) // TT#1189 - MD - Jellis - Group Allocation - Implement Lock Not Persist to Database pt 1
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            foreach (AllocationProfile ap in apList)
            {
                if (!ap.GetStoreLocked(aAllocationSummaryNode, aStore))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets Store Locked audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>True when store Locked; false otherwise.</returns>
        override public bool GetStoreLocked(HdrColorBin aColor, Index_RID aStore) // TT#1189 - MD - Jellis - Group Allocation - Implement Lock Not Persist to Database pt 1
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                if (!ap.GetStoreLocked(aColor.ColorCodeRID, aStore))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets Store Locked audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aSize">HdrSizeBin for the size where the store resides.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>True when store Locked; false otherwise.</returns>
        override public bool GetStoreLocked(HdrSizeBin aSize, Index_RID aStore) // TT#1189 - MD - Jellis - Group Allocation - Implement Lock Not Persist to Database pt 1
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                // begin TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                HdrSizeBin sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aSize.Color.ColorCodeRID).ColorSizes[aSize.SizeCodeRID];
                if (sizeBin != null)
                {
                    if (!ap.GetStoreLocked(sizeBin, aStore))
                    //if (!ap.GetStoreLocked(aSize.Color.ColorCodeRID, aSize.SizeCodeRID, aStore))
                        //TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                    {
                        return false;
                    }
                } // TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
            }
            return true;
        }
        #endregion GetstoreLocked

        #region SetStoreLocked 
        /// <summary>
        /// Sets Store Locked audit flag for specified store on specified node.
        /// </summary>
        /// <param name="aAllocationSummaryNode">Allocation summary node.</param>
        /// <param name="aStore">Index_RID for store</param>
        /// <param name="aFlagValue">True when store Locked; false otherwise.</param>
        override public void SetStoreLocked(eAllocationSummaryNode aAllocationSummaryNode, Index_RID aStore, // TT#1189 - MD - Jellis - Group Allocation - Implement Lock Not Persist to Database pt 1
            bool aFlagValue, eDistributeChange aDistributeChange)
        {
            // begin TT#4255 - MD - Jellis - GA - Qty Cannot Be Negative
            if (AppSessionTransaction != null
                && AppSessionTransaction.GetBuildItem(BuildItemKey))
            {
                Build_Item_VSW_Values();
            }
            // end TT#4255 - MD - Jellis - GA - Qty Cannot Be Negative

            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            foreach (AllocationProfile ap in apList)
            {
                ap.SetStoreLocked(aAllocationSummaryNode, aStore, aFlagValue, aDistributeChange);
            }
        }

        /// <summary>
        /// Sets Store Locked audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <param name="aFlagValue">True when store Locked; false otherwise.</param>
        /// <param name="aDistributeChange">Indicates how the change should be reflected in parent and children nodes</param>
        override public void SetStoreLocked(HdrColorBin aColor, Index_RID aStore, bool aFlagValue, eDistributeChange aDistributeChange) // TT#1189 - MD - Jellis - Group Allocation - Implement Lock Not Persist to Database pt 1
        {
            // begin TT#4255 - MD - Jellis - GA - Qty Cannot Be Negative
            if (AppSessionTransaction != null
                && AppSessionTransaction.GetBuildItem(BuildItemKey))
            {
                Build_Item_VSW_Values();
            }
            // end TT#4255 - MD - Jellis - GA - Qty Cannot Be Negative

            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                ap.SetStoreLocked(aColor.ColorCodeRID, aStore, aFlagValue, aDistributeChange);
            }
        }
         
        /// <summary>
        /// Sets Store Locked audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aSize">HdrSizeBin for the size where the store resides.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <param name="aFlagValue">True when store Locked; false otherwise.</param>
        /// <param name="aDistributeChange">Indicates how the change should be reflected in parent and children nodes.</param>
        override public void SetStoreLocked(HdrSizeBin aSize, Index_RID aStore, bool aFlagValue, eDistributeChange aDistributeChange) // TT#1189 - MD - Jellis - Group Allocation - Implement Lock Not Persist to Database pt 1
        {
            // begin TT#4255 - MD - Jellis - GA - Qty Cannot Be Negative
            if (AppSessionTransaction != null
                && AppSessionTransaction.GetBuildItem(BuildItemKey))
            {
                Build_Item_VSW_Values();
            }
            // end TT#4255 - MD - Jellis - GA - Qty Cannot Be Negative

            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                // begin TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                HdrSizeBin sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aSize.Color.ColorCodeRID).ColorSizes[aSize.SizeCodeRID];
                if (sizeBin != null)
                {
                    ap.SetStoreLocked(aSize.Color.ColorCodeRID, aSize.SizeCodeRID, aStore, aFlagValue, aDistributeChange);
                    //ap.SetStoreLocked(aSize.Color.ColorCodeRID, aSize.SizeCodeRID, aStore, aFlagValue, aDistributeChange);
                }
                // end TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
            }
        }
        #endregion SetStorLocked
        #endregion StoreLocked

        #region StoreTempLock
        #region GetStoreTempLock
        /// <summary>
        /// Gets Store TempLock audit flag for specified store on specified node.
        /// </summary>
        /// <param name="aAllocationSummaryNode">Allocation summary node.</param>
        /// <param name="aStore">Index_RID for store</param>
        /// <returns>True when store TempLock; false otherwise.</returns>
        override internal bool GetStoreTempLock(eAllocationSummaryNode aAllocationSummaryNode, Index_RID aStore)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            foreach (AllocationProfile ap in apList)
            {
                if (!ap.GetStoreTempLock(aAllocationSummaryNode, aStore))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets Store TempLock audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>True when store TempLock; false otherwise.</returns>
        override internal bool GetStoreTempLock(HdrColorBin aColor, Index_RID aStore)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                if (!ap.GetStoreTempLock(aColor.ColorCodeRID, aStore))
                {
                    return false;
                }
            }
            return true;
        }

         /// <summary>
        /// Gets Store TempLock audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aSize">HdrSizeBin for the size where the store resides.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>True when store TempLock; false otherwise.</returns>
        override internal bool GetStoreTempLock(HdrSizeBin aSize, Index_RID aStore)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                // begin TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                HdrSizeBin sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aSize.Color.ColorCodeRID).ColorSizes[aSize.SizeCodeRID];
                if (sizeBin != null)
                {
                    if (!ap.GetStoreTempLock(sizeBin, aStore))
                    //if (!ap.GetStoreTempLock(aSize.Color.ColorCodeRID, aStore))
                    // end TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                    {
                        return false;
                    }
                } // TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
            }
            return true;
        }
        #endregion GetStoreTempLock

        #region SetStoreTempLock
         /// <summary>
        /// Sets Store TempLock audit flag for specified store on specified node.
        /// </summary>
        /// <param name="aAllocationSummaryNode">Allocation summary node.</param>
        /// <param name="aStore">Index_RID for store</param>
        /// <param name="aFlagValue">True when store TempLock; false otherwise.</param>
        override internal void SetStoreTempLock(eAllocationSummaryNode aAllocationSummaryNode, Index_RID aStore,
            bool aFlagValue, eDistributeChange aDistributeChange)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            foreach (AllocationProfile ap in apList)
            {
                ap.SetStoreTempLock(aAllocationSummaryNode, aStore, aFlagValue, aDistributeChange);
            }
        }
 
        /// <summary>
        /// Sets Store TempLock audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <param name="aFlagValue">True when store TempLock; false otherwise.</param>
        /// <param name="aDistributeChange">Indicates how the change should be reflected in parent and children nodes</param>
        override internal void SetStoreTempLock(HdrColorBin aColor, Index_RID aStore, bool aFlagValue, eDistributeChange aDistributeChange)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                ap.SetStoreTempLock(aColor.ColorCodeRID, aStore, aFlagValue, aDistributeChange);
            }
        }

        /// <summary>
        /// Sets Store TempLock audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aSize">HdrSizeBin for the size where the store resides.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <param name="aFlagValue">True when store TempLock; false otherwise.</param>
        /// <param name="aDistributeChange">Indicates how the change should be reflected in parent and children nodes.</param>
        override internal void SetStoreTempLock(HdrSizeBin aSize, Index_RID aStore, bool aFlagValue, eDistributeChange aDistributeChange)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                // begin TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                HdrSizeBin sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aSize.Color.ColorCodeRID).ColorSizes[aSize.SizeCodeRID];
                if (sizeBin != null)
                {
                    ap.SetStoreTempLock(sizeBin, aStore, aFlagValue, aDistributeChange);
                    //ap.SetStoreTempLock(aSize.Color.ColorCodeRID, aSize.SizeCodeRID, aStore, aFlagValue, aDistributeChange);
                }
                // end TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
            }
        }
        #endregion SetStoreTempLock

        #region ReSetStoreTempLock
        // begin TT#59 Implement Temp Locks
        // begin TT#225 Balance Size Proportional Gets error
        // begin TT#408 Cannot Cancel Allocation
        //internal void ResetTempLocks()
        //{
        //    ResetTempLocks(true);
        //}
        // end TT#408 Cannot Cancel Allocation
        override public void ResetTempLocks(bool aEnableTempLock) //TT#1227 - MD- DOConnell - Get a spread lock error when trying to remove a color from a placeholder
        //override internal void ResetTempLocks(bool aEnableTempLock)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            foreach (AllocationProfile ap in apList)
            {
                ap.ResetTempLocks(aEnableTempLock);
            }
            base.ResetTempLocks(aEnableTempLock);
        }
        // end TT#59 Implement Temp Locks
        #endregion ReSetStoreTempLock

        #region GetStoreItemTempLock
        /// <summary>
        /// Gets Store Item TempLock audit flag for specified store on specified node.
        /// </summary>
        /// <param name="aAllocationSummaryNode">Allocation summary node.</param>
        /// <param name="aStore">Index_RID for store</param>
        /// <returns>True when Store Item TempLock; false otherwise.</returns>
        override internal bool GetStoreItemTempLock(eAllocationSummaryNode aAllocationSummaryNode, Index_RID aStore)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            foreach (AllocationProfile ap in apList)
            {
                if (!ap.GetStoreItemTempLock(aAllocationSummaryNode, aStore))
                {
                    return false;
                }
            }
            return true;
        }

 
        /// <summary>
        /// Gets Store Item TempLock audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>True when Store Item TempLock; false otherwise.</returns>
        override internal bool GetStoreItemTempLock(HdrColorBin aColor, Index_RID aStore)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                if (!ap.GetStoreItemTempLock(aColor.ColorCodeRID, aStore))
                {
                    return false;
                }
            }
            return true;
        }

         /// <summary>
        /// Gets Store Item TempLock audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aSize">HdrSizeBin for the size where the store resides.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>True when Store Item TempLock; false otherwise.</returns>
        override internal bool GetStoreItemTempLock(HdrSizeBin aSize, Index_RID aStore)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                // begin TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                HdrSizeBin sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aSize.Color.ColorCodeRID).ColorSizes[aSize.SizeCodeRID];
                if (sizeBin != null)
                {
                    if (!ap.GetStoreItemTempLock(sizeBin, aStore))
                    //if (!ap.GetStoreItemTempLock(aSize.Color.ColorCodeRID, aSize.SizeCodeRID, aStore))
                        // end TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                    {
                        return false;
                    }
                }  // TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
            }
            return true;
        }
        #endregion GetStoreItemTempLock

        #region SetStoreItemTempLock
        /// <summary>
        /// Sets Store Item TempLock audit flag for specified store on specified node.
        /// </summary>
        /// <param name="aAllocationSummaryNode">Allocation summary node.</param>
        /// <param name="aStore">Index_RID for store</param>
        /// <param name="aFlagValue">True when Store Item TempLock; false otherwise.</param>
        override internal void SetStoreItemTempLock(eAllocationSummaryNode aAllocationSummaryNode, Index_RID aStore,
            bool aFlagValue, eDistributeChange aDistributeChange)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            foreach (AllocationProfile ap in apList)
            {
                ap.SetStoreItemTempLock(aAllocationSummaryNode, aStore, aFlagValue, aDistributeChange);
            }
        }
 
        /// <summary>
        /// Sets Store Item TempLock audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <param name="aFlagValue">True when Store Item TempLock; false otherwise.</param>
        /// <param name="aDistributeChange">Indicates how the change should be reflected in parent and children nodes</param>
        override internal void SetStoreItemTempLock(HdrColorBin aColor, Index_RID aStore, bool aFlagValue, eDistributeChange aDistributeChange)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                ap.SetStoreItemTempLock(aColor.ColorCodeRID, aStore, aFlagValue, aDistributeChange);
            }
        }

        /// <summary>
        /// Sets Store Item TempLock audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aSize">HdrSizeBin for the size where the store resides.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <param name="aFlagValue">True when Store Item TempLock; false otherwise.</param>
        /// <param name="aDistributeChange">Indicates how the change should be reflected in parent and children nodes.</param>
        override internal void SetStoreItemTempLock(HdrSizeBin aSize, Index_RID aStore, bool aFlagValue, eDistributeChange aDistributeChange)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                // begin TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                HdrSizeBin sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aSize.Color.ColorCodeRID).ColorSizes[aSize.SizeCodeRID];
                if (sizeBin != null)
                {
                    ap.SetStoreItemTempLock(sizeBin, aStore, aFlagValue, aDistributeChange);
                    //ap.SetStoreItemTempLock(aSize.Color.ColorCodeRID, aSize.SizeCodeRID, aStore, aFlagValue, aDistributeChange);
                }
                // end TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
            }
        }
        #endregion SetStoreItemTempLock
        #endregion StoreTempLock

        #region StoreOut
        #region GetStoreOut
        /// <summary>
        /// Gets Store Out audit flag for specified store on specified node.
        /// </summary>
        /// <param name="aAllocationSummaryNode">Allocation summary node.</param>
        /// <param name="aStore">Index_RID for store</param>
        /// <returns>True when store Out; false otherwise.</returns>
        override internal bool GetStoreOut(eAllocationSummaryNode aAllocationSummaryNode, Index_RID aStore)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            foreach (AllocationProfile ap in apList)
            {
                if (!ap.GetStoreOut(aAllocationSummaryNode, aStore))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets Store Out audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>True when store Out; false otherwise.</returns>
        override internal bool GetStoreOut(HdrColorBin aColor, Index_RID aStore)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                if (!ap.GetStoreOut(aColor.ColorCodeRID, aStore))
                {
                    return false;
                }
            }
            return true;
        }

        //xxxx;  // how should we handle size out on an assortment?????
        ///// <summary>
        ///// Gets Store Out audit flag for specified store on specified color node.
        ///// </summary>
        ///// <param name="aSize">HdrSizeBin for the size where the store resides.</param>
        ///// <param name="aStore">Index_RID for the store</param>
        ///// <returns>True when store Out; false otherwise.</returns>
        //internal bool GetStoreOut(HdrSizeBin aSize, Index_RID aStore)
        //{
        //    // begin TT#1391 - TMW New Action
        //    SizeNeedMethod snm = GetSizeNeedMethod(aSize.Color);
        //    if (snm != null)
        //    {
        //        return snm.IsStoreExcluded(aStore.RID, aSize.Color.ColorCodeRID, aSize.SizeCodeRID);
        //    }
        //    return false;
        //    //return aSize.GetStoreSizeOut(aStore.Index);
        //    // end TT#1391 - TMW New Action
        //}
        //xxxx;  // how should we handle size out on an assortment?????
        #endregion GetStoreOut

        #region SetStoreOut
        /// <summary>
        /// Sets Store Out audit flag for specified store on specified node.
        /// </summary>
        /// <param name="aAllocationSummaryNode">Allocation summary node.</param>
        /// <param name="aStore">Index_RID for store</param>
        /// <param name="aFlagValue">True when store Out; false otherwise.</param>
        override internal void SetStoreOut(eAllocationSummaryNode aAllocationSummaryNode, Index_RID aStore,
            bool aFlagValue, eDistributeChange aDistributeChange)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            foreach (AllocationProfile ap in apList)
            {
                ap.SetStoreOut(aAllocationSummaryNode, aStore, aFlagValue, aDistributeChange);
            }
        }

        /// <summary>
        /// Sets Store Out audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <param name="aFlagValue">True when store Out; false otherwise.</param>
        /// <param name="aDistributeChange">Indicates how the change should be reflected in parent and children nodes</param>
        override internal void SetStoreOut(HdrColorBin aColor, Index_RID aStore, bool aFlagValue, eDistributeChange aDistributeChange)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                ap.SetStoreOut(aColor.ColorCodeRID, aStore, aFlagValue, aDistributeChange); // TT#915 - MD - Jellis - Group Allocation Stack Overflow on Cancel Allocation
            }
        }

        /// <summary>
        /// Sets Store Out audit flag for specified store on specified color node.
        /// </summary>
        /// <param name="aSize">HdrSizeBin for the size where the store resides.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <param name="aFlagValue">True when store Out; false otherwise.</param>
        /// <param name="aDistributeChange">Indicates how the change should be reflected in parent and children nodes.</param>
        override internal void SetStoreOut(HdrSizeBin aSize, Index_RID aStore, bool aFlagValue, eDistributeChange aDistributeChange)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                // begin TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                HdrSizeBin sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aSize.Color.ColorCodeRID).ColorSizes[aSize.SizeCodeRID];
                if (sizeBin != null)
                {
                    ap.SetStoreOut(sizeBin, aStore, aFlagValue, aDistributeChange);
                    //ap.SetStoreOut(aSize.Color.ColorCodeRID, aStore, aFlagValue, aDistributeChange);
                }
                // TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
            }
        }
        #endregion SetStoreOut
        #endregion StoreOut

        #region StoreOrigQtyAllocated
        #region GetStoreOrigQtyAllocated
        /// <summary>
        /// Gets Store Original Quantity Allocated for specified store on specified node.
        /// </summary>
        /// <param name="aAllocationSummaryNode">Allocation summary node.</param>
        /// <param name="aStore">Index_RID for store</param>
        /// <returns>Original Quantity Allocated to the store for the specified node.</returns>
        override internal int GetStoreOrigQtyAllocated(eAllocationSummaryNode aAllocationSummaryNode, Index_RID aStore)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            int allocated = 0;
            foreach (AllocationProfile ap in apList)
            {
                allocated += ap.GetStoreOrigQtyAllocated(aAllocationSummaryNode, aStore);
            }
            return allocated;
        }

        /// <summary>
        /// Gets Store Original Quantity Allocated for specified store on specified color node.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>Original Quantity Allocated to the store for the specified node.</returns>
        override internal int GetStoreOrigQtyAllocated(HdrColorBin aColor, Index_RID aStore)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            int allocated = 0;
            foreach (AllocationProfile ap in apList)
            {
                allocated += ap.GetStoreOrigQtyAllocated(aColor.ColorCodeRID, aStore);
            }
            return allocated;
        }

        /// <summary>
        /// Gets Store Original Quantity Allocated for specified store on specified color node.
        /// </summary>
        /// <param name="aSize">HdrSizeBin for the size where the store resides.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>Original Quantity Allocated to the store for the specified node.</returns>
        override internal int GetStoreOrigQtyAllocated(HdrSizeBin aSize, Index_RID aStore)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);
            int allocated = 0;
            foreach (AllocationProfile ap in apList)
            {
                 // begin TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                HdrSizeBin sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aSize.Color.ColorCodeRID).ColorSizes[aSize.SizeCodeRID];
                if (sizeBin != null)
                {
                    allocated += ap.GetStoreOrigQtyAllocated(sizeBin, aStore);
                    //allocated += ap.GetStoreOrigQtyAllocated(aSize.Color.ColorCodeRID, aStore);
                }
                // end TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
            }
            return allocated;
        }
        #endregion GetStoreOrigQtyAllocated

        #endregion StoreOrigQtyAllocated

        #region StoreQtyAllocated
        #region GetStoreQtyAllocated
        /// <summary>
        /// Gets Store Quantity Allocated for specified store on specified node.
        /// </summary>
        /// <param name="aAllocationSummaryNode">Allocation summary node.</param>
        /// <param name="aStore">Index_RID for store</param>
        /// <returns>Quantity Allocated to the store for the specified node.</returns>
        override internal int GetStoreQtyAllocated(eAllocationSummaryNode aAllocationSummaryNode, Index_RID aStore)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            int allocated = 0;
            foreach (AllocationProfile ap in apList)
            {
                allocated += ap.GetStoreQtyAllocated(aAllocationSummaryNode, aStore);
            }
            return allocated;
        }

        /// <summary>
        /// Gets Store Quantity Allocated for specified store on specified color node.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>Quantity Allocated to the store for the specified node.</returns>
        override internal int GetStoreQtyAllocated(HdrColorBin aColor, Index_RID aStore)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            int allocated = 0;
            foreach (AllocationProfile ap in apList)
            {
                allocated += ap.GetStoreQtyAllocated(aColor.ColorCodeRID, aStore);
            }
            return allocated;
        }

        /// <summary>
        /// Gets Store Quantity Allocated for specified store on specified color node.
        /// </summary>
        /// <param name="aSize">HdrSizeBin for the size where the store resides.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>Quantity Allocated to the store for the specified node.</returns>
        override internal int GetStoreQtyAllocated(HdrSizeBin aSize, Index_RID aStore)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);
            int allocated = 0;
            foreach (AllocationProfile ap in apList)
            {
                // begin TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                HdrSizeBin sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aSize.Color.ColorCodeRID).ColorSizes[aSize.SizeCodeRID];
                if (sizeBin != null)
                {
                    allocated += ap.GetStoreQtyAllocated(sizeBin, aStore);
                    //allocated += ap.GetStoreQtyAllocated(aSize.Color.ColorCodeRID, aSize.SizeCodeRID, aStore);
                }
                // end TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
            }
            return allocated;
        }
        #endregion GetStoreQtyAllocated

        #region SetStoreQtyAllocated


		// Begin TT#1713-MD - stodd - Velocity is only allocating to a single store when running against a Group Allocation
        override internal eAllocationUpdateStatus SetStoreQtyAllocated(eAllocationSummaryNode aAllocationSummaryNode, 
            Index_RID aStore, int aQtyAllocated, eDistributeChange aDistributeChange, bool aIsManuallyAllocated, bool aPermitMultiHdrUpdtWhenIntransit, bool aApplyTempLock)   // TT#59 Implement Temp Locks
        {
            int qtyAllocated = aQtyAllocated;
            // Begin TT#4810 - JSmith - Manual change in Group Style Review adds qty instead of replaces qty
            //return SetStoreQtyAllocated(aAllocationSummaryNode, aStore, ref qtyAllocated, aDistributeChange, aIsManuallyAllocated, aPermitMultiHdrUpdtWhenIntransit, aApplyTempLock, false);
            if (!aIsManuallyAllocated)
            {
                _setAllocationZero = true;  // TT#1772-MD - JSmith - GA-> ppk and bulk same style/color -> Velocity WOS , cannot balance remaining 14 units and headers have positive on pack and negative on bulk
                return SetStoreQtyAllocated(aAllocationSummaryNode, aStore, ref qtyAllocated, aDistributeChange, aIsManuallyAllocated, aPermitMultiHdrUpdtWhenIntransit, aApplyTempLock, false);
            }
            else
            {
                bool atryToBalance = true;
                eAllocationUpdateStatus allocUpdateStatus = SetStoreQtyAllocated(aAllocationSummaryNode, aStore, ref qtyAllocated, aDistributeChange, aIsManuallyAllocated, aPermitMultiHdrUpdtWhenIntransit, aApplyTempLock, atryToBalance);
                if (qtyAllocated > 0 && atryToBalance)
                {
                    atryToBalance = false;
                    bool unitsToAllocate = true;
                    int attempts = 0;
                    int currentAllocated = qtyAllocated;
                    while (unitsToAllocate)
                    {
                        allocUpdateStatus = SetStoreQtyAllocated(aAllocationSummaryNode, aStore, ref qtyAllocated, aDistributeChange, aIsManuallyAllocated, aPermitMultiHdrUpdtWhenIntransit, aApplyTempLock, atryToBalance);
                        ++attempts;
                        if (qtyAllocated == 0 ||
                            attempts > 10 ||
                            currentAllocated == qtyAllocated)
                        {
                            unitsToAllocate = false;
                        }
                        currentAllocated = qtyAllocated;
                    }
                }

                return allocUpdateStatus;
            }
            // End TT#4810 - JSmith - Manual change in Group Style Review adds qty instead of replaces qty

        }
		// End TT#17##-MD - stodd - Velocity is only allocating to a single store when running against a Group Allocation

		// Begin TT#4734 - stodd - Running Velocity with Reconcile turned on for a Group that has been previously allocated, the Quantity Allocated column does not contain the previous allocation data
        override internal eAllocationUpdateStatus SetStoreQtyAllocated(eAllocationSummaryNode aAllocationSummaryNode,
            Index_RID aStore, ref int aQtyAllocated, eDistributeChange aDistributeChange, bool aIsManuallyAllocated, bool aPermitMultiHdrUpdtWhenIntransit, bool aApplyTempLock, bool tryToBalance)   
        {
            return SetStoreQtyAllocated(aAllocationSummaryNode, aStore, ref aQtyAllocated, aDistributeChange, aIsManuallyAllocated, aPermitMultiHdrUpdtWhenIntransit, aApplyTempLock, tryToBalance, false);

        }
		// End TT#4734 - stodd - Running Velocity with Reconcile turned on for a Group that has been previously allocated, the Quantity Allocated column does not contain the previous allocation data

        //override internal eAllocationUpdateStatus SetStoreQtyAllocatedBase(eAllocationSummaryNode aAllocationSummaryNode,
        //    Index_RID aStore, ref int aQtyAllocated, eDistributeChange aDistributeChange, bool aIsManuallyAllocated, bool aPermitMultiHdrUpdtWhenIntransit, bool aApplyTempLock, bool tryToBalance)
        //{
        //    return base.SetStoreQtyAllocated(aAllocationSummaryNode, aStore, ref aQtyAllocated, aDistributeChange, aIsManuallyAllocated, aPermitMultiHdrUpdtWhenIntransit, aApplyTempLock, tryToBalance, false);
        //}


        /// <summary>
        /// Sets Store Quantity Allocated for specified store on specified node.
        /// </summary>
        /// <param name="aAllocationSummaryNode">Allocation summary node.</param>
        /// <param name="aStore">Index_RID for store</param>
        /// <param name="aQtyAllocated">Quantity Allocated</param>
        /// <param name="aDistributeChange">Indicates how these units should be distributed to parent and children components.</param>
        /// <param name="aIsManuallyAllocated">True indicates a manually key value; false indicate otherwise.</param>
        /// <param name="aPermitMultiHdrUpdtWhenIntransit">True: allows multi header to change even when intransit; False: Intransit inhibits change</param>
        /// <param name="aApplyTempLock">True:  Apply Temporary Locks; False:  Do not apply temporary locks (Locks should only be applied at point of the initial change for a store</param>
        /// <returns>Status of the update</returns>
		// Begin TT#1713-MD - stodd - Velocity is only allocating to a single store when running against a Group Allocation
		// Begin TT#4734 - stodd - Running Velocity with Reconcile turned on for a Group that has been previously allocated, the Quantity Allocated column does not contain the previous allocation data
        override internal eAllocationUpdateStatus SetStoreQtyAllocated(eAllocationSummaryNode aAllocationSummaryNode, 
            Index_RID aStore, ref int aQtyAllocated, eDistributeChange aDistributeChange, bool aIsManuallyAllocated, bool aPermitMultiHdrUpdtWhenIntransit, bool aApplyTempLock, bool tryToBalance, bool overrideToBase)   // TT#59 Implement Temp Locks
 		// End TT#4734 - stodd - Running Velocity with Reconcile turned on for a Group that has been previously allocated, the Quantity Allocated column does not contain the previous allocation data
        {
			// Begin TT#4734 - stodd - Running Velocity with Reconcile turned on for a Group that has been previously allocated, the Quantity Allocated column does not contain the previous allocation data
            // stodd - when using velocity with reconcile turned on, the base method needs to be called for the assortment profile header to restore the original allocation on the header.
            if (overrideToBase)
            {
                return base.SetStoreQtyAllocated(aAllocationSummaryNode, aStore, ref aQtyAllocated, aDistributeChange, aIsManuallyAllocated, aPermitMultiHdrUpdtWhenIntransit, aApplyTempLock, tryToBalance, overrideToBase);

            }
			// End TT#4734 - stodd - Running Velocity with Reconcile turned on for a Group that has been previously allocated, the Quantity Allocated column does not contain the previous allocation data
		// End TT#1713-MD - stodd - Velocity is only allocating to a single store when running against a Group Allocation
            eAllocationUpdateStatus spreadUpdateStatus = eAllocationUpdateStatus.Successful;
            eAllocationUpdateStatus allocationUpdateStatus;
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            MIDGenericSortItem[] mgsi = new MIDGenericSortItem[apList.Length]; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            int i = 0;

            int totalAllocated = aQtyAllocated;
            int spreadBasis = 0;
            
            bool ruleAllowsMoreAllocation;  // TT#1085 - MD - Jellis - Out Stores Get Allocation
            int maximum;                    // TT#4212 - MD - Jellis - Velocity GA Minimums not observed
            int maxPossibleAllocation = 0;  // TT#4212 - MD - Jellis - Velocity GA Minimums not observed
            // begin TT#1436 - MD - Jellis - ??????
            GeneralComponent gc;
            int componentUnitsToAllocate;
            int componentUnitsAllocated;
            switch (aAllocationSummaryNode)
            {
                case (eAllocationSummaryNode.Bulk):
                    {
                        gc = new GeneralComponent(eComponentType.Bulk);
                        break;
                    }
                case (eAllocationSummaryNode.DetailType):
                    {
                        gc = new GeneralComponent(eComponentType.DetailType);
                        break;
                    }
                case (eAllocationSummaryNode.Total):
                default:
                    {
                        gc = new GeneralComponent(eComponentType.Total);
                        break;
                    }
            }
            componentUnitsToAllocate = GetQtyToAllocate(gc);
            componentUnitsAllocated = GetQtyAllocated(gc);
            // end TT#1436 - MD - Jellis - ??????
            //// Begin TT#4771 - stodd - Group Allocation Smoothing Test 5.4.5765 
            //int maximum;                    // TT#4212 - MD - Jellis - Velocity GA Minimums not observed
            //int maxPossibleAllocation = 0;  // TT#4212 - MD - Jellis - Velocity GA Minimums not observed
            //// End TT#4771 - stodd - Group Allocation Smoothing Test 5.4.5765 
            if (!this._processingRules                                             // TT#1117 - MD - Jellis - Units Allocated Less than Min
                && GetStoreQtyAllocated(aAllocationSummaryNode, aStore) > 0) // TT#1117 - MD - Jellis - Units Allocated Less than Min
            {
                // when store was previously allocated, use its allocation as the basis
                int basisValue;
                foreach (AllocationProfile ap in apList)
                {
                    basisValue = ap.GetStoreQtyAllocated(aAllocationSummaryNode, aStore);
                    spreadBasis += basisValue;
                    mgsi[i] = new MIDGenericSortItem();
                    mgsi[i].Item = i;
                    // begin TT#1117 - MD - Jellis - Units Allocated less than Min
                    //mgsi[i].SortKey = new double[2];
                    //mgsi[i].SortKey[0] = basisValue;
                    //mgsi[i].SortKey[1] = AppSessionTransaction.GetRandomDouble();
                    mgsi[i].SortKey = new double[5];  // TT#4145 - Jellis - Urban - Group Minimum Not Holding // TT#4212 - MD - Jellis - Velocity Group Minimum not observed
                    // want largest min-max spread at end so it can absorb minimum or maximum constraints on earlier headers
                    maximum = ap.GetStoreMaximum(aAllocationSummaryNode, aStore, true); // TT#4212 - MD - Jellis - Velocity Group Minimum not observed
                    //mgsi[i].SortKey[0] =  // TT#1566 - MD - Jellis - GA Min Rule Results in allocation less than Min
                    mgsi[i].SortKey[1] =    // TT#1566 - MD - Jellis - GA Min Rule Results in allocation less than Min
                             Math.Max(0,                                           // TT#1143 - MD - Jellis - Group Allocation Min Broken
                                     //ap.GetStoreMaximum(aAllocationSummaryNode, aStore, true) // TT#4212 - MD - Jellis - Velocity Group Minimum not observed
                                     maximum                                                   // TT#4212 - MD - Jellis - Velocity Group Minimum not observed
                                     - ap.GetStoreMinimum(aAllocationSummaryNode, aStore, true));
                    // begin TT#4145 - Jellis - Urban - Group Minimum Not Holding
                    //mgsi[i].SortKey[1] = // TT#1566 - MD - Jellis - GA Min Rule Results in allocation less than Min
                    if (ap.TotalUnitsToAllocate == 0)
                    {
                        mgsi[i].SortKey[1] = 0;
                    }
                    else
                    {
                    mgsi[i].SortKey[0] =   // TT#1566 - MD - Jellis - GA Min Rule Results in allocation less than Min
                            ap.BulkUnitsToAllocate
                            / ap.TotalUnitsToAllocate;
                    }
					// End TT#1548-MD - stodd - Post Receipt Asst - Try to run Spread Avg on headers and receive a severe error.
                    // end TT#4145 - Jellis - Urban - Group Minimum Not Holding
                    mgsi[i].SortKey[2] = basisValue; // TT#4145 - Jellis - Urban - Group Minimum Not Holding
                    // begin TT#4212 - MD - Jellis - Velocity Group Minimum not observed
                    if (basisValue == 0)
                    {
                        mgsi[i].SortKey[3] = 0;
                    }
                    else
                    {
                        mgsi[i].SortKey[3] = maximum;
                        if (maxPossibleAllocation < int.MaxValue)
                        {
                            if (maximum < int.MaxValue)
                            {
                                maxPossibleAllocation += maximum;
                            }
                            else
                            {
                                maxPossibleAllocation = int.MaxValue;
                            }
                        }
                    }
                    // end TT#4212 - MD - Jellis - Velocity Group Minimum not observed
                    mgsi[i].SortKey[4] = AppSessionTransaction.GetRandomDouble(); // TT#4145 - Jellis - Urban - Group Minimum Not Holding // TT#4212 - MD - Jellis - Velocity Group Minimum not observed
                    // end TT#4145 - Jellis - Urban - Group Minimum Not Holding
                    i++;

                    // Begin TT#4810 - JSmith - Manual change in Group Style Review adds qty instead of replaces qty
                    // Begin TT#1772-MD - JSmith - GA-> ppk and bulk same style/color -> Velocity WOS , cannot balance remaining 14 units and headers have positive on pack and negative on bulk
                    //if (aIsManuallyAllocated &&
                    //    tryToBalance)
                    if ((aIsManuallyAllocated && tryToBalance) ||
                        _setAllocationZero)
                    // End TT#1772-MD - JSmith - GA-> ppk and bulk same style/color -> Velocity WOS , cannot balance remaining 14 units and headers have positive on pack and negative on bulk
                    {
                        ap.SetStoreQtyAllocated(aAllocationSummaryNode, aStore, 0, eDistributeChange.ToAll, false, false);
                    }
                    // End TT#4810 - JSmith - Manual change in Group Style Review adds qty instead of replaces qty
                }
            }
            else
            if (!_processingRules        // TT#4145 - Urban - Jellis - Group Min not holding
                && componentUnitsToAllocate > componentUnitsAllocated) // TT#1436 - MD - Jellis - ?????
                //&& TotalUnitsToAllocate > TotalUnitsAllocated) // TT#4145 - Urban - Jellis - Group Min not holding // TT#1436 - MD - Jellis - ????
            {
                // when store is not previously allocated, but some stores are, use remaining to allocate as basis (if possible)
                int basisValue; 
                foreach (AllocationProfile ap in apList)
                {
                    // begin TT#1085 - MD - Jellis - Out Stores Get Allocation
                    ruleAllowsMoreAllocation =
                        (Enum.IsDefined(typeof(eChosenRuleAllowsMoreAllocation), (int)ap.GetStoreChosenRuleType(aAllocationSummaryNode, aStore)));
                    if (!ruleAllowsMoreAllocation
                        || ap.GetStoreIsManuallyAllocated(aAllocationSummaryNode, aStore)
                        || ap.GetStoreOut(aAllocationSummaryNode, aStore)
                        || ap.GetStoreLocked(aAllocationSummaryNode, aStore)
                        || ap.GetStoreTempLock(aAllocationSummaryNode, aStore)
                        )
                    {
                        basisValue = 0;
                    }
                    else
                    {
                        // begin TT#1436 - MD - Jellis - ?????
                        //// end TT#1085 - MD - Jellis - Out Stores Get Allocation
                        //basisValue =
                        //    ap.TotalUnitsToAllocate
                        //    - ap.TotalUnitsAllocated;
                        basisValue =
                            ap.GetQtyToAllocate(gc)
                            - ap.GetQtyAllocated(gc);
                        // end TT#1436 - MD - Jellis - ??????
                    }  // TT#1085 - MD - Jellis - Out Stores Get Allocation
                    mgsi[i] = new MIDGenericSortItem();
                    mgsi[i].Item = i;
                    mgsi[i].SortKey = new double[5]; // TT#1117 - MD - Jellis - Units Allocated less than min // TT#4145 - Jellis - Urban - Group Minimum Not Holding // TT#4212 - MD - Jellis - Velocity Group Minimum not observed
                    if (basisValue > 0)
                    {
                        spreadBasis += basisValue;

                        // begin TT#1117 - MD - Jellis - Units Allocated less than Min
                        //mgsi[i].SortKey[0] = basisValue;
                        //mgsi[i].SortKey[1] = AppSessionTransaction.GetRandomDouble();
                        
                        // want the largest min-max spread at end so it can absorb minimum or maximum constraints on earlier headers
                        maximum = ap.GetStoreMaximum(aAllocationSummaryNode, aStore, true); // TT#4212 - MD - Jellis - Velocity Group Minimum not observed
                        //mgsi[i].SortKey[0] = // TT#1566 - MD - Jellis - GA Min Rule Results in allocation less than Min
                        mgsi[i].SortKey[1] =   // TT#1566 - MD - Jellis - GA Min Rule Results in allocation less than Min
                              Math.Max(0,                                           // TT#1143 - MD - Jellis - Group Allocation Min Broken
                                     //ap.GetStoreMaximum(aAllocationSummaryNode, aStore, true) // TT#4212 - MD - Jellis - Velocity Group Minimum not observed
                                     maximum                                                    // TT#4212 - MD - Jellis - Velocity Group Minimum not observed
                                     - ap.GetStoreMinimum(aAllocationSummaryNode, aStore, true));
                        // begin TT#4145 - Jellis - Urban - Group Minimum Not Holding
                        //mgsi[i].SortKey[1] = // TT#1566 - MD - Jellis - GA Min Rule Results in allocation less than Min
                        if (ap.TotalUnitsToAllocate == 0)
                        {
                            mgsi[i].SortKey[1] = 0;
                        }
                        else
                        {
                        mgsi[i].SortKey[0] =   // TT#1566 - MD - Jellis - GA Min Rule Results in allocation less than Min
                                ap.BulkUnitsToAllocate
                                / ap.TotalUnitsToAllocate;
                        }
						// End TT#1548-MD - stodd - Post Receipt Asst - Try to run Spread Avg on headers and receive a severe error.
                        // end TT#4145 - Jellis - Urban - Group Minimum Not Holding
                        mgsi[i].SortKey[2] = basisValue; // TT#4145 - Jellis - Urban - Group Minimum Not Holding
                        // begin TT#4212 - MD - Jellis - Velocity Group Minimum not observed
                        if (basisValue == 0)
                        {
                            mgsi[i].SortKey[3] = 0;
                        }
                        else
                        {
                            mgsi[i].SortKey[3] = maximum;
                            if (maxPossibleAllocation < int.MaxValue)
                            {
                                if (maximum < int.MaxValue)
                                {
                                    maxPossibleAllocation += maximum;
                                }
                                else
                                {
                                    maxPossibleAllocation = int.MaxValue;
                                }
                            }
                        }
                        mgsi[i].SortKey[4] = AppSessionTransaction.GetRandomDouble(); // TT#4145 - Jellis - Urban - Group Minimum Not Holding // TT#4212 - MD - Jellis - Velocity Group Minimum not observed
                        // end TT#1117 - MD - Jellis - Units Allocated Less than Min
                    }
                    else
                    {
                        mgsi[i].SortKey[0] = 0;
                        mgsi[i].SortKey[1] = 0;  // TT#1117 - MD - Jellis - Units Allocated less than min
                        mgsi[i].SortKey[2] = 0; // TT#1117 - MD - Jellis -Units Allocated less than min
                        mgsi[i].SortKey[3] = 0;  // TT#4145 - Jellis - Urban - Group Minimum Not Holding
                        mgsi[i].SortKey[4] = 0;  // TT#4212 - MD - Jellis - Velocity Group Minimum not observed 
                    }
                    i++;
                }
            }
            else 
            {
                // default is to use units to allocate as the basis
                int basisValue; // TT#1085 - MD - Jellis - Out Stores Get Allocation
                // begin TT#4213 - MD - Jellis - GA Velocity unexpected results 
                // NOTE:  multi-layer rules in GA MUST have the allocations from all member headers zeroed out before applying rule 
                //        this is due to the way inventory minimum and maximums are calculated
                if (_processingRules)
                {
                    foreach (AllocationProfile ap in apList)
                    {
                        // Begin TT#4983 - JSmith - GA- Out rule not being honored 
                        if (GetStoreOut(aAllocationSummaryNode, aStore)  
                            && !ap.GetStoreIsManuallyAllocated(aAllocationSummaryNode, aStore)  
                            && !ap.GetStoreLocked(aAllocationSummaryNode, aStore)
                            && !ap.GetStoreTempLock(aAllocationSummaryNode, aStore)
                            && aQtyAllocated == 0
                        )
                        {
                            ap.SetStoreQtyAllocated(aAllocationSummaryNode, aStore, 0, eDistributeChange.ToAll, false, false);
                        }
                        else if (ap.GetStoreIsManuallyAllocated(aAllocationSummaryNode, aStore)
                        //if (ap.GetStoreIsManuallyAllocated(aAllocationSummaryNode, aStore)
                        // End TT#4983 - JSmith - GA- Out rule not being honored
                        || ap.GetStoreOut(aAllocationSummaryNode, aStore)
                        || ap.GetStoreLocked(aAllocationSummaryNode, aStore)
                        || ap.GetStoreTempLock(aAllocationSummaryNode, aStore)
                        || !tryToBalance // TT#4699 - JSmith - GA - Group Minimum is not being honored in Velocity
                        )
                        {
                            // Do not zero out allocations that rule will not affect
                        }
                        else
                        {
                            ap.SetStoreQtyAllocated(aAllocationSummaryNode, aStore, 0, eDistributeChange.ToAll, false, false);
                        }
                    }
                }
                foreach (AllocationProfile ap in apList)
                {
                    // begin TT#1085 - MD - Jellis - Out Stores Get Allocation
                    // begin TT#4213 - MD - Jellis - GA Velocity Unexpected results
                    if (!_processingRules)
                    {
                        // end TT#4213 - MD - Jellis - GA Velocity Unexpected results
                        ruleAllowsMoreAllocation =
                            (Enum.IsDefined(typeof(eChosenRuleAllowsMoreAllocation), (int)ap.GetStoreChosenRuleType(aAllocationSummaryNode, aStore)));
                        // begin TT#4213 - MD - Jellis - GA Velocity Unexpected results
                    }
                    else
                    {
                        ruleAllowsMoreAllocation = true;
                    }
                    // end TT#4213 - MD - Jellis - GA Velocity Unexpected results
                    if (!ruleAllowsMoreAllocation
                        || ap.GetStoreIsManuallyAllocated(aAllocationSummaryNode, aStore)
                        || ap.GetStoreOut(aAllocationSummaryNode, aStore)
                        || ap.GetStoreLocked(aAllocationSummaryNode, aStore)
                        || ap.GetStoreTempLock(aAllocationSummaryNode, aStore)
                        )
                    {
                        basisValue = 0;
                    }
                    else
                    {
                        // begin TT#1436 - MD - Jellis - ?????
                        // end TT#1085 - MD - Jellis - Out Stores Get Allocation
                        // Begin TT#4530 - stodd - Group Allocation with Packs v 5.4
                        //basisValue =
                        //    ap.TotalUnitsToAllocate;
						// Begin TT#1713-MD - stodd - Velocity is only allocating to a single store when running against a Group Allocation
                        if (tryToBalance)
                        {
                            basisValue =
                                ap.GetQtyToAllocate(gc) - ap.GetQtyAllocated(gc);
                        }
                        else
                        {
                            basisValue = ap.GetQtyToAllocate(gc);
                        }
						// End TT#1713-MD - stodd - Velocity is only allocating to a single store when running against a Group Allocation
                        // End TT#4530 - stodd - Group Allocation with Packs v 5.4
                    }  // TT#1085 - MD - Jellis - Out Stores Get Allocation
                    spreadBasis += basisValue; // TT#1085 - MD - Jellis - Out Stores Get Allocation
                    mgsi[i] = new MIDGenericSortItem();
                    mgsi[i].Item = i;
                    // begin TT#1117 - MD - Jellis - Units Allocated less than Min
                    //mgsi[i].SortKey = new double[2];
                    //mgsi[i].SortKey[0] = basisValue; // TT#1085 - MD - Jellis - Out Stores Get Allocation
                    //mgsi[i].SortKey[1] = AppSessionTransaction.GetRandomDouble(); 
					// Begin TT#4771 - stodd - Group Allocation Smoothing Test 5.4.5765 
                    mgsi[i].SortKey = new double[5];  // TT#4145 - Jellis - Urban - Group Minimum Not Holding
                    // want the largest min-max spread at end so it can absorb minimum or maximum constraints on earlier headers
                    maximum = ap.GetStoreMaximum(aAllocationSummaryNode, aStore, true); // TT#4212 - MD - Jellis - Velocity Group Minimum not observed
                    //mgsi[i].SortKey[0] = // TT#1566 - MD - Jellis - GA Min Rule Results in allocation less than Min
                    mgsi[i].SortKey[1] =   // TT#1566 - MD - Jellis - GA Min Rule Results in allocation less than Min
                          Math.Max(0,                                                         // TT#1143 - MD - Jellis - Group Allocation Min Broken
                                 //ap.GetStoreMaximum(aAllocationSummaryNode, aStore, true)   // TT#4212 - MD - Jellis - Velocity Group Minimum not observed
                                 maximum                                                      // TT#4212 - MD - Jellis - Velocity Group Minimum not observed  
                                 - ap.GetStoreMinimum(aAllocationSummaryNode, aStore, true));
                    // begin TT#4145 - Jellis - Urban - Group Minimum Not Holding
                    //mgsi[i].SortKey[1] = // TT#1566 - MD - Jellis - GA Min Rule Results in allocation less than Min
					// Begin TT#1548-MD - stodd - Post Receipt Asst - Try to run Spread Avg on headers and receive a severe error.
                    if (ap.TotalUnitsToAllocate == 0)
                    {
                        mgsi[i].SortKey[0] = 0;
                    }
                    else
                    {
                        mgsi[i].SortKey[0] =
                            ap.BulkUnitsToAllocate
                            / ap.TotalUnitsToAllocate;
                    }
					// End TT#1548-MD - stodd - Post Receipt Asst - Try to run Spread Avg on headers and receive a severe error.
                    // end TT#4145 - Jellis - Urban - Group Minimum Not Holding
                    mgsi[i].SortKey[2] = basisValue;   // TT#4145 - Jellis - Urban - Group Minimum Not Holding
                    // begin TT#4212 - MD - Jellis - Velocity Group Minimum not observed
                    if (basisValue == 0)
                    {
                        mgsi[i].SortKey[3] = 0;
                    }
                    else
                    {
                        mgsi[i].SortKey[3] = maximum;
                        if (maxPossibleAllocation < int.MaxValue)
                        {
                            if (maximum < int.MaxValue)
                            {
                                maxPossibleAllocation += maximum;
                            }
                            else
                            {
                                maxPossibleAllocation = int.MaxValue;
                            }
                        }
                    }
                    mgsi[i].SortKey[4] = AppSessionTransaction.GetRandomDouble(); // TT#4145 - Jellis - Urban - Group Minimum Not Holding // TT#4212 - MD - Jellis - Velocity Group Minimum not observed 
					// End TT#4771 - stodd - Group Allocation Smoothing Test 5.4.5765 
                    // end TT#1117 - MD - Jellis - Units Allocated Less than Min

                    i++;
                }
            }
            //Array.Sort(mgsi, new MIDGenericSortDescendingComparer()); // TT#1143 - MD - Jellis - Group Allocation Min Broken
            Array.Sort(mgsi, new MIDGenericSortAscendingComparer());    // TT#1143 - MD - Jellis - Group Allocation Min Broken
            int allocated;
            AllocationProfile sap;
            int sapBasis;
			// Begin TT#4771 - stodd - Group Allocation Smoothing Test 5.4.5765 
            int minAllocationForSpreadSuccess;
            //int maximum;  // TT#1117 - MD-  JEllis - Allocation Less than Minimum // TT#4212 - MD - Jellis - Velocity Group Minimum not observed
			// End TT#4771 - stodd - Group Allocation Smoothing Test 5.4.5765 
#if (DEBUG)
            string aLine = "SetStoreQtyAllocated() SORT KEY: ";
            foreach (MIDGenericSortItem sortItem in mgsi)
            {
                foreach (double aKey in sortItem.SortKey)
                {
                    aLine += aKey.ToString() + " ";
                }
            }
            //Debug.WriteLine(aLine);
#endif
            foreach (MIDGenericSortItem sortItem in mgsi)
            {
                sapBasis = (int)sortItem.SortKey[2]; // TT#1117 - MD - Jellis - Units allocated less than minimum // TT#4145 - Jellis - Urban - Group Minimum Not Holding
				// Begin TT#4771 - stodd - Group Allocation Smoothing Test 5.4.5765 
                // begin TT#4212 - MD - Jellis - Velocity Group Minimum not observed
                if (maxPossibleAllocation < int.MaxValue)
                {
                    maxPossibleAllocation -= (int)sortItem.SortKey[3];
                    minAllocationForSpreadSuccess = Math.Max(0, totalAllocated - maxPossibleAllocation);  // How much do we have to allocate now in order to successfully spread all of the remaining units allocated (totalAllocated)?
                }
                else
                {
                    minAllocationForSpreadSuccess = 0;
                }
                // end TT#4212 - MD - Jellis - Velocity Group Minimum not observed
				// End TT#4771 - stodd - Group Allocation Smoothing Test 5.4.5765 
                sap = apList[sortItem.Item];
                int currQtyAllocated = sap.GetStoreQtyAllocated(aAllocationSummaryNode, aStore);  // TT#1735-MD - JSmith - Velocity and Capacity

                //Debug.WriteLine("SetStoreQtyAllocated() STORE: " + aStore.RID + " HDR: " + sap.Key + " ALLOCATED: " + "n/a" + " TOTAL ALLOCATED: " + totalAllocated
                //    + " AP BASIS: " + sapBasis + " SPREAD BASIS: " + spreadBasis);

                if (spreadBasis > 0)
                {
                    allocated =
                        (int) (((double)sapBasis
                              * (double) totalAllocated
                              / spreadBasis) + .5d);
					// Begin TT#4771 - stodd - Group Allocation Smoothing Test 5.4.5765 		  
                    // begin TT#4212 - MD - Jellis - Velocity Group Minimum not observed
                    allocated = Math.Max(allocated, minAllocationForSpreadSuccess);
                    // end TT#4212 - MD - Jellis - Velocity Group Minimum not observed
					// End TT#4771 - stodd - Group Allocation Smoothing Test 5.4.5765 
                    // begin TT#1565 - MD - JEllis - Allocation for a Header set-up identically to GA different from GA
                    int multiple = 1;
                    switch (aAllocationSummaryNode)
                    {
                        case (eAllocationSummaryNode.Total):
                        case (eAllocationSummaryNode.Type):
                            {
                                multiple = sap.AllocationMultiple;
                                break;
                            }
                        case (eAllocationSummaryNode.Bulk):
                        case (eAllocationSummaryNode.BulkColorTotal):
                            {
                                multiple = sap.BulkMultiple;
                                break;
                            }
                        case (eAllocationSummaryNode.DetailType):
                        case (eAllocationSummaryNode.DetailSubType):
                            {
                                multiple = DetailTypeMultiple;
                                break;
                            }
                        case (eAllocationSummaryNode.GenericType):
                            {
                                multiple = sap.GenericMultiple;
                                break;
                            }
                    }
                    // end TT#1565 - MD - JEllis - Allocation for a Header set-up identically to GA different from GA
                    // Begin TT#1824-MD - JSmith - Packs not allocating 
                    //allocated =
                    //    (int) ((double) allocated
                    //         / multiple);             // TT#1565 - MD - JEllis - Allocation for a Header set-up identically to GA different from GA // TT#4731 - After running velocity on group, headers in group  are not spread uniformly.
                    if (sap.BulkColors.Count == 0)
                    {
                        allocated = (int) Math.Ceiling((double)allocated
                                 / multiple);
                    }
                    else
                    {
                        allocated =
                            (int)((double)allocated
                                 / multiple);
                    }
                    // End TT#1824-MD - JSmith - Packs not allocating 
                    // / sap.AllocationMultiple + .5d);  // TT#1565 - MD - JEllis - Allocation for a Header set-up identically to GA different from GA
                    allocated =
                        allocated
                        * multiple;                      // TT#1565 - MD - JEllis - Allocation for a Header set-up identically to GA different from GA
                    //*sap.AllocationMultiple;         // TT#1565 - MD - JEllis - Allocation for a Header set-up identically to GA different from GA
                    // begin TT#1117 - MD - JEllis- Allocations Less than Minimum
                    maximum = sap.GetStoreMaximum(aAllocationSummaryNode, aStore, true);

                    //Debug.WriteLine("                   STORE: " + aStore.RID + " HDR: " + sap.Key + " ALLOCATED: " + allocated + " MAX: " + maximum + " CURR QTY ALLOCATED: " + currQtyAllocated);


                    //Debug.WriteLine("SetStoreQtyAllocated() STORE: " + aStore.RID + " HDR: " + sap.Key + " ALLOCATED: " + allocated + " TOTAL ALLOCATED: " + totalAllocated
                    //    + " AP BASIS: " + sapBasis + " SPREAD BASIS: " + spreadBasis + " MAX: " + maximum);

                    // Begin TT#1735-MD - JSmith - Velocity and Capacity
					// Begin TT#5280 - JSmith - GA - Gen PPKS-Method with Inv Min/Max at Style for Header- Velocity with Inv Min/Max style Process as a Group with WOS Rule.  2 stores not getting expected Allocation
                    if (allocated > maximum)
                    {
                        allocated = maximum;
                    }
                    //if (allocated + currQtyAllocated > maximum)
                    //{
                    //    allocated = maximum;
                    //}
                    // End TT#5280 - JSmith - GA - Gen PPKS-Method with Inv Min/Max at Style for Header- Velocity with Inv Min/Max style Process as a Group with WOS Rule.  2 stores not getting expected Allocation
                    // End TT#1735-MD - JSmith - Velocity and Capacity
                    // end TT#1117 - MD - Jellis - ALlocations Less than Minimum
                    if (allocated > totalAllocated)
                    {
                        allocated = totalAllocated;
                    }
					// Begin TT#1713-MD - stodd - Velocity is only allocating to a single store when running against a Group Allocation
                    if (allocated < 0)
                    {
                        allocated = 0;
                    }
                    if (tryToBalance)
                    {
                        if (allocated > sapBasis)
                        {
                            allocated = sapBasis;
                        }
                    }
					// End TT#1713-MD - stodd - Velocity is only allocating to a single store when running against a Group Allocation
                    // begin TT#1117 - MD- Jellis - Allocations Less than Minimum
                    // Begin TT#1735-MD - JSmith - Velocity and Capacity
					//if (allocated < sap.GetStoreMinimum(aAllocationSummaryNode, aStore, true))
                    //{
                    //    allocated = 0;
                    //}
                    if (allocated + currQtyAllocated < sap.GetStoreMinimum(aAllocationSummaryNode, aStore, true))
                    {
                        allocated = 0;
                    }
                    // End TT#1735-MD - JSmith - Velocity and Capacity
                    // end TT#1117 - MD - Jellis- Allocations Less than Minimum

                    //Debug.WriteLine("SetStoreQtyAllocated() STORE: " + aStore.RID + " HDR: " + sap.Key + " ALLOCATED: " + allocated + " TOTAL ALLOCATED: " + totalAllocated
                    //     + " AP BASIS: " + sapBasis + " SPREAD BASIS: " + spreadBasis + " MAX: " + maximum);
                    //Debug.WriteLine("                   STORE: " + aStore.RID + " HDR: " + sap.Key + " ALLOCATED: " + allocated);

                }
                else
                {
                    allocated = 0;
                }
                spreadBasis -= sapBasis;

                // Begin TT#4699 - JSmith - GA - Group Minimum is not being honored in Velocity
                //allocationUpdateStatus = sap.SetStoreQtyAllocated(aAllocationSummaryNode, aStore, allocated, aDistributeChange, aIsManuallyAllocated,
                //    aPermitMultiHdrUpdtWhenIntransit, aApplyTempLock);
				// Begin TT#4721 - stodd - GA - After velocity one or more headers are over allocated and one or more headers are under allocated.
                //int currQtyAllocated = sap.GetStoreQtyAllocated(aAllocationSummaryNode, aStore);    // TT#1735-MD - JSmith - Velocity and Capacity
                int totalUnits = allocated + currQtyAllocated;
				// End TT#4721 - stodd - GA - After velocity one or more headers are over allocated and one or more headers are under allocated.
                //int totalUnits = allocated + sap.GetStoreQtyAllocated(aAllocationSummaryNode, aStore);

                //Debug.WriteLine("                   STORE: " + aStore.RID + " HDR: " + sap.Key + " ALLOCATED: " + allocated + "(FINAL)");
                //Debug.WriteLine("                   STORE: " + aStore.RID + " HDR: " + sap.Key + " NEW SPREAD BASIS: " + spreadBasis);


                allocationUpdateStatus = sap.SetStoreQtyAllocated(aAllocationSummaryNode, aStore, totalUnits, aDistributeChange, aIsManuallyAllocated,
                    aPermitMultiHdrUpdtWhenIntransit, aApplyTempLock);
				// End TT#4699 - JSmith - GA - Group Minimum is not being honored in Velocity
                if (allocationUpdateStatus != eAllocationUpdateStatus.Successful)
                {
                    spreadUpdateStatus = eAllocationUpdateStatus.Failed_SpreadFailed;
                }
                // begin TT#1117 - MD - Jellis - Units allocated less than minimum
				// Begin TT#4699 - JSmith - GA - Group Minimum is not being honored in Velocity
                //totalAllocated -= sap.GetStoreQtyAllocated(aAllocationSummaryNode, aStore);
                // figure out how many of the allocated units are applied to the store
                totalAllocated -= allocated - (totalUnits - sap.GetStoreQtyAllocated(aAllocationSummaryNode, aStore));
				// End TT#4699 - JSmith - GA - Group Minimum is not being honored in Velocity
                //totalAllocated -= allocated;
                // end TT#1117 - MD - Jellis - Units allocated less than minimum

                //Debug.WriteLine("                   STORE: " + aStore.RID + " HDR: " + sap.Key + " NEW TOTAL ALLOCATED: " + totalAllocated);


                aQtyAllocated = totalAllocated;		// TT#1713-MD - stodd - Velocity is only allocating to a single store when running against a Group Allocation

                //Debug.WriteLine("                       STORE: " + aStore.RID + " HDR: " + sap.Key + " ALLOCATED: " + allocated + " TOTAL ALLOCATED: " + totalAllocated
                //     + " AP BASIS: " + sapBasis + " SPREAD BASIS: " + spreadBasis);

            }
            _setAllocationZero = false;  // TT#1772-MD - JSmith - GA-> ppk and bulk same style/color -> Velocity WOS , cannot balance remaining 14 units and headers have positive on pack and negative on bulk
            return spreadUpdateStatus;
        }
        
        // begin TT#1008 - MD - Jellis - Get Non Negative Message for various variables
        /// <summary>
        /// Sets Store Quantity Allocated for specified store on specified pack node.
        /// </summary>
        /// <param name="aPack">Pack header object where store resides.</param>
        /// <param name="aStore">Index_RID for store</param>
        /// <param name="aQtyAllocated">Quantity Allocated.</param>
        /// <param name="aDistributeChange">Indicates how to apply the new value to the parent and/or children components of this component.</param>
        /// <param name="aIsManual">True: indicates this is a manually keyed value.</param>
        /// <param name="aPermitMultiHdrUpdtWhenIntransit">True: allow multi header to change even if intransit; False: Intransit inhibits changes</param>
        /// <param name="aApplyTempLocks">True: set temp lock; False: Do not set temp lock (Only set temp lock when this is the initial value change for the store)</param>
        /// <returns>Status of the update</returns>
        override internal eAllocationUpdateStatus SetStoreQtyAllocated(PackHdr aPack, Index_RID aStore, // TT#488 - MD - JEllis - Group Allocation     // TT#1018 - MD - Jellis - GA Bulk Overallocated
            int aQtyAllocated, eDistributeChange aDistributeChange, bool aIsManual, bool aPermitMultiHdrUpdtWhenIntransit, bool aApplyTempLocks) // TT#59 Temp Locks
        {
            eAllocationUpdateStatus status = eAllocationUpdateStatus.Failed_Other;
            AllocationProfile allocationProfile = GetAssortmentPackHome(aPack.PackRID);
            if (allocationProfile != null)
            {
                status = allocationProfile.SetStoreQtyAllocated(aPack, aStore, aQtyAllocated, aDistributeChange, aIsManual, aPermitMultiHdrUpdtWhenIntransit, aApplyTempLocks);
            }
            return status;
        }
        // end TT#1008 - MD - Jellis - Get Non Negative Message for various variables

		// Begin TT#1713-MD - stodd - Velocity is only allocating to a single store when running against a Group Allocation
        override internal eAllocationUpdateStatus SetStoreQtyAllocated(HdrColorBin aColor, Index_RID aStore,
            int aQtyAllocated, eDistributeChange aDistributeChange, bool aIsManual, bool aPermitMultiHdrUpdtWhenIntransit, bool aApplyTempLocks)   // TT#59 Temp Locks
        {
            int qtyAllocated = aQtyAllocated;
            return SetStoreQtyAllocated(aColor, aStore, ref qtyAllocated, aDistributeChange, aIsManual, aPermitMultiHdrUpdtWhenIntransit, aApplyTempLocks, false); 
        }
		// End TT#1713-MD - stodd - Velocity is only allocating to a single store when running against a Group Allocation
		
        /// <summary>
        /// Sets Store Quantity Allocated for specified store on specified color node.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <param name="aQtyAllocated">Quantity Allocated</param>
        /// <param name="aDistributeChange">Indicates how to apply the new value to parent and children components.</param>
        /// <param name="aIsManual">True: Indicates a manually keyed value by the user.</param>
        /// <param name="aPermitMultiHdrUpdtWhenIntransit">True: multi header may change even when intransit; False: Intransit inhibits change</param>
        /// <param name="aApplyTempLocks">True:  Apply temp locks; False: Do not apply temp locks(only apply temp lock when intial store value change)</param>
        /// <returns>Status of the update.</returns>
		// Begin TT#1713-MD - stodd - Velocity is only allocating to a single store when running against a Group Allocation
        override internal eAllocationUpdateStatus SetStoreQtyAllocated(HdrColorBin aColor, Index_RID aStore,
            ref int aQtyAllocated, eDistributeChange aDistributeChange, bool aIsManual, bool aPermitMultiHdrUpdtWhenIntransit, bool aApplyTempLocks, bool tryToBalance)   // TT#59 Temp Locks
        {
		// End TT#1713-MD - stodd - Velocity is only allocating to a single store when running against a Group Allocation
            eAllocationUpdateStatus spreadUpdateStatus = eAllocationUpdateStatus.Successful;
            eAllocationUpdateStatus allocationUpdateStatus;
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            MIDGenericSortItem[] mgsi = new MIDGenericSortItem[apList.Count];
            
            int spreadBasis = 0;
            int colorUnits;
            HdrColorBin colorBin;
            bool ruleAllowsMoreAllocation; // TT#1085 - MD - Jellis - Out Stores Get Allocation

            int i = 0;
            if (!_processingRules                  // TT#1117 - MD - Jellis - Units Allocated less than min
                && GetStoreQtyAllocated(aColor, aStore) > 0) // TT#1117 - MD - Jellis - Units Allocated less than min
            {
                // when store was previously allocated, use its allocation as the basis
                int basisValue;
                foreach (AllocationProfile ap in apList)
                {
                    colorBin = ap.GetHdrColorBin(aColor.ColorCodeRID); // TT#1117 - MD - Jellis - Units Allocated less than min
                    basisValue = ap.GetStoreQtyAllocated(aColor.ColorCodeRID, aStore);
                    spreadBasis += basisValue;
                    mgsi[i].Item = i;
                    // begin TT#1117 - MD - Jellis - Units Allocated less than Min
                    //mgsi[i].SortKey = new double[2]; 
                    //mgsi[i].SortKey[0] = basisValue;
                    //mgsi[i].SortKey[1] = AppSessionTransaction.GetRandomDouble();
                    // want the largest min-max spread at end so it can absorb minimum or maximum constraints or earlier headers
                    mgsi[i].SortKey = new double[3];
                    mgsi[i].SortKey[0] =
                          Math.Max(0,                                           // TT#1143 - MD - Jellis - Group Allocation Min Broken
                                 ap.GetStoreMaximum(colorBin, aStore, true)
                                 - ap.GetStoreMinimum(colorBin, aStore, true));
                    mgsi[i].SortKey[1] = basisValue;
                    mgsi[i].SortKey[2] = AppSessionTransaction.GetRandomDouble(); 
                    // end TT#1117 - MD - Jellis - Units Allocated Less than Min
                    i++;          
                }
            }
            else
            if (!_processingRules // TT#4145 - Urban - Jellis - Group Min not holding
                && GetColorUnitsToAllocate(aColor) > GetColorUnitsAllocated(aColor)) // TT#4145 - Urban - Jellis - Group Min not holding
            {
                // when store is not previously allocated, but some stores are, use remaining to allocate as basis (if possible)
                int basisValue; 
                foreach (AllocationProfile ap in apList)
                {
                    colorBin = ap.GetHdrColorBin(aColor.ColorCodeRID);
                    // begin TT#1085 - MD - Jellis - Out Stores Get Allocation
                    ruleAllowsMoreAllocation =
                        (Enum.IsDefined(typeof(eChosenRuleAllowsMoreAllocation), (int)ap.GetStoreChosenRuleType(colorBin, aStore)));
                    if (!ruleAllowsMoreAllocation
                        || ap.GetStoreIsManuallyAllocated(colorBin, aStore)
                        || ap.GetStoreOut(colorBin, aStore)
                        || ap.GetStoreLocked(colorBin, aStore)
                        || ap.GetStoreTempLock(colorBin, aStore)
                        )
                    {
                        basisValue = 0;
                    }
                    else
                    {
                        // end TT#1085 - MD - Jellis - Out Stores Get Allocation
                        basisValue =
                            ap.GetColorUnitsToAllocate(aColor.ColorCodeRID)
                            - ap.GetColorUnitsAllocated(aColor.ColorCodeRID);
                    } // TT#1085 - MD - Jellis - Out Stores Get Allocation
                    mgsi[i].Item = i;
                    mgsi[i].SortKey = new double[3];  // TT#1117 - MD -Jellis - Units allocated less than min
                    if (basisValue > 0)
                    {
                        spreadBasis += basisValue;
                        // begin TT#1117 - MD - Jellis - Units Allocated less than Min
                        //mgsi[i].SortKey[0] = basisValue;
                        //mgsi[i].SortKey[1] = AppSessionTransaction.GetRandomDouble();
                        // want the largest min-max spread at end so it can absorb minimum or maximum constraints or earlier headers
                        mgsi[i].SortKey[0] =
                            Math.Max(0,                                           // TT#1143 - MD - Jellis - Group Allocation Min Broken
                                     ap.GetStoreMaximum(colorBin, aStore, true)
                                     - ap.GetStoreMinimum(colorBin, aStore, true));
                        mgsi[i].SortKey[1] = basisValue;
                        mgsi[i].SortKey[2] = AppSessionTransaction.GetRandomDouble();
                        // end TT#1117 - MD - Jellis - Units Allocated Less than Min

                    }
                    // begin TT#1117 - MD - Jellis - Units ALlocated less than min
                    else
                    {
                        mgsi[i].SortKey[0] = 0;
                        mgsi[i].SortKey[1] = 0;
                        mgsi[i].SortKey[2] = 0;
                    }
                    // end TT#1117 - MD - Jellis - Units ALlocated less than min
                    i++;
                }
            }
            else 
            {
                // default is to use units to allocate as the basis
                int basisValue;
                // begin TT#4213 - MD - Jellis - GA Velocity unexpected results 
                // NOTE:  multi-layer rules in GA MUST have the allocations from all member headers zeroed out before applying rule 
                //        this is due to the way inventory minimum and maximums are calculated
                if (_processingRules)
                {
                    foreach (AllocationProfile ap in apList)
                    {
                        if (ap.GetStoreIsManuallyAllocated(aColor, aStore)
                        || ap.GetStoreOut(aColor, aStore)
                        || ap.GetStoreLocked(aColor, aStore)
                        || ap.GetStoreTempLock(aColor, aStore)
                        )
                        {
                            // Do not zero out allocations that rule will not affect
                        }
                        else
                        {
                            ap.SetStoreQtyAllocated(aColor, aStore, 0, eDistributeChange.ToAll, false, false);
                        }
                    }
                }
                // end TT#4213 - MD - Jellis - GA Velocity unexpected results

                foreach (AllocationProfile ap in apList)
                {
                    colorUnits = ap.GetColorUnitsToAllocate(aColor.ColorCodeRID);
					// Begin TT#1713-MD - stodd - Velocity is only allocating to a single store when running against a Group Allocation
                    if (tryToBalance)
                    {
                        int colorUnitsAllocated = ap.GetColorUnitsToAllocate(aColor.ColorCodeRID); 
                        colorUnits = colorUnits - colorUnitsAllocated;
                    }
                    // End TT#1713-MD - stodd - Velocity is only allocating to a single store when running against a Group Allocation
                    // begin TT#1085 - MD - Jellis - Out Stores Get Allocation
                    colorBin = ap.GetHdrColorBin(aColor.ColorCodeRID);
                    // begin TT#4213 - MD - Jellis - GA Velocity unexpected results
                    if (!_processingRules)
                    {
                        // end TT#4213 - MD - Jellis -  GA Velocity unexpected results
                        ruleAllowsMoreAllocation =
                            (Enum.IsDefined(typeof(eChosenRuleAllowsMoreAllocation), (int)ap.GetStoreChosenRuleType(colorBin, aStore)));
                        // begin TT#4213 - MD - Jellis - GA Velocity unexpected results
                    }
                    else
                    {
                        ruleAllowsMoreAllocation = true;
                    }
                    // end TT#4213 - MD - Jellis - GA Velocity unexpected results
                    if (!ruleAllowsMoreAllocation
                        || ap.GetStoreIsManuallyAllocated(colorBin, aStore)
                        || ap.GetStoreOut(colorBin, aStore)
                        || ap.GetStoreLocked(colorBin, aStore)
                        || ap.GetStoreTempLock(colorBin, aStore)
                        )
                    {
                        basisValue = 0;
                    }
                    else
                    {
                        basisValue = colorUnits;
                    }
                        // end TT#1085 - MD - Jellis - Out Stores Get Allocation

                    spreadBasis += basisValue;  // TT#1085 - MD - Jellis - Out Stores Get Allocation
                    mgsi[i] = new MIDGenericSortItem();
                    mgsi[i].Item = i;
                    //// begin TT#1117 - MD - Jellis - Units Allocated less than Min 
                    //mgsi[i].SortKey = new double[2]; 
                    //mgsi[i].SortKey[0] = basisValue; // TT#1085 - MD - Jellis - Out Stores Get Allocation
                    //mgsi[i].SortKey[1] = AppSessionTransaction.GetRandomDouble();
                    mgsi[i].SortKey = new double[3];
                    // want the largest min-max spread at end so it can absorb minimum or maximum constraints or earlier headers
                    mgsi[i].SortKey[0] =
                          Math.Max(0,                                   // TT#1143 - MD - JEllis - Group ALlocation Min Broken
                                 ap.GetStoreMaximum(colorBin, aStore, true)
                                 - ap.GetStoreMinimum(colorBin, aStore, true));
                    mgsi[i].SortKey[1] = basisValue;
                    mgsi[i].SortKey[2] = AppSessionTransaction.GetRandomDouble();
                    // end TT#1117 - MD - Jellis - Units Allocated Less than Min

                    i++;   
                }
            }
            //Array.Sort(mgsi, new MIDGenericSortDescendingComparer()); // TT#1143 - MD - Jellis - Group Allocation Min Broken
            Array.Sort(mgsi, new MIDGenericSortAscendingComparer());    // TT#1143 - MD - Jellis - Group Allocation Min Broken
            int totalAllocated = aQtyAllocated;
            AllocationProfile sap;
            int sapBasis;
            int maximum; // TT#1117 - MD - Jellis - Allocation Less than Minimum
            foreach (MIDGenericSortItem sortItem in mgsi)
            {
                sapBasis = (int)sortItem.SortKey[1];  // TT#1117 - MD - Jellis - Units Allocated less than min
                sap = apList[sortItem.Item];
                colorBin = sap.GetHdrColorBin(aColor.ColorCodeRID);
                if (spreadBasis > 0)
                {
                    colorUnits =
                        (int) (((double) sapBasis
                                  * (double) totalAllocated
                                  / (double) spreadBasis) + .5d);
                    colorUnits =
                        (int)((double)colorUnits
                               / (double) colorBin.ColorMultiple + .5d);
                    colorUnits =
                        colorUnits
                        * colorBin.ColorMultiple;
                    // begin TT#1117 - MD - JEllis- Allocations Less than Minimum
                    maximum = sap.GetStoreMaximum(colorBin, aStore, true);
                    // Begin TT#1773-MD - JSmith - GA -> Velocity-> Qty allocated column-> attempt to make manual change and get unhandled exception
                    //if (colorUnits > maximum)
                    if (colorUnits > maximum &&
                        _honorMaximums)
                    // End TT#1773-MD - JSmith - GA -> Velocity-> Qty allocated column-> attempt to make manual change and get unhandled exception
                    {
                        colorUnits = maximum;
                    }
                    // end TT#1117 - MD - Jellis - ALlocations Less than Minimum
                    if (colorUnits > totalAllocated)
                    {
                        colorUnits = totalAllocated;
                    }
                    // Begin TT#1713-MD - stodd - Velocity is only allocating to a single store when running against a Group Allocation
                    if (colorUnits < 0)
                    {
                        colorUnits = 0;
                    }
                    if (tryToBalance)
                    {
                        if (colorUnits > sapBasis)
                        {
                            colorUnits = sapBasis;
                        }
                    }
                    // Begin TT#1713-MD - stodd - Velocity is only allocating to a single store when running against a Group Allocation

                    // begin TT#1117 - MD- Jellis - Allocations Less than Minimum
                    if (colorUnits < sap.GetStoreMinimum(colorBin, aStore, true))
                    {
                        colorUnits = 0;
                    }
                    //totalAllocated -= colorUnits;
                    // end TT#1117 - MD - Jellis- Allocations Less than Minimum
                }
                else
                {
                    colorUnits = 0;
                }
                spreadBasis -= sapBasis;
                allocationUpdateStatus = sap.SetStoreQtyAllocated(colorBin, aStore, colorUnits, aDistributeChange, aIsManual, aPermitMultiHdrUpdtWhenIntransit, aApplyTempLocks);
                // begin TT#1117 - MD - Jellis - Units allocated less than minimum
                totalAllocated -= sap.GetStoreQtyAllocated(colorBin, aStore);
                // end TT#1117 - MD - Jellis - Units allocated less than minimum

                if (allocationUpdateStatus != eAllocationUpdateStatus.Successful)
                {
                    spreadUpdateStatus = eAllocationUpdateStatus.Failed_SpreadFailed;
                }
				
				aQtyAllocated = totalAllocated;		// TT#1713-MD - stodd - Velocity is only allocating to a single store when running against a Group Allocation

           }
            // Begin TT#1773-MD - JSmith - GA -> Velocity-> Qty allocated column-> attempt to make manual change and get unhandled exception
            if (spreadUpdateStatus == eAllocationUpdateStatus.Successful &&
                aQtyAllocated > 0)
            {
                spreadUpdateStatus = eAllocationUpdateStatus.Failed_NotAllUnitsSpread;
            }
            // End TT#1773-MD - JSmith - GA -> Velocity-> Qty allocated column-> attempt to make manual change and get unhandled exception
            return spreadUpdateStatus;
        }

        /// <summary>
        /// Sets Store Quantity Allocated for specified store on specified color node.
        /// </summary>
        /// <param name="aSize">HdrSizeBin for the size where the store resides.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <param name="aQtyAllocated">Quantity Allocated</param>
        /// <param name="aDistributeChange">Indicates how the quantity allocated is to be applied to parent nodes (there are no children nodes).</param>
        /// <param name="aIsManual">True indicates the user manually keyed the value; false indicates otherwise.</param>
        /// <param name="aPermitMultiHdrUpdtWhenIntransit">True: allows multi header allocation to change when intransit updated; False: intransit inhibits changes</param>
        /// <param name="aApplyTempLocks">True: Apply Temp Locks; False: Do not apply temp locks (only apply when this is the initial change to the store's value)</param>
        /// <returns>Status of the update.</returns>
        override internal eAllocationUpdateStatus SetStoreQtyAllocated(HdrSizeBin aSize, Index_RID aStore,
            int aQtyAllocated, eDistributeChange aDistributeChange, bool aIsManual, bool aPermitMultiHdrUpdtWhenIntransit, bool aApplyTempLocks) // TT#59 Temp Locks
        {
            eAllocationUpdateStatus spreadUpdateStatus = eAllocationUpdateStatus.Successful;
            eAllocationUpdateStatus allocationUpdateStatus;
            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);
            MIDGenericSortItem[] mgsi = new MIDGenericSortItem[apList.Count];
            int spreadBasis = 0;
            int sizeUnits;
            HdrColorBin colorBin;
            HdrSizeBin sizeBin;
            int i = 0;
            if (!_processingRules      // TT#1117 - MD - Jellis - Units Allocated less than min
                && GetStoreQtyAllocated(aSize, aStore) > 0) // TT#1117 - MD - Jellis - Units Allocated less than min
            {
                // when store was previously allocated, use its allocation as the basis
                int basisValue;
                foreach (AllocationProfile ap in apList)
                {
                    // begin TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                    sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aSize.Color.ColorCodeRID).ColorSizes[aSize.SizeCodeRID];
                    if (sizeBin != null)
                    {
                        basisValue = ap.GetStoreQtyAllocated(sizeBin, aStore);
                        spreadBasis += basisValue;
                    }
                    else
                    {
                        basisValue = 0;
                    }
                    //basisValue = ap.GetStoreQtyAllocated(sizeBin, aStore);
                    //spreadBasis += basisValue;
                    // end TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                    mgsi[i] = new MIDGenericSortItem();
                    mgsi[i].Item = i;
                    // begin TT#1117 - MD - Jellis - Units Allocated less than Min
                    //mgsi[i].SortKey = new double[3]; // TT#1117 - MD - Jellis - Units Allocated less than min
                    //mgsi[i].SortKey[0] = basisValue;
                    //mgsi[i].SortKey[1] = AppSessionTransaction.GetRandomDouble();
                    // want the largest min-max spread at end so it can absorb minimum or maximum constraints on earlier headers
                    mgsi[i].SortKey = new double[3]; // TT#1193 - MD - Jellis - Null Reference
                    mgsi[i].SortKey[0] =
                          Math.Max(0,                                           // TT#1143 - MD - Jellis - Group Allocation Min Broken
                                 ap.GetStoreMaximum(sizeBin, aStore, true)
                                 - ap.GetStoreMinimum(sizeBin, aStore, true));
                    mgsi[i].SortKey[1] = basisValue;
                    mgsi[i].SortKey[2] = AppSessionTransaction.GetRandomDouble(); 
                    // end TT#1117 - MD - Jellis - Units Allocated Less than Min

                    i++;
                }
            }
            else if (GetStoreQtyAllocated(aSize.Color, aStore) > 0)
                // begin TT#1088 - MD - JEllis - Size Allocation Exceeds Color Allocation
            {
                int basisValue;
                AllocationColorSizeComponent allSizeComponent =
                    new AllocationColorSizeComponent(
                        new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, aSize.Color.ColorCodeRID),
                        new GeneralComponent(eComponentType.AllSizes));
                foreach (AllocationProfile ap in apList)
                {
                    colorBin = ap.GetHdrColorBin(aSize.Color.ColorCodeRID);
                    sizeBin = (HdrSizeBin)colorBin.ColorSizes[aSize.SizeCodeRID];
                    if (sizeBin != null)
                    {
                        basisValue =
                            ap.GetStoreQtyAllocated(colorBin, aStore)
                            - ap.GetStoreQtyAllocated(allSizeComponent, aStore);
						// Begin TT#1788-MD - stodd - Fill Size not allocating bulk when headers belong to a group allocation
						// This check is because when Fill Size is run, after the first size is allocated and spread up to color,
						// color and all size will match causing the basis to be zero and the remaining sizes are not allocated.
                        if (basisValue == 0)
                        {

                            basisValue =
                                colorBin.ColorUnitsToAllocate -
                                sizeBin.SizeUnitsToAllocate;
                        }
						// End TT#1788-MD - stodd - Fill Size not allocating bulk when headers belong to a group allocation
                    }
                    else
                    {
                        basisValue = 0;
                    }

                    mgsi[i] = new MIDGenericSortItem();
                    mgsi[i].Item = i;
                    mgsi[i].SortKey = new double[3]; // TT#1117 - MD- Jellis - Units Allocated less than size bin
                    if (basisValue > 0)
                    {
                        spreadBasis += basisValue;
                        // begin TT#1117 - MD - Jellis - Units Allocated less than Min
                        //mgsi[i].SortKey[0] = basisValue;
                        //mgsi[i].SortKey[1] = AppSessionTransaction.GetRandomDouble();
                        // want the largest min-max spread at end so itcan absorb minimum or maximum constraints on earlier headers
                        mgsi[i].SortKey[0] =
                              Math.Max(0,                                           // TT#1143 - MD - Jellis - Group Allocation Min Broken
                                     ap.GetStoreMaximum(colorBin, aStore, true)
                                     - ap.GetStoreMinimum(colorBin, aStore, true));
                        mgsi[i].SortKey[1] = basisValue;
                        mgsi[i].SortKey[2] = AppSessionTransaction.GetRandomDouble();
                        // end TT#1117 - MD - Jellis - Units Allocated Less than Min
                    }
                    else
                    {
                        mgsi[i].SortKey[0] = 0;  // TT#1088 - MD - Jellis - Size AllocationExceeds Color Allocation
                        mgsi[i].SortKey[1] = 0;  // TT#1117 - MD - Jellis - Units Allocated less than min
                        mgsi[i].SortKey[2] = 0; // TT#1117 - MD - Jellis - Units Allocated less than min
                    }
                    i++;
                }
            }
            else if (GetSizeUnitsToAllocate(aSize) > GetSizeUnitsAllocated(aSize))
                // end TT#1088 - MD - Jellis - Size Allocation Exceeds Color Allocation
            //if (GetSizeUnitsToAllocate(aSize) > GetSizeUnitsAllocated(aSize))
            {
                // when store is not previously allocated, but some stores are, use remaining to allocate as basis (if possible)
                int basisValue; 
                foreach (AllocationProfile ap in apList)
                {
                    colorBin = ap.GetHdrColorBin(aSize.Color.ColorCodeRID);
                    // begin TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                    sizeBin = (HdrSizeBin)colorBin.ColorSizes[aSize.SizeCodeRID];
                    if (sizeBin != null)
                    {
                        basisValue =
                            ap.GetSizeUnitsToAllocate(colorBin, aSize.SizeCodeRID)
                            - ap.GetSizeUnitsAllocated(colorBin, aSize.SizeCodeRID);
                    }
                    else
                    {
                        basisValue = 0;
                    }
                    //basisValue =
                    //    ap.GetSizeUnitsToAllocate(colorBin, aSize.SizeCodeRID)
                    //    - ap.GetSizeUnitsAllocated(colorBin, aSize.SizeCodeRID);
                    // end TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                    mgsi[i] = new MIDGenericSortItem();
                    mgsi[i].Item = i;
                    mgsi[i].SortKey = new double[3]; // TT#1117 - MD- JEllis - Units Allocated less than min
                    if (basisValue > 0)
                    {
                        spreadBasis += basisValue;
                        // begin TT#1117 - MD - Jellis - Units Allocated less than Min
                        //mgsi[i].SortKey[0] = basisValue;
                        //mgsi[i].SortKey[1] = AppSessionTransaction.GetRandomDouble();
                        // want these at end so they can absorb minimum or maximum constraints or earlier headers
                        mgsi[i].SortKey[0] =
                              Math.Max(0,                                           // TT#1143 - MD - Jellis - Group Allocation Min Broken
                                     ap.GetStoreMaximum(sizeBin, aStore, true)
                                     - ap.GetStoreMinimum(sizeBin, aStore, true));
                        mgsi[i].SortKey[1] = basisValue;
                        mgsi[i].SortKey[2] = AppSessionTransaction.GetRandomDouble(); 
                        // end TT#1117 - MD - Jellis - Units Allocated Less than Min
                    }
                    else
                    {
                        // begin TT#1117 - MD - Jellis - Units Allocated less than Min
                        //mgsi[i].SortKey[0] = basisValue;
                        //mgsi[i].SortKey[1] = AppSessionTransaction.GetRandomDouble(); 
                        // want the largest min-max spread at end so it can absorb minimum or maximum constraints on earlier headers
                        mgsi[i].SortKey[0] = 0;
                        mgsi[i].SortKey[1] = 0;
                        mgsi[i].SortKey[2] = 0; 
                        // end TT#1117 - MD - Jellis - Units Allocated Less than Min
                    }
                    i++;
                }
            }
            else 
            {
                // default is to use units to allocate as the basis
                foreach (AllocationProfile ap in apList)
                {
                    // begin TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                    colorBin = ap.GetHdrColorBin(aSize.Color.ColorCodeRID);
                    sizeBin = (HdrSizeBin)colorBin.ColorSizes[aSize.SizeCodeRID];
                    if (sizeBin != null)
                    {
                        sizeUnits = ap.GetSizeUnitsToAllocate(sizeBin);
                        spreadBasis += sizeUnits;
                    }
                    else
                    {
                        sizeUnits = 0;
                    }
				    //sizeUnits = ap.GetSizeUnitsToAllocate(aSize.Color.ColorCodeRID, aSize.SizeCodeRID);
					// end TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                    spreadBasis += sizeUnits;
                    mgsi[i] = new MIDGenericSortItem();
                    mgsi[i].Item = i;
                    // begin TT#1117 - MD - Jellis - Units Allocated less than Min
                    //mgsi[i].SortKey = new double[2]; 
                    //mgsi[i].SortKey[0] = sizeUnits;
                    //mgsi[i].SortKey[1] = AppSessionTransaction.GetRandomDouble();
                    mgsi[i].SortKey = new double[3];
                    // want the largest min-max spread at end so it can absorb minimum or maximum constraints on earlier headers
                    mgsi[i].SortKey[0] =
                          Math.Max(0,                                // TT#1143 - MD - Jellis - Group ALlocation Min Broken
                                 ap.GetStoreMaximum(sizeBin, aStore, true)
                                 - ap.GetStoreMinimum(sizeBin, aStore, true));
                    mgsi[i].SortKey[1] = sizeUnits;
                    mgsi[i].SortKey[2] = AppSessionTransaction.GetRandomDouble(); 
                    // end TT#1117 - MD - Jellis - Units Allocated Less than Min
                    i++;
                }
            }
            //Array.Sort(mgsi, new MIDGenericSortDescendingComparer()); // TT#1143 - MD - Jellis - Group Allocation Min Broken
            Array.Sort(mgsi, new MIDGenericSortAscendingComparer());    // TT#1143 - MD - Jellis - Group Allocation Min Broken
            int totalAllocated = aQtyAllocated;

            AllocationProfile sap;
            int sapBasis;
            int maximum; // TT#1117 - MD - Jellis - Allocations less than minimum
            foreach (MIDGenericSortItem sortItem in mgsi)
            {
                sapBasis = (int)sortItem.SortKey[1];  // TT#1117 - MD - Jellis- Units Allocated less than min
                sap = apList[sortItem.Item];
                colorBin = sap.GetHdrColorBin(aSize.Color.ColorCodeRID);
                //sizeBin = colorBin.GetSizeBin(aSize.SizeCodeRID); //   TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                sizeBin = (HdrSizeBin)colorBin.ColorSizes[aSize.SizeCodeRID];   // TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                if (sizeBin != null) // TTT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                {                    // TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                    if (spreadBasis > 0)
                    {
                        sizeUnits =
                            //(int)(((double)sizeBin.SizeUnitsToAllocate  // TT#1088 - MD - Jellis - Size Allocation Exceeds Color Allocation
                            (int)(((double)sapBasis                       // TT#1088 - MD - Jellis - Size Allocation Exceeds Color Allocation
                                      * (double)totalAllocated
                                      / (double)spreadBasis) + .5d);
                        sizeUnits =
                            (int)((double)sizeUnits
                                   / (double)sizeBin.SizeMultiple + .5d);
                        sizeUnits =
                            sizeUnits
                            * sizeBin.SizeMultiple;
                        // begin TT#1117 - MD - JEllis- Allocations Less than Minimum
                        maximum = sap.GetStoreMaximum(sizeBin, aStore, true);
                        // Begin TT#1773-MD - JSmith - GA -> Velocity-> Qty allocated column-> attempt to make manual change and get unhandled exception
                        //if (sizeUnits > maximum)
                        if (sizeUnits > maximum &&
                            _honorMaximums)
                        // End TT#1773-MD - JSmith - GA -> Velocity-> Qty allocated column-> attempt to make manual change and get unhandled exception
                        {
                            sizeUnits = maximum;
                        }
                        // end TT#1117 - MD - Jellis - ALlocations Less than Minimum
                        if (sizeUnits > totalAllocated)
                        {
                            sizeUnits = totalAllocated;
                        }
                        // begin TT#1117 - MD- Jellis - Allocations Less than Minimum
                        if (sizeUnits < sap.GetStoreMinimum(sizeBin, aStore, true))
                        {
                            sizeUnits = 0;
                        }
                        //totalAllocated -= sizeUnits;
                        // end TT#1117 - MD - Jellis- Allocations Less than Minimum
                    }
                    else
                    {
                        sizeUnits = 0;
                    }
                    //spreadBasis -= sizeBin.SizeUnitsToAllocate;   // TT#1088 - MD - Jellis - Size Allocation Exceeds Color Allocation
                    spreadBasis -= sapBasis;     // TT#1088 - MD - Jellis - Size Allocation Exceeds Color Allocation

                    //Debug.WriteLine("SetStoreQtyAllocated(SIZE) ST: " + aStore.RID + " SZ: " + sizeBin.SizeCodeRID + " INIT UNITS: " + aQtyAllocated + " UNITS ALLOC: " + sizeUnits);


                    allocationUpdateStatus = sap.SetStoreQtyAllocated(sizeBin, aStore, sizeUnits, aDistributeChange, aIsManual, aPermitMultiHdrUpdtWhenIntransit, aApplyTempLocks);
                    // begin TT#1117 - MD - Jellis - Units allocated less than minimum
                    totalAllocated -= sap.GetStoreQtyAllocated(sizeBin, aStore);
                    // end TT#1117 - MD - Jellis - Units allocated less than minimum

                    if (allocationUpdateStatus != eAllocationUpdateStatus.Successful)
                    {
                        spreadUpdateStatus = eAllocationUpdateStatus.Failed_SpreadFailed;
                    }



                } // TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
            }
            // Begin TT#1773-MD - JSmith - GA -> Velocity-> Qty allocated column-> attempt to make manual change and get unhandled exception
            if (spreadUpdateStatus == eAllocationUpdateStatus.Successful &&
                totalAllocated > 0)
            {
                spreadUpdateStatus = eAllocationUpdateStatus.Failed_NotAllUnitsSpread;
            }
            // End TT#1773-MD - JSmith - GA -> Velocity-> Qty allocated column-> attempt to make manual change and get unhandled exception
            return spreadUpdateStatus;
        }
        #endregion SetStoreQtyAllocated
        #endregion StoreQtyAllocated

        #region StoreQtyAllocatedByAuto
        #region GetStoreQtyAllocatedByAuto
        /// <summary>
        /// Gets Store Quantity Allocated By Auto for specified store on specified node.
        /// </summary>
        /// <param name="aAllocationSummaryNode">Allocation summary node.</param>
        /// <param name="aStore">Index_RID for store</param>
        /// <returns>Quantity Allocated By Auto to the store for the specified node.</returns>
        override internal int GetStoreQtyAllocatedByAuto(eAllocationSummaryNode aAllocationSummaryNode, Index_RID aStore)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            int allocated = 0;
            foreach (AllocationProfile ap in apList)
            {
                allocated += ap.GetStoreQtyAllocatedByAuto(aAllocationSummaryNode, aStore);
            }
            return allocated; 
        }

        /// <summary>
        /// Gets Store Quantity Allocated By Auto for specified store on specified color node.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>Quantity Allocated By Auto to the store for the specified node.</returns>
        override internal int GetStoreQtyAllocatedByAuto(HdrColorBin aColor, Index_RID aStore) // TT#488 - MD - Jellis - Group Allocation
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            int allocated = 0;
            foreach (AllocationProfile ap in apList)
            {
                allocated += ap.GetStoreQtyAllocatedByAuto(aColor.ColorCodeRID, aStore);
            }
            return allocated;
        }

        /// <summary>
        /// Gets Store Quantity Allocated By Auto for specified store on specified color node.
        /// </summary>
        /// <param name="aSize">HdrSizeBin for the size where the store resides.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>Quantity Allocated By Auto to the store for the specified node.</returns>
        override internal int GetStoreQtyAllocatedByAuto(HdrSizeBin aSize, Index_RID aStore) // TT#488 - MD - Jellis - Group Allocation
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);
            int allocated = 0;
            HdrSizeBin sizeBin;
            foreach (AllocationProfile ap in apList)
            {
                sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aSize.Color.ColorCodeRID).ColorSizes[aSize.SizeCodeRID];
                if (sizeBin != null)
                {
                    allocated += ap.GetStoreQtyAllocatedByAuto(sizeBin, aStore);
                }
            }
            return allocated;
        }
        #endregion GetStoreQtyAllocatedByAuto

        #endregion StoreQtyAllocatedByAuto

        #region StoreItemQtyAllocated
        #region GetStoreItemQtyAllocated
        /// <summary>
        /// Gets Store Item Quantity Allocated for specified store on specified node.
        /// </summary>
        /// <param name="aAllocationSummaryNode">Allocation summary node.</param>
        /// <param name="aStore">Index_RID for store</param>
        /// <returns>Item Quantity Allocated to the store for the specified node.</returns>
        override internal int GetStoreItemQtyAllocated(eAllocationSummaryNode aAllocationSummaryNode, Index_RID aStore)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            int allocated = 0;
            foreach (AllocationProfile ap in apList)
            {
                allocated += ap.GetStoreItemQtyAllocated(aAllocationSummaryNode, aStore);
            }
            return allocated;
        }

        /// <summary>
        /// Gets Store Item Quantity Allocated for specified store on specified color node.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>Item Quantity Allocated to the store for the specified node.</returns>
        override internal int GetStoreItemQtyAllocated(HdrColorBin aColor, Index_RID aStore)
        {
            List<AllocationProfile> apList = GetHeadersWithColor (aColor.ColorCodeRID);
            int allocated = 0;
            foreach (AllocationProfile ap in apList)
            {
                allocated += ap.GetStoreItemQtyAllocated(aColor.ColorCodeRID, aStore);
            }
            return allocated;
        }

        /// <summary>
        /// Gets Store Item Quantity Allocated for specified store on specified color node.
        /// </summary>
        /// <param name="aSize">HdrSizeBin for the size where the store resides.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>Item Quantity Allocated to the store for the specified node.</returns>
        override internal int GetStoreItemQtyAllocated(HdrSizeBin aSize, Index_RID aStore)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);
            int allocated = 0;
            HdrColorBin colorBin;
            HdrSizeBin sizeBin;
            foreach (AllocationProfile ap in apList)
            {
                colorBin = ap.GetHdrColorBin(aSize.Color.ColorCodeRID);
                sizeBin = (HdrSizeBin)colorBin.ColorSizes[aSize.SizeCodeRID];
                if (sizeBin != null)
                {
                    allocated += ap.GetStoreItemQtyAllocated(sizeBin, aStore);
                }
                return allocated;
            }
            return allocated;
        }
        #endregion GetStoreItemQtyAllocated

        #endregion StoreItemQtyAllocated


        #region StoreTotalItemUnitsManuallyAllocated
        #region GetStoreTotalItemUnitsManuallyAllocated
        override public int GetStoreTotalItemUnitsManuallyAllocated(eAllocationSummaryNode aAllocationSummaryNode, Index_RID aStore)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            int manuallyAllocated = 0;
            foreach (AllocationProfile ap in apList)
            {
                manuallyAllocated += ap.GetStoreTotalItemUnitsManuallyAllocated (aAllocationSummaryNode, aStore);
            }
            return manuallyAllocated;
        }
        override internal int GetStoreTotalItemUnitsManuallyAllocated(HdrColorBin aColor, Index_RID aStore)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            int manuallyAllocated = 0;
            HdrColorBin colorBin;
            foreach (AllocationProfile ap in apList)
            {
                colorBin = ap.GetHdrColorBin(aColor.ColorCodeRID);
                manuallyAllocated += ap.GetStoreTotalItemUnitsManuallyAllocated (colorBin, aStore);
            }
            return manuallyAllocated;
        }
        #endregion GetStoreTotalItemUnitsManuallyAllocated
        #endregion StoreTotalItemUnitsManuallyAllocated

        #region StoreImoQtyAllocated
        #region GetStoreImoQtyAllocated
        /// <summary>
        /// Gets Store IMO Quantity Allocated for specified store on specified node.
        /// </summary>
        /// <param name="aAllocationSummaryNode">Allocation summary node.</param>
        /// <param name="aStore">Index_RID for store</param>
        /// <returns>IMO Quantity Allocated to the store for the specified node.</returns>
        override internal int GetStoreImoQtyAllocated(eAllocationSummaryNode aAllocationSummaryNode, Index_RID aStore)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            int allocated = 0;
            foreach (AllocationProfile ap in apList)
            {
                allocated += ap.GetStoreImoQtyAllocated(aAllocationSummaryNode, aStore);
            }
            return allocated;
        }

        /// <summary>
        /// Gets Store IMO Quantity Allocated for specified store on specified color node.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>IMO Quantity Allocated to the store for the specified node.</returns>
        override internal int GetStoreImoQtyAllocated(HdrColorBin aColor, Index_RID aStore)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            int allocated = 0;
            foreach (AllocationProfile ap in apList )
            {
                allocated += ap.GetStoreImoQtyAllocated(aColor.ColorCodeRID, aStore);
            }
            return allocated;
        }

        /// <summary>
        /// Gets Store IMO Quantity Allocated for specified store on specified color node.
        /// </summary>
        /// <param name="aSize">HdrSizeBin for the size where the store resides.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>IMO Quantity Allocated to the store for the specified node.</returns>
        override internal int GetStoreImoQtyAllocated(HdrSizeBin aSize, Index_RID aStore)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);
            int allocated = 0;
            HdrColorBin colorBin;
            HdrSizeBin sizeBin;
            foreach (AllocationProfile ap in apList)
            {
                colorBin = ap.GetHdrColorBin(aSize.Color.ColorCodeRID);
                sizeBin = (HdrSizeBin)colorBin.ColorSizes[aSize.SizeCodeRID];
                if (sizeBin != null)
                {
                    allocated += ap.GetStoreImoQtyAllocated(sizeBin, aStore);
                }
            }
            return allocated;
        }
        #endregion GetStoreImoQtyAllocated
        #endregion StoreImoQtyAllocated

        #region StoreShippingStatus
        #region GetStoreShippingStatus
        override public eHeaderShipStatus GetStoreShippingStatus(GeneralComponent aGeneralComponent, int aStoreRID)
        {
            //AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error  // TT#1436 - MD - Jellis - ?????
            int headersShipped = 0;
            int headersNotShipped = 0;
            int headersPartialShipped = 0;
            int headersHeld = 0;
            // begin TT#1202 - MD - Jellis - Group Allocation - Argument Except - Cannot view Group in Style REview
            if (aGeneralComponent.ComponentType == eComponentType.SpecificPack)
            {
                PackHdr ph = GetPackHdr(((AllocationPackComponent)aGeneralComponent).PackName);
                return GetStoreShippingStatus(ph, StoreIndex(aStoreRID));
            }
            // end TT#1202 - MD - Jellis - Group Allocation - Argument Except - Cannot view Group in Style REview
            // begin TT#1436 - MD - Jellis - ????
            AllocationProfile[] apList;
            if (aGeneralComponent.ComponentType == eComponentType.SpecificColor)
            {
                AllocationColorOrSizeComponent colorComponent = (AllocationColorOrSizeComponent)aGeneralComponent;
                List<AllocationProfile> aList = GetHeadersWithColor(colorComponent.ColorRID);
                apList = new AllocationProfile[aList.Count];
                aList.CopyTo(apList);
            }
            else
            {
                apList = AssortmentMembers;
            }
            // end TT#1436 - MD - Jellis - ????
            foreach (AllocationProfile ap in apList)
            {
                switch (ap.GetStoreShippingStatus(aGeneralComponent, aStoreRID))
                {
                    case (eHeaderShipStatus.NotShipped):
                        {
                            headersNotShipped++;
                            break;
                        }
                    case (eHeaderShipStatus.Partial):
                        {
                            headersPartialShipped++;
                            break;
                        }
                    case (eHeaderShipStatus.Shipped):
                        {
                            headersShipped++;
                            break;
                        }
                    default:
                        {
                            headersHeld++;
                            break;
                        }
                }
            }
            if (headersShipped == apList.Length) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
            {
                return eHeaderShipStatus.Shipped;
            }
            if (headersNotShipped == apList.Length) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
            {
                return eHeaderShipStatus.NotShipped;
            }
            if (headersHeld == apList.Length) // TT#891 - MD - Jellis - Group Allocation Need Gets Error
            {
                return eHeaderShipStatus.OnHold;
            }
            return eHeaderShipStatus.Partial;
        }
        #endregion GetStoreShippingStatus

        #region StoreShippingStarted
        #region GetStoreShippingStarted
        /// <summary>
        /// Gets Store Shipping Started audit flag for the specified store and component.
        /// </summary>
        /// <param name="aAllocationSummaryNode">Allocation Summary Component</param>
        /// <param name="aStore">Index_RID identifier for the store.</param>
        /// <returns>True if shipping has started for this store.</returns>
        override internal bool GetStoreShippingStarted(eAllocationSummaryNode aAllocationSummaryNode, Index_RID aStore)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Gets Error
            foreach (AllocationProfile ap in apList)
            {
                if (ap.GetStoreShippingStarted(aAllocationSummaryNode, aStore))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets Store Shipping Started audit flag for the specified store and component.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color</param>
        /// <param name="aStore">Index_RID identifier for the store.</param>
        /// <returns>True if shipping has started for this store.</returns>
        override internal bool GetStoreShippingStarted(HdrColorBin aColor, Index_RID aStore)
        {
            //AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Gets Error // TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);  // TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
            foreach (AllocationProfile ap in apList)
            {
                if (ap.GetStoreShippingStarted(aColor.ColorCodeRID, aStore))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets Store Shipping Started audit flag for the specified store and component.
        /// </summary>
        /// <param name="aSize">HdrSizeBin of the size.</param>
        /// <param name="aStore">Index_RID identifier for the store.</param>
        /// <returns>True if shipping has started for this store.</returns>
        override internal bool GetStoreShippingStarted(HdrSizeBin aSize, Index_RID aStore)
        {
            //AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Gets Error // TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);  // TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
            foreach (AllocationProfile ap in apList)
            {
                // begin TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                HdrSizeBin sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aSize.Color.ColorCodeRID).ColorSizes[aSize.SizeCodeRID];
                if (sizeBin != null)
                {
                    if (ap.GetStoreShippingStarted(aSize.Color.ColorCodeRID, aSize.SizeCodeRID, aStore))
                    //if (ap.GetStoreShippingStarted(aSize.Color.ColorCodeRID, aSize.SizeCodeRID, aStore))
                    // end TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                    {
                        return true;
                    }
                }  // TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
            }
            return false;
        }
        #endregion GetStoreShippingStarted

        #endregion StoreShippingStarted

        #region StoreShippingComplete
        #region GetStoreShippingComplete
        /// <summary>
        /// Gets Store Shipping Complete audit flag for the specified store and component.
        /// </summary>
        /// <param name="aAllocationSummaryNode">Allocation Summary Component</param>
        /// <param name="aStore">Index_RID identifier for the store.</param>
        /// <returns>True if shipping is Complete for this store.</returns>
        override internal bool GetStoreShippingComplete(eAllocationSummaryNode aAllocationSummaryNode,
            Index_RID aStore)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Gets Error
            foreach (AllocationProfile ap in apList)
            {
                if (!ap.GetStoreShippingComplete(aAllocationSummaryNode, aStore))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets Store Shipping Complete audit flag for the specified store and component.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color</param>
        /// <param name="aStore">Index_RID identifier for the store.</param>
        /// <returns>True if shipping is Complete for this store.</returns>
        override internal bool GetStoreShippingComplete(HdrColorBin aColor, Index_RID aStore)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                if (!ap.GetStoreShippingComplete(aColor.ColorCodeRID, aStore))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets Store Shipping Complete audit flag for the specified store and component.
        /// </summary>
        /// <param name="aSize">HdrSizeBin of the size.</param>
        /// <param name="aStore">Index_RID identifier for the store.</param>
        /// <returns>True if shipping is Complete for this store.</returns>
        override internal bool GetStoreShippingComplete(HdrSizeBin aSize, Index_RID aStore)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);
            foreach (AllocationProfile ap in apList)
            {
                // begin TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                HdrSizeBin sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aSize.Color.ColorCodeRID).ColorSizes[aSize.SizeCodeRID];
                if (sizeBin != null)
                {
                    if (!ap.GetStoreShippingComplete(sizeBin, aStore))
                    //if (!ap.GetStoreShippingComplete(aSize.Color.ColorCodeRID, aSize.SizeCodeRID, aStore))
                    // end TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                    {
                        return false;
                    }
                }   // TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
            }
            return true;

        }
        #endregion GetStoreShippingComplete

        #endregion StoreShippingComplete

        #region StoreQtyShipped
        #region GetStoreQtyShipped
 
        /// <summary>
        /// Gets Store Quantity Shipped for specified store on specified node.
        /// </summary>
        /// <param name="aAllocationSummaryNode">Allocation summary node.</param>
        /// <param name="aStore">Index_RID for store</param>
        /// <returns>Quantity Shipped to the store for the specified node.</returns>
        override internal int GetStoreQtyShipped(eAllocationSummaryNode aAllocationSummaryNode, Index_RID aStore)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Gets Error
            int shipped = 0;
            foreach (AllocationProfile ap in apList)
            {
                shipped += ap.GetStoreQtyShipped(aAllocationSummaryNode, aStore);
            }
            return shipped;
        }

        /// <summary>
        /// Gets Store Quantity Shipped for specified store on specified color node.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>Quantity Shipped to the store for the specified node.</returns>
        override internal int GetStoreQtyShipped(HdrColorBin aColor, Index_RID aStore)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            int shipped = 0;
            foreach (AllocationProfile ap in apList)
            {
                shipped += ap.GetStoreQtyShipped(aColor.ColorCodeRID, aStore);
            }
            return shipped;
        }

        /// <summary>
        /// Gets Store Quantity Shipped for specified store on specified color node.
        /// </summary>
        /// <param name="aSize">HdrSizeBin for the size where the store resides.</param>
        /// <param name="aStore">Index_RID for the store</param>
        /// <returns>Quantity Shipped to the store for the specified node.</returns>
        override internal int GetStoreQtyShipped(HdrSizeBin aSize, Index_RID aStore)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);
            int shipped = 0;
            foreach (AllocationProfile ap in apList)
            {
                // begin TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                HdrSizeBin sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aSize.Color.ColorCodeRID).ColorSizes[aSize.SizeCodeRID];
                if (sizeBin != null)
                {
                    shipped += ap.GetStoreQtyShipped(sizeBin, aStore);
                    //shipped += ap.GetStoreQtyShipped(aSize.Color.ColorCodeRID, aSize.SizeCodeRID, aStore);
                }
                // end TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
            }
            return shipped;
        }
        #endregion GetStoreQtyShipped

        #endregion StoreQtyShipped

        #endregion StoreShippingStatus

        #endregion Stores

        // begin TT#3749 - - MD - Jellis - Velocity Balance not matching Style Review Balance
        #region GetAllocatedBalance
        /// <summary>
        /// Gets Allocated Balance for specified color node.
        /// </summary>
        /// <param name="aColor">HdrColorBin for the color.</param>
        /// <returns>Allocated Balance for the specified node.</returns>
        override internal int GetAllocatedBalance(HdrColorBin aColor) 
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aColor.ColorCodeRID);
            int balance = 0;
            foreach (AllocationProfile ap in apList)
            {
                balance += ap.GetAllocatedBalance(aColor.ColorCodeRID);
            }
            return balance;
        }
        /// <summary>
        /// Gets Allocated Balance for specified color-size node.
        /// </summary>
        /// <param name="aSize">HdrSizeBin for the size where the store resides.</param>
        /// <returns>Allocated Balance for the specified node.</returns>
        override internal int GetAllocatedBalance(HdrSizeBin aSize)
        {
            List<AllocationProfile> apList = GetHeadersWithColor(aSize.Color.ColorCodeRID);
            int balance = 0;
            foreach (AllocationProfile ap in apList)
            {
                HdrSizeBin sizeBin = (HdrSizeBin)ap.GetHdrColorBin(aSize.Color.ColorCodeRID).ColorSizes[aSize.SizeCodeRID];
                if (sizeBin != null)
                {
                    balance += ap.GetAllocatedBalance(sizeBin);
                }
            }
            return balance;
        }
        #endregion GetAllocatedBalance
        // end TT#3749 - - MD - Jellis - Velocity Balance not matching Style Review Balance

        #endregion Allocation Profile Method Overrides


        // begin TT#4208 - MD - Jellis - GA Velocity allocates under Minimum
        public int GetStoreTotalMemberMaximum(Index_RID aStoreIndexRID)
        {
            if (_buildInventoryBasisAllocation)
            {
                CreateInventoryBasisAllocationXref();
            }
            return (int)_storeTotalMemberMaximum.GetStoreValue(aStoreIndexRID.RID);
        }
        // end TT#4208 - MD - Jellis - GA Velocity allocates under minimum

        // begin TT#1074 - MD - Jellis - Inventory Min Max
        /// <summary>
        /// Gets a list of Inventory Basis RIDs that are updated by the style/color
        /// </summary>
        /// <param name="aStyleHnRID">Style Hierarchy NODE RID</param>
        /// <param name="aColorCodeRID">Color Code RID</param>
        /// <param name="aSizeInventory">Is the request for a Size Level Inventory Basis?</param>
        /// <returns>Array of Inventory Basis RIDs that are updated by the style/color</returns>
        public int[] GetInventoryUpdateList(int aStyleHnRID, int aColorCodeRID, bool aSizeInventory)
        {
            // begin TT#1172 - MD - Jellis - Need not giving up to max __ and Null Reference
            if (_buildInventoryBasisAllocation)
            {
                CreateInventoryBasisAllocationXref();
            }
            // end TT#1172 - MD - Jellis - Need not giving up to max __ and Null Reference
            List<int> updateList;
            if (aSizeInventory)
            {
                if (!_sizeUpdatesInventoryXref.TryGetValue(MIDMath.PackIntegers(Math.Max(0,aColorCodeRID), (aStyleHnRID)), out updateList))
                { 
                    updateList = new List<int>();
                }
            }
            else
            {
                if (!(_styleColorUpdatesInventoryXref.TryGetValue(MIDMath.PackIntegers(Math.Max(0, aColorCodeRID), (aStyleHnRID)), out updateList)))
                {
                    updateList = new List<int>();
                }
            }
            int[] resultList = new int[updateList.Count];
            updateList.CopyTo(resultList);
            return resultList;
        }
        /// <summary>
        /// Creates the Inventory Basis and Capacity RID cross reference dictionaries
        /// </summary>
        /// <returns>True: if success</returns>
        private bool CreateInventoryBasisAllocationXref()
        {
            _styleColorUpdatesInventoryXref = new Dictionary<long, List<int>>();
            _sizeUpdatesInventoryXref = new Dictionary<long, List<int>>();
            _inventoryBasisAllocation = new Dictionary<long, StoreVector>();
            Dictionary<int, StoreVector> maximumBasisAllocation = new Dictionary<int,StoreVector>(); // TT#4208 - MD - Jellis - GA Velocity allocates less than minimum
            _storeTotalMemberMaximum = new StoreVector();                  // TT#4208 - MD - JEllis - GA Velocity allocates less than minimum

            Dictionary<int, HierarchyNodeList> inventoryBasisDescendantStyles = new Dictionary<int, HierarchyNodeList>();
            Dictionary<int, HierarchyNodeList> inventoryBasisDescendantStyleColors = new Dictionary<int, HierarchyNodeList>();
            Dictionary<int, HierarchyNodeList> inventoryBasisDescendantStyleColorSizes = new Dictionary<int, HierarchyNodeList>();
            AllocationProfile[] aplist = AssortmentMembers;
            HierarchyNodeList inventoryList;
            List<int> inventoryXref;

            long styleNoColor;
            long styleColor;
            foreach (AllocationProfile ap in aplist)
            {
                #region build XREF inventory containers
                // build empty lists for each style and color on the headers
                styleNoColor = MIDMath.PackIntegers(Include.IntransitKeyTypeNoColor, ap.StyleHnRID);
                if (!_styleColorUpdatesInventoryXref.TryGetValue(styleNoColor, out inventoryXref))
                {
                    inventoryXref = new List<int>();
                    _styleColorUpdatesInventoryXref.Add(styleNoColor, inventoryXref);
                }

                if (!_sizeUpdatesInventoryXref.TryGetValue(styleNoColor, out inventoryXref))
                {
                    inventoryXref = new List<int>();
                    _sizeUpdatesInventoryXref.Add(styleNoColor, inventoryXref);
                }
                foreach (PackHdr ph in ap.Packs.Values)
                {
                    foreach (PackColorSize pcs in ph.PackColors.Values)
                    {
                        // NOTE: for a pack, pcs.ColorCodeRID may be the no color code
                        styleColor = MIDMath.PackIntegers(pcs.ColorCodeRID, ap.StyleHnRID);
                        if (!_styleColorUpdatesInventoryXref.TryGetValue(styleColor, out inventoryXref))
                        {
                            inventoryXref = new List<int>();
                            _styleColorUpdatesInventoryXref.Add(styleColor, inventoryXref);
                        }
                        if (!_sizeUpdatesInventoryXref.TryGetValue(styleColor, out inventoryXref))
                        {
                            inventoryXref = new List<int>();
                            _sizeUpdatesInventoryXref.Add(styleColor, inventoryXref);
                        }
                    }
                }
                foreach (HdrColorBin hcb in ap.BulkColors.Values)
                {
                    styleColor = MIDMath.PackIntegers(hcb.ColorCodeRID, ap.StyleHnRID);
                    if (!_styleColorUpdatesInventoryXref.TryGetValue(styleColor, out inventoryXref))
                    {
                        inventoryXref = new List<int>();
                        _styleColorUpdatesInventoryXref.Add(styleColor, inventoryXref);
                    }
                    if (!_sizeUpdatesInventoryXref.TryGetValue(styleColor, out inventoryXref))
                    {
                        inventoryXref = new List<int>();
                        _sizeUpdatesInventoryXref.Add(styleColor, inventoryXref);
                    }
                }
                #endregion

                // determine which styles and colors update the inventory basis MDSE levels
                if (ap.GradeInventoryBasisHnRID != Include.NoRID)
                {
                    HierarchyNodeProfile hnp = AppSessionTransaction.GetNodeData(ap.GradeInventoryBasisHnRID);
                    if (hnp.LevelType == eHierarchyLevelType.Color)
                    {
                        if (!inventoryBasisDescendantStyleColors.TryGetValue(ap.GradeInventoryBasisHnRID, out inventoryList))
                        {
                            inventoryList = new HierarchyNodeList(eProfileType.HierarchyNode);
                            inventoryList.Add(hnp);
                            inventoryBasisDescendantStyleColors.Add(ap.GradeInventoryBasisHnRID, inventoryList);
                        }
                    }
                    else
                    {
                        if (!inventoryBasisDescendantStyles.TryGetValue(ap.GradeInventoryBasisHnRID, out inventoryList))
                        {
                            inventoryList = AppSessionTransaction.GetDescendantData(ap.GradeInventoryBasisHnRID, eHierarchyLevelType.Style, eNodeSelectType.All);
                            inventoryBasisDescendantStyles.Add(ap.GradeInventoryBasisHnRID, inventoryList);
                        }
                        if (!inventoryBasisDescendantStyleColors.TryGetValue(ap.GradeInventoryBasisHnRID, out inventoryList))
                        {
                            inventoryList = AppSessionTransaction.GetDescendantData(ap.GradeInventoryBasisHnRID, eHierarchyLevelType.Color, eNodeSelectType.All);
                            inventoryBasisDescendantStyleColors.Add(ap.GradeInventoryBasisHnRID, inventoryList);
                        }
                    }
                }
                if (ap.CapacityNodeRID != Include.NoRID
                    && ap.CapacityNodeRID != ap.GradeInventoryBasisHnRID)
                {
                    HierarchyNodeProfile hnp = AppSessionTransaction.GetNodeData(ap.CapacityNodeRID);
                    if (hnp.LevelType == eHierarchyLevelType.Color)
                    {
                        if (!inventoryBasisDescendantStyleColors.TryGetValue(ap.CapacityNodeRID, out inventoryList))
                        {
                            inventoryList = new HierarchyNodeList(eProfileType.HierarchyNode);
                            inventoryList.Add(hnp);
                            inventoryBasisDescendantStyleColors.Add(ap.CapacityNodeRID, inventoryList);
                        }
                    }
                    else
                    {
                        if (!inventoryBasisDescendantStyles.TryGetValue(ap.CapacityNodeRID, out inventoryList))
                        {
                            inventoryList = AppSessionTransaction.GetDescendantData(ap.CapacityNodeRID, eHierarchyLevelType.Style, eNodeSelectType.All);
                            inventoryBasisDescendantStyles.Add(ap.CapacityNodeRID, inventoryList);
                        }
                        if (!inventoryBasisDescendantStyleColors.TryGetValue(ap.CapacityNodeRID, out inventoryList))
                        {
                            inventoryList = AppSessionTransaction.GetDescendantData(ap.CapacityNodeRID, eHierarchyLevelType.Color, eNodeSelectType.All);
                            inventoryBasisDescendantStyleColors.Add(ap.CapacityNodeRID, inventoryList);
                        }
                    }
                }
                foreach (HdrColorBin hcb in ap.BulkColors.Values)
                {
                    SizeNeedMethod snm = ap.GetSizeNeedMethod(hcb);
                    if (snm != null)
                    {
                        // begin TT#1176 - MD - Jellis - Group Allocation Size need not observing inv min max
                        int invMdseRID = snm.GetInventoryMdseBasisRID(hcb.ColorCodeRID);
                        if (invMdseRID != Include.NoRID)
                        {
                            HierarchyNodeProfile hnp = AppSessionTransaction.GetNodeData(invMdseRID);
                            if (hnp.LevelType == eHierarchyLevelType.Color)
                            {
                                if (!inventoryBasisDescendantStyleColorSizes.TryGetValue(invMdseRID, out inventoryList))
                                {
                                    inventoryList = new HierarchyNodeList(eProfileType.HierarchyNode);
                                    inventoryList.Add(hnp);
                                    inventoryBasisDescendantStyleColorSizes.Add(invMdseRID, inventoryList);
                                }
                            }
                            else
                            {
                                if (!inventoryBasisDescendantStyleColorSizes.TryGetValue(invMdseRID, out inventoryList))
                                {
                                    inventoryList = AppSessionTransaction.GetDescendantData(invMdseRID, eHierarchyLevelType.Style, eNodeSelectType.All);
                                    inventoryBasisDescendantStyleColorSizes.Add(invMdseRID, inventoryList);
                                }
                            }
                        }
                        int mdseRID = snm.GetMerchandiseBasisRID(hcb.ColorCodeRID);
                        if (mdseRID != Include.NoRID
                            && mdseRID != invMdseRID)
                        {
                            HierarchyNodeProfile hnp = AppSessionTransaction.GetNodeData(mdseRID);
                            if (hnp.LevelType == eHierarchyLevelType.Color)
                            {
                                if (!inventoryBasisDescendantStyleColorSizes.TryGetValue(mdseRID, out inventoryList))
                                {
                                    inventoryList = new HierarchyNodeList(eProfileType.HierarchyNode);
                                    inventoryList.Add(hnp);
                                    inventoryBasisDescendantStyleColorSizes.Add(mdseRID, inventoryList);
                                }
                            }
                            else
                            {
                                if (!inventoryBasisDescendantStyleColorSizes.TryGetValue(mdseRID, out inventoryList))
                                {
                                    inventoryList = AppSessionTransaction.GetDescendantData(mdseRID, eHierarchyLevelType.Style, eNodeSelectType.All);
                                    inventoryBasisDescendantStyleColorSizes.Add(mdseRID, inventoryList);
                                }
                            }
                        }
                        //if (!inventoryBasisDescendantStyleColorSizes.TryGetValue(snm.GetInventoryMdseBasisRID(hcb.ColorCodeRID), out inventoryList))
                        //{
                        //    inventoryList = AppSessionTransaction.GetDescendantData(snm.GetInventoryMdseBasisRID(hcb.ColorCodeRID), eHierarchyLevelType.Size, eNodeSelectType.All);
                        //    inventoryBasisDescendantStyleColorSizes.Add(snm.GetInventoryMdseBasisRID(hcb.ColorCodeRID), inventoryList);
                        //}
                        //// begin TT#1176 - MD - Jellis - Group Allocation Size Need not observing inv min max
                        //int mdseRID = snm.GetMerchandiseBasisRID(hcb.ColorCodeRID);
                        //if (mdseRID != snm.GetInventoryMdseBasisRID(hcb.ColorCodeRID))
                        //{
                        //    inventoryList = AppSessionTransaction.GetDescendantData(mdseRID, eHierarchyLevelType.Size, eNodeSelectType.All);
                        //    inventoryBasisDescendantStyleColorSizes.Add(mdseRID, inventoryList);
                        //}
                        //// end TT#1176 - MD - Jellis - Group Allocation Size Need not observing inv min max
                        // end TT#1176 - MD - Jellis - Group Allocation Size need not observing inv min max
                    }
                }
            }

            // populate the inventory lists for each style and color on the header
            // NOTE:  if a header's style updates an inventory basis, then any color in that style should not be directed to populate that same inventory basis.
            //        In other words, the lists for a color and its style are mutually exclusive for the purposes of the style-color allocations.
            foreach (KeyValuePair<int, HierarchyNodeList> keyPair in inventoryBasisDescendantStyles)
            {
                foreach (HierarchyNodeProfile hnp in keyPair.Value)
                {
                    styleNoColor = MIDMath.PackIntegers(Include.IntransitKeyTypeNoColor, hnp.Key); 
                    if (_styleColorUpdatesInventoryXref.TryGetValue(styleNoColor, out inventoryXref))
                    {
					    // Begin TT#4633 - JSmith - GA-Cancel Size Allocation-> Size Need Method again-> Style quantity changes
						//inventoryXref.Add(keyPair.Key);
                        if (!inventoryXref.Contains(keyPair.Key))
                        {
                            inventoryXref.Add(keyPair.Key);
                        }
						// End TT#4633 - JSmith - GA-Cancel Size Allocation-> Size Need Method again-> Style quantity changes
                    }
                }
            }
            bool styleUpdatesInventoryBasis;
            foreach (KeyValuePair<int, HierarchyNodeList> keyPair in inventoryBasisDescendantStyleColors)
            {
                foreach (HierarchyNodeProfile hnp in keyPair.Value)
                {
                    // Parent RID is style
                    List<int> inventoryLevelsUpdatedByStyle;
                    // when style updates inventory basis, it is not necessary to track color inventory allocations (because they will be tracked at the style level)
                    styleNoColor = MIDMath.PackIntegers(Include.IntransitKeyTypeNoColor, hnp.HomeHierarchyParentRID); 
                    if (_styleColorUpdatesInventoryXref.TryGetValue(styleNoColor, out inventoryLevelsUpdatedByStyle))
                    {
                        styleUpdatesInventoryBasis = (inventoryLevelsUpdatedByStyle.Exists(i => i == keyPair.Key));
                    }
                    else
                    {
                        styleUpdatesInventoryBasis = false;
                    }
                    styleColor = MIDMath.PackIntegers(hnp.ColorOrSizeCodeRID, hnp.HomeHierarchyParentRID); 
                    if (!styleUpdatesInventoryBasis)
                    {
                        if (_styleColorUpdatesInventoryXref.TryGetValue(styleColor, out inventoryXref))
                        {
                            // Begin TT#4633 - JSmith - GA-Cancel Size Allocation-> Size Need Method again-> Style quantity changes
							//inventoryXref.Add(keyPair.Key);
							if (!inventoryXref.Contains(keyPair.Key))
                            {
                                inventoryXref.Add(keyPair.Key);
                            }
							// End TT#4633 - JSmith - GA-Cancel Size Allocation-> Size Need Method again-> Style quantity changes
                        }
                    }
                }
            }
            List<long> removeKey = new List<long>();
            foreach (KeyValuePair<long, List<int>> keyPair in _styleColorUpdatesInventoryXref)
            {
                if (keyPair.Value.Count == 0)
                {
                    removeKey.Add(keyPair.Key);
                }
            }
            foreach (long key in removeKey)
            {
                _styleColorUpdatesInventoryXref.Remove(key);
            }

            // populate the size lists for each color on the headers
            foreach (KeyValuePair<int, HierarchyNodeList> keyPair in inventoryBasisDescendantStyleColorSizes)
            {
                foreach (HierarchyNodeProfile hnp in keyPair.Value)
                {
                    // Parent RID is style
                    styleNoColor = MIDMath.PackIntegers(Include.IntransitKeyTypeNoSize, hnp.HomeHierarchyParentRID); 
                    if (_sizeUpdatesInventoryXref.TryGetValue(styleNoColor, out inventoryXref))
                    {
                        if (!inventoryXref.Exists(i => i == keyPair.Key))
                        {
                            inventoryXref.Add(keyPair.Key);
                        }
                    }
                    styleColor = MIDMath.PackIntegers(hnp.ColorOrSizeCodeRID, hnp.HomeHierarchyParentRID);
                    if (_sizeUpdatesInventoryXref.TryGetValue(styleColor, out inventoryXref))
                    {
                        if (!inventoryXref.Exists(i => i == keyPair.Key))
                        {
                            inventoryXref.Add(keyPair.Key);
                        }
                    }
                }
            }
            removeKey.Clear();
            foreach (KeyValuePair<long, List<int>> keyPair in _sizeUpdatesInventoryXref)
            {
                if (keyPair.Value.Count == 0)
                {
                    removeKey.Add(keyPair.Key);
                }
            }
            foreach (long key in removeKey)
            {
                _sizeUpdatesInventoryXref.Remove(key);
            }

            // initial the store vectors
            foreach (AllocationProfile ap in aplist)
            {
                styleNoColor = MIDMath.PackIntegers(Include.IntransitKeyTypeNoColor, ap.StyleHnRID);

                // begin - TT#4208 - MD - Jellis - GA Velocity allocates less than Minimum
                // Want to set maximums BEFORE building Inventory Basis Allocation
                int max;
                int existingMax;
                StoreVector inventoryVector;
                foreach (Index_RID storeIdxRID in AppSessionTransaction.StoreIndexRIDArray())
                {
                    if (!ap.GetStoreOut(eAllocationSummaryNode.Total, storeIdxRID))
                    {
                        max = ap.GetStoreMaximum(eAllocationSummaryNode.Total, storeIdxRID, false); // This should get the grade maximum BEFORE any ALLOCATION adjustments (there may be intransit or onhand adjustments)
                        if (max < int.MaxValue)
                        {
                            existingMax = (int)_storeTotalMemberMaximum.GetStoreValue(storeIdxRID.RID);

                            if (ap.GradeInventoryBasisHnRID != Include.NoRID)
                            {
                                if (!maximumBasisAllocation.TryGetValue(ap.GradeInventoryBasisHnRID, out inventoryVector))
                                {
                                    inventoryVector = new StoreVector();
                                    maximumBasisAllocation.Add(ap.GradeInventoryBasisHnRID, inventoryVector);
                                }
                                max = Math.Max(0, max - (int)inventoryVector.GetStoreValue(storeIdxRID.RID));
                            }
                            if (existingMax < int.MaxValue)
                            {
                                _storeTotalMemberMaximum.SetStoreValue(storeIdxRID.RID, existingMax + max);
                            }
                        }
                        else
                        {
                            _storeTotalMemberMaximum.SetStoreValue(storeIdxRID.RID, int.MaxValue);
                        }
                        if (_styleColorUpdatesInventoryXref.TryGetValue(styleNoColor, out inventoryXref))
                        {
                            foreach (int inventoryBasisRID in inventoryXref)
                            {
                                if (!maximumBasisAllocation.TryGetValue(inventoryBasisRID, out inventoryVector))
                                {
                                    inventoryVector = new StoreVector();
                                    maximumBasisAllocation.Add(inventoryBasisRID, inventoryVector);
                                }
                                if (max < int.MaxValue)
                                {
                                    existingMax = (int)inventoryVector.GetStoreValue(storeIdxRID.RID);
                                    inventoryVector.SetStoreValue(storeIdxRID.RID, existingMax + max);
                                }
                                else
                                {
                                    inventoryVector.SetStoreValue(storeIdxRID.RID, max);
                                }
                            }
                        }
                        foreach (PackHdr ph in ap.Packs.Values)
                        {
                            foreach (PackColorSize pcs in ph.PackColors.Values)
                            {
                                styleColor = MIDMath.PackIntegers(pcs.ColorCodeRID, ap.StyleHnRID);
                                if (_styleColorUpdatesInventoryXref.TryGetValue(styleColor, out inventoryXref))
                                {
                                    foreach (int inventoryBasisRID in inventoryXref)
                                    {
                                        if (!maximumBasisAllocation.TryGetValue(inventoryBasisRID, out inventoryVector))
                                        {
                                            inventoryVector = new StoreVector();
                                            maximumBasisAllocation.Add(inventoryBasisRID, inventoryVector);
                                        }
                                        if (max < int.MaxValue)
                                        {
                                            existingMax = (int)inventoryVector.GetStoreValue(storeIdxRID.RID);
                                            inventoryVector.SetStoreValue(storeIdxRID.RID, existingMax + max);
                                        }
                                        else
                                        {
                                            inventoryVector.SetStoreValue(storeIdxRID.RID, max);
                                        }
                                    }
                                }
                            }
                        }
                        foreach (HdrColorBin hcb in ap.BulkColors.Values)
                        {
                            styleColor = MIDMath.PackIntegers(hcb.ColorCodeRID, ap.StyleHnRID);
                            if (_styleColorUpdatesInventoryXref.TryGetValue(styleColor, out inventoryXref))
                            {
                                foreach (int inventoryBasisRID in inventoryXref)
                                {
                                    if (!maximumBasisAllocation.TryGetValue(inventoryBasisRID, out inventoryVector))
                                    {
                                        inventoryVector = new StoreVector();
                                        maximumBasisAllocation.Add(inventoryBasisRID, inventoryVector);
                                    }
                                    if (max < int.MaxValue)
                                    {
                                        existingMax = (int)inventoryVector.GetStoreValue(storeIdxRID.RID);
                                        inventoryVector.SetStoreValue(storeIdxRID.RID, existingMax + max);
                                    }
                                    else
                                    {
                                        inventoryVector.SetStoreValue(storeIdxRID.RID, max);
                                    }
                                }
                            }
                        }
                    }
                }
                // end - TT#4208 - MD - Jellis - GA Velocity allocates less than Minimum


                if (!ap.StyleIntransitUpdated)
                {
                    if (_styleColorUpdatesInventoryXref.TryGetValue(styleNoColor, out inventoryXref))
                    {
                        foreach (int inventoryBasisRID in inventoryXref)
                        {
                            foreach (Index_RID storeIdxRID in AppSessionTransaction.StoreIndexRIDArray())
                            {
                                SetStoreInventoryBasisAllocation(
                                    inventoryBasisRID,
                                    Include.IntransitKeyTypeNoSize,
                                    storeIdxRID,
                                    GetStoreInventoryBasisAllocation(inventoryBasisRID, Include.IntransitKeyTypeNoSize, storeIdxRID)
                                    + ap.GetStoreQtyAllocated(eAllocationSummaryNode.Total, storeIdxRID));
                            }
                        }
                    }
                    foreach (PackHdr ph in ap.Packs.Values)
                    {
                        foreach (PackColorSize pcs in ph.PackColors.Values)
                        {
                            styleColor = MIDMath.PackIntegers(pcs.ColorCodeRID, ap.StyleHnRID);
                            if (_styleColorUpdatesInventoryXref.TryGetValue(styleColor, out inventoryXref))
                            {
                                foreach (int inventoryBasisRID in inventoryXref)
                                {
                                    foreach (Index_RID storeIdxRID in AppSessionTransaction.StoreIndexRIDArray())
                                    {
                                        SetStoreInventoryBasisAllocation(
                                            inventoryBasisRID,
                                            Include.IntransitKeyTypeNoSize,
                                            storeIdxRID,
                                            GetStoreInventoryBasisAllocation(inventoryBasisRID, Include.IntransitKeyTypeNoSize, storeIdxRID)
                                            + ap.GetStoreQtyAllocated(ph, storeIdxRID)
                                              * pcs.ColorUnitsInPack);
                                    }
                                }
                            }
                            if (_sizeUpdatesInventoryXref.TryGetValue(styleColor, out inventoryXref))
                            {
                                foreach (int inventoryBasisRID in inventoryXref)
                                {
                                    foreach (PackSizeBin psb in pcs.ColorSizes.Values)
                                    {
                                        foreach (Index_RID storeIdxRID in AppSessionTransaction.StoreIndexRIDArray())
                                        {
                                            SetStoreInventoryBasisAllocation(
                                                inventoryBasisRID,
                                                psb.ContentCodeRID,
                                                storeIdxRID,
                                                GetStoreInventoryBasisAllocation(inventoryBasisRID, psb.ContentCodeRID, storeIdxRID)
                                                + ap.GetStoreQtyAllocated(ph, storeIdxRID)
                                                  * psb.ContentUnits);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    foreach (HdrColorBin hcb in ap.BulkColors.Values)
                    {
                        styleColor = MIDMath.PackIntegers(hcb.ColorCodeRID, ap.StyleHnRID);
                        if (_styleColorUpdatesInventoryXref.TryGetValue(styleColor, out inventoryXref))
                        {
                            foreach (int inventoryBasisRID in inventoryXref)
                            {
                                foreach (Index_RID storeIdxRID in AppSessionTransaction.StoreIndexRIDArray())
                                {
                                    SetStoreInventoryBasisAllocation(
                                        inventoryBasisRID,
                                        Include.IntransitKeyTypeNoSize,
                                        storeIdxRID,
                                        GetStoreInventoryBasisAllocation(inventoryBasisRID, Include.IntransitKeyTypeNoSize, storeIdxRID)
                                        + ap.GetStoreQtyAllocated(hcb, storeIdxRID));
                                }
                            }
                        }
                    }
                }
                if (!ap.BulkSizeIntransitUpdated)
                {
                    foreach (HdrColorBin hcb in ap.BulkColors.Values)
                    {
                        styleColor = MIDMath.PackIntegers(hcb.ColorCodeRID, ap.StyleHnRID);
                        if (_sizeUpdatesInventoryXref.TryGetValue(styleColor, out inventoryXref))
                        {
                            foreach (int inventoryBasisRID in inventoryXref)
                            {
                                foreach (HdrSizeBin hsb in hcb.ColorSizes.Values)
                                {
                                    foreach (Index_RID storeIdxRID in AppSessionTransaction.StoreIndexRIDArray())
                                    {
                                        SetStoreInventoryBasisAllocation(
                                            inventoryBasisRID,
                                            hsb.SizeCodeRID,
                                            storeIdxRID,
                                            GetStoreInventoryBasisAllocation(inventoryBasisRID, hsb.SizeCodeRID, storeIdxRID)
                                            + ap.GetStoreQtyAllocated(hsb, storeIdxRID));
                                   
                                    }
                                }
                            }
                        }
                    }
                }

            }
            _buildInventoryBasisAllocation = false;
            return true;
        }

        public void AdjustStoreInventoryBasisAllocation(int aStyleHnRID, int aColorCodeRID, Index_RID aStoreIdxRID, int aQtyDifference)
        {
            if (_buildInventoryBasisAllocation)
            {
                CreateInventoryBasisAllocationXref();
            }
            long styleColor;
            List<int> inventoryXref;
            styleColor = MIDMath.PackIntegers(Math.Max(aColorCodeRID, Include.IntransitKeyTypeNoColor), aStyleHnRID);
            if (_styleColorUpdatesInventoryXref.TryGetValue(styleColor, out inventoryXref))
            {
                foreach (int inventoryBasisRID in inventoryXref)
                {
                    SetStoreInventoryBasisAllocation(
                        inventoryBasisRID,
                        Include.IntransitKeyTypeNoSize,
                        aStoreIdxRID,
                        GetStoreInventoryBasisAllocation(inventoryBasisRID, aStoreIdxRID)
                        + aQtyDifference);
                }
            }
        }

        public void AdjustStoreSizeInventoryBasisAllocation(int aStyleHnRID, int aColorCodeRID, int aSizeCodeRID, Index_RID aStoreIdxRID, int aQtyDifference)
        {
            if (_buildInventoryBasisAllocation)
            {
                CreateInventoryBasisAllocationXref();
            }
            long styleColor;
            List<int> inventoryXref;
            styleColor = MIDMath.PackIntegers(Math.Max(aColorCodeRID, Include.IntransitKeyTypeNoColor), aStyleHnRID);
            if (_sizeUpdatesInventoryXref.TryGetValue(styleColor, out inventoryXref))
            {
                foreach (int inventoryBasisRID in inventoryXref)
                {
                    SetStoreInventoryBasisAllocation(
                        inventoryBasisRID,
                        aSizeCodeRID,
                        aStoreIdxRID,
                        GetStoreInventoryBasisAllocation(inventoryBasisRID, aSizeCodeRID, aStoreIdxRID)
                        + aQtyDifference);
                }
            }
        }

        public int GetStoreSizeInventoryBasisAllocation(List<int> aInventoryBasisRIDs, int aSizeCodeRID, Index_RID aStoreIdxRID)
        {
            int allocation = 0;
            foreach (int inventoryBasisRID in aInventoryBasisRIDs)
            {
                allocation += GetStoreSizeInventoryBasisAllocation(inventoryBasisRID, aSizeCodeRID, aStoreIdxRID);
            }
            return allocation;
        }
        public int GetStoreSizeInventoryBasisAllocation(int aInventoryBasisRID, int aSizeCodeRID, Index_RID aStoreIdxRID)
        {
            if (_buildInventoryBasisAllocation)
            {
                CreateInventoryBasisAllocationXref();
            }
            return GetStoreInventoryBasisAllocation(aInventoryBasisRID, aSizeCodeRID, aStoreIdxRID);
        }
        public int GetStoreInventoryBasisAllocation(List<int> aInventoryBasisRIDs, Index_RID aStoreIdxRID)
        {
            int allocation = 0;
            foreach (int inventoryBasisRID in aInventoryBasisRIDs)
            {
                allocation += GetStoreInventoryBasisAllocation(inventoryBasisRID, aStoreIdxRID);
            }
            return allocation;
        }
        public int GetStoreInventoryBasisAllocation(int aInventoryBasisRID, Index_RID aStoreIdxRID)
        {
            if (_buildInventoryBasisAllocation)
            {
                CreateInventoryBasisAllocationXref();
            }
            return GetStoreInventoryBasisAllocation(aInventoryBasisRID, Include.IntransitKeyTypeNoSize, aStoreIdxRID);
        }
        private int GetStoreInventoryBasisAllocation(int aInventoryBasisRID, int aSizeCodeRID, Index_RID aStoreIdxRID)
        {
            long basisInventorySize = MIDMath.PackIntegers(aSizeCodeRID, aInventoryBasisRID);
            StoreVector inventoryVector;
            if (_inventoryBasisAllocation.TryGetValue(basisInventorySize, out inventoryVector))
            {
                return (int)inventoryVector.GetStoreValue(aStoreIdxRID.RID);
            }
            return 0;
        }

        private void SetStoreInventoryBasisAllocation(int aInventoryBasisRID, int aSizeCodeRID, Index_RID aStoreIdxRID, int aQtyAllocated)
        {
            long basisInventorySize = MIDMath.PackIntegers(aSizeCodeRID, aInventoryBasisRID);
            StoreVector inventoryVector;
            if (!_inventoryBasisAllocation.TryGetValue(basisInventorySize, out inventoryVector))
            {
                inventoryVector = new StoreVector(); 
                _inventoryBasisAllocation.Add(basisInventorySize, inventoryVector);        
            }
            inventoryVector.SetStoreValue(aStoreIdxRID.RID, aQtyAllocated);
        }

        // begin TT#3941 - Urban - Jellis - Group Allocation Item/VSW split incorrect
        /// <summary>
        /// Gets store style color prior Item manually allocated units across all member headers.
        /// </summary>
        /// <param name="aStyleHnRID">Style Hierarchy Node RID that identifies the Style</param>
        /// <param name="aColorCodeRID">Color CODE RID that identifies the color</param>
        /// <param name="aStoreIdxRID">Store Index_RID that identifies the store</param>
        /// <returns>Total ITEM units manually allocated across all member headers</returns>
        public int GetStoreStyleColorPriorItemManuallyAllocated(int aStyleHnRID, int aColorCodeRID, Index_RID aStoreIdxRID)
        {
            if (_buildPriorItemUnitsManuallyAllocated)
            {
                BuildPriorItemUnitsManuallyAllocated();
            }
            long styleColorItemKey = ((long)aStyleHnRID << 32) + (long)aColorCodeRID;
            StoreVector storeItemManuallyAllocatedVector;
            if (_priorStyleColorItemUnitsManuallyAllocated.TryGetValue(styleColorItemKey, out storeItemManuallyAllocatedVector))
            {
                return (int)storeItemManuallyAllocatedVector.GetStoreValue(aStoreIdxRID.RID);
            }
            return 0;
        }
        /// <summary>
        /// Gets store style-color-size prior Item manually allocated units across all member headers 
        /// </summary>
        /// <param name="aStyleHnRID">Style Hierarchy Node RID that identifies the style</param>
        /// <param name="aColorCodeRID">Color CODE RID that identifies the color</param>
        /// <param name="aSizeCodeRID">Size CODE RID that identifies the size</param>
        /// <param name="aStoreIdxRID">Store Index_RID that identifies the store</param>
        /// <returns>Total SIZE ITEM units manually allocated across all member headers</returns>
        public int GetStoreSizeColorPriorItemManuallyAllocated(int aStyleHnRID, int aColorCodeRID, int aSizeCodeRID, Index_RID aStoreIdxRID)
        {
            if (_buildPriorItemUnitsManuallyAllocated)
            {
                BuildPriorItemUnitsManuallyAllocated();
            }
            long styleColorItemKey = ((long)aStyleHnRID << 32) + (long)aColorCodeRID;
            StoreVector storeItemManuallyAllocatedVector;
            Dictionary<int, StoreVector> priorSizeItemManuallyAllocated;
            if (_priorSizeItemUnitsManuallyAllocated.TryGetValue(styleColorItemKey, out priorSizeItemManuallyAllocated))
            {
                if (priorSizeItemManuallyAllocated.TryGetValue(aSizeCodeRID, out storeItemManuallyAllocatedVector))
                {
                    return (int)storeItemManuallyAllocatedVector.GetStoreValue(aStoreIdxRID.RID);
                }
            }
            return 0;
        }
        /// <summary>
        /// Builds Prior Item Units Manually allocated across all member headers.
        /// </summary>
        private void BuildPriorItemUnitsManuallyAllocated()
        {
            _priorStyleColorItemUnitsManuallyAllocated = new Dictionary<long, StoreVector>();
            _priorSizeItemUnitsManuallyAllocated = new Dictionary<long, Dictionary<int, StoreVector>>();
           
            long styleColorItemKey;
            long styleTotalItemKey;
            AllocationProfile[] apList = AssortmentMembers;
            Index_RID[] storeIdxRIDArray = AppSessionTransaction.StoreIndexRIDArray();
            StoreVector styleTotalItemUnitsManuallyAllocated;
            StoreVector styleColorItemUnitsManuallyAllocated;
            Dictionary<int, StoreVector> styleTotalSizeItemUnitsManuallyAllocated;
            Dictionary<int, StoreVector> styleColorSizeItemUnitsManuallyAllocated;
            // Begin TT#1516-MD - stodd - Decreasing the Quantity on a header attached to an Assortment causes a "Quantity to allocate cannot be less than zero" error
            //bool addStyleTotalVector = false;
            //bool addStyleColorVector = false;
            // End TT#1516-MD - stodd - Decreasing the Quantity on a header attached to an Assortment causes a "Quantity to allocate cannot be less than zero" error
            foreach (AllocationProfile ap in apList)
            {
                // Begin TT#1516-MD - stodd - Decreasing the Quantity on a header attached to an Assortment causes a "Quantity to allocate cannot be less than zero" error
                bool addStyleTotalVector = false;
                bool addStyleColorVector = false;
                // End TT#1516-MD - stodd - Decreasing the Quantity on a header attached to an Assortment causes a "Quantity to allocate cannot be less than zero" error

                styleTotalItemKey = ((long)ap.StyleHnRID << 32) + (long)Include.DummyColorRID;
                styleColorItemKey = ((long)ap.StyleHnRID << 32) + (long)ap.HeaderColorCodeRID;

                if (!_priorStyleColorItemUnitsManuallyAllocated.TryGetValue(styleTotalItemKey, out styleTotalItemUnitsManuallyAllocated))
                {
                    styleTotalItemUnitsManuallyAllocated = new StoreVector();
                    addStyleTotalVector = true;
                }
                    
                if (!_priorStyleColorItemUnitsManuallyAllocated.TryGetValue(styleColorItemKey, out styleColorItemUnitsManuallyAllocated))
                {
                    // NOTE:  if ap.HeaderColorCodeRID == Include.DummyColorRID, then styleColorItemUnitsManuallyAllocated Could be equal styleTotalItemUnitsManuallyAllocated
                    if (ap.HeaderColorCodeRID != Include.DummyColorRID)
                    {
                        styleColorItemUnitsManuallyAllocated = new StoreVector();
                        addStyleColorVector = true;
                    }
                }

                if (!_priorSizeItemUnitsManuallyAllocated.TryGetValue(styleTotalItemKey, out styleTotalSizeItemUnitsManuallyAllocated))
                {
                    styleTotalSizeItemUnitsManuallyAllocated = new Dictionary<int,StoreVector>();
                    _priorSizeItemUnitsManuallyAllocated.Add(styleTotalItemKey, styleTotalSizeItemUnitsManuallyAllocated);
                }
                if (!_priorSizeItemUnitsManuallyAllocated.TryGetValue(styleColorItemKey, out styleColorSizeItemUnitsManuallyAllocated))
                {
                    if (ap.HeaderColorCodeRID != Include.DummyColorRID)
                    {
                        styleColorSizeItemUnitsManuallyAllocated = new Dictionary<int,StoreVector>();
                        _priorSizeItemUnitsManuallyAllocated.Add(styleColorItemKey, styleColorSizeItemUnitsManuallyAllocated);
                    }
                }
                if (!ap.StyleIntransitUpdated)
                {
                    foreach (Index_RID storeIdxRID in storeIdxRIDArray)
                    {
                        if (ap.GetStoreItemIsManuallyAllocated(eAllocationSummaryNode.Total, storeIdxRID))
                        {
                            styleTotalItemUnitsManuallyAllocated.SetStoreValue(
                                storeIdxRID.RID, 
                                styleTotalItemUnitsManuallyAllocated.GetStoreValue(storeIdxRID.RID)
                                + ap.GetStoreItemQtyAllocated(eAllocationSummaryNode.Total, storeIdxRID));
                            if (ap.HeaderColorCodeRID != Include.DummyColorRID)
                            {
                                styleColorItemUnitsManuallyAllocated.SetStoreValue(
                                    storeIdxRID.RID,
                                    styleColorItemUnitsManuallyAllocated.GetStoreValue(storeIdxRID.RID)
                                    + ap.GetStoreItemQtyAllocated(eAllocationSummaryNode.Total, storeIdxRID));
                            }
                            else
                            {
                                UpdateColorPriorItemUnitsManuallyAllocated(
                                    ap,
                                    true,
                                    false,
                                    false,
                                    ref styleTotalItemUnitsManuallyAllocated,
                                    ref styleColorItemUnitsManuallyAllocated,
                                    ref styleTotalSizeItemUnitsManuallyAllocated,
                                    ref styleColorSizeItemUnitsManuallyAllocated,
                                    storeIdxRID);
                            }
                        }
                        else if (ap.GetStoreItemIsManuallyAllocated(eAllocationSummaryNode.DetailType, storeIdxRID))
                        {
                            styleTotalItemUnitsManuallyAllocated.SetStoreValue(
                                storeIdxRID.RID,
                                styleTotalItemUnitsManuallyAllocated.GetStoreValue(storeIdxRID.RID)
                                + ap.GetStoreItemQtyAllocated(eAllocationSummaryNode.DetailType, storeIdxRID));
                            if (ap.HeaderColorCodeRID != Include.DummyColorRID)
                            {
                                styleColorItemUnitsManuallyAllocated.SetStoreValue(
                                    storeIdxRID.RID,
                                    styleColorItemUnitsManuallyAllocated.GetStoreValue(storeIdxRID.RID)
                                    + ap.GetStoreItemQtyAllocated(eAllocationSummaryNode.DetailType, storeIdxRID));
                            }
                            else
                            {
                                UpdateColorPriorItemUnitsManuallyAllocated(
                                    ap,
                                    false,
                                    true,
                                    false,
                                    ref styleTotalItemUnitsManuallyAllocated,
                                    ref styleColorItemUnitsManuallyAllocated,
                                    ref styleTotalSizeItemUnitsManuallyAllocated,
                                    ref styleColorSizeItemUnitsManuallyAllocated,
                                    storeIdxRID);
                            }
                        }
                        else if (ap.GetStoreItemIsManuallyAllocated(eAllocationSummaryNode.Bulk, storeIdxRID))
                        {
                            styleTotalItemUnitsManuallyAllocated.SetStoreValue(
                                storeIdxRID.RID,
                                styleTotalItemUnitsManuallyAllocated.GetStoreValue(storeIdxRID.RID)
                                + ap.GetStoreItemQtyAllocated(eAllocationSummaryNode.Bulk, storeIdxRID));
                            if (ap.HeaderColorCodeRID != Include.DummyColorRID)
                            {
                                styleColorItemUnitsManuallyAllocated.SetStoreValue(
                                    storeIdxRID.RID,
                                    styleColorItemUnitsManuallyAllocated.GetStoreValue(storeIdxRID.RID)
                                    + ap.GetStoreItemQtyAllocated(eAllocationSummaryNode.Bulk, storeIdxRID));
                            }
                            else
                            {
                                UpdateColorPriorItemUnitsManuallyAllocated(
                                    ap,
                                    false,
                                    false,
                                    true,
                                    ref styleTotalItemUnitsManuallyAllocated,
                                    ref styleColorItemUnitsManuallyAllocated,
                                    ref styleTotalSizeItemUnitsManuallyAllocated,
                                    ref styleColorSizeItemUnitsManuallyAllocated,
                                    storeIdxRID);
                            }
                        }
                        else
                        {
                            UpdateColorPriorItemUnitsManuallyAllocated(
                                ap,
                                false,
                                false,
                                false,
                                ref styleTotalItemUnitsManuallyAllocated,
                                ref styleColorItemUnitsManuallyAllocated,
                                ref styleTotalSizeItemUnitsManuallyAllocated,
                                ref styleColorSizeItemUnitsManuallyAllocated,
                                storeIdxRID);
                        }
                    }
                    if (addStyleTotalVector
                        && styleTotalItemUnitsManuallyAllocated.AllStoreTotalValue > 0)
                    {
                        _priorStyleColorItemUnitsManuallyAllocated.Add(styleTotalItemKey, styleTotalItemUnitsManuallyAllocated);
                        addStyleTotalVector = false;
                    }
                    if (addStyleColorVector
                        && styleColorItemUnitsManuallyAllocated != null  // TT#4850 - JSmith - GA - header one has sizes that are in dimension, header two has different sizes that do not have dimension-> error message when attempt to create a Group Header
                        && styleColorItemUnitsManuallyAllocated.AllStoreTotalValue > 0)
                    {
                        _priorStyleColorItemUnitsManuallyAllocated.Add(styleColorItemKey, styleColorItemUnitsManuallyAllocated);
                        addStyleColorVector = false;
                    }
                }
                else if (!ap.BulkSizeIntransitUpdated)
                {
                    Dictionary<int, StoreVector> colorSizeItemManuallyAllocated;
                    StoreVector sizeItemManuallyAllocated;
                    long colorItemKey;
                    foreach (Index_RID storeIdxRID in storeIdxRIDArray)
                    {
                        foreach (HdrColorBin hcb in ap.BulkColors.Values)
                        {
                            colorItemKey = ((long)ap.StyleHnRID << 32) + (long)hcb.ColorCodeRID;
                            if (ap.HeaderColorCodeRID == hcb.ColorCodeRID)
                            {
                                colorSizeItemManuallyAllocated = styleColorSizeItemUnitsManuallyAllocated;
								// Begin TT#1593-MD - JSmith - Quantity Allocated cannot be less than zero
								//foreach (HdrSizeBin hsb in hcb.ColorSizes)
                                foreach (HdrSizeBin hsb in hcb.ColorSizes.Values)
								// End TT#1593-MD - JSmith - Quantity Allocated cannot be less than zero
                                {
                                    if (ap.GetStoreItemIsManuallyAllocated(hsb, storeIdxRID))
                                    {
                                        if (!styleTotalSizeItemUnitsManuallyAllocated.TryGetValue(hsb.SizeCodeRID, out sizeItemManuallyAllocated))
                                        {
                                            sizeItemManuallyAllocated = new StoreVector();
                                            styleTotalSizeItemUnitsManuallyAllocated.Add(hsb.SizeCodeRID, sizeItemManuallyAllocated);
                                        }
                                        sizeItemManuallyAllocated.SetStoreValue(
                                            storeIdxRID.RID,
                                            sizeItemManuallyAllocated.GetStoreValue(storeIdxRID.RID)
                                            + ap.GetStoreItemQtyAllocated(hsb, storeIdxRID));
                                        if (!colorSizeItemManuallyAllocated.TryGetValue(hsb.SizeCodeRID, out sizeItemManuallyAllocated))
                                        {
                                            sizeItemManuallyAllocated = new StoreVector();
                                            colorSizeItemManuallyAllocated.Add(hsb.SizeCodeRID, sizeItemManuallyAllocated);
                                        }
                                        sizeItemManuallyAllocated.SetStoreValue(
                                            storeIdxRID.RID,
                                            sizeItemManuallyAllocated.GetStoreValue(storeIdxRID.RID)
                                            + ap.GetStoreItemQtyAllocated(hsb, storeIdxRID));
                                        }
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aAllocationProfile"></param>
        /// <param name="aStyleTotalItemUnitsManuallyAllocated"></param>
        /// <param name="aStyleColorItemUnitsManuallyAllocated"></param>
        /// <param name="aStoreIdxRID"></param>
        private void UpdateColorPriorItemUnitsManuallyAllocated(
            AllocationProfile aAllocationProfile, 
            bool aTotalManuallyAllocated,
            bool aDetailManuallyAllocated,
            bool aBulkManuallyAllocated,
            ref StoreVector aStyleTotalItemUnitsManuallyAllocated, 
            ref StoreVector aStyleColorItemUnitsManuallyAllocated,
            ref Dictionary<int, StoreVector> aTotalSizeItemUnitsManuallyAllocated,
            ref Dictionary<int, StoreVector> aColorSizeItemUnitsManuallyAllocated,
            Index_RID aStoreIdxRID)
        {
            foreach (PackHdr ph in aAllocationProfile.GenericPacks.Values)
            {
                if (aAllocationProfile.GetStoreItemIsManuallyAllocated(ph, aStoreIdxRID))
                {
                    UpdatePackPriorItemManuallyAllocated(
                        (aTotalManuallyAllocated),
                        aAllocationProfile,
                        ph,
                        ref aStyleTotalItemUnitsManuallyAllocated,
                        ref aStyleColorItemUnitsManuallyAllocated,
                        ref aTotalSizeItemUnitsManuallyAllocated,
                        ref aColorSizeItemUnitsManuallyAllocated,
                        aStoreIdxRID);
                }
            }
            bool totalManuallyAllocated = (aTotalManuallyAllocated || aDetailManuallyAllocated);
            foreach (PackHdr ph in aAllocationProfile.NonGenericPacks.Values)
            {
                if (aAllocationProfile.GetStoreItemIsManuallyAllocated(ph, aStoreIdxRID))
                {
                    UpdatePackPriorItemManuallyAllocated(
                        totalManuallyAllocated,
                        aAllocationProfile,
                        ph,
                        ref aStyleTotalItemUnitsManuallyAllocated,
                        ref aStyleColorItemUnitsManuallyAllocated,
                        ref aTotalSizeItemUnitsManuallyAllocated,
                        ref aColorSizeItemUnitsManuallyAllocated,
                        aStoreIdxRID);
                }
            }
            if (aAllocationProfile.BulkIsDetail)
            {
                totalManuallyAllocated = (totalManuallyAllocated || aBulkManuallyAllocated);
            }
            else
            {
                totalManuallyAllocated = (aTotalManuallyAllocated || aBulkManuallyAllocated);
            }

            Dictionary<int, StoreVector> colorSizeItemManuallyAllocated;
            long colorItemKey;
            foreach (HdrColorBin hcb in aAllocationProfile.BulkColors.Values)
            {
                colorItemKey = ((long)aAllocationProfile.StyleHnRID << 32) + (long)hcb.ColorCodeRID;
                if (aAllocationProfile.GetStoreItemIsManuallyAllocated(hcb, aStoreIdxRID))
                {
                    if (!totalManuallyAllocated)
                    {
                        aStyleTotalItemUnitsManuallyAllocated.SetStoreValue(
                            aStoreIdxRID.RID,
                            aStyleTotalItemUnitsManuallyAllocated.GetStoreValue(aStoreIdxRID.RID)
                            + aAllocationProfile.GetStoreItemQtyAllocated(hcb, aStoreIdxRID));
                    }
                    if (aAllocationProfile.HeaderColorCodeRID == hcb.ColorCodeRID)
                    {
                        aStyleColorItemUnitsManuallyAllocated.SetStoreValue(
                            aStoreIdxRID.RID,
                            aStyleColorItemUnitsManuallyAllocated.GetStoreValue(aStoreIdxRID.RID)
                            + aAllocationProfile.GetStoreItemQtyAllocated(hcb, aStoreIdxRID));
                    }
                    else
                    {
                        // more than one color is on the header
                        StoreVector colorItemUnitsManuallyAllocated;
                        if (_priorStyleColorItemUnitsManuallyAllocated.TryGetValue(colorItemKey, out colorItemUnitsManuallyAllocated))
                        {
                            colorItemUnitsManuallyAllocated = new StoreVector();
                            _priorStyleColorItemUnitsManuallyAllocated.Add(colorItemKey, colorItemUnitsManuallyAllocated);
                        }
                        colorItemUnitsManuallyAllocated.SetStoreValue(
                            aStoreIdxRID.RID,
                            colorItemUnitsManuallyAllocated.GetStoreValue(aStoreIdxRID.RID)
                            + aAllocationProfile.GetStoreItemQtyAllocated(hcb, aStoreIdxRID));
                    }
                }
                StoreVector sizeItemManuallyAllocated;
                if (aAllocationProfile.HeaderColorCodeRID == hcb.ColorCodeRID)
                {
                    colorSizeItemManuallyAllocated = aColorSizeItemUnitsManuallyAllocated;
                    foreach (HdrSizeBin hsb in hcb.ColorSizes.Values)
                    {
                        if (aAllocationProfile.GetStoreItemIsManuallyAllocated(hsb, aStoreIdxRID))
                        {
                            if (!aTotalSizeItemUnitsManuallyAllocated.TryGetValue(hsb.SizeCodeRID, out sizeItemManuallyAllocated))
                            {
                                sizeItemManuallyAllocated = new StoreVector();
                                aTotalSizeItemUnitsManuallyAllocated.Add(hsb.SizeCodeRID, sizeItemManuallyAllocated);
                            }
                            sizeItemManuallyAllocated.SetStoreValue(
                                aStoreIdxRID.RID,
                                sizeItemManuallyAllocated.GetStoreValue(aStoreIdxRID.RID)
                                + aAllocationProfile.GetStoreItemQtyAllocated(hsb, aStoreIdxRID));
                            if (!colorSizeItemManuallyAllocated.TryGetValue(hsb.SizeCodeRID, out sizeItemManuallyAllocated))
                            {
                                sizeItemManuallyAllocated = new StoreVector();
                                colorSizeItemManuallyAllocated.Add(hsb.SizeCodeRID, sizeItemManuallyAllocated);
                            }
                            sizeItemManuallyAllocated.SetStoreValue(
                                aStoreIdxRID.RID,
                                sizeItemManuallyAllocated.GetStoreValue(aStoreIdxRID.RID)
                                + aAllocationProfile.GetStoreItemQtyAllocated(hsb, aStoreIdxRID));
                         }
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aTotalManuallyAllocated"></param>
        /// <param name="aAllocationProfile"></param>
        /// <param name="aPackHdr"></param>
        /// <param name="aStyleTotalItemUnitsManuallyAllocated"></param>
        /// <param name="aStyleColorItemUnitsManuallyAllocated"></param>
        /// <param name="aStoreIdxRID"></param>
        private void UpdatePackPriorItemManuallyAllocated
            (
            bool aTotalManuallyAllocated,
            AllocationProfile aAllocationProfile,
            PackHdr aPackHdr,
            ref StoreVector aStyleTotalItemUnitsManuallyAllocated,
            ref StoreVector aStyleColorItemUnitsManuallyAllocated,
            ref Dictionary<int, StoreVector> aTotalSizeItemUnitsManuallyAllocated,
            ref Dictionary<int, StoreVector> aColorSizeItemUnitsManuallyAllocated,
            Index_RID aStoreIdxRID
            )
        {
            StoreVector sizeItemUnitsManuallyAllocated;
            if (aAllocationProfile.GetStoreItemIsManuallyAllocated(aPackHdr, aStoreIdxRID))
            {
                if (!aTotalManuallyAllocated)
                {
                    aStyleTotalItemUnitsManuallyAllocated.SetStoreValue(
                        aStoreIdxRID.RID,
                        aStyleTotalItemUnitsManuallyAllocated.GetStoreValue(aStoreIdxRID.RID)
                        + aAllocationProfile.GetStoreItemQtyAllocated(aPackHdr, aStoreIdxRID)
                          * aPackHdr.PackMultiple);
                }
                foreach (PackColorSize pcs in aPackHdr.PackColors.Values)
                {
                    if (aAllocationProfile.HeaderColorCodeRID != Include.DummyColorRID)
                    {
                        if (aAllocationProfile.HeaderColorCodeRID == pcs.ColorCodeRID)
                        {
                            aStyleColorItemUnitsManuallyAllocated.SetStoreValue(
                                aStoreIdxRID.RID,
                                aStyleColorItemUnitsManuallyAllocated.GetStoreValue(aStoreIdxRID.RID)
                                + aAllocationProfile.GetStoreItemQtyAllocated(aPackHdr, aStoreIdxRID)
                                  * pcs.ColorUnitsInPack);
                            foreach (PackContentBin sizeBin in pcs.ColorSizes.Values)
                            {
                                if (!aTotalSizeItemUnitsManuallyAllocated.TryGetValue(sizeBin.ContentCodeRID, out sizeItemUnitsManuallyAllocated))
                                {
                                    sizeItemUnitsManuallyAllocated = new StoreVector();
                                    aTotalSizeItemUnitsManuallyAllocated.Add(sizeBin.ContentCodeRID, sizeItemUnitsManuallyAllocated);
                                }
                                sizeItemUnitsManuallyAllocated.SetStoreValue(
                                    aStoreIdxRID.RID,
                                    sizeItemUnitsManuallyAllocated.GetStoreValue(aStoreIdxRID.RID)
                                    + aAllocationProfile.GetStoreItemQtyAllocated(aPackHdr, aStoreIdxRID)
                                      * sizeBin.ContentUnits);
                                if (!aColorSizeItemUnitsManuallyAllocated.TryGetValue(sizeBin.ContentCodeRID, out sizeItemUnitsManuallyAllocated))
                                {
                                    sizeItemUnitsManuallyAllocated = new StoreVector();
                                    aColorSizeItemUnitsManuallyAllocated.Add(sizeBin.ContentCodeRID, sizeItemUnitsManuallyAllocated);
                                }
                                sizeItemUnitsManuallyAllocated.SetStoreValue(
                                    aStoreIdxRID.RID,
                                    sizeItemUnitsManuallyAllocated.GetStoreValue(aStoreIdxRID.RID)
                                    + aAllocationProfile.GetStoreItemQtyAllocated(aPackHdr, aStoreIdxRID)
                                      * sizeBin.ContentUnits);
                            }
                        }
                        else
                        {
                            // more than one color is on the header
                            StoreVector colorItemUnitsManuallyAllocated;
                            long colorItemKey = ((long)aAllocationProfile.StyleHnRID << 32) + (long)pcs.ColorCodeRID;
                            if (_priorStyleColorItemUnitsManuallyAllocated.TryGetValue(colorItemKey, out colorItemUnitsManuallyAllocated))
                            {
                                colorItemUnitsManuallyAllocated = new StoreVector();
                                _priorStyleColorItemUnitsManuallyAllocated.Add(colorItemKey, colorItemUnitsManuallyAllocated);
                            }
                            colorItemUnitsManuallyAllocated.SetStoreValue(
                                aStoreIdxRID.RID,
                                colorItemUnitsManuallyAllocated.GetStoreValue(aStoreIdxRID.RID)
                                + aAllocationProfile.GetStoreQtyAllocated(aPackHdr, aStoreIdxRID)
                                  * pcs.ColorUnitsInPack);
                            Dictionary<int, StoreVector> multiColorItemUnitsManuallyAllocated;
                            if (!_priorSizeItemUnitsManuallyAllocated.TryGetValue(colorItemKey, out multiColorItemUnitsManuallyAllocated))
                            {
                                multiColorItemUnitsManuallyAllocated = new Dictionary<int,StoreVector>();
                                _priorSizeItemUnitsManuallyAllocated.Add(colorItemKey, multiColorItemUnitsManuallyAllocated);
                            }
                            foreach (PackContentBin sizeBin in pcs.ColorSizes.Values)
                            {
                                if (!aTotalSizeItemUnitsManuallyAllocated.TryGetValue(sizeBin.ContentCodeRID, out sizeItemUnitsManuallyAllocated))
                                {
                                    sizeItemUnitsManuallyAllocated = new StoreVector();
                                    aTotalSizeItemUnitsManuallyAllocated.Add(sizeBin.ContentCodeRID, sizeItemUnitsManuallyAllocated);
                                }
                                sizeItemUnitsManuallyAllocated.SetStoreValue(
                                    aStoreIdxRID.RID,
                                    sizeItemUnitsManuallyAllocated.GetStoreValue(aStoreIdxRID.RID)
                                    + aAllocationProfile.GetStoreItemQtyAllocated(aPackHdr, aStoreIdxRID)
                                      * sizeBin.ContentUnits);
                                if (!multiColorItemUnitsManuallyAllocated.TryGetValue(sizeBin.ContentCodeRID, out sizeItemUnitsManuallyAllocated))
                                {
                                    sizeItemUnitsManuallyAllocated = new StoreVector();
                                    multiColorItemUnitsManuallyAllocated.Add(sizeBin.ContentCodeRID, sizeItemUnitsManuallyAllocated);
                                }
                                sizeItemUnitsManuallyAllocated.SetStoreValue(
                                    aStoreIdxRID.RID,
                                    sizeItemUnitsManuallyAllocated.GetStoreValue(aStoreIdxRID.RID)
                                    + aAllocationProfile.GetStoreItemQtyAllocated(aPackHdr, aStoreIdxRID)
                                      * sizeBin.ContentUnits);
                            }
                        }
                    }
                    else
                    {
                        foreach (PackContentBin sizeBin in pcs.ColorSizes.Values)
                        {
                            if (!aTotalSizeItemUnitsManuallyAllocated.TryGetValue(sizeBin.ContentCodeRID, out sizeItemUnitsManuallyAllocated))
                            {
                                sizeItemUnitsManuallyAllocated = new StoreVector();
                                aTotalSizeItemUnitsManuallyAllocated.Add(sizeBin.ContentCodeRID, sizeItemUnitsManuallyAllocated);
                            }
                            sizeItemUnitsManuallyAllocated.SetStoreValue(
                                aStoreIdxRID.RID,
                                sizeItemUnitsManuallyAllocated.GetStoreValue(aStoreIdxRID.RID)
                                + aAllocationProfile.GetStoreItemQtyAllocated(aPackHdr, aStoreIdxRID)
                                    * sizeBin.ContentUnits);
                        }
                    }
                }
            }
        }
        // end TT#3941 - Urban - Jellis - Group Allocation Item/VSW split incorrect

        // end TT#1074 - MD - Jellis- Inventory Min Max

        #region Allocation Special Methods

        public void AssortmentPropertiesRebuild()
        {
            // TT#935 - MD - Jellis - Group Allocation Infrastructure built wrong
            if (_buildingAssortmentProperties    // TT#946 - MD - Jellis - Group Allocation Not Working
                || !_assortmentLoaded)           // TT#946 - MD - Jellis - Group Allocation Not Working
            {
                return;
            }
            //_buildingAssortmentProperties = true; // TT#897 - MD - Jellis - Null Reference  // TT#927 - MD - Jellis - Allocations disappear from Style Review when coming from Group Allocation
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Gets Error
            try // TT#946 - MD - Jellis - Group Allocation Not Working
            {   // TT#946 - MD - Jellis - Group Allocation Not Working
                _buildingAssortmentProperties = true; // TT#897 - MD - Jellis - Null Reference  // TT#927 - MD - Jellis - Allocations disappear from Style Review when coming from Group Allocation
                if (!base.StoresLoaded)
                {
                    LoadStores(false);   // TT#1154 - MD - Jellis - Group Allocation Style Review No Stores 
                }  // TT#946 - MD - Jellis - Group Allocation Not Working
                foreach (AllocationProfile ap in apList)
                {
                    if (!ap.StoresLoaded)
                    {
                        ap.LoadStores(false);   // TT#1154 - MD - Jellis - Group Allocation Style Review No Stores 
                    }
                }
                // begin TT#891 - MD - JEllis - Group Allocation Need gets error
                foreach (AllocationProfile ap in AssortmentPlaceHolders) // TT#995 - MD - Jellis - Group Allocation Infrastructure built wrong
                {
                    if (!ap.StoresLoaded)
                    {
                        ap.LoadStores(false);   // TT#1154 - MD - Jellis - Group Allocation Style Review No Stores 
                    }
                }
                // end TT#891 - MD - Jellis - Group Allocation NEED gets error
            //} // TT#946 - MD - Jellis - Group Allocation Not Working
            //try
            //{
                ResetTempLocks(false);
                // begin TT#946 - MD - Jellis - Group Allocation Not Working
       
                Index_RID[] storeIndexRIDs = AppSessionTransaction.StoreIndexRIDArray(); 
                Hashtable bulkColors = BulkColors;
                Hashtable packs = Packs;  // TT#1008 - MD - Jellis - Get Non Negative Message for various variables

                //foreach (Index_RID storeIdxRID in storeIndexRIDs)
                //{
                //    base.SetStoreQtyAllocated(eAllocationSummaryNode.Bulk, storeIdxRID, 0, eDistributeChange.ToNone, false, false, false);
                //    base.SetStoreItemQtyAllocated(eAllocationSummaryNode.Bulk, storeIdxRID, 0, false);
                //    base.SetStoreQtyAllocated(eAllocationSummaryNode.DetailType, storeIdxRID, 0, eDistributeChange.ToNone, false, false, false);
                //    base.SetStoreItemQtyAllocated(eAllocationSummaryNode.DetailType, storeIdxRID, 0, false);
                //    base.SetStoreQtyAllocated(eAllocationSummaryNode.Total, storeIdxRID, 0, eDistributeChange.ToNone, false, false, false);
                //    base.SetStoreItemQtyAllocated(eAllocationSummaryNode.Total, storeIdxRID, 0, false);
                //}
                // end TT#946 - MD - Jellis - Group Allocation Not Working
                base.ReserveUnits = 0;
                base.AllocatedUnits = 0;
                base.OrigAllocatedUnits = 0;
                base.RsvAllocatedUnits = 0;
                _memberColorReceiptsBalanceToBulk = 0;
                _memberSizeReceiptsBalanceToColor = 0;
                _memberBulkSizeIntransitUpdated = 0;
                _memberBulkColorIntransitUpdated = 0;
                _memberStyleIntransitUpdated = 0;
                _memberBottomUpSizePerformed = 0;
                _memberRulesDefinedAndProcessed = 0;
                _memberNeedAllocationPerformed = 0;
                _memberPackBreakoutByContent = 0;
                _memberBulkSizeBreakoutPerformed = 0; 
                // begin TT#1064 - MD - Jellis - Cannot release Group ALlocation
                _memberReleaseApproved = 0;
                _memberReleased = 0;
                _memberShippingStarted = 0;
                _memberShippingComplete = 0;
                _memberShippingOnHold = 0;
                // end TT#1064 - MD - Jellis - Cannot release Group ALlocation
                base.UnitsShipped = 0;
                // begin TT#897 - MD - Jellis - Null Reference (and Qty cannot be negative)
                // Changed order of initialization from top-down to bottom - up (bulk to detail to generi to total)
                base.BulkUnitsToAllocate = 0;
                base.BulkUnitsAllocated = 0;
                base.BulkItemUnitsAllocated = 0; // TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                base.DetailTypeUnitsToAllocate = 0;
                base.DetailTypeUnitsAllocated = 0;
                base.DetailTypeItemUnitsAllocated = 0; // TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                base.GenericUnitsToAllocate = 0;
                base.GenericUnitsAllocated = 0;
                base.GenericItemUnitsAllocated = 0; // TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                base.TotalUnitsToAllocate = 0;
                base.TotalUnitsAllocated = 0;
                base.TotalItemUnitsAllocated = 0; // TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                // end TT#897 - MD - Jellis - Null Reference (and Qty cannot be negative)
                base.StoreStyleManualAllocationTotal = 0;
                base.StoreSizeManualAllocationTotal = 0;
                base.StoreStyleAllocationManuallyChangedCount = 0;
                //Index_RID[] storeIndexRIDs = AppSessionTransaction.StoreIndexRIDArray();   // TT#946 - MD - Jellis - Group Allocation Not Working
                bool[] styleManualChange = new bool[storeIndexRIDs.Length];
                bool[] sizeManualChange = new bool[storeIndexRIDs.Length];
                styleManualChange.Initialize();
                sizeManualChange.Initialize();
                _allMembersHaveSameCapacityNode = true;  // TT#1148 - MD - Jellis - Group ALlocation enforces capacity on wrong header
                bool[] excludeStoreInGroup = new bool[storeIndexRIDs.Length];  // TT#3797 - UR - Jellis - Style Review Blank for Groups
                excludeStoreInGroup.Initialize();                              // TT#3797 - UR - Jellis - Style Review Blank for Groups
                foreach (AllocationProfile ap in apList)
                {
                    base.ReserveUnits += ap.ReserveUnits;
                    base.AllocatedUnits += ap.AllocatedUnits;
                    base.OrigAllocatedUnits += ap.OrigAllocatedUnits;
                    base.RsvAllocatedUnits += ap.RsvAllocatedUnits;
                    if (ap.BottomUpSizePerformed)
                    {
                        _memberBottomUpSizePerformed += 1;
                    }
                    if (ap.RulesDefinedAndProcessed)
                    {
                        _memberRulesDefinedAndProcessed += 1;
                    }
                    if (ap.NeedAllocationPerformed)
                    {
                        _memberNeedAllocationPerformed += 1;
                    }
                    if (ap.PackBreakoutByContent)
                    {
                        _memberPackBreakoutByContent += 1;
                    }
                    if (ap.BulkSizeBreakoutPerformed)
                    {
                        _memberBulkSizeBreakoutPerformed += 1;
                    }
                    if (ap.SizeReceiptsBalanceToColor)
                    {
                        _memberSizeReceiptsBalanceToColor += 1;
                    }
                    if (ap.ColorReceiptsBalanceToBulk)
                    {
                        _memberColorReceiptsBalanceToBulk += 1;
                    }
                    if (ap.StyleIntransitUpdated)
                    {
                        _memberStyleIntransitUpdated += 1;
                    }
                    if (ap.BulkColorIntransitUpdated)
                    {
                        _memberBulkColorIntransitUpdated += 1;
                    }
                    if (ap.BulkSizeIntransitUpdated)
                    {
                        _memberBulkSizeIntransitUpdated += 1;
                    }
                    
                    // begin TT#1064 - MD - Jellis - Cannot Release Group Allocation
                    if (ap.ReleaseApproved)
                    {
                        _memberReleaseApproved += 1;
                    }
                    if (ap.Released)
                    {
                        _memberReleased += 1;
                    }
                    if (ap.ShippingStarted)
                    {
                        _memberShippingStarted += 1;
                    }
                    if (ap.ShippingComplete)
                    {
                        _memberShippingComplete += 1;
                    }
                    if (ap.ShippingOnHold)
                    {
                        _memberShippingOnHold += 1;
                    }
                    // end TT#1064 - MD - Jellis - Cannot Release Group ALlocation
                    // begin TT#1148 - MD - Jellis- Group Allocation Enforces capacity on wrong header
                    if (base.CapacityNodeRID != ap.CapacityNodeRID)
                    {
                        _allMembersHaveSameCapacityNode = false;
                    }
                    // end TT#1148 - MD - Jellis- Group Allocation Enforces capacity on wrong header
                    base.TotalUnitsToAllocate += ap.TotalUnitsToAllocate;
                    base.TotalUnitsAllocated += ap.TotalUnitsAllocated;
                    base.UnitsShipped += ap.UnitsShipped;
                    base.GenericUnitsToAllocate += ap.GenericUnitsToAllocate;
                    base.GenericUnitsAllocated += ap.GenericUnitsAllocated;
                    base.DetailTypeUnitsToAllocate += ap.DetailTypeUnitsToAllocate;
                    base.DetailTypeUnitsAllocated += ap.DetailTypeUnitsAllocated;  // TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                    base.BulkUnitsToAllocate += ap.BulkUnitsToAllocate;
                    base.BulkUnitsAllocated += ap.BulkUnitsAllocated;
                    base.StoreStyleManualAllocationTotal += ap.StoreStyleManualAllocationTotal;
                    base.StoreSizeManualAllocationTotal += ap.StoreSizeManualAllocationTotal;
                    foreach (Index_RID storeIdxRID in storeIndexRIDs)
                    {
                        // begin TT#946 - MD - Jellis - Group Allocation Not Working
                        //base.SetStoreQtyAllocated(eAllocationSummaryNode.Total, storeIdxRID, ap.GetStoreQtyAllocated(eAllocationSummaryNode.Total, storeIdxRID), eDistributeChange.ToNone, false, false, false);
                        //base.SetStoreQtyAllocated(eAllocationSummaryNode.DetailType, storeIdxRID, ap.GetStoreQtyAllocated(eAllocationSummaryNode.DetailType, storeIdxRID), eDistributeChange.ToNone, false, false, false);
                        //base.SetStoreQtyAllocated(eAllocationSummaryNode.Bulk, storeIdxRID, ap.GetStoreQtyAllocated(eAllocationSummaryNode.Bulk, storeIdxRID), eDistributeChange.ToNone, false, false, false);
                        //foreach (HdrColorBin hcb in ap.BulkColors.Values)
                        //{
                        //    HdrColorBin colorBin = (HdrColorBin)bulkColors[hcb.ColorCodeRID];
                        //    if (colorBin == null)
                        //    {
                        //        throw new MIDException(eErrorLevel.severe,
                        //            (int)eMIDTextCode.msg_ColorNotDefinedInBulk,
                        //            MIDText.GetText(eMIDTextCode.msg_ColorNotDefinedInBulk) + " HeaderID=[)" + HeaderID + "] ColorRID=[" + hcb.ColorCodeRID.ToString() + "]");

                        //    }
                        //    base.SetStoreQtyAllocated(colorBin, storeIdxRID, hcb.GetStoreUnitsAllocated(storeIdxRID.Index), eDistributeChange.ToNone, false, false, false);
                        //    base.SetStoreItemQtyAllocated(colorBin, storeIdxRID, hcb.GetStoreItemUnitsAllocated(storeIdxRID.Index), false);
                        //    foreach (HdrSizeBin hsb in hcb.ColorSizes.Values)
                        //    {
                        //        HdrSizeBin sizeBin = (HdrSizeBin)colorBin.ColorSizes[hsb.SizeCodeRID];
                        //        if (sizeBin == null)
                        //        {
                        //            throw new MIDException(eErrorLevel.severe,
                        //                (int)eMIDTextCode.msg_SizeNotDefinedInBulkColor,
                        //                MIDText.GetText(eMIDTextCode.msg_SizeNotDefinedInBulkColor) + " HeaderID=[)" + HeaderID + "] ColorRID=[" + hcb.ColorCodeRID.ToString() + "] SizeRID=" + hsb.SizeCodeRID + "]");
                        //        }
                        //        base.SetStoreQtyAllocated(sizeBin, storeIdxRID, hsb.GetStoreSizeUnitsAllocated(storeIdxRID.Index), eDistributeChange.ToNone, false, false, false);
                        //        base.SetStoreItemQtyAllocated(sizeBin, storeIdxRID, hsb.GetStoreSizeItemUnitsAllocated(storeIdxRID.Index), false);
                        //    }
                        //}
                        // end TT#946 - MD - Jellis - Group Allocation Not Working
                        styleManualChange[storeIdxRID.Index] =
                            (styleManualChange[storeIdxRID.Index]
                              || ap.GetStoreStyleAllocationIsManuallyAllocated(storeIdxRID));
                        sizeManualChange[storeIdxRID.Index] =
                            (sizeManualChange[storeIdxRID.Index]
                             || ap.GetStoreSizeAllocationIsManuallyAllocated(storeIdxRID));
                        // begin TT#3797 - UR - Jellis - Style Review Blank for Groups
                        if (ap.GetIncludeStoreInAllocation(storeIdxRID)
                            || excludeStoreInGroup[storeIdxRID.Index] == false)
                        {
                            base.SetIncludeStoreInAllocation(storeIdxRID, true);
                            excludeStoreInGroup[storeIdxRID.Index] = false;
                        }
                        else
                        {
                            base.SetIncludeStoreInAllocation(storeIdxRID, false);
                        }
                        // end TT#3797 - UR - Jellis - Style Review Blank for Groups
                    }
                }
                for (int i = 0; i < storeIndexRIDs.Length; i++)
                {
                    if (styleManualChange[i])
                    {
                        base.StoreStyleAllocationManuallyChangedCount++;
                    }
                    if (sizeManualChange[i])
                    {
                        base.StoreSizeAllocationManuallyChangedCount++;
                    }
                }


                BuildAssortmentProperties = false;
            }
            finally
            {
                _buildingAssortmentProperties = false;  // TT#897 - MD- Jellis - Null Reference
                ResetTempLocks(true);
            }
        }

        public void AssortmentPropertiesRebuildItemVsw()
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Gets Error
            base.ItemAllocatedUnits = 0;
            base.TotalItemUnitsAllocated = 0;
            base.TotalItemOrigUnitsAllocated = 0;
            base.GenericItemUnitsAllocated = 0;
            base.DetailTypeItemUnitsAllocated = 0;
            base.BulkItemUnitsAllocated = 0;

            foreach (AllocationProfile ap in apList)
            {
                base.ItemAllocatedUnits += ap.ItemAllocatedUnits;
                base.TotalItemUnitsAllocated += ap.TotalItemUnitsAllocated;
                base.TotalItemOrigUnitsAllocated += ap.TotalItemOrigUnitsAllocated;
                base.GenericItemUnitsAllocated += ap.GenericItemUnitsAllocated;
                base.DetailTypeItemUnitsAllocated += ap.DetailTypeItemUnitsAllocated;
                base.BulkItemUnitsAllocated += ap.BulkItemUnitsAllocated;
            }

            AppSessionTransaction.SetCalcAssortmentItemVsw(BuildItemKey, false);
        }
        /// <summary>
        /// Gets the list of Allocation Profiles containing the specified color
        /// </summary>
        /// <param name="aColorRID">RID identifying the color</param>
        /// <returns></returns>
        public List<AllocationProfile> GetHeadersWithColor(int aColorRID)
        {
            AllocationProfile[] apList = AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Gets Error
            List<AllocationProfile> apListWithColors = new List<AllocationProfile>();
            foreach (AllocationProfile ap in apList)
            {
                if (ap.BulkColors.ContainsKey(aColorRID))
                {
                    apListWithColors.Add(ap);
                }
            }
            return apListWithColors;
        }

        // begin TT#1008 - MD - Jellis - Get Non Negative Message for various variables
        internal AllocationProfile GetAssortmentPackHome(int aPackRID)
        {
            if (_assortmentPackHome == null)
            {
                _assortmentPackHome = new Dictionary<int, AllocationProfile>();
            }
            if (!_assortmentPacksLoaded)
            {
                AllocationProfile[] apList = AssortmentMembers;
                foreach (AllocationProfile ap in apList)
                {
                    foreach (PackHdr ph in ap.Packs.Values)
                    {
                        _assortmentPackHome.Add(ph.PackRID, ap);
                    }
                }
                _assortmentPacksLoaded = true;
            }
            AllocationProfile allocationProfile;
            _assortmentPackHome.TryGetValue(aPackRID, out allocationProfile);
            return allocationProfile;
        }
        // end TT#1008 - MD - Jellis - Get Non Negative Message for various variables

        // Begin TT#4811 - JSmith - Choose to not save changes in style review but changes save anyway
        /// <summary>
        /// ReReads the allocation header from the database
        /// </summary>
        /// <remarks>This method should be performed after a cross-session update has occurred.</remarks>
        override public void ReReadHeader()
        {
			// Begin TT#4811 - stodd - Choose to not save changes in style review but changes save anyway
            //SetStoresNotLoaded();
            ReReadHeaderWithStores();
			// End TT#4811 - stodd - Choose to not save changes in style review but changes save anyway
			
            AllocationProfile[] apList = AssortmentMembers;
            foreach (AllocationProfile ap in apList)
            {
				// Begin TT#4811 - stodd - Choose to not save changes in style review but changes save anyway
                //ap.SetStoresNotLoaded();
                ap.ReReadHeaderWithStores();
				// End TT#4811 - stodd - Choose to not save changes in style review but changes save anyway
            }
            foreach (AllocationProfile ap in AssortmentPlaceHolders) 
            {	
				// Begin TT#4811 - stodd - Choose to not save changes in style review but changes save anyway
                //ap.SetStoresNotLoaded();
                ap.ReReadHeaderWithStores();
				// End TT#4811 - stodd - Choose to not save changes in style review but changes save anyway
            }
            AssortmentPropertiesRebuild();
        }
        // End TT#4811 - JSmith - Choose to not save changes in style review but changes save anyway
        #endregion Allocation Special Methods
        // end TT#488 - MD - Jellis - Group Allocation

        #region Fill
		// BEGIN TT#1183 - stodd - assortment
		private void Fill()
		{
			Fill(true);
		}
		// End TT#1183 - stodd - assortment

        private void Fill(bool buildSummary)
		{
            try
            {
                _assortmentLoaded = false; // TT#488 - MD - Jellis - Group ALlocation // TT#888 - MD - Jellis - Assortment/Group members not populated
                _assortmentPacksLoaded = false; // TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                _processingRules = false;  // TT#1117 - MD - Jellis - Units Allocated less than minimum
                _basisList = new List<AssortmentBasis>();
                _assortApplyToDay = null;	// TT#1224 - stodd - committed
                _assortBeginDay = null;   // TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working

                _nodeAncestorList = new Hashtable();   // TT#1489 - RMatelic - Add Hierarchy levels above style as default displayable items
                //_assortmentMembers = new List<AllocationProfile>();             // TT#488 - MD - Jellis - Group Allocation
                _assortmentMembers = null;                                        // TT#488 - MD - Jellis - Group Allocation Member List not populated
                _assortmentPackHome = null;                                       // TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                _placeHolders = null;                                             // TT#891 - MD - Jellis - Group Allocation NEED Action Gets error
                _buildAssortmentProperties = true; // TT#488 - MD - Jellis - Group Allocation
                _buildingAssortmentProperties = false; // TT#897 - MD- Jellis - Null Reference
                _suspendAssortmentUpdates = false; // TT#488 - MD - Jellis - Group Allocation
                _memberColorReceiptsBalanceToBulk = 0;  // TT#488 - MD - Jellis - Group Allocation
                _memberSizeReceiptsBalanceToColor = 0; // TT#488 - MD - Jellis - Group Allocation
                _memberBulkSizeIntransitUpdated = 0;   // TT#488 - MD - Jellis - Group Allocation
                _memberBulkColorIntransitUpdated = 0;  // TT#488 - Md - Jellis - Group Allocation
                _memberBottomUpSizePerformed = 0;     // TT#488 - MD - Jellis - Group Allocation
                _memberRulesDefinedAndProcessed = 0;  // TT#488 - MD - Jellis - Group Allocation
                _memberNeedAllocationPerformed = 0;   // TT#488 - MD - Jellis - Group Allocation
                _memberPackBreakoutByContent = 0;     // TT#488 - MD - Jellis - Group Allocation
                _memberBulkSizeBreakoutPerformed = 0; // TT#488 - MD - Jellis - Group Allocation
                // begin TT#1064 - MD - Jellis - Cannot Release Group Allocation
                _memberReleaseApproved = 0;
                _memberReleased = 0;
                _memberShippingStarted = 0;
                _memberShippingComplete = 0;
                _memberShippingOnHold = 0;
                // end TT#1064 - MD - Jellis - Cannot Release Group Allocation
                _memberAllocationMultiple = -1;           // TT#488 - MD - Jellis - Group Allocation
                _memberGenericMultiple = -1;              // TT#488 - MD - Jellis - Group Allocation
                _memberDetailTypeMultiple = -1;           // TT#488 - MD - JEllis - Group Allocation
                _memberBulkMultiple = -1;                 // TT#488 - MD - Jellis - Group Allocation
                _allMembersHaveSameCapacityNode = false;  // TT#1148 - MD - Jellis - Group Allocation enforces capacity on wrong header
                _buildInventoryBasisAllocation = true;    // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                _buildPriorItemUnitsManuallyAllocated = true; // TT#3941 - Urban - Jellis - Group Allocation Item/VSW split not correct
                _processingGroupAllocation = false;       // TT#488 - MD - Jellis - Group Allocation
                _processingActionOnHeaderInGroup = false; // TT#1064 - MD - Jellis - Cannot Release Group Allocation
                _placeholderAction = false;		// TT#1006 - md - stodd - GA Screen and Allocation WS after run Size Need status' are out of sync- 
                _assortStoreGroupRid = Include.AllStoreGroupRID;	// Begin TT#952 - MD - Add Matrix to Group Allocation - 	

                if (this._key == Include.NoRID)
                {
                    //_storeFilterRid = Include.UndefinedStoreFilter;	// TT#2 - stodd - assortment
                    _assortReserveAmount = Include.UndefinedReserve;
                    _assortReserveType = eReserveType.Unknown;
                    // Begin TT#5124 - JSmith - Performance
                    //GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
                    //gop.LoadOptions();
                    //_assortStoreGroupRid = gop.OTSPlanStoreGroupRID;
                    _assortStoreGroupRid = SAB.ApplicationServerSession.GlobalOptions.AllocationStoreGroupRID;
                    // End TT#5124 - JSmith - Performance
                    _assortVariableType = eAssortmentVariableType.None;
                    _assortVariableNumber = Include.Undefined;
                    // Begin TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
                    //_assortInclOnhand = false;
                    //_assortInclIntransit = false;
                    _assortInclOnhand = true;
                    _assortInclIntransit = true;
                    // End TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
                    _assortInclSimStores = false;
                    _assortInclCommitted = false;	// TT#2 - stodd - assortment
                    _assortAverageBy = eStoreAverageBy.None;
                    //_genAssortMethodRid = Include.NoRID;	// TT#2 - stodd - assortment
                    _assortGradeBoundary = eGradeBoundary.Unknown;
                    _assortCdrRid = Include.UndefinedCalendarDateRange;	// TT#2 - stodd - assortment
                    _assortBeginDayCdrRid = Include.UndefinedCalendarDateRange;   // TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
                    base.HeaderDay = DateTime.Now;
                    //base.HeaderType = eHeaderType.Assortment;
                    base.AsrtType = (int)eAssortmentType.PreReceipt;
                }
                else
                {
                    int asrtRID;
                    if (this.HeaderType == eHeaderType.Assortment)
                    {
                        asrtRID = this.Key;
                    }
                    else
                    {
                        asrtRID = this.AsrtRID;
                    }

                    // BEGIN TT#488-MD - Stodd - Group Allocation
                    if (this.AsrtType == (int)eAssortmentType.GroupAllocation)
                    {
						// Begin TT#952 - MD - stodd - add matrix to Group Allocation Review
                        SetupSummaryfromGroupAllocation(AppSessionTransaction);
                        if (buildSummary)
                        {
                            BuildAssortmentSummary();
                        }
						// End TT#952 - MD - stodd - add matrix to Group Allocation Review
                    }
                    else
                    {
                        SetupSummaryfromAssortment(asrtRID);	// TT#952 - MD - stodd - add matrix to Group Allocation Review
                        // BEGIN TT#1183 - stodd - assortment
                        // Don't bother building the Summary during startup.
                        if (buildSummary)
                        {
                            BuildAssortmentSummary();
                        }
                        // END TT#1183 - stodd - assortment
                    }
                    // END TT#488-MD - Stodd - Group Allocation
                }
                _assortmentLoaded = true; // TT#888 - MD - Jellis - Assortment/Group members not populated
            }
            catch
            {
                // failed to fill
                throw;
            }
            // begin TT#488 - MD - Jellis - Group Allocation
            finally
            {
                //_assortmentLoad = false;  // TT#888 - MD - Jellis - Assortment/Group members not populated
            }
            // end TT#488 - MD - Jellis - Group Alocation
		}

		private void FillAssortHeader(int aAsrtRID)
		{
			try
			{
				//===================
				// Header Assortment
				//===================
                // Begin TT#2 - Ron Matelic - assortment
                //DataTable dtAssortHeader = HeaderDataRecord.GetAssortmentHeader(this._key);
                DataTable dtAssortHeader = HeaderDataRecord.GetAssortmentProperties(aAsrtRID);
                // End TT#2 - Ron Matelic - assortment
				// Begin TT#2 - stodd - assortment
				if (dtAssortHeader.Rows.Count > 0)
				{
					//_storeFilterRid = Convert.ToInt32(dtAssortHeader.Rows[0]["STORE_FILTER_RID"], CultureInfo.CurrentUICulture);
					_assortReserveAmount = Convert.ToDouble(dtAssortHeader.Rows[0]["RESERVE"], CultureInfo.CurrentUICulture);
					_assortReserveType = (eReserveType)Convert.ToInt32(dtAssortHeader.Rows[0]["RESERVE_TYPE_IND"], CultureInfo.CurrentUICulture);
					_assortStoreGroupRid = Convert.ToInt32(dtAssortHeader.Rows[0]["SG_RID"], CultureInfo.CurrentUICulture);
					_assortVariableType = (eAssortmentVariableType)Convert.ToInt32(dtAssortHeader.Rows[0]["VARIABLE_TYPE"], CultureInfo.CurrentUICulture);
					_assortVariableNumber = Convert.ToInt32(dtAssortHeader.Rows[0]["VARIABLE_NUMBER"], CultureInfo.CurrentUICulture);
                    // Begin TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
                    //_assortInclOnhand = Include.ConvertCharToBool(Convert.ToChar(dtAssortHeader.Rows[0]["INCL_ONHAND"], CultureInfo.CurrentUICulture));
                    //_assortInclIntransit = Include.ConvertCharToBool(Convert.ToChar(dtAssortHeader.Rows[0]["INCL_INTRANSIT"], CultureInfo.CurrentUICulture));
                    _assortInclOnhand = true;
                    _assortInclIntransit = true;
                    // End TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
					_assortInclSimStores = Include.ConvertCharToBool(Convert.ToChar(dtAssortHeader.Rows[0]["INCL_SIMILAR_STORES"], CultureInfo.CurrentUICulture));
					_assortInclCommitted = Include.ConvertCharToBool(Convert.ToChar(dtAssortHeader.Rows[0]["INCL_COMMITTED"], CultureInfo.CurrentUICulture));
					_assortAverageBy = (eStoreAverageBy)Convert.ToInt32(dtAssortHeader.Rows[0]["AVERAGE_BY"], CultureInfo.CurrentUICulture);
					//_genAssortMethodRid = Convert.ToInt32(dtAssortHeader.Rows[0]["HDR_RID"], CultureInfo.CurrentUICulture);
					_assortGradeBoundary = (eGradeBoundary)Convert.ToInt32(dtAssortHeader.Rows[0]["GRADE_BOUNDARY_IND"], CultureInfo.CurrentUICulture);
					_assortCdrRid = Convert.ToInt32(dtAssortHeader.Rows[0]["CDR_RID"], CultureInfo.CurrentUICulture);
                    _assortBeginDayCdrRid = Convert.ToInt32(dtAssortHeader.Rows[0]["BEGIN_DAY_CDR_RID"], CultureInfo.CurrentUICulture);  // TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
					_assortAnchorNodeRid = Convert.ToInt32(dtAssortHeader.Rows[0]["ANCHOR_HN_RID"], CultureInfo.CurrentUICulture);
					_assortUserRid = Convert.ToInt32(dtAssortHeader.Rows[0]["USER_RID"], CultureInfo.CurrentUICulture);
					_assortLastProcessDt = Convert.ToDateTime(dtAssortHeader.Rows[0]["LAST_PROCESS_DATETIME"], CultureInfo.CurrentUICulture);
				}
				else
				{
					//_storeFilterRid = Include.UndefinedStoreFilter;	// TT#2 - stodd - assortment
					_assortReserveAmount = 0d;
					_assortReserveType = eReserveType.Unknown;
                    // Begin TT#5124 - JSmith - Performance
                    //GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
                    //gop.LoadOptions();
                    //_assortStoreGroupRid = gop.OTSPlanStoreGroupRID;
                    _assortStoreGroupRid = SAB.ApplicationServerSession.GlobalOptions.AllocationStoreGroupRID;
                    // End TT#5124 - JSmith - Performance
					_assortVariableType = eAssortmentVariableType.None;
					_assortVariableNumber = Include.Undefined;
                    // Begin TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
                    //_assortInclOnhand = false;
                    //_assortInclIntransit = false;
                    _assortInclOnhand = true;
                    _assortInclIntransit = true;
                    // Begin TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
					_assortInclSimStores = false;
					_assortInclCommitted = false;	// TT#2 - stodd - assortment
					_assortAverageBy = eStoreAverageBy.None;
					//_genAssortMethodRid = Include.NoRID;	// TT#2 - stodd - assortment
					_assortGradeBoundary = eGradeBoundary.Unknown;
					_assortCdrRid = Include.UndefinedCalendarDateRange;	// TT#2 - stodd - assortment
                    _assortBeginDayCdrRid = Include.UndefinedCalendarDateRange;  // TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
					_assortAnchorNodeRid = Include.NoRID;
				}
				// End TT#2 - stodd - assortment
			}
			catch
			{
				throw;
			}
		}

        private void FillAssortBasis(int aAsrtRID)
		{
			try
			{
				//==========================
				// Header Assortment Basis
				//==========================
				//DataTable dtAssortBasis = HeaderDataRecord.GetAssortmentBasis(this._key);
                DataTable dtAssortBasis = HeaderDataRecord.GetAssortmentPropertiesBasis(aAsrtRID);
				foreach (DataRow aRow in dtAssortBasis.Rows)
				{
					int hierNodeRid = Convert.ToInt32(aRow["HN_RID"], CultureInfo.CurrentUICulture);
					int versionRid = Convert.ToInt32(aRow["FV_RID"], CultureInfo.CurrentUICulture);
					int dateRangeRid = Convert.ToInt32(aRow["CDR_RID"], CultureInfo.CurrentUICulture);
					float weight = (float)Convert.ToDouble(aRow["WEIGHT"], CultureInfo.CurrentUICulture);

                    AssortmentBasis ab = new AssortmentBasis(SAB, AppSessionTransaction, hierNodeRid, versionRid, dateRangeRid, weight, AssortmentApplyToDate); // TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 
					_basisList.Add(ab);
				}
			}
			catch
			{
				throw;
			}
		}

        private void FillAssortGrades(int aAsrtRID)
		{
            DataTable dtAssortGrades; //TT#2505 - DOConnell - Placeholder values changing when combining assortments
			try
			{
				//================================
				// Header Assortment Store Gradesv
				//================================
				//DataTable dtAssortGrades = HeaderDataRecord.GetAssortmentGrades(this._key);
				
				//BEGIN TT#2505 - DOConnell - Placeholder values changing when combining assortments
                //DataTable dtAssortGrades = HeaderDataRecord.GetAssortmentPropertiesStoreGrades(aAsrtRID);
                // begin TT#488 - MD - Jellis - Group Allocation
                //if (_transaction != null)
                //{
                //    if (_transaction.AssortmentViewLoadedFrom == eAssortmentBasisLoadedFrom.UserSelectionCriteria)
                //    {
                //        dtAssortGrades = _transaction.AssortmentViewSelectionCriteria.StoreGradeDataTable;
                //    }
                //    else
                if (AppSessionTransaction != null)
                {
                    if (AppSessionTransaction.AssortmentViewLoadedFrom == eAssortmentBasisLoadedFrom.UserSelectionCriteria)
                    {
                        dtAssortGrades = AppSessionTransaction.AssortmentViewSelectionCriteria.StoreGradeDataTable;
                    }
                    else
                // end TT#488 - MD - Jellis - Group Allocation
                    {
                        dtAssortGrades = HeaderDataRecord.GetAssortmentPropertiesStoreGrades(aAsrtRID);
                    }
                }
                else
                {
                    dtAssortGrades = HeaderDataRecord.GetAssortmentPropertiesStoreGrades(aAsrtRID);
                }
				//END TT#2505 - DOConnell - Placeholder values changing when combining assortments
				// Begin TT#2 - stodd - assortment
				foreach (DataRow aRow in dtAssortGrades.Rows)
				{
					int seq = Convert.ToInt32(aRow["STORE_GRADE_SEQ"], CultureInfo.CurrentUICulture);
					int boundary = Convert.ToInt32(aRow["BOUNDARY_INDEX"], CultureInfo.CurrentUICulture);
					int boundaryUnits = Convert.ToInt32(aRow["BOUNDARY_UNITS"], CultureInfo.CurrentUICulture);
					string gradeCode = aRow["GRADE_CODE"].ToString().Trim();
					StoreGradeProfile sgp = new StoreGradeProfile(boundary);
					sgp.Boundary = boundary;
					sgp.StoreGrade = gradeCode;
					sgp.BoundaryUnits = boundaryUnits;
					AssortmentStoreGradeList.Add(sgp);	// TT#488-MD - STodd - Group Allocation 
				}
				// End TT#2 - stodd - assortment
			}
			catch
			{
				throw;
			}
		}

		// BEGIN TT#1876 - stodd - summary incorrect when coming from selection window
		private void FillAssortHeaderFromSelection(ApplicationSessionTransaction trans)
		{
			try
			{
				//===================
				// Header Assortment
				//===================
				// Begin TT#952 - MD - Add Matrix to Group Allocation - 
                if (HeaderType == eHeaderType.Assortment && AsrtType == (int)eAssortmentType.GroupAllocation)
                {
                    _assortAnchorNodeRid = StyleHnRID;
                    _assortCdrRid = (SAB.ApplicationServerSession.Calendar.AddDateRangeFromWeeks(SAB.ApplicationServerSession.Calendar.CurrentWeek.YearWeek, SAB.ApplicationServerSession.Calendar.CurrentWeek.YearWeek)).Key;
                    _assortStoreGroupRid = Include.AllStoreGroupRID;
                    //ProfileList variables = SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList;
					// Begin TT#952 - MD - stodd - add matrix to Group Allocation Review
                    _assortVariableType = eAssortmentVariableType.Sales;

                    bool nullAsp = false;
                    if (_assortmentSummaryProfile == null)
                    {
                        _assortmentSummaryProfile = new Business.AssortmentSummaryProfile(this, SAB, trans);
                        nullAsp = true;
                    }
                    _assortVariableNumber = _assortmentSummaryProfile.GetVariableNumber(_assortVariableType);
                    if (nullAsp)
                    {
                        _assortmentSummaryProfile = null;
                    }
					// End TT#952 - MD - stodd - add matrix to Group Allocation Review

                    // Begin TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
                    _assortInclOnhand = false;
                    _assortInclIntransit = false;
                    // End TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
                    //_assortInclSimStores = false;
                    //_assortInclCommitted = false;
                    _assortInclSimStores = true;
                    _assortInclCommitted = true;
                    _assortAverageBy = eStoreAverageBy.AllStores;
                    _assortGradeBoundary = eGradeBoundary.Index;
                }
                else
                {
				// End TT#952 - MD - Add Matrix to Group Allocation - 
                    _assortStoreGroupRid = trans.AssortmentViewSelectionCriteria.StoreGroupRID;
                    _assortVariableType = trans.AssortmentViewSelectionCriteria.VariableType;
                    _assortVariableNumber = trans.AssortmentViewSelectionCriteria.VariableNumber;
                    // Begin TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
                    //_assortInclOnhand = trans.AssortmentViewSelectionCriteria.IncludeOnhand;
                    //_assortInclIntransit = trans.AssortmentViewSelectionCriteria.IncludeIntransit;
                    _assortInclOnhand = true;
                    _assortInclIntransit = true;
                    // End TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
                    _assortInclSimStores = trans.AssortmentViewSelectionCriteria.IncludeSimStore;
                    _assortInclCommitted = trans.AssortmentViewSelectionCriteria.IncludeCommitted;
                    _assortAverageBy = trans.AssortmentViewSelectionCriteria.AverageBy;
                    _assortGradeBoundary = trans.AssortmentViewSelectionCriteria.GradeBoundary;
                }
			}
			catch
			{
				throw;
			}
		}

		private void FillAssortBasisFromSelection(ApplicationSessionTransaction trans)
		{
			try
			{
				//==========================
				// Header Assortment Basis
				//==========================
				_basisList.Clear();
				// Begin TT#952 - MD - Add Matrix to Group Allocation - 
                if (HeaderType == eHeaderType.Assortment && AsrtType == (int)eAssortmentType.GroupAllocation)
                {
					// Begin TT#952 - MD - stodd - add matrix to Group Allocation Review
                    DayProfile aDay = SAB.ApplicationServerSession.Calendar.GetDay(ShipToDay);
                    int aCdrRid = (SAB.ApplicationServerSession.Calendar.AddDateRangeFromWeeks(SAB.ApplicationServerSession.Calendar.CurrentWeek.YearWeek, aDay.Week.YearWeek)).Key;

                    AssortmentBasis ab = new AssortmentBasis(SAB, AppSessionTransaction, PlanLevelStartHnRID, Include.FV_ActionRID, aCdrRid, 100, AssortmentApplyToDate);	// TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 
                    Debug.WriteLine(PlanLevelStartHnRID.ToString() + " " + "Action " + SAB.ApplicationServerSession.Calendar.CurrentWeek.YearWeek + " thru " + aDay.Week.YearWeek);                  
					// End TT#952 - MD - stodd - add matrix to Group Allocation Review
                    _basisList.Add(ab);
                }
                else
                {
				// End TT#952 - MD - Add Matrix to Group Allocation - 
                    DataTable dtAssortBasis = trans.AssortmentViewSelectionCriteria.BasisDataTable;
                    foreach (DataRow aRow in dtAssortBasis.Rows)
                    {
                        int hierNodeRid = Convert.ToInt32(aRow["HN_RID"], CultureInfo.CurrentUICulture);
                        int versionRid = Convert.ToInt32(aRow["FV_RID"], CultureInfo.CurrentUICulture);
                        int dateRangeRid = Convert.ToInt32(aRow["CDR_RID"], CultureInfo.CurrentUICulture);
                        float weight = (float)Convert.ToDouble(aRow["WEIGHT"], CultureInfo.CurrentUICulture);

                        AssortmentBasis ab = new AssortmentBasis(SAB, AppSessionTransaction, hierNodeRid, versionRid, dateRangeRid, weight, null);	// TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 
                        _basisList.Add(ab);
                    }
                }
			}
			catch
			{
				throw;
			}
		}

		private void FillAssortGradesFromSelection(ApplicationSessionTransaction trans)
		{
			try
			{
				//================================
				// Header Assortment Store Gradesv
				//================================
				AssortmentStoreGradeList.Clear();	// TT#488-MD - STodd - Group Allocation 
				// Begin TT#952 - MD - Add Matrix to Group Allocation - 
                if (HeaderType == eHeaderType.Assortment && AsrtType == (int)eAssortmentType.GroupAllocation)
                {
                    StoreGradeProfile sgp = new StoreGradeProfile(0);
                    sgp.Boundary = 0;
                    sgp.StoreGrade = "V";
                    sgp.BoundaryUnits = 0;
                    AssortmentStoreGradeList.Add(sgp);	// TT#488-MD - STodd - Group Allocation 
                }
                else
                {
				// End TT#952 - MD - Add Matrix to Group Allocation - 
                    DataTable dtAssortGrades = trans.AssortmentViewSelectionCriteria.StoreGradeDataTable;
                    foreach (DataRow aRow in dtAssortGrades.Rows)
                    {
                        int seq = Convert.ToInt32(aRow["STORE_GRADE_SEQ"], CultureInfo.CurrentUICulture);
                        int boundary = Convert.ToInt32(aRow["BOUNDARY_INDEX"], CultureInfo.CurrentUICulture);
                        int boundaryUnits = Convert.ToInt32(aRow["BOUNDARY_UNITS"], CultureInfo.CurrentUICulture);
                        string gradeCode = aRow["GRADE_CODE"].ToString().Trim();
                        StoreGradeProfile sgp = new StoreGradeProfile(boundary);
                        sgp.Boundary = boundary;
                        sgp.StoreGrade = gradeCode;
                        sgp.BoundaryUnits = boundaryUnits;
                        AssortmentStoreGradeList.Add(sgp);	// TT#488-MD - STodd - Group Allocation 
                    }
                }
			}
			catch
			{
				throw;
			}
		}
		// END TT#1876 - stodd - summary incorrect when coming from selection window

		// Begin TT#952 - MD - stodd - add matrix to Group Allocation Review
        private void FillAssortHeaderFromGroupAllocation(ApplicationSessionTransaction trans)
        {
            try
            {
                //===================
                // Header Assortment
                //===================

				// Begin TT#1115 - md - stodd - Sales on Matrix is not matching the plan level - 
                //if (PlanLevelStartHnRID != Include.NoRID)
                if (PlanHnRID != Include.NoRID)
                {
                    _assortAnchorNodeRid = PlanHnRID;
                }
				// End TT#1115 - md - stodd - Sales on Matrix is not matching the plan level - 
                else
                {
                    // Find the highest node in the organizational hierarchy. Use it as a dummy plan level.
                    // This happens when a Group Allocation has no headers yet.
                    HierarchyNodeList hierarchyChildrenList = SAB.HierarchyServerSession.GetRootNodes();
                    foreach (HierarchyNodeProfile hnp in hierarchyChildrenList)
                    {
                        if (hnp.HomeHierarchyType == eHierarchyType.organizational)
                        {
                            _assortAnchorNodeRid = hnp.Key;
                        }
                    }
                }

				// Begin TT#1122 - md - stodd - calendar exception creating group allocation -
				// Begin TT#1127 - md -stodd - invalid calendar date 
                DateTime anchorDay = ShipToDay;
                if (anchorDay == Include.UndefinedDate)
                {
                    anchorDay = SAB.ApplicationServerSession.Calendar.CurrentDate.Date;
                }
				// End TT#1122 - md - stodd - calendar exception creating group allocation - 
				// Begin TT#1119 - md -stodd - summary calculations wrong
                WeekProfile anchorWeek = SAB.ApplicationServerSession.Calendar.GetWeek(anchorDay);
                DateRangeProfile drp = SAB.ApplicationServerSession.Calendar.AddDateRangeFromWeeks(anchorWeek.YearWeek, anchorWeek.YearWeek);
				// End TT#1127 - md -stodd - invalid calendar date 
				// End TT#1119 - md -stodd - summary calculations wrong
                _assortCdrRid = drp.Key;

                _assortStoreGroupRid = Include.AllStoreGroupRID;
                //ProfileList variables = SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList;
                _assortVariableType = eAssortmentVariableType.Sales;

                bool nullAsp = false;
                if (_assortmentSummaryProfile == null)
                {
                    _assortmentSummaryProfile = new Business.AssortmentSummaryProfile(this, SAB, trans);
                    nullAsp = true;
                }
                _assortVariableNumber = _assortmentSummaryProfile.GetVariableNumber(_assortVariableType);
                if (nullAsp)
                {
                    _assortmentSummaryProfile = null;
                }

                // Begin TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
                //_assortInclOnhand = false;
                //_assortInclIntransit = false;
                _assortInclOnhand = true;
                _assortInclIntransit = true;
                // End TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
                _assortInclSimStores = false;
                _assortInclCommitted = false;
                _assortAverageBy = eStoreAverageBy.AllStores;
                _assortGradeBoundary = eGradeBoundary.Index;

            }
            catch
            {
                throw;
            }
        }

        private void FillAssortBasisFromGroupAllocation()
        {
            try
            {
                //==========================
                // Header Assortment Basis
                //==========================
                _basisList.Clear();

                HierarchyNodeProfile planLevelHnp = null;
                bool dummyPlanLevel = false;
				// Begin TT#1115 - md - stodd - Sales on Matrix is not matching the plan level - 
                int planLevelNodeRid = PlanHnRID;
                if (planLevelNodeRid == Include.NoRID)
				// End TT#1115 - md - stodd - Sales on Matrix is not matching the plan level - 
                {
                    dummyPlanLevel = true;
                    // Find the highest node in the organizational hierarchy. Use it as a dummy plan level.
                    // This happens when a Group Allocation has no headers yet.
                    HierarchyNodeList hierarchyChildrenList = SAB.HierarchyServerSession.GetRootNodes();
                    foreach (HierarchyNodeProfile hnp in hierarchyChildrenList)
                    {
                        if (hnp.HomeHierarchyType == eHierarchyType.organizational)
                        {
                            planLevelNodeRid = hnp.Key;
                            planLevelHnp = hnp;
                        }
                    }
                }

				// Begin TT#1122 - md - stodd - calendar exception creating group allocation - 
				// Begin TT#1127 - md -stodd - invalid calendar date 
                DateTime anchorDay = ShipToDay;
                if (anchorDay == Include.UndefinedDate)
                {
                    anchorDay = SAB.ApplicationServerSession.Calendar.CurrentDate.Date;
                }
				// End TT#1122 - md - stodd - calendar exception creating group allocation - 

                DayProfile aDay = SAB.ApplicationServerSession.Calendar.GetDay(anchorDay);
				// End TT#1127 - md -stodd - invalid calendar date 
				// Begin TT#3940 - stodd - error when trying to open prior groups
                //WeekProfile fromWeek = SAB.ApplicationServerSession.Calendar.CurrentWeek;
                WeekProfile fromWeek = OnHandDayProfile.Week;
				// End TT#3940 - stodd - error when trying to open prior groups

                WeekProfile toWeek = aDay.Week;
                if (dummyPlanLevel)
                {
                    int fromWeekKey = SAB.ApplicationServerSession.Calendar.AddWeeks(fromWeek.Key, 260);
                    int toWeekKey = SAB.ApplicationServerSession.Calendar.AddWeeks(toWeek.Key, 260);
                    fromWeek = SAB.ApplicationServerSession.Calendar.GetWeek(fromWeekKey);
                    toWeek = SAB.ApplicationServerSession.Calendar.GetWeek(toWeekKey);
                }

				// Begin TT#3940 - stodd - error when trying to open prior groups
                if (fromWeek.Key > toWeek.Key)
                {
                    int toWeekKey = SAB.ApplicationServerSession.Calendar.AddWeeks(fromWeek.Key, AppSessionTransaction.GlobalOptions.ShippingHorizonWeeks);
                    toWeek = SAB.ApplicationServerSession.Calendar.GetWeek(toWeekKey);
                }
				// End TT#3940 - stodd - error when trying to open prior groups

                DateRangeProfile drp = SAB.ApplicationServerSession.Calendar.AddDateRangeFromWeeks(fromWeek.YearWeek, toWeek.YearWeek);
                int aCdrRid = drp.Key;

                AssortmentBasis ab = new AssortmentBasis(SAB, AppSessionTransaction, planLevelNodeRid, Include.FV_ActionRID, aCdrRid, 100, null);	// TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 

                if (planLevelHnp == null)
                {
                    planLevelHnp = SAB.HierarchyServerSession.GetNodeData(planLevelNodeRid, false);
                }

                string debugMsg = planLevelHnp.Text + " (" + PlanHnRID.ToString() + "), " + "Action, " + drp.DisplayDate;	// TT#1115 - md - stodd - Sales on Matrix is not matching the plan level - 
                SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Group Allocation: " + HeaderID + ", " + debugMsg, "SummaryProfile", true);
                Debug.WriteLine(debugMsg);
                _basisList.Add(ab);
            }
            catch
            {
                throw;
            }
        }

        public void FillAssortGradesFromGroupAllocation(ApplicationSessionTransaction trans)	// TT#4332 - stodd - Matrix randomly shows all zeros for a grade tier
        {
            try
            {
                //================================
                // Header Assortment Store Gradesv
                //================================
                AssortmentStoreGradeList.Clear();	// TT#488-MD - STodd - Group Allocation 
				// Begin TT#1146-MD - stodd - "f" store received allocation - 
                ProfileList sgl = (ProfileList)GetStoreGrades();
                //StoreGradeList sgl = trans.GetStoreGradeList(_assortAnchorNodeRid);
                foreach (StoreGradeProfile aGrade in sgl.ArrayList)
                {
                    AssortmentStoreGradeList.Add(aGrade);
                }
				// end TT#1146-MD - stodd - "f" store received allocation - 
            }
            catch
            {
                throw;
            }
        }
		// End TT#952 - MD - stodd - add matrix to Group Allocation Review

		/// <summary>
		/// Fills Assortment Summary from Database table
		/// </summary>
		public void BuildAssortmentSummary()
		{
			try
			{
                StoreGroupProfile sgp = StoreMgmt.StoreGroup_GetFilled(_assortStoreGroupRid); //SAB.StoreServerSession.GetStoreGroupFilled(_assortStoreGroupRid);
				// Begin TT#2 - stodd - assortment
				_lastSglRidUsedInSummary = sgp.Key;
				_assortmentSummaryProfile = new AssortmentSummaryProfile(this, SAB, this.AppSessionTransaction, sgp, AssortmentStoreGradeList, _assortAverageBy);	// TT#488-MD - STodd - Group Allocation 
				//_assortmentSummary = new AssortmentSummary(sgp, StoreGradeList);

				//if (fromDB)
				//    _dtTotalAssortment = HeaderDataRecord.GetTotalAssortment(this._key);

				//_assortmentSummaryProfile.FillAssortmentSummary(_dtTotalAssortment);
				//FillAssortmentSummary();
				// End TT#2 - stodd - assortment
				//Begin TT#2 - JScott - Assortment Planning - Phase 2

				BasisReader.AssortmentPlanCubeGroup.OpenGradeCubes(AssortmentStoreGradeList, _assortmentSummaryProfile.SetGradeStoreXRef);	// TT#488-MD - STodd - Group Allocation 
				//End TT#2 - JScott - Assortment Planning - Phase 2

                BuildAssortmentGradesByStore();		// TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
			}
			catch
			{
				throw;
			}
		}
        #endregion Fill

        // Begin TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
        // Begin TT#2122-MD - JSmith - After a REDO the Assortment Grades in Style Review are the same as before doing the REDO.  Expected them to change based on the REDO being processed.
        //private void BuildAssortmentGradesByStore()
        public void BuildAssortmentGradesByStore()
        // End TT#2122-MD - JSmith - After a REDO the Assortment Grades in Style Review are the same as before doing the REDO.  Expected them to change based on the REDO being processed.
        {
            _assortmentGradesByStore = new Dictionary<int, string>();
            foreach (StoreGroupLevelProfile sglp in _assortmentSummaryProfile.SGP.GroupLevels)
            {
                foreach (StoreGradeProfile sgp  in _assortmentSummaryProfile.Gradelist)
                {
                    ProfileList gradeStoreList = GetStoresInSetGrade(sglp.Key, sgp.Key);
                    foreach (StoreProfile sp in gradeStoreList)
                    {
                        _assortmentGradesByStore.Add(sp.Key, sgp.StoreGrade);
                    }
                }
            }
        }

        public string GetAssortmentGrade(int aStoreRID)
        {
            string storeGrade = " ";
            if (_assortmentGradesByStore == null)
            {
                BuildAssortmentGradesByStore();
            }

            if (!_assortmentGradesByStore.TryGetValue(aStoreRID, out storeGrade))
            {
                storeGrade = " ";
            }

            return storeGrade;
        }
		// End TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working

		// Begin TT#2 - stodd -assortment
		// BEGIN TT#488-MD - STodd - Group Allocation 
		//object ICloneable.Clone()
		//{
		//    return this.Clone();
		//}

		//public AssortmentProfile Clone()
		//{
		//    return (AssortmentProfile)this.MemberwiseClone();
		//}
		// END TT#488-MD - STodd - Group Allocation 

		//public object Clone()
		//{
		//    AssortmentProfile newAsp = new AssortmentProfile(AppSessionTransaction, base.HeaderID, base.Key, base.Session);		// TT#2 - stodd - 2.3.2011

		//    newAsp.AssortmentReserveAmount = _assortReserveAmount;
		//    newAsp.AssortmentReserveType = _assortReserveType;	
		//    newAsp.AssortmentStoreGroupRID = _assortStoreGroupRid;
		//    newAsp.AssortmentVariableType = _assortVariableType;
		//    newAsp.AssortmentVariableNumber = _assortVariableNumber;
		//    newAsp.AssortmentIncludeOnhand = _assortInclOnhand;
		//    newAsp.AssortmentIncludeIntransit = _assortInclIntransit;
		//    newAsp.AssortmentIncludeSimilarStores = _assortInclSimStores;
		//    newAsp.AssortmentIncludeCommitted = _assortInclCommitted;
		//    newAsp.AssortmentAverageBy = _assortAverageBy;
		//    newAsp.AssortmentGradeBoundary = _assortGradeBoundary;
		//    newAsp.AssortmentCalendarDateRangeRid = _assortCdrRid;
		//    newAsp.HeaderDay = base.HeaderDay;
		//    newAsp.AsrtType =  base.AsrtType;
		//    newAsp.AssortmentBasisList = _basisList;
		//    newAsp.StoreGradeList = _storeGradeList;
		//    return newAsp;
		//}
		// End TT#2 - stodd -assortment

        #region Set Assortment Fields
		/// <summary>
		/// Sets the assortment fields within the assortment profile from the arguemtns sent in.
		/// </summary>
		/// <param name="headerRid"></param>
		/// <param name="aReserverAmt"></param>
		/// <param name="aReserveType"></param>
		/// <param name="aStoreGroupRid"></param>
		/// <param name="aVariableType"></param>
		/// <param name="aInclOnhand"></param>
		/// <param name="aInclIntransit"></param>
		/// <param name="aInclSimStores"></param>
		/// <param name="aUserRid"></param>
		/// <param name="aProcessDate"></param>
		// Begin TT#2 - stodd - assortment
		public void SetAssortmentHeader(
			int headerRid,
			//int aStoreFilterRid,
			double aReserverAmt,
			eReserveType aReserveType,
			int aStoreGroupRid,
			eAssortmentVariableType aVariableType,
			int aVariableNumber,
			bool aInclOnhand,
			bool aInclIntransit,
			bool aInclSimStores,
			bool aInclCommitted,
			eStoreAverageBy aAverageBy,
			//int aGenAssortMethodRid,
			int aCdrRid,
			int aUserRid,
			DateTime aProcessDate)
		{
			try
			{
				//_storeFilterRid = aStoreFilterRid;
				_assortReserveAmount = aReserverAmt;
				_assortReserveType = aReserveType;
				_assortStoreGroupRid = aStoreGroupRid;
				_assortVariableType = aVariableType;
				_assortVariableNumber = aVariableNumber;
                // Begin TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
                //_assortInclIntransit = aInclIntransit;
                //_assortInclOnhand = aInclOnhand;
                _assortInclIntransit = true;
                _assortInclOnhand = true;
                // Begin TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
				_assortInclSimStores = aInclSimStores;
				_assortInclCommitted = aInclCommitted;
				_assortAverageBy = aAverageBy;
				//_genAssortMethodRid = aGenAssortMethodRid;
				_assortUserRid = aUserRid;
				_assortLastProcessDt = aProcessDate;
			}
			catch
			{
				throw;
			}
		}
		// End TT#2 - stodd - assortment

		/// <summary>
		/// Sets the assortment grade fields
		/// </summary>
		/// <param name="headerRid"></param>
		/// <param name="gradeCodeList"></param>
		/// <param name="boundaryList"></param>
		/// <returns></returns>
		// Begin TT#2 - stodd - assortment
		public void SetAssortmentGrades(
			int headerRid,
			List<string> gradeCodeList,
			List<double> boundaryList,
			List<double> boundaryUnitsList)
		{
			try
			{
				AssortmentStoreGradeList.Clear();	// TT#488-MD - STodd - Group Allocation 
				for (int i=0;i<gradeCodeList.Count;i++)
				{
					StoreGradeProfile sgp = new StoreGradeProfile((int)boundaryList[i]);
					sgp.Boundary = (int)boundaryList[i];
					sgp.BoundaryUnits = (int)boundaryUnitsList[i];
					sgp.StoreGrade = gradeCodeList[i].Trim();
					AssortmentStoreGradeList.Add(sgp);	// TT#488-MD - STodd - Group Allocation 
				}
			}
			catch
			{
				throw;
			}
		}
		// End TT#2 - stodd - assortment

		/// <summary>
		/// Sets the assortment basis fields
		/// </summary>
		/// <param name="headerRid"></param>
		/// <param name="hierNodeList"></param>
		/// <param name="versionList"></param>
		/// <param name="dateRangeList"></param>
		/// <param name="weightList"></param>
		/// <returns></returns>
		public void SetAssortmentBasis(
			int headerRid,
			List<int> hierNodeList,
			List<int> versionList,
			List<int> dateRangeList,
			List<double> weightList)
		{
			try
			{

				_basisList.Clear();
				for (int i = 0; i < hierNodeList.Count; i++)
				{
                    AssortmentBasis ab = new AssortmentBasis(SAB, AppSessionTransaction, hierNodeList[i], versionList[i], dateRangeList[i], (float)weightList[i], AssortmentApplyToDate);	// TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 
					_basisList.Add(ab);
				}
			}
			catch
			{
				throw;
			}
		}
        #endregion Set Assortment Fields

        #region Write Assortment
		/// <summary>
		/// This only writes the Assortment portion of the header information.
		/// </summary>
		public void WriteAssortment()
		{
			try
			{
                if (HeaderDataRecord.DeleteAllAssortmentPropertiesData(this.Key))
                {
                    WriteAssortmentHeader();

                    WriteAssortmentGrades();

                    WriteAssortmentBasis();
                }

			}
			catch
			{
				throw;
			}
		}

		private void WriteAssortmentBasis()
		{
			try
			{
				//=========================
				// Write Assortment Basis
				//=========================
				int seq = 1;
                
				foreach (AssortmentBasis ab in _basisList)
				{
					HeaderDataRecord.WriteAssortmentPropertiesBasis(this.Key, seq++, ab.HierarchyNodeProfile.Key, ab.VersionProfile.Key, ab.HorizonDate.Key, ab.Weight);
				}
			}
			catch
			{
				throw;
			}
		}

		private void WriteAssortmentGrades()
		{
			try
			{
				//=========================
				// Write Assortment Grades
				//=========================
				int seq = 1;

				foreach (StoreGradeProfile sgp in AssortmentStoreGradeList.ArrayList)	// TT#488-MD - STodd - Group Allocation 
				{
					HeaderDataRecord.WriteAssortmentPropertiesStoreGrade(this.Key, seq++, sgp.StoreGrade, sgp.Boundary, sgp.BoundaryUnits);
				}
			}
			catch
			{
				throw;
			}
		}

		private void WriteAssortmentHeader()
		{
			try
			{
				//===========================
				// Write Assortment Header
				//===========================
				// Begin TT#2 - stodd - assortment
				HeaderDataRecord.WriteAssortmentProperties(
					this.Key,
					_assortReserveAmount,
					_assortReserveType,
					_assortStoreGroupRid,
					_assortVariableType,
					_assortVariableNumber,
                    _assortInclOnhand,     //TT2016-MD - AGallagher - Assortment Review Navigation
                    _assortInclIntransit,  //TT2016-MD - AGallagher - Assortment Review Navigation
					_assortInclSimStores,
					_assortInclCommitted,
					_assortAverageBy,
					_assortCdrRid,
					_assortAnchorNodeRid,
					_assortUserRid,
					_assortLastProcessDt,
                    _assortBeginDayCdrRid);   // TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
				// End TT#2 - stodd - assortment
			}
			catch
			{
				throw;
			}
		}

		// Begin TT#1006 - MD - stodd - status out of sync - 
        override internal bool WriteHeaderData(Header aHeaderDataRecord)
        {
            bool updateSuccess = true;
            AllocationProfile[] apList = AssortmentMembers;
            
            try
            {
                // begin TT#1062 - MD - Jellis - Group Allocation Action Commmit Multiple Times
                if (_assortmentActionCount == 0)
                {
                    // Begin TT#790-MD - stodd - Attached a header with a detail pack to an assortment.  PH does not decrease in qty
                    if (AssortmentProfile != null && AssortmentProfile.AsrtType == (int)eAssortmentType.GroupAllocation) 	// TT#806-MD - stodd - Running cancel allocation on PH causes Ship To Date to change  
                    {
                        // end TT#1062 - MD - Jellis - Group Allocation Action Commmit Multiple Times
                        foreach (AllocationProfile ap in apList)
                        {
                            // Begin TT#1723-MD - JSmith - Records written during cancel allocation
                            // Add audit entry so WriteHeaderData will know BackoutAllocation is being performed
                            int auditEntry = Include.Undefined;
                            if (_currentAction == eAllocationMethodType.BackoutAllocation)
                            {
                                ap.ActionAuditList.Add(new AllocationActionAuditStruct
                                            ((int)eAllocationMethodType.BackoutAllocation,
                                            this,
                                            new GeneralComponent(eComponentType.Total),
                                            0,
                                            0,
                                            false,
                                            false));
                                auditEntry = ap.ActionAuditList.Count;
                            }
                            // End TT#1723-MD - JSmith - Records written during cancel allocation
                            //if (!ap.WriteHeaderData(ap.HeaderDataRecord))
                            if (!ap.WriteHeaderData(aHeaderDataRecord))
                            {
                                updateSuccess = false;
                                break;
                            }
                            // Begin TT#1723-MD - JSmith - Records written during cancel allocation
                            if (auditEntry != Include.Undefined)
                            {
                                ap.ActionAuditList.RemoveAt(auditEntry - 1);
                            }
                            // End TT#1723-MD - JSmith - Records written during cancel allocation
                        }
                    }
                    // End TT#790-MD - stodd - Attached a header with a detail pack to an assortment.  PH does not decrease in qty

                    if (updateSuccess)
                    {
                        // begin TT#925 - MD - Jellis - Group Balance To Reserve ignores store allocations
                        if (_placeholderAction)
                        {
                            foreach (AllocationProfile ap in AssortmentPlaceHolders) // TT#995 - MD - Jellis - Group Allocation Infrastructure built wrong
                            {
                                // Begin TT#1723-MD - JSmith - Records written during cancel allocation
                                // Add audit entry so WriteHeaderData will know BackoutAllocation is being performed
                                int auditEntry = Include.Undefined;
                                if (_currentAction == eAllocationMethodType.BackoutAllocation)
                                {
                                    ap.ActionAuditList.Add(new AllocationActionAuditStruct
                                                ((int)eAllocationMethodType.BackoutAllocation,
                                                this,
                                                new GeneralComponent(eComponentType.Total),
                                                0,
                                                0,
                                                false,
                                                false));
                                    auditEntry = ap.ActionAuditList.Count;
                                }
                                // End TT#1723-MD - JSmith - Records written during cancel allocation
                                if (!ap.WriteHeaderData(ap.HeaderDataRecord))
                                {
                                    updateSuccess = false;
                                    break;
                                }
                                // Begin TT#1723-MD - JSmith - Records written during cancel allocation
                                if (auditEntry != Include.Undefined)
                                {
                                    ap.ActionAuditList.RemoveAt(auditEntry - 1);
                                }
                                // End TT#1723-MD - JSmith - Records written during cancel allocation
                            }
                        }
                        if (updateSuccess)
                        {
                            // end TT#925 - MD - Jellis - Group Balance To Reserve ignores store allocations
                            //updateSuccess = base.WriteHeaderData(base.HeaderDataRecord);
                            // Begin TT#1723-MD - JSmith - Records written during cancel allocation
                            // Add audit entry so WriteHeaderData will know BackoutAllocation is being performed
                            int auditEntry = Include.Undefined;
                            if (_currentAction == eAllocationMethodType.BackoutAllocation)
                            {
                                ActionAuditList.Add(new AllocationActionAuditStruct
                                            ((int)eAllocationMethodType.BackoutAllocation,
                                            this,
                                            new GeneralComponent(eComponentType.Total),
                                            0,
                                            0,
                                            false,
                                            false));
                                auditEntry = ActionAuditList.Count;
                            }
                            // End TT#1723-MD - JSmith - Records written during cancel allocation
                            updateSuccess = base.WriteHeaderData(aHeaderDataRecord);
                            // Begin TT#1723-MD - JSmith - Records written during cancel allocation
                            if (auditEntry != Include.Undefined)
                            {
                                ActionAuditList.RemoveAt(auditEntry - 1);
                            }
                            // End TT#1723-MD - JSmith - Records written during cancel allocation
                            // Begin TT#2109-MD - JSmith - Change the Beginning and Delivery Date in an Asst and the Delivery Date is incorrect.
                            if (updateSuccess
                                && SaveAssortmentMembers)
                            {
                                foreach (AllocationProfile ap in apList)
                                {
                                    auditEntry = Include.Undefined;
                                    if (_currentAction == eAllocationMethodType.BackoutAllocation)
                                    {
                                        ap.ActionAuditList.Add(new AllocationActionAuditStruct
                                                    ((int)eAllocationMethodType.BackoutAllocation,
                                                    this,
                                                    new GeneralComponent(eComponentType.Total),
                                                    0,
                                                    0,
                                                    false,
                                                    false));
                                        auditEntry = ap.ActionAuditList.Count;
                                    }
                                    if (!ap.WriteHeaderData(aHeaderDataRecord))
                                    {
                                        updateSuccess = false;
                                        break;
                                    }
                                    if (auditEntry != Include.Undefined)
                                    {
                                        ap.ActionAuditList.RemoveAt(auditEntry - 1);
                                    }
                                }
                            }
                            // End TT#2109-MD - JSmith - Change the Beginning and Delivery Date in an Asst and the Delivery Date is incorrect.
                            if (updateSuccess)
                            {
                                //HeaderDataRecord.CommitData();
                                aHeaderDataRecord.CommitData();
                                // begin TT#1074 - MD - Jelis -- Group Allocation Inventory Min Max Broken
                                SetStoreCapacityNotLoaded();
                                foreach (AllocationProfile memberAp in AssortmentMembers)
                                {
                                    memberAp.SetStoreCapacityNotLoaded();
                                }
                                // end TT#1074 - MD - Jellis - - Group Allocation Inventory Min Max Broken
                            }
                        } // TT#925 - MD - Jellis - Group Balance To Reserve ignores store allocations
                    }
                }  // TT#1051 - MD - Jellis - Pack Allocation Zeroed out
            }  // TT#1062 - MD - Jellis - Group Allocation Action Commmit Multiple Times
            catch
            {
                updateSuccess = false;
                throw;
            }
            finally
            {
                if (_processingMethod)
                {
                    //if (HeaderDataRecord.ConnectionIsOpen)
                    //{
                    //    HeaderDataRecord.CloseUpdateConnection();
                    //}
                    ProcessingGroupAllocation = false;
                }

            }
            return updateSuccess;
        }
		// End TT#1006 - MD - stodd - status out of sync - 
		
        #endregion Write Assortment

		///// <summary>
		///// Clears the total assortment dataTable of data.
		///// </summary>
		//public void ClearTotalAssortment()
		//{
		//    try
		//    {
		//        if (_dtTotalAssortment == null)
		//        {
		//            _dtTotalAssortment = HeaderDataRecord.TotalAssortment_DataTable();
		//        }
		//        else
		//        {
		//            _dtTotalAssortment.Clear();
		//        }
		//    }
		//    catch
		//    {
				
		//        throw;
		//    }
		//    //HeaderDataRecord.DeleteTotalAssortment(headerRid);
		//}

		///// <summary>
		///// Sets the total assortment information and fills the assortment summary information
		///// </summary>
		///// <param name="headerRid"></param>
		///// <param name="variableNumber"></param>
		///// <param name="storeList"></param>
		///// <param name="storeSetList"></param>
		///// <param name="storeGradeIndexList"></param>
		///// <param name="unitsList"></param>
		///// <param name="avgStoreList"></param>
		//public void SetAssortmentStoreSummary(

		//    int headerRid,
		//    int variableNumber,
		//    List<int> storeList,
		//    List<int> storeSetList,  //StoreGroupLevelRid
		//    List<int> storeGradeIndexList,
		//    List<int> unitsList,
		//    List<int> avgStoreList,
		//    List<int> intransitList,
		//    List<int> needList,
		//    List<decimal> pctNeedList)
		//{
		//    try
		//    {
		//        if (storeSetList.Count > 0)
		//        {
		//            for (int i = 0; i < storeList.Count; i++)
		//            {
		//                DataRow newRow = _dtTotalAssortment.NewRow();
		//                newRow["HDR_RID"] = this.Key;
		//                newRow["ST_RID"] = storeList[i];
		//                newRow["SGL_RID"] = storeSetList[i];
		//                newRow["VARIABLE_NUMBER"] = variableNumber;
		//                newRow["STORE_GRADE_INDEX"] = storeGradeIndexList[i];
		//                newRow["UNITS"] = unitsList[i];
		//                newRow["AVERAGE_STORE"] = avgStoreList[i];
		//                newRow["INTRANSIT"] = intransitList[i];
		//                newRow["NEED"] = needList[i];
		//                newRow["PCT_NEED"] = pctNeedList[i];
		//                _dtTotalAssortment.Rows.Add(newRow);
		//            }
		//        }
		//    }
		//    catch
		//    {

		//        throw;
		//    }
		//}

        #region Pack Color Hash
		private class PackColorHashEntry
		{
			private int _RID;
			private string _ID;
			private string _alternateID;

			public PackColorHashEntry(int aRID, string aID, string aAlternateID)
			{
				_RID = aRID;
				_ID = aID;
				_alternateID = aAlternateID;
			}

			public int RID
			{
				get
				{
					return _RID;
				}
			}

			public string ID
			{
				get
				{
					return _ID;
				}
			}

			public string AlternateID
			{
				get
				{
					return _alternateID;
				}
			}
		}
        #endregion Pack Color Hash

        // begin TT#488 - MD - Jellis - Group Assortment
        #region Event Handlers
        // begin TT#488 - MD - Jellis - Group Assortment
        /// <summary>
        /// reset AssortmentMembers whenever the Master Profile List for Allocation changes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void OnMasterProfileChange(object source, ProfileListChangeEventArgs e)
        {
            try
            {
                if (e.ChangeType != eChangeType.none
                    && e.ProfileType == eProfileType.AssortmentMember)  // TT#488 - MD - Jellis - Group Allocation
                    //&& e.ProfileType == eProfileType.Allocation)  // TT#488 - MD - JEllis - Group Allocation
                {
                    ClearAssortment(); // TT#946 - MD - Jellis - Group Allocation Not Working
                    _assortmentMembers = null;
                    _assortmentPackHome = null; // TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                    _placeHolders = null;   // TT#891 - MD - Jellis - Group Allocation NEED Action Gets Error
                    _memberAllocationMultiple = -1;
                    _memberGenericMultiple = -1;
                    _memberDetailTypeMultiple = -1;
                    _memberBulkMultiple = -1;
                    DoColorCheck = true;

                    _sortedAssortmentMembers = null; // TT#488 - MD - Jellis - Group Assortment
                    _suspendAssortmentUpdates = false; // TT#488 - MD - Jellis - Group Allocation
                    _buildAssortmentProperties = true; // TT#488 - MD - Jellis - Group Allocation
                    _buildInventoryBasisAllocation = true; // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                    _buildPriorItemUnitsManuallyAllocated = true; // TT#3941 - Urbaan - Jellis - Group Allocation Item/VSW split incorrect
                    _assortmentPacksLoaded = false;  // TT#1008 - MD - Jellis - Get Non Negative Message for various variables

                    //ClearAssortment();               // TT#488 - MD - Jellis - Group Allocation  // TT#946 - MD - Jellis - Group Allocation Not Working
                    AllocationProfileList apl = (AllocationProfileList)base.AppSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember); // TT#488 - MD - Jellis - Group Allocation
                    if (apl != null)
                    {
                        apl.ProfileListChangeEvent.OnProfileListChangeHandler -= new ProfileListChangeEvent.ProfileListChangeEventHandler(OnMasterProfileChange);
                    }
                }
            }
            catch 
            {
                throw;
            }
        }
        #endregion Event Handlers
	
        #region IDisposable Members
        override public void Dispose() // TT#488 - MD - Jellis - Group Allocation
		{
			Dispose(true);
			System.GC.SuppressFinalize(this);
		}

		override protected void Dispose(bool disposing) // TT#488 - Jellis - Group Allocation
		{
			if (_assortmentMembers != null)
			{
                _assortmentMembers = null;
                _assortmentPackHome = null; // TT#1008 - MD - Jellis - Get Non Negative Message for various variables
                _placeHolders = null; // TT#891 - MD - Jellis - Group Allocation Need Action gets error
                _sortedAssortmentMembers = null; // TT#488 - MD - Jellis - Group Allocation
                AllocationProfileList apl = (AllocationProfileList)base.AppSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember); // TT#488 - MD - Jellis - Group Allocation
                if (apl != null)
                {
                    apl.ProfileListChangeEvent.OnProfileListChangeHandler -= new ProfileListChangeEvent.ProfileListChangeEventHandler(OnMasterProfileChange);
                }
			}
            base.Dispose(disposing);
		}

		~AssortmentProfile()
		{
			Dispose(false);
		}

		#endregion IDisposable Members
        // end TT#488 - MD - Jellis - Group Assortment

        #endregion Methods
    }



	#region AssortmentBasis
	public class AssortmentBasis
	{
		//=======
		// FIELDS
		//=======

		private HierarchyNodeProfile _hierarchyNodeProfile;
		private VersionProfile _versionProfile;
		private DateRangeProfile _dateRangeProfile;
		//private eBasisIncludeExclude _includeExclude;
		private float _weight;

		public HierarchyNodeProfile HierarchyNodeProfile
		{
			get { return _hierarchyNodeProfile; }
			set { _hierarchyNodeProfile = value; }
		}
		public VersionProfile VersionProfile
		{
			get { return _versionProfile; }
			set { _versionProfile = value; }
		}
		public DateRangeProfile HorizonDate
		{
			get { return _dateRangeProfile; }
			set { _dateRangeProfile = value; }
		}
		public float Weight
		{
			get { return _weight; }
			set { _weight = value; }
		}

		//=============
		// CONSTRUCTORS
		//=============

		// Begin TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 
		public AssortmentBasis(SessionAddressBlock aSab, ApplicationSessionTransaction aTrans,
			int hierNodeRid, int versionRid, int DateRangeRid, float weight, DayProfile anchorDay)
		{
            Fill(aSab, aTrans, hierNodeRid, versionRid, DateRangeRid, weight, anchorDay);
		}

		private void Fill(SessionAddressBlock aSab, ApplicationSessionTransaction aTrans, int hierNodeRid, int versionRid, int DateRangeRid, float weight, DayProfile anchorDay)
		{
			try
			{
				// End TT#2 - stodd - assortment
				if (aTrans == null)
					aTrans = aSab.ApplicationServerSession.CreateTransaction();
				//_hierarchyNodeProfile = aTrans.GetPlanLevelData(hierNodeRid);
				// anchor node s/b plan level
				_hierarchyNodeProfile = aTrans.GetNodeData(hierNodeRid);
				// End TT#2 - stodd - assortment
				if (_hierarchyNodeProfile == null)
				{
					throw new MIDException(eErrorLevel.severe,
						(int)(eMIDTextCode.msg_al_PlanLevelUndetermined),
						MIDText.GetText(eMIDTextCode.msg_al_PlanLevelUndetermined));
				}

				ProfileList vpList = aSab.ClientServerSession.GetUserForecastVersions();
				_versionProfile = (VersionProfile)vpList.FindKey(versionRid);

                //BEGIN TT#840-MD-DOConnell-When reopening an assortment with a dynamic-to-plan date in the basis, the basis date is blank.
                //DayProfile anchorDate = aSab.ClientServerSession.Calendar.CurrentDate;
                //_dateRangeProfile = aSab.ApplicationServerSession.Calendar.GetDateRange(DateRangeRid);
                _dateRangeProfile = aSab.ApplicationServerSession.Calendar.GetDateRange(DateRangeRid, anchorDay);
                //END TT#840-MD-DOConnell-When reopening an assortment with a dynamic-to-plan date in the basis, the basis date is blank.

				_weight = weight;
			}
			catch
			{
				throw;
			}
		}
		// End TT#1440-MD - stodd - Sales total on Summary matrix do not match Sales for the same weeks in OTS Forecast Review - 

	}
	#endregion
}
