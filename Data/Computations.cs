using System;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{

	public partial class ComputationData : DataLayer
	{	
		private StringBuilder _documentXML;
		int _recordsWritten = 0;
		private Hashtable _ComputationItems;
		
		public ComputationData() : base()
		{
			_ComputationItems = new Hashtable();
		}

		public int RecordsWritten
		{
			get
			{
				return _recordsWritten;
			}
		}

		public int ComputationModel_Add(string aComputationModelID, string aCalcMode)
		{
			try
			{
                //MIDDbParameter[] InParams  = { new MIDDbParameter("@MODEL_ID", aComputationModelID, eDbType.VarChar, eParameterDirection.Input),
                //                              new MIDDbParameter("@CALC_MODE", aCalcMode, eDbType.VarChar, eParameterDirection.Input)} ;
				
                //MIDDbParameter[] OutParams = { new MIDDbParameter("@MODEL_RID", DBNull.Value, eDbType.Int, eParameterDirection.Output) };
				
                //return _dba.ExecuteStoredProcedure("SP_MID_COMPUTATION_MODEL_INSERT", InParams, OutParams);
                return StoredProcedures.SP_MID_COMPUTATION_MODEL_INSERT.InsertAndReturnRID(_dba,
                                                                                         MODEL_ID: aComputationModelID,
                                                                                         CALC_MODE: aCalcMode
                                                                                         );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public bool ComputationModel_Update(int aComputationModelRID, string aComputationModelID, string aCalcMode)
		{
			try
			{
                //StringBuilder SQLCommand = new StringBuilder();
                //SQLCommand.Append("update COMPUTATION_MODEL");
                //SQLCommand.Append(" set COMP_MODEL_ID = @MODEL_ID,");
                //SQLCommand.Append("     CALC_MODE = @CALC_MODE");
                //SQLCommand.Append(" where COMP_MODEL_RID = @COMP_MODEL_RID");
                //MIDDbParameter[] InParams  = { new MIDDbParameter("@MODEL_ID", aComputationModelID, eDbType.VarChar, eParameterDirection.Input),
                //                              new MIDDbParameter("@CALC_MODE", aCalcMode, eDbType.VarChar, eParameterDirection.Input),
                //                              new MIDDbParameter("@COMP_MODEL_RID", aComputationModelRID, eDbType.Int, eParameterDirection.Input) };
				
                //return (_dba.ExecuteNonQuery( SQLCommand.ToString(), InParams ) > 0);
                int rowsUpdated = StoredProcedures.MID_COMPUTATION_MODEL_UPDATE.Update(_dba,
                                                                     COMP_MODEL_RID: aComputationModelRID,
                                                                     COMP_MODEL_ID: aComputationModelID,
                                                                     CALC_MODE: aCalcMode
                                                                     );
                return (rowsUpdated > 0);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}


		public DataTable ComputationModel_Read()
		{
			try
			{
                //StringBuilder SQLCommand = new StringBuilder();
                //SQLCommand.Append("SELECT COMP_MODEL_RID, COMP_MODEL_ID, CALC_MODE FROM COMPUTATION_MODEL");
					
                //return _dba.ExecuteSQLQuery( SQLCommand.ToString(), "COMPUTATION_MODEL" );
                return StoredProcedures.MID_COMPUTATION_MODEL_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable ComputationModel_Read(int aComputationModelRID)
		{
			try
			{
                //StringBuilder SQLCommand = new StringBuilder();
                //SQLCommand.Append("SELECT COMP_MODEL_RID, COMP_MODEL_ID, CALC_MODE FROM COMPUTATION_MODEL");
                //SQLCommand.Append(" where COMP_MODEL_RID = @COMP_MODEL_RID");;

                //MIDDbParameter[] InParams  = { new MIDDbParameter("@COMP_MODEL_RID", aComputationModelRID, eDbType.Int, eParameterDirection.Input)} ;
					
                //return _dba.ExecuteSQLQuery( SQLCommand.ToString(), "COMPUTATION_MODEL", InParams );
                return StoredProcedures.MID_COMPUTATION_MODEL_READ.Read(_dba, COMP_MODEL_RID: aComputationModelRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable ComputationModel_Read(string aComputationModelID)
		{
			try
			{
                //StringBuilder SQLCommand = new StringBuilder();
                //SQLCommand.Append("SELECT COMP_MODEL_RID, COMP_MODEL_ID, CALC_MODE FROM COMPUTATION_MODEL");
                //SQLCommand.Append( " where COMP_MODEL_ID = @COMP_MODEL_ID");

                //MIDDbParameter[] InParams  = { new MIDDbParameter("@COMP_MODEL_ID", aComputationModelID, eDbType.VarChar, eParameterDirection.Input)} ;
					
                //return _dba.ExecuteSQLQuery( SQLCommand.ToString(), "COMPUTATION_MODEL", InParams );
                return StoredProcedures.MID_COMPUTATION_MODEL_READ_FROM_ID.Read(_dba, COMP_MODEL_ID: aComputationModelID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public bool ComputationModelEntry_Add(int aComputationModelRID, int aComputationModelSequence, eComputationType aCompType, int aVersionRID, int aChangeVariable, int aProductLevel)
		{
			try
			{
                //StringBuilder SQLCommand = new StringBuilder();
                //SQLCommand.Append("insert into COMPUTATION_MODEL_ENTRY ");
                //SQLCommand.Append(" (COMP_MODEL_RID, COMP_MODEL_SEQUENCE, COMP_TYPE, FV_RID, CHANGE_VARIABLE, PRODUCT_LEVEL)");
                //SQLCommand.Append(" values (@COMP_MODEL_RID, @COMP_MODEL_SEQUENCE, @COMP_TYPE, @FV_RID, @CHANGE_VARIABLE, @PRODUCT_LEVEL)");
				
                //MIDDbParameter[] InParams  = { new MIDDbParameter("@COMP_MODEL_RID", aComputationModelRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@COMP_MODEL_SEQUENCE", aComputationModelSequence, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@COMP_TYPE", Convert.ToInt32(aCompType), eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@FV_RID", aVersionRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@CHANGE_VARIABLE", aChangeVariable, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@PRODUCT_LEVEL", aProductLevel, eDbType.Int, eParameterDirection.Input) };
				
                //return (_dba.ExecuteNonQuery( SQLCommand.ToString(), InParams ) > 0);
                int rowsInserted = StoredProcedures.MID_COMPUTATION_MODEL_ENTRY_INSERT.Insert(_dba,
                                                                                             COMP_MODEL_RID: aComputationModelRID,
                                                                                             COMP_MODEL_SEQUENCE: aComputationModelSequence,
                                                                                             COMP_TYPE: Convert.ToInt32(aCompType),
                                                                                             FV_RID: aVersionRID,
                                                                                             CHANGE_VARIABLE: aChangeVariable,
                                                                                             PRODUCT_LEVEL: aProductLevel
                                                                                             );
                return (rowsInserted > 0);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable ComputationModelEntry_Read(int aComputationModelRID)
		{
			try
			{
                //StringBuilder SQLCommand = new StringBuilder();
                //SQLCommand.Append("SELECT COMP_MODEL_RID, COMP_MODEL_SEQUENCE, COMP_TYPE, FV_RID, ");
                //SQLCommand.Append(" CHANGE_VARIABLE, PRODUCT_LEVEL FROM COMPUTATION_MODEL_ENTRY");
                //SQLCommand.Append(" where COMP_MODEL_RID = @COMP_MODEL_RID");

                //MIDDbParameter[] InParams  = { new MIDDbParameter("@COMP_MODEL_RID", aComputationModelRID, eDbType.Int, eParameterDirection.Input)} ;
					
                //return _dba.ExecuteSQLQuery( SQLCommand.ToString(), "COMPUTATION_MODEL_ENTRY", InParams );
                return StoredProcedures.MID_COMPUTATION_MODEL_ENTRY_READ.Read(_dba, COMP_MODEL_RID: aComputationModelRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public int ComputationGroup_Add(eProcesses aProcess, int aComputationModelRID)
		{
			try
			{
                //MIDDbParameter[] InParams  = { new MIDDbParameter("@PROCESS", Convert.ToInt32(aProcess, CultureInfo.CurrentUICulture), eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@COMP_MODEL_RID", aComputationModelRID, eDbType.Int, eParameterDirection.Input)} ;
				
                //MIDDbParameter[] OutParams = { new MIDDbParameter("@COMP_GROUP_RID", DBNull.Value, eDbType.Int, eParameterDirection.Output) };
				
                //return _dba.ExecuteStoredProcedure("SP_MID_COMPUTATION_GROUP_INSERT", InParams, OutParams);
                return StoredProcedures.SP_MID_COMPUTATION_GROUP_INSERT.InsertAndReturnRID(_dba,
                                                                                         PROCESS: Convert.ToInt32(aProcess, CultureInfo.CurrentUICulture),
                                                                                         COMP_MODEL_RID: aComputationModelRID
                                                                                         );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public bool DeleteComputationItems()
		{
			try
			{
                //StringBuilder SQLCommand = new StringBuilder();
                //SQLCommand.Append("truncate table COMPUTATION_ITEM"); 
                //return (_dba.ExecuteNonQuery( SQLCommand.ToString() ) > 0);
                int rowsDeleted = StoredProcedures.MID_COMPUTATION_ITEM_DELETE_ALL.Delete(_dba);
                return (rowsDeleted > 0);
			}
			catch 
			{
				throw;
			}
		}

		public bool DeleteProcessedComputationItems()
		{
			try
			{
                //StringBuilder SQLCommand = new StringBuilder();
                //SQLCommand.Append("delete from COMPUTATION_ITEM where ITEM_PROCESSED is not null"); 
                //return (_dba.ExecuteNonQuery( SQLCommand.ToString() ) > 0);
                int rowsDeleted = StoredProcedures.MID_COMPUTATION_ITEM_DELETE_ALL_PROCESSED.Delete(_dba);
                return (rowsDeleted > 0);
			}
			catch 
			{
				throw;
			}
		}

		public DataTable ComputationGroups_Read()
		{
			try
			{
                //StringBuilder SQLCommand = new StringBuilder();
                //SQLCommand.Append(" SELECT cg.CG_RID, cg.PROCESS, cg.COMP_MODEL_RID  ");
                //SQLCommand.Append(" FROM COMPUTATION_GROUP cg");
				
                //return _dba.ExecuteSQLQuery( SQLCommand.ToString(), "COMPUTATION_GROUP" );
                return StoredProcedures.MID_COMPUTATION_GROUP_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public bool DeleteComputationGroup(int aComputationGroupRID)
		{
			try
			{
                //StringBuilder SQLCommand = new StringBuilder();
                //SQLCommand.Append("DELETE FROM COMPUTATION_GROUP ");
                //SQLCommand.Append(" where CG_RID = @CG_RID");

                //MIDDbParameter[] InParams  = {   new MIDDbParameter("@CG_RID", aComputationGroupRID, eDbType.Int, eParameterDirection.Input) };
				
                //return (_dba.ExecuteNonQuery( SQLCommand.ToString(), InParams) > 0);
                int rowsDeleted = StoredProcedures.MID_COMPUTATION_GROUP_DELETE.Delete(_dba, CG_RID: aComputationGroupRID);
                return (rowsDeleted > 0);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void Computation_XMLInit()
		{
			try
			{
				_documentXML = new StringBuilder();
				// add root element
				_documentXML.Append("<root> ");
				_recordsWritten = 0;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void Computation_XMLInsert(eProcesses aProcess, int aComputationGroupRID, int aNodeRID, eComputationType aCompType,
			int aVersionRID, int aFiscalYearWeek)
		{
			try
			{
				if (!DuplicateComputationItem(aNodeRID, aCompType, aFiscalYearWeek, aVersionRID))
				{
					++_recordsWritten;
					// add node element with attributes
					_documentXML.Append(" <node PROCESS=\"");
					_documentXML.Append(Convert.ToInt32(aProcess).ToString());
					_documentXML.Append("\" CG_RID=\"");
					_documentXML.Append(aComputationGroupRID.ToString());
					_documentXML.Append("\" HN_RID=\"");
					_documentXML.Append(aNodeRID.ToString());
					_documentXML.Append("\" ITEM_TYPE=\"");
					_documentXML.Append(Convert.ToInt32(aCompType).ToString());
					_documentXML.Append("\" FV_RID=\"");
					_documentXML.Append(aVersionRID.ToString());
					_documentXML.Append("\" FISCAL_YEAR_WEEK=\"");
					_documentXML.Append(aFiscalYearWeek.ToString());
					_documentXML.Append("\"> ");

					// terminate node element
					_documentXML.Append(" </node>");
				}

				return;
			}
			catch
			{
				throw;
			}
		}

		private bool DuplicateComputationItem(int aNodeRID, eComputationType aCompType,
			int aFiscalYearWeek, int aVersionRID)
		{
			try
			{
				string key = aNodeRID.ToString() + "|" + Convert.ToInt32(aCompType).ToString() + "|" + aVersionRID.ToString()+ "|" + aFiscalYearWeek.ToString();
				if (_ComputationItems.Contains(key))
				{
					return true;
				}
				else
				{
					_ComputationItems.Add(key, null);
					return false;
				}
			}
			catch
			{
				throw;
			}
		}

		
		public void Computation_XMLWrite()
		{
			try
			{
				// only send document if values or flags were sent
				if (_recordsWritten > 0)
				{
					// terminate root element
					_documentXML.Append(" </root>");
                    //MIDDbParameter[] InParams  = { new MIDDbParameter("@xmlDoc", _documentXML.ToString(), eDbType.Text) };	  
                    //InParams[0].Direction = eParameterDirection.Input;
				
                    //_dba.ExecuteStoredProcedure("dbo.SP_MID_XML_COMPUTATION_ITEM_WRITE", InParams);
                    StoredProcedures.SP_MID_XML_COMPUTATION_ITEM_WRITE.Insert(_dba, xmlDoc: _documentXML.ToString());
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}


        //Begin TT#846-MD -jsobek -New Stored Procedures for Performance -unused function
        //public bool ComputationItem_Insert(int aComputationGroupRID, int aNodeRID, eComputationType aCompType,
        //    int aVersionRID, int aFiscalYearWeek)
        //{
        //    try
        //    {
        //        StringBuilder SQLCommand = new StringBuilder();
        //        SQLCommand.Append("INSERT COMPUTATION_ITEM (CG_RID, HN_RID, ITEM_TYPE, FV_RID, FISCAL_YEAR_WEEK)");
        //        SQLCommand.Append(" VALUES (@CG_RID, @HN_RID, @ITEM_TYPE, @FV_RID, @FISCAL_YEAR_WEEK )");
        //        MIDDbParameter[] InParams  = {   new MIDDbParameter("@CG_RID", aComputationGroupRID, eDbType.Int, eParameterDirection.Input),
        //                                      new MIDDbParameter("@HN_RID", aNodeRID, eDbType.Int, eParameterDirection.Input),
        //                                      new MIDDbParameter("@ITEM_TYPE", Convert.ToInt32(aCompType), eDbType.Int, eParameterDirection.Input),
        //                                      new MIDDbParameter("@FV_RID", aVersionRID, eDbType.Int, eParameterDirection.Input),
        //                                      new MIDDbParameter("@FISCAL_YEAR_WEEK", aFiscalYearWeek, eDbType.Int, eParameterDirection.Input),
        //        };

        //        return (_dba.ExecuteNonQuery( SQLCommand.ToString(), InParams ) > 0);
        //    }
        //    catch ( Exception err )
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}
        

        //public DataTable ComputationItems_Read()
        //{
        //    try
        //    {
        //        StringBuilder SQLCommand = new StringBuilder();
        //        SQLCommand.Append(" SELECT ci.PROCESS, ci.CG_RID, ci.HN_RID, ci.ITEM_TYPE, ci.FV_RID, ci.FISCAL_YEAR_WEEK,  ");
        //        SQLCommand.Append(" cm.CALC_MODE  ");
        //        SQLCommand.Append(" FROM COMPUTATION_ITEM ci, COMPUTATION_GROUP cg, COMPUTATION_MODEL cm ");
        //        SQLCommand.Append(" where ci.CG_RID  = cg.CG_RID ");
        //        SQLCommand.Append("   and cg.COMP_MODEL_RID = cm.COMP_MODEL_RID  ");
				
        //        return _dba.ExecuteSQLQuery( SQLCommand.ToString(), "COMPUTATION_ITEM" );
        //    }
        //    catch ( Exception err )
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}
        //End TT#846-MD -jsobek -New Stored Procedures for Performance -unused function

		public DataTable ComputationItems_Read(eProcesses aProcess)
		{
			try
			{
                //StringBuilder SQLCommand = new StringBuilder();
                //SQLCommand.Append(" SELECT ci.PROCESS, ci.CG_RID, ci.HN_RID, ci.ITEM_TYPE, ci.FV_RID, ci.FISCAL_YEAR_WEEK,  ");
                //SQLCommand.Append(" cm.CALC_MODE  ");
                //SQLCommand.Append(" FROM COMPUTATION_ITEM ci, COMPUTATION_GROUP cg, COMPUTATION_MODEL cm ");
                //SQLCommand.Append(" where ci.PROCESS = @PROCESS");
                //SQLCommand.Append("   and ci.CG_RID  = cg.CG_RID ");
                //SQLCommand.Append("   and cg.COMP_MODEL_RID = cm.COMP_MODEL_RID  ");
                //SQLCommand.Append("   and ci.ITEM_PROCESSED is null  ");
                //SQLCommand.Append(" order by ci.CG_RID, ci.ITEM_TYPE, ci.HN_RID, ci.FV_RID, ci.FISCAL_YEAR_WEEK");

                //MIDDbParameter[] InParams  = {   new MIDDbParameter("@PROCESS", Convert.ToInt32(aProcess), eDbType.Int, eParameterDirection.Input) };
				
                //return _dba.ExecuteSQLQuery( SQLCommand.ToString(), "COMPUTATION_ITEM", InParams );
                return StoredProcedures.MID_COMPUTATION_ITEM_READ.Read(_dba, PROCESS: Convert.ToInt32(aProcess));
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        //Begin TT#846-MD -jsobek -New Stored Procedures for Performance -unused function
        //public DataTable ComputationItems_DistinctRead(eProcesses aProcess)
        //{
        //    try
        //    {
        //        StringBuilder SQLCommand = new StringBuilder();
        //        SQLCommand.Append("SELECT DISTINCT CG_RID, HN_RID, ITEM_TYPE, FV_RID FROM COMPUTATION_ITEM ");
        //        SQLCommand.Append(" where PROCESS = @PROCESS");

        //        MIDDbParameter[] InParams  = {   new MIDDbParameter("@PROCESS", Convert.ToInt32(aProcess), eDbType.Int, eParameterDirection.Input) };
				
        //        return _dba.ExecuteSQLQuery( SQLCommand.ToString(), "COMPUTATION_ITEM", InParams );
        //    }
        //    catch ( Exception err )
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}
        

        //public ArrayList GetComputationTypes(int aComputationGroupRID)
        //{
        //    try
        //    {
        //        ArrayList al = new ArrayList();
        //        StringBuilder SQLCommand = new StringBuilder();
        //        SQLCommand.Append("SELECT DISTINCT ITEM_TYPE FROM COMPUTATION_ITEM ");
        //        SQLCommand.Append(" where CG_RID = @CG_RID");
        //        SQLCommand.Append(" AND ITEM_PROCESSED is null ");
        //        SQLCommand.Append(" order by ITEM_TYPE");

        //        MIDDbParameter[] InParams  = {   new MIDDbParameter("@CG_RID", aComputationGroupRID, eDbType.Int, eParameterDirection.Input) };
				
        //        DataTable dt = _dba.ExecuteSQLQuery( SQLCommand.ToString(), "COMPUTATION TYPES", InParams );
        //        foreach(DataRow dr in dt.Rows)
        //        {
        //            al.Add(Convert.ToInt32(dr["ITEM_TYPE"], CultureInfo.CurrentUICulture));
        //        }
        //        return al;
        //    }
        //    catch ( Exception err )
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}
        

        //public ArrayList GetComputationVersions(int aComputationGroupRID)
        //{
        //    try
        //    {
        //        ArrayList al = new ArrayList();
        //        StringBuilder SQLCommand = new StringBuilder();
        //        SQLCommand.Append("SELECT DISTINCT FV_RID FROM COMPUTATION_ITEM ");
        //        SQLCommand.Append(" where CG_RID = @CG_RID");
        //        SQLCommand.Append(" AND ITEM_PROCESSED is null ");
        //        SQLCommand.Append(" order by FV_RID");

        //        MIDDbParameter[] InParams  = {   new MIDDbParameter("@CG_RID", aComputationGroupRID, eDbType.Int, eParameterDirection.Input) };
				
        //        DataTable dt = _dba.ExecuteSQLQuery( SQLCommand.ToString(), "COMPUTATION TYPES", InParams );
        //        foreach(DataRow dr in dt.Rows)
        //        {
        //            al.Add(Convert.ToInt32(dr["FV_RID"], CultureInfo.CurrentUICulture));
        //        }
        //        return al;
        //    }
        //    catch ( Exception err )
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}
        //End TT#846-MD -jsobek -New Stored Procedures for Performance -unused function


		public int AddComputationProcess(int aProcessRID, eComputationType aCompType, eProcessCompletionStatus aStatusCode)
		{
			try
			{
                //MIDDbParameter[] InParams  = { new MIDDbParameter("@PROCESS_RID", aProcessRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@COMPUTATION_TYPE", Convert.ToInt32(aCompType, CultureInfo.CurrentUICulture), eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@STATUS_CODE", Convert.ToInt32(aStatusCode, CultureInfo.CurrentUICulture), eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@START_TIME", DateTime.Now, eDbType.DateTime, eParameterDirection.Input)} ;
				
                //MIDDbParameter[] OutParams = { new MIDDbParameter("@COMPUTATION_RID", DBNull.Value, eDbType.Int, eParameterDirection.Output) };
				
                //return  _dba.ExecuteStoredProcedure("SP_MID_COMPUTATION_PROCESS_INSERT", InParams, OutParams);
                return StoredProcedures.SP_MID_COMPUTATION_PROCESS_INSERT.InsertAndReturnRID(_dba,
                                                                                             PROCESS_RID: aProcessRID,
                                                                                             COMPUTATION_TYPE: Convert.ToInt32(aCompType, CultureInfo.CurrentUICulture),
                                                                                             STATUS_CODE: Convert.ToInt32(aStatusCode, CultureInfo.CurrentUICulture),
                                                                                             START_TIME: DateTime.Now
                                                                                             );

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void UpdateComputationDriverProcess(int aComputationRID, eProcessCompletionStatus aStatusCode)
		{
			try
			{
                //string SQLCommand = "UPDATE COMPUTATION_PROCESS SET "  
                //    + " STATUS_CODE = @STATUS_CODE," 
                //    + " STOP_TIME = @STOP_TIME" 
                //    + " WHERE COMPUTATION_RID = @COMPUTATION_RID";
                //MIDDbParameter[] InParams  = { new MIDDbParameter("@COMPUTATION_RID", aComputationRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@STATUS_CODE", Convert.ToInt32(aStatusCode, CultureInfo.CurrentUICulture), eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@STOP_TIME", DateTime.Now, eDbType.DateTime, eParameterDirection.Input) } ;
				
                //_dba.ExecuteNonQuery(SQLCommand, InParams);
                StoredProcedures.MID_COMPUTATION_PROCESS_UPDATE.Update(_dba,
                                                                       COMPUTATION_RID: aComputationRID,
                                                                       STATUS_CODE: Convert.ToInt32(aStatusCode, CultureInfo.CurrentUICulture),
                                                                       STOP_TIME: DateTime.Now
                                                                       );
				return;

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        //Begin TT#846-MD -jsobek -New Stored Procedures for Performance -unused function
        //public int GetItemCount()
        //{
        //    try
        //    {
        //        StringBuilder SQLCommand = new StringBuilder();
        //        SQLCommand.Append("SELECT COUNT(*) AS MyCount FROM COMPUTATION_ITEM ");

        //        return _dba.ExecuteRecordCount( SQLCommand.ToString());
        //    }
        //    catch ( Exception err )
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}
        //End TT#846-MD -jsobek -New Stored Procedures for Performance -unused function

		public int GetItemCount(int aComputationGroupRID)
		{
			try
			{
                //StringBuilder SQLCommand = new StringBuilder();
                //SQLCommand.Append("SELECT COUNT(*) AS MyCount FROM COMPUTATION_ITEM (NOLOCK) ");
                //SQLCommand.Append(" where CG_RID = @CG_RID");

                //MIDDbParameter[] InParams  = {   new MIDDbParameter("@CG_RID", aComputationGroupRID, eDbType.Int, eParameterDirection.Input) };
				
                //return _dba.ExecuteRecordCount( SQLCommand.ToString(), InParams);
                return StoredProcedures.MID_COMPUTATION_ITEM_READ_COUNT.ReadRecordCount(_dba, CG_RID: aComputationGroupRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public int GetItemCount(eProcesses aProcess)
		{
			try
			{
                //StringBuilder SQLCommand = new StringBuilder();
                //SQLCommand.Append("SELECT COUNT(*) AS MyCount FROM COMPUTATION_ITEM ");
                //SQLCommand.Append(" where PROCESS = @PROCESS");
                //SQLCommand.Append(" AND ITEM_PROCESSED is null");
				
                //MIDDbParameter[] InParams  = {   new MIDDbParameter("@PROCESS", Convert.ToInt32(aProcess), eDbType.Int, eParameterDirection.Input),
                //                          };

                //return _dba.ExecuteRecordCount( SQLCommand.ToString(), InParams);
                return StoredProcedures.MID_COMPUTATION_ITEM_READ_COUNT_FROM_PROCESS.ReadRecordCount(_dba, PROCESS: Convert.ToInt32(aProcess));
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        //Begin TT#846-MD -jsobek -New Stored Procedures for Performance -unused function
        //public int GetItemCount(int aComputationGroupRID, eComputationType aCompType, int aVersionRID)
        //{
        //    try
        //    {
        //        StringBuilder SQLCommand = new StringBuilder();
        //        SQLCommand.Append("SELECT COUNT(*) AS MyCount FROM COMPUTATION_ITEM ");
        //        SQLCommand.Append(" where CG_RID = @CG_RID");
        //        SQLCommand.Append(" and ITEM_TYPE = @ITEM_TYPE");
        //        SQLCommand.Append(" and FV_RID = @FV_RID");
				
        //        MIDDbParameter[] InParams  = {   new MIDDbParameter("@CG_RID", aComputationGroupRID, eDbType.Int, eParameterDirection.Input),
        //                                      new MIDDbParameter("@ITEM_TYPE", Convert.ToInt32(aCompType), eDbType.Int, eParameterDirection.Input),
        //                                      new MIDDbParameter("@FV_RID", aVersionRID, eDbType.Int, eParameterDirection.Input)
        //        };

        //        return _dba.ExecuteRecordCount( SQLCommand.ToString(), InParams);
        //    }
        //    catch ( Exception err )
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}
        //End TT#846-MD -jsobek -New Stored Procedures for Performance -unused function

		public int UpdateItemProcessed(eProcesses aProcess, int aComputationGroupRID, eComputationType aCompType, int aNodeRID, int aVersionRID, int aFromFiscalYearWeek, int aToFiscalYearWeek)
		{
			try
			{
                //StringBuilder SQLCommand = new StringBuilder();
                //SQLCommand.Append("UPDATE COMPUTATION_ITEM ");
                //SQLCommand.Append(" SET ITEM_PROCESSED = '1' ");
                //SQLCommand.Append(" where PROCESS = @PROCESS");
                //SQLCommand.Append(" and CG_RID = @CG_RID");
                //SQLCommand.Append(" and ITEM_TYPE = @ITEM_TYPE");
                //SQLCommand.Append(" and HN_RID = @HN_RID");
                //SQLCommand.Append(" and FV_RID = @FV_RID");
                //SQLCommand.Append(" and FISCAL_YEAR_WEEK between @FROM_FISCAL_YEAR_WEEK and @TO_FISCAL_YEAR_WEEK");
				
                //MIDDbParameter[] InParams  = {   new MIDDbParameter("@PROCESS", Convert.ToInt32(aProcess), eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@CG_RID", aComputationGroupRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@ITEM_TYPE", Convert.ToInt32(aCompType), eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@HN_RID", aNodeRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@FV_RID", aVersionRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@FROM_FISCAL_YEAR_WEEK", aFromFiscalYearWeek, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@TO_FISCAL_YEAR_WEEK", aToFiscalYearWeek, eDbType.Int, eParameterDirection.Input)
                //                          };

                //return _dba.ExecuteRecordCount( SQLCommand.ToString(), InParams);
                return StoredProcedures.MID_COMPUTATION_ITEM_UPDATE_TO_PROCESSED.Update(_dba,
                                                                                        PROCESS: Convert.ToInt32(aProcess),
                                                                                        CG_RID: aComputationGroupRID,
                                                                                        ITEM_TYPE: Convert.ToInt32(aCompType),
                                                                                        HN_RID: aNodeRID,
                                                                                        FV_RID: aVersionRID,
                                                                                        FROM_FISCAL_YEAR_WEEK: aFromFiscalYearWeek,
                                                                                        TO_FISCAL_YEAR_WEEK: aToFiscalYearWeek
                                                                                        );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        //Begin TT#846-MD -jsobek -New Stored Procedures for Performance -unused function
        //public DataTable GetWorkComputationTableNames(string aTableNamePrefix)
        //{
        //    try
        //    {
        //        StringBuilder SQLCommand = new StringBuilder();
        //        SQLCommand.Append("select name from sysobjects where xtype = 'U' and name like '" + aTableNamePrefix + "%'");
				
        //        return _dba.ExecuteSQLQuery( SQLCommand.ToString(), "WorkComputationTableNames" );
        //    }
        //    catch ( Exception err )
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}
        //End TT#846-MD -jsobek -New Stored Procedures for Performance -unused function

	}
}
