using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Globalization;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Xml;
using System.Xml.Serialization;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.DailyPercentagesCriteriaLoad
{
    /// <summary>
    /// Summary description for DailyPercentagesCriteriaLoad.
    /// </summary>
    class DailyPercentagesCriteriaLoad
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            char[] delimiter = { '~' };

            bool errorFound = false;
            string userId = null;
            string passWd = null;
            string msgText = null;
            string sveMsgText = null;
            string fileLocation = null;
            string optDelimiter = null;
            string eventLogID = "MIDDailyPercentagesCriteriaLoad";
            string sourceModule = "DailyPercentagesCriteriaLoad.cs";
            eMIDMessageLevel highestMessage;
            Exception innerE;
            ArrayList files = new ArrayList();
            Hashtable processedFiles = new Hashtable();
            int _processId = Include.NoRID;

            DailyPercentagesCriteriaLoadProcess dpclp;
            SessionSponsor sponsor;
            IMessageCallback messageCallback;
            SessionAddressBlock SAB;
            System.Runtime.Remoting.Channels.IChannel channel;
            eSecurityAuthenticate authentication;

            if (!EventLog.SourceExists(eventLogID))
            {
                EventLog.CreateEventSource(eventLogID, null);
            }

            try
            {
                sponsor = new SessionSponsor();
                messageCallback = new BatchMessageCallback();
                SAB = new SessionAddressBlock(messageCallback, sponsor);

                try
                {
                    // =========================
                    // Register callback channel
                    // =========================

                    try
                    {
                        channel = SAB.OpenCallbackChannel();
                    }
                    catch (Exception Ex)
                    {
                        EventLog.WriteEntry(eventLogID, "Error opening port #0 - " + Ex.ToString(), EventLogEntryType.Error);
                        return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
                    }

                    // ===============
                    // Create Sessions
                    // ===============

                    try
                    {
                        SAB.CreateSessions((int)eServerType.Client | (int)eServerType.Hierarchy | (int)eServerType.Store | (int)eServerType.Application);
                    }
                    catch (Exception Ex)
                    {
                        errorFound = true;
                        innerE = Ex;

                        while (innerE.InnerException != null)
                        {
                            innerE = innerE.InnerException;
                        }

                        EventLog.WriteEntry(eventLogID, "Error creating sessions - " + innerE.ToString(), EventLogEntryType.Error);
                        return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
                    }

                    // =====
                    // Login
                    // =====

                    userId = MIDConfigurationManager.AppSettings["User"];
                    passWd = MIDConfigurationManager.AppSettings["Password"];

                    if ((userId == "" || userId == null) &&
                        (passWd == "" || passWd == null))
                    {
                        EventLog.WriteEntry(eventLogID, "User and Password NOT specified", EventLogEntryType.Error);
                        System.Console.Write("User and Password NOT specified");
                        return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
                    }

                    authentication = SAB.ClientServerSession.UserLogin(userId, passWd, eProcesses.DailyPercentagesCriteraLoad);

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
                        EventLog.WriteEntry(eventLogID, "Unable to log in with user: [" + userId + "] password: [" + passWd + "]", EventLogEntryType.Error);
                        System.Console.Write("Unable to log in with user: [" + userId + "] password: [" + passWd + "]");
                        return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
                    }

                    // ========================
                    // Process input parameters
                    // ========================

                    if (args.Length > 0)
                    {
                        if (args[0] == Include.SchedulerID)
                        {
                            fileLocation = args[1];
                            _processId = Convert.ToInt32(args[2]);
                            optDelimiter = MIDConfigurationManager.AppSettings["Delimiter"];

                            if (optDelimiter != null)
                            {
                                delimiter = optDelimiter.ToCharArray();
                            }
                        }
                        else
                        {
                            if (args[0].Length > 0)
                            {
                                fileLocation = args[0];
                            }
                            else
                            {
                                fileLocation = MIDConfigurationManager.AppSettings["InputFile"];
                            }

                            if (args.Length > 1)
                            {
                                delimiter = args[1].ToCharArray();
                            }
                            else
                            {
                                optDelimiter = MIDConfigurationManager.AppSettings["Delimiter"];

                                if (optDelimiter != null)
                                {
                                    delimiter = optDelimiter.ToCharArray();
                                }
                            }
                        }
                    }
                    else
                    {
                        fileLocation = MIDConfigurationManager.AppSettings["InputFile"];
                        optDelimiter = MIDConfigurationManager.AppSettings["Delimiter"];

                        if (optDelimiter != null)
                        {
                            delimiter = optDelimiter.ToCharArray();
                        }
                    }

                    // =================================================
                    // Initialize client session to make audit available
                    // =================================================

                    if (_processId != Include.NoRID)
                    {
                        SAB.ClientServerSession.Initialize(_processId);
                        //SAB.HierarchyServerSession.Initialize();  // TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
                        SAB.StoreServerSession.Initialize();
                        // Begin TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
                        // StoreServerSession must be initialized before HierarchyServerSession 
                        SAB.HierarchyServerSession.Initialize();
                        // End TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
                        SAB.ApplicationServerSession.Initialize();
                    }
                    else
                    {
                        SAB.ClientServerSession.Initialize();
                        //SAB.HierarchyServerSession.Initialize();  // TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
                        SAB.StoreServerSession.Initialize();
                        // Begin TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
                        // StoreServerSession must be initialized before HierarchyServerSession 
                        SAB.HierarchyServerSession.Initialize();
                        // End TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
                        SAB.ApplicationServerSession.Initialize();
                    }

                    // ===========================
                    // Does transaction file exist
                    // ===========================

                    sveMsgText = MIDText.GetText(eMIDTextCode.msg_BatchInputFile);

                    if (fileLocation == null || fileLocation == "")
                    {
                        msgText = sveMsgText.Replace("{0}.", "[" + fileLocation + "] NOT specified" + System.Environment.NewLine);
                        msgText += "Header Load Process NOT run";

                        SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msgText, sourceModule, true);
                        SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_InputFileNotFound, "", SAB.GetHighestAuditMessageLevel());
                        return Convert.ToInt32(SAB.GetHighestAuditMessageLevel(), CultureInfo.CurrentUICulture);
                    }
                    else
                    {
                        if (!File.Exists(fileLocation))
                        {
                            msgText = sveMsgText.Replace("{0}.", "[" + fileLocation + "] does NOT exist" + System.Environment.NewLine);
                            msgText += "Daily Percentages Load Process NOT run";

                            SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msgText, sourceModule, true);

                            try
                            {
                                SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_InputFileNotFound, "", SAB.GetHighestAuditMessageLevel());
                            }
                            catch
                            {
                                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Update process header failed", sourceModule);
                                SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", eMIDMessageLevel.Severe);
                            }
                            highestMessage = eMIDMessageLevel.None;
                            try
                            {
                                highestMessage = SAB.GetHighestAuditMessageLevel();
                            }
                            catch
                            {
                                highestMessage = eMIDMessageLevel.Severe;
                            }

                            return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
                        }
                        else
                        {
                            FileInfo txnFileInfo = new FileInfo(fileLocation);

                            if (txnFileInfo.Length == 0)
                            {
                                msgText = sveMsgText.Replace("{0}.", "[" + fileLocation + "] is an empty file" + System.Environment.NewLine);
                                msgText += "Daily Percentages Load Process NOT run";

                                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, msgText, sourceModule, true);

                                try
                                {
                                    SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_InputFileNotFound, "", SAB.GetHighestAuditMessageLevel());
                                }
                                catch
                                {
                                    SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Update process header failed", sourceModule);
                                    SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", eMIDMessageLevel.Severe);
                                }
                                highestMessage = eMIDMessageLevel.None;
                                try
                                {
                                    highestMessage = SAB.GetHighestAuditMessageLevel();
                                }
                                catch
                                {
                                    highestMessage = eMIDMessageLevel.Severe;
                                }

                                return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
                            }
                            else
                            {
                                msgText = sveMsgText.Replace("{0}", "[" + fileLocation + "]");

                                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, sourceModule, true);

                                // ===================
                                // Initialize sessions
                                // ===================

                                dpclp = new DailyPercentagesCriteriaLoadProcess(SAB, ref errorFound);
                                if (fileLocation.Substring(fileLocation.Length - 4).ToUpper() == ".XML")
                                {
                                    dpclp.ProcessXMLTransFile(fileLocation, delimiter, ref errorFound);
                                }
                                else
                                {
                                    dpclp.ProcessVariableFile(fileLocation, delimiter, ref errorFound);
                                }
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    errorFound = true;
                    SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, Ex.ToString(), sourceModule);
                }
                finally
                {
                    if (SAB.ClientServerSession != null && SAB.ClientServerSession.Audit != null)
                    {
                        if (!errorFound)
                        {
                            SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
                        }
                        else
                        {
                            //BEGIN TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                            SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                            //SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                            //END TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                        }
                    }

                    highestMessage = SAB.CloseSessions();
                }

                return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
            }
            catch (Exception)
            {
                EventLog.WriteEntry(eventLogID, "Error creating SAB", EventLogEntryType.Error);
                System.Console.Write("Error creating SAB");
                return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
            }
        }
    }
}


