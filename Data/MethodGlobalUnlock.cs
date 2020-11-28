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
	public class OTSGlobalUnlockMethodData: MethodBaseData
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
		/// Creates an instance of the OTSGlobalUnlockMethod class.
		/// </summary>
		public OTSGlobalUnlockMethodData()
		{
			
		}

		/// <summary>
		/// Creates an instance of the OTSGlobalUnlockMethod class.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		public OTSGlobalUnlockMethodData(int aMethodRID, eChangeType changeType)
		{
			_methodRid = aMethodRID;
			switch (changeType)
			{
				case eChangeType.populate:
                    PopulateGlobalUnlock(aMethodRID);
					break;
			}
		}

		/// <summary>
		/// Creates an instance of the OTSGlobalUnlockMethod class.
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		/// <param name="aMethodRID">The record ID of the method</param>
        public OTSGlobalUnlockMethodData(TransactionData td, int aMethodRID)
		{
			_dba = td.DBA;
			_methodRid = aMethodRID;
		}

		public bool PopulateGlobalUnlock(int aMethodRID)
		{
			try
			{
				if (PopulateMethod(aMethodRID))
				{
					_methodRid =  aMethodRID; 	
				
					DataTable dtForecastSpreadMethod = MIDEnvironment.CreateDataTable();
                    dtForecastSpreadMethod = StoredProcedures.MID_METHOD_GLOBAL_UNLOCK_READ.Read(_dba, METHOD_RID: _methodRid);

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

                        _sglRIDList = GetGlobalUnlockGroupLevelsList();

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

        public ArrayList GetGlobalUnlockGroupLevelsList()
		{
            //Begin TT#1296-MD -jsobek -Recieve error when trying to open an existing Global Lock Method
          
			//DataTable dtChild = MIDEnvironment.CreateDataTable("Child");
            //DataSet ds = MIDEnvironment.CreateDataSet("test");
			
            //dtChild.Columns.Add("SGL_RID", System.Type.GetType("System.Int32"));

            DataTable dtChild = StoredProcedures.MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_READ.Read(_dba, METHOD_RID: _methodRid);
            //ds.Tables.Add(dtChild);
           
			//dtChild.TableName = "Child";
          
            //dtChild.Columns[0].ColumnName = "SGL_RID";
            //End TT#1296-MD -jsobek -Recieve error when trying to open an existing Global Lock Method

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

                StoredProcedures.MID_METHOD_GLOBAL_UNLOCK_INSERT.Insert(_dba,
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
                if (DeleteChildData() && UpdateGlobalUnlockMethod(aMethodRID) && UpdateChildData())
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

        public bool UpdateGlobalUnlockMethod(int aMethodRID)
		{
			bool UpdateSuccessful = true;

			try
			{
				_methodRid = aMethodRID;
                

                int? OLL_RID_Nullable = null;
                if (_overrideLowLevelRid != Include.NoRID) OLL_RID_Nullable = _overrideLowLevelRid;

                StoredProcedures.MID_METHOD_GLOBAL_UNLOCK_UPDATE.Update(_dba,
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
                //-- Delete all METHOD_GLOBAL_UNLOCK_GRP_LVL records where equal to _methodRid --
                StoredProcedures.MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_DELETE_FROM_METHOD.Delete(_dba, METHOD_RID: _methodRid);

                //-- Add all sglKey's back to METHOD_GLOBAL_UNLOCK_GRP_LVL where equal to _methodRid --
                foreach (int sglKey in _sglRIDList)
				{
     

                    StoredProcedures.MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_INSERT.Insert(_dba,
                                                                                METHOD_RID: _methodRid,
                                                                                SGL_RID: sglKey
                                                                                );
				}

                //DataView dv = new DataView();
                //dv.Table = _dsGlobalUnlock.Tables["Basis"];
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
                    StoredProcedures.MID_METHOD_GLOBAL_UNLOCK_DELETE.Delete(_dba, METHOD_RID: aMethodRID);
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

                StoredProcedures.MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_DELETE.Delete(_dba, METHOD_RID: _methodRid);
				
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
      
        public bool DeleteChainData(int version, int nodeKey, int startWeekKey, int endWeekKey)
        {
            bool DeleteSuccessfull = true;

            try
            {
                StoredProcedures.MID_CHAIN_FORECAST_WEEK_LOCK_DELETE.Delete(_dba,
                                                                            HN_RID: nodeKey,
                                                                            FV_RID: version,
                                                                            TIME_ID_START: startWeekKey,
                                                                            TIME_ID_END: endWeekKey
                                                                            );

                DeleteSuccessfull = true;
            }
            catch (Exception err)
            {
                DeleteSuccessfull = false;
                string message = err.ToString();
            }
            return DeleteSuccessfull;
        }

        public bool DeleteStoreData(int version, int nodeKey, int startWeekKey, int endWeekKey, ArrayList storeRidList)
        {
            bool DeleteSuccessfull = true;

            try
            {
                

                DataTable dtStoreList = new DataTable();
                dtStoreList.Columns.Add("ST_RID", typeof(int));
                for (int i = 0; i < storeRidList.Count; i++)
                {
                    //ensure styleHNRids are distinct, and only added to the datatable one time
                    if (dtStoreList.Select("ST_RID=" + storeRidList[i].ToString()).Length == 0)
                    {
                        DataRow dr = dtStoreList.NewRow();
                        dr["ST_RID"] = storeRidList[i];
                        dtStoreList.Rows.Add(dr);
                    }
                } 

                StoredProcedures.MID_STORE_FORECAST_WEEK_LOCK_DELETE.Delete(_dba,
                                                                                HN_RID: nodeKey,
                                                                                FV_RID: version,
                                                                                TIME_ID_START: startWeekKey,
                                                                                TIME_ID_END: endWeekKey,
                                                                                STORE_RID_LIST: dtStoreList
                                                                                );

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
