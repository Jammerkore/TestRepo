using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.Data;
using System.IO;  
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
    #region ShipToDayStructure
    /// <summary>
    /// This structure is used to hold the ship-to-day for a store.  Once allocation begins this day does not change.
    /// </summary>
    /// <remarks>Every store has its own ship-to-day.  The ship-to-day is determined at the start of allocation.</remarks>
    [Serializable]
    public struct ShipToDayStructure
    {
        // Note: this structure is identified by eSQL_StructureType.ShipToDayStructure
        private short _storeRID;
        private short _shipToYear;
        private sbyte _shipToMonth;
        private sbyte _shipToDay;
        /// <summary>
        /// Creates an instance of the ShipToDay Structure
        /// </summary>
        /// <param name="aStoreRID">RID that identifies the store</param>
        /// <param name="aShipToDay">The target ship to day for the store (the day on which on-hand is expected to be charged to the store)</param>
        public ShipToDayStructure
            (
            short aStoreRID,
            DateTime aShipToDay
            )
        {
            _storeRID = aStoreRID;
            _shipToYear = (short)aShipToDay.Year;
            _shipToMonth = (sbyte)aShipToDay.Month;
            _shipToDay = (sbyte)aShipToDay.Day;
        }
        /// <summary>
        /// Gets the Store RID
        /// </summary>
        public short StoreRID
        {
            get { return _storeRID; }
        }
        /// <summary>
        /// Gets the Ship-To-Day for the store
        /// </summary>
        public DateTime ShipToDay
        {
            get
            {
                return new DateTime((int)_shipToYear, (int)_shipToMonth, (int)_shipToDay);
            }
        }
    }
    #endregion ShipToDayStructure

    #region GradeStructure
    /// <summary>
    /// This structure holds the grade index associated with a store at allocation time.
    /// </summary>
    [Serializable]
    public struct GradeStructure
    {
        // Note: this structure is identified by eSQL_StructureType.GradeStructure
        private short _storeRID;
        private short _storeGradeIndex;
        /// <summary>
        /// Creates an instance of the Grade Structure
        /// </summary>
        /// <param name="aStoreRID">RID that identifies the store</param>
        /// <param name="aStoreGradeIndex">Grade Index for this store at allocation time</param>
        public GradeStructure
            (
            short aStoreRID,
            short aStoreGradeIndex
            )
        {
            _storeRID = aStoreRID;
            _storeGradeIndex = aStoreGradeIndex;
        }
        /// <summary>
        /// Gets Store RID
        /// </summary>
        public short StoreRID
        {
            get { return _storeRID; }
        }
        /// <summary>
        /// Gets Grade index for this store
        /// </summary>
        public short StoreGradeIndex
        {
            get { return _storeGradeIndex; }
        }
    }
    #endregion GradeStructure

    #region CapacityStructure
    /// <summary>
    /// This structure holds the capacity values that were present for a store at allocation time.
    /// </summary>
    [Serializable]
    public struct CapacityStructure
    {
        // Note: this structure is identified by eSQL_StructureType.CapacityStructure
        private short _storeRID;
        private int _storeCapacity;
        private double _exceedCapacityPercent;
        /// <summary>
        /// Creates an instance of the Capacity Structure
        /// </summary>
        /// <param name="aStoreRID">RID that identifies the store</param>
        /// <param name="aStoreCapacity">Capacity for this store</param>
        /// <param name="aExceedCapacityPercent">The percentage by which this store may exceed capacity</param>
        public CapacityStructure
            (
            short aStoreRID,
            int aStoreCapacity,
            double aExceedCapacityPercent
            )
        {
            _storeRID = aStoreRID;
            _storeCapacity = aStoreCapacity;
            _exceedCapacityPercent = aExceedCapacityPercent;
        }
        /// <summary>
        /// Gets Store RID
        /// </summary>
        public short StoreRID
        {
            get { return _storeRID; }
        }
        /// <summary>
        /// Gets Capacity for this store
        /// </summary>
        public int StoreCapacity
        {
            get { return _storeCapacity; }
        }
        /// <summary>
        /// Gets the percentage by which this store may exceed capacity
        /// </summary>
        public double ExceedCapacityPercent
        {
            get { return _exceedCapacityPercent; }
        }
    }
    #endregion CapacityStructure

    #region GeneralAuditStructure
    /// <summary>
    /// This structure holds the general allocation audit flags.
    /// </summary>
    [Serializable]
    public struct GeneralAuditStructure
    {
        // Note: this structure is identified by eSQL_StructureType.GeneralAuditStructure
        private short _storeRID;
        private int _storeGeneralAuditFlags;

        /// <summary>
        /// Creates an instance of the GeneralAuditStructure
        /// </summary>
        /// <param name="aStoreRID">RID that identifies the store</param>
        /// <param name="aStoreGeneralAuditFlags">Store General Audit Flags (AllocationStoreGeneralAuditFlags cast as integer)</param>
        public GeneralAuditStructure
            (
            short aStoreRID,
            int aStoreGeneralAuditFlags
            )
        {
            _storeRID = aStoreRID;
            _storeGeneralAuditFlags = aStoreGeneralAuditFlags;
        }
        /// <summary>
        /// Gets Store RID
        /// </summary>
        public short StoreRID
        {
            get { return _storeRID; }
        }
        /// <summary>
        /// Gets Store General Audit Flags (cast as AllocationStoreGeneralAuditFlags)
        /// </summary>
        public int StoreGeneralAuditFlags
        {
            get { return _storeGeneralAuditFlags; }
        }
    }
    #endregion GeneralAuditStructure

    #region AllocatedStructure
    /// <summary>
    /// This structure holds the quantity allocated to a store
    /// </summary>
    [Serializable]
    public struct AllocatedStructure
    {
        // Note: this structure is identified by eSQL_StructureType.AllocatedStructure
        private short _storeRID;
        private int _qtyAllocated;

        /// <summary>
        /// Creates an instance of the AllocatedStructure
        /// </summary>
        /// <param name="aStoreRID">RID that identifies the store</param>
        /// <param name="aQtyAllocated">Quantity Allocated to the store</param>
        public AllocatedStructure
            (
            short aStoreRID,
            int aQtyAllocated
            )
        {
            _storeRID = aStoreRID;
            _qtyAllocated = aQtyAllocated;
        }
        /// <summary>
        /// Gets store RID
        /// </summary>
        public short StoreRID
        {
            get { return _storeRID; }
        }
        /// <summary>
        /// Gets Quantity Allocated to the store
        /// </summary>
        public int QtyAllocated
        {
            get { return _qtyAllocated; }
        }
    }
    #endregion AllocatedStructure

    // begin TT#1401 - JEllis - Urban Reservation Stores pt 2
    #region ImoStructure
    /// <summary>
    /// This structure holds the IMO Criteria for a store
    /// </summary>
    [Serializable]
    public struct ImoStructure
    {
        // Note: this structure is identified by eSQL_StructureType.ImoStructure
        private short _storeRID;
        private int _imoMaxValue;
        private int _imoMinShipQty;
        private double _imoPctPackThreshold;

        /// <summary>
        /// Creates an instance of the ImoStructure
        /// </summary>
        /// <param name="aStoreRID">RID that identifies the store</param>
        public ImoStructure
            (
            short aStoreRID,
            int aImoMaxValue,
            int aImoMinShipQty,
            double aImoPctPackThreshold
            )
        {
            _storeRID = aStoreRID;
            _imoMaxValue = aImoMaxValue;
            _imoMinShipQty = aImoMinShipQty;
            _imoPctPackThreshold = aImoPctPackThreshold;
        }
        /// <summary>
        /// Gets store RID
        /// </summary>
        public short StoreRID
        {
            get { return _storeRID; }
        }
        /// <summary>
        /// Gets Imo Max Value to the store
        /// </summary>
        public int ImoMaxValue
        {
            get { return _imoMaxValue; }
        }
        public int ImoMinShipQty
        {
            get { return _imoMinShipQty; }
        }
        public double ImoPctPackThreshold
        {
            get { return _imoPctPackThreshold; }
        }
    }
    #endregion ImoAllocatedStructure


    #region ItemAllocatedStructure
    /// <summary>
    /// This structure holds the item quantity allocated to a store
    /// </summary>
    [Serializable]
    public struct ItemAllocatedStructure
    {
        // Note: this structure is identified by eSQL_StructureType.ItemAllocatedStructure
        private short _storeRID;
        private int _itemQtyAllocated;

        /// <summary>
        /// Creates an instance of the AllocatedStructure
        /// </summary>
        /// <param name="aStoreRID">RID that identifies the store</param>
        /// <param name="aQtyAllocated">Item Quantity Allocated to the store</param>
        public ItemAllocatedStructure
            (
            short aStoreRID,
            int aItemQtyAllocated
            )
        {
            _storeRID = aStoreRID;
            _itemQtyAllocated = aItemQtyAllocated;
        }
        /// <summary>
        /// Gets store RID
        /// </summary>
        public short StoreRID
        {
            get { return _storeRID; }
        }
        /// <summary>
        /// Gets Item Quantity Allocated to the store
        /// </summary>
        public int ItemQtyAllocated
        {
            get { return _itemQtyAllocated; }
        }
    }
    #endregion ItemAllocatedStructure
    // end TT#1401 - JEllis - Urban Reservation Stores pt 2

    #region RuleStructure
    /// <summary>
    /// This structure holds the Chosen Rule for a store
    /// </summary>
    [Serializable]
    public struct RuleStructure
    {
        // Note: this structure is identified by eSQL_StructureType.RuleStructure
        private short _storeRID;
        private int _chosenRuleType;
        private short _chosenRuleLayerID;
        private int _chosenRuleUnits;

        /// <summary>
        /// Creates an instance of the RuleStructure
        /// </summary>
        /// <param name="aStoreRID">RID that identifies the store</param>
        /// <param name="aChosenRuleType">eRuleType (cast as an integer) that identifies the chosen rule assigned to this store</param>
        /// <param name="aChosenRuleLayerID">Rule Layer on which the chosen rule resides</param>
        /// <param name="aChosenRuleUnits">Quantity associated with this rule</param>
        public RuleStructure
            (
            short aStoreRID,
            int aChosenRuleType,
            short aChosenRuleLayerID,
            int aChosenRuleUnits
            )
        {
            _storeRID = aStoreRID;
            _chosenRuleType = aChosenRuleType;
            _chosenRuleLayerID = aChosenRuleLayerID;
            _chosenRuleUnits = aChosenRuleUnits;
        }
        /// <summary>
        /// Gets the Store RID
        /// </summary>
        public short StoreRID
        {
            get { return _storeRID; }
        }
        /// <summary>
        /// Gets the ChosenRuleType (cast as eRuleType)
        /// </summary>
        public int ChosenRuleType
        {
            get { return _chosenRuleType; }
        }
        /// <summary>
        /// Gets Rule layer where the chosen rule resides
        /// </summary>
        public short ChosenRuleLayerID
        {
            get { return _chosenRuleLayerID; }
        }
        /// <summary>
        /// Gets Chosen Rule Units
        /// </summary>
        public int ChosenRuleUnits
        {
            get { return _chosenRuleUnits; }
        }
    }
    #endregion RuleStructure

    #region AllocatedAuditStructure
    /// <summary>
    /// This structure holds an audit of the quantity allocated to a store
    /// </summary>
    [Serializable]
    public struct AllocatedAuditStructure
    {
        // Note: this structure is identified by eSQL_StructureType.AllocatedAuditStructure
        private short _storeRID;
        private int _qtyAllocatedByRule;
        private int _qtyAllocatedByAuto;

        /// <summary>
        /// Creates an instance of the AllocatedAuditStructure
        /// </summary>
        /// <param name="aStoreRID">RID that identifies the store</param>
        /// <param name="aQtyAllocatedByAuto">Quantity Allocated by automated actions and methods to this store</param>
        /// <param name="aQtyAllocatedByRule">Quantity Allocated to store as a result of a rule (from parent, child or this component)</param>
        public AllocatedAuditStructure
            (
            short aStoreRID,
            int aQtyAllocatedByAuto,
            int aQtyAllocatedByRule
            )
        {
            _storeRID = aStoreRID;
            _qtyAllocatedByRule = aQtyAllocatedByRule;
            _qtyAllocatedByAuto = aQtyAllocatedByAuto;
        }
        /// <summary>
        /// Gets store RID
        /// </summary>
        public short StoreRID
        {
            get { return _storeRID; }
        }
        /// <summary>
        /// Gets Quantity Allocated By Rule to the store
        /// </summary>
        public int QtyAllocatedByRule
        {
            get { return _qtyAllocatedByRule; }
        }
        /// <summary>
        /// Gets Quantity Allocated to the store by Automated actions and methods
        /// </summary>
        public int QtyAllocatedByAuto
        {
            get { return _qtyAllocatedByAuto; }
        }
    }
    #endregion AllocatedAuditStructure

    #region NeedAuditStructure
    /// <summary>
    /// This structure holds the need audit before a need action
    /// </summary>
    [Serializable]
    public struct NeedAuditStructure
    {
        // Note: this structure is identified by eSQL_StructureType.NeedAuditStructure
        private short _storeRID;
        private short _needYear;
        private sbyte _needMonth;
        private sbyte _needDay;
        private int _unitNeedBefore;
        private double _percentNeedBefore;

        /// <summary>
        /// Creates an instance of the NeedAuditStructure
        /// </summary>
        /// <param name="aStoreRID">RID that identifies the store</param>
        /// <param name="aNeedDay">The last Need Day used for a store during a need action</param>
        /// <param name="aUnitNeedBefore">The need before a need action</param>
        /// <param name="aPercentNeedBefore">The percent need before a need action</param>
        public NeedAuditStructure
            (
            short aStoreRID,
            DateTime aNeedDay,
            int aUnitNeedBefore,
            double aPercentNeedBefore
            )
        {
            _storeRID = aStoreRID;
            _needYear = (short)aNeedDay.Year;
            _needMonth = (sbyte)aNeedDay.Month;
            _needDay = (sbyte)aNeedDay.Day;
            _unitNeedBefore = aUnitNeedBefore;
            _percentNeedBefore = aPercentNeedBefore;
        }
        /// <summary>
        /// Gets Store RID
        /// </summary>
        public short StoreRID
        {
            get { return _storeRID; }
        }
        /// <summary>
        /// Gets the last need day used for the store during a need action
        /// </summary>
        public DateTime NeedDay
        {
            get
            {
                return new DateTime((int)_needYear, (int)_needMonth, (int)_needDay);
            }
        }
        /// <summary>
        /// Gets the unit need of the store before the need action occurred
        /// </summary>
        public int UnitNeedBefore
        {
            get { return _unitNeedBefore; }
        }
        /// <summary>
        /// Gets the percent need of the store before the need action occurred
        /// </summary>
        public double PercentNeedBefore
        {
            get { return _percentNeedBefore; }
        }
    }
    #endregion NeedAuditStructure

    #region MinimumStructure
    /// <summary>
    /// This structure holds a store's minimum constraint
    /// </summary>
    [Serializable]
    public struct MinimumStructure
    {
        // Note: this structure is identified by eSQL_StructureType.MinimumStructure
        private short _storeRID;
        private int _minimum;

        /// <summary>
        /// Creates an instance of the MinimumStructure
        /// </summary>
        /// <param name="aStoreRID">RID that identifies the store</param>
        /// <param name="aMinimum">Minimum constraint for this store</param>
        public MinimumStructure
            (
            short aStoreRID,
            int aMinimum
            )
        {
            _storeRID = aStoreRID;
            _minimum = aMinimum;
        }
        /// <summary>
        /// Gets Store RID
        /// </summary>
        public short StoreRID
        {
            get { return _storeRID; }
        }
        /// <summary>
        /// Gets the Minimum Constraint for this store
        /// </summary>
        public int Minimum
        {
            get { return _minimum; }
        }
    }
    #endregion MinimumStructure

    #region MaximumStructure
    /// <summary>
    /// This structure hold the maximum constraint
    /// </summary>
    [Serializable]
    public struct MaximumStructure
    {
        // Note: this structure is identified by eSQL_StructureType.MaximumStructure
        private short _storeRID;
        private int _maximum;

        /// <summary>
        /// Creates an instance of the MaximumStructure
        /// </summary>
        /// <param name="aStoreRID">RID that identifies the store</param>
        /// <param name="aMaximum">Maximum Constraint for this store</param>
        public MaximumStructure
            (
            short aStoreRID,
            int aMaximum
            )
        {
            _storeRID = aStoreRID;
            _maximum = aMaximum;
        }
        /// <summary>
        /// Gets Store RID
        /// </summary>
        public short StoreRID
        {
            get { return _storeRID; }
        }
        /// <summary>
        /// Gets the store's Maximum constraint
        /// </summary>
        public int Maximum
        {
            get { return _maximum; }
        }
    }
    #endregion MaximumStructure

    #region PrimaryMaxStructure
    /// <summary>
    /// This structure holds the Primary Maximum constraint
    /// </summary>
    [Serializable]
    public struct PrimaryMaxStructure
    {
        // Note: this structure is identified by eSQL_StructureType.PrimaryMaxStructure
        private short _storeRID;
        private int _primaryMax;

        /// <summary>
        /// Creates an instance of the PrimaryMaxStructure
        /// </summary>
        /// <param name="aStoreRID">RID that identifies the store</param>
        /// <param name="aPrimaryMax">Primary Maximum constraint for this store</param>
        public PrimaryMaxStructure
            (
            short aStoreRID,
            int aPrimaryMax
            )
        {
            _storeRID = aStoreRID;
            _primaryMax = aPrimaryMax;
        }
        /// <summary>
        /// Gets Store RID
        /// </summary>
        public short StoreRID
        {
            get { return _storeRID; }
        }
        /// <summary>
        /// Gets store's primary maximum constraint
        /// </summary>
        public int PrimaryMax
        {
            get { return _primaryMax; }
        }
    }
    #endregion PrimaryMaxStructure

    #region DetailAuditStructure
    /// <summary>
    /// This structure holds the detail audit flags for a store
    /// </summary>
    [Serializable]
    public struct DetailAuditStructure
    {
        // Note: this structure is identified by eSQL_StructureType.DetailAuditStructure
        private short _storeRID;
        private int _storeDetailAuditFlags;

        /// <summary>
        /// Creates an instance of the DetailAuditStructure
        /// </summary>
        /// <param name="aStoreRID">RID that identifies the store</param>
        /// <param name="aStoreDetailAuditFlags">Store Detail Audit Flags (AllocationStoreDetailAuditFlags cast as an integer)</param>
        public DetailAuditStructure
            (
            short aStoreRID,
            int aStoreDetailAuditFlags
            )
        {
            _storeRID = aStoreRID;
            _storeDetailAuditFlags = aStoreDetailAuditFlags;
        }
        /// <summary>
        /// Gets Store RID
        /// </summary>
        public short StoreRID
        {
            get { return _storeRID; }
        }
        /// <summary>
        /// Gets StoreDetailAuditFlags (cast as AllocationStoreDetailAuditFlags)
        /// </summary>
        public int StoreDetailAuditFlags
        {
            get { return _storeDetailAuditFlags; }
        }
    }
    #endregion DetailAuditStructure

    #region ShippingStructure
    /// <summary>
    /// This structure holds the shipping results for a store
    /// </summary>
    [Serializable]
    public struct ShippingStructure
    {
        // Note: this structure is identified by eSQL_StructureType.ShippingStructure
        private short _storeRID;
        private int _shippingStatusFlags;
        private int _qtyShipped;
        /// <summary>
        /// Creates an instance of the ShippingStructure
        /// </summary>
        /// <param name="aStoreRID">RID that identifies the store</param>
        /// <param name="aShippingStatusFlags">Sthipping Status Flags for this store (ShippingStatusFlags cast as an integer)</param>
        /// <param name="aQtyShipped">Quantity shipped to store</param>
        public ShippingStructure
            (
            short aStoreRID,
            int aShippingStatusFlags,
            int aQtyShipped
            )
        {
            _storeRID = aStoreRID;
            _shippingStatusFlags = aShippingStatusFlags;
            _qtyShipped = aQtyShipped;
        }
        /// <summary>
        /// Gets Store RID
        /// </summary>
        public short StoreRID
        {
            get { return _storeRID; }
        }
        /// <summary>
        /// Gets Shipping Status Flags (cast as ShippingStatusFlags)
        /// </summary>
        public int ShippingStatusFlags
        {
            get
            {
                return _shippingStatusFlags;
            }
        }
        /// <summary>
        /// Gets Quantity Shipped
        /// </summary>
        public int QtyShipped
        {
            get { return _qtyShipped; }
        }
    }
    #endregion ShippingStructure

    #region StoreAllocationStructureStatus
    /// <summary>
    /// This struct holds 
    /// 1. A bool that indicates whether SqlStructure data are from the expanded table or from the binarey table
    /// 2. An array of bools that indicates whether a given SQL structure has changed (in the Allocation Profile).
    /// </summary>
    public struct StoreAllocationStructureStatus
    {
        private bool _fromExpandedTable;
        private bool[] _sqlStructureChangeStatus;

        public StoreAllocationStructureStatus(bool aFromExpandedTable)
        {
            _fromExpandedTable = aFromExpandedTable;
            _sqlStructureChangeStatus = new bool[Include.SQL_StructureTypes.Length];
        }

        #region Properties
        /// <summary>
        /// Identifies whether the underlying information in the SQL Structures is from the expanded tables on the database
        /// </summary>
        internal bool FromExpandedTable
        {
            get { return _fromExpandedTable; }
            set { _fromExpandedTable = value; }
        }

        #endregion Properties
        #region Methods
        /// <summary>
        /// Gets change status of a given structure type
        /// </summary>
        /// <param name="aSQL_StructureType"></param>
        /// <returns></returns>
        public bool GetSQL_StructureChange(eSQL_StructureType aSQL_StructureType)
        {
            return _sqlStructureChangeStatus[(int)aSQL_StructureType];
        }
        /// <summary>
        /// Sets change status of a given structure type
        /// </summary>
        /// <param name="aSQL_StructureType">SQL_StructureType</param>
        /// <param name="aChanged">True: Structure has changed; False: Structure has not changed</param>
        public void SetSQL_StructureChange(eSQL_StructureType aSQL_StructureType, bool aChanged)
        {
            _sqlStructureChangeStatus[(int)aSQL_StructureType] = aChanged;
        }
        #endregion Methods
    }
    #endregion StoreAllocationStructureStatus

    // begin TT#246 - JEllis - AnF VSW In-Store Minimum
    #region ItemMinimumStructure
    /// <summary>
    /// This structure holds a store's Item Minimum constraint
    /// </summary>
    [Serializable]
    public struct ItemMinimumStructure
    {
        // Note: this structure is identified by eSQL_StructureType.ItemMinimumStructure
        private short _storeRID;
        private int _itemMinimum;
        private int _itemIdealMinimum;  // TT#246 - MD -  JEllis - AnF VSW In Store Minimum phase 2

        /// <summary>
        /// Creates an instance of the ItemMinimumStructure
        /// </summary>
        /// <param name="aStoreRID">RID that identifies the store</param>
        /// <param name="aItemMinimum">ItemMinimum constraint for this store</param>
        public ItemMinimumStructure
            (
            short aStoreRID,
            int aItemMinimum,
            int aItemIdealMinimum // TT#246 - MD - Jellis - AnF VSW In Store Minimum phase 2
            )
        {
            _storeRID = aStoreRID;
            _itemMinimum = aItemMinimum;
            _itemIdealMinimum = aItemIdealMinimum; // TT#246 - MD - JEllis - AnF VSW In Store Minimum phase 2
        }
        /// <summary>
        /// Gets Store RID
        /// </summary>
        public short StoreRID
        {
            get { return _storeRID; }
        }
        /// <summary>
        /// Gets the ItemMinimum Constraint for this store
        /// </summary>
        public int ItemMinimum
        {
            get { return _itemMinimum; }
        }
        // begin TT#246 - MD - JEllis - AnF VSW In Store Minimum phase 2
        public int ItemIdealMinimum
        {
            get { return _itemIdealMinimum; }
        }
        // end TT#246 - MD - JEllis - AnF VSW In Store Minimum phase 2
    }
    #endregion ItemMinimumStructure

    // end TT#246 - JEllis - AnF VSW In-Store Minimum
}