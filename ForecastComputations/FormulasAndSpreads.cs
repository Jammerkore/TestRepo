using System;
using System.Collections;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;

namespace MIDRetail.ForecastComputations
{
	/// <summary>
	/// The FormulasAndSpreads class is where the Formula and Spread routines are defined.
	/// </summary>
	/// <remarks>
	/// This class is where the Formula and Spread routines are defined.  A formula or spread is defined in a FormulaSpreadProfile, which contains
	/// an Id, a name, and a FormulaSpreadDelegate that points to a method within this class that executes the formula or spread.  This method will contain
	/// all the logic to calculate or spread values as required.
	/// </remarks>

	abstract public class BasePlanFormulasAndSpreads
	{
		//=======
		// FIELDS
		//=======

		private BasePlanComputations _basePlanComputations;
		private  int _seq;

		//-------------------------------------------
		#region Spreads
		//-------------------------------------------

		protected SpreadProfile _spread_Set;
		protected SpreadProfile _spread_SetAverage;
		protected SpreadProfile _spread_SetComp;
		protected SpreadProfile _spread_SetNonComp;
		protected SpreadProfile _spread_SetNew;
		protected SpreadProfile _spread_All;
		protected SpreadProfile _spread_AllAverage;
		protected SpreadProfile _spread_AllComp;
		protected SpreadProfile _spread_AllNonComp;
		protected SpreadProfile _spread_AllNew;
		protected SpreadProfile _spread_Period;
		protected SpreadProfile _spread_PeriodAverage;
		protected SpreadProfile _spread_Date;
		protected SpreadProfile _spread_DateAverage;
		protected SpreadProfile _spread_LowLevel;
		protected SpreadProfile _spread_LowLevelAverage;
		protected SpreadProfile _spread_PeriodNoCascade;
		protected SpreadProfile _spread_TotalPeriodToWeeks;
		protected SpreadProfile _spread_SalesTotalUnitsAverage;
		protected SpreadProfile _spread_SalesRegularUnitsAverage;
		protected SpreadProfile _spread_SalesPromoUnitsAverage;
		protected SpreadProfile _spread_SalesRegPromoUnitsAverage;
		protected SpreadProfile _spread_InventoryTotalUnitsAverage;
		protected SpreadProfile _spread_InventoryRegularUnitsAverage;
		protected SpreadProfile _spread_SalesTotalUnitsAverage_LowLevelTotal;
		protected SpreadProfile _spread_SalesRegularUnitsAverage_LowLevelTotal;
		protected SpreadProfile _spread_SalesPromoUnitsAverage_LowLevelTotal;
		protected SpreadProfile _spread_SalesRegPromoUnitsAverage_LowLevelTotal;
		protected SpreadProfile _spread_InventoryTotalUnitsAverage_LowLevelTotal;
		protected SpreadProfile _spread_InventoryRegularUnitsAverage_LowLevelTotal;
        protected SpreadProfile _spread_SalesRegPromoUnits;     // TT#1722 - RMatelic - Open Sales R/P to be editable 

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Inits
		//-------------------------------------------

		protected FormulaProfile _init_Null;
		protected FormulaProfile _init_SalesTotalUnits;
		protected FormulaProfile _init_SalesRegularUnits;
		protected FormulaProfile _init_SalesPromoUnits;
		protected FormulaProfile _init_SalesRegPromoUnits;
		protected FormulaProfile _init_SalesMarkdownUnits;
		protected FormulaProfile _init_SalesTotalSetIndex;
		protected FormulaProfile _init_SalesRegularSetIndex;
		protected FormulaProfile _init_SalesPromoSetIndex;
		protected FormulaProfile _init_SalesRegPromoSetIndex;
		protected FormulaProfile _init_SalesMarkdownSetIndex;
		protected FormulaProfile _init_SalesTotalAllIndex;
		protected FormulaProfile _init_SalesRegularAllIndex;
		protected FormulaProfile _init_SalesPromoAllIndex;
		protected FormulaProfile _init_SalesRegPromoAllIndex;
		protected FormulaProfile _init_SalesMarkdownAllIndex;
		protected FormulaProfile _init_InventoryTotalUnits;
		protected FormulaProfile _init_InventoryRegularUnits;
		protected FormulaProfile _init_InventoryMarkdownUnits;
		protected FormulaProfile _init_InventoryTotalSetIndex;
		protected FormulaProfile _init_InventoryRegularSetIndex;
		protected FormulaProfile _init_InventoryMarkdownSetIndex;
		protected FormulaProfile _init_InventoryTotalAllIndex;
		protected FormulaProfile _init_InventoryRegularAllIndex;
		protected FormulaProfile _init_InventoryMarkdownAllIndex;
		protected FormulaProfile _init_ReceiptTotalUnits;
		protected FormulaProfile _init_ReceiptRegularUnits;
		protected FormulaProfile _init_ReceiptMarkdownUnits;
		protected FormulaProfile _init_SalesStockRatioTotal;
		protected FormulaProfile _init_SalesStockRatioRegPromo;
		protected FormulaProfile _init_SalesStockRatioMarkdown;
		protected FormulaProfile _init_ForwardWOSTotal;
		protected FormulaProfile _init_ForwardWOSRegPromo;
		protected FormulaProfile _init_ForwardWOSMarkdown;
		protected FormulaProfile _init_ForwardWOSTotalSetIndex;
		protected FormulaProfile _init_ForwardWOSRegPromoSetIndex;
		protected FormulaProfile _init_ForwardWOSMarkdownSetIndex;
		protected FormulaProfile _init_ForwardWOSTotalAllIndex;
		protected FormulaProfile _init_ForwardWOSRegPromoAllIndex;
		protected FormulaProfile _init_ForwardWOSMarkdownAllIndex;
		protected FormulaProfile _init_SellThruPctTotal;
		protected FormulaProfile _init_SellThruPctRegPromo;
		protected FormulaProfile _init_SellThruPctMarkdown;
		protected FormulaProfile _init_SellThruPctTotalSetIndex;
		protected FormulaProfile _init_SellThruPctRegPromoSetIndex;
		protected FormulaProfile _init_SellThruPctMarkdownSetIndex;
		protected FormulaProfile _init_SellThruPctTotalAllIndex;
		protected FormulaProfile _init_SellThruPctRegPromoAllIndex;
		protected FormulaProfile _init_SellThruPctMarkdownAllIndex;
		protected FormulaProfile _init_Intransit;
		protected FormulaProfile _init_GradeTotal;
		protected FormulaProfile _init_GradeRegPromo;
		protected FormulaProfile _init_StoreStatus;

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Generic Total Inits
		//-------------------------------------------

		protected FormulaProfile _init_SumDetail;
		protected FormulaProfile _init_AvgDetail;
		protected FormulaProfile _init_AvgDetail_LowLevelTotal;
		protected FormulaProfile _init_PeriodSumDetail;
		//Begin TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
		protected FormulaProfile _init_PeriodAvgDetail;
		//End TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Time Total Inits
		//-------------------------------------------

		protected FormulaProfile _init_SalesUnitsTimeAvg;
		protected FormulaProfile _init_InventoryUnitsTimeAvg;
		protected FormulaProfile _init_InventoryUnitsEnding;
		protected FormulaProfile _init_SalesTotalUnitsT3;
		protected FormulaProfile _init_SalesRegularUnitsT2;
		protected FormulaProfile _init_SalesPromoUnitsT2;
		protected FormulaProfile _init_SalesRegPromoUnitsT3;
		protected FormulaProfile _init_SalesMarkdownUnitsT3;
		protected FormulaProfile _init_SalesTotalSetIndexT1;
		protected FormulaProfile _init_SalesRegularSetIndexT1;
		protected FormulaProfile _init_SalesPromoSetIndexT1;
		protected FormulaProfile _init_SalesRegPromoSetIndexT1;
		protected FormulaProfile _init_SalesMarkdownSetIndexT1;
		protected FormulaProfile _init_SalesTotalAllIndexT1;
		protected FormulaProfile _init_SalesRegularAllIndexT1;
		protected FormulaProfile _init_SalesPromoAllIndexT1;
		protected FormulaProfile _init_SalesRegPromoAllIndexT1;
		protected FormulaProfile _init_SalesMarkdownAllIndexT1;
		protected FormulaProfile _init_InventoryTotalUnitsT4;
		protected FormulaProfile _init_InventoryTotalUnitsT5;
		protected FormulaProfile _init_InventoryTotalUnitsT6;
		protected FormulaProfile _init_InventoryTotalUnitsT7;
        protected FormulaProfile _init_InventoryTotalUnitsT8;		// TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
		protected FormulaProfile _init_InventoryRegularUnitsT4;
		protected FormulaProfile _init_InventoryRegularUnitsT5;
		protected FormulaProfile _init_InventoryRegularUnitsT6;
		protected FormulaProfile _init_InventoryRegularUnitsT7;
        protected FormulaProfile _init_InventoryRegularUnitsT8;		// TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
		protected FormulaProfile _init_InventoryTotalSetIndexT1;
		protected FormulaProfile _init_InventoryTotalSetIndexT2;
		protected FormulaProfile _init_InventoryTotalSetIndexT3;
		protected FormulaProfile _init_InventoryRegularSetIndexT1;
		protected FormulaProfile _init_InventoryRegularSetIndexT2;
		protected FormulaProfile _init_InventoryRegularSetIndexT3;
		protected FormulaProfile _init_InventoryMarkdownSetIndexT1;
		protected FormulaProfile _init_InventoryMarkdownSetIndexT2;
		protected FormulaProfile _init_InventoryMarkdownSetIndexT3;
		protected FormulaProfile _init_InventoryTotalAllIndexT1;
		protected FormulaProfile _init_InventoryTotalAllIndexT2;
		protected FormulaProfile _init_InventoryTotalAllIndexT3;
		protected FormulaProfile _init_InventoryRegularAllIndexT1;
		protected FormulaProfile _init_InventoryRegularAllIndexT2;
		protected FormulaProfile _init_InventoryRegularAllIndexT3;
		protected FormulaProfile _init_InventoryMarkdownAllIndexT1;
		protected FormulaProfile _init_InventoryMarkdownAllIndexT2;
		protected FormulaProfile _init_InventoryMarkdownAllIndexT3;
		protected FormulaProfile _init_ForwardWOSTotalSetIndexT1;
		protected FormulaProfile _init_ForwardWOSRegPromoSetIndexT1;
		protected FormulaProfile _init_ForwardWOSMarkdownSetIndexT1;
		protected FormulaProfile _init_ForwardWOSTotalAllIndexT1;
		protected FormulaProfile _init_ForwardWOSRegPromoAllIndexT1;
		protected FormulaProfile _init_ForwardWOSMarkdownAllIndexT1;
		protected FormulaProfile _init_SalesStockRatioTotalT1;
		protected FormulaProfile _init_SalesStockRatioRegPromoT1;
		protected FormulaProfile _init_SalesStockRatioMarkdownT1;
		protected FormulaProfile _init_SellThruPctTotalT1;
		protected FormulaProfile _init_SellThruPctRegPromoT1;
		protected FormulaProfile _init_SellThruPctMarkdownT1;
		protected FormulaProfile _init_SellThruPctTotalSetIndexT1;
		protected FormulaProfile _init_SellThruPctRegPromoSetIndexT1;
		protected FormulaProfile _init_SellThruPctMarkdownSetIndexT1;
		protected FormulaProfile _init_SellThruPctTotalAllIndexT1;
		protected FormulaProfile _init_SellThruPctRegPromoAllIndexT1;
		protected FormulaProfile _init_SellThruPctMarkdownAllIndexT1;
		protected FormulaProfile _init_GradeTotalT1;
		protected FormulaProfile _init_GradeRegPromoT1;

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Set/Store Inits
		//-------------------------------------------

		protected FormulaProfile _init_SetSalesTotalAllIndex;
		protected FormulaProfile _init_SetSalesRegularAllIndex;
		protected FormulaProfile _init_SetSalesPromoAllIndex;
		protected FormulaProfile _init_SetSalesRegPromoAllIndex;
		protected FormulaProfile _init_SetSalesMarkdownAllIndex;
		protected FormulaProfile _init_SetInventoryTotalAllIndex;
		protected FormulaProfile _init_SetInventoryRegularAllIndex;
		protected FormulaProfile _init_SetInventoryMarkdownAllIndex;
		protected FormulaProfile _init_SetSalesTotalAllIndexT1;
		protected FormulaProfile _init_SetSalesRegularAllIndexT1;
		protected FormulaProfile _init_SetSalesPromoAllIndexT1;
		protected FormulaProfile _init_SetSalesRegPromoAllIndexT1;
		protected FormulaProfile _init_SetSalesMarkdownAllIndexT1;
		protected FormulaProfile _init_SetInventoryTotalAllIndexT1;
		protected FormulaProfile _init_SetInventoryTotalAllIndexT2;
		protected FormulaProfile _init_SetInventoryTotalAllIndexT3;
		protected FormulaProfile _init_SetInventoryRegularAllIndexT1;
		protected FormulaProfile _init_SetInventoryRegularAllIndexT2;
		protected FormulaProfile _init_SetInventoryRegularAllIndexT3;
		protected FormulaProfile _init_SetInventoryMarkdownAllIndexT1;
		protected FormulaProfile _init_SetInventoryMarkdownAllIndexT2;
		protected FormulaProfile _init_SetInventoryMarkdownAllIndexT3;

		protected FormulaProfile _init_SumCompStore;
		protected FormulaProfile _init_SumNonCompStore;
		protected FormulaProfile _init_SumNewStore;

		protected FormulaProfile _init_SumStoreSalesTotalUnitsT3;
		protected FormulaProfile _init_SumCompStoreSalesTotalUnitsT3;
		protected FormulaProfile _init_SumNonCompStoreSalesTotalUnitsT3;
		protected FormulaProfile _init_SumNewStoreSalesTotalUnitsT3;
		protected FormulaProfile _init_SumStoreSalesRegularUnitsT2;
		protected FormulaProfile _init_SumCompStoreSalesRegularUnitsT2;
		protected FormulaProfile _init_SumNonCompStoreSalesRegularUnitsT2;
		protected FormulaProfile _init_SumNewStoreSalesRegularUnitsT2;
		protected FormulaProfile _init_SumStoreSalesPromoUnitsT2;
		protected FormulaProfile _init_SumCompStoreSalesPromoUnitsT2;
		protected FormulaProfile _init_SumNonCompStoreSalesPromoUnitsT2;
		protected FormulaProfile _init_SumNewStoreSalesPromoUnitsT2;
		protected FormulaProfile _init_SumStoreSalesRegPromoUnitsT3;
		protected FormulaProfile _init_SumCompStoreSalesRegPromoUnitsT3;
		protected FormulaProfile _init_SumNonCompStoreSalesRegPromoUnitsT3;
		protected FormulaProfile _init_SumNewStoreSalesRegPromoUnitsT3;
		protected FormulaProfile _init_SumStoreSalesMarkdownUnitsT3;
		protected FormulaProfile _init_SumCompStoreSalesMarkdownUnitsT3;
		protected FormulaProfile _init_SumNonCompStoreSalesMarkdownUnitsT3;
		protected FormulaProfile _init_SumNewStoreSalesMarkdownUnitsT3;

		protected FormulaProfile _init_SumStoreInventoryTotalUnitsT4;
		protected FormulaProfile _init_SumCompStoreInventoryTotalUnitsT4;
		protected FormulaProfile _init_SumNonCompStoreInventoryTotalUnitsT4;
		protected FormulaProfile _init_SumNewStoreInventoryTotalUnitsT4;
		protected FormulaProfile _init_SumStoreInventoryTotalUnitsT5;
		protected FormulaProfile _init_SumCompStoreInventoryTotalUnitsT5;
		protected FormulaProfile _init_SumNonCompStoreInventoryTotalUnitsT5;
		protected FormulaProfile _init_SumNewStoreInventoryTotalUnitsT5;
		protected FormulaProfile _init_SumStoreInventoryTotalUnitsT6;
		protected FormulaProfile _init_SumCompStoreInventoryTotalUnitsT6;
		protected FormulaProfile _init_SumNonCompStoreInventoryTotalUnitsT6;
		protected FormulaProfile _init_SumNewStoreInventoryTotalUnitsT6;
		protected FormulaProfile _init_SumStoreInventoryTotalUnitsT7;
		protected FormulaProfile _init_SumCompStoreInventoryTotalUnitsT7;
		protected FormulaProfile _init_SumNonCompStoreInventoryTotalUnitsT7;
		protected FormulaProfile _init_SumNewStoreInventoryTotalUnitsT7;
		// Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
        protected FormulaProfile _init_SumStoreInventoryTotalUnitsT8;
        protected FormulaProfile _init_SumCompStoreInventoryTotalUnitsT8;
        protected FormulaProfile _init_SumNonCompStoreInventoryTotalUnitsT8;
        protected FormulaProfile _init_SumNewStoreInventoryTotalUnitsT8;
		// End TT#2054
		protected FormulaProfile _init_SumStoreInventoryRegularUnitsT4;
		protected FormulaProfile _init_SumCompStoreInventoryRegularUnitsT4;
		protected FormulaProfile _init_SumNonCompStoreInventoryRegularUnitsT4;
		protected FormulaProfile _init_SumNewStoreInventoryRegularUnitsT4;
		protected FormulaProfile _init_SumStoreInventoryRegularUnitsT5;
		protected FormulaProfile _init_SumCompStoreInventoryRegularUnitsT5;
		protected FormulaProfile _init_SumNonCompStoreInventoryRegularUnitsT5;
		protected FormulaProfile _init_SumNewStoreInventoryRegularUnitsT5;
		protected FormulaProfile _init_SumStoreInventoryRegularUnitsT6;
		protected FormulaProfile _init_SumCompStoreInventoryRegularUnitsT6;
		protected FormulaProfile _init_SumNonCompStoreInventoryRegularUnitsT6;
		protected FormulaProfile _init_SumNewStoreInventoryRegularUnitsT6;
		protected FormulaProfile _init_SumStoreInventoryRegularUnitsT7;
		protected FormulaProfile _init_SumCompStoreInventoryRegularUnitsT7;
		protected FormulaProfile _init_SumNonCompStoreInventoryRegularUnitsT7;
		protected FormulaProfile _init_SumNewStoreInventoryRegularUnitsT7;
		// Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
        protected FormulaProfile _init_SumStoreInventoryRegularUnitsT8;
        protected FormulaProfile _init_SumCompStoreInventoryRegularUnitsT8;
        protected FormulaProfile _init_SumNonCompStoreInventoryRegularUnitsT8;
        protected FormulaProfile _init_SumNewStoreInventoryRegularUnitsT8;
		// TT#2054
		protected FormulaProfile _init_AvgStoreSalesTotalUnits;
		protected FormulaProfile _init_AvgStoreSalesRegularUnits;
		protected FormulaProfile _init_AvgStoreSalesMarkdownUnits;
		protected FormulaProfile _init_AvgStoreInventoryTotalUnits;
		protected FormulaProfile _init_AvgStoreInventoryRegularUnits;
		protected FormulaProfile _init_AvgStoreInventoryMarkdownUnits;

		protected FormulaProfile _init_AvgStoreSalesTotalUnits_LowLevelTotal;
		protected FormulaProfile _init_AvgStoreSalesRegularUnits_LowLevelTotal;
		protected FormulaProfile _init_AvgStoreSalesMarkdownUnits_LowLevelTotal;
		protected FormulaProfile _init_AvgStoreInventoryTotalUnits_LowLevelTotal;
		protected FormulaProfile _init_AvgStoreInventoryRegularUnits_LowLevelTotal;
		protected FormulaProfile _init_AvgStoreInventoryMarkdownUnits_LowLevelTotal;

		protected FormulaProfile _init_AvgStoreSalesTotalUnitsT3;
		protected FormulaProfile _init_AvgStoreSalesRegularUnitsT2;
		protected FormulaProfile _init_AvgStoreSalesPromoUnitsT2;
		protected FormulaProfile _init_AvgStoreSalesRegPromoUnitsT3;
		protected FormulaProfile _init_AvgStoreSalesMarkdownUnitsT3;

		protected FormulaProfile _init_AvgStoreInventoryTotalUnitsT4;
		protected FormulaProfile _init_AvgStoreInventoryTotalUnitsT5;
		protected FormulaProfile _init_AvgStoreInventoryTotalUnitsT6;
        protected FormulaProfile _init_AvgStoreInventoryTotalUnitsT8;		// TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
		protected FormulaProfile _init_AvgStoreInventoryRegularUnitsT4;
		protected FormulaProfile _init_AvgStoreInventoryRegularUnitsT5;
		protected FormulaProfile _init_AvgStoreInventoryRegularUnitsT6;
        protected FormulaProfile _init_AvgStoreInventoryRegularUnitsT8;		// TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Period Inits
		//-------------------------------------------

		protected FormulaProfile _init_SumWeekDetail;

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Low-level Inits
		//-------------------------------------------

		protected FormulaProfile _init_SumLowLevelTotalDetail;
		protected FormulaProfile _init_SumLowLevelRegularDetail;

		protected FormulaProfile _init_SumLowLevelInventoryTotalUnitsT7;
		protected FormulaProfile _init_SumLowLevelInventoryRegularUnitsT7;
		protected FormulaProfile _init_SumLowLevelStoreInventoryTotalUnitsT7;
		protected FormulaProfile _init_SumLowLevelStoreInventoryRegularUnitsT7;

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Comparative Inits
		//-------------------------------------------

		protected FormulaProfile _init_PctChange;
		protected FormulaProfile _init_PctToSet;
		protected FormulaProfile _init_PctToAll;
		protected FormulaProfile _init_PctToLowLevel;
		protected FormulaProfile _init_PctToTimePeriod;
		protected FormulaProfile _init_ChainBalance;
		protected FormulaProfile _init_StoreBalance;
		protected FormulaProfile _init_Difference;
		protected FormulaProfile _init_PctChangeToPlan;

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Change Formulas
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Time Total Change Formulas
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Static.  Creates a new instance of FormulasAndSpreads.
		/// </summary>

		public BasePlanFormulasAndSpreads(BasePlanComputations aBasePlanComputations)
		{
			_basePlanComputations = aBasePlanComputations;
			_seq = 1;

			//-------------------------------------------
			#region Spreads
			//-------------------------------------------

			_spread_Set = new clsSpread_Set(aBasePlanComputations, NextSequence);
			_spread_SetAverage = new clsSpread_SetAverage(aBasePlanComputations, NextSequence);
			_spread_SetComp = new clsSpread_SetComp(aBasePlanComputations, NextSequence);
			_spread_SetNonComp = new clsSpread_SetNonComp(aBasePlanComputations, NextSequence);
			_spread_SetNew = new clsSpread_SetNew(aBasePlanComputations, NextSequence);
			_spread_All = new clsSpread_All(aBasePlanComputations, NextSequence);
			_spread_AllAverage = new clsSpread_AllAverage(aBasePlanComputations, NextSequence);
			_spread_AllComp = new clsSpread_AllComp(aBasePlanComputations, NextSequence);
			_spread_AllNonComp = new clsSpread_AllNonComp(aBasePlanComputations, NextSequence);
			_spread_AllNew = new clsSpread_AllNew(aBasePlanComputations, NextSequence);
			_spread_Period = new clsSpread_Period(aBasePlanComputations, NextSequence);
			_spread_PeriodAverage = new clsSpread_PeriodAverage(aBasePlanComputations, NextSequence);
			_spread_Date = new clsSpread_Date(aBasePlanComputations, NextSequence);
			_spread_DateAverage = new clsSpread_DateAverage(aBasePlanComputations, NextSequence);
			_spread_LowLevel = new clsSpread_LowLevel(aBasePlanComputations, NextSequence);
			_spread_LowLevelAverage = new clsSpread_LowLevelAverage(aBasePlanComputations, NextSequence);
			_spread_PeriodNoCascade = new clsSpread_PeriodNoCascade(aBasePlanComputations, NextSequence);
			_spread_TotalPeriodToWeeks = new clsSpread_TotalPeriodToWeeks(aBasePlanComputations, NextSequence);
			_spread_SalesTotalUnitsAverage = new clsSpread_SalesTotalUnitsAverage(aBasePlanComputations, NextSequence);
			_spread_SalesRegularUnitsAverage = new clsSpread_SalesRegularUnitsAverage(aBasePlanComputations, NextSequence);
			_spread_SalesPromoUnitsAverage = new clsSpread_SalesPromoUnitsAverage(aBasePlanComputations, NextSequence);
			_spread_SalesRegPromoUnitsAverage = new clsSpread_SalesRegPromoUnitsAverage(aBasePlanComputations, NextSequence);
			_spread_InventoryTotalUnitsAverage = new clsSpread_InventoryTotalUnitsAverage(aBasePlanComputations, NextSequence);
			_spread_InventoryRegularUnitsAverage = new clsSpread_InventoryRegularUnitsAverage(aBasePlanComputations, NextSequence);
			_spread_SalesTotalUnitsAverage_LowLevelTotal = new clsSpread_SalesTotalUnitsAverage_LowLevelTotal(aBasePlanComputations, NextSequence);
			_spread_SalesRegularUnitsAverage_LowLevelTotal = new clsSpread_SalesRegularUnitsAverage_LowLevelTotal(aBasePlanComputations, NextSequence);
			_spread_SalesPromoUnitsAverage_LowLevelTotal = new clsSpread_SalesPromoUnitsAverage_LowLevelTotal(aBasePlanComputations, NextSequence);
			_spread_SalesRegPromoUnitsAverage_LowLevelTotal = new clsSpread_SalesRegPromoUnitsAverage_LowLevelTotal(aBasePlanComputations, NextSequence);
			_spread_InventoryTotalUnitsAverage_LowLevelTotal = new clsSpread_InventoryTotalUnitsAverage_LowLevelTotal(aBasePlanComputations, NextSequence);
			_spread_InventoryRegularUnitsAverage_LowLevelTotal = new clsSpread_InventoryRegularUnitsAverage_LowLevelTotal(aBasePlanComputations, NextSequence);
            _spread_SalesRegPromoUnits = new clsSpread_SalesRegPromoUnits(aBasePlanComputations, NextSequence);  // TT#1722 - RMatelic - Open Sales R/P to be editable 

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Variable Inits
			//-------------------------------------------

			_init_Null = new clsInit_Null(aBasePlanComputations, NextSequence);
			_init_SalesTotalUnits = new clsInit_SalesTotalUnits(aBasePlanComputations, NextSequence);
			_init_SalesRegularUnits = new clsInit_SalesRegularUnits(aBasePlanComputations, NextSequence);
			_init_SalesPromoUnits = new clsInit_SalesPromoUnits(aBasePlanComputations, NextSequence);
			_init_SalesRegPromoUnits = new clsInit_SalesRegPromoUnits(aBasePlanComputations, NextSequence);
			_init_SalesMarkdownUnits = new clsInit_SalesMarkdownUnits(aBasePlanComputations, NextSequence);
			_init_SalesTotalSetIndex = new clsInit_SalesTotalSetIndex(aBasePlanComputations, NextSequence);
			_init_SalesRegularSetIndex = new clsInit_SalesRegularSetIndex(aBasePlanComputations, NextSequence);
			_init_SalesPromoSetIndex = new clsInit_SalesPromoSetIndex(aBasePlanComputations, NextSequence);
			_init_SalesRegPromoSetIndex = new clsInit_SalesRegPromoSetIndex(aBasePlanComputations, NextSequence);
			_init_SalesMarkdownSetIndex = new clsInit_SalesMarkdownSetIndex(aBasePlanComputations, NextSequence);
			_init_SalesTotalAllIndex = new clsInit_SalesTotalAllIndex(aBasePlanComputations, NextSequence);
			_init_SalesRegularAllIndex = new clsInit_SalesRegularAllIndex(aBasePlanComputations, NextSequence);
			_init_SalesPromoAllIndex = new clsInit_SalesPromoAllIndex(aBasePlanComputations, NextSequence);
			_init_SalesRegPromoAllIndex = new clsInit_SalesRegPromoAllIndex(aBasePlanComputations, NextSequence);
			_init_SalesMarkdownAllIndex = new clsInit_SalesMarkdownAllIndex(aBasePlanComputations, NextSequence);
			_init_InventoryTotalUnits = new clsInit_InventoryTotalUnits(aBasePlanComputations, NextSequence);
			_init_InventoryRegularUnits = new clsInit_InventoryRegularUnits(aBasePlanComputations, NextSequence);
			_init_InventoryMarkdownUnits = new clsInit_InventoryMarkdownUnits(aBasePlanComputations, NextSequence);
			_init_InventoryTotalSetIndex = new clsInit_InventoryTotalSetIndex(aBasePlanComputations, NextSequence);
			_init_InventoryRegularSetIndex = new clsInit_InventoryRegularSetIndex(aBasePlanComputations, NextSequence);
			_init_InventoryMarkdownSetIndex = new clsInit_InventoryMarkdownSetIndex(aBasePlanComputations, NextSequence);
			_init_InventoryTotalAllIndex = new clsInit_InventoryTotalAllIndex(aBasePlanComputations, NextSequence);
			_init_InventoryRegularAllIndex = new clsInit_InventoryRegularAllIndex(aBasePlanComputations, NextSequence);
			_init_InventoryMarkdownAllIndex = new clsInit_InventoryMarkdownAllIndex(aBasePlanComputations, NextSequence);
			_init_ReceiptTotalUnits = new clsInit_ReceiptTotalUnits(aBasePlanComputations, NextSequence);
			_init_ReceiptRegularUnits = new clsInit_ReceiptRegularUnits(aBasePlanComputations, NextSequence);
			_init_ReceiptMarkdownUnits = new clsInit_ReceiptMarkdownUnits(aBasePlanComputations, NextSequence);
			_init_SalesStockRatioTotal = new clsInit_SalesStockRatioTotal(aBasePlanComputations, NextSequence);
			_init_SalesStockRatioRegPromo = new clsInit_SalesStockRatioRegPromo(aBasePlanComputations, NextSequence);
			_init_SalesStockRatioMarkdown = new clsInit_SalesStockRatioMarkdown(aBasePlanComputations, NextSequence);
			_init_ForwardWOSTotal = new clsInit_ForwardWOSTotal(aBasePlanComputations, NextSequence);
			_init_ForwardWOSRegPromo = new clsInit_ForwardWOSRegPromo(aBasePlanComputations, NextSequence);
			_init_ForwardWOSMarkdown = new clsInit_ForwardWOSMarkdown(aBasePlanComputations, NextSequence);
			_init_ForwardWOSTotalSetIndex = new clsInit_ForwardWOSTotalSetIndex(aBasePlanComputations, NextSequence);
			_init_ForwardWOSRegPromoSetIndex = new clsInit_ForwardWOSRegPromoSetIndex(aBasePlanComputations, NextSequence);
			_init_ForwardWOSMarkdownSetIndex = new clsInit_ForwardWOSMarkdownSetIndex(aBasePlanComputations, NextSequence);
			_init_ForwardWOSTotalAllIndex = new clsInit_ForwardWOSTotalAllIndex(aBasePlanComputations, NextSequence);
			_init_ForwardWOSRegPromoAllIndex = new clsInit_ForwardWOSRegPromoAllIndex(aBasePlanComputations, NextSequence);
			_init_ForwardWOSMarkdownAllIndex = new clsInit_ForwardWOSMarkdownAllIndex(aBasePlanComputations, NextSequence);
			_init_SellThruPctTotal = new clsInit_SellThruPctTotal(aBasePlanComputations, NextSequence);
			_init_SellThruPctRegPromo = new clsInit_SellThruPctRegPromo(aBasePlanComputations, NextSequence);
			_init_SellThruPctMarkdown = new clsInit_SellThruPctMarkdown(aBasePlanComputations, NextSequence);
			_init_SellThruPctTotalSetIndex = new clsInit_SellThruPctTotalSetIndex(aBasePlanComputations, NextSequence);
			_init_SellThruPctRegPromoSetIndex = new clsInit_SellThruPctRegPromoSetIndex(aBasePlanComputations, NextSequence);
			_init_SellThruPctMarkdownSetIndex = new clsInit_SellThruPctMarkdownSetIndex(aBasePlanComputations, NextSequence);
			_init_SellThruPctTotalAllIndex = new clsInit_SellThruPctTotalAllIndex(aBasePlanComputations, NextSequence);
			_init_SellThruPctRegPromoAllIndex = new clsInit_SellThruPctRegPromoAllIndex(aBasePlanComputations, NextSequence);
			_init_SellThruPctMarkdownAllIndex = new clsInit_SellThruPctMarkdownAllIndex(aBasePlanComputations, NextSequence);
			_init_Intransit = new clsInit_Intransit(aBasePlanComputations, NextSequence);
			_init_GradeTotal = new clsInit_GradeTotal(aBasePlanComputations, NextSequence);
			_init_GradeRegPromo = new clsInit_GradeRegPromo(aBasePlanComputations, NextSequence);
			_init_StoreStatus = new clsInit_StoreStatus(aBasePlanComputations, NextSequence);

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Generic Total Inits
			//-------------------------------------------

			_init_SumDetail = new clsInit_SumDetail(aBasePlanComputations, NextSequence);
			_init_AvgDetail = new clsInit_AvgDetail(aBasePlanComputations, NextSequence);
			_init_AvgDetail_LowLevelTotal = new clsInit_AvgDetail_LowLevelTotal(aBasePlanComputations, NextSequence);
			_init_PeriodSumDetail = new clsInit_PeriodSumDetail(aBasePlanComputations, NextSequence);
			//Begin TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
			_init_PeriodAvgDetail = new clsInit_PeriodAvgDetail(aBasePlanComputations, NextSequence);
			//End TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Time Total Inits
			//-------------------------------------------

			_init_SalesUnitsTimeAvg = new clsInit_SalesUnitsTimeAvg(aBasePlanComputations, NextSequence);
			_init_InventoryUnitsTimeAvg = new clsInit_InventoryUnitsTimeAvg(aBasePlanComputations, NextSequence);
			_init_InventoryUnitsEnding = new clsInit_InventoryUnitsEnding(aBasePlanComputations, _seq++);
			_init_SalesTotalUnitsT3 = new clsInit_SalesTotalUnitsT3(aBasePlanComputations, NextSequence);
			_init_SalesRegularUnitsT2 = new clsInit_SalesRegularUnitsT2(aBasePlanComputations, NextSequence);
			_init_SalesPromoUnitsT2 = new clsInit_SalesPromoUnitsT2(aBasePlanComputations, NextSequence);
			_init_SalesRegPromoUnitsT3 = new clsInit_SalesRegPromoUnitsT3(aBasePlanComputations, NextSequence);
			_init_SalesMarkdownUnitsT3 = new clsInit_SalesMarkdownUnitsT3(aBasePlanComputations, NextSequence);
			_init_SalesTotalSetIndexT1 = new clsInit_SalesTotalSetIndexT1(aBasePlanComputations, NextSequence);
			_init_SalesRegularSetIndexT1 = new clsInit_SalesRegularSetIndexT1(aBasePlanComputations, NextSequence);
			_init_SalesPromoSetIndexT1 = new clsInit_SalesPromoSetIndexT1(aBasePlanComputations, NextSequence);
			_init_SalesRegPromoSetIndexT1 = new clsInit_SalesRegPromoSetIndexT1(aBasePlanComputations, NextSequence);
			_init_SalesMarkdownSetIndexT1 = new clsInit_SalesMarkdownSetIndexT1(aBasePlanComputations, NextSequence);
			_init_SalesTotalAllIndexT1 = new clsInit_SalesTotalAllIndexT1(aBasePlanComputations, NextSequence);
			_init_SalesRegularAllIndexT1 = new clsInit_SalesRegularAllIndexT1(aBasePlanComputations, NextSequence);
			_init_SalesPromoAllIndexT1 = new clsInit_SalesPromoAllIndexT1(aBasePlanComputations, NextSequence);
			_init_SalesRegPromoAllIndexT1 = new clsInit_SalesRegPromoAllIndexT1(aBasePlanComputations, NextSequence);
			_init_SalesMarkdownAllIndexT1 = new clsInit_SalesMarkdownAllIndexT1(aBasePlanComputations, NextSequence);
			_init_InventoryTotalUnitsT4 = new clsInit_InventoryTotalUnitsT4(aBasePlanComputations, NextSequence);
			_init_InventoryTotalUnitsT5 = new clsInit_InventoryTotalUnitsT5(aBasePlanComputations, NextSequence);
			_init_InventoryTotalUnitsT6 = new clsInit_InventoryTotalUnitsT6(aBasePlanComputations, NextSequence);
			_init_InventoryTotalUnitsT7 = new clsInit_InventoryTotalUnitsT7(aBasePlanComputations, NextSequence);
            _init_InventoryTotalUnitsT8 = new clsInit_InventoryTotalUnitsT8(aBasePlanComputations, NextSequence);			// TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
			_init_InventoryRegularUnitsT4 = new clsInit_InventoryRegularUnitsT4(aBasePlanComputations, NextSequence);
			_init_InventoryRegularUnitsT5 = new clsInit_InventoryRegularUnitsT5(aBasePlanComputations, NextSequence);
			_init_InventoryRegularUnitsT6 = new clsInit_InventoryRegularUnitsT6(aBasePlanComputations, NextSequence);
			_init_InventoryRegularUnitsT7 = new clsInit_InventoryRegularUnitsT7(aBasePlanComputations, NextSequence);
            _init_InventoryRegularUnitsT8 = new clsInit_InventoryRegularUnitsT8(aBasePlanComputations, NextSequence);		// TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
			_init_InventoryTotalSetIndexT1 = new clsInit_InventoryTotalSetIndexT1(aBasePlanComputations, NextSequence);
			_init_InventoryTotalSetIndexT2 = new clsInit_InventoryTotalSetIndexT2(aBasePlanComputations, NextSequence);
			_init_InventoryTotalSetIndexT3 = new clsInit_InventoryTotalSetIndexT3(aBasePlanComputations, NextSequence);
			_init_InventoryRegularSetIndexT1 = new clsInit_InventoryRegularSetIndexT1(aBasePlanComputations, NextSequence);
			_init_InventoryRegularSetIndexT2 = new clsInit_InventoryRegularSetIndexT2(aBasePlanComputations, NextSequence);
			_init_InventoryRegularSetIndexT3 = new clsInit_InventoryRegularSetIndexT3(aBasePlanComputations, NextSequence);
			_init_InventoryMarkdownSetIndexT1 = new clsInit_InventoryMarkdownSetIndexT1(aBasePlanComputations, NextSequence);
			_init_InventoryMarkdownSetIndexT2 = new clsInit_InventoryMarkdownSetIndexT2(aBasePlanComputations, NextSequence);
			_init_InventoryMarkdownSetIndexT3 = new clsInit_InventoryMarkdownSetIndexT3(aBasePlanComputations, NextSequence);
			_init_InventoryTotalAllIndexT1 = new clsInit_InventoryTotalAllIndexT1(aBasePlanComputations, NextSequence);
			_init_InventoryTotalAllIndexT2 = new clsInit_InventoryTotalAllIndexT2(aBasePlanComputations, NextSequence);
			_init_InventoryTotalAllIndexT3 = new clsInit_InventoryTotalAllIndexT3(aBasePlanComputations, NextSequence);
			_init_InventoryRegularAllIndexT1 = new clsInit_InventoryRegularAllIndexT1(aBasePlanComputations, NextSequence);
			_init_InventoryRegularAllIndexT2 = new clsInit_InventoryRegularAllIndexT2(aBasePlanComputations, NextSequence);
			_init_InventoryRegularAllIndexT3 = new clsInit_InventoryRegularAllIndexT3(aBasePlanComputations, NextSequence);
			_init_InventoryMarkdownAllIndexT1 = new clsInit_InventoryMarkdownAllIndexT1(aBasePlanComputations, NextSequence);
			_init_InventoryMarkdownAllIndexT2 = new clsInit_InventoryMarkdownAllIndexT2(aBasePlanComputations, NextSequence);
			_init_InventoryMarkdownAllIndexT3 = new clsInit_InventoryMarkdownAllIndexT3(aBasePlanComputations, NextSequence);
			_init_ForwardWOSTotalSetIndexT1 = new clsInit_ForwardWOSTotalSetIndexT1(aBasePlanComputations, NextSequence);
			_init_ForwardWOSRegPromoSetIndexT1 = new clsInit_ForwardWOSRegPromoSetIndexT1(aBasePlanComputations, NextSequence);
			_init_ForwardWOSMarkdownSetIndexT1 = new clsInit_ForwardWOSMarkdownSetIndexT1(aBasePlanComputations, NextSequence);
			_init_ForwardWOSTotalAllIndexT1 = new clsInit_ForwardWOSTotalAllIndexT1(aBasePlanComputations, NextSequence);
			_init_ForwardWOSRegPromoAllIndexT1 = new clsInit_ForwardWOSRegPromoAllIndexT1(aBasePlanComputations, NextSequence);
			_init_ForwardWOSMarkdownAllIndexT1 = new clsInit_ForwardWOSMarkdownAllIndexT1(aBasePlanComputations, NextSequence);
			_init_SalesStockRatioTotalT1 = new clsInit_SalesStockRatioTotalT1(aBasePlanComputations, NextSequence);
			_init_SalesStockRatioRegPromoT1 = new clsInit_SalesStockRatioRegPromoT1(aBasePlanComputations, NextSequence);
			_init_SalesStockRatioMarkdownT1 = new clsInit_SalesStockRatioMarkdownT1(aBasePlanComputations, NextSequence);
			_init_SellThruPctTotalT1 = new clsInit_SellThruPctTotalT1(aBasePlanComputations, NextSequence);
			_init_SellThruPctRegPromoT1 = new clsInit_SellThruPctRegPromoT1(aBasePlanComputations, NextSequence);
			_init_SellThruPctMarkdownT1 = new clsInit_SellThruPctMarkdownT1(aBasePlanComputations, NextSequence);
			_init_SellThruPctTotalSetIndexT1 = new clsInit_SellThruPctTotalSetIndexT1(aBasePlanComputations, NextSequence);
			_init_SellThruPctRegPromoSetIndexT1 = new clsInit_SellThruPctRegPromoSetIndexT1(aBasePlanComputations, NextSequence);
			_init_SellThruPctMarkdownSetIndexT1 = new clsInit_SellThruPctMarkdownSetIndexT1(aBasePlanComputations, NextSequence);
			_init_SellThruPctTotalAllIndexT1 = new clsInit_SellThruPctTotalAllIndexT1(aBasePlanComputations, NextSequence);
			_init_SellThruPctRegPromoAllIndexT1 = new clsInit_SellThruPctRegPromoAllIndexT1(aBasePlanComputations, NextSequence);
			_init_SellThruPctMarkdownAllIndexT1 = new clsInit_SellThruPctMarkdownAllIndexT1(aBasePlanComputations, NextSequence);
			_init_GradeTotalT1 = new clsInit_GradeTotalT1(aBasePlanComputations, NextSequence);
			_init_GradeRegPromoT1 = new clsInit_GradeRegPromoT1(aBasePlanComputations, NextSequence);

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Set/Store Inits
			//-------------------------------------------

			_init_SetSalesTotalAllIndex = new clsInit_SetTotalAllIndex(aBasePlanComputations, NextSequence, "SetSalesTotalAllIndex Init", BasePlanVariables.SalesTotalUnits);
			_init_SetSalesRegularAllIndex = new clsInit_SetRegularAllIndex(aBasePlanComputations, NextSequence, "SetSalesRegularAllIndex Init", BasePlanVariables.SalesRegularUnits);
			_init_SetSalesPromoAllIndex = new clsInit_SetRegularAllIndex(aBasePlanComputations, NextSequence, "SetSalesPromoAllIndex Init", BasePlanVariables.SalesPromoUnits);
			_init_SetSalesRegPromoAllIndex = new clsInit_SetRegularAllIndex(aBasePlanComputations, NextSequence, "SetSalesRegPromoAllIndex Init", BasePlanVariables.SalesRegPromoUnits);
			_init_SetSalesMarkdownAllIndex = new clsInit_SetMarkdownAllIndex(aBasePlanComputations, NextSequence, "SetSalesMarkdownAllIndex Init", BasePlanVariables.SalesMarkdownUnits);
			_init_SetInventoryTotalAllIndex = new clsInit_SetTotalAllIndex(aBasePlanComputations, NextSequence, "SetInventoryTotalAllIndex Init", BasePlanVariables.InventoryTotalUnits);
			_init_SetInventoryRegularAllIndex = new clsInit_SetRegularAllIndex(aBasePlanComputations, NextSequence, "SetInventoryRegularAllIndex Init", BasePlanVariables.InventoryRegularUnits);
			_init_SetInventoryMarkdownAllIndex = new clsInit_SetMarkdownAllIndex(aBasePlanComputations, NextSequence, "SetInventoryMarkdownAllIndex Init", BasePlanVariables.InventoryMarkdownUnits);
			_init_SetSalesTotalAllIndexT1 = new clsInit_SetTotalAllIndexTT(aBasePlanComputations, NextSequence, "SetSalesTotalAllIndexT1 Init", BasePlanTimeTotalVariables.SalesTotalUnitsT1);
			_init_SetSalesRegularAllIndexT1 = new clsInit_SetRegularAllIndexTT(aBasePlanComputations, NextSequence, "SetSalesRegularAllIndexT1 Init", BasePlanTimeTotalVariables.SalesRegularUnitsT1);
			_init_SetSalesPromoAllIndexT1 = new clsInit_SetRegularAllIndexTT(aBasePlanComputations, NextSequence, "SetSalesPromoAllIndexT1 Init", BasePlanTimeTotalVariables.SalesPromoUnitsT1);
			_init_SetSalesRegPromoAllIndexT1 = new clsInit_SetRegularAllIndexTT(aBasePlanComputations, NextSequence, "SetSalesRegPromoAllIndexT1 Init", BasePlanTimeTotalVariables.SalesRegPromoUnitsT1);
			_init_SetSalesMarkdownAllIndexT1 = new clsInit_SetMarkdownAllIndexTT(aBasePlanComputations, NextSequence, "SetSalesMarkdownAllIndexT1 Init", BasePlanTimeTotalVariables.SalesMarkdownUnitsT1);
			_init_SetInventoryTotalAllIndexT1 = new clsInit_SetTotalAllIndexTT(aBasePlanComputations, NextSequence, "SetInventoryTotalAllIndexT1 Init", BasePlanTimeTotalVariables.InventoryTotalUnitsT1);
			_init_SetInventoryTotalAllIndexT2 = new clsInit_SetTotalAllIndexTT(aBasePlanComputations, NextSequence, "SetInventoryTotalAllIndexT2 Init", BasePlanTimeTotalVariables.InventoryTotalUnitsT2);
			_init_SetInventoryTotalAllIndexT3 = new clsInit_SetTotalAllIndexTT(aBasePlanComputations, NextSequence, "SetInventoryTotalAllIndexT3 Init", BasePlanTimeTotalVariables.InventoryTotalUnitsT3);
			_init_SetInventoryRegularAllIndexT1 = new clsInit_SetRegularAllIndexTT(aBasePlanComputations, NextSequence, "SetInventoryRegularAllIndexT1 Init", BasePlanTimeTotalVariables.InventoryRegularUnitsT1);
			_init_SetInventoryRegularAllIndexT2 = new clsInit_SetRegularAllIndexTT(aBasePlanComputations, NextSequence, "SetInventoryRegularAllIndexT2 Init", BasePlanTimeTotalVariables.InventoryRegularUnitsT2);
			_init_SetInventoryRegularAllIndexT3 = new clsInit_SetRegularAllIndexTT(aBasePlanComputations, NextSequence, "SetInventoryRegularAllIndexT3 Init", BasePlanTimeTotalVariables.InventoryRegularUnitsT3);
			_init_SetInventoryMarkdownAllIndexT1 = new clsInit_SetMarkdownAllIndexTT(aBasePlanComputations, NextSequence, "SetInventoryMarkdownAllIndexT1 Init", BasePlanTimeTotalVariables.InventoryRegularUnitsT1);
			_init_SetInventoryMarkdownAllIndexT2 = new clsInit_SetMarkdownAllIndexTT(aBasePlanComputations, NextSequence, "SetInventoryMarkdownAllIndexT2 Init", BasePlanTimeTotalVariables.InventoryRegularUnitsT2);
			_init_SetInventoryMarkdownAllIndexT3 = new clsInit_SetMarkdownAllIndexTT(aBasePlanComputations, NextSequence, "SetInventoryMarkdownAllIndexT3 Init", BasePlanTimeTotalVariables.InventoryRegularUnitsT3);

			_init_SumCompStore = new clsInit_SumCompStore(aBasePlanComputations, NextSequence);
			_init_SumNonCompStore = new clsInit_SumNonCompStore(aBasePlanComputations, NextSequence);
			_init_SumNewStore = new clsInit_SumNewStore(aBasePlanComputations, NextSequence);

			_init_SumStoreSalesTotalUnitsT3 = new clsInit_SumStoreSalesTotalUnitsT3(aBasePlanComputations, NextSequence);
			_init_SumCompStoreSalesTotalUnitsT3 = new clsInit_SumCompStoreSalesTotalUnitsT3(aBasePlanComputations, NextSequence);
			_init_SumNonCompStoreSalesTotalUnitsT3 = new clsInit_SumNonCompStoreSalesTotalUnitsT3(aBasePlanComputations, NextSequence);
			_init_SumNewStoreSalesTotalUnitsT3 = new clsInit_SumNewStoreSalesTotalUnitsT3(aBasePlanComputations, NextSequence);
			_init_SumStoreSalesRegularUnitsT2 = new clsInit_SumStoreSalesRegularUnitsT2(aBasePlanComputations, NextSequence);
			_init_SumCompStoreSalesRegularUnitsT2 = new clsInit_SumCompStoreSalesRegularUnitsT2(aBasePlanComputations, NextSequence);
			_init_SumNonCompStoreSalesRegularUnitsT2 = new clsInit_SumNonCompStoreSalesRegularUnitsT2(aBasePlanComputations, NextSequence);
			_init_SumNewStoreSalesRegularUnitsT2 = new clsInit_SumNewStoreSalesRegularUnitsT2(aBasePlanComputations, NextSequence);
			_init_SumStoreSalesPromoUnitsT2 = new clsInit_SumStoreSalesPromoUnitsT2(aBasePlanComputations, NextSequence);
			_init_SumCompStoreSalesPromoUnitsT2 = new clsInit_SumCompStoreSalesPromoUnitsT2(aBasePlanComputations, NextSequence);
			_init_SumNonCompStoreSalesPromoUnitsT2 = new clsInit_SumNonCompStoreSalesPromoUnitsT2(aBasePlanComputations, NextSequence);
			_init_SumNewStoreSalesPromoUnitsT2 = new clsInit_SumNewStoreSalesPromoUnitsT2(aBasePlanComputations, NextSequence);
			_init_SumStoreSalesRegPromoUnitsT3 = new clsInit_SumStoreSalesRegPromoUnitsT3(aBasePlanComputations, NextSequence);
			_init_SumCompStoreSalesRegPromoUnitsT3 = new clsInit_SumCompStoreSalesRegPromoUnitsT3(aBasePlanComputations, NextSequence);
			_init_SumNonCompStoreSalesRegPromoUnitsT3 = new clsInit_SumNonCompStoreSalesRegPromoUnitsT3(aBasePlanComputations, NextSequence);
			_init_SumNewStoreSalesRegPromoUnitsT3 = new clsInit_SumNewStoreSalesRegPromoUnitsT3(aBasePlanComputations, NextSequence);
			_init_SumStoreSalesMarkdownUnitsT3 = new clsInit_SumStoreSalesMarkdownUnitsT3(aBasePlanComputations, NextSequence);
			_init_SumCompStoreSalesMarkdownUnitsT3 = new clsInit_SumCompStoreSalesMarkdownUnitsT3(aBasePlanComputations, NextSequence);
			_init_SumNonCompStoreSalesMarkdownUnitsT3 = new clsInit_SumNonCompStoreSalesMarkdownUnitsT3(aBasePlanComputations, NextSequence);
			_init_SumNewStoreSalesMarkdownUnitsT3 = new clsInit_SumNewStoreSalesMarkdownUnitsT3(aBasePlanComputations, NextSequence);

			_init_SumStoreInventoryTotalUnitsT4 = new clsInit_SumStoreInventoryTotalUnitsT4(aBasePlanComputations, NextSequence);
			_init_SumCompStoreInventoryTotalUnitsT4 = new clsInit_SumCompStoreInventoryTotalUnitsT4(aBasePlanComputations, NextSequence);
			_init_SumNonCompStoreInventoryTotalUnitsT4 = new clsInit_SumNonCompStoreInventoryTotalUnitsT4(aBasePlanComputations, NextSequence);
			_init_SumNewStoreInventoryTotalUnitsT4 = new clsInit_SumNewStoreInventoryTotalUnitsT4(aBasePlanComputations, NextSequence);
			_init_SumStoreInventoryTotalUnitsT5 = new clsInit_SumStoreInventoryTotalUnitsT5(aBasePlanComputations, NextSequence);
			_init_SumCompStoreInventoryTotalUnitsT5 = new clsInit_SumCompStoreInventoryTotalUnitsT5(aBasePlanComputations, NextSequence);
			_init_SumNonCompStoreInventoryTotalUnitsT5 = new clsInit_SumNonCompStoreInventoryTotalUnitsT5(aBasePlanComputations, NextSequence);
			_init_SumNewStoreInventoryTotalUnitsT5 = new clsInit_SumNewStoreInventoryTotalUnitsT5(aBasePlanComputations, NextSequence);
			_init_SumStoreInventoryTotalUnitsT6 = new clsInit_SumStoreInventoryTotalUnitsT6(aBasePlanComputations, NextSequence);
			_init_SumCompStoreInventoryTotalUnitsT6 = new clsInit_SumCompStoreInventoryTotalUnitsT6(aBasePlanComputations, NextSequence);
			_init_SumNonCompStoreInventoryTotalUnitsT6 = new clsInit_SumNonCompStoreInventoryTotalUnitsT6(aBasePlanComputations, NextSequence);
			_init_SumNewStoreInventoryTotalUnitsT6 = new clsInit_SumNewStoreInventoryTotalUnitsT6(aBasePlanComputations, NextSequence);
			_init_SumStoreInventoryTotalUnitsT7 = new clsInit_SumStoreInventoryTotalUnitsT7(aBasePlanComputations, NextSequence);
			_init_SumCompStoreInventoryTotalUnitsT7 = new clsInit_SumCompStoreInventoryTotalUnitsT7(aBasePlanComputations, NextSequence);
			_init_SumNonCompStoreInventoryTotalUnitsT7 = new clsInit_SumNonCompStoreInventoryTotalUnitsT7(aBasePlanComputations, NextSequence);
			_init_SumNewStoreInventoryTotalUnitsT7 = new clsInit_SumNewStoreInventoryTotalUnitsT7(aBasePlanComputations, NextSequence);
			// Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
            _init_SumStoreInventoryTotalUnitsT8 = new clsInit_SumStoreInventoryTotalUnitsT8(aBasePlanComputations, NextSequence);
            _init_SumCompStoreInventoryTotalUnitsT8 = new clsInit_SumCompStoreInventoryTotalUnitsT8(aBasePlanComputations, NextSequence);
            _init_SumNonCompStoreInventoryTotalUnitsT8 = new clsInit_SumNonCompStoreInventoryTotalUnitsT8(aBasePlanComputations, NextSequence);
            _init_SumNewStoreInventoryTotalUnitsT8 = new clsInit_SumNewStoreInventoryTotalUnitsT8(aBasePlanComputations, NextSequence);
			// End TT#2054
			_init_SumStoreInventoryRegularUnitsT4 = new clsInit_SumStoreInventoryRegularUnitsT4(aBasePlanComputations, NextSequence);
			_init_SumCompStoreInventoryRegularUnitsT4 = new clsInit_SumCompStoreInventoryRegularUnitsT4(aBasePlanComputations, NextSequence);
			_init_SumNonCompStoreInventoryRegularUnitsT4 = new clsInit_SumNonCompStoreInventoryRegularUnitsT4(aBasePlanComputations, NextSequence);
			_init_SumNewStoreInventoryRegularUnitsT4 = new clsInit_SumNewStoreInventoryRegularUnitsT4(aBasePlanComputations, NextSequence);
			_init_SumStoreInventoryRegularUnitsT5 = new clsInit_SumStoreInventoryRegularUnitsT5(aBasePlanComputations, NextSequence);
			_init_SumCompStoreInventoryRegularUnitsT5 = new clsInit_SumCompStoreInventoryRegularUnitsT5(aBasePlanComputations, NextSequence);
			_init_SumNonCompStoreInventoryRegularUnitsT5 = new clsInit_SumNonCompStoreInventoryRegularUnitsT5(aBasePlanComputations, NextSequence);
			_init_SumNewStoreInventoryRegularUnitsT5 = new clsInit_SumNewStoreInventoryRegularUnitsT5(aBasePlanComputations, NextSequence);
			_init_SumStoreInventoryRegularUnitsT6 = new clsInit_SumStoreInventoryRegularUnitsT6(aBasePlanComputations, NextSequence);
			_init_SumCompStoreInventoryRegularUnitsT6 = new clsInit_SumCompStoreInventoryRegularUnitsT6(aBasePlanComputations, NextSequence);
			_init_SumNonCompStoreInventoryRegularUnitsT6 = new clsInit_SumNonCompStoreInventoryRegularUnitsT6(aBasePlanComputations, NextSequence);
			_init_SumNewStoreInventoryRegularUnitsT6 = new clsInit_SumNewStoreInventoryRegularUnitsT6(aBasePlanComputations, NextSequence);
			_init_SumStoreInventoryRegularUnitsT7 = new clsInit_SumStoreInventoryRegularUnitsT7(aBasePlanComputations, NextSequence);
			_init_SumCompStoreInventoryRegularUnitsT7 = new clsInit_SumCompStoreInventoryRegularUnitsT7(aBasePlanComputations, NextSequence);
			_init_SumNonCompStoreInventoryRegularUnitsT7 = new clsInit_SumNonCompStoreInventoryRegularUnitsT7(aBasePlanComputations, NextSequence);
			_init_SumNewStoreInventoryRegularUnitsT7 = new clsInit_SumNewStoreInventoryRegularUnitsT7(aBasePlanComputations, NextSequence);
			// Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
            _init_SumStoreInventoryRegularUnitsT8 = new clsInit_SumStoreInventoryRegularUnitsT8(aBasePlanComputations, NextSequence);
            _init_SumCompStoreInventoryRegularUnitsT8 = new clsInit_SumCompStoreInventoryRegularUnitsT8(aBasePlanComputations, NextSequence);
            _init_SumNonCompStoreInventoryRegularUnitsT8 = new clsInit_SumNonCompStoreInventoryRegularUnitsT8(aBasePlanComputations, NextSequence);
            _init_SumNewStoreInventoryRegularUnitsT8 = new clsInit_SumNewStoreInventoryRegularUnitsT8(aBasePlanComputations, NextSequence);
			// End TT#2054
			_init_AvgStoreSalesTotalUnits = new clsInit_AvgStoreSalesTotalUnits(aBasePlanComputations, NextSequence);
			_init_AvgStoreSalesRegularUnits = new clsInit_AvgStoreSalesRegularUnits(aBasePlanComputations, NextSequence);
			_init_AvgStoreSalesMarkdownUnits = new clsInit_AvgStoreSalesMarkdownUnits(aBasePlanComputations, NextSequence);
			_init_AvgStoreInventoryTotalUnits = new clsInit_AvgStoreInventoryTotalUnits(aBasePlanComputations, NextSequence);
			_init_AvgStoreInventoryRegularUnits = new clsInit_AvgStoreInventoryRegularUnits(aBasePlanComputations, NextSequence);
			_init_AvgStoreInventoryMarkdownUnits = new clsInit_AvgStoreInventoryMarkdownUnits(aBasePlanComputations, NextSequence);

			_init_AvgStoreSalesTotalUnits_LowLevelTotal = new clsInit_AvgStoreSalesTotalUnits_LowLevelTotal(aBasePlanComputations, NextSequence);
			_init_AvgStoreSalesRegularUnits_LowLevelTotal = new clsInit_AvgStoreSalesRegularUnits_LowLevelTotal(aBasePlanComputations, NextSequence);
			_init_AvgStoreSalesMarkdownUnits_LowLevelTotal = new clsInit_AvgStoreSalesMarkdownUnits_LowLevelTotal(aBasePlanComputations, NextSequence);
			_init_AvgStoreInventoryTotalUnits_LowLevelTotal = new clsInit_AvgStoreInventoryTotalUnits_LowLevelTotal(aBasePlanComputations, NextSequence);
			_init_AvgStoreInventoryRegularUnits_LowLevelTotal = new clsInit_AvgStoreInventoryRegularUnits_LowLevelTotal(aBasePlanComputations, NextSequence);
			_init_AvgStoreInventoryMarkdownUnits_LowLevelTotal = new clsInit_AvgStoreInventoryMarkdownUnits_LowLevelTotal(aBasePlanComputations, NextSequence);

			_init_AvgStoreSalesTotalUnitsT3 = new clsInit_AvgStoreSalesTotalUnitsT3(aBasePlanComputations, NextSequence);
			_init_AvgStoreSalesRegularUnitsT2 = new clsInit_AvgStoreSalesRegularUnitsT2(aBasePlanComputations, NextSequence);
			_init_AvgStoreSalesPromoUnitsT2 = new clsInit_AvgStoreSalesPromoUnitsT2(aBasePlanComputations, NextSequence);
			_init_AvgStoreSalesRegPromoUnitsT3 = new clsInit_AvgStoreSalesRegPromoUnitsT3(aBasePlanComputations, NextSequence);
			_init_AvgStoreSalesMarkdownUnitsT3 = new clsInit_AvgStoreSalesMarkdownUnitsT3(aBasePlanComputations, NextSequence);

			_init_AvgStoreInventoryTotalUnitsT4 = new clsInit_AvgStoreInventoryTotalUnitsT4(aBasePlanComputations, NextSequence);
			_init_AvgStoreInventoryTotalUnitsT5 = new clsInit_AvgStoreInventoryTotalUnitsT5(aBasePlanComputations, NextSequence);
			_init_AvgStoreInventoryTotalUnitsT6 = new clsInit_AvgStoreInventoryTotalUnitsT6(aBasePlanComputations, NextSequence);
            _init_AvgStoreInventoryTotalUnitsT8 = new clsInit_AvgStoreInventoryTotalUnitsT8(aBasePlanComputations, NextSequence);		// TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
			_init_AvgStoreInventoryRegularUnitsT4 = new clsInit_AvgStoreInventoryRegularUnitsT4(aBasePlanComputations, NextSequence);
			_init_AvgStoreInventoryRegularUnitsT5 = new clsInit_AvgStoreInventoryRegularUnitsT5(aBasePlanComputations, NextSequence);
			_init_AvgStoreInventoryRegularUnitsT6 = new clsInit_AvgStoreInventoryRegularUnitsT6(aBasePlanComputations, NextSequence);
            _init_AvgStoreInventoryRegularUnitsT8 = new clsInit_AvgStoreInventoryRegularUnitsT8(aBasePlanComputations, NextSequence);	// TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Period Inits
			//-------------------------------------------

			_init_SumWeekDetail = new clsInit_SumWeekDetail(aBasePlanComputations, NextSequence);

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Low-level Inits
			//-------------------------------------------

			_init_SumLowLevelTotalDetail = new clsInit_SumLowLevelTotalDetail(aBasePlanComputations, NextSequence);
			_init_SumLowLevelRegularDetail = new clsInit_SumLowLevelRegularDetail(aBasePlanComputations, NextSequence);

			_init_SumLowLevelInventoryTotalUnitsT7 = new clsInit_SumLowLevelInventoryTotalUnitsT7(aBasePlanComputations, NextSequence);
			_init_SumLowLevelInventoryRegularUnitsT7 = new clsInit_SumLowLevelInventoryRegularUnitsT7(aBasePlanComputations, NextSequence);
			_init_SumLowLevelStoreInventoryTotalUnitsT7 = new clsInit_SumLowLevelStoreInventoryTotalUnitsT7(aBasePlanComputations, NextSequence);
			_init_SumLowLevelStoreInventoryRegularUnitsT7 = new clsInit_SumLowLevelStoreInventoryRegularUnitsT7(aBasePlanComputations, NextSequence);

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Comparative Inits
			//-------------------------------------------

			_init_PctChange = new clsInit_PctChange(aBasePlanComputations, NextSequence);
			_init_PctToSet = new clsInit_PctToSet(aBasePlanComputations, NextSequence);
			_init_PctToAll = new clsInit_PctToAll(aBasePlanComputations, NextSequence);
			_init_PctToLowLevel = new clsInit_PctToLowLevel(aBasePlanComputations, NextSequence);
			_init_PctToTimePeriod = new clsInit_PctToTimePeriod(aBasePlanComputations, NextSequence);
			_init_ChainBalance = new clsInit_ChainBalance(aBasePlanComputations, NextSequence);
			_init_StoreBalance = new clsInit_StoreBalance(aBasePlanComputations, NextSequence);
			_init_Difference = new clsInit_Difference(aBasePlanComputations, NextSequence);
			_init_PctChangeToPlan = new clsInit_PctChangeToPlan(aBasePlanComputations, NextSequence);

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Variable Change Formulas
			//-------------------------------------------

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Time Total Change Formulas
			//-------------------------------------------

			//-------------------------------------------
			#endregion
			//-------------------------------------------

		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the next sequence number for creating Change Methods
		/// </summary>

		protected int NextSequence
		{
			get
			{
				return _seq++;
			}
		}
		
		/// <summary>
		/// Gets the BasePlanComputations
		/// </summary>

		protected BasePlanComputations BasePlanComputations
		{
			get
			{
				return _basePlanComputations;
			}
		}

		/// <summary>
		/// Gets the BasePlanVariables
		/// </summary>

		protected BasePlanVariables BasePlanVariables
		{
			get
			{
				return _basePlanComputations.BasePlanVariables;
			}
		}

		/// <summary>
		/// Gets the BasePlanTimeTotalVariables
		/// </summary>

		protected BasePlanTimeTotalVariables BasePlanTimeTotalVariables
		{
			get
			{
				return _basePlanComputations.BasePlanTimeTotalVariables;
			}
		}

		//-------------------------------------------
		#region Spreads
		//-------------------------------------------

		public SpreadProfile Spread_Set { get { return _spread_Set; } }
		public SpreadProfile Spread_SetAverage { get { return _spread_SetAverage; } }
		public SpreadProfile Spread_SetComp { get { return _spread_SetComp; } }
		public SpreadProfile Spread_SetNonComp { get { return _spread_SetNonComp; } }
		public SpreadProfile Spread_SetNew { get { return _spread_SetNew; } }
		public SpreadProfile Spread_All { get { return _spread_All; } }
		public SpreadProfile Spread_AllAverage { get { return _spread_AllAverage; } }
		public SpreadProfile Spread_AllComp { get { return _spread_AllComp; } }
		public SpreadProfile Spread_AllNonComp { get { return _spread_AllNonComp; } }
		public SpreadProfile Spread_AllNew { get { return _spread_AllNew; } }
		public SpreadProfile Spread_Period { get { return _spread_Period; } }
		public SpreadProfile Spread_PeriodAverage { get { return _spread_PeriodAverage; } }
		public SpreadProfile Spread_Date { get { return _spread_Date; } }
		public SpreadProfile Spread_DateAverage { get { return _spread_DateAverage; } }
		public SpreadProfile Spread_LowLevel { get { return _spread_LowLevel; } }
		public SpreadProfile Spread_LowLevelAverage { get { return _spread_LowLevelAverage; } }
		public SpreadProfile Spread_PeriodNoCascade { get { return _spread_PeriodNoCascade; } }
		public SpreadProfile Spread_TotalPeriodToWeeks { get { return _spread_TotalPeriodToWeeks; } }
		public SpreadProfile Spread_SalesTotalUnitsAverage { get { return _spread_SalesTotalUnitsAverage; } }
		public SpreadProfile Spread_SalesRegularUnitsAverage { get { return _spread_SalesRegularUnitsAverage; } }
		public SpreadProfile Spread_SalesPromoUnitsAverage { get { return _spread_SalesPromoUnitsAverage; } }
		public SpreadProfile Spread_SalesRegPromoUnitsAverage { get { return _spread_SalesRegPromoUnitsAverage; } }
		public SpreadProfile Spread_InventoryTotalUnitsAverage { get { return _spread_InventoryTotalUnitsAverage; } }
		public SpreadProfile Spread_InventoryRegularUnitsAverage { get { return _spread_InventoryRegularUnitsAverage; } }
		public SpreadProfile Spread_SalesTotalUnitsAverage_LowLevelTotal { get { return _spread_SalesTotalUnitsAverage_LowLevelTotal; } }
		public SpreadProfile Spread_SalesRegularUnitsAverage_LowLevelTotal { get { return _spread_SalesRegularUnitsAverage_LowLevelTotal; } }
		public SpreadProfile Spread_SalesPromoUnitsAverage_LowLevelTotal { get { return _spread_SalesPromoUnitsAverage_LowLevelTotal; } }
		public SpreadProfile Spread_SalesRegPromoUnitsAverage_LowLevelTotal { get { return _spread_SalesRegPromoUnitsAverage_LowLevelTotal; } }
		public SpreadProfile Spread_InventoryTotalUnitsAverage_LowLevelTotal { get { return _spread_InventoryTotalUnitsAverage_LowLevelTotal; } }
		public SpreadProfile Spread_InventoryRegularUnitsAverage_LowLevelTotal { get { return _spread_InventoryRegularUnitsAverage_LowLevelTotal; } }
        public SpreadProfile Spread_SalesRegPromoUnits { get { return _spread_SalesRegPromoUnits; } }   // TT#1722 - RMatelic - Open Sales R/P to be editable 
        
        //-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Inits
		//-------------------------------------------

		public FormulaProfile Init_Null { get { return _init_Null; } }
		public FormulaProfile Init_SalesTotalUnits { get { return _init_SalesTotalUnits; } }
		public FormulaProfile Init_SalesRegularUnits { get { return _init_SalesRegularUnits; } }
		public FormulaProfile Init_SalesPromoUnits { get { return _init_SalesPromoUnits; } }
		public FormulaProfile Init_SalesRegPromoUnits { get { return _init_SalesRegPromoUnits; } }
		public FormulaProfile Init_SalesMarkdownUnits { get { return _init_SalesMarkdownUnits; } }
		public FormulaProfile Init_SalesTotalSetIndex { get { return _init_SalesTotalSetIndex; } }
		public FormulaProfile Init_SalesRegularSetIndex { get { return _init_SalesRegularSetIndex; } }
		public FormulaProfile Init_SalesPromoSetIndex { get { return _init_SalesPromoSetIndex; } }
		public FormulaProfile Init_SalesRegPromoSetIndex { get { return _init_SalesRegPromoSetIndex; } }
		public FormulaProfile Init_SalesMarkdownSetIndex { get { return _init_SalesMarkdownSetIndex; } }
		public FormulaProfile Init_SalesTotalAllIndex { get { return _init_SalesTotalAllIndex; } }
		public FormulaProfile Init_SalesRegularAllIndex { get { return _init_SalesRegularAllIndex; } }
		public FormulaProfile Init_SalesPromoAllIndex { get { return _init_SalesPromoAllIndex; } }
		public FormulaProfile Init_SalesRegPromoAllIndex { get { return _init_SalesRegPromoAllIndex; } }
		public FormulaProfile Init_SalesMarkdownAllIndex { get { return _init_SalesMarkdownAllIndex; } }
		public FormulaProfile Init_InventoryTotalUnits { get { return _init_InventoryTotalUnits; } }
		public FormulaProfile Init_InventoryRegularUnits { get { return _init_InventoryRegularUnits; } }
		public FormulaProfile Init_InventoryMarkdownUnits { get { return _init_InventoryMarkdownUnits; } }
		public FormulaProfile Init_InventoryTotalSetIndex { get { return _init_InventoryTotalSetIndex; } }
		public FormulaProfile Init_InventoryRegularSetIndex { get { return _init_InventoryRegularSetIndex; } }
		public FormulaProfile Init_InventoryMarkdownSetIndex { get { return _init_InventoryMarkdownSetIndex; } }
		public FormulaProfile Init_InventoryTotalAllIndex { get { return _init_InventoryTotalAllIndex; } }
		public FormulaProfile Init_InventoryRegularAllIndex { get { return _init_InventoryRegularAllIndex; } }
		public FormulaProfile Init_InventoryMarkdownAllIndex { get { return _init_InventoryMarkdownAllIndex; } }
		public FormulaProfile Init_ReceiptTotalUnits { get { return _init_ReceiptTotalUnits; } }
		public FormulaProfile Init_ReceiptRegularUnits { get { return _init_ReceiptRegularUnits; } }
		public FormulaProfile Init_ReceiptMarkdownUnits { get { return _init_ReceiptMarkdownUnits; } }
		public FormulaProfile Init_SalesStockRatioTotal { get { return _init_SalesStockRatioTotal; } }
		public FormulaProfile Init_SalesStockRatioRegPromo { get { return _init_SalesStockRatioRegPromo; } }
		public FormulaProfile Init_SalesStockRatioMarkdown { get { return _init_SalesStockRatioMarkdown; } }
		public FormulaProfile Init_ForwardWOSTotal { get { return _init_ForwardWOSTotal; } }
		public FormulaProfile Init_ForwardWOSRegPromo { get { return _init_ForwardWOSRegPromo; } }
		public FormulaProfile Init_ForwardWOSMarkdown { get { return _init_ForwardWOSMarkdown; } }
		public FormulaProfile Init_ForwardWOSTotalSetIndex { get { return _init_ForwardWOSTotalSetIndex; } }
		public FormulaProfile Init_ForwardWOSRegPromoSetIndex { get { return _init_ForwardWOSRegPromoSetIndex; } }
		public FormulaProfile Init_ForwardWOSMarkdownSetIndex { get { return _init_ForwardWOSMarkdownSetIndex; } }
		public FormulaProfile Init_ForwardWOSTotalAllIndex { get { return _init_ForwardWOSTotalAllIndex; } }
		public FormulaProfile Init_ForwardWOSRegPromoAllIndex { get { return _init_ForwardWOSRegPromoAllIndex; } }
		public FormulaProfile Init_ForwardWOSMarkdownAllIndex { get { return _init_ForwardWOSMarkdownAllIndex; } }
		public FormulaProfile Init_SellThruPctTotal { get { return _init_SellThruPctTotal; } }
		public FormulaProfile Init_SellThruPctRegPromo { get { return _init_SellThruPctRegPromo; } }
		public FormulaProfile Init_SellThruPctMarkdown { get { return _init_SellThruPctMarkdown; } }
		public FormulaProfile Init_SellThruPctTotalSetIndex { get { return _init_SellThruPctTotalSetIndex; } }
		public FormulaProfile Init_SellThruPctRegPromoSetIndex { get { return _init_SellThruPctRegPromoSetIndex; } }
		public FormulaProfile Init_SellThruPctMarkdownSetIndex { get { return _init_SellThruPctMarkdownSetIndex; } }
		public FormulaProfile Init_SellThruPctTotalAllIndex { get { return _init_SellThruPctTotalAllIndex; } }
		public FormulaProfile Init_SellThruPctRegPromoAllIndex { get { return _init_SellThruPctRegPromoAllIndex; } }
		public FormulaProfile Init_SellThruPctMarkdownAllIndex { get { return _init_SellThruPctMarkdownAllIndex; } }
		public FormulaProfile Init_Intransit { get { return _init_Intransit; } }
		public FormulaProfile Init_GradeTotal { get { return _init_GradeTotal; } }
		public FormulaProfile Init_GradeRegPromo { get { return _init_GradeRegPromo; } }
		public FormulaProfile Init_StoreStatus { get { return _init_StoreStatus; } }

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Generic Total Inits
		//-------------------------------------------

		public FormulaProfile Init_SumDetail { get { return _init_SumDetail; } }
		public FormulaProfile Init_AvgDetail { get { return _init_AvgDetail; } }
		public FormulaProfile Init_AvgDetail_LowLevelTotal { get { return _init_AvgDetail_LowLevelTotal; } }
		public FormulaProfile Init_PeriodSumDetail { get { return _init_PeriodSumDetail; } }
		//Begin TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
		public FormulaProfile Init_PeriodAvgDetail { get { return _init_PeriodAvgDetail; } }
		//End TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Time Total Inits
		//-------------------------------------------

		public FormulaProfile Init_SalesUnitsTimeAvg { get { return _init_SalesUnitsTimeAvg; } }
		public FormulaProfile Init_InventoryUnitsTimeAvg { get { return _init_InventoryUnitsTimeAvg; } }
		public FormulaProfile Init_InventoryUnitsEnding { get { return _init_InventoryUnitsEnding; } }
		public FormulaProfile Init_SalesTotalUnitsT3 { get { return _init_SalesTotalUnitsT3; } }
		public FormulaProfile Init_SalesRegularUnitsT2 { get { return _init_SalesRegularUnitsT2; } }
		public FormulaProfile Init_SalesPromoUnitsT2 { get { return _init_SalesPromoUnitsT2; } }
		public FormulaProfile Init_SalesRegPromoUnitsT3 { get { return _init_SalesRegPromoUnitsT3; } }
		public FormulaProfile Init_SalesMarkdownUnitsT3 { get { return _init_SalesMarkdownUnitsT3; } }
		public FormulaProfile Init_SalesTotalSetIndexT1 { get { return _init_SalesTotalSetIndexT1; } }
		public FormulaProfile Init_SalesRegularSetIndexT1 { get { return _init_SalesRegularSetIndexT1; } }
		public FormulaProfile Init_SalesPromoSetIndexT1 { get { return _init_SalesPromoSetIndexT1; } }
		public FormulaProfile Init_SalesRegPromoSetIndexT1 { get { return _init_SalesRegPromoSetIndexT1; } }
		public FormulaProfile Init_SalesMarkdownSetIndexT1 { get { return _init_SalesMarkdownSetIndexT1; } }
		public FormulaProfile Init_SalesTotalAllIndexT1 { get { return _init_SalesTotalAllIndexT1; } }
		public FormulaProfile Init_SalesRegularAllIndexT1 { get { return _init_SalesRegularAllIndexT1; } }
		public FormulaProfile Init_SalesPromoAllIndexT1 { get { return _init_SalesPromoAllIndexT1; } }
		public FormulaProfile Init_SalesRegPromoAllIndexT1 { get { return _init_SalesRegPromoAllIndexT1; } }
		public FormulaProfile Init_SalesMarkdownAllIndexT1 { get { return _init_SalesMarkdownAllIndexT1; } }
		public FormulaProfile Init_InventoryTotalUnitsT4 { get { return _init_InventoryTotalUnitsT4; } }
		public FormulaProfile Init_InventoryTotalUnitsT5 { get { return _init_InventoryTotalUnitsT5; } }
		public FormulaProfile Init_InventoryTotalUnitsT6 { get { return _init_InventoryTotalUnitsT6; } }
		public FormulaProfile Init_InventoryTotalUnitsT7 { get { return _init_InventoryTotalUnitsT7; } }
        public FormulaProfile Init_InventoryTotalUnitsT8 { get { return _init_InventoryTotalUnitsT8; } }		// TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
		public FormulaProfile Init_InventoryRegularUnitsT4 { get { return _init_InventoryRegularUnitsT4; } }
		public FormulaProfile Init_InventoryRegularUnitsT5 { get { return _init_InventoryRegularUnitsT5; } }
		public FormulaProfile Init_InventoryRegularUnitsT6 { get { return _init_InventoryRegularUnitsT6; } }
		public FormulaProfile Init_InventoryRegularUnitsT7 { get { return _init_InventoryRegularUnitsT7; } }
        public FormulaProfile Init_InventoryRegularUnitsT8 { get { return _init_InventoryRegularUnitsT8; } }	// TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
		public FormulaProfile Init_InventoryTotalSetIndexT1 { get { return _init_InventoryTotalSetIndexT1; } }
		public FormulaProfile Init_InventoryTotalSetIndexT2 { get { return _init_InventoryTotalSetIndexT2; } }
		public FormulaProfile Init_InventoryTotalSetIndexT3 { get { return _init_InventoryTotalSetIndexT3; } }
		public FormulaProfile Init_InventoryRegularSetIndexT1 { get { return _init_InventoryRegularSetIndexT1; } }
		public FormulaProfile Init_InventoryRegularSetIndexT2 { get { return _init_InventoryRegularSetIndexT2; } }
		public FormulaProfile Init_InventoryRegularSetIndexT3 { get { return _init_InventoryRegularSetIndexT3; } }
		public FormulaProfile Init_InventoryMarkdownSetIndexT1 { get { return _init_InventoryMarkdownSetIndexT1; } }
		public FormulaProfile Init_InventoryMarkdownSetIndexT2 { get { return _init_InventoryMarkdownSetIndexT2; } }
		public FormulaProfile Init_InventoryMarkdownSetIndexT3 { get { return _init_InventoryMarkdownSetIndexT3; } }
		public FormulaProfile Init_InventoryTotalAllIndexT1 { get { return _init_InventoryTotalAllIndexT1; } }
		public FormulaProfile Init_InventoryTotalAllIndexT2 { get { return _init_InventoryTotalAllIndexT2; } }
		public FormulaProfile Init_InventoryTotalAllIndexT3 { get { return _init_InventoryTotalAllIndexT3; } }
		public FormulaProfile Init_InventoryRegularAllIndexT1 { get { return _init_InventoryRegularAllIndexT1; } }
		public FormulaProfile Init_InventoryRegularAllIndexT2 { get { return _init_InventoryRegularAllIndexT2; } }
		public FormulaProfile Init_InventoryRegularAllIndexT3 { get { return _init_InventoryRegularAllIndexT3; } }
		public FormulaProfile Init_InventoryMarkdownAllIndexT1 { get { return _init_InventoryMarkdownAllIndexT1; } }
		public FormulaProfile Init_InventoryMarkdownAllIndexT2 { get { return _init_InventoryMarkdownAllIndexT2; } }
		public FormulaProfile Init_InventoryMarkdownAllIndexT3 { get { return _init_InventoryMarkdownAllIndexT3; } }
		public FormulaProfile Init_ForwardWOSTotalSetIndexT1 { get { return _init_ForwardWOSTotalSetIndexT1; } }
		public FormulaProfile Init_ForwardWOSRegPromoSetIndexT1 { get { return _init_ForwardWOSRegPromoSetIndexT1; } }
		public FormulaProfile Init_ForwardWOSMarkdownSetIndexT1 { get { return _init_ForwardWOSMarkdownSetIndexT1; } }
		public FormulaProfile Init_ForwardWOSTotalAllIndexT1 { get { return _init_ForwardWOSTotalAllIndexT1; } }
		public FormulaProfile Init_ForwardWOSRegPromoAllIndexT1 { get { return _init_ForwardWOSRegPromoAllIndexT1; } }
		public FormulaProfile Init_ForwardWOSMarkdownAllIndexT1 { get { return _init_ForwardWOSMarkdownAllIndexT1; } }
		public FormulaProfile Init_SalesStockRatioTotalT1 { get { return _init_SalesStockRatioTotalT1; } }
		public FormulaProfile Init_SalesStockRatioRegPromoT1 { get { return _init_SalesStockRatioRegPromoT1; } }
		public FormulaProfile Init_SalesStockRatioMarkdownT1 { get { return _init_SalesStockRatioMarkdownT1; } }
		public FormulaProfile Init_SellThruPctTotalT1 { get { return _init_SellThruPctTotalT1; } }
		public FormulaProfile Init_SellThruPctRegPromoT1 { get { return _init_SellThruPctRegPromoT1; } }
		public FormulaProfile Init_SellThruPctMarkdownT1 { get { return _init_SellThruPctMarkdownT1; } }
		public FormulaProfile Init_SellThruPctTotalSetIndexT1 { get { return _init_SellThruPctTotalSetIndexT1; } }
		public FormulaProfile Init_SellThruPctRegPromoSetIndexT1 { get { return _init_SellThruPctRegPromoSetIndexT1; } }
		public FormulaProfile Init_SellThruPctMarkdownSetIndexT1 { get { return _init_SellThruPctMarkdownSetIndexT1; } }
		public FormulaProfile Init_SellThruPctTotalAllIndexT1 { get { return _init_SellThruPctTotalAllIndexT1; } }
		public FormulaProfile Init_SellThruPctRegPromoAllIndexT1 { get { return _init_SellThruPctRegPromoAllIndexT1; } }
		public FormulaProfile Init_SellThruPctMarkdownAllIndexT1 { get { return _init_SellThruPctMarkdownAllIndexT1; } }
		public FormulaProfile Init_GradeTotalT1 { get { return _init_GradeTotalT1; } }
		public FormulaProfile Init_GradeRegPromoT1 { get { return _init_GradeRegPromoT1; } }

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Set/Store Inits
		//-------------------------------------------

		public FormulaProfile Init_SetSalesTotalAllIndex { get { return _init_SetSalesTotalAllIndex; } }
		public FormulaProfile Init_SetSalesRegularAllIndex { get { return _init_SetSalesRegularAllIndex; } }
		public FormulaProfile Init_SetSalesPromoAllIndex { get { return _init_SetSalesPromoAllIndex; } }
		public FormulaProfile Init_SetSalesRegPromoAllIndex { get { return _init_SetSalesRegPromoAllIndex; } }
		public FormulaProfile Init_SetSalesMarkdownAllIndex { get { return _init_SetSalesMarkdownAllIndex; } }
		public FormulaProfile Init_SetInventoryTotalAllIndex { get { return _init_SetInventoryTotalAllIndex; } }
		public FormulaProfile Init_SetInventoryRegularAllIndex { get { return _init_SetInventoryRegularAllIndex; } }
		public FormulaProfile Init_SetInventoryMarkdownAllIndex { get { return _init_SetInventoryMarkdownAllIndex; } }
		public FormulaProfile Init_SetSalesTotalAllIndexT1 { get { return _init_SetSalesTotalAllIndexT1; } }
		public FormulaProfile Init_SetSalesRegularAllIndexT1 { get { return _init_SetSalesRegularAllIndexT1; } }
		public FormulaProfile Init_SetSalesPromoAllIndexT1 { get { return _init_SetSalesPromoAllIndexT1; } }
		public FormulaProfile Init_SetSalesRegPromoAllIndexT1 { get { return _init_SetSalesRegPromoAllIndexT1; } }
		public FormulaProfile Init_SetSalesMarkdownAllIndexT1 { get { return _init_SetSalesMarkdownAllIndexT1; } }
		public FormulaProfile Init_SetInventoryTotalAllIndexT1 { get { return _init_SetInventoryTotalAllIndexT1; } }
		public FormulaProfile Init_SetInventoryTotalAllIndexT2 { get { return _init_SetInventoryTotalAllIndexT2; } }
		public FormulaProfile Init_SetInventoryTotalAllIndexT3 { get { return _init_SetInventoryTotalAllIndexT3; } }
		public FormulaProfile Init_SetInventoryRegularAllIndexT1 { get { return _init_SetInventoryRegularAllIndexT1; } }
		public FormulaProfile Init_SetInventoryRegularAllIndexT2 { get { return _init_SetInventoryRegularAllIndexT2; } }
		public FormulaProfile Init_SetInventoryRegularAllIndexT3 { get { return _init_SetInventoryRegularAllIndexT3; } }
		public FormulaProfile Init_SetInventoryMarkdownAllIndexT1 { get { return _init_SetInventoryMarkdownAllIndexT1; } }
		public FormulaProfile Init_SetInventoryMarkdownAllIndexT2 { get { return _init_SetInventoryMarkdownAllIndexT2; } }
		public FormulaProfile Init_SetInventoryMarkdownAllIndexT3 { get { return _init_SetInventoryMarkdownAllIndexT3; } }

		public FormulaProfile Init_SumCompStore { get { return _init_SumCompStore; } }
		public FormulaProfile Init_SumNonCompStore { get { return _init_SumNonCompStore; } }
		public FormulaProfile Init_SumNewStore { get { return _init_SumNewStore; } }

		public FormulaProfile Init_SumStoreSalesTotalUnitsT3 { get { return _init_SumStoreSalesTotalUnitsT3; } }
		public FormulaProfile Init_SumCompStoreSalesTotalUnitsT3 { get { return _init_SumCompStoreSalesTotalUnitsT3; } }
		public FormulaProfile Init_SumNonCompStoreSalesTotalUnitsT3 { get { return _init_SumNonCompStoreSalesTotalUnitsT3; } }
		public FormulaProfile Init_SumNewStoreSalesTotalUnitsT3 { get { return _init_SumNewStoreSalesTotalUnitsT3; } }
		public FormulaProfile Init_SumStoreSalesRegularUnitsT2 { get { return _init_SumStoreSalesRegularUnitsT2; } }
		public FormulaProfile Init_SumCompStoreSalesRegularUnitsT2 { get { return _init_SumCompStoreSalesRegularUnitsT2; } }
		public FormulaProfile Init_SumNonCompStoreSalesRegularUnitsT2 { get { return _init_SumNonCompStoreSalesRegularUnitsT2; } }
		public FormulaProfile Init_SumNewStoreSalesRegularUnitsT2 { get { return _init_SumNewStoreSalesRegularUnitsT2; } }
		public FormulaProfile Init_SumStoreSalesPromoUnitsT2 { get { return _init_SumStoreSalesPromoUnitsT2; } }
		public FormulaProfile Init_SumCompStoreSalesPromoUnitsT2 { get { return _init_SumCompStoreSalesPromoUnitsT2; } }
		public FormulaProfile Init_SumNonCompStoreSalesPromoUnitsT2 { get { return _init_SumNonCompStoreSalesPromoUnitsT2; } }
		public FormulaProfile Init_SumNewStoreSalesPromoUnitsT2 { get { return _init_SumNewStoreSalesPromoUnitsT2; } }
		public FormulaProfile Init_SumStoreSalesRegPromoUnitsT3 { get { return _init_SumStoreSalesRegPromoUnitsT3; } }
		public FormulaProfile Init_SumCompStoreSalesRegPromoUnitsT3 { get { return _init_SumCompStoreSalesRegPromoUnitsT3; } }
		public FormulaProfile Init_SumNonCompStoreSalesRegPromoUnitsT3 { get { return _init_SumNonCompStoreSalesRegPromoUnitsT3; } }
		public FormulaProfile Init_SumNewStoreSalesRegPromoUnitsT3 { get { return _init_SumNewStoreSalesRegPromoUnitsT3; } }
		public FormulaProfile Init_SumStoreSalesMarkdownUnitsT3 { get { return _init_SumStoreSalesMarkdownUnitsT3; } }
		public FormulaProfile Init_SumCompStoreSalesMarkdownUnitsT3 { get { return _init_SumCompStoreSalesMarkdownUnitsT3; } }
		public FormulaProfile Init_SumNonCompStoreSalesMarkdownUnitsT3 { get { return _init_SumNonCompStoreSalesMarkdownUnitsT3; } }
		public FormulaProfile Init_SumNewStoreSalesMarkdownUnitsT3 { get { return _init_SumNewStoreSalesMarkdownUnitsT3; } }

		public FormulaProfile Init_SumStoreInventoryTotalUnitsT4 { get { return _init_SumStoreInventoryTotalUnitsT4; } }
		public FormulaProfile Init_SumCompStoreInventoryTotalUnitsT4 { get { return _init_SumCompStoreInventoryTotalUnitsT4; } }
		public FormulaProfile Init_SumNonCompStoreInventoryTotalUnitsT4 { get { return _init_SumNonCompStoreInventoryTotalUnitsT4; } }
		public FormulaProfile Init_SumNewStoreInventoryTotalUnitsT4 { get { return _init_SumNewStoreInventoryTotalUnitsT4; } }
		public FormulaProfile Init_SumStoreInventoryTotalUnitsT5 { get { return _init_SumStoreInventoryTotalUnitsT5; } }
		public FormulaProfile Init_SumCompStoreInventoryTotalUnitsT5 { get { return _init_SumCompStoreInventoryTotalUnitsT5; } }
		public FormulaProfile Init_SumNonCompStoreInventoryTotalUnitsT5 { get { return _init_SumNonCompStoreInventoryTotalUnitsT5; } }
		public FormulaProfile Init_SumNewStoreInventoryTotalUnitsT5 { get { return _init_SumNewStoreInventoryTotalUnitsT5; } }
		public FormulaProfile Init_SumStoreInventoryTotalUnitsT6 { get { return _init_SumStoreInventoryTotalUnitsT6; } }
		public FormulaProfile Init_SumCompStoreInventoryTotalUnitsT6 { get { return _init_SumCompStoreInventoryTotalUnitsT6; } }
		public FormulaProfile Init_SumNonCompStoreInventoryTotalUnitsT6 { get { return _init_SumNonCompStoreInventoryTotalUnitsT6; } }
		public FormulaProfile Init_SumNewStoreInventoryTotalUnitsT6 { get { return _init_SumNewStoreInventoryTotalUnitsT6; } }
		public FormulaProfile Init_SumStoreInventoryTotalUnitsT7 { get { return _init_SumStoreInventoryTotalUnitsT7; } }
		public FormulaProfile Init_SumCompStoreInventoryTotalUnitsT7 { get { return _init_SumCompStoreInventoryTotalUnitsT7; } }
		public FormulaProfile Init_SumNonCompStoreInventoryTotalUnitsT7 { get { return _init_SumNonCompStoreInventoryTotalUnitsT7; } }
		public FormulaProfile Init_SumNewStoreInventoryTotalUnitsT7 { get { return _init_SumNewStoreInventoryTotalUnitsT7; } }
	    // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
        public FormulaProfile Init_SumStoreInventoryTotalUnitsT8 { get { return _init_SumStoreInventoryTotalUnitsT8; } }
        public FormulaProfile Init_SumCompStoreInventoryTotalUnitsT8 { get { return _init_SumCompStoreInventoryTotalUnitsT8; } }
        public FormulaProfile Init_SumNonCompStoreInventoryTotalUnitsT8 { get { return _init_SumNonCompStoreInventoryTotalUnitsT8; } }
        public FormulaProfile Init_SumNewStoreInventoryTotalUnitsT8 { get { return _init_SumNewStoreInventoryTotalUnitsT8; } }
		// End TT#2054
		public FormulaProfile Init_SumStoreInventoryRegularUnitsT4 { get { return _init_SumStoreInventoryRegularUnitsT4; } }
		public FormulaProfile Init_SumCompStoreInventoryRegularUnitsT4 { get { return _init_SumCompStoreInventoryRegularUnitsT4; } }
		public FormulaProfile Init_SumNonCompStoreInventoryRegularUnitsT4 { get { return _init_SumNonCompStoreInventoryRegularUnitsT4; } }
		public FormulaProfile Init_SumNewStoreInventoryRegularUnitsT4 { get { return _init_SumNewStoreInventoryRegularUnitsT4; } }
		public FormulaProfile Init_SumStoreInventoryRegularUnitsT5 { get { return _init_SumStoreInventoryRegularUnitsT5; } }
		public FormulaProfile Init_SumCompStoreInventoryRegularUnitsT5 { get { return _init_SumCompStoreInventoryRegularUnitsT5; } }
		public FormulaProfile Init_SumNonCompStoreInventoryRegularUnitsT5 { get { return _init_SumNonCompStoreInventoryRegularUnitsT5; } }
		public FormulaProfile Init_SumNewStoreInventoryRegularUnitsT5 { get { return _init_SumNewStoreInventoryRegularUnitsT5; } }
		public FormulaProfile Init_SumStoreInventoryRegularUnitsT6 { get { return _init_SumStoreInventoryRegularUnitsT6; } }
		public FormulaProfile Init_SumCompStoreInventoryRegularUnitsT6 { get { return _init_SumCompStoreInventoryRegularUnitsT6; } }
		public FormulaProfile Init_SumNonCompStoreInventoryRegularUnitsT6 { get { return _init_SumNonCompStoreInventoryRegularUnitsT6; } }
		public FormulaProfile Init_SumNewStoreInventoryRegularUnitsT6 { get { return _init_SumNewStoreInventoryRegularUnitsT6; } }
		public FormulaProfile Init_SumStoreInventoryRegularUnitsT7 { get { return _init_SumStoreInventoryRegularUnitsT7; } }
		public FormulaProfile Init_SumCompStoreInventoryRegularUnitsT7 { get { return _init_SumCompStoreInventoryRegularUnitsT7; } }
		public FormulaProfile Init_SumNonCompStoreInventoryRegularUnitsT7 { get { return _init_SumNonCompStoreInventoryRegularUnitsT7; } }
		public FormulaProfile Init_SumNewStoreInventoryRegularUnitsT7 { get { return _init_SumNewStoreInventoryRegularUnitsT7; } }
		// Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
        public FormulaProfile Init_SumStoreInventoryRegularUnitsT8 { get { return _init_SumStoreInventoryRegularUnitsT8; } }
        public FormulaProfile Init_SumCompStoreInventoryRegularUnitsT8 { get { return _init_SumCompStoreInventoryRegularUnitsT8; } }
        public FormulaProfile Init_SumNonCompStoreInventoryRegularUnitsT8 { get { return _init_SumNonCompStoreInventoryRegularUnitsT8; } }
        public FormulaProfile Init_SumNewStoreInventoryRegularUnitsT8 { get { return _init_SumNewStoreInventoryRegularUnitsT8; } }
		// End TT#2054
		public FormulaProfile Init_AvgStoreSalesTotalUnits { get { return _init_AvgStoreSalesTotalUnits; } }
		public FormulaProfile Init_AvgStoreSalesRegularUnits { get { return _init_AvgStoreSalesRegularUnits; } }
		public FormulaProfile Init_AvgStoreSalesMarkdownUnits { get { return _init_AvgStoreSalesMarkdownUnits; } }
		public FormulaProfile Init_AvgStoreInventoryTotalUnits { get { return _init_AvgStoreInventoryTotalUnits; } }
		public FormulaProfile Init_AvgStoreInventoryRegularUnits { get { return _init_AvgStoreInventoryRegularUnits; } }
		public FormulaProfile Init_AvgStoreInventoryMarkdownUnits { get { return _init_AvgStoreInventoryMarkdownUnits; } }

		public FormulaProfile Init_AvgStoreSalesTotalUnits_LowLevelTotal { get { return _init_AvgStoreSalesTotalUnits_LowLevelTotal; } }
		public FormulaProfile Init_AvgStoreSalesRegularUnits_LowLevelTotal { get { return _init_AvgStoreSalesRegularUnits_LowLevelTotal; } }
		public FormulaProfile Init_AvgStoreSalesMarkdownUnits_LowLevelTotal { get { return _init_AvgStoreSalesMarkdownUnits_LowLevelTotal; } }
		public FormulaProfile Init_AvgStoreInventoryTotalUnits_LowLevelTotal { get { return _init_AvgStoreInventoryTotalUnits_LowLevelTotal; } }
		public FormulaProfile Init_AvgStoreInventoryRegularUnits_LowLevelTotal { get { return _init_AvgStoreInventoryRegularUnits_LowLevelTotal; } }
		public FormulaProfile Init_AvgStoreInventoryMarkdownUnits_LowLevelTotal { get { return _init_AvgStoreInventoryMarkdownUnits_LowLevelTotal; } }

		public FormulaProfile Init_AvgStoreSalesTotalUnitsT3 { get { return _init_AvgStoreSalesTotalUnitsT3; } }
		public FormulaProfile Init_AvgStoreSalesRegularUnitsT2 { get { return _init_AvgStoreSalesRegularUnitsT2; } }
		public FormulaProfile Init_AvgStoreSalesPromoUnitsT2 { get { return _init_AvgStoreSalesPromoUnitsT2; } }
		public FormulaProfile Init_AvgStoreSalesRegPromoUnitsT3 { get { return _init_AvgStoreSalesRegPromoUnitsT3; } }
		public FormulaProfile Init_AvgStoreSalesMarkdownUnitsT3 { get { return _init_AvgStoreSalesMarkdownUnitsT3; } }

		public FormulaProfile Init_AvgStoreInventoryTotalUnitsT4 { get { return _init_AvgStoreInventoryTotalUnitsT4; } }
		public FormulaProfile Init_AvgStoreInventoryTotalUnitsT5 { get { return _init_AvgStoreInventoryTotalUnitsT5; } }
		public FormulaProfile Init_AvgStoreInventoryTotalUnitsT6 { get { return _init_AvgStoreInventoryTotalUnitsT6; } }
        public FormulaProfile Init_AvgStoreInventoryTotalUnitsT8 { get { return _init_AvgStoreInventoryTotalUnitsT8; } }		// TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
		public FormulaProfile Init_AvgStoreInventoryRegularUnitsT4 { get { return _init_AvgStoreInventoryRegularUnitsT4; } }
		public FormulaProfile Init_AvgStoreInventoryRegularUnitsT5 { get { return _init_AvgStoreInventoryRegularUnitsT5; } }
		public FormulaProfile Init_AvgStoreInventoryRegularUnitsT6 { get { return _init_AvgStoreInventoryRegularUnitsT6; } }
        public FormulaProfile Init_AvgStoreInventoryRegularUnitsT8 { get { return _init_AvgStoreInventoryRegularUnitsT8; } }	// TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Period Inits
		//-------------------------------------------

		public FormulaProfile Init_SumWeekDetail { get { return _init_SumWeekDetail; } }

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Low-level Inits
		//-------------------------------------------

		public FormulaProfile Init_SumLowLevelTotalDetail { get { return _init_SumLowLevelTotalDetail; } }
		public FormulaProfile Init_SumLowLevelRegularDetail { get { return _init_SumLowLevelRegularDetail; } }

		public FormulaProfile Init_SumLowLevelInventoryTotalUnitsT7 { get { return _init_SumLowLevelInventoryTotalUnitsT7; } }
		public FormulaProfile Init_SumLowLevelInventoryRegularUnitsT7 { get { return _init_SumLowLevelInventoryRegularUnitsT7; } }
		public FormulaProfile Init_SumLowLevelStoreInventoryTotalUnitsT7 { get { return _init_SumLowLevelStoreInventoryTotalUnitsT7; } }
		public FormulaProfile Init_SumLowLevelStoreInventoryRegularUnitsT7 { get { return _init_SumLowLevelStoreInventoryRegularUnitsT7; } }

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Comparative Inits
		//-------------------------------------------

		public FormulaProfile Init_PctChange { get { return _init_PctChange; } }
		public FormulaProfile Init_PctToSet { get { return _init_PctToSet; } }
		public FormulaProfile Init_PctToAll { get { return _init_PctToAll; } }
		public FormulaProfile Init_PctToLowLevel { get { return _init_PctToLowLevel; } }
		public FormulaProfile Init_PctToTimePeriod { get { return _init_PctToTimePeriod; } }
		public FormulaProfile Init_ChainBalance { get { return _init_ChainBalance; } }
		public FormulaProfile Init_StoreBalance { get { return _init_StoreBalance; } }
		public FormulaProfile Init_Difference { get { return _init_Difference; } }
		public FormulaProfile Init_PctChangeToPlan { get { return _init_PctChangeToPlan; } }

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Change Formulas
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Time Total Change Formulas
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//========
		// METHODS
		//========

		//========
		// CLASSES
		//========

		//-------------------------------------------
		#region Base Classes
		//-------------------------------------------

		abstract protected class PlanFormulaProfile : FormulaProfile
		{
			//=======
			// FIELDS
			//=======

			BasePlanComputations _basePlanComputations;

			//=============
			// CONSTRUCTORS
			//=============

			public PlanFormulaProfile(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aKey, aName)
			{
				_basePlanComputations = aBasePlanComputations;
			}

			//===========
			// PROPERTIES
			//===========

			protected BasePlanComputations BasePlanComputations
			{
				get
				{
					return _basePlanComputations;
				}
			}

			protected BasePlanVariables BasePlanVariables
			{
				get
				{
					return _basePlanComputations.BasePlanVariables;
				}
			}

			protected BasePlanQuantityVariables BasePlanQuantityVariables
			{
				get
				{
					return _basePlanComputations.BasePlanQuantityVariables;
				}
			}

			protected BasePlanTimeTotalVariables BasePlanTimeTotalVariables
			{
				get
				{
					return _basePlanComputations.BasePlanTimeTotalVariables;
				}
			}

			protected BasePlanToolBox BasePlanToolBox
			{
				get
				{
					return _basePlanComputations.BasePlanToolBox;
				}
			}

			//========
			// METHODS
			//========

			override public eComputationFormulaReturnType ExecuteCalc(ComputationScheduleEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, string aCaller)
			{
				PlanScheduleFormulaEntry schedEntry;

				try
				{
                    //return Execute((PlanScheduleFormulaEntry)aSchdEntry, aGetCellMode, aSetCellMode);
                    eComputationFormulaReturnType returnType = Execute((PlanScheduleFormulaEntry)aSchdEntry, aGetCellMode, aSetCellMode);
                    if (aSchdEntry.ComputationFormulaReturnType == eComputationFormulaReturnType.Pending)
                    {
                        ComputationCellReference lastPendingCell = aSchdEntry.LastPendingCell;
                        schedEntry = (PlanScheduleFormulaEntry)aSchdEntry;
                        schedEntry.LastPendingCell = lastPendingCell;
                        return eComputationFormulaReturnType.Pending;
                    }

                    return returnType;
				}
				catch (CellPendingException exc)
				{
					schedEntry = (PlanScheduleFormulaEntry)aSchdEntry;
					schedEntry.LastPendingCell = exc.ComputationCellReference;

					return eComputationFormulaReturnType.Pending;
				}
				catch (Exception exc)
				{
					throw;
				}
			}

			abstract public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode);
		}

		abstract protected class PlanSpreadProfile : SpreadProfile
		{
			//=======
			// FIELDS
			//=======

			BasePlanComputations _basePlanComputations;

			//=============
			// CONSTRUCTORS
			//=============

			public PlanSpreadProfile(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aKey, aName)
			{
				_basePlanComputations = aBasePlanComputations;
			}

			//===========
			// PROPERTIES
			//===========

			protected BasePlanComputations BasePlanComputations
			{
				get
				{
					return _basePlanComputations;
				}
			}

			protected BasePlanVariables BasePlanVariables
			{
				get
				{
					return _basePlanComputations.BasePlanVariables;
				}
			}

			protected BasePlanQuantityVariables BasePlanQuantityVariables
			{
				get
				{
					return _basePlanComputations.BasePlanQuantityVariables;
				}
			}

			protected BasePlanTimeTotalVariables BasePlanTimeTotalVariables
			{
				get
				{
					return _basePlanComputations.BasePlanTimeTotalVariables;
				}
			}

			protected BasePlanToolBox BasePlanToolBox
			{
				get
				{
					return _basePlanComputations.BasePlanToolBox;
				}
			}

			//========
			// METHODS
			//========

			override public eComputationFormulaReturnType ExecuteCalc(ComputationScheduleEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, string aCaller)
			{
				PlanScheduleSpreadEntry schedEntry;

				try
				{
                    //return Execute((PlanScheduleSpreadEntry)aSchdEntry, aGetCellMode, aSetCellMode);
                    eComputationFormulaReturnType returnType = Execute((PlanScheduleSpreadEntry)aSchdEntry, aGetCellMode, aSetCellMode);
                    if (aSchdEntry.ComputationFormulaReturnType == eComputationFormulaReturnType.Pending)
                    {
                        ComputationCellReference lastPendingCell = aSchdEntry.LastPendingCell;
                        schedEntry = (PlanScheduleSpreadEntry)aSchdEntry;
                        schedEntry.LastPendingCell = lastPendingCell;
                        return eComputationFormulaReturnType.Pending;
                    }

                    return returnType;
				}
				catch (CellPendingException exc)
				{
					schedEntry = (PlanScheduleSpreadEntry)aSchdEntry;
					schedEntry.LastPendingCell = exc.ComputationCellReference;

					return eComputationFormulaReturnType.Pending;
				}
				catch (Exception exc)
				{
					throw;
				}
			}

			override public ArrayList GetSpreadToCellReferenceList(ComputationCellReference aCompCellRef)
			{
				try
				{
					return GetSpreadToCellReferenceList((PlanCellReference)aCompCellRef);
				}
				catch (Exception exc)
				{
					throw;
				}
			}

			abstract public eComputationFormulaReturnType Execute(PlanScheduleSpreadEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode);

			abstract public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef);
		}

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Base/Abstract Formulas
		//-------------------------------------------

		protected class clsFormula_Sum : PlanFormulaProfile
		{
			public clsFormula_Sum(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, BasePlanToolBox.SumDetailComponents(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef));

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsFormula_SumHidden : PlanFormulaProfile
		{
			public clsFormula_SumHidden(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, BasePlanToolBox.SumDetailComponents(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, true));

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsFormula_SumComp : PlanFormulaProfile
		{
			public clsFormula_SumComp(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, BasePlanToolBox.SumDetailComponents(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, eStoreStatus.Comp));

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsFormula_SumNonComp : PlanFormulaProfile
		{
			public clsFormula_SumNonComp(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, BasePlanToolBox.SumDetailComponents(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, eStoreStatus.NonComp));

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsFormula_SumNew : PlanFormulaProfile
		{
			public clsFormula_SumNew(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, BasePlanToolBox.SumDetailComponents(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, eStoreStatus.New));

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsFormula_Average : PlanFormulaProfile
		{
			public clsFormula_Average(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, BasePlanToolBox.CalculateAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, true));

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsFormula_AverageHidden : PlanFormulaProfile
		{
			public clsFormula_AverageHidden(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, BasePlanToolBox.CalculateAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, true));

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsFormula_AverageValue : PlanFormulaProfile
		{
			public clsFormula_AverageValue(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, BasePlanToolBox.CalculateAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value));

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsFormula_TimeAverage : PlanFormulaProfile
		{
			public clsFormula_TimeAverage(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;
				VariableProfile varProf;

				try
				{
					varProf = (VariableProfile)BasePlanVariables.VariableProfileList.FindKey(aSchdEntry.PlanCellRef[eProfileType.Variable]);
					newValue = BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, varProf.TotalTimeTotalVariableProfile, aSchdEntry.PlanCellRef.isCellHidden);
					BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue / BasePlanToolBox.GetDetailCellCount(aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden));

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		abstract protected class clsFormula_SumAlternateDetail : PlanFormulaProfile
		{
			public clsFormula_SumAlternateDetail(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			abstract protected eCubeType CubeType { get; }
			abstract protected bool isTypeOK(PlanCellReference aPlanCellRef);
			abstract protected bool isStatusOK(PlanCellReference aPlanCellRef);

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				ArrayList cellRefArray;
				double newValue;

				try
				{
					if (isTypeOK(aSchdEntry.PlanCellRef))
					{
						cellRefArray = BasePlanToolBox.GetDetailCellRefArray(aSchdEntry.PlanCellRef, CubeType, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden);
						newValue = 0;

						foreach (PlanCellReference planCellRef in cellRefArray)
						{
							if (isStatusOK(planCellRef))
							{
								newValue += BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, planCellRef, true, planCellRef.isCellHidden);
							}
						}

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		abstract protected class clsFormula_SumAlternateDetail_LowLevelTotal : PlanFormulaProfile
		{
			public clsFormula_SumAlternateDetail_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			abstract protected eCubeType CubeType { get; }
			abstract protected bool isTypeOK(PlanCellReference aPlanCellRef);
			abstract protected eStoreStatus StoreStatus { get; }

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					if (isTypeOK(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, BasePlanToolBox.SumDetailComponents(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, CubeType, StoreStatus, BasePlanQuantityVariables.Value));
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		abstract protected class clsFormula_AverageAlternateDetail : PlanFormulaProfile
		{
			public clsFormula_AverageAlternateDetail(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			abstract protected eCubeType CubeType { get; }
			abstract protected bool isTypeOK(PlanCellReference aPlanCellRef);
			abstract protected bool isStatusOK(PlanCellReference aPlanCellRef);

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				ArrayList cellRefArray;
				double newValue;
				int count;

				try
				{
					if (isTypeOK(aSchdEntry.PlanCellRef))
					{
						cellRefArray = BasePlanToolBox.GetDetailCellRefArray(aSchdEntry.PlanCellRef, CubeType, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden);
						newValue = 0;
						count = 0;

						foreach (PlanCellReference planCellRef in cellRefArray)
						{
							if (isStatusOK(planCellRef))
							{
								newValue += BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, planCellRef, true, planCellRef.isCellHidden);
								count++;
							}
						}

						if (count > 0)
						{
							BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, (double)(decimal)(newValue / count));
						}
						else
						{
							BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, 0);
						}
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		abstract protected class clsFormula_AverageAlternateDetail_LowLevelTotal : PlanFormulaProfile
		{
			public clsFormula_AverageAlternateDetail_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			abstract protected eCubeType CubeType { get; }
			abstract protected VariableProfile InventoryVariableProfile { get; }
			abstract protected bool CalculateNonZeroAverage(PlanCellReference aPlanCellRef);
			abstract protected bool isTypeOK(PlanCellReference aPlanCellRef);
			abstract protected eStoreStatus StoreStatus { get; }

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					if (isTypeOK(aSchdEntry.PlanCellRef))
					{
						if (CalculateNonZeroAverage(aSchdEntry.PlanCellRef))
						{
							BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, BasePlanToolBox.CalculateNonZeroAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, CubeType, StoreStatus, BasePlanQuantityVariables.Value, InventoryVariableProfile));
						}
						else
						{
							BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, BasePlanToolBox.CalculateAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, CubeType, StoreStatus, BasePlanQuantityVariables.Value));
						}
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		abstract protected class clsFormula_SumStoreNeedFill : clsFormula_SumAlternateDetail
		{
			public clsFormula_SumStoreNeedFill(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			abstract protected eStoreStatus StoreStatus { get; }

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				eStoreStatus storeStat;
				ProfileList timeList;
				PeriodProfile perProf;

				try
				{
					storeStat = StoreStatus;

					if (storeStat != eStoreStatus.None)
					{
						timeList = aPlanCellRef.PlanCube.PlanCubeGroup.OpenParms.GetDetailDateProfileList(aPlanCellRef.PlanCube.SAB.ApplicationServerSession);

						if (timeList.ProfileType == eProfileType.Week)
						{
							return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(aPlanCellRef[eProfileType.Store], timeList[timeList.Count - 1].Key) == storeStat;
						}
						else
						{
							perProf = (PeriodProfile)timeList[timeList.Count - 1];
					
							return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(aPlanCellRef[eProfileType.Store], perProf.Weeks[0].Key) == storeStat;
						}
					}
					else
					{
						return true;
					}
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		abstract protected class clsFormula_SumStorePctNeed : PlanFormulaProfile
		{
			public clsFormula_SumStorePctNeed(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			abstract protected bool isTypeOK(PlanCellReference aPlanCellRef);
			abstract protected eStoreStatus StoreStatus { get; }
			abstract protected TimeTotalVariableProfile NeedVariable { get; }

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				ArrayList storeRIDList;
				ArrayList cellRefArray;
				double newValue;

				try
				{
					if (isTypeOK(aSchdEntry.PlanCellRef))
					{
						storeRIDList = new ArrayList();

						cellRefArray = BasePlanToolBox.GetDetailCellRefArray(aSchdEntry.PlanCellRef, eCubeType.StoreDetail, aSchdEntry.PlanCellRef.isCellHidden);

						foreach (PlanCellReference planCellRef in cellRefArray)
						{
							if (isStatusOK(planCellRef))
							{
								storeRIDList.Add(planCellRef[eProfileType.Store]);
							}
						}

						newValue = BasePlanToolBox.GetPctNeedCellValue(
							aSchdEntry,
							aGetCellMode,
							aSetCellMode,
							aSchdEntry.PlanCellRef,
							(int[])storeRIDList.ToArray(typeof(int)),
							NeedVariable,
							aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}

			private bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				eStoreStatus storeStat;
				ProfileList timeList;
				PeriodProfile perProf;

				try
				{
					storeStat = StoreStatus;

					if (storeStat != eStoreStatus.None)
					{
						timeList = aPlanCellRef.PlanCube.PlanCubeGroup.OpenParms.GetDetailDateProfileList(aPlanCellRef.PlanCube.SAB.ApplicationServerSession);

						if (timeList.ProfileType == eProfileType.Week)
						{
							return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(aPlanCellRef[eProfileType.Store], timeList[timeList.Count - 1].Key) == storeStat;
						}
						else
						{
							perProf = (PeriodProfile)timeList[timeList.Count - 1];
					
							return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(aPlanCellRef[eProfileType.Store], perProf.Weeks[0].Key) == storeStat;
						}
					}
					else
					{
						return true;
					}
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		abstract protected class clsFormula_SumLowLevelPctNeed : PlanFormulaProfile
		{
			public clsFormula_SumLowLevelPctNeed(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			abstract protected bool isTypeOK(PlanCellReference aPlanCellRef);
			abstract protected TimeTotalVariableProfile NeedVariable { get; }

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				int[] storeRIDArr;
				ArrayList cellRefArray;
				double newValue;

				try
				{
					if (isTypeOK(aSchdEntry.PlanCellRef))
					{
						storeRIDArr = new int[1];

						cellRefArray = BasePlanToolBox.GetDetailCellRefArray(aSchdEntry.PlanCellRef, eCubeType.LowLevelDetail, aSchdEntry.PlanCellRef.isCellHidden);

						storeRIDArr[0] = aSchdEntry.PlanCellRef[eProfileType.Store];

						newValue = BasePlanToolBox.GetPctNeedCellValue(
							aSchdEntry,
							aGetCellMode,
							aSetCellMode,
							aSchdEntry.PlanCellRef,
							cellRefArray,
							storeRIDArr,
							NeedVariable,
							aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		abstract protected class clsFormula_SumLowLevelStorePctNeed : PlanFormulaProfile
		{
			public clsFormula_SumLowLevelStorePctNeed(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			abstract protected bool isTypeOK(PlanCellReference aPlanCellRef);
			abstract protected TimeTotalVariableProfile NeedVariable { get; }

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				ArrayList storeRIDList;
				ArrayList lowLvlCellRefArray;
				ArrayList cellRefArray;
				double newValue;

				try
				{
					if (isTypeOK(aSchdEntry.PlanCellRef))
					{
						storeRIDList = new ArrayList();

						lowLvlCellRefArray = BasePlanToolBox.GetDetailCellRefArray(aSchdEntry.PlanCellRef, eCubeType.LowLevelDetail, aSchdEntry.PlanCellRef.isCellHidden);
						cellRefArray = BasePlanToolBox.GetDetailCellRefArray(aSchdEntry.PlanCellRef, eCubeType.StoreDetail, aSchdEntry.PlanCellRef.isCellHidden);

						foreach (PlanCellReference planCellRef in cellRefArray)
						{
							storeRIDList.Add(planCellRef[eProfileType.Store]);
						}

						newValue = BasePlanToolBox.GetPctNeedCellValue(
							aSchdEntry,
							aGetCellMode,
							aSetCellMode,
							aSchdEntry.PlanCellRef,
							lowLvlCellRefArray,
							(int[])storeRIDList.ToArray(typeof(int)),
							NeedVariable,
							aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		abstract protected class clsFormula_SetAllIndex : PlanFormulaProfile
		{
			private VariableProfile varProf;

			public clsFormula_SetAllIndex(BasePlanComputations aBasePlanComputations, int aKey, string aName, VariableProfile aVariableProfile)
				: base(aBasePlanComputations, aKey, aName)
			{
				varProf = aVariableProfile;
			}

			abstract public bool isDisplayed(PlanCellReference aPlanCellRef);

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (isDisplayed(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, varProf, BasePlanQuantityVariables.StoreAverage, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, varProf, BasePlanQuantityVariables.StoreAverage, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		abstract protected class clsFormula_SetAllIndexTT : PlanFormulaProfile
		{
			private TimeTotalVariableProfile timeTotVarProf;

			public clsFormula_SetAllIndexTT(BasePlanComputations aBasePlanComputations, int aKey, string aName, TimeTotalVariableProfile aTimeTotalVariableProfile)
				: base(aBasePlanComputations, aKey, aName)
			{
				timeTotVarProf = aTimeTotalVariableProfile;
			}

			abstract public bool isDisplayed(PlanCellReference aPlanCellRef);

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (isDisplayed(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.StoreAverage, timeTotVarProf, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.StoreAverage, timeTotVarProf, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Abstract Spreads
		//-------------------------------------------

		abstract protected class clsSpread_BasePctContribution : PlanSpreadProfile
		{
			public clsSpread_BasePctContribution(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleSpreadEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double spreadValue;

				try
				{
					spreadValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.PlanCellRef.isCellHidden);
//					BasePlanToolBox.CheckSpreadCellsForPending(aSchdEntry);

					BasePlanToolBox.ExecutePctContributionSpread(
						aSchdEntry,
						aSetCellMode,
						aSchdEntry.SpreadCellRefList,
						aSchdEntry.PlanCellRef.GetFormatTypeVariableProfile().NumDecimals,
						spreadValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetSpreadDetailCellRefArray(aPlanCellRef, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		abstract protected class clsSpread_BasePctContributionCascasde : clsSpread_BasePctContribution
		{
			public clsSpread_BasePctContributionCascasde(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			override public bool CascadeChangeMethods
			{
				get
				{
					return true;
				}
			}
		}

		abstract protected class clsSpread_BasePctContributionNoCascasde : clsSpread_BasePctContribution
		{
			public clsSpread_BasePctContributionNoCascasde(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			override public bool CascadeChangeMethods
			{
				get
				{
					return false;
				}
			}
		}

		abstract protected class clsSpread_BaseTotal : PlanSpreadProfile
		{
			public clsSpread_BaseTotal(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleSpreadEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double oldValue = 0;
				double newValue = 0;

				try
				{
					switch (aSchdEntry.PlanCellRef.GetCalcVariableProfile().VariableSpreadType)
					{
						case eVariableSpreadType.PctChange :

							oldValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, eGetCellMode.Previous, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.PlanCellRef.isCellHidden);
							newValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, eGetCellMode.Current, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.PlanCellRef.isCellHidden);
							break;

						case eVariableSpreadType.PctContribution :

							newValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.PlanCellRef.isCellHidden);
							break;

						case eVariableSpreadType.Plug :

							newValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.PlanCellRef.isCellHidden);
							break;
					}

//					BasePlanToolBox.CheckSpreadCellsForPending(aSchdEntry);

					switch (aSchdEntry.PlanCellRef.GetCalcVariableProfile().VariableSpreadType)
					{
						case eVariableSpreadType.PctChange :

							BasePlanToolBox.ExecutePctChangeSpread(
								aSchdEntry,
								aSetCellMode,
								aSchdEntry.SpreadCellRefList,
								aSchdEntry.PlanCellRef.GetFormatTypeVariableProfile().NumDecimals,
								oldValue,
								newValue);

							break;

						case eVariableSpreadType.PctContribution :

							BasePlanToolBox.ExecutePctContributionSpread(
								aSchdEntry,
								aSetCellMode,
								aSchdEntry.SpreadCellRefList,
								aSchdEntry.PlanCellRef.GetFormatTypeVariableProfile().NumDecimals,
								newValue);

							break;

						case eVariableSpreadType.Plug :

							BasePlanToolBox.ExecutePlugSpread(
								aSchdEntry,
								aSetCellMode,
								aSchdEntry.SpreadCellRefList,
								aSchdEntry.PlanCellRef.GetFormatTypeVariableProfile().NumDecimals,
								newValue);

							break;
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetSpreadDetailCellRefArray(aPlanCellRef, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		abstract protected class clsSpread_BaseTotalCascade : clsSpread_BaseTotal
		{
			public clsSpread_BaseTotalCascade(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			override public bool CascadeChangeMethods
			{
				get
				{
					return true;
				}
			}
		}

		abstract protected class clsSpread_BaseTotalNoCascade : clsSpread_BaseTotal
		{
			public clsSpread_BaseTotalNoCascade(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			override public bool CascadeChangeMethods
			{
				get
				{
					return false;
				}
			}
		}

		abstract protected class clsSpread_BaseAverage : PlanSpreadProfile
		{
			public clsSpread_BaseAverage(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}
			
			override public bool CascadeChangeMethods
			{
				get
				{
					return true;
				}
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleSpreadEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double averageValue;
				double spreadValue;
				PlanCellReference valCellRef;

				try
				{
					valCellRef = (PlanCellReference)aSchdEntry.PlanCellRef.Copy();
					valCellRef[eProfileType.QuantityVariable] = BasePlanQuantityVariables.Value.Key;

					averageValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.PlanCellRef.isCellHidden);
					spreadValue = averageValue * GetSpreadDetailCount(aSchdEntry, aGetCellMode, aSetCellMode);

					BasePlanToolBox.ExecutePctContributionSpread(
						aSchdEntry,
						aSetCellMode,
						aSchdEntry.SpreadCellRefList,
						valCellRef.GetFormatTypeVariableProfile().NumDecimals,
						spreadValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}

			}

			abstract public int GetSpreadDetailCount(PlanScheduleSpreadEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode);
		}

		abstract protected class clsSpread_AverageToQuantityValue : clsSpread_BaseAverage
		{
			public clsSpread_AverageToQuantityValue(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetSpreadDetailCellRefArray(aPlanCellRef, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}

			override public int GetSpreadDetailCount(PlanScheduleSpreadEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					return BasePlanToolBox.GetDetailCellCount(aSchdEntry.PlanCellRef, aSchdEntry.PlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		abstract protected class clsSpread_Average : clsSpread_BaseAverage
		{
			public clsSpread_Average(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetSpreadDetailCellRefArray(aPlanCellRef, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}

			override public int GetSpreadDetailCount(PlanScheduleSpreadEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					return BasePlanToolBox.GetDetailCellCount(aSchdEntry.PlanCellRef, aSchdEntry.PlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Spreads
		//-------------------------------------------

		protected class clsSpread_Set : clsSpread_BaseTotalCascade
		{
			public clsSpread_Set(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "Set Spread")
			{
			}
		}

		protected class clsSpread_SetAverage : clsSpread_AverageToQuantityValue
		{
			public clsSpread_SetAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "Set Average Spread")
			{
			}
		}

		protected class clsSpread_SetComp : clsSpread_BaseTotalCascade
		{
			public clsSpread_SetComp(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "Set Comp Spread")
			{
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetSpreadDetailCellRefArray(aPlanCellRef, eStoreStatus.Comp, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsSpread_SetNonComp : clsSpread_BaseTotalCascade
		{
			public clsSpread_SetNonComp(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "Set Non-Comp Spread")
			{
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetSpreadDetailCellRefArray(aPlanCellRef, eStoreStatus.NonComp, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsSpread_SetNew : clsSpread_BaseTotalCascade
		{
			public clsSpread_SetNew(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "Set New Spread")
			{
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetSpreadDetailCellRefArray(aPlanCellRef, eStoreStatus.New, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsSpread_All : clsSpread_BaseTotalCascade
		{
			public clsSpread_All(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "All Store Spread")
			{
			}
		}

		protected class clsSpread_AllAverage : clsSpread_AverageToQuantityValue
		{
			public clsSpread_AllAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "All Average Spread")
			{
			}
		}

		protected class clsSpread_AllComp : clsSpread_BaseTotalCascade
		{
			public clsSpread_AllComp(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "All Store Comp Spread")
			{
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetSpreadDetailCellRefArray(aPlanCellRef, eStoreStatus.Comp, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsSpread_AllNonComp : clsSpread_BaseTotalCascade
		{
			public clsSpread_AllNonComp(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "All Store Non-Comp Spread")
			{
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetSpreadDetailCellRefArray(aPlanCellRef, eStoreStatus.NonComp, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsSpread_AllNew : clsSpread_BaseTotalCascade
		{
			public clsSpread_AllNew(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "All Store New Spread")
			{
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetSpreadDetailCellRefArray(aPlanCellRef, eStoreStatus.New, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsSpread_Period : clsSpread_BaseTotalCascade
		{
			public clsSpread_Period(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "Period Spread")
			{
			}
		}

		protected class clsSpread_PeriodAverage : clsSpread_AverageToQuantityValue
		{
			public clsSpread_PeriodAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "Period Average Spread")
			{
			}
		}

		protected class clsSpread_Date : clsSpread_BaseTotalCascade
		{
			public clsSpread_Date(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "Date Spread")
			{
			}
		}

		protected class clsSpread_DateAverage : clsSpread_Average
		{
			public clsSpread_DateAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "Date Average Spread")
			{
			}
		}

		protected class clsSpread_LowLevel : clsSpread_BaseTotalCascade
		{
			public clsSpread_LowLevel(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "Low-level Spread")
			{
			}
		}

		protected class clsSpread_LowLevelAverage : clsSpread_AverageToQuantityValue
		{
			public clsSpread_LowLevelAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "Low-level Average Spread")
			{
			}
		}

		protected class clsSpread_PeriodNoCascade : clsSpread_BaseTotalNoCascade
		{
			public clsSpread_PeriodNoCascade(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "PeriodNoCascade Spread")
			{
			}
		}

		protected class clsSpread_TotalPeriodToWeeks : clsSpread_BaseTotalCascade
		{
			public clsSpread_TotalPeriodToWeeks(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "PeriodNoCascade Spread")
			{
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetDetailCellRefArray(aPlanCellRef, eCubeType.WeekDetail, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsSpread_SalesTotalUnitsAverage : PlanSpreadProfile
		{
			public clsSpread_SalesTotalUnitsAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesTotalUnitsAverage Spread")
			{
			}

			public override bool CascadeChangeMethods
			{
				get
				{
					return true;
				}
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetSpreadDetailCellRefArray(aPlanCellRef, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleSpreadEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				int storeCount;
				double averageSales;
				double spreadValue;
				PlanCellReference valCellRef;

				try
				{
					valCellRef = (PlanCellReference)aSchdEntry.PlanCellRef.Copy();
					valCellRef[eProfileType.QuantityVariable] = BasePlanQuantityVariables.Value.Key;

					averageSales = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.PlanCellRef.isCellHidden);

					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.CalculateNonZeroAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, BasePlanVariables.InventoryTotalUnits, out storeCount);
					}
					else
					{
						BasePlanToolBox.CalculateAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, out storeCount);
					}

					spreadValue = averageSales * storeCount;

					BasePlanToolBox.ExecutePctContributionSpread(
						aSchdEntry,
						aSetCellMode,
						aSchdEntry.SpreadCellRefList,
						valCellRef.GetFormatTypeVariableProfile().NumDecimals,
						spreadValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsSpread_SalesRegularUnitsAverage : PlanSpreadProfile
		{
			public clsSpread_SalesRegularUnitsAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegularUnitsAverage Spread")
			{
			}

			public override bool CascadeChangeMethods
			{
				get
				{
					return true;
				}
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetSpreadDetailCellRefArray(aPlanCellRef, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleSpreadEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				int storeCount;
				double averageSales;
				double spreadValue;
				PlanCellReference valCellRef;

				try
				{
					valCellRef = (PlanCellReference) aSchdEntry.PlanCellRef.Copy();
					valCellRef[eProfileType.QuantityVariable] = BasePlanQuantityVariables.Value.Key;

					averageSales = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.PlanCellRef.isCellHidden);

					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.CalculateNonZeroAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, BasePlanVariables.InventoryRegularUnits, out storeCount);
					}
					else
					{
						BasePlanToolBox.CalculateAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, out storeCount);
					}

					spreadValue = averageSales * storeCount;

					BasePlanToolBox.ExecutePctContributionSpread(
						aSchdEntry,
						aSetCellMode,
						aSchdEntry.SpreadCellRefList,
						valCellRef.GetFormatTypeVariableProfile().NumDecimals,
						spreadValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsSpread_SalesPromoUnitsAverage : PlanSpreadProfile
		{
			public clsSpread_SalesPromoUnitsAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesPromoUnitsAverage Spread")
			{
			}

			public override bool CascadeChangeMethods
			{
				get
				{
					return true;
				}
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetSpreadDetailCellRefArray(aPlanCellRef, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleSpreadEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				int storeCount;
				double averageSales;
				double spreadValue;
				PlanCellReference valCellRef;

				try
				{
					valCellRef = (PlanCellReference)aSchdEntry.PlanCellRef.Copy();
					valCellRef[eProfileType.QuantityVariable] = BasePlanQuantityVariables.Value.Key;

					averageSales = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.PlanCellRef.isCellHidden);

					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.CalculateNonZeroAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, BasePlanVariables.InventoryRegularUnits, out storeCount);
					}
					else
					{
						BasePlanToolBox.CalculateAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, out storeCount);
					}

					spreadValue = averageSales * storeCount;

					BasePlanToolBox.ExecutePctContributionSpread(
						aSchdEntry,
						aSetCellMode,
						aSchdEntry.SpreadCellRefList,
						valCellRef.GetFormatTypeVariableProfile().NumDecimals,
						spreadValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsSpread_SalesRegPromoUnitsAverage : PlanSpreadProfile
		{
			public clsSpread_SalesRegPromoUnitsAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegPromoUnitsAverage Spread")
			{
			}

			public override bool CascadeChangeMethods
			{
				get
				{
					return true;
				}
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetSpreadDetailCellRefArray(aPlanCellRef, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleSpreadEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				int storeCount;
				double averageSales;
				double spreadValue;
				PlanCellReference valCellRef;

				try
				{
					valCellRef = (PlanCellReference)aSchdEntry.PlanCellRef.Copy();
					valCellRef[eProfileType.QuantityVariable] = BasePlanQuantityVariables.Value.Key;

					averageSales = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.PlanCellRef.isCellHidden);

					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.CalculateNonZeroAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, BasePlanVariables.InventoryRegularUnits, out storeCount);
					}
					else
					{
						BasePlanToolBox.CalculateAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, out storeCount);
					}

					spreadValue = averageSales * storeCount;

					BasePlanToolBox.ExecutePctContributionSpread(
						aSchdEntry,
						aSetCellMode,
						aSchdEntry.SpreadCellRefList,
						valCellRef.GetFormatTypeVariableProfile().NumDecimals,
						spreadValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsSpread_InventoryTotalUnitsAverage : PlanSpreadProfile
		{
			public clsSpread_InventoryTotalUnitsAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryTotalUnitsAverage Spread")
			{
			}

			public override bool CascadeChangeMethods
			{
				get
				{
					return true;
				}
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetSpreadDetailCellRefArray(aPlanCellRef, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleSpreadEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				int storeCount;
				double averageInventory;
				double spreadValue;
				PlanCellReference valCellRef;

				try
				{
					valCellRef = (PlanCellReference)aSchdEntry.PlanCellRef.Copy();
					valCellRef[eProfileType.QuantityVariable] = BasePlanQuantityVariables.Value.Key;

					averageInventory = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.PlanCellRef.isCellHidden);

					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.CalculateNonZeroAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, null, out storeCount);
					}
					else
					{
						BasePlanToolBox.CalculateAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, out storeCount);
					}

					spreadValue = averageInventory * storeCount;

					BasePlanToolBox.ExecutePctContributionSpread(
						aSchdEntry,
						aSetCellMode,
						aSchdEntry.SpreadCellRefList,
						valCellRef.GetFormatTypeVariableProfile().NumDecimals,
						spreadValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsSpread_InventoryRegularUnitsAverage : PlanSpreadProfile
		{
			public clsSpread_InventoryRegularUnitsAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryRegularUnitsAverage Spread")
			{
			}

			public override bool CascadeChangeMethods
			{
				get
				{
					return true;
				}
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetSpreadDetailCellRefArray(aPlanCellRef, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleSpreadEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				int storeCount;
				double averageInventory;
				double spreadValue;
				PlanCellReference valCellRef;

				try
				{
					valCellRef = (PlanCellReference)aSchdEntry.PlanCellRef.Copy();
					valCellRef[eProfileType.QuantityVariable] = BasePlanQuantityVariables.Value.Key;

					averageInventory = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.PlanCellRef.isCellHidden);

					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.CalculateNonZeroAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, null, out storeCount);
					}
					else
					{
						BasePlanToolBox.CalculateAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, out storeCount);
					}

					spreadValue = averageInventory * storeCount;

					BasePlanToolBox.ExecutePctContributionSpread(
						aSchdEntry,
						aSetCellMode,
						aSchdEntry.SpreadCellRefList,
						valCellRef.GetFormatTypeVariableProfile().NumDecimals,
						spreadValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsSpread_SalesTotalUnitsAverage_LowLevelTotal : clsSpread_BaseAverage
		{
			public clsSpread_SalesTotalUnitsAverage_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesTotalUnitsAverage_LowLevelTotal Spread")
			{
			}

			override public bool CascadeChangeMethods
			{
				get
				{
					return true;
				}
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetDetailCellRefArray(aPlanCellRef, eCubeType.StoreDetail, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}

			override public int GetSpreadDetailCount(PlanScheduleSpreadEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				int storeCount;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.CalculateNonZeroAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, BasePlanVariables.InventoryTotalUnits, out storeCount);
					}
					else
					{
						BasePlanToolBox.CalculateAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, out storeCount);
					}

					return storeCount;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsSpread_SalesRegularUnitsAverage_LowLevelTotal : clsSpread_BaseAverage
		{
			public clsSpread_SalesRegularUnitsAverage_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegularUnitsAverage_LowLevelTotal Spread")
			{
			}

			override public bool CascadeChangeMethods
			{
				get
				{
					return true;
				}
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetDetailCellRefArray(aPlanCellRef, eCubeType.StoreDetail, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}

			override public int GetSpreadDetailCount(PlanScheduleSpreadEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				int storeCount;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.CalculateNonZeroAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, BasePlanVariables.InventoryRegularUnits, out storeCount);
					}
					else
					{
						BasePlanToolBox.CalculateAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, out storeCount);
					}

					return storeCount;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsSpread_SalesPromoUnitsAverage_LowLevelTotal : clsSpread_BaseAverage
		{
			public clsSpread_SalesPromoUnitsAverage_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesPromoUnitsAverage_LowLevelTotal Spread")
			{
			}

			override public bool CascadeChangeMethods
			{
				get
				{
					return true;
				}
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetDetailCellRefArray(aPlanCellRef, eCubeType.StoreDetail, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}

			override public int GetSpreadDetailCount(PlanScheduleSpreadEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				int storeCount;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.CalculateNonZeroAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, BasePlanVariables.InventoryRegularUnits, out storeCount);
					}
					else
					{
						BasePlanToolBox.CalculateAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, out storeCount);
					}

					return storeCount;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsSpread_SalesRegPromoUnitsAverage_LowLevelTotal : clsSpread_BaseAverage
		{
			public clsSpread_SalesRegPromoUnitsAverage_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegPromoUnitsAverage_LowLevelTotal Spread")
			{
			}

			override public bool CascadeChangeMethods
			{
				get
				{
					return true;
				}
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetDetailCellRefArray(aPlanCellRef, eCubeType.StoreDetail, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}

			override public int GetSpreadDetailCount(PlanScheduleSpreadEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				int storeCount;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.CalculateNonZeroAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, BasePlanVariables.InventoryRegularUnits, out storeCount);
					}
					else
					{
						BasePlanToolBox.CalculateAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, out storeCount);
					}

					return storeCount;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsSpread_InventoryTotalUnitsAverage_LowLevelTotal : clsSpread_BaseAverage
		{
			public clsSpread_InventoryTotalUnitsAverage_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryTotalUnitsAverage_LowLevelTotal Spread")
			{
			}

			override public bool CascadeChangeMethods
			{
				get
				{
					return true;
				}
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetDetailCellRefArray(aPlanCellRef, eCubeType.StoreDetail, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}

			override public int GetSpreadDetailCount(PlanScheduleSpreadEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				int storeCount;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.CalculateNonZeroAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, null, out storeCount);
					}
					else
					{
						BasePlanToolBox.CalculateAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, out storeCount);
					}

					return storeCount;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsSpread_InventoryRegularUnitsAverage_LowLevelTotal : clsSpread_BaseAverage
		{
			public clsSpread_InventoryRegularUnitsAverage_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryRegularUnitsAverage_LowLevelTotal Spread")
			{
			}

			override public bool CascadeChangeMethods
			{
				get
				{
					return true;
				}
			}

			override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.GetDetailCellRefArray(aPlanCellRef, eCubeType.StoreDetail, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden);
				}
				catch (Exception exc)
				{
					throw;
				}
			}

			override public int GetSpreadDetailCount(PlanScheduleSpreadEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				int storeCount;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.CalculateNonZeroAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, null, out storeCount);
					}
					else
					{
						BasePlanToolBox.CalculateAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.SpreadCellRefList, out storeCount);
					}

					return storeCount;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

        // Begin TT#1722 - RMatelic - Open Sales R/P to be editable 
        protected class clsSpread_SalesRegPromoUnits : clsSpread_BaseTotalNoCascade
        {
            public clsSpread_SalesRegPromoUnits(BasePlanComputations aComputations, int aKey)
                : base(aComputations, aKey, "SalesRegPromoUnits Spread")
            {
            }

            override public ArrayList GetSpreadToCellReferenceList(PlanCellReference aPlanCellRef)
            {
                ArrayList cellRefArray;

                try
                {
                    cellRefArray = new ArrayList();
                    cellRefArray.Add(BasePlanToolBox.GetOperandCell(null, eSetCellMode.Computation, aPlanCellRef, ((PlanVariables)BasePlanVariables).SalesRegularUnits, false));
                    cellRefArray.Add(BasePlanToolBox.GetOperandCell(null, eSetCellMode.Computation, aPlanCellRef, ((PlanVariables)BasePlanVariables).SalesPromoUnits, false));

                    return cellRefArray;
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }
        // End TT#1722

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Inits
		//-------------------------------------------

		protected class clsInit_Null : PlanFormulaProfile
		{
			public clsInit_Null(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "Null Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}


		protected class clsInit_SalesTotalUnits : PlanFormulaProfile
		{
			public clsInit_SalesTotalUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesTotalUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesRegularUnits, aSchdEntry.PlanCellRef.isCellHidden) +
							BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesPromoUnits, aSchdEntry.PlanCellRef.isCellHidden) +
							BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesMarkdownUnits, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						if (!BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
						{
							BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
						}
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesRegularUnits : PlanFormulaProfile
		{
			public clsInit_SalesRegularUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegularUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					if (BasePlanToolBox.isForecast(aSchdEntry.PlanCellRef) && !BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesPromoUnits : PlanFormulaProfile
		{
			public clsInit_SalesPromoUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesPromoUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					if (BasePlanToolBox.isForecast(aSchdEntry.PlanCellRef) && !BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesRegPromoUnits : PlanFormulaProfile
		{
			public clsInit_SalesRegPromoUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegPromoUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesRegularUnits, aSchdEntry.PlanCellRef.isCellHidden) +
							BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesPromoUnits, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesMarkdownUnits : PlanFormulaProfile
		{
			public clsInit_SalesMarkdownUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesMarkdownUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					if (!BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesTotalSetIndex : PlanFormulaProfile
		{
			public clsInit_SalesTotalSetIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesTotalSetIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesTotalUnits, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanVariables.SalesTotalUnits, BasePlanQuantityVariables.StoreAverage, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesRegularSetIndex : PlanFormulaProfile
		{
			public clsInit_SalesRegularSetIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegularSetIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesRegularUnits, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanVariables.SalesRegularUnits, BasePlanQuantityVariables.StoreAverage, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesPromoSetIndex : PlanFormulaProfile
		{
			public clsInit_SalesPromoSetIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesPromoSetIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesPromoUnits, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanVariables.SalesPromoUnits, BasePlanQuantityVariables.StoreAverage, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesRegPromoSetIndex : PlanFormulaProfile
		{
			public clsInit_SalesRegPromoSetIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegPromoSetIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesRegPromoUnits, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanVariables.SalesRegPromoUnits, BasePlanQuantityVariables.StoreAverage, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesMarkdownSetIndex : PlanFormulaProfile
		{
			public clsInit_SalesMarkdownSetIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesMarkdownSetIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesMarkdownUnits, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanVariables.SalesMarkdownUnits, BasePlanQuantityVariables.StoreAverage, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesTotalAllIndex : PlanFormulaProfile
		{
			public clsInit_SalesTotalAllIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesTotalAllIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesTotalUnits, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanVariables.SalesTotalUnits, BasePlanQuantityVariables.StoreAverage, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesRegularAllIndex : PlanFormulaProfile
		{
			public clsInit_SalesRegularAllIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegularAllIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesRegularUnits, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanVariables.SalesRegularUnits, BasePlanQuantityVariables.StoreAverage, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesPromoAllIndex : PlanFormulaProfile
		{
			public clsInit_SalesPromoAllIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesPromoAllIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesPromoUnits, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanVariables.SalesPromoUnits, BasePlanQuantityVariables.StoreAverage, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesRegPromoAllIndex : PlanFormulaProfile
		{
			public clsInit_SalesRegPromoAllIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegPromoAllIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesRegPromoUnits, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanVariables.SalesRegPromoUnits, BasePlanQuantityVariables.StoreAverage, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesMarkdownAllIndex : PlanFormulaProfile
		{
			public clsInit_SalesMarkdownAllIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesMarkdownAllIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesMarkdownUnits, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanVariables.SalesMarkdownUnits, BasePlanQuantityVariables.StoreAverage, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryTotalUnits : PlanFormulaProfile
		{
			public clsInit_InventoryTotalUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryTotalUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryRegularUnits, aSchdEntry.PlanCellRef.isCellHidden) +
							BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryMarkdownUnits, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						if (!BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
						{
							BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
						}
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryRegularUnits : PlanFormulaProfile
		{
			public clsInit_InventoryRegularUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryRegularUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					if (BasePlanToolBox.isForecast(aSchdEntry.PlanCellRef) && !BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryMarkdownUnits : PlanFormulaProfile
		{
			public clsInit_InventoryMarkdownUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryMarkdownUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					if (!BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryTotalSetIndex : PlanFormulaProfile
		{
			public clsInit_InventoryTotalSetIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryTotalSetIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryTotalUnits, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanVariables.InventoryTotalUnits, BasePlanQuantityVariables.StoreAverage, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryRegularSetIndex : PlanFormulaProfile
		{
			public clsInit_InventoryRegularSetIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryRegularSetIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryRegularUnits, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanVariables.InventoryRegularUnits, BasePlanQuantityVariables.StoreAverage, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryMarkdownSetIndex : PlanFormulaProfile
		{
			public clsInit_InventoryMarkdownSetIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryMarkdownSetIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryMarkdownUnits, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanVariables.InventoryMarkdownUnits, BasePlanQuantityVariables.StoreAverage, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryTotalAllIndex : PlanFormulaProfile
		{
			public clsInit_InventoryTotalAllIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryTotalAllIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryTotalUnits, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanVariables.InventoryTotalUnits, BasePlanQuantityVariables.StoreAverage, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryRegularAllIndex : PlanFormulaProfile
		{
			public clsInit_InventoryRegularAllIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryRegularAllIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryRegularUnits, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanVariables.InventoryRegularUnits, BasePlanQuantityVariables.StoreAverage, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryMarkdownAllIndex : PlanFormulaProfile
		{
			public clsInit_InventoryMarkdownAllIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryMarkdownAllIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryMarkdownUnits, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanVariables.InventoryMarkdownUnits, BasePlanQuantityVariables.StoreAverage, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_ReceiptTotalUnits : PlanFormulaProfile
		{
			public clsInit_ReceiptTotalUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "ReceiptTotalUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryTotalUnits, BasePlanToolBox.GetCurrentPlanTimeDetail(aSchdEntry.PlanCellRef, 1), true) +
							BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesTotalUnits, aSchdEntry.PlanCellRef.isCellHidden) -
							BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryTotalUnits, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_ReceiptRegularUnits : PlanFormulaProfile
		{
			public clsInit_ReceiptRegularUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "ReceiptRegularUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryRegularUnits, BasePlanToolBox.GetCurrentPlanTimeDetail(aSchdEntry.PlanCellRef, 1), true) +
							BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesRegPromoUnits, aSchdEntry.PlanCellRef.isCellHidden) -
							BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryRegularUnits, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_ReceiptMarkdownUnits : PlanFormulaProfile
		{
			public clsInit_ReceiptMarkdownUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "ReceiptMarkdownUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryMarkdownUnits, BasePlanToolBox.GetCurrentPlanTimeDetail(aSchdEntry.PlanCellRef, 1), true) +
							BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesMarkdownUnits, aSchdEntry.PlanCellRef.isCellHidden) -
							BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryMarkdownUnits, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesStockRatioTotal : PlanFormulaProfile
		{
			public clsInit_SalesStockRatioTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesStockRatioTotal Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryTotalUnits, aSchdEntry.PlanCellRef.isCellHidden) /
							BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesTotalUnits, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesStockRatioRegPromo : PlanFormulaProfile
		{
			public clsInit_SalesStockRatioRegPromo(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesStockRatioRegPromo Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryRegularUnits, aSchdEntry.PlanCellRef.isCellHidden) /
							BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesRegPromoUnits, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesStockRatioMarkdown : PlanFormulaProfile
		{
			public clsInit_SalesStockRatioMarkdown(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesStockRatioMarkdown Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryMarkdownUnits, aSchdEntry.PlanCellRef.isCellHidden) /
							BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesMarkdownUnits, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_ForwardWOSTotal : PlanFormulaProfile
		{
			public clsInit_ForwardWOSTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "ForwardWOSTotal Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.SetCellValue(
							aSetCellMode,
							aSchdEntry.PlanCellRef,
							BasePlanToolBox.CalculateFWOS(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryTotalUnits, BasePlanVariables.SalesTotalUnits));
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_ForwardWOSRegPromo : PlanFormulaProfile
		{
			public clsInit_ForwardWOSRegPromo(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "ForwardWOSRegPromo Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.SetCellValue(
							aSetCellMode,
							aSchdEntry.PlanCellRef,
							BasePlanToolBox.CalculateFWOS(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryRegularUnits, BasePlanVariables.SalesRegPromoUnits));
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_ForwardWOSMarkdown : PlanFormulaProfile
		{
			public clsInit_ForwardWOSMarkdown(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "ForwardWOSMarkdown Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.SetCellValue(
							aSetCellMode,
							aSchdEntry.PlanCellRef,
							BasePlanToolBox.CalculateFWOS(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryMarkdownUnits, BasePlanVariables.SalesMarkdownUnits));
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_ForwardWOSTotalSetIndex : PlanFormulaProfile
		{
			public clsInit_ForwardWOSTotalSetIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "ForwardWOSTotalSetIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.ForwardWOSTotal, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanVariables.ForwardWOSTotal, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_ForwardWOSRegPromoSetIndex : PlanFormulaProfile
		{
			public clsInit_ForwardWOSRegPromoSetIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "ForwardWOSRegPromoSetIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.ForwardWOSRegPromo, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanVariables.ForwardWOSRegPromo, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_ForwardWOSMarkdownSetIndex : PlanFormulaProfile
		{
			public clsInit_ForwardWOSMarkdownSetIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "ForwardWOSMarkdownSetIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.ForwardWOSMarkdown, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanVariables.ForwardWOSMarkdown, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_ForwardWOSTotalAllIndex : PlanFormulaProfile
		{
			public clsInit_ForwardWOSTotalAllIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "ForwardWOSTotalAllIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.ForwardWOSTotal, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanVariables.ForwardWOSTotal, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_ForwardWOSRegPromoAllIndex : PlanFormulaProfile
		{
			public clsInit_ForwardWOSRegPromoAllIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "ForwardWOSRegPromoAllIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.ForwardWOSRegPromo, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanVariables.ForwardWOSRegPromo, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_ForwardWOSMarkdownAllIndex : PlanFormulaProfile
		{
			public clsInit_ForwardWOSMarkdownAllIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "ForwardWOSMarkdownAllIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.ForwardWOSMarkdown, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanVariables.ForwardWOSMarkdown, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SellThruPctTotal : PlanFormulaProfile
		{
			public clsInit_SellThruPctTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SellThruPctTotal Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesTotalUnits, aSchdEntry.PlanCellRef.isCellHidden) /
							BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryTotalUnits, aSchdEntry.PlanCellRef.isCellHidden));

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SellThruPctRegPromo : PlanFormulaProfile
		{
			public clsInit_SellThruPctRegPromo(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SellThruPctRegPromo Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesRegPromoUnits, aSchdEntry.PlanCellRef.isCellHidden) /
							BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryRegularUnits, aSchdEntry.PlanCellRef.isCellHidden));

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SellThruPctMarkdown : PlanFormulaProfile
		{
			public clsInit_SellThruPctMarkdown(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SellThruPctMarkdown Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesMarkdownUnits, aSchdEntry.PlanCellRef.isCellHidden) /
							BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryMarkdownUnits, aSchdEntry.PlanCellRef.isCellHidden));

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SellThruPctTotalSetIndex : PlanFormulaProfile
		{
			public clsInit_SellThruPctTotalSetIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SellThruPctTotalSetIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SellThruPctTotal, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanVariables.SellThruPctTotal, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SellThruPctRegPromoSetIndex : PlanFormulaProfile
		{
			public clsInit_SellThruPctRegPromoSetIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SellThruPctRegPromoSetIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SellThruPctRegPromo, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanVariables.SellThruPctRegPromo, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SellThruPctMarkdownSetIndex : PlanFormulaProfile
		{
			public clsInit_SellThruPctMarkdownSetIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SellThruPctMarkdownSetIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SellThruPctMarkdown, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanVariables.SellThruPctMarkdown, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SellThruPctTotalAllIndex : PlanFormulaProfile
		{
			public clsInit_SellThruPctTotalAllIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SellThruPctTotalAllIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SellThruPctTotal, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanVariables.SellThruPctTotal, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SellThruPctRegPromoAllIndex : PlanFormulaProfile
		{
			public clsInit_SellThruPctRegPromoAllIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SellThruPctRegPromoAllIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SellThruPctRegPromo, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanVariables.SellThruPctRegPromo, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SellThruPctMarkdownAllIndex : PlanFormulaProfile
		{
			public clsInit_SellThruPctMarkdownAllIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SellThruPctMarkdownAllIndex Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SellThruPctMarkdown, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanVariables.SellThruPctMarkdown, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_Intransit : PlanFormulaProfile
		{
			public clsInit_Intransit(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "Intransit Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					newValue = BasePlanToolBox.GetWeeklyIntransit(aSchdEntry.PlanCellRef);

					BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_GradeTotal : PlanFormulaProfile
		{
			public clsInit_GradeTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "GradeTotal Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.SetGradeCellValue(
							aSetCellMode,
							aSchdEntry.PlanCellRef,
							BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesTotalUnits, aSchdEntry.PlanCellRef.isCellHidden),
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanVariables.SalesTotalUnits, BasePlanQuantityVariables.StoreAverage, aSchdEntry.PlanCellRef.isCellHidden));
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_GradeRegPromo : PlanFormulaProfile
		{
			public clsInit_GradeRegPromo(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "GradeRegPromo Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.SetGradeCellValue(
							aSetCellMode,
							aSchdEntry.PlanCellRef,
							BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesRegPromoUnits, aSchdEntry.PlanCellRef.isCellHidden),
							BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanVariables.SalesRegPromoUnits, BasePlanQuantityVariables.StoreAverage, aSchdEntry.PlanCellRef.isCellHidden));
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_StoreStatus : PlanFormulaProfile
		{
			public clsInit_StoreStatus(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "StoreStatus Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, BasePlanToolBox.DetermineStoreStatus(aSchdEntry.PlanCellRef));

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}
		
		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Generic Total Inits
		//-------------------------------------------

		protected class clsInit_SumDetail : clsFormula_Sum
		{
			public clsInit_SumDetail(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumDetail Init")
			{
			}
		}

		protected class clsInit_AvgDetail : clsFormula_AverageValue
		{
			public clsInit_AvgDetail(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgDetail Init")
			{
			}
		}

		protected class clsInit_AvgDetail_LowLevelTotal : clsFormula_AverageAlternateDetail_LowLevelTotal
		{
			public clsInit_AvgDetail_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgDetail_LowLevelTotal Init")
			{
			}
			
			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			override protected VariableProfile InventoryVariableProfile
			{
				get
				{
					return null;
				}
			}

			override protected bool CalculateNonZeroAverage(PlanCellReference aPlanCellRef)
			{
				return false;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.None;
				}
			}
		}

		protected class clsInit_PeriodSumDetail : clsFormula_SumHidden
		{
			public clsInit_PeriodSumDetail(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "PeriodSumDetail Init")
			{
			}
		}

		//Begin TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
		protected class clsInit_PeriodAvgDetail : clsFormula_AverageValue
		{
			public clsInit_PeriodAvgDetail(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "PeriodAvgDetail Init")
			{
			}
		}

		//End TT#619 - JScott - OTS Forecast - Chain Plan not required (#46)
		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Time Total Inits
		//-------------------------------------------

		protected class clsInit_SalesUnitsTimeAvg : clsFormula_TimeAverage
		{
			public clsInit_SalesUnitsTimeAvg(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesUnitsTimeAvg Init")
			{
			}

			public override eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				VariableProfile varProf;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) &&
						BasePlanToolBox.isCurrentPostingWeekInTimeLine(aSchdEntry.PlanCellRef) &&
						BasePlanToolBox.isStore(aSchdEntry.PlanCellRef))
					{
						varProf = (VariableProfile)BasePlanVariables.VariableProfileList.FindKey(aSchdEntry.PlanCellRef[eProfileType.Variable]);
						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, BasePlanToolBox.CalculateActualCurrentWeekAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, varProf));

						return eComputationFormulaReturnType.Successful;
					}
					else
					{
						return base.Execute(aSchdEntry, aGetCellMode, aSetCellMode);
					}
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryUnitsTimeAvg : clsFormula_AverageHidden
		{
			public clsInit_InventoryUnitsTimeAvg(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryUnitsTimeAvg Init")
			{
			}

			public override eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				VariableProfile varProf;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) &&
						BasePlanToolBox.isCurrentPostingWeekInTimeLine(aSchdEntry.PlanCellRef) &&
						BasePlanToolBox.isStore(aSchdEntry.PlanCellRef))
					{
						varProf = (VariableProfile)BasePlanVariables.VariableProfileList.FindKey(aSchdEntry.PlanCellRef[eProfileType.Variable]);
						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, BasePlanToolBox.CalculateActualCurrentWeekAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, varProf));

						return eComputationFormulaReturnType.Successful;
					}
					else
					{
						return base.Execute(aSchdEntry, aGetCellMode, aSetCellMode);
					}
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryUnitsEnding : clsFormula_SumHidden
		{
			public clsInit_InventoryUnitsEnding(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryUnitsEnding Init")
			{
			}
		}

		protected class clsInit_SalesTotalUnitsT3 : PlanFormulaProfile
		{
			public clsInit_SalesTotalUnitsT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesTotalUnitsT3 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = BasePlanToolBox.GetWTDSalesCellValue(aSchdEntry.PlanCellRef, BasePlanVariables.GetTotalWeekToDaySalesVariableList());

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesRegularUnitsT2 : PlanFormulaProfile
		{
			public clsInit_SalesRegularUnitsT2(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegularUnitsT2 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				ProfileList varProfList;
				double newValue;

				try
				{
					if (BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						varProfList = new ProfileList(eProfileType.Variable);
						varProfList.Add(BasePlanVariables.SalesRegularUnits);
						newValue = BasePlanToolBox.GetWTDSalesCellValue(aSchdEntry.PlanCellRef, varProfList);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesPromoUnitsT2 : PlanFormulaProfile
		{
			public clsInit_SalesPromoUnitsT2(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesPromoUnitsT2 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				ProfileList varProfList;
				double newValue;

				try
				{
					if (BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						varProfList = new ProfileList(eProfileType.Variable);
						varProfList.Add(BasePlanVariables.SalesPromoUnits);
						newValue = BasePlanToolBox.GetWTDSalesCellValue(aSchdEntry.PlanCellRef, varProfList);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesRegPromoUnitsT3 : PlanFormulaProfile
		{
			public clsInit_SalesRegPromoUnitsT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegPromoUnitsT3 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				ProfileList varProfList;
				double newValue;

				try
				{
					if (BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						varProfList = new ProfileList(eProfileType.Variable);
						varProfList.Add(BasePlanVariables.SalesRegularUnits);
						varProfList.Add(BasePlanVariables.SalesPromoUnits);
						newValue = BasePlanToolBox.GetWTDSalesCellValue(aSchdEntry.PlanCellRef, varProfList);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesMarkdownUnitsT3 : PlanFormulaProfile
		{
			public clsInit_SalesMarkdownUnitsT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesMarkdownUnitsT3 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				ProfileList varProfList;
				double newValue;

				try
				{
					if (BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						varProfList = new ProfileList(eProfileType.Variable);
						varProfList.Add(BasePlanVariables.SalesMarkdownUnits);
						newValue = BasePlanToolBox.GetWTDSalesCellValue(aSchdEntry.PlanCellRef, varProfList);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesTotalSetIndexT1 : PlanFormulaProfile
		{
			public clsInit_SalesTotalSetIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesTotalSetIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.SalesTotalUnitsT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.SalesTotalUnitsT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesRegularSetIndexT1 : PlanFormulaProfile
		{
			public clsInit_SalesRegularSetIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegularSetIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.SalesRegularUnitsT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.SalesRegularUnitsT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesPromoSetIndexT1 : PlanFormulaProfile
		{
			public clsInit_SalesPromoSetIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesPromoSetIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.SalesPromoUnitsT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.SalesPromoUnitsT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesRegPromoSetIndexT1 : PlanFormulaProfile
		{
			public clsInit_SalesRegPromoSetIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegPromoSetIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.SalesRegPromoUnitsT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.SalesRegPromoUnitsT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesMarkdownSetIndexT1 : PlanFormulaProfile
		{
			public clsInit_SalesMarkdownSetIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesMarkdownSetIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.SalesMarkdownUnitsT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.SalesMarkdownUnitsT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesTotalAllIndexT1 : PlanFormulaProfile
		{
			public clsInit_SalesTotalAllIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesTotalAllIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.SalesTotalUnitsT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.SalesTotalUnitsT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesRegularAllIndexT1 : PlanFormulaProfile
		{
			public clsInit_SalesRegularAllIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegularAllIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.SalesRegularUnitsT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.SalesRegularUnitsT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesPromoAllIndexT1 : PlanFormulaProfile
		{
			public clsInit_SalesPromoAllIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesPromoAllIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.SalesPromoUnitsT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.SalesPromoUnitsT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesRegPromoAllIndexT1 : PlanFormulaProfile
		{
			public clsInit_SalesRegPromoAllIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegPromoAllIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.SalesRegPromoUnitsT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.SalesRegPromoUnitsT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesMarkdownAllIndexT1 : PlanFormulaProfile
		{
			public clsInit_SalesMarkdownAllIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesMarkdownAllIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.SalesMarkdownUnitsT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.SalesMarkdownUnitsT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryTotalUnitsT4 : PlanFormulaProfile
		{
			public clsInit_InventoryTotalUnitsT4(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryTotalUnitsT4 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = BasePlanToolBox.GetOnHandCellValue(aSchdEntry.PlanCellRef);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}
					newValue = BasePlanToolBox.GetOnHandCellValue(aSchdEntry.PlanCellRef);

					BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryTotalUnitsT5 : PlanFormulaProfile
		{
			public clsInit_InventoryTotalUnitsT5(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryTotalUnitsT5 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
                        // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
                        //newValue = BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryTotalUnitsT4, aSchdEntry.PlanCellRef.isCellHidden) +
                        //    BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.IntransitT1, aSchdEntry.PlanCellRef.isCellHidden);

                        newValue = BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryTotalUnitsT4, aSchdEntry.PlanCellRef.isCellHidden)
                                 + BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.IntransitT1, aSchdEntry.PlanCellRef.isCellHidden)
                                 + BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryTotalUnitsT8, aSchdEntry.PlanCellRef.isCellHidden);  
                        // End TT#2054
						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryTotalUnitsT6 : PlanFormulaProfile
		{
			public clsInit_InventoryTotalUnitsT6(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryTotalUnitsT6 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = BasePlanToolBox.GetNeedCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesTotalUnits, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryTotalUnitsT7 : PlanFormulaProfile
		{
			public clsInit_InventoryTotalUnitsT7(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryTotalUnitsT7 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = BasePlanToolBox.GetPctNeedCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryTotalUnitsT6, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

        // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
        protected class clsInit_InventoryTotalUnitsT8 : PlanFormulaProfile
        {
            public clsInit_InventoryTotalUnitsT8(BasePlanComputations aBasePlanComputations, int aKey)
                : base(aBasePlanComputations, aKey, "InventoryTotalUnitsT8 Init")
            {
            }

            override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
            {
                double newValue;

                try
                {
                    if (BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
                    {
                        newValue = BasePlanToolBox.GetVSWOnHandCellValue(aSchdEntry.PlanCellRef);

                        BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
                    }
                    else
                    {
                        BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
                    }
                    newValue = BasePlanToolBox.GetVSWOnHandCellValue(aSchdEntry.PlanCellRef);

                    BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);

                    return eComputationFormulaReturnType.Successful;
                }
                catch (Exception exc)
                {
                    throw;
                }
            }
        }
        // End TT#2054

		protected class clsInit_InventoryRegularUnitsT4 : PlanFormulaProfile
		{
			public clsInit_InventoryRegularUnitsT4(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "clsInit_InventoryRegularUnitsT4")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					newValue = BasePlanToolBox.GetOnHandCellValue(aSchdEntry.PlanCellRef);

					BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryRegularUnitsT5 : PlanFormulaProfile
		{
			public clsInit_InventoryRegularUnitsT5(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryRegularUnitsT5 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{   // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals 
                        //newValue = BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryRegularUnitsT4, aSchdEntry.PlanCellRef.isCellHidden) +
                        //    BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.IntransitT1, aSchdEntry.PlanCellRef.isCellHidden);

                        newValue = BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryRegularUnitsT4, aSchdEntry.PlanCellRef.isCellHidden)  
                                 + BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.IntransitT1, aSchdEntry.PlanCellRef.isCellHidden)
                                 + BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryRegularUnitsT8, aSchdEntry.PlanCellRef.isCellHidden);
                        // End TT#2054

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryRegularUnitsT6 : PlanFormulaProfile
		{
			public clsInit_InventoryRegularUnitsT6(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryRegularUnitsT6 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = BasePlanToolBox.GetNeedCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesRegPromoUnits, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryRegularUnitsT7 : PlanFormulaProfile
		{
			public clsInit_InventoryRegularUnitsT7(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryRegularUnitsT7 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = BasePlanToolBox.GetPctNeedCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryRegularUnitsT6, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

        // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
        protected class clsInit_InventoryRegularUnitsT8 : PlanFormulaProfile
        {
            public clsInit_InventoryRegularUnitsT8(BasePlanComputations aBasePlanComputations, int aKey)
                : base(aBasePlanComputations, aKey, "clsInit_InventoryRegularUnitsT8")
            {
            }

            override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
            {
                double newValue;

                try
                {
                    newValue = BasePlanToolBox.GetVSWOnHandCellValue(aSchdEntry.PlanCellRef);

                    BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);

                    return eComputationFormulaReturnType.Successful;
                }
                catch (Exception exc)
                {
                    throw;
                }
            }
        }
        // End TT#2054  

		protected class clsInit_InventoryTotalSetIndexT1 : PlanFormulaProfile
		{
			public clsInit_InventoryTotalSetIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryTotalSetIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryTotalUnitsT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.InventoryTotalUnitsT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryTotalSetIndexT2 : PlanFormulaProfile
		{
			public clsInit_InventoryTotalSetIndexT2(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryTotalSetIndexT2 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryTotalUnitsT2, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.InventoryTotalUnitsT2, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryTotalSetIndexT3 : PlanFormulaProfile
		{
			public clsInit_InventoryTotalSetIndexT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryTotalSetIndexT3 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryTotalUnitsT3, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.InventoryTotalUnitsT3, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryRegularSetIndexT1 : PlanFormulaProfile
		{
			public clsInit_InventoryRegularSetIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryRegularSetIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryRegularUnitsT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.InventoryRegularUnitsT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryRegularSetIndexT2 : PlanFormulaProfile
		{
			public clsInit_InventoryRegularSetIndexT2(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryRegularSetIndexT2 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryRegularUnitsT2, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.InventoryRegularUnitsT2, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryRegularSetIndexT3 : PlanFormulaProfile
		{
			public clsInit_InventoryRegularSetIndexT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryRegularSetIndexT3 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryRegularUnitsT3, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.InventoryRegularUnitsT3, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryMarkdownSetIndexT1 : PlanFormulaProfile
		{
			public clsInit_InventoryMarkdownSetIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryMarkdownSetIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryMarkdownUnitsT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.InventoryMarkdownUnitsT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryMarkdownSetIndexT2 : PlanFormulaProfile
		{
			public clsInit_InventoryMarkdownSetIndexT2(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryMarkdownSetIndexT2 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryMarkdownUnitsT2, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.InventoryMarkdownUnitsT2, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryMarkdownSetIndexT3 : PlanFormulaProfile
		{
			public clsInit_InventoryMarkdownSetIndexT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "_InventoryMarkdownSetIndexT3 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryMarkdownUnitsT3, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.InventoryMarkdownUnitsT3, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryTotalAllIndexT1 : PlanFormulaProfile
		{
			public clsInit_InventoryTotalAllIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryTotalAllIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryTotalUnitsT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.InventoryTotalUnitsT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryTotalAllIndexT2 : PlanFormulaProfile
		{
			public clsInit_InventoryTotalAllIndexT2(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryTotalAllIndexT2 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryTotalUnitsT2, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.InventoryTotalUnitsT2, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryTotalAllIndexT3 : PlanFormulaProfile
		{
			public clsInit_InventoryTotalAllIndexT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryTotalAllIndexT3 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryTotalUnitsT3, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.InventoryTotalUnitsT3, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryRegularAllIndexT1 : PlanFormulaProfile
		{
			public clsInit_InventoryRegularAllIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryRegularAllIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryRegularUnitsT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.InventoryRegularUnitsT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryRegularAllIndexT2 : PlanFormulaProfile
		{
			public clsInit_InventoryRegularAllIndexT2(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "clsInit_InventoryRegularAllIndexT2")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryRegularUnitsT2, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.InventoryRegularUnitsT2, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryRegularAllIndexT3 : PlanFormulaProfile
		{
			public clsInit_InventoryRegularAllIndexT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryRegularAllIndexT3 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryRegularUnitsT3, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.InventoryRegularUnitsT3, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryMarkdownAllIndexT1 : PlanFormulaProfile
		{
			public clsInit_InventoryMarkdownAllIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryMarkdownAllIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryMarkdownUnitsT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.InventoryMarkdownUnitsT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryMarkdownAllIndexT2 : PlanFormulaProfile
		{
			public clsInit_InventoryMarkdownAllIndexT2(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryMarkdownAllIndexT2 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryMarkdownUnitsT2, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.InventoryMarkdownUnitsT2, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_InventoryMarkdownAllIndexT3 : PlanFormulaProfile
		{
			public clsInit_InventoryMarkdownAllIndexT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryMarkdownAllIndexT3 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryMarkdownUnitsT3, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.InventoryMarkdownUnitsT3, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_ForwardWOSTotalSetIndexT1 : PlanFormulaProfile
		{
			public clsInit_ForwardWOSTotalSetIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "ForwardWOSTotalSetIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.ForwardWOSTotalT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.Value, BasePlanTimeTotalVariables.ForwardWOSTotalT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_ForwardWOSRegPromoSetIndexT1 : PlanFormulaProfile
		{
			public clsInit_ForwardWOSRegPromoSetIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "ForwardWOSRegPromoSetIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.ForwardWOSRegPromoT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.Value, BasePlanTimeTotalVariables.ForwardWOSRegPromoT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_ForwardWOSMarkdownSetIndexT1 : PlanFormulaProfile
		{
			public clsInit_ForwardWOSMarkdownSetIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "ForwardWOSMarkdownSetIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.ForwardWOSMarkdownT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.Value, BasePlanTimeTotalVariables.ForwardWOSMarkdownT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_ForwardWOSTotalAllIndexT1 : PlanFormulaProfile
		{
			public clsInit_ForwardWOSTotalAllIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "ForwardWOSTotalAllIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.ForwardWOSTotalT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.Value, BasePlanTimeTotalVariables.ForwardWOSTotalT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_ForwardWOSRegPromoAllIndexT1 : PlanFormulaProfile
		{
			public clsInit_ForwardWOSRegPromoAllIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "ForwardWOSRegPromoAllIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.ForwardWOSRegPromoT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.Value, BasePlanTimeTotalVariables.ForwardWOSRegPromoT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_ForwardWOSMarkdownAllIndexT1 : PlanFormulaProfile
		{
			public clsInit_ForwardWOSMarkdownAllIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "ForwardWOSMarkdownAllIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.ForwardWOSMarkdownT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.Value, BasePlanTimeTotalVariables.ForwardWOSMarkdownT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesStockRatioTotalT1 : PlanFormulaProfile
		{
			public clsInit_SalesStockRatioTotalT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesStockRatioTotalT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryTotalUnitsT1, aSchdEntry.PlanCellRef.isCellHidden) / 
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.SalesTotalUnitsT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesStockRatioRegPromoT1 : PlanFormulaProfile
		{
			public clsInit_SalesStockRatioRegPromoT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesStockRatioRegPromoT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryRegularUnitsT1, aSchdEntry.PlanCellRef.isCellHidden) / 
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.SalesRegPromoUnitsT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SalesStockRatioMarkdownT1 : PlanFormulaProfile
		{
			public clsInit_SalesStockRatioMarkdownT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesStockRatioMarkdownT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.InventoryMarkdownUnitsT1, aSchdEntry.PlanCellRef.isCellHidden) / 
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.SalesMarkdownUnitsT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SellThruPctTotalT1 : PlanFormulaProfile
		{
			public clsInit_SellThruPctTotalT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SellThruPctTotalT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesTotalUnits.AverageTimeTotalVariableProfile, aSchdEntry.PlanCellRef.isCellHidden) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryTotalUnits.AverageTimeTotalVariableProfile, aSchdEntry.PlanCellRef.isCellHidden));

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SellThruPctRegPromoT1 : PlanFormulaProfile
		{
			public clsInit_SellThruPctRegPromoT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SellThruPctRegPromoT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesRegPromoUnits.AverageTimeTotalVariableProfile, aSchdEntry.PlanCellRef.isCellHidden) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryRegularUnits.AverageTimeTotalVariableProfile, aSchdEntry.PlanCellRef.isCellHidden));

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SellThruPctMarkdownT1 : PlanFormulaProfile
		{
			public clsInit_SellThruPctMarkdownT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SellThruPctMarkdownT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.SalesMarkdownUnits.AverageTimeTotalVariableProfile, aSchdEntry.PlanCellRef.isCellHidden) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanVariables.InventoryMarkdownUnits.AverageTimeTotalVariableProfile, aSchdEntry.PlanCellRef.isCellHidden));

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SellThruPctTotalSetIndexT1 : PlanFormulaProfile
		{
			public clsInit_SellThruPctTotalSetIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SellThruPctTotalSetIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.SellThruPctTotalT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.Value, BasePlanTimeTotalVariables.SellThruPctTotalT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SellThruPctRegPromoSetIndexT1 : PlanFormulaProfile
		{
			public clsInit_SellThruPctRegPromoSetIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SellThruPctRegPromoSetIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.SellThruPctRegPromoT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.Value, BasePlanTimeTotalVariables.SellThruPctRegPromoT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SellThruPctMarkdownSetIndexT1 : PlanFormulaProfile
		{
			public clsInit_SellThruPctMarkdownSetIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SellThruPctMarkdownSetIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.SellThruPctMarkdownT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.Value, BasePlanTimeTotalVariables.SellThruPctMarkdownT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SellThruPctTotalAllIndexT1 : PlanFormulaProfile
		{
			public clsInit_SellThruPctTotalAllIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SellThruPctTotalAllIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.SellThruPctTotalT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.Value, BasePlanTimeTotalVariables.SellThruPctTotalT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SellThruPctRegPromoAllIndexT1 : PlanFormulaProfile
		{
			public clsInit_SellThruPctRegPromoAllIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SellThruPctRegPromoAllIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.SellThruPctRegPromoT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.Value, BasePlanTimeTotalVariables.SellThruPctRegPromoT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SellThruPctMarkdownAllIndexT1 : PlanFormulaProfile
		{
			public clsInit_SellThruPctMarkdownAllIndexT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SellThruPctMarkdownAllIndexT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						newValue = (BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.SellThruPctMarkdownT1, aSchdEntry.PlanCellRef.isCellHidden) * 100) /
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.Value, BasePlanTimeTotalVariables.SellThruPctMarkdownT1, aSchdEntry.PlanCellRef.isCellHidden);

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_GradeTotalT1 : PlanFormulaProfile
		{
			public clsInit_GradeTotalT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "clsInit_GradeTotalT1")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isTotalPlanType(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.SetGradeCellValue(
							aSetCellMode,
							aSchdEntry.PlanCellRef,
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.SalesTotalUnitsT1, aSchdEntry.PlanCellRef.isCellHidden),
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.SalesTotalUnitsT1, aSchdEntry.PlanCellRef.isCellHidden));
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_GradeRegPromoT1 : PlanFormulaProfile
		{
			public clsInit_GradeRegPromoT1(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "GradeRegPromoT1 Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef) || BasePlanToolBox.isRegularPlanType(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.SetGradeCellValue(
							aSetCellMode,
							aSchdEntry.PlanCellRef,
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanTimeTotalVariables.SalesRegPromoUnitsT1, aSchdEntry.PlanCellRef.isCellHidden),
							BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.StoreAverage, BasePlanTimeTotalVariables.SalesRegPromoUnitsT1, aSchdEntry.PlanCellRef.isCellHidden));
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Set/Store Inits
		//-------------------------------------------

		protected class clsInit_SetTotalAllIndex : clsFormula_SetAllIndex
		{
			public clsInit_SetTotalAllIndex(BasePlanComputations aBasePlanComputations, int aKey, string aName, VariableProfile aVariableProfile)
				: base(aBasePlanComputations, aKey, aName, aVariableProfile)
			{
			}

			override public bool isDisplayed(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.isActual(aPlanCellRef) || BasePlanToolBox.isTotalPlanType(aPlanCellRef);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SetRegularAllIndex : clsFormula_SetAllIndex
		{
			public clsInit_SetRegularAllIndex(BasePlanComputations aBasePlanComputations, int aKey, string aName, VariableProfile aVariableProfile)
				: base(aBasePlanComputations, aKey, aName, aVariableProfile)
			{
			}

			override public bool isDisplayed(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.isActual(aPlanCellRef) || BasePlanToolBox.isRegularPlanType(aPlanCellRef);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SetMarkdownAllIndex : clsFormula_SetAllIndex
		{
			public clsInit_SetMarkdownAllIndex(BasePlanComputations aBasePlanComputations, int aKey, string aName, VariableProfile aVariableProfile)
				: base(aBasePlanComputations, aKey, aName, aVariableProfile)
			{
			}

			override public bool isDisplayed(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.isActual(aPlanCellRef);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SetTotalAllIndexTT : clsFormula_SetAllIndexTT
		{
			public clsInit_SetTotalAllIndexTT(BasePlanComputations aBasePlanComputations, int aKey, string aName, TimeTotalVariableProfile aTimeTotalVariableProfile)
				: base(aBasePlanComputations, aKey, aName, aTimeTotalVariableProfile)
			{
			}

			override public bool isDisplayed(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.isActual(aPlanCellRef) || BasePlanToolBox.isTotalPlanType(aPlanCellRef);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SetRegularAllIndexTT : clsFormula_SetAllIndexTT
		{
			public clsInit_SetRegularAllIndexTT(BasePlanComputations aBasePlanComputations, int aKey, string aName, TimeTotalVariableProfile aTimeTotalVariableProfile)
				: base(aBasePlanComputations, aKey, aName, aTimeTotalVariableProfile)
			{
			}

			override public bool isDisplayed(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.isActual(aPlanCellRef) || BasePlanToolBox.isRegularPlanType(aPlanCellRef);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SetMarkdownAllIndexTT : clsFormula_SetAllIndexTT
		{
			public clsInit_SetMarkdownAllIndexTT(BasePlanComputations aBasePlanComputations, int aKey, string aName, TimeTotalVariableProfile aTimeTotalVariableProfile)
				: base(aBasePlanComputations, aKey, aName, aTimeTotalVariableProfile)
			{
			}

			override public bool isDisplayed(PlanCellReference aPlanCellRef)
			{
				try
				{
					return BasePlanToolBox.isActual(aPlanCellRef);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_SumCompStore : clsFormula_SumComp
		{
			public clsInit_SumCompStore(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumCompStoreDetail Init")
			{
			}
		}

		protected class clsInit_SumNonCompStore : clsFormula_SumNonComp
		{
			public clsInit_SumNonCompStore(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNonCompStoreDetail Init")
			{
			}
		}

		protected class clsInit_SumNewStore : clsFormula_SumNew
		{
			public clsInit_SumNewStore(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNewStoreDetail Init")
			{
			}
		}

		protected class clsInit_SumStoreSalesTotalUnitsT3 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumStoreSalesTotalUnitsT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumStoreSalesTotalUnitsT3 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumCompStoreSalesTotalUnitsT3 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumCompStoreSalesTotalUnitsT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumCompStoreSalesTotalUnitsT3 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
					aPlanCellRef[eProfileType.Store],
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.Comp;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumNonCompStoreSalesTotalUnitsT3 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumNonCompStoreSalesTotalUnitsT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNonCompStoreSalesTotalUnitsT3 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
					aPlanCellRef[eProfileType.Store],
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.NonComp;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumNewStoreSalesTotalUnitsT3 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumNewStoreSalesTotalUnitsT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNewStoreSalesTotalUnitsT3 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
					aPlanCellRef[eProfileType.Store],
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.New;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumStoreSalesRegularUnitsT2 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumStoreSalesRegularUnitsT2(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumStoreSalesRegularUnitsT2 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumCompStoreSalesRegularUnitsT2 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumCompStoreSalesRegularUnitsT2(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumCompStoreSalesRegularUnitsT2 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
					aPlanCellRef[eProfileType.Store],
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.Comp;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumNonCompStoreSalesRegularUnitsT2 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumNonCompStoreSalesRegularUnitsT2(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNonCompStoreSalesRegularUnitsT2 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
					aPlanCellRef[eProfileType.Store],
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.NonComp;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumNewStoreSalesRegularUnitsT2 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumNewStoreSalesRegularUnitsT2(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNewStoreSalesRegularUnitsT2 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
					aPlanCellRef[eProfileType.Store],
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.New;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumStoreSalesPromoUnitsT2 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumStoreSalesPromoUnitsT2(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumStoreSalesPromoUnitsT2 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumCompStoreSalesPromoUnitsT2 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumCompStoreSalesPromoUnitsT2(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumCompStoreSalesPromoUnitsT2 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
					aPlanCellRef[eProfileType.Store],
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.Comp;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumNonCompStoreSalesPromoUnitsT2 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumNonCompStoreSalesPromoUnitsT2(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNonCompStoreSalesPromoUnitsT2 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
					aPlanCellRef[eProfileType.Store],
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.NonComp;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumNewStoreSalesPromoUnitsT2 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumNewStoreSalesPromoUnitsT2(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNewStoreSalesPromoUnitsT2 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
					aPlanCellRef[eProfileType.Store],
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.New;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumStoreSalesRegPromoUnitsT3 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumStoreSalesRegPromoUnitsT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumStoreSalesRegPromoUnitsT3 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumCompStoreSalesRegPromoUnitsT3 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumCompStoreSalesRegPromoUnitsT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumCompStoreSalesRegPromoUnitsT3 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
					aPlanCellRef[eProfileType.Store],
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.Comp;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumNonCompStoreSalesRegPromoUnitsT3 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumNonCompStoreSalesRegPromoUnitsT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNonCompStoreSalesRegPromoUnitsT3 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
					aPlanCellRef[eProfileType.Store],
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.NonComp;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumNewStoreSalesRegPromoUnitsT3 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumNewStoreSalesRegPromoUnitsT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNewStoreSalesRegPromoUnitsT3 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
					aPlanCellRef[eProfileType.Store],
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.New;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}


		protected class clsInit_SumStoreSalesMarkdownUnitsT3 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumStoreSalesMarkdownUnitsT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumStoreSalesMarkdownUnitsT3 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumCompStoreSalesMarkdownUnitsT3 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumCompStoreSalesMarkdownUnitsT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumCompStoreSalesMarkdownUnitsT3 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
					aPlanCellRef[eProfileType.Store],
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.Comp;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumNonCompStoreSalesMarkdownUnitsT3 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumNonCompStoreSalesMarkdownUnitsT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNonCompStoreSalesMarkdownUnitsT3 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
					aPlanCellRef[eProfileType.Store],
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.NonComp;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumNewStoreSalesMarkdownUnitsT3 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumNewStoreSalesMarkdownUnitsT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNewStoreSalesMarkdownUnitsT3 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
					aPlanCellRef[eProfileType.Store],
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.New;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumStoreInventoryTotalUnitsT4 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumStoreInventoryTotalUnitsT4(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumStoreInventoryTotalUnitsT4 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}
		}

		protected class clsInit_SumCompStoreInventoryTotalUnitsT4 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumCompStoreInventoryTotalUnitsT4(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumCompStoreInventoryTotalUnitsT4 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
					aPlanCellRef[eProfileType.Store],
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.Comp;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}
		}

		protected class clsInit_SumNonCompStoreInventoryTotalUnitsT4 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumNonCompStoreInventoryTotalUnitsT4(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNonCompStoreInventoryTotalUnitsT4 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
					aPlanCellRef[eProfileType.Store],
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.NonComp;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}
		}

		protected class clsInit_SumNewStoreInventoryTotalUnitsT4 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumNewStoreInventoryTotalUnitsT4(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNewStoreInventoryTotalUnitsT4 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
					aPlanCellRef[eProfileType.Store],
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.New;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}
		}

		protected class clsInit_SumStoreInventoryTotalUnitsT5 : clsFormula_SumStoreNeedFill
		{
			public clsInit_SumStoreInventoryTotalUnitsT5(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumStoreInventoryTotalUnitsT5 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.None;
				}
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumStoreInventoryTotalUnitsT6 : clsFormula_SumStoreNeedFill
		{
			public clsInit_SumStoreInventoryTotalUnitsT6(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumStoreInventoryTotalUnitsT6 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.None;
				}
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumCompStoreInventoryTotalUnitsT5 : clsFormula_SumStoreNeedFill
		{
			public clsInit_SumCompStoreInventoryTotalUnitsT5(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumCompStoreInventoryTotalUnitsT5 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.Comp;
				}
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumCompStoreInventoryTotalUnitsT6 : clsFormula_SumStoreNeedFill
		{
			public clsInit_SumCompStoreInventoryTotalUnitsT6(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumCompStoreInventoryTotalUnitsT6 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.Comp;
				}
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumNonCompStoreInventoryTotalUnitsT5 : clsFormula_SumStoreNeedFill
		{
			public clsInit_SumNonCompStoreInventoryTotalUnitsT5(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNonCompStoreInventoryTotalUnitsT5 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.NonComp;
				}
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumNonCompStoreInventoryTotalUnitsT6 : clsFormula_SumStoreNeedFill
		{
			public clsInit_SumNonCompStoreInventoryTotalUnitsT6(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNonCompStoreInventoryTotalUnitsT6 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.NonComp;
				}
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumNewStoreInventoryTotalUnitsT5 : clsFormula_SumStoreNeedFill
		{
			public clsInit_SumNewStoreInventoryTotalUnitsT5(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNewStoreInventoryTotalUnitsT5 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.New;
				}
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumNewStoreInventoryTotalUnitsT6 : clsFormula_SumStoreNeedFill
		{
			public clsInit_SumNewStoreInventoryTotalUnitsT6(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNewStoreInventoryTotalUnitsT6 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.New;
				}
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumStoreInventoryTotalUnitsT7 : clsFormula_SumStorePctNeed
		{
			public clsInit_SumStoreInventoryTotalUnitsT7(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumStoreInventoryTotalUnitsT7 Init")
			{
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}

			protected override TimeTotalVariableProfile NeedVariable
			{
				get
				{
					return BasePlanTimeTotalVariables.InventoryTotalUnitsT6;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.None;
				}
			}
		}

		protected class clsInit_SumCompStoreInventoryTotalUnitsT7 : clsFormula_SumStorePctNeed
		{
			public clsInit_SumCompStoreInventoryTotalUnitsT7(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumCompStoreInventoryTotalUnitsT7 Init")
			{
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}

			protected override TimeTotalVariableProfile NeedVariable
			{
				get
				{
					return BasePlanTimeTotalVariables.InventoryTotalUnitsT6;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.Comp;
				}
			}
		}

		protected class clsInit_SumNonCompStoreInventoryTotalUnitsT7 : clsFormula_SumStorePctNeed
		{
			public clsInit_SumNonCompStoreInventoryTotalUnitsT7(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNonCompStoreInventoryTotalUnitsT7 Init")
			{
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}

			protected override TimeTotalVariableProfile NeedVariable
			{
				get
				{
					return BasePlanTimeTotalVariables.InventoryTotalUnitsT6;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.NonComp;
				}
			}
		}

		protected class clsInit_SumNewStoreInventoryTotalUnitsT7 : clsFormula_SumStorePctNeed
		{
			public clsInit_SumNewStoreInventoryTotalUnitsT7(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNewStoreInventoryTotalUnitsT7 Init")
			{
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}

			protected override TimeTotalVariableProfile NeedVariable
			{
				get
				{
					return BasePlanTimeTotalVariables.InventoryTotalUnitsT6;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.New;
				}
			}
		}

        // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
        protected class clsInit_SumStoreInventoryTotalUnitsT8 : clsFormula_SumAlternateDetail
        {
            public clsInit_SumStoreInventoryTotalUnitsT8(BasePlanComputations aBasePlanComputations, int aKey)
                : base(aBasePlanComputations, aKey, "SumStoreInventoryTotalUnitsT8 Init")
            {
            }

            protected override eCubeType CubeType
            {
                get
                {
                    return eCubeType.StoreDetail;
                }
            }

            protected override bool isStatusOK(PlanCellReference aPlanCellRef)
            {
                return true;
            }

            protected override bool isTypeOK(PlanCellReference aPlanCellRef)
            {
                return true;
            }
        }

        protected class clsInit_SumCompStoreInventoryTotalUnitsT8 : clsFormula_SumAlternateDetail
        {
            public clsInit_SumCompStoreInventoryTotalUnitsT8(BasePlanComputations aBasePlanComputations, int aKey)
                : base(aBasePlanComputations, aKey, "SumCompStoreInventoryTotalUnitsT8 Init")
            {
            }

            protected override eCubeType CubeType
            {
                get
                {
                    return eCubeType.StoreDetail;
                }
            }

            protected override bool isStatusOK(PlanCellReference aPlanCellRef)
            {
                return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
                    aPlanCellRef[eProfileType.Store],
                    aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.Comp;
            }

            protected override bool isTypeOK(PlanCellReference aPlanCellRef)
            {
                return true;
            }
        }

        protected class clsInit_SumNonCompStoreInventoryTotalUnitsT8 : clsFormula_SumAlternateDetail
        {
            public clsInit_SumNonCompStoreInventoryTotalUnitsT8(BasePlanComputations aBasePlanComputations, int aKey)
                : base(aBasePlanComputations, aKey, "SumNonCompStoreInventoryTotalUnitsT8 Init")
            {
            }

            protected override eCubeType CubeType
            {
                get
                {
                    return eCubeType.StoreDetail;
                }
            }

            protected override bool isStatusOK(PlanCellReference aPlanCellRef)
            {
                return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
                    aPlanCellRef[eProfileType.Store],
                    aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.NonComp;
            }

            protected override bool isTypeOK(PlanCellReference aPlanCellRef)
            {
                return true;
            }
        }

        protected class clsInit_SumNewStoreInventoryTotalUnitsT8 : clsFormula_SumAlternateDetail
        {
            public clsInit_SumNewStoreInventoryTotalUnitsT8(BasePlanComputations aBasePlanComputations, int aKey)
                : base(aBasePlanComputations, aKey, "SumNewStoreInventoryTotalUnitsT8 Init")
            {
            }

            protected override eCubeType CubeType
            {
                get
                {
                    return eCubeType.StoreDetail;
                }
            }

            protected override bool isStatusOK(PlanCellReference aPlanCellRef)
            {
                return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
                    aPlanCellRef[eProfileType.Store],
                    aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.New;
            }

            protected override bool isTypeOK(PlanCellReference aPlanCellRef)
            {
                return true;
            }
        }
        // End TT#2054 

		protected class clsInit_SumStoreInventoryRegularUnitsT4 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumStoreInventoryRegularUnitsT4(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumStoreInventoryRegularUnitsT4 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}
		}

		protected class clsInit_SumCompStoreInventoryRegularUnitsT4 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumCompStoreInventoryRegularUnitsT4(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumCompStoreInventoryRegularUnitsT4 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
					aPlanCellRef[eProfileType.Store],
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.Comp;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}
		}

		protected class clsInit_SumNonCompStoreInventoryRegularUnitsT4 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumNonCompStoreInventoryRegularUnitsT4(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNonCompStoreInventoryRegularUnitsT4 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
					aPlanCellRef[eProfileType.Store],
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.NonComp;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}
		}

		protected class clsInit_SumNewStoreInventoryRegularUnitsT4 : clsFormula_SumAlternateDetail
		{
			public clsInit_SumNewStoreInventoryRegularUnitsT4(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNewStoreInventoryRegularUnitsT4 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
					aPlanCellRef[eProfileType.Store],
					aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.New;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}
		}

		protected class clsInit_SumStoreInventoryRegularUnitsT5 : clsFormula_SumStoreNeedFill
		{
			public clsInit_SumStoreInventoryRegularUnitsT5(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumStoreInventoryRegularUnitsT5 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.None;
				}
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumStoreInventoryRegularUnitsT6 : clsFormula_SumStoreNeedFill
		{
			public clsInit_SumStoreInventoryRegularUnitsT6(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumStoreInventoryRegularUnitsT6 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.None;
				}
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumCompStoreInventoryRegularUnitsT5 : clsFormula_SumStoreNeedFill
		{
			public clsInit_SumCompStoreInventoryRegularUnitsT5(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumCompStoreInventoryRegularUnitsT5 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.Comp;
				}
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumCompStoreInventoryRegularUnitsT6 : clsFormula_SumStoreNeedFill
		{
			public clsInit_SumCompStoreInventoryRegularUnitsT6(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumCompStoreInventoryRegularUnitsT6 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.Comp;
				}
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumNonCompStoreInventoryRegularUnitsT5 : clsFormula_SumStoreNeedFill
		{
			public clsInit_SumNonCompStoreInventoryRegularUnitsT5(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNonCompStoreInventoryRegularUnitsT5 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.NonComp;
				}
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumNonCompStoreInventoryRegularUnitsT6 : clsFormula_SumStoreNeedFill
		{
			public clsInit_SumNonCompStoreInventoryRegularUnitsT6(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNonCompStoreInventoryRegularUnitsT6 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.NonComp;
				}
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumNewStoreInventoryRegularUnitsT5 : clsFormula_SumStoreNeedFill
		{
			public clsInit_SumNewStoreInventoryRegularUnitsT5(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNewStoreInventoryRegularUnitsT5 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.New;
				}
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumNewStoreInventoryRegularUnitsT6 : clsFormula_SumStoreNeedFill
		{
			public clsInit_SumNewStoreInventoryRegularUnitsT6(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNewStoreInventoryRegularUnitsT6 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.New;
				}
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumStoreInventoryRegularUnitsT7 : clsFormula_SumStorePctNeed
		{
			public clsInit_SumStoreInventoryRegularUnitsT7(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumStoreInventoryRegularUnitsT7 Init")
			{
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}

			protected override TimeTotalVariableProfile NeedVariable
			{
				get
				{
					return BasePlanTimeTotalVariables.InventoryRegularUnitsT6;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.None;
				}
			}
		}

		protected class clsInit_SumCompStoreInventoryRegularUnitsT7 : clsFormula_SumStorePctNeed
		{
			public clsInit_SumCompStoreInventoryRegularUnitsT7(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumCompStoreInventoryRegularUnitsT7 Init")
			{
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}

			protected override TimeTotalVariableProfile NeedVariable
			{
				get
				{
					return BasePlanTimeTotalVariables.InventoryRegularUnitsT6;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.Comp;
				}
			}
		}

		protected class clsInit_SumNonCompStoreInventoryRegularUnitsT7 : clsFormula_SumStorePctNeed
		{
			public clsInit_SumNonCompStoreInventoryRegularUnitsT7(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNonCompStoreInventoryRegularUnitsT7 Init")
			{
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}

			protected override TimeTotalVariableProfile NeedVariable
			{
				get
				{
					return BasePlanTimeTotalVariables.InventoryRegularUnitsT6;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.NonComp;
				}
			}
		}

		protected class clsInit_SumNewStoreInventoryRegularUnitsT7 : clsFormula_SumStorePctNeed
		{
			public clsInit_SumNewStoreInventoryRegularUnitsT7(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumNewStoreInventoryRegularUnitsT7 Init")
			{
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}

			protected override TimeTotalVariableProfile NeedVariable
			{
				get
				{
					return BasePlanTimeTotalVariables.InventoryRegularUnitsT6;
				}
			}

			protected override eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.New;
				}
			}
		}

        // Begin TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
        protected class clsInit_SumStoreInventoryRegularUnitsT8 : clsFormula_SumAlternateDetail
        {
            public clsInit_SumStoreInventoryRegularUnitsT8(BasePlanComputations aBasePlanComputations, int aKey)
                : base(aBasePlanComputations, aKey, "SumStoreInventoryRegularUnitsT8 Init")
            {
            }

            protected override eCubeType CubeType
            {
                get
                {
                    return eCubeType.StoreDetail;
                }
            }

            protected override bool isStatusOK(PlanCellReference aPlanCellRef)
            {
                return true;
            }

            protected override bool isTypeOK(PlanCellReference aPlanCellRef)
            {
                return true;
            }
        }

        protected class clsInit_SumCompStoreInventoryRegularUnitsT8 : clsFormula_SumAlternateDetail
        {
            public clsInit_SumCompStoreInventoryRegularUnitsT8(BasePlanComputations aBasePlanComputations, int aKey)
                : base(aBasePlanComputations, aKey, "SumCompStoreInventoryRegularUnitsT8 Init")
            {
            }

            protected override eCubeType CubeType
            {
                get
                {
                    return eCubeType.StoreDetail;
                }
            }

            protected override bool isStatusOK(PlanCellReference aPlanCellRef)
            {
                return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
                    aPlanCellRef[eProfileType.Store],
                    aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.Comp;
            }

            protected override bool isTypeOK(PlanCellReference aPlanCellRef)
            {
                return true;
            }
        }

        protected class clsInit_SumNonCompStoreInventoryRegularUnitsT8 : clsFormula_SumAlternateDetail
        {
            public clsInit_SumNonCompStoreInventoryRegularUnitsT8(BasePlanComputations aBasePlanComputations, int aKey)
                : base(aBasePlanComputations, aKey, "SumNonCompStoreInventoryRegularUnitsT8 Init")
            {
            }

            protected override eCubeType CubeType
            {
                get
                {
                    return eCubeType.StoreDetail;
                }
            }

            protected override bool isStatusOK(PlanCellReference aPlanCellRef)
            {
                return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
                    aPlanCellRef[eProfileType.Store],
                    aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.NonComp;
            }

            protected override bool isTypeOK(PlanCellReference aPlanCellRef)
            {
                return true;
            }
        }

        protected class clsInit_SumNewStoreInventoryRegularUnitsT8 : clsFormula_SumAlternateDetail
        {
            public clsInit_SumNewStoreInventoryRegularUnitsT8(BasePlanComputations aBasePlanComputations, int aKey)
                : base(aBasePlanComputations, aKey, "SumNewStoreInventoryRegularUnitsT8 Init")
            {
            }

            protected override eCubeType CubeType
            {
                get
                {
                    return eCubeType.StoreDetail;
                }
            }

            protected override bool isStatusOK(PlanCellReference aPlanCellRef)
            {
                return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(
                    aPlanCellRef[eProfileType.Store],
                    aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key) == eStoreStatus.New;
            }

            protected override bool isTypeOK(PlanCellReference aPlanCellRef)
            {
                return true;
            }
        }
		// End TT#2054  


		protected class clsInit_AvgStoreSalesTotalUnits : clsFormula_AverageValue
		{
			public clsInit_AvgStoreSalesTotalUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreSalesTotalUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, BasePlanToolBox.CalculateNonZeroAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, BasePlanVariables.InventoryTotalUnits));

						return eComputationFormulaReturnType.Successful;
					}
					else
					{
						return base.Execute(aSchdEntry, aGetCellMode, aSetCellMode);
					}
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_AvgStoreSalesRegularUnits : clsFormula_AverageValue
		{
			public clsInit_AvgStoreSalesRegularUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreSalesRegularUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, BasePlanToolBox.CalculateNonZeroAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, BasePlanVariables.InventoryRegularUnits));

						return eComputationFormulaReturnType.Successful;
					}
					else
					{
						return base.Execute(aSchdEntry, aGetCellMode, aSetCellMode);
					}
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_AvgStoreSalesMarkdownUnits : clsFormula_AverageValue
		{
			public clsInit_AvgStoreSalesMarkdownUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreSalesMarkdownUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, BasePlanToolBox.CalculateNonZeroAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, BasePlanVariables.InventoryMarkdownUnits));

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_AvgStoreInventoryTotalUnits : clsFormula_AverageValue
		{
			public clsInit_AvgStoreInventoryTotalUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreInventoryTotalUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, BasePlanToolBox.CalculateNonZeroAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, null));

						return eComputationFormulaReturnType.Successful;
					}
					else
					{
						return base.Execute(aSchdEntry, aGetCellMode, aSetCellMode);
					}
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_AvgStoreInventoryRegularUnits : clsFormula_AverageValue
		{
			public clsInit_AvgStoreInventoryRegularUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreInventoryRegularUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					if (BasePlanToolBox.isActual(aSchdEntry.PlanCellRef))
					{
						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, BasePlanToolBox.CalculateNonZeroAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, null));

						return eComputationFormulaReturnType.Successful;
					}
					else
					{
						return base.Execute(aSchdEntry, aGetCellMode, aSetCellMode);
					}
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_AvgStoreInventoryMarkdownUnits : clsFormula_AverageValue
		{
			public clsInit_AvgStoreInventoryMarkdownUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreInventoryMarkdownUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, BasePlanToolBox.CalculateNonZeroAverage(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, null));

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_AvgStoreSalesTotalUnits_LowLevelTotal : clsFormula_AverageAlternateDetail_LowLevelTotal
		{
			public clsInit_AvgStoreSalesTotalUnits_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreSalesTotalUnits_LowLevelTotal Init")
			{
			}

			override protected eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			override protected VariableProfile InventoryVariableProfile
			{
				get
				{
					return BasePlanVariables.InventoryTotalUnits;
				}
			}

			override protected bool CalculateNonZeroAverage(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isActual(aPlanCellRef);
			}

			override protected bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			override protected eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.None;
				}
			}
		}

		protected class clsInit_AvgStoreSalesRegularUnits_LowLevelTotal : clsFormula_AverageAlternateDetail_LowLevelTotal
		{
			public clsInit_AvgStoreSalesRegularUnits_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreSalesRegularUnits_LowLevelTotal Init")
			{
			}

			override protected eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			override protected VariableProfile InventoryVariableProfile
			{
				get
				{
					return BasePlanVariables.InventoryRegularUnits;
				}
			}

			override protected bool CalculateNonZeroAverage(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isActual(aPlanCellRef);
			}

			override protected bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			override protected eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.None;
				}
			}
		}

		protected class clsInit_AvgStoreSalesMarkdownUnits_LowLevelTotal : clsFormula_AverageAlternateDetail_LowLevelTotal
		{
			public clsInit_AvgStoreSalesMarkdownUnits_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreSalesMarkdownUnits_LowLevelTotal Init")
			{
			}

			override protected eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			override protected VariableProfile InventoryVariableProfile
			{
				get
				{
					return BasePlanVariables.InventoryMarkdownUnits;
				}
			}

			override protected bool CalculateNonZeroAverage(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			override protected bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			override protected eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.None;
				}
			}
		}

		protected class clsInit_AvgStoreInventoryTotalUnits_LowLevelTotal : clsFormula_AverageAlternateDetail_LowLevelTotal
		{
			public clsInit_AvgStoreInventoryTotalUnits_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreInventoryTotalUnits_LowLevelTotal Init")
			{
			}

			override protected eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			override protected VariableProfile InventoryVariableProfile
			{
				get
				{
					return null;
				}
			}

			override protected bool CalculateNonZeroAverage(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isActual(aPlanCellRef);
			}

			override protected bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			override protected eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.None;
				}
			}
		}

		protected class clsInit_AvgStoreInventoryRegularUnits_LowLevelTotal : clsFormula_AverageAlternateDetail_LowLevelTotal
		{
			public clsInit_AvgStoreInventoryRegularUnits_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreInventoryRegularUnits_LowLevelTotal Init")
			{
			}

			override protected eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			override protected VariableProfile InventoryVariableProfile
			{
				get
				{
					return null;
				}
			}

			override protected bool CalculateNonZeroAverage(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isActual(aPlanCellRef);
			}

			override protected bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			override protected eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.None;
				}
			}
		}

		protected class clsInit_AvgStoreInventoryMarkdownUnits_LowLevelTotal : clsFormula_AverageAlternateDetail_LowLevelTotal
		{
			public clsInit_AvgStoreInventoryMarkdownUnits_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreInventoryMarkdownUnits_LowLevelTotal Init")
			{
			}

			override protected eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			override protected VariableProfile InventoryVariableProfile
			{
				get
				{
					return null;
				}
			}

			override protected bool CalculateNonZeroAverage(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			override protected bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			override protected eStoreStatus StoreStatus
			{
				get
				{
					return eStoreStatus.None;
				}
			}
		}

		protected class clsInit_AvgStoreSalesTotalUnitsT3 : clsFormula_AverageAlternateDetail
		{
			public clsInit_AvgStoreSalesTotalUnitsT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreSalesTotalUnitsT3 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_AvgStoreSalesRegularUnitsT2 : clsFormula_AverageAlternateDetail
		{
			public clsInit_AvgStoreSalesRegularUnitsT2(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreSalesRegularUnitsT2 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_AvgStoreSalesPromoUnitsT2 : clsFormula_AverageAlternateDetail
		{
			public clsInit_AvgStoreSalesPromoUnitsT2(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreSalesPromoUnitsT2 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_AvgStoreSalesRegPromoUnitsT3 : clsFormula_AverageAlternateDetail
		{
			public clsInit_AvgStoreSalesRegPromoUnitsT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreSalesRegPromoUnitsT3 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_AvgStoreSalesMarkdownUnitsT3 : clsFormula_AverageAlternateDetail
		{
			public clsInit_AvgStoreSalesMarkdownUnitsT3(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreSalesMarkdownUnitsT3 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_AvgStoreInventoryTotalUnitsT4 : clsFormula_AverageAlternateDetail
		{
			public clsInit_AvgStoreInventoryTotalUnitsT4(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreInventoryTotalUnitsT4 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}
		}

		protected class clsInit_AvgStoreInventoryTotalUnitsT5 : clsFormula_AverageAlternateDetail
		{
			public clsInit_AvgStoreInventoryTotalUnitsT5(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreInventoryTotalUnitsT5 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_AvgStoreInventoryTotalUnitsT6 : clsFormula_AverageAlternateDetail
		{
			public clsInit_AvgStoreInventoryTotalUnitsT6(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreInventoryTotalUnitsT6 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}
		}

        // TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
        protected class clsInit_AvgStoreInventoryTotalUnitsT8 : clsFormula_AverageAlternateDetail
        {
            public clsInit_AvgStoreInventoryTotalUnitsT8(BasePlanComputations aBasePlanComputations, int aKey)
                : base(aBasePlanComputations, aKey, "AvgStoreInventoryTotalUnitsT8 Init")
            {
            }

            protected override eCubeType CubeType
            {
                get
                {
                    return eCubeType.StoreDetail;
                }
            }

            protected override bool isStatusOK(PlanCellReference aPlanCellRef)
            {
                return true;
            }

            protected override bool isTypeOK(PlanCellReference aPlanCellRef)
            {
                return true;
            }
        }
        // End TT#2054

		protected class clsInit_AvgStoreInventoryRegularUnitsT4 : clsFormula_AverageAlternateDetail
		{
			public clsInit_AvgStoreInventoryRegularUnitsT4(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreInventoryRegularUnitsT4 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}
		}

		protected class clsInit_AvgStoreInventoryRegularUnitsT5 : clsFormula_AverageAlternateDetail
		{
			public clsInit_AvgStoreInventoryRegularUnitsT5(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreInventoryRegularUnitsT5 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_AvgStoreInventoryRegularUnitsT6 : clsFormula_AverageAlternateDetail
		{
			public clsInit_AvgStoreInventoryRegularUnitsT6(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AvgStoreInventoryRegularUnitsT6 Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.StoreDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

        // TT#2054 - RMatelic - Add VSW On Hand to OTS Forecast Review Totals
        protected class clsInit_AvgStoreInventoryRegularUnitsT8 : clsFormula_AverageAlternateDetail
        {
            public clsInit_AvgStoreInventoryRegularUnitsT8(BasePlanComputations aBasePlanComputations, int aKey)
                : base(aBasePlanComputations, aKey, "AvgStoreInventoryRegularUnitsT8 Init")
            {
            }

            protected override eCubeType CubeType
            {
                get
                {
                    return eCubeType.StoreDetail;
                }
            }

            protected override bool isStatusOK(PlanCellReference aPlanCellRef)
            {
                return true;
            }

            protected override bool isTypeOK(PlanCellReference aPlanCellRef)
            {
                return true;
            }
        }
        // End TT#2054

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Period Inits
		//-------------------------------------------

		protected class clsInit_SumWeekDetail : clsFormula_SumAlternateDetail
		{
			public clsInit_SumWeekDetail(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumWeekDetail Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.WeekDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}
		}

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Low-level Inits
		//-------------------------------------------
		
		protected class clsInit_SumLowLevelTotalDetail : clsFormula_SumAlternateDetail
		{
			public clsInit_SumLowLevelTotalDetail(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumLowLevelTotalDetail Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.LowLevelDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isActual(aPlanCellRef) || BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumLowLevelRegularDetail : clsFormula_SumAlternateDetail
		{
			public clsInit_SumLowLevelRegularDetail(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumLowLevelRegularDetail Init")
			{
			}

			protected override eCubeType CubeType
			{
				get
				{
					return eCubeType.LowLevelDetail;
				}
			}

			protected override bool isStatusOK(PlanCellReference aPlanCellRef)
			{
				return true;
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isActual(aPlanCellRef) || BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}
		}

		protected class clsInit_SumLowLevelInventoryTotalUnitsT7 : clsFormula_SumLowLevelPctNeed
		{
			public clsInit_SumLowLevelInventoryTotalUnitsT7(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumLowLevelInventoryTotalUnitsT7 Init")
			{
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}

			protected override TimeTotalVariableProfile NeedVariable
			{
				get
				{
					return BasePlanTimeTotalVariables.InventoryTotalUnitsT6;
				}
			}
		}

		protected class clsInit_SumLowLevelInventoryRegularUnitsT7 : clsFormula_SumLowLevelPctNeed
		{
			public clsInit_SumLowLevelInventoryRegularUnitsT7(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumLowLevelInventoryRegularUnitsT7 Init")
			{
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}

			protected override TimeTotalVariableProfile NeedVariable
			{
				get
				{
					return BasePlanTimeTotalVariables.InventoryRegularUnitsT6;
				}
			}
		}

		protected class clsInit_SumLowLevelStoreInventoryTotalUnitsT7 : clsFormula_SumLowLevelStorePctNeed
		{
			public clsInit_SumLowLevelStoreInventoryTotalUnitsT7(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumLowLevelStoreInventoryTotalUnitsT7 Init")
			{
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isTotalPlanType(aPlanCellRef);
			}

			protected override TimeTotalVariableProfile NeedVariable
			{
				get
				{
					return BasePlanTimeTotalVariables.InventoryTotalUnitsT6;
				}
			}
		}

		protected class clsInit_SumLowLevelStoreInventoryRegularUnitsT7 : clsFormula_SumLowLevelStorePctNeed
		{
			public clsInit_SumLowLevelStoreInventoryRegularUnitsT7(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SumLowLevelStoreInventoryRegularUnitsT7 Init")
			{
			}

			protected override bool isTypeOK(PlanCellReference aPlanCellRef)
			{
				return BasePlanToolBox.isRegularPlanType(aPlanCellRef);
			}

			protected override TimeTotalVariableProfile NeedVariable
			{
				get
				{
					return BasePlanTimeTotalVariables.InventoryRegularUnitsT6;
				}
			}
		}

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Comparative Inits
		//-------------------------------------------

		protected class clsInit_PctChange : PlanFormulaProfile
		{
			public clsInit_PctChange(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "PctChange Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				PlanCellReference basisCellRef;
				double newValue;
				double basisValue;

				try
				{
					basisCellRef = BasePlanToolBox.GetBasisOperandCellForPctChange(aSchdEntry, aSetCellMode, aSchdEntry.PlanCellRef);

					if (!basisCellRef.isCellNull)
					{
						basisValue = basisCellRef.GetCellValue(aGetCellMode, aSchdEntry.PlanCellRef.isCellHidden);
						newValue = ((BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden) - basisValue) * 100) / basisValue;

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_PctToSet : PlanFormulaProfile
		{
			public clsInit_PctToSet(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "PctToSet Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;
				double setValue;

				try
				{
					setValue = BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden);
					newValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden) / setValue * 100;
					BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_PctToAll : PlanFormulaProfile
		{
			public clsInit_PctToAll(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "PctToAll Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;
				double AllValue;

				try
				{
					AllValue = BasePlanToolBox.GetTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden);
					newValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden) / AllValue * 100;
					BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_PctToLowLevel : PlanFormulaProfile
		{
			public clsInit_PctToLowLevel(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "PctToLowLevel Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					newValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden) /
						BasePlanToolBox.GetLowLevelTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, aSchdEntry.PlanCellRef.isCellHidden) * 100;
					
					BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_PctToTimePeriod : PlanFormulaProfile
		{
			public clsInit_PctToTimePeriod(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "PctToTimePeriod Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;
				double timeValue;
				VariableProfile varProf;
				TimeTotalVariableProfile timeTotVarProf;

				try
				{
					varProf = (VariableProfile)BasePlanVariables.VariableProfileList.FindKey(aSchdEntry.PlanCellRef[eProfileType.Variable]);
					timeTotVarProf = varProf.TotalTimeTotalVariableProfile;

					if (timeTotVarProf != null)
					{
						timeValue = BasePlanToolBox.GetTimeTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, timeTotVarProf, aSchdEntry.PlanCellRef.isCellHidden);
						newValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden) / timeValue * 100;
						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_ChainBalance : PlanFormulaProfile
		{
			public clsInit_ChainBalance(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "ChainBalance Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					newValue = BasePlanToolBox.GetHighLevelChainOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden) -
						BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden);
					BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_StoreBalance : PlanFormulaProfile
		{
			public clsInit_StoreBalance(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "StoreBalance Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					newValue = BasePlanToolBox.GetHighLevelStoreOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden) -
						BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden);
					BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_Difference : PlanFormulaProfile
		{
			public clsInit_Difference(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "Difference Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					newValue = BasePlanToolBox.GetStoreTotalOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden) -
						BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden);
					BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsInit_PctChangeToPlan : PlanFormulaProfile
		{
			public clsInit_PctChangeToPlan(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "PctChangeToPlan Init")
			{
			}

			override public eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				PlanCellReference planCellRef;
				double newValue;
				double planValue;
				double basisValue;

				try
				{
					planCellRef = BasePlanToolBox.GetPlanOperandCellForPctChange(aSchdEntry, aSetCellMode, aSchdEntry.PlanCellRef);

					if (!planCellRef.isCellNull)
					{
						planValue = planCellRef.GetCellValue(aGetCellMode, aSchdEntry.PlanCellRef.isCellHidden);
						basisValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, BasePlanQuantityVariables.Value, aSchdEntry.PlanCellRef.isCellHidden);
						newValue = ((planValue - basisValue) * 100) / basisValue;

						BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
					}
					else
					{
						BasePlanToolBox.SetCellNull(aSchdEntry.PlanCellRef);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Change Formulas
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Time Total Change Formulas
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------
	}
}
