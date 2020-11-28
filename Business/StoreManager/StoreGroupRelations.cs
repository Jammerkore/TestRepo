//using System;
//using System.IO;
//using System.Collections;
//using System.Data;
//using System.Diagnostics;
//using System.Globalization;
//using System.Text;

//using MIDRetail.Data;
//using MIDRetail.Common;
//using MIDRetail.DataCommon;
//using System.Reflection.Emit;

//namespace MIDRetail.Business
//{
//    /// <summary>
//    /// StoreGroupRelations is an interface into the Store Group and Store Levels relationships.
//    /// </summary>
//    /// <remarks>
//    /// The important object that this class has control of is the Store Group List.  The Store
//    /// Group List keeps track of what group levels belong to what groups and what stores
//    /// are in each group level.
//    /// </remarks>
//    public class StoreGroupRelations
//    {
//        private StoreData _storeData = null;
//        private DataTable _dtAllStores = null;
//        //private DataTable _dtStoreGroup;
//        //private DataTable _dtStoreGroupLevel;
//        //private DataTable _dtStoreGroupLevelStatement;
//        //private DataTable _dtStoreGroupLevelJoin;

//        private ProfileList _storeGroupList = null;
//        private ProfileList _allStoreList = null;
//        //private Hashtable _allStoreCharacteristicsHash = null;
//        private Hashtable _groupLevelHash = null;		// Keyed by Store Group Level and contains store group level profile
//        private ArrayList _availableStores = null;
//        //private StoreCharacteristics _characteristics;
//        private eStoreDisplayOptions _globalStoreDisplayOption;
//        private Audit _audit;
//        private MRSCalendar _calendar;
//        //private string _storeStatusText;
//        private bool _isStartup = false;   //TT#1598 - store perf
//        private string _sglInUseSql = string.Empty;	// TT#739-MD - STodd - delete stores

//        //Begin TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
//        //private string _availableStoresText = MIDText.GetTextOnly(eMIDTextCode.lbl_AvailableStores);

//        //End TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
//        System.DateTime beginTime;
//        System.DateTime endTime;
//        System.DateTime beginTime2;
//        System.DateTime endTime2;
//        System.DateTime beginTime3;
//        System.DateTime endTime3;

		
//        //public DataTable AllStoreDataTable
//        //{
//        //    get { return _dtAllStores ; }
//        //    set { _dtAllStores = value; }
//        //}

//        //public ProfileList AllStoreList
//        //{
//        //    get { return _allStoreList ; }
//        //    set { _allStoreList = value; }
//        //}

//        //public Hashtable AllStoreCharacteristicsHash
//        //{
//        //    get { return _allStoreCharacteristicsHash ; }
//        //    set { _allStoreCharacteristicsHash = value; }
//        //}

//        // BEGIN TT#1598 - stodd - store perf
//        public bool IsStartUp
//        {
//            get { return _isStartup; }
//            set { _isStartup = value; }
//        }
//        // END TT#1598 - stodd - store perf

//        //internal string StoreStatusText
//        //{
//        //    get	
//        //    { 
//        //        if (_storeStatusText == null)
//        //            _storeStatusText = MIDText.GetTextOnly(eMIDTextCode.lbl_StoreStatus);
//        //        return _storeStatusText;
//        //    }
//        //}

//        public StoreGroupRelations(StoreData storeData, DataTable dtAllStores, ProfileList allStoreList, Audit audit, MRSCalendar calendar)
//        {
//            _storeData = storeData;
//            _dtAllStores = dtAllStores;
//            _allStoreList = allStoreList;
//            _audit = audit;
//            _calendar = calendar;
			
//            //_dtStoreGroup = sg;
//            //_dtStoreGroupLevel = sgl;
//            //_dtStoreGroupLevelStatement = sgls;
//            //_dtStoreGroupLevelJoin = sglj;

//            //_characteristics = chars;
			
//            GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
//            gop.LoadOptions();
//            _globalStoreDisplayOption = gop.StoreDisplay;

//            //PopulateStoreGroups();
//        }

//        /// <summary>
//        /// StoreGroupRelations constructor LITE.
//        /// Holds no data.  Only used by Session because it provides some methods that would
//        /// otherwise have to be duplicated in the store session.
//        /// </summary>
//        /// <param name="audit"></param>
//        /// <param name="calendar"></param>
//        public StoreGroupRelations(Audit audit, MRSCalendar calendar)
//        {
//            _audit = audit;
//            _calendar = calendar;
//            GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
//            gop.LoadOptions();
//            _globalStoreDisplayOption = gop.StoreDisplay;
//        }

//        //internal void Refresh(DataTable dtAllStores)
//        //{
//        //    _dtAllStores = dtAllStores;
//        //    PopulateStoreGroups();
//        //}

//        //internal void RefreshStoresInGroups(DataTable dtAllStores)
//        //{
//        //    _dtAllStores = dtAllStores;
//        //    PopulateStoreGroups();
//        //}

//        //internal ProfileList GetStoreGroupList()
//        //{
//        //    // Begin TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//        //    //_storeGroupList.ArrayList.Sort(new SGSequenceComparer());
//        //    // End TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//        //    return _storeGroupList;
//        //}

//        /// <summary>
//        /// NEW!!!
//        /// </summary>
//        //private void PopulateStoreGroups()
//        //{
//        //    try
//        //    {
//        //        if (_storeGroupList == null)
//        //            _storeGroupList = new ProfileList(eProfileType.StoreGroup);
//        //        else
//        //            _storeGroupList.Clear();

//        //        if (_availableStores == null)
//        //            _availableStores = new ArrayList();
//        //        else
//        //            _availableStores.Clear();

//        //        if (_groupLevelHash == null)
//        //            _groupLevelHash = new Hashtable();
//        //        else
//        //            _groupLevelHash.Clear();

//        //        // Begin Track #4872 - JSmith - Global/User Attributes
//        //        BuildStoreGroup(_dtStoreGroup, _dtStoreGroupLevel);

//        //        //char dynamicInd;
//        //        //int groupRID;
                
//        //        //StoreGroupProfile stGroup = null;
//        //        //StoreGroupLevelProfile stGroupLevel = null;

//        //        //DataSet dsStoreGroup = MIDEnvironment.CreateDataSet("StoreGroup");
//        //        //dsStoreGroup.Tables.Add(_dtStoreGroup);
//        //        //dsStoreGroup.Tables.Add(_dtStoreGroupLevel);
//        //        //dsStoreGroup.Tables.Add(_dtStoreGroupLevelStatement);
//        //        //dsStoreGroup.Tables.Add(_dtStoreGroupLevelJoin);

//        //        //DataRelation drStoreGroup = new DataRelation("StoreGroup", _dtStoreGroup.Columns["SG_RID"], _dtStoreGroupLevel.Columns["SG_RID"]);
//        //        //dsStoreGroup.Relations.Add(drStoreGroup);

//        //        //DataRelation drStoreGroupLevel = new DataRelation("StoreGroupLevel", _dtStoreGroupLevel.Columns["SGL_RID"], _dtStoreGroupLevelStatement.Columns["SGL_RID"]);
//        //        //dsStoreGroup.Relations.Add(drStoreGroupLevel);

//        //        //DataRelation drStoreGroupLevelJoin = new DataRelation("StoreGroupLevelJoin", _dtStoreGroupLevel.Columns["SGL_RID"], _dtStoreGroupLevelJoin.Columns["SGL_RID"]);
//        //        //dsStoreGroup.Relations.Add(drStoreGroupLevelJoin);

//        //        ////========================================
//        //        //// loop through Store groups (attributes)
//        //        ////========================================
//        //        //foreach(DataRow storeGroupRow in dsStoreGroup.Tables[0].Rows)
//        //        //{
//        //        //    groupRID = Convert.ToInt32(storeGroupRow["SG_RID"], CultureInfo.CurrentUICulture);
//        //        //    stGroup = new StoreGroupProfile(groupRID);
//        //        //    stGroup.Name = (string)storeGroupRow["SG_ID"];;
//        //        //    dynamicInd = Convert.ToChar(storeGroupRow["SG_DYNAMIC_GROUP_IND"], CultureInfo.CurrentUICulture);
//        //        //    stGroup.IsDynamicGroup = Include.ConvertCharToBool(dynamicInd);
//        //        //    // Begin Track #4872 - JSmith - Global/User Attributes
//        //        //    stGroup.OwnerUserRID = Convert.ToInt32(storeGroupRow["USER_RID"]);
//        //        //    // End Track #4872
//        //        //    //========================================================
//        //        //    // loop through child store group levels (attribute sets)
//        //        //    //========================================================
//        //        //    DataRow [] childRowsA = storeGroupRow.GetChildRows(drStoreGroup);
//        //        //    foreach (DataRow sglRow in childRowsA)
//        //        //    {
//        //        //        int sglRid = Convert.ToInt32(sglRow["SGL_RID"], CultureInfo.CurrentUICulture);
//        //        //        stGroupLevel = new StoreGroupLevelProfile(sglRid);
//        //        //        stGroupLevel.GroupRid = groupRID;
//        //        //        if (sglRow["SGL_ID"] == DBNull.Value)
//        //        //            stGroupLevel.Name = null;
//        //        //        else
//        //        //            stGroupLevel.Name = (string)sglRow["SGL_ID"];
//        //        //        if (sglRow["SGL_SEQUENCE"] == DBNull.Value)
//        //        //            stGroupLevel.Sequence = 0;
//        //        //        else
//        //        //            stGroupLevel.Sequence = Convert.ToInt32(sglRow["SGL_SEQUENCE"], CultureInfo.CurrentUICulture);

//        //        //        stGroupLevel.SqlStatement = null;
//        //        //        stGroupLevel.SqlStatementList.Clear();
//        //        //        //===================================================
//        //        //        // loop through child store group level statements
//        //        //        //====================================================
//        //        //        DataRow [] childRowsB = sglRow.GetChildRows(drStoreGroupLevel);
//        //        //        foreach (DataRow stateRow in childRowsB)
//        //        //        {		
//        //        //            StoreGroupLevelStatementItem sglStatementItem = ConvertToStoreGroupLevelStatement(stateRow);
//        //        //            stGroupLevel.SqlStatementList.Add(sglStatementItem);
//        //        //            stGroupLevel.SqlStatement += " " + (string)stateRow["SGLS_STATEMENT"];
//        //        //        }

//        //        //        //===================================================
//        //        //        // loop through child store group level joins
//        //        //        //====================================================
//        //        //        DataRow [] childRowsJ = sglRow.GetChildRows(drStoreGroupLevelJoin);
//        //        //        foreach (DataRow joinRow in childRowsJ)
//        //        //        {		
//        //        //            int storeRid = Convert.ToInt32(joinRow["ST_RID"],CultureInfo.CurrentUICulture);
//        //        //            stGroupLevel.StaticStoreList.Add(storeRid);
//        //        //        }
						
//        //        //        stGroup.GroupLevels.Add(stGroupLevel);
//        //        //        _groupLevelHash.Add(stGroupLevel.Key, stGroupLevel);
//        //        //    }

//        //        //    _storeGroupList.Add(stGroup);
//        //        //}
//        //        // End Track #4872

//        //        // Don't need these any longer...
//        //        _dtStoreGroup = null;
//        //        _dtStoreGroupLevel = null;
//        //        //_dtStoreGroupLevelStatement = null;
//        //        //_dtStoreGroupLevelJoin = null;
//        //    }
//        //    catch ( Exception err )
//        //    {
//        //        string message = err.ToString();
//        //        throw;
//        //    }
//        //}

//        // Begin Track #4872 - JSmith - Global/User Attributes
//        //public void BuildStoreGroup(DataTable aDtStoreGroup, DataTable aDtStoreGroupLevel)
//        //{
//        //    try
//        //    {
//        //        char dynamicInd;
//        //        int groupRID;

//        //        StoreGroupProfile stGroup = null;
//        //        StoreGroupLevelProfile stGroupLevel = null;

//        //        DataSet dsStoreGroup = MIDEnvironment.CreateDataSet("StoreGroup");
//        //        dsStoreGroup.Tables.Add(aDtStoreGroup);
//        //        dsStoreGroup.Tables.Add(aDtStoreGroupLevel);
//        //        //dsStoreGroup.Tables.Add(aDtStoreGroupLevelStatement);
//        //        //dsStoreGroup.Tables.Add(aDtStoreGroupLevelJoin);

//        //        DataRelation drStoreGroup = new DataRelation("StoreGroup", aDtStoreGroup.Columns["SG_RID"], aDtStoreGroupLevel.Columns["SG_RID"]);
//        //        dsStoreGroup.Relations.Add(drStoreGroup);

//        //        //DataRelation drStoreGroupLevel = new DataRelation("StoreGroupLevel", aDtStoreGroupLevel.Columns["SGL_RID"], aDtStoreGroupLevelStatement.Columns["SGL_RID"]);
//        //        //dsStoreGroup.Relations.Add(drStoreGroupLevel);

//        //        //DataRelation drStoreGroupLevelJoin = new DataRelation("StoreGroupLevelJoin", aDtStoreGroupLevel.Columns["SGL_RID"], aDtStoreGroupLevelJoin.Columns["SGL_RID"]);
//        //       // dsStoreGroup.Relations.Add(drStoreGroupLevelJoin);

//        //        //========================================
//        //        // loop through Store groups (attributes)
//        //        //========================================
//        //        foreach (DataRow storeGroupRow in dsStoreGroup.Tables[0].Rows)
//        //        {
//        //            groupRID = Convert.ToInt32(storeGroupRow["SG_RID"], CultureInfo.CurrentUICulture);
//        //            stGroup = new StoreGroupProfile(groupRID);
//        //            stGroup.Name = (string)storeGroupRow["SG_ID"]; ;
//        //            dynamicInd = Convert.ToChar(storeGroupRow["SG_DYNAMIC_GROUP_IND"], CultureInfo.CurrentUICulture);
//        //            stGroup.IsDynamicGroup = Include.ConvertCharToBool(dynamicInd);
//        //            stGroup.OwnerUserRID = Convert.ToInt32(storeGroupRow["USER_RID"]);
//        //            stGroup.FilterRID = Convert.ToInt32(storeGroupRow["FILTER_RID"]);
//        //            //========================================================
//        //            // loop through child store group levels (attribute sets)
//        //            //========================================================
//        //            DataRow[] childRowsA = storeGroupRow.GetChildRows(drStoreGroup);
//        //            foreach (DataRow sglRow in childRowsA)
//        //            {
//        //                int sglRid = Convert.ToInt32(sglRow["SGL_RID"], CultureInfo.CurrentUICulture);
//        //                stGroupLevel = new StoreGroupLevelProfile(sglRid);
//        //                stGroupLevel.GroupRid = groupRID;
//        //                if (sglRow["SGL_ID"] == DBNull.Value)
//        //                    stGroupLevel.Name = null;
//        //                else
//        //                    stGroupLevel.Name = (string)sglRow["SGL_ID"];
//        //                if (sglRow["SGL_SEQUENCE"] == DBNull.Value)
//        //                    stGroupLevel.Sequence = 0;
//        //                else
//        //                    stGroupLevel.Sequence = Convert.ToInt32(sglRow["SGL_SEQUENCE"], CultureInfo.CurrentUICulture);

//        //                //stGroupLevel.SqlStatement = null;
//        //                //stGroupLevel.SqlStatementList.Clear();
//        //                //===================================================
//        //                // loop through child store group level statements
//        //                //====================================================
//        //                //DataRow[] childRowsB = sglRow.GetChildRows(drStoreGroupLevel);
//        //                //foreach (DataRow stateRow in childRowsB)
//        //                //{
//        //                //    StoreGroupLevelStatementItem sglStatementItem = ConvertToStoreGroupLevelStatement(stateRow);
//        //                //    stGroupLevel.SqlStatementList.Add(sglStatementItem);
//        //                //    stGroupLevel.SqlStatement += " " + (string)stateRow["SGLS_STATEMENT"];
//        //                //}

//        //                //===================================================
//        //                // loop through child store group level joins
//        //                //====================================================
//        //                //DataRow[] childRowsJ = sglRow.GetChildRows(drStoreGroupLevelJoin);
//        //                //foreach (DataRow joinRow in childRowsJ)
//        //                //{
//        //                //    int storeRid = Convert.ToInt32(joinRow["ST_RID"], CultureInfo.CurrentUICulture);
//        //                //    stGroupLevel.StaticStoreList.Add(storeRid);
//        //                //}

//        //                stGroup.GroupLevels.Add(stGroupLevel);
//        //                _groupLevelHash.Add(stGroupLevel.Key, stGroupLevel);
//        //            }

//        //            _storeGroupList.Add(stGroup);
//        //        }

//        //        // Begin TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//        //        SortStoreGroupList();
//        //        // End TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//        //    }
//        //    catch (Exception err)
//        //    {
//        //        string message = err.ToString();
//        //        throw;
//        //    }
//        //}
//        // End Track #4872

//        // Begin TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//        public void SortStoreGroupList()
//        {
//            _storeGroupList.ArrayList.Sort(new SGSequenceComparer());
//        }
//        // End TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors

//        //internal StoreGroupLevelStatementItem ConvertToStoreGroupLevelStatement(DataRow dr)
//        //{
//        //    try
//        //    {
//        //        StoreGroupLevelStatementItem sglStatementItem = new StoreGroupLevelStatementItem();
//        //        sglStatementItem.Sgl_rid = Convert.ToInt32(dr["SGL_RID"], CultureInfo.CurrentUICulture);
//        //        sglStatementItem.Sequence = Convert.ToInt32(dr["SGLS_STATEMENT_SEQ"], CultureInfo.CurrentUICulture);
//        //        sglStatementItem.Sql_statement = (string)dr["SGLS_STATEMENT"];

//        //        if (dr["SGLS_CHAR_IND"] == DBNull.Value)
//        //            sglStatementItem.IsCharacteristic = false;
//        //        else if (dr["SGLS_CHAR_IND"].ToString() == "0")
//        //            sglStatementItem.IsCharacteristic = false;
//        //        else 
//        //            sglStatementItem.IsCharacteristic = true;

//        //        if (dr["SGLS_CHAR_ID"] == DBNull.Value)
//        //            sglStatementItem.CharId = Include.NoRID;
//        //        else
//        //            sglStatementItem.CharId = Convert.ToInt32(dr["SGLS_CHAR_ID"], CultureInfo.CurrentUICulture);

//        //        if (dr["SGLS_SQL_OPERATOR"] == DBNull.Value)
//        //            sglStatementItem.SqlOperator = eENGSQLOperator.Equals;
//        //        else
//        //            sglStatementItem.SqlOperator = (eENGSQLOperator)Convert.ToInt32(dr["SGLS_SQL_OPERATOR"], CultureInfo.CurrentUICulture);

//        //        if (dr["SGLS_ENG_SQL"] == DBNull.Value)
//        //            sglStatementItem.EnglishSql = string.Empty;
//        //        else
//        //            sglStatementItem.EnglishSql = dr["SGLS_ENG_SQL"].ToString();

//        //        if (dr["SGLS_DT"] == DBNull.Value)
//        //            sglStatementItem.DataType = eStoreCharType.unknown;
//        //        else
//        //            sglStatementItem.DataType = (eStoreCharType)Convert.ToInt32(dr["SGLS_DT"], CultureInfo.CurrentUICulture);

//        //        sglStatementItem.Value = dr["SGLS_VALUE"];

//        //        if (dr["SGLS_PREFIX"] == DBNull.Value)
//        //            sglStatementItem.Prefix = string.Empty;
//        //        else
//        //            sglStatementItem.Prefix = dr["SGLS_PREFIX"].ToString();

//        //        if (dr["SGLS_SUFFIX"] == DBNull.Value)
//        //            sglStatementItem.Suffix = string.Empty;
//        //        else
//        //            sglStatementItem.Suffix = dr["SGLS_SUFFIX"].ToString();

//        //        return sglStatementItem;
//        //    }
//        //    catch
//        //    {
//        //        throw;
//        //    }
//        //}

//        //// BEGIN TT#739-MD - STodd - delete stores
//        ///// <summary>
//        ///// Called by purge to cleanup empty dynamic store sets.
//        ///// </summary>
//        ///// <returns>bool indicates some sets could not be deleted.</returns>
//        //public bool DeleteUnusedGroupLevels(ref int numGroupsRemoved)
//        //{
//        //    bool SetInUse = false;
//        //    // END TT#739-MD - STodd - delete stores
//        //    // Begin Track #6421 - stodd
//        //    int delCnt = 0;
//        //    int emptyCnt = 0;	// TT#739-MD - STodd - delete stores
//        //    try
//        //    {
//        //        // BEGIN TT#739-MD - STodd - delete stores
//        //        //EventLog.WriteEntry("MIDStoreService", "MIDStoreService:StoreServerGlobal removing empty, unused dynamic attribute sets...", EventLogEntryType.Information);
//        //        // END TT#739-MD - STodd - delete stores
//        //        // End Track #6421 - stodd

//        //        // BEGIN TT#739-MD - STodd - delete stores
//        //        //DataTable dtTables = _storeData.StoreGroupLevel_InUseGetTables(); 	//  TT#739-MD - STodd - delete stores
//        //        SystemData sysData = new SystemData();
//        //        // END TT#739-MD - STodd - delete stores
				
//        //        int groupCnt = this._storeGroupList.Count;
//        //        ArrayList keysToDeleteList = new ArrayList();
//        //        for (int i = 0; i < groupCnt; i++)
//        //        {
//        //            StoreGroupProfile aGroup = (StoreGroupProfile)_storeGroupList[i];
//        //            if (aGroup.IsDynamicGroup)
//        //            {
//        //                int levelCnt = aGroup.GroupLevels.Count;
//        //                for (int j = 0; j < levelCnt; j++)
//        //                {
//        //                    StoreGroupLevelProfile aLevel = (StoreGroupLevelProfile)aGroup.GroupLevels[j];
//        //                    //Begin TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
//        //                    //if (aLevel.Name != _availableStoresText) // don't remove avaliable stores set
//        //                    if (aLevel.Sequence != int.MaxValue) // don't remove avaliable stores set
//        //                    //End TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
//        //                    {
//        //                        if (aLevel.Stores.Count == 0)
//        //                        {
						
//        //                            emptyCnt++;		// TT#739-MD - STodd - delete stores

//        //                            // BEGIN TT#739-MD - STodd - delete stores
//        //                            //MIDDbParameter[] inParams = { new MIDDbParameter("@SGL_RID", aLevel.Key, eDbType.Int, eParameterDirection.Input)
//        //                            //        };
//        //                            //MIDDbParameter[] outParams = { new MIDDbParameter("@ReturnCode", 0, eDbType.Int) };
//        //                            //outParams[0].Direction = eParameterDirection.Output;

//        //                            // BEGIN TT#739-MD - STodd - delete stores
//        //                            // There's no updating going on, but the execute requires an update connection.
//        //                            //_storeData.OpenUpdateConnection();

//        //                            //bool inUse = IsStoreGroupLevelInUsed(aLevel.Key, dtTables);
//        //                            bool inUse = true;
//        //                            bool AllowDelete = false;
//        //                            sysData.GetInUseData(aLevel.Key, (int)eProfileType.StoreGroupLevel, out AllowDelete);

//        //                            if (AllowDelete)
//        //                            {
//        //                                inUse = false;
//        //                            }

//        //                            //int canDelete = _storeData.ExecuteStoredProcedure("SP_MID_STOREGROUPLEVEL_IN_USE", inParams, outParams);
//        //                            // End TT#974
//        //                            //_storeData.CloseUpdateConnection();
//        //                            // END TT#739-MD - STodd - delete stores

//        //                            // BEGIN TT#739-MD - STodd - delete stores
//        //                            if (inUse)
//        //                            {
//        //                                if (_audit != null)
//        //                                {
//        //                                    SetInUse = true;
//        //                                    string msg = "Dynamic Store set, " + aLevel.Name + ", contains no stores. It cannot be purged because it is in use. Use 'In Use' menu option on Store Explorer to see where it is being used.";
//        //                                    _audit.Add_Msg(eMIDMessageLevel.Information, msg, this.ToString());
//        //                                }
//        //                            }
//        //                            else
//        //                            // END TT#739-MD - STodd - delete stores
//        //                            {
//        //                                string logMsg = "Removed empty dynamic attribute set (attr / attr set): " + aGroup.Name + " / " + aLevel.Name;
//        //                                Debug.WriteLine(logMsg);
//        //                                keysToDeleteList.Add(aLevel.Key);
//        //                            }
//        //                            // END TT#739-MD - STodd - delete stores
//        //                            // End Track #6421 - stodd
//        //                        }
//        //                    }
//        //                }
//        //            }
//        //        }

//        //        // do actual deleting...
//        //        int commitCnt = 0;
//        //        _storeData.OpenUpdateConnection();
//        //        foreach (int aKey in keysToDeleteList)
//        //        {
//        //            try
//        //            {
//        //                DeleteGroupLevelGlobal(aKey);
//        //                delCnt++;
//        //                commitCnt++;
//        //                if (commitCnt > 300)
//        //                {
//        //                    _storeData.CommitData();
//        //                    commitCnt = 0;
//        //                }
//        //            }
//        //            catch (Exception ex)
//        //            {
//        //                Debug.WriteLine("could not delete set. " + ex.Message);
//        //                _storeData.OpenUpdateConnection();
//        //                // swallow exception
//        //            }
//        //        }
//        //        _storeData.CommitData();
//        //        _storeData.CloseUpdateConnection();

//        //        numGroupsRemoved = keysToDeleteList.Count;	// TT#739-MD - STodd - delete stores

//        //    // Begin Track #6421 - stodd
//        //        // BEGIN TT#739-MD - STodd - delete stores
//        //        //string msg = "The Store Service removed " + delCnt.ToString() + " empty, unused Attribute Sets.";
//        //        //Debug.WriteLine(msg);
//        //        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
//        //        //_audit.Add_Msg(eMIDMessageLevel.Information, msg, this.ToString());
//        //        if (_audit != null)
//        //        {
//        //            string msg = emptyCnt.ToString() + " empty Store Attribute Sets found. " + delCnt.ToString() + " were unused and were deleted.";
//        //            _audit.Add_Msg(eMIDMessageLevel.Information, msg, this.ToString());
//        //        }
//        //        //else
//        //        //{
//        //        //    EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Information);
//        //        //}
//        //        // END TT#739-MD - STodd - delete stores
//        //        // End TT#189
//        //        return SetInUse;	// TT#739-MD - STodd - delete stores
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        string message = ex.ToString();
//        //        throw;
//        //    }
//        //    finally
//        //    {
//        //        // BEGIN TT#739-MD - STodd - delete stores
//        //        //EventLog.WriteEntry("MIDStoreService", "MIDStoreService:StoreServerGlobal - " + emptyCnt.ToString() + " empty Store Attribute Sets found. " + delCnt.ToString() + " were unused and were deleted.", EventLogEntryType.Information);
//        //        //EventLog.WriteEntry("MIDStoreService", "MIDStoreService:StoreServerGlobal removed " + delCnt.ToString() + " empty, unused Attribute Sets.", EventLogEntryType.Information);
//        //        // END TT#739-MD - STodd - delete stores
//        //    }
//        //    // End Track #6421 - stodd
//        //}

//        // BEGIN TT#739-MD - STodd - delete stores
//        //public bool IsStoreGroupLevelInUsed(int sglRid, DataTable dtTables)
//        //{
//        //    bool inUse = false;

//        //    foreach (DataRow aRow in dtTables.Rows)
//        //    {
//        //        string aTable = aRow["TableName"].ToString();
//        //        string aColumn = aRow["ColumnName"].ToString();

//        //        inUse =_storeData.StoreGroupLevel_IsInUseInTable(sglRid, aTable, aColumn);
//        //        if (inUse)
//        //        {
//        //            break;
//        //        }
//        //    }
//        //    return inUse;
//        //}
//        // END TT#739-MD - STodd - delete stores

//        //public ArrayList CheckForDuplicateGroupLevelNames()
//        //{
//        //    int groupCnt = this._storeGroupList.Count;
//        //    ArrayList dupList = new ArrayList();
//        //    ArrayList tempList = new ArrayList();
//        //    for (int i=0;i<groupCnt;i++)
//        //    {
//        //        StoreGroupProfile aGroup = (StoreGroupProfile)_storeGroupList[i];
				
//        //        tempList.Clear();

//        //        int levelCnt = aGroup.GroupLevels.Count;
//        //        for (int j=0;j<levelCnt;j++)
//        //        {
//        //            StoreGroupLevelProfile aLevel = (StoreGroupLevelProfile)aGroup.GroupLevels[j];
//        //            if (tempList.Contains(aLevel.Name))
//        //            {
//        //                if (!dupList.Contains(aGroup.Name))
//        //                {
//        //                    dupList.Add(aGroup.Name);
//        //                }
//        //            }
//        //            else
//        //                tempList.Add(aLevel.Name);
//        //        }
//        //    }

//        //    return dupList;
//        //}

//        internal void FillGroupsWithStores()
//        {
//            DateTime beginTime = System.DateTime.Now;
//            string errorMsg = "Filling Groups with Stores... -- " + System.Convert.ToString(beginTime, CultureInfo.CurrentUICulture);
            
//            // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
//            //_audit.Add_Msg(eMIDMessageLevel.Information, errorMsg, this.ToString());
//            if (_audit != null)
//            {
//                _audit.Add_Msg(eMIDMessageLevel.Information, errorMsg, this.ToString());
//            }
//            else
//            {
//                EventLog.WriteEntry("MIDStoreService", errorMsg, EventLogEntryType.Information);
//            }
//            // End TT#189

//            Debug.WriteLine(errorMsg);

//            int groupCnt = this._storeGroupList.Count;
//            for (int i=0;i<groupCnt;i++)
//            {
//                StoreGroupProfile currGroup = (StoreGroupProfile)_storeGroupList[i];

//                GetStoresInGroup(currGroup);
//            }
//            DateTime endTime = System.DateTime.Now;
//            errorMsg = "Filling Groups with Stores Completed -- " + System.Convert.ToString(endTime.Subtract(beginTime), CultureInfo.CurrentUICulture);
//            Debug.WriteLine(errorMsg);
//            // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
//            //_audit.Add_Msg(eMIDMessageLevel.Information, errorMsg, this.ToString());
//            if (_audit != null)
//            {
//                _audit.Add_Msg(eMIDMessageLevel.Information, errorMsg, this.ToString());
//            }
//            else
//            {
//                EventLog.WriteEntry("MIDStoreService", errorMsg, EventLogEntryType.Information);
//            }
//            // End TT#189
//        }

//        // Begin Issue 3910 - stodd
//        /// <summary>
//        /// Sorts the Store Group Levels within each store group by it's sequence number
//        /// </summary>
//        //internal void RefreshSortOfGroupLevels()
//        //{
//        //    //StoreServerGlobal.AcquireStoreGroupWriterLock();
//        //    // Begin TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//        //    try
//        //    {
//        //    // End TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//        //        int cnt = _storeGroupList.Count;
//        //        for (int i = 0; i < cnt; i++)
//        //        {
//        //            StoreGroupProfile sgp = (StoreGroupProfile)_storeGroupList[i];
//        //            sgp.GroupLevels.ArrayList.Sort(new SGLSequenceComparer());
//        //        }
//        //        // Begin TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//        //        //StoreServerGlobal.ReleaseStoreGroupWriterLock();
//        //    }
//        //    finally
//        //    {
//        //        //StoreServerGlobal.ReleaseStoreGroupWriterLock();
//        //    }
//        //    // End TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//        //}
//        // End issue 3910 

//        /// <summary>
//        /// Refreshes the store lists in all of the group levels of ALL of the defined groups
//        /// </summary>
//        //internal void RefreshStoresInAllGroups(eStoreDisplayOptions displayOption, bool refreshOnlyFilledGroups, bool refreshOnlyDynamicGroups)
//        //{
//        //    try
//        //    {
//        //        this._globalStoreDisplayOption = displayOption;

//        //        int groupCnt = _storeGroupList.Count;
//        //        // Begin TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//        //        StoreServerGlobal.AcquireCompleteWriterLock();
//        //        try
//        //        {
//        //        // End TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//        //            for (int i = 0; i < groupCnt; i++)
//        //            {
//        //                // Begin TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//        //                //StoreServerGlobal.AcquireCompleteWriterLock();
//        //                // End TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors

//        //                StoreGroupProfile sgp = (StoreGroupProfile)_storeGroupList[i];

//        //                // Begin Issue 3910 - stodd
//        //                // re-sort Group Levels
//        //                sgp.GroupLevels.ArrayList.Sort(new SGLSequenceComparer());
//        //                // End Issue 3910 - stodd

//        //                if (refreshOnlyFilledGroups)
//        //                {
//        //                    if (sgp.Filled)
//        //                    {
//        //                        if (refreshOnlyDynamicGroups)
//        //                        {
//        //                            if (sgp.IsDynamicGroup)
//        //                                RefreshStoresInGroup(sgp.Key);
//        //                        }
//        //                        else
//        //                            RefreshStoresInGroup(sgp.Key);
//        //                    }
//        //                }
//        //                else
//        //                {
//        //                    if (refreshOnlyDynamicGroups)
//        //                    {
//        //                        if (sgp.IsDynamicGroup)
//        //                        {
//        //                            RefreshStoresInGroup(sgp.Key);
//        //                        }
//        //                    }
//        //                    else
//        //                        RefreshStoresInGroup(sgp.Key);
//        //                }

//        //                // Begin TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//        //                //StoreServerGlobal.ReleaseCompleteWriterLock();
//        //                // End TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//        //            }
//        //        // Begin TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//        //        }
//        //        finally
//        //        {
//        //            StoreServerGlobal.ReleaseCompleteWriterLock();
//        //        }
//        //        // End TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//        //    }
//        //    catch ( Exception err )
//        //    {
//        //        string message = err.ToString();
//        //        throw;
//        //    }

//        //}

//        ///// <summary>
//        ///// Refreshes the store lists in all of the group levels of the group
//        ///// </summary>
//        ///// <param name="groupRID"></param>
//        //internal StoreGroupProfile RefreshStoresInGroup(int groupRID)
//        //{
//        //    try
//        //    {
//        //        int aGroupLevelRID = 0;
//        //        StoreGroupProfile sgp = (StoreGroupProfile)_storeGroupList.FindKey(groupRID);
		
//        //        foreach (StoreGroupLevelProfile sgl in sgp.GroupLevels.ArrayList)
//        //        {
//        //            aGroupLevelRID = sgl.Key;
//        //            // clears out the store lists
//        //            sgl.Stores.Clear();
//        //        }

//        //        // this essentially filles the group back up
//        //        sgp.Filled = false;
//        //        GetStoresInGroup(groupRID, aGroupLevelRID);
//        //        return sgp;
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
//        //        //_audit.Log_Exception(ex);
//        //        //throw;
//        //        if (_audit != null)
//        //        {
//        //            _audit.Log_Exception(ex);
//        //        }
//        //        throw;
//        //        // End TT#189 
//        //    }
//        //}

//        //internal ProfileList GetStoresInGroup(int groupRID, int groupLevelRID)
//        //{
//        //    ProfileList returnList = new ProfileList(eProfileType.Store);
	
//        //    // Grab the selected group
//        //    StoreGroupProfile currGroup = (StoreGroupProfile)_storeGroupList.FindKey(groupRID);

//        //    // If the group has already been filled in, 
//        //    // we send back the list of Store Profiles.
//        //    if (currGroup.Filled)
//        //    {
//        //        returnList = ((StoreGroupLevelProfile)_groupLevelHash[groupLevelRID]).Stores;
//        //    }
//        //    else
//        //    {
//        //        GetStoresInGroup(currGroup);

//        //        StoreGroupLevelProfile sglp = this.GetStoreGroupLevel(groupLevelRID);
//        //        returnList = sglp.Stores;
//        //    }
//        //    return returnList;
//        //}



//        internal void GetStoresInGroup(StoreGroupProfile currGroup)
//        {
	
//            if (!currGroup.Filled)
//            {
//                bool firstTime = true, resetAllStores = false;
//                // determine the stores in each group
//                // We do this to handle Dynamic Stores that might
//                // occur on more than one group
//                //
//                // Doing static stores now...
//                beginTime = System.DateTime.Now;
//                foreach(StoreGroupLevelProfile currGroupLevel in currGroup.GroupLevels)
//                {
//                    if (firstTime)
//                    {
//                        resetAllStores = true;
//                        firstTime = false;
//                    }

//                    ProfileList staticStoreList = GetStaticStoresInGroup(currGroupLevel.Key);
//                    CheckStoreList(ref staticStoreList, resetAllStores);
//                    currGroupLevel.Stores.AddRange( staticStoreList );
//                    resetAllStores = false;
//                }
//                Debug.WriteLine(currGroup.Name);
//                endTime = System.DateTime.Now;
//                Debug.WriteLine("static stores -- " + System.Convert.ToString(endTime.Subtract(beginTime), CultureInfo.CurrentUICulture));
//                beginTime2 = System.DateTime.Now;

//                // Doing dynamic stores now...
//                foreach(StoreGroupLevelProfile currGroupLevel in currGroup.GroupLevels)
//                {
//                    // Write now this store's dynamic stores are figured last
//                    //Begin TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
//                    //if (currGroupLevel.Name == _availableStoresText)
//                    if (currGroupLevel.Sequence == int.MaxValue)
//                    //End TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
//                        continue;

//                    ProfileList dynamStoreList = new ProfileList(eProfileType.Store);
//                    //string sqlStatement = string.Empty;
//                    try
//                    {
//                        //=============================================================================================
//                        // Place holders have been inserted into SQl that uses GreaterThan, LessThan, & Between.
//                        // They are disquinguished by "*SCGRID_" followed by the store char group rid (ex: *SCGRID_17)
//                        //=============================================================================================
//                        //if (currGroupLevel.SqlStatement != null)
//                        //{
//                        //    if (currGroupLevel.SqlStatement.IndexOf("*SCGRID__") >= 0)
//                        //    {
//                        //        sqlStatement = ReplaceStoreCharGroupPlaceholders(currGroupLevel.SqlStatement);
//                        //        dynamStoreList = GetDynamicStoresInGroup(sqlStatement);
//                        //    }
//                        //    else
//                        //    {
//                        //        dynamStoreList = GetDynamicStoresInGroup(currGroupLevel.SqlStatement);
//                        //    }
//                        //}
//                        //else
//                        //{
//                        //    dynamStoreList = GetDynamicStoresInGroup(currGroupLevel.SqlStatement);
//                        //}
//                    }
//                    catch (Exception ex)
//                    {
//                        string message = ex.ToString();
//                        string errMsg = "Error resolving stores in a dynamic store attribute. \r" +
//                            "Attr (key)= " + currGroup.Name + " (" + currGroup.Key.ToString() + ") \r" +  
//                            " Set (key)= " + currGroupLevel.Name + " (" + currGroupLevel.Key.ToString() + ") \r" + 
//                            //" Invalid SQL=  " + currGroupLevel.SqlStatement + "\r" +
//                            //" Invalid SQL2= " + sqlStatement + "\r" +
//                            " exception message: " + ex.ToString();
//                        EventLog.WriteEntry("MIDStoreService", errMsg, EventLogEntryType.Error);
//                        throw new MIDException (eErrorLevel.severe,	0, errMsg);
//                    }

//                    CheckStoreList(ref dynamStoreList, resetAllStores);
//                    currGroupLevel.Stores.AddRange( dynamStoreList );
					
//                }
//                endTime2 = System.DateTime.Now;
//                Debug.WriteLine("dynamic stores -- " + System.Convert.ToString(endTime2.Subtract(beginTime2), CultureInfo.CurrentUICulture));
//                beginTime3 = System.DateTime.Now;

//                // catching stores not in any Group now...
//                foreach(StoreGroupLevelProfile currGroupLevel in currGroup.GroupLevels)
//                {
//                    //Begin TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
//                    //if (currGroupLevel.Name == _availableStoresText && _availableStores.Count > 0)
//                    if (currGroupLevel.Sequence == int.MaxValue && _availableStores.Count > 0)
//                    //End TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
//                    {
//                        foreach (StoreProfile currStore in _availableStores)
//                        {
//                            currStore.DynamicStore = true;
//                        }

//                        if (currGroup.IsDynamicGroup && !IsStartUp)	// TT#1598 - stodd - store perf
//                        {
//                            // Gets the dynamic description for the group
//                            DataTable dtDynamicGroupDesc = this._storeData.StoreDynamicGroupDesc_Read(currGroup.Key);
//                            // build dynamic groups until all of the stores are in one
//                            // BEGIN TT#190 - MD - stodd - store service looping
//                            //=============================================================================================
//                            // This change is to catch when the store service is looping due to a bug or a data issue.
//                            // The _availableStores.Count is the total stores is will create new dynamic sets for.
//                            // The most time it will do this is once per store; i.e. 1 store per new dynamic set.
//                            // If it loops through the while more times than that, we have a problem.
//                            //=============================================================================================
//                            int availStoreMaxCount = _availableStores.Count;
//                            int whileCount = 0;
//                            while(_availableStores.Count > 0)
//                            {
//                                whileCount++;
//                                // Looping error
//                                if (whileCount > availStoreMaxCount)
//                                {

//                                    string msg = MIDText.GetTextOnly(eMIDTextCode.msg_StoreServiceLooping);
//                                    msg = msg.Replace("{0}",currGroup.Name);
//                                    msg = msg.Replace("{1}", currGroup.Key.ToString());
//                                    msg = msg.Replace("{2}", currGroupLevel.Name);
//                                    msg = msg.Replace("{3}", currGroupLevel.Key.ToString());
//                                    msg = msg.Replace("{4}", ((StoreProfile)_availableStores[0]).StoreId);
//                                    msg = msg.Replace("{5}", ((StoreProfile)_availableStores[0]).Key.ToString());

//                                    //string errMsg = "Looping problem resolving leftover (available) stores in a dynamic store attribute. \r" +
//                                    //    "Attr (key)= " + currGroup.Name + " (" + currGroup.Key.ToString() + ") \r" +
//                                    //    " Set (key)= " + currGroupLevel.Name + " (" + currGroupLevel.Key.ToString() + ") \r" +
//                                    //    " Store (Key)  " + ((StoreProfile)_availableStores[0]).StoreId + " (" + ((StoreProfile)_availableStores[0]).Key.ToString() + ") \r";
//                                    currGroupLevel.Stores.AddRange(_availableStores);
//                                    _availableStores.Clear();
//                                    EventLog.WriteEntry("MIDStoreService", msg, EventLogEntryType.Warning);
//                                    continue;
//                                }
//                                // EMD TT#190 - MD - stodd - store service looping
								
//                                if (dtDynamicGroupDesc == null) // no definition found
//                                {
//                                    currGroupLevel.Stores.AddRange(_availableStores);
//                                    ProfileList dynamStoreList = new ProfileList(eProfileType.Store, _availableStores);
//                                    CheckStoreList(ref dynamStoreList, resetAllStores);
//                                }
//                                else
//                                {
//                                    StoreGroupLevelProfile newGroupLevel = ProcessNewDynamicGroup(dtDynamicGroupDesc, currGroup.Key);   
//                                    // see what stores fit into the new group

//                                    //TODO -Attribute Set Filter
//                                    //ProfileList dynamStoreList = GetDynamicStoresInGroup(null); //GetDynamicStoresInGroup(newGroupLevel.SqlStatement);

//                                    // Is this a 'Null' set?  If so, we really want these in the 'Available Stores' set
//                                    string[] strArray = newGroupLevel.Name.Split(new Char[] {':'});
//                                    bool nullSet = true;
//                                    foreach(string aString in strArray)
//                                    {
//                                        string aTrimmedString = aString.Trim();
//                                        if (aTrimmedString.ToUpper() == "NULL" || aTrimmedString == string.Empty)
//                                        {
									
//                                        }
//                                        else
//                                        {
//                                            nullSet = false;
//                                            break;
//                                        }
//                                    }
//                                    // Set IS a 'Null' set
//                                    if (nullSet)
//                                    {
//                                        //CheckStoreList(ref dynamStoreList, resetAllStores);
//                                        //currGroupLevel.Stores.AddRange( dynamStoreList );
//                                        //_storeData.OpenUpdateConnection();
//                                        //this.DeleteGroupLevel(newGroupLevel.Key);
//                                        //_storeData.CommitData();
//                                        //_storeData.CloseUpdateConnection();
//                                    }
//                                    else
//                                    {
//                                        //if (dynamStoreList.Count != 0)
//                                        //{
//                                            //CheckStoreList(ref dynamStoreList, resetAllStores);
//                                            //newGroupLevel.Stores.AddRange( dynamStoreList );
//                                        //}
//                                            // During the ProcessNewDynamicGroup() method we construct the SqlStatement 
//                                            // to find stores, using the stores themselves.  So if we don't find any matches,
//                                            // there is a problem.
//                                        //else  
//                                        //{
//                                            //										string errMsg = "Problem encountered during dynamic set processing. ";
//                                            //										errMsg += "The remaining stores will be added to the Available Stores set. ";
//                                            //										diagResult = _SAB.MessageCallback.HandleMessage(
//                                            //											errMsg,
//                                            //											"Dynamic Attribute Set processing",
//                                            //											System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);

//                                            currGroupLevel.Stores.AddRange(_availableStores);
//                                            _availableStores.Clear();
//                                        //}
//                                    }
//                                }
//                            }
//                        }
//                        else
//                        {
//                            currGroupLevel.Stores.AddRange(_availableStores);
//                        }

//                        break;
//                    }
//                }
//                endTime3 = System.DateTime.Now;
//                Debug.WriteLine("dyn sets -- " + System.Convert.ToString(endTime3.Subtract(beginTime3), CultureInfo.CurrentUICulture));

//                currGroup.Filled = true;
//            }
//        }

//        //private string ReplaceStoreCharGroupPlaceholders(string sql)
//        //{
//        //    string newSql = sql;

//        //    try
//        //    {
//        //        string [] aList = sql.Split(' ');
//        //        foreach (string aToken in aList)
//        //        {
//        //            if (aToken.StartsWith("*SCGRID__"))
//        //            {
//        //                string ridPart = aToken.Remove(0,9);
//        //                int scgRid = Convert.ToInt32(ridPart,CultureInfo.CurrentUICulture);
//        //                string charGroupName = this._characteristics.GetCharacteristicGroupID(scgRid);
//        //                charGroupName = charGroupName.Replace("]",@"\]");
//        //                charGroupName = "[" + charGroupName + "]";  // done in case name contains comma
//        //                newSql = newSql.Replace(aToken,charGroupName);
//        //            }
//        //        }
//        //    }
//        //    catch (Exception)
//        //    {
//        //        //throw new MIDException (eErrorLevel.severe,	0, "Error processing sql " + sql, ex.InnerException);				
//        //        throw;
//        //    }

//        //    return newSql;
//        //}


//        /// <summary>
//        /// When stores for a dynamic group are requested and stores are left over (not in any currently defined
//        /// group levels (sets)), the stores need to be interigated and new group levels (sets) made from the 
//        /// store data.
//        /// </summary>
//        /// <param name="dtDynamicGroup"></param>
//        /// <param name="groupRID"></param>
//        /// <returns></returns>
//        private StoreGroupLevelProfile ProcessNewDynamicGroup(DataTable dtDynamicGroup, int groupRID)
//        {
//            try
//            {
//                ArrayList charArray = new ArrayList();
//                //ArrayList statementItems = new ArrayList();


//                // Unload each store characteristic that the Group is defined using
//                // into an array.
//                for (int iRow = 0; iRow < dtDynamicGroup.Rows.Count; iRow++)
//                {
//                    DataRow dr = dtDynamicGroup.Rows[iRow];
//                    charArray.Add(Convert.ToInt32(dr["SDGD_CHAR_ID"], CultureInfo.CurrentUICulture));
//                }

//                StoreProfile sp = (StoreProfile)_availableStores[0];
//                //int scgRID;
//                string groupLevelName = "";
//                //string textValue = "";
//                //bool isNull = false;
//                //for (int iChar = 0; iChar < charArray.Count; iChar++)
//                //{
//                    //StoreGroupLevelStatementItem sglsi = new StoreGroupLevelStatementItem();
//                //}
                 

//                //*********************************
//                // ADD Store Group Level Record
//                //*********************************
//                _storeData.OpenUpdateConnection();
//                int groupLevelRID = AddGroupLevel(groupRID, groupLevelName);

//                //***************************************
//                // update store group level RIDs on statement items
//                // AND add them to the DB
//                //***************************************
//                //foreach (StoreGroupLevelStatementItem sglsi in statementItems)
//                //{
//                //    sglsi.Sgl_rid = groupLevelRID;
//                //}
//                //// add 
//                //AddGroupLevelStatement(statementItems);

//                _storeData.CommitData();
//                _storeData.CloseUpdateConnection();

//                return (StoreGroupLevelProfile)_groupLevelHash[groupLevelRID];
//            }
//            catch (Exception ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
//                //_audit.Log_Exception(ex);
//                //throw;
//                if (_audit != null)
//                {
//                    _audit.Log_Exception(ex);
//                }
//                throw;
//                // End TT#189 
//            }
//            finally
//            {
//            }

//        }

//        /// <summary>
//        /// Returns this store's value for the characteristic named.
//        /// </summary>
//        /// <param name="CharacteristicName"></param>
//        /// <returns></returns>
//        //public object GetStoreCharacteristicValue(int storeKey, string CharacteristicName)
//        //{
//        //    object returnValue = null;
//        //    ArrayList charList = (ArrayList)this._allStoreCharacteristicsHash[storeKey];

//        //    for (int i=0;i<charList.Count;i++)
//        //    {
//        //        StoreCharGroupProfile scgp = (StoreCharGroupProfile)charList[i];
//        //        if (scgp.Name == CharacteristicName)
//        //        {
//        //            returnValue = scgp.CharacteristicValue.CharValue;
//        //            break;
//        //        }
//        //    }

//        //    return returnValue;
//        //}

//        /// <summary>
//        /// Returns this store's value for the characteristic group RID sent.
//        /// </summary>
//        /// <param name="CharacteristicGroupRID"></param>
//        /// <returns></returns>
//        //public object GetStoreCharacteristicValue(int storeKey, int CharacteristicGroupRID)
//        //{
//        //    object returnValue = null;
//        //    ArrayList charList = (ArrayList)this._allStoreCharacteristicsHash[storeKey];

//        //    for (int i=0;i<charList.Count;i++)
//        //    {
//        //        StoreCharGroupProfile scgp = (StoreCharGroupProfile)charList[i];
//        //        if (scgp.Key == CharacteristicGroupRID)
//        //        {
//        //            returnValue = scgp.CharacteristicValue.CharValue;
//        //            break;
//        //        }
//        //    }

//        //    return returnValue;
//        //}

//        /// <summary>
//        /// Returns this store's characteristic value's RID for the characteristic group RID sent.
//        /// </summary>
//        /// <param name="CharacteristicGroupRID"></param>
//        /// <returns></returns>
//        //public int GetStoreCharacteristicRID(int storeKey, int CharacteristicGroupRID)
//        //{
//        //    int returnValue=0;
//        //    ArrayList charList = (ArrayList)this._allStoreCharacteristicsHash[storeKey];

//        //    for (int i=0;i<charList.Count;i++)
//        //    {
//        //        StoreCharGroupProfile scgp = (StoreCharGroupProfile)charList[i];
//        //        if (scgp.Key == CharacteristicGroupRID)
//        //        {
//        //            returnValue = scgp.CharacteristicValue.SC_RID;
//        //            break;
//        //        }
//        //    }

//        //    return returnValue;
//        //}

//        //internal ProfileList GetDynamicStoresInGroup(string filter)
//        //{
//        //    ProfileList filteredStoreList = new ProfileList(eProfileType.Store);
	
//        //    // get filtered subset of store data table
//        //    try
//        //    {
//        //        if (filter != null)
//        //        {
//        //            DataRow [] rows = _dtAllStores.Select(filter);
//        //            // Move dataTable of Store Rows to a list of StoreProfiles
//        //            foreach(DataRow selectedStoreRow in rows)
//        //            {
//        //                int key = Convert.ToInt32( selectedStoreRow["ST_RID"], CultureInfo.CurrentUICulture );
//        //                StoreProfile sp = (StoreProfile)_allStoreList.FindKey(key);
//        //                StoreProfile spClone = (StoreProfile)sp.Clone();
//        //                spClone.DynamicStore = true;
//        //                filteredStoreList.Add(spClone);

//        //            }
//        //        }
//        //    }
//        //    catch ( Exception err )
//        //    {
//        //        string message = err.ToString();
//        //        throw;
//        //    }

//        //    return filteredStoreList;
//        //}

//        internal ProfileList GetStaticStoresInGroup(int groupLevelRid)
//        {
//            ProfileList staticStoreList = new ProfileList(eProfileType.Store);

//            StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)_groupLevelHash[groupLevelRid];

//            //Begin Track #5727 - KJohnson - Object Reference Not Set Error
//            if (sglp != null)
//            {
//                foreach (int stRid in sglp.StaticStoreList)
//                {
//                    StoreProfile sp = (StoreProfile)_allStoreList.FindKey(stRid);
//                    StoreProfile spClone = (StoreProfile)sp.Clone();
//                    spClone.DynamicStore = false;
//                    staticStoreList.Add(spClone);
//                }
//            }
//            //End Track #5727 - KJohnson

//            return staticStoreList;
//        }

//        //**********************
//        // STORE GROUP Methods
//        //**********************

//        //private StoreGroupProfile NewStoreGroup(int RID, string name, bool isDynamic, ref int prevGroupRID, ref bool addGroup)
//        //{
//        //    StoreGroupProfile stGroup = new StoreGroupProfile(RID);
//        //    stGroup.Name = name;
//        //    stGroup.IsDynamicGroup = isDynamic;
//        //    prevGroupRID = RID;
//        //    addGroup = true;
//        //    return stGroup;
//        //}

//        // Begin Track #4872 - JSmith - Global/User Attributes
//        //internal int AddGroup(string groupName, bool isDynamic)
//        internal int AddGroup(string groupName, bool isDynamic, int aUserRID, int filterRID) //TT#1414-MD -jsobek -Attribute Set Filter
//        // End Track #4872
//        {
//            try
//            {
//                // DB
//                // Begin Track #4872 - JSmith - Global/User Attributes
//                //int groupRID = _storeData.StoreGroup_Insert(groupName, isDynamic);
//                StoreGroupMaint storeGroupMaint = new StoreGroupMaint();
//                int groupRID = storeGroupMaint.StoreGroup_InsertAndAddUserItem(groupName, isDynamic, aUserRID, filterRID); //TT#1414-MD -jsobek -Attribute Set Filter
//                // End Track #4872

//                // Add to Store Group List
//                StoreGroupProfile sg = new StoreGroupProfile(groupRID);
//                sg.Name = groupName;
//                sg.IsDynamicGroup = isDynamic;
//                // Begin Track #4872 - JSmith - Global/User Attributes
//                sg.OwnerUserRID = aUserRID;
//                // End Track #4872

//                sg.FilterRID = filterRID; //TT#1414-MD -jsobek -Attribute Set Filter

//                _storeGroupList.Add(sg);

//                // Begin TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//                SortStoreGroupList();
//                // End TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors

//                return groupRID;
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        // Begin Track #4872 - JSmith - Global/User Attributes
//        //internal bool DoesGroupNameExist(string name)
////        internal bool DoesGroupNameExist(string name, int aUserRID)
////        // End Track #4872
////        {
////            bool exists = false;
////            int groupCnt = _storeGroupList.ArrayList.Count;
////            for (int i=0;i<groupCnt;i++)
////            {
////                StoreGroupProfile sgp = (StoreGroupProfile)_storeGroupList[i];
//////Begin Track #4880 - JScott - Error encountered when selecting "Region" attribute
//////				if (sgp.Name == name)
////                // Begin Track #4872 - JSmith - Global/User Attributes
////                //if (sgp.Name.ToLower() == name.ToLower())
////                if (sgp.Name.ToLower() == name.ToLower() &&
////                    sgp.OwnerUserRID == aUserRID)
////                // End Track #4872
//////End Track #4880 - JScott - Error encountered when selecting "Region" attribute
////                {
////                    exists = true;
////                    break;
////                }
////            }

////            return exists;
////        }

////        internal bool DoesGroupLevelNameExist(int groupRid, string name)
////        {
////            bool exists = false;

////            StoreGroupProfile sgp = (StoreGroupProfile)_storeGroupList.FindKey(groupRid);

////            int levelCnt = sgp.GroupLevels.Count;
////            for (int i=0;i<levelCnt;i++)
////            {
////                StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)sgp.GroupLevels[i];
//////Begin Track #4880 - JScott - Error encountered when selecting "Region" attribute
//////				if (sglp.Name == name)
////                if (sglp.Name.ToLower() == name.ToLower())
//////End Track #4880 - JScott - Error encountered when selecting "Region" attribute
////                {
////                    exists = true;
////                    break;
////                }
////            }

////            return exists;
////        }

//        ///// <summary>
//        ///// Returns the Store Group that contains the Store Group Level Param
//        ///// </summary>
//        ///// <param name="groupLevelRID"></param>
//        ///// <returns></returns>
//        //internal StoreGroupProfile GetGroupFromGroupLevel(int groupLevelRID)
//        //{
//        //    StoreGroupProfile storeGroup = null;
//        //    foreach(StoreGroupProfile sg in _storeGroupList)
//        //    {
//        //        if (storeGroup != null)
//        //            break;
//        //        foreach(StoreGroupLevelProfile sgl in sg.GroupLevels)
//        //        {
//        //            if (sgl.Key == groupLevelRID)
//        //            {
//        //                storeGroup = sg;
//        //                break;
//        //            }
//        //        }
//        //    }
//        //    return storeGroup;
//        //}

//        internal StoreGroupProfile GetStoreGroup(int groupRid)
//        {
//            StoreGroupProfile storeGroup = (StoreGroupProfile)_storeGroupList.FindKey(groupRid);
//            return storeGroup;
//        }

//        //internal StoreGroupProfile GetStoreGroupFilled(int groupRID)
//        //{
//        //    StoreGroupProfile storeGroup = null;
//        //    foreach(StoreGroupProfile sg in _storeGroupList)
//        //    {
//        //        if (sg.Key == groupRID)
//        //        {
//        //            storeGroup = sg;
//        //            if (!storeGroup.Filled)
//        //                this.RefreshStoresInGroup(storeGroup.Key);
//        //            break;
//        //        }
//        //    }
//        //    return storeGroup;
//        //}

//        internal void RenameGroup(int groupRID, string newName)
//        {

//            try
//            {
//                // on DB
//                _storeData.StoreGroup_Update(groupRID, newName);

//                // rename in store group list
//                StoreGroupProfile sg = (StoreGroupProfile)_storeGroupList.FindKey(groupRID);
//                sg.Name = newName;

//                // Begin TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//                SortStoreGroupList();
//                // End TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        //// Begin track #5005 stodd
//        //internal void UpdateStoreGroup(StoreGroupProfile sgp)
//        //{

//        //    try
//        //    {
//        //        // on DB
//        //        _storeData.StoreGroup_Update(sgp.Key, sgp.Name, sgp.OwnerUserRID);

//        //        StoreGroupProfile sg = (StoreGroupProfile)_storeGroupList.FindKey(sgp.Key);
//        //        sg.Name = sgp.Name;
//        //        sg.OwnerUserRID = sgp.OwnerUserRID;

//        //        // Begin TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//        //        SortStoreGroupList();
//        //        // End TT#3352 - JSmith - ANF Adults - AcquireStoreWriterLock errors
//        //    }
//        //    catch (Exception err)
//        //    {
//        //        string message = err.ToString();
//        //        throw;
//        //    }
//        //}
//        //// End Track #5005

//        //internal string GetStoreGroupName(int groupRID)
//        //{

//        //    try
//        //    {
//        //        string name = "Not Found";
//        //        StoreGroupProfile sg = (StoreGroupProfile)_storeGroupList.FindKey(groupRID);
//        //        if (sg != null)
//        //            name = sg.Name;
//        //        return name;
//        //    }
//        //    catch ( Exception err )
//        //    {
//        //        string message = err.ToString();
//        //        throw;
//        //    }
//        //}

//        //// Begin Issue 4248 - stodd
//        //internal void DeleteGroup(int groupRID)
//        //{
//        //    try
//        //    {
//        //        ArrayList groupLevelKeyList = new ArrayList();
//        //        // Do to a quirk with loop through and array AND trying to remove from that
//        //        // array, we first grab all of the keys into a second array, then do the deleting.
//        //        StoreGroupProfile sg = (StoreGroupProfile)_storeGroupList.FindKey(groupRID);
//        //        foreach (StoreGroupLevelProfile sglp in sg.GroupLevels)
//        //        {
//        //            // Goodbye Store Group Level Join records
//        //            //_storeData.StoreGroupLevelJoin_Delete(sglp.Key);
//        //            // Goodbye Store Group Level Statement records
//        //            //_storeData.StoreGroupLevelStatement_Delete(sglp.Key);
//        //            // Goodbye Store Group Level records
//        //            _storeData.StoreGroupLevel_Delete(sglp.Key);	
					
//        //            groupLevelKeyList.Add(sglp.Key);
//        //        }
//        //        _storeData.StoreGroup_Delete(groupRID);


//        //        foreach (int key in groupLevelKeyList)
//        //        {
//        //            StoreGroupLevelProfile sgl = (StoreGroupLevelProfile)sg.GroupLevels.FindKey(key);
//        //            // remove from list
//        //            sg.GroupLevels.Remove(sgl);
//        //            // remove from hash
//        //            _groupLevelHash.Remove(key);
//        //        }

//        //        // remove from list
//        //        _storeGroupList.Remove(sg);
//        //    }
//        //    catch ( Exception err )
//        //    {
//        //        string message = err.ToString();
//        //        throw;
//        //    }
//        //}
//        //// End Issue 4248 - stodd

//        //===============================
//        // STORE GROUP LEVEL Methods
//        //===============================

//        //private StoreGroupLevelProfile NewStoreGroupLevel(int groupRid, int RID, string name, int seq, ref int prevGroupLevelRID, ref bool addGroupLevel)
//        //{
//        //    StoreGroupLevelProfile stGroupLevel = new StoreGroupLevelProfile(RID);
//        //    stGroupLevel.GroupRid = groupRid;
//        //    stGroupLevel.Name = name;
//        //    prevGroupLevelRID = RID;
//        //    stGroupLevel.Sequence = seq;	
//        //    if (sql != null)
//        //        stGroupLevel.SqlStatement += " " + sql;
//        //    else 
//        //        stGroupLevel.SqlStatement = null;
//        //    addGroupLevel = true;
//        //    return stGroupLevel;
//        //}

//        internal int AddGroupLevel(int groupRID, string newName)
//        {
//            try
//            {
//                int groupLevelRid = Include.NoRID;
//                StoreGroupProfile sgp = (StoreGroupProfile)_storeGroupList.FindKey(groupRID);
//                {
//                    int sequence = 0;
//                    //Begin TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
//                    //if (newName == _availableStoresText)
//                    if (newName == Include.AvailableStoresGroupName)
//                    //End TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
//                        sequence = int.MaxValue;
//                    else
//                        sequence = GetNextGroupLevelSequence(groupRID);
//                    // Add to DB
//                    groupLevelRid = AddGroupLevelDB(groupRID, newName, ref sequence);
//                    // add to group list
//                    StoreGroupLevelProfile sgl = new StoreGroupLevelProfile(groupLevelRid);
//                    sgl.GroupRid = groupRID;
//                    sgl.Name = newName;
//                    sgl.Sequence = sequence;
//                    sgp.GroupLevels.Add(sgl);
//                    // add to hash table
//                    _groupLevelHash.Add(sgl.Key, sgl);
//                }
//                return groupLevelRid;
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }


//        internal int AddGroupLevelDB(int groupRID, string newName, ref int sequence)
//        {
//            try
//            {
//                //BeginTT#1310-MD -jsobek -Error when adding a new Store
//                //if (newName == null)
//                //{
//                //    newName = Include.AvailableStoresGroupName;
//                //}
//                //End TT#1310-MD -jsobek -Error when adding a new Store

//                int groupLevelRID = _storeData.StoreGroupLevel_Insert(sequence, groupRID, newName);

//                return groupLevelRID;
//            }
//            catch ( Exception err )
//            {
//                string message = err.ToString();
//                throw;
//            }
//        }

//        internal int GetGroupLevelSequence(int storeGroupLevelKey)
//        {
//            StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)_groupLevelHash[storeGroupLevelKey];
//            return sglp.Sequence;
//        }


//        internal int GetNextGroupLevelSequence(int SG_RID)
//        {
//            int maxSeq = 0;

//            StoreGroupProfile sgp = (StoreGroupProfile)_storeGroupList.FindKey(SG_RID);

//            int levelCnt = sgp.GroupLevels.Count;
//            for(int i=0;i<levelCnt;i++)
//            {
//                StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)sgp.GroupLevels[i];
//                //Begin TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
//                // Begin Track #6301 - JSmith - Select PreOpen results in Error
//                ////if (sglp.Name == _availableStoresText)
//                //if (sglp.Name == _availableStoresText &&
//                //    sglp.Sequence == int.MaxValue)
//                //// End Track #6301
//                if (sglp.Sequence == int.MaxValue)
//                //End TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
//                {
//                    // skip it
//                }
//                else
//                {
//                    if (sglp.Sequence > maxSeq)
//                        maxSeq = sglp.Sequence;
//                }
//            }

//            maxSeq++;
//            return maxSeq;
//        }

//        //internal StoreGroupLevelProfile GetStoreGroupLevel(int groupLevelRID)
//        //{
//        //    StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)_groupLevelHash[groupLevelRID];
//        //    StoreGroupProfile sgp = (StoreGroupProfile)_storeGroupList.FindKey(sglp.GroupRid);
//        //    if (!sgp.Filled)
//        //    {
//        //        this.RefreshStoresInGroup(sgp.Key);
//        //    }
//        //    return sglp;
//        //}

//        //internal StoreGroupLevelListViewProfile GetStoreGroupLevelListView(int groupLevelRID)
//        //{
//        //    StoreGroupLevelListViewProfile storeGroupLevelListView = null;
//        //    foreach(StoreGroupProfile sg in _storeGroupList)
//        //    {
//        //        if (storeGroupLevelListView != null)
//        //            break;
//        //        foreach(StoreGroupLevelProfile sgl in sg.GroupLevels)
//        //        {
//        //            if (sgl.Key == groupLevelRID)
//        //            {
//        //                storeGroupLevelListView = new StoreGroupLevelListViewProfile(sgl.Key);
//        //                storeGroupLevelListView.Name = sgl.Name;
//        //                storeGroupLevelListView.GroupRid = sgl.GroupRid;
//        //                storeGroupLevelListView.Sequence = sgl.Sequence;
//        //                break;
//        //            }
//        //        }
//        //    }
//        //    return storeGroupLevelListView;
//        //}

//        //internal void RenameGroupLevel(int groupLevelRID, string newName)
//        //{
//        //    try
//        //    {
//        //        // on DB
//        //        RenameGroupLevelDB(groupLevelRID, newName);
//        //        // in store group list and hash
//        //        StoreGroupLevelProfile sgl = (StoreGroupLevelProfile)_groupLevelHash[groupLevelRID];
//        //        sgl.Name = newName;
//        //    }
//        //    catch ( Exception err )
//        //    {
//        //        string message = err.ToString();
//        //        throw;
//        //    }
//        //}

//        //internal void RenameGroupLevelDB(int groupLevelRID, string newName)
//        //{
//        //    try
//        //    {
//        //        _storeData.StoreGroupLevel_Update(groupLevelRID, newName);
//        //    }
//        //    catch ( Exception err )
//        //    {
//        //        string message = err.ToString();
//        //        throw;
//        //    }
//        //}
		

//        //internal void UpdateGroupLevelSequence(int groupLevelRID, int sglSeq)
//        //{
//        //    try
//        //    {
//        //        // update database
//        //        _storeData.StoreGroupLevelStatement_UpdateSequence(groupLevelRID, sglSeq);

//        //        // update store group list and hash
//        //        StoreGroupLevelProfile sgl = (StoreGroupLevelProfile)_groupLevelHash[groupLevelRID];
//        //        sgl.Sequence = sglSeq;
//        //    }
//        //    catch ( Exception err )
//        //    {
//        //        string message = err.ToString();
//        //        throw;
//        //    }
//        //}

//        // Begin Issue 4067 - stodd
//        /// <summary>
//        /// Sorts group levels in order according to seq 
//        /// </summary>
//        /// <param name="storeGroupRid"></param>
//        //internal void SortGroupLevels(int storeGroupRid)
//        //{	
//        //    StoreGroupProfile sg = this.GetStoreGroup(storeGroupRid);

//        //    sg.GroupLevels.ArrayList.Sort(new SGLSequenceComparer());
//        //}
//        // End Issue 4067 - stodd

//        //internal void DeleteGroupLevel(int groupLevelRID)
//        //{

//        //    try
//        //    {
//        //        // Goodbye Store Group Level Join records
//        //        //DeleteGroupLevelJoin(groupLevelRID);
//        //        // Goodbye Store Group Level Statement records
//        //        //DeleteGroupLevelStatementDB(groupLevelRID);
//        //        // Goodbye Store Group Level records
//        //        //DeleteGroupLevelDB(groupLevelRID);

//        //        StoreGroupProfile sg = this.GetGroupFromGroupLevel(groupLevelRID);
//        //        StoreGroupLevelProfile sgl = (StoreGroupLevelProfile)sg.GroupLevels.FindKey(groupLevelRID);
					
//        //        // remove from list
//        //        sg.GroupLevels.Remove(sgl);
//        //        // remove from hash
//        //        _groupLevelHash.Remove(groupLevelRID);
//        //    }
//        //    catch ( Exception err )
//        //    {
//        //        string message = err.ToString();
//        //        throw;
//        //    }
//        //}

//        //internal void DeleteGroupLevelGlobal(int groupLevelRID)
//        //{

//        //    try
//        //    {
//        //        // Goodbye Store Group Level Join records
//        //        // delete from Database
//        //        //_storeData.StoreGroupLevelJoin_Delete(groupLevelRID);
//        //        // Goodbye Store Group Level Statement records
//        //        //_storeData.StoreGroupLevelStatement_Delete(groupLevelRID);
//        //        // Goodbye Store Group Level records
//        //        _storeData.StoreGroupLevel_Delete(groupLevelRID);	

//        //        StoreGroupProfile sg = this.GetGroupFromGroupLevel(groupLevelRID);
//        //        StoreGroupLevelProfile sgl = (StoreGroupLevelProfile)sg.GroupLevels.FindKey(groupLevelRID);
					
//        //        // remove from list
//        //        sg.GroupLevels.Remove(sgl);
//        //        // remove from hash
//        //        _groupLevelHash.Remove(groupLevelRID);
//        //    }
//        //    catch ( Exception err )
//        //    {
//        //        _storeData.Rollback();
//        //        string message = err.ToString();
//        //        throw;
//        //    }
//        //}

//        //internal void DeleteGroupLevelDB(int groupLevelRID)
//        //{
//        //    try
//        //    {
//        //        _storeData.StoreGroupLevel_Delete(groupLevelRID);	
//        //    }
//        //    catch ( Exception err )
//        //    {
//        //        string message = err.ToString();
//        //        throw;
//        //    }
//        //}

//        ///// <summary>
//        ///// Returns an ArrayList of Group Level RIDs that belong to the Group
//        ///// </summary>
//        ///// <param name="groupRID">Group RID.</param>
//        ///// <returns>ArrayList of int's.</returns>
//        //internal ArrayList GetGroupLevelsInGroup(int groupRID)
//        //{
//        //    ArrayList arGroupLevels = new ArrayList();
//        //    StoreGroupProfile sg = (StoreGroupProfile)_storeGroupList.FindKey(groupRID);

//        //    for (int i=0;i<sg.GroupLevels.Count;i++)
//        //    {
//        //        StoreGroupLevelProfile sgl = (StoreGroupLevelProfile)sg.GroupLevels[i];
//        //        arGroupLevels.Add( sgl.Key );
//        //    }
//        //    return arGroupLevels;
//        //}

//        ///// <summary>
//        ///// Returns an ArrayList of Group Level Profiles that belong to the Group
//        ///// </summary>
//        ///// <param name="groupRID">Group RID.</param>
//        ///// <returns>ProfileList of Store Group Level profiles.</returns>
//        //internal ProfileList GetStoreGroupLevelList(int groupRID)
//        //{
//        //    ArrayList aGroupLevels = new ArrayList();
//        //    StoreGroupProfile sg = (StoreGroupProfile)_storeGroupList.FindKey(groupRID);
//        //    // The group sent was not found, so no list can be sent back.
//        //    if (sg == null)
//        //        return null;
//        //    // this call amkes sure that all group levels within the group
//        //    // have their stores filled in.
//        //    StoreGroupLevelProfile sgli = (StoreGroupLevelProfile)sg.GroupLevels[0];
//        //    ProfileList dummyArray = this.GetStoresInGroup(sg.Key, sgli.Key);

//        //    aGroupLevels.AddRange(sg.GroupLevels.ArrayList);

//        //    aGroupLevels.Sort(new SGLSequenceComparer());

//        //    ProfileList groupLevelList = new ProfileList(eProfileType.StoreGroupLevel, aGroupLevels);

//        //    return groupLevelList;
//        //}

		
//        //internal ProfileList GetStoreGroupLevelListViewList(int groupRID, bool fillStores)
//        //{
//        //    ProfileList pl = new ProfileList(eProfileType.StoreGroupLevelListView);
//        //    StoreGroupProfile sg = null;
//        //    if (fillStores)
//        //        sg = this.GetStoreGroupFilled(groupRID);
//        //    else
//        //        sg = (StoreGroupProfile)_storeGroupList.FindKey(groupRID);
		
//        //    // The group sent was not found, so no list can be sent back.
//        //    if (sg == null)
//        //        return null;
//        //    int groupCnt = sg.GroupLevels.Count;
//        //    for (int i=0;i<groupCnt;i++)
//        //    {
//        //        StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)sg.GroupLevels[i];
//        //        StoreGroupLevelListViewProfile view = new StoreGroupLevelListViewProfile(sglp.Key);
//        //        view.GroupRid = sglp.GroupRid;
//        //        view.Name = sglp.Name;
//        //        view.Sequence = sglp.Sequence;
//        //        if (fillStores)
//        //        {
//        //            if (view.Stores == null)
//        //                view.Stores = new ProfileList(eProfileType.Store);
//        //            else
//        //                view.Stores.Clear();
//        //            int stCnt = sglp.Stores.Count;
//        //            for (int j=0;j<stCnt;j++)
//        //            {
//        //                StoreProfile sp = (StoreProfile)sglp.Stores[j];
//        //                view.Stores.Add(sp);		
//        //            }
//        //        }
//        //        pl.Add(view);
//        //    }
//        //    pl.ArrayList.Sort(new SGLLVSequenceComparer());
//        //    return pl;
//        //}

//        //======================================
//        // STORE GROUP LEVEL STATEMENT Methods
//        //======================================
//        /// <summary>
//        /// Takes an array list of StoreGroupLevelStatementItem and
//        /// Adds the sql statements to the store group list
//        /// </summary>
//        /// <param name="sglsiArray"></param>
//        //internal void AddGroupLevelStatement(ArrayList sglsiArray)
//        //{
//        //    StoreGroupLevelProfile sgl = null;
//        //    string lSQL = "";

//        //    if (sglsiArray.Count > 0)
//        //    {
//        //        StoreGroupLevelStatementItem sglItem = (StoreGroupLevelStatementItem)sglsiArray[0];
//        //        sgl = (StoreGroupLevelProfile)_groupLevelHash[sglItem.Sgl_rid];
//        //        sgl.SqlStatementList.Clear();
		
//        //        // Add to store Group
//        //        foreach (StoreGroupLevelStatementItem sglsi in sglsiArray)
//        //        {
//        //            sgl.SqlStatementList.Add(sglsi.Clone());
//        //            if (sglsi.Sequence > 1)
//        //                lSQL += " " + sglsi.Sql_statement.ToString(CultureInfo.CurrentUICulture);
//        //            else
//        //                lSQL += sglsi.Sql_statement.ToString(CultureInfo.CurrentUICulture);
//        //        }
//        //        if (sgl != null)
//        //            sgl.SqlStatement = lSQL;
		
//        //        // Add to DB and Datatable
//        //        foreach (StoreGroupLevelStatementItem sglsItem in sglsiArray)
//        //        {
//        //            _storeData.StoreGroupLevelStatement_Insert(sglsItem.Sgl_rid, sglsItem.Sequence, sglsItem.Sql_statement,
//        //                sglsItem.IsCharacteristic, sglsItem.CharId, sglsItem.SqlOperator, sglsItem.EnglishSql,
//        //                sglsItem.DataType, sglsItem.Value, sglsItem.Prefix, sglsItem.Suffix);
//        //        }
//        //    }
//        //}

//        //Begin TT#1414-MD -jsobek -Attribute Set Filter -unused function
//        ///// <summary>
//        ///// Checks to see if a Characteristic Group is used
//        ///// in any Store Group Level definitions
//        ///// </summary>
//        ///// <returns></returns>
//        //internal bool IsStoreCharGroupUsedAnywhere(int scgRid, ref InUseInfo inUseInfo)
//        //{
//        //    bool used = false;
//        //    try
//        //    {
//        //        int sgCount = this._storeGroupList.Count;
//        //        for (int i=0;i<sgCount;i++)
//        //        {
//        //            StoreGroupProfile sg = (StoreGroupProfile)_storeGroupList[i];
//        //            int sglCount = sg.GroupLevels.Count;
//        //            for (int j=0;j<sglCount;j++)
//        //            {
//        //                StoreGroupLevelProfile sgl = (StoreGroupLevelProfile)sg.GroupLevels[j];
//        //                int sglsCount = sgl.SqlStatementList.Count;
//        //                for (int k=0;k<sglsCount;k++)
//        //                {
//        //                    StoreGroupLevelStatementItem sgls = (StoreGroupLevelStatementItem)sgl.SqlStatementList[k];
//        //                    if (sgls.CharId == scgRid)
//        //                    {
//        //                        if (sg.IsDynamicGroup)
//        //                        {
//        //                            inUseInfo.AddItem(sg.GroupId, string.Empty);
//        //                        }
//        //                        else
//        //                        {
//        //                            inUseInfo.AddItem(sg.GroupId, sgl.Name);
//        //                        }
//        //                        used = true;
//        //                    }
//        //                }
//        //            }
//        //        }

//        //        if(!used)
//        //        {
//        //            // Begin Issue 4107 - stodd
//        //            DataTable dt = _storeData.IsStoreCharGroupUsedAnywhere(scgRid);
//        //            if (dt.Rows.Count > 0)
//        //            {
//        //                foreach (DataRow row in dt.Rows)
//        //                {
//        //                    string attrName = row["SG_ID"].ToString();
//        //                    inUseInfo.AddItem(attrName, string.Empty);
//        //                }
//        //                used = true;
//        //            }
//        //            // End issue 4107
//        //        }

//        //        return used;

//        //    }
//        //    catch ( Exception err )
//        //    {
//        //        string message = err.ToString();
//        //        throw;
//        //    }
//        //}
//        //End TT#1414-MD -jsobek -Attribute Set Filter -unused function

//        /// <summary>
//        /// Returns an ArrayList of the Store Group Level Statements for this
//        /// Store Group Level.  Array of StoreGroupLevelStatementItem objects.
//        /// </summary>
//        /// <param name="sglRid"></param>
//        /// <returns></returns>
//        //internal ArrayList GetStoreGroupLevelStatementList(int sglRid)
//        //{
//        //    ArrayList sglsArray = null;

//        //    try
//        //    {
//        //        int sgCount = this._storeGroupList.Count;
//        //        for (int i=0;i<sgCount;i++)
//        //        {
//        //            StoreGroupProfile sg = (StoreGroupProfile)_storeGroupList[i];
//        //            int sglCount = sg.GroupLevels.Count;
//        //            for (int j=0;j<sglCount;j++)
//        //            {
//        //                StoreGroupLevelProfile sgl = (StoreGroupLevelProfile)sg.GroupLevels[j];
//        //                if (sgl.Key == sglRid)
//        //                {
//        //                    sglsArray = sgl.SqlStatementList;
//        //                    break;
//        //                }
//        //            }
//        //            if (sglsArray != null)
//        //                break;
//        //        }

//        //        return sglsArray;
//        //    }
//        //    catch ( Exception err )
//        //    {
//        //        string message = err.ToString();
//        //        throw;
//        //    }
//        //}

//        //internal DataTable GetStoreGroupLevelStatementDtDefinition()
//        //{
//        //    DataTable dtSgls = _storeData.StoreGroupLevelStatement_DataTable();
//        //    return dtSgls;
//        //}

//        //Begin TT#1414-MD -jsobek -Attribute Set Filter -unused function
//        //internal bool DoesStoreCharGroupHaveStoreValuesAssigned(int scg_rid)
//        //{
//        //    bool used = false;

//        //    used = _storeData.DoesStoreCharGroupHaveStoreValuesAssigned(scg_rid);

//        //    return used;
//        //}
//        //End TT#1414-MD -jsobek -Attribute Set Filter -unused function

//        //internal void DeleteGroupLevelStatement(int SGL_RID)
//        //{
//        //    DeleteGroupLevelStatementDB(SGL_RID);

//        //    // deletes from hash and store group list
//        //    StoreGroupLevelProfile sgl = (StoreGroupLevelProfile)_groupLevelHash[SGL_RID];
//        //    sgl.SqlStatement = null;
//        //    sgl.SqlStatementList.Clear();
//        //}
		
//        //internal void DeleteGroupLevelStatementDB(int SGL_RID)
//        //{
//        //    try
//        //    {
//        //        _storeData.StoreGroupLevelStatement_Delete(SGL_RID);
//        //    }
//        //    catch ( Exception err )
//        //    {
//        //        string message = err.ToString();
//        //        throw;
//        //    }
//        //}

//        //===================================
//        // STORE GROUP LEVEL JOIN Methods
//        //===================================
//        //internal void AddGroupLevelJoin(int [] storeRID, int groupLevelRID)
//        //{
//        //    StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)_groupLevelHash[groupLevelRID];
//        //    for(int i=0; i<=storeRID.Length -1; i++)
//        //    {
//        //        // Add to Database
//        //        //_storeData.StoreGroupLevelJoin_Insert(storeRID[i], groupLevelRID);
//        //        // Add to static store list
//        //        sglp.StaticStoreList.Add(storeRID[i]);
//        //    }
//        //}


//        //internal void DeleteGroupLevelJoin(int SGL_RID)
//        //{
//        //    try
//        //    {
//        //        // Remove from Database
//        //        //_storeData.StoreGroupLevelJoin_Delete(SGL_RID);

//        //        // Remove from Store Group level
//        //        StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)_groupLevelHash[SGL_RID];
//        //        sglp.StaticStoreList.Clear();
//        //    }
//        //    catch ( Exception err )
//        //    {
//        //        string message = err.ToString();
//        //        throw;
//        //    }
//        //}

//        //internal void DeleteGroupLevelJoin(int sglRid, int storeRid)
//        //{
//        //    try
//        //    {
//        //        // delete from Database
//        //        //_storeData.StoreGroupLevelJoin_Delete(storeRid, sglRid);

//        //        // Remove from Store Group Level
//        //        StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)_groupLevelHash[sglRid];
//        //        try
//        //        {
//        //            sglp.StaticStoreList.Remove(storeRid);
//        //        }
//        //        catch
//        //        {
//        //        }
//        //    }
//        //    catch ( Exception err )
//        //    {
//        //        string message = err.ToString();
//        //        throw;
//        //    }
//        //}

//        //internal void AddStoreToGroupLevel(int storeRID, int groupLevelRID)
//        //{
//        //    int [] stores = new int[1];
//        //    stores[0] = storeRID;
//        //    AddGroupLevelJoin(stores, groupLevelRID);

//        //    StoreGroupProfile sg = GetGroupFromGroupLevel(groupLevelRID);
//        //    if (sg.Filled)
//        //    {
//        //        // Find the group level
//        //        StoreGroupLevelProfile sgl = (StoreGroupLevelProfile)_groupLevelHash[groupLevelRID];
//        //        // get the current store info from global
//        //        DataRow storeDataRow = this._dtAllStores.Rows.Find(storeRID);
//        //        // convert the store data row to a store profile record
//        //        StoreProfile currStore = ConvertToStoreProfile(storeDataRow, false);
//        //        // add it to the group level
//        //        sgl.Stores.Add(currStore);
//        //    }
//        //}

//        //internal void DeleteStore(int storeRID, int groupLevelRID)
//        //{
//        //    DeleteGroupLevelJoin(groupLevelRID, storeRID);

//        //    StoreGroupProfile sg = GetGroupFromGroupLevel(groupLevelRID);
//        //    if (sg.Filled)
//        //    {
//        //        // Find the group level
//        //        StoreGroupLevelProfile sgl = (StoreGroupLevelProfile)_groupLevelHash[groupLevelRID];
//        //        // search through stores and remove store
//        //        foreach (StoreProfile sp in sgl.Stores)
//        //        {
//        //            if (sp.Key == storeRID)
//        //            {
//        //                sgl.Stores.Remove(sp);
//        //                break;
//        //            }
//        //        }
//        //    }
//        //}

//        //************************
//        // STORE & MISC Methods
//        //************************
		
//        //internal StoreProfile ConvertToStoreProfile(DataRow dr, bool dynamicStore)
//        //{
//        //    int key = -1;

//        //    if (dr["ST_RID"] != DBNull.Value)
//        //        key		= Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture);

//        //    StoreProfile sp = new StoreProfile(key);

//        //    if (dr["ST_ID"] != DBNull.Value)
//        //        sp.StoreId			= (string)dr["ST_ID"];
//        //    if (dr["STORE_NAME"] != DBNull.Value)
//        //        sp.StoreName		= (string)dr["STORE_NAME"];
//        //    if (dr["STORE_DESC"] != DBNull.Value)
//        //        sp.StoreDescription		= (string)dr["STORE_DESC"];
//        //    if (dr["ACTIVE_IND"] != DBNull.Value)
//        //        sp.ActiveInd = ((string)dr["ACTIVE_IND"] == "1")? true: false;
//        //    if (dr["CITY"] != DBNull.Value)
//        //        sp.City				= (string)dr["CITY"];
//        //    if (dr["STATE"] != DBNull.Value)
//        //        sp.State			= (string)dr["STATE"];
//        //    if (dr["SELLING_SQ_FT"] != DBNull.Value)
//        //        sp.SellingSqFt		= Convert.ToInt32(dr["SELLING_SQ_FT"], CultureInfo.CurrentUICulture);
		
//        //    if (dr["SELLING_OPEN_DATE"] == DBNull.Value)
//        //        sp.SellingOpenDt = sp.SellingOpenDt;
//        //    else
//        //        sp.SellingOpenDt	= (DateTime)dr["SELLING_OPEN_DATE"];
//        //    if (dr["SELLING_CLOSE_DATE"] == DBNull.Value)
//        //        sp.SellingCloseDt = sp.SellingCloseDt;
//        //    else
//        //        sp.SellingCloseDt	= (DateTime)dr["SELLING_CLOSE_DATE"];

//        //    if (dr["STOCK_OPEN_DATE"] == DBNull.Value)
//        //        sp.StockOpenDt = sp.StockOpenDt;
//        //    else
//        //        sp.StockOpenDt	= (DateTime)dr["STOCK_OPEN_DATE"];
//        //    if (dr["STOCK_CLOSE_DATE"] == DBNull.Value)
//        //        sp.StockCloseDt = sp.StockCloseDt;
//        //    else
//        //        sp.StockCloseDt	= (DateTime)dr["STOCK_CLOSE_DATE"];

//        //    if (dr["LEAD_TIME"] != DBNull.Value)
//        //        sp.LeadTime		= Convert.ToInt32(dr["LEAD_TIME"], CultureInfo.CurrentUICulture);

//        //    if (dr["SHIP_ON_MONDAY"] != DBNull.Value)
//        //        sp.ShipOnMonday = ((string)dr["SHIP_ON_MONDAY"] == "1")? true: false;
//        //    if (dr["SHIP_ON_TUESDAY"] != DBNull.Value)
//        //        sp.ShipOnTuesday = ((string)dr["SHIP_ON_TUESDAY"] == "1")? true: false;
//        //    if (dr["SHIP_ON_WEDNESDAY"] != DBNull.Value)
//        //        sp.ShipOnWednesday = ((string)dr["SHIP_ON_WEDNESDAY"] == "1")? true: false;
//        //    if (dr["SHIP_ON_THURSDAY"] != DBNull.Value)
//        //        sp.ShipOnThursday = ((string)dr["SHIP_ON_THURSDAY"] == "1")? true: false;
//        //    if (dr["SHIP_ON_FRIDAY"] != DBNull.Value)
//        //        sp.ShipOnFriday = ((string)dr["SHIP_ON_FRIDAY"] == "1")? true: false;
//        //    if (dr["SHIP_ON_SATURDAY"] != DBNull.Value)
//        //        sp.ShipOnSaturday = ((string)dr["SHIP_ON_SATURDAY"] == "1")? true: false;
//        //    if (dr["SHIP_ON_SUNDAY"] != DBNull.Value)
//        //        sp.ShipOnSunday = ((string)dr["SHIP_ON_SUNDAY"] == "1")? true: false;
//        //    sp.DynamicStore		= dynamicStore;

//        //    // Begin Issue 3557 stodd
//        //    if (dr["SIMILAR_STORE_MODEL"] != DBNull.Value)
//        //        sp.SimilarStoreModel = ((string)dr["SIMILAR_STORE_MODEL"] == "1")? true: false;
//        //    // End Issue 3557 stodd

//        //    // Get statues
//        //    WeekProfile currentWeek = _calendar.GetWeek(_calendar.PostDate.Date);
//        //    sp.Status = StoreServerGlobal.GetStoreStatus(currentWeek ,sp.SellingOpenDt, sp.SellingCloseDt);
//        //    sp.StockStatus = StoreServerGlobal.GetStoreStatus(currentWeek ,sp.StockOpenDt, sp.StockCloseDt);


//        //    sp.Text = Include.GetStoreDisplay(_globalStoreDisplayOption, sp.StoreId, sp.StoreName, sp.StoreDescription);

//        //    //sp.Characteristics = new ArrayList();

//        //    //int maxCols = _dtAllStores.Columns.Count;
//        //    //int startCol = StoreServerGlobal.NumberOfStoreProfileColumns;
//        //    //for (int col=startCol;col<maxCols;col++)
//        //    //{
//        //    //    StoreCharGroupProfile ch = new StoreCharGroupProfile(-1);
//        //    //    ch.Name = _dtAllStores.Columns[col].Caption;
//        //    //    if (!ch.Name.StartsWith("SCGRID__"))
//        //    //    {
//        //    //        ch.Key = _characteristics.StoreCharRelations.GetCharacteristicGroupRID(ch.Name);
//        //    //        ch.CharacteristicValue.StoreCharType = _characteristics.StoreCharRelations.GetCharacteristicDataType(ch.Key);
//        //    //        if (Convert.ToString(dr[col], CultureInfo.CurrentCulture) == "<none>")
//        //    //        {
//        //    //            ch.CharacteristicValue.CharValue = System.DBNull.Value;
//        //    //        }
//        //    //        else
//        //    //        {
//        //    //            ch.CharacteristicValue.CharValue = dr[col];
//        //    //        }
//        //    //        ch.CharacteristicValue.SC_RID = _characteristics.StoreCharRelations.CharacteristicExists(ch.Name, ch.CharacteristicValue.CharValue);
						
//        //    //        sp.Characteristics.Add( ch );
//        //    //    }
//        //    //}

//        //    return sp;
//        //}

	
//        //internal void UnloadStoreDataRows(DataTable dtStores, ProfileList al, bool dynamicStores)
//        //{
//        //    ArrayList stores = new ArrayList();
//        //    // Move dataTable of Store Rows to a list of StoreProfiles
//        //    foreach(DataRow storeDataRow in dtStores.Rows)
//        //    {
//        //        // protects against duplicates (which can happen)
//        //        int key = Convert.ToInt32( storeDataRow["ST_RID"], CultureInfo.CurrentUICulture );
//        //        if (stores.Contains(key)) 
//        //            continue; 
//        //        stores.Add(key);
								
//        //        // unload dataTable row to a StoreProfile
//        //        StoreProfile currStore = ConvertToStoreProfile(storeDataRow, dynamicStores);
					
//        //        al.Add(currStore);
//        //    }
//        //}


//        /// <summary>
//        /// Checks the profile list against the avaialbe store list.  If a store is in the
//        /// profile list, but is not available, it's removed.
//        /// </summary>
//        /// <param name="storeList"></param>
//        /// <param name="reset"></param>
//        private void CheckStoreList(ref ProfileList storeList, bool reset)
//        {
//            if (reset)
//            {
//                PopulateAvailableStoreList();
//            }

//            int sIdx, aIdx;
//            bool storeFound;

//            sIdx = 0;
//            if (storeList != null)
//            {
//                while (sIdx < storeList.Count)
//                {
//                    storeFound = false;
//                    for (aIdx = 0; aIdx < _availableStores.Count; aIdx++)
//                    {
//                        int a = ((StoreProfile)storeList[sIdx]).Key;
//                        int b = ((StoreProfile)_availableStores[aIdx]).Key;

//                        if (((StoreProfile)storeList[sIdx]).Key ==
//                            ((StoreProfile)_availableStores[aIdx]).Key)
//                        {
//                            _availableStores.RemoveAt(aIdx);
//                            storeFound = true;
//                            break;
//                        }
//                    }
//                    // If the store does not belong, we remove it.  But now the list we are searching
//                    // has shifted, so the sIdx is not incremented.
//                    if (!storeFound)
//                        storeList.RemoveAt(sIdx);
//                    else
//                        sIdx++;
//                }
//            }
//        }

//        private void PopulateAvailableStoreList()
//        {
//            _availableStores.Clear();

//            int stCnt = _allStoreList.Count;
//            for (int i=0;i<stCnt;i++)
//            {

//                StoreProfile currStore = (StoreProfile)_allStoreList[i];
//                if (currStore.ActiveInd)
//                    _availableStores.Add(currStore);
//            }
//        }


//        //private void DumpAllStores()
//        //{
//        //    Debug.WriteLine("RELATIONS " + _dtAllStores.Rows.Count.ToString(CultureInfo.CurrentUICulture));
//        //    foreach (DataRow row in _dtAllStores.Rows)
//        //    {
//        //        int rid = Convert.ToInt32( row["ST_RID"], CultureInfo.CurrentUICulture );
//        //        Debug.WriteLine("RID " + rid.ToString(CultureInfo.CurrentUICulture) + " NAME " + row["ST_ID"].ToString() );
//        //    }
//        //}

//    }

	

//}
