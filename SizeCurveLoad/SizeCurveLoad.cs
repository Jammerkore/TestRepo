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

namespace MIDRetail.SizeCurveLoad
{
	/// <summary>
	/// Summary description for SizeCurveLoad.
	/// </summary>
	class SizeCurveLoad
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args)
		{
			char[] delimiter = {'~'};

			bool errorFound = false;
			bool scErrorFound = false;
			bool scgErrorFound = false;
			bool scCodesPresent = false;

			string userId = null;
			string passWd = null;
			string msgText = null;
			string optDelimiter = null;
			string optCodesPresent = null;
			string sizeCurveTransFile = null;
			string sizeCurveGroupTransFile = null;
			string eventLogID = "MIDSizeCurveLoad";
			string sourceModule = "SizeCurveLoad.cs";
            eMIDMessageLevel highestMessage;
			// BEGIN MID Track #5153 - JSmith - Create curve on modify if not found
			bool createOnModify = false;
			// END MID Track #5153
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
			int _processId = Include.NoRID;
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"

            //Hashtable port;

			SessionSponsor sponsor;

			SessionAddressBlock SAB;

			SizeCurveLoadProcess sclp = null;

			SizeCurveGroupLoadProcess scglp = null;

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

				eSecurityAuthenticate authentication = SAB.ClientServerSession.UserLogin(userId, passWd, eProcesses.sizeCurveLoad);

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
				{
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
					if (args[0] == Include.SchedulerID)
					{
						if (args[1].IndexOf(".grp") != -1)
						{
							sizeCurveGroupTransFile = args[1];
							sizeCurveTransFile = sizeCurveGroupTransFile.Replace(".grp", ".crv");
						}
						else if (args[1].IndexOf(".crv") != -1)
						{
							sizeCurveTransFile = args[1];
							sizeCurveGroupTransFile = sizeCurveTransFile.Replace(".crv", ".grp");
						}
						else
						{
							msgText = MIDText.GetText(eMIDTextCode.msg_BatchInputFile);
							msgText = msgText.Replace("{0}.", "Invalid format of file name [" + args[1] + "]. Must contain .grp or .crv"+ System.Environment.NewLine);
								EventLog.WriteEntry(eventLogID, msgText, EventLogEntryType.Error);
								System.Console.Write(msgText);
						}

						_processId = Convert.ToInt32(args[2]);

                        // Begin TT#1054 - JSmith - Relieve Intransit not working.
                        //optDelimiter = ConfigurationSettings.AppSettings["Delimiter"];
                        optDelimiter = MIDConfigurationManager.AppSettings["Delimiter"];
                        // End TT#1054

						if (optDelimiter != "" && optDelimiter != null)
						{
							delimiter = optDelimiter.ToCharArray();
						}
					}
					else
					{
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//Begin Track #4183 - JScott - Add Size Curve Load to scheduler
//						sizeCurveGroupTransFile = args[0];
//
//End Track #4183 - JScott - Add Size Curve Load to scheduler
						if (args.Length > 1)
						{
//Begin Track #4183 - JScott - Add Size Curve Load to scheduler
							sizeCurveGroupTransFile = args[0];
//End Track #4183 - JScott - Add Size Curve Load to scheduler
							sizeCurveTransFile = args[1];

							if (args.Length > 2)
							{
								delimiter = args[2].ToCharArray();
							}
							else
							{
								optDelimiter = MIDConfigurationManager.AppSettings["Delimiter"];

								if (optDelimiter != "" && optDelimiter != null)
								{
									delimiter = optDelimiter.ToCharArray();
								}
							}
						}
						else
						{
//Begin Track #4183 - JScott - Add Size Curve Load to scheduler
//							sizeCurveTransFile = MIDConfigurationManager.AppSettings["CurveTransFile"];
//
//							optDelimiter = MIDConfigurationManager.AppSettings["Delimiter"];
//
//							if (optDelimiter != "" && optDelimiter != null)
//							{
//								delimiter = optDelimiter.ToCharArray();
//							}
							if (args[0].IndexOf(".grp") != -1)
							{
								sizeCurveGroupTransFile = args[0];
								sizeCurveTransFile = sizeCurveGroupTransFile.Replace(".grp", ".crv");
							}
							else if (args[0].IndexOf(".crv") != -1)
							{
								sizeCurveTransFile = args[0];
								sizeCurveGroupTransFile = sizeCurveTransFile.Replace(".crv", ".grp");
							}
							else
							{
								msgText = MIDText.GetText(eMIDTextCode.msg_BatchInputFile);
								msgText = msgText.Replace("{0}.", "Invalid format of file name [" + args[0] + "]. Must contain .grp or .crv"+ System.Environment.NewLine);

//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//								SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msgText, sourceModule, true);
								EventLog.WriteEntry(eventLogID, msgText, EventLogEntryType.Error);
								System.Console.Write(msgText);
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
							}

							optDelimiter = MIDConfigurationManager.AppSettings["Delimiter"];

							if (optDelimiter != "" && optDelimiter != null)
							{
								delimiter = optDelimiter.ToCharArray();
							}
//End Track #4183 - JScott - Add Size Curve Load to scheduler
						}
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
					}
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
				}
				else
				{
					sizeCurveGroupTransFile = MIDConfigurationManager.AppSettings["CurveGroupTransFile"];

					sizeCurveTransFile = MIDConfigurationManager.AppSettings["CurveTransFile"];

					optDelimiter = MIDConfigurationManager.AppSettings["Delimiter"];

					if (optDelimiter != "" && optDelimiter != null)
					{
						delimiter = optDelimiter.ToCharArray();
					}
				}

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
				// ==========================================
				// Retrieve CodesPresent configuration option
				// ==========================================
				optCodesPresent = MIDConfigurationManager.AppSettings["CodesPresent"];

				if (optCodesPresent == "" || optCodesPresent == null)
				{
					scCodesPresent = true;
				}
				else
				{
					try
					{
						scCodesPresent = Convert.ToBoolean(optCodesPresent, CultureInfo.CurrentUICulture);
					}

					catch
					{
						scCodesPresent = true;
					}
				}

				// BEGIN MID Track #5153 - JSmith - Create curve on modify if not found
				string strCreateOnModify = MIDConfigurationManager.AppSettings["CreateOnModify"];
				if (strCreateOnModify != null)
				{
					try
					{
						createOnModify = Convert.ToBoolean(strCreateOnModify);
					}
					catch
					{
						createOnModify = false;
					}
				}
				// END MID Track #5153

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

				// =====================================
				// Process size curve group transactions
				// =====================================
				msgText = MIDText.GetText(eMIDTextCode.msg_BatchInputFile);

				if (sizeCurveGroupTransFile == "" || sizeCurveGroupTransFile == null)
				{
					msgText = msgText.Replace("{0}.", "[" + sizeCurveGroupTransFile + "] NOT specified" + System.Environment.NewLine);
					msgText += "Size Curve Group Load Process NOT run";

					// Begin Track #5035 - JSmith - file not found message level inconsistent
//					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, sourceModule, true);
					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, msgText, sourceModule, true);
					// End Track #5035
				}
				else
				{
					if (sizeCurveGroupTransFile.Substring(sizeCurveGroupTransFile.Length - 4).ToUpper() != ".XML")
					{
						msgText = msgText.Replace("{0}.", "[" + sizeCurveGroupTransFile + "] does NOT exist" + System.Environment.NewLine);
						msgText += "Size Curve Group Load Process NOT run";

						// Begin Track #5035 - JSmith - file not found message level inconsistent
//						SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, sourceModule, true);
						SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msgText, sourceModule, true);
						// End Track #5035
					}
					else
					{
						if (!File.Exists(sizeCurveGroupTransFile))
						{
							msgText = msgText.Replace("{0}.", "[" + sizeCurveGroupTransFile + "] does NOT exist" + System.Environment.NewLine);
							msgText += "Size Curve Group Load Process NOT run";

							// Begin Track #5035 - JSmith - file not found message level inconsistent
//							SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, sourceModule, true);
							SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msgText, sourceModule, true);
							// End Track #5035
						}
						else
						{
							FileInfo scgFileInfo = new FileInfo(sizeCurveGroupTransFile);

							if (scgFileInfo.Length == 0)
							{
								msgText = msgText.Replace("{0}.", "[" + sizeCurveGroupTransFile + "] is an empty file" + System.Environment.NewLine);
								msgText += "Size Curve Group Load Process NOT run";

								// Begin Track #5035 - JSmith - file not found message level inconsistent
								SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, msgText, sourceModule, true);
								// End Track #5035
							}
							else
							{
								msgText = msgText.Replace("{0}", "[" + sizeCurveGroupTransFile + "]");

								SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, sourceModule, true);

								// BEGIN MID Track #5153 - JSmith - Create curve on modify if not found
//								scglp = new SizeCurveGroupLoadProcess(SAB, scCodesPresent, ref scgErrorFound);
								scglp = new SizeCurveGroupLoadProcess(SAB, scCodesPresent, createOnModify, ref scgErrorFound);
								// END MID Track #5153

								if (!scgErrorFound)
								{
									scglp.ProcessGroupTrans(sizeCurveGroupTransFile, ref scgErrorFound);
								}
							}
						}
					}
				}

				// ===============================
				// Process size curve transactions
				// ===============================
				msgText = MIDText.GetText(eMIDTextCode.msg_BatchInputFile);

				if (sizeCurveTransFile == "" || sizeCurveTransFile == null)
				{
					msgText = msgText.Replace("{0}.", "[" + sizeCurveTransFile + "] NOT specified" + System.Environment.NewLine);
					msgText += "Size Curve Load Process NOT run";

					// Begin Track #5035 - JSmith - file not found message level inconsistent
//					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, sourceModule);
					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, msgText, sourceModule);
					// End Track #5035
				}
				else
				{
					if (sizeCurveTransFile.Substring(sizeCurveTransFile.Length - 4).ToUpper() != ".XML")
					{
						msgText = msgText.Replace("{0}.", "[" + sizeCurveTransFile + "] does NOT exist" + System.Environment.NewLine);
						msgText += "Size Curve Load Process NOT run";

						// Begin Track #5035 - JSmith - file not found message level inconsistent
//						SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, sourceModule);
						SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msgText, sourceModule);
						// End Track #5035
					}
					else
					{
						if (!File.Exists(sizeCurveTransFile))
						{
							msgText = msgText.Replace("{0}.", "[" + sizeCurveTransFile + "] does NOT exist" + System.Environment.NewLine);
							msgText += "Size Curve Load Process NOT run";

							// Begin Track #5035 - JSmith - file not found message level inconsistent
//							SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, sourceModule);
							SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msgText, sourceModule);
							// End Track #5035
						}
						else
						{
							FileInfo scFileInfo = new FileInfo(sizeCurveTransFile);

							if (scFileInfo.Length == 0)
							{
								msgText = msgText.Replace("{0}.", "[" + sizeCurveTransFile + "] is an empty file" + System.Environment.NewLine);
								msgText += "Size Curve Load Process NOT run";

								// Begin Track #5035 - JSmith - file not found message level inconsistent
//								SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msgText, sourceModule);
								SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, msgText, sourceModule);
								// End Track #5035
							}
							else
							{
								msgText = msgText.Replace("{0}", "[" + sizeCurveTransFile + "]");

								SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, sourceModule);

								sclp = new SizeCurveLoadProcess(SAB, ref scErrorFound);
								
								if (!scErrorFound)
								{
									sclp.ProcessCurveTrans(sizeCurveTransFile, ref scErrorFound);
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
				if (scErrorFound || scgErrorFound)
				{
					errorFound = true;
				}

				if (SAB.ClientServerSession != null && SAB.ClientServerSession.Audit != null)
				{
                    //BEGIN TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
                    if (sclp != null)
                    {
                        if (!errorFound)
                        {
                            SAB.ClientServerSession.Audit.SizeCurveLoadAuditInfo_Add(sclp.CurvesRead, sclp.CurvesError, sclp.CurvesCreate, 0, 0, scglp.GroupsRead, scglp.GroupsWithErrors, scglp.GroupsCreated, scglp.GroupsModify, scglp.GroupsRemoved);
                            SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
                        }
                        else
                        {
                            SAB.ClientServerSession.Audit.SizeCurveLoadAuditInfo_Add(sclp.CurvesRead, sclp.CurvesError, sclp.CurvesCreate, 0, 0, scglp.GroupsRead, scglp.GroupsWithErrors, scglp.GroupsCreated, scglp.GroupsModify, scglp.GroupsRemoved);
                            //BEGIN TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                            //SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                            SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                            //END TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                        }
                    }
                    //END TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
                    //BEGIN TT#1675 - MD - DOConnell -Size Curve Load returning as UnAssigned when error occurs
                    else
                    {
                        SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                    }
                    //END TT#1675 - MD - DOConnell -Size Curve Load returning as UnAssigned when error occurs
                }

				highestMessage = SAB.CloseSessions();
			}

			return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
		}
	}
}
