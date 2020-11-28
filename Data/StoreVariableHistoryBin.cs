//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Globalization;
//using System.Diagnostics;
//using MIDRetail.DataCommon;

//namespace MIDRetail.Data
//{
//    /// <summary>
//    /// Container for Store History Variables.
//    /// </summary>
//    [Serializable]
//    public class StoreVariableHistoryBin : DataLayer, IDisposable
//    {
        
//        #region Fields
//        private bool _isDisposed;
//        private int _clientThreadID;
//        private bool _updateAccess;
//        private MIDEnqueue _midEnqueue;
//        // begin TT#467 Enq Conflict Except During Size Hist Load
//        //private int _lastLockHnRID;
//        //private Dictionary<int, int> _lockDictionary; 
//        // begin TT#707 - JEllis - Container not thread safe (part 2)
//        private HistoryDayDatabaseBin _historyDayDatabaseBin;
//        private HistoryWeekDatabaseBin _historyWeekDatabaseBin;
//        // end TT#707 - JEllis - Container not thread safe (part 2)
//        private Dictionary<HistoryDatabaseBinKey, int[]> _lockDayDictionary;
//        private Dictionary<HistoryDatabaseBinKey, int[]> _lockWeekDictionary;
//        // end TT#467 Enq Conflict Except During Size Hist Load
//        private StoreVariableDataDictionary<HistoryDatabaseBinKey> _storeVariableDayContainer;
//        private StoreVariableDataDictionary<HistoryDatabaseBinKey> _storeVariableWeekContainer;
//        private List<MIDEnqueueConflict> _lockConflictList;
//        private eSQLTimeIdType _lastTimeIDType;
//        // begin TT#555 Total variable is aggregate of other variables
//        //private HistoryDatabaseBinKey _lastDatabaseBinKey;
//        //private StoreVariableVectorContainer _lastStoreVariableVectorContainer;
//        private StoreVariableData<HistoryDatabaseBinKey> _lastStoreVariableData;
//        // end TT#555 Total variable is aggregate of other variables
//        //Begin TT#467 - JSmith - EnqueueConflictException during size history load
//        private int _maximumRetryAttempts = 4;
//        private int _retrySleepTime = 2000;
//        //End TT#467

//        #endregion Fields

//        #region Contructors
//        /// <summary>
//        /// Creates a READ-ONLY instance of this class
//        /// </summary>
//        public StoreVariableHistoryBin()
//        {
//            Initialize(false, 0);
//        }
//        /// <summary>
//        /// Creates a WRITE or READ instance of this class
//        /// </summary>
//        /// <param name="aUpdateAccess">True: allows write access to Store Style Color Size Variable Data; False: allows READ-ONLY access</param>
//        /// <param name="aClientThreadID">Thread ID where the database access is occurring</param>
//        public StoreVariableHistoryBin(bool aUpdateAccess, int aClientThreadID)
//        {
//            Initialize(aUpdateAccess, aClientThreadID);
//        }

//        // Begin TT#634 - JSmith - Color rename
//        public StoreVariableHistoryBin(bool aUpdateAccess, int aClientThreadID, TransactionData td)
//            : base(td.DBA)
//        {
//            Initialize(aUpdateAccess, aClientThreadID);
//        }
//        // End TT#634

//        /// <summary>
//        /// Initializes this container
//        /// </summary>
//        /// <param name="aUpdateAccess">TRUE: Container is prepared to update database; FALSE: Container will be READ only</param>
//        /// <param name="aClientThreadID">Identifies the Thread ID where this container exists</param>
//        private void Initialize(bool aUpdateAccess, int aClientThreadID)
//        {
//            _isDisposed = false;
//            _clientThreadID = aClientThreadID;
//            _updateAccess = aUpdateAccess;
//            if (_updateAccess)
//            {
//                _midEnqueue = new MIDEnqueue();
//                _lockConflictList = new List<MIDEnqueueConflict>();
//                //_lockDictionary = new Dictionary<int, int>(); // TT#467 Enq Conflict Except during Size Hist Load
//                //_lastLockHnRID = Include.NoRID;  // TT#467 Enq Conflict Except during Size Hist Load
//                _lockDayDictionary = new Dictionary<HistoryDatabaseBinKey,int[]>(); // TT#467 Enq Conflict Except during Size Hist Load
//                _lockWeekDictionary = new Dictionary<HistoryDatabaseBinKey,int[]>(); // TT#467 Enq Conflict Except during Size Hist Load
//            }
//            _storeVariableDayContainer = new StoreVariableDataDictionary<HistoryDatabaseBinKey>();
//            _storeVariableWeekContainer = new StoreVariableDataDictionary<HistoryDatabaseBinKey>();
//            _lastTimeIDType = eSQLTimeIdType.TimeIdIsDaily;
//            // begin TT#555 Total variable is aggregate of other variables
//            //_lastDatabaseBinKey = null;
//            //_lastStoreVariableVectorContainer = null;
//            _lastStoreVariableData = null;
//            // end TT#555 Total variable is aggregate of other variables

//            //Begin TT#467 - JSmith - EnqueueConflictException during size history load
//            string sParm = MIDConfigurationManager.AppSettings["EnqueueRetryCount"];
//            if (sParm != null)
//            {
//                try
//                {
//                    _maximumRetryAttempts = Convert.ToInt32(sParm, CultureInfo.CurrentUICulture);
//                }
//                catch
//                {
//                }
//            }

//            sParm = MIDConfigurationManager.AppSettings["EnqueueRetryInterval"];
//            if (sParm != null)
//            {
//                try
//                {
//                    _retrySleepTime = Convert.ToInt32(sParm, CultureInfo.CurrentUICulture);
//                }
//                catch
//                {
//                }
//            }
//            //End TT#467
//        }
//        #endregion Contructors

//        #region Properties
//        /// <summary>
//        /// Gets a list of the Style Hn RIDs that could not be locked due to another user/thread already having it locked.
//        /// </summary>
//        public List<MIDEnqueueConflict> LockConflictList
//        {
//            get { return _lockConflictList; }
//        }
//        #endregion Properties

//        #region Methods

//        #region Lock
//        // begin TT#467 Enq Conflict Except During Size Hist Load
//        /// <summary>
//        /// Locks Time, Style, Size, Color as in-use by the specified User.
//        /// </summary>
//        /// <param name="aUserRID">RID that identifies the user</param>
//        /// <param name="aStyleHnRIDs">Array of Style Hierarchy RIDs to be locked</param>
//        /// <returns>True when successful; False otherwise</returns>
//        public bool LockTimeHnRIDNode(int aUserRID, SQL_TimeID[] aSQL_TimeIDs, int[] aHnRIDs, int[] aStyleHnRIDs, int[] aColorCodeRIDs, int[] aSizeCodeRIDs)
//        {
//            int retryCount = 0;
//            bool retry = true;

//            while (retry)
//            {
//                try
//                {
//                    if (LockTimeHnRIDNode2(aUserRID, aSQL_TimeIDs, aHnRIDs, aStyleHnRIDs, aColorCodeRIDs, aSizeCodeRIDs))
//                    {
//                        retry = false;
//                        return true;
//                    }
//                }
//                catch (EnqueueConflictException)
//                {
//                    ++retryCount;
//                    if (retryCount < _maximumRetryAttempts)
//                    {
//                        System.Threading.Thread.Sleep(_retrySleepTime);
//                    }
//                    else
//                    {
//                        retry = false;
//                        throw;
//                    }
//                }
//                catch
//                {
//                    retry = false;
//                    throw;
//                }
//            }

//            return false;
//        }

//        private bool LockTimeHnRIDNode2(int aUserRID, SQL_TimeID[] aSQL_TimeIDs, int[] aHnRIDs, int[] aStyleHnRIDs, int[] aColorCodeRIDs, int[] aSizeCodeRIDs)
//        {
//            bool returnCode = false;
//            if (_updateAccess)
//            {
//                if (aSQL_TimeIDs.Length != aHnRIDs.Length
//                    || aSQL_TimeIDs.Length != aStyleHnRIDs.Length
//                    || aSQL_TimeIDs.Length != aColorCodeRIDs.Length
//                    || aSQL_TimeIDs.Length != aSizeCodeRIDs.Length)
//                {
//                    throw new ArgumentException("LockTimeHnRIDNode parameter arrays must contain same number of entries");
//                }
//                DataTable lockTable;
//                try
//                {
//                    _lockConflictList.Clear();
//                    // begin TT#467 Enq Conflict During Size Hist Load
//                    //lockTable = Lock_Read(aStyleHnRIDs);  
//                    //foreach (DataRow dataRow in lockTable.Rows)
//                    //{
//                    //    _lockConflictList.Add(
//                    //        new MIDEnqueueConflict(
//                    //            Convert.ToInt32(dataRow["ENQUEUE_TYPE"], CultureInfo.CurrentCulture),
//                    //            Convert.ToInt32(dataRow["RID"], CultureInfo.CurrentUICulture),
//                    //            Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture),
//                    //            Convert.ToInt32(dataRow["OWNING_THREADID"], CultureInfo.CurrentUICulture)));
//                    //}
//                    string[] keyLabel = new string[5];
//                    keyLabel[1] = " Size Hierarchy Node RID ";
//                    keyLabel[2] = " in Style Hierarchy Node RID ";
//                    keyLabel[3] = " Color Code RID ";
//                    keyLabel[4] = " Size Code RID ";
//                    int[] keys = new int[5];
//                    for (int i = 0; i < aSQL_TimeIDs.Length; i++)
//                    {
//                        lockTable = _midEnqueue.StoreVariableHistory_EnqueueRead(aSQL_TimeIDs[i], aHnRIDs[i]);
//                        foreach (DataRow dataRow in lockTable.Rows)
//                        {
//                            keys[0] = aSQL_TimeIDs[i].SqlTimeID;
//                            keys[1] = aHnRIDs[i];
//                            keys[2] = aStyleHnRIDs[i];
//                            keys[3] = aColorCodeRIDs[i];
//                            keys[4] = aSizeCodeRIDs[i];
//                            if (aSQL_TimeIDs[i].TimeIdType == eSQLTimeIdType.TimeIdIsDaily)
//                            {
//                                keyLabel[0] = " Daily Time ID ";
//                            }
//                            else
//                            {
//                                keyLabel[0] = " Weekly Time ID ";
//                            }
//                            _lockConflictList.Add(
//                                new MIDEnqueueConflict(
//                                    Convert.ToInt32(dataRow["ENQUEUE_TYPE"], CultureInfo.CurrentUICulture),
//                                    keyLabel,
//                                    keys,
//                                    Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture),
//                                    Convert.ToInt32(dataRow["OWNING_THREADID"], CultureInfo.CurrentUICulture)));
//                        }
//                    }

//                    if (_lockConflictList.Count > 0)
//                    {
//                        // Start TT#522 - stodd - size day to week summary
//                        if (!EventLog.SourceExists("MIDStoreVariableHistoryBin"))
//                        {
//                            EventLog.CreateEventSource("MIDStoreVariableHistoryBin", null);
//                        }
//                        foreach (MIDEnqueueConflict conflict in _lockConflictList)
//                        {
//                            string labels = string.Empty;
//                            string keysInCon = string.Empty;
//                            foreach (string st in conflict.KeyLabel)
//                            {
//                                labels = labels + st + " ";
//                            }
//                            foreach (int iKey in conflict.Key_InConflict)
//                            {
//                                keysInCon = keysInCon + iKey + " ";
//                            }
//                            string msg = "Lock Conflict. Lock Type: " + conflict.LockType + " User: " + conflict.OwnedByUserID + " Labels: " + labels +
//                                " Keys: " + keysInCon;
//                            EventLog.WriteEntry("MIDStoreVariableHistoryBin", msg, EventLogEntryType.Error);
//                        }
//                        // End TT#522 - stodd - size day to week summary

//                        throw new EnqueueConflictException();
//                    }
//                    else
//                    {
//                        HistoryDatabaseBinKey lockKey;
//                        List<HistoryDatabaseBinKey> dayKeys = new List<HistoryDatabaseBinKey>();
//                        List<HistoryDatabaseBinKey> weekKeys = new List<HistoryDatabaseBinKey>();
//                        _midEnqueue.OpenUpdateConnection();
//                        //int[] sizeHnUser = new int[2];
//                        int[] sizeHnUser;
//                        for (int i = 0; i < aSQL_TimeIDs.Length; i++)
//                        {
//                            lockKey = new HistoryDatabaseBinKey((short)aSQL_TimeIDs[i].SqlTimeID, aStyleHnRIDs[i], aColorCodeRIDs[i], aSizeCodeRIDs[i]);
//                            _midEnqueue.StoreVariableHistory_EnqueueInsert(aSQL_TimeIDs[i], aHnRIDs[i], aUserRID, _clientThreadID);
//                            sizeHnUser = new int[2];
//                            sizeHnUser[0] = aHnRIDs[i];
//                            sizeHnUser[1] = aUserRID;
//                            if (aSQL_TimeIDs[i].TimeIdType == eSQLTimeIdType.TimeIdIsDaily)
//                            {
//                                dayKeys.Add(lockKey);
//                                _lockDayDictionary.Add(lockKey, sizeHnUser);
//                            }
//                            else
//                            {
//                                weekKeys.Add(lockKey);
//                                _lockWeekDictionary.Add(lockKey, sizeHnUser);
//                            }
//                        }
//                        _midEnqueue.CommitData();
//                        Flush(dayKeys, weekKeys);   // TT#173 Provide database container for large data collections
//                        returnCode = true;
//                    }
//                }
//                catch
//                {
//                    returnCode = false;
//                    throw;
//                }
//                finally
//                {
//                    if (_midEnqueue.ConnectionIsOpen)
//                    {
//                        _midEnqueue.CloseUpdateConnection();
//                    }
//                }
//            }
//            return returnCode;
//        }

//        ///// <summary>
//        ///// Reads the current locks for the specified Style Hierarchy Nodes
//        ///// </summary>
//        ///// <param name="aHnRIDs">List of Style Hierarchy Node RIDs to be read</param>
//        ///// <returns>DataTable containing information about current locks</returns>
//        //private DataTable Lock_Read(int[] aHnRIDs)
//        //{
//        //    string hnRIds = string.Empty, keyString;

//        //    for (int i = 0; i < aHnRIDs.Length; i++)
//        //    {
//        //        keyString = Convert.ToString(aHnRIDs[i], CultureInfo.CurrentUICulture);
//        //        if (i == 0)
//        //            hnRIds = " AND RID = " + keyString;
//        //        else
//        //            hnRIds = hnRIds + " OR RID = " + keyString;
//        //    }
//        //    return _midEnqueue.Enqueue_Read(eLockType.StoreVariableHistory, hnRIds);
//        //}
        

//        ///// <summary>
//        ///// Locks the specified Style Hierarchy Nodes
//        ///// </summary>
//        ///// <param name="aUserRID">RID of the user requesting lock</param>
//        ///// <param name="aHnRIDs">List of Style Hierarchy Node RIDs to be locked</param>
//        //private void Lock_Insert(int aUserRID, SQL_TimeID[] aSQL_TimeIDs, int[] aHnRIDs, int[] aStyleRIDs, int[] aColorCodeRIDs, int[] aSizeCodeRIDs)
//        //{
//        //    for (int i = 0; i < aHnRIDs.Length; i++)
//        //    {
//        //        _midEnqueue.Enqueue_Insert(eLockType.StoreVariableHistory, aHnRIDs[i], aUserRID, _clientThreadID);
//        //        _lockDictionary.Add(aHnRIDs[i], aUserRID);
//        //    }
//        //}


//        /// <summary>
//        /// Unlocks specified Hierarchy Nodes
//        /// </summary>
//        /// <param name="aHnRID">List of the Hierarchy Nodes to be UnLocked</param>
//        //public void UnLockStyleNode(int[] aStyleHnRID)
//        public void UnLockTimeHnRID(SQL_TimeID[] aSQL_TimeIDs, int[] aStyleHnRIDs, int[] aColorCodeRIDs, int[] aSizeCodeRIDs)
//        {
//            if (aSQL_TimeIDs.Length != aStyleHnRIDs.Length
//                || aSQL_TimeIDs.Length != aColorCodeRIDs.Length
//                || aSQL_TimeIDs.Length != aSizeCodeRIDs.Length)
//            {
//                throw new ArgumentException("LockTimeHnRIDNode parameter arrays must contain same number of entries");
//            } 
//            try
//            {
//                _midEnqueue.OpenUpdateConnection();
//                for (int i = 0; i < aSQL_TimeIDs.Length; i++)
//                {
//                    try
//                    {
//                        HistoryDatabaseBinKey key = new HistoryDatabaseBinKey((short)aSQL_TimeIDs[i].SqlTimeID, aStyleHnRIDs[i], aColorCodeRIDs[i], aSizeCodeRIDs[i]);
//                        if (aSQL_TimeIDs[i].TimeIdType == eSQLTimeIdType.TimeIdIsDaily)
//                        {
//                            int[] userHnRID = _lockDayDictionary[key];
//                            _midEnqueue.StoreVariableHistory_EnqueueDelete(aSQL_TimeIDs[i], userHnRID[0], userHnRID[1]);
//                            _lockDayDictionary.Remove(key);
//                        }
//                        else
//                        {
//                            int[] userHnRID = _lockWeekDictionary[key];
//                            _midEnqueue.StoreVariableHistory_EnqueueDelete(aSQL_TimeIDs[i], userHnRID[0], userHnRID[1]);
//                            _lockWeekDictionary.Remove(key);
//                        }
//                        _midEnqueue.CommitData();
//                    }
//                    catch
//                    {
//                    }
//                }
//                //for (int i = 0; i < aStyleHnRID.Length; i++)
//                //{
//                //    try
//                //    {
//                //        int userRID = _lockDictionary[aStyleHnRID[i]];
//                //        _midEnqueue.Enqueue_Delete(eLockType.StoreVariableHistory, aStyleHnRID[i], userRID);
//                //        _lockDictionary.Remove(aStyleHnRID[i]);
//                //        _midEnqueue.CommitData();
//                //    }
//                //    catch
//                //    {
//                //    }
//                //}
//            }
//            finally
//            {
//                _midEnqueue.CloseUpdateConnection();
//            }
//        }
//        /// <summary>
//        /// Removes all Hierarchy Node Locks created by this instance of this class
//        /// </summary>
//        public void RemoveAllLocks()
//        {
//            HistoryDatabaseBinKey[] historyKeys;
//            SQL_TimeID[] sqlTimeIDs;
//            int[] styleHnRIDs;
//            int[] colorCodeRIDs;
//            int[] sizeCodeRIDs;
//            if (_lockDayDictionary.Count > 0)
//            {
//                historyKeys = new HistoryDatabaseBinKey[_lockDayDictionary.Count];
//                _lockDayDictionary.Keys.CopyTo(historyKeys, 0);
//                sqlTimeIDs = new SQL_TimeID[historyKeys.Length];
//                styleHnRIDs = new int[historyKeys.Length];
//                colorCodeRIDs = new int [historyKeys.Length];
//                sizeCodeRIDs = new int [historyKeys.Length];
//                for (int i=0; i<historyKeys.Length; i++)
//                {
//                    sqlTimeIDs[i] = new SQL_TimeID('D', historyKeys[i].TimeID);
//                    styleHnRIDs[i] = historyKeys[i].HnRID;
//                    colorCodeRIDs[i] = historyKeys[i].ColorCodeRID;
//                    sizeCodeRIDs[i] = historyKeys[i].SizeCodeRID;
//                }
//                UnLockTimeHnRID(sqlTimeIDs, styleHnRIDs, colorCodeRIDs, sizeCodeRIDs);
//            }
//            if (_lockWeekDictionary.Count > 0)
//            {
//                historyKeys = new HistoryDatabaseBinKey[_lockWeekDictionary.Count];
//                _lockDayDictionary.Keys.CopyTo(historyKeys, 0);
//                sqlTimeIDs = new SQL_TimeID[historyKeys.Length];
//                styleHnRIDs = new int[historyKeys.Length];
//                colorCodeRIDs = new int[historyKeys.Length];
//                sizeCodeRIDs = new int[historyKeys.Length];
//                for (int i = 0; i < historyKeys.Length; i++)
//                {
//                    sqlTimeIDs[i] = new SQL_TimeID('W', historyKeys[i].TimeID);
//                    styleHnRIDs[i] = historyKeys[i].HnRID;
//                    colorCodeRIDs[i] = historyKeys[i].ColorCodeRID;
//                    sizeCodeRIDs[i] = historyKeys[i].SizeCodeRID;
//                }
//                UnLockTimeHnRID(sqlTimeIDs, styleHnRIDs, colorCodeRIDs, sizeCodeRIDs);
//            }
//        }
//        #endregion Lock

//        #region Flush
//        public void Flush(SQL_TimeID[] aSQL_TimeIDs, int[] aHnRIDs, int[] aStyleHnRIDs, int[] aColorCodeRIDs, int[] aSizeCodeRIDs)
//        {
//            if (aSQL_TimeIDs.Length != aHnRIDs.Length
//                || aSQL_TimeIDs.Length != aStyleHnRIDs.Length
//                || aSQL_TimeIDs.Length != aColorCodeRIDs.Length
//                || aSQL_TimeIDs.Length != aSizeCodeRIDs.Length)
//            {
//                throw new ArgumentException("LockTimeHnRIDNode parameter arrays must contain same number of entries");
//            }
//            List<HistoryDatabaseBinKey> dayKeys = new List<HistoryDatabaseBinKey>();
//            List<HistoryDatabaseBinKey> weekKeys = new List<HistoryDatabaseBinKey>();
//            for (int i = 0; i < aSQL_TimeIDs.Length; i++)
//            {
//                HistoryDatabaseBinKey key = new HistoryDatabaseBinKey((short)aSQL_TimeIDs[i].SqlTimeID, aStyleHnRIDs[i], aColorCodeRIDs[i], aSizeCodeRIDs[i]);
//                if (aSQL_TimeIDs[i].TimeIdType == eSQLTimeIdType.TimeIdIsDaily)
//                {
//                    dayKeys.Add(key);
//                }
//                else
//                {
//                    weekKeys.Add(key);
//                }
//            }
//            Flush(dayKeys, weekKeys);
//        }
//        /// <summary>
//        /// Flushes the containers associated with the given hierarchy node RIDs from memory
//        /// </summary>
//        /// <param name="aHnRIDs">Array of the Hierarchy Node RIDs to flush</param>
//        public void Flush(int[] aHnRIDs)
//        {
//            List<HistoryDatabaseBinKey> dayKeys = new List<HistoryDatabaseBinKey>();
//            List<HistoryDatabaseBinKey> weekKeys = new List<HistoryDatabaseBinKey>();
//            for (int i = 0; i < aHnRIDs.Length; i++)
//            {
//                foreach (HistoryDatabaseBinKey hdbk in _storeVariableDayContainer.Keys)
//                {
//                    if (hdbk.HnRID == aHnRIDs[i])
//                    {
//                        dayKeys.Add(hdbk);
//                    }
//                }
//                foreach (HistoryDatabaseBinKey hdbk in _storeVariableWeekContainer.Keys)
//                {
//                    if (hdbk.HnRID == aHnRIDs[i])
//                    {
//                        weekKeys.Add(hdbk);
//                    }
//                }
//            }
//            Flush(dayKeys, weekKeys);
//        }
//        public void Flush(List<HistoryDatabaseBinKey> aDayKeys, List<HistoryDatabaseBinKey> aWeekKeys)
//        {
//            foreach (HistoryDatabaseBinKey hdbk in aDayKeys)
//            {
//                _storeVariableDayContainer.Remove(hdbk);
//            }
//            foreach (HistoryDatabaseBinKey hdbk in aWeekKeys)
//            {
//                _storeVariableWeekContainer.Remove(hdbk);
//            }
//            // begin TT#555 Total variable is aggregate of other variables
//            //_lastDatabaseBinKey = null;
//            //_lastStoreVariableVectorContainer = null;
//            _lastStoreVariableData = null;
//            // end TT#555 Total variable is aggregate of other variables
//        }
//        /// <summary>
//        /// Flushes the all cubes from memory
//        /// </summary>
//        public void FlushAll()
//        {
//            _storeVariableDayContainer.Clear();
//            _storeVariableWeekContainer.Clear();
//            // begin TT#555 Total variable is aggregate of other variables
//            //_lastDatabaseBinKey = null;
//            //_lastStoreVariableVectorContainer = null;
//            _lastStoreVariableData = null;
//            // end TT#555 Total variable is aggregate of other variables
//        }
//        #endregion Flush

//        #region GetStoreSizeAggregateValues
//        /// <summary>
//        /// Gets list of Store Size aggregated values across a given lists of variables, Size Aggregate Basis Keys and Time IDs
//        /// </summary>
//        /// <param name="aVariableNameList">List of variables</param>
//        /// <param name="aSizeAggregateKeyList">List of Size Aggregate Keys</param>
//        /// <param name="aTimeIDList">List Time IDs</param>
//        /// <returns>List of StoreSizeValues</returns>
//        public List<StoreSizeValue> GetStoreSizeAggregateValues
//            (string[] aVariableNameList,
//             SizeAggregateBasisKey[] aSizeAggregateKeyList,
//             SQL_TimeID[] aTimeIDList)
//        {
//            int[] storeRIDs = new int[MIDStorageTypeInfo.GetStoreMaxRID(0)];
//            Dictionary<int, int[]> sizeStoreValuesDict = new Dictionary<int, int[]>();
//            int[] sizeStoreValues;
//            double[] colorSizeStoreValues;
//            for (int i=0; i<storeRIDs.Length; i++)
//            {
//                storeRIDs[i] = i+1;
//            }
//            foreach (SQL_TimeID timeID in aTimeIDList)
//            {
//                foreach (SizeAggregateBasisKey sabk in aSizeAggregateKeyList)
//                {
//                    if (!sizeStoreValuesDict.TryGetValue(sabk.SizeCodeRID, out sizeStoreValues))
//                    {
//                        sizeStoreValues = new int[storeRIDs.Length];
//                        sizeStoreValuesDict.Add(sabk.SizeCodeRID, sizeStoreValues);
//                    }
//                    foreach (string variableName in aVariableNameList)
//                    {
//                        colorSizeStoreValues = 
//                            this.GetStoreVariableValue(variableName, sabk.HierarchyNodeRID, timeID, sabk.ColorCodeRID, sabk.SizeCodeRID, storeRIDs);
//                        for (int i = 0; i < storeRIDs.Length; i++)
//                        {
//                            sizeStoreValues[i] += (int)colorSizeStoreValues[i];
//                        }
//                    }
//                }
//                this.FlushAll();
//            }
//            List<StoreSizeValue> ssvList = new List<StoreSizeValue>();
//            foreach (KeyValuePair<int, int[]> kvp in sizeStoreValuesDict)
//            {
//                for (int i = 0; i < kvp.Value.Length; i++)
//                {
//                    if (kvp.Value[i] != 0)
//                    {
//                        ssvList.Add(new StoreSizeValue(storeRIDs[i], kvp.Key, kvp.Value[i]));
//                    }
//                }
//            }
//            return ssvList;
//        }
//        #endregion GetStoreSizeValues

//        #region GetStoreVariableValue
//        /// <summary>
//        /// Gets the Store Color-Size data value within the given Style for the specified variable at the specified TimeID
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Database Bin variable name</param>
//        /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
//        /// <param name="aTimeID">Time ID</param>
//        /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
//        /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
//        /// <param name="aStoreRID">RID that identifies the store</param>
//        /// <returns>Variable's Data Value for the given Store, Hierarchy Node, Color Code, Size Code and Time ID</returns>
//        public double GetStoreVariableValue
//            (string aDatabaseVariableName,
//            int aHnRID,
//            SQL_TimeID aTimeID,
//            int aColorCodeRID,
//            int aSizeCodeRID,
//            int aStoreRID)
//        {
//            int[] storeRID = new int[1];
//            storeRID[0] = aStoreRID;
//            return GetStoreVariableValue
//                (aDatabaseVariableName,
//                aHnRID,
//                aTimeID,
//                aColorCodeRID,
//                aSizeCodeRID,
//                storeRID)[0];
//        }
//        //Begin TT#739-MD -jsobek -Delete Stores -Bin Removal Prep
//        /// <summary>
//        /// Gets the Store Color-Size data value within the given Style for the specified variable at the specified TimeID
//        /// </summary>
//        /// <param name="aVariableID">ID code that identifies the variable</param>
//        /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
//        /// <param name="aTimeID">Time ID</param>
//        /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
//        /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
//        /// <param name="aStoreRID">RID that identifies the store</param>
//        /// <returns>Variable's Data Value for the given Store, Hierarchy Node, Color Code, Size Code and Time ID</returns>
//        //public double GetStoreVariableValue
//        //    (int aVariableID,
//        //    int aHnRID,
//        //    SQL_TimeID aTimeID,
//        //    int aColorCodeRID,
//        //    int aSizeCodeRID,
//        //    int aStoreRID)
//        //{
//        //    int[] storeRID = new int[1];
//        //    storeRID[0] = aStoreRID;
//        //    return GetStoreVariableValue
//        //        (aVariableID,
//        //        aHnRID,
//        //        aTimeID,
//        //        aColorCodeRID,
//        //        aSizeCodeRID,
//        //        storeRID)[0];
//        //}
//        //End TT#739-MD -jsobek -Delete Stores -Bin Removal Prep

//        //Begin TT#739-MD -jsobek -Delete Stores -Bin Removal Prep
//        /// <summary>
//        /// Gets the Store Color-Size data value within the given Style for the specified variable at the specified TimeID
//        /// </summary>
//        /// <param name="aVariableIDX">Variable Index (Ordinal, Column number) that identifies the variable</param>
//        /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
//        /// <param name="aTimeID">Time ID</param>
//        /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
//        /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
//        /// <param name="aStoreRID">RID that identifies the store</param>
//        /// <returns>Variable's Data Value for the given Store, Hierarchy Node, Color Code, Size Code and Time ID</returns>
//        //public double GetStoreVariableValue
//        //    (UInt16 aVariableIDX,
//        //    int aHnRID,
//        //    SQL_TimeID aTimeID,
//        //    int aColorCodeRID,
//        //    int aSizeCodeRID,
//        //    int aStoreRID)
//        //{
//        //    int[] storeRID = new int[1];
//        //    storeRID[0] = aStoreRID;
//        //    return GetStoreVariableValue
//        //        (aVariableIDX,
//        //        aHnRID,
//        //        aTimeID,
//        //        aColorCodeRID,
//        //        aSizeCodeRID,
//        //        storeRID)[0];
//        //}
//        //End TT#739-MD -jsobek -Delete Stores -Bin Removal Prep

//        /// <summary>
//        /// Gets the Color-Size data values for an array of stores within the given Style for the specified variable at the specified TimeID
//        /// </summary>
//        /// <param name="aDatabaseVariableName">A Database BIN Variable Name</param>
//        /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
//        /// <param name="aTimeID">Time ID</param>
//        /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
//        /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
//        /// <param name="aStoreRID">RIDs that identify the stores</param>
//        /// <returns>Variable's Data Values for the given Stores, Hierarchy Node, Color Code, Size Code and Time; data values array is in one-to-one correspondence with the original Store RID array</returns>
//        public double[] GetStoreVariableValue
//            (string aDatabaseVariableName,
//            int aHnRID,
//            SQL_TimeID aTimeID,
//            int aColorCodeRID,
//            int aSizeCodeRID,
//            int[] aStoreRID)
//        {
//            if (aTimeID.TimeIdType == eSQLTimeIdType.TimeIdIsDaily)
//            {
//                return GetStoreVariableDayValue(
//                    aDatabaseVariableName,
//                    aHnRID,
//                    aTimeID,
//                    aColorCodeRID,
//                    aSizeCodeRID,
//                    aStoreRID);
//            }
//            return GetStoreVariableWeekValue(
//                aDatabaseVariableName,
//                aHnRID,
//                aTimeID,
//                aColorCodeRID,
//                aSizeCodeRID,
//                aStoreRID);
//        }
//        /// <summary>
//        /// Gets the Color-Size data values for an array of stores within the given Style for the specified variable at the specified TimeID
//        /// </summary>
//        /// <param name="aVariableID">ID code that identifies the variable</param>
//        /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
//        /// <param name="aTimeID">Time ID</param>
//        /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
//        /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
//        /// <param name="aStoreRID">RIDs that identify the stores</param>
//        /// <returns>Variable's Data Values for the given Stores, Hierarchy Node, Color Code, Size Code and Time; data values array is in one-to-one correspondence with the original Store RID array</returns>
//        public double[] GetStoreVariableValue
//            (int aVariableID,
//            int aHnRID,
//            SQL_TimeID aTimeID,
//            int aColorCodeRID,
//            int aSizeCodeRID,
//            int[] aStoreRID)
//        {
//            if (aTimeID.TimeIdType == eSQLTimeIdType.TimeIdIsDaily)
//            {
//                return GetStoreVariableDayValue(
//                    aVariableID,
//                    aHnRID,
//                    aTimeID,
//                    aColorCodeRID,
//                    aSizeCodeRID,
//                    aStoreRID);
//            }
//            return GetStoreVariableWeekValue(
//                aVariableID,
//                aHnRID,
//                aTimeID,
//                aColorCodeRID,
//                aSizeCodeRID,
//                aStoreRID);
//        }
//        /// <summary>
//        /// Gets the Color-Size data values for an array of stores within the given Style for the specified variable at the specified TimeID
//        /// </summary>
//        /// <param name="aVariableIDX">INDEX code that identifies the variable (Ordinal, column number where variable resides)</param>
//        /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
//        /// <param name="aTimeID">Time ID</param>
//        /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
//        /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
//        /// <param name="aStoreRID">RIDs that identify the stores</param>
//        /// <returns>Variable's Data Values for the given Stores, Hierarchy Node, Color Code, Size Code and Time; data values array is in one-to-one correspondence with the original Store RID array</returns>
//        public double[] GetStoreVariableValue
//            (UInt16 aVariableIDX,
//            int aHnRID,
//            SQL_TimeID aTimeID,
//            int aColorCodeRID,
//            int aSizeCodeRID,
//            int[] aStoreRID)
//        {
//            if (aTimeID.TimeIdType == eSQLTimeIdType.TimeIdIsDaily)
//            {
//                return GetStoreVariableDayValue(
//                    aVariableIDX,
//                    aHnRID,
//                    aTimeID,
//                    aColorCodeRID,
//                    aSizeCodeRID,
//                    aStoreRID);
//            }
//            return GetStoreVariableWeekValue(
//                aVariableIDX,
//                aHnRID,
//                aTimeID,
//                aColorCodeRID,
//                aSizeCodeRID,
//                aStoreRID);
//        }
//        /// <summary>
//        /// Gets the Color-Size DAY data values for an array of stores within the given HnRID for the specified variable at the specified Time ID
//        /// </summary>
//        /// <param name="aDatabaseVariableName">A Database BIN Variable Name</param>
//        /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
//        /// <param name="aTimeID">Time ID</param>
//        /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
//        /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
//        /// <param name="aStoreRID">RIDs that identify the stores</param>
//        /// <returns>Variable's Data Values for the given Stores, Hierarchy Node, Color Code, Size Code and Time; data values array is in one-to-one correspondence with the original Store RID array</returns>
//        public double[] GetStoreVariableDayValue
//            (string aDatabaseVariableName,
//            int aHnRID,
//            SQL_TimeID aTimeID,
//            int aColorCodeRID,
//            int aSizeCodeRID,
//            int[] aStoreRID)
//        {
//            short variableIdx;  
//            if (GetHistoryDayDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx)) // TT#707 - JEllis - Container not thread safe (part 2)
//            {
//                return GetStoreVariableDayValue
//                    (variableIdx,
//                     aHnRID,
//                     aTimeID,
//                     aColorCodeRID,
//                     aSizeCodeRID,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable Name=" + aDatabaseVariableName + " is not a valid History Database BIN day variable");
//        }

  
//        /// <summary>
//        /// Gets the Color-Size DAY data values for an array of stores within the given HnRID for the specified variable at the specified Time ID
//        /// </summary>
//        /// <param name="aVariableID">ID Code that identifies the variable</param>
//        /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
//        /// <param name="aTimeID">Time ID</param>
//        /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
//        /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
//        /// <param name="aStoreRID">RIDs that identify the stores</param>
//        /// <returns>Variable's Data Values for the given Stores, Hierarchy Node, Color Code, Size Code and Time; data values array is in one-to-one correspondence with the original Store RID array</returns>
//        public double[] GetStoreVariableDayValue
//            (int aVariableID,
//            int aHnRID,
//            SQL_TimeID aTimeID,
//            int aColorCodeRID,
//            int aSizeCodeRID,
//            int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetHistoryDayDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx)) // TT#707 - JEllis - Container not thread safe (part 2)
//            {
//                return GetStoreVariableDayValue
//                    (variableIdx,
//                     aHnRID,
//                     aTimeID,
//                     aColorCodeRID,
//                     aSizeCodeRID,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable ID = " + aVariableID.ToString() + " is not a valid History Database BIN day variable");
//        }
//        /// <summary>
//        /// Gets the Color-Size DAY data values for an array of stores within the given HnRID for the specified variable INDEX at the specified Time ID
//        /// </summary>
//        /// <param name="aVariableIDX">Database Variable INDEX (Ordinal) of the variable (the column number where the variable resides</param>
//        /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
//        /// <param name="aTimeID">Time ID</param>
//        /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
//        /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
//        /// <param name="aStoreRID">RIDs that identify the stores</param>
//        /// <returns>Variable's Data Values for the given Stores, Hierarchy Node, Color Code, Size Code and Time; data values array is in one-to-one correspondence with the original Store RID array</returns>
//        public double[] GetStoreVariableDayValue
//            (short aVariableIDX,
//             int aHnRID,
//             SQL_TimeID aTimeID,
//             int aColorCodeRID,
//             int aSizeCodeRID,
//             int[] aStoreRID)  
//        {
//            // begin TT#555 Total variable is aggregate of other variables
//            //StoreVariableVectorContainer svvc = GetStoreVariableVectorContainer(aTimeID, aHnRID, aColorCodeRID, aSizeCodeRID);
//            StoreVariableData<HistoryDatabaseBinKey> svd = GetStoreVariableData(aTimeID, aHnRID, aColorCodeRID, aSizeCodeRID);
//            if (svd.InitialCalcVariables
//                && GetHistoryDayDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).VariableIsCalculated) // TT#707 - JEllis - Container not thread safe (part 2)
//            {
//                svd.CalculateVariables(GetHistoryDayDatabaseBin().VariableModel); // TT#707 - JEllis - Container not thread safe (part 2)
//            }
//            StoreVariableVectorContainer svvc = svd.StoreVariableVectorContainer;
//            // end TT#555 Total variable is aggregate of other variables
//            double[] variableValues = new double[aStoreRID.Length];
//            int storeRID;
//            double multiplier = Math.Pow(10, GetHistoryDayDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision); // TT#707 - JEllis - Container not thread safe (part 2)
//            for (int i = 0; i < aStoreRID.Length; i++)
//            {
//                storeRID = aStoreRID[i];
//                variableValues[i] =
//                svvc.GetStoreVariableValue(storeRID, aVariableIDX) / multiplier;
//            }
//            return variableValues;
//        }
//        /// <summary>
//        /// Gets the Color-Size WEEK data values for an array of stores within the given HnRID for the specified variable at the specified Time ID
//        /// </summary>
//        /// <param name="aDatabaseVariableName">A Database BIN Variable Name</param>
//        /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
//        /// <param name="aTimeID">Time ID</param>
//        /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
//        /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
//        /// <param name="aStoreRID">RIDs that identify the stores</param>
//        /// <returns>Variable's Data Values for the given Stores, Hierarchy Node, Color Code, Size Code and Time; data values array is in one-to-one correspondence with the original Store RID array</returns>
//        public double[] GetStoreVariableWeekValue
//          (string aDatabaseVariableName,
//           int aHnRID,
//           SQL_TimeID aTimeID,
//           int aColorCodeRID,
//           int aSizeCodeRID,
//           int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetHistoryWeekDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx)) // TT#707 - JEllis - Container not thread safe (part 2)
//            {
//                return GetStoreVariableWeekValue
//                    (variableIdx,
//                     aHnRID,
//                     aTimeID,
//                     aColorCodeRID,
//                     aSizeCodeRID,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable Name=" + aDatabaseVariableName + " is not a valid History Database BIN week variable");
//        }
//        /// <summary>
//        /// Gets the Color-Size WEEK data values for an array of stores within the given HnRID for the specified variable at the specified Time ID
//        /// </summary>
//        /// <param name="aVariableID">ID Code that identifies the variable</param>
//        /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
//        /// <param name="aTimeID">Time ID</param>
//        /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
//        /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
//        /// <param name="aStoreRID">RIDs that identify the stores</param>
//        /// <returns>Variable's Data Values for the given Stores, Hierarchy Node, Color Code, Size Code and Time; data values array is in one-to-one correspondence with the original Store RID array</returns>
//         public double[] GetStoreVariableWeekValue
//           (int aVariableID,
//            int aHnRID,
//            SQL_TimeID aTimeID,
//            int aColorCodeRID,
//            int aSizeCodeRID,
//            int[] aStoreRID)
//        {
//            short variableIdx;
//            if (GetHistoryWeekDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx)) // TT#707 - JEllis - Container not thread safe (part 2)
//            {
//                return GetStoreVariableWeekValue
//                    (variableIdx,
//                     aHnRID,
//                     aTimeID,
//                     aColorCodeRID,
//                     aSizeCodeRID,
//                     aStoreRID);
//            }
//            throw new ArgumentException("Variable ID=" + aVariableID.ToString() + " is not a valid History Database BIN week variable");
//        }
//         /// <summary>
//         /// Gets the Color-Size WEEK data values for an array of stores within the given HnRID for the specified variable INDEX at the specified Time ID
//         /// </summary>
//         /// <param name="aVariableIDX">Database Variable INDEX (Ordinal) of the variable (the column number where the variable resides</param>
//         /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
//         /// <param name="aTimeID">Time ID</param>
//         /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
//         /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
//         /// <param name="aStoreRID">RIDs that identify the stores</param>
//         /// <returns>Variable's Data Values for the given Stores, Hierarchy Node, Color Code, Size Code and Time; data values array is in one-to-one correspondence with the original Store RID array</returns>
//         public double[] GetStoreVariableWeekValue
//            (short aVariableIDX,
//             int aHnRID,
//             SQL_TimeID aTimeID,
//             int aColorCodeRID,
//             int aSizeCodeRID,
//             int[] aStoreRID)
//        {
//             // begin TT#555 Total variable is aggregate of other variables
//             //StoreVariableVectorContainer svvc = GetStoreVariableVectorContainer(aTimeID, aHnRID, aColorCodeRID, aSizeCodeRID);
//             StoreVariableData<HistoryDatabaseBinKey> svd = GetStoreVariableData(aTimeID, aHnRID, aColorCodeRID, aSizeCodeRID);
//             if (svd.InitialCalcVariables
//                && GetHistoryWeekDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).VariableIsCalculated)  // TT#707 - JEllis - Container not thread safe (part 2)
//             {
//                svd.CalculateVariables(GetHistoryWeekDatabaseBin().VariableModel); // TT#707 - JEllis - Container not thread safe (part 2)
//             }
//             StoreVariableVectorContainer svvc = svd.StoreVariableVectorContainer;
//             // end TT#555 Total variable is aggregate of other variables
//             double[] variableValues = new double[aStoreRID.Length];
//             int storeRID;
//            double multiplier = Math.Pow(10, GetHistoryWeekDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision); // TT#707 - JEllis - Container not thread safe (part 2)
//             for (int i = 0; i < aStoreRID.Length; i++)
//             {
//                 storeRID = aStoreRID[i];
//                 variableValues[i] =
//                 svvc.GetStoreVariableValue(storeRID, aVariableIDX) / multiplier;
//             }
//             return variableValues;
//        }

//        #endregion GetStoreVariableValue

//        #region SetStoreVariableValue
//        /// <summary>
//        /// Sets the Color-Size data values for an array of stores within the given Style for the specified variable at the specified TimeID; The Store and Variable arrays must be in one-to-one correspondence
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Name of a database Bin Variable</param>
//        /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
//        /// <param name="aTimeID">Time ID</param>
//         /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
//         /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
//         /// <param name="aStoreRIDs">RIDs that identify the stores</param>
//        /// <param name="aVariableValues">Variable's data values for the given stores in the given Color-Size and Time</param>
//        public void SetStoreVariableValue
//             (string aDatabaseVariableName,
//             int aHnRID,
//             SQL_TimeID aTimeID,
//             int aColorCodeRID,
//             int aSizeCodeRID,
//             int[] aStoreRIDs,
//             double[] aVariableValues
//             )
//        {
//            if (aTimeID.TimeIdType == eSQLTimeIdType.TimeIdIsDaily)
//            {
//                SetStoreVariableDayValue
//                    (aDatabaseVariableName,
//                    aHnRID,
//                    aTimeID,
//                    aColorCodeRID,
//                    aSizeCodeRID,
//                    aStoreRIDs,
//                    aVariableValues
//                    );
//            }
//            else
//            {
//                SetStoreVariableWeekValue
//                    (aDatabaseVariableName,
//                    aHnRID,
//                    aTimeID,
//                    aColorCodeRID,
//                    aSizeCodeRID,
//                    aStoreRIDs,
//                    aVariableValues
//                    );
//            }
//        }
//        /// <summary>
//        /// Sets the Color-Size data values for an array of stores within the given Style for the specified variable at the specified TimeID; The Store and Variable arrays must be in one-to-one correspondence
//        /// </summary>
//        /// <param name="aVariableID">ID code that identifies the variable</param>
//        /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
//        /// <param name="aTimeID">Time ID</param>
//        /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
//        /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
//        /// <param name="aStoreRIDs">RIDs that identify the stores</param>
//        /// <param name="aVariableValues">Variable's data values for the given stores in the given Color-Size and Time</param>
//        public void SetStoreVariableValue
//            (int aVariableID,
//            int aHnRID,
//            SQL_TimeID aTimeID,
//            int aColorCodeRID,
//            int aSizeCodeRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            if (aTimeID.TimeIdType == eSQLTimeIdType.TimeIdIsDaily)
//            {
//                SetStoreVariableDayValue
//                    (aVariableID,
//                    aHnRID,
//                    aTimeID,
//                    aColorCodeRID,
//                    aSizeCodeRID,
//                    aStoreRIDs,
//                    aVariableValues
//                    );
//            }
//            else
//            {
//                SetStoreVariableWeekValue
//                    (aVariableID,
//                    aHnRID,
//                    aTimeID,
//                    aColorCodeRID,
//                    aSizeCodeRID,
//                    aStoreRIDs,
//                    aVariableValues
//                    );
//            }
//        }
//        /// <summary>
//        /// Sets the Color-Size data values for an array of stores within the given Style for the specified variable at the specified TimeID; The Store and Variable arrays must be in one-to-one correspondence
//        /// </summary>
//        /// <param name="aVariableIDX">INDEX code that identifies the variable (Ordinal, column number on database table where variable resides)</param>
//        /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
//        /// <param name="aTimeID">Time ID</param>
//        /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
//        /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
//        /// <param name="aStoreRIDs">RIDs that identify the stores</param>
//        /// <param name="aVariableValues">Variable's data values for the given stores in the given Color-Size and Time</param>
//        public void SetStoreVariableValue
//            (UInt16 aVariableIDX,
//            int aHnRID,
//            SQL_TimeID aTimeID,
//            int aColorCodeRID,
//            int aSizeCodeRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            if (aTimeID.TimeIdType == eSQLTimeIdType.TimeIdIsDaily)
//            {
//                SetStoreVariableDayValue
//                    (aVariableIDX,
//                    aHnRID,
//                    aTimeID,
//                    aColorCodeRID,
//                    aSizeCodeRID,
//                    aStoreRIDs,
//                    aVariableValues
//                    );
//            }
//            else
//            {
//                SetStoreVariableWeekValue
//                    (aVariableIDX,
//                    aHnRID,
//                    aTimeID,
//                    aColorCodeRID,
//                    aSizeCodeRID,
//                    aStoreRIDs,
//                    aVariableValues
//                    );
//            }
//        }
//        /// <summary>
//        /// Sets the Color-Size DAY data values for an array of stores within the given Style for the specified variable at the specified TimeID; The Store and Variable arrays must be in one-to-one correspondence
//        /// </summary>
//        /// <param name="aDatabaseVariableName">Name of Database BIN variable</param>
//        /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
//        /// <param name="aTimeID">Time ID</param>
//        /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
//        /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
//        /// <param name="aStoreRIDs">RIDs that identify the stores</param>
//        /// <param name="aVariableValues">Variable's data values for the given stores in the given Color-Size and Time</param>
//        public void SetStoreVariableDayValue
//            (string aDatabaseVariableName,
//            int aHnRID,
//            SQL_TimeID aTimeID,
//            int aColorCodeRID,
//            int aSizeCodeRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetHistoryDayDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx))  // TT#707 - JEllis - Container not thread safe (part 2)
//            {
//                SetStoreVariableDayValue
//                    (variableIdx,
//                     aHnRID,
//                     aTimeID,
//                     aColorCodeRID,
//                     aSizeCodeRID,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable Name = " + aDatabaseVariableName + " is not a valid variable for table BIN = " + GetHistoryDayDatabaseBin().BinTableName);  // TT#707 - JEllis - Container not thread safe (part 2)
//        }
//        /// <summary>
//        /// Sets the Color-Size DAY data values for an array of stores within the given Style for the specified variable at the specified TimeID; The Store and Variable arrays must be in one-to-one correspondence
//        /// </summary>
//        /// <param name="aVariableID">ID code that identifies the variable</param>
//        /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
//        /// <param name="aTimeID">Time ID</param>
//        /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
//        /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
//        /// <param name="aStoreRIDs">RIDs that identify the stores</param>
//        /// <param name="aVariableValues">Variable's data values for the given stores in the given Color-Size and Time</param>
//        public void SetStoreVariableDayValue
//            (int aVariableID,
//            int aHnRID,
//            SQL_TimeID aTimeID,
//            int aColorCodeRID,
//            int aSizeCodeRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetHistoryDayDatabaseBin().VariableModel.TryGetVariableIDX(aVariableID, out variableIdx)) // TT#707 - JEllis - Container not thread safe (part 2)
//            {
//                SetStoreVariableDayValue
//                    (variableIdx,
//                     aHnRID,
//                     aTimeID,
//                     aColorCodeRID,
//                     aSizeCodeRID,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable ID = " + aVariableID.ToString() + " is not a valid variable for table BIN = " + GetHistoryDayDatabaseBin().BinTableName); // TT#707 - JEllis - Container not thread safe (part 2)
//        }
//        /// <summary>
//        /// Sets the Color-Size DAY data values for an array of stores within the given Style for the specified variable INDEX (Ordinal, column number) at the specified TimeID; The Store and Variable arrays must be in one-to-one correspondence
//        /// </summary>
//        /// <param name="aVariableIDX">Database Variable INDEX (Ordinal) of the variable (the column number where the variable resides</param>
//        /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
//        /// <param name="aTimeID">Time ID</param>
//        /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
//        /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
//        /// <param name="aStoreRIDs">RIDs that identify the stores</param>
//        /// <param name="aVariableValues">Variable's data values for the given stores in the given Color-Size and Time</param>
//        public void SetStoreVariableDayValue
//            (short aVariableIDX,
//             int aHnRID,
//             SQL_TimeID aTimeID,
//             int aColorCodeRID,
//             int aSizeCodeRID,
//             int[] aStoreRIDs,
//             double[] aVariableValues)
//        {
//            if (!_updateAccess)
//            {
//                throw new Exception("'update access' was not declared when StoreVariableHistBin was instantiated");
//            }
//            if (aStoreRIDs.Length != aVariableValues.Length)
//            {
//                throw new Exception("Store array length = " + aStoreRIDs.Length + " is not same as Variable array length = " + aVariableValues.Length);
//            }
//            // begin TT#467 Enq Conflict Except During Size Hist Load
//            //if (this._lockDictionary.ContainsKey(aHnRID))
//            //{
//            int storeRID;
//            Int64 variableValue;
//            // begin TT#555 Total variable is aggregate of other variables
//            //StoreVariableVectorContainer svvc = GetStoreVariableVectorContainer(aTimeID, aHnRID, aColorCodeRID, aSizeCodeRID);
//            StoreVariableData<HistoryDatabaseBinKey> svd = GetStoreVariableData(aTimeID, aHnRID, aColorCodeRID, aSizeCodeRID);
//            StoreVariableVectorContainer svvc = svd.StoreVariableVectorContainer;
//            // end TT#555 Total variable is aggregate of other variables
//            bool processSet = false;

//            if (_lastTimeIDType == eSQLTimeIdType.TimeIdIsDaily)
//            {
//                // begin TT#555 Total variable is aggregate of other variables
//                //if (_lockDayDictionary.ContainsKey(_lastDatabaseBinKey))
//                if (_lockDayDictionary.ContainsKey(svd.DatabaseBinKey))
//                    // end TT#555 Total variable is aggregate of othe variables
//                {
//                    processSet = true;
//                }
//            }
//            else
//            {
//                // begin TT#555 Total variable is aggregate of other variables
//                //if (_lockWeekDictionary.ContainsKey(_lastDatabaseBinKey))
//                if (_lockWeekDictionary.ContainsKey(svd.DatabaseBinKey))
//                    // end TT#555 Total variable is aggregate of other variables
//                {
//                    processSet = true;
//                }
//            }
//            if (processSet)
//            {
//                eMIDVariableModelType variableModelType = (eMIDVariableModelType)GetHistoryDayDatabaseBin().VariableModel.VariableModelID; // TT#707 - JEllis - Container not thread safe (part 2)
//                double multiplier = Math.Pow(10, GetHistoryDayDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision); // TT#707 - JEllis - Container not thread safe (part 2)
//                svd.InitialCalcVariables = true; // TT#555 Total variable is aggregate of other variables
//                for (int i = 0; i < aStoreRIDs.Length; i++)
//                {
//                    storeRID = aStoreRIDs[i];
//                    if (aVariableValues[i] < 0)
//                    {
//                        variableValue =
//                           (Int64)(aVariableValues[i]
//                                   * multiplier
//                                   - .5);   // rounding should use same sign as original value
//                    }
//                    else
//                    {
//                        variableValue =
//                            (Int64)(aVariableValues[i]
//                                    * multiplier
//                                    + .5);
//                    }
//                    svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//                    svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//                }
//                return;
//            }
            
//            throw new Exception("update failed because Time/Hierarchy Node RID is not locked. TimeID='" + aTimeID.ToString() + "' Hierarchy Node RID='" + aHnRID.ToString() + "'" + " Color Code RID='" + aColorCodeRID.ToString() + "' Size Code RID='" + aSizeCodeRID.ToString() +"'");
//        }
//        /// <summary>
//        /// Sets the Color-Size WEEK data values for an array of stores within the given Style for the specified variable at the specified TimeID; The Store and Variable arrays must be in one-to-one correspondence
//        /// </summary>
//        /// <param name="aDatabaseVariableName">A Database Bin Variable Name</param>
//        /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
//        /// <param name="aTimeID">Time ID</param>
//        /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
//        /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
//        /// <param name="aStoreRIDs">RIDs that identify the stores</param>
//        /// <param name="aVariableValues">Variable's data values for the given stores in the given Color-Size and Time</param>     
//        public void SetStoreVariableWeekValue
//            (string aDatabaseVariableName,
//            int aHnRID,
//            SQL_TimeID aTimeID,
//            int aColorCodeRID,
//            int aSizeCodeRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetHistoryWeekDatabaseBin().VariableModel.TryGetVariableIDX(aDatabaseVariableName, out variableIdx))  // TT#707 - JEllis - Container not thread safe (part 2)
//            {
//                SetStoreVariableWeekValue
//                    (variableIdx,
//                     aHnRID,
//                     aTimeID,
//                     aColorCodeRID,
//                     aSizeCodeRID,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable Name = " + aDatabaseVariableName + " is not a valid variable for table BIN = " + GetHistoryDayDatabaseBin().BinTableName); // TT#707 - JEllis - Container not thread safe (part 2)
//        }

//        /// <summary>
//        /// Sets the Color-Size WEEK data values for an array of stores within the given Style for the specified variable at the specified TimeID; The Store and Variable arrays must be in one-to-one correspondence
//        /// </summary>
//        /// <param name="aVariableID">ID code that identifies the variable</param>
//        /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
//        /// <param name="aTimeID">Time ID</param>
//        /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
//        /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
//        /// <param name="aStoreRIDs">RIDs that identify the stores</param>
//        /// <param name="aVariableValues">Variable's data values for the given stores in the given Color-Size and Time</param>     
//        public void SetStoreVariableWeekValue
//            (int aVariableID,
//            int aHnRID,
//            SQL_TimeID aTimeID,
//            int aColorCodeRID,
//            int aSizeCodeRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            short variableIdx;
//            if (GetHistoryWeekDatabaseBin().VariableModel.TryGetVariableIDX((int)aVariableID, out variableIdx)) // TT#707 - JEllis - Container not thread safe (part 2)
//            {
//                SetStoreVariableWeekValue
//                    (variableIdx,
//                     aHnRID,
//                     aTimeID,
//                     aColorCodeRID,
//                     aSizeCodeRID,
//                     aStoreRIDs,
//                     aVariableValues);
//                return;
//            }
//            throw new ArgumentException("Variable ID = " + aVariableID.ToString() + " is not a valid variable for table BIN = " + GetHistoryDayDatabaseBin().BinTableName); // TT#707 - JEllis - Container not thread safe (part 2)
//        }

//        /// <summary>
//        /// Sets the Color-Size WEEK data values for an array of stores within the given Style for the specified variable at the specified TimeID; The Store and Variable arrays must be in one-to-one correspondence
//        /// </summary>
//        /// <param name="aVariableIDX">Database Variable INDEX (Ordinal) of the variable (the column number where the variable resides</param>
//        /// <param name="aHnRID">RID that identifies the Hierarchy Node</param>
//        /// <param name="aTimeID">Time ID</param>
//        /// <param name="aColorCodeRID">RID that identifies the Color Code</param>
//        /// <param name="aSizeCodeRID">RID that identifies the Size Code</param>
//        /// <param name="aStoreRIDs">RIDs that identify the stores</param>
//        /// <param name="aVariableValues">Variable's data values for the given stores in the given Color-Size and Time</param>
//        public void SetStoreVariableWeekValue
//            (short aVariableIDX,
//            int aHnRID,
//            SQL_TimeID aTimeID,
//            int aColorCodeRID,
//            int aSizeCodeRID,
//            int[] aStoreRIDs,
//            double[] aVariableValues
//            )
//        {
//            if (!_updateAccess)
//            {
//                throw new Exception("'update access' was not declared when SizeVariableQuickAccess was instantiated");
//            }
//            if (aStoreRIDs.Length != aVariableValues.Length)
//            {
//                throw new Exception("Store array length = " + aStoreRIDs.Length + " is not same as Variable array length = " + aVariableValues.Length);
//            }
//            // begin TT#467 Enq Conflict Except During Size Hist Load
//            //if (_lastLockHnRID == aHnRID
//            //    || this._lockDictionary.ContainsKey(aHnRID))
//            //{
//            //    _lastLockHnRID = aHnRID;
//            int storeRID;
//            Int64 variableValue;

//            // begin TT#555 Total variable is aggregate of other variables
//            //StoreVariableVectorContainer svvc = GetStoreVariableVectorContainer(aTimeID, aHnRID, aColorCodeRID, aSizeCodeRID);
//            StoreVariableData<HistoryDatabaseBinKey> svd = GetStoreVariableData(aTimeID, aHnRID, aColorCodeRID, aSizeCodeRID);
//            StoreVariableVectorContainer svvc = svd.StoreVariableVectorContainer;
//            // end TT#555 Total variable is aggregate of other variables
//            bool processSet = false;
//            if (_lastTimeIDType == eSQLTimeIdType.TimeIdIsDaily)
//            {
//                // begin TT#555 Total variable is aggregate of other variables
//                //if (_lockDayDictionary.ContainsKey(_lastDatabaseBinKey))
//                if (_lockDayDictionary.ContainsKey(svd.DatabaseBinKey))
//                    // end TT#555 Total Variable is aggregate of other variables
//                {
//                    processSet = true;
//                }
//            }
//            else
//            {
//                // begin TT#555 Total variable is aggregate of other variables
//                //if (_lockWeekDictionary.ContainsKey(_lastDatabaseBinKey))
//                if (_lockWeekDictionary.ContainsKey(svd.DatabaseBinKey))
//                    // end TT#555 Total variable is aggregate of other variables
//                {
//                    processSet = true;
//                }
//            }
//            if (processSet)
//            {
//                // end TT#467 Enq Confict Except During Size Hist Load
//                eMIDVariableModelType variableModelType = (eMIDVariableModelType)GetHistoryWeekDatabaseBin().VariableModel.VariableModelID;  // TT#707 - JEllis - Container not thread safe (part 2)
//                double multiplier = Math.Pow(10, GetHistoryWeekDatabaseBin().VariableModel.GetVariableInfo(aVariableIDX).DecimalPrecision); // TT#707 - JEllis - Container not thread safe (part 2)
//                svd.InitialCalcVariables = true; // TT#555 Total variable is aggregate of other variables
//                for (int i = 0; i < aStoreRIDs.Length; i++)
//                {
//                    storeRID = aStoreRIDs[i];
//                    if (aVariableValues[i] < 0)
//                    {
//                        variableValue =
//                           (Int64)(aVariableValues[i]
//                                   * multiplier
//                                   - .5);   // rounding should use same sign as original value
//                    }
//                    else
//                    {
//                        variableValue =
//                            (Int64)(aVariableValues[i]
//                                    * multiplier
//                                    + .5);
//                    }
//                    svvc.SetStoreVariableValue(storeRID, variableModelType, ref svvc, aVariableIDX, variableValue);
//                }
//                return;
//            }
//            throw new Exception("update failed because Time/Hierarchy Node RID is not locked. TimeID='" + aTimeID.ToString() + "' Hierarchy Node RID='" + aHnRID.ToString() + "'" + " Color Code RID='" + aColorCodeRID.ToString() + "' Size Code RID='" + aSizeCodeRID.ToString() + "'");
//        }
//        #endregion SetStoreVariableValue

//        #region Commit
//        /// <summary>
//        /// Commits updates to the database
//        /// </summary>
//        /// <param name="aStatusMessage">Commit status message (additional information about the Commit)</param>
//        /// <returns>True: Commit was successful; False: Commit failed</returns>
//        public bool Commit(out string aStatusMessage)
//        {
//            if (_updateAccess)
//            {
//                try
//                {
//                    // begin T#555 Total variable is aggregate of other variables
//                    foreach (StoreVariableData<HistoryDatabaseBinKey> svd in _storeVariableDayContainer.Values)
//                    {
//                        svd.ResetCalcVariables();
//                    }
//                    foreach (StoreVariableData<HistoryDatabaseBinKey> svd in _storeVariableWeekContainer.Values)
//                    {
//                        svd.ResetCalcVariables();
//                    }
//                    // end TT#555 Total variable is aggregate of other variables
//                    this.OpenUpdateConnection(eLockType.StoreVariableHistory, this.GetType().Name);
//                    // begin TT#467 Change Store Container Enqueue -- Part 2
//                    //HistoryDayDatabaseBin.WriteStoreVariables(_dba, _storeVariableDayContainer);
//                    //HistoryWeekDatabaseBin.WriteStoreVariables(_dba, _storeVariableWeekContainer);
//                    HistoryDatabaseBinKey[] keysToWrite;
//                    if (_lockDayDictionary.Count > 0)
//                    {
//                        keysToWrite = new HistoryDatabaseBinKey[_lockDayDictionary.Count];
//                        _lockDayDictionary.Keys.CopyTo(keysToWrite,0);
//                        GetHistoryDayDatabaseBin().WriteStoreVariables(_dba, _storeVariableDayContainer, keysToWrite); // TT#707 - JEllis - Container not thread safe (part 2)
//                    }
//                    if (_lockWeekDictionary.Count > 0)
//                    {
//                        keysToWrite = new HistoryDatabaseBinKey[_lockWeekDictionary.Count];
//                        _lockWeekDictionary.Keys.CopyTo(keysToWrite, 0);
//                        GetHistoryWeekDatabaseBin().WriteStoreVariables(_dba, _storeVariableWeekContainer, keysToWrite); // TT#707 - JEllis - Container not thread safe (part 2)
//                    }
//                    // end TT#467 Change Store Container Enqueue -- Part 2
//                    this.CommitData();
//                    foreach (StoreVariableData<HistoryDatabaseBinKey> svd in _storeVariableDayContainer.Values)
//                    {
//                        svd.CommitStatus();
//                    }
//                    foreach (StoreVariableData<HistoryDatabaseBinKey> svd in _storeVariableWeekContainer.Values)
//                    {
//                        svd.CommitStatus();
//                    }
//                    aStatusMessage = "StoreVariableHistory Bin COMMIT was successful";
//                }
//                finally
//                {
//                    if (ConnectionIsOpen)
//                    {
//                        this.CloseUpdateConnection();
//                    }
//                }
//                return true;
//            }
//            aStatusMessage = "StoreVariableHistory Bin COMMIT Failed because StoreVariableHistoryBin was not instantiated for update";
//            return false;
//        }
//        #endregion Commit

//        #region Get StoreVariableVectorContainer
//        /// <summary>
//        /// Gets the StoreVariableVectorContainer for the specified key values
//        /// </summary>
//        /// <param name="aSQL_TimeID">Time ID</param>
//        /// <param name="aHnRID">Hierarchy RID</param>
//        /// <param name="aColorCodeRID">Color Code RID</param>
//        /// <param name="aSizeCodeRID">Size Code RID</param>
//        /// <returns>SizeVariableVectorArray for the specified key values</returns>
//        // begin TT#555 Total variable is aggregate of other variables
//        //private StoreVariableVectorContainer GetStoreVariableVectorContainer(SQL_TimeID aSQL_TimeID, int aHnRID, int aColorCodeRID, int aSizeCodeRID)
//        private StoreVariableData<HistoryDatabaseBinKey> GetStoreVariableData(SQL_TimeID aSQL_TimeID, int aHnRID, int aColorCodeRID, int aSizeCodeRID)
//            // end TT#555 Total variable is aggregate of other variables
//        {
//            // begin TT#555 Total variable is aggregate of other variables
//            //if (_lastDatabaseBinKey == null
//            //    || _lastDatabaseBinKey.TimeID != aSQL_TimeID.SqlTimeID
//            //    || _lastTimeIDType != aSQL_TimeID.TimeIdType
//            //    || _lastDatabaseBinKey.HnRID != aHnRID
//            //    || _lastDatabaseBinKey.ColorCodeRID != aColorCodeRID
//            //    || _lastDatabaseBinKey.SizeCodeRID != aSizeCodeRID)
//            if (_lastStoreVariableData == null
//                || _lastStoreVariableData.DatabaseBinKey.TimeID != aSQL_TimeID.SqlTimeID
//                || _lastTimeIDType != aSQL_TimeID.TimeIdType
//                || _lastStoreVariableData.DatabaseBinKey.HnRID != aHnRID
//                || _lastStoreVariableData.DatabaseBinKey.ColorCodeRID != aColorCodeRID
//                || _lastStoreVariableData.DatabaseBinKey.SizeCodeRID != aSizeCodeRID)
//            {
//                HistoryDatabaseBinKey hdbk = new HistoryDatabaseBinKey((Int16)aSQL_TimeID.SqlTimeID, aHnRID, aColorCodeRID, aSizeCodeRID);
//                //StoreVariableData<HistoryDatabaseBinKey> svd;   // TT#555 Total variable is aggregate of other variables
//                //StoreVariableVectorContainer svvc = null;       // TT#555 Total variable is aggregate of other variables
//                List<StoreVariableData<HistoryDatabaseBinKey>> svdList;

//                if (aSQL_TimeID.TimeIdType == eSQLTimeIdType.TimeIdIsDaily)
//                {
//                    // begin TT#555 Total variable is aggregate of other variables
//                    //if (!_storeVariableDayContainer.TryGetValue(hdbk, out svd))
//                    if (!_storeVariableDayContainer.TryGetValue(hdbk, out _lastStoreVariableData))
//                    {
//                        svdList = GetHistoryDayDatabaseBin().ReadStoreVariables(_dba, hdbk);  // TT#707 - JEllis - Container not thread safe (part 2)
//                        if (svdList.Count > 0)
//                        {
//                            // begin TT#555 Total variable is aggregate of other variables
//                            //svvc = svdList[0].StoreVariableVectorContainer;
//                            //_storeVariableDayContainer.Add(svdList[0].DatabaseBinKey, svdList[0]);
//                            _lastStoreVariableData = svdList[0];
//                            _storeVariableDayContainer.Add(_lastStoreVariableData.DatabaseBinKey, _lastStoreVariableData);
//                            // end TT#555 Total variable is aggregate of other variables
//                        }
//                        else
//                        {
//                            // begin TT#555 Total variable is aggregate of other variables
//                            //svvc = new StoreVariableVectorContainer();
//                            //svd = new StoreVariableData<HistoryDatabaseBinKey>(0, hdbk, svvc);
//                            //_storeVariableDayContainer.Add(hdbk, svd);
//                            StoreVariableVectorContainer svvc = new StoreVariableVectorContainer();
//                            _lastStoreVariableData = new StoreVariableData<HistoryDatabaseBinKey>(0, hdbk, svvc);
//                            _storeVariableDayContainer.Add(hdbk, _lastStoreVariableData);
//                            // end TT#555 Total variable is aggregate of other variables
//                        }
//                    }
//                    // begin TT#555 Total variable is aggregate of other variables
//                    //// begin TT#292 Get Null Reference when posting Size History
//                    //else
//                    //{
//                    //    svvc = svd.StoreVariableVectorContainer;
//                    //}
//                    //// end TT#292 Get Null Reference when posting Size History  
//                    // end TT#555 Total variable is aggregate of other variables
//                }
//                else
//                {
//                    // begin TT#555 Total variable is aggregate of other variables
//                    //if (!_storeVariableWeekContainer.TryGetValue(hdbk, out svd))
//                    if (!_storeVariableWeekContainer.TryGetValue(hdbk, out _lastStoreVariableData))
//                        // end TT#555 Total variable is aggregate of other variables
//                    {
//                        svdList = GetHistoryWeekDatabaseBin().ReadStoreVariables(_dba, hdbk); // TT#707 - JEllis - Container not thread safe (part 2)
//                        if (svdList.Count > 0)
//                        {
//                            // begin TT#555 Total variable is aggregate of other variables
//                            //svvc = svdList[0].StoreVariableVectorContainer;
//                            //_storeVariableWeekContainer.Add(svdList[0].DatabaseBinKey, svdList[0]);
//                            _lastStoreVariableData = svdList[0];
//                            _storeVariableWeekContainer.Add(_lastStoreVariableData.DatabaseBinKey, _lastStoreVariableData);
//                            // end TT#555 Total variable is aggregate of other variables
//                        }
//                        else
//                        {
//                            // begin TT#555 Total variable is aggregate of other variables
//                            //svvc = new StoreVariableVectorContainer();
//                            //svd = new StoreVariableData<HistoryDatabaseBinKey>(0, hdbk, svvc);
//                            //_storeVariableWeekContainer.Add(hdbk, svd);
//                            StoreVariableVectorContainer svvc = new StoreVariableVectorContainer();
//                            _lastStoreVariableData = new StoreVariableData<HistoryDatabaseBinKey>(0, hdbk, svvc);
//                            _storeVariableWeekContainer.Add(hdbk, _lastStoreVariableData);
//                            // end TT#555 Total variable is aggregate of other variables
//                        }
//                    }
//                    // begin TT#555 Total variable is aggregate of other variables
//                    //// begin TT#292 Get Null Reference when posting Size History
//                    //else
//                    //{
//                    //    svvc = svd.StoreVariableVectorContainer;
//                    //}
//                    //// end TT#292 Get Null Reference when posting Size History
//                    // end TT#555 Total variable is aggregate of other variables
//                }
//                // begin TT#555 Total variable is aggregate of other variables
//                //_lastDatabaseBinKey = hdbk;
//                //_lastTimeIDType = aSQL_TimeID.TimeIdType;
//                //_lastStoreVariableVectorContainer = svvc;
//                _lastTimeIDType = aSQL_TimeID.TimeIdType;
//                // end TT#555 Total variable is aggregate of other variables
//            }
//            //return _lastStoreVariableVectorContainer; // TT#555 Total variable is aggregate of other variables
//            return _lastStoreVariableData;              // TT#555 Total variable is aggregate of other variables
//        }
//        #endregion Get StoreVariableVectorContainer


//        #region Get Styles & Colors For Time Range
//        //Begin TT#739-MD -jsobek -Delete Stores
//        //public DataTable GetStylesForTimeRange(List<int> timeId)
//        //{
//        //    string SQLCommand;
//        //    DataTable dtBinStyles;

//        //    try
//        //    {
//        //        //================
//        //        // Get Bin Styles
//        //        //================
//        //        string timeIdList = "("; 
//        //        for (int t=0;t< timeId.Count; t++)
//        //        {
//        //            if (t == (timeId.Count - 1))	// last value in list
//        //            {
//        //                timeIdList = timeIdList + timeId[t] + ")";
//        //            }
//        //            else
//        //            {
//        //                timeIdList = timeIdList + timeId[t] + ",";
//        //            }
//        //        }
//        //        // Begin TT#865 - stodd - null ref on HN_RID = 0
//        //        SQLCommand = "SELECT distinct sd.HN_RID FROM STORE_DAY_HISTORY_BIN sd" +
//        //            " inner join BASE_NODE bn on bn.HN_RID = sd.HN_RID" +  
//        //            " where TIME_ID in " + timeIdList;
//        //        // End TT#865 - stodd - null ref on HN_RID = 0
//        //        dtBinStyles = _dba.ExecuteQuery(SQLCommand);

//        //        return dtBinStyles;
//        //    }
//        //    catch (Exception exc)
//        //    {
//        //        string message = exc.ToString();
//        //        throw;
//        //    }
//        //}
//        //End TT#739-MD -jsobek -Delete Stores

//        //Begin TT#739-MD -jsobek -Delete Stores
//        //public void GetStylesForTimeRange(List<int> timeId, NodeDescendantList nodeDescList, ref DataTable dtStyles)
//        //{
//        //    string SQLCommand;
//        //    DataTable dtBinStyles;

//        //    try
//        //    {
//        //        //================
//        //        // Get Bin Styles
//        //        //================
//        //        string timeIdList = "(";
//        //        for (int t = 0; t < timeId.Count; t++)
//        //        {
//        //            if (t == (timeId.Count - 1))	// last value in list
//        //            {
//        //                timeIdList = timeIdList + timeId[t] + ")";
//        //            }
//        //            else
//        //            {
//        //                timeIdList = timeIdList + timeId[t] + ",";
//        //            }
//        //        }
//        //        // Begin TT#865 - stodd - null ref on HN_RID = 0
//        //        //SQLCommand = "SELECT distinct HN_RID FROM   STORE_DAY_HISTORY_BIN where TIME_ID in " + timeIdList;
//        //        SQLCommand = "SELECT distinct sd.HN_RID FROM STORE_DAY_HISTORY_BIN sd" +
//        //            " inner join BASE_NODE bn on bn.HN_RID = sd.HN_RID" +
//        //            " where TIME_ID in " + timeIdList;
//        //        // End TT#865 - stodd - null ref on HN_RID = 0
//        //        dtBinStyles = _dba.ExecuteQuery(SQLCommand);
//        //        DataColumn[] PrimaryKeyColumn;
//        //        PrimaryKeyColumn = new DataColumn[1];
//        //        PrimaryKeyColumn[0] = dtBinStyles.Columns["HN_RID"];
//        //        dtBinStyles.PrimaryKey = PrimaryKeyColumn;

//        //        //================================================================
//        //        // Intersect the nodes sent in with those found on the bin table
//        //        //================================================================
//        //        foreach (NodeDescendantProfile ndp in nodeDescList.ArrayList)
//        //        {
//        //            DataRow[] rows = dtBinStyles.Select("HN_RID = " + ndp.Key.ToString() );
//        //            if (rows.Length > 0)
//        //            {
//        //                object[] objs = new object[] { ndp.Key.ToString() };
//        //                dtStyles.LoadDataRow(objs, false);
//        //            }
//        //        }
//        //        dtStyles.AcceptChanges();
//        //    }
//        //    catch (Exception exc)
//        //    {
//        //        string message = exc.ToString();
//        //        throw;
//        //    }
//        //}
//        //End TT#739-MD -jsobek -Delete Stores

//        //Begin TT#739-MD -jsobek -Delete Stores
//        //public DataTable GetColorsForTimeRange(List<int> timeId)
//        //{
//        //    string SQLCommand;
//        //    DataTable dtBinColors;

//        //    try
//        //    {
//        //        //================
//        //        // Get Bin Styles
//        //        //================
//        //        string timeIdList = "(";
//        //        for (int t = 0; t < timeId.Count; t++)
//        //        {
//        //            if (t == (timeId.Count - 1))	// last value in list
//        //            {
//        //                timeIdList = timeIdList + timeId[t] + ")";
//        //            }
//        //            else
//        //            {
//        //                timeIdList = timeIdList + timeId[t] + ",";
//        //            }
//        //        }
//        //        SQLCommand = "SELECT distinct HN_RID, COLOR_CODE_RID FROM STORE_DAY_HISTORY_BIN where TIME_ID in " + timeIdList;
//        //        dtBinColors = _dba.ExecuteQuery(SQLCommand);
//        //        DataColumn[] PrimaryKeyColumn;
//        //        PrimaryKeyColumn = new DataColumn[2];
//        //        PrimaryKeyColumn[0] = dtBinColors.Columns["HN_RID"];
//        //        PrimaryKeyColumn[1] = dtBinColors.Columns["COLOR_CODE_RID"];
//        //        dtBinColors.PrimaryKey = PrimaryKeyColumn;

//        //        return dtBinColors;
//        //    }
//        //    catch (Exception exc)
//        //    {
//        //        string message = exc.ToString();
//        //        throw;
//        //    }
//        //}
//        //End TT#739-MD -jsobek -Delete Stores
//        #endregion Get StylesForTimeRange

//        // Begin TT#634 - JSmith - Color rename
//        //Begin TT#739-MD -jsobek -Delete Stores
//        ///// <summary>
//        ///// Update the color code RID on the daily history bin table
//        ///// </summary>
//        ///// <returns>boolean; true if successful, false if failed</returns>
//        //public void UpdateStyleColorOnDayBin(int aStyleRID, int aColorCodeRID, int aNewColorCodeRID)
//        //{
//        //    try
//        //    {
//        //        string SQLCommand = "UPDATE STORE_DAY_HISTORY_BIN with (rowlock) SET "
//        //            + " COLOR_CODE_RID = @newcolorcoderid"
//        //            + " WHERE HN_RID = @hnrid"
//        //            + "   AND COLOR_CODE_RID = @colorcoderid";
//        //        MIDDbParameter[] InParams = { new MIDDbParameter("@hnrid", aStyleRID, eDbType.Int),
//        //                                      new MIDDbParameter("@colorcoderid", aColorCodeRID, eDbType.Int),
//        //                                      new MIDDbParameter("@newcolorcoderid", aNewColorCodeRID, eDbType.Int)};

//        //        _dba.ExecuteNonQuery(SQLCommand, InParams);
//        //        return;
//        //    }
//        //    catch (Exception err)
//        //    {
//        //        string message = err.ToString();
//        //        throw;
//        //    }
//        //}
//        //End TT#739-MD -jsobek -Delete Stores

//        //Begin TT#739-MD -jsobek -Delete Stores
//        ///// <summary>
//        ///// Update the color code RID on the daily history bin table
//        ///// </summary>
//        ///// <returns>boolean; true if successful, false if failed</returns>
//        //public void UpdateStyleColorOnWeekBin(int aStyleRID, int aColorCodeRID, int aNewColorCodeRID)
//        //{
//        //    try
//        //    {
//        //        string SQLCommand = "UPDATE STORE_WEEK_HISTORY_BIN with (rowlock) SET "
//        //            + " COLOR_CODE_RID = @newcolorcoderid"
//        //            + " WHERE HN_RID = @hnrid"
//        //            + "   AND COLOR_CODE_RID = @colorcoderid";
//        //        MIDDbParameter[] InParams = { new MIDDbParameter("@hnrid", aStyleRID, eDbType.Int),
//        //                                      new MIDDbParameter("@colorcoderid", aColorCodeRID, eDbType.Int),
//        //                                      new MIDDbParameter("@newcolorcoderid", aNewColorCodeRID, eDbType.Int)};

//        //        _dba.ExecuteNonQuery(SQLCommand, InParams);
//        //        return;
//        //    }
//        //    catch (Exception err)
//        //    {
//        //        string message = err.ToString();
//        //        throw;
//        //    }
//        //}
//        //End TT#739-MD -jsobek -Delete Stores
//        // End TT#634
//        private HistoryDayDatabaseBin GetHistoryDayDatabaseBin()
//        {
//            if (_historyDayDatabaseBin == null)
//            {
//                _historyDayDatabaseBin = new HistoryDayDatabaseBin();
//            }
//            return _historyDayDatabaseBin;
//        }
//        private HistoryWeekDatabaseBin GetHistoryWeekDatabaseBin()
//        {
//            if (_historyWeekDatabaseBin == null)
//            {
//                _historyWeekDatabaseBin = new HistoryWeekDatabaseBin();
//            }
//            return _historyWeekDatabaseBin;
//        }

//        #region Dispose
//        /// <summary>
//        /// Dispose of object
//        /// </summary>
//        public void Dispose()
//        {
//            Dispose(true);
//            System.GC.SuppressFinalize(this);
//        }
//        /// <summary>
//        /// Dispose of object
//        /// </summary>
//        /// <param name="disposing">
//        /// True:  dispose all managed and unmanaged resources.
//        /// False: dispose only unmanaged resources
//        /// </param>
//        protected void Dispose(bool disposing)
//        {
//            if (!_isDisposed)
//            {
//                if (disposing)
//                {
//                    if (_updateAccess)
//                    {
//                        RemoveAllLocks();
//                        if (ConnectionIsOpen)
//                        {
//                            CloseUpdateConnection();
//                        }
//                    }
//                    FlushAll(); 
//                    this._lockConflictList = null;
//                    this._lockDayDictionary = null;
//                    this._lockWeekDictionary = null;
//                    this._storeVariableDayContainer = null;
//                    this._storeVariableWeekContainer = null;
//                    this._midEnqueue = null;
//                }
//                _isDisposed = true;
//            }
//        }
//        ~StoreVariableHistoryBin()
//        {
//            Dispose(false);
//        }
//        #endregion Dispose

//        #endregion Methods
//    }
//}
