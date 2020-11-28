//using System;
//using System.Collections;
//using System.Configuration;
//using System.Data;
//using System.Diagnostics;
//using System.Globalization;
////Begin TT#708 - JScott - Services need a Retry availalbe.
//using System.Threading;
////End TT#708 - JScott - Services need a Retry availalbe.

//using MIDRetail.Data;
//using MIDRetail.Common;
//using MIDRetail.DataCommon;

//namespace MIDRetail.Business
//{
//    public class StoreServerGlobalChar : Global
//    {
//        //=======
//        // FIELDS
//        //=======
//        //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

//        static private ArrayList _loadLock;
//        static private bool _loaded;
//        static private Audit _audit;

//        //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
//        static private DataTable _dtStoreGroup;
//        static private DataTable _dtStoreGroupLevel;
//        //static private DataTable _dtStoreGroupLevelStatement;
//        //static private DataTable _dtStoreGroupLevelJoin;

//        static private MIDReaderWriterLock stores_rwl = new MIDReaderWriterLock();
//        static private MIDReaderWriterLock storeGroup_rwl = new MIDReaderWriterLock();
//        static private MIDReaderWriterLock storeCharacteristics_rwl = new MIDReaderWriterLock();
//        static private MIDReaderWriterLock calendar_rwl = new MIDReaderWriterLock();

 
//        static private StoreCharacteristics _characteristics;

//        static private DateTime _createdDate;
//        static private DateTime _lastRefreshDate;
//        static private int _numStoreProfileColumns = 0;
//        static private DataSet _dsAllStores;
//        static private ProfileList _allStoreList;
//        static private Hashtable _allStoreCharacteristicsHash;
//        //static private bool _postingDateSet = false;
//        static private eStoreDisplayOptions _globalStoreDisplayOption;
//        static private int _noncompStorePeriodEnd;
//        static private int _noncompStorePeriodBegin;
//        static private int _newStorePeriodEnd;
//        static private int _newStorePeriodBegin;
//        static private string _module = "StoreServerGlobal";
//        static private string _storeStatusText;
//        static private string _hiddenColumnPrefix = "SCGRID__";

//        //=============
//        // CONSTRUCTORS
//        //=============

//        /// <summary>
//        /// Creates a new instance of StoreServerGlobal
//        /// </summary>

//        static StoreServerGlobalChar()
//        {
//            try
//            {
//                //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
//                _loadLock = new ArrayList();
//                _loaded = false;

//                //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
//                if (!EventLog.SourceExists("MIDStoreService"))
//                {
//                    EventLog.CreateEventSource("MIDStoreService", null);
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService: StoreServerGlobal encountered error - " + ex.Message);
//            }
//        }

//        //===========
//        // PROPERTIES
//        //===========
//        #region Global Properties
//        //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
//        static private Audit Audit
//        {
//            get
//            {
//                return _audit;
//            }
//        }

//        static public bool Loaded
//        {
//            get
//            {
//                return _loaded;
//            }
//        }

//        //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
   

//        /// <summary>
//        /// A flag that identifies if the posting date has been set in the global calendar.
//        /// </summary>
//        //static internal bool PostingDateSet
//        //{
//        //    get	{ return _postingDateSet; }
//        //}


//        static internal DataTable StoreData
//        {
//            get
//            {
//                return GetStoreDataTable();
//            }
//        }

//        //static internal DataTable StoreGroupData
//        //{
//        //    get	
//        //    { 
//        //        DataTable dt = MIDEnvironment.CreateDataTable("STORE_GROUP");
//        //        dt = _dtStoreGroup.Copy();
//        //        return dt;
//        //    }
//        //}

//        //static internal DataTable StoreGroupLevelData
//        //{
//        //    get	
//        //    { 
//        //        DataTable dt = MIDEnvironment.CreateDataTable("STORE_GROUP_LEVELS");
//        //        dt = _dtStoreGroupLevel.Copy();
//        //        return dt;
//        //    }
//        //}

//        static internal ArrayList CharacteristicGroupList
//        {
//            get
//            {
//                return GetCharacteristicGroupList();
//            }
//        }

//        static internal string StoreStatusText
//        {
//            get
//            {
//                if (_storeStatusText == null)
//                    _storeStatusText = MIDText.GetTextOnly(eMIDTextCode.lbl_StoreStatus);
//                return _storeStatusText;
//            }
//        }

//        static internal string HiddenColumnPrefix
//        {
//            get
//            {
//                return _hiddenColumnPrefix;
//            }
//        }
//        #endregion

//        //========
//        // METHODS
//        //========
//        // Begin TT#1243 - JSmith - Audit Performance
//        /// <summary>
//        /// Clean up the global resources
//        /// </summary>
//        static public void CloseAudit()
//        {
//            try
//            {
//                // Begin TT#1303 - stodd - null ref
//                if (Audit != null)
//                {
//                    Audit.CloseUpdateConnection();
//                }
//                // End TT#1303 - stodd - null ref
//            }
//            catch
//            {
//                throw;
//            }
//        }
//        // End TT#1243

//        static public void BuildStoreServerGlobalArea()
//        {
//            try
//            {
//                AcquireCompleteWriterLock();
//                try
//                {
//                    //_go = new GlobalOptions();
//                    _storeData = new StoreData();
//                    _dtAllStores = _storeData.StoreProfile_Read();

//                    BuildStoreGroupData();
//                    BuildStoreGroupLevelData();
//                    //BuildStoreGroupLevelStatementData();
//                    //BuildStoreGroupLevelJoinData();

//                    _createdDate = new DateTime();

//                    //****************************
//                    // Store Characteristics
//                    //****************************
//                    _characteristics = new StoreCharacteristics(_storeData);

//                    //************************
//                    // Populate stores
//                    //************************
//                    PopulateStores();
//                    PopulateAllStoreList();
//                    CalculateStatusForStores();
//                    _allStoreList.ArrayList.Sort(new StoreTextSort());

//                    //====================================================
//                    // store group relationships
//                    //====================================================
//                    _storeGroupRelations = new StoreGroupRelations(_storeData, _dtAllStores, _dtStoreGroup,
//                        _dtStoreGroupLevel, _characteristics,
//                        _allStoreList, _allStoreCharacteristicsHash, Audit, Calendar);

//                    // BEGIN TT#1598 - stodd - store perf
//                    _storeGroupRelations.IsStartUp = true;
//                    _storeGroupRelations.FillGroupsWithStores();
//                    _storeGroupRelations.IsStartUp = false;
//                    // END TT#1598 - stodd - store perf

//                    // BEGIN TT#739-MD - STodd - delete stores
//                    // This function is being moved to purge.
//                    //bool skipGroupLevelCleanUp = Include.ConvertBoolConfigValue(MIDConfigurationManager.AppSettings["SkipGroupLevelCleanUp"]);
//                    //if (!skipGroupLevelCleanUp)
//                    //{
//                    //    _storeGroupRelations.DeleteUnusedGroupLevels();
//                    //}
//                    // END TT#739-MD - STodd - delete stores

//                    //Begin  TT#2279 - RBeck - Remove call to CheckForDuplicateGroupLevelNames
//                    //===================================================================
//                    // TEMPORARY
//                    // this is to check for duplicate group level names within a group
//                    //===================================================================
//                    ArrayList dupList = StoreServerGlobal.CheckForDuplicateGroupLevelNames();
//                    if (dupList.Count > 0)
//                    {
//                        string nameList = string.Empty;
//                        foreach (string aGroupName in dupList)
//                        {
//                            nameList += aGroupName + "...";
//                        }
//                        string errMsg = "The following Attribute(s) contain duplicate Attribute Set names-- " + nameList;

//                        EventLog.WriteEntry("MIDStoreService", errMsg + "Duplicate Attribute Set Names Found", EventLogEntryType.Warning);

//                        if (Audit != null)
//                        {
//                            Audit.Add_Msg(eMIDMessageLevel.Warning, errMsg + "Duplicate Attribute Set Names Found", System.Reflection.MethodBase.GetCurrentMethod().Name);
//                        }
//                    }
//                    //End    TT#2279 - RBeck - Remove call to CheckForDuplicateGroupLevelNames

//                }
//                finally
//                {
//                    ReleaseCompleteWriterLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex);
//                }
//                throw;
//            }
//        }

//        // Begin Track #4872 - JSmith - Global/User Attributes
//        static public void BuildStoreGroup(int aGroupRID)
//        {
//            DataTable dtStoreGroup;
//            DataTable dtStoreGroupLevel;
//            //DataTable dtStoreGroupLevelStatement;
//            //DataTable dtStoreGroupLevelJoin;
//            //DataColumn[] PrimaryKeyColumn;

//            try
//            {
//                //BuildStoreGroupData();
//                dtStoreGroup = _storeData.StoreGroup_Read(aGroupRID, eDataOrderBy.RID);
//                dtStoreGroup.PrimaryKey = new DataColumn[] { dtStoreGroup.Columns["SG_RID"] };

//                //BuildStoreGroupLevelData();
//                dtStoreGroupLevel = _storeData.StoreGroupLevelsForGroup_Read(aGroupRID);
//                dtStoreGroupLevel.PrimaryKey = new DataColumn[] { dtStoreGroupLevel.Columns["SGL_RID"] };

//                //BuildStoreGroupLevelStatementData();
//                //dtStoreGroupLevelStatement = _storeData.StoreGroupLevelStatementForGroup_Read(aGroupRID);
//                //make Group Level RID column the primary key
//                //PrimaryKeyColumn = new DataColumn[2];
//                //PrimaryKeyColumn[0] = dtStoreGroupLevelStatement.Columns["SGL_RID"];
//                //PrimaryKeyColumn[1] = dtStoreGroupLevelStatement.Columns["SGLS_STATEMENT_SEQ"];
//                //dtStoreGroupLevelStatement.PrimaryKey = PrimaryKeyColumn;

//                //BuildStoreGroupLevelJoinData();
//                //dtStoreGroupLevelJoin = _storeData.StoreGroupLevelJoinForGroup_Read(aGroupRID);
//                //make Group Level RID column the primary key
//                //PrimaryKeyColumn = new DataColumn[2];
//                //PrimaryKeyColumn[0] = dtStoreGroupLevelJoin.Columns["SGL_RID"];
//                //PrimaryKeyColumn[1] = dtStoreGroupLevelJoin.Columns["ST_RID"];
//                //dtStoreGroupLevelJoin.PrimaryKey = PrimaryKeyColumn;

//                AcquireStoreGroupWriterLock();
//                try
//                {
//                    _storeGroupRelations.BuildStoreGroup(dtStoreGroup, dtStoreGroupLevel);

//                    //_storeGroupRelations.FillGroupsWithStores();
//                    //_storeGroupRelations.DeleteUnusedGroupLevels();
//                }
//                finally
//                {
//                    ReleaseStoreGroupWriterLock();
//                }
//            }
//            catch (ApplicationException)
//            {
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:StoreServerGlobal:BuildStoreGroup writer lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:StoreServerGlobal:BuildStoreGroup writer lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex);
//                }
//                throw;
//            }
//        }
//        // End Track #4872

//        static public void SetPostingDate(DateTime postingDate)
//        {
//            try
//            {
//                if (Calendar.PostDate == null ||
//                    postingDate.Date != Calendar.PostDate.Date.Date)
//                {

//                    calendar_rwl.AcquireWriterLock(WriterLockTimeOut);
//                    try
//                    {
//                        Calendar.SetPostingDate(postingDate);
//                    }
//                    finally
//                    {
//                        calendar_rwl.ReleaseWriterLock();
//                    }

//                    CalculateStatusForStores();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.Message, EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex);
//                }
//                throw;
//            }
//        }

//        static private DataTable GetStoreDataTable()
//        {
//            try
//            {
//                stores_rwl.AcquireReaderLock(ReaderLockTimeOut);
//                try
//                {
//                    DataTable dt = MIDEnvironment.CreateDataTable("STORES");
//                    dt = _dtAllStores.Copy();
//                    return dt;
//                }
//                finally
//                {
//                    stores_rwl.ReleaseReaderLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex);
//                }
//                throw;
//            }
//        }

//        static private void BuildStoreGroupData()
//        {
//            try
//            {
//                storeGroup_rwl.AcquireWriterLock(WriterLockTimeOut);
//                try
//                {
//                    _dtStoreGroup = _storeData.StoreGroup_Read(eDataOrderBy.RID);
//                    _dtStoreGroup.PrimaryKey = new DataColumn[] { _dtStoreGroup.Columns["SG_RID"] };
//                }
//                finally
//                {
//                    // Ensure that the lock is released.
//                    storeGroup_rwl.ReleaseWriterLock();
//                }
//            }
//            catch (ApplicationException)
//            {
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:StoreServerGlobal writer lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:StoreServerGlobal writer lock has timed out");
//            }
//        }

//        static private void BuildStoreGroupLevelData()
//        {
//            try
//            {
//                storeGroup_rwl.AcquireWriterLock(WriterLockTimeOut);
//                try
//                {
//                    _dtStoreGroupLevel = _storeData.StoreGroupLevel_Read();
//                    _dtStoreGroupLevel.PrimaryKey = new DataColumn[] { _dtStoreGroupLevel.Columns["SGL_RID"] };
//                }
//                finally
//                {
//                    // Ensure that the lock is released.
//                    storeGroup_rwl.ReleaseWriterLock();
//                }
//            }
//            catch (ApplicationException)
//            {
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:StoreServerGlobal writer lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:StoreServerGlobal writer lock has timed out");
//            }
//        }

//        //static private void BuildStoreGroupLevelStatementData()
//        //{
//        //    try
//        //    {
//        //        storeGroup_rwl.AcquireWriterLock(WriterLockTimeOut);
//        //        try
//        //        {
//        //            _dtStoreGroupLevelStatement = _storeData.StoreGroupLevelStatement_Read();
//        //            //make Group Level RID column the primary key
//        //            DataColumn[] PrimaryKeyColumn = new DataColumn[2];
//        //            PrimaryKeyColumn[0] = _dtStoreGroupLevelStatement.Columns["SGL_RID"];
//        //            PrimaryKeyColumn[1] = _dtStoreGroupLevelStatement.Columns["SGLS_STATEMENT_SEQ"];
//        //            _dtStoreGroupLevelStatement.PrimaryKey = PrimaryKeyColumn;
//        //        }
//        //        finally
//        //        {
//        //            // Ensure that the lock is released.
//        //            storeGroup_rwl.ReleaseWriterLock();
//        //        }
//        //    }
//        //    catch (ApplicationException)
//        //    {
//        //        // The writer lock request timed out.
//        //        EventLog.WriteEntry("MIDStoreService", "MIDStoreService:StoreServerGlobal writer lock has timed out", EventLogEntryType.Error);
//        //        throw new MIDException (eErrorLevel.severe,	0, "MIDStoreService:StoreServerGlobal writer lock has timed out");
//        //    }
//        //}

//        //static private void BuildStoreGroupLevelJoinData()
//        //{
//        //    try
//        //    {
//        //        storeGroup_rwl.AcquireWriterLock(WriterLockTimeOut);
//        //        try
//        //        {
//        //            _dtStoreGroupLevelJoin = _storeData.StoreGroupLevelJoin_Read();
//        //            //make Group Level RID column the primary key
//        //            DataColumn[] PrimaryKeyColumn = new DataColumn[2];
//        //            PrimaryKeyColumn[0] = _dtStoreGroupLevelJoin.Columns["SGL_RID"];
//        //            PrimaryKeyColumn[1] = _dtStoreGroupLevelJoin.Columns["ST_RID"];
//        //            _dtStoreGroupLevelJoin.PrimaryKey = PrimaryKeyColumn;
//        //        }
//        //        finally
//        //        {
//        //            // Ensure that the lock is released.
//        //            storeGroup_rwl.ReleaseWriterLock();
//        //        }
//        //    }
//        //    catch (ApplicationException)
//        //    {
//        //        // The writer lock request timed out.
//        //        EventLog.WriteEntry("MIDStoreService", "MIDStoreService:StoreServerGlobal writer lock has timed out", EventLogEntryType.Error);
//        //        throw new MIDException (eErrorLevel.severe,	0, "MIDStoreService:StoreServerGlobal writer lock has timed out");
//        //    }
//        //}

//        static private void PopulateStores()
//        {
//            try
//            {
//                GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
//                gop.LoadOptions();
//                _globalStoreDisplayOption = gop.StoreDisplay;
//                _noncompStorePeriodEnd = gop.NonCompStorePeriodEnd;
//                _noncompStorePeriodBegin = gop.NonCompStorePeriodBegin;
//                _newStorePeriodEnd = gop.NewStorePeriodEnd;
//                _newStorePeriodBegin = gop.NewStorePeriodBegin;

//                _dsAllStores = MIDEnvironment.CreateDataSet("Stores");
//                _dsAllStores.Tables.Add(_dtAllStores);
//                _numStoreProfileColumns = NumberOfStoreProfileColumns;

//                //make Store RID column the primary key
//                DataColumn[] PrimaryKeyColumn = new DataColumn[1];
//                PrimaryKeyColumn[0] = _dtAllStores.Columns["ST_RID"];
//                _dtAllStores.PrimaryKey = PrimaryKeyColumn;

//                // for insert purposes
//                _dtAllStores.Columns["ST_RID"].AllowDBNull = true;
//                _dtAllStores.Columns["ACTIVE_IND"].DefaultValue = "1";
//                _dtAllStores.Columns["SHIP_ON_MONDAY"].DefaultValue = "0";
//                _dtAllStores.Columns["SHIP_ON_TUESDAY"].DefaultValue = "0";
//                _dtAllStores.Columns["SHIP_ON_WEDNESDAY"].DefaultValue = "0";
//                _dtAllStores.Columns["SHIP_ON_THURSDAY"].DefaultValue = "0";
//                _dtAllStores.Columns["SHIP_ON_FRIDAY"].DefaultValue = "0";
//                _dtAllStores.Columns["SHIP_ON_SATURDAY"].DefaultValue = "0";
//                _dtAllStores.Columns["SHIP_ON_SUNDAY"].DefaultValue = "0";
//                _dtAllStores.Columns["SIMILAR_STORE_MODEL"].DefaultValue = "0";  // Issue 3557 stodd

//                AddCharacteristicColumns();
//                PopulateCharacteristicColumnValues();
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex);
//                }
//                throw;
//            }
//        }

//        static private void PopulateAllStoreList()
//        {
//            try
//            {
//                if (_allStoreList == null)
//                    _allStoreList = new ProfileList(eProfileType.Store);
//                else
//                    _allStoreList.Clear();

//                if (_allStoreCharacteristicsHash == null)
//                    _allStoreCharacteristicsHash = new Hashtable();
//                else
//                    _allStoreCharacteristicsHash.Clear();

//                try
//                {
//                    foreach (DataRow storeDataRow in _dtAllStores.Rows)
//                    {
//                        // unload dataTable row to a StoreProfile
//                        StoreProfile currStore = ConvertToStoreProfile(storeDataRow, false);
//                        ArrayList charList = ConvertToCharacteristicList(storeDataRow);
//                        _allStoreCharacteristicsHash.Add(currStore.Key, charList);
//                        currStore.Characteristics = null;
//                        _allStoreList.Add(currStore);
//                    }
//                }
//                catch (Exception)
//                {
//                    throw;
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex);
//                }
//                throw;
//            }
//        }

//        static private StoreProfile ConvertToStoreProfile(DataRow dr, bool dynamicStore)
//        {
//            try
//            {
//                int key = -1;

//                if (dr["ST_RID"] != DBNull.Value)
//                    key = Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture);

//                StoreProfile sp = new StoreProfile(key);

//                if (dr["ST_ID"] != DBNull.Value)
//                    sp.StoreId = (string)dr["ST_ID"];
//                if (dr["STORE_NAME"] != DBNull.Value)
//                    sp.StoreName = (string)dr["STORE_NAME"];
//                if (dr["STORE_DESC"] != DBNull.Value)
//                    sp.StoreDescription = (string)dr["STORE_DESC"];
//                if (dr["ACTIVE_IND"] != DBNull.Value)
//                    sp.ActiveInd = ((string)dr["ACTIVE_IND"] == "1") ? true : false;
//                if (dr["CITY"] != DBNull.Value)
//                    sp.City = (string)dr["CITY"];
//                if (dr["STATE"] != DBNull.Value)
//                    sp.State = (string)dr["STATE"];
//                if (dr["SELLING_SQ_FT"] != DBNull.Value)
//                    sp.SellingSqFt = Convert.ToInt32(dr["SELLING_SQ_FT"], CultureInfo.CurrentUICulture);

//                if (dr["SELLING_OPEN_DATE"] == DBNull.Value)
//                    sp.SellingOpenDt = sp.SellingOpenDt;
//                else
//                    sp.SellingOpenDt = (DateTime)dr["SELLING_OPEN_DATE"];
//                if (dr["SELLING_CLOSE_DATE"] == DBNull.Value)
//                    sp.SellingCloseDt = sp.SellingCloseDt;
//                else
//                    sp.SellingCloseDt = (DateTime)dr["SELLING_CLOSE_DATE"];

//                if (dr["STOCK_OPEN_DATE"] == DBNull.Value)
//                    sp.StockOpenDt = sp.StockOpenDt;
//                else
//                    sp.StockOpenDt = (DateTime)dr["STOCK_OPEN_DATE"];
//                if (dr["STOCK_CLOSE_DATE"] == DBNull.Value)
//                    sp.StockCloseDt = sp.StockCloseDt;
//                else
//                    sp.StockCloseDt = (DateTime)dr["STOCK_CLOSE_DATE"];

//                if (dr["LEAD_TIME"] != DBNull.Value)
//                    sp.LeadTime = Convert.ToInt32(dr["LEAD_TIME"], CultureInfo.CurrentUICulture);

//                if (dr["SHIP_ON_MONDAY"] != DBNull.Value)
//                    sp.ShipOnMonday = ((string)dr["SHIP_ON_MONDAY"] == "1") ? true : false;
//                if (dr["SHIP_ON_TUESDAY"] != DBNull.Value)
//                    sp.ShipOnTuesday = ((string)dr["SHIP_ON_TUESDAY"] == "1") ? true : false;
//                if (dr["SHIP_ON_WEDNESDAY"] != DBNull.Value)
//                    sp.ShipOnWednesday = ((string)dr["SHIP_ON_WEDNESDAY"] == "1") ? true : false;
//                if (dr["SHIP_ON_THURSDAY"] != DBNull.Value)
//                    sp.ShipOnThursday = ((string)dr["SHIP_ON_THURSDAY"] == "1") ? true : false;
//                if (dr["SHIP_ON_FRIDAY"] != DBNull.Value)
//                    sp.ShipOnFriday = ((string)dr["SHIP_ON_FRIDAY"] == "1") ? true : false;
//                if (dr["SHIP_ON_SATURDAY"] != DBNull.Value)
//                    sp.ShipOnSaturday = ((string)dr["SHIP_ON_SATURDAY"] == "1") ? true : false;
//                if (dr["SHIP_ON_SUNDAY"] != DBNull.Value)
//                    sp.ShipOnSunday = ((string)dr["SHIP_ON_SUNDAY"] == "1") ? true : false;
//                sp.DynamicStore = dynamicStore;

//                // Begin Issue 3557 stodd
//                if (dr["SIMILAR_STORE_MODEL"] != DBNull.Value)
//                    sp.SimilarStoreModel = ((string)dr["SIMILAR_STORE_MODEL"] == "1") ? true : false;
//                // End Issue 3557 stodd
//                // BEGIN TT#1401 - stodd - add resevation stores (IMO)
//                if (dr["IMO_ID"] != DBNull.Value)
//                    sp.IMO_ID = dr["IMO_ID"].ToString();
//                // END TT#1401 - stodd - add resevation stores (IMO)
//                // BEGIN TT#739-MD - STodd - delete stores
//                if (dr["STORE_DELETE_IND"] != DBNull.Value)
//                    sp.DeleteStore = ((string)dr["STORE_DELETE_IND"] == "1") ? true : false;
//                // END TT#739-MD - STodd - delete stores

//                // Get status
//                // BEGIN TT#190 - MD - stodd - store service looping
//                //WeekProfile currentWeek = Calendar.GetWeek(Calendar.PostDate.Date);
//                WeekProfile currentWeek = Calendar.CurrentWeek;
//                // END TT#190 - MD - stodd - store service looping
//                sp.Status = GetStoreStatus(currentWeek, sp.SellingOpenDt, sp.SellingCloseDt);
//                sp.StockStatus = GetStoreStatus(currentWeek, sp.StockOpenDt, sp.StockCloseDt);

//                sp.Text = Include.GetStoreDisplay(_globalStoreDisplayOption, sp.StoreId, sp.StoreName, sp.StoreDescription);

//                // REMOVED ========================================================
//                // Characteristics have been moved to _allStoreCharacteristicsHash
//                //=================================================================
//                //			sp.Characteristics = new ArrayList();
//                //			int maxCols = _dtAllStores.Columns.Count;
//                //			int startCol = StoreServerGlobal.NumberOfStoreProfileColumns;
//                //			for (int col=startCol;col<maxCols;col++)
//                //			{
//                //				StoreCharGroupProfile ch = new StoreCharGroupProfile(-1);
//                //				ch.Name = _dtAllStores.Columns[col].Caption;
//                //				ch.Key = _characteristics.GetCharacteristicGroupRID(ch.Name);
//                //				ch.CharacteristicValue.CharValue = dr[col];
//                //				ch.CharacteristicValue.SC_RID = _characteristics.CharacteristicExists(ch.Name, ch.CharacteristicValue.CharValue);
//                //						
//                //				sp.Characteristics.Add( ch );
//                //			}

//                return sp;
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex);
//                }
//                throw;
//            }
//        }

//        static private ArrayList ConvertToCharacteristicList(DataRow dr)
//        {
//            try
//            {
//                ArrayList charList = new ArrayList();

//                int maxCols = _dtAllStores.Columns.Count;
//                int startCol = StoreServerGlobal.NumberOfStoreProfileColumns;
//                for (int col = startCol; col < maxCols; col++)
//                {
//                    string caption = _dtAllStores.Columns[col].Caption;

//                    if (!caption.StartsWith(_hiddenColumnPrefix))
//                    {
//                        StoreCharGroupProfile ch = new StoreCharGroupProfile(-1);
//                        ch.Name = _dtAllStores.Columns[col].Caption;
//                        ch.Key = _characteristics.GetCharacteristicGroupRID(ch.Name);
//                        ch.CharacteristicValue.CharValue = dr[col];
//                        ch.CharacteristicValue.SC_RID = _characteristics.CharacteristicExists(ch.Name, ch.CharacteristicValue.CharValue);

//                        charList.Add(ch);
//                    }
//                }

//                return charList;
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex);
//                }
//                throw;
//            }
//        }

//        static public void CalculateStatusForStores()
//        {
//            try
//            {
//                stores_rwl.AcquireWriterLock(WriterLockTimeOut);

//                try
//                {
//                    GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
//                    gop.LoadOptions();
//                    _globalStoreDisplayOption = gop.StoreDisplay;
//                    _noncompStorePeriodEnd = gop.NonCompStorePeriodEnd;
//                    _noncompStorePeriodBegin = gop.NonCompStorePeriodBegin;
//                    _newStorePeriodEnd = gop.NewStorePeriodEnd;
//                    _newStorePeriodBegin = gop.NewStorePeriodBegin;

//                    // recalc store comp/non-comp values
//                    DateTime sellingOpenDt;
//                    DateTime sellingCloseDt;
//                    DateTime stockOpenDt;
//                    DateTime stockCloseDt;
//                    int storeRID = 0;

//                    //**************************************
//                    // insert new COMP value for each store
//                    //**************************************
//                    foreach (DataRow dr in _dtAllStores.Rows)
//                    {
//                        storeRID = Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture);
//                        StoreProfile sp = (StoreProfile)_allStoreList.FindKey(storeRID);
//                        ArrayList storeCharList = (ArrayList)_allStoreCharacteristicsHash[sp.Key];

//                        if (dr["SELLING_OPEN_DATE"] == DBNull.Value)
//                        {
//                            sellingOpenDt = Include.UndefinedDate;
//                        }
//                        else
//                        {
//                            sellingOpenDt = (DateTime)dr["SELLING_OPEN_DATE"];
//                        }
//                        if (dr["SELLING_CLOSE_DATE"] == DBNull.Value)
//                        {
//                            sellingCloseDt = Include.UndefinedDate;
//                        }
//                        else
//                        {
//                            sellingCloseDt = (DateTime)dr["SELLING_CLOSE_DATE"];
//                        }

//                        if (dr["STOCK_OPEN_DATE"] == DBNull.Value)
//                        {
//                            stockOpenDt = Include.UndefinedDate;
//                        }
//                        else
//                        {
//                            stockOpenDt = (DateTime)dr["STOCK_OPEN_DATE"];
//                        }
//                        if (dr["STOCK_CLOSE_DATE"] == DBNull.Value)
//                        {
//                            stockCloseDt = Include.UndefinedDate;
//                        }
//                        else
//                        {
//                            stockCloseDt = (DateTime)dr["STOCK_CLOSE_DATE"];
//                        }

//                        bool storeDone = false;
//                        // BEGIN TT#190 - MD - stodd - store service looping
//                        //WeekProfile currentWeek = Calendar.GetWeek(Calendar.PostDate.Date);
//                        WeekProfile currentWeek = Calendar.CurrentWeek;
//                        // END TT#190 - MD - stodd - store service looping
//                        if (!storeDone)
//                        {
//                            eStoreStatus storeStatus = GetStoreStatus(currentWeek, sellingOpenDt, sellingCloseDt);
//                            sp.Status = storeStatus;
//                            sp.StockStatus = GetStoreStatus(currentWeek, stockOpenDt, stockCloseDt);
//                            DataRow storeRow;
//                            switch (storeStatus)
//                            {
//                                case eStoreStatus.Closed:
//                                    // update Data Table
//                                    storeRow = _dtAllStores.Rows.Find(storeRID);
//                                    if (storeRow != null)
//                                    {
//                                        storeRow["Store Status"] = eStoreStatus.Closed;
//                                    }
//                                    break;
//                                case eStoreStatus.Preopen:
//                                    storeRow = _dtAllStores.Rows.Find(storeRID);
//                                    if (storeRow != null)
//                                    {
//                                        storeRow["Store Status"] = eStoreStatus.Preopen;
//                                    }
//                                    break;
//                                case eStoreStatus.New:
//                                    storeRow = _dtAllStores.Rows.Find(storeRID);
//                                    if (storeRow != null)
//                                    {
//                                        storeRow["Store Status"] = eStoreStatus.New;
//                                    }
//                                    break;
//                                case eStoreStatus.NonComp:
//                                    storeRow = _dtAllStores.Rows.Find(storeRID);
//                                    if (storeRow != null)
//                                    {
//                                        storeRow["Store Status"] = eStoreStatus.NonComp;
//                                    }
//                                    break;
//                                case eStoreStatus.Comp:
//                                    storeRow = _dtAllStores.Rows.Find(storeRID);
//                                    if (storeRow != null)
//                                    {
//                                        storeRow["Store Status"] = eStoreStatus.Comp;
//                                    }
//                                    break;
//                            }
//                        }
//                    }
//                }
//                finally
//                {
//                    _dtAllStores.AcceptChanges();
//                    stores_rwl.ReleaseWriterLock();
//                }
//            }
//            catch (ApplicationException)
//            {
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:StoreServerGlobal writer lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:StoreServerGlobal writer lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex);
//                }
//                throw;
//            }
//        }

//        //static public eStoreStatus GetStoreStatus(WeekProfile baseWeek, int storeRID)
//        //{
//        //    try
//        //    {
//        //        StoreProfile sp = (StoreProfile)_allStoreList.FindKey(storeRID);
//        //        return GetStoreStatus(baseWeek, sp.SellingOpenDt, sp.SellingCloseDt);
//        //    }
//        //    catch ( Exception ex)
//        //    {
//        //        EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//        //        if (Audit != null)
//        //        {
//        //            Audit.Log_Exception(ex);
//        //        }
//        //        throw;
//        //    }
//        //}

//        static public eStoreStatus GetStoreStatus(WeekProfile baseWeek, DateTime sellingOpenDt, DateTime sellingCloseDt)
//        {
//            try
//            {
//                eStoreStatus storeStatus = eStoreStatus.Comp;

//                bool storeDone = false;
//                WeekProfile storeOpenDate = null;
//                WeekProfile storeCloseDate = null;
//                if (sellingOpenDt != Include.UndefinedDate)
//                {
//                    storeOpenDate = Calendar.GetWeek(sellingOpenDt);
//                }

//                if (sellingCloseDt != Include.UndefinedDate)
//                {
//                    storeCloseDate = Calendar.GetWeek(sellingCloseDt);
//                    if (baseWeek.Key > storeCloseDate.Key)
//                    {
//                        storeStatus = eStoreStatus.Closed;
//                        storeDone = true;
//                    }
//                }


//                if (!storeDone)
//                {

//                    if (sellingOpenDt == Include.UndefinedDate)
//                    {
//                        storeStatus = eStoreStatus.Comp;
//                    }
//                    else
//                    {
//                        //WeekProfile storeOpenDate = Calendar.GetWeek(sp.SellingOpenDt);	

//                        //********************************************
//                        // convert weeks time frames to Week Profiles
//                        //********************************************
//                        WeekProfile noncompEnd = null;
//                        if (_noncompStorePeriodEnd != Include.UndefinedNonCompStorePeriodEnd)
//                            noncompEnd = Calendar.Add(storeOpenDate, _noncompStorePeriodEnd);

//                        WeekProfile noncompBegin = null;
//                        if (_noncompStorePeriodBegin != Include.UndefinedNonCompStorePeriodBegin)
//                            noncompBegin = Calendar.Add(storeOpenDate, _noncompStorePeriodBegin);

//                        WeekProfile newEnd = null;
//                        if (_newStorePeriodEnd != Include.UndefinedNewStorePeriodEnd)
//                            newEnd = Calendar.Add(storeOpenDate, _newStorePeriodEnd);

//                        WeekProfile newBegin = null;
//                        if (_newStorePeriodBegin != Include.UndefinedNewStorePeriodBegin)
//                            newBegin = Calendar.Add(storeOpenDate, _newStorePeriodBegin);

//                        //*****************************************************************************
//                        // If the New Store Begin Date is filled in and the Non-COmp End Date
//                        // is filled in, we check to see if the New Store End Date and the Non-COmp
//                        // Store Begin date is filled in.  If they aren't we must do some substitution.
//                        //*****************************************************************************
//                        if (newBegin != null && noncompEnd != null)
//                        {
//                            if (newEnd == null && noncompBegin == null)
//                            {
//                                newEnd = storeOpenDate;
//                                noncompBegin = storeOpenDate;
//                            }
//                            else if (newEnd == null && noncompBegin != null)
//                            {
//                                newEnd = noncompBegin;
//                            }
//                            else if (newEnd != null && noncompBegin == null)
//                            {
//                                noncompBegin = newEnd;
//                            }

//                        }

//                        //********************************************
//                        // if there is a non-comp end date, but
//                        // still no non-comp begin date...
//                        //********************************************
//                        if (noncompBegin == null && noncompEnd != null)
//                        {
//                            noncompBegin = storeOpenDate;
//                        }

//                        //********************************************
//                        // if there is a new begin date, but
//                        // still no new end date...
//                        //********************************************
//                        if (newEnd == null && newBegin != null)
//                        {
//                            newEnd = storeOpenDate;
//                        }

//                        // START NEW/NON-COMP/COMP/PREOPEN store calculation
//                        //******************************************************
//                        // If current week falls prior to opening date... 
//                        //******************************************************
//                        if (newBegin != null)
//                        {
//                            if (newBegin.Key > baseWeek.Key)
//                            {
//                                storeStatus = eStoreStatus.Preopen;
//                                storeDone = true;
//                            }
//                        }

//                        //******************************************************
//                        // If current week falls within the NEW store range... 
//                        //******************************************************
//                        if (!storeDone)
//                        {
//                            if (newBegin != null)
//                            {
//                                if (newBegin.Key <= baseWeek.Key &&
//                                    newEnd.Key >= baseWeek.Key)
//                                {
//                                    storeStatus = eStoreStatus.New;
//                                    storeDone = true;
//                                }
//                            }
//                        }

//                        //******************************************************
//                        // If current week falls within the NON-COMP store range... 
//                        //******************************************************
//                        if (!storeDone)
//                        {
//                            if (noncompEnd != null)
//                            {
//                                if (noncompBegin.Key <= baseWeek.Key &&
//                                    noncompEnd.Key >= baseWeek.Key)
//                                {
//                                    storeStatus = eStoreStatus.NonComp;
//                                    storeDone = true;
//                                }
//                            }
//                        }

//                        //******************************************************
//                        // If the store didn't fall into either of the other ranges,
//                        // it must be COMP
//                        //******************************************************
//                        if (!storeDone)
//                        {
//                            storeStatus = eStoreStatus.Comp;
//                        }
//                    }
//                }

//                return storeStatus;
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex);
//                }
//                throw;
//            }
//        }



//        static public void AcquireCompleteWriterLock()
//        {
//            try
//            {
//                AcquireStoreWriterLock();
//                AcquireStoreGroupWriterLock();
//                AcquireStoreCharacteristicsWriterLock();
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex);
//                }
//                throw;
//            }
//        }

//        static public void ReleaseCompleteWriterLock()
//        {
//            try
//            {
//                ReleaseStoreWriterLock();
//                ReleaseStoreGroupWriterLock();
//                ReleaseStoreCharacteristicsWriterLock();
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex);
//                }
//                throw;
//            }
//        }

//        static public void AcquireCompleteReaderLock()
//        {
//            try
//            {
//                AcquireStoreReaderLock();
//                AcquireStoreGroupReaderLock();
//                AcquireStoreCharacteristicsReaderLock();
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex);
//                }
//                throw;
//            }
//        }

//        static public void ReleaseCompleteReaderLock()
//        {
//            try
//            {
//                ReleaseStoreReaderLock();
//                ReleaseStoreGroupReaderLock();
//                ReleaseStoreCharacteristicsReaderLock();
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex);
//                }
//                throw;
//            }
//        }

//        static public void AcquireStoreWriterLock()
//        {
//            try
//            {
//                stores_rwl.AcquireWriterLock(WriterLockTimeOut);
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:AcquireStoreWriterLock writer lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:AcquireStoreWriterLock writer lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }


//        static public void AcquireStoreReaderLock()
//        {
//            try
//            {
//                stores_rwl.AcquireReaderLock(ReaderLockTimeOut);
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:AcquireStoreReaderLock reader lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:AcquireStoreReaderLock reader lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }


//        static public void ReleaseStoreWriterLock()
//        {
//            try
//            {
//                stores_rwl.ReleaseWriterLock();
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:ReleaseStoreWriterLock writer lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:ReleaseStoreWriterLock writer lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void ReleaseStoreReaderLock()
//        {
//            try
//            {
//                stores_rwl.ReleaseReaderLock();
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:ReleaseStoreReaderLock reader lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:ReleaseReaderWriterLock reader lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void AcquireStoreGroupWriterLock()
//        {
//            try
//            {
//                storeGroup_rwl.AcquireWriterLock(WriterLockTimeOut);
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:AcquireStoreGroupWriterLock writer lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:AcquireStoreGroupWriterLock writer lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void AcquireStoreGroupReaderLock()
//        {
//            try
//            {
//                storeGroup_rwl.AcquireReaderLock(ReaderLockTimeOut);
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:AcquireStoreGroupReaderLock reader lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:AcquireStoreGroupReaderLock reader lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void ReleaseStoreGroupWriterLock()
//        {
//            try
//            {
//                storeGroup_rwl.ReleaseWriterLock();
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:ReleaseStoreGroupWriterLock writer lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:ReleaseStoreGroupWriterLock writer lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void ReleaseStoreGroupReaderLock()
//        {
//            try
//            {
//                storeGroup_rwl.ReleaseReaderLock();
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:ReleaseStoreGroupReaderLock reader lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:ReleaseStoreGroupReaderLock reader lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void AcquireStoreCharacteristicsWriterLock()
//        {
//            try
//            {
//                storeCharacteristics_rwl.AcquireWriterLock(WriterLockTimeOut);
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:AcquireStoreCharacteristicsWriterLock writer lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:AcquireStoreCharacteristicsWriterLock writer lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void AcquireStoreCharacteristicsReaderLock()
//        {
//            try
//            {
//                storeCharacteristics_rwl.AcquireReaderLock(ReaderLockTimeOut);
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:AcquireStoreCharacteristicsReaderLock reader lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:AcquireStoreCharacteristicsReaderLock reader lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void ReleaseStoreCharacteristicsWriterLock()
//        {
//            try
//            {
//                storeCharacteristics_rwl.ReleaseWriterLock();
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:ReleaseStoreCharacteristicsWriterLock writer lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:ReleaseStoreCharacteristicsWriterLock writer lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void ReleaseStoreCharacteristicsReaderLock()
//        {
//            try
//            {
//                storeCharacteristics_rwl.ReleaseReaderLock();
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:ReleaseStoreCharacteristicsReaderLock reader lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:ReleaseStoreCharacteristicsReaderLock reader lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        //===========================================================
//        // Keeping thisd in here to help furture locking debugging
//        //===========================================================
//        #region alternate locking for debugging
//        static public void AcquireCompleteWriterLock(string who)
//        {
//            try
//            {
//                AcquireStoreWriterLock(who);
//                AcquireStoreGroupWriterLock(who);
//                AcquireStoreCharacteristicsWriterLock(who);
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void ReleaseCompleteWriterLock(string who)
//        {
//            try
//            {
//                ReleaseStoreWriterLock(who);
//                ReleaseStoreGroupWriterLock(who);
//                ReleaseStoreCharacteristicsWriterLock(who);
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void AcquireStoreWriterLock(string who)
//        {
//            try
//            {
//                Debug.WriteLine("Acquire Store Lock --- " + who);
//                stores_rwl.AcquireWriterLock(WriterLockTimeOut);
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }
//        static public void ReleaseStoreWriterLock(string who)
//        {
//            try
//            {
//                Debug.WriteLine("Release Store Lock --- " + who);
//                stores_rwl.ReleaseWriterLock();
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void AcquireStoreGroupWriterLock(string who)
//        {
//            try
//            {
//                Debug.WriteLine("Acquire Store Group Lock --- " + who);
//                storeGroup_rwl.AcquireWriterLock(WriterLockTimeOut);
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }
//        static public void ReleaseStoreGroupWriterLock(string who)
//        {
//            try
//            {
//                Debug.WriteLine("Release Store Group Lock --- " + who);
//                storeGroup_rwl.ReleaseWriterLock();
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void AcquireStoreCharacteristicsWriterLock(string who)
//        {
//            try
//            {
//                Debug.WriteLine("Acquire Store Char Lock --- " + who);
//                storeCharacteristics_rwl.AcquireWriterLock(WriterLockTimeOut);
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }
//        static public void ReleaseStoreCharacteristicsWriterLock(string who)
//        {
//            try
//            {
//                Debug.WriteLine("Release Store CHar Lock --- " + who);
//                storeCharacteristics_rwl.ReleaseWriterLock();
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }
//        #endregion

//        static private ArrayList CheckForDuplicateGroupLevelNames()
//        {
//            try
//            {
//                AcquireStoreGroupReaderLock();
//                try
//                {
//                    return _storeGroupRelations.CheckForDuplicateGroupLevelNames();
//                }
//                finally
//                {
//                    ReleaseStoreGroupReaderLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static internal ProfileList GetAllStoresList()
//        {
//            try
//            {
//                stores_rwl.AcquireReaderLock(ReaderLockTimeOut);
//                try
//                {
//                    return (ProfileList)_allStoreList.Clone();
//                }
//                finally
//                {
//                    // Ensure that the lock is released.
//                    stores_rwl.ReleaseReaderLock();
//                }
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The reader lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:GetAllStoresList reader lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:GetAllStoresList reader lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static internal ProfileList GetStoresInGroupLevel(int groupLevelKey)
//        {
//            try
//            {
//                AcquireCompleteReaderLock();
//                try
//                {
//                    StoreGroupLevelProfile sglp = _storeGroupRelations.GetStoreGroupLevel(groupLevelKey);
//                    return (ProfileList)sglp.Stores.Clone();
//                }
//                finally
//                {
//                    ReleaseCompleteReaderLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        // Begin Issue 4067 - stodd
//        static internal void SortGroupLevels(int storeGroupRid)
//        {
//            try
//            {
//                AcquireStoreGroupReaderLock();
//                try
//                {
//                    _storeGroupRelations.SortGroupLevels(storeGroupRid);
//                }
//                finally
//                {
//                    ReleaseStoreGroupReaderLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }
//        // End Issue 4067 - stodd

//        //static internal ProfileList GetStaticStoresInGroup(int groupLevelKey)
//        //{
//        //    try
//        //    {
//        //        return _storeGroupRelations.GetStaticStoresInGroup(groupLevelKey);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//        //        if (Audit != null)
//        //        {
//        //            Audit.Log_Exception(ex, _module);
//        //        }
//        //        throw;
//        //    }
//        //}

//        /// <summary>
//        /// Returns an empty datatable of the STORE_GROUP_LEVEL_STATEMENT table
//        /// for use in the StoreGroupBuilder window.
//        /// </summary>
//        /// <returns></returns>
//        //static internal DataTable GetStoreGroupLevelStatementDtDefinition()
//        //{
//        //    try
//        //    {
//        //        AcquireStoreGroupReaderLock();
//        //        try
//        //        {
//        //            return _storeGroupRelations.GetStoreGroupLevelStatementDtDefinition();
//        //        }
//        //        finally
//        //        {
//        //            ReleaseStoreGroupReaderLock();
//        //        }
//        //    }
//        //    catch ( Exception ex)
//        //    {
//        //        EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//        //        if (Audit != null)
//        //        {
//        //            Audit.Log_Exception(ex, _module);
//        //        }
//        //        throw;
//        //    }
//        //}

//        static internal void AddGroupLevelJoin(int[] storeRID, int groupLevelRID)
//        {
//            try
//            {
//                OpenUpdateConnection();
//                _storeGroupRelations.AddGroupLevelJoin(storeRID, groupLevelRID);
//                CommitData();
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//            finally
//            {
//                CloseUpdateConnection();
//            }
//        }

//        //static internal void AddGroupLevelStatement(ArrayList sglsiArray)
//        //{
//        //    try
//        //    {
//        //        AcquireStoreGroupReaderLock();
//        //        try
//        //        {
//        //            OpenUpdateConnection();
//        //            _storeGroupRelations.AddGroupLevelStatement(sglsiArray);
//        //            CommitData();
//        //        }
//        //        finally
//        //        {
//        //            ReleaseStoreGroupReaderLock();
//        //            CloseUpdateConnection();
//        //        }
//        //    }
//        //    catch ( Exception ex)
//        //    {
//        //        EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//        //        if (Audit != null)
//        //        {
//        //            Audit.Log_Exception(ex, _module);
//        //        }
//        //        throw;
//        //    }
//        //}

//        static internal Hashtable GetAllStoreCharacteristicsHash()
//        {
//            try
//            {
//                stores_rwl.AcquireReaderLock(ReaderLockTimeOut);
//                storeCharacteristics_rwl.AcquireReaderLock(ReaderLockTimeOut);

//                try
//                {
//                    return (Hashtable)_allStoreCharacteristicsHash.Clone();
//                }
//                finally
//                {
//                    // Ensure that the lock is released.
//                    stores_rwl.ReleaseReaderLock();
//                    storeCharacteristics_rwl.ReleaseReaderLock();
//                }
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The reader lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:GetAllStoreCharacteristicsHash reader lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:GetAllStoreCharacteristicsHash reader lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static internal void AddStoreProfileToList(StoreProfile sp)
//        {
//            try
//            {
//                AcquireStoreWriterLock();
//                try
//                {
//                    if (sp.Characteristics != null)
//                    {
//                        ArrayList charList = new ArrayList();
//                        int cnt = sp.Characteristics.Count;
//                        for (int i = 0; i < cnt; i++)
//                        {
//                            StoreCharGroupProfile scgp = (StoreCharGroupProfile)sp.Characteristics[i];
//                            charList.Add(scgp);
//                        }

//                        _allStoreCharacteristicsHash.Add(sp.Key, charList);
//                        sp.Characteristics = null;
//                    }

//                    _allStoreList.Add(sp);
//                }
//                finally
//                {
//                    ReleaseStoreWriterLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static internal void AddStoreProfileToDataTable(StoreProfile store)
//        {
//            try
//            {
//                stores_rwl.AcquireWriterLock(WriterLockTimeOut);
//                try
//                {
//                    DataRow row = _dtAllStores.NewRow();
//                    row["ST_RID"] = store.Key;
//                    row["ST_ID"] = store.StoreId;
//                    row["STORE_NAME"] = store.StoreName;
//                    row["STORE_DESC"] = store.StoreDescription;
//                    row["ACTIVE_IND"] = Include.ConvertBoolToChar(store.ActiveInd);
//                    row["CITY"] = store.City;
//                    row["STATE"] = store.State;
//                    row["SELLING_SQ_FT"] = store.SellingSqFt;
//                    if (store.SellingOpenDt != Include.UndefinedDate)
//                        row["SELLING_OPEN_DATE"] = store.SellingOpenDt.ToString("MM/dd/yyyy");
//                    if (store.SellingCloseDt != Include.UndefinedDate)
//                        row["SELLING_CLOSE_DATE"] = store.SellingCloseDt.ToString("MM/dd/yyyy");
//                    if (store.StockOpenDt != Include.UndefinedDate)
//                        row["STOCK_OPEN_DATE"] = store.StockOpenDt.ToString("MM/dd/yyyy");
//                    if (store.StockCloseDt != Include.UndefinedDate)
//                        row["STOCK_CLOSE_DATE"] = store.StockCloseDt.ToString("MM/dd/yyyy");
//                    row["LEAD_TIME"] = store.LeadTime;
//                    row["SHIP_ON_MONDAY"] = Include.ConvertBoolToChar(store.ShipOnMonday);
//                    row["SHIP_ON_TUESDAY"] = Include.ConvertBoolToChar(store.ShipOnTuesday);
//                    row["SHIP_ON_WEDNESDAY"] = Include.ConvertBoolToChar(store.ShipOnWednesday);
//                    row["SHIP_ON_THURSDAY"] = Include.ConvertBoolToChar(store.ShipOnThursday);
//                    row["SHIP_ON_FRIDAY"] = Include.ConvertBoolToChar(store.ShipOnFriday);
//                    row["SHIP_ON_SATURDAY"] = Include.ConvertBoolToChar(store.ShipOnSaturday);
//                    row["SHIP_ON_SUNDAY"] = Include.ConvertBoolToChar(store.ShipOnSunday);

//                    // Issue 3557 stodd
//                    row["SIMILAR_STORE_MODEL"] = Include.ConvertBoolToChar(store.SimilarStoreModel);
//                    // BEGIN TT#1401 - stodd - add resevation stores (IMO)
//                    row["IMO_ID"] = store.IMO_ID;
//                    // END TT#1401 - stodd - add resevation stores (IMO)

//                    row["STORE_DELETE_IND"] = Include.ConvertBoolToChar(store.DeleteStore);		//TT#3272 - stodd - marked for delete not showing until restart services.

//                    row["Store Status"] = (int)store.Status;

//                    if (store.Characteristics != null)
//                    {
//                        foreach (StoreCharGroupProfile scgp in store.Characteristics)
//                        {
//                            if (scgp.CharacteristicValue.CharValue != System.DBNull.Value)
//                            {
//                                //string colName = scgp.Name.Replace(" ","_");
//                                string colName = scgp.Name;
//                                object aValue = scgp.CharacteristicValue.CharValue;
//                                // Begin MID Issue #3408 stodd
//                                int scRid = scgp.CharacteristicValue.SC_RID;
//                                string hiddenColName = GetHiddenColumnName(scgp.Key);
//                                row[hiddenColName] = scRid;
//                                // Begin MID Issue #3408 stodd
//                                switch (scgp.CharacteristicValue.StoreCharType)
//                                {
//                                    case eStoreCharType.date:
//                                        row[colName] = (Convert.ToDateTime(aValue, CultureInfo.CurrentUICulture)).ToString("MM/dd/yyyy", CultureInfo.CurrentUICulture);
//                                        break;
//                                    case eStoreCharType.dollar:
//                                        row[colName] = Convert.ToDecimal(aValue, CultureInfo.CurrentUICulture);
//                                        break;
//                                    case eStoreCharType.number:
//                                        row[colName] = Convert.ToInt32(aValue, CultureInfo.CurrentUICulture);
//                                        break;
//                                    case eStoreCharType.text:
//                                        row[colName] = aValue.ToString();
//                                        break;
//                                    case eStoreCharType.unknown:
//                                        row[colName] = aValue.ToString();
//                                        break;
//                                }
//                            }
//                        }
//                    }

//                    _dtAllStores.Rows.Add(row);
//                    _dtAllStores.AcceptChanges();
//                }
//                finally
//                {
//                    // Ensure that the lock is released.
//                    stores_rwl.ReleaseWriterLock();
//                }
//            }
//            catch (ApplicationException)
//            {
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:StoreServerGlobal writer lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:StoreServerGlobal writer lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static internal void DeleteStoreProfileFromDataTable(StoreProfile store)
//        {
//            try
//            {
//                stores_rwl.AcquireWriterLock(WriterLockTimeOut);
//                try
//                {
//                    DataRow storeRow = _dtAllStores.Rows.Find(store.Key);
//                    if (storeRow != null)
//                        _dtAllStores.Rows.Remove(storeRow);
//                    _dtAllStores.AcceptChanges();
//                }
//                finally
//                {
//                    // Ensure that the lock is released.
//                    stores_rwl.ReleaseWriterLock();
//                }
//            }
//            catch (ApplicationException)
//            {
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:StoreServerGlobal writer lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:StoreServerGlobal writer lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static internal void UpdateStoreProfileInList(StoreProfile sp)
//        {
//            try
//            {
//                AcquireStoreWriterLock();
//                try
//                {
//                    if (sp.Characteristics != null)
//                    {
//                        _allStoreCharacteristicsHash.Remove(sp.Key);
//                        ArrayList charList = new ArrayList();
//                        int cnt = sp.Characteristics.Count;
//                        for (int i = 0; i < cnt; i++)
//                        {
//                            StoreCharGroupProfile scgp = (StoreCharGroupProfile)sp.Characteristics[i];
//                            charList.Add(scgp);
//                        }

//                        _allStoreCharacteristicsHash.Add(sp.Key, charList);
//                        sp.Characteristics = null;
//                    }

//                    _allStoreList.Remove(sp);
//                    sp.Characteristics = null;
//                    _allStoreList.Add(sp);
//                }
//                finally
//                {
//                    ReleaseStoreWriterLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static internal void UpdateStoreProfiles(DataTable dtStoreProfiles)
//        {
//            try
//            {
//                AcquireStoreWriterLock();
//                try
//                {
//                    _dsAllStores.Merge(dtStoreProfiles, false, MissingSchemaAction.Ignore);
//                    _dsAllStores.AcceptChanges();
//                }
//                finally
//                {
//                    ReleaseStoreWriterLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static internal int InsertStoreProfile(object[] sa)
//        {
//            try
//            {
//                int storeRid = _storeData.StoreProfile_Insert(sa);

//                return storeRid;
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static internal void UpdateStoreProfile(object[] sa)
//        {
//            try
//            {
//                _storeData.StoreProfile_Update(sa);
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        // BEGIN TT#739-MD - STodd - delete stores
//        static internal void MarkStoreForDelete(int StoreRid, bool markForDelete)
//        {
//            try
//            {
//                _storeData.StoreProfile_MarkForDelete(StoreRid, markForDelete);
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }
//        // END TT#739-MD - STodd - delete stores

//        static internal void DeleteStore(int storeRID, int fromGroupLevelRID)
//        {
//            try
//            {
//                AcquireStoreWriterLock();
//                try
//                {
//                    OpenUpdateConnection();
//                    _storeGroupRelations.DeleteStore(storeRID, fromGroupLevelRID);
//                    CommitData();
//                }
//                finally
//                {
//                    ReleaseStoreWriterLock();
//                    CloseUpdateConnection();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static internal void AddStoreToGroupLevel(int storeRID, int toGroupLevelRID)
//        {
//            try
//            {
//                AcquireStoreGroupWriterLock();
//                try
//                {
//                    OpenUpdateConnection();
//                    _storeGroupRelations.AddStoreToGroupLevel(storeRID, toGroupLevelRID);
//                    CommitData();
//                }
//                finally
//                {
//                    ReleaseStoreGroupWriterLock();
//                    CloseUpdateConnection();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }

//        }

//        static internal StoreGroupProfile GetStoreGroup(int groupRid)
//        {
//            try
//            {
//                storeGroup_rwl.AcquireReaderLock(ReaderLockTimeOut);

//                try
//                {
//                    StoreGroupProfile sgp = _storeGroupRelations.GetStoreGroup(groupRid);
//                    //Begin Track #4052 - JScott - Filters not being enqueued
//                    //					if (!sgp.Filled)
//                    //						_storeGroupRelations.RefreshStoresInGroup(groupRid);
//                    //					StoreGroupProfile clonedStoreGroup = (StoreGroupProfile)sgp.Clone();
//                    //					return clonedStoreGroup;
//                    if (sgp != null)
//                    {
//                        if (!sgp.Filled)
//                            _storeGroupRelations.RefreshStoresInGroup(groupRid);
//                        sgp = (StoreGroupProfile)sgp.Clone();
//                    }
//                    return sgp;
//                    //End Track #4052 - JScott - Filters not being enqueued
//                }
//                finally
//                {
//                    // Ensure that the lock is released.
//                    storeGroup_rwl.ReleaseReaderLock();
//                }
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The reader lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:GetStoreGroup reader lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:GetStoreGroup reader lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        // Begin Track #4872 - JSmith - Global/User Attributes
//        //static internal int AddGroup(string groupName, bool isDynamic)
//        static internal int AddGroup(string groupName, bool isDynamic, int aUserRID)
//        // End Track #4872
//        {
//            int groupKey = Include.NoRID;
//            try
//            {
//                AcquireStoreGroupWriterLock();
//                try
//                {
//                    OpenUpdateConnection();
//                    // Begin Track #4872 - JSmith - Global/User Attributes
//                    //groupKey = _storeGroupRelations.AddGroup(groupName, isDynamic);
//                    groupKey = _storeGroupRelations.AddGroup(groupName, isDynamic, aUserRID);
//                    // End Track #4872
//                    CommitData();
//                }
//                finally
//                {
//                    ReleaseStoreGroupWriterLock();
//                    CloseUpdateConnection();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }


//            return groupKey;
//        }

//        static internal void RenameGroup(int groupRID, string newName)
//        {
//            try
//            {
//                AcquireStoreGroupWriterLock();
//                try
//                {
//                    OpenUpdateConnection();
//                    _storeGroupRelations.RenameGroup(groupRID, newName);
//                    CommitData();

//                }
//                finally
//                {
//                    // Ensure that the lock is released.
//                    ReleaseStoreGroupWriterLock();
//                    CloseUpdateConnection();
//                }
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The reader lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:RenameGroup reader lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:RenameGroup reader lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        // Begin Track #5005 - stodd
//        static internal void UpdateStoreGroup(StoreGroupProfile sgp)
//        {
//            try
//            {
//                AcquireStoreGroupWriterLock();
//                try
//                {
//                    OpenUpdateConnection();
//                    _storeGroupRelations.UpdateStoreGroup(sgp);
//                    CommitData();

//                }
//                finally
//                {
//                    // Ensure that the lock is released.
//                    ReleaseStoreGroupWriterLock();
//                    CloseUpdateConnection();
//                }
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The reader lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:UpdateStoreGroup reader lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:UpdateStoreGroup reader lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }
//        // End track #5005

//        static internal void DeleteGroup(int groupRID)
//        {
//            try
//            {
//                AcquireStoreGroupWriterLock();
//                try
//                {
//                    OpenUpdateConnection();
//                    _storeGroupRelations.DeleteGroup(groupRID);
//                    CommitData();
//                }
//                finally
//                {
//                    ReleaseStoreGroupWriterLock();
//                    CloseUpdateConnection();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Warning); //Issue 4585
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }

//        }

//        //Begin TT#1414-MD -jsobek -Attribute Set Filter -unused function
//        //static internal bool IsStoreCharGroupUsedAnywhere(int scg_rid, ref InUseInfo inUseInfo)
//        //{
//        //    try
//        //    {
//        //        return _storeGroupRelations.IsStoreCharGroupUsedAnywhere(scg_rid, ref inUseInfo);
//        //    }
//        //    catch ( Exception ex)
//        //    {
//        //        EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//        //        if (Audit != null)
//        //        {
//        //            Audit.Log_Exception(ex, _module);
//        //        }
//        //        throw;
//        //    }
//        //}
//        //End TT#1414-MD -jsobek -Attribute Set Filter -unused function

//        //Begin TT#1414-MD -jsobek -Attribute Set Filter -unused function
//        //static internal bool DoesStoreCharGroupHaveStoreValuesAssigned(int scg_rid)
//        //{
//        //    try
//        //    {
//        //        return _storeGroupRelations.DoesStoreCharGroupHaveStoreValuesAssigned(scg_rid);
//        //    }
//        //    catch ( Exception ex)
//        //    {
//        //        EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//        //        if (Audit != null)
//        //        {
//        //            Audit.Log_Exception(ex, _module);
//        //        }
//        //        throw;
//        //    }
//        //}
//        //End TT#1414-MD -jsobek -Attribute Set Filter -unused function

//        static internal bool DoesGroupLevelNameExist(int groupRid, string name)
//        {
//            try
//            {
//                return _storeGroupRelations.DoesGroupLevelNameExist(groupRid, name);
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        // Begin Track #4872 - JSmith - Global/User Attributes
//        //static internal bool DoesGroupNameExist(string name)
//        static internal bool DoesGroupNameExist(string name, int aUserRID)
//        // End Track #4872
//        {
//            try
//            {
//                // Begin Track #4872 - JSmith - Global/User Attributes
//                //return _storeGroupRelations.DoesGroupNameExist(name);
//                return _storeGroupRelations.DoesGroupNameExist(name, aUserRID);
//                // End Track #4872
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public int AddGroupLevel(int groupRID, string groupLevelName, int filterRID)
//        {
//            int groupLevelRID = Include.NoRID;
//            try
//            {
//                AcquireStoreGroupWriterLock();
//                try
//                {
//                    OpenUpdateConnection();
//                    groupLevelRID = _storeGroupRelations.AddGroupLevel(groupRID, groupLevelName, filterRID);
//                    CommitData();
//                }
//                finally
//                {
//                    ReleaseStoreGroupWriterLock();
//                    CloseUpdateConnection();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }

//            return groupLevelRID;
//        }

//        static public int GetGroupLevelSequence(int storeGroupLevelKey)
//        {
//            try
//            {
//                storeGroup_rwl.AcquireReaderLock(ReaderLockTimeOut);

//                try
//                {
//                    return _storeGroupRelations.GetGroupLevelSequence(storeGroupLevelKey);
//                }
//                finally
//                {
//                    // Ensure that the lock is released.
//                    storeGroup_rwl.ReleaseReaderLock();
//                }
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The reader lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:GetGroupLevelSequence reader lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:GetGroupLevelSequence reader lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void RenameGroupLevel(int groupLevelRID, string newName)
//        {
//            try
//            {
//                AcquireStoreGroupWriterLock();

//                try
//                {
//                    OpenUpdateConnection();
//                    _storeGroupRelations.RenameGroupLevel(groupLevelRID, newName);
//                    CommitData();
//                }
//                finally
//                {
//                    // Ensure that the lock is released.
//                    ReleaseStoreGroupWriterLock();
//                    CloseUpdateConnection();
//                }
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The reader lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:RenameGroupLevel reader lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:RenameGroupLevel reader lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void DeleteGroupLevel(int groupLevelRID)
//        {
//            try
//            {
//                AcquireStoreGroupWriterLock();
//                try
//                {
//                    OpenUpdateConnection();
//                    _storeGroupRelations.DeleteGroupLevel(groupLevelRID);
//                    CommitData();
//                }
//                finally
//                {
//                    ReleaseStoreGroupWriterLock();
//                    CloseUpdateConnection();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Warning); //Issue 4585
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }

//        }

//        static public void UpdateGroupLevelSequence(int groupLevelRID, int sglSeq)
//        {
//            try
//            {
//                AcquireStoreGroupWriterLock();
//                try
//                {
//                    OpenUpdateConnection();
//                    _storeGroupRelations.UpdateGroupLevelSequence(groupLevelRID, sglSeq);
//                    CommitData();
//                }
//                finally
//                {
//                    // Ensure that the lock is released.
//                    ReleaseStoreGroupWriterLock();
//                    CloseUpdateConnection();
//                }
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The reader lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:UpdateGroupLevelSequence reader lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:UpdateGroupLevelSequence reader lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        //static internal void DeleteGroupLevelJoin(int SGL_RID)
//        //{
//        //    try
//        //    {
//        //        AcquireStoreGroupWriterLock();
//        //        try
//        //        {
//        //            OpenUpdateConnection();
//        //            _storeGroupRelations.DeleteGroupLevelJoin(SGL_RID);
//        //            CommitData();
//        //        }
//        //        finally
//        //        {
//        //            ReleaseStoreGroupWriterLock();
//        //            CloseUpdateConnection();
//        //        }
//        //    }
//        //    catch ( Exception ex)
//        //    {
//        //        EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//        //        if (Audit != null)
//        //        {
//        //            Audit.Log_Exception(ex, _module);
//        //        }
//        //        throw;
//        //    }

//        //}

//        static internal void DeleteGroupLevelJoin(int SGL_RID, int storeRID)
//        {
//            try
//            {
//                AcquireStoreGroupWriterLock();
//                try
//                {
//                    OpenUpdateConnection();
//                    _storeGroupRelations.DeleteGroupLevelJoin(SGL_RID, storeRID);
//                    CommitData();
//                }
//                finally
//                {
//                    ReleaseStoreGroupWriterLock();
//                    CloseUpdateConnection();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }

//        }

//        //static internal void DeleteGroupLevelStatement(int SGL_RID)
//        //{
//        //    try
//        //    {
//        //        AcquireStoreGroupWriterLock();
//        //        try
//        //        {
//        //            OpenUpdateConnection();
//        //            _storeGroupRelations.DeleteGroupLevelStatement(SGL_RID);
//        //            CommitData();
//        //        }
//        //        finally
//        //        {
//        //            ReleaseStoreGroupWriterLock();
//        //            CloseUpdateConnection();
//        //        }
//        //    }
//        //    catch ( Exception ex)
//        //    {
//        //        EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//        //        if (Audit != null)
//        //        {
//        //            Audit.Log_Exception(ex, _module);
//        //        }
//        //        throw;
//        //    }

//        //}


//        /// <summary>
//        /// Gets a cloned StoreGroupList from Global. 
//        /// </summary>
//        /// <returns></returns>
//        static public ProfileList GetStoreGroupList()
//        {
//            try
//            {
//                storeGroup_rwl.AcquireWriterLock(WriterLockTimeOut);

//                try
//                {
//                    ProfileList clonedStoreGroupList = new ProfileList(eProfileType.StoreGroup);
//                    ProfileList storeGroupList = _storeGroupRelations.GetStoreGroupList();
//                    int groupCnt = storeGroupList.Count;
//                    for (int i = 0; i < groupCnt; i++)
//                    {
//                        StoreGroupProfile sgp = (StoreGroupProfile)storeGroupList[i];
//                        clonedStoreGroupList.Add((StoreGroupProfile)sgp.Clone());
//                    }
//                    return clonedStoreGroupList;
//                }
//                finally
//                {
//                    // Ensure that the lock is released.
//                    storeGroup_rwl.ReleaseWriterLock();
//                }
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The reader lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:GetStoreGroupList reader lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:GetStoreGroupList reader lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        // Begin Track #4872 - JSmith - Global/User Attributes
//        /// <summary>
//        /// Gets a cloned StoreGroupList from Global for a specific user. 
//        /// </summary>
//        /// <returns>
//        /// ProfileList containing all StoreGroupProfiles.  Visible flag set to identify which
//        /// the user can see.
//        /// </returns>
//        static public ProfileList GetStoreGroupList(int aUserRID)
//        {
//            Hashtable sharedGroups;
//            try
//            {
//                sharedGroups = GetSharedGroups(aUserRID);
//                // Begin TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//                //storeGroup_rwl.AcquireWriterLock(WriterLockTimeOut);
//                storeGroup_rwl.AcquireReaderLock(ReaderLockTimeOut);
//                // End TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors

//                try
//                {
//                    ProfileList clonedStoreGroupList = new ProfileList(eProfileType.StoreGroup);
//                    ProfileList storeGroupList = _storeGroupRelations.GetStoreGroupList();
//                    int groupCnt = storeGroupList.Count;
//                    for (int i = 0; i < groupCnt; i++)
//                    {
//                        StoreGroupProfile sgp = (StoreGroupProfile)((StoreGroupProfile)storeGroupList[i]).Clone();
//                        if (sgp.OwnerUserRID == Include.GlobalUserRID ||
//                            sgp.OwnerUserRID == aUserRID ||
//                            sharedGroups.ContainsKey(sgp.Key))
//                        {
//                            sgp.Visible = true;
//                        }
//                        else
//                        {
//                            sgp.Visible = false;
//                        }
//                        clonedStoreGroupList.Add(sgp);
//                    }
//                    return clonedStoreGroupList;
//                }
//                finally
//                {
//                    // Ensure that the lock is released.
//                    // Begin TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//                    //storeGroup_rwl.ReleaseWriterLock();
//                    storeGroup_rwl.ReleaseReaderLock();
//                    // End TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//                }
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                // End TT#189
//                // The reader lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:GetStoreGroupList reader lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:GetStoreGroupList reader lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        /// <summary>
//        /// Gets a Hashtable with keys of all groups shared to the user.
//        /// </summary>
//        /// <returns></returns>
//        static private Hashtable GetSharedGroups(int aUserRID)
//        {
//            Hashtable sharedGroups;
//            DataTable dt;
//            try
//            {
//                sharedGroups = new Hashtable();
//                dt = _storeData.SharedStoreGroups_Read(aUserRID);
//                foreach (DataRow dr in dt.Rows)
//                {
//                    sharedGroups.Add(Convert.ToInt32(dr["SG_RID"]), null);
//                }

//                return sharedGroups;
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }
//        // End Track #4872

//        static public void StoreCharJoin_DeleteAll(int storeRID)
//        {
//            try
//            {
//                _storeData.StoreCharJoin_DeleteAll(storeRID);
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }


//        static public StoreCharacteristics Characteristics()
//        {
//            try
//            {
//                AcquireStoreCharacteristicsReaderLock();
//                try
//                {
//                    return _characteristics;
//                }
//                finally
//                {
//                    ReleaseStoreCharacteristicsReaderLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static private void AddCharacteristicColumns()
//        {
//            try
//            {
//                AcquireStoreCharacteristicsWriterLock();

//                //============================
//                // Add Store Status column
//                //============================
//                AddCharacteristicColumnToStoreDataTable(StoreStatusText, "Store Status", eStoreCharType.number, null);

//                try
//                {
//                    ArrayList charGroupList = GetStoreCharacteristicList();

//                    foreach (CharacteristicGroup cg in charGroupList)
//                    {
//                        // BEGIN Issue 4959/4960
//                        AddCharacteristicColumnToStoreDataTable(GetHiddenColumnName(cg.RID), null, eStoreCharType.number, 0);
//                        // END Issue 4959/4960
//                        AddCharacteristicColumnToStoreDataTable(cg.Name, null, cg.DataType, null);
//                    }
//                }
//                finally
//                {
//                    ReleaseStoreCharacteristicsWriterLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        /// <summary>
//        /// Takes the characteristic name and builds a column from it.
//        /// The name becomes the caption.
//        /// If the column name is sent in, we use it instead of the replacing spaces.
//        /// </summary>
//        /// <param name="caption"></param>
//        /// <param name="column"></param>
//        /// <param name="aCharType"></param>
//        /// <param name="defaultValue"></param>
//        static internal void AddCharacteristicColumnToStoreDataTable(string caption, string column,
//            eStoreCharType aCharType, object defaultValue)  // Issue 4959/4960
//        {
//            try
//            {
//                AcquireStoreWriterLock();
//                try
//                {
//                    DataColumn myColumn = new DataColumn();
//                    myColumn.AllowDBNull = true;
//                    myColumn.Caption = caption;
//                    if (column == null)
//                    {
//                        //string columnName = caption.Replace(" ","_");
//                        string columnName = caption;
//                        myColumn.ColumnName = columnName;
//                    }
//                    else
//                        myColumn.ColumnName = column;
//                    myColumn.DefaultValue = null;
//                    myColumn.ReadOnly = false;
//                    switch (aCharType)
//                    {
//                        case DataCommon.eStoreCharType.text:
//                            myColumn.DataType = System.Type.GetType("System.String");
//                            if (defaultValue != null)
//                                myColumn.DefaultValue = defaultValue.ToString();
//                            else
//                                myColumn.DefaultValue = string.Empty;
//                            break;
//                        case DataCommon.eStoreCharType.date:
//                            myColumn.DataType = System.Type.GetType("System.DateTime");
//                            if (defaultValue != null)
//                                myColumn.DefaultValue = Convert.ToDateTime(defaultValue);
//                            break;
//                        case DataCommon.eStoreCharType.number:
//                            myColumn.DataType = System.Type.GetType("System.Single");
//                            if (defaultValue != null)
//                                myColumn.DefaultValue = Convert.ToDecimal(defaultValue);
//                            break;
//                        case DataCommon.eStoreCharType.dollar:
//                            myColumn.DataType = System.Type.GetType("System.Single");
//                            if (defaultValue != null)
//                                myColumn.DefaultValue = Convert.ToDecimal(defaultValue);
//                            break;
//                    }

//                    // Add the column to the table. 
//                    _dtAllStores.Columns.Add(myColumn);
//                    _dsAllStores.AcceptChanges();
//                }
//                finally
//                {
//                    ReleaseStoreWriterLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static internal void AddCharacteristicColumn(string columnName, int charGroupRid, eStoreCharType aCharType, bool hasList)
//        {
//            try
//            {
//                AcquireStoreCharacteristicsWriterLock();
//                try
//                {
//                    // BEGIN Issue 4959/4960 
//                    AddCharacteristicColumnToStoreDataTable(GetHiddenColumnName(charGroupRid), null, eStoreCharType.number, 0);
//                    // END Issue 4959/4960
//                    AddCharacteristicColumnToStoreDataTable(columnName, null, aCharType, null);

//                    IDictionaryEnumerator myEnumerator = _allStoreCharacteristicsHash.GetEnumerator();

//                    while (myEnumerator.MoveNext())
//                    {
//                        ArrayList aList = (ArrayList)myEnumerator.Value;
//                        StoreCharGroupProfile ch = new StoreCharGroupProfile(charGroupRid);
//                        ch.Name = columnName;
//                        ch.CharacteristicValue.CharValue = System.DBNull.Value;
//                        ch.CharacteristicValue.SC_RID = 0;
//                        aList.Add(ch);
//                    }

//                    // add to relations
//                    _characteristics.AddStoreCharGroup(columnName, charGroupRid, aCharType, hasList);
//                }
//                finally
//                {
//                    ReleaseStoreCharacteristicsWriterLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        // Begin TT# 166 - stodd
//        static internal int AddStoreCharGroupDB(string charGroupName, eStoreCharType aCharType, bool hasList)
//        // End Track #4872
//        {
//            int charGroupKey = Include.NoRID;
//            try
//            {
//                AcquireStoreCharacteristicsWriterLock();
//                try
//                {
//                    OpenUpdateConnection();
//                    charGroupKey = _characteristics.AddStoreCharGroupDB(charGroupName, aCharType, hasList);
//                    CommitData();
//                }
//                finally
//                {
//                    ReleaseStoreCharacteristicsWriterLock();
//                    CloseUpdateConnection();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }


//            return charGroupKey;
//        }
//        // End TT# 166


//        static internal void AddStoreChar(int charGroupRid, int charValueRid, object eValue)
//        {
//            try
//            {
//                AcquireStoreCharacteristicsWriterLock();
//                try
//                {
//                    _characteristics.AddStoreChar(charGroupRid, charValueRid, eValue);
//                }
//                finally
//                {
//                    ReleaseStoreCharacteristicsWriterLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        /// <summary>
//        /// used only in special cases when a store char needs to be added to the DB, but
//        /// NOT through the maint screen
//        /// </summary>
//        /// <param name="charGroupName"></param>
//        /// <param name="eValue"></param>
//        /// <returns></returns>
//        static public int AddStoreChar(string charGroupName, object eValue)
//        {
//            try
//            {
//                AcquireStoreCharacteristicsWriterLock();
//                try
//                {
//                    OpenUpdateConnection();
//                    int scRid = _characteristics.InsertStoreChar(charGroupName, eValue);
//                    CommitData();
//                    CloseUpdateConnection();

//                    return scRid;
//                }
//                finally
//                {
//                    ReleaseStoreCharacteristicsWriterLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void AddStoreCharJoin(int storeKey, int charKey)
//        {
//            try
//            {
//                AcquireStoreCharacteristicsWriterLock();
//                try
//                {
//                    OpenUpdateConnection();
//                    _characteristics.InsertStoreCharJoin(storeKey, charKey);
//                    CommitData();
//                }
//                finally
//                {
//                    ReleaseStoreCharacteristicsWriterLock();
//                    CloseUpdateConnection();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void DeleteStoreCharJoin(int storeKey, int charKey)
//        {
//            try
//            {
//                AcquireStoreCharacteristicsWriterLock();
//                try
//                {
//                    OpenUpdateConnection();
//                    _characteristics.DeleteStoreCharJoin(storeKey, charKey);
//                    CommitData();
//                }
//                finally
//                {
//                    ReleaseStoreCharacteristicsWriterLock();
//                    CloseUpdateConnection();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        /// <summary>
//        /// Deletes all char values in list from all stores
//        /// </summary>
//        /// <param name="scRidList"></param>
//        static internal void DeleteStoreCharJoin(ArrayList scRidList)
//        {
//            try
//            {
//                AcquireStoreCharacteristicsWriterLock();
//                try
//                {
//                    _characteristics.OpenUpdateConnection();
//                    foreach (int scRid in scRidList)
//                    {
//                        _characteristics.DeleteStoreCharJoin(scRid);
//                    }
//                    _characteristics.CommitData();
//                }
//                finally
//                {
//                    ReleaseStoreCharacteristicsWriterLock();
//                    _characteristics.CloseUpdateConnection();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static internal void DeleteStoreCharJoin(int scRid)
//        {
//            try
//            {
//                AcquireStoreCharacteristicsWriterLock();
//                try
//                {
//                    _characteristics.OpenUpdateConnection();
//                    _characteristics.DeleteStoreCharJoin(scRid);
//                    _characteristics.CommitData();
//                }
//                finally
//                {
//                    ReleaseStoreCharacteristicsWriterLock();
//                    _characteristics.CloseUpdateConnection();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        /// <summary>
//        /// deletes from the database all values for the store char group
//        /// </summary>
//        /// <param name="scgRid"></param>
//        //static internal void DeleteStoreCharacteristicNonListValues(int scgRid)
//        //{
//        //    try
//        //    {
//        //        AcquireStoreCharacteristicsWriterLock();
//        //        try
//        //        {
//        //            Hashtable sgHash = _characteristics.HashByCharGroupRID;
//        //            CharacteristicGroup cg = (CharacteristicGroup)sgHash[scgRid];
//        //            // load keys into separate array
//        //            ArrayList keyList = new ArrayList();
//        //            foreach (StoreCharValue scv in cg.Values)
//        //            {
//        //                keyList.Add(scv.SC_RID);
//        //            }
//        //            // loop through keys and remove them
//        //            _characteristics.OpenUpdateConnection();
//        //            foreach (int aCharKey in keyList)
//        //            {
//        //                _characteristics.DeleteStoreCharValue(aCharKey);
//        //            }
//        //            _characteristics.CommitData();
//        //            _characteristics.CloseUpdateConnection();
//        //        }
//        //        finally
//        //        {
//        //            ReleaseStoreCharacteristicsWriterLock();
//        //        }
//        //    }
//        //    catch ( Exception ex)
//        //    {
//        //        EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//        //        if (Audit != null)
//        //        {
//        //            Audit.Log_Exception(ex, _module);
//        //        }
//        //        throw;
//        //    }
//        //}


//        static internal void DeleteCharacteristicColumn(string columnName)
//        {
//            try
//            {
//                AcquireStoreWriterLock();
//                AcquireStoreCharacteristicsWriterLock();
//                try
//                {
//                    // remove the column from the stores Data table. 
//                    //string columnNameNoSpaces = columnName.Replace(" ","_");
//                    int scgRid = _characteristics.GetCharacteristicGroupRID(columnName);

//                    if (_dtAllStores.Columns.Contains(columnName))
//                    {
//                        _dtAllStores.Columns.Remove(columnName);
//                        _dtAllStores.Columns.Remove(GetHiddenColumnName(scgRid));
//                        _dtAllStores.AcceptChanges();

//                        IDictionaryEnumerator myEnumerator = _allStoreCharacteristicsHash.GetEnumerator();

//                        while (myEnumerator.MoveNext())
//                        {
//                            ArrayList aList = (ArrayList)myEnumerator.Value;
//                            foreach (StoreCharGroupProfile scgp in aList)
//                            {
//                                if (columnName == scgp.Name)
//                                {
//                                    int index = aList.IndexOf(scgp);
//                                    aList.RemoveAt(index);
//                                    break;
//                                }
//                            }
//                        }
//                    }

//                    _characteristics.DeleteStoreCharGroup(columnName);
//                }
//                finally
//                {
//                    ReleaseStoreWriterLock();
//                    ReleaseStoreCharacteristicsWriterLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void DeleteStoreCharValue(int sc_rid)
//        {
//            try
//            {
//                AcquireStoreWriterLock();
//                AcquireStoreCharacteristicsWriterLock();
//                try
//                {
//                    // from stores
//                    string charGroupName = _characteristics.StoreCharRelations.GetCharacteristicGroupIDFromValue(sc_rid);
//                    int charGroupRid = _characteristics.StoreCharRelations.GetCharacteristicGroupRID(charGroupName);
//                    object charValue = _characteristics.StoreCharRelations.GetCharacteristicValue(sc_rid);
//                    DeleteCharacteristicValueFromStores(charGroupName, charGroupRid, charValue);
//                    //Begin TT#1230-MD - jsobek -Unable to delete Store Characteristic
//                    OpenUpdateConnection();
//                    _characteristics.DeleteStoreCharValue(sc_rid);
//                    CommitData();
//                    CloseUpdateConnection();
//                    //End TT#1230-MD - jsobek -Unable to delete Store Characteristic
//                }
//                finally
//                {
//                    ReleaseStoreWriterLock();
//                    ReleaseStoreCharacteristicsWriterLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static internal void DeleteCharacteristicValueFromStores(string columnName, int scgRid, object charValue)
//        {
//            try
//            {
//                AcquireStoreWriterLock();
//                AcquireStoreCharacteristicsWriterLock();
//                try
//                {
//                    eStoreCharType charType = _characteristics.GetCharacteristicDataType(columnName);
//                    int rCount = _dtAllStores.Rows.Count;

//                    // remove the value from the char column in the stores Data table.
//                    //string columnNameNoSpaces = columnName.Replace(" ","_");
//                    if (_dtAllStores.Columns.Contains(columnName))
//                    {
//                        if (charType == eStoreCharType.date)
//                        {
//                            for (int i = 0; i < rCount; i++)
//                            {
//                                DataRow aRow = _dtAllStores.Rows[i];
//                                if (aRow[columnName] != DBNull.Value)
//                                {
//                                    DateTime d1 = (DateTime)aRow[columnName];
//                                    DateTime d2 = (DateTime)charValue;
//                                    if (d1 == d2)
//                                    {
//                                        aRow[columnName] = DBNull.Value;
//                                        aRow[GetHiddenColumnName(scgRid)] = DBNull.Value;
//                                    }
//                                }
//                            }
//                        }
//                        else if (charType == eStoreCharType.text)
//                        {
//                            for (int i = 0; i < rCount; i++)
//                            {
//                                DataRow aRow = _dtAllStores.Rows[i];
//                                if (aRow[columnName] != DBNull.Value)
//                                {
//                                    string s1 = (string)aRow[columnName];
//                                    string s2 = (string)charValue;
//                                    if (s1 == s2)
//                                    {
//                                        aRow[columnName] = DBNull.Value;
//                                        aRow[GetHiddenColumnName(scgRid)] = DBNull.Value;
//                                    }
//                                }
//                            }
//                        }
//                        else if (charType == eStoreCharType.number || charType == eStoreCharType.dollar)
//                        {
//                            for (int i = 0; i < rCount; i++)
//                            {
//                                DataRow aRow = _dtAllStores.Rows[i];
//                                if (aRow[columnName] != DBNull.Value)
//                                {
//                                    double d1 = Convert.ToDouble(aRow[columnName], CultureInfo.CurrentUICulture);
//                                    double d2 = Convert.ToDouble(charValue, CultureInfo.CurrentUICulture);
//                                    if (d1 == d2)
//                                    {
//                                        aRow[columnName] = DBNull.Value;
//                                        aRow[GetHiddenColumnName(scgRid)] = DBNull.Value;
//                                    }
//                                }
//                            }
//                        }
//                    }
//                    _dtAllStores.AcceptChanges();

//                    // remove the column from the store profile lists
//                    int scRid = _characteristics.CharacteristicExists(columnName, charValue);

//                    IDictionaryEnumerator myEnumerator = _allStoreCharacteristicsHash.GetEnumerator();

//                    while (myEnumerator.MoveNext())
//                    {
//                        ArrayList aList = (ArrayList)myEnumerator.Value;
//                        foreach (StoreCharGroupProfile scgp in aList)
//                        {
//                            if (columnName == scgp.Name && scRid == scgp.CharacteristicValue.SC_RID)
//                            {
//                                scgp.CharacteristicValue.CharValue = DBNull.Value;
//                                scgp.CharacteristicValue.SC_RID = 0;
//                                break;
//                            }
//                        }
//                    }
//                }
//                finally
//                {
//                    ReleaseStoreWriterLock();
//                    ReleaseStoreCharacteristicsWriterLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void UpdateCharacteristicValueInStores(string columnName, object oldValue, object charValue)
//        {
//            try
//            {
//                AcquireStoreWriterLock();
//                AcquireStoreCharacteristicsReaderLock();
//                try
//                {
//                    eStoreCharType charType = _characteristics.StoreCharRelations.GetCharacteristicDataType(columnName);
//                    int rCount = _dtAllStores.Rows.Count;

//                    // remove the value from the char column in the stores Data table. 
//                    //string columnNameNoSpaces = columnName.Replace(" ","_");
//                    if (_dtAllStores.Columns.Contains(columnName))
//                    {
//                        if (charType == eStoreCharType.date)
//                        {
//                            for (int i = 0; i < rCount; i++)
//                            {
//                                DataRow aRow = _dtAllStores.Rows[i];
//                                if (oldValue == DBNull.Value)
//                                {
//                                    if (aRow[columnName] == DBNull.Value)
//                                        aRow[columnName] = charValue;
//                                }
//                                else
//                                {
//                                    if (aRow[columnName] != DBNull.Value)
//                                    {
//                                        DateTime d1 = (DateTime)aRow[columnName];
//                                        DateTime d2 = (DateTime)oldValue;
//                                        if (d1 == d2)
//                                            aRow[columnName] = charValue;
//                                    }
//                                }
//                            }
//                        }
//                        else if (charType == eStoreCharType.text)
//                        {
//                            for (int i = 0; i < rCount; i++)
//                            {
//                                DataRow aRow = _dtAllStores.Rows[i];
//                                if (oldValue == DBNull.Value)
//                                {
//                                    if (aRow[columnName] == DBNull.Value)
//                                        aRow[columnName] = charValue;
//                                }
//                                else
//                                {
//                                    if (aRow[columnName] != DBNull.Value)
//                                    {
//                                        string s1 = (string)aRow[columnName];
//                                        string s2 = (string)oldValue;
//                                        if (s1 == s2)
//                                            aRow[columnName] = charValue;
//                                    }
//                                }
//                            }
//                        }
//                        else if (charType == eStoreCharType.number || charType == eStoreCharType.dollar)
//                        {
//                            for (int i = 0; i < rCount; i++)
//                            {
//                                DataRow aRow = _dtAllStores.Rows[i];
//                                if (oldValue == DBNull.Value)
//                                {
//                                    if (aRow[columnName] == DBNull.Value)
//                                        aRow[columnName] = charValue;
//                                }
//                                else
//                                {
//                                    if (aRow[columnName] != DBNull.Value)
//                                    {
//                                        double d1 = Convert.ToDouble(aRow[columnName], CultureInfo.CurrentUICulture);
//                                        double d2 = Convert.ToDouble(oldValue, CultureInfo.CurrentUICulture);
//                                        if (d1 == d2)
//                                            aRow[columnName] = charValue;
//                                    }
//                                }
//                            }
//                        }
//                    }
//                    _dtAllStores.AcceptChanges();

//                    // remove the column from the store profile lists
//                    int scRid = _characteristics.StoreCharRelations.CharacteristicExists(columnName, charValue);

//                    IDictionaryEnumerator myEnumerator = _allStoreCharacteristicsHash.GetEnumerator();

//                    while (myEnumerator.MoveNext())
//                    {
//                        ArrayList aList = (ArrayList)myEnumerator.Value;
//                        foreach (StoreCharGroupProfile scgp in aList)
//                        {
//                            if (columnName == scgp.Name && scRid == scgp.CharacteristicValue.SC_RID)
//                            {
//                                scgp.CharacteristicValue.CharValue = charValue;
//                                break;
//                            }
//                        }
//                    }
//                }
//                finally
//                {
//                    ReleaseStoreWriterLock();
//                    ReleaseStoreCharacteristicsReaderLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void UpdateCharacteristicValueInAStore(int storeKey, string columnName, object charValue)
//        {
//            try
//            {
//                AcquireStoreWriterLock();
//                AcquireStoreCharacteristicsWriterLock();
//                try
//                {
//                    //eStoreCharType charType = _characteristics.GetCharacteristicDataType(columnName);
//                    // remove the value from the char column in the stores Data table.
//                    //					DataRow [] rows = _dtAllStores.Select("ST_RID = " + storeKey.ToString(CultureInfo.CurrentUICulture));
//                    //					if (rows != null)
//                    //					{
//                    //						DataRow aRow = rows[0];
//                    //						aRow[columnName] = charValue;
//                    //					}
//                    //					_dtAllStores.AcceptChanges();

//                    if (!columnName.StartsWith(_hiddenColumnPrefix))
//                    {
//                        // remove the column from the store profile lists
//                        int scRid = _characteristics.StoreCharRelations.CharacteristicExists(columnName, charValue);

//                        IDictionaryEnumerator myEnumerator = _allStoreCharacteristicsHash.GetEnumerator();

//                        ArrayList aList = (ArrayList)_allStoreCharacteristicsHash[storeKey];

//                        foreach (StoreCharGroupProfile scgp in aList)
//                        {
//                            if (columnName == scgp.Name)
//                            {
//                                scgp.CharacteristicValue.CharValue = charValue;
//                                scgp.CharacteristicValue.SC_RID = scRid;
//                                break;
//                            }
//                        }
//                    }
//                }
//                finally
//                {
//                    ReleaseStoreWriterLock();
//                    ReleaseStoreCharacteristicsWriterLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static private void PopulateCharacteristicColumnValues()
//        {
//            string colName;
//            int storeRID, currStoreRID = 0;
//            DataCommon.eStoreCharType dataType;

//            try
//            {
//                DataTable dt = _characteristics.GetStoreValues();
//                DataRow[] filteredRows = { dt.NewRow() }; //serves only to init obj
//                DataRow currRow = null;
//                foreach (DataRow dr in dt.Rows)
//                {
//                    storeRID = Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture);
//                    dataType = (eStoreCharType)Convert.ToInt32(dr["SCG_TYPE"], CultureInfo.CurrentUICulture);
//                    //colName = ((string)dr["SCG_ID"]).Replace(" ","_");
//                    colName = (string)dr["SCG_ID"];

//                    if (storeRID != currStoreRID)
//                    {
//                        currRow = _dtAllStores.Rows.Find(storeRID);
//                        currStoreRID = storeRID;
//                    }

//                    switch (dataType)
//                    {
//                        case DataCommon.eStoreCharType.text:
//                            currRow[colName] = (string)dr["TEXT_VALUE"];
//                            break;
//                        case DataCommon.eStoreCharType.date:
//                            currRow[colName] = (DateTime)dr["DATE_VALUE"];
//                            break;
//                        case DataCommon.eStoreCharType.number:
//                            currRow[colName] = Convert.ToSingle(dr["NUMBER_VALUE"], CultureInfo.CurrentUICulture);
//                            break;
//                        case DataCommon.eStoreCharType.dollar:
//                            currRow[colName] = Convert.ToSingle(dr["DOLLAR_VALUE"], CultureInfo.CurrentUICulture);
//                            break;
//                    }

//                    colName = GetHiddenColumnName(Convert.ToInt32(dr["SCG_RID"], CultureInfo.CurrentUICulture));
//                    int aValue = Convert.ToInt32(dr["SC_RID"], CultureInfo.CurrentUICulture);
//                    currRow[colName] = aValue;
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//            finally
//            {
//                _dtAllStores.AcceptChanges();
//            }

//        }

//        static public DataCommon.eStoreCharType GetCharactersticDataType(string charGroup)
//        {
//            try
//            {
//                return _characteristics.DataType(charGroup);
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public ArrayList GetCharacteristicGroupList()
//        {
//            try
//            {
//                AcquireStoreCharacteristicsReaderLock();
//                try
//                {
//                    ArrayList cgl = _characteristics.CharacteristicGroupList;
//                    ArrayList cglCopy = new ArrayList();
//                    foreach (CharacteristicGroup cg in cgl)
//                    {
//                        CharacteristicGroup cgClone = cg.Clone();
//                        cglCopy.Add(cgClone);
//                    }
//                    return cglCopy;
//                }
//                finally
//                {
//                    ReleaseStoreCharacteristicsReaderLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public ArrayList GetStoreCharacteristicList()
//        {
//            try
//            {
//                AcquireStoreCharacteristicsReaderLock();
//                try
//                {
//                    return _characteristics.GetList();
//                }
//                finally
//                {
//                    ReleaseStoreCharacteristicsReaderLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public DataSet GetAllStoreCharacteristicsData()
//        {
//            try
//            {
//                DataSet dsStoreChar = RefreshStoreCharacteristics();
//                return dsStoreChar.Copy();
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        // Characteristic Maintenance Window Methods

//        static public DataSet RefreshStoreCharacteristics()
//        {
//            try
//            {
//                AcquireStoreCharacteristicsWriterLock();
//                try
//                {
//                    _characteristics.Refresh();
//                    DataSet dsStoreChar = _characteristics.GetCharacteristicsMaintData();
//                    return dsStoreChar;
//                }
//                finally
//                {
//                    ReleaseStoreCharacteristicsWriterLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public DataSet GetStoreCharacteristicsData()
//        {
//            try
//            {
//                AcquireStoreCharacteristicsReaderLock();
//                try
//                {
//                    DataSet dsStoreChar = _characteristics.GetCharacteristicsMaintData();
//                    return dsStoreChar.Copy();
//                }
//                finally
//                {
//                    ReleaseStoreCharacteristicsReaderLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }


//        static public DataTable UpdateStoreChar(DataTable xDataTable)
//        {
//            try
//            {
//                return _characteristics.UpdateStoreChar(xDataTable);
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void UpdateStoreChar(int charValueRid, object eValue)
//        {
//            try
//            {
//                AcquireStoreCharacteristicsWriterLock();
//                try
//                {
//                    object oldValue = _characteristics.StoreCharRelations.GetCharacteristicValue(charValueRid);
//                    DataSet dsChar = _characteristics.GetCharacteristicsMaintData();
//                    DataRow[] rows = dsChar.Tables["STORE_CHAR"].Select("SC_RID = " + charValueRid.ToString(CultureInfo.CurrentUICulture));
//                    int scgKey = Convert.ToInt32(rows[0]["SCG_RID"], CultureInfo.CurrentUICulture);
//                    CharacteristicGroup scg = (CharacteristicGroup)_characteristics.StoreCharRelations.HashByCharGroupRID[scgKey];

//                    _characteristics.UpdateStoreChar(charValueRid, eValue);

//                    UpdateCharacteristicValueInStores(scg.Name, oldValue, eValue);
//                }
//                finally
//                {
//                    ReleaseStoreCharacteristicsWriterLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }


//        static internal StoreGroupProfile RefreshStoresInGroup(int storeGroupKey)
//        {
//            try
//            {
//                AcquireStoreReaderLock();
//                AcquireStoreGroupWriterLock();
//                try
//                {
//                    StoreGroupProfile sgp = _storeGroupRelations.RefreshStoresInGroup(storeGroupKey);
//                    StoreGroupProfile cloneSgp = (StoreGroupProfile)sgp.Clone(true);
//                    return cloneSgp;
//                }
//                finally
//                {
//                    ReleaseStoreReaderLock();
//                    ReleaseStoreGroupWriterLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        // Begin Issue 3910 - stodd
//        /// <summary>
//        /// Sorts the Store Group Levels within each store group by it's sequence number
//        /// </summary>
//        static internal void RefreshSortOfGroupLevels()
//        {
//            try
//            {
//                _storeGroupRelations.RefreshSortOfGroupLevels();

//            }
//            catch (ApplicationException)
//            {
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:StoreServerGlobal writer lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:StoreServerGlobal writer lock has timed out");
//            }
//            catch (Exception err)
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }
//        // End Issue 3910

//        static internal void RefreshStoresInAllGroups()
//        {
//            try
//            {
//                GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
//                gop.LoadOptions();
//                _globalStoreDisplayOption = gop.StoreDisplay;

//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:StoreServerGlobal refreshing stores in groups...", EventLogEntryType.Information);
//                _storeGroupRelations.RefreshStoresInAllGroups(_globalStoreDisplayOption, false, false);
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:StoreServerGlobal refreshing stores in groups Completed", EventLogEntryType.Information);

//                // Begin TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//                RefreshSortOfGroupLevels();
//                // End TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//            }
//            catch (ApplicationException)
//            {
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:StoreServerGlobal writer lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:StoreServerGlobal writer lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static internal void RefreshStoreText()
//        {
//            try
//            {
//                GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
//                gop.LoadOptions();
//                _globalStoreDisplayOption = gop.StoreDisplay;

//                foreach (StoreProfile sp in _allStoreList)
//                {
//                    sp.Text = Include.GetStoreDisplay(_globalStoreDisplayOption, sp.StoreId, sp.StoreName, sp.StoreDescription);
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        // BEGIN TT#739-MD - STodd - delete stores
//        static internal bool DeleteUnusedGroupLevels(ref int numGroupsRemoved)
//        {
//            try
//            {
//                return _storeGroupRelations.DeleteUnusedGroupLevels(ref numGroupsRemoved);
//            }
//            catch (ApplicationException)
//            {
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:StoreServerGlobal writer lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:StoreServerGlobal writer lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }
//        // END TT#739-MD - STodd - delete stores

//        static public DataTable UpdateStoreCharGroup(DataTable xDataTable)
//        {
//            try
//            {
//                return _characteristics.UpdateStoreCharGroup(xDataTable);
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static internal void UpdateStoreCharGroup(string charGroupName, int charGroupRid, bool hasList)
//        {
//            try
//            {
//                AcquireStoreCharacteristicsWriterLock();
//                AcquireStoreGroupWriterLock();
//                try
//                {
//                    string oldCharGroupName = _characteristics.GetCharacteristicGroupID(charGroupRid);

//                    _characteristics.UpdateStoreCharGroup(charGroupName, charGroupRid, hasList);
//                    // Begin MID Issue #3384 - stodd
//                    //string oldName = oldCharGroupName.Replace(" ","_");
//                    if (charGroupName != oldCharGroupName)
//                    {
//                        DataColumn dColumn = _dtAllStores.Columns[oldCharGroupName];
//                        // End MID Issue #3384 - stodd
//                        dColumn.ColumnName = charGroupName;
//                        dColumn.Caption = charGroupName;
//                        _dtAllStores.AcceptChanges();
//                    }
//                }
//                finally
//                {
//                    ReleaseStoreCharacteristicsWriterLock();
//                    ReleaseStoreGroupWriterLock();
//                }
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void OpenUpdateConnection()
//        {
//            try
//            {
//                _storeData.OpenUpdateConnection();
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void CommitData()
//        {
//            try
//            {
//                _storeData.CommitData();
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static public void CloseUpdateConnection()
//        {
//            try
//            {
//                _storeData.CloseUpdateConnection();
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        static private string GetHiddenColumnName(int scgRid)
//        {
//            try
//            {
//                string hiddenColumnName = _hiddenColumnPrefix + scgRid.ToString(CultureInfo.CurrentUICulture);
//                return hiddenColumnName;
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        /// <summary>
//        /// The Load method is called by the service or client to trigger the instantiation of the static StoreServerGlobal
//        /// object
//        /// </summary>

//        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
//        //static public void Load()
//        static public void Load(bool aLocal)
//        // End TT#189
//        {
//            try
//            {
//                try
//                {
//                    //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
//                    //LoadBase(eProcesses.storeService);
//                    lock (_loadLock.SyncRoot)
//                    {
//                        if (!_loaded)
//                        {
//                            // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
//                            //_audit = new Audit(eProcesses.storeService, Include.AdministratorUserRID);
//                            if (!aLocal)
//                            {
//                                _audit = new Audit(eProcesses.storeService, Include.AdministratorUserRID);
//                            }
//                            //End TT#189  
//                            // Begin TT#2307 - JSmith - Incorrect Stock Values
//                            int messagingInterval = Include.Undefined;
//                            object parm = MIDConfigurationManager.AppSettings["MessagingInterval"];
//                            if (parm != null)
//                            {
//                                messagingInterval = Convert.ToInt32(parm);
//                            }
//                            //LoadBase();
//                            LoadBase(eMIDMessageSenderRecepient.storeService, messagingInterval, aLocal, eProcesses.storeService);
//                            // End TT#2307;

//                            //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
//                            EventLog.WriteEntry("MIDStoreService", "MIDStoreService begin building store information", EventLogEntryType.Information);

//                            // create reader locks
//                            //stores_rwl.AcquireWriterLock(WriterLockTimeOut);

//                            //===============================================
//                            // get posting date
//                            //===============================================
//                            DateTime postingDate = DateTime.Now;
//                            MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
//                            DataTable dt = mhd.PostingDate_Read();
//                            if (dt.Rows.Count == 1)
//                            {
//                                DataRow dr = dt.Rows[0];
//                                if (dr["POSTING_DATE"] != DBNull.Value)
//                                {
//                                    postingDate = Convert.ToDateTime(dr["POSTING_DATE"], CultureInfo.CurrentUICulture);
//                                }
//                            }
//                            Calendar.SetPostingDate(postingDate);

//                            //=========================
//                            // Build Global Area
//                            //=========================
//                            BuildStoreServerGlobalArea();

//                            // Begin TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//                            StoreServerGlobal.RefreshSortOfGroupLevels();
//                            // End TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors

//                            SetInitialMemoryCounts();
//                            EventLog.WriteEntry("MIDStoreService", "MIDStoreService completed building store information", EventLogEntryType.Information);
//                            _lastRefreshDate = _createdDate;
//                            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

//                            // Begin TT#2307 - JSmith - Incorrect Stock Values
//                            MessageProcessor.Messaging.OnMessageSentHandler += new MessageEvent.MessageEventHandler(Messaging_OnMessageSentHandler);
//                            // End TT#2307

//                            // Begin TT#195 MD - JSmith - Add environment authentication
//                            if (!aLocal)
//                            {
//                                RegisterServiceStart();
//                            }
//                            // End TT#195 MD

//                            _loaded = true;
//                        }
//                    }
//                    //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
//                }
//                finally
//                {
//                    // Ensure that the lock is released.
//                    //stores_rwl.ReleaseWriterLock();
//                }
//            }
//            catch (ApplicationException)
//            {
//                // The writer lock request timed out.
//                EventLog.WriteEntry("MIDStoreService", "MIDStoreService:StoreServerGlobal writer lock has timed out", EventLogEntryType.Error);
//                throw new MIDException(eErrorLevel.severe, 0, "MIDStoreService:StoreServerGlobal writer lock has timed out");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("MIDStoreService", ex.ToString(), EventLogEntryType.Error);
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex, _module);
//                }
//                throw;
//            }
//        }

//        // Begin TT#2307 - JSmith - Incorrect Stock Values
//        static void Messaging_OnMessageSentHandler(object source, MessageEventArgs e)
//        {
//            try
//            {
//                switch (e.MessageCode)
//                {
//                    default:
//                        break;
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }
//        // End TT#2307 

//        //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

//        /// <summary>
//        /// Cleans up all resources for the service
//        /// </summary>

//        static public void CleanUp()
//        {
//            try
//            {
//                // Begin TT#2307 - JSmith - Incorrect Stock Values
//                if (isExecutingLocal &&
//                    MessageProcessor.isListeningForMessages)
//                {
//                    StopMessageListener();
//                }
//                // End TT#2307

//                if (Audit != null)
//                {
//                    Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", Audit.HighestMessageLevel);
//                    // Begin TT#1243 - JSmith - Audit Performance
//                    Audit.CloseUpdateConnection();
//                    // End TT#1243
//                }
//            }
//            catch (Exception ex)
//            {
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex);
//                }
//            }
//        }
//        //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
//    }


//}
