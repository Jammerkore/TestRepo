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
	public class OTSRollupMethodData: MethodBaseData
	{
		private int				_methodRid;
		private int				_hnRID;
		private int				_versionRID;
		private int				_cdrRID;

        private DataSet          _dsRollup;
		
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
		
        public DataSet DSRollup
		{
            get { return _dsRollup; }
            set { _dsRollup = value; }
		}

		//public DataTable DTLowerLevels  <---DKJ
		//{
		//	get	{return _dtLowerLevels;}
		//	set	{_dtLowerLevels = value;}
		//}

		/// <summary>
        /// Creates an instance of the OTSRollupMethodData class.
		/// </summary>
		public OTSRollupMethodData()
		{
			
		}

		/// <summary>
        /// Creates an instance of the OTSRollupMethodData class.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		public OTSRollupMethodData(int aMethodRID, eChangeType changeType)
		{
			_methodRid = aMethodRID;
			switch (changeType)
			{
				case eChangeType.populate:
                    PopulateRollup(aMethodRID);
					break;
			}
		}

		/// <summary>
        /// Creates an instance of the OTSRollupMethodData class.
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		/// <param name="aMethodRID">The record ID of the method</param>
        public OTSRollupMethodData(TransactionData td, int aMethodRID)
		{
			_dba = td.DBA;
			_methodRid = aMethodRID;
		}

        public bool PopulateRollup(int aMethodRID)
		{
			try
			{
				if (PopulateMethod(aMethodRID))
				{
					_methodRid =  aMethodRID; 
					// ANF - Weighting Multiple Basis: add EQUALIZE_WEIGHTING column 

                    DataTable dtRollupMethod = MIDEnvironment.CreateDataTable();
                    dtRollupMethod = StoredProcedures.MID_METHOD_ROLLUP_READ.Read(_dba,
                                                                 METHOD_RID: _methodRid,
                                                                 HN_RID: Include.NoRID,
                                                                 FV_RID: Include.NoRID,
                                                                 CDR_RID: Include.UndefinedCalendarDateRange
                                                                 );

                    if (dtRollupMethod.Rows.Count != 0)
					{
                        DataRow dr = dtRollupMethod.Rows[0];
						_hnRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
						_versionRID = Convert.ToInt32(dr["FV_RID"], CultureInfo.CurrentUICulture);
						_cdrRID = Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture);
                        _dsRollup = GetRollupChildData();
						//_dtLowerLevels = GetSpreadLowerLevelsTable(); <---DKJ

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

        public DataSet GetRollupChildData()
		{ 
			try
			{
                _dsRollup = MIDEnvironment.CreateDataSet();

				//DataTable dtGroupLevel = SetupCopyGroupLevelTable();
				DataTable dtBasis = SetupSpreadBasisTable();
                //_dsRollup.Tables.Add(dtGroupLevel);
                _dsRollup.Tables.Add(dtBasis);
                return _dsRollup;
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
            dtBasis.Columns.Add("FromLevel", System.Type.GetType("System.Object"));
            dtBasis.Columns.Add("FROM_LEVEL_HRID", System.Type.GetType("System.Int32"));
            dtBasis.Columns.Add("FROM_LEVEL_TYPE", System.Type.GetType("System.Int32"));
            dtBasis.Columns.Add("FROM_LEVEL_SEQ", System.Type.GetType("System.Int32"));
            dtBasis.Columns.Add("FROM_LEVEL_OFFSET", System.Type.GetType("System.Int32"));
            dtBasis.Columns.Add("ToLevel", System.Type.GetType("System.Object"));
            dtBasis.Columns.Add("TO_LEVEL_HRID", System.Type.GetType("System.Int32"));
            dtBasis.Columns.Add("TO_LEVEL_TYPE", System.Type.GetType("System.Int32"));
            dtBasis.Columns.Add("TO_LEVEL_SEQ", System.Type.GetType("System.Int32"));
            dtBasis.Columns.Add("TO_LEVEL_OFFSET", System.Type.GetType("System.Int32"));
            dtBasis.Columns.Add("Store", System.Type.GetType("System.Boolean"));
            dtBasis.Columns.Add("Chain", System.Type.GetType("System.Boolean"));
            dtBasis.Columns.Add("StoreToChain", System.Type.GetType("System.Boolean"));
            dtBasis.Columns.Add("STORE_IND", System.Type.GetType("System.Int32"));
            dtBasis.Columns.Add("CHAIN_IND", System.Type.GetType("System.Int32"));
            dtBasis.Columns.Add("STORE_TO_CHAIN_IND", System.Type.GetType("System.Int32"));

            dtBasis = StoredProcedures.MID_METHOD_ROLLUP_BASIS_DETAIL_READ.Read(_dba, METHOD_RID: _methodRid);
			
			dtBasis.TableName = "Basis";
			dtBasis.Columns[0].ColumnName = "DETAIL_SEQ";
            dtBasis.Columns[1].ColumnName = "FromLevel";
            dtBasis.Columns[2].ColumnName = "FROM_LEVEL_HRID";
            dtBasis.Columns[3].ColumnName = "FROM_LEVEL_TYPE";
            dtBasis.Columns[4].ColumnName = "FROM_LEVEL_SEQ";
            dtBasis.Columns[5].ColumnName = "FROM_LEVEL_OFFSET";
            dtBasis.Columns[6].ColumnName = "ToLevel";
            dtBasis.Columns[7].ColumnName = "TO_LEVEL_HRID";
            dtBasis.Columns[8].ColumnName = "TO_LEVEL_TYPE";
            dtBasis.Columns[9].ColumnName = "TO_LEVEL_SEQ";
            dtBasis.Columns[10].ColumnName = "TO_LEVEL_OFFSET";
            dtBasis.Columns[11].ColumnName = "Store";
            dtBasis.Columns[12].ColumnName = "Chain";
            dtBasis.Columns[13].ColumnName = "StoreToChain";
            dtBasis.Columns[14].ColumnName = "STORE_IND";
            dtBasis.Columns[15].ColumnName = "CHAIN_IND";
            dtBasis.Columns[16].ColumnName = "STORE_TO_CHAIN_IND";

            foreach (DataRow row in dtBasis.Rows)
            {
                row["Store"] = (Convert.ToChar(row["STORE_IND"]) == '1') ? true : false;
                row["Chain"] = (Convert.ToChar(row["CHAIN_IND"]) == '1') ? true : false;
                row["StoreToChain"] = (Convert.ToChar(row["STORE_TO_CHAIN_IND"]) == '1') ? true : false;
            }

			return dtBasis;
		}
		
     

		public bool InsertMethod(int aMethodRID, TransactionData td)
		{
			bool InsertSuccessful = true;

			try
			{	
				_methodRid = aMethodRID;
                //ANF - Weighting Multiple Basis: add EQUALIZE_WEIGHTING column 
                StoredProcedures.MID_METHOD_ROLLUP_INSERT.Insert(_dba,
                                                                 METHOD_RID: aMethodRID,
                                                                 HN_RID: _hnRID,
                                                                 FV_RID: _versionRID,
                                                                 CDR_RID: _cdrRID
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
                if (DeleteChildData() && UpdateRollupMethod(aMethodRID) && UpdateChildData())
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

        public bool UpdateRollupMethod(int aMethodRID)
		{
			bool UpdateSuccessful = true;

			try
			{
				_methodRid = aMethodRID;
                //// ANF - Weighting Multiple Basis: add EQUALIZE_WEIGHTING column
                StoredProcedures.MID_METHOD_ROLLUP_UPDATE.Update(_dba,
                                                                  METHOD_RID: aMethodRID,
                                                                  HN_RID: _hnRID,
                                                                  FV_RID: _versionRID,
                                                                  CDR_RID: _cdrRID
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
                if (_dsRollup == null)
				{
					return UpdateSuccessful;
				}
				 
				
                //for (int i = 0; i < _dtLowerLevels.Rows.Count; i++)  <---DKJ
                //{
                //    string addLowerLevel = BuildInsertLowerLevelStatement(_dtLowerLevels.Rows[i]);
                //    //====================================================================
                //    // If the lower level node's version matches the parent &
                //    // the switch is set to include, this is the default.  No reason to 
                //    // add it to the database, so the string is returned empty.
                //    //====================================================================
                //    if (addLowerLevel != string.Empty)
                //        _dba.ExecuteNonQuery(addLowerLevel);
                //}

				DataView dv = new DataView();
                dv.Table = _dsRollup.Tables["Basis"];
				for (int i = 0; i < dv.Count; i++)
				{
                    //string addBasis = BuildInsertBasisStatement(dv[i]);
                    //_dba.ExecuteNonQuery(addBasis);
                    InsertBasisDetail(dv[i]);
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



		private void InsertBasisDetail(DataRowView rv)
		{	
         

            int? FROM_LEVEL_HRID_Nullable = null;
            if (rv["FROM_LEVEL_HRID"] != System.DBNull.Value) FROM_LEVEL_HRID_Nullable = Convert.ToInt32(rv["FROM_LEVEL_HRID"]);

            int? FROM_LEVEL_TYPE_Nullable = null;
            if (rv["FROM_LEVEL_TYPE"] != System.DBNull.Value) FROM_LEVEL_TYPE_Nullable = Convert.ToInt32(rv["FROM_LEVEL_TYPE"]);

            int? FROM_LEVEL_SEQ_Nullable = null;
            if (rv["FROM_LEVEL_SEQ"] != System.DBNull.Value) FROM_LEVEL_SEQ_Nullable = Convert.ToInt32(rv["FROM_LEVEL_SEQ"]);

            int? FROM_LEVEL_OFFSET_Nullable = null;
            if (rv["FROM_LEVEL_OFFSET"] != System.DBNull.Value) FROM_LEVEL_OFFSET_Nullable = Convert.ToInt32(rv["FROM_LEVEL_OFFSET"]);

            int? TO_LEVEL_HRID_Nullable = null;
            if (rv["TO_LEVEL_HRID"] != System.DBNull.Value) TO_LEVEL_HRID_Nullable = Convert.ToInt32(rv["TO_LEVEL_HRID"]);

            int? TO_LEVEL_TYPE_Nullable = null;
            if (rv["TO_LEVEL_TYPE"] != System.DBNull.Value) TO_LEVEL_TYPE_Nullable = Convert.ToInt32(rv["TO_LEVEL_TYPE"]);

            int? TO_LEVEL_SEQ_Nullable = null;
            if (rv["TO_LEVEL_SEQ"] != System.DBNull.Value) TO_LEVEL_SEQ_Nullable = Convert.ToInt32(rv["TO_LEVEL_SEQ"]);

            int? TO_LEVEL_OFFSET_Nullable = null;
            if (rv["TO_LEVEL_OFFSET"] != System.DBNull.Value) TO_LEVEL_OFFSET_Nullable = Convert.ToInt32(rv["TO_LEVEL_OFFSET"]);

            int? STORE_IND_Nullable = null;
            if (rv["STORE_IND"] != System.DBNull.Value) STORE_IND_Nullable = Convert.ToInt32(rv["STORE_IND"]);

            int? CHAIN_IND_Nullable = null;
            if (rv["CHAIN_IND"] != System.DBNull.Value) CHAIN_IND_Nullable = Convert.ToInt32(rv["CHAIN_IND"]);

            //Begin TT1295-MD -jsobek -Store To Chain flag not saved or not retrieved
            //Store to chain indicator is a char on the database - but an int field here
            //char? STORE_TO_CHAIN_IND_Nullable = null;
            //if (rv["STORE_TO_CHAIN_IND"] != System.DBNull.Value) STORE_TO_CHAIN_IND_Nullable = Convert.ToChar(rv["STORE_TO_CHAIN_IND"]);
            int? STORE_TO_CHAIN_IND_Nullable = null;
            if (rv["STORE_TO_CHAIN_IND"] != System.DBNull.Value) STORE_TO_CHAIN_IND_Nullable = Convert.ToInt32(rv["STORE_TO_CHAIN_IND"]);
            //End TT1295-MD -jsobek -Store To Chain flag not saved or not retrieved

            StoredProcedures.MID_METHOD_ROLLUP_BASIS_DETAIL_INSERT.Insert(_dba,
                                                                      METHOD_RID: _methodRid,
                                                                      DETAIL_SEQ: Convert.ToInt32(rv["DETAIL_SEQ"]),
                                                                      FROM_LEVEL_HRID: FROM_LEVEL_HRID_Nullable,
                                                                      FROM_LEVEL_TYPE: FROM_LEVEL_TYPE_Nullable,
                                                                      FROM_LEVEL_SEQ: FROM_LEVEL_SEQ_Nullable,
                                                                      FROM_LEVEL_OFFSET: FROM_LEVEL_OFFSET_Nullable,
                                                                      TO_LEVEL_HRID: TO_LEVEL_HRID_Nullable,
                                                                      TO_LEVEL_TYPE: TO_LEVEL_TYPE_Nullable,
                                                                      TO_LEVEL_SEQ: TO_LEVEL_SEQ_Nullable,
                                                                      TO_LEVEL_OFFSET: TO_LEVEL_OFFSET_Nullable,
                                                                      STORE_IND: STORE_IND_Nullable,
                                                                      CHAIN_IND: CHAIN_IND_Nullable,
                                                                      STORE_TO_CHAIN_IND: STORE_TO_CHAIN_IND_Nullable
                                                                      );
		}	


		public bool DeleteMethod(int aMethodRID, TransactionData td)
		{
			_dba = td.DBA;

			bool DeleteSuccessfull = true;

			try
			{
				if (DeleteChildData())
				{
                    StoredProcedures.MID_METHOD_ROLLUP_DELETE.Delete(_dba, METHOD_RID: aMethodRID);
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
                StoredProcedures.MID_METHOD_ROLLUP_BASIS_DETAIL_DELETE.Delete(_dba, METHOD_RID: _methodRid);
				
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

        public DataTable MethodRollup_Read(int aHierNodeRID, int aVersionRID, int aCDR_RID)
        {
            try
            {
                return StoredProcedures.MID_METHOD_ROLLUP_BASIS_DETAIL_READ_FROM_NODE.Read(_dba,
                                                                                           HN_RID: aHierNodeRID,
                                                                                           FV_RID: aVersionRID,
                                                                                           CDR_RID: aCDR_RID
                                                                                           );
            }
            catch
            {
                throw;
            }
        }

        //Begin Track 6082 - JSmith - B OTB Rollup method zeroes out the dept's projected numbers
        public DataTable MethodRollup_Read(int aMethodRID)
        {
            try
            {
                //Begin TT#2090 - JSmith - Rollup method not logging parameters in audit
                return StoredProcedures.MID_METHOD_ROLLUP_BASIS_DETAIL_READ_FROM_METHOD.Read(_dba, METHOD_RID: aMethodRID);
            }
            catch
            {
                throw;
            }
        }
        //End Track 6082

	}
}
