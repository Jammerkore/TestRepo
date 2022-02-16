using System;
using System.Collections;
using System.Collections.Generic;
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
	/// This is the business Group Allocation Method.  It inherits from the business AllocationBaseMethod.
	/// </remarks>
	public class GroupAllocationMethod:AllocationBaseMethod
	{
		//=======
		// FIELDS
		//=======
		private GroupAllocationMethodData _groupAllocationData;
        private DataSet _dsGroupAllocation;

        private AllocationCriteria _allocationCriteria;
	
		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates an instance of the AllocationOverrideMethod.
		/// </summary>
		/// <param name="aSAB">Session Address Block.</param>
		/// <param name="aMethodRID">RID that identifies the Method.</param>
        public GroupAllocationMethod(SessionAddressBlock aSAB, int aMethodRID)
            : base(aSAB, aMethodRID, eMethodType.GroupAllocation, eProfileType.MethodGroupAllocation)
        {
            _groupAllocationData = new GroupAllocationMethodData(aMethodRID);
            // begin TT#488 - Jellis - Group Allocation
            _allocationCriteria = new AllocationCriteria(aSAB);
            if (base.Filled)
            {
                _groupAllocationData.PopulateGroupAllocation(aMethodRID);
                _allocationCriteria.BeginCdrRID = _groupAllocationData.BeginCdrRid;
                _allocationCriteria.ShipToCdrRID = _groupAllocationData.ShipToCdrRid;
                _allocationCriteria.UseStoreGradeDefault = _groupAllocationData.UseStoreGradeDefault;
                if (!_allocationCriteria.UseStoreGradeDefault)
                {
                    _allocationCriteria.GradeWeekCount = _groupAllocationData.Grade_Week_Count;
                }
                _allocationCriteria.ExceedMaximums = Include.ConvertCharToBool(_groupAllocationData.Exceed_Maximums_Ind);
                _allocationCriteria.UsePctNeedDefault = _groupAllocationData.UsePctNeedDefault;
                if (!_allocationCriteria.UsePctNeedDefault)
                {
                    _allocationCriteria.PercentNeedLimit = _groupAllocationData.Percent_Need_Limit;
                }
                _allocationCriteria.ReserveIsPercent = Include.ConvertCharToBool(_groupAllocationData.Percent_Ind);
                _allocationCriteria.ReserveQty = _groupAllocationData.Reserve;
                _allocationCriteria.ReserveAsPacks = _groupAllocationData.ReserveAsPacks;
                _allocationCriteria.ReserveAsBulk = _groupAllocationData.ReserveAsBulk;
                _allocationCriteria.MerchUnspecified = _groupAllocationData.Merch_Plan_Unspecified;
                _allocationCriteria.OTSPlanRID = _groupAllocationData.Merch_Plan_HN_RID;
                _allocationCriteria.OTSPlanPHL = _groupAllocationData.Merch_Plan_PH_RID;
                _allocationCriteria.OTSPlanPHLSeq = _groupAllocationData.Merch_Plan_PHL_SEQ;
                _allocationCriteria.OnHandUnspecified = _groupAllocationData.Merch_OnHand_Unspecified;
                _allocationCriteria.OTSOnHandRID = _groupAllocationData.Merch_OnHand_HN_RID;
                _allocationCriteria.OTSOnHandPHL = _groupAllocationData.Merch_OnHand_PH_RID;
                _allocationCriteria.OTSOnHandPHLSeq = _groupAllocationData.Merch_OnHand_PHL_SEQ;
                _allocationCriteria.UseFactorPctDefault = _groupAllocationData.UseFactorPctDefault;
                if (!_allocationCriteria.UseFactorPctDefault)
                {
                    _allocationCriteria.OTSPlanFactorPercent = _groupAllocationData.Plan_Factor_Percent;
                }
                _allocationCriteria.CapacityStoreGroupRID = _groupAllocationData.Store_Group_RID;
                _allocationCriteria.GradeStoreGroupRID = _groupAllocationData.Tab_Store_Group_RID;
                _allocationCriteria.InventoryInd = _groupAllocationData.Inventory_Ind;
                _allocationCriteria.Inventory_MERCH_HN_RID = _groupAllocationData.IB_MERCH_HN_RID;
                _allocationCriteria.Inventory_MERCH_PH_RID = _groupAllocationData.IB_MERCH_PH_RID;
                _allocationCriteria.Inventory_MERCH_PHL_SEQ = _groupAllocationData.IB_MERCH_PHL_SEQ;
                _allocationCriteria.LineItemMinOverrideInd = _groupAllocationData.LineItemMinOverrideInd;
                _allocationCriteria.LineItemMinOverride = _groupAllocationData.LineItemMinOverride;
                // BEGIN TT#1131-MD - AGallagher - Add Header Min/Max Options and Inventory Basis
                _allocationCriteria.HdrInventoryInd = _groupAllocationData.HDRInventory_Ind;
                _allocationCriteria.HdrInventory_MERCH_HN_RID = _groupAllocationData.HDRIB_MERCH_HN_RID;
                _allocationCriteria.HdrInventory_MERCH_PH_RID = _groupAllocationData.HDRIB_MERCH_PH_RID;
                _allocationCriteria.HdrInventory_MERCH_PHL_SEQ = _groupAllocationData.HDRIB_MERCH_PHL_SEQ;
                // END TT#1131-MD - AGallagher - Add Header Min/Max Options and Inventory Basis
            }
            _dsGroupAllocation = _groupAllocationData.GetGroupAllocationChildData();
            LoadTables();
        }
 

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
				return eProfileType.MethodGroupAllocation;
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
			get	{ return _allocationCriteria.ExceedMaximums; }
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
			get	{ return _allocationCriteria.PercentNeedLimit; }
			set	{ _allocationCriteria.PercentNeedLimit = value; }
		}

		/// <summary>
		/// Gets or sets ReserveIsPercent flag value.
		/// </summary>
		public bool ReserveIsPercent
		{
			get	{ return _allocationCriteria.ReserveIsPercent; }
			set	{ _allocationCriteria.ReserveIsPercent = value; }
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
			get	{ return _allocationCriteria.OTSPlanPHL; }
			set	{ _allocationCriteria.OTSPlanPHL = value; }
		}
		public int OTSPlanPHLSeq
		{
			get	{ return _allocationCriteria.OTSPlanPHLSeq; }
			set	{ _allocationCriteria.OTSPlanPHLSeq = value; }
		}
		/// <summary>
		/// Gets or sets OTSPlanRID which identifies a specific merchandise plan within the hierarchy as a source of plans.
		/// </summary>
		public int OTSPlanRID
		{
			get { return _allocationCriteria.OTSPlanRID; }
			set	{ _allocationCriteria.OTSPlanRID = value; }
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
			get	{ return _allocationCriteria.OTSOnHandPHLSeq; }
			set { _allocationCriteria.OTSOnHandPHLSeq = value; }
		}

		/// <summary>
		/// Gets or sets the OTSOnHandRID which identifies a specific merchandise node within the hierarchy as the source for store actual onhands and intransit.
		/// </summary>
		public int OTSOnHandRID
		{
			get	{ return _allocationCriteria.OTSOnHandRID; }
			set	{ _allocationCriteria.OTSOnHandRID = value; }
		}

		/// <summary>
		/// Gets or sets the OTSPlanFactorPercent.
		/// </summary>
		/// <remarks>
		/// This percent is used to extract the part of a plan that the onhand source represents.
		/// </remarks>
		public double OTSPlanFactorPercent
		{
			get	{ return _allocationCriteria.OTSPlanFactorPercent; }
			set	{ _allocationCriteria.OTSPlanFactorPercent = value; }
		}

		/// <summary>
		/// Gets or sets the Capacity StoreGroupRID.
		/// </summary>
		public int CapacityStoreGroupRID
		{
			get	{ return _allocationCriteria.CapacityStoreGroupRID; }
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
			get	{ return _allocationCriteria.UseStoreGradeDefault; }
			set	{ _allocationCriteria.UseStoreGradeDefault = value; }
		}

		/// <summary>
		/// Gets or sets the UsePctNeedDefault flag value.
		/// </summary>
		public bool UsePctNeedDefault
		{
			get	{ return _allocationCriteria.UsePctNeedDefault; }
			set	{ _allocationCriteria.UsePctNeedDefault = value; }
		}

		/// <summary>
		/// Gets or sets the UseFactorPctDefault flag value.
		/// </summary>
		public bool UseFactorPctDefault
		{
			get	{ return _allocationCriteria.UseFactorPctDefault; }
			set	{ _allocationCriteria.UseFactorPctDefault = value; }
		}

		/// <summary>
		/// Gets or sets the group allocation Child Occurs data.
		/// </summary>
		public DataSet DSGroupAllocation 
		{
			get
			{
				return _dsGroupAllocation;
			}
			set
			{
				_dsGroupAllocation = value;
			}
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
            get
            {
                return _allocationCriteria.InventoryInd;
            }
            set
            {
                _allocationCriteria.InventoryInd = value;
            }
        }
        /// <summary>
        /// Gets or sets the Inventory Merchandise Hierarchy Node RID
        /// </summary>
        public int MERCH_HN_RID
        {
            get
            {
                return _allocationCriteria.Inventory_MERCH_HN_RID;
            }

            set
            {
                _allocationCriteria.Inventory_MERCH_HN_RID = value;
            }
        }
        /// <summary>
        /// Gets or sets the Inventory Merchandise Product Hierarchy Node RID
        /// </summary>
        public int MERCH_PH_RID
        {
            get
            {
                return _allocationCriteria.Inventory_MERCH_PH_RID;
            }

            set
            {
                _allocationCriteria.Inventory_MERCH_PH_RID = value;
            }
        }
        /// <summary>
        /// Gets or sets the Inventory Merchandise Product Level Sequence
        /// </summary>
        public int MERCH_PHL_SEQ
        {
            get
            {
                return _allocationCriteria.Inventory_MERCH_PHL_SEQ;
            }

            set
            {
                _allocationCriteria.Inventory_MERCH_PHL_SEQ = value;
            }
        }
        // BEGIN TT#1131-MD - AGallagher - Add Header Min/Max Options and Inventory Basis
        public char HdrInventoryInd
        {
            get
            {
                return _allocationCriteria.HdrInventoryInd;
            }
            set
            {
                _allocationCriteria.HdrInventoryInd = value;
            }
        }
        /// <summary>
        /// Gets or sets the Inventory Merchandise Hierarchy Node RID
        /// </summary>
        public int HDRMERCH_HN_RID
        {
            get
            {
                return _allocationCriteria.HdrInventory_MERCH_HN_RID;
            }

            set
            {
                _allocationCriteria.HdrInventory_MERCH_HN_RID = value;
            }
        }
        /// <summary>
        /// Gets or sets the Inventory Merchandise Product Hierarchy Node RID
        /// </summary>
        public int HDRMERCH_PH_RID
        {
            get
            {
                return _allocationCriteria.HdrInventory_MERCH_PH_RID;
            }

            set
            {
                _allocationCriteria.HdrInventory_MERCH_PH_RID = value;
            }
        }
        /// <summary>
        /// Gets or sets the Inventory Merchandise Product Level Sequence
        /// </summary>
        public int HDRMERCH_PHL_SEQ
        {
            get
            {
                return _allocationCriteria.HdrInventory_MERCH_PHL_SEQ;
            }

            set
            {
                _allocationCriteria.HdrInventory_MERCH_PHL_SEQ = value;
            }
        }
        // END TT#1131-MD - AGallagher - Add Header Min/Max Options and Inventory Basis
        /// <summary>
        /// Gets or sets the Inventory Merchandise Type
        /// </summary>
        public eMerchandiseType MerchandiseType
        {
            get { return _allocationCriteria.InventoryMerchandiseType; }
            set { _allocationCriteria.InventoryMerchandiseType = value; }
        }
        // BEGIN TT#1131-MD - AGallagher - Add Header Min/Max Options and Inventory Basis
        public eMerchandiseType HdrMerchandiseType
        {
            get { return _allocationCriteria.HdrInventoryMerchandiseType; }
            set { _allocationCriteria.HdrInventoryMerchandiseType = value; }
        }
        // BEGIN TT#1131-MD - AGallagher - Add Header Min/Max Options and Inventory Basis
        /// <summary>
        /// Gets or sets the Group Shipping Horizon Begin Calendar RID
        /// </summary>
		public int BeginCdrRid
		{
			get { return _allocationCriteria.BeginCdrRID; }
			set { _allocationCriteria.BeginCdrRID = value; }
		}
		/// <summary>
		/// Gets or set the Group Shipping Horizon Shipping Calendar RID
		/// </summary>
        public int ShipToCdrRid
		{
			get { return _allocationCriteria.ShipToCdrRID; }
			set { _allocationCriteria.ShipToCdrRID = value; }
		}
        /// <summary>
        /// Gets or sets the Line Item Minimum Override Indicator
        /// </summary>
		public bool LineItemMinOverrideInd
		{
			get { return _allocationCriteria.LineItemMinOverrideInd; }
			set { _allocationCriteria.LineItemMinOverrideInd = value; }
		}
        /// <summary>
        /// Gets or sets the Line Item Minimum Override value
        /// </summary>
		public int LineItemMinOverride
		{
			get { return _allocationCriteria.LineItemMinOverride; }
			set { _allocationCriteria.LineItemMinOverride = value; }
		}

		// Begin TT#4332 - stodd - Matrix randomly shows all zeros for a grade tier
        public DataTable StoreGrades
        {
            get
            {
                return _dsGroupAllocation.Tables["StoreGrades"];
            }
        }
		// End TT#4332 - stodd - Matrix randomly shows all zeros for a grade tier

		//========
		// METHODS
		//========

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        override internal bool CheckForUserData()
        {
            if (CheckAllocationCriteriaForUserData(_allocationCriteria))
            {
                return true;
            }

            return false;
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

		/// <summary>
		/// Load tables from the data source tables.
		/// </summary>
		private void LoadTables()
		{
			if (_dsGroupAllocation.Tables["StoreGrades"].Rows.Count > 0)
			{
				LoadStoreGrades();
			}
        }

		/// <summary>
		/// Load Store Grade data.
		/// </summary>
		private void LoadStoreGrades()
		{
			string gradeCode;
            int boundary, minStock, maxStock, minHeader, maxHeader; 

            int sglRID;     
			try
			{
				DataTable dtStoreGrades = _dsGroupAllocation.Tables["StoreGrades"];
				foreach (DataRow dr in dtStoreGrades.Rows)
				{
					boundary = Convert.ToInt32(dr["Boundary"],CultureInfo.CurrentUICulture);
					gradeCode =  Convert.ToString(dr["Grade"],CultureInfo.CurrentUICulture);
                    sglRID = Convert.ToInt32(dr["SGLRID"], CultureInfo.CurrentUICulture);
					AddGrade(sglRID, boundary, gradeCode); 
					if (dr["Group Min"] != System.DBNull.Value)
					{
						minStock = Convert.ToInt32(dr["Group Min"],CultureInfo.CurrentUICulture);
                        SetGradeGroupMinimum(sglRID, boundary, minStock); 
					}
					
					if (dr["Group Max"] != System.DBNull.Value)
					{
						maxStock = Convert.ToInt32(dr["Group Max"],CultureInfo.CurrentUICulture);
                        SetGradeGroupMaximum(sglRID, boundary, maxStock); 
					}
					 
					if (dr["Header Min"] != System.DBNull.Value)
					{
                        minHeader = Convert.ToInt32(dr["Header Min"], CultureInfo.CurrentUICulture); 
                        SetGradeHeaderMinimum(sglRID, boundary, minHeader);           
					}

					if (dr["Header Max"] != System.DBNull.Value)
					{
                        maxHeader = Convert.ToInt32(dr["Header Max"], CultureInfo.CurrentUICulture); 
                        SetGradeHeaderMaximum(sglRID, boundary, maxHeader);          
                    }
                    if (dr["SGLRID"] != System.DBNull.Value)
                    {
                        sglRID = Convert.ToInt32(dr["SGLRID"], CultureInfo.CurrentUICulture);
                        SetGradeAttributeSet(sglRID, boundary);
                    }
				}
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

        ///// <summary>
        ///// Determines if Attribute Set (aSglRID) and Low Boundary (aBoundary) have a grade defined
        ///// </summary>
        ///// <param name="aSglRID">Attribute Set</param>
        ///// <param name="aBoundary">Low Boundary</param>
        ///// <param name="aAllocationGradeBin">If Grade Is In Method, this is the GroupAllocationGradeBin found, otherwise null</param>
        ///// <returns>True: Grade IS in method and aAllocationGradeBin is that grade; False: Grade is not in method</returns>
        //public bool GradeIsInMethod(int aSglRID, double aBoundary, out GroupAllocationGradeBin aGroupAllocationGradeBin)
        //{
        //    Dictionary<int, Dictionary<double, GroupAllocationGradeBin>> grade =
        //        _allocationCriteria.GetGroupAllocationGrades();
        //    Dictionary<double, GroupAllocationGradeBin> boundaryHT;
        //    if (grade.TryGetValue(aSglRID, out boundaryHT))
        //    {
        //        return boundaryHT.TryGetValue(aBoundary, out aGroupAllocationGradeBin);
        //    }
        //    else
        //    {
        //        aGroupAllocationGradeBin = null;
        //        return false;
        //    }
        //}

        /// <summary>
        /// Adds a Grade for the given Attribute Set and Low Boundary
        /// </summary>
        /// <param name="aAttributeSetRID">Attribute Set RID</param>
        /// <param name="aLowBoundary">Grade Low Boundary</param>
        /// <param name="aGradeName">Grade Name</param>
		public void AddGrade(int aAttributeSetRID, double aLowBoundary ,string aGradeName)
		{
            _allocationCriteria.AddGrade(aAttributeSetRID, aLowBoundary, aGradeName);
		}
        /// <summary>
        /// Sets Group Grade Minimum for the given Attribute Set and Grade Low Boundary
        /// </summary>
        /// <param name="aAttributeSetRID">Attribute Set</param>
        /// <param name="aLowBoundary">Grade Low Boundary</param>
        /// <param name="aMinimum">Minimum Value</param>
        public void SetGradeGroupMinimum(int aAttributeSetRID, double aLowBoundary, int aMinimum)
		{
            _allocationCriteria.SetGroupGradeMinimum(aAttributeSetRID, aLowBoundary, aMinimum);
		}
        /// <summary>
        /// Sets Group Grade Maximum for the given Attribute Set and Grade Low Boundary
        /// </summary>
        /// <param name="aAttributeSetRID">Attribute Set</param>
        /// <param name="aLowBoundary">Grade Low Boundary</param>
        /// <param name="aMaximum">Maximum Value</param>
        public void SetGradeGroupMaximum(int aAttributeSetRID, double aLowBoundary, int aMaximum)
		{
            _allocationCriteria.SetGroupGradeMaximum(aAttributeSetRID, aLowBoundary, aMaximum);
		}
        /// <summary>
        /// Sets Header Grade Minimum for the given Attribute Set and Grade Low Boundary
        /// </summary>
        /// <param name="aAttributeSetRID">Attribute Set</param>
        /// <param name="aLowBoundary">Grade Low Boundary</param>
        /// <param name="aHeaderMinimum">Minimum Value</param>
        public void SetGradeHeaderMinimum(int aAttributeSetRID, double aLowBoundary, int aHeaderMinimum)
        {
            _allocationCriteria.SetHeaderGradeMinimum(aAttributeSetRID, aLowBoundary, aHeaderMinimum);
        }
        /// <summary>
        /// Sets Header Grade maximum for the given attribute set and grade low boundary
        /// </summary>
        /// <param name="aAttributeSetRID">Atrribute Set</param>
        /// <param name="aLowBoundary">Grade Low Boundary</param>
        /// <param name="aHeaderMaximum">Maximum Value</param>
        public void SetGradeHeaderMaximum(int aAttributeSetRID, double aLowBoundary, int aHeaderMaximum)
		{
            _allocationCriteria.SetHeaderGradeMaximum(aAttributeSetRID, aLowBoundary, aHeaderMaximum);
		}
        /// <summary>
        /// Sets Grade Attribute Set for the given Low Boundary
        /// </summary>
        /// <param name="aAttributeSetRID">Attribute Set</param>
        /// <param name="aLowBoundary">Grade Low Boundary</param>
        public void SetGradeAttributeSet(int aAttributeSetRID, double aLowBoundary)
        {
            _allocationCriteria.SetGradeAttributeSet(aAttributeSetRID, aLowBoundary);
         }
        /// <summary>
        /// Sets Header Grade Ship Up To for the given Attribute Set and Grade Low Boundary
        /// </summary>
        /// <param name="aAttributeSetRID">Attribute Set</param>
        /// <param name="aLowBoundary">Grade Low Boundary</param>
        /// <param name="aShipUpTo">Ship Up To Value</param>
        public void SetGradeHeaderShipUpTo(int aAttributeSetRID, double aLowBoundary, int aShipUpTo)
        {
            _allocationCriteria.SetHeaderGradeShipUpTo(aAttributeSetRID, aLowBoundary, aShipUpTo);
        }
 		public override void ProcessMethod(                 // TT#488 - MD - Jellis - Group Allocation
			ApplicationSessionTransaction aApplicationTransaction,
			int aStoreFilter, Profile methodProfile)
		{
			
			aApplicationTransaction.ResetAllocationActionStatus();
			foreach (AssortmentProfile ap in (AllocationProfileList)aApplicationTransaction.GetMasterProfileList(eProfileType.Allocation))
			{
				AllocationWorkFlowStep awfs = 
					new AllocationWorkFlowStep(
					this,
					new GeneralComponent(eGeneralComponentType.Total),
					false,
					true,
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
            AssortmentProfile ap = aProfile as AssortmentProfile; // TT#946 - MD - Jellis - Group Allocation Not Working
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
                MIDException statusMessage;
                if (oac.ProcessAllocationCriteria(
                    (AllocationWorkFlowStep)aWorkFlowStep,
                    //(AllocationProfile)aProfile,  // TT#946 - MD - Jellis - Group Allocation Not Working
                    ap,         // TT#946 - MD - Jellis - Group Allocation Not Working
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
            }
        }
			
  
		override public void Update(TransactionData td)
		{
			if (_groupAllocationData == null || base.Key < 0 || base.Method_Change_Type == eChangeType.add)
			{
				_groupAllocationData = new GroupAllocationMethodData(td, base.Key);
			}
			//  TO DO
			_groupAllocationData.Grade_Week_Count = _allocationCriteria.GradeWeekCount;
			_groupAllocationData.Exceed_Maximums_Ind = Include.ConvertBoolToChar(_allocationCriteria.ExceedMaximums);
			_groupAllocationData.Percent_Need_Limit = _allocationCriteria.PercentNeedLimit;
			_groupAllocationData.Reserve = _allocationCriteria.ReserveQty;
			_groupAllocationData.Percent_Ind = Include.ConvertBoolToChar(_allocationCriteria.ReserveIsPercent);
			_groupAllocationData.Merch_Plan_HN_RID = OTSPlanRID;
			_groupAllocationData.Merch_Plan_PH_RID = OTSPlanPHL;
			_groupAllocationData.Merch_Plan_PHL_SEQ = OTSPlanPHLSeq;
			_groupAllocationData.Merch_OnHand_HN_RID = OTSOnHandRID;
			_groupAllocationData.Merch_OnHand_PH_RID = OTSOnHandPHL;
			_groupAllocationData.Merch_OnHand_PHL_SEQ = OTSOnHandPHLSeq;
			_groupAllocationData.Plan_Factor_Percent = OTSPlanFactorPercent;
			_groupAllocationData.Store_Group_RID = CapacityStoreGroupRID;
            _groupAllocationData.Tab_Store_Group_RID = GradeStoreGroupRID; 
			_groupAllocationData.UseStoreGradeDefault =UseStoreGradeDefault;
			_groupAllocationData.UsePctNeedDefault = UsePctNeedDefault;
			_groupAllocationData.UseFactorPctDefault = UseFactorPctDefault;
			_groupAllocationData.DSGroupAllocation = _dsGroupAllocation;
            _groupAllocationData.Merch_Plan_Unspecified = MerchUnspecified;
            _groupAllocationData.Merch_OnHand_Unspecified = OnHandUnspecified;
			_groupAllocationData.ReserveAsBulk = ReserveAsBulk;
			_groupAllocationData.ReserveAsPacks = ReserveAsPacks;
            _groupAllocationData.Inventory_Ind = InventoryInd;
            _groupAllocationData.IB_MERCH_HN_RID = MERCH_HN_RID;
            _groupAllocationData.IB_MERCH_PH_RID = MERCH_PH_RID;
            _groupAllocationData.IB_MERCH_PHL_SEQ = MERCH_PHL_SEQ;
			_groupAllocationData.BeginCdrRid = BeginCdrRid;
			_groupAllocationData.ShipToCdrRid = ShipToCdrRid;
			_groupAllocationData.LineItemMinOverrideInd = LineItemMinOverrideInd;
			_groupAllocationData.LineItemMinOverride = LineItemMinOverride;
            // BEGIN TT#1131-MD - AGallagher - Add Header Min/Max Options and Inventory Basis
            _groupAllocationData.HDRInventory_Ind = HdrInventoryInd;
            _groupAllocationData.HDRIB_MERCH_HN_RID = HDRMERCH_HN_RID;
            _groupAllocationData.HDRIB_MERCH_PH_RID = HDRMERCH_PH_RID;
            _groupAllocationData.HDRIB_MERCH_PHL_SEQ = HDRMERCH_PHL_SEQ;
            // END TT#1131-MD - AGallagher - Add Header Min/Max Options and Inventory Basis
			try
			{
				switch (base.Method_Change_Type)
				{
					case eChangeType.add:
						base.Update(td);
						_groupAllocationData.InsertMethod(base.Key, td);
						break;
					case eChangeType.update:
						base.Update(td);
                        // make sure the key in the data layer is the same
                        _groupAllocationData.MethodRid = base.Key;
						_groupAllocationData.UpdateMethod(base.Key, td);
						break;
					case eChangeType.delete:
						_groupAllocationData.DeleteMethod(base.Key, td);
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
        override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges, bool aCloneCustomOverrideModels)
        // End Track #5912
		{
			GroupAllocationMethod newGroupAllocationMethod = null;

			try
			{
				newGroupAllocationMethod = (GroupAllocationMethod)this.MemberwiseClone();
				newGroupAllocationMethod.DSGroupAllocation = DSGroupAllocation.Copy();
				newGroupAllocationMethod.ExceedMaximums = ExceedMaximums;
				newGroupAllocationMethod.GradeWeekCount = GradeWeekCount;
				newGroupAllocationMethod.Method_Change_Type = eChangeType.none;
				newGroupAllocationMethod.Method_Description = Method_Description;
				newGroupAllocationMethod.MethodStatus = MethodStatus;
				newGroupAllocationMethod.Name = Name;
				newGroupAllocationMethod.OTSOnHandPHL = OTSOnHandPHL;
				newGroupAllocationMethod.OTSOnHandPHLSeq = OTSOnHandPHLSeq;
				newGroupAllocationMethod.OTSOnHandRID = OTSOnHandRID;
				newGroupAllocationMethod.OTSPlanFactorPercent = OTSPlanFactorPercent;
				newGroupAllocationMethod.OTSPlanPHL = OTSPlanPHL;
				newGroupAllocationMethod.OTSPlanPHLSeq = OTSPlanPHLSeq;
				newGroupAllocationMethod.OTSPlanRID = OTSPlanRID;
				newGroupAllocationMethod.PercentNeedLimit = PercentNeedLimit;
				newGroupAllocationMethod.ReserveIsPercent = ReserveIsPercent;
				newGroupAllocationMethod.ReserveQty = ReserveQty;
				newGroupAllocationMethod.SG_RID = SG_RID;
				newGroupAllocationMethod.CapacityStoreGroupRID = CapacityStoreGroupRID;
                newGroupAllocationMethod.GradeStoreGroupRID = GradeStoreGroupRID;
				newGroupAllocationMethod.UseFactorPctDefault = UseFactorPctDefault;
				newGroupAllocationMethod.UsePctNeedDefault = UsePctNeedDefault;
				newGroupAllocationMethod.User_RID = User_RID;
				newGroupAllocationMethod.Virtual_IND = Virtual_IND;
                newGroupAllocationMethod.Template_IND = Template_IND;

                newGroupAllocationMethod.MerchUnspecified = MerchUnspecified;
                newGroupAllocationMethod.OnHandUnspecified = OnHandUnspecified;

				return newGroupAllocationMethod;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

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

            if (_allocationCriteria.Inventory_MERCH_HN_RID != Include.NoRID)
            {
                hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_allocationCriteria.Inventory_MERCH_HN_RID, (int)eSecurityTypes.Store);
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
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalGroupAllocation);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserGroupAllocation);
            }

        }
        override public ROMethodProperties MethodGetData(out bool successful, ref string message, bool processingApply = false)
        {
            successful = true;

            throw new NotImplementedException("MethodGetData is not implemented");
        }

        override public ROOverrideLowLevel MethodGetOverrideModelList(ROOverrideLowLevel overrideLowLevel, out bool successful, ref string message)
        {
            successful = true;

            throw new NotImplementedException("MethodGetOverrideModelList is not implemented");
        }

        override public bool MethodSetData(ROMethodProperties methodProperties, ref string message, bool processingApply)
        {
            throw new NotImplementedException("MethodSaveData is not implemented");
        }

        override public ROMethodProperties MethodCopyData()
        {
            throw new NotImplementedException("MethodCopyData is not implemented");
        }
        // END RO-642 - RDewey
    }
}
