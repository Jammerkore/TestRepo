using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Globalization;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Common
{
	/// <summary>
	/// Summary description for Global_Options.
	/// </summary>
	[Serializable()]
	public class GlobalOptionsProfile : Profile 
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public GlobalOptionsProfile(int aKey)
			: base(aKey)
		{
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.GlobalOptions;
			}
		}

		#region Internal Fields
		private string _companyName;
		private string _companyStreet;
		private string _companyCity;
		private string _companyState;
		private string _companyZip;
		private string _companyPhone;
		private string _companyFax;
		private string _companyEmail;
		private char _productLevelDelimiter;
		private int _purgeAllocationsPeriod;
		private eStoreDisplayOptions _storeDisplayOptionId;
		private int _defaultOtsSgRid;
		private int _defaultAllocSgRid;
		private int _newStorePeriodBegin;
		private int _newStorePeriodEnd;
		private int _nonCompStorePeriodBegin;
		private int _nonCompStorePeriodEnd;
		private eHierarchyDisplayOptions _productLevelDisplayId;
		private double _defaultPercentNeedLimit;
		private double _defaultBalanceTolerance;
		private double _defaultPackSizeErrorPercent;
		private double _defaultMaxSizeErrorPercent;
		private double _defaultFillSizeHolesPercent;
		private bool _sizeBreakoutInd;
//		private bool _sizeNeedInd;
		private bool _bulkIsDetailInd;
		private int _storeGradePeriod;
		private bool _protectInterfaceHeadersInd;
		private int _reserveStoreRid;
		private int _numberOfStoreDataTables;
		private bool _useWindowsLogin;
		private int _shippingHorizonWeeks;
		private HeaderTypeProfileList _headerTypeProfileList;
		private AppConfig _appConfig;
		private int _headerLinkCharacteristicKey;
		private string _headerLinkCharacteristicValue;
		private string _sizeCurveCharMask;		// BEGIN ANF Generic Size Constraints
		private string _sizeGroupCharMask;
		private string _sizeAlternateCharMask;
		private string _sizeConstraintCharMask;	// END ANF Generic Size Constraints
		// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
		private bool _normalizeSizeCurves;
		// END MID Track #4826
		// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
		private eFillSizesToType _fillSizesToType;
		// End MID Track #4921
        private double _genericPackRounding1stPackPct;   // TT#616 - AGallagher - Allocation - Pack Rounding (#67)
        private double _genericPackRoundingNthPackPct;   // TT#616 - AGallagher - Allocation - Pack Rounding (#67)
		// BEGIN MID Track #6335 Option to not Release Hdr with all units in reserve
		private bool _allowReleaseIfAllUnitsInReserve;
        // END MID Track #6335 Option to not Release Hdr with all units in reserve
        // BEGIN MID Track #6043 - KJohnson - Forcast Chain WOS Calculations
        private int _numberOfWeeksWithZeroSales;
        private int _maximumChainWOS; 
        private bool _prorateChainStock;
		// END MID Track #6043 - KJohnson
		// BEGIN MID Track #6074 - stodd - velocity changes
		// Begin TT #91 - stodd
		//private bool _defaultGradesByBasis;
		// End TT #91 - stodd
		// End MID Track #6074
        // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
        private eGenericSizeCurveNameType _genericSizeCurveNameType;
        // End TT#413

        private eVSWSizeConstraints _vSWSizeConstraints;  // TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
        private bool _RI_EXPAND_IND;  // TT#628-MD - JSmith -  Add Option To Not Expand Allocation Records Once Relieved

		// Begin TT#391 - stodd - Tells size curve method to use in stock sales.
		private eGenerateSizeCurveUsing _generateSizeCurveUsing;
		// End TT#391
        // begin TT#1365 - JEllis - FL Detail Packs Size Need Enhancement
        private bool _packToleranceNoMaxStep;
        private bool _packToleranceStepped;
        // end TT#1365 - JEllis - FL Detail Packs Size Need Enhancement
        // Begin TT#2170 - JSmith - Daily Size Purge Not Working
        private bool _performOnetimePurge;
        // End TT#2170
        private bool _allowStoreMaxValueModification;  // TT#1401 - AGallagher - Reservation Stores
		private bool _isStoreDeleteInProgress; 	// TT#739-MD - STodd - delete stores
		private bool _constraintsDropped; 	// TT#739-MD - STodd - delete stores
        private bool _enableVelocityGradeOptions; //TT#855-MD -jsobek -Velocity Enhancements
        private bool _forceSingleClientInstance; //TT#894-MD -jsobek -Single Client Instance System Option
        private bool _forceSingleUserInstance; //TT#898-MD -jsobek -Single User Instance System Option
        private bool _useActiveDirectoryAuthentication;
        private bool _useActiveDirectoryAuthenticationWithDomain; 
        private eVSWItemFWOSMax _vswItemFWOSMax;   // TT#933-MD - AGallagher - Item Max vs. FWOS Max 
        private int _dcCartonRoundingSGRid;        // TT#1652-MD - RMatelic - DC Carton Rounding  
		private bool _priorHeaderIncludeReserve;// TT#1608-MD - SRisch - Prior Header
        // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
        private eDCFulfillmentSplitOption _split_Option;
        private char _apply_Minimums_Ind;
        private char _prioritize_Type;
        private int _header_Field;
        private int _hcg_Rid;
        private eDCFulfillmentHeadersOrder _header_Order;
        private eDCFulfillmentStoresOrder _store_Order;
        private eDCFulfillmentSplitByOption _split_By_Option;
        private eDCFulfillmentReserve _split_By_Reserve;
        private eDCFulfillmentMinimums _apply_By;
        private eDCFulfillmentWithinDC _within_Dc;
        // END TT#1966-MD - AGallagher - DC Fulfillment
        private bool _useExternalEligibility;
		#endregion

		public int OTSPlanStoreGroupRID
		{
			get	{ return _defaultOtsSgRid;}
			set	{ _defaultOtsSgRid = value;	}
		}
		public eStoreDisplayOptions StoreDisplay
		{
			get { return _storeDisplayOptionId; }
			set { _storeDisplayOptionId = value; }
		}
		public eHierarchyDisplayOptions ProductLevelDisplay
		{
			get { return _productLevelDisplayId; }
			set { _productLevelDisplayId = value; }
		}
		public string CompanyName
		{
			get{return _companyName;}
			set{_companyName=value;}
		}
		public string Street
		{
			get{return _companyStreet;}
			set{_companyStreet = value;}
		}
		public string City
		{
			get{return _companyCity;}
			set{_companyCity = value;}
		}
		public string State
		{
			get{return _companyState;}
			set{_companyState = value;}
		}
		public string Zip
		{
			get{return _companyZip;}
			set{_companyZip = value;}
		}
		public string Telephone
		{
			get{return _companyPhone;}
			set{_companyPhone = value;}
		}
		public string Fax
		{
			get{return _companyFax;}
			set{_companyFax = value;}
		}
		public string Email
		{
			get{return _companyEmail;}
			set{_companyEmail = value;}
		}
		public char ProductLevelDelimiter
		{
			get{return _productLevelDelimiter;}
			set{_productLevelDelimiter = value;}
		}
		public int PurgeAllocationsPeriod
		{
			get{return _purgeAllocationsPeriod;}
			set{_purgeAllocationsPeriod = value;}
		}
		public int AllocationStoreGroupRID 
		{
			get{return _defaultAllocSgRid;}
			set{_defaultAllocSgRid = value;}
		}
		public int NewStorePeriodBegin 
		{
			get{return _newStorePeriodBegin;}
			set{_newStorePeriodBegin = value;}
		}
		public int NewStorePeriodEnd
		{
			get{return _newStorePeriodEnd;}
			set{_newStorePeriodEnd = value;}
		}
		public int NonCompStorePeriodBegin 
		{
			get{return _nonCompStorePeriodBegin;}
			set{_nonCompStorePeriodBegin = value;}
		}
		public int NonCompStorePeriodEnd 
		{
			get{return _nonCompStorePeriodEnd;}
			set{_nonCompStorePeriodEnd = value;}
		}
		public double PercentNeedLimit
		{
			get{return _defaultPercentNeedLimit;}
			set{_defaultPercentNeedLimit = value;}
		}
		public double BalanceTolerancePercent
		{
			get{return _defaultBalanceTolerance;}
			set{_defaultBalanceTolerance = value;}
		}
		public double PackSizeErrorPercent
		{
			get{return _defaultPackSizeErrorPercent;}
			set{_defaultPackSizeErrorPercent = value;}
		}
		public double MaxSizeErrorPercent
		{
			get{return _defaultMaxSizeErrorPercent;}
			set{_defaultMaxSizeErrorPercent = value;}
		}
		public double FillSizeHolesPercent
		{
			get{return _defaultFillSizeHolesPercent;}
			set{_defaultFillSizeHolesPercent = value;}
		}
        // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
        public double GenericPackRounding1stPackPct
        {
            get { return _genericPackRounding1stPackPct; }
            set { _genericPackRounding1stPackPct = value; }
        }
        public double GenericPackRoundingNthPackPct
        {
            get { return _genericPackRoundingNthPackPct; }
            set { _genericPackRoundingNthPackPct = value; }
        }
        // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)
		public bool SizeBreakoutInd
		{
			get{return _sizeBreakoutInd;}
			set{_sizeBreakoutInd = value;}
		}
//		public bool SizeNeedInd
//		{
//			get{return _sizeNeedInd;}
//			set{_sizeNeedInd = value;}
//		}
		public bool BulkIsDetail
		{
			get{return _bulkIsDetailInd;}
			set{_bulkIsDetailInd = value;}
		}
		public int StoreGradePeriod
		{
			get{return _storeGradePeriod;}
			set{_storeGradePeriod = value;}
		}		
		public bool ProtectInterfacedHeadersInd
		{
			get{return _protectInterfaceHeadersInd;}
			set{_protectInterfaceHeadersInd = value;}
		}
		public int ReserveStoreRID
		{
			get{return _reserveStoreRid;}
			set{_reserveStoreRid = value;}
		}	
		public int NumberOfStoreDataTables
		{
			get{return _numberOfStoreDataTables;}
			set{_numberOfStoreDataTables = value;}
		}	
		public bool UseWindowsLogin
		{
			get{return _useWindowsLogin;}
			set{_useWindowsLogin = value;}
		}
		public int ShippingHorizonWeeks
		{
			get{return _shippingHorizonWeeks;}
			set{_shippingHorizonWeeks = value;}
		}
		public HeaderTypeProfileList HeaderTypeProfileList
		{
			get{return _headerTypeProfileList;}
			set{_headerTypeProfileList = value;}
		}
		public AppConfig AppConfig
		{
			get{return _appConfig;}
		}
		public int HeaderLinkCharacteristicKey
		{
			get	{ return _headerLinkCharacteristicKey;}
			set	{ _headerLinkCharacteristicKey = value;	}
		}
		public string HeaderLinkCharacteristicValue
		{
			get	{ return _headerLinkCharacteristicValue;}
			set	{ _headerLinkCharacteristicValue = value;	}
		}
		// BEGIN ANF Generic Size Constrainst
		public string SizeCurveCharMask
		{
			get	{ return _sizeCurveCharMask;}
			set	{ _sizeCurveCharMask = value;	}
		}
		public string SizeGroupCharMask
		{
			get	{ return _sizeGroupCharMask;}
			set	{ _sizeGroupCharMask = value;	}
		}
		public string SizeAlternateCharMask
		{
			get	{ return _sizeAlternateCharMask;}
			set	{ _sizeAlternateCharMask = value;	}
		}
		public string SizeConstraintCharMask
		{
			get	{ return _sizeConstraintCharMask;}
			set	{ _sizeConstraintCharMask = value;	}
		}
	 	// END ANF Generic Size Constraints
		// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
		public bool NormalizeSizeCurves
		{
			get	{ return _normalizeSizeCurves;}
			set	{ _normalizeSizeCurves = value;	}
		}
		// END MID Track #4826

		// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
		public eFillSizesToType FillSizesToType
		{
			get	{ return _fillSizesToType;}
			set	{ _fillSizesToType = value;	}
		}
		// End MID Track #4921

        // BEGIN MID Track #6335 Option to not Release Hdr with all units in reserve
		public bool AllowReleaseIfAllUnitsInReserve
		{
			get { return _allowReleaseIfAllUnitsInReserve; }
			set { _allowReleaseIfAllUnitsInReserve = value; }
		}
		// END MID Track #6335 Option to not Release Hdr with all units in reserve

        // BEGIN MID Track #6043 - KJohnson - Forcast Chain WOS Calculations
        public int NumberOfWeeksWithZeroSales
        {
            get { return _numberOfWeeksWithZeroSales; }
            set { _numberOfWeeksWithZeroSales = value; }
        }
        public int MaximumChainWOS
        {
            get { return _maximumChainWOS; }
            set { _maximumChainWOS = value; }
        }		
        public bool ProrateChainStock
        {
            get { return _prorateChainStock; }
            set { _prorateChainStock = value; }
        }
        // END MID Track #6043 - KJohnson
		// BEGIN MID Track #6074 - stodd - velocity changes
		// Begin TT #91 - stodd
		//public bool DefaultGradesByBasis
		//{
		//    get { return _defaultGradesByBasis; }
		//    set { _defaultGradesByBasis = value; }
		//}
		// End TT #91 - stodd
		// End MID Track #6074
        // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
        public eGenericSizeCurveNameType GenericSizeCurveNameType
        {
            get { return _genericSizeCurveNameType; }
            set { _genericSizeCurveNameType = value; }
        }
        // End TT#413
		// Begin TT#391 - stodd - Tells size curve method to use in stock sales.
		public eGenerateSizeCurveUsing GenerateSizeCurveUsing
		{
			get { return _generateSizeCurveUsing; }
			set { _generateSizeCurveUsing = value; }
		}
		// End TT#391

        // begin TT#1365 - JEllis - FL Detail Packs Size Need Enhancement
        public bool PackToleranceNoMaxStep
        {
            get { return _packToleranceNoMaxStep; }
            set { _packToleranceNoMaxStep = value; }
        }
        public bool PackToleranceStepped
        {
            get { return _packToleranceStepped; }
            set { _packToleranceStepped = value; }
        }
        // end TT#1365 - JEllis - FL Detail Packs Size Need Enhancement
		
		// Begin TT#2170 - JSmith - Daily Size Purge Not Working
        public bool PerformOnetimePurge
        {
            get { return _performOnetimePurge; }
            set { _performOnetimePurge = value; }
        }
        // End TT#2170

        // BEGIN TT#1401 - AGallagher - Reservation Stores
        public bool AllowStoreMaxValueModification
        {
            get { return _allowStoreMaxValueModification; }
            set { _allowStoreMaxValueModification = value; }
        }
        // END TT#1401 - AGallagher - Reservation Stores

        // BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
        public eVSWSizeConstraints VSWSizeConstraints
        {
            get { return _vSWSizeConstraints; }
            set { _vSWSizeConstraints = value; }
        }
        // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 

        // Begin TT#628-MD - JSmith -  Add Option To Not Expand Allocation Records Once Relieved
        public bool RI_EXPAND
        {
            get { return _RI_EXPAND_IND; }
            set { _RI_EXPAND_IND = value; }
        }
        // End TT#628-MD - JSmith -  Add Option To Not Expand Allocation Records Once Relieved

		// BEGIN TT#739-MD - STodd - delete stores
		public bool IsStoreDeleteInProgress
		{
			get { return _isStoreDeleteInProgress; }
			//set { _RI_EXPAND_IND = value; }
		}

		public bool ConstraintsDropped
		{
			get { return _constraintsDropped; }
			//set { _RI_EXPAND_IND = value; }
		}
		// END TT#739-MD - STodd - delete stores

        //Begin TT#855-MD -jsobek -Velocity Enhancements
        public bool EnableVelocityGradeOptions
        {
            get { return _enableVelocityGradeOptions; }
            set { _enableVelocityGradeOptions = value; }
        }
        //End TT#855-MD -jsobek -Velocity Enhancements
		
        //Begin TT#894-MD -jsobek -Single Client Instance System Option
        public bool ForceSingleClientInstance
        {
            get { return _forceSingleClientInstance; }
            set { _forceSingleClientInstance = value; }
        }
        //End TT#894-MD -jsobek -Single Client Instance System Option
        //Begin TT#898-MD -jsobek -Single User Instance System Option
        public bool ForceSingleUserInstance
        {
            get { return _forceSingleUserInstance; }
            set { _forceSingleUserInstance = value; }
        }
        //End TT#898-MD -jsobek -Single User Instance System Option
        public bool UseActiveDirectoryAuthentication
        {
            get { return _useActiveDirectoryAuthentication; }
            set { _useActiveDirectoryAuthentication = value; }
        }
        public bool UseActiveDirectoryAuthenticationWithDomain
        {
            get { return _useActiveDirectoryAuthenticationWithDomain; }
            set { _useActiveDirectoryAuthenticationWithDomain = value; }
        }

        // BEGIN TT#933-MD - AGallagher - Item Max vs. FWOS Max
        public eVSWItemFWOSMax VSWItemFWOSMax
        {
            get { return _vswItemFWOSMax; }
            set { _vswItemFWOSMax = value; }
        }
        // END TT#933-MD - AGallagher - Item Max vs. FWOS Max

        // Begin TT#1652-MD - RMatelic - DC Carton Rounding  
        public int DCCartonRoundingSGRid
        {
            get { return _dcCartonRoundingSGRid; }
            set { _dcCartonRoundingSGRid = value; }
        }
        // End TT#1652-MD  
		
		// BEGIN TT#1608-MD - SRisch - Prior Header
        public bool PriorHeaderIncludeReserve
        {
            get {return _priorHeaderIncludeReserve;}
            set {_priorHeaderIncludeReserve = value;}
        }
        // END TT#1608-MD - SRisch - Prior Header

        // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
        public  eDCFulfillmentSplitOption Split_Option
        {
            get { return _split_Option; }
            set { _split_Option = value; }
        }
        public char Apply_Minimum_IND
        {
            get { return _apply_Minimums_Ind; }
            set { _apply_Minimums_Ind = value; }
        }
        public char Prioritize_Type
        {
            get { return _prioritize_Type; }
            set { _prioritize_Type = value; }
        }
        public int Header_Field
        {
            get { return _header_Field; }
            set { _header_Field = value; }
        }
        public int HCG_Rid
        {
            get { return _hcg_Rid; }
            set { _hcg_Rid = value; }
        }
        public eDCFulfillmentHeadersOrder Header_Order
        {
            get { return _header_Order; }
            set { _header_Order = value; }
        }
        public eDCFulfillmentStoresOrder STORE_ORDER
        {
            get { return _store_Order; }
            set { _store_Order = value; }
        }
        public eDCFulfillmentSplitByOption Split_BY_Option
        {
            get { return _split_By_Option; }
            set { _split_By_Option = value; }
        }
        public eDCFulfillmentReserve Split_By_Reserve
        {
            get { return _split_By_Reserve; }
            set { _split_By_Reserve = value; }
        }
        public eDCFulfillmentMinimums Apply_By
        {
            get { return _apply_By; }
            set { _apply_By = value; }
        }
        public eDCFulfillmentWithinDC Within_Dc
        {
            get { return _within_Dc; }
            set { _within_Dc = value; }
        }
        // END TT#1966-MD - AGallagher - DC Fulfillment

        public bool UseExternalEligibility
        {
            get { return _useExternalEligibility; }
            set { _useExternalEligibility = value; }
        }

		public void LoadOptions()
		{
			try
			{
				MIDRetail.Data.GlobalOptions opts = new MIDRetail.Data.GlobalOptions();
				DataTable dt = opts.GetGlobalOptions();
				DataRow dr = dt.Rows[0];
				this.Key = Convert.ToInt32(dr["SYSTEM_OPTION_RID"], CultureInfo.CurrentUICulture);

				// company tab
				this.CompanyName = dr["COMPANY_NAME"].ToString();
				this.Street = dr["COMPANY_STREET"].ToString();
				this.City = dr["COMPANY_CITY"].ToString();
				this.State = dr["COMPANY_SP_ABBREVIATION"].ToString();
				this.Zip = dr["COMPANY_POSTAL_CODE"].ToString();
				this.Telephone = dr["COMPANY_TELEPHONE"].ToString();
				this.Fax = dr["COMPANY_FAX"].ToString();
				this.Email = dr["COMPANY_EMAIL"].ToString();
				if (dr["PRODUCT_LEVEL_DELIMITER"] != DBNull.Value)
				{
					ProductLevelDelimiter = Convert.ToChar(dr["PRODUCT_LEVEL_DELIMITER"], CultureInfo.CurrentCulture);
				}
				else
				{
					ProductLevelDelimiter = '\\';
				}

				// purge tab
				this.PurgeAllocationsPeriod = Convert.ToInt32(dr["PURGE_ALLOCATIONS"], CultureInfo.CurrentUICulture);

				// product & stores tab
				this.ProductLevelDisplay = (eHierarchyDisplayOptions)Convert.ToInt32(dr["PRODUCT_LEVEL_DISPLAY_ID"], CultureInfo.CurrentUICulture);
				this.StoreDisplay = (eStoreDisplayOptions)Convert.ToInt32(dr["STORE_DISPLAY_OPTION_ID"], CultureInfo.CurrentUICulture);
				this.OTSPlanStoreGroupRID = Convert.ToInt32(dr["DEFAULT_OTS_SG_RID"], CultureInfo.CurrentUICulture);
				this.AllocationStoreGroupRID = Convert.ToInt32(dr["DEFAULT_ALLOC_SG_RID"], CultureInfo.CurrentUICulture);

				this.NewStorePeriodBegin = (dr["NEW_STORE_TIMEFRAME_BEGIN"] == DBNull.Value) ? 
					Include.UndefinedNewStorePeriodBegin : Convert.ToInt32(dr["NEW_STORE_TIMEFRAME_BEGIN"], CultureInfo.CurrentUICulture);
				this.NewStorePeriodEnd = (dr["NEW_STORE_TIMEFRAME_END"] == DBNull.Value) ? 
					Include.UndefinedNewStorePeriodEnd : Convert.ToInt32(dr["NEW_STORE_TIMEFRAME_END"], CultureInfo.CurrentUICulture);
				this.NonCompStorePeriodBegin = (dr["NON_COMP_STORE_TIMEFRAME_BEGIN"] == DBNull.Value) ? 
					Include.UndefinedNonCompStorePeriodBegin : Convert.ToInt32(dr["NON_COMP_STORE_TIMEFRAME_BEGIN"], CultureInfo.CurrentUICulture);
				this.NonCompStorePeriodEnd = (dr["NON_COMP_STORE_TIMEFRAME_END"] == DBNull.Value) ? 
					Include.UndefinedNonCompStorePeriodEnd : Convert.ToInt32(dr["NON_COMP_STORE_TIMEFRAME_END"], CultureInfo.CurrentUICulture);

				// Allocation Defaults
				this.PercentNeedLimit = (dr["DEFAULT_PCT_NEED_LIMIT"] == DBNull.Value) ? 
					Include.DefaultPercentNeedLimit : Convert.ToDouble(dr["DEFAULT_PCT_NEED_LIMIT"], CultureInfo.CurrentUICulture);
				this.BalanceTolerancePercent = (dr["DEFAULT_BALANCE_TOLERANCE"] == DBNull.Value) ? 
					Include.DefaultBalanceTolerancePercent : Convert.ToDouble(dr["DEFAULT_BALANCE_TOLERANCE"], CultureInfo.CurrentUICulture);
				this.PackSizeErrorPercent = (dr["DEFAULT_PACK_SIZE_ERROR_PCT"] == DBNull.Value) ? 
					Include.DefaultPackSizeErrorPercent : Convert.ToDouble(dr["DEFAULT_PACK_SIZE_ERROR_PCT"], CultureInfo.CurrentUICulture);
				this.MaxSizeErrorPercent = (dr["DEFAULT_MAX_SIZE_ERROR_PCT"] == DBNull.Value) ? 
					Include.DefaultMaxSizeErrorPercent : Convert.ToDouble(dr["DEFAULT_MAX_SIZE_ERROR_PCT"], CultureInfo.CurrentUICulture);
				this.FillSizeHolesPercent = (dr["DEFAULT_FILL_SIZE_HOLES_PCT"] == DBNull.Value) ? 
					Include.DefaultFillSizeHolesPercent : Convert.ToDouble(dr["DEFAULT_FILL_SIZE_HOLES_PCT"], CultureInfo.CurrentUICulture);
				this.StoreGradePeriod = Convert.ToInt32(dr["STORE_GRADE_TIMEFRAME"], CultureInfo.CurrentUICulture);
				this.SizeBreakoutInd = (dr["SIZE_BREAKOUT_IND"].ToString() == "1");
//				this.SizeNeedInd = (dr["SIZE_NEED_IND"].ToString() == "1");
				this.BulkIsDetail = (dr["BULK_IS_DETAIL_IND"].ToString() == "1");
				this.ProtectInterfacedHeadersInd = (dr["PROTECT_IF_HDRS_IND"].ToString() == "1");
				this.ReserveStoreRID = (dr["RESERVE_ST_RID"] == DBNull.Value) ? 
					Include.UndefinedStoreRID : Convert.ToInt32(dr["RESERVE_ST_RID"], CultureInfo.CurrentUICulture);
				this.NumberOfStoreDataTables = (dr["STORE_TABLE_COUNT"] == DBNull.Value) ? 
					1 : Convert.ToInt32(dr["STORE_TABLE_COUNT"], CultureInfo.CurrentUICulture);
				this.UseWindowsLogin = (dr["USE_WINDOWS_LOGIN"].ToString() == "1");
				this.ShippingHorizonWeeks = (dr["SHIPPING_HORIZON_WEEKS"] == DBNull.Value) ? 
					0 : Convert.ToInt32(dr["SHIPPING_HORIZON_WEEKS"], CultureInfo.CurrentUICulture);
				// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
				this.NormalizeSizeCurves = (dr["NORMALIZE_SIZE_CURVES_IND"] == DBNull.Value) ? 
					true : Include.ConvertCharToBool(Convert.ToChar(dr["NORMALIZE_SIZE_CURVES_IND"], CultureInfo.CurrentUICulture));
				// END MID Track #4826
				// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
				this.FillSizesToType = (dr["FILL_SIZES_TO_TYPE"] == DBNull.Value) ? 
					eFillSizesToType.Holes : (eFillSizesToType)Convert.ToInt32(dr["FILL_SIZES_TO_TYPE"], CultureInfo.CurrentUICulture);
			// End MID Track #4921 

		       // BEGIN MID Track #6335 Option to not Release Hdr with all units in reserve
		       this.AllowReleaseIfAllUnitsInReserve = (dr["ALLOW_RLSE_IF_ALL_IN_RSRV_IND"] == DBNull.Value) ?
				   true : Include.ConvertCharToBool(Convert.ToChar(dr["ALLOW_RLSE_IF_ALL_IN_RSRV_IND"], CultureInfo.CurrentUICulture));
               // END MID Track #6335 Option to not Release Hdr with all units in reserve

				// Begin TT#391 - stodd - add for use with in stock sales
			   this.GenerateSizeCurveUsing = (eGenerateSizeCurveUsing)Convert.ToInt32(dr["GEN_SIZE_CURVE_USING"]);
				// End TT#391 - stodd - add for use with in stock sales
				
				 // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
               this.GenericPackRounding1stPackPct = (dr["GENERIC_PACK_ROUNDING_1ST_PACK_PCT"] == DBNull.Value) ?
                    Include.DefaultGenericPackRounding1stPackPct : Convert.ToDouble(dr["GENERIC_PACK_ROUNDING_1ST_PACK_PCT"], CultureInfo.CurrentUICulture);

               this.GenericPackRoundingNthPackPct = (dr["GENERIC_PACK_ROUNDING_NTH_PACK_PCT"] == DBNull.Value) ?
                   Include.DefaultGenericPackRoundingNthPackPct : Convert.ToDouble(dr["GENERIC_PACK_ROUNDING_NTH_PACK_PCT"], CultureInfo.CurrentUICulture);
               // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)

               // BEGIN TT#1401 - AGallagher - Reservation Stores
               this.AllowStoreMaxValueModification = (dr["ALLOW_STORE_MAX_VALUE_MODIFICATION"] == DBNull.Value) ?
                     true : (Convert.ToBoolean(dr["ALLOW_STORE_MAX_VALUE_MODIFICATION"], CultureInfo.CurrentUICulture));
               // END TT#1401 - AGallagher - Reservation Stores

               // BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options
               this.VSWSizeConstraints = (eVSWSizeConstraints)Convert.ToInt32(dr["VSW_SIZE_CONSTRAINTS"]);
               // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options

               this.RI_EXPAND = (dr["RI_EXPAND_IND"].ToString() == "1"); // TT#628-MD - JSmith -  Add Option To Not Expand Allocation Records Once Relieved

				_headerTypeProfileList = new HeaderTypeProfileList(eProfileType.HeaderType);
				dt = opts.GetHeaderReleaseTypes();
				foreach (DataRow releaseRow in dt.Rows)
				{
					int headerType = Convert.ToInt32(releaseRow["HEADER_TYPE"], CultureInfo.CurrentUICulture);
					HeaderTypeProfile hrp = new HeaderTypeProfile(headerType);
					hrp.HeaderTypeName = Convert.ToString(releaseRow["TEXT_VALUE"], CultureInfo.CurrentUICulture);
					hrp.ReleaseHeaderType = Include.ConvertCharToBool(Convert.ToChar(releaseRow["RELEASE_HEADER_TYPE_IND"], CultureInfo.CurrentUICulture));
					_headerTypeProfileList.Add(hrp);
				}

				_appConfig = new AppConfig();
				if (dr["LICENSE_KEY"] == DBNull.Value)
				{
					_appConfig.SetLicenseKey(string.Empty);
				}
				else
				{
					_appConfig.SetLicenseKey(Convert.ToString(dr["LICENSE_KEY"], CultureInfo.CurrentCulture));
				}

				// Begin - John Smith - Linked headers
				this.HeaderLinkCharacteristicKey = (dr["HEADER_LINK_CHARACTERISTIC"] == DBNull.Value) ? 
					Include.NoRID : Convert.ToInt32(dr["HEADER_LINK_CHARACTERISTIC"], CultureInfo.CurrentUICulture);
				
				if (HeaderLinkCharacteristicKey > Include.NoRID)
				{
					HeaderCharacteristicsData headerCharacteristicsData = new HeaderCharacteristicsData();
					DataTable HCGdt = headerCharacteristicsData.HeaderCharGroup_Read(HeaderLinkCharacteristicKey);
					DataRow HCGdr = HCGdt.Rows[0];
					this.HeaderLinkCharacteristicValue = (HCGdr["HCG_ID"] == DBNull.Value) ? 
						string.Empty : Convert.ToString(HCGdr["HCG_ID"], CultureInfo.CurrentUICulture);
				}
				else
				{
					this.HeaderLinkCharacteristicValue = string.Empty;
				}
				// End - Linked headers
				
				// BEGIN ANF Generic SIze Constraints 
				this.SizeCurveCharMask = dr["SIZE_CURVE_CHARMASK"].ToString();
				this.SizeGroupCharMask = dr["SIZE_GROUP_CHARMASK"].ToString();
				this.SizeAlternateCharMask = dr["SIZE_ALTERNATE_CHARMASK"].ToString();
				this.SizeConstraintCharMask = dr["SIZE_CONSTRAINT_CHARMASK"].ToString();
				// END ANF Generic SIze Constraints 

                // BEGIN MID Track #6043 - KJohnson - Forcast Chain WOS Calculations
                this.NumberOfWeeksWithZeroSales = Convert.ToInt32(dr["NUMBER_OF_WEEKS_WITH_ZERO_SALES"], CultureInfo.CurrentUICulture);
                this.MaximumChainWOS = Convert.ToInt32(dr["MAXIMUM_CHAIN_WOS"], CultureInfo.CurrentUICulture);
                this.ProrateChainStock = (dr["PRORATE_CHAIN_STOCK"].ToString() == "1");
                // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                this.GenericSizeCurveNameType = (eGenericSizeCurveNameType)Convert.ToInt32(dr["GENERIC_SIZE_CURVE_NAME_TYPE"]); 
                // End TT#413
                // begin TT#1365 - JEllis - FL Detail Packs Size Need Enhancement
                _packToleranceNoMaxStep = (dr["PACK_TOLERANCE_NO_MAX_STEP_IND"].ToString() == "1");
                _packToleranceStepped = (dr["PACK_TOLERANCE_STEPPED_IND"].ToString() == "1");
                // end TT#1365 - JEllis - FL Detail Packs Size Need Enhancement
			    // END MID Track #6043 - KJohnson
				// Begin Track #6074 stodd
				// begin TT#12 - stodd -
				//_defaultGradesByBasis = Include.ConvertCharToBool(Convert.ToChar(dr["DEFAULT_GRADES_BY_BASIS_IND"]));
				// End TT#12 - stodd -
				// End Track #6074 stodd
                // Begin TT#2170 - JSmith - Daily Size Purge Not Working
                _performOnetimePurge = (dr["PERFORM_ONETIME_SPECIAL_PURGE_IND"].ToString() == "1");
				// BEGIN TT#739-MD - STodd - delete stores
				_isStoreDeleteInProgress = (dr["STORE_DELETE_IN_PROGRESS_IND"] == DBNull.Value) ? false : Include.ConvertCharToBool(Convert.ToChar(dr["STORE_DELETE_IN_PROGRESS_IND"]));
				_constraintsDropped = (dr["CONSTRAINTS_DROPPED"] == DBNull.Value) ? false : Include.ConvertCharToBool(Convert.ToChar(dr["CONSTRAINTS_DROPPED"]));
                // END TT#739-MD - STodd - delete stores
                // End TT#2170
                _enableVelocityGradeOptions = (dr["ENABLE_VELOCITY_GRADE_OPTIONS"].ToString() == "1"); //TT#855-MD -jsobek -Velocity Enhancements
                _vswItemFWOSMax = (eVSWItemFWOSMax)(Convert.ToInt32(dr["VSW_ITEM_FWOS_MAX_IND"]));  // TT#933-MD - AGallagher - Item Max vs. FWOS Max

                _forceSingleClientInstance = (dr["FORCE_SINGLE_CLIENT_INSTANCE"].ToString() == "1"); //TT#894-MD -jsobek -Single Client Instance System Option
                _forceSingleUserInstance = (dr["FORCE_SINGLE_USER_INSTANCE"].ToString() == "1"); //TT#898-MD -jsobek -Single User Instance System Option
                _useActiveDirectoryAuthentication = (dr["USE_ACTIVE_DIRECTORY_AUTHENTICATION"].ToString() == "1"); //TT#1521-MD -jsobek -Active Directory Authentication
                _useActiveDirectoryAuthenticationWithDomain = (dr["USE_ACTIVE_DIRECTORY_AUTHENTICATION_WITH_DOMAIN"].ToString() == "1"); //TT#1521-MD -jsobek -Active Directory Authentication
                _vswItemFWOSMax = (eVSWItemFWOSMax)(Convert.ToInt32(dr["VSW_ITEM_FWOS_MAX_IND"]));  // TT#933-MD - AGallagher - Item Max vs. FWOS Max
                // Begin TT#1652-MD - RMatelic - DC Carton Rounding  
                if (dr["DC_CARTON_ROUNDING_SG_RID"] == DBNull.Value)
                {
                    this.DCCartonRoundingSGRid = Include.NoRID;
                }
                else
                {
                    this.DCCartonRoundingSGRid = Convert.ToInt32(dr["DC_CARTON_ROUNDING_SG_RID"], CultureInfo.CurrentUICulture);
                }
                // End TT#1652-MD
				_priorHeaderIncludeReserve = Convert.ToBoolean(Convert.ToInt64(dr["PRIOR_HEADER_INCLUDE_RESERVE_IND"])); //TT#1608-MD - SRisch - Prior Header
                // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
                _split_Option = (eDCFulfillmentSplitOption)(Convert.ToInt32(dr["SPLIT_OPTION"]));

                _apply_Minimums_Ind = Convert.ToChar(dr["APPLY_MINIMUMS_IND"], CultureInfo.CurrentUICulture);
                
                if (dr["PRIORITIZE_TYPE"] == DBNull.Value)
                { _prioritize_Type = 'H'; }
                else
                { _prioritize_Type = Convert.ToChar(dr["PRIORITIZE_TYPE"], CultureInfo.CurrentUICulture); }

                if (dr["HEADER_FIELD"] == DBNull.Value)
                { _header_Field = -7; }
                else
                { _header_Field = Convert.ToInt32(dr["HEADER_FIELD"], CultureInfo.CurrentUICulture); }

                if (dr["HCG_RID"] == DBNull.Value)
                { _hcg_Rid = -1; }
                else
                { _hcg_Rid = Convert.ToInt32(dr["HCG_RID"], CultureInfo.CurrentUICulture); }

                _header_Order = (eDCFulfillmentHeadersOrder)(Convert.ToInt32(dr["HEADERS_ORDER"]));
                
                _store_Order = (eDCFulfillmentStoresOrder)(Convert.ToInt32(dr["STORES_ORDER"]));

                _split_By_Option = (eDCFulfillmentSplitByOption)(Convert.ToInt32(dr["SPLIT_BY_OPTION"]));

                _split_By_Reserve = (eDCFulfillmentReserve)(Convert.ToInt32(dr["SPLIT_BY_RESERVE"]));

                _apply_By = (eDCFulfillmentMinimums)(Convert.ToInt32(dr["APPLY_BY"]));

                _within_Dc = (eDCFulfillmentWithinDC)(Convert.ToInt32(dr["APPLY_BY"]));
                // END TT#1966-MD - AGallagher - DC Fulfillment
			}
			catch ( Exception innerException )
			{
				throw new MIDException(eErrorLevel.severe,0,"Error loading global options", innerException);
			}
		}

		public bool IsReleaseable(eHeaderType aHeaderType)
		{
			try
			{
				bool releaseHeader = true;
				HeaderTypeProfile hrp = (HeaderTypeProfile)_headerTypeProfileList.FindKey(Convert.ToInt32(aHeaderType,CultureInfo.CurrentCulture));
				if (hrp != null)
				{
					releaseHeader = hrp.ReleaseHeaderType;
				}
				return releaseHeader;
			}
			catch
			{
				throw;
			}
		}

	}

	/// <summary>
	/// Contains information by header type
	/// </summary>
	/// <remarks>
	/// The key is the integer value of eHeaderType.
	/// </remarks>
	[Serializable()]
	public class HeaderTypeProfile : Profile 
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HeaderTypeProfile(int aKey)
			: base(aKey)
		{
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.HeaderType;
			}
		}

		#region Internal Fields
		private string _headerTypeName;
		private bool _releaseHeaderType;
		#endregion

		public string HeaderTypeName
		{
			get	{ return _headerTypeName;}
			set	{ _headerTypeName = value;	}
		}
		public bool ReleaseHeaderType
		{
			get { return _releaseHeaderType; }
			set { _releaseHeaderType = value; }
		}

		public override string ToString()
		{
			return _headerTypeName;
		}
	}

	/// <summary>
	/// Used to retrieve a list of header release profiles
	/// </summary>
	[Serializable()]
	public class HeaderTypeProfileList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HeaderTypeProfileList(eProfileType aProfileType)
			: base(aProfileType)
		{
			
		}
	}

	/// <summary>
	/// Summary description for Global_Options.
	/// </summary>
	[Serializable()]
	public class UserOptionsProfile : Profile 
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public UserOptionsProfile(int aKey)
			: base(aKey)
		{
			_userRID = aKey;
			_auditLoggingLevel = eMIDMessageLevel.Information;
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.UserOptions;
			}
		}

		#region Internal Fields
		private int _userRID;
		private string _myHierarchyName;
		private string _myHierarchyColor;
		private string _myWorkflowMethods;
		private int _themeRID = Include.NoRID;
		private bool _showLogin;
		private bool _forecastMonitorIsActive;
		private string _forecastMonitorDirectory;
        // Begin TT#1966-MD - JSmith - DC Fulfillment
        private bool _dcfulfillmentMonitorIsActive;
        private string _dcfulfillmentMonitorDirectory;
        // End TT#1966-MD - JSmith - DC Fulfillment
		private eForecastMonitorMode _forecastMonitorMode;
		private bool _modifySalesMonitorIsActive;
		private string _modifySalesMonitorDirectory;
		private eMIDMessageLevel _auditLoggingLevel;

        //BEGIN TT#46-MD -jsobek -Develop My Activity Log
        private bool _userActivityOptionShowChart=false;
        private int _userActivityOptionMessageLevel=2; //Information
        private int _userActivityOptionMaxMessages=5000;
        //END TT#46-MD -jsobek -Develop My Activity Log
        private bool _showSignOffPrompt;    // TT#4243 - RMatelic - Add a prompt when the user closes the MID app using the "x" in the top right corner

		#endregion

		public string MyHierarchyName
		{
			get	{ return _myHierarchyName;}
			set	{ _myHierarchyName = value;	}
		}
		public string MyHierarchyColor
		{
			get { return _myHierarchyColor; }
			set { _myHierarchyColor = value; }
		}
		public string MyWorkflowMethods
		{
			get { return _myWorkflowMethods; }
			set { _myWorkflowMethods = value; }
		}
		public int ThemeRID
		{
			get { return _themeRID; }
			set { _themeRID = value; }
		}
		public bool ShowLogin
		{
			get { return _showLogin; }
			set { _showLogin = value; }
		}
		public bool ForecastMonitorIsActive
		{
			get{return _forecastMonitorIsActive;}
			set{_forecastMonitorIsActive=value;}
		}
		public string ForecastMonitorDirectory
		{
			get{return _forecastMonitorDirectory;}
			set{_forecastMonitorDirectory = value;}
		}
        // Begin TT#1966-MD - JSmith - DC Fulfillment
        public bool DCFulfillmentMonitorIsActive
        {
            get { return _dcfulfillmentMonitorIsActive; }
            set { _dcfulfillmentMonitorIsActive = value; }
        }
        public string DCFulfillmentMonitorDirectory
        {
            get { return _dcfulfillmentMonitorDirectory; }
            set { _dcfulfillmentMonitorDirectory = value; }
        }
        // End TT#1966-MD - JSmith - DC Fulfillment
		public eForecastMonitorMode ForecastMonitorMode
		{
			get{return _forecastMonitorMode;}
			set{_forecastMonitorMode = value;}
		}
		public bool ModifySalesMonitorIsActive
		{
			get{return _modifySalesMonitorIsActive;}
			set{_modifySalesMonitorIsActive=value;}
		}
		public string ModifySalesMonitorDirectory
		{
			get{return _modifySalesMonitorDirectory;}
			set{_modifySalesMonitorDirectory = value;}
		}
		public eMIDMessageLevel AuditLoggingLevel
		{
			get{return _auditLoggingLevel;}
			set{_auditLoggingLevel = value;}
		}

        // Begin TT#4243 - RMatelic - Add a prompt when the user closes the MID app using the "x" in the top right corner
        public bool ShowSignOffPrompt
        {
            get { return _showSignOffPrompt; }
            set { _showSignOffPrompt = value; }
        }
        // End TT#4243
		public void LoadOptions()
		{
			SecurityAdmin securityAdmin = null;
			DataTable dt = null;
			
			try
			{
				securityAdmin = new SecurityAdmin();
				dt = securityAdmin.GetUserOptions(_userRID);

				if (dt.Rows.Count == 0)
				{
					_myHierarchyName = "My Hierarchies";
					_myHierarchyColor = Include.MIDDefault;
					_myWorkflowMethods = "My Workflow/Methods";
					MIDUserInfo userInfo = new MIDUserInfo();
                    // Begin Track #5755 - JSmith - Windows login changes
                    //_showLogin = userInfo.ShowLogin;
                    _showLogin = false;
                    // End Track #5755
					string appSetting = MIDConfigurationManager.AppSettings["MonitorForecast"];
					try
					{
						_forecastMonitorIsActive = Convert.ToBoolean(appSetting);
					}
					catch
					{
						_forecastMonitorIsActive = false;
					}
					_forecastMonitorDirectory = MIDConfigurationManager.AppSettings["MonitorForecastFilePath"];
					_modifySalesMonitorIsActive = _forecastMonitorIsActive;
					_modifySalesMonitorDirectory = _forecastMonitorDirectory;
					string forecastMonitorMode = MIDConfigurationManager.AppSettings["MonitorForecastMode"];
					if (forecastMonitorMode != null)
					{
						if (forecastMonitorMode.ToUpper(CultureInfo.CurrentUICulture) == "VERBOSE")
						{
							ForecastMonitorMode = eForecastMonitorMode.Verbose;
						}
						else
						{
							ForecastMonitorMode = eForecastMonitorMode.Default;
						}
					}
					else
					{
						ForecastMonitorMode = eForecastMonitorMode.Default;
					}

                    // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
                    _dcfulfillmentMonitorDirectory = MIDConfigurationManager.AppSettings["MonitorDCFulfillmentFilePath"];
                    string dcfulfillmentMonitorMode = MIDConfigurationManager.AppSettings["MonitorDCFulfillment"];
                    try
                    {
                        _dcfulfillmentMonitorIsActive = Convert.ToBoolean(appSetting);
                    }
                    catch
                    {
                        _dcfulfillmentMonitorIsActive = false;
                    }
                    // END TT#1966-MD - AGallagher - DC Fulfillment
					DetermineAuditLoggingLevel();
                    _showSignOffPrompt = false;      // TT#4243 - RMatelic - Add a prompt when the user closes the MID app using the "x" in the top right corner
				}
				else
				{
					DataRow dr = dt.Rows[0];

					if ((string)dr["MY_HIERARCHY"] == " ")
					{
						_myHierarchyName = "My Hierarchies";
					}
					else
					{
						_myHierarchyName = (string)dr["MY_HIERARCHY"];
					}

					if ((string)dr["MY_HIERARCHY_COLOR"] == " ")
					{
						_myHierarchyColor = Include.MIDDefault;
					}
					else
					{
						_myHierarchyColor = (string)dr["MY_HIERARCHY_COLOR"];
					}

					if (dr["MY_WORKFLOWMETHODS"] == System.DBNull.Value)
					{
						_myWorkflowMethods = "My Workflow/Methods";
					}
					else
					{
						_myWorkflowMethods = (string)dr["MY_WORKFLOWMETHODS"];
					}

					if (dr["THEME_RID"] == System.DBNull.Value)
					{
						_themeRID = Include.Undefined;
					}
					else
					{
						_themeRID = System.Convert.ToInt32(dr["THEME_RID"], CultureInfo.CurrentUICulture);
					}

					if (dr["SHOW_LOGIN"] == System.DBNull.Value)
					{
                        // Begin Track #5755 - JSmith - Windows login changes
                        //MIDUserInfo userInfo = new MIDUserInfo();
                        //_showLogin = userInfo.ShowLogin;
                        _showLogin = false;
                        // End Track #5755
					}
					else
					{
						_showLogin = Include.ConvertCharToBool(System.Convert.ToChar(dr["SHOW_LOGIN"], CultureInfo.CurrentUICulture));
					}

					if (dr["FORECAST_MONITOR_ACTIVE"] == System.DBNull.Value)
					{
						string appSetting = MIDConfigurationManager.AppSettings["MonitorForecast"];
						try
						{
							_forecastMonitorIsActive = Convert.ToBoolean(appSetting);
						}
						catch
						{
							_forecastMonitorIsActive = false;
						}
					}
					else
					{
						_forecastMonitorIsActive = Include.ConvertCharToBool(System.Convert.ToChar(dr["FORECAST_MONITOR_ACTIVE"], CultureInfo.CurrentUICulture));
					}

					string forecastMonitorMode = MIDConfigurationManager.AppSettings["MonitorForecastMode"];
					if (forecastMonitorMode != null)
					{
						if (forecastMonitorMode.ToUpper(CultureInfo.CurrentUICulture) == "VERBOSE")
						{
							ForecastMonitorMode = eForecastMonitorMode.Verbose;
						}
						else
						{
							ForecastMonitorMode = eForecastMonitorMode.Default;
						}
					}
					else
					{
						ForecastMonitorMode = eForecastMonitorMode.Default;
					}

					if (dr["FORECAST_MONITOR_DIRECTORY"] == System.DBNull.Value)
					{
						_forecastMonitorDirectory = MIDConfigurationManager.AppSettings["MonitorForecastFilePath"];;
					}
					else
					{
						_forecastMonitorDirectory = System.Convert.ToString(dr["FORECAST_MONITOR_DIRECTORY"], CultureInfo.CurrentUICulture);
					}

					if (dr["MODIFY_SALES_MONITOR_ACTIVE"] == System.DBNull.Value)
					{
						_modifySalesMonitorIsActive = _forecastMonitorIsActive;
					}
					else
					{
						_modifySalesMonitorIsActive = Include.ConvertCharToBool(System.Convert.ToChar(dr["MODIFY_SALES_MONITOR_ACTIVE"], CultureInfo.CurrentUICulture));
					}

					if (dr["MODIFY_SALES_MONITOR_DIRECTORY"] == System.DBNull.Value)
					{
						_modifySalesMonitorDirectory = _forecastMonitorDirectory;
					}
					else
					{
						_modifySalesMonitorDirectory = System.Convert.ToString(dr["MODIFY_SALES_MONITOR_DIRECTORY"], CultureInfo.CurrentUICulture);
					}

                    // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
                    if (dr["DCFULFILLMENT_MONITOR_ACTIVE"] == System.DBNull.Value)
                    {
                        string DCFappSetting = MIDConfigurationManager.AppSettings["MonitorDCFulfillment"];
                        try
                        {
                            _dcfulfillmentMonitorIsActive = Convert.ToBoolean(DCFappSetting);
                        }
                        catch
                        {
                            _dcfulfillmentMonitorIsActive = false;
                        }
                    }
                    else
                    {
                        _dcfulfillmentMonitorIsActive = Include.ConvertCharToBool(System.Convert.ToChar(dr["DCFULFILLMENT_MONITOR_ACTIVE"], CultureInfo.CurrentUICulture));
                    }

                    if (dr["DCFULFILLMENT_MONITOR_DIRECTORY"] == System.DBNull.Value)
                    {
                        _dcfulfillmentMonitorDirectory = MIDConfigurationManager.AppSettings["MonitorDCFulfillmentFilePath"];
                    }
                    else
                    {
                        _dcfulfillmentMonitorDirectory = System.Convert.ToString(dr["DCFULFILLMENT_MONITOR_DIRECTORY"], CultureInfo.CurrentUICulture);
                    }
                    // END TT#1966-MD - AGallagher - DC Fulfillment
    				if (dr["AUDIT_LOGGING_LEVEL"] == System.DBNull.Value)
					{
						DetermineAuditLoggingLevel();
					}
					else
					{
						_auditLoggingLevel = (eMIDMessageLevel)System.Convert.ToInt32(dr["AUDIT_LOGGING_LEVEL"], CultureInfo.CurrentUICulture);
					}

                    //BEGIN TT#46-MD -jsobek -Develop My Activity Log
                    //if (dr["ACTIVITY_SHOW_CHART"] != System.DBNull.Value)
                    //{
                    //    _userActivityOptionShowChart = Include.ConvertCharToBool(System.Convert.ToChar(dr["ACTIVITY_SHOW_CHART"], CultureInfo.CurrentUICulture));
                    //}
                    //if (dr["ACTIVITY_MSG_LEVEL"] != System.DBNull.Value)
                    //{
                    //    _userActivityOptionMessageLevel = System.Convert.ToInt32(System.Convert.ToChar(dr["ACTIVITY_MSG_LEVEL"], CultureInfo.CurrentUICulture));
                    //}
                    //if (dr["ACTIVITY_MAX_MSG"] != System.DBNull.Value)
                    //{
                    //    _userActivityOptionMaxMessages = System.Convert.ToInt32(System.Convert.ToChar(dr["ACTIVITY_MAX_MSG"], CultureInfo.CurrentUICulture));
                    //}
                    //END TT#46-MD -jsobek -Develop My Activity Log

                    // Begin TT#4243 - RMatelic - Add a prompt when the user closes the MID app using the "x" in the top right corner
                    if (dr["SHOW_SIGNOFF_PROMPT"] == System.DBNull.Value)
                    {
                        _showSignOffPrompt = false; 
                    }
                    else
                    {
                        _showSignOffPrompt = Include.ConvertCharToBool(System.Convert.ToChar(dr["SHOW_SIGNOFF_PROMPT"], CultureInfo.CurrentUICulture));
                    }
                    // End TT#4243  
				}

                // Begin TT#1966-MD - JSmith - DC Fulfillment
                //string DCAppSetting = MIDConfigurationManager.AppSettings["MonitorDCFulfillment"];
                //try
                //{
                //    _dcfulfillmentMonitorIsActive = Convert.ToBoolean(DCAppSetting);
                //}
                //catch
                //{
                //    _dcfulfillmentMonitorIsActive = false;
                //}
                //_dcfulfillmentMonitorDirectory = MIDConfigurationManager.AppSettings["MonitorDCFulfillmentFilePath"];
                // End TT#1966-MD - JSmith - DC Fulfillment
               
			}
			catch 
			{
				throw;
			}
		}
        
		private void DetermineAuditLoggingLevel()
		{
			string loggingLevel = MIDConfigurationManager.AppSettings["AuditLoggingLevel"];
			if (loggingLevel != null)
			{
				switch (loggingLevel.Trim().ToUpper())
				{
					case "DEBUG":
						_auditLoggingLevel = eMIDMessageLevel.Debug;
						break;
					case "INFORMATION":
						_auditLoggingLevel = eMIDMessageLevel.Information;
						break;
					case "EDIT":
						_auditLoggingLevel = eMIDMessageLevel.Edit;
						break;
					case "WARNING":
						_auditLoggingLevel = eMIDMessageLevel.Warning;
						break;
					case "ERROR":
						_auditLoggingLevel = eMIDMessageLevel.Error;
						break;
					case "SEVERE":
						_auditLoggingLevel = eMIDMessageLevel.Severe;
						break;
					default:
						_auditLoggingLevel = eMIDMessageLevel.Edit;
						break;

				}
			}
		}

		public void UpdateMyHierarchy(string myHierarchyName, string myHierarchyColor)
		{
			try
			{
				// default parameters if not provided
				if (myHierarchyName == null)
				{
					myHierarchyName = _myHierarchyName;
				}
				if (myHierarchyColor == null)
				{
					myHierarchyColor = _myHierarchyColor;
				}

				SecurityAdmin securityAdmin = new SecurityAdmin();
				securityAdmin.OpenUpdateConnection();

				try
				{
					securityAdmin.UpdateMyHierarchy(_userRID, myHierarchyName, myHierarchyColor);
					securityAdmin.CommitData();

					_myHierarchyName = myHierarchyName;
					_myHierarchyColor = myHierarchyColor;
				}
				catch (Exception err)
				{
					securityAdmin.Rollback();
					string message = err.ToString();
					throw;
				}
				finally
				{
					securityAdmin.CloseUpdateConnection();
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void UpdateMyWorkflowMethodsText(string myWorkflowMethods)
		{
			try
			{
				SecurityAdmin securityAdmin = new SecurityAdmin();
				securityAdmin.OpenUpdateConnection();

				try
				{
					securityAdmin.UpdateMyWorkflowMethodsText(_userRID, myWorkflowMethods);
					_myWorkflowMethods = myWorkflowMethods;
					securityAdmin.CommitData();
				}
				catch ( Exception err )
				{
					securityAdmin.Rollback();
					string message = err.ToString();
					throw;
				}
				finally
				{
					securityAdmin.CloseUpdateConnection();
				}

				_myWorkflowMethods = myWorkflowMethods;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void UpdateShowLogin(bool aShowLogin)
		{
			try
			{
				SecurityAdmin securityAdmin = new SecurityAdmin();
				securityAdmin.OpenUpdateConnection();

				try
				{
					securityAdmin.UpdateShowLogin(_userRID, aShowLogin);
					securityAdmin.CommitData();
				}
				catch ( Exception err )
				{
					securityAdmin.Rollback();
					string message = err.ToString();
					throw;
				}
				finally
				{
					securityAdmin.CloseUpdateConnection();
				}

				_showLogin = aShowLogin;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

	}
}
