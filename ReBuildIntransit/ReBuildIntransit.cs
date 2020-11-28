using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Security.Principal;
using System.Windows;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.ReBuildIntransit
{
	/// <summary>
	/// Summary description for RelieveIntransit.
	/// </summary>
	class ReBuildIntransit
	{
		public ReBuildIntransit()
		{
		}
		/// <summary>
		/// The main entry point for ReBuildIntransit
		/// </summary>
		/// <param name="args">List of Hierarchy Node IDs for which intransit is to be rebuilt</param>
		/// <returns>Highest message level issued</returns>
 
		[STAThread()]
        //BEGIN TT#1644-VSuart-Process Control-MID
        //public static void Main(string[] args)
        static int Main(string[] args)
        //END TT#1644-VSuart-Process Control-MID
        {
            string moduleName = "ReBuildIntransit";
            string message;
			bool errorFound = false;
    		string eventLogID = "MIDReBuildIntransit";
	    	eMIDMessageLevel highestMessage;
	    	SessionSponsor sponsor;
		    SessionAddressBlock SAB;
			IMessageCallback messageCallback;
			System.Runtime.Remoting.Channels.IChannel channel;
			sponsor = new SessionSponsor();
			messageCallback = new BatchMessageCallback();
			SAB = new SessionAddressBlock(messageCallback, sponsor);
            ClientServerSession clientServerSession = null;
            Audit audit = null;
            try
            {
                if (!EventLog.SourceExists(eventLogID))
                {
                    EventLog.CreateEventSource(eventLogID, null);
                }
                // =========================
                // Register callback channel
                // =========================
                try
                {
                    channel = SAB.OpenCallbackChannel();
                }
                catch (Exception Ex)
                {
                    message = moduleName + ":  Error opening port #0 - " + Ex.Message;
                    try
                    {
                        EventLog.WriteEntry(eventLogID, message, EventLogEntryType.Error);
                    }
                    catch (Exception e)
                    {
                        System.Console.Write("Event Log Error [" + e.Message + "] writing event log messge [" + message + "]");
                    }
                    //BEGIN TT#1644-VSuart-Process Control-MID
                    return (int)eMIDMessageLevel.Severe;
                    //END TT#1644-VSuart-Process Control-MID
                }
                // ===============
                // Create Sessions
                // ===============
                try
                {
                    SAB.CreateSessions((int)eServerType.Client | (int)eServerType.Application | (int)eServerType.Hierarchy | (int)eServerType.Store);
                    clientServerSession = SAB.ClientServerSession;
                    audit = clientServerSession.Audit;
                }
                catch (Exception Ex)
                {
                    errorFound = true;
                    Exception innerE = Ex;
                    while (innerE.InnerException != null)
                    {
                        innerE = innerE.InnerException;
                    }
                    message = moduleName + ": Error creating session - " + innerE.Message;
                    try
                    {
                        EventLog.WriteEntry(eventLogID, message, EventLogEntryType.Error);
                    }
                    catch (Exception e)
                    {
                        System.Console.Write("Event Log Error [" + e.Message + "] writing event log message [" + message + "]");
                    }
                    //BEGIN TT#1644-VSuart-Process Control-MID
                    return (int)eMIDMessageLevel.Severe;
                    //END TT#1644-VSuart-Process Control-MID
                }

                // =====
                // Login
                // =====
                // Begin TT#1912 - JSmith - Rebuild Intransit API not decrypting the application user id and password
                //eSecurityAuthenticate authentication = clientServerSession.UserLogin(ConfigurationSettings.AppSettings["User"],
                //    ConfigurationSettings.AppSettings["Password"],
                //    eProcesses.reBuildIntransit);
                eSecurityAuthenticate authentication = clientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"],
                    MIDConfigurationManager.AppSettings["Password"],
                    eProcesses.reBuildIntransit);
                // End TT#1912

                //BEGIN TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID
                //BEGIN TT#1644-VSuart-Process Control-MID
                if (authentication == eSecurityAuthenticate.Unavailable)
                {
                    //return Convert.ToInt32(eMIDMessageLevel.ProcessUnavailable);
                    return Convert.ToInt32(eMIDMessageLevel.Severe);
                }
                //END TT#1644-VSuart-Process Control-MID
                //END TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID

                if (authentication != eSecurityAuthenticate.UserAuthenticated)
                {
                    // Begin TT#1912 - JSmith - Rebuild Intransit API not decrypting the application user id and password
                    //message = moduleName + ": Unable to log in with user:" + ConfigurationSettings.AppSettings["User"] + " password:" + ConfigurationSettings.AppSettings["Password"];
                    message = moduleName + ": Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"];
                    // End TT#1912
                    try
                    {
                        EventLog.WriteEntry(eventLogID, message, EventLogEntryType.Error);
                        System.Console.Write(message);
                    }
                    catch (Exception e)
                    {
                        System.Console.Write("Event Log Error [" + e.Message + "] writing event log message [" + message + "]");
                    }
                    //BEGIN TT#1644-VSuart-Process Control-MID
                    return (int)eMIDMessageLevel.Severe;
                    //END TT#1644-VSuart-Process Control-MID
                }

                clientServerSession.Initialize();

                // ===================
                // Initialize sessions
                // ===================
                SAB.ApplicationServerSession.Initialize();
                //SAB.HierarchyServerSession.Initialize();  // TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
                SAB.StoreServerSession.Initialize();
                // Begin TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
                // StoreServerSession must be initialized before HierarchyServerSession 
                SAB.HierarchyServerSession.Initialize();
                // End TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.

                try
                {
                    System.Windows.Forms.Application.Run(new ReBuildIntransitProcess(SAB, ref errorFound));
                }
                catch (Exception Ex)
                {
                    errorFound = true;
                    if (audit != null)
                    {
                        audit.Add_Msg(eMIDMessageLevel.Severe, Ex.Message, moduleName);
                    }
                    message = moduleName + ": " + MIDText.GetTextOnly(eMIDTextCode.sum_Failed) + ": " + Ex.Message;
                    try
                    {
                        EventLog.WriteEntry(eventLogID, message, EventLogEntryType.Information);
                    }
                    catch (Exception e)
                    {
                        System.Console.Write("Event Log Error [" + e.Message + "] writing event log message [" + message + "]");
                    }
                }
                finally
                {
                }
            }
            catch (Exception Ex)
            {
                errorFound = true;
                if (audit != null)
                {
                    audit.Add_Msg(eMIDMessageLevel.Severe, Ex.Message, moduleName);
                }
            }
            finally
            {
                if (!errorFound)
                {
                    message = moduleName + ": " + MIDText.GetTextOnly(eMIDTextCode.sum_Successful);
                    try
                    {
                        EventLog.WriteEntry(eventLogID, message, EventLogEntryType.Information);
                    }
                    catch (Exception e)
                    {
                        System.Console.Write("Event Log Error [" + e.Message + "] writing event log message [" + message + "]");
                    }
                    if (audit != null)
                    {
                        audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
                    }
                }
                else
                {
                    if (audit != null)
                    {
                        audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                        message = moduleName + ": " + MIDText.GetTextOnly(eMIDTextCode.sum_Failed);
                    }
                    else
                    {
                        message = moduleName + ": Process failed";
                    }
                    try
                    {
                        EventLog.WriteEntry(eventLogID, message, EventLogEntryType.Information);
                    }
                    catch (Exception e)
                    {
                        System.Console.Write("Event Log Error [" + e.Message + "] writing event log message [" + message + "]");
                    }
                }
                highestMessage = SAB.CloseSessions();
                if (clientServerSession != null)
                {
                    message = moduleName + " Ended with Highest Message Level: " + MIDText.GetTextOnly((int)highestMessage);
                }
                else
                {
                    message = moduleName + " Ended with severe errors";
                }
                try
                {
                    EventLog.WriteEntry(eventLogID, message, EventLogEntryType.Information);
                }
                catch (Exception e)
                {
                    System.Console.Write("Event Log Error [" + e.Message + "] writing event log message [" + message + "]");
                }
            }
            //BEGIN TT#1644-VSuart-Process Control-MID
            return (int)eMIDMessageLevel.Severe;
            //END TT#1644-VSuart-Process Control-MID
        }
 	}
}


