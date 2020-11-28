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

namespace HeaderReconcile
{

    class HeaderReconcile
    {
        static string _inputDirectory;
        static string _outputDirectory;
        static string _connectionString;
        static string _triggerSuffix;
        static string _removeTransactionsFileName;
        static string _removeTransactionsTriggerSuffix;
        static string _headerTypes;
        static List<string> _headerTypeList;
        static string _headerKeysToMatch;
        static List<string> _headerKeysToMatchList;
        // Begin TT#1966-MD - JSmith - DC Fulfillment
        static List<string> _masterHeaderIdKeysList = new List<string>();
        // End TT#1966-MD - JSmith - DC Fulfillment
        static string _headerProcessingKeysFile;
        static int _taskListRid = Include.NoRID;
        static int _taskSeq = 0;
        static int _processId = Include.NoRID;
     

        static int Main(string[] args)
        {
            string sourceModule = "HeaderReconcile.cs";
            string eventLogID = "MIDHeaderReconcile";
            IMessageCallback messageCallback;
            SessionSponsor sponsor;
            SessionAddressBlock SAB = null;
            System.Runtime.Remoting.Channels.IChannel channel;
            eSecurityAuthenticate authentication;
            List<string> inFiles = new List<string>();
            HeaderReconcileProcess headerReconcileProc = null;
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

                    // Create Sessions: Client, Application, Hierarcgy, & Header.

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

                    // FOR TESTING PROCESS CONTROL
                    //string reason = "";
                    //SAB.ControlServerSession.GetProcessingPermission(eProcesses.headerLoad, 1010192, ref reason);
                    //SAB.ControlServerSession.SetProcessState(eProcesses.headerLoad, 1010192, false);

                    //===================================================================================
                    // PROCESSING PERMISSION: Are there conflicts with other running processes?
                    // USER AUTHENTICATION:
                    //===================================================================================
                    authentication = SAB.ClientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"],
                        MIDConfigurationManager.AppSettings["Password"], eProcesses.HeaderReconcile);
                    if (authentication == eSecurityAuthenticate.Unavailable)
                    {
                        errorFound = true;
                        return Convert.ToInt32(eMIDMessageLevel.Severe);    // TT#1663-MD - stodd - Header Load and Header Reconcile should return "Severe" if stopped from executing by process control rules
                    }
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
                    LoadAppSettings();

                    // ===============================================
                    // Retrieve command-line and configuration options
                    // ===============================================
                    LoadArguments(args, eventLogID, SAB);

                    SAB.ControlServerSession.Initialize();  // TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed

                    if (_processId != Include.NoRID)
                    {
                       SAB.ClientServerSession.Initialize(_processId);
                    }
                    else
                    {
                    	SAB.ClientServerSession.Initialize();
                    }

                    //============================================
                    // Check application settings for validity
                    //============================================
                    CheckAppSettings();

                    //=============================================================================================
                    // Loads the _headerKeysToMatch list using the data found in the _headerProcessingKeysFile
                    //=============================================================================================
                    // Begin TT#1966-MD - JSmith - DC Fulfillment
                    //List<string> nullList = null;
                    List<string> masterHeaderIdKeysList = new List<string>();
                    List<string> headerIdKeysList = new List<string>();
                    //if (!HeaderKeys.LoadKeys(_headerProcessingKeysFile, ref _headerKeysToMatchList, ref nullList, ref errorMessage))
                    //if (!HeaderKeys.LoadKeys(_headerProcessingKeysFile, ref _headerKeysToMatchList, ref nullList, ref _masterHeaderIdKeysList, ref errorMessage))
                    if (!SAB.ControlServerSession.LoadHeaderKeys(ref headerIdKeysList, ref masterHeaderIdKeysList, ref _headerKeysToMatchList, ref errorMessage))
                    // End TT#1966-MD - JSmith - DC Fulfillment
                    {
                        // Event Viewer
                        EventLog.WriteEntry(eventLogID, errorMessage, EventLogEntryType.Error);
                        System.Console.Write(errorMessage);
                        // Audit
                        SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, errorMessage, sourceModule);
                        SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
                        errorFound = true;
                        return Convert.ToInt32(eMIDMessageLevel.Severe);
                    }

                    try
                    {
                        string fileMsgText = MIDText.GetText(eMIDTextCode.msg_BatchInputFile);
                        // =============================
                        // Get Files in Input Directory
                        // =============================
                        inFiles = GetFiles();

                        //=======================
                        // Log processing parms
                        //=======================
                        LogProcessingParms(sourceModule, SAB);

                        //============================================
                        // Create processing class and initialize
                        //============================================
                        headerReconcileProc = new HeaderReconcileProcess(
                            SAB,
                            Thread.CurrentThread.GetHashCode(),
                            _triggerSuffix,
                            _headerKeysToMatchList,
                            _headerTypeList,
                            _outputDirectory,
                            _removeTransactionsFileName,
                            _removeTransactionsTriggerSuffix
                            );

                        //==================================
                        // PRE-Process transaction files
                        //   Looking for duplicates
                        //==================================
                        headerReconcileProc.PreProcessTransactions(inFiles);

                        //==================================
                        // Process transaction files
                        //==================================
                        foreach (string inFile in inFiles)
                        {
                            if (headerReconcileProc.ValidTransactionFile(inFile))
                            {
								// Begin TT#4693 - stodd - Completion Status is not correct based on Max Return setting in the Task List
                                eReturnCode rc = headerReconcileProc.ProcessTransactionFile(inFile);
                                if (rc != eReturnCode.successful)
                                {
                                    errorFound = true;
                                }
								// End TT#4693 - stodd - Completion Status is not correct based on Max Return setting in the Task List
                            }

                            //================================
                            // Removes input Trigger File
                            //================================
                            if (_triggerSuffix.Length > 0)
                            {
                                string triggerFileName = inFile + _triggerSuffix;
                                System.IO.File.Delete(triggerFileName);
                            }
                        }

                        //==================================
                        // POST-Process transaction files
                        //   Create "remove" transactions
                        //==================================
                        headerReconcileProc.PostProcessing();

                    }
                    catch (Exception exc)
                    {
                        SAB.ClientServerSession.Audit.Log_Exception(exc, sourceModule);
                        errorFound = true;  // TT#4693 - stodd - Completion Status is not correct based on Max Return setting in the Task List
                        throw;
                    }
                    finally
                    {
                        headerReconcileProc.Cleanup();
                    }

                    if (headerReconcileProc.TotalRejectedRecs == 0)
                    {
                        SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Processing completed successfully.", sourceModule);
                    }
                    else
                    {
                        SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, "Processing completed with Transaction errors.", sourceModule);
                    }

                    SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());

                    SAB.ClientServerSession.Audit.HeaderReconcileAuditInfo_Add(headerReconcileProc.TotalFilesProcessed
                                                                                    ,headerReconcileProc.TotalHeaderRecsRead
                                                                                    , headerReconcileProc.TotalHeaderRecsWritten
                                                                                    , headerReconcileProc.TotalFilesWritten
                                                                                    , headerReconcileProc.TotalDuplicateRecsFound
                                                                                    , headerReconcileProc.TotalSkippedRecs
                                                                                    , headerReconcileProc.TotalRemoveRecsWritten
                                                                                    , headerReconcileProc.TotalRemoveFilesWritten

                                                                                   );
                    //return Convert.ToInt32(SAB.CloseSessions(), CultureInfo.CurrentUICulture);
                }
                catch (Exception exc)
                {
                    errorFound = true;  // TT#4693 - stodd - Completion Status is not correct based on Max Return setting in the Task List
                    SAB.ClientServerSession.Audit.Log_Exception(exc, sourceModule);
                    SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Processing terminated due to errors.", sourceModule);

                    SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());

                    //return Convert.ToInt32(SAB.CloseSessions(), CultureInfo.CurrentUICulture);
                }
            }
            catch (Exception exc)
            {
                errorFound = true;  // TT#4693 - stodd - Completion Status is not correct based on Max Return setting in the Task List
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
                            // Begin TT#1663-MD - stodd - Header Load and Header Reconcile should return "Severe" if stopped from executing by process control rules
                            //SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                            SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                            // End TT#1663-MD - stodd - Header Load and Header Reconcile should return "Severe" if stopped from executing by process control rules
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
                        _inputDirectory = args[0];
                        if (args.Length > 1)
                        {
                            _triggerSuffix = "." + args[1].Trim('.', ' ');
                        }
                    }
                }
                else
                {
                    //_inputDirectory & _triggerSuffix are already filled in by app config
                }
            }
            catch
            {
                throw;
            }
        }

        private static string CheckAppSettings()
        {
            String errorMessage = string.Empty;

            if (_headerTypes == string.Empty)
            {
                errorMessage = MIDText.GetText(eMIDTextCode.msg_NoFieldSpecified);
                errorMessage =  errorMessage.Replace("{0}", "Header Type(s)");
                throw new Exception(errorMessage);
            }

            if (_inputDirectory == string.Empty)
            {
                errorMessage = MIDText.GetText(eMIDTextCode.msg_NoFieldSpecified);
                errorMessage = errorMessage.Replace("{0}", "Input Directory");
                throw new Exception(errorMessage);
            }

            if (_outputDirectory == string.Empty)
            {
                errorMessage = MIDText.GetText(eMIDTextCode.msg_NoFieldSpecified);
                errorMessage = errorMessage.Replace("{0}", "Output Directory");
                throw new Exception(errorMessage);
            }

            if (!Directory.Exists(_inputDirectory))
            {
                errorMessage = MIDText.GetText(eMIDTextCode.msg_InputDirectoryNotFound);
                errorMessage = errorMessage.Replace("{0}", _inputDirectory);
                throw new Exception(errorMessage);
            }

            if (!Directory.Exists(_outputDirectory))
            {
                errorMessage = MIDText.GetText(eMIDTextCode.msg_OutputDirectoryNotFound);
                errorMessage = errorMessage.Replace("{0}", _outputDirectory);
                throw new Exception(errorMessage);
            }

            if (_headerProcessingKeysFile == string.Empty)
            {
                errorMessage = MIDText.GetText(eMIDTextCode.msg_NoFieldSpecified);
                errorMessage = errorMessage.Replace("{0}", "Header Processing Keys File");
                throw new Exception(errorMessage);
            }

            if (!File.Exists(_headerProcessingKeysFile))
            {
                errorMessage = MIDText.GetText(eMIDTextCode.msg_NoHeaderProcessingKeyFile);
                errorMessage = errorMessage.Replace("{0}", _headerProcessingKeysFile);
                throw new Exception(errorMessage);
            }

            return errorMessage;
        }

        // Begin TT#1581-MD - stodd - Header Reconcile API
        private static void LogProcessingParms(string sourceModule, SessionAddressBlock SAB)
        {
            string headerKeys = string.Empty;
            _headerKeysToMatchList.ForEach(i => headerKeys += i + ", ");
            headerKeys = headerKeys.TrimEnd(new char[] { ',', ' ' });
            SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Header Types to Process: " + _headerTypes, sourceModule, true);
            SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Header Matching Keys: " + headerKeys, sourceModule, true);
            SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Input File Directory: " + _inputDirectory, sourceModule, true);
            SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Output File Directory: " + _outputDirectory, sourceModule, true);
            SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Input/Output File Trigger Suffix: " + _triggerSuffix, sourceModule, true);
            SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Remove Transactions File Name: " + _removeTransactionsFileName, sourceModule, true);
            SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Remove Transactions File Trigger Suffix: " + _removeTransactionsTriggerSuffix, sourceModule, true);
        }
        // End TT#1581-MD - stodd - Header Reconcile API

        static void LoadAppSettings()
        {
            string appSetting;

            try
            {
                appSetting = MIDConfigurationManager.AppSettings["InputDirectory"];

                if (appSetting == null)
                {
                    _inputDirectory = string.Empty;
                }
                else
                {
                    _inputDirectory = appSetting;
                }

                appSetting = MIDConfigurationManager.AppSettings["OutputDirectory"];

                if (appSetting == null)
                {
                    _outputDirectory = string.Empty;
                }
                else
                {
                    _outputDirectory = appSetting;
                }

                appSetting = MIDConfigurationManager.AppSettings["ConnectionString"];

                if (appSetting == null)
                {
                    _connectionString = string.Empty;
                }
                else
                {
                    _connectionString = appSetting;
                }

                appSetting = MIDConfigurationManager.AppSettings["TriggerSuffix"];

                if (appSetting == null ||
                    appSetting.Trim().Length == 0)
                {
                    _triggerSuffix = string.Empty;
                }
                else
                {
                    _triggerSuffix = "." + appSetting.Trim('.', ' ');
                }

                appSetting = MIDConfigurationManager.AppSettings["RemoveTransactionsFileName"];
                if (appSetting == null)
                {
                    _removeTransactionsFileName = "HeaderReconcileRemoveTransactions";
                }
                else
                {
                    _removeTransactionsFileName = appSetting.ToString().Trim();
                }

                appSetting = MIDConfigurationManager.AppSettings["RemoveTransactionsTriggerSuffix"];

                if (appSetting == null ||
                    appSetting.Trim().Length == 0)
                {
                    _removeTransactionsTriggerSuffix = string.Empty;
                }
                else
                {
                    _removeTransactionsTriggerSuffix = "." + appSetting.Trim('.', ' ');
                }

                appSetting = MIDConfigurationManager.AppSettings["HeaderTypes"];
                if (appSetting == null)
                {
                    _headerTypes = "All";
                    _headerTypeList.Add("ALL");

                }
                else
                {
                    _headerTypes = appSetting.ToString().Trim().ToUpper();
                    _headerTypeList = MIDstringTools.SplitGeneric(appSetting.ToString().Trim().ToUpper(), ',', true);
                }

                appSetting = MIDConfigurationManager.AppSettings["HeaderProcessingKeysFile"];
                if (appSetting == null)
                {
                    _headerProcessingKeysFile = string.Empty;
                }
                else
                {
                    _headerProcessingKeysFile = appSetting.ToString().Trim();
                }
            }
            catch
            {
                throw;
            }
        }

        static List<string> GetFiles()
        {
            List<string> transFiles = new List<string>();
            FileInfo[] dirFiles;

            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(_inputDirectory);

                if (!string.IsNullOrEmpty(_triggerSuffix))
                {
                    dirFiles = directoryInfo.GetFiles("*" + _triggerSuffix);
                }
                else
                {
                    dirFiles = directoryInfo.GetFiles();
                }

                eMIDMessageLevel messageLevel = eMIDMessageLevel.Warning;

                string dataFileName;
                string dataFileNameWithFilePath;
                foreach (FileInfo aFileInfo in dirFiles)
                {
                    string message;
                    dataFileName = Path.GetFileNameWithoutExtension(aFileInfo.FullName);
                    if (File.Exists(aFileInfo.DirectoryName + @"\" + dataFileName) || _triggerSuffix == string.Empty)
                    {
                        dataFileNameWithFilePath = aFileInfo.FullName.Substring(0, aFileInfo.FullName.Length - (_triggerSuffix.Length));
                        transFiles.Add(dataFileNameWithFilePath);
                    }
                    else
                    {
                        string[] errParms = new string[1];
                        errParms.SetValue(aFileInfo.ToString(), 0);
                        message = "File Name=" + aFileInfo.FullName;
                        //sab.ClientServerSession.Audit.Add_Msg(messageLevel, eMIDTextCode.msg_InvalidReclassTriggerFileName, message, GetType().Name);
                    }
                }
            }
            catch
            {
                throw;
            }

            return transFiles;
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

                DataRow taskRow = scheduleData.TaskHeaderReconcile_Read(_taskListRid, _taskSeq);

                _inputDirectory = taskRow["INPUT_DIRECTORY"].ToString();
                _outputDirectory = taskRow["OUTPUT_DIRECTORY"].ToString();

                if (taskRow["TRIGGER_SUFFIX"] == DBNull.Value)
                {
                    _triggerSuffix = string.Empty;
                }
                else
                {
                    _triggerSuffix = "." + taskRow["TRIGGER_SUFFIX"].ToString().Trim('.', ' ');
                }

                _removeTransactionsFileName = taskRow["REMOVE_TRANS_FILE_NAME"].ToString();

                if (taskRow["REMOVE_TRANS_TRIGGER_SUFFIX"] == DBNull.Value)
                {
                    _removeTransactionsTriggerSuffix = string.Empty;    // TT#1626-MD - stodd - Header Reconcile Processing Incorrectly
                }
                else
                {
                    _removeTransactionsTriggerSuffix = "." + taskRow["REMOVE_TRANS_TRIGGER_SUFFIX"].ToString().Trim('.', ' ');  // TT#1626-MD - stodd - Header Reconcile Processing Incorrectly
                }

                _headerTypes = taskRow["HEADER_TYPES"].ToString();
                _headerTypeList = MIDstringTools.SplitGeneric(_headerTypes.Trim().ToUpper(), ',', true);  // TT#4972 - JSmith - Header Reconcile Generating Updates Files for Headers That Do Not Need Updating
                _headerProcessingKeysFile = taskRow["HEADER_KEYS_FILE_NAME"].ToString();
            }
            catch
            {
                throw;
            }
        }
    }
}
