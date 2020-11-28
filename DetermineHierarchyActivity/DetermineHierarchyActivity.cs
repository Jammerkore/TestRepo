using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;


namespace MIDRetail.DetermineHierarchyActivity
{
    class DetermineHierarchyActivity
    {
        static int Main(string[] args)
        {
            string sourceModule = "DetermineHierarchyActivity.cs";
			string eventLogID = "MIDDetermineHierarchyActivity";
			SessionAddressBlock SAB;
			SessionSponsor sponsor;
			IMessageCallback messageCallback;
			DateTime start = DateTime.Now;
			bool errorFound = false;
			messageCallback = new BatchMessageCallback();
			sponsor = new SessionSponsor();
			SAB = new SessionAddressBlock(messageCallback, sponsor);
			System.Runtime.Remoting.Channels.IChannel channel;
			eSecurityAuthenticate authentication = eSecurityAuthenticate.UnknownUser;
			eMIDMessageLevel highestMessage;
			int _processId = Include.NoRID;
            int fromOffset = 2;
            int toOffset = 0;
            int forecastOffset = 0;
            int intransitOffset = 2;
            // Begin TT#632-MD - JSmith - Add Concurrent Processes to Determine Hierarchy Activity
            int concurrentProcesses = 5; 
            int commitLimit = int.MaxValue;
            // End TT#632-MD - JSmith - Add Concurrent Processes to Determine Hierarchy Activity
            DetermineHierarchyActivityProcess _dhap;

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
                    EventLog.WriteEntry(eventLogID, "Error opening port #0 - " + ex.Message, EventLogEntryType.Error);
                    return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
                }

                // Create Sessions

                try
                {
                    SAB.CreateSessions((int)eServerType.Client | (int)eServerType.Hierarchy);
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

                authentication = SAB.ClientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"],
                    MIDConfigurationManager.AppSettings["Password"], eProcesses.determineHierarchyActivity);

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
                    EventLog.WriteEntry(eventLogID, "Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"], EventLogEntryType.Error);
                    System.Console.Write("Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"]);
                    return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
                }

                

                try
                {
                    string strParm = MIDConfigurationManager.AppSettings["FromOffset"];
                    if (strParm != null)
                    {
                        fromOffset = Convert.ToInt32(strParm);
                    }
                    strParm = MIDConfigurationManager.AppSettings["ToOffset"];
                    if (strParm != null)
                    {
                        toOffset = Convert.ToInt32(strParm);
                    }
                    strParm = MIDConfigurationManager.AppSettings["ForecastOffset"];
                    if (strParm != null)
                    {
                        forecastOffset = Convert.ToInt32(strParm);
                    }
                    strParm = MIDConfigurationManager.AppSettings["IntransitOffset"];
                    if (strParm != null)
                    {
                        intransitOffset = Convert.ToInt32(strParm);
                    }
                    // Begin TT#632-MD - JSmith - Add Concurrent Processes to Determine Hierarchy Activity
                    strParm = MIDConfigurationManager.AppSettings["ConcurrentProcesses"];
                    if (strParm != null)
                    {
                        try
                        {
                            concurrentProcesses = Convert.ToInt32(strParm);
                        }
                        catch
                        {
                        }
                    }
                    strParm = MIDConfigurationManager.AppSettings["CommitLimit"];
                    if (strParm != null)
                    {
                        if (strParm.ToUpper(CultureInfo.CurrentCulture) == "UNLIMITED")
                        {
                            commitLimit = int.MaxValue;
                        }
                        else
                        {
                            try
                            {
                                commitLimit = Convert.ToInt32(strParm);
                            }
                            catch
                            {
                            }
                        }
                    }
                    // End TT#632-MD - JSmith - Add Concurrent Processes to Determine Hierarchy Activity

                    if (args.Length > 0)
                    {
                        fromOffset = Convert.ToInt32(args[0]);
                        if (args.Length > 1)
                        {
                            toOffset = Convert.ToInt32(args[1]);
                            if (args.Length > 2)
                            {
                                intransitOffset = Convert.ToInt32(args[2]);
                                if (args.Length > 3)
                                {
                                    forecastOffset = Convert.ToInt32(args[3]);
                                }
                            }
                        }
                    }
                }
                catch
                {
                }

                SAB.ClientServerSession.Initialize();
                SAB.HierarchyServerSession.Initialize();

                _dhap = new DetermineHierarchyActivityProcess(SAB, ref errorFound);

                // Begin TT#632-MD - JSmith - Add Concurrent Processes to Determine Hierarchy Activity
                //if (_dhap.ProcessHierarchyActivity(fromOffset * -1, toOffset, forecastOffset, intransitOffset) != eReturnCode.successful)
                if (_dhap.ProcessHierarchyActivity(commitLimit, concurrentProcesses, fromOffset * -1, toOffset, forecastOffset, intransitOffset) != eReturnCode.successful)
                // End TT#632-MD - JSmith - Add Concurrent Processes to Determine Hierarchy Activity
                {
                    errorFound = true;
                }
            }

            catch (Exception ex)
            {
                errorFound = true;
                SAB.ClientServerSession.Audit.Log_Exception(ex, sourceModule);
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

                highestMessage = SAB.CloseSessions();
            }

            return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
        }
    }
}
