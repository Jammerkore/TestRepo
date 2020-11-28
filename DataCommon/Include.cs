using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;           // MID Track 4033 allow multi header children to change
using System.Windows;
using System.Windows.Forms;
using Microsoft.Win32;
using C1.Win.C1FlexGrid;
//Begin TT#1281 - JScott - WUB header load failed
using System.Text.RegularExpressions;
using System.IO;
//End TT#1281 - JScott - WUB header load failed



namespace MIDRetail.DataCommon
{
	/// <summary>
	/// Summary description for Include.
	/// </summary>
	/// 
	public class Include
	{
		//=======
		// FIELDS
		//=======

        // Used for testing with conditional break points.
        public const int FocusStoreRID = 87;

		// some basic error messages that can't be put in the APPLICATION_TEXT table
		// because, hey, we can't even get to the database
		public const string ErrorSystemError	= "System Error";
		public const string ErrorBadConfigFile	= "ERROR 020002 - Bad or missing application config file: ";
		public const string ErrorBadTextCode	= "Bad or missing application text code: ";
        public const string ErrorDatabase = "The following database errors occurred:\r\n"; // TT#3435 - JSmith - Errors messages not complete with encounter database errors.
		public const string NoSecondarySize		= "No Secondary Size";
		public const string NoneText			= "None";  // MID Track #3942

		// some other basic constants
//		public const string MIDGraphicsDir = "\\..\\Graphics"; 
//		public const string MIDFolder = "*.*folder.gif";
//		public const string MIDOpenFolder = ".openfolder.gif";
//		public const string MIDClosedFolder = ".closedfolder.gif";
//		public const string MIDDefaultClosedFolder = "default.closedfolder.gif";
//		public const string MIDDefaultOpenFolder = "default.openfolder.gif";
//		public const string MIDUpArrow = "UpArrow.gif";
//		public const string MIDDownArrow = "DownArrow.gif";
//		public const string MIDCalendarImage = "calendar.bmp";
//		public const string MIDStoreImage = "store.bmp";
//		public const string MIDErrorImage = "Error.ico";
//		public const string MIDReoccurringImage = "dynamic.gif";
		public const string MIDDefaultColor = "default";
		public const string MyHierarchyName = "My Hierarchy";
//		public const string MIDInheritanceImage = "Inheritance.ico";
//		public const string MIDStyleImage = "Style.bmp";
		public const string MIDUserNameFile = "MIDUserName.txt";
		public const string MIDDatabaseUpdateFile = "MIDDatabaseUpdate.txt";
		// BEGIN MID Track #5170 - JSmith - Model enhancements
		//public const string CaseInsensitiveCollation = " collate SQL_Latin1_General_CP1_CI_AS ";
		// End MID Track #5170

		// XML contants
		public const string XML_Version = "<?xml version=\"1.0\"?> ";

		// history/plan load bulk insert constants
		//public const string BIStoreWeeklyHistoryFile = "StoreWeeklyHistory";
		//public const string BIStoreDailyHistoryFile = "StoreDailyHistory";
		//public const string BIStoreWeeklyForecastFile = "StoreWeeklyForecast";
		//public const string BIStoreIntransitFile = "StoreIntransit";
		//public const string BIChainDailyHistoryFile = "ChainDailyHistory";
		//public const string BIChainWeeklyHistoryFile = "ChainWeeklyHistory";
		//public const string BIChainWeeklyForecastFile = "ChainWeeklyForecast";
		
		// {#} is substituted with the correct number associated with the node RID
		public const string DBTableCountReplaceString = "{#}";

		// database view names
		public const string DBStoreWeeklyHistoryView = "VW_STORE_HISTORY_WEEK";
		public const string DBStoreDailyHistoryView = "VW_STORE_HISTORY_DAY";
		public const string DBStoreWeeklyForecastView = "VW_STORE_FORECAST_WEEK";
		public const string DBStoreExternalIntransitView = "VW_STORE_EXTERNAL_INTRANSIT";
		public const string DBChainDailyHistoryView = "VW_CHAIN_HISTORY_DAY";
		public const string DBChainWeeklyHistoryView = "VW_CHAIN_HISTORY_WEEK";
		public const string DBChainWeeklyForecastView = "VW_CHAIN_FORECAST_WEEK";

		//Begin Track #4637 - JSmith - Split variables by type
		public const string cLockExtension = "_LOCK";

		public const string DBStoreWeeklyAllHistoryView = "VW_STORE_ALL_HISTORY_WEEK";
		public const string DBStoreDailyAllHistoryView = "VW_STORE_ALL_HISTORY_DAY";
		public const string DBStoreWeeklyAllForecastView = "VW_STORE_ALL_FORECAST_WEEK";
		public const string DBChainDailyAllHistoryView = "VW_CHAIN_ALL_HISTORY_DAY";
		public const string DBChainWeeklyAllHistoryView = "VW_CHAIN_ALL_HISTORY_WEEK";
		public const string DBChainWeeklyAllForecastView = "VW_CHAIN_ALL_FORECAST_WEEK";

		// Style view names
		public const string DBStoreWeeklyStyleHistoryView = "VW_STORE_STYLE_HISTORY_WEEK";
		public const string DBStoreDailyStyleHistoryView = "VW_STORE_STYLE_HISTORY_DAY";
		public const string DBStoreWeeklyStyleForecastView = "VW_STORE_STYLE_FORECAST_WEEK";
		public const string DBChainDailyStyleHistoryView = "VW_CHAIN_STYLE_HISTORY_DAY";
		public const string DBChainWeeklyStyleHistoryView = "VW_CHAIN_STYLE_HISTORY_WEEK";
		public const string DBChainWeeklyStyleForecastView = "VW_CHAIN_STYLE_FORECAST_WEEK";

		// Color view names
		public const string DBStoreWeeklyColorHistoryView = "VW_STORE_COLOR_HISTORY_WEEK";
		public const string DBStoreDailyColorHistoryView = "VW_STORE_COLOR_HISTORY_DAY";
		public const string DBStoreWeeklyColorForecastView = "VW_STORE_COLOR_FORECAST_WEEK";
		public const string DBChainDailyColorHistoryView = "VW_CHAIN_COLOR_HISTORY_DAY";
		public const string DBChainWeeklyColorHistoryView = "VW_CHAIN_COLOR_HISTORY_WEEK";
		public const string DBChainWeeklyColorForecastView = "VW_CHAIN_COLOR_FORECAST_WEEK";

		// Size view names
		public const string DBStoreWeeklySizeHistoryView = "VW_STORE_SIZE_HISTORY_WEEK";
		public const string DBStoreDailySizeHistoryView = "VW_STORE_SIZE_HISTORY_DAY";
		public const string DBStoreWeeklySizeForecastView = "VW_STORE_SIZE_FORECAST_WEEK";
		public const string DBChainDailySizeHistoryView = "VW_CHAIN_SIZE_HISTORY_DAY";
		public const string DBChainWeeklySizeHistoryView = "VW_CHAIN_SIZE_HISTORY_WEEK";
		public const string DBChainWeeklySizeForecastView = "VW_CHAIN_SIZE_FORECAST_WEEK";
		//End Track #4637

		// database table names
		public const string DBStoreWeeklyLockTable = "STORE_FORECAST_WEEK_LOCK";
		public const string DBChainWeeklyLockTable = "CHAIN_FORECAST_WEEK_LOCK";

		public const string DBStoreWeeklyHistoryTable = "STORE_HISTORY_WEEK{#}";
		public const string DBStoreDailyHistoryTable = "STORE_HISTORY_DAY{#}";
		public const string DBStoreWeeklyForecastTable = "STORE_FORECAST_WEEK{#}";
		public const string DBStoreExternalIntransitTable = "STORE_EXTERNAL_INTRANSIT";
		public const string DBChainDailyHistoryTable = "CHAIN_HISTORY_DAY";
		public const string DBChainWeeklyHistoryTable = "CHAIN_HISTORY_WEEK";
		public const string DBChainWeeklyForecastTable = "CHAIN_FORECAST_WEEK";

		//Begin Track #4637 - JSmith - Split variables by type
		// Style tables
		public const string DBStoreWeeklyStyleHistoryTable = "STORE_STYLE_HISTORY_WEEK{#}";
		public const string DBStoreDailyStyleHistoryTable = "STORE_STYLE_HISTORY_DAY{#}";
		public const string DBStoreWeeklyStyleForecastTable = "STORE_STYLE_FORECAST_WEEK{#}";
		public const string DBChainDailyStyleHistoryTable = "CHAIN_STYLE_HISTORY_DAY";
		public const string DBChainWeeklyStyleHistoryTable = "CHAIN_STYLE_HISTORY_WEEK";
		public const string DBChainWeeklyStyleForecastTable = "CHAIN_STYLE_FORECAST_WEEK";

		// Color tables
		public const string DBStoreWeeklyColorHistoryTable = "STORE_COLOR_HISTORY_WEEK{#}";
		public const string DBStoreDailyColorHistoryTable = "STORE_COLOR_HISTORY_DAY{#}";
		public const string DBStoreWeeklyColorForecastTable = "STORE_COLOR_FORECAST_WEEK{#}";
		public const string DBChainDailyColorHistoryTable = "CHAIN_COLOR_HISTORY_DAY";
		public const string DBChainWeeklyColorHistoryTable = "CHAIN_COLOR_HISTORY_WEEK";
		public const string DBChainWeeklyColorForecastTable = "CHAIN_COLOR_FORECAST_WEEK";

		// Size tables
		public const string DBStoreWeeklySizeHistoryTable = "STORE_SIZE_HISTORY_WEEK{#}";
		public const string DBStoreDailySizeHistoryTable = "STORE_SIZE_HISTORY_DAY{#}";
		public const string DBStoreWeeklySizeForecastTable = "STORE_SIZE_FORECAST_WEEK{#}";
		public const string DBChainDailySizeHistoryTable = "CHAIN_SIZE_HISTORY_DAY";
		public const string DBChainWeeklySizeHistoryTable = "CHAIN_SIZE_HISTORY_WEEK";
		public const string DBChainWeeklySizeForecastTable = "CHAIN_SIZE_FORECAST_WEEK";

		// database read stored procedure names
		public const string DBStoreWeeklyHistoryReadSP = "SP_MID_ST_HIS_WK{#}_READ";
		public const string DBStoreDailyHistoryReadSP = "SP_MID_ST_HIS_DAY{#}_READ";
        public const string DBStoreWeeklyForecastReadSP = "SP_MID_ST_FOR_WK{#}_READ";  // TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables
        public const string DBStoreWeeklyForecastLockReadSP = "SP_MID_ST_FOR_WK_LOCK_READ";  // TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables
		public const string DBStoreWeeklyModVerReadSP = "SP_MID_ST_MOD_WK{#}_READ";
		public const string DBChainDailyHistoryReadSP = "SP_MID_CHN_HIS_DAY_READ";
		public const string DBChainWeeklyHistoryReadSP = "SP_MID_CHN_HIS_WK_READ";
        public const string DBChainWeeklyForecastReadSP = "SP_MID_CHN_FOR_WK_READ";  // TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables
        public const string DBChainWeeklyForecastLockReadSP = "SP_MID_CHN_FOR_WK_LOCK_READ";  // TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables
		public const string DBChainWeeklyModVerReadSP = "SP_MID_CHN_MOD_WK_READ";

		// Style read stored procedure names
		public const string DBStoreWeeklyStyleHistoryReadSP = "SP_MID_ST_STY_HIS_WK{#}_READ";
		public const string DBStoreDailyStyleHistoryReadSP = "SP_MID_ST_STY_HIS_DAY{#}_READ";
		public const string DBStoreWeeklyStyleModVerReadSP = "SP_MID_ST_STY_MOD_WK{#}_READ";
		public const string DBChainDailyStyleHistoryReadSP = "SP_MID_CHN_STY_HIS_DAY_READ";
		public const string DBChainWeeklyStyleHistoryReadSP = "SP_MID_CHN_STY_HIS_WK_READ";
		public const string DBChainWeeklyStyleModVerReadSP = "SP_MID_CHN_STY_MOD_WK_READ";

		// Color read stored procedure names
		public const string DBStoreWeeklyColorHistoryReadSP = "SP_MID_ST_CLR_HIS_WK{#}_READ";
		public const string DBStoreDailyColorHistoryReadSP = "SP_MID_ST_CLR_HIS_DAY{#}_READ";
		public const string DBStoreWeeklyColorModVerReadSP = "SP_MID_ST_CLR_MOD_WK{#}_READ";
		public const string DBChainDailyColorHistoryReadSP = "SP_MID_CHN_CLR_HIS_DAY_READ";
		public const string DBChainWeeklyColorHistoryReadSP = "SP_MID_CHN_CLR_HIS_WK_READ";
		public const string DBChainWeeklyColorModVerReadSP = "SP_MID_CHN_CLR_MOD_WK_READ";

		// Size read stored procedure names
		public const string DBStoreWeeklySizeHistoryReadSP = "SP_MID_ST_SZ_HIS_WK{#}_READ";
		public const string DBStoreDailySizeHistoryReadSP = "SP_MID_ST_SZ_HIS_DAY{#}_READ";
		public const string DBStoreWeeklySizeModVerReadSP = "SP_MID_ST_SZ_MOD_WK{#}_READ";
		public const string DBChainDailySizeHistoryReadSP = "SP_MID_CHN_SZ_HIS_DAY_READ";
		public const string DBChainWeeklySizeHistoryReadSP = "SP_MID_CHN_SZ_HIS_WK_READ";
		public const string DBChainWeeklySizeModVerReadSP = "SP_MID_CHN_SZ_MOD_WK_READ";
		//Begin TT#155 - JScott - Size Curve Method
        //public const string DBStoreHistorySizesReadSP = "SP_MID_ST_HIS_SIZES_READ"; //TT#3445 -jsobek -Failure of Size Curve Generation
		//End TT#155 - JScott - Size Curve Method

        //Begin TT#739-MD -jsobek -Delete Stores
        //public const string DBStoreHistoryGetSizeDataFromNodeDay = "UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE_DAY{#}"; //TT#3456 -jsobek -Size Day To Week Failure
        //public const string DBStoreHistoryGetSizeDataFromNode = "UDF_STORE_HISTORY_DAY_GET_DATA_IN_TIME_PERIOD_FROM_NODE"; //TT#3456 -jsobek -Size Day To Week Failure
        public const string DBStoreHistoryGetSizeDataFromNode = "UDF_STORE_HISTORY_DAY_GET_DATA_IN_TIME_PERIOD_FROM_NODE"; //TT#3486 - JSmith - Create new database failed
        public const string DBSizeCurveInsertSummary = "MID_SIZE_CURVE_INSERT_SUMMARY_DATA_IN_TIME_PERIOD_FROM_NODE";
        //End TT#739-MD -jsobek -Delete Stores

        // Begin TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables
        // database type names
        public const string DBStoreWeeklyHistoryType = "MID_ST_HIS_WK_TYPE";
        public const string DBStoreDailyHistoryType = "MID_ST_HIS_DAY_TYPE";
        public const string DBStoreWeeklyForecastType = "MID_ST_FOR_WK_TYPE";
        public const string DBStoreWeeklyForecastLockType = "MID_ST_FOR_WK_LOCK_TYPE";
        public const string DBChainDailyHistoryType = "MID_CHN_HIS_DAY_TYPE";
        public const string DBChainWeeklyHistoryType = "MID_CHN_HIS_WK_TYPE";
        public const string DBChainWeeklyForecastType = "MID_CHN_FOR_WK_TYPE";
        public const string DBChainWeeklyForecastLockType = "MID_CHN_FOR_WK_LOCK_TYPE";

        public const string DBChainWeeklyHistoryReadType = "MID_CHN_HIS_WK_READ_TYPE";
        public const string DBChainWeeklyModifiedReadType = "MID_CHN_MOD_WK_READ_TYPE";
        public const string DBChainWeeklyForecastReadType = "MID_CHN_FOR_WK_READ_TYPE";
        public const string DBChainWeeklyForecastReadLockType = "MID_CHN_FOR_WK_READ_LOCK_TYPE";
        public const string DBStoreWeeklyHistoryReadType = "MID_ST_HIS_WK_READ_TYPE";
        public const string DBStoreDailyHistoryReadType = "MID_ST_HIS_DAY_READ_TYPE";
        public const string DBStoreWeeklyModifiedReadType = "MID_ST_MOD_WK_READ_TYPE";
        public const string DBStoreWeeklyForecastReadType = "MID_ST_FOR_WK_READ_TYPE";
        public const string DBStoreWeeklyForecastReadLockType = "MID_ST_FOR_WK_READ_LOCK_TYPE";
        // End TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables

        public const string DBGetTableFromType = "SP_MID_GET_TABLE_FROM_TYPE";  // TT#3373 - JSmith - Save Store Forecast receive DBNull error

		// database write stored procedure names
		public const string DBStoreWeeklyHistoryWriteSP = "SP_MID_ST_HIS_WK{#}_WRITE";
		public const string DBStoreDailyHistoryWriteSP = "SP_MID_ST_HIS_DAY{#}_WRITE";
		public const string DBStoreWeeklyForecastWriteSP = "SP_MID_ST_FOR_WK{#}_WRITE";
        public const string DBStoreWeeklyForecastLockWriteSP = "SP_MID_ST_FOR_WK_LOCK_WRITE"; // TT#3373 - JSmith - Save Store Forecast receive DBNull error
		public const string DBChainDailyHistoryWriteSP = "SP_MID_CHN_HIS_DAY_WRITE";
		public const string DBChainWeeklyHistoryWriteSP = "SP_MID_CHN_HIS_WK_WRITE";
		public const string DBChainWeeklyForecastWriteSP = "SP_MID_CHN_FOR_WK_WRITE";

		// base write stored procedure names
		public const string DBStoreWeeklyBaseHistoryWriteSP = "SP_MID_ST_BASE_HIS_WK{#}_WRITE";
		public const string DBStoreDailyBaseHistoryWriteSP = "SP_MID_ST_BASE_HIS_DAY{#}_WRITE";
		public const string DBStoreWeeklyBaseForecastWriteSP = "SP_MID_ST_BASE_FOR_WK{#}_WRITE";
		public const string DBChainDailyBaseHistoryWriteSP = "SP_MID_CHN_BASE_HIS_DAY_WRITE";
		public const string DBChainWeeklyBaseHistoryWriteSP = "SP_MID_CHN_BASE_HIS_WK_WRITE";
		public const string DBChainWeeklyBaseForecastWriteSP = "SP_MID_CHN_BASE_FOR_WK_WRITE";

		// Style write stored procedure names
		public const string DBStoreWeeklyStyleHistoryWriteSP = "SP_MID_ST_STY_HIS_WK{#}_WRITE";
		public const string DBStoreDailyStyleHistoryWriteSP = "SP_MID_ST_STY_HIS_DAY{#}_WRITE";
		public const string DBStoreWeeklyStyleForecastWriteSP = "SP_MID_ST_STY_FOR_WK{#}_WRITE";
		public const string DBChainDailyStyleHistoryWriteSP = "SP_MID_CHN_STY_HIS_DAY_WRITE";
		public const string DBChainWeeklyStyleHistoryWriteSP = "SP_MID_CHN_STY_HIS_WK_WRITE";
		public const string DBChainWeeklyStyleForecastWriteSP = "SP_MID_CHN_STY_FOR_WK_WRITE";

		// Color write stored procedure names
		public const string DBStoreWeeklyColorHistoryWriteSP = "SP_MID_ST_CLR_HIS_WK{#}_WRITE";
		public const string DBStoreDailyColorHistoryWriteSP = "SP_MID_ST_CLR_HIS_DAY{#}_WRITE";
		public const string DBStoreWeeklyColorForecastWriteSP = "SP_MID_ST_CLR_FOR_WK{#}_WRITE";
		public const string DBChainDailyColorHistoryWriteSP = "SP_MID_CHN_CLR_HIS_DAY_WRITE";
		public const string DBChainWeeklyColorHistoryWriteSP = "SP_MID_CHN_CLR_HIS_WK_WRITE";
		public const string DBChainWeeklyColorForecastWriteSP = "SP_MID_CHN_CLR_FOR_WK_WRITE";

		// Size write stored procedure names
		public const string DBStoreWeeklySizeHistoryWriteSP = "SP_MID_ST_SZ_HIS_WK{#}_WRITE";
		public const string DBStoreDailySizeHistoryWriteSP = "SP_MID_ST_SZ_HIS_DAY{#}_WRITE";
		public const string DBStoreWeeklySizeForecastWriteSP = "SP_MID_ST_SZ_FOR_WK{#}_WRITE";
		public const string DBChainDailySizeHistoryWriteSP = "SP_MID_CHN_SZ_HIS_DAY_WRITE";
		public const string DBChainWeeklySizeHistoryWriteSP = "SP_MID_CHN_SZ_HIS_WK_WRITE";
		public const string DBChainWeeklySizeForecastWriteSP = "SP_MID_CHN_SZ_FOR_WK_WRITE";

		// Delete zero rows stored procedure names
		public const string DBStoreWeeklySizeForecastDelZeroSP = "SP_MID_ST_SZ_FOR_WK{#}_DEL_ZERO";
		public const string DBStoreWeeklyColorForecastDelZeroSP = "SP_MID_ST_CLR_FOR_WK{#}_DEL_ZERO";
		public const string DBStoreWeeklyStyleForecastDelZeroSP = "SP_MID_ST_STY_FOR_WK{#}_DEL_ZERO";
		public const string DBStoreWeeklyBaseForecastDelZeroSP = "SP_MID_ST_BASE_FOR_WK{#}_DEL_ZERO";
		public const string DBStoreWeeklyForecastDelZeroSP = "SP_MID_ST_FOR_WK{#}_DEL_ZERO";
		public const string DBChainWeeklySizeForecastDelZeroSP = "SP_MID_CHN_SZ_FOR_WK_DEL_ZERO";
		public const string DBChainWeeklyColorForecastDelZeroSP = "SP_MID_CHN_CLR_FOR_WK_DEL_ZERO";
		public const string DBChainWeeklyStyleForecastDelZeroSP = "SP_MID_CHN_STY_FOR_WK_DEL_ZERO";
		public const string DBChainWeeklyBaseForecastDelZeroSP = "SP_MID_CHN_BASE_FOR_WK_DEL_ZERO";
		public const string DBChainWeeklyForecastDelZeroSP = "SP_MID_CHN_FOR_WK_DEL_ZERO";
		
		// Delete unlocked rows stored procedure names
		public const string DBStoreWeeklySizeForecastDelUnlockedSP = "SP_MID_ST_SZ_FOR_WK_DEL_UNLK";
		public const string DBStoreWeeklyColorForecastDelUnlockedSP = "SP_MID_ST_CLR_FOR_WK_DEL_UNLK";
		public const string DBStoreWeeklyStyleForecastDelUnlockedSP = "SP_MID_ST_STY_FOR_WK_DEL_UNLK";
		public const string DBStoreWeeklyBaseForecastDelUnlockedSP = "SP_MID_ST_BASE_FOR_WK_DEL_UNLK";
		public const string DBStoreWeeklyForecastDelUnlockedSP = "SP_MID_ST_FOR_WK_DEL_UNLK";
		public const string DBChainWeeklySizeForecastDelUnlockedSP = "SP_MID_CHN_SZ_FOR_WK_DEL_UNLK";
		public const string DBChainWeeklyColorForecastDelUnlockedSP = "SP_MID_CHN_CLR_FOR_WK_DEL_UNLK";
		public const string DBChainWeeklyStyleForecastDelUnlockedSP = "SP_MID_CHN_STY_FOR_WK_DEL_UNLK";
		public const string DBChainWeeklyBaseForecastDelUnlockedSP = "SP_MID_CHN_BASE_FOR_WK_DEL_UNLK";
		public const string DBChainWeeklyForecastDelUnlockedSP = "SP_MID_CHN_FOR_WK_DEL_UNLK";

		// table count columns
		public const string DBStoreTableCountColumn = "STORE_TABLE_COUNT";
		public const string DBStyleStoreTableCountColumn = "STORE_STYLE_TABLE_COUNT";
		public const string DBColorStoreTableCountColumn = "STORE_COLOR_TABLE_COUNT";
		public const string DBSizeStoreTableCountColumn = "STORE_SIZE_TABLE_COUNT";

		// temp table names
		public const string DBTempTableName = "#TEMP";
		public const string DBBaseTempTableName = "#TEMPBASE";
		public const string DBStyleTempTableName = "#TEMPSTYLE";
		public const string DBColorTempTableName = "#TEMPCOLOR";
		public const string DBSizeTempTableName = "#TEMPSIZE";
		//End Track #4637

        // Begin Track #6395 - JSmith - Backposting rollup fails with database timeout messages
        // database Rollup stored procedure names
        public const string DBStoreWeeklyHistoryRollupSP = "SP_MID_ST_HIS_WK{#}_ROLLUP";
        public const string DBStoreDailyHistoryRollupSP = "SP_MID_ST_HIS_DAY{#}_ROLLUP";
        public const string DBStoreWeeklyForecastRollupSP = "SP_MID_ST_FOR_WK{#}_ROLLUP";
        public const string DBStoreWeeklyForecastHonorLocksRollupSP = "SP_MID_ST_FOR_WK{#}_LOCKS_ROLLUP";
        public const string DBStoreDayToWeekHistoryRollupSP = "SP_MID_ST_HIS_DAY_WK{#}_ROLLUP";
        public const string DBStoreToChainHistoryRollupSP = "SP_MID_HIS_ST_TO_CHN_ROLLUP";
        public const string DBStoreToChainForecastRollupSP = "SP_MID_FOR_ST_TO_CHN_ROLLUP";
        public const string DBStoreToChainForecastHonorLocksRollupSP = "SP_MID_FOR_ST_TO_CHN_LOCKS_ROLLUP";
        public const string DBStoreIntransitRollupSP = "SP_MID_ST_INTRANSIT_ROLLUP";
        public const string DBStoreExternalIntransitRollupSP = "SP_MID_ST_EXT_INTRANSIT_ROLLUP";
        public const string DBChainDailyHistoryRollupSP = "SP_MID_CHN_HIS_DAY_ROLLUP";
        public const string DBChainWeeklyHistoryRollupSP = "SP_MID_CHN_HIS_WK_ROLLUP";
        public const string DBChainWeeklyForecastRollupSP = "SP_MID_CHN_FOR_WK_ROLLUP";
        public const string DBChainWeeklyForecastHonorLocksRollupSP = "SP_MID_CHN_FOR_WK_LOCKS_ROLLUP";

        public const string DBStoreWeeklyHistoryNoZeroRollupSP = "SP_MID_ST_HIS_WK{#}_NOZERO_ROLLUP";
        public const string DBStoreDailyHistoryNoZeroRollupSP = "SP_MID_ST_HIS_DAY{#}_NOZERO_ROLLUP";
        public const string DBStoreWeeklyForecastNoZeroRollupSP = "SP_MID_ST_FOR_WK{#}_NOZERO_ROLLUP";
        public const string DBStoreToChainHistoryNoZeroRollupSP = "SP_MID_HIS_ST_TO_CHN_NOZERO_ROLLUP";
        public const string DBStoreToChainForecastNoZeroRollupSP = "SP_MID_FOR_ST_TO_CHN_NOZERO_ROLLUP";
        public const string DBChainDailyHistoryNoZeroRollupSP = "SP_MID_CHN_HIS_DAY_NOZERO_ROLLUP";
        public const string DBChainWeeklyHistoryNoZeroRollupSP = "SP_MID_CHN_HIS_WK_NOZERO_ROLLUP";
        public const string DBChainWeeklyForecastNoZeroRollupSP = "SP_MID_CHN_FOR_WK_NOZERO_ROLLUP";
        // End Track #6395

        public const string SQL_FOLDER_CONSTRAINTS = "SQL_Constraints";
        public const string SQL_FOLDER_SCALAR_FUNCTIONS = "SQL_Functions_Scalar";
        public const string SQL_FOLDER_TABLE_FUNCTIONS = "SQL_Functions_Table";
        public const string SQL_FOLDER_GENERATED_NONTABLE_FILES = "SQL_GeneratedNonTableFiles";
        public const string SQL_FOLDER_GENERATED_TABLE_FILES = "SQL_GeneratedTableFiles";
        public const string SQL_FOLDER_INDEXES = "SQL_Indexes";
        public const string SQL_FOLDER_SCRIPTS = "SQL_Scripts";
        public const string SQL_FOLDER_STORED_PROCEDURES = "SQL_StoredProcedures";
        public const string SQL_FOLDER_TABLE_KEYS = "SQL_TableKeys";
        public const string SQL_FOLDER_TABLES = "SQL_Tables";
        public const string SQL_FOLDER_TRIGGERS = "SQL_Triggers";
        public const string SQL_FOLDER_TYPES = "SQL_Types";
        public const string SQL_FOLDER_VIEWS = "SQL_Views";
        public const string SQL_FOLDER_UPGRADE_VERSIONS = "UpgradeVersions";

        // Begin TT#739-MD -JSmith - Delete Stores
        public const string DBAllocationWorkTablesSuffix = "_MIDALWORK";
        public const string DBTotalAllocation = "TOTAL_ALLOCATION";
        public const string DBDetailAllocation = "DETAIL_ALLOCATION";
        public const string DBBulkAllocation = "BULK_ALLOCATION";
        public const string DBPackAllocation = "PACK_ALLOCATION";
        public const string DBBulkColorAllocation = "BULK_COLOR_ALLOCATION";
        public const string DBBulkColorSizeAllocation = "BULK_COLOR_SIZE_ALLOCATION";
        // End TT#739-MD -JSmith - Delete Stores

//		// database bulk insert table names
//		public const string BulkStoreWeeklyHistoryTable = "BULK_STORE_HISTORY_WEEK";
//		public const string BulkStoreDailyHistoryTable = "BULK_STORE_HISTORY_DAY";
//		public const string BulkStoreWeeklyForecastTable = "BULK_STORE_FORECAST_WEEK";
//		public const string BulkStoreExternalIntransitTable = "BULK_STORE_EXTERNAL_INTRANSIT";
//		public const string BulkChainDailyHistoryTable = "BULK_CHAIN_HISTORY_DAY";
//		public const string BulkChainWeeklyHistoryTable = "BULK_CHAIN_HISTORY_WEEK";
//		public const string BulkChainWeeklyForecastTable = "BULK_CHAIN_FORECAST_WEEK";
		
		// history column constants
		public const int NoColumnPosition = -1;

		// System constants
		public const int UndefinedUserRID = 1;
		public const int AdministratorUserRID = 2;
		public const int SystemUserRID = 3;
		public const int GlobalUserRID = 4;
        public const int CustomUserRID = 5;
		public const int UndefinedCalendarDateRange = 1;
		public const int UndefinedStoreRID = -1;
		public const int UndefinedStoreCharRID = 0;
		public const int UndefinedStoreGroupRID = 0;
		public const int UndefinedStoreIndex = -1;
		public const int UndefinedHeaderGroupRID = 1;
		public const int UndefinedSizeGroupRID = 1;
		public const int UndefinedMethodRID = 1;
		public const int UndefinedVelocityMethodRID = 2;
		public const int UndefinedWorkflowRID = 1;
		public const int UndefinedGroupByRID = 1;
		public const int UndefinedComponentCriteria = 1;
		public const int UndefinedStoreFilter = 1;
		public static readonly DateTime UndefinedDate = DateTime.MinValue;
		public const int UndefinedDynamicSwitchDate = 0;	// Issue 5171
		public const int UndefinedPlaceholderSeq = 0;	// TT#1227 - stodd - assortment
		public const int UndefinedHeaderSeq = 0;	// TT#1227 - stodd - assortment
		public const int BaseIDMaxSize = 50;
		public const int ColorIDMaxSize = 50;
		public const int StoreGroupIDMaxSize = 50;
		public const int StoreGroupLevelIDMaxSize = 50;
		public const int SizeIDMaxSize = 50;
		public const int toolTipAutoPopDelay = 5000;
		public const int toolTipInitialDelay = 500;
		public const int toolTipReshowDelay = 500;
		public const int ParentLevelOffset = -1;
		public const int PercentToInteger = 10000000;
		//Begin TT#1069 - JScott - Calendar does not automatically expand correctly
		//public const int CalendarStartupYearRange = 20;
		public const int CalendarStartupYearRange = 50;
		//End TT#1069 - JScott - Calendar does not automatically expand correctly
		public const double DefaultSimilarStoreRatio = 100;
		public const int AllStoreTotal = -1;
		public const double UseSystemTolerancePercent = -1;
		public const int DefaultModifier = 1;
		public const int TotalMatrixLevelRID = 0;
		public const int DecimalPositionsInStoreSizePctToColor = 3;
		public const int DummyColorRID = 0;
		public const string DummyColorID = "Unknown";
		public const int AllStoreGroupRID = 1;
		public const int AllStoreGroupLevelRID = 1;
		//Begin TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
		public const string AvailableStoresGroupLevelName = "Available Stores";
		//End TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
        public const string DefaultThemeName = "Modern 1";
        public const string NullForStringValue = "~NFSV~"; //TT#1310-MD -jsobek -Error when adding a new Store -changing null to ~NFSV~ so it will not be guessed easily by clients and wont interfere will characteristic code


		// Batch constants
//Begin Track #3727 - JScott - Write plans out in smaller chunks
//		public const int DefaultCommitLimit = 10000;
		public const int DefaultCommitLimit = 1000;
//End Track #3727 - JScott - Write plans out in smaller chunks
		public const int DefaultBulkInsertBatchSize = 100000;
		public const int DefaultBulkInsertRetryCount = 3;
		public const int DefaultBulkInsertCommandTimeout = 60;  // in seconds
		public const bool DefaultSerializeXMLFile = false;
		public const bool DefaultAllowAutoAdds = true;
		public const bool DefaultUseBulkInsert = false;
		public const bool DefaultRollupValues = false;
		public static readonly DateTime PurgeMinDate = new DateTime(1950, 01,01);
		public static readonly DateTime PurgeMaxDate = new DateTime(2200, 01,01);

        // menues
        public const string menuFile = "file_menu";
        public const string menuEdit = "edit_menu";
        public const string menuView = "view_menu";
        public const string menuAlloc = "alloc_menu";
        public const string menuPlanning = "planning_menu";
        public const string menuAdmin = "admin_menu";
        public const string menuTools = "tools_menu";
        public const string menuWindow = "window_menu";
        public const string menuHelp = "help_menu";
        public const string menuCharacteristicSubMenu = "characteristicSubMenu";
        public const string menuStoreChar = "menuStoreChar";
        public const string menuStoreSub = "storesSubMenu";
        public const string menuModelsSubMenu = "modelsSubMenu";
        public const string menuAllocSub = "allocSub";
        public const string menuReports = "reports_menu";
        public const string menuSizeSubMenu = "sizeSubMenu";

        // menu buttons
        public const string btNew = "btNew";
        public const string btClose = "btClose";
        public const string btSave = "btSave";
        public const string btSaveAs = "btSaveAs";
        public const string btCut = "btCut";
        public const string btCopy = "btCopy";
        public const string btPaste = "btPaste";
        public const string btDelete = "btDelete";
        public const string btClear = "btClear";
        public const string btFind = "btFind";
        public const string btExport = "btExport";
        public const string btUndo = "btUndo";
        public const string btReplace = "btReplace";
        public const string btQuickFilter = "btQuickFilter";
        public const string btViewAdmin = "btViewAdmin";
        public const string btPlanView = "btPlanView";
        public const string btSecurity = "btSecurity";
        public const string btCalendar = "btCalendar";
        public const string btHeaderChar = "btHeaderChar";
        public const string btProductChar = "btProductChar";
        public const string btStoreChar = "btStoreChar";
        public const string btProfiles = "btProfiles";
        public const string btCharacteristics = "btCharacteristics";
        public const string btOptions = "btOptions";
        public const string btEligModels = "btEligModels";
        public const string btStkModModels = "btStkModModels";
        public const string btFWOSModModels = "btFWOSModModels";
        public const string btFWOSMaxModels = "btFWOSMaxModels"; //TT#108 - MD - DOConnell - FWOS Max Model
        public const string btSlsModModels = "btSlsModModels";
        public const string btForecastingModels = "btForecastingModels";
        public const string btForecastBalModels = "btForecastBalModels";
        public const string btSizeConstraintsModels = "btSizeConstraintsModels";
        public const string btLoginAsAdmin = "btLoginAsAdmin"; //TT#1521-MD -jsobek -Active Directory Authentication
        public const string btExit = "btExit";
        public const string btWorkspace = "btWorkspace";
        public const string btPlanWorkspace = "btPlanWorkspace";
        public const string btMerchandise = "btMerchandise";
        public const string btStore = "btStore";
        public const string btStoreFilter = "btStoreFilter";
        public const string btHeaderFilter = "btHeaderFilter"; //TT#1313-MD -jsobek -Header Filters
        public const string btAssortmentFilter = "btAssortmentFilter"; //TT#1313-MD -jsobek -Header Filters
        public const string btTaskList = "btTaskList";
        public const string btWorkflow = "btWorkflow";
        public const string btSelect = "btSelect";
        public const string btStyle = "btStyle";
        public const string btSize = "btSize";
        public const string btSummary = "btSummary";
        public const string btAssortment = "btAssortment";
		public const string btGroupAllocation = "btGroupAllocation";	// TT#488-MD - STodd - Group Allocation - 
		public const string btAssortmentExplorer = "btAssortmentExplorer";
        public const string btSizeGroups = "btSizeGroups";
        public const string btSizeCurves = "btSizeCurves";
        public const string btSort = "btSort";
        public const string btFilter = "btFilter";
        public const string btFilterWizard = "btFilterWizard";
        public const string btRefresh = "btRefresh";
        public const string btAudit = "btAudit";
        public const string btAuditReclass = "btAuditReclass";
        //Begin Track #6232 - KJohnson - Incorporate Audit Reports in Version 3.0 Base
        public const string btNodePropertiesOverrides = "btNodePropertiesOverrides";
        public const string btForecastAuditMerchandise = "btForecastAuditMerchandise";
        public const string btForecastAuditMethod = "btForecastAuditMethod";
        public const string btAllocationAudit = "btAllocationAudit";
        //End Track #6232 - KJohnson
        public const string btTextEditor = "btTextEditor";
        //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
        public const string btEmailMessage = "btEmailMessage";
        //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
        //Begin TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
        public const string btAllocationAnalysis = "btAllocationAnalysis";
        public const string btForecastAnalysis = "btForecastAnalysis";
        //End TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
        public const string btReportUserOptionsReview = "btReportUserOptionsReview"; //TT#554-MD -jsobek -User Log Level Report
        public const string btReportAllocationByStore = "btReportAllocationByStore"; //TT#739-MD -jsobek -Delete Stores -Allocation by Store Report
        public const string btScheduleBrowser = "btScheduleBrowser";
        public const string btUserOptions = "btUserOptions";
        public const string btAbout = "btAbout";
        public const string btSizeAlternatesModels = "btSizeAlternatesModels";
        public const string btRelease = "btRelease";
        public const string btCustomReports = "btCustomReports";
        public const string btMemory = "btMemory";
        public const string btOverrideLowLevel = "btOverrideLowLevel";
        public const string btTheme = "btTheme";                   // MID Track #5006 - Add Theme to Tools menu 
        public const string btRestoreLayout = "btRestoreLayout";   // Workspace Usability Enhancement 
        public const string btProcessControl = "btProcessControl";	// TT#1581-MD - stodd - API Header Reconcile
        public const string btSchedulerJobManager = "btSchedulerJobManager";

        public const string btAssortmentWorkspace = "btAssortmentWorkspace"; // TT#2 Assortment Planning
        public const string btUserActivityExplorer = "btUserActivityExplorer";   // TT#46 MD - JSmith - User Dashboard 
        
        // Toolbars
        public const string tbExplorers = "Explorers";
        public const string tbAnalysis = "Analysis";

        // Toolbar Buttons
        public const string tbbAssortment = "Assortment";
		public const string tbbGroupAllocation = "Group Allocation";	// TT#488-MD - STodd - Group Allocation - 
        public const string tbbSize = "Size";
        public const string tbbMerchandise = "Merchandise";
        public const string tbbWorkflowMethods = @"Workflow\Methods";
        public const string tbbStores = "Stores";
        public const string tbbAllocationWorkspace = "Allocation Workspace";
        public const string tbbStoreFilters = "Store Filters";
        public const string tbbHeaderFilters = "Header Filters"; //TT#1313-MD -jsobek -Header Filters
        public const string tbbAssortmentFilters = "Assortment Filters"; //TT#1313-MD -jsobek -Header Filters
        public const string tbbTaskLists = "Task Lists";
        public const string tbbSummary = "Summary";
        public const string tbbOTSPlan = "OTS Plan";
        public const string tbbStyle = "Style";
		public const string tbbAssortmentExplorer = "Assortment Explorer";		// TT#2 Assortment Planning
        public const string tbbAssortmentWorkspace = "Assortment Workspace";    // TT#2 Assortment Planning
        public const string tbbUserActivityExplorer = "My Activity";  // TT#46 MD - JSmith - User Dashboard //TT#46-MD -jsobek -Develop User Activity Log

//		// Scheduler constants
//		public const eScheduleTimeUnit DefaultRecurTimeUnit = eScheduleTimeUnit.Week;
//		public const int DefaultRecurTimeMultiple = 1;


		// Allocation constants
		public static eSQL_StructureType[] SQL_StructureTypes = (eSQL_StructureType[])Enum.GetValues(typeof(eSQL_StructureType)); // MID Track 3994 Performance
		public const int DefaultHeaderRID = 1;
		public const int DefaultUnitMultiple = 1;
		public const int DefaultPrimarySecondaryRID = 1;
		public const int DefaultPlanHnRID = 1;
        //public const int DefaultHeaderStyleHnRID = 1;		// TT#488-MD - Stodd - Group Allocation  // TT#991 - MD - Jellis - GA - Last Header Removed from Group gets Foreign Key Error (The default must be negative).
		public const int DefaultOnHandHnRID = 1;
		public const int DefaultPlanViewRID = 1;
		public const int DefaultAssortmentViewRID = 1;
		public const int DefaultPostReceiptViewRID = 2;	// TT#490-MD - stodd -  post-receipts should not show placeholders
        public const int DefaultGroupAllocationViewRID = 3;	 // TT#952 - MD - stodd - add matrix to Group Allocation Review
		public const int DefaultThemeRID = 16;
        public const int DefaultVelocityMatrixViewRID = 1;  // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        public const int DefaultVelocityDetailViewRID = 10; // End TT#231  
		public const double DefaultPlanFactorPercent = 0;
        public const double DefaultGenericPackRounding1stPackPct = -1; // double.MinValue; // TT#616 - stodd   // TT#616 - AGallagher - Allocation - Pack Rounding (#67)
		public const double DefaultGenericPackRoundingNthPackPct = -1; // double.MinValue; // TT#616 - stodd   // TT#616 - AGallagher - Allocation - Pack Rounding (#67)
//		public const double DefaultPercentNeedLimit = 0.0;
		public const double DefaultPercentNeedLimit = double.MinValue;
		public const double DefaultBalanceTolerancePercent = double.MaxValue;
		public const double DefaultPackSizeErrorPercent = double.MaxValue;
		public const double DefaultMaxSizeErrorPercent = double.MaxValue;
        public const int DefaultBldPacksMaxPackNeedTol = 0;   // TT#370 Build Packs Enhancement (correction)
		public const double DefaultFillSizeHolesPercent = 0.0;
		public const bool DefaultBulkIsDetail = false;
		public const eSecurityLevel DefaultNotSpecifiedSecurity = eSecurityLevel.Deny;
		public const int UndefinedNewStorePeriodBegin = int.MaxValue;
		public const int UndefinedNewStorePeriodEnd = int.MaxValue;
		public const int UndefinedNonCompStorePeriodBegin = int.MaxValue;
		public const int UndefinedNonCompStorePeriodEnd = int.MaxValue;
		public const int UndefinedDay = -1;
//		public const int NoCDR = -1;			use UndefinedCalendarDateRange instead
		public const int NoRID = -1;
		public const int Undefined = -1;
        public const int NoImoMaxValue = int.MaxValue; // TT#1401 - JEllis - Urban Reservation Stores pt 2
		public const double UndefinedReserve = -1;
		public const double UndefinedDouble = -1;
		public const int IntransitKeyTypeNoColor = 0;
		public const int IntransitKeyTypeNoSize = 0;
		// Begin TT#1224 - stodd - assortment committed
		public const int CommittedKeyTypeNoColor = 0;
		public const int CommittedKeyTypeNoSize = 0;
		// End TT#1224 - stodd - assortment committed
		public const int LargestIntegerMaximum = int.MaxValue;
		public const int NoLayerID = -1;
		public const int MaxStoreGradeWeeks = 26;
		public const double MaxWeeksOfSupply = 53.0;
		public const int MinVelocityGrades = 2;
		public const int MaxShipUpToQty = 9999;
		public const double NoRuleQty = 0.0;
		public const int UndefinedMinimum = 0;
		public const int UndefinedMaximum = int.MaxValue;
		public const int UndefinedMultiple = -1;
		public const int UndefinedRule = -1;
		public const int UndefinedQuantity = 0;
		public const int MaskedRID = int.MinValue; // MID Track 3844 Constraints not working
		public const string HeaderNameExcludedCharacters = "\\/:*?\"<>,|";
		public const int UndefinedHeader = -1;
		public const int UndefinedPack = -1;
		public const int UndefinedColor = -1;
		public const int BulkColor = -2;
		public const int UndefinedOverrideModel = -2;	// Override Low level Changes
		//Begin TT#707 - JScott - Size Curve process needs to multi-thread
		public const int ConcurrentSizeCurveProcesses = 5;
		//End TT#707 - JScott - Size Curve process needs to multi-thread
        public const long NoTransaction = 0; // TT#1185 - Verify ENQ before Update 
		public const int MaxPackNeedTolerance = 5; // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement

		// Planning Constants
		public const string ComputationsAssemblyName = "MIDRetail.ForecastComputations.dll";
		public const int PlanReadPreWeeks = 0;
		public const int PlanReadPostWeeks = 4;
		public const int PlanReadExpansionSize = 8;

		// Size Group Constants
		public const string DefaultSizeCurveName = "DEFAULT";
		public const int SizeGroupGridInitialRows = 12;
		public const int SizeGroupGridInitialColumns = 6;
		public const int SizeGroupWidthHeaderColumnWidth = 20;
	
        // BEGIN TT#1401 - GRT - Reservation Stores
        // VSW Constants
        public const double PercentPackThresholdDefault = 0.5;
        // END TT#1401 - GRT - Reservation Stores

		// Store Constants
		public const int AllStoreFilterRID = 1;

		// Version Constants
		public const int FV_PlanLowLevelTotalRID = 0;
		public const int FV_ActualRID = 1;
		public const int FV_ActionRID = 2;
		//Begin Track #4457 - JSmith - Add forecast versions
		public const int FV_BaselineRID = 3;
		//End Track #4457
		public const int FV_ModifiedRID = 4;

		// Audit constants
        //Begin TT#1885 - DOConnell - Audit REPORT_MESSAGE is truncated on long messages which looses important information.
		//public const int AuditReportMessageLength = 500;
        //End TT#1885 - DOConnell - Audit REPORT_MESSAGE is truncated on long messages which looses important information.
		public const string AuditDateTimeFormat = "yyyy/MM/dd HH:mm:ss.fff";

//		// security images
//		public const string SecUserImage = "User.bmp";
//		public const string SecGroupImage = "Group.bmp";
//		public const string SecClosedFolderImage = "CLSDFOLD.ICO";
//		public const string SecOpenFolderImage = "OPENFOLD.ICO";
//		public const string SecNoAccessImage = "TRFFC10C.ico";
//		public const string SecReadOnlyAccessImage = "TRFFC10B.ico";
//		public const string SecFullAccessImage = "TRFFC10A.ico";
//
//		//OTS Plan Method image
//		public const string GlobalImage = "globe.ico";

		public const string MIDApplicationRoot = "ApplicationRoot";
		public const string MIDDefault = "default";
		//		public const string MIDLevels = "Levels";
		//		public const string MIDNewHierarchy = "New Hierarchy";
		//		public const string MIDNewPersonalHierarchy = "New Personal Hierarchy";
		//		public const string MIDNewLevel = "(New Level)";
		public const string MIDHierarchyMaintenance = "Hierarchy Maintenance";
		public const string MIDMerchandiseExplorer = "Merchandise Explorer";
		public const string MIDAllocationWorkspaceExplorer = "Allocation Workspace Explorer";
        //public const string MIDUserDashboard = "User Dashboard"; // TT#46 MD - JSmith - User Dashboard 

		//OTS Planning cell value formatting - common pre-built formats.  Each cell of the grid must be formatted to a string before returning
		//to the Client.  In order to help increase efficiency, the most common format requests are predefined, so that the string doesn't have
		//to be built for every cell.
		public static readonly string[] DecimalFormats = {	"###,###,##0;-###,###,##0;0",
															"###,###,##0.0;-###,###,##0.0;0.0",
															"###,###,##0.00;-###,###,##0.00;0.00",
															"###,###,##0.000;-###,###,##0.000;0.000",
															"###,###,##0.0000;-###,###,##0.0000;0.0000",
															"###,###,##0.00000;-###,###,##0.00000;0.00000"   };

		//Begin BonTon Calcs - JScott - Add Display Precision
		public static readonly string[] NoCommaDecimalFormats = {	"########0;-########0;0",
																	"########0.0;-########0.0;0.0",
																	"########0.00;-########0.00;0.00",
																	"########0.000;-########0.000;0.000",
																	"########0.0000;-########0.0000;0.0000",
																	"########0.00000;-########0.00000;0.00000"   };

		//End BonTon Calcs - JScott - Add Display Precision
		public const int ExcelExportNegativeColor = 0x00FF0000;
		public const int ExcelExportRowHeadingColor = 0x0099CCFF;
		public const int ExcelExportColHeadingColor = 0x0099CCFF;
		public const int ExcelExportDetailColor = 0x00FFFFCC;
		public const int ExcelExportSetTotalColor = 0x00CCFFFF;
		public const int ExcelExportAllStoreTotalColor = 0x0099CCFF;

		// begin MID Track 4341 Performance Issues
		public static char[] StoreTrimChar = "0".ToCharArray();
		public static char[] DateTrimChar = "0".ToCharArray();
		public const char CommaCharacter = ',';
		public const char SemiColonCharacter = ';';
		public static char[] NonZeroDigitCharArray ="123456789".ToCharArray();
		public static long LongNoRID = (long)(Math.Pow(2,32) - 1); // MID Track 4372 Generic Size Constraints
		// end MID Track 4341 Performance Issues

		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		////Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		//public const string SharedExtension = ".shared";
		////End Track #4815
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"

		public const string SchedulerID = "##MID##";

//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"

        public const string ForceLocalID = "##ForceLocal##";  // TT#1646-MD - JSmith - Add or Remove Hierarchy Levels

        // begin TT#173  Provide database container for large data collections
        public const string DbKeyname_HdrRID = "HDR_RID";
        public const string DbKeyname_HdrPackRID = "HDR_PACK_RID";
        public const string DbKeyname_Hdr_BC_RID = "HDR_BC_RID";
        public const string DbKeyname_Hdr_BCSZ_Key = "HDR_BCSZ_KEY";
        public const string DbKeyname_TimeID = "TIME_ID";
        public const string DbKeyname_HnRID = "HN_RID";
        public const string DbKeyname_ColorCodeRID = "COLOR_CODE_RID";
        public const string DbKeyname_SizeCodeRID = "SIZE_CODE_RID";
        public const string DbKeyname_AllocationSummaryNode = "ALLOCATION_SUMMARY_NODE"; // TT#370 Build Packs Enhancement
        // end TT#173  Provide database container for large data collections
		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		public const int AssortmentPlanVariableKeyOffset = 20000;
		//End TT#2 - JScott - Assortment Planning - Phase 2
		public const string NodeEnqueueLockID = "NodeEnqueue"; // TT#2015 - JSmith - Apply Changes to Low Levels in Node Properties

        public delegate void AuditFilterFormDelegate(object sender, System.EventArgs e); //TT#435-MD-DOConnell-Add new features to Audit

		// Begin TT#1581-MD - stodd - Header Reconcile
        // "Header"-type keys that can be used to construct a header ID.
        public static List<string> HeaderIDKeyList = new List<string>() { "STYLEID", "DISTCENTER", "HEADERDATE", "VENDOR", "PURCHASEORDER", "VSWID" };	// TT#1657-MD - stodd - Generated Header ID does not match on Header Date 
        // "Header"-type keys that can be used to match transactions and headers.
        public static List<string> HeaderKeyList = new List<string>() { "HEADERDESCRIPTION","HEADERDATE", "UNITRETAIL", "UNITCOST", "STYLEID", "PARENTOFSTYLEID",
                "SIZEGROUPNAME", "HEADERTYPE", "DISTCENTER", "VENDOR", "PURCHASEORDER", "WORKFLOW", "VSWID"};
		// End TT#1581-MD - stodd - Header Reconcile
        public static string Sequence = "SEQUENCE";	// TT#1605-MD - stodd - Adding 'sequence' to other header ID keys causes sequence to replace all other keys.
        // Begin TT#1662-MD - RMatelic - In Style Review screen, columns not aligning and totals not populating
        public const int DefaultStyleViewRID = 15;
        public const int DefaultSizeViewRID = 20; 
        // End TT#1652-MD
		//=============
		// CONSTRUCTORS
		//=============

		public Include()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Converts boolean inParam to char return value: 1=true, 0=false
		/// </summary>
		/// <param name="inParam">Boolean input parameter to be converted to char</param>
		/// <returns>char 1=true, 0=false</returns>
		public static char ConvertBoolToChar(bool inParam)
		{
			switch (inParam)
			{
				case true:
					return '1';
				default:
					return '0';
			}
		}
        public static int? ConvertObjectToNullableInt(object o)
        {
            int? myNullableInt = null;
            //Begin TT#1231-MD -jsobek -Unlock error when saving changes to Store Profile
            if (o != null && o != DBNull.Value && o.ToString() != string.Empty) myNullableInt = (int)o;
            //End TT#1231-MD -jsobek -Unlock error when saving changes to Store Profile
            return myNullableInt;
        }
        public static double? ConvertObjectToNullableDouble(object o)
        {
            double? myNullableDouble = null;
            //Begin TT#1231-MD -jsobek -Unlock error when saving changes to Store Profile
            if (o != null && o != DBNull.Value && o.ToString() != string.Empty) myNullableDouble = Convert.ToDouble(o);
            //End TT#1231-MD -jsobek -Unlock error when saving changes to Store Profile
            return myNullableDouble;
        }
        public static DateTime? ConvertObjectToNullableDateTime(object o)
        {
            DateTime? myNullableDateTime = null;
            //Begin TT#1231-MD -jsobek -Unlock error when saving changes to Store Profile
           
            if (o != null && o != DBNull.Value && o.ToString() != string.Empty)
            {
                myNullableDateTime = (DateTime)o;
            }
            //End TT#1231-MD -jsobek -Unlock error when saving changes to Store Profile
            return myNullableDateTime;
        }
        public static string ConvertObjectToNullableString(object o)
        {
            string myNullableString = Include.NullForStringValue; //TT#1310-MD -jsobek -Error when adding a new Store
            //Begin TT#1231-MD -jsobek -Unlock error when saving changes to Store Profile
          
            if (o != null && o != DBNull.Value) myNullableString = (string)o;
            //End TT#1231-MD -jsobek -Unlock error when saving changes to Store Profile
            return myNullableString;
        }

        public static char? ConvertObjectToNullableChar(object o)
        {
            char? myNullableChar = null;


            //Begin TT#1231-MD -jsobek -Unlock error when saving changes to Store Profile
           
            if (o != null && o != DBNull.Value && o.ToString() != string.Empty) 
            {
                string c = o.ToString();
                if (c.Length > 0)
                {
                    char[] cArray = c.ToCharArray();
                    myNullableChar = cArray[0];
                }
            }
            //End TT#1231-MD -jsobek -Unlock error when saving changes to Store Profile
           
            return myNullableChar;
        }

        //Begin TT#1517-MD -jsobek -Store Service Optimization
        public static DateTime ConvertObjectToDateTime(object proposedValue)
        {
            try
            {
                DateTime dtValue;
                if (proposedValue == DBNull.Value)
                {
                    dtValue = Include.UndefinedDate;
                }
                else
                {
                    dtValue = Convert.ToDateTime(proposedValue);
                }
                return dtValue;
            }
            catch
            {
                throw;
            }
        }
        //End TT#1517-MD -jsobek -Store Service Optimization


        // Begin TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables
        /// <summary>
        /// Converts boolean inParam to char return value: 1=true, 0=false
        /// </summary>
        /// <param name="inParam">Boolean input parameter to be converted to int</param>
        /// <returns>char 1=true, 0=false</returns>
        public static int ConvertBoolToInt(bool inParam)
        {
            switch (inParam)
            {
                case true:
                    return 1;
                default:
                    return 0;
            }
        }
		// End TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables

		// Begin - stodd
		/// <summary>
		/// Converts string inParam to bool return value: 1=true, 0=false
		/// </summary>
		/// <param name="inParam">string input parameter to be converted to bool</param>
		/// <returns>char 1=true, 0=false</returns>
		public static bool ConvertStringToBool(string inParam)
		{
			if (inParam != null)
			{
				string sBool = inParam.ToLower();
				if (sBool == "true" || sBool == "yes" || sBool == "t" || sBool == "y" || sBool == "1")
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}

		}
		// End - stodd

        //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
        public static char? ConvertStringToChar(string inParam)
        {
            if (inParam != null && inParam != string.Empty)
            {
                return (inParam.ToCharArray(0,1))[0]; // returns first character in the string
            }
            else
            {
                return null;
            }
        }
        public static int? ConvertStringFieldToIntNullable(string s)
        {
            int? valueNullable = null;
            int value;
            if (int.TryParse(s, out value) == true)
            {
                valueNullable = value;
            }
            return valueNullable;
        }
        //End TT#846-MD -jsobek -New Stored Procedures for Performance


		/// <summary>
		/// Converts char inParam to bool return value: 1=true, 0=false
		/// </summary>
		/// <param name="inParam">char input parameter to be converted to bool</param>
		/// <returns>char 1=true, 0=false</returns>
		public static bool ConvertCharToBool(char inParam)
		{
			switch (inParam)
			{
				case '0':
					return false;
				default:
					return true;
			}
		}

		// BEGIN Issue 5000
		/// <summary>
		/// Converts int inParam to bool return value: 0=false, else true
		/// </summary>
		/// <param name="inParam">int input parameter to be converted to bool</param>
		/// <returns>char 0=false, else true</returns>
		public static bool ConvertIntToBool(int inParam)
		{
			switch (inParam)
			{
				case 0:
					return false;
				default:
					return true;
			}
		}
		// END Issue 5000

		// BEGIN TT#1598 - stodd - store perf
		// Moved here from OTSForecastExport
		public static bool ConvertBoolConfigValue(string aBoolConfigValue)
		{
			try
			{
				if (aBoolConfigValue != null)
				{
					if (aBoolConfigValue.ToLower() == "true" || aBoolConfigValue.ToLower() == "yes" ||	// TT#1401 - stodd - push to back stock
						aBoolConfigValue.ToLower() == "t" || aBoolConfigValue.ToLower() == "y")
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public static int ConvertNumericConfigValue(string aNumericConfigValue)
		{
			try
			{
				return Convert.ToInt32(aNumericConfigValue);
			}
			catch (FormatException)
			{
				return 0;
			}
			catch (OverflowException)
			{
				return 0;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
		// END TT#1598 - stodd - store perf

		/// <summary>
		/// Converts int UserRID to eGlobalUserType return value
		/// </summary>
		/// <param name="userRID">Check if UserRID is UndefinedUserRID = 1</param>
		/// <returns>enum eGlobalUserType</returns>
		public static eGlobalUserType GeteGlobalUserType(int userRID)
		{
			if (GlobalUserRID == userRID)
				return eGlobalUserType.Global;
			else
				return eGlobalUserType.User;
		}

		/// <summary>
		/// Gets the "UndefinedUserRID" in case the "const" variable name changes again!
		/// </summary>
		/// <returns>int System UserRID (not an actual user) - defines Global versus User type</returns>
		public static int GetGlobalUserRID()
		{
			return GlobalUserRID;
		}

        // Begin TT#1902-MD - JSmith - Store Services - VSW API Error
        public static StoreProfile GetUnknownStoreProfile()
        {
            StoreProfile sp = new StoreProfile(Include.UndefinedStoreRID);
            sp.SetFieldsToUnknownStore();
            return sp;
        }
        // End TT#1902-MD - JSmith - Store Services - VSW API Error

		/// <summary>
		/// Evaluates enum and returns appropriate UserRID
		/// </summary>
		/// <param name="globalUserType">eGlobalUserType</param>
		/// <param name="UserID">UserID to evaluate</param>
		/// <returns>Evaluates enum and returns appropriate UserRID</returns>
		public static int EvalGlobalUserTypeUserRID(eGlobalUserType globalUserType, int UserID)
		{
			if (globalUserType == eGlobalUserType.User)
				return UserID;
			else
				return GetGlobalUserRID();
		}

        //Begin TT#1310-MD -jsobek -Error when adding a new Store -unused function
        ///// <summary>
        ///// Convert DBNull.Value to string: null
        ///// </summary>
        ///// <param name="drObj">DataRow column...(more specific data type here?)</param>
        ///// <returns>string null if DBNull.Value else return drObj.ToString()</returns>
        //public static string DBNullToNullString(object drObj)
        //{
        //    if (drObj == DBNull.Value)
        //        return "null";
        //    return drObj.ToString();
        //}
        //End TT#1310-MD -jsobek -Error when adding a new Store -unused function

		public static string GetConfigFilename()
		{
			string[] fields = System.Environment.CommandLine.Split('\\');
			string exeName = fields[fields.Length-1].TrimEnd('"','\\');

			return exeName + ".config";
		}

		/// <summary>
		/// Formats the node display based on the display option
		/// </summary>
		/// <param name="displayOption">The display option for the format</param>
		/// <param name="nodeID">The ID of the node</param>
		/// <param name="nodeName">The name of the node</param>
		/// <param name="nodeDescription">The description of the node</param>
		/// <returns></returns>
		public static string GetNodeDisplay(eHierarchyDisplayOptions displayOption, string nodeID, 
			string nodeName, string nodeDescription)
		{
			string nodeDisplay;
			switch (displayOption)
			{
				case eHierarchyDisplayOptions.DescriptionOnly:
					nodeDisplay = nodeDescription;
					break;
				case eHierarchyDisplayOptions.IdAndDescription:
					nodeDisplay = nodeID + " [" + nodeDescription + "]";
					break;
				case eHierarchyDisplayOptions.IdAndName:
					nodeDisplay = nodeID + " [" + nodeName + "]";
					break;
				case eHierarchyDisplayOptions.IdAndNameAndDesc:
					nodeDisplay = nodeID + " [" + nodeName + "]"+ " [" + nodeDescription + "]";
					break;
				case eHierarchyDisplayOptions.IdOnly:
					nodeDisplay = nodeID;
					break;
				case eHierarchyDisplayOptions.NameAndDescription:
					nodeDisplay = nodeName + " [" + nodeDescription + "]";;
					break;
				case eHierarchyDisplayOptions.NameOnly:
					nodeDisplay = nodeName;
					break;
				default:
					nodeDisplay = nodeName;
					break;
			}
			return nodeDisplay;
		}


		public static string GetStoreDisplay(eStoreDisplayOptions displayOption, string storeID, 
			string storeName, string storeDescription)
		{
			string storeDisplay;
			switch (displayOption)
			{
				case eStoreDisplayOptions.DescriptionOnly:
					storeDisplay = storeDescription;
					break;
				case eStoreDisplayOptions.IdAndDescription:
					storeDisplay = storeID + " [" + storeDescription + "]";
					break;
				case eStoreDisplayOptions.IdAndName:
					storeDisplay = storeID + " [" + storeName + "]";
					break;
				case eStoreDisplayOptions.IdAndNameAndDesc:
					storeDisplay = storeID + " [" + storeName + "]"+ " [" + storeDescription + "]";
					break;
				case eStoreDisplayOptions.IdOnly:
					storeDisplay = storeID;
					break;
				case eStoreDisplayOptions.NameAndDescription:
					storeDisplay = storeName + " [" + storeDescription + "]";;
					break;
				case eStoreDisplayOptions.NameOnly:
					storeDisplay = storeName;
					break;
				default:
					storeDisplay = storeName;
					break;
			}
			if (storeDisplay.Length == 0)
			{
				storeDisplay = storeID;
			}
			return storeDisplay;
		}

		// begin MID Track 4967 Size Functinos Not Showing total Qty
		private static System.Array _sizeFunctionCodeTypes;
		public static System.Array GetSizeFunctionCodeTypes()
		{
			if (_sizeFunctionCodeTypes == null)
			{
				_sizeFunctionCodeTypes = Enum.GetValues(typeof(eSizeMethodType));
			}
			return _sizeFunctionCodeTypes;
		}
		// end MID Track 4967 Size Functions Not Showing Total Qty

		// begin MID Track 4033 allow multi header children to change
		private static string _headerTableColumns;
		public static string GetHeaderTableColumns()
		{
  			if (_headerTableColumns == null)
			{	
				// Begin TT#893 - MD - stodd - Add "Group Allocation ID" Column to Allocation Workspace
                StringBuilder headerTableColumns = new StringBuilder("H.HDR_RID, H.HDR_ID");
                headerTableColumns.Append(", COALESCE(H.HDR_DESC, ' ') AS HDR_DESC");
                headerTableColumns.Append(", H.HDR_DAY"); // removing nulls from date fields and replacing with Undefined date results in wrong date of 1/2/1900
                headerTableColumns.Append(", H.ORIG_DAY");
                headerTableColumns.Append(", COALESCE(H.UNIT_RETAIL,0) AS UNIT_RETAIL");
                headerTableColumns.Append(", COALESCE(H.UNIT_COST, 0) AS UNIT_COST");
                headerTableColumns.Append(", COALESCE(H.UNITS_RECEIVED,0) AS UNITS_RECEIVED");
                headerTableColumns.Append(", COALESCE(H.STYLE_HNRID, "); headerTableColumns.Append(Include.NoRID.ToString()); headerTableColumns.Append(") AS STYLE_HNRID");
                headerTableColumns.Append(", COALESCE(H.PLAN_HNRID, "); headerTableColumns.Append(Include.DefaultPlanHnRID.ToString()); headerTableColumns.Append(") AS PLAN_HNRID");
                headerTableColumns.Append(", COALESCE(H.ON_HAND_HNRID, "); headerTableColumns.Append(Include.DefaultOnHandHnRID.ToString()); headerTableColumns.Append(") AS ON_HAND_HNRID");
                headerTableColumns.Append(", COALESCE(H.BULK_MULTIPLE, 1) AS BULK_MULTIPLE");
                headerTableColumns.Append(", COALESCE(H.ALLOCATION_MULTIPLE, 1) AS ALLOCATION_MULTIPLE");
                headerTableColumns.Append(", COALESCE(H.ALLOCATION_MULTIPLE_DEFAULT, 1) AS ALLOCATION_MULTIPLE_DEFAULT");  // MID Track 5761 Allocation Multiple not saved to data base
                headerTableColumns.Append(", COALESCE(H.VENDOR, ' ') AS VENDOR");
                headerTableColumns.Append(", COALESCE(H.PURCHASE_ORDER, ' ') AS PURCHASE_ORDER");
                headerTableColumns.Append(", H.BEGIN_DAY");
                headerTableColumns.Append(", H.NEED_DAY");
                headerTableColumns.Append(", H.SHIP_TO_DAY");
                headerTableColumns.Append(", H.RELEASE_DATETIME");
                headerTableColumns.Append(", H.RELEASE_APPROVED_DATETIME");
                headerTableColumns.Append(", COALESCE(H.HDR_GROUP_RID, "); headerTableColumns.Append(Include.DefaultHeaderRID.ToString()); headerTableColumns.Append(") AS HDR_GROUP_RID");
                headerTableColumns.Append(", COALESCE(H.SIZE_GROUP_RID, "); headerTableColumns.Append(Include.UndefinedSizeGroupRID.ToString()); headerTableColumns.Append(") AS SIZE_GROUP_RID");
                headerTableColumns.Append(", COALESCE(H.WORKFLOW_RID, "); headerTableColumns.Append(Include.UndefinedWorkflowRID.ToString()); headerTableColumns.Append(") AS WORKFLOW_RID");
                headerTableColumns.Append(", COALESCE(H.METHOD_RID, "); headerTableColumns.Append(Include.UndefinedMethodRID.ToString()); headerTableColumns.Append(") AS METHOD_RID");
                headerTableColumns.Append(", COALESCE(H.ALLOCATION_STATUS_FLAGS, 0) AS ALLOCATION_STATUS_FLAGS");
                headerTableColumns.Append(", COALESCE(H.BALANCE_STATUS_FLAGS, 0) AS BALANCE_STATUS_FLAGS");
                headerTableColumns.Append(", COALESCE(H.SHIPPING_STATUS_FLAGS, 0) AS SHIPPING_STATUS_FLAGS");
                headerTableColumns.Append(", COALESCE(H.ALLOCATION_TYPE_FLAGS, 0) AS ALLOCATION_TYPE_FLAGS");
                headerTableColumns.Append(", COALESCE(H.INTRANSIT_STATUS_FLAGS,0) AS INTRANSIT_STATUS_FLAGS");
                headerTableColumns.Append(", H.PERCENT_NEED_LIMIT");   // allow null value (desired value when null is double.MinValue but that value causes an error if we try to coalesce it
                headerTableColumns.Append(", COALESCE(H.PLAN_PERCENT_FACTOR, 100.0) AS PLAN_PERCENT_FACTOR");
                headerTableColumns.Append(", COALESCE(H.RESERVE_UNITS, 0) AS RESERVE_UNITS");
                headerTableColumns.Append(", H.GRADE_WEEK_COUNT");  // important to allow a null value here! 
                headerTableColumns.Append(", COALESCE(H.DIST_CENTER, ' ') AS DIST_CENTER");
                headerTableColumns.Append(", COALESCE(H.HEADER_NOTES, ' ') AS HEADER_NOTES");
                headerTableColumns.Append(", COALESCE(H.WORKFLOW_TRIGGER, '0') AS WORKFLOW_TRIGGER");  // MID Track 4191 Cannot trigger undefined workflow
                headerTableColumns.Append(", H.EARLIEST_SHIP_DAY");
                headerTableColumns.Append(", COALESCE(H.API_WORKFLOW_RID, "); headerTableColumns.Append(Include.UndefinedWorkflowRID.ToString()); headerTableColumns.Append(") AS API_WORKFLOW_RID");
                headerTableColumns.Append(", COALESCE(H.API_WORKFLOW_TRIGGER, '0') AS API_WORKFLOW_TRIGGER"); // MID Track 4191 Cannot trigger undefined workflow 
                headerTableColumns.Append(", COALESCE(H.ALLOCATED_UNITS, 0) AS ALLOCATED_UNITS");
                headerTableColumns.Append(", COALESCE(H.ORIG_ALLOCATED_UNITS, 0) AS ORIG_ALLOCATED_UNITS");
                headerTableColumns.Append(", COALESCE(H.RELEASE_COUNT, 0) AS RELEASE_COUNT");
                headerTableColumns.Append(", COALESCE(H.RSV_ALLOCATED_UNITS, 0) AS RSV_ALLOCATED_UNITS");
                headerTableColumns.Append(", COALESCE(H.ASRT_RID, -1) AS ASRT_RID");
                headerTableColumns.Append(", COALESCE(H.PLACEHOLDER_RID, -1) AS PLACEHOLDER_RID");
                headerTableColumns.Append(", COALESCE(H.ASRT_TYPE, 0) AS ASRT_TYPE");
                headerTableColumns.Append(", COALESCE(H.MANUALLYCHGDSTRSTYLALOCTNCNT, 0) AS MANUALLYCHGDSTRSTYLALOCTNCNT"); // MID Track 4448 ANF Audit Enhancement
                headerTableColumns.Append(", COALESCE(H.MANUALLYCHGDSTRSIZEALOCTNCNT, 0) AS MANUALLYCHGDSTRSIZEALOCTNCNT"); // MID Track 4448 ANF Audit Enhancement
                headerTableColumns.Append(", COALESCE(H.MANUALLYCHGDSTRSTYLEALOCTN,0) AS MANUALLYCHGDSTRSTYLEALOCTN");  // MID Track 4448 AnF Audit Enhancement
                headerTableColumns.Append(", COALESCE(H.MANUALLYCHGDSTRSIZEALOCTN,0) AS MANUALLYCHGDSTRSIZEALOCTN");  // MID Track 4448 AnF Audit Enhancement
                headerTableColumns.Append(", COALESCE(H.HORIZON_OVERRIDE, '0') AS HORIZON_OVERRIDE");                 // MID Track 4448 AnF Audit Enhancement
                headerTableColumns.Append(", COALESCE(H.STORES_WITH_ALOCTN_COUNT, 0) AS STORES_WITH_ALOCTN_COUNT");   // MID Track 4448 AnF Audit Enhancement
                headerTableColumns.Append(", COALESCE(H.GRADE_SG_RID, 1) AS GRADE_SG_RID");   // TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
                headerTableColumns.Append(", COALESCE(H.ASRT_PLACEHOLDER_SEQ, 0) AS ASRT_PLACEHOLDER_SEQ");   // TT#1227 - stodd - assortment
                headerTableColumns.Append(", COALESCE(H.ASRT_HEADER_SEQ, 0) AS ASRT_HEADER_SEQ");   // TT#1227 - stodd - assortment
                // Begin TT#1065 - JSmith - Style 12340 Color WSN was allocated with incorrect workflow
                headerTableColumns.Append(", H.DISPLAY_STATUS, H.DISPLAY_TYPE, H.DISPLAY_INTRANSIT, H.DISPLAY_SHIP_STATUS");
                // End TT#1065
                headerTableColumns.Append(", COALESCE(H.GRADE_INVENTORY_IND, '0') AS GRADE_INVENTORY_IND");  // TT#1287 - Jellis - Inventory Minimum and Maximum
                headerTableColumns.Append(", COALESCE(H.GRADE_INVENTORY_HNRID, "); headerTableColumns.Append(Include.NoRID.ToString()); headerTableColumns.Append(") AS GRADE_INVENTORY_HNRID");  // TT#1287  Jellis - Inventory Minimum and Maximum
                // Begin TT#1401 - RMatelic - Reservation Stores
                headerTableColumns.Append(", COALESCE(H.IMO_ID, ' ') AS IMO_ID");
                headerTableColumns.Append(", COALESCE(H.ITEM_UNITS_ALLOCATED, 0) AS ITEM_UNITS_ALLOCATED");
                headerTableColumns.Append(", COALESCE(H.ITEM_ORIG_UNITS_ALLOCATED, 0) AS ITEM_ORIG_UNITS_ALLOCATED");
                headerTableColumns.Append(", COALESCE(H.ASRT_RID, ' ') AS ASRT_ID"); //TT#403 - MD - DOConnell - Assortment ID is not displaying correctly in the Allocation Workspace
                headerTableColumns.Append(", COALESCE(H2.ASRT_TYPE, 0) AS ASRT_TYPE_PARENT"); //TT#893 - MD - stodd - add group allocation ID
				// End TT#893 - MD - stodd - Add "Group Allocation ID" Column to Allocation Workspace
                // End TT#1401 
                _headerTableColumns = headerTableColumns.ToString();
			}
			return _headerTableColumns;
		}
		// end MID Track 4033 allow multi header children to change

		public static string GetColorDisplay(string aColorID, string aColorName)
		{
			string colorDisplay;
			colorDisplay = aColorID + " [" + aColorName + "]";

			return colorDisplay;
		}

		/// <summary>
		/// Formats the size key
		/// </summary>
		/// <param name="productCategory">The product category of the size.</param>
		/// <param name="primary">The primary description of the size.</param>
		/// <param name="secondary">The secondary description of the size.</param>
		/// <returns></returns>
		public static string GetSizeKey(string productCategory, string primary, string secondary)
		{
			string sizeKey;
			if (secondary == NoSecondarySize || secondary == NoneText) // MID Track #3942 - add NoneText
			{
				secondary = "";
			}
			if (secondary != null && secondary.Trim().Length > 0)
			{
				sizeKey = productCategory + "|" + primary + "|" + secondary;
			}
			else
			{
				sizeKey = productCategory + "|" + primary;
			}
			return sizeKey;
		}

		/// <summary>
		/// Formats the size key
		/// </summary>
		/// <param name="primary">The primary description of the size.</param>
		/// <param name="secondary">The secondary description of the size.</param>
		/// <param name="ID">The ID of the size.</param>
		/// <returns></returns>
		public static string GetSizeName(string primary, string secondary, string ID)
		{
			string sizeName;
			if (secondary == NoSecondarySize || secondary == NoneText) // MID Track #3942 - add NoneText
			{
				secondary = "";
			}
			if (primary != null && primary.Trim().Length > 0)
			{
				if (secondary != null && secondary.Trim().Length > 0)
				{
					sizeName = primary + secondary;
				}
				else
				{
					sizeName = primary;  
				}
			}
			else
				if (secondary != null && secondary.Trim().Length > 0)
			{
				sizeName = secondary;  
			}
			else
			{
				sizeName = ID;
			}
			return sizeName;
		}

		/// <summary>
		/// Recursively disposes all controls in the form
		/// </summary>
		/// <param name="aControls">The ControlCollection to recursively dispose</param>
		public static void DisposeControls(System.Windows.Forms.Control.ControlCollection aControls)
		{
			try
			{
				Stack controlStack = new Stack();

				// create separate stack of controls since dispose removes control from collection
				// which disrupts the iterator
				foreach (Control control in aControls)
				{
					if (control != null)
					{
						controlStack.Push(control);
					}
				}

				while (controlStack.Count > 0)
				{
					Control control = (Control)controlStack.Pop();
                    if (control is System.Windows.Forms.SplitContainer)
                    {
                        continue;
                    }
					// recurse for the controls of this control
					DisposeControls(control.Controls);
					// dispose of this control
                    if (!control.IsDisposed &&
                        !control.Disposing)
                    {
                        try
                        {
                            DisposeControl(control);
                        }
                        catch (Exception)
                        {
                            // eat errors
                        }
                    }
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Disposes a control
		/// </summary>
		/// <param name="aControl">The Control being disposed</param>
		public static void DisposeControl(System.Windows.Forms.Control aControl)
		{
			try
			{
				if (aControl != null)
				{
					// if the control has a context menu, dispose of the context menu first to remove reference
					if (aControl.ContextMenu != null)
					{
						aControl.ContextMenu.Dispose();
					}
					// Clear DataSource if control has property
					if (aControl.GetType().GetProperty("DataSource") != null)
					{
						PropertyInfo pi = aControl.GetType().GetProperty("DataSource");
						pi.SetValue(aControl, null, null);
					}
					// Clear DropMode if control is Flex grid
					if (aControl is C1.Win.C1FlexGrid.C1FlexGrid)
					{
						((C1FlexGrid)aControl).DropMode = C1.Win.C1FlexGrid.DropModeEnum.None;
					}
	
                    try
                    {
                        if (!aControl.Disposing)
                        {
                            aControl.Dispose();
                        }
                    }
                    catch (Exception)
                    {
                       // eat errors
                    }
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public static int CreateHashKey(int aKey1, int aKey2)
		{
			try
			{
				return ((aKey1 & 0xFFFF) << 16) | (aKey2 & 0xFFFF);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public static int CreateHashKey(int aKey1, int aKey2, int aKey3)
		{
			try
			{
				return ((aKey1 & 0x0FFF) << 20) | ((aKey2 & 0x0FFF) << 8) | (aKey3 & 0xFF);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public static int CreateHashKey(int aKey1, int aKey2, int aKey3, int aKey4)
		{
			try
			{
				return ((aKey1 & 0xFF) << 24) | ((aKey2 & 0xFF) << 16) | ((aKey3 & 0xFF) << 8) | (aKey4 & 0xFF);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public static int CreateHashKey(params int[] aKeys)
		{
			try
			{
				switch (aKeys.Length)
				{
					case 1:

						return aKeys[0];
                        //break;

					case 2:

						return ((aKeys[0] & 0xFFFF) << 16) | (aKeys[1] & 0xFFFF);
                        //break;

					case 3:

						return ((aKeys[0] & 0x0FFF) << 20) | ((aKeys[1] & 0x0FFF) << 8) | (aKeys[2] & 0xFF);
                        //break;

					default:

						return ((aKeys[0] & 0xFF) << 24) | ((aKeys[1] & 0xFF) << 16) | ((aKeys[2] & 0xFF) << 8) | (aKeys[3] & 0xFF);
                        //break;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public static string CreateNodeLockKey(int aParentRID, string aNodeID)
		{
			try
			{
				return aParentRID.ToString() + "|" + aNodeID;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// BEGIN MID Track 4279 - trim spaces off string fields
		/// <summary>
		/// This method will recursively check all objects in the object and trim all string fields
		/// </summary>
		/// <param name="aObject">The object to trim</param>
		/// <remarks>Currently supports objects and Arrays</remarks>
		public static void TrimStringsInObject(object aObject)
		{
			try
			{
                // Begin TT#1824 - JSmith - Fields not trimming
                //FieldInfo[] fields = aObject.GetType().GetFields();
                FieldInfo[] fields = aObject.GetType().GetFields(BindingFlags.Instance |
                       BindingFlags.Static |
                       BindingFlags.NonPublic |
                       BindingFlags.Public);
                // ENd TT#1824
				foreach(FieldInfo field in fields)
				{
					object fieldValue = field.GetValue(aObject);
					if (field.FieldType == typeof(System.String))
					{
						if (fieldValue != null)
						{
							string trimStrValue = (Convert.ToString(fieldValue)).Trim();
							field.SetValue(aObject, trimStrValue);
						}
					}
					else if (field.FieldType.BaseType == typeof(System.Array))
					{
						if (fieldValue != null)
						{
							object[] objects = (object[])fieldValue;
							foreach (object Object in objects)
							{
								TrimStringsInObject(Object);
							}
						}
					}
				}
			}
			catch
			{
				throw;
			}
		}
		// END MID Track 4279

        public static string GetMenuItem(eMIDMenuItem aMenuItem)
        {
            try
            {
                string menuItem = string.Empty;
                switch (aMenuItem)
                {
                    case eMIDMenuItem.FileNew:
                        menuItem = Include.btNew;
                        break;
                    case eMIDMenuItem.FileClose:
                        menuItem = Include.btClose;
                        break;
                    case eMIDMenuItem.FileSave:
                        menuItem = Include.btSave;
                        break;
                    case eMIDMenuItem.FileSaveAs:
                        menuItem = Include.btSaveAs;
                        break;
                    case eMIDMenuItem.EditCut:
                        menuItem = Include.btCut;
                        break;
                    case eMIDMenuItem.EditCopy:
                        menuItem = Include.btCopy;
                        break;
                    case eMIDMenuItem.EditPaste:
                        menuItem = Include.btPaste;
                        break;
                    case eMIDMenuItem.EditDelete:
                        menuItem = Include.btDelete;
                        break;
                    case eMIDMenuItem.EditClear:
                        menuItem = Include.btClear;
                        break;
                    case eMIDMenuItem.EditFind:
                        menuItem = Include.btFind;
                        break;
                    case eMIDMenuItem.ToolsRestoreLayout:       // BEGIN Workspace Usability Enhancement
                        menuItem = Include.btRestoreLayout;
                        break;                                  // END Workspace Usability Enhancement
                    // Begin TT#4018 - stodd - Export from Characteristics Tab should be disabled
                    case eMIDMenuItem.FileExport:
                        menuItem = Include.btExport;
                        break;
                    // End TT#4018 - stodd - Export from Characteristics Tab should be disabled
                }

                return menuItem;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		public static bool IsExcelInstalled()
		{
			try
			{
				return RegistryContains("Excel.Application");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private static bool RegistryContains(string aKeyValue)
		{
			try
			{
				RegistryKey rk = Registry.ClassesRoot;
				string[] names = rk.GetSubKeyNames();
				foreach (string name in names)
				{
					if (name.ToLower() == aKeyValue.ToLower())
					{
						return true;
					}
				}

				return false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin Track #5486 - JSmith - Export fails
        public static string FormatExcelSheetName(string aSheetName)
        {
            string sheetName;
            string oldChar, newChar;
            string replaceList;
            try
            {
                newChar = "-";
                replaceList = @"*?/[]|:";
                sheetName = aSheetName;

                for (int i = 0; i < replaceList.Length; i++)
                {
                    oldChar = replaceList.Substring(i, 1);
                    sheetName = sheetName.Replace(oldChar, newChar);
                }

                return sheetName;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End Track #5486

		//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		public static string FormatFileName(string aFileName)
		{
			string fileName;
			string oldChar, newChar;
			string replaceList;
			try
			{
				newChar = "-";
				replaceList = @"\\/:*?""<>!";
				fileName = aFileName;

				for (int i = 0; i < replaceList.Length; i++)
				{
					oldChar = replaceList.Substring(i, 1);
					fileName = fileName.Replace(oldChar, newChar);
				}

				return fileName;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

        // Begin TT#698 - JSmith - Enhance environment information
        public static System.Diagnostics.FileVersionInfo GetMainAssemblyInfo()
        {
            try
            {
                // Begin TT#2228 - JSmith - Make version numbers consistent
                //string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().Location;
                //return System.Diagnostics.FileVersionInfo.GetVersionInfo(assemblyName);

                string assemblyName = Path.GetDirectoryName(Application.ExecutablePath) + @"\" + "MIDRetail.Windows.dll";
                return System.Diagnostics.FileVersionInfo.GetVersionInfo(assemblyName);
                // End TT#2228
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#698

        // Begin TT#195 MD - JSmith - Add environment authentication
        public static System.Diagnostics.FileVersionInfo GetCurrentAssemblyInfo()
        {
            try
            {
                return System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#195 MD

		//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
		public static eMIDMessageLevel TranslateErrorLevel(eErrorLevel aErrorLevel)
		{
			switch (aErrorLevel)
			{
				case eErrorLevel.fatal:
				case eErrorLevel.severe:
					return eMIDMessageLevel.Severe;
				//Begin TT#1020 - JScott - Add new Error level to the eErrorLevel enumerator
				case eErrorLevel.error:
					return eMIDMessageLevel.Error;
				//End TT#1020 - JScott - Add new Error level to the eErrorLevel enumerator
				case eErrorLevel.warning:
					return eMIDMessageLevel.Warning;
			}
			 
			return eMIDMessageLevel.Information;
		}
		//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"

		// BEGIN TT#1766 - stodd - fifo file processing
		//Begin TT#1281 - JScott - WUB header load failed
		public static string[] GetFiles(string path, string searchPattern)
		{
			try
			{
				return GetFiles(path, searchPattern, eAPIFileProcessingDirection.Default);
			}
			catch
			{
				throw;
			}
		}
		//End TT#1281 - JScott - WUB header load failed

        // Begin TT#3521 - JSmith - Header Load delay
        //public static string[] GetFiles(string path, string searchPattern, eAPIFileProcessingDirection direction)
        //{
        //    string[] files;
        //    Regex regex;
        //    ArrayList outStrs;

        //    try
        //    {
        //        files = Directory.GetFiles(path, "*.*");
        //        DirectoryInfo dirinfo = default(DirectoryInfo);
        //        FileInfo[] allFiles = null;
        //        dirinfo = new DirectoryInfo(path);
        //        allFiles = dirinfo.GetFiles("*.*");

        //        searchPattern = String.Format(".*{0}", searchPattern.Replace("*.", "\\."));
        //        regex = new Regex(searchPattern, RegexOptions.IgnoreCase);
        //        outStrs = new ArrayList();

        //        //===============================================================
        //        // NOTE on Direction
        //        // Because the method that calls this push the file names
        //        // unto a stack and later pops them off, The files are initially 
        //        // sorted the oposite direction. Then the push and pop will 
        //        // reverse them to be correct.
        //        //===============================================================
        //        switch (direction)
        //        {
        //            case eAPIFileProcessingDirection.FIFO:
        //                Array.Sort(allFiles, new clsCompareFileInfoFILO());
        //                break;
        //            case eAPIFileProcessingDirection.FILO:
        //                Array.Sort(allFiles, new clsCompareFileInfoFIFO());
        //                break;
        //            default:
        //                break;
        //        }

        //        //foreach (string str in files)
        //        foreach (FileInfo fileInfo in allFiles)
        //        {
        //            string str = fileInfo.FullName;
        //            if (regex.IsMatch(str))
        //            {
        //                outStrs.Add(str);
        //            }
        //        }

        //        return (string[])outStrs.ToArray(typeof(string));
        //    }
        //    catch (IOException ex)
        //    {
        //        if (System.Runtime.InteropServices.Marshal.GetHRForException(ex) == 0x80070012)
        //        {
        //            return new string[0];
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
		// END TT#1766 - stodd - fifo file processing
		
		
		public static string[] GetFiles(string path, string searchPattern, eAPIFileProcessingDirection direction)
		{
			Regex regex;
			ArrayList outStrs;

			try
			{
				DirectoryInfo dirinfo = default(DirectoryInfo);
				FileInfo[] allFiles = null;
				dirinfo = new DirectoryInfo(path);
                if (searchPattern == null ||
                    searchPattern.Trim().Length == 0)
                {
                    searchPattern = "*.*";
                }
                allFiles = dirinfo.GetFiles(searchPattern);

                outStrs = new ArrayList();
                if (allFiles.Length > 0)
                {
                    // Begin TT#4591 - JSmith - Error: 111034:Cooresponding data file not found
                    //searchPattern = String.Format(".*{0}", searchPattern.Replace("*.", "\\."));
                    searchPattern = String.Format(".*{0}$", searchPattern.Replace("*.", "\\."));
                    // End TT#4591 - JSmith - Error: 111034:Cooresponding data file not found
                    regex = new Regex(searchPattern, RegexOptions.IgnoreCase);


                    //===============================================================
                    // NOTE on Direction
                    // Because the method that calls this push the file names
                    // unto a stack and later pops them off, The files are initially 
                    // sorted the oposite direction. Then the push and pop will 
                    // reverse them to be correct.
                    //===============================================================
                    switch (direction)
                    {
                        case eAPIFileProcessingDirection.FIFO:
                            Array.Sort(allFiles, new clsCompareFileInfoFILO());
                            break;
                        case eAPIFileProcessingDirection.FILO:
                            Array.Sort(allFiles, new clsCompareFileInfoFIFO());
                            break;
                        default:
                            break;
                    }

                    //foreach (string str in files)
                    foreach (FileInfo fileInfo in allFiles)
                    {
                        string str = fileInfo.FullName;
                        if (regex.IsMatch(str))
                        {
                            outStrs.Add(str);
                        }
                    }
                }

				return (string[])outStrs.ToArray(typeof(string));
			}
			catch (IOException ex)
			{
				if (System.Runtime.InteropServices.Marshal.GetHRForException(ex) == 0x80070012)
				{
					return new string[0];
				}
				else
				{
					throw;
				}
			}
			catch
			{
				throw;
			}
		}
        // End TT#3521 - JSmith - Header Load delay

        // Begin TT#1159 - JSmith - Improve Messaging
        public static eErrorLevel TranslateMessageLevel(eMIDMessageLevel aMessageLevel)
        {
            switch (aMessageLevel)
            {
                case eMIDMessageLevel.Severe:
                    return eErrorLevel.severe;
                case eMIDMessageLevel.Error:
                    return eErrorLevel.error;
                case eMIDMessageLevel.Warning:
                    return eErrorLevel.warning;
            }

            return eErrorLevel.information;
        }
        // End TT#1159

        // Begin TT#4106 - JSmith - Task List Explorer - Allows users to copy/paste read only system task lists.
        public static ArrayList GetAvailableTasks(FunctionSecurityProfile aUserSecurity, FunctionSecurityProfile aGlobalSecurity, FunctionSecurityProfile aSystemSecurity, bool aIsSizeInstalled)
        {
            ArrayList alTasks = new ArrayList();

            alTasks.Add(eTaskType.Allocate);
            alTasks.Add(eTaskType.Forecasting);
            alTasks.Add(eTaskType.computationDriver);

            if (aIsSizeInstalled)
            {
                alTasks.Add(eTaskType.SizeCurveMethod);
                alTasks.Add(eTaskType.SizeCurves);
            }

            alTasks.Add(eTaskType.None);
            alTasks.Add(eTaskType.Rollup);

            if (aSystemSecurity.AllowUpdate)
            {
                alTasks.Add(eTaskType.Purge);
                alTasks.Add(eTaskType.None);
                alTasks.Add(eTaskType.StoreLoad);
                alTasks.Add(eTaskType.HierarchyLoad);
                alTasks.Add(eTaskType.ChainSetPercentCriteriaLoad);
                alTasks.Add(eTaskType.DailyPercentagesCriteriaLoad); 
                alTasks.Add(eTaskType.PushToBackStockLoad);
                alTasks.Add(eTaskType.HistoryPlanLoad);
                alTasks.Add(eTaskType.HeaderReconcile);		// TT#1581-MD - stodd - API Header Reconcile
                alTasks.Add(eTaskType.HeaderLoad);
                alTasks.Add(eTaskType.BuildPackCriteriaLoad);
                alTasks.Add(eTaskType.RelieveIntransit);
                alTasks.Add(eTaskType.StoreEligibilityCriteriaLoad);
                alTasks.Add(eTaskType.VSWCriteriaLoad);
                if (aIsSizeInstalled)
                {
                    alTasks.Add(eTaskType.SizeCodeLoad);
                    alTasks.Add(eTaskType.SizeCurveLoad);
                    alTasks.Add(eTaskType.SizeConstraintsLoad);
                    alTasks.Add(eTaskType.ColorCodeLoad);
                    alTasks.Add(eTaskType.SizeDayToWeekSummary);
                }
                alTasks.Add(eTaskType.BatchComp);		// TT#1595-MD - stodd - batch comp
                alTasks.Add(eTaskType.None);
                alTasks.Add(eTaskType.ExternalProgram);
            }

            //if (aIsSizeInstalled)
            //{
            //    alTasks.Add(eTaskType.SizeDayToWeekSummary);
            //}

            //if (aSystemSecurity.AllowUpdate)
            //{
            //    alTasks.Add(eTaskType.None);
            //    alTasks.Add(eTaskType.ExternalProgram);
            //}

            return alTasks;
        }
        // End TT#4106 - JSmith - Task List Explorer - Allows users to copy/paste read only system task lists.
	}
     
    // begin TT#41 - MD - Jellis - Size Inventory Min Max pt 1
    public class MID_HashCode
    {
        public static int CalculateHashCode(Object[] Parms)
        {
            int hashCode = 23;
            foreach (Object obj in Parms)
            {
                hashCode = (hashCode << 5) * 37 + (obj == null ? 0: obj.GetHashCode());
            }
            return hashCode;
        }
    }
    // end   TT#41 - MD - Jellis - Size Inventory Min Max pt 1

	/// <summary>
	/// Need is defined as the inventory required to achieve a planned, future on-hand level.
	/// Percent need is a measurement of need relative to plan.
	/// </summary>
	/// <remarks>
	/// The Need class contains static methods that calculate a Unit OTS Plan, Unit Need and Percent Need.
	/// The static methods are:
	/// <list type="bullet">
	/// <item>UnitOTSPlan: Input for this method is an ending inventory plan and a sales plan for a for a selling horizon. Output from this method is a single plan value.</item>
	/// <item>UnitNeed: Input for this method is a UnitOTSPlan, a beginning onhand value and an intransit value for the selling horizon used to calculate the UnitOTSPlan and any units already allocated. Output from this method is the unit need.</item>
	/// <item>PctUnitNeed: Input for this method is a UnitNeed and a UnitOTSPlan (the same plan used to calculate UnitNeed). Output from this method is the PctUnitNeed.</item>
	/// </list> 
	/// </remarks>
	public class Need
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
		/// <summary>
		/// UnitOTSPlan calculates an OTS plan for a given ending inventory plan and a sales
		/// plan for a shipping horizon (weekly sales plan for a range of weeks)
		/// </summary>
		/// <param name="aEndInventoryPlan">
		/// The planned ending inventory level at the end of the shipping horizon (range of weeks).
		/// </param>
		/// <param name="aSalesPlan">
		/// The weekly sales plans for a shipping horizon (a range of weeks).
		/// </param>
		/// <returns>
		/// The unit open to ship plan.
		/// </returns>
		public static double UnitOTSPlan (double aEndInventoryPlan, double []aSalesPlan)
		{
			double i = Math.Max(aEndInventoryPlan,0.0d);
			foreach (double _salesPlan in aSalesPlan)
			{
				if (_salesPlan > 0)
				{
					i += _salesPlan;
				}
			}
			return i;
		}

		/// <summary>
		/// UnitNeed calculates the unit need for a shipping horizon (range of weeks) using 
		/// the given ownership (onhand, intransit and units allocated) and OTS unit plan for the horizon 
		/// </summary>
		/// <param name="aUnitOTSPlan">The unit Open To Ship combined sales and inventory plan for the shipping horizon</param>
		/// <param name="aOnhand">Units owned at the beginning of the shipping horizon.</param>
		/// <param name="aIntransit">The total units intransit for the shipping horizon</param>
		/// <param name="aUnitsAllocated">Units already allocated.  For a planning funcion, this value is zero.</param>
		/// <returns>Unit Need for the shipping horizon.</returns>
		public static double UnitNeed(double aUnitOTSPlan, double aOnhand, int aIntransit, int aUnitsAllocated)
		{
			double i = aUnitOTSPlan;
			if (aIntransit > 0)
			{
				i -= (double) aIntransit;
			}
			if (aUnitsAllocated > 0)
			{
				i -= (double) aUnitsAllocated;
			}
			if (aOnhand > 0)
			{
				i -= aOnhand;
			}
			return i;
		}

		/// <summary>
		/// PctUnitNeed for a shipping horizon using the given Unit Need and Unit OTS Plan for the shipping horizon
		/// </summary>
		/// <param name="aUnitNeed">
		/// The Unit Need for the shipping horizon (range of weeks).
		/// </param>
		/// <param name="aUnitOTSPlan">
		/// The combined sales and ending inventory plans for the shipping horizon.
		/// </param>
		/// <returns>
		/// Percent Unit Need.
		/// </returns>
		public static double PctUnitNeed (double aUnitNeed, double aUnitOTSPlan)
		{
			if (aUnitOTSPlan > 0)
			{
				return (aUnitNeed * 100) / aUnitOTSPlan;
			}
			else
			{
				return (double) 0.0;
			}
		}
	}

	public struct TriBool
	{
		public static readonly TriBool Null = new TriBool(0);
		public static readonly TriBool False = new TriBool(-1);
		public static readonly TriBool True = new TriBool(1);
		private int _value; 

		private TriBool(int aValue) 
		{
			_value = aValue;
		}

		public static implicit operator TriBool(bool x) 
		{
			return x ? True : False;
		}

		public static explicit operator bool(TriBool x) 
		{
			return x._value > 0;
		}

		public static bool operator ==(TriBool x, TriBool y) 
		{
			return x._value == y._value ? true : false;
		}

		public static bool operator !=(TriBool x, TriBool y) 
		{
			return x._value != y._value ? true : false;
		}

		public static TriBool operator !(TriBool x) 
		{
			return new TriBool(-x._value);
		}

		public static TriBool operator &(TriBool x, TriBool y) 
		{
			return new TriBool(x._value < y._value ? x._value : y._value);
		}

		public static TriBool operator |(TriBool x, TriBool y) 
		{
			return new TriBool(x._value > y._value ? x._value : y._value);
		}

		public static bool operator true(TriBool x) 
		{
			return x._value > 0;
		}

		public static bool operator false(TriBool x) 
		{
			return x._value < 0;
		}

		public static implicit operator string(TriBool x) 
		{
			return x._value > 0 ? "True" : x._value < 0 ? "False" : "Null";
		}

		public override bool Equals(object obj) 
		{
			if (obj.GetType() == this.GetType() || obj.GetType().IsSubclassOf(this.GetType()))
			{
				return (bool)((TriBool)obj == this);
			}
			else
			{
				return false;
			}

		}

		public override int GetHashCode() 
		{
			return _value;
		}

		public override string ToString() 
		{
			switch (_value) 
			{
				case -1:
					return "TriBool.False";
				case 0:
					return "TriBool.Null";
				case 1:
					return "TriBool.True";
				default:
					return "Invalid Value";
			}
		}
	}
//Begin Track #5026 - JScott - Specific Period and Variable protected even when weeks are not

	/// <summary>
	/// This class defines an object that is used as a key to a Hashtable.
	/// </summary>

	public class HashKeyObject : IComparable
	{
		//=======
		// FIELDS
		//=======

		private int[] _keys;
		private int _hashCode = -1;

		//=============
		// CONSTRUCTORS
		//=============

		public HashKeyObject(params int[] aKeys)
		{
			try
			{
				if (aKeys.Length < 2)
				{
					throw new Exception("Cannot create HashKeyObject with less than 2 keys");
				}

				_keys = new int[aKeys.Length];
				aKeys.CopyTo(_keys, 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override public int GetHashCode()
		{
			try
			{
				if (_hashCode == -1)
				{
					switch (_keys.Length)
					{
						case 2:
							_hashCode = Include.CreateHashKey(_keys[0], _keys[1]);
							break;
						case 3:
							_hashCode = Include.CreateHashKey(_keys[0], _keys[1], _keys[2]);
							break;
						default:
							_hashCode = Include.CreateHashKey(_keys[0], _keys[1], _keys[2], _keys[3]);
							break;
					}
				}

				return _hashCode;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public bool Equals(object obj)
		{
			try
			{
				return CompareTo(obj) == 0;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public int CompareTo(object obj)
		{
			int i;
			HashKeyObject comp;

			try
			{
				if (obj.GetType() == typeof(HashKeyObject))
				{
					comp = (HashKeyObject)obj;

					for (i = 0; i < _keys.Length && _keys[i] == comp._keys[i]; i++);

					if (i == _keys.Length)
					{
						return 0;
					}
					else if (_keys[i] < comp._keys[i])
					{
						return -1;
					}
					else
					{
						return 1;
					}
				}
				else
				{
					return -1;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
//End Track #5026 - JScott - Specific Period and Variable protected even when weeks are not

	// BEGIN TT#1766 - stodd - fifo file processing
	public class clsCompareFileInfoFIFO : IComparer
	{
		public int Compare(object x, object y)
		{
			FileInfo File1 = null;
			FileInfo File2 = null;
			File1 = (FileInfo)x;
			File2 = (FileInfo)y;
            // Begin TT#584-MD - JSmith - File order processing (FIFO, LIFO) may process files in wrong order
            File1.Refresh();
            File2.Refresh();
            // End TT#584-MD - JSmith - File order processing (FIFO, LIFO) may process files in wrong order
			return DateTime.Compare(File1.LastWriteTime, File2.LastWriteTime);
		}
	}
	public class clsCompareFileInfoFILO : IComparer
	{
		public int Compare(object x, object y)
		{
			FileInfo File1 = null;
			FileInfo File2 = null;
			File1 = (FileInfo)x;
			File2 = (FileInfo)y;
            // Begin TT#584-MD - JSmith - File order processing (FIFO, LIFO) may process files in wrong order
            File1.Refresh();
            File2.Refresh();
            // End TT#584-MD - JSmith - File order processing (FIFO, LIFO) may process files in wrong order
			return DateTime.Compare(File2.LastWriteTime, File1.LastWriteTime);
		}
	}
	// END TT#1766 - stodd - fifo file processing

    // Begin TT#2621 - JSmith - 
    #region DailyPercentagesComparer
    /// <summary>
    /// Compares two Allocation profiles based on the units allocated to a specific store in the GeneralComponent.
    /// </summary>
    public class DailyPercentagesComparer : IComparer
    {

        public int Compare(object x, object y)
        {
            if (!((x is DateRangeProfile)
                || (y is DateRangeProfile)))
            {
                throw new MIDException(eErrorLevel.severe, 0, "Invalid types in DailyPercentagesComparer");
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            if (((DateRangeProfile)x).StartDateKey
                < ((DateRangeProfile)y).StartDateKey)
            {
                return -1;
            }
            else if (((DateRangeProfile)x).StartDateKey
                == ((DateRangeProfile)y).StartDateKey)
            {
                if (((DateRangeProfile)x).EndDateKey
                >= ((DateRangeProfile)y).EndDateKey)
                {
                    return -1;
                }
                else
                {
                    return +1;
                }
            }
            else if (((DateRangeProfile)x).EndDateKey
                >= ((DateRangeProfile)y).EndDateKey)
            {
                return +1;
            }
            return -1;
        }
    }
    #endregion DailyPercentagesComparer

    #region SortComparer
    public class AscendedDateComparer : IComparer<DateTime>
    {
        public int Compare(DateTime x, DateTime y)
        {
            return x.CompareTo(y);
        }
    }

    public class DescendedDateComparer : IComparer<DateTime>
    {
        public int Compare(DateTime x, DateTime y)
        {
            return y.CompareTo(x);
        }
    }

    public class AscendedIntComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            return x.CompareTo(y);
        }
    }

    public class DescendedIntComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            return y.CompareTo(x);
        }
    }

    public class AscendedDoubleComparer : IComparer<double>
    {
        public int Compare(double x, double y)
        {
            return x.CompareTo(y);
        }
    }

    public class DescendedDoubleComparer : IComparer<double>
    {
        public int Compare(double x, double y)
        {
            return y.CompareTo(x);
        }
    }

    public class AscendedStringComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return x.CompareTo(y);
        }
    }

    public class DescendedStringComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return y.CompareTo(x);
        }
    }

    #endregion SortComparer

    // Begin TT#1581-MD - stodd - Header Reconcile API
    #region HeaderKeys
    public class HeaderKeys
    {
        // Begin TT#1966-MD - JSmith - DC Fulfillment
        //public enum lineType { Ignore, HeaderKeysToMatch, HeaderIdKeys }
        public enum lineType { Ignore, HeaderKeysToMatch, HeaderIdKeys, MasterHeaderIdKeys }
        // End TT#1966-MD - JSmith - DC Fulfillment

        /// <summary>
        /// Reads the Header Keys files and builds the key lists for how to match headers and how to generate header IDs.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="headerKeysToMatchList"></param>
        /// <param name="HeaderIdKeysList"></param>
        // Begin TT#1966-MD - JSmith - DC Fulfillment
        //public static bool LoadKeys(string fileName, ref List<string> headerKeysToMatchList, ref List<string> HeaderIdKeysList, ref string errorMessage)
        public static bool LoadKeys(string fileName, ref List<string> headerKeysToMatchList, ref List<string> HeaderIdKeysList, ref List<string> MasterHeaderIdKeysList, ref string errorMessage)
        // End TT#1966-MD - JSmith - DC Fulfillment
        {
            StreamReader sr = null;
            bool success = true;
            try
            {
                sr = new StreamReader(fileName);
                lineType lineType = lineType.Ignore;
                string line = sr.ReadLine();
                while (line != null)
                {
                    if (line.StartsWith("HeaderKeysToMatch:"))
                    {
                        line = TrimStart(line, "HeaderKeysToMatch:");
                        lineType = lineType.HeaderKeysToMatch;
                    }
                    else if (line.StartsWith("HeaderIdKeys:"))
                    {
                        line = TrimStart(line, "HeaderIdKeys:");
                        lineType = lineType.HeaderIdKeys;
                    }
                    // Begin TT#1966-MD - JSmith - DC Fulfillment
                    else if (line.StartsWith("MasterHeaderIdKeys:"))
                    {
                        line = TrimStart(line, "MasterHeaderIdKeys:");
                        lineType = lineType.MasterHeaderIdKeys;
                    }
                    // End TT#1966-MD - JSmith - DC Fulfillment
                    else
                    {
                        lineType = lineType.Ignore;
                    }


                    if (lineType == lineType.HeaderKeysToMatch)
                    {
                        headerKeysToMatchList = MIDstringTools.SplitGeneric(line.ToString().Trim().ToUpper(), ',', true);
                    }

                    if (lineType == lineType.HeaderIdKeys)
                    {
                        HeaderIdKeysList = MIDstringTools.SplitGeneric(line.ToString().Trim().ToUpper(), ',', true);
                    }

                    // Begin TT#1966-MD - JSmith - DC Fulfillment
                    if (lineType == lineType.MasterHeaderIdKeys)
                    {
                        MasterHeaderIdKeysList = MIDstringTools.SplitGeneric(line.ToString().Trim().ToUpper(), ',', true);
                    }
                    // End TT#1966-MD - JSmith - DC Fulfillment

                    line = sr.ReadLine();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }

                if (headerKeysToMatchList == null || (headerKeysToMatchList.Count == 1 && headerKeysToMatchList[0] == null))
                {
                    errorMessage = "No Keys To Match headers on were found in file at:" + fileName;
                    success = false;
                }
                else if (headerKeysToMatchList.Count == 1 && headerKeysToMatchList[0] == "UNDEFINED")
                {
                    errorMessage = "Keys To Match headers on have not been defined in file at:" + fileName;
                    success = false;
                }
                else if (HeaderIdKeysList == null || (HeaderIdKeysList.Count == 1 && HeaderIdKeysList[0] == null))
                {
                    errorMessage = "No Header ID keys were found in file at:" + fileName;
                    success = false;
                }
                else if (HeaderIdKeysList.Count == 1 && HeaderIdKeysList[0] == "UNDEFINED")
                {
                    errorMessage = "Header ID keys have not been defined in file at:" + fileName;
                    success = false;
                }

                // Begin TT#1605-MD - stodd - Adding 'sequence' to other header ID keys causes sequence to replace all other keys.
                //=======================================================================
                // By itself, 'Sequence' is always valid.
                // When included with a list of other header ID keys, it is ignored
                //   and removed from the list 
                //=======================================================================
                if (HeaderIdKeysList.Contains(Include.Sequence) && HeaderIdKeysList.Count > 1)
                {
                    HeaderIdKeysList.Remove(Include.Sequence);
                }
                // End TT#1605-MD - stodd - Adding 'sequence' to other header ID keys causes sequence to replace all other keys.
            }

            return success;
        }

        static string TrimStart(string target, string trimChars)
        {
            return target.TrimStart(trimChars.ToCharArray());
        }
    }
    #endregion HeaderKeys
	// End TT#1581-MD - stodd - Header Reconcile API
}
