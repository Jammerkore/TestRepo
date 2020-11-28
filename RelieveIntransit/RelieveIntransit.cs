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

namespace MIDRetail.RelieveIntransit
{
	/// <summary>
	/// Summary description for RelieveIntransit.
	/// </summary>
	class RelieveIntransit
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args)
		{

			bool useDefault = true;
			bool errorFound = false;
			bool serializeXMLFile = false;
			bool headerIDsPresent = true;
			bool relieveIntransitForAllStores = false;  // MID Track 5694 MA Relieve Intransit Enhancement: Relieve Intransit by Header ID

			char[] delimiter = {'~'};
			char[] levelDelimiter = {'\\'};

			string message = null;
			string fileLocation = null;
			string eventLogID = "MIDRelieveIntransit";
			string sourceModule = "RelieveIntransit.cs";
			eMIDMessageLevel highestMessage;
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
			int _processId = Include.NoRID;
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"

            //Hashtable port;

			SessionSponsor sponsor;

			SessionAddressBlock SAB;

			RelieveIntransitProcess rip;

			IMessageCallback messageCallback;

            //BinaryServerFormatterSinkProvider provider;

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
					EventLog.WriteEntry(eventLogID, "Error opening port #0 - " + Ex.Message, EventLogEntryType.Error);
					return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
				}

				// ===============
				// Create Sessions
				// ===============
				try
				{
                    SAB.CreateSessions((int)eServerType.Client | (int)eServerType.Application | (int)eServerType.Hierarchy | (int)eServerType.Store | (int)eServerType.Header);	// TT#1133-MD - stodd - Relieve IT not working with group allocation - 
				}

				catch (Exception Ex)
				{
					errorFound = true;
					Exception innerE = Ex;

					while (innerE.InnerException != null)
					{
						innerE = innerE.InnerException;
					}

					EventLog.WriteEntry(eventLogID, "Error creating sessions - " + innerE.Message, EventLogEntryType.Error);

					return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
				}

				// =====
				// Login
				// =====
				eSecurityAuthenticate authentication = SAB.ClientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"],
																						 MIDConfigurationManager.AppSettings["Password"],
																						 eProcesses.relieveIntransit);

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
					EventLog.WriteEntry(eventLogID, "Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"], EventLogEntryType.Error);
					System.Console.Write("Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"]);
					return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
				}

//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//				SAB.ClientServerSession.Initialize();
//
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
				// ===============================================
				// Retrieve command-line and configuration options
				// ===============================================
				if (args.Length > 0)
				{
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
					if (args[0] == Include.SchedulerID)
					{
						fileLocation = args[1];
						_processId = Convert.ToInt32(args[2]);
                        // Begin TT#1054 - JSmith - Relieve Intransit not working.
                        //delimiter = ConfigurationSettings.AppSettings["Delimiter"].ToCharArray();
                        delimiter = MIDConfigurationManager.AppSettings["Delimiter"].ToCharArray();
                        // End TT#1054
					}
					else
					{
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
						fileLocation = args[0];
						if (args.Length > 1)
						{
							delimiter = args[1].ToCharArray();
						}
						else
						{
							delimiter = MIDConfigurationManager.AppSettings["Delimiter"].ToCharArray();
						}
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
					}
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
				}
				else
				{
					fileLocation = MIDConfigurationManager.AppSettings["InputFile"];
					string strDelimiter = MIDConfigurationManager.AppSettings["Delimiter"];
					if (strDelimiter != null)
					{
						delimiter = strDelimiter.ToCharArray();
					}
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
				// ================================
				// Retrieve SerializeXMLFile option
				// ================================
				useDefault = true;

				string strSerializeXMLFile = MIDConfigurationManager.AppSettings["SerializeXMLFile"];

				if (strSerializeXMLFile != null)
				{
					try
					{
						useDefault = false;
						serializeXMLFile = Convert.ToBoolean(strSerializeXMLFile);
					}
					catch
					{
						useDefault = true;
					}
				}

				if (useDefault)
				{
					message = MIDText.GetText(eMIDTextCode.msg_BatchSerializeXMLFileDefaulted);
					message = message.Replace("{0}", Include.DefaultSerializeXMLFile.ToString());
					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule);
					serializeXMLFile = Include.DefaultSerializeXMLFile;
				}

				// ================================
				// Retrieve HeaderIDsPresent option
				// ================================
				useDefault = true;

				string strHeaderIDsPresent = MIDConfigurationManager.AppSettings["HeaderIDsPresent"];

				if (strHeaderIDsPresent != null)
				{
					try
					{
						useDefault = false;
						headerIDsPresent = Convert.ToBoolean(strHeaderIDsPresent);
					}
					catch
					{
						useDefault = true;
					}
				}

                // begin MID TRack 5694 MA Relieve Intransit Enhancement: ability to relieve by header id
				if (headerIDsPresent)
				{
                    // Begin TT#1054 - JSmith - Relieve Intransit not working.
                    //string strRelieveAllStores = ConfigurationSettings.AppSettings["RelieveAllStores"];
                    string strRelieveAllStores = MIDConfigurationManager.AppSettings["RelieveAllStores"];
                    // End TT#1054
					if (strRelieveAllStores != null)
					{
						try
						{
							relieveIntransitForAllStores = Convert.ToBoolean(strRelieveAllStores);
						}
						catch
						{
						}
					}
					message = MIDText.GetText(eMIDTextCode.msg_RelieveAllStoresDefaultSetting);
					message = message.Replace("{0}", relieveIntransitForAllStores.ToString());
					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule,true);
				}
				// end MID TRack 5694 MA Relieve Intransit Enhancement: ability to relieve by header id
// (CSMITH) - BEG MID Track #2979: Empty Input File
//				// ====================
//				// Process transactions
//				// ====================
//				if (fileLocation == null || !File.Exists(fileLocation))
//				{
//					SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_InputFileNotFound, "", SAB.GetHighestAuditMessageLevel());
//					return Convert.ToInt32(SAB.GetHighestAuditMessageLevel(),CultureInfo.CurrentUICulture);
//				}
//				else
//				{
//					// ===================
//					// Initialize sessions
//					// ===================
//
//					SAB.ApplicationServerSession.Initialize();
//
//					SAB.HierarchyServerSession.Initialize();
//
//					SAB.StoreServerSession.Initialize();
//
//					message = MIDText.GetText(eMIDTextCode.msg_BatchInputFile);
//					message = message.Replace("{0}", fileLocation);
//					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule);
//					rip = new RelieveIntransitProcess(SAB, ref errorFound, headerIDsPresent, levelDelimiter);
//					if (!errorFound)
//					{
//						if (fileLocation.Substring(fileLocation.Length - 4).ToUpper() == ".XML")
//						{
//							if (serializeXMLFile)
//							{
//								rip.SerializeVariableFile(fileLocation, ref errorFound);
//							}
//							else
//							{
//								rip.ProcessVariableFile(fileLocation, ref errorFound);
//							}
//						}
//						else
//						{
//							rip.ProcessVariableFile(fileLocation, delimiter, ref errorFound);
//						}
//					}
//				}

				// ====================
				// Process transactions
				// ====================
				message = MIDText.GetText(eMIDTextCode.msg_BatchInputFile);

				if (fileLocation == "" || fileLocation == null)
				{
					message = message.Replace("{0}.", "[" + fileLocation + "] NOT specified" + System.Environment.NewLine);
					message += "Relieve Intransit Process NOT run";

					// Begin Track #5035 - JSmith - file not found message level inconsistent
//					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule, true);
					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, message, sourceModule, true);
					// End Track #5035

					SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_InputFileNotFound, "", SAB.GetHighestAuditMessageLevel());

					return Convert.ToInt32(SAB.GetHighestAuditMessageLevel(), CultureInfo.CurrentUICulture);
				}
				else
				{
					if (!File.Exists(fileLocation))
					{
						message = message.Replace("{0}.", "[" + fileLocation + "] does NOT exist" + System.Environment.NewLine);
						message += "Relieve Intransit Process NOT run";

						SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule, true);

						SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_InputFileNotFound, "", SAB.GetHighestAuditMessageLevel());

						return Convert.ToInt32(SAB.GetHighestAuditMessageLevel(), CultureInfo.CurrentUICulture);
					}
					else
					{
						FileInfo txnFileInfo = new FileInfo(fileLocation);

						if (txnFileInfo.Length == 0)
						{
							message = message.Replace("{0}.", "[" + fileLocation + "] is an empty file" + System.Environment.NewLine);
							message += "Relieve Intransit Process NOT run";

							// Begin Track #5035 - JSmith - file not found message level inconsistent
//							SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule, true);
							SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, message, sourceModule, true);
							// End Track #5035

							SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_InputFileNotFound, "", SAB.GetHighestAuditMessageLevel());

							return Convert.ToInt32(SAB.GetHighestAuditMessageLevel(), CultureInfo.CurrentUICulture);
						}
						else
						{
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

							message = message.Replace("{0}", "[" + fileLocation + "]");

							SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule, true);

							rip = new RelieveIntransitProcess(SAB, ref errorFound, headerIDsPresent, levelDelimiter, relieveIntransitForAllStores); // MID Track 5694 Relieve Intransit By Header ID

							if (!errorFound)
							{
								if (fileLocation.Substring(fileLocation.Length - 4).ToUpper() == ".XML")
								{
									if (serializeXMLFile)
									{
										rip.SerializeVariableFile(fileLocation, ref errorFound);
									}
									else
									{
										rip.ProcessVariableFile(fileLocation, ref errorFound);
									}
								}
								else
								{
									rip.ProcessVariableFile(fileLocation, delimiter, ref errorFound);
								}
							}
						}
					}
				}
// (CSMITH) - END MID Track #2979
			}

			catch (Exception Ex)
			{
				errorFound = true;

				SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, Ex.Message, sourceModule);

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
						SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
					}
				}

				highestMessage = SAB.CloseSessions();
			}

			return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
		}
	}
}
