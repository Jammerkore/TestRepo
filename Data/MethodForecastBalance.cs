using System;
using System.Data;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	/// <summary>
	/// Inherits MethodBase containing all properties for a Method.
	/// Adds properties for ForecastBalance
	/// </summary>
	public class OTSForecastBalanceMethodData: MethodBaseData
	{

		private int				_filterRID;
		private int				_hnRID;
		private int				_highLevelVersionRID;
		private int				_cdrRID;
		private int				_lowLevelVersionRID;
		private eLowLevelsType	_lowLevelsType;
		private int				_lowLevelOffset;
		private int				_lowLevelSequence;
		private bool			_similarStoresInd;
		private bool			_ineligibleStoresInd;
		private int				_variableNumber;
		private eIterationType	_iterationType;
		private int				_iterationsCount;
		private eBalanceMode	_balanceMode;
		private string			_computationMode;
		private int				_overrideLowLevelRid;  //  Override low level enhancement
		// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
		private eMatrixType     _matrixType;
		private int             _modelRID;
		private DataSet 		_dsForecastSpread;
		// END MID Track #5647

		public int FilterRID
		{
			get	{return _filterRID;}
			set	{_filterRID = value;}
		}

		public int HnRID
		{
			get	{return _hnRID;}
			set	{_hnRID = value;}
		}

		public int HighLevelVersionRID
		{
			get {return _highLevelVersionRID;}
			set {_highLevelVersionRID = value;}
		}

		public int CDR_RID
		{
			get	{return _cdrRID;}
			set	{_cdrRID = value;	}
		}

		// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
		public eMatrixType Matrix_Type
		{
			get	{return _matrixType;}
			set	{_matrixType = value;	}
		}

		public int Model_RID
		{
			get	{return _modelRID;}
			set	{_modelRID = value;	}
		}
		// END MID Track #5647

		public int LowLevelVersionRID
		{
			get	{return _lowLevelVersionRID;}
			set	{_lowLevelVersionRID = value;	}
		}

		public eLowLevelsType LowLevelsType
		{
			get	{return _lowLevelsType;}
			set	{_lowLevelsType = value;	}
		}

		public int LowLevelOffset
		{
			get	{return _lowLevelOffset;}
			set	{_lowLevelOffset = value;	}
		}

		public int LowLevelSequence
		{
			get	{return _lowLevelSequence;}
			set	{_lowLevelSequence = value;	}
		}

		public bool SimilarStoresInd
		{
			get	{return _similarStoresInd;}
			set	{_similarStoresInd = value;	}
		}

		public bool IneligibleStoresInd
		{
			get	{return _ineligibleStoresInd;}
			set	{_ineligibleStoresInd = value;	}
		}

		public int VariableNumber
		{
			get	{return _variableNumber;}
			set	{_variableNumber = value;	}
		}

		public eIterationType IterationType
		{
			get	{return _iterationType;}
			set	{_iterationType = value;	}
		}

		public int IterationsCount
		{
			get	{return _iterationsCount;}
			set	{_iterationsCount = value;	}
		}

		// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
		public DataSet DSForecastSpread
		{
			get	{return _dsForecastSpread;}
			set	{_dsForecastSpread = value;}
		}
		// END MID Track #5647

		public eBalanceMode BalanceMode
		{
			get	{return _balanceMode;}
			set	{_balanceMode = value;	}
		}

		public string ComputationMode
		{
			get	{return _computationMode;}
			set	{_computationMode = value;	}
		}

		//  BEGIN Override low level enhancement
		public int OverrideLowLevelRid
		{
			get { return _overrideLowLevelRid; }
			set { _overrideLowLevelRid = value; }
		}
		//  END Override low level enhancement

		public OTSForecastBalanceMethodData()
		{
			
		}

		public OTSForecastBalanceMethodData(int aMethodRID, eChangeType changeType)
		{
			switch (changeType)
			{
				case eChangeType.populate:
					PopulateForecastBalance(aMethodRID);
					break;
			}
		}
		
		public bool PopulateForecastBalance(int aMethodRID)
		{
			try
			{
				if (PopulateMethod(aMethodRID))
				{

                    DataTable dtForecastBalance = MIDEnvironment.CreateDataTable();
                    dtForecastBalance = StoredProcedures.MID_METHOD_MATRIX_READ.Read(_dba, METHOD_RID: aMethodRID);

					if(dtForecastBalance.Rows.Count != 0)
					{
						DataRow dr = dtForecastBalance.Rows[0];
						if (dr["FILTER_RID"] == DBNull.Value)
						{
							_filterRID = Include.NoRID;
						}
						else
						{
							_filterRID = Convert.ToInt32(dr["FILTER_RID"], CultureInfo.CurrentUICulture);
						}
						_hnRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
						_highLevelVersionRID = Convert.ToInt32(dr["HIGH_LEVEL_FV_RID"], CultureInfo.CurrentUICulture);
						_cdrRID = Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture);
						// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
						if (dr["MATRIX_TYPE"] != DBNull.Value)
						{
							_matrixType = (eMatrixType)Convert.ToInt32(dr["MATRIX_TYPE"], CultureInfo.CurrentUICulture);
						}
						if (dr["MODEL_RID"] == DBNull.Value)
						{
							_modelRID = Include.NoRID;
						} 
						else 
						{
							_modelRID = Convert.ToInt32(dr["MODEL_RID"], CultureInfo.CurrentUICulture);
						}
                        _dsForecastSpread = GetForecastSpreadChildData(aMethodRID);
						// END MID Track #5647
						_lowLevelVersionRID = Convert.ToInt32(dr["LOW_LEVEL_FV_RID"], CultureInfo.CurrentUICulture);
						_lowLevelsType = (eLowLevelsType)(Convert.ToInt32(dr["LOW_LEVEL_TYPE"], CultureInfo.CurrentUICulture));
						_lowLevelOffset = Convert.ToInt32(dr["LOW_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
						_lowLevelSequence = Convert.ToInt32(dr["LOW_LEVEL_SEQUENCE"], CultureInfo.CurrentUICulture);
 						_similarStoresInd = Include.ConvertCharToBool(Convert.ToChar(dr["INCLUDE_SIMILAR_STORES_IND"], CultureInfo.CurrentUICulture));
						_ineligibleStoresInd = Include.ConvertCharToBool(Convert.ToChar(dr["INCLUDE_INELIGIBLE_STORES_IND"], CultureInfo.CurrentUICulture));
						_variableNumber = Convert.ToInt32(dr["VARIABLE_NUMBER"], CultureInfo.CurrentUICulture);
						_iterationType = (eIterationType)(Convert.ToInt32(dr["ITERATIONS_TYPE"], CultureInfo.CurrentUICulture));
						_iterationsCount = Convert.ToInt32(dr["ITERATIONS_COUNT"], CultureInfo.CurrentUICulture);
						_balanceMode = (eBalanceMode)(Convert.ToInt32(dr["BALANCE_MODE"], CultureInfo.CurrentUICulture));
						_computationMode = Convert.ToString(dr["CALC_MODE"], CultureInfo.CurrentUICulture);
						_overrideLowLevelRid = Convert.ToInt32(dr["OLL_RID"], CultureInfo.CurrentUICulture);
						return true;
					}
					else
						return false;
				}
				else
					return false;
			}
			catch ( Exception exc )
			{
				string message = exc.ToString();
				throw;
			}
		}

		// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
		public DataSet GetForecastSpreadChildData(int aMethodRID)
		{ 
			try
			{
                //_dsForecastSpread = new DataSet();
                //_dsForecastSpread.Locale = CultureInfo.InvariantCulture;
                _dsForecastSpread = MIDEnvironment.CreateDataSet();

				//DataTable dtGroupLevel = SetupCopyGroupLevelTable();
				DataTable dtBasis = SetupSpreadBasisTable(aMethodRID);
				//_dsForecastSpread.Tables.Add(dtGroupLevel);
				_dsForecastSpread.Tables.Add(dtBasis);
				return _dsForecastSpread;
			}

			catch (Exception Ex)
			{
				string message = Ex.ToString();
				throw;
			}
		}

		private DataTable SetupSpreadBasisTable(int aMethodRID)
		{

            //DataTable dtBasis = new DataTable("Basis");
            //dtBasis.Locale = CultureInfo.InvariantCulture;
            DataTable dtBasis = MIDEnvironment.CreateDataTable("Basis");
			
			dtBasis.Columns.Add("DETAIL_SEQ", System.Type.GetType("System.Int32"));
			//dtBasis.Columns.Add("Merchandise", System.Type.GetType("System.String"));
			//dtBasis.Columns.Add("HN_RID", System.Type.GetType("System.Int32"));
			dtBasis.Columns.Add("FV_RID", System.Type.GetType("System.Int32"));
			dtBasis.Columns.Add("DateRange",System.Type.GetType("System.String"));
			dtBasis.Columns.Add("CDR_RID", System.Type.GetType("System.Int32"));
			dtBasis.Columns.Add("WEIGHT", System.Type.GetType("System.Double"));
			//dtBasis.Columns.Add("INCLUDE_EXCLUDE", System.Type.GetType("System.Int32"));
			//dtBasis.Columns.Add("IncludeButton",System.Type.GetType("System.String"));

   
            dtBasis = StoredProcedures.MID_METHOD_MATRIX_BASIS_DETAILS_READ.Read(_dba, METHOD_RID: aMethodRID);
			
			dtBasis.TableName = "Basis";
			dtBasis.Columns[0].ColumnName = "DETAIL_SEQ";
			//dtBasis.Columns[1].ColumnName = "Merchandise";
			//dtBasis.Columns[2].ColumnName = "HN_RID";
			dtBasis.Columns[1].ColumnName = "FV_RID";
			dtBasis.Columns[2].ColumnName = "DateRange";
			dtBasis.Columns[3].ColumnName = "CDR_RID";
			dtBasis.Columns[4].ColumnName = "WEIGHT";
			//dtBasis.Columns[7].ColumnName = "INCLUDE_EXCLUDE";
			//dtBasis.Columns[8].ColumnName = "IncludeButton";

			return dtBasis;
		}
		// END MID Track #5647

		public DataTable GetAllForecastBalances()
		{
			try
			{
				
                DataTable dtForecastBalance = MIDEnvironment.CreateDataTable();
                dtForecastBalance = StoredProcedures.MID_METHOD_MATRIX_READ_ALL.Read(_dba);

				return dtForecastBalance;
			}
			catch ( Exception exc )
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataTable GetForecastBalanceBasis(int aMethodRID)
		{
			try
			{
				
				DataTable dtBasis = MIDEnvironment.CreateDataTable();
                dtBasis = StoredProcedures.MID_METHOD_MATRIX_BASIS_DETAILS_READ.Read(_dba, METHOD_RID: aMethodRID);

				return dtBasis;
			}
			catch ( Exception exc )
			{
				string message = exc.ToString();
				throw;
			}
		}

	
		public DataTable GetForecastBalanceUIWorkflows(int aMethodRID)
		{ 
			try
			{	
				WorkflowBaseData wbd = new WorkflowBaseData();
				// Begin MID ISssue #3501 - stodd
				return wbd.GetOTSMethodPropertiesUIWorkflows(aMethodRID);
				// End MID issue #3501
			}
			catch ( Exception exc )
			{
				string message = exc.ToString();
				throw;
			}
		}


		/// <summary>
		/// Insert a row in the ForecastBalance table
		/// </summary>
		/// <param name="td">An instance of the TransactionData class which contains the database connection</param>
		/// <param name="aMethodRID">The record ID of the method</param>
		public bool InsertForecastBalance(int aMethodRID, TransactionData td)
		{
//			bool InsertSuccessfull = true;
			try
			{	
               
                int? MODEL_RID_Nullable = null;
                if (_modelRID != Include.NoRID) MODEL_RID_Nullable = _modelRID;
                int? OLL_RID_Nullable = null;
                if (_overrideLowLevelRid != Include.NoRID) OLL_RID_Nullable = _overrideLowLevelRid;
                int rowsInserted = StoredProcedures.MID_METHOD_MATRIX_INSERT.Insert(td.DBA,
                                                                 METHOD_RID: aMethodRID,
                                                                 FILTER_RID: _filterRID,
                                                                 HN_RID: _hnRID,
                                                                 HIGH_LEVEL_FV_RID: _highLevelVersionRID,
                                                                 CDR_RID: _cdrRID,
                                                                 MODEL_RID: MODEL_RID_Nullable,
                                                                 MATRIX_TYPE: (int)_matrixType,
                                                                 LOW_LEVEL_FV_RID: _lowLevelVersionRID,
                                                                 LOW_LEVEL_TYPE: (int)_lowLevelsType,
                                                                 LOW_LEVEL_OFFSET: _lowLevelOffset,
                                                                 LOW_LEVEL_SEQUENCE: _lowLevelSequence,
                                                                 INCLUDE_INELIGIBLE_STORES_IND: Include.ConvertBoolToChar(_ineligibleStoresInd),
                                                                 INCLUDE_SIMILAR_STORES_IND: Include.ConvertBoolToChar(_similarStoresInd),
                                                                 VARIABLE_NUMBER: _variableNumber,
                                                                 ITERATIONS_TYPE: (int)_iterationType,
                                                                 ITERATIONS_COUNT: _iterationsCount,
                                                                 BALANCE_MODE: (int)_balanceMode,
                                                                 CALC_MODE: _computationMode,
                                                                 OLL_RID: OLL_RID_Nullable
                                                                 );
                return (rowsInserted > 0);
			}
			catch
			{
//				InsertSuccessfull = false;
				throw;
			}
//			return InsertSuccessfull;
		}

		/// <summary>
		/// Insert a row in the Forecast Balance Basis table
		/// </summary>
		/// <param name="td">An instance of the TransactionData class which contains the database connection</param>
		/// <param name="aMethodRID">The record ID of the method</param>
		public bool InsertForecastBalanceBasis(int aMethodRID, TransactionData td, int aSeqID, int aFvRID, int aCdrRID, float aWeight, bool aIsIncludedInd)
		{
			try
			{	

                int rowsInserted = StoredProcedures.MID_METHOD_MATRIX_BASIS_DETAILS_INSERT.Insert(td.DBA,
                                                                                                   METHOD_RID: aMethodRID,
                                                                                                   SEQ_ID: aSeqID,
                                                                                                   FV_RID: aFvRID,
                                                                                                   CDR_RID: aCdrRID,
                                                                                                   WEIGHT: aWeight,
                                                                                                   IS_INCLUDED_IND: Include.ConvertBoolToChar(aIsIncludedInd)
                                                                                                   );
                return (rowsInserted > 0);
			}
			catch
			{
				throw;
			}
		}

		
		public bool UpdateForecastBalance(int aMethodRID, TransactionData td)
		{
			bool UpdateSuccessfull = true;
			try
			{
				// sets fields to NULL when 'NoRid' is found.
               

				// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
                MIDDbParameter modelDbParm = null;
				if (_modelRID == Include.NoRID)
					modelDbParm = new MIDDbParameter("@MODEL_RID", DBNull.Value, eDbType.Int, eParameterDirection.Input);
				else
					modelDbParm = new MIDDbParameter("@MODEL_RID", _modelRID, eDbType.Int, eParameterDirection.Input);
				// END MID Track #5647

				MIDDbParameter ollDbParm = null;
				if (_overrideLowLevelRid == Include.NoRID)
					ollDbParm = new MIDDbParameter("@OLL_RID", DBNull.Value, eDbType.Int, eParameterDirection.Input);
				else
					ollDbParm = new MIDDbParameter("@OLL_RID", _overrideLowLevelRid, eDbType.Int, eParameterDirection.Input);

              
                int? MODEL_RID_Nullable = null;
                if (_modelRID != Include.NoRID) MODEL_RID_Nullable = _modelRID;
                int? OLL_RID_Nullable = null;
                if (_overrideLowLevelRid != Include.NoRID) OLL_RID_Nullable = _overrideLowLevelRid;
                StoredProcedures.MID_METHOD_MATRIX_UPDATE.Update(td.DBA,
                                                                 METHOD_RID: aMethodRID,
                                                                 FILTER_RID: _filterRID,
                                                                 HN_RID: _hnRID,
                                                                 HIGH_LEVEL_FV_RID: _highLevelVersionRID,
                                                                 CDR_RID: _cdrRID,
                                                                 MATRIX_TYPE: (int)_matrixType,
                                                                 MODEL_RID: MODEL_RID_Nullable,
                                                                 LOW_LEVEL_FV_RID: _lowLevelVersionRID,
                                                                 LOW_LEVEL_TYPE: (int)_lowLevelsType,
                                                                 LOW_LEVEL_OFFSET: _lowLevelOffset,
                                                                 LOW_LEVEL_SEQUENCE: _lowLevelSequence,
                                                                 INCLUDE_INELIGIBLE_STORES_IND: Include.ConvertBoolToChar(_ineligibleStoresInd),
                                                                 INCLUDE_SIMILAR_STORES_IND: Include.ConvertBoolToChar(_similarStoresInd),
                                                                 VARIABLE_NUMBER: _variableNumber,
                                                                 ITERATIONS_TYPE: (int)_iterationType,
                                                                 ITERATIONS_COUNT: _iterationsCount,
                                                                 BALANCE_MODE: (int)_balanceMode,
                                                                 CALC_MODE: _computationMode,
                                                                 OLL_RID: OLL_RID_Nullable
                                                                 );
	
				UpdateSuccessfull = true;
			}
			catch
			{
				UpdateSuccessfull = false;
				throw;
			}
			return UpdateSuccessfull;
		}

		public bool DeleteForecastBalance(int aMethodRID, TransactionData td)
		{
			bool DeleteSuccessfull = true;
			try
			{
                StoredProcedures.MID_METHOD_MATRIX_DELETE.Delete(td.DBA, METHOD_RID: aMethodRID);
	
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

		public bool DeleteForecastBalanceBasis(int aMethodRID, TransactionData td)
		{
			bool DeleteSuccessfull = true;
			try
			{	
                StoredProcedures.MID_METHOD_MATRIX_BASIS_DETAILS_DELETE.Delete(td.DBA, METHOD_RID: aMethodRID);
	
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
                return StoredProcedures.MID_METHOD_MATRIX_READ_FROM_NODE.Read(_dba, HN_RID: aNodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

       
		
	}
}
