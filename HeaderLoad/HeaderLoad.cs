using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
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

namespace MIDRetail.HeaderLoad
{
	/// <summary>
	/// Summary description for HeaderLoad.
	/// </summary>
	class HeaderLoad
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args)
		{
			char[] delimiter = {'~'};

			bool hdrErrorFound = false;
			// BEGIN MID Track #3595 - Update Style Description
			bool updateStyleDescription = false;
			// END MID Trac #3595
			string userId = null;
			string passWd = null;
			string msgText = null;
			string sveMsgText = null;
			string hdrTransFile = null;
			string optDelimiter = null;
			string eventLogID = "MIDHeaderLoad";
			string sourceModule = "HeaderLoad.cs";
			eMIDMessageLevel highestMessage;
			ArrayList files = new ArrayList();
			string endSuffix = null;
			Hashtable processedFiles = new Hashtable();
			bool runUntil = false;
			DirectoryInfo directoryInfo = null;;
			string wildcard = null;
			// BEGIN MID Track #4264 - Create header on modify if not found
			bool createOnModify = false;
			// END MID Track #4264
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
			int _processId = Include.NoRID;
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
            bool _autoAddCharacteristics = false;    // TT#168 - RMatelic - Header characteristics auto add
			// Begin TT#1581-MD - stodd - Header Reconcile API
            int _headerIdSequenceLength = 5;
            string _headerIdDelimiter = "-";
            bool _generateHeaderID = false;
            List<string> _headerKeysToMatchList = new List<string>();
            List<string> _headerIdKeysList = new List<string>();
			// End TT#1581-MD - stodd - Header Reconcile API
            // Begin TT#1966-MD - JSmith - DC Fulfillment
            List<string> _masterHeaderIdKeysList = new List<string>();
            // End TT#1966-MD - JSmith - DC Fulfillment
			
            //Hashtable port;

			HeaderLoadProcess hlp;

			SessionSponsor sponsor;

			SessionAddressBlock SAB;

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
					// Begin TT#1040 - MD - stodd - header load API for Group Allocation 
                    SAB.CreateSessions((int)eServerType.Client | (int)eServerType.Application | (int)eServerType.Hierarchy | (int)eServerType.Store | (int)eServerType.Header);
					// End TT#1040 - MD - stodd - header load API for Group Allocation 
				}

				catch (Exception Ex)
				{
					hdrErrorFound = true;

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

                // FOR TESTING PROCESS CONTROL
                //string reason = "";
                //SAB.ControlServerSession.GetProcessingPermission(eProcesses.HeaderReconcile, 1010192, ref reason);
                //SAB.ControlServerSession.SetProcessState(eProcesses.headerLoad, 1010192, false);

				eSecurityAuthenticate authentication = SAB.ClientServerSession.UserLogin(userId, passWd, eProcesses.headerLoad);

				// Begin TT#1581-MD - stodd Header Reconcile
                if (authentication == eSecurityAuthenticate.Unavailable)
                {
                    hdrErrorFound = true;   // TT#1663-MD - stodd - Header Load and Header Reconcile should return "Severe" if stopped from executing by process control rules
                    return Convert.ToInt32(eMIDMessageLevel.Severe);    // TT#1663-MD - stodd - Header Load and Header Reconcile should return "Severe" if stopped from executing by process control rules
                }
				// End TT#1581-MD - stodd Header Reconcile
				if (authentication != eSecurityAuthenticate.UserAuthenticated)
				{
					EventLog.WriteEntry(eventLogID, "Unable to log in with user: [" + userId + "] password: [" + passWd + "]", EventLogEntryType.Error);

					System.Console.Write("Unable to log in with user: [" + userId + "] password: [" + passWd + "]");

					return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
				}

//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//				// =================================================
//				// Initialize client session to make audit available
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
						hdrTransFile = args[1];
						_processId = Convert.ToInt32(args[2]);
                        // Begin TT#1054 - JSmith - Relieve Intransit not working.
                        //endSuffix = ConfigurationSettings.AppSettings["EndSuffix"];
                        endSuffix = MIDConfigurationManager.AppSettings["EndSuffix"];
                        // End TT#1054
					}
					else
					{
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
						hdrTransFile = args[0];

						if (args.Length > 1)
						{
							endSuffix = args[1].Trim();
							if (endSuffix.Length > 0)
							{
								runUntil = true;
							}
						}
						else
						{
							endSuffix = MIDConfigurationManager.AppSettings["EndSuffix"];
						}
//						if (args.Length > 1)
//						{
//							delimiter = args[1].ToCharArray();
//						}
//						else
//						{
//							optDelimiter = MIDConfigurationManager.AppSettings["Delimiter"];
//
//							if (optDelimiter != "" && optDelimiter != null)
//							{
//								delimiter = optDelimiter.ToCharArray();
//							}
//						}
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
					}
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
				}
				else
				{
					hdrTransFile = MIDConfigurationManager.AppSettings["InputFile"];

					optDelimiter = MIDConfigurationManager.AppSettings["Delimiter"];

					if (optDelimiter != "" && optDelimiter != null)
					{
						delimiter = optDelimiter.ToCharArray();
					}

                    endSuffix = MIDConfigurationManager.AppSettings["EndSuffix"];
				}

//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
				// =================================================
				// Initialize client session to make audit available
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
				// BEGIN MID Track #3595 - Update Style Description
				string strUpdateStyleDesc = MIDConfigurationManager.AppSettings["UpdateStyleDescription"];
				if (strUpdateStyleDesc != null)
				{
					try
					{
						updateStyleDescription = Convert.ToBoolean(strUpdateStyleDesc);
					}
					catch
					{
						updateStyleDescription = false;
					}
				}
				// END MID Trac #3595

				// BEGIN MID Track #4264 - Create header on modify if not found
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
				// END MID Track #4264

                // Begin TT#168 - RMatelic - Header characteristics auto add
                string strAutoAddCharacteristics = MIDConfigurationManager.AppSettings["AutoAddCharacteristics"];
                if (strAutoAddCharacteristics != null)
                {
                    try
                    {
                        _autoAddCharacteristics = Convert.ToBoolean(strAutoAddCharacteristics);
                    }
                    catch
                    {
                        _autoAddCharacteristics = false;
                    }
                }
                // End TT#168

				// Begin TT#1581-MD - stodd - Header Reconcile API
                string strHeaderIdSequenceLength = MIDConfigurationManager.AppSettings["HeaderIdSequenceLength"];
                if (strHeaderIdSequenceLength != null)
                {
                    try
                    {
                        _headerIdSequenceLength = Convert.ToInt32(strHeaderIdSequenceLength);
                    }
                    catch
                    {
                        _headerIdSequenceLength = 7;    // Default is 7
                    }
                }

                string headerProcessingKeysFile;
                string strHeaderProcessingKeysFile = MIDConfigurationManager.AppSettings["HeaderProcessingKeysFile"];
                if (strHeaderProcessingKeysFile == null)
                {
                    headerProcessingKeysFile = string.Empty;
                }
                else
                {
                    headerProcessingKeysFile = strHeaderProcessingKeysFile.ToString().Trim();
                }

                string headerIdDelimiter = MIDConfigurationManager.AppSettings["HeaderIdDelimiter"];
                if (headerIdDelimiter != null)
                {
                    try
                    {
                        _headerIdDelimiter = headerIdDelimiter;
                    }
                    catch
                    {
                        _headerIdDelimiter = "-";    // Default is dash
                    }
                }

                string strGenerateHeaderID = MIDConfigurationManager.AppSettings["GenerateHeaderID"];
                if (strGenerateHeaderID != null)
                {
                    try
                    {
                        _generateHeaderID = bool.Parse(strGenerateHeaderID);
                    }
                    catch
                    {
                        _generateHeaderID = false;    // Default is false
                    }
                }

                // End TT#1581-MD - stodd - Header Reconcile API


			
				// ===================
				// Initialize sessions
				// ===================
                SAB.ControlServerSession.Initialize();   // TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed

				SAB.ApplicationServerSession.Initialize();

                //SAB.HierarchyServerSession.Initialize();  // TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.

				SAB.StoreServerSession.Initialize();

                // Begin TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
                // StoreServerSession must be initialized before HierarchyServerSession 
                SAB.HierarchyServerSession.Initialize();
                // End TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.

                // Begin TT#1581-MD - stodd - Header Reconcile API
                if (_generateHeaderID)
                {
                    //=============================================================================================
                    // Loads the _headerKeysToMatch list using the data found in the headerProcessingKeysFile
                    // Loads the _headerIdKeys list using the data found in the headerProcessingKeysFile
                    //=============================================================================================
                    // Begin TT#1966-MD - JSmith - DC Fulfillment
                    //if (!HeaderKeys.LoadKeys(headerProcessingKeysFile, ref _headerKeysToMatchList, ref _headerIdKeysList, ref msgText))
                    if (!SAB.ControlServerSession.LoadHeaderKeys(ref _headerIdKeysList, ref _masterHeaderIdKeysList, ref _headerKeysToMatchList, ref msgText))
                    // End TT#1966-MD - JSmith - DC Fulfillment
                    {
                        // Event Viewer
                        EventLog.WriteEntry(eventLogID, msgText, EventLogEntryType.Error);
                        System.Console.Write(msgText);
                        // Audit
                        SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msgText, sourceModule);
                        SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
                        return Convert.ToInt32(eMIDMessageLevel.Severe);
                    }

                    //=======================
                    // Log processing parms
                    //=======================
                    LogProcessingParms(sourceModule, _headerIdSequenceLength, _headerIdDelimiter, _generateHeaderID, _headerKeysToMatchList, _headerIdKeysList, SAB);
                }
                // End TT#1581-MD - stodd - Header Reconcile API

				// ===========================
				// Does transaction file exist
				// ===========================
				sveMsgText = MIDText.GetText(eMIDTextCode.msg_BatchInputFile);

				if (hdrTransFile == "" || hdrTransFile == null)
				{
					msgText = sveMsgText.Replace("{0}.", "[" + hdrTransFile + "] NOT specified" + System.Environment.NewLine);
					msgText += "Header Load Process NOT run";

					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msgText, sourceModule, true);

					SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_InputFileNotFound, "", SAB.GetHighestAuditMessageLevel());

					return Convert.ToInt32(SAB.GetHighestAuditMessageLevel(), CultureInfo.CurrentUICulture);
				}
				else
				{
					if (hdrTransFile.IndexOf("*") > -1)
					{
						int index = hdrTransFile.LastIndexOf(@"\");
						string directory = hdrTransFile.Substring(0, index);
						directoryInfo = new DirectoryInfo(directory);
						if (directoryInfo.Exists)
						{
							wildcard = hdrTransFile.Substring(index + 1, hdrTransFile.Length - index - 1);
//							FileInfo[] shortcutFiles;
//							shortcutFiles = directoryInfo.GetFiles(wildcard);
//							foreach (FileInfo file in shortcutFiles)
//							{
//								files.Add(file.DirectoryName + @"\" + file.Name);
//							}
							files = GetFiles(directoryInfo, wildcard, processedFiles);
							if (endSuffix != null)
							{
								runUntil = true;
							}
						}
						else
						{
							msgText = sveMsgText.Replace("{0}.", "[" + directory + "] does NOT exist" + System.Environment.NewLine);
							msgText += "Header Load Process NOT run";

							SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msgText, sourceModule, true);

							SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_InputFileNotFound, "", SAB.GetHighestAuditMessageLevel());
							
							return Convert.ToInt32(SAB.GetHighestAuditMessageLevel(), CultureInfo.CurrentUICulture);
						}
					}
					else
					{
						files.Add(hdrTransFile);
						runUntil = false;
					}
				}

				if (!hdrErrorFound)
				{
					// BEGIN MID Track #4264 - Create header on modify if not found
//					hlp = new HeaderLoadProcess(SAB, ref hdrErrorFound);
                    // Begin TT#168 - RMatelic - Header characteristics auto add
                    //hlp = new HeaderLoadProcess(SAB, ref hdrErrorFound, createOnModify);
					// END MID Track #4264
					// Begin TT#1581-MD - stodd - Header Reconcile API
                    hlp = new HeaderLoadProcess(SAB, ref hdrErrorFound, createOnModify, _autoAddCharacteristics, _headerIdSequenceLength, _headerIdKeysList, _headerKeysToMatchList, _headerIdDelimiter, _generateHeaderID);
					// End TT#1581-MD - stodd - Header Reconcile API
                    // End TT#168 

					bool loop = true;
					while (loop)
					{
						foreach (string transFile in files)
						{
							if (transFile.Substring(transFile.Length - 4).ToUpper() != ".XML")
							{
								msgText = sveMsgText.Replace("{0}.", "[" + transFile + "] does NOT exist" + System.Environment.NewLine);
								msgText += "Header Load Process NOT run";

								// Begin Track #5035 - JSmith - file not found message level inconsistent
//								SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msgText, sourceModule, true);
								SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msgText, sourceModule, true);
								// End Track #5035
							}
							else
							{
								if (!File.Exists(transFile))
								{
									msgText = sveMsgText.Replace("{0}.", "[" + transFile + "] does NOT exist" + System.Environment.NewLine);
									msgText += "Header Load Process NOT run";

									SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msgText, sourceModule, true);
								}
								else
								{
									FileInfo hdrFileInfo = new FileInfo(transFile);

									if (transFile.Length == 0)
									{
										msgText = sveMsgText.Replace("{0}.", "[" + transFile + "] is an empty file" + System.Environment.NewLine);
										msgText += "Header Load Process NOT run";

										// Begin Track #5035 - JSmith - file not found message level inconsistent
//										SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msgText, sourceModule, true);
										SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, msgText, sourceModule, true);
										// End Track #5035
									}
								}
							}

							// ====================
							// Process transactions
							// ====================
							msgText = sveMsgText.Replace("{0}", "[" + transFile + "]");

							SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, sourceModule, true);

							try
							{
								bool fileErrorFound = false;
								hlp.SerializeVariableFile(transFile, updateStyleDescription, ref fileErrorFound);
								if (fileErrorFound)
								{
									hdrErrorFound = true;
								}
							}
							catch (Exception Ex)
							{
								hdrErrorFound = true;
								SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, Ex.ToString(), sourceModule);
							}
						}

						if (runUntil)
						{
							System.Threading.Thread.Sleep(2000);
							files = GetFiles(directoryInfo, wildcard, processedFiles);
							if (EndOfFiles(directoryInfo, "*." + endSuffix))
							{
								runUntil = false;
							}
						}
						else
						{
							loop = false;
						}
					}
					hlp.WriteRecordCounts();
				}
			}

			catch (Exception Ex)
			{
				hdrErrorFound = true;

				SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, Ex.ToString(), sourceModule);

			}

			finally
			{
                //BEGIN TT#1681 - MD - DOConnell - Header Load API is removing itself from the Control Server prematurely
                //Process currentProcess = Process.GetCurrentProcess();
                //SAB.ControlServerSession.SetProcessState(eProcesses.headerLoad, currentProcess.Id, false);
                //END TT#1681 - MD - DOConnell - Header Load API is removing itself from the Control Server prematurely

				if (SAB.ClientServerSession != null && SAB.ClientServerSession.Audit != null)
				{
					if (!hdrErrorFound)
					{
						SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
					}
					else
					{
                        // Begin TT#1663-MD - stodd - Header Load and Header Reconcile should return "Severe" if stopped from executing by process control rules
                        //SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel()); 
                        SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                        // End TT#1663-MD - stodd - Header Load and Header Reconcile should return "Severe" if stopped from executing by process control rules
					}
				}

				highestMessage = SAB.CloseSessions();
			}

			return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
		}

        // Begin TT#1581-MD - stodd - Header Reconcile API
        private static void LogProcessingParms(string sourceModule, int _headerIdSequenceLength, string _headerIdDelimiter, bool _generateHeaderID, List<string> _headerKeysToMatchList, List<string> _headerIdKeysList, SessionAddressBlock SAB)
        {
            string headerKeys = string.Empty;
            _headerKeysToMatchList.ForEach(i => headerKeys += i + ", ");
            headerKeys = headerKeys.TrimEnd(new char[] { ',', ' ' });

            string headerIdKeys = string.Empty;
            _headerIdKeysList.ForEach(i => headerIdKeys += i + ", ");
            headerIdKeys = headerIdKeys.TrimEnd(new char[] { ',', ' ' });

            SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Generate Header ID: " + _generateHeaderID, sourceModule, true);
            if (_generateHeaderID)
            {
                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Header Matching Keys: " + headerKeys, sourceModule, true);
                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Header ID Sequence Length: " + _headerIdSequenceLength, sourceModule, true);
                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Header ID Delimiter: " + _headerIdDelimiter, sourceModule, true);
                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Generate Header ID Keys: " + headerIdKeys, sourceModule, true);
            }
        }
        // End TT#1581-MD - stodd - Header Reconcile API


		static ArrayList GetFiles(DirectoryInfo aDirectoryInfo, string aFileName, Hashtable aProcessedFiles)
		{
			try
			{
				// BEGIN TT#1766 - stodd - FIFO processing
				eAPIFileProcessingDirection direction = eAPIFileProcessingDirection.Default;
				string fileProcessingDirection = MIDConfigurationManager.AppSettings["APIFileProcessingDirection"];
				if (fileProcessingDirection == null)
				{
					direction = eAPIFileProcessingDirection.Default;
				}
				else
				{
					switch (fileProcessingDirection.ToUpper().Trim())
					{
						case "FIFO":
							direction = eAPIFileProcessingDirection.FIFO;
							break;
						case "FILO":
							direction = eAPIFileProcessingDirection.FILO;
							break;
						default:
							direction = eAPIFileProcessingDirection.Default;
							break;
					}
				}
				// END TT#1766 - stodd - FIFO processing
				ArrayList files = new ArrayList();
				FileInfo[] dirFiles = aDirectoryInfo.GetFiles(aFileName);

				switch (direction)
				{
					case eAPIFileProcessingDirection.FIFO:
						Array.Sort(dirFiles, new clsCompareFileInfoFIFO());
						break;
					case eAPIFileProcessingDirection.FILO:
						Array.Sort(dirFiles, new clsCompareFileInfoFILO());
						break;
					default:
						break;
				}

				foreach (FileInfo file in dirFiles)
				{
					string fileName = file.DirectoryName + @"\" + file.Name;
					if (!aProcessedFiles.Contains(fileName) &&
						fileName.IndexOf(".out.xml") < 0)
					{
						// BEGIN Track #4215 - JSmith - Check file for sharing issues
						// If the file can be opened for exclusive access it means that the file
						// is not locked by another program
						FileStream inputStream = null;
						bool fileAvailable = true;
						try
						{
							inputStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
						}
						catch (IOException)
						{
							fileAvailable = false;
						}
						
						if (fileAvailable)
						{
							// close the file if it was opened and add to the list to process
							inputStream.Close();
							files.Add(fileName);
							aProcessedFiles.Add(fileName, null);
						}
						// END Track #4215
					}
				}
				return files;
			}
			catch
			{
				throw;
			}
		}

		static bool EndOfFiles(DirectoryInfo aDirectoryInfo, string aFileName)
		{
			try
			{
				bool endOfFiles = false;
				FileInfo[] dirFiles = aDirectoryInfo.GetFiles(aFileName);
				foreach (FileInfo file in dirFiles)
				{
					string fileName = file.DirectoryName + @"\" + file.Name;
					System.IO.File.Delete(fileName);
					endOfFiles = true;
				}
				return endOfFiles;
			}
			catch
			{
				throw;
			}
		}
	}
}
