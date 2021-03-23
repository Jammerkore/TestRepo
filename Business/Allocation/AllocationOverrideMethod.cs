using System;
using System.Collections;
using System.Collections.Generic; // TT#488 - MD - Jellis - Group Allocation
using System.Data;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using Logility.ROWebSharedTypes;


namespace MIDRetail.Business.Allocation
{
	/// <summary>
	/// Defines the overrides to general criteria to drive the allocation process.
	/// </summary>
	/// <remarks>
	/// This is the business Allocation Override Method.  It inherits from the business AllocationBaseMethod.
	/// </remarks>
	public class AllocationOverrideMethod:AllocationBaseMethod
	{
		//=======
		// FIELDS
		//=======
		private AllocationOverrideMethodData _mao;
        private DataSet _dsOverRide;
        private int _storeFilterRID;
        private DataSet _imoDataSet;
        private IMOProfileList _imoProfileList;
        private ProfileList _imoGroupLevelList;
        private IMOMethodOverrideProfileList _imoMethodOverrideProfileList;
        private bool _applyVSW;
        private MIDException StatusMessage; 

        private AllocationCriteria _allocationCriteria;

        //private int _storeFilterRID;
        //private int _gradeWeekCount;
        //private bool _exceedMaximums;
        //private double _percentNeedLimit;
        //private bool _reserveQtyIsPercent;
        //private double _reserveQty;
        //private AllocationMerchBin _OTSPlan;
        //private AllocationMerchBin _OTSOnHand;
        //private double _OTSPlanFactorPercent;
        //private int _allColorMultiple;
        //private int _allSizeMultiple;
        //private MinMaxAllocationBin _allColor;
        //// begin TT#488 - MD - Jellis - Group Allocation
        ////private Hashtable _colorMinMax;
        ////private Hashtable _colorSizeMinMax;
        ////private Hashtable _grade;
        //private Dictionary<int, ColorOrSizeMinMaxBin> _colorMinMax;
        //private Dictionary<int, ColorSizeMinMaxBin> _colorSizeMinMax;
        //private Dictionary<int, Dictionary<double, AllocationGradeBin>> _grade;
        //// end TT#488 - MD - Jellis - Group Allocation
        //private bool _exceedCapacity;
        ////private Hashtable _sglCapacity; // TT#488 - MD - Jellis - Group Allocation
        //private Dictionary<int, AllocationCapacityBin> _sglCapacity; // TT#488 - MD - Jellis - Group Allocation
        //private int _storeGroupRID;
        //private int _sGstoreGroupRID;   // TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
        //private bool _useStoreGradeDefault;
        //private bool _usePctNeedDefault;
        //private bool _useFactorPctDefault;
        //private bool _useAllColorsMinDefault;
        //private bool _useAllColorsMaxDefault;
        //private DataSet _dsOverRide;
        //// Begin TT#2731 - JSmith - Unable to copy allocation override method from global
        //private DataSet _imoDataSet; // TT#1401 - GTaylor - Reservation Stores
        //private IMOProfileList _imoProfileList;
        //private ProfileList _imoGroupLevelList;
        //// End TT#2731 - JSmith - Unable to copy allocation override method from global
        //private IMOMethodOverrideProfileList _imoMethodOverrideProfileList;
        //private bool _applyVSW; // TT#1401 - GTaylor - Reservation Stores
        //private bool _merchUnspecified;      // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level
        //private bool _onHandUnspecified;     // End TT#709 
        //// BEGIN TT#667 - Stodd - Pre-allocate Reserve
        //private double _reserveAsBulk;
        //private double _reserveAsPacks;
        //// END TT#667 - Stodd - Pre-allocate Reserve
        //// BEGIN TT#1287 - AGallagher - Inventory Min/Max
        //private bool InventoryMinMax;
        //private int InventoryBasisHnRID;
        ////private string StatusMessage;     // TT#488 - MD - Jellis - Group Allocation
        //private MIDException StatusMessage; // TT#488 - MD - Jellis - Group Allocation
        //private char _InventoryInd;
        //private int _MERCH_HN_RID;
        //private int _MERCH_PH_RID;
        //private int _MERCH_PHL_SEQ;
        //private eMerchandiseType _merchandiseType2;  
        //// END TT#1287 - AGallagher - Inventory Min/Max

		
		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates an instance of the AllocationOverrideMethod.
		/// </summary>
		/// <param name="aSAB">Session Address Block.</param>
		/// <param name="aMethodRID">RID that identifies the Method.</param>
		//Begin TT#523 - JScott - Duplicate folder when new folder added
		//public AllocationOverrideMethod(SessionAddressBlock aSAB, int aMethodRID): base (aSAB, aMethodRID, eMethodType.AllocationOverride)
		public AllocationOverrideMethod(SessionAddressBlock aSAB, int aMethodRID)
            : base (aSAB, aMethodRID, eMethodType.AllocationOverride, eProfileType.MethodAllocationOverride)
		//End TT#523 - JScott - Duplicate folder when new folder added
		{
			_storeFilterRID = Include.AllStoreFilterRID;
            _mao = new AllocationOverrideMethodData(aMethodRID);
            _allocationCriteria = new AllocationCriteria(aSAB);
            _imoDataSet = null;
			_applyVSW = true;
            if (base.Filled)
            {
                _mao.PopulateAllocationOverride(aMethodRID);
                _allocationCriteria.UseStoreGradeDefault = _mao.UseStoreGradeDefault;
                if (!_allocationCriteria.UseStoreGradeDefault)
                {
                    _allocationCriteria.GradeWeekCount = _mao.Grade_Week_Count;
                }
                _allocationCriteria.ExceedMaximums = Include.ConvertCharToBool(_mao.Exceed_Maximums_Ind);
                _allocationCriteria.UsePctNeedDefault = _mao.UsePctNeedDefault;
                if (!_allocationCriteria.UsePctNeedDefault)
                {
                    _allocationCriteria.PercentNeedLimit = _mao.Percent_Need_Limit;
                }
                _allocationCriteria.ReserveIsPercent = Include.ConvertCharToBool(_mao.Percent_Ind);
                _allocationCriteria.ReserveQty = _mao.Reserve;
                _allocationCriteria.ReserveAsPacks = _mao.ReserveAsPacks;
                _allocationCriteria.ReserveAsBulk = _mao.ReserveAsBulk;
                _allocationCriteria.MerchUnspecified = _mao.Merch_Plan_Unspecified;
                _allocationCriteria.OTSPlanRID = _mao.Merch_Plan_HN_RID;
                _allocationCriteria.OTSPlanPHL = _mao.Merch_Plan_PH_RID;
                _allocationCriteria.OTSPlanPHLSeq = _mao.Merch_Plan_PHL_SEQ;
                _allocationCriteria.OnHandUnspecified = _mao.Merch_OnHand_Unspecified;
                _allocationCriteria.OTSOnHandRID = _mao.Merch_OnHand_HN_RID;
                _allocationCriteria.OTSOnHandPHL = _mao.Merch_OnHand_PH_RID;
                _allocationCriteria.OTSOnHandPHLSeq = _mao.Merch_OnHand_PHL_SEQ;
                _allocationCriteria.UseFactorPctDefault = _mao.UseFactorPctDefault;
                if (!_allocationCriteria.UseFactorPctDefault)
                {
                    _allocationCriteria.OTSPlanFactorPercent = _mao.Plan_Factor_Percent;
                }
                _allocationCriteria.AllColorMultiple = _mao.All_Color_Multiple;
                _allocationCriteria.AllSizeMultiple = _mao.All_Size_Multiple;
                _allocationCriteria.UseAllColorsMaxDefault = _mao.UseAllColorsMaxDefault;
                if (!_allocationCriteria.UseAllColorsMaxDefault)
                {
                    _allocationCriteria.AllColorMaximum = _mao.All_Color_Maximum;
                }
                _allocationCriteria.UseAllColorsMinDefault = _mao.UseAllColorsMinDefault;
                if (!_allocationCriteria.UseAllColorsMinDefault)
                {
                    _allocationCriteria.AllColorMinimum = _mao.All_Color_Minimum;
                }
                _allocationCriteria.CapacityStoreGroupRID = _mao.Store_Group_RID;
                _allocationCriteria.ExceedCapacity = Include.ConvertCharToBool(_mao.Exceed_Capacity_Ind); // TT#1012 - MD - Jellis - Exceed Capacity Broken
                _allocationCriteria.GradeStoreGroupRID = _mao.Tab_Store_Group_RID;
                // begin TT#1140 - MD - Jellis -  Group Allocation - Header Inventory Min Max Options
                //_allocationCriteria.InventoryInd = _mao.Inventory_Ind;
                //_allocationCriteria.Inventory_MERCH_HN_RID = _mao.IB_MERCH_HN_RID;
                //_allocationCriteria.Inventory_MERCH_PH_RID = _mao.IB_MERCH_PH_RID;
                //_allocationCriteria.Inventory_MERCH_PHL_SEQ = _mao.IB_MERCH_PHL_SEQ;
                _allocationCriteria.HdrInventoryInd = _mao.Inventory_Ind;
                _allocationCriteria.HdrInventory_MERCH_HN_RID = _mao.IB_MERCH_HN_RID;
                _allocationCriteria.HdrInventory_MERCH_PH_RID = _mao.IB_MERCH_PH_RID;
                _allocationCriteria.HdrInventory_MERCH_PHL_SEQ = _mao.IB_MERCH_PHL_SEQ;
                // end TT#1140 - MD - Jellis - Group Allocation - Header Inventory Min Max Options
                _imoDataSet = _mao.IMODataSet;
				_applyVSW = _mao.ApplyVSW;
            }
            _dsOverRide = _mao.GetOverRideChildData();
            LoadTables();
        }

        //    if (base.Filled)
        //    {
        //        //_mao = new AllocationOverrideMethodData(aMethodRID);
        //        _mao.PopulateAllocationOverride(aMethodRID);
        //        _storeFilterRID = _mao.Store_Filter_RID;
                
        //        if (_mao.UseStoreGradeDefault)
        //            _gradeWeekCount = SAB.ApplicationServerSession.GlobalOptions.StoreGradePeriod;
					
        //        else
        //            _gradeWeekCount = _mao.Grade_Week_Count;
				
        //        _exceedMaximums = Include.ConvertCharToBool(_mao.Exceed_Maximums_Ind);

        //        if (_mao.UsePctNeedDefault)
        //            _percentNeedLimit = SAB.ApplicationServerSession.GlobalOptions.PercentNeedLimit;
        //        else
        //            _percentNeedLimit = _mao.Percent_Need_Limit;

        //        _reserveQtyIsPercent = Include.ConvertCharToBool(_mao.Percent_Ind);
        //        _reserveQty = (double)_mao.Reserve;
        //        _OTSPlan.SetMdseHnRID(_mao.Merch_Plan_HN_RID);
        //        _OTSPlan.SetProductHnLvlRID(_mao.Merch_Plan_PH_RID);
        //        _OTSPlan.SetProductHnLvlSeq(_mao.Merch_Plan_PHL_SEQ);
        //        _OTSOnHand.SetMdseHnRID(_mao.Merch_OnHand_HN_RID);
        //        _OTSOnHand.SetProductHnLvlRID(_mao.Merch_OnHand_PH_RID);
        //        _OTSOnHand.SetProductHnLvlSeq(_mao.Merch_OnHand_PHL_SEQ);
        //        if (_mao.UseFactorPctDefault)
        //            _OTSPlanFactorPercent = Include.DefaultPlanFactorPercent;
        //        else
        //            _OTSPlanFactorPercent = _mao.Plan_Factor_Percent;
				
        //        _allColorMultiple = _mao.All_Color_Multiple;
        //        _allSizeMultiple = _mao.All_Size_Multiple;
				
        //        if (_mao.UseAllColorsMaxDefault)
        //            _allColor.SetMaximum(_allColor.LargestMaximum);
        //        else
        //            _allColor.SetMaximum(_mao.All_Color_Maximum);
				
        //        if (_mao.UseAllColorsMinDefault)
        //            _allColor.SetMinimum(0);
        //        else
        //            _allColor.SetMinimum(_mao.All_Color_Minimum);

        //        _storeGroupRID = _mao.Store_Group_RID;
        //        _sGstoreGroupRID = _mao.Tab_Store_Group_RID;    // TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
        //        _exceedCapacity = Include.ConvertCharToBool(_mao.Exceed_Capacity_Ind);
        //        _useStoreGradeDefault = _mao.UseStoreGradeDefault;
        //        _usePctNeedDefault = _mao.UsePctNeedDefault;
        //        _useFactorPctDefault = _mao.UseFactorPctDefault;
        //        _useAllColorsMinDefault = _mao.UseAllColorsMinDefault;
        //        _useAllColorsMaxDefault = _mao.UseAllColorsMaxDefault;
        //        // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level
        //        _merchUnspecified = _mao.Merch_Plan_Unspecified;
        //        _onHandUnspecified = _mao.Merch_OnHand_Unspecified;   
        //        // End TT#709 
        //        // BEGIN TT#667 - Stodd - Pre-allocate Reserve
        //        _reserveAsBulk = _mao.ReserveAsBulk;
        //        _reserveAsPacks = _mao.ReserveAsPacks;
        //        // END TT#667 - Stodd - Pre-allocate Reserve
        //        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        //        _InventoryInd = _mao.Inventory_Ind;
        //        _MERCH_HN_RID = _mao.IB_MERCH_HN_RID;
        //        _MERCH_PH_RID = _mao.IB_MERCH_PH_RID;
        //        _MERCH_PHL_SEQ = _mao.IB_MERCH_PHL_SEQ;
        //        // END TT#1287 - AGallagher - Inventory Min/Max
        //        // BEGIN TT#1401 - stodd - VSW
        //        _imoDataSet = _mao.IMODataSet;
        //        _applyVSW = _mao.ApplyVSW;
        //        // END TT#1401 - stodd - VSW
        //    }
        //    else
        //    {
        //        _storeFilterRID = Include.AllStoreFilterRID;
        //        _gradeWeekCount = SAB.ApplicationServerSession.GlobalOptions.StoreGradePeriod;
        //        _exceedMaximums = false;
        //        _percentNeedLimit = SAB.ApplicationServerSession.GlobalOptions.PercentNeedLimit;
        //        _reserveQtyIsPercent = false;
        //        _reserveQty = 0;
        //        _OTSPlan.SetMdseHnRID(Include.DefaultPlanHnRID);
        //        _OTSPlan.SetProductHnLvlRID(Include.DefaultPlanHnRID);
        //        _OTSPlan.SetProductHnLvlSeq(0);
        //        _OTSOnHand.SetMdseHnRID(Include.DefaultPlanHnRID);
        //        _OTSOnHand.SetProductHnLvlRID(Include.DefaultPlanHnRID);
        //        _OTSOnHand.SetProductHnLvlSeq(0);
        //        _OTSPlanFactorPercent = Include.DefaultPlanFactorPercent;;
        //        _allColorMultiple = 1;
        //        _allSizeMultiple = 1;
        //        _allColor.SetMinimum(0);
        //        _allColor.SetMaximum(_allColor.LargestMaximum);
        //        _exceedCapacity = false;
        //        //_grade = null; // TT#488 - 
        //        _storeGroupRID = SAB.ApplicationServerSession.GlobalOptions.AllocationStoreGroupRID;
        //        // BEGIN TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
        //        _sGstoreGroupRID = SAB.ApplicationServerSession.GlobalOptions.AllocationStoreGroupRID;
        //        // END TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
        //        _useStoreGradeDefault = true;
        //        _usePctNeedDefault = true;
        //        _useFactorPctDefault = true;
        //        _useAllColorsMinDefault = true;
        //        _useAllColorsMaxDefault = true;
        //        // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level
        //        _merchUnspecified = true;
        //        _onHandUnspecified = true;
        //        // End TT#709 
        //        // BEGIN TT#667 - Stodd - Pre-allocate Reserve
        //        _reserveAsBulk = 0;
        //        _reserveAsPacks = 0;
        //        // END TT#667 - Stodd - Pre-allocate Reserve
        //        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        //        // BEGIN TT#1566 - AGallagher - Velocity inventory basis is sub class (same as ots forecast)  Not getting expected results
        //        _InventoryInd = 'A';
        //        //_MERCH_HN_RID = Include.NoRID;
        //        //_MERCH_PH_RID = 1;
        //        //_MERCH_PHL_SEQ = 5;
        //        HierarchyProfile hp = aSAB.HierarchyServerSession.GetMainHierarchyData();
        //        // BEGIN TT#1401 - stodd - VSW
        //        _imoDataSet = null;
        //        _applyVSW = true;
        //        // END TT#1401 - stodd - VSW
        //        for (int levelIndex = 1; levelIndex <= hp.HierarchyLevels.Count; levelIndex++)
        //        {
        //            HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[levelIndex];
        //            //hlp.LevelID is level name 
        //            //hlp.Level is level number 
        //            //hlp.LevelType is level type 
        //            if (hlp.LevelType == eHierarchyLevelType.Style)
        //            {
        //                //this._MerchType = eMerchandiseType.HierarchyLevel;
        //                _MERCH_PHL_SEQ = hlp.Level;
        //                _MERCH_PH_RID = hp.Key;
        //                _MERCH_HN_RID = Include.NoRID;
        //                //styleFound = true;
        //                break;
        //            }
        //        }
        //        // END TT#1566 - AGallagher - Velocity inventory basis is sub class (same as ots forecast)  Not getting expected results
        //        // END TT#1287 - AGallagher - Inventory Min/Max
        //    }
        //    _dsOverRide = _mao.GetOverRideChildData();
        //    // BEGIN TT#1401 - stodd - VSW
        //    // BEGIN TT#1401 - GTaylor - Reservation Stores
        //    //_imoDataSet = _mao.IMODataSet;
        //    //_applyVSW = _mao.ApplyVSW;
        //    // END TT#1401 - GTaylor - Reservation Stores
        //    // END TT#1401 - stodd - VSW
        //    LoadHashTables();
        //}

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets Profile Type of this profile.
		/// </summary>
		public override eProfileType ProfileType
		{
			get
			{
				return eProfileType.MethodAllocationOverride;
			}
		}

		/// <summary>
		/// Gets or sets the StoreFilterRID.
		/// </summary>
		/// <remarks>
		/// This filter overrides the filter on the workflow.
		/// </remarks>
		public int StoreFilterRID
		{
			get
			{
				return _storeFilterRID;
			}
			set
			{
				_storeFilterRID = value;
			}
		}

        /// <summary>
        /// Gets or sets the number of weeks to use to calculate grades.
        /// </summary>
        public int GradeWeekCount
        {
            get { return _allocationCriteria.GradeWeekCount; }
            set { _allocationCriteria.GradeWeekCount = value; }
        }

        /// <summary>
        /// Gets or sets the ExceedMaximums flag value.
        /// </summary>
        /// <remarks>
        /// True indicates that an allocation may exceed the "grade" maximum.
        /// </remarks>
        public bool ExceedMaximums
        {
            get { return _allocationCriteria.ExceedMaximums; }
            set { _allocationCriteria.ExceedMaximums = value; }
        }

        /// <summary>
        /// Gets or sets the percent need limit.
        /// </summary>
        /// <remarks>
        /// Controls the need algorithm. The need allocation process will stop for stores whose percent need is equal to or less than this limit.
        /// The first time a store achieves this limit during the current process, the store will get no additional units.
        /// </remarks>
        public double PercentNeedLimit
        {
            get { return _allocationCriteria.PercentNeedLimit; }
            set { _allocationCriteria.PercentNeedLimit = value; }
        }

        /// <summary>
        /// Gets or sets ReserveIsPercent flag value.
        /// </summary>
        public bool ReserveIsPercent
        {
            get { return _allocationCriteria.ReserveIsPercent; }
            set { _allocationCriteria.ReserveIsPercent = value; }
        }

        /// <summary>
        /// Gets or sets ReserveQty.
        /// </summary>
        /// <remarks>
        /// This quantity is a percent when ReserveIsPercet is true; otherwise, it is a unit value.
        /// </remarks>
        public double ReserveQty
        {
            get { return _allocationCriteria.ReserveQty; }
            set { _allocationCriteria.ReserveQty = value; }
        }

        /// <summary>
        /// Gets or sets OTSPlanPHL which identifies a dynamic product level within the heirarchy as a source of plans.
        /// </summary>
        /// <remarks>
        /// This variable identifies the level within the hierarchy where the system should look for plans.  The plan chosen is relative to the style on the header.
        /// </remarks>
        public int OTSPlanPHL
        {
            get { return _allocationCriteria.OTSPlanPHL; }
            set { _allocationCriteria.OTSPlanPHL = value; }
        }
        public int OTSPlanPHLSeq
        {
            get { return _allocationCriteria.OTSPlanPHLSeq; }
            set { _allocationCriteria.OTSPlanPHLSeq = value; }
        }
        /// <summary>
        /// Gets or sets OTSPlanRID which identifies a specific merchandise plan within the hierarchy as a source of plans.
        /// </summary>
        public int OTSPlanRID
        {
            get { return _allocationCriteria.OTSPlanRID; }
            set { _allocationCriteria.OTSPlanRID = value; }
        }

        /// <summary>
        /// Gets or sets the OTSOnHandPHL which identifies a dynamic product level within the hierarchy as a source for store actual Onhands and Intransit. The merchandise chosen at this level will be relative to the header's style.
        /// </summary>
        public int OTSOnHandPHL
        {
            get { return _allocationCriteria.OTSOnHandPHL; }
            set { _allocationCriteria.OTSOnHandPHL = value; }
        }
        public int OTSOnHandPHLSeq
        {
            get { return _allocationCriteria.OTSOnHandPHLSeq; }
            set { _allocationCriteria.OTSOnHandPHLSeq = value; }
        }

        /// <summary>
        /// Gets or sets the OTSOnHandRID which identifies a specific merchandise node within the hierarchy as the source for store actual onhands and intransit.
        /// </summary>
        public int OTSOnHandRID
        {
            get { return _allocationCriteria.OTSOnHandRID; }
            set { _allocationCriteria.OTSOnHandRID = value; }
        }

        /// <summary>
        /// Gets or sets the OTSPlanFactorPercent.
        /// </summary>
        /// <remarks>
        /// This percent is used to extract the part of a plan that the onhand source represents.
        /// </remarks>
        public double OTSPlanFactorPercent
        {
            get { return _allocationCriteria.OTSPlanFactorPercent; }
            set { _allocationCriteria.OTSPlanFactorPercent = value; }
        }
        /// <summary>
        /// Gets or sets the all color multiple.
        /// </summary>
        public int AllColorMultiple
        {
            get { return _allocationCriteria.AllColorMultiple; }
            set { _allocationCriteria.AllColorMultiple = value; }
        }

        /// <summary>
        /// Gets or sets the all size multiple.
        /// </summary>
        public int AllSizeMultiple
        {
            get { return _allocationCriteria.AllSizeMultiple; }
            set { _allocationCriteria.AllSizeMultiple = value; }
        }

        /// <summary>
        /// Gets or sets the all color minimum allocation constraint.
        /// </summary>
        public int AllColorMinimum
        {
            get { return _allocationCriteria.AllColorMinimum; }
            set { _allocationCriteria.AllColorMinimum = value; }
        }

        /// <summary>
        /// Gets or sets the all color maximum allocation constraint.
        /// </summary>
        public int AllColorMaximum
        {
            get { return _allocationCriteria.AllColorMaximum; }
            set { _allocationCriteria.AllColorMaximum = value; }
        }
        /// <summary>
        /// Gets or sets the ExceedCapacity flag value.
        /// </summary>
        /// <remarks>
        /// True lets the system allocate more than a store's capacity for ALL stores; False prevents the system from allocating more than capacity.
        /// </remarks>
        public bool ExceedCapacity
        {
            get { return _allocationCriteria.ExceedCapacity; }
            set { _allocationCriteria.ExceedCapacity = value; }
        }
        /// <summary>
        /// Gets or sets the Capacity StoreGroupRID.
        /// </summary>
        public int CapacityStoreGroupRID
        {
            get { return _allocationCriteria.CapacityStoreGroupRID; }
            set { _allocationCriteria.CapacityStoreGroupRID = value; }
        }

        /// <summary>
        /// Gets or set the Store Grade StoreGroupRID
        /// </summary>
        public int GradeStoreGroupRID
        {
            get { return _allocationCriteria.GradeStoreGroupRID; }
            set { _allocationCriteria.GradeStoreGroupRID = value; }
        }

        /// <summary>
        /// Gets or sets the UseStoreGradeDefault flag value.
        /// </summary>
        public bool UseStoreGradeDefault
        {
            get { return _allocationCriteria.UseStoreGradeDefault; }
            set { _allocationCriteria.UseStoreGradeDefault = value; }
        }

        /// <summary>
        /// Gets or sets the UsePctNeedDefault flag value.
        /// </summary>
        public bool UsePctNeedDefault
        {
            get { return _allocationCriteria.UsePctNeedDefault; }
            set { _allocationCriteria.UsePctNeedDefault = value; }
        }

        /// <summary>
        /// Gets or sets the UseFactorPctDefault flag value.
        /// </summary>
        public bool UseFactorPctDefault
        {
            get { return _allocationCriteria.UseFactorPctDefault; }
            set { _allocationCriteria.UseFactorPctDefault = value; }
        }
        /// <summary>
        /// Gets or sets the UseAllColorsMinDefault flag value.
        /// </summary>
        public bool UseAllColorsMinDefault
        {
            get { return _allocationCriteria.UseAllColorsMinDefault; }
            set { _allocationCriteria.UseAllColorsMinDefault = value; }
        }
        /// <summary>
        /// Gets or sets the UseAllColorsMaxDefault flag value.
        /// </summary>
        public bool UseAllColorsMaxDefault
        {
            get { return _allocationCriteria.UseAllColorsMaxDefault; }
            set { _allocationCriteria.UseAllColorsMaxDefault = value; }
        }
        /// <summary>
        /// Gets or sets the OverRide Child Occurs data.
        /// </summary>
        public DataSet DSOverRide
        {
            get
            {
                return _dsOverRide;
            }
            set
            {
                _dsOverRide = value;
            }
        }

        public DataSet IMODataSet
        {
            get
            {
                if (_imoDataSet == null)
                {
                    Reservation_Populate(Include.NoRID);
                }
                return _imoDataSet;
            }
            set { _imoDataSet = value; }
        }
        public bool ApplyVSW
        {
            get { return _applyVSW; }
            set { _applyVSW = value; }
        }

        public ProfileList IMOGroupLevelList
        {
            get
            {
                if (_imoGroupLevelList == null)
                {
                    _imoGroupLevelList = StoreMgmt.StoreGroup_GetLevelListViewList(SAB.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID, true); //SAB.StoreServerSession.GetStoreGroupLevelListViewList(SAB.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID, true);
                }
                return _imoGroupLevelList;
            }
            set { _imoGroupLevelList = value; }
        }

        /// <summary>
        /// Gets or sets the MerchUnspecified flag value.
        /// </summary>
        /// <remarks>
        /// True indicates that no Basis forecast level or node is set.
        /// </remarks>
        public bool MerchUnspecified
        {
            get { return _allocationCriteria.MerchUnspecified; }
            set { _allocationCriteria.MerchUnspecified = value; }
        }

        /// <summary>
        /// Gets or sets the OnHandUnspecified flag value.
        /// </summary>
        /// <remarks>
        /// True indicates that no Basis Onhand forecast level or node is set.
        /// </remarks>
        public bool OnHandUnspecified
        {
            get { return _allocationCriteria.OnHandUnspecified; }
            set { _allocationCriteria.OnHandUnspecified = value; }
        }
        /// <summary>
        /// Gets or sets the Bulk Reserve quantity (in units or as a percent
        /// </summary>
        public double ReserveAsBulk
        {
            get { return _allocationCriteria.ReserveAsBulk; }
            set { _allocationCriteria.ReserveAsBulk = value; }
        }
        /// <summary>
        /// Gets or sets the Pack Reserve quantity (in units or as a persent)
        /// </summary>
        public double ReserveAsPacks
        {
            get { return _allocationCriteria.ReserveAsPacks; }
            set { _allocationCriteria.ReserveAsPacks = value; }
        }
        /// <summary>
        /// Gets or sets the Inventory Basis Indicator ('A' is the default)
        /// </summary>
        public char InventoryInd
        {
            get { return _allocationCriteria.HdrInventoryInd; }  // TT#1140 - MD - Jellis - Group Allocation Header Inventory Min Max Options
            set { _allocationCriteria.HdrInventoryInd = value; }     // TT#1140 - MD - Jellis - Group Allocation Header Inventory Min Max Options
        }
        /// <summary>
        /// Gets or sets the Inventory Merchandise Hierarchy Node RID
        /// </summary>
        public int MERCH_HN_RID
        {
            get { return _allocationCriteria.HdrInventory_MERCH_HN_RID; }   // TT#1140 - MD - Jellis - Group Allocation Header Inventory Min Max Options 
            set { _allocationCriteria.HdrInventory_MERCH_HN_RID = value; }   // TT#1140 - MD - Jellis - Group Allocation Header Inventory Min Max Options
        }
        /// <summary>
        /// Gets or sets the Inventory Merchandise Product Hierarchy Node RID
        /// </summary>
        public int MERCH_PH_RID
        {
            get { return _allocationCriteria.HdrInventory_MERCH_PH_RID; }   // TT#1140 - MD - Jellis - Group Allocation Header Inventory Min Max Options
            set { _allocationCriteria.HdrInventory_MERCH_PH_RID = value; }   // TT#1140 - MD - Jellis - Group Allocation Header Inventory Min Max Options
        }
        /// <summary>
        /// Gets or sets the Inventory Merchandise Product Level Sequence
        /// </summary>
        public int MERCH_PHL_SEQ
        {
            get { return _allocationCriteria.HdrInventory_MERCH_PHL_SEQ; }  // TT#1140 - MD - Jellis - Group Allocation Header Inventory Min Max Options
            set { _allocationCriteria.HdrInventory_MERCH_PHL_SEQ = value; }  // TT#1140 - MD - Jellis - Group Allocation Header Inventory Min Max Options
        }
        /// <summary>
        /// Gets or sets the Inventory Merchandise Type
        /// </summary>
        public eMerchandiseType MerchandiseType
        {
            get { return _allocationCriteria.HdrInventoryMerchandiseType; }  // TT#1140 - MD - Jellis - Group Allocation Header Inventory Min Max Options
            set { _allocationCriteria.HdrInventoryMerchandiseType = value; }  // TT#1140 - MD - Jellis - Group Allocation Header Inventory Min Max Options
        }

		// Begin TT#4332 - stodd - Matrix randomly shows all zeros for a grade tier
        public DataTable StoreGrades
        {
            get
            {
                DataTable dtStoreGrades = null;
                dtStoreGrades = _dsOverRide.Tables["StoreGrades"];
                return dtStoreGrades;
            }
        }
		// End TT#4332 - stodd - Matrix randomly shows all zeros for a grade tier

        ///// <summary>
        ///// Gets or sets the number of weeks to use to calculate grades.
        ///// </summary>
        //public int GradeWeekCount
        //{
        //    get 
        //    {
        //        return _gradeWeekCount;
        //    }
        //    set
        //    {
        //        if (value < 1)
        //        {
        //            throw new MIDException (eErrorLevel.severe,
        //                (int)eMIDTextCode.msg_GradeWeekCountCannotBeLT1,
        //                MIDText.GetText(eMIDTextCode.msg_GradeWeekCountCannotBeLT1));
        //        }
        //        _gradeWeekCount = value;
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the ExceedMaximums flag value.
        ///// </summary>
        ///// <remarks>
        ///// True indicates that an allocation may exceed the "grade" maximum.
        ///// </remarks>
        //public bool ExceedMaximums
        //{
        //    get
        //    {
        //        return _exceedMaximums;
        //    }
        //    set
        //    {
        //        _exceedMaximums = value;
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the percent need limit.
        ///// </summary>
        ///// <remarks>
        ///// Controls the need algorithm. The need allocation process will stop for stores whose percent need is equal to or less than this limit.
        ///// The first time a store achieves this limit during the current process, the store will get no additional units.
        ///// </remarks>
        //public double PercentNeedLimit
        //{
        //    get
        //    {
        //        return _percentNeedLimit;
        //    }
        //    set
        //    {
        //        _percentNeedLimit = value;
        //    }
        //}

        ///// <summary>
        ///// Gets or sets ReserveIsPercent flag value.
        ///// </summary>
        //public bool ReserveIsPercent
        //{
        //    get
        //    {
        //        return _reserveQtyIsPercent;
        //    }
        //    set
        //    {
        //        _reserveQtyIsPercent = value;
        //    }
        //}

        ///// <summary>
        ///// Gets or sets ReserveQty.
        ///// </summary>
        ///// <remarks>
        ///// This quantity is a percent when ReserveIsPercet is true; otherwise, it is a unit value.
        ///// </remarks>
        //public double ReserveQty
        //{
        //    get
        //    {
        //        return _reserveQty;
        //    }
        //    set
        //    {
        //        _reserveQty = value;
        //    }
        //}

        ///// <summary>
        ///// Gets or sets OTSPlanPHL which identifies a dynamic product level within the heirarchy as a source of plans.
        ///// </summary>
        ///// <remarks>
        ///// This variable identifies the level within the hierarchy where the system should look for plans.  The plan chosen is relative to the style on the header.
        ///// </remarks>
        //public int OTSPlanPHL
        //{
        //    get
        //    {
        //        return _OTSPlan.ProductHnLvlRID;
        //    }
        //    set
        //    {
        //        _OTSPlan.SetProductHnLvlRID(value);
        //    }
        //}
        //public int OTSPlanPHLSeq
        //{
        //    get
        //    {
        //        return _OTSPlan.ProductHnLvlSeq;
        //    }
        //    set
        //    {
        //        _OTSPlan.SetProductHnLvlSeq(value);
        //    }
        //}
        ///// <summary>
        ///// Gets or sets OTSPlanRID which identifies a specific merchandise plan within the hierarchy as a source of plans.
        ///// </summary>
        //public int OTSPlanRID
        //{
        //    get
        //    {
        //        return _OTSPlan.MdseHnRID;
        //    }
        //    set
        //    {
        //        _OTSPlan.SetMdseHnRID(value);
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the OTSOnHandPHL which identifies a dynamic product level within the hierarchy as a source for store actual Onhands and Intransit. The merchandise chosen at this level will be relative to the header's style.
        ///// </summary>
        //public int OTSOnHandPHL
        //{
        //    get
        //    {
        //        return _OTSOnHand.ProductHnLvlRID;
        //    }
        //    set
        //    {
        //        _OTSOnHand.SetProductHnLvlRID(value);
        //    }
        //}
        //public int OTSOnHandPHLSeq
        //{
        //    get
        //    {
        //        return _OTSOnHand.ProductHnLvlSeq;
        //    }
        //    set
        //    {
        //        _OTSOnHand.SetProductHnLvlSeq(value);
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the OTSOnHandRID which identifies a specific merchandise node within the hierarchy as the source for store actual onhands and intransit.
        ///// </summary>
        //public int OTSOnHandRID
        //{
        //    get
        //    {
        //        return _OTSOnHand.MdseHnRID;
        //    }
        //    set
        //    {
        //        _OTSOnHand.SetMdseHnRID(value);
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the OTSPlanFactorPercent.
        ///// </summary>
        ///// <remarks>
        ///// This percent is used to extract the part of a plan that the onhand source represents.
        ///// </remarks>
        //public double OTSPlanFactorPercent
        //{
        //    get
        //    {
        //        return _OTSPlanFactorPercent;
        //    }
        //    set
        //    {
        //        _OTSPlanFactorPercent = value;
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the all color multiple.
        ///// </summary>
        //public int AllColorMultiple
        //{
        //    get
        //    {
        //        return _allColorMultiple;
        //    }
        //    set
        //    {
        //        if (value < 1)
        //        {
        //            throw new MIDException (eErrorLevel.severe,
        //                (int)eMIDTextCode.msg_MultipleCannotBeLessThan1,
        //                MIDText.GetText(eMIDTextCode.msg_MultipleCannotBeLessThan1));
        //        }
        //        _allColorMultiple = value;
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the all size multiple.
        ///// </summary>
        //public int AllSizeMultiple
        //{
        //    get
        //    {
        //        return _allSizeMultiple;
        //    }
        //    set
        //    {
        //        if (value < 1)
        //        {
        //            throw new MIDException (eErrorLevel.severe,
        //                (int)eMIDTextCode.msg_MultipleCannotBeLessThan1,
        //                MIDText.GetText(eMIDTextCode.msg_MultipleCannotBeLessThan1));
        //        }
        //        _allSizeMultiple = value;
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the all color minimum allocation constraint.
        ///// </summary>
        //public int AllColorMinimum
        //{
        //    get
        //    {
        //        return _allColor.Minimum;
        //    }
        //    set
        //    {
        //        _allColor.SetMinimum(value);
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the all color maximum allocation constraint.
        ///// </summary>
        //public int AllColorMaximum
        //{
        //    get
        //    {
        //        return _allColor.Maximum;
        //    }
        //    set
        //    {
        //        _allColor.SetMaximum(value);
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the ExceedCapacity flag value.
        ///// </summary>
        ///// <remarks>
        ///// True lets the system allocate more than a store's capacity for ALL stores; False prevents the system from allocating more than capacity.
        ///// </remarks>
        //public bool ExceedCapacity
        //{
        //    get
        //    {
        //        return _exceedCapacity;
        //    }
        //    set
        //    {
        //        _exceedCapacity = value;
        //    }
        //}
        ///// <summary>
        ///// Gets or sets the StoreGroupRID.
        ///// </summary>
        //public int StoreGroupRID
        //{
        //    get
        //    {
        //        return _storeGroupRID;
        //    }
        //    set
        //    {
        //        _storeGroupRID = value;
        //    }
        //}
        //// BEGIN TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
        //public int sGstoreGroupRID
        //{
        //    get
        //    {
        //        return _sGstoreGroupRID;
        //    }
        //    set
        //    {
        //        _sGstoreGroupRID = value;
        //    }
        //}
        //// END TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
        ///// <summary>
        ///// Gets or sets the UseStoreGradeDefault flag value.
        ///// </summary>
        //public bool UseStoreGradeDefault
        //{
        //    get
        //    {
        //        return _useStoreGradeDefault;
        //    }
        //    set
        //    {
        //        _useStoreGradeDefault = value;
        //    }
        //}
        ///// <summary>
        ///// Gets or sets the UsePctNeedDefault flag value.
        ///// </summary>
        //public bool UsePctNeedDefault
        //{
        //    get
        //    {
        //        return _usePctNeedDefault;
        //    }
        //    set
        //    {
        //        _usePctNeedDefault = value;
        //    }
        //}
        ///// <summary>
        ///// Gets or sets the UseFactorPctDefault flag value.
        ///// </summary>
        //public bool UseFactorPctDefault
        //{
        //    get
        //    {
        //        return _useFactorPctDefault;
        //    }
        //    set
        //    {
        //        _useFactorPctDefault = value;
        //    }
        //}
        ///// <summary>
        ///// Gets or sets the UseAllColorsMinDefault flag value.
        ///// </summary>
        //public bool UseAllColorsMinDefault
        //{
        //    get
        //    {
        //        return _useAllColorsMinDefault;
        //    }
        //    set
        //    {
        //        _useAllColorsMinDefault = value;
        //    }
        //}
        ///// <summary>
        ///// Gets or sets the UseAllColorsMaxDefault flag value.
        ///// </summary>
        //public bool UseAllColorsMaxDefault
        //{
        //    get
        //    {
        //        return _useAllColorsMaxDefault;
        //    }
        //    set
        //    {
        //        _useAllColorsMaxDefault = value;
        //    }
        //}
        ///// <summary>
        ///// Gets or sets the OverRide Child Occurs data.
        ///// </summary>
        //public DataSet DSOverRide 
        //{
        //    get
        //    {
        //        return _dsOverRide;
        //    }
        //    set
        //    {
        //        _dsOverRide = value;
        //    }
        //}
        
        //// BEGIN TT#1401 - GTaylor - Reservation Stores
        //public DataSet IMODataSet
        //{
        //    // Begin TT#2731 - JSmith - Unable to copy allocation override method from global
        //    //get { return _imoDataSet; }
        //    get 
        //    {
        //        if (_imoDataSet == null)
        //        {
        //            Reservation_Populate(Include.NoRID);
        //        }
        //        return _imoDataSet; 
        //    }
        //    // End TT#2731 - JSmith - Unable to copy allocation override method from global
        //    set { _imoDataSet = value; }
        //}
        //public bool ApplyVSW
        //{
        //    get { return _applyVSW; }
        //    set { _applyVSW = value; }
        //}
        //// END TT#1401 - GTaylor - Reservation Stores

        //// Begin TT#2731 - JSmith - Unable to copy allocation override method from global
        //public ProfileList IMOGroupLevelList
        //{
        //    get 
        //    {
        //        if (_imoGroupLevelList == null)
        //        {
        //            _imoGroupLevelList = SAB.StoreServerSession.GetStoreGroupLevelListViewList(SAB.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID, true);
        //        }
        //        return _imoGroupLevelList; 
        //    }
        //    set { _imoGroupLevelList = value; }
        //}
        // End TT#2731 - JSmith - Unable to copy allocation override method from global

        //// Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level
        ///// <summary>
        ///// Gets or sets the MerchUnspecified flag value.
        ///// </summary>
        ///// <remarks>
        ///// True indicates that no Basis forecast level or node is set.
        ///// </remarks>
        //public bool MerchUnspecified
        //{
        //    get
        //    {
        //        return _merchUnspecified;
        //    }
        //    set
        //    {
        //        _merchUnspecified = value;
        //    }
        //}
        ///// <summary>
        ///// Gets or sets the OnHandUnspecified flag value.
        ///// </summary>
        ///// <remarks>
        ///// True indicates that no Basis Onhand forecast level or node is set.
        ///// </remarks>
        //public bool OnHandUnspecified
        //{
        //    get
        //    {
        //        return _onHandUnspecified;
        //    }
        //    set
        //    {
        //        _onHandUnspecified = value;
        //    }
        //}
        //// End TT#709  

        //// BEGIN TT#667 - Stodd - Pre-allocate Reserve
        //public double ReserveAsBulk
        //{
        //    get { return _reserveAsBulk; }
        //    set { _reserveAsBulk = value; }
        //}

        //public double ReserveAsPacks
        //{
        //    get { return _reserveAsPacks; }
        //    set { _reserveAsPacks = value; }
        //}
        //// END TT#667 - Stodd - Pre-allocate Reserve
        //// BEGIN TT#1287 - AGallagher - Inventory Min/Max
        //public char InventoryInd
        //{
        //    get
        //    {
        //        return _InventoryInd;
        //    }
        //    set
        //    {
        //        _InventoryInd = value;
        //    }
        //}
        //public int MERCH_HN_RID
        //{
        //    get
        //    {
        //        return _MERCH_HN_RID;
        //    }

        //    set
        //    {
        //        _MERCH_HN_RID = value;
        //    }
        //}

        //public int MERCH_PH_RID
        //{
        //    get
        //    {
        //        return _MERCH_PH_RID;
        //    }

        //    set
        //    {
        //        _MERCH_PH_RID = value;
        //    }
        //}

        //public int MERCH_PHL_SEQ
        //{
        //    get
        //    {
        //        return _MERCH_PHL_SEQ;
        //    }

        //    set
        //    {
        //        _MERCH_PHL_SEQ = value;
        //    }
        //}

        //// END TT#1287 - AGallagher - Inventory Min/Max

        //// BEGIN TT#1287 - AGallagher - Inventory Min/Max
        //public eMerchandiseType MerchandiseType
        //{
        //    get { return _merchandiseType2; }
        //    set { _merchandiseType2 = value; }
        //}
        //// END TT#1287 - AGallagher - Inventory Min/Max


		//========
		// METHODS
		//========
        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        override internal bool CheckForUserData()
        {
            if (IsFilterUser(_storeFilterRID))
            {
                return true;
            }

            if (CheckAllocationCriteriaForUserData(_allocationCriteria))
            {
                return true;
            }

            return false;
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

		/// <summary>
		/// Load the tables from the data source tables.
		/// </summary>
		//private void LoadHashTables() // TT#488 - MD - Jellis - Group Allocation
        private void LoadTables()       // TT#488 - MD - Jellis - Group Allocaiton
		{
            //_colorMinMax = new Dictionary<int, ColorOrSizeMinMaxBin>();             // TT#488 - MD - Jellis - Group Allocation
            //_colorSizeMinMax = new Dictionary<int, ColorSizeMinMaxBin>();           // TT#488 - MD - Jellis - Group Allocation
            //_grade = new Dictionary<int, Dictionary<double, AllocationGradeBin>>(); // TT#488 - MD - Jellis - Group Allocation
            //_sglCapacity = new Dictionary<int, AllocationCapacityBin>();            // TT#488 - MD - Jellis - Group Allocation

			if (_dsOverRide.Tables["Colors"].Rows.Count > 0)
			{
				LoadColorMinMax();
			}
			if (_dsOverRide.Tables["StoreGrades"].Rows.Count > 0)
			{
				LoadStoreGrades();
			}
			if (_dsOverRide.Tables["Capacity"].Rows.Count > 0)
			{
				LoadCapacity();
			}
        }
		/// <summary>
		/// Load Color Minimums and maximums.
		/// </summary>
		private void LoadColorMinMax()
		{
			try
			{
				int colorCodeRID, colorMin, colorMax;
				DataTable dtColor = _dsOverRide.Tables["Colors"];
				foreach (DataRow dr in dtColor.Rows)
				{
					colorCodeRID = Convert.ToInt32(dr["Color"],CultureInfo.CurrentUICulture);
                    //AddColorMinMaxToMethod (colorCodeRID);         // TT#488 - MD - Jellis - Group Allocation
                    _allocationCriteria.AddColorMinMax(colorCodeRID); // TT#488 - MD - Jellis - Group Allocation
					if (dr["Minimum"] != System.DBNull.Value)
					{
						colorMin = Convert.ToInt32(dr["Minimum"],CultureInfo.CurrentUICulture);
						SetColorMin(colorCodeRID, colorMin);
					}
					 
					if (dr["Maximum"] != System.DBNull.Value)
					{
						colorMax = Convert.ToInt32(dr["Maximum"],CultureInfo.CurrentUICulture);
						SetColorMax(colorCodeRID, colorMax);
					}
				}
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}
		/// <summary>
		/// Load Store Grade data.
		/// </summary>
		private void LoadStoreGrades()
		{
			string gradeCode;
			int boundary, minStock, maxStock, minAd, minColor, maxColor;
            int sglRID;     //  TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
            int shipUpTo;   // TT#617 - RMatelic - Allocation Override - Add Ship Up To Rule (#36, #39)
			try
			{
				DataTable dtStoreGrades = _dsOverRide.Tables["StoreGrades"];
				foreach (DataRow dr in dtStoreGrades.Rows)
				{
					boundary = Convert.ToInt32(dr["Boundary"],CultureInfo.CurrentUICulture);
					gradeCode =  Convert.ToString(dr["Grade"],CultureInfo.CurrentUICulture);
                    sglRID = Convert.ToInt32(dr["SGLRID"], CultureInfo.CurrentUICulture);
					AddGrade(sglRID, boundary, gradeCode); 
					if (dr["Allocation Min"] != System.DBNull.Value)
					{
						minStock = Convert.ToInt32(dr["Allocation Min"],CultureInfo.CurrentUICulture);
						SetGradeMinimum (sglRID, boundary, minStock);
					}
					
					if (dr["Allocation Max"] != System.DBNull.Value)
					{
						maxStock = Convert.ToInt32(dr["Allocation Max"],CultureInfo.CurrentUICulture);
                        SetGradeMaximum(sglRID, boundary, maxStock);
					}
					 
					if (dr["Min Ad"] != System.DBNull.Value)
					{
						minAd = Convert.ToInt32(dr["Min Ad"],CultureInfo.CurrentUICulture);
                        SetGradeAdMinimum(sglRID, boundary, minAd);
					}
					 
					if (dr["Color Min"] != System.DBNull.Value)
					{
						minColor = Convert.ToInt32(dr["Color Min"],CultureInfo.CurrentUICulture);
                        SetGradeColorMinimum(sglRID, boundary, minColor);
					}
					 
					if (dr["Color Max"] != System.DBNull.Value)
					{
						maxColor = Convert.ToInt32(dr["Color Max"],CultureInfo.CurrentUICulture);
                        SetGradeColorMaximum(sglRID, boundary, maxColor);
					}
            		// Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
                    if (dr["SGLRID"] != System.DBNull.Value)
                    {
                        sglRID = Convert.ToInt32(dr["SGLRID"], CultureInfo.CurrentUICulture);
                        SetGradeAttributeSet(sglRID, boundary);
                    }
                    // End TT#618
                    // Begin TT#617 - RMatelic - Allocation Override - Add Ship Up To Rule (#36, #39)
                    if (dr["Ship Up To"] != System.DBNull.Value)
                    {
                        shipUpTo = Convert.ToInt32(dr["Ship Up To"], CultureInfo.CurrentUICulture);
                        SetGradeShipUpTo(sglRID, boundary, shipUpTo);
                    }
                    // End TT#617
				}
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}
		/// <summary>
		/// Load Capacity data.
		/// </summary>
		private void LoadCapacity()
		{
			int sglRID;
			char exceedCapacity;
			bool b_exceedCapacity;
			double exceedByPct;
			try
			{
				DataTable dtCapacity = _dsOverRide.Tables["Capacity"];
				foreach (DataRow dr in dtCapacity.Rows)
				{
					sglRID = Convert.ToInt32(dr["SglRID"],CultureInfo.CurrentUICulture);
					
                    //AddStrGroupLvlToMethod (sglRID);   // TT#488 - MD - Jellis - Group Allocation
                    _allocationCriteria.AddCapacityStrGroupLvl(sglRID); // TT#488 - MD - Jellis - Group Allocation
					if (dr["ExceedChar"] != System.DBNull.Value)
					{
						exceedCapacity = Convert.ToChar(dr["ExceedChar"],CultureInfo.CurrentUICulture);
						b_exceedCapacity = Include.ConvertCharToBool(exceedCapacity);
						SetStrGroupLvlExceedCapacity (sglRID, b_exceedCapacity);
					}
				
					if (dr["Exceed by %"] != System.DBNull.Value)
					{
						exceedByPct = Convert.ToDouble(dr["Exceed by %"],CultureInfo.CurrentUICulture);
						SetStrGroupLvlExceedCapacityByPct (sglRID, exceedByPct);
					}
				}
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

        // begin TT#488 - MD - Jellis - Group Allocation
        ///// <summary>
        ///// Determines if there is a min/max for the color.
        ///// </summary>
        ///// <param name="aColorRID">Color RID</param>
        ///// <returns>True when there is a min/max for the color; false if there is no specific min/max for the color</returns>
        //public bool ColorMinMaxIsInMethod(int aColorRID)
        //{
        //    if (_colorMinMax == null)
        //    {
        //        return false;
        //    }
        //    return _colorMinMax.Contains(aColorRID);  
        //}

        ///// <summary>
        ///// Removes color from method's color min/max table.
        ///// </summary>
        ///// <param name="aColorRID"></param>
        //public void RemoveColorMinMaxFromMethod (int aColorRID)
        //{
        //    if (ColorMinMaxIsInMethod(aColorRID))
        //    {
        //        _colorMinMax.Remove(aColorRID);
        //    }
        //}
        // end TT#488 - MD - Jellis - Group Allocation

        ///// <summary>
        ///// Adds color min/maxto allocation override method.
        ///// </summary>
        ///// <param name="aColorRID">Database RID for the color.</param>
        ////public void AddColorMinMaxToMethod (int aColorRID)               // TT#488 - MD - Jellis - Group Allocation
        //public ColorOrSizeMinMaxBin AddColorMinMaxToMethod (int aColorRID) // TT#488 - MD - Jellis - Group Allocation
        //{
        //    // begin TT#488 - MD - Jellis - Group Allocation
        //    //ColorOrSizeMinMaxBin _aColorBin;
        //    //if (_colorMinMax == null)
        //    //{
        //    //    _colorMinMax = Hashtable.Synchronized(new Hashtable()); 
        //    //}
        //    //if(!ColorMinMaxIsInMethod(aColorRID))
        //    //{
        //    //    _aColorBin = new ColorOrSizeMinMaxBin(aColorRID);
        //    //    _aColorBin.SetMaximum(AllColorMaximum);
        //    //    _aColorBin.SetMinimum(AllColorMinimum);
        //    //    _colorMinMax.Add(aColorRID, _aColorBin);
        //    //}
        //    //else
        //    //{
        //    //    throw new MIDException (eErrorLevel.warning,
        //    //        (int)eMIDTextCode.msg_DuplicateColorNotAllowed,
        //    //        MIDText.GetText(eMIDTextCode.msg_DuplicateColorNotAllowed));
        //    //}
        //    ColorOrSizeMinMaxBin colorBin;
        //    if (_colorMinMax.TryGetValue(aColorRID, out colorBin))
        //    {
        //        colorBin = new ColorOrSizeMinMaxBin(aColorRID);
        //        colorBin.SetMaximum(AllColorMaximum);
        //        colorBin.SetMinimum(AllColorMinimum);
        //        _colorMinMax.Add(aColorRID, colorBin);
        //        return colorBin;
        //    }
        //    throw new MIDException (eErrorLevel.warning,
        //        (int)eMIDTextCode.msg_DuplicateColorNotAllowed,
        //        MIDText.GetText(eMIDTextCode.msg_DuplicateColorNotAllowed));
        //    // end TT#488 - MD - JEllis - Group Allocation
        //}
		public int GetColorMax (int aColorRID)
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            //if (!ColorMinMaxIsInMethod(aColorRID))
            //{
            //    throw new MIDException (eErrorLevel.warning,
            //        (int)eMIDTextCode.msg_ColorNotDefinedInMethod,
            //        MIDText.GetText(eMIDTextCode.msg_ColorNotDefinedInMethod));
            //}
            //ColorOrSizeMinMaxBin _aColorBin;
            //_aColorBin = (ColorOrSizeMinMaxBin)_colorMinMax[aColorRID];
            //return (int) _aColorBin.Maximum;
            return _allocationCriteria.GetColorMax(aColorRID);
            // end TT#488 - MD - Jellis - Group Allocation
		}
		public void SetColorMax (int aColorRID, int aMaximum)
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            //if (!ColorMinMaxIsInMethod(aColorRID))
            //{
            //    AddColorMinMaxToMethod(aColorRID);
            //}
            //ColorOrSizeMinMaxBin _aColorBin;
            //_aColorBin = (ColorOrSizeMinMaxBin)_colorMinMax[aColorRID];
            //_aColorBin.SetMaximum(aMaximum);
            _allocationCriteria.SetColorMax(aColorRID, aMaximum);
            // end TT#488 - MD - Jellis - Group Allocation
		}
		public int GetColorMin (int aColorRID)
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            //if (!ColorMinMaxIsInMethod(aColorRID))
            //{
            //    throw new MIDException (eErrorLevel.warning,
            //        (int)eMIDTextCode.msg_ColorNotDefinedInMethod,
            //        MIDText.GetText(eMIDTextCode.msg_ColorNotDefinedInMethod));
            //}
            //ColorOrSizeMinMaxBin _aColorBin;
            //_aColorBin = (ColorOrSizeMinMaxBin)_colorMinMax[aColorRID];
            //return (int) _aColorBin.Minimum;
            return _allocationCriteria.GetColorMin(aColorRID);
            // end TT#488 - MD - Jellis - Group Allocation
		}
		public void SetColorMin (int aColorRID, int aMinimum)
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            //if (!ColorMinMaxIsInMethod(aColorRID))
            //{
            //    AddColorMinMaxToMethod(aColorRID);
            //}
            //ColorOrSizeMinMaxBin _aColorBin;
            //_aColorBin = (ColorOrSizeMinMaxBin)_colorMinMax[aColorRID];
            //_aColorBin.SetMinimum(aMinimum);
            _allocationCriteria.SetColorMin(aColorRID, aMinimum);
            // end TT#488 - MD - Jellis - Group Allocation
		}
        //public bool DoesColorHaveSizesInMethod(int aColorRID)
        //{
        //    // begin TT#488 - MD - Jellis - Group Allocaton
        //    //if (_colorSizeMinMax == null)
        //    //{
        //    //    return false;
        //    //}
        //    //return _colorSizeMinMax.Contains(aColorRID); 
        //    return _colorSizeMinMax.ContainsKey(aColorRID);
        //    // end TT#488 - MD - Jellis - Group Allocation
        //}

        //public bool IsColorSizeMinMaxInMethod (int aColorRID, int aSizeRID)
        //{
        //    if (!(DoesColorHaveSizesInMethod(aColorRID)))
        //    {
        //        return false;
        //    }
        //    ColorSizeMinMaxBin aMinMaxBin = (ColorSizeMinMaxBin)_colorSizeMinMax[aColorRID];
        //    return aMinMaxBin.IsSizeMinMaxInMethodColor(aSizeRID);
        //}
		public int GetColorSizeMin(int aColorRID, int aSizeRID)
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            //if (!DoesColorHaveSizesInMethod(aColorRID))
            //{
            //    return 0;
            //}
            //ColorSizeMinMaxBin aMinMaxBin = (ColorSizeMinMaxBin)_colorSizeMinMax[aColorRID];
            //return aMinMaxBin.GetSizeMinimum(aSizeRID);
            return _allocationCriteria.GetColorSizeMin(aColorRID, aSizeRID);
            // end TT#488  - MD - Jellis - Group Allocation
		}
		public void SetColorSizeMin(int aColorRID, int aSizeRID, int aMinimum)
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            //ColorSizeMinMaxBin aMinMaxBin;
            //if (_colorSizeMinMax == null)
            //{
            //    _colorSizeMinMax = new Hashtable();
            //}
            //if (!(DoesColorHaveSizesInMethod(aColorRID)))
            //{
            //    aMinMaxBin = new ColorSizeMinMaxBin(aColorRID);
            //    _colorSizeMinMax.Add(aColorRID, aMinMaxBin);
            //}
            //else
            //{
            //    aMinMaxBin = (ColorSizeMinMaxBin) _colorSizeMinMax[aColorRID];
            //}
            //aMinMaxBin.SetSizeMinimum(aSizeRID, aMinimum);
            _allocationCriteria.SetColorSizeMin(aColorRID, aSizeRID, aMinimum);
            // end TT#488 - MD - Jellis - Group Allocation
		}

		public int GetColorSizeMax(int aColorRID, int aSizeRID)
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            //if (!DoesColorHaveSizesInMethod(aColorRID))
            //{
            //    return 0;
            //}
            //ColorSizeMinMaxBin aMinMaxBin = (ColorSizeMinMaxBin)_colorSizeMinMax[aColorRID];
            //return aMinMaxBin.GetSizeMaximum(aSizeRID);
            return _allocationCriteria.GetColorSizeMax(aColorRID, aSizeRID);
            // end TT#488 - MD - Jellis - Group Allocation
		}
		public void SetColorSizeMax(int aColorRID, int aSizeRID, int aMaximum)
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            //ColorSizeMinMaxBin aMinMaxBin;
            //if (_colorSizeMinMax == null)
            //{
            //    _colorSizeMinMax = new Hashtable();
            //}
            //if (!(DoesColorHaveSizesInMethod(aColorRID)))
            //{
            //    aMinMaxBin = new ColorSizeMinMaxBin(aColorRID);
            //    _colorSizeMinMax.Add(aColorRID, aMinMaxBin);
            //}
            //else
            //{
            //    aMinMaxBin = (ColorSizeMinMaxBin) _colorSizeMinMax[aColorRID];
            //}
            //aMinMaxBin.SetSizeMaximum(aSizeRID, aMaximum);
            _allocationCriteria.SetColorSizeMax(aColorRID, aSizeRID, aMaximum);
            // end TT#488 - MD - Jellis - Group Allocation
		}

        //public bool StrGroupLvlIsInMethod(int aStrGroupLvlRID)
        //{
        //    // begin TT#488 - MD - Jellis - Group Allocation
        //    //if (_sglCapacity == null)
        //    //{
        //    //    return false;
        //    //}
        //    //return _sglCapacity.Contains(aStrGroupLvlRID);
        //    return _sglCapacity.ContainsKey(aStrGroupLvlRID);
        //    // end TT#488 - MD - Jellis - Group Allocation
        //}
		public void RemoveStrGroupLvlFromMethod (int aStrGroupLvlRID)
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            //if (StrGroupLvlIsInMethod(aStrGroupLvlRID))
            //{
            //    _sglCapacity.Remove(aStrGroupLvlRID);
            //}
            _allocationCriteria.RemoveStrGroupLvlFromMethod(aStrGroupLvlRID);
            // end TT#488 - MD - Jellis - Group Allocation
		}
        ///// <summary>
        ///// Adds StrGroupLvl to allocation override method.
        ///// </summary>
        ///// <param name="aStrGroupLvlRID">Database RID for the StrGroupLvl.</param>
        ////public void AddStrGroupLvlToMethod (int aStrGroupLvlRID) // TT#488 - MD - Jellis - Group Allocation
        //public AllocationCapacityBin AddStrGroupLvlToMethod(int aStrGroupLvlRID)
        //{
        //    // begin TT#488 - MD - Jellis - Group Allocation
        //    //AllocationCapacityBin _aStrGroupLvlBin;
        //    //if (_sglCapacity == null)
        //    //{
        //    //    _sglCapacity = Hashtable.Synchronized(new Hashtable());
        //    //}
        //    //if(!StrGroupLvlIsInMethod(aStrGroupLvlRID))
        //    //{
        //    //    _aStrGroupLvlBin = new AllocationCapacityBin();
        //    //    _aStrGroupLvlBin.SetStoreGroupLevelRID(aStrGroupLvlRID);
        //    //    _aStrGroupLvlBin.SetExceedCapacity(false);
        //    //    _aStrGroupLvlBin.SetExceedCapacityByPercent(0.0d);
        //    //    _sglCapacity.Add(aStrGroupLvlRID, _aStrGroupLvlBin);

        //    //}
        //    //else
        //    //{
        //    //    throw new MIDException (eErrorLevel.warning,
        //    //        (int)eMIDTextCode.msg_DuplicateStrGroupLvlNotAllowed,
        //    //        MIDText.GetText(eMIDTextCode.msg_DuplicateStrGroupLvlNotAllowed));
        //    //}
        //    AllocationCapacityBin strGroupLvlBin;
        //    if (_sglCapacity.TryGetValue(aStrGroupLvlRID, out strGroupLvlBin))
        //    {
        //        throw new MIDException (eErrorLevel.warning,
        //            (int)eMIDTextCode.msg_DuplicateStrGroupLvlNotAllowed,
        //            MIDText.GetText(eMIDTextCode.msg_DuplicateStrGroupLvlNotAllowed));
        //    }
        //    strGroupLvlBin.SetStoreGroupLevelRID(aStrGroupLvlRID);
        //    strGroupLvlBin.SetExceedCapacity(false);
        //    strGroupLvlBin.SetExceedCapacityByPercent(0.0d);
        //    _sglCapacity.Add(aStrGroupLvlRID, strGroupLvlBin);
        //    return strGroupLvlBin;
        //    // end TT#488 - MD - Jellis - Group Allocation
        //}
		public bool GetStrGroupLvlExceedCapacity (int aStrGroupLvlRID)
		{
            // begin TT#488 - MD - JEllis - Group Allocation
            //if (!StrGroupLvlIsInMethod(aStrGroupLvlRID))
            //{
            //    throw new MIDException (eErrorLevel.warning,
            //        (int)eMIDTextCode.msg_StrGroupLvlNotDefinedInMethod,
            //        MIDText.GetText(eMIDTextCode.msg_StrGroupLvlNotDefinedInMethod));
            //}
            //AllocationCapacityBin _aStrGroupLvlBin;
            //_aStrGroupLvlBin = (AllocationCapacityBin)_sglCapacity[aStrGroupLvlRID];
            //return _aStrGroupLvlBin.ExceedCapacity;
            return _allocationCriteria.GetStrGroupLvlExceedCapacity(aStrGroupLvlRID);
            // end TT#488 - MD - Jellis - Group Allocation
		}

		/// <summary>
		/// Sets store group level exceed capacity flag value
		/// </summary>
		/// <param name="aStrGroupLvlRID">Store Group Level RID</param>
		/// <param name="aExceedCapacity">Exceed Capacity Flag Value: true or false.</param>
		public void SetStrGroupLvlExceedCapacity (int aStrGroupLvlRID, bool aExceedCapacity)
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            //if (!StrGroupLvlIsInMethod(aStrGroupLvlRID))
            //{
            //    AddStrGroupLvlToMethod(aStrGroupLvlRID);
            //}
            //AllocationCapacityBin _aStrGroupLvlBin;
            //_aStrGroupLvlBin = (AllocationCapacityBin)_sglCapacity[aStrGroupLvlRID];
            //_aStrGroupLvlBin.SetExceedCapacity(aExceedCapacity);
            _allocationCriteria.SetStrGroupLvlExceedCapacity(aStrGroupLvlRID, aExceedCapacity);
            // end TT#488 - MD - Jellis - Group Allocation
		}
		public double GetStrGroupLvlExceedCapacityByPct (int aStrGroupLvlRID)
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            //if (!StrGroupLvlIsInMethod(aStrGroupLvlRID))
            //{
            //    throw new MIDException (eErrorLevel.warning,
            //        (int)eMIDTextCode.msg_StrGroupLvlNotDefinedInMethod,
            //        MIDText.GetText(eMIDTextCode.msg_StrGroupLvlNotDefinedInMethod));
            //}
            //AllocationCapacityBin _aStrGroupLvlBin;
            //_aStrGroupLvlBin = (AllocationCapacityBin)_sglCapacity[aStrGroupLvlRID];
            //return  _aStrGroupLvlBin.ExceedCapacityByPercent;
            return _allocationCriteria.GetStrGroupLvlExceedCapacityByPct(aStrGroupLvlRID);
            // end TT#488 - MD - Jellis - Group Allocation
		}

		/// <summary>
		/// Sets store group level exceed capacity by percent value.
		/// </summary>
		/// <param name="aStrGroupLvlRID">Store Group Level RID</param>
		/// <param name="aPercent">Percent by which the allocation may exceed capacity.</param>
		public void SetStrGroupLvlExceedCapacityByPct (int aStrGroupLvlRID, double aPercent)
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            //// RonM commented out edit for MIDTrack 1345
            ////if (aPercent < 0)
            ////{
            ////	throw new MIDException (eErrorLevel.severe,
            ////		(int)eMIDTextCode.msg_CapacityPctCannotBeNeg,
            ////		MIDText.GetText(eMIDTextCode.msg_CapacityPctCannotBeNeg));
            ////}
            //if (!StrGroupLvlIsInMethod(aStrGroupLvlRID))
            //{
            //    AddStrGroupLvlToMethod(aStrGroupLvlRID);
            //}
            //AllocationCapacityBin _aStrGroupLvlBin;
            //_aStrGroupLvlBin = (AllocationCapacityBin)_sglCapacity[aStrGroupLvlRID];
            //_aStrGroupLvlBin.SetExceedCapacityByPercent(aPercent);
            _allocationCriteria.SetStrGroupLvlExceedCapacityByPct(aStrGroupLvlRID, aPercent);
            // end TT#488 - MD - Jellis - Group Allocation
		}
        
        // begin TT#488 - MD - Jellis - Group Allocation
        // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
		//public bool GradeIsInMethod(string aGradeName)
        //public bool GradeIsInMethod(int aSglRID, double aBoundary)
        //{
        //    if (_grade == null)
        //    {
        //        return false;
        //    }
        //    //return _grade.Contains(aGradeName);
          
        //    Hashtable boundaryHT = new Hashtable();
        //    //AllocationGradeBin gradeBin; // Not Used // TT#1185 - Verify ENQ before Update (unrelated)
        //    if (_grade.ContainsKey(aSglRID))
        //    {
        //        boundaryHT = (Hashtable)_grade[aSglRID];
        //        return boundaryHT.ContainsKey(aBoundary);
        //    }
        //    else
        //    {
        //        //gradeBin = new AllocationGradeBin();
        //        //boundaryHT.Add(aBoundary, gradeBin);
        //        //_grade.Add(aSglRID, boundaryHT);
        //        return false;
        //    }
        //}
        // end TT#488 - MD - Jellis - Group Allocation

        //  BEGIN TT#1401 - GTaylor - Reservation Stores
        public bool GetApplyVSW()
        {
            return _mao.GetApplyVSW();
        }

        public IMOMethodOverrideProfileList GetMethodOverrideIMO(int methodRID)
        {
            return _mao.GetMethodOverrideIMO(methodRID);
        }
        
        public IMOMethodOverrideProfileList GetMethodOverrideIMO()
        {
            return _mao.GetMethodOverrideIMO(Include.NoRID);
        }
        //  END TT#1401 - GTaylor - Reservation Stores

        //public void RemoveGradeFromMethod (string aGradeName)
        //{
        //    if (GradeIsInMethod(aGradeName))
        //    {
        //        _grade.Remove(aGradeName);
        //    }
        //}
        // End TT#618

        // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
        // Restructured methods to accommodate atttibute set (SglRID); tagging each change would result in 
        // hard to read code.   Use diff to see previous code versions
		public void AddGrade(int aSglRID, double aLowBoundary ,string aGradeName)
		{
            _allocationCriteria.AddGrade(aSglRID, aLowBoundary, aGradeName);
            //AllocationGradeBin gradeBin;
            //// begin TT#488 - MD - Jellis - Group Allocation
            ////if (_grade == null)
            ////{
            ////    //_grade = Hashtable.Synchronized(new Hashtable());  // TT#488  - MD - Jellis Group Allocation
            ////    _grade = new Dictionary<int, Dictionary<double, AllocationGradeBin>>();  // TT#488 - MD - JEllis - Group Allocation
            ////}
            ////if(!GradeIsInMethod(aSglRID, aLowBoundary))
            //Dictionary<double, AllocationGradeBin> boundaryDICT;
            //if (!_grade.TryGetValue(aSglRID, out boundaryDICT))
            //{
            //    boundaryDICT = new Dictionary<double, AllocationGradeBin>();
            //    _grade.Add(aSglRID, boundaryDICT);
            //}
            //if (!boundaryDICT.TryGetValue(aLowBoundary, out gradeBin))
            //    // end TT#488 - MD - JEllis - Group Allocation
            //{
            //    gradeBin = new AllocationGradeBin();
            //    gradeBin.SetLowBoundary(aLowBoundary);
            //    gradeBin.SetGrade(aGradeName);
            //    gradeBin.SetGradeAdMinimum(0);
            //    gradeBin.SetGradeColorMaximum(gradeBin.GradeColorLargestMaximum);
            //    gradeBin.SetGradeColorMinimum(0);
            //    gradeBin.SetGradeMaximum(gradeBin.GradeLargestMaximum);
            //    gradeBin.SetGradeMinimum(0);
            //    gradeBin.SetGradeAttributeSet(aSglRID);
            //    gradeBin.SetGradeShipUpTo(0);      // TT#617 - RMatelic - Allocation Override - Add Ship Up To Rule (#36, #39)
            //    // begin TT#488 - MD - Jellis - Group Allocation
            //    //Hashtable boundaryHT;
            //    //if (_grade.ContainsKey(aSglRID))
            //    //{
            //    //    boundaryHT = (Hashtable)_grade[aSglRID];
            //    //    boundaryHT.Add(aLowBoundary, gradeBin);
            //    //}
            //    //else
            //    //{
            //    //    boundaryHT = new Hashtable();
            //    //    boundaryHT.Add(aLowBoundary, gradeBin);
            //    //    _grade.Add(aSglRID, boundaryHT);
            //    //}
            //    boundaryDICT.Add(aLowBoundary, gradeBin);
            //    // end TT#488 - MD - Jellis - Group Allocation
            //}
            //else
            //{
            //    // BEGIN TT#618 - STOdd - Allocation Override - Add Attribute Sets (#35)

            //    // *****   TEMPORARY  ***** //

            //    // Until the TT#618 processing is begun, I've commented this out.
            //    // The method that calls this method is sending every row from the 
            //    // store grades table. This table now contains the grades for each attribute set.
            //    // This causes this function to throw the exception.

            //    //throw new MIDException (eErrorLevel.warning,
            //    //    (int)eMIDTextCode.msg_DuplicateGradeNameNotAllowed,
            //    //    MIDText.GetText(eMIDTextCode.msg_DuplicateGradeNameNotAllowed));

            //    // END TT#618 - STOdd - Allocation Override - Add Attribute Sets (#35)
            //}
		}

        // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35) - following methods not used
        //public double GetGradeLowBoundary (string aGradeName)
        //{
        //    if (!GradeIsInMethod(aGradeName))
        //    {
        //        throw new MIDException (eErrorLevel.warning,
        //            (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
        //            MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
        //    }
        //    AllocationGradeBin _aGradeBin;
        //    _aGradeBin = (AllocationGradeBin)_grade[aGradeName];
        //    return  _aGradeBin.LowBoundary;
        //}
        //public void SetGradeLowBoundary (string aGradeName, double aLowBoundary)
        //{
        //    if (!GradeIsInMethod(aGradeName))
        //    {
        //        throw new MIDException (eErrorLevel.warning,
        //            (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
        //            MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
        //    }
        //    AllocationGradeBin _aGradeBin;
        //    _aGradeBin = (AllocationGradeBin)_grade[aGradeName];
        //    _aGradeBin.SetLowBoundary(aLowBoundary);
        //}
        //public int GetGradeMinimum (string aGradeName)
        //{
        //    if (!GradeIsInMethod(aGradeName))
        //    {
        //        throw new MIDException (eErrorLevel.warning,
        //            (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
        //            MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
        //    }
        //    AllocationGradeBin _aGradeBin;
        //    _aGradeBin = (AllocationGradeBin)_grade[aGradeName];
        //    return  _aGradeBin.GradeMinimum;
        //} 
        // End TT#618 
		public void SetGradeMinimum (int aSglRID, double aLowBoundary, int aMinimum)
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            //if (!GradeIsInMethod(aSglRID, aLowBoundary))
            //{
            //    throw new MIDException (eErrorLevel.warning,
            //        (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
            //        MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
            //}
            //Hashtable boundaryHT = (Hashtable)_grade[aSglRID];
            //AllocationGradeBin gradeBin;
            //gradeBin = (AllocationGradeBin)boundaryHT[aLowBoundary];
            //gradeBin.SetGradeMinimum(aMinimum);
            _allocationCriteria.SetHeaderGradeMinimum(aSglRID, aLowBoundary, aMinimum);
            // end TT#488 - MD - Jellis - Group Allocation
		}
        // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35) - following method not used
        //public int GetGradeAdMinimum (string aGradeName)
        //{
        //    if (!GradeIsInMethod(aGradeName))
        //    {
        //        throw new MIDException (eErrorLevel.warning,
        //            (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
        //            MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
        //    }
        //    AllocationGradeBin _aGradeBin;
        //    _aGradeBin = (AllocationGradeBin)_grade[aGradeName];
        //    return  _aGradeBin.GradeAdMinimum;
        //}
        // End TT#618

        public void SetGradeAdMinimum(int aSglRID, double aLowBoundary, int aAdMinimum)
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            //if (!GradeIsInMethod(aSglRID, aLowBoundary))
            //{
            //    throw new MIDException (eErrorLevel.warning,
            //        (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
            //        MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
            //}
            //Hashtable boundaryHT = (Hashtable)_grade[aSglRID];
            //AllocationGradeBin gradeBin;
            //gradeBin = (AllocationGradeBin)boundaryHT[aLowBoundary];
            //gradeBin.SetGradeAdMinimum(aAdMinimum);
            _allocationCriteria.SetHeaderGradeAdMinimum(aSglRID, aLowBoundary, aAdMinimum);
            // end TT#488 - MD - Jellis - Group Allocation
		}
        // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35) - following method not used
        //public int GetGradeMaximum (string aGradeName)
        //{
        //    if (!GradeIsInMethod(aGradeName))
        //    {
        //        throw new MIDException (eErrorLevel.warning,
        //            (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
        //            MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
        //    }
        //    AllocationGradeBin _aGradeBin;
        //    _aGradeBin = (AllocationGradeBin)_grade[aGradeName];
        //    return  _aGradeBin.GradeMaximum;
        //}
        // End TT#618  

        public void SetGradeMaximum(int aSglRID, double aLowBoundary, int aMaximum)
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            //if (!GradeIsInMethod(aSglRID, aLowBoundary))
            //{
            //    throw new MIDException (eErrorLevel.warning,
            //        (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
            //        MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
            //}
            //Hashtable boundaryHT = (Hashtable)_grade[aSglRID];
            //AllocationGradeBin gradeBin;
            //gradeBin = (AllocationGradeBin)boundaryHT[aLowBoundary];
            //gradeBin.SetGradeMaximum(aMaximum);
            _allocationCriteria.SetHeaderGradeMaximum(aSglRID, aLowBoundary, aMaximum);
            // end TT#488 - MD - Jellis - Group Allocation
		}

        // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35) - following method not used
        //public int GetGradeColorMinimum (string aGradeName)
        //{
        //    if (!GradeIsInMethod(aGradeName))
        //    {
        //        throw new MIDException (eErrorLevel.warning,
        //            (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
        //            MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
        //    }
        //    AllocationGradeBin _aGradeBin;
        //    _aGradeBin = (AllocationGradeBin)_grade[aGradeName];
        //    return  _aGradeBin.GradeColorMinimum;
        //}
        // End TT#618

        public void SetGradeColorMinimum(int aSglRID, double aLowBoundary, int aColorMinimum)
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            //if (!GradeIsInMethod(aSglRID, aLowBoundary))
            //{
            //    throw new MIDException (eErrorLevel.warning,
            //        (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
            //        MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
            //}
            //Hashtable boundaryHT = (Hashtable)_grade[aSglRID];
            //AllocationGradeBin gradeBin;
            //gradeBin = (AllocationGradeBin)boundaryHT[aLowBoundary];
            //gradeBin.SetGradeColorMinimum(aColorMinimum);
            _allocationCriteria.SetColorGradeMinimum(aSglRID, aLowBoundary, aColorMinimum);
            // end TT#488 - MD - Jellis - Group Allocation
		}

        // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35) - following methods not used
        //public int GetGradeColorMaximum (string aGradeName)
        //{
        //    if (!GradeIsInMethod(aGradeName))
        //    {
        //        throw new MIDException (eErrorLevel.warning,
        //            (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
        //            MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
        //    }
        //    AllocationGradeBin _aGradeBin;
        //    _aGradeBin = (AllocationGradeBin)_grade[aGradeName];
        //    return  _aGradeBin.GradeColorMaximum;
        //}
        // End TT#618

        public void SetGradeColorMaximum(int aSglRID, double aLowBoundary, int aColorMaximum)
		{
            // begin TT#488 - MD - Jellis - Group Allocation
            //if (!GradeIsInMethod(aSglRID, aLowBoundary))
            //{
            //    throw new MIDException (eErrorLevel.warning,
            //        (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
            //        MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
            //}
            //Hashtable boundaryHT = (Hashtable)_grade[aSglRID];
            //AllocationGradeBin gradeBin;
            //gradeBin = (AllocationGradeBin)boundaryHT[aLowBoundary];
            //gradeBin.SetGradeColorMaximum(aColorMaximum);
            _allocationCriteria.SetColorGradeMaximum(aSglRID, aLowBoundary, aColorMaximum);
            // end TT#488 - MD - Jellis - Group Allocation
		}
        public void SetGradeAttributeSet(int aSglRID, double aLowBoundary)
        {
            // begin TT#488 - MD - Jellis - Group Allocation
            //if (!GradeIsInMethod(aSglRID, aLowBoundary))
            //{
            //    throw new MIDException(eErrorLevel.warning,
            //        (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
            //        MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
            //}
            //Hashtable boundaryHT = (Hashtable)_grade[aSglRID];
            //AllocationGradeBin gradeBin;
            //gradeBin = (AllocationGradeBin)boundaryHT[aLowBoundary];
            //gradeBin.SetGradeAttributeSet(aSglRID);
            _allocationCriteria.SetGradeAttributeSet(aSglRID, aLowBoundary);
            // end TT#488 - MD - Jellis - Group Allocation
        }
        // End TT#618  

        // Begin TT#617 - RMatelic - Allocation Override - Add Ship Up To Rule (#36, #39)
        public void SetGradeShipUpTo(int aSglRID, double aLowBoundary, int aShipUpTo)
        {
            // begin TT#488 - MD - Jellis - Group Allocation
            //if (!GradeIsInMethod(aSglRID, aLowBoundary))
            //{
            //    throw new MIDException(eErrorLevel.warning,
            //        (int)eMIDTextCode.msg_GradeNotDefinedInMethod,
            //        MIDText.GetText(eMIDTextCode.msg_GradeNotDefinedInMethod));
            //}
            //Hashtable boundaryHT = (Hashtable)_grade[aSglRID];
            //AllocationGradeBin gradeBin;
            //gradeBin = (AllocationGradeBin)boundaryHT[aLowBoundary];
            //gradeBin.SetGradeShipUpTo(aShipUpTo);
            _allocationCriteria.SetHeaderGradeShipUpTo(aSglRID, aLowBoundary, aShipUpTo);
            // end TT#488 - MD - Jellis - Group Allocation
        }
        // End TT#617 - RMatelic - Allocation Override - Add Ship Up To Rule (#36, #39)

        // Begin TT#2731 - JSmith - Unable to copy allocation override method from global
        private void Reservation_Define()
        {
            try
            {
                _imoDataSet = MIDEnvironment.CreateDataSet("reservationDataSet");

                DataTable setTable = _imoDataSet.Tables.Add("Sets");

                DataColumn dataColumn;
                //Create Columns and rows for datatable

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "SetID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Reservation Store";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Min Ship Qty";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Pct Pack Threshold";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Item Max";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                setTable.Columns.Add(dataColumn);

                //make set ID the primary key
                DataColumn[] PrimaryKeyColumn = new DataColumn[1];
                PrimaryKeyColumn[0] = setTable.Columns["SetID"];
                setTable.PrimaryKey = PrimaryKeyColumn;

                DataTable storeTable = _imoDataSet.Tables.Add("Stores");

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "SetID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Boolean");
                dataColumn.ColumnName = "Updated";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Store RID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = true;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Store ID";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Reservation Store";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Min Ship Qty";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Pct Pack Threshold";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Item Max";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                storeTable.Columns.Add(dataColumn);

                _imoDataSet.Relations.Add("Stores",
                    _imoDataSet.Tables["Sets"].Columns["SetID"],
                    _imoDataSet.Tables["Stores"].Columns["SetID"]);
            }
            catch 
            {
                throw;
            }
        }

        private void Reservation_Populate(int nodeRID)
        {
            Reservation_Populate(nodeRID, false);
        }

        public void Reservation_Populate(int nodeRID, bool aAttributeChanged)
        {
            try
            {
                Reservation_Define();

                IMOProfile imop;
                IMOMethodOverrideProfile imomop;

                if (!aAttributeChanged)
                {
                    _imoMethodOverrideProfileList = GetMethodOverrideIMO();

                    _applyVSW = GetApplyVSW();


                    if (nodeRID != Include.NoRID || IMOGroupLevelList == null)
                    {
                        //  if the list is empty, repopulate it
                        ProfileList storeList = StoreMgmt.StoreProfiles_GetActiveStoresList(); // SAB.StoreServerSession.GetActiveStoresList();
                        _imoProfileList = SAB.HierarchyServerSession.GetNodeIMOList(storeList, nodeRID);
                    }
                }

                foreach (StoreGroupLevelListViewProfile sglp in IMOGroupLevelList)
                {
                    _imoDataSet.Tables["Sets"].Rows.Add(new object[] { sglp.Name, string.Empty, string.Empty, string.Empty, string.Empty });

                    foreach (StoreProfile storeProfile in sglp.Stores)
                    {
                        if ((_imoProfileList != null) && (_imoProfileList.Contains(storeProfile.Key)))
                        {
                            //  the profile exists
                            imop = (IMOProfile)_imoProfileList.FindKey(storeProfile.Key);
                            imop.IMOIsDefault = false;
                        }
                        else
                        {
                            //  create a new profile with existing data
                            //      if so, get it
                            //      if not, make it up
                            imop = new IMOProfile(storeProfile.Key);
                            imop.IMOStoreRID = storeProfile.Key;
                            imop.IMOMinShipQty = 0;
                            imop.IMOPackQty = Include.PercentPackThresholdDefault;
                            imop.IMOMaxValue = int.MaxValue;
                            imop.IMOIsDefault = true;
                        }

                        //  is this store key in the method override data?
                        if ((_imoMethodOverrideProfileList.Contains(storeProfile.Key)) && (nodeRID == Include.NoRID))
                        {
                            //  blend it
                            imomop = (IMOMethodOverrideProfile)_imoMethodOverrideProfileList.FindKey(storeProfile.Key);

                            imop.IMOMaxValue = ((imomop.IMOMaxValue != Include.NoRID && imomop.IMOMaxValue != int.MaxValue) ? imomop.IMOMaxValue : imop.IMOMaxValue);
                            imop.IMOMinShipQty = ((imomop.IMOMinShipQty != Include.NoRID && imomop.IMOMinShipQty != 0) ? imomop.IMOMinShipQty : imop.IMOMinShipQty);
                            imop.IMOPackQty = ((imomop.IMOPackQty != Include.NoRID && imomop.IMOPackQty != Include.PercentPackThresholdDefault) ? imomop.IMOPackQty : imop.IMOPackQty);
                            imop.IMOIsDefault = false;
                        }

                        _imoDataSet.Tables["Stores"].Rows.Add(new object[] {sglp.Name, 
                            ((nodeRID == Include.NoRID) ? false : true),  // if this is the initial load, the nodeRid will equal include.norid
                            imop.IMOStoreRID, 
                            storeProfile.Text,
                            (storeProfile.IMO_ID ?? String.Empty),
                            (((imop.IMOIsDefault == true) || (imop.IMOMinShipQty == 0)) ? String.Empty : imop.IMOMinShipQty.ToString()),
                            (((imop.IMOIsDefault == true) || (imop.IMOPackQty == Include.PercentPackThresholdDefault)) ? String.Empty : (imop.IMOPackQty*100).ToString()),    
                            (((imop.IMOIsDefault == true) || (imop.IMOMaxValue == int.MaxValue)) ? String.Empty : imop.IMOMaxValue.ToString())
                        });

                    }
                }
            }
            catch 
            {
                throw;
            }
        }
        // End TT#2731 - JSmith - Unable to copy allocation override method from global

		public override void ProcessMethod(
			ApplicationSessionTransaction aApplicationTransaction,
			int aStoreFilter, Profile methodProfile)
		{
			//  BEGIN MIDTrack #2382  Failed Action when processing multiple headers
			aApplicationTransaction.ResetAllocationActionStatus();
			//  END MIDTrack #2382

			foreach (AllocationProfile ap in (AllocationProfileList)aApplicationTransaction.GetMasterProfileList(eProfileType.Allocation))
			{
//              BEGIN MIDTrack #2382  Failed Action when processing multiple headers
//				aApplicationTransaction.ResetAllocationActionStatus();
//              END MIDTrack #2382

				AllocationWorkFlowStep awfs = 
					new AllocationWorkFlowStep(
					this,
					new GeneralComponent(eGeneralComponentType.Total),
					false,
					true,
//					aApplicationTransaction.SAB.ApplicationServerSession.GlobalOptions.BalanceTolerancePercent,
					aApplicationTransaction.GlobalOptions.BalanceTolerancePercent,
					aStoreFilter,
					-1);
				this.ProcessAction(
					aApplicationTransaction.SAB,
					aApplicationTransaction,
					awfs,
					ap,
					true,
					Include.NoRID);
			}
		}

		/// <summary>
		/// Processes the action associated with this method.
		/// </summary>
		/// <param name="aSAB">Session Address Block</param>
		/// <param name="aApplicationTransaction">An instance of the Application Transaction object</param>
		/// <param name="aWorkFlowStep">Workflow Step that describes parameters associated with this action.</param>
		/// <param name="aProfile">Allocation Profile to which to apply this action</param>
		/// <param name="aWriteToDB">True: write results of action to database; False: Do not write results of action to database.</param>
		public override void ProcessAction(
			SessionAddressBlock aSAB, 
			ApplicationSessionTransaction aApplicationTransaction, 
			ApplicationWorkFlowStep aWorkFlowStep, 
			Profile aProfile,
			bool aWriteToDB,
			int aStoreFilterRID)
		{
            //bool actionSuccess = true;  // TT#488 - MD - JEllis - Group Allocation - field not  used
            AllocationProfile ap = aProfile as AllocationProfile;
            Audit audit = aSAB.ApplicationServerSession.Audit;
            if (ap == null)
            {
                string auditMsg = MIDText.GetText(eMIDTextCode.msg_NotAllocationProfile);
                audit.Add_Msg(
                    eMIDMessageLevel.Severe,
                    eMIDTextCode.msg_NotAllocationProfile,
                    auditMsg,
                    this.GetType().Name);
                throw new MIDException(eErrorLevel.severe,
                    (int)(eMIDTextCode.msg_NotAllocationProfile),
                    auditMsg);
            }
            // Begin TT#1966-MD - JSmith- DC Fulfillment
            else if (ap.IsMasterHeader && ap.DCFulfillmentProcessed)
            {
                string errorMessage = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed), ap.HeaderID);
                audit.Add_Msg(
                    eMIDMessageLevel.Warning,
                    eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed,
                    this.Name + " " + errorMessage,
                    this.GetType().Name);
                aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
            }
            else if (ap.IsSubordinateHeader && !ap.DCFulfillmentProcessed)
            {
                string errorMessage = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed), ap.HeaderID);
                audit.Add_Msg(
                    eMIDMessageLevel.Warning,
                    eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed,
                    this.Name + " " + errorMessage,
                    this.GetType().Name);
                aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
            }
            else
            {
                // End TT#1966-MD - JSmith- DC Fulfillment
                AllocationWorkFlowStep allocationWorkFlowStep = (AllocationWorkFlowStep)aWorkFlowStep;

                OverrideAllocationCriteria oac
                    = new OverrideAllocationCriteria(ap.Session, aApplicationTransaction);
                _allocationCriteria.StoreVSWOverride = null;
                _allocationCriteria.PackRoundingOverrideList = new List<HeaderPackRoundingOverride>();
                if (!ap.AllocationStarted || ap.AllUnitsInReserve)
                {
                    int storeRID;
                    double packQty;
                    IMOProfileList ipl = new IMOProfileList(eProfileType.IMO);
                    IMOProfile imop = null;
                    ProfileList storeList = StoreMgmt.StoreProfiles_GetActiveStoresList(); //SAB.StoreServerSession.GetActiveStoresList();
                    IMOMethodOverrideProfileList imoMOPL = _mao.GetMethodOverrideIMO(Include.NoRID);
                    if (ap.BeginDay == Include.UndefinedDate)
                    {
                        if (_applyVSW)	// Should we apply ANY IMO values
                        {
                            if (imoMOPL.Count > 0)	// Does the Method Oriver have any values? If so, use them.
                            {
                                foreach (IMOMethodOverrideProfile imomop in imoMOPL)
                                {
                                    if (imomop.IMO_Apply_VSW == true)
                                    {
                                        storeRID = imomop.IMOStoreRID;
                                        packQty = imomop.IMOPackQty;
                                        if (storeList.Contains(storeRID))
                                        {
                                            imop = new IMOProfile(storeRID);
                                            imop.IMOStoreRID = imomop.IMOStoreRID;
                                            imop.IMOMinShipQty = ((imomop.IMOMinShipQty < 0) ? 0 : imomop.IMOMinShipQty);

                                            imop.IMOPackQty = ((packQty > 0) && (packQty < 1)) ? packQty : ((packQty < 0) ? Include.PercentPackThresholdDefault : (packQty / 100));

                                            imop.IMOMaxValue = ((imomop.IMOMaxValue < 0) ? int.MaxValue : imomop.IMOMaxValue);
                                            imop.IMOPshToBackStock = 0;
                                            ipl.Add(imop);
                                        }
                                    }
                                }
                                _allocationCriteria.StoreVSWOverride = ipl;
                            }
                        }
                        else // ApplyVSW was false, remove any previous values
                        {
                            foreach (StoreProfile sp in storeList.ArrayList)
                            {
                                imop = new IMOProfile(sp.Key);
                                imop.IMOStoreRID = sp.Key;
                                imop.IMOMinShipQty = 0;
                                imop.IMOPackQty = Include.PercentPackThresholdDefault;
                                imop.IMOMaxValue = int.MaxValue;
                                imop.IMOPshToBackStock = 0;
                                ipl.Add(imop);
                            }
                            _allocationCriteria.StoreVSWOverride = ipl;
                        }
                    }
                    DataTable dtPackRounding = _dsOverRide.Tables["PackRounding"];
                    foreach (DataRow row in dtPackRounding.Rows)
                    {
                        HeaderPackRoundingOverride hpro = new HeaderPackRoundingOverride();
                        hpro.HeaderRid = this.Key;
                        hpro.PackMultipleRid = Convert.ToInt32(row["PackMultiple"]);
                        if (row["FstPack"] == DBNull.Value)
                        {
                            hpro.PackRounding1stPack = Include.DefaultGenericPackRounding1stPackPct;
                        }
                        else
                        {
                            hpro.PackRounding1stPack = Convert.ToDouble(row["FstPack"]);
                        }
                        if (row["NthPack"] == DBNull.Value)
                        {
                            hpro.PackRoundingNthPack = Include.DefaultGenericPackRoundingNthPackPct;
                        }
                        else
                        {
                            hpro.PackRoundingNthPack = Convert.ToDouble(row["NthPack"]);
                        }
                        _allocationCriteria.PackRoundingOverrideList.Add(hpro);
                    }
                }
                MIDException statusMessage;
                if (oac.ProcessAllocationCriteria(
                    (AllocationWorkFlowStep)aWorkFlowStep,
                    (AllocationProfile)aProfile,
                    _allocationCriteria,
                    out statusMessage))
                {
                    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
                    aApplicationTransaction.WriteAllocationAuditInfo
                        (ap.Key,
                        0,
                        this.MethodType,
                        this.Key,
                        this.Name,
                        allocationWorkFlowStep.Component.ComponentType,
                        null,
                        null,
                        0,
                        0
                        );
                }
                else
                {
                    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);
                    throw statusMessage;
                }

                // Begin TT#952 - MD - stodd - manula merge 
                //ap.SetStoreCapacityNotLoaded(); // TT#3145 - JSmith - Exceed capacity run manually versus run in a workflow give different results.
                // End TT#952 - MD - stodd - manula merge 
            }
        }

//            bool actionSuccess = true; // TT#1185 - Verify ENQ before Update
//            // begin TT#421 Detail packs/bulk not allocated by Size Need Method.
//            //AllocationProfile ap = (AllocationProfile) aAllocationProfile;
//            AllocationProfile ap = aProfile as AllocationProfile;
//            Audit audit = aSAB.ApplicationServerSession.Audit;
//            if (ap == null)
//            {
//                string auditMsg = MIDText.GetText(eMIDTextCode.msg_NotAllocationProfile);
//                audit.Add_Msg(
//                    eMIDMessageLevel.Severe,
//                    eMIDTextCode.msg_NotAllocationProfile,
//                    auditMsg,
//                    this.GetType().Name);
//                throw new MIDException(eErrorLevel.severe,
//                    (int)(eMIDTextCode.msg_NotAllocationProfile),
//                    auditMsg);
//            }
//            // end TT#421 Detail packs/bulk not allocated by Size Need Method.
//            try
//            {
//                // begin TT#421 Detail packs/bulk not allocated by Size Need Method.
//                ap.ResetTempLocks(false); // turn temp locks off.
//                // end TT#421 Detail packs/bulk not allocated by Size Need Method.
//                //Audit audit = aSAB.ApplicationServerSession.Audit;  // MID Track 3011 Do not process headers with status rec'd out of bal // TT#421 Detail packs/bulk not allocated by Size Need Method.
//                // BEGIN MID Track 3011 Headers with status Rec'd OUT of Bal are being processed
//                if (ap.HeaderAllocationStatus == eHeaderAllocationStatus.ReceivedOutOfBalance)
//                {
//                    string msg = string.Format(
//                        audit.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction,false), MIDText.GetTextOnly((int)ap.HeaderAllocationStatus));
//                    audit.Add_Msg(
//                        eMIDMessageLevel.Warning,eMIDTextCode.msg_HeaderStatusDisallowsAction,
//                        (this.Name + " " + ap.HeaderID + " " + msg),    // MID Track 3011 Headers with status Rec'd OUT of Bal are being processed
//                        this.GetType().Name);
//                    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
//                }
//                    // begin MID Track 4020 Plan level not reset on cancel
//                else if (this.OTSPlanRID == Include.NoRID
//                    && this.OTSPlanPHL != Include.NoRID
//                    && aApplicationTransaction.GetColorLevelSequence() == this.OTSPlanPHLSeq
//                    && ap.PlanLevelStartHnRID == ap.StyleHnRID) // Color level specified and more than one color
//                {
//                    audit.Add_Msg(
//                        eMIDMessageLevel.Warning,
//                        eMIDTextCode.msg_al_CannotDetermineOTSColorWhenMultipleColors,
//                        this.Name + " " + MIDText.GetTextOnly(eMIDTextCode.msg_al_CannotDetermineOTSColorWhenMultipleColors) + ": " + ap.HeaderID,
//                        this.GetType().Name);
//                    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
//                }
//                else if (this.OTSOnHandRID == Include.NoRID
//                    && this.OTSOnHandPHL != Include.NoRID
//                    && aApplicationTransaction.GetColorLevelSequence() == this.OTSOnHandPHLSeq
//                    && ap.PlanLevelStartHnRID == ap.StyleHnRID) // Color level specified and more than one color
//                {
//                    audit.Add_Msg(
//                        eMIDMessageLevel.Warning,
//                        eMIDTextCode.msg_al_CannotDetermineOnHandColorWhenMultipleColors,
//                        this.Name + " " + MIDText.GetTextOnly(eMIDTextCode.msg_al_CannotDetermineOnHandColorWhenMultipleColors) + ": " + ap.HeaderID,
//                        this.GetType().Name);
//                    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
//                }
//                    // end MID Track 4020 Plan level not reset on cancel
//                    // begin TT#1607 - JEllis - Inventory Basis is wrong when multi color
//                else if (_MERCH_HN_RID == Include.NoRID
//                    && _MERCH_PH_RID != Include.NoRID
//                    && aApplicationTransaction.GetColorLevelSequence() == _MERCH_PHL_SEQ
//                    && ap.PlanLevelStartHnRID == ap.StyleHnRID)
//                {
//                    string msg = string.Format(
//                        audit.GetText(
//                        eMIDTextCode.msg_al_DynamicColorBasisInvalid, false),
//                        ap.HeaderID,
//                        MIDText.GetTextOnly(eMIDTextCode.lbl_InventoryBasis),
//                        MIDText.GetTextOnly((int)eMethodType.AllocationOverride),
//                        MIDText.GetTextOnly((int)eHierarchyLevelType.Color));
//                    audit.Add_Msg(
//                        MIDText.GetMessageLevel((int)eMIDTextCode.msg_al_DynamicColorBasisInvalid),
//                        eMIDTextCode.msg_al_DynamicColorBasisInvalid,
//                        msg,
//                        GetType().Name);
//                    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
//                }
//                // end TT#1607 - JEllis - Inventory Basis is wrong when multi color
//                else
//                {
//                    // END MID Track 3011 Headers with status Rec'd OUT of Bal are being processed
//                    AllocationWorkFlowStep allocationWorkFlowStep = (AllocationWorkFlowStep) aWorkFlowStep;
//                    // Begin TT#2008 - JSmith - Allocation Workflow Not Anticipated Store Quantities
//                    //ap.LoadStores();  // TT#1360 - Exceed Maximum Broken
//                    if (!ap.StoresLoaded)
//                    {
//                        ap.LoadStores();  // TT#1360 - Exceed Maximum Broken
//                    }
//                    // End TT#2008
//                    ap.PercentNeedLimit = this.PercentNeedLimit;
//                    Index_RID storeIdxRID;
//                    bool exceedCapacity;
//                    bool calculateCapacityMaximums = false;
//                    double exceedCapacityPercent;
//                    foreach (StoreGroupLevelProfile sglp in SAB.StoreServerSession.GetStoreGroupLevelList(this._storeGroupRID))
//                    {
//                        if (this.StrGroupLvlIsInMethod(sglp.Key))
//                        {
//                            exceedCapacity = false; // MID Track 4166 Override Capacity not working
//                            if (this.ExceedCapacity)
//                            {
//                                exceedCapacity = this.ExceedCapacity;
//                                exceedCapacityPercent = double.MaxValue;
//                            }
//                            else
//                            {
//                                // begin  MID Track 3961 Capacity override not working
//                                // // begin MID Track 4166 Override Capacity not working
//                                // //exceedCapacity = this.GetStrGroupLvlExceedCapacity(sglp.Key);
//                                // //
//                                // //if (exceedCapacity)
//                                // if (this.GetStrGroupLvlExceedCapacity(sglp.Key))
//                                // 	// end MID Track 4166 Override Capacity not working
//                                // {
//                                // 	exceedCapacityPercent = this.GetStrGroupLvlExceedCapacityByPct(sglp.Key);
//                                // }
//                                // else
//                                // {
//                                // 	exceedCapacityPercent = 0;
//                                // }
//                                exceedCapacity = this.GetStrGroupLvlExceedCapacity(sglp.Key);
//                                if (exceedCapacity)
//                                {
//                                    exceedCapacityPercent = this.GetStrGroupLvlExceedCapacityByPct(sglp.Key);
//                                    if (exceedCapacityPercent <= 0)
//                                    {
//                                        exceedCapacityPercent = double.MaxValue;
//                                    }
//                                }
//                                else
//                                {
//                                    exceedCapacityPercent = 0;
//                                }
//                                // end MID Track 3961 Capacity override not working
//                            }
//                            foreach (StoreProfile sp in sglp.Stores)
//                            {
//                                // if sp.key is in the store filter
//                                storeIdxRID = ap.StoreIndex(sp.Key);
//                                //ap.SetStoreMayExceedMax(storeIdxRID, this.ExceedMaximums); // TT#1360 Exceed Maximums Broken
//                                if (exceedCapacity != ap.GetStoreMayExceedCapacity(storeIdxRID)
//                                    || exceedCapacityPercent != ap.GetStoreCapacityExceedByPct(storeIdxRID))
//                                {
//                                    calculateCapacityMaximums = true;
//                                }
//                                ap.SetStoreMayExceedCapacity(storeIdxRID, exceedCapacity);
//                                ap.SetStoreCapacityExceedByPct(storeIdxRID, exceedCapacityPercent);
//                            }
//                        }
//                    }
//                    // begin TT#1360 Exceed Maximums Broken
//                    foreach (Index_RID strIdxRID in ap.AppSessionTransaction.StoreIndexRIDArray())
//                    {
//                        ap.SetStoreMayExceedMax(strIdxRID, this.ExceedMaximums);
//                    }
//                    // end TT#1360 Exceed Maximums Broken
//                    // BEGIN TT#1287 - AGallagher - Inventory Min/Max
//                    if (this._MERCH_HN_RID == Include.NoRID)
//                    {
//                        if (this._MERCH_PH_RID == Include.NoRID)
//                        {
//                            HierarchyNodeProfile hnp = aApplicationTransaction.GetPlanLevelData(ap.PlanLevelStartHnRID);
//                            if (hnp == null)
//                            {
//                                throw new MIDException(eErrorLevel.severe,
//                                    (int)(eMIDTextCode.msg_al_PlanLevelUndetermined),
//                                    MIDText.GetText(eMIDTextCode.msg_al_PlanLevelUndetermined));
//                            }
//                            else
//                            {
//                                InventoryBasisHnRID = hnp.Key;
//                            }
//                        }
//                        else
//                        {
//                            InventoryBasisHnRID = aApplicationTransaction.GetAncestorDataByLevel(this._MERCH_PH_RID, ap.PlanLevelStartHnRID, this._MERCH_PHL_SEQ).Key;
//                        }
//                    }
//                    else
//                    {
//                        InventoryBasisHnRID = this._MERCH_HN_RID;
//                    }

//                    if (_InventoryInd == 'I')
//                    { InventoryMinMax = true; }
//                    else
//                    { InventoryMinMax = false; }

//                    //ap.SetGradeMinimumMaximumType(InventoryMinMax, InventoryBasisHnRID, out StatusMessage);  // TT#488 - MD - Jellis - Group Allocation
//                    if (!ap.SetGradeMinimumMaximumType(InventoryMinMax, InventoryBasisHnRID, out StatusMessage)) // TT#488 - MD - Jellis - Group Allocation
//                    {                               // TT#488 - MD - Jellis - Group Allocation
//                        throw StatusMessage;        // TT#488 - MD - Jellis - Group Allocation
//                    }                               // TT#488 - MD - Jellis - Group Allocation
//                    // END TT#1287 - AGallagher - Inventory Min/Max
//                    if (calculateCapacityMaximums)
//                    {
//                        ap.CalculateCapacityMaximum();
//                    }
//                    if (this.GradeWeekCount > 0)
//                    {
//                        ap.GradeWeekCount = this.GradeWeekCount;
//                    }
//                    if (this.OTSOnHandRID == Include.NoRID)
//                    {
//                        if (this.OTSOnHandPHL == Include.NoRID)
//                        {
//                            // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level
//                            //if (this.OTSOnHandPHLSeq == 0)
//                            if (this.OTSOnHandPHLSeq == 0 && !this.OnHandUnspecified)
//                            // End TT#709
//                            {
//                                //							ap.OnHandHnRID = (aSAB.HierarchyServerSession.GetAncestorData(ap.StyleHnRID, eHierarchyLevelType.Planlevel)).Key;
//                                // BEGIN MID Track #3872 - use color or style node for plan level lookup
//                                //ap.OnHandHnRID = (aApplicationTransaction.GetPlanLevelData(ap.StyleHnRID)).Key;
							
//                                HierarchyNodeProfile hnp = aApplicationTransaction.GetPlanLevelData(ap.PlanLevelStartHnRID);
//                                if (hnp == null)
//                                {
//                                    throw new MIDException(eErrorLevel.severe,
//                                        (int)(eMIDTextCode.msg_al_PlanLevelUndetermined),
//                                        MIDText.GetText(eMIDTextCode.msg_al_PlanLevelUndetermined));
//                                }
//                                else
//                                    ap.OnHandHnRID = hnp.Key;
//                                // END MID Track #3872 
//                            }
//                        }
//                        else
//                        {
//                            //ap.OnHandHnRID = SAB.HierarchyServerSession.GetAncestorDataByLevel(this.OTSOnHandPHL, ap.StyleHnRID, this.OTSOnHandPHLSeq).Key; // MID Change j.ellis Performance--cache ancestor data
//                            //ap.OnHandHnRID = aApplicationTransaction.GetAncestorDataByLevel(this.OTSOnHandPHL, ap.StyleHnRID, this.OTSOnHandPHLSeq).Key; // MID Change j.ellis Performance--cache ancestor data // MID Track 4020 Plan level not reset on cancel
//                            ap.OnHandHnRID = aApplicationTransaction.GetAncestorDataByLevel(this.OTSOnHandPHL, ap.PlanLevelStartHnRID, this.OTSOnHandPHLSeq).Key; // MID Change j.ellis Performance--cache ancestor data // MID Track 4020 Plan level not reset on cancel
//                        }
//                    }
//                    else
//                    {
//                        ap.OnHandHnRID = this.OTSOnHandRID;
//                    }
//                    if (this.OTSPlanFactorPercent > 0)
//                    {
//                        ap.PlanFactor = this.OTSPlanFactorPercent;
//                    }

//                    if ((this.OTSPlanRID == Include.NoRID))
//                    {
//                        if (this.OTSPlanPHL == Include.NoRID)
//                        {
//                            // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level
//                            //if (this.OTSPlanPHLSeq == 0)
//                            if (this.OTSPlanPHLSeq == 0 && !this.MerchUnspecified)
//                            // End TT#709
//                            {
//                                //							ap.PlanHnRID = (aSAB.HierarchyServerSession.GetAncestorData(ap.StyleHnRID, eHierarchyLevelType.Planlevel)).Key;
//                                // BEGIN MID Track #3872 - use color or style node for plan level lookup
//                                //ap.PlanHnRID = (aApplicationTransaction.GetPlanLevelData(ap.StyleHnRID)).Key;
								
//                                HierarchyNodeProfile hnp = aApplicationTransaction.GetPlanLevelData(ap.PlanLevelStartHnRID);
//                                if (hnp == null)
//                                {
//                                    throw new MIDException(eErrorLevel.severe,
//                                        (int)(eMIDTextCode.msg_al_PlanLevelUndetermined),
//                                        MIDText.GetText(eMIDTextCode.msg_al_PlanLevelUndetermined));
//                                }
//                                else
//                                    ap.PlanHnRID = hnp.Key;
//                                // END MID Track #3872
//                            }
//                        }
//                        else
//                        {
//                            //ap.PlanHnRID = SAB.HierarchyServerSession.GetAncestorDataByLevel(this.OTSPlanPHL, ap.StyleHnRID, this.OTSPlanPHLSeq).Key; // MID Change j.ellis Performance--cache ancestor data
//                            //ap.PlanHnRID = aApplicationTransaction.GetAncestorDataByLevel(this.OTSPlanPHL, ap.StyleHnRID, this.OTSPlanPHLSeq).Key; // MID Change j.ellis Performance--cache ancestor data // MID Track 4020 Plan level not reset on cancel
//                            ap.PlanHnRID = aApplicationTransaction.GetAncestorDataByLevel(this.OTSPlanPHL, ap.PlanLevelStartHnRID, this.OTSPlanPHLSeq).Key; // MID Change j.ellis Performance--cache ancestor data // MID Track 4020 Plan level not reset on cancel
//                        }
//                    }
//                    else
//                    {
//                        ap.PlanHnRID = this.OTSPlanRID;
//                    }

//                    // begin TT#2853 - Jellis - Orvis: apply AL Override after Fill Size Gets Unexpected Results
//                    //if (!ap.AllocationStarted ||                                
//                    //    (ap.AllocationStarted && ap.AllUnitsInReserve))         
//                    //{
//                    // begin TT#488 - MD - Jellis - Group Allocation                                   
//                    //if (this._grade != null &&
//                    //    this._grade.Count > 0)
//                    if (_grade.Count > 0)
//                     // end TT#488 - MD - Jellis - Group Allocation
//                    {
//                        ArrayList aGradeList = new ArrayList();
//                        AllocationGradeBin gradeBin;
//                        // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
//                        //foreach(AllocationGradeBin aGradeBin in _grade.Values)
//                        //{
//                        //    // begin MID Track 4133 When multiple headers processed by override, cached "grades" were used instead of original
//                        //    //aGradeList.Add(aGradeBin);
//                        //    gradeBin = new AllocationGradeBin();
//                        //    gradeBin.SetGrade(aGradeBin.Grade);
//                        //    gradeBin.SetLowBoundary(aGradeBin.LowBoundary);
//                        //    gradeBin.SetGradeAdMinimum(aGradeBin.GradeAdMinimum);
//                        //    gradeBin.SetGradeColorMaximum(aGradeBin.GradeColorMaximum);
//                        //    gradeBin.SetGradeColorMinimum(aGradeBin.GradeColorMinimum);
//                        //    gradeBin.SetGradeMaximum(aGradeBin.GradeMaximum);
//                        //    gradeBin.SetGradeMinimum(aGradeBin.GradeMinimum);
//                        //    gradeBin.SetGradeOriginalAdMinimum(aGradeBin.GradeOriginalAdMinimum);
//                        //    gradeBin.SetGradeOriginalMaximum(aGradeBin.GradeOriginalMaximum);
//                        //    gradeBin.SetGradeOriginalMinimum(aGradeBin.GradeOriginalMinimum);
//                        //    aGradeList.Add(gradeBin);
//                        //    // end MID Track 4133 When multiple headers processed by override, cached "grades" were used instead of original
//                        //}
//                        ap.GradeSG_RID = this.sGstoreGroupRID;
//                        //foreach (Hashtable boundaryHT in _grade.Values)  // TT#488 - MD - Jellis - Group Allocation
//                        foreach (Dictionary<double, AllocationGradeBin> boundaryDICT in _grade.Values) // TT#488 - MD - Jellis - Group Allocation
//                        {
//                            //foreach (AllocationGradeBin pGradeBin in boundaryHT.Values) // TT#488 - MD - Jellis - Group Allocation
//                            foreach (AllocationGradeBin pGradeBin in boundaryDICT.Values) // TT#488 - MD - Jellis - Group Allocation
//                            {
//                                gradeBin = new AllocationGradeBin();
//                                gradeBin.SetGradeAttributeSet(pGradeBin.GradeSglRID);
//                                gradeBin.SetLowBoundary(pGradeBin.LowBoundary);
//                                gradeBin.SetGrade(pGradeBin.Grade);
//                                gradeBin.SetGradeAdMinimum(pGradeBin.GradeAdMinimum);
//                                gradeBin.SetGradeColorMaximum(pGradeBin.GradeColorMaximum);
//                                gradeBin.SetGradeColorMinimum(pGradeBin.GradeColorMinimum);
//                                gradeBin.SetGradeMaximum(pGradeBin.GradeMaximum);
//                                gradeBin.SetGradeMinimum(pGradeBin.GradeMinimum);
//                                gradeBin.SetGradeOriginalAdMinimum(pGradeBin.GradeOriginalAdMinimum);
//                                gradeBin.SetGradeOriginalMaximum(pGradeBin.GradeOriginalMaximum);
//                                gradeBin.SetGradeOriginalMinimum(pGradeBin.GradeOriginalMinimum);
//                                gradeBin.SetGradeShipUpTo(pGradeBin.GradeShipUpTo);      // TT#617 - RMatelic - Allocation Override - Add Ship Up To Rule (#36, #39)
//                                aGradeList.Add(gradeBin);
//                            }
//                        }
//                        // End TT#618
//                        ap.GradeList = aGradeList;
//                    }
//                    if (!ap.AllocationStarted ||                                 
//                        (ap.AllocationStarted && ap.AllUnitsInReserve))    
//                    {
//                        // end     TT#2853 - Jellis - Orvis: apply AL Override after Fill Size Gets Unexpected Results                        
//                        if (ReserveQty != Include.UndefinedReserve)
//                        {
//                            // begin TT#667 Allow bulk and pack reserve
//                            //if (ReserveIsPercent)
//                            //{
//                            //    ap.ReserveUnits = (int)(((double)ap.TotalUnitsToAllocate * ((double)ReserveQty / 100.0d)) + .5d);
//                            //}
//                            //else
//                            //{
//                            //    ap.ReserveUnits = (int)ReserveQty;
//                            //}
//                            //foreach (PackHdr p in ap.Packs.Values)
//                            //{
//                            //    p.SetReservePacks(0);
//                            //}
//                            //foreach (HdrColorBin c in ap.BulkColors.Values)
//                            //{
//                            //    c.SetReserveUnits(0);
//                            //    foreach (HdrSizeBin s in c.ColorSizes.Values)
//                            //    {
//                            //        s.SetReserveUnits(0);
//                            //    }
//                            //}
//                            //if (ap.ReserveUnits > 0)
//                            //{
//                            //    ap.AllocateReserve();
//                            //}
//                            AllocateReserveSpecification ars =
//                                 new AllocateReserveSpecification(ReserveIsPercent, ReserveQty, this.ReserveAsPacks, this.ReserveAsBulk);
//                            ap.AllocateReserve(ars);
//                            // end TT#667 Allow pack and bulk reserve
//                        }

//                        //ap.PackRounding1stPack = this._

//                    }
//                    foreach (HdrColorBin c in ap.BulkColors.Values)
//                    {
//// Begin Assortment: Color/Size change
//                        if (this.ColorMinMaxIsInMethod(c.ColorCodeRID))
//                        {
//                            ap.SetColorMaximum(c.ColorCodeRID,GetColorMax(c.ColorCodeRID));
//                            ap.SetColorMinimum(c.ColorCodeRID,GetColorMin(c.ColorCodeRID));
//                        }
//                        else
//                        {
//                            ap.SetColorMaximum(c.ColorCodeRID,AllColorMaximum);
//                            ap.SetColorMinimum(c.ColorCodeRID,AllColorMinimum);
//                        }
//                        ap.SetColorMultiple(c.ColorCodeRID,AllColorMultiple);
//                        foreach(HdrSizeBin s in c.ColorSizes.Values)
//                        {

//                            ap.SetSizeMaximum(c.ColorCodeRID, s.SizeCodeRID, GetColorSizeMax(c.ColorCodeRID, s.SizeCodeRID)); // Assortment: color/size changes
//                            ap.SetSizeMinimum(c.ColorCodeRID, s.SizeCodeRID, GetColorSizeMin(c.ColorCodeRID, s.SizeCodeRID)); // Assortment: color/size changes
//                            ap.SetSizeMultiple(c.ColorCodeRID, s.SizeCodeRID, AllSizeMultiple); // Assortment: color/size changes
//                        }
//// End Assortment: Color/Size Change
//                    }
//                    // BEGIN TT#1401 - GTaylor - Reservation Stores - VSW
//                    if (!ap.AllocationStarted || ap.AllUnitsInReserve)
//                    {
//                        int storeRID;
//                        double packQty;
//                        IMOProfileList ipl = new IMOProfileList(eProfileType.IMO);
//                        IMOProfile imop = null;
//                        ProfileList storeList = SAB.StoreServerSession.GetAllStoresList();
//                        IMOMethodOverrideProfileList imoMOPL = _mao.GetMethodOverrideIMO(Include.NoRID);
//                        if (ap.BeginDay == Include.UndefinedDate)
//                        {
//                            // BEGIN TT#1401 - stodd - VSW changes
//                            if (_applyVSW)	// Should we apply ANY IMO values
//                            {
//                                if (imoMOPL.Count > 0)	// Does the Method Oriver have any values? If so, use them.
//                                {
//                                    //ipl = ap.SAB.HierarchyServerSession.GetNodeIMOList(storeList, ap.PlanLevelStartHnRID);
//                                    foreach (IMOMethodOverrideProfile imomop in imoMOPL)
//                                    {
//                                        if (imomop.IMO_Apply_VSW == true)
//                                        {
//                                            storeRID = imomop.IMOStoreRID;
//                                            packQty = imomop.IMOPackQty;
//                                            if (storeList.Contains(storeRID))
//                                            {
//                                                imop = new IMOProfile(storeRID);
//                                                imop.IMOStoreRID = imomop.IMOStoreRID;
//                                                imop.IMOMinShipQty = ((imomop.IMOMinShipQty < 0) ? 0 : imomop.IMOMinShipQty);

//                                                imop.IMOPackQty = ((packQty > 0) && (packQty < 1)) ? packQty : ((packQty < 0) ? Include.PercentPackThresholdDefault : (packQty / 100));

//                                                imop.IMOMaxValue = ((imomop.IMOMaxValue < 0) ? int.MaxValue : imomop.IMOMaxValue);
//                                                imop.IMOPshToBackStock = 0;
//                                                //try
//                                                //{
//                                                //    ipl.Remove(imop);
//                                                //}
//                                                //catch
//                                                //{
//                                                //    throw;
//                                                //}
//                                                ipl.Add(imop);
//                                            }
//                                        }
//                                    }
//                                    ap.SetStoreImoCriteria(ipl, true);  // TT#1401 - AGallagher - VSW  // TT#2083 - Urban Virtual Store Warehouse Min SHip Qty
//                                }
//                                else // No override values, use node properties values
//                                {
//                                    // BEGIN TT#1401 - AGallagher - VSW
//                                    // ipl = SAB.HierarchyServerSession.GetNodeIMOList(storeList, ap.PlanHnRID);
//                                    // END TT#1401 - AGallagher - VSW
//                                }
//                                // BEGIN TT#1401 - AGallagher - VSW
//                                // ap.SetStoreImoCriteria(ipl);
//                                // END TT#1401 - AGallagher - VSW
//                            }
//                            else // ApplyVSW was false, remove any previous values
//                            {
//                                foreach (StoreProfile sp in storeList.ArrayList)
//                                {
//                                    imop = new IMOProfile(sp.Key);
//                                    imop.IMOStoreRID = sp.Key;
//                                    imop.IMOMinShipQty = 0;
//                                    imop.IMOPackQty = Include.PercentPackThresholdDefault;
//                                    imop.IMOMaxValue = int.MaxValue;
//                                    imop.IMOPshToBackStock = 0;
//                                    ipl.Add(imop);
//                                }
//                                ap.SetStoreImoCriteria(ipl, true);  // TT#2083 - Urban Virtual Store Warehouse Min SHip Qty
//                            }
//                            // END TT#1401 - stodd - VSW changes
//                        }
//                    }
//                    // END TT#1401 - GTaylor - Reservation Stores - VSW
//                    // BEGIN TT#616 - stodd - pack rounding
//                    DataTable dtPackRounding = _dsOverRide.Tables["PackRounding"];
//                    ap.PackRoundingOverrideList = new System.Collections.Generic.List<HeaderPackRoundingOverride>();
//                    foreach (DataRow row in dtPackRounding.Rows)
//                    {
//                        HeaderPackRoundingOverride hpro = new HeaderPackRoundingOverride();
//                        hpro.HeaderRid = this.Key;
//                        hpro.PackMultipleRid = Convert.ToInt32(row["PackMultiple"]);
//                        if (row["FstPack"] == DBNull.Value)
//                        {
//                            hpro.PackRounding1stPack = Include.DefaultGenericPackRounding1stPackPct;
//                        }
//                        else
//                        {
//                            hpro.PackRounding1stPack = Convert.ToDouble(row["FstPack"]);
//                        }
//                        if (row["NthPack"] == DBNull.Value)
//                        {
//                            hpro.PackRoundingNthPack = Include.DefaultGenericPackRoundingNthPackPct;
//                        }
//                        else
//                        {
//                            hpro.PackRoundingNthPack = Convert.ToDouble(row["NthPack"]);
//                        }
//                        ap.PackRoundingOverrideList.Add(hpro);
//                    }
//                    // END TT#616 - stodd - pack rounding
					
//                    // change in _grade structure; count comparison is now not valid
//                    //if (this._grade != null
//                    //    && this._grade.Count == ap.GradeList.Count)
//                    // begin TT#488 - MD - Jellis - Group Allocation
//                    //if (this._grade != null)
//                    if (this._grade.Count > 0)
//                        // end TT#488 - MD - Jellis - Group Allocation
//                    {
//                        //AllocationGradeBin[] grades = new AllocationGradeBin[this._grade.Count];
//                        int boundaryCount = 0;
//                        //foreach (Hashtable boundaryHT in this._grade.Values) // TT#488 - MD - Jellis - Group Allocation
//                        foreach (Dictionary<double, AllocationGradeBin> boundaryDICT in _grade.Values) // TT#488 - MD - Jellis - Group Allocation
//                        {
//                            //boundaryCount += boundaryHT.Values.Count; // TT#488 - MD - Jellis - Group Allocation
//                            boundaryCount += boundaryDICT.Count;       // TT#488 - MD - Jellis - Group Allocation
//                        }
//                        AllocationGradeBin[] grades = new AllocationGradeBin[boundaryCount];
//                        // End TT#618

//                        // begin MID Track 4133 When multiple headers processed by override, cached "grades" were used instead of original
//                        //this._grade.Values.CopyTo(grades,0);
//                        int j = 0;
//                        // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
//                        //foreach (AllocationGradeBin agb in this._grade.Values)
//                        //{
//                        //    grades[j] = new AllocationGradeBin();
//                        //    grades[j].SetGrade(agb.Grade);
//                        //    grades[j].SetLowBoundary(agb.LowBoundary);
//                        //    grades[j].SetGradeAdMinimum(agb.GradeAdMinimum);
//                        //    grades[j].SetGradeColorMaximum(agb.GradeColorMaximum);
//                        //    grades[j].SetGradeColorMinimum(agb.GradeColorMinimum);
//                        //    grades[j].SetGradeMaximum(agb.GradeMaximum);
//                        //    grades[j].SetGradeMinimum(agb.GradeMinimum);
//                        //    grades[j].SetGradeOriginalAdMinimum(agb.GradeOriginalAdMinimum);
//                        //    grades[j].SetGradeOriginalMaximum(agb.GradeOriginalMaximum);
//                        //    grades[j].SetGradeOriginalMinimum(agb.GradeOriginalMinimum);
//                        //    j++;
//                        //}
//                        // end MID Track 4133 When multiple headers processed by override, cached "grades" were used instead of original
//                        //foreach (Hashtable boundaryHT in this._grade.Values) // TT#488 - MD - Jellis - Group Allocation
//                        foreach (Dictionary<double, AllocationGradeBin> boundaryDICT in _grade.Values) // TT#488 - MD - Jellis - Group Allocation
//                        {
//                            //foreach (AllocationGradeBin agb in boundaryHT.Values) // TT#488 - MD - Jellis - Group Allocation
//                            foreach (AllocationGradeBin agb in boundaryDICT.Values) // TT#488 - MD - Jellis - Group Allocation
//                            {
//                                grades[j] = new AllocationGradeBin();
//                                grades[j].SetGradeAttributeSet(agb.GradeSglRID);
//                                grades[j].SetGrade(agb.Grade);
//                                grades[j].SetLowBoundary(agb.LowBoundary);
//                                grades[j].SetGradeAdMinimum(agb.GradeAdMinimum);
//                                grades[j].SetGradeColorMaximum(agb.GradeColorMaximum);
//                                grades[j].SetGradeColorMinimum(agb.GradeColorMinimum);
//                                grades[j].SetGradeMaximum(agb.GradeMaximum);
//                                grades[j].SetGradeMinimum(agb.GradeMinimum);
//                                grades[j].SetGradeOriginalAdMinimum(agb.GradeOriginalAdMinimum);
//                                grades[j].SetGradeOriginalMaximum(agb.GradeOriginalMaximum);
//                                grades[j].SetGradeOriginalMinimum(agb.GradeOriginalMinimum);
//                                grades[j].SetGradeShipUpTo(agb.GradeShipUpTo);      // TT#617 - RMatelic - Allocation Override - Add Ship Up To Rule (#36, #39)
//                                j++;
//                            }
//                        }
//                        // End TT#618

//                        // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
//                        //Array.Sort(grades, new AllocationGradeBinCompareDescend());
//                        //bool gradesEqual = true;
//                        ArrayList gradeList = new ArrayList();
//                        for (int i = 0; i < grades.Length; i++)
//                        {
//                            //if (((AllocationGradeBin)ap.GradeList[i]).LowBoundary !=
//                            //    ((AllocationGradeBin)grades[i]).LowBoundary)
//                            //{
//                            //    gradesEqual = false;
//                            //    break;
//                            //}
//                            gradeList.Add((AllocationGradeBin)grades[i]);
//                        }
//                        //if (gradesEqual)
//                        //{
//                        //    ap.GradeList = gradeList;
//                        //    //for(int i=0; i<grades.Length; i++)
//                        //    //{
//                        //    //	ap.GradeList[i] = grades[i];
//                        //    //}
//                        //}
//                        ap.GradeList = gradeList;
//                        // End TT#618
//                    }
//                    if (actionSuccess) // TT#1185 - Verify ENQ before Update 
//                    {                  // TT#1185 - Verify ENQ before Update
//                        if (aWriteToDB)
//                        {
//                            actionSuccess = ap.WriteHeader(); // TT#1185 - Veriy ENQ before Update
//                        }
//                        if (actionSuccess) // TT#1185 - Verify ENQ before Update
//                        {                  // TT#1185 - Verify ENQ before Update
//                            aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
//                            // begin MID Track 4448 AnF Audit Enhancement
//                            aApplicationTransaction.WriteAllocationAuditInfo
//                                (ap.Key,
//                                0,
//                                this.MethodType,
//                                this.Key,
//                                this.Name,
//                                allocationWorkFlowStep.Component.ComponentType,
//                                null,
//                                null,  // MID Track 4448 AnF Audit Enhancement
//                                0,     // MID Track 4448 AnF Audit Enhancement
//                                0      // MID Track 4448 AnF Audit Enhancement
//                                );
//                            // end MID Track 4448 AnF Audit Enhancement	
//                        }   // TT#1185 - Verify ENQ before Update
//                        else // TT#1185 - Verify ENQ before Update
//                        {    // TT#1185 - Verify ENQ before Update
//                            aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);  // TT#1185 - Verify ENQ before update
//                        }    // TT#1185 - Verify ENQ before Update
//                    } // TT#1185 - Verify ENQ before Update
//                    else // TT#1185 - Verify ENQ before Update
//                    {  // TT#1185 - Verify ENQ before Update
//                        aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);  // TT#1185 - Verify ENQ before update
//                    } // TT#1185 - Verify ENQ before Update
//                } // MID Track 3011 Do not process headers with status of rec'd out bal
//            }
//            catch (Exception error)
//            {
//                aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);
//                string message = error.ToString();
//                throw;
//            }
//            finally
//            {
//                ap.ResetTempLocks(true); //TT#421 Detail packs/bulk not allocated by Size Need Method. 
//            }
//        }
			
  
		override public void Update(TransactionData td)
		{
			if (_mao == null || base.Key < 0 || base.Method_Change_Type == eChangeType.add)
			{
				_mao = new AllocationOverrideMethodData(td, base.Key);
			}
			//  TO DO
			_mao.Store_Filter_RID = _storeFilterRID;
			_mao.Grade_Week_Count = _allocationCriteria.GradeWeekCount;
			_mao.Exceed_Maximums_Ind = Include.ConvertBoolToChar(_allocationCriteria.ExceedMaximums);
			_mao.Percent_Need_Limit = _allocationCriteria.PercentNeedLimit;
			_mao.Reserve = _allocationCriteria.ReserveQty;
			_mao.Percent_Ind = Include.ConvertBoolToChar(_allocationCriteria.ReserveIsPercent);
			_mao.Merch_Plan_HN_RID = OTSPlanRID;
			_mao.Merch_Plan_PH_RID = OTSPlanPHL;
			_mao.Merch_Plan_PHL_SEQ = OTSPlanPHLSeq;
			_mao.Merch_OnHand_HN_RID = OTSOnHandRID;
			_mao.Merch_OnHand_PH_RID = OTSOnHandPHL;
			_mao.Merch_OnHand_PHL_SEQ = OTSOnHandPHLSeq;
			_mao.Plan_Factor_Percent = OTSPlanFactorPercent;
			_mao.All_Color_Multiple = _allocationCriteria.AllColorMultiple;
		    _mao.All_Size_Multiple = _allocationCriteria.AllSizeMultiple;
			_mao.All_Color_Minimum = AllColorMinimum;
			_mao.All_Color_Maximum = AllColorMaximum;
			_mao.Store_Group_RID = _allocationCriteria.CapacityStoreGroupRID;
            _mao.Tab_Store_Group_RID = _allocationCriteria.GradeStoreGroupRID; 
			_mao.Exceed_Capacity_Ind = Include.ConvertBoolToChar(_allocationCriteria.ExceedCapacity);
			_mao.UseStoreGradeDefault =UseStoreGradeDefault;
			_mao.UsePctNeedDefault = UsePctNeedDefault;
			_mao.UseFactorPctDefault = UseFactorPctDefault;
			_mao.UseAllColorsMinDefault =UseAllColorsMinDefault;
			_mao.UseAllColorsMaxDefault =UseAllColorsMaxDefault;
			_mao.DSOverRide = _dsOverRide;
            // Begin TT#2731 - JSmith - Unable to copy allocation override method from global
            //_mao.IMODataSet = _imoDataSet; // TT#1401 - GTaylor - Reservation Stores
            _mao.IMODataSet = IMODataSet;
            // Begin TT#2731 - JSmith - Unable to copy allocation override method from global
            _mao.ApplyVSW = _applyVSW; // TT#1401 - GTaylor - Reservation Stores
            // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level
            _mao.Merch_Plan_Unspecified = _allocationCriteria.MerchUnspecified;
            _mao.Merch_OnHand_Unspecified = _allocationCriteria.OnHandUnspecified;
            // End TT#709  
			// BEGIN TT#667 - Stodd - Pre-allocate Reserve
			_mao.ReserveAsBulk = _allocationCriteria.ReserveAsBulk;
			_mao.ReserveAsPacks = _allocationCriteria.ReserveAsPacks;
			// END TT#667 - Stodd - Pre-allocate Reserve
            // BEGIN TT#1287 - AGallagher - Inventory Min/Max
            _mao.Inventory_Ind = InventoryInd;
            _mao.IB_MERCH_HN_RID = MERCH_HN_RID;
            _mao.IB_MERCH_PH_RID = MERCH_PH_RID;
            _mao.IB_MERCH_PHL_SEQ = MERCH_PHL_SEQ;
            // END TT#1287 - AGallagher - Inventory Min/Max
			try
			{
				switch (base.Method_Change_Type)
				{
					case eChangeType.add:
						base.Update(td);
						_mao.InsertMethod(base.Key, td);
						break;
					case eChangeType.update:
						base.Update(td);
						_mao.UpdateMethod(base.Key, td);
						break;
					case eChangeType.delete:
						_mao.DeleteMethod(base.Key, td);
						base.Update(td);
						break;
				}
			}
			catch (Exception e)
			{
				string message = e.ToString();
				throw;
			}
			finally
			{
				//TO DO:  whatever has to be done after an update or exception.
			}
		}
		public override bool WithinTolerance(double aTolerancePercent)
		{
			return true;
		}

		/// <summary>
		/// Returns a copy of this object.
		/// </summary>
		/// <param name="aSession">
		/// The current session
		/// </param>
		/// <param name="aCloneDateRanges">
		/// A flag identifying if date ranges are to be cloned or use the original</param>
        /// <param name="aCloneCustomOverrideModels">
        /// A flag identifying if custom override models are to be cloned or use the original
        /// </param>
		/// <returns>
		/// A copy of the object.
		/// </returns>
        // Begin Track #5912 - JSmith - Save As needs to clone custom override models
        //override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges)
        override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges, bool aCloneCustomOverrideModels)
        // End Track #5912
		{
			AllocationOverrideMethod newAllocationOverrideMethod = null;

			try
			{
				newAllocationOverrideMethod = (AllocationOverrideMethod)this.MemberwiseClone();
				newAllocationOverrideMethod.AllColorMaximum = AllColorMaximum;
				newAllocationOverrideMethod.AllColorMinimum = AllColorMinimum;
				newAllocationOverrideMethod.AllColorMultiple = AllColorMultiple;
				newAllocationOverrideMethod.AllSizeMultiple = AllSizeMultiple;
				newAllocationOverrideMethod.DSOverRide = DSOverRide.Copy();
				newAllocationOverrideMethod.ExceedCapacity = ExceedCapacity;
				newAllocationOverrideMethod.ExceedMaximums = ExceedMaximums;
				newAllocationOverrideMethod.GradeWeekCount = GradeWeekCount;
				newAllocationOverrideMethod.Method_Change_Type = eChangeType.none;
				newAllocationOverrideMethod.Method_Description = Method_Description;
				newAllocationOverrideMethod.MethodStatus = MethodStatus;
				newAllocationOverrideMethod.Name = Name;
				newAllocationOverrideMethod.OTSOnHandPHL = OTSOnHandPHL;
				newAllocationOverrideMethod.OTSOnHandPHLSeq = OTSOnHandPHLSeq;
				newAllocationOverrideMethod.OTSOnHandRID = OTSOnHandRID;
				newAllocationOverrideMethod.OTSPlanFactorPercent = OTSPlanFactorPercent;
				newAllocationOverrideMethod.OTSPlanPHL = OTSPlanPHL;
				newAllocationOverrideMethod.OTSPlanPHLSeq = OTSPlanPHLSeq;
				newAllocationOverrideMethod.OTSPlanRID = OTSPlanRID;
				newAllocationOverrideMethod.PercentNeedLimit = PercentNeedLimit;
				newAllocationOverrideMethod.ReserveIsPercent = ReserveIsPercent;
				newAllocationOverrideMethod.ReserveQty = ReserveQty;
				newAllocationOverrideMethod.SG_RID = SG_RID;
				newAllocationOverrideMethod.StoreFilterRID = StoreFilterRID;
				newAllocationOverrideMethod.CapacityStoreGroupRID = CapacityStoreGroupRID;
                newAllocationOverrideMethod.GradeStoreGroupRID = GradeStoreGroupRID;
				newAllocationOverrideMethod.UseAllColorsMaxDefault = UseAllColorsMaxDefault;
				newAllocationOverrideMethod.UseAllColorsMinDefault = UseAllColorsMinDefault;
				newAllocationOverrideMethod.UseFactorPctDefault = UseFactorPctDefault;
				newAllocationOverrideMethod.UsePctNeedDefault = UsePctNeedDefault;
				newAllocationOverrideMethod.User_RID = User_RID;
				newAllocationOverrideMethod.Virtual_IND = Virtual_IND;
                newAllocationOverrideMethod.Template_IND = Template_IND;

                // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level
                newAllocationOverrideMethod.MerchUnspecified = MerchUnspecified;
                newAllocationOverrideMethod.OnHandUnspecified = OnHandUnspecified;
                // End TT#709  

				return newAllocationOverrideMethod;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// Begin MID Track 4858 - JSmith - Security changes
		/// <summary>
		/// Returns a flag identifying if the user can update the data on the method.
		/// </summary>
		/// <param name="aSession">
		/// The current session
		/// </param>
		/// <param name="aUserRID">
		/// The internal key of the user</param>
		/// <returns>
		/// A flag.
		/// </returns>
		override public bool AuthorizedToUpdate(Session aSession, int aUserRID)
		{
			return true;
		}
		// End MID Track 4858

        // Begin TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.
        /// <summary>
        /// Returns a flag identifying if the user can view the data on the method.
        /// </summary>
        /// <param name="aSession">
        /// The current session
        /// </param>
        /// <param name="aUserRID">
        /// The internal key of the user</param>
        /// <returns>
        /// A flag.
        /// </returns>
        override public bool AuthorizedToView(Session aSession, int aUserRID)
        {
            HierarchyNodeSecurityProfile hierNodeSecurity;

            if (_allocationCriteria.OTSOnHandRID != Include.NoRID)
            {
                hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_allocationCriteria.OTSOnHandRID, (int)eSecurityTypes.Store);
                if (!hierNodeSecurity.AllowView)
                {
                    return false;
                }
            }

            if (_allocationCriteria.OTSPlanRID != Include.NoRID)
            {
                hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_allocationCriteria.OTSPlanRID, (int)eSecurityTypes.Store);
                if (!hierNodeSecurity.AllowView)
                {
                    return false;
                }
            }

            if (_allocationCriteria.HdrInventory_MERCH_HN_RID != Include.NoRID)
            {
                hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_allocationCriteria.HdrInventory_MERCH_HN_RID, (int)eSecurityTypes.Store);
                if (!hierNodeSecurity.AllowView)
                {
                    return false;
                }
            }

            return true;
        }
        // End TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.

        // BEGIN RO-642 - RDewey
        override public FunctionSecurityProfile GetFunctionSecurity()
        {
            if (this.GlobalUserType == eGlobalUserType.Global)
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalAllocationOverride);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserAllocationOverride);
            }

        }

        int _storeGradesAttributeSetKey = Include.NoRID;
        int _storeGradesAttributeKey = Include.NoRID;
        int _storeGradesMerchandiseKey = 0;
        bool _populateStoreGrades = false;
        bool _storeGroupAttributeChanged = false;
        StoreGradeList _storeGradeList = null;

        int _VSWAttributeSetKey = Include.NoRID;
        int _VSWAttributeKey = Include.NoRID;

        override public ROMethodProperties MethodGetData(out bool successful, ref string message, bool processingApply = false)
        {
            successful = true;
            int? storeGradeWeekCount = null;
            int? colorMultiple = null;
            int? sizeMultiple = null;
            int? allColorMinimum = null;
            int? allColorMaximum = null;
            double? percentNeedLimit = null;
            double? reserve = null;
            double? reserveAsBulk = null;
            double? reserveAsPacks = null;
            double? onHandFactor = null;
            KeyValuePair<int, string> merchandise = default(KeyValuePair<int, string>);
            KeyValuePair<int, string> onHandMerchandise = default(KeyValuePair<int, string>);
            KeyValuePair<int, int> merchandiseHierarchy = default(KeyValuePair<int, int>);
            KeyValuePair<int, int> onHandMerchandiseHierarchy = default(KeyValuePair<int, int>);
            int capacityAttributeKey = Include.NoRID;
            ProfileList attributeSetList;

            eMerchandiseType otsMerchandiseType, otsOnHandType, inventoryBasisMerchandiseType;
            eMinMaxType localMinimumMaximumType;
            if (InventoryInd == 'I')
            {
                localMinimumMaximumType = eMinMaxType.Inventory;
            }
            else
            {
                localMinimumMaximumType = eMinMaxType.Allocation;
            }
            inventoryBasisMerchandiseType = eMerchandiseType.Undefined;
            if (localMinimumMaximumType == eMinMaxType.Inventory)
            {
                if (MERCH_HN_RID != Include.NoRID)
                {
                    inventoryBasisMerchandiseType = eMerchandiseType.Node;
                }
                else if (MERCH_PH_RID != Include.NoRID)
                {
                    inventoryBasisMerchandiseType = eMerchandiseType.HierarchyLevel;
                }
                else
                {
                    inventoryBasisMerchandiseType = eMerchandiseType.OTSPlanLevel;
                }
            }
            if (MerchUnspecified)
            {
                otsMerchandiseType = eMerchandiseType.Undefined;
            }
            else if (OTSPlanRID != Include.NoRID)
            {
                otsMerchandiseType = eMerchandiseType.Node;
            }
            else if (OTSPlanPHL != Include.NoRID)
            {
                otsMerchandiseType = eMerchandiseType.HierarchyLevel;
            }
            else
            {
                otsMerchandiseType = eMerchandiseType.OTSPlanLevel;
            }

            if (OnHandUnspecified)
            {
                otsOnHandType = eMerchandiseType.Undefined;
            }
            else if (OTSOnHandRID != Include.NoRID)
            {
                otsOnHandType = eMerchandiseType.Node;
            }
            else if (OTSOnHandPHL != Include.NoRID)
            {
                otsOnHandType = eMerchandiseType.HierarchyLevel;
            }
            else
            {
                otsOnHandType = eMerchandiseType.OTSPlanLevel;
            }

            if (!UseStoreGradeDefault)
            {
                storeGradeWeekCount = _mao.Grade_Week_Count;
            }

            if (!UsePctNeedDefault)
            {
                percentNeedLimit = _mao.Percent_Need_Limit;
            }

            if (ReserveQty != Include.UndefinedReserve)
            {
                if (ReserveQty > 0)
                {
                    reserve = ReserveQty;
                }
            }

            if (ReserveAsBulk > 0)
            {
                reserveAsBulk = ReserveAsBulk;
            }

            if (ReserveAsPacks > 0)
            {
                reserveAsPacks = ReserveAsPacks;
            }

            if (!UseFactorPctDefault)
            {
                onHandFactor = OTSPlanFactorPercent;
            }

            if (otsMerchandiseType != eMerchandiseType.Undefined)
            {
                merchandise = GetName.GetLevelKeyValuePair(
                    merchandiseType: otsMerchandiseType,
                    nodeRID: OTSPlanRID,
                    merchPhRID: OTSPlanPHL,
                    merchPhlSequence: OTSPlanPHLSeq,
                    SAB: SAB
                    );
                merchandiseHierarchy = new KeyValuePair<int, int>(OTSPlanPHL, OTSPlanPHLSeq);
            }

            if (otsOnHandType != eMerchandiseType.Undefined)
            {
                onHandMerchandise = GetName.GetLevelKeyValuePair(
                    merchandiseType: otsOnHandType,
                    nodeRID: OTSOnHandRID,
                    merchPhRID: OTSOnHandPHL,
                    merchPhlSequence: OTSOnHandPHLSeq,
                    SAB: SAB
                    );
                onHandMerchandiseHierarchy = new KeyValuePair<int, int>(OTSOnHandPHL, OTSOnHandPHLSeq);
            }

            // Windows code uses SG_RID to set field and not CapacityStoreGroupRID
            if (SG_RID > 0)
            {
                capacityAttributeKey = SG_RID;
            }
            else
            {
                capacityAttributeKey = SAB.ClientServerSession.GlobalOptions.AllocationStoreGroupRID;
            }

            if (GradeStoreGroupRID > 0)
            {
                _storeGradesAttributeKey = GradeStoreGroupRID;
            }
            else
            {
                _storeGradesAttributeKey = SAB.ClientServerSession.GlobalOptions.AllocationStoreGroupRID;
            }

            // get key of first set in attribute
            if (_storeGradesAttributeSetKey == Include.NoRID)
            {
                attributeSetList = StoreMgmt.StoreGroup_GetLevelListViewList(_storeGradesAttributeKey);
                if (attributeSetList.Count > 0)
                {
                    _storeGradesAttributeSetKey = attributeSetList[0].Key;
                }
            }

            if (_VSWAttributeKey == Include.NoRID)
            {
                _VSWAttributeKey = SAB.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID;
            }

            // get key of first set in attribute
            if (_VSWAttributeSetKey == Include.NoRID)
            {
                attributeSetList = StoreMgmt.StoreGroup_GetLevelListViewList(_VSWAttributeKey);
                if (attributeSetList.Count > 0)
                {
                    _VSWAttributeSetKey = attributeSetList[0].Key;
                }
            }

            if ( _mao.All_Color_Multiple > 1)
            {
                colorMultiple = _mao.All_Color_Multiple;
            }

            if (_mao.All_Size_Multiple > 1)
            {
                sizeMultiple = _mao.All_Size_Multiple;
            }

            if (!_allocationCriteria.UseAllColorsMinDefault)
            {
                allColorMinimum = _mao.All_Color_Minimum;
            }

            if (!_allocationCriteria.UseAllColorsMaxDefault)
            {
                allColorMaximum = _mao.All_Color_Maximum;
            }

            ROMethodAllocationOverrideProperties method = new ROMethodAllocationOverrideProperties(
                method: GetName.GetMethod(method: this),
                description: Method_Description,
                userKey: User_RID,
                storeGradeWeekCount: storeGradeWeekCount,
                percentNeedLimit: percentNeedLimit,
                exceedMaxInd: Include.ConvertCharToBool(_mao.Exceed_Maximums_Ind),
                reserve: reserve,
                percentInd: Include.ConvertCharToBool(_mao.Percent_Ind),
                reserveAsBulk: reserveAsBulk,
                reserveAsPacks: reserveAsPacks,
                merchandiseType: otsMerchandiseType,
                merchandise: merchandise,
                merchandiseHierarchy: merchandiseHierarchy,
                onHandMerchandiseType: otsOnHandType,
                onHandMerchandise: onHandMerchandise,
                onHandMerchandiseHierarchy: onHandMerchandiseHierarchy,
                onHandFactor: onHandFactor,
                colorMult: colorMultiple,
                sizeMult: sizeMultiple,
                allColorMin: allColorMinimum,
                allColorMax: allColorMaximum,
                capacityAttribute: GetName.GetAttributeName(key: capacityAttributeKey),
                exceedCapacity: Include.ConvertCharToBool(_mao.Exceed_Capacity_Ind),
                storeGradesAttribute: GetName.GetAttributeName(key: _storeGradesAttributeKey),
                storeGradesAttributeSet: GetName.GetAttributeSetName(key: _storeGradesAttributeSetKey),
                inventoryIndicator: EnumTools.VerifyEnumValue(localMinimumMaximumType),
                inventoryBasisMerchType: EnumTools.VerifyEnumValue(inventoryBasisMerchandiseType),
                inventoryBasisMerchandise: GetName.GetLevelKeyValuePair(merchandiseType: EnumTools.VerifyEnumValue(inventoryBasisMerchandiseType), 
                                                                      nodeRID: _mao.IB_MERCH_HN_RID,
                                                                      merchPhRID: _mao.IB_MERCH_PH_RID,
                                                                      merchPhlSequence: _mao.IB_MERCH_PHL_SEQ,
                                                                      SAB: SAB),

                inventoryBasisMerchandiseHierarchy: new KeyValuePair<int, int>(_mao.IB_MERCH_PH_RID, _mao.IB_MERCH_PHL_SEQ),
                vswAttribute: GetName.GetAttributeName(key: _VSWAttributeKey),
                doNotApplyVSW: _applyVSW,   // the panel needs the value to be flip-flopped - the db stores "applyVSW" but the panel displays "DoNotApplyVSW"
                storeGradeValues: null,
                capacity: new System.Collections.Generic.List<ROMethodOverrideCapacityProperties>(),
                colorMinMax: new System.Collections.Generic.List<ROMethodOverrideColorProperties>(),
                packRounding: new System.Collections.Generic.List<ROMethodOverridePackRoundingProperties>(),
                vswAttributeSetValues: null,
                isTemplate: Template_IND,
                vswAttributeSet: GetName.GetAttributeSetName(key: _VSWAttributeSetKey)
                );

            method.HierarchyLevels = BuildHierarchyLevels();

            if (_applyVSW == false)
            {
                method.DoNotApplyVSW = true;
            }
            else
            {
                method.DoNotApplyVSW = false;
            }

            // build initial store grade list from database 
            if (_storeGradeList == null)
            {
                _storeGradeList = new StoreGradeList(eProfileType.StoreGrade);
                foreach (DataRow row in _dsOverRide.Tables["GradeBoundary"].Rows)
                {
                    string gradeCode = row["GradeCode"].ToString();
                    int boundary = int.Parse(row["Boundary"].ToString());
                    StoreGradeProfile gradeProf = new StoreGradeProfile(boundary);
                    gradeProf.StoreGrade = gradeCode;
                    gradeProf.Boundary = boundary;
                    _storeGradeList.Add(gradeProf);
                }
            }

            if (_populateStoreGrades)
            {
                StoreGrades_InitialPopulate(
                    storeGradesMerchandiseKey: _storeGradesMerchandiseKey,
                    storeGradesAttributeKey: _storeGradesAttributeKey,
                    resetGrid: _storeGroupAttributeChanged
                    );
                _populateStoreGrades = false;
                _storeGroupAttributeChanged = false;
            }

            if (_storeGradesMerchandiseKey != Include.NoRID)
            {
                method.StoreGradesMerchandise = GetName.GetMerchandiseName(nodeRID: _storeGradesMerchandiseKey,
                    SAB: SAB);
                _storeGradesMerchandiseKey = Include.NoRID;
            }

            ROAttributeSetAllocationStoreGrade myAttributeSet = null;
            DataTable dtStoreGrades = _dsOverRide.Tables["StoreGrades"];
            // Insure the rows come out in the expected order
            dtStoreGrades.DefaultView.Sort = "RowPosition ASC";

            // Loop through each set
            foreach (DataRow dr in dtStoreGrades.Rows)
            {
                int sglRID = Convert.ToInt32(dr["SGLRID"], CultureInfo.CurrentUICulture);

                if (sglRID != _storeGradesAttributeSetKey)
                {
                    continue;
                }

                if (myAttributeSet == null
                    || sglRID != myAttributeSet.AttributeSet.Key)
                {
                    // Create store grade instance for attribute
                    myAttributeSet = new ROAttributeSetAllocationStoreGrade();
                    myAttributeSet.AttributeSet = GetName.GetAttributeSetName(key: sglRID);
                    method.StoreGradeValues = myAttributeSet;
                }

                ROAllocationStoreGrade storeGrades = new ROAllocationStoreGrade();
                storeGrades.StoreGrade = new KeyValuePair<int, string>(Convert.ToInt32(dr["Boundary"], CultureInfo.CurrentUICulture), Convert.ToString(dr["Grade"], CultureInfo.CurrentUICulture));

                if (dr["Allocation Min"] != System.DBNull.Value)
                {
                    storeGrades.Minimum = Convert.ToInt32(dr["Allocation Min"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    storeGrades.Minimum = null;
                }

                if (dr["Allocation Max"] != System.DBNull.Value)
                {
                    storeGrades.Maximum = Convert.ToInt32(dr["Allocation Max"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    storeGrades.Maximum = null;
                }

                if (dr["Min Ad"] != System.DBNull.Value)
                {
                    storeGrades.AdMinimum = Convert.ToInt32(dr["Min Ad"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    storeGrades.AdMinimum = null;
                }

                if (dr["Color Min"] != System.DBNull.Value)
                {
                    storeGrades.ColorMinimum = Convert.ToInt32(dr["Color Min"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    storeGrades.ColorMinimum = null;
                }

                if (dr["Color Max"] != System.DBNull.Value)
                {
                    storeGrades.ColorMaximum = Convert.ToInt32(dr["Color Max"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    storeGrades.ColorMaximum = null;
                }

                if (dr["Ship Up To"] != System.DBNull.Value)
                {
                    storeGrades.ShipUpTo = Convert.ToInt32(dr["Ship Up To"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    storeGrades.ShipUpTo = null;
                }

                myAttributeSet.StoreGrades.Add(storeGrades);
            }

            bool localExceedCapacity;
            double? localExceedByPct;
            DataTable dtCapacity = _dsOverRide.Tables["Capacity"];
            if (dtCapacity.Rows.Count > 0)
            {
                foreach (DataRow dr in dtCapacity.Rows)
                {
                    if (dr["ExceedChar"] != System.DBNull.Value)
                    {
                        localExceedCapacity = Include.ConvertCharToBool(Convert.ToChar(dr["ExceedChar"], CultureInfo.CurrentUICulture));
                    }
                    else
                    {
                        localExceedCapacity = false;
                    }

                    if (dr["Exceed by %"] != System.DBNull.Value)
                    {
                        localExceedByPct = Convert.ToDouble(dr["Exceed by %"], CultureInfo.CurrentUICulture);
                    }
                    else
                    {
                        localExceedByPct = null;
                    }

                    ROMethodOverrideCapacityProperties capacities = new ROMethodOverrideCapacityProperties(
                        attributeSet: GetName.GetAttributeSetName(Convert.ToInt32(dr["SglRID"], CultureInfo.CurrentUICulture)),
                        exceedCapacity: localExceedCapacity,
                        exceedByPct: localExceedByPct
                        );

                    method.Capacity.Add(capacities);
                }
            }
            else if (capacityAttributeKey != Include.NoRID)
            {
                attributeSetList = StoreMgmt.StoreGroup_GetLevelListViewList(capacityAttributeKey);
                foreach (StoreGroupLevelListViewProfile attributeSet in attributeSetList)
                {
                    localExceedCapacity = false;
                    localExceedByPct = null;

                    ROMethodOverrideCapacityProperties capacities = new ROMethodOverrideCapacityProperties(
                        attributeSet: GetName.GetAttributeSetName(attributeSet.Key),
                        exceedCapacity: localExceedCapacity,
                        exceedByPct: localExceedByPct
                        );

                    method.Capacity.Add(capacities);
                }
            }

            DataTable dtColor = _dsOverRide.Tables["Colors"];
            foreach (DataRow dr in dtColor.Rows)
            {
                int? localColorMinimum, localColorMaximum;
                if (dr["Minimum"] != System.DBNull.Value)
                {
                    localColorMinimum = Convert.ToInt32(dr["Minimum"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    localColorMinimum = null;
                }
                if (dr["Maximum"] != System.DBNull.Value)
                {
                    localColorMaximum = Convert.ToInt32(dr["Maximum"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    localColorMaximum = null;
                }

                ROMethodOverrideColorProperties colorMinMax = new ROMethodOverrideColorProperties(
                    colorCode: GetName.GetColor(Convert.ToInt32(dr["Color"], CultureInfo.CurrentUICulture), SAB: SAB),
                    colorMinimum: localColorMinimum,
                    colorMaximum: localColorMaximum
                    );

                method.ColorMinMax.Add(colorMinMax);
            }

            DataTable dtPackRounding = _dsOverRide.Tables["PackRounding"];
            foreach (DataRow row in dtPackRounding.Rows)
            {
                string packText =  Convert.ToString(row["PackText"]); ;
                double? localFirstPack, localNthPack;
                int localPackMultRID = Convert.ToInt32(row["PackMultiple"]);
                if (row["FstPack"] == DBNull.Value)
                {
                    localFirstPack = null;
                }
                else
                {
                    localFirstPack = Convert.ToDouble(row["FstPack"]);
                }
                if (row["NthPack"] == DBNull.Value)
                {
                    localNthPack = null;
                }
                else
                {
                    localNthPack = Convert.ToDouble(row["NthPack"]);
                }

                ROMethodOverridePackRoundingProperties roPackRounding = new ROMethodOverridePackRoundingProperties(
                    packMultiple: localPackMultRID,
                    firstPackPct: localFirstPack,
                    nthPackPct: localNthPack,
                    packName: packText
                    );
                method.PackRounding.Add(roPackRounding);
            }

            int? localMinimumShipQuantity, localMaximumValue;
            double? localPercentPackThreshold;
            string localReservationStore;
            KeyValuePair<int, string> localAttributeSet, localEntry;
            DataTable dataTableVSWSets = IMODataSet.Tables["Sets"];
            ROMethodOverrideVSWAttributeSet VSWAttributeSet;
            DataTable dataTableVSWStores = IMODataSet.Tables["Stores"];
            for (int r = 0; r < dataTableVSWSets.Rows.Count; r++)
            {
                DataRow row = dataTableVSWSets.Rows[r];

                string setID = Convert.ToString(row["SetID"], CultureInfo.CurrentUICulture);

                if (setID != method.VSWAttributeSet.Value)
                {
                    continue;
                }

                string localString = Convert.ToString(row["Min Ship Qty"]);
                if (string.IsNullOrEmpty(localString))
                {
                    localMinimumShipQuantity = null;
                }
                else
                {
                    localMinimumShipQuantity = Convert.ToInt32(localString);
                }
                localString = Convert.ToString(row["Pct Pack Threshold"]);
                if (string.IsNullOrEmpty(localString))
                {
                    localPercentPackThreshold = null;
                }
                else
                {
                    localPercentPackThreshold = Convert.ToDouble(localString);
                }
                localString = Convert.ToString(row["Item Max"]);
                if (string.IsNullOrEmpty(localString))
                {
                    localMaximumValue = null;
                }
                else
                {
                    localMaximumValue = Convert.ToInt32(localString);
                }
                localReservationStore = Convert.ToString(row["Reservation Store"]);

                localAttributeSet = GetName.GetAttributeSetName(key: _VSWAttributeSetKey);
                VSWAttributeSet = new ROMethodOverrideVSWAttributeSet();
                VSWAttributeSet.VSWAttributeSetValues = new ROMethodOverrideVSW(
                    updated: false,
                    entry: localAttributeSet,
                    reservationStore: localReservationStore,
                    minimumShipQuantity: localMinimumShipQuantity,
                    pctPackThreshold: localPercentPackThreshold,
                    itemMaximum: localMaximumValue
                    );

                VSWAttributeSet.VSWAttributeSetValues.MinimumShipQuantity = localMinimumShipQuantity;
                VSWAttributeSet.VSWAttributeSetValues.PctPackThreshold = localPercentPackThreshold;
                VSWAttributeSet.VSWAttributeSetValues.ItemMaximum = localMaximumValue;
                method.VSWAttributeSetValues = VSWAttributeSet;

                for (int s = 0; s < dataTableVSWStores.Rows.Count; s++)
                {
                    DataRow storeRow = dataTableVSWStores.Rows[s];

                    setID = Convert.ToString(storeRow["SetID"], CultureInfo.CurrentUICulture);

                    if (setID != method.VSWAttributeSet.Value)
                    {
                        continue;
                    }

                    localReservationStore = Convert.ToString(storeRow["Reservation Store"]);
                    localString = Convert.ToString(storeRow["Min Ship Qty"]);
                    if (string.IsNullOrEmpty(localString))
                    {
                        localMinimumShipQuantity = null;
                    }
                    else
                    {
                        localMinimumShipQuantity = Convert.ToInt32(localString);
                    }
                    localString = Convert.ToString(storeRow["Pct Pack Threshold"]);
                    if (string.IsNullOrEmpty(localString))
                    {
                        localPercentPackThreshold = null;
                    }
                    else
                    {
                        localPercentPackThreshold = Convert.ToDouble(localString);
                    }
                    localString = Convert.ToString(storeRow["Item Max"]);
                    if (string.IsNullOrEmpty(localString))
                    {
                        localMaximumValue = null;
                    }
                    else
                    {
                        localMaximumValue = Convert.ToInt32(localString);
                    }
                    int storeRID = Convert.ToInt16(storeRow["Store RID"]);
                    localEntry = GetName.GetStoreName(storeRID);
                    bool updated = false;

                    ROMethodOverrideVSW vswStore = new ROMethodOverrideVSW(
                        updated: updated,
                        entry: GetName.GetStoreName(localEntry.Key),
                        reservationStore: localReservationStore,
                        minimumShipQuantity: localMinimumShipQuantity,
                        pctPackThreshold: localPercentPackThreshold,
                        itemMaximum: localMaximumValue
                        );

                    VSWAttributeSet.VSWStoresValues.Add(vswStore);
                }
            }

            return method;
        }

        /// <summary>
		/// Populates store grade values when the Merchandise Node changes
		/// </summary>
		/// <param name="storeGradesMerchandiseKey">The merchandise key to use to retrieve store grades</param>
        /// <param name="storeGradesAttributeKey">The attribute key to use to populate store grades</param>
		private void StoreGrades_InitialPopulate(
            int storeGradesMerchandiseKey,
            int storeGradesAttributeKey,
            bool resetGrid)
        {
            ProfileList attributeSetList;

            try
            {
                int count = 0;
                int? minimumStock, maximumStock, minimumAd, minimumColor, maximumColor, shipUpTo;
                 
                _dsOverRide.Tables["StoreGrades"].Clear();
                _dsOverRide.Tables["StoreGrades"].AcceptChanges();

                if (storeGradesMerchandiseKey != Include.NoRID)
                {
                    _storeGradeList = SAB.HierarchyServerSession.GetStoreGradeList(storeGradesMerchandiseKey, false, true);
                }

                attributeSetList = StoreMgmt.StoreGroup_GetLevelListViewList(storeGradesAttributeKey);

                foreach (StoreGroupLevelListViewProfile storeGroupLevelListViewProfile in attributeSetList)
                {
                    foreach (StoreGradeProfile storeGradeProfile in _storeGradeList)
                    {
                        if (storeGradeProfile.MinStock > Include.Undefined
                            && !resetGrid)
                        {
                            minimumStock = storeGradeProfile.MinStock;
                        }
                        else
                        {
                            minimumStock = null;
                        }
                        if (storeGradeProfile.MaxStock > Include.Undefined
                            && !resetGrid)
                        {
                            maximumStock = storeGradeProfile.MaxStock;
                        }
                        else
                        {
                            maximumStock = null;
                        }
                        if (storeGradeProfile.MinAd > Include.Undefined
                            && !resetGrid)
                        {
                            minimumAd = storeGradeProfile.MinAd;
                        }
                        else
                        {
                            minimumAd = null;
                        }
                        if (storeGradeProfile.MinColor > Include.Undefined
                            && !resetGrid)
                        {
                            minimumColor = storeGradeProfile.MinColor;
                        }
                        else
                        {
                            minimumColor = null;
                        }
                        if (storeGradeProfile.MaxColor > Include.Undefined
                            && !resetGrid)
                        {
                            maximumColor = storeGradeProfile.MaxColor;
                        }
                        else
                        {
                            maximumColor = null;
                        }
                        if (storeGradeProfile.ShipUpTo > Include.Undefined
                            && !resetGrid)
                        {
                            shipUpTo = storeGradeProfile.ShipUpTo;
                        }
                        else
                        {
                            shipUpTo = null;
                        }

                        _dsOverRide.Tables["StoreGrades"].Rows.Add(new object[] 
                        {
                            count,
                            storeGroupLevelListViewProfile.Key,
                            storeGradeProfile.Boundary,
                            storeGradeProfile.StoreGrade,
                            minimumStock,
                            maximumStock,
                            minimumAd,
                            minimumColor,
                            maximumColor,
                            shipUpTo
                        });

                        ++count;
                    }
                }

                _dsOverRide.Tables["StoreGrades"].DefaultView.RowFilter = null;

            }
            catch
            {
                throw;
            }
        }

        override public bool MethodSetData(ROMethodProperties methodProperties, ref string message, bool processingApply)
        {
            ROMethodAllocationOverrideProperties roMethodAllocationOverrideProperties = (ROMethodAllocationOverrideProperties)methodProperties;
            try
            {
                if (roMethodAllocationOverrideProperties.StoreGradeWeekCountIsSet)
                {
                    _allocationCriteria.GradeWeekCount = (int)roMethodAllocationOverrideProperties.StoreGradeWeekCount;
                    _mao.UseStoreGradeDefault = false;
                    _allocationCriteria.UseStoreGradeDefault = false;
                }
                else
                {
                    _mao.UseStoreGradeDefault = true;
                    _allocationCriteria.UseStoreGradeDefault = true;
                }

                _allocationCriteria.ExceedMaximums = roMethodAllocationOverrideProperties.ExceedMaxInd;
                if (roMethodAllocationOverrideProperties.PercentNeedLimitIsSet)
                {
                    _allocationCriteria.PercentNeedLimit = (double)roMethodAllocationOverrideProperties.PercentNeedLimit;
                    _mao.UsePctNeedDefault = false;
                    _allocationCriteria.UsePctNeedDefault = false;
                }
                else
                {
                    _mao.UsePctNeedDefault = true;
                    _allocationCriteria.UsePctNeedDefault = true;
                }
                if (roMethodAllocationOverrideProperties.ReserveIsSet)
                {
                    _allocationCriteria.ReserveQty = (double)roMethodAllocationOverrideProperties.Reserve;
                }
                else
                {
                    _mao.Reserve = 0;
                    _allocationCriteria.ReserveQty = 0;
                }
                _allocationCriteria.ReserveIsPercent = roMethodAllocationOverrideProperties.PercentInd;
                OTSPlanRID = Include.NoRID;
                OTSPlanPHL = Include.NoRID;
                OTSPlanPHLSeq = 0;
                if (roMethodAllocationOverrideProperties.MerchandiseType == eMerchandiseType.Node)
                {
                    OTSPlanRID = roMethodAllocationOverrideProperties.Merchandise.Key;
                }
                else if (roMethodAllocationOverrideProperties.MerchandiseType == eMerchandiseType.HierarchyLevel
                    || roMethodAllocationOverrideProperties.MerchandiseType == eMerchandiseType.LevelOffset)
                {
                    OTSPlanPHL = roMethodAllocationOverrideProperties.MerchandiseHierarchy.Key;
                    OTSPlanPHLSeq = roMethodAllocationOverrideProperties.MerchandiseHierarchy.Value;
                }
                OTSOnHandRID = Include.NoRID;
                OTSOnHandPHL = Include.NoRID;
                OTSOnHandPHLSeq = 0;
                if (roMethodAllocationOverrideProperties.OnHandMerchandiseType == eMerchandiseType.Node)
                {
                    OTSOnHandRID = roMethodAllocationOverrideProperties.OnHandMerchandise.Key;
                }
                else if (roMethodAllocationOverrideProperties.OnHandMerchandiseType == eMerchandiseType.HierarchyLevel
                    || roMethodAllocationOverrideProperties.OnHandMerchandiseType == eMerchandiseType.LevelOffset)
                {
                    OTSOnHandPHL = roMethodAllocationOverrideProperties.OnHandMerchandiseHierarchy.Key;
                    OTSOnHandPHLSeq = roMethodAllocationOverrideProperties.OnHandMerchandiseHierarchy.Value;
                }
                if (roMethodAllocationOverrideProperties.OnHandFactorIsSet)
                {
                    OTSPlanFactorPercent = (double)roMethodAllocationOverrideProperties.OnHandFactor;
                    UseFactorPctDefault = false;
                    _allocationCriteria.UseFactorPctDefault = false;
                }
                else
                {
                    UseFactorPctDefault = true;
                    _allocationCriteria.UseFactorPctDefault = true;
                }
                if (roMethodAllocationOverrideProperties.ColorMultIsSet
                    && (int)roMethodAllocationOverrideProperties.ColorMult > 1)
                {
                    _allocationCriteria.AllColorMultiple = (int)roMethodAllocationOverrideProperties.ColorMult;
                }
                else
                {
                    _allocationCriteria.AllColorMultiple = 1;
                }
                if (roMethodAllocationOverrideProperties.SizeMultIsSet
                    && (int)roMethodAllocationOverrideProperties.SizeMult > 1)
                {
                    _allocationCriteria.AllSizeMultiple = (int)roMethodAllocationOverrideProperties.SizeMult;
                }
                else
                {
                    _allocationCriteria.AllSizeMultiple = 1;
                }
                if (roMethodAllocationOverrideProperties.AllColorMinIsSet)
                {
                    AllColorMinimum = (int)roMethodAllocationOverrideProperties.AllColorMin;
                }
                else
                {
                    UseAllColorsMinDefault = true;
                    _allocationCriteria.UseAllColorsMinDefault = true;
                }
                if (roMethodAllocationOverrideProperties.AllColorMaxIsSet)
                {
                    AllColorMaximum = (int)roMethodAllocationOverrideProperties.AllColorMax;
                }
                else
                {
                    UseAllColorsMaxDefault = true;
                    _allocationCriteria.UseAllColorsMaxDefault = true;
                }
                // Both fields are set to the same capacity attribute key
                SG_RID = roMethodAllocationOverrideProperties.CapacityAttribute.Key;
                CapacityStoreGroupRID = roMethodAllocationOverrideProperties.CapacityAttribute.Key;
                GradeStoreGroupRID = roMethodAllocationOverrideProperties.StoreGradesAttribute.Key;
                _allocationCriteria.ExceedCapacity = roMethodAllocationOverrideProperties.ExceedCapacity;
                _applyVSW = roMethodAllocationOverrideProperties.DoNotApplyVSW;

                if (roMethodAllocationOverrideProperties.MerchandiseType == eMerchandiseType.Undefined)
                {
                    _allocationCriteria.MerchUnspecified = true;
                }
                else
                {
                    _allocationCriteria.MerchUnspecified = false;
                }
                if (roMethodAllocationOverrideProperties.OnHandMerchandiseType == eMerchandiseType.Undefined)
                {
                    _allocationCriteria.OnHandUnspecified = true;
                }
                else
                {
                    _allocationCriteria.OnHandUnspecified = false;
                }
                if (roMethodAllocationOverrideProperties.ReserveAsBulkIsSet)
                {
                    _allocationCriteria.ReserveAsBulk = (double)roMethodAllocationOverrideProperties.ReserveAsBulk;
                }
                else
                {
                    _allocationCriteria.ReserveAsBulk = 0;
                }
                if (roMethodAllocationOverrideProperties.ReserveAsPacksIsSet)
                {
                    _allocationCriteria.ReserveAsPacks = (double)roMethodAllocationOverrideProperties.ReserveAsPacks;
                }
                else
                {
                    _allocationCriteria.ReserveAsPacks = 0;
                }
                //InventoryInd = roMethodAllocationOverrideProperties.InventoryIndicator;

                InventoryInd = 'A';
                MERCH_HN_RID = Include.NoRID;
                MERCH_PH_RID = Include.NoRID;
                MERCH_PHL_SEQ = 0;
                if (roMethodAllocationOverrideProperties.InventoryIndicator == eMinMaxType.Inventory)
                {
                    InventoryInd = 'I';
                    if (roMethodAllocationOverrideProperties.InventoryBasisMerchType == eMerchandiseType.Node)
                    {
                        MERCH_HN_RID = roMethodAllocationOverrideProperties.InventoryBasisMerchandise.Key;
                    }
                    else if (roMethodAllocationOverrideProperties.InventoryBasisMerchType == eMerchandiseType.HierarchyLevel
                        || roMethodAllocationOverrideProperties.InventoryBasisMerchType == eMerchandiseType.LevelOffset)
                    {
                        MERCH_PH_RID = roMethodAllocationOverrideProperties.InventoryBasisMerchandiseHierarchy.Key;
                        MERCH_PHL_SEQ = roMethodAllocationOverrideProperties.InventoryBasisMerchandiseHierarchy.Value;
                    }
                }

                if (roMethodAllocationOverrideProperties.OnHandFactorIsSet)
                {
                    OTSPlanFactorPercent = (double)roMethodAllocationOverrideProperties.OnHandFactor;
                    UseFactorPctDefault = false;
                    _allocationCriteria.UseFactorPctDefault = false;
                }
                else
                {
                    UseFactorPctDefault = true;
                    _allocationCriteria.UseFactorPctDefault = true;
                }

                // color
                // Building a dataTable called "Colors" to be placed in _dsOverRide
                _dsOverRide.Tables["Colors"].Rows.Clear();
                int i = 0;
                int? localColorMinimum, localColorMaximum;

                foreach (ROMethodOverrideColorProperties colorProperty in roMethodAllocationOverrideProperties.ColorMinMax)
                {
                    if (colorProperty.ColorMinimumIsSet)
                    {
                        localColorMinimum = colorProperty.ColorMinimum;
                    }
                    else
                    {
                        localColorMinimum = null;
                    }
                    if (colorProperty.ColorMaximumIsSet)
                    {
                        localColorMaximum = colorProperty.ColorMaximum;
                    }
                    else
                    {
                        localColorMaximum = null;
                    }

                    _dsOverRide.Tables["Colors"].Rows.Add(new object[] { i, colorProperty.ColorCode.Key, localColorMinimum, localColorMaximum });

                    i += 1;
                }

                // capacity
                _dsOverRide.Tables["Capacity"].Rows.Clear();

                // check for mismatch between attribute and sets during a save only
				// if attributes do not match, update capacity attribute from set entries attribute
                if (!processingApply
                    && roMethodAllocationOverrideProperties.Capacity.Count > 0)
                {
                    // get atttribute set key from first entry
                    int attributeSetKey = roMethodAllocationOverrideProperties.Capacity[0].AttributeSet.Key;
                    int capacityAttributeKey = StoreMgmt.StoreGroupLevel_GetGroup(attributeSetKey);
                    
                    if (capacityAttributeKey != _allocationCriteria.CapacityStoreGroupRID)
                    {
                        CapacityStoreGroupRID = capacityAttributeKey;
                    }
                }

                // Capacity attribute is held in multiple fields
                SG_RID = CapacityStoreGroupRID;

                foreach (ROMethodOverrideCapacityProperties capacityProperty in roMethodAllocationOverrideProperties.Capacity)
                {
                    if (!capacityProperty.ExceedByPctIsSet)
                    {
                        _dsOverRide.Tables["Capacity"].Rows.Add(new object[] { capacityProperty.AttributeSet.Key, capacityProperty.AttributeSet.Value, 0, null });
                    }
                    else
                    {
                        _dsOverRide.Tables["Capacity"].Rows.Add(new object[] { capacityProperty.AttributeSet.Key, capacityProperty.AttributeSet.Value, 1, capacityProperty.ExceedByPct });
                    }
                }

                // pack rounding
                _dsOverRide.Tables["PackRounding"].Rows.Clear();
                foreach (ROMethodOverridePackRoundingProperties packProperty in roMethodAllocationOverrideProperties.PackRounding)
                {
                    _dsOverRide.Tables["PackRounding"].Rows.Add(
                        new object[] {
                            packProperty.PackName,
                            packProperty.FirstPackPct,
                            packProperty.NthPackPct,
                            packProperty.PackMultiple
                        });
                }

                // store grade
                // remove rows for the set
                //_dsOverRide.Tables["StoreGrades"].Rows.Clear();
                if (_storeGradesAttributeSetKey != Include.NoRID)
                {
                    string selectString = "SGLRID =" + _storeGradesAttributeSetKey;
                    DataRow[] detailDataRows = _dsOverRide.Tables["StoreGrades"].Select(selectString);
                    foreach (var detailDataRow in detailDataRows)
                    {
                        detailDataRow.Delete();
                    }
                    _dsOverRide.Tables["StoreGrades"].AcceptChanges();
                }

                // If merchandise key is provided, set flag to populate store grades during get
                // If key is -1 store grades will be cleared
                if (roMethodAllocationOverrideProperties.StoreGradesMerchandiseIsSet
                    && roMethodAllocationOverrideProperties.StoreGradesMerchandise.Key != _storeGradesMerchandiseKey)
                {
                    _storeGradesMerchandiseKey = roMethodAllocationOverrideProperties.StoreGradesMerchandise.Key;
                    _populateStoreGrades = true;
                }
                else if (_storeGradesAttributeKey != roMethodAllocationOverrideProperties.StoreGradesAttribute.Key)
                {
                    _storeGradesAttributeKey = roMethodAllocationOverrideProperties.StoreGradesAttribute.Key;
                    _populateStoreGrades = true;
                    _storeGroupAttributeChanged = true;
                }

                i = 0;

                //foreach (ROAttributeSetStoreGrade storeGrade in roMethodAllocationOverrideProperties.StoreGradeValues)
                ROAttributeSetAllocationStoreGrade storeGrade = roMethodAllocationOverrideProperties.StoreGradeValues;
                if (storeGrade != null
                    && storeGrade.StoreGrades != null)
                {
                    foreach (ROAllocationStoreGrade setStoreGrades in storeGrade.StoreGrades)
                    {
                        if (!setStoreGrades.MinimumIsSet 
                            && !setStoreGrades.MaximumIsSet 
                            && !setStoreGrades.AdMinimumIsSet 
                            && !setStoreGrades.ColorMinimumIsSet 
                            && !setStoreGrades.ColorMaximumIsSet
                            )
                        {
                            _dsOverRide.Tables["StoreGrades"].Rows.Add(new object[] {
                                i,
                                storeGrade.AttributeSet.Key,
                                setStoreGrades.StoreGrade.Key,
                                setStoreGrades.StoreGrade.Value,
                                null,
                                null,
                                null,
                                null,
                                null,
                                null
                            });
                        }
                        else
                        {
                            _dsOverRide.Tables["StoreGrades"].Rows.Add(new object[] {
                                i,
                                storeGrade.AttributeSet.Key,
                                setStoreGrades.StoreGrade.Key,
                                setStoreGrades.StoreGrade.Value,
                                ((setStoreGrades.MinimumIsSet) ? setStoreGrades.Minimum : null),
                                ((setStoreGrades.MaximumIsSet) ? setStoreGrades.Maximum : null),
                                ((setStoreGrades.AdMinimumIsSet) ? setStoreGrades.AdMinimum : null),
                                ((setStoreGrades.ColorMinimumIsSet) ? setStoreGrades.ColorMinimum : null),
                                ((setStoreGrades.ColorMaximumIsSet) ? setStoreGrades.ColorMaximum : null),
                                ((setStoreGrades.ShipUpToIsSet) ? setStoreGrades.ShipUpTo : null)
                            });  
                        }
                        i += 1;
                    }

                }

                // vsw 
                // get ID associated with the attribute set key
                KeyValuePair<int, string> VSWAttributeSet = GetName.GetAttributeSetName(
                     key: roMethodAllocationOverrideProperties.VSWAttributeSetValues.VSWAttributeSetValues.Entry.Key
                     );
                string localSetID = VSWAttributeSet.Value;

                // remove rows for the set
                if (roMethodAllocationOverrideProperties.VSWAttributeSetValues != null
                    && roMethodAllocationOverrideProperties.VSWAttributeSetValues.VSWAttributeSetValues.EntryIsSet )
                {
                    string selectString = "SetID = '" + localSetID + "'";
                    DataRow[] detailDataRows = IMODataSet.Tables["Sets"].Select(selectString);
                    foreach (var detailDataRow in detailDataRows)
                    {
                        detailDataRow.Delete();
                    }
                    IMODataSet.Tables["Sets"].AcceptChanges();

                   detailDataRows = IMODataSet.Tables["Stores"].Select(selectString);
                    foreach (var detailDataRow in detailDataRows)
                    {
                        detailDataRow.Delete();
                    }
                    IMODataSet.Tables["Stores"].AcceptChanges();
                }

                IMODataSet.Tables["Sets"].Rows.Add(new object[] { localSetID, string.Empty, string.Empty, string.Empty, string.Empty });

                foreach (ROMethodOverrideVSW vswStores in roMethodAllocationOverrideProperties.VSWAttributeSetValues.VSWStoresValues)
                {
                    if (vswStores.MinimumShipQuantity == System.Int32.MinValue && vswStores.ItemMaximum == System.Int32.MaxValue && vswStores.PctPackThreshold == System.Double.MinValue)
                    {
                        IMODataSet.Tables["Stores"].Rows.Add(new object[] { localSetID, false, vswStores.Entry.Key, vswStores.Entry.Value, vswStores.ReservationStore, string.Empty, string.Empty, string.Empty });
                    }
                    else
                    {
                        IMODataSet.Tables["Stores"].Rows.Add(new object[] { localSetID, false, vswStores.Entry.Key, vswStores.Entry.Value, vswStores.ReservationStore, 
                                ((vswStores.MinimumShipQuantity == int.MinValue) ? (int?) null : vswStores.MinimumShipQuantity),
                                ((vswStores.PctPackThreshold == double.MinValue) ? (double?) null : vswStores.PctPackThreshold),
                                ((vswStores.ItemMaximum == int.MaxValue) ? (int?) null : vswStores.ItemMaximum)
                                });
                    }
                }

                // the panel value needs to be flip-flopped - the db stores "applyVSW" but the panel displays "DoNotApplyVSW"
                if (roMethodAllocationOverrideProperties.DoNotApplyVSW == true)
                {
                    _applyVSW = false;
                }
                else
                {
                    _applyVSW = true;
                }

                // Save selected attribute set to populate duing get.
                if (roMethodAllocationOverrideProperties.StoreGradesAttributeSetIsSet)
                {
                    _storeGradesAttributeSetKey = roMethodAllocationOverrideProperties.StoreGradesAttributeSet.Key;
                    // check to determine if attribute set is part of new attribute
                    // if not, set so will use first set in the attribute
                    if (_storeGroupAttributeChanged)
                    {
                        ProfileList attributeSetList = StoreMgmt.StoreGroup_GetLevelListViewList(_storeGradesAttributeKey);
                        if (attributeSetList.FindKey(aKey: _storeGradesAttributeSetKey) == null)
                        {
                            _storeGradesAttributeSetKey = Include.NoRID;
                        }
                    }
                }

                // If VSW attribute is changed, rebuild VSW dataset with new attribute
                if (roMethodAllocationOverrideProperties.VSWAttributeIsSet
                    && roMethodAllocationOverrideProperties.VSWAttribute.Key != _VSWAttributeKey)
                {
                    IMOGroupLevelList = StoreMgmt.StoreGroup_GetLevelListViewList(roMethodAllocationOverrideProperties.VSWAttribute.Key, true);
                    Reservation_Populate(
                        nodeRID: Include.NoRID, 
                        aAttributeChanged: true);
                    _VSWAttributeKey = roMethodAllocationOverrideProperties.VSWAttribute.Key;
                }

                if (roMethodAllocationOverrideProperties.VSWAttributeSetIsSet)
                {
                    _VSWAttributeSetKey = roMethodAllocationOverrideProperties.VSWAttributeSet.Key;
                    // check to determine if attribute set is part of new attribute
                    // if not, set so will use first set in the attribute
                    if (roMethodAllocationOverrideProperties.VSWAttributeIsSet)
                    {
                        ProfileList attributeSetList = StoreMgmt.StoreGroup_GetLevelListViewList(_VSWAttributeKey);
                        if (attributeSetList.FindKey(aKey: _VSWAttributeSetKey) == null)
                        {
                            _VSWAttributeSetKey = Include.NoRID;
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }
            //throw new NotImplementedException("MethodSaveData is not implemented");
        }

        override public ROMethodProperties MethodCopyData()
        {
            throw new NotImplementedException("MethodCopyData is not implemented");
        }
        // END RO-642 - RDewey
    }
}
