using MIDRetail.DataCommon;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Logility.ROWebSharedTypes
{

    [DataContract(Name = "ROAllocationHeaderSummary", Namespace = "http://Logility.ROWeb/")]
    public class ROAllocationHeaderSummary
    {
        #region "DataMember"
        //=======
        // FIELDS
        //=======

        [DataMember(IsRequired = true)]
        private int _updatedStyleNodeKey;

        [DataMember(IsRequired = true)]
        private string _updatedStyleNodeDescription;

        [DataMember(IsRequired = true)]
        private int _key;

        [DataMember(IsRequired = true)]
        private string _headerID;

        [DataMember(IsRequired = true)]
        private string _headerDescription;

        [DataMember(IsRequired = true)]
        private DateTime _headerReceiptDay;

        [DataMember(IsRequired = true)]
        private DateTime _originalReceiptDay;

        [DataMember(IsRequired = true)]
        private double _unitRetail;

        [DataMember(IsRequired = true)]
        private double _unitCost;

        [DataMember(IsRequired = true)]
        private int _styleHnRID;

        [DataMember(IsRequired = true)]
        private int _planHnRID;

        [DataMember(IsRequired = true)]
        private string _planHnText;

        [DataMember(IsRequired = true)]
        private int _onHandHnRID;

        [DataMember(IsRequired = true)]
        private string _onHandHnText;

        [DataMember(IsRequired = true)]
        private string _vendor;

        [DataMember(IsRequired = true)]
        private string _purchaseOrder;

        [DataMember(IsRequired = true)]
        private DateTime _beginDay;

        [DataMember(IsRequired = true)]
        private DateTime _shipDay;

        [DataMember(IsRequired = true)]
        private DateTime _lastNeedDay;

        [DataMember(IsRequired = true)]
        private DateTime _releaseApprovedDate;

        [DataMember(IsRequired = true)]
        private DateTime _releaseDate;

        [DataMember(IsRequired = true)]
        private DateTime _earliestShipDay;

        [DataMember(IsRequired = true)]
        private int _headerGroupRID;

        [DataMember(IsRequired = true)]
        private int _sizeGroupRID;

        [DataMember(IsRequired = true)]
        private int _workflowRID;

        [DataMember(IsRequired = true)]
        private bool _workflowTrigger;

        [DataMember(IsRequired = true)]
        private int _apiWorkflowRID;

        [DataMember(IsRequired = true)]
        private bool _apiWorkflowTrigger;

        [DataMember(IsRequired = true)]
        private int _methodRID;

        [DataMember(IsRequired = true)]
        private bool _allocationStarted;

        [DataMember(IsRequired = true)]
        private double _percentNeedLimit;

        [DataMember(IsRequired = true)]
        private double _planPercentFactor;

        [DataMember(IsRequired = true)]
        private int _reserveUnits;

        [DataMember(IsRequired = true)]
        private int _allocatedUnits;

        [DataMember(IsRequired = true)]
        private int _origAllocatedUnits;

        [DataMember(IsRequired = true)]
        private int _rsvAllocatedUnits;

        [DataMember(IsRequired = true)]
        private int _releaseCount;

        [DataMember(IsRequired = true)]
        private int _gradeWeekCount;

        [DataMember(IsRequired = true)]
        private int _primarySecondaryRID;

        [DataMember(IsRequired = true)]
        private string _distributionCenter;

        [DataMember(IsRequired = true)]
        private string _allocationNotes;

        [DataMember(IsRequired = true)]
        private int _headerTotalQtyToAllocate;

        [DataMember(IsRequired = true)]
        private int _headerTotalQtyAllocated;

        [DataMember(IsRequired = true)]
        private int _headerTotalUnitMultiple;

        [DataMember(IsRequired = true)]
        private int _headerTotalUnitMultipleDefault;

        [DataMember(IsRequired = true)]
        private int _unitsShipped;

        [DataMember(IsRequired = true)]
        private int _genericTotalQtyToAllocate;

        [DataMember(IsRequired = true)]
        private int _genericTotalQtyAllocated;

        [DataMember(IsRequired = true)]
        private int _genericTotalUnitMultiple;

        [DataMember(IsRequired = true)]
        private int _detailTotalQtyToAllocate;

        [DataMember(IsRequired = true)]
        private int _detailTotalQtyAllocated;

        [DataMember(IsRequired = true)]
        private int _detailTotalUnitMultiple;

        [DataMember(IsRequired = true)]
        private int _bulkTotalQtyToAllocate;

        [DataMember(IsRequired = true)]
        private int _bulkTotalQtyAllocated;

        [DataMember(IsRequired = true)]
        private int _bulkTotalUnitMultiple;

        [DataMember(IsRequired = true)]
        private int _masterRID;

        [DataMember(IsRequired = true)]
        private string _masterID;

        [DataMember(IsRequired = true)]
        private int _subordinateRID;

        [DataMember(IsRequired = true)]
        private List<int> _subordinateRIDs;

        [DataMember(IsRequired = true)]
        private string _subordinateID;

        [DataMember(IsRequired = true)]
        private int _storeStyleAllocationManuallyChgdCnt;

        [DataMember(IsRequired = true)]
        private int _storeSizeAllocationManuallyChgdCnt;

        [DataMember(IsRequired = true)]
        private int _storeStyleManualAllocationTotal;

        [DataMember(IsRequired = true)]
        private int _storeSizeManualAllocationTotal;

        [DataMember(IsRequired = true)]
        private int _storesWithAllocationCnt;

        [DataMember(IsRequired = true)]
        private bool _horizonOverride;

        [DataMember(IsRequired = true)]
        private int _asrtRID;

        [DataMember(IsRequired = true)]
        private int _placeHolderRID;

        [DataMember(IsRequired = true)]
        private int _asrtType;

        [DataMember(IsRequired = true)]
        private string _nodeDisplayForOtsForecast;

        [DataMember(IsRequired = true)]
        private string _nodeDisplayForOnHand;

        [DataMember(IsRequired = true)]
        private string _nodeDisplayForGradeInvBasis;

        [DataMember(IsRequired = true)]
        private string _workflowName;

        [DataMember(IsRequired = true)]
        private string _headerMethodName;

        [DataMember(IsRequired = true)]
        private string _apiWorkflowName;

        [DataMember(IsRequired = true)]
        private int _gradeSG_RID;

        [DataMember(IsRequired = true)]
        private bool _gradeInventoryMinMax;

        [DataMember(IsRequired = true)]
        private int _gradeInventoryHnRID;

        [DataMember(IsRequired = true)]
        private string _gradeInventoryHnText;

        [DataMember(IsRequired = true)]
        private string _imoID;

        [DataMember(IsRequired = true)]
        private int _itemUnitsAllocated;

        [DataMember(IsRequired = true)]
        private int _itemOrigUnitsAllocated;

        [DataMember(IsRequired = true)]
        private int _asrtPlaceholderSeq;

        [DataMember(IsRequired = true)]
        private int _asrtHeaderSeq;

        [DataMember(IsRequired = true)]
        private int _asrtUserRid;

        [DataMember(IsRequired = true)]
        private string _asrtID;

        [DataMember(IsRequired = true)]
        private int _asrtTypeForParentAsrt;

        [DataMember(IsRequired = true)]
        private int _unitsPerCarton;

        [DataMember(IsRequired = true)]
        private bool _DCFulfillmentProcessed;

        [DataMember(IsRequired = true)]
        private eHeaderType _headerType;

        [DataMember(IsRequired = true)]
        private string _headerTypeText;

        [DataMember(IsRequired = true)]
        private bool _canView;

        [DataMember(IsRequired = true)]
        private bool _canUpdate;

        [DataMember(IsRequired = true)]
        private eHeaderAllocationStatus _headerStatus;

        [DataMember(IsRequired = true)]
        private string _headerStatusText;

        [DataMember(IsRequired = true)]
        private eHeaderIntransitStatus _intransitStatus;

        [DataMember(IsRequired = true)]
        private string _intransitStatusText;

        [DataMember(IsRequired = true)]
        private eHeaderShipStatus _shipStatus;

        [DataMember(IsRequired = true)]
        private string _shipStatusText;

        [DataMember(IsRequired = true)]
        private HierarchyNodeProfile _hnp_style;

        [DataMember(IsRequired = true)]
        private string _parentID;

        [DataMember(IsRequired = true)]
        private string _styleID;

        [DataMember(IsRequired = true)]
        private int _numberOfStores;

        [DataMember(IsRequired = true)]
        private int _packCount;

        [DataMember(IsRequired = true)]
        private int _bulkColorCount;

        [DataMember(IsRequired = true)]
        private int _bulkColorSizeCount;

        [DataMember(IsRequired = true)]
        private string _sizeGroupName;

        [DataMember(IsRequired = true)]
        private string _subordID;

        [DataMember(IsRequired = true)]
        private string _masterSubord;

        [DataMember(IsRequired = true)]
        private int _adjustVSW;

        [DataMember(IsRequired = true)]
        private int _groupAllocRid;

        [DataMember(IsRequired = true)]
        private int _asrtSortSeq;

        [DataMember(IsRequired = true)]
        private int _storetot;

        [DataMember(IsRequired = true)]
        private int _vSWtot;

        [DataMember(IsRequired = true)]
        private int _balance;

        [DataMember(IsRequired = true)]
        private List<ROAllocationHeaderCharacteristic> _headerCharacteristics;

        [DataMember(IsRequired = true)]
        private List<ROCharacteristic> _productCharacteristics = null;

        [DataMember(IsRequired = true)]
        private int _anchorHnRID;

        [DataMember(IsRequired = true)]
        private string _anchorNode;

        [DataMember(IsRequired = true)]
        private int _cdrRID;

        [DataMember(IsRequired = true)]
        private string _dateRange;

        [DataMember(IsRequired = true)]
        public List<ROHeaderBulkColorDetails> _bulkColorDetails;

        [DataMember(IsRequired = true)]
        public List<ROHeaderPackProfile> _packDetails;

        [DataMember(IsRequired = true)]
        private int _digitalAssetKey;

        [DataMember(IsRequired = true)]
        private List<ROUpdateContent> _contentUpdateRequests = null;

        #endregion Fields

        #region "Properties"

        [ObsoleteAttribute("This property is no longer used. Use StyleHnRID instead.")]
        public int UpdatedStyleNodeKey
        {
            get
            {
                return _updatedStyleNodeKey;
            }
            set
            {
                _updatedStyleNodeKey = value;
            }
        }

        [ObsoleteAttribute("This property is no longer used. Use HeaderDescription instead.")]
        public string UpdatedStyleNodeDescription
        {
            get
            {
                return _updatedStyleNodeDescription;
            }
            set
            {
                _updatedStyleNodeDescription = value;
            }
        }

        public int DigitalAssetKey
        {
            get
            {
                return _digitalAssetKey;
            }
            set
            {
                _digitalAssetKey = value;
            }
        }

        public int Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
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
        /// Gets or sets PlanHnText
        /// </summary>
        /// <remarks>
        /// Identifies the merchandise plan source.
        /// </remarks>
        public string PlanHnText
        {
            get
            {
                return _planHnText;
            }
            set
            {
                _planHnText = value;
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
        /// Gets or sets OnHandHnText
        /// </summary>
        /// <remarks>
        /// Identifies the merchandise OnHand source. 
        /// </remarks>
        public string OnHandHnText
        {
            get
            {
                return _onHandHnText;
            }
            set
            {
                _onHandHnText = value;
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
            set { value = _subordinateRIDs; }
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



        /// <summary>
        /// Gets or sets the Grade Store Group RID
        /// </summary>
        public int GradeSG_RID
        {
            get { return _gradeSG_RID; }
            set { _gradeSG_RID = value; }
        }


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

        /// <summary>
        /// Gets or sets the Inventory Basis HnRID Text used to calculate an Inventory Minimum or Maximum
        /// </summary>
        public string GradeInventoryBasisHnText
        {
            get
            {
                return _gradeInventoryHnText;
            }
            set
            {
                _gradeInventoryHnText = value;
            }
        }

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


        public int AsrtUserRid
        {
            get { return _asrtUserRid; }
            set { _asrtUserRid = value; }
        }


        /// <summary>
        /// Gets or sets the Assortment ID
        /// </summary>
        public string AssortmentID
        {
            get { return _asrtID; }
            set { _asrtID = value; }
        }

        public int AsrtTypeForParentAsrt
        {
            get { return _asrtTypeForParentAsrt; }
            set { _asrtTypeForParentAsrt = value; }
        }

        public int UnitsPerCarton
        {
            get { return _unitsPerCarton; }
            set { _unitsPerCarton = value; }
        }

        public eHeaderType HeaderType { get { return _headerType; } set { _headerType = value; } }
        public string HeaderTypeText { get { return _headerTypeText; } set { _headerTypeText = value; } }
        public bool CanView { get { return _canView; } set { _canView = value; } }
        public bool CanUpdate { get { return _canUpdate; } set { _canUpdate = value; } }
        public eHeaderAllocationStatus HeaderStatus { get { return _headerStatus; } set { _headerStatus = value; } }
        public string HeaderStatusText { get { return _headerStatusText; } set { _headerStatusText = value; } }
        public eHeaderIntransitStatus IntransitStatus { get { return _intransitStatus; } set { _intransitStatus = value; } }

        public string IntransitStatusText { get { return _intransitStatusText; } set { _intransitStatusText = value; } }

        public eHeaderShipStatus ShipStatus { get { return _shipStatus; } set { _shipStatus = value; } }

        public string ShipStatusText { get { return _shipStatusText; } set { _shipStatusText = value; } }


        public HierarchyNodeProfile Hnp_Style { get { return _hnp_style; } set { _hnp_style = value; } }

        public string ParentID { get { return _parentID; } set { _parentID = value; } }

        public string StyleID { get { return _styleID; } set { _styleID = value; } }

        public int NumberOfStores { get { return _numberOfStores; } set { _numberOfStores = value; } }


        public int PackCount { get { return _packCount; } set { _packCount = value; } }

        public int BulkColorCount { get { return _bulkColorCount; } set { _bulkColorCount = value; } }


        public int BulkColorSizeCount { get { return _bulkColorSizeCount; } set { _bulkColorSizeCount = value; } }

        public string SizeGroupName { get { return _sizeGroupName; } set { _sizeGroupName = value; } }

        public string SubordID { get { return _subordID; } set { _subordID = value; } }


        public string MasterSubord { get { return _masterSubord; } set { _masterSubord = value; } }


        public int AdjustVSW { get { return _adjustVSW; } set { _adjustVSW = value; } }


        public int GroupAllocRid { get { return _groupAllocRid; } set { _groupAllocRid = value; } }


        public int AsrtSortSeq { get { return _asrtSortSeq; } set { _asrtSortSeq = value; } }


        public int StoreTot { get { return _storetot; } set { _storetot = value; } }

        public int VSWtot { get { return _vSWtot; } set { _vSWtot = value; } }


        public int Balance { get { return _balance; } set { _balance = value; } }


        public List<ROAllocationHeaderCharacteristic> HeaderCharacteristics
        {
            get { return _headerCharacteristics; }
            set { _headerCharacteristics = value; }
        }

        public List<ROCharacteristic> ProductCharacteristics
        {
            get { return _productCharacteristics; }
            set { _productCharacteristics = value; }
        }

        public int AnchorHnRID { get { return _anchorHnRID; } set { _anchorHnRID = value; } }

        public string AnchorNode { get { return _anchorNode; } set { _anchorNode = value; } }

        public int CdrRID { get { return _cdrRID; } set { _cdrRID = value; } }

        public string DateRange { get { return _dateRange; } set { _dateRange = value; } }


        public List<ROHeaderBulkColorDetails> BulkColorDetails
        {
            get { return _bulkColorDetails; }
            set { _bulkColorDetails = value; }
        }

        public List<ROHeaderPackProfile> PackDetails
        {
            get { return _packDetails; }
            set { _packDetails = value; }
        }

        /// <summary>
        /// Contains ROUpdateContent objects for the header
        /// </summary>
        public List<ROUpdateContent> ContentUpdateRequests
        {
            get
            {
                if (_contentUpdateRequests == null)
                {
                    _contentUpdateRequests = new List<ROUpdateContent>();
                }
                return _contentUpdateRequests;
            }
        }

        #endregion Properties

        #region "Constructor"
        public ROAllocationHeaderSummary(int aKey, string headerID, string headerDescription, DateTime headerReceiptDay,
                                        DateTime originalReceiptDay, double unitRetail, double unitCost, int styleHnRID, int planHnRID, int onHandHnRID,
                                        string vendor, string purchaseOrder, DateTime beginDay, DateTime shipDay, DateTime lastNeedDay,
                                         DateTime releaseApprovedDate, DateTime releaseDate, DateTime earliestShipDay,
                                         int headerGroupRID, int sizeGroupRID, int workflowRID, bool workflowTrigger, int apiWorkflowRID,
                                         bool apiWorkflowTrigger, int methodRID, bool allocationStarted, double percentNeedLimit, double planPercentFactor,
                                         int reserveUnits, int allocatedUnits, int origAllocatedUnits, int rsvAllocatedUnits, int releaseCount,
                                         int gradeWeekCount, int primarySecondaryRID, string distributionCenter, string allocationNotes,
                                         int headerTotalQtyToAllocate, int headerTotalQtyAllocated, int headerTotalUnitMultiple,
                                         int headerTotalUnitMultipleDefault, int unitsShipped, int genericTotalQtyToAllocate, int genericTotalQtyAllocated,
                                         int genericTotalUnitMultiple, int detailTotalQtyToAllocate, int detailTotalQtyAllocated, int detailTotalUnitMultiple,
                                         int bulkTotalQtyToAllocate, int bulkTotalQtyAllocated, int bulkTotalUnitMultiple, int masterRID, string masterID,
                                         int subordinateRID, List<int> subordinateRIDs, string subordinateID, int storeStyleAllocationManuallyChgdCnt,
                                         int storeSizeAllocationManuallyChgdCnt, int storeStyleManualAllocationTotal, int storeSizeManualAllocationTotal,
                                         int storesWithAllocationCnt, bool horizonOverride, int asrtRID, int placeHolderRID, int asrtType,
                                         string nodeDisplayForOtsForecast, string nodeDisplayForOnHand, string nodeDisplayForGradeInvBasis, string workflowName,
                                         string headerMethodName, string apiWorkflowName, int gradeSG_RID, bool gradeInventoryMinMax, int gradeInventoryHnRID,
                                         string imoID, int itemUnitsAllocated, int itemOrigUnitsAllocated, int asrtPlaceholderSeq, int asrtHeaderSeq, int asrtUserRid,
                                         string asrtID, int asrtTypeForParentAsrt, int unitsPerCarton, bool DCFulfillmentProcessed, eHeaderType headerType,
                                         string headerTypeText, bool canView, bool canUpdate, eHeaderAllocationStatus headerStatus,
                                         string headerStatusText,
                                         eHeaderIntransitStatus intransitStatus, string intransitStatusText, eHeaderShipStatus shipStatus,
                                         string shipStatusText,
                                         HierarchyNodeProfile hnp_Style, string parentID, string styleID, int numberOfStores,
                                         int packCount, int bulkColorCount,
                                         int bulkColorSizeCount, string sizeGroupName, string subordID, string masterSubord, int digitalAssetKey,
                                         int adjustVSW, int groupAllocRid, int asrtSortSeq, int storeTot, int vSWtot, int balance,
                                         string planHnText, string onHandHnText,
                                         string gradeInvBasis, List<ROAllocationHeaderCharacteristic> headerCharacteristics,
                                         int anchorHnRID = Include.NoRID,
                                         string anchorNode = "", int cdrRID = Include.NoRID, string dateRange = "",
                                         List<ROHeaderBulkColorDetails> bulkColorDetails = null,
                                         List<ROHeaderPackProfile> packDetails = null)
        {
            Key = aKey;
            _headerID = headerID;
            _headerDescription = headerDescription;
            _headerReceiptDay = headerReceiptDay;
            _originalReceiptDay = originalReceiptDay;
            _unitRetail = unitRetail;
            _unitCost = unitCost;
            _styleHnRID = styleHnRID;
            _updatedStyleNodeKey = Include.Undefined;
            _planHnRID = planHnRID;
            _onHandHnRID = onHandHnRID;
            _vendor = vendor;
            _purchaseOrder = purchaseOrder;
            _beginDay = beginDay;
            _shipDay = shipDay;
            _lastNeedDay = lastNeedDay;
            _releaseApprovedDate = releaseApprovedDate;
            _releaseDate = releaseDate;
            _earliestShipDay = earliestShipDay;
            _headerGroupRID = headerGroupRID;
            _sizeGroupRID = sizeGroupRID;
            _workflowRID = workflowRID;
            _workflowTrigger = workflowTrigger;
            _apiWorkflowRID = apiWorkflowRID;
            _apiWorkflowTrigger = apiWorkflowTrigger;
            _methodRID = methodRID;
            _allocationStarted = allocationStarted;
            _percentNeedLimit = percentNeedLimit;
            _planPercentFactor = planPercentFactor;
            _reserveUnits = reserveUnits;
            _allocatedUnits = allocatedUnits;
            _origAllocatedUnits = origAllocatedUnits;
            _rsvAllocatedUnits = rsvAllocatedUnits;
            _releaseCount = releaseCount;
            _gradeWeekCount = gradeWeekCount;
            _primarySecondaryRID = primarySecondaryRID;
            _distributionCenter = distributionCenter;
            _allocationNotes = allocationNotes;
            _headerTotalQtyToAllocate = headerTotalQtyToAllocate;
            _headerTotalQtyAllocated = headerTotalQtyAllocated;
            _headerTotalUnitMultiple = headerTotalUnitMultiple;
            _headerTotalUnitMultipleDefault = headerTotalUnitMultipleDefault;
            _unitsShipped = unitsShipped;
            _genericTotalQtyToAllocate = genericTotalQtyToAllocate;
            _genericTotalQtyAllocated = genericTotalQtyAllocated;
            _genericTotalUnitMultiple = genericTotalUnitMultiple;
            _detailTotalQtyToAllocate = detailTotalQtyToAllocate;
            _detailTotalQtyAllocated = detailTotalQtyAllocated;
            _detailTotalUnitMultiple = detailTotalUnitMultiple;
            _bulkTotalQtyToAllocate = bulkTotalQtyToAllocate;
            _bulkTotalQtyAllocated = bulkTotalQtyAllocated;
            _bulkTotalUnitMultiple = bulkTotalUnitMultiple;
            _masterRID = masterRID;
            _masterID = masterID;
            _subordinateRID = subordinateRID;
            _subordinateRIDs = subordinateRIDs;
            _subordinateID = subordinateID;
            _storeStyleAllocationManuallyChgdCnt = storeStyleAllocationManuallyChgdCnt;
            _storeSizeAllocationManuallyChgdCnt = storeSizeAllocationManuallyChgdCnt;
            _storeStyleManualAllocationTotal = storeStyleManualAllocationTotal;
            _storeSizeManualAllocationTotal = storeSizeManualAllocationTotal;
            _storesWithAllocationCnt = storesWithAllocationCnt;
            _horizonOverride = horizonOverride;
            _asrtRID = asrtRID;
            _placeHolderRID = placeHolderRID;
            _asrtType = asrtType;
            _nodeDisplayForOtsForecast = nodeDisplayForOtsForecast;
            _nodeDisplayForOnHand = nodeDisplayForOnHand;
            _nodeDisplayForGradeInvBasis = nodeDisplayForGradeInvBasis;
            _workflowName = workflowName;
            _headerMethodName = headerMethodName;
            _apiWorkflowName = apiWorkflowName;
            _gradeInventoryMinMax = gradeInventoryMinMax;
            _gradeInventoryHnRID = gradeInventoryHnRID;
            _imoID = imoID;
            _itemUnitsAllocated = itemUnitsAllocated;
            _itemOrigUnitsAllocated = itemOrigUnitsAllocated;
            _asrtPlaceholderSeq = asrtPlaceholderSeq;
            _asrtHeaderSeq = asrtHeaderSeq;
            _asrtUserRid = asrtUserRid;
            _asrtID = asrtID;
            _asrtTypeForParentAsrt = asrtTypeForParentAsrt;
            _unitsPerCarton = unitsPerCarton;
            _DCFulfillmentProcessed = DCFulfillmentProcessed;
            _headerType = headerType;
            _headerTypeText = headerTypeText;
            _canView = canView;
            _canUpdate = canUpdate;
            _headerStatus = headerStatus;
            _headerStatusText = headerStatusText;
            _intransitStatus = intransitStatus;
            _shipStatus = shipStatus;
            _shipStatusText = shipStatusText;
            _hnp_style = hnp_Style;
            _parentID = parentID;
            _styleID = styleID;
            _numberOfStores = numberOfStores;
            _packCount = packCount;
            _bulkColorCount = bulkColorCount;
            _bulkColorSizeCount = bulkColorSizeCount;
            _sizeGroupName = sizeGroupName;
            _subordID = subordID;
            _masterSubord = masterSubord;
            _adjustVSW = adjustVSW;
            _groupAllocRid = groupAllocRid;
            _asrtSortSeq = asrtSortSeq;
            _storetot = storeTot;
            _vSWtot = vSWtot;
            _balance = balance;
            _planHnText = planHnText;
            _onHandHnText = onHandHnText;
            _gradeInventoryHnText = gradeInvBasis;
            _headerCharacteristics = headerCharacteristics;

            _anchorHnRID = anchorHnRID;
            _anchorNode = anchorNode;
            _cdrRID = cdrRID;
            _dateRange = dateRange;

            _bulkColorDetails = bulkColorDetails;
            _packDetails = packDetails;
            _digitalAssetKey = digitalAssetKey;
        }


        #endregion
    }

    public class ROHeaderPackProfile
    {
        //public string AssociatedPackName { get; set; }
        public KeyValuePair<int, string> AssociatedPack { get; set; }
        public List<int> AssociatedPackRIDs { get; set; }
        public bool GenericInd { get; set; }

        //public string HeaderPackName { get; set; }
        public int? HeaderRID { get; set; }
        public long? InstanceId { get; set; }
        public KeyValuePair<int, string> Pack { get; set; }
        public int? Multiple { get; set; }
        public int? Packs { get; set; }
        public eProfileType ProfileType { get; set; }
        public int? ReservePacks { get; set; }
        public int? Sequence { get; set; }
        public int? Balance { get; set; }

        public eChangeType ChangeType { get; set; }
        public List<HeaderPackColor> ColorsInfo { get; set; }

        private List<ROUpdateContent> _contentUpdateRequests = null;

        /// <summary>
        /// Contains ROUpdateContent objects for the pack
        /// </summary>
        public List<ROUpdateContent> ContentUpdateRequests
        {
            get
            {
                if (_contentUpdateRequests == null)
                {
                    _contentUpdateRequests = new List<ROUpdateContent>();
                }
                return _contentUpdateRequests;
            }
        }

    }

    public class HeaderPackColor
    {
        public int? ColorCodeRID { get; set; }
        public string ColorDescription { get; set; }
        public string ColorName { get; set; }
        public int? HdrPCRID { get; set; }
        public long? InstanceID { get; set; }
        public KeyValuePair<int, string> Color { get; set; }
        public int? Last_PCSZ_Key_Used { get; set; }
        public eProfileType ProfileType { get; set; }
        public int? Sequence { get; set; }
        public int? Units { get; set; }
        public int? Balance { get; set; }
        public List<HeaderPackColorSize> Sizes { get; set; }
        public List<ROCharacteristic> ProductCharacteristics { get; set; }
        public int? DigitalAssetKey { get; set; }

    }

    public class HeaderPackColorSize
    {
        public int? Hdr_PCSZ_Key { get; set; }
        public int? Units { get; set; }
        public int? Sequence { get; set; }
        public bool isFound { get; set; }
        public long? InstanceId { get; set; }
        public eProfileType ProfileType { get; set; }
        public KeyValuePair<int, string> Size { get; set; }
        public KeyValuePair<int, string> SizePrimary { get; set; }
        public KeyValuePair<int, string> SizeSecondary { get; set; }
    }

    public class ROHeaderBulkColorDetails
    {
        public int? AsrtBCRID { get; set; }
        public bool ColorStatusFlags { get; set; }
        public string Description { get; set; }
        public long? InstanceID { get; set; }
        public KeyValuePair<int, string> Color { get; set; }
        public int? Last_BCSZ_Key_Used { get; set; }
        public int? Maximum { get; set; }
        public int? Minimum { get; set; }
        public int? Multiple { get; set; }
        public string Name { get; set; }
        public eProfileType HeaderBulkColor { get; set; }
        public int? ReserveUnits { get; set; }
        public int? Sequence { get; set; }
        public int? Units { get; set; }
        public int? Balance { get; set; }

        public eChangeType ChangeType { get; set; }

        public List<BulkColorSize> BulkColorSizeProfile { get; set; }

        public List<ROCharacteristic> ProductCharacteristics { get; set; }

        public int? DigitalAssetKey { get; set; }

        private List<ROUpdateContent> _contentUpdateRequests = null;

        /// <summary>
        /// Contains ROUpdateContent objects for the entire assortment
        /// </summary>
        public List<ROUpdateContent> ContentUpdateRequests
        {
            get
            {
                if (_contentUpdateRequests == null)
                {
                    _contentUpdateRequests = new List<ROUpdateContent>();
                }
                return _contentUpdateRequests;
            }
        }
    }

    public class BulkColorSize
    {
        public int? HDR_BCSZ_Key { get; set; }
        public long? InstanceID { get; set; }
        public KeyValuePair<int, string> Size { get; set; }
        public KeyValuePair<int, string> SizePrimary { get; set; }
        public KeyValuePair<int, string> SizeSecondary { get; set; }
        public int? Maximum { get; set; }
        public int? Minimum { get; set; }
        public int? Multiple { get; set; }
        public eProfileType HeaderBulkColorSize { get; set; }
        public int? ReserveUnits { get; set; }
        public int? Sequence { get; set; }
        public int? Units { get; set; }
    }

    /// <summary>
    /// Allocation Worklist View Column Definition
    /// </summary>
    /// <remarks>Inherits ROAllocationWorklistEntry so fields can be used by other classes without changing existing methods</remarks>
    [DataContract(Name = "ROAllocationWorklistOut", Namespace = "http://Logility.ROWeb/")]
    public class ROAllocationWorklistOut : ROAllocationWorklistEntry
    {

        #region MemberVariables
        

        #endregion

        #region Constructor
        public ROAllocationWorklistOut(int viewKey, string bandKey, string columnKey, int visiblePosition, bool isHidden, bool isGroupByColumn,
               int sortDirection, int sortSequence, int width, string columnType, string headerCharacteristicGroupKey, string label, string itemField) :
            base(viewKey, bandKey, columnKey, visiblePosition, isHidden, isGroupByColumn,
               sortDirection, sortSequence, width, columnType, headerCharacteristicGroupKey, label, itemField)
        {


        }
        #endregion  

    }




    [DataContract(Name = "ROAllocationWorklistValuesOut", Namespace = "http://Logility.ROWeb/")]
    public class ROAllocationWorklistValuesOut : ROOut
    {

        #region MemberVariables
        [DataMember(IsRequired = true)]
        private int _userRID;

        [DataMember(IsRequired = true)]
        private int _viewRID;

        [DataMember(IsRequired = true)]
        private string _viewName;

        [DataMember(IsRequired = true)]
        private int _filterRID;

        [DataMember(IsRequired = true)]
        private string _filterName;

        [DataMember(IsRequired = true)]
        private int _totalNumberOfHeaders;

        [DataMember(IsRequired = true)]
        private int _totalQuantityToAllocate;

        #endregion

        #region Constructor
        public ROAllocationWorklistValuesOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, ROAllocationWorklistValues rOAllocationWorklistValues) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _userRID = rOAllocationWorklistValues.UserRID;
            _viewRID = rOAllocationWorklistValues.ViewRID;
            _viewName = rOAllocationWorklistValues.ViewName;
            _filterRID = rOAllocationWorklistValues.FilterRID;
            _filterName = rOAllocationWorklistValues.FilterName;
            _totalNumberOfHeaders = rOAllocationWorklistValues.TotalNumberOfHeaders;
            _totalQuantityToAllocate = rOAllocationWorklistValues.TotalQuantityToAllocate;

        }
        #endregion  

        #region Public Properties
        public int UserRID
        {
            get { return _userRID; }
            set { _userRID = value; }
        }
        public int ViewRID
        {
            get { return _viewRID; }
            set { _viewRID = value; }
        }

        public string ViewName
        {
            get { return _viewName; }
            set { _viewName = value; }
        }

        public int FilterRID
        {
            get { return _filterRID; }
            set { _filterRID = value; }
        }

        public string FilterName
        {
            get { return _filterName; }
            set { _filterName = value; }
        }

        public int TotalNumberOfHeaders
        {
            get { return _totalNumberOfHeaders; }
            set { _totalNumberOfHeaders = value; }
        }

        public int TotalQuantityToAllocate
        {
            get { return _totalQuantityToAllocate; }
            set { _totalQuantityToAllocate = value; }
        }

        #endregion  
    }

    [DataContract(Name = "ROAllocationReviewSelectionBasis", Namespace = "http://Logility.ROWeb/")]
    public class ROAllocationReviewSelectionBasis
    {
        [DataMember(IsRequired = true)]
        private int _basisPHRID;
        [DataMember(IsRequired = true)]
        private int _basisSequence;
        [DataMember(IsRequired = true)]
        private int _HN_RID;
        [DataMember(IsRequired = true)]
        private int _FV_RID;
        [DataMember(IsRequired = true)]
        private int _CDR_RID;
        [DataMember(IsRequired = true)]
        private double _weight;
        [DataMember(IsRequired = true)]
        private string _merchandise;
        [DataMember(IsRequired = true)]
        private string _version;
        [DataMember(IsRequired = true)]
        private string _horizonDateRange;

        public int BASIS_PHRID { get { return _basisPHRID; } }
        public int BASIS_SEQUENCE { get { return _basisSequence; } }
        public int HN_RID { get { return _HN_RID; } }
        public int FV_RID { get { return _FV_RID; } }
        public int CDR_RID { get { return _CDR_RID; } }
        public double WEIGHT { get { return _weight; } }
        public string Merchandise { get { return _merchandise; } }
        public string Version { get { return _version; } }
        public string HorizonDateRange { get { return _horizonDateRange; } }


        public ROAllocationReviewSelectionBasis(int basisPHRID, int basicSequence, int HN_RID, int FV_RID, int CDR_RID, double weight, string Merchandise, string Version, string HorizonDateRange)
        {
            _basisPHRID = basisPHRID;
            _basisSequence = basicSequence;
            _HN_RID = HN_RID;
            _FV_RID = FV_RID;
            _CDR_RID = CDR_RID;
            _weight = weight;
            _merchandise = Merchandise;
            _version = Version;
            _horizonDateRange = HorizonDateRange;

        }
    }


    [DataContract(Name = "ROAllocationReviewSelectionGridData", Namespace = "http://Logility.ROWeb/")]
    public class ROAllocationReviewSelectionGridData
    {

        #region Member Variables

        [DataMember(IsRequired = true)]
        private string _header;

        [DataMember(IsRequired = true)]
        private int _rowPosition;

        [DataMember(IsRequired = true)]
        private string _description;

        [DataMember(IsRequired = true)]
        private int? _rid;
        #endregion

        #region Constructor

        public string Header { get { return _header; } }

        public int RowPosition { get { return _rowPosition; } }

        public string Description { get { return _description; } }

        public int? RID { get { return _rid; } }

        public ROAllocationReviewSelectionGridData(int iRowPosition, string iHeader, string iDescription, int? iMethodRID)
        {
            _rowPosition = iRowPosition;
            _header = iHeader;
            _description = iDescription;
            _rid = iMethodRID;
        }
        #endregion
    };

    [DataContract(Name = "ROWorklistViewOut", Namespace = "http://Logility.ROWeb/")]
    public class ROWorklistViewOut
    {

        #region MemberVariables
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _view;

        [DataMember(IsRequired = true)]
        private int _filterKey;

        #endregion

        #region Constructor
        public ROWorklistViewOut(int viewKey, string viewName, int filterKey)
        {
            _view = new KeyValuePair<int, string>(viewKey, viewName);
            _filterKey = filterKey;

        }
        #endregion  

        #region Public Properties
        public KeyValuePair<int, string> View
        {
            get { return _view; }
            set { _view = value; }
        }

        public int FilterKey
        {
            get { return _filterKey; }
            set { _filterKey = value; }
        }


        #endregion
    }

    #region "Reference Classes"
    public class ROAllocationWorklistValues
    {
        public int UserRID { get; set; }

        public int ViewRID { get; set; }

        public string ViewName { get; set; }

        public int FilterRID { get; set; }

        public string FilterName { get; set; }

        public int TotalNumberOfHeaders { get; set; }

        public int TotalQuantityToAllocate { get; set; }

        // Additional fields for Alloction Worklist can be added as needed.
    }

    public class ROAllocationHeaderCharacteristic
    {
        public int HeaderCharGroupKey { get; set; }
        public string HeaderCharGroupName { get; set; }

        public int HeaderCharKey { get; set; }
        public string HeaderCharValue { get; set; }
    }

    #endregion

    [DataContract(Name = "ROAllocationReviewViewDetailsOut", Namespace = "http://Logility.ROWeb/")]
    public class ROAllocationReviewViewDetailsOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private ROAllocationReviewViewDetails _viewDetails;

        public ROAllocationReviewViewDetailsOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, ROAllocationReviewViewDetails ROAllocationReviewViewDetails) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _viewDetails = ROAllocationReviewViewDetails;
        }

        public ROAllocationReviewViewDetails ROAllocationReviewViewDetails { get { return _viewDetails; } }

    }

}