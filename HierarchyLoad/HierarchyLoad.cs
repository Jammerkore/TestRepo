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


namespace MIDRetail.HierarchyLoad
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class HierarchyLoad
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args)
		{
			string sourceModule = "HierarchyLoad.cs";
			string eventLogID = "MIDHierarchyLoad";
			SessionAddressBlock SAB;
			SessionSponsor sponsor;
			IMessageCallback messageCallback;
			DateTime start = DateTime.Now;
//			int returnCode = 0;
			string fileLocation = null;
			char[] delimiter = {'~'};
//			int processRID = -1;
			string message = null;
			int commitLimit = 1000;
			bool errorFound = false;
			bool addCharacteristicGroups = false;
            // Begin TT#221 - JSmith - Characteristic with pre-defined values dynamically adds new values
            //bool addCharacteristicValues = true;
            bool addCharacteristicValues = false;
            // End TT#221
            // Begin TT#167 - JSmith - Product Hierarchy characteristics auto add
            string characteristicDelimiter = "\\";
            // End TT#167
			messageCallback = new BatchMessageCallback();
			sponsor = new SessionSponsor();
            // Begin TT#1646-MD - JSmith - Add or Remove Hierarchy Levels
            //SAB = new SessionAddressBlock(messageCallback, sponsor);
            bool forceLocalOverride = false;
            // End TT#1646-MD - JSmith - Add or Remove Hierarchy Levels
            //BinaryServerFormatterSinkProvider provider;
            //Hashtable port;
			System.Runtime.Remoting.Channels.IChannel channel;
			eSecurityAuthenticate authentication = eSecurityAuthenticate.UnknownUser;
			eMIDMessageLevel highestMessage;
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
			int _processId = Include.NoRID;
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"

            // Begin TT#2010 - JSmith - Split merchandise characteristics to separate delimited transaction layout
            bool useCharacteristicTransaction = false;
            // End TT#2010

            // Begin TT#1646-MD - JSmith - Add or Remove Hierarchy Levels
            //SAB = new SessionAddressBlock(messageCallback, sponsor);

            if (args.Length > 0)
            {
                if (args[0] == Include.ForceLocalID)
                {
                    forceLocalOverride = true;
                    fileLocation = args[1];
                    if (!System.IO.File.Exists(fileLocation))
                    {
                        return Convert.ToInt32(eMIDMessageLevel.Error);
                    }
                    delimiter = MIDConfigurationManager.AppSettings["Delimiter"].ToCharArray();
                }
            }

            if (forceLocalOverride)
            {
                SAB = new SessionAddressBlock(messageCallback, sponsor, MIDConfigurationManager.AppSettings["ConnectionString"], true);
            }
            else
            {
                SAB = new SessionAddressBlock(messageCallback, sponsor);
            }
            // End TT#1646-MD - JSmith - Add or Remove Hierarchy Levels

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
					return Convert.ToInt32(eMIDMessageLevel.Severe,CultureInfo.CurrentUICulture);
				}

				// Create Sessions

				try
				{
					SAB.CreateSessions((int)eServerType.Client|(int)eServerType.Hierarchy|(int)eServerType.Store|(int)eServerType.Application);
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
					MIDConfigurationManager.AppSettings["Password"], eProcesses.hierarchyLoad);

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
					return Convert.ToInt32(eMIDMessageLevel.Severe,CultureInfo.CurrentUICulture);
				}

//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//				// initialize client session to make audit available
//				SAB.ClientServerSession.Initialize();
//
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
                // Begin TT#1646-MD - JSmith - Add or Remove Hierarchy Levels
                //if (args.Length > 0)
                if (forceLocalOverride)
                {
                    delimiter = MIDConfigurationManager.AppSettings["Delimiter"].ToCharArray();
                }
                else if (args.Length > 0)
                // End TT#1646-MD - JSmith - Add or Remove Hierarchy Levels
				{
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
					if (args[0] == Include.SchedulerID)
					{
						fileLocation = args[1];
						_processId = Convert.ToInt32(args[2]);
                        // Begin TT#1054 - JSmith - Relieve Intransit not working.
                        //delimiter = ConfigurationManager.AppSettings["Delimiter"].ToCharArray();
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
				// initialize client session to make audit available
				if (_processId != Include.NoRID)
				{
					SAB.ClientServerSession.Initialize(_processId);
				}
				else
				{
					SAB.ClientServerSession.Initialize();
				}

//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
				string strCommitLimit = MIDConfigurationManager.AppSettings["CommitLimit"];
				if (strCommitLimit != null)
				{
					commitLimit = Convert.ToInt32(strCommitLimit);
				}

        //Begin TT#425 - MD - Remove Exception File logic RBeck
                //string exceptionFile = MIDConfigurationManager.AppSettings["ExceptionFile"];
                //if (exceptionFile == null)
                //{
                //exceptionFile = @".\exceptionFile.txt";
                //}
                string exceptionFile = null;
        //End   TT#425 - MD - Remove Exception File logic RBeck

				string strSetting = MIDConfigurationManager.AppSettings["AutoAddCharacteristics"];
				if (strSetting != null)
				{
					addCharacteristicGroups = Convert.ToBoolean(strSetting);
                    // Begin TT#221 - JSmith - Characteristic with pre-defined values dynamically adds new values
                    addCharacteristicValues = addCharacteristicGroups;
                    // End TT#221
				}

                // Begin TT#221 - JSmith - Characteristic with pre-defined values dynamically adds new values
                //strSetting = MIDConfigurationManager.AppSettings["AutoAddCharacteristicValues"];
                //if (strSetting != null)
                //{
                //    addCharacteristicValues = Convert.ToBoolean(strSetting);
                //}
                // End TT#221

                // Begin TT#167 - JSmith - Product Hierarchy characteristics auto add
                strSetting = MIDConfigurationManager.AppSettings["CharacteristicDelimiter"];
                if (strSetting != null)
                {
                    characteristicDelimiter = strSetting;
                }
                // End TT#167

                // Begin TT#2010 - JSmith - Split merchandise characteristics to separate delimited transaction layout
                strSetting = MIDConfigurationManager.AppSettings["UseCharacteristicTransaction"];
                if (strSetting != null)
                {
                    useCharacteristicTransaction = Convert.ToBoolean(strSetting);
                }
                // End TT#2010

				message = MIDText.GetText(eMIDTextCode.msg_BatchInputFile);

				if (fileLocation == "" || fileLocation == null)
				{
					errorFound = true;

					message = message.Replace("{0}.", "[" + fileLocation + "] NOT specified" + System.Environment.NewLine);
					message += "Hierarchy Load Process NOT run";

					// Begin Track #5035 - JSmith - file not found message level inconsistent
//					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule, true);
					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, message, sourceModule, true);
					// End Track #5035
				}
				else
				{
					if (!File.Exists(fileLocation))
					{
						errorFound = true;

						message = message.Replace("{0}.", "[" + fileLocation + "] does NOT exist" + System.Environment.NewLine);
						message += "Hierarchy Load Process NOT run";

						SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule, true);
					}
					else
					{
						FileInfo txnFileInfo = new FileInfo(fileLocation);

						if (txnFileInfo.Length == 0)
						{
							errorFound = true;

							message = message.Replace("{0}.", "[" + fileLocation + "] is an empty file" + System.Environment.NewLine);
							message += "Hierarchy Load Process NOT run";

							// Begin Track #5035 - JSmith - file not found message level inconsistent
//							SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule, true);
							SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, message, sourceModule, true);
							// End Track #5035
						}
						else
						{
							message = message.Replace("{0}", "[" + fileLocation + "]");

							SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule, true);

							if (!errorFound)
							{
                                //SAB.HierarchyServerSession.Initialize();  // TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
								SAB.ApplicationServerSession.Initialize();
								SAB.StoreServerSession.Initialize();
                                // Begin TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
                                // StoreServerSession must be initialized before HierarchyServerSession 
                                SAB.HierarchyServerSession.Initialize();
                                // End TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.

								if (fileLocation.Substring(fileLocation.Length - 4).ToUpper() == ".XML")
								{
									errorFound = SAB.HierarchyServerSession.LoadXMLTransFile(SAB, fileLocation, commitLimit, exceptionFile, addCharacteristicGroups, addCharacteristicValues);
								}
								else
								{
                                    // Begin TT#167 - JSmith - Product Hierarchy characteristics auto add
                                    //errorFound = SAB.HierarchyServerSession.LoadDelimitedTransFile(SAB, fileLocation, delimiter, commitLimit, exceptionFile, addCharacteristicGroups, addCharacteristicValues);
                                    // Begin TT#2010 - JSmith - Split merchandise characteristics to separate delimited transaction layout
                                    //errorFound = SAB.HierarchyServerSession.LoadDelimitedTransFile(SAB, fileLocation, delimiter, commitLimit, exceptionFile, addCharacteristicGroups, addCharacteristicValues, characteristicDelimiter);
                                    errorFound = SAB.HierarchyServerSession.LoadDelimitedTransFile(SAB, fileLocation, delimiter, commitLimit, exceptionFile, addCharacteristicGroups, addCharacteristicValues, characteristicDelimiter, useCharacteristicTransaction);
                                    // End TT#2010
                                    // End TT#167
								}
							}
						}
					}
				}
// (CSMITH) - END MID Track #2979
			}

			catch (XMLHierarchyLoadProcessException XMLhlpe)
			{
				errorFound = true;
				SAB.ClientServerSession.Audit.Log_Exception(XMLhlpe, sourceModule);
			}
			catch ( Exception ex )
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
                            //BEGIN TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                            //SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                            SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                            //END TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
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
