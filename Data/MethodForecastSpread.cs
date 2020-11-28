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
	public class OTSForecastSpreadMethodData: MethodBaseData
	{
		private int				_methodRid;
		private int				_hnRID;
		private int				_versionRID;
		private int				_cdrRID;
		private eSpreadOption	_spreadOption;
		private bool  			_ignoreLocks;
        private bool            _multiLevel;
        private eFromLevelsType _fromLevelType;
        private int             _fromLevelOffset;
        private int             _fromLevelSequence;
        private eToLevelsType   _toLevelType;
        private int             _toLevelOffset;
        private int             _toLevelSequence;
		private bool  			_equalizeWeighting;		// ANF - Weighting Multiple Basis

		private DataSet 		_dsForecastSpread;
		//private DataTable 		_dtLowerLevels;  <--DKJ
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
		
		public eSpreadOption SpreadOption
		{
			get	{return _spreadOption;}
			set	{_spreadOption = value;}
		}

		public bool IgnoreLocks
		{
			get	{return _ignoreLocks;}
			set	{_ignoreLocks = value;}
		}

        public bool MultiLevel
        {
            get { return _multiLevel; }
            set { _multiLevel = value; }
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
		
		public bool EqualizeWeighting				// ANF - Weighting Multiple Basis
		{
			get	{return _equalizeWeighting;}
			set	{_equalizeWeighting = value;}
		}

		public DataSet DSForecastSpread
		{
			get	{return _dsForecastSpread;}
			set	{_dsForecastSpread = value;}
		}

		public int OverrideLowLevelRid
		{
			get { return _overrideLowLevelRid; }
			set { _overrideLowLevelRid = value; }
		}

		/// <summary>
        /// Creates an instance of the OTSForcastSpreadMethodData class.
		/// </summary>
		public OTSForecastSpreadMethodData()
		{
			
		}

		/// <summary>
        /// Creates an instance of the OTSForcastSpreadMethodData class.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		public OTSForecastSpreadMethodData(int aMethodRID, eChangeType changeType)
		{
			_methodRid = aMethodRID;
			switch (changeType)
			{
				case eChangeType.populate:
                    PopulateForecastSpread(aMethodRID);
					break;
			}
		}

		/// <summary>
        /// Creates an instance of the OTSForcastSpreadMethodData class.
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		/// <param name="aMethodRID">The record ID of the method</param>
		public OTSForecastSpreadMethodData(TransactionData td, int aMethodRID)
		{
			_dba = td.DBA;
			_methodRid = aMethodRID;
		}

		public bool PopulateForecastSpread(int aMethodRID)
		{
			try
			{
				if (PopulateMethod(aMethodRID))
				{
					_methodRid =  aMethodRID; 
					// ANF - Weighting Multiple Basis: add EQUALIZE_WEIGHTING column 
                   

					DataTable dtForecastSpreadMethod = MIDEnvironment.CreateDataTable();
                    dtForecastSpreadMethod = StoredProcedures.MID_METHOD_SPREAD_FORECAST_READ.Read(_dba, METHOD_RID: _methodRid);
                    if (dtForecastSpreadMethod.Rows.Count != 0)
					{
                        DataRow dr = dtForecastSpreadMethod.Rows[0];
						_hnRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
						_versionRID = Convert.ToInt32(dr["FV_RID"], CultureInfo.CurrentUICulture);
						_cdrRID = Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture);
						_spreadOption = (eSpreadOption)Convert.ToInt32(dr["SPREAD_OPTION"], CultureInfo.CurrentUICulture);
				        _ignoreLocks = Include.ConvertCharToBool(Convert.ToChar(dr["IGNORE_LOCKS"], CultureInfo.CurrentUICulture));				
						// BEGIN ANF - Weighting Multiple Basis
                        _multiLevel = Include.ConvertCharToBool(Convert.ToChar(dr["MULTI_LEVEL_IND"], CultureInfo.CurrentUICulture));
                        _fromLevelType = (eFromLevelsType)Convert.ToInt32(dr["FROM_LEVEL_TYPE"], CultureInfo.CurrentUICulture);
                        _fromLevelOffset = Convert.ToInt32(dr["FROM_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
                        _fromLevelSequence = Convert.ToInt32(dr["FROM_LEVEL_SEQ"], CultureInfo.CurrentUICulture);
                        _toLevelType = (eToLevelsType)Convert.ToInt32(dr["TO_LEVEL_TYPE"], CultureInfo.CurrentUICulture);
                        _toLevelOffset = Convert.ToInt32(dr["TO_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
                        _toLevelSequence = Convert.ToInt32(dr["TO_LEVEL_SEQ"], CultureInfo.CurrentUICulture);
						_equalizeWeighting = Include.ConvertCharToBool(Convert.ToChar(dr["EQUALIZE_WEIGHTING"], CultureInfo.CurrentUICulture));				
						// END ANF - Weighting Multiple Basis
						_overrideLowLevelRid = Convert.ToInt32(dr["OLL_RID"], CultureInfo.CurrentUICulture);
						_dsForecastSpread = GetForecastSpreadChildData();

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

		public DataSet GetForecastSpreadChildData()
		{ 
			try
			{
				_dsForecastSpread = MIDEnvironment.CreateDataSet();

				//DataTable dtGroupLevel = SetupCopyGroupLevelTable();
				DataTable dtBasis = SetupSpreadBasisTable();
				_dsForecastSpread.Tables.Add(dtBasis);
				return _dsForecastSpread;
			}

			catch (Exception Ex)
			{
				string message = Ex.ToString();
				throw;
			}
		}

		private DataTable SetupSpreadBasisTable()
		{
			DataTable dtBasis = MIDEnvironment.CreateDataTable("Basis");
			
			dtBasis.Columns.Add("DETAIL_SEQ", System.Type.GetType("System.Int32"));
			dtBasis.Columns.Add("FV_RID", System.Type.GetType("System.Int32"));
			dtBasis.Columns.Add("DateRange",System.Type.GetType("System.String"));
			dtBasis.Columns.Add("CDR_RID", System.Type.GetType("System.Int32"));
			dtBasis.Columns.Add("WEIGHT", System.Type.GetType("System.Double"));

            dtBasis = StoredProcedures.MID_METHOD_SPREAD_BASIS_DETAIL_READ.Read(_dba, METHOD_RID: _methodRid);
			
			dtBasis.TableName = "Basis";
			dtBasis.Columns[0].ColumnName = "DETAIL_SEQ";
			dtBasis.Columns[1].ColumnName = "FV_RID";
			dtBasis.Columns[2].ColumnName = "DateRange";
			dtBasis.Columns[3].ColumnName = "CDR_RID";
			dtBasis.Columns[4].ColumnName = "WEIGHT";

			return dtBasis;
		}

		public bool InsertMethod(int aMethodRID, TransactionData td)
		{
			bool InsertSuccessful = true;

			try
			{	
				_methodRid = aMethodRID;
               
                //// ANF - Weighting Multiple Basis: add EQUALIZE_WEIGHTING column
              
                int? OLL_RID_Nullable = null;
                if (_overrideLowLevelRid != Include.NoRID) OLL_RID_Nullable = _overrideLowLevelRid;
                StoredProcedures.MID_METHOD_SPREAD_FORECAST_INSERT.Insert(_dba,
                                                                               METHOD_RID: aMethodRID,
                                                                               HN_RID: _hnRID,
                                                                               FV_RID: _versionRID,
                                                                               CDR_RID: _cdrRID,
                                                                               SPREAD_OPTION: (int)_spreadOption,
                                                                               IGNORE_LOCKS: Include.ConvertBoolToChar(_ignoreLocks),
                                                                               MULTI_LEVEL_IND: Include.ConvertBoolToChar(_multiLevel),
                                                                               FROM_LEVEL_TYPE: (int)_fromLevelType,
                                                                               FROM_LEVEL_SEQ: _fromLevelSequence,
                                                                               FROM_LEVEL_OFFSET: _fromLevelOffset,
                                                                               TO_LEVEL_TYPE: (int)_toLevelType,
                                                                               TO_LEVEL_SEQ: _toLevelSequence,
                                                                               TO_LEVEL_OFFSET: _toLevelOffset,
                                                                               EQUALIZE_WEIGHTING: Include.ConvertBoolToChar(_equalizeWeighting),
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
				if (DeleteChildData() && UpdateForecastSpreadMethod(aMethodRID) && UpdateChildData())
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

		public bool UpdateForecastSpreadMethod(int aMethodRID)
		{
			bool UpdateSuccessful = true;

			try
			{
				_methodRid = aMethodRID;
              
                //// ANF - Weighting Multiple Basis: add EQUALIZE_WEIGHTING column
              
                int? OLL_RID_Nullable = null;
                if (_overrideLowLevelRid != Include.NoRID) OLL_RID_Nullable = _overrideLowLevelRid;
                StoredProcedures.MID_METHOD_SPREAD_FORECAST_UPDATE.Update(_dba,
                                                                        METHOD_RID: aMethodRID,
                                                                        HN_RID: _hnRID,
                                                                        FV_RID: _versionRID,
                                                                        CDR_RID: _cdrRID,
                                                                        SPREAD_OPTION: (int)_spreadOption,
                                                                        IGNORE_LOCKS: Include.ConvertBoolToChar(_ignoreLocks),
                                                                        MULTI_LEVEL_IND: Include.ConvertBoolToChar(_multiLevel),
                                                                        FROM_LEVEL_TYPE: (int)_fromLevelType,
                                                                        FROM_LEVEL_SEQ: _fromLevelSequence,
                                                                        FROM_LEVEL_OFFSET: _fromLevelOffset,
                                                                        TO_LEVEL_TYPE: (int)_toLevelType,
                                                                        TO_LEVEL_SEQ: _toLevelSequence,
                                                                        TO_LEVEL_OFFSET: _toLevelOffset,
                                                                        EQUALIZE_WEIGHTING: Include.ConvertBoolToChar(_equalizeWeighting),
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
				if (_dsForecastSpread == null)
				{
					return UpdateSuccessful;
				}
				
				DataView dv = new DataView();
                dv.Table = _dsForecastSpread.Tables["Basis"];
				for (int i = 0; i < dv.Count; i++)
				{

                    StoredProcedures.MID_METHOD_SPREAD_BASIS_DETAIL_INSERT.Insert(_dba,
                                                                                METHOD_RID: _methodRid,
                                                                                DETAIL_SEQ: (int)dv[i]["DETAIL_SEQ"],
                                                                                FV_RID: (int)dv[i]["FV_RID"],
                                                                                CDR_RID: (int)dv[i]["CDR_RID"],
                                                                                WEIGHT: Include.ConvertObjectToNullableDouble(dv[i]["WEIGHT"])
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
                    StoredProcedures.MID_METHOD_SPREAD_FORECAST_DELETE.Delete(_dba, METHOD_RID: aMethodRID);
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
                StoredProcedures.MID_METHOD_SPREAD_BASIS_DETAIL_DELETE.Delete(_dba, METHOD_RID: _methodRid);

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
                return StoredProcedures.MID_METHOD_SPREAD_FORECAST_READ_FROM_NODE.Read(_dba, HN_RID: aNodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
		
	}
}
