// Begin TT#827 - JSmith - Allocation Performance
//
// Too many changes to mark. Compare for differences
// Also removed old commend code for readability
//
// End TT#827 - JSmith - Allocation Performance
using System;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Globalization;
using System.Data;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.AllocationScheduler
{
    /// <summary>
    /// Summary description for PlanForecasting.
    /// </summary>
    class AllocationScheduler
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <remarks>
        /// First Arg = TaskList_RID
        /// Second Arg (optional) = Task_Seq
        /// </remarks>
        [STAThread]
        static int Main(string[] args)
        {
            string sourceModule = "AllocationScheduler.cs";
            string eventLogID = "AllocationScheduler";
            SessionAddressBlock SAB;
            SessionSponsor sponsor;
            IMessageCallback messageCallback;
            DateTime start = DateTime.Now;
            eMIDMessageLevel returnLevel = eMIDMessageLevel.None;
            string message = null;
            messageCallback = new BatchMessageCallback();
            sponsor = new SessionSponsor();
            SAB = new SessionAddressBlock(messageCallback, sponsor);
            System.Runtime.Remoting.Channels.IChannel channel;
            int _taskListRID = Include.NoRID;
            int _taskSeq = 0;
            int _processId = Include.NoRID;
            ScheduleData _scheduleData;
            bool errorFound = false;

            try
            {
                if (!EventLog.SourceExists(eventLogID))
                {
                    EventLog.CreateEventSource(eventLogID, null);
                }

                //==========================
                // Register callback channel
                //==========================
                try
                {
                    channel = SAB.OpenCallbackChannel();
                }
                catch (Exception e)
                {
                    EventLog.WriteEntry(eventLogID, "Error opening port #0 - " + e.Message, EventLogEntryType.Error);
                    throw;
                }

                //===========================
                // edit for arguments
                //===========================
                if (args.Length > 0)
                {
                    if (args[0] == Include.SchedulerID)
                    {
                        _taskListRID = Convert.ToInt32(args[1]);
                        _taskSeq = Convert.ToInt32(args[2]);
                        _processId = Convert.ToInt32(args[3]);
                    }
                    else
                    {
                        _taskListRID = Convert.ToInt32(args[0]);
                        if (args.Length > 1)
                        {
                            _taskSeq = Convert.ToInt32(args[1]);
                        }
                        else
                        {
                            EventLog.WriteEntry(eventLogID, "Missing Argument: Task Sequence", EventLogEntryType.Error);
                            returnLevel = eMIDMessageLevel.Error;
                        }
                    }
                }
                else
                {
                    EventLog.WriteEntry(eventLogID, "Missing Argument: Task List RID", EventLogEntryType.Error);
                    returnLevel = eMIDMessageLevel.Error;
                }

                if (returnLevel == eMIDMessageLevel.None)
                {
                    //==================
                    // Create Sessions
                    //==================
                    try
                    {
                        SAB.CreateSessions((int)eServerType.Client | (int)eServerType.Application | (int)eServerType.Store | (int)eServerType.Hierarchy | (int)eServerType.Header);
                    }
                    catch (Exception ex)
                    {
                        errorFound = true;
                        Exception innerE = ex;
                        while (innerE.InnerException != null)
                        {
                            innerE = innerE.InnerException;
                        }
                        EventLog.WriteEntry(eventLogID, "Error creating sessions - " + innerE.Message, EventLogEntryType.Error);
                        return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
                    }

                    //=================================
                    // get user rid from tasklist table
                    //=================================
                    _scheduleData = new ScheduleData();
                    DataRow taskListRow = _scheduleData.TaskList_Read(_taskListRID);
                    int userRid = Include.UndefinedUserRID;
                    if (taskListRow != null)
                    {
                        userRid = Convert.ToInt32(taskListRow["USER_RID"], CultureInfo.CurrentUICulture);

                        eSecurityAuthenticate authentication =
                            SAB.ClientServerSession.UserLogin(userRid, eProcesses.allocate);

                        //BEGIN TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID
                        //BEGIN TT#1644-VSuart-Process Control-MID
                        if (authentication == eSecurityAuthenticate.Unavailable)
                        {
                            //return Convert.ToInt32(eMIDMessageLevel.ProcessUnavailable);
                            errorFound = true; //TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                            return Convert.ToInt32(eMIDMessageLevel.Severe);
                        }
                        //END TT#1644-VSuart-Process Control-MID
                        //END TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID

                        if (authentication != eSecurityAuthenticate.ActiveUser)
                        {
                            EventLog.WriteEntry(eventLogID, "Unable to log in with user RID:" + userRid.ToString(), EventLogEntryType.Error);
                            System.Console.Write("Unable to log in with user RID:" + userRid.ToString());
                            errorFound = true;
                            return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
                        }

                        if (_processId != Include.NoRID)
                        {
                            SAB.ClientServerSession.Initialize(_processId);
                        }
                        else
                        {
                            SAB.ClientServerSession.Initialize();
                        }

                        SAB.ApplicationServerSession.Initialize();
                        SAB.StoreServerSession.Initialize();
                        SAB.HierarchyServerSession.Initialize();
                        SAB.HeaderServerSession.Initialize();

                        //Begin TT#1313-MD -jsobek -Header Filters
                        //DataTable headerDataTable = SAB.HeaderServerSession.GetHeadersForTaskList(_taskListRID, _taskSeq);
                        Header headerData = new Header();
                        //End TT#1313-MD -jsobek -Header Filters

                        //=========================================
                        // get the allocate tasks for this request
                        //=========================================

                        DataTable dtTaskAllocate = _scheduleData.TaskAllocate_ReadByTaskList(_taskListRID, _taskSeq);
                        int rowCount = dtTaskAllocate.Rows.Count;
                        int r = 0;
                        int allocateSeq = 0, workFlowRID = 0;
                        bool dateInRange = false;
                        //================================================
                        // gather up all the allocate items for this Task List
                        //================================================
                        for (r = 0; r < rowCount; r++)
                        {
                            DataRow aRow = dtTaskAllocate.Rows[r];
                            allocateSeq = Convert.ToInt32(aRow["ALLOCATE_SEQUENCE"], CultureInfo.CurrentUICulture);
                            //Begin TT#1313-MD -jsobek -Header Filters

                            //Default to the current AWS filter if no filter was supplied
                            int allocateSeqFilterRID;
                            if (aRow["FILTER_RID"] != DBNull.Value && Convert.ToInt32(aRow["FILTER_RID"], CultureInfo.CurrentUICulture) != Include.NoRID)
                            {
                                allocateSeqFilterRID = Convert.ToInt32(aRow["FILTER_RID"], CultureInfo.CurrentUICulture);
                            }
                            else
                            {
                                allocateSeqFilterRID = SAB.AllocationWorkspaceCurrentHeaderFilter;
                            }

                            int merchandiseOverrideHnRID = Include.NoRID;
                            if (aRow["HN_RID"] != DBNull.Value)
                            {
                                merchandiseOverrideHnRID = Convert.ToInt32(aRow["HN_RID"], CultureInfo.CurrentUICulture);
                            }

                            //End TT#1313-MD -jsobek -Header Filters
                            DataTable dtTaskAllocateDetail = _scheduleData.TaskAllocateDetail_ReadByTaskList(_taskListRID, _taskSeq, allocateSeq);
                            int DetailRowCount = dtTaskAllocateDetail.Rows.Count;
                            int r2 = 0;
                            //=================================================================================
                            // For each allocation detail, check to see if current date is within date range.
                            // if it is, run the forecast.
                            //=================================================================================
                            for (r2 = 0; r2 < DetailRowCount; r2++)
                            {
                                DataRow aDetailRow = dtTaskAllocateDetail.Rows[r2];
                                int detailSeq = Convert.ToInt32(aDetailRow["DETAIL_SEQUENCE"], CultureInfo.CurrentUICulture);
                                workFlowRID = Convert.ToInt32(aDetailRow["WORKFLOW_RID"], CultureInfo.CurrentUICulture);
                                if (aDetailRow["EXECUTE_CDR_RID"] == System.DBNull.Value)
                                    dateInRange = true;
                                else
                                {
                                    int cdrRID = Convert.ToInt32(aDetailRow["EXECUTE_CDR_RID"], CultureInfo.CurrentUICulture);
                                    dateInRange = SAB.ApplicationServerSession.Calendar.IsCurrentDateWithinRange(cdrRID);
                                }

                                if (dateInRange)
                                {
                                    AllocationHeaderProfileList headerList = new AllocationHeaderProfileList(eProfileType.AllocationHeader);
                                    AllocationHeaderProfile headerProfile;
                                    //Begin TT#1313-MD -jsobek -Header Filters

                                    //Get the headers using the filter for just this task sequence and just this allocate sequence - not combining SQL calls for multiple filters to keep the design simpler

                                    //DataRow[] drh = headerDataTable.Select("ALLOCATE_SEQUENCE=" + allocateSeq);
                                    FilterHeaderOptions headerFilterOptions = new FilterHeaderOptions();
                                    headerFilterOptions.USE_WORKSPACE_FIELDS = false;
                                    headerFilterOptions.filterType = filterTypes.HeaderFilter;
                                    headerFilterOptions.HN_RID_OVERRIDE = merchandiseOverrideHnRID;
                                    DataTable dtHeaders = headerData.GetHeadersFromFilter(allocateSeqFilterRID, headerFilterOptions);
                                    //End TT#1313-MD -jsobek -Header Filters
                                    foreach (DataRow dr in dtHeaders.Rows)
                                    {
                                        headerProfile = new AllocationHeaderProfile(
                                                                    Convert.ToString(dr["HDR_ID"], CultureInfo.CurrentUICulture),
                                                                    Convert.ToInt32(dr["HDR_RID"], CultureInfo.CurrentUICulture));
                                        headerList.Add(headerProfile);
                                    }
                                    if (headerList.Count > 0)
                                    {
                                        bool enqueueFailure = false;
                                        bool headerProcessed = false;  // TT#2013-MD - JSmith - Process Allocate Task List - the Header attached to an Asst receives a Null Reference Exception
                                        foreach (AllocationHeaderProfile ahp in headerList)
                                        {
                                            AllocationHeaderProfileList ahpl = new AllocationHeaderProfileList(eProfileType.AllocationHeader);
                                            ahpl.Add(ahp);
                                            ApplicationSessionTransaction _transaction = SAB.ApplicationServerSession.CreateTransaction();
                                            try
                                            {
                                                string msgText;
                                                if (_transaction.EnqueueHeaders(ahpl, out msgText))
                                                {
                                                    // Begin TT#2013-MD - JSmith - Process Allocate Task List - the Header attached to an Asst receives a Null Reference Exception
                                                    //_transaction.ProcessAllocationWorkflow(workFlowRID, true, true, 1, eWorkflowProcessOrder.AllStepsForHeader, false);
                                                    if (_transaction.AddAllocationProfile(aAllocationHeaderList: ahpl, bIncludeAssortmentAndGroupAllocation: false, bIncludeMultiHeaders: false, bIncludeReleaseHeaders: false))
                                                    {
                                                        _transaction.ProcessAllocationWorkflow(workFlowRID, true, true, 1, eWorkflowProcessOrder.AllStepsForHeader, false);
                                                        headerProcessed = true;
                                                    }
                                                    //else
                                                    //{
                                                    //    string errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_ScheduleHasNoHeadersToProcess), _taskListRID, _taskSeq);
                                                    //    SAB.ClientServerSession.Audit.Add_Msg(
                                                    //        eMIDMessageLevel.Warning,
                                                    //        eMIDTextCode.msg_al_ScheduleHasNoHeadersToProcess,
                                                    //        errorMessage,
                                                    //        sourceModule);
                                                    //    returnLevel = eMIDMessageLevel.Warning;
                                                    //}
                                                    // End TT#2013-MD - JSmith - Process Allocate Task List - the Header attached to an Asst receives a Null Reference Exception
                                                }
                                                else
                                                {
                                                    SAB.ClientServerSession.Audit.Add_Msg(
                                                        eMIDMessageLevel.Information,
                                                        eMIDTextCode.msg_al_HeaderEnqFailed,
                                                        MIDText.GetText(eMIDTextCode.msg_al_HeaderEnqFailed),
                                                        sourceModule);
                                                    SAB.ClientServerSession.Audit.Add_Msg(
                                                        eMIDMessageLevel.Error,
                                                        eMIDTextCode.msg_al_HeadersInUse,
                                                        msgText,
                                                        sourceModule);
                                                    msgText =
                                                            MIDText.GetText(eMIDTextCode.msg_al_HeaderUpdtFailedDueToEnqIssues)
                                                            + " [" + ahp.HeaderID + "]";
                                                    SAB.ClientServerSession.Audit.Add_Msg(
                                                        eMIDMessageLevel.Error,
                                                        eMIDTextCode.msg_al_HeaderUpdtFailedDueToEnqIssues,
                                                        msgText,
                                                        sourceModule);

                                                    enqueueFailure = true;
                                                }
                                            }
                                            finally
                                            {
                                                if (_transaction != null)
                                                {
                                                    _transaction.DequeueHeaders();
                                                    _transaction.Dispose();
                                                }
                                            }
                                        }

                                        // Begin TT#2013-MD - JSmith - Process Allocate Task List - the Header attached to an Asst receives a Null Reference Exception
                                        if (!headerProcessed)
                                        {
                                            string errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_ScheduleHasNoHeadersToProcess), _taskListRID, _taskSeq);
                                            SAB.ClientServerSession.Audit.Add_Msg(
                                                eMIDMessageLevel.Warning,
                                                eMIDTextCode.msg_al_ScheduleHasNoHeadersToProcess,
                                                errorMessage,
                                                sourceModule);
                                            returnLevel = eMIDMessageLevel.Warning;
                                        }
                                        // End TT#2013-MD - JSmith - Process Allocate Task List - the Header attached to an Asst receives a Null Reference Exception

                                        if (enqueueFailure)
                                        {
                                            string errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_ScheduleEnqueueFailure), _taskListRID, _taskSeq);
                                            SAB.ClientServerSession.Audit.Add_Msg(
                                                eMIDMessageLevel.Error,
                                                eMIDTextCode.msg_al_ScheduleEnqueueFailure,
                                                errorMessage,
                                                sourceModule);
                                        }
                                    }
                                    else
                                    {
                                        string errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_ScheduleHasNoHeadersToProcess), _taskListRID, _taskSeq);
                                        SAB.ClientServerSession.Audit.Add_Msg(
                                            eMIDMessageLevel.Warning,
                                            eMIDTextCode.msg_al_ScheduleHasNoHeadersToProcess,
                                            errorMessage,
                                            sourceModule);
                                        returnLevel = eMIDMessageLevel.Warning;
                                    }
                                }
                                else
                                {
                                    SAB.ClientServerSession.Audit.Add_Msg(
                                        eMIDMessageLevel.Information,
                                        eMIDTextCode.msg_al_CurrentDateOutsideRange,
                                        MIDText.GetText(eMIDTextCode.msg_al_CurrentDateOutsideRange),
                                        sourceModule, true);
                                }
                            }
                        }
                    }
                    else
                    {
                        string errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_AllocationTaskListNotFound), _taskListRID);
                        SAB.ClientServerSession.Audit.Add_Msg(
                                        eMIDMessageLevel.Error,
                                        eMIDTextCode.msg_AllocationTaskListNotFound,
                                        MIDText.GetText(eMIDTextCode.msg_AllocationTaskListNotFound),
                                        sourceModule, true);
                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_AllocationTaskListNotFound,
                            errorMessage);
                    }
                }
            }
            catch (Exception err)
            {
                message = err.Message;
                if (SAB.ClientServerSession != null)
                {
                    SAB.ClientServerSession.Audit.Log_Exception(err, sourceModule, eExceptionLogging.logAllInnerExceptions);
                    SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, err.Message, sourceModule);
                    SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                }
                errorFound = true;
                returnLevel = eMIDMessageLevel.Error;
            }
            finally
            {
                if (SAB.ClientServerSession != null)
                {
                    if (!errorFound)
                    {
                        if (SAB.ClientServerSession.Audit != null)
                        {
                            SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
                        }
                    }
                    //BEGIN TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                    else
                    {
                        if (SAB.ClientServerSession != null && SAB.ClientServerSession.Audit != null)
                        {
                            SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                        }
                    }
                    //END TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                }

                returnLevel = SAB.CloseSessions();
            }

            return (int)returnLevel;
        }
    }
}
