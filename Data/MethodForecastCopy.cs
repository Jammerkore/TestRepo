using System;
using System.Data;
using System.Text;
using System.Globalization;
using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	/// <summary>
	/// Inherits MethodBase containing all properties for a Method.
	/// Adds properties for OTS_Plan
	/// </summary>
	public class OTSForecastCopyMethodData: MethodBaseData
	{
		private int				_fcMethodRID;
		private int				_hnRID;
		private int				_versionRID;
		private int				_cdrRID;
		private int				_filterRID;
		private int  			_planType;
        //MID Track #4863 - JBolles - Multi-Level Copy
        private bool _multLevelInd;
        // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
        //private int _toLevel;
        //private int _fromLevel;
        private eFromLevelsType _fromLevelType;
        private int _fromLevelOffset;
        private int _fromLevelSequence;
        private eToLevelsType _toLevelType;
        private int _toLevelOffset;
        private int _toLevelSequence;
        // END Track #6107
        //End MID Track #4863
		private int _overrideLowLevelRid;
        // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
        private bool _copyPreInitValues;
        // End Track #6347

		private DataSet 		_dsForecastCopy;
		
		public int ForecastCopy_Method_RID
		{
			get	{return _fcMethodRID;}
			set	{_fcMethodRID = value;}
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
		
		public int PlanType
		{
			get	{return _planType;}
			set	{_planType = value;}
		}

		public int FilterRID
		{
			get	{return _filterRID;}
			set	{_filterRID = value;}
		}

        // MID Track #4863 - JBolles - Multi-Level Copy
        public bool MultiLevelInd
        {
            get { return _multLevelInd; }
            set { _multLevelInd = value; }
        }

        // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
        //public int FromLevel
        //{
        //    get { return _fromLevel; }
        //    set { _fromLevel = value; }
        //}

        //public int ToLevel
        //{
        //    get { return _toLevel; }
        //    set { _toLevel = value; }
        //}
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
            set { _fromLevelSequence = value; }
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
        // END Track #6107
		//End MID Track #4863

		public int OverrideLowLevelRid
		{
			get { return _overrideLowLevelRid; }
			set { _overrideLowLevelRid = value; }
		}

		public DataSet DSForecastCopy
		{
			get	{return _dsForecastCopy;}
			set	{_dsForecastCopy = value;}
		}

        // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
        public bool CopyPreInitValues
        {
            get { return _copyPreInitValues; }
            set { _copyPreInitValues = value; }
        }
        // End Track #6347

		/// <summary>
		/// Creates an instance of the OTSForecastCopyMethodData class.
		/// </summary>
		public OTSForecastCopyMethodData()
		{
		}

		/// <summary>
		/// Creates an instance of the OTSForecastCopyMethodData class.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		public OTSForecastCopyMethodData(int aMethodRID, eChangeType changeType)
		{
			_fcMethodRID = aMethodRID;
			switch (changeType)
			{
				case eChangeType.populate:
					PopulateForecastCopy(aMethodRID);
					break;
			}
		}

		/// <summary>
		/// Creates an instance of the OTSForecastCopyMethodData class.
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		/// <param name="aMethodRID">The record ID of the method</param>
		public OTSForecastCopyMethodData(TransactionData td, int aMethodRID)
		{
			_dba = td.DBA;
			_fcMethodRID = aMethodRID;
		}

		public bool PopulateForecastCopy(int aMethodRID)
		{
			try
			{
				if (PopulateMethod(aMethodRID))
				{
					_fcMethodRID =  aMethodRID;
				
                    DataTable dtForecastCopyMethod = MIDEnvironment.CreateDataTable();
                    dtForecastCopyMethod = StoredProcedures.MID_METHOD_COPY_FORECAST_READ.Read(_dba, METHOD_RID: _fcMethodRID);
					if (dtForecastCopyMethod.Rows.Count != 0)
					{
						DataRow dr = dtForecastCopyMethod.Rows[0];
						_hnRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
						_versionRID = Convert.ToInt32(dr["FV_RID"], CultureInfo.CurrentUICulture);
						_cdrRID = Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture);
						_planType = Convert.ToInt32(dr["PLAN_TYPE"], CultureInfo.CurrentUICulture);
				        _filterRID = Convert.ToInt32(dr["STORE_FILTER_RID"], CultureInfo.CurrentUICulture);
                        if (dr["MULTI_LEVEL_IND"] == DBNull.Value)
                        {
                            _multLevelInd = false;
                            // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
                            //_toLevel = Include.NoRID;
                            //_fromLevel = Include.NoRID;
                            _fromLevelType = eFromLevelsType.None;
                            _fromLevelOffset = 0;
                            _fromLevelSequence = 0;
                            _toLevelType = eToLevelsType.None;
                            _toLevelOffset = 0;
                            _toLevelSequence = 0;
                            // END Track #6107
                        }
                        else
                        {
                            _multLevelInd = Convert.ToBoolean(dr["MULTI_LEVEL_IND"], CultureInfo.CurrentUICulture);
                            // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
                            //_toLevel = Convert.ToInt32(dr["TO_LEVEL"], CultureInfo.CurrentUICulture);
                            //_fromLevel = Convert.ToInt32(dr["FROM_LEVEL"], CultureInfo.CurrentUICulture);
                            _fromLevelType = (eFromLevelsType)Convert.ToInt32(dr["FROM_LEVEL_TYPE"], CultureInfo.CurrentUICulture);
                            _fromLevelOffset = Convert.ToInt32(dr["FROM_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
                            _fromLevelSequence = Convert.ToInt32(dr["FROM_LEVEL_SEQ"], CultureInfo.CurrentUICulture);
                            _toLevelType = (eToLevelsType)Convert.ToInt32(dr["TO_LEVEL_TYPE"], CultureInfo.CurrentUICulture);
                            _toLevelOffset = Convert.ToInt32(dr["TO_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
                            _toLevelSequence = Convert.ToInt32(dr["TO_LEVEL_SEQ"], CultureInfo.CurrentUICulture);
                            // END Track #6107
                        }
						_overrideLowLevelRid = Convert.ToInt32(dr["OLL_RID"], CultureInfo.CurrentUICulture);
                        // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
                        _copyPreInitValues = Include.ConvertCharToBool(Convert.ToChar(dr["PREINIT_VALUES_IND"], CultureInfo.CurrentUICulture));
                        // End Track #6347

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

		public DataSet GetForecastCopyChildData()
		{ 
			try
			{
				_dsForecastCopy = MIDEnvironment.CreateDataSet();

				DataTable dtGroupLevel = SetupCopyGroupLevelTable();
				DataTable dtBasis = SetupCopyBasisTable();
				_dsForecastCopy.Tables.Add(dtGroupLevel);
				_dsForecastCopy.Tables.Add(dtBasis);
				return _dsForecastCopy;
			}

			catch (Exception Ex)
			{
				string message = Ex.ToString();
				throw;
			}
		}

		private DataTable SetupCopyGroupLevelTable()
		{
			DataTable dtGroupLevel = MIDEnvironment.CreateDataTable("GroupLevel");
		
			dtGroupLevel.Columns.Add("SglRID", System.Type.GetType("System.Int32"));
		
            
            dtGroupLevel = StoredProcedures.MID_METHOD_COPY_GROUP_LEVEL_READ.Read(_dba, METHOD_RID: _fcMethodRID);

			dtGroupLevel.TableName = "GroupLevel";
			dtGroupLevel.Columns[0].ColumnName = "SGL_RID";

			return dtGroupLevel;
		}

		private DataTable SetupCopyBasisTable()
		{
			DataTable dtBasis = MIDEnvironment.CreateDataTable("Basis");
			
			dtBasis.Columns.Add("SGL_RID", System.Type.GetType("System.Int32"));
			dtBasis.Columns.Add("DETAIL_SEQ", System.Type.GetType("System.Int32"));
			dtBasis.Columns.Add("Merchandise", System.Type.GetType("System.String"));
			dtBasis.Columns.Add("HN_RID", System.Type.GetType("System.Int32"));
			dtBasis.Columns.Add("FV_RID", System.Type.GetType("System.Int32"));
			dtBasis.Columns.Add("DateRange",System.Type.GetType("System.String"));
			dtBasis.Columns.Add("CDR_RID", System.Type.GetType("System.Int32"));
			dtBasis.Columns.Add("WEIGHT", System.Type.GetType("System.Double"));
			dtBasis.Columns.Add("INCLUDE_EXCLUDE", System.Type.GetType("System.Int32"));
			dtBasis.Columns.Add("IncludeButton",System.Type.GetType("System.String"));

           
            dtBasis = StoredProcedures.MID_METHOD_COPY_BASIS_DETAIL_READ.Read(_dba, METHOD_RID: _fcMethodRID);
			
			dtBasis.TableName = "Basis";
			dtBasis.Columns[0].ColumnName = "SGL_RID";
			dtBasis.Columns[1].ColumnName = "DETAIL_SEQ";
			dtBasis.Columns[2].ColumnName = "Merchandise";
			dtBasis.Columns[3].ColumnName = "HN_RID";
			dtBasis.Columns[4].ColumnName = "FV_RID";
			dtBasis.Columns[5].ColumnName = "DateRange";
			dtBasis.Columns[6].ColumnName = "CDR_RID";
			dtBasis.Columns[7].ColumnName = "WEIGHT";
			dtBasis.Columns[8].ColumnName = "INCLUDE_EXCLUDE";
			dtBasis.Columns[9].ColumnName = "IncludeButton";

			return dtBasis;
		}
		
		public bool InsertMethod(int aMethodRID, TransactionData td)
		{
			bool InsertSuccessful = true;

			try
			{	
				_fcMethodRID = aMethodRID;
               
                int? OLL_RID_Nullable = null;
                if (_overrideLowLevelRid != Include.NoRID) OLL_RID_Nullable = _overrideLowLevelRid;
                StoredProcedures.MID_METHOD_COPY_FORECAST_INSERT.Insert(_dba,
                                                                        METHOD_RID: aMethodRID,
                                                                        HN_RID: _hnRID,
                                                                        FV_RID: _versionRID,
                                                                        CDR_RID: _cdrRID,
                                                                        PLAN_TYPE: _planType,
                                                                        STORE_FILTER_RID: _filterRID,
                                                                        MULTI_LEVEL_IND: Include.ConvertBoolToInt(_multLevelInd),
                                                                        FROM_LEVEL_TYPE: (int)_fromLevelType,
                                                                        FROM_LEVEL_SEQ: _fromLevelSequence,
                                                                        FROM_LEVEL_OFFSET: _fromLevelOffset,
                                                                        TO_LEVEL_TYPE: (int)_toLevelType,
                                                                        TO_LEVEL_SEQ: _toLevelSequence,
                                                                        TO_LEVEL_OFFSET: _toLevelOffset,
                                                                        OLL_RID: OLL_RID_Nullable,
                                                                        PREINIT_VALUES_IND: Include.ConvertBoolToChar(_copyPreInitValues)
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
				if (DeleteChildData() && UpdateForecastCopyMethod(aMethodRID) && UpdateChildData())
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

		public bool UpdateForecastCopyMethod(int aMethodRID)
		{
			bool UpdateSuccessful = true;

			try
			{
				_fcMethodRID = aMethodRID;
               
                int? OLL_RID_Nullable = null;
                if (_overrideLowLevelRid != Include.NoRID) OLL_RID_Nullable = _overrideLowLevelRid;
                StoredProcedures.MID_METHOD_COPY_FORECAST_UPDATE.Update(_dba,
                                                                        METHOD_RID: aMethodRID,
                                                                        HN_RID: _hnRID,
                                                                        FV_RID: _versionRID,
                                                                        CDR_RID: _cdrRID,
                                                                        PLAN_TYPE: _planType,
                                                                        STORE_FILTER_RID: _filterRID,
                                                                        MULTI_LEVEL_IND: Include.ConvertBoolToInt(_multLevelInd),
                                                                        FROM_LEVEL_TYPE: (int)_fromLevelType,
                                                                        FROM_LEVEL_SEQ: _fromLevelSequence,
                                                                        FROM_LEVEL_OFFSET: _fromLevelOffset,
                                                                        TO_LEVEL_TYPE: (int)_toLevelType,
                                                                        TO_LEVEL_SEQ: _toLevelSequence,
                                                                        TO_LEVEL_OFFSET: _toLevelOffset,
                                                                        OLL_RID: OLL_RID_Nullable,
                                                                        PREINIT_VALUES_IND: Include.ConvertBoolToChar(_copyPreInitValues)
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
				if (_dsForecastCopy == null)
				{
					return UpdateSuccessful;
				}
				 
				DataView dv = new DataView();
				dv.Table = _dsForecastCopy.Tables["GroupLevel"];
				for (int i = 0; i < dv.Count; i++)
				{
                    StoredProcedures.MID_METHOD_COPY_GROUP_LEVEL_INSERT.Insert(_dba,
                                                                                   METHOD_RID: _fcMethodRID,
                                                                                   SGL_RID: (int)dv[i]["SGL_RID"]
                                                                                   );
				}

				dv.Table = _dsForecastCopy.Tables["Basis"];
				for (int i = 0; i < dv.Count; i++)
				{
                    int? HN_RID_Nullable = null;
                    if ((int)dv[i]["HN_RID"] != Include.NoRID) HN_RID_Nullable = (int)dv[i]["HN_RID"];

                    StoredProcedures.MID_METHOD_COPY_BASIS_DETAIL_INSERT.Insert(_dba,
                                                                                    METHOD_RID: _fcMethodRID,
                                                                                    SGL_RID: (int)dv[i]["SGL_RID"],
                                                                                    DETAIL_SEQ: (int)dv[i]["DETAIL_SEQ"],
                                                                                    HN_RID: HN_RID_Nullable,
                                                                                    FV_RID: (int)dv[i]["FV_RID"],
                                                                                    CDR_RID: (int)dv[i]["CDR_RID"],
                                                                                    WEIGHT: Include.ConvertObjectToNullableDouble(dv[i]["WEIGHT"]),
                                                                                    INCLUDE_EXCLUDE: (int)dv[i]["INCLUDE_EXCLUDE"]
                                                                                    );
				}
	
				
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
                    StoredProcedures.MID_METHOD_COPY_FORECAST_DELETE.Delete(_dba, METHOD_RID: aMethodRID);
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
                StoredProcedures.MID_METHOD_COPY_BASIS_DETAIL_DELETE.Delete(_dba, METHOD_RID: _fcMethodRID);
                StoredProcedures.MID_METHOD_COPY_GROUP_LEVEL_DELETE.Delete(_dba, METHOD_RID: _fcMethodRID);
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

		public DataTable GetMethodsByNode(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_METHOD_COPY_FORECAST_READ_FROM_NODE.Read(_dba, HN_RID: aNodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetMethodsBasisByNode(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_METHOD_COPY_BASIS_DETAIL_READ_FROM_NODE.Read(_dba, HN_RID: aNodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
	}
}
