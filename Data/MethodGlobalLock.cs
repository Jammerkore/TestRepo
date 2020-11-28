using System;
using System.Data;
using System.Text;
using System.Globalization;
using MIDRetail.DataCommon;
using System.Collections;
using System.Diagnostics;

namespace MIDRetail.Data
{
	/// <summary>
	/// Inherits MethodBase containing all properties for a Method.
	/// Adds properties for OTS_Plan
	/// </summary>
	public class OTSGlobalLockMethodData: MethodBaseData
	{
		private int				_methodRid;
		private int				_hnRID;
		private int				_versionRID;
		private int				_cdrRID;
        private ArrayList       _sglRIDList;
		private eSpreadOption	_spreadOption;
        private bool            _multiLevel;
        private bool            _stores;
        private bool            _chain;
        private int             _filterRid;
        private eFromLevelsType _fromLevelType;
		private int				_fromLevelOffset;
		private int				_fromLevelSequence;
        private eToLevelsType   _toLevelType;
        private int             _toLevelOffset;
        private int             _toLevelSequence;
		private int				_overrideLowLevelRid;

		public int MethodRid
		{
			get	{return _methodRid;}
			set	{_methodRid = value;}
		}

		public int HierNodeRID
		{
			get {return _hnRID;	}
			set	{_hnRID = value;}
		}
			
		public int VersionRID
		{
			get {return _versionRID;	}
			set	{_versionRID = value;}
		}

		public int CDR_RID
		{
			get{return _cdrRID;}
			set{_cdrRID = value;}
		}

        public ArrayList SGL_RID_List
        {
            get { return _sglRIDList; }
            set { _sglRIDList = value; }
        }

        public int Filter
        {
            get { return _filterRid; }
            set { _filterRid = value; }
        }
		
		public eSpreadOption SpreadOption
		{
			get	{return _spreadOption;}
			set	{_spreadOption = value;}
		}

        public bool MultiLevel
		{
            get {return _multiLevel; }
            set {_multiLevel = value; }
		}

        public bool Stores
        {
            get { return _stores; }
            set { _stores = value; }
        }

        public bool Chain
        {
            get { return _chain; }
            set { _chain = value; }
        }

		public eFromLevelsType FromLevelType
		{
            get { return _fromLevelType; }
            set { _fromLevelType = value; }
		}

		public int FromLevelOffset
		{
            get { return _fromLevelOffset; }
            set { _fromLevelOffset = value; }
		}

		public int FromLevelSequence
		{
            get { return _fromLevelSequence; }
			set{_fromLevelSequence = value;}
		}


        public eToLevelsType ToLevelType
        {
            get { return _toLevelType; }
            set { _toLevelType = value; }
        }

        public int ToLevelOffset
        {
            get { return _toLevelOffset; }
            set { _toLevelOffset = value; }
        }

        public int ToLevelSequence
        {
            get { return _toLevelSequence; }
            set { _toLevelSequence = value; }
        }

		public int OverrideLowLevelRid
		{
			get { return _overrideLowLevelRid; }
			set { _overrideLowLevelRid = value; }
		}

		/// <summary>
		/// Creates an instance of the OTSGlobalLockMethod class.
		/// </summary>
		public OTSGlobalLockMethodData()
		{
			
		}

		/// <summary>
		/// Creates an instance of the OTSGlobalLockMethod class.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		public OTSGlobalLockMethodData(int aMethodRID, eChangeType changeType)
		{
			_methodRid = aMethodRID;
			switch (changeType)
			{
				case eChangeType.populate:
                    PopulateGlobalLock(aMethodRID);
					break;
			}
		}

		/// <summary>
		/// Creates an instance of the OTSGlobalLockMethod class.
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		/// <param name="aMethodRID">The record ID of the method</param>
        public OTSGlobalLockMethodData(TransactionData td, int aMethodRID)
		{
			_dba = td.DBA;
			_methodRid = aMethodRID;
		}

		public bool PopulateGlobalLock(int aMethodRID)
		{
			try
			{
				if (PopulateMethod(aMethodRID))
				{
					_methodRid =  aMethodRID; 
							
					DataTable dtForecastSpreadMethod = MIDEnvironment.CreateDataTable();
                    dtForecastSpreadMethod = StoredProcedures.MID_METHOD_GLOBAL_LOCK_READ.Read(_dba, METHOD_RID: _methodRid);
					if (dtForecastSpreadMethod.Rows.Count != 0)
					{
						DataRow dr = dtForecastSpreadMethod.Rows[0];
						_hnRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
						_versionRID = Convert.ToInt32(dr["FV_RID"], CultureInfo.CurrentUICulture);
						_cdrRID = Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture);
                        _multiLevel = Include.ConvertCharToBool(Convert.ToChar(dr["MULTI_LEVEL_IND"], CultureInfo.CurrentUICulture));
                        _stores = Include.ConvertCharToBool(Convert.ToChar(dr["STORE_IND"], CultureInfo.CurrentUICulture));
                        _chain = Include.ConvertCharToBool(Convert.ToChar(dr["CHAIN_IND"], CultureInfo.CurrentUICulture));
                        _filterRid = Convert.ToInt32(dr["STORE_FILTER_RID"], CultureInfo.CurrentUICulture);
                        _fromLevelType = (eFromLevelsType)Convert.ToInt32(dr["FROM_LEVEL_TYPE"], CultureInfo.CurrentUICulture);
						_fromLevelOffset = Convert.ToInt32(dr["FROM_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
						_fromLevelSequence = Convert.ToInt32(dr["FROM_LEVEL_SEQ"], CultureInfo.CurrentUICulture);
                        _toLevelType = (eToLevelsType)Convert.ToInt32(dr["TO_LEVEL_TYPE"], CultureInfo.CurrentUICulture);
                        _toLevelOffset = Convert.ToInt32(dr["TO_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
                        _toLevelSequence = Convert.ToInt32(dr["TO_LEVEL_SEQ"], CultureInfo.CurrentUICulture);
						_overrideLowLevelRid = Convert.ToInt32(dr["OLL_RID"], CultureInfo.CurrentUICulture);

                        _sglRIDList = GetGlobalLockGroupLevelsList();

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

			catch (Exception Ex)
			{
				string message = Ex.ToString();
				throw;
			}
		}

        public ArrayList GetGlobalLockGroupLevelsList()
		{
			DataTable dtChild = MIDEnvironment.CreateDataTable("Child");
            DataSet ds = MIDEnvironment.CreateDataSet("test");
			ds.Tables.Add(dtChild);

            dtChild.Columns.Add("SGL_RID", System.Type.GetType("System.Int32"));

            dtChild = StoredProcedures.MID_METHOD_GLOBAL_LOCK_GRP_LVL_READ.Read(_dba, METHOD_RID: _methodRid);
			
			dtChild.TableName = "Child";
            dtChild.Columns[0].ColumnName = "SGL_RID";

            ArrayList SGL_RID_List = new ArrayList();
            foreach (DataRow dr in dtChild.Rows)
            {
                SGL_RID_List.Add(Convert.ToInt32(dr["SGL_RID"]));
            }

            return SGL_RID_List;
		}


		public bool InsertMethod(int aMethodRID, TransactionData td)
		{
			bool InsertSuccessful = true;

			try
			{	
				_methodRid = aMethodRID;
              
                int? OLL_RID_Nullable = null;
                if (_overrideLowLevelRid != Include.NoRID) OLL_RID_Nullable = _overrideLowLevelRid;

                StoredProcedures.MID_METHOD_GLOBAL_LOCK_INSERT.Insert(_dba,
                                                                      METHOD_RID: aMethodRID,
                                                                      HN_RID: _hnRID,
                                                                      FV_RID: _versionRID,
                                                                      CDR_RID: _cdrRID,
                                                                      MULTI_LEVEL_IND: Include.ConvertBoolToChar(_multiLevel),
                                                                      FROM_LEVEL_TYPE: (int)_fromLevelType,
                                                                      FROM_LEVEL_SEQ: _fromLevelSequence,
                                                                      FROM_LEVEL_OFFSET: _fromLevelOffset,
                                                                      TO_LEVEL_TYPE: (int)_toLevelType,
                                                                      TO_LEVEL_SEQ: _toLevelSequence,
                                                                      TO_LEVEL_OFFSET: _toLevelOffset,
                                                                      STORE_IND: Include.ConvertBoolToChar(_stores),
                                                                      CHAIN_IND: Include.ConvertBoolToChar(_chain),
                                                                      STORE_FILTER_RID: _filterRid,
                                                                      OLL_RID: OLL_RID_Nullable
                                                                      );

				if (UpdateChildData())
				{
					InsertSuccessful = true;
				}
				else
				{
					InsertSuccessful = false;
				}
			}

			catch (Exception Ex)
			{
				string exceptionMessage = Ex.Message;
				InsertSuccessful = false;
				throw;
			}
			return InsertSuccessful;
		}
		
		public bool UpdateMethod(int aMethodRID, TransactionData td)
		{
			_dba = td.DBA;

			bool UpdateSuccessful = true;

			try
			{
                if (DeleteChildData() && UpdateGlobalLockMethod(aMethodRID) && UpdateChildData())
				{
					UpdateSuccessful = true;
				}
			}

			catch
			{
				UpdateSuccessful = false;
				throw;
			}
			finally
			{
			}
			return UpdateSuccessful;
		}

        public bool UpdateGlobalLockMethod(int aMethodRID)
		{
			bool UpdateSuccessful = true;

			try
			{
				_methodRid = aMethodRID;
                
                int? OLL_RID_Nullable = null;
                if (_overrideLowLevelRid != Include.NoRID) OLL_RID_Nullable = _overrideLowLevelRid;

                StoredProcedures.MID_METHOD_GLOBAL_LOCK_UPDATE.Update(_dba,
                                                                      METHOD_RID: aMethodRID,
                                                                      HN_RID: _hnRID,
                                                                      FV_RID: _versionRID,
                                                                      CDR_RID: _cdrRID,
                                                                      MULTI_LEVEL_IND: Include.ConvertBoolToChar(_multiLevel),
                                                                      FROM_LEVEL_TYPE: (int)_fromLevelType,
                                                                      FROM_LEVEL_SEQ: _fromLevelSequence,
                                                                      FROM_LEVEL_OFFSET: _fromLevelOffset,
                                                                      TO_LEVEL_TYPE: (int)_toLevelType,
                                                                      TO_LEVEL_SEQ: _toLevelSequence,
                                                                      TO_LEVEL_OFFSET: _toLevelOffset,
                                                                      STORE_IND: Include.ConvertBoolToChar(_stores),
                                                                      CHAIN_IND: Include.ConvertBoolToChar(_chain),
                                                                      STORE_FILTER_RID: _filterRid,
                                                                      OLL_RID: OLL_RID_Nullable
                                                                      );
				UpdateSuccessful = true;
			}

			catch
			{
				UpdateSuccessful = false;
				throw;
			}
			finally
			{
			}
			return UpdateSuccessful;
		}

		private bool UpdateChildData()
		{
			bool UpdateSuccessful = true; 

			try
			{
                //-- Delete all METHOD_GLOBAL_LOCK_GRP_LVL records where equal to _methodRid --
      
                StoredProcedures.MID_METHOD_GLOBAL_LOCK_GRP_LVL_DELETE.Delete(_dba, METHOD_RID: _methodRid);

                //-- Add all sglKey's back to METHOD_GLOBAL_LOCK_GRP_LVL where equal to _methodRid --
                foreach (int sglKey in _sglRIDList)
				{

                    StoredProcedures.MID_METHOD_GLOBAL_LOCK_GRP_LVL_INSERT.Insert(_dba,
                                                                              METHOD_RID: _methodRid,
                                                                              SGL_RID: sglKey
                                                                              );
				}

                //DataView dv = new DataView();
                //dv.Table = _dsGlobalLock.Tables["Basis"];
                //for (int i = 0; i < dv.Count; i++)
                //{
                //    string addBasis = BuildInsertBasisStatement(dv[i]);
                //    _dba.ExecuteNonQuery(addBasis);
                //}
	
				
			}
			catch(Exception Ex)
			{
				string exceptionMessage = Ex.Message;
				UpdateSuccessful = false;
				throw;
			}
			finally
			{
			}
			return UpdateSuccessful;
		}



		public bool DeleteMethod(int aMethodRID, TransactionData td)
		{
			_dba = td.DBA;

			bool DeleteSuccessfull = true;

			try
			{
				if (DeleteChildData())
				{
                    StoredProcedures.MID_METHOD_GLOBAL_LOCK_DELETE.Delete(_dba, METHOD_RID: aMethodRID);
                    DeleteSuccessfull = true;
				}
			}
			catch
			{
				DeleteSuccessfull = false;
				throw;
			}
			finally
			{
			}
			return DeleteSuccessfull;
		}
		
		private bool DeleteChildData()
		{
			bool DeleteSuccessfull = true;

			try
			{
                StoredProcedures.MID_METHOD_GLOBAL_LOCK_GRP_LVL_DELETE.Delete(_dba, METHOD_RID: _methodRid);
				DeleteSuccessfull = true;
			}

			catch
			{
				DeleteSuccessfull = false;
				throw;
			}
			finally
			{
			}
			return DeleteSuccessfull;
		}
      
        public bool DeleteChainData(int version, int nodeKey, SortedList locks, ProfileList weekList)
        {
            bool DeleteSuccessfull = true;
            string inParamName;
            string insertVals = null;
            string insertVals1 = null;

            WeekProfile startWeek = (WeekProfile)weekList[0];
            WeekProfile endWeek = (WeekProfile)weekList[weekList.Count - 1];
            int startWeekKey = startWeek.Key;
            int endWeekKey = endWeek.Key;
            int UpdCnt = 0;
            DataTable StoresInTable = new DataTable();
            string insertString = null;
            string expression;



            StringBuilder InsertSQLCommand = new StringBuilder();
            StringBuilder UpdateSQLCommand = new StringBuilder();

            try
            {
                //StoresInTable = ForecastWeekLockTable(version, nodeKey, storeRidList, weekList);

                UpdateSQLCommand.Append("UPDATE CHAIN_FORECAST_WEEK_LOCK SET ");

                int inParamsCount = 0;
                foreach (DictionaryEntry val in locks)
                {
                    if (((VariableProfile)val.Value).DatabaseColumnName != null)
                    {
                        inParamName = ((VariableProfile)val.Value).DatabaseColumnName + "_LOCK";

                        if (inParamsCount > 0)
                        {
                            UpdateSQLCommand.Append(",");
                        }

                        UpdateSQLCommand.Append(inParamName + "='1'");

                        ++inParamsCount;
                    }
                }

                UpdateSQLCommand.Append(" WHERE HN_RID=" + (Convert.ToString(nodeKey, CultureInfo.CurrentUICulture)));
                UpdateSQLCommand.Append(" AND FV_RID=" + (Convert.ToString(version, CultureInfo.CurrentUICulture)));
                UpdateSQLCommand.Append(" AND TIME_ID BETWEEN " + (Convert.ToString(startWeekKey, CultureInfo.CurrentUICulture)) + " AND " + (Convert.ToString(endWeekKey, CultureInfo.CurrentUICulture)));



                InsertSQLCommand.Append("INSERT INTO CHAIN_FORECAST_WEEK_LOCK(HN_RID, FV_RID, TIME_ID");
                string insertValList = null;
                foreach (DictionaryEntry val in locks)
                {
                    if (((VariableProfile)val.Value).DatabaseColumnName != null)
                    {
                        inParamName = ((VariableProfile)val.Value).DatabaseColumnName + "_LOCK";
                        insertString = insertString + ", " + inParamName;
                        insertValList += ", '1'";
                    }
                }
                insertString = insertString + ") ";

                InsertSQLCommand.Append(insertString);
                foreach (WeekProfile wp in weekList)
                {

                    expression = " and TIME_ID = " + (Convert.ToString(wp.Key, CultureInfo.CurrentUICulture));

                    insertVals = "(" + (Convert.ToString(nodeKey, CultureInfo.CurrentUICulture)) + "," + (Convert.ToString(version, CultureInfo.CurrentUICulture)) + ",";
                    insertVals = insertVals + (Convert.ToString(wp.Key, CultureInfo.CurrentUICulture) + insertValList + ")");
                    insertVals1 = insertVals1 + insertVals;
                    if (wp.Key != endWeekKey)
                    {
                        insertVals1 = insertVals1 + ",";
                    }
                         
                }

                UpdCnt = _dba.ExecuteNonQuery(UpdateSQLCommand.ToString());

                if (UpdCnt < weekList.Count)
                {
                    InsertSQLCommand.Append(" VALUES " + insertVals1);

                    _dba.ExecuteNonQuery(InsertSQLCommand.ToString());
                }

                DeleteSuccessfull = true;
            }
            catch (Exception err)
            {
                DeleteSuccessfull = false;
                string message = err.ToString();
            }
            return DeleteSuccessfull;
        }

        public DataTable ForecastWeekLockTable(int version, int nodeKey, ArrayList storeRidList, ProfileList weekList)
        {
            DataTable FWLT = new DataTable();
            StringBuilder GetSQLCommand = new StringBuilder();
            WeekProfile startWeek = (WeekProfile)weekList[0];
            WeekProfile endWeek = (WeekProfile)weekList[weekList.Count - 1];
            try
            {

                DataTable dtStoreList = new DataTable();
                dtStoreList.Columns.Add("ST_RID", typeof(int));
                foreach (int ST_RID in storeRidList)
                {
                    //ensure store RIDs are distinct, and only added to the datatable one time
                    if (dtStoreList.Select("ST_RID=" + ST_RID.ToString()).Length == 0)
                    {
                        DataRow dr = dtStoreList.NewRow();
                        dr["ST_RID"] = ST_RID;
                        dtStoreList.Rows.Add(dr);
                    }
                }

              
                FWLT = StoredProcedures.MID_STORE_FORECAST_WEEK_LOCK_READ.Read(_dba,
                                                                               HN_RID: nodeKey,
                                                                               FV_RID: version,
                                                                               STORE_RID_LIST: dtStoreList,
                                                                               START_WEEK: startWeek.Key,
                                                                               END_WEEK: endWeek.Key
                                                                               );

            }
            catch (Exception)
            {

            }

            return FWLT;
        }


        public bool DeleteStoreData(int version, int nodeKey, ArrayList storeRidList, SortedList locks, ProfileList weekList)
        {
            bool DeleteSuccessfull = true;
            string inParamName;
            string insertVals = null;
            string insertVals1 = null;
            string stRIDUpdate = "(";

            WeekProfile startWeek = (WeekProfile)weekList[0];
            WeekProfile endWeek = (WeekProfile)weekList[weekList.Count - 1];
            int startWeekKey = startWeek.Key;
            int endWeekKey = endWeek.Key;
            int TotInsCnt = 0;  //TT#274 - MD - DOConnell - Global Lock not being set for mutlitple sets
            int InsCnt = 0;
            DataTable StoresInTable = new DataTable();
            string insertString = null;
            string expression;
            DataRow[] foundRows;


            StringBuilder InsertSQLCommand = new StringBuilder();
            StringBuilder UpdateSQLCommand = new StringBuilder();

            try
            {
                StoresInTable = ForecastWeekLockTable(version, nodeKey, storeRidList, weekList);

                UpdateSQLCommand.Append("UPDATE STORE_FORECAST_WEEK_LOCK SET ");
                for (int i = 0; i < storeRidList.Count; i++)
                {
                    int inParamsCount = 0;
                    foreach (DictionaryEntry val in locks)
                    {
                        if (((VariableProfile)val.Value).DatabaseColumnName != null)
                        {
                            inParamName = ((VariableProfile)val.Value).DatabaseColumnName + "_LOCK";



                            if (inParamsCount > 0)
                            {
                                if (i == 0) UpdateSQLCommand.Append(",");
                            }

                            if (i == 0) UpdateSQLCommand.Append(inParamName + "='1'");
 
                            ++inParamsCount;
                        }
                    }

                    if ((i + 1) != storeRidList.Count)
                    {
                        //Begin TT#2590 - DOConnell - global lock not locking all stores for all weeks
                        stRIDUpdate = stRIDUpdate + (Convert.ToString(storeRidList[i], CultureInfo.CurrentUICulture));
                        
                        if ((i + 1) != storeRidList.Count)
                        {
                            stRIDUpdate = stRIDUpdate + ",";
                        }

                        //if (i == 0)
                        //{
                        //    stRIDUpdate = stRIDUpdate + (Convert.ToString(storeRidList[i], CultureInfo.CurrentUICulture));
                        //}
                        //else
                        //{
                        //    stRIDUpdate = stRIDUpdate + "," + (Convert.ToString(storeRidList[i], CultureInfo.CurrentUICulture));
                        //}
                        //END TT#2590 - DOConnell - global lock not locking all stores for all weeks
                    }
                    else
                    {
                        //BEGIN TT#2589 - DOConnell - global lock not locking with filter selected
                        //stRIDUpdate = stRIDUpdate + "," + (Convert.ToString(storeRidList[i], CultureInfo.CurrentUICulture)) + ")";
                        stRIDUpdate = stRIDUpdate + (Convert.ToString(storeRidList[i], CultureInfo.CurrentUICulture)) + ")";
                        //END TT#2589 - DOConnell - global lock not locking with filter selected

                    }
                }

                UpdateSQLCommand.Append(" WHERE HN_RID=" + (Convert.ToString(nodeKey, CultureInfo.CurrentUICulture)));
                UpdateSQLCommand.Append(" AND FV_RID=" + (Convert.ToString(version, CultureInfo.CurrentUICulture)));
                UpdateSQLCommand.Append(" AND TIME_ID BETWEEN " + (Convert.ToString(startWeekKey, CultureInfo.CurrentUICulture)) + " AND " + (Convert.ToString(endWeekKey, CultureInfo.CurrentUICulture)));
                UpdateSQLCommand.Append(" AND ST_RID IN " + stRIDUpdate);

                _dba.ExecuteNonQuery(UpdateSQLCommand.ToString()); //TT#274 - MD - DOConnell - this line moved up from below
                
                InsertSQLCommand.Append("INSERT INTO STORE_FORECAST_WEEK_LOCK(HN_RID, FV_RID, TIME_ID, ST_RID");
                string insertValList = null;
                foreach (DictionaryEntry val in locks)
                {
                    if (((VariableProfile)val.Value).DatabaseColumnName != null)
                    {
                        inParamName = ((VariableProfile)val.Value).DatabaseColumnName + "_LOCK";
                        insertString = insertString + ", " + inParamName;
                        insertValList += ", '1'";
                    }
                }
                insertString = insertString + ") ";

                InsertSQLCommand.Append(insertString);
                foreach (WeekProfile wp in weekList)
                {
                    for (int i = 0; i < storeRidList.Count; i++)
                    {
                        expression = "ST_RID = " + (Convert.ToString(storeRidList[i], CultureInfo.CurrentUICulture)) + " and TIME_ID = " + (Convert.ToString(wp.Key, CultureInfo.CurrentUICulture));
                        foundRows = StoresInTable.Select(expression);

                        if (foundRows.Length == 0)
                        {
                            insertVals = "(" + (Convert.ToString(nodeKey, CultureInfo.CurrentUICulture)) + "," + (Convert.ToString(version, CultureInfo.CurrentUICulture)) + ",";
                            insertVals = insertVals + (Convert.ToString(wp.Key, CultureInfo.CurrentUICulture) + "," + (Convert.ToString(storeRidList[i], CultureInfo.CurrentUICulture)) + insertValList + ")");
                            insertVals1 = insertVals1 + insertVals;
                            if ((i + 1) != storeRidList.Count)
                            {
                                insertVals1 = insertVals1 + ",";
                            }
                            else if ((i + 1) == storeRidList.Count)
                            {
                                if (wp.Key != endWeekKey)
                                {
                                    insertVals1 = insertVals1 + ",";
                                }
                            }
                            InsCnt++;
                            
                            //BEGIN TT#274 - MD - DOConnell - Global Lock not being set for mutlitple sets
                            TotInsCnt++;
                            if (InsCnt >= 100 || (i + 1) == storeRidList.Count)
                            {

                                if (insertVals1.EndsWith(","))
                                {
                                    int insertLenght = insertVals1.Length;
                                    insertVals1 = insertVals1.Substring(0, (insertLenght - 1));
                                }

                                InsertSQLCommand.Append(" VALUES " + insertVals1);

                                _dba.ExecuteNonQuery(InsertSQLCommand.ToString());
                                InsCnt = 0;
                                InsertSQLCommand = new StringBuilder();
                                InsertSQLCommand.Append("INSERT INTO STORE_FORECAST_WEEK_LOCK(HN_RID, FV_RID, TIME_ID, ST_RID");
                                InsertSQLCommand.Append(insertString);
                                insertVals1 = null;

                            }
                            //END TT#274 - MD - DOConnell - Global Lock not being set for mutlitple sets
                        }
                    }
                    // Begin TT#2830 - JSmith - Multiple Lock Methods
                    if (InsCnt > 0)
                    {

                        if (insertVals1.EndsWith(","))
                        {
                            int insertLenght = insertVals1.Length;
                            insertVals1 = insertVals1.Substring(0, (insertLenght - 1));
                        }

                        InsertSQLCommand.Append(" VALUES " + insertVals1);

                        _dba.ExecuteNonQuery(InsertSQLCommand.ToString());
                        InsCnt = 0;
                        InsertSQLCommand = new StringBuilder();
                        InsertSQLCommand.Append("INSERT INTO STORE_FORECAST_WEEK_LOCK(HN_RID, FV_RID, TIME_ID, ST_RID");
                        InsertSQLCommand.Append(insertString);
                        insertVals1 = null;

                    }
                    // End TT#2830 - JSmith - Multiple Lock Methods
                }


                //_dba.ExecuteNonQuery(UpdateSQLCommand.ToString()); //TT#274 - MD - DOConnell - this line moved up

                //if (TotInsCnt > 0)
                //{
                //    if (insertVals1.EndsWith(","))
                //    {
                //        int insertLenght = insertVals1.Length;
                //        insertVals1 = insertVals1.Substring(0, (insertLenght - 1));
                //    }

                //    InsertSQLCommand.Append(" VALUES " + insertVals1);

                //    _dba.ExecuteNonQuery(InsertSQLCommand.ToString());
                //}

                DeleteSuccessfull = true;
            }
            catch (Exception err)
            {
                DeleteSuccessfull = false;
                string message = err.ToString();
            }
            return DeleteSuccessfull;
        }
        // End TT#54
	}
}
