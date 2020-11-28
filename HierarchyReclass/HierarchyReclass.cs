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
using MIDRetail.Data;   //TT#3894-VStuart-Alternate Reclass Failure-Oakley

namespace MIDRetail.HierarchyReclass
{
	class HierarchyReclass
	{
		const string cDelimiter = "~";
		const int cMaxRecsPerFile = 30000;

		static string _inputDirectory;
		static string _outputDirectory;
		static string _connectionString;
		static string _delimiter;
		static string _triggerSuffix;
        // Begin TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files
        static string _reclassMoveTriggerSuffix;
        static string _reclassDeleteTriggerSuffix;
        static string _reclassRenameTriggerSuffix;
        // End TT#2186
		static int _maxRecsPerFile;
		static bool _allowDeletes;
        // Begin TT#223 MD - JSmith - Add file order processsing (FIFO, LIFO)
        static eAPIFileProcessingDirection _fileProcessingDirection = eAPIFileProcessingDirection.FIFO;
        // End TT#223 MD

// Begin TT#228 - RBeck - Hierarchy Reclass message incorrectly displayed  
        private Audit _audit = null;
// End TT#228 */

  
		static int Main(string[] args)
		{
			string sourceModule = "HierarchyReclass.cs";
			string eventLogID = "MIDHierarchyReclass";
			IMessageCallback messageCallback;
			SessionSponsor sponsor;
			SessionAddressBlock SAB;
			System.Runtime.Remoting.Channels.IChannel channel;
			eSecurityAuthenticate authentication;

//Begin TT228 - RBeck - Hierarchy Reclass message incorrectly displayed  
           int _success = 0;
//End   TT228 - RBeck - Hierarchy Reclass message incorrectly displayed
  
			string tmpSuffix;
			string outSuffix;
			string[] inFiles;
			HierarchyReclassProcess _relcassProc;

			Exception innerE;

            bool forceLocalOverride = false; // TT#1646-MD - JSmith - Add or Remove Hierarchy Levels

			try
			{
				messageCallback = new BatchMessageCallback();
				sponsor = new SessionSponsor();
				authentication = eSecurityAuthenticate.UnknownUser;
                // Begin TT#1646-MD - JSmith - Add or Remove Hierarchy Levels
                //SAB = new SessionAddressBlock(messageCallback, sponsor);
                if (args.Length > 0)
                {
                    if (args[0] == Include.ForceLocalID)
                    {
                        forceLocalOverride = true;
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
						throw new Exception("Error opening port #0 " + ex.Message);
					}

					// Create Sessions

					try
					{
						SAB.CreateSessions((int)eServerType.Client);
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

					authentication = SAB.ClientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"],
						MIDConfigurationManager.AppSettings["Password"], eProcesses.hierarchyReclass);

                    //BEGIN TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID
                    //BEGIN TT#1644 - MD- DOConnell - Process Control
                    if (authentication == eSecurityAuthenticate.Unavailable)
                    {
                        //return Convert.ToInt32(eMIDMessageLevel.ProcessUnavailable);
                        //BEGIN TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                        SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                        //END TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
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

					SAB.ClientServerSession.Initialize();

					// Load application settings and override with command line arguments

					LoadAppSettings();
                    // Begin TT#1646-MD - JSmith - Add or Remove Hierarchy Levels
                    if (!forceLocalOverride)
                    {
                        // End TT#1646-MD - JSmith - Add or Remove Hierarchy Levels
                        if (args.Length <= 3)
                        {
                            if (args.Length > 0)
                            {
                                _inputDirectory = args[0];
                            }

                            if (args.Length > 1)
                            {
                                _outputDirectory = args[1];
                            }

                            if (args.Length > 2)
                            {
                                _triggerSuffix = "." + args[2].Trim('.', ' ');
                            }
                        }
                        else
                        {
                            throw new Exception("Usage: HierarchyReclass [<Input Directory> [<Output Directory> [<Trigger Suffix>]]]");
                        }
                        // Begin TT#1646-MD - JSmith - Add or Remove Hierarchy Levels
                    }
                    // End TT#1646-MD - JSmith - Add or Remove Hierarchy Levels

					// Check application settings for validity

					if (_inputDirectory == string.Empty)
					{
						throw new Exception("No Input Directory specified");
					}

					if (_outputDirectory == string.Empty)
					{
						throw new Exception("No Output Directory specified");
					}

					if (!Directory.Exists(_inputDirectory))
					{
						throw new Exception("Input directory does not exist or is not specified correctly: " + _inputDirectory);
					}

					if (!Directory.Exists(_outputDirectory))
					{
						throw new Exception("Output directory does not exist or is not specified correctly: " + _outputDirectory);
					}

                    // Begin TT#223 MD - JSmith - Add file order processsing (FIFO, LIFO)
                    //if (_triggerSuffix != string.Empty)
                    //{
                    //    inFiles = Directory.GetFiles(_inputDirectory, "*" + _triggerSuffix);
                    //}
                    //else
                    //{
                    //    inFiles = Directory.GetFiles(_inputDirectory);
                    //}

                    //outSuffix = string.Empty;

                    //// Check to make sure all input files are of the same type and have the same extension

                    //foreach (string inFile in inFiles)
                    //{
                    //    tmpSuffix = GetFileSuffix(inFile);

                    //    if (outSuffix == string.Empty)
                    //    {
                    //        outSuffix = tmpSuffix;
                    //    }
                    //    else
                    //    {
                    //        if (tmpSuffix != outSuffix)
                    //        {
                    //            throw new Exception("Input files must be of same type (XML or CSV), and must have the same suffix");
                    //        }
                    //    }
                    //}

                    // Begin TT#4077 - JSmith - Valid transaction trigger file formats error
                    ////BEGIN TT#3894-VStuart-Alternate Reclass Failure-Oakley
                    //FileInfo[] dirFilesChk;
                    //DirectoryInfo directoryInfo = new DirectoryInfo(_inputDirectory);
                    //if (_triggerSuffix != string.Empty)
                    //{
                    //    dirFilesChk = directoryInfo.GetFiles("*" + _triggerSuffix);
                    //}
                    //else
                    //{
                    //    dirFilesChk = directoryInfo.GetFiles();
                    //}

                    ////Check to make sure each trigger file has three components; filename, extension and trigger exstension.
                    //foreach (FileInfo aFileInfo in dirFilesChk)
                    //{
                    //    string message;
                    //    string[] fparts = aFileInfo.ToString().Split('.');
                    //        if (fparts.Length != 3)
                    //        {
                    //            string[] errParms = new string[1];
                    //            errParms.SetValue(aFileInfo.ToString(), 0);
                    //            message = MIDText.GetText(eMIDTextCode.msg_InvalidReclassTriggerFileName, errParms);
                    //            SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, message, sourceModule);
                    //            //BEGIN TT#3894-VStuart-Alternate Reclass Failure-Oakley
                    //            //SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_InvalidNumberOfParameters, "", SAB.GetHighestAuditMessageLevel());

                    //            //return Convert.ToInt32(SAB.CloseSessions(), CultureInfo.CurrentUICulture);
                    //            //END TT#3894-VStuart-Alternate Reclass Failure-Oakley
                    //        }
                    //}
                    ////END TT#3894-VStuart-Alternate Reclass Failure-Oakley

                    ////BEGIN TT#3894-VStuart-Alternate Reclass Failure-Oakley
                    //FileInfo[] dirFiles;
                    ////DirectoryInfo directoryInfo = new DirectoryInfo(_inputDirectory);
                    //if (_triggerSuffix != string.Empty)
                    //{
                    //    dirFiles = directoryInfo.GetFiles("*" + "." + "*" + _triggerSuffix);
                    //}
                    //else
                    //{
                    //    dirFiles = directoryInfo.GetFiles();
                    //}
                    ////END TT#3894-VStuart-Alternate Reclass Failure-Oakley

                    bool blFileErrorFound = false;
                    ArrayList alFiles = new ArrayList();
                    FileInfo[] dirFiles;
                    DirectoryInfo directoryInfo = new DirectoryInfo(_inputDirectory);
                    if (_triggerSuffix != string.Empty)
                    {
                        dirFiles = directoryInfo.GetFiles("*" + _triggerSuffix);
                    }
                    else
                    {
                        dirFiles = directoryInfo.GetFiles();
                    }

                    eMIDMessageLevel messageLevel = eMIDMessageLevel.Warning;
                    if (_allowDeletes)
                    {
                        messageLevel = eMIDMessageLevel.Error;
                    }
                    string dataFileName;
                    foreach (FileInfo aFileInfo in dirFiles)
                    {
                        string message;
                        dataFileName = Path.GetFileNameWithoutExtension(aFileInfo.FullName);
                        if (File.Exists(aFileInfo.DirectoryName + @"\" + dataFileName) ||
                            _triggerSuffix == string.Empty)
                        {
                            alFiles.Add(aFileInfo);
                        }
                        else
                        {
                            string[] errParms = new string[1];
                            errParms.SetValue(aFileInfo.ToString(), 0);
                            message = "File Name=" + aFileInfo.FullName;
                            SAB.ClientServerSession.Audit.Add_Msg(messageLevel, eMIDTextCode.msg_InvalidReclassTriggerFileName, message, sourceModule);
                            blFileErrorFound = true;
                        }
                    }

                    if (blFileErrorFound &&
                        _allowDeletes)
                    {
                        throw new Exception("AllowDeletes is enabled with trigger files and transaction files not paired.  Processing terminated.");
                    }

                    dirFiles = new FileInfo[alFiles.Count];
                    alFiles.CopyTo(0, dirFiles, 0, alFiles.Count);
                    // End TT#4077 - JSmith - Valid transaction trigger file formats error

                    switch (_fileProcessingDirection)
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


					outSuffix = string.Empty;
                    inFiles = new string[dirFiles.GetLength(0)];
                    int index = 0;

					// Check to make sure all input files are of the same type and have the same extension

                    foreach (FileInfo file in dirFiles)
					{
                        string inFile = file.DirectoryName + @"\" + file.Name;

                        // Begin TT#223 - JSmith - Add file order processsing (FIFO, LIFO)
                        string msg = "File=" + inFile + ";LastWriteTime=" + file.LastWriteTime.ToString();
                        SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Debug, msg, sourceModule);
                        // End TT#223 - JSmith - Add file order processsing (FIFO, LIFO)

                        inFiles[index] = inFile;
						tmpSuffix = GetFileSuffix(inFile);

						if (outSuffix == string.Empty)
						{
							outSuffix = tmpSuffix;
						}
						else
						{
							if (tmpSuffix != outSuffix)
							{
								throw new Exception("Input files must be of same type (XML or CSV), and must have the same suffix");
							}
						}
                        ++index;
					}
                    // End TT#223 MD

					if (outSuffix == ".xml")
					{
						outSuffix = ".txt";
					}

					// Create processing class and initialize

					_relcassProc = new HierarchyReclassProcess(
						SAB,
						Thread.CurrentThread.GetHashCode(),
						_delimiter,
						_triggerSuffix,
                        // Begin TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files
                        _reclassMoveTriggerSuffix,
                        _reclassDeleteTriggerSuffix,
                        _reclassRenameTriggerSuffix,
                        // End TT#2186
						outSuffix,
						_outputDirectory,
						_maxRecsPerFile,
						_allowDeletes);

					_relcassProc.Initialize();

					try
					{
						_relcassProc.ClearTransactionTable();

						// Read and load input files

						foreach (string inFile in inFiles)
						{
							if (GetFileSuffix(inFile) == ".xml")
							{
								_relcassProc.LoadXMLTransactionFile(inFile.Substring(0, inFile.Length - _triggerSuffix.Length));
							}
							else
							{
								_relcassProc.LoadDelimitedTransactionFile(inFile.Substring(0, inFile.Length - _triggerSuffix.Length));
							}

							if (_triggerSuffix.Length > 0)
							{
								System.IO.File.Delete(inFile);
							}
						}

						// Process transactions

						_relcassProc.ProcessTransactions();
					}
					catch (Exception exc)
					{
						SAB.ClientServerSession.Audit.Log_Exception(exc, sourceModule);
						throw;
					}
					finally
					{
						_relcassProc.ClearTransactionTable();
						_relcassProc.Cleanup();

					}

                    //BEGIN TT#3900-VStuart-Audit is not accurate-MID
                    //SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Hierarchy Transactions Written: " + _relcassProc.NumHierarchyRecs, sourceModule, true);
                    //SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Add/Change Transactions Written: " + _relcassProc.NumAddUpdateRecs, sourceModule, true);
                    //SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Delete Transactions Written: " + _relcassProc.NumDeleteRecs, sourceModule, true);
                    //SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Move Transactions Written: " + _relcassProc.NumEditRecs, sourceModule, true);
                    //SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Transactions Rejected: " + _relcassProc.NumRejectedRecs, sourceModule, true);
                    //END TT#3900-VStuart-Audit is not accurate-MID

					if (_relcassProc.NumRejectedRecs == 0)
					{
						SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Processing completed successfully.", sourceModule);
                        _success = 1;
                    }
					else
					{
						SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, "Processing completed with Transaction errors.", sourceModule);
                        
                    }

					SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
                     
                    //Begin TT228 - RBeck - Hierarchy Reclass message incorrectly displayed  
                    SAB.ClientServerSession.Audit.HierarchyReclassAuditInfo_Add(  _relcassProc.NumHierarchyRecs
                                                                                    , _relcassProc.NumAddUpdateRecs
                                                                                    , _relcassProc.NumDeleteRecs
                                                                                    , _relcassProc.NumEditRecs
                                                                                    , _relcassProc.NumRejectedRecs
                                                                                   );
                    //End  TT228 - RBeck - Hierarchy Reclass message incorrectly displayed  
					return Convert.ToInt32(SAB.CloseSessions(), CultureInfo.CurrentUICulture);
				}
				catch (Exception exc)
				{
					SAB.ClientServerSession.Audit.Log_Exception(exc, sourceModule);
					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Processing terminated due to errors.", sourceModule);

					SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());

					return Convert.ToInt32(SAB.CloseSessions(), CultureInfo.CurrentUICulture);
				}
			}
			catch (Exception exc)
			{
				EventLog.WriteEntry(eventLogID, "Exception encountered: " + exc.Message, EventLogEntryType.Error);
				return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
			}

		}

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

                appSetting = MIDConfigurationManager.AppSettings["Delimiter"];

				if (appSetting == null)
				{
					_delimiter = cDelimiter;
				}
				else
				{
					_delimiter = appSetting;
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

                // Begin TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files
                appSetting = MIDConfigurationManager.AppSettings["ReclassMoveTriggerSuffix"];

				if (appSetting == null)
				{
					_reclassMoveTriggerSuffix = _triggerSuffix;
				}
				else
				{
                    _reclassMoveTriggerSuffix = "." + appSetting.Trim('.', ' ');
				}

                appSetting = MIDConfigurationManager.AppSettings["ReclassDeleteTriggerSuffix"];

                if (appSetting == null)
                {
                    _reclassDeleteTriggerSuffix = _triggerSuffix;
                }
                else
                {
                    _reclassDeleteTriggerSuffix = "." + appSetting.Trim('.', ' ');
                }

                appSetting = MIDConfigurationManager.AppSettings["ReclassRenameTriggerSuffix"];

                if (appSetting == null)
                {
                    _reclassRenameTriggerSuffix = _triggerSuffix;
                }
                else
                {
                    _reclassRenameTriggerSuffix = "." + appSetting.Trim('.', ' ');
                }
                // End TT#2186

                appSetting = MIDConfigurationManager.AppSettings["MaximumRecordsPerOutputFile"];

				if (appSetting == null)
				{
					_maxRecsPerFile = cMaxRecsPerFile;
				}
				else
				{
					_maxRecsPerFile = Convert.ToInt32(appSetting);
				}

                appSetting = MIDConfigurationManager.AppSettings["AllowDeletes"];

				if (appSetting == null)
				{
					_allowDeletes = true;
				}
				else
				{
					_allowDeletes = Convert.ToBoolean(appSetting);
				}

                // Begin TT#223 MD - JSmith - Add file order processsing (FIFO, LIFO)
                appSetting = MIDConfigurationManager.AppSettings["APIFileProcessingDirection"];
                if (appSetting == null)
                {
                    _fileProcessingDirection = eAPIFileProcessingDirection.FIFO;
                }
                else
                {
                    switch (appSetting.ToUpper().Trim())
                    {
                        case "FIFO":
                            _fileProcessingDirection = eAPIFileProcessingDirection.FIFO;
                            break;
                        case "FILO":
                            _fileProcessingDirection = eAPIFileProcessingDirection.FILO;
                            break;
                        default:
                            _fileProcessingDirection = eAPIFileProcessingDirection.Default;
                            break;
                    }
                }
                // End TT#223 MD
			}
			catch
			{
				throw;
			}
		}

		static string GetFileSuffix(string aFilename)
		{
			string suffix;
            // Begin TT#1913 - JSmith - HierarchyReclass.exe failing when we attempt to execute
            string filename;
            // End TT#1913

			try
			{
                // Begin TT#1913 - JSmith - HierarchyReclass.exe failing when we attempt to execute
                //suffix = aFilename.Substring(aFilename.IndexOf('.'));
                //return suffix.Substring(0, suffix.Length - _triggerSuffix.Length).ToLower();
                filename = aFilename.Substring(0, aFilename.LastIndexOf('.'));
                suffix = filename.Substring(filename.LastIndexOf('.'));
				return suffix.ToLower();
                // End TT#1913
			}
			catch
			{
				throw;
			}
		}
	}
}
