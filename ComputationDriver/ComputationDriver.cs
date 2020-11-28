using System;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Globalization;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;


namespace MIDRetail.ComputationDriver
{
	/// <summary>
	/// Summary description for ComputationDriver.
	/// </summary>
	class ComputationDriver
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args)
		{
			string sourceModule = "ComputationDriver.cs";
			string eventLogID = "MIDComputationDriver";
			SessionAddressBlock SAB;
			SessionSponsor sponsor;
			IMessageCallback messageCallback;
			DateTime start = DateTime.Now;
			int taskListRID = 0;
			int taskSequence = 0;
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
			int _processId = Include.NoRID;
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
			int concurrentProcesses = 5;
			string message = null;
			bool errorFound = false;
            //bool fromScheduler = false;
			string strParm = null;
			messageCallback = new BatchMessageCallback();
			sponsor = new SessionSponsor();
			SAB = new SessionAddressBlock(messageCallback, sponsor);
			System.Runtime.Remoting.Channels.IChannel channel;
			eSecurityAuthenticate authentication = eSecurityAuthenticate.UnknownUser;
			MIDRetail.Business.ComputationDriver ComputationDriver;
			ArrayList ComputationDriverVariables = new ArrayList();
			eMIDMessageLevel highestMessage;


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
				catch (Exception e)
				{
					EventLog.WriteEntry(eventLogID, "Error opening port #0 - " + e.Message, EventLogEntryType.Error);
					throw;
				}

				// Create Sessions

				try
				{
					SAB.CreateSessions((int)eServerType.Client|(int)eServerType.Hierarchy|(int)eServerType.Application|(int)eServerType.Store);
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
					return Convert.ToInt32(eMIDMessageLevel.Severe,CultureInfo.CurrentUICulture);
				}

				authentication = SAB.ClientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"],
					MIDConfigurationManager.AppSettings["Password"], eProcesses.computationDriver);

                //BEGIN TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID
                if (authentication == eSecurityAuthenticate.Unavailable)
                {
                    errorFound = true; //TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                    return Convert.ToInt32(eMIDMessageLevel.Severe);
                }
                //END TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID
                
                if (authentication != eSecurityAuthenticate.UserAuthenticated)
				{
					EventLog.WriteEntry(eventLogID, "Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"], EventLogEntryType.Error);
					System.Console.Write("Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"]);
					errorFound = true;
					return Convert.ToInt32(eMIDMessageLevel.Severe,CultureInfo.CurrentUICulture);
				}

//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//				SAB.ClientServerSession.Initialize();
//
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
				// called from scheduler?
				if (args.Length > 0)
				{
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
					if (args[0] == Include.SchedulerID)
					{
						taskListRID = Convert.ToInt32(args[1]);
						taskSequence = Convert.ToInt32(args[2]);
						_processId = Convert.ToInt32(args[3]);
					}
					else
					{
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
						//fromScheduler = true;
						if (args[0].Length > 0)
						{
							try
							{
								taskListRID = Convert.ToInt32(args[0]);
							}
							catch
							{
								errorFound = true;
								message = MIDText.GetText(eMIDTextCode.msg_InvalidArgument);
								message = message.Replace("{0}", args[0].ToString());
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//								SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, sourceModule);
								EventLog.WriteEntry(eventLogID, message, EventLogEntryType.Error);
								System.Console.Write(message);
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
							}
						}
				
						if (args[1].Length > 0)
						{
							try
							{
								taskSequence = Convert.ToInt32(args[1]);
							}
							catch
							{
								errorFound = true;
								message = MIDText.GetText(eMIDTextCode.msg_InvalidArgument);
								message = message.Replace("{0}", args[1].ToString());
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//								SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, sourceModule);
								EventLog.WriteEntry(eventLogID, message, EventLogEntryType.Error);
								System.Console.Write(message);
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
							}
						}
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
					}
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
				}

//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
				if (_processId != Include.NoRID)
				{
					SAB.ClientServerSession.Initialize(_processId);
				}
				else
				{
					SAB.ClientServerSession.Initialize();
				}

//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"

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

				if (!errorFound)
				{
					SAB.StoreServerSession.Initialize();
					SAB.HierarchyServerSession.Initialize();
					SAB.ApplicationServerSession.Initialize();

					ComputationDriver = new MIDRetail.Business.ComputationDriver(SAB, concurrentProcesses);
					
					ComputationDriver.ProcessComputationDriverRequests(SAB.ClientServerSession, eProcesses.historyPlanLoad);

					SAB.ClientServerSession.Audit.ComputationDriverAuditInfo_Add(ComputationDriver.TotalItems, concurrentProcesses,
						ComputationDriver.TotalErrors);
				}
			}

			catch ( Exception err )
			{
				errorFound = true;
				message = "";
				while(err != null)
				{
					message += " -- " + err.Message;
					err = err.InnerException;
				}
				if (SAB.ClientServerSession.Audit != null)
				{
					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, sourceModule);
					SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
				}
			}
			finally
			{
				if (!errorFound)
				{
					if (SAB.ClientServerSession != null && SAB.ClientServerSession.Audit != null)
					{
						SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
					}
				}
				else
				{
					if (SAB.ClientServerSession != null && SAB.ClientServerSession.Audit != null)
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
	}
}
