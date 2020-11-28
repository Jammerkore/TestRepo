using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business.Allocation
{
	#region AllocationHeaderProfileList
	/// <summary>
	/// Contains a list of allocation header profiles necessary to support remote procedures.  This profile
	/// is a subset of the AllocationProfile and is used to communicate information from the AllocationProfiles
	/// found on the transactions in the sessions to remoted locations.
	/// </summary>
	[Serializable]
	public class AllocationHeaderProfileList : ProfileList
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		private int _grandTotalToAllocate;
		#endregion Fields

		#region Constructors
		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public AllocationHeaderProfileList(eProfileType aProfileType)
			: base(aProfileType)
		{
			
		}
		#endregion Constructors

		#region Properties
		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets the grand total units to allocate across all headers in this profile list.
		/// </summary>
		public int GrandTotalToAllocate
		{
			get
			{
				return _grandTotalToAllocate;
			}
			set
			{
				_grandTotalToAllocate = value;
			}
		}
		#endregion Properties

		#region Methods
		public void LoadAll(AllocationProfileList aAllocationProfileList)
		{
			foreach(AllocationProfile ap in aAllocationProfileList)
			{
				AllocationHeaderProfile ahp = new AllocationHeaderProfile(ap.HeaderID, ap.Key);
				ahp.HeaderDescription = ap.HeaderDescription;
				ahp.HeaderDay = ap.HeaderDay;
				ahp.OriginalReceiptDay = ap.OriginalReceiptDay;
				ahp.UnitRetail = ap.UnitRetail;
				ahp.UnitCost = ap.UnitCost;
				ahp.StyleHnRID = ap.StyleHnRID;
				ahp.PlanHnRID = ap.PlanHnRID;
				ahp.OnHandHnRID = ap.OnHandHnRID;
				ahp.Vendor = ap.Vendor;
				ahp.PurchaseOrder = ap.PurchaseOrder;
				ahp.BeginDay = ap.BeginDay;
				ahp.ShipToDay = ap.ShipToDay;
				ahp.LastNeedDay = ap.LastNeedDay;
				ahp.ReleaseApprovedDate = ap.ReleaseApprovedDate;
				ahp.ReleaseDate = ap.ReleaseDate;
				ahp.HeaderGroupRID = ap.HeaderGroupRID;
				ahp.SizeGroupRID = ap.SizeGroupRID;
				ahp.WorkflowRID = ap.WorkflowRID;
				ahp.API_WorkflowRID = ap.API_WorkflowRID;
				ahp.MethodRID = ap.MethodRID;
				// begin MID Track 3523 Allow Work Up Size Buy
				//ahp.BulkIsDetail = ap.BulkIsDetail;
				ahp.AllocationStarted = ap.AllocationStarted;
				//ahp.ReceivedInBalance = ap.ReceivedInBalance;
				//ahp.BottomUpSizePerformed = ap.BottomUpSizePerformed;
				//ahp.RulesDefinedAndProcessed = ap.RulesDefinedAndProcessed;
				//ahp.NeedAllocationPerformed = ap.NeedAllocationPerformed;
				//ahp.PackBreakoutByContent = ap.PackBreakoutByContent;
				//ahp.BulkSizeBreakoutPerformed = ap.BulkSizeBreakoutPerformed;
				//ahp.ReleaseApproved = ap.ReleaseApproved;
				//ahp.Released = ap.Released;
				//ahp.ShippingStarted = ap.ShippingStarted;
				//ahp.ShippingComplete = ap.ShippingComplete;
				//ahp.ShippingOnHold = ap.ShippingOnHold;
				//ahp.SizeReceiptsBalanceToColor = ap.SizeReceiptsBalanceToColor;
				//ahp.ColorReceiptsBalanceToBulk = ap.ColorReceiptsBalanceToBulk;
				//ahp.PackSizeBalanceToPackColor = ap.PackSizeBalanceToPackColor;
				//ahp.PackColorBalanceToPack = ap.PackColorBalanceToPack;
				//ahp.PackPlusBulkReceiptsBalanceToTotal = ap.PackPlusBulkReceiptsBalanceToTotal;
				//ahp.PackAllocationInBalance = ap.PackAllocationInBalance;
				//ahp.BulkColorAllocationInBalance = ap.BulkColorAllocationInBalance;
				//ahp.BulkPlusPackAllocationInBalance = ap.BulkPlusPackAllocationInBalance;
				//ahp.BulkSizeAllocationInBalance = ap.BulkSizeAllocationInBalance;
				//ahp.StyleIntransitUpdated = ap.StyleIntransitUpdated;
				//ahp.BulkColorIntransitUpdated = ap.BulkColorIntransitUpdated;
				ahp.AllocationStatusFlags = ap.AllocationStatusFlags;
				ahp.AllocationTypeFlags = ap.AllocationTypeFlags;
				ahp.BalanceStatusFlags = ap.BalanceStatusFlags;
				ahp.ShippingStatusFlags = ap.ShippingStatusFlags;
				ahp.IntransitUpdateStatusFlags = ap.IntransitUpdateStatusFlags;
				// end MID Track 3523 Allow WOrk Up Size Buy
				ahp.PercentNeedLimit = ap.PercentNeedLimit;
				ahp.PlanFactor = ap.PlanFactor;
				ahp.ReserveUnits = ap.ReserveUnits;
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
				ahp.AllocatedUnits = ap.AllocatedUnits;
				ahp.OrigAllocatedUnits = ap.OrigAllocatedUnits;
				ahp.RsvAllocatedUnits = ap.RsvAllocatedUnits;
				ahp.ReleaseCount = ap.ReleaseCount;
// (CSMITH) - END MID Track #3219
				ahp.TotalUnitsToAllocate = ap.TotalUnitsToAllocate;
//				ahp.TotalUnitsAllocated = ap.TotalUnitsAllocated;
				ahp.UnitsShipped = ap.UnitsShipped;
				ahp.AllocationMultiple = ap.AllocationMultiple;
				ahp.GenericUnitsToAllocate = ap.GenericUnitsToAllocate;
//				ahp.GenericUnitsAllocated = ap.GenericUnitsAllocated;
				ahp.GenericMultiple = ap.GenericMultiple;
				ahp.DetailTypeUnitsToAllocate = ap.DetailTypeUnitsToAllocate;
//				ahp.DetailTypeUnitsAllocated = ap.DetailTypeUnitsAllocated;
				ahp.DetailTypeMultiple = ap.DetailTypeMultiple;
				ahp.BulkUnitsToAllocate = ap.BulkUnitsToAllocate;
//				ahp.BulkUnitsAllocated = ap.BulkUnitsAllocated;
				ahp.BulkMultiple = ap.BulkMultiple;
				ahp.HeaderAllocationStatus = ap.HeaderAllocationStatus;
				// begin MID Track 4448 AnF Audit Enhancement
				ahp.StoreStyleAllocationManuallyChangedCount = ap.StoreStyleAllocationManuallyChangedCount;
				ahp.StoreSizeAllocationManuallyChangedCount = ap.StoreSizeAllocationManuallyChangedCount;
				ahp.StoreStyleManualAllocationTotal = ap.StoreStyleManualAllocationTotal;
				ahp.StoreSizeManualAllocationTotal = ap.StoreSizeManualAllocationTotal;
				ahp.StoresWithAllocationCount = ap.GetCountOfStoresWithAllocation(new GeneralComponent(eComponentType.Total));
				ahp.HorizonOverride = ap.HorizonOverride;
				// end MID Track 4448 AnF Audit Enhancement
				// begin MID Track 3523 Allow Size Work Up Buy
				//ahp.IsDummy = ap.IsDummy;  //flag is part of the allocation type flags
				//ahp.IsPurchaseOrder = ap.IsPurchaseOrder; // flag is part of the allocation type flags
				// end MID Track 3523 Allow Size Work Up Buy
//				if (ap.Packs != null) 
//				{
//
//					foreach(PackHdr aPack in ap.Packs.Values)
//					{
//						ahp.Packs.Add(aPack.PackName, aPack);
//					}
//
//				}
//
//				if (ap.BulkColors != null) 
//				{
//
//					foreach (HdrColorBin aColor in ap.BulkColors.Values)
//					{
//						ahp.BulkColors.Add(aColor.ColorKey, aColor);
//					}
//
//				}
                // Assortment BEGIN 
                ahp.AsrtRID = ap.AsrtRID;
				ahp.HeaderType = ap.HeaderType;
                ahp.PlaceHolderRID = ap.PlaceHolderRID;
                ahp.AsrtType = ap.AsrtType;
                // Assortment END

                // BEGIN Workspace Usability Enhancement - Ron Matelic
                ahp.PackCount = ap.PackCount;
                ahp.BulkColorCount = ap.BulkColorCount;
                ahp.BulkColorSizeCount = ap.BulkColorSizeCount;
                // END Workspace Usability Enhancement 
     

                ahp.GradeSG_RID = ap.GradeSG_RID;       // TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
                ahp.GradeInventoryBasisHnRID = ap.GradeInventoryBasisHnRID; // TT#1287 - JEllis - Inventory Min Max
                ahp.GradeInventoryMinimumMaximum = ap.GradeInventoryMinimumMaximum; //TT#1287 - JEllis - Inventory min max

                // Begin TT#1401 - RMatelic - Reservation Stores
                ahp.ImoID = ap.ImoID;
                //ahp.TotalItemUnitsAllocated = ap.TotalItemUnitsAllocated;
                //ahp.TotalItemOrigUnitsAllocated = ap.TotalItemOrigUnitsAllocated;
                // End TT#1401 
				// Begin TT#1227 - RMatelic - Placeholders s/b sorted by a sequence number
                ahp.AsrtPlaceholderSeq = ap.AsrtPlaceholderSeq;
                ahp.AsrtHeaderSeq = ap.AsrtHeaderSeq;
                // End TT#1227
                // Begin TT#1966-MD - JSmith - DC Fulfillment
                ahp.MasterRID = ap.MasterRID;
                foreach (int subordinateRID in ap.SubordinateRIDs)
                {
                    ahp.SubordinateRIDs.Add(subordinateRID);
                }
                ahp.DCFulfillmentProcessed = ap.DCFulfillmentProcessed;
                // End TT#1966-MD - JSmith - DC Fulfillment

				this.Add(ahp);
			}
		}
		#endregion Methods
		
	}
	#endregion AllocationHeaderProfileList

	#region AllocationHeaderProfile
	[Serializable]
	public class AllocationHeaderProfile:Profile
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		private string _headerID;       				
		private string _headerDescription;				
		private DateTime _headerReceiptDay;                
		private DateTime _originalReceiptDay;           
		private double _unitRetail;
		private double _unitCost;
		private int _styleHnRID;						
		private int _planHnRID;
		private int _onHandHnRID;
		private string _vendor;                         
		private string _purchaseOrder;                  
		private DateTime _beginDay;                      
		private DateTime _shipDay;                   	 
		private DateTime _lastNeedDay;                      
		private DateTime _releaseApprovedDate;              
		private DateTime _releaseDate;
		private DateTime _earliestShipDay;
		private int _headerGroupRID;
		private int _sizeGroupRID;
		private int _workflowRID;
		private bool _workflowTrigger;
		private int _apiWorkflowRID;
		private bool _apiWorkflowTrigger;
		private int _methodRID;
		// status flags
		private AllocationTypeFlags _allocationTypeFlags;
		private AllocationStatusFlags _allocationStatusFlags;
		private BalanceStatusFlags _balanceStatusFlags;
		private ShippingStatusFlags _shippingStatusFlags;
		private IntransitUpdateStatusFlags _intransitUpdateStatusFlags;
		private bool _allocationStarted;
		private double _percentNeedLimit;
		private double _planPercentFactor;
		private int _reserveUnits;
		private int _allocatedUnits;
		private int _origAllocatedUnits;
		private int _rsvAllocatedUnits;
		private int _releaseCount;
		private int _gradeWeekCount;
		private int _primarySecondaryRID;
		private string _distributionCenter;
		private string _allocationNotes;
		private int _headerTotalQtyToAllocate;
		private int _headerTotalQtyAllocated;
		private int _headerTotalUnitMultiple;
        private int _headerTotalUnitMultipleDefault; // MID Track 5761 Allocation Multiple not saved on Database
		private int _unitsShipped;
		private int _genericTotalQtyToAllocate;
		private int _genericTotalQtyAllocated;
		private int _genericTotalUnitMultiple;
		private int _detailTotalQtyToAllocate;
		private int _detailTotalQtyAllocated;
		private int _detailTotalUnitMultiple;
		private int _bulkTotalQtyToAllocate;
		private int _bulkTotalQtyAllocated;
		private int _bulkTotalUnitMultiple;
		// if packs and colors are included, all class used to define these will need to be made Serializable
		private Hashtable _packs = null;
		private Hashtable _bulkColors = null;
		private eHeaderAllocationStatus _headerAllocationStatus;
		private eHeaderType _headerType;
		private eHeaderIntransitStatus _headerIntransitStatus;
		private eHeaderShipStatus _headerShipStatus;
		private HeaderCharProfileList _characteristics;
		private int _masterRID; 
		private string _masterID; 
		private int _subordinateRID;
        // Begin TT#1966-MD - JSmith- DC Fulfillment
        private List<int> _subordinateRIDs = null;
        // End TT#1966-MD - JSmith- DC Fulfillment
		private string _subordinateID;
		// MID Track 3523 Allow Size work up buy
		//private bool _isDummy;
		//private bool _isPurchaseOrder;
		// MID Track 3523 Allow Size work up buy
		private int _storeStyleAllocationManuallyChgdCnt; // MID Track 4448 ANF Audit Enhancement
		private int _storeSizeAllocationManuallyChgdCnt; // MID Track 4448 ANF Audit Enhancement
		private int _storeStyleManualAllocationTotal;    // MID Track 4448 AnF Audit Enhancement
		private int _storeSizeManualAllocationTotal;     // MID Track 4448 AnF Audit Enhancement
		private int _storesWithAllocationCnt;           // MID Track 4448 AnF Audit Enhancement
		private bool _horizonOverride;                   // MID Track 4448 AnF Audit Enhancement
		private int _asrtRID;
		private int _placeHolderRID;
		private int _asrtType;
        private int _packCount;                         // Workspace Usability Enhancement - Ron Matelic
        private int _bulkColorCount;                    // Workspace Usability Enhancement  
        private int _bulkColorSizeCount;                // Workspace Usability Enhancement  
        private string _nodeDisplayForOtsForecast; //TT#1313-MD -jsobek -Header Filters
        private string _nodeDisplayForOnHand; //TT#1313-MD -jsobek -Header Filters
        private string _nodeDisplayForGradeInvBasis; //TT#1313-MD -jsobek -Header Filters
        private string _workflowName; //TT#1313-MD -jsobek -Header Filters
        private string _headerMethodName; //TT#1313-MD -jsobek -Header Filters
        private string _apiWorkflowName; //TT#1313-MD -jsobek -Header Filters
		private List<HeaderPackRoundingOverride> _packRounding;		// TT#616 - stodd - pack Rounding
		private int _gradeSG_RID;                       // TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
        private bool _gradeInventoryMinMax;       // TT#1287 - JEllis - Inventory Minimum Maximum
        private int _gradeInventoryHnRID;         // TT#1287 - JEllis - Inventory Minimum Maximum
        private string _imoID;                    // Begin TT#1401 - RMatelic - Reservation Stores 
        private int _itemUnitsAllocated;
        private int _itemOrigUnitsAllocated;      // End TT#1401  
		private int _asrtPlaceholderSeq;				// TT#1227 - stodd - assortment
		private int _asrtHeaderSeq;						// TT#1227 - stodd - assortment
		private int _asrtUserRid;			// stodd manual merge;
        private string _asrtID; 		//TT#403 - MD - DOConnell - Assortment ID is not displaying correctly in the Allocation Workspace
        private int _asrtTypeForParentAsrt;	// TT#893 - MD - stodd - Add "Group Allocation ID" Column to Allocation Workspace
        private int _unitsPerCarton;        // TT#1652-MD- RMatelic - DC Carton Rounding
        private bool _DCFulfillmentProcessed = false;   // TT#1966-MD - JSmith- DC Fulfillment
        #endregion Fields
	
		#region Constructors
		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates a new instance of the Allocation Header Profile
		/// </summary>
		/// <param name="aHeaderID">The user assigned name of the allocation header.</param>
		/// <param name="aKey">A unique integer identifier for the allocation header.</param>
		/// <remarks>
		/// An allocation header profile describes an allocation header and is built from a subset 
		/// of the AllocationProfile.
		/// </remarks>
		public AllocationHeaderProfile(string aHeaderID, int aKey)
			:base(aKey)
		{
			if (aKey == -1)
			{
				Header hdr = new Header();
				DataTable dt = hdr.GetHeader(aHeaderID);
				if (dt.Rows.Count == 1)
				{
					System.Data.DataRow dr = dt.Rows[0];
					this.Key = Convert.ToInt32(dr["HDR_RID"], CultureInfo.CurrentUICulture);
					this.HeaderDescription = dr["HDR_DESC"].ToString();
				}
				_asrtRID = Include.NoRID;             // Assortment
				_placeHolderRID = Include.NoRID;      // Assortment
				_asrtPlaceholderSeq = Include.UndefinedPlaceholderSeq;	// TT#1227 - stodd - assortment
				_asrtHeaderSeq = Include.UndefinedHeaderSeq;			// TT#1227 - stodd - assortment
			}
			_headerID = aHeaderID;
			_masterID = string.Empty;
			_masterRID = Include.NoRID;
			_subordinateID = string.Empty;
			_subordinateRID = Include.NoRID;
            _asrtRID = Include.NoRID;             // Assortment
            _placeHolderRID = Include.NoRID;      // Assortment
            _gradeSG_RID = Include.AllStoreGroupRID;  // TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
            _gradeInventoryMinMax = false;  // TT#1287 - JEllis - Inventory Min Max
            _gradeInventoryHnRID = Include.NoRID; // TT#1287 - JEllis - Inventory Min Max
            // begin TT#2558 - JEllis - AnF Cancel Gets Error  
            _bulkTotalUnitMultiple = Include.DefaultUnitMultiple;
            _detailTotalUnitMultiple = Include.DefaultUnitMultiple;
            _genericTotalUnitMultiple = Include.DefaultUnitMultiple;
            _headerTotalUnitMultiple = Include.DefaultUnitMultiple;
            _headerTotalUnitMultipleDefault = Include.DefaultUnitMultiple;
            // end TT#2558 - JEllis - AnF Cancel Gets Error
		}

		/// <summary>
		/// Creates a new instance of the Allocation Header Profile
		/// </summary>
		/// <param name="aKey">A unique integer identifier for the allocation header.</param>
		/// <remarks>
		/// An allocation header profile describes an allocation header and is built from a subset 
		/// of the AllocationProfile.
		/// </remarks>
		public AllocationHeaderProfile(int aKey)
			:base(aKey)
		{

            //Begin TT#1313-MD -jsobek -Header Filters
            //Lets not go to the database everytime to get the Header ID
                //TT#2099- Begin - MD - Default columns disappear - rbeck
                //Header hdr = new Header();
                //this.HeaderID = hdr.GetHeaderID(aKey);
                //TT#2099- End - MD - Default columns disappear - rbeck
            //End TT#1313-MD -jsobek -Header Filters
            
            _masterID = string.Empty;
			_masterRID = Include.NoRID;
			_subordinateID = string.Empty;
			_subordinateRID = Include.NoRID;
            _asrtRID = Include.NoRID;       // Assortment
            _placeHolderRID = Include.NoRID;       // Assortment
            _gradeSG_RID = Include.AllStoreGroupRID;  // TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
            _gradeInventoryMinMax = false;  // TT#1287 - JEllis - Inventory Min Max
            _gradeInventoryHnRID = Include.NoRID; // TT#1287 - JEllis - Inventory Min Max
            // begin TT#2558 - JEllis - AnF Cancel Gets Error  
            _bulkTotalUnitMultiple = Include.DefaultUnitMultiple;
            _detailTotalUnitMultiple = Include.DefaultUnitMultiple;
            _genericTotalUnitMultiple = Include.DefaultUnitMultiple;
            _headerTotalUnitMultiple = Include.DefaultUnitMultiple;
            _headerTotalUnitMultipleDefault = Include.DefaultUnitMultiple;
            // end TT#2558 - JEllis - AnF Cancel Gets Error
        }
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
				return eProfileType.AllocationHeader;
			}
		}

		public Hashtable Packs
		{
			get
			{
				if (_packs == null)
				{
					CreatePacksHash();
				}
				return _packs;
			}
		}

		public Hashtable BulkColors
		{
			get
			{
				if (_bulkColors == null)
				{
					CreateBulkColorsHash();
				}
				return _bulkColors;
			}
		}

		/// <summary>
		/// Gets or sets the Header Group RID
		/// </summary>
		/// <remarks>
		/// Optional RID to connect to a header group
		/// </remarks>
		public int HeaderGroupRID
		{
			get
			{
				return _headerGroupRID;
			}
			set
			{
				_headerGroupRID = value;
			}
		}

		/// <summary>
		/// Gets or sets the Size Group RID
		/// need to verify that all pack and bulk sizes are contained in the size group
		/// </summary>
		public int SizeGroupRID
		{
			get
			{
				return _sizeGroupRID;
			}
			set
			{
				_sizeGroupRID = value;
			}
		}

		public int WorkflowRID
		{
			get
			{
				return _workflowRID;
			}
			set
			{
				_workflowRID = value;
			}
		}

		public bool WorkflowTrigger
		{
			get
			{
				return _workflowTrigger;
			}
			set
			{
				_workflowTrigger = value;
			}
		}
		/// <summary>
		/// WorkFlow RID at API time
		/// </summary>
		public int API_WorkflowRID
		{
			get
			{
				return _apiWorkflowRID;
			}
			set
			{
				_apiWorkflowRID = value;
			}
		}

		/// <summary>
		/// WorkFlow Trigger at API time
		/// </summary>
		public bool API_WorkflowTrigger
		{
			get
			{
				return _apiWorkflowTrigger;
			}
			set
			{
				_apiWorkflowTrigger = value;
			}
		}


		public int MethodRID
		{
			get
			{
				return _methodRID;
			}
			set
			{
				_methodRID = value;
			}
		}

		/// <summary>
		/// Gets or sets Header ID
		/// </summary>
		/// <remarks>
		/// A required, unique, user-assigned identifier for the allocation header.
		/// </remarks>
		public string HeaderID
		{
			get
			{
				return _headerID;
			}
			set
			{
				_headerID = value;
			}
		}

		/// <summary>
		/// Gets or sets Header Description
		/// </summary>
		/// <remarks>
		/// An optional user supplied description of the allocation header.
		/// </remarks>
		public string HeaderDescription
		{
			get
			{
				return _headerDescription;
			}
			set
			{
				_headerDescription = value;
			}
		}

        // BEGIN:  Assortment Header Type used for display purposes
        ///// <summary>
        ///// Gets the type of the header
        ///// </summary>
        //public eHeaderType HeaderType
        //{
        //    get
        //    {
        //        eHeaderType headerType;
        //        if (Receipt)
        //            headerType = eHeaderType.Receipt;
        //        else if (ASN)
        //            headerType = eHeaderType.ASN;
        //        else if (DropShip)
        //            headerType = eHeaderType.DropShip;
        //        else if (IsDummy)
        //            headerType = eHeaderType.Dummy;
        //        else if (MultiHeader)                 
        //            headerType = eHeaderType.MultiHeader;
        //        else if (Reserve)
        //            headerType = eHeaderType.Reserve;
        //        else if (WorkUpTotalBuy)
        //            headerType = eHeaderType.WorkupTotalBuy;
        //        else if (IsPurchaseOrder)
        //            headerType = eHeaderType.PurchaseOrder;
        //        else if (Assortment)
        //            headerType = eHeaderType.Assortment;
        //        else if (Placeholder)
        //            headerType = eHeaderType.Placeholder;
        //        else // set to "Receipt"
        //            headerType = eHeaderType.Receipt;
        //        return headerType;
        //    }
        //}
        // END:  Assortment Header Type used for display purposes

		/// <summary>
		/// Gets or Sets StyleHnRID
		/// </summary>
		/// <remarks>
		/// Required style RID from the product heirarchy.
		/// </remarks>
		public int StyleHnRID
		{
			get
			{
				return _styleHnRID;
			}
			set
			{
				_styleHnRID = value;
			}
		}

		/// <summary>
		/// Gets or sets the vendor
		/// </summary>
		/// <remarks>
		/// Optional vendor identification
		/// </remarks>
		public string Vendor
		{
			get
			{
				return _vendor;
			}
			set
			{
				_vendor = value;
			}
		}

		/// <summary>
		/// Gets or sets the Purchase Order
		/// </summary>
		/// <remarks>
		/// Optional purchase order identification
		/// </remarks>
		public string PurchaseOrder
		{
			get
			{
				return _purchaseOrder;
			}
			set
			{
				_purchaseOrder = value;
			}
		}

		/// <summary>
		/// Gets or sets HeaderDay.
		/// </summary>
		/// <remarks>
		/// Required, user-specified date when merchandise is expected in the distribution center
		/// </remarks>
		public DateTime HeaderDay
		{
			get
			{
				return _headerReceiptDay;
			}
			set
			{
				_headerReceiptDay = value;
			}
		}

		/// <summary>
		/// Gets the original receipt day.
		/// </summary>
		/// <remarks>
		/// Copy of the Receipt Day the first time the allocation header is saved to the database.
		/// </remarks>
		public DateTime OriginalReceiptDay
		{
			get
			{
				return _originalReceiptDay;
			}
			set
			{
				_originalReceiptDay = value;
			}
		}

		/// <summary>
		/// Gets or sets the Unit Retail
		/// </summary>
		/// <remarks>
		/// The retail value for one unit of merchandise on the allocation header.
		/// </remarks>
		public double UnitRetail
		{
			get
			{
				return _unitRetail;
			}
			set
			{
				_unitRetail = value;
			}
		}

		/// <summary>
		/// Gets or sets the Unit Cost
		/// </summary>
		/// <remarks>
		/// The Cost for one unit of merchandise on the allocation header.
		/// </remarks>
		public double UnitCost
		{
			get
			{
				return _unitCost;
			}
			set
			{
				_unitCost = value;
			}
		}

		/// <summary>
		/// Gets or sets ReserveUnits.
		/// </summary>
		public int ReserveUnits
		{
			get
			{
				return _reserveUnits;
			}
			set
			{
				_reserveUnits = value;
			}
		}

// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
		/// <summary>
		/// Gets or sets AllocatedUnits.
		/// </summary>
		public int AllocatedUnits
		{
			get
			{
				return _allocatedUnits;
			}

			set
			{
				_allocatedUnits = value;
			}
		}

		/// <summary>
		/// Gets or sets OrigAllocatedUnits.
		/// </summary>
		public int OrigAllocatedUnits
		{
			get
			{
				return _origAllocatedUnits;
			}
			set
			{
				_origAllocatedUnits = value;
			}
		}

		/// <summary>
		/// Gets or sets RsvAllocatedUnits.
		/// </summary>
		public int RsvAllocatedUnits
		{
			get
			{
				return _rsvAllocatedUnits;
			}
			set
			{
				_rsvAllocatedUnits = value;
			}
		}

		/// <summary>
		/// Gets or sets ReleaseCount.
		/// </summary>
		public int ReleaseCount
		{
			get
			{
				return _releaseCount;
			}
			set
			{
				_releaseCount = value;
			}
		}
// (CSMITH) - END MID Track #3219

		/// <summary>
		/// Gets or sets GradeWeekCount.
		/// </summary>
		public int GradeWeekCount
		{
			get
			{
				return _gradeWeekCount;
			}
			set
			{
				_gradeWeekCount = value;
			}
		}

		/// <summary>
		/// Gets or sets PrimarySecondaryRID.
		/// </summary>
		public int PrimarySecondaryRID
		{
			get
			{
				return _primarySecondaryRID;
			}
			set
			{
				_primarySecondaryRID = value;
			}
		}

		/// <summary>
		/// Gets or sets DistributionCenter.
		/// </summary>
		public string DistributionCenter
		{
			get
			{
				return _distributionCenter;
			}
			set
			{
				_distributionCenter = value;
			}
		}

		/// <summary>
		/// Gets or sets AllocationNotes.
		/// </summary>
		public string AllocationNotes
		{
			get
			{
				return _allocationNotes;
			}
			set
			{
				_allocationNotes = value;
			}
		}

		/// <summary>
		/// Gets or sets percent need limit.
		/// </summary>
		public double PercentNeedLimit
		{
			get
			{
				return _percentNeedLimit;
			}
			set
			{
				_percentNeedLimit = value;
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
		public DateTime BeginDay
		{
			get
			{
				return _beginDay;
			}
			set
			{
				_beginDay = value;
			}
		}

		/// <summary>
		/// Gets or sets the Ship Day for the Shipping Horizon.
		/// </summary>
		/// <remarks>
		/// <para>Optional, user-specified ending or target date for the shipping horizon.</para>
		/// <para>When specified, all stores use the same ship day.</para>
		/// <para>When not specified, every store's ship day is dependent on its lead time and picking-shipping schedule.</para>
		/// </remarks>
		public DateTime ShipToDay
		{
			get
			{
				return _shipDay;
			}
			set 
			{
				_shipDay = value;
			}
		}

		/// <summary>
		/// Gets or sets the Last Need Day
		/// </summary>
		/// <remarks>
		/// A general audit of a need allocation.  This date represents the last
		/// inventory target date used to allocate by need. This date starts out
		/// equal to the ship date and is increased as required to get the merchandise
		/// to the stores.
		/// </remarks>
		public DateTime LastNeedDay
		{
			get
			{
				return _lastNeedDay;
			}
			set
			{
				_lastNeedDay = value;
			}
		}

		/// <summary>
		/// Gets or sets the release approved date
		/// </summary>
		/// <remarks>
		/// The date and time when the allocation header was approved for release. 
		/// The actual release may occur at a different date and time.
		/// </remarks>
		public DateTime ReleaseApprovedDate
		{
			get
			{
				return _releaseApprovedDate;
			}
			set
			{
				_releaseApprovedDate = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the release date of the allocation header.
		/// </summary>
		/// <remarks>
		/// The date and time when the allocation header was released 
		/// for picking and shipping to the stores.
		/// </remarks>
		public DateTime ReleaseDate
		{
			get
			{
				return _releaseDate;
			}
			set 
			{
				_releaseDate = value;
			}
		}

		/// <summary>
		/// Gets or sets the earliest ship date of the allocation header.
		/// </summary>
		public DateTime EarliestShipDay
		{
			get
			{
				return _earliestShipDay;
			}
			set 
			{
				_earliestShipDay = value;
			}
		}

		/// <summary>
		/// Gets or sets the allocation status flags for the header.
		/// </summary>
		public AllocationStatusFlags AllocationStatusFlags
		{
			get { return _allocationStatusFlags ; }
			set { _allocationStatusFlags = value; }
		}
		/// <summary>
		/// Gets or sets the balance status flags for the header.
		/// </summary>
		public BalanceStatusFlags BalanceStatusFlags
		{
			get { return _balanceStatusFlags ; }
			set { _balanceStatusFlags = value; }
		}
		/// <summary>
		/// Gets or sets the shipping status flags for the header.
		/// </summary>
		public ShippingStatusFlags ShippingStatusFlags
		{
			get { return _shippingStatusFlags ; }
			set { _shippingStatusFlags = value; }
		}
		/// <summary>
		/// Gets or sets the allocation type flags for the header.
		/// </summary>
		public AllocationTypeFlags AllocationTypeFlags
		{
			get { return _allocationTypeFlags ; }
			set { _allocationTypeFlags = value; }
		}
		/// <summary>
		/// Gets or sets the intransit status flags for the header.
		/// </summary>
		public IntransitUpdateStatusFlags IntransitUpdateStatusFlags
		{
			get { return _intransitUpdateStatusFlags ; }
			set { _intransitUpdateStatusFlags = value; }
		}

		
		/// <summary>
		/// Gets the BulkIsDetail flag value.
		/// </summary>
		public bool BulkIsDetail
		{
			get
			{
				return _allocationTypeFlags.BulkIsDetail;
			}
		}

		/// <summary>
		/// Gets allocation started status flag value.
		/// </summary>
		public bool AllocationStarted
		{
			get
			{
				return _allocationStarted;
			}
			set
			{
				_allocationStarted = value;
			}
		}

				 
		/// <summary>
		/// Gets ReceivedInBalance flag
		/// </summary>
		public bool ReceivedInBalance
		{
			get
			{
				return _allocationStatusFlags.ReceivedInBalance;
			}
		}

		/// <summary>
		/// Gets or sets BottomUpSizePerformed flag.
		/// </summary>
		public bool BottomUpSizePerformed
		{	
			get 
			{
				return _allocationStatusFlags.BottomUpSizePerformed;
			}
		}
		

		/// <summary>
		/// Gets or sets RulesDefinedAndProcessed flag
		/// </summary>
		public bool RulesDefinedAndProcessed
		{
			get
			{
                return _allocationStatusFlags.RulesDefinedAndProcessed;
			}
		}

		/// <summary>
		/// Gets or sets NeedAllocationPerformed flag
		/// </summary>
		public bool NeedAllocationPerformed
		{
			get
			{
				return _allocationStatusFlags.NeedAllocationPerformed;
			}
		}

		/// <summary>
		/// Gets PackBreakoutByContent flag
		/// </summary>
		public bool PackBreakoutByContent
		{
			get
			{
				return _allocationStatusFlags.PackBreakoutByContent;
			}
		}

		/// <summary>
		/// Gets BulkSizeBreakoutPerformed
		/// </summary>
		public bool BulkSizeBreakoutPerformed
		{
			get
			{
				return _allocationStatusFlags.BulkSizeBreakoutPerformed;
			}
		}

		/// <summary>
		/// Gets ReleaseApproved flag
		/// </summary>
		public bool ReleaseApproved
		{
			get
			{
				return _allocationStatusFlags.ReleaseApproved;
			}
		}

		/// <summary>
		/// Gets Released flag
		/// </summary>
		public bool Released
		{
			get
			{
				return _allocationStatusFlags.Released;
			}
		}
        // begin TT#246 - MD - JEllis - AnF VSW Size In Store Minimums
        // end TT#246 - MD - JEllis - AnF VSW Size in Store Minimums
		/// <summary>
		/// Gets ShippingStarted flag
		/// </summary>
		public bool ShippingStarted
		{
			get
			{
				return _shippingStatusFlags.ShippingStarted;
			}
		}

		/// <summary>
		/// Gets ShippingComplete flag
		/// </summary>
		public bool ShippingComplete
		{
			get
			{
				return _shippingStatusFlags.ShippingComplete;
			}
		}

		/// <summary>
		/// Gets ShippingOnHold audit flag.
		/// </summary>
		public bool ShippingOnHold
		{
			get
			{
				return _shippingStatusFlags.ShippingOnHold;
			}
		}
        
		/// <summary>
		/// Gets SizeReceiptsBalanceToColor
		/// </summary>
		public bool SizeReceiptsBalanceToColor
		{
			get
			{
				return _balanceStatusFlags.SizeReceiptsBalanceToColor;
			}
		}

		/// <summary>
		/// Gets ColorReceiptsBalanceToBulk
		/// </summary>
		public bool ColorReceiptsBalanceToBulk
		{
			get
			{
				return _balanceStatusFlags.ColorReceiptsBalanceToBulk;
			}
		}

		/// <summary>
		/// Gets PackSizeBalanceToPackColor
		/// </summary>
		public bool PackSizeBalanceToPackColor
		{
			get
			{
				return _balanceStatusFlags.PackSizesBalanceToPackColor;
			}
		}

		/// <summary>
		/// Gets PackColorBalanceToPack
		/// </summary>
		public bool PackColorBalanceToPack
		{
			get
			{
				return _balanceStatusFlags.PackColorsBalanceToPack;
			}
		}

		/// <summary>
		/// Gets PackPlusBulkReceiptsBalanceToTotal
		/// </summary>
		public bool PackPlusBulkReceiptsBalanceToTotal
		{
			get
			{
				return _balanceStatusFlags.PackPlusBulkReceiptsBalanceToTotal;
			}
		}

		/// <summary>
		/// Gets PackAllocationInBalance
		/// </summary>
		public bool PackAllocationInBalance
		{
			get
			{
				return _balanceStatusFlags.PackAllocationInBalance;
			}
		}

		/// <summary>
		/// Gets BulkColorAllocationInBalance
		/// </summary>
		public bool BulkColorAllocationInBalance
		{
			get
			{
				return _balanceStatusFlags.BulkColorAllocationInBalance;
			}
		}

		/// <summary>
		/// Gets BulkPlusPackAllocationInBalance
		/// </summary>
		public bool BulkPlusPackAllocationInBalance
		{
			get
			{
				return _balanceStatusFlags.BulkPlusPackAllocationInBalance;
			}
		}

		/// <summary>
		/// Gets BulkSizeAllocationInBalance
		/// </summary>
		public bool BulkSizeAllocationInBalance
		{
			get 
			{
				return _balanceStatusFlags.BulkSizeAllocationInBalance;
			}
		}
        
		/// <summary>
		/// Gets StyleIntransitUpdated
		/// </summary>
		public bool StyleIntransitUpdated
		{
			get
			{
				return _intransitUpdateStatusFlags.StyleIntransitUpdated;
			}
		}

		/// <summary>
		/// Gets BulkColorIntransitUpdated
		/// </summary>
		public bool BulkColorIntransitUpdated
		{
			get
			{
                return _intransitUpdateStatusFlags.BulkColorIntransitUpdated;
			}
		}

		/// <summary>
		/// Gets BulkSizeIntransitUpdated
		/// </summary>
		public bool BulkSizeIntransitUpdated
		{
			get
			{
				return _intransitUpdateStatusFlags.BulkSizeIntransitUpdated;
			}
		}

		/// <summary>
		/// Gets or sets PlanHnRID
		/// </summary>
		/// <remarks>
		/// Identifies the merchandise plan source.
		/// </remarks>
		public int PlanHnRID
		{
			get
			{
				return _planHnRID;
			}
			set
			{
				_planHnRID = value;
			}
		}

		/// <summary>
		/// Gets or sets OnHandHnRID
		/// </summary>
		/// <remarks>
		/// Identifies the merchandise OnHand source. 
		/// </remarks>
		public int OnHandHnRID
		{
			get
			{
				return _onHandHnRID;
			}
			set
			{
				_onHandHnRID = value;
			}
		}

		/// <summary>
		/// Gets or sets PlanFactor
		/// </summary>
		public double PlanFactor
		{
			get
			{
				return _planPercentFactor;
			}
			set
			{
				_planPercentFactor = value;
			}
		}
		
		/// <summary>
		/// Gets or sets Total Units To Allocate
		/// </summary>
		public int TotalUnitsToAllocate
		{
			get
			{
				return _headerTotalQtyToAllocate;
			}
			set
			{
				_headerTotalQtyToAllocate = value;
			}
		}

		/// <summary>
		/// Gets or sets TotalUnitsAllocated
		/// </summary>
		public int TotalUnitsAllocated
		{
			get
			{
				return _headerTotalQtyAllocated;
			}
			set
			{
				_headerTotalQtyAllocated = value;
			}
		}

		/// <summary>
		/// Gets total UnitsShipped
		/// </summary>
		public int UnitsShipped
		{
			get
			{
				return _unitsShipped;
			}
			set
			{
				_unitsShipped = value;
			}
		}

		/// <summary>
		/// Gets or sets Allocation Multiple
		/// </summary>
		public int AllocationMultiple
		{
			get
			{
				return _headerTotalUnitMultiple;
			}
			set
			{
				_headerTotalUnitMultiple = value;
			}
		}

        // begin MID Track 5761 Allocation Multiple not saved on DB
        /// <summary>
        /// Gets the Allocation Multiple that should be displayed on the Workspace Explorer
        /// </summary>
        public int AllocationMultipleDsply
        {
            get
            {
                if (AllocationMultiple > 1)
                {
                    return AllocationMultiple;
                }
                return AllocationMultipleDefault;
            }
        }
        /// <summary>
        /// Gets or sets Allocation Multiple Default
        /// </summary>
        public int AllocationMultipleDefault
        {
            get
            {
                return _headerTotalUnitMultipleDefault;
            }
            set
            {
                _headerTotalUnitMultipleDefault = value;
            }
        }
        // end MID Track 5761 Allocation Multiple not saved on DB

		/// <summary>
		/// Gets Generic Units To Allocate
		/// </summary>
		public int GenericUnitsToAllocate
		{
			get
			{
				return _genericTotalQtyToAllocate;
			}
			set
			{
				_genericTotalQtyToAllocate = value;
			}
		}

		/// <summary>
		/// Gets or sets GenericUnitsAllocated
		/// </summary>
		public int GenericUnitsAllocated
		{
			get
			{
				return _genericTotalQtyAllocated;
			}
			set
			{
				_genericTotalQtyAllocated = value;
			}
		}

		/// <summary>
		/// Gets or sets Generic Multiple
		/// </summary>
		public int GenericMultiple
		{
			get
			{
				return _genericTotalUnitMultiple;
			}
			set
			{
				_genericTotalUnitMultiple = value;
			}
		}

		/// <summary>
		/// Gets or sets Detail Type Units To Allocate
		/// </summary>
		public int DetailTypeUnitsToAllocate
		{
			get
			{
				return _detailTotalQtyToAllocate;
			}
			set
			{
				_detailTotalQtyToAllocate = value;
			}
		}

		/// <summary>
		/// Gets or sets DetailTypeUnitsAllocated
		/// </summary>
		public int DetailTypeUnitsAllocated
		{
			get
			{
				return _detailTotalQtyAllocated;
			}
			set
			{
				_detailTotalQtyAllocated = value;
			}
		}

		/// <summary>
		/// Gets or sets DetailTypeMultiple
		/// </summary>
		public int DetailTypeMultiple
		{
			get
			{
				return _detailTotalUnitMultiple;
			}
			set
			{
				_detailTotalUnitMultiple = value;
			}
		}

		/// <summary>
		/// Gets or sets Bulk Units To Allocate
		/// </summary>
		public int BulkUnitsToAllocate
		{
			get
			{
				return _bulkTotalQtyToAllocate;
			}
			set
			{
				_bulkTotalQtyToAllocate = value;
			}
		}

		/// <summary>
		/// Gets or sets BulkUnitsAllocated
		/// </summary>
		public int BulkUnitsAllocated
		{
			get
			{
				return _bulkTotalQtyAllocated;
			}
			set
			{
				_bulkTotalQtyAllocated = value;
			}
		}

		/// <summary>
		/// Gets or sets Bulk Multiple
		/// </summary>
		public int BulkMultiple
		{
			get
			{
				return _bulkTotalUnitMultiple;
			}
			set
			{
				_bulkTotalUnitMultiple = value;
			}
		}

		/// <summary>
		/// Gets or sets Header Allocation Status
		/// </summary>
		public eHeaderAllocationStatus HeaderAllocationStatus 
		{
			get
			{
				return _headerAllocationStatus;
			}
			set
			{
				_headerAllocationStatus = value;
			}
		}
		
		/// <summary>
		/// Gets IsDummy flag
		/// </summary>
		public bool IsDummy
		{
			// begin MID Track 3523 Allow Size Work Up Buy
			get
			{
				return _allocationTypeFlags.IsDummy;
			}
		}

		/// <summary>
		/// Gets IsPurchaseOrder flag
		/// </summary>
		public bool IsPurchaseOrder
		{
			get
			{
				return _allocationTypeFlags.PurchaseOrder;
			}

		}

		/// <summary>
		/// Gets or sets Reserve flag
		/// </summary>
		public bool Reserve
		{
			get
			{
				return _allocationTypeFlags.Reserve;
			}
		}

		/// <summary>
		/// Gets the MultiHeader flag value
		/// </summary>
		public bool MultiHeader
		{
			get
			{
				return _allocationTypeFlags.MultiHeader;
			}
		}

		/// <summary>
		/// Gets or sets the Receipt flag value
		/// </summary>
		public bool Receipt
		{
			get
			{
				return _allocationTypeFlags.Receipt;
			}
		}

		/// <summary>
		/// Gets the ASN flag value.
		/// </summary>
		public bool ASN
		{
			get
			{
				return _allocationTypeFlags.ASN;
			}
		}

		/// <summary>
		/// Gets the DropShip flag value.
		/// </summary>
		public bool DropShip
		{
			get
			{
				return _allocationTypeFlags.DropShip;
			}
		}

		/// <summary>
		/// Gets the InUseByMulti flag value.
		/// </summary>
		public bool InUseByMulti
		{
			get
			{
				return _allocationTypeFlags.InUseByMulti;
			}
		}

		/// <summary>
		/// Gets WorkUpBulkSizeBuy flag
		/// </summary>
		public bool WorkUpBulkSizeBuy
		{
			get
			{
				return _allocationTypeFlags.WorkUpBulkSizeBuy;
			}
		}

		/// <summary>
		/// Gets WorkUpBulk flag
		/// </summary>
		public bool WorkUpBulkBuy
		{
			get
			{
				return _allocationTypeFlags.WorkUpBulkBuy;
			}

		}

		/// <summary>
		/// Gets WorkUpTotalBuy flag.
		/// </summary>
		public bool WorkUpTotalBuy
		{
			get
			{
				return _allocationTypeFlags.WorkUpTotalBuy;
			}
		}

        // BEGIN Assortment--J.Ellis
        /// <summary>
        /// Gets Assortment flag
        /// </summary>
        public bool Assortment
        {
            get
            {
                return _allocationTypeFlags.Assortment;
            }
        }

        /// <summary>
        /// Gets Placeholder flag
        /// </summary>
        public bool Placeholder
        {
            get
            {
                return _allocationTypeFlags.Placeholder;
            }
         }

        // begin TT#2225 - JEllis - AnF VSW Fwos Enhancement
        /// <summary>
        /// Gets AdjustVSW_OnHand flag
        /// </summary>
        public bool AdjustVSW_OnHand
        {
            get
            {
                return _allocationTypeFlags.AdjustVSW_OnHand;
            }
        }
        // end TT#2225 - JEllis - AnF VSW Fwos Enhancement
        // END Assortment--J.Ellis

		// BEGIN MID Track #4357 - security for interfaced and non-interfaced headers
		/// <summary>
		/// Gets IsInterfaced flag.
		/// </summary>
		public bool IsInterfaced
		{
			get
			{
				return _allocationTypeFlags.IsInterfaced;
			}
		}
		// END MID Track #4357

		/// <summary>
		/// Gets the header allocation status
		/// </summary>
		public eHeaderAllocationStatus DetermineHeaderAllocationStatus
		{
			get
			{
				if (this.ReceivedInBalance != true)
				{
					return eHeaderAllocationStatus.ReceivedOutOfBalance;
				}
				if (this.InUseByMulti)
				{
					return eHeaderAllocationStatus.InUseByMultiHeader;
				}
				//				if (this.Released || this.ReleaseApproved)

				if (this.Released)
				{
					return eHeaderAllocationStatus.Released;
				}
				if (this.ReleaseApproved)
				{
					return eHeaderAllocationStatus.ReleaseApproved;
				}
				if (this.AllocationStarted)
				{
					if (this.BulkSizeBreakoutPerformed)
					{
						if (this.BulkSizeAllocationInBalance)
						{
							return eHeaderAllocationStatus.AllInBalance;
						}
						return eHeaderAllocationStatus.SizesOutOfBalance;
					}
					if (this.BulkPlusPackAllocationInBalance)
					{
						if (this.BulkColors.Count == 0)
						{
							return eHeaderAllocationStatus.AllInBalance;
						}
						foreach (HdrColorBin hcb in this.BulkColors.Values)
						{
							if (hcb.ColorSizes.Count != 0)
							{
								return eHeaderAllocationStatus.AllocatedInBalance;
							}
						}
						return eHeaderAllocationStatus.AllInBalance;
					}
					if (this.NeedAllocationPerformed)
					{
						return eHeaderAllocationStatus.AllocatedOutOfBalance;
					}
					if (this.RulesDefinedAndProcessed)
					{
						return eHeaderAllocationStatus.AllocatedOutOfBalance;
					}
					if (this.BottomUpSizePerformed)
					{
						if (this.BulkSizeAllocationInBalance)
						{
							return eHeaderAllocationStatus.PartialSizeInBalance;
						}
						return eHeaderAllocationStatus.PartialSizeOutOfBalance;
					}
					return eHeaderAllocationStatus.AllocatedOutOfBalance;
				}
				if (this.ReceivedInBalance)
				{
					return eHeaderAllocationStatus.ReceivedInBalance;
				}
				return eHeaderAllocationStatus.ReceivedOutOfBalance;
			}
		}
		public eHeaderType HeaderType
		{
			get { return _headerType; }
			set { _headerType = value; }
		}

		/// <summary>
		/// Gets or sets HeaderIntransitStatus.
		/// </summary>
		public eHeaderIntransitStatus HeaderIntransitStatus
		{
			get { return _headerIntransitStatus; }
			set { _headerIntransitStatus = value; }
		}

		/// <summary>
		/// Gets or sets HeaderShipStatus.
		/// </summary>
		public eHeaderShipStatus HeaderShipStatus
		{
			get { return _headerShipStatus; }
			set { _headerShipStatus = value; }
		}

		/// <summary>
		/// Gets or sets header characteristics.
		/// </summary>
		public HeaderCharProfileList Characteristics
		{
			get { return _characteristics; }
			set { _characteristics = value; }
		}

		/// <summary>
		/// Gets or sets header MasterRID.
		/// </summary>
		public int MasterRID
		{
			get { return _masterRID; }
			set { _masterRID = value; }
		}

		/// <summary>
		/// Gets or sets header MasterID.
		/// </summary>
		public string MasterID
		{
			get { return _masterID; }
			set { _masterID = value; }
		}

		/// <summary>
		/// Gets or sets header SubordinateRID.
		/// </summary>
		public int SubordinateRID
		{
			get { return _subordinateRID; }
			set { _subordinateRID = value; }
		}

		/// <summary>
		/// Gets or sets header SubordinateID.
		/// </summary>
		public string SubordinateID
		{
			get { return _subordinateID; }
			set { _subordinateID = value; }
		}

        // Begin TT#1966-MD - JSmith- DC Fulfillment
        /// <summary>
        /// Gets the Subordinate RIDs for the subordinate headers of this header when this header is a master header
        /// </summary>
        public List<int> SubordinateRIDs
        {
            get
            {
                if (_subordinateRIDs == null)
                {
                    _subordinateRIDs = new List<int>();
                    
                }
                return _subordinateRIDs;
            }
        }
        public bool IsMasterHeader
        {
            get { return SubordinateRIDs.Count > 0; }
        }

        public bool IsSubordinateHeader
        {
            get { return MasterRID != Include.NoRID; }
        }

        public bool DCFulfillmentProcessed
        {
            get { return _DCFulfillmentProcessed; }
            set { _DCFulfillmentProcessed = value; }
        }
        // End TT#1966-MD - JSmith- DC Fulfillment

		// begin MID Track 4448 AnF Audit Enhancement
		/// <summary>
		/// Gets or sets the number of stores having an allocation
		/// </summary>
		public int StoresWithAllocationCount
		{
			get
			{
				return this._storesWithAllocationCnt;
			}
			set
			{
				this._storesWithAllocationCnt = value;
			}
		}
        /// <summary>
		/// Gets or sets the total store style allocation of the manually changed stores.
		/// </summary>
		public int StoreStyleManualAllocationTotal
		{
			get
			{
				return this._storeStyleManualAllocationTotal;
			}
			set
			{
				this._storeStyleManualAllocationTotal = value;
			}
		}
		/// <summary>
		/// Gets or sets the total store size allocation of the manually changed store sizes
		/// </summary>
		public int StoreSizeManualAllocationTotal
		{
			get
			{
				return this._storeSizeManualAllocationTotal;
			}
			set
			{
				this._storeSizeManualAllocationTotal = value;
			}
		}
		/// <summary>
		/// Gets or sets the number of stores whose style/color allocation has been manually changed
		/// </summary>
		public int StoreStyleAllocationManuallyChangedCount
		{
			get
			{
				return this._storeStyleAllocationManuallyChgdCnt;
			}
			set
			{
				this._storeStyleAllocationManuallyChgdCnt = value;
			}
		}
		/// <summary>
		/// Gets or sets the number of stores whose color-size allocation has been manually changed
		/// </summary>
		public int StoreSizeAllocationManuallyChangedCount
		{
			get
			{
				return this._storeSizeAllocationManuallyChgdCnt;
			}
			set
			{
				this._storeSizeAllocationManuallyChgdCnt = value;
			}
		}
		/// <summary>
		/// Gets or sets Horizon Override Indicator:  True: Horizon has been overridden; False: Horizon is the default.
		/// </summary>
		public bool HorizonOverride
		{
			get
			{
				return this._horizonOverride;
			}
			set
			{
				this._horizonOverride = value;
			}
		}
		// end MID Track 4448 AnF Audit Enhancement

		/// <summary>
		/// Gets or sets Assortment RID.
		/// </summary>
		public int AsrtRID
		{
			get { return _asrtRID; }
			set { _asrtRID = value; }
		}
		/// <summary>
		/// Gets or sets PlaceHolder RID.
		/// </summary>
		public int PlaceHolderRID
		{
			get { return _placeHolderRID; }
			set { _placeHolderRID = value; }
		}
		/// <summary>
		/// Gets or sets Assortment Type.
		/// </summary>
		public int AsrtType
		{
			get { return _asrtType; }
			set { _asrtType = value; }
		}
        // BEGIN Workspace Usability Enhancement - Ron Matelic
        /// <summary>
        /// Gets or sets Pack Count.
        /// </summary>
        public int PackCount
        {
            get { return _packCount; }
            set { _packCount = value; }
        }
        /// <summary>
        /// Gets or sets Bulk Color Count.
        /// </summary>
        public int BulkColorCount
        {
            get { return _bulkColorCount; }
            set { _bulkColorCount = value; }
        }
        /// <summary>
        /// Gets or sets Bulk Color Size Count.
        /// </summary>
        public int BulkColorSizeCount
        {
            get { return _bulkColorSizeCount; }
            set { _bulkColorSizeCount = value; }
        }
        // END Workspace Usability Enhancement  

        //Begin TT#1313-MD -jsobek -Header Filters -used for performance
        public string NodeDisplayForOtsForecast
        {
            get { return _nodeDisplayForOtsForecast; }
            set { _nodeDisplayForOtsForecast = value; }
        }
        public string NodeDisplayForOnHand
        {
            get { return _nodeDisplayForOnHand; }
            set { _nodeDisplayForOnHand = value; }
        }
        public string NodeDisplayForGradeInvBasis
        {
            get { return _nodeDisplayForGradeInvBasis; }
            set { _nodeDisplayForGradeInvBasis = value; }
        }
        public string WorkflowName
        {
            get { return _workflowName; }
            set { _workflowName = value; }
        }
        public string HeaderMethodName
        {
            get { return _headerMethodName; }
            set { _headerMethodName = value; }
        }
        public string APIWorkflowName
        {
            get { return _apiWorkflowName; }
            set { _apiWorkflowName = value; }
        }
        //End TT#1313-MD -jsobek -Header Filters

		// Begin TT#616 - stodd - pack rounding
		public List<HeaderPackRoundingOverride> PackRounding
		{
			get 
			{
				if (_packRounding == null)
				{
					_packRounding = new List<HeaderPackRoundingOverride>();
				}
				return _packRounding; 
			}
			set { _packRounding = value; }
		}
		// End TT#616 - stodd - pack rounding

        // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
        /// <summary>
        /// Gets or sets the Grade Store Group RID
        /// </summary>
        public int GradeSG_RID
        {
            get { return _gradeSG_RID; }
            set { _gradeSG_RID = value; }
        }
        // End TT#618
        // begin TT#1287 - JEllis - Inventory Minimum and Maximum
        /// <summary>
        /// Gets or sets bool that indicates whether grade Minmums/Maximums are Inventory or Allocation Minimums/Maximums
        /// </summary>
        public bool GradeInventoryMinimumMaximum
        {
            get
            {
                return _gradeInventoryMinMax;
            }
            set
            {
                _gradeInventoryMinMax = value;
            }
        }
        /// <summary>
        /// Gets or sets the Inventory Basis HnRID used to calculate an Inventory Minimum or Maximum
        /// </summary>
        public int GradeInventoryBasisHnRID
        {
            get
            {
                return _gradeInventoryHnRID;
            }
            set
            {
                _gradeInventoryHnRID = value;
            }
        }
        // end TT#1287 - JEllis - Inventory Minimum and Maximum

        // Begin TT#1401 - RMatelic - Reservation Stores
        /// <summary>
        /// Gets or sets the IMO ID
        /// </summary>
        public string ImoID
        {
            get { return _imoID; }
            set { _imoID = value; }
        }

        /// <summary>
        /// Gets or sets the ItemUnitsAllocated  >>> added 'Total' prefix to match AllocationProfile
        /// </summary>
        public int TotalItemUnitsAllocated
        {
            get { return _itemUnitsAllocated; }
            set { _itemUnitsAllocated = value; }
        }

        /// <summary>
        /// Gets or sets the ItemUnitsAllocated  >>> added 'Total' prefix to match AllocationProfile
        /// </summary>
        public int TotalItemOrigUnitsAllocated
        {
            get { return _itemOrigUnitsAllocated; }
            set { _itemOrigUnitsAllocated = value; }
        }
        // End TT#1401 
		
		// Begin TT#1227 - stodd - assortment
		public int AsrtPlaceholderSeq
		{
			get { return _asrtPlaceholderSeq; }
			set { _asrtPlaceholderSeq = value; }
		}
		
		public int AsrtHeaderSeq
		{
			get { return _asrtHeaderSeq; }
			set { _asrtHeaderSeq = value; }
		}
		// End TT#1227


		public int AsrtUserRid
		{
			get { return _asrtUserRid; }
			set { _asrtUserRid = value; }
		}
		
		//BEGIN TT#403 - MD - DOConnell - Assortment ID is not displaying correctly in the Allocation Workspace
        /// <summary>
        /// Gets or sets the Assortment ID
        /// </summary>
        public string AssortmentID
        {
            get { return _asrtID; }
            set { _asrtID = value; }
        }
		//END TT#403 - MD - DOConnell - Assortment ID is not displaying correctly in the Allocation Workspace

		// Begin TT#893 - MD - stodd - Add "Group Allocation ID" Column to Allocation Workspace
        public int AsrtTypeForParentAsrt
        {
            get { return _asrtTypeForParentAsrt; }
            set { _asrtTypeForParentAsrt = value; }
        }
		// End TT#893 - MD - stodd - Add "Group Allocation ID" Column to Allocation Workspace

        // Begin TT#1652-MD - RMatelic - DC Carton Rounding
        public int UnitsPerCarton
        {
            get { return _unitsPerCarton; }
            set { _unitsPerCarton = value; }
        }
        // End TT#1652-MD 
		
		#endregion Properties

		#region Methods
		//========
		// METHODS
		//========
	
		#region CreateHashtables
		/// <summary>
		/// Creates pack hashtable.
		/// </summary>
		private void CreatePacksHash()
		{
			_packs = new Hashtable();
//			_genericPacks = new ArrayList();
//			_nonGenericPacks = new ArrayList();
		}

		/// <summary>
		/// Creates Bulk Color Hashtable.
		/// </summary>
		private void CreateBulkColorsHash()
		{
			_bulkColors = new Hashtable();
		}
		#endregion CreateHashtables

		#endregion Methods
	}
	#endregion AllocationHeaderProfile

	/// <summary>
	/// Contains the information about the packs of a Header
	/// </summary>
	[Serializable()]
	public class HeaderPackProfile:Profile
	{
		// Fields
		private int							_headerRID;
		private string						_headerPackName;
		private int							_packs;
		private int							_multiple;
		private int							_reservePacks;
		private bool						_genericInd;
		private int							_associatedPackRID;
        private int                         _sequence;          // Assortment
		private Hashtable					_colors;
        private List<int> _associatedPackRIDs = null;  // TT#1966-MD - JSmith - DC Fulfillment
        private string _associatedPackName;  // TT#2119-MD - JSmith - Cancelling allocation on subordinate headers on master runs forever if subordinates have same pack names with different size runs

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HeaderPackProfile(int aKey)
		:base(aKey)
		{
			_colors = new Hashtable();
		}
		
		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.HeaderPack;
			}
		}
		/// <summary>
		/// Gets or sets the record ID of the header to which the pack belongs.
		/// </summary>
		public int HeaderRID 
		{
			get { return _headerRID ; }
			set { _headerRID = value; }
		}
		/// <summary>
		/// Gets or sets the ID (name) of the pack name.
		/// </summary>
		public string HeaderPackName 
		{
			get { return _headerPackName ; }
			set { _headerPackName = value; }
		}
		/// <summary>
		/// Gets or sets the pack count.
		/// </summary>
		public int Packs 
		{
			get { return _packs ; }
			set { _packs = value; }
		}
		/// <summary>
		/// Gets or sets the pack multiple.
		/// </summary>
		public int Multiple 
		{
			get { return _multiple ; }
			set { _multiple = value; }
		}
		/// <summary>
		/// Gets or sets the number of packs to keep in reserve.
		/// </summary>
		public int ReservePacks 
		{
			get { return _reservePacks ; }
			set { _reservePacks = value; }
		}
		/// <summary>
		/// Gets or sets the flag identifying if the pack is a generic pack.
		/// </summary>
		public bool GenericInd 
		{
			get { return _genericInd ; }
			set { _genericInd = value; }
		}
		/// <summary>
		/// Gets or sets the record ID of the header to which the associated pack belongs.
		/// </summary>
		public int AssociatedPackRID 
		{
			get { return _associatedPackRID ; }
			set { _associatedPackRID = value; }
		}
        // Begin TT#1966-MD - JSmith - DC Fulfillment
        /// <summary>
        /// Gets or sets the record ID of the header pack to which the associated pack belongs.
        /// </summary>
        public List<int> AssociatedPackRIDs
        {
            get 
            {
                if (_associatedPackRIDs == null)
                {
                    _associatedPackRIDs = new List<int>();
                }
                return _associatedPackRIDs; 
            }
        }

        // Begin TT#2119-MD - JSmith - Cancelling allocation on subordinate headers on master runs forever if subordinates have same pack names with different size runs
		/// <summary>
        /// Gets or sets the ID (name) of the associated pack.
        /// </summary>
        public string AssociatedPackName
        {
            get { return _associatedPackName; }
            set { _associatedPackName = value; }
        }
		// End TT#2119-MD - JSmith - Cancelling allocation on subordinate headers on master runs forever if subordinates have same pack names with different size runs
        // End TT#1966-MD - JSmith - DC Fulfillment
		/// <summary>
		/// Gets or sets the colors in the pack.
		/// </summary>
		public Hashtable Colors 
		{
			get { return _colors ; }
			set { _colors = value; }
		}
        /// <summary>
        /// Gets or sets the pack sequence.
        /// </summary>
        public int Sequence
        {
            get { return _sequence; }
            set { _sequence = value; }
        }
	}

	/// <summary>
	/// Contains the information about the colors of a pack
	/// </summary>
	[Serializable()]
	public class HeaderPackColorProfile:Profile
	{
		// Fields
		private int							_colorCodeRID;
		private int							_units;
		private int							_sequence;
		private Hashtable					_sizes;
        private int                         _hdrPCRID;      // Assortment
        private string                      _name;          // Assortment
        private string                      _description;   // Assortment
        private int                         _last_PCSZ_Key_Used; // Assortment

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HeaderPackColorProfile(int aKey)
			:base(aKey)
		{
			_sizes = new Hashtable();
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.HeaderPackColor;
			}
		}
		/// <summary>
		/// Gets or sets if the record ID of the color.
		/// </summary>
		public int ColorCodeRID 
		{
			get { return _colorCodeRID ; }
			set { _colorCodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the number of units for the color.
		/// </summary>
		public int Units 
		{
			get { return _units ; }
			set { _units = value; }
		}
		/// <summary>
		/// Gets or sets the sequence number of the color in the pack.
		/// </summary>
		public int Sequence 
		{
			get { return _sequence ; }
			set { _sequence = value; }
		}
		/// <summary>
		/// Gets or sets the sizes in the color.
		/// </summary>
		public Hashtable Sizes 
		{
			get { return _sizes ; }
			set { _sizes = value; }
		}
        /// <summary>
        /// Gets or sets the HDR_PC_RID of the pack color.
        /// </summary>
        public int HdrPCRID
        {
            get { return _hdrPCRID; }
            set { _hdrPCRID = value; }
        }
        /// <summary>
        /// Gets or sets the color name of the pack color.
        /// </summary>
        public string ColorName
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// Gets or sets the color name of the pack color.
        /// </summary>
        public string ColorDescription
        {
            get { return _description; }
            set { _description = value; }
        }
        // begin Assortment: Color/Size Change
        /// <summary>
        /// Gets or sets Last_PCSZ_Key_Used
        /// </summary>
        public int Last_PCSZ_Key_Used
        {
            get { return _last_PCSZ_Key_Used; }
            set { _last_PCSZ_Key_Used = value; }
        }
        // end Assortment: Color/Size Change
	}

	/// <summary>
	/// Contains the information about the sizes of a color in a pack
	/// </summary>
	[Serializable()]
	public class HeaderPackColorSizeProfile:Profile
	{
		// Fields
        private int                         _hdr_PCSZ_Key; // Assortment: Color/Size Change
		private int							_units;
		private int							_sequence;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HeaderPackColorSizeProfile(int aKey)
			:base(aKey)
		{
		}
		// Properties

        // begin Assortment: Color/Size Change
        /// <summary>
        /// Gets or sets HDR_PCSZ_Key
        /// </summary>
        public int HDR_PCSZ_Key
        {
            get { return _hdr_PCSZ_Key; }
            set { _hdr_PCSZ_Key = value; }
        }
        // end Assortment: Color/Size Change

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.HeaderPackColorSize;
			}
		}
		/// <summary>
		/// Gets or sets the number of units for the size.
		/// </summary>
		public int Units 
		{
			get { return _units ; }
			set { _units = value; }
		}
		/// <summary>
		/// Gets or sets the sequence number of the size of the color in the pack.
		/// </summary>
		public int Sequence 
		{
			get { return _sequence ; }
			set { _sequence = value; }
		}
	}

	/// <summary>
	/// Contains the information about the bulk colors of a header
	/// </summary>
	[Serializable()]
	public class HeaderBulkColorProfile:Profile
	{
		// Fields
		private int							_units;
		private int							_multiple;
		private int							_minimum;
		private int							_maximum;
		private int							_reserveUnits;
		private int							_sequence;
        private int                         _hdrBCRID;
        private string                      _name;
        private string                      _description;
        private int                         _asrtBCRID;
		private Hashtable					_sizes = null;
        private int                         _last_BCSZ_Key_Used;
        private ColorStatusFlags            _colorStatusFlags;  // TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HeaderBulkColorProfile(int aKey)
			:base(aKey)
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
				return eProfileType.HeaderBulkColor;
			}
		}
		/// <summary>
		/// Gets or sets the number of units for the color.
		/// </summary>
		public int Units 
		{
			get { return _units ; }
			set { _units = value; }
		}
		/// <summary>
		/// Gets or sets the bulk multiple of the color.
		/// </summary>
		public int Multiple 
		{
			get { return _multiple ; }
			set { _multiple = value; }
		}
		/// <summary>
		/// Gets or sets the minimum value for the color.
		/// </summary>
		public int Minimum 
		{
			get { return _minimum ; }
			set { _minimum = value; }
		}
		/// <summary>
		/// Gets or sets the maximum value of the color.
		/// </summary>
		public int Maximum 
		{
			get { return _maximum ; }
			set { _maximum = value; }
		}
		/// <summary>
		/// Gets or sets the reserve units for the color.
		/// </summary>
		public int ReserveUnits 
		{
			get { return _reserveUnits ; }
			set { _reserveUnits = value; }
		}
		/// <summary>
		/// Gets or sets the sequence number of the color in the pack.
		/// </summary>
		public int Sequence 
		{
			get { return _sequence ; }
			set { _sequence = value; }
		}
        /// <summary>
        /// Gets or sets the databse RID of the corresponding the color.
        /// </summary>
        public int HdrBCRID
        {
            get { return _hdrBCRID; }
            set { _hdrBCRID = value; }
        }
        /// <summary>
        /// Gets or sets the name of the placeholder color.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// Gets or sets the description of the placeholder color.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        /// <summary>
        /// Gets or sets the RID of the corresponding placeholder color.
        /// </summary>
        public int AsrtBCRID
        {
            get { return _asrtBCRID; }
            set { _asrtBCRID = value; }
        }
		/// <summary>
		/// Gets or sets the sizes in the color.
		/// </summary>
		public Hashtable Sizes 
		{
			get 
			{
				if (_sizes == null)
				{
					_sizes = new Hashtable();
				}
				return _sizes ; 
			}
			set { _sizes = value; }
		}
        // begin Assortment: Color/Size Change
        /// <summary>
        /// Gets or sets the Last_BCSZ_Key_Used
        /// </summary>
        public int Last_BCSZ_Key_Used
        {
            get { return _last_BCSZ_Key_Used; }
            set { _last_BCSZ_Key_Used = value; }
        }
        // end Assortment: color/size change
        // begin TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5
        public ColorStatusFlags ColorStatusFlags
        {
            get { return _colorStatusFlags; }
            set { _colorStatusFlags = value; }
        }
        // end TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5
	}

	/// <summary>
	/// Contains the information about the bulk color sizes of a header
	/// </summary>
	[Serializable()]
	public class HeaderBulkColorSizeProfile:Profile
	{
		// Fields
        private int                         _hdr_BCSZ_Key; // Assortment: Color/Size Change
		private int							_units;
		private int							_multiple;
		private int							_minimum;
		private int							_maximum;
		private int							_reserveUnits;
		private int							_sequence;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HeaderBulkColorSizeProfile(int aKey)
			:base(aKey)
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
				return eProfileType.HeaderBulkColorSize;
			}
		}
        // begin Assortment: Color/Size Change
        /// <summary>
        /// Gets or sets HDR_BCSZ_Key
        /// </summary>
        public int HDR_BCSZ_Key
        {
            get { return _hdr_BCSZ_Key; }
            set { _hdr_BCSZ_Key = value; }
        }
        // end Assortment: Color/Size Change
		/// <summary>
		/// Gets or sets the number of units for the color.
		/// </summary>
		public int Units 
		{
			get { return _units ; }
			set { _units = value; }
		}
		/// <summary>
		/// Gets or sets the bulk multiple of the color.
		/// </summary>
		public int Multiple 
		{
			get { return _multiple ; }
			set { _multiple = value; }
		}
		/// <summary>
		/// Gets or sets the minimum value for the color.
		/// </summary>
		public int Minimum 
		{
			get { return _minimum ; }
			set { _minimum = value; }
		}
		/// <summary>
		/// Gets or sets the maximum value of the color.
		/// </summary>
		public int Maximum 
		{
			get { return _maximum ; }
			set { _maximum = value; }
		}
		/// <summary>
		/// Gets or sets the reserve units for the color.
		/// </summary>
		public int ReserveUnits 
		{
			get { return _reserveUnits ; }
			set { _reserveUnits = value; }
		}
		/// <summary>
		/// Gets or sets the sequence number of the color in the pack.
		/// </summary>
		public int Sequence 
		{
			get { return _sequence ; }
			set { _sequence = value; }
		}
	}

	// TT#616 - stodd - pack rounding
	#region HeaderPackRoundingOverride
	public class HeaderPackRoundingOverride
	{
		private int _headerRid;
		private int _packMultipleRid;
		private double _packRounding1stPack;
		private double _packRoundingNthPack;

		public int HeaderRid
		{
			get { return _headerRid; }
			set { _headerRid = value; }
		}
		public int PackMultipleRid
		{
			get { return _packMultipleRid; }
			set { _packMultipleRid = value; }
		}
		public double PackRounding1stPack
		{
			get { return _packRounding1stPack; }
			set { _packRounding1stPack = value; }
		}
		public double PackRoundingNthPack
		{
			get { return _packRoundingNthPack; }
			set { _packRoundingNthPack = value; }
		}

		public HeaderPackRoundingOverride()
		{
			_headerRid = Include.NoRID;
			_packMultipleRid = Include.NoRID;
			_packRounding1stPack = Include.NoRID;
			_packRoundingNthPack = Include.NoRID;
		}

		public HeaderPackRoundingOverride(int headerRid, int packMultipleRid, double packRounding1stPack, double packRoundingNthPack)
		{
			_headerRid = headerRid;
			_packMultipleRid = packMultipleRid;
			_packRounding1stPack = packRounding1stPack;
			_packRoundingNthPack = packRoundingNthPack;
		}
	}
	#endregion HeaderPackRoundingOverride
	// TT#616 - stodd - pack rounding
}
