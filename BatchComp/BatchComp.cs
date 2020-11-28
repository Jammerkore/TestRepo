using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;   

namespace BatchComp
{
    class BatchComp
    {
        static string _computation;
        //static string _connectionString;
        static int _taskListRid = Include.NoRID;
        static int _taskSeq = 0;
        static int _processId = Include.NoRID;

        static int Main(string[] args)
        {
            string sourceModule = "BatchComp.cs";
            string eventLogID = "MIDBatchComp";
            IMessageCallback messageCallback;
            SessionSponsor sponsor;
            SessionAddressBlock SAB = null;
            System.Runtime.Remoting.Channels.IChannel channel;
            eSecurityAuthenticate authentication;
            List<string> inFiles = new List<string>();
            CustomBusinessRoutines customBusinessRoutines = null;
            Hashtable processedFiles = new Hashtable();
            bool errorFound = false;
            eMIDMessageLevel highestMessageLevel = eMIDMessageLevel.None;
            Exception innerE;

            try
            {
                messageCallback = new BatchMessageCallback();
                sponsor = new SessionSponsor();
                authentication = eSecurityAuthenticate.UnknownUser;
                SAB = new SessionAddressBlock(messageCallback, sponsor);
                String errorMessage = string.Empty;

                try
                {
                    if (!EventLog.SourceExists(eventLogID))
                    {
                        EventLog.CreateEventSource(eventLogID, null);
                    }

                    // Register callback channel

                    try
                    {
                        channel = SAB.OpenCallbackChannel();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error opening port #0 " + ex.Message);
                    }

                    // Create Sessions: Client, Application, Hierarchy, & Header.

                    try
                    {
                        SAB.CreateSessions((int)eServerType.Client | (int)eServerType.Application | (int)eServerType.Hierarchy | (int)eServerType.Header);
                    }
                    catch (Exception ex)
                    {
                        innerE = ex;

                        while (innerE.InnerException != null)
                        {
                            innerE = innerE.InnerException;
                        }

                        throw new Exception("Error creating sessions - " + innerE.Message);
                    }


                    //===================================================================================
                    // PROCESSING PERMISSION: Are there conflicts with other running processes?
                    // USER AUTHENTICATION:
                    //===================================================================================
                    authentication = SAB.ClientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"],
                        MIDConfigurationManager.AppSettings["Password"], eProcesses.BatchComp);
                    //if (authentication == eSecurityAuthenticate.Unavailable)
                    //{
                    //    errorFound = true;
                    //    return Convert.ToInt32(eMIDMessageLevel.ProcessUnavailable);
                    //}

                    //BEGIN TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID
                    //BEGIN TT#1644 - MD- DOConnell - Process Control
                    if (authentication == eSecurityAuthenticate.Unavailable)
                    {
                        //return Convert.ToInt32(eMIDMessageLevel.ProcessUnavailable);
                        errorFound = true; //TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                        return Convert.ToInt32(eMIDMessageLevel.Severe);
                    }
                    //END TT#1644 - MD- DOConnell - Process Control
                    //END TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID

                    if (authentication != eSecurityAuthenticate.UserAuthenticated)
                    {
                        errorFound = true;
                        EventLog.WriteEntry(eventLogID, "Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"], EventLogEntryType.Error);
                        System.Console.Write("Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"]);
                        return Convert.ToInt32(eMIDMessageLevel.Severe);
                    }

                    //===================================================================
                    // Load application settings and override with command line arguments
                    //===================================================================
                    //LoadAppSettings();

                    // ===============================================
                    // Retrieve command-line and configuration options
                    // ===============================================
                    LoadArguments(args, eventLogID, SAB);

                    if (_processId != Include.NoRID)
                    {
                        SAB.ClientServerSession.Initialize(_processId);
                    }
                    else
                    {
                        SAB.ClientServerSession.Initialize();
                    }

                    SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Batch Comp Args: " + _computation, "BatchComp", true);

                    //============================================
                    // Check application settings for validity
                    //============================================
                    //CheckAppSettings();

                    eReturnCode rc;
                    customBusinessRoutines = new CustomBusinessRoutines(SAB, null);
                    try
                    {
                        rc = customBusinessRoutines.ProcessBatchComp(_computation);
                    }
                    catch (Exception exc)
                    {
                        SAB.ClientServerSession.Audit.Log_Exception(exc, sourceModule);
                        throw;
                    }
                    finally
                    {
                    }

                    if (rc == eReturnCode.successful)
                    {
                        SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Processing completed successfully.", sourceModule);
                    }
                    else
                    {
                        SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Processing completed with errors.", sourceModule);
                    }

                    SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());

                    //SAB.ClientServerSession.Audit.HeaderReconcileAuditInfo_Add(headerReconcileProc.TotalFilesProcessed
                    //                                                                , headerReconcileProc.TotalHeaderRecsRead
                    //                                                                , headerReconcileProc.TotalHeaderRecsWritten
                    //                                                                , headerReconcileProc.TotalFilesWritten
                    //                                                                , headerReconcileProc.TotalDuplicateRecsFound
                    //                                                                , headerReconcileProc.TotalSkippedRecs
                    //                                                                , headerReconcileProc.TotalRemoveRecsWritten
                    //                                                                , headerReconcileProc.TotalRemoveFilesWritten

                    //                                                               );
                }
                catch (Exception exc)
                {
                    SAB.ClientServerSession.Audit.Log_Exception(exc, sourceModule);
                    SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Processing terminated due to errors.", sourceModule);

                    SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
                }
            }
            catch (Exception exc)
            {
                EventLog.WriteEntry(eventLogID, "Exception encountered: " + exc.Message, EventLogEntryType.Error);
                highestMessageLevel = eMIDMessageLevel.Severe;
            }
            finally
            {
                if (!errorFound)
                {
                    if (SAB.ClientServerSession != null && SAB.ClientServerSession.Audit != null)
                    {
                        try
                        {
                            SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
                        }
                        catch
                        {
                            SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", eMIDMessageLevel.Severe);
                        }
                    }
                }
                else
                {
                    if (SAB.ClientServerSession != null && SAB.ClientServerSession.Audit != null)
                    {
                        try
                        {
                            SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                        }
                        catch
                        {
                            SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", eMIDMessageLevel.Severe);
                        }
                    }
                }

                highestMessageLevel = SAB.CloseSessions();
            }

            return (int)highestMessageLevel;
        }

        private static void LoadArguments(string[] args, string eventLogID, SessionAddressBlock sab)
        {
            try
            {
                string argsList = string.Empty;
                foreach (string arg in args)
                {
                    argsList += arg + " ";
                }
                EventLog.WriteEntry(eventLogID, "args: " + argsList, EventLogEntryType.Information);

                if (args.Length > 0)
                {
                    // From Task List
                    if (args[0] == Include.SchedulerID)
                    {
                        _taskListRid = Convert.ToInt32(args[1]);
                        _taskSeq = Convert.ToInt32(args[2]);
                        _processId = Convert.ToInt32(args[3]);
                        LoadFromTasklist(eventLogID, sab);
                    }
                    else
                    // From Command Line
                    {
                        _computation = args[0];
                    }
                }
                else
                {
                    _computation = string.Empty;
                }
            }
            catch
            {
                throw;
            }
        }

        static void LoadFromTasklist(string eventLogID, SessionAddressBlock sab)
        {
            try
            {
                ScheduleData scheduleData = new ScheduleData();

                int userRid = Include.UndefinedUserRID;
                string tasklistName = string.Empty;

                if (_taskListRid != Include.NoRID)
                {
                    DataRow taskListRow = scheduleData.TaskList_Read(_taskListRid);
                    if (taskListRow != null)
                    {
                        userRid = Convert.ToInt32(taskListRow["USER_RID"], CultureInfo.CurrentUICulture);
                        tasklistName = taskListRow["TASKLIST_NAME"].ToString();
                    }
                    else
                    {
                        EventLog.WriteEntry(eventLogID, "Invalid tasklist RID:" + _taskListRid.ToString(), EventLogEntryType.Error);
                        System.Console.Write("Invalid tasklist RID:" + _taskListRid.ToString());
                        throw new Exception("Invalid tasklist RID:" + _taskListRid.ToString());
                    }
                }

                DataRow taskRow = scheduleData.TaskBatchComp_Read(_taskListRid, _taskSeq);

                int batchCompRid = int.Parse(taskRow["BATCH_COMP_RID"].ToString());
                _computation = MIDText.GetTextOnly(batchCompRid);
            }
            catch (Exception exp)
            {
                EventLog.WriteEntry(eventLogID, "LoadFromTasklist: " + exp.ToString(), EventLogEntryType.Error);
                throw;
            }
        }


    }
}
