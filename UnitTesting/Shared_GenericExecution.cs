using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MIDRetail.Data;

namespace UnitTesting
{
    public static class Shared_GenericExecution
    {
     


        public static void DoRead(baseStoredProcedure sp, DatabaseAccess dba, out bool hasError, out string failureMsg, out DataTable dt, out DataSet dsOutputParameters)
        {
            hasError = false;
            dt = null;
            failureMsg = string.Empty;
            try
            {
                CopyToGenericSP(sp);
                
                dt = genericSP.ExecuteStoredProcedureForRead(dba);
                dsOutputParameters = genericSP.GetOutputParametersAsDataSet();
            }
            catch (Exception ex)
            {
                hasError = true;
                failureMsg = "Error while reading..." + System.Environment.NewLine + ex.ToString();
                dsOutputParameters = null;
                return;
            }

        }
        public static void DoReadAsRecordCount(baseStoredProcedure sp, DatabaseAccess dba, out bool hasError, out string failureMsg, out int count)
        {
            hasError = false;
            count = 0;
            failureMsg = string.Empty;
            try
            {
                CopyToGenericSP(sp);
                count = genericSP.ExecuteStoredProcedureForRecordCount(dba);
            }
            catch (Exception ex)
            {
                hasError = true;
                failureMsg = "Error while reading..." + System.Environment.NewLine + ex.ToString();
                return;
            }

        }
        public static void DoReadScalar(baseStoredProcedure sp, DatabaseAccess dba, out bool hasError, out string failureMsg, out object scalarValue)
        {
            hasError = false;
            scalarValue = null;
            failureMsg = string.Empty;
            try
            {
        
                CopyToGenericSP(sp);
                scalarValue = genericSP.ExecuteStoredProcedureForScalarValue(dba);
            }
            catch (Exception ex)
            {
                hasError = true;
                failureMsg = "Error while reading..." + System.Environment.NewLine + ex.ToString();
                return;
            }

        }
        public static void DoInsert(baseStoredProcedure sp, DatabaseAccess dba, out bool hasError, out string failureMsg, out int rowsInserted, out DataSet dsOutputParameters)
        {
            dba.OpenUpdateConnection();
            hasError = false;
            rowsInserted = 0;
            failureMsg = string.Empty;
            dsOutputParameters = null;
            try
            {
                CopyToGenericSP(sp);
                rowsInserted = genericSP.ExecuteStoredProcedureForInsert(dba);
                dsOutputParameters = genericSP.GetOutputParametersAsDataSet();
            }
            catch (Exception ex)
            {
                hasError = true;
                failureMsg = "Error while inserting..." + System.Environment.NewLine + ex.ToString();
                dba.CloseUpdateConnection();
                return;
            }
            if (hasError == false)
            {
                dba.CommitData();
            }
            dba.CloseUpdateConnection();
        }
        public static void DoInsertAndReturnRID(baseStoredProcedure sp, DatabaseAccess dba, out bool hasError, out string failureMsg, out int newRowRID, out DataSet dsOutputParameters)
        {
            dba.OpenUpdateConnection();
            hasError = false;
            newRowRID = 0;
            failureMsg = string.Empty;
            try
            {
                CopyToGenericSP(sp);
                newRowRID = genericSP.ExecuteStoredProcedureForInsertAndReturnRID(dba);
                dsOutputParameters = genericSP.GetOutputParametersAsDataSet();
            }
            catch (Exception ex)
            {
                hasError = true;
                failureMsg = "Error while inserting..." + System.Environment.NewLine + ex.ToString();
                dba.CloseUpdateConnection();
                dsOutputParameters = null;
                return;
            }
            if (hasError == false)
            {
                dba.CommitData();
            }
            dba.CloseUpdateConnection();
        }
        public static void DoUpdate(baseStoredProcedure sp, DatabaseAccess dba, out bool hasError, out string failureMsg, out int rowsUpdated, out DataSet dsOutputParameters)
        {
            dba.OpenUpdateConnection();
            hasError = false;
            rowsUpdated = 0;
            failureMsg = string.Empty;
            try
            {
                CopyToGenericSP(sp);
                rowsUpdated = genericSP.ExecuteStoredProcedureForUpdate(dba);
                dsOutputParameters = genericSP.GetOutputParametersAsDataSet();
            }
            catch (Exception ex)
            {
                hasError = true;
                failureMsg = "Error while updating..." + System.Environment.NewLine + ex.ToString();
                dba.CloseUpdateConnection();
                dsOutputParameters = null;
                return;
            }
            if (hasError == false)
            {
                dba.CommitData();
            }
            dba.CloseUpdateConnection();
        }
        public static void DoDelete(baseStoredProcedure sp, DatabaseAccess dba, out bool hasError, out string failureMsg, out int rowsDeleted, out DataSet dsOutputParameters)
        {
            dba.OpenUpdateConnection();
            hasError = false;
            rowsDeleted = 0;
            failureMsg = string.Empty;
            try
            {
                CopyToGenericSP(sp);
                rowsDeleted = genericSP.ExecuteStoredProcedureForDelete(dba);
                dsOutputParameters = genericSP.GetOutputParametersAsDataSet();
            }
            catch (Exception ex)
            {
                hasError = true;
                failureMsg = "Error while deleting..." + System.Environment.NewLine + ex.ToString();
                dba.CloseUpdateConnection();
                dsOutputParameters = null;
                return;
            }
            if (hasError == false)
            {
                dba.CommitData();
            }
            dba.CloseUpdateConnection();
        }



        private static void CopyToGenericSP(MIDRetail.Data.baseStoredProcedure sp)
        {
            genericSP.procedureName = sp.procedureName;
            genericSP.inputParameterList.Clear();
            genericSP.outputParameterList.Clear();
            foreach (MIDRetail.Data.baseParameter param in sp.inputParameterList)
            {
                genericSP.inputParameterList.Add(param);
            }
            foreach (MIDRetail.Data.baseParameter param in sp.outputParameterList)
            {
                genericSP.outputParameterList.Add(param);
            }

        }
        public static genericSP_def genericSP = new genericSP_def();
        public class genericSP_def : baseStoredProcedure
        {
            public genericSP_def()
            {
                base.SetCommandTimeout(60);  //allow upto one minute to execute a test
            }

            public new DataTable ExecuteStoredProcedureForRead(DatabaseAccess dba)
            {
                return base.ExecuteStoredProcedureForRead(dba);
            }
            public new DataSet ExecuteStoredProcedureForReadAsDataSet(DatabaseAccess dba)
            {
                return base.ExecuteStoredProcedureForReadAsDataSet(dba);
            }
            public new int ExecuteStoredProcedureForInsert(DatabaseAccess dba)
            {
                return base.ExecuteStoredProcedureForInsert(dba);
            }
            public new int ExecuteStoredProcedureForInsertAndReturnRID(DatabaseAccess dba)
            {
                return base.ExecuteStoredProcedureForInsertAndReturnRID(dba);
            }
            public new int ExecuteStoredProcedureForUpdate(DatabaseAccess dba)
            {
                return base.ExecuteStoredProcedureForUpdate(dba);
            }
            public new int ExecuteStoredProcedureForDelete(DatabaseAccess dba)
            {
                return base.ExecuteStoredProcedureForDelete(dba);
            }
            public new int ExecuteStoredProcedureForRecordCount(DatabaseAccess dba)
            {
                return base.ExecuteStoredProcedureForRecordCount(dba);
            }
            public new object ExecuteStoredProcedureForScalarValue(DatabaseAccess dba)
            {
                return base.ExecuteStoredProcedureForScalarValue(dba);
            }
        }

        public static DataTable GetGenericDataTable(string conn, string cmdText, out bool hasError, out string failureMsg)
        {
            hasError = false;
            failureMsg = string.Empty;
            DataTable dt = null;
            try
            {
                System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(conn);
                System.Data.SqlClient.SqlCommand sqlCmd = new System.Data.SqlClient.SqlCommand(cmdText, sqlConn);

                dt = new DataTable();
                System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter(sqlCmd);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                hasError = true;
                failureMsg = "Error reading data..." + System.Environment.NewLine + ex.ToString();
            }
            return dt;
        }

        public static DataSet MakeUnitTestResultDataSet()
        {
            DataSet dsResults = new DataSet();
            dsResults.Tables.Add("Results");
            //dsResults.Tables[0].Columns.Add("environmentName", typeof(string));
            dsResults.Tables[0].Columns.Add("testName", typeof(string));
            dsResults.Tables[0].Columns.Add("procedureName", typeof(string));
            dsResults.Tables[0].Columns.Add("procedureType", typeof(string));
            dsResults.Tables[0].Columns.Add("executedSequence", typeof(string));
            dsResults.Tables[0].Columns.Add("sequence", typeof(string));
            dsResults.Tables[0].Columns.Add("isSuspended", typeof(string));
            dsResults.Tables[0].Columns.Add("resultKind", typeof(string));
            dsResults.Tables[0].Columns.Add("fieldName", typeof(string));
            dsResults.Tables[0].Columns.Add("expectedValue", typeof(string));
            dsResults.Tables[0].Columns.Add("actualValue", typeof(string));
            dsResults.Tables[0].Columns.Add("passFail", typeof(string));
            dsResults.Tables[0].Columns.Add("failureMessage", typeof(string));
            return dsResults;
        }
    }

}
