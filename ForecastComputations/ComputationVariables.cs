using System;
using System.Collections;
using System.Data;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business;

namespace MIDRetail.ForecastComputations
{
	/// <summary>
	/// The Variables class is where the Variable profiles are defined.
	/// </summary>
	/// <remarks>
	/// This class is where the Variable profiles are defined.  A ProfileList of all the Variables, a hash table for the Variables'
	/// name, and a TimeTotalVariable to Variable profile cross-reference are built during contruction.
	/// </remarks>

	[Serializable]
	abstract public class BasePlanVariables : IPlanComputationVariables
	{
		//=======
		// FIELDS
		//=======

		private bool _customVariablesDefined;

		private ProfileList _profileList;
		private ProfileList _orderedVariableList = null;
		private ProfileList _onHandList;
		private ProfileList _totalWeekToDaySalesList;
		private ProfileList _regularWeekToDaySalesList;
		private ProfileList _promoWeekToDaySalesList;
		private ProfileList _regPromoWeekToDaySalesList;
		private ProfileList _markdownWeekToDaySalesList;
		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		private ProfileList _assortmentList;
		//End TT#2 - JScott - Assortment Planning - Phase 2
		private ProfileXRef _timeTotalXRef;
		private int _maxTimeTotalVariables;
		private int _maxStoreTimeTotalVariables;
		private int _maxChainTimeTotalVariables;
		private int _maxDatabaseVariables;
		private Hashtable _nameHash;
		private Hashtable _databaseColumnHash = null;
		private int _seq;
		private int _futureSeq;
		ProfileList _databaseVariableList;
		ProfileList _storeWeeklyHistoryDatabaseVariableList = null;
		ProfileList _storeDailyHistoryDatabaseVariableList = null;
		ProfileList _storeWeeklyForecastDatabaseVariableList = null;
		ProfileList _chainWeeklyHistoryDatabaseVariableList = null;
		ProfileList _chainWeeklyForecastDatabaseVariableList = null;
        protected ArrayList _variableGroupings = null;
		
		public VariableProfile SalesTotalUnits;
		public VariableProfile SalesRegularUnits;
		public VariableProfile SalesPromoUnits;
		public VariableProfile SalesRegPromoUnits;
		public VariableProfile SalesMarkdownUnits;
		public VariableProfile SalesTotalSetIndex;
		public VariableProfile SalesRegularSetIndex;
		public VariableProfile SalesPromoSetIndex;
		public VariableProfile SalesRegPromoSetIndex;
		public VariableProfile SalesMarkdownSetIndex;
		public VariableProfile SalesTotalAllStoreIndex;
		public VariableProfile SalesRegularAllStoreIndex;
		public VariableProfile SalesPromoAllStoreIndex;
		public VariableProfile SalesRegPromoAllStoreIndex;
		public VariableProfile SalesMarkdownAllStoreIndex;
		public VariableProfile InventoryTotalUnits;
		public VariableProfile InventoryRegularUnits;
		public VariableProfile InventoryMarkdownUnits;
		public VariableProfile InventoryTotalSetIndex;
		public VariableProfile InventoryRegularSetIndex;
		public VariableProfile InventoryMarkdownSetIndex;
		public VariableProfile InventoryTotalAllStoreIndex;
		public VariableProfile InventoryRegularAllStoreIndex;
		public VariableProfile InventoryMarkdownAllStoreIndex;
		public VariableProfile ReceiptTotalUnits;
		public VariableProfile ReceiptRegularUnits;
		public VariableProfile ReceiptMarkdownUnits;
		public VariableProfile SalesStockRatioTotal;
		public VariableProfile SalesStockRatioRegPromo;
		public VariableProfile SalesStockRatioMarkdown;
		public VariableProfile ForwardWOSTotal;
		public VariableProfile ForwardWOSRegPromo;
		public VariableProfile ForwardWOSMarkdown;
		public VariableProfile ForwardWOSTotalSetIndex;
		public VariableProfile ForwardWOSRegPromoSetIndex;
		public VariableProfile ForwardWOSMarkdownSetIndex;
		public VariableProfile ForwardWOSTotalAllStoreIndex;
		public VariableProfile ForwardWOSRegPromoAllStoreIndex;
		public VariableProfile ForwardWOSMarkdownAllStoreIndex;
		public VariableProfile SellThruPctTotal;
		public VariableProfile SellThruPctRegPromo;
		public VariableProfile SellThruPctMarkdown;
		public VariableProfile SellThruPctTotalSetIndex;
		public VariableProfile SellThruPctRegPromoSetIndex;
		public VariableProfile SellThruPctMarkdownSetIndex;
		public VariableProfile SellThruPctTotalAllStoreIndex;
		public VariableProfile SellThruPctRegPromoAllStoreIndex;
		public VariableProfile SellThruPctMarkdownAllStoreIndex;
		public VariableProfile Intransit;
		public VariableProfile GradeTotal;
		public VariableProfile GradeRegPromo;
		public VariableProfile StoreStatus;
		//Begin TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
		public VariableProfile WOSTotal;
		public VariableProfile WOSRegPromo;
		//End TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
		// Begin TT#1224 - stodd - add committed
		public VariableProfile Committed;
		// End TT#1224 - stodd - add committed
        //Begin TT#739-MD -jsobek -Delete Stores
        public VariableProfile InStockSalesTotalUnits;
        public VariableProfile InStockSalesRegularUnits;
        public VariableProfile InStockSalesPromoUnits;
        public VariableProfile InStockSalesMarkdownUnits;
        
        public VariableProfile AccumSellThruSalesUnits;
        public VariableProfile AccumSellThruStockUnits;
        public VariableProfile DaysInStock;
        public VariableProfile ReceivedStockDuringWeek;
        //End TT#739-MD -jsobek -Delete Stores

		//=============
		// CONSTRUCTORS
		//=============
		
		/// <summary>
		/// Creates a new instance of Variables.
		/// </summary>

		public BasePlanVariables(bool aCustomVariablesDefined) 
		{
			try
			{
				_customVariablesDefined = aCustomVariablesDefined;

				_profileList = new ProfileList(eProfileType.Variable);
				_onHandList = new ProfileList(eProfileType.Variable);
				_totalWeekToDaySalesList = new ProfileList(eProfileType.Variable);
				_regularWeekToDaySalesList = new ProfileList(eProfileType.Variable);
				_promoWeekToDaySalesList = new ProfileList(eProfileType.Variable);
				_regPromoWeekToDaySalesList = new ProfileList(eProfileType.Variable);
				_markdownWeekToDaySalesList = new ProfileList(eProfileType.Variable);
				_nameHash = new Hashtable();
				_databaseColumnHash = new Hashtable();
				_seq = 1;
				_futureSeq = 10000;
				_databaseVariableList = new ProfileList(eProfileType.Variable);

				SalesTotalUnits = new VariableProfile(NextSequence, "Sales", eVariableCategory.Both, eVariableType.Sales, "SALES", eVariableDatabaseType.Integer, eVariableDatabaseType.Integer, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.PctContribution, eVariableWeekType.EOW, eEligibilityType.Sales, eSimilarStoreDateType.Sales, eVariableTimeTotalType.All, eVariableForecastType.Sales, eValueFormatType.GenericNumeric, 0, true, true, false, eLevelRollType.Sum, eDayToWeekRollType.Sum, eStoreToChainRollType.Sum, 0, 0, 0, 0, 0, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, "Sales");
				SalesRegularUnits = new VariableProfile(NextSequence, "Sales Reg", eVariableCategory.Both, eVariableType.Sales, "SALES_REG", eVariableDatabaseType.Integer, eVariableDatabaseType.Integer, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.PctContribution, eVariableWeekType.EOW, eEligibilityType.Sales, eSimilarStoreDateType.Sales, eVariableTimeTotalType.All, eVariableForecastType.Sales, eValueFormatType.GenericNumeric, 0, true, true, false, eLevelRollType.Sum, eDayToWeekRollType.Sum, eStoreToChainRollType.Sum, 0, 0, 0, 0, 0, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, "Sales");
				SalesPromoUnits = new VariableProfile(NextSequence, "Sales Promo", eVariableCategory.Both, eVariableType.Sales, "SALES_PROMO", eVariableDatabaseType.Integer, eVariableDatabaseType.Integer, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.PctContribution, eVariableWeekType.EOW, eEligibilityType.Sales, eSimilarStoreDateType.Sales, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, true, true, false, eLevelRollType.Sum, eDayToWeekRollType.Sum, eStoreToChainRollType.Sum, 0, 0, 0, 0, 0, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, "Sales");
                // Begin TT#1722 - RMatelic - Open Sales R/P to be editable
				//SalesRegPromoUnits = new VariableProfile(NextSequence, "Sales R/P", eVariableCategory.Both, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.Sales, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Sales");
				// BEGIN TT#3054 - stodd - forecasting issue with sales R/P variables and similar stores
                //SalesRegPromoUnits = new VariableProfile(NextSequence, "Sales R/P", eVariableCategory.Both, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.PctContribution, eVariableWeekType.EOW, eEligibilityType.Sales, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, false, false, eLevelRollType.Sum, eDayToWeekRollType.Sum, eStoreToChainRollType.Sum, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Sales");
				SalesRegPromoUnits = new VariableProfile(NextSequence, "Sales R/P", eVariableCategory.Both, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.PctContribution, eVariableWeekType.EOW, eEligibilityType.Sales, eSimilarStoreDateType.Sales, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, false, false, eLevelRollType.Sum, eDayToWeekRollType.Sum, eStoreToChainRollType.Sum, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Sales");
				// BEGIN TT#3054 - stodd - forecasting issue with sales R/P variables and similar stores
                // End TT#1722
				SalesMarkdownUnits = new VariableProfile(NextSequence, "Sales Mkdn", eVariableCategory.Both, eVariableType.Sales, "SALES_MKDN", eVariableDatabaseType.Integer, eVariableDatabaseType.Integer, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.PctContribution, eVariableWeekType.EOW, eEligibilityType.Sales, eSimilarStoreDateType.Sales, eVariableTimeTotalType.All, eVariableForecastType.Sales, eValueFormatType.GenericNumeric, 0, false, false, false, eLevelRollType.Sum, eDayToWeekRollType.Sum, eStoreToChainRollType.Sum, 0, 0, 0, 0, 0, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, "Sales");
				SalesTotalSetIndex = new VariableProfile(NextSequence, "Set Sales IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Dynamic, eVariableSpreadType.PctChange, eVariableWeekType.EOW, eEligibilityType.Sales, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Sales");
				SalesRegularSetIndex = new VariableProfile(NextSequence, "Set Sales Reg IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Dynamic, eVariableSpreadType.PctChange, eVariableWeekType.EOW, eEligibilityType.Sales, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Sales");
				SalesPromoSetIndex = new VariableProfile(NextSequence, "Set Sales Promo IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Dynamic, eVariableSpreadType.PctChange, eVariableWeekType.EOW, eEligibilityType.Sales, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Sales");
				SalesRegPromoSetIndex = new VariableProfile(NextSequence, "Set Sales R/P IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.Sales, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Sales");
				SalesMarkdownSetIndex = new VariableProfile(NextSequence, "Set Sales Mkdn IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.Sales, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Sales");
				SalesTotalAllStoreIndex = new VariableProfile(NextSequence, "All Sales IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Dynamic, eVariableSpreadType.PctChange, eVariableWeekType.EOW, eEligibilityType.Sales, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Sales");
				SalesRegularAllStoreIndex = new VariableProfile(NextSequence, "All Sales Reg IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Dynamic, eVariableSpreadType.PctChange, eVariableWeekType.EOW, eEligibilityType.Sales, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Sales");
				SalesPromoAllStoreIndex = new VariableProfile(NextSequence, "All Sales Promo IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Dynamic, eVariableSpreadType.PctChange, eVariableWeekType.EOW, eEligibilityType.Sales, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Sales");
				SalesRegPromoAllStoreIndex = new VariableProfile(NextSequence, "All Sales R/P IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.Sales, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Sales");
				SalesMarkdownAllStoreIndex = new VariableProfile(NextSequence, "All Sales Mkdn IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.Sales, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Sales");
				InventoryTotalUnits = new VariableProfile(NextSequence, "Stock", eVariableCategory.Both, eVariableType.BegStock, "STOCK", eVariableDatabaseType.Integer, eVariableDatabaseType.Integer, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.PctContribution, eVariableWeekType.BOW, eEligibilityType.Stock, eSimilarStoreDateType.Stock, eVariableTimeTotalType.First, eVariableForecastType.Stock, eValueFormatType.GenericNumeric, 0, true, true, false, eLevelRollType.Sum, eDayToWeekRollType.First, eStoreToChainRollType.Sum, 0, 0, 0, 0, 0, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, "Stock");
				InventoryRegularUnits = new VariableProfile(NextSequence, "Stock Reg", eVariableCategory.Both, eVariableType.BegStock, "STOCK_REG", eVariableDatabaseType.Integer, eVariableDatabaseType.Integer, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.PctContribution, eVariableWeekType.BOW, eEligibilityType.Stock, eSimilarStoreDateType.Stock, eVariableTimeTotalType.First, eVariableForecastType.Stock, eValueFormatType.GenericNumeric, 0, true, true, false, eLevelRollType.Sum, eDayToWeekRollType.First, eStoreToChainRollType.Sum, 0, 0, 0, 0, 0, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, "Stock");
				InventoryMarkdownUnits = new VariableProfile(NextSequence, "Stock Mkdn", eVariableCategory.Both, eVariableType.BegStock, "STOCK_MKDN", eVariableDatabaseType.Integer, eVariableDatabaseType.Integer, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.PctContribution, eVariableWeekType.BOW, eEligibilityType.Stock, eSimilarStoreDateType.Stock, eVariableTimeTotalType.First, eVariableForecastType.Stock, eValueFormatType.GenericNumeric, 0, false, true, false, eLevelRollType.Sum, eDayToWeekRollType.First, eStoreToChainRollType.Sum, 0, 0, 0, 0, 0, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, "Stock");
				InventoryTotalSetIndex = new VariableProfile(NextSequence, "Set Stock IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Dynamic, eVariableSpreadType.PctChange, eVariableWeekType.BOW, eEligibilityType.Stock, eSimilarStoreDateType.None, eVariableTimeTotalType.First, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Stock");
				InventoryRegularSetIndex = new VariableProfile(NextSequence, "Set Stock Reg IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Dynamic, eVariableSpreadType.PctChange, eVariableWeekType.BOW, eEligibilityType.Stock, eSimilarStoreDateType.None, eVariableTimeTotalType.First, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Stock");
				InventoryMarkdownSetIndex = new VariableProfile(NextSequence, "Set Stock Mkdn IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.BOW, eEligibilityType.Stock, eSimilarStoreDateType.None, eVariableTimeTotalType.First, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Stock");
				InventoryTotalAllStoreIndex = new VariableProfile(NextSequence, "All Stock IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Dynamic, eVariableSpreadType.PctChange, eVariableWeekType.BOW, eEligibilityType.Stock, eSimilarStoreDateType.None, eVariableTimeTotalType.First, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Stock");
				InventoryRegularAllStoreIndex = new VariableProfile(NextSequence, "All Stock Reg IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Dynamic, eVariableSpreadType.PctChange, eVariableWeekType.BOW, eEligibilityType.Stock, eSimilarStoreDateType.None, eVariableTimeTotalType.First, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Stock");
				InventoryMarkdownAllStoreIndex = new VariableProfile(NextSequence, "All Stock Mkdn IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.BOW, eEligibilityType.Stock, eSimilarStoreDateType.None, eVariableTimeTotalType.First, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Stock");
				ReceiptTotalUnits = new VariableProfile(NextSequence, "Receipts", eVariableCategory.Both, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				ReceiptRegularUnits = new VariableProfile(NextSequence, "Receipts Reg", eVariableCategory.Both, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				ReceiptMarkdownUnits = new VariableProfile(NextSequence, "Receipts Mkdn", eVariableCategory.Both, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				SalesStockRatioTotal = new VariableProfile(NextSequence, "S/S Ratio", eVariableCategory.Both, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				SalesStockRatioRegPromo = new VariableProfile(NextSequence, "S/S Ratio R/P", eVariableCategory.Both, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				SalesStockRatioMarkdown = new VariableProfile(NextSequence, "S/S Ratio Mkdn", eVariableCategory.Both, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				ForwardWOSTotal = new VariableProfile(NextSequence, "FWOS", eVariableCategory.Both, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.Stock, eSimilarStoreDateType.None, eVariableTimeTotalType.First, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				ForwardWOSRegPromo = new VariableProfile(NextSequence, "FWOS R/P", eVariableCategory.Both, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.Stock, eSimilarStoreDateType.None, eVariableTimeTotalType.First, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				ForwardWOSMarkdown = new VariableProfile(NextSequence, "FWOS Mkdn", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.Stock, eSimilarStoreDateType.None, eVariableTimeTotalType.First, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				ForwardWOSTotalSetIndex = new VariableProfile(NextSequence, "Set FWOS IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.Stock, eSimilarStoreDateType.None, eVariableTimeTotalType.First, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				ForwardWOSRegPromoSetIndex = new VariableProfile(NextSequence, "Set FWOS R/P IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.Stock, eSimilarStoreDateType.None, eVariableTimeTotalType.First, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				ForwardWOSMarkdownSetIndex = new VariableProfile(NextSequence, "Set FWOS Mkdn IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.Stock, eSimilarStoreDateType.None, eVariableTimeTotalType.First, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				ForwardWOSTotalAllStoreIndex = new VariableProfile(NextSequence, "All FWOS IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.Stock, eSimilarStoreDateType.None, eVariableTimeTotalType.First, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				ForwardWOSRegPromoAllStoreIndex = new VariableProfile(NextSequence, "All FWOS R/P IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.Stock, eSimilarStoreDateType.None, eVariableTimeTotalType.First, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				ForwardWOSMarkdownAllStoreIndex = new VariableProfile(NextSequence, "All FWOS Mkdn IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.Stock, eSimilarStoreDateType.None, eVariableTimeTotalType.First, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				SellThruPctTotal = new VariableProfile(NextSequence, "Sell Thru % ", eVariableCategory.Both, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				SellThruPctRegPromo = new VariableProfile(NextSequence, "Sell Thru % R/P", eVariableCategory.Both, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				SellThruPctMarkdown = new VariableProfile(NextSequence, "Sell Thru % Mkdn", eVariableCategory.Both, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				SellThruPctTotalSetIndex = new VariableProfile(NextSequence, "Set Sell Thru % IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				SellThruPctRegPromoSetIndex = new VariableProfile(NextSequence, "Set Sell Thru % R/P IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				SellThruPctMarkdownSetIndex = new VariableProfile(NextSequence, "Set Sell Thru % Mkdn IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				SellThruPctTotalAllStoreIndex = new VariableProfile(NextSequence, "All Sell Thru % IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				SellThruPctRegPromoAllStoreIndex = new VariableProfile(NextSequence, "All Sell Thru % R/P IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				SellThruPctMarkdownAllStoreIndex = new VariableProfile(NextSequence, "All Sell Thru % Mkdn IDX", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
                // Begin TT#28 - RMatelic - Filter Explorer - Receive unhandled exception when selected some of the filters, but not all of them >>> moved definition to end of list
                // Begin TT#1224 - stodd - add committed
                //Committed = new VariableProfile(NextSequence, "Committed", eVariableCategory.Store, eVariableType.Intransit, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				// End TT#1224 - stodd - add committed
				// End TT#28
                Intransit = new VariableProfile(NextSequence, "Intransit", eVariableCategory.Store, eVariableType.Intransit, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				GradeTotal = new VariableProfile(NextSequence, "Grade", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.None, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.Sales, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.StoreGrade, 0, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				GradeRegPromo = new VariableProfile(NextSequence, "Grade R/P", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.None, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.Sales, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.StoreGrade, 0, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				StoreStatus = new VariableProfile(NextSequence, "Status", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.None, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.StoreStatus, 0, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				//NOTE -- NOTE -- NOTE
				//NOTE -- NOTE -- NOTE
				//When adding any new base variables, DO NOT USE the NextSequence property for the sequence #.  Use NextFutureSequence.  This is to prevent the renumbering
				//of Client-specific variables that would invalidate Filters and Forcasting Models.
				//NOTE -- NOTE -- NOTE
				//NOTE -- NOTE -- NOTE
				//Begin TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
				WOSTotal = new VariableProfile(NextFutureSequence, "Fcst WOS", eVariableCategory.Chain, eVariableType.FWOS, "WOS", eVariableDatabaseType.Real, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.Plug, eVariableWeekType.EOW, eEligibilityType.Stock, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				WOSRegPromo = new VariableProfile(NextFutureSequence, "Fcst WOS R/P", eVariableCategory.Chain, eVariableType.FWOS, "WOS_REGPROMO", eVariableDatabaseType.Real, eVariableDatabaseType.None, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.Plug, eVariableWeekType.EOW, eEligibilityType.Stock, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 2, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
				//End TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
                // Begin TT#28 - RMatelic - Filter Explorer - Receive unhandled exception when selected some of the filters, but not all of them >>> moved from above 
                Committed = new VariableProfile(NextFutureSequence, "Committed", eVariableCategory.Store, eVariableType.Intransit, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other");
                // End TT#28

                //Begin TT#739-MD -jsobek -Delete Stores
                InStockSalesTotalUnits = new VariableProfile(NextFutureSequence, "In Stock Sales", eVariableCategory.Store, eVariableType.Other, "IN_STOCK_SALES", eVariableDatabaseType.Integer, eVariableDatabaseType.Integer, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.None, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.None, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, true, false, eLevelRollType.Sum, eDayToWeekRollType.Sum, eStoreToChainRollType.Sum, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, "Other");
                InStockSalesRegularUnits = new VariableProfile(NextFutureSequence, "In Stock Sales Reg", eVariableCategory.Store, eVariableType.Other, "IN_STOCK_SALES_REG", eVariableDatabaseType.Integer, eVariableDatabaseType.Integer, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.None, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.None, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, true, false, eLevelRollType.Sum, eDayToWeekRollType.Sum, eStoreToChainRollType.Sum, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, "Other");
                InStockSalesPromoUnits = new VariableProfile(NextFutureSequence, "In Stock Sales Promo", eVariableCategory.Store, eVariableType.Other, "IN_STOCK_SALES_PROMO", eVariableDatabaseType.Integer, eVariableDatabaseType.Integer, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.None, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.None, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, true, false, eLevelRollType.Sum, eDayToWeekRollType.Sum, eStoreToChainRollType.Sum, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, "Other");
                InStockSalesMarkdownUnits = new VariableProfile(NextFutureSequence, "In Stock Sales Mkdn", eVariableCategory.Store, eVariableType.Other, "IN_STOCK_SALES_MKDN", eVariableDatabaseType.Integer, eVariableDatabaseType.Integer, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.None, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.None, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, true, false, eLevelRollType.Sum, eDayToWeekRollType.Sum, eStoreToChainRollType.Sum, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, "Other");

                AccumSellThruSalesUnits = new VariableProfile(NextFutureSequence, "Accum Sell Thru Sales", eVariableCategory.Store, eVariableType.Other, "ACCUM_SELL_THRU_SALES", eVariableDatabaseType.Integer, eVariableDatabaseType.Integer, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.None, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.None, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, true, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, "Other");
                AccumSellThruStockUnits = new VariableProfile(NextFutureSequence, "Accum Sell Thru Stock", eVariableCategory.Store, eVariableType.Other, "ACCUM_SELL_THRU_STOCK", eVariableDatabaseType.Integer, eVariableDatabaseType.Integer, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.None, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.None, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, true, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, "Other");
                DaysInStock = new VariableProfile(NextFutureSequence, "Days In Stock", eVariableCategory.Store, eVariableType.Other, "DAYS_IN_STOCK", eVariableDatabaseType.Integer, eVariableDatabaseType.Integer, eVariableStyle.None, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.None, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.None, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, true, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, "Other");
                ReceivedStockDuringWeek = new VariableProfile(NextFutureSequence, "Received Stock", eVariableCategory.Store, eVariableType.Other, "RECEIVED_STOCK", eVariableDatabaseType.Integer, eVariableDatabaseType.Integer, eVariableStyle.None, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.None, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.None, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, true, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, "Other");
                //End TT#739-MD -jsobek -Delete Stores
			
            }
			catch (Exception exc)
			{
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the next available variable sequence number.
		/// </summary>

		protected int NextSequence
		{
			get
			{
				return _seq++;
			}
		}

		/// <summary>
		/// Gets the next available variable sequence number for future variables.
		/// </summary>

		protected int NextFutureSequence
		{
			get
			{
				return _futureSeq++;
			}
		}

		/// <summary>
		/// Gets the SalesTotalUnits variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile SalesTotalUnitsVariable
		{
			get
			{
				return SalesTotalUnits;
			}
		}

		/// <summary>
		/// Gets the SalesRegularUnits variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile SalesRegularUnitsVariable
		{
			get
			{
				return SalesRegularUnits;
			}
		}

		/// <summary>
		/// Gets the SalesPromoUnitsVariable variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile SalesPromoUnitsVariable
		{
			get
			{
				return SalesPromoUnits;
			}
		}

		/// <summary>
		/// Gets the SalesMarkdownUnitsVariable variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile SalesMarkdownUnitsVariable
		{
			get
			{
				return SalesMarkdownUnits;
			}
		}

		/// <summary>
		/// Gets the SalesRegPromoUnits variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile SalesRegPromoUnitsVariable
		{
			get
			{
				return SalesRegPromoUnits;
			}
		}

		/// <summary>
		/// Gets the SalesTotalSetIndex variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile SalesTotalSetIndexVariable
		{
			get
			{
				return SalesTotalSetIndex;
			}
		}

		/// <summary>
		/// Gets the SalesTotalAllStoreIndex variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile SalesTotalAllStoreIndexVariable
		{
			get
			{
				return SalesTotalAllStoreIndex;
			}
		}

		/// <summary>
		/// Gets the SalesRegPromoSetIndex variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile SalesRegPromoSetIndexVariable
		{
			get
			{
				return SalesRegPromoSetIndex;
			}
		}

		/// <summary>
		/// Gets the SalesRegPromoAllStoreIndex variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile SalesRegPromoAllStoreIndexVariable
		{
			get
			{
				return SalesRegPromoAllStoreIndex;
			}
		}
        //Begin TT#855-MD -jsobek -Velocity Enhancements
        public VariableProfile InventoryRegularAllStoreIndexVariable
        {
            get
            {
                return InventoryRegularAllStoreIndex;
            }
        }
        public VariableProfile InventoryTotalAllStoreIndexVariable
        {
            get
            {
                return InventoryTotalAllStoreIndex;
            }
        }
        public VariableProfile InventoryRegularSetIndexVariable
        {
            get
            {
                return InventoryRegularSetIndex;
            }
        }
        public VariableProfile InventoryTotalSetIndexVariable
        {
            get
            {
                return InventoryTotalSetIndex;
            }
        }
        //End TT#855-MD -jsobek -Velocity Enhancements

		/// <summary>
		/// Gets the InventoryTotalUnits variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile InventoryTotalUnitsVariable
		{
			get
			{
				return InventoryTotalUnits;
			}
		}

		/// <summary>
		/// Gets the InventoryRegularUnits variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile InventoryRegularUnitsVariable
		{
			get
			{
				return InventoryRegularUnits;
			}
		}

		/// <summary>
		/// Gets the SalesStockRatioTotal variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		
		public VariableProfile SalesStockRatioTotalVariable
		{
			get
			{
				return SalesStockRatioTotal;
			}
		}

		/// <summary>
		/// Gets the SalesStockRatioRegPromo variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		
		public VariableProfile SalesStockRatioRegPromoVariable
		{
			get
			{
				return SalesStockRatioRegPromo;
			}
		}

		/// <summary>
		/// Gets the SalesStockRatioMarkdown variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		
		public VariableProfile SalesStockRatioMarkdownVariable
		{
			get
			{
				return SalesStockRatioMarkdown;
			}
		}

		/// <summary>
		/// Gets the SellThruPctTotal variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile SellThruPctTotalVariable
		{
			get
			{
				return SellThruPctTotal;
			}
		}

		/// <summary>
		/// Gets the SellThruPctTotalSetIndex variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile SellThruPctTotalSetIndexVariable
		{
			get
			{
				return SellThruPctTotalSetIndex;
			}
		}

		/// <summary>
		/// Gets the SellThruPctTotalAllStoreIndex variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile SellThruPctTotalAllStoreIndexVariable
		{
			get
			{
				return SellThruPctTotalAllStoreIndex;
			}
		}

		/// <summary>
		/// Gets the SellThruPctRegPromo variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile SellThruPctRegPromoVariable
		{
			get
			{
				return SellThruPctRegPromo;
			}
		}

		/// <summary>
		/// Gets the SellThruPctRegPromoSetIndex variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile SellThruPctRegPromoSetIndexVariable
		{
			get
			{
				return SellThruPctRegPromoSetIndex;
			}
		}

		/// <summary>
		/// Gets the SellThruPctRegPromoAllStoreIndex variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile SellThruPctRegPromoAllStoreIndexVariable
		{
			get
			{
				return SellThruPctRegPromoAllStoreIndex;
			}
		}

		// Begin TT#1224 - Stodd - add comitted variable
		/// <summary>
		/// Gets the Committed variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile CommittedVariable
		{
			get
			{
				return Committed;
			}
		}
		// End TT#1224 - Stodd - add comitted variable

		/// <summary>
		/// Gets the Intransit variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile IntransitVariable
		{
			get
			{
				return Intransit;
			}
		}

		/// <summary>
		/// Gets the ReceiptTotalUnits variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile ReceiptTotalUnitsVariable
		{
			get
			{
				return ReceiptTotalUnits;
			}
		}
		/// <summary>
		/// Gets the ReceiptRegularUnits variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile ReceiptRegularUnitsVariable
		{
			get
			{
				return ReceiptRegularUnits;
			}
		}

		// BEGIN TT#619 - STodd - OTS Forecast - Chain Plan not required (#46)
		/// <summary>
		/// Gets the WOSTotal variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile WOSTotalVariable
		{
			get
			{
				return WOSTotal;
			}
		}

		/// <summary>
		/// Gets the WOSRegPromo variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public VariableProfile WOSRegPromoVariable
		{
			get
			{
				return WOSRegPromo;
			}
		}
		// END TT#619 - STodd - OTS Forecast - Chain Plan not required (#46)

        // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
        /// <summary>
        /// Gets the GradeTotal variable.  
        /// </summary>
        public VariableProfile GradeTotalVariable
        {
            get
            {
                return GradeTotal;
            }
        }
        /// <summary>
        /// Gets the GradeTotal variable.  
        /// </summary>
        public VariableProfile GradeRegPromoVariable
        {
            get
            {
                return GradeRegPromo;
            }
        }
        // End TT#638 

		/// <summary>
		/// Gets a boolean indicating if custom variables are defined.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public bool CustomVariablesDefined
		{
			get
			{
				return _customVariablesDefined;
			}
		}

		/// <summary>
		/// Gets the number of Variables.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public int NumVariables
		{
			get
			{
				try
				{
					return _profileList.Count;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the ProfileList of Variables.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public ProfileList VariableProfileList
		{
			get
			{
				if (_orderedVariableList == null)
				{
					return _profileList;
				}
				else
				{
					return _orderedVariableList;
				}
			}
		}

		/// <summary>
		/// Gets the maximum time total variables.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public int MaxTimeTotalVariables
		{
			get
			{
				return _maxTimeTotalVariables;
			}
		}

		/// <summary>
		/// Gets the maximum Store time total variables.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public int MaxStoreTimeTotalVariables
		{
			get
			{
				return _maxStoreTimeTotalVariables;
			}
		}

		/// <summary>
		/// Gets the maximum Chain time total variables.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public int MaxChainTimeTotalVariables
		{
			get
			{
				return _maxChainTimeTotalVariables;
			}
		}

		/// <summary>
		/// Returns the ProfileXRef of the Variable to TimeTotalVariable profile relationship.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public ProfileXRef TimeTotalXRef
		{
			get
			{
				return _timeTotalXRef;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Initializes this instance of Variables.
		/// </summary>
		/// <param name="aBasePlanTimeTotalVariables">
		/// The TimeTotalVariables object to initialize with.
		/// </param>
		/// <param name="aInitDBColPos">
		/// Identifies if the position of the variable on the database table is to be set.  This requires a 
		/// database connection.
		/// </param>

		public void Initialize(BasePlanTimeTotalVariables aBasePlanTimeTotalVariables)
		{
			SortedList orderedVariableList;

			try
			{
				orderedVariableList = new SortedList();

				InitializeVariables(aBasePlanTimeTotalVariables);

				_maxTimeTotalVariables = 0;
				_maxStoreTimeTotalVariables = 0;
				_maxChainTimeTotalVariables = 0;
				_maxDatabaseVariables = 0;
				_timeTotalXRef = new ProfileXRef(eProfileType.TimeTotalVariable, eProfileType.Variable);

				foreach (VariableProfile varProf in _profileList)
				{
					_maxTimeTotalVariables = System.Math.Max(_maxTimeTotalVariables, varProf.TimeTotalVariables.Count);
					_maxStoreTimeTotalVariables = System.Math.Max(_maxStoreTimeTotalVariables, varProf.TimeTotalStoreVariables.Count);
					_maxChainTimeTotalVariables = System.Math.Max(_maxChainTimeTotalVariables, varProf.TimeTotalChainVariables.Count);

					if (varProf.DatabaseColumnName != null)
					{
						varProf.DatabaseColumnPosition = _maxDatabaseVariables;
						_maxDatabaseVariables++;
						_databaseVariableList.Add(varProf);
					}

					foreach (TimeTotalVariableProfile timeTotVarProf in varProf.TimeTotalVariables)
					{
						_timeTotalXRef.AddXRefIdEntry(timeTotVarProf.Key, varProf.Key);
					}

					_nameHash.Add(varProf.VariableName, varProf);

					if (varProf.DatabaseColumnName != null)
					{
						_databaseColumnHash.Add(varProf.DatabaseColumnName, varProf);
					}

					orderedVariableList.Add(varProf.DisplaySequence > 0 ? varProf.DisplaySequence : varProf.Key * 10000, varProf);
				}

				_orderedVariableList = new ProfileList(eProfileType.Variable);

				foreach (VariableProfile varProf in orderedVariableList.Values)
				{
					_orderedVariableList.Add(varProf);
				}

                /// <example>
                /// Add variable grouping hierarchy for chooser.  Levels are separated by "|".
                /// _variableGroupings.Add("Sales|Units");
                /// _variableGroupings.Add("Sales|Dollars");
                /// _variableGroupings.Add("Stock|Units");
                /// _variableGroupings.Add("Stock|Dollars");
                /// _variableGroupings.Add("Other");
                /// </example>

                if (_variableGroupings == null)  // not added by custom definitions
                {
                    _variableGroupings = new ArrayList();
                    _variableGroupings.Add("Sales");
                    _variableGroupings.Add("Stock");
                    _variableGroupings.Add("Other");
                }
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Initialized the variables for this instance.
		/// </summary>
		/// <param name="aBasePlanTimeTotalVariables">
		/// The TimeTotalVariables object to initialize with.
		/// </param>

		virtual protected void InitializeVariables(BasePlanTimeTotalVariables aBasePlanTimeTotalVariables)
		{
			try
			{
				// Add variables to profile list
				
				_profileList.Add(SalesTotalUnits);
				_profileList.Add(SalesRegularUnits);
				_profileList.Add(SalesPromoUnits);
				_profileList.Add(SalesRegPromoUnits);
				_profileList.Add(SalesMarkdownUnits);
				_profileList.Add(SalesTotalSetIndex);
				_profileList.Add(SalesRegularSetIndex);
				_profileList.Add(SalesPromoSetIndex);
				_profileList.Add(SalesRegPromoSetIndex);
				_profileList.Add(SalesMarkdownSetIndex);
				_profileList.Add(SalesTotalAllStoreIndex);
				_profileList.Add(SalesRegularAllStoreIndex);
				_profileList.Add(SalesPromoAllStoreIndex);
				_profileList.Add(SalesRegPromoAllStoreIndex);
				_profileList.Add(SalesMarkdownAllStoreIndex);
				_profileList.Add(InventoryTotalUnits);
				_profileList.Add(InventoryRegularUnits);
				_profileList.Add(InventoryMarkdownUnits);
				_profileList.Add(InventoryTotalSetIndex);
				_profileList.Add(InventoryRegularSetIndex);
				_profileList.Add(InventoryMarkdownSetIndex);
				_profileList.Add(InventoryTotalAllStoreIndex);
				_profileList.Add(InventoryRegularAllStoreIndex);
				_profileList.Add(InventoryMarkdownAllStoreIndex);
				_profileList.Add(ReceiptTotalUnits);
				_profileList.Add(ReceiptRegularUnits);
				_profileList.Add(ReceiptMarkdownUnits);
				_profileList.Add(SalesStockRatioTotal);
				_profileList.Add(SalesStockRatioRegPromo);
				_profileList.Add(SalesStockRatioMarkdown);
				_profileList.Add(ForwardWOSTotal);
				_profileList.Add(ForwardWOSRegPromo);
				_profileList.Add(ForwardWOSMarkdown);
				_profileList.Add(ForwardWOSTotalSetIndex);
				_profileList.Add(ForwardWOSRegPromoSetIndex);
				_profileList.Add(ForwardWOSMarkdownSetIndex);
				_profileList.Add(ForwardWOSTotalAllStoreIndex);
				_profileList.Add(ForwardWOSRegPromoAllStoreIndex);
				_profileList.Add(ForwardWOSMarkdownAllStoreIndex);
				_profileList.Add(SellThruPctTotal);
				_profileList.Add(SellThruPctRegPromo);
				_profileList.Add(SellThruPctMarkdown);
				_profileList.Add(SellThruPctTotalSetIndex);
				_profileList.Add(SellThruPctRegPromoSetIndex);
				_profileList.Add(SellThruPctMarkdownSetIndex);
				_profileList.Add(SellThruPctTotalAllStoreIndex);
				_profileList.Add(SellThruPctRegPromoAllStoreIndex);
				_profileList.Add(SellThruPctMarkdownAllStoreIndex);
				_profileList.Add(Intransit);
				_profileList.Add(GradeTotal);
				_profileList.Add(GradeRegPromo);
				_profileList.Add(StoreStatus);
				//Begin TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
				_profileList.Add(WOSTotal);
				_profileList.Add(WOSRegPromo);
				//End TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
				// Begin TT#1224 - stodd - add Committed
				_profileList.Add(Committed);
				// End TT#1224 - stodd - add Committed
                //Begin TT#739-MD -jsobek -Delete Stores
                _profileList.Add(InStockSalesTotalUnits);
                _profileList.Add(InStockSalesRegularUnits);
                _profileList.Add(InStockSalesPromoUnits);
                _profileList.Add(InStockSalesMarkdownUnits);
        
                _profileList.Add(AccumSellThruSalesUnits);
                _profileList.Add(AccumSellThruStockUnits);
                _profileList.Add(DaysInStock);
                _profileList.Add(ReceivedStockDuringWeek);
                //End TT#739-MD -jsobek -Delete Stores



				// Create OnHand list

				_onHandList.Add(InventoryRegularUnits);
				_onHandList.Add(InventoryMarkdownUnits);

				// Create Total WeekToDay Sales list

				_totalWeekToDaySalesList.Add(SalesRegularUnits);
				_totalWeekToDaySalesList.Add(SalesPromoUnits);
				_totalWeekToDaySalesList.Add(SalesMarkdownUnits);

				SalesTotalUnits.ActualWTDSalesVariableList = _totalWeekToDaySalesList;

				// Create Regular WeekToDay Sales list

				_regularWeekToDaySalesList.Add(SalesRegularUnits);

				SalesRegularUnits.ActualWTDSalesVariableList = _regularWeekToDaySalesList;

				// Create Promo WeekToDay Sales list

				_promoWeekToDaySalesList.Add(SalesPromoUnits);

				SalesPromoUnits.ActualWTDSalesVariableList = _promoWeekToDaySalesList;

				// Create RegPromo WeekToDay Sales list

				_regPromoWeekToDaySalesList.Add(SalesRegularUnits);
				_regPromoWeekToDaySalesList.Add(SalesPromoUnits);

				SalesRegPromoUnits.ActualWTDSalesVariableList = _regPromoWeekToDaySalesList;

				// Create Markdown WeekToDay Sales list

				_markdownWeekToDaySalesList.Add(SalesMarkdownUnits);

				SalesMarkdownUnits.ActualWTDSalesVariableList = _markdownWeekToDaySalesList;

				// Add TimeTotalVariables to Variables

				SalesTotalUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesTotalUnitsT1);
				SalesTotalUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesTotalUnitsT2);
				SalesTotalUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesTotalUnitsT3);
				SalesRegularUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesRegularUnitsT1);
				SalesRegularUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesRegularUnitsT2);
				SalesPromoUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesPromoUnitsT1);
				SalesPromoUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesPromoUnitsT2);
				SalesRegPromoUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesRegPromoUnitsT1);
				SalesRegPromoUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesRegPromoUnitsT2);
				SalesRegPromoUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesRegPromoUnitsT3);
				SalesMarkdownUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesMarkdownUnitsT1);
				SalesMarkdownUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesMarkdownUnitsT2);
				SalesMarkdownUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesMarkdownUnitsT3);
				SalesTotalSetIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesTotalSetIndexT1);
				SalesRegularSetIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesRegularSetIndexT1);
				SalesPromoSetIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesPromoSetIndexT1);
				SalesRegPromoSetIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesRegPromoSetIndexT1);
				SalesMarkdownSetIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesMarkdownSetIndexT1);
				SalesTotalAllStoreIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesTotalAllStoreIndexT1);
				SalesRegularAllStoreIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesRegularAllStoreIndexT1);
				SalesPromoAllStoreIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesPromoAllStoreIndexT1);
				SalesRegPromoAllStoreIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesRegPromoAllStoreIndexT1);
				SalesMarkdownAllStoreIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesMarkdownAllStoreIndexT1);
				InventoryTotalUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryTotalUnitsT1);
				InventoryTotalUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryTotalUnitsT2);
				InventoryTotalUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryTotalUnitsT3);
				InventoryTotalUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryTotalUnitsT4);
				InventoryTotalUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryTotalUnitsT5);
				InventoryTotalUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryTotalUnitsT6);
				InventoryTotalUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryTotalUnitsT7);
                InventoryTotalUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryTotalUnitsT8);        // TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
				InventoryRegularUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryRegularUnitsT1);
				InventoryRegularUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryRegularUnitsT2);
				InventoryRegularUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryRegularUnitsT3);
				InventoryRegularUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryRegularUnitsT4);
				InventoryRegularUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryRegularUnitsT5);
				InventoryRegularUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryRegularUnitsT6);
				InventoryRegularUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryRegularUnitsT7);
                InventoryRegularUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryRegularUnitsT8);    // TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
				InventoryMarkdownUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryMarkdownUnitsT1);
				InventoryMarkdownUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryMarkdownUnitsT2);
				InventoryMarkdownUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryMarkdownUnitsT3);
				InventoryTotalSetIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryTotalSetIndexT1);
				InventoryTotalSetIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryTotalSetIndexT2);
				InventoryTotalSetIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryTotalSetIndexT3);
				InventoryRegularSetIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryRegularSetIndexT1);
				InventoryRegularSetIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryRegularSetIndexT2);
				InventoryRegularSetIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryRegularSetIndexT3);
				InventoryMarkdownSetIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryMarkdownSetIndexT1);
				InventoryMarkdownSetIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryMarkdownSetIndexT2);
				InventoryMarkdownSetIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryMarkdownSetIndexT3);
				InventoryTotalAllStoreIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryTotalAllStoreIndexT1);
				InventoryTotalAllStoreIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryTotalAllStoreIndexT2);
				InventoryTotalAllStoreIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryTotalAllStoreIndexT3);
				InventoryRegularAllStoreIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryRegularAllStoreIndexT1);
				InventoryRegularAllStoreIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryRegularAllStoreIndexT2);
				InventoryRegularAllStoreIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryRegularAllStoreIndexT3);
				InventoryMarkdownAllStoreIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryMarkdownAllStoreIndexT1);
				InventoryMarkdownAllStoreIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryMarkdownAllStoreIndexT2);
				InventoryMarkdownAllStoreIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.InventoryMarkdownAllStoreIndexT3);
				ReceiptTotalUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.ReceiptTotalUnitsT1);
				ReceiptRegularUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.ReceiptRegularUnitsT1);
				ReceiptMarkdownUnits.AddTimeTotalVariable(aBasePlanTimeTotalVariables.ReceiptMarkdownUnitsT1);
				SalesStockRatioTotal.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesStockRatioTotalT1);
				SalesStockRatioRegPromo.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesStockRatioRegPromoT1);
				SalesStockRatioMarkdown.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SalesStockRatioMarkdownT1);
				ForwardWOSTotal.AddTimeTotalVariable(aBasePlanTimeTotalVariables.ForwardWOSTotalT1);
				ForwardWOSRegPromo.AddTimeTotalVariable(aBasePlanTimeTotalVariables.ForwardWOSRegPromoT1);
				ForwardWOSMarkdown.AddTimeTotalVariable(aBasePlanTimeTotalVariables.ForwardWOSMarkdownT1);
				ForwardWOSTotalSetIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.ForwardWOSTotalSetIndexT1);
				ForwardWOSRegPromoSetIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.ForwardWOSRegPromoSetIndexT1);
				ForwardWOSMarkdownSetIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.ForwardWOSMarkdownSetIndexT1);
				ForwardWOSTotalAllStoreIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.ForwardWOSTotalAllStoreIndexT1);
				ForwardWOSRegPromoAllStoreIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.ForwardWOSRegPromoAllStoreIndexT1);
				ForwardWOSMarkdownAllStoreIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.ForwardWOSMarkdownAllStoreIndexT1);
				SellThruPctTotal.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SellThruPctTotalT1);
				SellThruPctRegPromo.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SellThruPctRegPromoT1);
				SellThruPctMarkdown.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SellThruPctMarkdownT1);
				SellThruPctTotalSetIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SellThruPctTotalSetIndexT1);
				SellThruPctRegPromoSetIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SellThruPctRegPromoSetIndexT1);
				SellThruPctMarkdownSetIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SellThruPctMarkdownSetIndexT1);
				SellThruPctTotalAllStoreIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SellThruPctTotalAllStoreIndexT1);
				SellThruPctRegPromoAllStoreIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SellThruPctRegPromoAllStoreIndexT1);
				SellThruPctMarkdownAllStoreIndex.AddTimeTotalVariable(aBasePlanTimeTotalVariables.SellThruPctMarkdownAllStoreIndexT1);
				Intransit.AddTimeTotalVariable(aBasePlanTimeTotalVariables.IntransitT1);
				GradeTotal.AddTimeTotalVariable(aBasePlanTimeTotalVariables.GradeTotalT1);
				GradeRegPromo.AddTimeTotalVariable(aBasePlanTimeTotalVariables.GradeRegPromoT1);
				//Begin TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
				WOSTotal.AddTimeTotalVariable(aBasePlanTimeTotalVariables.WOSTotalT1);
				WOSRegPromo.AddTimeTotalVariable(aBasePlanTimeTotalVariables.WOSRegPromoT1);
				//End TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
				// Begin TT#1224 - stodd - add Committed
				Committed.AddTimeTotalVariable(aBasePlanTimeTotalVariables.CommittedT1);
				// End TT#1224 - stodd - add Committed

				// Assign total and average TimeTotalVariable properties

				SalesTotalUnits.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SalesTotalUnitsT1;
				SalesTotalUnits.AverageTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SalesTotalUnitsT2;
				SalesRegularUnits.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SalesRegularUnitsT1;
				SalesPromoUnits.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SalesPromoUnitsT1;
				SalesRegPromoUnits.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SalesRegPromoUnitsT1;
				SalesRegPromoUnits.AverageTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SalesRegPromoUnitsT2;
				SalesMarkdownUnits.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SalesMarkdownUnitsT1;
				SalesMarkdownUnits.AverageTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SalesMarkdownUnitsT2;
				SalesTotalSetIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SalesTotalSetIndexT1;
				SalesRegularSetIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SalesRegularSetIndexT1;
				SalesPromoSetIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SalesPromoSetIndexT1;
				SalesRegPromoSetIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SalesRegPromoSetIndexT1;
				SalesMarkdownSetIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SalesMarkdownSetIndexT1;
				SalesTotalAllStoreIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SalesTotalAllStoreIndexT1;
				SalesRegularAllStoreIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SalesRegularAllStoreIndexT1;
				SalesPromoAllStoreIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SalesPromoAllStoreIndexT1;
				SalesRegPromoAllStoreIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SalesRegPromoAllStoreIndexT1;
				SalesMarkdownAllStoreIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SalesMarkdownAllStoreIndexT1;
				InventoryTotalUnits.AverageTimeTotalVariableProfile = aBasePlanTimeTotalVariables.InventoryTotalUnitsT2;
				InventoryRegularUnits.AverageTimeTotalVariableProfile = aBasePlanTimeTotalVariables.InventoryRegularUnitsT2;
				InventoryMarkdownUnits.AverageTimeTotalVariableProfile = aBasePlanTimeTotalVariables.InventoryMarkdownUnitsT2;
				InventoryTotalSetIndex.AverageTimeTotalVariableProfile = aBasePlanTimeTotalVariables.InventoryTotalSetIndexT2;
				InventoryRegularSetIndex.AverageTimeTotalVariableProfile = aBasePlanTimeTotalVariables.InventoryRegularSetIndexT2;
				InventoryMarkdownSetIndex.AverageTimeTotalVariableProfile = aBasePlanTimeTotalVariables.InventoryMarkdownSetIndexT2;
				InventoryTotalAllStoreIndex.AverageTimeTotalVariableProfile = aBasePlanTimeTotalVariables.InventoryTotalAllStoreIndexT2;
				InventoryRegularAllStoreIndex.AverageTimeTotalVariableProfile = aBasePlanTimeTotalVariables.InventoryRegularAllStoreIndexT2;
				InventoryMarkdownAllStoreIndex.AverageTimeTotalVariableProfile = aBasePlanTimeTotalVariables.InventoryMarkdownAllStoreIndexT2;
				ReceiptTotalUnits.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.ReceiptTotalUnitsT1;
				ReceiptRegularUnits.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.ReceiptRegularUnitsT1;
				ReceiptMarkdownUnits.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.ReceiptMarkdownUnitsT1;
				SalesStockRatioTotal.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SalesStockRatioTotalT1;
				SalesStockRatioRegPromo.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SalesStockRatioRegPromoT1;
				SalesStockRatioMarkdown.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SalesStockRatioMarkdownT1;
				ForwardWOSTotal.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.ForwardWOSTotalT1;
				ForwardWOSRegPromo.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.ForwardWOSRegPromoT1;
				ForwardWOSMarkdown.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.ForwardWOSMarkdownT1;
				ForwardWOSTotalSetIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.ForwardWOSTotalSetIndexT1;
				ForwardWOSRegPromoSetIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.ForwardWOSRegPromoSetIndexT1;
				ForwardWOSMarkdownSetIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.ForwardWOSMarkdownSetIndexT1;
				ForwardWOSTotalAllStoreIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.ForwardWOSTotalAllStoreIndexT1;
				ForwardWOSRegPromoAllStoreIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.ForwardWOSRegPromoAllStoreIndexT1;
				ForwardWOSMarkdownAllStoreIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.ForwardWOSMarkdownAllStoreIndexT1;
				SellThruPctTotal.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SellThruPctTotalT1;
				SellThruPctRegPromo.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SellThruPctRegPromoT1;
				SellThruPctMarkdown.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SellThruPctMarkdownT1;
				SellThruPctTotalSetIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SellThruPctTotalSetIndexT1;
				SellThruPctRegPromoSetIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SellThruPctRegPromoSetIndexT1;
				SellThruPctMarkdownSetIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SellThruPctMarkdownSetIndexT1;
				SellThruPctTotalAllStoreIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SellThruPctTotalAllStoreIndexT1;
				SellThruPctRegPromoAllStoreIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SellThruPctRegPromoAllStoreIndexT1;
				SellThruPctMarkdownAllStoreIndex.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.SellThruPctMarkdownAllStoreIndexT1;
				Intransit.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.IntransitT1;
				GradeTotal.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.GradeTotalT1;
				GradeRegPromo.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.GradeRegPromoT1;
				//Begin TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
				WOSTotal.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.WOSTotalT1;
				WOSRegPromo.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.WOSRegPromoT1;
				//End TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
				// Begin TT#1224 - stodd - add committed
				Committed.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.CommittedT1;
				// End TT#1224 - stodd - add committed
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// This method returns ArrayList VariableProfiles for a given variable type.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		/// <param name="aVariableType">
		/// The type of the variable to lookup.
		/// </param>
		/// <returns>
		/// An ArrayList of Variable profiles with the given type.
		/// </returns>

		public ProfileList GetVariableProfilesByType(eVariableType aVariableType)
		{
			try
			{
				ProfileList variableProfiles = new ProfileList(eProfileType.Variable);

				foreach (VariableProfile vp in _profileList)
				{
					if (vp.VariableType == aVariableType)
					{
						variableProfiles.Add(vp);
					}
				}

				return variableProfiles;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// This method returns ArrayList VariableProfiles for a given variable type.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		/// <param name="aVariableType">
		/// The type of the variable to lookup.
		/// </param>
		/// <returns>
		/// An ArrayList of Variable profiles with the given type.
		/// </returns>

		public ProfileList GetVariableProfilesByCustomType(eClientCustomVariableType aCustomVariableType)
		{
			try
			{
				ProfileList variableProfiles = new ProfileList(eProfileType.Variable);
				foreach (VariableProfile vp in _profileList)
				{
					if (vp.CustomVariableType == aCustomVariableType)
					{
						variableProfiles.Add(vp);
					}
				}
				return variableProfiles;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// This method returns a ProfileList of the variables that are used in the Total OnHand calculation.
		/// </summary>
		/// <returns>
		/// A ProfileList of VariableProfiles
		/// </returns>

		public ProfileList GetTotalOnHandVariableList()
		{
			try
			{
				return _onHandList;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// This method returns a ProfileList of the variables that are used in the Total WeekToDay Sales calculation.
		/// </summary>
		/// <returns>
		/// A ProfileList of VariableProfiles
		/// </returns>

		public ProfileList GetTotalWeekToDaySalesVariableList()
		{
			try
			{
				return _totalWeekToDaySalesList;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		/// <summary>
		/// This method returns ArrayList VariableProfiles for Assortment Planning.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		/// <returns>
		/// An ArrayList of Variable profiles for Assortment Planning.
		/// </returns>

		public ProfileList GetAssortmentPlanningVariableList()
		{
			VariableProfile newVarProf;
			try
			{
				if (_assortmentList == null)
				{
					_assortmentList = new ProfileList(eProfileType.Variable);
					foreach (VariableProfile vp in _profileList)
					{
						if (vp.AllowAssortmentPlanning)
						{
							newVarProf = (VariableProfile)vp.Clone();
							newVarProf.Key = newVarProf.Key + Include.AssortmentPlanVariableKeyOffset;
							_assortmentList.Add(newVarProf);
						}
					}
				}
				return _assortmentList;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		//End TT#2 - JScott - Assortment Planning - Phase 2
		/// <summary>
		/// This method returns VariableProfile for a given variable name.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		/// <param name="aName">
		/// The name of the variable to lookup.
		/// </param>
		/// <returns>
		/// The Variable profile with the given name.
		/// </returns>

		public VariableProfile GetVariableProfileByName(string aName)
		{
			try
			{
				return (VariableProfile)_nameHash[aName];
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// This method returns the VariableProfile for a given database column.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		/// <param name="aDatabaseColumn">
		/// The database column of the variable to lookup.
		/// </param>
		/// <returns>
		/// The Variable profile with the given database column.
		/// </returns>

		public VariableProfile GetDatabaseColumnProfile(string aDatabaseColumn)
		{
			try
			{
				if (_databaseColumnHash.Contains(aDatabaseColumn))
				{
					return (VariableProfile)_databaseColumnHash[aDatabaseColumn];
				}
			
				return null;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// This method returns the maximum database column position in the variable tables.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		/// <returns>
		/// The maximum database column position.
		/// </returns>

		public int GetMaximumDatabaseColumnPosition()
		{
			try
			{
				return _maxDatabaseVariables;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// This method returns a ProfileList containing all database variables.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public ProfileList GetDatabaseVariableList()
		{
			try
			{
				return _databaseVariableList;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

        // Begin TT#1560 - JSmith - Create filter to Out stores w no OH-put in Rule, processed. Vel still alloc
        public VariableProfile GetVariableProfileByRegOrTotal(VariableProfile aVP, eOTSPlanLevelType aOTSPlanLevelType)
        {
            try
            {
                switch (aOTSPlanLevelType)
                {
                    case eOTSPlanLevelType.Regular:
                        if (aVP.VariableType == eVariableType.Sales)
                        {
                            return SalesRegularUnits;
                        }
                        else if (aVP.VariableType == eVariableType.BegStock)
                        {
                            return InventoryRegularUnits;
                        }
                        else if (aVP.VariableType == eVariableType.EndStock)
                        {
                            return InventoryRegularUnits;
                        }
                        else if (aVP.VariableType == eVariableType.FWOS)
                        {
                            return ForwardWOSRegPromo;
                        }
                        break;
                    case eOTSPlanLevelType.Total:
                        if (aVP.VariableType == eVariableType.Sales)
                        {
                            return SalesTotalUnits;
                        }
                        else if (aVP.VariableType == eVariableType.BegStock)
                        {
                            return InventoryTotalUnits;
                        }
                        else if (aVP.VariableType == eVariableType.EndStock)
                        {
                            return InventoryTotalUnits;
                        }
                        else if (aVP.VariableType == eVariableType.FWOS)
                        {
                            return ForwardWOSTotal;
                        }
                        break;
                }

                return aVP;
            }
            catch (Exception exc)
            {
                throw;
            }
        }
        // End TT#1560 - JSmith - Create filter to Out stores w no OH-put in Rule, processed. Vel still alloc

        /// <summary>
        /// This method returns an ArrayList containing all variable groupings.  IComputations requirement.  DO NOT REMOVE.
        /// </summary>

        public ArrayList GetVariableGroupings()
        {
            try
            {
                return _variableGroupings;
            }
            catch (Exception exc)
            {
                throw;
            }
        }

		/// <summary>
		/// This method returns a ProfileList containing database variables for store weekly history.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetStoreWeeklyHistoryDatabaseVariableList()
		{
			if (_storeWeeklyHistoryDatabaseVariableList == null)
			{
				_storeWeeklyHistoryDatabaseVariableList = new ProfileList(eProfileType.Variable);
				foreach (VariableProfile vp in _databaseVariableList)
				{
					if (vp.StoreWeeklyHistoryModelType != eVariableDatabaseModelType.None)
					{
						_storeWeeklyHistoryDatabaseVariableList.Add(vp);
					}
				}
			}
			return _storeWeeklyHistoryDatabaseVariableList;
		}

		/// <summary>
		/// This method returns a ProfileList containing variables for store daily history.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetStoreDailyHistoryDatabaseVariableList()
		{
			if (_storeDailyHistoryDatabaseVariableList == null)
			{
				_storeDailyHistoryDatabaseVariableList = new ProfileList(eProfileType.Variable);
				foreach (VariableProfile vp in _databaseVariableList)
				{
					if (vp.StoreDailyHistoryModelType != eVariableDatabaseModelType.None)
					{
						_storeDailyHistoryDatabaseVariableList.Add(vp);
					}
				}
			}
			return _storeDailyHistoryDatabaseVariableList;
		}

		/// <summary>
		/// This method returns a ProfileList containing database variables for store weekly forecast.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetStoreWeeklyForecastDatabaseVariableList()
		{
			if (_storeWeeklyForecastDatabaseVariableList == null)
			{
				_storeWeeklyForecastDatabaseVariableList = new ProfileList(eProfileType.Variable);
				foreach (VariableProfile vp in _databaseVariableList)
				{
					if (vp.StoreForecastModelType != eVariableDatabaseModelType.None)
					{
						_storeWeeklyForecastDatabaseVariableList.Add(vp);
					}
				}
			}
			return _storeWeeklyForecastDatabaseVariableList;
		}

		/// <summary>
		/// This method returns a ProfileList containing database variables for chain weekly history.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetChainWeeklyHistoryDatabaseVariableList()
		{
			if (_chainWeeklyHistoryDatabaseVariableList == null)
			{
				_chainWeeklyHistoryDatabaseVariableList = new ProfileList(eProfileType.Variable);
				foreach (VariableProfile vp in _databaseVariableList)
				{
					if (vp.ChainHistoryModelType != eVariableDatabaseModelType.None)
					{
						_chainWeeklyHistoryDatabaseVariableList.Add(vp);
					}
				}
			}
			return _chainWeeklyHistoryDatabaseVariableList;
		}

		/// <summary>
		/// This method returns a ProfileList containing database variables for chain weekly forecast.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetChainWeeklyForecastDatabaseVariableList()
		{
			if (_chainWeeklyForecastDatabaseVariableList == null)
			{
				_chainWeeklyForecastDatabaseVariableList = new ProfileList(eProfileType.Variable);
				foreach (VariableProfile vp in _databaseVariableList)
				{
					if (vp.ChainForecastModelType != eVariableDatabaseModelType.None)
					{
						_chainWeeklyForecastDatabaseVariableList.Add(vp);
					}
				}
			}
			return _chainWeeklyForecastDatabaseVariableList;
		}

		/// <summary>
		/// This method returns a ProfileList containing database variables for store style weekly history.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetStoreWeeklyStyleHistoryDatabaseVariableList()
		{
			ProfileList profList = new ProfileList(eProfileType.Variable);
			foreach (VariableProfile vp in _databaseVariableList)
			{
				if (vp.StoreWeeklyHistoryModelType <= eVariableDatabaseModelType.All)
				{
					profList.Add(vp);
				}
			}
			return profList;
		}

		/// <summary>
		/// This method returns a ProfileList containing database variables for store style daily history.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetStoreDailyStyleHistoryDatabaseVariableList()
		{
			ProfileList profList = new ProfileList(eProfileType.Variable);
			foreach (VariableProfile vp in _databaseVariableList)
			{
				if (vp.StoreDailyHistoryModelType <= eVariableDatabaseModelType.All)
				{
					profList.Add(vp);
				}
			}
			return profList;
		}

		/// <summary>
		/// This method returns a ProfileList containing database variables for store style weekly forecast.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetStoreWeeklyStyleForecastDatabaseVariableList()
		{
			ProfileList profList = new ProfileList(eProfileType.Variable);
			foreach (VariableProfile vp in _databaseVariableList)
			{
				if (vp.StoreForecastModelType <= eVariableDatabaseModelType.All)
				{
					profList.Add(vp);
				}
			}
			return profList;
		}

		/// <summary>
		/// This method returns a ProfileList containing database variables for chain style weekly history.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetChainWeeklyStyleHistoryDatabaseVariableList()
		{
			ProfileList profList = new ProfileList(eProfileType.Variable);
			foreach (VariableProfile vp in _databaseVariableList)
			{
				if (vp.ChainHistoryModelType <= eVariableDatabaseModelType.All)
				{
					profList.Add(vp);
				}
			}
			return profList;
		}

		/// <summary>
		/// This method returns a ProfileList containing database variables for chain style weekly forecast.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetChainWeeklyStyleForecastDatabaseVariableList()
		{
			ProfileList profList = new ProfileList(eProfileType.Variable);
			foreach (VariableProfile vp in _databaseVariableList)
			{
				if (vp.ChainForecastModelType <= eVariableDatabaseModelType.All)
				{
					profList.Add(vp);
				}
			}
			return profList;
		}

		/// <summary>
		/// This method returns a ProfileList containing database variables for store color weekly history.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetStoreWeeklyColorHistoryDatabaseVariableList()
		{
			ProfileList profList = new ProfileList(eProfileType.Variable);
			foreach (VariableProfile vp in _databaseVariableList)
			{
				if (vp.StoreWeeklyHistoryModelType <= eVariableDatabaseModelType.All)
				{
					profList.Add(vp);
				}
			}
			return profList;
		}

		/// <summary>
		/// This method returns a ProfileList containing database variables for store color daily history.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetStoreDailyColorHistoryDatabaseVariableList()
		{
			ProfileList profList = new ProfileList(eProfileType.Variable);
			foreach (VariableProfile vp in _databaseVariableList)
			{
				if (vp.StoreDailyHistoryModelType <= eVariableDatabaseModelType.All)
				{
					profList.Add(vp);
				}
			}
			return profList;
		}

		/// <summary>
		/// This method returns a ProfileList containing database variables for store color weekly forecast.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetStoreWeeklyColorForecastDatabaseVariableList()
		{
			ProfileList profList = new ProfileList(eProfileType.Variable);
			foreach (VariableProfile vp in _databaseVariableList)
			{
				if (vp.StoreForecastModelType <= eVariableDatabaseModelType.All)
				{
					profList.Add(vp);
				}
			}
			return profList;
		}

		/// <summary>
		/// This method returns a ProfileList containing database variables for chain color weekly history.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetChainWeeklyColorHistoryDatabaseVariableList()
		{
			ProfileList profList = new ProfileList(eProfileType.Variable);
			foreach (VariableProfile vp in _databaseVariableList)
			{
				if (vp.ChainHistoryModelType <= eVariableDatabaseModelType.All)
				{
					profList.Add(vp);
				}
			}
			return profList;
		}

		/// <summary>
		/// This method returns a ProfileList containing database variables for chain color weekly forecast.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetChainWeeklyColorForecastDatabaseVariableList()
		{
			ProfileList profList = new ProfileList(eProfileType.Variable);
			foreach (VariableProfile vp in _databaseVariableList)
			{
				if (vp.ChainForecastModelType <= eVariableDatabaseModelType.All)
				{
					profList.Add(vp);
				}
			}
			return profList;
		}

		/// <summary>
		/// This method returns a ProfileList containing database variables for store size weekly history.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetStoreWeeklySizeHistoryDatabaseVariableList()
		{
			ProfileList profList = new ProfileList(eProfileType.Variable);
			foreach (VariableProfile vp in _databaseVariableList)
			{
				if (vp.StoreWeeklyHistoryModelType <= eVariableDatabaseModelType.All)
				{
					profList.Add(vp);
				}
			}
			return profList;
		}

		/// <summary>
		/// This method returns a ProfileList containing database variables for store size daily history.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetStoreDailySizeHistoryDatabaseVariableList()
		{
			ProfileList profList = new ProfileList(eProfileType.Variable);
			foreach (VariableProfile vp in _databaseVariableList)
			{
				if (vp.StoreDailyHistoryModelType <= eVariableDatabaseModelType.All)
				{
					profList.Add(vp);
				}
			}
			return profList;
		}

		/// <summary>
		/// This method returns a ProfileList containing database variables for store size weekly forecast.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetStoreWeeklySizeForecastDatabaseVariableList()
		{
			ProfileList profList = new ProfileList(eProfileType.Variable);
			foreach (VariableProfile vp in _databaseVariableList)
			{
				if (vp.StoreForecastModelType <= eVariableDatabaseModelType.All)
				{
					profList.Add(vp);
				}
			}
			return profList;
		}

		/// <summary>
		/// This method returns a ProfileList containing database variables for chain size weekly history.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetChainWeeklySizeHistoryDatabaseVariableList()
		{
			ProfileList profList = new ProfileList(eProfileType.Variable);
			foreach (VariableProfile vp in _databaseVariableList)
			{
				if (vp.ChainHistoryModelType <= eVariableDatabaseModelType.All)
				{
					profList.Add(vp);
				}
			}
			return profList;
		}

		/// <summary>
		/// This method returns a ProfileList containing database variables for chain size weekly forecast.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetChainWeeklySizeForecastDatabaseVariableList()
		{
			ProfileList profList = new ProfileList(eProfileType.Variable);
			foreach (VariableProfile vp in _databaseVariableList)
			{
				if (vp.ChainForecastModelType <= eVariableDatabaseModelType.All)
				{
					profList.Add(vp);
				}
			}
			return profList;
		}

        // Begin TT#2131-MD - JSmith - Halo Integration
        /// <summary>
		/// This method returns a ProfileList containing variables for store weekly forecast.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetStoreWeeklyVariableList()
        {
            ProfileList profList = new ProfileList(eProfileType.Variable);
            foreach (VariableProfile vp in _profileList)
            {
                if (vp.VariableCategory == eVariableCategory.Store
                    || vp.VariableCategory == eVariableCategory.Both)
                {
                    profList.Add(vp);
                }
            }
            return profList;
        }

        /// <summary>
		/// This method returns a ProfileList containing variables for store weekly forecast.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetChainWeeklyVariableList()
        {
            ProfileList profList = new ProfileList(eProfileType.Variable);
            foreach (VariableProfile vp in _profileList)
            {
                if (vp.VariableCategory == eVariableCategory.Chain
                    || vp.VariableCategory == eVariableCategory.Both)
                {
                    profList.Add(vp);
                }
            }
            return profList;
        }

        // End TT#2131-MD - JSmith - Halo Integration
    }

    /// <summary>
    /// The TimeTotalVariables class is where the Time Total Variable profiles are defined.
    /// </summary>
    /// <remarks>
    /// This class is where the Time Total Variable profiles are defined.  A ProfileList of all the Variables and a hash table for the Variables'
    /// name are built during contruction.
    /// </remarks>

    [Serializable]
	abstract public class BasePlanTimeTotalVariables : IPlanComputationTimeTotalVariables
	{
		//=======
		// FIELDS
		//=======

		private ProfileList _profileList;
		private int _seq;
		private int _futureSeq;

		public TimeTotalVariableProfile SalesTotalUnitsT1;
		public TimeTotalVariableProfile SalesTotalUnitsT2;
		public TimeTotalVariableProfile SalesTotalUnitsT3;
		public TimeTotalVariableProfile SalesRegularUnitsT1;
		public TimeTotalVariableProfile SalesRegularUnitsT2;
		public TimeTotalVariableProfile SalesPromoUnitsT1;
		public TimeTotalVariableProfile SalesPromoUnitsT2;
		public TimeTotalVariableProfile SalesRegPromoUnitsT1;
		public TimeTotalVariableProfile SalesRegPromoUnitsT2;
		public TimeTotalVariableProfile SalesRegPromoUnitsT3;
		public TimeTotalVariableProfile SalesMarkdownUnitsT1;
		public TimeTotalVariableProfile SalesMarkdownUnitsT2;
		public TimeTotalVariableProfile SalesMarkdownUnitsT3;
		public TimeTotalVariableProfile SalesTotalSetIndexT1;
		public TimeTotalVariableProfile SalesRegularSetIndexT1;
		public TimeTotalVariableProfile SalesPromoSetIndexT1;
		public TimeTotalVariableProfile SalesRegPromoSetIndexT1;
		public TimeTotalVariableProfile SalesMarkdownSetIndexT1;
		public TimeTotalVariableProfile SalesTotalAllStoreIndexT1;
		public TimeTotalVariableProfile SalesRegularAllStoreIndexT1;
		public TimeTotalVariableProfile SalesPromoAllStoreIndexT1;
		public TimeTotalVariableProfile SalesRegPromoAllStoreIndexT1;
		public TimeTotalVariableProfile SalesMarkdownAllStoreIndexT1;
		public TimeTotalVariableProfile InventoryTotalUnitsT1;
		public TimeTotalVariableProfile InventoryTotalUnitsT2;
		public TimeTotalVariableProfile InventoryTotalUnitsT3;
		public TimeTotalVariableProfile InventoryTotalUnitsT4;
		public TimeTotalVariableProfile InventoryTotalUnitsT5;
		public TimeTotalVariableProfile InventoryTotalUnitsT6;
		public TimeTotalVariableProfile InventoryTotalUnitsT7;
        public TimeTotalVariableProfile InventoryTotalUnitsT8;      // TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
		public TimeTotalVariableProfile InventoryRegularUnitsT1;
		public TimeTotalVariableProfile InventoryRegularUnitsT2;
		public TimeTotalVariableProfile InventoryRegularUnitsT3;
		public TimeTotalVariableProfile InventoryRegularUnitsT4;
		public TimeTotalVariableProfile InventoryRegularUnitsT5;
		public TimeTotalVariableProfile InventoryRegularUnitsT6;
		public TimeTotalVariableProfile InventoryRegularUnitsT7;
        public TimeTotalVariableProfile InventoryRegularUnitsT8;    // TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
		public TimeTotalVariableProfile InventoryMarkdownUnitsT1;
		public TimeTotalVariableProfile InventoryMarkdownUnitsT2;
		public TimeTotalVariableProfile InventoryMarkdownUnitsT3;
		public TimeTotalVariableProfile InventoryTotalSetIndexT1;
		public TimeTotalVariableProfile InventoryTotalSetIndexT2;
		public TimeTotalVariableProfile InventoryTotalSetIndexT3;
		public TimeTotalVariableProfile InventoryRegularSetIndexT1;
		public TimeTotalVariableProfile InventoryRegularSetIndexT2;
		public TimeTotalVariableProfile InventoryRegularSetIndexT3;
		public TimeTotalVariableProfile InventoryMarkdownSetIndexT1;
		public TimeTotalVariableProfile InventoryMarkdownSetIndexT2;
		public TimeTotalVariableProfile InventoryMarkdownSetIndexT3;
		public TimeTotalVariableProfile InventoryTotalAllStoreIndexT1;
		public TimeTotalVariableProfile InventoryTotalAllStoreIndexT2;
		public TimeTotalVariableProfile InventoryTotalAllStoreIndexT3;
		public TimeTotalVariableProfile InventoryRegularAllStoreIndexT1;
		public TimeTotalVariableProfile InventoryRegularAllStoreIndexT2;
		public TimeTotalVariableProfile InventoryRegularAllStoreIndexT3;
		public TimeTotalVariableProfile InventoryMarkdownAllStoreIndexT1;
		public TimeTotalVariableProfile InventoryMarkdownAllStoreIndexT2;
		public TimeTotalVariableProfile InventoryMarkdownAllStoreIndexT3;
		public TimeTotalVariableProfile ReceiptTotalUnitsT1;
		public TimeTotalVariableProfile ReceiptRegularUnitsT1;
		public TimeTotalVariableProfile ReceiptMarkdownUnitsT1;
		public TimeTotalVariableProfile SalesStockRatioTotalT1;
		public TimeTotalVariableProfile SalesStockRatioRegPromoT1;
		public TimeTotalVariableProfile SalesStockRatioMarkdownT1;
		public TimeTotalVariableProfile ForwardWOSTotalT1;
		public TimeTotalVariableProfile ForwardWOSRegPromoT1;
		public TimeTotalVariableProfile ForwardWOSMarkdownT1;
		public TimeTotalVariableProfile ForwardWOSTotalSetIndexT1;
		public TimeTotalVariableProfile ForwardWOSRegPromoSetIndexT1;
		public TimeTotalVariableProfile ForwardWOSMarkdownSetIndexT1;
		public TimeTotalVariableProfile ForwardWOSTotalAllStoreIndexT1;
		public TimeTotalVariableProfile ForwardWOSRegPromoAllStoreIndexT1;
		public TimeTotalVariableProfile ForwardWOSMarkdownAllStoreIndexT1;
		public TimeTotalVariableProfile SellThruPctTotalT1;
		public TimeTotalVariableProfile SellThruPctRegPromoT1;
		public TimeTotalVariableProfile SellThruPctMarkdownT1;
		public TimeTotalVariableProfile SellThruPctTotalSetIndexT1;
		public TimeTotalVariableProfile SellThruPctRegPromoSetIndexT1;
		public TimeTotalVariableProfile SellThruPctMarkdownSetIndexT1;
		public TimeTotalVariableProfile SellThruPctTotalAllStoreIndexT1;
		public TimeTotalVariableProfile SellThruPctRegPromoAllStoreIndexT1;
		public TimeTotalVariableProfile SellThruPctMarkdownAllStoreIndexT1;
		public TimeTotalVariableProfile IntransitT1;
		public TimeTotalVariableProfile GradeTotalT1;
		public TimeTotalVariableProfile GradeRegPromoT1;
		//Begin TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
		public TimeTotalVariableProfile WOSTotalT1;
		public TimeTotalVariableProfile WOSRegPromoT1;
		//End TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
		// Begin TT#1224 - stodd - add committed
		public TimeTotalVariableProfile CommittedT1;
		// End TT#1224 - stodd - add committed

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Static.  Creates a new instance of aBasePlanTimeTotalVariables.
		/// </summary>

		public BasePlanTimeTotalVariables() 
		{
			try
			{
				_profileList = new ProfileList(eProfileType.TimeTotalVariable);
				_seq = 1;
				_futureSeq = 10000;

				SalesTotalUnitsT1 = new TimeTotalVariableProfile(NextSequence, "Tot Sales", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.PctContribution, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 0);
				SalesTotalUnitsT2 = new TimeTotalVariableProfile(NextSequence, "Avg Sales", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.PctChange, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SalesTotalUnitsT3 = new TimeTotalVariableProfile(NextSequence, "WTD Sales", eVariableCategory.Store, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.None, eValueFormatType.GenericNumeric, 0);
				SalesRegularUnitsT1 = new TimeTotalVariableProfile(NextSequence, "Tot Sales Reg", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.PctContribution, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 0);
				SalesRegularUnitsT2 = new TimeTotalVariableProfile(NextSequence, "WTD Sales Reg", eVariableCategory.Store, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.None, eValueFormatType.GenericNumeric, 0);
				SalesPromoUnitsT1 = new TimeTotalVariableProfile(NextSequence, "Tot Sales Promo", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.PctContribution, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 0);
				SalesPromoUnitsT2 = new TimeTotalVariableProfile(NextSequence, "WTD Sales Promo", eVariableCategory.Store, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.None, eValueFormatType.GenericNumeric, 0);
				SalesRegPromoUnitsT1 = new TimeTotalVariableProfile(NextSequence, "Tot Sales R/P", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 0);
				SalesRegPromoUnitsT2 = new TimeTotalVariableProfile(NextSequence, "Avg Sales R/P", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SalesRegPromoUnitsT3 = new TimeTotalVariableProfile(NextSequence, "WTD Sales R/P", eVariableCategory.Store, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.None, eValueFormatType.GenericNumeric, 0);
				SalesMarkdownUnitsT1 = new TimeTotalVariableProfile(NextSequence, "Tot Sales Mkdn", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 0);
				SalesMarkdownUnitsT2 = new TimeTotalVariableProfile(NextSequence, "Avg Sales Mkdn", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SalesMarkdownUnitsT3 = new TimeTotalVariableProfile(NextSequence, "WTD Sales Mkdn", eVariableCategory.Store, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.None, eValueFormatType.GenericNumeric, 0);
				SalesTotalSetIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot Set Sales IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SalesRegularSetIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot Set Sales Reg IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SalesPromoSetIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot Set Sales Promo IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SalesRegPromoSetIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot Set Sales R/P IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SalesMarkdownSetIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot Set Sales Mkdn IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SalesTotalAllStoreIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot All Sales IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SalesRegularAllStoreIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot All Sales Reg IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SalesPromoAllStoreIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot All Sales Promo IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SalesRegPromoAllStoreIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot All Sales R/P IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SalesMarkdownAllStoreIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot All Sales Mkdn IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				InventoryTotalUnitsT1 = new TimeTotalVariableProfile(NextSequence, "Beg Stock", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.PctContribution, eVariableWeekType.BOW, eVariableTimeTotalType.First, eValueFormatType.GenericNumeric, 0);
				InventoryTotalUnitsT2 = new TimeTotalVariableProfile(NextSequence, "Avg Stock", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.AllPlusNext, eValueFormatType.GenericNumeric, 2);
				InventoryTotalUnitsT3 = new TimeTotalVariableProfile(NextSequence, "End Stock", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.Next, eValueFormatType.GenericNumeric, 0);
				InventoryTotalUnitsT4 = new TimeTotalVariableProfile(NextSequence, "On Hand", eVariableCategory.Store, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.None, eValueFormatType.GenericNumeric, 0);
				InventoryTotalUnitsT5 = new TimeTotalVariableProfile(NextSequence, "Tot Fill", eVariableCategory.Store, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.None, eValueFormatType.GenericNumeric, 0);
				InventoryTotalUnitsT6 = new TimeTotalVariableProfile(NextSequence, "Tot Need", eVariableCategory.Store, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.None, eValueFormatType.GenericNumeric, 0);
			    InventoryTotalUnitsT7 = new TimeTotalVariableProfile(NextSequence, "Tot % Need", eVariableCategory.Store, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.None, eValueFormatType.GenericNumeric, 2);
                // Begin TT#28 - RMatelic - Filter Explorer - Receive unhandled exception when selected some of the filters, but not all of them >>> moved definition to end of list
                // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
                //InventoryTotalUnitsT8 = new TimeTotalVariableProfile(NextSequence, "VSW On Hand", eVariableCategory.Store, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.None, eValueFormatType.GenericNumeric, 0);
				// End TT#2054
                // End TT#28
                InventoryRegularUnitsT1 = new TimeTotalVariableProfile(NextSequence, "Beg Stock Reg", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.PctContribution, eVariableWeekType.BOW, eVariableTimeTotalType.First, eValueFormatType.GenericNumeric, 0);
				InventoryRegularUnitsT2 = new TimeTotalVariableProfile(NextSequence, "Avg Stock Reg", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.AllPlusNext, eValueFormatType.GenericNumeric, 2);
				InventoryRegularUnitsT3 = new TimeTotalVariableProfile(NextSequence, "End Stock Reg", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.Next, eValueFormatType.GenericNumeric, 0);
				InventoryRegularUnitsT4 = new TimeTotalVariableProfile(NextSequence, "On Hand", eVariableCategory.Store, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.None, eValueFormatType.GenericNumeric, 0);
				InventoryRegularUnitsT5 = new TimeTotalVariableProfile(NextSequence, "Reg Fill", eVariableCategory.Store, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.None, eValueFormatType.GenericNumeric, 0);
				InventoryRegularUnitsT6 = new TimeTotalVariableProfile(NextSequence, "Reg Need", eVariableCategory.Store, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.None, eValueFormatType.GenericNumeric, 0);
				InventoryRegularUnitsT7 = new TimeTotalVariableProfile(NextSequence, "Reg % Need", eVariableCategory.Store, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.None, eValueFormatType.GenericNumeric, 2);
                // Begin TT#28 - RMatelic - Filter Explorer - Receive unhandled exception when selected some of the filters, but not all of them >>> moved definition to end of list
                // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
                //InventoryRegularUnitsT8 = new TimeTotalVariableProfile(NextSequence, "VSW On Hand", eVariableCategory.Store, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.None, eValueFormatType.GenericNumeric, 0);
				// End TT#2054
                // End TT#28
                InventoryMarkdownUnitsT1 = new TimeTotalVariableProfile(NextSequence, "Beg Stock Mkdn", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.First, eValueFormatType.GenericNumeric, 0);
				InventoryMarkdownUnitsT2 = new TimeTotalVariableProfile(NextSequence, "Avg Stock Mkdn", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.AllPlusNext, eValueFormatType.GenericNumeric, 2);
				InventoryMarkdownUnitsT3 = new TimeTotalVariableProfile(NextSequence, "End Stock Mkdn", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.Next, eValueFormatType.GenericNumeric, 0);
				InventoryTotalSetIndexT1 = new TimeTotalVariableProfile(NextSequence, "Beg Set Stock IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.First, eValueFormatType.GenericNumeric, 2);
				InventoryTotalSetIndexT2 = new TimeTotalVariableProfile(NextSequence, "Avg Set Stock IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				InventoryTotalSetIndexT3 = new TimeTotalVariableProfile(NextSequence, "End Set Stock IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.Next, eValueFormatType.GenericNumeric, 2);
				InventoryRegularSetIndexT1 = new TimeTotalVariableProfile(NextSequence, "Beg Set Stock Reg IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.First, eValueFormatType.GenericNumeric, 2);
				InventoryRegularSetIndexT2 = new TimeTotalVariableProfile(NextSequence, "Avg Set Stock Reg IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				InventoryRegularSetIndexT3 = new TimeTotalVariableProfile(NextSequence, "End Set Stock Reg IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.Next, eValueFormatType.GenericNumeric, 2);
				InventoryMarkdownSetIndexT1 = new TimeTotalVariableProfile(NextSequence, "Beg Set Stock Mkdn IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.First, eValueFormatType.GenericNumeric, 2);
				InventoryMarkdownSetIndexT2 = new TimeTotalVariableProfile(NextSequence, "Avg Set Stock Mkdn IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				InventoryMarkdownSetIndexT3 = new TimeTotalVariableProfile(NextSequence, "End Set Stock Mkdn IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.Next, eValueFormatType.GenericNumeric, 2);
				InventoryTotalAllStoreIndexT1 = new TimeTotalVariableProfile(NextSequence, "Beg All Stock IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.First, eValueFormatType.GenericNumeric, 2);
				InventoryTotalAllStoreIndexT2 = new TimeTotalVariableProfile(NextSequence, "Avg All Stock IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				InventoryTotalAllStoreIndexT3 = new TimeTotalVariableProfile(NextSequence, "End All Stock IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.Next, eValueFormatType.GenericNumeric, 2);
				InventoryRegularAllStoreIndexT1 = new TimeTotalVariableProfile(NextSequence, "Beg All Stock Reg IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.First, eValueFormatType.GenericNumeric, 2);
				InventoryRegularAllStoreIndexT2 = new TimeTotalVariableProfile(NextSequence, "Avg All Stock Reg IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				InventoryRegularAllStoreIndexT3 = new TimeTotalVariableProfile(NextSequence, "End All Stock Reg IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.Next, eValueFormatType.GenericNumeric, 2);
				InventoryMarkdownAllStoreIndexT1 = new TimeTotalVariableProfile(NextSequence, "Beg All Stock Mkdn IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.First, eValueFormatType.GenericNumeric, 2);
				InventoryMarkdownAllStoreIndexT2 = new TimeTotalVariableProfile(NextSequence, "Avg All Stock Mkdn IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				InventoryMarkdownAllStoreIndexT3 = new TimeTotalVariableProfile(NextSequence, "End All Stock Mkdn IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.Next, eValueFormatType.GenericNumeric, 2);
				ReceiptTotalUnitsT1 = new TimeTotalVariableProfile(NextSequence, "Tot Receipts", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 0);
				ReceiptRegularUnitsT1 = new TimeTotalVariableProfile(NextSequence, "Tot Receipts Reg", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 0);
				ReceiptMarkdownUnitsT1 = new TimeTotalVariableProfile(NextSequence, "Tot Receipts Mkdn", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 0);
				SalesStockRatioTotalT1 = new TimeTotalVariableProfile(NextSequence, "Tot S/S Ratio", eVariableCategory.Both, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SalesStockRatioRegPromoT1 = new TimeTotalVariableProfile(NextSequence, "Tot S/S Ratio R/P", eVariableCategory.Both, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SalesStockRatioMarkdownT1 = new TimeTotalVariableProfile(NextSequence, "Tot S/S Ratio Mkdn", eVariableCategory.Both, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				ForwardWOSTotalT1 = new TimeTotalVariableProfile(NextSequence, "Tot FWOS", eVariableCategory.Both, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.First, eValueFormatType.GenericNumeric, 2);
				ForwardWOSRegPromoT1 = new TimeTotalVariableProfile(NextSequence, "Tot FWOS R/P", eVariableCategory.Both, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.First, eValueFormatType.GenericNumeric, 2);
				ForwardWOSMarkdownT1 = new TimeTotalVariableProfile(NextSequence, "Tot FWOS Mkdn", eVariableCategory.Both, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.First, eValueFormatType.GenericNumeric, 2);
				ForwardWOSTotalSetIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot Set FWOS IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.First, eValueFormatType.GenericNumeric, 2);
				ForwardWOSRegPromoSetIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot Set FWOS R/P IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.First, eValueFormatType.GenericNumeric, 2);
				ForwardWOSMarkdownSetIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot Set FWOS Mkdn IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.First, eValueFormatType.GenericNumeric, 2);
				ForwardWOSTotalAllStoreIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot All FWOS IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.First, eValueFormatType.GenericNumeric, 2);
				ForwardWOSRegPromoAllStoreIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot All FWOS R/P IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.First, eValueFormatType.GenericNumeric, 2);
				ForwardWOSMarkdownAllStoreIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot All FWOS Mkdn IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.First, eValueFormatType.GenericNumeric, 2);
				SellThruPctTotalT1 = new TimeTotalVariableProfile(NextSequence, "Tot Sell Thru % ", eVariableCategory.Both, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SellThruPctRegPromoT1 = new TimeTotalVariableProfile(NextSequence, "Tot Sell Thru % R/P", eVariableCategory.Both, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SellThruPctMarkdownT1 = new TimeTotalVariableProfile(NextSequence, "Tot Sell Thru % Mkdn", eVariableCategory.Both, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SellThruPctTotalSetIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot Set Sell Thru % IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SellThruPctRegPromoSetIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot Set Sell Thru % R/P IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SellThruPctMarkdownSetIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot Set Sell Thru % Mkdn IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SellThruPctTotalAllStoreIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot All Sell Thru % IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SellThruPctRegPromoAllStoreIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot All Sell Thru % R/P IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				SellThruPctMarkdownAllStoreIndexT1 = new TimeTotalVariableProfile(NextSequence, "Tot All Sell Thru % Mkdn IDX", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.DisplayOnly, eVariableScope.Dynamic, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				IntransitT1 = new TimeTotalVariableProfile(NextSequence, "Tot Intransit", eVariableCategory.Store, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 0);
				GradeTotalT1 = new TimeTotalVariableProfile(NextSequence, "Tot Grade", eVariableCategory.Store, eVariableStyle.None, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.StoreGrade, 0);
				GradeRegPromoT1 = new TimeTotalVariableProfile(NextSequence, "Tot Grade R/P", eVariableCategory.Store, eVariableStyle.None, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.StoreGrade, 0);
				//NOTE -- NOTE -- NOTE
				//NOTE -- NOTE -- NOTE
				//When adding any new base variables, DO NOT USE the NextSequence property for the sequence #.  Use NextFutureSequence.  This is to prevent the renumbering
				//of Client-specific variables that would invalidate Filters and Forcasting Models.
				//NOTE -- NOTE -- NOTE
				//NOTE -- NOTE -- NOTE
				//Begin TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
				WOSTotalT1 = new TimeTotalVariableProfile(NextSequence, "Tot Fcst WOS", eVariableCategory.Chain, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.Plug, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				WOSRegPromoT1 = new TimeTotalVariableProfile(NextSequence, "Tot Fcst WOS R/P", eVariableCategory.Chain, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.Plug, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 2);
				//End TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
				// Begin TT#1224 - stodd - add committed
				CommittedT1 = new TimeTotalVariableProfile(NextSequence, "Tot Committed", eVariableCategory.Store, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 0);
				// End TT#1224 - stodd - add committed
                // Begin TT#28 - RMatelic - Filter Explorer - Receive unhandled exception when selected some of the filters, but not all of them >>> moved definition from above
                InventoryTotalUnitsT8 = new TimeTotalVariableProfile(NextSequence, "VSW On Hand", eVariableCategory.Store, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.None, eValueFormatType.GenericNumeric, 0);
                InventoryRegularUnitsT8 = new TimeTotalVariableProfile(NextSequence, "VSW On Hand", eVariableCategory.Store, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.BOW, eVariableTimeTotalType.None, eValueFormatType.GenericNumeric, 0);
                // End TT#28
			}
			catch (Exception exc)
			{
				throw;
			}
		}
		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the next available variable sequence number.
		/// </summary>

		protected int NextSequence
		{
			get
			{
				return _seq++;
			}
		}

		/// <summary>
		/// Gets the next available variable sequence number for future variables.
		/// </summary>

		protected int NextFutureSequence
		{
			get
			{
				return _futureSeq++;
			}
		}

		/// <summary>
		/// Gets the SalesTotalUnits total variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile TotalSalesTotalUnitsVariable
		{
			get
			{
				return SalesTotalUnitsT1;
			}
		}

		/// <summary>
		/// Gets the SalesTotalUnits average variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile AverageSalesTotalUnitsVariable
		{
			get
			{
				return SalesTotalUnitsT2;
			}
		}

		/// <summary>
		/// Gets the SalesRegPromoUnits total variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile TotalSalesRegPromoUnitsVariable
		{
			get
			{
				return SalesRegPromoUnitsT1;
			}
		}

		/// <summary>
		/// Gets the SalesRegPromoUnits average variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile AverageSalesRegPromoUnitsVariable
		{
			get
			{
				return SalesRegPromoUnitsT2;
			}
		}

		/// <summary>
		/// Gets the SalesTotalSetIndex total variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile TotalSalesTotalSetIndexVariable
		{
			get
			{
				return SalesTotalSetIndexT1;
			}
		}

		/// <summary>
		/// Gets the SalesTotalAllStoreIndex total variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile TotalSalesTotalAllStoreIndexVariable
		{
			get
			{
				return SalesTotalAllStoreIndexT1;
			}
		}

		/// <summary>
		/// Gets the SalesRegPromoSetIndex total variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile TotalSalesRegPromoSetIndexVariable
		{
			get
			{
				return SalesRegPromoSetIndexT1;
			}
		}

		/// <summary>
		/// Gets the SalesRegPromoAllStoreIndex total variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile TotalSalesRegPromoAllStoreIndexVariable
		{
			get
			{
				return SalesRegPromoAllStoreIndexT1;
			}
		}
        //Begin TT#855-MD -jsobek -Velocity Enhancements 
        public TimeTotalVariableProfile TotalInventoryRegularAllStoreIndexVariable
        {
            get
            {
                return InventoryRegularAllStoreIndexT1;
            }
        }
        public TimeTotalVariableProfile TotalInventoryTotalAllStoreIndexVariable
        {
            get
            {
                return InventoryTotalAllStoreIndexT1;
            }
        }
        public TimeTotalVariableProfile TotalInventoryRegularSetIndexVariable
        {
            get
            {
                return InventoryRegularSetIndexT1;
            }
        }
        public TimeTotalVariableProfile TotalInventoryTotalSetIndexVariable
        {
            get
            {
                return InventoryTotalSetIndexT1;
            }
        }
        //End TT#855-MD -jsobek -Velocity Enhancements 

		/// <summary>
		/// Gets the InventoryTotalUnits beginning variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile BeginningInventoryTotalUnitsVariable
		{
			get
			{
				return InventoryTotalUnitsT1;
			}
		}

		/// <summary>
		/// Gets the InventoryTotalUnits average variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile AverageInventoryTotalUnitsVariable
		{
			get
			{
				return InventoryTotalUnitsT2;
			}
		}

		/// <summary>
		/// Gets the InventoryRegularUnits beginning variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile BeginningInventoryRegularUnitsVariable
		{
			get
			{
				return InventoryRegularUnitsT1;
			}
		}

		/// <summary>
		/// Gets the InventoryRegularUnits average variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile AverageInventoryRegularUnitsVariable
		{
			get
			{
				return InventoryRegularUnitsT2;
			}
		}

		/// <summary>
		/// Gets the SellThruPctTotal total variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile TotalSellThruPctTotalVariable
		{
			get
			{
				return SellThruPctTotalT1;
			}
		}

		/// <summary>
		/// Gets the SellThruPctTotalSetIndex total variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile TotalSellThruPctTotalSetIndexVariable
		{
			get
			{
				return SellThruPctTotalSetIndexT1;
			}
		}

		/// <summary>
		/// Gets the SellThruPctTotalAllStoreIndex total variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile TotalSellThruPctTotalAllStoreIndexVariable
		{
			get
			{
				return SellThruPctTotalAllStoreIndexT1;
			}
		}

		/// <summary>
		/// Gets the SellThruPctRegPromo total variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile TotalSellThruPctRegPromoVariable
		{
			get
			{
				return SellThruPctRegPromoT1;
			}
		}

		/// <summary>
		/// Gets the SellThruPctRegPromoSetIndex total variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile TotalSellThruPctRegPromoSetIndexVariable
		{
			get
			{
				return SellThruPctRegPromoSetIndexT1;
			}
		}

		/// <summary>
		/// Gets the SellThruPctRegPromoAllStoreIndex total variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile TotalSellThruPctRegPromoAllStoreIndexVariable
		{
			get
			{
				return SellThruPctRegPromoAllStoreIndexT1;
			}
		}

		/// <summary>
		/// Gets the ReceiptRegularUnits total variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile TotalReceiptRegularUnitsVariable
		{
			get
			{
				return ReceiptRegularUnitsT1;
			}
		}

		/// <summary>
		/// Gets the ReceiptTotalUnits total variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile TotalReceiptTotalUnitsVariable
		{
			get
			{
				return ReceiptTotalUnitsT1;
			}
		}

		/// <summary>
		/// Gets the Intransit total variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile TotalIntransitVariable
		{
			get
			{
				return IntransitT1;
			}
		}

		/// <summary>
		/// Gets the InventoryRegularUnits total variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile TotalOnHandRegularUnitsVariable
		{
			get
			{
				return InventoryRegularUnitsT4;
			}
		}

		/// <summary>
		/// Gets the InventoryTotalUnits total variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile TotalOnHandTotalUnitsVariable
		{
			get
			{
				return InventoryTotalUnitsT4;
			}
		}

		/// <summary>
		/// Gets the InventoryRegularUnits total variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile TotalNeedRegularVariable
		{
			get
			{
				return InventoryRegularUnitsT6;
			}
		}

		/// <summary>
		/// Gets the InventoryTotalUnits total variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile TotalNeedTotalVariable
		{
			get
			{
				return InventoryTotalUnitsT6;
			}
		}

        // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
        public TimeTotalVariableProfile TotalVSWOnHandRegularUnitsVariable
        {
            get
            {
                return InventoryRegularUnitsT8;
            }
        }

        public TimeTotalVariableProfile TotalVSWOnHandTotalUnitsVariable
        {
            get
            {
                return InventoryTotalUnitsT8;
            }
        }
        // End TT#2054

        // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
        /// <summary>
        /// Gets the GradeTotal total variable.  IComputations requirement.  DO NOT REMOVE.
        /// </summary>
        public TimeTotalVariableProfile TotalGradeTotalVariable
        {
            get
            {
                return GradeTotalT1;
            }
        }
        /// <summary>
        /// Gets the GradeTotal total variable.  IComputations requirement.  DO NOT REMOVE.
        /// </summary>
        public TimeTotalVariableProfile TotalGradeRegPromoVariable
        {
            get
            {
                return GradeRegPromoT1;
            }
        }
        // End TT#638 

		// Begin TT#1224 - stodd - assortment
		/// <summary>
		/// Gets the Committed total variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public TimeTotalVariableProfile TotalCommittedVariable
		{
			get
			{
				return CommittedT1;
			}
		}
		// End TT#1224 - stodd - assortment

		/// <summary>
		/// Gets the number of Time Total Variables.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public int NumTimeTotalVariables
		{
			get
			{
				try
				{
					return _profileList.Count;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the ProfileList of Time Total Variables.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public ProfileList TimeTotalVariableProfileList
		{
			get
			{
				return _profileList;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Initializes this instance of BasePlanTimeTotalVariables.
		/// </summary>
		/// <param name="aBasePlanVariables">
		/// The Variables object to initialize with.
		/// </param>

		public void Initialize(BasePlanVariables aBasePlanVariables)
		{
			try
			{
				InitializeVariables(aBasePlanVariables);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Initialized the variables for this instance.
		/// </summary>
		/// <param name="aBasePlanVariables">
		/// The Variables object to initialize with.
		/// </param>

		virtual protected void InitializeVariables(BasePlanVariables aBasePlanVariables)
		{
			try
			{
				// Add variables to profile list

				_profileList.Add(SalesTotalUnitsT1);
				_profileList.Add(SalesTotalUnitsT2);
				_profileList.Add(SalesTotalUnitsT3);
				_profileList.Add(SalesRegularUnitsT1);
				_profileList.Add(SalesRegularUnitsT2);
				_profileList.Add(SalesPromoUnitsT1);
				_profileList.Add(SalesPromoUnitsT2);
				_profileList.Add(SalesRegPromoUnitsT1);
				_profileList.Add(SalesRegPromoUnitsT2);
				_profileList.Add(SalesRegPromoUnitsT3);
				_profileList.Add(SalesMarkdownUnitsT1);
				_profileList.Add(SalesMarkdownUnitsT2);
				_profileList.Add(SalesMarkdownUnitsT3);
				_profileList.Add(SalesTotalSetIndexT1);
				_profileList.Add(SalesRegularSetIndexT1);
				_profileList.Add(SalesPromoSetIndexT1);
				_profileList.Add(SalesRegPromoSetIndexT1);
				_profileList.Add(SalesMarkdownSetIndexT1);
				_profileList.Add(SalesTotalAllStoreIndexT1);
				_profileList.Add(SalesRegularAllStoreIndexT1);
				_profileList.Add(SalesPromoAllStoreIndexT1);
				_profileList.Add(SalesRegPromoAllStoreIndexT1);
				_profileList.Add(SalesMarkdownAllStoreIndexT1);
				_profileList.Add(InventoryTotalUnitsT1);
				_profileList.Add(InventoryTotalUnitsT2);
				_profileList.Add(InventoryTotalUnitsT3);
				_profileList.Add(InventoryTotalUnitsT4);
				_profileList.Add(InventoryTotalUnitsT5);
				_profileList.Add(InventoryTotalUnitsT6);
				_profileList.Add(InventoryTotalUnitsT7);
                // Begin TT#28 - RMatelic - Filter Explorer - Receive unhandled exception when selected some of the filters, but not all of them >>> moved definition to end of list
                //_profileList.Add(InventoryTotalUnitsT8);        // TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
                // End TT#28
				_profileList.Add(InventoryRegularUnitsT1);
				_profileList.Add(InventoryRegularUnitsT2);
				_profileList.Add(InventoryRegularUnitsT3);
				_profileList.Add(InventoryRegularUnitsT4);
				_profileList.Add(InventoryRegularUnitsT5);
				_profileList.Add(InventoryRegularUnitsT6);
				_profileList.Add(InventoryRegularUnitsT7);
                // Begin TT#28 - RMatelic - Filter Explorer - Receive unhandled exception when selected some of the filters, but not all of them >>> moved definition to end of list
                //_profileList.Add(InventoryRegularUnitsT8);      // TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
                // End TT#28
				_profileList.Add(InventoryMarkdownUnitsT1);
				_profileList.Add(InventoryMarkdownUnitsT2);
				_profileList.Add(InventoryMarkdownUnitsT3);
				_profileList.Add(InventoryTotalSetIndexT1);
				_profileList.Add(InventoryTotalSetIndexT2);
				_profileList.Add(InventoryTotalSetIndexT3);
				_profileList.Add(InventoryRegularSetIndexT1);
				_profileList.Add(InventoryRegularSetIndexT2);
				_profileList.Add(InventoryRegularSetIndexT3);
				_profileList.Add(InventoryMarkdownSetIndexT1);
				_profileList.Add(InventoryMarkdownSetIndexT2);
				_profileList.Add(InventoryMarkdownSetIndexT3);
				_profileList.Add(InventoryTotalAllStoreIndexT1);
				_profileList.Add(InventoryTotalAllStoreIndexT2);
				_profileList.Add(InventoryTotalAllStoreIndexT3);
				_profileList.Add(InventoryRegularAllStoreIndexT1);
				_profileList.Add(InventoryRegularAllStoreIndexT2);
				_profileList.Add(InventoryRegularAllStoreIndexT3);
				_profileList.Add(InventoryMarkdownAllStoreIndexT1);
				_profileList.Add(InventoryMarkdownAllStoreIndexT2);
				_profileList.Add(InventoryMarkdownAllStoreIndexT3);
				_profileList.Add(ReceiptTotalUnitsT1);
				_profileList.Add(ReceiptRegularUnitsT1);
				_profileList.Add(ReceiptMarkdownUnitsT1);
				_profileList.Add(SalesStockRatioTotalT1);
				_profileList.Add(SalesStockRatioRegPromoT1);
				_profileList.Add(SalesStockRatioMarkdownT1);
				_profileList.Add(ForwardWOSTotalT1);
				_profileList.Add(ForwardWOSRegPromoT1);
				_profileList.Add(ForwardWOSMarkdownT1);
				_profileList.Add(ForwardWOSTotalSetIndexT1);
				_profileList.Add(ForwardWOSRegPromoSetIndexT1);
				_profileList.Add(ForwardWOSMarkdownSetIndexT1);
				_profileList.Add(ForwardWOSTotalAllStoreIndexT1);
				_profileList.Add(ForwardWOSRegPromoAllStoreIndexT1);
				_profileList.Add(ForwardWOSMarkdownAllStoreIndexT1);
				_profileList.Add(SellThruPctTotalT1);
				_profileList.Add(SellThruPctRegPromoT1);
				_profileList.Add(SellThruPctMarkdownT1);
				_profileList.Add(SellThruPctTotalSetIndexT1);
				_profileList.Add(SellThruPctRegPromoSetIndexT1);
				_profileList.Add(SellThruPctMarkdownSetIndexT1);
				_profileList.Add(SellThruPctTotalAllStoreIndexT1);
				_profileList.Add(SellThruPctRegPromoAllStoreIndexT1);
				_profileList.Add(SellThruPctMarkdownAllStoreIndexT1);
				_profileList.Add(IntransitT1);
				_profileList.Add(GradeTotalT1);
				_profileList.Add(GradeRegPromoT1);
				//Begin TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
				_profileList.Add(WOSTotalT1);
				_profileList.Add(WOSRegPromoT1);
				//End TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
				// Begin TT#1224 - stodd - assortment
				_profileList.Add(CommittedT1);
				// Begin TT#1224 - stodd - assortment
                
                // Begin TT#28 - RMatelic - Filter Explorer - Receive unhandled exception when selected some of the filters, but not all of them >>> moved from above
                _profileList.Add(InventoryTotalUnitsT8);        // TT#28 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
                _profileList.Add(InventoryRegularUnitsT8);      // TT#28 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
                // End TT#28 

				// Assign parent variable property
				
				SalesTotalUnitsT1.ParentVariableProfile = aBasePlanVariables.SalesTotalUnits;
				SalesTotalUnitsT2.ParentVariableProfile = aBasePlanVariables.SalesTotalUnits;
				SalesTotalUnitsT3.ParentVariableProfile = aBasePlanVariables.SalesTotalUnits;
				SalesRegularUnitsT1.ParentVariableProfile = aBasePlanVariables.SalesRegularUnits;
				SalesRegularUnitsT2.ParentVariableProfile = aBasePlanVariables.SalesRegularUnits;
				SalesPromoUnitsT1.ParentVariableProfile = aBasePlanVariables.SalesPromoUnits;
				SalesPromoUnitsT2.ParentVariableProfile = aBasePlanVariables.SalesPromoUnits;
				SalesRegPromoUnitsT1.ParentVariableProfile = aBasePlanVariables.SalesRegPromoUnits;
				SalesRegPromoUnitsT2.ParentVariableProfile = aBasePlanVariables.SalesRegPromoUnits;
				SalesRegPromoUnitsT3.ParentVariableProfile = aBasePlanVariables.SalesRegPromoUnits;
				SalesMarkdownUnitsT1.ParentVariableProfile = aBasePlanVariables.SalesMarkdownUnits;
				SalesMarkdownUnitsT2.ParentVariableProfile = aBasePlanVariables.SalesMarkdownUnits;
				SalesMarkdownUnitsT3.ParentVariableProfile = aBasePlanVariables.SalesMarkdownUnits;
				SalesTotalSetIndexT1.ParentVariableProfile = aBasePlanVariables.SalesTotalSetIndex;
				SalesRegularSetIndexT1.ParentVariableProfile = aBasePlanVariables.SalesRegularSetIndex;
				SalesPromoSetIndexT1.ParentVariableProfile = aBasePlanVariables.SalesPromoSetIndex;
				SalesRegPromoSetIndexT1.ParentVariableProfile = aBasePlanVariables.SalesRegPromoSetIndex;
				SalesMarkdownSetIndexT1.ParentVariableProfile = aBasePlanVariables.SalesMarkdownSetIndex;
				SalesTotalAllStoreIndexT1.ParentVariableProfile = aBasePlanVariables.SalesTotalAllStoreIndex;
				SalesRegularAllStoreIndexT1.ParentVariableProfile = aBasePlanVariables.SalesRegularAllStoreIndex;
				SalesPromoAllStoreIndexT1.ParentVariableProfile = aBasePlanVariables.SalesPromoAllStoreIndex;
				SalesRegPromoAllStoreIndexT1.ParentVariableProfile = aBasePlanVariables.SalesRegPromoAllStoreIndex;
				SalesMarkdownAllStoreIndexT1.ParentVariableProfile = aBasePlanVariables.SalesMarkdownAllStoreIndex;
				InventoryTotalUnitsT1.ParentVariableProfile = aBasePlanVariables.InventoryTotalUnits;
				InventoryTotalUnitsT2.ParentVariableProfile = aBasePlanVariables.InventoryTotalUnits;
				InventoryTotalUnitsT3.ParentVariableProfile = aBasePlanVariables.InventoryTotalUnits;
				InventoryTotalUnitsT4.ParentVariableProfile = aBasePlanVariables.InventoryTotalUnits;
				InventoryTotalUnitsT5.ParentVariableProfile = aBasePlanVariables.InventoryTotalUnits;
				InventoryTotalUnitsT6.ParentVariableProfile = aBasePlanVariables.InventoryTotalUnits;
				InventoryTotalUnitsT7.ParentVariableProfile = aBasePlanVariables.InventoryTotalUnits;
                InventoryTotalUnitsT8.ParentVariableProfile = aBasePlanVariables.InventoryTotalUnits;           // TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
				InventoryRegularUnitsT1.ParentVariableProfile = aBasePlanVariables.InventoryRegularUnits;
				InventoryRegularUnitsT2.ParentVariableProfile = aBasePlanVariables.InventoryRegularUnits;
				InventoryRegularUnitsT3.ParentVariableProfile = aBasePlanVariables.InventoryRegularUnits;
				InventoryRegularUnitsT4.ParentVariableProfile = aBasePlanVariables.InventoryRegularUnits;
				InventoryRegularUnitsT5.ParentVariableProfile = aBasePlanVariables.InventoryRegularUnits;
				InventoryRegularUnitsT6.ParentVariableProfile = aBasePlanVariables.InventoryRegularUnits;
				InventoryRegularUnitsT7.ParentVariableProfile = aBasePlanVariables.InventoryRegularUnits;
                InventoryRegularUnitsT8.ParentVariableProfile = aBasePlanVariables.InventoryRegularUnits;       // TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
				InventoryMarkdownUnitsT1.ParentVariableProfile = aBasePlanVariables.InventoryMarkdownUnits;
				InventoryMarkdownUnitsT2.ParentVariableProfile = aBasePlanVariables.InventoryMarkdownUnits;
				InventoryMarkdownUnitsT3.ParentVariableProfile = aBasePlanVariables.InventoryMarkdownUnits;
				InventoryTotalSetIndexT1.ParentVariableProfile = aBasePlanVariables.InventoryTotalSetIndex;
				InventoryTotalSetIndexT2.ParentVariableProfile = aBasePlanVariables.InventoryTotalSetIndex;
				InventoryTotalSetIndexT3.ParentVariableProfile = aBasePlanVariables.InventoryTotalSetIndex;
				InventoryRegularSetIndexT1.ParentVariableProfile = aBasePlanVariables.InventoryRegularSetIndex;
				InventoryRegularSetIndexT2.ParentVariableProfile = aBasePlanVariables.InventoryRegularSetIndex;
				InventoryRegularSetIndexT3.ParentVariableProfile = aBasePlanVariables.InventoryRegularSetIndex;
				InventoryMarkdownSetIndexT1.ParentVariableProfile = aBasePlanVariables.InventoryMarkdownSetIndex;
				InventoryMarkdownSetIndexT2.ParentVariableProfile = aBasePlanVariables.InventoryMarkdownSetIndex;
				InventoryMarkdownSetIndexT3.ParentVariableProfile = aBasePlanVariables.InventoryMarkdownSetIndex;
				InventoryTotalAllStoreIndexT1.ParentVariableProfile = aBasePlanVariables.InventoryTotalAllStoreIndex;
				InventoryTotalAllStoreIndexT2.ParentVariableProfile = aBasePlanVariables.InventoryTotalAllStoreIndex;
				InventoryTotalAllStoreIndexT3.ParentVariableProfile = aBasePlanVariables.InventoryTotalAllStoreIndex;
				InventoryRegularAllStoreIndexT1.ParentVariableProfile = aBasePlanVariables.InventoryRegularAllStoreIndex;
				InventoryRegularAllStoreIndexT2.ParentVariableProfile = aBasePlanVariables.InventoryRegularAllStoreIndex;
				InventoryRegularAllStoreIndexT3.ParentVariableProfile = aBasePlanVariables.InventoryRegularAllStoreIndex;
				InventoryMarkdownAllStoreIndexT1.ParentVariableProfile = aBasePlanVariables.InventoryMarkdownAllStoreIndex;
				InventoryMarkdownAllStoreIndexT2.ParentVariableProfile = aBasePlanVariables.InventoryMarkdownAllStoreIndex;
				InventoryMarkdownAllStoreIndexT3.ParentVariableProfile = aBasePlanVariables.InventoryMarkdownAllStoreIndex;
				ReceiptTotalUnitsT1.ParentVariableProfile = aBasePlanVariables.ReceiptTotalUnits;
				ReceiptRegularUnitsT1.ParentVariableProfile = aBasePlanVariables.ReceiptRegularUnits;
				ReceiptMarkdownUnitsT1.ParentVariableProfile = aBasePlanVariables.ReceiptMarkdownUnits;
				SalesStockRatioTotalT1.ParentVariableProfile = aBasePlanVariables.SalesStockRatioTotal;
				SalesStockRatioRegPromoT1.ParentVariableProfile = aBasePlanVariables.SalesStockRatioRegPromo;
				SalesStockRatioMarkdownT1.ParentVariableProfile = aBasePlanVariables.SalesStockRatioMarkdown;
				ForwardWOSTotalT1.ParentVariableProfile = aBasePlanVariables.ForwardWOSTotal;
				ForwardWOSRegPromoT1.ParentVariableProfile = aBasePlanVariables.ForwardWOSRegPromo;
				ForwardWOSMarkdownT1.ParentVariableProfile = aBasePlanVariables.ForwardWOSMarkdown;
				ForwardWOSTotalSetIndexT1.ParentVariableProfile = aBasePlanVariables.ForwardWOSTotalSetIndex;
				ForwardWOSRegPromoSetIndexT1.ParentVariableProfile = aBasePlanVariables.ForwardWOSRegPromoSetIndex;
				ForwardWOSMarkdownSetIndexT1.ParentVariableProfile = aBasePlanVariables.ForwardWOSMarkdownSetIndex;
				ForwardWOSTotalAllStoreIndexT1.ParentVariableProfile = aBasePlanVariables.ForwardWOSTotalAllStoreIndex;
				ForwardWOSRegPromoAllStoreIndexT1.ParentVariableProfile = aBasePlanVariables.ForwardWOSRegPromoAllStoreIndex;
				ForwardWOSMarkdownAllStoreIndexT1.ParentVariableProfile = aBasePlanVariables.ForwardWOSMarkdownAllStoreIndex;
				SellThruPctTotalT1.ParentVariableProfile = aBasePlanVariables.SellThruPctTotal;
				SellThruPctRegPromoT1.ParentVariableProfile = aBasePlanVariables.SellThruPctRegPromo;
				SellThruPctMarkdownT1.ParentVariableProfile = aBasePlanVariables.SellThruPctMarkdown;
				SellThruPctTotalSetIndexT1.ParentVariableProfile = aBasePlanVariables.SellThruPctTotalSetIndex;
				SellThruPctRegPromoSetIndexT1.ParentVariableProfile = aBasePlanVariables.SellThruPctRegPromoSetIndex;
				SellThruPctMarkdownSetIndexT1.ParentVariableProfile = aBasePlanVariables.SellThruPctMarkdownSetIndex;
				SellThruPctTotalAllStoreIndexT1.ParentVariableProfile = aBasePlanVariables.SellThruPctTotalAllStoreIndex;
				SellThruPctRegPromoAllStoreIndexT1.ParentVariableProfile = aBasePlanVariables.SellThruPctRegPromoAllStoreIndex;
				SellThruPctMarkdownAllStoreIndexT1.ParentVariableProfile = aBasePlanVariables.SellThruPctMarkdownAllStoreIndex;
				IntransitT1.ParentVariableProfile = aBasePlanVariables.Intransit;
				GradeTotalT1.ParentVariableProfile = aBasePlanVariables.GradeTotal;
				GradeRegPromoT1.ParentVariableProfile = aBasePlanVariables.GradeRegPromo;
				//Begin TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
				WOSTotalT1.ParentVariableProfile = aBasePlanVariables.WOSTotal;
				WOSRegPromoT1.ParentVariableProfile = aBasePlanVariables.WOSRegPromo;
				//End TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
				// Begin TT#1224 - stodd - assortment
				CommittedT1.ParentVariableProfile = aBasePlanVariables.Committed;
				// End TT#1224 - stodd - assortment
				
			}
			catch (Exception exc)
			{
				throw;
			}
		}

        // Begin TT#2131-MD - JSmith - Halo Integration
        /// <summary>
		/// This method returns a ProfileList containing variables for store weekly forecast.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetStoreTimeTotalVariableList()
        {
            ProfileList profList = new ProfileList(eProfileType.Variable);
            foreach (TimeTotalVariableProfile vp in _profileList)
            {
                if (vp.VariableCategory == eVariableCategory.Store
                    || vp.VariableCategory == eVariableCategory.Both)
                {
                    profList.Add(vp);
                }
            }
            return profList;
        }

        /// <summary>
		/// This method returns a ProfileList containing variables for store weekly forecast.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		public ProfileList GetChainTimeTotalVariableList()
        {
            ProfileList profList = new ProfileList(eProfileType.Variable);
            foreach (TimeTotalVariableProfile vp in _profileList)
            {
                if (vp.VariableCategory == eVariableCategory.Chain
                    || vp.VariableCategory == eVariableCategory.Both)
                {
                    profList.Add(vp);
                }
            }
            return profList;
        }

        // End TT#2131-MD - JSmith - Halo Integration
    }

    /// <summary>
    /// The QuantityVariables class is where the Time Total Variable profiles are defined.
    /// </summary>
    /// <remarks>
    /// This class is where the Time Total Variable profiles are defined.  A ProfileList of all the Variables and a hash table for the Variables'
    /// name are built during contruction.
    /// </remarks>

    [Serializable]
	abstract public class BasePlanQuantityVariables : IPlanComputationQuantityVariables
	{
		//=======
		// FIELDS
		//=======

		private ProfileList _profileList;
		private int _seq;
		private int _futureSeq;

		public QuantityVariableProfile Value;
		public QuantityVariableProfile StoreAverage;
		public QuantityVariableProfile PctChange;
		public QuantityVariableProfile PctChangeToPlan;
		public QuantityVariableProfile PctToSet;
		public QuantityVariableProfile PctToAllStore;
		public QuantityVariableProfile PctToTimePeriod;
		public QuantityVariableProfile Comp;
		public QuantityVariableProfile NonComp;
		public QuantityVariableProfile New;
		public QuantityVariableProfile LowLevelAverage;
		public QuantityVariableProfile PctToLowLevelTotal;
		public QuantityVariableProfile Balance;
		public QuantityVariableProfile Difference;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of QuantityVariables.
		/// </summary>

		public BasePlanQuantityVariables() 
		{
			try
			{
				_profileList = new ProfileList(eProfileType.QuantityVariable);
				_seq = 1;
				_futureSeq = 10000;

				Value = new QuantityVariableProfile(NextSequence, "Value", eVariableCategory.Both, eVariableStyle.Overridden, eVariableAccess.Overridden, eVariableScope.Overridden, eVariableWeekType.Overridden, QuantityLevelFlagValues.All, QuantityLevelFlagValues.All, false, eValueFormatType.None, 0);
				StoreAverage = new QuantityVariableProfile(NextSequence, "Store Average", eVariableCategory.Store, eVariableStyle.Overridden, eVariableAccess.Overridden, eVariableScope.Overridden, eVariableWeekType.Overridden, (ushort)(QuantityLevelFlagValues.HighLevel | QuantityLevelFlagValues.LowLevel | QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.StoreTotalCube), (ushort)(QuantityLevelFlagValues.StoreSingleView | QuantityLevelFlagValues.StoreMultiView), true, eValueFormatType.GenericNumeric, 2);
				PctChange = new QuantityVariableProfile(NextSequence, "% Change", eVariableCategory.Both, eVariableStyle.Percentage, eVariableAccess.Overridden, eVariableScope.Dynamic, eVariableWeekType.Overridden, QuantityLevelFlagValues.All, QuantityLevelFlagValues.All, true, eValueFormatType.GenericNumeric, 2);
				PctToSet = new QuantityVariableProfile(NextSequence, "% Set", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.Overridden, eVariableScope.Dynamic, eVariableWeekType.Overridden, (ushort)(QuantityLevelFlagValues.AllLevels | QuantityLevelFlagValues.StoreDetailCube), (ushort)(QuantityLevelFlagValues.StoreSingleView | QuantityLevelFlagValues.StoreMultiView), true, eValueFormatType.GenericNumeric, 2);
				PctToAllStore = new QuantityVariableProfile(NextSequence, "% All Store", eVariableCategory.Store, eVariableStyle.Percentage, eVariableAccess.Overridden, eVariableScope.Dynamic, eVariableWeekType.Overridden, (ushort)(QuantityLevelFlagValues.AllLevels | QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.StoreSetCube), (ushort)(QuantityLevelFlagValues.StoreSingleView | QuantityLevelFlagValues.StoreMultiView), true, eValueFormatType.GenericNumeric, 2);
				PctToTimePeriod = new QuantityVariableProfile(NextSequence, "% Time Period", eVariableCategory.Both, eVariableStyle.Percentage, eVariableAccess.Overridden, eVariableScope.Dynamic, eVariableWeekType.Overridden, QuantityLevelFlagValues.All, QuantityLevelFlagValues.All, true, eValueFormatType.GenericNumeric, 2);
				Comp = new QuantityVariableProfile(NextSequence, "Comp", eVariableCategory.Store, eVariableStyle.Overridden, eVariableAccess.Overridden, eVariableScope.Overridden, eVariableWeekType.Overridden, (ushort)(QuantityLevelFlagValues.HighLevel | QuantityLevelFlagValues.LowLevel | QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.StoreTotalCube), (ushort)(QuantityLevelFlagValues.StoreSingleView | QuantityLevelFlagValues.StoreMultiView), true, eValueFormatType.None, 0);
				NonComp = new QuantityVariableProfile(NextSequence, "Non-comp", eVariableCategory.Store, eVariableStyle.Overridden, eVariableAccess.Overridden, eVariableScope.Overridden, eVariableWeekType.Overridden, (ushort)(QuantityLevelFlagValues.HighLevel | QuantityLevelFlagValues.LowLevel | QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.StoreTotalCube), (ushort)(QuantityLevelFlagValues.StoreSingleView | QuantityLevelFlagValues.StoreMultiView), true, eValueFormatType.None, 0);
				New = new QuantityVariableProfile(NextSequence, "New", eVariableCategory.Store, eVariableStyle.Overridden, eVariableAccess.Overridden, eVariableScope.Overridden, eVariableWeekType.Overridden, (ushort)(QuantityLevelFlagValues.HighLevel | QuantityLevelFlagValues.LowLevel | QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.StoreTotalCube), (ushort)(QuantityLevelFlagValues.StoreSingleView | QuantityLevelFlagValues.StoreMultiView), true, eValueFormatType.None, 0);
				LowLevelAverage = new QuantityVariableProfile(NextSequence, "Low-Level Average", eVariableCategory.Both, eVariableStyle.Overridden, eVariableAccess.Overridden, eVariableScope.Overridden, eVariableWeekType.Overridden, (ushort)(QuantityLevelFlagValues.LowLevelTotal | QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.ChainDetailCube), (ushort)(QuantityLevelFlagValues.ChainMultiView | QuantityLevelFlagValues.StoreMultiView), true, eValueFormatType.GenericNumeric, 2);
				PctToLowLevelTotal = new QuantityVariableProfile(NextSequence, "% Low-Level Total", eVariableCategory.Both, eVariableStyle.Percentage, eVariableAccess.Overridden, eVariableScope.Dynamic, eVariableWeekType.Overridden, (ushort)(QuantityLevelFlagValues.LowLevel | QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.ChainDetailCube), (ushort)(QuantityLevelFlagValues.ChainMultiView | QuantityLevelFlagValues.StoreMultiView), true, eValueFormatType.GenericNumeric, 2);
				Balance = new QuantityVariableProfile(NextSequence, "Balance", eVariableCategory.Both, eVariableStyle.Overridden, eVariableAccess.Overridden, eVariableScope.Dynamic, eVariableWeekType.Overridden, (ushort)(QuantityLevelFlagValues.LowLevelTotal | QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.StoreTotalCube | QuantityLevelFlagValues.ChainDetailCube), (ushort)(QuantityLevelFlagValues.ChainMultiView | QuantityLevelFlagValues.StoreMultiView), false, eValueFormatType.None, 0);
				Difference = new QuantityVariableProfile(NextSequence, "Difference", eVariableCategory.Both, eVariableStyle.Overridden, eVariableAccess.Overridden, eVariableScope.Dynamic, eVariableWeekType.Overridden, (ushort)(QuantityLevelFlagValues.AllLevels | QuantityLevelFlagValues.ChainDetailCube), QuantityLevelFlagValues.StoreMultiView, false, eValueFormatType.None, 0);
				//NOTE -- NOTE -- NOTE
				//NOTE -- NOTE -- NOTE
				//When adding any new base variables, DO NOT USE the NextSequence property for the sequence #.  Use NextFutureSequence.  This is to prevent the renumbering
				//of Client-specific variables that would invalidate Filters and Forcasting Models.
				//NOTE -- NOTE -- NOTE
				//NOTE -- NOTE -- NOTE
				PctChangeToPlan = new QuantityVariableProfile(NextSequence, "% Change to Plan", eVariableCategory.Both, eVariableStyle.Percentage, eVariableAccess.Overridden, eVariableScope.Dynamic, eVariableWeekType.Overridden, QuantityLevelFlagValues.All, QuantityLevelFlagValues.All, true, eValueFormatType.GenericNumeric, 2);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the next available variable sequence number.
		/// </summary>

		protected int NextSequence
		{
			get
			{
				return _seq++;
			}
		}

		/// <summary>
		/// Gets the next available variable sequence number for future variables.
		/// </summary>

		protected int NextFutureSequence
		{
			get
			{
				return _futureSeq++;
			}
		}

		/// <summary>
		/// Gets the Value variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public QuantityVariableProfile ValueQuantity
		{
			get
			{
				return Value;
			}
		}

		/// <summary>
		/// Gets the StoreAverage variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public QuantityVariableProfile StoreAverageQuantity
		{
			get
			{
				return StoreAverage;
			}
		}

		/// <summary>
		/// Gets the PctChange variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public QuantityVariableProfile PctChangeQuantity
		{
			get
			{
				return PctChange;
			}
		}

		/// <summary>
		/// Gets the PctChange variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public QuantityVariableProfile PctChangeToPlanQuantity
		{
			get
			{
				return PctChangeToPlan;
			}
		}

		/// <summary>
		/// Gets the Balance variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public QuantityVariableProfile BalanceQuantity
		{
			get
			{
				return Balance;
			}
		}

		/// <summary>
		/// Gets the Balance variable.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public QuantityVariableProfile DifferenceQuantity
		{
			get
			{
				return Difference;
			}
		}

		/// <summary>
		/// Gets the number of Quantity Variables.
		/// </summary>

		public int NumQuantityVariables
		{
			get
			{
				try
				{
					return _profileList.Count;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the ProfileList of Quantity Variables.
		/// </summary>

		public ProfileList QuantityVariableProfileList
		{
			get
			{
				return _profileList;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Initializes this instance of BasePlanQuantityVariables.
		/// </summary>

		public void Initialize()
		{
			try
			{
				InitializeVariables();
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Initialized the variables for this instance.
		/// </summary>

		virtual protected void InitializeVariables()
		{
			try
			{
				_profileList.Add(Value);
				_profileList.Add(StoreAverage);
				_profileList.Add(PctChange);
				_profileList.Add(PctChangeToPlan);
				_profileList.Add(PctToSet);
				_profileList.Add(PctToAllStore);
				_profileList.Add(PctToTimePeriod);
				_profileList.Add(Comp);
				_profileList.Add(NonComp);
				_profileList.Add(New);
				_profileList.Add(LowLevelAverage);
				_profileList.Add(PctToLowLevelTotal);
				_profileList.Add(Balance);
				_profileList.Add(Difference);
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a count of QuantityVariables that are valid for the given level flag values.
		/// </summary>
		/// <param name="aCubeFlags">
		/// The level flag values to check.  If more than one set of values is given, they will be combined with "or".
		/// </param>
		/// <returns>
		/// A count of valid QuantityVariables.
		/// </returns>

		public int GetQuantityVariableCountByCube(params ushort[] aCubeFlags)
		{
			int count;

			try
			{
				count = 0;

				foreach (QuantityVariableProfile quanVarProf in _profileList)
				{
					foreach (ushort flags in aCubeFlags)
					{
						if (quanVarProf.IsValidQuantityCube(flags))
						{
							count++;
							break;
						}
					}
				}

				return count;
			}
			catch (Exception exc)
			{
				throw;
			}
		}
	}
}
