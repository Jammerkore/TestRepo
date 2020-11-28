using System;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	public partial class AuditData : DataLayer
	{
		public AuditData() : base()
		{
        }
      

		public DataTable ProcessAuditHeader_Read()
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_PROC_HDR_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable ProcessAuditHeader_Read(int Process_RID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_PROC_HDR_READ.Read(_dba, PROCESS_RID: Process_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

       
		public int ProcessAuditHeader_Add(eProcesses ProcessID, eProcessExecutionStatus ExecutionStatus, DateTime Start_Time, int User_RID, eMIDTextCode processSummary)
		{
			try
			{
                return StoredProcedures.SP_MID_PROC_HDR_INSERT.InsertAndReturnRID(_dba, 
                                                                                   PROCESS_ID: (int)ProcessID,
                                                                                   COMPLETION_STATUS_CODE: (int)eProcessCompletionStatus.None,
                                                                                   EXECUTION_STATUS_CODE: (int)ExecutionStatus,
                                                                                   START_TIME: Start_Time,
                                                                                   USER_RID: User_RID,
                                                                                   PROCESS_SUMMARY: (int)processSummary
                                                                                   );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void ProcessAuditHeader_Update(int Process_RID, eProcessCompletionStatus CompletionStatus, DateTime Stop_Time, eMIDTextCode processSummary, eMIDMessageLevel highestMessageLevel, string procDesc)
		{
			// resolve all parameters in command except those that might vary by database
			try
			{
                StoredProcedures.MID_PROC_HDR_UPDATE.Update(_dba, 
                                                            PROCESS_RID: Process_RID,
                                                            COMPLETION_STATUS_CODE: (int)CompletionStatus,
                                                            EXECUTION_STATUS_CODE: (int)eProcessExecutionStatus.Completed,
                                                            STOP_TIME: Stop_Time,
                                                            SUMMARY_CODE: (int)processSummary,
                                                            HIGHEST_LEVEL: (int)highestMessageLevel,
                                                            PROC_DESC: Include.ConvertStringToChar(procDesc)
                                                            );
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		
		public void ProcessAuditHeader_Update(int Process_RID, eProcessExecutionStatus ExecutionStatus, DateTime Stop_Time, 
			eMIDTextCode processSummary, eMIDMessageLevel highestMessageLevel, string procDesc)
		{
			// resolve all parameters in command except those that might vary by database
			try
			{
                StoredProcedures.MID_PROC_HDR_UPDATE_NON_COMPLETE.Update(_dba, 
                                                                         PROCESS_RID: Process_RID,
                                                                         EXECUTION_STATUS_CODE: (int)ExecutionStatus,
                                                                         STOP_TIME: Stop_Time,
                                                                         SUMMARY_CODE: (int)processSummary,
                                                                         HIGHEST_LEVEL: (int)highestMessageLevel,
                                                                         PROC_DESC: Include.ConvertStringToChar(procDesc)
                                                                         );
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#739-MD - JSmith - Delete Stores
        public void CloseAuditHeaderIfUnexpected(int Process_RID)
        {
            try
            {
                DataTable dt = ProcessAuditHeader_Read(Process_RID);
                eProcessExecutionStatus status = (eProcessExecutionStatus)dt.Rows[0]["Execution Status Code"];
                if (status == eProcessExecutionStatus.Running)
                {
                    try
                    {
                        _dba.OpenUpdateConnection();
                        StoredProcedures.MID_PROC_RPT_UPDATE_IF_UNEXPECTED.Update(_dba, 
                                                                                  COMPLETION_STATUS_CODE: (int)eProcessCompletionStatus.Unexpected,
                                                                                  EXECUTION_STATUS_CODE: (int)eProcessExecutionStatus.Unexpected,
                                                                                  SUMMARY_CODE: (int)eProcessExecutionStatus.Unexpected,
                                                                                  PROCESS_RID: Process_RID
                                                                                  );
                        _dba.CommitData();
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        _dba.CloseUpdateConnection();
                    }
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#739-MD - JSmith - Delete Stores

        // Begin TT#4483 - JSmith - Modify Control Service start to not update other running services as Unexpected Termination
        public void MarkAllRunningForProcessAsUnexpectedTermination(eProcesses Process_ID)
        {
            try
            {
                StoredProcedures.MID_PROC_RPT_UPDATE_PROCESS_AS_UNEXPECTED_TERMINATION.Update(_dba,
                                                                                      PROCESS_ID: (int)Process_ID,
                                                                                      COMPLETION_STATUS_CODE: (int)eProcessCompletionStatus.Unexpected,
                                                                                      EXECUTION_STATUS_CODE: (int)eProcessExecutionStatus.Unexpected,
                                                                                      SUMMARY_CODE: (int)eMIDTextCode.sum_UnexpectedTermination
                                                                                      );
                return;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#4483 - JSmith - Modify Control Service start to not update other running services as Unexpected Termination

		public void MarkAllRunningAsUnexpectedTermination()
		{
			try
			{
                StoredProcedures.MID_PROC_RPT_UPDATE_AS_UNEXPECTED_TERMINATION.Update(_dba, 
                                                                                      COMPLETION_STATUS_CODE: (int)eProcessCompletionStatus.Unexpected,
                                                                                      EXECUTION_STATUS_CODE: (int)eProcessExecutionStatus.Unexpected,
                                                                                      SUMMARY_CODE: (int)eMIDTextCode.sum_UnexpectedTermination
                                                                                      );
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		
		public void ProcessAuditHeader_Delete(int Process_RID)
		{
			try
			{
                StoredProcedures.MID_PROC_HDR_DELETE.Delete(_dba, PROCESS_RID: Process_RID);
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable AuditReport_Read(int Process_RID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_PROC_RPT_READ_FOR_AUDIT_REPORT.Read(_dba, PROCESS_RID: Process_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#220 MD - JSmith - Modify audit to get column length once during initialization and not every time a message is added
        //public void AuditReport_Add(int processID, string reportingModule, int lineNumber,
        //    eMIDMessageLevel messageLevel, eMIDTextCode messageCode, string reportMessage)
        // End TT#220 MD
        public void AuditReport_Add(int processID, string reportingModule, int lineNumber, eMIDMessageLevel messageLevel, eMIDTextCode messageCode, string reportMessage, int AuditReportMessageLength)
		{
			try
			{
				string rptModule = "unknown";
				string[] moduleName;
                //Begin TT#1885 - DOConnell - Audit REPORT_MESSAGE is truncated on long messages which looses important information.
                // Begin TT#220 MD - JSmith - Modify audit to get column length once during initialization and not every time a message is added
                //int AuditReportMessageLength;
                //AuditReportMessageLength = GetColumnSize("PROC_RPT", "REPORT_MESSAGE");
                // End TT#220 MD
                //if (reportMessage != null && reportMessage.Length > Include.AuditReportMessageLength)
                //{
                //    reportMessage = reportMessage.Substring(0, Include.AuditReportMessageLength);   
                //}

                if (reportMessage != null && reportMessage.Length > AuditReportMessageLength)
                {
                    reportMessage = reportMessage.Substring(0, AuditReportMessageLength);
                }
                //End TT#1885 - DOConnell - Audit REPORT_MESSAGE is truncated on long messages which looses important information.
				if (reportingModule != null)
				{
					moduleName = reportingModule.Split('\\');
					rptModule = moduleName[moduleName.Length - 1];
				}
                //// Begin TT#1243 - JSmith - Audit Performance


                if (reportMessage.Length > AuditReportMessageLength)
                {
                    int index = 0;
                    while (index < reportMessage.Length)
                    {
                        StoredProcedures.MID_PROC_RPT_INSERT.Insert(_dba, 
                                                                    PROCESS_RID: processID,
                                                                    TIME_STAMP: DateTime.Now,
                                                                    REPORTING_MODULE: rptModule,
                                                                    LINE_NUMBER: lineNumber,
                                                                    MESSAGE_LEVEL: (int)messageLevel,
                                                                    MESSAGE_CODE: (int)messageCode,
                                                                    REPORT_MESSAGE: reportMessage.Substring(index, index + AuditReportMessageLength > reportMessage.Length ? reportMessage.Length - index : AuditReportMessageLength)
                                                                    );

                        index += AuditReportMessageLength;
                    }
                }
                else
                {  
                    StoredProcedures.MID_PROC_RPT_INSERT.Insert(_dba, 
                                                                PROCESS_RID: processID,
                                                                TIME_STAMP: DateTime.Now,
                                                                REPORTING_MODULE: rptModule,
                                                                LINE_NUMBER: lineNumber,
                                                                MESSAGE_LEVEL: (int)messageLevel,
                                                                MESSAGE_CODE: (int)messageCode,
                                                                REPORT_MESSAGE: reportMessage
                                                                );
                }
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void AuditReport_Add(int processID, string reportingModule, int lineNumber,
			eMIDMessageLevel messageLevel, eMIDTextCode messageCode)
		{
			try
			{
				string rptModule = "unknown";
				string[] moduleName;
				if (reportingModule != null)
				{
					moduleName = reportingModule.Split('\\');
					rptModule = moduleName[moduleName.Length - 1];
				}

                StoredProcedures.MID_PROC_RPT_INSERT.Insert(_dba, 
                                                            PROCESS_RID: processID,
                                                            TIME_STAMP: DateTime.Now,
                                                            REPORTING_MODULE: rptModule,
                                                            LINE_NUMBER: lineNumber,
                                                            MESSAGE_LEVEL: (int)messageLevel,
                                                            MESSAGE_CODE: (int)messageCode,
                                                            REPORT_MESSAGE: null
                                                            );
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#220 MD - JSmith - Modify audit to get column length once during initialization and not every time a message is added
        //public void AuditReport_Add(int processID, string reportingModule, int lineNumber,
        //    eMIDMessageLevel messageLevel, string reportMessage)
        public void AuditReport_Add(int processID, string reportingModule, int lineNumber,
            eMIDMessageLevel messageLevel, string reportMessage, int AuditReportMessageLength)
        // End TT#220 MD
		{
			try
			{
				string rptModule = "unknown";
				string[] moduleName;
                //Begin TT#1885 - DOConnell - Audit REPORT_MESSAGE is truncated on long messages which looses important information.
                // Begin TT#220 MD - JSmith - Modify audit to get column length once during initialization and not every time a message is added
                //int AuditReportMessageLength;
                //AuditReportMessageLength = GetColumnSize("PROC_RPT", "REPORT_MESSAGE");
                // End TT#220 MD
                //if (reportMessage.Length > Include.AuditReportMessageLength)
                //{
                //    reportMessage = reportMessage.Substring(0, Include.AuditReportMessageLength);
                //}

                // Begin TT#220 MD - JSmith - Modify audit to get column length once during initialization and not every time a message is added
                //if (reportMessage.Length > AuditReportMessageLength)
                //{
                //    reportMessage = reportMessage.Substring(0, AuditReportMessageLength);
                //}
                // End TT#220 MD
                //End TT#1885 - DOConnell - Audit REPORT_MESSAGE is truncated on long messages which looses important information.

				if (reportingModule != null)
				{
					moduleName = reportingModule.Split('\\');
					rptModule = moduleName[moduleName.Length - 1];
				}
                
                if (reportMessage.Length > AuditReportMessageLength)
                {
                    int index = 0;
                    while (index < reportMessage.Length)
                    {
                        StoredProcedures.MID_PROC_RPT_INSERT.Insert(_dba, 
                                                                    PROCESS_RID: processID,
                                                                    TIME_STAMP: DateTime.Now,
                                                                    REPORTING_MODULE: rptModule,
                                                                    LINE_NUMBER: lineNumber,
                                                                    MESSAGE_LEVEL: (int)messageLevel,
                                                                    MESSAGE_CODE: null,
                                                                    REPORT_MESSAGE: reportMessage.Substring(index, index + AuditReportMessageLength > reportMessage.Length ? reportMessage.Length - index : AuditReportMessageLength)
                                                                    );

                        index += AuditReportMessageLength;
                    }
                }
                else
                {
                    StoredProcedures.MID_PROC_RPT_INSERT.Insert(_dba, 
                                                                PROCESS_RID: processID,
                                                                TIME_STAMP: DateTime.Now,
                                                                REPORTING_MODULE: rptModule,
                                                                LINE_NUMBER: lineNumber,
                                                                MESSAGE_LEVEL: (int)messageLevel,
                                                                MESSAGE_CODE: null,
                                                                REPORT_MESSAGE: reportMessage
                                                                );
                }

				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void AuditReport_Delete(int Process_RID)
		{
			try
			{
                StoredProcedures.MID_PROC_RPT_DELETE.Delete(_dba, PROCESS_RID: Process_RID);
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable PostingAuditInfo_Read(int Process_RID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                //Begin TT#1267-MD -jsobek -5.22 Audit Summary results do not display
                //return StoredProcedures.MID_POSTING_INFO_READ_ALL.Read(_dba);
                return StoredProcedures.MID_POSTING_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);
                //End TT#1267-MD -jsobek -5.22 Audit Summary results do not display
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
        
		public void PostingAuditInfo_Add(int ProcessID, int CH_DAY_HIS_RECS, int CH_WK_HIS_RECS,
			int CH_WK_FOR_RECS, int	ST_DAY_HIS_RECS, int ST_WK_HIS_RECS, int ST_WK_FOR_RECS,
			int INTRANSIT_RECS, int RECS_WITH_ERRORS, int aNodesAdded)
		{
			try
			{
                StoredProcedures.MID_POSTING_INFO_INSERT.Insert(_dba, 
                                                                PROCESS_RID: ProcessID,
                                                                CH_DAY_HIS_RECS: CH_DAY_HIS_RECS,
                                                                CH_WK_HIS_RECS: CH_WK_HIS_RECS,
                                                                CH_WK_FOR_RECS: CH_WK_FOR_RECS,
                                                                ST_DAY_HIS_RECS: ST_DAY_HIS_RECS,
                                                                ST_WK_HIS_RECS: ST_WK_HIS_RECS,
                                                                ST_WK_FOR_RECS: ST_WK_FOR_RECS,
                                                                INTRANSIT_RECS: INTRANSIT_RECS,
                                                                RECS_WITH_ERRORS: RECS_WITH_ERRORS,
                                                                NODES_ADDED: aNodesAdded
                                                                );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void PostingAuditInfo_Delete(int Process_RID)
		{
			try
			{
                StoredProcedures.MID_POSTING_INFO_DELETE.Delete(_dba, PROCESS_RID: Process_RID);
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable HierarchyLoadAuditInfoAuditInfo_Read(int Process_RID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_HIER_LOAD_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
        //Begin TT228 - RBeck - Hierarchy Reclass message incorrectly displayed  
        public DataTable HierarchyReclassAuditInfo_Read(int Process_RID)
        {
            try
            {
                return StoredProcedures.MID_HIER_RECLASS_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //End   TT228 - RBeck - Hierarchy Reclass message incorrectly displayed  

		// Begin TT#1581-MD - stodd - API Header Reconcile
        public DataTable HeaderReconcileAuditInfo_Read(int Process_RID)
        {
            try
            {
                return StoredProcedures.MID_HEADER_RECONCILE_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
		// End TT#1581-MD - stodd - API Header Reconcile

        //Begin TT#106 MD - JSmith - Number of Nodes Added needs to be included in the Audit > Summary of the Hierarchy Load API
        //public void HierarchyLoadAuditInfo_Add(int ProcessID, int HIER_RECS, int LEVEL_RECS,
        //    int MERCH_RECS,	int RECS_WITH_ERRORS, int aMoveRecs, int aRenameRecs, int aDeleteRecs)
        //{
        //    try
        //    {
        //        string SQLCommand = "INSERT INTO HIER_LOAD_INFO(PROCESS_RID, HIER_RECS, LEVEL_RECS, MERCH_RECS, MOVE_RECS, RENAME_RECS, DELETE_RECS, RECS_WITH_ERRORS)"
        //            + " VALUES ("
        //            + ProcessID.ToString(CultureInfo.CurrentUICulture) + "," 
        //            + HIER_RECS.ToString(CultureInfo.CurrentUICulture) + ","
        //            + LEVEL_RECS.ToString(CultureInfo.CurrentUICulture) + ","
        //            + MERCH_RECS.ToString(CultureInfo.CurrentUICulture) + ","
        //            + aMoveRecs.ToString(CultureInfo.CurrentUICulture) + ","
        //            + aRenameRecs.ToString(CultureInfo.CurrentUICulture) + ","
        //            + aDeleteRecs.ToString(CultureInfo.CurrentUICulture) + ","
        //            + RECS_WITH_ERRORS.ToString(CultureInfo.CurrentUICulture) + ")";
        //        _dba.ExecuteNonQuery(SQLCommand);

        //        return;
        //    }
        //    catch ( Exception err )
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}
        public void HierarchyLoadAuditInfo_Add(int ProcessID, int HIER_RECS, int LEVEL_RECS,
            int MERCH_RECS, int RECS_WITH_ERRORS, int aMoveRecs, int aRenameRecs, int aDeleteRecs,
            int aProductsAdded, int aProductsUpdated)
        {
            try
            {
                StoredProcedures.MID_HIER_LOAD_INFO_INSERT.Insert(_dba, 
                                                                  PROCESS_RID: ProcessID,
                                                                  HIER_RECS: HIER_RECS,
                                                                  LEVEL_RECS: LEVEL_RECS,
                                                                  MERCH_RECS: MERCH_RECS,
                                                                  MOVE_RECS: aMoveRecs,
                                                                  RENAME_RECS: aRenameRecs,
                                                                  DELETE_RECS: aDeleteRecs,
                                                                  MERCH_ADDED: aProductsAdded,
                                                                  MERCH_UPDATED: aProductsUpdated,
                                                                  RECS_WITH_ERRORS: RECS_WITH_ERRORS
                                                                  );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //End TT#106 MD

		public void HierarchyLoadAuditInfo_Delete(int Process_RID)
		{
			try
			{
                StoredProcedures.MID_HIER_LOAD_INFO_DELETE.Delete(_dba, PROCESS_RID: Process_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable StoreLoadAuditInfoAuditInfo_Read(int Process_RID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_STR_LOAD_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
        
		// Begin MID Track #4668 - add number added and modified
//		public void StoreLoadAuditInfo_Add(int ProcessID, int STORE_RECS, int RECS_WITH_ERRORS)
		public void StoreLoadAuditInfo_Add(int ProcessID, int STORE_RECS, int RECS_WITH_ERRORS,
			int RECS_ADDED, int RECS_MODIFIED, int RECS_DELETED, int RECS_RECOVERED)	// TT#739-MD - STodd - delete stores
		// End MID Track #4668
		{
			try
			{
                StoredProcedures.MID_STR_LOAD_INFO_INSERT.Insert(_dba, 
                                                                 PROCESS_RID: ProcessID,
                                                                 STORE_RECS: STORE_RECS,
                                                                 STORES_CREATED: RECS_ADDED,
                                                                 STORES_MODIFIED: RECS_MODIFIED,
                                                                 STORES_DELETED: RECS_DELETED,
                                                                 STORES_RECOVERED: RECS_RECOVERED,
                                                                 RECS_WITH_ERRORS: RECS_WITH_ERRORS
                                                                 );
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void StoreLoadAuditInfo_Delete(int Process_RID)
		{
			try
			{
                StoredProcedures.MID_STR_LOAD_INFO_DELETE.Delete(_dba, PROCESS_RID: Process_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		// BEGIN Issue 5117 stodd special request
		public DataTable SpecialRequestAuditInfo_Read(int Process_RID)
		{
			try
			{
                return StoredProcedures.MID_SPECIAL_REQUEST_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SpecialRequestAuditInfo_Add(int processID, int totalJobs, int jobsProcessed,
			int jobsWithErrors, int successfulJobs)
		{
			try
			{
                StoredProcedures.MID_SPECIAL_REQUEST_INFO_INSERT.Insert(_dba, 
                                                                        PROCESS_RID: processID,
                                                                        TOTAL_JOBS: totalJobs,
                                                                        JOBS_PROCESSED: jobsProcessed,
                                                                        JOBS_WITH_ERRORS: jobsWithErrors,
                                                                        SUCCESSFUL_JOBS: successfulJobs
                                                                        );
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SpecialRequestAuditInfo_Delete(int Process_RID)
		{
			try
			{
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //string SQLCommand = "DELETE FROM SPECIAL_REQUEST_INFO WHERE PROCESS_RID = " + Process_RID.ToString(CultureInfo.CurrentUICulture);
                //_dba.ExecuteNonQuery(SQLCommand);
                 //MIDDbParameter[] InParams = { new MIDDbParameter("@PROCESS_RID", Process_RID, eDbType.Int) };
                 //_dba.ExecuteStoredProcedureForDelete("MID_SPECIAL_REQUEST_INFO_DELETE", InParams);
                StoredProcedures.MID_SPECIAL_REQUEST_INFO_DELETE.Delete(_dba, PROCESS_RID: Process_RID);
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
                
				return;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
		//END Issue 5117 stodd

		public DataTable PurgeAuditInfo_Read(int Process_RID)
		{
			try
			{
                //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
                //string SQLCommand = "SELECT PROCESS_RID, COALESCE(STORE_DAILY_HISTORY,0) STORE_DAILY_HISTORY, "
                //    + " COALESCE(CHAIN_WEEKLY_HISTORY,0) CHAIN_WEEKLY_HISTORY,"
                //    + " COALESCE(STORE_WEEKLY_HISTORY,0) STORE_WEEKLY_HISTORY,"
                //    + " COALESCE(CHAIN_WEEKLY_FORECAST,0) CHAIN_WEEKLY_FORECAST,"
                //    + " COALESCE(STORE_WEEKLY_FORECAST,0) STORE_WEEKLY_FORECAST,"
                //    //Begin Track #4815 - JSmith - #283-User (Security) Maintenance
                //    //					+ " COALESCE(HEADERS,0) HEADERS"
                //    + " COALESCE(HEADERS,0) HEADERS,"
                //    + " COALESCE(INTRANSIT,0) INTRANSIT,"
                //    //Begin TT#795 - JScott - STORE_INTRANSIT_REV table is not being purged
                //    + " COALESCE(INTRANSIT_REV,0) INTRANSIT_REV,"
                //    //End TT#795 - JScott - STORE_INTRANSIT_REV table is not being purged
                //    + " COALESCE(USERS,0) USERS,"
                //    // Begin TT#767 - JSmith - Purge Performance
                //    //+ " COALESCE(GROUPS,0) GROUPS"
                //    + " COALESCE(GROUPS,0) GROUPS,"
                //    // Begin TT#269-MD - JSmith - Modify Purge to use header weeks when purging daily percentages
                //    + " COALESCE(DAILY_PERCENTAGES,0) DAILY_PERCENTAGES,"
                //    // End TT#269-MD - JSmith - Modify Purge to use header weeks when purging daily percentages
                //    + " COALESCE(EMPTY_ATTRIBUTE_SETS,0) EMPTY_ATTRIBUTE_SETS,"		// TT#739-MD - STodd - delete stores
                //    + " COALESCE(AUDITS,0) AUDITS"
                //    // End TT#767
                //    //End Track #4815
                //    + " FROM PURGE_INFO ";
                //// end MID Track # 2354
                //SQLCommand += " WHERE PROCESS_RID=" + Process_RID.ToString(CultureInfo.CurrentUICulture);
				
                //return _dba.ExecuteSQLQuery( SQLCommand, "PurgeAuditInfo" );
                //MIDDbParameter[] InParams = { new MIDDbParameter("@PROCESS_RID", Process_RID, eDbType.Int) };
                //return _dba.ExecuteStoredProcedureForRead("MID_PURGE_INFO_READ", InParams);
                return StoredProcedures.MID_PURGE_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);
                //End TT#846-MD -jsobek -New Stored Procedures for Performance
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
        
		public void PurgeAuditInfo_Add(int ProcessID, 
                                       int STORE_DAILY_HISTORY, 
                                       int CHAIN_WEEKLY_HISTORY,
                                       int STORE_WEEKLY_HISTORY, 
                                       int CHAIN_WEEKLY_FORECAST, 
                                       int STORE_WEEKLY_FORECAST, 
                                       int HEADERS,
                                       int aIntransitRecs, 
                                       int aIntransitReviewRecs, 
                                       int aUserRecs, 
                                       int aGroupRecs, 
                                       int aAuditRecs, 
                                       int aDailyPercentages, 
                                       int emptyStoreSets,
                                       int aImoRev) //TT#1576-MD -jsobek -Data Layer Request - Add VSW Revision Records to Purge
		{
			try
			{
               
                StoredProcedures.MID_PURGE_INFO_INSERT.Insert(_dba, 
                                                              PROCESS_RID: ProcessID,
                                                              STORE_DAILY_HISTORY: STORE_DAILY_HISTORY,
                                                              CHAIN_WEEKLY_HISTORY: CHAIN_WEEKLY_HISTORY,
                                                              STORE_WEEKLY_HISTORY: STORE_WEEKLY_HISTORY,
                                                              CHAIN_WEEKLY_FORECAST: CHAIN_WEEKLY_FORECAST,
                                                              STORE_WEEKLY_FORECAST: STORE_WEEKLY_FORECAST,
                                                              HEADERS: HEADERS,
                                                              INTRANSIT: aIntransitRecs,
                                                              INTRANSIT_REV: aIntransitReviewRecs,
                                                              USERS: aUserRecs,
                                                              GROUPS: aGroupRecs,
                                                              DAILY_PERCENTAGES: aDailyPercentages,
                                                              EMPTY_ATTRIBUTE_SETS: emptyStoreSets,
                                                              AUDITS: aAuditRecs,
                                                              IMO_REV: aImoRev //TT#1576-MD -jsobek -Data Layer Request - Add VSW Revision Records to Purge
                                                              );
  
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void PurgeAuditInfo_Delete(int Process_RID)
		{
			try
			{
                StoredProcedures.MID_PURGE_INFO_DELETE.Delete(_dba, PROCESS_RID: Process_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable HeaderLoadAuditInfoAuditInfo_Read(int Process_RID)
		{
			try
			{
                // MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_HEADER_LOAD_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);     
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

//Begin TT228 - RBeck - Hierarchy Reclass message incorrectly displayed  
        public void HierarchyReclassAuditInfo_Add(  int aProcessRID
                                                    , int hierarchyReClsRecs
                                                    , int addChgReClsRecs
                                                    , int deleteReClsRecs
                                                    , int moveReClsRecs
                                                    , int rejectReClsRecs
                                                 )
        {
            try
            {
         
                StoredProcedures.MID_HIER_RECLASS_INFO_INSERT.Insert(_dba, 
                                                                     PROCESS_RID: aProcessRID,
                                                                     HEIR_TRANS_WRITTEN: hierarchyReClsRecs,
                                                                     ADDCHG_TRANS_WRITTEN: addChgReClsRecs,
                                                                     DELETE_TRANS_WRITTEN: deleteReClsRecs,
                                                                     MOVE_TRANS_WRITTEN: moveReClsRecs,
                                                                     TRANS_REJECTED: rejectReClsRecs
                                                                     );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
//End TT228 - RBeck

		// Begin TT#1581-MD - stodd - API Header Reconcile
        public void HeaderReconcileAuditInfo_Add(int aProcessRID
                                            , int filesRead
                                            , int recsRead
                                            , int recsWritten
                                            , int filesWritten
                                            , int duplicateRecsFound
                                            , int skippedRecs
                                            , int removeRecsWritten
                                            , int removeFilesWritten
                                         )
        {
            try
            {

                StoredProcedures.MID_HEADER_RECONCILE_INFO_INSERT.Insert(_dba,
                                                                     PROCESS_RID: aProcessRID,
                                                                     HEADER_FILES_READ: filesRead,
                                                                     HEADER_TRANS_READ: recsRead,
                                                                     HEADER_TRANS_WRITTEN: recsWritten,
                                                                     HEADER_FILES_WRITTEN: filesWritten,
                                                                     HEADER_TRANS_DUPLICATES: duplicateRecsFound,
                                                                     HEADER_TRANS_SKIPPED: skippedRecs,
                                                                     REMOVE_TRANS_WRITTEN: removeRecsWritten,
                                                                     REMOVE_FILES_WRITTEN: removeFilesWritten
                                                                     );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
		// End TT#1581-MD - stodd - API Header Reconcile



		public void HeaderLoadAuditInfo_Add(int aProcessID, int aRecsRead, int aRecsWithErrors,
			int aHdrsCreated, int aHdrsModified, int aHdrsRemoved, int aHdrsReset)
		{
			try
			{
         
                StoredProcedures.MID_HEADER_LOAD_INFO_INSERT.Insert(_dba, 
                                                                    PROCESS_RID: aProcessID,
                                                                    RECS_READ: aRecsRead,
                                                                    RECS_WITH_ERRORS: aRecsWithErrors,
                                                                    HDRS_CREATED: aHdrsCreated,
                                                                    HDRS_MODIFIED: aHdrsModified,
                                                                    HDRS_REMOVED: aHdrsRemoved,
                                                                    HDRS_RESET: aHdrsReset
                                                                    );

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void HeaderLoadAuditInfo_Delete(int Process_RID)
		{
			try
			{
                StoredProcedures.MID_HEADER_LOAD_INFO_DELETE.Delete(_dba, PROCESS_RID: Process_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		//Begin MOD - JScott - Build Pack Criteria Load
        public DataTable BuildPackCriteriaLoadAuditInfo_Read(int Process_RID) //TT#846-MD -jsobek -New Stored Procedures for Performance
		{
			try
			{
                // MID Track # 2354 - removed nolock because it causes concurrency issues            
                return StoredProcedures.MID_BUILD_PACK_CRITERIA_LOAD_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void BuildPackCriteriaLoadAuditInfo_Add(int aProcessID, int aRecsRead, int aRecsWithErrors, int aCriteriaAddedUpdated)
		{
			try
			{
                StoredProcedures.MID_BUILD_PACK_CRITERIA_LOAD_INFO_INSERT.Insert(_dba, 
                                                                                 PROCESS_RID: aProcessID,
                                                                                 RECS_READ: aRecsRead,
                                                                                 RECS_WITH_ERRORS: aRecsWithErrors,
                                                                                 CRITERIA_ADDED_UPDATED: aCriteriaAddedUpdated
                                                                                 );
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void BuildPackCriteriaLoadAuditInfo_Delete(int Process_RID)
		{
			try
			{
                StoredProcedures.MID_BUILD_PACK_CRITERIA_LOAD_INFO_DELETE.Delete(_dba, PROCESS_RID: Process_RID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		//End MOD - JScott - Build Pack Criteria Load

        //Begin TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2
        public DataTable ChainSetPercentCriteriaLoadAuditInfo_Read(int Process_RID)
        {
            try
            {
                // MID Track # 2354 - removed nolock because it causes concurrency issues 
                return StoredProcedures.MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void ChainSetPercentCriteriaLoadAuditInfo_Add(int aProcessID, int aRecsRead, int aRecsWithErrors, int aCriteriaAddedUpdated)
        {
            try
            {
                StoredProcedures.MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_INSERT.Insert(_dba, 
                                                                                        PROCESS_RID: aProcessID,
                                                                                        RECS_READ: aRecsRead,
                                                                                        RECS_WITH_ERRORS: aRecsWithErrors,
                                                                                        CRITERIA_ADDED_UPDATED: aCriteriaAddedUpdated
                                                                                        );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void ChainSetPercentCriteriaLoadAuditInfo_Delete(int Process_RID)
        {
            try
            {
                StoredProcedures.MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_DELETE.Delete(_dba, PROCESS_RID: Process_RID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //End TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2

        //BEGIN TT#43 – MD – DOConnell – Projected Sales Enhancement
        public DataTable DailyPercentagesCriteriaLoadAuditInfo_Read(int Process_RID)
        {
            try
            { 
                // MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void DailyPercentagesCriteriaLoadAuditInfo_Add(int aProcessID, int aRecsRead, int aRecsWithErrors, int aCriteriaAddedUpdated)
        {
            try
            {
                StoredProcedures.MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_INSERT.Insert(_dba, 
                                                                                        PROCESS_RID: aProcessID,
                                                                                        RECS_READ: aRecsRead,
                                                                                        RECS_WITH_ERRORS: aRecsWithErrors,
                                                                                        CRITERIA_ADDED_UPDATED: aCriteriaAddedUpdated
                                                                                        );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void DailyPercentagesCriteriaLoadAuditInfo_Delete(int Process_RID)
        {
            try
            {
                StoredProcedures.MID_DAILY_PERCENTAGES_CRITERIA_LOAD_INFO_DELETE.Delete(_dba, PROCESS_RID: Process_RID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //End TT#43 – MD – DOConnell – Projected Sales Enhancement

        //BEGIN TT#816 – MD – DOConnell – Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
        public DataTable StoreEligibilityCriteriaLoadAuditInfo_Read(int Process_RID)
        {
            try
            {
                // MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);          
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void StoreEligibilityCriteriaLoadAuditInfo_Add(int aProcessID, int aRecsRead, int aRecsWithErrors, int aCriteriaAddedUpdated)
        {
            try
            {
                StoredProcedures.MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_INSERT.Insert(_dba, 
                                                                                        PROCESS_RID: aProcessID,
                                                                                        RECS_READ: aRecsRead,
                                                                                        RECS_WITH_ERRORS: aRecsWithErrors,
                                                                                        CRITERIA_ADDED_UPDATED: aCriteriaAddedUpdated
                                                                                        );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void StoreEligibilityCriteriaLoadAuditInfo_Delete(int Process_RID)
        {
            try
            {
                StoredProcedures.MID_STORE_ELIGIBILITY_CRITERIA_LOAD_INFO_DELETE.Delete(_dba, PROCESS_RID: Process_RID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //End TT#816 – MD – DOConnell – Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API

        //BEGIN TT#817 – MD – DOConnell – Node Properties Maintenance Enhancement - Phase 1 - VSW Load API
        public DataTable VSWCriteriaLoadAuditInfo_Read(int Process_RID)
        {
            try
            {
                // MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_VSW_CRITERIA_LOAD_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);   
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void VSWCriteriaLoadAuditInfo_Add(int aProcessID, int aRecsRead, int aRecsWithErrors, int aCriteriaAddedUpdated)
        {
            try
            {
                StoredProcedures.MID_VSW_CRITERIA_LOAD_INFO_INSERT.Insert(_dba, 
                                                                          PROCESS_RID: aProcessID,
                                                                          RECS_READ: aRecsRead,
                                                                          RECS_WITH_ERRORS: aRecsWithErrors,
                                                                          CRITERIA_ADDED_UPDATED: aCriteriaAddedUpdated
                                                                          );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void VSWCriteriaLoadAuditInfo_Delete(int Process_RID)
        {
            try
            {
                StoredProcedures.MID_VSW_CRITERIA_LOAD_INFO_DELETE.Delete(_dba, PROCESS_RID: Process_RID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //End TT#817 – MD – DOConnell – Node Properties Maintenance Enhancement - Phase 1 - VSW Load API

		public DataTable ColorCodeLoadAuditInfo_Read(int Process_RID)
		{
			try
			{
                // MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_COLOR_CODE_LOAD_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void ColorCodeLoadAuditInfo_Add(int aProcessID, int aRecsRead, int aRecsWithErrors,
			int aCodesAdded, int aCodesUpdated)
		{
			try
			{
                StoredProcedures.MID_COLOR_CODE_LOAD_INFO_INSERT.Insert(_dba, 
                                                                        PROCESS_RID: aProcessID,
                                                                        RECS_READ: aRecsRead,
                                                                        RECS_WITH_ERRORS: aRecsWithErrors,
                                                                        CODES_CREATED: aCodesAdded,
                                                                        CODES_MODIFIED: aCodesUpdated
                                                                        );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void ColorCodeLoadAuditInfo_Delete(int Process_RID)
		{
			try
			{
                StoredProcedures.MID_COLOR_CODE_LOAD_INFO_DELETE.Delete(_dba, PROCESS_RID: Process_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable SizeConstraintsLoadAuditInfo_Read(int Process_RID)
		{
			try
			{
                // MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_SIZE_CONSTRAINT_LOAD_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable SizeCodeLoadAuditInfo_Read(int Process_RID)
		{
			try
			{
                // MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_SIZE_CODE_LOAD_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SizeConstraintsLoadAuditInfo_Add(int aProcessID, int aRecsRead, int aRecsWithErrors, int modelsCreated, int modelsModified, int ModelsRemoved)
		{
			try
			{
                StoredProcedures.MID_SIZE_CONSTRAINT_LOAD_INFO_INSERT.Insert(_dba, 
                                                                             PROCESS_RID: aProcessID,
                                                                             RECS_READ: aRecsRead,
                                                                             RECS_WITH_ERRORS: aRecsWithErrors,
                                                                             MODELS_CREATED: modelsCreated,
                                                                             MODELS_MODIFIED: modelsModified,
                                                                             MODELS_REMOVED: ModelsRemoved
                                                                             );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SizeCodeLoadAuditInfo_Add(int aProcessID, int aRecsRead, int aRecsWithErrors, int aCodesAdded, int aCodesUpdated)
		{
			try
			{
                StoredProcedures.MID_SIZE_CODE_LOAD_INFO_INSERT.Insert(_dba, 
                                                                       PROCESS_RID: aProcessID,
                                                                       RECS_READ: aRecsRead,
                                                                       RECS_WITH_ERRORS: aRecsWithErrors,
                                                                       CODES_CREATED: aCodesAdded,
                                                                       CODES_MODIFIED: aCodesUpdated
                                                                       );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SizeConstraintsLoadAuditInfo_Delete(int Process_RID)
		{
			try
			{
                StoredProcedures.MID_SIZE_CONSTRAINT_LOAD_INFO_DELETE.Delete(_dba, PROCESS_RID: Process_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SizeCodeLoadAuditInfo_Delete(int Process_RID)
		{
			try
			{
                StoredProcedures.MID_SIZE_CODE_LOAD_INFO_DELETE.Delete(_dba, PROCESS_RID: Process_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		//Begin TT#707 - JScott - Size Curve process needs to multi-thread
		public DataTable SizeCurveGenerateAuditInfo_Read(int Process_RID)
		{
			try
			{
                return StoredProcedures.MID_SIZE_CURVE_GENERATE_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SizeCurveGenerateAuditInfo_Add(int aProcessID, int aExecuted, int aSuccessful, int aFailed, int aNoAction)
		{
			try
			{
                StoredProcedures.MID_SIZE_CURVE_GENERATE_INFO_INSERT.Insert(_dba, 
                                                                            PROCESS_RID: aProcessID,
                                                                            MTHDS_EXECUTED: aExecuted,
                                                                            MTHDS_SUCCESSFUL: aSuccessful,
                                                                            MTHDS_FAILED: aFailed,
                                                                            MTHDS_NO_ACTION: aNoAction
                                                                            );
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void SizeCurveGenerateAuditInfo_Delete(int Process_RID)
		{
			try
			{
                StoredProcedures.MID_SIZE_CURVE_GENERATE_INFO_DELETE.Delete(_dba, PROCESS_RID: Process_RID);   
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		//End TT#707 - JScott - Size Curve process needs to multi-thread
		public DataTable RollupAuditInfo_Read(int Process_RID)
		{
			try
			{
                return StoredProcedures.MID_ROLLUP_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void RollupAuditInfo_Add(int aProcessID, int aTotalItems, int aBatchSize,
			int aConcurrentProcesses, int aTotalBatches, int aRecordsWithErrors)
		{
			try
			{
                StoredProcedures.MID_ROLLUP_INFO_INSERT.Insert(_dba, 
                                                               PROCESS_RID: aProcessID,
                                                               TOTAL_ITEMS: aTotalItems,
                                                               BATCH_SIZE: aBatchSize,
                                                               CONCURRENT_PROCESSES: aConcurrentProcesses,
                                                               TOTAL_BATCHES: aTotalBatches,
                                                               RECS_WITH_ERRORS: aRecordsWithErrors
                                                               );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void RollupAuditInfo_Delete(int Process_RID)
		{
			try
			{
                StoredProcedures.MID_ROLLUP_INFO_DELETE.Delete(_dba, PROCESS_RID: Process_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		// Begin MID Track # 4330 - database error when deleting Rollup item
		public void RollupProcess_Delete(int Process_RID)
		{
			try
			{
                StoredProcedures.MID_ROLLUP_PROCESS_DELETE.Delete(_dba, PROCESS_RID: Process_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		// End MID Track # 4330

		//Begin - Abercrombie & Fitch #4411 - JSmith - Chain Forecasting

		public DataTable ComputationDriverAuditInfo_Read(int aProcess_RID)
		{
			try
			{
                return StoredProcedures.MID_COMPUTATION_DRIVER_INFO_READ.Read(_dba, PROCESS_RID: aProcess_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void ComputationDriverAuditInfo_Add(int aProcess_RID, int aTotalItems, int aConcurrentProcesses, int aRecordsWithErrors)
		{
			try
			{
                StoredProcedures.MID_COMPUTATION_DRIVER_INFO_INSERT.Insert(_dba, PROCESS_RID: aProcess_RID,
                                                                           TOTAL_ITEMS: aTotalItems,
                                                                           CONCURRENT_PROCESSES: aConcurrentProcesses,
                                                                           RECS_WITH_ERRORS: aRecordsWithErrors
                                                                           );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void ComputationDriverAuditInfo_Delete(int aProcess_RID)
		{
			try
			{
                StoredProcedures.MID_COMPUTATION_DRIVER_INFO_DELETE.Delete(_dba, PROCESS_RID: aProcess_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void ComputationDriverProcess_Delete(int aProcess_RID)
		{
			try
			{
                StoredProcedures.MID_COMPUTATION_PROCESS_DELETE.Delete(_dba, PROCESS_RID: aProcess_RID);;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		//End - Abercrombie & Fitch #4411

        //Begin Track #5100 - JSmith - Add counts to audit

        public DataTable RelieveIntransitAuditInfo_Read(int aProcess_RID)
        {
            try
            {
                return StoredProcedures.MID_AUDIT_RELIEVE_INTRANSIT_INFO_READ.Read(_dba, PROCESS_RID: aProcess_RID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void RelieveIntransitAuditInfo_Add(int aProcess_RID, int aRecsRead, int aRecsAccepted, int aRecordsWithErrors)
        {
            try
            {
                StoredProcedures.MID_AUDIT_RELIEVE_INTRANSIT_INFO_INSERT.Insert(_dba, 
                                                                                PROCESS_RID: aProcess_RID,
                                                                                RECS_READ: aRecsRead,
                                                                                RECS_ACCEPTED: aRecsAccepted,
                                                                                RECS_WITH_ERRORS: aRecordsWithErrors
                                                                                );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void RelieveIntransitAuditInfo_Delete(int aProcess_RID)
        {
            try
            {
                StoredProcedures.MID_AUDIT_RELIEVE_INTRANSIT_INFO_DELETE.Delete(_dba, PROCESS_RID: aProcess_RID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End Track #5100

		public void Audit_Delete(int Process_RID)
		{
			try
			{
				this.OpenUpdateConnection();
				HierarchyLoadAuditInfo_Delete(Process_RID);
				StoreLoadAuditInfo_Delete(Process_RID);
				PostingAuditInfo_Delete(Process_RID);
				// Begin MID Track # 4319 - database error when deleting Header Load item
				HeaderLoadAuditInfo_Delete(Process_RID);
				// End MID Track # 4319
				//Begin MOD - JScott - Build Pack Criteria Load
				BuildPackCriteriaLoadAuditInfo_Delete(Process_RID);
				//End MOD - JScott - Build Pack Criteria Load
                //Begin TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2
                ChainSetPercentCriteriaLoadAuditInfo_Delete(Process_RID);
                //BEGIN TT#43 – MD – DOConnell – Projected Sales Enhancement
                DailyPercentagesCriteriaLoadAuditInfo_Delete(Process_RID);
                //END TT#43 – MD – DOConnell – Projected Sales Enhancement
                //End TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2
				// Begin MID Track # 4330 - database error when deleting Rollup item
				//Begin TT#707 - JScott - Size Curve process needs to multi-thread
				SizeCurveGenerateAuditInfo_Delete(Process_RID);
				//End TT#707 - JScott - Size Curve process needs to multi-thread
				RollupAuditInfo_Delete(Process_RID);
				RollupProcess_Delete(Process_RID);
				// End MID Track # 4330
				//Begin - Abercrombie & Fitch #4411 - JSmith - Chain Forecasting
				ComputationDriverAuditInfo_Delete(Process_RID);
				ComputationDriverProcess_Delete(Process_RID);
				//End - Abercrombie & Fitch #4411
                //Begin Track #5100 - JSmith - Add counts to audit
                RelieveIntransitAuditInfo_Delete(Process_RID);
                //End Track #5100
				ReclassAudit_Delete(Process_RID);
				AuditReport_Delete(Process_RID);
				// Begin MID Track #4438 - JSmith - foreign key constraint on delete
				AuditReclass_Delete(Process_RID);
                PushToBackStock_Delete(Process_RID); //TT#112 - RBeck - Purge error - Push to Back Stock
				// Begin MID Track #4438
				ProcessAuditHeader_Delete(Process_RID);
				// BEGIN Issue 5117 stodd 4.17.2008
				SpecialRequestAuditInfo_Delete(Process_RID);
				// END Issue 5117
				
				this.CommitData();

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
			finally
			{
				CloseUpdateConnection();
			}
		}

		public void AuditSummary_Delete(int Process_RID)
		{
			try
			{
				this.OpenUpdateConnection();
				HierarchyLoadAuditInfo_Delete(Process_RID);
				StoreLoadAuditInfo_Delete(Process_RID);
				// BEGIN Issue 5117 stodd 4.17.2008
				SpecialRequestAuditInfo_Delete(Process_RID);
				// END Issue 5117
				PostingAuditInfo_Delete(Process_RID);
				//Begin MOD - JScott - Build Pack Criteria Load
				BuildPackCriteriaLoadAuditInfo_Delete(Process_RID);
				//End MOD - JScott - Build Pack Criteria Load
                //Begin TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2
                ChainSetPercentCriteriaLoadAuditInfo_Delete(Process_RID);
                //End TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2
                //BEGIN TT#43 – MD – DOConnell – Projected Sales Enhancement
                DailyPercentagesCriteriaLoadAuditInfo_Delete(Process_RID);
                //END TT#43 – MD – DOConnell – Projected Sales Enhancement
				this.CommitData();

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
			finally
			{
				CloseUpdateConnection();
			}
		}

		public void AuditDetail_Delete(int Process_RID)
		{
			try
			{
				this.OpenUpdateConnection();
				AuditReport_Delete(Process_RID);
				
				this.CommitData();

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
			finally
			{
				CloseUpdateConnection();
			}
		}

		public void ReclassAudit_Add(int aProcessRID, string aReclassAction, string aReclassItemType, string aReclassItem, string aReclassComment)
		{
			try
			{
                StoredProcedures.SP_MID_RECLASS_AUDIT_INSERT.Insert(_dba, 
                                                                    PROCESS_RID: aProcessRID,
                                                                    RECLASS_ACTION: aReclassAction,
                                                                    RECLASS_ITEM_TYPE: aReclassItemType,
                                                                    RECLASS_ITEM: aReclassItem,
                                                                    RECLASS_COMMENT: aReclassComment
                                                                    );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void ReclassAudit_Delete(int aProcessRID)
		{
			try
			{
                StoredProcedures.MID_AUDIT_RECLASS_DELETE.Delete(_dba, PROCESS_RID: aProcessRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataSet ReclassAudit_Report(DataSet ds)
		{
			try
			{
                ds.Tables.Add(StoredProcedures.MID_AUDIT_RECLASS_READ_ALL.Read(_dba));
                ds.Tables[0].TableName = "AuditReclassDataTable";
                return ds;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}


		// Begin MID Track #4438 - JSmith - foreign key constraint on delete
		public void AuditReclass_Delete(int Process_RID)
		{
			try
			{
                StoredProcedures.MID_AUDIT_RECLASS_DELETE.Delete(_dba, PROCESS_RID: Process_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		// End MID Track #4438

		//Begin - Abercrombie & Fitch #4448 - JSmith - Audit
		/// <summary>
		/// Adds an audit row to the Audit_Header Table
		/// </summary>
		/// <param name="aProcessDateTime">Date_Time stamp when the header action or method completed</param>
		/// <param name="aProcessRID">Process RID</param>
		/// <param name="aUserRID">User RID</param>
		/// <param name="aHeaderRID">HeaderRID</param>
		/// <param name="aActionType">Action Type if this is an action</param>
		/// <param name="aMethodType">Method Type if this is a method</param>
		/// <param name="aMethodRID">Method RID if this is a method</param>
		/// <param name="aMethodName">Method Name if this is a method</param>
		/// <param name="aHeaderComponentType">Header Component Type on which the action or method was performed</param>
		/// <param name="aPackOrColorName">Pack name if this component is a pack component; color name if this is a color or size component</param>
		/// <param name="aSizeName">Size name if this component is a size component</param>
		/// <param name="UnitsAllocatedByProcess">Units allocated by this method or action</param>
		/// <param name="StoreAffectedByProcessCnt">Number of stores affected by the method or action</param>
		/// <returns></returns>
		public int AllocationAuditHeader_Add
			(DateTime aProcessDateTime, 
			int aProcessRID, 
			int aUserRID, 
			int aHeaderRID, 
			eAllocationActionType aActionType, 
			eMethodType aMethodType,
			int aMethodRID, 
			string aMethodName, 
			eComponentType aHeaderComponentType, 
			string aPackOrColorName, 
			string aSizeName,             // MID Track 4448 AnF Audit Enhancement
			int aUnitsAllocatedByProcess,  // MID Track 4448 AnF Audit Enhancement
			int aStoreAffectedByProcessCnt)// MID Track 4448 AnF Audit Enhancement
		{
			try
			{
                
                return StoredProcedures.SP_MID_AUDIT_HEADER_INSERT.InsertAndReturnRID(_dba, 
                                                                                       PROCESS_DATE_TIME: aProcessDateTime,
                                                                                       PROCESS_RID: aProcessRID,
                                                                                       USER_RID: aUserRID,
                                                                                       HDR_RID: aHeaderRID,
                                                                                       METHOD_RID: aMethodRID,
                                                                                       METHOD_NAME: aMethodName,
                                                                                       METHOD_TYPE: ((int)aMethodType),
                                                                                       HEADER_COMPONENT_TYPE: (int)aHeaderComponentType,
                                                                                       ACTION_TYPE: ((int)aActionType),
                                                                                       PACK_OR_COLOR_NAME: aPackOrColorName,
                                                                                       SIZE_NAME: aSizeName,
                                                                                       UNITS_ALLOCATED_BY_PROCESS: aUnitsAllocatedByProcess,
                                                                                       STORE_COUNT: aStoreAffectedByProcessCnt
                                                                                       );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

   

        

        ///<summary>
        ///Clears LAST_ENTRY for all rows in the Audit_Header Table associated with a particular Header (RID)
        ///</summary>
        ///<param name="aHdrRID">The RID of the header whose associated rows in the Audit Header Table are to be cleared</param>
        ///<param name="aComponentType">Component Type to clear</param>
        ///<param name="aPackOrColorComponentName">Pack or Color Component Name when type is Pack or Color</param>
        ///<param name="aSizeComponentName">Size Component Name when type is Size</param>
        public void AllocationAuditHeader_ClearLastEntryByHdrComponent
            (int aHdrRID, 
            eComponentType aComponentType,
            string aPackOrColorComponentName,
            string aSizeComponentName)
        {
            try
            {
                switch (aComponentType)
                {
                    case (eComponentType.Bulk):
                        {
                            StoredProcedures.MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_TYPE.Update(_dba, 
                                                                                                         HDR_RID: aHdrRID,
                                                                                                         COMPONENT_TYPE1: (int)aComponentType,
                                                                                                         COMPONENT_TYPE2: (int)eComponentType.SpecificColor
                                                                                                         );
                            break;
                        }
                    case (eComponentType.SpecificPack):
                    case (eComponentType.SpecificColor):
                        {
                            StoredProcedures.MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_AND_COLOR.Update(_dba, 
                                                                                                              HDR_RID: aHdrRID,
                                                                                                              HEADER_COMPONENT_TYPE: (int)aComponentType,
                                                                                                              PACK_OR_COLOR_NAME: aPackOrColorComponentName
                                                                                                              );
                            break;
                        }
                    case (eComponentType.ColorAndSize):
                    case (eComponentType.SpecificSize):
                        {
                            StoredProcedures.MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_COMPONENT_COLOR_SIZE.Update(_dba, 
                                                                                                               HDR_RID: aHdrRID,
                                                                                                               HEADER_COMPONENT_TYPE: (int)aComponentType,
                                                                                                               PACK_OR_COLOR_NAME: aPackOrColorComponentName,
                                                                                                               SIZE_NAME: aSizeComponentName
                                                                                                               );
                            break;
                        }
                    default:
                        {
                            // clear all audit records
                            StoredProcedures.MID_AUDIT_HEADER_UPDATE_LAST_ENTRY.Update(_dba, HDR_RID: aHdrRID);
                            break;
                        }
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        
	

        /// <summary>
        /// Clears LAST_ENTRY for all rows in the Audit_Header Table associated with a particular Header (RID)
        /// </summary>
        /// <param name="aHdrRID">The RID of the header whose associated rows in the Audit Header Table are to be cleared</param>
        public void AllocationAuditHeader_ClearLastEntryByHdrSizeActions
            (int aHdrRID,
            eComponentType aComponentType,
            string aPackOrColorComponentName,
            string aSizeComponentName)
        {
            try
            {
                System.Array actionTypes = Include.GetSizeFunctionCodeTypes();
              


                DataTable dtActionOrMethodList = new DataTable();
                dtActionOrMethodList.Columns.Add("ACTION_OR_METHOD_VALUE", typeof(int));
                foreach (int i in actionTypes)
                {
                    //ensure actionTypes are distinct, and only added to the datatable one time
                    if (dtActionOrMethodList.Select("ACTION_OR_METHOD_VALUE=" + i.ToString()).Length == 0)
                    {
                        DataRow dr = dtActionOrMethodList.NewRow();
                        dr["ACTION_OR_METHOD_VALUE"] = i;
                        dtActionOrMethodList.Rows.Add(dr);
                    }
                }
                switch (aComponentType)
                {
                    case (eComponentType.Bulk):
                        {
                            StoredProcedures.MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_AND_COMPONENT_TYPE.Update(_dba, 
                                                                                                                    HDR_RID: aHdrRID,
                                                                                                                    ACTION_OR_METHOD_LIST: dtActionOrMethodList,
                                                                                                                    COMPONENT_TYPE1: (int)aComponentType,
                                                                                                                    COMPONENT_TYPE2: (int)eComponentType.SpecificColor
                                                                                                                    );
                            break;
                        }
                    case (eComponentType.SpecificPack):
                    case (eComponentType.SpecificColor):
                        {
                            StoredProcedures.MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_COMPONENT_COLOR.Update(_dba, 
                                                                                                                 HDR_RID: aHdrRID,
                                                                                                                 ACTION_OR_METHOD_LIST: dtActionOrMethodList,
                                                                                                                 COMPONENT_TYPE: (int)aComponentType,
                                                                                                                 PACK_OR_COLOR_NAME: aPackOrColorComponentName
                                                                                                                 );
                            break;
                        }
                    case (eComponentType.ColorAndSize):
                    case (eComponentType.SpecificSize):
                        {
                            StoredProcedures.MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION_COMPONENT_COLOR_SIZE.Update(_dba, 
                                                                                                                        HDR_RID: aHdrRID,
                                                                                                                        ACTION_OR_METHOD_LIST: dtActionOrMethodList,
                                                                                                                        COMPONENT_TYPE: (int)aComponentType,
                                                                                                                        PACK_OR_COLOR_NAME: aPackOrColorComponentName,
                                                                                                                        SIZE_NAME: aSizeComponentName
                                                                                                                        );
                            break;
                        }
                    default:
                        {
                            // clear all audit records
                            StoredProcedures.MID_AUDIT_HEADER_UPDATE_LAST_ENTRY_BY_ACTION.Update(_dba,
                                                                                                 HDR_RID: aHdrRID,
                                                                                                 ACTION_OR_METHOD_LIST: dtActionOrMethodList
                                                                                                 );
                            break;
                        }
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //End TT#571-MD -jsobek -Allocation Audit needs to keep iterations of allocation

		// end MID Track 4967 Cancel Allocation must remove audit
		public int ForecastAuditForecast_Add(DateTime aProcessDateTime, int aProcessRID, int aUserRID,
            int aNodeRID, 
            string aNodeText,             //tt#1049 - Forecast Audit and Trend Tab Apply to do not match on the merchandise levels - apicchetti - 1/12/2010
            int aMethodRID, string aMethodName, eMethodType aMethodType, 
			int aStoreVersionRID, int aChainVersionRID,
			eCalendarRangeType aTimeRangeType, string aTimeRangeName, 
			string aTimeRangeDisplay, int aTimeRangeBegin, int aTimeRangeEnd, 
			int aAttributeRID, string aAttributeName)
		{
			try
			{
				char timeRangeType;
				switch (aTimeRangeType)
				{
					case eCalendarRangeType.Dynamic:
						timeRangeType = 'D';
						break;
					case eCalendarRangeType.Reoccurring:
						timeRangeType = 'R';
						break;
					case eCalendarRangeType.DynamicSwitch:
						timeRangeType = 'C';
						break;
					default:
						timeRangeType = 'S';
						break;
				}

               
                return StoredProcedures.SP_MID_AUDIT_FORECAST_INSERT.InsertAndReturnRID(_dba, 
                                                                                         PROCESS_DATE_TIME: aProcessDateTime,
                                                                                         PROCESS_RID: aProcessRID,
                                                                                         USER_RID: aUserRID,
                                                                                         HN_RID: aNodeRID,
                                                                                         METHOD_RID: aMethodRID,
                                                                                         METHOD_NAME: aMethodName,
                                                                                         METHOD_TYPE: ((int)aMethodType),
                                                                                         STORE_FV_RID: aStoreVersionRID,
                                                                                         CHAIN_FV_RID: aChainVersionRID,
                                                                                         TIME_RANGE_TYPE: timeRangeType,
                                                                                         TIME_RANGE_NAME: aTimeRangeName,
                                                                                         TIME_RANGE_DISPLAY: aTimeRangeDisplay,
                                                                                         TIME_RANGE_BEGIN: aTimeRangeBegin,
                                                                                         TIME_RANGE_END: aTimeRangeEnd,
                                                                                         SG_RID: aAttributeRID,
                                                                                         ATTRIBUTE_NAME: aAttributeName,
                                                                                         HN_TEXT: aNodeText
                                                                                         );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

    
		public void ForecastAuditForecast_Delete(int aAuditForecastRID)
		{
			try
			{
                StoredProcedures.SP_MID_AUDIT_FORECAST_DELETE.Delete(_dba, AUDIT_FORECAST_RID: aAuditForecastRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        public DataTable ForecastAuditForecast_GetLastProcessed(int Method_RID)
        {
            try
            {
                return StoredProcedures.MID_AUDIT_FORECAST_READ_LAST_PROCESSED.Read(_dba, METHOD_RID: Method_RID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

		public void ForecastAuditSet_Insert(int aAuditForecastRID, int aStoreGroupingLevelRID, string aStoreGroupingLevelName, string aForecastMethodType, bool aStockMinMax)
		{
			try
			{

                StoredProcedures.MID_AUDIT_OTS_FORECAST_SET_INSERT.Insert(_dba,
                                                                          AUDIT_FORECAST_RID: aAuditForecastRID,
                                                                          SGL_RID: aStoreGroupingLevelRID,
                                                                          SET_NAME: aStoreGroupingLevelName,
                                                                          FORECAST_METHOD_TYPE: aForecastMethodType,
                                                                          STOCK_MIN_MAX: Include.ConvertBoolToChar(aStockMinMax)
                                                                          );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

       
		public void ForecastAuditSetBasis_Insert(int aAuditForecastRID, int aStoreGroupingLevelRID, int aSequence,
			int aNodeRID, string aNodeText, int aVersionRID, string aTimePeriod, double aWeight, int basisSortCode, string basisType)
		{
			try
			{
                StoredProcedures.MID_AUDIT_OTS_FORECAST_SET_BASIS_INSERT.Insert(_dba,
                                                                                AUDIT_FORECAST_RID: aAuditForecastRID,
                                                                                SGL_RID: aStoreGroupingLevelRID,
                                                                                BASIS_SEQUENCE: aSequence,
                                                                                BASIS_HN_RID: aNodeRID,
                                                                                BASIS_HN_TEXT: aNodeText,
                                                                                BASIS_FV_RID: aVersionRID,
                                                                                BASIS_TIME_PERIOD: aTimePeriod,
                                                                                BASIS_WEIGHT: aWeight,
                                                                                BASIS_TYPE_SORT_CODE: basisSortCode,
                                                                                BASIS_TYPE: basisType
                                                                                );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

    
		public void ForecastAuditModifySales_Insert(int aAuditForecastRID, string aFilterName, string aAverageBy)
		{
			try
			{
                StoredProcedures.MID_AUDIT_MODIFY_SALES_INSERT.Insert(_dba,
                                                                      AUDIT_FORECAST_RID: aAuditForecastRID,
                                                                      FILTER_NAME: aFilterName,
                                                                      AVERAGE_BY: aAverageBy
                                                                      ); 
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

    

		public void ForecastAuditModifySalesMatrix_Insert(int aAuditForecastRID, 
					int sglRid, int boundary, string grade, int sellThru, int numStores, 
					eModifySalesRuleType rule, double ruleQty)
		{
			try
			{
                StoredProcedures.MID_AUDIT_MODIFY_SALES_MATRIX_INSERT.Insert(_dba,
                                                                             AUDIT_FORECAST_RID: aAuditForecastRID,
                                                                             SGL_RID: sglRid,
                                                                             BOUNDARY: boundary,
                                                                             GRADE_CODE: grade,
                                                                             SELL_THRU: sellThru,
                                                                             NUMBER_OF_STORES: numStores,
                                                                             MATRIX_RULE: (int)rule,
                                                                             MATRIX_RULE_QUANTITY: ruleQty
                                                                             );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		//End - Abercrombie & Fitch #4411

		// Begin TT#465 - stodd - performance 
		public DataTable SizeDayToWeekSummary_Read(int Process_RID)
		{
			try
			{
                return StoredProcedures.MID_SIZE_DAY_TO_WEEK_SUMMARY_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

      
		public DataTable PushToBackStock_Read(int Process_RID)
		{
			try
			{
                return StoredProcedures.MID_PUSH_TO_BACK_STOCK_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void PushToBackStock_Add(int ProcessID, int hdrsRead, int hdrsErrors,int hdrsProcessed, int hdrsSkipped)
		{
			try
			{
                StoredProcedures.MID_PUSH_TO_BACK_STOCK_INFO_INSERT.Insert(_dba,
                                                                           PROCESS_RID: ProcessID,
                                                                           HDRS_READ: hdrsRead,
                                                                           HDRS_WITH_ERRORS: hdrsErrors,
                                                                           HDRS_PROCESSED: hdrsProcessed,
                                                                           HDRS_SKIPPED: hdrsSkipped
                                                                           );
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void PushToBackStock_Delete(int Process_RID)
		{
			try
			{
                StoredProcedures.MID_PUSH_TO_BACK_STOCK_INFO_DELETE.Delete(_dba, PROCESS_RID: Process_RID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
		// END TT#1401 - stodd - add resevation stores (IMO)

        // Begin TT#710 - JSmith - Generate relieve intransit
        public DataTable GenerateRelieveIntransitSummary_Read(int Process_RID)
		{
			try
			{   
                return StoredProcedures.MID_GENERATE_RELIEVE_INTRANSIT_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);    
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        public void GenerateRelieveIntransitSummary_Add(int ProcessID, int aHeadersToRelieve, int aFilesGenerated, int aTotalErrors)
		{
			try
			{
       
                StoredProcedures.MID_GENERATE_RELIEVE_INTRANSIT_INFO_INSERT.Insert(_dba,
                                                                                   PROCESS_RID: ProcessID,
                                                                                   HEADERS_TO_RELIEVE: aHeadersToRelieve,
                                                                                   FILES_GENERATED: aFilesGenerated,
                                                                                   TOTAL_ERRORS: aTotalErrors
                                                                                   );
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
    
        // End TT#710

        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
        public DataTable DetermineHierarchyActivitySummary_Read(int Process_RID)
        {
            try
            {
                return StoredProcedures.MID_DETERMINE_NODE_ACTIVITY_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);      
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void DetermineHierarchyActivitySummary_Add(int ProcessID, int aTotalNodes, int aActiveNodes,
            int aInactiveNodes, int aTotalErrors)
        {
            try
            {
                StoredProcedures.MID_DETERMINE_NODE_ACTIVITY_INFO_INSERT.Insert(_dba,
                                                                                PROCESS_RID: ProcessID,
                                                                                TOTAL_NODES: aTotalNodes,
                                                                                ACTIVE_NODES: aActiveNodes,
                                                                                INACTIVE_NODES: aInactiveNodes,
                                                                                TOTAL_ERRORS: aTotalErrors
                                                                                );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        // End TT#988

        //Begin TT#1413-MD-stodd-Data Layer Request - SIZE_CURVE_LOAD_INFO
        public DataTable SizeCurveLoadAuditInfo_Read(int Process_RID)
        {
            try
            {
                return StoredProcedures.MID_SIZE_CURVE_LOAD_INFO_READ.Read(_dba, PROCESS_RID: Process_RID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void SizeCurveLoadAuditInfo_Add(int aProcessID, int aCurvesRead, int aCurvesWithErrors, int aCurvesCreated, int aCurvesModified, 
            int aCurvesRemoved, int aGroupsRead, int aGroupsWithErrors, int aGroupsCreated, int aGroupsModified, int aGroupsRemoved)
        {
            try
            {
                StoredProcedures.MID_SIZE_CURVE_LOAD_INFO_INSERT.Insert(_dba,
                                                                        PROCESS_RID: aProcessID,
                                                                        CURVES_READ: aCurvesRead,
                                                                        CURVES_WITH_ERRORS: aCurvesWithErrors,
                                                                        CURVES_CREATED: aCurvesCreated,
                                                                        CURVES_MODIFIED: aCurvesModified,
                                                                        CURVES_REMOVED: aCurvesRemoved,
                                                                        GROUPS_READ: aGroupsRead,
                                                                        GROUPS_WITH_ERRORS: aGroupsWithErrors,
                                                                        GROUPS_CREATED: aGroupsCreated,
                                                                        GROUPS_MODIFIED: aGroupsModified,
                                                                        GROUPS_REMOVED: aGroupsRemoved
                                                                        );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public void SizeCurveLoadAuditInfo_Delete(int Process_RID)
        {
            try
            {
                StoredProcedures.MID_SIZE_CURVE_LOAD_INFO_DELETE.Delete(_dba, PROCESS_RID: Process_RID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //End TT#1413-MD-stodd-Data Layer Request - SIZE_CURVE_LOAD_INFO
        //

	}
}
