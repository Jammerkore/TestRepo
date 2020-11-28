using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
    public partial class DatabaseSchema
    {
        // Fields

        private static DatabaseAccess _dba;
        private static Dictionary<string, DataTable> _dicSchemaDataTable;
        private static ReaderWriterLockSlim dictionaryLock;

        static DatabaseSchema()
		{
			_dba = new DatabaseAccess();
            _dicSchemaDataTable = new Dictionary<string, DataTable>();
            dictionaryLock = Locks.GetLockInstance(LockRecursionPolicy.NoRecursion); //setup the lock;
		}

        public static DataTable GetTableSchema(string aTableName)
        {
            DataTable dt;
            try
            {
                bool found = false;
                using (new WriteLock(dictionaryLock))
                {
                    found = _dicSchemaDataTable.TryGetValue(aTableName, out dt);
                    if (!found || dt == null)
                    {
                        // Begin TT#3373 - JSmith - Save Store Forecast receive DBNull error
                        if (aTableName.Contains("_HISTORY_DAY") ||
                            aTableName.Contains("_HISTORY_WEEK") ||
                            aTableName.Contains("_FORECAST_WEEK"))
                        {
                            dt = GetTableSchemaFromType(aTableName);
                        }
                        else
                        {
                            // End TT#3373 - JSmith - Save Store Forecast receive DBNull error
                            switch (aTableName)
                            {
                                case Include.DBChainWeeklyHistoryReadType:
                                    dt = BuildDBChainWeeklyHistoryReadType();
                                    break;
                                case Include.DBChainWeeklyModifiedReadType:
                                    dt = BuildDBChainWeeklyModifiedReadType();
                                    break;
                                case Include.DBChainWeeklyForecastReadType:
                                    dt = BuildDBChainWeeklyForecastReadType();
                                    break;
                                case Include.DBChainWeeklyForecastReadLockType:
                                    dt = BuildDBChainWeeklyForecastReadLockType();
                                    break;
                                case Include.DBStoreDailyHistoryReadType:
                                    dt = BuildDBStoreDailyHistoryReadType();
                                    break;
                                case Include.DBStoreWeeklyHistoryReadType:
                                    dt = BuildDBStoreWeeklyHistoryReadType();
                                    break;
                                case Include.DBStoreWeeklyModifiedReadType:
                                    dt = BuildDBStoreWeeklyModifiedReadType();
                                    break;
                                case Include.DBStoreWeeklyForecastReadType:
                                    dt = BuildDBStoreWeeklyForecastReadType();
                                    break;
                                case Include.DBStoreWeeklyForecastReadLockType:
                                    dt = BuildDBStoreWeeklyForecastReadLockType();
                                    break;
                                default:
                                    dt = GetTableSchemaFromTableName(aTableName);
                                    break;
                            }
                        }
                        if (dt != null)
                        {
                            _dicSchemaDataTable[aTableName] = dt;
                        }
                    }
                    return dt.Clone();
                }
            }
            catch
            {
                throw;
            }
        }

        private static DataTable BuildDBChainWeeklyHistoryReadType()
        {
            DataTable dt = null;
            try
            {
                dt = new DataTable(Include.DBChainWeeklyHistoryReadType);
                dt.Columns.Add("HN_RID", typeof(int));
                dt.Columns.Add("TIME_ID", typeof(int));
                return dt;
            }
            catch
            {
                throw;
            }
        }

        private static DataTable BuildDBChainWeeklyModifiedReadType()
        {
            DataTable dt = null;
            try
            {
                dt = new DataTable(Include.DBChainWeeklyModifiedReadType);
                dt.Columns.Add("HN_RID", typeof(int));
                dt.Columns.Add("FV_RID", typeof(int));
                dt.Columns.Add("TIME_ID", typeof(int));
                return dt;
            }
            catch
            {
                throw;
            }
        }

        private static DataTable BuildDBChainWeeklyForecastReadType()
        {
            DataTable dt = null;
            try
            {
                dt = new DataTable(Include.DBChainWeeklyForecastReadType);
                dt.Columns.Add("HN_RID", typeof(int));
                dt.Columns.Add("FV_RID", typeof(int));
                dt.Columns.Add("TIME_ID", typeof(int));
                return dt;
            }
            catch
            {
                throw;
            }
        }

        private static DataTable BuildDBChainWeeklyForecastReadLockType()
        {
            DataTable dt = null;
            try
            {
                dt = new DataTable(Include.DBChainWeeklyForecastReadLockType);
                dt.Columns.Add("HN_RID", typeof(int));
                dt.Columns.Add("FV_RID", typeof(int));
                dt.Columns.Add("TIME_ID", typeof(int));
                return dt;
            }
            catch
            {
                throw;
            }
        }

        private static DataTable BuildDBStoreDailyHistoryReadType()
        {
            DataTable dt = null;
            try
            {
                dt = new DataTable(Include.DBStoreDailyHistoryReadType);
                dt.Columns.Add("HN_MOD", typeof(int));
                dt.Columns.Add("HN_RID", typeof(int));
                dt.Columns.Add("TIME_ID", typeof(int));
                return dt;
            }
            catch
            {
                throw;
            }
        }

        private static DataTable BuildDBStoreWeeklyHistoryReadType()
        {
            DataTable dt = null;
            try
            {
                dt = new DataTable(Include.DBStoreWeeklyHistoryReadType);
                dt.Columns.Add("HN_MOD", typeof(int));
                dt.Columns.Add("HN_RID", typeof(int));
                dt.Columns.Add("TIME_ID", typeof(int));
                return dt;
            }
            catch
            {
                throw;
            }
        }

        private static DataTable BuildDBStoreWeeklyModifiedReadType()
        {
            DataTable dt = null;
            try
            {
                dt = new DataTable(Include.DBStoreWeeklyModifiedReadType);
                dt.Columns.Add("HN_MOD", typeof(int));
                dt.Columns.Add("HN_RID", typeof(int));
                dt.Columns.Add("FV_RID", typeof(int));
                dt.Columns.Add("TIME_ID", typeof(int));
                return dt;
            }
            catch
            {
                throw;
            }
        }

        private static DataTable BuildDBStoreWeeklyForecastReadType()
        {
            DataTable dt = null;
            try
            {
                dt = new DataTable(Include.DBStoreWeeklyForecastReadType);
                dt.Columns.Add("HN_MOD", typeof(int));
                dt.Columns.Add("HN_RID", typeof(int));
                dt.Columns.Add("FV_RID", typeof(int));
                dt.Columns.Add("TIME_ID", typeof(int));
                return dt;
            }
            catch
            {
                throw;
            }
        }

        private static DataTable BuildDBStoreWeeklyForecastReadLockType()
        {
            DataTable dt = null;
            try
            {
                dt = new DataTable(Include.DBStoreWeeklyForecastReadLockType);
                dt.Columns.Add("HN_RID", typeof(int));
                dt.Columns.Add("FV_RID", typeof(int));
                dt.Columns.Add("TIME_ID", typeof(int));
                return dt;
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#3373 - JSmith - Save Store Forecast receive DBNull error
        private static DataTable GetTableSchemaFromType(string aTableName)
        {
            DataTable dt = null;
            int tableType = 0;
            try
            {
                if (aTableName == "CHAIN_FORECAST_WEEK_LOCK")
                {
                    tableType = 1;
                }
                else if (aTableName == "CHAIN_FORECAST_WEEK")
                {
                    tableType = 2;
                }
                else if (aTableName == "CHAIN_HISTORY_WEEK")
                {
                    tableType = 3;
                }
                else if (aTableName == "STORE_FORECAST_WEEK_LOCK")
                {
                    tableType = 4;
                }
                else if (aTableName.Contains("STORE_FORECAST_WEEK"))
                {
                    tableType = 5;
                }
                else if (aTableName.Contains("STORE_HISTORY_DAY"))
                {
                    tableType = 6;
                }
                else if (aTableName.Contains("STORE_HISTORY_WEEK"))
                {
                    tableType = 7;
                }

                if (tableType > 0)
                {
                    dt = GetTableSchemaFromType(tableType, aTableName);
                }

                return dt;
            }
            catch
            {
                throw;
            }
        }
        // End TT#3373 - JSmith - Save Store Forecast receive DBNull error

        /// <summary>
        /// Get the columns defined to a table.
        /// </summary>
        /// <param name="aTableName"></param>
        /// <returns>Returns an empty DataTable containing the columns of the table</returns>
        private static DataTable GetTableSchemaFromTableName(string aTableName)
        {
            try
            {
                //// Begin TT#3235 - JSmith - Cancel Allocation Action is not working
                ////string SQLCommand = "select top 0 * from " + aTableName;
                //string SQLCommand = "select top 0 * from " + aTableName + " with (nolock)";
                //// Begin TT#3235 - JSmith - Cancel Allocation Action is not working
                //return ExecuteSQLQuery(SQLCommand, aTableName);

                return StoredProcedures.MID_TABLE_READ_SCHEMA.Read(_dba, TABLE_NAME: aTableName);
                
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#3373 - JSmith - Save Store Forecast receive DBNull error
        /// <summary>
        /// Get the columns defined to a SQL Type.
        /// </summary>
        /// <param name="aTableType"></param>
        /// <param name="aTableName"></param>
        /// <returns>Returns an empty DataTable containing the columns of the type</returns>
        private static DataTable GetTableSchemaFromType(int aTableType, string aTableName)
        {
            try
            {
                //MIDDbParameter[] inParams = { new MIDDbParameter("@TABLE_TYPE", aTableType, eDbType.Int, eParameterDirection.Input) };
                //return ExecuteQuery("SP_MID_GET_TABLE_FROM_TYPE", aTableName, inParams);
                return StoredProcedures.SP_MID_GET_TABLE_FROM_TYPE.Read(_dba, TABLE_TYPE: aTableType);
            }
            catch
            {
                throw;
            }
        }
        // End TT#3373 - JSmith - Save Store Forecast receive DBNull error
    }

 
}
