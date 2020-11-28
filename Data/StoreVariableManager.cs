using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
    public static partial class MaxStoresHelper
    {
        private static int _storeMAX_RID;
        /// <summary>
        /// Gets the smaller of the largest Store RID defined on the database or the largest valid store RID yet encountered that is greater or equal to the specified store RID
        /// </summary>
        /// <param name="aStoreRID">Specified Store RID</param>
        /// <returns>A vaild store RID that is larger than the specified store RID</returns>
        /// <remarks>A store RID is valid if its value is less than the largest store RID defined on the database.</remarks>
        public static int GetStoreMaxRID(int aStoreRID)
        {
            if (aStoreRID > _storeMAX_RID
                || _storeMAX_RID == 0)
            {
                ReadStoreMaxRID();
                if (aStoreRID > _storeMAX_RID)
                {
                    throw new Exception("Store RID = '" + _storeMAX_RID.ToString() + "' is larger than MAX Store RID '" + _storeMAX_RID.ToString());
                }

            }
            return Math.Max(_storeMAX_RID, aStoreRID);
        }

        public static void Refresh()
        {
            ReadStoreMaxRID();
        }

        private static object _readStoreMaxRIDLock = new object();        // TT#707 - JEllis - Container not thread safe
        /// <summary>
        /// Get the largest store RID defined on the database
        /// </summary>
        private static void ReadStoreMaxRID()
        {
            lock (_readStoreMaxRIDLock)        // TT#707 - JEllis - Container not thread safe
            {
                //DatabaseAccess _dba = new DatabaseAccess(MIDConfigurationManager.AppSettings["ConnectionString"]);
                DatabaseAccess _dba = new DatabaseAccess(MIDConnectionString.ConnectionString);
                //MIDStorageTypeDataAccess dataAccess = new MIDStorageTypeDataAccess();
                DataTable dt = StoredProcedures.MID_STORES_READ_MAX.Read(_dba);
                if (dt.Rows.Count > 0)
                {
                    _storeMAX_RID = Convert.ToInt32(dt.Rows[0]["MAX_STORE_RID"]);
                }
            }        // TT#707 - JEllis - Container not thread safe
        }
    }

    //public class StoreHistoryVariableManager
    //{
    //    DatabaseAccess _dba;
    //    private static object _lock = new object();

    //    private static DataTable dtDayTemplate;
    //    private static DataTable dtWeekTemplate;
    //    private static int numberOfHistoryTables;

    //    public StoreHistoryVariableManager()
    //    {
    //        lock (_lock)   
    //        {
    //            DatabaseAccess _dba = new DatabaseAccess(MIDConfigurationManager.AppSettings["ConnectionString"]);
    //            //MIDStorageTypeDataAccess dataAccess = new MIDStorageTypeDataAccess();
    //            string SQLCommand = "select top 1 * from STORE_HISTORY_DAY0";
    //            dtDayTemplate = _dba.ExecuteSQLQuery(SQLCommand, "GET_STORE_HISTORY_DAY_TEMPLATE");
    //            SQLCommand = "select top 1 * from STORE_HISTORY_WEEK0";
    //            dtWeekTemplate = _dba.ExecuteSQLQuery(SQLCommand, "GET_STORE_HISTORY_WEEK_TEMPLATE");
    //            SQLCommand = "SELECT STORE_TABLE_COUNT FROM SYSTEM_OPTIONS";
    //            DataTable dt = _dba.ExecuteSQLQuery(SQLCommand, "GET_STORE_HISTORY_TABLE_COUNT");
    //            if (dt.Rows.Count > 0)
    //            {
    //                numberOfHistoryTables = Convert.ToInt32(dt.Rows[0]["STORE_TABLE_COUNT"]);
    //            }
    //        }       
    //    }

    //    //private StoreVariableDataDictionary<HistoryDatabaseBinKey> _storeVariableDayContainer;
    //    //private StoreVariableDataDictionary<HistoryDatabaseBinKey> _storeVariableWeekContainer;

    //    // begin TT#467 Enq Conflict Except During Size Hist Load
    //    /// <summary>
    //    /// Locks Time, Style, Size, Color as in-use by the specified User.
    //    /// </summary>
    //    /// <param name="aUserRID">RID that identifies the user</param>
    //    /// <param name="aStyleHnRIDs">Array of Style Hierarchy RIDs to be locked</param>
    //    /// <returns>True when successful; False otherwise</returns>
    //    public bool LockTimeHnRIDNode(int aUserRID, SQL_TimeID[] aSQL_TimeIDs, int[] aHnRIDs, int[] aStyleHnRIDs, int[] aColorCodeRIDs, int[] aSizeCodeRIDs)
    //    {
    //        //int retryCount = 0;
    //        //bool retry = true;

    //        //while (retry)
    //        //{
    //        //    try
    //        //    {
    //        //        if (LockTimeHnRIDNode2(aUserRID, aSQL_TimeIDs, aHnRIDs, aStyleHnRIDs, aColorCodeRIDs, aSizeCodeRIDs))
    //        //        {
    //        //            retry = false;
    //        //            return true;
    //        //        }
    //        //    }
    //        //    catch (EnqueueConflictException)
    //        //    {
    //        //        ++retryCount;
    //        //        if (retryCount < _maximumRetryAttempts)
    //        //        {
    //        //            System.Threading.Thread.Sleep(_retrySleepTime);
    //        //        }
    //        //        else
    //        //        {
    //        //            retry = false;
    //        //            throw;
    //        //        }
    //        //    }
    //        //    catch
    //        //    {
    //        //        retry = false;
    //        //        throw;
    //        //    }
    //        //}

    //        return true; //return false
    //    }

    //    //private bool LockTimeHnRIDNode2(int aUserRID, SQL_TimeID[] aSQL_TimeIDs, int[] aHnRIDs, int[] aStyleHnRIDs, int[] aColorCodeRIDs, int[] aSizeCodeRIDs)
    //    //{
    //    //    bool returnCode = false;
    //        //if (_updateAccess)
    //        //{
    //        //    if (aSQL_TimeIDs.Length != aHnRIDs.Length
    //        //        || aSQL_TimeIDs.Length != aStyleHnRIDs.Length
    //        //        || aSQL_TimeIDs.Length != aColorCodeRIDs.Length
    //        //        || aSQL_TimeIDs.Length != aSizeCodeRIDs.Length)
    //        //    {
    //        //        throw new ArgumentException("LockTimeHnRIDNode parameter arrays must contain same number of entries");
    //        //    }
    //        //    DataTable lockTable;
    //        //    try
    //        //    {
    //        //        _lockConflictList.Clear();
    //        //        // begin TT#467 Enq Conflict During Size Hist Load
    //        //        //lockTable = Lock_Read(aStyleHnRIDs);  
    //        //        //foreach (DataRow dataRow in lockTable.Rows)
    //        //        //{
    //        //        //    _lockConflictList.Add(
    //        //        //        new MIDEnqueueConflict(
    //        //        //            Convert.ToInt32(dataRow["ENQUEUE_TYPE"], CultureInfo.CurrentCulture),
    //        //        //            Convert.ToInt32(dataRow["RID"], CultureInfo.CurrentUICulture),
    //        //        //            Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture),
    //        //        //            Convert.ToInt32(dataRow["OWNING_THREADID"], CultureInfo.CurrentUICulture)));
    //        //        //}
    //        //        string[] keyLabel = new string[5];
    //        //        keyLabel[1] = " Size Hierarchy Node RID ";
    //        //        keyLabel[2] = " in Style Hierarchy Node RID ";
    //        //        keyLabel[3] = " Color Code RID ";
    //        //        keyLabel[4] = " Size Code RID ";
    //        //        int[] keys = new int[5];
    //        //        for (int i = 0; i < aSQL_TimeIDs.Length; i++)
    //        //        {
    //        //            lockTable = _midEnqueue.StoreVariableHistory_EnqueueRead(aSQL_TimeIDs[i], aHnRIDs[i]);
    //        //            foreach (DataRow dataRow in lockTable.Rows)
    //        //            {
    //        //                keys[0] = aSQL_TimeIDs[i].SqlTimeID;
    //        //                keys[1] = aHnRIDs[i];
    //        //                keys[2] = aStyleHnRIDs[i];
    //        //                keys[3] = aColorCodeRIDs[i];
    //        //                keys[4] = aSizeCodeRIDs[i];
    //        //                if (aSQL_TimeIDs[i].TimeIdType == eSQLTimeIdType.TimeIdIsDaily)
    //        //                {
    //        //                    keyLabel[0] = " Daily Time ID ";
    //        //                }
    //        //                else
    //        //                {
    //        //                    keyLabel[0] = " Weekly Time ID ";
    //        //                }
    //        //                _lockConflictList.Add(
    //        //                    new MIDEnqueueConflict(
    //        //                        Convert.ToInt32(dataRow["ENQUEUE_TYPE"], CultureInfo.CurrentUICulture),
    //        //                        keyLabel,
    //        //                        keys,
    //        //                        Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture),
    //        //                        Convert.ToInt32(dataRow["OWNING_THREADID"], CultureInfo.CurrentUICulture)));
    //        //            }
    //        //        }

    //        //        if (_lockConflictList.Count > 0)
    //        //        {
    //        //            // Start TT#522 - stodd - size day to week summary
    //        //            if (!EventLog.SourceExists("MIDStoreVariableHistoryBin"))
    //        //            {
    //        //                EventLog.CreateEventSource("MIDStoreVariableHistoryBin", null);
    //        //            }
    //        //            foreach (MIDEnqueueConflict conflict in _lockConflictList)
    //        //            {
    //        //                string labels = string.Empty;
    //        //                string keysInCon = string.Empty;
    //        //                foreach (string st in conflict.KeyLabel)
    //        //                {
    //        //                    labels = labels + st + " ";
    //        //                }
    //        //                foreach (int iKey in conflict.Key_InConflict)
    //        //                {
    //        //                    keysInCon = keysInCon + iKey + " ";
    //        //                }
    //        //                string msg = "Lock Conflict. Lock Type: " + conflict.LockType + " User: " + conflict.OwnedByUserID + " Labels: " + labels +
    //        //                    " Keys: " + keysInCon;
    //        //                EventLog.WriteEntry("MIDStoreVariableHistoryBin", msg, EventLogEntryType.Error);
    //        //            }
    //        //            // End TT#522 - stodd - size day to week summary

    //        //            throw new EnqueueConflictException();
    //        //        }
    //        //        else
    //        //        {
    //        //            HistoryDatabaseBinKey lockKey;
    //        //            List<HistoryDatabaseBinKey> dayKeys = new List<HistoryDatabaseBinKey>();
    //        //            List<HistoryDatabaseBinKey> weekKeys = new List<HistoryDatabaseBinKey>();
    //        //            _midEnqueue.OpenUpdateConnection();
    //        //            //int[] sizeHnUser = new int[2];
    //        //            int[] sizeHnUser;
    //        //            for (int i = 0; i < aSQL_TimeIDs.Length; i++)
    //        //            {
    //        //                lockKey = new HistoryDatabaseBinKey((short)aSQL_TimeIDs[i].SqlTimeID, aStyleHnRIDs[i], aColorCodeRIDs[i], aSizeCodeRIDs[i]);
    //        //                _midEnqueue.StoreVariableHistory_EnqueueInsert(aSQL_TimeIDs[i], aHnRIDs[i], aUserRID, _clientThreadID);
    //        //                sizeHnUser = new int[2];
    //        //                sizeHnUser[0] = aHnRIDs[i];
    //        //                sizeHnUser[1] = aUserRID;
    //        //                if (aSQL_TimeIDs[i].TimeIdType == eSQLTimeIdType.TimeIdIsDaily)
    //        //                {
    //        //                    dayKeys.Add(lockKey);
    //        //                    _lockDayDictionary.Add(lockKey, sizeHnUser);
    //        //                }
    //        //                else
    //        //                {
    //        //                    weekKeys.Add(lockKey);
    //        //                    _lockWeekDictionary.Add(lockKey, sizeHnUser);
    //        //                }
    //        //            }
    //        //            _midEnqueue.CommitData();
    //        //            Flush(dayKeys, weekKeys);   // TT#173 Provide database container for large data collections
    //        //            returnCode = true;
    //        //        }
    //        //    }
    //        //    catch
    //        //    {
    //        //        returnCode = false;
    //        //        throw;
    //        //    }
    //        //    finally
    //        //    {
    //        //        if (_midEnqueue.ConnectionIsOpen)
    //        //        {
    //        //            _midEnqueue.CloseUpdateConnection();
    //        //        }
    //        //    }
    //        //}
    //    //    return returnCode;
    //    //}

    //    /// <summary>
    //    /// Unlocks specified Hierarchy Nodes
    //    /// </summary>
    //    /// <param name="aHnRID">List of the Hierarchy Nodes to be UnLocked</param>
    //    //public void UnLockStyleNode(int[] aStyleHnRID)
    //    public void UnLockTimeHnRID(SQL_TimeID[] aSQL_TimeIDs, int[] aStyleHnRIDs, int[] aColorCodeRIDs, int[] aSizeCodeRIDs)
    //    {
    //        if (aSQL_TimeIDs.Length != aStyleHnRIDs.Length
    //            || aSQL_TimeIDs.Length != aColorCodeRIDs.Length
    //            || aSQL_TimeIDs.Length != aSizeCodeRIDs.Length)
    //        {
    //            throw new ArgumentException("LockTimeHnRIDNode parameter arrays must contain same number of entries");
    //        }
    //        //try
    //        //{
    //        //    _midEnqueue.OpenUpdateConnection();
    //        //    for (int i = 0; i < aSQL_TimeIDs.Length; i++)
    //        //    {
    //        //        try
    //        //        {
    //        //            HistoryDatabaseBinKey key = new HistoryDatabaseBinKey((short)aSQL_TimeIDs[i].SqlTimeID, aStyleHnRIDs[i], aColorCodeRIDs[i], aSizeCodeRIDs[i]);
    //        //            if (aSQL_TimeIDs[i].TimeIdType == eSQLTimeIdType.TimeIdIsDaily)
    //        //            {
    //        //                int[] userHnRID = _lockDayDictionary[key];
    //        //                _midEnqueue.StoreVariableHistory_EnqueueDelete(aSQL_TimeIDs[i], userHnRID[0], userHnRID[1]);
    //        //                _lockDayDictionary.Remove(key);
    //        //            }
    //        //            else
    //        //            {
    //        //                int[] userHnRID = _lockWeekDictionary[key];
    //        //                _midEnqueue.StoreVariableHistory_EnqueueDelete(aSQL_TimeIDs[i], userHnRID[0], userHnRID[1]);
    //        //                _lockWeekDictionary.Remove(key);
    //        //            }
    //        //            _midEnqueue.CommitData();
    //        //        }
    //        //        catch
    //        //        {
    //        //        }
    //        //    }
    //        //    //for (int i = 0; i < aStyleHnRID.Length; i++)
    //        //    //{
    //        //    //    try
    //        //    //    {
    //        //    //        int userRID = _lockDictionary[aStyleHnRID[i]];
    //        //    //        _midEnqueue.Enqueue_Delete(eLockType.StoreVariableHistory, aStyleHnRID[i], userRID);
    //        //    //        _lockDictionary.Remove(aStyleHnRID[i]);
    //        //    //        _midEnqueue.CommitData();
    //        //    //    }
    //        //    //    catch
    //        //    //    {
    //        //    //    }
    //        //    //}
    //        //}
    //        //finally
    //        //{
    //        //    _midEnqueue.CloseUpdateConnection();
    //        //}
    //    }
    //    /// <summary>
    //    /// Removes all Hierarchy Node Locks created by this instance of this class
    //    /// </summary>
    //    public void RemoveAllLocks()
    //    {
    //        //HistoryDatabaseBinKey[] historyKeys;
    //        //SQL_TimeID[] sqlTimeIDs;
    //        //int[] styleHnRIDs;
    //        //int[] colorCodeRIDs;
    //        //int[] sizeCodeRIDs;
    //        //if (_lockDayDictionary.Count > 0)
    //        //{
    //        //    historyKeys = new HistoryDatabaseBinKey[_lockDayDictionary.Count];
    //        //    _lockDayDictionary.Keys.CopyTo(historyKeys, 0);
    //        //    sqlTimeIDs = new SQL_TimeID[historyKeys.Length];
    //        //    styleHnRIDs = new int[historyKeys.Length];
    //        //    colorCodeRIDs = new int[historyKeys.Length];
    //        //    sizeCodeRIDs = new int[historyKeys.Length];
    //        //    for (int i = 0; i < historyKeys.Length; i++)
    //        //    {
    //        //        sqlTimeIDs[i] = new SQL_TimeID('D', historyKeys[i].TimeID);
    //        //        styleHnRIDs[i] = historyKeys[i].HnRID;
    //        //        colorCodeRIDs[i] = historyKeys[i].ColorCodeRID;
    //        //        sizeCodeRIDs[i] = historyKeys[i].SizeCodeRID;
    //        //    }
    //        //    UnLockTimeHnRID(sqlTimeIDs, styleHnRIDs, colorCodeRIDs, sizeCodeRIDs);
    //        //}
    //        //if (_lockWeekDictionary.Count > 0)
    //        //{
    //        //    historyKeys = new HistoryDatabaseBinKey[_lockWeekDictionary.Count];
    //        //    _lockDayDictionary.Keys.CopyTo(historyKeys, 0);
    //        //    sqlTimeIDs = new SQL_TimeID[historyKeys.Length];
    //        //    styleHnRIDs = new int[historyKeys.Length];
    //        //    colorCodeRIDs = new int[historyKeys.Length];
    //        //    sizeCodeRIDs = new int[historyKeys.Length];
    //        //    for (int i = 0; i < historyKeys.Length; i++)
    //        //    {
    //        //        sqlTimeIDs[i] = new SQL_TimeID('W', historyKeys[i].TimeID);
    //        //        styleHnRIDs[i] = historyKeys[i].HnRID;
    //        //        colorCodeRIDs[i] = historyKeys[i].ColorCodeRID;
    //        //        sizeCodeRIDs[i] = historyKeys[i].SizeCodeRID;
    //        //    }
    //        //    UnLockTimeHnRID(sqlTimeIDs, styleHnRIDs, colorCodeRIDs, sizeCodeRIDs);
    //        //}
    //    }

    //    /// <summary>
    //    /// Commits updates to the database
    //    /// </summary>
    //    /// <param name="aStatusMessage">Commit status message (additional information about the Commit)</param>
    //    /// <returns>True: Commit was successful; False: Commit failed</returns>
    //    public bool Commit(out string aStatusMessage)
    //    {
    //        //if (_updateAccess)
    //        //{
    //            //try
    //            //{
    //            //    // begin T#555 Total variable is aggregate of other variables
    //            //    foreach (StoreVariableData<HistoryDatabaseBinKey> svd in _storeVariableDayContainer.Values)
    //            //    {
    //            //        svd.ResetCalcVariables();
    //            //    }
    //            //    foreach (StoreVariableData<HistoryDatabaseBinKey> svd in _storeVariableWeekContainer.Values)
    //            //    {
    //            //        svd.ResetCalcVariables();
    //            //    }
    //            //    // end TT#555 Total variable is aggregate of other variables
    //            //    this.OpenUpdateConnection(eLockType.StoreVariableHistory, this.GetType().Name);
    //            //    // begin TT#467 Change Store Container Enqueue -- Part 2
    //            //    //HistoryDayDatabaseBin.WriteStoreVariables(_dba, _storeVariableDayContainer);
    //            //    //HistoryWeekDatabaseBin.WriteStoreVariables(_dba, _storeVariableWeekContainer);
    //            //    HistoryDatabaseBinKey[] keysToWrite;
    //            //    if (_lockDayDictionary.Count > 0)
    //            //    {
    //            //        keysToWrite = new HistoryDatabaseBinKey[_lockDayDictionary.Count];
    //            //        _lockDayDictionary.Keys.CopyTo(keysToWrite, 0);
    //            //        GetHistoryDayDatabaseBin().WriteStoreVariables(_dba, _storeVariableDayContainer, keysToWrite); // TT#707 - JEllis - Container not thread safe (part 2)
    //            //    }
    //            //    if (_lockWeekDictionary.Count > 0)
    //            //    {
    //            //        keysToWrite = new HistoryDatabaseBinKey[_lockWeekDictionary.Count];
    //            //        _lockWeekDictionary.Keys.CopyTo(keysToWrite, 0);
    //            //        GetHistoryWeekDatabaseBin().WriteStoreVariables(_dba, _storeVariableWeekContainer, keysToWrite); // TT#707 - JEllis - Container not thread safe (part 2)
    //            //    }
    //            //    // end TT#467 Change Store Container Enqueue -- Part 2
    //            //    this.CommitData();
    //            //    foreach (StoreVariableData<HistoryDatabaseBinKey> svd in _storeVariableDayContainer.Values)
    //            //    {
    //            //        svd.CommitStatus();
    //            //    }
    //            //    foreach (StoreVariableData<HistoryDatabaseBinKey> svd in _storeVariableWeekContainer.Values)
    //            //    {
    //            //        svd.CommitStatus();
    //            //    }
    //                aStatusMessage = "StoreVariableHistory COMMIT was successful";
    //            //}
    //            //finally
    //            //{
    //            //    if (ConnectionIsOpen)
    //            //    {
    //            //        this.CloseUpdateConnection();
    //            //    }
    //            //}
    //            return true;
    //        //}
    //        //aStatusMessage = "StoreVariableHistory Bin COMMIT Failed because StoreVariableHistoryBin was not instantiated for update";
    //        //return false;
    //    }
    //    /// <summary>
    //    /// Sets the Color-Size data values for an array of stores within the given Style for the specified variable at the specified TimeID; The Store and Variable arrays must be in one-to-one correspondence
    //    /// </summary>
    //    /// <param name="aDatabaseVariableName">Name of a database Bin Variable</param>
    //    /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
    //    /// <param name="aTimeID">Time ID</param>
    //    /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
    //    /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
    //    /// <param name="aStoreRIDs">RIDs that identify the stores</param>
    //    /// <param name="aVariableValues">Variable's data values for the given stores in the given Color-Size and Time</param>
    //    public void SetStoreVariableValue
    //         (string aDatabaseVariableName,
    //         int aHnRID,
    //         SQL_TimeID aTimeID,
    //         int aColorCodeRID,
    //         int aSizeCodeRID,
    //         int[] aStoreRIDs,
    //         double[] aVariableValues
    //         )
    //    {
    //        if (aTimeID.TimeIdType == eSQLTimeIdType.TimeIdIsDaily)
    //        {
    //            SetStoreVariableDayValue
    //                (aDatabaseVariableName,
    //                aHnRID,
    //                aTimeID,
    //                aColorCodeRID,
    //                aSizeCodeRID,
    //                aStoreRIDs,
    //                aVariableValues
    //                );
    //        }
    //        else
    //        {
    //            SetStoreVariableWeekValue
    //                (aDatabaseVariableName,
    //                aHnRID,
    //                aTimeID,
    //                aColorCodeRID,
    //                aSizeCodeRID,
    //                aStoreRIDs,
    //                aVariableValues
    //                );
    //        }
    //    }

    //    /// <summary>
    //    /// Sets the Color-Size DAY data values for an array of stores within the given Style for the specified variable INDEX (Ordinal, column number) at the specified TimeID; The Store and Variable arrays must be in one-to-one correspondence
    //    /// </summary>
    //    /// <param name="aVariableIDX">Database Variable INDEX (Ordinal) of the variable (the column number where the variable resides</param>
    //    /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
    //    /// <param name="aTimeID">Time ID</param>
    //    /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
    //    /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
    //    /// <param name="aStoreRIDs">RIDs that identify the stores</param>
    //    /// <param name="aVariableValues">Variable's data values for the given stores in the given Color-Size and Time</param>
    //    public void SetStoreVariableDayValue
    //        (string aDatabaseVariableName,// short aVariableIDX,
    //         int aHnRID,
    //         SQL_TimeID aTimeID,
    //         int aColorCodeRID,
    //         int aSizeCodeRID,
    //         int[] aStoreRIDs,
    //         double[] aVariableValues)
    //    {
    //        //if (!_updateAccess)
    //        //{
    //        //    throw new Exception("'update access' was not declared when StoreVariableHistBin was instantiated");
    //        //}
    //        if (aStoreRIDs.Length != aVariableValues.Length)
    //        {
    //            throw new Exception("Store array length = " + aStoreRIDs.Length + " is not same as Variable array length = " + aVariableValues.Length);
    //        }
    //        //// begin TT#467 Enq Conflict Except During Size Hist Load
    //        ////if (this._lockDictionary.ContainsKey(aHnRID))
    //        ////{
    //        int storeRID;
    //        Int64 variableValue;

    //        HistoryDatabaseBinKey dayHistoryKey = new HistoryDatabaseBinKey((Int16)aTimeID.SqlTimeID, aHnRID, aColorCodeRID, aSizeCodeRID);

    //        //// begin TT#555 Total variable is aggregate of other variables
    //        ////StoreVariableVectorContainer svvc = GetStoreVariableVectorContainer(aTimeID, aHnRID, aColorCodeRID, aSizeCodeRID);
    //        //StoreVariableData<HistoryDatabaseBinKey> svd = GetStoreVariableData(aTimeID, aHnRID, aColorCodeRID, aSizeCodeRID);
    //        //StoreVariableVectorContainer svvc = svd.StoreVariableVectorContainer;
    //        //// end TT#555 Total variable is aggregate of other variables
    //        //bool processSet = false;

    //        //if (_lastTimeIDType == eSQLTimeIdType.TimeIdIsDaily)
    //        //{
    //        //    // begin TT#555 Total variable is aggregate of other variables
    //        //    //if (_lockDayDictionary.ContainsKey(_lastDatabaseBinKey))
    //        //    if (_lockDayDictionary.ContainsKey(svd.DatabaseBinKey))
    //        //    // end TT#555 Total variable is aggregate of othe variables
    //        //    {
    //        //        processSet = true;
    //        //    }
    //        //}
    //        //else
    //        //{
    //        //    // begin TT#555 Total variable is aggregate of other variables
    //        //    //if (_lockWeekDictionary.ContainsKey(_lastDatabaseBinKey))
    //        //    if (_lockWeekDictionary.ContainsKey(svd.DatabaseBinKey))
    //        //    // end TT#555 Total variable is aggregate of other variables
    //        //    {
    //        //        processSet = true;
    //        //    }
    //        //}
    //        //if (processSet)
    //        //{
    //        //    eMIDVariableModelType variableModelType = (eMIDVariableModelType)GetHistoryDayDatabaseBin().VariableModel.VariableModelID; // TT#707 - JEllis - Container not thread safe (part 2)
    //        //    double multiplier = Math.Pow(10, GetHistoryDayDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision); // TT#707 - JEllis - Container not thread safe (part 2)
    //        //    svd.InitialCalcVariables = true; // TT#555 Total variable is aggregate of other variables

    //        double multiplier = Math.Pow(10, StoreHistoryVariables.Find(aDatabaseVariableName).DecimalPrecision);
            

    //            for (int i = 0; i < aStoreRIDs.Length; i++)
    //            {
    //                storeRID = aStoreRIDs[i];
    //                if (aVariableValues[i] < 0)
    //                {
    //                    variableValue =
    //                       (Int64)(aVariableValues[i]
    //                               * multiplier
    //                               - .5);   // rounding should use same sign as original value
    //                }
    //                else
    //                {
    //                    variableValue =
    //                        (Int64)(aVariableValues[i]
    //                                * multiplier
    //                                + .5);
    //                }

    //                InsertOrUpdateDayRow(GetDayTableIndexFromHistoryKey(dayHistoryKey),aHnRID, aTimeID.SqlTimeID, storeRID, aDatabaseVariableName, (int)variableValue);
    //        //        svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
    //        //        svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
    //            }
    //            return;
    //        //}

    //        //throw new Exception("update failed because Time/Hierarchy Node RID is not locked. TimeID='" + aTimeID.ToString() + "' Hierarchy Node RID='" + aHnRID.ToString() + "'" + " Color Code RID='" + aColorCodeRID.ToString() + "' Size Code RID='" + aSizeCodeRID.ToString() + "'");
    //    }


    //    private DataSet dsDayVariables; //holds a dataTable for each History Key
    //    private DataSet dsWeekVariables; //holds a dataTable for each History Key
    //    private List<HistoryDatabaseBinKey> dayKeyList = new List<HistoryDatabaseBinKey>();
    //    private List<HistoryDatabaseBinKey> weekKeyList = new List<HistoryDatabaseBinKey>();
    //    private void AddDayHistoryKey(HistoryDatabaseBinKey dayHistoryKey)
    //    {
    //        bool keyExists = dayKeyList.Exists(
    //             delegate(HistoryDatabaseBinKey o)
    //             {
    //                 return o == dayHistoryKey;
    //             }
    //             );

    //        if (!keyExists)
    //        {
    //            dayKeyList.Add(dayHistoryKey);
    //            AddDayTableForKey();
    //        }
    //    }
    //    private void AddWeekHistoryKey(HistoryDatabaseBinKey weekHistoryKey)
    //    {
    //        bool keyExists = weekKeyList.Exists(
    //             delegate(HistoryDatabaseBinKey o)
    //             {
    //                 return o == weekHistoryKey;
    //             }
    //             );

    //        if (!keyExists)
    //        {
    //            weekKeyList.Add(weekHistoryKey);
    //            AddWeekTableForKey();
    //        }
    //    }
    

    //    private void AddDayTableForKey()
    //    {
    //        dsDayVariables.Tables.Add(StoreHistoryVariableManager.dtDayTemplate);
    //    }
    //    private void AddWeekTableForKey()
    //    {
    //        dsWeekVariables.Tables.Add(StoreHistoryVariableManager.dtWeekTemplate);
    //    }
    //    private int GetDayTableIndexFromHistoryKey(HistoryDatabaseBinKey dayHistoryKey)
    //    {
    //        return dayKeyList.IndexOf(dayHistoryKey);
    //    }
    //    private int GetWeekTableIndexFromHistoryKey(HistoryDatabaseBinKey weekHistoryKey)
    //    {
    //        return weekKeyList.IndexOf(weekHistoryKey);
    //    }
    //    private void InsertOrUpdateDayRow(int tableIndex, 
    //         int aHnRID,
    //         int aTimeID,
    //         int aStoreRID,
    //         string variableDatabaseName,
    //         int variableValue)
    //    {
    //        DataRow dr;

    //        DataRow[] drExists = dsDayVariables.Tables[tableIndex].Select("HN_RID=" + aHnRID.ToString() + " AND TIME_ID=" + aTimeID.ToString() + " AND ST_RID=" + aStoreRID.ToString());
    //        if (drExists.Length > 0)
    //        {
    //            dr = drExists[0];
    //        }
    //        else
    //        {
    //            dr = dsDayVariables.Tables[tableIndex].NewRow();
    //            dr["HN_MOD"] = aHnRID % StoreHistoryVariableManager.numberOfHistoryTables;
    //            dr["HN_RID"] = aHnRID;
    //            dr["TIME_ID"] = aTimeID;
    //            dr["ST_RID"] = aStoreRID;
    //        }
    //        dr[variableDatabaseName] = variableValue;
    //    }
    //    private void WriteDayVariablesToDatabase()
    //    {
    //        foreach (DataTable dt in dsDayVariables.Tables)
    //        {

    //        }
    //    }

    //    /// <summary>
    //    /// Sets the Color-Size WEEK data values for an array of stores within the given Style for the specified variable at the specified TimeID; The Store and Variable arrays must be in one-to-one correspondence
    //    /// </summary>
    //    /// <param name="aVariableIDX">Database Variable INDEX (Ordinal) of the variable (the column number where the variable resides</param>
    //    /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
    //    /// <param name="aTimeID">Time ID</param>
    //    /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
    //    /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
    //    /// <param name="aStoreRIDs">RIDs that identify the stores</param>
    //    /// <param name="aVariableValues">Variable's data values for the given stores in the given Color-Size and Time</param>
    //    public void SetStoreVariableWeekValue
    //        (string aDatabaseVariableName,// short aVariableIDX
    //        int aHnRID,
    //        SQL_TimeID aTimeID,
    //        int aColorCodeRID,
    //        int aSizeCodeRID,
    //        int[] aStoreRIDs,
    //        double[] aVariableValues
    //        )
    //    {
    //        //if (!_updateAccess)
    //        //{
    //        //    throw new Exception("'update access' was not declared when SizeVariableQuickAccess was instantiated");
    //        //}
    //        //if (aStoreRIDs.Length != aVariableValues.Length)
    //        //{
    //        //    throw new Exception("Store array length = " + aStoreRIDs.Length + " is not same as Variable array length = " + aVariableValues.Length);
    //        //}
    //        //// begin TT#467 Enq Conflict Except During Size Hist Load
    //        ////if (_lastLockHnRID == aHnRID
    //        ////    || this._lockDictionary.ContainsKey(aHnRID))
    //        ////{
    //        ////    _lastLockHnRID = aHnRID;
    //        //int storeRID;
    //        //Int64 variableValue;

    //        //// begin TT#555 Total variable is aggregate of other variables
    //        ////StoreVariableVectorContainer svvc = GetStoreVariableVectorContainer(aTimeID, aHnRID, aColorCodeRID, aSizeCodeRID);
    //        //StoreVariableData<HistoryDatabaseBinKey> svd = GetStoreVariableData(aTimeID, aHnRID, aColorCodeRID, aSizeCodeRID);
    //        //StoreVariableVectorContainer svvc = svd.StoreVariableVectorContainer;
    //        //// end TT#555 Total variable is aggregate of other variables
    //        //bool processSet = false;
    //        //if (_lastTimeIDType == eSQLTimeIdType.TimeIdIsDaily)
    //        //{
    //        //    // begin TT#555 Total variable is aggregate of other variables
    //        //    //if (_lockDayDictionary.ContainsKey(_lastDatabaseBinKey))
    //        //    if (_lockDayDictionary.ContainsKey(svd.DatabaseBinKey))
    //        //    // end TT#555 Total Variable is aggregate of other variables
    //        //    {
    //        //        processSet = true;
    //        //    }
    //        //}
    //        //else
    //        //{
    //        //    // begin TT#555 Total variable is aggregate of other variables
    //        //    //if (_lockWeekDictionary.ContainsKey(_lastDatabaseBinKey))
    //        //    if (_lockWeekDictionary.ContainsKey(svd.DatabaseBinKey))
    //        //    // end TT#555 Total variable is aggregate of other variables
    //        //    {
    //        //        processSet = true;
    //        //    }
    //        //}
    //        //if (processSet)
    //        //{
    //        //    // end TT#467 Enq Confict Except During Size Hist Load
    //        //    eMIDVariableModelType variableModelType = (eMIDVariableModelType)GetHistoryWeekDatabaseBin().VariableModel.VariableModelID;  // TT#707 - JEllis - Container not thread safe (part 2)
    //        //    double multiplier = Math.Pow(10, GetHistoryWeekDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision); // TT#707 - JEllis - Container not thread safe (part 2)
    //        //    svd.InitialCalcVariables = true; // TT#555 Total variable is aggregate of other variables
    //        //    for (int i = 0; i < aStoreRIDs.Length; i++)
    //        //    {
    //        //        storeRID = aStoreRIDs[i];
    //        //        if (aVariableValues[i] < 0)
    //        //        {
    //        //            variableValue =
    //        //               (Int64)(aVariableValues[i]
    //        //                       * multiplier
    //        //                       - .5);   // rounding should use same sign as original value
    //        //        }
    //        //        else
    //        //        {
    //        //            variableValue =
    //        //                (Int64)(aVariableValues[i]
    //        //                        * multiplier
    //        //                        + .5);
    //        //        }
    //        //        svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
    //        //    }
    //        //    return;
    //        //}
    //        //throw new Exception("update failed because Time/Hierarchy Node RID is not locked. TimeID='" + aTimeID.ToString() + "' Hierarchy Node RID='" + aHnRID.ToString() + "'" + " Color Code RID='" + aColorCodeRID.ToString() + "' Size Code RID='" + aSizeCodeRID.ToString() + "'");
    //    }
    //}

    //public class StoreDailyVariableManager
    //{
        //read variable values from normalized tables in database to memory

        //write variable values from memory to normalized tables in database

        //read variable values from memory

        //write variables values in memory
    //}

    //public class StoreWeeklyVariableManager
    //{
        //read variable values from normalized tables in database to memory

        //write variable values from memory to normalized tables in database

        //read variable values from memory

        //write variables values in memory
    //}

    /// <summary>
    /// Container for Store VSW Reverse Onhand Variables.
    /// </summary>
    public partial class VswReverseOnhandVariableManager
    {
       // private bool _isDisposed;
        private Dictionary<int, bool> _isHeaderCached;


      //  private DataTable _dtVSW_ReverseOnhandVariableData;

        /// <summary>
        /// Creates an instance of this class
        /// </summary>
        public VswReverseOnhandVariableManager()
        {
            //_isDisposed = false;
            _isHeaderCached = new Dictionary<int, bool>();
            //_vswReverseOnhandContainer = new StoreVariableDataDictionary<VswReverseOnhandDatabaseBinKey>();
        }

        public bool IsHeaderCached(int aHdrRID)
        {
            bool cached = false;
            if (_isHeaderCached.TryGetValue(aHdrRID, out cached))
            {
                return cached;
            }
            return false;
        }

    

   
        /// <summary>
        /// Gets the store's VSW Reverse Onhand value from memory for the given header, Hierarchy Node
        /// If not found in memory, retrieve from database
        /// If not found in database, create new value in memory and default to zero
        /// </summary>
        /// <param name="aVariableCol">Variable to get</param>
        /// <param name="aHdrRID">Header RID</param>
        /// <param name="aHnRID">VSW Reverse Onhand Hierarchy Node</param>
        /// <param name="aStoreRID">Store RID</param>
        /// <returns>Store's Vsw Reverse Onhand value.</returns>
        public double GetStoreVswReverseOnhandValue
            (VswReverseOnhandVariables aVariableCol,
            int aHdrRID,
            int aHnRID,
            int aStoreRID,
            bool returnZeroForNulls = true,
            bool returnZeroForNoRecord = false)
        {
            //int[] storeRID = new int[1];
            //storeRID[0] = aStoreRID;

            //DataRow[] drResult = _dtVSW_ReverseOnhandVariableData.Select("HDR_RID=" + aHdrRID + " AND HN_RID=" + aHnRID + " AND ST_RID=" + aStoreRID);
            //if (drResult.Length > 0)
            //{
            //    if (drResult[0][aVariableCol] != DBNull.Value)
            //    {
            //        return (double)drResult[0][aVariableCol];
            //    }
            //    else
            //    {
            //        //how to handle null values?
            //        if (returnZeroForNulls)
            //        {
            //            return 0;
            //        }
            //        else
            //        {
            //            throw new NoNullAllowedException("Null value for " + aVariableCol + ": HDR_RID=" + aHdrRID + " AND HN_RID=" + aHnRID + " AND ST_RID=" + aStoreRID);
            //        }
            //    }
            //}
            //else
            //{
            //    //how to handle record not found?
            //    if (returnZeroForNoRecord)
            //    {
            //        return 0;
            //    }
            //    else
            //    {
            //        throw new NoNullAllowedException("No record found for " + aVariableCol + ": HDR_RID=" + aHdrRID + " AND HN_RID=" + aHnRID + " AND ST_RID=" + aStoreRID);
            //    }
            //}

            if (IsHeaderCached(aHdrRID) == false)
            {
                ReadStoreVswReverseOnhand(aHdrRID);  //Reads all entries for the HdrRID from the database
            }
            VSW_HeaderStorage headerStorage = HeaderStorageFindInList(aHdrRID);
            if (headerStorage == null)
            {
                
                //throw new NoNullAllowedException("No header storage found for: HDR_RID=" + aHdrRID + " AND HN_RID=" + aHnRID + " AND ST_RID=" + aStoreRID);

                // if no entry is found in the database - create one and add it to memory
                // default units to zero
                headerStorage = new VSW_HeaderStorage();
                headerStorage.HDR_RID = aHdrRID;
               
                VSW_UnitStorage newUnitStorage = new VSW_UnitStorage();
                newUnitStorage.HN_RID = aHnRID;
                newUnitStorage.STORE_RID = aStoreRID;
                newUnitStorage.REVERSE_ON_HAND_UNITS = 0;
                headerStorage.VSW_UnitList.Add(newUnitStorage);
              
                this.VSW_HeaderList.Add(headerStorage);
                _isHeaderCached.Add(aHdrRID, true);
                
            }
            VSW_UnitStorage unitStorage = headerStorage.UnitStorageFindInList(aStoreRID, aHnRID);
            if (unitStorage == null)
            {
                //throw new NoNullAllowedException("No unit storage found for: HDR_RID=" + aHdrRID + " AND HN_RID=" + aHnRID + " AND ST_RID=" + aStoreRID);

                // if no entry is found for this store - create one and add to memory
                // default units to zero
                unitStorage = new VSW_UnitStorage();
                unitStorage.HN_RID = aHnRID;
                unitStorage.STORE_RID = aStoreRID;
                unitStorage.REVERSE_ON_HAND_UNITS = 0;
                headerStorage.VSW_UnitList.Add(unitStorage);
            }
            return (double)unitStorage.REVERSE_ON_HAND_UNITS;

        }

        //write variables values in memory
        /// <summary>
        /// Sets the store VSW Reverse Onhand variable value for a given header, Hierarchy Node 
        /// </summary>
        /// <param name="aVariableIDX">Variable Index (position of the variable within the container)</param>
        /// <param name="aVariableID">Variable ID</param>
        /// <param name="aHdrRID">Header RID</param>
        /// <param name="aHnRID">Hierarchy RID</param>
        /// <param name="aStoreRIDs">Store RID Array</param>
        /// <param name="aVariableValues">Variable Values Array corresponding to the Store RID Array</param>
        public void SetStoreVswReverseOnhandValue
            (
           VswReverseOnhandVariables aVariableCol,
                int aHdrRID,
                int aHnRID,
                int[] aStoreRIDs,
                double[] aVariableValues)
        {
            if (aStoreRIDs.Length != aVariableValues.Length)
            {
                throw new Exception("Store array length = " + aStoreRIDs.Length + " is not same as Variable array length = " + aVariableValues.Length);
            }
            // Begin TT#3334 - JSmith - VSW On Hand units "sticking"
            if (IsHeaderCached(aHdrRID) == false)
            {
                ReadStoreVswReverseOnhand(aHdrRID);  //Reads all entries for the HdrRID from the database
            }
            VSW_HeaderStorage hs = HeaderStorageFindInList(aHdrRID);
            if (hs == null)
            {
                // if no entry is found in the database - create one and add it to memory
                hs = new VSW_HeaderStorage();
                hs.HDR_RID = aHdrRID;

                this.VSW_HeaderList.Add(hs);
                _isHeaderCached.Add(aHdrRID, true);

            }
            // End TT#3334 - JSmith - VSW On Hand units "sticking" 

            VSW_HeaderStorage headerStorage = HeaderStorageFindInList(aHdrRID);
            if (headerStorage != null)
            {


                //int storeRID;
                Int64 variableValue;
                //StoreVariableVectorContainer svvc = GetStoreVswReverseOnhandVariableVectorContainer(aHdrRID, aHnRID);
                //eMIDVariableModelType variableModelType = (eMIDVariableModelType)GetVswReverseOnhandDatabaseBin().VariableModel.VariableModelID; 
                //double multiplier = Math.Pow(10, GetVswReverseOnhandDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision);  


                double multiplier = Math.Pow(10, VswReverseOnhandVariables.VswReverseOnhandUnits.DecimalPrecision);

                for (int i = 0; i < aStoreRIDs.Length; i++)
                {

                 

                    //storeRID = aStoreRIDs[i];
                    if (aVariableValues[i] < 0)
                    {
                        variableValue =
                            (Int64)(aVariableValues[i]
                                    * multiplier
                                    - .5);   // rounding should use same sign as original value
                    }
                    else
                    {
                        variableValue =
                            (Int64)(aVariableValues[i]
                                    * multiplier
                                    + .5);
                    }

                    headerStorage.UnitStorageUpdate(aStoreRIDs[i], aHnRID, variableValue);
                    //    svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
                    //    svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
                }
            }
            return;
        }

       

        /// <summary>
        /// Deletes StoreVswReverseOnhand from database for given keys
        /// </summary>
        /// <param name="aHeaderDataRecord">Database Access</param>
        /// <param name="aHdrKeysToDelete">Header whose VSW Reverse Onhands is to be deleted</param>
        public void DeleteStoreVswReverseOnhand(Header aHeaderDataRecord, int[] aHdrKeysToDelete)
        {
            Flush(aHdrKeysToDelete);
            //string sHeaderRidsCommaDelimited = string.Empty;
            //for (int i = 0; i < aHdrKeysToDelete.Length; i++)
            //{
            //    if (i == 0)
            //    {
            //        sHeaderRidsCommaDelimited += aHdrKeysToDelete[i];
            //    }
            //    else
            //    {
            //        sHeaderRidsCommaDelimited += "," + aHdrKeysToDelete[i];
            //    }

            ////    VswReverseOnhandDatabaseBinKey vroKey = new VswReverseOnhandDatabaseBinKey(aHdrKeysToDelete[i]);
            //    //GetVswReverseOnhandDatabaseBin().DeleteStoreVariables(
            //    //    aHeaderDataRecord._dba,
            //    //    vroKey);
            //}
            DataTable dtHeaderList = new DataTable();
            dtHeaderList.Columns.Add("HDR_RID", typeof(int));
            for (int i = 0; i < aHdrKeysToDelete.Length; i++)
            {
                //ensure styleHNRids are distinct, and only added to the datatable one time
                if (dtHeaderList.Select("HDR_RID=" + aHdrKeysToDelete[i].ToString()).Length == 0)
                {
                    DataRow dr = dtHeaderList.NewRow();
                    dr["HDR_RID"] = aHdrKeysToDelete[i];
                    dtHeaderList.Rows.Add(dr);
                }
            }


            lock (_deleteVSWLock)
            {
                DatabaseAccess _dba = new DatabaseAccess(MIDConnectionString.ConnectionString);
                //Begin TT#1274-MD -jsobek -Cancel Allocation action failed in Workspace
                try
                {
                    _dba.OpenUpdateConnection();
                    StoredProcedures.MID_VSW_REVERSE_ON_HAND_DELETE_FROM_HEADER.Delete(_dba, HEADER_LIST: dtHeaderList);

                    _dba.CommitData();
                }
                finally
                {
                    _dba.CloseUpdateConnection();
                }
                //End TT#1274-MD -jsobek -Cancel Allocation action failed in Workspace
            }
        }

        /// <summary>
        /// Flushes the containers associated with the given header RIDs from memory
        /// </summary>
        /// <param name="aHnRIDs">Array of the header RIDs to flush</param>
        public void Flush(int[] aHdrRIDs)
        {
            //List<VswReverseOnhandDatabaseBinKey> vroKeys = new List<VswReverseOnhandDatabaseBinKey>();
            for (int i = 0; i < aHdrRIDs.Length; i++)
            {
            //    foreach (VswReverseOnhandDatabaseBinKey vrobk in _vswReverseOnhandContainer.Keys)
            //    {
            //        if (vrobk.HdrRID == aHdrRIDs[i])
            //        {
            //            vroKeys.Add(vrobk);
            //        }
            //    }
                _isHeaderCached.Remove(aHdrRIDs[i]);

                HeaderStorageRemoveFromList(aHdrRIDs[i]);
               
               

            }
            //foreach (VswReverseOnhandDatabaseBinKey vrobk in vroKeys)
            //{
            //    _vswReverseOnhandContainer.Remove(vrobk);
            //}
            //_lastVswReverseOnhandDatabaseBinKey = null;
            //_lastStoreVswReverseOnhandVectorContainer = null;
        }

        public class VSW_UnitStorage
        {
            public int HN_RID;
            public int STORE_RID;
            public int REVERSE_ON_HAND_UNITS;
        }
     
        public class VSW_HeaderStorage
        {
            public int HDR_RID;
            public List<VSW_UnitStorage> VSW_UnitList = new List<VSW_UnitStorage>();

            public VSW_UnitStorage UnitStorageFindInList(int aStoreRID, int aHN_RID)
            {
                VSW_UnitStorage unitStorage = this.VSW_UnitList.Find(
                             delegate(VSW_UnitStorage o)
                             {
                                 return (o.STORE_RID == aStoreRID && o.HN_RID == aHN_RID);
                             }
                             );
                return unitStorage;
            }
            public void UnitStorageUpdate(int aStoreRID, int aHN_RID, double newVal)
            {
                VSW_UnitStorage unitStorage = UnitStorageFindInList(aStoreRID, aHN_RID);
                if (unitStorage != null)
                {
                    unitStorage.REVERSE_ON_HAND_UNITS = (int)newVal;
                }
                // Begin TT#3334 - JSmith - VSW On Hand units "sticking"
                else if (newVal > 0)
                {
                    unitStorage = new VSW_UnitStorage();
                    unitStorage.HN_RID = aHN_RID;
                    unitStorage.STORE_RID = aStoreRID;
                    unitStorage.REVERSE_ON_HAND_UNITS = (int)newVal;
                    VSW_UnitList.Add(unitStorage);
                }
                // End TT#3334 - JSmith - VSW On Hand units "sticking"
            }
        }
        public List<VSW_HeaderStorage> VSW_HeaderList = new List<VSW_HeaderStorage>();
        public VSW_HeaderStorage HeaderStorageFindInList(int aHdrRID)
        {
            VSW_HeaderStorage headerStorage = this.VSW_HeaderList.Find(
                         delegate(VSW_HeaderStorage o)
                         {
                             return o.HDR_RID == aHdrRID;
                         }
                         );
            return headerStorage;
        }
        public void HeaderStorageRemoveFromList(int aHdrRID)
        {
            VSW_HeaderStorage headerStorage = HeaderStorageFindInList(aHdrRID);
            if (headerStorage != null)
            {
                headerStorage.VSW_UnitList = null;
                this.VSW_HeaderList.Remove(headerStorage);
            }
        }

        

        private static object _readVSWLock = new object();
        private static object _writeVSWLock = new object();
        private static object _deleteVSWLock = new object();
        /// <summary>
        /// Get VSW Reverse Onhand for the given header RID
        /// </summary>
        /// <param name="aHdrRID">Header RID</param>
        public void ReadStoreVswReverseOnhand(int aHdrRID)
        {



            //only the header RID is used as an identifyer for the VSW reverse on hand...HN RID is set to Include.NoRID which is a negative one
            lock (_readVSWLock)        
            {
                Header hd = new Header();
              
                DataTable dt = hd.GetVSWStorage(aHdrRID);
                if (dt.Rows.Count > 0)
                {

                    VSW_HeaderStorage headerStorage = new VSW_HeaderStorage();
                    headerStorage.HDR_RID = aHdrRID;
                    foreach (DataRow dr in dt.Rows)
                    {
                        VSW_UnitStorage unitStorage = new VSW_UnitStorage();
                        unitStorage.HN_RID = int.Parse(dr["HN_RID"].ToString());
                        unitStorage.STORE_RID = int.Parse(dr["ST_RID"].ToString());
                        unitStorage.REVERSE_ON_HAND_UNITS = int.Parse(dr["VSW_REVERSE_ON_HAND_UNITS"].ToString());
                        headerStorage.VSW_UnitList.Add(unitStorage);
                    }
                    VSW_HeaderStorage alreadyExists = HeaderStorageFindInList(aHdrRID);
                    if (alreadyExists != null)
                    {
                        VSW_HeaderList.Remove(alreadyExists);
                    }
                    this.VSW_HeaderList.Add(headerStorage);

                  
                    _isHeaderCached.Add(aHdrRID, true);
                    
                }
               
            }

       


            //VswReverseOnhandDatabaseBinKey vrodbk = new VswReverseOnhandDatabaseBinKey(aHdrRID);  // Key for ALL HnRIDs associated with this header
            //List<StoreVariableData<VswReverseOnhandDatabaseBinKey>> vswReverseOnhandList = new List<StoreVariableData<VswReverseOnhandDatabaseBinKey>>();

            //try
            //{
            //    Header header = new Header();
            //    vswReverseOnhandList =
            //        GetVswReverseOnhandDatabaseBin().ReadStoreVariables(header._dba, vrodbk);

            //    if (vswReverseOnhandList.Count > 0)
            //    {
            //        foreach (StoreVariableData<VswReverseOnhandDatabaseBinKey> svd in vswReverseOnhandList)
            //        {
            //            _vswReverseOnhandContainer.Add(svd.DatabaseBinKey, svd);
            //        }
            //    }
            //    try
            //    {
            //        _isHeaderCached.Add(aHdrRID, true);
            //    }
            //    catch (ArgumentException)
            //    {
            //        //  Key already exists!
            //    }
            //}
            //catch
            //{
            //    _isHeaderCached.Remove(aHdrRID);
            //    _vswReverseOnhandContainer.Remove(vrodbk);
            //    throw;
            //}
            //finally
            //{
            //}
        }

       
        /// <summary>
        /// Writes the store VSW Reverse Onhand for each header represented in the container
        /// </summary>
        /// <param name="aHeaderDataRecord">Header data record containing an open connection</param>
        /// <param name="aHdrKeysToWrite">Array of header keys to write</param>
        public void WriteStoreVswReverseOnhand(Header aHeaderDataRecord, int[] aHdrKeysToWrite)
        {
            //List<VswReverseOnhandDatabaseBinKey> keysToWrite =
            //    new List<VswReverseOnhandDatabaseBinKey>();
            for (int i = 0; i < aHdrKeysToWrite.Length; i++)
            {
                // Begin TT#3334 - JSmith - VSW On Hand units "sticking"
                //WriteStoreVswReverseOnhand(aHdrKeysToWrite[i]);
                WriteStoreVswReverseOnhand(aHeaderDataRecord, aHdrKeysToWrite[i]);
                // End TT#3334 - JSmith - VSW On Hand units "sticking"
                //    foreach (VswReverseOnhandDatabaseBinKey vroKey in _vswReverseOnhandContainer.Keys)
                //    {
                //        if (vroKey.HdrRID == aHdrKeysToWrite[i])
                //        {
                //            keysToWrite.Add(vroKey);
                //        }
                //    }
            }
            //VswReverseOnhandDatabaseBinKey[] vroKeysToWrite = new VswReverseOnhandDatabaseBinKey[keysToWrite.Count];
            //keysToWrite.CopyTo(vroKeysToWrite, 0);
            //GetVswReverseOnhandDatabaseBin().WriteStoreVariables(
            //                aHeaderDataRecord._dba,
            //                _vswReverseOnhandContainer,
            //                vroKeysToWrite);
        }
        // Begin TT#3334 - JSmith - VSW On Hand units "sticking"
        //public void WriteStoreVswReverseOnhand(int aHdrRID)
        public void WriteStoreVswReverseOnhand(Header aHeaderDataRecord, int aHdrRID)
        // End TT#3334 - JSmith - VSW On Hand units "sticking"
        {
            VSW_HeaderStorage headerStorage = HeaderStorageFindInList(aHdrRID);

            if (headerStorage != null)
            {
                lock (_writeVSWLock)
                {
                    //DatabaseAccess _dba = new DatabaseAccess(MIDConfigurationManager.AppSettings["ConnectionString"]);
                    // Begin TT#3334 - JSmith - VSW On Hand units "sticking"
                    //DatabaseAccess _dba = new DatabaseAccess(MIDConnectionString.ConnectionString);
                    // End TT#3334 - JSmith - VSW On Hand units "sticking"
                    Header hd = new Header();

                    foreach (VSW_UnitStorage unitStorage in headerStorage.VSW_UnitList)
                    {

                        //do not write zero values to the database
                        if (unitStorage.REVERSE_ON_HAND_UNITS != 0)
                        {
                            //MIDDbParameter[] InParams = { new MIDDbParameter("@HDR_RID", headerStorage.HDR_RID, eDbType.Int, eParameterDirection.Input),
                            //                  new MIDDbParameter("@HN_RID", unitStorage.HN_RID, eDbType.Int, eParameterDirection.Input),
                            //                  new MIDDbParameter("@ST_RID", unitStorage.STORE_RID, eDbType.Int, eParameterDirection.Input),
                            //                  new MIDDbParameter("@VSW_REVERSE_ON_HAND_UNITS", unitStorage.REVERSE_ON_HAND_UNITS, eDbType.Int, eParameterDirection.Input)};
                            //// Begin TT#3334 - JSmith - VSW On Hand units "sticking"
                            ////_dba.ExecuteStoredProcedure("MID_VSW_REVERSE_ON_HAND_UPSERT", InParams);
                            //aHeaderDataRecord.DBA.ExecuteStoredProcedure("MID_VSW_REVERSE_ON_HAND_UPSERT", InParams);

                            hd.VSW_Upsert(aHeaderDataRecord.DBA, headerStorage.HDR_RID, unitStorage.HN_RID, unitStorage.STORE_RID, unitStorage.REVERSE_ON_HAND_UNITS);
                           
                            // End TT#3334 - JSmith - VSW On Hand units "sticking"
                        }
                    }
                }
            }
        }

    }

    /// <summary>
    /// This class provides a way to use "emum as strings" typesafe parameter
    /// </summary>
    public sealed class VswReverseOnhandVariables
    {
        public static readonly VswReverseOnhandVariables VswReverseOnhandUnits = new VswReverseOnhandVariables("VSW_REVERSE_ON_HAND_UNITS", 0,0, 805126);
        //public static readonly vswReverseOnhandVariables Name2 = new vswReverseOnhandVariables("Name2");

        private VswReverseOnhandVariables(string DatabaseName, int DecimalPrecision, int Index, int ID)
        {
            this.DatabaseName = DatabaseName;
            this.DecimalPrecision = DecimalPrecision;
            this.Index = Index;
            this.ID = ID;
            this._DisplayName = String.Empty;
        }



        public string DatabaseName { get; private set; }
        public int DecimalPrecision { get; private set; }
        public int Index { get; private set; }
        public int ID { get; private set; }

        private string _DisplayName;
        public string DisplayName
        {
            get
            {
                if (_DisplayName == String.Empty)
                {
                    _DisplayName = MIDText.GetTextOnly(this.ID);
                }
                return _DisplayName;
            }
            private set
            {
                _DisplayName = value;
            }
        }

        public static implicit operator string(VswReverseOnhandVariables op) { return op.DatabaseName; }
       
    }

    /// <summary>
    /// This class provides a way to use "emum as strings" typesafe parameter
    /// </summary>
    //public sealed class AllocationTotalBinVariables
    //{
    //    private static List<AllocationTotalBinVariables> _variableList = new List<AllocationTotalBinVariables>();

    //    public static readonly AllocationTotalBinVariables DetailAuditFlags = new AllocationTotalBinVariables("DETAIL_AUDIT_FLAGS", DecimalPrecision: 0, RollType: 0, Index: 0, ID: 805101);
    //    public static readonly AllocationTotalBinVariables Minimum = new AllocationTotalBinVariables("MINIMUM", DecimalPrecision: 0, RollType: 1, Index: 1, ID: 805102);
    //    public static readonly AllocationTotalBinVariables Maximum = new AllocationTotalBinVariables("MAXIMUM", DecimalPrecision: 0, RollType: 1, Index: 2, ID: 805103);
    //    public static readonly AllocationTotalBinVariables PrimaryMaximum = new AllocationTotalBinVariables("PRIMARY_MAXIMUM", DecimalPrecision: 0, RollType: 1, Index: 3, ID: 805104);
    //    public static readonly AllocationTotalBinVariables UnitsAllocated = new AllocationTotalBinVariables("UNITS_ALLOCATED", DecimalPrecision: 0, RollType: 1, Index: 4, ID: 805105);
    //    public static readonly AllocationTotalBinVariables UnitsAllocatedByAuto = new AllocationTotalBinVariables("UNITS_ALLOCATED_BY_AUTO", DecimalPrecision: 0, RollType: 1, Index: 6, ID: 805107);
    //    public static readonly AllocationTotalBinVariables UnitsAllocatedByRule = new AllocationTotalBinVariables("UNITS_ALLOCATED_BY_RULE", DecimalPrecision: 0, RollType: 1, Index: 8, ID: 805109);
    //    public static readonly AllocationTotalBinVariables ShipStatusFlags = new AllocationTotalBinVariables("SHIP_STATUS_FLAGS", DecimalPrecision: 0, RollType: 0, Index: 10, ID: 805111);
    //    public static readonly AllocationTotalBinVariables UnitsShipped = new AllocationTotalBinVariables("UNITS_SHIPPED", DecimalPrecision: 0, RollType: 0, Index: 11, ID: 805112);
    //    public static readonly AllocationTotalBinVariables ChosenRuleType = new AllocationTotalBinVariables("CHOSEN_RULE_TYPE", DecimalPrecision: 0, RollType: 0, Index: 13, ID: 805114);
    //    public static readonly AllocationTotalBinVariables ChosenRuleLayer = new AllocationTotalBinVariables("CHOSEN_RULE_LAYER", DecimalPrecision: 0, RollType: 0, Index: 14, ID: 805115);
    //    public static readonly AllocationTotalBinVariables ChosenRuleUnits = new AllocationTotalBinVariables("CHOSEN_RULE_UNITS", DecimalPrecision: 0, RollType: 0, Index: 15, ID: 805116);
    //    public static readonly AllocationTotalBinVariables NeedDay = new AllocationTotalBinVariables("NEED_DAY", DecimalPrecision: 0, RollType: 0, Index: 17, ID: 805118);
    //    public static readonly AllocationTotalBinVariables UnitNeedBefore = new AllocationTotalBinVariables("UNIT_NEED_BEFORE", DecimalPrecision: 0, RollType: 0, Index: 18, ID: 805119);
    //    public static readonly AllocationTotalBinVariables UnitPlanBefore = new AllocationTotalBinVariables("UNIT_PLAN_BEFORE", DecimalPrecision: 0, RollType: 0, Index: 19, ID: 805120);
    //    public static readonly AllocationTotalBinVariables GeneralAuditFlags = new AllocationTotalBinVariables("GENERAL_AUDIT_FLAGS", DecimalPrecision: 0, RollType: 0, Index: 20, ID: 805121);
    //    public static readonly AllocationTotalBinVariables GradeIndex = new AllocationTotalBinVariables("GRADE_INDEX", DecimalPrecision: 0, RollType: 0, Index: 21, ID: 805122);
    //    public static readonly AllocationTotalBinVariables ShipToDay = new AllocationTotalBinVariables("SHIP_TO_DAY", DecimalPrecision: 0, RollType: 0, Index: 22, ID: 805123);
    //    public static readonly AllocationTotalBinVariables CapacityUnits = new AllocationTotalBinVariables("CAPACITY_UNITS", DecimalPrecision: 0, RollType: 0, Index: 23, ID: 805124);
    //    public static readonly AllocationTotalBinVariables CapacityExceedByPercent = new AllocationTotalBinVariables("CAPACITY_EXCEED_BY_PERCENT", DecimalPrecision: 0, RollType: 0, Index: 24, ID: 805125);


    //    private AllocationTotalBinVariables(string DatabaseName, int DecimalPrecision, int RollType, int Index, int ID)
    //    {
    //        this.DatabaseName = DatabaseName;
    //        this.DecimalPrecision = DecimalPrecision;
    //        this.RollType = RollType;
    //        this.Index = Index;
    //        this.ID = ID;
    //        this._DisplayName = String.Empty;
    //        _variableList.Add(this);
    //    }

    //    public static AllocationTotalBinVariables Find(string DatabaseName)
    //    {
    //        AllocationTotalBinVariables result = _variableList.Find(
    //             delegate(AllocationTotalBinVariables o)
    //             {
    //                 return o.DatabaseName == DatabaseName;
    //             }
    //             );
    //        return result;
    //    }
   

    //    public string DatabaseName { get; private set; }
    //    public int DecimalPrecision { get; private set; }
    //    public int RollType { get; private set; }
    //    public int Index { get; private set; }
    //    public int ID { get; private set; }

    //    private string _DisplayName;
    //    public string DisplayName
    //    {
    //        get
    //        {
    //            if (_DisplayName == String.Empty)
    //            {
    //                _DisplayName = MIDText.GetTextOnly(this.ID);
    //            }
    //            return _DisplayName;
    //        }
    //        private set
    //        {
    //            _DisplayName = value;
    //        }
    //    }

    //    public static implicit operator string(AllocationTotalBinVariables op) { return op.DatabaseName; }

    //}

    ///// <summary>
    ///// This class provides a way to use "emum as strings" typesafe parameter
    ///// </summary>
    //public sealed class StoreHistoryVariables
    //{
    //    private static List<StoreHistoryVariables> _variableList = new List<StoreHistoryVariables>();

    //    public static readonly StoreHistoryVariables SalesTotal = new StoreHistoryVariables("SALES", DecimalPrecision: 0, RollType: 1, Index: 0, ID: 806101);
    //    public static readonly StoreHistoryVariables SalesRegular = new StoreHistoryVariables("SALES_REG", DecimalPrecision: 0, RollType: 1, Index: 1, ID: 806102);
    //    public static readonly StoreHistoryVariables SalesPromo = new StoreHistoryVariables("SALES_PROMO", DecimalPrecision: 0, RollType: 1, Index: 2, ID: 806103);
    //    public static readonly StoreHistoryVariables SalesMarkdown = new StoreHistoryVariables("SALES_MKDN", DecimalPrecision: 0, RollType: 1, Index: 3, ID: 806104);
    //    public static readonly StoreHistoryVariables StockTotal = new StoreHistoryVariables("STOCK", DecimalPrecision: 0, RollType: 1, Index: 4, ID: 806105);
    //    public static readonly StoreHistoryVariables StockRegular = new StoreHistoryVariables("STOCK_REG", DecimalPrecision: 0, RollType: 1, Index: 5, ID: 806106);
    //    public static readonly StoreHistoryVariables StockMarkdown = new StoreHistoryVariables("STOCK_MKDN", DecimalPrecision: 0, RollType: 1, Index: 6, ID: 806107);
    //    public static readonly StoreHistoryVariables InStockSales = new StoreHistoryVariables("IN_STOCK_SALES", DecimalPrecision: 0, RollType: 1, Index: 7, ID: 806108);
    //    public static readonly StoreHistoryVariables InStockSalesReg = new StoreHistoryVariables("IN_STOCK_SALES_REG", DecimalPrecision: 0, RollType: 1, Index: 8, ID: 806109);
    //    public static readonly StoreHistoryVariables InStockSalesPromo = new StoreHistoryVariables("IN_STOCK_SALES_PROMO", DecimalPrecision: 0, RollType: 1, Index: 9, ID: 806110);
    //    public static readonly StoreHistoryVariables InStockSalesMkdn = new StoreHistoryVariables("IN_STOCK_SALES_MKDN", DecimalPrecision: 0, RollType: 1, Index: 10, ID: 806111);
    //    public static readonly StoreHistoryVariables AccumSellThruSales = new StoreHistoryVariables("ACCUM_SELL_THRU_SALES", DecimalPrecision: 0, RollType: 1, Index: 11, ID: 806112);
    //    public static readonly StoreHistoryVariables AccumSellThruStock = new StoreHistoryVariables("ACCUM_SELL_THRU_STOCK", DecimalPrecision: 0, RollType: 1, Index: 12, ID: 806113);
    //    public static readonly StoreHistoryVariables DaysInStock = new StoreHistoryVariables("DAYS_IN_STOCK", DecimalPrecision: 0, RollType: 1, Index: 13, ID: 806114);
    //    public static readonly StoreHistoryVariables ReceivedStock = new StoreHistoryVariables("RECEIVED_STOCK", DecimalPrecision: 0, RollType: 1, Index: 14, ID: 806115);



    //    private StoreHistoryVariables(string DatabaseName, int DecimalPrecision, int RollType, int Index, int ID)
    //    {
    //        this.DatabaseName = DatabaseName;
    //        this.DecimalPrecision = DecimalPrecision;
    //        this.RollType = RollType;
    //        this.Index = Index;
    //        this.ID = ID;
    //        this._DisplayName = String.Empty;
    //        _variableList.Add(this);
    //    }

    //    public static StoreHistoryVariables Find(string DatabaseName)
    //    {
    //        StoreHistoryVariables result = _variableList.Find(
    //             delegate(StoreHistoryVariables o)
    //             {
    //                 return o.DatabaseName == DatabaseName;
    //             }
    //             );
    //        return result;
    //    }

    //    public string DatabaseName { get; private set; }
    //    public int DecimalPrecision { get; private set; }
    //    public int RollType { get; private set; }
    //    public int Index { get; private set; }
    //    public int ID { get; private set; }

    //    private string _DisplayName;
    //    public string DisplayName
    //    {
    //        get
    //        {
    //            if (_DisplayName == String.Empty)
    //            {
    //                _DisplayName = MIDText.GetTextOnly(this.ID);
    //            }
    //            return _DisplayName;
    //        }
    //        private set
    //        {
    //            _DisplayName = value;
    //        }
    //    }

    //    public static implicit operator string(StoreHistoryVariables op) { return op.DatabaseName; }

    //}
}
