//// Begin Track #4637 - JSmith - Split variables by type
//// Too many lines changed to mark.  Use SCM Compare for details.
//// End Track #4637
//// Begin TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables
////
//// Too many changes to mark. Compare for differences
//// Also removed old commend code for readability
////
//// End TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables
//using System;
//using System.Collections;
//using System.Configuration;
//using System.Data.SqlTypes;
//using System.Globalization;
//using System.IO;

//using MIDRetail.Business;
//using MIDRetail.DataCommon;
//using MIDRetail.ForecastComputations;

//namespace MIDRetail.DatabaseUpdate
//{
//    /// <summary>
//    /// Summary description for DatabaseObjectGen.
//    /// </summary>
//    public class DatabaseObjectGen
//    {
//        private eDatabaseType _databaseType;

//        /// <summary>
//        /// The main entry point for the application.
//        /// </summary>
//        /// 
//        public DatabaseObjectGen(eDatabaseType aDatabaseType)
//        {
//            _databaseType = aDatabaseType;
//        }

//        public int GenFile(string aFileName, bool aGenerateTables, bool aGenerateStoredProcedures,
//            int aNoDataTables, string aAllocationFileGroup, string aForecastFileGroup, string aHistoryFileGroup,
//            int aNoHistoryFileGroup, string aDailyHistoryFileGroup, int aNoDailyHistoryFileGroup, string aAuditFileGroup, string aWeekArchiveFileGroup, string aDayArchiveFileGroup) // TT#173 Provide database container for large data collections
//        {
//            GenDatabase database;
//            ArrayList databaseVariables = new ArrayList();
//            try
//            {
//                //					string input = ".\\database.sql";
//                StreamWriter writer = new StreamWriter(aFileName);

//                if (_databaseType == eDatabaseType.SQLServer2012)
//                {
//                    database = new GenSQLServer2012();
//                }
//                else if (_databaseType == eDatabaseType.SQLServer2008)
//                {
//                    database = new GenSQLServer2008();
//                }
//                else 
//                {
//                    database = new GenSQLServerBase();
//                }
							
//                bool generateTables = aGenerateTables;

//                bool generateStoredProcedures = aGenerateStoredProcedures;

//                int tableCount = aNoDataTables;

//                PlanComputationsCollection compCollections = new PlanComputationsCollection();
//                IPlanComputationVariables variables = compCollections.GetDefaultComputations().PlanVariables;
//                databaseVariables = variables.GetDatabaseVariableList().ArrayList;

//                if (generateTables)
//                {
//                    Generate_Database_Tables(database, variables, writer,  tableCount, 
//                        aAllocationFileGroup,  aForecastFileGroup, aHistoryFileGroup,
//                        aNoHistoryFileGroup,  aDailyHistoryFileGroup,  aNoDailyHistoryFileGroup, aAuditFileGroup, aWeekArchiveFileGroup, aDayArchiveFileGroup); // TT#173 Provide database container for large data collections
//                }
//                if (generateStoredProcedures)
//                {
//                    Generate_Stored_Procedures(database, variables, writer, tableCount);
//                }

//                writer.Close();
//                return 0;
//            }
//            catch (Exception ex)
//            {
//                string message = ex.ToString();
//                throw;
//            }
//        }

//        private void Generate_Database_Tables(GenDatabase aDatabase, IPlanComputationVariables aVariables, StreamWriter aWriter, 
//            int aTableCount, string aAllocationFileGroup, string aForecastFileGroup, string aHistoryFileGroup,
//            int aNoHistoryFileGroup, string aDailyHistoryFileGroup, int aNoDailyHistoryFileGroup, string aAuditFileGroup, string aWeekArchiveFileGroup, string aDayArchiveFileGroup)  // TT#173 Provide database container for large data collections
//        {
//            try
//            {
//                aDatabase.Generate_STORE_FORECAST_WEEK_LOCK(Include.DBStoreWeeklyLockTable, aVariables.GetStoreWeeklyForecastDatabaseVariableList(), aWriter, aForecastFileGroup);
//                aDatabase.Generate_CHAIN_FORECAST_WEEK_LOCK(Include.DBChainWeeklyLockTable,  aVariables.GetChainWeeklyForecastDatabaseVariableList(), aWriter, aForecastFileGroup);
//                aDatabase.Generate_STORE_FORECAST_WEEK(Include.DBStoreWeeklyForecastTable, aVariables.GetStoreWeeklyForecastDatabaseVariableList(), aWriter, aTableCount, aForecastFileGroup);
//                aDatabase.Generate_STORE_HISTORY_DAY(Include.DBStoreDailyHistoryTable, aVariables.GetStoreDailyHistoryDatabaseVariableList(), aWriter, aTableCount, aDailyHistoryFileGroup, aNoDailyHistoryFileGroup);
//                aDatabase.Generate_STORE_HISTORY_WEEK(Include.DBStoreWeeklyHistoryTable, aVariables.GetStoreWeeklyHistoryDatabaseVariableList(), aWriter, aTableCount, aHistoryFileGroup, aNoHistoryFileGroup);
//                aDatabase.Generate_CHAIN_FORECAST_WEEK(Include.DBChainWeeklyForecastTable, aVariables.GetChainWeeklyForecastDatabaseVariableList(), aWriter, aForecastFileGroup);
//                aDatabase.Generate_CHAIN_HISTORY_WEEK(Include.DBChainWeeklyHistoryTable, aVariables.GetChainWeeklyHistoryDatabaseVariableList(), aWriter, aHistoryFileGroup);
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void Generate_Stored_Procedures(GenDatabase aDatabase, IPlanComputationVariables aVariables, StreamWriter aWriter, int aTableCount)
//        {
//            try
//            {
//                //aDatabase.Generate_Views(aWriter, aTableCount, aVariables);

//                Generate_Stored_Procedures(aDatabase, 
//                    aVariables.GetChainWeeklyHistoryDatabaseVariableList(), 
//                    aVariables.GetChainWeeklyForecastDatabaseVariableList(), 
//                    aVariables.GetStoreWeeklyHistoryDatabaseVariableList(), 
//                    aVariables.GetStoreDailyHistoryDatabaseVariableList(), 
//                    aVariables.GetStoreWeeklyForecastDatabaseVariableList(),
//                    aVariables.SalesTotalUnitsVariable,
//                    aVariables.SalesRegularUnitsVariable,
//                    aVariables.SalesPromoUnitsVariable,
//                    aWriter, 
//                    Include.DBStoreDailyHistoryTable, Include.DBStoreDailyHistoryView,
//                    Include.DBStoreWeeklyHistoryTable, Include.DBStoreWeeklyHistoryView,
//                    Include.DBStoreWeeklyForecastTable, Include.DBStoreWeeklyForecastView,
//                    Include.DBStoreWeeklyLockTable,
//                    Include.DBStoreWeeklyHistoryTable, 
//                    Include.DBStoreWeeklyForecastTable,
									
//                    Include.DBChainWeeklyHistoryTable, Include.DBChainWeeklyHistoryView,
//                    Include.DBChainWeeklyForecastTable, Include.DBChainWeeklyForecastView, 
//                    Include.DBChainWeeklyLockTable, 
//                    Include.DBChainWeeklyHistoryTable, 
//                    Include.DBChainWeeklyForecastTable,
				
//                    Include.DBStoreDailyHistoryWriteSP, 
//                    Include.DBStoreWeeklyHistoryWriteSP, 
//                    Include.DBStoreWeeklyForecastWriteSP, 
//                    Include.DBStoreDailyHistoryReadSP,  
//                    Include.DBStoreWeeklyHistoryReadSP, 
//                    Include.DBStoreWeeklyModVerReadSP,
//                    Include.DBStoreWeeklyForecastDelZeroSP,
//                    Include.DBStoreWeeklyForecastDelUnlockedSP,
				
//                    Include.DBChainWeeklyHistoryWriteSP, 
//                    Include.DBChainWeeklyForecastWriteSP,
//                    Include.DBChainWeeklyHistoryReadSP,
//                    Include.DBChainWeeklyModVerReadSP, 
//                    Include.DBChainWeeklyForecastDelZeroSP,
//                    Include.DBChainWeeklyForecastDelUnlockedSP,
//                    //Include.DBStoreHistorySizesReadSP, //TT#3445 -jsobek -Failure of Size Curve Generation
                    
//                    aTableCount, Include.DBStoreTableCountColumn,
//                    Include.DBTempTableName);

//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void Generate_Stored_Procedures(GenDatabase aDatabase, 
//            ProfileList aChainHistoryVariables, ProfileList aChainForecastVariables, 
//            ProfileList aStoreWeeklyHistoryVariables, ProfileList aStoreDailyHistoryVariables, 
//            ProfileList aStoreForecastVariables, 
//            VariableProfile aSalesTotalUnitsVariable,
//            VariableProfile aSalesRegularUnitsVariable,
//            VariableProfile aSalesPromoUnitsVariable,
//            StreamWriter aWriter, 
//            string aStoreDailyHistoryTable, string aStoreDailyHistoryView,
//            string aStoreWeeklyHistoryTable, string aStoreWeeklyHistoryView,
//            string aStoreWeeklyForecastTable, string aStoreWeeklyForecastView, 
//            string aStoreWeeklyLockTable,
//            string aStoreWeeklyModVerHistoryTable, 
//            string aStoreWeeklyModVerForecastTable,
//            string aChainWeeklyHistoryTable, string aChainWeeklyHistoryView, 
//            string aChainWeeklyForecastTable, string aChainWeeklyForecastView,
//            string aChainWeeklyLockTable, 
//            string aChainWeeklyModVerHistoryTable, 
//            string aChainWeeklyModVerForecastTable,

//            string aStoreDailyHistoryWriteSP, 
//            string aStoreWeeklyHistoryWriteSP, 
//            string aStoreWeeklyForecastWriteSP, 
//            string aStoreDailyHistoryReadSP, 
//            string aStoreWeeklyHistoryReadSP,  
//            string aStoreWeeklyModVerReadSP,
//            string aStoreWeeklyForecastDelZeroSP,
//            string aStoreWeeklyForecastDelUnlockedSP,

//            string aChainWeeklyHistoryWriteSP, 
//            string aChainWeeklyForecastWriteSP, 
//            string aChainWeeklyHistoryReadSP, 
//            string aChainWeeklyModVerReadSP, 
//            string aChainWeeklyForecastDelZeroSP,
//            string aChainWeeklyForecastDelUnlockedSP,
//            //string aStoreHistorySizesReadSP, //TT#3445 -jsobek -Failure of Size Curve Generation
//            int aTableCount, string aTableCountColumn,
//            string aTempTableName)
//        {
//            try
//            {
//                //bool stop = true;
//                ProfileList chainWeeklyHistoryRollupVariables = new ProfileList(eProfileType.Variable);
//                ProfileList chainWeeklyForecastRollupVariables = new ProfileList(eProfileType.Variable);
//                ProfileList storeDailyHistoryRollupVariables = new ProfileList(eProfileType.Variable);
//                ProfileList storeWeeklyHistoryRollupVariables = new ProfileList(eProfileType.Variable);
//                ProfileList storeWeeklyForecastRollupVariables = new ProfileList(eProfileType.Variable);
//                foreach (VariableProfile vp in aChainHistoryVariables)
//                {
//                    // Begin TT#3158 - JSmith - Planning History Rollup
//                    //if (vp.LevelRollType != eLevelRollType.None &&
//                    //                    vp.ChainHistoryModelType != eVariableDatabaseModelType.None)
//                    if (vp.LevelRollType != eLevelRollType.None &&
//                                        vp.ChainHistoryModelType != eVariableDatabaseModelType.None &&
//                        (vp.VariableCategory == eVariableCategory.Chain || vp.VariableCategory == eVariableCategory.Both))
//                    // End TT#3158 - JSmith - Planning History Rollup
//                    {
//                        chainWeeklyHistoryRollupVariables.Add(vp);
//                    }
//                }

//                foreach (VariableProfile vp in aChainForecastVariables)
//                {
//                    if (vp.LevelRollType != eLevelRollType.None &&
//                        vp.ChainForecastModelType != eVariableDatabaseModelType.None &&
//                        (vp.VariableCategory == eVariableCategory.Chain || vp.VariableCategory == eVariableCategory.Both))
//                    {
//                        chainWeeklyForecastRollupVariables.Add(vp);
//                    }
//                }
//                foreach (VariableProfile vp in aStoreWeeklyHistoryVariables)
//                {
//                    if (vp.LevelRollType != eLevelRollType.None &&
//                        vp.StoreWeeklyHistoryModelType != eVariableDatabaseModelType.None &&
//                        (vp.VariableCategory == eVariableCategory.Store || vp.VariableCategory == eVariableCategory.Both))
//                    {
//                        storeWeeklyHistoryRollupVariables.Add(vp);
//                    }
//                }
//                foreach (VariableProfile vp in aStoreDailyHistoryVariables)
//                {
//                    if (vp.LevelRollType != eLevelRollType.None &&
//                        vp.StoreDailyHistoryModelType != eVariableDatabaseModelType.None &&
//                        (vp.VariableCategory == eVariableCategory.Store || vp.VariableCategory == eVariableCategory.Both))
//                    {
//                        storeDailyHistoryRollupVariables.Add(vp);
//                    }
//                }
//                foreach (VariableProfile vp in aStoreForecastVariables)
//                {
//                    if (vp.LevelRollType != eLevelRollType.None &&
//                        vp.StoreForecastModelType != eVariableDatabaseModelType.None &&
//                        (vp.VariableCategory == eVariableCategory.Store || vp.VariableCategory == eVariableCategory.Both))
//                    {
//                        storeWeeklyForecastRollupVariables.Add(vp);
//                    }
//                }

//                for (int i = 0; i < aTableCount; i++)
//                {
//                    aDatabase.Generate_SP_MID_DROP(aStoreDailyHistoryWriteSP, aWriter, i);
//                    aDatabase.Generate_SP_MID_DROP(aStoreWeeklyHistoryWriteSP, aWriter, i);
//                    aDatabase.Generate_SP_MID_DROP(aStoreWeeklyForecastWriteSP, aWriter, i);
//                    aDatabase.Generate_SP_MID_DROP(aStoreDailyHistoryReadSP, aWriter, i);
//                    aDatabase.Generate_SP_MID_DROP(aStoreWeeklyHistoryReadSP, aWriter, i);
//                    aDatabase.Generate_SP_MID_DROP(aStoreWeeklyModVerReadSP, aWriter, i);
//                    aDatabase.Generate_SP_MID_DROP(Include.DBStoreWeeklyForecastReadSP, aWriter, i);
//                }

//                // Begin TT#3373 - JSmith - Save Store Forecast receive DBNull error
//                aDatabase.Generate_SP_MID_DROP(Include.DBStoreWeeklyForecastLockWriteSP, aWriter, 0);
//                //aDatabase.Generate_SP_MID_DROP(Include.DBGetTableFromType, aWriter, 0);
//                // End TT#3373 - JSmith - Save Store Forecast receive DBNull error

//                //aDatabase.Generate_SP_MID_XML_ST_HIS_DAY_TYPE(aStoreDailyHistoryVariables, aWriter);
//                //aDatabase.Generate_SP_MID_XML_ST_HIS_WK_TYPE(aStoreWeeklyHistoryVariables, aWriter);
//                //aDatabase.Generate_SP_MID_XML_ST_FOR_WK_TYPE(aStoreForecastVariables, aWriter);
//                //aDatabase.Generate_SP_MID_XML_ST_Read_TYPES(aWriter);
                
//                for (int i=0; i<aTableCount; i++)
//                {
//                    if (aStoreDailyHistoryVariables.Count > 0)
//                    {
//                        //aDatabase.Generate_SP_MID_XML_ST_HIS_DAY_WRITE(aStoreDailyHistoryWriteSP, aStoreDailyHistoryTable, aTempTableName, aStoreDailyHistoryVariables, aWriter, i, aTableCountColumn);
//                        //aDatabase.Generate_SP_MID_XML_ST_HIS_DAY_READ(aStoreDailyHistoryReadSP, aStoreDailyHistoryTable, aStoreDailyHistoryView, aTempTableName, aStoreDailyHistoryVariables, aWriter, i, aTableCountColumn);
//                        aDatabase.Generate_SP_MID_ST_HIS_DAY_ROLLUP(storeDailyHistoryRollupVariables, storeWeeklyHistoryRollupVariables, aWriter, i);
//                    }
//                    if (aStoreWeeklyHistoryVariables.Count > 0) 
//                    {
//                        //aDatabase.Generate_SP_MID_XML_ST_HIS_WK_WRITE(aStoreWeeklyHistoryWriteSP, aStoreWeeklyHistoryTable, aTempTableName, aStoreWeeklyHistoryVariables, aWriter, i, aTableCountColumn);
//                        //aDatabase.Generate_SP_MID_XML_ST_HIS_WK_READ(aStoreWeeklyHistoryReadSP, aStoreWeeklyHistoryTable, aStoreWeeklyHistoryView, aTempTableName, aStoreWeeklyHistoryVariables, aWriter, i, aTableCountColumn);
//                        aDatabase.Generate_SP_MID_ST_HIS_WK_ROLLUP(storeWeeklyHistoryRollupVariables, aWriter, i);
//                    }
//                    if (aStoreForecastVariables.Count > 0)
//                    {
//                        //aDatabase.Generate_SP_MID_XML_ST_FOR_WK_DEL_ZERO(aStoreWeeklyForecastDelZeroSP, aStoreWeeklyForecastTable, aStoreWeeklyLockTable, aStoreForecastVariables, aWriter, i);
//                        //aDatabase.Generate_SP_MID_XML_ST_FOR_WK_WRITE(aStoreWeeklyForecastWriteSP, aStoreWeeklyForecastTable, aStoreWeeklyLockTable, aTempTableName, aStoreForecastVariables, aWriter, i, aTableCountColumn);
//                        //aDatabase.Generate_SP_MID_XML_ST_FOR_WK_READ(Include.DBStoreWeeklyForecastReadSP, aStoreWeeklyForecastTable, aStoreWeeklyForecastView, aTempTableName, aStoreForecastVariables, aWriter, i, aTableCountColumn);
//                        //aDatabase.Generate_SP_MID_XML_ST_MOD_WK_READ(aStoreWeeklyModVerReadSP, aStoreWeeklyForecastTable, aStoreWeeklyHistoryTable, aStoreWeeklyLockTable, aStoreWeeklyForecastView, aStoreWeeklyHistoryView, aTempTableName, aStoreForecastVariables, aWriter, i, aTableCountColumn);
//                        aDatabase.Generate_SP_MID_ST_FOR_WK_ROLLUP(storeWeeklyForecastRollupVariables, aWriter, i);
//                    }

//                    //aDatabase.Generate_UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE_DAY(Include.DBStoreHistoryGetSizeDataFromNodeDay, aWriter, i); //TT#739-MD -jsobek -Delete Stores //TT#3456 -jsobek -Size Day To Week Failure
//                }



//                //aDatabase.Generate_UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE(aWriter, aTableCount -1); //TT#739-MD -jsobek -Delete Stores //TT#3456 -jsobek -Size Day To Week Failure
//                //aDatabase.Generate_UDF_STORE_GET_ACTIVE_RIDS(aWriter); // TT#3507 - JSmith - Database Error during Upgrade
//                //aDatabase.Generate_VW_HIERARCHY_ACTIVE_ORGANIZATIONAL_NODES(aWriter, aTableCount - 1); //TT#3496 - JSmith - Database Upgrade failed for Version 5.21 post conversion
//                //aDatabase.Generate_UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE(aWriter, aTableCount - 1); //TT#3496 - JSmith - Database Upgrade failed for Version 5.21 post conversion
//                //aDatabase.Generate_UDF_STORE_HISTORY_DAY_GET_DATA_IN_TIME_PERIOD_FROM_NODE(aWriter, aTableCount - 1); //TT#3486 - JSmith - Create new database failed
//                //aDatabase.Generate_MID_SIZE_CURVE_INSERT_SUMMARY_DATA_IN_TIME_PERIOD_FROM_NODE(aWriter, aTableCount -1); //TT#739-MD -jsobek -Delete Stores

//                if (aStoreForecastVariables.Count > 0)
//                {
//                    //aDatabase.Generate_SP_MID_XML_ST_FOR_WK_DEL_UNLOCK(aStoreWeeklyForecastDelUnlockedSP, aStoreWeeklyLockTable, aStoreForecastVariables, aWriter, aTableCount);
//                    //aDatabase.Generate_SP_MID_XML_ST_FOR_WK_LOCK_READ(Include.DBStoreWeeklyForecastLockReadSP, aStoreWeeklyLockTable, aStoreWeeklyForecastView, aTableCountColumn, aTempTableName, aStoreForecastVariables, aWriter);
//                    // Begin TT#3373 - JSmith - Save Store Forecast receive DBNull error
//                    //aDatabase.Generate_SP_MID_XML_ST_FOR_WK_LOCK_WRITE(aStoreWeeklyForecastWriteSP, aStoreWeeklyForecastTable, aStoreWeeklyLockTable, aTempTableName, aStoreForecastVariables, aWriter, aTableCountColumn);
//                    // End TT#3373 - JSmith - Save Store Forecast receive DBNull error
//                }

//                if (aChainHistoryVariables.Count > 0)
//                {
//                    //aDatabase.Generate_SP_MID_XML_CHN_HIS_WK_WRITE(aChainWeeklyHistoryWriteSP, aChainWeeklyHistoryTable, aTempTableName, aTableCountColumn, aChainHistoryVariables, aWriter);
//                    //aDatabase.Generate_SP_MID_XML_CHN_HIS_WK_READ(aChainWeeklyHistoryReadSP, aChainWeeklyHistoryTable, aChainWeeklyHistoryView, aTableCountColumn, aTempTableName, aChainHistoryVariables, aWriter);
//                    aDatabase.Generate_SP_MID_CHN_HIS_WK_ROLLUP(chainWeeklyHistoryRollupVariables, aWriter);
//                }
//                if (aChainForecastVariables.Count > 0)
//                {
//                    //aDatabase.Generate_SP_MID_XML_CHN_FOR_WK_WRITE(aChainWeeklyForecastWriteSP, aChainWeeklyForecastTable, aChainWeeklyLockTable, aTempTableName, aTableCountColumn, aChainForecastVariables, aWriter);
//                    //aDatabase.Generate_SP_MID_XML_CHN_FOR_WK_READ(Include.DBChainWeeklyForecastReadSP, aChainWeeklyForecastTable, aChainWeeklyForecastView, aTableCountColumn, aTempTableName, aChainForecastVariables, aWriter);
//                    //aDatabase.Generate_SP_MID_XML_CHN_FOR_WK_LOCK_READ(Include.DBChainWeeklyForecastLockReadSP, aChainWeeklyLockTable, aChainWeeklyForecastView, aTableCountColumn, aTempTableName, aChainForecastVariables, aWriter);
//                    //aDatabase.Generate_SP_MID_XML_CHN_MOD_WK_READ(aChainWeeklyModVerReadSP, aChainWeeklyForecastTable, aChainWeeklyHistoryTable, aChainWeeklyLockTable, aChainWeeklyForecastView, aChainWeeklyHistoryView, aTableCountColumn, aTempTableName, aChainForecastVariables, aWriter);
//                    //aDatabase.Generate_SP_MID_XML_CHN_FOR_WK_DEL_ZERO(aChainWeeklyForecastDelZeroSP, aChainWeeklyForecastTable, aChainWeeklyLockTable, aChainForecastVariables, aWriter);
//                    //aDatabase.Generate_SP_MID_XML_CHN_FOR_WK_DEL_UNLOCK(aChainWeeklyForecastDelUnlockedSP, aChainWeeklyLockTable, aChainForecastVariables, aWriter);
//                    aDatabase.Generate_SP_MID_CHN_FOR_WK_ROLLUP(chainWeeklyForecastRollupVariables, aWriter);
//                }

//                if (storeWeeklyHistoryRollupVariables.Count > 0 &&
//                    chainWeeklyHistoryRollupVariables.Count > 0)
//                {
//                    aDatabase.Generate_SP_MID_HIS_ST_OTHER_ROLLUPS(storeWeeklyHistoryRollupVariables, chainWeeklyHistoryRollupVariables, aWriter);
//                }

//                if (storeWeeklyForecastRollupVariables.Count > 0 &&
//                    chainWeeklyForecastRollupVariables.Count > 0)
//                {
//                    aDatabase.Generate_SP_MID_FOR_ST_TO_CHN_ROLLUP(storeWeeklyForecastRollupVariables, chainWeeklyForecastRollupVariables, aWriter);

//                }

//                //Begin TT#3445 -jsobek -Failure of Size Curve Generation
//                //if (aSalesTotalUnitsVariable != null && aSalesRegularUnitsVariable != null && aSalesPromoUnitsVariable != null)
//                //{
//                //    aDatabase.Generate_SP_MID_ST_HIS_SIZES_READ(aSalesTotalUnitsVariable, aSalesRegularUnitsVariable, aSalesPromoUnitsVariable, aWriter);
//                //}
//                //End TT#3445 -jsobek -Failure of Size Curve Generation

//                // Begin TT#3373 - JSmith - Save Store Forecast receive DBNull error
//                aDatabase.Generate_SP_MID_GET_TABLE_FROM_TYPE(aWriter);
//                // End TT#3373 - JSmith - Save Store Forecast receive DBNull error
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        abstract public class GenDatabase
//        {
//            MIDRetail.Business.Rollup _rollup;

//            public GenDatabase()
//            {
//                _rollup = new Rollup(null, 0, 0, false, false, false);
//            }

//            protected Rollup Rollup
//            {
//                get { return _rollup;}
//            }

//            abstract public void Generate_STORE_FORECAST_WEEK_LOCK(string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, string aForecastFileGroup);
//            abstract public void Generate_CHAIN_FORECAST_WEEK_LOCK(string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, string aForecastFileGroup);
//            abstract public void Generate_STORE_FORECAST_WEEK(string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableCount, string aForecastFileGroup);
//            abstract public void Generate_STORE_HISTORY_DAY(string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableCount, string aDailyHistoryFileGroup, int aNoDailyHistoryFileGroup);
//            abstract public void Generate_STORE_HISTORY_WEEK(string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableCount, string aHistoryFileGroup, int aNoHistoryFileGroup);
//            abstract public void Generate_CHAIN_FORECAST_WEEK(string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, string aForecastFileGroup);
//            abstract public void Generate_CHAIN_HISTORY_WEEK(string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, string aHistoryFileGroup);
 
//            //abstract public void Generate_Views(StreamWriter aWriter, int aTableCount, IPlanComputationVariables aVariables);

//            abstract public void Generate_SP_MID_DROP(string aProcedureName, StreamWriter aWriter, int aTableNumber);
//            //abstract public void Generate_SP_MID_XML_ST_HIS_DAY_TYPE(ProfileList aDatabaseVariables, StreamWriter aWriter);
//            //abstract public void Generate_SP_MID_XML_ST_HIS_WK_TYPE(ProfileList aDatabaseVariables, StreamWriter aWriter);
//            //abstract public void Generate_SP_MID_XML_ST_FOR_WK_TYPE(ProfileList aDatabaseVariables, StreamWriter aWriter);
//            //abstract public void Generate_SP_MID_XML_ST_Read_TYPES(StreamWriter aWriter);
//            abstract public void Generate_SP_MID_GET_TABLE_FROM_TYPE(StreamWriter aWriter);  // TT#3373 - JSmith - Save Store Forecast receive DBNull error
            
//            //abstract public void Generate_SP_MID_XML_ST_HIS_DAY_WRITE(string aProcedureName, string aTableName, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableNumber, string aTableCountColumn);
			
//            //abstract public void Generate_SP_MID_XML_ST_HIS_WK_WRITE(string aProcedureName, string aTableName, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableNumber, string aTableCountColumn);
//            //abstract public void Generate_SP_MID_XML_ST_FOR_WK_WRITE(string aProcedureName, string aTableName, string aLockTableName, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableNumber, string aTableCountColumn);
//            //abstract public void Generate_SP_MID_XML_ST_FOR_WK_LOCK_WRITE(string aProcedureName, string aTableName, string aLockTableName, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, string aTableCountColumn);// TT#3373 - JSmith - Save Store Forecast receive DBNull error
			
//            //abstract public void Generate_SP_MID_XML_ST_HIS_DAY_READ(string aProcedureName, string aTableName, string aViewName, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableNumber, string aTableCountColumn);
//            //abstract public void Generate_SP_MID_XML_ST_HIS_WK_READ(string aProcedureName, string aTableName, string aViewName, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableNumber, string aTableCountColumn);
//            //abstract public void Generate_SP_MID_XML_ST_FOR_WK_READ(string aProcedureName, string aTableName, string aViewName, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableNumber, string aTableCountColumn);
//            //abstract public void Generate_SP_MID_XML_ST_FOR_WK_LOCK_READ(string aProcedureName, string aTableName, string aViewName, string aTableCountColumn, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter);
//            //abstract public void Generate_SP_MID_XML_ST_MOD_WK_READ(string aProcedureName, string aForecastTableName, string aHistoryTableName, string aLockTableName, string aForecastViewName, string aHistoryViewName, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableNumber, string aTableCountColumn);

//            abstract public void Generate_SP_MID_XML_ST_HIS_DAY_ROLL_READ(string aProcedureName, string aTableName, string aViewName, int aTableNumber, string aTableCountColumn, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter);
//            abstract public void Generate_SP_MID_XML_ST_HIS_WK_ROLL_READ(string aProcedureName, string aTableName, string aViewName, int aTableNumber, string aTableCountColumn, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter);
//            abstract public void Generate_SP_MID_XML_ST_MOD_WK_ROLL_READ(string aProcedureName, string aForecastTableName, string aHistoryTableName, string aLockTableName, string aForecastViewName, string aHistoryViewName, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableNumber, string aTableCountColumn);
			
//            //abstract public void Generate_SP_MID_XML_CHN_HIS_WK_WRITE(string aProcedureName, string aTableName, string aTempTableName, string aTableCountColumn, ProfileList aDatabaseVariables, StreamWriter aWriter);
//            //abstract public void Generate_SP_MID_XML_CHN_FOR_WK_WRITE(string aProcedureName, string aTableName, string aLockTableName, string aTempTableName, string aTableCountColumn, ProfileList aDatabaseVariables, StreamWriter aWriter);
		
//            //abstract public void Generate_SP_MID_XML_CHN_HIS_WK_READ(string aProcedureName, string aTableName, string aViewName, string aTableCountColumn, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter);
//            //abstract public void Generate_SP_MID_XML_CHN_FOR_WK_READ(string aProcedureName, string aTableName, string aViewName, string aTableCountColumn, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter);
//            //abstract public void Generate_SP_MID_XML_CHN_FOR_WK_LOCK_READ(string aProcedureName, string aTableName, string aViewName, string aTableCountColumn, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter);
//            //abstract public void Generate_SP_MID_XML_CHN_MOD_WK_READ(string aProcedureName, string aForecastTableName, string aHistoryTableName, string aLockTableName, string aForecastViewName, string aHistoryViewName, string aTableCountColumn, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter);

//            abstract public void Generate_SP_MID_XML_CHN_HIS_WK_ROLL_READ(string aProcedureName, string aTableName, string aViewName, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter);
//            abstract public void Generate_SP_MID_XML_CHN_MOD_WK_ROLL_READ(string aProcedureName, string aForecastTableName, string aHistoryTableName, string aLockTableName, string aForecastViewName, string aHistoryViewName, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter);
//            //abstract public void Generate_SP_MID_XML_CHN_FOR_WK_DEL_ZERO(string aProcedureName, string aTableName, string aLockTableName, ProfileList aDatabaseVariables, StreamWriter aWriter);
//            //abstract public void Generate_SP_MID_XML_CHN_FOR_WK_DEL_UNLOCK(string aProcedureName, string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter);
//            //abstract public void Generate_SP_MID_XML_ST_FOR_WK_DEL_ZERO(string aProcedureName, string aTableName, string aLockTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableNumber);
//            //abstract public void Generate_SP_MID_XML_ST_FOR_WK_DEL_UNLOCK(string aProcedureName, string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableCount);

//            abstract public void Generate_SP_MID_ST_HIS_DAY_ROLLUP(ProfileList aDailyDatabaseVariables, ProfileList aWeeklyDatabaseVariables, StreamWriter aWriter, int aTableNumber);
//            abstract public void Generate_SP_MID_ST_HIS_WK_ROLLUP(ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableNumber);
//            abstract public void Generate_SP_MID_ST_FOR_WK_ROLLUP(ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableNumber);
//            abstract public void Generate_SP_MID_HIS_ST_OTHER_ROLLUPS(ProfileList aStoreDatabaseVariables, ProfileList aChainDatabaseVariables, StreamWriter aWriter);
//            abstract public void Generate_SP_MID_FOR_ST_TO_CHN_ROLLUP(ProfileList aStoreDatabaseVariables, ProfileList aChainDatabaseVariables, StreamWriter aWriter);
//            abstract public void Generate_SP_MID_CHN_HIS_WK_ROLLUP(ProfileList aDatabaseVariables, StreamWriter aWriter);
//            abstract public void Generate_SP_MID_CHN_FOR_WK_ROLLUP(ProfileList aDatabaseVariables, StreamWriter aWriter);
//            //abstract public void Generate_SP_MID_ST_HIS_SIZES_READ(VariableProfile aSalesTotalUnitsVariable, VariableProfile aSalesRegularUnitsVariable, VariableProfile aSalesPromoUnitsVariable, StreamWriter aWriter); //TT#3445 -jsobek -Failure of Size Curve Generation
//            //abstract public void Generate_UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE_DAY(string aFunctionName, StreamWriter aWriter, int aTableNumber); //TT#739-MD -jsobek -Delete Stores //TT#3456 -jsobek -Size Day To Week Failure
//            //abstract public void Generate_UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE(StreamWriter aWriter, int maxTableNumber); //TT#739-MD -jsobek -Delete Stores //TT#3456 -jsobek -Size Day To Week Failure
//            //abstract public void Generate_VW_HIERARCHY_ACTIVE_ORGANIZATIONAL_NODES(StreamWriter aWriter, int maxTableNumber); //TT#3496 - JSmith - Database Upgrade failed for Version 5.21 post conversion
//            //abstract public void Generate_UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE(StreamWriter aWriter, int maxTableNumber); //TT#3496 - JSmith - Database Upgrade failed for Version 5.21 post conversion
//            //abstract public void Generate_UDF_STORE_HISTORY_DAY_GET_DATA_IN_TIME_PERIOD_FROM_NODE(StreamWriter aWriter, int maxTableNumber); //TT#3486 - JSmith - Create new database failed
//            //abstract public void Generate_UDF_STORE_GET_ACTIVE_RIDS(StreamWriter aWriter); //TT#3507 - JSmith - Database Error during Upgrade
//            //abstract public void Generate_MID_SIZE_CURVE_INSERT_SUMMARY_DATA_IN_TIME_PERIOD_FROM_NODE(StreamWriter aWriter, int maxTableNumber); //TT#739-MD -jsobek -Delete Stores
//        }


//        abstract public class GenSQLServer : GenDatabase
//        {
//            protected string _indent5 = "     ";
//            protected string _indent10 = "          ";
//            protected string _indent15 = "               ";
//            protected string _blankLine = new string(' ', 100);

//            public GenSQLServer()
//            {
//            }

//            override public void Generate_STORE_FORECAST_WEEK_LOCK(string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, string aForecastFileGroup)
//            {
//                try
//                {
//                    string tableName = aTableName;

//                    if (aDatabaseVariables == null ||
//                        aDatabaseVariables.Count == 0)
//                    {
//                        return;
//                    }

//                    Generate_Store_Forecast_Table(tableName, aDatabaseVariables, aWriter, true, true, aForecastFileGroup);
//                    Generate_Store_Forecast_Key(tableName, false, aWriter);
//                    Generate_Node_Constraint(tableName, "HIER_NODE_STR_FORE_WK_LOCK_FK1", aWriter);
//                    Generate_Store_Constraint(tableName, "STRS_STR_FORE_WK_LOCK_FK1", aWriter);
//                    Generate_Version_Constraint(tableName, "FORE_VER_STR_FORE_WK_LOCK_FK1", aWriter);
//                    Generate_Store_Index(tableName, aWriter);	// TT#739-MD - STodd - delete stores
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            override public void Generate_CHAIN_FORECAST_WEEK_LOCK(string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, string aForecastFileGroup)
//            {
//                try
//                {

//                    if (aDatabaseVariables == null ||
//                        aDatabaseVariables.Count == 0)
//                    {
//                        return;
//                    }

//                    string tableName = aTableName;
//                    Generate_Chain_Forecast_Table(tableName, aDatabaseVariables, aWriter, true, true, aForecastFileGroup);
//                    Generate_Chain_Forecast_Key(tableName, aWriter);
//                    Generate_Node_Constraint(tableName, "HIER_NODE_CHN_FORE_WK_LCK_FK1", aWriter);
//                    Generate_Version_Constraint(tableName, "FORE_VER_CHAIN_FORE_WK_LCK_FK1", aWriter);

//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            override public void Generate_STORE_FORECAST_WEEK(string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableCount, string aForecastFileGroup)
//            {
//                try
//                {
//                    if (aDatabaseVariables == null ||
//                        aDatabaseVariables.Count == 0)
//                    {
//                        return;
//                    }

//                    for (int i=0; i<aTableCount; i++)
//                    {
//                        string tableNo = i.ToString(CultureInfo.CurrentCulture);
//                        string tableName = aTableName.Replace(Include.DBTableCountReplaceString, tableNo);
//                        Generate_Store_Forecast_Table(tableName, aDatabaseVariables, aWriter, false, true, aForecastFileGroup);
//                        Generate_Store_Forecast_Key(tableName, true, aWriter);
//                        Generate_Check_Constraint(tableName, tableName + "_HN_MOD", aWriter,  tableNo);
//                        Generate_Node_Constraint(tableName, "HIER_NODE_STR_FOR_WK" + tableNo + "_FK1", aWriter);
//                        Generate_Store_Constraint(tableName, "STRS_STR_FOR_WK" + tableNo + "_FK1", aWriter);
//                        Generate_Version_Constraint(tableName, "FOR_VER_STR_FOR_WK" + tableNo + "_FK1", aWriter);
//                        Generate_Store_Index(tableName, aWriter);	// TT#739-MD - STodd - delete stores
//                    }
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            override public void Generate_STORE_HISTORY_DAY(string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableCount, string aDailyHistoryFileGroup, int aNoDailyHistoryFileGroup)
//            {
//                try
//                {
//                    string fileGroup;

//                    if (aDatabaseVariables == null ||
//                        aDatabaseVariables.Count == 0)
//                    {
//                        return;
//                    }

//                    for (int i=0; i<aTableCount; i++)
//                    {
//                        string tableNo = i.ToString(CultureInfo.CurrentCulture);
//                        string tableName = "STORE_HISTORY_DAY" + tableNo;
//                        int remainder = i % aNoDailyHistoryFileGroup;
//                        if (remainder == 0)
//                        {
//                            fileGroup = aDailyHistoryFileGroup;
//                        }
//                        else
//                        {
//                            fileGroup = aDailyHistoryFileGroup + (remainder + 1).ToString();
//                        }
//                        Generate_Store_History_Table(tableName, aDatabaseVariables, aWriter, true, fileGroup);
//                        Generate_Store_History_Key(tableName, aWriter);
//                        Generate_Check_Constraint(tableName, tableName + "_HN_MOD", aWriter, tableNo);
//                        Generate_Node_Constraint(tableName, "HIER_NODE_STR_HIS_DAY" + tableNo + "_FK1", aWriter);
//                        Generate_Store_Constraint(tableName, "STRS_STR_HIS_DAY" + tableNo + "_FK1", aWriter);
//                        Generate_Store_Index(tableName, aWriter);	// TT#739-MD - STodd - delete stores
//                    }
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            override public void Generate_STORE_HISTORY_WEEK(string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableCount, string aHistoryFileGroup, int aNoHistoryFileGroup)
//            {
//                try
//                {
//                    string fileGroup;

//                    if (aDatabaseVariables == null ||
//                        aDatabaseVariables.Count == 0)
//                    {
//                        return;
//                    }

//                    for (int i=0; i<aTableCount; i++)
//                    {
//                        string tableNo = i.ToString(CultureInfo.CurrentCulture);
//                        string tableName = aTableName.Replace(Include.DBTableCountReplaceString, i.ToString());
//                        int remainder = i % aNoHistoryFileGroup;
//                        if (remainder == 0)
//                        {
//                            fileGroup = aHistoryFileGroup;
//                        }
//                        else
//                        {
//                            fileGroup = aHistoryFileGroup + (remainder + 1).ToString();
//                        }
//                        Generate_Store_History_Table(tableName, aDatabaseVariables, aWriter, true, fileGroup);
//                        Generate_Store_History_Key(tableName, aWriter);
//                        Generate_Check_Constraint(tableName, tableName + "_HN_MOD", aWriter, tableNo);
//                        Generate_Node_Constraint(tableName, "HIER_NODE_STR_HIS_WK" + tableNo + "_FK1", aWriter);
//                        Generate_Store_Constraint(tableName, "STRS_STR_HIS_WK" + tableNo + "_FK1", aWriter);
//                        Generate_Store_Index(tableName, aWriter);	// TT#739-MD - STodd - delete stores
//                    }
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            override public void Generate_CHAIN_FORECAST_WEEK(string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, string aForecastFileGroup)
//            {
//                try
//                {
//                    if (aDatabaseVariables == null ||
//                        aDatabaseVariables.Count == 0)
//                    {
//                        return;
//                    }

//                    string tableName = aTableName;
//                    Generate_Chain_Forecast_Table(tableName, aDatabaseVariables, aWriter, false, true, aForecastFileGroup);
//                    Generate_Chain_Forecast_Key(tableName, aWriter);
//                    Generate_Node_Constraint(tableName, "HIER_NODE_CHN_FOR_WK_FK1", aWriter);
//                    Generate_Version_Constraint(tableName, "FOR_VER_CHN_FOR_WK_FK1", aWriter);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            override public void Generate_CHAIN_HISTORY_WEEK(string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, string aHistoryFileGroup)
//            {
//                try
//                {
//                    if (aDatabaseVariables == null ||
//                        aDatabaseVariables.Count == 0)
//                    {
//                        return;
//                    }

//                    string tableName = aTableName;
//                    Generate_Chain_History_Table(tableName, aDatabaseVariables, aWriter, true, aHistoryFileGroup);
//                    Generate_Chain_History_Key(tableName, aWriter);
//                    Generate_Node_Constraint(tableName, "HIER_NODE_CHN_HIS_WK_FK1", aWriter);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            private void Generate_Chain_Forecast_Table(string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, bool aIsLockTable, bool aAllowNull, string aFileGroup)
//            {
//                try
//                {
//                    if (aDatabaseVariables == null ||
//                        aDatabaseVariables.Count == 0)
//                    {
//                        return;
//                    }

//                    aWriter.WriteLine("create table " + aTableName + " ( ");
//                    aWriter.WriteLine(_indent5 + "HN_RID int not null,");
//                    aWriter.WriteLine(_indent5 + "FV_RID int not null,");
//                    aWriter.WriteLine(_indent5 + "TIME_ID int not null,");
//                    int count = 0;
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        string line = AddVariable(vp, aIsLockTable, aAllowNull, eVariableCategory.Chain);
//                        if (count < aDatabaseVariables.Count)
//                        {
//                            line += ",";
//                        }
//                        else
//                        {
//                            line += ") on '" + aFileGroup + "'";
//                        }
//                        aWriter.WriteLine(_indent5 + line);
//                    }
					
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            private void Generate_Chain_History_Table(string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, bool aAllowNull, string aFileGroup)
//            {
//                try
//                {
//                    if (aDatabaseVariables == null ||
//                        aDatabaseVariables.Count == 0)
//                    {
//                        return;
//                    }

//                    aWriter.WriteLine("create table " + aTableName + " ( ");
//                    aWriter.WriteLine(_indent5 + "HN_RID int not null,");
//                    aWriter.WriteLine(_indent5 + "TIME_ID int not null,");
//                    int count = 0;
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        string line = AddVariable(vp, false, aAllowNull, eVariableCategory.Chain);
//                        if (count < aDatabaseVariables.Count)
//                        {
//                            line += ",";
//                        }
//                        else
//                        {
//                            line += ") on '" + aFileGroup + "'";
//                        }
//                        aWriter.WriteLine(_indent5 + line);
//                    }
					
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            private void Generate_Store_Forecast_Table(string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, bool aIsLockTable, bool aAllowNull, string aFileGroup)
//            {
//                try
//                {
//                    if (aDatabaseVariables == null ||
//                        aDatabaseVariables.Count == 0)
//                    {
//                        return;
//                    }

//                    aWriter.WriteLine("create table " + aTableName + " ( ");
//                    if (!aIsLockTable)
//                    {
//                        aWriter.WriteLine(_indent5 + "HN_MOD smallint not null,");
//                    }
//                    aWriter.WriteLine(_indent5 + "HN_RID int not null,");
//                    aWriter.WriteLine(_indent5 + "FV_RID int not null,");
//                    aWriter.WriteLine(_indent5 + "TIME_ID int not null,");
//                    aWriter.WriteLine(_indent5 + "ST_RID int not null,");
//                    int count = 0;
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        string line = AddVariable(vp, aIsLockTable, aAllowNull, eVariableCategory.Store);
//                        if (count < aDatabaseVariables.Count)
//                        {
//                            line += ",";
//                        }
//                        else
//                        {
//                            line += ") on '" + aFileGroup + "'";
//                        }
//                        aWriter.WriteLine(_indent5 + line);
//                    }
					
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            private void Generate_Store_History_Table(string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, bool aAllowNull, string aFileGroup)
//            {
//                try
//                {
//                    if (aDatabaseVariables == null ||
//                        aDatabaseVariables.Count == 0)
//                    {
//                        return;
//                    }

//                    aWriter.WriteLine("create table " + aTableName + " ( ");
//                    aWriter.WriteLine(_indent5 + "HN_MOD smallint not null,");
//                    aWriter.WriteLine(_indent5 + "HN_RID int not null,");
//                    aWriter.WriteLine(_indent5 + "TIME_ID int not null,");
//                    aWriter.WriteLine(_indent5 + "ST_RID int not null,");
//                    int count = 0;
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        string line = AddVariable(vp, false, aAllowNull, eVariableCategory.Store);
//                        if (count < aDatabaseVariables.Count)
//                        {
//                            line += ",";
//                        }
//                        else
//                        {
//                            line += ") on '" + aFileGroup + "'";
//                        }
//                        aWriter.WriteLine(_indent5 + line);
//                    }
					
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            private string AddVariable(VariableProfile aVariableProfile, bool aIsLock, bool aAllowNull, eVariableCategory aVariableCategory)
//            {
//                try
//                {
//                    eVariableDatabaseType databaseVariableType;
//                    switch (aVariableCategory)
//                    {
//                        case eVariableCategory.Chain:
//                            databaseVariableType = aVariableProfile.ChainDatabaseVariableType;
//                            break;
//                        case eVariableCategory.Store:
//                            databaseVariableType = aVariableProfile.StoreDatabaseVariableType;
//                            break;
//                        default:
//                            databaseVariableType = eVariableDatabaseType.None;
//                            break;
//                    }
//                    string command = null;
//                    if (aIsLock)
//                    {
//                        if (aAllowNull)
//                        {
//                            command = aVariableProfile.DatabaseColumnName + Include.cLockExtension + " char(1) null default 0";
//                        }
//                        else
//                        {
//                            command = aVariableProfile.DatabaseColumnName + Include.cLockExtension + " char(1) not null default 0";
//                        }
//                    }
//                    else
//                    {
//                        switch (databaseVariableType)
//                        {
//                            case eVariableDatabaseType.Integer:
//                                if (aAllowNull)
//                                {
//                                    command = aVariableProfile.DatabaseColumnName + " int null";
//                                }
//                                else
//                                {
//                                    command = aVariableProfile.DatabaseColumnName + " int not null";
//                                }
//                                break;
//                            case eVariableDatabaseType.Real:
//                                if (aAllowNull)
//                                {
//                                    command = aVariableProfile.DatabaseColumnName + " real null";
//                                }
//                                else
//                                {
//                                    command = aVariableProfile.DatabaseColumnName + " real not null";
//                                }
//                                break;
//                            case eVariableDatabaseType.DateTime:
//                                if (aAllowNull)
//                                {
//                                    command = aVariableProfile.DatabaseColumnName + " datetime null";
//                                }
//                                else
//                                {
//                                    command = aVariableProfile.DatabaseColumnName + " datetime not null";
//                                }
//                                break;
//                            case eVariableDatabaseType.String:
//                                if (aAllowNull)
//                                {
//                                    command = aVariableProfile.DatabaseColumnName + " varchar(100) null";
//                                }
//                                else
//                                {
//                                    command = aVariableProfile.DatabaseColumnName + " varchar(100) not null";
//                                }
//                                break;
//                            case eVariableDatabaseType.Char:
//                                if (aAllowNull)
//                                {
//                                    command = aVariableProfile.DatabaseColumnName + " char(1) null";
//                                }
//                                else
//                                {
//                                    command = aVariableProfile.DatabaseColumnName + " char(1) not null";
//                                }
//                                break;
//                            case eVariableDatabaseType.Float:
//                                if (aAllowNull)
//                                {
//                                    command = aVariableProfile.DatabaseColumnName + " float null";
//                                }
//                                else
//                                {
//                                    command = aVariableProfile.DatabaseColumnName + " float not null";
//                                }
//                                break;
//                            case eVariableDatabaseType.BigInteger:
//                                if (aAllowNull)
//                                {
//                                    command = aVariableProfile.DatabaseColumnName + " bigint null";
//                                }
//                                else
//                                {
//                                    command = aVariableProfile.DatabaseColumnName + " bigint not null";
//                                }
//                                break;
//                        }
//                    }
//                    return command;
//                }
//                catch
//                {
//                    throw;
//                }
			
//            }

//            private void Generate_Chain_History_Key(string aTableName, StreamWriter aWriter)
//            {
//                try
//                {
//                    aWriter.WriteLine("alter table " + aTableName);
//                    aWriter.WriteLine(_indent5 +  "add constraint " + aTableName + "_PK primary key clustered (HN_RID, TIME_ID)");
					
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            private void Generate_Chain_Forecast_Key(string aTableName, StreamWriter aWriter)
//            {
//                try
//                {
//                    aWriter.WriteLine("alter table " + aTableName);
//                    aWriter.WriteLine(_indent5 +  "add constraint " + aTableName + "_PK primary key clustered (HN_RID, FV_RID, TIME_ID)");
					
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            private void Generate_Store_History_Key(string aTableName, StreamWriter aWriter)
//            {
//                try
//                {
//                    aWriter.WriteLine("alter table " + aTableName);
//                    aWriter.WriteLine(_indent5 +  "add constraint " + aTableName + "_PK primary key clustered (HN_RID, TIME_ID, ST_RID, HN_MOD)   ");
										
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            private void Generate_Store_Forecast_Key(string aTableName, bool aAddMod, StreamWriter aWriter)
//            {
//                try
//                {
//                    aWriter.WriteLine("alter table " + aTableName);
//                    if (aAddMod)
//                    {
//                        aWriter.WriteLine(_indent5 + "add constraint " + aTableName + "_PK primary key clustered (HN_RID, FV_RID, TIME_ID, ST_RID, HN_MOD)   ");
//                    }
//                    else
//                    {
//                        aWriter.WriteLine(_indent5 + "add constraint " + aTableName + "_PK primary key clustered (HN_RID, FV_RID, TIME_ID, ST_RID)   ");
//                    }
										
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            private void Generate_Chain_History_Index(string aTableName, StreamWriter aWriter)
//            {
//                try
//                {
//                    aWriter.WriteLine("create index " + aTableName + "_PK on " + aTableName + " (");
//                    aWriter.WriteLine(_indent5 + "HN_RID,");
//                    aWriter.WriteLine(_indent5 + "TIME_ID)");
					
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            private void Generate_Chain_Forecast_Index(string aTableName, StreamWriter aWriter)
//            {
//                try
//                {
//                    aWriter.WriteLine("create index " + aTableName + "_PK on " + aTableName + " (");
//                    aWriter.WriteLine(_indent5 + "HN_RID,");
//                    aWriter.WriteLine(_indent5 + "FV_RID,");
//                    aWriter.WriteLine(_indent5 + "TIME_ID)");
	
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            private void Generate_Store_History_Index(string aTableName, StreamWriter aWriter)
//            {
//                try
//                {
//                    aWriter.WriteLine("create index " + aTableName + "_PK on " + aTableName + " (");
//                    aWriter.WriteLine(_indent5 + "HN_RID,");
//                    aWriter.WriteLine(_indent5 + "TIME_ID,");
//                    aWriter.WriteLine(_indent5 + "ST_RID,");
//                    aWriter.WriteLine(_indent5 + "HN_MOD)");
					
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            private void Generate_Store_Forecast_Index(string aTableName, StreamWriter aWriter)
//            {
//                try
//                {
//                    aWriter.WriteLine("create index " + aTableName + "_PK on " + aTableName + " (");
//                    aWriter.WriteLine(_indent5 + "HN_RID,");
//                    aWriter.WriteLine(_indent5 + "FV_RID,");
//                    aWriter.WriteLine(_indent5 + "TIME_ID,");
//                    aWriter.WriteLine(_indent5 + "ST_RID,");
//                    aWriter.WriteLine(_indent5 + "HN_MOD)");
					
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            private void Generate_Store_Index(string aTableName, StreamWriter aWriter)
//            {
//                try
//                {
                    // Begin TT#3599 - JSmith - Issue with Purge in MID Test
                    //aWriter.WriteLine("create index " + aTableName + "_ST_IDX on " + aTableName + " (");
//                    // Begin TT#3330 - jsmith - Creating New Database Fails
                    //if (aTableName.Contains("_DAY"))
                    //{
                    //    aWriter.WriteLine(_indent5 + "ST_RID) WITH (FILLFACTOR = 65)");
                    //}
                    //else
                    //{
                    //    aWriter.WriteLine(_indent5 + "ST_RID) WITH (FILLFACTOR = 80)");
                    //}
//                    aWriter.WriteLine("create unique nonclustered index " + aTableName + "_ST_IDX on " + aTableName + " (");
////                    if (aTableName.Contains("_DAY"))
////                    {
//                        aWriter.WriteLine(_indent5 + "[ST_RID] ASC,");
//                        aWriter.WriteLine(_indent5 + "[HN_RID] ASC,");
//                        aWriter.WriteLine(_indent5 + "[TIME_ID] ASC");
//                        aWriter.WriteLine(_indent5 + "ST_RID) WITH (FILLFACTOR = 65)");
////                    }
//                    else if (aTableName.Contains("_FORECAST"))
//                    {
//                        aWriter.WriteLine(_indent5 + "[ST_RID] ASC,");
//                        aWriter.WriteLine(_indent5 + "[HN_RID] ASC,");
//                        aWriter.WriteLine(_indent5 + "[FV_RID] ASC,");
//                        aWriter.WriteLine(_indent5 + "[TIME_ID] ASC");
//                        aWriter.WriteLine(_indent5 + ") WITH (FILLFACTOR = 80) ");
//                    }
////                    else
////                    {
//                        aWriter.WriteLine(_indent5 + "[ST_RID] ASC,");
//                        aWriter.WriteLine(_indent5 + "[HN_RID] ASC,");
//                        aWriter.WriteLine(_indent5 + "[TIME_ID] ASC");
//                        aWriter.WriteLine(_indent5 + "ST_RID) WITH (FILLFACTOR = 80)");
//                    }
                    // End TT#3599 - JSmith - Issue with Purge in MID Test

//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            private void Generate_Node_Constraint(string aTableName, string aConstraintName, StreamWriter aWriter)
//            {
//                try
//                {
//                    aWriter.WriteLine("alter table " + aTableName);
//                    aWriter.WriteLine("   add constraint " + aConstraintName + " foreign key (");
//                    aWriter.WriteLine("      HN_RID)");
//                    aWriter.WriteLine("   references HIERARCHY_NODE (");
//                    aWriter.WriteLine("      HN_RID) on update no action on delete no action");
					
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            private void Generate_Store_Constraint(string aTableName, string aConstraintName, StreamWriter aWriter)
//            {
//                try
//                {
//                    aWriter.WriteLine("alter table " + aTableName);
//                    aWriter.WriteLine("   add constraint " + aConstraintName + " foreign key (");
//                    aWriter.WriteLine("      ST_RID)");
//                    aWriter.WriteLine("   references STORES (");
//                    // BEGIN TT#739-MD - STodd - delete stores
//                    aWriter.WriteLine("      ST_RID) on update no action on delete cascade");
//                    // END TT#739-MD - STodd - delete stores
					
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            private void Generate_Version_Constraint(string aTableName, string aConstraintName, StreamWriter aWriter)
//            {
//                try
//                {
//                    aWriter.WriteLine("alter table " + aTableName);
//                    aWriter.WriteLine("   add constraint " + aConstraintName + " foreign key (");
//                    aWriter.WriteLine("      FV_RID)");
//                    aWriter.WriteLine("   references FORECAST_VERSION (");
//                    aWriter.WriteLine("      FV_RID) on update no action on delete no action");
					
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }


//            private void Generate_Check_Constraint(string aTableName, string aConstraintName, StreamWriter aWriter, string aCheckConstraint)
//            {
//                try
//                {
//                    aWriter.WriteLine("alter table " + aTableName + " add constraint " + aConstraintName + " check ([HN_MOD] = " + aCheckConstraint + ")");

//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            //override public void Generate_Views(StreamWriter aWriter, int aTableCount, IPlanComputationVariables aVariables)
//            //{
//            //    try
//            //    {
//            //        // Generate views 
//            //        Generate_Views(aWriter, aTableCount, 
//            //            Include.DBChainWeeklyForecastView, Include.DBChainWeeklyForecastTable, aVariables.GetChainWeeklyForecastDatabaseVariableList().ArrayList.Count,
//            //            Include.DBChainWeeklyHistoryView, Include.DBChainWeeklyHistoryTable, aVariables.GetChainWeeklyHistoryDatabaseVariableList().ArrayList.Count,
//            //            Include.DBStoreWeeklyForecastView, Include.DBStoreWeeklyForecastTable, aVariables.GetStoreWeeklyForecastDatabaseVariableList().ArrayList.Count,
//            //            Include.DBStoreWeeklyHistoryView, Include.DBStoreWeeklyHistoryTable, aVariables.GetStoreWeeklyHistoryDatabaseVariableList().ArrayList.Count, 
//            //            Include.DBStoreDailyHistoryView, Include.DBStoreDailyHistoryTable, aVariables.GetStoreDailyHistoryDatabaseVariableList().ArrayList.Count);	
//            //    }
//            //    catch
//            //    {
//            //    }
//            //}

//            //private void Generate_Views(StreamWriter aWriter, int aTableCount, 
//            //    string aChainWeeklyForecastView, string aChainWeeklyForecastTable, int aNoChainWeeklyForecastVariables,
//            //    string aChainWeeklyHistoryView, string aChainWeeklyHistoryTable, int aNoChainWeeklyHistoryVariables,
//            //    string aStoreWeeklyForecastView, string aStoreWeeklyForecastTable, int aNoStoreWeeklyForecastVariables,
//            //    string aStoreWeeklyHistoryView, string aStoreWeeklyHistoryTable, int aNoStoreWeeklyHistoryVariables,
//            //    string aStoreDailyHistoryView, string aStoreDailyHistoryTable, int aNoStoreDailyHistoryVariables)
//            //{
//            //    try
//            //    {
//            //        string tableName;
					
//            //        //AddViewDrop(aChainWeeklyForecastView, aWriter);
//            //        //if (aNoChainWeeklyForecastVariables > 0)
//            //        //{
//            //        //    aWriter.WriteLine("CREATE VIEW [dbo].[" + aChainWeeklyForecastView + "]");
//            //        //    aWriter.WriteLine("AS");
//            //        //    aWriter.WriteLine("SELECT * FROM " + aChainWeeklyForecastTable + " WITH (NOLOCK, INDEX(" + aChainWeeklyForecastTable + "_PK))");
//            //        //    aWriter.WriteLine("  ");
//            //        //    aWriter.WriteLine("GO");
//            //        //    aWriter.WriteLine("  ");
//            //        //}

//            //        //AddViewDrop(aChainWeeklyHistoryView, aWriter);
//            //        //if (aNoChainWeeklyHistoryVariables > 0)
//            //        //{
//            //        //    aWriter.WriteLine("CREATE VIEW [dbo].[" + aChainWeeklyHistoryView + "]");
//            //        //    aWriter.WriteLine("AS");
//            //        //    aWriter.WriteLine("SELECT * FROM " + aChainWeeklyHistoryTable + " WITH (NOLOCK, INDEX(" + aChainWeeklyHistoryTable + "_PK))");
//            //        //    aWriter.WriteLine("  ");
//            //        //    aWriter.WriteLine("GO");
//            //        //    aWriter.WriteLine("  ");
//            //        //}

//            //        //AddViewDrop(aStoreWeeklyHistoryView, aWriter);
//            //        //if (aNoStoreWeeklyHistoryVariables > 0)
//            //        //{
//            //        //    aWriter.WriteLine("CREATE VIEW [dbo].[" + aStoreWeeklyHistoryView + "]");
//            //        //    aWriter.WriteLine("AS");
//            //        //    for (int i=0; i<aTableCount; i++)
//            //        //    {
//            //        //        tableName = aStoreWeeklyHistoryTable.Replace(Include.DBTableCountReplaceString, i.ToString());
//            //        //        aWriter.WriteLine("SELECT * FROM " + tableName + " WITH (NOLOCK, INDEX(" + tableName + "_PK))");
//            //        //        if (i < aTableCount - 1)
//            //        //        {
//            //        //            aWriter.WriteLine("UNION ALL");
//            //        //        }
//            //        //    }
//            //        //    aWriter.WriteLine("  ");
//            //        //    aWriter.WriteLine("GO");
//            //        //    aWriter.WriteLine("  ");
//            //        //}

//            //        //AddViewDrop(aStoreWeeklyForecastView, aWriter);
//            //        //if (aNoStoreWeeklyForecastVariables > 0)
//            //        //{
//            //        //    aWriter.WriteLine("CREATE VIEW [dbo].[" + aStoreWeeklyForecastView + "]");
//            //        //    aWriter.WriteLine("AS");
//            //        //    for (int i=0; i<aTableCount; i++)
//            //        //    {
//            //        //        tableName = aStoreWeeklyForecastTable.Replace(Include.DBTableCountReplaceString, i.ToString());
//            //        //        aWriter.WriteLine("SELECT * FROM " + tableName + " WITH (NOLOCK, INDEX(" + tableName + "_PK))");
//            //        //        if (i < aTableCount - 1)
//            //        //        {
//            //        //            aWriter.WriteLine("UNION ALL");
//            //        //        }
//            //        //    }
//            //        //    aWriter.WriteLine("  ");
//            //        //    aWriter.WriteLine("GO");
//            //        //    aWriter.WriteLine("  ");
//            //        //}

//            //        //AddViewDrop(aStoreDailyHistoryView, aWriter);
//            //        //if (aNoStoreDailyHistoryVariables > 0)
//            //        //{
//            //        //    aWriter.WriteLine("CREATE VIEW [dbo].[" + aStoreDailyHistoryView + "]");
//            //        //    aWriter.WriteLine("AS");
//            //        //    for (int i=0; i<aTableCount; i++)
//            //        //    {
//            //        //        tableName = aStoreDailyHistoryTable.Replace(Include.DBTableCountReplaceString, i.ToString());
//            //        //        aWriter.WriteLine("SELECT * FROM " + tableName + " WITH (NOLOCK, INDEX(" + tableName + "_PK))");
//            //        //        if (i < aTableCount - 1)
//            //        //        {
//            //        //            aWriter.WriteLine("UNION ALL");
//            //        //        }
//            //        //    }
//            //        //    aWriter.WriteLine("  ");
//            //        //    aWriter.WriteLine("GO");
//            //        //    aWriter.WriteLine("  ");
//            //        //}
//            //    }
//            //    catch
//            //    {
//            //    }
//            //}

//            override public void Generate_SP_MID_DROP(string aProcedureName, StreamWriter aWriter, int aTableNumber)
//            {
//                try
//                {
//                    string procedureName = aProcedureName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());

//                    AddStoredProcedureDrop(procedureName, aWriter);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            //override public void Generate_SP_MID_XML_ST_HIS_DAY_TYPE(ProfileList aDatabaseVariables, StreamWriter aWriter)
//            //{
//            //    int count = 0;
//            //    string line;
//            //    try
//            //    {
//            //        AddTypeDrop(Include.DBStoreDailyHistoryType, aWriter);

//            //        aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBStoreDailyHistoryType + "] AS TABLE(");
//            //        aWriter.WriteLine("  [HN_MOD] [int],");
//            //        aWriter.WriteLine("  [HN_RID] [int],");
//            //        aWriter.WriteLine("  [TIME_ID] [int],");
//            //        aWriter.WriteLine("  [ST_RID] [int],");
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line = "  [" + vp.DatabaseColumnName + "] [";
//            //            switch (vp.StoreDatabaseVariableType)
//            //            {
//            //                case eVariableDatabaseType.Integer:
//            //                    line += "int]";
//            //                    break;
//            //                case eVariableDatabaseType.Float:
//            //                    line += "float]";
//            //                    break;
//            //                case eVariableDatabaseType.DateTime:
//            //                    line += "smalldatetime]";
//            //                    break;
//            //                case eVariableDatabaseType.Real:
//            //                    line += "real]";
//            //                    break;
//            //                case eVariableDatabaseType.BigInteger:
//            //                    line += "bigint]";
//            //                    break;
//            //            }
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }

//            //            aWriter.WriteLine(line);
//            //        }
//            //        aWriter.WriteLine("PRIMARY KEY (HN_MOD, HN_RID, TIME_ID, ST_RID)");
//            //        aWriter.WriteLine(")");

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}

//            //override public void Generate_SP_MID_XML_ST_HIS_WK_TYPE(ProfileList aDatabaseVariables, StreamWriter aWriter)
//            //{
//            //    int count = 0;
//            //    string line;
//            //    try
//            //    {
//            //        AddTypeDrop(Include.DBStoreWeeklyHistoryType, aWriter);

//            //        aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBStoreWeeklyHistoryType + "] AS TABLE(");
//            //        aWriter.WriteLine("  [HN_MOD] [int],");
//            //        aWriter.WriteLine("  [HN_RID] [int],");
//            //        aWriter.WriteLine("  [TIME_ID] [int],");
//            //        aWriter.WriteLine("  [ST_RID] [int],");
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line = "  [" + vp.DatabaseColumnName + "] [";
//            //            switch (vp.ChainDatabaseVariableType)
//            //            {
//            //                case eVariableDatabaseType.Integer:
//            //                    line += "int]";
//            //                    break;
//            //                case eVariableDatabaseType.Float:
//            //                    line += "float]";
//            //                    break;
//            //                case eVariableDatabaseType.DateTime:
//            //                    line += "smalldatetime]";
//            //                    break;
//            //                case eVariableDatabaseType.Real:
//            //                    line += "real]";
//            //                    break;
//            //                case eVariableDatabaseType.BigInteger:
//            //                    line += "bigint]";
//            //                    break;
//            //            }
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }

//            //            aWriter.WriteLine(line);
//            //        }
//            //        aWriter.WriteLine("PRIMARY KEY (HN_MOD, HN_RID, TIME_ID, ST_RID)");
//            //        aWriter.WriteLine(")");

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}

//            //override public void Generate_SP_MID_XML_ST_FOR_WK_TYPE(ProfileList aDatabaseVariables, StreamWriter aWriter)
//            //{
//            //    int count = 0;
//            //    string line;
//            //    try
//            //    {
//            //        AddTypeDrop(Include.DBStoreWeeklyForecastType, aWriter);
//            //        AddTypeDrop(Include.DBStoreWeeklyForecastLockType, aWriter);

//            //        aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBStoreWeeklyForecastType + "] AS TABLE(");
//            //        aWriter.WriteLine("  [HN_MOD] [int],");
//            //        aWriter.WriteLine("  [HN_RID] [int],");
//            //        aWriter.WriteLine("  [FV_RID] [int],");
//            //        aWriter.WriteLine("  [TIME_ID] [int],");
//            //        aWriter.WriteLine("  [ST_RID] [int],");
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line = "  [" + vp.DatabaseColumnName + "] [";
//            //            switch (vp.ChainDatabaseVariableType)
//            //            {
//            //                case eVariableDatabaseType.Integer:
//            //                    line += "int]";
//            //                    break;
//            //                case eVariableDatabaseType.Float:
//            //                    line += "float]";
//            //                    break;
//            //                case eVariableDatabaseType.DateTime:
//            //                    line += "smalldatetime]";
//            //                    break;
//            //                case eVariableDatabaseType.Real:
//            //                    line += "real]";
//            //                    break;
//            //                case eVariableDatabaseType.BigInteger:
//            //                    line += "bigint]";
//            //                    break;
//            //            }
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }

//            //            aWriter.WriteLine(line);
//            //        }
//            //        aWriter.WriteLine("PRIMARY KEY (HN_MOD, HN_RID, FV_RID, TIME_ID, ST_RID)");
//            //        aWriter.WriteLine(")");

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);

//            //        count = 0;
//            //        aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBStoreWeeklyForecastLockType + "] AS TABLE(");
//            //        aWriter.WriteLine("  [HN_RID] [int],");
//            //        aWriter.WriteLine("  [FV_RID] [int],");
//            //        aWriter.WriteLine("  [TIME_ID] [int],");
//            //        aWriter.WriteLine("  [ST_RID] [int],");
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line = "  [" + vp.DatabaseColumnName + "_LOCK] [char]";
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }

//            //            aWriter.WriteLine(line);
//            //        }
//            //        aWriter.WriteLine("PRIMARY KEY (HN_RID, FV_RID, TIME_ID, ST_RID)");
//            //        aWriter.WriteLine(")");

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}

//            //override public void Generate_SP_MID_XML_ST_Read_TYPES(StreamWriter aWriter)
//            //{
//            //    try
//            //    {
//            //        //AddTypeDrop(Include.DBStoreWeeklyHistoryReadType, aWriter);
//            //        //AddTypeDrop(Include.DBStoreDailyHistoryReadType, aWriter);
//            //        //AddTypeDrop(Include.DBStoreWeeklyModifiedReadType, aWriter);
//            //        //AddTypeDrop(Include.DBStoreWeeklyForecastReadType, aWriter);

//            //        //aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBStoreWeeklyHistoryReadType + "] AS TABLE(");
//            //        //aWriter.WriteLine("  [HN_MOD] [int],");
//            //        //aWriter.WriteLine("  [HN_RID] [int],");
//            //        //aWriter.WriteLine("  [TIME_ID] [int],");
                    
//            //        //aWriter.WriteLine("PRIMARY KEY (HN_MOD, HN_RID, TIME_ID)");
//            //        //aWriter.WriteLine(")");

//            //        //aWriter.WriteLine(System.Environment.NewLine);
//            //        //aWriter.WriteLine("GO");
//            //        //aWriter.WriteLine(System.Environment.NewLine);

//            //        //aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBStoreDailyHistoryReadType + "] AS TABLE(");
//            //        //aWriter.WriteLine("  [HN_MOD] [int],");
//            //        //aWriter.WriteLine("  [HN_RID] [int],");
//            //        //aWriter.WriteLine("  [TIME_ID] [int],");

//            //        //aWriter.WriteLine("PRIMARY KEY (HN_MOD, HN_RID, TIME_ID)");
//            //        //aWriter.WriteLine(")");

//            //        //aWriter.WriteLine(System.Environment.NewLine);
//            //        //aWriter.WriteLine("GO");
//            //        //aWriter.WriteLine(System.Environment.NewLine);

//            //        //aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBStoreWeeklyModifiedReadType + "] AS TABLE(");
//            //        //aWriter.WriteLine("  [HN_MOD] [int],");
//            //        //aWriter.WriteLine("  [HN_RID] [int],");
//            //        //aWriter.WriteLine("  [FV_RID] [int],");
//            //        //aWriter.WriteLine("  [TIME_ID] [int],");

//            //        //aWriter.WriteLine("PRIMARY KEY (HN_MOD, HN_RID, FV_RID, TIME_ID)");
//            //        //aWriter.WriteLine(")");

//            //        //aWriter.WriteLine(System.Environment.NewLine);
//            //        //aWriter.WriteLine("GO");
//            //        //aWriter.WriteLine(System.Environment.NewLine);

//            //        //aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBStoreWeeklyForecastReadType + "] AS TABLE(");
//            //        //aWriter.WriteLine("  [HN_MOD] [int],");
//            //        //aWriter.WriteLine("  [HN_RID] [int],");
//            //        //aWriter.WriteLine("  [FV_RID] [int],");
//            //        //aWriter.WriteLine("  [TIME_ID] [int],");

//            //        //aWriter.WriteLine("PRIMARY KEY (HN_MOD, HN_RID, FV_RID, TIME_ID)");
//            //        //aWriter.WriteLine(")");

//            //        //aWriter.WriteLine(System.Environment.NewLine);
//            //        //aWriter.WriteLine("GO");
//            //        //aWriter.WriteLine(System.Environment.NewLine);

                    
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}

//            //override public void Generate_SP_MID_XML_ST_HIS_DAY_WRITE(string aProcedureName, string aTableName, 
//            //    string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableNumber, 
//            //    string aTableCountColumn)
//            //{
//            //    string line;
//            //    int count = 0;
//            //    try
//            //    {
//            //        string procedureName = aProcedureName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//            //        string tableName = aTableName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());

//            //        if (aDatabaseVariables == null ||
//            //            aDatabaseVariables.Count == 0)
//            //        {
//            //            return;
//            //        }

//            //        aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
//            //        aWriter.WriteLine(" ");
//            //        aWriter.WriteLine("@dt " + Include.DBStoreDailyHistoryType + " READONLY");
//            //        aWriter.WriteLine("AS");
//            //        aWriter.WriteLine("SET NOCOUNT ON");

//            //        aWriter.WriteLine("MERGE INTO " + tableName + " with (ROWLOCK) AS TARGET");
//            //        aWriter.WriteLine("USING @dt AS SOURCE");
//            //        aWriter.WriteLine("on (SOURCE.HN_MOD = TARGET.HN_MOD and SOURCE.HN_RID = TARGET.HN_RID and SOURCE.TIME_ID = TARGET.TIME_ID and SOURCE.ST_RID = TARGET.ST_RID)");
//            //        aWriter.WriteLine("WHEN MATCHED THEN");
//            //        aWriter.WriteLine("UPDATE ");
//            //        count = 0;
//            //        line = _blankLine;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            if (count == 1)
//            //            {
//            //                line = line.Insert(9, "SET");
//            //            }
//            //            line = line.Insert(13, "TARGET." + vp.DatabaseColumnName);
//            //            line = line.TrimEnd();
//            //            line += " = COALESCE(SOURCE." + vp.DatabaseColumnName + ", ";
//            //            line += "TARGET." + vp.DatabaseColumnName + ")";
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ",";
//            //            }
//            //            aWriter.WriteLine(line);
//            //            line = _blankLine;
//            //        }

//            //        aWriter.WriteLine("WHEN NOT MATCHED BY TARGET THEN");
//            //        line = "INSERT (HN_MOD, HN_RID, TIME_ID, ST_RID, ";
//            //        count = 0;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += vp.DatabaseColumnName;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            if (line.Length > 150)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent5;

//            //            }
//            //        }
//            //        line += ")";
//            //        aWriter.WriteLine(line);

//            //        count = 0;
//            //        line = "VALUES ( SOURCE.HN_MOD, SOURCE.HN_RID, SOURCE.TIME_ID, SOURCE.ST_RID, ";
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += " SOURCE." + vp.DatabaseColumnName;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ",";
//            //            }
//            //            if (line.Length > 110)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent5;

//            //            }
//            //        }
//            //        line += ");";
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);
//            //        }

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}

//            //override public void Generate_SP_MID_XML_ST_HIS_WK_WRITE(string aProcedureName, string aTableName, 
//            //    string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, 
//            //    int aTableNumber, string aTableCountColumn)
//            //{
//            //    string line;
//            //    int count = 0;
//            //    try
//            //    {
//            //        string procedureName = aProcedureName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//            //        string tableName = aTableName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());

//            //        if (aDatabaseVariables == null ||
//            //            aDatabaseVariables.Count == 0)
//            //        {
//            //            return;
//            //        }

//            //        aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
//            //        aWriter.WriteLine(" ");
//            //        aWriter.WriteLine("@dt " + Include.DBStoreWeeklyHistoryType + " READONLY");
//            //        aWriter.WriteLine("AS");
//            //        aWriter.WriteLine("SET NOCOUNT ON");

//            //        aWriter.WriteLine("MERGE INTO " + tableName + " with (ROWLOCK) AS TARGET");
//            //        aWriter.WriteLine("USING @dt AS SOURCE");
//            //        aWriter.WriteLine("on (SOURCE.HN_MOD = TARGET.HN_MOD and SOURCE.HN_RID = TARGET.HN_RID and SOURCE.TIME_ID = TARGET.TIME_ID and SOURCE.ST_RID = TARGET.ST_RID)");
//            //        aWriter.WriteLine("WHEN MATCHED THEN");
//            //        aWriter.WriteLine("UPDATE ");
//            //        count = 0;
//            //        line = _blankLine;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            if (count == 1)
//            //            {
//            //                line = line.Insert(9, "SET");
//            //            }
//            //            line = line.Insert(13, "TARGET." + vp.DatabaseColumnName);
//            //            line = line.TrimEnd();
//            //            line += " = COALESCE(SOURCE." + vp.DatabaseColumnName + ", ";
//            //            line += "TARGET." + vp.DatabaseColumnName + ")";
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ",";
//            //            }
//            //            aWriter.WriteLine(line);
//            //            line = _blankLine;
//            //        }

//            //        aWriter.WriteLine("WHEN NOT MATCHED BY TARGET THEN");
//            //        line = "INSERT (HN_MOD, HN_RID, TIME_ID, ST_RID, ";
//            //        count = 0;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += vp.DatabaseColumnName;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            if (line.Length > 150)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent5;

//            //            }
//            //        }
//            //        line += ")";
//            //        aWriter.WriteLine(line);

//            //        count = 0;
//            //        line = "VALUES ( SOURCE.HN_MOD, SOURCE.HN_RID, SOURCE.TIME_ID, SOURCE.ST_RID, ";
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += " SOURCE." + vp.DatabaseColumnName;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ",";
//            //            }
//            //            if (line.Length > 110)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent5;

//            //            }
//            //        }
//            //        line += ");";
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);
//            //        }

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}

//            //override public void Generate_SP_MID_XML_ST_FOR_WK_WRITE(string aProcedureName, string aTableName, 
//            //    string aLockTableName, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, 
//            //    int aTableNumber, string aTableCountColumn)
//            //{
//            //    string line;
//            //    int count = 0;
//            //    try
//            //    {
//            //        string procedureName = aProcedureName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//            //        string tableName = aTableName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());

//            //        if (aDatabaseVariables == null ||
//            //            aDatabaseVariables.Count == 0)
//            //        {
//            //            return;
//            //        }

//            //        aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
//            //        aWriter.WriteLine(" ");
//            //        // Begin TT#3373 - JSmith - Save Store Forecast receive DBNull error
//            //        //aWriter.WriteLine("@dt " + Include.DBStoreWeeklyForecastType + " READONLY,");
//            //        //aWriter.WriteLine("@dtLock " + Include.DBStoreWeeklyForecastLockType + " READONLY,");
//            //        //aWriter.WriteLine("@SaveLocks CHAR");
//            //        aWriter.WriteLine("@dt " + Include.DBStoreWeeklyForecastType + " READONLY");
//            //        // End TT#3373 - JSmith - Save Store Forecast receive DBNull error
//            //        aWriter.WriteLine("AS");
//            //        aWriter.WriteLine("SET NOCOUNT ON");
//            //        aWriter.WriteLine("MERGE INTO " + tableName + " with (ROWLOCK) AS TARGET");
//            //        aWriter.WriteLine("USING @dt AS SOURCE");
//            //        aWriter.WriteLine("on (SOURCE.HN_MOD = TARGET.HN_MOD and SOURCE.HN_RID = TARGET.HN_RID and SOURCE.FV_RID = TARGET.FV_RID and SOURCE.TIME_ID = TARGET.TIME_ID and SOURCE.ST_RID = TARGET.ST_RID)");
//            //        aWriter.WriteLine("WHEN MATCHED THEN");
//            //        aWriter.WriteLine("UPDATE ");
//            //        count = 0;
//            //        line = _blankLine;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            if (count == 1)
//            //            {
//            //                line = line.Insert(9, "SET");
//            //            }
//            //            line = line.Insert(13, "TARGET." + vp.DatabaseColumnName);
//            //            line = line.TrimEnd();
//            //            line += " = COALESCE(SOURCE." + vp.DatabaseColumnName + ", ";
//            //            line += "TARGET." + vp.DatabaseColumnName + ")";
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ",";
//            //            }
//            //            aWriter.WriteLine(line);
//            //            line = _blankLine;
//            //        }

//            //        aWriter.WriteLine("WHEN NOT MATCHED BY TARGET THEN");
//            //        line = "INSERT (HN_MOD, HN_RID, FV_RID, TIME_ID, ST_RID, ";
//            //        count = 0;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += vp.DatabaseColumnName;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            if (line.Length > 150)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent5;

//            //            }
//            //        }
//            //        line += ")";
//            //        aWriter.WriteLine(line);

//            //        count = 0;
//            //        line = "VALUES ( SOURCE.HN_MOD, SOURCE.HN_RID, SOURCE.FV_RID, SOURCE.TIME_ID, SOURCE.ST_RID,";
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += " SOURCE." + vp.DatabaseColumnName;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ",";
//            //            }
//            //            if (line.Length > 110)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent5;

//            //            }
//            //        }
//            //        line += ");";
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);
//            //        }

//            //        // Begin TT#3373 - JSmith - Save Store Forecast receive DBNull error
//            //        //  process locks
//            //        //aWriter.WriteLine("if @SaveLocks = '1'");
//            //        //aWriter.WriteLine("begin");
//            //        //aWriter.WriteLine("MERGE INTO " + aLockTableName + " with (ROWLOCK) AS TARGET");
//            //        //aWriter.WriteLine("USING @dtLock AS SOURCE");
//            //        //aWriter.WriteLine("on (SOURCE.HN_RID = TARGET.HN_RID and SOURCE.FV_RID = TARGET.FV_RID and SOURCE.TIME_ID = TARGET.TIME_ID and SOURCE.ST_RID = TARGET.ST_RID)");
//            //        //aWriter.WriteLine("WHEN MATCHED THEN");
//            //        //aWriter.WriteLine("UPDATE ");
//            //        //count = 0;
//            //        //line = _blankLine;
//            //        //foreach (VariableProfile vp in aDatabaseVariables)
//            //        //{
//            //        //    ++count;
//            //        //    if (count == 1)
//            //        //    {
//            //        //        line = line.Insert(9, "SET");
//            //        //    }
//            //        //    line = line.Insert(13, "TARGET." + vp.DatabaseColumnName + "_LOCK");
//            //        //    line = line.TrimEnd();
//            //        //    line += " = COALESCE(SOURCE." + vp.DatabaseColumnName + "_LOCK, ";
//            //        //    line += "TARGET." + vp.DatabaseColumnName + "_LOCK)";
//            //        //    if (count < aDatabaseVariables.Count)
//            //        //    {
//            //        //        line += ",";
//            //        //    }
//            //        //    aWriter.WriteLine(line);
//            //        //    line = _blankLine;
//            //        //}

//            //        //aWriter.WriteLine("WHEN NOT MATCHED BY TARGET THEN");
//            //        //line = "INSERT (HN_RID, FV_RID, TIME_ID, ST_RID, ";
//            //        //count = 0;
//            //        //foreach (VariableProfile vp in aDatabaseVariables)
//            //        //{
//            //        //    ++count;
//            //        //    line += vp.DatabaseColumnName + "_LOCK";
//            //        //    if (count < aDatabaseVariables.Count)
//            //        //    {
//            //        //        line += ", ";
//            //        //    }
//            //        //    if (line.Length > 150)
//            //        //    {
//            //        //        aWriter.WriteLine(line);
//            //        //        line = _indent5;

//            //        //    }
//            //        //}
//            //        //line += ")";
//            //        //aWriter.WriteLine(line);

//            //        //count = 0;
//            //        //line = "VALUES ( SOURCE.HN_RID, SOURCE.FV_RID, SOURCE.TIME_ID, SOURCE.ST_RID,";
//            //        //foreach (VariableProfile vp in aDatabaseVariables)
//            //        //{
//            //        //    ++count;
//            //        //    line += " SOURCE." + vp.DatabaseColumnName + "_LOCK";
//            //        //    if (count < aDatabaseVariables.Count)
//            //        //    {
//            //        //        line += ",";
//            //        //    }
//            //        //    if (line.Length > 110)
//            //        //    {
//            //        //        aWriter.WriteLine(line);
//            //        //        line = _indent5;

//            //        //    }
//            //        //}
//            //        //line += ");";
//            //        //if (line.Length > 0)
//            //        //{
//            //        //    aWriter.WriteLine(line);
//            //        //}
//            //        //aWriter.WriteLine("end");
//            //        // End TT#3373 - JSmith - Save Store Forecast receive DBNull error

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}

//            // Begin TT#3373 - JSmith - Save Store Forecast receive DBNull error
//            //override public void Generate_SP_MID_XML_ST_FOR_WK_LOCK_WRITE(string aProcedureName, string aTableName,
//            //    string aLockTableName, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter,
//            //    string aTableCountColumn)
//            //{
//            //    string line;
//            //    int count = 0;
//            //    try
//            //    {
//            //        string procedureName = Include.DBStoreWeeklyForecastLockWriteSP;

//            //        if (aDatabaseVariables == null ||
//            //            aDatabaseVariables.Count == 0)
//            //        {
//            //            return;
//            //        }

//            //        aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
//            //        aWriter.WriteLine(" ");
//            //        aWriter.WriteLine("@dtLock " + Include.DBStoreWeeklyForecastLockType + " READONLY");
//            //        aWriter.WriteLine("AS");
//            //        aWriter.WriteLine("SET NOCOUNT ON");
                    
//            //        aWriter.WriteLine("MERGE INTO " + aLockTableName + " with (ROWLOCK) AS TARGET");
//            //        aWriter.WriteLine("USING @dtLock AS SOURCE");
//            //        aWriter.WriteLine("on (SOURCE.HN_RID = TARGET.HN_RID and SOURCE.FV_RID = TARGET.FV_RID and SOURCE.TIME_ID = TARGET.TIME_ID and SOURCE.ST_RID = TARGET.ST_RID)");
//            //        aWriter.WriteLine("WHEN MATCHED THEN");
//            //        aWriter.WriteLine("UPDATE ");
//            //        count = 0;
//            //        line = _blankLine;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            if (count == 1)
//            //            {
//            //                line = line.Insert(9, "SET");
//            //            }
//            //            line = line.Insert(13, "TARGET." + vp.DatabaseColumnName + "_LOCK");
//            //            line = line.TrimEnd();
//            //            line += " = COALESCE(SOURCE." + vp.DatabaseColumnName + "_LOCK, ";
//            //            line += "TARGET." + vp.DatabaseColumnName + "_LOCK)";
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ",";
//            //            }
//            //            aWriter.WriteLine(line);
//            //            line = _blankLine;
//            //        }

//            //        aWriter.WriteLine("WHEN NOT MATCHED BY TARGET THEN");
//            //        line = "INSERT (HN_RID, FV_RID, TIME_ID, ST_RID, ";
//            //        count = 0;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += vp.DatabaseColumnName + "_LOCK";
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            if (line.Length > 150)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent5;

//            //            }
//            //        }
//            //        line += ")";
//            //        aWriter.WriteLine(line);

//            //        count = 0;
//            //        line = "VALUES ( SOURCE.HN_RID, SOURCE.FV_RID, SOURCE.TIME_ID, SOURCE.ST_RID,";
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += " SOURCE." + vp.DatabaseColumnName + "_LOCK";
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ",";
//            //            }
//            //            if (line.Length > 110)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent5;

//            //            }
//            //        }
//            //        line += ");";
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);
//            //        }

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}
//            // End TT#3373 - JSmith - Save Store Forecast receive DBNull error

//            //override public void Generate_SP_MID_XML_CHN_HIS_WK_WRITE(string aProcedureName, string aTableName, 
//            //    string aTempTableName, string aTableCountColumn, ProfileList aDatabaseVariables, StreamWriter aWriter)
//            //{
//            //    string line;
//            //    int count = 0;
//            //    try
//            //    {
//            //        AddStoredProcedureDrop(aProcedureName, aWriter);

                    
//            //        AddTypeDrop(Include.DBChainWeeklyHistoryType, aWriter);

//            //        if (aDatabaseVariables == null ||
//            //            aDatabaseVariables.Count == 0)
//            //        {
//            //            return;
//            //        }

//            //        aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBChainWeeklyHistoryType + "] AS TABLE(");
//            //        aWriter.WriteLine("  [HN_RID] [int],");
//            //        aWriter.WriteLine("  [TIME_ID] [int],");
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line = "  [" + vp.DatabaseColumnName + "] [";
//            //            switch (vp.ChainDatabaseVariableType)
//            //            {
//            //                case eVariableDatabaseType.Integer:
//            //                    line += "int]";
//            //                    break;
//            //                case eVariableDatabaseType.Float:
//            //                    line += "float]";
//            //                    break;
//            //                case eVariableDatabaseType.DateTime:
//            //                    line += "smalldatetime]";
//            //                    break;
//            //                case eVariableDatabaseType.Real:
//            //                    line += "real]";
//            //                    break;
//            //                case eVariableDatabaseType.BigInteger:
//            //                    line += "bigint]";
//            //                    break;
//            //            }
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }

//            //            aWriter.WriteLine(line);
//            //        }
//            //        aWriter.WriteLine("PRIMARY KEY (HN_RID, TIME_ID)");
//            //        aWriter.WriteLine(")");

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);

//            //        aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
//            //        aWriter.WriteLine(" ");
//            //        aWriter.WriteLine("@dt " + Include.DBChainWeeklyHistoryType + " READONLY");
//            //        aWriter.WriteLine("AS");
//            //        aWriter.WriteLine("SET NOCOUNT ON");

//            //        aWriter.WriteLine("MERGE INTO " + aTableName + " with (ROWLOCK) AS TARGET");
//            //        aWriter.WriteLine("USING @dt AS SOURCE");
//            //        aWriter.WriteLine("on (SOURCE.HN_RID = TARGET.HN_RID and SOURCE.TIME_ID = TARGET.TIME_ID)");
//            //        aWriter.WriteLine("WHEN MATCHED THEN");
//            //        aWriter.WriteLine("UPDATE ");
//            //        count = 0;
//            //        line = _blankLine;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            if (count == 1)
//            //            {
//            //                line = line.Insert(9, "SET");
//            //            }
//            //            line = line.Insert(13, "TARGET." + vp.DatabaseColumnName); 
//            //            line = line.TrimEnd();
//            //            line += " = COALESCE(SOURCE." + vp.DatabaseColumnName + ", ";
//            //            line += "TARGET." + vp.DatabaseColumnName + ")";
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ",";
//            //            }
//            //            aWriter.WriteLine(line);
//            //            line = _blankLine;
//            //        }

//            //        aWriter.WriteLine("WHEN NOT MATCHED BY TARGET THEN");
//            //        line = "INSERT (HN_RID, TIME_ID, ";
//            //        count = 0;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += vp.DatabaseColumnName;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            if (line.Length > 150)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent5;

//            //            }
//            //        }
//            //        line += ")";
//            //        aWriter.WriteLine(line);

//            //        count = 0;
//            //        line = "VALUES ( SOURCE.HN_RID, SOURCE.TIME_ID,";
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += " SOURCE." + vp.DatabaseColumnName;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ",";
//            //            }
//            //            if (line.Length > 110)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent5;

//            //            }
//            //        }
//            //        line += ");";
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);
//            //        }

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}

//            //override public void Generate_SP_MID_XML_CHN_FOR_WK_WRITE(string aProcedureName, string aTableName, 
//            //    string aLockTableName, string aTempTableName, string aTableCountColumn,
//            //    ProfileList aDatabaseVariables, StreamWriter aWriter)
//            //{
//            //    string line;
//            //    int count = 0;
//            //    try
//            //    {
//            //        AddStoredProcedureDrop(aProcedureName, aWriter);

//            //        AddTypeDrop(Include.DBChainWeeklyForecastType, aWriter);
//            //        AddTypeDrop(Include.DBChainWeeklyForecastLockType, aWriter);

//            //        if (aDatabaseVariables == null ||
//            //            aDatabaseVariables.Count == 0)
//            //        {
//            //            return;
//            //        }

//            //        //aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBChainWeeklyForecastType + "] AS TABLE(");
//            //        //aWriter.WriteLine("  [HN_RID] [int],");
//            //        //aWriter.WriteLine("  [FV_RID] [int],");
//            //        //aWriter.WriteLine("  [TIME_ID] [int],");
//            //        //foreach (VariableProfile vp in aDatabaseVariables)
//            //        //{
//            //        //    ++count;
//            //        //    line = "  [" + vp.DatabaseColumnName + "] [";
//            //        //    switch (vp.ChainDatabaseVariableType)
//            //        //    {
//            //        //        case eVariableDatabaseType.Integer:
//            //        //            line += "int]";
//            //        //            break;
//            //        //        case eVariableDatabaseType.Float:
//            //        //            line += "float]";
//            //        //            break;
//            //        //        case eVariableDatabaseType.DateTime:
//            //        //            line += "smalldatetime]";
//            //        //            break;
//            //        //        case eVariableDatabaseType.Real:
//            //        //            line += "real]";
//            //        //            break;
//            //        //        case eVariableDatabaseType.BigInteger:
//            //        //            line += "bigint]";
//            //        //            break;
//            //        //    }
//            //        //    if (count < aDatabaseVariables.Count)
//            //        //    {
//            //        //        line += ", ";
//            //        //    }

//            //        //    aWriter.WriteLine(line);
//            //        //}
//            //        //aWriter.WriteLine("PRIMARY KEY (HN_RID, FV_RID, TIME_ID)");
//            //        //aWriter.WriteLine(")");

//            //        //aWriter.WriteLine(System.Environment.NewLine);
//            //        //aWriter.WriteLine("GO");
//            //        //aWriter.WriteLine(System.Environment.NewLine);

//            //        //count = 0;
//            //        //aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBChainWeeklyForecastLockType + "] AS TABLE(");
//            //        //aWriter.WriteLine("  [HN_RID] [int],");
//            //        //aWriter.WriteLine("  [FV_RID] [int],");
//            //        //aWriter.WriteLine("  [TIME_ID] [int],");
//            //        //foreach (VariableProfile vp in aDatabaseVariables)
//            //        //{
//            //        //    ++count;
//            //        //    line = "  [" + vp.DatabaseColumnName + "_LOCK] [char]";
//            //        //    if (count < aDatabaseVariables.Count)
//            //        //    {
//            //        //        line += ", ";
//            //        //    }

//            //        //    aWriter.WriteLine(line);
//            //        //}
//            //        //aWriter.WriteLine("PRIMARY KEY (HN_RID, FV_RID, TIME_ID)");
//            //        //aWriter.WriteLine(")");

//            //        //aWriter.WriteLine(System.Environment.NewLine);
//            //        //aWriter.WriteLine("GO");
//            //        //aWriter.WriteLine(System.Environment.NewLine);

//            //        aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
//            //        aWriter.WriteLine(" ");
//            //        aWriter.WriteLine("@dt " + Include.DBChainWeeklyForecastType + " READONLY,");
//            //        aWriter.WriteLine("@dtLock " + Include.DBChainWeeklyForecastLockType + " READONLY,");
//            //        aWriter.WriteLine("@SaveLocks CHAR");
//            //        aWriter.WriteLine("AS");
//            //        aWriter.WriteLine("SET NOCOUNT ON");
//            //        aWriter.WriteLine("MERGE INTO " + aTableName + " with (ROWLOCK) AS TARGET");
//            //        aWriter.WriteLine("USING @dt AS SOURCE");
//            //        aWriter.WriteLine("on (SOURCE.HN_RID = TARGET.HN_RID and SOURCE.FV_RID = TARGET.FV_RID and SOURCE.TIME_ID = TARGET.TIME_ID)");
//            //        aWriter.WriteLine("WHEN MATCHED THEN");
//            //        aWriter.WriteLine("UPDATE ");
//            //        count = 0;
//            //        line = _blankLine;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            if (count == 1)
//            //            {
//            //                line = line.Insert(9, "SET");
//            //            }
//            //            line = line.Insert(13, "TARGET." + vp.DatabaseColumnName);
//            //            line = line.TrimEnd();
//            //            line += " = COALESCE(SOURCE." + vp.DatabaseColumnName + ", ";
//            //            line += "TARGET." + vp.DatabaseColumnName + ")";
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ",";
//            //            }
//            //            aWriter.WriteLine(line);
//            //            line = _blankLine;
//            //        }

//            //        aWriter.WriteLine("WHEN NOT MATCHED BY TARGET THEN");
//            //        line = "INSERT (HN_RID, FV_RID, TIME_ID, ";
//            //        count = 0;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += vp.DatabaseColumnName;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            if (line.Length > 150)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent5;

//            //            }
//            //        }
//            //        line += ")";
//            //        aWriter.WriteLine(line);

//            //        count = 0;
//            //        line = "VALUES ( SOURCE.HN_RID, SOURCE.FV_RID, SOURCE.TIME_ID,";
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += " SOURCE." + vp.DatabaseColumnName;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ",";
//            //            }
//            //            if (line.Length > 110)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent5;

//            //            }
//            //        }
//            //        line += ");";
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);
//            //        }

//            //        //  process locks
//            //        aWriter.WriteLine("if @SaveLocks = '1'");
//            //        aWriter.WriteLine("begin");
//            //        aWriter.WriteLine("MERGE INTO " + aLockTableName + " with (ROWLOCK) AS TARGET");
//            //        aWriter.WriteLine("USING @dtLock AS SOURCE");
//            //        aWriter.WriteLine("on (SOURCE.HN_RID = TARGET.HN_RID and SOURCE.FV_RID = TARGET.FV_RID and SOURCE.TIME_ID = TARGET.TIME_ID)");
//            //        aWriter.WriteLine("WHEN MATCHED THEN");
//            //        aWriter.WriteLine("UPDATE ");
//            //        count = 0;
//            //        line = _blankLine;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            if (count == 1)
//            //            {
//            //                line = line.Insert(9, "SET");
//            //            }
//            //            line = line.Insert(13, "TARGET." + vp.DatabaseColumnName + "_LOCK");
//            //            line = line.TrimEnd();
//            //            line += " = COALESCE(SOURCE." + vp.DatabaseColumnName + "_LOCK, ";
//            //            line += "TARGET." + vp.DatabaseColumnName + "_LOCK)";
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ",";
//            //            }
//            //            aWriter.WriteLine(line);
//            //            line = _blankLine;
//            //        }

//            //        aWriter.WriteLine("WHEN NOT MATCHED BY TARGET THEN");
//            //        line = "INSERT (HN_RID, FV_RID, TIME_ID, ";
//            //        count = 0;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += vp.DatabaseColumnName + "_LOCK";
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            if (line.Length > 150)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent5;

//            //            }
//            //        }
//            //        line += ")";
//            //        aWriter.WriteLine(line);

//            //        count = 0;
//            //        line = "VALUES ( SOURCE.HN_RID, SOURCE.FV_RID, SOURCE.TIME_ID,";
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += " SOURCE." + vp.DatabaseColumnName + "_LOCK";
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ",";
//            //            }
//            //            if (line.Length > 110)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent5;

//            //            }
//            //        }
//            //        line += ");";
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);
//            //        }
//            //        aWriter.WriteLine("end");

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}
            
//            //override public void Generate_SP_MID_XML_ST_HIS_DAY_READ(string aProcedureName, string aTableName, 
//            //    string aViewName, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter,
//            //    int aTableNumber, string aTableCountColumn)
//            //{
//            //    try
//            //    {
//            //        string procedureName = aProcedureName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//            //        string tableName = aTableName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());

//            //        if (aDatabaseVariables == null ||
//            //            aDatabaseVariables.Count == 0)
//            //        {
//            //            return;
//            //        }

//            //        string tempTableName = "@dt";

//            //        aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
//            //        aWriter.WriteLine("@dt " + Include.DBStoreDailyHistoryReadType + " READONLY,");
//            //        aWriter.WriteLine("@Rollup CHAR(1) = NULL");
//            //        aWriter.WriteLine("AS");
//            //        aWriter.WriteLine("SET NOCOUNT ON");
//            //        aWriter.WriteLine("DECLARE @Tables INT,");
//            //        aWriter.WriteLine("        @Loop INT,");
//            //        aWriter.WriteLine("        @HN_TYPE INT,");
//            //        aWriter.WriteLine("        @HN_RID INT,");
//            //        aWriter.WriteLine("        @HN_MOD INT,");
//            //        aWriter.WriteLine("        @ROLL_OPTION INT,");
//            //        aWriter.WriteLine("        @LoopCount INT,");
//            //        aWriter.WriteLine("        @NextLoopCount INT");
//            //        aWriter.WriteLine("SELECT @Tables = " + aTableCountColumn + " FROM SYSTEM_OPTIONS");

//            //        aWriter.WriteLine("SELECT DISTINCT @HN_TYPE = ph.PH_TYPE, @HN_RID = t.HN_RID, @HN_MOD = t.HN_RID % @Tables,");
//            //        aWriter.WriteLine("                @ROLL_OPTION = COALESCE(ph.HISTORY_ROLL_OPTION, 800251)");
//            //        aWriter.WriteLine("        FROM HIERARCHY_NODE hn (NOLOCK)");
//            //        aWriter.WriteLine("        JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//            //        aWriter.WriteLine("        JOIN " + tempTableName + " t on t.HN_RID = hn.HN_RID");
//            //        aWriter.WriteLine("IF @HN_TYPE = 800001 and @ROLL_OPTION = 800251 -- Realtime");
//            //        aWriter.WriteLine("   BEGIN");
//            //        aWriter.WriteLine("   SET @LoopCount = 0");
//            //        aWriter.WriteLine("   CREATE TABLE #TREE (LOOPCOUNT INT NOT NULL, PARENT_HN_RID INT NOT NULL, HOME_PH_RID INT, PH_TYPE INT, CHILD_HN_RID INT NOT NULL, CHILD_HN_MOD INT NOT NULL)");
//            //        aWriter.WriteLine("   -- insert the children of the node into the temp table");
//            //        aWriter.WriteLine("   INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, CHILD_HN_RID, CHILD_HN_MOD) ");
//            //        aWriter.WriteLine("       select @LoopCount as LOOPCOUNT, @HN_RID as PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
//            //        aWriter.WriteLine("         from HIER_NODE_JOIN hnj (NOLOCK)");
//            //        aWriter.WriteLine("           JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//            //        aWriter.WriteLine("           JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//            //        aWriter.WriteLine("       where @HN_RID = hnj.PARENT_HN_RID");
//            //        aWriter.WriteLine("   SET @Loop = @@ROWCOUNT");
//            //        aWriter.WriteLine("   -- chase all paths until you get the main hierarchy (type 800000) or the lowest leaf");
//            //        aWriter.WriteLine("   SET @NextLoopCount = @LoopCount + 1");
//            //        aWriter.WriteLine("   WHILE @Loop > 0");
//            //        aWriter.WriteLine("   BEGIN");
//            //        aWriter.WriteLine("      INSERT #TREE");
//            //        aWriter.WriteLine("        select @NextLoopCount as LOOPCOUNT, hnj.PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
//            //        aWriter.WriteLine("          from HIER_NODE_JOIN hnj (NOLOCK)");
//            //        aWriter.WriteLine("            JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//            //        aWriter.WriteLine("            JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//            //        aWriter.WriteLine("            JOIN #TREE t ON hnj.PARENT_HN_RID = t.CHILD_HN_RID");
//            //        aWriter.WriteLine("        WHERE t.LOOPCOUNT =  @LoopCount AND t.PH_TYPE <> 800000");
//            //        aWriter.WriteLine("      SET @Loop = @@ROWCOUNT");
//            //        aWriter.WriteLine("      SET @LoopCount = @LoopCount + 1");
//            //        aWriter.WriteLine("      SET @NextLoopCount = @LoopCount + 1");
//            //        aWriter.WriteLine("   END");
//            //        aWriter.WriteLine("   -- join with dates from xml selecting only nodes from the main hierarchy or lowest leaf alternates");
//            //        aWriter.WriteLine("   SELECT * ");
//            //        aWriter.WriteLine("     INTO " + aTempTableName + "2");
//            //        aWriter.WriteLine("     FROM #TREE");
//            //        aWriter.WriteLine("     CROSS JOIN " + tempTableName);
//            //        aWriter.WriteLine("   where LOOPCOUNT = @LoopCount - 1");
//            //        aWriter.WriteLine("	     or PH_TYPE = 800000");

//            //        int count = 0;
//            //        string line = "       SELECT @HN_RID AS HN_RID, 1 AS FV_RID, ST_RID, shd.TIME_ID, ";
//            //        // add variables
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "COALESCE(SUM(" + vp.DatabaseColumnName + "), 0) " + vp.DatabaseColumnName + ", ";
//            //            if (line.Length > 100)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;

//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        // add locks
//            //        count = 0;
//            //        line = _indent10;
//            //        // add variables
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "0 AS " + vp.DatabaseColumnName + Include.cLockExtension;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            if (line.Length > 100)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;

//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        aWriter.WriteLine("		FROM " + aTempTableName + "2 t");
//            //        aWriter.WriteLine("		JOIN " + aViewName + " shd (NOLOCK) ON t.CHILD_HN_RID = shd.HN_RID AND shd.ST_RID > 0");
//            //        aWriter.WriteLine("			AND shd.TIME_ID = t.TIME_ID AND t.CHILD_HN_MOD = shd.HN_MOD ");
//            //        aWriter.WriteLine("		GROUP BY ST_RID, shd.TIME_ID");
//            //        aWriter.WriteLine("		OPTION (MAXDOP 1)");
//            //        aWriter.WriteLine("	RETURN 0");
//            //        aWriter.WriteLine("	END");
//            //        aWriter.WriteLine("-- Process variables");
//            //        aWriter.WriteLine("-- GET ALL THE ROWS");
//            //        aWriter.WriteLine("IF @Rollup = 'Y'");
//            //        count = 0;
//            //        line = "       SELECT shd.HN_RID, 1 AS FV_RID, shd.ST_RID, ";
//            //        // add variables
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "COALESCE(SUM(shd." + vp.DatabaseColumnName + "), 0) " + vp.DatabaseColumnName + ", ";
//            //            if (line.Length > 90)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;

//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        // add locks
//            //        count = 0;
//            //        line = _indent10;
//            //        // add variables
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "0 AS " + vp.DatabaseColumnName + Include.cLockExtension;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            if (line.Length > 90)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;

//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        aWriter.WriteLine("        FROM " + tempTableName + " xml");
//            //        aWriter.WriteLine("	JOIN " + aViewName + " shd (NOLOCK) ON xml.HN_RID = shd.HN_RID");
//            //        aWriter.WriteLine("		AND shd.TIME_ID = xml.TIME_ID");
//            //        aWriter.WriteLine("		AND shd.HN_MOD = xml.HN_MOD");
//            //        aWriter.WriteLine("	GROUP BY shd.HN_RID, shd.ST_RID");
//            //        aWriter.WriteLine("	ORDER BY shd.ST_RID");
//            //        aWriter.WriteLine("	OPTION (MAXDOP 1)");
//            //        aWriter.WriteLine("ELSE");
//            //        count = 0;
//            //        line = "       SELECT shd.HN_RID, 1 AS FV_RID, shd.TIME_ID, shd.ST_RID, ";
//            //        // add variables
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "COALESCE(shd." + vp.DatabaseColumnName + ", 0) " + vp.DatabaseColumnName + ", ";
//            //            if (line.Length > 90)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;

//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        // add locks
//            //        count = 0;
//            //        line = _indent10;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "0 AS " + vp.DatabaseColumnName + Include.cLockExtension;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            if (line.Length > 90)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;

//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        aWriter.WriteLine("		FROM " + tempTableName + " xml");
//            //        aWriter.WriteLine("	JOIN " + tableName + " shd (NOLOCK) ON xml.HN_RID = shd.HN_RID AND shd.HN_MOD = xml.HN_MOD");
//            //        aWriter.WriteLine("		AND shd.TIME_ID = xml.TIME_ID");
//            //        aWriter.WriteLine("		AND shd.HN_MOD = xml.HN_MOD");
//            //        aWriter.WriteLine("	OPTION (MAXDOP 1)");

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "2')) > 0 DROP TABLE " + aTempTableName + "2");
//            //        aWriter.WriteLine(System.Environment.NewLine);

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}

//            override public void Generate_SP_MID_XML_ST_HIS_DAY_ROLL_READ(string aProcedureName, string aTableName, 
//                string aViewName, int aTableNumber, string aTableCountColumn, string aTempTableName, 
//                ProfileList aDatabaseVariables, StreamWriter aWriter)
//            {
//                try
//                {
//                    string procedureName = aProcedureName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//                    string tableName = aTableName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());

//                    AddStoredProcedureDrop(procedureName, aWriter);

//                    if (aDatabaseVariables == null ||
//                        aDatabaseVariables.Count == 0)
//                    {
//                        return;
//                    }

//                    aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
//                    aWriter.WriteLine("@xmlDoc AS NTEXT,");
//                    aWriter.WriteLine("@Rollup CHAR(1) = NULL");
//                    aWriter.WriteLine("AS");
//                    aWriter.WriteLine("SET NOCOUNT ON");
//                    aWriter.WriteLine("DECLARE @idoc int,");
//                    aWriter.WriteLine("        @Tables INT,");
//                    aWriter.WriteLine("        @Loop INT,");
//                    aWriter.WriteLine("        @HN_RID INT,");
//                    aWriter.WriteLine("        @HN_MOD INT,");
//                    aWriter.WriteLine("        @LoopCount INT,");
//                    aWriter.WriteLine("        @NextLoopCount INT");
//                    aWriter.WriteLine("EXEC sp_xml_preparedocument @idoc OUTPUT, @xmlDoc");
//                    aWriter.WriteLine("SELECT @Tables = " + aTableCountColumn + " FROM SYSTEM_OPTIONS");

//                    aWriter.WriteLine("SELECT xmlHN_RID, xmlTIME_ID, xmlHN_RID % @Tables AS xmlHN_MOD");
//                    aWriter.WriteLine("	INTO " + aTempTableName);
//                    aWriter.WriteLine("	FROM OPENXML (@idoc, '/root/node/time',2)");
//                    aWriter.WriteLine("         WITH (  xmlHN_RID  int      '../@ID',");
//                    aWriter.WriteLine("                 xmlTIME_ID INT '@ID') xml");
//                    aWriter.WriteLine("   SET @LoopCount = 0");
//                    aWriter.WriteLine("   CREATE TABLE #TREE (LOOPCOUNT INT NOT NULL, PARENT_HN_RID INT NOT NULL, HOME_PH_RID INT, PH_TYPE INT, CHILD_HN_RID INT NOT NULL, CHILD_HN_MOD INT NOT NULL)");
//                    aWriter.WriteLine("   -- insert the children of the node into the temp table");
//                    aWriter.WriteLine("   INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, CHILD_HN_RID, CHILD_HN_MOD) ");
//                    aWriter.WriteLine("       select @LoopCount as LOOPCOUNT, @HN_RID as PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
//                    aWriter.WriteLine("         from HIER_NODE_JOIN hnj (NOLOCK)");
//                    aWriter.WriteLine("           JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//                    aWriter.WriteLine("           JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//                    aWriter.WriteLine("       where @HN_RID = hnj.PARENT_HN_RID");
//                    aWriter.WriteLine("   SET @Loop = @@ROWCOUNT");
//                    aWriter.WriteLine("   -- chase all paths until you get the main hierarchy (type 800000) or the lowest leaf");
//                    aWriter.WriteLine("   SET @NextLoopCount = @LoopCount + 1");
//                    aWriter.WriteLine("   WHILE @Loop > 0");
//                    aWriter.WriteLine("   BEGIN");
//                    aWriter.WriteLine("      INSERT #TREE");
//                    aWriter.WriteLine("        select @NextLoopCount as LOOPCOUNT, hnj.PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
//                    aWriter.WriteLine("          from HIER_NODE_JOIN hnj (NOLOCK)");
//                    aWriter.WriteLine("            JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//                    aWriter.WriteLine("            JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//                    aWriter.WriteLine("            JOIN #TREE t ON hnj.PARENT_HN_RID = t.CHILD_HN_RID");
//                    aWriter.WriteLine("        WHERE t.LOOPCOUNT =  @LoopCount AND t.PH_TYPE <> 800000");
//                    aWriter.WriteLine("      SET @Loop = @@ROWCOUNT");
//                    aWriter.WriteLine("      SET @LoopCount = @LoopCount + 1");
//                    aWriter.WriteLine("      SET @NextLoopCount = @LoopCount + 1");
//                    aWriter.WriteLine("   END");
//                    aWriter.WriteLine("   -- join with dates from xml selecting only nodes from the main hierarchy or lowest leaf alternates");
//                    aWriter.WriteLine("   SELECT * ");
//                    aWriter.WriteLine("     INTO " + aTempTableName + "2");
//                    aWriter.WriteLine("     FROM #TREE");
//                    aWriter.WriteLine("     CROSS JOIN " + aTempTableName);
//                    aWriter.WriteLine("   where LOOPCOUNT = @LoopCount - 1");
//                    aWriter.WriteLine("	     or PH_TYPE = 800000");
					
//                    int count = 0;
//                    string line = "       SELECT @HN_RID AS HN_RID, 1 AS FV_RID, ST_RID, shd.TIME_ID, ";
//                    // add variables
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        line += "COALESCE(SUM(" + vp.DatabaseColumnName + "), 0) " + vp.DatabaseColumnName + ", ";
//                        if (line.Length > 100)
//                        {
//                            aWriter.WriteLine(line);
//                            line = _indent10;
					
//                        }
											
//                    }
//                    if (line.Length > _indent10.Length)
//                    {
//                        aWriter.WriteLine(line);
					
//                    }
//                    // add locks
//                    count = 0;
//                    line = _indent10;
//                    // add variables
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        line += "0 AS " + vp.DatabaseColumnName + Include.cLockExtension;
//                        if (count < aDatabaseVariables.Count)
//                        {
//                            line += ", ";
//                        }
//                        if (line.Length > 100)
//                        {
//                            aWriter.WriteLine(line);
//                            line = _indent10;
					
//                        }
											
//                    }
//                    if (line.Length > _indent10.Length)
//                    {
//                        aWriter.WriteLine(line);
					
//                    }
//                    aWriter.WriteLine("		FROM " + aTempTableName + "2 t");
//                    aWriter.WriteLine("		JOIN " + aViewName + " shd (NOLOCK) ON t.CHILD_HN_RID = shd.HN_RID AND shd.ST_RID > 0");
//                    aWriter.WriteLine("			AND shd.TIME_ID = t.TIME_ID AND t.CHILD_HN_MOD = shd.HN_MOD ");
//                    aWriter.WriteLine("		GROUP BY ST_RID, shd.TIME_ID");
//                    aWriter.WriteLine("		OPTION (MAXDOP 1)");

//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            //override public void Generate_SP_MID_XML_ST_HIS_WK_READ(string aProcedureName, string aTableName, 
//            //    string aViewName, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, 
//            //    int aTableNumber, string aTableCountColumn)
//            //{
//            //    try
//            //    {
//            //        int count = 0;
//            //        string line = string.Empty;
//            //        string procedureName = aProcedureName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//            //        string tableName = aTableName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());

//            //        if (aDatabaseVariables == null ||
//            //            aDatabaseVariables.Count == 0)
//            //        {
//            //            return;
//            //        }

//            //        string tempTableName = "@dt";

//            //        aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
//            //        aWriter.WriteLine("@dt " + Include.DBStoreWeeklyHistoryReadType + " READONLY,");
//            //        aWriter.WriteLine("@Rollup CHAR(1) = NULL");
//            //        aWriter.WriteLine("AS");
//            //        aWriter.WriteLine("SET NOCOUNT ON");
//            //        aWriter.WriteLine("DECLARE @Tables INT,");
//            //        aWriter.WriteLine("        @Loop INT,");
//            //        aWriter.WriteLine("        @HN_TYPE INT,");
//            //        aWriter.WriteLine("        @HN_RID INT,");
//            //        aWriter.WriteLine("        @HN_MOD INT,");
//            //        aWriter.WriteLine("        @ROLL_OPTION INT,");
//            //        aWriter.WriteLine("        @LoopCount INT,");
//            //        aWriter.WriteLine("        @NextLoopCount INT");
//            //        aWriter.WriteLine("SELECT @Tables = " + aTableCountColumn + " FROM SYSTEM_OPTIONS");

//            //        aWriter.WriteLine("SELECT DISTINCT @HN_TYPE = ph.PH_TYPE, @HN_RID = t.HN_RID, @HN_MOD = t.HN_RID % @Tables,");
//            //        aWriter.WriteLine("                @ROLL_OPTION = COALESCE(ph.HISTORY_ROLL_OPTION, 800251)");
//            //        aWriter.WriteLine("        FROM HIERARCHY_NODE hn (NOLOCK)");
//            //        aWriter.WriteLine("        JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//            //        aWriter.WriteLine("        JOIN " + tempTableName + " t on t.HN_RID = hn.HN_RID");
//            //        aWriter.WriteLine("IF @HN_TYPE = 800001 and @ROLL_OPTION = 800251 -- Realtime");
//            //        aWriter.WriteLine("   BEGIN");
//            //        aWriter.WriteLine("   SET @LoopCount = 0");
//            //        aWriter.WriteLine("   CREATE TABLE #TREE (LOOPCOUNT INT NOT NULL, PARENT_HN_RID INT NOT NULL, HOME_PH_RID INT, PH_TYPE INT, CHILD_HN_RID INT NOT NULL, CHILD_HN_MOD INT NOT NULL)");
//            //        aWriter.WriteLine("   -- insert the children of the node into the temp table");
//            //        aWriter.WriteLine("   INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, CHILD_HN_RID, CHILD_HN_MOD) ");
//            //        aWriter.WriteLine("       select @LoopCount as LOOPCOUNT, @HN_RID as PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
//            //        aWriter.WriteLine("         from HIER_NODE_JOIN hnj (NOLOCK)");
//            //        aWriter.WriteLine("           JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//            //        aWriter.WriteLine("           JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//            //        aWriter.WriteLine("       where @HN_RID = hnj.PARENT_HN_RID");
//            //        aWriter.WriteLine("   SET @Loop = @@ROWCOUNT");
//            //        aWriter.WriteLine("   -- chase all paths until you get the main hierarchy (type 800000) or the lowest leaf");
//            //        aWriter.WriteLine("   SET @NextLoopCount = @LoopCount + 1");
//            //        aWriter.WriteLine("   WHILE @Loop > 0");
//            //        aWriter.WriteLine("   BEGIN");
//            //        aWriter.WriteLine("      INSERT #TREE");
//            //        aWriter.WriteLine("        select @NextLoopCount as LOOPCOUNT, hnj.PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
//            //        aWriter.WriteLine("          from HIER_NODE_JOIN hnj (NOLOCK)");
//            //        aWriter.WriteLine("            JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//            //        aWriter.WriteLine("            JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//            //        aWriter.WriteLine("            JOIN #TREE t ON hnj.PARENT_HN_RID = t.CHILD_HN_RID");
//            //        aWriter.WriteLine("        WHERE t.LOOPCOUNT =  @LoopCount AND t.PH_TYPE <> 800000");
//            //        aWriter.WriteLine("      SET @Loop = @@ROWCOUNT");
//            //        aWriter.WriteLine("      SET @LoopCount = @LoopCount + 1");
//            //        aWriter.WriteLine("      SET @NextLoopCount = @LoopCount + 1");
//            //        aWriter.WriteLine("   END");
//            //        aWriter.WriteLine("   -- join with dates from xml selecting only nodes from the main hierarchy or lowest leaf alternates");
//            //        aWriter.WriteLine("   SELECT * ");
//            //        aWriter.WriteLine("     INTO " + aTempTableName + "2");
//            //        aWriter.WriteLine("     FROM #TREE");
//            //        aWriter.WriteLine("     CROSS JOIN " + tempTableName);
//            //        aWriter.WriteLine("   where LOOPCOUNT = @LoopCount - 1");
//            //        aWriter.WriteLine("	     or PH_TYPE = 800000");

//            //        count = 0;
//            //        line = "       SELECT @HN_RID AS HN_RID, 1 AS FV_RID, ST_RID, shw.TIME_ID, ";
//            //        // add variables
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "COALESCE(SUM(" + vp.DatabaseColumnName + "), 0) " + vp.DatabaseColumnName + ", ";
//            //            if (line.Length > 100)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;

//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        // add locks
//            //        count = 0;
//            //        line = _indent10;
//            //        // add variables
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "0 AS " + vp.DatabaseColumnName + Include.cLockExtension;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            if (line.Length > 100)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;

//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        aWriter.WriteLine("		FROM " + aTempTableName + "2 t");
//            //        aWriter.WriteLine("		JOIN " + aViewName + " shw (NOLOCK) ON t.CHILD_HN_RID = shw.HN_RID AND shw.ST_RID > 0");
//            //        aWriter.WriteLine("			AND shw.TIME_ID = t.TIME_ID AND t.CHILD_HN_MOD = shw.HN_MOD ");
//            //        aWriter.WriteLine("		GROUP BY ST_RID, shw.TIME_ID");
//            //        aWriter.WriteLine("		OPTION (MAXDOP 1)");
//            //        aWriter.WriteLine("	RETURN 0");
//            //        aWriter.WriteLine("	END");
//            //        aWriter.WriteLine("-- Process variables");
//            //        aWriter.WriteLine("-- GET ALL THE ROWS");
//            //        aWriter.WriteLine("IF @Rollup = 'Y'");
//            //        count = 0;
//            //        line = "       SELECT shw.HN_RID, 1 AS FV_RID, shw.ST_RID, ";
//            //        // add variables
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "COALESCE(SUM(shw." + vp.DatabaseColumnName + "), 0) " + vp.DatabaseColumnName + ", ";
//            //            if (line.Length > 90)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;

//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        // add locks
//            //        count = 0;
//            //        line = _indent10;
//            //        // add variables
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "0 AS " + vp.DatabaseColumnName + Include.cLockExtension;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            if (line.Length > 90)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;

//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        aWriter.WriteLine("        FROM " + tempTableName + " xml");
//            //        aWriter.WriteLine("	JOIN " + aViewName + " shw (NOLOCK) ON xml.HN_RID = shw.HN_RID");
//            //        aWriter.WriteLine("		AND shw.TIME_ID = xml.TIME_ID");
//            //        aWriter.WriteLine("		AND shw.HN_MOD = xml.HN_MOD");
//            //        aWriter.WriteLine("	GROUP BY shw.HN_RID, shw.ST_RID");
//            //        aWriter.WriteLine("	ORDER BY shw.ST_RID");
//            //        aWriter.WriteLine("	OPTION (MAXDOP 1)");
//            //        aWriter.WriteLine("ELSE");
//            //        count = 0;
//            //        line = "       SELECT shw.HN_RID, 1 AS FV_RID, shw.TIME_ID, shw.ST_RID, ";
//            //        // add variables
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "COALESCE(shw." + vp.DatabaseColumnName + ", 0) " + vp.DatabaseColumnName + ", ";
//            //            if (line.Length > 90)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;

//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        // add locks
//            //        count = 0;
//            //        line = _indent10;
//            //        // add variables
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "0 AS " + vp.DatabaseColumnName + Include.cLockExtension;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            if (line.Length > 90)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;

//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        aWriter.WriteLine("		FROM " + tempTableName + " xml");
//            //        aWriter.WriteLine("	JOIN " + tableName + " shw (NOLOCK) ON xml.HN_RID = shw.HN_RID AND shw.HN_MOD = xml.HN_MOD");
//            //        aWriter.WriteLine("		AND shw.TIME_ID = xml.TIME_ID");
//            //        aWriter.WriteLine("		AND shw.HN_MOD = xml.HN_MOD");
//            //        aWriter.WriteLine("	OPTION (MAXDOP 1)");

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "2')) > 0 DROP TABLE " + aTempTableName + "2");
//            //        aWriter.WriteLine(System.Environment.NewLine);

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}

//            //override public void Generate_SP_MID_XML_ST_FOR_WK_READ(string aProcedureName, string aTableName,
//            //     string aViewName, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter,
//            //     int aTableNumber, string aTableCountColumn)
//            //{
//            //    try
//            //    {
//            //        int count = 0;
//            //        string line = string.Empty;
//            //        string procedureName = aProcedureName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//            //        string tableName = aTableName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());

//            //        if (aDatabaseVariables == null ||
//            //            aDatabaseVariables.Count == 0)
//            //        {
//            //            return;
//            //        }

//            //        string tempTableName = "@dt";


//            //        aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
//            //        aWriter.WriteLine("@dt " + Include.DBStoreWeeklyForecastReadType + " READONLY");
//            //        aWriter.WriteLine("AS");
//            //        aWriter.WriteLine("SET NOCOUNT ON");

//            //        count = 0;
//            //        line = "       SELECT sfw.HN_RID, sfw.FV_RID, sfw.TIME_ID, sfw.ST_RID, ";
//            //        // add variables
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "COALESCE(sfw." + vp.DatabaseColumnName + ", 0) " + vp.DatabaseColumnName;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            if (line.Length > 90)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;

//            //            }

//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        aWriter.WriteLine("		FROM " + tempTableName + " xml");
//            //        aWriter.WriteLine("	JOIN " + aViewName + " sfw ON xml.HN_MOD = sfw.HN_MOD AND xml.HN_RID = sfw.HN_RID");
//            //        aWriter.WriteLine("		AND sfw.FV_RID = xml.FV_RID");
//            //        aWriter.WriteLine("		AND sfw.TIME_ID = xml.TIME_ID");
//            //        aWriter.WriteLine("	OPTION (MAXDOP 1)");

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}

//            //override public void Generate_SP_MID_XML_ST_FOR_WK_LOCK_READ(string aProcedureName, string aTableName,
//            //    string aViewName, string aTableCountColumn, string aTempTableName,
//            //    ProfileList aDatabaseVariables, StreamWriter aWriter)
//            //{
//            //    int count = 0;
//            //    string line;
//            //    try
//            //    {
//            //        AddStoredProcedureDrop(aProcedureName, aWriter);
//            //        //AddTypeDrop(Include.DBStoreWeeklyForecastReadLockType, aWriter);

//            //        if (aDatabaseVariables == null ||
//            //            aDatabaseVariables.Count == 0)
//            //        {
//            //            return;
//            //        }

//            //        string tempTableName = "@dt";

//            //        //aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBStoreWeeklyForecastReadLockType + "] AS TABLE(");
//            //        //aWriter.WriteLine("  [HN_RID] [int],");
//            //        //aWriter.WriteLine("  [FV_RID] [int],");
//            //        //aWriter.WriteLine("  [TIME_ID] [int]");
//            //        //aWriter.WriteLine("PRIMARY KEY (HN_RID, FV_RID, TIME_ID)");
//            //        //aWriter.WriteLine(")");

//            //        //aWriter.WriteLine(System.Environment.NewLine);
//            //        //aWriter.WriteLine("GO");
//            //        //aWriter.WriteLine(System.Environment.NewLine);

//            //        aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
//            //        aWriter.WriteLine("@dt " + Include.DBStoreWeeklyForecastReadLockType + " READONLY");
//            //        aWriter.WriteLine("AS");
//            //        aWriter.WriteLine("SET NOCOUNT ON");

//            //        count = 0;
//            //        line = "       SELECT sfwl.HN_RID, sfwl.FV_RID, sfwl.TIME_ID, sfwl.ST_RID, ";
//            //        // add variables
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "COALESCE(sfwl." + vp.DatabaseColumnName + Include.cLockExtension + ", '0') " + vp.DatabaseColumnName + Include.cLockExtension;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            if (line.Length > 90)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;

//            //            }

//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        aWriter.WriteLine("		FROM " + tempTableName + " t");
//            //        aWriter.WriteLine("	JOIN " + aTableName + " sfwl ON t.HN_RID = sfwl.HN_RID");
//            //        aWriter.WriteLine("		AND sfwl.FV_RID = t.FV_RID");
//            //        aWriter.WriteLine("		AND sfwl.TIME_ID = t.TIME_ID");
//            //        aWriter.WriteLine("	OPTION (MAXDOP 1)");

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}

//            override public void Generate_SP_MID_XML_ST_HIS_WK_ROLL_READ(string aProcedureName, string aTableName,
//                string aViewName, int aTableNumber, string aTableCountColumn, string aTempTableName,
//                ProfileList aDatabaseVariables, StreamWriter aWriter)
//            {
//                try
//                {
//                    string procedureName = aProcedureName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//                    string tableName = aTableName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());

//                    AddStoredProcedureDrop(procedureName, aWriter);

//                    if (aDatabaseVariables == null ||
//                        aDatabaseVariables.Count == 0)
//                    {
//                        return;
//                    }

//                    aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
//                    aWriter.WriteLine("@xmlDoc AS NTEXT,");
//                    aWriter.WriteLine("@Rollup CHAR(1) = NULL");
//                    aWriter.WriteLine("AS");
//                    aWriter.WriteLine("SET NOCOUNT ON");
//                    aWriter.WriteLine("DECLARE @idoc int,");
//                    aWriter.WriteLine("        @Tables INT,");
//                    aWriter.WriteLine("        @Loop INT,");
//                    aWriter.WriteLine("        @HN_RID INT,");
//                    aWriter.WriteLine("        @HN_MOD INT,");
//                    aWriter.WriteLine("        @LoopCount INT,");
//                    aWriter.WriteLine("        @NextLoopCount INT");
//                    aWriter.WriteLine("EXEC sp_xml_preparedocument @idoc OUTPUT, @xmlDoc");
//                    aWriter.WriteLine("SELECT @Tables = " + aTableCountColumn + " FROM SYSTEM_OPTIONS");

//                    aWriter.WriteLine("SELECT xmlHN_RID, xmlTIME_ID, xmlHN_RID % @Tables AS xmlHN_MOD");
//                    aWriter.WriteLine("	INTO " + aTempTableName);
//                    aWriter.WriteLine("	FROM OPENXML (@idoc, '/root/node/time',2)");
//                    aWriter.WriteLine("         WITH (  xmlHN_RID  int      '../@ID',");
//                    aWriter.WriteLine("                 xmlTIME_ID INT '@ID') xml");
//                    aWriter.WriteLine("   SET @LoopCount = 0");
//                    aWriter.WriteLine("   CREATE TABLE #TREE (LOOPCOUNT INT NOT NULL, PARENT_HN_RID INT NOT NULL, HOME_PH_RID INT, PH_TYPE INT, CHILD_HN_RID INT NOT NULL, CHILD_HN_MOD INT NOT NULL)");
//                    aWriter.WriteLine("   -- insert the children of the node into the temp table");
//                    aWriter.WriteLine("   INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, CHILD_HN_RID, CHILD_HN_MOD) ");
//                    aWriter.WriteLine("       select @LoopCount as LOOPCOUNT, @HN_RID as PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
//                    aWriter.WriteLine("         from HIER_NODE_JOIN hnj (NOLOCK)");
//                    aWriter.WriteLine("           JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//                    aWriter.WriteLine("           JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//                    aWriter.WriteLine("       where @HN_RID = hnj.PARENT_HN_RID");
//                    aWriter.WriteLine("   SET @Loop = @@ROWCOUNT");
//                    aWriter.WriteLine("   -- chase all paths until you get the main hierarchy (type 800000) or the lowest leaf");
//                    aWriter.WriteLine("   SET @NextLoopCount = @LoopCount + 1");
//                    aWriter.WriteLine("   WHILE @Loop > 0");
//                    aWriter.WriteLine("   BEGIN");
//                    aWriter.WriteLine("      INSERT #TREE");
//                    aWriter.WriteLine("        select @NextLoopCount as LOOPCOUNT, hnj.PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
//                    aWriter.WriteLine("          from HIER_NODE_JOIN hnj (NOLOCK)");
//                    aWriter.WriteLine("            JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//                    aWriter.WriteLine("            JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//                    aWriter.WriteLine("            JOIN #TREE t ON hnj.PARENT_HN_RID = t.CHILD_HN_RID");
//                    aWriter.WriteLine("        WHERE t.LOOPCOUNT =  @LoopCount AND t.PH_TYPE <> 800000");
//                    aWriter.WriteLine("      SET @Loop = @@ROWCOUNT");
//                    aWriter.WriteLine("      SET @LoopCount = @LoopCount + 1");
//                    aWriter.WriteLine("      SET @NextLoopCount = @LoopCount + 1");
//                    aWriter.WriteLine("   END");
//                    aWriter.WriteLine("   -- join with dates from xml selecting only nodes from the main hierarchy or lowest leaf alternates");
//                    aWriter.WriteLine("   SELECT * ");
//                    aWriter.WriteLine("     INTO " + aTempTableName + "2");
//                    aWriter.WriteLine("     FROM #TREE");
//                    aWriter.WriteLine("     CROSS JOIN " + aTempTableName);
//                    aWriter.WriteLine("   where LOOPCOUNT = @LoopCount - 1");
//                    aWriter.WriteLine("	     or PH_TYPE = 800000");

//                    int count = 0;
//                    string line = "       SELECT @HN_RID AS HN_RID, 1 AS FV_RID, ST_RID, shw.TIME_ID, ";
//                    // add variables
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        line += "COALESCE(SUM(" + vp.DatabaseColumnName + "), 0) " + vp.DatabaseColumnName + ", ";
//                        if (line.Length > 100)
//                        {
//                            aWriter.WriteLine(line);
//                            line = _indent10;

//                        }
						
//                    }
//                    if (line.Length > _indent10.Length)
//                    {
//                        aWriter.WriteLine(line);

//                    }
//                    // add locks
//                    count = 0;
//                    line = _indent10;
//                    // add variables
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        line += "0 AS " + vp.DatabaseColumnName + Include.cLockExtension;
//                        if (count < aDatabaseVariables.Count)
//                        {
//                            line += ", ";
//                        }
//                        if (line.Length > 100)
//                        {
//                            aWriter.WriteLine(line);
//                            line = _indent10;

//                        }
						
//                    }
//                    if (line.Length > _indent10.Length)
//                    {
//                        aWriter.WriteLine(line);

//                    }
//                    aWriter.WriteLine("		FROM " + aTempTableName + "2 t");
//                    aWriter.WriteLine("		JOIN " + aViewName + " shw (NOLOCK) ON t.CHILD_HN_RID = shw.HN_RID AND shw.ST_RID > 0");
//                    aWriter.WriteLine("			AND shw.TIME_ID = t.TIME_ID AND t.CHILD_HN_MOD = shw.HN_MOD ");
//                    aWriter.WriteLine("		GROUP BY ST_RID, shw.TIME_ID");
//                    aWriter.WriteLine("		OPTION (MAXDOP 1)");
//                    aWriter.WriteLine("	RETURN 0");
					
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            //override public void Generate_SP_MID_XML_CHN_HIS_WK_READ(string aProcedureName, string aTableName, 
//            //    string aViewName, string aTableCountColumn, string aTempTableName,
//            //    ProfileList aDatabaseVariables, StreamWriter aWriter)
//            //{
//            //    try
//            //    {
//            //        AddStoredProcedureDrop(aProcedureName, aWriter);
//            //        AddTypeDrop(Include.DBChainWeeklyHistoryReadType, aWriter);

//            //        if (aDatabaseVariables == null ||
//            //            aDatabaseVariables.Count == 0)
//            //        {
//            //            return;
//            //        }

//            //        string tempTableName = "@dt";

//            //        //aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBChainWeeklyHistoryReadType + "] AS TABLE(");
//            //        //aWriter.WriteLine("  [HN_RID] [int],");
//            //        //aWriter.WriteLine("  [TIME_ID] [int]");
//            //        //aWriter.WriteLine("PRIMARY KEY (HN_RID, TIME_ID)");
//            //        //aWriter.WriteLine(")");

//            //        //aWriter.WriteLine(System.Environment.NewLine);
//            //        //aWriter.WriteLine("GO");
//            //        //aWriter.WriteLine(System.Environment.NewLine);

//            //        aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
//            //        aWriter.WriteLine("@dt " +Include.DBChainWeeklyHistoryReadType + " READONLY,");
//            //        aWriter.WriteLine("@Rollup CHAR(1) = NULL");
//            //        aWriter.WriteLine("AS");
//            //        aWriter.WriteLine("SET NOCOUNT ON");
//            //        aWriter.WriteLine("DECLARE @idoc int,");
//            //        aWriter.WriteLine("        @Tables INT,");
//            //        aWriter.WriteLine("        @Loop INT,");
//            //        aWriter.WriteLine("        @HN_TYPE INT,");
//            //        aWriter.WriteLine("        @HN_RID INT,");
//            //        aWriter.WriteLine("        @HN_MOD INT,");
//            //        aWriter.WriteLine("        @ROLL_OPTION INT,");
//            //        aWriter.WriteLine("        @LoopCount INT,");
//            //        aWriter.WriteLine("        @NextLoopCount INT");
//            //        aWriter.WriteLine("SELECT @Tables = " + aTableCountColumn + " FROM SYSTEM_OPTIONS");

//            //        aWriter.WriteLine("SELECT DISTINCT @HN_TYPE = ph.PH_TYPE, @HN_RID = t.HN_RID, @HN_MOD = t.HN_RID % @Tables,");
//            //        aWriter.WriteLine("                @ROLL_OPTION = COALESCE(ph.HISTORY_ROLL_OPTION, 800251)");
//            //        aWriter.WriteLine("        FROM HIERARCHY_NODE hn (NOLOCK)");
//            //        aWriter.WriteLine("        JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//            //        aWriter.WriteLine("        JOIN " + tempTableName + " t on t.HN_RID = hn.HN_RID");
//            //        aWriter.WriteLine("IF @HN_TYPE = 800001 and @ROLL_OPTION = 800251 -- Realtime");
//            //        aWriter.WriteLine("   BEGIN");
//            //        aWriter.WriteLine("   SET @LoopCount = 0");
//            //        aWriter.WriteLine("   CREATE TABLE #TREE (LOOPCOUNT INT NOT NULL, PARENT_HN_RID INT NOT NULL, HOME_PH_RID INT, PH_TYPE INT, CHILD_HN_RID INT NOT NULL, CHILD_HN_MOD INT NOT NULL)");
//            //        aWriter.WriteLine("   -- insert the children of the node into the temp table");
//            //        aWriter.WriteLine("   INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, CHILD_HN_RID, CHILD_HN_MOD) ");
//            //        aWriter.WriteLine("       select @LoopCount as LOOPCOUNT, @HN_RID as PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
//            //        aWriter.WriteLine("         from HIER_NODE_JOIN hnj (NOLOCK)");
//            //        aWriter.WriteLine("           JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//            //        aWriter.WriteLine("           JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//            //        aWriter.WriteLine("       where @HN_RID = hnj.PARENT_HN_RID");
//            //        aWriter.WriteLine("   SET @Loop = @@ROWCOUNT");
//            //        aWriter.WriteLine("   -- chase all paths until you get the main hierarchy (type 800000) or the lowest leaf");
//            //        aWriter.WriteLine("   SET @NextLoopCount = @LoopCount + 1");
//            //        aWriter.WriteLine("   WHILE @Loop > 0");
//            //        aWriter.WriteLine("   BEGIN");
//            //        aWriter.WriteLine("      INSERT #TREE");
//            //        aWriter.WriteLine("        select @NextLoopCount as LOOPCOUNT, hnj.PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
//            //        aWriter.WriteLine("          from HIER_NODE_JOIN hnj (NOLOCK)");
//            //        aWriter.WriteLine("            JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//            //        aWriter.WriteLine("            JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//            //        aWriter.WriteLine("            JOIN #TREE t ON hnj.PARENT_HN_RID = t.CHILD_HN_RID");
//            //        aWriter.WriteLine("        WHERE t.LOOPCOUNT =  @LoopCount AND t.PH_TYPE <> 800000");
//            //        aWriter.WriteLine("      SET @Loop = @@ROWCOUNT");
//            //        aWriter.WriteLine("      SET @LoopCount = @LoopCount + 1");
//            //        aWriter.WriteLine("      SET @NextLoopCount = @LoopCount + 1");
//            //        aWriter.WriteLine("   END");
//            //        aWriter.WriteLine("   -- join with dates from xml selecting only nodes from the main hierarchy or lowest leaf alternates");
//            //        aWriter.WriteLine("   SELECT * ");
//            //        aWriter.WriteLine("     INTO " + aTempTableName + "2");
//            //        aWriter.WriteLine("     FROM #TREE");
//            //        aWriter.WriteLine("     CROSS JOIN " + tempTableName);
//            //        aWriter.WriteLine("   where LOOPCOUNT = @LoopCount - 1");
//            //        aWriter.WriteLine("	     or PH_TYPE = 800000");

//            //        int count = 0;
//            //        string line = "       SELECT @HN_RID AS HN_RID, 1 AS FV_RID, chw.TIME_ID, ";
//            //        // add variables
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "COALESCE(SUM(" + vp.DatabaseColumnName + "), 0) " + vp.DatabaseColumnName + ", ";
//            //            if (line.Length > 100)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;

//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        // add locks
//            //        count = 0;
//            //        line = _indent10;
//            //        // add variables
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "0 AS " + vp.DatabaseColumnName + Include.cLockExtension;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            if (line.Length > 100)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;

//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        aWriter.WriteLine("		FROM " + aTempTableName + "2 t");
//            //        aWriter.WriteLine("		JOIN " + aViewName + " chw (NOLOCK) ON t.CHILD_HN_RID = chw.HN_RID");
//            //        aWriter.WriteLine("			AND chw.TIME_ID = t.TIME_ID");
//            //        aWriter.WriteLine("		GROUP BY chw.TIME_ID");
//            //        aWriter.WriteLine("		OPTION (MAXDOP 1)");
//            //        aWriter.WriteLine("	RETURN 0");
//            //        aWriter.WriteLine("	END");
//            //        aWriter.WriteLine("-- Process variables");
//            //        aWriter.WriteLine("-- GET ALL THE ROWS");
//            //        aWriter.WriteLine("IF @Rollup = 'Y'");
//            //        count = 0;
//            //        line = "       SELECT chw.HN_RID, 1 AS FV_RID, ";
//            //        // add variables
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "COALESCE(SUM(chw." + vp.DatabaseColumnName + "), 0) " + vp.DatabaseColumnName + ", ";
//            //            if (line.Length > 90)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;

//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        // add locks
//            //        count = 0;
//            //        line = _indent10;
//            //        // add variables
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "0 AS " + vp.DatabaseColumnName + Include.cLockExtension;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            if (line.Length > 90)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;

//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        aWriter.WriteLine("        FROM " + tempTableName + " xml");
//            //        aWriter.WriteLine("	JOIN " + aViewName + " chw ON xml.HN_RID = chw.HN_RID");
//            //        aWriter.WriteLine("		AND chw.TIME_ID = xml.TIME_ID");
//            //        aWriter.WriteLine("	GROUP BY chw.HN_RID");
//            //        aWriter.WriteLine("	OPTION (MAXDOP 1)");
//            //        aWriter.WriteLine("ELSE");
//            //        count = 0;
//            //        line = "       SELECT chw.HN_RID, 1 AS FV_RID, chw.TIME_ID, ";
//            //        // add variables
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "COALESCE(chw." + vp.DatabaseColumnName + ", 0) " + vp.DatabaseColumnName + ", ";
//            //            if (line.Length > 90)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;

//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        // add locks
//            //        count = 0;
//            //        line = _indent10;
//            //        // add variables
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "0 AS " + vp.DatabaseColumnName + Include.cLockExtension;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            if (line.Length > 90)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;
//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        aWriter.WriteLine("		FROM " + tempTableName + " xml");
//            //        aWriter.WriteLine("	JOIN " + aViewName + " chw ON xml.HN_RID = chw.HN_RID");
//            //        aWriter.WriteLine("		AND chw.TIME_ID = xml.TIME_ID");
//            //        aWriter.WriteLine("	OPTION (MAXDOP 1)");

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        //aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "')) > 0 DROP TABLE " + aTempTableName);
//            //        aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "2')) > 0 DROP TABLE " + aTempTableName + "2");
//            //        aWriter.WriteLine(System.Environment.NewLine);

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}

//            //override public void Generate_SP_MID_XML_CHN_FOR_WK_READ(string aProcedureName, string aTableName,
//            //    string aViewName, string aTableCountColumn, string aTempTableName,
//            //    ProfileList aDatabaseVariables, StreamWriter aWriter)
//            //{
//            //    int count = 0;
//            //    string line;
//            //    try
//            //    {
//            //        AddStoredProcedureDrop(aProcedureName, aWriter);
//            //        //AddTypeDrop(Include.DBChainWeeklyForecastReadType, aWriter);

//            //        if (aDatabaseVariables == null ||
//            //            aDatabaseVariables.Count == 0)
//            //        {
//            //            return;
//            //        }

//            //        string tempTableName = "@dt";

//            //        //aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBChainWeeklyForecastReadType + "] AS TABLE(");
//            //        //aWriter.WriteLine("  [HN_RID] [int],");
//            //        //aWriter.WriteLine("  [FV_RID] [int],");
//            //        //aWriter.WriteLine("  [TIME_ID] [int]");
//            //        //aWriter.WriteLine("PRIMARY KEY (HN_RID, FV_RID, TIME_ID)");
//            //        //aWriter.WriteLine(")");

//            //        //aWriter.WriteLine(System.Environment.NewLine);
//            //        //aWriter.WriteLine("GO");
//            //        //aWriter.WriteLine(System.Environment.NewLine);

//            //        aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
//            //        aWriter.WriteLine("@dt " + Include.DBChainWeeklyForecastReadType + " READONLY");
//            //        aWriter.WriteLine("AS");
//            //        aWriter.WriteLine("SET NOCOUNT ON");
                    
//            //        count = 0;
//            //        line = "       SELECT cfw.HN_RID, cfw.FV_RID, cfw.TIME_ID, ";
//            //        // add variables
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "COALESCE(cfw." + vp.DatabaseColumnName + ", 0) " + vp.DatabaseColumnName;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            if (line.Length > 90)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;

//            //            }

//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        aWriter.WriteLine("		FROM " + tempTableName + " xml");
//            //        aWriter.WriteLine("	JOIN " + aViewName + " cfw ON xml.HN_RID = cfw.HN_RID");
//            //        aWriter.WriteLine("		AND cfw.FV_RID = xml.FV_RID");
//            //        aWriter.WriteLine("		AND cfw.TIME_ID = xml.TIME_ID");
//            //        aWriter.WriteLine("	OPTION (MAXDOP 1)");

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}

//            //override public void Generate_SP_MID_XML_CHN_FOR_WK_LOCK_READ(string aProcedureName, string aTableName,
//            //    string aViewName, string aTableCountColumn, string aTempTableName,
//            //    ProfileList aDatabaseVariables, StreamWriter aWriter)
//            //{
//            //    int count = 0;
//            //    string line;
//            //    try
//            //    {
//            //        AddStoredProcedureDrop(aProcedureName, aWriter);
//            //        //AddTypeDrop(Include.DBChainWeeklyForecastReadLockType, aWriter);

//            //        if (aDatabaseVariables == null ||
//            //            aDatabaseVariables.Count == 0)
//            //        {
//            //            return;
//            //        }

//            //        string tempTableName = "@dt";

//            //        //aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBChainWeeklyForecastReadLockType + "] AS TABLE(");
//            //        //aWriter.WriteLine("  [HN_RID] [int],");
//            //        //aWriter.WriteLine("  [FV_RID] [int],");
//            //        //aWriter.WriteLine("  [TIME_ID] [int]");
//            //        //aWriter.WriteLine("PRIMARY KEY (HN_RID, FV_RID, TIME_ID)");
//            //        //aWriter.WriteLine(")");

//            //        //aWriter.WriteLine(System.Environment.NewLine);
//            //        //aWriter.WriteLine("GO");
//            //        //aWriter.WriteLine(System.Environment.NewLine);

//            //        aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
//            //        aWriter.WriteLine("@dt " + Include.DBChainWeeklyForecastReadLockType + " READONLY");
//            //        aWriter.WriteLine("AS");
//            //        aWriter.WriteLine("SET NOCOUNT ON");

//            //        count = 0;
//            //        line = "       SELECT cfwl.HN_RID, cfwl.FV_RID, cfwl.TIME_ID, ";
//            //        // add variables
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "COALESCE(cfwl." + vp.DatabaseColumnName + Include.cLockExtension + ", '0') " + vp.DatabaseColumnName + Include.cLockExtension;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            if (line.Length > 90)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;

//            //            }

//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);

//            //        }
//            //        aWriter.WriteLine("		FROM " + tempTableName + " t");
//            //        aWriter.WriteLine("	JOIN " + aTableName + " cfwl ON t.HN_RID = cfwl.HN_RID");
//            //        aWriter.WriteLine("		AND cfwl.FV_RID = t.FV_RID");
//            //        aWriter.WriteLine("		AND cfwl.TIME_ID = t.TIME_ID");
//            //        aWriter.WriteLine("	OPTION (MAXDOP 1)");

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}
            
//            override public void Generate_SP_MID_XML_CHN_HIS_WK_ROLL_READ(string aProcedureName, string aTableName, 
//                string aViewName, string aTempTableName, ProfileList aDatabaseVariables, StreamWriter aWriter)
//            {
//                try
//                {
//                    string procedureName = aProcedureName;
//                    string tableName = aTableName;

//                    AddStoredProcedureDrop(aProcedureName, aWriter);

//                    if (aDatabaseVariables == null ||
//                        aDatabaseVariables.Count == 0)
//                    {
//                        return;
//                    }

//                    aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
//                    aWriter.WriteLine("@xmlDoc AS NTEXT,");
//                    aWriter.WriteLine("@Rollup CHAR(1) = NULL");
//                    aWriter.WriteLine("AS");
//                    aWriter.WriteLine("SET NOCOUNT ON");
//                    aWriter.WriteLine("DECLARE @idoc int,");
//                    aWriter.WriteLine("        @Loop INT,");
//                    aWriter.WriteLine("        @HN_TYPE INT,");
//                    aWriter.WriteLine("        @HN_RID INT,");
//                    aWriter.WriteLine("        @HN_MOD INT,");
//                    aWriter.WriteLine("        @Tables INT,");
//                    aWriter.WriteLine("        @ROLL_OPTION INT,");
//                    aWriter.WriteLine("        @LoopCount INT,");
//                    aWriter.WriteLine("        @NextLoopCount INT");
//                    aWriter.WriteLine("EXEC sp_xml_preparedocument @idoc OUTPUT, @xmlDoc");
//                    aWriter.WriteLine("SELECT @Tables = 1");
//                    aWriter.WriteLine("SELECT xmlHN_RID, xmlTIME_ID");
//                    aWriter.WriteLine("	INTO " + aTempTableName);
//                    aWriter.WriteLine("	FROM OPENXML (@idoc, '/root/node/time',2)");
//                    aWriter.WriteLine("         WITH (  xmlHN_RID  int      '../@ID',");
//                    aWriter.WriteLine("                 xmlTIME_ID INT '@ID') xml");
//                    aWriter.WriteLine("   SET @LoopCount = 0");
//                    aWriter.WriteLine("   CREATE TABLE #TREE (LOOPCOUNT INT NOT NULL, PARENT_HN_RID INT NOT NULL, HOME_PH_RID INT, PH_TYPE INT, CHILD_HN_RID INT NOT NULL, CHILD_HN_MOD INT NOT NULL)");
//                    aWriter.WriteLine("   -- insert the children of the node into the temp table");
//                    aWriter.WriteLine("   INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, CHILD_HN_RID, CHILD_HN_MOD) ");
//                    aWriter.WriteLine("       select @LoopCount as LOOPCOUNT, @HN_RID as PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
//                    aWriter.WriteLine("         from HIER_NODE_JOIN hnj (NOLOCK)");
//                    aWriter.WriteLine("           JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//                    aWriter.WriteLine("           JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//                    aWriter.WriteLine("       where @HN_RID = hnj.PARENT_HN_RID");
//                    aWriter.WriteLine("   SET @Loop = @@ROWCOUNT");
//                    aWriter.WriteLine("   -- chase all paths until you get the main hierarchy (type 800000) or the lowest leaf");
//                    aWriter.WriteLine("   SET @NextLoopCount = @LoopCount + 1");
//                    aWriter.WriteLine("   WHILE @Loop > 0");
//                    aWriter.WriteLine("   BEGIN");
//                    aWriter.WriteLine("      INSERT #TREE");
//                    aWriter.WriteLine("        select @NextLoopCount as LOOPCOUNT, hnj.PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
//                    aWriter.WriteLine("          from HIER_NODE_JOIN hnj (NOLOCK)");
//                    aWriter.WriteLine("            JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//                    aWriter.WriteLine("            JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//                    aWriter.WriteLine("            JOIN #TREE t ON hnj.PARENT_HN_RID = t.CHILD_HN_RID");
//                    aWriter.WriteLine("        WHERE t.LOOPCOUNT =  @LoopCount AND t.PH_TYPE <> 800000");
//                    aWriter.WriteLine("      SET @Loop = @@ROWCOUNT");
//                    aWriter.WriteLine("      SET @LoopCount = @LoopCount + 1");
//                    aWriter.WriteLine("      SET @NextLoopCount = @LoopCount + 1");
//                    aWriter.WriteLine("   END");
//                    aWriter.WriteLine("   -- join with dates from xml selecting only nodes from the main hierarchy or lowest leaf alternates");
//                    aWriter.WriteLine("   SELECT * ");
//                    aWriter.WriteLine("     INTO " + aTempTableName + "2");
//                    aWriter.WriteLine("     FROM #TREE");
//                    aWriter.WriteLine("     CROSS JOIN " + aTempTableName);
//                    aWriter.WriteLine("   where LOOPCOUNT = @LoopCount - 1");
//                    aWriter.WriteLine("	     or PH_TYPE = 800000");

//                    int count = 0;
//                    string line = "       SELECT @HN_RID AS HN_RID, 1 AS FV_RID, chw.TIME_ID, ";
//                    // add variables
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        line += "COALESCE(SUM(" + vp.DatabaseColumnName + "), 0) " + vp.DatabaseColumnName + ", ";
//                        if (line.Length > 100)
//                        {
//                            aWriter.WriteLine(line);
//                            line = _indent10;

//                        }
						
//                    }
//                    if (line.Length > _indent10.Length)
//                    {
//                        aWriter.WriteLine(line);

//                    }
//                    // add locks
//                    count = 0;
//                    line = _indent10;
//                    // add variables
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        line += "0 AS " + vp.DatabaseColumnName + Include.cLockExtension;
//                        if (count < aDatabaseVariables.Count)
//                        {
//                            line += ", ";
//                        }
//                        if (line.Length > 100)
//                        {
//                            aWriter.WriteLine(line);
//                            line = _indent10;

//                        }
						
//                    }
//                    if (line.Length > _indent10.Length)
//                    {
//                        aWriter.WriteLine(line);

//                    }
//                    aWriter.WriteLine("		FROM " + aTempTableName + "2 t");
//                    aWriter.WriteLine("		JOIN " + aViewName + " chw (NOLOCK) ON t.CHILD_HN_RID = chw.HN_RID");
//                    aWriter.WriteLine("			AND chw.TIME_ID = t.TIME_ID");
//                    aWriter.WriteLine("		GROUP BY chw.TIME_ID");
//                    aWriter.WriteLine("		OPTION (MAXDOP 1)");
//                    aWriter.WriteLine("	RETURN 0");
					

//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "')) > 0 DROP TABLE " + aTempTableName);
//                    aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "2')) > 0 DROP TABLE " + aTempTableName + "2");
//                    aWriter.WriteLine(System.Environment.NewLine);

//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            //override public void Generate_SP_MID_XML_ST_MOD_WK_READ(string aProcedureName, 
//            //    string aForecastTableName, string aHistoryTableName, string aLockTableName, 
//            //    string aForecastViewName, string aHistoryViewName, string aTempTableName, 
//            //    ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableNumber, string aTableCountColumn)
//            //{
//            //    try
//            //    {
//            //        string procedureName = aProcedureName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//            //        string forecastTableName = aForecastTableName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//            //        string historyTableName = aHistoryTableName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//            //        string lockTableName = aLockTableName;

//            //        if (aDatabaseVariables == null ||
//            //            aDatabaseVariables.Count == 0)
//            //        {
//            //            return;
//            //        }
//            //        string line = string.Empty;
//            //        int count = 0;
//            //        string tempTableName = "@dt";

//            //        aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
//            //        aWriter.WriteLine("@dt " + Include.DBStoreWeeklyModifiedReadType + " READONLY,");
//            //        aWriter.WriteLine("@Rollup CHAR(1) = NULL");
//            //        aWriter.WriteLine("AS");
//            //        aWriter.WriteLine("SET NOCOUNT ON");
//            //        aWriter.WriteLine("DECLARE @Tables INT,");
//            //        aWriter.WriteLine("        @Loop INT,");
//            //        aWriter.WriteLine("        @HN_TYPE INT,");
//            //        aWriter.WriteLine("        @HN_RID INT,");
//            //        aWriter.WriteLine("        @FV_RID INT,");
//            //        aWriter.WriteLine("        @HN_MOD INT,");
//            //        aWriter.WriteLine("        @ROLL_OPTION INT,");
//            //        aWriter.WriteLine("        @LoopCount INT,");
//            //        aWriter.WriteLine("        @NextLoopCount INT");
//            //        aWriter.WriteLine("SELECT @Tables = " + aTableCountColumn + " FROM SYSTEM_OPTIONS");

//            //        aWriter.WriteLine("SELECT DISTINCT @HN_TYPE = ph.PH_TYPE, @HN_RID = t.HN_RID, @HN_MOD = t.HN_RID % @Tables,");
//            //        aWriter.WriteLine("                @ROLL_OPTION = COALESCE(ph.HISTORY_ROLL_OPTION, 800251), @FV_RID = t.FV_RID");
//            //        aWriter.WriteLine("        FROM HIERARCHY_NODE hn (NOLOCK)");
//            //        aWriter.WriteLine("        JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//            //        aWriter.WriteLine("        JOIN " + tempTableName + " t on t.HN_RID = hn.HN_RID");
			
//            //        aWriter.WriteLine(" ");
//            //        aWriter.WriteLine(" -- build temp table of values and locks for modified version");
//            //        aWriter.WriteLine(" create table " + aTempTableName + "MOD ");
//            //        aWriter.WriteLine(" (HN_RID  int not null,");
//            //        aWriter.WriteLine("  FV_RID  int not null,");
//            //        aWriter.WriteLine("  TIME_ID int not null,");
//            //        aWriter.WriteLine("  ST_RID int not null,");
//            //        count = 0;
//            //        // add variables
//            //        line = "  ";
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            line += "  " + vp.DatabaseColumnName;
//            //            switch (vp.StoreDatabaseVariableType)
//            //            {
//            //                case eVariableDatabaseType.Integer:
//            //                    line += " int ";
//            //                    break;
//            //                case eVariableDatabaseType.Real:
//            //                    line += " real ";
//            //                    break;
//            //                case eVariableDatabaseType.Float:
//            //                    line += " float ";
//            //                    break;
//            //                case eVariableDatabaseType.BigInteger:
//            //                    line += " bigint ";
//            //                    break;
//            //            }
//            //            line += "  null,";
//            //            aWriter.WriteLine(line);
//            //            line = "  ";
//            //        }
//            //        count = 0;
//            //        // add locks
//            //        line = "  ";
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "  " + vp.DatabaseColumnName + Include.cLockExtension + "  char(1) null";
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            else
//            //            {
//            //                line += ")";
//            //            }
//            //            aWriter.WriteLine(line);
//            //            line = "  ";
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);
//            //        }

//            //        aWriter.WriteLine(" ");
//            //        aWriter.WriteLine(" insert into " + aTempTableName + "MOD");
//            //        aWriter.WriteLine("  select sfw.HN_RID, sfw.FV_RID, sfw.TIME_ID, sfw.ST_RID,");
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            line += "  sfw." + vp.DatabaseColumnName + ", ";
//            //            if (line.Length > 80)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = "  ";
//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);
//            //        }
//            //        count = 0;
//            //        line = "  ";
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "  COALESCE(sfwl." + vp.DatabaseColumnName + Include.cLockExtension + ", 0) " + vp.DatabaseColumnName + Include.cLockExtension;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            aWriter.WriteLine(line);
//            //            line = "  ";
						
//            //        }

//            //        aWriter.WriteLine("   FROM " + tempTableName + " t");
//            //        aWriter.WriteLine("   JOIN " + aForecastViewName + " sfw (NOLOCK)");
//            //        aWriter.WriteLine("    on sfw.HN_RID = t.HN_RID");
//            //        aWriter.WriteLine("    and sfw.FV_RID = t.FV_RID");
//            //        aWriter.WriteLine("    and sfw.TIME_ID = t.TIME_ID");
//            //        aWriter.WriteLine("   left outer join " + aLockTableName + " sfwl (NOLOCK)");
//            //        aWriter.WriteLine("    on sfwl.HN_RID = sfw.HN_RID");
//            //        aWriter.WriteLine("    and sfwl.FV_RID =sfw.FV_RID");
//            //        aWriter.WriteLine("    and sfwl.TIME_ID = sfw.TIME_ID");
//            //        aWriter.WriteLine("    and sfwl.ST_RID = sfw.ST_RID");
//            //        aWriter.WriteLine("  union");
//            //        aWriter.WriteLine("  select sfwl.HN_RID, sfwl.FV_RID, sfwl.TIME_ID, sfwl.ST_RID, ");
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            line += "  sfw." + vp.DatabaseColumnName + ", ";
//            //            if (line.Length > 80)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = "  ";
//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);
//            //        }
//            //        count = 0;
//            //        line = "  ";
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "  COALESCE(sfwl." + vp.DatabaseColumnName + Include.cLockExtension + ", 0) " + vp.DatabaseColumnName + Include.cLockExtension;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            aWriter.WriteLine(line);
//            //            line = "  ";
						
//            //        }

//            //        aWriter.WriteLine(" FROM " + tempTableName + " t");
//            //        aWriter.WriteLine(" JOIN " + aLockTableName + " sfwl (NOLOCK)");
//            //        aWriter.WriteLine("  on sfwl.HN_RID = t.HN_RID");
//            //        aWriter.WriteLine("  and sfwl.FV_RID = t.FV_RID");
//            //        aWriter.WriteLine("  and sfwl.TIME_ID = t.TIME_ID");
//            //        aWriter.WriteLine(" left outer join " + aForecastViewName + " sfw (NOLOCK)");
//            //        aWriter.WriteLine("  on sfw.HN_RID = sfwl.HN_RID");
//            //        aWriter.WriteLine("  and sfw.FV_RID =sfwl.FV_RID");
//            //        aWriter.WriteLine("  and sfw.TIME_ID = sfwl.TIME_ID");
//            //        aWriter.WriteLine("  and sfw.ST_RID = sfwl.ST_RID");

//            //        aWriter.WriteLine(" ");
//            //        aWriter.WriteLine(" -- create temp table for history values");
//            //        aWriter.WriteLine(" create table " + aTempTableName + "HISTORY ");
//            //        aWriter.WriteLine(" (HN_RID  int not null,");
//            //        aWriter.WriteLine("  FV_RID  int not null,");
//            //        aWriter.WriteLine("  TIME_ID int not null,");
//            //        aWriter.WriteLine("  ST_RID int not null,");
//            //        count = 0;
//            //        // add variables
//            //        line = "  ";
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "  " + vp.DatabaseColumnName;
//            //            switch (vp.StoreDatabaseVariableType)
//            //            {
//            //                case eVariableDatabaseType.Integer:
//            //                    line += " int ";
//            //                    break;
//            //                case eVariableDatabaseType.Real:
//            //                    line += " real ";
//            //                    break;
//            //                case eVariableDatabaseType.DateTime:
//            //                    line += " datetime ";
//            //                    break;
//            //                case eVariableDatabaseType.String:
//            //                    line += " varchar(100) ";
//            //                    break;
//            //                case eVariableDatabaseType.Char:
//            //                    line += " char(1) ";
//            //                    break;
//            //                case eVariableDatabaseType.Float:
//            //                    line += " float ";
//            //                    break;
//            //                case eVariableDatabaseType.BigInteger:
//            //                    line += " bigint ";
//            //                    break;
//            //            }
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += "null, ";
//            //            }
//            //            else
//            //            {
//            //                line += "null)";
//            //            }
//            //            aWriter.WriteLine(line);
//            //            line = "  ";
//            //        }
//            //        aWriter.WriteLine(" ");

//            //        aWriter.WriteLine(" -- alternate and real time roll");
//            //        aWriter.WriteLine("IF @HN_TYPE = 800001 and @ROLL_OPTION = 800251");
//            //        aWriter.WriteLine("   BEGIN");
//            //        aWriter.WriteLine("   SET @LoopCount = 0");
//            //        aWriter.WriteLine("   CREATE TABLE #TREE (LOOPCOUNT INT NOT NULL, PARENT_HN_RID INT NOT NULL, HOME_PH_RID INT, PH_TYPE INT, CHILD_HN_RID INT NOT NULL, CHILD_HN_MOD INT NOT NULL)");
//            //        aWriter.WriteLine("   -- insert the children of the node into the temp table");
//            //        aWriter.WriteLine("   INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, CHILD_HN_RID, CHILD_HN_MOD) ");
//            //        aWriter.WriteLine("       select @LoopCount as LOOPCOUNT, @HN_RID as PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
//            //        aWriter.WriteLine("         from HIER_NODE_JOIN hnj (NOLOCK)");
//            //        aWriter.WriteLine("           JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//            //        aWriter.WriteLine("           JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//            //        aWriter.WriteLine("       where @HN_RID = hnj.PARENT_HN_RID");
//            //        aWriter.WriteLine("   SET @Loop = @@ROWCOUNT");
//            //        aWriter.WriteLine("   -- chase all paths until you get the main hierarchy (type 800000) or the lowest leaf");
//            //        aWriter.WriteLine("   SET @NextLoopCount = @LoopCount + 1");
//            //        aWriter.WriteLine("   WHILE @Loop > 0");
//            //        aWriter.WriteLine("   BEGIN");
//            //        aWriter.WriteLine("      INSERT #TREE");
//            //        aWriter.WriteLine("        select @NextLoopCount as LOOPCOUNT, hnj.PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
//            //        aWriter.WriteLine("          from HIER_NODE_JOIN hnj (NOLOCK)");
//            //        aWriter.WriteLine("            JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//            //        aWriter.WriteLine("            JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//            //        aWriter.WriteLine("            JOIN #TREE t ON hnj.PARENT_HN_RID = t.CHILD_HN_RID");
//            //        aWriter.WriteLine("        WHERE t.LOOPCOUNT =  @LoopCount AND t.PH_TYPE <> 800000");
//            //        aWriter.WriteLine("      SET @Loop = @@ROWCOUNT");
//            //        aWriter.WriteLine("      SET @LoopCount = @LoopCount + 1");
//            //        aWriter.WriteLine("      SET @NextLoopCount = @LoopCount + 1");
//            //        aWriter.WriteLine("   END");
//            //        aWriter.WriteLine("   -- join with dates from xml selecting only nodes from the main hierarchy or lowest leaf alternates");
//            //        aWriter.WriteLine("   SELECT * ");
//            //        aWriter.WriteLine("     INTO " + aTempTableName + "2");
//            //        aWriter.WriteLine("     FROM #TREE");
//            //        aWriter.WriteLine("     CROSS JOIN " + tempTableName);
//            //        aWriter.WriteLine("   where LOOPCOUNT = @LoopCount - 1");
//            //        aWriter.WriteLine("	     or PH_TYPE = 800000");

//            //        count = 0;
//            //        aWriter.WriteLine("       -- build temp table of summed history values ");
//            //        aWriter.WriteLine("       insert into " + aTempTableName + "HISTORY ");
//            //        aWriter.WriteLine("       select @HN_RID as HN_RID, @FV_RID as FV_RID, shw.TIME_ID, shw.ST_RID, ");
//            //        // add variables
//            //        line = _indent10;
//            //        int variables = 0;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Integer ||
//            //                vp.StoreDatabaseVariableType == eVariableDatabaseType.Real ||
//            //                vp.StoreDatabaseVariableType == eVariableDatabaseType.Float ||
//            //                vp.StoreDatabaseVariableType == eVariableDatabaseType.BigInteger)
//            //            {
//            //                ++variables;
//            //            }
						
//            //        }
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Integer ||
//            //                vp.StoreDatabaseVariableType == eVariableDatabaseType.Real ||
//            //                vp.StoreDatabaseVariableType == eVariableDatabaseType.Float ||
//            //                vp.StoreDatabaseVariableType == eVariableDatabaseType.BigInteger)
//            //            {
//            //                ++count;
//            //                line += "SUM(shw." + vp.DatabaseColumnName + ") " + vp.DatabaseColumnName;
//            //                if (count < variables)
//            //                {
//            //                    line += ", ";
//            //                }
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;
//            //            }
						
//            //        }
//            //        aWriter.WriteLine("		FROM " + aTempTableName + "2 t");
//            //        aWriter.WriteLine("	    JOIN " + aHistoryViewName + " shw (NOLOCK) ON t.CHILD_HN_RID = shw.HN_RID");
//            //        aWriter.WriteLine("		    AND shw.TIME_ID = t.TIME_ID");
//            //        aWriter.WriteLine("	    GROUP BY shw.TIME_ID, ST_RID");
//            //        aWriter.WriteLine(" END ");
//            //        aWriter.WriteLine(" ELSE ");
//            //        aWriter.WriteLine(" BEGIN ");
//            //        aWriter.WriteLine("       -- build temp table of history values ");
//            //        aWriter.WriteLine("       insert into " + aTempTableName + "HISTORY ");
//            //        aWriter.WriteLine("       select shw.HN_RID,  t.FV_RID as FV_RID, shw.TIME_ID, shw.ST_RID, ");
//            //        // add variables
//            //        count = 0;
//            //        line = _indent10;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "shw." + vp.DatabaseColumnName + " as " + vp.DatabaseColumnName;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            aWriter.WriteLine(line);
//            //            line = _indent10;
						
//            //        }
//            //        aWriter.WriteLine("  FROM " + tempTableName + " t");
//            //        aWriter.WriteLine("  JOIN " + aHistoryViewName + " shw (NOLOCK)");
//            //        aWriter.WriteLine("    on shw.HN_RID = t.HN_RID");
//            //        aWriter.WriteLine("   and shw.TIME_ID = t.TIME_ID");
//            //        aWriter.WriteLine(" END ");
//            //        aWriter.WriteLine(" ");
					
//            //        aWriter.WriteLine("  --select * from " + aTempTableName + "MOD");
//            //        aWriter.WriteLine("  --select * from " + aTempTableName + "HISTORY");

//            //        aWriter.WriteLine("   -- combine modified values with history");
//            //        aWriter.WriteLine(" select tmpmod.HN_RID, tmpmod.FV_RID, tmpmod.TIME_ID, tmpmod.ST_RID,");
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            line += "COALESCE(COALESCE(tmpmod." + vp.DatabaseColumnName + ", tmphis." + vp.DatabaseColumnName + "), 0) " + vp.DatabaseColumnName + ", ";
//            //            aWriter.WriteLine(line);
//            //            line = _indent10;
						
//            //        }
//            //        count = 0;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "COALESCE(tmpmod." + vp.DatabaseColumnName + Include.cLockExtension + ", 0) " + vp.DatabaseColumnName + Include.cLockExtension;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            aWriter.WriteLine(line);
//            //            line = _indent10;
						
//            //        }
//            //        aWriter.WriteLine("    into  " + aTempTableName + "MOD2");
//            //        aWriter.WriteLine(" FROM " + aTempTableName + "MOD tmpmod");
//            //        aWriter.WriteLine(" left outer JOIN " + aTempTableName + "HISTORY tmphis  ON tmphis.HN_RID = tmpmod.HN_RID");
//            //        aWriter.WriteLine(" AND tmphis.FV_RID = tmpmod.FV_RID");
//            //        aWriter.WriteLine(" AND tmphis.TIME_ID = tmpmod.TIME_ID");
//            //        aWriter.WriteLine(" AND tmphis.ST_RID = tmpmod.ST_RID");

//            //        aWriter.WriteLine(" ");
//            //        aWriter.WriteLine(" -- remove duplicate rows from history table");
//            //        aWriter.WriteLine("  delete " + aTempTableName + "HISTORY ");
//            //        aWriter.WriteLine("    from " + aTempTableName + "HISTORY, " + aTempTableName + "MOD");
//            //        aWriter.WriteLine("    where " + aTempTableName + "HISTORY.HN_RID = " + aTempTableName + "MOD.HN_RID");
//            //        aWriter.WriteLine("         and " + aTempTableName + "HISTORY.FV_RID = " + aTempTableName + "MOD.FV_RID");
//            //        aWriter.WriteLine("         and " + aTempTableName + "HISTORY.TIME_ID = " + aTempTableName + "MOD.TIME_ID");
//            //        aWriter.WriteLine("         and " + aTempTableName + "HISTORY.ST_RID = " + aTempTableName + "MOD.ST_RID");
//            //        aWriter.WriteLine(" ");

//            //        aWriter.WriteLine(" --select * from " + aTempTableName + "HISTORY");

//            //        aWriter.WriteLine(" insert into " + aTempTableName + "MOD2");
//            //        aWriter.WriteLine(" select tmphis.HN_RID, tmphis.FV_RID, tmphis.TIME_ID, tmphis.ST_RID,");
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            line += "COALESCE(tmphis." + vp.DatabaseColumnName + ", 0) " + vp.DatabaseColumnName + ", ";
//            //            aWriter.WriteLine(line);
//            //            line = _indent10;
//            //        }
//            //        count = 0;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "0 as " + vp.DatabaseColumnName + Include.cLockExtension;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            aWriter.WriteLine(line);
//            //            line = _indent10;
//            //        }

//            //        aWriter.WriteLine(" from " + aTempTableName + "HISTORY tmphis");
//            //        aWriter.WriteLine(" ");
//            //        aWriter.WriteLine(" select * from " + aTempTableName + "MOD2 order by TIME_ID, ST_RID");
//            //        aWriter.WriteLine(" ");

//            //        aWriter.WriteLine(" OPTION (MAXDOP 1)");
					
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "2')) > 0 DROP TABLE " + aTempTableName + "2");
//            //        aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "MOD')) > 0 DROP TABLE " + aTempTableName + "MOD");
//            //        aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "MOD2')) > 0 DROP TABLE " + aTempTableName + "MOD2");
//            //        aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "HISTORY')) > 0 DROP TABLE " + aTempTableName + "HISTORY");
//            //        aWriter.WriteLine(System.Environment.NewLine);

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}

//            //override public void Generate_SP_MID_XML_ST_FOR_WK_DEL_UNLOCK(string aProcedureName, string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableCount)
//            //{
//            //    string line;
//            //    int count;

//            //    try
//            //    {
//            //        AddStoredProcedureDrop(aProcedureName, aWriter);

//            //        if (aDatabaseVariables == null ||
//            //            aDatabaseVariables.Count == 0)
//            //        {
//            //            return;
//            //        }

//            //        aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
//            //        aWriter.WriteLine("@COMMIT_LIMIT INT,");
//            //        aWriter.WriteLine("@RECORDS_DELETED int output");
//            //        aWriter.WriteLine("AS");
//            //        aWriter.WriteLine(System.Environment.NewLine);

//            //        aWriter.WriteLine("DELETE TOP (@COMMIT_LIMIT) " + aTableName);

//            //        line = _indent10 + "WHERE ";
//            //        count = 0;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "COALESCE(" + vp.DatabaseColumnName + Include.cLockExtension + ", 0) = 0";
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += " AND ";
//            //            }
//            //            if (line.Length > 90)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;
//            //            }
//            //        }

//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);
//            //        }

//            //        aWriter.WriteLine("set @RECORDS_DELETED = @@rowcount");

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}

//            override public void Generate_SP_MID_XML_ST_MOD_WK_ROLL_READ(string aProcedureName, 
//                string aForecastTableName, string aHistoryTableName, string aLockTableName, 
//                string aForecastViewName, string aHistoryViewName, string aTempTableName, 
//                ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableNumber, string aTableCountColumn)
//            {
//                try
//                {
//                    string procedureName = aProcedureName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//                    string forecastTableName = aForecastTableName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//                    string historyTableName = aHistoryTableName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//                    string lockTableName = aLockTableName;

//                    AddStoredProcedureDrop(procedureName, aWriter);

//                    if (aDatabaseVariables == null ||
//                        aDatabaseVariables.Count == 0)
//                    {
//                        return;
//                    }

//                    string line = string.Empty;
//                    int count = 0;

//                    aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
//                    aWriter.WriteLine("@xmlDoc AS NTEXT,");
//                    aWriter.WriteLine("@Rollup CHAR(1) = NULL");
//                    aWriter.WriteLine("AS");
//                    aWriter.WriteLine("SET NOCOUNT ON");
//                    aWriter.WriteLine("DECLARE @idoc int,");
//                    aWriter.WriteLine("        @Tables INT,");
//                    aWriter.WriteLine("        @Loop INT,");
//                    aWriter.WriteLine("        @HN_RID INT,");
//                    aWriter.WriteLine("        @FV_RID INT,");
//                    aWriter.WriteLine("        @HN_MOD INT,");
//                    aWriter.WriteLine("        @LoopCount INT,");
//                    aWriter.WriteLine("        @NextLoopCount INT");
//                    aWriter.WriteLine("EXEC sp_xml_preparedocument @idoc OUTPUT, @xmlDoc");
//                    aWriter.WriteLine("SELECT @Tables = " + aTableCountColumn + " FROM SYSTEM_OPTIONS");

//                    aWriter.WriteLine("SELECT xmlHN_RID, xmlFV_RID, xmlTIME_ID, xmlHN_RID % @Tables AS xmlHN_MOD");
//                    aWriter.WriteLine("	INTO " + aTempTableName);
//                    aWriter.WriteLine("	FROM OPENXML (@idoc, '/root/node/time',2)");
//                    aWriter.WriteLine("         WITH (  xmlHN_RID  int      '../@ID',");
//                    aWriter.WriteLine("                 xmlFV_RID INT '../@FV_RID',");
//                    aWriter.WriteLine("                 xmlTIME_ID INT '@ID') xml");
					
//                    aWriter.WriteLine(" ");
//                    aWriter.WriteLine(" -- build temp table of values and locks for modified version");
//                    aWriter.WriteLine(" create table " + aTempTableName + "MOD ");
//                    aWriter.WriteLine(" (HN_RID  int not null,");
//                    aWriter.WriteLine("  FV_RID  int not null,");
//                    aWriter.WriteLine("  TIME_ID int not null,");
//                    aWriter.WriteLine("  ST_RID int not null,");
//                    count = 0;
//                    // add variables
//                    line = "  ";
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        line += "  " + vp.DatabaseColumnName;
//                        switch (vp.ChainDatabaseVariableType)
//                        {
//                            case eVariableDatabaseType.Integer:
//                                line += " int ";
//                                break;
//                            case eVariableDatabaseType.Real:
//                                line += " real ";
//                                break;
//                            case eVariableDatabaseType.DateTime:
//                                line += " datetime ";
//                                break;
//                            case eVariableDatabaseType.String:
//                                line += " varchar(100) ";
//                                break;
//                            case eVariableDatabaseType.Char:
//                                line += " char(1) ";
//                                break;
//                            case eVariableDatabaseType.Float:
//                                line += " float ";
//                                break;
//                            case eVariableDatabaseType.BigInteger:
//                                line += " bigint ";
//                                break;
//                        }
//                        line += "  null,";
//                        aWriter.WriteLine(line);
//                        line = "  ";
//                    }
//                    count = 0;
//                    // add locks
//                    line = "  ";
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        line += "  " + vp.DatabaseColumnName + Include.cLockExtension + "   char(1) null";
//                        if (count < aDatabaseVariables.Count)
//                        {
//                            line += ", ";
//                        }
//                        else
//                        {
//                            line += ")";
//                        }
//                        aWriter.WriteLine(line);
//                        line = "  ";
						
//                    }
//                    if (line.Length > "  ".Length)
//                    {
//                        aWriter.WriteLine(line);
//                    }

//                    aWriter.WriteLine(" ");
//                    aWriter.WriteLine(" insert into " + aTempTableName + "MOD");
//                    aWriter.WriteLine("  select sfw.HN_RID, sfw.FV_RID, sfw.TIME_ID, sfw.ST_RID,");
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        line += "  sfw." + vp.DatabaseColumnName + ", ";
//                        if (line.Length > 80)
//                        {
//                            aWriter.WriteLine(line);
//                            line = "  ";
//                        }
						
//                    }
//                    if (line.Length > "  ".Length)
//                    {
//                        aWriter.WriteLine(line);
//                    }
//                    count = 0;
//                    line = "  ";
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        line += "  COALESCE(sfwl." + vp.DatabaseColumnName + Include.cLockExtension + ", 0) " + vp.DatabaseColumnName + Include.cLockExtension;
//                        if (count < aDatabaseVariables.Count)
//                        {
//                            line += ", ";
//                        }
//                        aWriter.WriteLine(line);
//                        line = "  ";
						
//                    }
     
//                    aWriter.WriteLine("   FROM " + aTempTableName + " t");
//                    aWriter.WriteLine("   JOIN " + aForecastViewName + " sfw (NOLOCK)");
//                    aWriter.WriteLine("    on sfw.HN_RID = t.xmlHN_RID");
//                    aWriter.WriteLine("    and sfw.FV_RID = t.xmlFV_RID");
//                    aWriter.WriteLine("    and sfw.TIME_ID = t.xmlTIME_ID");
//                    aWriter.WriteLine("   left outer join " + lockTableName + " sfwl (NOLOCK)");
//                    aWriter.WriteLine("    on sfwl.HN_RID = sfw.HN_RID");
//                    aWriter.WriteLine("    and sfwl.FV_RID =sfw.FV_RID");
//                    aWriter.WriteLine("    and sfwl.TIME_ID = sfw.TIME_ID");
//                    aWriter.WriteLine("    and sfwl.ST_RID = sfw.ST_RID");
//                    aWriter.WriteLine("  union");
//                    aWriter.WriteLine("  select sfwl.HN_RID, sfwl.FV_RID, sfwl.TIME_ID, sfwl.ST_RID, ");
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        line += "  sfw." + vp.DatabaseColumnName + ", ";
//                        if (line.Length > 80)
//                        {
//                            aWriter.WriteLine(line);
//                            line = "  ";
//                        }
						
//                    }
//                    if (line.Length > "  ".Length)
//                    {
//                        aWriter.WriteLine(line);
//                    }
//                    count = 0;
//                    line = "  ";
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        line += "  COALESCE(sfwl." + vp.DatabaseColumnName + Include.cLockExtension + ", 0) " + vp.DatabaseColumnName + Include.cLockExtension;
//                        if (count < aDatabaseVariables.Count)
//                        {
//                            line += ", ";
//                        }
//                        aWriter.WriteLine(line);
//                        line = "  ";
						
//                    }

//                    aWriter.WriteLine(" FROM " + aTempTableName + " t");
//                    aWriter.WriteLine(" JOIN " + lockTableName + " sfwl (NOLOCK)");
//                    aWriter.WriteLine("  on sfwl.HN_RID = t.xmlHN_RID");
//                    aWriter.WriteLine("  and sfwl.FV_RID = t.xmlFV_RID");
//                    aWriter.WriteLine("  and sfwl.TIME_ID = t.xmlTIME_ID");
//                    aWriter.WriteLine(" left outer join " + aForecastViewName + " sfw (NOLOCK)");
//                    aWriter.WriteLine("  on sfw.HN_RID = sfwl.HN_RID");
//                    aWriter.WriteLine("  and sfw.FV_RID =sfwl.FV_RID");
//                    aWriter.WriteLine("  and sfw.TIME_ID = sfwl.TIME_ID");
//                    aWriter.WriteLine("  and sfw.ST_RID = sfwl.ST_RID");

//                    aWriter.WriteLine(" ");
//                    aWriter.WriteLine(" -- create temp table for history values");
//                    aWriter.WriteLine(" create table " + aTempTableName + "HISTORY ");
//                    aWriter.WriteLine(" (HN_RID  int not null,");
//                    aWriter.WriteLine("  FV_RID  int not null,");
//                    aWriter.WriteLine("  TIME_ID int not null,");
//                    aWriter.WriteLine("  ST_RID int not null,");
//                    count = 0;
//                    // add variables
//                    line = "  ";
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        line += "  " + vp.DatabaseColumnName;
//                        switch (vp.ChainDatabaseVariableType)
//                        {
//                            case eVariableDatabaseType.Integer:
//                                line += " int ";
//                                break;
//                            case eVariableDatabaseType.Real:
//                                line += " real ";
//                                break;
//                            case eVariableDatabaseType.DateTime:
//                                line += " datetime ";
//                                break;
//                            case eVariableDatabaseType.String:
//                                line += " varchar(100) ";
//                                break;
//                            case eVariableDatabaseType.Char:
//                                line += " char(1) ";
//                                break;
//                            case eVariableDatabaseType.Float:
//                                line += " float ";
//                                break;
//                            case eVariableDatabaseType.BigInteger:
//                                line += " bigint ";
//                                break;
//                        }
//                        if (count < aDatabaseVariables.Count)
//                        {
//                            line += "null, ";
//                        }
//                        else
//                        {
//                            line += "null)";
//                        }
//                        aWriter.WriteLine(line);
//                        line = "  ";
//                    }
//                    aWriter.WriteLine(" ");

//                    aWriter.WriteLine(" -- alternate and real time roll");
//                    aWriter.WriteLine("IF @HN_TYPE = 800001 and @ROLL_OPTION = 800251");
//                    aWriter.WriteLine("   BEGIN");
//                    aWriter.WriteLine("   SET @LoopCount = 0");
//                    aWriter.WriteLine("   CREATE TABLE #TREE (LOOPCOUNT INT NOT NULL, PARENT_HN_RID INT NOT NULL, HOME_PH_RID INT, PH_TYPE INT, CHILD_HN_RID INT NOT NULL, CHILD_HN_MOD INT NOT NULL)");
//                    aWriter.WriteLine("   -- insert the children of the node into the temp table");
//                    aWriter.WriteLine("   INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, CHILD_HN_RID, CHILD_HN_MOD) ");
//                    aWriter.WriteLine("       select @LoopCount as LOOPCOUNT, @HN_RID as PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
//                    aWriter.WriteLine("         from HIER_NODE_JOIN hnj (NOLOCK)");
//                    aWriter.WriteLine("           JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//                    aWriter.WriteLine("           JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//                    aWriter.WriteLine("       where @HN_RID = hnj.PARENT_HN_RID");
//                    aWriter.WriteLine("   SET @Loop = @@ROWCOUNT");
//                    aWriter.WriteLine("   -- chase all paths until you get the main hierarchy (type 800000) or the lowest leaf");
//                    aWriter.WriteLine("   SET @NextLoopCount = @LoopCount + 1");
//                    aWriter.WriteLine("   WHILE @Loop > 0");
//                    aWriter.WriteLine("   BEGIN");
//                    aWriter.WriteLine("      INSERT #TREE");
//                    aWriter.WriteLine("        select @NextLoopCount as LOOPCOUNT, hnj.PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
//                    aWriter.WriteLine("          from HIER_NODE_JOIN hnj (NOLOCK)");
//                    aWriter.WriteLine("            JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//                    aWriter.WriteLine("            JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//                    aWriter.WriteLine("            JOIN #TREE t ON hnj.PARENT_HN_RID = t.CHILD_HN_RID");
//                    aWriter.WriteLine("        WHERE t.LOOPCOUNT =  @LoopCount AND t.PH_TYPE <> 800000");
//                    aWriter.WriteLine("      SET @Loop = @@ROWCOUNT");
//                    aWriter.WriteLine("      SET @LoopCount = @LoopCount + 1");
//                    aWriter.WriteLine("      SET @NextLoopCount = @LoopCount + 1");
//                    aWriter.WriteLine("   END");
//                    aWriter.WriteLine("   -- join with dates from xml selecting only nodes from the main hierarchy or lowest leaf alternates");
//                    aWriter.WriteLine("   SELECT * ");
//                    aWriter.WriteLine("     INTO " + aTempTableName + "2");
//                    aWriter.WriteLine("     FROM #TREE");
//                    aWriter.WriteLine("     CROSS JOIN " + aTempTableName);
//                    aWriter.WriteLine("   where LOOPCOUNT = @LoopCount - 1");
//                    aWriter.WriteLine("	     or PH_TYPE = 800000");

//                    count = 0;
//                    aWriter.WriteLine("       -- build temp table of summed history values ");
//                    aWriter.WriteLine("       insert into " + aTempTableName + "HISTORY ");
//                    aWriter.WriteLine("       select @HN_RID as HN_RID, @FV_RID as FV_RID, shw.TIME_ID, shw.ST_RID, ");
//                    // add variables
//                    line = _indent10;
//                    int variables = 0;
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        if (vp.ChainDatabaseVariableType == eVariableDatabaseType.Integer ||
//                            vp.ChainDatabaseVariableType == eVariableDatabaseType.Real ||
//                            vp.ChainDatabaseVariableType == eVariableDatabaseType.Float ||
//                            vp.ChainDatabaseVariableType == eVariableDatabaseType.BigInteger)
//                        {
//                            ++variables;
//                        }
						
//                    }
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        if (vp.ChainDatabaseVariableType == eVariableDatabaseType.Integer ||
//                            vp.ChainDatabaseVariableType == eVariableDatabaseType.Real ||
//                            vp.ChainDatabaseVariableType == eVariableDatabaseType.Float ||
//                            vp.ChainDatabaseVariableType == eVariableDatabaseType.BigInteger)
//                        {
//                            ++count;
//                            line += "SUM(shw." + vp.DatabaseColumnName + ") " + vp.DatabaseColumnName;
//                            if (count < variables)
//                            {
//                                line += ", ";
//                            }
//                            aWriter.WriteLine(line);
//                            line = _indent10;
//                        }
						
//                    }
//                    aWriter.WriteLine("		FROM " + aTempTableName + "2 t");
//                    aWriter.WriteLine("	    JOIN " + aHistoryViewName + " shw (NOLOCK) ON t.CHILD_HN_RID = shw.HN_RID");
//                    aWriter.WriteLine("		    AND shw.TIME_ID = t.TIME_ID");
//                    aWriter.WriteLine("	    GROUP BY shw.TIME_ID, ST_RID");
//                    aWriter.WriteLine(" ");
					
//                    aWriter.WriteLine("  --select * from " + aTempTableName + "MOD");
//                    aWriter.WriteLine("  --select * from " + aTempTableName + "HISTORY");

//                    aWriter.WriteLine("   -- combine modified values with history");
//                    aWriter.WriteLine(" select tmpmod.HN_RID, tmpmod.FV_RID, tmpmod.TIME_ID, tmpmod.ST_RID,");
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        line += "COALESCE(COALESCE(tmpmod." + vp.DatabaseColumnName + ", tmphis." + vp.DatabaseColumnName + "), 0) " + vp.DatabaseColumnName + ", ";
//                        aWriter.WriteLine(line);
//                        line = _indent10;
						
//                    }
//                    count = 0;
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        line += "COALESCE(tmpmod." + vp.DatabaseColumnName + Include.cLockExtension + ", 0) " + vp.DatabaseColumnName + Include.cLockExtension;
//                        if (count < aDatabaseVariables.Count)
//                        {
//                            line += ", ";
//                        }
//                        aWriter.WriteLine(line);
//                        line = _indent10;
						
//                    }
//                    aWriter.WriteLine("    into  " + aTempTableName + "MOD2");
//                    aWriter.WriteLine(" FROM " + aTempTableName + "MOD tmpmod");
//                    aWriter.WriteLine(" left outer JOIN " + aTempTableName + "HISTORY tmphis  ON tmphis.HN_RID = tmpmod.HN_RID");
//                    aWriter.WriteLine(" AND tmphis.FV_RID = tmpmod.FV_RID");
//                    aWriter.WriteLine(" AND tmphis.TIME_ID = tmpmod.TIME_ID");
//                    aWriter.WriteLine(" AND tmphis.ST_RID = tmpmod.ST_RID");

//                    aWriter.WriteLine(" ");
//                    aWriter.WriteLine(" -- remove duplicate rows from history table");
//                    aWriter.WriteLine("  delete " + aTempTableName + "HISTORY ");
//                    aWriter.WriteLine("    from " + aTempTableName + "HISTORY, " + aTempTableName + "MOD");
//                    aWriter.WriteLine("    where " + aTempTableName + "HISTORY.HN_RID = " + aTempTableName + "MOD.HN_RID");
//                    aWriter.WriteLine("         and " + aTempTableName + "HISTORY.FV_RID = " + aTempTableName + "MOD.FV_RID");
//                    aWriter.WriteLine("         and " + aTempTableName + "HISTORY.TIME_ID = " + aTempTableName + "MOD.TIME_ID");
//                    aWriter.WriteLine("         and " + aTempTableName + "HISTORY.ST_RID = " + aTempTableName + "MOD.ST_RID");
//                    aWriter.WriteLine(" ");

//                    aWriter.WriteLine(" --select * from " + aTempTableName + "HISTORY");

//                    aWriter.WriteLine(" insert into " + aTempTableName + "MOD2");
//                    aWriter.WriteLine(" select tmphis.HN_RID, tmphis.FV_RID, tmphis.TIME_ID, tmphis.ST_RID,");
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        line += "COALESCE(tmphis." + vp.DatabaseColumnName + ", 0) " + vp.DatabaseColumnName + ", ";
//                        aWriter.WriteLine(line);
//                        line = _indent10;
//                    }
//                    count = 0;
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        line += "0 as " + vp.DatabaseColumnName + Include.cLockExtension;
//                        if (count < aDatabaseVariables.Count)
//                        {
//                            line += ", ";
//                        }
//                        aWriter.WriteLine(line);
//                        line = _indent10;
//                    }

//                    aWriter.WriteLine(" from " + aTempTableName + "HISTORY tmphis");
//                    aWriter.WriteLine(" ");
//                    aWriter.WriteLine(" select * from " + aTempTableName + "MOD2 order by TIME_ID, ST_RID");
//                    aWriter.WriteLine(" ");

//                    aWriter.WriteLine(" OPTION (MAXDOP 1)");
					
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            //override public void Generate_SP_MID_XML_CHN_FOR_WK_DEL_ZERO(string aProcedureName, string aTableName, string aLockTableName, ProfileList aDatabaseVariables, StreamWriter aWriter)
//            //{
//            //    string line;
//            //    int count;

//            //    try
//            //    {
//            //        AddStoredProcedureDrop("SP_MID_CHN_FOR_WK{#}_DEL_ZERO", aWriter);

//            //        AddStoredProcedureDrop(aProcedureName, aWriter);

//            //        if (aDatabaseVariables == null ||
//            //            aDatabaseVariables.Count == 0)
//            //        {
//            //            return;
//            //        }

//            //        aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
//            //        aWriter.WriteLine("@COMMIT_LIMIT INT,");
//            //        aWriter.WriteLine("@RECORDS_DELETED int output");
//            //        aWriter.WriteLine("AS");
//            //        aWriter.WriteLine(System.Environment.NewLine);

//            //        aWriter.WriteLine("DELETE top (@COMMIT_LIMIT) from " + aTableName);
                    
//            //        line = _indent10 + "WHERE ";
//            //        count = 0;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "COALESCE(" + vp.DatabaseColumnName + ", 0) = 0";
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += " AND ";
//            //            }
//            //            if (line.Length > 90)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;
//            //            }
//            //        }

//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);
//            //        }

//            //        aWriter.WriteLine("set @RECORDS_DELETED = @@rowcount");
//            //        aWriter.WriteLine(System.Environment.NewLine);

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}

//            //override public void Generate_SP_MID_XML_CHN_FOR_WK_DEL_UNLOCK(string aProcedureName, string aTableName, ProfileList aDatabaseVariables, StreamWriter aWriter)
//            //{
//            //    string line;
//            //    int count;

//            //    try
//            //    {
//            //        AddStoredProcedureDrop(aProcedureName, aWriter);

//            //        if (aDatabaseVariables == null ||
//            //            aDatabaseVariables.Count == 0)
//            //        {
//            //            return;
//            //        }

//            //        aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
//            //        aWriter.WriteLine("@COMMIT_LIMIT INT,");
//            //        aWriter.WriteLine("@RECORDS_DELETED int output");
//            //        aWriter.WriteLine("AS");
//            //        aWriter.WriteLine(System.Environment.NewLine);

//            //        aWriter.WriteLine("DELETE TOP (@COMMIT_LIMIT) " + aTableName);

//            //        line = _indent10 + "WHERE ";
//            //        count = 0;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "COALESCE(" + vp.DatabaseColumnName + Include.cLockExtension + ", 0) = 0";
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += " AND ";
//            //            }
//            //            if (line.Length > 90)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;
//            //            }
//            //        }

//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);
//            //        }

//            //        aWriter.WriteLine("set @RECORDS_DELETED = @@rowcount");

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}

//            //override public void Generate_SP_MID_XML_ST_FOR_WK_DEL_ZERO(string aProcedureName, string aTableName, string aLockTableName, ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableNumber)
//            //{
//            //    string line;
//            //    int count;

//            //    try
//            //    {
//            //        string procedureName = aProcedureName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//            //        string tableName = aTableName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());

//            //        AddStoredProcedureDrop(procedureName, aWriter);

//            //        if (aDatabaseVariables == null ||
//            //            aDatabaseVariables.Count == 0)
//            //        {
//            //            return;
//            //        }
                    
//            //        aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
//            //        aWriter.WriteLine("@COMMIT_LIMIT INT,");
//            //        aWriter.WriteLine("@RECORDS_DELETED int output");
//            //        aWriter.WriteLine("AS");
//            //        aWriter.WriteLine(System.Environment.NewLine);

//            //        aWriter.WriteLine("DELETE top (@COMMIT_LIMIT) from " + tableName);
                    
//            //        line = _indent10 + "WHERE ";
//            //        count = 0;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "COALESCE(" + vp.DatabaseColumnName + ", 0) = 0";
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += " AND ";
//            //            }
//            //            if (line.Length > 90)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;
//            //            }
//            //        }

//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);
//            //        }

//            //        aWriter.WriteLine("set @RECORDS_DELETED = @@rowcount");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}

//            //override public void Generate_SP_MID_XML_CHN_MOD_WK_READ(string aProcedureName, 
//            //    string aForecastTableName, string aHistoryTableName, string aLockTableName, 
//            //    string aForecastViewName, string aHistoryViewName, string aTableCountColumn, string aTempTableName, 
//            //    ProfileList aDatabaseVariables, StreamWriter aWriter)
//            //{
//            //    try
//            //    {
//            //        AddStoredProcedureDrop(aProcedureName, aWriter);
//            //        AddTypeDrop(Include.DBChainWeeklyModifiedReadType, aWriter);
                    

//            //        if (aDatabaseVariables == null ||
//            //            aDatabaseVariables.Count == 0)
//            //        {
//            //            return;
//            //        }

//            //        string tempTableName = "@dt";

//            //        //aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBChainWeeklyModifiedReadType + "] AS TABLE(");
//            //        //aWriter.WriteLine("  [HN_RID] [int],");
//            //        //aWriter.WriteLine("  [FV_RID] [int],");
//            //        //aWriter.WriteLine("  [TIME_ID] [int]");
//            //        //aWriter.WriteLine("PRIMARY KEY (HN_RID, FV_RID, TIME_ID)");
//            //        //aWriter.WriteLine(")");

//            //        //aWriter.WriteLine(System.Environment.NewLine);
//            //        //aWriter.WriteLine("GO");
//            //        //aWriter.WriteLine(System.Environment.NewLine);


//            //        string line = string.Empty;
//            //        int count = 0;

//            //        aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
//            //        aWriter.WriteLine("@dt " + Include.DBChainWeeklyModifiedReadType + " READONLY,");
//            //        aWriter.WriteLine("@Rollup CHAR(1) = NULL");
//            //        aWriter.WriteLine("AS");
//            //        aWriter.WriteLine("SET NOCOUNT ON");
//            //        aWriter.WriteLine("DECLARE @Tables INT,");
//            //        aWriter.WriteLine("        @Loop INT,");
//            //        aWriter.WriteLine("        @HN_TYPE INT,");
//            //        aWriter.WriteLine("        @HN_RID INT,");
//            //        aWriter.WriteLine("        @FV_RID INT,");
//            //        aWriter.WriteLine("        @HN_MOD INT,");
//            //        aWriter.WriteLine("        @ROLL_OPTION INT,");
//            //        aWriter.WriteLine("        @LoopCount INT,");
//            //        aWriter.WriteLine("        @NextLoopCount INT");

//            //        aWriter.WriteLine("SELECT @Tables = " + aTableCountColumn + " FROM SYSTEM_OPTIONS");

//            //        aWriter.WriteLine("SELECT DISTINCT @HN_TYPE = ph.PH_TYPE, @HN_RID = t.HN_RID, @HN_MOD = t.HN_RID % @Tables,");
//            //        aWriter.WriteLine("                @ROLL_OPTION = COALESCE(ph.HISTORY_ROLL_OPTION, 800251), @FV_RID = t.FV_RID");
//            //        aWriter.WriteLine("        FROM HIERARCHY_NODE hn (NOLOCK)");
//            //        aWriter.WriteLine("        JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//            //        aWriter.WriteLine("        JOIN " + tempTableName + " t on t.HN_RID = hn.HN_RID");
			
//            //        aWriter.WriteLine(" ");
//            //        aWriter.WriteLine(" -- build temp table of values and locks for modified version");
//            //        aWriter.WriteLine(" create table " + aTempTableName + "MOD ");
//            //        aWriter.WriteLine(" (HN_RID  int not null,");
//            //        aWriter.WriteLine("  FV_RID  int not null,");
//            //        aWriter.WriteLine("  TIME_ID int not null,");
//            //        count = 0;
//            //        // add variables
//            //        line = "  ";
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            line += "  " + vp.DatabaseColumnName;
//            //            switch (vp.ChainDatabaseVariableType)
//            //            {
//            //                case eVariableDatabaseType.Integer:
//            //                    line += " int ";
//            //                    break;
//            //                case eVariableDatabaseType.Real:
//            //                    line += " real ";
//            //                    break;
//            //                case eVariableDatabaseType.DateTime:
//            //                    line += " datetime ";
//            //                    break;
//            //                case eVariableDatabaseType.String:
//            //                    line += " varchar(100) ";
//            //                    break;
//            //                case eVariableDatabaseType.Char:
//            //                    line += " char(1) ";
//            //                    break;
//            //                case eVariableDatabaseType.Float:
//            //                    line += " float ";
//            //                    break;
//            //                case eVariableDatabaseType.BigInteger:
//            //                    line += " bigint ";
//            //                    break;
//            //            }
//            //            line += "  null,";
//            //            aWriter.WriteLine(line);
//            //            line = "  ";
//            //        }
//            //        count = 0;
//            //        // add locks
//            //        line = "  ";
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "  " + vp.DatabaseColumnName + Include.cLockExtension + "   char(1) null";
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            else
//            //            {
//            //                line += ")";
//            //            }
//            //            aWriter.WriteLine(line);
//            //            line = "  ";
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);
//            //        }

//            //        aWriter.WriteLine(" ");
//            //        aWriter.WriteLine(" insert into " + aTempTableName + "MOD");
//            //        aWriter.WriteLine("  select cfw.HN_RID, cfw.FV_RID, cfw.TIME_ID,");
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            line += "  cfw." + vp.DatabaseColumnName + ", ";
//            //            if (line.Length > 80)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = "  ";
//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);
//            //        }
//            //        count = 0;
//            //        line = "  ";
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "  COALESCE(cfwl." + vp.DatabaseColumnName + Include.cLockExtension + ", 0) " + vp.DatabaseColumnName + Include.cLockExtension;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            aWriter.WriteLine(line);
//            //            line = "  ";
						
//            //        }

//            //        aWriter.WriteLine("   FROM " + tempTableName + " t");
//            //        aWriter.WriteLine("   JOIN " + aForecastViewName + " cfw (NOLOCK)");
//            //        aWriter.WriteLine("    on cfw.HN_RID = t.HN_RID");
//            //        aWriter.WriteLine("    and cfw.FV_RID = t.FV_RID");
//            //        aWriter.WriteLine("    and cfw.TIME_ID = t.TIME_ID");
//            //        aWriter.WriteLine("   left outer join " + aLockTableName + " cfwl (NOLOCK)");
//            //        aWriter.WriteLine("    on cfwl.HN_RID = cfw.HN_RID");
//            //        aWriter.WriteLine("    and cfwl.FV_RID =cfw.FV_RID");
//            //        aWriter.WriteLine("    and cfwl.TIME_ID = cfw.TIME_ID");
//            //        aWriter.WriteLine("  union");
//            //        aWriter.WriteLine("  select cfwl.HN_RID,  cfwl.FV_RID,  cfwl.TIME_ID, ");
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            line += "  cfw." + vp.DatabaseColumnName + ", ";
//            //            if (line.Length > 80)
//            //            {
//            //                aWriter.WriteLine(line);
//            //                line = "  ";
//            //            }
						
//            //        }
//            //        if (line.Length > 0)
//            //        {
//            //            aWriter.WriteLine(line);
//            //        }
//            //        count = 0;
//            //        line = "  ";
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "  COALESCE(cfwl." + vp.DatabaseColumnName + Include.cLockExtension + ", 0) " + vp.DatabaseColumnName + Include.cLockExtension;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            aWriter.WriteLine(line);
//            //            line = "  ";
						
//            //        }

//            //        aWriter.WriteLine(" FROM " + tempTableName + " t");
//            //        aWriter.WriteLine(" JOIN " + aLockTableName + " cfwl (NOLOCK)");
//            //        aWriter.WriteLine("  on cfwl.HN_RID = t.HN_RID");
//            //        aWriter.WriteLine("  and cfwl.FV_RID = t.FV_RID");
//            //        aWriter.WriteLine("  and cfwl.TIME_ID = t.TIME_ID");
//            //        aWriter.WriteLine(" left outer join " + aForecastViewName + " cfw (NOLOCK)");
//            //        aWriter.WriteLine("  on cfw.HN_RID = cfwl.HN_RID");
//            //        aWriter.WriteLine("  and cfw.FV_RID =cfwl.FV_RID");
//            //        aWriter.WriteLine("  and cfw.TIME_ID = cfwl.TIME_ID");

//            //        aWriter.WriteLine(" ");
//            //        aWriter.WriteLine(" -- create temp table for history values");
//            //        aWriter.WriteLine(" create table " + aTempTableName + "HISTORY ");
//            //        aWriter.WriteLine(" (HN_RID  int not null,");
//            //        aWriter.WriteLine("  FV_RID  int not null,");
//            //        aWriter.WriteLine("  TIME_ID int not null,");
//            //        count = 0;
//            //        // add variables
//            //        line = "  ";
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "  " + vp.DatabaseColumnName;
//            //            switch (vp.ChainDatabaseVariableType)
//            //            {
//            //                case eVariableDatabaseType.Integer:
//            //                    line += " int ";
//            //                    break;
//            //                case eVariableDatabaseType.Real:
//            //                    line += " real ";
//            //                    break;
//            //                case eVariableDatabaseType.DateTime:
//            //                    line += " datetime ";
//            //                    break;
//            //                case eVariableDatabaseType.String:
//            //                    line += " varchar(100) ";
//            //                    break;
//            //                case eVariableDatabaseType.Char:
//            //                    line += " char(1) ";
//            //                    break;
//            //                case eVariableDatabaseType.Float:
//            //                    line += " float ";
//            //                    break;
//            //                case eVariableDatabaseType.BigInteger:
//            //                    line += " bigint ";
//            //                    break;
//            //            }
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += "null, ";
//            //            }
//            //            else
//            //            {
//            //                line += "null)";
//            //            }
//            //            aWriter.WriteLine(line);
//            //            line = "  ";
//            //        }
//            //        aWriter.WriteLine(" ");

//            //        aWriter.WriteLine(" -- alternate and real time roll");
//            //        aWriter.WriteLine("IF @HN_TYPE = 800001 and @ROLL_OPTION = 800251");
//            //        aWriter.WriteLine("   BEGIN");
//            //        aWriter.WriteLine("   SET @LoopCount = 0");
//            //        aWriter.WriteLine("   CREATE TABLE #TREE (LOOPCOUNT INT NOT NULL, PARENT_HN_RID INT NOT NULL, HOME_PH_RID INT, PH_TYPE INT, CHILD_HN_RID INT NOT NULL, CHILD_HN_MOD INT NOT NULL)");
//            //        aWriter.WriteLine("   -- insert the children of the node into the temp table");
//            //        aWriter.WriteLine("   INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, CHILD_HN_RID, CHILD_HN_MOD) ");
//            //        aWriter.WriteLine("       select @LoopCount as LOOPCOUNT, @HN_RID as PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
//            //        aWriter.WriteLine("         from HIER_NODE_JOIN hnj (NOLOCK)");
//            //        aWriter.WriteLine("           JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//            //        aWriter.WriteLine("           JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//            //        aWriter.WriteLine("       where @HN_RID = hnj.PARENT_HN_RID");
//            //        aWriter.WriteLine("   SET @Loop = @@ROWCOUNT");
//            //        aWriter.WriteLine("   -- chase all paths until you get the main hierarchy (type 800000) or the lowest leaf");
//            //        aWriter.WriteLine("   SET @NextLoopCount = @LoopCount + 1");
//            //        aWriter.WriteLine("   WHILE @Loop > 0");
//            //        aWriter.WriteLine("   BEGIN");
//            //        aWriter.WriteLine("      INSERT #TREE");
//            //        aWriter.WriteLine("        select @NextLoopCount as LOOPCOUNT, hnj.PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
//            //        aWriter.WriteLine("          from HIER_NODE_JOIN hnj (NOLOCK)");
//            //        aWriter.WriteLine("            JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//            //        aWriter.WriteLine("            JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//            //        aWriter.WriteLine("            JOIN #TREE t ON hnj.PARENT_HN_RID = t.CHILD_HN_RID");
//            //        aWriter.WriteLine("        WHERE t.LOOPCOUNT =  @LoopCount AND t.PH_TYPE <> 800000");
//            //        aWriter.WriteLine("      SET @Loop = @@ROWCOUNT");
//            //        aWriter.WriteLine("      SET @LoopCount = @LoopCount + 1");
//            //        aWriter.WriteLine("      SET @NextLoopCount = @LoopCount + 1");
//            //        aWriter.WriteLine("   END");
//            //        aWriter.WriteLine("   -- join with dates from xml selecting only nodes from the main hierarchy or lowest leaf alternates");
//            //        aWriter.WriteLine("   SELECT * ");
//            //        aWriter.WriteLine("     INTO " + aTempTableName + "2");
//            //        aWriter.WriteLine("     FROM #TREE");
//            //        aWriter.WriteLine("     CROSS JOIN " + tempTableName);
//            //        aWriter.WriteLine("   where LOOPCOUNT = @LoopCount - 1");
//            //        aWriter.WriteLine("	     or PH_TYPE = 800000");

//            //        count = 0;
//            //        aWriter.WriteLine("       -- build temp table of summed history values ");
//            //        aWriter.WriteLine("       insert into " + aTempTableName + "HISTORY ");
//            //        aWriter.WriteLine("       select @HN_RID as HN_RID, @FV_RID as FV_RID, chw.TIME_ID, ");
//            //        // add variables
//            //        line = _indent10;
//            //        int variables = 0;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            if (vp.ChainDatabaseVariableType == eVariableDatabaseType.Integer ||
//            //                vp.ChainDatabaseVariableType == eVariableDatabaseType.Real ||
//            //                vp.ChainDatabaseVariableType == eVariableDatabaseType.Float ||
//            //                vp.ChainDatabaseVariableType == eVariableDatabaseType.BigInteger)
//            //            {
//            //                ++variables;
//            //            }
						
//            //        }
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            if (vp.ChainDatabaseVariableType == eVariableDatabaseType.Integer ||
//            //                vp.ChainDatabaseVariableType == eVariableDatabaseType.Real ||
//            //                vp.ChainDatabaseVariableType == eVariableDatabaseType.Float ||
//            //                vp.ChainDatabaseVariableType == eVariableDatabaseType.BigInteger)
//            //            {
//            //                ++count;
//            //                line += "SUM(chw." + vp.DatabaseColumnName + ") " + vp.DatabaseColumnName;
//            //                if (count < variables)
//            //                {
//            //                    line += ", ";
//            //                }
//            //                aWriter.WriteLine(line);
//            //                line = _indent10;
//            //            }
//            //        }
//            //        aWriter.WriteLine("		FROM " + aTempTableName + "2 t");
//            //        aWriter.WriteLine("	    JOIN " + aHistoryViewName + " chw (NOLOCK) ON t.CHILD_HN_RID = chw.HN_RID");
//            //        aWriter.WriteLine("		    AND chw.TIME_ID = t.TIME_ID");
//            //        aWriter.WriteLine("	    GROUP BY chw.TIME_ID");
//            //        aWriter.WriteLine(" END ");
//            //        aWriter.WriteLine(" ELSE ");
//            //        aWriter.WriteLine(" BEGIN ");
//            //        aWriter.WriteLine("       -- build temp table of history values ");
//            //        aWriter.WriteLine("       insert into " + aTempTableName + "HISTORY ");
//            //        aWriter.WriteLine("       select chw.HN_RID,  t.FV_RID as FV_RID, chw.TIME_ID, ");
//            //        // add variables
//            //        count = 0;
//            //        line = _indent10;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "chw." + vp.DatabaseColumnName + " as " + vp.DatabaseColumnName;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            aWriter.WriteLine(line);
//            //            line = _indent10;
						
//            //        }
//            //        aWriter.WriteLine("  FROM " + tempTableName + " t");
//            //        aWriter.WriteLine("  JOIN " + aHistoryViewName + " chw (NOLOCK)");
//            //        aWriter.WriteLine("    on chw.HN_RID = t.HN_RID");
//            //        aWriter.WriteLine("   and chw.TIME_ID = t.TIME_ID");
//            //        aWriter.WriteLine(" END ");
//            //        aWriter.WriteLine(" ");
					
//            //        aWriter.WriteLine("  --select * from " + aTempTableName + "MOD");
//            //        aWriter.WriteLine("  --select * from " + aTempTableName + "HISTORY");

//            //        aWriter.WriteLine("   -- combine modified values with history");
//            //        aWriter.WriteLine(" select tmpmod.HN_RID, tmpmod.FV_RID, tmpmod.TIME_ID,");
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            line += "COALESCE(COALESCE(tmpmod." + vp.DatabaseColumnName + ", tmphis." + vp.DatabaseColumnName + "), 0) " + vp.DatabaseColumnName + ", ";
//            //            aWriter.WriteLine(line);
//            //            line = _indent10;
						
//            //        }
//            //        count = 0;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "COALESCE(tmpmod." + vp.DatabaseColumnName + Include.cLockExtension + ", 0) " + vp.DatabaseColumnName + Include.cLockExtension;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            aWriter.WriteLine(line);
//            //            line = _indent10;
						
//            //        }
//            //        aWriter.WriteLine("    into  " + aTempTableName + "MOD2");
//            //        aWriter.WriteLine(" FROM " + aTempTableName + "MOD tmpmod");
//            //        aWriter.WriteLine(" left outer JOIN " + aTempTableName + "HISTORY tmphis  ON tmphis.HN_RID = tmpmod.HN_RID");
//            //        aWriter.WriteLine(" AND tmphis.FV_RID = tmpmod.FV_RID");
//            //        aWriter.WriteLine(" AND tmphis.TIME_ID = tmpmod.TIME_ID");

//            //        aWriter.WriteLine(" ");
//            //        aWriter.WriteLine(" -- remove duplicate rows from history table");
//            //        aWriter.WriteLine("  delete " + aTempTableName + "HISTORY ");
//            //        aWriter.WriteLine("    from " + aTempTableName + "HISTORY, " + aTempTableName + "MOD");
//            //        aWriter.WriteLine("    where " + aTempTableName + "HISTORY.HN_RID = " + aTempTableName + "MOD.HN_RID");
//            //        aWriter.WriteLine("         and " + aTempTableName + "HISTORY.FV_RID = " + aTempTableName + "MOD.FV_RID");
//            //        aWriter.WriteLine("         and " + aTempTableName + "HISTORY.TIME_ID = " + aTempTableName + "MOD.TIME_ID");
//            //        aWriter.WriteLine(" ");

//            //        aWriter.WriteLine(" --select * from " + aTempTableName + "HISTORY");

//            //        aWriter.WriteLine(" insert into " + aTempTableName + "MOD2");
//            //        aWriter.WriteLine(" select tmphis.HN_RID, tmphis.FV_RID, tmphis.TIME_ID,");
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            line += "COALESCE(tmphis." + vp.DatabaseColumnName + ", 0) " + vp.DatabaseColumnName + ", ";
//            //            aWriter.WriteLine(line);
//            //            line = _indent10;
//            //        }
//            //        count = 0;
//            //        foreach (VariableProfile vp in aDatabaseVariables)
//            //        {
//            //            ++count;
//            //            line += "0 as " + vp.DatabaseColumnName + Include.cLockExtension;
//            //            if (count < aDatabaseVariables.Count)
//            //            {
//            //                line += ", ";
//            //            }
//            //            aWriter.WriteLine(line);
//            //            line = _indent10;
//            //        }

//            //        aWriter.WriteLine(" from " + aTempTableName + "HISTORY tmphis");
//            //        aWriter.WriteLine(" ");
//            //        aWriter.WriteLine(" select * from " + aTempTableName + "MOD2 order by TIME_ID");
//            //        aWriter.WriteLine(" ");

//            //        aWriter.WriteLine(" OPTION (MAXDOP 1)");
					
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        //aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "')) > 0 DROP TABLE " + aTempTableName);
//            //        aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "2')) > 0 DROP TABLE " + aTempTableName + "2");
//            //        aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "MOD')) > 0 DROP TABLE " + aTempTableName + "MOD");
//            //        aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "MOD2')) > 0 DROP TABLE " + aTempTableName + "MOD2");
//            //        aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "HISTORY')) > 0 DROP TABLE " + aTempTableName + "HISTORY");
//            //        aWriter.WriteLine(System.Environment.NewLine);

//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine(System.Environment.NewLine);
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}
            
//            override public void Generate_SP_MID_XML_CHN_MOD_WK_ROLL_READ(string aProcedureName, 
//                string aForecastTableName, string aHistoryTableName, string aLockTableName,
//                string aForecastViewName, string aHistoryViewName, string aTempTableName, 
//                ProfileList aDatabaseVariables, StreamWriter aWriter)
//            {
//                try
//                {
//                    string procedureName = aProcedureName;
//                    string forecastTableName = aForecastTableName;
//                    string historyTableName = aHistoryTableName;
//                    string lockTableName = aLockTableName;

//                    AddStoredProcedureDrop(aProcedureName, aWriter);

//                    if (aDatabaseVariables == null ||
//                        aDatabaseVariables.Count == 0)
//                    {
//                        return;
//                    }

//                    string line = string.Empty;
//                    int count = 0;

//                    aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
//                    aWriter.WriteLine("@xmlDoc AS NTEXT,");
//                    aWriter.WriteLine("@Rollup CHAR(1) = NULL");
//                    aWriter.WriteLine("AS");
//                    aWriter.WriteLine("SET NOCOUNT ON");
//                    aWriter.WriteLine("DECLARE @idoc int,");
//                    aWriter.WriteLine("        @Tables INT,");
//                    aWriter.WriteLine("        @Loop INT,");
//                    aWriter.WriteLine("        @HN_RID INT,");
//                    aWriter.WriteLine("        @FV_RID INT,");
//                    aWriter.WriteLine("        @HN_MOD INT,");
//                    aWriter.WriteLine("        @LoopCount INT,");
//                    aWriter.WriteLine("        @NextLoopCount INT");
//                    aWriter.WriteLine("EXEC sp_xml_preparedocument @idoc OUTPUT, @xmlDoc");
//                    aWriter.WriteLine("SELECT @Tables = 1");

//                    aWriter.WriteLine("SELECT xmlHN_RID, xmlFV_RID, xmlTIME_ID, xmlHN_RID % @Tables AS xmlHN_MOD");
//                    aWriter.WriteLine("	INTO " + aTempTableName);
//                    aWriter.WriteLine("	FROM OPENXML (@idoc, '/root/node/time',2)");
//                    aWriter.WriteLine("         WITH (  xmlHN_RID  int      '../@ID',");
//                    aWriter.WriteLine("                 xmlFV_RID INT '../@FV_RID',");
//                    aWriter.WriteLine("                 xmlTIME_ID INT '@ID') xml");
					
//                    aWriter.WriteLine(" ");
//                    aWriter.WriteLine(" -- build temp table of values and locks for modified version");
//                    aWriter.WriteLine(" create table " + aTempTableName + "MOD ");
//                    aWriter.WriteLine(" (HN_RID  int not null,");
//                    aWriter.WriteLine("  FV_RID  int not null,");
//                    aWriter.WriteLine("  TIME_ID int not null,");
//                    count = 0;
//                    // add variables
//                    line = "  ";
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        line += "  " + vp.DatabaseColumnName;
//                        switch (vp.ChainDatabaseVariableType)
//                        {
//                            case eVariableDatabaseType.Integer:
//                                line += " int ";
//                                break;
//                            case eVariableDatabaseType.Real:
//                                line += " real ";
//                                break;
//                            case eVariableDatabaseType.DateTime:
//                                line += " datetime ";
//                                break;
//                            case eVariableDatabaseType.String:
//                                line += " varchar(100) ";
//                                break;
//                            case eVariableDatabaseType.Char:
//                                line += " char(1) ";
//                                break;
//                        }
//                        line += "  null,";
//                        aWriter.WriteLine(line);
//                        line = "  ";
//                    }
//                    count = 0;
//                    // add locks
//                    line = "  ";
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        line += "  " + vp.DatabaseColumnName + Include.cLockExtension + "   char(1) null";
//                        if (count < aDatabaseVariables.Count)
//                        {
//                            line += ", ";
//                        }
//                        else
//                        {
//                            line += ")";
//                        }
//                        aWriter.WriteLine(line);
//                        line = "  ";
						
//                    }
//                    if (line.Length > "  ".Length)
//                    {
//                        aWriter.WriteLine(line);
//                    }

//                    aWriter.WriteLine(" ");
//                    aWriter.WriteLine(" insert into " + aTempTableName + "MOD");
//                    aWriter.WriteLine("  select cfw.HN_RID, cfw.FV_RID, cfw.TIME_ID,");
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        line += "  cfw." + vp.DatabaseColumnName + ", ";
//                        if (line.Length > 80)
//                        {
//                            aWriter.WriteLine(line);
//                            line = "  ";
//                        }
						
//                    }
//                    if (line.Length > "  ".Length)
//                    {
//                        aWriter.WriteLine(line);
//                    }
//                    count = 0;
//                    line = "  ";
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        line += "  COALESCE(cfwl." + vp.DatabaseColumnName + Include.cLockExtension + ", 0) " + vp.DatabaseColumnName + Include.cLockExtension;
//                        if (count < aDatabaseVariables.Count)
//                        {
//                            line += ", ";
//                        }
//                        aWriter.WriteLine(line);
//                        line = "  ";
						
//                    }
     
//                    aWriter.WriteLine("   FROM " + aTempTableName + " t");
//                    aWriter.WriteLine("   JOIN " + aForecastTableName + " cfw (NOLOCK)");
//                    aWriter.WriteLine("    on cfw.HN_RID = t.xmlHN_RID");
//                    aWriter.WriteLine("    and cfw.FV_RID = t.xmlFV_RID");
//                    aWriter.WriteLine("    and cfw.TIME_ID = t.xmlTIME_ID");
//                    aWriter.WriteLine("   left outer join " + aLockTableName + " cfwl (NOLOCK)");
//                    aWriter.WriteLine("    on cfwl.HN_RID = cfw.HN_RID");
//                    aWriter.WriteLine("    and cfwl.FV_RID =cfw.FV_RID");
//                    aWriter.WriteLine("    and cfwl.TIME_ID = cfw.TIME_ID");
//                    aWriter.WriteLine("  union");
//                    aWriter.WriteLine("  select cfwl.HN_RID,  cfwl.FV_RID,  cfwl.TIME_ID, ");
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        line += "  cfw." + vp.DatabaseColumnName + ", ";
//                        if (line.Length > 80)
//                        {
//                            aWriter.WriteLine(line);
//                            line = "  ";
//                        }
						
//                    }
//                    if (line.Length > "  ".Length)
//                    {
//                        aWriter.WriteLine(line);
//                    }
//                    count = 0;
//                    line = "  ";
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        line += "  COALESCE(cfwl." + vp.DatabaseColumnName + Include.cLockExtension + ", 0) " + vp.DatabaseColumnName + Include.cLockExtension;
//                        if (count < aDatabaseVariables.Count)
//                        {
//                            line += ", ";
//                        }
//                        aWriter.WriteLine(line);
//                        line = "  ";
						
//                    }

//                    aWriter.WriteLine(" FROM " + aTempTableName + " t");
//                    aWriter.WriteLine(" JOIN " + aLockTableName + " cfwl (NOLOCK)");
//                    aWriter.WriteLine("  on cfwl.HN_RID = t.xmlHN_RID");
//                    aWriter.WriteLine("  and cfwl.FV_RID = t.xmlFV_RID");
//                    aWriter.WriteLine("  and cfwl.TIME_ID = t.xmlTIME_ID");
//                    aWriter.WriteLine(" left outer join " + aForecastTableName + " cfw (NOLOCK)");
//                    aWriter.WriteLine("  on cfw.HN_RID = cfwl.HN_RID");
//                    aWriter.WriteLine("  and cfw.FV_RID =cfwl.FV_RID");
//                    aWriter.WriteLine("  and cfw.TIME_ID = cfwl.TIME_ID");

//                    aWriter.WriteLine(" ");
//                    aWriter.WriteLine(" -- create temp table for history values");
//                    aWriter.WriteLine(" create table " + aTempTableName + "HISTORY ");
//                    aWriter.WriteLine(" (HN_RID  int not null,");
//                    aWriter.WriteLine("  FV_RID  int not null,");
//                    aWriter.WriteLine("  TIME_ID int not null,");
//                    count = 0;
//                    // add variables
//                    line = "  ";
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        line += "  " + vp.DatabaseColumnName;
//                        switch (vp.ChainDatabaseVariableType)
//                        {
//                            case eVariableDatabaseType.Integer:
//                                line += " int ";
//                                break;
//                            case eVariableDatabaseType.Real:
//                                line += " real ";
//                                break;
//                            case eVariableDatabaseType.DateTime:
//                                line += " datetime ";
//                                break;
//                            case eVariableDatabaseType.String:
//                                line += " varchar(100) ";
//                                break;
//                            case eVariableDatabaseType.Char:
//                                line += " char(1) ";
//                                break;
//                        }
//                        if (count < aDatabaseVariables.Count)
//                        {
//                            line += "null, ";
//                        }
//                        else
//                        {
//                            line += "null)";
//                        }
//                        aWriter.WriteLine(line);
//                        line = "  ";
//                    }
//                    aWriter.WriteLine(" ");

//                    aWriter.WriteLine(" -- alternate and real time roll");
//                    aWriter.WriteLine("   SET @LoopCount = 0");
//                    aWriter.WriteLine("   CREATE TABLE #TREE (LOOPCOUNT INT NOT NULL, PARENT_HN_RID INT NOT NULL, HOME_PH_RID INT, PH_TYPE INT, CHILD_HN_RID INT NOT NULL, CHILD_HN_MOD INT NOT NULL)");
//                    aWriter.WriteLine("   -- insert the children of the node into the temp table");
//                    aWriter.WriteLine("   INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, CHILD_HN_RID, CHILD_HN_MOD) ");
//                    aWriter.WriteLine("       select @LoopCount as LOOPCOUNT, @HN_RID as PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
//                    aWriter.WriteLine("         from HIER_NODE_JOIN hnj (NOLOCK)");
//                    aWriter.WriteLine("           JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//                    aWriter.WriteLine("           JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//                    aWriter.WriteLine("       where @HN_RID = hnj.PARENT_HN_RID");
//                    aWriter.WriteLine("   SET @Loop = @@ROWCOUNT");
//                    aWriter.WriteLine("   -- chase all paths until you get the main hierarchy (type 800000) or the lowest leaf");
//                    aWriter.WriteLine("   SET @NextLoopCount = @LoopCount + 1");
//                    aWriter.WriteLine("   WHILE @Loop > 0");
//                    aWriter.WriteLine("   BEGIN");
//                    aWriter.WriteLine("      INSERT #TREE");
//                    aWriter.WriteLine("        select @NextLoopCount as LOOPCOUNT, hnj.PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
//                    aWriter.WriteLine("          from HIER_NODE_JOIN hnj (NOLOCK)");
//                    aWriter.WriteLine("            JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//                    aWriter.WriteLine("            JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//                    aWriter.WriteLine("            JOIN #TREE t ON hnj.PARENT_HN_RID = t.CHILD_HN_RID");
//                    aWriter.WriteLine("        WHERE t.LOOPCOUNT =  @LoopCount AND t.PH_TYPE <> 800000");
//                    aWriter.WriteLine("      SET @Loop = @@ROWCOUNT");
//                    aWriter.WriteLine("      SET @LoopCount = @LoopCount + 1");
//                    aWriter.WriteLine("      SET @NextLoopCount = @LoopCount + 1");
//                    aWriter.WriteLine("   END");
//                    aWriter.WriteLine("   -- join with dates from xml selecting only nodes from the main hierarchy or lowest leaf alternates");
//                    aWriter.WriteLine("   SELECT * ");
//                    aWriter.WriteLine("     INTO " + aTempTableName + "2");
//                    aWriter.WriteLine("     FROM #TREE");
//                    aWriter.WriteLine("     CROSS JOIN " + aTempTableName);
//                    aWriter.WriteLine("   where LOOPCOUNT = @LoopCount - 1");
//                    aWriter.WriteLine("	     or PH_TYPE = 800000");

//                    count = 0;
//                    aWriter.WriteLine("       -- build temp table of summed history values ");
//                    aWriter.WriteLine("       insert into " + aTempTableName + "HISTORY ");
//                    aWriter.WriteLine("       select @HN_RID as HN_RID, @FV_RID as FV_RID, chw.TIME_ID, ");
//                    // add variables
//                    line = _indent10;
//                    int variables = 0;
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        if (vp.ChainDatabaseVariableType == eVariableDatabaseType.Integer ||
//                            vp.ChainDatabaseVariableType == eVariableDatabaseType.Real)
//                        {
//                            ++variables;
//                        }
						
//                    }
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        if (vp.ChainDatabaseVariableType == eVariableDatabaseType.Integer ||
//                            vp.ChainDatabaseVariableType == eVariableDatabaseType.Real)
//                        {
//                            ++count;
//                            line += "SUM(chw." + vp.DatabaseColumnName + ") " + vp.DatabaseColumnName;
//                            if (count < variables)
//                            {
//                                line += ", ";
//                            }
//                            aWriter.WriteLine(line);
//                            line = _indent10;
//                        }
//                    }
//                    aWriter.WriteLine("		FROM " + aTempTableName + "2 t");
//                    aWriter.WriteLine("	    JOIN " + aHistoryTableName + " chw (NOLOCK) ON t.CHILD_HN_RID = chw.HN_RID");
//                    aWriter.WriteLine("		    AND chw.TIME_ID = t.TIME_ID");
//                    aWriter.WriteLine("	    GROUP BY chw.TIME_ID");
//                    aWriter.WriteLine(" ");
					
//                    aWriter.WriteLine("  --select * from " + aTempTableName + "MOD");
//                    aWriter.WriteLine("  --select * from " + aTempTableName + "HISTORY");

//                    aWriter.WriteLine("   -- combine modified values with history");
//                    aWriter.WriteLine(" select tmpmod.HN_RID, tmpmod.FV_RID, tmpmod.TIME_ID,");
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        line += "COALESCE(COALESCE(tmpmod." + vp.DatabaseColumnName + ", tmphis." + vp.DatabaseColumnName + "), 0) " + vp.DatabaseColumnName + ", ";
//                        aWriter.WriteLine(line);
//                        line = _indent10;
						
//                    }
//                    count = 0;
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        line += "COALESCE(tmpmod." + vp.DatabaseColumnName + Include.cLockExtension + ", 0) " + vp.DatabaseColumnName + Include.cLockExtension;
//                        if (count < aDatabaseVariables.Count)
//                        {
//                            line += ", ";
//                        }
//                        aWriter.WriteLine(line);
//                        line = _indent10;
						
//                    }
//                    aWriter.WriteLine("    into  " + aTempTableName + "MOD2");
//                    aWriter.WriteLine(" FROM " + aTempTableName + "MOD tmpmod");
//                    aWriter.WriteLine(" left outer JOIN " + aTempTableName + "HISTORY tmphis  ON tmphis.HN_RID = tmpmod.HN_RID");
//                    aWriter.WriteLine(" AND tmphis.FV_RID = tmpmod.FV_RID");
//                    aWriter.WriteLine(" AND tmphis.TIME_ID = tmpmod.TIME_ID");

//                    aWriter.WriteLine(" ");
//                    aWriter.WriteLine(" -- remove duplicate rows from history table");
//                    aWriter.WriteLine("  delete " + aTempTableName + "HISTORY ");
//                    aWriter.WriteLine("    from " + aTempTableName + "HISTORY, " + aTempTableName + "MOD");
//                    aWriter.WriteLine("    where " + aTempTableName + "HISTORY.HN_RID = " + aTempTableName + "MOD.HN_RID");
//                    aWriter.WriteLine("         and " + aTempTableName + "HISTORY.FV_RID = " + aTempTableName + "MOD.FV_RID");
//                    aWriter.WriteLine("         and " + aTempTableName + "HISTORY.TIME_ID = " + aTempTableName + "MOD.TIME_ID");
//                    aWriter.WriteLine(" ");

//                    aWriter.WriteLine(" --select * from " + aTempTableName + "HISTORY");

//                    aWriter.WriteLine(" insert into " + aTempTableName + "MOD2");
//                    aWriter.WriteLine(" select tmphis.HN_RID, tmphis.FV_RID, tmphis.TIME_ID,");
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        line += "COALESCE(tmphis." + vp.DatabaseColumnName + ", 0) " + vp.DatabaseColumnName + ", ";
//                        aWriter.WriteLine(line);
//                        line = _indent10;
//                    }
//                    count = 0;
//                    foreach (VariableProfile vp in aDatabaseVariables)
//                    {
//                        ++count;
//                        line += "0 as " + vp.DatabaseColumnName + Include.cLockExtension;
//                        if (count < aDatabaseVariables.Count)
//                        {
//                            line += ", ";
//                        }
//                        aWriter.WriteLine(line);
//                        line = _indent10;
//                    }

//                    aWriter.WriteLine(" from " + aTempTableName + "HISTORY tmphis");
//                    aWriter.WriteLine(" ");
//                    aWriter.WriteLine(" select * from " + aTempTableName + "MOD2 order by TIME_ID");
//                    aWriter.WriteLine(" ");

//                    aWriter.WriteLine(" OPTION (MAXDOP 1)");
					
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            override public void Generate_SP_MID_ST_HIS_DAY_ROLLUP(ProfileList aDailyDatabaseVariables, ProfileList aWeeklyDatabaseVariables, StreamWriter aWriter, int aTableNumber)
//            {
//                StoreHistoryDayRollupProcess rollupProcess;
//                StoreDayWeekRollupProcess dayToWeekrollupProcess;
//                string procedureName;
//                bool includeZeroInAverage = true;
//                bool honorLocks = false;
//                bool zeroParentsWithNoChildren = true;
//                ArrayList databaseVariables;

//                try
//                {
//                    procedureName = Include.DBStoreDailyHistoryRollupSP.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//                    AddStoredProcedureDrop(procedureName, aWriter);
//                    rollupProcess = new StoreHistoryDayRollupProcess(null, null, aDailyDatabaseVariables.ArrayList, 0, 0, 0, 0, aTableNumber, aTableNumber, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
//                    aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);

//                    zeroParentsWithNoChildren = false;
//                    procedureName = Include.DBStoreDailyHistoryNoZeroRollupSP.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//                    AddStoredProcedureDrop(procedureName, aWriter);
//                    rollupProcess = new StoreHistoryDayRollupProcess(null, null, aDailyDatabaseVariables.ArrayList, 0, 0, 0, 0, aTableNumber, aTableNumber, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
//                    aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);

//                    zeroParentsWithNoChildren = true;
//                    procedureName = Include.DBStoreDayToWeekHistoryRollupSP.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//                    AddStoredProcedureDrop(procedureName, aWriter);
//                    databaseVariables = new ArrayList();
//                    Rollup.BuildIntersectionOfVariableLists(aDailyDatabaseVariables.ArrayList, aWeeklyDatabaseVariables.ArrayList, databaseVariables, eRollType.storeDailyHistoryToWeeks);
//                    dayToWeekrollupProcess = new StoreDayWeekRollupProcess(null, null, databaseVariables, 0, 0, 0, 0, aTableNumber, aTableNumber, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
//                    aWriter.Write(dayToWeekrollupProcess.BuildStoredProcedure(procedureName));
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            override public void Generate_SP_MID_ST_HIS_WK_ROLLUP(ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableNumber)
//            {
//                StoreHistoryWeekRollupProcess rollupProcess;
//                string procedureName;
//                bool includeZeroInAverage = true;
//                bool honorLocks = false;
//                bool zeroParentsWithNoChildren = true;

//                try
//                {
//                    procedureName = Include.DBStoreWeeklyHistoryRollupSP.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//                    AddStoredProcedureDrop(procedureName, aWriter);
//                    rollupProcess = new StoreHistoryWeekRollupProcess(null, null, aDatabaseVariables.ArrayList, 0, 0, 0, 0, aTableNumber, aTableNumber, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
//                    aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);

//                    zeroParentsWithNoChildren = false;
//                    procedureName = Include.DBStoreWeeklyHistoryNoZeroRollupSP.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//                    AddStoredProcedureDrop(procedureName, aWriter);
//                    rollupProcess = new StoreHistoryWeekRollupProcess(null, null, aDatabaseVariables.ArrayList, 0, 0, 0, 0, aTableNumber, aTableNumber, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
//                    aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            override public void Generate_SP_MID_ST_FOR_WK_ROLLUP(ProfileList aDatabaseVariables, StreamWriter aWriter, int aTableNumber)
//            {
//                StoreForecastWeekRollupProcess rollupProcess;
//                string procedureName;
//                bool includeZeroInAverage = true;
//                bool honorLocks = false;
//                bool zeroParentsWithNoChildren = true;

//                try
//                {
//                    procedureName = Include.DBStoreWeeklyForecastRollupSP.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//                    AddStoredProcedureDrop(procedureName, aWriter);
//                    rollupProcess = new StoreForecastWeekRollupProcess(null, null, aDatabaseVariables.ArrayList, 0, 0, 0, 0, aTableNumber, aTableNumber, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
//                    aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName, honorLocks));
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);

//                    zeroParentsWithNoChildren = false;
//                    procedureName = Include.DBStoreWeeklyForecastNoZeroRollupSP.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//                    AddStoredProcedureDrop(procedureName, aWriter);
//                    rollupProcess = new StoreForecastWeekRollupProcess(null, null, aDatabaseVariables.ArrayList, 0, 0, 0, 0, aTableNumber, aTableNumber, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
//                    aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName, honorLocks));
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);

//                    zeroParentsWithNoChildren = true;
//                    honorLocks = true;
//                    procedureName = Include.DBStoreWeeklyForecastHonorLocksRollupSP.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
//                    AddStoredProcedureDrop(procedureName, aWriter);
//                    rollupProcess = new StoreForecastWeekRollupProcess(null, null, aDatabaseVariables.ArrayList, 0, 0, 0, 0, aTableNumber, aTableNumber, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
//                    aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName, honorLocks));
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            override public void Generate_SP_MID_HIS_ST_OTHER_ROLLUPS(ProfileList aStoreDatabaseVariables, ProfileList aChainDatabaseVariables, StreamWriter aWriter)
//            {
//                StoreToChainHistoryRollupProcess rollupProcess;
//                StoreIntransitRollupProcess intransitRollupProcess;
//                StoreExternalIntransitRollupProcess extIntransitRollupProcess;
//                ArrayList databaseVariables;
//                string procedureName;
//                bool includeZeroInAverage = true;
//                bool honorLocks = false;
//                bool zeroParentsWithNoChildren = true;

//                try
//                {
//                    procedureName = Include.DBStoreToChainHistoryRollupSP;
//                    databaseVariables = new ArrayList();
//                    Rollup.BuildIntersectionOfVariableLists(aStoreDatabaseVariables.ArrayList, aChainDatabaseVariables.ArrayList, databaseVariables, eRollType.storeToChain);
//                    AddStoredProcedureDrop(procedureName, aWriter);
//                    rollupProcess = new StoreToChainHistoryRollupProcess(null, null, databaseVariables, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
//                    aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);

//                    zeroParentsWithNoChildren = false;
//                    procedureName = Include.DBStoreToChainHistoryNoZeroRollupSP;
//                    AddStoredProcedureDrop(procedureName, aWriter);
//                    rollupProcess = new StoreToChainHistoryRollupProcess(null, null, databaseVariables, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
//                    aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);

//                    zeroParentsWithNoChildren = true;
//                    honorLocks = false;
//                    procedureName = Include.DBStoreIntransitRollupSP;
//                    AddStoredProcedureDrop(procedureName, aWriter);
//                    intransitRollupProcess = new StoreIntransitRollupProcess(null, null, databaseVariables, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
//                    aWriter.Write(intransitRollupProcess.BuildStoredProcedure(procedureName));
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);

//                    procedureName = Include.DBStoreExternalIntransitRollupSP;
//                    AddStoredProcedureDrop(procedureName, aWriter);
//                    extIntransitRollupProcess = new StoreExternalIntransitRollupProcess(null, null, databaseVariables, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
//                    aWriter.Write(extIntransitRollupProcess.BuildStoredProcedure(procedureName));
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            override public void Generate_SP_MID_FOR_ST_TO_CHN_ROLLUP(ProfileList aStoreDatabaseVariables, ProfileList aChainDatabaseVariables, StreamWriter aWriter)
//            {
//                StoreToChainForecastRollupProcess rollupProcess;
//                ArrayList databaseVariables;
//                string procedureName;
//                bool includeZeroInAverage = true;
//                bool honorLocks = false;
//                bool zeroParentsWithNoChildren = true;

//                try
//                {
//                    procedureName = Include.DBStoreToChainForecastRollupSP;
//                    databaseVariables = new ArrayList();
//                    Rollup.BuildIntersectionOfVariableLists(aStoreDatabaseVariables.ArrayList, aChainDatabaseVariables.ArrayList, databaseVariables, eRollType.storeToChain);
//                    AddStoredProcedureDrop(procedureName, aWriter);
//                    rollupProcess = new StoreToChainForecastRollupProcess(null, null, databaseVariables, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
//                    aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);

//                    honorLocks = true;
//                    procedureName = Include.DBStoreToChainForecastHonorLocksRollupSP;
//                    databaseVariables = new ArrayList();
//                    Rollup.BuildIntersectionOfVariableLists(aStoreDatabaseVariables.ArrayList, aChainDatabaseVariables.ArrayList, databaseVariables, eRollType.storeToChain);
//                    AddStoredProcedureDrop(procedureName, aWriter);
//                    rollupProcess = new StoreToChainForecastRollupProcess(null, null, databaseVariables, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
//                    aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);

//                    zeroParentsWithNoChildren = false;
//                    honorLocks = false;
//                    procedureName = Include.DBStoreToChainForecastNoZeroRollupSP;
//                    AddStoredProcedureDrop(procedureName, aWriter);
//                    rollupProcess = new StoreToChainForecastRollupProcess(null, null, databaseVariables, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
//                    aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            override public void Generate_SP_MID_CHN_HIS_WK_ROLLUP(ProfileList aDatabaseVariables, StreamWriter aWriter)
//            {
//                ChainHistoryWeekRollupProcess rollupProcess;
//                string procedureName;
//                bool includeZeroInAverage = true;
//                bool honorLocks = false;
//                bool zeroParentsWithNoChildren = true;

//                try
//                {
//                    procedureName = Include.DBChainWeeklyHistoryRollupSP;
//                    AddStoredProcedureDrop(procedureName, aWriter);
//                    rollupProcess = new ChainHistoryWeekRollupProcess(null, null, aDatabaseVariables.ArrayList, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
//                    aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);

//                    zeroParentsWithNoChildren = false;
//                    procedureName = Include.DBChainWeeklyHistoryNoZeroRollupSP;
//                    AddStoredProcedureDrop(procedureName, aWriter);
//                    rollupProcess = new ChainHistoryWeekRollupProcess(null, null, aDatabaseVariables.ArrayList, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
//                    aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            override public void Generate_SP_MID_CHN_FOR_WK_ROLLUP(ProfileList aDatabaseVariables, StreamWriter aWriter)
//            {
//                ChainForecastWeekRollupProcess rollupProcess;
//                string procedureName;
//                bool includeZeroInAverage = true;
//                bool honorLocks = false;
//                bool zeroParentsWithNoChildren = true;

//                try
//                {
//                    procedureName = Include.DBChainWeeklyForecastRollupSP;
//                    AddStoredProcedureDrop(procedureName, aWriter);
//                    rollupProcess = new ChainForecastWeekRollupProcess(null, null, aDatabaseVariables.ArrayList, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
//                    aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName, honorLocks));
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);

//                    zeroParentsWithNoChildren = false;
//                    procedureName = Include.DBChainWeeklyForecastNoZeroRollupSP;
//                    AddStoredProcedureDrop(procedureName, aWriter);
//                    rollupProcess = new ChainForecastWeekRollupProcess(null, null, aDatabaseVariables.ArrayList, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
//                    aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName, honorLocks));
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);

//                    zeroParentsWithNoChildren = true;
//                    honorLocks = true;
//                    procedureName = Include.DBChainWeeklyForecastHonorLocksRollupSP;
//                    AddStoredProcedureDrop(procedureName, aWriter);
//                    rollupProcess = new ChainForecastWeekRollupProcess(null, null, aDatabaseVariables.ArrayList, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
//                    aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName, honorLocks));
//                    aWriter.WriteLine(System.Environment.NewLine);
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(System.Environment.NewLine);
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            //Begin TT#3445 -jsobek -Failure of Size Curve Generation
//            //override public void Generate_SP_MID_ST_HIS_SIZES_READ(VariableProfile aSalesTotalUnitsVariable, VariableProfile aSalesRegularUnitsVariable, VariableProfile aSalesPromoUnitsVariable, StreamWriter aWriter)
//            //{
//            //    string procedureName;

//            //    try
//            //    {
//            //        procedureName = Include.DBStoreHistorySizesReadSP;

//            //        AddStoredProcedureDrop(procedureName, aWriter);

//            //        aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
//            //        aWriter.WriteLine("@aHN_RID AS INT,");
//            //        aWriter.WriteLine("@aOLL_RID AS INT,");
//            //        aWriter.WriteLine("@aUseReg AS CHAR,");
//            //        aWriter.WriteLine("@aTimeDoc AS NTEXT");
//            //        aWriter.WriteLine("AS");
//            //        aWriter.WriteLine("BEGIN");
//            //        aWriter.WriteLine("DECLARE @idoc INT");
//            //        aWriter.WriteLine("DECLARE @Tables INT");
//            //        aWriter.WriteLine("DECLARE @Loop INT");
//            //        aWriter.WriteLine("DECLARE @HN_TYPE INT");
//            //        aWriter.WriteLine("DECLARE @HN_RID INT");
//            //        aWriter.WriteLine("DECLARE @LoopCount INT");
//            //        aWriter.WriteLine("DECLARE @NextLoopCount INT");
//            //        aWriter.WriteLine("DECLARE @SizeLevel INT");
//            //        aWriter.WriteLine("DECLARE @ColorLevel INT");
//            //        aWriter.WriteLine("DECLARE @CurrLevel INT");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("EXEC sp_xml_preparedocument @idoc OUTPUT, @aTimeDoc");
//            //        aWriter.WriteLine(" ");
//            //        aWriter.WriteLine("SELECT xmlTIME_ID");
//            //        aWriter.WriteLine("INTO #TEMP1");
//            //        aWriter.WriteLine("FROM OPENXML (@idoc, '/root/time', 2)");
//            //        aWriter.WriteLine("WITH (xmlTIME_ID INT '@ID')");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("SELECT DISTINCT @HN_TYPE = ph.PH_TYPE, @HN_RID = HN_RID");
//            //        aWriter.WriteLine("FROM HIERARCHY_NODE hn (NOLOCK)");
//            //        aWriter.WriteLine("JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//            //        aWriter.WriteLine("WHERE hn.HN_RID = @aHN_RID");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("--------------------- ");
//            //        aWriter.WriteLine("-- Get all size nodes");
//            //        aWriter.WriteLine("--------------------- ");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("SELECT @ColorLevel = PHL_SEQUENCE FROM PRODUCT_HIERARCHY_LEVELS WHERE PHL_TYPE = 800203");
//            //        aWriter.WriteLine("SELECT @SizeLevel = PHL_SEQUENCE FROM PRODUCT_HIERARCHY_LEVELS WHERE PHL_TYPE = 800204");
//            //        aWriter.WriteLine("SELECT @CurrLevel = HOME_LEVEL FROM HIERARCHY_NODE WHERE HN_RID = @HN_RID");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("-- Create the temp table");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("CREATE TABLE #TREE (LOOPCOUNT INT NOT NULL, PARENT_HN_RID INT NOT NULL, HOME_PH_RID INT, PH_TYPE INT, HN_RID INT NOT NULL, HOME_LEVEL INT NULL)");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("-- Insert the children of the node into the temp table");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("SET @LoopCount = 0");
//            //        aWriter.WriteLine("SET @CurrLevel = @CurrLevel + 1");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("IF @CurrLevel = @ColorLevel");
//            //        aWriter.WriteLine("BEGIN");
//            //        aWriter.WriteLine("INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, HN_RID, HOME_LEVEL)");
//            //        aWriter.WriteLine("SELECT @LoopCount AS LOOPCOUNT, @HN_RID AS PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hn.HOME_LEVEL");
//            //        aWriter.WriteLine("FROM HIER_NODE_JOIN hnj (NOLOCK)");
//            //        aWriter.WriteLine("JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//            //        aWriter.WriteLine("JOIN COLOR_NODE cn (NOLOCK) ON cn.HN_RID = hnj.HN_RID");
//            //        aWriter.WriteLine("JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//            //        aWriter.WriteLine("WHERE @HN_RID = hnj.PARENT_HN_RID AND");
//            //        aWriter.WriteLine("cn.COLOR_CODE_RID <> 0 AND");
//            //        aWriter.WriteLine("hnj.HN_RID NOT IN (SELECT HN_RID FROM OVERRIDE_LL_MODEL_DETAIL WHERE OLL_RID = @aOLL_RID AND EXCLUDE_IND = '1')");
//            //        aWriter.WriteLine("END");
//            //        aWriter.WriteLine("ELSE");
//            //        aWriter.WriteLine("BEGIN");
//            //        aWriter.WriteLine("INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, HN_RID, HOME_LEVEL)");
//            //        aWriter.WriteLine("SELECT @LoopCount AS LOOPCOUNT, @HN_RID AS PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hn.HOME_LEVEL");
//            //        aWriter.WriteLine("FROM HIER_NODE_JOIN hnj (NOLOCK)");
//            //        aWriter.WriteLine("JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//            //        aWriter.WriteLine("JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//            //        aWriter.WriteLine("WHERE @HN_RID = hnj.PARENT_HN_RID AND hnj.HN_RID NOT IN (SELECT HN_RID FROM OVERRIDE_LL_MODEL_DETAIL WHERE OLL_RID = @aOLL_RID AND EXCLUDE_IND = '1')");
//            //        aWriter.WriteLine("END");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("-- Chase all paths until you get the lowest leaf");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("SET @Loop = @@ROWCOUNT");
//            //        aWriter.WriteLine("SET @NextLoopCount = @LoopCount + 1");
//            //        aWriter.WriteLine("SET @CurrLevel = @CurrLevel + 1");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("WHILE @Loop > 0");
//            //        aWriter.WriteLine("BEGIN");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("IF @CurrLevel = @ColorLevel");
//            //        aWriter.WriteLine("BEGIN");
//            //        aWriter.WriteLine("INSERT #TREE");
//            //        aWriter.WriteLine("SELECT @NextLoopCount AS LOOPCOUNT, hnj.PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hn.HOME_LEVEL");
//            //        aWriter.WriteLine("FROM HIER_NODE_JOIN hnj (NOLOCK)");
//            //        aWriter.WriteLine("JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//            //        aWriter.WriteLine("JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//            //        aWriter.WriteLine("JOIN COLOR_NODE cn (NOLOCK) ON cn.HN_RID = hnj.HN_RID");
//            //        aWriter.WriteLine("JOIN #TREE t ON hnj.PARENT_HN_RID = t.HN_RID");
//            //        aWriter.WriteLine("WHERE t.LOOPCOUNT =  @LoopCount AND");
//            //        aWriter.WriteLine("cn.COLOR_CODE_RID <> 0 AND");
//            //        aWriter.WriteLine("hnj.HN_RID NOT IN (SELECT HN_RID FROM OVERRIDE_LL_MODEL_DETAIL WHERE OLL_RID = @aOLL_RID AND EXCLUDE_IND = '1')");
//            //        aWriter.WriteLine("END");
//            //        aWriter.WriteLine("ELSE");
//            //        aWriter.WriteLine("BEGIN");
//            //        aWriter.WriteLine("INSERT #TREE");
//            //        aWriter.WriteLine("SELECT @NextLoopCount AS LOOPCOUNT, hnj.PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hn.HOME_LEVEL");
//            //        aWriter.WriteLine("FROM HIER_NODE_JOIN hnj (NOLOCK)");
//            //        aWriter.WriteLine("JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
//            //        aWriter.WriteLine("JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
//            //        aWriter.WriteLine("JOIN #TREE t ON hnj.PARENT_HN_RID = t.HN_RID");
//            //        aWriter.WriteLine("WHERE t.LOOPCOUNT =  @LoopCount AND hnj.HN_RID NOT IN (SELECT HN_RID FROM OVERRIDE_LL_MODEL_DETAIL WHERE OLL_RID = @aOLL_RID AND EXCLUDE_IND = '1')");
//            //        aWriter.WriteLine("END");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("SET @Loop = @@ROWCOUNT");
//            //        aWriter.WriteLine("SET @LoopCount = @LoopCount + 1");
//            //        aWriter.WriteLine("SET @NextLoopCount = @LoopCount + 1");
//            //        aWriter.WriteLine("SET @CurrLevel = @CurrLevel + 1");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("END");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("-- Remove all but size nodes");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("DELETE FROM #TREE WHERE PH_TYPE <> 800000");
//            //        aWriter.WriteLine("DELETE FROM #TREE WHERE HOME_LEVEL <> @SizeLevel");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("-- join with dates from xml ");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("SELECT *");
//            //        aWriter.WriteLine("INTO #TEMP2");
//            //        aWriter.WriteLine("FROM #TREE");
//            //        aWriter.WriteLine("CROSS JOIN #TEMP1");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("---------------------- ");
//            //        aWriter.WriteLine("-- Get all size values");
//            //        aWriter.WriteLine("---------------------- ");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("IF @aUseReg = '1'");
//            //        aWriter.WriteLine("BEGIN");
//            //        aWriter.WriteLine("SELECT ST_RID, sn.SIZE_CODE_RID, SUM(COALESCE(" + aSalesRegularUnitsVariable.DatabaseColumnName + ", 0)+COALESCE(" + aSalesPromoUnitsVariable.DatabaseColumnName + ", 0)) SALES");
//            //        aWriter.WriteLine("INTO #TEMP3");
//            //        aWriter.WriteLine("FROM #TEMP2 t2");
//            //        aWriter.WriteLine("JOIN " + Include.DBStoreWeeklyHistoryView + " shw (NOLOCK) ON t2.HN_RID = shw.HN_RID AND shw.ST_RID > 0 AND shw.TIME_ID = t2.xmlTIME_ID");
//            //        aWriter.WriteLine("JOIN SIZE_NODE sn (NOLOCK) ON t2.HN_RID = sn.HN_RID");
//            //        aWriter.WriteLine("GROUP BY ST_RID, sn.SIZE_CODE_RID");
//            //        aWriter.WriteLine("OPTION (MAXDOP 1)");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("SELECT ST_RID, SIZE_CODE_RID, SALES FROM #TEMP3");
//            //        aWriter.WriteLine("WHERE SALES <> 0");
//            //        aWriter.WriteLine("ORDER BY ST_RID, SIZE_CODE_RID");
//            //        aWriter.WriteLine("END");
//            //        aWriter.WriteLine("ELSE");
//            //        aWriter.WriteLine("BEGIN");
//            //        aWriter.WriteLine("SELECT ST_RID, sn.SIZE_CODE_RID, SUM(COALESCE(" + aSalesTotalUnitsVariable.DatabaseColumnName + ", 0)) SALES");
//            //        aWriter.WriteLine("INTO #TEMP4");
//            //        aWriter.WriteLine("FROM #TEMP2 t2");
//            //        aWriter.WriteLine("JOIN " + Include.DBStoreWeeklyHistoryView + " shw (NOLOCK) ON t2.HN_RID = shw.HN_RID AND shw.ST_RID > 0 AND shw.TIME_ID = t2.xmlTIME_ID");
//            //        aWriter.WriteLine("JOIN SIZE_NODE sn (NOLOCK) ON t2.HN_RID = sn.HN_RID");
//            //        aWriter.WriteLine("GROUP BY ST_RID, sn.SIZE_CODE_RID");
//            //        aWriter.WriteLine("OPTION (MAXDOP 1)");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("SELECT ST_RID, SIZE_CODE_RID, SALES FROM #TEMP4");
//            //        aWriter.WriteLine("WHERE SALES <> 0");
//            //        aWriter.WriteLine("ORDER BY ST_RID, SIZE_CODE_RID");
//            //        aWriter.WriteLine("END");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("RETURN 0");
//            //        aWriter.WriteLine("END");
//            //        aWriter.WriteLine("");
//            //        aWriter.WriteLine("GO");
//            //        aWriter.WriteLine("");
//            //    }
//            //    catch
//            //    {
//            //        throw;
//            //    }
//            //}
//            //End TT#3445 -jsobek -Failure of Size Curve Generation
            
//            //Begin TT#739-MD -jsobek -Delete Stores 

//            //Begin TT#3456 -jsobek -Size Day To Week Failure
//            //override public void Generate_UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE_DAY(string aFunctionName, StreamWriter aWriter, int aTableNumber)
//            //{
//            //    aFunctionName = aFunctionName.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());

//            //    AddTableValuedFunctionDrop(aFunctionName, aWriter);

//            //    aWriter.WriteLine("CREATE FUNCTION [dbo].[" + aFunctionName + "]");
//            //    aWriter.WriteLine("(	");
//            //    aWriter.WriteLine("	@START_TIME INT, @END_TIME INT, @SELECTED_NODE_RID INT, @STORE_RID INT=-1 ");
//            //    aWriter.WriteLine(") ");
//            //    aWriter.WriteLine("RETURNS TABLE ");
//            //    aWriter.WriteLine("AS ");
//            //    aWriter.WriteLine("RETURN  ");
//            //    aWriter.WriteLine("( ");
//            //    aWriter.WriteLine("	SELECT q.* FROM ");
//            //    aWriter.WriteLine("	( ");
//            //    aWriter.WriteLine("		SELECT  ");
//            //    aWriter.WriteLine("				HN_RID, TIME_ID, ST_RID,  ");
//            //    aWriter.WriteLine("				SALES, ");
//            //    aWriter.WriteLine("				SALES_REG, ");
//            //    aWriter.WriteLine("				SALES_PROMO, ");
//            //    aWriter.WriteLine("				SALES_MKDN, ");
//            //    aWriter.WriteLine("				STOCK, ");
//            //    aWriter.WriteLine("				STOCK_REG, ");
//            //    aWriter.WriteLine("				STOCK_MKDN ");
//            //    aWriter.WriteLine("		FROM [dbo].[STORE_HISTORY_DAY" + aTableNumber.ToString() + "] WHERE TIME_ID BETWEEN @START_TIME AND @END_TIME ");
//            //    aWriter.WriteLine("	) AS q   --nested query ");
//            //    aWriter.WriteLine("	INNER JOIN [dbo].[UDF_STORE_GET_ACTIVE_RIDS]() st ON st.ST_RID=q.ST_RID AND (@STORE_RID=-1 OR st.ST_RID=@STORE_RID) ");
//            //    aWriter.WriteLine("	INNER JOIN [dbo].[UDF_HIERARCHY_GET_ACTIVE_SIZE_DESCENDANT_NODE_RIDS](@SELECTED_NODE_RID) dn ON dn.HN_RID=q.HN_RID ");
//            //    aWriter.WriteLine(" ");
//            //    aWriter.WriteLine("	--INNER JOIN HIERARCHY_NODE hn on coalesce(hn.ACTIVE_IND, 1)=1 AND hn.HN_RID=q.HN_RID  ");
//            //    aWriter.WriteLine("	--INNER JOIN [dbo].[HIER_NODE_JOIN] hnj ON hnj.HN_RID= hn.HN_RID AND hnj.PH_RID=hn.HOME_PH_RID ");
//            //    aWriter.WriteLine("	--INNER JOIN PRODUCT_HIERARCHY_LEVELS phl ON phl.PHL_SEQUENCE=hn.HOME_LEVEL ");
//            //    aWriter.WriteLine("	--WHERE phl.PHL_TYPE=800204 ");
//            //    aWriter.WriteLine("	----Exclude sizes from hidden 'Unknown Color' colors ");
//            //    aWriter.WriteLine("	--AND hnj.PARENT_HN_RID NOT IN  ");
//            //    aWriter.WriteLine("	--( ");
//            //    aWriter.WriteLine("	--	SELECT hn.HN_RID ");
//            //    aWriter.WriteLine("	--	FROM [dbo].[HIERARCHY_NODE] hn ");
//            //    aWriter.WriteLine("	--	INNER JOIN PRODUCT_HIERARCHY_LEVELS phl ON phl.PHL_SEQUENCE=hn.HOME_LEVEL ");
//            //    aWriter.WriteLine("	--	INNER JOIN COLOR_NODE cn ON cn.HN_RID=hn.HN_RID AND cn.COLOR_CODE_RID = 0 ");
//            //    aWriter.WriteLine("	--	WHERE coalesce(hn.ACTIVE_IND, 1)=1 AND phl.PHL_TYPE=800203  ");
//            //    aWriter.WriteLine("	--) ");
//            //    aWriter.WriteLine(" ");
//            //    aWriter.WriteLine(") ");
//            //    aWriter.WriteLine(" ");
//            //    aWriter.WriteLine("GO ");
//            //    aWriter.WriteLine("");

//            //}

            
//            //override public void Generate_UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE(StreamWriter aWriter, int maxTableNumber)
//            //{
//            //    string functionName;
                
//            //    functionName = Include.DBStoreHistoryGetSizeDataFromNode;

//            //    AddTableValuedFunctionDrop(functionName, aWriter);

//            //    aWriter.WriteLine("--dv ============================================= ");
//            //    aWriter.WriteLine("--dv Author:		Jeff Sobek ");
//            //    aWriter.WriteLine("--dv Create date: 5/7/2013 ");
//            //    aWriter.WriteLine("--dv Description:	Retrieves both rid and variable data within store day history withing the specified time period ");
//            //    aWriter.WriteLine("--dv				Start and end times are in Julian format (YYYYDDD) where DDD=0 at the begining of this year ");
//            //    aWriter.WriteLine("--dv				Inclusive of both the start_time and end_time ");
//            //    aWriter.WriteLine("--dv				Only includes active stores and active nodes ");
//            //    aWriter.WriteLine("--dv				Excludes the hidden 'Unknown Colors' colors (COLOR_CODE_RID=0) and their descendant size nodes  ");
//            //    aWriter.WriteLine("--dv Notes:		doing a nested query to join on the fewest rows possible ");
//            //    aWriter.WriteLine("--dv Benchmark1:	(Just Day0 joined with active stores) On midretail29:DotsProdDev, 7 days(2013027 to 2013034) brought back ~3,200,000 rows in 27 seconds ");
//            //    aWriter.WriteLine("--dv Benchmark2:	(Just Day0 joined with active stores and active size nodes) On midretail29:DotsProdDev, 7 days(2013027 to 2013034) brought back ~1,500,000 rows in 12 seconds ");
//            //    aWriter.WriteLine("--dv Benchmark3:	(Day0 thru Day9 joined with active stores and active size nodes) On midretail29:DotsProdDev, 7 days(2013027 to 2013034) brought back ~15,200,000 rows in 6 minutes 31 seconds ");
//            //    aWriter.WriteLine("--dv Benchmark4:	(Day0 thru Day9 joined with active stores and active size nodes separately) On midretail29:DotsProdDev, 7 days(2013027 to 2013034) brought back ~15,200,000 rows in 2 minutes 9 seconds (as stored procs it took 6 minutes 31 seconds) ");
//            //    aWriter.WriteLine("--dv Benchmark5:	(Just Day0 joined with active stores and active size nodes without hidden colors joining on UDF and specifying starting node of 101) On midretail29:DotsProdDev, 7 days(2013027 to 2013034) brought back ~523,000 rows in 22 seconds ");
//            //    aWriter.WriteLine("--dv Benchmark6:	(Just Day0 joined with active stores and active size nodes without hidden colors joining on tables directly without specifying starting node) On midretail29:DotsProdDev, 7 days(2013027 to 2013034) brought back ~1,523,000 rows in 42 seconds (so slower) ");
//            //    aWriter.WriteLine("--dv Benchmark7:	(Just Day0 joined with active stores and active size nodes without hidden colors joining on UDF and specifying starting node of 101, specifiy columns needed) On midretail29:DotsProdDev, 7 days(2013027 to 2013034) brought back ~523,000 rows in 4 seconds (2nd pass) ");
//            //    aWriter.WriteLine("--dv Benchmark8:	(Day0 thru Day9 joined with active stores and active size nodes without hidden colors joining on UDF and specifying starting node of 101, specifiy columns needed) On midretail29:DotsProdDev, 7 days(2013027 to 2013034) brought back ~4,900,000 rows in 4 minutes 39 seconds (1st pass) ");
//            //    aWriter.WriteLine("--dv ============================================= ");
//            //    aWriter.WriteLine("CREATE FUNCTION [dbo].[UDF_STORE_HISTORY_DAY_GET_DATA_IN_TIME_PERIOD_FROM_NODE] ");
//            //    aWriter.WriteLine("(	 ");
//            //    aWriter.WriteLine("	@START_TIME INT, @END_TIME INT, @SELECTED_NODE_RID INT, @STORE_RID INT = -1 ");
//            //    aWriter.WriteLine(") ");
//            //    aWriter.WriteLine("RETURNS TABLE  ");
//            //    aWriter.WriteLine("AS ");
//            //    aWriter.WriteLine("RETURN  ");
//            //    aWriter.WriteLine("( ");
//            //    aWriter.WriteLine("		      SELECT * FROM [dbo].[UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE_DAY0](@START_TIME,@END_TIME,@SELECTED_NODE_RID,@STORE_RID) ");
//            //    for (int i = 1; i <= maxTableNumber; i++)  //for the case of 10 total tables, 0...9, expecting maxTableNumber to be 9
//            //    {
//            //        aWriter.WriteLine("	UNION ALL SELECT * FROM [dbo].[UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE_DAY" + i.ToString() + "](@START_TIME,@END_TIME,@SELECTED_NODE_RID,@STORE_RID) ");
//            //    }
//            //    aWriter.WriteLine(")");
//            //    aWriter.WriteLine("");
//            //    aWriter.WriteLine("GO");
//            //    aWriter.WriteLine("");

//            //}

//            // Begin TT#3496 - JSmith - Database Upgrade failed for Version 5.21 post conversion
//            //override public void Generate_VW_HIERARCHY_ACTIVE_ORGANIZATIONAL_NODES(StreamWriter aWriter, int maxTableNumber)
//            //{

//            //    AddViewDrop("VW_HIERARCHY_ACTIVE_ORGANIZATIONAL_NODES", aWriter);


//            //    aWriter.WriteLine("--dv =============================================");
//            //    aWriter.WriteLine("--dv Create date: 1/15/2013");
//            //    aWriter.WriteLine("--dv Description:	Retrieves all active organizational hierarchy nodes");
//            //    aWriter.WriteLine("--dv =============================================");
//            //    aWriter.WriteLine("CREATE VIEW [dbo].[VW_HIERARCHY_ACTIVE_ORGANIZATIONAL_NODES]");
//            //    aWriter.WriteLine("AS");
//            //    aWriter.WriteLine("");
//            //    aWriter.WriteLine("	SELECT ");
//            //    aWriter.WriteLine("		HN_RID, ");
//            //    aWriter.WriteLine("		HN_TYPE, ");
//            //    aWriter.WriteLine("		HN_RID % 10 as HN_MOD ");
//            //    aWriter.WriteLine("	FROM dbo.HIERARCHY_NODE WITH (NOLOCK, INDEX(HIERARCHY_NODE_PK))  ");
//            //    aWriter.WriteLine("	WHERE HOME_PH_RID = (SELECT PH_RID FROM dbo.PRODUCT_HIERARCHY WITH (NOLOCK) WHERE PH_TYPE = 800000) ");
//            //    aWriter.WriteLine("		  AND COALESCE(ACTIVE_IND, 1)=1");
//            //    aWriter.WriteLine("");
//            //    aWriter.WriteLine("GO");
//            //    aWriter.WriteLine("");

//            //}

//            //override public void Generate_UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE(StreamWriter aWriter, int maxTableNumber)
//            //{

//            //    AddTableValuedFunctionDrop("UDF_HIERARCHY_GET_ACTIVE_SIZE_DESCENDANT_NODE_RIDS", aWriter);


//            //    aWriter.WriteLine("--dv =============================================");
//            //    aWriter.WriteLine("--dv Create date: 5/8/2013");
//            //    aWriter.WriteLine("--dv Modified:	1/15/2014");
//            //    aWriter.WriteLine("--dv Description:	Retrieves active descendant size nodes from a starting node");
//            //    aWriter.WriteLine("--dv				Excludes the hidden Unknown Colors colors (COLOR_CODE_RID=0)");
//            //    aWriter.WriteLine("--dv History: 1/15/2014 Added HN_MOD to the return set, using organizational view, removed PRODUCT_HIERARCHY_LEVELS join");
//            //    aWriter.WriteLine("--dv =============================================");
//            //    aWriter.WriteLine("CREATE FUNCTION [dbo].[UDF_HIERARCHY_GET_ACTIVE_SIZE_DESCENDANT_NODE_RIDS] ");
//            //    aWriter.WriteLine("(	");
//            //    aWriter.WriteLine("	@SELECTED_NODE_RID INT");
//            //    aWriter.WriteLine(")");
//            //    aWriter.WriteLine("RETURNS TABLE ");
//            //    aWriter.WriteLine("AS");
//            //    aWriter.WriteLine("RETURN ");
//            //    aWriter.WriteLine("(");
//            //    aWriter.WriteLine("     --Using a common table expression");
//            //    aWriter.WriteLine("	    WITH cte_descendants(HN_RID, PH_RID, PARENT_HN_RID, HN_TYPE)");
//            //    aWriter.WriteLine("	    AS (");
//            //    aWriter.WriteLine("		    SELECT");
//            //    aWriter.WriteLine("			    hn.HN_RID, hnj.PH_RID, hnj.PARENT_HN_RID, hn.HN_TYPE");
//            //    aWriter.WriteLine("		    FROM [dbo].[HIER_NODE_JOIN] hnj ");
//            //    aWriter.WriteLine("		    INNER JOIN [dbo].[VW_HIERARCHY_ACTIVE_ORGANIZATIONAL_NODES] hn ON hn.HN_RID = hnj.HN_RID --AND hn.HOME_PH_RID=hnj.PH_RID AND coalesce(hn.ACTIVE_IND, 1)=1");
//            //    aWriter.WriteLine("		    WHERE hn.HN_RID=@SELECTED_NODE_RID");
//            //    aWriter.WriteLine("		    UNION ALL");
//            //    aWriter.WriteLine("		    SELECT ");
//            //    aWriter.WriteLine("			    hn.HN_RID, hnj.PH_RID, hnj.PARENT_HN_RID, hn.HN_TYPE");
//            //    aWriter.WriteLine("		    FROM [dbo].[HIER_NODE_JOIN] hnj ");
//            //    aWriter.WriteLine("		    INNER JOIN [dbo].[VW_HIERARCHY_ACTIVE_ORGANIZATIONAL_NODES] hn ON hn.HN_RID = hnj.HN_RID --AND hn.HOME_PH_RID=hnj.PH_RID AND coalesce(hn.ACTIVE_IND, 1)=1");
//            //    aWriter.WriteLine("		    INNER JOIN cte_descendants AS cte ON hnj.PARENT_HN_RID= cte.HN_RID AND hnj.PH_RID=cte.PH_RID");
//            //    aWriter.WriteLine("		    )");
//            //    aWriter.WriteLine("	    SELECT cte.HN_RID, cte.HN_RID % 10 as HN_MOD");
//            //    aWriter.WriteLine("	    FROM cte_descendants cte ");
//            //    aWriter.WriteLine("	    --INNER JOIN [dbo].[PRODUCT_HIERARCHY_LEVELS] phl ON phl.PHL_SEQUENCE=cte.HOME_LEVEL");
//            //    aWriter.WriteLine("	    --WHERE phl.PHL_TYPE=800204");
//            //    aWriter.WriteLine("	    WHERE cte.HN_TYPE=800204 --sizes only");
//            //    aWriter.WriteLine("	    --Exclude sizes from hidden Unknown Color colors");
//            //    aWriter.WriteLine("	    AND cte.PARENT_HN_RID NOT IN ");
//            //    aWriter.WriteLine("	    (");
//            //    aWriter.WriteLine("		    SELECT hn.HN_RID");
//            //    aWriter.WriteLine("		    FROM [dbo].[HIERARCHY_NODE] hn");
//            //    aWriter.WriteLine("		    --INNER JOIN [dbo].[PRODUCT_HIERARCHY_LEVELS] phl ON phl.PHL_SEQUENCE=hn.HOME_LEVEL");
//            //    aWriter.WriteLine("		    INNER JOIN [dbo].[COLOR_NODE] cn ON cn.HN_RID=hn.HN_RID AND cn.COLOR_CODE_RID = 0");
//            //    aWriter.WriteLine("		    WHERE coalesce(hn.ACTIVE_IND, 1)=1 ");
//            //    aWriter.WriteLine("	    )");

//            //    aWriter.WriteLine(")");
//            //    aWriter.WriteLine("");
//            //    aWriter.WriteLine("GO");
//            //    aWriter.WriteLine("");

//            //}
//            // End TT#3496 - JSmith - Database Upgrade failed for Version 5.21 post conversion

         

//            // Begin TT#3507 - JSmith - Database Error during Upgrade
//            //override public void Generate_UDF_STORE_GET_ACTIVE_RIDS(StreamWriter aWriter)
//            //{

//            //    AddTableValuedFunctionDrop("UDF_STORE_GET_ACTIVE_RIDS", aWriter);

//            //    aWriter.WriteLine("--dv ============================================= ");
//            //    aWriter.WriteLine("--dv Create date: 5/7/2013");
//            //    aWriter.WriteLine("--dv Description:	Gets a list of active store rids");
//            //    aWriter.WriteLine("--dv =============================================");
//            //    aWriter.WriteLine("CREATE FUNCTION [dbo].[UDF_STORE_GET_ACTIVE_RIDS]");
//            //    aWriter.WriteLine("(	");
//            //    aWriter.WriteLine(")");
//            //    aWriter.WriteLine("RETURNS TABLE ");
//            //    aWriter.WriteLine("AS");
//            //    aWriter.WriteLine("RETURN ");
//            //    aWriter.WriteLine("(");
//            //    aWriter.WriteLine("	SELECT ST_RID");
//            //    aWriter.WriteLine("	FROM [dbo].[STORES] st");
//            //    aWriter.WriteLine("	  WHERE st.ACTIVE_IND=1 ");
//            //    aWriter.WriteLine(")");
//            //    aWriter.WriteLine("");
//            //    aWriter.WriteLine("GO");
//            //    aWriter.WriteLine("");

//            //}
//            // End TT#3507 - JSmith - Database Error during Upgrade
//            //End TT#3456 -jsobek -Size Day To Week Failure

           
                //Begin TT#1076-MD -jsobek -Size Day to Week Summary - Positive Sales Option & Color Level Fields
                //aWriter.WriteLine("IF .dbo.UDF_ColumnAlreadyExists('SYSTEM_OPTIONS', 'USE_NET_SALES_IN_SIZE_DAY_TO_WEEK_SUMMARY')=0");
                //aWriter.WriteLine("	BEGIN");
                //aWriter.WriteLine("	  ALTER TABLE SYSTEM_OPTIONS ADD USE_NET_SALES_IN_SIZE_DAY_TO_WEEK_SUMMARY char(1) default '0' null ");
                //aWriter.WriteLine(" END");
                //aWriter.WriteLine("");
                //aWriter.WriteLine("GO");
                //aWriter.WriteLine("");
                ////End TT#1076-MD -jsobek -Size Day to Week Summary - Positive Sales Option & Color Level Fields
                //aWriter.WriteLine("--dv History:	3/5/2014 TT#3566 -Stock is being summed instead of using the Begining of Week Stock  ");

                ////Begin TT#1076-MD -jsobek -Size Day to Week Summary - Positive Sales Option & Color Level Fields
                //aWriter.WriteLine("--Get option to use either positive sales or net sales ");
                //aWriter.WriteLine("DECLARE @USE_NET_SALES varchar(1); ");
                //aWriter.WriteLine("SELECT @USE_NET_SALES=USE_NET_SALES_IN_SIZE_DAY_TO_WEEK_SUMMARY FROM SYSTEM_OPTIONS; ");
                //aWriter.WriteLine("DECLARE @STORE_TABLE_COUNT int; ");
                //aWriter.WriteLine("SELECT @STORE_TABLE_COUNT=STORE_TABLE_COUNT FROM SYSTEM_OPTIONS ");
                ////End TT#1076-MD -jsobek -Size Day to Week Summary - Positive Sales Option & Color Level Fields

                //aWriter.WriteLine("INSERT INTO @PROCESS_SUMMARY SELECT 'Parameter @START_TIME= ' + CAST(@START_TIME AS VARCHAR(50)), 0, 0, null, null ");
                //aWriter.WriteLine("INSERT INTO @PROCESS_SUMMARY SELECT 'Parameter @END_TIME= ' + CAST(@END_TIME AS VARCHAR(50)), 0, 0, null, null ");
                //aWriter.WriteLine("INSERT INTO @PROCESS_SUMMARY SELECT 'Parameter @SELECTED_NODE_RID= ' + CAST(@SELECTED_NODE_RID AS VARCHAR(50)), 0, 0, null, null ");
                //aWriter.WriteLine("INSERT INTO @PROCESS_SUMMARY SELECT 'Parameter @STORE_RID= ' + CAST(@STORE_RID AS VARCHAR(50)), 0, 0, null, null ");
                //aWriter.WriteLine("SELECT @STORE_TABLE_COUNT=STORE_TABLE_COUNT FROM SYSTEM_OPTIONS ");

      

                //Begin TT#1076-MD -jsobek -Size Day to Week Summary - Positive Sales Option & Color Level Fields
                //Begin TT#3482-MD -jsobek -In Stock Sales - negative oh hands are not treated as 0.  In a store multi level view the all store, set and High and Low Levels are not populating for the In stock variables.
                //aWriter.WriteLine("				CASE WHEN coalesce(SALES,0) <> 0 THEN coalesce(SALES,0) ELSE coalesce(SALES_REG,0) + coalesce(SALES_MKDN,0) + coalesce(SALES_PROMO,0) END AS SALES, ");
                //aWriter.WriteLine("				SALES_REG, ");
                //aWriter.WriteLine("				SALES_PROMO, ");
                //aWriter.WriteLine("				SALES_MKDN, ");
                //aWriter.WriteLine("				--Equal to STOCK if not = to 0, else equal to STOCK_REG + STOCK_MKDN ");
                //aWriter.WriteLine("				CASE WHEN coalesce(STOCK,0) <> 0 THEN coalesce(STOCK,0) ELSE coalesce(STOCK_REG,0) + coalesce(STOCK_MKDN,0) END AS STOCK, ");
                //aWriter.WriteLine("				STOCK_REG, ");
                //aWriter.WriteLine("				STOCK_MKDN, ");
                //aWriter.WriteLine("				CASE WHEN coalesce(CASE WHEN SALES < 0 THEN 0 ELSE SALES END,0) <> 0 THEN coalesce(CASE WHEN SALES < 0 THEN 0 ELSE SALES END,0) ELSE coalesce(CASE WHEN SALES_REG < 0 THEN 0 ELSE SALES_REG END,0) + coalesce(CASE WHEN SALES_MKDN < 0 THEN 0 ELSE SALES_MKDN END,0) + coalesce(CASE WHEN SALES_PROMO < 0 THEN 0 ELSE SALES_PROMO END,0) END AS SALES, ");
                //aWriter.WriteLine("				CASE WHEN SALES_REG < 0 THEN 0 ELSE SALES_REG END AS SALES_REG, ");
                //aWriter.WriteLine("				CASE WHEN SALES_PROMO < 0 THEN 0 ELSE SALES_PROMO END AS SALES_PROMO, ");
                //aWriter.WriteLine("				CASE WHEN SALES_MKDN < 0 THEN 0 ELSE SALES_MKDN END AS SALES_MKDN, ");
                //aWriter.WriteLine("				--Equal to STOCK if not = to 0, else equal to STOCK_REG + STOCK_MKDN ");
                //aWriter.WriteLine("				CASE WHEN coalesce(CASE WHEN STOCK < 0 THEN 0 ELSE STOCK END,0) <> 0 THEN coalesce(CASE WHEN STOCK < 0 THEN 0 ELSE STOCK END,0) ELSE coalesce(CASE WHEN STOCK_REG < 0 THEN 0 ELSE STOCK_REG END,0) + coalesce(CASE WHEN STOCK_MKDN < 0 THEN 0 ELSE STOCK_MKDN END,0) END AS STOCK, ");
                //aWriter.WriteLine("				CASE WHEN STOCK_REG < 0 THEN 0 ELSE STOCK_REG END AS STOCK_REG, ");
                //aWriter.WriteLine("				CASE WHEN STOCK_MKDN < 0 THEN 0 ELSE STOCK_MKDN END AS STOCK_MKDN, ");
                //End TT#3482-MD -jsobek -In Stock Sales - negative oh hands are not treated as 0.  In a store multi level view the all store, set and High and Low Levels are not populating for the In stock variables.

//                aWriter.WriteLine("--Equal to SALES if not = to 0, else equal to SALES_REG + SALES_MKDN + SALES_PROMO  ");
//                aWriter.WriteLine("CASE WHEN @USE_NET_SALES='1' THEN ");
//                aWriter.WriteLine("	CASE WHEN coalesce(SALES,0) <> 0 THEN coalesce(SALES,0) ELSE coalesce(SALES_REG,0) + coalesce(SALES_MKDN,0) + coalesce(SALES_PROMO,0) END ");
//                aWriter.WriteLine("ELSE ");
//                aWriter.WriteLine("	CASE WHEN coalesce(CASE WHEN SALES < 0 THEN 0 ELSE SALES END,0) <> 0 THEN coalesce(CASE WHEN SALES < 0 THEN 0 ELSE SALES END,0) ELSE coalesce(CASE WHEN SALES_REG < 0 THEN 0 ELSE SALES_REG END,0) + coalesce(CASE WHEN SALES_MKDN < 0 THEN 0 ELSE SALES_MKDN END,0) + coalesce(CASE WHEN SALES_PROMO < 0 THEN 0 ELSE SALES_PROMO END,0) END  ");
//                aWriter.WriteLine("END AS SALES, ");
//                aWriter.WriteLine(" ");
//                aWriter.WriteLine("CASE WHEN @USE_NET_SALES='1' THEN ");
//                aWriter.WriteLine("	SALES_REG ");
//                aWriter.WriteLine("ELSE ");
//                aWriter.WriteLine("	CASE WHEN SALES_REG < 0 THEN 0 ELSE SALES_REG END ");
//                aWriter.WriteLine("END AS SALES_REG, ");
//                aWriter.WriteLine(" ");
//                aWriter.WriteLine("CASE WHEN @USE_NET_SALES='1' THEN ");
//                aWriter.WriteLine("	SALES_PROMO ");
//                aWriter.WriteLine("ELSE ");
//                aWriter.WriteLine("	CASE WHEN SALES_PROMO < 0 THEN 0 ELSE SALES_PROMO END ");
//                aWriter.WriteLine("END AS SALES_PROMO, ");
//                aWriter.WriteLine(" ");
//                aWriter.WriteLine("CASE WHEN @USE_NET_SALES='1' THEN ");
//                aWriter.WriteLine("	SALES_MKDN ");
//                aWriter.WriteLine("ELSE ");
//                aWriter.WriteLine("	CASE WHEN SALES_MKDN < 0 THEN 0 ELSE SALES_MKDN END ");
//                aWriter.WriteLine("END AS SALES_MKDN, ");
//                aWriter.WriteLine(" ");
//                aWriter.WriteLine(" ");
//                aWriter.WriteLine("--Equal to STOCK if not = to 0, else equal to STOCK_REG + STOCK_MKDN  ");
//                aWriter.WriteLine("CASE WHEN @USE_NET_SALES='1' THEN ");
//                aWriter.WriteLine("	CASE WHEN coalesce(STOCK,0) <> 0 THEN coalesce(STOCK,0) ELSE coalesce(STOCK_REG,0) + coalesce(STOCK_MKDN,0) END ");
//                aWriter.WriteLine("ELSE ");
//                aWriter.WriteLine("	CASE WHEN coalesce(CASE WHEN STOCK < 0 THEN 0 ELSE STOCK END,0) <> 0 THEN coalesce(CASE WHEN STOCK < 0 THEN 0 ELSE STOCK END,0) ELSE coalesce(CASE WHEN STOCK_REG < 0 THEN 0 ELSE STOCK_REG END,0) + coalesce(CASE WHEN STOCK_MKDN < 0 THEN 0 ELSE STOCK_MKDN END,0) END ");
//                aWriter.WriteLine("END AS STOCK, ");
//                aWriter.WriteLine(" ");
//                aWriter.WriteLine("CASE WHEN @USE_NET_SALES='1' THEN ");
//                aWriter.WriteLine("	STOCK_REG ");
//                aWriter.WriteLine("ELSE ");
//                aWriter.WriteLine("	CASE WHEN STOCK_REG < 0 THEN 0 ELSE STOCK_REG END ");
//                aWriter.WriteLine("END AS STOCK_REG, ");
//                aWriter.WriteLine(" ");
//                aWriter.WriteLine("CASE WHEN @USE_NET_SALES='1' THEN ");
//                aWriter.WriteLine("	STOCK_MKDN ");
//                aWriter.WriteLine("ELSE ");
//                aWriter.WriteLine("	CASE WHEN STOCK_MKDN < 0 THEN 0 ELSE STOCK_MKDN END ");
//                aWriter.WriteLine("END AS STOCK_MKDN, ");
//                //End TT#1076-MD -jsobek -Size Day to Week Summary - Positive Sales Option & Color Level Fields


////            // Begin TT#3373 - JSmith - Save Store Forecast receive DBNull error
//                aWriter.WriteLine("				CASE WHEN DATENAME(dw, DATETIME_TIME_ID) = 'Monday' THEN (CASE WHEN coalesce(STOCK,0) <> 0 THEN 1 ELSE 0 END) ELSE NULL END AS MONDAY_IN_STOCK_DAY, ");
//                aWriter.WriteLine("				CASE WHEN DATENAME(dw, DATETIME_TIME_ID) = 'Tuesday' THEN (CASE WHEN coalesce(STOCK,0) <> 0 THEN 1 ELSE 0 END) ELSE NULL END AS TUESDAY_IN_STOCK_DAY,	 ");
//                aWriter.WriteLine("				CASE WHEN DATENAME(dw, DATETIME_TIME_ID) = 'Wednesday' THEN (CASE WHEN coalesce(STOCK,0) <> 0 THEN 1 ELSE 0 END) ELSE NULL END AS WEDNESDAY_IN_STOCK_DAY, ");
//                aWriter.WriteLine("				CASE WHEN DATENAME(dw, DATETIME_TIME_ID) = 'Thursday' THEN (CASE WHEN coalesce(STOCK,0) <> 0 THEN 1 ELSE 0 END) ELSE NULL END AS THURSDAY_IN_STOCK_DAY, ");
//                aWriter.WriteLine("				CASE WHEN DATENAME(dw, DATETIME_TIME_ID) = 'Friday' THEN (CASE WHEN coalesce(STOCK,0) <> 0 THEN 1 ELSE 0 END) ELSE NULL END AS FRIDAY_IN_STOCK_DAY, ");
//                aWriter.WriteLine("				CASE WHEN DATENAME(dw, DATETIME_TIME_ID) = 'Saturday' THEN (CASE WHEN coalesce(STOCK,0) <> 0 THEN 1 ELSE 0 END) ELSE NULL END AS SATURDAY_IN_STOCK_DAY, ");
//                aWriter.WriteLine("				CASE WHEN DATENAME(dw, DATETIME_TIME_ID) = 'Sunday' THEN (CASE WHEN coalesce(STOCK,0) <> 0 THEN 1 ELSE 0 END) ELSE NULL END AS SUNDAY_IN_STOCK_DAY, ");
//            override public void Generate_SP_MID_GET_TABLE_FROM_TYPE(StreamWriter aWriter)
//            {
                //aWriter.WriteLine("STOCK =  ");
                //aWriter.WriteLine("  MONDAY_STOCK ");
                //aWriter.WriteLine("+ TUESDAY_STOCK ");
                //aWriter.WriteLine("+ WEDNESDAY_STOCK ");
                //aWriter.WriteLine("+ THURSDAY_STOCK ");
                //aWriter.WriteLine("+ FRIDAY_STOCK ");
                //aWriter.WriteLine("+ SATURDAY_STOCK ");
                //aWriter.WriteLine("+ SUNDAY_STOCK, ");
                //aWriter.WriteLine(" ");
                //aWriter.WriteLine("STOCK_REG =  ");
                //aWriter.WriteLine("  MONDAY_STOCK_REG ");
                //aWriter.WriteLine("+ TUESDAY_STOCK_REG ");
                //aWriter.WriteLine("+ WEDNESDAY_STOCK_REG ");
                //aWriter.WriteLine("+ THURSDAY_STOCK_REG ");
                //aWriter.WriteLine("+ FRIDAY_STOCK_REG ");
                //aWriter.WriteLine("+ SATURDAY_STOCK_REG ");
                //aWriter.WriteLine("+ SUNDAY_STOCK_REG, ");
                //aWriter.WriteLine(" ");
                //aWriter.WriteLine("STOCK_MKDN =  ");
                //aWriter.WriteLine("  MONDAY_STOCK_MKDN ");
                //aWriter.WriteLine("+ TUESDAY_STOCK_MKDN ");
                //aWriter.WriteLine("+ WEDNESDAY_STOCK_MKDN ");
                //aWriter.WriteLine("+ THURSDAY_STOCK_MKDN ");
                //aWriter.WriteLine("+ FRIDAY_STOCK_MKDN ");
                //aWriter.WriteLine("+ SATURDAY_STOCK_MKDN ");
                //aWriter.WriteLine("+ SUNDAY_STOCK_MKDN, ");

                //aWriter.WriteLine("STOCK = ");
                //aWriter.WriteLine("CASE ");
                //aWriter.WriteLine("WHEN DATENAME(dw, FIRST_DAY_OF_WEEK_DATETIME) = 'Monday' THEN MONDAY_STOCK ");
                //aWriter.WriteLine("WHEN DATENAME(dw, FIRST_DAY_OF_WEEK_DATETIME) = 'Tuesday' THEN TUESDAY_STOCK ");
                //aWriter.WriteLine("WHEN DATENAME(dw, FIRST_DAY_OF_WEEK_DATETIME) = 'Wednesday' THEN WEDNESDAY_STOCK ");
                //aWriter.WriteLine("WHEN DATENAME(dw, FIRST_DAY_OF_WEEK_DATETIME) = 'Thursday' THEN THURSDAY_STOCK ");
                //aWriter.WriteLine("WHEN DATENAME(dw, FIRST_DAY_OF_WEEK_DATETIME) = 'Friday' THEN FRIDAY_STOCK ");
                //aWriter.WriteLine("WHEN DATENAME(dw, FIRST_DAY_OF_WEEK_DATETIME) = 'Saturday' THEN SATURDAY_STOCK ");
                //aWriter.WriteLine("WHEN DATENAME(dw, FIRST_DAY_OF_WEEK_DATETIME) = 'Sunday' THEN SUNDAY_STOCK ");
                //aWriter.WriteLine("END,");

                //aWriter.WriteLine("STOCK_REG = ");
                //aWriter.WriteLine("CASE ");
                //aWriter.WriteLine("WHEN DATENAME(dw, FIRST_DAY_OF_WEEK_DATETIME) = 'Monday' THEN MONDAY_STOCK_REG ");
                //aWriter.WriteLine("WHEN DATENAME(dw, FIRST_DAY_OF_WEEK_DATETIME) = 'Tuesday' THEN TUESDAY_STOCK_REG ");
                //aWriter.WriteLine("WHEN DATENAME(dw, FIRST_DAY_OF_WEEK_DATETIME) = 'Wednesday' THEN WEDNESDAY_STOCK_REG ");
                //aWriter.WriteLine("WHEN DATENAME(dw, FIRST_DAY_OF_WEEK_DATETIME) = 'Thursday' THEN THURSDAY_STOCK_REG ");
                //aWriter.WriteLine("WHEN DATENAME(dw, FIRST_DAY_OF_WEEK_DATETIME) = 'Friday' THEN FRIDAY_STOCK_REG ");
                //aWriter.WriteLine("WHEN DATENAME(dw, FIRST_DAY_OF_WEEK_DATETIME) = 'Saturday' THEN SATURDAY_STOCK_REG ");
                //aWriter.WriteLine("WHEN DATENAME(dw, FIRST_DAY_OF_WEEK_DATETIME) = 'Sunday' THEN SUNDAY_STOCK_REG ");
                //aWriter.WriteLine("END,");

                //aWriter.WriteLine("+ SUNDAY_STOCK_MKDN, ");
                //aWriter.WriteLine("CASE ");
                //aWriter.WriteLine("WHEN DATENAME(dw, FIRST_DAY_OF_WEEK_DATETIME) = 'Monday' THEN MONDAY_STOCK_MKDN ");
                //aWriter.WriteLine("WHEN DATENAME(dw, FIRST_DAY_OF_WEEK_DATETIME) = 'Tuesday' THEN TUESDAY_STOCK_MKDN ");
                //aWriter.WriteLine("WHEN DATENAME(dw, FIRST_DAY_OF_WEEK_DATETIME) = 'Wednesday' THEN WEDNESDAY_STOCK_MKDN ");
                //aWriter.WriteLine("WHEN DATENAME(dw, FIRST_DAY_OF_WEEK_DATETIME) = 'Thursday' THEN THURSDAY_STOCK_MKDN ");
                //aWriter.WriteLine("WHEN DATENAME(dw, FIRST_DAY_OF_WEEK_DATETIME) = 'Friday' THEN FRIDAY_STOCK_MKDN ");
                //aWriter.WriteLine("WHEN DATENAME(dw, FIRST_DAY_OF_WEEK_DATETIME) = 'Saturday' THEN SATURDAY_STOCK_MKDN ");
                //aWriter.WriteLine("WHEN DATENAME(dw, FIRST_DAY_OF_WEEK_DATETIME) = 'Sunday' THEN SUNDAY_STOCK_MKDN ");
                //aWriter.WriteLine("END,");



                //    aWriter.WriteLine("w.ACCUM_SELL_THRU_SALES = csv.ACCUM_SELL_THRU_SALES, ");
                //    aWriter.WriteLine("w.ACCUM_SELL_THRU_STOCK = csv.ACCUM_SELL_THRU_STOCK, ");
                //    aWriter.WriteLine("w.DAYS_IN_STOCK = csv.DAYS_IN_STOCK, ");
                    //aWriter.WriteLine("w.RECEIVED_STOCK = csv.RECEIVED_STOCK ");
//                string procedureName;

//                procedureName = Include.DBGetTableFromType;

//                aWriter.WriteLine("--dv ============================================= ");
//                aWriter.WriteLine("--dv Author:		John Smith ");
//                aWriter.WriteLine("--dv Create date: 11/27/2013 ");
//                aWriter.WriteLine("--dv Modified:	11/27/2013 ");
//                aWriter.WriteLine("--dv Description:	Returns datatable based on SQL Type ");
//                aWriter.WriteLine("--dv ============================================= ");
//                aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "] ");
//                aWriter.WriteLine("	@TABLE_TYPE int ");
//                aWriter.WriteLine("AS ");
//                aWriter.WriteLine("BEGIN ");
//                aWriter.WriteLine(" ");
//                aWriter.WriteLine("DECLARE @CFWL MID_CHN_FOR_WK_LOCK_TYPE ");
//                aWriter.WriteLine("DECLARE @CFW MID_CHN_FOR_WK_TYPE ");
//                aWriter.WriteLine("DECLARE @CHW MID_CHN_HIS_WK_TYPE ");
//                aWriter.WriteLine("DECLARE @SFWL MID_ST_FOR_WK_LOCK_TYPE ");
//                aWriter.WriteLine("DECLARE @SFW MID_ST_FOR_WK_TYPE ");
//                aWriter.WriteLine("DECLARE @SHD MID_ST_HIS_DAY_TYPE ");
//                aWriter.WriteLine("DECLARE @SHW MID_ST_HIS_WK_TYPE ");

//                aWriter.WriteLine("if @TABLE_TYPE = 1 SELECT top 0 * FROM @CFWL ");
//                aWriter.WriteLine("else if @TABLE_TYPE = 2 SELECT top 0 * FROM @CFW ");
//                aWriter.WriteLine("else if @TABLE_TYPE = 3 SELECT top 0 * FROM @CHW ");
//                aWriter.WriteLine("else if @TABLE_TYPE = 4 SELECT top 0 * FROM @SFWL ");
//                aWriter.WriteLine("else if @TABLE_TYPE = 5 SELECT top 0 * FROM @SFW ");
//                aWriter.WriteLine("else if @TABLE_TYPE = 6 SELECT top 0 * FROM @SHD ");
//                aWriter.WriteLine("else if @TABLE_TYPE = 7 SELECT top 0 * FROM @SHW ");
//                aWriter.WriteLine(" ");
//                aWriter.WriteLine("END ");
//                aWriter.WriteLine(" ");
//                aWriter.WriteLine("GO ");
//                aWriter.WriteLine("");
//            }
//            // End TT#3373 - JSmith - Save Store Forecast receive DBNull error

//            //End TT#739-MD -jsobek -Delete Stores 
//            protected string BuildXMLVarName(string aVarName, bool aIsLockVariable)
//            {
//                try
//                {
//                    string varName = "xml" + aVarName.ToLower(CultureInfo.CurrentCulture);
//                    // remove underscores "_"
//                    while (true)
//                    {
//                        int index = varName.IndexOf("_");
//                        if (index == -1)
//                        {
//                            break;
//                        }
//                        else
//                        {
//                            varName = varName.Remove(index,1);
//                        }
//                    }
//                    if (aIsLockVariable)
//                    {
//                        varName += "lock";
//                    }
//                    return varName;
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            protected void AddStoredProcedureDrop(string aTableName, StreamWriter aWriter)
//            {
//                try
//                {
//                    aWriter.WriteLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + aTableName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
//                    aWriter.WriteLine("drop procedure [dbo].[" + aTableName + "]");
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(" ");
//                }
//                catch
//                {
//                    throw;
//                }
//            }

//            // Begin TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables
//            protected void AddTypeDrop(string aTypeName, StreamWriter aWriter)
//            {
//                try
//                {
//                    aWriter.WriteLine("if exists (select * from sys.types WHERE is_table_type = 1 AND name = '" + aTypeName + "')");
//                    aWriter.WriteLine("drop TYPE [dbo].[" + aTypeName + "]");
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(" ");
//                }
//                catch
//                {
//                    throw;
//                }
//            }
//            // End TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables

//            //Begin TT#739-MD -jsobek -Delete Stores 
//            protected void AddTableValuedFunctionDrop(string aFunctionName, StreamWriter aWriter)
//            {
//                try
//                {
//                    aWriter.WriteLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + aFunctionName + "]') and OBJECTPROPERTY(id, N'IsTableFunction') = 1)");
//                    aWriter.WriteLine("drop function [dbo].[" + aFunctionName + "]");
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(" ");
//                }
//                catch
//                {
//                    throw;
//                }
//            }
//            //End TT#739-MD -jsobek -Delete Stores 

//            protected void AddViewDrop(string aViewName, StreamWriter aWriter)
//            {
//                try
//                {
//                    aWriter.WriteLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + aViewName + "]') and OBJECTPROPERTY(id, N'IsView') = 1)");
//                    aWriter.WriteLine("drop view [dbo].[" + aViewName + "]");
//                    aWriter.WriteLine("GO");
//                    aWriter.WriteLine(" ");
//                }
//                catch
//                {
//                    throw;
//                }
//            }
//        }
        
//        public class GenSQLServerBase : GenSQLServer
//        {
//            public GenSQLServerBase()
//            {
//            }
//        }

//        public class GenSQLServer2008 : GenSQLServer
//        {
//            public GenSQLServer2008()
//            {
//            }
//        }

//        public class GenSQLServer2012 : GenSQLServer
//        {
//            public GenSQLServer2012()
//            {
//            }
//        }

//    }
//}
