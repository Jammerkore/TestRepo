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

namespace MIDRetail.SizeConstraintsLoad
{
	/// <summary>
	/// Summary description for SizeConstraintsLoad.
	/// </summary>
	public class SizeConstraintsLoad
	{
		[STAThread]
		static int Main(string[] args)
		{
			char[] delimiter = {'~'};
		
			bool errorFound = false;
			bool scErrorFound = false;

			string userId = null;
			string passWd = null;
			string msgText = "";
			string sizeConstraintsTransFile = null;
			string eventLogID = "MIDSizeConstraintsLoad";
			string sourceModule = "SizeConstraintsLoad.cs";
			eMIDMessageLevel highestMessage;
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
			int _processId = Include.NoRID;
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"

			SessionSponsor sponsor;

			SessionAddressBlock SAB;

			SizeConstraintsLoadProcess sclp;

			IMessageCallback messageCallback;

			System.Runtime.Remoting.Channels.IChannel channel;

			sponsor = new SessionSponsor();

			messageCallback = new BatchMessageCallback();

			SAB = new SessionAddressBlock(messageCallback, sponsor);
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
					EventLog.WriteEntry(eventLogID, "Error opening port #0 - " + Ex.ToString(), EventLogEntryType.Error);
					return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
				}

				// ===============
				// Create Sessions
				// ===============
				try
				{
					SAB.CreateSessions((int)eServerType.Client | (int)eServerType.Application | (int)eServerType.Hierarchy | (int)eServerType.Store);
				}

				catch (Exception Ex)
				{
					errorFound = true;

					Exception innerE = Ex;

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

				eSecurityAuthenticate authentication = SAB.ClientServerSession.UserLogin(userId, passWd, eProcesses.sizeConstraintsLoad);

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

				if (authentication != eSecurityAuthenticate.UserAuthenticated)
				{
					EventLog.WriteEntry(eventLogID, "Unable to log in with user: [" + userId + "] password: [" + passWd + "]", EventLogEntryType.Error);

					System.Console.Write("Unable to log in with user: [" + userId + "] password: [" + passWd + "]");

					return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
				}

//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//				// =================================================
//				// initialize client session to make audit available
//				// =================================================
//				SAB.ClientServerSession.Initialize();
//
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
				// ===============================================
				// Retrieve command-line and configuration options
				// ===============================================
				if (args.Length > 0)
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
					if (args[0] == Include.SchedulerID)
					{
						sizeConstraintsTransFile = args[1];
						_processId = Convert.ToInt32(args[2]);
					}
					else
					{
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
						sizeConstraintsTransFile = args[0];
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
					}
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
				else
				  sizeConstraintsTransFile = MIDConfigurationManager.AppSettings["SizeConstraintsTransFile"];

//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
				// =================================================
				// initialize client session to make audit available
				// =================================================
				if (_processId != Include.NoRID)
				{
					SAB.ClientServerSession.Initialize(_processId);
				}
				else
				{
					SAB.ClientServerSession.Initialize();
				}

//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
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
	
				msgText = MIDText.GetText(eMIDTextCode.msg_BatchInputFile);

				if (sizeConstraintsTransFile == "" || sizeConstraintsTransFile == null)
				{

					msgText =  msgText.Replace("{0}.","[" + sizeConstraintsTransFile + "] NOT found" + System.Environment.NewLine);
					msgText += "Size Constraints Load Process NOT run";

					// Begin Track #5035 - JSmith - file not found message level inconsistent
//					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msgText, sourceModule, true);
					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, msgText, sourceModule, true);
					// End Track #5035
				}
				else
				{
					if (sizeConstraintsTransFile.Substring(sizeConstraintsTransFile.Length - 4).ToUpper() != ".XML")
					{
						msgText = msgText.Replace("{0}.", "[" + sizeConstraintsTransFile + "] is not an XML file" + System.Environment.NewLine);
						msgText += "Size Constraints Load Process NOT run";

						SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msgText, sourceModule);
					}
					else
					{
						if (!File.Exists(sizeConstraintsTransFile))
						{
							msgText = msgText.Replace("{0}.", "[" + sizeConstraintsTransFile + "] does NOT exist" + System.Environment.NewLine);
							msgText += "Size Constraints Load Process NOT run";

							// Begin Track #5035 - JSmith - file not found message level inconsistent
//							SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msgText, sourceModule);
							SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msgText, sourceModule);
							// End Track #5035
						}
						else
						{
							FileInfo scFileInfo = new FileInfo(sizeConstraintsTransFile);

							if (scFileInfo.Length == 0)
							{
								msgText = msgText.Replace("{0}.", "[" + sizeConstraintsTransFile + "] is an empty file" + System.Environment.NewLine);
								msgText += "Size Constraints Load Process NOT run";

								// Begin Track #5035 - JSmith - file not found message level inconsistent
//								SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msgText, sourceModule);
								SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, msgText, sourceModule);
								// End Track #5035
							}
							else
							{
								// ===============================
								// Process size curve transactions
								// ===============================
								msgText = msgText.Replace("{0}", "[" + sizeConstraintsTransFile + "]");
								SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, sourceModule);
								sclp = new SizeConstraintsLoadProcess(SAB, ref scErrorFound);
								if (!scErrorFound)
								{
									sclp.ProcessSizeConstraintsTrans(sizeConstraintsTransFile, ref scErrorFound);
								}
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
				if (scErrorFound)
				{
					errorFound = true;
				}

				if (SAB.ClientServerSession != null && SAB.ClientServerSession.Audit != null)
				{
					if (!errorFound)
					{
						SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
					}
					else
					{
                        //BEGIN TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                        //SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                        SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                        //END TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
					}
				}

				highestMessage = SAB.CloseSessions();
			}

			return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
		}
	}
}
