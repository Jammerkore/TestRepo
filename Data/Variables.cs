// Begin TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables
//
// Too many changes to mark. Compare for differences
// Also removed old commend code for readability
//
// End TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

using MIDRetail.DataCommon;



namespace MIDRetail.Data
{
    public partial class VariablesData : DataLayer
    {
        private int _numberOfStoreDataTables = 1;
        private DataTable _dtVariableValues = null;
        private DataTable _dtVariableLocks = null;
        private DataTable[] _variableWriteDT = null;
        //private bool _writeXML;

        // Begin TT#5124 - JSmith - Performance
        //public VariablesData()
        //    : base()
        //{
        //    try
        //    {
        //        GlobalOptions opts = new GlobalOptions();
        //        DataTable dt = opts.GetGlobalOptions();
        //        DataRow dr = dt.Rows[0];
        //        this._numberOfStoreDataTables = (dr["STORE_TABLE_COUNT"] == DBNull.Value) ?
        //            1 : Convert.ToInt32(dr["STORE_TABLE_COUNT"], CultureInfo.CurrentUICulture);
        //        //_writeXML = false;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        // End TT#5124 - JSmith - Performance

        //Begin  TT#2131-MD - JSmith - Halo Integration
        private ExtractValueManager evm = null;
        // End TT#2131-MD - JSmith - Halo Integration

        public VariablesData(int aNumberOfStoreDataTables)
            : base()
        {
            try
            {
                this._numberOfStoreDataTables = aNumberOfStoreDataTables;
                //_writeXML = false;
                if (MIDEnvironment.ExtractIsEnabled)
                {
                    evm = new ExtractValueManager();
                }
            }
            catch
            {
                throw;
            }
        }

        public void Variable_XMLInit()
        {
            try
            {
                _variableWriteDT = new DataTable[_numberOfStoreDataTables];
                for (int i = 0; i < _numberOfStoreDataTables; i++)
                {
                    _variableWriteDT[i] = null;
                }
                _dtVariableValues = null;
                _dtVariableLocks = null;
                //_writeXML = true;
            }
            catch
            {
                throw;
            }
        }



        public DataTable ChainDailyHistory_Read(int HN_RID, int TIME_ID, ProfileList variableList)
        {
            try
            {
                string SQLCommand = "SELECT 1 \"FV_RID\", HN_RID, TIME_ID";
                SQLCommand += BuildVariableList(variableList, eVariableCategory.Chain, eCalendarDateType.Day, Include.FV_ActualRID, false, "chd", "");
                SQLCommand += " FROM CHAIN_HISTORY_DAY chd ";
                SQLCommand += "WHERE HN_RID = " + HN_RID.ToString(CultureInfo.CurrentUICulture);
                SQLCommand += "  AND TIME_ID = " + TIME_ID.ToString(CultureInfo.CurrentUICulture);

                return _dba.ExecuteSQLQuery(SQLCommand, "ChainDailyHistory");
            }
            catch
            {
                throw;
            }
        }

        public DataTable ChainDailyHistory_Read(int HN_RID, ProfileList TimeList, ProfileList variableList)
        {
            try
            {
                string SQLCommand = "SELECT 1 \"FV_RID\", HN_RID, TIME_ID";
                SQLCommand += BuildVariableList(variableList, eVariableCategory.Chain, eCalendarDateType.Day, Include.FV_ActualRID, false, "chd", "");
                SQLCommand += " FROM CHAIN_HISTORY_DAY chd ";
                SQLCommand += "WHERE HN_RID = " + HN_RID.ToString(CultureInfo.CurrentUICulture);
                SQLCommand += " AND " + BuildTimeList(TimeList, "");

                return _dba.ExecuteSQLQuery(SQLCommand, "ChainDailyHistory");
            }
            catch
            {
                throw;
            }
        }

        public void ChainDailyHistory_Update_Insert(int HN_RID, int TIME_ID, Hashtable values)
        {
            try
            {
                //if (_writeXML)
                //{
                ChainDailyHistory_Update_XMLInsert(HN_RID, TIME_ID, values);
                //}
                //else
                //{
                //    //ChainDailyHistory_Update_SQLInsert(HN_RID, TIME_ID, values);
                //    throw new Exception("Store Daily History Update Insert Exception - Must use write XML.");
                //}
            }
            catch
            {
                throw;
            }
        }


        private void ChainDailyHistory_Update_XMLInsert(int HN_RID, int TIME_ID, Hashtable values)
        {
            try
            {
                DataRow drChainHistoryWeek;
                if (_dtVariableValues == null)
                {
                    _dtVariableValues = DatabaseSchema.GetTableSchema("CHAIN_HISTORY_WEEK");
                }

                drChainHistoryWeek = _dtVariableValues.NewRow();
                _dtVariableValues.Rows.Add(drChainHistoryWeek);

                drChainHistoryWeek["HN_RID"] = HN_RID;
                drChainHistoryWeek["TIME_ID"] = TIME_ID;
                foreach (DictionaryEntry val in values)
                {
                    drChainHistoryWeek[((VariableProfile)val.Key).DatabaseColumnName] = val.Value;
                }

                return;
            }
            catch
            {
                throw;
            }
        }


        public DataTable ChainWeek_Read(int HN_RID, int FV_RID, ProfileList TimeList, ProfileList variableList)
        {
            DataTable dtVariable = null;
            DataRow drVariable = null;

            try
            {
                if (FV_RID == Include.FV_ActualRID)
                {
                    dtVariable = DatabaseSchema.GetTableSchema(Include.DBChainWeeklyHistoryReadType);

                    foreach (Profile timeID in TimeList)
                    {
                        drVariable = dtVariable.NewRow();
                        dtVariable.Rows.Add(drVariable);
                        drVariable["HN_RID"] = HN_RID;
                        drVariable["TIME_ID"] = timeID.Key;
                    }

                    return StoredProcedures.SP_MID_CHN_HIS_WK_READ.Read(_dba,
                                                                        dt: dtVariable,
                                                                        Rollup: 'N'
                                                                        );
                }
                else if (FV_RID == Include.FV_ModifiedRID)
                {
                    dtVariable = DatabaseSchema.GetTableSchema(Include.DBChainWeeklyModifiedReadType);

                    foreach (Profile timeID in TimeList)
                    {
                        drVariable = dtVariable.NewRow();
                        dtVariable.Rows.Add(drVariable);
                        drVariable["HN_RID"] = HN_RID;
                        drVariable["FV_RID"] = FV_RID;
                        drVariable["TIME_ID"] = timeID.Key;
                    }

                    return StoredProcedures.SP_MID_CHN_MOD_WK_READ.Read(_dba,
                                                                        dt: dtVariable,
                                                                        Rollup: 'N'
                                                                        );
                }
                else
                {
                    dtVariable = DatabaseSchema.GetTableSchema(Include.DBChainWeeklyForecastReadType);

                    foreach (Profile timeID in TimeList)
                    {
                        drVariable = dtVariable.NewRow();
                        dtVariable.Rows.Add(drVariable);
                        drVariable["HN_RID"] = HN_RID;
                        drVariable["FV_RID"] = FV_RID;
                        drVariable["TIME_ID"] = timeID.Key;
                    }

                    return StoredProcedures.SP_MID_CHN_FOR_WK_READ.Read(_dba, dt: dtVariable);
                }
            }
            catch
            {
                throw;
            }
        }

        public DataTable ChainWeekLock_Read(int HN_RID, int FV_RID, ProfileList TimeList, ProfileList variableList)
        {
            DataTable dtVariable = null;
            DataRow drVariable = null;
            dtVariable = DatabaseSchema.GetTableSchema(Include.DBChainWeeklyForecastReadLockType);

            foreach (Profile timeID in TimeList)
            {
                drVariable = dtVariable.NewRow();
                dtVariable.Rows.Add(drVariable);
                drVariable["HN_RID"] = HN_RID;
                drVariable["FV_RID"] = FV_RID;
                drVariable["TIME_ID"] = timeID.Key;
            }

            return StoredProcedures.SP_MID_CHN_FOR_WK_LOCK_READ.Read(_dba, dt: dtVariable);
        }

        // Begin TT#2131-MD - JSmith - Halo Integration
		//public void ChainWeek_Update_Insert(int HN_RID, int TIME_ID, int FV_RID,
        //    Hashtable values, Hashtable locks, bool aSaveLocks)
        public void ChainWeek_Update_Insert(int HN_RID, int TIME_ID, int FV_RID,
            Hashtable values, Hashtable locks, bool aSaveLocks, bool cellChanged = true)
        // End TT#2131-MD - JSmith - Halo Integration
        {
            try
            {
                //if (_writeXML)
                //{
                ChainWeek_Update_XMLInsert(HN_RID, TIME_ID, FV_RID, values, locks, aSaveLocks);
                //}
                //else
                //{
                //    //ChainWeek_Update_SQLInsert(HN_RID, TIME_ID, FV_RID, values, locks, aSaveLocks);
                //    throw new Exception("Store Daily History Update Insert Exception - Must use write XML.");
                //}

                // Begin TT#2131-MD - JSmith - Halo Integration
                if (MIDEnvironment.ExtractIsEnabled
                    && cellChanged)
                {
                    evm.AddValue(HN_RID, FV_RID, ePlanType.Chain, TIME_ID);
                }
                // End TT#2131-MD - JSmith - Halo Integration
            }
            catch
            {
                throw;
            }
        }


        private void ChainWeek_Update_XMLInsert(int HN_RID, int TIME_ID, int FV_RID, Hashtable values,
            Hashtable locks, bool aSaveLocks)
        {
            try
            {
                if (FV_RID == Include.FV_ActualRID)
                {
                    DataRow drChainHistoryWeek;
                    if (_dtVariableValues == null)
                    {
                        _dtVariableValues = DatabaseSchema.GetTableSchema("CHAIN_HISTORY_WEEK");

                    }

                    drChainHistoryWeek = _dtVariableValues.NewRow();
                    _dtVariableValues.Rows.Add(drChainHistoryWeek);

                    drChainHistoryWeek["HN_RID"] = HN_RID;
                    drChainHistoryWeek["TIME_ID"] = TIME_ID;
                    foreach (DictionaryEntry val in values)
                    {
                        drChainHistoryWeek[((VariableProfile)val.Key).DatabaseColumnName] = val.Value;
                    }

                    return;
                }
                else
                {
                    DataRow drChainForecastWeek;
                    if (_dtVariableValues == null)
                    {
                        _dtVariableValues = DatabaseSchema.GetTableSchema("CHAIN_FORECAST_WEEK");
                    }

                    drChainForecastWeek = _dtVariableValues.NewRow();
                    _dtVariableValues.Rows.Add(drChainForecastWeek);

                    drChainForecastWeek["HN_RID"] = HN_RID;
                    drChainForecastWeek["FV_RID"] = FV_RID;
                    drChainForecastWeek["TIME_ID"] = TIME_ID;
                    foreach (DictionaryEntry val in values)
                    {
                        drChainForecastWeek[((VariableProfile)val.Key).DatabaseColumnName] = val.Value;
                    }

                    if (aSaveLocks)
                    {
                        // add locks element
                        DataRow drChainForecastWeekLock;
                        if (_dtVariableLocks == null)
                        {
                            _dtVariableLocks = DatabaseSchema.GetTableSchema("CHAIN_FORECAST_WEEK_LOCK");
                        }

                        drChainForecastWeekLock = _dtVariableLocks.NewRow();
                        _dtVariableLocks.Rows.Add(drChainForecastWeekLock);

                        drChainForecastWeekLock["HN_RID"] = HN_RID;
                        drChainForecastWeekLock["FV_RID"] = FV_RID;
                        drChainForecastWeekLock["TIME_ID"] = TIME_ID;
                        foreach (DictionaryEntry val in locks)
                        {
                            drChainForecastWeekLock[((VariableProfile)val.Key).DatabaseColumnName + "_LOCK"] = Include.ConvertBoolToInt((bool)val.Value);
                        }
                    }

                    return;
                }
            }
            catch
            {
                throw;
            }
        }

        public void ChainWeek_XMLUpdate(int FV_RID, bool aSaveLocks)
        {
            try
            {
                // Begin TT#2131-MD - JSmith - Halo Integration
                if (MIDEnvironment.ExtractIsEnabled)
                {
                    EXTRACT_PLANNING_CONTROL_Update();
                }
                // End TT#2131-MD - JSmith - Halo Integration

                // only send document if values or flags were sent
                if ((_dtVariableValues != null &&
                   _dtVariableValues.Rows.Count > 0) ||
                   (_dtVariableLocks != null &&
                   _dtVariableLocks.Rows.Count > 0))
                {
                    if (FV_RID == Include.FV_ActualRID)
                    {
                        StoredProcedures.SP_MID_CHN_HIS_WK_WRITE.Insert(_dba, dt: _dtVariableValues);
                    }
                    else
                    {
                        if (_dtVariableLocks == null)
                        {
                            _dtVariableLocks = DatabaseSchema.GetTableSchema("CHAIN_FORECAST_WEEK_LOCK");
                        }
                        StoredProcedures.SP_MID_CHN_FOR_WK_WRITE.Insert(_dba,
                                                                    dt: _dtVariableValues,
                                                                    dtLock: _dtVariableLocks,
                                                                    SaveLocks: Include.ConvertBoolToChar(aSaveLocks)
                                                                    );
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void ChainWeek_Delete(int HN_RID, int FV_RID, ProfileList TimeList)
        {
            try
            {
                if (FV_RID == Include.FV_ActualRID)
                {
                    StoredProcedures.MID_CHAIN_HISTORY_WEEK_DELETE_FROM_NODE.Delete(_dba,
                                                                                HN_RID: HN_RID,
                                                                                TIME_LIST: BuildTimeListAsDataTable(TimeList)
                                                                                );
                }
                else
                {
                    StoredProcedures.MID_CHAIN_FORECAST_WEEK_DELETE_FROM_NODE.Delete(_dba,
                                                                                 HN_RID: HN_RID,
                                                                                 FV_RID: FV_RID,
                                                                                 TIME_LIST: BuildTimeListAsDataTable(TimeList)
                                                                                 );

                    StoredProcedures.MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FROM_NODE.Delete(_dba,
                                                                                      HN_RID: HN_RID,
                                                                                      FV_RID: FV_RID,
                                                                                      TIME_LIST: BuildTimeListAsDataTable(TimeList)
                                                                                      );
                }
            }
            catch
            {
                throw;
            }
        }

        public void ChainWeek_Copy(ProfileList aVariableList, ProfileList aTimeList, int aTo_HN_RID, int aTo_FV_RID,
            int aFrom_HN_RID, int aFrom_FV_RID)
        {
            string SQLCommand;
            try
            {
                if (aTo_FV_RID == Include.FV_ActualRID &&
                    aFrom_FV_RID == Include.FV_ActualRID)
                {
                    SQLCommand = "INSERT CHAIN_HISTORY_WEEK (HN_RID, TIME_ID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Chain, eCalendarDateType.Week, aTo_FV_RID, false, true, "", "") + ")";
                    SQLCommand += " select " + aTo_HN_RID + ", TIME_ID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Chain, eCalendarDateType.Week, aTo_FV_RID, false, "chw", "");
                    SQLCommand += "   from CHAIN_HISTORY_WEEK chw ";
                    SQLCommand += " where HN_RID = " + aFrom_HN_RID;
                    SQLCommand += "   and " + BuildTimeList(aTimeList, "");
                }
                else if (aTo_FV_RID == Include.FV_ActualRID)
                {
                    SQLCommand = "INSERT CHAIN_HISTORY_WEEK (HN_RID, TIME_ID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Chain, eCalendarDateType.Week, aTo_FV_RID, false, true, "", "") + ")";
                    SQLCommand += " select " + aTo_HN_RID + ", TIME_ID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Chain, eCalendarDateType.Week, aTo_FV_RID, false, "cfw", "");
                    SQLCommand += "   from CHAIN_FORECAST_WEEK cfw ";
                    SQLCommand += " where HN_RID = " + aFrom_HN_RID;
                    SQLCommand += "   and FV_RID = " + aFrom_FV_RID;
                    SQLCommand += "   and " + BuildTimeList(aTimeList, "");
                }
                else if (aFrom_FV_RID == Include.FV_ActualRID)
                {
                    SQLCommand = "INSERT CHAIN_FORECAST_WEEK (HN_RID, FV_RID, TIME_ID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Chain, eCalendarDateType.Week, aTo_FV_RID, false, true, "", "") + ")";
                    SQLCommand += " select " + aTo_HN_RID + "," + aTo_FV_RID + ", TIME_ID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Chain, eCalendarDateType.Week, aTo_FV_RID, false, "chw", "");
                    SQLCommand += "   from CHAIN_HISTORY_WEEK chw ";
                    SQLCommand += " where HN_RID = " + aFrom_HN_RID;
                    SQLCommand += "   and " + BuildTimeList(aTimeList, "");
                }
                else
                {
                    SQLCommand = "INSERT CHAIN_FORECAST_WEEK (HN_RID, FV_RID, TIME_ID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Chain, eCalendarDateType.Week, aTo_FV_RID, false, true, "", "") + ")";
                    SQLCommand += " select " + aTo_HN_RID + "," + aTo_FV_RID + ", TIME_ID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Chain, eCalendarDateType.Week, aTo_FV_RID, false, "cfw", "cfwl");
                    SQLCommand += "   from CHAIN_FORECAST_WEEK cfw ";
                    SQLCommand += " where HN_RID = " + aFrom_HN_RID;
                    SQLCommand += "   and FV_RID = " + aFrom_FV_RID;
                    SQLCommand += "   and " + BuildTimeList(aTimeList, "");
                }

                _dba.ExecuteNonQuery(SQLCommand);
            }
            catch
            {
                throw;
            }
        }

        public void ChainWeek_Copy(ProfileList aVariableList, int aTo_HN_RID, int aTo_FV_RID, int aTo_TimeID,
            int aFrom_HN_RID, int aFrom_FV_RID, int aFrom_TimeID)
        {
            string SQLCommand;
            try
            {
                if (aTo_FV_RID == Include.FV_ActualRID &&
                    aFrom_FV_RID == Include.FV_ActualRID)
                {
                    SQLCommand = "INSERT CHAIN_HISTORY_WEEK (HN_RID, TIME_ID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Chain, eCalendarDateType.Week, aTo_FV_RID, false, true, "", "") + ")";
                    SQLCommand += " select " + aTo_HN_RID + "," + aTo_TimeID;
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Chain, eCalendarDateType.Week, aTo_FV_RID, false, "chw", "");
                    SQLCommand += "   from CHAIN_HISTORY_WEEK chw ";
                    SQLCommand += " where HN_RID = " + aFrom_HN_RID;
                    SQLCommand += "   and TIME_ID = " + aFrom_TimeID;
                }
                else if (aTo_FV_RID == Include.FV_ActualRID)
                {
                    SQLCommand = "INSERT CHAIN_HISTORY_WEEK (HN_RID, TIME_ID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Chain, eCalendarDateType.Week, aTo_FV_RID, false, true, "", "") + ")";
                    SQLCommand += " select " + aTo_HN_RID + "," + aTo_TimeID;
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Chain, eCalendarDateType.Week, aTo_FV_RID, false, "cfw", "");
                    SQLCommand += "   from CHAIN_FORECAST_WEEK cfw ";
                    SQLCommand += " where HN_RID = " + aFrom_HN_RID;
                    SQLCommand += "   and FV_RID = " + aFrom_FV_RID;
                    SQLCommand += "   and TIME_ID = " + aFrom_TimeID;
                }
                else if (aFrom_FV_RID == Include.FV_ActualRID)
                {
                    SQLCommand = "INSERT CHAIN_FORECAST_WEEK (HN_RID, FV_RID, TIME_ID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Chain, eCalendarDateType.Week, aTo_FV_RID, false, true, "", "") + ")";
                    SQLCommand += " select " + aTo_HN_RID + "," + aTo_FV_RID + "," + aTo_TimeID;
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Chain, eCalendarDateType.Week, aTo_FV_RID, false, "chw", "");
                    SQLCommand += "   from CHAIN_HISTORY_WEEK chw ";
                    SQLCommand += " where HN_RID = " + aFrom_HN_RID;
                    SQLCommand += "   and TIME_ID = " + aFrom_TimeID;
                }
                else
                {
                    SQLCommand = "INSERT CHAIN_FORECAST_WEEK (HN_RID, FV_RID, TIME_ID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Chain, eCalendarDateType.Week, aTo_FV_RID, false, true, "", "") + ")";
                    SQLCommand += " select " + aTo_HN_RID + "," + aTo_FV_RID + "," + aTo_TimeID;
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Chain, eCalendarDateType.Week, aTo_FV_RID, false, "cfw", "cfwl");
                    SQLCommand += "   from CHAIN_FORECAST_WEEK cfw ";
                    SQLCommand += " where HN_RID = " + aFrom_HN_RID;
                    SQLCommand += "   and FV_RID = " + aFrom_FV_RID;
                    SQLCommand += "   and TIME_ID = " + aFrom_TimeID;
                }

                _dba.ExecuteNonQuery(SQLCommand);
            }
            catch
            {
                throw;
            }
        }

        public int ChainWeeklyHistory_DeleteLessThanDate(int aCommitLimit)
        {
            try
            {
                return StoredProcedures.MID_CHAIN_HISTORY_WEEK_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
            }
            catch
            {
                throw;
            }
        }

        public int ChainWeeklyHistory_DeleteLessThanDate(int aPurgeDate, int aCommitLimit)
        {
            try
            {
                return StoredProcedures.MID_CHAIN_HISTORY_WEEK_DELETE.Delete(_dba,
                                                                      COMMIT_LIMIT: aCommitLimit,
                                                                      TIME_ID: aPurgeDate
                                                                      );
            }
            catch
            {
                throw;
            }
        }

        public int ChainWeeklyHistory_DeleteLessThanDate(int TIME_ID, ArrayList aNodeList, int aCommitLimit)
        {
            try
            {
                return StoredProcedures.MID_CHAIN_HISTORY_WEEK_DELETE_LESS_THAN_DATE.Delete(_dba,
                                                                                     COMMIT_LIMIT: aCommitLimit,
                                                                                     TIME_ID: TIME_ID,
                                                                                     NODE_LIST: BuildNodeListAsDataTable(aNodeList)
                                                                                     );
            }
            catch
            {
                throw;
            }
        }

        public int ChainPlans_DeleteLessThanDate(int aCommitLimit)
        {
            try
            {
                return StoredProcedures.MID_CHAIN_FORECAST_WEEK_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
            }
            catch
            {
                throw;
            }
        }

        public int ChainPlanLocks_DeleteLessThanDate(int aCommitLimit)
        {
            try
            {
                return StoredProcedures.MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
            }
            catch
            {
                throw;
            }
        }

        public int ChainPlans_DeleteLessThanDate(int aPurgeDate, int aCommitLimit)
        {
            try
            {
                return StoredProcedures.MID_CHAIN_FORECAST_WEEK_DELETE_FOR_PURGE_LESS_THAN_DATE.Delete(_dba,
                                                                                                COMMIT_LIMIT: aCommitLimit,
                                                                                                TIME_ID: aPurgeDate
                                                                                                );
            }
            catch
            {
                throw;
            }
        }

        public int ChainPlanLocks_DeleteLessThanDate(int aPurgeDate, int aCommitLimit)
        {
            try
            {
                return StoredProcedures.MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_LESS_THAN_DATE.Delete(_dba,
                                                                                                     COMMIT_LIMIT: aCommitLimit,
                                                                                                     TIME_ID: aPurgeDate);

            }
            catch
            {
                throw;
            }
        }

        public int ChainPlans_DeleteLessThanDate(int TIME_ID, ArrayList aNodeList, int aCommitLimit)
        {
            try
            {
                return StoredProcedures.MID_CHAIN_FORECAST_WEEK_DELETE_LESS_THAN_DATE.Delete(_dba,
                                                                                      COMMIT_LIMIT: aCommitLimit,
                                                                                      TIME_ID: TIME_ID,
                                                                                      NODE_LIST: BuildNodeListAsDataTable(aNodeList)
                                                                                      );
            }
            catch
            {
                throw;
            }
        }

        public int ChainPlansLock_DeleteLessThanDate(int TIME_ID, ArrayList aNodeList, int aCommitLimit)
        {
            try
            {
                return StoredProcedures.MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_LESS_THAN_DATE.Delete(_dba,
                                                                                           COMMIT_LIMIT: aCommitLimit,
                                                                                           TIME_ID: TIME_ID,
                                                                                           NODE_LIST: BuildNodeListAsDataTable(aNodeList)
                                                                                           );
            }
            catch
            {
                throw;
            }
        }

        public DataTable StoreDailyHistory_Read(int HN_RID, int TIME_ID, ProfileList variableList)
        {
            DataTable dtVariable = null;
            DataRow drVariable = null;
            try
            {
                int tableNumber = HN_RID % _numberOfStoreDataTables;
                string procedureName = Include.DBStoreDailyHistoryReadSP.Replace(Include.DBTableCountReplaceString, tableNumber.ToString());

                dtVariable = DatabaseSchema.GetTableSchema(Include.DBStoreDailyHistoryReadType);

                drVariable = dtVariable.NewRow();
                dtVariable.Rows.Add(drVariable);
                drVariable["HN_MOD"] = tableNumber;
                drVariable["HN_RID"] = HN_RID;
                drVariable["TIME_ID"] = TIME_ID;


                MIDDbParameter[] InParams = { new MIDDbParameter("@dt", dtVariable, eDbType.Structured, eParameterDirection.Input),
												  new MIDDbParameter("@Rollup", 'N', eDbType.Char, eParameterDirection.Input) };
                return _dba.ExecuteQuery(procedureName, "StoreDailyHistory", InParams);
            }
            catch
            {
                throw;
            }
        }


        public void StoreDailyHistory_Update_Insert(int HN_RID, int TIME_ID, int StoreRID, Hashtable values)
        {
            try
            {
                //if (_writeXML)
                //{
                StoreDay_Update_XMLInsert(HN_RID, TIME_ID, StoreRID, values);
                //}
                //else
                //{
                //    //StoreDay_Update_SQLInsert(HN_RID, TIME_ID, StoreRID, values);
                //    throw new Exception("Store Daily History Update Insert Exception - Must use write XML.");
                //}
            }
            catch
            {
                throw;
            }
        }


        private void StoreDay_Update_XMLInsert(int HN_RID, int TIME_ID, int StoreRID, Hashtable values)
        {
            try
            {
                int tableNumber = HN_RID % _numberOfStoreDataTables;

                DataTable dtVariableValues = (DataTable)_variableWriteDT[tableNumber];

                DataRow drStoreHistoryDay;
                if (dtVariableValues == null)
                {
                    dtVariableValues = DatabaseSchema.GetTableSchema("STORE_HISTORY_DAY" + tableNumber.ToString());
                    _variableWriteDT[tableNumber] = dtVariableValues;
                }

                drStoreHistoryDay = dtVariableValues.NewRow();
                dtVariableValues.Rows.Add(drStoreHistoryDay);

                drStoreHistoryDay["HN_MOD"] = tableNumber;
                drStoreHistoryDay["HN_RID"] = HN_RID;
                drStoreHistoryDay["TIME_ID"] = TIME_ID;
                drStoreHistoryDay["ST_RID"] = StoreRID;
                foreach (DictionaryEntry val in values)
                {
                    drStoreHistoryDay[((VariableProfile)val.Key).DatabaseColumnName] = val.Value;
                }

                return;
            }
            catch
            {
                throw;
            }
        }

        public void StoreDaily_XMLUpdate()
        {
            try
            {
                for (int i = 0; i < _numberOfStoreDataTables; i++)
                {
                    StoreDaily_XMLUpdate(i);
                }
            }
            catch //(Exception ex)
            {
                throw;
            }
        }

        public void StoreDaily_XMLUpdate(int aTableNumber)
        {
            try
            {
                // only send document if values or flags were sent
                DataTable dtVariableValues = (DataTable)_variableWriteDT[aTableNumber];
                if (dtVariableValues != null &&
                    dtVariableValues.Rows.Count > 0)
                {
                    MIDDbParameter[] InParams = { new MIDDbParameter("@dt", dtVariableValues, eDbType.Structured, eParameterDirection.Input) };
                    string procedureName = Include.DBStoreDailyHistoryWriteSP.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
                    _dba.ExecuteStoredProcedure(procedureName, InParams);

                }
            }
            catch (Exception ex)
            {
                string exMsg = ex.ToString();
                throw;
            }
        }

        public int StoreDailyHistory_DeleteLessThanDate(int aTableNumber, int aCommitLimit, int aNumberOfTables)
        {
            try
            {

                if (aTableNumber == 0) return StoredProcedures.MID_STORE_HISTORY_DAY0_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 1) return StoredProcedures.MID_STORE_HISTORY_DAY1_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 2) return StoredProcedures.MID_STORE_HISTORY_DAY2_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 3) return StoredProcedures.MID_STORE_HISTORY_DAY3_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 4) return StoredProcedures.MID_STORE_HISTORY_DAY4_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 5) return StoredProcedures.MID_STORE_HISTORY_DAY5_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 6) return StoredProcedures.MID_STORE_HISTORY_DAY6_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 7) return StoredProcedures.MID_STORE_HISTORY_DAY7_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 8) return StoredProcedures.MID_STORE_HISTORY_DAY8_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else return StoredProcedures.MID_STORE_HISTORY_DAY9_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);

            }
            catch
            {
                throw;
            }
        }

        public int StoreDailyHistory_DeleteLessThanDate(int aTableNumber, int aPurgeDate, int aCommitLimit, int aNumberOfTables)
        {
            try
            {
                if (aTableNumber == 0) return StoredProcedures.MID_STORE_HISTORY_DAY0_DELETE_FOR_PURGE_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 1) return StoredProcedures.MID_STORE_HISTORY_DAY1_DELETE_FOR_PURGE_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 2) return StoredProcedures.MID_STORE_HISTORY_DAY2_DELETE_FOR_PURGE_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 3) return StoredProcedures.MID_STORE_HISTORY_DAY3_DELETE_FOR_PURGE_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 4) return StoredProcedures.MID_STORE_HISTORY_DAY4_DELETE_FOR_PURGE_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 5) return StoredProcedures.MID_STORE_HISTORY_DAY5_DELETE_FOR_PURGE_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 6) return StoredProcedures.MID_STORE_HISTORY_DAY6_DELETE_FOR_PURGE_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 7) return StoredProcedures.MID_STORE_HISTORY_DAY7_DELETE_FOR_PURGE_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 8) return StoredProcedures.MID_STORE_HISTORY_DAY8_DELETE_FOR_PURGE_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else return StoredProcedures.MID_STORE_HISTORY_DAY9_DELETE_FOR_PURGE_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);

            }
            catch
            {
                throw;
            }
        }



        public DataTable StoreWeek_Read(int HN_RID, int FV_RID, int TIME_ID, ProfileList variableList)
        {
            DataTable dtVariable = null;
            DataRow drVariable = null;
            int tableNumber = HN_RID % _numberOfStoreDataTables;

            if (FV_RID == Include.FV_ActualRID)
            {
                string procedureName = Include.DBStoreWeeklyHistoryReadSP.Replace(Include.DBTableCountReplaceString, tableNumber.ToString());

                dtVariable = DatabaseSchema.GetTableSchema(Include.DBStoreWeeklyHistoryReadType);

                drVariable = dtVariable.NewRow();
                dtVariable.Rows.Add(drVariable);
                drVariable["HN_MOD"] = tableNumber;
                drVariable["HN_RID"] = HN_RID;
                drVariable["TIME_ID"] = TIME_ID;

                MIDDbParameter[] InParams = { new MIDDbParameter("@dt", dtVariable, eDbType.Structured, eParameterDirection.Input),
												  new MIDDbParameter("@Rollup", 'N', eDbType.Char, eParameterDirection.Input) };
                return _dba.ExecuteQuery(procedureName, "StoreWeeklyHistory", InParams);
            }
            else if (FV_RID == Include.FV_ModifiedRID)
            {
                string procedureName = Include.DBStoreWeeklyModVerReadSP.Replace(Include.DBTableCountReplaceString, tableNumber.ToString());

                dtVariable = DatabaseSchema.GetTableSchema(Include.DBStoreWeeklyModifiedReadType);

                drVariable = dtVariable.NewRow();
                dtVariable.Rows.Add(drVariable);
                drVariable["HN_MOD"] = tableNumber;
                drVariable["HN_RID"] = HN_RID;
                drVariable["TIME_ID"] = TIME_ID;

                MIDDbParameter[] InParams = { new MIDDbParameter("@dt", dtVariable, eDbType.Structured, eParameterDirection.Input),
												  new MIDDbParameter("@Rollup", 'N', eDbType.Char, eParameterDirection.Input) };
                //Begin TT#1001-MD -jsobek -Error reading modified weekly version history
                //return _dba.ExecuteQuery( "dbo.SP_MID_XML_ST_MOD_WK_READ", "StoreWeeklyForecast", InParams );
                return _dba.ExecuteQuery(procedureName, "StoreWeeklyForecast", InParams);
                //End TT#1001-MD -jsobek -Error reading modified weekly version history
            }
            else
            {
                string procedureName = Include.DBStoreWeeklyForecastReadSP.Replace(Include.DBTableCountReplaceString, tableNumber.ToString());
                dtVariable = DatabaseSchema.GetTableSchema(Include.DBStoreWeeklyForecastReadType);

                drVariable = dtVariable.NewRow();
                dtVariable.Rows.Add(drVariable);
                drVariable["HN_MOD"] = tableNumber;
                drVariable["HN_RID"] = HN_RID;
                drVariable["FV_RID"] = FV_RID;
                drVariable["TIME_ID"] = TIME_ID;

                MIDDbParameter[] InParams = { new MIDDbParameter("@dt", dtVariable, eDbType.Structured, eParameterDirection.Input) };
                return _dba.ExecuteQuery(procedureName, "StoreWeeklyForecast", InParams);
            }
        }

        public DataTable StoreWeek_Read(int HN_RID, int FV_RID, ProfileList TimeList, ProfileList variableList)
        {
            DataTable dtVariable = null;
            DataRow drVariable = null;

            int tableNumber = HN_RID % _numberOfStoreDataTables;

            if (FV_RID == Include.FV_ActualRID)
            {
                string procedureName = Include.DBStoreWeeklyHistoryReadSP.Replace(Include.DBTableCountReplaceString, tableNumber.ToString());

                dtVariable = DatabaseSchema.GetTableSchema(Include.DBStoreWeeklyHistoryReadType);

                foreach (Profile timeID in TimeList)
                {
                    drVariable = dtVariable.NewRow();
                    dtVariable.Rows.Add(drVariable);
                    drVariable["HN_MOD"] = tableNumber;
                    drVariable["HN_RID"] = HN_RID;
                    drVariable["TIME_ID"] = timeID.Key;
                }

                MIDDbParameter[] InParams = { new MIDDbParameter("@dt", dtVariable, eDbType.Structured, eParameterDirection.Input),
												  new MIDDbParameter("@Rollup", 'N', eDbType.Char, eParameterDirection.Input) };

                return _dba.ExecuteQuery(procedureName, "StoreWeeklyHistory", InParams);
            }
            else if (FV_RID == Include.FV_ModifiedRID)
            {
                string procedureName = Include.DBStoreWeeklyModVerReadSP.Replace(Include.DBTableCountReplaceString, tableNumber.ToString());

                dtVariable = DatabaseSchema.GetTableSchema(Include.DBStoreWeeklyModifiedReadType);

                foreach (Profile timeID in TimeList)
                {
                    drVariable = dtVariable.NewRow();
                    dtVariable.Rows.Add(drVariable);
                    drVariable["HN_MOD"] = tableNumber;
                    drVariable["HN_RID"] = HN_RID;
                    drVariable["FV_RID"] = FV_RID;
                    drVariable["TIME_ID"] = timeID.Key;
                }

                MIDDbParameter[] InParams = { new MIDDbParameter("@dt", dtVariable, eDbType.Structured, eParameterDirection.Input),
												  new MIDDbParameter("@Rollup", 'N', eDbType.Char, eParameterDirection.Input) };
                return _dba.ExecuteQuery(procedureName, "StoreWeeklyForecast", InParams);
            }
            else
            {
                string procedureName = Include.DBStoreWeeklyForecastReadSP.Replace(Include.DBTableCountReplaceString, tableNumber.ToString());
                dtVariable = DatabaseSchema.GetTableSchema(Include.DBStoreWeeklyForecastReadType);

                foreach (Profile timeID in TimeList)
                {
                    drVariable = dtVariable.NewRow();
                    dtVariable.Rows.Add(drVariable);
                    drVariable["HN_MOD"] = tableNumber;
                    drVariable["HN_RID"] = HN_RID;
                    drVariable["FV_RID"] = FV_RID;
                    drVariable["TIME_ID"] = timeID.Key;
                }

                MIDDbParameter[] InParams = { new MIDDbParameter("@dt", dtVariable, eDbType.Structured, eParameterDirection.Input) };
                return _dba.ExecuteQuery(procedureName, "StoreWeeklyForecast", InParams);
            }
        }

        public DataTable StoreWeekLock_Read(int HN_RID, int FV_RID, ProfileList TimeList, ProfileList variableList)
        {
            DataTable dtVariable = null;
            DataRow drVariable = null;
            dtVariable = DatabaseSchema.GetTableSchema(Include.DBStoreWeeklyForecastReadLockType);

            foreach (Profile timeID in TimeList)
            {
                drVariable = dtVariable.NewRow();
                dtVariable.Rows.Add(drVariable);
                drVariable["HN_RID"] = HN_RID;
                drVariable["FV_RID"] = FV_RID;
                drVariable["TIME_ID"] = timeID.Key;
            }
            return StoredProcedures.SP_MID_ST_FOR_WK_LOCK_READ.Read(_dba, dt: dtVariable);
        }


        // Begin TT#2131-MD - JSmith - Halo Integration
		//public void StoreWeek_Update_Insert(int HN_RID, int TIME_ID, int FV_RID, int StoreRID,
        //    Hashtable values, Hashtable locks, bool aSaveLocks)
        public void StoreWeek_Update_Insert(int HN_RID, int TIME_ID, int FV_RID, int StoreRID,
            Hashtable values, Hashtable locks, bool aSaveLocks, bool cellChanged = true)
        // End TT#2131-MD - JSmith - Halo Integration
        {
            try
            {
                //if (_writeXML)
                //{
                StoreWeek_Update_XMLInsert(HN_RID, TIME_ID, FV_RID, StoreRID, values, locks, aSaveLocks);
                //}
                //else
                //{
                //    //StoreWeek_Update_SQLInsert(HN_RID, TIME_ID, FV_RID, StoreRID, values, locks, aSaveLocks);
                //    throw new Exception("Store Week History Update Insert Exception - Must use write XML.");
                //}

                // Begin TT#2131-MD - JSmith - Halo Integration
                if (MIDEnvironment.ExtractIsEnabled
                    && cellChanged)
                {
                    evm.AddValue(HN_RID, FV_RID, ePlanType.Store, TIME_ID);
                }
                // End TT#2131-MD - JSmith - Halo Integration
            }
            catch
            {
                throw;
            }
        }


        private void StoreWeek_Update_XMLInsert(int HN_RID, int TIME_ID, int FV_RID, int StoreRID,
            Hashtable values, Hashtable locks, bool aSaveLocks)
        {
            try
            {
                int tableNumber = HN_RID % _numberOfStoreDataTables;

                DataTable dtVariableValues = (DataTable)_variableWriteDT[tableNumber];

                if (FV_RID == Include.FV_ActualRID)
                {
                    DataRow drStoreHistoryWeek;
                    if (dtVariableValues == null)
                    {
                        dtVariableValues = DatabaseSchema.GetTableSchema("STORE_HISTORY_WEEK" + tableNumber.ToString());
                        _variableWriteDT[tableNumber] = dtVariableValues;
                    }

                    drStoreHistoryWeek = dtVariableValues.NewRow();
                    dtVariableValues.Rows.Add(drStoreHistoryWeek);

                    drStoreHistoryWeek["HN_MOD"] = tableNumber;
                    drStoreHistoryWeek["HN_RID"] = HN_RID;
                    drStoreHistoryWeek["TIME_ID"] = TIME_ID;
                    drStoreHistoryWeek["ST_RID"] = StoreRID;
                    foreach (DictionaryEntry val in values)
                    {
                        drStoreHistoryWeek[((VariableProfile)val.Key).DatabaseColumnName] = val.Value;
                    }

                    return;
                }
                else
                {
                    DataRow drStoreForecastWeek;
                    if (dtVariableValues == null)
                    {
                        dtVariableValues = DatabaseSchema.GetTableSchema("STORE_FORECAST_WEEK" + tableNumber.ToString());
                        _variableWriteDT[tableNumber] = dtVariableValues;
                    }

                    drStoreForecastWeek = dtVariableValues.NewRow();
                    dtVariableValues.Rows.Add(drStoreForecastWeek);

                    drStoreForecastWeek["HN_MOD"] = tableNumber;
                    drStoreForecastWeek["HN_RID"] = HN_RID;
                    drStoreForecastWeek["FV_RID"] = FV_RID;
                    drStoreForecastWeek["TIME_ID"] = TIME_ID;
                    drStoreForecastWeek["ST_RID"] = StoreRID;
                    foreach (DictionaryEntry val in values)
                    {
                        drStoreForecastWeek[((VariableProfile)val.Key).DatabaseColumnName] = val.Value;
                    }


                    if (aSaveLocks)
                    {
                        // add locks element
                        DataRow drStoreForecastWeekLock;
                        if (_dtVariableLocks == null)
                        {
                            _dtVariableLocks = DatabaseSchema.GetTableSchema("STORE_FORECAST_WEEK_LOCK");
                        }

                        drStoreForecastWeekLock = _dtVariableLocks.NewRow();
                        _dtVariableLocks.Rows.Add(drStoreForecastWeekLock);

                        drStoreForecastWeekLock["HN_RID"] = HN_RID;
                        drStoreForecastWeekLock["FV_RID"] = FV_RID;
                        drStoreForecastWeekLock["TIME_ID"] = TIME_ID;
                        drStoreForecastWeekLock["ST_RID"] = StoreRID;
                        foreach (DictionaryEntry val in locks)
                        {
                            drStoreForecastWeekLock[((VariableProfile)val.Key).DatabaseColumnName + "_LOCK"] = Include.ConvertBoolToInt((bool)val.Value);
                        }
                    }

                    return;
                }
            }
            catch
            {
                throw;
            }
        }

        public void StoreWeek_XMLUpdate(int FV_RID, bool aSaveLocks)
        {
            try
            {
                // Begin TT#2131-MD - JSmith - Halo Integration
                if (MIDEnvironment.ExtractIsEnabled)
                {
                    EXTRACT_PLANNING_CONTROL_Update();
                }
                // End TT#2131-MD - JSmith - Halo Integration

                // Begin TT#3373 - JSmith - Save Store Forecast receive DBNull error
                // only send document if values or flags were sent
                //for (int i = 0; i<_numberOfStoreDataTables; i++)
                //{
                //    StoreWeek_XMLUpdate(FV_RID, i, aSaveLocks);
                //}
                for (int i = 0; i < _numberOfStoreDataTables; i++)
                {
                    StoreWeek_XMLUpdate(FV_RID, i);
                }

                if (aSaveLocks)
                {
                    StoreWeekLocks_XMLUpdate();
                }
                // End TT#3373 - JSmith - Save Store Forecast receive DBNull error
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#3373 - JSmith - Save Store Forecast receive DBNull error
        //public void StoreWeek_XMLUpdate(int FV_RID, int aTableNumber, bool aSaveLocks)
        public void StoreWeek_XMLUpdate(int FV_RID, int aTableNumber)
        // End TT#3373 - JSmith - Save Store Forecast receive DBNull error
        {
            try
            {
                // Begin TT#2131-MD - JSmith - Halo Integration
                if (MIDEnvironment.ExtractIsEnabled)
                {
                    EXTRACT_PLANNING_CONTROL_Update();
                }
                // End TT#2131-MD - JSmith - Halo Integration

                // only send document if values or flags were sent
                DataTable dtVariableValues = (DataTable)_variableWriteDT[aTableNumber];
                // Begin TT#3373 - JSmith - Save Store Forecast receive DBNull error
                //if ((dtVariableValues != null &&
                //    dtVariableValues.Rows.Count > 0) ||
                //    (_dtVariableLocks != null &&
                //    _dtVariableLocks.Rows.Count > 0))
                if (dtVariableValues != null &&
                    dtVariableValues.Rows.Count > 0)
                // End TT#3373 - JSmith - Save Store Forecast receive DBNull error
                {
                    if (FV_RID == Include.FV_ActualRID)
                    {
                        MIDDbParameter[] InParams = { new MIDDbParameter("@dt", dtVariableValues, eDbType.Structured, eParameterDirection.Input) };
                        string procedureName = Include.DBStoreWeeklyHistoryWriteSP.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
                        _dba.ExecuteStoredProcedure(procedureName, InParams);
                    }
                    else
                    {
                        // Begin TT#3373 - JSmith - Save Store Forecast receive DBNull error
                        //if (dtVariableValues == null)
                        //{
                        //    _dtVariableLocks = DatabaseSchema.GetTableSchema("STORE_FORECAST_WEEK" + aTableNumber.ToString());
                        //}
                        //if (_dtVariableLocks == null)
                        //{
                        //    _dtVariableLocks = DatabaseSchema.GetTableSchema("STORE_FORECAST_WEEK_LOCK");
                        //}
                        //MIDDbParameter[] InParams = { new MIDDbParameter("@dt", dtVariableValues, eDbType.Structured, eParameterDirection.Input),
                        //                              new MIDDbParameter("@dtLock", _dtVariableLocks, eDbType.Structured, eParameterDirection.Input),
                        //                              new MIDDbParameter("@SaveLocks", Include.ConvertBoolToChar(aSaveLocks), eDbType.Char, eParameterDirection.Input) };	  
                        //string procedureName = Include.DBStoreWeeklyForecastWriteSP.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
                        //_dba.ExecuteStoredProcedure(procedureName, InParams);

                        MIDDbParameter[] InParams = { new MIDDbParameter("@dt", dtVariableValues, eDbType.Structured, eParameterDirection.Input) };
                        string procedureName = Include.DBStoreWeeklyForecastWriteSP.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
                        _dba.ExecuteStoredProcedure(procedureName, InParams);
                        // End TT#3373 - JSmith - Save Store Forecast receive DBNull error
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#3373 - JSmith - Save Store Forecast receive DBNull error
        public void StoreWeekLocks_XMLUpdate()
        {
            try
            {
                if (_dtVariableLocks != null &&
                    _dtVariableLocks.Rows.Count > 0)
                {
                    MIDDbParameter[] InParams = { new MIDDbParameter("@dtLock", _dtVariableLocks, eDbType.Structured, eParameterDirection.Input) };
                    _dba.ExecuteStoredProcedure(Include.DBStoreWeeklyForecastLockWriteSP, InParams);
                }
            }
            catch
            {
                throw;
            }
        }
        // End TT#3373 - JSmith - Save Store Forecast receive DBNull error

        public void StoreWeek_Delete(int HN_RID, int FV_RID, ProfileList TimeList)
        {
            int tableNumber = HN_RID % _numberOfStoreDataTables;

            if (FV_RID == Include.FV_ActualRID)
            {
                if (tableNumber == 0) StoredProcedures.MID_STORE_HISTORY_WEEK0_DELETE.Delete(_dba, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList));
                else if (tableNumber == 1) StoredProcedures.MID_STORE_HISTORY_WEEK1_DELETE.Delete(_dba, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList));
                else if (tableNumber == 2) StoredProcedures.MID_STORE_HISTORY_WEEK2_DELETE.Delete(_dba, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList));
                else if (tableNumber == 3) StoredProcedures.MID_STORE_HISTORY_WEEK3_DELETE.Delete(_dba, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList));
                else if (tableNumber == 4) StoredProcedures.MID_STORE_HISTORY_WEEK4_DELETE.Delete(_dba, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList));
                else if (tableNumber == 5) StoredProcedures.MID_STORE_HISTORY_WEEK5_DELETE.Delete(_dba, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList));
                else if (tableNumber == 6) StoredProcedures.MID_STORE_HISTORY_WEEK6_DELETE.Delete(_dba, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList));
                else if (tableNumber == 7) StoredProcedures.MID_STORE_HISTORY_WEEK7_DELETE.Delete(_dba, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList));
                else if (tableNumber == 8) StoredProcedures.MID_STORE_HISTORY_WEEK8_DELETE.Delete(_dba, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList));
                else if (tableNumber == 9) StoredProcedures.MID_STORE_HISTORY_WEEK9_DELETE.Delete(_dba, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList));

            }
            else
            {
                if (tableNumber == 0) StoredProcedures.MID_STORE_FORECAST_WEEK0_DELETE.Delete(_dba, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList));
                else if (tableNumber == 1) StoredProcedures.MID_STORE_FORECAST_WEEK1_DELETE.Delete(_dba, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList));
                else if (tableNumber == 2) StoredProcedures.MID_STORE_FORECAST_WEEK2_DELETE.Delete(_dba, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList));
                else if (tableNumber == 3) StoredProcedures.MID_STORE_FORECAST_WEEK3_DELETE.Delete(_dba, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList));
                else if (tableNumber == 4) StoredProcedures.MID_STORE_FORECAST_WEEK4_DELETE.Delete(_dba, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList));
                else if (tableNumber == 5) StoredProcedures.MID_STORE_FORECAST_WEEK5_DELETE.Delete(_dba, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList));
                else if (tableNumber == 6) StoredProcedures.MID_STORE_FORECAST_WEEK6_DELETE.Delete(_dba, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList));
                else if (tableNumber == 7) StoredProcedures.MID_STORE_FORECAST_WEEK7_DELETE.Delete(_dba, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList));
                else if (tableNumber == 8) StoredProcedures.MID_STORE_FORECAST_WEEK8_DELETE.Delete(_dba, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList));
                else if (tableNumber == 9) StoredProcedures.MID_STORE_FORECAST_WEEK9_DELETE.Delete(_dba, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList));

                StoredProcedures.MID_STORE_FORECAST_WEEK_LOCK_DELETE_FROM_NODE.Delete(_dba,
                                                                                      HN_RID: HN_RID,
                                                                                      FV_RID: FV_RID,
                                                                                      TIME_LIST: BuildTimeListAsDataTable(TimeList)
                                                                                      );
            }
        }

        public void StoreWeek_Delete(int HN_RID, int FV_RID, ProfileList TimeList, ProfileList StoreList)
        {
            try
            {
                int tableNumber = HN_RID % _numberOfStoreDataTables;
                //int storesPerCall = 50;
                //int storeCount = 0;
                //string stores = string.Empty;

                if (StoreList.Count == 0)
                {
                    throw new EmptyStoreList(MIDText.GetText(eMIDTextCode.msg_pl_EmptyStoreList));
                }
                else if (FV_RID == Include.FV_ActualRID)
                {

                    if (tableNumber == 0) StoredProcedures.MID_STORE_HISTORY_WEEK0_DELETE_FROM_NODE.Delete(_dba, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 1) StoredProcedures.MID_STORE_HISTORY_WEEK1_DELETE_FROM_NODE.Delete(_dba, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 2) StoredProcedures.MID_STORE_HISTORY_WEEK2_DELETE_FROM_NODE.Delete(_dba, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 3) StoredProcedures.MID_STORE_HISTORY_WEEK3_DELETE_FROM_NODE.Delete(_dba, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 4) StoredProcedures.MID_STORE_HISTORY_WEEK4_DELETE_FROM_NODE.Delete(_dba, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 5) StoredProcedures.MID_STORE_HISTORY_WEEK5_DELETE_FROM_NODE.Delete(_dba, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 6) StoredProcedures.MID_STORE_HISTORY_WEEK6_DELETE_FROM_NODE.Delete(_dba, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 7) StoredProcedures.MID_STORE_HISTORY_WEEK7_DELETE_FROM_NODE.Delete(_dba, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 8) StoredProcedures.MID_STORE_HISTORY_WEEK8_DELETE_FROM_NODE.Delete(_dba, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 9) StoredProcedures.MID_STORE_HISTORY_WEEK9_DELETE_FROM_NODE.Delete(_dba, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                }
                else
                {
                    if (tableNumber == 0) StoredProcedures.MID_STORE_FORECAST_WEEK0_DELETE_FROM_NODE.Delete(_dba, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 1) StoredProcedures.MID_STORE_FORECAST_WEEK1_DELETE_FROM_NODE.Delete(_dba, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 2) StoredProcedures.MID_STORE_FORECAST_WEEK2_DELETE_FROM_NODE.Delete(_dba, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 3) StoredProcedures.MID_STORE_FORECAST_WEEK3_DELETE_FROM_NODE.Delete(_dba, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 4) StoredProcedures.MID_STORE_FORECAST_WEEK4_DELETE_FROM_NODE.Delete(_dba, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 5) StoredProcedures.MID_STORE_FORECAST_WEEK5_DELETE_FROM_NODE.Delete(_dba, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 6) StoredProcedures.MID_STORE_FORECAST_WEEK6_DELETE_FROM_NODE.Delete(_dba, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 7) StoredProcedures.MID_STORE_FORECAST_WEEK7_DELETE_FROM_NODE.Delete(_dba, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 8) StoredProcedures.MID_STORE_FORECAST_WEEK8_DELETE_FROM_NODE.Delete(_dba, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 9) StoredProcedures.MID_STORE_FORECAST_WEEK9_DELETE_FROM_NODE.Delete(_dba, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));

                    StoredProcedures.MID_STORE_FORECAST_WEEK_LOCK_DELETE_FROM_NODE_AND_STORE_LIST.Delete(_dba, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                }
            }
            catch
            {
                throw;
            }
        }

        public void StoreWeek_Delete(int HN_RID, int FV_RID, ProfileList TimeList, ProfileList StoreList,
            TransactionData TD)
        {
            try
            {
                int tableNumber = HN_RID % _numberOfStoreDataTables;
                //int storesPerCall = 50;
                //int storeCount = 0;
                string stores = string.Empty;
                //string command;

                if (StoreList.Count == 0)
                {
                    throw new EmptyStoreList(MIDText.GetText(eMIDTextCode.msg_pl_EmptyStoreList));
                }
                else if (FV_RID == Include.FV_ActualRID)
                {
                    if (tableNumber == 0) StoredProcedures.MID_STORE_HISTORY_WEEK0_DELETE_FROM_NODE.Delete(TD.DBA, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 1) StoredProcedures.MID_STORE_HISTORY_WEEK1_DELETE_FROM_NODE.Delete(TD.DBA, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 2) StoredProcedures.MID_STORE_HISTORY_WEEK2_DELETE_FROM_NODE.Delete(TD.DBA, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 3) StoredProcedures.MID_STORE_HISTORY_WEEK3_DELETE_FROM_NODE.Delete(TD.DBA, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 4) StoredProcedures.MID_STORE_HISTORY_WEEK4_DELETE_FROM_NODE.Delete(TD.DBA, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 5) StoredProcedures.MID_STORE_HISTORY_WEEK5_DELETE_FROM_NODE.Delete(TD.DBA, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 6) StoredProcedures.MID_STORE_HISTORY_WEEK6_DELETE_FROM_NODE.Delete(TD.DBA, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 7) StoredProcedures.MID_STORE_HISTORY_WEEK7_DELETE_FROM_NODE.Delete(TD.DBA, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 8) StoredProcedures.MID_STORE_HISTORY_WEEK8_DELETE_FROM_NODE.Delete(TD.DBA, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 9) StoredProcedures.MID_STORE_HISTORY_WEEK9_DELETE_FROM_NODE.Delete(TD.DBA, HN_RID: HN_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));

                }
                else
                {
                    if (tableNumber == 0) StoredProcedures.MID_STORE_FORECAST_WEEK0_DELETE_FROM_NODE.Delete(TD.DBA, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 1) StoredProcedures.MID_STORE_FORECAST_WEEK1_DELETE_FROM_NODE.Delete(TD.DBA, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 2) StoredProcedures.MID_STORE_FORECAST_WEEK2_DELETE_FROM_NODE.Delete(TD.DBA, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 3) StoredProcedures.MID_STORE_FORECAST_WEEK3_DELETE_FROM_NODE.Delete(TD.DBA, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 4) StoredProcedures.MID_STORE_FORECAST_WEEK4_DELETE_FROM_NODE.Delete(TD.DBA, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 5) StoredProcedures.MID_STORE_FORECAST_WEEK5_DELETE_FROM_NODE.Delete(TD.DBA, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 6) StoredProcedures.MID_STORE_FORECAST_WEEK6_DELETE_FROM_NODE.Delete(TD.DBA, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 7) StoredProcedures.MID_STORE_FORECAST_WEEK7_DELETE_FROM_NODE.Delete(TD.DBA, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 8) StoredProcedures.MID_STORE_FORECAST_WEEK8_DELETE_FROM_NODE.Delete(TD.DBA, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                    else if (tableNumber == 9) StoredProcedures.MID_STORE_FORECAST_WEEK9_DELETE_FROM_NODE.Delete(TD.DBA, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));

                    StoredProcedures.MID_STORE_FORECAST_WEEK_LOCK_DELETE_FROM_NODE_AND_STORE_LIST.Delete(TD.DBA, HN_RID: HN_RID, FV_RID: FV_RID, TIME_LIST: BuildTimeListAsDataTable(TimeList), STORE_LIST: BuildStoreListAsDataTable(StoreList));
                }
            }
            catch
            {
                throw;
            }
        }

        public void StoreWeek_Copy(ProfileList aVariableList, ProfileList aTimeList, int aTo_HN_RID, int aTo_FV_RID,
            int aFrom_HN_RID, int aFrom_FV_RID)
        {
            string SQLCommand;
            int fromTableNumber, toTableNumber;
            try
            {
                fromTableNumber = aFrom_HN_RID % _numberOfStoreDataTables;
                toTableNumber = aTo_HN_RID % _numberOfStoreDataTables;

                if (aTo_FV_RID == Include.FV_ActualRID &&
                    aFrom_FV_RID == Include.FV_ActualRID)
                {
                    SQLCommand = "INSERT STORE_HISTORY_WEEK" + toTableNumber + " (HN_MOD, HN_RID, TIME_ID, ST_RID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Store, eCalendarDateType.Week, aTo_FV_RID, false, true, "", "") + ")";
                    SQLCommand += " select " + toTableNumber + "," + aTo_HN_RID + ", TIME_ID, ST_RID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Store, eCalendarDateType.Week, aTo_FV_RID, false, "shw", "");
                    SQLCommand += "   from STORE_HISTORY_WEEK" + fromTableNumber + " shw ";
                    SQLCommand += " where HN_RID = " + aFrom_HN_RID;
                    SQLCommand += "   and " + BuildTimeList(aTimeList, "");
                }
                else if (aTo_FV_RID == Include.FV_ActualRID)
                {
                    SQLCommand = "INSERT STORE_HISTORY_WEEK" + toTableNumber + " (HN_MOD, HN_RID, TIME_ID, ST_RID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Store, eCalendarDateType.Week, aTo_FV_RID, false, true, "", "") + ")";
                    SQLCommand += " select " + toTableNumber + "," + aTo_HN_RID + ", TIME_ID, ST_RID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Store, eCalendarDateType.Week, aTo_FV_RID, false, "sfw", "");
                    SQLCommand += "   from STORE_FORECAST_WEEK" + fromTableNumber + " sfw ";
                    SQLCommand += " where HN_RID = " + aFrom_HN_RID;
                    SQLCommand += "   and FV_RID = " + aFrom_FV_RID;
                    SQLCommand += "   and " + BuildTimeList(aTimeList, "");
                }
                else if (aFrom_FV_RID == Include.FV_ActualRID)
                {
                    SQLCommand = "INSERT STORE_FORECAST_WEEK" + toTableNumber + " (HN_MOD, HN_RID, FV_RID, TIME_ID, ST_RID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Store, eCalendarDateType.Week, aTo_FV_RID, false, true, "", "") + ")";
                    SQLCommand += " select " + toTableNumber + "," + aTo_HN_RID + "," + aTo_FV_RID + ", TIME_ID, ST_RID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Store, eCalendarDateType.Week, aTo_FV_RID, false, "shw", "");
                    SQLCommand += "   from STORE_HISTORY_WEEK" + fromTableNumber + " shw ";
                    SQLCommand += " where HN_RID = " + aFrom_HN_RID;
                    SQLCommand += "   and " + BuildTimeList(aTimeList, "");
                }
                else
                {
                    SQLCommand = "INSERT STORE_FORECAST_WEEK" + toTableNumber + " (HN_MOD, HN_RID, FV_RID, TIME_ID, ST_RID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Store, eCalendarDateType.Week, aTo_FV_RID, false, true, "", "") + ")";
                    SQLCommand += " select " + toTableNumber + "," + aTo_HN_RID + "," + aTo_FV_RID + ", TIME_ID, ST_RID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Store, eCalendarDateType.Week, aTo_FV_RID, false, "sfw", "sfwl");
                    SQLCommand += "   from STORE_FORECAST_WEEK" + fromTableNumber + " sfw ";
                    SQLCommand += " where HN_RID = " + aFrom_HN_RID;
                    SQLCommand += "   and FV_RID = " + aFrom_FV_RID;
                    SQLCommand += "   and " + BuildTimeList(aTimeList, "");
                }

                _dba.ExecuteNonQuery(SQLCommand);
            }
            catch
            {
                throw;
            }
        }

        public void StoreWeek_Copy(ProfileList aVariableList, int aTo_HN_RID, int aTo_FV_RID, int aTo_TimeID,
            int aFrom_HN_RID, int aFrom_FV_RID, int aFrom_TimeID)
        {
            string SQLCommand;
            int fromTableNumber, toTableNumber;
            try
            {
                fromTableNumber = aFrom_HN_RID % _numberOfStoreDataTables;
                toTableNumber = aTo_HN_RID % _numberOfStoreDataTables;

                if (aTo_FV_RID == Include.FV_ActualRID &&
                    aFrom_FV_RID == Include.FV_ActualRID)
                {
                    SQLCommand = "INSERT STORE_HISTORY_WEEK" + toTableNumber + " (HN_MOD, HN_RID, TIME_ID, ST_RID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Store, eCalendarDateType.Week, aTo_FV_RID, false, true, "", "") + ")";
                    SQLCommand += " select " + toTableNumber + "," + aTo_HN_RID + "," + aTo_TimeID + ", ST_RID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Store, eCalendarDateType.Week, aTo_FV_RID, false, "shw", "");
                    SQLCommand += "   from STORE_HISTORY_WEEK" + fromTableNumber + " shw ";
                    SQLCommand += " where HN_RID = " + aFrom_HN_RID;
                    SQLCommand += "   and TIME_ID = " + aFrom_TimeID;
                }
                else if (aTo_FV_RID == Include.FV_ActualRID)
                {
                    SQLCommand = "INSERT STORE_HISTORY_WEEK" + toTableNumber + " (HN_MOD, HN_RID, TIME_ID, ST_RID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Store, eCalendarDateType.Week, aTo_FV_RID, false, true, "", "") + ")";
                    SQLCommand += " select " + toTableNumber + "," + aTo_HN_RID + "," + aTo_TimeID + ", ST_RID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Store, eCalendarDateType.Week, aTo_FV_RID, false, "sfw", "");
                    SQLCommand += "   from STORE_FORECAST_WEEK" + fromTableNumber + " sfw ";
                    SQLCommand += " where HN_RID = " + aFrom_HN_RID;
                    SQLCommand += "   and FV_RID = " + aFrom_FV_RID;
                    SQLCommand += "   and TIME_ID = " + aFrom_TimeID;
                }
                else if (aFrom_FV_RID == Include.FV_ActualRID)
                {
                    SQLCommand = "INSERT STORE_FORECAST_WEEK" + toTableNumber + " (HN_MOD, HN_RID, FV_RID, TIME_ID, ST_RID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Store, eCalendarDateType.Week, aTo_FV_RID, false, true, "", "") + ")";
                    SQLCommand += " select " + toTableNumber + "," + aTo_HN_RID + "," + aTo_FV_RID + "," + aTo_TimeID + ", ST_RID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Store, eCalendarDateType.Week, aTo_FV_RID, false, "shw", "");
                    SQLCommand += "   from STORE_HISTORY_WEEK" + fromTableNumber + " shw ";
                    SQLCommand += " where HN_RID = " + aFrom_HN_RID;
                    SQLCommand += "   and TIME_ID = " + aFrom_TimeID;
                }
                else
                {
                    SQLCommand = "INSERT STORE_FORECAST_WEEK" + toTableNumber + " (HN_MOD, HN_RID, FV_RID, TIME_ID, ST_RID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Store, eCalendarDateType.Week, aTo_FV_RID, false, true, "", "") + ")";
                    SQLCommand += " select " + toTableNumber + "," + aTo_HN_RID + "," + aTo_FV_RID + "," + aTo_TimeID + ", ST_RID";
                    SQLCommand += BuildVariableList(aVariableList, eVariableCategory.Store, eCalendarDateType.Week, aTo_FV_RID, false, "sfw", "sfwl");
                    SQLCommand += "   from STORE_FORECAST_WEEK" + fromTableNumber + " sfw ";
                    SQLCommand += " where HN_RID = " + aFrom_HN_RID;
                    SQLCommand += "   and FV_RID = " + aFrom_FV_RID;
                    SQLCommand += "   and TIME_ID = " + aFrom_TimeID;
                }

                _dba.ExecuteNonQuery(SQLCommand);
            }
            catch
            {
                throw;
            }
        }


        public int StoreWeeklyHistory_DeleteLessThanDate(int aTableNumber, int aCommitLimit, int aNumberOfTables)
        {
            try
            {
                if (aTableNumber == 0) return StoredProcedures.MID_STORE_HISTORY_WEEK0_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 1) return StoredProcedures.MID_STORE_HISTORY_WEEK1_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 2) return StoredProcedures.MID_STORE_HISTORY_WEEK2_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 3) return StoredProcedures.MID_STORE_HISTORY_WEEK3_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 4) return StoredProcedures.MID_STORE_HISTORY_WEEK4_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 5) return StoredProcedures.MID_STORE_HISTORY_WEEK5_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 6) return StoredProcedures.MID_STORE_HISTORY_WEEK6_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 7) return StoredProcedures.MID_STORE_HISTORY_WEEK7_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 8) return StoredProcedures.MID_STORE_HISTORY_WEEK8_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else return StoredProcedures.MID_STORE_HISTORY_WEEK9_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
            }
            catch
            {
                throw;
            }
        }

        public int StoreWeeklyHistory_DeleteLessThanDate(int aTableNumber, int aPurgeDate, int aCommitLimit, int aNumberOfTables)
        {
            try
            {

                if (aTableNumber == 0) return StoredProcedures.MID_STORE_HISTORY_WEEK0_DELETE_FOR_PURGE_FOR_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 1) return StoredProcedures.MID_STORE_HISTORY_WEEK1_DELETE_FOR_PURGE_FOR_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 2) return StoredProcedures.MID_STORE_HISTORY_WEEK2_DELETE_FOR_PURGE_FOR_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 3) return StoredProcedures.MID_STORE_HISTORY_WEEK3_DELETE_FOR_PURGE_FOR_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 4) return StoredProcedures.MID_STORE_HISTORY_WEEK4_DELETE_FOR_PURGE_FOR_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 5) return StoredProcedures.MID_STORE_HISTORY_WEEK5_DELETE_FOR_PURGE_FOR_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 6) return StoredProcedures.MID_STORE_HISTORY_WEEK6_DELETE_FOR_PURGE_FOR_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 7) return StoredProcedures.MID_STORE_HISTORY_WEEK7_DELETE_FOR_PURGE_FOR_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 8) return StoredProcedures.MID_STORE_HISTORY_WEEK8_DELETE_FOR_PURGE_FOR_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else return StoredProcedures.MID_STORE_HISTORY_WEEK9_DELETE_FOR_PURGE_FOR_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
            }
            catch
            {
                throw;
            }
        }



        public int StorePlans_DeleteLessThanDate(int aTableNumber, int aCommitLimit, int aNumberOfTables)
        {
            try
            {

                if (aTableNumber == 0) return StoredProcedures.MID_STORE_FORECAST_WEEK0_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 1) return StoredProcedures.MID_STORE_FORECAST_WEEK1_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 2) return StoredProcedures.MID_STORE_FORECAST_WEEK2_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 3) return StoredProcedures.MID_STORE_FORECAST_WEEK3_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 4) return StoredProcedures.MID_STORE_FORECAST_WEEK4_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 5) return StoredProcedures.MID_STORE_FORECAST_WEEK5_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 6) return StoredProcedures.MID_STORE_FORECAST_WEEK6_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 7) return StoredProcedures.MID_STORE_FORECAST_WEEK7_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else if (aTableNumber == 8) return StoredProcedures.MID_STORE_FORECAST_WEEK8_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                else return StoredProcedures.MID_STORE_FORECAST_WEEK9_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
            }
            catch
            {
                throw;
            }
        }

        public int StorePlanLocks_DeleteLessThanDate(int aCommitLimit)
        {
            try
            {
                return StoredProcedures.MID_STORE_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);

            }
            catch
            {
                throw;
            }
        }

        public int ChainPlans_ZeroRows(int aCommitLimit)
        {
            try
            {
                return StoredProcedures.SP_MID_CHN_FOR_WK_DEL_ZERO.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
                //return (int)StoredProcedures.SP_MID_CHN_FOR_WK_DEL_ZERO.RECORDS_DELETED.Value;
            }
            catch
            {
                throw;
            }
        }

        public int StorePlans_ZeroRows(int aTableNumber, int aCommitLimit)
        {
            try
            {
                MIDDbParameter[] InParams = { new MIDDbParameter("@COMMIT_LIMIT", aCommitLimit, eDbType.Int, eParameterDirection.Input) };
                MIDDbParameter[] OutParams = { new MIDDbParameter("@RECORDS_DELETED", DBNull.Value, eDbType.Int, eParameterDirection.Output) };

                string procedureName = Include.DBStoreWeeklyForecastDelZeroSP.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
                return _dba.ExecuteStoredProcedure(procedureName, InParams, OutParams);
            }
            catch
            {
                throw;
            }
        }

        public int ChainPlans_DeleteAllUnlocked(int aCommitLimit)
        {
            try
            {
                MIDDbParameter[] InParams = { new MIDDbParameter("@COMMIT_LIMIT", aCommitLimit, eDbType.Int, eParameterDirection.Input) };
                MIDDbParameter[] OutParams = { new MIDDbParameter("@RECORDS_DELETED", DBNull.Value, eDbType.Int, eParameterDirection.Output) };

                return _dba.ExecuteStoredProcedure(Include.DBChainWeeklyForecastDelUnlockedSP, InParams, OutParams);
            }
            catch
            {
                throw;
            }
        }

        public int StorePlans_DeleteAllUnlocked(int aCommitLimit)
        {
            try
            {
                MIDDbParameter[] InParams = { new MIDDbParameter("@COMMIT_LIMIT", aCommitLimit, eDbType.Int, eParameterDirection.Input) };
                MIDDbParameter[] OutParams = { new MIDDbParameter("@RECORDS_DELETED", DBNull.Value, eDbType.Int, eParameterDirection.Output) };

                return _dba.ExecuteStoredProcedure(Include.DBStoreWeeklyForecastDelUnlockedSP, InParams, OutParams);
            }
            catch
            {
                throw;
            }
        }

        public int StorePlans_DeleteLessThanDate(int aTableNumber, int aPurgeDate, int aCommitLimit, int aNumberOfTables)
        {
            try
            {

                if (aTableNumber == 0) return StoredProcedures.MID_STORE_FORECAST_WEEK0_DELETE_FOR_PURGE_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 1) return StoredProcedures.MID_STORE_FORECAST_WEEK1_DELETE_FOR_PURGE_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 2) return StoredProcedures.MID_STORE_FORECAST_WEEK2_DELETE_FOR_PURGE_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 3) return StoredProcedures.MID_STORE_FORECAST_WEEK3_DELETE_FOR_PURGE_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 4) return StoredProcedures.MID_STORE_FORECAST_WEEK4_DELETE_FOR_PURGE_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 5) return StoredProcedures.MID_STORE_FORECAST_WEEK5_DELETE_FOR_PURGE_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 6) return StoredProcedures.MID_STORE_FORECAST_WEEK6_DELETE_FOR_PURGE_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 7) return StoredProcedures.MID_STORE_FORECAST_WEEK7_DELETE_FOR_PURGE_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else if (aTableNumber == 8) return StoredProcedures.MID_STORE_FORECAST_WEEK8_DELETE_FOR_PURGE_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);
                else return StoredProcedures.MID_STORE_FORECAST_WEEK9_DELETE_FOR_PURGE_DATE.Delete(_dba, COMMIT_LIMIT: aCommitLimit, TIME_ID: aPurgeDate);

            }
            catch
            {
                throw;
            }
        }

        public int StorePlanLocks_DeleteLessThanDate(int aPurgeDate, int aCommitLimit)
        {
            try
            {
                return StoredProcedures.MID_STORE_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_DATE.Delete(_dba,
                                                                                            COMMIT_LIMIT: aCommitLimit,
                                                                                            TIME_ID: aPurgeDate
                                                                                            );
            }
            catch
            {
                throw;
            }
        }


        public int StorePlanLocks_DeleteLessThanDate(int aTableNumber, int TIME_ID, ArrayList aNodeList, int aCommitLimit)
        {
            try
            {
                return StoredProcedures.MID_STORE_FORECAST_WEEK_LOCK_DELETE_LESS_THAN_DATE.Delete(_dba,
                                                                                           COMMIT_LIMIT: aCommitLimit,
                                                                                           TIME_ID: TIME_ID,
                                                                                           NODE_LIST: BuildNodeListAsDataTable(aNodeList)
                                                                                           );
            }
            catch
            {
                throw;
            }
        }
        private DataTable BuildNodeListAsDataTable(ArrayList aNodeList)
        {
            DataTable dtNodeList = new DataTable();
            dtNodeList.Columns.Add("HN_RID", typeof(int));
            foreach (int hnRID in aNodeList)
            {
                //ensure hnRIDs are distinct, and only added to the datatable one time
                if (dtNodeList.Select("HN_RID=" + hnRID.ToString()).Length == 0)
                {
                    DataRow dr = dtNodeList.NewRow();
                    dr["HN_RID"] = hnRID;
                    dtNodeList.Rows.Add(dr);
                }
            }
            return dtNodeList;
        }
        private DataTable BuildTimeListAsDataTable(ProfileList TimeList)
        {
            DataTable dtTimeList = new DataTable();
            dtTimeList.Columns.Add("TIME_ID", typeof(int));
            foreach (Profile timeID in TimeList)
            {
                //ensure timeIDs are distinct, and only added to the datatable one time
                if (dtTimeList.Select("TIME_ID=" + timeID.Key.ToString()).Length == 0)
                {
                    DataRow dr = dtTimeList.NewRow();
                    dr["TIME_ID"] = timeID.Key;
                    dtTimeList.Rows.Add(dr);
                }
            }
            return dtTimeList;
        }
        private DataTable BuildStoreListAsDataTable(ProfileList StoreList)
        {
            DataTable dtStoreList = new DataTable();
            dtStoreList.Columns.Add("ST_RID", typeof(int));
            foreach (Profile store in StoreList)
            {
                //ensure storeRIDs are distinct, and only added to the datatable one time
                if (dtStoreList.Select("ST_RID=" + store.Key.ToString()).Length == 0)
                {
                    DataRow dr = dtStoreList.NewRow();
                    dr["ST_RID"] = store.Key;
                    dtStoreList.Rows.Add(dr);
                }
            }
            return dtStoreList;
        }


        public DataTable GetOhByTimeByNode(int timeId, int firstDayOfCurrentWeek, int HN_RID,
            ProfileList aDailyVariableList, ProfileList aWeeklyVariableList, bool aDailyOnhandExists,
            eHierarchyLevelType aLevelType)
        {
            DataTable OnHandDataTable = null;
            if (aDailyOnhandExists ||
                aLevelType == eHierarchyLevelType.Size)
            {
                if (aDailyVariableList.Count > 0)
                {
                    OnHandDataTable = StoreDailyHistory_Read(HN_RID, timeId, null);
                }
            }
            else
            {
                if (aWeeklyVariableList.Count > 0)
                {
                    OnHandDataTable = StoreWeek_Read(HN_RID, Include.FV_ActualRID, firstDayOfCurrentWeek, null);
                }
            }

            return OnHandDataTable;
        }

        /// <summary>
        /// Gets current week-to-day sales by time by node
        /// </summary>
        /// <param name="timeId">Time ID (yyyyddd)</param>
        /// <param name="firstDayOfCurrentWeek">First Day of Current Week (yyyyddd)</param>
        /// <param name="HN_RID">Merchandise Hierarchy node RID</param>
        /// <returns>Datetable containing current week-to-day sales</returns>
        public DataTable GetCurrentWKtoDaySalesByTimeByNode(int timeId, int firstDayOfCurrentWeek, int HN_RID)
        {
            DataTable weekToDaySalesDataTable;
            weekToDaySalesDataTable = StoreDailyHistory_Read(HN_RID, timeId, null);

            return weekToDaySalesDataTable;
        }

        private string BuildVariableList(ProfileList aVariableList, eVariableCategory aVariableCategory, eCalendarDateType aCalendarDateType, int aVersionRID, bool aIncludeLock, string aValueAlias, string aLockAlias)
        {
            return BuildVariableList(aVariableList, aVariableCategory, aCalendarDateType, aVersionRID, aIncludeLock, false, aValueAlias, aLockAlias);
        }

        private string BuildVariableList(ProfileList aVariableList, eVariableCategory aVariableCategory, eCalendarDateType aCalendarDateType, int aVersionRID, bool aIncludeLock,
            bool aForInsert, string aValueAlias, string aLockAlias)
        {
            string SQLCommand = "";
            bool validVariable = false;

            foreach (VariableProfile varProfile in aVariableList)
            {
                validVariable = false;
                if (varProfile.DatabaseColumnName != null)
                {
                    if (aVariableCategory == eVariableCategory.Chain)
                    {
                        if (aVersionRID == Include.FV_ActualRID)
                        {
                            if (varProfile.ChainHistoryModelType != eVariableDatabaseModelType.None)
                            {
                                validVariable = true;
                            }
                        }
                        else if (varProfile.ChainForecastModelType != eVariableDatabaseModelType.None)
                        {
                            validVariable = true;
                        }
                    }
                    else if (aVariableCategory == eVariableCategory.Store)
                    {
                        if (aCalendarDateType == eCalendarDateType.Day)
                        {
                            if (aVersionRID == Include.FV_ActualRID)
                            {
                                if (varProfile.StoreDailyHistoryModelType != eVariableDatabaseModelType.None)
                                {
                                    validVariable = true;
                                }
                            }
                        }
                        else if (aCalendarDateType == eCalendarDateType.Week)
                        {
                            if (aVersionRID == Include.FV_ActualRID)
                            {
                                if (varProfile.StoreWeeklyHistoryModelType != eVariableDatabaseModelType.None)
                                {
                                    validVariable = true;
                                }
                            }
                            else if (varProfile.StoreForecastModelType != eVariableDatabaseModelType.None)
                            {
                                validVariable = true;
                            }
                        }
                    }
                }

                if (validVariable)
                {
                    if (aForInsert)
                    {
                        SQLCommand += ", " + varProfile.DatabaseColumnName;
                        if (aIncludeLock)
                        {
                            SQLCommand += ", " + varProfile.DatabaseColumnName + "_LOCK";
                        }
                    }
                    else
                    {
                        SQLCommand += ", COALESCE(" + aValueAlias + "." + varProfile.DatabaseColumnName + ", 0) " + varProfile.DatabaseColumnName;
                        if (aIncludeLock)
                        {
                            SQLCommand += ", COALESCE(" + aLockAlias + "." + varProfile.DatabaseColumnName + "_LOCK, '0') " + varProfile.DatabaseColumnName + "_LOCK";
                        }
                    }
                }
            }

            return SQLCommand;
        }

        private string BuildVariableLockList(ProfileList aVariableList, eVariableCategory aVariableCategory, eCalendarDateType aCalendarDateType, int aVersionRID, bool aIncludeLock, string aValueAlias, string aLockAlias)
        {
            return BuildVariableLockList(aVariableList, aVariableCategory, aCalendarDateType, aVersionRID, aIncludeLock, false, aValueAlias, aLockAlias);
        }

        private string BuildVariableLockList(ProfileList aVariableList, eVariableCategory aVariableCategory, eCalendarDateType aCalendarDateType, int aVersionRID, bool aIncludeLock,
            bool aForInsert, string aValueAlias, string aLockAlias)
        {
            string SQLCommand = "";
            bool validVariable = false;

            foreach (VariableProfile varProfile in aVariableList)
            {
                validVariable = false;
                if (varProfile.DatabaseColumnName != null)
                {
                    if (aVariableCategory == eVariableCategory.Chain)
                    {
                        if (aVersionRID == Include.FV_ActualRID)
                        {
                            if (varProfile.ChainHistoryModelType != eVariableDatabaseModelType.None)
                            {
                                validVariable = true;
                            }
                        }
                        else if (varProfile.ChainForecastModelType != eVariableDatabaseModelType.None)
                        {
                            validVariable = true;
                        }
                    }
                    else if (aVariableCategory == eVariableCategory.Store)
                    {
                        if (aCalendarDateType == eCalendarDateType.Day)
                        {
                            if (aVersionRID == Include.FV_ActualRID)
                            {
                                if (varProfile.StoreDailyHistoryModelType != eVariableDatabaseModelType.None)
                                {
                                    validVariable = true;
                                }
                            }
                        }
                        else if (aCalendarDateType == eCalendarDateType.Week)
                        {
                            if (aVersionRID == Include.FV_ActualRID)
                            {
                                if (varProfile.StoreWeeklyHistoryModelType != eVariableDatabaseModelType.None)
                                {
                                    validVariable = true;
                                }
                            }
                            else if (varProfile.StoreForecastModelType != eVariableDatabaseModelType.None)
                            {
                                validVariable = true;
                            }
                        }
                    }
                }

                if (validVariable)
                {
                    if (aForInsert)
                    {
                        SQLCommand += ", " + varProfile.DatabaseColumnName + "_LOCK";
                    }
                    else
                    {
                        SQLCommand += ", COALESCE(" + aLockAlias + "." + varProfile.DatabaseColumnName + "_LOCK, '0') " + varProfile.DatabaseColumnName + "_LOCK";
                    }
                }
            }

            return SQLCommand;
        }

        private string BuildTimeList(ProfileList aTimeList, string aTableAlias)
        {
            string SQLCommand = "";

            if (aTimeList.Count == 1)
            {
                SQLCommand += aTableAlias + "TIME_ID = " + Convert.ToString(aTimeList[0].Key, CultureInfo.CurrentUICulture);
            }
            else
            {
                SQLCommand += aTableAlias + "TIME_ID IN (";
                int times = 0;
                foreach (Profile timeID in aTimeList)
                {
                    if (times > 0)
                    {
                        SQLCommand += ",";
                    }
                    SQLCommand += Convert.ToString(timeID.Key, CultureInfo.CurrentUICulture);
                    ++times;
                }
                SQLCommand += ")";
            }

            return SQLCommand;
        }


        public DataTable ChainWeeklyHistory_TimeIDs(int aNodeRID)
        {
            try
            {
                return StoredProcedures.MID_CHAIN_HISTORY_WEEK_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
            }
            catch
            {
                throw;
            }
        }

        public DataTable ChainWeeklyForecast_TimeIDs(int aNodeRID, int aFvRID)
        {
            try
            {
                return StoredProcedures.MID_CHAIN_FORECAST_WEEK_READ_TIME_IDS.Read(_dba,
                                                                                   HN_RID: aNodeRID,
                                                                                   FV_RID: aFvRID
                                                                                   );
            }
            catch
            {
                throw;
            }
        }

        public DataTable StoreDailyHistory_TimeIDs(int aNodeRID)
        {
            try
            {
                int tableNumber = aNodeRID % _numberOfStoreDataTables;

                if (tableNumber == 0)
                {
                    return StoredProcedures.MID_STORE_HISTORY_DAY0_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
                }
                else if (tableNumber == 1)
                {
                    return StoredProcedures.MID_STORE_HISTORY_DAY1_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
                }
                else if (tableNumber == 2)
                {
                    return StoredProcedures.MID_STORE_HISTORY_DAY2_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
                }
                else if (tableNumber == 3)
                {
                    return StoredProcedures.MID_STORE_HISTORY_DAY3_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
                }
                else if (tableNumber == 4)
                {
                    return StoredProcedures.MID_STORE_HISTORY_DAY4_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
                }
                else if (tableNumber == 5)
                {
                    return StoredProcedures.MID_STORE_HISTORY_DAY5_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
                }
                else if (tableNumber == 6)
                {
                    return StoredProcedures.MID_STORE_HISTORY_DAY6_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
                }
                else if (tableNumber == 7)
                {
                    return StoredProcedures.MID_STORE_HISTORY_DAY7_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
                }
                else if (tableNumber == 8)
                {
                    return StoredProcedures.MID_STORE_HISTORY_DAY8_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
                }
                else //if (tableNumber == 9)
                {
                    return StoredProcedures.MID_STORE_HISTORY_DAY9_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
                }


            }
            catch
            {
                throw;
            }
        }

        public DataTable StoreWeeklyHistory_TimeIDs(int aNodeRID)
        {
            try
            {
                int tableNumber = aNodeRID % _numberOfStoreDataTables;

                if (tableNumber == 0)
                {
                    return StoredProcedures.MID_STORE_HISTORY_WEEK0_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
                }
                else if (tableNumber == 1)
                {
                    return StoredProcedures.MID_STORE_HISTORY_WEEK1_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
                }
                else if (tableNumber == 2)
                {
                    return StoredProcedures.MID_STORE_HISTORY_WEEK2_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
                }
                else if (tableNumber == 3)
                {
                    return StoredProcedures.MID_STORE_HISTORY_WEEK3_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
                }
                else if (tableNumber == 4)
                {
                    return StoredProcedures.MID_STORE_HISTORY_WEEK4_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
                }
                else if (tableNumber == 5)
                {
                    return StoredProcedures.MID_STORE_HISTORY_WEEK5_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
                }
                else if (tableNumber == 6)
                {
                    return StoredProcedures.MID_STORE_HISTORY_WEEK6_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
                }
                else if (tableNumber == 7)
                {
                    return StoredProcedures.MID_STORE_HISTORY_WEEK7_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
                }
                else if (tableNumber == 8)
                {
                    return StoredProcedures.MID_STORE_HISTORY_WEEK8_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
                }
                else //if (tableNumber == 9)
                {
                    return StoredProcedures.MID_STORE_HISTORY_WEEK9_READ_TIME_IDS.Read(_dba, HN_RID: aNodeRID);
                }
            }
            catch
            {
                throw;
            }
        }

        public DataTable StoreWeeklyForecast_TimeIDs(int aNodeRID, int aFvRID)
        {
            try
            {
                int tableNumber = aNodeRID % _numberOfStoreDataTables;


                if (tableNumber == 0)
                {
                    return StoredProcedures.MID_STORE_FORECAST_WEEK0_READ_TIME_IDS.Read(_dba,
                                                                                   HN_RID: aNodeRID,
                                                                                   FV_RID: aFvRID
                                                                                   );
                }
                else if (tableNumber == 1)
                {
                    return StoredProcedures.MID_STORE_FORECAST_WEEK1_READ_TIME_IDS.Read(_dba,
                                                                                   HN_RID: aNodeRID,
                                                                                   FV_RID: aFvRID
                                                                                   );
                }
                else if (tableNumber == 2)
                {
                    return StoredProcedures.MID_STORE_FORECAST_WEEK2_READ_TIME_IDS.Read(_dba,
                                                                                   HN_RID: aNodeRID,
                                                                                   FV_RID: aFvRID
                                                                                   );
                }
                else if (tableNumber == 3)
                {
                    return StoredProcedures.MID_STORE_FORECAST_WEEK3_READ_TIME_IDS.Read(_dba,
                                                                                   HN_RID: aNodeRID,
                                                                                   FV_RID: aFvRID
                                                                                   );
                }
                else if (tableNumber == 4)
                {
                    return StoredProcedures.MID_STORE_FORECAST_WEEK4_READ_TIME_IDS.Read(_dba,
                                                                                   HN_RID: aNodeRID,
                                                                                   FV_RID: aFvRID
                                                                                   );
                }
                else if (tableNumber == 5)
                {
                    return StoredProcedures.MID_STORE_FORECAST_WEEK5_READ_TIME_IDS.Read(_dba,
                                                                                   HN_RID: aNodeRID,
                                                                                   FV_RID: aFvRID
                                                                                   );
                }
                else if (tableNumber == 6)
                {
                    return StoredProcedures.MID_STORE_FORECAST_WEEK6_READ_TIME_IDS.Read(_dba,
                                                                                   HN_RID: aNodeRID,
                                                                                   FV_RID: aFvRID
                                                                                   );
                }
                else if (tableNumber == 7)
                {
                    return StoredProcedures.MID_STORE_FORECAST_WEEK7_READ_TIME_IDS.Read(_dba,
                                                                                   HN_RID: aNodeRID,
                                                                                   FV_RID: aFvRID
                                                                                   );
                }
                else if (tableNumber == 8)
                {
                    return StoredProcedures.MID_STORE_FORECAST_WEEK8_READ_TIME_IDS.Read(_dba,
                                                                                   HN_RID: aNodeRID,
                                                                                   FV_RID: aFvRID
                                                                                   );
                }
                else //if (tableNumber == 9)
                {
                    return StoredProcedures.MID_STORE_FORECAST_WEEK9_READ_TIME_IDS.Read(_dba,
                                                                                   HN_RID: aNodeRID,
                                                                                   FV_RID: aFvRID
                                                                                   );
                }
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#2131-MD - JSmith - Halo Integration
        public void AddPlanningExtractControlValue(int HN_RID, int TIME_ID, int FV_RID,
            ePlanType PlanType)
        {
            try
            {
                if (MIDEnvironment.ExtractIsEnabled)
                {
                    evm.AddValue(HN_RID, FV_RID, PlanType, TIME_ID);
                }
            }
            catch
            {
                throw;
            }
        }

        public void EXTRACT_PLANNING_CONTROL_Update(bool forExtract = false)
        {
            try
            {
                DataTable dtExtractValues;
                if (MIDEnvironment.ExtractIsEnabled
                    && evm.ContainsValues)
                {
                    dtExtractValues = DatabaseSchema.GetTableSchema("EXTRACT_PLANNING_CONTROL");
                    evm.PopulateDataTable(dtExtractValues, forExtract);

                    // only send document if values were sent
                    if (dtExtractValues != null &&
                       dtExtractValues.Rows.Count > 0)
                    {
                        StoredProcedures.MID_EXTRACT_PLANNING_CONTROL_WRITE.Insert(_dba,
                                                                        dt: dtExtractValues
                                                                        );
                    }
                    evm.Clear();
                }
            }
            catch
            {
                throw;
            }
        }

        public DataTable EXTRACT_PLANNING_CONTROL_Read(int HN_RID,
                                      int FV_RID,
                                      ePlanType PLAN_TYPE)
        {
            try
            {
                DataTable dt = null;
                if (MIDEnvironment.ExtractIsEnabled)
                {
                    dt = StoredProcedures.MID_EXTRACT_PLANNING_CONTROL_READ.Read(_dba,
                                                                        HN_RID: HN_RID,
                                                                        FV_RID: FV_RID,
                                                                        PLAN_TYPE: Convert.ToInt32(PLAN_TYPE)
                                                                        );
                }
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public void EXTRACT_PLANNING_CONTROL_CLEAR_EXTRACT_DATES()
        {
            try
            {
                StoredProcedures.MID_EXTRACT_PLANNING_CONTROL_CLEAR_EXTRACT_DATES.Update(_dba);
            }
            catch
            {
                throw;
            }
        }

        public int EXTRACT_PLANNING_CONTROL_ChainForecastDeleteLessThanDate(int aCommitLimit)
        {
            try
            {
                return StoredProcedures.MID_EXTRACT_PLANNING_CONTROL_CHAIN_FORECAST_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
            }
            catch
            {
                throw;
            }
        }

        public int EXTRACT_PLANNING_CONTROL_ChainForecastDeleteLessThanDate(int aPurgeDate, int aCommitLimit)
        {
            try
            {
                return StoredProcedures.MID_EXTRACT_PLANNING_CONTROL_CHAIN_FORECAST_DELETE_FOR_PURGE_FOR_DATE.Delete(_dba,
                                                                                                COMMIT_LIMIT: aCommitLimit,
                                                                                                TIME_ID: aPurgeDate
                                                                                                );
            }
            catch
            {
                throw;
            }
        }

        public int EXTRACT_PLANNING_CONTROL_ChainHistoryDeleteLessThanDate(int aCommitLimit)
        {
            try
            {
                return StoredProcedures.MID_EXTRACT_PLANNING_CONTROL_CHAIN_HISTORY_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
            }
            catch
            {
                throw;
            }
        }

        public int EXTRACT_PLANNING_CONTROL_ChainHistoryDeleteLessThanDate(int aPurgeDate, int aCommitLimit)
        {
            try
            {
                return StoredProcedures.MID_EXTRACT_PLANNING_CONTROL_CHAIN_HISTORY_DELETE_FOR_PURGE_FOR_DATE.Delete(_dba,
                                                                                                COMMIT_LIMIT: aCommitLimit,
                                                                                                TIME_ID: aPurgeDate
                                                                                                );
            }
            catch
            {
                throw;
            }
        }

        public int EXTRACT_PLANNING_CONTROL_StoreForecastDeleteLessThanDate(int aCommitLimit)
        {
            try
            {
                return StoredProcedures.MID_EXTRACT_PLANNING_CONTROL_STORE_FORECAST_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
            }
            catch
            {
                throw;
            }
        }

        public int EXTRACT_PLANNING_CONTROL_StoreForecastDeleteLessThanDate(int aPurgeDate, int aCommitLimit)
        {
            try
            {
                return StoredProcedures.MID_EXTRACT_PLANNING_CONTROL_STORE_FORECAST_DELETE_FOR_PURGE_FOR_DATE.Delete(_dba,
                                                                                                COMMIT_LIMIT: aCommitLimit,
                                                                                                TIME_ID: aPurgeDate
                                                                                                );
            }
            catch
            {
                throw;
            }
        }

        public int EXTRACT_PLANNING_CONTROL_StoreHistoryDeleteLessThanDate(int aCommitLimit)
        {
            try
            {
                return StoredProcedures.MID_EXTRACT_PLANNING_CONTROL_STORE_HISTORY_DELETE_FOR_PURGE.Delete(_dba, COMMIT_LIMIT: aCommitLimit);
            }
            catch
            {
                throw;
            }
        }

        public int EXTRACT_PLANNING_CONTROL_StoreHistoryDeleteLessThanDate(int aPurgeDate, int aCommitLimit)
        {
            try
            {
                return StoredProcedures.MID_EXTRACT_PLANNING_CONTROL_STORE_HISTORY_DELETE_FOR_PURGE_FOR_DATE.Delete(_dba,
                                                                                                COMMIT_LIMIT: aCommitLimit,
                                                                                                TIME_ID: aPurgeDate
                                                                                                );
            }
            catch
            {
                throw;
            }
        }

        // End TT#2131-MD - JSmith - Halo Integration
    }

    public class XMLDocument
    {
        private StringBuilder _variableXML;
        private bool _writeXML;
        private bool _endTagAdded;
        private int _rowsAdded;
        private bool _valuesWritten;

        public XMLDocument()
        {
            _variableXML = new StringBuilder();
            _writeXML = false;
            _endTagAdded = false;
            _valuesWritten = false;
            _rowsAdded = 0;
            // add root element
            _variableXML.Append("<root> ");
        }

        public StringBuilder XMLDoc
        {
            get { return _variableXML; }
            set { _variableXML = value; }
        }
        public bool WriteXML
        {
            get { return _writeXML; }
            set { _writeXML = value; }
        }
        public bool ValuesWritten
        {
            get { return _valuesWritten; }
            set { _valuesWritten = value; }
        }
        public bool EndTagAdded
        {
            get { return _endTagAdded; }
            set { _endTagAdded = value; }
        }
        public int RowsAdded
        {
            get { return _rowsAdded; }
            set { _rowsAdded = value; }
        }
    }
}
